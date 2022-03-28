using System;
using System.Web.UI;
using BusinessEntity;
using BusinessLayer;
using System.Data;


public partial class EmailTemplatePopup : System.Web.UI.Page
{
    BusinessEntity.CompanyOffice objCompany = new BusinessEntity.CompanyOffice();
    BL_Company objBL_Company = new BL_Company();
    User objPropUser = new User();
    BL_User objBL_User = new BL_User();
    Customer objCustomer = new Customer();
    BL_Customer objBL_Customer = new BL_Customer();
    
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["uid"] != null)
        {
            if (Convert.ToInt32(Request.QueryString["uid"]) != 0)
            {
                this.Title = "Edit Template";
            }
            else
            {
              
                this.Title = "Add Template";
                if (hdnID.Value != "")
                {
                    if (Convert.ToInt32(hdnID.Value) > 0)
                    {
                        this.Title = "Edit Template";
                    }
                }               

            }
        }

        if (!IsPostBack)
        {
            if (Request.QueryString["uid"] != null)
            {
                if (Convert.ToInt32(Request.QueryString["uid"]) != 0)
                {
                    hdnID.Value= Convert.ToString(Request.QueryString["uid"]);
                 
                    BL_SafetyTest bl_SafetyTest = new BL_SafetyTest();
                    DataSet ds = bl_SafetyTest.GetTestSetupEmailFormsById(Session["config"].ToString(), Convert.ToInt32(Request.QueryString["uid"]));
                    if (ds != null)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            txtEmailFormsName.Text = ds.Tables[0].Rows[0]["Name"].ToString();
                            txtBodyCKE.Text = ds.Tables[0].Rows[0]["Body"].ToString();
                            chkActive.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsActive"]);
                            this.Title = "Edit Template";
                        }
                    }
                }
                else
                {
                    txtBodyCKE.Text = "<p>	Dear {UserName}</p><p>Please review the attached file</p><p>Thanks</p>";
                    this.Title = "Add Template";
                    hdnID.Value = "0";
                }
            }
        } 
    }

    protected void Page_PreInit(object sender, System.EventArgs e)

    {
        Control header = Page.Master.FindControl("divHeader");
        header.Visible = false;
        Control menu = Page.Master.FindControl("menu");
        //menu.Visible = false;
    }

    protected void lnkUploadEmail_Click(object sender, EventArgs e)
    {
        try
        {           
            TestSetupEmailForm obj = new TestSetupEmailForm();          
            obj.ConnConfig = Session["config"].ToString();      
            obj.Name = txtEmailFormsName.Text;
            obj.Body = txtBodyCKE.Text;           
            obj.IsActive =chkActive.Checked;
            
            try
            {             

                if (Convert.ToInt32(hdnID.Value) > 0)
                {
                    obj.Id = Convert.ToInt32(hdnID.Value);
                    int result = 1;
                    BL_SafetyTest bl_SafetyTest = new BL_SafetyTest();
                    obj.UpdatedBy = Session["username"].ToString();
                    result=bl_SafetyTest.UpdateTestSetupEmailForms(obj);
                    if (result == 1)
                    {
                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyNoteSucc", "noty({text: 'Email template is updated successfully', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyNoteSucc", "noty({text: 'Name is exist!', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                    }
                    
                }
                else
                {
                    BL_SafetyTest bl_SafetyTest = new BL_SafetyTest();
                    obj.AddedBy = Session["username"].ToString();
                    hdnID.Value =Convert.ToString( bl_SafetyTest.AddTestSetupEmailForms(obj));
                    if (hdnID.Value != "-1")
                    {
                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyNoteSucc", "noty({text: 'Email template is created successfully', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                        this.Title = "Edit Template";
                    }
                    else
                    {
                        hdnID.Value = "0";
                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyNoteSucc", "noty({text: 'Name is exist!', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                    }

                }              
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "updateparent", "updateparent();", true);
            }
            catch (Exception ex)
            {
                string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccEmailForm", "noty({text: '" + str + "', dismissQueue: true,  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);

            }

        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyUploadEmailErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
}