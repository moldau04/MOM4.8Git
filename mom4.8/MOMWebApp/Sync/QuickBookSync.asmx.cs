using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Web;
using System.Web.Services;
using System.IO;
using System.Security.Cryptography;
using Microsoft.Win32;
using System.Xml;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using BusinessLayer;
using BusinessEntity;
using System.Threading;

namespace MOMWebApp.Sync
{

    //
    // QuickBooks Web Connector Sample: WCWebService
    // Copyright (c) 2006-2012 Intuit, Inc
    //
    // This sample is a C# ASP.NET web service application that 
    // communicates with QuickBooks via QBWebConnector. The sample focuses 
    // primarily on demonstrating how to setup all web service web methods 
    // to run against QBWebConnector and does not focus on any particular 
    // use case. For simplicity, it sends three request XMLs: 
    // CustomerQuery, InvoiceQuery and BillQuery. 
    //
    // This sample assumes that you have configured IIS with ASP.NET and 
    // have a functional system to deploy this web service sample. If you have 
    // not yet configured ASP.NET with IIS, you may need to run the 
    // following command from c:\windows\Microsoft.NET\Framework\
    // your_asp_dot_net_version path: -
    // aspnet_regiis /i 
    // This will help avoid the occasional message from microsoft development 
    // environment such as "VS.NET has detected that the specified web server 
    // is not running ASP.NET version 1.1. You will be unable to run ASP.NET 
    // web applications or services)". 

    /*
     * Useful note about using OwnerID and FileID in a real-world application
     *  
     * As part of your QB Web Connector configuration (.QWC) file, you include
     * OwnerID and FileID. Following note on these two parameters may be useful. 
     * 
     * OwnerID -- this is a GUID that represents your application or suite of 
     * applications, if your application needs to store private data in the 
     * company file for one reason or another (one of the most common cases 
     * being to check if you have communicated with this company file before, 
     * and possibly some data about that communication) that private data will 
     * be visible to any application that knows the OwnerID.
     * 
     * FileID -- this is a GUID we stamp in the file on your behalf 
     * (using your OwnerID) as a private data extension to the "Company" object. 
     * It allows an application to verify that the company file it is exchanging 
     * data with is consistent over time (by doing a CompanyQuery with the field 
     * set appropriately and reading the DataExtRet values returned.
     * 
     * */



    /// <summary>
    /// Web Service Namespace="http://developer.intuit.com/"
    /// Web Service Name="WCWebService"
    /// Web Service Description="Sample WebService in ASP.NET to 
    /// demonstrate QuickBooks WebConnector"
    /// </summary>
    [WebService(
         Namespace = "http://developer.intuit.com/",
         Name = "WCWebService",
         Description = "Sample WebService in ASP.NET to demonstrate " +
                "QuickBooks WebConnector")]

    // Important Note: 	
    // You should keep the namespace as http://developer.intuit.com/ for all web 
    // services that communicates with QuickBooks Web Connector. 

    public class QuickBookSync : System.Web.Services.WebService
    {
        public static string WCWBFilepath = System.Web.Configuration.WebConfigurationManager.AppSettings["WebConnectorPath"].Trim(); //"F:\NK\QuickBooks Softwares\Company Files\Ideavate.QBW";
        public static string Connection = System.Web.Configuration.WebConfigurationManager.AppSettings["WebConnectorDatabase"].Trim();//"qbtest";
        public static string qbxmlversion = System.Web.Configuration.WebConfigurationManager.AppSettings["qbxmlv"].Trim();
        public static string strImportInvoice = System.Web.Configuration.WebConfigurationManager.AppSettings["ImportInvoice"].Trim();
        public static string strTransferInvoice = System.Web.Configuration.WebConfigurationManager.AppSettings["TransferInvoice"].Trim();
        public static string strQBWCSyncDirection = GetAppSettingsValueByKey("QBWCSyncDirection");
        BL_User objBL_User = new BL_User();
        BusinessEntity.User objProp_User = new BusinessEntity.User();
        GeneralFunctions objGenFunctions = new GeneralFunctions();
        BL_General objBL_General = new BL_General();
        General objGeneral = new General();
        BL_Contracts objBL_Contracts = new BL_Contracts();
        Contracts objProp_Contracts = new Contracts();
        BL_MapData objBL_MapData = new BL_MapData();
        MapData objMapData = new MapData();

        #region GlobalVariables
        System.Diagnostics.EventLog evLog = new System.Diagnostics.EventLog();
        public int count = 0;
        public ArrayList req = new ArrayList();
        #endregion

        #region Constructor
        public QuickBookSync()
        {
            //CODEGEN: This call is required by the ASP.NET 
            //Web Services Designer
            InitializeComponent();
            // Initializing EventLog for logging
            initEvLog();
        }
        #endregion

        #region AutoGeneratedMethods
        //Required by the Web Services Designer 
        private IContainer components = null;

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #endregion

        #region WebMethods
        [WebMethod]
        /// <summary>
        /// WebMethod - getInteractiveURL()
        /// 
        /// Signature: public string getInteractiveURL(string wcTicket, string sessionID)
        ///
        /// IN: 
        /// string wcTicket
        /// string sessionID
        ///
        /// OUT: 
        /// URL string 
        /// Possible values: 
        /// URL to a website
        /// </summary>
        public string getInteractiveURL(string wcTicket, string sessionID)
        {
            return "";
        }

        [WebMethod]
        /// <summary>
        /// WebMethod - interactiveRejected()
        /// 
        /// Signature: public string interactiveRejected(string wcTicket, string reason)
        ///
        /// IN: 
        /// string wcTicket
        /// string reason
        ///
        /// OUT: 
        /// string 
        /// </summary>
        public string interactiveRejected(string wcTicket, string reason)
        {
            return "";
        }

        [WebMethod]
        /// <summary>
        /// WebMethod - interactiveDone()
        /// 
        /// Signature: public string interactiveDone(string wcTicket)
        ///
        /// IN: 
        /// string wcTicket
        ///
        /// OUT: 
        /// string 
        /// </summary>
        public string interactiveDone(string wcTicket)
        {
            return "";
        }

        [WebMethod(EnableSession = true)]
        /// <summary>
        /// WebMethod - serverVersion()
        /// To enable web service with its version number returned back to QBWC
        /// Signature: public string serverVersion()
        ///
        /// OUT: 
        /// string 
        /// Possible values: 
        /// Version string representing server version
        /// </summary>

        public string serverVersion()
        {
            Session.Clear();
            NullifySessions();
            Session.Abandon();

            string serverVersion = "2.0.0.1";
            string evLogTxt = "WebMethod: serverVersion() has been called " +
                "by QBWebconnector" + "\r\n\r\n";
            evLogTxt = evLogTxt + "No Parameters required.";
            evLogTxt = evLogTxt + "Returned: " + serverVersion;
            return serverVersion;
        }



        [WebMethod]
        /// <summary>
        /// WebMethod - clientVersion()
        /// To enable web service with QBWC version control
        /// Signature: public string clientVersion(string strVersion)
        ///
        /// IN: 
        /// string strVersion
        ///
        /// OUT: 
        /// string errorOrWarning
        /// Possible values: 
        /// string retVal
        /// - NULL or <emptyString> = QBWC will let the web service update
        /// - "E:<any text>" = popup ERROR dialog with <any text> 
        ///					- abort update and force download of new QBWC.
        /// - "W:<any text>" = popup WARNING dialog with <any text> 
        ///					- choice to user, continue update or not.
        /// </summary>
        public string clientVersion(string strVersion)
        {
            string evLogTxt = "WebMethod: clientVersion() has been called " +
                "by QBWebconnector" + "\r\n\r\n";
            evLogTxt = evLogTxt + "Parameters received:\r\n";
            evLogTxt = evLogTxt + "string strVersion = " + strVersion + "\r\n";
            evLogTxt = evLogTxt + "\r\n";

            string retVal = null;
            double recommendedVersion = 1.5;
            double supportedMinVersion = 1.0;
            double suppliedVersion = Convert.ToDouble(this.parseForVersion(strVersion));
            evLogTxt = evLogTxt + "QBWebConnector version = " + strVersion + "\r\n";
            evLogTxt = evLogTxt + "Recommended Version = " + recommendedVersion.ToString() + "\r\n";
            evLogTxt = evLogTxt + "Supported Minimum Version = " + supportedMinVersion.ToString() + "\r\n";
            evLogTxt = evLogTxt + "SuppliedVersion = " + suppliedVersion.ToString() + "\r\n";
            if (suppliedVersion < recommendedVersion)
            {
                retVal = "W:We recommend that you upgrade your QBWebConnector";
            }
            else if (suppliedVersion < supportedMinVersion)
            {
                retVal = "E:You need to upgrade your QBWebConnector";
            }
            evLogTxt = evLogTxt + "\r\n";
            evLogTxt = evLogTxt + "Return values: " + "\r\n";
            evLogTxt = evLogTxt + "string retVal = " + retVal;
            logEvent(evLogTxt);
            return retVal;
        }




        [WebMethod(EnableSession = true)]
        /// <summary>
        /// WebMethod - authenticate()
        /// To verify username and password for the web connector that is trying to connect
        /// Signature: public string[] authenticate(string strUserName, string strPassword)
        /// 
        /// IN: 
        /// string strUserName 
        /// string strPassword
        ///
        /// OUT: 
        /// string[] authReturn
        /// Possible values: 
        /// string[0] = ticket
        /// string[1]
        /// - empty string = use current company file
        /// - "none" = no further request/no further action required
        /// - "nvu" = not valid user
        /// - any other string value = use this company file
        /// </summary>
        public string[] authenticate(string strUserName, string strPassword)
        {
            Session.Clear();
            NullifySessions();
            Session.Abandon();

            string evLogTxt = "WebMethod: authenticate() has been called by QBWebconnector" + "\r\n\r\n";
            evLogTxt = evLogTxt + "Parameters received:\r\n";
            evLogTxt = evLogTxt + "string strUserName = " + strUserName + "\r\n";
            evLogTxt = evLogTxt + "string strPassword = " + strPassword + "\r\n";
            evLogTxt = evLogTxt + "\r\n";


            string[] authReturn = new string[2];
            // Code below uses a random GUID to use as session ticket
            // An example of a GUID is {85B41BEE-5CD9-427a-A61B-83964F1EB426}
            authReturn[0] = System.Guid.NewGuid().ToString();

            // For simplicity of sample, a hardcoded username/password is used.
            // In real world, you should handle authentication in using a standard way. 
            // For example, you could validate the username/password against an LDAP 
            // or a directory server
            string pwd = "password";
            evLogTxt = evLogTxt + "Password locally stored = " + pwd + "\r\n";
            if (strUserName.Trim().Equals("username") && strPassword.Trim().Equals(pwd))
            {
                // An empty string for authReturn[1] means asking QBWebConnector 
                // to connect to the company file that is currently openned in QB
                //authReturn[1] = @"F:\KUNAL\QuickBooks Softwares\Company Files\Elevator Refurbishing Corp.QBW";
                authReturn[1] = WCWBFilepath;

            }
            else
            {
                authReturn[1] = "nvu";
            }
            // You could also return "none" to indicate there is no work to do
            // or a company filename in the format C:\full\path\to\company.qbw
            // based on your program logic and requirements.

            evLogTxt = evLogTxt + "\r\n";
            evLogTxt = evLogTxt + "Return values: " + "\r\n";
            evLogTxt = evLogTxt + "string[] authReturn[0] = " + authReturn[0].ToString() + "\r\n";
            evLogTxt = evLogTxt + "string[] authReturn[1] = " + authReturn[1].ToString();
            logEvent(evLogTxt);
            return authReturn;
        }




        [WebMethod(Description = "This web method facilitates web service to handle connection error between QuickBooks and QBWebConnector", EnableSession = true)]
        /// <summary>
        /// WebMethod - connectionError()
        /// To facilitate capturing of QuickBooks error and notifying it to web services
        /// Signature: public string connectionError (string ticket, string hresult, string message)
        ///
        /// IN: 
        /// string ticket = A GUID based ticket string to maintain identity of QBWebConnector 
        /// string hresult = An HRESULT value thrown by QuickBooks when trying to make connection
        /// string message = An error message corresponding to the HRESULT
        ///
        /// OUT:
        /// string retVal
        /// Possible values: 
        /// - “done” = no further action required from QBWebConnector
        /// - any other string value = use this name for company file
        /// </summary>
        public string connectionError(string ticket, string hresult, string message)
        {
            if (Session["ce_counter"] == null)
            {
                Session["ce_counter"] = 0;
            }

            string evLogTxt = "WebMethod: connectionError() has been called by QBWebconnector" + "\r\n\r\n";
            evLogTxt = evLogTxt + "Parameters received:\r\n";
            evLogTxt = evLogTxt + "string ticket = " + ticket + "\r\n";
            evLogTxt = evLogTxt + "string hresult = " + hresult + "\r\n";
            evLogTxt = evLogTxt + "string message = " + message + "\r\n";
            evLogTxt = evLogTxt + "\r\n";

            string retVal = null;
            // 0x80040400 - QuickBooks found an error when parsing the provided XML text stream. 
            const string QB_ERROR_WHEN_PARSING = "0x80040400";
            // 0x80040401 - Could not access QuickBooks.  
            const string QB_COULDNT_ACCESS_QB = "0x80040401";
            // 0x80040402 - Unexpected error. Check the qbsdklog.txt file for possible, additional information. 
            const string QB_UNEXPECTED_ERROR = "0x80040402";
            // Add more as you need...

            if (hresult.Trim().Equals(QB_ERROR_WHEN_PARSING))
            {
                evLogTxt = evLogTxt + "HRESULT = " + hresult + "\r\n";
                evLogTxt = evLogTxt + "Message = " + message + "\r\n";
                retVal = "DONE";
            }
            else if (hresult.Trim().Equals(QB_COULDNT_ACCESS_QB))
            {
                evLogTxt = evLogTxt + "HRESULT = " + hresult + "\r\n";
                evLogTxt = evLogTxt + "Message = " + message + "\r\n";
                retVal = "DONE";
            }
            else if (hresult.Trim().Equals(QB_UNEXPECTED_ERROR))
            {
                evLogTxt = evLogTxt + "HRESULT = " + hresult + "\r\n";
                evLogTxt = evLogTxt + "Message = " + message + "\r\n";
                retVal = "DONE";
            }
            else
            {
                // Depending on various hresults return different value 
                if ((int)Session["ce_counter"] == 0)
                {
                    // Try again with this company file
                    evLogTxt = evLogTxt + "HRESULT = " + hresult + "\r\n";
                    evLogTxt = evLogTxt + "Message = " + message + "\r\n";
                    evLogTxt = evLogTxt + "Sending empty company file to try again.";
                    retVal = "";
                }
                else
                {
                    evLogTxt = evLogTxt + "HRESULT = " + hresult + "\r\n";
                    evLogTxt = evLogTxt + "Message = " + message + "\r\n";
                    evLogTxt = evLogTxt + "Sending DONE to stop.";
                    retVal = "DONE";
                }
            }
            evLogTxt = evLogTxt + "\r\n";
            evLogTxt = evLogTxt + "Return values: " + "\r\n";
            evLogTxt = evLogTxt + "string retVal = " + retVal + "\r\n";
            logEvent(evLogTxt);
            Session["ce_counter"] = ((int)Session["ce_counter"]) + 1;
            return retVal;
        }



        [WebMethod(Description = "This web method facilitates web service to send request XML to QuickBooks via QBWebConnector", EnableSession = true)]
        /// <summary>
        /// WebMethod - sendRequestXML()
        /// Signature: public string sendRequestXML(string ticket, string strHCPResponse, string strCompanyFileName, 
        /// string Country, int qbXMLMajorVers, int qbXMLMinorVers)
        /// 
        /// IN: 
        /// int qbXMLMajorVers
        /// int qbXMLMinorVers
        /// string ticket
        /// string strHCPResponse 
        /// string strCompanyFileName 
        /// string Country
        /// int qbXMLMajorVers
        /// int qbXMLMinorVers
        ///
        /// OUT:
        /// string request
        /// Possible values: 
        /// - “any_string” = Request XML for QBWebConnector to process
        /// - "" = No more request XML 
        /// </summary>
        public string sendRequestXML(string ticket, string strHCPResponse, string strCompanyFileName,
            string qbXMLCountry, int qbXMLMajorVers, int qbXMLMinorVers)
        {

            //if (Path.GetFileName(strCompanyFileName).ToUpper() != Path.GetFileName(WCWBFilepath).ToUpper())
            if (strCompanyFileName.ToUpper() != WCWBFilepath.ToUpper())
            {
                throw new System.Exception("Company file name " + WCWBFilepath + " does not match" + strCompanyFileName);
                //throw new System.Exception("Company file name " + Path.GetFileName(WCWBFilepath) + " does not match" + Path.GetFileName(strCompanyFileName));
            }

            if (Session["counter"] == null)
            {
                Session["counter"] = 0;
            }
            string evLogTxt = "WebMethod: sendRequestXML() has been called by QBWebconnector" + "\r\n\r\n";
            evLogTxt = evLogTxt + "Parameters received:\r\n";
            evLogTxt = evLogTxt + "string ticket = " + ticket + "\r\n";
            evLogTxt = evLogTxt + "string strHCPResponse = " + strHCPResponse + "\r\n";
            evLogTxt = evLogTxt + "string strCompanyFileName = " + strCompanyFileName + "\r\n";
            evLogTxt = evLogTxt + "string qbXMLCountry = " + qbXMLCountry + "\r\n";
            evLogTxt = evLogTxt + "int qbXMLMajorVers = " + qbXMLMajorVers.ToString() + "\r\n";
            evLogTxt = evLogTxt + "int qbXMLMinorVers = " + qbXMLMinorVers.ToString() + "\r\n";
            evLogTxt = evLogTxt + "\r\n";

            ArrayList req = buildRequest();
            string request = "";
            int total = req.Count;
            count = Convert.ToInt32(Session["counter"]);

            if (count < total)
            {
                request = req[count].ToString();
                evLogTxt = evLogTxt + "sending request no = " + (count + 1) + "\r\n";
                Session["counter"] = ((int)Session["counter"]) + 1;
            }
            else
            {
                count = 0;
                Session["counter"] = 0;
                request = "";
            }
            evLogTxt = evLogTxt + "\r\n";
            evLogTxt = evLogTxt + "Return values: " + "\r\n";
            evLogTxt = evLogTxt + "string request = " + request + "\r\n";
            logEvent(evLogTxt);

            return request;

        }



        [WebMethod(Description = "This web method facilitates web service to receive response XML from QuickBooks via QBWebConnector", EnableSession = true)]
        /// <summary>
        /// WebMethod - receiveResponseXML()
        /// Signature: public int receiveResponseXML(string ticket, string response, string hresult, string message)
        /// 
        /// IN: 
        /// string ticket
        /// string response
        /// string hresult
        /// string message
        ///
        /// OUT: 
        /// int retVal
        /// Greater than zero  = There are more request to send
        /// 100 = Done. no more request to send
        /// Less than zero  = Custom Error codes
        /// </summary>
        public int receiveResponseXML(string ticket, string response, string hresult, string message)
        {
            #region Table Schema
            DataTable dt = new DataTable();
            dt.Columns.Add("ListID", typeof(string));
            dt.Columns.Add("CustomerName", typeof(string));
            dt.Columns.Add("Remarks", typeof(string));
            dt.Columns.Add("MainContact", typeof(string));
            dt.Columns.Add("Phone", typeof(string));
            dt.Columns.Add("Email", typeof(string));
            dt.Columns.Add("Cell", typeof(string));
            dt.Columns.Add("Address", typeof(string));
            dt.Columns.Add("City", typeof(string));
            dt.Columns.Add("State", typeof(string));
            dt.Columns.Add("Zip", typeof(string));
            dt.Columns.Add("IsJob", typeof(string));
            dt.Columns.Add("Fax", typeof(string));
            dt.Columns.Add("ParentCustID", typeof(string));
            dt.Columns.Add("BillAddress", typeof(string));
            dt.Columns.Add("BillCity", typeof(string));
            dt.Columns.Add("BillState", typeof(string));
            dt.Columns.Add("BillZip", typeof(string));
            dt.Columns.Add("LastUpdateDate", typeof(DateTime));
            dt.Columns.Add("Type", typeof(string));
            dt.Columns.Add("LocType", typeof(string));
            dt.Columns.Add("Status", typeof(bool));
            dt.Columns.Add("QBStaxID", typeof(string));
            dt.Columns.Add("Balance", typeof(double));

            #endregion

            # region Parse response XML
            XDocument xdoc = new XDocument();
            try
            {
                xdoc = XDocument.Parse(response);
                try
                {
                    foreach (XElement Queryitem in xdoc.Descendants("TxnDeletedQueryRs"))
                    {
                        foreach (XElement item in Queryitem.Descendants("TxnDeletedRet"))
                        {
                            string deltype = item.Element("TxnDelType").Value;
                            string listid = item.Element("TxnID").Value;
                            if (!string.IsNullOrEmpty(listid))
                            {
                                if (deltype == "Invoice")
                                {
                                    objProp_Contracts.ConnConfig = objGenFunctions.ConnectionStr(Connection);
                                    objProp_Contracts.QBInvID = listid;
                                    objBL_Contracts.DeleteInvoiceByListID(objProp_Contracts);
                                }
                            }
                        }
                    }

                    foreach (XElement Queryitem in xdoc.Descendants("ListDeletedQueryRs"))
                    {
                        foreach (XElement item in Queryitem.Descendants("ListDeletedRet"))
                        {
                            string deltype = item.Element("ListDelType").Value;
                            string listid = item.Element("ListID").Value;
                            if (!string.IsNullOrEmpty(listid))
                            {
                                objProp_User.ConnConfig = objGenFunctions.ConnectionStr(Connection);

                                if (deltype == "Employee")
                                {
                                    objProp_User.QBEmployeeID = listid;
                                    objBL_User.DeleteEmployeeByListID(objProp_User);
                                }
                                if (deltype == "Customer")
                                {
                                    objProp_User.QBlocationID = listid;
                                    objBL_User.DeleteLocationByListID(objProp_User);

                                    objProp_User.QBCustomerID = listid;
                                    objBL_User.DeleteCustomerByListID(objProp_User);
                                }
                                if (deltype == "CustomerType")
                                {
                                    objProp_User.QBCustomerTypeID = listid;
                                    objBL_User.DeleteCustomerTypeByListID(objProp_User);
                                }
                                if (deltype == "JobType")
                                {
                                    objProp_User.QBJobtypeID = listid;
                                    objBL_User.DeleteLocTypeByListID(objProp_User);
                                }
                                if (deltype == "ItemDiscount" || deltype == "ItemFixedAsset" || deltype == "ItemGroup" || deltype == "ItemInventory" || deltype == "ItemInventoryAssembly" || deltype == "ItemNonInventory" || deltype == "ItemOtherCharge" || deltype == "ItemPayment" || deltype == "ItemSalesTax" || deltype == "ItemSalesTaxGroup" || deltype == "ItemService" || deltype == "ItemSubtotal")
                                {
                                    objProp_User.QBInvID = listid;
                                    new BusinessLayer.Billing.BL_BillCodes().DeleteBillingCodebyListID(objProp_User);
                                }
                                if (deltype == "ItemSalesTax")
                                {
                                    objProp_User.QBSalesTaxID = listid;
                                    objBL_User.DeleteSalesTaxByListID(objProp_User);
                                }
                                if (deltype == "Class")
                                {
                                    objProp_User.QBJobtypeID = listid;
                                    objBL_User.DeleteDepartmentByListID(objProp_User);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    SaveErrorCodes("Deleting MOM's data", "", "", "Error", ex.Message);
                }

                try
                {
                    #region Customer Type
                    foreach (XElement Queryitem in xdoc.Descendants("CustomerTypeQueryRs"))
                    {
                        foreach (XElement item in Queryitem.Descendants("CustomerTypeRet"))
                        {
                            objProp_User.QBCustomerTypeID = item.Element("ListID").Value;
                            objProp_User.CustomerType = objGenFunctions.QBEncode(item.Element("Name").Value, 50);
                            objProp_User.Remarks = item.Element("FullName").Value;
                            objProp_User.ConnConfig = objGenFunctions.ConnectionStr(Connection);

                            objBL_User.AddQBCustomerType(objProp_User);
                        }
                    }
                    #endregion

                    #region Import Job/Location Type
                    foreach (XElement Queryitem in xdoc.Descendants("JobTypeQueryRs"))
                    {
                        foreach (XElement item in Queryitem.Descendants("JobTypeRet"))
                        {
                            objProp_User.QBCustomerTypeID = item.Element("ListID").Value;
                            objProp_User.CustomerType = objGenFunctions.QBEncode(item.Element("Name").Value, 50);
                            objProp_User.Remarks = item.Element("FullName").Value;
                            objProp_User.ConnConfig = objGenFunctions.ConnectionStr(Connection);

                            objBL_User.AddQBLocType(objProp_User);
                        }
                    }
                    #endregion

                    #region Import Sales Tax
                    foreach (XElement Queryitem in xdoc.Descendants("ItemSalesTaxQueryRs"))
                    //foreach (XElement Queryitem in xdoc.Descendants("SalesTaxCodeQueryRs"))
                    {
                        foreach (XElement item in Queryitem.Descendants("ItemSalesTaxRet"))
                        //foreach (XElement item in Queryitem.Descendants("SalesTaxCodeRet"))
                        {
                            objProp_User.QBSalesTaxID = item.Element("ListID").Value;
                            objProp_User.SalesTax = objGenFunctions.QBEncode(item.Element("Name").Value, 25);

                            if (item.Element("ItemDesc") != null)
                                objProp_User.SalesDescription = objGenFunctions.QBEncode(item.Element("ItemDesc").Value, 75);
                            else
                                objProp_User.SalesDescription = "";

                            if (item.Element("TaxRate") != null)
                                objProp_User.SalesRate = Convert.ToDouble(item.Element("TaxRate").Value);
                            else
                                objProp_User.SalesRate = 0;

                            objProp_User.State = "";
                            objProp_User.Remarks = "";
                            //objProp_User.IsTaxable = Convert.ToInt32(Convert.ToBoolean(item.Element("IsTaxable").Value));
                            objProp_User.IsTaxable = 0;
                            objProp_User.LastUpdateDate = Convert.ToDateTime(item.Element("TimeModified").Value);
                            objProp_User.ConnConfig = objGenFunctions.ConnectionStr(Connection);

                            objBL_User.AddQBSalesTax(objProp_User);
                        }
                    }
                    #endregion

                    #region Import Customer/Jobs/Location
                    foreach (XElement Queryitem in xdoc.Descendants("CustomerQueryRs"))
                    {
                        var statusCode = Queryitem.Attribute("statusCode").Value;
                        if (statusCode == "0")
                        {
                            List<string> CustomerIteratorIDs = new List<string>();
                            if (Session["CustomerIteratorIDs"] != null)
                            {
                                CustomerIteratorIDs = (List<string>)Session["CustomerIteratorIDs"];
                            }

                            var iteratorRemainingCount = Queryitem.Attribute("iteratorRemainingCount").Value;
                            if (iteratorRemainingCount != "0")
                            {
                                var iteratorID = Queryitem.Attribute("iteratorID").Value;
                                CustomerIteratorIDs.Add(iteratorID);
                                Session["CustomerIteratorIDs"] = CustomerIteratorIDs;
                            }

                            if (Queryitem.Attribute("requestID").Value == "1")
                            {
                                //if (Session["CustomerRet_1"] == null)
                                //{
                                foreach (XElement item in Queryitem.Descendants("CustomerRet"))
                                {
                                    //Session["CustomerRet_1"] = 1;
                                    DataRow dr = dt.NewRow();

                                    #region variable declaration

                                    string IsActive = string.Empty;
                                    string TimeModified = string.Empty;
                                    string FullName = string.Empty;
                                    string Notes = string.Empty;
                                    string Contact = string.Empty;
                                    string Phone = string.Empty;
                                    string Email = string.Empty;
                                    string Mobile = string.Empty;
                                    string Listid = string.Empty;
                                    string Sublevel = string.Empty;
                                    string Fax = string.Empty;
                                    string CustType = string.Empty;
                                    string LocType = string.Empty;
                                    string ParentCustID = string.Empty;
                                    string BillAdd1 = string.Empty;
                                    string BillAdd2 = string.Empty;
                                    string BillAdd3 = string.Empty;
                                    string BillAdd4 = string.Empty;
                                    string BillAdd5 = string.Empty;
                                    string BillCity = string.Empty;
                                    string BillState = string.Empty;
                                    string BillZip = string.Empty;
                                    string BillNotes = string.Empty;
                                    string ShipAdd1 = string.Empty;
                                    string ShipAdd2 = string.Empty;
                                    string ShipAdd3 = string.Empty;
                                    string ShipAdd4 = string.Empty;
                                    string ShipAdd5 = string.Empty;
                                    string ShipCity = string.Empty;
                                    string ShipState = string.Empty;
                                    string ShipZip = string.Empty;
                                    string ShipNotes = string.Empty;
                                    string FirstName = string.Empty;
                                    string LastName = string.Empty;
                                    string CompanyName = string.Empty;
                                    string MiddleName = string.Empty;
                                    string QBStaxID = string.Empty;
                                    string Balance = string.Empty;
                                    #endregion

                                    #region Assign values from XML to variables

                                    if (item.Element("IsActive") != null)
                                        IsActive = item.Element("IsActive").Value;

                                    if (item.Element("TimeModified") != null)
                                        TimeModified = item.Element("TimeModified").Value;

                                    //if (item.Element("FullName") != null)
                                    //    FullName = item.Element("FullName").Value;

                                    if (item.Element("Name") != null)
                                        FullName = item.Element("Name").Value;

                                    if (item.Element("CompanyName") != null)
                                        CompanyName = item.Element("CompanyName").Value;

                                    if (FullName == string.Empty)
                                    {
                                        FullName = CompanyName;
                                    }

                                    //if (item.Element("Notes") != null)
                                    //    Notes = item.Element("Notes").Value;

                                    //if (item.Element("Contact") != null)
                                    //    Contact = item.Element("Contact").Value;

                                    if (item.Element("FirstName") != null)
                                        FirstName = item.Element("FirstName").Value;

                                    if (item.Element("MiddleName") != null)
                                        MiddleName = item.Element("MiddleName").Value;

                                    if (item.Element("LastName") != null)
                                        LastName = item.Element("LastName").Value;

                                    if (MiddleName.Trim() == string.Empty)
                                    {
                                        Contact = FirstName.Trim() + " " + LastName.Trim();
                                    }
                                    else
                                    {
                                        Contact = FirstName.Trim() + " " + MiddleName.Trim() + " " + LastName.Trim();
                                    }

                                    if (item.Element("Phone") != null)
                                        Phone = item.Element("Phone").Value;

                                    if (item.Element("Email") != null)
                                        Email = item.Element("Email").Value;

                                    //if (item.Element("Mobile") != null)
                                    //    Mobile = item.Element("Mobile").Value;

                                    if (item.Element("ListID") != null)
                                        Listid = item.Element("ListID").Value;

                                    if (item.Element("Sublevel") != null)
                                        Sublevel = item.Element("Sublevel").Value;

                                    if (item.Element("Fax") != null)
                                        Fax = item.Element("Fax").Value;

                                    if (item.Element("Balance") != null)
                                    {
                                        if (item.Element("Balance").Value != string.Empty)
                                            Balance = item.Element("Balance").Value;
                                        else
                                            Balance = "0";
                                    }

                                    if (item.Element("CustomerTypeRef") != null)
                                    {
                                        if (item.Element("CustomerTypeRef").Element("FullName") != null)
                                            CustType = item.Element("CustomerTypeRef").Element("FullName").Value;
                                    }

                                    if (item.Element("JobTypeRef") != null)
                                    {
                                        if (item.Element("JobTypeRef").Element("FullName") != null)
                                            LocType = item.Element("JobTypeRef").Element("FullName").Value;
                                    }

                                    if (item.Element("ParentRef") != null)
                                    {
                                        if (item.Element("ParentRef").Element("ListID") != null)
                                            ParentCustID = item.Element("ParentRef").Element("ListID").Value;
                                    }

                                    if (item.Element("ItemSalesTaxRef") != null)
                                    {
                                        if (item.Element("ItemSalesTaxRef").Element("ListID") != null)
                                            QBStaxID = item.Element("ItemSalesTaxRef").Element("ListID").Value;
                                    }

                                    if (item.Element("BillAddress") != null)
                                    {
                                        if (item.Element("BillAddress").Element("Addr1") != null)
                                            BillAdd1 = item.Element("BillAddress").Element("Addr1").Value;

                                        if (item.Element("BillAddress").Element("Addr2") != null)
                                            BillAdd2 = item.Element("BillAddress").Element("Addr2").Value;

                                        if (item.Element("BillAddress").Element("Addr3") != null)
                                            BillAdd3 = item.Element("BillAddress").Element("Addr3").Value;

                                        //if (item.Element("BillAddress").Element("Addr4") != null)
                                        //    BillAdd4 = item.Element("BillAddress").Element("Addr4").Value;

                                        //if (item.Element("BillAddress").Element("Addr5") != null)
                                        //    BillAdd5 = item.Element("BillAddress").Element("Addr5").Value;

                                        if (item.Element("BillAddress").Element("City") != null)
                                            BillCity = item.Element("BillAddress").Element("City").Value;

                                        if (item.Element("BillAddress").Element("State") != null)
                                            BillState = item.Element("BillAddress").Element("State").Value;

                                        if (item.Element("BillAddress").Element("PostalCode") != null)
                                            BillZip = item.Element("BillAddress").Element("PostalCode").Value;

                                        //if (item.Element("BillAddress").Element("Note") != null)
                                        //    BillNotes = item.Element("BillAddress").Element("Note").Value;
                                    }
                                    string BillAddress = SuffixSpace(BillAdd1) + Environment.NewLine + SuffixSpace(BillAdd2) + Environment.NewLine + SuffixSpace(BillAdd3);// +SuffixSpace(BillAdd4) + SuffixSpace(BillAdd5);

                                    if (item.Element("ShipAddress") != null)
                                    {
                                        if (item.Element("ShipAddress").Element("Addr1") != null)
                                            ShipAdd1 = item.Element("ShipAddress").Element("Addr1").Value;

                                        if (item.Element("ShipAddress").Element("Addr2") != null)
                                            ShipAdd2 = item.Element("ShipAddress").Element("Addr2").Value;

                                        if (item.Element("ShipAddress").Element("Addr3") != null)
                                            ShipAdd3 = item.Element("ShipAddress").Element("Addr3").Value;

                                        //if (item.Element("ShipAddress").Element("Addr4") != null)
                                        //    ShipAdd4 = item.Element("ShipAddress").Element("Addr4").Value;

                                        //if (item.Element("ShipAddress").Element("Addr5") != null)
                                        //    ShipAdd5 = item.Element("ShipAddress").Element("Addr5").Value;

                                        if (item.Element("ShipAddress").Element("City") != null)
                                            ShipCity = item.Element("ShipAddress").Element("City").Value;

                                        if (item.Element("ShipAddress").Element("State") != null)
                                            ShipState = item.Element("ShipAddress").Element("State").Value;

                                        if (item.Element("ShipAddress").Element("PostalCode") != null)
                                            ShipZip = item.Element("ShipAddress").Element("PostalCode").Value;

                                        //if (item.Element("ShipAddress").Element("Note") != null)
                                        //    ShipNotes = item.Element("ShipAddress").Element("Note").Value;
                                    }
                                    string ShipAddress = SuffixSpace(ShipAdd1) + Environment.NewLine + SuffixSpace(ShipAdd2) + Environment.NewLine + SuffixSpace(ShipAdd3);// +SuffixSpace(ShipAdd4) + SuffixSpace(ShipAdd5);



                                    #endregion

                                    #region Fill Datatable

                                    if (Convert.ToBoolean(IsActive) == true)
                                        dr["Status"] = false;
                                    else
                                        dr["Status"] = true;

                                    dr["LastUpdateDate"] = TimeModified;
                                    dr["CustomerName"] = FullName;
                                    dr["Remarks"] = Notes;
                                    dr["MainContact"] = Contact;
                                    dr["Phone"] = Phone;
                                    dr["Email"] = Email;
                                    dr["Cell"] = Mobile;
                                    dr["ListID"] = Listid;
                                    dr["IsJob"] = Sublevel;
                                    dr["Fax"] = Fax;
                                    dr["type"] = CustType;
                                    dr["loctype"] = LocType;
                                    dr["ParentCustID"] = ParentCustID;
                                    dr["BillAddress"] = BillAddress.Trim();
                                    dr["BillCity"] = BillCity.Trim();
                                    dr["BillState"] = BillState.Trim();
                                    dr["BillZip"] = BillZip.Trim();
                                    dr["Remarks"] = BillNotes.Trim();
                                    dr["Address"] = ShipAddress.Trim();
                                    dr["City"] = ShipCity.Trim();
                                    dr["State"] = ShipState.Trim();
                                    dr["Zip"] = ShipZip.Trim();
                                    dr["QBStaxID"] = QBStaxID;
                                    dr["Balance"] = Balance;

                                    if (ShipNotes.Trim() != string.Empty)
                                    {
                                        dr["Remarks"] = ShipNotes.Trim();
                                    }

                                    if (dr["Address"].ToString().Trim() == string.Empty)
                                    {
                                        dr["Address"] = dr["BillAddress"];
                                        dr["City"] = dr["BillCity"];
                                        dr["State"] = dr["BillState"];
                                        dr["Zip"] = dr["BillZip"];
                                    }

                                    if (dr["Address"].ToString().Trim() == string.Empty)
                                    {
                                        dr["Address"] = dr["CustomerName"].ToString();
                                    }

                                    if (item.Element("ParentRef") != null)
                                    {
                                        if (item.Element("ParentRef").Element("ListID") != null)
                                            dr["ParentCustID"] = item.Element("ParentRef").Element("ListID").Value;
                                    }

                                    dt.Rows.Add(dr);

                                    #endregion
                                }
                                //}
                            }
                        }
                    }

                    if (dt.Rows.Count > 0)
                    {
                        #region Import Customers from QB
                        var queryCust = from row in dt.AsEnumerable()
                                    where row.Field<string>("IsJob").Equals("0")
                                    select row;

                        DataTable dtnewCust = dt.Clone();
                        foreach (var record in queryCust)
                        {
                            DataRow drRow = dtnewCust.NewRow();
                            drRow = record;

                            dtnewCust.ImportRow(drRow);
                        }

                        // Insert customers to MOM
                        foreach (DataRow dr in dtnewCust.Rows)
                        {
                            objProp_User.FirstName = dr["CustomerName"].ToString();
                            objProp_User.Remarks = dr["Remarks"].ToString();
                            objProp_User.MainContact = dr["MainContact"].ToString();
                            objProp_User.Phone = dr["Phone"].ToString();
                            objProp_User.Email = dr["Email"].ToString();
                            objProp_User.Cell = dr["Cell"].ToString();
                            objProp_User.QBCustomerID = dr["ListID"].ToString();
                            //dr["BillAddress"] = BillAddress.Trim();
                            //dr["BillCity"] = BillCity.Trim();
                            //dr["BillState"] = BillState.Trim();
                            //dr["BillZip"] = BillZip.Trim();
                            objProp_User.Address = dr["BillAddress"].ToString();
                            objProp_User.City = dr["BillCity"].ToString();
                            objProp_User.State = dr["BillState"].ToString();
                            objProp_User.Zip = dr["BillZip"].ToString();
                            objProp_User.LastUpdateDate = Convert.ToDateTime(dr["LastUpdateDate"]);
                            objProp_User.Status = Convert.ToInt16(dr["Status"]);
                            objProp_User.Balance = Convert.ToDouble(dr["balance"].ToString());

                            objProp_User.Username = "";
                            objProp_User.Password = "";
                            objProp_User.Website = "";
                            objProp_User.Type = dr["type"].ToString();
                            objProp_User.Schedule = 0;
                            objProp_User.Mapping = 0;
                            objProp_User.Internet = 0;
                            objProp_User.ConnConfig = objGenFunctions.ConnectionStr(Connection);

                            objBL_User.AddUpdateCustomerQB(objProp_User);
                        }
                        #endregion

                        #region Import Locations from QB
                        var queryLocation = from row in dt.AsEnumerable()
                                            where row.Field<string>("IsJob").Equals("1")
                                            select row;

                        DataTable dtnewLoc = dt.Clone();
                        foreach (var record in queryLocation)
                        {
                            DataRow drRow = dtnewLoc.NewRow();
                            drRow = record;

                            dtnewLoc.ImportRow(drRow);
                        }

                        DataTable dtLocFromCust = dtnewLoc.Clone();
                        foreach (DataRow drCust in dtnewCust.Rows)
                        {
                            int isHavingJob = 0;
                            foreach (DataRow drLoc in dtnewLoc.Rows)
                            {
                                if (drCust["ListID"].ToString() == drLoc["ParentCustID"].ToString())
                                {
                                    isHavingJob = 1;
                                    break;
                                }
                            }
                            if (isHavingJob == 0)
                            {
                                drCust["ParentCustID"] = drCust["listid"];
                                dtLocFromCust.ImportRow(drCust);
                            }
                        }

                        foreach (DataRow dr in dtLocFromCust.Rows)
                        {
                            dtnewLoc.ImportRow(dr);
                        }

                        foreach (DataRow dr in dtnewLoc.Rows)
                        {
                            objProp_User.AccountNo = dr["CustomerName"].ToString();
                            objProp_User.Locationname = dr["CustomerName"].ToString();
                            objProp_User.Address = dr["Address"].ToString();
                            objProp_User.Status = Convert.ToInt16(dr["Status"]);
                            objProp_User.City = dr["City"].ToString();
                            objProp_User.State = dr["State"].ToString();
                            objProp_User.Zip = dr["Zip"].ToString();
                            objProp_User.Remarks = dr["Remarks"].ToString();
                            objProp_User.MainContact = dr["MainContact"].ToString();
                            objProp_User.Phone = dr["Phone"].ToString();
                            objProp_User.Fax = dr["Fax"].ToString();
                            objProp_User.Cell = dr["Cell"].ToString();
                            objProp_User.Email = dr["Email"].ToString();
                            objProp_User.RolAddress = dr["BillAddress"].ToString();
                            objProp_User.RolCity = dr["BillCity"].ToString();
                            objProp_User.RolState = dr["BillState"].ToString();
                            objProp_User.RolZip = dr["BillZip"].ToString();
                            objProp_User.QBlocationID = dr["ListID"].ToString();
                            objProp_User.QBCustomerID = dr["ParentCustID"].ToString();
                            objProp_User.LastUpdateDate = Convert.ToDateTime(dr["LastUpdateDate"]);
                            objProp_User.Type = dr["loctype"].ToString();
                            objProp_User.Stax = dr["QBStaxID"].ToString().Trim();
                            objProp_User.Balance = Convert.ToDouble(dr["balance"].ToString());
                            objProp_User.ConnConfig = objGenFunctions.ConnectionStr(Connection);
                            objBL_User.AddUpdateQBLocation(objProp_User);
                        }
                        #endregion
                    }
                    #endregion

                    #region Import Class/Department
                    foreach (XElement Queryitem in xdoc.Descendants("ClassQueryRs"))
                    {
                        if (Queryitem.Attribute("requestID").Value == "1")
                        {
                            //if (Session["ClassRet_1"] == null)
                            //{
                            foreach (XElement item in Queryitem.Descendants("ClassRet"))
                            {
                                Session["ClassRet_1"] = 1;
                                objProp_User.QBJobtypeID = item.Element("ListID").Value;
                                objProp_User.Type = objGenFunctions.QBEncode(item.Element("Name").Value, 50);
                                objProp_User.Remarks = item.Element("FullName").Value;
                                objProp_User.ConnConfig = objGenFunctions.ConnectionStr(Connection);
                                objProp_User.LastUpdateDate = Convert.ToDateTime(item.Element("TimeModified").Value);

                                objBL_User.AddQBDepartment(objProp_User);
                            }
                            //}
                        }
                    }
                    #endregion

                    #region Import ServiceItem/billcode
                    foreach (XElement Queryitem in xdoc.Descendants("ItemQueryRs"))
                    {
                        List<string> ItemIteratorIDs = new List<string>();
                        if (Session["ItemIteratorIDs"] != null)
                        {
                            ItemIteratorIDs = (List<string>)Session["ItemIteratorIDs"];
                        }

                        var iteratorRemainingCount = Queryitem.Attribute("iteratorRemainingCount").Value;
                        if (iteratorRemainingCount != "0")
                        {
                            var iteratorID = Queryitem.Attribute("iteratorID").Value;
                            ItemIteratorIDs.Add(iteratorID);
                            Session["ItemIteratorIDs"] = ItemIteratorIDs;
                        }

                        foreach (XElement item in Queryitem.Descendants("ItemServiceRet"))
                        {
                            objProp_User.QBInvID = item.Element("ListID").Value;
                            objProp_User.ConnConfig = objGenFunctions.ConnectionStr(Connection);
                            objProp_User.ContactName = objGenFunctions.QBEncode(item.Element("Name").Value, 30);
                            objProp_User.SalesDescription = item.Element("FullName").Value;
                            objProp_User.CatStatus = Status(item.Element("IsActive").Value);
                            double bal = 0;
                            string QBAccountID = string.Empty;
                            if (item.Element("SalesOrPurchase") != null)
                            {
                                if (item.Element("SalesOrPurchase").Element("Price") != null)
                                    bal = Convert.ToDouble(item.Element("SalesOrPurchase").Element("Price").Value);

                                if (item.Element("AccountRef") != null)
                                    QBAccountID = item.Element("AccountRef").Element("ListID").Value;
                            }
                            objProp_User.QBAccountID = QBAccountID;
                            objProp_User.Balance = bal;
                            objProp_User.Measure = "";
                            objProp_User.Remarks = "";
                            objProp_User.Type = "1";
                            objProp_User.LastUpdateDate = Convert.ToDateTime(item.Element("TimeModified").Value);
                            objBL_User.AddQBBillCode(objProp_User);
                        }
                        foreach (XElement item in Queryitem.Descendants("ItemNonInventoryRet"))
                        {
                            objProp_User.QBInvID = item.Element("ListID").Value;
                            objProp_User.ConnConfig = objGenFunctions.ConnectionStr(Connection);
                            objProp_User.ContactName = objGenFunctions.QBEncode(item.Element("Name").Value, 30);
                            objProp_User.SalesDescription = item.Element("FullName").Value;
                            objProp_User.CatStatus = Status(item.Element("IsActive").Value);
                            objProp_User.Balance = 0.00;
                            objProp_User.Measure = "";
                            objProp_User.Remarks = "";
                            objProp_User.Type = "0";
                            objProp_User.QBAccountID = "";
                            objProp_User.LastUpdateDate = Convert.ToDateTime(item.Element("TimeModified").Value);

                            objBL_User.AddQBBillCode(objProp_User);
                        }

                        foreach (XElement item in Queryitem.Descendants("ItemInventoryRet"))
                        {
                            objProp_User.QBInvID = item.Element("ListID").Value;
                            objProp_User.ConnConfig = objGenFunctions.ConnectionStr(Connection);
                            objProp_User.ContactName = objGenFunctions.QBEncode(item.Element("Name").Value, 30);
                            objProp_User.SalesDescription = item.Element("FullName").Value;
                            objProp_User.CatStatus = Status(item.Element("IsActive").Value);
                            objProp_User.Balance = 0.00;
                            objProp_User.Measure = "";
                            objProp_User.Remarks = "";
                            objProp_User.Type = "0";
                            objProp_User.QBAccountID = "";
                            objProp_User.LastUpdateDate = Convert.ToDateTime(item.Element("TimeModified").Value);

                            objBL_User.AddQBBillCode(objProp_User);
                        }

                        foreach (XElement item in Queryitem.Descendants("ItemInventoryAssemblyRet"))
                        {
                            objProp_User.QBInvID = item.Element("ListID").Value;
                            objProp_User.ConnConfig = objGenFunctions.ConnectionStr(Connection);
                            objProp_User.ContactName = objGenFunctions.QBEncode(item.Element("Name").Value, 30);
                            objProp_User.SalesDescription = item.Element("FullName").Value;
                            objProp_User.CatStatus = Status(item.Element("IsActive").Value);
                            objProp_User.Balance = 0.00;
                            objProp_User.Measure = "";
                            objProp_User.Remarks = "";
                            objProp_User.Type = "0";
                            objProp_User.QBAccountID = "";
                            objProp_User.LastUpdateDate = Convert.ToDateTime(item.Element("TimeModified").Value);

                            objBL_User.AddQBBillCode(objProp_User);
                        }

                        foreach (XElement item in Queryitem.Descendants("ItemOtherChargeRet"))
                        {
                            objProp_User.QBInvID = item.Element("ListID").Value;
                            objProp_User.ConnConfig = objGenFunctions.ConnectionStr(Connection);
                            objProp_User.ContactName = objGenFunctions.QBEncode(item.Element("Name").Value, 30);
                            objProp_User.SalesDescription = item.Element("FullName").Value;
                            objProp_User.CatStatus = Status(item.Element("IsActive").Value);
                            objProp_User.Balance = 0.00;
                            objProp_User.Measure = "";
                            objProp_User.Remarks = "";
                            objProp_User.Type = "1";
                            objProp_User.QBAccountID = "";
                            objProp_User.LastUpdateDate = Convert.ToDateTime(item.Element("TimeModified").Value);

                            objBL_User.AddQBBillCode(objProp_User);
                        }

                        foreach (XElement item in Queryitem.Descendants("ItemFixedAssetRet"))
                        {
                            objProp_User.QBInvID = item.Element("ListID").Value;
                            objProp_User.ConnConfig = objGenFunctions.ConnectionStr(Connection);
                            objProp_User.ContactName = objGenFunctions.QBEncode(item.Element("Name").Value, 30);
                            //objProp_User.SalesDescription = item.Element("FullName").Value;
                            objProp_User.CatStatus = Status(item.Element("IsActive").Value);
                            objProp_User.Balance = 0.00;
                            objProp_User.Measure = "";
                            objProp_User.Remarks = "";
                            objProp_User.Type = "1";
                            objProp_User.QBAccountID = "";
                            objProp_User.LastUpdateDate = Convert.ToDateTime(item.Element("TimeModified").Value);

                            objBL_User.AddQBBillCode(objProp_User);
                        }

                        foreach (XElement item in Queryitem.Descendants("ItemSubtotalRet"))
                        {
                            objProp_User.QBInvID = item.Element("ListID").Value;
                            objProp_User.ConnConfig = objGenFunctions.ConnectionStr(Connection);
                            objProp_User.ContactName = objGenFunctions.QBEncode(item.Element("Name").Value, 30);
                            //objProp_User.SalesDescription = item.Element("FullName").Value;
                            objProp_User.CatStatus = Status(item.Element("IsActive").Value);
                            objProp_User.Balance = 0.00;
                            objProp_User.Measure = "";
                            objProp_User.Remarks = "";
                            objProp_User.Type = "1";
                            objProp_User.QBAccountID = "";
                            objProp_User.LastUpdateDate = Convert.ToDateTime(item.Element("TimeModified").Value);

                            objBL_User.AddQBBillCode(objProp_User);
                        }
                        foreach (XElement item in Queryitem.Descendants("ItemDiscountRet"))
                        {
                            objProp_User.QBInvID = item.Element("ListID").Value;
                            objProp_User.ConnConfig = objGenFunctions.ConnectionStr(Connection);
                            objProp_User.ContactName = objGenFunctions.QBEncode(item.Element("Name").Value, 30);
                            objProp_User.SalesDescription = item.Element("FullName").Value;
                            objProp_User.CatStatus = Status(item.Element("IsActive").Value);
                            objProp_User.Balance = 0.00;
                            objProp_User.Measure = "";
                            objProp_User.Remarks = "";
                            objProp_User.Type = "1";
                            objProp_User.QBAccountID = "";
                            objProp_User.LastUpdateDate = Convert.ToDateTime(item.Element("TimeModified").Value);

                            objBL_User.AddQBBillCode(objProp_User);
                        }
                        foreach (XElement item in Queryitem.Descendants("ItemPaymentRet"))
                        {
                            objProp_User.QBInvID = item.Element("ListID").Value;
                            objProp_User.ConnConfig = objGenFunctions.ConnectionStr(Connection);
                            objProp_User.ContactName = objGenFunctions.QBEncode(item.Element("Name").Value, 30);
                            //objProp_User.SalesDescription = item.Element("FullName").Value;
                            objProp_User.CatStatus = Status(item.Element("IsActive").Value);
                            objProp_User.Balance = 0.00;
                            objProp_User.Measure = "";
                            objProp_User.Remarks = "";
                            objProp_User.Type = "1";
                            objProp_User.QBAccountID = "";
                            objProp_User.LastUpdateDate = Convert.ToDateTime(item.Element("TimeModified").Value);

                            objBL_User.AddQBBillCode(objProp_User);
                        }
                        foreach (XElement item in Queryitem.Descendants("ItemSalesTaxRet"))
                        {
                            objProp_User.QBInvID = item.Element("ListID").Value;
                            objProp_User.ConnConfig = objGenFunctions.ConnectionStr(Connection);
                            objProp_User.ContactName = objGenFunctions.QBEncode(item.Element("Name").Value, 30);
                            //objProp_User.SalesDescription = item.Element("FullName").Value;
                            objProp_User.CatStatus = Status(item.Element("IsActive").Value);
                            objProp_User.Balance = 0.00;
                            objProp_User.Measure = "";
                            objProp_User.Remarks = "";
                            objProp_User.Type = "1";
                            objProp_User.QBAccountID = "";
                            objProp_User.LastUpdateDate = Convert.ToDateTime(item.Element("TimeModified").Value);

                            objBL_User.AddQBBillCode(objProp_User);
                        }
                        foreach (XElement item in Queryitem.Descendants("ItemSalesTaxGroupRet"))
                        {
                            objProp_User.QBInvID = item.Element("ListID").Value;
                            objProp_User.ConnConfig = objGenFunctions.ConnectionStr(Connection);
                            objProp_User.ContactName = objGenFunctions.QBEncode(item.Element("Name").Value, 30);
                            //objProp_User.SalesDescription = item.Element("FullName").Value;
                            objProp_User.CatStatus = Status(item.Element("IsActive").Value);
                            objProp_User.Balance = 0.00;
                            objProp_User.Measure = "";
                            objProp_User.Remarks = "";
                            objProp_User.Type = "1";
                            objProp_User.QBAccountID = "";
                            objProp_User.LastUpdateDate = Convert.ToDateTime(item.Element("TimeModified").Value);

                            objBL_User.AddQBBillCode(objProp_User);
                        }
                        foreach (XElement item in Queryitem.Descendants("ItemGroupRet"))
                        {
                            objProp_User.QBInvID = item.Element("ListID").Value;
                            objProp_User.ConnConfig = objGenFunctions.ConnectionStr(Connection);
                            objProp_User.ContactName = objGenFunctions.QBEncode(item.Element("Name").Value, 30);
                            //objProp_User.SalesDescription = item.Element("FullName").Value;
                            objProp_User.CatStatus = Status(item.Element("IsActive").Value);
                            objProp_User.Balance = 0.00;
                            objProp_User.Measure = "";
                            objProp_User.Remarks = "";
                            objProp_User.Type = "1";
                            objProp_User.QBAccountID = "";
                            objProp_User.LastUpdateDate = Convert.ToDateTime(item.Element("TimeModified").Value);

                            objBL_User.AddQBBillCode(objProp_User);
                        }
                    }
                    #endregion

                    #region Import Terms

                    foreach (XElement Queryitem in xdoc.Descendants("TermsQueryRs"))
                    {
                        //if (Queryitem.Attribute("requestID").Value == "1")
                        //{
                        foreach (XElement item in Queryitem.Descendants("StandardTermsRet"))
                        {
                            objProp_User.QBTermsID = item.Element("ListID").Value;
                            objProp_User.Type = objGenFunctions.QBEncode(item.Element("Name").Value, 50);
                            objProp_User.ConnConfig = objGenFunctions.ConnectionStr(Connection);
                            objProp_User.LastUpdateDate = Convert.ToDateTime(item.Element("TimeModified").Value);

                            objBL_User.AddQBTerms(objProp_User);
                        }
                        //}
                    }
                    #endregion

                    #region Import PayrollWage

                    foreach (XElement Queryitem in xdoc.Descendants("PayrollItemWageQueryRs"))
                    {
                        foreach (XElement item in Queryitem.Descendants("PayrollItemWageRet"))
                        {
                            string QBAccountID = string.Empty;
                            objProp_User.QBWageID = item.Element("ListID").Value;
                            if (item.Element("ExpenseAccountRef") != null)
                            {
                                if (item.Element("ExpenseAccountRef").Element("ListID") != null)
                                    QBAccountID = item.Element("ExpenseAccountRef").Element("ListID").Value;
                            }
                            objProp_User.QBAccountID = QBAccountID;
                            objProp_User.Type = objGenFunctions.QBEncode(item.Element("Name").Value, 50);
                            objProp_User.ConnConfig = objGenFunctions.ConnectionStr(Connection);
                            objProp_User.LastUpdateDate = Convert.ToDateTime(item.Element("TimeModified").Value);

                            objBL_User.AddQBPayrollWage(objProp_User);
                        }
                    }
                    #endregion

                    #region Import Invoice
                    foreach (XElement Queryitem in xdoc.Descendants("InvoiceQueryRs"))
                    {
                        List<string> InvoiceIteratorIDs = new List<string>();
                        if (Session["InvoiceIteratorIDs"] != null)
                        {
                            InvoiceIteratorIDs = (List<string>)Session["InvoiceIteratorIDs"];
                        }

                        var iteratorRemainingCount = Queryitem.Attribute("iteratorRemainingCount").Value;
                        if (iteratorRemainingCount != "0")
                        {
                            var iteratorID = Queryitem.Attribute("iteratorID").Value;
                            InvoiceIteratorIDs.Add(iteratorID);
                            Session["InvoiceIteratorIDs"] = InvoiceIteratorIDs;
                        }

                        foreach (XElement item in Queryitem.Descendants("InvoiceRet"))
                        {
                            #region Lineitems

                            #region dtLineItems
                            DataTable dtLineItems = new DataTable();
                            dtLineItems.Columns.Add("Ref", typeof(int));
                            dtLineItems.Columns.Add("line", typeof(int));
                            dtLineItems.Columns.Add("QBinvID", typeof(string));
                            dtLineItems.Columns.Add("Quan", typeof(double));
                            dtLineItems.Columns.Add("fDesc", typeof(string));
                            dtLineItems.Columns.Add("Price", typeof(double));
                            dtLineItems.Columns.Add("Amount", typeof(double));
                            dtLineItems.Columns.Add("STax", typeof(int));
                            dtLineItems.Columns.Add("Job", typeof(int));
                            dtLineItems.Columns.Add("JobItem", typeof(int));
                            dtLineItems.Columns.Add("TransID", typeof(int));
                            dtLineItems.Columns.Add("Measure", typeof(string));
                            dtLineItems.Columns.Add("Disc", typeof(double));
                            dtLineItems.Columns.Add("StaxAmt", typeof(double));
                            #endregion

                            double staxpercent = 0;
                            if (item.Element("SalesTaxPercentage") != null)
                                staxpercent = Convert.ToDouble(item.Element("SalesTaxPercentage").Value);


                            int lineindex = 1;
                            foreach (XElement lineitem in item.Descendants("InvoiceLineRet"))
                            {
                                DataRow dr = dtLineItems.NewRow();
                                dr["Ref"] = 0;
                                dr["line"] = lineindex;

                                if (lineitem.Element("ItemRef") != null)
                                    dr["QBinvID"] = lineitem.Element("ItemRef").Element("ListID").Value;

                                if (lineitem.Element("Quantity") != null)
                                    dr["Quan"] = Convert.ToDouble(lineitem.Element("Quantity").Value);
                                else
                                    dr["Quan"] = 1;

                                if (lineitem.Element("Desc") != null)
                                    dr["fDesc"] = lineitem.Element("Desc").Value;

                                if (lineitem.Element("Rate") != null)
                                {
                                    dr["Price"] = Convert.ToDouble(lineitem.Element("Rate").Value);
                                }
                                else
                                {
                                    dr["Price"] = 0;
                                }

                                if (lineitem.Element("Amount") != null)
                                {
                                    if (lineitem.Element("Rate") == null && lineitem.Element("RatePercent") != null)
                                        dr["Price"] = Convert.ToDouble(lineitem.Element("Amount").Value);
                                }

                                if (lineitem.Element("IsTaxable") != null)
                                {
                                    int stax = 0;
                                    if (lineitem.Element("IsTaxable").Value == "false")
                                    {
                                        stax = 1;
                                    }

                                    dr["STax"] = stax;
                                }
                                else
                                {
                                    dr["STax"] = 0;
                                }

                                if (lineitem.Element("SalesTaxCodeRef") != null)
                                {
                                    if (lineitem.Element("SalesTaxCodeRef").Element("FullName").Value.ToUpper() == "TAX")
                                        dr["STax"] = 1;
                                    else
                                        dr["STax"] = 0;
                                }

                                //if (lineitem.Element("TaxAmount") != null)
                                //dr["STaxAmt"] = Convert.ToDouble(lineitem.Element("TaxAmount").Value);
                                if (dr["STax"].ToString() == "1")
                                    dr["STaxAmt"] = (((Convert.ToDouble(dr["Quan"].ToString()) * Convert.ToDouble(dr["Price"].ToString())) * staxpercent) / 100);
                                else
                                    dr["STaxAmt"] = 0;

                                //if (lineitem.Element("Amount") != null)
                                //dr["Amount"] = Convert.ToDouble(lineitem.Element("Amount").Value);
                                dr["Amount"] = (Convert.ToDouble(dr["Quan"].ToString()) * Convert.ToDouble(dr["Price"].ToString())) + Convert.ToDouble(dr["STaxAmt"]);
                                //else
                                //    dr["Amount"] = 0;

                                dr["Job"] = 0;
                                dr["JobItem"] = 0;
                                dr["TransID"] = 0;
                                dr["Measure"] = string.Empty;
                                dr["Disc"] = 0;

                                dtLineItems.Rows.Add(dr);
                                lineindex++;
                            }
                            #endregion

                            objProp_Contracts.LastUpdateDate = Convert.ToDateTime(item.Element("TimeModified").Value);

                            if (item.Element("TxnID") != null)
                                objProp_Contracts.QBInvID = item.Element("TxnID").Value;

                            if (item.Element("Subtotal") != null)
                                objProp_Contracts.Total = Convert.ToDouble(item.Element("Subtotal").Value);
                            else
                                objProp_Contracts.Total = 0;

                            if (item.Element("Subtotal") != null)
                                objProp_Contracts.Amount = Convert.ToDouble(item.Element("Subtotal").Value);
                            else
                                objProp_Contracts.Amount = 0;

                            if (item.Element("SalesTaxTotal") != null)
                                objProp_Contracts.Staxtotal = Convert.ToDouble(item.Element("SalesTaxTotal").Value);
                            else
                                objProp_Contracts.Staxtotal = 0;

                            if (item.Element("SalesTaxPercentage") != null)
                                objProp_Contracts.Taxrate = Convert.ToDouble(item.Element("SalesTaxPercentage").Value);
                            else
                                objProp_Contracts.Taxrate = 0;

                            if (item.Element("TxnDate") != null)
                                objProp_Contracts.Date = Convert.ToDateTime(item.Element("TxnDate").Value);

                            if (item.Element("ClassRef") != null)
                                objProp_Contracts.QBJobtypeID = item.Element("ClassRef").Element("ListID").Value;

                            objProp_Contracts.JobId = 0;

                            if (item.Element("CustomerRef") != null)
                                objProp_Contracts.QBCustomerID = item.Element("CustomerRef").Element("ListID").Value;

                            if (item.Element("TermsRef") != null)
                                objProp_Contracts.QBTermsID = item.Element("TermsRef").Element("ListID").Value;

                            if (item.Element("PONumber") != null)
                                objProp_Contracts.PO = item.Element("PONumber").Value;
                            else
                                objProp_Contracts.PO = string.Empty;

                            objProp_Contracts.Status = 0;
                            if (item.Element("IsPaid") != null)
                            {
                                if (Convert.ToBoolean(item.Element("IsPaid").Value) == true)
                                    objProp_Contracts.Status = 1;
                            }

                            if (item.Element("IsPending") != null)
                            {
                                if (Convert.ToBoolean(item.Element("IsPending").Value) == true)
                                    objProp_Contracts.Status = 4;
                            }

                            if (item.Element("Memo") != null)
                                objProp_Contracts.Remarks = item.Element("Memo").Value;
                            else
                                objProp_Contracts.Remarks = string.Empty;

                            objProp_Contracts.TaxRegion = string.Empty;
                            if (item.Element("ItemSalesTaxRef") != null)
                            {
                                if (item.Element("ItemSalesTaxRef").Element("FullName") != null)
                                    objProp_Contracts.TaxRegion = item.Element("ItemSalesTaxRef").Element("FullName").Value;
                            }

                            if (item.Element("TxnDate") != null)
                                objProp_Contracts.Idate = Convert.ToDateTime(item.Element("TxnDate").Value);

                            if (item.Element("RefNumber") != null)
                                objProp_Contracts.InvoiceIDCustom = item.Element("RefNumber").Value;

                            string BillAdd1 = string.Empty;
                            string BillAdd2 = string.Empty;
                            string BillAdd3 = string.Empty;
                            string BillCity = string.Empty;
                            string BillState = string.Empty;
                            string BillZip = string.Empty;

                            if (item.Element("BillAddress") != null)
                            {
                                if (item.Element("BillAddress").Element("Addr1") != null)
                                    BillAdd1 = item.Element("BillAddress").Element("Addr1").Value;

                                if (item.Element("BillAddress").Element("Addr2") != null)
                                    BillAdd2 = item.Element("BillAddress").Element("Addr2").Value;

                                if (item.Element("BillAddress").Element("Addr3") != null)
                                    BillAdd3 = item.Element("BillAddress").Element("Addr3").Value;

                                if (item.Element("BillAddress").Element("City") != null)
                                    BillCity = item.Element("BillAddress").Element("City").Value;

                                if (item.Element("BillAddress").Element("State") != null)
                                    BillState = item.Element("BillAddress").Element("State").Value;

                                if (item.Element("BillAddress").Element("PostalCode") != null)
                                    BillZip = item.Element("BillAddress").Element("PostalCode").Value;

                            }
                            string BillAddress = SuffixSpace(BillAdd1) + Environment.NewLine + SuffixSpace(BillAdd2) + Environment.NewLine + SuffixSpace(BillAdd3) + SuffixSpace(BillCity) + SuffixSpace(BillState) + SuffixSpace(BillZip);

                            objProp_Contracts.BillTo = BillAddress;
                            objProp_Contracts.ConnConfig = objGenFunctions.ConnectionStr(Connection);
                            objProp_Contracts.DtRecContr = dtLineItems;
                            objProp_Contracts.Fuser = "QB";
                            objProp_Contracts.StaxI = 1;
                            objProp_Contracts.Batch = 0;
                            objProp_Contracts.Gtax = 0.00;
                            objProp_Contracts.Mech = 0;//Convert.ToInt32(ddlRoute.SelectedValue);
                            objProp_Contracts.Taxrate2 = 0;
                            objProp_Contracts.TaxRegion2 = string.Empty;
                            //objProp_Contracts.Remarks = txtRemarks.Text;
                            objProp_Contracts.Taxfactor = 100;
                            objProp_Contracts.Taxable = Convert.ToInt32(0);
                            objProp_Contracts.TicketID = 0;
                            objBL_Contracts.CreateQBInvoice(objProp_Contracts);
                        }
                    }
                    #endregion

                    #region Import Employee

                    foreach (XElement Queryitem in xdoc.Descendants("EmployeeQueryRs"))
                    {
                        foreach (XElement item in Queryitem.Descendants("EmployeeRet"))
                        {
                            string Add1 = string.Empty;
                            string Add2 = string.Empty;
                            string City = string.Empty;
                            string State = string.Empty;
                            string Zip = string.Empty;

                            if (item.Element("EmployeeAddress") != null)
                            {
                                if (item.Element("EmployeeAddress").Element("Addr1") != null)
                                {
                                    Add1 = item.Element("EmployeeAddress").Element("Addr1").Value;
                                }
                                if (item.Element("EmployeeAddress").Element("Addr2") != null)
                                {
                                    Add2 = item.Element("EmployeeAddress").Element("Addr2").Value;
                                }
                                if (item.Element("EmployeeAddress").Element("City") != null)
                                    City = item.Element("EmployeeAddress").Element("City").Value;

                                if (item.Element("EmployeeAddress").Element("State") != null)
                                    State = item.Element("EmployeeAddress").Element("State").Value;

                                if (item.Element("EmployeeAddress").Element("PostalCode") != null)
                                    Zip = item.Element("EmployeeAddress").Element("PostalCode").Value;
                            }
                            string Address = SuffixSpace(Add1) + Environment.NewLine + SuffixSpace(Add2);

                            objProp_User.Address = Address;
                            objProp_User.City = City;
                            objProp_User.State = State;
                            objProp_User.Zip = Zip;

                            if (item.Element("Mobile") != null)
                                objProp_User.Cell = item.Element("Mobile").Value;
                            else
                                objProp_User.Cell = string.Empty;

                            if (item.Element("Email") != null)
                                objProp_User.Email = item.Element("Email").Value;
                            else
                                objProp_User.Email = string.Empty;

                            if (item.Element("Phone") != null)
                                objProp_User.Tele = item.Element("Phone").Value;
                            else
                                objProp_User.Tele = string.Empty;

                            objProp_User.Pager = string.Empty;

                            if (item.Element("FirstName") != null)
                            {
                                objProp_User.FirstName = item.Element("FirstName").Value;
                                objProp_User.Username = item.Element("FirstName").Value;
                                objProp_User.Password = "password";
                            }
                            else
                            {
                                objProp_User.FirstName = string.Empty;
                                objProp_User.Username = string.Empty;
                                objProp_User.Password = string.Empty;
                            }

                            if (item.Element("MiddleName") != null)
                            {
                                objProp_User.MiddleName = item.Element("MiddleName").Value;
                                //if (item.Element("MiddleName").Value.Trim() != string.Empty)
                                //{
                                //    objProp_User.Username += "_" + item.Element("MiddleName").Value;
                                //}
                            }
                            else
                            {
                                objProp_User.MiddleName = string.Empty;
                            }

                            if (item.Element("LastName") != null)
                                objProp_User.LastNAme = item.Element("LastName").Value;
                            else
                                objProp_User.LastNAme = string.Empty;

                            if (item.Element("IsActive") != null)
                            {
                                if (Convert.ToBoolean(item.Element("IsActive").Value) == true)
                                    objProp_User.Status = 0;
                                else
                                    objProp_User.Status = 1;
                            }

                            if (item.Element("Notes") != null)
                                objProp_User.Remarks = item.Element("Notes").Value;
                            else
                                objProp_User.Remarks = string.Empty;

                            if (item.Element("ReleasedDate") != null)
                                objProp_User.DtFired = Convert.ToDateTime(item.Element("ReleasedDate").Value);

                            if (item.Element("HiredDate") != null)
                                objProp_User.DtHired = Convert.ToDateTime(item.Element("HiredDate").Value);

                            if (item.Element("ListID") != null)
                                objProp_User.QBEmployeeID = item.Element("ListID").Value;

                            objProp_User.DeviceID = string.Empty;
                            objProp_User.Field = 0;
                            objProp_User.Supervisor = string.Empty;
                            objProp_User.Lang = "english";
                            objProp_User.Salesperson = 0;
                            objProp_User.Schedule = 0;
                            objProp_User.Mapping = 0;
                            objProp_User.AccessUser = "N";
                            objProp_User.Expenses = "Y";
                            objProp_User.LocationRemarks = "Y";
                            objProp_User.ProgFunctions = "N";
                            objProp_User.PurchaseOrd = "Y";
                            objProp_User.ServiceHist = "Y";
                            objProp_User.CreateTicket = "Y";
                            objProp_User.WorkDate = "N";
                            objProp_User.MerchantInfoId = 0;
                            objProp_User.UserLic = string.Empty;
                            objProp_User.UserLicID = 0;
                            objProp_User.LastUpdateDate = Convert.ToDateTime(item.Element("TimeModified").Value);

                            objProp_User.ConnConfig = objGenFunctions.ConnectionStr(Connection);
                            DataSet dsUserAddRs = objBL_User.AddQBUser(objProp_User);
                            if (dsUserAddRs.Tables.Count > 0)
                            {
                                if (dsUserAddRs.Tables[0].Rows.Count > 0)
                                {
                                    string UserN = dsUserAddRs.Tables[0].Rows[0]["UserName"].ToString();
                                    int UserID = Convert.ToInt32(dsUserAddRs.Tables[0].Rows[0]["userid"]);

                                    UserRegistration(UserN, UserID);
                                }
                            }
                        }
                    }

                    #endregion
                }
                catch (Exception ex)
                {
                    SaveErrorCodes("Import data QB --> MOM", "", "", "Error", ex.Message);
                }

                try
                {
                    #region Handle add response from QB

                    // for CustomerType update QBcustomerTypeID field in MSM otype table
                    #region for CustomerType update QBcustomerTypeID field in MSM otype table
                    foreach (XElement Queryitem in xdoc.Descendants("CustomerTypeAddRs"))
                    {
                        if (Queryitem.Attribute("statusCode").Value != "0")
                        {
                            SaveErrorCodes(Queryitem.Name.ToString(), Queryitem.Attribute("requestID").Value, Queryitem.Attribute("statusCode").Value, Queryitem.Attribute("statusSeverity").Value, Queryitem.Attribute("statusMessage").Value);
                        }
                        foreach (XElement item in Queryitem.Descendants("CustomerTypeRet"))
                        {
                            objProp_User.ConnConfig = objGenFunctions.ConnectionStr(Connection);
                            objProp_User.QBCustomerTypeID = item.Element("ListID").Value;
                            objProp_User.CustomerType = Queryitem.Attribute("requestID").Value;
                            objBL_User.UpdateQBcustomertypeID(objProp_User);
                        }
                    }
                    #endregion

                    // for LocationType update QBLocTypeID field in MSM loctype table
                    #region for LocationType update QBLocTypeID field in MSM loctype table
                    foreach (XElement Queryitem in xdoc.Descendants("JobTypeAddRs"))
                    {
                        if (Queryitem.Attribute("statusCode").Value != "0")
                        {
                            SaveErrorCodes(Queryitem.Name.ToString(), Queryitem.Attribute("requestID").Value, Queryitem.Attribute("statusCode").Value, Queryitem.Attribute("statusSeverity").Value, Queryitem.Attribute("statusMessage").Value);
                        }
                        foreach (XElement item in Queryitem.Descendants("JobTypeRet"))
                        {
                            objProp_User.ConnConfig = objGenFunctions.ConnectionStr(Connection);
                            objProp_User.QBCustomerTypeID = item.Element("ListID").Value;
                            objProp_User.CustomerType = Queryitem.Attribute("requestID").Value;
                            objBL_User.UpdateQBJobtypeID(objProp_User);
                        }
                    }
                    #endregion

                    // for SalesTax update QBSalesTaxID field in MSM stax table
                    #region for SalesTax update QBSalesTaxID field in MSM stax table
                    foreach (XElement Queryitem in xdoc.Descendants("ItemSalesTaxAddRs"))
                    {
                        if (Queryitem.Attribute("statusCode").Value != "0")
                        {
                            SaveErrorCodes(Queryitem.Name.ToString(), Queryitem.Attribute("requestID").Value, Queryitem.Attribute("statusCode").Value, Queryitem.Attribute("statusSeverity").Value, Queryitem.Attribute("statusMessage").Value);
                        }

                        foreach (XElement item in Queryitem.Descendants("ItemSalesTaxRet"))
                        {
                            objProp_User.ConnConfig = objGenFunctions.ConnectionStr(Connection);
                            objProp_User.QBSalesTaxID = item.Element("ListID").Value;
                            objProp_User.SalesTax = Queryitem.Attribute("requestID").Value;
                            objBL_User.UpdateQBsalestaxID(objProp_User);

                            objProp_User.QBInvID = item.Element("ListID").Value;
                            objProp_User.ContactName = objGenFunctions.QBEncode(item.Element("Name").Value, 30);
                            objProp_User.CatStatus = Status(item.Element("IsActive").Value);
                            objProp_User.Balance = 0.00;
                            objProp_User.Measure = "";
                            objProp_User.Remarks = "";
                            objProp_User.Type = "1";
                            objProp_User.QBAccountID = "";
                            objProp_User.LastUpdateDate = Convert.ToDateTime(item.Element("TimeModified").Value);

                            objBL_User.AddQBBillCode(objProp_User);
                        }
                    }
                    #endregion

                    // for customers/locations update QBcustomerID/QBlocationID field in MSM owner/loc table
                    #region for customers/locations update QBcustomerID/QBlocationID field in MSM owner/loc table
                    foreach (XElement Queryitem in xdoc.Descendants("CustomerAddRs"))
                    {
                        if (Queryitem.Attribute("statusCode").Value != "0")
                        {
                            SaveErrorCodes(Queryitem.Name.ToString(), Queryitem.Attribute("requestID").Value, Queryitem.Attribute("statusCode").Value, Queryitem.Attribute("statusSeverity").Value, Queryitem.Attribute("statusMessage").Value);
                        }

                        if (Queryitem.Attribute("requestID").Value.Split('-').Count() == 2)
                        {
                            // for customers update QBcustomerID field in MSM owner table
                            if (Queryitem.Attribute("requestID").Value.Split('-')[0] == "1")
                            {
                                foreach (XElement item in Queryitem.Descendants("CustomerRet"))
                                {
                                    objProp_User.ConnConfig = objGenFunctions.ConnectionStr(Connection);
                                    objProp_User.QBCustomerID = item.Element("ListID").Value;
                                    objProp_User.CustomerID = Convert.ToInt32(Queryitem.Attribute("requestID").Value.Split('-')[1]);
                                    //objProp_User.CustomerID = Convert.ToInt32(item.Element("AltPhone").Value);
                                    objBL_User.UpdateQBCustomerID(objProp_User);
                                }
                            }
                            // for locations update QBlocationID field in MSM loc table
                            else if (Queryitem.Attribute("requestID").Value.Split('-')[0] == "2")
                            {
                                foreach (XElement item in Queryitem.Descendants("CustomerRet"))
                                {
                                    objProp_User.ConnConfig = objGenFunctions.ConnectionStr(Connection);
                                    objProp_User.QBlocationID = item.Element("ListID").Value;
                                    objProp_User.LocID = Convert.ToInt32(Queryitem.Attribute("requestID").Value.Split('-')[1]);
                                    objBL_User.UpdateQBLocationID(objProp_User);
                                }
                            }
                        }
                    }
                    #endregion

                    // for Class update QBjobtypeID field in MSM jobtype table
                    #region for Class update QBjobtypeID field in MSM jobtype table
                    foreach (XElement Queryitem in xdoc.Descendants("ClassAddRs"))
                    {
                        if (Queryitem.Attribute("statusCode").Value != "0")
                        {
                            SaveErrorCodes(Queryitem.Name.ToString(), Queryitem.Attribute("requestID").Value, Queryitem.Attribute("statusCode").Value, Queryitem.Attribute("statusSeverity").Value, Queryitem.Attribute("statusMessage").Value);
                        }

                        foreach (XElement item in Queryitem.Descendants("ClassRet"))
                        {
                            objProp_User.ConnConfig = objGenFunctions.ConnectionStr(Connection);
                            objProp_User.QBJobtypeID = item.Element("ListID").Value;
                            objProp_User.DepartmentID = Convert.ToInt32(Queryitem.Attribute("requestID").Value);
                            objBL_User.UpdateQBDepartmentID(objProp_User);
                        }
                    }
                    #endregion

                    // for Billcode update QBInvID field in MSM Inv table
                    #region for Billcode update QBInvID field in MSM Inv table
                    foreach (XElement Queryitem in xdoc.Descendants("ItemServiceAddRs"))
                    {
                        if (Queryitem.Attribute("statusCode").Value != "0")
                        {
                            SaveErrorCodes(Queryitem.Name.ToString(), Queryitem.Attribute("requestID").Value, Queryitem.Attribute("statusCode").Value, Queryitem.Attribute("statusSeverity").Value, Queryitem.Attribute("statusMessage").Value);
                        }

                        foreach (XElement item in Queryitem.Descendants("ItemServiceRet"))
                        {
                            objProp_User.ConnConfig = objGenFunctions.ConnectionStr(Connection);
                            objProp_User.QBInvID = item.Element("ListID").Value;
                            objProp_User.BillCode = Convert.ToInt32(Queryitem.Attribute("requestID").Value);
                            objBL_User.UpdateQBInvID(objProp_User);
                        }
                    }
                    #endregion

                    // for terms update QBTermsID field in MSM tblterms table
                    #region for terms update QBTermsID field in MSM tblterms table
                    List<int> duplicateTermsID = new List<int>();
                    Session["duplicateTermsID"] = null;
                    foreach (XElement Queryitem in xdoc.Descendants("StandardTermsAddRs"))
                    {
                        if (Queryitem.Attribute("statusCode").Value != "0")
                        {
                            SaveErrorCodes(Queryitem.Name.ToString(), Queryitem.Attribute("requestID").Value, Queryitem.Attribute("statusCode").Value, Queryitem.Attribute("statusSeverity").Value, Queryitem.Attribute("statusMessage").Value);
                        }

                        foreach (XElement item in Queryitem.Descendants("StandardTermsRet"))
                        {
                            objProp_User.ConnConfig = objGenFunctions.ConnectionStr(Connection);
                            objProp_User.QBTermsID = item.Element("ListID").Value;
                            objProp_User.TermsID = Convert.ToInt32(Queryitem.Attribute("requestID").Value);
                            objBL_User.UpdateQBTermsID(objProp_User);
                        }
                        if (Queryitem.Attribute("statusCode").Value == "3100")
                        {
                            duplicateTermsID.Add(Convert.ToInt32(Queryitem.Attribute("requestID").Value));
                        }
                    }
                    if (duplicateTermsID.Count > 0)
                        Session["duplicateTermsID"] = duplicateTermsID;
                    #endregion

                    // for Account update QBAccountID field in MSM chart table
                    #region for Account update QBAccountID field in MSM chart table
                    foreach (XElement Queryitem in xdoc.Descendants("AccountAddRs"))
                    {
                        if (Queryitem.Attribute("statusCode").Value != "0")
                        {
                            SaveErrorCodes(Queryitem.Name.ToString(), Queryitem.Attribute("requestID").Value, Queryitem.Attribute("statusCode").Value, Queryitem.Attribute("statusSeverity").Value, Queryitem.Attribute("statusMessage").Value);
                        }

                        foreach (XElement item in Queryitem.Descendants("AccountRet"))
                        {
                            objProp_User.ConnConfig = objGenFunctions.ConnectionStr(Connection);
                            objProp_User.QBAccountID = item.Element("ListID").Value;
                            objProp_User.AccountID = Convert.ToInt32(Queryitem.Attribute("requestID").Value);
                            objBL_User.UpdateQBAccountID(objProp_User);
                        }
                    }
                    #endregion

                    // for Vendor update QBVendorID field in MSM Vendor table
                    #region for Vendor update QBVendorID field in MSM Vendor table
                    foreach (XElement Queryitem in xdoc.Descendants("VendorAddRs"))
                    {
                        if (Queryitem.Attribute("statusCode").Value != "0")
                        {
                            SaveErrorCodes(Queryitem.Name.ToString(), Queryitem.Attribute("requestID").Value, Queryitem.Attribute("statusCode").Value, Queryitem.Attribute("statusSeverity").Value, Queryitem.Attribute("statusMessage").Value);
                        }

                        foreach (XElement item in Queryitem.Descendants("VendorRet"))
                        {
                            objProp_User.ConnConfig = objGenFunctions.ConnectionStr(Connection);
                            objProp_User.QBAccountID = item.Element("ListID").Value;
                            objProp_User.AccountID = Convert.ToInt32(Queryitem.Attribute("requestID").Value);
                            objBL_User.UpdateQBVendorID(objProp_User);
                        }
                    }
                    #endregion

                    // for Payroll wage update QBWageID field in MSM PRwage table
                    #region for Payroll wage update QBWageID field in MSM PRwage table
                    foreach (XElement Queryitem in xdoc.Descendants("PayrollItemWageAddRs"))
                    {
                        if (Queryitem.Attribute("statusCode").Value != "0")
                        {
                            SaveErrorCodes(Queryitem.Name.ToString(), Queryitem.Attribute("requestID").Value, Queryitem.Attribute("statusCode").Value, Queryitem.Attribute("statusSeverity").Value, Queryitem.Attribute("statusMessage").Value);
                        }

                        foreach (XElement item in Queryitem.Descendants("PayrollItemWageRet"))
                        {
                            objProp_User.ConnConfig = objGenFunctions.ConnectionStr(Connection);
                            objProp_User.QBAccountID = item.Element("ListID").Value;
                            objProp_User.AccountID = Convert.ToInt32(Queryitem.Attribute("requestID").Value);
                            objBL_User.UpdateQBWageID(objProp_User);
                        }
                    }
                    #endregion

                    // for Invoice update QBInvoiceID field in MSM TicketD table
                    #region for Invoice update QBInvoiceID field in MSM TicketD table
                    foreach (XElement Queryitem in xdoc.Descendants("InvoiceAddRs"))
                    {
                        if (Queryitem.Attribute("statusCode").Value != "0")
                        {
                            SaveErrorCodes(Queryitem.Name.ToString(), Queryitem.Attribute("requestID").Value, Queryitem.Attribute("statusCode").Value, Queryitem.Attribute("statusSeverity").Value, Queryitem.Attribute("statusMessage").Value);
                        }

                        foreach (XElement item in Queryitem.Descendants("InvoiceRet"))
                        {
                            objMapData.ConnConfig = objGenFunctions.ConnectionStr(Connection);
                            objMapData.QBInvoiceID = item.Element("TxnID").Value;
                            objMapData.ManualInvoiceID = item.Element("RefNumber").Value;
                            objMapData.TicketID = Convert.ToInt32(Queryitem.Attribute("requestID").Value);
                            objBL_MapData.UpdateQBInvoiceTicketID(objMapData);

                            #region Invoice add

                            #region Lineitems

                            #region dtLineItems
                            DataTable dtLineItems = new DataTable();
                            dtLineItems.Columns.Add("Ref", typeof(int));
                            dtLineItems.Columns.Add("line", typeof(int));
                            dtLineItems.Columns.Add("QBinvID", typeof(string));
                            dtLineItems.Columns.Add("Quan", typeof(double));
                            dtLineItems.Columns.Add("fDesc", typeof(string));
                            dtLineItems.Columns.Add("Price", typeof(double));
                            dtLineItems.Columns.Add("Amount", typeof(double));
                            dtLineItems.Columns.Add("STax", typeof(int));
                            dtLineItems.Columns.Add("Job", typeof(int));
                            dtLineItems.Columns.Add("JobItem", typeof(int));
                            dtLineItems.Columns.Add("TransID", typeof(int));
                            dtLineItems.Columns.Add("Measure", typeof(string));
                            dtLineItems.Columns.Add("Disc", typeof(double));
                            dtLineItems.Columns.Add("StaxAmt", typeof(double));
                            #endregion

                            double staxpercent = 0;
                            if (item.Element("SalesTaxPercentage") != null)
                                staxpercent = Convert.ToDouble(item.Element("SalesTaxPercentage").Value);


                            int lineindex = 1;
                            foreach (XElement lineitem in item.Descendants("InvoiceLineRet"))
                            {
                                DataRow dr = dtLineItems.NewRow();
                                dr["Ref"] = 0;
                                dr["line"] = lineindex;

                                if (lineitem.Element("ItemRef") != null)
                                    dr["QBinvID"] = lineitem.Element("ItemRef").Element("ListID").Value;

                                if (lineitem.Element("Quantity") != null)
                                    dr["Quan"] = Convert.ToDouble(lineitem.Element("Quantity").Value);
                                else
                                    dr["Quan"] = 1;

                                if (lineitem.Element("Desc") != null)
                                    dr["fDesc"] = lineitem.Element("Desc").Value;

                                if (lineitem.Element("Rate") != null)
                                {
                                    dr["Price"] = Convert.ToDouble(lineitem.Element("Rate").Value);
                                }
                                else
                                {
                                    dr["Price"] = 0;
                                }

                                if (lineitem.Element("Amount") != null)
                                {
                                    if (lineitem.Element("Rate") == null && lineitem.Element("RatePercent") != null)
                                        dr["Price"] = Convert.ToDouble(lineitem.Element("Amount").Value);
                                }

                                if (lineitem.Element("IsTaxable") != null)
                                {
                                    int stax = 0;
                                    if (lineitem.Element("IsTaxable").Value == "false")
                                    {
                                        stax = 1;
                                    }

                                    dr["STax"] = stax;
                                }
                                else
                                {
                                    dr["STax"] = 0;
                                }

                                if (lineitem.Element("SalesTaxCodeRef") != null)
                                {
                                    if (lineitem.Element("SalesTaxCodeRef").Element("FullName").Value.ToUpper() == "TAX")
                                        dr["STax"] = 1;
                                    else
                                        dr["STax"] = 0;
                                }

                                //if (lineitem.Element("TaxAmount") != null)
                                //dr["STaxAmt"] = Convert.ToDouble(lineitem.Element("TaxAmount").Value);
                                if (dr["STax"].ToString() == "1")
                                    dr["STaxAmt"] = (((Convert.ToDouble(dr["Quan"].ToString()) * Convert.ToDouble(dr["Price"].ToString())) * staxpercent) / 100);
                                else
                                    dr["STaxAmt"] = 0;

                                //if (lineitem.Element("Amount") != null)
                                //dr["Amount"] = Convert.ToDouble(lineitem.Element("Amount").Value);
                                dr["Amount"] = (Convert.ToDouble(dr["Quan"].ToString()) * Convert.ToDouble(dr["Price"].ToString())) + Convert.ToDouble(dr["STaxAmt"]);
                                //else
                                //    dr["Amount"] = 0;

                                dr["Job"] = 0;
                                dr["JobItem"] = 0;
                                dr["TransID"] = 0;
                                dr["Measure"] = string.Empty;
                                dr["Disc"] = 0;

                                dtLineItems.Rows.Add(dr);
                                lineindex++;
                            }
                            #endregion

                            objProp_Contracts.LastUpdateDate = Convert.ToDateTime(item.Element("TimeModified").Value);

                            if (item.Element("TxnID") != null)
                                objProp_Contracts.QBInvID = item.Element("TxnID").Value;

                            if (item.Element("Subtotal") != null)
                                objProp_Contracts.Total = Convert.ToDouble(item.Element("Subtotal").Value);
                            else
                                objProp_Contracts.Total = 0;

                            if (item.Element("Subtotal") != null)
                                objProp_Contracts.Amount = Convert.ToDouble(item.Element("Subtotal").Value);
                            else
                                objProp_Contracts.Amount = 0;

                            if (item.Element("SalesTaxTotal") != null)
                                objProp_Contracts.Staxtotal = Convert.ToDouble(item.Element("SalesTaxTotal").Value);
                            else
                                objProp_Contracts.Staxtotal = 0;

                            if (item.Element("SalesTaxPercentage") != null)
                                objProp_Contracts.Taxrate = Convert.ToDouble(item.Element("SalesTaxPercentage").Value);
                            else
                                objProp_Contracts.Taxrate = 0;

                            if (item.Element("TxnDate") != null)
                                objProp_Contracts.Date = Convert.ToDateTime(item.Element("TxnDate").Value);

                            if (item.Element("ClassRef") != null)
                                objProp_Contracts.QBJobtypeID = item.Element("ClassRef").Element("ListID").Value;

                            objProp_Contracts.JobId = 0;

                            if (item.Element("CustomerRef") != null)
                                objProp_Contracts.QBCustomerID = item.Element("CustomerRef").Element("ListID").Value;

                            if (item.Element("TermsRef") != null)
                                objProp_Contracts.QBTermsID = item.Element("TermsRef").Element("ListID").Value;

                            if (item.Element("PONumber") != null)
                                objProp_Contracts.PO = item.Element("PONumber").Value;
                            else
                                objProp_Contracts.PO = string.Empty;

                            objProp_Contracts.Status = 0;
                            if (item.Element("IsPaid") != null)
                            {
                                if (Convert.ToBoolean(item.Element("IsPaid").Value) == true)
                                    objProp_Contracts.Status = 1;
                            }

                            if (item.Element("IsPending") != null)
                            {
                                if (Convert.ToBoolean(item.Element("IsPending").Value) == true)
                                    objProp_Contracts.Status = 4;
                            }

                            if (item.Element("Memo") != null)
                                objProp_Contracts.Remarks = item.Element("Memo").Value;
                            else
                                objProp_Contracts.Remarks = string.Empty;

                            objProp_Contracts.TaxRegion = string.Empty;
                            if (item.Element("ItemSalesTaxRef") != null)
                            {
                                if (item.Element("ItemSalesTaxRef").Element("FullName") != null)
                                    objProp_Contracts.TaxRegion = item.Element("ItemSalesTaxRef").Element("FullName").Value;
                            }

                            if (item.Element("TxnDate") != null)
                                objProp_Contracts.Idate = Convert.ToDateTime(item.Element("TxnDate").Value);

                            if (item.Element("RefNumber") != null)
                                objProp_Contracts.InvoiceIDCustom = item.Element("RefNumber").Value;

                            string BillAdd1 = string.Empty;
                            string BillAdd2 = string.Empty;
                            string BillAdd3 = string.Empty;
                            string BillCity = string.Empty;
                            string BillState = string.Empty;
                            string BillZip = string.Empty;

                            if (item.Element("BillAddress") != null)
                            {
                                if (item.Element("BillAddress").Element("Addr1") != null)
                                    BillAdd1 = item.Element("BillAddress").Element("Addr1").Value;

                                if (item.Element("BillAddress").Element("Addr2") != null)
                                    BillAdd2 = item.Element("BillAddress").Element("Addr2").Value;

                                if (item.Element("BillAddress").Element("Addr3") != null)
                                    BillAdd3 = item.Element("BillAddress").Element("Addr3").Value;

                                if (item.Element("BillAddress").Element("City") != null)
                                    BillCity = item.Element("BillAddress").Element("City").Value;

                                if (item.Element("BillAddress").Element("State") != null)
                                    BillState = item.Element("BillAddress").Element("State").Value;

                                if (item.Element("BillAddress").Element("PostalCode") != null)
                                    BillZip = item.Element("BillAddress").Element("PostalCode").Value;

                            }
                            string BillAddress = SuffixSpace(BillAdd1) + Environment.NewLine + SuffixSpace(BillAdd2) + Environment.NewLine + SuffixSpace(BillAdd3) + SuffixSpace(BillCity) + SuffixSpace(BillState) + SuffixSpace(BillZip);

                            objProp_Contracts.BillTo = BillAddress;
                            objProp_Contracts.ConnConfig = objGenFunctions.ConnectionStr(Connection);
                            objProp_Contracts.DtRecContr = dtLineItems;
                            objProp_Contracts.Fuser = "QB";
                            objProp_Contracts.StaxI = 1;
                            objProp_Contracts.Batch = 0;
                            objProp_Contracts.Gtax = 0.00;
                            objProp_Contracts.Mech = 0;//Convert.ToInt32(ddlRoute.SelectedValue);
                            objProp_Contracts.Taxrate2 = 0;
                            objProp_Contracts.TaxRegion2 = string.Empty;
                            //objProp_Contracts.Remarks = txtRemarks.Text;
                            objProp_Contracts.Taxfactor = 100;
                            objProp_Contracts.Taxable = Convert.ToInt32(0);
                            objProp_Contracts.TicketID = Convert.ToInt32(Queryitem.Attribute("requestID").Value);
                            objBL_Contracts.CreateQBInvoice(objProp_Contracts);

                            #endregion
                        }
                    }
                    #endregion

                    // for TimeSheet update QBQBTimeTxnID field in MSM TicketD table
                    #region for TimeSheet update QBQBTimeTxnID field in MSM TicketD table
                    foreach (XElement Queryitem in xdoc.Descendants("TimeTrackingAddRs"))
                    {
                        if (Queryitem.Attribute("statusCode").Value != "0")
                        {
                            SaveErrorCodes(Queryitem.Name.ToString(), Queryitem.Attribute("requestID").Value, Queryitem.Attribute("statusCode").Value, Queryitem.Attribute("statusSeverity").Value, Queryitem.Attribute("statusMessage").Value);
                        }

                        foreach (XElement item in Queryitem.Descendants("TimeTrackingRet"))
                        {
                            objMapData.ConnConfig = objGenFunctions.ConnectionStr(Connection);
                            objMapData.QBInvoiceID = item.Element("TxnID").Value;
                            objMapData.TicketID = Convert.ToInt32(Queryitem.Attribute("requestID").Value.Split('_')[0]);
                            objMapData.Custom1 = Queryitem.Attribute("requestID").Value.Split('_')[1];
                            objBL_MapData.UpdateQBTimeTxnIDTicket(objMapData);
                        }
                    }
                    #endregion

                    foreach (XElement Queryitem in xdoc.Descendants("StandardTermsAddRs"))
                    {
                        if (Queryitem.Attribute("statusCode").Value != "0")
                        {
                            SaveErrorCodes(Queryitem.Name.ToString(), Queryitem.Attribute("requestID").Value, Queryitem.Attribute("statusCode").Value, Queryitem.Attribute("statusSeverity").Value, Queryitem.Attribute("statusMessage").Value);
                        }
                    }

                    #endregion
                }
                catch (Exception ex)
                {
                    SaveErrorCodes("Update MOM's data after importing data to QB", "", "", "Error", ex.Message);
                }

                try
                {
                    #region Get Edit Sequence for Updating records at QB

                    Session["dtModCustResponse"] = null;
                    Session["dtModLocResponse"] = null;
                    Session["dtModClassResponse"] = null;
                    Session["dtModTimeSheetResponse"] = null;
                    Session["dtModItemServiceResponse"] = null;
                    Session["dtModItemSalesTAxResponse"] = null;


                    //if (Convert.ToInt32(Session["counter"]) > 4)
                    //{

                    #region SalesTaxitem
                    foreach (XElement Queryitem in xdoc.Descendants("ItemSalesTaxQueryRs"))
                    {
                        if (Queryitem.Attribute("requestID").Value == "4")
                        {
                            DataTable dtModItemSalesTAxResponse = new DataTable();
                            dtModItemSalesTAxResponse.Columns.Add("ListID");
                            dtModItemSalesTAxResponse.Columns.Add("EditSequence");
                            dtModItemSalesTAxResponse.Columns.Add("TimeModified");

                            foreach (XElement item in Queryitem.Descendants("ItemSalesTaxRet"))
                            {
                                DataRow dr = dtModItemSalesTAxResponse.NewRow();
                                dr["ListID"] = item.Element("ListID").Value;
                                dr["EditSequence"] = item.Element("EditSequence").Value;
                                dr["TimeModified"] = item.Element("TimeModified").Value;
                                dtModItemSalesTAxResponse.Rows.Add(dr);
                            }

                            Session["dtModItemSalesTAxResponse"] = dtModItemSalesTAxResponse;
                        }
                    }

                    foreach (XElement Queryitem in xdoc.Descendants("ItemSalesTaxModRs"))
                    {
                        if (Queryitem.Attribute("statusCode").Value != "0")
                        {
                            SaveErrorCodes(Queryitem.Name.ToString(), Queryitem.Attribute("requestID").Value, Queryitem.Attribute("statusCode").Value, Queryitem.Attribute("statusSeverity").Value, Queryitem.Attribute("statusMessage").Value);
                        }
                    }

                    #endregion

                    #region Customer
                    foreach (XElement Queryitem in xdoc.Descendants("CustomerQueryRs"))
                    {
                        if (Queryitem.Attribute("requestID").Value == "2")
                        {
                            DataTable dtModCustResponse = new DataTable();
                            dtModCustResponse.Columns.Add("ListID");
                            dtModCustResponse.Columns.Add("EditSequence");
                            dtModCustResponse.Columns.Add("TimeModified");

                            foreach (XElement item in Queryitem.Descendants("CustomerRet"))
                            {
                                //Session["EditSeqCust"] = item.Element("EditSequence").Value;
                                //Session["ModifyDate"] = item.Element("TimeModified").Value;   
                                DataRow dr = dtModCustResponse.NewRow();
                                dr["ListID"] = item.Element("ListID").Value;
                                dr["EditSequence"] = item.Element("EditSequence").Value;
                                dr["TimeModified"] = item.Element("TimeModified").Value;
                                dtModCustResponse.Rows.Add(dr);
                            }

                            Session["dtModCustResponse"] = dtModCustResponse;
                        }
                        else if (Queryitem.Attribute("requestID").Value == "3")
                        {
                            DataTable dtModLocResponse = new DataTable();
                            dtModLocResponse.Columns.Add("ListID");
                            dtModLocResponse.Columns.Add("EditSequence");
                            dtModLocResponse.Columns.Add("TimeModified");

                            foreach (XElement item in Queryitem.Descendants("CustomerRet"))
                            {
                                DataRow dr = dtModLocResponse.NewRow();
                                dr["ListID"] = item.Element("ListID").Value;
                                dr["EditSequence"] = item.Element("EditSequence").Value;
                                dr["TimeModified"] = item.Element("TimeModified").Value;
                                dtModLocResponse.Rows.Add(dr);
                            }

                            Session["dtModLocResponse"] = dtModLocResponse;
                        }
                    }

                    foreach (XElement Queryitem in xdoc.Descendants("CustomerModRs"))
                    {
                        if (Queryitem.Attribute("statusCode").Value != "0")
                        {
                            SaveErrorCodes(Queryitem.Name.ToString(), Queryitem.Attribute("requestID").Value, Queryitem.Attribute("statusCode").Value, Queryitem.Attribute("statusSeverity").Value, Queryitem.Attribute("statusMessage").Value);
                        }
                    }

                    #endregion

                    #region Class
                    foreach (XElement Queryitem in xdoc.Descendants("ClassQueryRs"))
                    {
                        if (Queryitem.Attribute("requestID").Value == "2")
                        {
                            DataTable dtModClassResponse = new DataTable();
                            dtModClassResponse.Columns.Add("ListID");
                            dtModClassResponse.Columns.Add("EditSequence");
                            dtModClassResponse.Columns.Add("TimeModified");

                            foreach (XElement item in Queryitem.Descendants("ClassRet"))
                            {
                                DataRow dr = dtModClassResponse.NewRow();
                                dr["ListID"] = item.Element("ListID").Value;
                                dr["EditSequence"] = item.Element("EditSequence").Value;
                                dr["TimeModified"] = item.Element("TimeModified").Value;
                                dtModClassResponse.Rows.Add(dr);
                            }

                            Session["dtModClassResponse"] = dtModClassResponse;
                        }
                    }

                    foreach (XElement Queryitem in xdoc.Descendants("ClassModRs"))
                    {
                        if (Queryitem.Attribute("statusCode").Value != "0")
                        {
                            SaveErrorCodes(Queryitem.Name.ToString(), Queryitem.Attribute("requestID").Value, Queryitem.Attribute("statusCode").Value, Queryitem.Attribute("statusSeverity").Value, Queryitem.Attribute("statusMessage").Value);
                        }
                    }

                    #endregion

                    #region Serviceitem
                    foreach (XElement Queryitem in xdoc.Descendants("ItemServiceQueryRs"))
                    {
                        if (Queryitem.Attribute("requestID").Value == "3")
                        {
                            DataTable dtModItemServiceResponse = new DataTable();
                            dtModItemServiceResponse.Columns.Add("ListID");
                            dtModItemServiceResponse.Columns.Add("EditSequence");
                            dtModItemServiceResponse.Columns.Add("TimeModified");

                            foreach (XElement item in Queryitem.Descendants("ItemServiceRet"))
                            {
                                DataRow dr = dtModItemServiceResponse.NewRow();
                                dr["ListID"] = item.Element("ListID").Value;
                                dr["EditSequence"] = item.Element("EditSequence").Value;
                                dr["TimeModified"] = item.Element("TimeModified").Value;
                                dtModItemServiceResponse.Rows.Add(dr);
                            }

                            Session["dtModItemServiceResponse"] = dtModItemServiceResponse;
                        }
                    }

                    foreach (XElement Queryitem in xdoc.Descendants("ItemServiceModRs"))
                    {
                        if (Queryitem.Attribute("statusCode").Value != "0")
                        {
                            SaveErrorCodes(Queryitem.Name.ToString(), Queryitem.Attribute("requestID").Value, Queryitem.Attribute("statusCode").Value, Queryitem.Attribute("statusSeverity").Value, Queryitem.Attribute("statusMessage").Value);
                        }
                    }

                    #endregion

                    #region Timesheet
                    foreach (XElement Queryitem in xdoc.Descendants("TimeTrackingQueryRs"))
                    {
                        if (Queryitem.Attribute("requestID").Value == "2")
                        {
                            DataTable dtModTimeSheetResponse = new DataTable();
                            dtModTimeSheetResponse.Columns.Add("ListID");
                            dtModTimeSheetResponse.Columns.Add("EditSequence");
                            dtModTimeSheetResponse.Columns.Add("TimeModified");

                            foreach (XElement item in Queryitem.Descendants("TimeTrackingRet"))
                            {
                                DataRow dr = dtModTimeSheetResponse.NewRow();
                                dr["ListID"] = item.Element("TxnID").Value;
                                dr["EditSequence"] = item.Element("EditSequence").Value;
                                dr["TimeModified"] = item.Element("TimeModified").Value;
                                dtModTimeSheetResponse.Rows.Add(dr);
                            }

                            Session["dtModTimeSheetResponse"] = dtModTimeSheetResponse;
                        }
                    }

                    foreach (XElement Queryitem in xdoc.Descendants("TimeTrackingModRs"))
                    {
                        if (Queryitem.Attribute("statusCode").Value != "0")
                        {
                            SaveErrorCodes(Queryitem.Name.ToString(), Queryitem.Attribute("requestID").Value, Queryitem.Attribute("statusCode").Value, Queryitem.Attribute("statusSeverity").Value, Queryitem.Attribute("statusMessage").Value);
                        }
                    }

                    #endregion


                    //}
                    #endregion
                }
                catch (Exception ex)
                {
                    SaveErrorCodes("Updating MOM's data", "", "", "Error", ex.Message);
                }
            }
            catch (Exception ex)
            {
                SaveErrorCodes("Parse response XML", "", "", "Error", ex.Message);
            }

            #endregion

            #region Logging and Counter logic
            string evLogTxt = response;
            evLogTxt = evLogTxt + "WebMethod: receiveResponseXML() has been called by QBWebconnector" + "\r\n\r\n";
            evLogTxt = evLogTxt + "Parameters received:\r\n";
            evLogTxt = evLogTxt + "string ticket = " + ticket + "\r\n";
            evLogTxt = evLogTxt + "string response = " + response + "\r\n";
            evLogTxt = evLogTxt + "string hresult = " + hresult + "\r\n";
            evLogTxt = evLogTxt + "string message = " + message + "\r\n";
            evLogTxt = evLogTxt + "\r\n";

            int retVal = 0;
            if (!hresult.ToString().Equals(""))
            {
                // if there is an error with response received, web service could also return a -ve int		
                evLogTxt = evLogTxt + "HRESULT = " + hresult + "\r\n";
                evLogTxt = evLogTxt + "Message = " + message + "\r\n";
                retVal = -101;
            }
            else
            {
                evLogTxt = evLogTxt + "Length of response received = " + response.Length + "\r\n";

                ArrayList req = buildRequest(false);
                int total = req.Count;
                int count = Convert.ToInt32(Session["counter"]);

                if (Convert.ToInt32(Session["counter"]) == (req.Count))
                {
                    //Session["dsNewMSMCustType"] = null;
                    //Session["dsNewMSMLocType"] = null;
                    //Session["dsNewSalesTaxCode"] = null;
                    //Session["dsNewMSMCust"] = null;
                    //Session["dsUpdateCustToQB"] = null;
                    //Session["dsNewMSMLoc"] = null;
                    //Session["dsUpdateLocToQB"] = null;

                    //Session["dtModCustResponse"] = null;
                    //Session["dtModLocResponse"] = null;
                    //Session["dtModClassResponse"] = null;
                    //Session["dtModTimeSheetResponse"] = null;
                    //Session["dtModItemServiceResponse"] = null;
                    //Session["duplicateTermsID"] = null;
                    //Session["dtModItemSalesTAxResponse"] = null;

                    //Session["ClassRet_1"] = null;
                    NullifySessions();

                    if (Convert.ToInt32(Session["firstsync"]) == 0)
                    {
                        objGeneral.ConnConfig = objGenFunctions.ConnectionStr(Connection);
                        objBL_General.UpdateQBLastSync(objGeneral);
                    }
                }

                int percentage = (count * 100) / total;
                if (percentage >= 100)
                {
                    count = 0;
                    Session["counter"] = 0;
                }
                retVal = percentage;
            }
            evLogTxt = evLogTxt + "\r\n";
            evLogTxt = evLogTxt + "Return values: " + "\r\n";
            evLogTxt = evLogTxt + "int retVal= " + retVal.ToString() + "\r\n";
            logEvent(evLogTxt);
            #endregion

            return retVal;
        }

        private string SuffixSpace(string input)
        {
            string output = string.Empty;

            if (input != string.Empty)
            {
                output = input + " ";
            }

            return output;
        }


        [WebMethod]
        /// <summary>
        /// WebMethod - getLastError()
        /// Signature: public string getLastError(string ticket)
        /// 
        /// IN:
        /// string ticket
        /// 
        /// OUT:
        /// string retVal
        /// Possible Values:
        /// Error message describing last web service error
        /// </summary>
        public string getLastError(string ticket)
        {
            string evLogTxt = "WebMethod: getLastError() has been called by QBWebconnector" + "\r\n\r\n";
            evLogTxt = evLogTxt + "Parameters received:\r\n";
            evLogTxt = evLogTxt + "string ticket = " + ticket + "\r\n";
            evLogTxt = evLogTxt + "\r\n";

            int errorCode = 0;
            string retVal = null;
            if (errorCode == -101)
            {
                retVal = "QuickBooks was not running!"; // This is just an example of custom user errors
            }
            else
            {
                retVal = "Error!";
            }
            evLogTxt = evLogTxt + "\r\n";
            evLogTxt = evLogTxt + "Return values: " + "\r\n";
            evLogTxt = evLogTxt + "string retVal= " + retVal + "\r\n";
            logEvent(evLogTxt);
            return retVal;
        }




        [WebMethod]
        /// <summary>
        /// WebMethod - closeConnection()
        /// At the end of a successful update session, QBWebConnector will call this web method.
        /// Signature: public string closeConnection(string ticket)
        /// 
        /// IN:
        /// string ticket 
        /// 
        /// OUT:
        /// string closeConnection result 
        /// </summary>
        public string closeConnection(string ticket)
        {
            string evLogTxt = "WebMethod: closeConnection() has been called by QBWebconnector" + "\r\n\r\n";
            evLogTxt = evLogTxt + "Parameters received:\r\n";
            evLogTxt = evLogTxt + "string ticket = " + ticket + "\r\n";
            evLogTxt = evLogTxt + "\r\n";
            string retVal = null;

            retVal = "OK";

            evLogTxt = evLogTxt + "\r\n";
            evLogTxt = evLogTxt + "Return values: " + "\r\n";
            evLogTxt = evLogTxt + "string retVal= " + retVal + "\r\n";
            logEvent(evLogTxt);
            return retVal;
        }


        #endregion

        #region UtilityMethods
        private void initEvLog()
        {
            try
            {
                string source = "WCWebService";
                if (!System.Diagnostics.EventLog.SourceExists(source))
                    System.Diagnostics.EventLog.CreateEventSource(source, "Application");
                evLog.Source = source;
            }
            catch { };
            return;
        }

        private void logEvent(string logText)
        {
            try
            {
                evLog.WriteEntry(logText);
            }
            catch { };
            return;
        }

        public ArrayList buildRequest(bool isSendingRequest = false)
        {
            string strRequestXML = "";
            XmlDocument inputXMLDoc = null;
            DataSet ds = new DataSet();
            DateTime LastSycnDate = new DateTime();
            String FormatDate = string.Empty;
            String MileageItem = string.Empty;
            String LaborItem = string.Empty;
            String ExpenseItem = string.Empty;
            objProp_User.ConnConfig = objGenFunctions.ConnectionStr(Connection);

            ds = objBL_User.GetControlForQB(objProp_User);
            
            if (ds.Tables[0].Rows[0]["QBLastSync"] != DBNull.Value)
            {
                LastSycnDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["QBLastSync"]);
                FormatDate = LastSycnDate.ToString("yyyy-MM-ddTHH:mm:ss");
            }
            if (ds.Tables[0].Rows[0]["QBServiceItem"] != DBNull.Value)
            {
                MileageItem = ds.Tables[0].Rows[0]["QBServiceItem"].ToString().Trim();
            }
            if (ds.Tables[0].Rows[0]["QBServiceItemLabor"] != DBNull.Value)
            {
                LaborItem = ds.Tables[0].Rows[0]["QBServiceItemLabor"].ToString().Trim();
            }
            if (ds.Tables[0].Rows[0]["QBServiceItemExp"] != DBNull.Value)
            {
                ExpenseItem = ds.Tables[0].Rows[0]["QBServiceItemExp"].ToString().Trim();
            }

            XmlElement qbXML;
            XmlElement qbXMLMsgsRq;


            //int firstsync = 1;
            //int TransferTimesheet = 0;
            int TransferInvoice = 0;
            int ImportInvoice = 0;
            int QBWCSyncDirection = 0;
            if (!string.IsNullOrEmpty(strImportInvoice))
            {
                ImportInvoice = Convert.ToInt16(strImportInvoice);
            }
            if (!string.IsNullOrEmpty(strTransferInvoice))
            {
                TransferInvoice = Convert.ToInt16(strTransferInvoice);
            }
            if (!string.IsNullOrEmpty(strQBWCSyncDirection))
            {
                QBWCSyncDirection = Convert.ToInt16(strQBWCSyncDirection);
            }
            //DataSet dsSyncItems = objBL_User.GetSyncItems(objProp_User);
            //if (dsSyncItems.Tables[0].Rows.Count > 0)
            //{
            //    firstsync = Convert.ToInt16(dsSyncItems.Tables[0].Rows[0]["QBFirstSync"].ToString());
            //    TransferTimesheet = Convert.ToInt16(dsSyncItems.Tables[0].Rows[0]["SyncTimesheet"].ToString());
            //    TransferInvoice = Convert.ToInt16(dsSyncItems.Tables[0].Rows[0]["SyncInvoice"].ToString());
            //}

            int firstsync = objBL_User.GetUserSyncStatus(objProp_User);
            Session["firstsync"] = firstsync;


            // 1 Employee Query
            #region Employee Query
            if (QBWCSyncDirection == 0 || QBWCSyncDirection == 1)
            {
                inputXMLDoc = new XmlDocument();
                inputXMLDoc.AppendChild(inputXMLDoc.CreateXmlDeclaration("1.0", "ISO-8859-1", null));
                inputXMLDoc.AppendChild(inputXMLDoc.CreateProcessingInstruction("qbxml", "version=\"" + qbxmlversion + "\""));

                qbXML = inputXMLDoc.CreateElement("QBXML");
                inputXMLDoc.AppendChild(qbXML);
                qbXMLMsgsRq = inputXMLDoc.CreateElement("QBXMLMsgsRq");
                qbXML.AppendChild(qbXMLMsgsRq);
                qbXMLMsgsRq.SetAttribute("onError", "continueOnError");
                XmlElement EmployeeQueryRq = inputXMLDoc.CreateElement("EmployeeQueryRq");
                qbXMLMsgsRq.AppendChild(EmployeeQueryRq);
                EmployeeQueryRq.SetAttribute("requestID", "1");

                /* ActiveStatus may have one of the following values: ActiveOnly [DEFAULT], InactiveOnly, All */
                XmlElement ActiveStatus = inputXMLDoc.CreateElement("ActiveStatus");
                EmployeeQueryRq.AppendChild(ActiveStatus).InnerText = "All";

                if (!string.IsNullOrEmpty(FormatDate))
                {
                    XmlElement TimeModified = inputXMLDoc.CreateElement("FromModifiedDate");
                    EmployeeQueryRq.AppendChild(TimeModified).InnerText = FormatDate;
                }

                strRequestXML = inputXMLDoc.OuterXml;
                req.Add(strRequestXML);

                // Clean up
                strRequestXML = "";
                inputXMLDoc = null;
                qbXML = null;
                qbXMLMsgsRq = null;
                //maxReturned = null;
            }
            #endregion

            if (firstsync == 0)
            {
                if (QBWCSyncDirection == 0 || QBWCSyncDirection == 1)
                {
                    // 2 CustomerType Query
                    #region CustomerType Query
                    inputXMLDoc = new XmlDocument();
                    inputXMLDoc.AppendChild(inputXMLDoc.CreateXmlDeclaration("1.0", "ISO-8859-1", null));
                    inputXMLDoc.AppendChild(inputXMLDoc.CreateProcessingInstruction("qbxml", "version=\"" + qbxmlversion + "\""));

                    qbXML = inputXMLDoc.CreateElement("QBXML");
                    inputXMLDoc.AppendChild(qbXML);
                    qbXMLMsgsRq = inputXMLDoc.CreateElement("QBXMLMsgsRq");
                    qbXML.AppendChild(qbXMLMsgsRq);
                    qbXMLMsgsRq.SetAttribute("onError", "continueOnError");
                    XmlElement CustomerTypeQueryRq = inputXMLDoc.CreateElement("CustomerTypeQueryRq"); //CustomerQueryRq CustomerTypeQueryRq JobTypeQueryRq SalesTaxCodeQueryRq
                    qbXMLMsgsRq.AppendChild(CustomerTypeQueryRq);
                    CustomerTypeQueryRq.SetAttribute("requestID", "1");

                    if (!string.IsNullOrEmpty(FormatDate))
                    {
                        XmlElement TimeModified = inputXMLDoc.CreateElement("FromModifiedDate");
                        CustomerTypeQueryRq.AppendChild(TimeModified).InnerText = FormatDate;
                    }

                    strRequestXML = inputXMLDoc.OuterXml;
                    req.Add(strRequestXML);

                    // Clean up
                    strRequestXML = "";
                    inputXMLDoc = null;
                    qbXML = null;
                    qbXMLMsgsRq = null;
                    //maxReturned=null;
                    #endregion

                    // 3 Job Type Query
                    #region Job Type Query
                    inputXMLDoc = new XmlDocument();
                    inputXMLDoc.AppendChild(inputXMLDoc.CreateXmlDeclaration("1.0", "ISO-8859-1", null));
                    inputXMLDoc.AppendChild(inputXMLDoc.CreateProcessingInstruction("qbxml", "version=\"" + qbxmlversion + "\""));

                    qbXML = inputXMLDoc.CreateElement("QBXML");
                    inputXMLDoc.AppendChild(qbXML);
                    qbXMLMsgsRq = inputXMLDoc.CreateElement("QBXMLMsgsRq");
                    qbXML.AppendChild(qbXMLMsgsRq);
                    qbXMLMsgsRq.SetAttribute("onError", "continueOnError");
                    XmlElement JobTypeQueryRq = inputXMLDoc.CreateElement("JobTypeQueryRq");
                    qbXMLMsgsRq.AppendChild(JobTypeQueryRq);
                    JobTypeQueryRq.SetAttribute("requestID", "2");
                    //maxReturned = inputXMLDoc.CreateElement("MaxReturned");
                    //invoiceQueryRq.AppendChild(maxReturned).InnerText = "1";
                    if (!string.IsNullOrEmpty(FormatDate))
                    {
                        XmlElement JobTimeModified = inputXMLDoc.CreateElement("FromModifiedDate");
                        JobTypeQueryRq.AppendChild(JobTimeModified).InnerText = FormatDate;
                    }
                    strRequestXML = inputXMLDoc.OuterXml;
                    req.Add(strRequestXML);

                    // Clean up
                    strRequestXML = "";
                    inputXMLDoc = null;
                    qbXML = null;
                    qbXMLMsgsRq = null;
                    //maxReturned = null;

                    #endregion

                    // 4 Sales Tax Query
                    #region Salestax Query
                    inputXMLDoc = new XmlDocument();
                    inputXMLDoc.AppendChild(inputXMLDoc.CreateXmlDeclaration("1.0", "ISO-8859-1", null));
                    inputXMLDoc.AppendChild(inputXMLDoc.CreateProcessingInstruction("qbxml", "version=\"" + qbxmlversion + "\""));

                    qbXML = inputXMLDoc.CreateElement("QBXML");
                    inputXMLDoc.AppendChild(qbXML);
                    qbXMLMsgsRq = inputXMLDoc.CreateElement("QBXMLMsgsRq");
                    qbXML.AppendChild(qbXMLMsgsRq);
                    qbXMLMsgsRq.SetAttribute("onError", "continueOnError");
                    //XmlElement SalesTaxCodeQueryRq = inputXMLDoc.CreateElement("SalesTaxCodeQueryRq");
                    XmlElement SalesTaxCodeQueryRq = inputXMLDoc.CreateElement("ItemSalesTaxQueryRq");
                    qbXMLMsgsRq.AppendChild(SalesTaxCodeQueryRq);
                    SalesTaxCodeQueryRq.SetAttribute("requestID", "3");
                    //maxReturned = inputXMLDoc.CreateElement("MaxReturned");
                    //billQueryRq.AppendChild(maxReturned).InnerText = "1";

                    ///* ActiveStatus may have one of the following values: ActiveOnly [DEFAULT], InactiveOnly, All */
                    //XmlElement STActiveStatus = inputXMLDoc.CreateElement("ActiveStatus");
                    //SalesTaxCodeQueryRq.AppendChild(STActiveStatus).InnerText = "All";

                    if (!string.IsNullOrEmpty(FormatDate))
                    {
                        XmlElement STTimeModified = inputXMLDoc.CreateElement("FromModifiedDate");
                        SalesTaxCodeQueryRq.AppendChild(STTimeModified).InnerText = FormatDate;
                    }
                    strRequestXML = inputXMLDoc.OuterXml;
                    req.Add(strRequestXML);

                    // Clean up
                    strRequestXML = "";
                    inputXMLDoc = null;
                    qbXML = null;
                    qbXMLMsgsRq = null;
                    //maxReturned = null;
                    #endregion

                    // 5 Customer Query
                    #region Customer Query
                    inputXMLDoc = new XmlDocument();
                    inputXMLDoc.AppendChild(inputXMLDoc.CreateXmlDeclaration("1.0", "ISO-8859-1", null));
                    inputXMLDoc.AppendChild(inputXMLDoc.CreateProcessingInstruction("qbxml", "version=\"" + qbxmlversion + "\""));

                    qbXML = inputXMLDoc.CreateElement("QBXML");
                    inputXMLDoc.AppendChild(qbXML);
                    qbXMLMsgsRq = inputXMLDoc.CreateElement("QBXMLMsgsRq");
                    qbXML.AppendChild(qbXMLMsgsRq);
                    qbXMLMsgsRq.SetAttribute("onError", "continueOnError");
                    XmlElement CustomerQueryRq = inputXMLDoc.CreateElement("CustomerQueryRq");
                    qbXMLMsgsRq.AppendChild(CustomerQueryRq);
                    CustomerQueryRq.SetAttribute("requestID", "1");
                    CustomerQueryRq.SetAttribute("iterator", "Start");
                    XmlElement CustomerMaxReturn = inputXMLDoc.CreateElement("MaxReturned");
                    CustomerQueryRq.AppendChild(CustomerMaxReturn).InnerText = "100";
                    /* ActiveStatus may have one of the following values: ActiveOnly [DEFAULT], InactiveOnly, All */
                    XmlElement CustActiveStatus = inputXMLDoc.CreateElement("ActiveStatus");
                    CustomerQueryRq.AppendChild(CustActiveStatus).InnerText = "All";

                    if (!string.IsNullOrEmpty(FormatDate))
                    {
                        XmlElement CustTimeModified = inputXMLDoc.CreateElement("FromModifiedDate");
                        CustomerQueryRq.AppendChild(CustTimeModified).InnerText = FormatDate;
                        ////string datee = "2013-01-01T00:00:00";
                        ////CustomerQueryRq.AppendChild(CustTimeModified).InnerText = datee;
                        //XmlElement CustTimeto = inputXMLDoc.CreateElement("ToModifiedDate");
                        //CustomerQueryRq.AppendChild(CustTimeto).InnerText = "2010-12-31T00:00:00"; ;

                    }
                    string[] retElements = new string[20] {
                        "IsActive"
                        , "TimeModified"
                        , "Name"
                        , "CompanyName"
                        , "Notes"
                        , "FirstName"
                        , "MiddleName"
                        , "LastName"
                        , "Phone"
                        , "Email"
                        , "ListID"
                        , "Sublevel"
                        , "Fax"
                        , "Balance"
                        , "CustomerTypeRef"
                        , "JobTypeRef"
                        , "ParentRef"
                        , "ItemSalesTaxRef"
                        , "BillAddress"
                        , "ShipAddress"
                    };

                    for (int i = 0; i < retElements.Length; i++)
                    {
                        XmlElement IncludeRetElement = inputXMLDoc.CreateElement("IncludeRetElement");
                        CustomerQueryRq.AppendChild(IncludeRetElement).InnerText = retElements[i];
                    }
                    strRequestXML = inputXMLDoc.OuterXml;
                    req.Add(strRequestXML);

                    // Clean up
                    strRequestXML = "";
                    inputXMLDoc = null;
                    qbXML = null;
                    qbXMLMsgsRq = null;
                    //maxReturned = null;

                    if (Session["CustomerIteratorIDs"] != null)
                    {
                        List<string> CustomerIteratorIDs = (List<string>)Session["CustomerIteratorIDs"];
                        foreach (var item in CustomerIteratorIDs)
                        {
                            inputXMLDoc = new XmlDocument();
                            inputXMLDoc.AppendChild(inputXMLDoc.CreateXmlDeclaration("1.0", "ISO-8859-1", null));
                            inputXMLDoc.AppendChild(inputXMLDoc.CreateProcessingInstruction("qbxml", "version=\"" + qbxmlversion + "\""));

                            qbXML = inputXMLDoc.CreateElement("QBXML");
                            inputXMLDoc.AppendChild(qbXML);
                            qbXMLMsgsRq = inputXMLDoc.CreateElement("QBXMLMsgsRq");
                            qbXML.AppendChild(qbXMLMsgsRq);
                            qbXMLMsgsRq.SetAttribute("onError", "continueOnError");
                            CustomerQueryRq = inputXMLDoc.CreateElement("CustomerQueryRq");
                            qbXMLMsgsRq.AppendChild(CustomerQueryRq);
                            CustomerQueryRq.SetAttribute("requestID", "1");
                            CustomerQueryRq.SetAttribute("iterator", "Continue");
                            CustomerQueryRq.SetAttribute("iteratorID", item);
                            CustomerMaxReturn = inputXMLDoc.CreateElement("MaxReturned");
                            CustomerQueryRq.AppendChild(CustomerMaxReturn).InnerText = "100";

                            for (int i = 0; i < retElements.Length; i++)
                            {
                                XmlElement IncludeRetElement = inputXMLDoc.CreateElement("IncludeRetElement");
                                CustomerQueryRq.AppendChild(IncludeRetElement).InnerText = retElements[i];
                            }
                            strRequestXML = inputXMLDoc.OuterXml;
                            req.Add(strRequestXML);
                            // Clean up
                            strRequestXML = "";
                            inputXMLDoc = null;
                            qbXML = null;
                            qbXMLMsgsRq = null;
                        }
                    }

                    #endregion

                    // 6 Class Query
                    #region Class Query
                    inputXMLDoc = new XmlDocument();
                    inputXMLDoc.AppendChild(inputXMLDoc.CreateXmlDeclaration("1.0", "ISO-8859-1", null));
                    inputXMLDoc.AppendChild(inputXMLDoc.CreateProcessingInstruction("qbxml", "version=\"" + qbxmlversion + "\""));

                    qbXML = inputXMLDoc.CreateElement("QBXML");
                    inputXMLDoc.AppendChild(qbXML);
                    qbXMLMsgsRq = inputXMLDoc.CreateElement("QBXMLMsgsRq");
                    qbXML.AppendChild(qbXMLMsgsRq);
                    qbXMLMsgsRq.SetAttribute("onError", "continueOnError");
                    XmlElement ClassQueryRq = inputXMLDoc.CreateElement("ClassQueryRq");
                    qbXMLMsgsRq.AppendChild(ClassQueryRq);
                    ClassQueryRq.SetAttribute("requestID", "1");
                    //maxReturned = inputXMLDoc.CreateElement("MaxReturned");
                    //invoiceQueryRq.AppendChild(maxReturned).InnerText = "1";
                    if (!string.IsNullOrEmpty(FormatDate))
                    {
                        XmlElement ClassTimeModified = inputXMLDoc.CreateElement("FromModifiedDate");
                        ClassQueryRq.AppendChild(ClassTimeModified).InnerText = FormatDate;
                    }
                    strRequestXML = inputXMLDoc.OuterXml;
                    req.Add(strRequestXML);

                    // Clean up
                    strRequestXML = "";
                    inputXMLDoc = null;
                    qbXML = null;
                    qbXMLMsgsRq = null;
                    //maxReturned = null;

                    #endregion

                    // 7 Item/Billingcode Query
                    #region ServiceItem/serviceBillingcode Query
                    inputXMLDoc = new XmlDocument();
                    inputXMLDoc.AppendChild(inputXMLDoc.CreateXmlDeclaration("1.0", "ISO-8859-1", null));
                    inputXMLDoc.AppendChild(inputXMLDoc.CreateProcessingInstruction("qbxml", "version=\"" + qbxmlversion + "\""));

                    qbXML = inputXMLDoc.CreateElement("QBXML");
                    inputXMLDoc.AppendChild(qbXML);
                    qbXMLMsgsRq = inputXMLDoc.CreateElement("QBXMLMsgsRq");
                    qbXML.AppendChild(qbXMLMsgsRq);
                    qbXMLMsgsRq.SetAttribute("onError", "continueOnError");
                    XmlElement ItemQueryRq = inputXMLDoc.CreateElement("ItemQueryRq");
                    qbXMLMsgsRq.AppendChild(ItemQueryRq);
                    ItemQueryRq.SetAttribute("requestID", "1");
                    ItemQueryRq.SetAttribute("iterator", "Start");
                    XmlElement ItemMaxReturned = inputXMLDoc.CreateElement("MaxReturned");
                    ItemQueryRq.AppendChild(ItemMaxReturned).InnerText = "100";

                    ///* ActiveStatus may have one of the following values: ActiveOnly [DEFAULT], InactiveOnly, All */
                    //XmlElement itemActiveStatus = inputXMLDoc.CreateElement("ActiveStatus");
                    //ItemQueryRq.AppendChild(itemActiveStatus).InnerText = "All";

                    if (!string.IsNullOrEmpty(FormatDate))
                    {
                        XmlElement TimeModified = inputXMLDoc.CreateElement("FromModifiedDate");
                        ItemQueryRq.AppendChild(TimeModified).InnerText = FormatDate;
                    }
                    strRequestXML = inputXMLDoc.OuterXml;
                    req.Add(strRequestXML);

                    // Clean up
                    strRequestXML = "";
                    inputXMLDoc = null;
                    qbXML = null;
                    qbXMLMsgsRq = null;

                    if (Session["ItemIteratorIDs"] != null)
                    {
                        List<string> ItemIteratorIDs = (List<string>)Session["ItemIteratorIDs"];
                        foreach (var item in ItemIteratorIDs)
                        {
                            inputXMLDoc = new XmlDocument();
                            inputXMLDoc.AppendChild(inputXMLDoc.CreateXmlDeclaration("1.0", "ISO-8859-1", null));
                            inputXMLDoc.AppendChild(inputXMLDoc.CreateProcessingInstruction("qbxml", "version=\"" + qbxmlversion + "\""));

                            qbXML = inputXMLDoc.CreateElement("QBXML");
                            inputXMLDoc.AppendChild(qbXML);
                            qbXMLMsgsRq = inputXMLDoc.CreateElement("QBXMLMsgsRq");
                            qbXML.AppendChild(qbXMLMsgsRq);
                            qbXMLMsgsRq.SetAttribute("onError", "continueOnError");
                            ItemQueryRq = inputXMLDoc.CreateElement("ItemQueryRq");
                            qbXMLMsgsRq.AppendChild(ItemQueryRq);
                            ItemQueryRq.SetAttribute("requestID", "1");
                            ItemQueryRq.SetAttribute("iterator", "Continue");
                            ItemQueryRq.SetAttribute("iteratorID", item);
                            ItemMaxReturned = inputXMLDoc.CreateElement("MaxReturned");
                            ItemQueryRq.AppendChild(ItemMaxReturned).InnerText = "100";

                            strRequestXML = inputXMLDoc.OuterXml;
                            req.Add(strRequestXML);
                            // Clean up
                            strRequestXML = "";
                            inputXMLDoc = null;
                            qbXML = null;
                            qbXMLMsgsRq = null;
                        }
                    }

                    #endregion

                    // 8 Terms Query
                    #region Terms Query
                    inputXMLDoc = new XmlDocument();
                    inputXMLDoc.AppendChild(inputXMLDoc.CreateXmlDeclaration("1.0", "ISO-8859-1", null));
                    inputXMLDoc.AppendChild(inputXMLDoc.CreateProcessingInstruction("qbxml", "version=\"" + qbxmlversion + "\""));

                    qbXML = inputXMLDoc.CreateElement("QBXML");
                    inputXMLDoc.AppendChild(qbXML);
                    qbXMLMsgsRq = inputXMLDoc.CreateElement("QBXMLMsgsRq");
                    qbXML.AppendChild(qbXMLMsgsRq);
                    qbXMLMsgsRq.SetAttribute("onError", "continueOnError");
                    XmlElement TermsQueryRq = inputXMLDoc.CreateElement("TermsQueryRq");
                    qbXMLMsgsRq.AppendChild(TermsQueryRq);
                    TermsQueryRq.SetAttribute("requestID", "1");
                    if (!string.IsNullOrEmpty(FormatDate))
                    {
                        XmlElement TimeModified = inputXMLDoc.CreateElement("FromModifiedDate");
                        TermsQueryRq.AppendChild(TimeModified).InnerText = FormatDate;
                    }
                    strRequestXML = inputXMLDoc.OuterXml;
                    req.Add(strRequestXML);

                    // Clean up
                    strRequestXML = "";
                    inputXMLDoc = null;
                    qbXML = null;
                    qbXMLMsgsRq = null;
                    //maxReturned = null;

                    #endregion

                    // 9 PayrollWage Query
                    #region PayrollWage Query
                    inputXMLDoc = new XmlDocument();
                    inputXMLDoc.AppendChild(inputXMLDoc.CreateXmlDeclaration("1.0", "ISO-8859-1", null));
                    inputXMLDoc.AppendChild(inputXMLDoc.CreateProcessingInstruction("qbxml", "version=\"" + qbxmlversion + "\""));

                    qbXML = inputXMLDoc.CreateElement("QBXML");
                    inputXMLDoc.AppendChild(qbXML);
                    qbXMLMsgsRq = inputXMLDoc.CreateElement("QBXMLMsgsRq");
                    qbXML.AppendChild(qbXMLMsgsRq);
                    qbXMLMsgsRq.SetAttribute("onError", "continueOnError");
                    XmlElement PayrollItemWageQueryRq = inputXMLDoc.CreateElement("PayrollItemWageQueryRq");
                    qbXMLMsgsRq.AppendChild(PayrollItemWageQueryRq);
                    PayrollItemWageQueryRq.SetAttribute("requestID", "1");
                    if (!string.IsNullOrEmpty(FormatDate))
                    {
                        XmlElement TimeModified = inputXMLDoc.CreateElement("FromModifiedDate");
                        PayrollItemWageQueryRq.AppendChild(TimeModified).InnerText = FormatDate;
                    }
                    strRequestXML = inputXMLDoc.OuterXml;
                    req.Add(strRequestXML);

                    // Clean up
                    strRequestXML = "";
                    inputXMLDoc = null;
                    qbXML = null;
                    qbXMLMsgsRq = null;
                    //maxReturned = null;
                    #endregion
                }

                // 10 Invoice Query
                #region Invoice Query
                if (ImportInvoice == 1)
                {
                    inputXMLDoc = new XmlDocument();
                    inputXMLDoc.AppendChild(inputXMLDoc.CreateXmlDeclaration("1.0", "ISO-8859-1", null));
                    inputXMLDoc.AppendChild(inputXMLDoc.CreateProcessingInstruction("qbxml", "version=\"" + qbxmlversion + "\""));

                    qbXML = inputXMLDoc.CreateElement("QBXML");
                    inputXMLDoc.AppendChild(qbXML);
                    qbXMLMsgsRq = inputXMLDoc.CreateElement("QBXMLMsgsRq");
                    qbXML.AppendChild(qbXMLMsgsRq);
                    qbXMLMsgsRq.SetAttribute("onError", "continueOnError");
                    XmlElement InvoiceQueryRq = inputXMLDoc.CreateElement("InvoiceQueryRq");
                    qbXMLMsgsRq.AppendChild(InvoiceQueryRq);
                    InvoiceQueryRq.SetAttribute("requestID", "2");
                    InvoiceQueryRq.SetAttribute("iterator", "Start");
                    XmlElement InvoiceMaxReturned = inputXMLDoc.CreateElement("MaxReturned");
                    InvoiceQueryRq.AppendChild(InvoiceMaxReturned).InnerText = "100";

                    if (!string.IsNullOrEmpty(FormatDate))
                    {
                        XmlElement ModifiedDateRangeFilter = inputXMLDoc.CreateElement("ModifiedDateRangeFilter");
                        InvoiceQueryRq.AppendChild(ModifiedDateRangeFilter);
                        XmlElement TimeModified = inputXMLDoc.CreateElement("FromModifiedDate");
                        ModifiedDateRangeFilter.AppendChild(TimeModified).InnerText = FormatDate;
                        //XmlElement ToModifiedDate = inputXMLDoc.CreateElement("ToModifiedDate");
                        //ModifiedDateRangeFilter.AppendChild(ToModifiedDate).InnerText = "2010-12-31T00:00:00"; 
                    }
                    XmlElement IncludeLineItems = inputXMLDoc.CreateElement("IncludeLineItems");
                    InvoiceQueryRq.AppendChild(IncludeLineItems).InnerText = "true";

                    strRequestXML = inputXMLDoc.OuterXml;
                    req.Add(strRequestXML);

                    // Clean up
                    strRequestXML = "";
                    inputXMLDoc = null;
                    qbXML = null;
                    qbXMLMsgsRq = null;

                    if (Session["InvoiceIteratorIDs"] != null)
                    {
                        List<string> InvoiceIteratorIDs = (List<string>)Session["InvoiceIteratorIDs"];
                        foreach (var item in InvoiceIteratorIDs)
                        {
                            inputXMLDoc = new XmlDocument();
                            inputXMLDoc.AppendChild(inputXMLDoc.CreateXmlDeclaration("1.0", "ISO-8859-1", null));
                            inputXMLDoc.AppendChild(inputXMLDoc.CreateProcessingInstruction("qbxml", "version=\"" + qbxmlversion + "\""));

                            qbXML = inputXMLDoc.CreateElement("QBXML");
                            inputXMLDoc.AppendChild(qbXML);
                            qbXMLMsgsRq = inputXMLDoc.CreateElement("QBXMLMsgsRq");
                            qbXML.AppendChild(qbXMLMsgsRq);
                            qbXMLMsgsRq.SetAttribute("onError", "continueOnError");
                            InvoiceQueryRq = inputXMLDoc.CreateElement("InvoiceQueryRq");
                            qbXMLMsgsRq.AppendChild(InvoiceQueryRq);
                            InvoiceQueryRq.SetAttribute("requestID", "2");
                            InvoiceQueryRq.SetAttribute("iterator", "Continue");
                            InvoiceQueryRq.SetAttribute("iteratorID", item);
                            InvoiceMaxReturned = inputXMLDoc.CreateElement("MaxReturned");
                            InvoiceQueryRq.AppendChild(InvoiceMaxReturned).InnerText = "100";
                            IncludeLineItems = inputXMLDoc.CreateElement("IncludeLineItems");
                            InvoiceQueryRq.AppendChild(IncludeLineItems).InnerText = "true";
                            strRequestXML = inputXMLDoc.OuterXml;
                            req.Add(strRequestXML);
                            // Clean up
                            strRequestXML = "";
                            inputXMLDoc = null;
                            qbXML = null;
                            qbXMLMsgsRq = null;
                        }
                    }
                }

                #endregion

                #region Export data to Quickbooks
                if (QBWCSyncDirection == 0 || QBWCSyncDirection == 2)
                {
                    // 11 Add Default Account to QB
                    #region Add Default Account to QB

                    DataSet dsNewMSMAccount = new DataSet();
                    objProp_User.ConnConfig = objGenFunctions.ConnectionStr(Connection);
                    dsNewMSMAccount = objBL_User.getMSMAccount(objProp_User);

                    inputXMLDoc = new XmlDocument();
                    inputXMLDoc.AppendChild(inputXMLDoc.CreateXmlDeclaration("1.0", "ISO-8859-1", null));
                    inputXMLDoc.AppendChild(inputXMLDoc.CreateProcessingInstruction("qbxml", "version=\"" + qbxmlversion + "\""));
                    qbXML = inputXMLDoc.CreateElement("QBXML");
                    inputXMLDoc.AppendChild(qbXML);
                    qbXMLMsgsRq = inputXMLDoc.CreateElement("QBXMLMsgsRq");
                    qbXML.AppendChild(qbXMLMsgsRq);
                    qbXMLMsgsRq.SetAttribute("onError", "continueOnError");

                    if (dsNewMSMAccount.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dsNewMSMAccount.Tables[0].Rows)
                        {
                            XmlElement AccountAddRq = inputXMLDoc.CreateElement("AccountAddRq");
                            qbXMLMsgsRq.AppendChild(AccountAddRq);
                            AccountAddRq.SetAttribute("requestID", dr["id"].ToString());
                            XmlElement AccountAdd = inputXMLDoc.CreateElement("AccountAdd");
                            AccountAddRq.AppendChild(AccountAdd);
                            AccountAdd.AppendChild(inputXMLDoc.CreateElement("Name")).InnerText = objGenFunctions.QBEncode(dr["fdesc"].ToString(), 31);
                            AccountAdd.AppendChild(inputXMLDoc.CreateElement("AccountType")).InnerText = "Expense";
                        }
                    }
                    strRequestXML = inputXMLDoc.OuterXml;
                    req.Add(strRequestXML);
                    #endregion

                    // 12 Add Default Payroll Item to QB
                    #region Add Default Payroll Item to QB

                    DataSet dsNewMSMPayrollItem = new DataSet();
                    objProp_User.ConnConfig = objGenFunctions.ConnectionStr(Connection);
                    dsNewMSMPayrollItem = objBL_User.getMSMPatrollWage(objProp_User);

                    inputXMLDoc = new XmlDocument();
                    inputXMLDoc.AppendChild(inputXMLDoc.CreateXmlDeclaration("1.0", "ISO-8859-1", null));
                    inputXMLDoc.AppendChild(inputXMLDoc.CreateProcessingInstruction("qbxml", "version=\"" + qbxmlversion + "\""));
                    qbXML = inputXMLDoc.CreateElement("QBXML");
                    inputXMLDoc.AppendChild(qbXML);
                    qbXMLMsgsRq = inputXMLDoc.CreateElement("QBXMLMsgsRq");
                    qbXML.AppendChild(qbXMLMsgsRq);
                    qbXMLMsgsRq.SetAttribute("onError", "continueOnError");
                    if (dsNewMSMPayrollItem.Tables[0].Rows.Count > 0)
                    {


                        foreach (DataRow dr in dsNewMSMPayrollItem.Tables[0].Rows)
                        {
                            XmlElement PayrollItemWageAddRq = inputXMLDoc.CreateElement("PayrollItemWageAddRq");
                            qbXMLMsgsRq.AppendChild(PayrollItemWageAddRq);
                            PayrollItemWageAddRq.SetAttribute("requestID", dr["id"].ToString());
                            XmlElement PayrollItemWageAdd = inputXMLDoc.CreateElement("PayrollItemWageAdd");
                            PayrollItemWageAddRq.AppendChild(PayrollItemWageAdd);
                            PayrollItemWageAdd.AppendChild(inputXMLDoc.CreateElement("Name")).InnerText = dr["fdesc"].ToString();
                            PayrollItemWageAdd.AppendChild(inputXMLDoc.CreateElement("WageType")).InnerText = "HourlyRegular";
                            XmlElement ExpenseAccountRef = inputXMLDoc.CreateElement("ExpenseAccountRef");
                            PayrollItemWageAdd.AppendChild(ExpenseAccountRef);
                            ExpenseAccountRef.AppendChild(inputXMLDoc.CreateElement("ListID")).InnerText = dr["qbaccountid"].ToString();
                        }

                    }
                    strRequestXML = inputXMLDoc.OuterXml;
                    req.Add(strRequestXML);
                    #endregion

                    // 13 Add Default Vendor to QB
                    #region Add Default Vendor to QB

                    DataSet dsNewMSMVendor = new DataSet();
                    objProp_User.ConnConfig = objGenFunctions.ConnectionStr(Connection);
                    dsNewMSMVendor = objBL_User.getMSMVendor(objProp_User);

                    inputXMLDoc = new XmlDocument();
                    inputXMLDoc.AppendChild(inputXMLDoc.CreateXmlDeclaration("1.0", "ISO-8859-1", null));
                    inputXMLDoc.AppendChild(inputXMLDoc.CreateProcessingInstruction("qbxml", "version=\"" + qbxmlversion + "\""));
                    qbXML = inputXMLDoc.CreateElement("QBXML");
                    inputXMLDoc.AppendChild(qbXML);
                    qbXMLMsgsRq = inputXMLDoc.CreateElement("QBXMLMsgsRq");
                    qbXML.AppendChild(qbXMLMsgsRq);
                    qbXMLMsgsRq.SetAttribute("onError", "continueOnError");

                    if (dsNewMSMVendor.Tables[0].Rows.Count > 0)
                    {


                        foreach (DataRow dr in dsNewMSMVendor.Tables[0].Rows)
                        {
                            XmlElement VendorAddRq = inputXMLDoc.CreateElement("VendorAddRq");
                            qbXMLMsgsRq.AppendChild(VendorAddRq);
                            VendorAddRq.SetAttribute("requestID", dr["id"].ToString());
                            XmlElement VendorAdd = inputXMLDoc.CreateElement("VendorAdd");
                            VendorAddRq.AppendChild(VendorAdd);
                            VendorAdd.AppendChild(inputXMLDoc.CreateElement("Name")).InnerText = dr["acct"].ToString();
                        }

                    }
                    strRequestXML = inputXMLDoc.OuterXml;
                    req.Add(strRequestXML);

                    #endregion

                    // 14 Export/Add customersType to QB
                    #region Export/Add customersType to QB

                    DataSet dsNewMSMCustType = new DataSet();
                    //if (Session["dsNewMSMCustType"] == null)
                    //{
                    objProp_User.ConnConfig = objGenFunctions.ConnectionStr(Connection);
                    dsNewMSMCustType = objBL_User.getMSMCustomertype(objProp_User);
                    //    Session["dsNewMSMCustType"] = dsNewMSMCustType;
                    //}
                    //else
                    //{
                    //    dsNewMSMCustType = (DataSet)Session["dsNewMSMCustType"];
                    //}
                    inputXMLDoc = new XmlDocument();
                    inputXMLDoc.AppendChild(inputXMLDoc.CreateXmlDeclaration("1.0", "ISO-8859-1", null));
                    inputXMLDoc.AppendChild(inputXMLDoc.CreateProcessingInstruction("qbxml", "version=\"" + qbxmlversion + "\""));
                    qbXML = inputXMLDoc.CreateElement("QBXML");
                    inputXMLDoc.AppendChild(qbXML);
                    qbXMLMsgsRq = inputXMLDoc.CreateElement("QBXMLMsgsRq");
                    qbXML.AppendChild(qbXMLMsgsRq);
                    qbXMLMsgsRq.SetAttribute("onError", "continueOnError");

                    if (dsNewMSMCustType.Tables[0].Rows.Count > 0)
                    {


                        foreach (DataRow dr in dsNewMSMCustType.Tables[0].Rows)
                        {
                            XmlElement CustomerTypeAddRq = inputXMLDoc.CreateElement("CustomerTypeAddRq");
                            qbXMLMsgsRq.AppendChild(CustomerTypeAddRq);
                            CustomerTypeAddRq.SetAttribute("requestID", dr["type"].ToString());
                            XmlElement CustomerTypeAdd = inputXMLDoc.CreateElement("CustomerTypeAdd");
                            CustomerTypeAddRq.AppendChild(CustomerTypeAdd);
                            CustomerTypeAdd.AppendChild(inputXMLDoc.CreateElement("Name")).InnerText = objGenFunctions.QBEncode(dr["type"].ToString(), 31);
                        }

                    }
                    strRequestXML = inputXMLDoc.OuterXml;
                    req.Add(strRequestXML);
                    #endregion

                    // 15 Export/Add LocationType to QB
                    #region Export/Add LocationType to QB

                    DataSet dsNewMSMLocType = new DataSet();
                    //if (Session["dsNewMSMLocType"] == null)
                    //{
                    objProp_User.ConnConfig = objGenFunctions.ConnectionStr(Connection);
                    dsNewMSMLocType = objBL_User.getMSMLoctype(objProp_User);
                    //    Session["dsNewMSMLocType"] = dsNewMSMLocType;
                    //}
                    //else
                    //{
                    //    dsNewMSMLocType = (DataSet)Session["dsNewMSMLocType"];
                    //}

                    inputXMLDoc = new XmlDocument();
                    inputXMLDoc.AppendChild(inputXMLDoc.CreateXmlDeclaration("1.0", "ISO-8859-1", null));
                    inputXMLDoc.AppendChild(inputXMLDoc.CreateProcessingInstruction("qbxml", "version=\"" + qbxmlversion + "\""));
                    qbXML = inputXMLDoc.CreateElement("QBXML");
                    inputXMLDoc.AppendChild(qbXML);
                    qbXMLMsgsRq = inputXMLDoc.CreateElement("QBXMLMsgsRq");
                    qbXML.AppendChild(qbXMLMsgsRq);
                    qbXMLMsgsRq.SetAttribute("onError", "continueOnError");
                    if (dsNewMSMLocType.Tables[0].Rows.Count > 0)
                    {


                        foreach (DataRow dr in dsNewMSMLocType.Tables[0].Rows)
                        {
                            XmlElement JobTypeAddRq = inputXMLDoc.CreateElement("JobTypeAddRq");
                            qbXMLMsgsRq.AppendChild(JobTypeAddRq);
                            JobTypeAddRq.SetAttribute("requestID", dr["type"].ToString());
                            XmlElement JobTypeAdd = inputXMLDoc.CreateElement("JobTypeAdd");
                            JobTypeAddRq.AppendChild(JobTypeAdd);
                            JobTypeAdd.AppendChild(inputXMLDoc.CreateElement("Name")).InnerText = objGenFunctions.QBEncode(dr["type"].ToString(), 31);
                        }

                    }
                    strRequestXML = inputXMLDoc.OuterXml;
                    req.Add(strRequestXML);
                    #endregion

                    // 16 Export/Add SalesTaxCode to QB
                    #region Export/Add SalesTaxCode to QB

                    DataSet dsNewSalesTaxCode = new DataSet();
                    //if (Session["dsNewSalesTaxCode"] == null)
                    //{
                    objProp_User.SearchValue = "0";
                    objProp_User.ConnConfig = objGenFunctions.ConnectionStr(Connection);
                    dsNewSalesTaxCode = objBL_User.getMSMSalesTax(objProp_User);
                    //    Session["dsNewSalesTaxCode"] = dsNewSalesTaxCode;
                    //}
                    //else
                    //{
                    //    dsNewSalesTaxCode = (DataSet)Session["dsNewSalesTaxCode"];
                    //}
                    inputXMLDoc = new XmlDocument();
                    inputXMLDoc.AppendChild(inputXMLDoc.CreateXmlDeclaration("1.0", "ISO-8859-1", null));
                    inputXMLDoc.AppendChild(inputXMLDoc.CreateProcessingInstruction("qbxml", "version=\"" + qbxmlversion + "\""));
                    qbXML = inputXMLDoc.CreateElement("QBXML");
                    inputXMLDoc.AppendChild(qbXML);
                    qbXMLMsgsRq = inputXMLDoc.CreateElement("QBXMLMsgsRq");
                    qbXML.AppendChild(qbXMLMsgsRq);
                    qbXMLMsgsRq.SetAttribute("onError", "continueOnError");

                    if (dsNewSalesTaxCode.Tables[0].Rows.Count > 0)
                    {


                        foreach (DataRow dr in dsNewSalesTaxCode.Tables[0].Rows)
                        {
                            XmlElement SalesTaxCodeAddRq = inputXMLDoc.CreateElement("ItemSalesTaxAddRq");
                            qbXMLMsgsRq.AppendChild(SalesTaxCodeAddRq);
                            SalesTaxCodeAddRq.SetAttribute("requestID", dr["name"].ToString());
                            XmlElement SalesTaxCodeAdd = inputXMLDoc.CreateElement("ItemSalesTaxAdd");
                            SalesTaxCodeAddRq.AppendChild(SalesTaxCodeAdd);
                            SalesTaxCodeAdd.AppendChild(inputXMLDoc.CreateElement("Name")).InnerText = objGenFunctions.QBEncode(dr["name"].ToString(), 31);
                            //SalesTaxCodeAdd.AppendChild(inputXMLDoc.CreateElement("IsTaxable")).InnerText = dr["IsTax"].ToString();                    
                            SalesTaxCodeAdd.AppendChild(inputXMLDoc.CreateElement("ItemDesc")).InnerText = objGenFunctions.QBEncode(dr["fdesc"].ToString(), 500);
                            SalesTaxCodeAdd.AppendChild(inputXMLDoc.CreateElement("TaxRate")).InnerText = dr["rate"].ToString();
                            XmlElement TaxVendorRef = inputXMLDoc.CreateElement("TaxVendorRef");
                            SalesTaxCodeAdd.AppendChild(TaxVendorRef);
                            TaxVendorRef.AppendChild(inputXMLDoc.CreateElement("ListID")).InnerText = dr["qbvendorid"].ToString();
                        }

                    }
                    strRequestXML = inputXMLDoc.OuterXml;
                    req.Add(strRequestXML);
                    #endregion

                    // 17-18 Export/Update SalesTaxCode to QB
                    #region Export/Update SalesTaxCode to QB

                    DataSet dsUpdateSalestaxToQB = new DataSet();
                    objProp_User.ConnConfig = objGenFunctions.ConnectionStr(Connection);
                    objProp_User.SearchValue = "1";
                    dsUpdateSalestaxToQB = objBL_User.getMSMSalesTax(objProp_User);


                    #region Get EditSequence ClassQuery
                    inputXMLDoc = new XmlDocument();
                    inputXMLDoc.AppendChild(inputXMLDoc.CreateXmlDeclaration("1.0", "ISO-8859-1", null));
                    inputXMLDoc.AppendChild(inputXMLDoc.CreateProcessingInstruction("qbxml", "version=\"" + qbxmlversion + "\""));
                    qbXML = inputXMLDoc.CreateElement("QBXML");
                    inputXMLDoc.AppendChild(qbXML);
                    qbXMLMsgsRq = inputXMLDoc.CreateElement("QBXMLMsgsRq");
                    qbXML.AppendChild(qbXMLMsgsRq);
                    qbXMLMsgsRq.SetAttribute("onError", "continueOnError");

                    if (dsUpdateSalestaxToQB.Tables[0].Rows.Count > 0)
                    {


                        XmlElement ItemServiceQueryRq = inputXMLDoc.CreateElement("ItemSalesTaxQueryRq");
                        qbXMLMsgsRq.AppendChild(ItemServiceQueryRq);
                        ItemServiceQueryRq.SetAttribute("requestID", "4");

                        foreach (DataRow dr in dsUpdateSalestaxToQB.Tables[0].Rows)
                        {
                            XmlElement XListID = inputXMLDoc.CreateElement("ListID");
                            ItemServiceQueryRq.AppendChild(XListID).InnerText = dr["QBStaxID"].ToString();
                        }
                        string[] retElements = new string[3] {
                            "ListID"
                            , "EditSequence"
                            , "TimeModified"
                        };
                        for (int i = 0; i < retElements.Length; i++)
                        {
                            XmlElement IncludeRetElement = inputXMLDoc.CreateElement("IncludeRetElement");
                            ItemServiceQueryRq.AppendChild(IncludeRetElement).InnerText = retElements[i];
                        }
                    }
                    strRequestXML = inputXMLDoc.OuterXml;
                    req.Add(strRequestXML);
                    #endregion

                    #region Mod Class Query
                    inputXMLDoc = new XmlDocument();
                    inputXMLDoc.AppendChild(inputXMLDoc.CreateXmlDeclaration("1.0", "ISO-8859-1", null));
                    inputXMLDoc.AppendChild(inputXMLDoc.CreateProcessingInstruction("qbxml", "version=\"" + qbxmlversion + "\""));
                    qbXML = inputXMLDoc.CreateElement("QBXML");
                    inputXMLDoc.AppendChild(qbXML);
                    qbXMLMsgsRq = inputXMLDoc.CreateElement("QBXMLMsgsRq");
                    qbXML.AppendChild(qbXMLMsgsRq);
                    qbXMLMsgsRq.SetAttribute("onError", "continueOnError");
                    if (dsUpdateSalestaxToQB.Tables[0].Rows.Count > 0)
                    {
                        DataTable dtModItemSalesTAxResponse = new DataTable();
                        if (Session["dtModItemSalesTAxResponse"] != null)
                        {
                            dtModItemSalesTAxResponse = (DataTable)Session["dtModItemSalesTAxResponse"];

                            foreach (DataRow dr in dsUpdateSalestaxToQB.Tables[0].Rows)
                            {
                                DateTime lastUpdateDateQB = System.DateTime.MinValue;
                                string strEditseq = string.Empty;
                                foreach (DataRow drModCustResponse in dtModItemSalesTAxResponse.Rows)
                                {
                                    if (drModCustResponse["ListID"].ToString() == dr["QBStaxID"].ToString())
                                    {
                                        lastUpdateDateQB = Convert.ToDateTime(drModCustResponse["TimeModified"].ToString());
                                        strEditseq = drModCustResponse["EditSequence"].ToString();

                                        if (lastUpdateDateQB < Convert.ToDateTime(dr["LastUpdateDate"].ToString()))
                                        {
                                            XmlElement ItemSalesTaxModRq = inputXMLDoc.CreateElement("ItemSalesTaxModRq");
                                            qbXMLMsgsRq.AppendChild(ItemSalesTaxModRq);
                                            ItemSalesTaxModRq.SetAttribute("requestID", dr["name"].ToString());
                                            XmlElement ItemSalesTaxMod = inputXMLDoc.CreateElement("ItemSalesTaxMod");
                                            ItemSalesTaxModRq.AppendChild(ItemSalesTaxMod);

                                            ItemSalesTaxMod.AppendChild(inputXMLDoc.CreateElement("ListID")).InnerText = dr["QBStaxID"].ToString();
                                            ItemSalesTaxMod.AppendChild(inputXMLDoc.CreateElement("EditSequence")).InnerText = strEditseq;
                                            ItemSalesTaxMod.AppendChild(inputXMLDoc.CreateElement("ItemDesc")).InnerText = objGenFunctions.QBEncode(dr["fdesc"].ToString(), 500);
                                            ItemSalesTaxMod.AppendChild(inputXMLDoc.CreateElement("TaxRate")).InnerText = dr["rate"].ToString();
                                        }
                                    }
                                }
                            }

                            // clean session
                            if (isSendingRequest) Session["dtModItemSalesTAxResponse"] = null;
                        }
                    }
                    strRequestXML = inputXMLDoc.OuterXml;
                    req.Add(strRequestXML);
                    #endregion

                    #endregion

                    // 19 Export/Add customers to QB
                    #region Export/Add customers to QB

                    DataSet dsNewMSMCust = new DataSet();
                    //if (Session["dsNewMSMCust"] == null)
                    //{
                    objProp_User.ConnConfig = objGenFunctions.ConnectionStr(Connection);
                    dsNewMSMCust = objBL_User.getMSMCustomers(objProp_User);
                    //    Session["dsNewMSMCust"] = dsNewMSMCust;
                    //}
                    //else
                    //{
                    //    dsNewMSMCust=(DataSet)Session["dsNewMSMCust"];
                    //}
                    inputXMLDoc = new XmlDocument();
                    inputXMLDoc.AppendChild(inputXMLDoc.CreateXmlDeclaration("1.0", "ISO-8859-1", null));
                    inputXMLDoc.AppendChild(inputXMLDoc.CreateProcessingInstruction("qbxml", "version=\"" + qbxmlversion + "\""));

                    qbXML = inputXMLDoc.CreateElement("QBXML");
                    inputXMLDoc.AppendChild(qbXML);
                    qbXMLMsgsRq = inputXMLDoc.CreateElement("QBXMLMsgsRq");
                    qbXML.AppendChild(qbXMLMsgsRq);
                    qbXMLMsgsRq.SetAttribute("onError", "continueOnError");

                    if (dsNewMSMCust.Tables[0].Rows.Count > 0)
                    {


                        foreach (DataRow dr in dsNewMSMCust.Tables[0].Rows)
                        {
                            XmlElement CustomerAddRq = inputXMLDoc.CreateElement("CustomerAddRq");
                            qbXMLMsgsRq.AppendChild(CustomerAddRq);
                            CustomerAddRq.SetAttribute("requestID", "1-" + dr["id"].ToString());
                            XmlElement CustomerAdd = inputXMLDoc.CreateElement("CustomerAdd");
                            CustomerAddRq.AppendChild(CustomerAdd);
                            string active = "";
                            if (dr["Status"].ToString() == "1")
                            {
                                active = "0";
                            }
                            else
                            {
                                active = "1";
                            }
                            CustomerAdd.AppendChild(inputXMLDoc.CreateElement("Name")).InnerText = objGenFunctions.QBEncode(dr["name"].ToString(), 41);
                            CustomerAdd.AppendChild(inputXMLDoc.CreateElement("IsActive")).InnerText = active;
                            CustomerAdd.AppendChild(inputXMLDoc.CreateElement("CompanyName")).InnerText = objGenFunctions.QBEncode(dr["name"].ToString(), 41);
                            //if (!string.IsNullOrEmpty(dr["remarks"].ToString().Trim()))
                            //{
                            //    CustomerAdd.AppendChild(inputXMLDoc.CreateElement("Notes")).InnerText = dr["remarks"].ToString();
                            //}
                            if (!string.IsNullOrEmpty(dr["Contact"].ToString().Trim()))
                            {
                                string[] contact = dr["Contact"].ToString().Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                int contactLength = contact.Count();
                                string firstname = string.Empty;
                                string middlename = string.Empty;
                                string lastname = string.Empty;

                                if (contactLength > 0)
                                {
                                    firstname = contact[0].Trim();
                                }
                                if (contactLength == 2)
                                {
                                    lastname = contact[1].Trim();
                                }
                                if (contactLength > 2)
                                {
                                    middlename = contact[1].Trim();
                                    lastname = contact[2].Trim();
                                }

                                if (firstname != string.Empty)
                                {
                                    CustomerAdd.AppendChild(inputXMLDoc.CreateElement("FirstName")).InnerText = objGenFunctions.QBEncode(firstname, 25);
                                }
                                if (middlename != string.Empty)
                                {
                                    CustomerAdd.AppendChild(inputXMLDoc.CreateElement("MiddleName")).InnerText = objGenFunctions.QBEncode(middlename, 5);
                                }
                                if (lastname != string.Empty)
                                {
                                    CustomerAdd.AppendChild(inputXMLDoc.CreateElement("LastName")).InnerText = objGenFunctions.QBEncode(lastname, 25);
                                }
                            }
                            //if (!string.IsNullOrEmpty(dr["Contact"].ToString().Trim()))
                            //{
                            //    //CustomerAdd.AppendChild(inputXMLDoc.CreateElement("LastName")).InnerText = dr["Contact"].ToString();
                            //}

                            #region BillAddress
                            string[] strBillAddress = dr["Address"].ToString().Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                            int intBillAddCount = strBillAddress.Count();
                            XmlElement BillAddress = inputXMLDoc.CreateElement("BillAddress");
                            CustomerAdd.AppendChild(BillAddress);

                            if (intBillAddCount > 0)
                            {
                                if (!string.IsNullOrEmpty(strBillAddress[0].Trim()))
                                {
                                    BillAddress.AppendChild(inputXMLDoc.CreateElement("Addr1")).InnerText = objGenFunctions.QBEncode(strBillAddress[0].Trim(), 41);
                                }
                            }
                            if (intBillAddCount > 1)
                            {
                                if (!string.IsNullOrEmpty(strBillAddress[1].Trim()))
                                {
                                    BillAddress.AppendChild(inputXMLDoc.CreateElement("Addr2")).InnerText = objGenFunctions.QBEncode(strBillAddress[1].Trim(), 41);
                                }
                            }
                            string BillstrAdd3 = string.Empty;
                            if (intBillAddCount > 2)
                            {
                                if (!string.IsNullOrEmpty(strBillAddress[2].Trim()))
                                {
                                    BillstrAdd3 = strBillAddress[2].Trim();
                                }
                            }
                            if (intBillAddCount > 3)
                            {
                                for (int i = 3; i < strBillAddress.Count(); i++)
                                {
                                    BillstrAdd3 += " " + strBillAddress[i].Trim();
                                }
                            }
                            if (BillstrAdd3 != string.Empty)
                            {
                                BillAddress.AppendChild(inputXMLDoc.CreateElement("Addr3")).InnerText = objGenFunctions.QBEncode(BillstrAdd3, 41);
                            }

                            if (!string.IsNullOrEmpty(dr["city"].ToString().Trim()))
                            {
                                BillAddress.AppendChild(inputXMLDoc.CreateElement("City")).InnerText = objGenFunctions.QBEncode(dr["city"].ToString(), 31);
                            }
                            if (!string.IsNullOrEmpty(dr["State"].ToString().Trim()))
                            {
                                BillAddress.AppendChild(inputXMLDoc.CreateElement("State")).InnerText = objGenFunctions.QBEncode(dr["State"].ToString(), 21);
                            }
                            if (!string.IsNullOrEmpty(dr["zip"].ToString().Trim()))
                            {
                                BillAddress.AppendChild(inputXMLDoc.CreateElement("PostalCode")).InnerText = objGenFunctions.QBEncode(dr["zip"].ToString(), 13);
                            }

                            #endregion

                            #region ShipAddress

                            //string[] strAddress = dr["address"].ToString().Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                            //int intAddCount = strAddress.Count();
                            //XmlElement ShipAddress = inputXMLDoc.CreateElement("ShipAddress");
                            //CustomerAdd.AppendChild(ShipAddress);
                            //if (intAddCount > 0)
                            //{
                            //    if (!string.IsNullOrEmpty(strAddress[0].Trim()))
                            //    {
                            //        ShipAddress.AppendChild(inputXMLDoc.CreateElement("Addr1")).InnerText = objGenFunctions.QBEncode(strAddress[0].Trim(), 41);
                            //    }
                            //}
                            //if (intAddCount > 1)
                            //{
                            //    if (!string.IsNullOrEmpty(strAddress[1].Trim()))
                            //    {
                            //        ShipAddress.AppendChild(inputXMLDoc.CreateElement("Addr2")).InnerText = objGenFunctions.QBEncode(strAddress[1].Trim(), 41);
                            //    }
                            //}
                            //string strAdd3 = string.Empty;
                            //if (intAddCount > 2)
                            //{
                            //    if (!string.IsNullOrEmpty(strAddress[2].Trim()))
                            //    {
                            //        strAdd3 = strAddress[2].Trim();
                            //    }
                            //}
                            //if (intAddCount > 3)
                            //{
                            //    for (int i = 3; i < strAddress.Count(); i++)
                            //    {
                            //        strAdd3 += " " + strAddress[i].Trim();
                            //    }
                            //}
                            //if (strAdd3 != string.Empty)
                            //{
                            //    ShipAddress.AppendChild(inputXMLDoc.CreateElement("Addr3")).InnerText = objGenFunctions.QBEncode(strAdd3, 41);
                            //}

                            ////if (!string.IsNullOrEmpty(dr["remarks"].ToString().Trim()))
                            ////{
                            ////    ShipAddress.AppendChild(inputXMLDoc.CreateElement("Note")).InnerText = dr["remarks"].ToString();
                            ////}
                            //if (!string.IsNullOrEmpty(dr["city"].ToString().Trim()))
                            //{
                            //    ShipAddress.AppendChild(inputXMLDoc.CreateElement("City")).InnerText = objGenFunctions.QBEncode(dr["city"].ToString(), 21);
                            //}
                            //if (!string.IsNullOrEmpty(dr["State"].ToString().Trim()))
                            //{
                            //    ShipAddress.AppendChild(inputXMLDoc.CreateElement("State")).InnerText = objGenFunctions.QBEncode(dr["State"].ToString(), 21);
                            //}
                            //if (!string.IsNullOrEmpty(dr["zip"].ToString().Trim()))
                            //{
                            //    ShipAddress.AppendChild(inputXMLDoc.CreateElement("PostalCode")).InnerText = objGenFunctions.QBEncode(dr["zip"].ToString(), 13);
                            //}

                            #endregion ShipAddress

                            #region ShipTOAddress


                            string[] strShipToAddress = dr["address"].ToString().Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                            int intShipToAddCount = strShipToAddress.Count();
                            XmlElement ShipTOAddress = inputXMLDoc.CreateElement("ShipToAddress");
                            CustomerAdd.AppendChild(ShipTOAddress);

                            ShipTOAddress.AppendChild(inputXMLDoc.CreateElement("Name")).InnerText = objGenFunctions.QBEncode(dr["name"].ToString(), 41);

                            if (intShipToAddCount > 0)
                            {
                                if (!string.IsNullOrEmpty(strShipToAddress[0].Trim()))
                                {
                                    ShipTOAddress.AppendChild(inputXMLDoc.CreateElement("Addr1")).InnerText = objGenFunctions.QBEncode(strShipToAddress[0].Trim(), 41);
                                }
                            }
                            if (intShipToAddCount > 1)
                            {
                                if (!string.IsNullOrEmpty(strShipToAddress[1].Trim()))
                                {
                                    ShipTOAddress.AppendChild(inputXMLDoc.CreateElement("Addr2")).InnerText = objGenFunctions.QBEncode(strShipToAddress[1].Trim(), 41);
                                }
                            }
                            string strShipToAdd3 = string.Empty;
                            if (intShipToAddCount > 2)
                            {
                                if (!string.IsNullOrEmpty(strShipToAddress[2].Trim()))
                                {
                                    strShipToAdd3 = strShipToAddress[2].Trim();
                                }
                            }
                            if (intShipToAddCount > 3)
                            {
                                for (int i = 3; i < strShipToAddress.Count(); i++)
                                {
                                    strShipToAdd3 += " " + strShipToAddress[i].Trim();
                                }
                            }
                            if (strShipToAdd3 != string.Empty)
                            {
                                ShipTOAddress.AppendChild(inputXMLDoc.CreateElement("Addr3")).InnerText = objGenFunctions.QBEncode(strShipToAdd3, 41);
                            }

                            if (!string.IsNullOrEmpty(dr["city"].ToString().Trim()))
                            {
                                ShipTOAddress.AppendChild(inputXMLDoc.CreateElement("City")).InnerText = objGenFunctions.QBEncode(dr["city"].ToString(), 31);
                            }
                            if (!string.IsNullOrEmpty(dr["State"].ToString().Trim()))
                            {
                                ShipTOAddress.AppendChild(inputXMLDoc.CreateElement("State")).InnerText = objGenFunctions.QBEncode(dr["State"].ToString(), 21);
                            }
                            if (!string.IsNullOrEmpty(dr["zip"].ToString().Trim()))
                            {
                                ShipTOAddress.AppendChild(inputXMLDoc.CreateElement("PostalCode")).InnerText = objGenFunctions.QBEncode(dr["zip"].ToString(), 13);
                            }

                            ShipTOAddress.AppendChild(inputXMLDoc.CreateElement("DefaultShipTo")).InnerText = "1";

                            #endregion ShipTOAddress


                            if (!string.IsNullOrEmpty(dr["Phone"].ToString().Trim()))
                            {
                                CustomerAdd.AppendChild(inputXMLDoc.CreateElement("Phone")).InnerText = objGenFunctions.QBEncode(dr["Phone"].ToString(), 21);
                            }
                            //if (!string.IsNullOrEmpty(dr["Cellular"].ToString().Trim()))
                            //{
                            //    CustomerAdd.AppendChild(inputXMLDoc.CreateElement("Mobile")).InnerText = objGenFunctions.QBEncode(dr["Cellular"].ToString(), 21);
                            //}  

                            if (!string.IsNullOrEmpty(dr["Fax"].ToString().Trim()))
                            {
                                CustomerAdd.AppendChild(inputXMLDoc.CreateElement("Fax")).InnerText = objGenFunctions.QBEncode(dr["Fax"].ToString(), 21);
                            }
                            if (!string.IsNullOrEmpty(dr["Email"].ToString().Trim()))
                            {
                                CustomerAdd.AppendChild(inputXMLDoc.CreateElement("Email")).InnerText = objGenFunctions.QBEncode(dr["Email"].ToString(), 99);
                            }

                            if (!string.IsNullOrEmpty(dr["QBCustomertypeID"].ToString().Trim()))
                            {
                                XmlElement CustomerTypeRef = inputXMLDoc.CreateElement("CustomerTypeRef");
                                CustomerAdd.AppendChild(CustomerTypeRef);
                                CustomerTypeRef.AppendChild(inputXMLDoc.CreateElement("ListID")).InnerText = dr["QBCustomertypeID"].ToString();
                            }

                            if (!string.IsNullOrEmpty(dr["Balance"].ToString().Trim()))
                            {
                                CustomerAdd.AppendChild(inputXMLDoc.CreateElement("OpenBalance")).InnerText = objGenFunctions.QBEncode(dr["Balance"].ToString(), 30);
                            }
                        }

                    }
                    strRequestXML = inputXMLDoc.OuterXml;
                    req.Add(strRequestXML);
                    #endregion

                    // 20-21 Export/Update Customers to QB
                    #region Export/Update Customers to QB

                    DataSet dsUpdateCustToQB = new DataSet();
                    //if (Session["dsUpdateCustToQB"] == null)
                    //{
                    objProp_User.ConnConfig = objGenFunctions.ConnectionStr(Connection);
                    dsUpdateCustToQB = objBL_User.getQBCustomers(objProp_User);
                    //    Session["dsUpdateCustToQB"] = dsUpdateCustToQB;
                    //}
                    //else
                    //{
                    //    dsUpdateCustToQB = (DataSet)Session["dsUpdateCustToQB"];
                    //}

                    #region Get EditSequence CustomerQuery
                    inputXMLDoc = new XmlDocument();
                    inputXMLDoc.AppendChild(inputXMLDoc.CreateXmlDeclaration("1.0", "ISO-8859-1", null));
                    inputXMLDoc.AppendChild(inputXMLDoc.CreateProcessingInstruction("qbxml", "version=\"" + qbxmlversion + "\""));
                    qbXML = inputXMLDoc.CreateElement("QBXML");
                    inputXMLDoc.AppendChild(qbXML);
                    qbXMLMsgsRq = inputXMLDoc.CreateElement("QBXMLMsgsRq");
                    qbXML.AppendChild(qbXMLMsgsRq);
                    qbXMLMsgsRq.SetAttribute("onError", "continueOnError");

                    if (dsUpdateCustToQB.Tables[0].Rows.Count > 0)
                    {
                        XmlElement CustomerQueryRqMod = inputXMLDoc.CreateElement("CustomerQueryRq");
                        qbXMLMsgsRq.AppendChild(CustomerQueryRqMod);
                        CustomerQueryRqMod.SetAttribute("requestID", "2");
                        foreach (DataRow dr in dsUpdateCustToQB.Tables[0].Rows)
                        {
                            XmlElement XListID = inputXMLDoc.CreateElement("ListID");
                            CustomerQueryRqMod.AppendChild(XListID).InnerText = dr["QBCustomerID"].ToString();
                        }
                        string[] retElements = new string[3] {
                            "ListID"
                            , "EditSequence"
                            , "TimeModified"
                        };
                        for (int i = 0; i < retElements.Length; i++)
                        {
                            XmlElement IncludeRetElement = inputXMLDoc.CreateElement("IncludeRetElement");
                            CustomerQueryRqMod.AppendChild(IncludeRetElement).InnerText = retElements[i];
                        }
                    }
                    strRequestXML = inputXMLDoc.OuterXml;
                    req.Add(strRequestXML);
                    #endregion

                    #region Mod Cust Query
                    inputXMLDoc = new XmlDocument();
                    inputXMLDoc.AppendChild(inputXMLDoc.CreateXmlDeclaration("1.0", "ISO-8859-1", null));
                    inputXMLDoc.AppendChild(inputXMLDoc.CreateProcessingInstruction("qbxml", "version=\"" + qbxmlversion + "\""));
                    qbXML = inputXMLDoc.CreateElement("QBXML");
                    inputXMLDoc.AppendChild(qbXML);
                    qbXMLMsgsRq = inputXMLDoc.CreateElement("QBXMLMsgsRq");
                    qbXML.AppendChild(qbXMLMsgsRq);
                    qbXMLMsgsRq.SetAttribute("onError", "continueOnError");

                    if (dsUpdateCustToQB.Tables[0].Rows.Count > 0)
                    {
                        DataTable dtModCustResponse = new DataTable();
                        if (Session["dtModCustResponse"] != null)
                        {
                            dtModCustResponse = (DataTable)Session["dtModCustResponse"];

                            foreach (DataRow dr in dsUpdateCustToQB.Tables[0].Rows)
                            {
                                DateTime lastUpdateDateQB = System.DateTime.MinValue;
                                //if (Session["ModifyDate"] != null)
                                //{
                                //    lastUpdateDateQB = Convert.ToDateTime(Session["ModifyDate"].ToString());
                                //}

                                string strEditseq = string.Empty;
                                //if (Session["EditSeqCust"] != null)
                                //{
                                //    strEditseq = Session["EditSeqCust"].ToString();
                                //}
                                foreach (DataRow drModCustResponse in dtModCustResponse.Rows)
                                {
                                    if (drModCustResponse["ListID"].ToString() == dr["QBCustomerID"].ToString())
                                    {
                                        lastUpdateDateQB = Convert.ToDateTime(drModCustResponse["TimeModified"].ToString());
                                        strEditseq = drModCustResponse["EditSequence"].ToString();

                                        string active = "";
                                        if (dr["Status"].ToString() == "1")
                                        {
                                            active = "0";
                                        }
                                        else
                                        {
                                            active = "1";
                                        }

                                        if (lastUpdateDateQB < Convert.ToDateTime(dr["LastUpdateDate"].ToString()))
                                        {
                                            XmlElement CustomerModRq = inputXMLDoc.CreateElement("CustomerModRq");
                                            qbXMLMsgsRq.AppendChild(CustomerModRq);
                                            CustomerModRq.SetAttribute("requestID", dr["id"].ToString());
                                            XmlElement CustomerMod = inputXMLDoc.CreateElement("CustomerMod");
                                            CustomerModRq.AppendChild(CustomerMod);

                                            CustomerMod.AppendChild(inputXMLDoc.CreateElement("ListID")).InnerText = dr["QBCustomerID"].ToString();
                                            CustomerMod.AppendChild(inputXMLDoc.CreateElement("EditSequence")).InnerText = strEditseq;
                                            CustomerMod.AppendChild(inputXMLDoc.CreateElement("Name")).InnerText = objGenFunctions.QBEncode(dr["Name"].ToString(), 41);
                                            CustomerMod.AppendChild(inputXMLDoc.CreateElement("IsActive")).InnerText = active;
                                            CustomerMod.AppendChild(inputXMLDoc.CreateElement("CompanyName")).InnerText = objGenFunctions.QBEncode(dr["Name"].ToString(), 41);
                                            if (!string.IsNullOrEmpty(dr["Contact"].ToString().Trim()))
                                            {
                                                string[] contact = dr["Contact"].ToString().Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                                int contactLength = contact.Count();
                                                string firstname = string.Empty;
                                                string middlename = string.Empty;
                                                string lastname = string.Empty;

                                                if (contactLength > 0)
                                                {
                                                    firstname = contact[0].Trim();
                                                }
                                                if (contactLength == 2)
                                                {
                                                    lastname = contact[1].Trim();
                                                }
                                                if (contactLength > 2)
                                                {
                                                    middlename = contact[1].Trim();
                                                    lastname = contact[2].Trim();
                                                }

                                                if (firstname != string.Empty)
                                                {
                                                    CustomerMod.AppendChild(inputXMLDoc.CreateElement("FirstName")).InnerText = objGenFunctions.QBEncode(firstname, 25);
                                                }
                                                if (middlename != string.Empty)
                                                {
                                                    CustomerMod.AppendChild(inputXMLDoc.CreateElement("MiddleName")).InnerText = objGenFunctions.QBEncode(middlename, 5);
                                                }
                                                if (lastname != string.Empty)
                                                {
                                                    CustomerMod.AppendChild(inputXMLDoc.CreateElement("LastName")).InnerText = objGenFunctions.QBEncode(lastname, 25);
                                                }
                                            }
                                            //if (!string.IsNullOrEmpty(dr["Contact"].ToString().Trim()))
                                            //{
                                            //    //CustomerMod.AppendChild(inputXMLDoc.CreateElement("LastName")).InnerText = dr["Contact"].ToString();
                                            //}
                                            #region address
                                            string[] strAddress = dr["address"].ToString().Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                                            int intAddCount = strAddress.Count();
                                            XmlElement ShipAddress = inputXMLDoc.CreateElement("ShipAddress");
                                            CustomerMod.AppendChild(ShipAddress);
                                            if (intAddCount > 0)
                                            {
                                                if (!string.IsNullOrEmpty(strAddress[0].Trim()))
                                                {
                                                    ShipAddress.AppendChild(inputXMLDoc.CreateElement("Addr1")).InnerText = objGenFunctions.QBEncode(strAddress[0].Trim(), 41);
                                                }
                                            }
                                            if (intAddCount > 1)
                                            {
                                                if (!string.IsNullOrEmpty(strAddress[1].Trim()))
                                                {
                                                    ShipAddress.AppendChild(inputXMLDoc.CreateElement("Addr2")).InnerText = objGenFunctions.QBEncode(strAddress[1].Trim(), 41);
                                                }
                                            }
                                            string strAdd3 = string.Empty;
                                            if (intAddCount > 2)
                                            {
                                                if (!string.IsNullOrEmpty(strAddress[2].Trim()))
                                                {
                                                    strAdd3 = strAddress[2].Trim();
                                                    //ShipAddress.AppendChild(inputXMLDoc.CreateElement("Addr3")).InnerText = strAddress[2].Trim();
                                                }
                                            }
                                            if (intAddCount > 3)
                                            {
                                                for (int i = 3; i < strAddress.Count(); i++)
                                                {
                                                    strAdd3 += " " + strAddress[i].Trim();
                                                }
                                            }
                                            if (strAdd3 != string.Empty)
                                            {
                                                ShipAddress.AppendChild(inputXMLDoc.CreateElement("Addr3")).InnerText = objGenFunctions.QBEncode(strAdd3, 41);
                                            }
                                            if (!string.IsNullOrEmpty(dr["city"].ToString().Trim()))
                                            {
                                                ShipAddress.AppendChild(inputXMLDoc.CreateElement("City")).InnerText = objGenFunctions.QBEncode(dr["city"].ToString(), 31);
                                            }
                                            if (!string.IsNullOrEmpty(dr["State"].ToString().Trim()))
                                            {
                                                ShipAddress.AppendChild(inputXMLDoc.CreateElement("State")).InnerText = objGenFunctions.QBEncode(dr["State"].ToString(), 21);
                                            }
                                            if (!string.IsNullOrEmpty(dr["zip"].ToString().Trim()))
                                            {
                                                ShipAddress.AppendChild(inputXMLDoc.CreateElement("PostalCode")).InnerText = objGenFunctions.QBEncode(dr["zip"].ToString(), 13);
                                            }

                                            #endregion address

                                            if (!string.IsNullOrEmpty(dr["Phone"].ToString().Trim()))
                                            {
                                                CustomerMod.AppendChild(inputXMLDoc.CreateElement("Phone")).InnerText = objGenFunctions.QBEncode(dr["Phone"].ToString(), 21);
                                            }

                                            //if (!string.IsNullOrEmpty(dr["Cellular"].ToString().Trim()))
                                            //{
                                            //    CustomerMod.AppendChild(inputXMLDoc.CreateElement("Mobile")).InnerText = objGenFunctions.QBEncode(dr["Cellular"].ToString(), 21);
                                            //}  

                                            if (!string.IsNullOrEmpty(dr["Fax"].ToString().Trim()))
                                            {
                                                CustomerMod.AppendChild(inputXMLDoc.CreateElement("Fax")).InnerText = objGenFunctions.QBEncode(dr["Fax"].ToString(), 21);
                                            }
                                            if (!string.IsNullOrEmpty(dr["Email"].ToString().Trim()))
                                            {
                                                CustomerMod.AppendChild(inputXMLDoc.CreateElement("Email")).InnerText = objGenFunctions.QBEncode(dr["Email"].ToString(), 99);
                                            }


                                            if (!string.IsNullOrEmpty(dr["QBCustomertypeID"].ToString().Trim()))
                                            {
                                                XmlElement CustomerTypeRef = inputXMLDoc.CreateElement("CustomerTypeRef");
                                                CustomerMod.AppendChild(CustomerTypeRef);
                                                CustomerTypeRef.AppendChild(inputXMLDoc.CreateElement("ListID")).InnerText = dr["QBCustomertypeID"].ToString();
                                            }
                                        }
                                    }
                                }
                            }

                            // clean session after sending request
                            if (isSendingRequest) Session["dtModCustResponse"] = null;
                        }
                    }
                    strRequestXML = inputXMLDoc.OuterXml;
                    req.Add(strRequestXML);
                    #endregion


                    #endregion

                    // 22 Export/Add Locations to QB
                    #region Export/Add Locations to QB

                    DataSet dsNewMSMLoc = new DataSet();
                    //if (Session["dsNewMSMLoc"] == null)
                    //{
                    objProp_User.ConnConfig = objGenFunctions.ConnectionStr(Connection);
                    dsNewMSMLoc = objBL_User.getMSMLocation(objProp_User);
                    //Session["dsNewMSMLoc"] = dsNewMSMLoc;
                    //}
                    //else
                    //{
                    //    dsNewMSMLoc = (DataSet)Session["dsNewMSMLoc"];
                    //}

                    inputXMLDoc = new XmlDocument();
                    inputXMLDoc.AppendChild(inputXMLDoc.CreateXmlDeclaration("1.0", "ISO-8859-1", null));
                    inputXMLDoc.AppendChild(inputXMLDoc.CreateProcessingInstruction("qbxml", "version=\"" + qbxmlversion + "\""));

                    qbXML = inputXMLDoc.CreateElement("QBXML");
                    inputXMLDoc.AppendChild(qbXML);
                    qbXMLMsgsRq = inputXMLDoc.CreateElement("QBXMLMsgsRq");
                    qbXML.AppendChild(qbXMLMsgsRq);
                    qbXMLMsgsRq.SetAttribute("onError", "continueOnError");

                    if (dsNewMSMLoc.Tables[0].Rows.Count > 0)
                    {


                        foreach (DataRow dr in dsNewMSMLoc.Tables[0].Rows)
                        {
                            if (!string.IsNullOrEmpty(dr["qbcustomerid"].ToString().Trim()))
                            {
                                XmlElement CustomerAddRq = inputXMLDoc.CreateElement("CustomerAddRq");
                                qbXMLMsgsRq.AppendChild(CustomerAddRq);
                                CustomerAddRq.SetAttribute("requestID", "2-" + dr["id"].ToString());
                                XmlElement CustomerAdd = inputXMLDoc.CreateElement("CustomerAdd");
                                CustomerAddRq.AppendChild(CustomerAdd);
                                string active = "";
                                if (dr["Status"].ToString() == "1")
                                {
                                    active = "0";
                                }
                                else
                                {
                                    active = "1";
                                }
                                CustomerAdd.AppendChild(inputXMLDoc.CreateElement("Name")).InnerText = objGenFunctions.QBEncode(dr["tag"].ToString(), 41);
                                CustomerAdd.AppendChild(inputXMLDoc.CreateElement("IsActive")).InnerText = active;
                                if (!string.IsNullOrEmpty(dr["qbcustomerid"].ToString().Trim()))
                                {
                                    XmlElement ParentRef = inputXMLDoc.CreateElement("ParentRef");
                                    CustomerAdd.AppendChild(ParentRef);
                                    ParentRef.AppendChild(inputXMLDoc.CreateElement("ListID")).InnerText = dr["qbcustomerid"].ToString();
                                }
                                CustomerAdd.AppendChild(inputXMLDoc.CreateElement("CompanyName")).InnerText = objGenFunctions.QBEncode(dr["tag"].ToString(), 41);
                                //if (!string.IsNullOrEmpty(dr["remarks"].ToString().Trim()))
                                //{
                                //    CustomerAdd.AppendChild(inputXMLDoc.CreateElement("Notes")).InnerText = dr["remarks"].ToString();
                                //}
                                if (!string.IsNullOrEmpty(dr["Contact"].ToString().Trim()))
                                {
                                    string[] contact = dr["Contact"].ToString().Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                    int contactLength = contact.Count();
                                    string firstname = string.Empty;
                                    string middlename = string.Empty;
                                    string lastname = string.Empty;

                                    if (contactLength > 0)
                                    {
                                        firstname = contact[0].Trim();
                                    }
                                    if (contactLength == 2)
                                    {
                                        lastname = contact[1].Trim();
                                    }
                                    if (contactLength > 2)
                                    {
                                        middlename = contact[1].Trim();
                                        lastname = contact[2].Trim();
                                    }

                                    if (firstname != string.Empty)
                                    {
                                        CustomerAdd.AppendChild(inputXMLDoc.CreateElement("FirstName")).InnerText = objGenFunctions.QBEncode(firstname, 25);
                                    }
                                    if (middlename != string.Empty)
                                    {
                                        CustomerAdd.AppendChild(inputXMLDoc.CreateElement("MiddleName")).InnerText = objGenFunctions.QBEncode(middlename, 5);
                                    }
                                    if (lastname != string.Empty)
                                    {
                                        CustomerAdd.AppendChild(inputXMLDoc.CreateElement("LastName")).InnerText = objGenFunctions.QBEncode(lastname, 25);
                                    }
                                }
                                //if (!string.IsNullOrEmpty(dr["Contact"].ToString().Trim()))
                                //{
                                //    //CustomerAdd.AppendChild(inputXMLDoc.CreateElement("LastName")).InnerText = dr["Contact"].ToString();
                                //}

                                #region BillAddress

                                string[] strBillAddress = dr["Address"].ToString().Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                                int intBillAddCount = strBillAddress.Count();
                                XmlElement BillAddress = inputXMLDoc.CreateElement("BillAddress");
                                CustomerAdd.AppendChild(BillAddress);

                                if (intBillAddCount > 0)
                                {
                                    if (!string.IsNullOrEmpty(strBillAddress[0].Trim()))
                                    {
                                        BillAddress.AppendChild(inputXMLDoc.CreateElement("Addr1")).InnerText = objGenFunctions.QBEncode(strBillAddress[0].Trim(), 41);
                                    }
                                }
                                if (intBillAddCount > 1)
                                {
                                    if (!string.IsNullOrEmpty(strBillAddress[1].Trim()))
                                    {
                                        BillAddress.AppendChild(inputXMLDoc.CreateElement("Addr2")).InnerText = objGenFunctions.QBEncode(strBillAddress[1].Trim(), 41);
                                    }
                                }
                                string strAdd3 = string.Empty;
                                if (intBillAddCount > 2)
                                {
                                    if (!string.IsNullOrEmpty(strBillAddress[2].Trim()))
                                    {
                                        strAdd3 = strBillAddress[2].Trim();
                                    }
                                }
                                if (intBillAddCount > 3)
                                {
                                    for (int i = 3; i < strBillAddress.Count(); i++)
                                    {
                                        strAdd3 += " " + strBillAddress[i].Trim();
                                    }
                                }
                                if (strAdd3 != string.Empty)
                                {
                                    BillAddress.AppendChild(inputXMLDoc.CreateElement("Addr3")).InnerText = objGenFunctions.QBEncode(strAdd3, 41);
                                }

                                if (!string.IsNullOrEmpty(dr["city"].ToString().Trim()))
                                {
                                    BillAddress.AppendChild(inputXMLDoc.CreateElement("City")).InnerText = objGenFunctions.QBEncode(dr["city"].ToString(), 31);
                                }
                                if (!string.IsNullOrEmpty(dr["State"].ToString().Trim()))
                                {
                                    BillAddress.AppendChild(inputXMLDoc.CreateElement("State")).InnerText = objGenFunctions.QBEncode(dr["State"].ToString(), 21);
                                }
                                if (!string.IsNullOrEmpty(dr["zip"].ToString().Trim()))
                                {
                                    BillAddress.AppendChild(inputXMLDoc.CreateElement("PostalCode")).InnerText = objGenFunctions.QBEncode(dr["zip"].ToString(), 13);
                                }

                                #endregion BillAddress

                                #region ShipAddress
                                string[] strShipAddress = dr["ShipAddress"].ToString().Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                                int intShipAddCount = strShipAddress.Count();
                                XmlElement ShipAddress = inputXMLDoc.CreateElement("ShipAddress");
                                CustomerAdd.AppendChild(ShipAddress);

                                if (intShipAddCount > 0)
                                {
                                    if (!string.IsNullOrEmpty(strShipAddress[0].Trim()))
                                    {
                                        ShipAddress.AppendChild(inputXMLDoc.CreateElement("Addr1")).InnerText = objGenFunctions.QBEncode(strShipAddress[0].Trim(), 41);
                                    }
                                }
                                if (intShipAddCount > 1)
                                {
                                    if (!string.IsNullOrEmpty(strShipAddress[1].Trim()))
                                    {
                                        ShipAddress.AppendChild(inputXMLDoc.CreateElement("Addr2")).InnerText = objGenFunctions.QBEncode(strShipAddress[1].Trim(), 41);
                                    }
                                }
                                string strShipAdd3 = string.Empty;
                                if (intShipAddCount > 2)
                                {
                                    if (!string.IsNullOrEmpty(strShipAddress[2].Trim()))
                                    {
                                        strShipAdd3 = strShipAddress[2].Trim();
                                    }
                                }
                                if (intShipAddCount > 3)
                                {
                                    for (int i = 3; i < strShipAddress.Count(); i++)
                                    {
                                        strShipAdd3 += " " + strShipAddress[i].Trim();
                                    }
                                }
                                if (strShipAdd3 != string.Empty)
                                {
                                    ShipAddress.AppendChild(inputXMLDoc.CreateElement("Addr3")).InnerText = objGenFunctions.QBEncode(strShipAdd3, 41);
                                }

                                if (!string.IsNullOrEmpty(dr["shipcity"].ToString().Trim()))
                                {
                                    ShipAddress.AppendChild(inputXMLDoc.CreateElement("City")).InnerText = objGenFunctions.QBEncode(dr["shipcity"].ToString(), 31);
                                }
                                if (!string.IsNullOrEmpty(dr["shipstate"].ToString().Trim()))
                                {
                                    ShipAddress.AppendChild(inputXMLDoc.CreateElement("State")).InnerText = objGenFunctions.QBEncode(dr["shipstate"].ToString(), 21);
                                }
                                if (!string.IsNullOrEmpty(dr["shipzip"].ToString().Trim()))
                                {
                                    ShipAddress.AppendChild(inputXMLDoc.CreateElement("PostalCode")).InnerText = objGenFunctions.QBEncode(dr["shipzip"].ToString(), 13);
                                }
                                #endregion ShipAddress

                                #region ShipTOAddress 


                                //string[] strShipToAddress = dr["ShipAddress"].ToString().Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                                //int intShipToAddCount = strShipToAddress.Count();
                                //XmlElement ShipTOAddress = inputXMLDoc.CreateElement("ShipToAddress");
                                //CustomerAdd.AppendChild(ShipTOAddress);

                                //ShipTOAddress.AppendChild(inputXMLDoc.CreateElement("Name")).InnerText = objGenFunctions.QBEncode(dr["tag"].ToString(), 41);

                                //if (intShipToAddCount > 0)
                                //{
                                //    if (!string.IsNullOrEmpty(strShipToAddress[0].Trim()))
                                //    {
                                //        ShipTOAddress.AppendChild(inputXMLDoc.CreateElement("Addr1")).InnerText = objGenFunctions.QBEncode(strShipToAddress[0].Trim(), 41);
                                //    }
                                //}
                                //if (intShipToAddCount > 1)
                                //{
                                //    if (!string.IsNullOrEmpty(strShipToAddress[1].Trim()))
                                //    {
                                //        ShipTOAddress.AppendChild(inputXMLDoc.CreateElement("Addr2")).InnerText = objGenFunctions.QBEncode(strShipToAddress[1].Trim(), 41);
                                //    }
                                //}
                                //string strShipToAdd3 = string.Empty;
                                //if (intShipToAddCount > 2)
                                //{
                                //    if (!string.IsNullOrEmpty(strShipToAddress[2].Trim()))
                                //    {
                                //        strShipToAdd3 = strShipToAddress[2].Trim();
                                //    }
                                //}
                                //if (intShipToAddCount > 3)
                                //{
                                //    for (int i = 3; i < strShipToAddress.Count(); i++)
                                //    {
                                //        strShipToAdd3 += " " + strShipToAddress[i].Trim();
                                //    }
                                //}
                                //if (strShipToAdd3 != string.Empty)
                                //{
                                //    ShipTOAddress.AppendChild(inputXMLDoc.CreateElement("Addr3")).InnerText = objGenFunctions.QBEncode(strShipToAdd3, 41);
                                //}

                                //if (!string.IsNullOrEmpty(dr["shipcity"].ToString().Trim()))
                                //{
                                //    ShipTOAddress.AppendChild(inputXMLDoc.CreateElement("City")).InnerText = objGenFunctions.QBEncode(dr["shipcity"].ToString(), 31);
                                //}
                                //if (!string.IsNullOrEmpty(dr["shipstate"].ToString().Trim()))
                                //{
                                //    ShipTOAddress.AppendChild(inputXMLDoc.CreateElement("State")).InnerText = objGenFunctions.QBEncode(dr["shipstate"].ToString(), 21);
                                //}
                                //if (!string.IsNullOrEmpty(dr["shipzip"].ToString().Trim()))
                                //{
                                //    ShipTOAddress.AppendChild(inputXMLDoc.CreateElement("PostalCode")).InnerText = objGenFunctions.QBEncode(dr["shipzip"].ToString(), 13);
                                //}

                                //ShipTOAddress.AppendChild(inputXMLDoc.CreateElement("DefaultShipTo")).InnerText = "1";

                                #endregion ShipTOAddress

                                if (!string.IsNullOrEmpty(dr["Phone"].ToString().Trim()))
                                {
                                    CustomerAdd.AppendChild(inputXMLDoc.CreateElement("Phone")).InnerText = objGenFunctions.QBEncode(dr["Phone"].ToString(), 21);
                                }

                                if (!string.IsNullOrEmpty(dr["Cellular"].ToString().Trim()))
                                {
                                    CustomerAdd.AppendChild(inputXMLDoc.CreateElement("Mobile")).InnerText = objGenFunctions.QBEncode(dr["Cellular"].ToString(), 21);
                                }

                                if (!string.IsNullOrEmpty(dr["Fax"].ToString().Trim()))
                                {
                                    CustomerAdd.AppendChild(inputXMLDoc.CreateElement("Fax")).InnerText = objGenFunctions.QBEncode(dr["Fax"].ToString(), 21);
                                }
                                if (!string.IsNullOrEmpty(dr["Email"].ToString().Trim()))
                                {
                                    CustomerAdd.AppendChild(inputXMLDoc.CreateElement("Email")).InnerText = objGenFunctions.QBEncode(dr["Email"].ToString(), 99);
                                }
                                #region Customertype
                                if (!string.IsNullOrEmpty(dr["QBCustomertypeID"].ToString().Trim()))
                                {
                                    XmlElement CustomerTypeRef = inputXMLDoc.CreateElement("CustomerTypeRef");
                                    CustomerAdd.AppendChild(CustomerTypeRef);
                                    CustomerTypeRef.AppendChild(inputXMLDoc.CreateElement("ListID")).InnerText = dr["QBCustomertypeID"].ToString();
                                }
                                #endregion Customertype

                                if (!string.IsNullOrEmpty(dr["Balance"].ToString().Trim()))
                                {
                                    CustomerAdd.AppendChild(inputXMLDoc.CreateElement("OpenBalance")).InnerText = objGenFunctions.QBEncode(dr["Balance"].ToString(), 30);
                                }

                                if (!string.IsNullOrEmpty(dr["QBstaxID"].ToString()))
                                {
                                    XmlElement ItemSalesTaxRef = inputXMLDoc.CreateElement("ItemSalesTaxRef");
                                    CustomerAdd.AppendChild(ItemSalesTaxRef);
                                    ItemSalesTaxRef.AppendChild(inputXMLDoc.CreateElement("ListID")).InnerText = dr["QBstaxID"].ToString();
                                }

                                if (!string.IsNullOrEmpty(dr["QBlocTypeID"].ToString()))
                                {
                                    XmlElement JobTypeRef = inputXMLDoc.CreateElement("JobTypeRef");
                                    CustomerAdd.AppendChild(JobTypeRef);
                                    JobTypeRef.AppendChild(inputXMLDoc.CreateElement("ListID")).InnerText = dr["QBlocTypeID"].ToString();
                                }
                            }
                        }
                    }
                    strRequestXML = inputXMLDoc.OuterXml;
                    req.Add(strRequestXML);
                    #endregion

                    // 23-24 Export/Update Location to QB
                    #region Export/Update Location to QB

                    DataSet dsUpdateLocToQB = new DataSet();
                    //if (Session["dsUpdateLocToQB"] == null)
                    //{
                    objProp_User.ConnConfig = objGenFunctions.ConnectionStr(Connection);
                    dsUpdateLocToQB = objBL_User.getQBLocation(objProp_User);
                    //    Session["dsUpdateLocToQB"] = dsUpdateLocToQB;
                    //}
                    //else
                    //{
                    //    dsUpdateLocToQB = (DataSet)Session["dsUpdateLocToQB"];
                    //}


                    #region Get EditSequence CustomerQuery
                    inputXMLDoc = new XmlDocument();
                    inputXMLDoc.AppendChild(inputXMLDoc.CreateXmlDeclaration("1.0", "ISO-8859-1", null));
                    inputXMLDoc.AppendChild(inputXMLDoc.CreateProcessingInstruction("qbxml", "version=\"" + qbxmlversion + "\""));
                    qbXML = inputXMLDoc.CreateElement("QBXML");
                    inputXMLDoc.AppendChild(qbXML);
                    qbXMLMsgsRq = inputXMLDoc.CreateElement("QBXMLMsgsRq");
                    qbXML.AppendChild(qbXMLMsgsRq);
                    qbXMLMsgsRq.SetAttribute("onError", "continueOnError");
                    if (dsUpdateLocToQB.Tables[0].Rows.Count > 0)
                    {
                        XmlElement CustomerQueryRqMod = inputXMLDoc.CreateElement("CustomerQueryRq");
                        qbXMLMsgsRq.AppendChild(CustomerQueryRqMod);
                        CustomerQueryRqMod.SetAttribute("requestID", "3");
                        
                        foreach (DataRow dr in dsUpdateLocToQB.Tables[0].Rows)
                        {
                            XmlElement XListID = inputXMLDoc.CreateElement("ListID");
                            CustomerQueryRqMod.AppendChild(XListID).InnerText = dr["QBLocID"].ToString();
                        }
                        // for getting only 3 columns
                        string[] retElements = new string[3] {
                            "ListID"
                            , "EditSequence"
                            , "TimeModified"
                        };
                        for (int i = 0; i < retElements.Length; i++)
                        {
                            XmlElement IncludeRetElement = inputXMLDoc.CreateElement("IncludeRetElement");
                            CustomerQueryRqMod.AppendChild(IncludeRetElement).InnerText = retElements[i];
                        }
                    }
                    strRequestXML = inputXMLDoc.OuterXml;
                    req.Add(strRequestXML);
                    #endregion

                    #region Mod Cust Query
                    inputXMLDoc = new XmlDocument();
                    inputXMLDoc.AppendChild(inputXMLDoc.CreateXmlDeclaration("1.0", "ISO-8859-1", null));
                    inputXMLDoc.AppendChild(inputXMLDoc.CreateProcessingInstruction("qbxml", "version=\"" + qbxmlversion + "\""));
                    qbXML = inputXMLDoc.CreateElement("QBXML");
                    inputXMLDoc.AppendChild(qbXML);
                    qbXMLMsgsRq = inputXMLDoc.CreateElement("QBXMLMsgsRq");
                    qbXML.AppendChild(qbXMLMsgsRq);
                    qbXMLMsgsRq.SetAttribute("onError", "continueOnError");

                    if (dsUpdateLocToQB.Tables[0].Rows.Count > 0)
                    {
                        DataTable dtModLocResponse = new DataTable();
                        if (Session["dtModLocResponse"] != null)
                        {
                            dtModLocResponse = (DataTable)Session["dtModLocResponse"];

                            foreach (DataRow dr in dsUpdateLocToQB.Tables[0].Rows)
                            {
                                DateTime lastUpdateDateQB = System.DateTime.MinValue;
                                //if (Session["ModifyDate"] != null)
                                //{
                                //    lastUpdateDateQB = Convert.ToDateTime(Session["ModifyDate"].ToString());
                                //}

                                string strEditseq = string.Empty;
                                //if (Session["EditSeqCust"] != null)
                                //{
                                //    strEditseq = Session["EditSeqCust"].ToString();
                                //}
                                foreach (DataRow drModCustResponse in dtModLocResponse.Rows)
                                {
                                    if (drModCustResponse["ListID"].ToString() == dr["QBLocID"].ToString())
                                    {
                                        lastUpdateDateQB = Convert.ToDateTime(drModCustResponse["TimeModified"].ToString());
                                        strEditseq = drModCustResponse["EditSequence"].ToString();

                                        string active = "";
                                        if (dr["Status"].ToString() == "1")
                                        {
                                            active = "0";
                                        }
                                        else
                                        {
                                            active = "1";
                                        }

                                        if (lastUpdateDateQB < Convert.ToDateTime(dr["LastUpdateDate"].ToString()))
                                        {
                                            XmlElement CustomerModRq = inputXMLDoc.CreateElement("CustomerModRq");
                                            qbXMLMsgsRq.AppendChild(CustomerModRq);
                                            CustomerModRq.SetAttribute("requestID", dr["id"].ToString());
                                            XmlElement CustomerMod = inputXMLDoc.CreateElement("CustomerMod");
                                            CustomerModRq.AppendChild(CustomerMod);

                                            CustomerMod.AppendChild(inputXMLDoc.CreateElement("ListID")).InnerText = dr["QBLocID"].ToString();
                                            CustomerMod.AppendChild(inputXMLDoc.CreateElement("EditSequence")).InnerText = strEditseq;
                                            CustomerMod.AppendChild(inputXMLDoc.CreateElement("Name")).InnerText = objGenFunctions.QBEncode(dr["tag"].ToString(), 41);
                                            CustomerMod.AppendChild(inputXMLDoc.CreateElement("IsActive")).InnerText = active;

                                            if (dr["QBLocID"].ToString() != dr["qbcustomerid"].ToString())
                                            {
                                                if (!string.IsNullOrEmpty(dr["qbcustomerid"].ToString().Trim()))
                                                {
                                                    XmlElement ParentRef = inputXMLDoc.CreateElement("ParentRef");
                                                    CustomerMod.AppendChild(ParentRef);
                                                    ParentRef.AppendChild(inputXMLDoc.CreateElement("ListID")).InnerText = dr["qbcustomerid"].ToString();
                                                }
                                            }
                                            CustomerMod.AppendChild(inputXMLDoc.CreateElement("CompanyName")).InnerText = objGenFunctions.QBEncode(dr["tag"].ToString(), 41);
                                            if (!string.IsNullOrEmpty(dr["Contact"].ToString().Trim()))
                                            {
                                                string[] contact = dr["Contact"].ToString().Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                                int contactLength = contact.Count();
                                                string firstname = string.Empty;
                                                string middlename = string.Empty;
                                                string lastname = string.Empty;

                                                if (contactLength > 0)
                                                {
                                                    firstname = contact[0].Trim();
                                                }
                                                if (contactLength == 2)
                                                {
                                                    lastname = contact[1].Trim();
                                                }
                                                if (contactLength > 2)
                                                {
                                                    middlename = contact[1].Trim();
                                                    lastname = contact[2].Trim();
                                                }

                                                if (firstname != string.Empty)
                                                {
                                                    CustomerMod.AppendChild(inputXMLDoc.CreateElement("FirstName")).InnerText = objGenFunctions.QBEncode(firstname, 25);
                                                }
                                                if (middlename != string.Empty)
                                                {
                                                    CustomerMod.AppendChild(inputXMLDoc.CreateElement("MiddleName")).InnerText = objGenFunctions.QBEncode(middlename, 5);
                                                }
                                                if (lastname != string.Empty)
                                                {
                                                    CustomerMod.AppendChild(inputXMLDoc.CreateElement("LastName")).InnerText = objGenFunctions.QBEncode(lastname, 25);
                                                }
                                            }
                                            //if (!string.IsNullOrEmpty(dr["Contact"].ToString().Trim()))
                                            //{
                                            //    //CustomerMod.AppendChild(inputXMLDoc.CreateElement("LastName")).InnerText = dr["Contact"].ToString();
                                            //}

                                            #region BillAddress


                                            string[] strBillAddress = dr["Address"].ToString().Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                                            int intBillAddCount = strBillAddress.Count();
                                            XmlElement BillAddress = inputXMLDoc.CreateElement("BillAddress");
                                            CustomerMod.AppendChild(BillAddress);

                                            if (intBillAddCount > 0)
                                            {
                                                if (!string.IsNullOrEmpty(strBillAddress[0].Trim()))
                                                {
                                                    BillAddress.AppendChild(inputXMLDoc.CreateElement("Addr1")).InnerText = objGenFunctions.QBEncode(strBillAddress[0].Trim(), 41);
                                                }
                                            }
                                            if (intBillAddCount > 1)
                                            {
                                                if (!string.IsNullOrEmpty(strBillAddress[1].Trim()))
                                                {
                                                    BillAddress.AppendChild(inputXMLDoc.CreateElement("Addr2")).InnerText = objGenFunctions.QBEncode(strBillAddress[1].Trim(), 41);
                                                }
                                            }
                                            string strAdd3 = string.Empty;
                                            if (intBillAddCount > 2)
                                            {
                                                if (!string.IsNullOrEmpty(strBillAddress[2].Trim()))
                                                {
                                                    strAdd3 = strBillAddress[2].Trim();
                                                }
                                            }
                                            if (intBillAddCount > 3)
                                            {
                                                for (int i = 3; i < strBillAddress.Count(); i++)
                                                {
                                                    strAdd3 += " " + strBillAddress[i].Trim();
                                                }
                                            }
                                            if (strAdd3 != string.Empty)
                                            {
                                                BillAddress.AppendChild(inputXMLDoc.CreateElement("Addr3")).InnerText = objGenFunctions.QBEncode(strAdd3, 41);
                                            }

                                            if (!string.IsNullOrEmpty(dr["city"].ToString().Trim()))
                                            {
                                                BillAddress.AppendChild(inputXMLDoc.CreateElement("City")).InnerText = objGenFunctions.QBEncode(dr["city"].ToString(), 31);
                                            }
                                            if (!string.IsNullOrEmpty(dr["State"].ToString().Trim()))
                                            {
                                                BillAddress.AppendChild(inputXMLDoc.CreateElement("State")).InnerText = objGenFunctions.QBEncode(dr["State"].ToString(), 21);
                                            }
                                            if (!string.IsNullOrEmpty(dr["zip"].ToString().Trim()))
                                            {
                                                BillAddress.AppendChild(inputXMLDoc.CreateElement("PostalCode")).InnerText = objGenFunctions.QBEncode(dr["zip"].ToString(), 13);
                                            }

                                            #endregion BillAddress

                                            #region ShipAddress

                                            string[] strShipAddress = dr["ShipAddress"].ToString().Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                                            int intShipAddCount = strShipAddress.Count();
                                            XmlElement ShipAddress = inputXMLDoc.CreateElement("ShipAddress");
                                            CustomerMod.AppendChild(ShipAddress);

                                            if (intShipAddCount > 0)
                                            {
                                                if (!string.IsNullOrEmpty(strShipAddress[0].Trim()))
                                                {
                                                    ShipAddress.AppendChild(inputXMLDoc.CreateElement("Addr1")).InnerText = objGenFunctions.QBEncode(strShipAddress[0].Trim(), 41);
                                                }
                                            }
                                            if (intShipAddCount > 1)
                                            {
                                                if (!string.IsNullOrEmpty(strShipAddress[1].Trim()))
                                                {
                                                    ShipAddress.AppendChild(inputXMLDoc.CreateElement("Addr2")).InnerText = objGenFunctions.QBEncode(strShipAddress[1].Trim(), 41);
                                                }
                                            }
                                            string strShipAdd3 = string.Empty;
                                            if (intShipAddCount > 2)
                                            {
                                                if (!string.IsNullOrEmpty(strShipAddress[2].Trim()))
                                                {
                                                    strShipAdd3 = strShipAddress[2].Trim();
                                                }
                                            }
                                            if (intShipAddCount > 3)
                                            {
                                                for (int i = 3; i < strShipAddress.Count(); i++)
                                                {
                                                    strShipAdd3 += " " + strShipAddress[i].Trim();
                                                }
                                            }
                                            if (strShipAdd3 != string.Empty)
                                            {
                                                ShipAddress.AppendChild(inputXMLDoc.CreateElement("Addr3")).InnerText = objGenFunctions.QBEncode(strShipAdd3, 41);
                                            }

                                            if (!string.IsNullOrEmpty(dr["shipcity"].ToString().Trim()))
                                            {
                                                ShipAddress.AppendChild(inputXMLDoc.CreateElement("City")).InnerText = objGenFunctions.QBEncode(dr["shipcity"].ToString(), 31);
                                            }
                                            if (!string.IsNullOrEmpty(dr["shipstate"].ToString().Trim()))
                                            {
                                                ShipAddress.AppendChild(inputXMLDoc.CreateElement("State")).InnerText = objGenFunctions.QBEncode(dr["shipstate"].ToString(), 21);
                                            }
                                            if (!string.IsNullOrEmpty(dr["shipzip"].ToString().Trim()))
                                            {
                                                ShipAddress.AppendChild(inputXMLDoc.CreateElement("PostalCode")).InnerText = objGenFunctions.QBEncode(dr["shipzip"].ToString(), 13);
                                            }

                                            #endregion ShipAddress


                                            if (!string.IsNullOrEmpty(dr["Phone"].ToString().Trim()))
                                            {
                                                CustomerMod.AppendChild(inputXMLDoc.CreateElement("Phone")).InnerText = objGenFunctions.QBEncode(dr["Phone"].ToString(), 21);
                                            }

                                            //if (!string.IsNullOrEmpty(dr["Cellular"].ToString().Trim()))
                                            //{
                                            //    CustomerMod.AppendChild(inputXMLDoc.CreateElement("Mobile")).InnerText = objGenFunctions.QBEncode(dr["Cellular"].ToString(), 21);
                                            //} 

                                            if (!string.IsNullOrEmpty(dr["Fax"].ToString().Trim()))
                                            {
                                                CustomerMod.AppendChild(inputXMLDoc.CreateElement("Fax")).InnerText = objGenFunctions.QBEncode(dr["Fax"].ToString(), 21);
                                            }
                                            if (!string.IsNullOrEmpty(dr["Email"].ToString().Trim()))
                                            {
                                                CustomerMod.AppendChild(inputXMLDoc.CreateElement("Email")).InnerText = objGenFunctions.QBEncode(dr["Email"].ToString(), 99);
                                            }

                                            #region Customertype
                                            if (!string.IsNullOrEmpty(dr["QBCustomertypeID"].ToString().Trim()))
                                            {
                                                XmlElement CustomerTypeRef = inputXMLDoc.CreateElement("CustomerTypeRef");
                                                CustomerMod.AppendChild(CustomerTypeRef);
                                                CustomerTypeRef.AppendChild(inputXMLDoc.CreateElement("ListID")).InnerText = dr["QBCustomertypeID"].ToString();
                                            }
                                            #endregion Customertype
                                            if (!string.IsNullOrEmpty(dr["QBstaxID"].ToString()))
                                            {
                                                XmlElement ItemSalesTaxRef = inputXMLDoc.CreateElement("ItemSalesTaxRef");
                                                CustomerMod.AppendChild(ItemSalesTaxRef);
                                                ItemSalesTaxRef.AppendChild(inputXMLDoc.CreateElement("ListID")).InnerText = dr["QBstaxID"].ToString();
                                            }
                                            if (!string.IsNullOrEmpty(dr["QBlocTypeID"].ToString().Trim()))
                                            {
                                                XmlElement JobTypeRef = inputXMLDoc.CreateElement("JobTypeRef");
                                                CustomerMod.AppendChild(JobTypeRef);
                                                JobTypeRef.AppendChild(inputXMLDoc.CreateElement("ListID")).InnerText = dr["QBlocTypeID"].ToString();
                                            }
                                        }
                                    }
                                }
                            }
                            // Clean session
                            if (isSendingRequest) Session["dtModLocResponse"] = null;
                        }
                    }
                    strRequestXML = inputXMLDoc.OuterXml;
                    req.Add(strRequestXML);
                    #endregion


                    #endregion

                    // 25 Export/Add Department-Class to QB
                    #region Export/Add Department-Class to QB

                    DataSet dsNewMSMDepartment = new DataSet();
                    objProp_User.ConnConfig = objGenFunctions.ConnectionStr(Connection);
                    dsNewMSMDepartment = objBL_User.getMSMDepartment(objProp_User);


                    inputXMLDoc = new XmlDocument();
                    inputXMLDoc.AppendChild(inputXMLDoc.CreateXmlDeclaration("1.0", "ISO-8859-1", null));
                    inputXMLDoc.AppendChild(inputXMLDoc.CreateProcessingInstruction("qbxml", "version=\"" + qbxmlversion + "\""));
                    qbXML = inputXMLDoc.CreateElement("QBXML");
                    inputXMLDoc.AppendChild(qbXML);
                    qbXMLMsgsRq = inputXMLDoc.CreateElement("QBXMLMsgsRq");
                    qbXML.AppendChild(qbXMLMsgsRq);
                    qbXMLMsgsRq.SetAttribute("onError", "continueOnError");

                    if (dsNewMSMDepartment.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dsNewMSMDepartment.Tables[0].Rows)
                        {
                            XmlElement ClassAddRq = inputXMLDoc.CreateElement("ClassAddRq");
                            qbXMLMsgsRq.AppendChild(ClassAddRq);
                            ClassAddRq.SetAttribute("requestID", dr["id"].ToString());
                            XmlElement ClassAdd = inputXMLDoc.CreateElement("ClassAdd");
                            ClassAddRq.AppendChild(ClassAdd);
                            ClassAdd.AppendChild(inputXMLDoc.CreateElement("Name")).InnerText = objGenFunctions.QBEncode(dr["type"].ToString().Split(':')[0], 31);
                        }
                    }
                    strRequestXML = inputXMLDoc.OuterXml;
                    req.Add(strRequestXML);

                    #endregion

                    // 26-27 Export/Update Department-Class to QB
                    #region Export/Update Department-Class to QB

                    DataSet dsUpdateDepartmentToQB = new DataSet();
                    objProp_User.ConnConfig = objGenFunctions.ConnectionStr(Connection);
                    dsUpdateDepartmentToQB = objBL_User.getQBDepartment(objProp_User);


                    #region Get EditSequence ClassQuery
                    inputXMLDoc = new XmlDocument();
                    inputXMLDoc.AppendChild(inputXMLDoc.CreateXmlDeclaration("1.0", "ISO-8859-1", null));
                    inputXMLDoc.AppendChild(inputXMLDoc.CreateProcessingInstruction("qbxml", "version=\"" + qbxmlversion + "\""));
                    qbXML = inputXMLDoc.CreateElement("QBXML");
                    inputXMLDoc.AppendChild(qbXML);
                    qbXMLMsgsRq = inputXMLDoc.CreateElement("QBXMLMsgsRq");
                    qbXML.AppendChild(qbXMLMsgsRq);
                    qbXMLMsgsRq.SetAttribute("onError", "continueOnError");

                    if (dsUpdateDepartmentToQB.Tables[0].Rows.Count > 0)
                    {
                        XmlElement CustomerQueryRqMod = inputXMLDoc.CreateElement("ClassQueryRq");
                        qbXMLMsgsRq.AppendChild(CustomerQueryRqMod);
                        CustomerQueryRqMod.SetAttribute("requestID", "2");
                        foreach (DataRow dr in dsUpdateDepartmentToQB.Tables[0].Rows)
                        {
                            XmlElement XListID = inputXMLDoc.CreateElement("ListID");
                            CustomerQueryRqMod.AppendChild(XListID).InnerText = dr["QBJobTypeID"].ToString();
                        }
                        string[] retElements = new string[3] {
                            "ListID"
                            , "EditSequence"
                            , "TimeModified"
                        };
                        for (int i = 0; i < retElements.Length; i++)
                        {
                            XmlElement IncludeRetElement = inputXMLDoc.CreateElement("IncludeRetElement");
                            CustomerQueryRqMod.AppendChild(IncludeRetElement).InnerText = retElements[i];
                        }
                    }
                    strRequestXML = inputXMLDoc.OuterXml;
                    req.Add(strRequestXML);
                    #endregion

                    #region Mod Class Query
                    inputXMLDoc = new XmlDocument();
                    inputXMLDoc.AppendChild(inputXMLDoc.CreateXmlDeclaration("1.0", "ISO-8859-1", null));
                    inputXMLDoc.AppendChild(inputXMLDoc.CreateProcessingInstruction("qbxml", "version=\"" + qbxmlversion + "\""));
                    qbXML = inputXMLDoc.CreateElement("QBXML");
                    inputXMLDoc.AppendChild(qbXML);
                    qbXMLMsgsRq = inputXMLDoc.CreateElement("QBXMLMsgsRq");
                    qbXML.AppendChild(qbXMLMsgsRq);
                    qbXMLMsgsRq.SetAttribute("onError", "continueOnError");

                    if (dsUpdateDepartmentToQB.Tables[0].Rows.Count > 0)
                    {
                        DataTable dtModCustResponse = new DataTable();
                        if (Session["dtModClassResponse"] != null)
                        {
                            dtModCustResponse = (DataTable)Session["dtModClassResponse"];

                            foreach (DataRow dr in dsUpdateDepartmentToQB.Tables[0].Rows)
                            {
                                DateTime lastUpdateDateQB = System.DateTime.MinValue;
                                string strEditseq = string.Empty;
                                foreach (DataRow drModCustResponse in dtModCustResponse.Rows)
                                {
                                    if (drModCustResponse["ListID"].ToString() == dr["QBjobtypeID"].ToString())
                                    {
                                        lastUpdateDateQB = Convert.ToDateTime(drModCustResponse["TimeModified"].ToString());
                                        strEditseq = drModCustResponse["EditSequence"].ToString();

                                        if (lastUpdateDateQB < Convert.ToDateTime(dr["LastUpdateDate"].ToString()))
                                        {
                                            XmlElement CustomerModRq = inputXMLDoc.CreateElement("ClassModRq");
                                            qbXMLMsgsRq.AppendChild(CustomerModRq);
                                            CustomerModRq.SetAttribute("requestID", dr["id"].ToString());
                                            XmlElement CustomerMod = inputXMLDoc.CreateElement("ClassMod");
                                            CustomerModRq.AppendChild(CustomerMod);

                                            CustomerMod.AppendChild(inputXMLDoc.CreateElement("ListID")).InnerText = dr["QBJobtypeID"].ToString();
                                            CustomerMod.AppendChild(inputXMLDoc.CreateElement("EditSequence")).InnerText = strEditseq;
                                            CustomerMod.AppendChild(inputXMLDoc.CreateElement("Name")).InnerText = objGenFunctions.QBEncode(dr["type"].ToString(), 31);
                                        }
                                    }
                                }
                            }

                            // Clean session
                            if (isSendingRequest) Session["dtModClassResponse"] = null;
                        }
                    }
                    strRequestXML = inputXMLDoc.OuterXml;
                    req.Add(strRequestXML);
                    #endregion

                    #endregion

                    // 28 Export/Add Billcode-items to QB
                    #region Export/Add Billcode-items to QB

                    DataSet dsNewMSMBillcode = new DataSet();
                    objProp_User.ConnConfig = objGenFunctions.ConnectionStr(Connection);
                    objProp_User.SearchValue = "0";
                    dsNewMSMBillcode = objBL_User.getMSMBillcode(objProp_User);

                    inputXMLDoc = new XmlDocument();
                    inputXMLDoc.AppendChild(inputXMLDoc.CreateXmlDeclaration("1.0", "ISO-8859-1", null));
                    inputXMLDoc.AppendChild(inputXMLDoc.CreateProcessingInstruction("qbxml", "version=\"" + qbxmlversion + "\""));
                    qbXML = inputXMLDoc.CreateElement("QBXML");
                    inputXMLDoc.AppendChild(qbXML);
                    qbXMLMsgsRq = inputXMLDoc.CreateElement("QBXMLMsgsRq");
                    qbXML.AppendChild(qbXMLMsgsRq);
                    qbXMLMsgsRq.SetAttribute("onError", "continueOnError");

                    if (dsNewMSMBillcode.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dsNewMSMBillcode.Tables[0].Rows)
                        {
                            XmlElement ItemServiceAddRq = inputXMLDoc.CreateElement("ItemServiceAddRq");
                            qbXMLMsgsRq.AppendChild(ItemServiceAddRq);
                            ItemServiceAddRq.SetAttribute("requestID", dr["id"].ToString());
                            XmlElement ItemServiceAdd = inputXMLDoc.CreateElement("ItemServiceAdd");
                            ItemServiceAddRq.AppendChild(ItemServiceAdd);
                            string active = "";
                            if (dr["cat"].ToString() == "1")
                            {
                                active = "0";
                            }
                            else
                            {
                                active = "1";
                            }
                            ItemServiceAdd.AppendChild(inputXMLDoc.CreateElement("Name")).InnerText = objGenFunctions.QBEncode(dr["name"].ToString(), 31);
                            ItemServiceAdd.AppendChild(inputXMLDoc.CreateElement("IsActive")).InnerText = active;
                            XmlElement SalesOrPurchase = inputXMLDoc.CreateElement("SalesOrPurchase");
                            ItemServiceAdd.AppendChild(SalesOrPurchase);
                            SalesOrPurchase.AppendChild(inputXMLDoc.CreateElement("Price")).InnerText = dr["Price1"].ToString();
                            XmlElement AccountRef = inputXMLDoc.CreateElement("AccountRef");
                            SalesOrPurchase.AppendChild(AccountRef);
                            AccountRef.AppendChild(inputXMLDoc.CreateElement("ListID")).InnerText = dr["qbaccountid"].ToString();

                        }
                    }
                    strRequestXML = inputXMLDoc.OuterXml;
                    req.Add(strRequestXML);

                    #endregion

                    // 29-30 Export/Update Billcode to QB
                    #region Export/Update Billcode to QB

                    DataSet dsUpdateBillcodeToQB = new DataSet();
                    objProp_User.ConnConfig = objGenFunctions.ConnectionStr(Connection);
                    objProp_User.SearchValue = "1";
                    dsUpdateBillcodeToQB = objBL_User.getMSMBillcode(objProp_User);


                    #region Get EditSequence ClassQuery
                    inputXMLDoc = new XmlDocument();
                    inputXMLDoc.AppendChild(inputXMLDoc.CreateXmlDeclaration("1.0", "ISO-8859-1", null));
                    inputXMLDoc.AppendChild(inputXMLDoc.CreateProcessingInstruction("qbxml", "version=\"" + qbxmlversion + "\""));
                    qbXML = inputXMLDoc.CreateElement("QBXML");
                    inputXMLDoc.AppendChild(qbXML);
                    qbXMLMsgsRq = inputXMLDoc.CreateElement("QBXMLMsgsRq");
                    qbXML.AppendChild(qbXMLMsgsRq);
                    qbXMLMsgsRq.SetAttribute("onError", "continueOnError");

                    if (dsUpdateBillcodeToQB.Tables[0].Rows.Count > 0)
                    {
                        XmlElement ItemServiceQueryRq = inputXMLDoc.CreateElement("ItemServiceQueryRq");
                        qbXMLMsgsRq.AppendChild(ItemServiceQueryRq);
                        ItemServiceQueryRq.SetAttribute("requestID", "3");

                        foreach (DataRow dr in dsUpdateBillcodeToQB.Tables[0].Rows)
                        {
                            XmlElement XListID = inputXMLDoc.CreateElement("ListID");
                            ItemServiceQueryRq.AppendChild(XListID).InnerText = dr["QBInvID"].ToString();
                        }
                        string[] retElements = new string[3] {
                            "ListID"
                            , "EditSequence"
                            , "TimeModified"
                        };
                        for (int i = 0; i < retElements.Length; i++)
                        {
                            XmlElement IncludeRetElement = inputXMLDoc.CreateElement("IncludeRetElement");
                            ItemServiceQueryRq.AppendChild(IncludeRetElement).InnerText = retElements[i];
                        }
                    }
                    strRequestXML = inputXMLDoc.OuterXml;
                    req.Add(strRequestXML);
                    #endregion

                    #region Mod Class Query
                    inputXMLDoc = new XmlDocument();
                    inputXMLDoc.AppendChild(inputXMLDoc.CreateXmlDeclaration("1.0", "ISO-8859-1", null));
                    inputXMLDoc.AppendChild(inputXMLDoc.CreateProcessingInstruction("qbxml", "version=\"" + qbxmlversion + "\""));
                    qbXML = inputXMLDoc.CreateElement("QBXML");
                    inputXMLDoc.AppendChild(qbXML);
                    qbXMLMsgsRq = inputXMLDoc.CreateElement("QBXMLMsgsRq");
                    qbXML.AppendChild(qbXMLMsgsRq);
                    qbXMLMsgsRq.SetAttribute("onError", "continueOnError");

                    if (dsUpdateBillcodeToQB.Tables[0].Rows.Count > 0)
                    {
                        DataTable dtModItemServiceResponse = new DataTable();
                        if (Session["dtModItemServiceResponse"] != null)
                        {
                            dtModItemServiceResponse = (DataTable)Session["dtModItemServiceResponse"];

                            foreach (DataRow dr in dsUpdateBillcodeToQB.Tables[0].Rows)
                            {
                                DateTime lastUpdateDateQB = System.DateTime.MinValue;
                                string strEditseq = string.Empty;
                                foreach (DataRow drModCustResponse in dtModItemServiceResponse.Rows)
                                {
                                    if (drModCustResponse["ListID"].ToString() == dr["QBinvID"].ToString())
                                    {
                                        lastUpdateDateQB = Convert.ToDateTime(drModCustResponse["TimeModified"].ToString());
                                        strEditseq = drModCustResponse["EditSequence"].ToString();

                                        if (lastUpdateDateQB < Convert.ToDateTime(dr["LastUpdateDate"].ToString()))
                                        {
                                            XmlElement ItemServiceModRq = inputXMLDoc.CreateElement("ItemServiceModRq");
                                            qbXMLMsgsRq.AppendChild(ItemServiceModRq);
                                            ItemServiceModRq.SetAttribute("requestID", dr["id"].ToString());
                                            XmlElement ItemServiceMod = inputXMLDoc.CreateElement("ItemServiceMod");
                                            ItemServiceModRq.AppendChild(ItemServiceMod);

                                            ItemServiceMod.AppendChild(inputXMLDoc.CreateElement("ListID")).InnerText = dr["QBinvID"].ToString();
                                            ItemServiceMod.AppendChild(inputXMLDoc.CreateElement("EditSequence")).InnerText = strEditseq;
                                            ItemServiceMod.AppendChild(inputXMLDoc.CreateElement("Name")).InnerText = objGenFunctions.QBEncode(dr["name"].ToString(), 31);
                                            string active = "";
                                            if (dr["cat"].ToString() == "1")
                                            {
                                                active = "0";
                                            }
                                            else
                                            {
                                                active = "1";
                                            }
                                            //ItemServiceMod.AppendChild(inputXMLDoc.CreateElement("IsActive")).InnerText = active;

                                            XmlElement SalesOrPurchase = inputXMLDoc.CreateElement("SalesOrPurchaseMod");
                                            ItemServiceMod.AppendChild(SalesOrPurchase);
                                            SalesOrPurchase.AppendChild(inputXMLDoc.CreateElement("Price")).InnerText = dr["Price1"].ToString();

                                            //XmlElement AccountRef = inputXMLDoc.CreateElement("AccountRef");
                                            //SalesOrPurchase.AppendChild(AccountRef);
                                            //AccountRef.AppendChild(inputXMLDoc.CreateElement("ListID")).InnerText = dr["qbaccountid"].ToString();
                                        }
                                    }
                                }
                            }

                            // Clean session
                            if (isSendingRequest) Session["dtModItemServiceResponse"] = null;
                        }
                    }
                    strRequestXML = inputXMLDoc.OuterXml;
                    req.Add(strRequestXML);
                    #endregion

                    #endregion

                    // 31-32 Export/Add Terms to QB
                    #region Export/Add Terms to QB

                    DataSet dsNewMSMTerms = new DataSet();

                    objProp_User.ConnConfig = objGenFunctions.ConnectionStr(Connection);
                    objProp_User.SearchValue = string.Empty;
                    dsNewMSMTerms = objBL_User.getMSMterms(objProp_User);

                    inputXMLDoc = new XmlDocument();
                    inputXMLDoc.AppendChild(inputXMLDoc.CreateXmlDeclaration("1.0", "ISO-8859-1", null));
                    inputXMLDoc.AppendChild(inputXMLDoc.CreateProcessingInstruction("qbxml", "version=\"" + qbxmlversion + "\""));
                    qbXML = inputXMLDoc.CreateElement("QBXML");
                    inputXMLDoc.AppendChild(qbXML);
                    qbXMLMsgsRq = inputXMLDoc.CreateElement("QBXMLMsgsRq");
                    qbXML.AppendChild(qbXMLMsgsRq);
                    qbXMLMsgsRq.SetAttribute("onError", "continueOnError");
                    if (dsNewMSMTerms.Tables[0].Rows.Count > 0)
                    {
                        if (Session["duplicateTermsID"] == null)
                        {
                            foreach (DataRow dr in dsNewMSMTerms.Tables[0].Rows)
                            {
                                XmlElement ItemServiceAdd = inputXMLDoc.CreateElement("StandardTermsAddRq");
                                qbXMLMsgsRq.AppendChild(ItemServiceAdd);
                                ItemServiceAdd.SetAttribute("requestID", dr["id"].ToString());
                                XmlElement ItemService = inputXMLDoc.CreateElement("StandardTermsAdd");
                                ItemServiceAdd.AppendChild(ItemService);
                                ItemService.AppendChild(inputXMLDoc.CreateElement("Name")).InnerText = objGenFunctions.QBEncode(dr["name"].ToString(), 31);
                            }
                        }
                    }
                    strRequestXML = inputXMLDoc.OuterXml;
                    req.Add(strRequestXML);



                    inputXMLDoc = new XmlDocument();
                    inputXMLDoc.AppendChild(inputXMLDoc.CreateXmlDeclaration("1.0", "ISO-8859-1", null));
                    inputXMLDoc.AppendChild(inputXMLDoc.CreateProcessingInstruction("qbxml", "version=\"" + qbxmlversion + "\""));
                    qbXML = inputXMLDoc.CreateElement("QBXML");
                    inputXMLDoc.AppendChild(qbXML);
                    qbXMLMsgsRq = inputXMLDoc.CreateElement("QBXMLMsgsRq");
                    qbXML.AppendChild(qbXMLMsgsRq);
                    qbXMLMsgsRq.SetAttribute("onError", "continueOnError");

                    if (Session["duplicateTermsID"] != null)
                    {
                        List<int> termsid = (List<int>)Session["duplicateTermsID"];
                        string strTermsID = string.Join(",", Array.ConvertAll(termsid.ToArray(), x => x.ToString()));
                        objProp_User.SearchValue = strTermsID;
                        objProp_User.ConnConfig = objGenFunctions.ConnectionStr(Connection);
                        dsNewMSMTerms = objBL_User.getMSMterms(objProp_User);

                        if (dsNewMSMTerms.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow dr in dsNewMSMTerms.Tables[0].Rows)
                            {
                                XmlElement ItemServiceAdd = inputXMLDoc.CreateElement("StandardTermsAddRq");
                                qbXMLMsgsRq.AppendChild(ItemServiceAdd);
                                ItemServiceAdd.SetAttribute("requestID", dr["id"].ToString());
                                XmlElement ItemService = inputXMLDoc.CreateElement("StandardTermsAdd");
                                ItemServiceAdd.AppendChild(ItemService);
                                ItemService.AppendChild(inputXMLDoc.CreateElement("Name")).InnerText = objGenFunctions.QBEncode(dr["dupname"].ToString(), 31);
                            }
                        }
                        //Session["duplicateTermsID"] = null;
                    }
                    strRequestXML = inputXMLDoc.OuterXml;
                    req.Add(strRequestXML);

                    #endregion

                }

                // 33 Export/Add Invoice to QB
                #region Export/Add Invoice to QB

                inputXMLDoc = new XmlDocument();
                inputXMLDoc.AppendChild(inputXMLDoc.CreateXmlDeclaration("1.0", "ISO-8859-1", null));
                inputXMLDoc.AppendChild(inputXMLDoc.CreateProcessingInstruction("qbxml", "version=\"" + qbxmlversion + "\""));
                qbXML = inputXMLDoc.CreateElement("QBXML");
                inputXMLDoc.AppendChild(qbXML);
                qbXMLMsgsRq = inputXMLDoc.CreateElement("QBXMLMsgsRq");
                qbXML.AppendChild(qbXMLMsgsRq);
                qbXMLMsgsRq.SetAttribute("onError", "continueOnError");

                if (TransferInvoice == 1)
                {
                    DataSet dsNewMSMInvoice = new DataSet();
                    objMapData.ConnConfig = objGenFunctions.ConnectionStr(Connection);
                    dsNewMSMInvoice = objBL_MapData.GetChargeableTickets(objMapData);

                    if (dsNewMSMInvoice.Tables[0].Rows.Count > 0)
                    {
                        objProp_Contracts.ConnConfig = objGenFunctions.ConnectionStr(Connection);
                        int Refno = objBL_Contracts.GetMaxQBInvoiceID(objProp_Contracts);

                        foreach (DataRow dr in dsNewMSMInvoice.Tables[0].Rows)
                        {
                            string strRefNo = Refno.ToString();
                            Refno++;

                            XmlElement InvoiceAddRq = inputXMLDoc.CreateElement("InvoiceAddRq");
                            qbXMLMsgsRq.AppendChild(InvoiceAddRq);
                            InvoiceAddRq.SetAttribute("requestID", dr["ticketid"].ToString());
                            XmlElement InvoiceAdd = inputXMLDoc.CreateElement("InvoiceAdd");
                            InvoiceAddRq.AppendChild(InvoiceAdd);

                            XmlElement CustomerRef = inputXMLDoc.CreateElement("CustomerRef");
                            InvoiceAdd.AppendChild(CustomerRef);
                            CustomerRef.AppendChild(inputXMLDoc.CreateElement("ListID")).InnerText = dr["QBCustID"].ToString();

                            if (!string.IsNullOrEmpty(dr["QBJobTypeID"].ToString().Trim()))
                            {
                                XmlElement ClassRef = inputXMLDoc.CreateElement("ClassRef");
                                InvoiceAdd.AppendChild(ClassRef);
                                ClassRef.AppendChild(inputXMLDoc.CreateElement("ListID")).InnerText = dr["QBJobTypeID"].ToString();
                            }

                            InvoiceAdd.AppendChild(inputXMLDoc.CreateElement("TxnDate")).InnerText = System.DateTime.Now.ToString("yyyy-MM-dd");
                            //InvoiceAdd.AppendChild(inputXMLDoc.CreateElement("RefNumber")).InnerText ="M-"+ dr["ticketID"].ToString();                        
                            InvoiceAdd.AppendChild(inputXMLDoc.CreateElement("RefNumber")).InnerText = strRefNo;

                            #region address

                            string[] strBillAddress = dr["Address"].ToString().Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                            int intBillAddCount = strBillAddress.Count();
                            XmlElement BillAddress = inputXMLDoc.CreateElement("BillAddress");
                            InvoiceAdd.AppendChild(BillAddress);

                            if (intBillAddCount > 0)
                            {
                                if (!string.IsNullOrEmpty(strBillAddress[0].Trim()))
                                {
                                    BillAddress.AppendChild(inputXMLDoc.CreateElement("Addr1")).InnerText = objGenFunctions.QBEncode(strBillAddress[0].Trim(), 41);
                                }
                            }
                            if (intBillAddCount > 1)
                            {
                                if (!string.IsNullOrEmpty(strBillAddress[1].Trim()))
                                {
                                    BillAddress.AppendChild(inputXMLDoc.CreateElement("Addr2")).InnerText = objGenFunctions.QBEncode(strBillAddress[1].Trim(), 41);
                                }
                            }
                            string strAdd3 = string.Empty;
                            if (intBillAddCount > 2)
                            {
                                if (!string.IsNullOrEmpty(strBillAddress[2].Trim()))
                                {
                                    strAdd3 = strBillAddress[2].Trim();
                                }
                            }
                            if (intBillAddCount > 3)
                            {
                                for (int i = 3; i < strBillAddress.Count(); i++)
                                {
                                    strAdd3 += " " + strBillAddress[i].Trim();
                                }
                            }
                            if (strAdd3 != string.Empty)
                            {
                                BillAddress.AppendChild(inputXMLDoc.CreateElement("Addr3")).InnerText = objGenFunctions.QBEncode(strAdd3, 41);
                            }

                            if (!string.IsNullOrEmpty(dr["city"].ToString().Trim()))
                            {
                                BillAddress.AppendChild(inputXMLDoc.CreateElement("City")).InnerText = objGenFunctions.QBEncode(dr["city"].ToString(), 31);
                            }
                            if (!string.IsNullOrEmpty(dr["State"].ToString().Trim()))
                            {
                                BillAddress.AppendChild(inputXMLDoc.CreateElement("State")).InnerText = objGenFunctions.QBEncode(dr["State"].ToString(), 21);
                            }
                            if (!string.IsNullOrEmpty(dr["zip"].ToString().Trim()))
                            {
                                BillAddress.AppendChild(inputXMLDoc.CreateElement("PostalCode")).InnerText = objGenFunctions.QBEncode(dr["zip"].ToString(), 13);
                            }

                            #endregion

                            InvoiceAdd.AppendChild(inputXMLDoc.CreateElement("IsPending")).InnerText = "1";
                            //InvoiceAdd.AppendChild(inputXMLDoc.CreateElement("PONumber")).InnerText = "";

                            if (!string.IsNullOrEmpty(dr["QBTermsID"].ToString().Trim()))
                            {
                                XmlElement TermsRef = inputXMLDoc.CreateElement("TermsRef");
                                InvoiceAdd.AppendChild(TermsRef);
                                TermsRef.AppendChild(inputXMLDoc.CreateElement("ListID")).InnerText = dr["QBTermsID"].ToString();
                            }

                            InvoiceAdd.AppendChild(inputXMLDoc.CreateElement("DueDate")).InnerText = System.DateTime.Now.ToString("yyyy-MM-dd");

                            if (!string.IsNullOrEmpty(dr["QBStaxID"].ToString().Trim()))
                            {
                                XmlElement ItemSalesTaxRef = inputXMLDoc.CreateElement("ItemSalesTaxRef");
                                InvoiceAdd.AppendChild(ItemSalesTaxRef);
                                ItemSalesTaxRef.AppendChild(inputXMLDoc.CreateElement("ListID")).InnerText = dr["QBStaxID"].ToString();
                            }

                            InvoiceAdd.AppendChild(inputXMLDoc.CreateElement("Memo")).InnerText = "Ticket ID: " + dr["ticketID"].ToString();//+ Environment.NewLine + dr["descres"].ToString();

                            #region line items
                            double totalTime = Convert.ToDouble(dr["total"]);
                            double Expenses = Convert.ToDouble(dr["othere"]) + Convert.ToDouble(dr["toll"]) + Convert.ToDouble(dr["zone"]);
                            int mileage = Convert.ToInt32(dr["emile"]) - Convert.ToInt32(dr["Smile"]);

                            DataTable dtLineItem = BuildLineItems(totalTime, Expenses, mileage, Convert.ToString(dr["QBServiceItem"]), Convert.ToString(dr["descres"]), MileageItem, LaborItem, ExpenseItem).Tables[0];

                            foreach (DataRow drLineItem in dtLineItem.Rows)
                            {
                                double Rate = Convert.ToDouble(drLineItem["price"].ToString());
                                double Amount = Convert.ToDouble(drLineItem["Amount"].ToString());

                                XmlElement InvoiceLineAdd = inputXMLDoc.CreateElement("InvoiceLineAdd");
                                InvoiceAdd.AppendChild(InvoiceLineAdd);

                                XmlElement ItemRef = inputXMLDoc.CreateElement("ItemRef");
                                InvoiceLineAdd.AppendChild(ItemRef);
                                ItemRef.AppendChild(inputXMLDoc.CreateElement("ListID")).InnerText = drLineItem["qbinvid"].ToString();

                                InvoiceLineAdd.AppendChild(inputXMLDoc.CreateElement("Desc")).InnerText = objGenFunctions.QBEncode(drLineItem["fdesc"].ToString().Trim(), 4090);
                                InvoiceLineAdd.AppendChild(inputXMLDoc.CreateElement("Quantity")).InnerText = drLineItem["Quan"].ToString();
                                InvoiceLineAdd.AppendChild(inputXMLDoc.CreateElement("Rate")).InnerText = Rate.ToString("0.00");
                                InvoiceLineAdd.AppendChild(inputXMLDoc.CreateElement("Amount")).InnerText = Amount.ToString("0.00");
                                //InvoiceLineAdd.AppendChild(inputXMLDoc.CreateElement("IsTaxable")).InnerText = "0";
                            }
                            #endregion
                        }
                    }
                }
                strRequestXML = inputXMLDoc.OuterXml;
                req.Add(strRequestXML);
                //using (System.IO.StreamWriter file = new System.IO.StreamWriter(Server.MapPath(this.Context.Request.ApplicationPath) + "/qblog.txt", true))
                //{
                //    file.WriteLine(strRequestXML);
                //}
                #endregion

                if (QBWCSyncDirection == 0 || QBWCSyncDirection == 2)
                {
                    // 34 Export/Add TimeSheet to QB
                    #region Export/Add TimeSheet to QB

                    inputXMLDoc = new XmlDocument();
                    inputXMLDoc.AppendChild(inputXMLDoc.CreateXmlDeclaration("1.0", "ISO-8859-1", null));
                    inputXMLDoc.AppendChild(inputXMLDoc.CreateProcessingInstruction("qbxml", "version=\"" + qbxmlversion + "\""));
                    qbXML = inputXMLDoc.CreateElement("QBXML");
                    inputXMLDoc.AppendChild(qbXML);
                    qbXMLMsgsRq = inputXMLDoc.CreateElement("QBXMLMsgsRq");
                    qbXML.AppendChild(qbXMLMsgsRq);
                    qbXMLMsgsRq.SetAttribute("onError", "continueOnError");

                    //if (TransferTimesheet == 1)
                    //{
                    DataSet dsNewMSMTimeSheet = new DataSet();

                    objMapData.ConnConfig = objGenFunctions.ConnectionStr(Connection);
                    objMapData.SearchValue = "0";
                    dsNewMSMTimeSheet = objBL_MapData.GetTicketTime(objMapData);

                    if (dsNewMSMTimeSheet.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dsNewMSMTimeSheet.Tables[0].Rows)
                        {
                            #region regular
                            if (Convert.ToDouble(dr["reg"]) != 0)
                            {
                                XmlElement TimeTrackingAddRq = inputXMLDoc.CreateElement("TimeTrackingAddRq");
                                qbXMLMsgsRq.AppendChild(TimeTrackingAddRq);
                                TimeTrackingAddRq.SetAttribute("requestID", dr["id"].ToString() + "_rt");
                                XmlElement TimeTrackingAdd = inputXMLDoc.CreateElement("TimeTrackingAdd");
                                TimeTrackingAddRq.AppendChild(TimeTrackingAdd);
                                TimeTrackingAdd.AppendChild(inputXMLDoc.CreateElement("TxnDate")).InnerText = Convert.ToDateTime(dr["edate"]).ToString("yyyy-MM-dd");

                                XmlElement EntityRef = inputXMLDoc.CreateElement("EntityRef");
                                TimeTrackingAdd.AppendChild(EntityRef);
                                EntityRef.AppendChild(inputXMLDoc.CreateElement("ListID")).InnerText = dr["QBEmployeeID"].ToString();

                                XmlElement CustomerRef = inputXMLDoc.CreateElement("CustomerRef");
                                TimeTrackingAdd.AppendChild(CustomerRef);
                                CustomerRef.AppendChild(inputXMLDoc.CreateElement("ListID")).InnerText = dr["QBcustID"].ToString();

                                XmlElement ItemServiceRef = inputXMLDoc.CreateElement("ItemServiceRef");
                                TimeTrackingAdd.AppendChild(ItemServiceRef);
                                ItemServiceRef.AppendChild(inputXMLDoc.CreateElement("ListID")).InnerText = dr["QBitemID"].ToString();

                                var timespan = TimeSpan.FromHours(Convert.ToDouble(dr["reg"]));
                                var hours = timespan.Hours;
                                var minutes = timespan.Minutes;

                                string totaltime = "PT" + hours.ToString() + "H" + minutes.ToString() + "M0S";

                                TimeTrackingAdd.AppendChild(inputXMLDoc.CreateElement("Duration")).InnerText = totaltime;

                                XmlElement PayrollItemWageRef = inputXMLDoc.CreateElement("PayrollItemWageRef");
                                TimeTrackingAdd.AppendChild(PayrollItemWageRef);
                                PayrollItemWageRef.AppendChild(inputXMLDoc.CreateElement("ListID")).InnerText = dr["QBwageID"].ToString();

                                TimeTrackingAdd.AppendChild(inputXMLDoc.CreateElement("Notes")).InnerText = "Regular Time" + Environment.NewLine + " Ticket# " + dr["id"].ToString() + Environment.NewLine + dr["descres"].ToString();
                            }
                            #endregion
                            #region Overtime
                            if (Convert.ToDouble(dr["ot"]) != 0)
                            {
                                XmlElement TimeTrackingAddRq = inputXMLDoc.CreateElement("TimeTrackingAddRq");
                                qbXMLMsgsRq.AppendChild(TimeTrackingAddRq);
                                TimeTrackingAddRq.SetAttribute("requestID", dr["id"].ToString() + "_ot");
                                XmlElement TimeTrackingAdd = inputXMLDoc.CreateElement("TimeTrackingAdd");
                                TimeTrackingAddRq.AppendChild(TimeTrackingAdd);
                                TimeTrackingAdd.AppendChild(inputXMLDoc.CreateElement("TxnDate")).InnerText = Convert.ToDateTime(dr["edate"]).ToString("yyyy-MM-dd");

                                XmlElement EntityRef = inputXMLDoc.CreateElement("EntityRef");
                                TimeTrackingAdd.AppendChild(EntityRef);
                                EntityRef.AppendChild(inputXMLDoc.CreateElement("ListID")).InnerText = dr["QBEmployeeID"].ToString();

                                XmlElement CustomerRef = inputXMLDoc.CreateElement("CustomerRef");
                                TimeTrackingAdd.AppendChild(CustomerRef);
                                CustomerRef.AppendChild(inputXMLDoc.CreateElement("ListID")).InnerText = dr["QBcustID"].ToString();

                                XmlElement ItemServiceRef = inputXMLDoc.CreateElement("ItemServiceRef");
                                TimeTrackingAdd.AppendChild(ItemServiceRef);
                                ItemServiceRef.AppendChild(inputXMLDoc.CreateElement("ListID")).InnerText = dr["QBitemID"].ToString();

                                var timespan = TimeSpan.FromHours(Convert.ToDouble(dr["ot"]));
                                var hours = timespan.Hours;
                                var minutes = timespan.Minutes;

                                string totaltime = "PT" + hours.ToString() + "H" + minutes.ToString() + "M0S";

                                TimeTrackingAdd.AppendChild(inputXMLDoc.CreateElement("Duration")).InnerText = totaltime;

                                XmlElement PayrollItemWageRef = inputXMLDoc.CreateElement("PayrollItemWageRef");
                                TimeTrackingAdd.AppendChild(PayrollItemWageRef);
                                PayrollItemWageRef.AppendChild(inputXMLDoc.CreateElement("ListID")).InnerText = dr["QBwageID"].ToString();

                                TimeTrackingAdd.AppendChild(inputXMLDoc.CreateElement("Notes")).InnerText = "Over Time" + Environment.NewLine + " Ticket# " + dr["id"].ToString() + Environment.NewLine + dr["descres"].ToString();
                            }
                            #endregion
                            #region double time
                            if (Convert.ToDouble(dr["dt"]) != 0)
                            {
                                XmlElement TimeTrackingAddRq = inputXMLDoc.CreateElement("TimeTrackingAddRq");
                                qbXMLMsgsRq.AppendChild(TimeTrackingAddRq);
                                TimeTrackingAddRq.SetAttribute("requestID", dr["id"].ToString() + "_dt");
                                XmlElement TimeTrackingAdd = inputXMLDoc.CreateElement("TimeTrackingAdd");
                                TimeTrackingAddRq.AppendChild(TimeTrackingAdd);
                                TimeTrackingAdd.AppendChild(inputXMLDoc.CreateElement("TxnDate")).InnerText = Convert.ToDateTime(dr["edate"]).ToString("yyyy-MM-dd");

                                XmlElement EntityRef = inputXMLDoc.CreateElement("EntityRef");
                                TimeTrackingAdd.AppendChild(EntityRef);
                                EntityRef.AppendChild(inputXMLDoc.CreateElement("ListID")).InnerText = dr["QBEmployeeID"].ToString();

                                XmlElement CustomerRef = inputXMLDoc.CreateElement("CustomerRef");
                                TimeTrackingAdd.AppendChild(CustomerRef);
                                CustomerRef.AppendChild(inputXMLDoc.CreateElement("ListID")).InnerText = dr["QBcustID"].ToString();

                                XmlElement ItemServiceRef = inputXMLDoc.CreateElement("ItemServiceRef");
                                TimeTrackingAdd.AppendChild(ItemServiceRef);
                                ItemServiceRef.AppendChild(inputXMLDoc.CreateElement("ListID")).InnerText = dr["QBitemID"].ToString();

                                var timespan = TimeSpan.FromHours(Convert.ToDouble(dr["dt"]));
                                var hours = timespan.Hours;
                                var minutes = timespan.Minutes;

                                string totaltime = "PT" + hours.ToString() + "H" + minutes.ToString() + "M0S";

                                TimeTrackingAdd.AppendChild(inputXMLDoc.CreateElement("Duration")).InnerText = totaltime;

                                XmlElement PayrollItemWageRef = inputXMLDoc.CreateElement("PayrollItemWageRef");
                                TimeTrackingAdd.AppendChild(PayrollItemWageRef);
                                PayrollItemWageRef.AppendChild(inputXMLDoc.CreateElement("ListID")).InnerText = dr["QBwageID"].ToString();

                                TimeTrackingAdd.AppendChild(inputXMLDoc.CreateElement("Notes")).InnerText = "Double Time" + Environment.NewLine + " Ticket# " + dr["id"].ToString() + Environment.NewLine + dr["descres"].ToString();
                            }
                            #endregion
                            #region traveltime
                            if (Convert.ToDouble(dr["tt"]) != 0)
                            {
                                XmlElement TimeTrackingAddRq = inputXMLDoc.CreateElement("TimeTrackingAddRq");
                                qbXMLMsgsRq.AppendChild(TimeTrackingAddRq);
                                TimeTrackingAddRq.SetAttribute("requestID", dr["id"].ToString() + "_tt");
                                XmlElement TimeTrackingAdd = inputXMLDoc.CreateElement("TimeTrackingAdd");
                                TimeTrackingAddRq.AppendChild(TimeTrackingAdd);
                                TimeTrackingAdd.AppendChild(inputXMLDoc.CreateElement("TxnDate")).InnerText = Convert.ToDateTime(dr["edate"]).ToString("yyyy-MM-dd");

                                XmlElement EntityRef = inputXMLDoc.CreateElement("EntityRef");
                                TimeTrackingAdd.AppendChild(EntityRef);
                                EntityRef.AppendChild(inputXMLDoc.CreateElement("ListID")).InnerText = dr["QBEmployeeID"].ToString();

                                XmlElement CustomerRef = inputXMLDoc.CreateElement("CustomerRef");
                                TimeTrackingAdd.AppendChild(CustomerRef);
                                CustomerRef.AppendChild(inputXMLDoc.CreateElement("ListID")).InnerText = dr["QBcustID"].ToString();

                                XmlElement ItemServiceRef = inputXMLDoc.CreateElement("ItemServiceRef");
                                TimeTrackingAdd.AppendChild(ItemServiceRef);
                                ItemServiceRef.AppendChild(inputXMLDoc.CreateElement("ListID")).InnerText = dr["QBitemID"].ToString();

                                var timespan = TimeSpan.FromHours(Convert.ToDouble(dr["tt"]));
                                var hours = timespan.Hours;
                                var minutes = timespan.Minutes;

                                string totaltime = "PT" + hours.ToString() + "H" + minutes.ToString() + "M0S";

                                TimeTrackingAdd.AppendChild(inputXMLDoc.CreateElement("Duration")).InnerText = totaltime;

                                XmlElement PayrollItemWageRef = inputXMLDoc.CreateElement("PayrollItemWageRef");
                                TimeTrackingAdd.AppendChild(PayrollItemWageRef);
                                PayrollItemWageRef.AppendChild(inputXMLDoc.CreateElement("ListID")).InnerText = dr["QBwageID"].ToString();

                                TimeTrackingAdd.AppendChild(inputXMLDoc.CreateElement("Notes")).InnerText = "Travel Time" + Environment.NewLine + " Ticket# " + dr["id"].ToString() + Environment.NewLine + dr["descres"].ToString();
                            }
                            #endregion
                            #region 1.7 time
                            if (Convert.ToDouble(dr["nt"]) != 0)
                            {
                                XmlElement TimeTrackingAddRq = inputXMLDoc.CreateElement("TimeTrackingAddRq");
                                qbXMLMsgsRq.AppendChild(TimeTrackingAddRq);
                                TimeTrackingAddRq.SetAttribute("requestID", dr["id"].ToString() + "_nt");
                                XmlElement TimeTrackingAdd = inputXMLDoc.CreateElement("TimeTrackingAdd");
                                TimeTrackingAddRq.AppendChild(TimeTrackingAdd);
                                TimeTrackingAdd.AppendChild(inputXMLDoc.CreateElement("TxnDate")).InnerText = Convert.ToDateTime(dr["edate"]).ToString("yyyy-MM-dd");

                                XmlElement EntityRef = inputXMLDoc.CreateElement("EntityRef");
                                TimeTrackingAdd.AppendChild(EntityRef);
                                EntityRef.AppendChild(inputXMLDoc.CreateElement("ListID")).InnerText = dr["QBEmployeeID"].ToString();

                                XmlElement CustomerRef = inputXMLDoc.CreateElement("CustomerRef");
                                TimeTrackingAdd.AppendChild(CustomerRef);
                                CustomerRef.AppendChild(inputXMLDoc.CreateElement("ListID")).InnerText = dr["QBcustID"].ToString();

                                XmlElement ItemServiceRef = inputXMLDoc.CreateElement("ItemServiceRef");
                                TimeTrackingAdd.AppendChild(ItemServiceRef);
                                ItemServiceRef.AppendChild(inputXMLDoc.CreateElement("ListID")).InnerText = dr["QBitemID"].ToString();

                                var timespan = TimeSpan.FromHours(Convert.ToDouble(dr["nt"]));
                                var hours = timespan.Hours;
                                var minutes = timespan.Minutes;

                                string totaltime = "PT" + hours.ToString() + "H" + minutes.ToString() + "M0S";

                                TimeTrackingAdd.AppendChild(inputXMLDoc.CreateElement("Duration")).InnerText = totaltime;

                                XmlElement PayrollItemWageRef = inputXMLDoc.CreateElement("PayrollItemWageRef");
                                TimeTrackingAdd.AppendChild(PayrollItemWageRef);
                                PayrollItemWageRef.AppendChild(inputXMLDoc.CreateElement("ListID")).InnerText = dr["QBwageID"].ToString();

                                TimeTrackingAdd.AppendChild(inputXMLDoc.CreateElement("Notes")).InnerText = "1.7 Time" + Environment.NewLine + " Ticket# " + dr["id"].ToString() + Environment.NewLine + dr["descres"].ToString();

                            }
                            #endregion
                        }
                    }
                    //}
                    strRequestXML = inputXMLDoc.OuterXml;
                    req.Add(strRequestXML);

                    #endregion

                    #region Export/Update Timesheet to QB

                    ////DataSet dsUpdateTimesheetToQB = new DataSet();
                    ////objMapData.ConnConfig = objGenFunctions.ConnectionStr(Connection);
                    ////objMapData.SearchValue = "1";
                    ////dsUpdateTimesheetToQB = objBL_MapData.GetTicketTime(objMapData);

                    ////if (dsUpdateTimesheetToQB.Tables[0].Rows.Count > 0)
                    ////{
                    ////    #region Get EditSequence TimeQuery
                    ////    inputXMLDoc = new XmlDocument();
                    ////    inputXMLDoc.AppendChild(inputXMLDoc.CreateXmlDeclaration("1.0", "ISO-8859-1", null));
                    ////    inputXMLDoc.AppendChild(inputXMLDoc.CreateProcessingInstruction("qbxml", "version=\"" + qbxmlversion + "\""));
                    ////    qbXML = inputXMLDoc.CreateElement("QBXML");
                    ////    inputXMLDoc.AppendChild(qbXML);
                    ////    qbXMLMsgsRq = inputXMLDoc.CreateElement("QBXMLMsgsRq");
                    ////    qbXML.AppendChild(qbXMLMsgsRq);
                    ////    qbXMLMsgsRq.SetAttribute("onError", "continueOnError");
                    ////    XmlElement TimeTrackingQueryRq = inputXMLDoc.CreateElement("TimeTrackingQueryRq");
                    ////    qbXMLMsgsRq.AppendChild(TimeTrackingQueryRq);
                    ////    TimeTrackingQueryRq.SetAttribute("requestID", "2");
                    ////    foreach (DataRow dr in dsUpdateTimesheetToQB.Tables[0].Rows)
                    ////    {
                    ////        XmlElement XListID = inputXMLDoc.CreateElement("TxnID");
                    ////        TimeTrackingQueryRq.AppendChild(XListID).InnerText = dr["QBTimeTxnID"].ToString();
                    ////    }
                    ////    strRequestXML = inputXMLDoc.OuterXml;
                    ////    req.Add(strRequestXML);
                    ////    #endregion

                    ////    #region Mod Timesheet Query
                    ////    inputXMLDoc = new XmlDocument();
                    ////    inputXMLDoc.AppendChild(inputXMLDoc.CreateXmlDeclaration("1.0", "ISO-8859-1", null));
                    ////    inputXMLDoc.AppendChild(inputXMLDoc.CreateProcessingInstruction("qbxml", "version=\"" + qbxmlversion + "\""));
                    ////    qbXML = inputXMLDoc.CreateElement("QBXML");
                    ////    inputXMLDoc.AppendChild(qbXML);
                    ////    qbXMLMsgsRq = inputXMLDoc.CreateElement("QBXMLMsgsRq");
                    ////    qbXML.AppendChild(qbXMLMsgsRq);
                    ////    qbXMLMsgsRq.SetAttribute("onError", "continueOnError");

                    ////    DataTable dtModTimeSheetResponse = new DataTable();
                    ////    if (Session["dtModTimeSheetResponse"] != null)
                    ////    {
                    ////        dtModTimeSheetResponse = (DataTable)Session["dtModTimeSheetResponse"];

                    ////        foreach (DataRow dr in dsUpdateDepartmentToQB.Tables[0].Rows)
                    ////        {
                    ////            DateTime lastUpdateDateQB = System.DateTime.MinValue;
                    ////            string strEditseq = string.Empty;
                    ////            foreach (DataRow drModTimeSheetResponse in dtModTimeSheetResponse.Rows)
                    ////            {
                    ////                if (drModTimeSheetResponse["ListID"].ToString() == dr["QBTimeTxnID"].ToString())
                    ////                {
                    ////                    lastUpdateDateQB = Convert.ToDateTime(drModTimeSheetResponse["TimeModified"].ToString());
                    ////                    strEditseq = drModTimeSheetResponse["EditSequence"].ToString();

                    ////                    if (lastUpdateDateQB < Convert.ToDateTime(dr["LastUpdateDate"].ToString()))
                    ////                    {
                    ////                        XmlElement TimeTrackingModRq = inputXMLDoc.CreateElement("TimeTrackingModRq");
                    ////                        qbXMLMsgsRq.AppendChild(TimeTrackingModRq);
                    ////                        TimeTrackingModRq.SetAttribute("requestID", dr["id"].ToString());
                    ////                        XmlElement TimeTrackingMod = inputXMLDoc.CreateElement("TimeTrackingMod");
                    ////                        TimeTrackingModRq.AppendChild(TimeTrackingMod);

                    ////                        TimeTrackingMod.AppendChild(inputXMLDoc.CreateElement("TxnID")).InnerText = dr["QBTimeTxnID"].ToString();
                    ////                        TimeTrackingMod.AppendChild(inputXMLDoc.CreateElement("EditSequence")).InnerText = strEditseq;

                    ////                        XmlElement EntityRef = inputXMLDoc.CreateElement("EntityRef");
                    ////                        TimeTrackingMod.AppendChild(EntityRef);
                    ////                        EntityRef.AppendChild(inputXMLDoc.CreateElement("ListID")).InnerText = dr["QBEmployeeID"].ToString();

                    ////                        XmlElement CustomerRef = inputXMLDoc.CreateElement("CustomerRef");
                    ////                        TimeTrackingMod.AppendChild(CustomerRef);
                    ////                        CustomerRef.AppendChild(inputXMLDoc.CreateElement("ListID")).InnerText = dr["QBcustID"].ToString();

                    ////                        XmlElement ItemServiceRef = inputXMLDoc.CreateElement("ItemServiceRef");
                    ////                        TimeTrackingMod.AppendChild(ItemServiceRef);
                    ////                        ItemServiceRef.AppendChild(inputXMLDoc.CreateElement("ListID")).InnerText = dr["QBitemID"].ToString();

                    ////                        var timespan = TimeSpan.FromHours(Convert.ToDouble(dr["total"]));
                    ////                        var hours = timespan.Hours;
                    ////                        var minutes = timespan.Minutes;
                    ////                        string totaltime = "PT" + hours.ToString() + "H" + minutes.ToString() + "M0S";

                    ////                        TimeTrackingMod.AppendChild(inputXMLDoc.CreateElement("Duration")).InnerText = totaltime;

                    ////                        XmlElement PayrollItemWageRef = inputXMLDoc.CreateElement("PayrollItemWageRef");
                    ////                        TimeTrackingMod.AppendChild(PayrollItemWageRef);
                    ////                        PayrollItemWageRef.AppendChild(inputXMLDoc.CreateElement("ListID")).InnerText = dr["QBwageID"].ToString();

                    ////                        TimeTrackingMod.AppendChild(inputXMLDoc.CreateElement("Notes")).InnerText = "Ticket# " + dr["id"].ToString() + Environment.NewLine + dr["descres"].ToString();
                    ////                    }
                    ////                }
                    ////            }
                    ////        }
                    ////    }
                    ////    strRequestXML = inputXMLDoc.OuterXml;
                    ////    req.Add(strRequestXML);
                    ////    #endregion
                    ////}
                    #endregion

                    #endregion


                    // 35 deleted from QB
                    #region deleted from QB


                    inputXMLDoc = new XmlDocument();
                    inputXMLDoc.AppendChild(inputXMLDoc.CreateXmlDeclaration("1.0", "ISO-8859-1", null));
                    inputXMLDoc.AppendChild(inputXMLDoc.CreateProcessingInstruction("qbxml", "version=\"" + qbxmlversion + "\""));

                    qbXML = inputXMLDoc.CreateElement("QBXML");
                    inputXMLDoc.AppendChild(qbXML);
                    qbXMLMsgsRq = inputXMLDoc.CreateElement("QBXMLMsgsRq");
                    qbXML.AppendChild(qbXMLMsgsRq);
                    qbXMLMsgsRq.SetAttribute("onError", "continueOnError");

                    XmlElement TxnDeletedQueryRq = inputXMLDoc.CreateElement("TxnDeletedQueryRq");
                    qbXMLMsgsRq.AppendChild(TxnDeletedQueryRq);
                    TxnDeletedQueryRq.SetAttribute("requestID", "2");
                    XmlElement TxnDelType = inputXMLDoc.CreateElement("TxnDelType");
                    TxnDeletedQueryRq.AppendChild(TxnDelType).InnerText = "Invoice";
                    //if (!string.IsNullOrEmpty(FormatDate))
                    //{
                    //    XmlElement DeletedDateRangeFilter = inputXMLDoc.CreateElement("DeletedDateRangeFilter");
                    //    TxnDeletedQueryRq.AppendChild(DeletedDateRangeFilter);
                    //    XmlElement FromDeletedDate = inputXMLDoc.CreateElement("FromDeletedDate");
                    //    DeletedDateRangeFilter.AppendChild(FromDeletedDate).InnerText = FormatDate;
                    //}

                    XmlElement ListDeletedQueryRq = inputXMLDoc.CreateElement("ListDeletedQueryRq");
                    qbXMLMsgsRq.AppendChild(ListDeletedQueryRq);
                    ListDeletedQueryRq.SetAttribute("requestID", "2");
                    //string[] lstDeletedItems = { "Employee", "CustomerType", "JobType", "ItemDiscount", "ItemFixedAsset", "ItemGroup", "ItemInventory", "ItemInventoryAssembly", "ItemNonInventory", "ItemOtherCharge", "ItemPayment", "ItemSalesTax", "ItemSalesTaxGroup", "ItemService", "ItemSubtotal", "Customer", "Class", "StandardTerms", "PayrollItemWage" };
                    string[] lstDeletedItems = { "Employee", "Customer", "CustomerType", "JobType", "ItemDiscount", "ItemFixedAsset", "ItemGroup", "ItemInventory", "ItemInventoryAssembly", "ItemNonInventory", "ItemOtherCharge", "ItemPayment", "ItemSalesTax", "ItemSalesTaxGroup", "ItemService", "ItemSubtotal", "Class" };
                    foreach (string stritem in lstDeletedItems)
                    {
                        XmlElement ListDelType = inputXMLDoc.CreateElement("ListDelType");
                        ListDeletedQueryRq.AppendChild(ListDelType).InnerText = stritem;
                    }
                    //if (!string.IsNullOrEmpty(FormatDate))
                    //{
                    //    XmlElement DeletedDateRangeFilter = inputXMLDoc.CreateElement("DeletedDateRangeFilter");
                    //    ListDeletedQueryRq.AppendChild(DeletedDateRangeFilter);
                    //    XmlElement FromDeletedDate = inputXMLDoc.CreateElement("FromDeletedDate");
                    //    DeletedDateRangeFilter.AppendChild(FromDeletedDate).InnerText = FormatDate;
                    //}

                    strRequestXML = inputXMLDoc.OuterXml;
                    req.Add(strRequestXML);

                    // Clean up
                    strRequestXML = "";
                    inputXMLDoc = null;
                    qbXML = null;
                    qbXMLMsgsRq = null;
                    //maxReturned = null;

                    #endregion
                }

            }
            return req;
        }

        private DataSet BuildLineItems(double totalTime, double Expenses, int mileage, string QBServiceInvID, string Desc, string MileageItem, string LaborItem, string ExpenseItem)
        {
            string line = string.Empty;
            if (Expenses != 0)
            {
                if (ExpenseItem == string.Empty)
                {
                    line = "'expenses'";
                }
            }
            if (mileage != 0)
            {
                if (MileageItem == string.Empty)
                {
                    if (line != string.Empty)
                    {
                        line += ",";
                    }
                    line += "'mileage'";
                }
            }

            if (LaborItem == string.Empty)
            {
                if (line != string.Empty)
                {
                    line += ",";
                }
                line += "'Time Spent'";
            }

            objProp_Contracts.ConnConfig = objGenFunctions.ConnectionStr(Connection);
            objProp_Contracts.TicketLineItems = line;
            objProp_Contracts.MileageItem = MileageItem;
            objProp_Contracts.LaborItem = LaborItem;
            objProp_Contracts.ExpenseItem = ExpenseItem;
            objProp_Contracts.QBInvID = QBServiceInvID;
            DataSet dsLineItem = objBL_Contracts.GetBillcodesforQBChargeableticket(objProp_Contracts);

            foreach (DataRow dr in dsLineItem.Tables[0].Rows)
            {
                if (Expenses != 0)
                {
                    if (ExpenseItem == string.Empty)
                    {
                        if (string.Equals(dr["billcode"].ToString(), "expenses", StringComparison.InvariantCultureIgnoreCase))
                        {
                            dr["Quan"] = 1;
                            //dr["price"] = Expenses;
                            dr["amount"] = Expenses;
                        }
                    }
                    else
                    {
                        if (string.Equals(dr["qbinvid"].ToString(), ExpenseItem, StringComparison.InvariantCultureIgnoreCase))
                        {
                            dr["Quan"] = 1;
                            //dr["price"] = Expenses;
                            dr["amount"] = Expenses;
                        }
                    }
                }
                if (mileage != 0)
                {
                    if (MileageItem == string.Empty)
                    {
                        if (string.Equals(dr["billcode"].ToString(), "mileage", StringComparison.InvariantCultureIgnoreCase))
                        {
                            double price = Convert.ToDouble(dr["price"]);
                            dr["Quan"] = mileage;
                            dr["amount"] = price * mileage;
                        }
                    }
                    else
                    {
                        if (string.Equals(dr["qbinvid"].ToString(), MileageItem, StringComparison.InvariantCultureIgnoreCase))
                        {
                            double price = Convert.ToDouble(dr["price"]);
                            dr["Quan"] = mileage;
                            dr["amount"] = price * mileage;
                        }
                    }
                }

                if (LaborItem == string.Empty)
                {
                    if (string.Equals(dr["billcode"].ToString(), "Time Spent", StringComparison.InvariantCultureIgnoreCase))
                    {
                        double price = Convert.ToDouble(dr["price"]);
                        dr["Quan"] = totalTime;
                        dr["amount"] = price * totalTime;
                        dr["fdesc"] = Desc;
                    }
                }
                else
                {
                    if (string.Equals(dr["qbinvid"].ToString(), LaborItem, StringComparison.InvariantCultureIgnoreCase))
                    {
                        double price = Convert.ToDouble(dr["price"]);
                        dr["Quan"] = totalTime;
                        dr["amount"] = price * totalTime;
                        dr["fdesc"] = Desc;
                    }
                }
            }

            return dsLineItem;


            //if (dsLineItem.Tables[0].Rows.Count > 0)
            //{
            //    if (Expenses != 0)
            //    {
            //        dsLineItem.Tables[0].Rows[0]["Quan"] = 1;
            //        dsLineItem.Tables[0].Rows[0]["price"] = Expenses;
            //        dsLineItem.Tables[0].Rows[0]["amount"] = Expenses;
            //    }
            //    if (mileage != 0)
            //    {
            //        double price = Convert.ToDouble(dsLineItem.Tables[0].Rows[countMil]["price"]);
            //        dsLineItem.Tables[0].Rows[countMil]["Quan"] = mileage;
            //        dsLineItem.Tables[0].Rows[countMil]["amount"] = price * mileage;
            //    }
            //    if (totalTime != 0)
            //    {
            //        double price = Convert.ToDouble(dsLineItem.Tables[0].Rows[countTT]["price"]);
            //        dsLineItem.Tables[0].Rows[countTT]["Quan"] = totalTime;
            //        dsLineItem.Tables[0].Rows[countTT]["amount"] = price * totalTime;
            //    }
            //}
        }

        private void UserRegistration(string Username, int UserID)
        {
            string strLic = "0";
            string strDay = "30";
            string strDate = System.DateTime.Now.ToShortDateString();
            string strUsername = Username;
            string strReg = string.Empty;
            string strRegEncr = string.Empty;

            objProp_User.DBName = Connection;
            DataSet dsinfo = new DataSet();
            dsinfo = objBL_User.getLicenseInfoUser(objProp_User);

            if (dsinfo.Tables[0].Rows.Count > 0)
            {
                string strRegDecr = SSTCryptographer.Decrypt(dsinfo.Tables[0].Rows[0]["str"].ToString(), "regu");
                string[] strRegItems = strRegDecr.Split('&');
                strLic = strRegItems[0];
                strDay = strRegItems[1];
                strDate = strRegItems[2];
                strReg = strLic + "&" + strDay + "&" + strDate + "&" + strUsername;
                strRegEncr = SSTCryptographer.Encrypt(strReg, "regu");

                objProp_User.UserLicID = Convert.ToInt32(dsinfo.Tables[0].Rows[0]["lid"]);
                objProp_User.UserLic = strRegEncr;
                objProp_User.UserID = UserID;
                objProp_User.ConnConfig = objGenFunctions.ConnectionStr(Connection);
                objBL_User.UserRegistrationTransfer(objProp_User);
            }
        }

        private string parseForVersion(string input)
        {
            // This method is created just to parse the first two version components
            // out of the standard four component version number:
            // <Major>.<Minor>.<Release>.<Build>
            // 
            // As long as you get the version in right format, you could use
            // any algorithm here. 
            string retVal = "";
            string major = "";
            string minor = "";
            Regex version = new Regex(@"^(?<major>\d+)\.(?<minor>\d+)(\.\w+){0,2}$", RegexOptions.Compiled);
            Match versionMatch = version.Match(input);
            if (versionMatch.Success)
            {
                major = versionMatch.Result("${major}");
                minor = versionMatch.Result("${minor}");
                retVal = major + "." + minor;
            }
            else
            {
                retVal = input;
            }
            return retVal;
        }

        private int Status(string strStatus)
        {
            bool status = Convert.ToBoolean(strStatus);

            if (status)
                return 0;
            else
                return 1;
        }

        private void SaveErrorCodes(string API, string requestID, string StatusCode, string statusSeverity, string statusMessage)
        {
            string message = "API-" + API + Environment.NewLine + "requestID-" + requestID + Environment.NewLine + "StatusCode-" + StatusCode + Environment.NewLine + "statusSeverity-" + statusSeverity + Environment.NewLine + "statusMessage-" + statusMessage;

            try
            {
                objGeneral.QBapi = API;
                objGeneral.QBRequestID = requestID;
                objGeneral.QBStatusCode = StatusCode;
                objGeneral.QBStatusMessage = statusMessage;
                objGeneral.QBStatusSeverity = statusSeverity;
                objGeneral.ConnConfig = objGenFunctions.ConnectionStr(Connection);
                objBL_General.AddQBErrorLog(objGeneral);

                mail(message);
            }
            catch
            {

            }

            int syncStoponError = Convert.ToInt16(System.Web.Configuration.WebConfigurationManager.AppSettings["SyncStopOnError"].Trim());
            if (syncStoponError == 1)
                throw new Exception(message);
        }

        private void mail(string message)
        {
            string to = System.Web.Configuration.WebConfigurationManager.AppSettings["ErrorEmail"].Trim();
            if (!string.IsNullOrEmpty(to))
            {
                Thread email = new Thread(delegate ()
                {
                    //string to = "@gmail.com";
                    string from = "qbsync@mom.webserv";

                    if (to.Trim() != string.Empty && from.Trim() != string.Empty)
                    {
                        try
                        {
                            Mail mail = new Mail();
                            mail.From = from;
                            mail.To = to.Split(';', ',').OfType<string>().ToList();
                            mail.Title = "Webconnector Error";
                            mail.Text = message;
                            mail.RequireAutentication = false;
                            mail.Send();
                        }
                        catch { }
                    }
                });
                email.IsBackground = true;
                email.Start();
            }
        }

        private void NullifySessions()
        {
            Session["dsNewMSMCustType"] = null;
            Session["dsNewMSMLocType"] = null;
            Session["dsNewSalesTaxCode"] = null;
            Session["dsNewMSMCust"] = null;
            Session["dsUpdateCustToQB"] = null;
            Session["dsNewMSMLoc"] = null;
            Session["dsUpdateLocToQB"] = null;

            Session["dtModCustResponse"] = null;
            Session["dtModLocResponse"] = null;
            Session["dtModClassResponse"] = null;
            Session["dtModTimeSheetResponse"] = null;
            Session["dtModItemServiceResponse"] = null;
            Session["duplicateTermsID"] = null;
            Session["dtModItemSalesTAxResponse"] = null;

            Session["ClassRet_1"] = null;
        }

        #endregion

        private static string GetAppSettingsValueByKey(string appKey)
        {
            string retVal = string.Empty;
            try
            {
                retVal = System.Web.Configuration.WebConfigurationManager.AppSettings[appKey].Trim();
            }
            catch
            {}
            return retVal;
        }

    } // class: QuickBookSync
      // namespace: Sync


}
