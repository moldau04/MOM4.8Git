using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BusinessLayer;
using BusinessEntity;

public partial class EstimateTemplate : System.Web.UI.Page
{
    Customer objProp_Customer = new Customer();
    BL_Customer objBL_Customer = new BL_Customer();

    protected void Page_Load(object sender, EventArgs e)
    {        
        Response.Redirect("projecttemplate.aspx?Role=SalesManager");
        if (Session["userid"] == null)
        {
            Response.Redirect("login.aspx");
        }
        if (!IsPostBack)
        {
            FillEstimateTemplate();
        }
        Permission();

    }
    private void Permission()
    {
        HtmlGenericControl li = (HtmlGenericControl)Page.Master.Master.FindControl("SalesMgr");
        //li.Style.Add("background", "url(images/active_menu_bg.png) repeat-x");
        li.Attributes.Add("class", "start active open");

        HyperLink a = (HyperLink)Page.Master.Master.FindControl("SalesLink");
        //a.Style.Add("color", "#2382b2");

        HyperLink lnkUsersSmenu = (HyperLink)Page.Master.Master.FindControl("lnkEstimateTempl");
        //lnkUsersSmenu.Style.Add("color", "#FF7A0A");
        lnkUsersSmenu.Style.Add("color", "#316b9d");
        lnkUsersSmenu.Style.Add("font-weight", "normal");
        lnkUsersSmenu.Style.Add("background-color", "#e5e5e5");
        //AjaxControlToolkit.HoverMenuExtender hm = (AjaxControlToolkit.HoverMenuExtender)Page.Master.Master.FindControl("HoverMenuExtenderSales");
        //hm.Enabled = false;
        HtmlGenericControl ul = (HtmlGenericControl)Page.Master.Master.FindControl("SalesMgrSub");
        //ul.Attributes.Remove("class");
        ul.Style.Add("display", "block");

        if (Session["type"].ToString() == "c")
        {
            Response.Redirect("home.aspx");
        }

        if (Session["MSM"].ToString() == "TS")
        {
            lnkDelete.Visible = false;
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
        }

    }
    protected void Page_PreRender(Object o, EventArgs e)
    {
        foreach (GridViewRow gr in gvEstimates.Rows)
        {
            CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");
            Label lblID = (Label)gr.FindControl("lblId");

            gr.Attributes["onclick"] = "SelectRowChk('" + gr.ClientID + "','" + chkSelect.ClientID + "','" + gvEstimates.ClientID + "',event);";
            gr.Attributes["ondblclick"] = "location.href='addestimatetemplate.aspx?uid=" + lblID.Text + "'";
        }
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "key8", "SelectedRowStyle('" + gvEstimates.ClientID + "');", true);
    }
    private void FillEstimateTemplate()
    {
        DataSet ds = new DataSet();
        objProp_Customer.ConnConfig = Session["config"].ToString();
        ds = objBL_Customer.getEstimateTemplate(objProp_Customer);
        gvEstimates.DataSource = ds.Tables[0];
        gvEstimates.DataBind();
    }
    protected void lnkClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("home.aspx");
    }
    protected void lnkSearch_Click(object sender, ImageClickEventArgs e)
    {
        FillEstimateTemplate();
    }
    protected void lnkShowAll_Click(object sender, EventArgs e)
    {
        FillEstimateTemplate();
    }
    protected void lnkEdit_Click(object sender, EventArgs e)
    {
        foreach (GridViewRow di in gvEstimates.Rows)
        {
            CheckBox chkSelect = (CheckBox)di.FindControl("chkSelect");
            Label lblID = (Label)di.FindControl("lblId");

            if (chkSelect.Checked == true)
            {
                Response.Redirect("addestimatetemplate.aspx?uid=" + lblID.Text);
            }
        }
    }
    protected void lnkDelete_Click(object sender, EventArgs e)
    {
        try
        {
            foreach (GridViewRow di in gvEstimates.Rows)
            {
                CheckBox chkSelect = (CheckBox)di.FindControl("chkSelect");
                Label lblProspectID = (Label)di.FindControl("lblID");

                if (chkSelect.Checked == true)
                {
                    objProp_Customer.ConnConfig = Session["config"].ToString();
                    objProp_Customer.dtLaborItems = null;
                    objProp_Customer.dtItems = null;
                    objProp_Customer.Mode = 2;
                    objProp_Customer.TemplateID = Convert.ToInt32(lblProspectID.Text);
                    objBL_Customer.AddEstimateTemplate(objProp_Customer);

                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyDel", "noty({text: 'Template Deleted Successfully!', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                    FillEstimateTemplate();
                }
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErrDelProspect", "noty({text: '" + str + "',  type : 'error', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);

        }
    }
    protected void lnkAdd_Click(object sender, EventArgs e)
    {
        Response.Redirect("addestimatetemplate.aspx");
    }
    protected void ddlSearch_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
}
