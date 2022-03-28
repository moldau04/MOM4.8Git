using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessEntity;
using BusinessLayer;
using System.Data;
using System.Net.Configuration;
using HtmlAgilityPack;

public partial class MailTicket : System.Web.UI.Page
{
    BL_MapData objBL_MapData = new BL_MapData();
    MapData objMapData = new MapData();

    User objPropUser = new User();
    BL_User objBL_User = new BL_User();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userid"] == null)
        {
            //string jScript;
            //jScript = "<script>window.parent.location.reload(1);</script>";
            //this.ClientScript.RegisterClientScriptBlock(this.GetType(), "keyClientBlock", jScript);
            Response.Redirect("timeout.htm");
            return;
        }

        if (!IsPostBack)
        {
            objPropUser.ConnConfig = Session["config"].ToString();
            objPropUser.Username = Session["username"].ToString();

            txtEmailFrom.Text = WebBaseUtility.GetFromEmailAddress();

            DataSet ds = new DataSet();
            objMapData.ConnConfig = Session["config"].ToString();
            objMapData.TicketID = Convert.ToInt32(Request.QueryString["id"].ToString());
            objMapData.ISTicketD = Convert.ToInt32(Request.QueryString["c"].ToString());
            ds = objBL_MapData.GetTicketByID(objMapData);
            if (ds.Tables[0].Rows.Count > 0)
            {       
                string Worker = ds.Tables[0].Rows[0]["dwork"].ToString();

                objPropUser.Username = Worker;
                txtEmail.Text = objBL_User.getUserPager(objPropUser);

                string Ticket = ds.Tables[0].Rows[0]["ID"].ToString();
                string Location = ds.Tables[0].Rows[0]["locname"].ToString();
                string Address = ds.Tables[0].Rows[0]["address"].ToString();
                string Phone = ds.Tables[0].Rows[0]["Phone"].ToString();
                string SchDate = string.Format("{0:MM/dd/yy hh:mm tt}", ds.Tables[0].Rows[0]["Edate"]);
                string Cat = ds.Tables[0].Rows[0]["Cat"].ToString();
                string Reason = ds.Tables[0].Rows[0]["fdesc"].ToString();
                string Equip = ds.Tables[0].Rows[0]["unitname"].ToString();
                string Customer= ds.Tables[0].Rows[0]["customerName"].ToString();
                string Caller = ds.Tables[0].Rows[0]["who"].ToString();
                string CallerPhone = ds.Tables[0].Rows[0]["cphone"].ToString();
                string locAddress = ds.Tables[0].Rows[0]["ldesc3"].ToString();
                string EnteredBy = ds.Tables[0].Rows[0]["fby"].ToString();

                string Remarks = ds.Tables[0].Rows[0]["Remarks"].ToString();
                //string TimeRoute = string.Format("{0:hh:mm tt}", ds.Tables[0].Rows[0]["timeroute"].ToString());
                //string TimeSite = string.Format("{0:hh:mm tt}", ds.Tables[0].Rows[0]["timesite"].ToString());
                //string TimeComp = string.Format("{0:hh:mm tt}", ds.Tables[0].Rows[0]["timecomp"].ToString());

                txtSubject.Text = "Ticket # "+Ticket + " " + locAddress;

                string MailBodyShort;
                string DbName = "";
                DbName = Session["dbname"].ToString();

                BL_General bL_General = new BL_General();
                EmailTemplate emailTemplate = new EmailTemplate();
                emailTemplate.ConnConfig = Session["config"].ToString();
                emailTemplate.Screen = "MailTicket";
                emailTemplate.FunctionName = "Email Ticket";
                string mailContent = bL_General.GetEmailTemplate(emailTemplate);

                if (string.IsNullOrEmpty(mailContent))
                {
                    //Ref ES-1964 Colley - Custom Text message format
                    if (DbName.Contains("COLLEY"))  // -----
                    {
                        if (CallerPhone == string.Empty) { CallerPhone = ""; }

                        MailBodyShort = "A new ticket with ID " + Ticket + " has been assigned.<br/>";
                        MailBodyShort += "Location address: " + Address + ".<br/>";
                        MailBodyShort += "Caller name: " + Caller + ".<br/>";
                        MailBodyShort += "Caller phone: " + CallerPhone + ".<br/>";
                        MailBodyShort += "Reason for service: " + Reason + ".<br/>";
                        MailBodyShort += "Entered By: " + EnteredBy + ".<br/>";
                    }
                    else if (DbName.Contains("WESTCOAST"))
                    {
                        if (CallerPhone == string.Empty) { CallerPhone = ""; }

                        MailBodyShort = "A new ticket with ID " + Ticket + " has been assigned<br/>";
                        //MailBodyShort += "Customer: " + Customer + "<br>";
                        MailBodyShort += "Location: " + Location + "<br/>";
                        MailBodyShort += "Address: " + Address + "<br/>";
                        MailBodyShort += "Caller: " + Caller + "<br/>";
                        MailBodyShort += "Phone: " + CallerPhone + "<br/>";
                        MailBodyShort += "Scheduled: " + SchDate + "<br/>";
                        MailBodyShort += "Category: " + Cat + "<br/>";
                        MailBodyShort += "Equipment: " + Equip + "<br/>";
                        MailBodyShort += "Reason: " + Reason + "<br/>";
                        MailBodyShort += "Remarks: " + Remarks + "<br/>";
                    }
                    else
                    {
                        MailBodyShort = "A new ticket with ID " + Ticket + " has been assigned<br/>";
                        MailBodyShort += "Customer: " + Customer + "<br/>";
                        MailBodyShort += "Location: " + Location + "<br/>";
                        MailBodyShort += "Address: " + Address + "<br/>";
                        MailBodyShort += "Caller: " + Caller + "<br/>";
                        if (CallerPhone != string.Empty)
                        {
                            MailBodyShort += "Phone: " + CallerPhone + "<br/>";
                        }
                        MailBodyShort += "Scheduled: " + SchDate + "<br/>";
                        MailBodyShort += "Category: " + Cat + "<br/>";
                        MailBodyShort += "Equipment: " + Equip + "<br/>";
                        MailBodyShort += "Reason: " + Reason + "<br/>";
                    }
                }
                else
                {
                    MailBodyShort = mailContent.Replace("{Ticket}", Ticket)
                        .Replace("{Customer}", Customer)
                        .Replace("{Location}", Location)
                        .Replace("{Address}", Address)
                        .Replace("{Caller}", Caller)
                        .Replace("{CallerPhone}", CallerPhone)
                        .Replace("{ScheduledDate}", SchDate)
                        .Replace("{Category}", Cat)
                        .Replace("{Equipment}", Equip)
                        .Replace("{Reason}", Reason)
                        .Replace("{Remarks}", Remarks)
                        .Replace("{EnteredBy}", EnteredBy);
                }

                txtBody.Text = MailBodyShort;
                
            } 
        }
    }

    private void UpdateEmailNotificationStatus()
    {
        objMapData.ConnConfig = Session["config"].ToString();
        objMapData.TicketID = Convert.ToInt32(Request.QueryString["id"].ToString());
        objBL_MapData.UpdateEmailNotificationStatus(objMapData);
    }

    protected void LnkSend_Click(object sender, EventArgs e)
    {
        if (txtEmail.Text.Trim() != string.Empty)
        {
            try
            {
                string result = txtBody.Text;

                HtmlDocument htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(result);
                result = htmlDoc.DocumentNode.InnerText;

                result = result.Replace("\t", "").Trim('\r', '\n');

                result = result.Replace("&amp;", "&");
                

                //if (result.Length > 140)
                //{
                //    result = result.Substring(0, 137);
                //    result += "...";
                //}

                //if (result.Length <= 140)
                //{

                Mail mail = new Mail();
                mail.From = txtEmailFrom.Text.Trim();
                mail.To = txtEmail.Text.Split(';', ',').OfType<string>().ToList();
                if (txtEmailCc.Text.Trim() != string.Empty)
                {
                    mail.Cc = txtEmailCc.Text.Split(';', ',').OfType<string>().ToList();
                }
                mail.Title = txtSubject.Text;
                mail.Text = result;
                mail.Priority = System.Net.Mail.MailPriority.High;
                mail.RequireAutentication = false;
                WebBaseUtility.UpdateMailConfigurationFromLoginUser(mail);
                mail.IsBodyHtml = false;
                mail.Send();
                ClientScript.RegisterStartupScript(Page.GetType(), "keySucc", "noty({text: 'Email sent successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : false});", true);
                UpdateEmailNotificationStatus();

                //}
                //else
                //{
                //    ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: 'Content of message larger than 140 charaters',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                //}
            }
            catch (Exception ex)
            {
                string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(ex.Message);
                ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '"+ str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);

            }
        }
    }
}
