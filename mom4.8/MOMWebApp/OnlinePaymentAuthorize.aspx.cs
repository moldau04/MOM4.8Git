using AuthorizeNet.Api.Contracts.V1;
using AuthorizeNet.Api.Controllers;
using AuthorizeNet.Api.Controllers.Bases;

using MOMWebApp;
using BusinessEntity;
using BusinessLayer;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Windows.Forms;
using Newtonsoft.Json;


namespace MOMWebApp
{
    public partial class OnlinePaymentAuthorize : System.Web.UI.Page
    {
        BL_Contracts objBL_Contracts = new BL_Contracts();
        Contracts objProp_Contracts = new Contracts();

        BL_OnlinePayment objBL_OnlinePayment = new BL_OnlinePayment();
        OnlinePayment objOnlinePayment = new OnlinePayment();

        BL_Deposit objBL_Deposit = new BL_Deposit();
        //ReceivedPayment objReceivePay = new ReceivedPayment();

        //dynamic gatewayResponse = JsonConvert.DeserializeObject(HttpContext.Current.Request.Form["gatewayResponse"]);


        protected void Page_Load(object sender, EventArgs e)
        {
            string ApiLoginID = "67QzSpk3tj";
            string ApiTransactionKey = "63F5s84Q992c9Nyt";
            string connectionString = @"server=DELL\MSSQLSERVER01;Initial Catalog=QAE;user=sa;password=Snowdrops123#;";

            objOnlinePayment.ConnConfig = connectionString;
            objProp_Contracts.ConnConfig = connectionString;
            //objReceivePay.ConnConfig = connectionString;

            DataSet ds = new DataSet();

            //if (Request.QueryString["InvoiceId"] == null)
            //{
            //    Response.Write("Invalid parameter");
            //    Response.End();
            //}
            //else {
            //    //objProp_Contracts.InvoiceID = Convert.ToInt32(Request.QueryString["InvoiceId"]);
            //}

            objProp_Contracts.InvoiceID = 257132; //!! hardcoded !!

            ds = objBL_Contracts.GetInvoiceByInvoiceID(objProp_Contracts);

            if (!IsPostBack)
            {
                //objOnlinePayment.ConnConfig = connectionString;
                // objProp_Contracts.ConnConfig = connectionString;
                //objProp_Contracts.InvoiceID = Convert.ToInt32(Request.QueryString["uid"]);
                //objProp_Contracts.InvoiceID = Convert.ToInt32(Request.QueryString["InvoiceId"]);
                //ds = objBL_Contracts.GetInvoiceByInvoiceID(objProp_Contracts);

                //** Display online payment form with invoice values **
                if (ds.Tables[0].Rows.Count > 0)
                {
                    txtInvoiceDate.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["fdate"]).ToShortDateString();
                    txtAddress.Text = ds.Tables[0].Rows[0]["billto"].ToString();
                    //txtjobamt.Text = string.Format("{0:c}", Convert.ToDouble(ds.Tables[0].Rows[0]["Amount"]));
                    txtamnt.Text = string.Format("{0:c}", Convert.ToDouble(ds.Tables[0].Rows[0]["Total"]));
                    //lblGstTax.Text = string.Format("{0:c}", Convert.ToDouble(ds.Tables[0].Rows[0]["GTax"]));
                    //lblPstTax.Text = string.Format("{0:c}", Convert.ToDouble(ds.Tables[0].Rows[0]["STax"]));
                    lblInv.Text = "Invoice# " + Convert.ToString(ds.Tables[0].Rows[0]["ref"]);
                    txtCustomer.Text = ds.Tables[0].Rows[0]["customername"].ToString();
                   // txtCity.Text = ds.Tables[0].Rows[0]["City"].ToString();
                }
            }
            else // IsPostBack
            {
                //** Make Payment clicked **

                objOnlinePayment.GatewayId = 1; //!! hardcoded !!
                int CustomerId = Convert.ToInt32(ds.Tables[0].Rows[0]["Owner"].ToString());
                objOnlinePayment.CustomerId = CustomerId;
                //objReceivePay.Rol = CustomerId; 

                decimal amount = Convert.ToDecimal(ds.Tables[0].Rows[0]["Amount"]);
                string LocationId = ds.Tables[0].Rows[0]["Loc"].ToString();
                string InvoiceId = Convert.ToString(ds.Tables[0].Rows[0]["ref"]);
                objOnlinePayment.Amount = amount;
                objOnlinePayment.InvoiceId = Convert.ToInt32(InvoiceId);
                objOnlinePayment.LocId = Convert.ToInt32(LocationId);
                //objReceivePay.Loc = Convert.ToInt32(LocationId);

                //------------------------------------------------
                /*
                CreateDatatable();
                DataTable dt = (DataTable)ViewState["ReceivPay"];
                DataTable dtReceive = dt.Clone();
                DataRow dr = dtReceive.NewRow();
                dr["InvoiceID"] = 257132; //!! hardcoded !!
                dr["Loc"] = LocationId;
                dr["Type"] = 0; //ii Invoice ii
                dr["PayAmount"] = amount;                         
                dr["Status"] = 1;
                dr["IsCredit"] = 1;
                dr["RefTranID"] = Convert.ToString(ds.Tables[0].Rows[0]["TransID"]);
                dtReceive.Rows.Add(dr);

                objReceivePay.DtPay = dtReceive; 
                */

/*********************************************************************************************

                //make dynamic for structuring
                dynamic gatewayResponse = JsonConvert.DeserializeObject(HttpContext.Current.Request.Form["gatewayResponse"]);

                if(gatewayResponse != null) 
                { 
                    if(gatewayResponse.encryptedBankData != null)
                    {
                        objOnlinePayment.BankNameOnAccount = gatewayResponse.encryptedBankData.nameOnAccount;
                        objOnlinePayment.RoutingNumber = gatewayResponse.encryptedBankData.routingNumber;
                        objOnlinePayment.AccountNumber = gatewayResponse.encryptedBankData.accountNumber;
                        objOnlinePayment.AccountType = gatewayResponse.encryptedBankData.accountType;
                    }
                    if (gatewayResponse.encryptedCardData != null)
                    {
                        objOnlinePayment.CardNumber = gatewayResponse.encryptedCardData.cardNumber;
                        objOnlinePayment.ExpiryDate = gatewayResponse.encryptedCardData.expDate;
                        objOnlinePayment.Bin = gatewayResponse.encryptedCardData.bin;                                             
                    }
                    if (gatewayResponse.customerInformation != null)
                    {
                        objOnlinePayment.FirstName = gatewayResponse.customerInformation.firstName;
                        objOnlinePayment.LastDate = gatewayResponse.customerInformation.lastName;
                    }                    
                }

*************************************************************************************/

                if (Request.Form["dataDescriptor"] != null)
                {
                    Run(ApiLoginID, ApiTransactionKey, amount);
                    Response.Write("All Done");                    
                }                
            }
        }   


        public ANetApiResponse Run(String ApiLoginID, String ApiTransactionKey, decimal amount)
        {
            ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = AuthorizeNet.Environment.SANDBOX;

            // define the merchant information (authentication / transaction id)
            ApiOperationBase<ANetApiRequest, ANetApiResponse>.MerchantAuthentication = new merchantAuthenticationType()
            {
                name = ApiLoginID,
                ItemElementName = ItemChoiceType.transactionKey,
                Item = ApiTransactionKey,
            };

            string dataValue = HttpContext.Current.Request.Form["dataValue"];
            string CustName = HttpContext.Current.Request.Form["txtCustomer"];
            string Address = HttpContext.Current.Request.Form["txtAddress"];
            string InvoiceDate = HttpContext.Current.Request.Form["txtInvoiceDate"];

            var opaqueData = new opaqueDataType
            {
                dataDescriptor = "COMMON.ACCEPT.INAPP.PAYMENT",
                dataValue = dataValue
            };

            objOnlinePayment.Token = opaqueData.dataValue;
            objOnlinePayment.InvoiceDate = Convert.ToDateTime(InvoiceDate);

            var billingAddress = new customerAddressType
            {
                firstName = CustName,  //get it from front
                lastName = "Doe",
                address = "",
                city = "OurTown",
                zip = "98004"
            };

            //standard api call to retrieve response
            var paymentType = new paymentType { Item = opaqueData };

            // Add line ItemsB
            var lineItems = new lineItemType[2];
            lineItems[0] = new lineItemType { itemId = "1", name = "t-shirt1", quantity = 2, unitPrice = new Decimal(10.00) };
            lineItems[1] = new lineItemType { itemId = "2", name = "snowboard", quantity = 1, unitPrice = new Decimal(20.00) };

            var transactionRequest = new transactionRequestType
            {
                transactionType = transactionTypeEnum.authCaptureTransaction.ToString(),    // charge the card

                amount = amount,
                payment = paymentType,
                billTo = billingAddress,
                lineItems = lineItems
            };

/*******************************************************************************************
            //For Recieve payment

            DateTime receivePay = DateTime.Now;

            //objReceivePay.ID = Convert.ToInt32(Request.QueryString["InvoiceId"]);
            objReceivePay.ID = 257132; //!! hardcoded !!

            //if (!string.IsNullOrEmpty(Convert.ToInt32(LocationId)))
            //{
            //    objReceivePay.Loc = Convert.ToInt32(LocationId);               
            //}
            //else
            //{
            //    objReceivePay.Loc = 0;              
            //}

            dynamic gatewayResponse = JsonConvert.DeserializeObject(HttpContext.Current.Request.Form["gatewayResponse"]);
            if (gatewayResponse.encryptedCardData != null)
            {
                objReceivePay.PaymentMethod = 4;
            }
            else 
            { 
                objReceivePay.PaymentMethod = 3; 
            }

            //Status coulum in datatable

            //remaining 

            objReceivePay.AmountDue = 0;
            objReceivePay.PaymentReceivedDate = receivePay;
            objReceivePay.Rol = 57;
            objReceivePay.CheckNumber = "";
            objReceivePay.fDesc = "online payment";
            objReceivePay.MOMUSer = "";
            
            //objReceivePay.DtPay = receivePay;

            objReceivePay.Amount = (double)amount;

            objBL_Deposit.UpdateReceivePayment(objReceivePay);
            int paymentReceiveId = objBL_Deposit.AddReceivePayment(objReceivePay);

*******************************************************************************************/

//================================================================================================================

                //make dynamic for structuring
                dynamic gatewayResponse = JsonConvert.DeserializeObject(HttpContext.Current.Request.Form["gatewayResponse"]);

                if(gatewayResponse != null) 
                {
                    objOnlinePayment.GatewayResponseDump = Convert.ToString(gatewayResponse);

                    if (gatewayResponse.encryptedBankData != null)
                    {
                        objOnlinePayment.BankNameOnAccount = gatewayResponse.encryptedBankData.nameOnAccount;
                        objOnlinePayment.RoutingNumber = gatewayResponse.encryptedBankData.routingNumber;
                        objOnlinePayment.AccountNumber = gatewayResponse.encryptedBankData.accountNumber;
                        objOnlinePayment.AccountType = gatewayResponse.encryptedBankData.accountType;

                        //objReceivePay.PaymentMethod = 3;
                    }
                    else if (gatewayResponse.encryptedCardData != null)
                    {
                        objOnlinePayment.CardNumber = gatewayResponse.encryptedCardData.cardNumber;
                        objOnlinePayment.ExpiryDate = gatewayResponse.encryptedCardData.expDate;
                        objOnlinePayment.Bin = gatewayResponse.encryptedCardData.bin;                                             

                        //objReceivePay.PaymentMethod = 4;
                    }
                    if (gatewayResponse.customerInformation != null)
                    {
                        objOnlinePayment.FirstName = gatewayResponse.customerInformation.firstName;
                        objOnlinePayment.LastDate = gatewayResponse.customerInformation.lastName;
                    }                    
                }

//================================================================================================================

            var request = new createTransactionRequest { transactionRequest = transactionRequest };

            //instantiate the controller that will call the service
            var controller = new createTransactionController(request);
            controller.Execute();

            // get the response from the service (errors contained if any)
             var response = controller.GetApiResponse();
            // OnlinePayment objOnlinePayment = new OnlinePayment();
            // validate response
            if (response != null)
            {
                if (response.messages.resultCode == messageTypeEnum.Ok)
                {
                    if (response.transactionResponse.messages != null)
                    {
                        Console.WriteLine("Successfully created transaction with Transaction ID: " + response.transactionResponse.transId);
                        Console.WriteLine("Response Code: " + response.transactionResponse.responseCode);
                        Console.WriteLine("Message Code: " + response.transactionResponse.messages[0].code);
                        Console.WriteLine("Description: " + response.transactionResponse.messages[0].description);
                        Console.WriteLine("Success, Auth Code : " + response.transactionResponse.authCode);
                        Console.WriteLine("Success, Transaction Code : " + response.transactionResponse.transId);

                        objOnlinePayment.TransactionStatus = true; // Success

                        if (response.transactionResponse.authCode != null)
                        {
                            objOnlinePayment.AuthCode = response.transactionResponse.authCode;
                        };

                       // sql direct insertion
                        objOnlinePayment.GatewayTransactionId = response.transactionResponse.transId;
                        objOnlinePayment.NetworkTransactionId = response.transactionResponse.networkTransId;
                        objOnlinePayment.transHashSha2 = response.transactionResponse.transHashSha2;
                        objOnlinePayment.AccountType = response.transactionResponse.accountType;
                        //objOnlinePayment.PaymentMode = response.messages.ToString();
                        objOnlinePayment.PaymentMode = response.transactionResponse.accountType;

                        //var res = JsonConvert.DeserializeObject(response.transactionResponse);
                        // objOnlinePayment.GatewayResponseDump = response.;

                        //insertion
                        //*************** objBL_OnlinePayment.OnlinePaymentTransactions_insert(objOnlinePayment);

                        // insert into received payment table ********************************************************

                        //DateTime receivePay = DateTime.Now;

                        ////objReceivePay.ID = Convert.ToInt32(Request.QueryString["InvoiceId"]);
                        //objReceivePay.ID = 257132; //!! hardcoded !!

                        //objReceivePay.AmountDue = 0;
                        //objReceivePay.PaymentReceivedDate = receivePay;
                        ////objReceivePay.Rol = 57;
                        //objReceivePay.CheckNumber = "";
                        //objReceivePay.fDesc = "online payment";
                        //objReceivePay.MOMUSer = "";
                        //objReceivePay.Amount = (double) amount;

                        //int paymentReceiveId = objBL_Deposit.AddReceivePayment(objReceivePay);

                        //********************************************************************************************
                    }
                    else // if (response.transactionResponse.messages = null)
                    {
                        Console.WriteLine("Failed Transaction.");

                        objOnlinePayment.TransactionStatus = false; // Failure

                        if (response.transactionResponse.errors != null)
                        {
                            Console.WriteLine("Error Code: " + response.transactionResponse.errors[0].errorCode);
                            Console.WriteLine("Error message: " + response.transactionResponse.errors[0].errorText);

                            /*********************
                            if (response.transactionResponse.errors[0].errorCode != "")
                            {
                                objOnlinePayment.TransactionStatus = false;
                            }
                            else
                            {
                                objOnlinePayment.TransactionStatus = true;
                            }
                            ************************/

                            objOnlinePayment.ErrorCode = response.transactionResponse.errors[0].errorCode;
                            objOnlinePayment.ErrorText = response.transactionResponse.errors[0].errorText;
                            //*************** objBL_OnlinePayment.OnlinePaymentTransactions_insert(objOnlinePayment);
                        }
                    }
                }
                else // if (response.messages.resultCode == messageTypeEnum.Ok)
                {
                    Console.WriteLine("Failed Transaction.");

                    objOnlinePayment.TransactionStatus = false; // Failure

                    if (response.transactionResponse != null && response.transactionResponse.errors != null)
                    {
                        Console.WriteLine("Error Code: " + response.transactionResponse.errors[0].errorCode);
                        Console.WriteLine("Error message: " + response.transactionResponse.errors[0].errorText);
                        //=======================================================
                        objOnlinePayment.ErrorCode = response.transactionResponse.errors[0].errorCode;
                        objOnlinePayment.ErrorText = response.transactionResponse.errors[0].errorText;
                        //=======================================================
                    }
                    else
                    {
                        Console.WriteLine("Error Code: " + response.messages.message[0].code);
                        Console.WriteLine("Error message: " + response.messages.message[0].text);
                        //=======================================================
                        objOnlinePayment.ErrorCode = response.messages.message[0].code;
                        objOnlinePayment.ErrorText = response.messages.message[0].text;
                        //=======================================================
                    }
                }
            }
            else
            {
                Console.WriteLine("Null Response.");
                //=======================================================
                objOnlinePayment.ErrorCode = "0";
                objOnlinePayment.ErrorText = "Null Response.";
                //=======================================================
            }

//========================================================================================================

            objBL_OnlinePayment.OnlinePaymentInsert(objOnlinePayment);

//========================================================================================================


            return response;
        }

        //-----------------

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
        private DataTable GetInvoiceItems(int _refId)
        {
            DataTable _dtItem = new DataTable();
            try
            {
                objProp_Contracts.InvoiceID = _refId;
                objProp_Contracts.ConnConfig = Session["config"].ToString();
                DataSet _dsItemDetails = objBL_Contracts.GetInvoiceItemByRef(objProp_Contracts);
                if (_dsItemDetails.Tables[0].Rows.Count < 1)
                {
                    _dtItem = LoadInvoiceDetails(_dsItemDetails.Tables[0], _refId);
                }
                else
                    _dtItem = _dsItemDetails.Tables[0];
            }
            catch (Exception ex)
            {
                string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
                ClientScript.RegisterStartupScript(Page.GetType(), "keyErr6543", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            }

            return _dtItem;
        }

        private DataTable LoadInvoiceDetails(DataTable _dt, int _idRef)
        {
            DataRow _dr = _dt.NewRow();
            _dr["Ref"] = _idRef;
            _dr["Acct"] = 0;
            _dr["Quan"] = 0;
            _dr["fDesc"] = string.Empty;
            _dr["Price"] = 0.00;
            _dr["Amount"] = 0.00;
            _dr["STax"] = 0.00;
            _dr["billcode"] = string.Empty;
            _dr["staxAmt"] = 0.00;
            _dr["balance"] = 0.00;
            _dr["amtpaid"] = 0.00;
            _dr["total"] = 0.00;
            _dt.Rows.Add(_dr);
            return _dt;
        }
        //-------------------

    }
}