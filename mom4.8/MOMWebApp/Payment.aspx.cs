using BusinessEntity;
using BusinessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public class TransactionResponse
{
    public const int Approved = 0;
    public const int PartialApproval = 200;
    public const int Declined = 12;
    public const int Duplicate = 110;
}

public partial class Payment : System.Web.UI.Page
{
    BL_Contracts objBL_Contracts = new BL_Contracts();
    Contracts objProp_Contracts = new Contracts();

    User objPropUser = new User();
    BL_User objBL_User = new BL_User();
     
    GeneralFunctions objgn = new GeneralFunctions();

    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        if (Request.QueryString["o"] != null)
        {
            Page.MasterPageFile = "popup.master";
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userid"] == null)
        {
            Response.Redirect("login.aspx");
        }

        string url_path_current = HttpContext.Current.Request.Url.ToString();
        if (url_path_current.StartsWith("http:") == true)
        {
            HttpContext.Current.Response.Redirect("https" + url_path_current.Remove(0, 4), false);
        }

        if (Request.QueryString["o"] == null)
        {
            HtmlGenericControl div = (HtmlGenericControl)Page.Master.FindControl("MenuWrap");
            div.Visible = false;
        }

        if (!IsPostBack)
        {
            //lblInvoiceName.Text = Request.QueryString["uid"].ToString();
            //txtAmount.Text = Request.QueryString["amt"].ToString();

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

            double totalAmt = Math.Round(dblAmount, 2);
            ViewState["invlst"] = lstInv;
            ViewState["amt"] = totalAmt;
            ViewState["uid"] = invcs;
            txtAmount.Text = totalAmt.ToString(); 

        }
    }

    protected void btnPayment_Click(object sender, EventArgs e)
    {
        lblMSG.ForeColor = System.Drawing.Color.Red;
        lblMSG.Text = string.Empty;
        lblErr.Text = string.Empty;

        if (ViewState["amt"].ToString().Trim() == string.Empty)
        {
            lblErr.Text = "Enter Valid Amount";
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

        //if (txtAmount.Text.Trim() == string.Empty || txtAmount.Text.Trim() == "0")
        //{
        //    ClientScript.RegisterStartupScript(Page.GetType(), "keyValidAmt", "alert('Enter Valid Amount')", true);
        //    return;
        //}

        //if (txtCardNumber.Text.Trim() == string.Empty || txtNameOnCard.Text.Trim() == string.Empty || ddlMonth.SelectedValue == string.Empty || ddlYear.SelectedValue == string.Empty || txtCVC.Text.Trim()==string.Empty)
        //{
        //    ClientScript.RegisterStartupScript(Page.GetType(), "keyValid", "alert('Invalid Information')", true);
        //    return;
        //}

        string response = string.Empty;
        string RefID = string.Empty;
        string alert = string.Empty;
        string resp = string.Empty;

        try
        {
            if (ValidateCard() != 0)
            {
                ClientScript.RegisterStartupScript(Page.GetType(), "keyValidCard", "alert('Invalid Card Number')", true);
                return;
            }

            string MerchantUsername = string.Empty;
            string MerchantPassword = string.Empty;

            MerchantUsername = System.Web.Configuration.WebConfigurationManager.AppSettings["PaymentUser"].Trim();
            MerchantPassword = System.Web.Configuration.WebConfigurationManager.AppSettings["PaymentPassword"].Trim();

            //DataSet dsPayment = new DataSet();
            //objProp_Contracts.ConnConfig = Session["config"].ToString();
            //objProp_Contracts.MerchantID = GetUserInfo().ToString();
            //dsPayment = objBL_Contracts.getPaymentGatewayInfo(objProp_Contracts);
            //if (dsPayment.Tables[0].Rows.Count != 0)
            //{
            //    MerchantUsername = dsPayment.Tables[0].Rows[0]["Username"].ToString();
            //    MerchantPassword = AES_Algo.Decrypt(dsPayment.Tables[0].Rows[0]["Password"].ToString(), "MSMPAY", "4Bvq75DG", "SHA1", 1000, "pOWaTbO92LfXbh69JkYzfT7P465TNc0h", 256).TrimEnd('\0');
            //}

            if (MerchantUsername != string.Empty && MerchantPassword != string.Empty)
            {
                string[] strResponse = ProcessCreditCard(MerchantUsername, MerchantPassword);
                response = strResponse[0] + " : " + strResponse[1];
                RefID = strResponse[2];
                resp = strResponse[0];
                
                alert = strResponse[1] + "</BR> Transaction ID: " + RefID;
                MakePayment(response, RefID, response, string.Empty, string.Empty);
                
                if (resp == TransactionResponse.Approved.ToString())
                {
                    //if (Request.QueryString["o"] == null)
                    //{
                    //    ClientScript.RegisterStartupScript(Page.GetType(), "keyRedirect", "window.location.href='addinvoice.aspx?uid=" + Request.QueryString["uid"].ToString() + "'", true);
                    //}
                    //else
                    //{
                    //    ClientScript.RegisterStartupScript(Page.GetType(), "keyRedirect", "window.location.href='addinvoice.aspx?uid=" + Request.QueryString["uid"].ToString() + "&o=1'", true);

                    //}
                    GeneralFunctions objGeneral = new GeneralFunctions();
                    objGeneral.ResetFormControlValues(this);
                    lblMSG.ForeColor = System.Drawing.Color.Green;
                    pnlPay.Visible = false;
                    alert = "Payment Successful.";
                }
                else
                {
                    lblMSG.ForeColor = System.Drawing.Color.Red;
                    alert = "Payment Failed. <BR/>" + response;
                }
            }
            else
            {
                alert = "Invalid Merchant. Please assign merchant to the logged in user.";
                response = "Invalid Merchant";
            }
            lblMSG.Text = alert;
            //ClientScript.RegisterStartupScript(Page.GetType(), "keyResp", "noty({text: '" + alert + "',  type : 'information', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
        catch (Exception ex)
        {            
            response = ex.Message;
            lblErr.Text = ex.Message;
            //string str = "Payment Failure : " + ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            //ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
        //finally
        //{
        //    MakePayment(response, RefID, response, string.Empty, string.Empty);
        //}        
    }

    //private void MakePayment(string response, string RefID)
    //{
    //    try
    //    {
    //        objProp_Contracts.InvoiceID = Convert.ToInt32(Request.QueryString["uid"].ToString());
    //        objProp_Contracts.TransDate = System.DateTime.Now;
    //        objProp_Contracts.CardNumber = ccformat(txtCardNumber.Text.Trim());
    //        objProp_Contracts.Amount = Convert.ToDouble(txtAmount.Text.Trim());
    //        objProp_Contracts.Response = response;
    //        objProp_Contracts.PaymentRefID = RefID;
    //        objProp_Contracts.UserID = Session["Username"].ToString();
    //        objProp_Contracts.Screen = "Invoice";

    //        objProp_Contracts.ConnConfig = Session["config"].ToString();
    //        objBL_Contracts.AddPayment(objProp_Contracts);
    //    }
    //    catch (Exception ex)
    //    {

    //    }
    //}

    private void MakePayment(string ResponseText, string RefID, string Approved, string ResponseCodes, string OrderID)
    {
        try
        {
            int Success = 0;
            if (Approved.Equals("transaction was approved", StringComparison.CurrentCultureIgnoreCase))
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
                object objPay = objBL_Contracts.AddPayment(objProp_Contracts);

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
            //lblErr.Text += ex.Message;
            string error = ex.Message + Environment.NewLine + ex.InnerException + Environment.NewLine + ex.StackTrace + Environment.NewLine + "invoices:" + ViewState["uid"].ToString() + Environment.NewLine + "amt:" + ViewState["amt"].ToString().Trim() + Environment.NewLine + objProp_Contracts.PaymentUID + Environment.NewLine + ccformat(txtCardNumber.Text.Trim()) + Environment.NewLine + objProp_Contracts.Approved;
            log(error);
        }
    }


    private string[] ProcessCreditCard(string MerchantUsername, string MerchantPassword)
    {
        //string MerchantUsername = string.Empty;
        //string MerchantPassword = string.Empty;
        
        //DataSet dsPayment = new DataSet();
        //objProp_Contracts.ConnConfig = Session["config"].ToString();
        //objProp_Contracts.MerchantID = GetUserInfo().ToString();
        //dsPayment = objBL_Contracts.getPaymentGatewayInfo(objProp_Contracts);
        //if (dsPayment.Tables[0].Rows.Count != 0)
        //{
        //    MerchantUsername = dsPayment.Tables[0].Rows[0]["Username"].ToString();
        //    MerchantPassword = AES_Algo.Decrypt(dsPayment.Tables[0].Rows[0]["Password"].ToString(), "MSMPAY", "4Bvq75DG", "SHA1", 1000, "pOWaTbO92LfXbh69JkYzfT7P465TNc0h", 256).TrimEnd('\0');
        //}
       
        MOMWebApp.WS_GlobalPayments.WS_GlobalPayments sdx = new MOMWebApp.WS_GlobalPayments.WS_GlobalPayments();
                
        //string username = "paya3928";
        //string password = "Test2013";
        string username = MerchantUsername;
        string password = MerchantPassword;
        string trans_type = "Sale";
        string card_num = txtCardNumber.Text.Trim() ;//"4003000123456781";
        string exp_date = ddlMonth.SelectedValue + ddlYear.SelectedValue; //"1215";
        string mag_data = "";
        string name_on_card = txtNameOnCard.Text.Trim();
        string amount = ViewState["amt"].ToString().Trim(); //txtAmount.Text.Trim(); //"1.06";
        string inv_num = "";
        string pnref = "";
        string zip = "";
        string street = "";
        string cvnum = txtCVC.Text.Trim();
        string ext_data = "";

        var res = sdx.ProcessCreditCard(username, password, trans_type, card_num, exp_date, mag_data, name_on_card, amount, inv_num, pnref, zip, street, cvnum, ext_data);


        string strTransID = string.Empty;
        string strResponseCode = string.Empty;
        string strResponsetext = string.Empty;        

        strTransID = res.PNRef;
        strResponseCode = Convert.ToString(res.Result);

        if (res.Result == TransactionResponse.Approved)
        {
            strResponsetext = "transaction was approved";            
        }
        else if (res.Result == TransactionResponse.PartialApproval)
        {
            strResponsetext = "transaction was partial approved";            
        }
        else if (res.Result == TransactionResponse.Duplicate)
        {
            strResponsetext = "duplicate transaction";
        }
        else if (res.Result == TransactionResponse.Declined)
        {
            strResponsetext = "transaction was declined";
        }
        else
        {
            strResponsetext = "error processing transaction - " + res.RespMSG;           
        }

        string[] strResponse=new string[3];
        strResponse[0]=strResponseCode;
        strResponse[1]=strResponsetext;
        strResponse[2]=strTransID;

        return strResponse;
    }

    private int ValidateCard()
    {
        MOMWebApp.WS_Validate.WS_Validate validate = new MOMWebApp.WS_Validate.WS_Validate();
        return validate.ValidCard(txtCardNumber.Text.Trim(),ddlMonth.SelectedValue+ddlYear.SelectedValue);
    }

    private string ccformat(string ccnumber)
    {
        string strFormatedcc = string.Empty;
        string strBlankCC=string.Empty;
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

    private int GetUserInfo()
    {
        objPropUser.UserID = Convert.ToInt32(Session["UserID"].ToString());
        objPropUser.TypeID = 1;
        objPropUser.DBName = Session["dbname"].ToString();
        DataSet ds = new DataSet();
        ds = objBL_User.GetUserPermissionByUserID(objPropUser);

        int MerchantInfoID = 0;
        if (ds.Tables[0].Rows.Count > 0)
        {
            string strMID=ds.Tables[0].Rows[0]["merchantinfoid"].ToString();
            if (!string.IsNullOrEmpty(strMID))
            {
                MerchantInfoID = Convert.ToInt32(strMID);
            }
        }
        return MerchantInfoID;
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        //if (Request.QueryString["o"] == null)
        //{
        //    Response.Redirect("addinvoice.aspx?uid=" + Request.QueryString["uid"].ToString());
        //}
        //else
        //{
        //    Response.Redirect("addinvoice.aspx?uid=" + Request.QueryString["uid"].ToString()+"&o=1");
        //}

        if (Request.QueryString["inv"] != null)
            Response.Redirect("invoices.aspx?fil=1");
        else
            Response.Redirect("printinvoice.aspx?uid=" + ViewState["uid"].ToString());
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

            if (to.Trim() != string.Empty && from.Trim() != string.Empty)
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

    protected void lnkBack_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["inv"] != null)
            Response.Redirect("invoices.aspx?fil=1");
        else
            Response.Redirect("printinvoice.aspx?uid=" + ViewState["uid"].ToString());
    }

    private string CheckPaid()
    {
        string invoices = string.Empty;
        objProp_Contracts.ConnConfig = Session["config"].ToString();
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
        string discard = string.Empty;
        List<Dictionary<string, string>> lstInv = (List<Dictionary<string, string>>)ViewState["invlst"];
        objProp_Contracts.ConnConfig = Session["config"].ToString();
        objProp_Contracts.InvoiceIDCustom = ViewState["uid"].ToString();
        DataSet ds = objBL_Contracts.GetInvoicesAmount(objProp_Contracts);

        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            foreach (Dictionary<string, string> dict in lstInv)
            {
                if (string.Equals(dr["ref"].ToString(), dict["inv"].ToString(), StringComparison.CurrentCultureIgnoreCase))
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
    
}
