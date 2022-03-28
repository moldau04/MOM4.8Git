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
using System.IO;
using System.Threading;
using ACHBAL;
using System.Web.Configuration;
using System.Text;
using Renci.SshNet;
using Microsoft.ApplicationBlocks.Data;

public partial class ACHPayment:System.Web.UI.Page
{
    BL_Contracts objBL_Contracts=new BL_Contracts();
    Contracts objProp_Contracts=new Contracts();

    GeneralFunctions objGeneral=new GeneralFunctions();
    protected void Page_Load(object sender,EventArgs e)
    {
        HtmlGenericControl LiDiv = (HtmlGenericControl)Page.Master.FindControl("LiDiv");
        LiDiv.Visible = false;

        if (Session["userid"]==null)
        {
            Response.Redirect("login.aspx");
        }
        if(Convert.ToInt16(Session["payment"])!=1)
        {
            Response.Redirect("home.aspx");
        }
        string url_path_current = HttpContext.Current.Request.Url.ToString();
        if (url_path_current.StartsWith("http:") == true)
        {
            HttpContext.Current.Response.Redirect("https" + url_path_current.Remove(0, 4), false);
        }
        if(!IsPostBack)
        {
            if(Session["uidv"]==null)
            {
                Response.Redirect("invoices.aspx?fil=c");
            }

            List<Dictionary<string,string>> lstInv=new List<Dictionary<string,string>>();
            if(Session["uidv"]!=null)
            {
                lstInv=(List<Dictionary<string,string>>)Session["uidv"];
                Session["uidv"]=null;
            }


            double dblAmount=0;
            string invcs=string.Empty;
            int count=0;
            foreach(Dictionary<string,string> d in lstInv)
            {
                dblAmount+=Convert.ToDouble(d["amt"]);
                if(count==0)
                    invcs+=d["inv"].ToString();
                else
                    invcs+=","+d["inv"].ToString();

                count++;
            }
            txtInvoices.Text=invcs;
            txtInvoices.ReadOnly=true;
            txtInvoices.TextMode=TextBoxMode.MultiLine;
            double totalAmt=Math.Round(dblAmount,2);
            ViewState["invlst"]=lstInv;
            ViewState["amt"]=totalAmt.ToString("0.00");
            ViewState["uid"]=invcs;
            txtAmount.Text=ViewState["amt"].ToString();
            GetACHCustomerAccounts();
        }
    }

    protected void btnPayment_Click(object sender,EventArgs e)
    {

        lblMSG.ForeColor=System.Drawing.Color.Red;
        lblMSG.Text=string.Empty;
        lblErr.Text=string.Empty;

        if(ViewState["amt"].ToString().Trim()==string.Empty)
        {
            lblErr.Text="Enter Valid Amount";
            return;
        }

        if(Convert.ToDouble(ViewState["amt"].ToString().Trim())==0)
        {
            lblErr.Text="Enter Valid Amount";
            return;
        }  

        string InvoicesChk=CheckPaid();

        if(InvoicesChk.Trim().Replace(",","")!=string.Empty)
        {
            lblErr.Text="Invoice(s)# "+InvoicesChk+" Already Paid, transaction declined.";
            return;
        }

        string InvoicesUnderProccessChk = CheckInvoicesUnderProccess();

        if (InvoicesUnderProccessChk.Trim().Replace(",", " ") != string.Empty)
        {
            lblErr.Text = "Payment on this invoice# (" + InvoicesUnderProccessChk + ") is already under process.";
            return;
        }

        string InvoicesAmountExceed=CheckAmountExceed();
        if(InvoicesAmountExceed.Trim()!=string.Empty)
        {
            lblErr.Text="Invoice# "+InvoicesAmountExceed+" exceeds maximum payment amount, transaction declined.";
            return;
        }

        string alert=string.Empty;
        //string resp=string.Empty;

        bool Sent=false;
        string ACHfileResponseText=string.Empty;
        string ACHControleResponseText=string.Empty;
        string ACHfileName=string.Empty;
        string ACHControlefileName=string.Empty;       
        string Time=DateTime.Now.ToString("yyyyMMdd.hhmmss");
        string FileCreationTime=DateTime.Now.Hour.ToString()+":"+DateTime.Now.Minute.ToString();
        string FileCreationDate=DateTime.Now.ToString("yy/MM/dd");
        string TempACHfileName=string.Empty;
        try
        {

            /*Create file from user's bank detail and save it locally to path given in web.config*/
             ACHfileName=CreateAchFile(Time,FileCreationTime,FileCreationDate,out TempACHfileName);
            //Create ACH controle File
            ACHControlefileName=CreateAchControleFile(Time,FileCreationTime,FileCreationDate);

            /*Send file through SFTP protocol to PNC bank server*/
           ACHfileResponseText=SendFile(ACHfileName);
           ACHControleResponseText=SendFile(ACHControlefileName);
            if(ACHfileResponseText == "true" && ACHControleResponseText =="true")
            {
                Sent=true;
            }
            /*Update transaction status in MOM*/
           MakePayment(TempACHfileName,Sent,ACHfileResponseText);

            if(Sent)
            {
                AddACHCustomerAccounts();
                objGeneral.ResetFormControlValues(this);
                lblMSG.ForeColor=System.Drawing.Color.Green;
                pnlPay.Visible=false;
                alert = "ACH File has been sent successfully to PNC Server. <BR/>Your payment will be processed soon.";
                Session["uidv"] = null;


            }
            else
            {
                lblMSG.ForeColor=System.Drawing.Color.Red;
                alert="ACH Payment Failed. <BR/> ACH File Response :-"+ACHfileResponseText;
                alert+="<BR/> ACH Controle Response  :-"+ACHControleResponseText;
                Session["uidv"] = null;
            }

            lblMSG.Text=alert;

        }
        catch(Exception ex)
        {
            lblErr.Text=ex.Message+Environment.NewLine+ex.InnerException+Environment.NewLine+ex.StackTrace;
        }
    }
    protected void btnCancel_Click(object sender,EventArgs e)
    {
        if(Request.QueryString["inv"]!=null)
            Response.Redirect("invoices.aspx?fil=c");
        else
            Response.Redirect("printinvoice.aspx?uid="+ViewState["uid"].ToString());
    }

    protected void lnkBack_Click(object sender,EventArgs e)
    {
        if(Request.QueryString["inv"]!=null)
            Response.Redirect("invoices.aspx?fil=c");
        else
            Response.Redirect("printinvoice.aspx?uid="+ViewState["uid"].ToString());
    }
    
    private string CheckPaid()
    {
        string invoices=string.Empty;
        objProp_Contracts.ConnConfig=Session["config"].ToString();
        objProp_Contracts.InvoiceIDCustom=ViewState["uid"].ToString();
        DataSet dsStatus=objBL_Contracts.GetInvoicesStatus(objProp_Contracts);
        int count=0;
        foreach(DataRow dr in dsStatus.Tables[0].Rows)
        {
            if(count==0)
                invoices+=dr["ref"].ToString();
            else
                invoices+=","+dr["ref"].ToString();

            count++;
        }

        return invoices;
    }
    private string CheckInvoicesUnderProccess()
    {
        string invoices = string.Empty;
        objProp_Contracts.ConnConfig = Session["config"].ToString();
        objProp_Contracts.InvoiceIDCustom = ViewState["uid"].ToString(); 

        StringBuilder varname1 = new StringBuilder();
        varname1.Append("SELECT  i.InvoiceID \n");
        varname1.Append("FROM   tblPaymentHistory i \n");
        varname1.Append("WHERE  InvoiceID IN ( " + objProp_Contracts.InvoiceIDCustom + " ) \n");
        varname1.Append("AND isnull(PayType,'')='ACH' \n");
        varname1.Append("AND isnull(Approved,'')='Sent' ");

        try
        {
             DataSet dsStatus= SqlHelper.ExecuteDataset(objProp_Contracts.ConnConfig, CommandType.Text, varname1.ToString());
            if (dsStatus != null)
            {
                int count = 0;
                foreach (DataRow dr in dsStatus.Tables[0].Rows)
                {
                    if (count == 0)
                        invoices += dr["InvoiceID"].ToString();
                    else
                        invoices += "," + dr["InvoiceID"].ToString();

                    count++;
                }
            }
            return invoices;
        }
        catch (Exception ex)
        {
            return ex.Message;
        }

         
    }
    private void GetACHCustomerAccounts()
    {
        int OwnerID = 0;
        string ConnConfig = string.Empty;
        ConnConfig = Session["config"].ToString();
        OwnerID = Convert.ToInt32(Session["custid"].ToString());
        DataSet dsStatus = objBL_Contracts.GetACHCustomerAccounts(OwnerID, ConnConfig);
        if (dsStatus.Tables[0].Rows.Count > 0)
        {
            GridViewACHPayment.DataSource = dsStatus;
            GridViewACHPayment.DataBind();
            if (dsStatus.Tables[0].Rows.Count == 1)
            {
                txtRouting.Text = dsStatus.Tables[0].Rows[0]["RoutingNo"].ToString();
                txtBankAc.Text = dsStatus.Tables[0].Rows[0]["AccountNo"].ToString();
                txtName.Text = dsStatus.Tables[0].Rows[0]["Name"].ToString();
                GridViewACHPayment.Visible = false;
            }
        }
        else
        {
            GridViewACHPayment.DataSource = null;
            GridViewACHPayment.DataBind();
            GridViewACHPayment.Visible = false;
        }
    }
    private void AddACHCustomerAccounts()
    {
        int OwnerID=0;
        string ConnConfig=string.Empty;
        string RoutingNo=txtRouting.Text;
        string AccountNo=txtBankAc.Text;
        string Name=txtName.Text;
        ConnConfig=Session["config"].ToString();
        OwnerID=Convert.ToInt32(Session["custid"].ToString());
        objBL_Contracts.AddACHCustomerAccounts(OwnerID,RoutingNo,AccountNo,Name,ConnConfig);
    }

    private string CheckAmountExceed()
    {
        string discard=string.Empty;
        List<Dictionary<string,string>> lstInv=(List<Dictionary<string,string>>)ViewState["invlst"];
        objProp_Contracts.ConnConfig=Session["config"].ToString();
        objProp_Contracts.InvoiceIDCustom=ViewState["uid"].ToString();
        DataSet ds=objBL_Contracts.GetInvoicesAmount(objProp_Contracts);

        foreach(DataRow dr in ds.Tables[0].Rows)
        {
            foreach(Dictionary<string,string> dict in lstInv)
            {
                if(string.Equals(dr["ref"].ToString(),dict["inv"].ToString(),StringComparison.CurrentCultureIgnoreCase))
                {
                    if(Convert.ToDouble(dict["amt"])>Convert.ToDouble(dr["balance"]))
                    {
                        discard=dr["ref"].ToString();
                    }
                }
            }
        }
        return discard;
    }
    /// <summary>
    /// Make Payment
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="Sent"></param>
    /// <param name="ResponseText"></param>
    private void MakePayment(string fileName,bool Sent,string ResponseText)
    {
        try
        {
            int Success=0;
            if(Sent)
                Success=1;

            int Status=1;
            if(Session["MSM"].ToString()=="TS")
                Status=5;


            objProp_Contracts.Status=Status;
            objProp_Contracts.TransDate=System.DateTime.Now;
            //objProp_Contracts.CardNumber = ccformat(txtCardNumber.Text.Trim());
            //////objProp_Contracts.Amount = Convert.ToDouble(ViewState["amt"].ToString().Trim());
            objProp_Contracts.Response=ResponseText=="true"?"Sent":ResponseText;
            //objProp_Contracts.PaymentRefID = RefID;
            objProp_Contracts.Approved="Sent";
            objProp_Contracts.IsSuccess=Success;
            //objProp_Contracts.ResponseCodes = ResponseCodes;
            objProp_Contracts.UserID=Session["Username"].ToString();
            objProp_Contracts.Screen="Invoice";
            objProp_Contracts.CustID=Convert.ToInt32(Session["custid"].ToString());
            objProp_Contracts.ConnConfig=Session["config"].ToString();
            objProp_Contracts.PaymentUID=System.Guid.NewGuid();
            //objProp_Contracts.GatewayOrderID = OrderID;

            objProp_Contracts.Routing=txtRouting.Text.Trim();
            objProp_Contracts.BankAC=txtBankAc.Text.Trim();
            objProp_Contracts.BankAcHolName=txtName.Text.Trim();
            objProp_Contracts.FileName=fileName;
            objProp_Contracts.PayType="ACH";

            string message="Payment made through ACH for invoice(s)# "+ViewState["uid"].ToString()+".<BR/>";
            message += "ACH File has been sent successfully to PNC Server. Your payment will be processed soon.<BR/>";
            message+="Total Amount Paid: $"+ViewState["amt"].ToString()+"<BR/>";
            message+="Date: "+objProp_Contracts.TransDate+"<BR/><BR/>";
            message+="<table width ='250px' style='border:solid 1px #000'>";
            message+="<tr><th style='border:solid 1px #ccc'>Invoice#</th><th style='border:solid 1px #ccc'>Paid Amount</th><th style='border:solid 1px #ccc'>Due Amount</th></tr>";

            List<Dictionary<string,string>> lstInv=(List<Dictionary<string,string>>)ViewState["invlst"];
            foreach(Dictionary<string,string> dr in lstInv)
            {
                objProp_Contracts.InvoiceID=Convert.ToInt32(dr["inv"]);
                objProp_Contracts.Amount=Convert.ToDouble(dr["amt"]);
                object objPay=objBL_Contracts.AddPayment(objProp_Contracts);

                message+="<tr><td style='border:solid 1px #ccc'>"+dr["inv"].ToString()+"</td><td style='border:solid 1px #ccc; text-align:right;'>$"+dr["amt"]+"</td><td style='border:solid 1px #ccc; text-align:right;'>$"+objPay.ToString()+"</td></tr>";
            }
            message+="<tr><th style='border:solid 1px #ccc'>Total</th><th style='border:solid 1px #ccc; text-align:right;'>$"+ViewState["amt"].ToString()+"</th><th style='border:solid 1px #ccc; text-align:right;'></th></tr>";
            message+="</table>";

            if (Success == 1)
            {
                mail(message);
            }
        }
        catch(Exception ex)
        {
            lblErr.Text+=ex.Message;
            string error=ex.Message
                +Environment.NewLine+ex.InnerException
                +Environment.NewLine+ex.StackTrace
                +Environment.NewLine+"invoices:"+ViewState["uid"].ToString()
                +Environment.NewLine+"amt:"+ViewState["amt"].ToString().Trim()
                +Environment.NewLine+objProp_Contracts.PaymentUID
                +Environment.NewLine+(txtBankAc.Text.Trim())
                +Environment.NewLine+objProp_Contracts.Approved;
            log(error);
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="message"></param>
    private void mail(string message)
    {
        User objPropUser=new User();
        BL_User objBL_User=new BL_User();

        Thread email=new Thread(delegate()
        {
            string to=string.Empty;
            string from=string.Empty;
            DataSet dsC=new DataSet();
            objPropUser.ConnConfig=Session["config"].ToString();
            dsC=objBL_User.getControl(objPropUser);
            if(dsC.Tables[0].Rows.Count>0)
            {
                from=dsC.Tables[0].Rows[0]["Email"].ToString();
            }

            objPropUser.CustomerID=Convert.ToInt32(Session["custid"].ToString());
            to = objBL_User.getCustomerEmail(objPropUser);

            if(to.Trim()!=string.Empty&&from.Trim()!=string.Empty)
            {
                try
                {
                    Mail mail=new Mail();
                    mail.From=from;
                    mail.To=to.Split(';',',').OfType<string>().ToList();
                    mail.Bcc.Add(from);
                    mail.Title = "ACH File has been sent successfully to PNC Server for invoice# " + ViewState["uid"].ToString();
                    mail.Text=message;
                    mail.RequireAutentication=false;
                    mail.Send();
                }
                catch(Exception ex)
                {
                    log(ex.Message+Environment.NewLine+ex.InnerException+Environment.NewLine+ex.StackTrace);
                }
            }
        });
        email.IsBackground=true;
        email.Start();
    }

    private void log(String message)
    {
        DateTime datetime=DateTime.Now;
        string savepath=Server.MapPath(Request.ApplicationPath)+"/logs/";
        String oFileName=savepath+"MOM_"+datetime.ToString("dd_MM_yyyy")+".log";
        if(!Directory.Exists(savepath))
        {
            Directory.CreateDirectory(savepath);
        }
        if(!File.Exists(oFileName))
        {
            System.IO.FileStream f=File.Create(oFileName);
            f.Close();
        }

        try
        {
            System.IO.StreamWriter writter=File.AppendText(oFileName);
            writter.WriteLine(datetime.ToString("MM-dd hh:mm")+" > "+message);
            writter.Flush();
            writter.Close();
        }
        catch(Exception e)
        {
            Console.WriteLine(e.Message.ToString());
        }
    }
    /// <summary>
    /// Create Ach File
    /// </summary>
    /// <param name="Time"></param>
    /// <param name="FileCreationTime"></param>
    /// <param name="FileCreationDate"></param>
    /// <returns></returns>
    private string CreateAchFile(string Time,string FileCreationTime,string FileCreationDate,out string  TempACHfileName)
    {
        string FileName=string.Empty;
        TempACHfileName=string.Empty;
        string path=String.Format(WebConfigurationManager.AppSettings["ACH_File_Path"].Trim());
        // specify your path
        if(!string.IsNullOrEmpty(path))
        {
            //FileName=path+"aut.southern.nacha.in."+Time+".txt";
            //TempACHfileName="aut.southern.nacha.in."+Time+".txt";
            FileName = path + "aut.southern.nacha.in." + Time ;
            TempACHfileName = "aut.southern.nacha.in." + Time ;
            filecontrolvariables.ResetValues();
            Append.ResetValues();

            ACH ObjACH=new ACH(); 
            //---------------
            ObjACH.CreateFileHeader(FileName.Trim(),FileCreationTime,FileCreationDate);
           //---------------     
            ObjACH.AddEntryDetails(txtRouting.Text,txtName.Text,txtBankAc.Text,ViewState["amt"].ToString());
            //---------------
            ObjACH.CreateBatchHeader();
            //----------------
            ObjACH.CreateFileControle(); 

        }
        return FileName;
    }
    /// <summary>
    /// Create Ach Controle File
    /// </summary>
    /// <param name="Time"></param>
    /// <param name="FileCreationTime"></param>
    /// <param name="FileCreationDate"></param>
    /// <returns></returns>
    private string CreateAchControleFile(string Time,string FileCreationTime,string FileCreationDate)
    {
        string FileName=string.Empty;

        string path=String.Format(WebConfigurationManager.AppSettings["ACH_File_Path"].Trim());
        if(!string.IsNullOrEmpty(path))
        {
            // specify your path
            //FileName=path+"aut.southern.cntl.in."+Time+".txt";
            FileName = path + "aut.southern.cntl.in." + Time ;
          
            string DecimalValue=string.Empty;
            string Amount=string.Empty;
            string ZeroValue=string.Empty;
            string Amt=ViewState["amt"].ToString();           
            Amount=Amt.ToString().Replace(".","");
            //---------------
            string CustomerNumber="0040000328";
            string NumberofCredits="00000000";
            string CreditAmount = "000000000000";
            string NumberofDebits="00000001";
            string DebitAmount="";
            string ItemCount="00000001";
            string SourceID="AUTPCG14";
            for(int i=1;i<=12-Amount.Length;i++)
            {
                DebitAmount += "0";
            }
            DebitAmount = DebitAmount + Amount;
            string Data=CustomerNumber+NumberofCredits+CreditAmount+NumberofDebits+DebitAmount+ItemCount+SourceID;
            using(StreamWriter file=new StreamWriter(FileName))
            {
                file.WriteLine(Data);
            } 
        }
        return FileName;
    }
    /// <summary>
    /// Send File 
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    private string SendFile(string fileName)
    {
        string Response=string.Empty;
        try
        {
            //sFTP Folder name. Leave blank if you want to upload to root folder.
            string sFTP_Folder = WebConfigurationManager.AppSettings["sFTP_Folder"].Trim();
            //sFTP Server URL.
            var sFTP_Host = WebConfigurationManager.AppSettings["sFTP_Host"].Trim();
            var sFTP_Port = WebConfigurationManager.AppSettings["sFTP_Port"].Trim();
            var sFTP_UserName = WebConfigurationManager.AppSettings["sFTP_UserName"].Trim();
            var sFTP_Password = WebConfigurationManager.AppSettings["sFTP_Password"].Trim();
            // path for file you want to upload
            var uploadFile = fileName;
            using (var client = new SftpClient(sFTP_Host, Convert.ToInt16(sFTP_Port), sFTP_UserName, sFTP_Password))
            {
                client.Connect();
                client.ChangeDirectory(sFTP_Folder);
                if (client.IsConnected)
                {
                    using (var fileStream = new FileStream(uploadFile, FileMode.Open))
                    {
                        client.BufferSize = 4 * 1024; // bypass Payload error large files
                        client.UploadFile(fileStream, Path.GetFileName(uploadFile));
                    }
                }
            }
            return Response="true";
    }
        catch(Exception ex) 
        {
            return Response=ex.Message.ToString();
        } 
    }
  
    protected void chkcheck_CheckedChanged(object sender,EventArgs e)
    {
        CheckBox activeCheckBox=sender as CheckBox;
        txtBankAc.Text=txtRouting.Text=txtName.Text="";

        foreach(GridViewRow rw in GridViewACHPayment.Rows)
        {
            CheckBox chkBx=(CheckBox)rw.FindControl("chkcheck");
            if(chkBx!=activeCheckBox)
            {
                chkBx.Checked=false;
            }
            if(chkBx.Checked)
            {
                Label BankAc=(Label)rw.FindControl("lblAccountNo");
                Label Routing=(Label)rw.FindControl("lblroutingno");
                Label Name=(Label)rw.FindControl("lblName");
                txtBankAc.Text=BankAc.Text;
                txtRouting.Text=Routing.Text;
                txtName.Text=Name.Text;
            }
        } 
    }
     
}