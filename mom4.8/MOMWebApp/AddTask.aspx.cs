using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessEntity;
using BusinessLayer;
using System.Data;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;
using System.Net.Mail;
using System.Text;
using System.Web.Script.Serialization;
using System.Collections.Generic;
using System.Web;

public partial class AddTask : System.Web.UI.Page
{
    User objPropUser = new User();
    BL_User objBL_User = new BL_User();

    Customer objProp_Customer = new Customer();
    BL_Customer objBL_Customer = new BL_Customer();

    BL_General objBL_General = new BL_General();
    General objGeneral = new General();

    GeneralFunctions objGeneralFunctions = new GeneralFunctions();
    private const string ASCENDING = " ASC";
    private const string DESCENDING = " DESC";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userid"] == null)
        {
            Response.Redirect("login.aspx");
        }
        if (!IsPostBack)
        {
            ViewState["edit"] = 0;
            FillUsers();
            FillTaskCategory();
            //ddlAssigned.SelectedValue = Session["username"].ToString();

            if (Request.QueryString["uid"] != null)
            {
                Page.Title = "Edit Task || MOM";
                lnkNewEmail.Visible = true;
                lnkNewEmail.NavigateUrl = "email.aspx";
                GetTask();
                lnkNewEmail.NavigateUrl = "email.aspx?rol=" + hdnId.Value;
                if (Request.QueryString["fl"] != null)
                {
                    ViewState["edit"] = 0;
                    lblHeader.Text = "Add Follow-Up Task";
                    if (Request.QueryString["fl"].ToString() == "2")
                    {
                        string strScript = "noty({text: 'Please create the follow-up task now.', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, theme : 'noty_theme_default',  closable : false});";
                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyload", strScript, true);
                    }
                }
                else
                {
                    ViewState["edit"] = 1;
                    lblHeader.Text = "Edit Task";
                    chkFollowUp.Visible = true;
                }
                //HyperLink1.NavigateUrl = "AddTask.aspx?rol=" + hdnId.Value + "&name=" + txtName.Text + "&assignedTo=" + ddlAssigned.SelectedItem.Text + "&customer=" + txtCustomer.Text;
                //HyperLink2.NavigateUrl = "AddTask.aspx?rol=" + hdnId.Value + "&name=" + txtName.Text + "&assignedTo=" + ddlAssigned.SelectedItem.Text + "&customer=" + txtCustomer.Text;
                HyperLink1.NavigateUrl = "AddTask.aspx?rol=" + hdnId.Value + "&name=" + txtName.Text + "&assignedTo=" + ddlAssigned.SelectedValue + "&customer=" + txtCustomer.Text + "&screen=Task&ref=" + Request.QueryString["uid"];
                HyperLink2.NavigateUrl = "AddTask.aspx?rol=" + hdnId.Value + "&name=" + txtName.Text + "&assignedTo=" + ddlAssigned.SelectedValue + "&customer=" + txtCustomer.Text +"&screen=Task&ref=" + Request.QueryString["uid"];
            }
            if (ViewState["edit"].ToString() == "0")
            {
                liSystemInfo.Style["display"] = "none";
                adSystemInfo.Style["display"] = "none";
                //pnlSysInfo.Visible = false;
                //menuLeads.Visible = false;
            }

            if (Request.QueryString["rol"] != null)
            {
                hdnId.Value = Request.QueryString["rol"].ToString();
                txtName.Text = Request.QueryString["name"].ToString();
                FillTasks(hdnId.Value);
                //FillContact(Convert.ToInt32(hdnId.Value));
                //BindEmails(GetMailsfromdb(-1, string.Empty));
                HyperLink1.NavigateUrl = "AddTask.aspx?rol=" + hdnId.Value + "&name=" + txtName.Text;
                HyperLink2.NavigateUrl = "AddTask.aspx?rol=" + hdnId.Value + "&name=" + txtName.Text;
                lnkNewEmail.NavigateUrl = "email.aspx?rol=" + hdnId.Value;


            }
            if (Request.QueryString["assignedTo"] != null)
            {
                ListItem LI = ddlAssigned.Items.FindByValue(Request.QueryString["assignedTo"].ToString());
                if (LI != null)
                {
                    ddlAssigned.SelectedValue = Request.QueryString["assignedTo"].ToString();
                    RequiredFieldValidator43.Enabled = false;
                }
                else
                {
                    LI = ddlAssigned.Items.FindByText(Request.QueryString["assignedTo"].ToString());
                    if(LI != null)
                    {
                        //ddlAssigned.SelectedItem.Text = Request.QueryString["assignedTo"].ToString();
                        ddlAssigned.SelectedValue = LI.Value;
                        RequiredFieldValidator43.Enabled = false;
                    }
                }

                //ListItem LI = ddlAssigned.Items.FindByText(Request.QueryString["assignedTo"].ToString());
                //if (LI != null)
                //{
                //    ddlAssigned.SelectedItem.Text = Request.QueryString["assignedTo"].ToString();
                //    RequiredFieldValidator43.Enabled = false;
                //}


            }

            if (Request.QueryString["customer"] != null)
            {
                txtCustomer.Text = Request.QueryString["customer"].ToString();
            }
            //if (Request.QueryString["leadId"] != null)
            //{
            //    ViewState["backToLead"] = Request.QueryString["leadId"];
            //}
            if(!string.IsNullOrEmpty(Request.QueryString["screen"]) && Request.QueryString["screen"].ToLower() == "lead" && !string.IsNullOrEmpty(Request.QueryString["ref"]))
            {
                ViewState["backToLead"] = Request.QueryString["ref"];
            }
            if (!string.IsNullOrEmpty(hdnId.Value) && hdnId.Value != "0")
            {
                lnkAddnewContact.Visible = true;
                btnEditContact.Visible = true;
                btnDeleteContact.Visible = true;
            }
            else
            {
                lnkAddnewContact.Visible = false;
                btnEditContact.Visible = false;
                btnDeleteContact.Visible = false;
            }

            if (Session["AddEditTaskSuccMess"] != null && Session["AddEditTaskSuccMess"].ToString() != "")
            {
                string strScript = string.Empty;

                strScript += "noty({text: 'Task " + Session["AddEditTaskSuccMess"].ToString() + " Successfully!', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});";
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccUp", strScript, true);
                Session["AddEditTaskSuccMess"] = null;
            }
        }
        //Permission();
        UserPermission();
        CompanyPermission();
        HighlightSideMenu();
        pnlNext.Visible = false;
        if (Request.QueryString["uid"] != null)
        {
            liLogs.Style["display"] = "inline-block";
            tbLogs.Style["display"] = "block";
            pnlNext.Visible = true;
        }
    }


    private void HighlightSideMenu()
    {
        HyperLink aNav = (HyperLink)Page.Master.FindControl("SalesMgr");
        //li.Style.Add("background", "url(images/active_menu_bg.png) repeat-x");
        aNav.CssClass = "active collapsible-header waves-effect waves-cyan collapsible-height-nl";

        //HyperLink a = (HyperLink)Page.Master.Master.FindControl("SalesLink");
        //a.Style.Add("color", "#2382b2");

        HyperLink lnkUsersSmenu = (HyperLink)Page.Master.FindControl("lnkTasks");
        //lnkUsersSmenu.Style.Add("color", "#FF7A0A");
        lnkUsersSmenu.Style.Add("color", "#316b9d");
        lnkUsersSmenu.Style.Add("font-weight", "normal");
        lnkUsersSmenu.Style.Add("background-color", "#e5e5e5");
        //AjaxControlToolkit.HoverMenuExtender hm = (AjaxControlToolkit.HoverMenuExtender)Page.Master.Master.FindControl("HoverMenuExtenderSales");
        //hm.Enabled = false;
        HtmlGenericControl div = (HtmlGenericControl)Page.Master.FindControl("SalesMgrSub");
        div.Style.Add("display", "block");
    }

    private void Permission()
    {
        HtmlGenericControl li = (HtmlGenericControl)Page.Master.Master.FindControl("SalesMgr");
        //li.Style.Add("background", "url(images/active_menu_bg.png) repeat-x");
        li.Attributes.Add("class", "start active open");

        HyperLink a = (HyperLink)Page.Master.Master.FindControl("SalesLink");
        //a.Style.Add("color", "#2382b2");

        HyperLink lnkUsersSmenu = (HyperLink)Page.Master.Master.FindControl("lnkTasks");
        //lnkUsersSmenu.Style.Add("color", "#FF7A0A");
        lnkUsersSmenu.Style.Add("color", "#316b9d");
        lnkUsersSmenu.Style.Add("font-weight", "normal");
        lnkUsersSmenu.Style.Add("background-color", "#e5e5e5");

        //AjaxControlToolkit.HoverMenuExtender hm = (AjaxControlToolkit.HoverMenuExtender)Page.Master.Master.FindControl("HoverMenuExtenderSales");
        //hm.Enabled = false;
        HtmlGenericControl ul = (HtmlGenericControl)Page.Master.Master.FindControl("SalesMgrSub");
        //ul.Attributes.Remove("class");
        //ul.Style.Add("display", "block");

        if (Session["type"].ToString() == "c")
        {
            Response.Redirect("home.aspx");
        }

        //if (Convert.ToInt32(Session["ISsupervisor"]) == 1)
        //{
        //    Response.Redirect("home.aspx");
        //}

        if (Session["type"].ToString() != "am")
        {
            DataTable dt = new DataTable();
            dt = (DataTable)Session["userinfo"];
            string Sales = dt.Rows[0]["sales"].ToString().Substring(0, 1);

            if (Sales == "N")
            {
                Response.Redirect("home.aspx");
            }

            //Contact.....................>
            string ContactPermission = dt.Rows[0]["ContactPermission"] == DBNull.Value ? "YYYY" : dt.Rows[0]["ContactPermission"].ToString();
            hdnAddeContact.Value = ContactPermission.Length < 1 ? "Y" : ContactPermission.Substring(0, 1);
            hdnEditeContact.Value = ContactPermission.Length < 2 ? "Y" : ContactPermission.Substring(1, 1);
            hdnDeleteContact.Value = ContactPermission.Length < 3 ? "Y" : ContactPermission.Substring(2, 1);
            hdnViewContact.Value = ContactPermission.Length < 4 ? "Y" : ContactPermission.Substring(3, 1);
        }

    }
    private void UserPermission()
    {


          // User Permission 
        if (Session["type"].ToString() != "am" && Session["type"].ToString() != "c")
        {
            DataTable ds = new DataTable();
            ds = GetUserById();



            string SalesManagermodulePermission = ds.Rows[0]["SalesManager"] == DBNull.Value ? "Y" : ds.Rows[0]["SalesManager"].ToString();

            if (SalesManagermodulePermission == "N")
            {
                Response.Redirect("Home.aspx?permission=no"); return;
            }



            int taskPermission = ds.Rows[0]["ToDo"] == DBNull.Value ? 0 : Convert.ToInt32(ds.Rows[0]["ToDo"]);

            if (taskPermission == 0)
            {
                Response.Redirect("Home.aspx?permission=no"); return;
                //lnkAdd.Visible = false;
                //lnkEdit.Visible = false;
                //lnkDelete.Visible = false;
            }

            int CompleteTaskPermission = ds.Rows[0]["ToDo"] == DBNull.Value ? 0 : Convert.ToInt32(ds.Rows[0]["ToDoC"]);

            ViewState["CompleteTaskPermission"] = CompleteTaskPermission;

        }
    }
    private DataTable GetUserById()
    {
        User objPropUser = new User();
        objPropUser.TypeID = Convert.ToInt32(Session["usertypeid"]);
        objPropUser.UserID = Convert.ToInt32(Session["userid"]);
        objPropUser.ConnConfig = Session["config"].ToString();
        objPropUser.DBName = Session["dbname"].ToString();
        DataSet ds = new DataSet();
        ds = objBL_User.GetUserPermissionByUserID(objPropUser);
        return ds.Tables[0];
    }



    private void FillTasks(string name)
    {
        DataSet ds = new DataSet();
        objProp_Customer.ConnConfig = Session["config"].ToString();
        objProp_Customer.SearchBy = "t.rol";
        objProp_Customer.SearchValue = name;
        objProp_Customer.StartDate = string.Empty;
        objProp_Customer.EndDate = string.Empty;

        objProp_Customer.Mode = 1;
        if (!string.IsNullOrEmpty(Request.QueryString["screen"]) && !string.IsNullOrEmpty(Request.QueryString["ref"]))
        {
            objProp_Customer.Screen = Request.QueryString["screen"].ToString();
            objProp_Customer.Ref = Convert.ToInt32(Request.QueryString["ref"].ToString());
        }
        else
        {
            objProp_Customer.Screen = "Sales";
            objProp_Customer.Ref = 0;
        }
        
        ds = objBL_Customer.getTasks(objProp_Customer);

        RadGrid_OpenTasks.VirtualItemCount = ds.Tables[0].Rows.Count;
        RadGrid_OpenTasks.DataSource = ds.Tables[0];
        RadGrid_OpenTasks.DataBind();
        //gvTasksOpen.DataSource = ds.Tables[0];
        //gvTasksOpen.DataBind();
        //menuLeads.Items[0].Text = "Open Tasks(" + ds.Tables[0].Rows.Count + ")";

        objProp_Customer.Mode = 0;
        //objProp_Customer.Screen = "Sales";
        //objProp_Customer.Ref = 0;

        ds = objBL_Customer.getTasks(objProp_Customer);
        RadGrid_TaskHistory.VirtualItemCount = ds.Tables[0].Rows.Count;
        RadGrid_TaskHistory.DataSource = ds.Tables[0];
        RadGrid_TaskHistory.DataBind();

        //gvTasks.DataSource = ds.Tables[0];
        //gvTasks.DataBind();
        //menuLeads.Items[1].Text = "Task History(" + ds.Tables[0].Rows.Count + ")";
    }
    private void GetTask()
    {
        objProp_Customer.ConnConfig = Session["config"].ToString();
        objProp_Customer.TemplateID = Convert.ToInt32(Request.QueryString["uid"].ToString());
        DataSet ds = new DataSet();
        ds = objBL_Customer.getTasksByID(objProp_Customer);
        if (ds.Tables[0].Rows.Count > 0)
        {
            hdnId.Value = ds.Tables[0].Rows[0]["rol"].ToString();
            //FillContact(Convert.ToInt32(hdnId.Value));
            txtName.Text = ds.Tables[0].Rows[0]["name"].ToString();
            txtContact.Text = ds.Tables[0].Rows[0]["Contact"].ToString();
            txtContactPhone.Text = ds.Tables[0].Rows[0]["Phone"].ToString();
            txtContactEmail.Text = ds.Tables[0].Rows[0]["Email"].ToString();
            txtCustomer.Text = ds.Tables[0].Rows[0]["CustomerName"].ToString();
            txtCompany.Text = ds.Tables[0].Rows[0]["Company"].ToString();
            lblHeaderLabel.Text = "Task# " + Convert.ToString(Request.QueryString["uid"]);
            if (Convert.ToString(ds.Tables[0].Rows[0]["subject"]) != "")
            {
                lblHeaderLabel.Text = lblHeaderLabel.Text + " | " + Convert.ToString(ds.Tables[0].Rows[0]["subject"]);
            }
            txtSubject.Text = ds.Tables[0].Rows[0]["subject"].ToString();
            
            if (Request.QueryString["fl"] != null)
            {
                txtDesc.Text = "Follow-Up Task." + Environment.NewLine + ds.Tables[0].Rows[0]["remarks"].ToString() + Environment.NewLine + ds.Tables[0].Rows[0]["result"].ToString();
                txtCallDt.Text = DateTime.Now.ToShortDateString();
                txtCallTime.Text = DateTime.Now.ToShortTimeString().Replace("12:00 AM", "");
            }
            else
            {
                txtDesc.Text = ds.Tables[0].Rows[0]["remarks"].ToString();
                txtCallDt.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["datedue"]).ToShortDateString();
                txtCallTime.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["timedue"]).ToShortTimeString().Replace("12:00 AM", "");
            }

            //ddlAssigned.SelectedValue = ds.Tables[0].Rows[0]["fuser"].ToString();
            var assignedUser = ds.Tables[0].Rows[0]["fuser"].ToString();
            if (ddlAssigned.Items.FindByValue(assignedUser) != null)
            {
                ddlAssigned.SelectedValue = assignedUser;
            }
            else if (ddlAssigned.Items.FindByText(ds.Tables[0].Rows[0]["fuser"].ToString()) != null)
            {
                ddlAssigned.Items.FindByText(ds.Tables[0].Rows[0]["fuser"].ToString()).Selected = true;
            }

            ddlTaskCategory.SelectedValue = ds.Tables[0].Rows[0]["Keyword"].ToString();
            chkAlert.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsAlert"]);
            txtDuration.Text = ds.Tables[0].Rows[0]["Duration"].ToString();
            lblType.Text = ds.Tables[0].Rows[0]["contacttype"].ToString();
            FillTasks(ds.Tables[0].Rows[0]["rol"].ToString());
            //BindEmails(GetMailsfromdb(-1, string.Empty));

            if (ds.Tables[0].Rows[0]["createdby"].ToString() != string.Empty)
                lblCreate.Text = ds.Tables[0].Rows[0]["createdby"].ToString() + ", " + ds.Tables[0].Rows[0]["createdate"].ToString();

            if (ds.Tables[0].Rows[0]["lastupdatedby"].ToString() != string.Empty)
                lblUpdate.Text = ds.Tables[0].Rows[0]["lastupdatedby"].ToString() + ", " + ds.Tables[0].Rows[0]["lastupdatedate"].ToString();

            if (ds.Tables[0].Rows[0]["statusID"].ToString() == "1" && Request.QueryString["fl"] == null)
            {
                ddlStatus.SelectedValue = "1";
                ddlStatus.Enabled = false;
                txtResol.Enabled = true;
                txtResol.Text = ds.Tables[0].Rows[0]["result"].ToString();
            }
        }
    }
    private void CompanyPermission()
    {
        if (Session["COPer"].ToString() == "1")
        {
            dvCompanyPermission.Visible = true;
        }
        else
        {
            dvCompanyPermission.Visible = false;
        }
    }
    protected void lnkSave_Click(object sender, EventArgs e)
    {
        if (ddlStatus.SelectedItem.Value == "1" && Convert.ToInt32(ViewState["CompleteTaskPermission"]) == 0)
        {
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "321125", "noty({text: 'You do not have completed task permissions!',  type : 'warning', layout:'topCenter',closeOnSelfClick:true,dismissQueue: true, timeout : 3000,theme : 'noty_theme_default',  closable : true});", true);
            return;
        }
        objProp_Customer.ConnConfig = Session["config"].ToString();
        objProp_Customer.ROL = Convert.ToInt32(hdnId.Value);
        objProp_Customer.DueDate = Convert.ToDateTime(txtCallDt.Text.Trim());
        objProp_Customer.TimeDue = Convert.ToDateTime("01/01/1900 " + txtCallTime.Text.Trim());
        objProp_Customer.Subject = txtSubject.Text.Trim();
        objProp_Customer.Remarks = txtDesc.Text.Trim();
        //objProp_Customer.AssignedTo = ddlAssigned.SelectedItem.Text.Trim();
        objProp_Customer.AssignedTo = ddlAssigned.SelectedValue;
        double dblDuration = 0;
        Double.TryParse(txtDuration.Text.Trim(),out dblDuration);
        objProp_Customer.Duration = dblDuration;
        objProp_Customer.Name = Session["Username"].ToString();
        objProp_Customer.Contact = txtContact.Text;
        objProp_Customer.Status = Convert.ToInt32(ddlStatus.SelectedValue);
        objProp_Customer.Resolution = txtResol.Text.Trim();
        objProp_Customer.LastUpdateUser = Session["username"].ToString();
        objProp_Customer.Phone = txtContactPhone.Text;
        objProp_Customer.Email = txtContactEmail.Text;
        objProp_Customer.Category = ddlTaskCategory.SelectedValue;
        objProp_Customer.IsAlert = chkAlert.Checked;

        try
        {
            string strMsg = "Added";
            if (ViewState["edit"].ToString() == "0")
            {
                objProp_Customer.TaskID = 0;
                objProp_Customer.Mode = 0;
                if (!string.IsNullOrEmpty(Request.QueryString["screen"]) && !string.IsNullOrEmpty(Request.QueryString["ref"]))
                {
                    if (ValidateScreen(Request.QueryString["screen"].ToString()))
                    {
                        objProp_Customer.Screen = Request.QueryString["screen"].ToString();
                        objProp_Customer.Ref = Convert.ToInt32(Request.QueryString["ref"].ToString());
                    }
                    else
                    {
                        string str = "The reference screen name is not valid.  Please check and try again!";
                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'warning', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, theme : 'noty_theme_default',  closable : true});", true);
                        return;
                    }
                }
                else
                {
                    objProp_Customer.Screen = string.Empty;
                    objProp_Customer.Ref = 0;
                }
                
                objBL_Customer.AddTask(objProp_Customer);

                #region Thomas: Send email with a appointment to login user 
                if (chkAlert.Checked)
                {
                    // Create
                    BusinessEntity.User objPropUser = new BusinessEntity.User();
                    BL_User objBL_User = new BL_User();
                    objPropUser.ConnConfig = Session["config"].ToString();
                    objPropUser.Username = ddlAssigned.SelectedItem.Value;
                    //objPropUser.UserID = Convert.ToInt32(ddlAssigned.SelectedItem.Value);
                    var mailTo = objBL_User.getUserEmail(objPropUser);
                    if (!string.IsNullOrEmpty(mailTo))
                    {
                        Mail mail = new Mail();
                        try
                        {
                            var uri = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, Request.ApplicationPath + "/addTask?uid=" + objProp_Customer.TaskID.ToString());
                            mail.To.Add(mailTo);
                            mail.From = WebBaseUtility.GetFromEmailAddress();
                            //mail.Title = "Task Appointment";
                            mail.Title = txtName.Text + ": " + objProp_Customer.Subject;

                            StringBuilder stringBuilder = new StringBuilder();
                            stringBuilder.AppendFormat("Dear {0}<br><br>", objProp_Customer.AssignedTo);
                            stringBuilder.Append("You are receiving an appointment task from MOM-->Sales-->Tasks<br><br>");
                            stringBuilder.AppendFormat("Customer Name: {0}<br>", txtCustomer.Text);
                            stringBuilder.AppendFormat("Location Name: {0}<br>", txtName.Text);
                            stringBuilder.AppendFormat("Contact Name: {0}, Phone: {1}, Email: {2}<br>", txtContact.Text, txtContactPhone.Text, txtContactEmail.Text);
                            stringBuilder.AppendFormat("Subject: {0}<br>", objProp_Customer.Subject);
                            stringBuilder.AppendFormat("Description: {0}<br>", objProp_Customer.Remarks);
                            stringBuilder.AppendFormat("Due on: {0} {1}<br><br>", txtCallDt.Text.Trim(), txtCallTime.Text.Trim());
                            stringBuilder.Append("Attached files is a task appointment assigned to you.<br>");
                            stringBuilder.Append("To add this appointment to your calendar, please open and save it<br><br>");
                            stringBuilder.AppendFormat("<a href={0}>{0}</a><br><br>", uri);
                            stringBuilder.Append("Thanks");
                            //var apBody = objProp_Customer.Remarks + "\\par Due on: ";
                            StringBuilder apBody = new StringBuilder();
                            var strRemarks = objProp_Customer.Remarks.Replace("\r\n", "=0D=0A").Replace("\n", "=0D=0A");
                            apBody.AppendFormat("{0}.=0D=0A", strRemarks);
                            apBody.AppendFormat("Due on: {0} {1}. =0D=0A ", txtCallDt.Text.Trim(), txtCallTime.Text.Trim());
                            apBody.Append("Attached files is a task appointment assigned to you.  =0D=0A");
                            //apBody.Append("To add this appointment to your calendar, please open and save it.=0D=0A");
                            apBody.Append("To add this appointment to your calendar, please open and save it.=0D=0A");
                            //apBody.Append("=0D=0A");
                            //apBody.AppendFormat("{0}", uri);
                            //apBody.Append("=0D=0A");
                            apBody.Append("Thanks");

                            mail.Text = stringBuilder.ToString();

                            WebBaseUtility.UpdateMailConfigurationFromLoginUser(mail);
                            //var apSubject = string.Format("Task name: {0}", objProp_Customer.Subject);
                            var apSubject = string.Format("Task name: {0}", txtCustomer.Text);
                            //var apBody = objProp_Customer.Remarks;
                            var strStartDate = string.Format("{0} {1}", txtCallDt.Text.Trim(), txtCallTime.Text.Trim());
                            var apStart = Convert.ToDateTime(strStartDate);
                            var apEnd = apStart.AddHours(objProp_Customer.Duration);

                            var icsAttachmentContentsStr = WebBaseUtility.CreateICSAttachmentCalendarStr(apSubject
                                , apBody.ToString()
                                , txtName.Text
                                , apStart
                                , apEnd
                                , 60
                                );
                            var myByteArray = System.Text.Encoding.UTF8.GetBytes(icsAttachmentContentsStr);
                            mail.attachmentBytes = myByteArray;
                            mail.FileName = "TaskAppointment.ics";
                            mail.Send();
                        }
                        catch (Exception ex)
                        {
                            //string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
                            string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(ex.Message);
                            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, theme : 'noty_theme_default',  closable : true});", true);
                        }
                    }
                }
                #endregion

                //if (ViewState["backToLead"] == null)
                if (string.IsNullOrEmpty(Request.QueryString["screen"]) || string.IsNullOrEmpty(Request.QueryString["ref"]))
                {
                    objGeneralFunctions.ResetFormControlValues(this);
                }
                else
                {
                    Session["AddEditTaskSuccMess"] = "Added";
                    lnkClose_Click(sender, e);
                }

                    //gvTasks.DataSource = null;
                    //gvTasks.DataBind();

                    //gvTasksOpen.DataSource = null;
                    //gvTasksOpen.DataBind();

                    //gvContacts.DataSource = null;
                    //gvContacts.DataBind();
            }
            else if (ViewState["edit"].ToString() == "1")
            {
                objProp_Customer.TaskID = Convert.ToInt32(Request.QueryString["uid"].ToString());
                objProp_Customer.Mode = 1;
                if (!string.IsNullOrEmpty(Request.QueryString["screen"]) && !string.IsNullOrEmpty(Request.QueryString["ref"]))
                {
                    //objProp_Customer.Screen = Request.QueryString["screen"].ToString();
                    //objProp_Customer.Ref = Convert.ToInt32(Request.QueryString["ref"].ToString());
                    if (ValidateScreen(Request.QueryString["screen"].ToString()))
                    {
                        objProp_Customer.Screen = Request.QueryString["screen"].ToString();
                        objProp_Customer.Ref = Convert.ToInt32(Request.QueryString["ref"].ToString());
                    }
                    else
                    {
                        string str = "The reference screen name is not valid.  Please check and try again!";
                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'warning', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, theme : 'noty_theme_default',  closable : true});", true);
                        return;
                    }
                }
                else
                {
                    objProp_Customer.Screen = string.Empty;
                    objProp_Customer.Ref = 0;
                }
                objBL_Customer.AddTask(objProp_Customer);

                if (objProp_Customer.Status == 0)
                {
                    #region Thomas: Send email with a appointment to login user 
                    if (chkAlert.Checked)
                    {
                        // Create
                        BusinessEntity.User objPropUser = new BusinessEntity.User();
                        BL_User objBL_User = new BL_User();
                        objPropUser.ConnConfig = Session["config"].ToString();
                        objPropUser.Username = ddlAssigned.SelectedItem.Value;
                        //objPropUser.UserID = Convert.ToInt32(ddlAssigned.SelectedItem.Value);
                        var mailTo = objBL_User.getUserEmail(objPropUser);
                        if (!string.IsNullOrEmpty(mailTo))
                        {
                            Mail mail = new Mail();
                            try
                            {
                                var uri = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, Request.ApplicationPath + "/addTask?uid=" + objProp_Customer.TaskID.ToString());
                                mail.To.Add(mailTo);
                                mail.From = WebBaseUtility.GetFromEmailAddress();
                                //mail.Title = "Task Appointment";
                                mail.Title = txtName.Text + ": " + objProp_Customer.Subject;

                                //StringBuilder stringBuilder = new StringBuilder();
                                //stringBuilder.AppendFormat("Dear {0}<br>", objProp_Customer.AssignedTo);
                                //stringBuilder.Append("Attached file is the appointment of your task which was assigned from MOM system.<br>");
                                //stringBuilder.Append("To add this appointment to your calendar, please open and save it<br>");

                                StringBuilder stringBuilder = new StringBuilder();
                                stringBuilder.AppendFormat("Dear {0}<br><br>", objProp_Customer.AssignedTo);
                                stringBuilder.Append("You are receiving an appointment task from MOM-->Sales-->Tasks<br><br>");
                                stringBuilder.AppendFormat("Customer Name: {0}<br>", txtCustomer.Text);
                                stringBuilder.AppendFormat("Location Name: {0}<br>", txtName.Text);
                                stringBuilder.AppendFormat("Contact Name: {0}, Phone: {1}, Email: {2}<br>", txtContact.Text, txtContactPhone.Text, txtContactEmail.Text);
                                stringBuilder.AppendFormat("Subject: {0}<br>", objProp_Customer.Subject);
                                stringBuilder.AppendFormat("Description: {0}<br>", objProp_Customer.Remarks);
                                stringBuilder.AppendFormat("Due on: {0} {1}<br><br>", txtCallDt.Text.Trim(), txtCallTime.Text.Trim());
                                stringBuilder.Append("Attached files is a task appointment assigned to you.<br>");
                                stringBuilder.Append("To add this appointment to your calendar, please open and save it<br><br>");
                                stringBuilder.AppendFormat("<a href={0}>{0}</a><br><br>", uri);
                                stringBuilder.Append("Thanks");

                                mail.Text = stringBuilder.ToString();

                                WebBaseUtility.UpdateMailConfigurationFromLoginUser(mail);
                                //var apSubject = string.Format("Task name: {0}", objProp_Customer.Subject);
                                var apSubject = string.Format("Task name: {0}", txtCustomer.Text);

                                StringBuilder apBody = new StringBuilder();
                                var strRemarks = objProp_Customer.Remarks.Replace("\r\n", "=0D=0A").Replace("\n", "=0D=0A");
                                apBody.AppendFormat("{0}.=0D=0A", strRemarks);
                                apBody.AppendFormat("Due on: {0} {1}. =0D=0A", txtCallDt.Text.Trim(), txtCallTime.Text.Trim());
                                apBody.Append("Attached files is a task appointment assigned to you. =0D=0A ");
                                //apBody.Append("To add this appointment to your calendar, please open and save it. =0D=0A");
                                apBody.Append("To add this appointment to your calendar, please open and save it.=0D=0A");
                                //apBody.Append("=0D=0A");
                                //apBody.AppendFormat("{0}", uri);
                                //apBody.Append("=0D=0A");
                                apBody.Append("Thanks");

                                //StringBuilder apBody = new StringBuilder();
                                //apBody.AppendFormat("{0}.", objProp_Customer.Remarks + "\n");
                                //apBody.AppendFormat("Due on: {0} {1}. \n", txtCallDt.Text.Trim(), txtCallTime.Text.Trim());
                                //apBody.Append("Attached files is a task appointment assigned to you.\n");
                                ////apBody.Append("To add this appointment to your calendar, please open and save it. =0D=0A");
                                //apBody.Append("To add this appointment to your calendar, please open and save it.\n");
                                //apBody.Append("\n");
                                //apBody.AppendFormat("{0}", uri);
                                //apBody.Append("\n");
                                //apBody.Append("Thanks");

                                //var apBody = objProp_Customer.Remarks;
                                var strStartDate = string.Format("{0} {1}", txtCallDt.Text.Trim(), txtCallTime.Text.Trim());
                                var apStart = Convert.ToDateTime(strStartDate);
                                var apEnd = apStart.AddHours(objProp_Customer.Duration);

                                //var icsAttachmentContents = WebBaseUtility.CreateICSAttachmentCalendar(apSubject
                                //    , apBody
                                //    , txtName.Text
                                //    , apStart
                                //    , apEnd
                                //    , 60
                                //    );

                                //System.IO.File.WriteAllLines(System.Web.HttpContext.Current.Server.MapPath("FileName.ics"), icsAttachmentContents);
                                ////MAKE AN ATTACHMENT OUT OF THE .ICS FILE CREATED
                                ////Attachment mailAttachment = new Attachment(System.Web.HttpContext.Current.Server.MapPath("FileName.ics"));
                                //mail.AttachmentFiles.Add(System.Web.HttpContext.Current.Server.MapPath("FileName.ics"));

                                var icsAttachmentContentsStr = WebBaseUtility.CreateICSAttachmentCalendarStr(apSubject
                                    , apBody.ToString()
                                    , txtName.Text
                                    , apStart
                                    , apEnd
                                    , 60
                                    );
                                var myByteArray = System.Text.Encoding.UTF8.GetBytes(icsAttachmentContentsStr);
                                mail.attachmentBytes = myByteArray;
                                mail.FileName = "TaskAppointment.ics";
                                mail.Send();
                                //mail.SendEmailWithAppointment();
                                //ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccUp", "alert('Email sent successfully.');", true);
                            }
                            catch (Exception ex)
                            {
                                //string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
                                string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(ex.Message);
                                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, theme : 'noty_theme_default',  closable : true});", true);
                            }
                        }
                    }
                    #endregion
                }

                //if (ViewState["backToLead"] == null)
                if (string.IsNullOrEmpty(Request.QueryString["screen"]) || string.IsNullOrEmpty(Request.QueryString["ref"]))
                {
                    if (Request.QueryString["redirect"] != null)
                    {
                        Session["AddEditTaskSuccMess"] = "Updated";
                        lnkClose_Click(sender, e);
                    }
                    else
                    {
                        strMsg = "Updated";
                        if (ddlStatus.SelectedValue == "1")
                        {
                            ddlStatus.Enabled = false;
                        }
                        FillTasks(hdnId.Value);
                        RadGrid_gvLogs.Rebind();
                    }
                }
                else
                {
                    Session["AddEditTaskSuccMess"] = "Updated";
                    lnkClose_Click(sender, e);
                }
                //BindEmails(GetMailsfromdb(-1, string.Empty));
            }

            string strScript = string.Empty;
            if (chkFollowUp.Checked == true)
            {
                strScript += "CheckFollowup(" + Request.QueryString["uid"].ToString() + "," + chkFollowUp.ClientID + ");";
            }
            strScript += "noty({text: 'Task " + strMsg + " Successfully!', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});";
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccUp", strScript, true);

            FillRecentProspect();
            // Update todo tasks notification
            UpdateTodoTasksNumberMasterpage();

            RadGrid_Contacts.Rebind();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, theme : 'noty_theme_default',  closable : true});", true);
        }

        //  Response.Redirect("AddProspect.aspx?uid=" + ProspectID);
    }

    protected void lnkClose_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(Request.QueryString["redirect"]))
        {
            Response.Redirect(HttpUtility.UrlDecode(Request.QueryString["redirect"]));
        }
        else
        {
            //if (ViewState["backToLead"] == null)
            //{
            //    Response.Redirect("tasks.aspx");
            //}
            //else
            //{
            //    Response.Redirect("addprospect.aspx?uid=" + ViewState["backToLead"].ToString());
            //}
            if (string.IsNullOrEmpty(Request.QueryString["screen"]) || string.IsNullOrEmpty(Request.QueryString["ref"]))
            {
                Response.Redirect("tasks.aspx");
            }
            else
            {
                if(Request.QueryString["screen"].ToLower() == "lead" && !string.IsNullOrEmpty(Request.QueryString["ref"]))
                {
                    Response.Redirect("addprospect.aspx?uid=" + Request.QueryString["ref"].ToString() + "&tab=opentask");
                }
                else if (Request.QueryString["screen"].ToLower() == "opportunity" && !string.IsNullOrEmpty(Request.QueryString["ref"]))
                {
                    Response.Redirect("addopprt.aspx?uid=" + Request.QueryString["ref"].ToString());
                }
                else if (Request.QueryString["screen"].ToLower() == "task" && !string.IsNullOrEmpty(Request.QueryString["ref"]))
                {
                    Response.Redirect("addtask.aspx?uid=" + Request.QueryString["ref"].ToString());
                }
                else
                {
                    Response.Redirect("tasks.aspx");
                }
            }
        }
    }
    protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlStatus.SelectedValue == "1")
        {
            txtResol.Enabled = true;
        }
        else
        {
            txtResol.Enabled = false;
        }
    }
    private void FillUsers()
    {
        objPropUser.ConnConfig = Session["config"].ToString();
        DataSet ds = new DataSet();
        ds = objBL_User.getTaskUsers(objPropUser);

        ddlAssigned.DataSource = ds.Tables[0];
        ddlAssigned.DataTextField = "fuser";
        ddlAssigned.DataValueField = "username";
        ddlAssigned.DataBind();

        ddlAssigned.Items.Insert(0, new ListItem("Select", ""));
    }
    private void FillRecentProspect()
    {
        //NewSalesMaster masterSalesMaster = (NewSalesMaster)Page.Master;
        //masterSalesMaster.FillRecentProspect();
    }
    protected void btnFillTasks_Click(object sender, EventArgs e)
    {
        FillTasks(hdnId.Value);
        RadGrid_Contacts.Rebind();
        //FillContact(Convert.ToInt32(hdnId.Value));
        //BindEmails(GetMailsfromdb(-1, string.Empty));
        HyperLink1.NavigateUrl = "AddTask.aspx?rol=" + hdnId.Value + "&name=" + txtName.Text;
        HyperLink2.NavigateUrl = "AddTask.aspx?rol=" + hdnId.Value + "&name=" + txtName.Text;
        lnkNewEmail.NavigateUrl = "email.aspx?rol=" + hdnId.Value;

        if(!string.IsNullOrEmpty(hdnId.Value) && hdnId.Value != "0")
        {
            lnkAddnewContact.Visible = true;
            btnEditContact.Visible = true;
            btnDeleteContact.Visible = true;
        }
        else
        {
            lnkAddnewContact.Visible = false;
            btnEditContact.Visible = false;
            btnDeleteContact.Visible = false;
        }
    }
    //private DataSet GetMailsfromdb(int type, string OrderBy)
    //{
    //    if (OrderBy == string.Empty)
    //        ViewState["sortexp"] = null;

    //    DataSet ds = null;
    //    if (hdnId.Value.Trim() != string.Empty)
    //    {
    //        objGeneral.OrderBy = OrderBy;
    //        objGeneral.ConnConfig = Session["config"].ToString();
    //        objGeneral.type = type;
    //        objGeneral.rol = Convert.ToInt32(hdnId.Value);
    //        objGeneral.userid = Convert.ToInt32(Session["userid"].ToString());
    //        ds = objBL_General.GetMails(objGeneral);
    //    }
    //    return ds;
    //}
    //private void BindEmails(DataSet ds)
    //{
    //    if (ds != null)
    //    {
    //        gvmail.DataSource = ds.Tables[0];
    //        gvmail.DataBind();
    //        //menuLeads.Items[3].Text = "Emails(" + ds.Tables[0].Rows.Count + ")";
    //        ViewState["newmail"] = ds.Tables[0].Rows.Count;
    //        ////lblNewEmail.Text = string.Empty;
    //        //lblEmailCount.Text = string.Empty;
    //        //panel9.Visible = false;
    //        hdnMailct.Value = ds.Tables[0].Rows.Count.ToString();
    //    }
    //    else
    //    {
    //        gvmail.DataBind();
    //        //menuLeads.Items[2].Text = "Emails(0)";
    //    }
    //}
    protected void lnkRefreshMails_Click(object sender, EventArgs e)
    {
        //if (Application["pop3"] == null)
        //{
        //    Application["pop3"] = 0;
        //}

        //if ((int)Application["pop3"] == 0)
        //{
        //    Application["pop3"] = 1;
        //    try
        //    {
        //        DataSet ds = new DataSet();
        //        objGeneral.ConnConfig = Session["config"].ToString();
        //        ds = objBL_General.GetEmailAccounts(objGeneral);
        //        DataSet dsEmail = objBL_General.getCRMEmails(objGeneral);
        //        foreach (DataRow dr in ds.Tables[0].Rows)
        //        {
        //            ////Thread email = new Thread(delegate()
        //            ////   {
        //            ////try
        //            ////{

        //            string host = dr["inserver"].ToString();
        //            string user = dr["inusername"].ToString();
        //            string pass = dr["inpassword"].ToString();
        //            string port = dr["inport"].ToString();
        //            int Userid = Convert.ToInt32(dr["Userid"]);
        //            string LastFetch = dr["lastfetch"].ToString();
        //            //objGeneralFunctions.DownloadMailsIMAP(host, user, pass, port, Userid, Session["config"].ToString(), LastFetch, dsEmail);

        //            //objGeneralFunctions.DownloadMails(host, user, pass, port, Userid, Session["config"].ToString());
        //            ////}
        //            ////catch(Exception ex)
        //            ////{
        //            ////    log(ex.Message + Environment.NewLine + ex.InnerException + Environment.NewLine + ex.StackTrace);
        //            ////}
        //            ////  });
        //            ////email.IsBackground = true;
        //            ////email.Start();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
        //        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, theme : 'noty_theme_default',  closable : true});", true);
        //    }
        //    finally
        //    {
        //        Application["pop3"] = 0;
        //    }
        //}
        //else
        //{
        //    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr1", "noty({text: 'Mail download in progress by another user. Please refresh to get downloaded mails.',  type : 'information', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, timeout: 5000, theme : 'noty_theme_default',  closable : true});", true);
        //}

        //BindEmails(GetMailsfromdb(-1, string.Empty));
    }


    protected void Timer1_Tick(object sender, EventArgs e)
    {
        //int newmail = 0;
        //if (ViewState["newmail"] != null)
        //{
        //    newmail = Convert.ToInt32(ViewState["newmail"].ToString());
        //}
        //DataSet ds = GetMailsfromdb(-1, string.Empty);

        //if (newmail != ds.Tables[0].Rows.Count)
        //{
        //    if (ViewState["newmail"] != null)
        //    {
        //        //lblNewEmail.Text = Convert.ToString(ds.Tables[0].Rows.Count - newmail) + " New Email(s)";
        //        lblEmailCount.Text = Convert.ToString(ds.Tables[0].Rows.Count - newmail) + " New Email(s)";
        //        panel9.Visible = true;
        //    }
        //}
        ////lblEmailCount.Text = Convert.ToString(newmail) + " New Email(s)";
        ////panel9.Visible = true;
    }

    //protected void Page_PreRender(Object o, EventArgs e)
    //{
    //    UpdateProgress up = (UpdateProgress)Page.Master.Master.FindControl("UpdateProgress1");
    //    up.Visible = false;
    //}
    //private void FillContact(int rol)
    //{
    //    objProp_Customer.ConnConfig = Session["config"].ToString();
    //    objProp_Customer.ROL = rol;
    //    DataSet ds = new DataSet();
    //    RadGrid_Contacts.Rebind();
    //    //ds = objBL_Customer.getContactByRolID(objProp_Customer);
    //    //RadGrid_Contacts.DataSource = ds.Tables[0];
    //    //RadGrid_Contacts.DataBind();
    //    //menuLeads.Items[2].Text = "Contacts(" + ds.Tables[0].Rows.Count + ")";
    //    //if (ds.Tables[1].Rows.Count > 0)
    //    //{
    //    //    txtCompany.Text = ds.Tables[1].Rows[0]["Company"].ToString();
    //    //}

    //}

    protected void RadGrid_Contacts_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        //if (Request.QueryString["rol"] != null)
        if(Request.QueryString["rol"] != null || !string.IsNullOrEmpty(hdnId.Value))
        {
            objProp_Customer.ConnConfig = Session["config"].ToString();
            objProp_Customer.ROL = hdnId.Value != "" ? Convert.ToInt32(hdnId.Value) : Convert.ToInt32(Request.QueryString["rol"]);
            DataSet ds = new DataSet();
            ds = objBL_Customer.GetContactAllByRolID(objProp_Customer);
            RadGrid_Contacts.VirtualItemCount = ds.Tables[0].Rows.Count;
            RadGrid_Contacts.DataSource = ds.Tables[0];
            if (ds.Tables[1].Rows.Count > 0)
            {
                txtCompany.Text = ds.Tables[1].Rows[0]["Company"].ToString();
            }
        }
        else
        {
            RadGrid_Contacts.DataSource = string.Empty;
        }
    }

    protected void RadGrid_Tasks_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {

    }

    protected void RadGrid_TaskHistory_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {

    }

    protected void RadGrid_Mail_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        if (hdnId.Value.Trim() != string.Empty)
        {
            DataSet ds = null;

            objGeneral.OrderBy = "";
            objGeneral.ConnConfig = Session["config"].ToString();
            objGeneral.type = -1;
            objGeneral.rol = Convert.ToInt32(hdnId.Value);
            objGeneral.userid = Convert.ToInt32(Session["userid"].ToString());
            ds = objBL_General.GetMails(objGeneral);

            RadGrid_Mail.VirtualItemCount = ds.Tables[0].Rows.Count;
            RadGrid_Mail.DataSource = ds.Tables[0];
        }
    }

    protected void lnkFirst_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = new DataTable();
            dt = (DataTable)Session["tasks"];
            string url = "addtask.aspx?uid=" + dt.Rows[0]["ID"];
            Response.Redirect(url);
        }
        catch (Exception ex)
        {

        }
    }

    protected void lnkPrevious_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = new DataTable();
            dt = (DataTable)Session["tasks"];
            DataColumn[] keyColumns = new DataColumn[1];
            keyColumns[0] = dt.Columns["ID"];
            dt.PrimaryKey = keyColumns;

            DataRow d = dt.Rows.Find(Request.QueryString["uid"].ToString());
            int index = dt.Rows.IndexOf(d);
            int c = dt.Rows.Count - 1;
            if (index < c)
            {
                string url = "addtask.aspx?uid=" + dt.Rows[index - 1]["ID"];
                Response.Redirect(url);
            }
        }
        catch (Exception ex)
        {

        }
    }

    protected void lnkNext_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = new DataTable();
            dt = (DataTable)Session["tasks"];
            DataColumn[] keyColumns = new DataColumn[1];
            keyColumns[0] = dt.Columns["ID"];
            dt.PrimaryKey = keyColumns;

            DataRow d = dt.Rows.Find(Request.QueryString["uid"].ToString());
            int index = dt.Rows.IndexOf(d);
            int c = dt.Rows.Count - 1;
            if (index < c)
            {
                string url = "addtask.aspx?uid=" + dt.Rows[index + 1]["ID"];
                Response.Redirect(url);
            }
        }
        catch (Exception ex)
        {

        }
    }

    protected void lnkLast_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = new DataTable();
            dt = (DataTable)Session["tasks"];
            string url = "addtask.aspx?uid=" + dt.Rows[dt.Rows.Count - 1]["ID"];
            Response.Redirect(url);
        }
        catch (Exception ex)
        {

        }
    }

    private void UpdateTodoTasksNumberMasterpage()
    {
        var lblTodoNumber = Page.Master.FindControl("lblTodoNumber") as Label;
        var hdnItemJSON = Page.Master.FindControl("hdnItemJSON") as HiddenField;
        var todoNotify = Page.Master.FindControl("todoNotify") as Panel;

        string jsonString = string.Empty;
        JavaScriptSerializer sr = new JavaScriptSerializer();
        List<Dictionary<object, object>> dictListEval = new List<Dictionary<object, object>>();

        DataSet ds = new DataSet();
        objProp_Customer.ConnConfig = Session["config"].ToString();
        objProp_Customer.Username = Session["username"].ToString();
        objProp_Customer.UserID = Convert.ToInt32(Session["UserID"].ToString());
        objProp_Customer.DueDate = DateTime.Now;

        ds = objBL_Customer.GetTodoTasksOfUserForTheDate(objProp_Customer);

        GeneralFunctions objGeneral = new GeneralFunctions();
        dictListEval = objGeneral.RowsToDictionary(ds.Tables[0]);
        jsonString = sr.Serialize(dictListEval);

        if (hdnItemJSON != null)
        {
            hdnItemJSON.Value = jsonString;
        }

        if (lblTodoNumber != null && todoNotify != null)
        {
            if (dictListEval.Count > 0)
            {
                lblTodoNumber.Visible = true;
                todoNotify.Visible = true;
            }
            else
            {
                lblTodoNumber.Visible = false;
                todoNotify.Visible = false;
            }

            lblTodoNumber.Text = dictListEval.Count.ToString();
        }
    }

    #region logs
    protected void RadGrid_gvLogs_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            RadGrid_gvLogs.AllowCustomPaging = !ShouldApplySortFilterOrGroupLogs();
            if (Request.QueryString["uid"] != null)
            {
                DataSet dsLog = new DataSet();
                objProp_Customer.ConnConfig = Session["config"].ToString();
                objProp_Customer.TaskID = Convert.ToInt32(Request.QueryString["uid"]);
                dsLog = objBL_Customer.GetTasksLogs(objProp_Customer);
                if (dsLog.Tables[0].Rows.Count > 0)
                {
                    RadGrid_gvLogs.VirtualItemCount = dsLog.Tables[0].Rows.Count;
                    RadGrid_gvLogs.DataSource = dsLog.Tables[0];
                }
                else
                {
                    RadGrid_gvLogs.DataSource = string.Empty;
                }
            }
        }
        catch { }
    }
    bool isGroupLog = false;
    public bool ShouldApplySortFilterOrGroupLogs()
    {
        return RadGrid_gvLogs.MasterTableView.FilterExpression != "" ||
            (RadGrid_gvLogs.MasterTableView.GroupByExpressions.Count > 0 || isGroupLog) ||
            RadGrid_gvLogs.MasterTableView.SortExpressions.Count > 0;
    }
    protected void RadGrid_gvLogs_ItemCreated(object sender, GridItemEventArgs e)
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
    #endregion

    private bool ValidateScreen(string screenName)
    {
        List<string> validScreens = new List<string>()
        {
            "opportunity",
            "lead",
            "project",
            "equipment",
            "task"
        };
        if (!string.IsNullOrEmpty(screenName) && validScreens.Contains(screenName.ToLower()))
            return true;
        else return false;
    }

    private void FillTaskCategory()
    {
        objProp_Customer.ConnConfig = Session["config"].ToString();
        DataSet ds = new DataSet();
        ds = objBL_Customer.GetTaskCategories(objProp_Customer);

        ddlTaskCategory.DataSource = ds.Tables[0];
        ddlTaskCategory.DataTextField = "Name";
        ddlTaskCategory.DataValueField = "Name";
        ddlTaskCategory.DataBind();

        ddlTaskCategory.Items.Insert(0, new ListItem("Select", ""));
    }

    protected void lnkAddnewContact_Click(object sender, EventArgs e)
    {
        RadWindowContact.Title = "Add Contact";
        txtContcName.Text = "";
        txtTitle.Text = "";
        txtContPhone.Text = "";
        txtContFax.Text = "";
        txtContCell.Text = "";
        txtContEmail.Text = "";
        ViewState["ContactID"] = "0";
        string script = "function f(){$find(\"" + RadWindowContact.ClientID + "\").show(); Sys.Application.remove_load(f);}" +
            "$('[id *= txtContPhone]').mask('(999) 999 - 9999 ? Ext 99999');" +
            "$('[id *= txtContPhone]').bind('paste', function () { $(this).val(''); });" +
            "$('[id *= txtContCell]').mask('(999) 999 - 9999');" +
            "$('[id *= txtContFax]').mask('(999) 999 - 9999');" +
            "Sys.Application.add_load(f);" +
            "Materialize.updateTextFields();";
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
    }

    protected void btnEditContact_Click(object sender, EventArgs e)
    {
        if (RadGrid_Contacts.SelectedItems.Count > 0)
        {
            RadWindowContact.Title = "Edit Contact";

            foreach (GridDataItem item in RadGrid_Contacts.SelectedItems)
            {
                //DataTable dt = (DataTable)Session["contacttablelead"];
                Label lblContactName = (Label)item.FindControl("lblContactName");
                Label lblContactTitle = (Label)item.FindControl("lblContactTitle");
                Label lblContactPhone = (Label)item.FindControl("lblContactPhone");
                Label lblContactFax = (Label)item.FindControl("lblContactFax");
                Label lblContactCell = (Label)item.FindControl("lblContactCell");
                Label lblEmail = (Label)item.FindControl("lblEmail");
                HiddenField hdnContactID = (HiddenField)item.FindControl("hdnContactID");

                //DataRow dr = dt.Rows[Convert.ToInt32(lblindex.Text)];

                txtContcName.Text = lblContactName.Text;
                txtTitle.Text = lblContactTitle.Text;
                txtContPhone.Text = lblContactPhone.Text;
                txtContFax.Text = lblContactFax.Text;
                txtContCell.Text = lblContactCell.Text;
                txtContEmail.Text = lblEmail.Text;
                ViewState["ContactID"] = hdnContactID.Value;
                //ViewState["index"] = lblindex.Text;
            }

            string script = "function f(){$find(\"" + RadWindowContact.ClientID + "\").show(); Sys.Application.remove_load(f);}" +
                "$('[id *= txtContPhone]').mask('(999) 999 - 9999 ? Ext 99999');" +
                "$('[id *= txtContPhone]').bind('paste', function () { $(this).val(''); });" +
                "$('[id *= txtContCell]').mask('(999) 999 - 9999');" +
                "$('[id *= txtContFax]').mask('(999) 999 - 9999');" +
                "Sys.Application.add_load(f); " +
                "Materialize.updateTextFields();";
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarn", "noty({text: 'Select a contact for updating!',  type : 'warning', dismissQueue: true, timeout: 3000, layout:'topCenter',closeOnSelfClick:false, theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void btnDeleteContact_Click(object sender, EventArgs e)
    {
        try
        {
            if (RadGrid_Contacts.SelectedItems.Count > 0)
            {
                foreach (GridDataItem item in RadGrid_Contacts.SelectedItems)
                {
                    HiddenField hdnContactID = (HiddenField)item.FindControl("hdnContactID");
                    PhoneModel objProp_Phone = new PhoneModel();
                    objProp_Phone.ConnConfig = Session["config"].ToString();
                    if (!string.IsNullOrEmpty(hdnContactID.Value))
                    {
                        objProp_Phone.ID = Convert.ToInt32(hdnContactID.Value);
                    }
                    else
                    {
                        objProp_Phone.ID = 0;
                    }
                    objBL_Customer.DeleteContact(objProp_Phone);
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "noty({text: 'Contact deleted',  type : 'success', dismissQueue: true, timeout: 3000, layout:'topCenter',closeOnSelfClick:false, theme : 'noty_theme_default',  closable : true});", true);
                }
                
                RadGrid_Contacts.Rebind();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarn", "noty({text: 'Select a contact for deleting!',  type : 'warning', dismissQueue: true, timeout: 3000, layout:'topCenter',closeOnSelfClick:false, theme : 'noty_theme_default',  closable : true});", true);
            }
        }
        catch (Exception ex)
        {
            string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(ex.Message);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', dismissQueue: true, timeout: 3000, layout:'topCenter',closeOnSelfClick:false, theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void lnkContactSave_Click(object sender, EventArgs e)
    {
        try
        {
            //DataTable dt = (DataTable)Session["contacttablelead"];
            PhoneModel objProp_Phone = new PhoneModel();
            objProp_Phone.ConnConfig = Session["config"].ToString();
            objProp_Phone.Name = objGeneralFunctions.Truncate(txtContcName.Text, 50);
            objProp_Phone.Title = objGeneralFunctions.Truncate(txtTitle.Text, 50);
            objProp_Phone.Phone = objGeneralFunctions.Truncate(txtContPhone.Text, 50);
            objProp_Phone.Fax = objGeneralFunctions.Truncate(txtContFax.Text, 22);
            objProp_Phone.Cell = objGeneralFunctions.Truncate(txtContCell.Text, 22);
            objProp_Phone.Email = objGeneralFunctions.Truncate(txtContEmail.Text, 50);
            objProp_Phone.Rol = Convert.ToInt32(hdnId.Value);
            if (ViewState["ContactID"] != null && ViewState["ContactID"].ToString() != "")
            {
                objProp_Phone.ID = Convert.ToInt32(ViewState["ContactID"].ToString());
            }
            else
            {
                objProp_Phone.ID = 0;
            }
            objBL_Customer.AddUpdateContact(objProp_Phone);

            RadGrid_Contacts.Rebind();
            if(objProp_Phone.ID == 0)
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "noty({text: 'Contact added',  type : 'success', dismissQueue: true, timeout: 3000, layout:'topCenter',closeOnSelfClick:false, theme : 'noty_theme_default',  closable : true});", true);
            else
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "noty({text: 'Contact updated',  type : 'success', dismissQueue: true, timeout: 3000, layout:'topCenter',closeOnSelfClick:false, theme : 'noty_theme_default',  closable : true});", true);

        }
        catch (Exception ex)
        {
            string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(ex.Message);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', dismissQueue: true, timeout: 3000, layout:'topCenter',closeOnSelfClick:false, theme : 'noty_theme_default',  closable : true});", true);
        }
    }
}
