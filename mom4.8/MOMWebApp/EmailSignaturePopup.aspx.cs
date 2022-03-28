using BusinessEntity;
using BusinessLayer;
using System;
using System.Data;
using System.Web.UI;

namespace MOMWebApp
{
    public partial class EmailSignaturePopup : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["id"]))
            {
                this.Title = "Update E-mail Signature";
            }
            else
            {
                this.Title = "Add E-mail Signature";
            }

            if (!IsPostBack)
            {
                EmailSignature eSignature = new EmailSignature();

                if (!string.IsNullOrEmpty(Request.QueryString["id"]))
                {
                    //this.Title = "Update E-mail Signature";
                    eSignature.Id = Convert.ToInt32(Request.QueryString["id"]);
                    eSignature.ConnConfig = Session["Config"].ToString();
                    BL_User objBL_User = new BL_User();
                    DataSet ds = objBL_User.GetEmailSignatureById(eSignature);

                    if(ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        txtSignBody.Text = ds.Tables[0].Rows[0]["SignContent"].ToString();
                        txtSignName.Text = ds.Tables[0].Rows[0]["SignName"].ToString();
                        chkDefaultSign.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsDefault"].ToString());
                    }
                }

                if (Session["AddEmailSignSuccMess"] != null && Session["AddEmailSignSuccMess"].ToString() != "")
                {
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "noty({text: '"+ Session["AddEmailSignSuccMess"].ToString() + "',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                    Session["AddEmailSignSuccMess"] = null;
                }
            }
        }

        GeneralFunctions objgn = new GeneralFunctions();
        User objPropUser = new User();
        BL_User objBL_User = new BL_User();
        protected void Page_PreInit(object sender, System.EventArgs e)

        {
            Control header = Page.Master.FindControl("divHeader");
            header.Visible = false;
            Control menu = Page.Master.FindControl("menu");
            menu.Visible = false;
        }

        protected void lnkSaveSignature_Click(object sender, EventArgs e)
        {
            EmailSignature eSignature = new EmailSignature();

            if (!string.IsNullOrEmpty(Request.QueryString["id"]))
            {
                eSignature.Id = Convert.ToInt32(Request.QueryString["id"]);
            }
            else
            {
                eSignature.Id = 0;
            }

            if (!string.IsNullOrEmpty(Request.QueryString["userid"]))
            {
                eSignature.UserId = Convert.ToInt32(Request.QueryString["userid"]);
                eSignature.IsDefault = chkDefaultSign.Checked;
                eSignature.Name = txtSignName.Text.Trim();
                eSignature.Body = txtSignBody.Text;
                eSignature.ConnConfig = Session["config"].ToString();
                try
                {
                    BL_User objBL_User = new BL_User();
                    var Id = objBL_User.AddEmailSignature(eSignature);
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyRefresh", "updateparent();", true);
                    if (eSignature.Id != 0)
                    {
                        if(!string.IsNullOrEmpty(Request.QueryString["page"]) && Request.QueryString["page"] == "userprofile")
                        {
                            Session["UP_EmailSignSuccMess"] = "Updated successfully!";
                        }
                        else
                        {
                            //Session["EmailSignSuccMess"] = "Updated successfully!";
                            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "noty({text: 'Updated successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                        }
                    }
                    else
                    {
                        chkDefaultSign.Checked = false;
                        txtSignName.Text = string.Empty;
                        txtSignBody.Text = string.Empty;
                        if (!string.IsNullOrEmpty(Request.QueryString["page"]) && Request.QueryString["page"] == "userprofile")
                        {
                            Session["UP_EmailSignSuccMess"] = "Added successfully!";
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "noty({text: 'Added successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                        }
                    }
                }
                catch (Exception ex)
                {
                    var str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(ex.Message);
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '"+ str + "',  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: 'UserId is invalid!',  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
            }
        }
    }
}