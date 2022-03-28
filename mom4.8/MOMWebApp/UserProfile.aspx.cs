using BusinessEntity;
using BusinessLayer;
using ImapX;
using MailKit.Security;
using System;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class UserProfile : System.Web.UI.Page
{
    User objPropUser = new User();
    BL_User objBL_User = new BL_User();
    private static bool _editState { get; set; }
    private static int _dashBoardID { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userid"] == null)
        {
            Response.Redirect("login.aspx");
        }
        if (!IsPostBack)
        {

            /////////////---- TO KEEP THE FORM VISIBLE AFTER POSTBACK USE THE BELOW CODE -----/////////////
            //revealcard.Attributes.Add("style", "display: block; transform: translateY(-100%);");
            /////////////////END/////////////////////
            LoadUserProfile();
            LoadListDashboard();
        }
    }

    private void LoadUserProfile()
    {

        var userinfo = (DataTable)Session["userinfo"];
        var userId = Session["userid"];
        int usertypeid = 0;
        if (userinfo != null)
        {
            usertypeid = Convert.ToInt32(userinfo.Rows[0]["usertypeid"]);
        }
        objPropUser.UserID = Convert.ToInt32(userId);
        objPropUser.TypeID = usertypeid;
        objPropUser.ConnConfig = Session["config"].ToString();
        objPropUser.DBName = Session["dbname"].ToString();
        ViewState["usertypeid"] = usertypeid;
        ViewState["userid"] = userId.ToString();
        hdnUserId.Value = userId.ToString();

        //Check user type is customer
        if (usertypeid == 2)
        {
            objPropUser.CustomerID = Convert.ToInt32(userId);
            LoadCustomerInfo();
        }
        else
        {
            LoadUserInfo();
        }

        lblUserType.InnerText = ddlUserType.SelectedItem.Text;
    }

    private void LoadCustomerInfo()
    {
        var ds = objBL_User.getCustomerByID(objPropUser);

        if (ds.Tables[0].Rows.Count > 0)
        {
            var row = ds.Tables[0].Rows[0];
            txtFName.Text = row["name"].ToString();
            lblUserName.Text = row["name"].ToString();
            txtAddress.InnerText = row["Address"].ToString();
            txtCity.Text = row["City"].ToString();
            txtPassword.Text = row["password"].ToString();
            ddlState.Text = row["State"].ToString();
            txtUserName.Text = row["flogin"].ToString();
            txtZip.Text = row["Zip"].ToString();
            txtTelePhone.Text = row["phone"].ToString();
            txtEmail.Text = row["email"].ToString();
            txtCellular.Text = row["cellular"].ToString();
            txtLat.Text = row["Lat"].ToString();
            txtLng.Text = row["Lng"].ToString();
            txtUserTitle.Text = row["Title"].ToString();
            ViewState["Username"] = row["flogin"].ToString();
            var profileImage = row["ProfileImage"].ToString();
            var coverImage = row["CoverImage"].ToString();

            if (!string.IsNullOrWhiteSpace(profileImage))
            {
                imgProfile.ImageUrl = profileImage;
                imgProfile2.ImageUrl = profileImage;
                UpdateImagePathInMasterpage(profileImage);
            }

            if (!string.IsNullOrWhiteSpace(coverImage))
            {
                imgUserBG.ImageUrl = coverImage;
            }

            //Hide controls which are not related to customer
            divEmailAccount.Visible = false;
            //divUserType.Visible = false;
            ddlUserType.SelectedValue = "2";
            divMsg.Visible = false;
            divEmergency.Visible = false;
            divMName.Visible = false;
            divLName.Visible = false;
            lbFName.InnerText = "Customer Name";
            liDashboard.Style["display"] = "none";
        }
    }

    private void LoadUserInfo()
    {

        DataSet ds = new DataSet();
        ds = objBL_User.GetUserInfoByID(objPropUser);
        Session["userprofile"] = ds;

        if (ds.Tables[0].Rows.Count > 0)
        {
            lblUserName.Text = String.Format("{0} {1}", ds.Tables[0].Rows[0]["fFirst"].ToString(), ds.Tables[0].Rows[0]["Last"].ToString());
            ViewState["Username"] = ds.Tables[0].Rows[0]["fUser"].ToString();
            //User Type - Need to ask Anita
            txtFName.Text = ds.Tables[0].Rows[0]["fFirst"].ToString();
            txtLName.Text = ds.Tables[0].Rows[0]["Last"].ToString();
            txtMName.Text = ds.Tables[0].Rows[0]["Middle"].ToString();
            ddlUserType.SelectedValue = ds.Tables[0].Rows[0]["Field"].ToString();
            txtTelePhone.Text = ds.Tables[0].Rows[0]["Phone"].ToString();
            txtEmail.Text = ds.Tables[0].Rows[0]["EMail"].ToString();
            txtCellular.Text = ds.Tables[0].Rows[0]["Cellular"].ToString();
            if (ds.Tables[0].Columns.Contains("pager"))
            {
                txtMsg.Text = ds.Tables[0].Rows[0]["pager"].ToString();
            }
            else
            {
                divMsg.Visible = false;
            }
            txtUserName.Text = ds.Tables[0].Rows[0]["fUser"].ToString();
            txtPassword.Text = ds.Tables[0].Rows[0]["Password"].ToString();
            txtUserTitle.Text = ds.Tables[0].Rows[0]["Title"].ToString(); ;

            ddlState.Text = ds.Tables[0].Rows[0]["State"].ToString();
            txtZip.Text = ds.Tables[0].Rows[0]["Zip"].ToString();
            txtCity.Text = ds.Tables[0].Rows[0]["City"].ToString();
            txtAddress.InnerText = ds.Tables[0].Rows[0]["Address"].ToString();
            ddlCountry.SelectedValue = ds.Tables[0].Rows[0]["Country"].ToString();
            txtEmName.Text = ds.Tables[0].Rows[0]["Contact"].ToString();
            txtEmNum.Text = ds.Tables[0].Rows[0]["Website"].ToString();
            txtLat.Text = ds.Tables[0].Rows[0]["Lat"].ToString();
            txtLng.Text = ds.Tables[0].Rows[0]["Lng"].ToString();
            var profileImage = ds.Tables[0].Rows[0]["ProfileImage"].ToString();
            var coverImage = ds.Tables[0].Rows[0]["CoverImage"].ToString();
            if (!string.IsNullOrWhiteSpace(profileImage))
            {
                imgProfile.ImageUrl = profileImage;
                imgProfile2.ImageUrl = profileImage;
                UpdateImagePathInMasterpage(profileImage);
            }

            if (!string.IsNullOrWhiteSpace(coverImage))
            {
                imgUserBG.ImageUrl = coverImage;
            }

            ViewState["sales"] = ds.Tables[0].Rows[0]["sales"].ToString();

            if (Session["MSM"].ToString() != "TS")
            {
                //if (ds.Tables[0].Rows[0]["sales"].ToString() == "1")
                //{
                //    if (Convert.ToInt16(ds.Tables[0].Rows[0]["emailaccount"]) == 1)
                //    {
                //        chkEmailAccount.Checked = true;
                //        //pnlEmailAccount.Visible = true;
                //        emailTab.Attributes.Add("style", "display:block;");
                //    }
                //}                
                if (Convert.ToInt16(ds.Tables[0].Rows[0]["emailaccount"]) == 1)
                {
                    //liEmail.Visible = true;
                    chkEmailAccount.Checked = true;
                    pnlEmailAccount.Visible = true;
                    //emailTab.Attributes.Add("style", "display:block;");
                    rfvEmail.Enabled = true;
                }
                else
                {
                    //liEmail.Visible = false;
                    chkEmailAccount.Checked = false;
                    rfvEmail.Enabled = false;
                }
            }

            if (ds.Tables.Count > 1 && ds.Tables[1] != null)
            {
                if (ds.Tables[1].Rows.Count > 0)
                {
                    txtInServer.Text = ds.Tables[1].Rows[0]["InServer"].ToString();
                    ViewState["InServerType"] = ds.Tables[1].Rows[0]["InServerType"].ToString();
                    txtInUSername.Text = ds.Tables[1].Rows[0]["InUsername"].ToString();
                    txtInPassword.Text = ds.Tables[1].Rows[0]["InPassword"].ToString();
                    txtinPort.Text = ds.Tables[1].Rows[0]["InPort"].ToString();
                    txtOutServer.Text = ds.Tables[1].Rows[0]["OutServer"].ToString();
                    txtOutUsername.Text = ds.Tables[1].Rows[0]["OutUsername"].ToString();
                    txtOutPassword.Text = ds.Tables[1].Rows[0]["OutPassword"].ToString();
                    txtOutPort.Text = ds.Tables[1].Rows[0]["OutPort"].ToString();
                    chkSSL.Checked = Convert.ToBoolean(ds.Tables[1].Rows[0]["SSL"].ToString());
                    chkTakeASentEmailCopy.Checked = Convert.ToBoolean(ds.Tables[1].Rows[0]["TakeASentEmailCopy"].ToString());
                    txtBccEmail.Text = ds.Tables[1].Rows[0]["BccEmail"].ToString();
                }
            }
            ViewState["userid"] = ds.Tables[0].Rows[0]["userid"].ToString();
            hdnUserId.Value = ds.Tables[0].Rows[0]["userid"].ToString();
            ViewState["rolid"] = ds.Tables[0].Rows[0]["rolid"].ToString();
            ViewState["empid"] = ds.Tables[0].Rows[0]["empid"].ToString();
            ViewState["workid"] = ds.Tables[0].Rows[0]["workid"].ToString();
        }
    }

    private void UpdateImagePathInMasterpage(string imageUrl)
    {
        var imgProfileImage = Page.Master.FindControl("imgProfileImage") as HtmlImage;
        var imgProfileImageLg = Page.Master.FindControl("imgProfileImageLg") as HtmlImage;

        if (imgProfileImage != null)
        {
            imgProfileImage.Src = imageUrl;
        }
        if (imgProfileImageLg != null)
        {
            imgProfileImageLg.Src = imageUrl;
        }

        Session["userProfileImage"] = imageUrl;
    }

    protected void chkEmailAccount_CheckedChanged(object sender, EventArgs e)
    {
        //objPropUser.EmailAccount = chkEmailAccount.Checked? 1: 0;

        //if (chkEmailAccount.Checked == true)
        //    emailTab.Attributes.Add("style", "display:block;");
        //else
        //    emailTab.Attributes.Add("style", "display:none;");

        //revealcard.Attributes.Add("style", "display: block; transform: translateY(-100%);");

        if (chkEmailAccount.Checked == true)
        {
            //liEmail.Visible = true;
            pnlEmailAccount.Visible = true;
            rfvEmail.Enabled = true;

            // ES-33
            // Update incoming email username in case txtEmail was set but incoming email username is empty
            //if(!string.IsNullOrWhiteSpace(txtEmail.Text) && string.IsNullOrWhiteSpace(txtInUSername.Text)) {
            //if (!(bool)ViewState["IsSetEmailAccount"])
            //{
            //    txtInUSername.Text = txtEmail.Text.Trim();
            //}
        }
        else
        {
            pnlEmailAccount.Visible = false;
            rfvEmail.Enabled = false;
            //liEmail.Visible = false;
        }

        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyPress", "Materialize.updateTextFields();", true);

    }

    protected void btnTestIncoming_BK_Click(object sender, EventArgs e)
    {
        try
        {
            using (ImapClient client = new ImapClient())
            {
                if (client.Connect())
                    client.Disconnect();

                try
                {
                    if (client.Connect(txtInServer.Text.Trim(), Convert.ToInt32(txtinPort.Text.Trim()), true, false))
                    {
                        if (client.Login(txtInUSername.Text.Trim(), txtInPassword.Text.Trim()))
                        {
                            //int count = client.Folders.Inbox.Messages.Count();
                            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: 'Connection Successful',  type : 'success', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, theme : 'noty_theme_default',  closable : true});", true);
                            //ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccUp", "alert('Connection Successful');", true);
                            //ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccUp", "alert('" + count.ToString() + " emails found.');", true);
                            client.Disconnect();
                        }
                        else
                        {
                            //ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccUp", "alert('Invalid Credentials');", true);
                            //int count = client.Folders.Inbox.Messages.Count();
                            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: 'Invalid Credentials',  type : 'error', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, theme : 'noty_theme_default',  closable : true});", true);
                        }
                    }
                    else
                    {
                        //ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccUp", "alert('Connection Failed');", true);
                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: 'Connection Failed',  type : 'error', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, theme : 'noty_theme_default',  closable : true});", true);
                    }
                }
                catch (ImapX.Exceptions.ServerAlertException ex)
                {
                    throw ex;
                }
                catch (ImapX.Exceptions.OperationFailedException ex)
                {
                    throw ex;
                }
                catch (ImapX.Exceptions.InvalidStateException ex)
                {
                    throw ex;
                }
            }
            //ScriptManager.RegisterStartupScript(this, Page.GetType(), "Chk", "SelectSameSameIncoming();", true);
            //revealcard.Attributes.Add("style", "display: block; transform: translateY(-100%);");
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyPress", "Materialize.updateTextFields();", true);
        }
        catch (Exception ex)
        {
            //string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(ex.Message);
            //ScriptManager.RegisterStartupScript(this, Page.GetType(), "Chk", "SelectSameSameIncoming();", true);
            //revealcard.Attributes.Add("style", "display: block; transform: translateY(-100%);");
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, theme : 'noty_theme_default',  closable : true});", true);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyPress", "Materialize.updateTextFields();", true);
        }
    }

    protected void btnTestIncoming_Click(object sender, EventArgs e)
    {
        try
        {
            using (var client = new MailKit.Net.Imap.ImapClient())
            {
                if (client.IsConnected)
                    client.Disconnect(true);

                try
                {
                    if (chkSSL.Checked)
                    {
                        try
                        {
                            client.Connect(txtInServer.Text.Trim(), Convert.ToInt32(txtinPort.Text.Trim()), SecureSocketOptions.SslOnConnect);
                        }
                        catch (Exception)
                        {
                        }
                    }
                    if (!client.IsConnected)
                    {
                        try
                        {
                            client.Connect(txtInServer.Text.Trim(), Convert.ToInt32(txtinPort.Text.Trim()), SecureSocketOptions.Auto);
                        }
                        catch
                        {
                            client.Connect(txtInServer.Text.Trim(), Convert.ToInt32(txtinPort.Text.Trim()), SecureSocketOptions.None);
                        }
                    }
                    //if (client.Connect(txtInServer.Text.Trim(), Convert.ToInt32(txtinPort.Text.Trim()), chkSSL.Checked, false))
                    if (client.IsConnected)
                    {
                        try
                        {
                            client.Authenticate(txtInUSername.Text.Trim(), txtInPassword.Text.Trim());
                            //ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccUp", "alert('Connection Successful');", true);
                            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: 'Connection Successful',  type : 'success', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, theme : 'noty_theme_default',  closable : true});", true);
                            client.Disconnect(true);
                        }
                        catch (Exception)
                        {
                            //ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccUp", "alert('Invalid Credentials');", true);
                            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: 'Invalid Credentials',  type : 'error', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, theme : 'noty_theme_default',  closable : true});", true);
                        }
                    }
                    else
                    {
                        //ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccUp", "alert('Connection Failed');", true);
                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: 'Connection Failed',  type : 'error', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, theme : 'noty_theme_default',  closable : true});", true);
                    }
                }
                catch (Exception)
                {
                    //ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccUp", "alert('Connection Failed');", true);
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: 'Connection Failed',  type : 'error', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, theme : 'noty_theme_default',  closable : true});", true);
                }
            }
        }
        catch (Exception ex)
        {
            //string str = ex.Message.Replace("'", "\"").Replace("\r\n", "<br/>");
            string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(ex.Message);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, theme : 'noty_theme_default',  closable : true});", true);
        }
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyPress", "Materialize.updateTextFields();", true);
    }

    protected void chkSame_CheckedChanged(object sender, EventArgs e)
    {
        if (chkSame.Checked == true)
        {
            txtOutUsername.Text = txtInUSername.Text.Trim();
            txtOutPassword.Text = txtInPassword.Text.Trim();
            txtOutPassword.Enabled = false;
            txtOutUsername.Enabled = false;
            RequiredFieldValidator26.Enabled = false;
        }
        else
        {
            txtOutPassword.Enabled = true;
            txtOutUsername.Enabled = true;
            RequiredFieldValidator26.Enabled = true;
        }
        //ScriptManager.RegisterStartupScript(this, Page.GetType(), "Chk", "SelectSameSameIncoming();", true);
        //revealcard.Attributes.Add("style", "display: block; transform: translateY(-100%);");
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyPress", "Materialize.updateTextFields();", true);
    }

    protected void btnTestOut_BK_Click(object sender, EventArgs e)
    {
        Mail mail = new Mail();
        try
        {
            mail.Username = txtOutUsername.Text.Trim();
            mail.Password = txtOutPassword.Text.Trim();
            mail.SMTPHost = txtOutServer.Text.Trim();
            mail.SMTPPort = Convert.ToInt32(txtOutPort.Text.Trim());

            mail.InUsername = txtInUSername.Text.Trim();
            mail.InPassword = txtInPassword.Text.Trim();
            mail.InHost = txtInServer.Text.Trim();
            mail.InPort = string.IsNullOrEmpty(txtinPort.Text) ? 0 : int.Parse(txtinPort.Text);

            var emailValidation = new System.ComponentModel.DataAnnotations.EmailAddressAttribute();
            if (emailValidation.IsValid(txtOutUsername.Text.Trim()))
            {
                mail.From = txtOutUsername.Text.Trim();
                mail.To = txtOutUsername.Text.Split(';', ',').OfType<string>().ToList();
            }
            else
            {
                mail.From = txtEmail.Text.Trim();
                mail.To = txtEmail.Text.Split(';', ',').OfType<string>().ToList();
            }

            mail.Bcc = txtBccEmail.Text.Split(';', ',').OfType<string>().ToList();
            mail.Title = "Test Email";
            mail.Text = "Test Email from Mobile Office Manager.";
            mail.RequireAutentication = true;
            mail.TakeASentEmailCopy = chkTakeASentEmailCopy.Checked;
            mail.Send();
            //ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccUp", "alert('Mail sent successfully.');", true);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: 'Email sent successfully',  type : 'success', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, theme : 'noty_theme_default',  closable : true});", true);
            //ScriptManager.RegisterStartupScript(this, Page.GetType(), "Chk", "SelectSameSameIncoming();", true);
            //revealcard.Attributes.Add("style", "display: block; transform: translateY(-100%);");
        }
        catch (Exception ex)
        {
            //string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(ex.Message);
            //ScriptManager.RegisterStartupScript(this, Page.GetType(), "Chk", "SelectSameSameIncoming();", true);
            //revealcard.Attributes.Add("style", "display: block; transform: translateY(-100%);");
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, theme : 'noty_theme_default',  closable : true});", true);
        }

        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyPress", "Materialize.updateTextFields();", true);
    }

    protected void btnTestOut_Click(object sender, EventArgs e)
    {
        Mail mail = new Mail();
        try
        {
            mail.Username = txtOutUsername.Text.Trim();
            mail.Password = txtOutPassword.Text.Trim();
            mail.SMTPHost = txtOutServer.Text.Trim();
            mail.SMTPPort = Convert.ToInt32(txtOutPort.Text.Trim());

            mail.InUsername = txtInUSername.Text.Trim();
            mail.InPassword = txtInPassword.Text.Trim();
            mail.InHost = txtInServer.Text.Trim();
            mail.InPort = string.IsNullOrEmpty(txtinPort.Text) ? 0 : int.Parse(txtinPort.Text);

            var emailValidation = new System.ComponentModel.DataAnnotations.EmailAddressAttribute();
            if (emailValidation.IsValid(txtOutUsername.Text.Trim()))
            {
                mail.From = txtOutUsername.Text.Trim();
                mail.To = txtOutUsername.Text.Split(';', ',').OfType<string>().ToList();
            }
            else
            {
                mail.From = txtEmail.Text.Trim();
                mail.To = txtEmail.Text.Split(';', ',').OfType<string>().ToList();
            }

            mail.Bcc = txtBccEmail.Text.Split(';', ',').OfType<string>().ToList();
            mail.Title = "Test Email";
            mail.Text = "Test Email from Mobile Office Manager.";
            mail.RequireAutentication = true;
            mail.TakeASentEmailCopy = chkTakeASentEmailCopy.Checked;
            mail.SSL = chkSSL.Checked;
            mail.SendTest();

            try
            {
                // Emailing logs: testing only
                EmailLog emailLog = new EmailLog()
                {
                    ConnConfig = Session["config"].ToString(),
                    SessionNo = Guid.NewGuid().ToString(),
                    EmailDate = DateTime.Now,
                    From = mail.From,
                    Ref = Session["userid"] != null ? Convert.ToInt32(Session["userid"].ToString()) : 0,
                    Screen = "AddUser",
                    Sender = mail.From,
                    To = String.Join(", ", mail.To.ToArray()),
                    Status = 1,
                    SysErrMessage = string.Empty,
                    Username = Session["Username"] != null ? Session["Username"].ToString() : "",
                    UsrErrMessage = string.Empty,
                    Function = "TestOut"
                };
                BL_EmailLog bL_EmailLog = new BL_EmailLog();
                bL_EmailLog.AddEmailLog(emailLog);
            }
            catch (Exception)
            {
            }

            //ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccUp", "alert('Email sent successfully.');", true);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: 'Email sent successfully',  type : 'success', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, theme : 'noty_theme_default',  closable : true});", true);

        }
        catch (Exception ex)
        {
            //string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            //string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(ex.Message);
            // Emailing logs: testing only
            try
            {
                EmailLog emailLog = new EmailLog()
                {
                    ConnConfig = Session["config"].ToString(),
                    SessionNo = Guid.NewGuid().ToString(),
                    EmailDate = DateTime.Now,
                    From = mail.From,
                    Ref = Session["userid"] != null ? Convert.ToInt32(Session["userid"].ToString()) : 0,
                    Screen = "AddUser",
                    Sender = mail.From,
                    To = String.Join(", ", mail.To.ToArray()),
                    Status = 0,
                    SysErrMessage = ex.Message,
                    Username = Session["Username"] != null ? Session["Username"].ToString() : "",
                    UsrErrMessage = ex.Message,
                    Function = "TestOut"
                };
                BL_EmailLog bL_EmailLog = new BL_EmailLog();
                bL_EmailLog.AddEmailLog(emailLog);
            }
            catch (Exception)
            {
            }

            //ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, theme : 'noty_theme_default',  closable : true});", true);
            //ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, theme : 'noty_theme_default',  closable : true});", true);
            WebBaseUtility.ShowEmailErrorMessageBox(this, Page.GetType(), ex);
        }
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyPress", "Materialize.updateTextFields();", true);
    }

    protected void btnUpload_Click(object sender, EventArgs e)
    {
        if (FileUpload1.HasFile)
        {
            System.Drawing.Image imgfile = System.Drawing.Image.FromStream(FileUpload1.PostedFile.InputStream);
            Session["userProfile"] = null;
            Session["userProfile"] = ResizeImage(imgfile, 225, 225);
            string img = "data:image/png;base64," + Convert.ToBase64String(ResizeImage(imgfile, 225, 225));
            imgProfile.ImageUrl = img;
            imgProfile2.ImageUrl = img;
            try
            {
                // Need to update the upload file to database 
                UpdateUserAvatar();
                UpdateImagePathInMasterpage(img);
            }
            catch (Exception ex)
            {
                string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, theme : 'noty_theme_default',  closable : true});", true);
            }

            revealcard.Attributes.Add("style", "display: none;");
        }
    }
    private byte[] ResizeImage(System.Drawing.Image stImage, int width, int height)
    {
        byte[] bmpBytes = null;
        if (stImage != null)
        {
            // Create a bitmap of the content of the fileUpload control in memory
            Bitmap originalBMP = new Bitmap(stImage);
            double sngRatioraw = 0;
            int sngRatio = 0;
            int newWidth = 0;
            int newHeight = 0;
            // Calculate the new image dimensions
            int origWidth = originalBMP.Width;
            int origHeight = originalBMP.Height;
            if (origWidth > origHeight)
            {
                sngRatioraw = Convert.ToDouble(origWidth) / Convert.ToDouble(origHeight);
                newWidth = width;
                sngRatio = Convert.ToInt32(Math.Round(sngRatioraw));
                newHeight = newWidth / sngRatio;
            }
            else
            {
                sngRatioraw = Convert.ToDouble(origHeight) / Convert.ToDouble(origWidth);
                newHeight = height;
                sngRatio = Convert.ToInt32(Math.Round(sngRatioraw));
                newWidth = newHeight / sngRatio;
            }

            // Create a new bitmap which will hold the previous resized bitmap
            Bitmap newBMP = new Bitmap(originalBMP, newWidth, newHeight);

            // Create a graphic based on the new bitmap
            Graphics oGraphics = Graphics.FromImage(newBMP);
            // Set the properties for the new graphic file
            oGraphics.SmoothingMode = SmoothingMode.AntiAlias; oGraphics.InterpolationMode = InterpolationMode.HighQualityBicubic;

            // Draw the new graphic based on the resized bitmap
            oGraphics.DrawImage(originalBMP, 0, 0, newWidth, newHeight);



            bmpBytes = BmpToBytes_MemStream(newBMP);

            // Once finished with the bitmap objects, we deallocate them.
            originalBMP.Dispose();
            newBMP.Dispose();
            oGraphics.Dispose();
        }
        return bmpBytes;
    }
    private byte[] BmpToBytes_MemStream(Bitmap bmp)
    {
        MemoryStream ms = new MemoryStream();
        // Save to memory using the Jpeg format
        bmp.Save(ms, ImageFormat.Png);

        // read to end
        byte[] bmpBytes = ms.GetBuffer();
        bmp.Dispose();
        ms.Close();

        return bmpBytes;
    }

    protected void btnUploadCoverPic_Click(object sender, EventArgs e)
    {
        if (FileUpload2.HasFile)
        {
            System.Drawing.Image imgfile = System.Drawing.Image.FromStream(FileUpload2.PostedFile.InputStream);
            Session["userbg"] = null;
            Session["userbg"] = ResizeImage(imgfile, 1500, 500);
            string img = "data:image/png;base64," + Convert.ToBase64String(ResizeImage(imgfile, 1000, 500));
            imgUserBG.ImageUrl = img;
            try
            {
                // Need to update database here
                UpdateUserCoverImage();
            }
            catch (Exception ex)
            {
                //string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
                string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(ex.Message);
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, theme : 'noty_theme_default',  closable : true});", true);
            }

            revealcard.Attributes.Add("style", "display: none;");
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(ddlCountry.SelectedValue))
        {
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + "Please select the country" + "',  type : 'error', dismissQueue: true, layout:'topCenter',closeOnSelfClick:true,timeout: 3000, theme : 'noty_theme_default',  closable : true});", true);
            return;
        }
        var usertypeid = (int)ViewState["usertypeid"];

        try
        {
            objPropUser.UserID = Convert.ToInt32(ViewState["userid"]);
            objPropUser.ConnConfig = Session["config"].ToString();
            objPropUser.DBName = Session["dbname"].ToString();
            objPropUser.Username = ViewState["Username"].ToString();
            objPropUser.Field = int.Parse(ddlUserType.SelectedValue);
            objPropUser.FirstName = txtFName.Text;
            objPropUser.LastNAme = txtLName.Text;
            objPropUser.MiddleName = txtMName.Text;
            objPropUser.Tele = txtTelePhone.Text;
            objPropUser.Email = txtEmail.Text;
            objPropUser.Cell = txtCellular.Text;
            objPropUser.Pager = txtMsg.Text;
            objPropUser.Password = txtPassword.Text;
            objPropUser.Title = txtUserTitle.Text;
            objPropUser.State = ddlState.Text;
            objPropUser.Zip = txtZip.Text;
            objPropUser.City = txtCity.Text;
            objPropUser.Address = txtAddress.InnerText;
            objPropUser.Country = ddlCountry.SelectedValue;
            objPropUser.EmName = txtEmName.Text;
            objPropUser.EmNum = txtEmNum.Text;
            objPropUser.Lat = txtLat.Text;
            objPropUser.Lng = txtLng.Text;
            objPropUser.ProfileImage = imgProfile.ImageUrl;
            objPropUser.CoverImage = imgUserBG.ImageUrl;

            //Check if it's customer
            if (usertypeid == 2)
            {
                UpdateCustomerInfo();
            }
            else
            {
                UpdateUserInfo();
            }
            LoadUserProfile();
            var str = "Profile updated successfully.";
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'success', dismissQueue: true, layout:'topCenter',closeOnSelfClick:true,timeout: 3000, theme : 'noty_theme_default',  closable : true});", true);
        }
        catch (Exception ex)
        {
            var err = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(ex.Message);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + err + "',  type : 'error', dismissQueue: true, layout:'topCenter',closeOnSelfClick:true,timeout: 3000, theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    private void UpdateCustomerInfo()
    {
        objBL_User.UpdateUserCustomerProfile(objPropUser);
    }

    private void UpdateUserInfo()
    {
        objPropUser.Salesperson = int.Parse((string)ViewState["sales"]);
        objPropUser.EmailAccount = chkEmailAccount.Checked ? 1 : 0;

        if (objPropUser.EmailAccount == 1)
        {

            objPropUser.InServer = txtInServer.Text;
            objPropUser.InServerType = (string)ViewState["InServerType"];
            objPropUser.InUsername = txtInUSername.Text;
            objPropUser.InPassword = txtInPassword.Text;
            objPropUser.InPort = string.IsNullOrEmpty(txtinPort.Text) ? 0 : int.Parse(txtinPort.Text);
            objPropUser.OutServer = txtOutServer.Text;
            if (chkSame.Checked)
            {
                objPropUser.OutUsername = txtInUSername.Text;
                objPropUser.OutPassword = txtInPassword.Text;
            }
            else
            {
                objPropUser.OutUsername = txtOutUsername.Text;
                objPropUser.OutPassword = txtOutPassword.Text;
            }
            objPropUser.OutPort = int.Parse(txtOutPort.Text);
            objPropUser.SSL = chkSSL.Checked;
            objPropUser.TakeASentEmailCopy = chkTakeASentEmailCopy.Checked;
            objPropUser.BccEmail = txtBccEmail.Text;
        }

        objBL_User.UpdateUserProfile(objPropUser);
    }

    protected void txtInPassword_PreRender(object sender, EventArgs e)
    {
        txtInPassword.Attributes["value"] = txtInPassword.Text;
    }

    protected void txtOutPassword_PreRender(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(txtOutPassword.Text))
            txtOutPassword.Attributes["value"] = txtOutPassword.Text;
    }

    protected void lnkAddDashboard_Click(object sender, EventArgs e)
    {
        _editState = false;
        txtDashboardName.Text = string.Empty;
        chkDefault.Checked = false;
        gvKPIs.MasterTableView.ClearChildSelectedItems();

        string script = "function f(){$find(\"" + AddDashboardWindow.ClientID + "\").show(); $find(\"" + AddDashboardWindow.ClientID + "\").set_title('Add Dashboard');Sys.Application.remove_load(f);}Sys.Application.add_load(f);";
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), AddDashboardWindow.ClientID, script, true);
    }

    protected void lnkEditDashboard_Click(object sender, EventArgs e)
    {
        _editState = true;
        var userId = Session["userid"];
        objPropUser.UserID = Convert.ToInt32(userId);
        objPropUser.ConnConfig = Session["config"].ToString();

        if (gvListDashboard.SelectedItems.Count > 0)
        {
            var item = (GridDataItem)gvListDashboard.SelectedItems[0];
            _dashBoardID = Convert.ToInt32(item.GetDataKeyValue("ID").ToString());

            var ds = objBL_User.GetDashboardByID(objPropUser, _dashBoardID);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                txtDashboardName.Text = ds.Tables[0].Rows[0]["Name"].ToString();
                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["IsDefault"].ToString()) && Convert.ToBoolean(ds.Tables[0].Rows[0]["IsDefault"].ToString()))
                {
                    chkDefault.Checked = true;
                }
                else
                {
                    chkDefault.Checked = false;
                }

                LoadUserDash(_dashBoardID);
            }

            string script = "function f(){$find(\"" + AddDashboardWindow.ClientID + "\").show(); $find(\"" + AddDashboardWindow.ClientID + "\").set_title('Edit Dashboard');Sys.Application.remove_load(f);}Sys.Application.add_load(f);";
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), AddDashboardWindow.ClientID, script, true);
            txtDashboardName.Focus();
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarningEditDashboard", "noty({text: 'Please select any one to edit.', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
    }

    protected void lnkDelDashboard_Click(object sender, EventArgs e)
    {
        try
        {
            objPropUser.ConnConfig = Session["config"].ToString();
            foreach (GridDataItem item in gvListDashboard.MasterTableView.Items)
            {
                GridDataItem dataitem = (GridDataItem)item;
                if (dataitem.Selected)
                {
                    var dashboardID = Convert.ToInt32(item.GetDataKeyValue("ID").ToString());
                    objBL_User.DeleteDashboard(objPropUser, dashboardID);
                }
            }

            LoadListDashboard();
        }
        catch (Exception ex)
        {
            string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(ex.Message);//ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyAddCattype", "noty({text: '" + str + "', dismissQueue: true,  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
    }

    protected void gvKPIs_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        objPropUser.ConnConfig = Session["config"].ToString();

        var ds = objBL_User.GetListKPIs(objPropUser);
        var data = ds.Tables[0];

        var financeStatement = (bool)Session["FinanceStatement"];
        if (!financeStatement)
        {
            var removeItems = data.Select("Module = 'Statements'");
            foreach (var removeItem in removeItems)
            {
                data.Rows.Remove(removeItem);
            }
        }

        gvKPIs.DataSource = ds.Tables[0];
    }

    protected void lnkDashBoardSave_Click(object sender, EventArgs e)
    {
        try
        {
            var userId = Session["userid"];
            objPropUser.UserID = Convert.ToInt32(userId);
            objPropUser.ConnConfig = Session["config"].ToString();

            // Unset all default if checked
            if (chkDefault.Checked)
            {
                objBL_User.UnsetDashboardDefault(objPropUser);
            }

            if (_editState)
            {
                // Update dashboard name
                objBL_User.UpdateDashboard(objPropUser, _dashBoardID, txtDashboardName.Text, chkDefault.Checked);
                objBL_User.DeleteUserDash(objPropUser, _dashBoardID);

                UserDash userDash = new UserDash();
                userDash.UserID = objPropUser.UserID;
                userDash.Dashboard = _dashBoardID;

                foreach (GridDataItem item in gvKPIs.MasterTableView.Items)
                {
                    GridDataItem dataitem = (GridDataItem)item;
                    if (dataitem.Selected)
                    {
                        userDash.KPIID = Convert.ToInt32(item.GetDataKeyValue("ID").ToString());
                        objBL_User.AddUserDashboard(objPropUser, userDash);
                    }
                }

                ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: 'Dashboard updated successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
            }
            else
            {
                var dashboardId = objBL_User.AddDashboard(objPropUser, txtDashboardName.Text, chkDefault.Checked);

                UserDash userDash = new UserDash();
                userDash.UserID = objPropUser.UserID;
                userDash.Dashboard = dashboardId;

                foreach (GridDataItem item in gvKPIs.MasterTableView.Items)
                {
                    GridDataItem dataitem = (GridDataItem)item;
                    if (dataitem.Selected)
                    {
                        userDash.KPIID = Convert.ToInt32(item.GetDataKeyValue("ID").ToString());
                        objBL_User.AddUserDashboard(objPropUser, userDash);
                    }
                }

                ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: 'Dashboard added successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
            }

            LoadListDashboard();
        }
        catch (Exception ex)
        {
            //string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(ex.Message);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyAddCattype", "noty({text: '" + str + "', dismissQueue: true,  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
    }

    private void LoadListDashboard()
    {
        var userId = Session["userid"];
        objPropUser.UserID = Convert.ToInt32(userId);
        objPropUser.ConnConfig = Session["config"].ToString();
        var ds = objBL_User.GetListDashboard(objPropUser);
        var data = ds.Tables[0];

        gvListDashboard.DataSource = ds.Tables[0];
        gvListDashboard.DataBind();
    }

    private void LoadUserDash(int dashboardId)
    {
        objPropUser.ConnConfig = Session["config"].ToString();

        var ds = objBL_User.GetListKPIs(objPropUser);
        var data = ds.Tables[0];

        gvKPIs.DataSource = ds.Tables[0];
        gvKPIs.DataBind();

        var dsUserDash = objBL_User.GetListDashKPI(objPropUser, dashboardId);
        var dataUserDash = dsUserDash.Tables[0];

        //gvKPIs.MasterTableView.ClearChildSelectedItems();

        foreach (GridDataItem item in gvKPIs.MasterTableView.Items)
        {
            GridDataItem dataitem = (GridDataItem)item;

            var kpiID = Convert.ToInt32(item.GetDataKeyValue("ID").ToString());
            var dataRow = dataUserDash.Select(string.Format("ID = {0}", kpiID));
            if (dataRow != null && dataRow.Count() > 0)
            {
                dataitem.Selected = true;
            }
        }
    }

    private void UpdateUserAvatar()
    {
        objPropUser.UserID = Convert.ToInt32(ViewState["userid"]);
        objPropUser.ConnConfig = Session["config"].ToString();
        objPropUser.DBName = Session["dbname"].ToString();
        objPropUser.Username = ViewState["Username"].ToString();
        objPropUser.ProfileImage = imgProfile.ImageUrl;
        var usertypeid = (int)ViewState["usertypeid"];
        objPropUser.Field = usertypeid;
        objBL_User.UpdateUserAvatar(objPropUser);
    }

    private void UpdateUserCoverImage()
    {
        objPropUser.UserID = Convert.ToInt32(ViewState["userid"]);
        objPropUser.ConnConfig = Session["config"].ToString();
        objPropUser.DBName = Session["dbname"].ToString();
        objPropUser.Username = ViewState["Username"].ToString();
        objPropUser.CoverImage = imgUserBG.ImageUrl;
        var usertypeid = (int)ViewState["usertypeid"];
        objPropUser.Field = usertypeid;
        objBL_User.UpdateUserCoverImage(objPropUser);
    }

    protected void RadGrid_EmailSigns_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        if (Session["userid"] != null)
        {
            objPropUser.ID = Convert.ToInt32(Session["userid"]);
            objPropUser.ConnConfig = Session["config"].ToString();
            var ds = objBL_User.GetUserEmailSignature(objPropUser);
            if (ds.Tables.Count > 0)
            {
                RadGrid_EmailSigns.DataSource = ds.Tables[0];
            }
        }
    }

    protected void lnkRefreshScreen_Click(object sender, EventArgs e)
    {
        RadGrid_EmailSigns.Rebind();
        if(Session["UP_EmailSignSuccMess"]!=null && Session["UP_EmailSignSuccMess"].ToString() != "")
        {
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "noty({text: '"+ Session["UP_EmailSignSuccMess"].ToString() + "',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
            Session["UP_EmailSignSuccMess"] = null;
        }
        
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyPress", "Materialize.updateTextFields();", true);
    }

    protected void btnDeleteEmailSign_Click(object sender, EventArgs e)
    {
        try
        {
            EmailSignature eSignature = new EmailSignature();
            eSignature.ConnConfig = Session["Config"].ToString();

            foreach (GridDataItem item in RadGrid_EmailSigns.SelectedItems)
            {
                HiddenField hdnSignId = (HiddenField)item.FindControl("hdnSignId");

                eSignature.Id = Convert.ToInt32(hdnSignId.Value);
            }

            BL_User objBL_User = new BL_User();
            objBL_User.DeleteEmailSignature(eSignature);
            RadGrid_EmailSigns.Rebind();

            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyDelSucc", "noty({text: 'Deleted successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false,dismissQueue: true});", true);
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true,dismissQueue: true});", true);
        }


    }
}