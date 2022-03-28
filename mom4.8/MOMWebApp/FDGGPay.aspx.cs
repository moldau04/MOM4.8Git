using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLayer;
using BusinessEntity;
using System.Data;
using System.Web.UI.HtmlControls;

using MOMWebApp.FirstData;
//using FDTEST;
using System.Security.Cryptography.X509Certificates;
using System.Net;
using System.IO;
using System.Threading;

public partial class FDGGPay : System.Web.UI.Page
{
    BL_Contracts objBL_Contracts = new BL_Contracts();
    Contracts objProp_Contracts = new Contracts();

    GeneralFunctions objGeneral = new GeneralFunctions();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userid"] == null)
        {
            Response.Redirect("login.aspx");
        }
        if (Convert.ToInt16(Session["payment"]) != 1)
        {
            Response.Redirect("home.aspx");
        }

        string url_path_current = HttpContext.Current.Request.Url.ToString();
        if (url_path_current.StartsWith("http:") == true)
        {
            HttpContext.Current.Response.Redirect("https" + url_path_current.Remove(0, 4), false);
        }

        if (Request.QueryString["o"] == null)
        {
            //HtmlGenericControl div = (HtmlGenericControl)Page.Master.FindControl("MenuWrap");
            HtmlGenericControl div = (HtmlGenericControl)Page.Master.FindControl("headerfix");
            div.Visible = false;
        }

        if (!IsPostBack)
        {
            if (Session["uidv"] == null)
            {
                Response.Redirect("invoices.aspx?fil=1");
            }

            List<Dictionary<string, string>> lstInv = new List<Dictionary<string, string>>();
            if (Session["uidv"] != null)
            {
                lstInv = (List<Dictionary<string, string>>)Session["uidv"];
                Session["uidv"] = null;
            }


            double dblAmount = 0;
            string invcs = string.Empty;
            int count = 0;
            foreach (Dictionary<string, string> d in lstInv)
            {
                dblAmount += Convert.ToDouble(d["amt"]);
                if (count == 0)
                    invcs += d["inv"].ToString();
                else
                    invcs += "," + d["inv"].ToString();

                count++;
            }
            lblInvoiceName.Text = invcs;

            double totalAmt=Math.Round(dblAmount,2);
            ViewState["invlst"] = lstInv;
            ViewState["amt"] = totalAmt;
            ViewState["uid"] = invcs;
            txtAmount.Text = totalAmt.ToString(); 
            
            DataSet dsowner = GetCustomerAddress();
            if (dsowner != null)
            {
                txtNameCustomer.Text = dsowner.Tables[0].Rows[0]["name"].ToString();
            }


            //txtCardNumber.Text = "4012000033330026";
            ////txtAmount.Text = "1";
            //txtCVC.Text = "123";
            //txtNameOnCard.Text = "TEST";
            //ddlMonth.SelectedValue = "12";
            //ddlYear.SelectedValue = "14";
        }
    }

    protected void btnPayment_Click(object sender, EventArgs e)
    {
        lblMSG.ForeColor = System.Drawing.Color.Red;
        lblMSG.Text = string.Empty;
        lblErr.Text = string.Empty;

        if (ViewState["amt"].ToString().Trim() == string.Empty)
        {
            lblErr.Text ="Enter Valid Amount";
            return;
        }

        if (Convert.ToDouble(ViewState["amt"].ToString().Trim()) == 0)
        {
            lblErr.Text = "Enter Valid Amount";
            return;
        }

        if (txtCardNumber.Text.Trim() == string.Empty || txtNameOnCard.Text.Trim() == string.Empty || ddlMonth.SelectedValue == string.Empty || ddlYear.SelectedValue == string.Empty || txtCVC.Text.Trim() == string.Empty)
        {
            lblErr.Text = "Invalid Information";
            return;
        }

        if (txtCardNumber.Text.Trim().Length < 16)
        {
            lblErr.Text = "Invalid Card Number.";
            return;
        }

        string InvoicesChk = CheckPaid();
        if (InvoicesChk.Trim().Replace(",", "") != string.Empty)
        {
            lblErr.Text = "Invoice(s)# " + InvoicesChk + " Already Paid, transaction declined.";
            return;
        }

        string InvoicesAmountExceed = CheckAmountExceed();
        if (InvoicesAmountExceed.Trim() != string.Empty)
        {
            lblErr.Text = "Invoice# " + InvoicesAmountExceed + " exceeds maximum payment amount, transaction declined.";
            return;
        }
        
        string strApproved = string.Empty;
        string strError = string.Empty;
        string strResponseCodes = string.Empty;
        string strTransID = string.Empty;
        string strResponseText = string.Empty;
        string strOrderID = string.Empty;

        string alert = string.Empty;
        string resp = string.Empty;

        try
        {
            string MerchantUsername = string.Empty;
            string MerchantPassword = string.Empty;
            string CertPath = string.Empty;
            string URL = string.Empty;

            MerchantUsername = System.Web.Configuration.WebConfigurationManager.AppSettings["PaymentUser"].Trim();
            MerchantPassword = System.Web.Configuration.WebConfigurationManager.AppSettings["PaymentPassword"].Trim();
            CertPath = System.Web.Configuration.WebConfigurationManager.AppSettings["FDGGCertPath"].Trim();
            URL = System.Web.Configuration.WebConfigurationManager.AppSettings["FDGGURL"].Trim();

            if (MerchantUsername != string.Empty && MerchantPassword != string.Empty && CertPath!=string.Empty)
            {
                string[] strResponse = ProcessCC(CertPath, MerchantUsername, MerchantPassword, URL);
                strTransID = strResponse[0];
                strApproved = strResponse[1];
                strError = strResponse[2];
                strResponseCodes = strResponse[3] + " " + strResponse[4];
                strResponseText = strApproved + " " + strError;
                strOrderID = strResponse[5];

                MakePayment(strResponseText, strTransID, strApproved, strResponseCodes, strOrderID);

                if (strApproved.Equals("APPROVED", StringComparison.CurrentCultureIgnoreCase))
                {
                    objGeneral.ResetFormControlValues(this);
                    lblMSG.ForeColor = System.Drawing.Color.Green;
                    pnlPay.Visible = false;
                    alert = "Payment Successful.";
                }
                else
                {
                    lblMSG.ForeColor = System.Drawing.Color.Red;
                    alert = "Payment Failed. <BR/>";   // + strResponseText+ "<BR/>"+ strResponseCodes+ "< BR /> "+ strApproved;
                }
            }
            else
            {
                alert = "Invalid Merchant.";
            }
            lblMSG.Text = alert;
           
        }
        catch (Exception ex)
        {
            lblErr.Text = ex.Message + Environment.NewLine + ex.InnerException + Environment.NewLine + ex.StackTrace;
        }              
    }

    private void MakePayment(string ResponseText, string RefID, string Approved, string ResponseCodes, string OrderID)
    {
        try
        {
            int Success = 0;
            if (Approved.Equals("APPROVED", StringComparison.CurrentCultureIgnoreCase))
                Success = 1;

            int Status = 1;
            if (Session["MSM"].ToString() == "TS")
                Status = 5;


            objProp_Contracts.Status = Status;
            objProp_Contracts.TransDate = System.DateTime.Now;
            objProp_Contracts.CardNumber = ccformat(txtCardNumber.Text.Trim());
            //objProp_Contracts.Amount = Convert.ToDouble(ViewState["amt"].ToString().Trim());
            objProp_Contracts.Response = ResponseText;
            objProp_Contracts.PaymentRefID = RefID;
            objProp_Contracts.Approved = Approved;
            objProp_Contracts.IsSuccess = Success;
            objProp_Contracts.ResponseCodes = ResponseCodes;
            objProp_Contracts.UserID = Session["Username"].ToString();
            objProp_Contracts.Screen = "Invoice";
            objProp_Contracts.CustID = Convert.ToInt32(Session["custid"].ToString());
            objProp_Contracts.ConnConfig = Session["config"].ToString();
            objProp_Contracts.PaymentUID = System.Guid.NewGuid();
            objProp_Contracts.GatewayOrderID = OrderID;

            string message = "Payment received for invoice(s)# " + ViewState["uid"].ToString() + ".<BR/>";
            message += "Total Amount Received: $" + ViewState["amt"].ToString() + "<BR/>";
            message += "Date: " + objProp_Contracts.TransDate + "<BR/><BR/>";
            message += "<table width ='250px' style='border:solid 1px #000'>";
            message += "<tr><th style='border:solid 1px #ccc'>Invoice#</th><th style='border:solid 1px #ccc'>Paid Amount</th><th style='border:solid 1px #ccc'>Due Amount</th></tr>";

            List<Dictionary<string, string>> lstInv = (List<Dictionary<string, string>>)ViewState["invlst"];
            foreach (Dictionary<string, string> dr in lstInv)
            {
                objProp_Contracts.InvoiceID = Convert.ToInt32(dr["inv"]);
                objProp_Contracts.Amount = Convert.ToDouble(dr["amt"]);
                object objPay= objBL_Contracts.AddPayment(objProp_Contracts);

                message += "<tr><td style='border:solid 1px #ccc'>" + dr["inv"].ToString() + "</td><td style='border:solid 1px #ccc; text-align:right;'>$" + dr["amt"] + "</td><td style='border:solid 1px #ccc; text-align:right;'>$" + objPay.ToString() + "</td></tr>";
            }
            message += "<tr><th style='border:solid 1px #ccc'>Total</th><th style='border:solid 1px #ccc; text-align:right;'>$" + ViewState["amt"].ToString() + "</th><th style='border:solid 1px #ccc; text-align:right;'></th></tr>";
            message += "</table>";

            if (Success == 1)
            {
                mail(message);
            }
        }
        catch (Exception ex)
        {
            lblErr.Text += ex.Message;
            string error = ex.Message + Environment.NewLine + ex.InnerException + Environment.NewLine + ex.StackTrace + Environment.NewLine + "invoices:" + ViewState["uid"].ToString() + Environment.NewLine + "amt:" + ViewState["amt"].ToString().Trim() + Environment.NewLine + objProp_Contracts.PaymentUID + Environment.NewLine + ccformat(txtCardNumber.Text.Trim()) + Environment.NewLine + objProp_Contracts.Approved;
            log(error);
        }
    }
    
    private string[] ProcessCC(string certpath, string username, string password, string URL)
    {
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.DefaultConnectionLimit = 9999;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;

        //System.Net.ServicePointManager.Expect100Continue = true;
        //System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
        //System.Net.ServicePointManager.Expect100Continue = false;
        //System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Ssl3;

        FDGGWSApiOrderService oFDGGWSApiOrderService = new FDGGWSApiOrderService();
        //////change url for CTE or Production
        ////oFDGGWSApiOrderService.Url = @"https://ws.firstdataglobalgateway.com/fdggwsapi/services";
        ////oFDGGWSApiOrderService.Url = @"https://ws.merchanttest.firstdataglobalgateway.com/fdggwsapi/services";
        ////oFDGGWSApiOrderService.ClientCertificates.Add(X509Certificate.CreateFromCertFile(@"C:\Users\administrator\Desktop\Gateway\Certs\FDGGWS_Certificate_WS1909199349._.1\WS1909199349._.1.pem"));

        oFDGGWSApiOrderService.Url = URL;
        oFDGGWSApiOrderService.ClientCertificates.Add(X509Certificate.CreateFromCertFile(certpath));
        ////Set the Authentication Credentials
        ////NetworkCredential nc = new NetworkCredential("WS1909199349._.1", "rOFPwqXr");
        NetworkCredential nc = new NetworkCredential(username, password);
        oFDGGWSApiOrderService.Credentials = nc;

        ////FDGGWSApiOrderRequest oOrderRequest = new FDGGWSApiOrderRequest();

       MOMWebApp.FirstData.Transaction oTransaction = new MOMWebApp.FirstData.Transaction();

        CreditCardTxType oCreditCardTxType = new CreditCardTxType();
        oCreditCardTxType.Type = CreditCardTxTypeType.sale;
        CreditCardData oCreditCardData = new CreditCardData();
        oCreditCardData.ItemsElementName = new ItemsChoiceType[] { ItemsChoiceType.CardNumber, ItemsChoiceType.ExpMonth, ItemsChoiceType.ExpYear, ItemsChoiceType.CardCodeValue };
        oCreditCardData.Items = new string[] { txtCardNumber.Text.Trim(), ddlMonth.SelectedValue, ddlYear.SelectedValue, txtCVC.Text.Trim() };
        //oTransaction.Items = new object[] { oCreditCardTxType, oCreditCardData };

        TransactionDetails oTransactionDetails = new TransactionDetails();
        oTransactionDetails.InvoiceNumber = objGeneral.Truncate(ViewState["uid"].ToString().Replace(',', ':'), 24);
        oTransactionDetails.Recurring = Recurring.No;
        oTransactionDetails.TransactionOrigin = TransactionDetailsTransactionOrigin.ECI;

        DataSet dsowner = GetCustomerAddress();
        if (dsowner != null)
        {
            Billing bill = new Billing();
            bill.Name = objGeneral.Truncate(txtNameOnCard.Text.Trim(), 90); //dsowner.Tables[0].Rows[0]["contact"].ToString();
            bill.Company = objGeneral.Truncate( txtNameCustomer.Text.Trim(),90);//dsowner.Tables[0].Rows[0]["name"].ToString();
            bill.Address1 = objGeneral.Truncate(dsowner.Tables[0].Rows[0]["address"].ToString(), 90);
            bill.City = objGeneral.Truncate(dsowner.Tables[0].Rows[0]["city"].ToString(), 90);
            bill.State =objGeneral.Truncate( dsowner.Tables[0].Rows[0]["state"].ToString(),90);
            bill.Zip = dsowner.Tables[0].Rows[0]["zip"].ToString();
            bill.Country =objGeneral.Truncate( dsowner.Tables[0].Rows[0]["country"].ToString(),30);
            bill.Email = objGeneral.Truncate(dsowner.Tables[0].Rows[0]["email"].ToString(), 60);
            bill.Phone = objGeneral.Truncate(dsowner.Tables[0].Rows[0]["phone"].ToString(), 30);
            bill.Fax = objGeneral.Truncate(dsowner.Tables[0].Rows[0]["fax"].ToString(), 30);
            
            oTransaction.Billing = bill;
        }

        Notes note = new Notes();
        note.Comments = "Invoice-" + ViewState["uid"].ToString() + "       Name on card- " + txtNameOnCard.Text.Trim() + "      CC- " + ccformat(txtCardNumber.Text.Trim());

        MOMWebApp.FirstData.Payment oPayment = new MOMWebApp.FirstData.Payment();
        decimal amtdc = Convert.ToDecimal(ViewState["amt"].ToString().Trim());
        oPayment.ChargeTotal = Math.Round(amtdc, 2);

        
        oTransaction.Items = new object[] { oCreditCardTxType, oCreditCardData };
        oTransaction.Payment = oPayment;
        oTransaction.TransactionDetails = oTransactionDetails;
        oTransaction.Notes = note;

        FDGGWSApiOrderRequest oOrderRequest = new FDGGWSApiOrderRequest();
        oOrderRequest.Item = oTransaction;
        
        FDGGWSApiOrderResponse oResponse = null;

        string strTransID = string.Empty;
        string strResponseCode = string.Empty;
        string strResponsetext = string.Empty;
        string strProcessResponse = string.Empty;
        string strERROR = string.Empty;
        string strOrderID = string.Empty;

        //for (int i = 0; i < 1; i++)
        //{
            try
            {
                oResponse = oFDGGWSApiOrderService.FDGGWSApiOrder(oOrderRequest);
                string sApprovalCode = oResponse.TransactionResult;

                strTransID = oResponse.TransactionID;
                strResponsetext = oResponse.TransactionResult;
                strERROR = oResponse.ErrorMessage;
                strOrderID = oResponse.OrderId;

                strResponseCode = "ApprovalCode:" + oResponse.ApprovalCode + "; AVSResponse:" + oResponse.AVSResponse + "; FraudAction:" + oResponse.FraudAction;
                strProcessResponse = "ProcessorApprovalCode:" + oResponse.ProcessorApprovalCode + "; ProcessorReferenceNumber:" + oResponse.ProcessorReferenceNumber + "; ProcessorResponseCode:" + oResponse.ProcessorResponseCode + "; ProcessorResponseMessage:" + oResponse.ProcessorResponseMessage;

            }
            catch (System.Web.Services.Protocols.SoapException ex)
            {             
              strResponsetext = ex.ToString();
            }
        //}
        string[] strResponse = new string[6];
        strResponse[0] = strTransID;
        strResponse[1] = strResponsetext;
        strResponse[2] = strERROR;
        strResponse[3] = strResponseCode;
        strResponse[4] = strProcessResponse;
        strResponse[5] = strOrderID;

        return strResponse;
    }

    private void mail(string message)
    {
        User objPropUser = new User();
        BL_User objBL_User = new BL_User();

        Thread email = new Thread(delegate()
        {
            string to = string.Empty;
            string from = string.Empty;
            DataSet dsC = new DataSet();
            objPropUser.ConnConfig = Session["config"].ToString();
            dsC = objBL_User.getControl(objPropUser);
            if (dsC.Tables[0].Rows.Count > 0)
            {
                from = dsC.Tables[0].Rows[0]["Email"].ToString();
            }

            objPropUser.CustomerID = Convert.ToInt32(Session["custid"].ToString());
            to = objBL_User.getCustomerEmail(objPropUser);

            if (to.Trim() != string.Empty && from.Trim()!=string.Empty)
            {
                try
                {
                    Mail mail = new Mail();
                    mail.From = from;
                    mail.To = to.Split(';', ',').OfType<string>().ToList();
                    mail.Bcc.Add(from);
                    mail.Title = "Payment Received for invoice# " + ViewState["uid"].ToString();
                    mail.Text = message;
                    mail.RequireAutentication = false;
                    mail.Send();
                }
                catch (Exception ex)
                {
                    log(ex.Message + Environment.NewLine + ex.InnerException + Environment.NewLine + ex.StackTrace);
                }
            }
        });
        email.IsBackground = true;
        email.Start();
    }
    
    private void log(String message)
    {
        DateTime datetime = DateTime.Now;
        string savepath = Server.MapPath(Request.ApplicationPath) + "/logs/";
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
    
    private string CheckPaid()
    {
        string invoices = string.Empty;
        objProp_Contracts.ConnConfig=Session["config"].ToString();
        objProp_Contracts.InvoiceIDCustom = ViewState["uid"].ToString();
        DataSet dsStatus = objBL_Contracts.GetInvoicesStatus(objProp_Contracts);
        int count = 0;
        foreach (DataRow dr in dsStatus.Tables[0].Rows)
        {
            if (count == 0)
                invoices += dr["ref"].ToString();
            else
                invoices += "," + dr["ref"].ToString();

            count++;
        }

        return invoices;
    }

    private string CheckAmountExceed()
    {
        string discard=string.Empty;
        List<Dictionary<string, string>> lstInv = (List<Dictionary<string, string>>)ViewState["invlst"];
        objProp_Contracts.ConnConfig = Session["config"].ToString();
        objProp_Contracts.InvoiceIDCustom = ViewState["uid"].ToString();
        DataSet ds = objBL_Contracts.GetInvoicesAmount(objProp_Contracts);
        
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            foreach (Dictionary<string, string> dict in lstInv)
            {
                if (string.Equals(dr["ref"].ToString(), dict["inv"].ToString(),StringComparison.CurrentCultureIgnoreCase))
                {
                    if (Convert.ToDouble(dict["amt"]) > Convert.ToDouble(dr["balance"]))
                    {
                        discard = dr["ref"].ToString();
                    }
                }
            }
        }
        return discard;
    }
    
    private string ccformat(string ccnumber)
    {
        string strFormatedcc = string.Empty;
        string strBlankCC = string.Empty;
        string strLastFour = string.Empty;

        for (int i = 0; i < ccnumber.Length - 4; i++)
        {
            strBlankCC += "X";
        }

        if (ccnumber.Length > 4)
        {
            strLastFour = ccnumber.Substring(ccnumber.Length - 4, 4);
        }

        return strFormatedcc = strBlankCC + strLastFour;
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["inv"] != null)
            Response.Redirect("invoices.aspx?fil=1");
        else
            Response.Redirect("printinvoice.aspx?uid=" + ViewState["uid"].ToString());
    }
    
    protected void lnkBack_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["inv"] != null)
            Response.Redirect("invoices.aspx?fil=1");
        else
            Response.Redirect("printinvoice.aspx?uid=" + ViewState["uid"].ToString());
    }

    private DataSet GetCustomerAddress()
    {
        DataSet ds = new DataSet();
        
        try
        {
            objProp_Contracts.Owner = Convert.ToInt32(Session["custid"].ToString());
            objProp_Contracts.ConnConfig = Session["config"].ToString();
            ds = objBL_Contracts.getCustomerAddress(objProp_Contracts);
        }
        catch
        {
            ds = null;
        }
        return ds;       
    }
}
