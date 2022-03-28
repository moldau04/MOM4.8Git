using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessEntity;
using BusinessLayer;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Telerik.Web.UI;

public partial class CustomFields : System.Web.UI.Page
{
    General objPropGeneral = new General();
    BL_General objBL_General = new BL_General();
    protected DataTable dtFormat = new DataTable();
    BL_User objBL_User = new BL_User();
    User objPropUser = new User();
    JobT _objJob = new JobT();
    BL_Job objBL_Job = new BL_Job();

    //*UserCustom   
    protected DataTable dtUserCstFormat = new DataTable();
    //protected DataTable dtWorkflowFormat = new DataTable();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userid"] == null)
        {
            Response.Redirect("login.aspx");
        }

        FillFormat();

        if (!IsPostBack)
        {
            string SSL = System.Web.Configuration.WebConfigurationManager.AppSettings["SSL"].Trim();

            if (Request.Url.Scheme == "http" && SSL == "1")
            {
                string URL = Request.Url.ToString();
                URL = URL.Replace("http://", "https://");
                Response.Redirect(URL);
            }

            DataSet ds = new DataSet();
            objPropGeneral.ConnConfig = Session["config"].ToString();

            ContentPlaceHolder cph = (ContentPlaceHolder)this.Master.FindControl("ContentPlaceHolder1");
            //AjaxControlToolkit.TabContainer TC = (AjaxControlToolkit.TabContainer)cph.FindControl("TabContainer1");
            //AjaxControlToolkit.TabPanel TP = (AjaxControlToolkit.TabPanel)TC.FindControl("TabPanel1");
            //AjaxControlToolkit.TabPanel TP2 = (AjaxControlToolkit.TabPanel)TC.FindControl("TabPanel2");
            int totalField = 0;
            for (int i = 1; i < 11; i++)
            {
                objPropGeneral.CustomName = "Ticket" + Convert.ToString(i);
                ds = objBL_General.getCustomFields(objPropGeneral);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    TextBox txt = (TextBox)cph.FindControl("txtCst" + Convert.ToString(i));
                    txt.Text = ds.Tables[0].Rows[0]["label"].ToString();
                    totalField++;
                }
            }

            for (int i = 1; i < 3; i++)
            {
                objPropGeneral.CustomName = "Owner" + Convert.ToString(i);
                ds = objBL_General.getCustomFields(objPropGeneral);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    TextBox txt = (TextBox)cph.FindControl("txtCstOwner" + Convert.ToString(i));
                    txt.Text = ds.Tables[0].Rows[0]["label"].ToString();
                    totalField++;
                }
            }

            for (int i = 1; i < 3; i++)
            {
                objPropGeneral.CustomName = "loc" + Convert.ToString(i);
                ds = objBL_General.getCustomFields(objPropGeneral);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    TextBox txt = (TextBox)cph.FindControl("txtCstLoc" + Convert.ToString(i));
                    txt.Text = ds.Tables[0].Rows[0]["label"].ToString();
                    totalField++;
                }
            }

            for (int i = 1; i < 6; i++)
            {
                objPropGeneral.CustomName = "TicketCst" + Convert.ToString(i);
                ds = objBL_General.getCustomFields(objPropGeneral);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    TextBox txt = (TextBox)cph.FindControl("txtTickCst" + Convert.ToString(i));
                    txt.Text = ds.Tables[0].Rows[0]["label"].ToString();
                    totalField++;
                }
            }

            for (int i = 1; i < 5; i++)
            {
                objPropGeneral.CustomName = "LoadTest" + Convert.ToString(i);
                ds = objBL_General.getCustomFields(objPropGeneral);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    TextBox txt = (TextBox)cph.FindControl("txtTestCst" + Convert.ToString(i));
                    txt.Text = ds.Tables[0].Rows[0]["label"].ToString();
                    totalField++;
                }
            }

            for (int i = 1; i <= 20; i++)
            {
                objPropGeneral.CustomName = "Job" + Convert.ToString(i);
                ds = objBL_General.getCustomFields(objPropGeneral);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    TextBox txt = (TextBox)cph.FindControl("txtJobCust" + Convert.ToString(i));
                    txt.Text = ds.Tables[0].Rows[0]["label"].ToString();
                    totalField++;
                }
            }

            for (int i = 1; i <= 5; i++)
            {
                objPropGeneral.CustomName = "Job_Attribute_General" + Convert.ToString(i);
                ds = objBL_General.getCustomFields(objPropGeneral);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    TextBox txt = (TextBox)cph.FindControl("txtAttributesGeneralCustom" + Convert.ToString(i));
                    txt.Text = ds.Tables[0].Rows[0]["label"].ToString();
                    totalField++;
                }
            }
            //lblTotalLabel.Text = totalField.ToString();

            //PO
            for (int i = 1; i < 3; i++)
            {
                objPropGeneral.CustomName = "PO" + Convert.ToString(i);
                ds = objBL_General.getCustomFields(objPropGeneral);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    TextBox txt = (TextBox)cph.FindControl("txtPO" + Convert.ToString(i));
                    txt.Text = ds.Tables[0].Rows[0]["label"].ToString();
                    totalField++;
                }
            }

            BindCustomGrid(true);

            //* User Custom
            FillUserCustomFormat();
            GetUserCustomTable();
            BindUserCustomGrid();

        }

        Permission();
        HighlightSideMenu("progMgr", "lnkCustomFields", "progMgrSub");
    }

    private void HighlightSideMenu(string MenuParent, string PageLink, string SubMenuDiv)
    {
        HyperLink aNav = (HyperLink)Page.Master.FindControl(MenuParent);
        //li.Style.Add("background", "url(images/active_menu_bg.png) repeat-x");
        aNav.CssClass = "active collapsible-header waves-effect waves-cyan collapsible-height-nl";

        //HyperLink a = (HyperLink)Page.Master.Master.FindControl("SalesLink");
        //a.Style.Add("color", "#2382b2");

        HyperLink lnkUsersSmenu = (HyperLink)Page.Master.FindControl(PageLink);
        //lnkUsersSmenu.Style.Add("color", "#FF7A0A");
        lnkUsersSmenu.Style.Add("color", "#316b9d");
        lnkUsersSmenu.Style.Add("font-weight", "normal");
        lnkUsersSmenu.Style.Add("background-color", "#e5e5e5");
        //AjaxControlToolkit.HoverMenuExtender hm = (AjaxControlToolkit.HoverMenuExtender)Page.Master.Master.FindControl("HoverMenuExtenderSales");
        //hm.Enabled = false;
        HtmlGenericControl div = (HtmlGenericControl)Page.Master.FindControl(SubMenuDiv);
        div.Style.Add("display", "block");
    }

    private void Permission()
    {

        //AjaxControlToolkit.HoverMenuExtender hm = (AjaxControlToolkit.HoverMenuExtender)Page.Master.FindControl("HoverMenuExtenderProg");
        //hm.Enabled = false;
        //HtmlGenericControl ul = (HtmlGenericControl)Page.Master.FindControl("progMgrSub");
        //ul.Style.Add("display", "block");
        //ul.Style.Add("visibility", "visible");

        if (Session["type"].ToString() == "c")
        {
            Response.Redirect("home.aspx");
        }
        if (Session["MSM"].ToString() == "TS")
        {
            Response.Redirect("home.aspx");
        }
        if (Convert.ToInt32(Session["ISsupervisor"]) == 1)
        {
            Response.Redirect("home.aspx");
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            ContentPlaceHolder cph = (ContentPlaceHolder)this.Master.FindControl("ContentPlaceHolder1");
            //AjaxControlToolkit.TabContainer TC = (AjaxControlToolkit.TabContainer)cph.FindControl("TabContainer1");
            //AjaxControlToolkit.TabPanel TP = (AjaxControlToolkit.TabPanel)TC.FindControl("TabPanel1");
            //AjaxControlToolkit.TabPanel TP2 = (AjaxControlToolkit.TabPanel)TC.FindControl("TabPanel2");
            objPropGeneral.ConnConfig = Session["config"].ToString();

            for (int i = 1; i < 11; i++)
            {
                TextBox txt = (TextBox)cph.FindControl("txtCst" + Convert.ToString(i));
                objPropGeneral.CustomLabel = txt.Text;
                objPropGeneral.CustomName = "Ticket" + Convert.ToString(i);
                objBL_General.UpdateCustom(objPropGeneral);
            }

            for (int i = 1; i < 3; i++)
            {
                TextBox txt = (TextBox)cph.FindControl("txtCstOwner" + Convert.ToString(i));
                objPropGeneral.CustomLabel = txt.Text;
                objPropGeneral.CustomName = "Owner" + Convert.ToString(i);
                objBL_General.UpdateCustom(objPropGeneral);
            }
            for (int i = 1; i < 3; i++)
            {
                TextBox txt = (TextBox)cph.FindControl("txtCstLoc" + Convert.ToString(i));
                objPropGeneral.CustomLabel = txt.Text;
                objPropGeneral.CustomName = "loc" + Convert.ToString(i);
                objBL_General.UpdateCustom(objPropGeneral);
            }

            for (int i = 1; i < 6; i++)
            {
                TextBox txt = (TextBox)cph.FindControl("txtTickCst" + Convert.ToString(i));
                objPropGeneral.CustomLabel = txt.Text;
                objPropGeneral.CustomName = "TicketCst" + Convert.ToString(i);
                objBL_General.UpdateCustom(objPropGeneral);
            }

            for (int i = 1; i < 5; i++)
            {
                TextBox txt = (TextBox)cph.FindControl("txtTestCst" + Convert.ToString(i));
                objPropGeneral.CustomLabel = txt.Text;
                objPropGeneral.CustomName = "LoadTest" + Convert.ToString(i);
                objBL_General.UpdateCustom(objPropGeneral);
            }
            for (int i = 1; i <= 20; i++)
            {
                TextBox txt = (TextBox)cph.FindControl("txtJobCust" + Convert.ToString(i));
                objPropGeneral.CustomLabel = txt.Text;
                objPropGeneral.CustomName = "Job" + Convert.ToString(i);
                objBL_General.UpdateCustom(objPropGeneral);
            }

            for (int i = 1; i <= 5; i++)
            {
                TextBox txt = (TextBox)cph.FindControl("txtAttributesGeneralCustom" + Convert.ToString(i));
                objPropGeneral.CustomLabel = txt.Text;
                objPropGeneral.CustomName = "Job_Attribute_General" + Convert.ToString(i);
                objBL_General.UpdateCustom(objPropGeneral);
            }

            for (int i = 1; i < 3; i++)
            {
                TextBox txt = (TextBox)cph.FindControl("txtPO" + Convert.ToString(i));
                objPropGeneral.CustomLabel = txt.Text;
                objPropGeneral.CustomName = "PO" + Convert.ToString(i);
                objBL_General.UpdateCustom(objPropGeneral);
            }

            UpdateEstimateCustomFields();
            BindCustomGrid(true);

            UpdateUserCustomFields();

            ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: 'Labels updated successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void lnkClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("Home.aspx");
    }

    private void FillFormat()
    {
        try
        {
            dtFormat = new DataTable();
            dtFormat.Columns.Add("value", typeof(string));
            dtFormat.Columns.Add("format", typeof(string));

            DataRow drCustom = dtFormat.NewRow();
            drCustom["value"] = 0;
            drCustom["format"] = "";
            dtFormat.Rows.Add(drCustom);

            //List<string> lstCustom = System.Enum.GetNames(typeof(CommonHelper.CustomField)).ToList();
            List<string> lstCustom = System.Enum.GetNames(typeof(CommonHelper.CustomFieldFormat)).ToList();

            int i = 0;
            foreach (var lst in lstCustom)
            {
                i = i + 1;
                drCustom = dtFormat.NewRow();
                drCustom["value"] = i;
                drCustom["format"] = lst;

                dtFormat.Rows.Add(drCustom);
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    private void BindCustomGrid(bool isRefresh = false)
    {
        try
        {

            DataTable dt = new DataTable();
            if (isRefresh)
            {
                objPropGeneral.Screen = "Estimate";
                objPropGeneral.ScreenRefID = 0;// In case of setup
                objPropGeneral.ConnConfig = Session["config"].ToString();
                var ds = objBL_General.GetScreenCustomFields(objPropGeneral);
                dt = ds.Tables[0];
                if (ds.Tables[2].Rows.Count > 0)
                {
                    if (!string.IsNullOrEmpty(ds.Tables[2].Rows[0]["MaxLine"].ToString()))
                    {
                        ViewState["maxLine"] = Convert.ToInt16(ds.Tables[2].Rows[0]["MaxLine"].ToString());
                    }
                    else
                    {
                        ViewState["maxLine"] = 0;
                    }
                }
                else
                {
                    ViewState["maxLine"] = 0;
                }

                if (dt.Rows.Count == 0)
                {
                    DataRow dr = dt.NewRow();
                    dr["ID"] = 0;
                    dr["Label"] = DBNull.Value;
                    dr["Line"] = 0;
                    dr["Value"] = DBNull.Value;
                    dr["Format"] = 0;
                    dr["OrderNo"] = 0;
                    dr["IsAlert"] = 0;
                    dr["TeamMember"] = DBNull.Value;
                    dr["TeamMemberDisplay"] = DBNull.Value;

                    dt.Rows.Add(dr);
                }
                ViewState["CustomTable"] = dt;
                ViewState["CustomValues"] = ds.Tables[1];
            }
            else
            {
                if (ViewState["CustomTable"] != null)
                    dt = (DataTable)ViewState["CustomTable"];
                else
                {
                    objPropGeneral.Screen = "Estimate";
                    objPropGeneral.ScreenRefID = 0;// In case of setup
                    objPropGeneral.ConnConfig = Session["config"].ToString();
                    var ds = objBL_General.GetScreenCustomFields(objPropGeneral);
                    dt = ds.Tables[0];
                    if (ds.Tables[2].Rows.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(ds.Tables[2].Rows[0]["MaxLine"].ToString()))
                        {
                            ViewState["maxLine"] = Convert.ToInt16(ds.Tables[2].Rows[0]["MaxLine"].ToString());
                        }
                        else
                        {
                            ViewState["maxLine"] = 0;
                        }
                    }
                    else
                    {
                        ViewState["maxLine"] = 0;
                    }

                    if (dt.Rows.Count == 0)
                    {
                        DataRow dr = dt.NewRow();
                        dr["ID"] = 0;
                        dr["Label"] = DBNull.Value;
                        dr["Line"] = 0;
                        dr["Value"] = DBNull.Value;
                        dr["Format"] = 0;
                        dr["OrderNo"] = 0;
                        dr["IsAlert"] = 0;
                        dr["TeamMember"] = DBNull.Value;
                        dr["TeamMemberDisplay"] = DBNull.Value;

                        dt.Rows.Add(dr);
                    }
                    ViewState["CustomTable"] = dt;
                    ViewState["CustomValues"] = ds.Tables[1];
                }
            }

            gvCustom.DataSource = dt;
            gvCustom.DataBind();

            if (gvCustom.Rows.Count > 0)
                ((Label)gvCustom.FooterRow.FindControl("lblRowCount")).Text = "Total Line Items: " + Convert.ToString(dt.Rows.Count);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    #region Custom

    protected void gvCustom_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandArgument != null && e.CommandArgument.ToString() != "")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = gvCustom.Rows[index];

                if (e.CommandName.Equals("AddCustomValue"))
                {
                    TextBox txtCustomValue = (TextBox)row.FindControl("txtCustomValue");
                    DropDownList ddlCustomValue = (DropDownList)row.FindControl("ddlCustomValue");
                    LinkButton lnkUpdateCustomValue = (LinkButton)row.FindControl("lnkUpdateCustomValue");
                    LinkButton lnkDelCustomValue = (LinkButton)row.FindControl("lnkDelCustomValue");

                    if (txtCustomValue.Text.Trim() != string.Empty)
                    {
                        ddlCustomValue.Items.Add(new ListItem(txtCustomValue.Text.Trim(), txtCustomValue.Text.Trim()));
                        txtCustomValue.Text = string.Empty;
                    }
                }
                else if (e.CommandName.Equals("UpdateCustomValue"))
                {
                    TextBox txtCustomValue = (TextBox)row.FindControl("txtCustomValue");
                    DropDownList ddlCustomValue = (DropDownList)row.FindControl("ddlCustomValue");
                    if (txtCustomValue.Text.Trim() != string.Empty)
                    {
                        ddlCustomValue.Items.Remove(new ListItem(ddlCustomValue.SelectedValue, ddlCustomValue.SelectedValue));
                        ddlCustomValue.Items.Add(new ListItem(txtCustomValue.Text.Trim(), txtCustomValue.Text.Trim()));
                        ddlCustomValue.SelectedValue = txtCustomValue.Text.Trim();
                    }
                }
                else if (e.CommandName.Equals("DeleteCustomValue"))
                {
                    LinkButton lnkDelCustomValue = (LinkButton)row.FindControl("lnkDelCustomValue");
                    TextBox txtCustomValue = (TextBox)row.FindControl("txtCustomValue");
                    DropDownList ddlCustomValue = (DropDownList)row.FindControl("ddlCustomValue");
                    LinkButton lnkAddCustomValue = (LinkButton)row.FindControl("lnkAddCustomValue");
                    LinkButton lnkUpdateCustomValue = (LinkButton)row.FindControl("lnkUpdateCustomValue");

                    ddlCustomValue.Items.Remove(new ListItem(ddlCustomValue.SelectedValue, ddlCustomValue.SelectedValue));
                    ddlCustomValue.SelectedIndex = 0;
                    lnkAddCustomValue.Visible = true;
                    lnkUpdateCustomValue.Visible = false;
                    lnkDelCustomValue.Visible = false;
                    txtCustomValue.Text = string.Empty;
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void gvCustom_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DropDownList ddlFormat = (DropDownList)e.Row.FindControl("ddlFormat");
            DropDownList ddlTab = (DropDownList)e.Row.FindControl("ddlTab");

            Panel pnlCustomValue = (Panel)e.Row.FindControl("pnlCustomValue");
            if (ddlFormat.SelectedItem.Text == "Dropdown")
                pnlCustomValue.Visible = true;
            else
                pnlCustomValue.Visible = false;

            DropDownList ddlCustomValue = (DropDownList)e.Row.FindControl("ddlCustomValue");
            //Label lblID = (Label)e.Row.FindControl("lblID");
            Label lblLine = (Label)e.Row.FindControl("lblLine");
            //TextBox txtMembers = (TextBox)e.Row.FindControl("txtMembers");
            //txtMembers.Attributes.Add("onclick", "ShowTeamMemberWindow("+ lblLine.Text + ", '"+ txtMembers.Text.Replace(" ","") + "')");

            if (ViewState["CustomValues"] != null)
            {
                DataTable dtCustomval = (DataTable)ViewState["CustomValues"];
                DataTable dt = dtCustomval.Clone();
                ////tblCustomTabID = " + Convert.ToInt32(lblID.Text) + " AND
                //DataRow[] result = dtCustomval.Select("Line = " + (e.Row.RowIndex + 1) + "");
                DataRow[] result = dtCustomval.Select("Line = " + Convert.ToInt32(lblLine.Text) + "");
                foreach (DataRow row in result)
                {
                    dt.ImportRow(row);
                }

                if (dt.Rows.Count > 0)
                {
                    dt.DefaultView.Sort = "Value  ASC";
                    dt = dt.DefaultView.ToTable();
                }

                ddlCustomValue.DataSource = dt;
                ddlCustomValue.DataTextField = "Value";
                ddlCustomValue.DataValueField = "Value";
                ddlCustomValue.DataBind();
                ddlCustomValue.Items.Insert(0, (new ListItem("--Add New--", "")));
            }

            //if (ddlJobType.SelectedValue.Equals("0"))
            //{
            //    ddlTab.Enabled = false;
            //    ddlTab.SelectedValue = "0";
            //}
        }
    }

    protected void lnkDelCustomValue_Click(object sender, EventArgs e)
    {
        try
        {
            LinkButton lnkDelete = (LinkButton)sender;
            GridViewRow row = (GridViewRow)lnkDelete.NamingContainer;
            TextBox txtCustomValue = (TextBox)row.FindControl("txtCustomValue");
            DropDownList ddlCustomValue = (DropDownList)row.FindControl("ddlCustomValue");
            LinkButton lnkAddCustomValue = (LinkButton)row.FindControl("lnkAddCustomValue");
            LinkButton lnkUpdateCustomValue = (LinkButton)row.FindControl("lnkUpdateCustomValue");

            ddlCustomValue.Items.Remove(new ListItem(ddlCustomValue.SelectedValue, ddlCustomValue.SelectedValue));
            ddlCustomValue.SelectedIndex = 0;
            lnkAddCustomValue.Visible = true;
            lnkUpdateCustomValue.Visible = false;
            lnkDelete.Visible = false;
            txtCustomValue.Text = string.Empty;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void lnkUpdateCustomValue_Click(object sender, EventArgs e)
    {
        try
        {
            LinkButton lnkUpdate = (LinkButton)sender;
            GridViewRow row = (GridViewRow)lnkUpdate.NamingContainer;
            TextBox txtCustomValue = (TextBox)row.FindControl("txtCustomValue");
            DropDownList ddlCustomValue = (DropDownList)row.FindControl("ddlCustomValue");
            if (txtCustomValue.Text.Trim() != string.Empty)
            {
                ddlCustomValue.Items.Remove(new ListItem(ddlCustomValue.SelectedValue, ddlCustomValue.SelectedValue));
                ddlCustomValue.Items.Add(new ListItem(txtCustomValue.Text.Trim(), txtCustomValue.Text.Trim()));
                ddlCustomValue.SelectedValue = txtCustomValue.Text.Trim();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void ddlFormat_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            DropDownList ddl = (DropDownList)sender;
            GridViewRow row = (GridViewRow)ddl.NamingContainer;
            Panel pnlCustomValue = (Panel)row.FindControl("pnlCustomValue");
            if (row != null)
            {
                if (ddl.SelectedItem.Text == "Dropdown")
                    pnlCustomValue.Visible = true;
                else
                    pnlCustomValue.Visible = false;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void lnkAddCustomValue_Click(object sender, EventArgs e)
    {
        try
        {
            LinkButton lnkAdd = (LinkButton)sender;
            GridViewRow row = (GridViewRow)lnkAdd.NamingContainer;
            TextBox txtCustomValue = (TextBox)row.FindControl("txtCustomValue");
            DropDownList ddlCustomValue = (DropDownList)row.FindControl("ddlCustomValue");
            LinkButton lnkUpdateCustomValue = (LinkButton)row.FindControl("lnkUpdateCustomValue");
            LinkButton lnkDelCustomValue = (LinkButton)row.FindControl("lnkDelCustomValue");
            if (txtCustomValue.Text.Trim() != string.Empty)
            {
                ddlCustomValue.Items.Add(new ListItem(txtCustomValue.Text.Trim(), txtCustomValue.Text.Trim()));
                txtCustomValue.Text = string.Empty;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void lnkAddnewRow_Click(object sender, EventArgs e)
    {
        try
        {
            CustTempGridData(false);
            DataTable dt = new DataTable();
            dt = (DataTable)ViewState["CustomTable"];
            DataRow dr = dt.NewRow();

            dr["ID"] = 0;
            dr["Label"] = DBNull.Value;
            dr["Line"] = 0;
            //dr["Value"] = DBNull.Value;
            dr["Format"] = 0;
            dr["OrderNo"] = dt.Rows.Count + 1;
            dr["IsAlert"] = 0;
            dr["TeamMember"] = DBNull.Value;
            dr["TeamMemberDisplay"] = DBNull.Value;
            dt.Rows.Add(dr);

            ViewState["CustomTable"] = dt;
            BindCustomGrid();
            //BindCustomDropDown();
            ReorderGridRow();
        }
        catch (Exception ex)
        {
            string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(ex.Message);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void ReorderGridRow()
    {
        int count = 0;
        foreach (GridViewRow gr in gvCustom.Rows)
        {
            HiddenField OrderNo = (HiddenField)gr.FindControl("txtRowLine");
            //Label OrderNo = (Label)gr.FindControl("lblIndex");
            OrderNo.Value = (count = count + 1).ToString();
        }
    }

    protected void ibtnDeleteCItem_Click(object sender, EventArgs e)
    {
        try
        {
            DeleteCustItem();
            ReorderGridRow();
        }
        catch (Exception ex)
        {
            string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(ex.Message);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }

    }

    protected void ddlCustomValue_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            DropDownList ddl = (DropDownList)sender;
            GridViewRow row = (GridViewRow)ddl.NamingContainer;
            LinkButton lnkAddCustomValue = (LinkButton)row.FindControl("lnkAddCustomValue");
            LinkButton lnkUpdateCustomValue = (LinkButton)row.FindControl("lnkUpdateCustomValue");
            LinkButton lnkDelCustomValue = (LinkButton)row.FindControl("lnkDelCustomValue");
            TextBox txtCustomValue = (TextBox)row.FindControl("txtCustomValue");
            if (ddl.SelectedIndex == 0)
            {
                lnkAddCustomValue.Visible = true;
                lnkUpdateCustomValue.Visible = false;
                lnkDelCustomValue.Visible = false;
                txtCustomValue.Text = string.Empty;
            }
            else
            {
                lnkAddCustomValue.Visible = false;
                lnkUpdateCustomValue.Visible = true;
                lnkDelCustomValue.Visible = true;
                txtCustomValue.Text = ddl.SelectedValue;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void CustTempGridData(bool isValidation = true)
    {
        try
        {
            DataTable dt = (DataTable)ViewState["CustomTable"];
            DataTable dtDetails = dt.Clone();
            //If Format type is Dropdown then we store  Default item values in  dtCustomValues table for Dropdown
            DataTable dtCustomValues = new DataTable();
            dtCustomValues.Columns.Add("ID", typeof(int));
            dtCustomValues.Columns.Add("Label", typeof(string));
            dtCustomValues.Columns.Add("Line", typeof(Int16));
            dtCustomValues.Columns.Add("Value", typeof(string));
            dtCustomValues.Columns.Add("Format", typeof(Int16));

            int line = 0;
            var tempLine = 0;
            //var curLine = 0;
            foreach (GridViewRow gr in gvCustom.Rows)
            {
                line++;
                Label lblID = (Label)gr.FindControl("lblID");
                Label lblIndex = (Label)gr.FindControl("lblIndex");
                Label lblLine = (Label)gr.FindControl("lblLine");
                TextBox lblDesc = (TextBox)gr.FindControl("lblDesc");
                HiddenField OrderNo = (HiddenField)gr.FindControl("txtRowLine");
                DropDownList ddlFormat = (DropDownList)gr.FindControl("ddlFormat");
                DropDownList ddlCustomValue = (DropDownList)gr.FindControl("ddlCustomValue");
                CheckBox chkSelectAlert = (CheckBox)gr.FindControl("chkSelectAlert");
                HiddenField hdnMembers = (HiddenField)gr.FindControl("hdnMembers");
                TextBox txtMembers = (TextBox)gr.FindControl("txtMembers");

                if (isValidation)
                {
                    if (!string.IsNullOrEmpty(lblDesc.Text.ToString()) && !ddlFormat.SelectedValue.Equals("0"))
                    {
                        if (lblLine != null && lblLine.Text != "0")
                        {
                            tempLine = Convert.ToInt16(lblLine.Text);
                            //curLine = Convert.ToInt16(lblLine.Text);
                        }
                        else
                        {

                            if (Convert.ToInt16(ViewState["maxLine"]) < line)
                            {
                                ViewState["maxLine"] = line;

                            }
                            else
                            {
                                line = Convert.ToInt16(ViewState["maxLine"]) + 1;
                                ViewState["maxLine"] = line;
                            }
                            tempLine = line;

                            //curLine = tempLine;
                        }

                        foreach (ListItem li in ddlCustomValue.Items)
                        {
                            if (li.Value != string.Empty)
                            {
                                DataRow drCustomVal = dtCustomValues.NewRow();
                                drCustomVal["Label"] = lblDesc.Text;
                                drCustomVal["Line"] = tempLine;
                                drCustomVal["Value"] = li.Value;
                                drCustomVal["Format"] = ddlFormat.SelectedValue;
                                dtCustomValues.Rows.Add(drCustomVal);
                            }
                        }
                        //custom items values of Grid
                        DataRow dr = dtDetails.NewRow();
                        dr["ID"] = Convert.ToInt32(lblID.Text);
                        dr["Label"] = lblDesc.Text.Trim();
                        dr["Line"] = tempLine;
                        dr["Format"] = ddlFormat.SelectedValue;
                        dr["OrderNo"] = Convert.ToInt32(OrderNo.Value);
                        dr["IsAlert"] = chkSelectAlert.Checked;
                        dr["TeamMember"] = hdnMembers.Value;
                        dr["TeamMemberDisplay"] = txtMembers.Text;
                        dtDetails.Rows.Add(dr);
                        //line++;
                    }
                    else if (string.IsNullOrEmpty(lblDesc.Text.ToString()) && !ddlFormat.SelectedValue.Equals("0"))
                    {
                        throw new Exception("Workflow label cannot be empty");
                    }
                    else if (ddlFormat.SelectedValue.Equals("0") && !string.IsNullOrEmpty(lblDesc.Text.ToString()))
                    {
                        throw new Exception("Please select a type for workflow format");
                    }
                }
                else
                {
                    if (lblLine != null && lblLine.Text != "0")
                    {
                        tempLine = Convert.ToInt16(lblLine.Text);
                        //curLine = Convert.ToInt16(lblLine.Text);
                    }
                    else
                    {

                        if (Convert.ToInt16(ViewState["maxLine"]) < line)
                        {
                            ViewState["maxLine"] = line;

                        }
                        else
                        {
                            line = Convert.ToInt16(ViewState["maxLine"]) + 1;
                            ViewState["maxLine"] = line;
                        }
                        tempLine = line;

                        //curLine = tempLine;
                    }

                    foreach (ListItem li in ddlCustomValue.Items)
                    {
                        if (li.Value != string.Empty)
                        {
                            DataRow drCustomVal = dtCustomValues.NewRow();
                            drCustomVal["Label"] = lblDesc.Text;
                            drCustomVal["Line"] = tempLine;
                            drCustomVal["Value"] = li.Value;
                            drCustomVal["Format"] = ddlFormat.SelectedValue;
                            dtCustomValues.Rows.Add(drCustomVal);
                        }
                    }
                    //custom items values of Grid
                    DataRow dr = dtDetails.NewRow();
                    dr["ID"] = Convert.ToInt32(lblID.Text);
                    dr["Label"] = lblDesc.Text.Trim();
                    dr["Line"] = tempLine;
                    dr["Format"] = ddlFormat.SelectedValue;
                    dr["OrderNo"] = Convert.ToInt32(OrderNo.Value);
                    dr["IsAlert"] = chkSelectAlert.Checked;
                    dr["TeamMember"] = hdnMembers.Value;
                    dr["TeamMemberDisplay"] = txtMembers.Text;
                    dtDetails.Rows.Add(dr);
                }
            }
            ViewState["CustomTable"] = dtDetails;
            ViewState["CustomValues"] = dtCustomValues;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    /// <summary>
    /// Delete Custom field item from Custom gridview
    /// </summary>
    private void DeleteCustItem()
    {
        try
        {
            CustTempGridData(false);

            DataTable dt = new DataTable();
            dt = (DataTable)ViewState["CustomTable"];
            DataTable dtdeleted = dt.Clone();
            int count = 0;
            foreach (GridViewRow gr in gvCustom.Rows)
            {
                CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");
                Label lblIndex = (Label)gr.FindControl("lblIndex");
                int index = Convert.ToInt32(lblIndex.Text) - 1;
                if (chkSelect.Checked == true)
                {
                    if (dt.Rows.Count > 0)
                    {
                        dtdeleted.ImportRow(dt.Rows[index - count]);
                        dt.Rows.RemoveAt(index - count);
                    }

                    count++;
                }
            }

            ViewState["CustomDeletedRows"] = dtdeleted;

            if (dt.Rows.Count == 0)
            {
                DataRow dr = dt.NewRow();
                dr["ID"] = 0;
                dr["Label"] = DBNull.Value;
                dr["Line"] = dt.Rows.Count + 1;
                dr["Value"] = DBNull.Value;
                dr["Format"] = 0;
                dr["OrderNo"] = 0;
                dr["IsAlert"] = 0;
                dr["TeamMember"] = DBNull.Value;
                dr["TeamMemberDisplay"] = DBNull.Value;
                dt.Rows.Add(dr);
            }

            ViewState["CustomTable"] = dt;
            BindCustomGrid();
            BindCustomDropDown();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    /// <summary>
    /// Bind Custom drop down
    /// </summary>
    private void BindCustomDropDown()
    {
        int rowIndex = 1;
        try
        {
            foreach (GridViewRow gr in gvCustom.Rows)
            {
                DropDownList ddlFormat = (DropDownList)gr.FindControl("ddlFormat");
                Label lblLine = (Label)gr.FindControl("lblLine");
                Panel pnlCustomValue = (Panel)gr.FindControl("pnlCustomValue");
                if (ddlFormat.SelectedItem.Text == "Dropdown")
                {
                    pnlCustomValue.Visible = true;
                }
                else
                    pnlCustomValue.Visible = false;

                DropDownList ddlCustomValue = (DropDownList)gr.FindControl("ddlCustomValue");
                //Label lblID = (Label)gr.FindControl("lblID");

                if (ViewState["CustomValues"] != null)
                {
                    DataTable dtCustomval = (DataTable)ViewState["CustomValues"];
                    DataTable dt = dtCustomval.Clone();
                    //DataRow[] result = dtCustomval.Select("ItemID = " + Convert.ToInt32(lblID.Text) + "");
                    DataRow[] result = dtCustomval.Select("Line = " + Convert.ToInt32(lblLine.Text) + "");
                    foreach (DataRow row in result)
                    {
                        dt.ImportRow(row);
                    }

                    if (dt.Rows.Count > 0)
                    {
                        dt.DefaultView.Sort = "Value  ASC";
                        dt = dt.DefaultView.ToTable();
                    }

                    ddlCustomValue.DataSource = dt;
                    ddlCustomValue.DataTextField = "Value";
                    ddlCustomValue.DataValueField = "Value";
                    ddlCustomValue.DataBind();
                    ddlCustomValue.Items.Insert(0, (new ListItem("--Add New--", "")));
                    if (ddlFormat.SelectedItem.Text == "Dropdown")
                    {
                        rowIndex++;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public bool ShouldApplySortFilterOrGroup()
    {
        return RadGrid_Emails.MasterTableView.FilterExpression != "" ||
            RadGrid_Emails.MasterTableView.GroupByExpressions.Count > 0 ||
            RadGrid_Emails.MasterTableView.SortExpressions.Count > 0;
    }

    protected void RadGrid_Emails_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        RadGrid_Emails.AllowCustomPaging = !ShouldApplySortFilterOrGroup();
        if (!IsPostBack)
        {

            if (Session["Emails_FilterExpression"] != null && Convert.ToString(Session["Emails_FilterExpression"]) != "" && Session["Emails_Filters"] != null)
            {
                RadGrid_Emails.MasterTableView.FilterExpression = Convert.ToString(Session["Emails_FilterExpression"]);
                var filtersGet = Session["Emails_Filters"] as List<RetainFilter>;
                if (filtersGet != null)
                {
                    foreach (var _filter in filtersGet)
                    {
                        GridColumn column = RadGrid_Emails.MasterTableView.GetColumnSafe(_filter.FilterColumn);
                        column.CurrentFilterValue = _filter.FilterValue;
                    }
                }
            }

        }

        InitTeamMemberGridView();
        //FillDistributionList("", "");

    }

    protected void RadGrid_Emails_PreRender(object sender, EventArgs e)
    {
        String filterExpression = Convert.ToString(RadGrid_Emails.MasterTableView.FilterExpression);
        if (filterExpression != "")
        {
            Session["Emails_FilterExpression"] = filterExpression;
            List<RetainFilter> filters = new List<RetainFilter>();

            foreach (GridColumn column in RadGrid_Emails.MasterTableView.OwnerGrid.Columns)
            {
                String filterValues = column.CurrentFilterValue;
                if (filterValues != "")
                {
                    String columnName = column.UniqueName;
                    RetainFilter filter = new RetainFilter();
                    filter.FilterColumn = columnName;
                    filter.FilterValue = filterValues;
                    filters.Add(filter);
                }
            }

            Session["Emails_Filters"] = filters;
        }
        else
        {
            Session["Emails_FilterExpression"] = null;
            Session["Emails_Filters"] = null;
        }

        ScriptManager.RegisterStartupScript(this, Page.GetType(), "bindingClickCheckbox", "BindClickEventForGridCheckBox();", true);
    }

    private void InitTeamMemberGridView()
    {
        DataSet ds = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();
        //objPropUser.Status = 0;
        //ds = objBL_User.GetUsersForTeamMemberList(objPropUser);
        ds = objBL_User.GetUsersAndRolesForTeamMemberList(objPropUser);
        var teamMembers = ds.Tables[0];

        // Get contacts list from exchange server
        //DataTable contactList = GetContactsForExchServer();
        DataTable contactList = WebBaseUtility.GetContactListOnExchangeServer();
        if (contactList != null && contactList.Rows.Count > 0)
        {
            // Merge this list to teamMembers
            foreach (DataRow item in contactList.Rows)
            {
                DataRow _dr = teamMembers.NewRow();
                if (string.IsNullOrEmpty(item["GroupName"].ToString()))
                {
                    _dr["memberkey"] = "3_" + item["Type"] + "|" + item["MemberEmail"] + "|" + item["MemberName"];
                    _dr["usertype"] = "Exchange " + item["Type"];
                }
                else
                {
                    _dr["memberkey"] = "4_" + item["GroupName"] + "|" + item["MemberEmail"] + "|" + item["MemberName"];
                    _dr["usertype"] = "Exchange Group: " + item["GroupName"];
                }
                _dr["fUser"] = item["MemberName"];
                _dr["email"] = item["MemberEmail"];
                teamMembers.Rows.Add(_dr);
            }
        }

        RadGrid_Emails.DataSource = teamMembers;
        if (teamMembers != null && teamMembers.Rows.Count > 0)
        {
            RadGrid_Emails.VirtualItemCount = teamMembers.Rows.Count;
        }
        else
        {
            RadGrid_Emails.VirtualItemCount = 0;
        }

    }

    protected void ReloadGrid_Click(object sender, EventArgs e)
    {
        InitTeamMemberGridView();
        RadGrid_Emails.Rebind();
    }

    private void UpdateEstimateCustomFields()
    {
        #region Custom

        CustTempGridData();
        DataTable dtCustom = (DataTable)ViewState["CustomTable"];
        if (!dtCustom.Columns.Contains("UpdatedDate"))
        {
            dtCustom.Columns.Add("UpdatedDate", typeof(DateTime));
        }
        if (!dtCustom.Columns.Contains("Username"))
        {
            dtCustom.Columns.Add("Username", typeof(string));
        }
        if (dtCustom.Columns.Contains("FieldControl"))
        {
            dtCustom.Columns.Remove("FieldControl");
        }

        DataTable dtCustomVal = (DataTable)ViewState["CustomValues"];

        DataTable dtCustomDelete = (DataTable)ViewState["CustomDeletedRows"];
        if (dtCustomDelete == null)
        {
            dtCustomDelete = dtCustom.Clone();
        }
        else
        {
            if (!dtCustomDelete.Columns.Contains("UpdatedDate"))
            {
                dtCustomDelete.Columns.Add("UpdatedDate", typeof(DateTime));
            }
            if (!dtCustomDelete.Columns.Contains("Username"))
            {
                dtCustomDelete.Columns.Add("Username", typeof(string));
            }
            if (dtCustomDelete.Columns.Contains("FieldControl"))
            {
                dtCustomDelete.Columns.Remove("FieldControl");
            }
        }


        //dtCustom.AcceptChanges();
        objPropGeneral.CustomItems = dtCustom;
        objPropGeneral.CustomItemsValue = dtCustomVal;
        objPropGeneral.CustomItemsDelete = dtCustomDelete;
        objPropGeneral.Screen = "Estimate";
        objPropGeneral.ConnConfig = Session["config"].ToString();

        objBL_General.UpdateCustomFields(objPropGeneral);

        #endregion
    }

    #endregion


    #region User Custom

    #region "Load data for User Custom"

    private void FillUserCustomFormat()
    {
        try
        {
            dtUserCstFormat = new DataTable();
            dtUserCstFormat.Columns.Add("Value", typeof(string));
            dtUserCstFormat.Columns.Add("Format", typeof(string));

            DataRow drUserCustom = dtUserCstFormat.NewRow();
            //drCustom["Value"] = 0;
            //drCustom["Format"] = "";
            //dtUserCstFormat.Rows.Add(drCustom);

            List<string> lstUserCustom = System.Enum.GetNames(typeof(CommonHelper.CustomField)).ToList();

            int i = 0;
            foreach (var lst in lstUserCustom)
            {
                i = i + 1;
                drUserCustom = dtUserCstFormat.NewRow();
                drUserCustom["Value"] = i;
                drUserCustom["Format"] = lst;

                dtUserCstFormat.Rows.Add(drUserCustom);
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    private DataTable FillMembers(string userName)
    {
        DataTable dt = new DataTable();
        DataSet ds = new DataSet();
        try
        {
            BusinessEntity.User objProp_User = new BusinessEntity.User();
            ds = objBL_User.getTeamByMonUser(Session["config"].ToString(), userName);
            dt = ds.Tables[0];
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
        return dt;
    }

    private void GetUserCustomTable()
    {
        DataSet dst = new DataSet();
        //BL_SafetyTest _objbltesttypes = new BL_SafetyTest();
        BL_UserCustom _objblusercustom = new BL_UserCustom();
        //dst = _objbltesttypes.GetAllUserCustom(Session["config"].ToString(), Session["dbname"].ToString());
        dst = _objblusercustom.GetAllUserCustom(Session["config"].ToString(), Session["dbname"].ToString());

        if (dst.Tables[0].Rows.Count == 0)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ID", typeof(int));
            dt.Columns.Add("Line", typeof(int));
            dt.Columns.Add("OrderNo", typeof(int));
            dt.Columns.Add("Label", typeof(string));
            dt.Columns.Add("IsAlert", typeof(Boolean));
            dt.Columns.Add("TeamMember", typeof(string));
            dt.Columns.Add("Format", typeof(int));
            dt.Columns.Add("TeamMemberDisplay", typeof(string));
            dt.Columns.Add("UserRoles", typeof(string));
            dt.Columns.Add("UserRolesDisplay", typeof(string));
            dt.Columns.Add("UseFormula", typeof(Boolean));
            dt.Columns.Add("Formula", typeof(string));
            dt.Columns.Add("MaxLineNo", typeof(Int16));
            DataRow dr = dt.NewRow();

            dr["ID"] = 0;
            dr["Line"] = dt.Rows.Count + 1;
            dr["OrderNo"] = 0;
            dr["Label"] = "";
            dr["IsAlert"] = 0;
            dr["TeamMember"] = "";
            dr["Format"] = 0;
            dr["TeamMemberDisplay"] = "";
            dr["UserRoles"] = "";
            dr["UserRolesDisplay"] = "";
            dr["UseFormula"] = 0;
            dr["Formula"] = "";
            dr["MaxLineNo"] = 0;
            dt.Rows.Add(dr);

            ViewState["UserCustomTable"] = dt;
            ViewState["UserCustomDeleteTable"] = dt;
        }
        else
        {
            ViewState["UserCustomTable"] = dst.Tables[0];
            ViewState["UserCustomValues"] = dst.Tables[1];
        }
    }

    #endregion

    private void BindUserCustomGrid()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = (DataTable)ViewState["UserCustomTable"];

            gvUserCustom.DataSource = dt;
            gvUserCustom.VirtualItemCount = dt.Rows.Count;
            gvUserCustom.DataBind();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void gvUserCustom_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem item = (GridDataItem)e.Item;
            //HiddenField hdSelectTeam = (HiddenField)item.FindControl("hdSelectTeam");
            DropDownList ddlUserCustomFormat = (DropDownList)item.FindControl("ddlUserCustomFormat");
            //RadComboBox drpMember = (RadComboBox)item.FindControl("ddlTeamMember");

            var formatValue = Convert.ToInt32(DataBinder.Eval(item.DataItem, "Format"));
            ddlUserCustomFormat.SelectedValue = Convert.ToString(formatValue);
            Panel pnlCustomValue = (Panel)item.FindControl("pnlUserCustomValue");

            if (ddlUserCustomFormat.SelectedItem != null && ddlUserCustomFormat.SelectedItem.Text == "Dropdown")
                pnlCustomValue.Visible = true;
            else
                pnlCustomValue.Visible = false;

            DropDownList ddlCustomValue = (DropDownList)item.FindControl("ddlUserCustomValue");
            Label lblID = (Label)item.FindControl("lblID");
            Label lblLine = (Label)item.FindControl("lblLine");

            if (ViewState["UserCustomValues"] != null)
            {
                DataTable dtCustomval = (DataTable)ViewState["UserCustomValues"];
                DataTable dataTemp = dtCustomval.Clone();
                DataRow[] result = dtCustomval.Select("Line = " + Convert.ToInt32(lblLine.Text) + "");
                foreach (DataRow row in result)
                {
                    dataTemp.ImportRow(row);
                }

                if (dataTemp.Rows.Count > 0)
                {
                    dataTemp.DefaultView.Sort = "Value  ASC";
                    dataTemp = dataTemp.DefaultView.ToTable();
                }

                ddlCustomValue.DataSource = dataTemp;
                ddlCustomValue.DataTextField = "Value";
                ddlCustomValue.DataValueField = "Value";
                ddlCustomValue.DataBind();
                ddlCustomValue.Items.Insert(0, (new ListItem("--Add New--", "")));
            }
        }
    }

    private void GetDataOnUserCustomGrid()
    {
        try
        {
            DataTable dt = (DataTable)ViewState["UserCustomTable"];
            DataTable dtDetails = dt.Clone();
            DataTable dtCustomValues = new DataTable();
            dtCustomValues.Columns.Add("ID", typeof(int));
            dtCustomValues.Columns.Add("tblUserCustomFieldsID", typeof(int));
            dtCustomValues.Columns.Add("Line", typeof(int));
            dtCustomValues.Columns.Add("Value", typeof(string));
            //dtCustomValues.Columns.Add("type", typeof(string));
            int line = 1;
            var tempLine = 0;

            foreach (GridDataItem gr in gvUserCustom.Items)
            {
                Label lblID = (Label)gr.FindControl("lblID");
                Label lblIndex = (Label)gr.FindControl("lblIndex");
                Label lblLine = (Label)gr.FindControl("lblLine");
                TextBox txtLabel = (TextBox)gr.FindControl("txtLabel");
                CheckBox chkAlert = (CheckBox)gr.FindControl("chkSelectAlert");
                HiddenField OrderNo = (HiddenField)gr.FindControl("txtOrderNo");
                RadComboBox drpMember = (RadComboBox)gr.FindControl("ddlTeamMember");
                DropDownList drpFormat = (DropDownList)gr.FindControl("ddlUserCustomFormat");
                DropDownList ddlCustomValue = (DropDownList)gr.FindControl("ddlUserCustomValue");

                HiddenField hdSelectTeam = (HiddenField)gr.FindControl("hdnMembers");
                TextBox txtMembers = (TextBox)gr.FindControl("txtMembers");

                HiddenField hdSelectRoles = (HiddenField)gr.FindControl("hdnRoles");
                TextBox txtRoles = (TextBox)gr.FindControl("txtRoles");
                TextBox txtFormula = (TextBox)gr.FindControl("txtFormula");
                CheckBox chkFormula = (CheckBox)gr.FindControl("chkFormula");

                HiddenField hdnMaxLineNo = (HiddenField)gr.FindControl("hdnMaxLineNo");

                if (lblLine != null && lblLine.Text != "0")
                {
                    tempLine = Convert.ToInt16(lblLine.Text);
                }
                else
                {
                    if (Convert.ToInt16(hdnMaxLineNo.Value) < line)
                    {
                        line++;
                    }
                    else
                    {
                        line = Convert.ToInt16(hdnMaxLineNo.Value) + 1;
                    }
                    tempLine = line;
                }

                foreach (ListItem li in ddlCustomValue.Items)
                {
                    if (li.Value != string.Empty)
                    {

                        DataRow drCustomVal = dtCustomValues.NewRow();
                        drCustomVal["ID"] = 0;
                        drCustomVal["tblUserCustomFieldsID"] = Convert.ToInt32(lblID.Text);
                        //drCustomVal["Line"] = lblLine.Text;
                        drCustomVal["Line"] = tempLine;
                        drCustomVal["Value"] = li.Value;
                        dtCustomValues.Rows.Add(drCustomVal);
                    }
                }

                DataRow dr = dtDetails.NewRow();
                dr["ID"] = Convert.ToInt32(lblID.Text);
                //dr["Line"] = lblLine.Text;
                dr["Line"] = tempLine;
                dr["OrderNo"] = Convert.ToInt32(OrderNo.Value);
                dr["Label"] = txtLabel.Text.Trim();
                dr["IsAlert"] = Convert.ToBoolean(chkAlert.Checked);
                //dr["TeamMember"] = hdSelectTeam.Value.Replace("0_", "").Replace("1_", "").Replace("3_", "").Replace("4_", "");
                dr["TeamMember"] = hdSelectTeam.Value;
                dr["Format"] = Convert.ToInt32(drpFormat.SelectedValue);
                dr["TeamMemberDisplay"] = txtMembers.Text;
                dr["UseFormula"] = Convert.ToBoolean(chkFormula.Checked);
                dr["Formula"] = txtFormula.Text;
                dr["MaxLineNo"] = hdnMaxLineNo.Value;
                //dr["UserRoles"] = hdSelectRoles.Value;
                //dr["UserRolesDisplay"] = txtRoles.Text;
                dtDetails.Rows.Add(dr);
                //line++;
            }
            ViewState["UserCustomTable"] = dtDetails;
            ViewState["UserCustomValues"] = dtCustomValues;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    #region "Add and Delete Row"

    protected void lnkAddnewRowUserCustom_Click(object sender, EventArgs e)
    {
        GetDataOnUserCustomGrid();
        DataTable dt = new DataTable();
        dt = (DataTable)ViewState["UserCustomTable"];
        var currMaxLineNo = 0;
        if (dt.Rows.Count > 0)
        {
            currMaxLineNo = Convert.ToInt16(dt.Rows[0]["MaxLineNo"]);
        }

        DataRow dr = dt.NewRow();

        dr["ID"] = 0;
        dr["Line"] = 0;
        dr["OrderNo"] = dt.Rows.Count + 1;
        dr["Label"] = "";
        dr["IsAlert"] = 0;
        dr["TeamMember"] = "";
        dr["Format"] = 0;
        dr["TeamMemberDisplay"] = "";
        dr["UserRoles"] = "";
        dr["UserRolesDisplay"] = "";
        dr["UseFormula"] = 0;
        dr["Formula"] = "";
        dr["MaxLineNo"] = currMaxLineNo;
        dt.Rows.Add(dr);

        ViewState["UserCustomTable"] = dt;

        FillUserCustomFormat();
        BindUserCustomGrid();
        ReorderUserCustomGridRow();
    }

    protected void ibtnDeleteUserCustomItem_Click(object sender, EventArgs e)
    {
        DeleteUserCustomItem();
        ReorderUserCustomGridRow();
    }

    private void DeleteUserCustomItem()
    {
        try
        {
            GetDataOnUserCustomGrid();

            DataTable dt = new DataTable();
            dt = (DataTable)ViewState["UserCustomTable"];
            DataTable dtdeleted = dt.Clone();
            int count = 0;
            foreach (GridDataItem gr in gvUserCustom.Items)
            {
                CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");
                Label lblIndex = (Label)gr.FindControl("lblIndex");
                int index = Convert.ToInt32(lblIndex.Text) - 1;
                if (chkSelect.Checked == true)
                {
                    if (dt.Rows.Count > 0)
                    {
                        dtdeleted.ImportRow(dt.Rows[index - count]);
                        dt.Rows.RemoveAt(index - count);
                    }
                    count++;
                }
            }

            ViewState["UserCustomDeletedRows"] = dtdeleted;

            if (dt.Rows.Count == 0)
            {
                DataRow dr = dt.NewRow();

                dr["ID"] = 0;
                dr["Line"] = 0;
                dr["OrderNo"] = dt.Rows.Count + 1;
                dr["Label"] = "";
                dr["IsAlert"] = 0;
                dr["TeamMember"] = "";
                dr["Format"] = 0;
                dr["TeamMemberDisplay"] = "";
                dr["UserRoles"] = "";
                dr["UserRolesDisplay"] = "";
                dr["UseFormula"] = 0;
                dr["Formula"] = "";
                dr["MaxLineNo"] = 0;
                dt.Rows.Add(dr);
            }

            ViewState["UserCustomTable"] = dt;

            FillUserCustomFormat();
            FillMembers("");
            BindUserCustomGrid();
            BindUserCustomDropDown();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void BindUserCustomDropDown()
    {
        int rowIndex = 1;
        try
        {
            foreach (GridDataItem gr in gvUserCustom.Items)
            {
                DropDownList ddlUserCustomFormat = (DropDownList)gr.FindControl("ddlUserCustomFormat");
                Label lblLine = (Label)gr.FindControl("lblLine");
                Panel pnlCustomValue = (Panel)gr.FindControl("pnlUserCustomValue");
                if (ddlUserCustomFormat.SelectedItem.Text == "Dropdown")
                {
                    pnlCustomValue.Visible = true;
                }
                else
                    pnlCustomValue.Visible = false;

                DropDownList ddlUserCustomValue = (DropDownList)gr.FindControl("ddlUserCustomValue");
                Label lblID = (Label)gr.FindControl("lblID");

                if (ViewState["UserCustomValues"] != null)
                {
                    DataTable dtCustomval = (DataTable)ViewState["UserCustomValues"];
                    DataTable dt = dtCustomval.Clone();
                    DataRow[] result = dtCustomval.Select("Line = " + Convert.ToInt32(lblLine.Text) + "");
                    foreach (DataRow row in result)
                    {
                        dt.ImportRow(row);
                    }

                    if (dt.Rows.Count > 0)
                    {
                        dt.DefaultView.Sort = "Value  ASC";
                        dt = dt.DefaultView.ToTable();
                    }

                    ddlUserCustomValue.DataSource = dt;
                    ddlUserCustomValue.DataTextField = "Value";
                    ddlUserCustomValue.DataValueField = "Value";
                    ddlUserCustomValue.DataBind();
                    ddlUserCustomValue.Items.Insert(0, (new ListItem("--Add New--", "")));
                    if (ddlUserCustomFormat.SelectedItem.Text == "Dropdown")
                    {
                        rowIndex++;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void ReorderUserCustomGridRow()
    {
        int count = 0;
        foreach (GridDataItem gr in gvUserCustom.Items)
        {
            HiddenField OrderNo = (HiddenField)gr.FindControl("txtOrderNo");
            OrderNo.Value = (count = count + 1).ToString();
        }
    }

    #endregion

    #region "User Custom Format"    

    protected void ddlUserCustomFormat_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            DropDownList ddl = (DropDownList)sender;
            GridDataItem row = (GridDataItem)ddl.NamingContainer;
            Panel pnlUserCustomValue = (Panel)row.FindControl("pnlUserCustomValue");
            if (row != null)
            {
                if (ddl.SelectedItem.Text == "Dropdown")
                    pnlUserCustomValue.Visible = true;
                else
                    pnlUserCustomValue.Visible = false;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void ddlUserCustomValue_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            DropDownList ddl = (DropDownList)sender;
            GridDataItem row = (GridDataItem)ddl.NamingContainer;
            LinkButton lnkAddCustomValue = (LinkButton)row.FindControl("lnkAddUserCustomValue");
            LinkButton lnkUpdateCustomValue = (LinkButton)row.FindControl("lnkUpdateUserCustomValue");
            LinkButton lnkDelCustomValue = (LinkButton)row.FindControl("lnkDeleteUserCustomValue");
            TextBox txtCustomValue = (TextBox)row.FindControl("txtUserCustomValue");
            if (ddl.SelectedIndex == 0)
            {
                lnkAddCustomValue.Visible = true;
                lnkUpdateCustomValue.Visible = false;
                lnkDelCustomValue.Visible = false;
                txtCustomValue.Text = string.Empty;
            }
            else
            {
                lnkAddCustomValue.Visible = false;
                lnkUpdateCustomValue.Visible = true;
                lnkDelCustomValue.Visible = true;
                txtCustomValue.Text = ddl.SelectedValue;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    #endregion

    protected void RadComboBox1_ItemDataBound(object sender, RadComboBoxItemEventArgs e)
    {
        e.Item.Text = ((DataRowView)e.Item.DataItem)["MomUserID"].ToString();
        e.Item.Value = ((DataRowView)e.Item.DataItem)["ID"].ToString();
    }

    protected void RadComboBox1_ItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
    {

        RadComboBox obj = sender as RadComboBox;
        obj.Items.Clear();

        obj.ClearCheckedItems();
        obj.ClearSelection();

        obj.DataSource = FillMembers(e.Text);
        obj.DataBind();
    }

    //protected void LnkSaveUserCustom_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        GetDataOnUserCustomGrid();
    //        DataTable dtUserCustom = (DataTable)ViewState["UserCustomTable"];
    //        DataTable dtCustomVal = (DataTable)ViewState["UserCustomValues"];
    //        DataTable dtDeleted = dtUserCustom.Clone();
    //        if (ViewState["UserCustomDeletedRows"] != null)
    //            dtDeleted = (DataTable)ViewState["UserCustomDeletedRows"];

    //        DataColumnCollection UserCustomCols = dtUserCustom.Columns;
    //        if (UserCustomCols.Contains("MaxLineNo"))
    //        {
    //            dtUserCustom.Columns.Remove("MaxLineNo");
    //            dtUserCustom.AcceptChanges();
    //        }

    //        DataColumnCollection dtDeletedCol = dtDeleted.Columns;
    //        if (dtDeletedCol.Contains("MaxLineNo"))
    //        {
    //            dtDeleted.Columns.Remove("MaxLineNo");
    //            dtDeleted.AcceptChanges();
    //        }

    //        UserCustom obj = new UserCustom();
    //        obj.ConnConfig = Session["config"].ToString();
    //        obj.UserCustomItem = dtUserCustom;
    //        obj.UserCustomItemDelete = dtDeleted;
    //        obj.UserCustomValue = dtCustomVal;
    //        BL_SafetyTest _bltest = new BL_SafetyTest();
    //        _bltest.CreateAndUpdateUserCustom(obj);

    //        GetUserCustomTable();
    //        FillUserCustomFormat();
    //        FillMembers("");
    //        BindUserCustomGrid();

    //        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccessMCPS", "noty({text: 'User custom updated successfully', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
    //    }
    //    catch (Exception ex)
    //    {
    //        string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
    //        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyAddWarehousetype1", "noty({text: '" + str + "', dismissQueue: true,  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
    //    }
    //}

    //protected void gvCustom_RowCommand(object sender, GridCommandEventArgs e)
    //{
    //    try
    //    {

    //        GridDataItem item = (GridDataItem)e.Item;
    //        LinkButton lnkAddCustomValue = (LinkButton)item.FindControl("lnkAddUserCustomValue");
    //        LinkButton lnkDelCustomValue = (LinkButton)item.FindControl("lnkDeleteUserCustomValue");
    //        LinkButton lnkUpdateCustomValue = (LinkButton)item.FindControl("lnkUpdateUserCustomValue");


    //        if (e.CommandName.Equals("AddUserCustomValue"))
    //        {
    //            TextBox txtCustomValue = (TextBox)item.FindControl("txtUserCustomValue");
    //            DropDownList ddlCustomValue = (DropDownList)item.FindControl("ddlUserCustomValue");

    //            Boolean isExistItem = false;
    //            foreach (ListItem x in ddlCustomValue.Items)
    //            {
    //                if (x.Text == txtCustomValue.Text.Trim())
    //                {
    //                    isExistItem = true;
    //                    break;
    //                }
    //            }

    //            if (txtCustomValue.Text.Trim() != string.Empty && !isExistItem)
    //            {
    //                ddlCustomValue.Items.Add(new ListItem(txtCustomValue.Text.Trim(), txtCustomValue.Text.Trim()));
    //                txtCustomValue.Text = string.Empty;
    //                ddlCustomValue.SelectedValue = txtCustomValue.Text.Trim();
    //            }
    //        }
    //        else if (e.CommandName.Equals("UpdateUserCustomValue"))
    //        {

    //            TextBox txtCustomValue = (TextBox)item.FindControl("txtUserCustomValue");
    //            DropDownList ddlCustomValue = (DropDownList)item.FindControl("ddlUserCustomValue");
    //            if (txtCustomValue.Text.Trim() != string.Empty)
    //            {
    //                ddlCustomValue.Items.Remove(new ListItem(ddlCustomValue.SelectedValue, ddlCustomValue.SelectedValue));
    //                ddlCustomValue.Items.Add(new ListItem(txtCustomValue.Text.Trim(), txtCustomValue.Text.Trim()));
    //                ddlCustomValue.SelectedValue = txtCustomValue.Text.Trim();

    //            }
    //            lnkAddCustomValue.Visible = true;
    //            lnkUpdateCustomValue.Visible = false;
    //            lnkDelCustomValue.Visible = false;
    //            txtCustomValue.Text = string.Empty;
    //        }
    //        else if (e.CommandName.Equals("DeleteUserCustomValue"))
    //        {
    //            TextBox txtCustomValue = (TextBox)item.FindControl("txtUserCustomValue");
    //            DropDownList ddlCustomValue = (DropDownList)item.FindControl("ddlUserCustomValue");

    //            ddlCustomValue.Items.Remove(new ListItem(ddlCustomValue.SelectedValue, ddlCustomValue.SelectedValue));
    //            ddlCustomValue.SelectedIndex = 0;
    //            lnkAddCustomValue.Visible = true;
    //            lnkUpdateCustomValue.Visible = false;
    //            lnkDelCustomValue.Visible = false;
    //            txtCustomValue.Text = string.Empty;
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    public DataTable RemoveDuplicateRows(DataTable table, string DistinctColumn)
    {
        try
        {
            ArrayList UniqueRecords = new ArrayList();
            ArrayList DuplicateRecords = new ArrayList();
            foreach (DataRow dRow in table.Rows)
            {
                if (UniqueRecords.Contains(dRow[DistinctColumn]))
                    DuplicateRecords.Add(dRow);
                else
                    UniqueRecords.Add(dRow[DistinctColumn]);
            }

            foreach (DataRow dRow in DuplicateRecords)
            {
                table.Rows.Remove(dRow);
            }

            return table;
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    protected void RadComboBox1_DataBound(object sender, EventArgs e)
    {
        RadComboBox obj = sender as RadComboBox;
        //set the initial footer label
        ((Literal)obj.Footer.FindControl("RadComboItemsCount")).Text = Convert.ToString(obj.Items.Count);
    }

    protected void gvUserCustom_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {

            GridDataItem item = (GridDataItem)e.Item;
            LinkButton lnkAddCustomValue = (LinkButton)item.FindControl("lnkAddUserCustomValue");
            LinkButton lnkDelCustomValue = (LinkButton)item.FindControl("lnkDeleteUserCustomValue");
            LinkButton lnkUpdateCustomValue = (LinkButton)item.FindControl("lnkUpdateUserCustomValue");


            if (e.CommandName.Equals("AddUserCustomValue"))
            {
                TextBox txtCustomValue = (TextBox)item.FindControl("txtUserCustomValue");
                DropDownList ddlCustomValue = (DropDownList)item.FindControl("ddlUserCustomValue");

                Boolean isExistItem = false;
                foreach (ListItem x in ddlCustomValue.Items)
                {
                    if (x.Text == txtCustomValue.Text.Trim())
                    {
                        isExistItem = true;
                        break;
                    }
                }

                if (txtCustomValue.Text.Trim() != string.Empty && !isExistItem)
                {
                    ddlCustomValue.Items.Add(new ListItem(txtCustomValue.Text.Trim(), txtCustomValue.Text.Trim()));
                    txtCustomValue.Text = string.Empty;
                    ddlCustomValue.SelectedValue = txtCustomValue.Text.Trim();
                }
            }
            else if (e.CommandName.Equals("UpdateUserCustomValue"))
            {

                TextBox txtCustomValue = (TextBox)item.FindControl("txtUserCustomValue");
                DropDownList ddlCustomValue = (DropDownList)item.FindControl("ddlUserCustomValue");
                if (txtCustomValue.Text.Trim() != string.Empty)
                {
                    ddlCustomValue.Items.Remove(new ListItem(ddlCustomValue.SelectedValue, ddlCustomValue.SelectedValue));
                    ddlCustomValue.Items.Add(new ListItem(txtCustomValue.Text.Trim(), txtCustomValue.Text.Trim()));
                    ddlCustomValue.SelectedValue = txtCustomValue.Text.Trim();

                }
                lnkAddCustomValue.Visible = true;
                lnkUpdateCustomValue.Visible = false;
                lnkDelCustomValue.Visible = false;
                txtCustomValue.Text = string.Empty;
            }
            else if (e.CommandName.Equals("DeleteUserCustomValue"))
            {
                TextBox txtCustomValue = (TextBox)item.FindControl("txtUserCustomValue");
                DropDownList ddlCustomValue = (DropDownList)item.FindControl("ddlUserCustomValue");

                ddlCustomValue.Items.Remove(new ListItem(ddlCustomValue.SelectedValue, ddlCustomValue.SelectedValue));
                ddlCustomValue.SelectedIndex = 0;
                lnkAddCustomValue.Visible = true;
                lnkUpdateCustomValue.Visible = false;
                lnkDelCustomValue.Visible = false;
                txtCustomValue.Text = string.Empty;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void RadGrid_Emails_uc_PreRender(object sender, EventArgs e)
    {
        String filterExpression = Convert.ToString(RadGrid_Emails_uc.MasterTableView.FilterExpression);
        if (filterExpression != "")
        {
            Session["Emails_uc_FilterExpression"] = filterExpression;
            List<RetainFilter> filters = new List<RetainFilter>();

            foreach (GridColumn column in RadGrid_Emails_uc.MasterTableView.OwnerGrid.Columns)
            {
                String filterValues = column.CurrentFilterValue;
                if (filterValues != "")
                {
                    String columnName = column.UniqueName;
                    RetainFilter filter = new RetainFilter();
                    filter.FilterColumn = columnName;
                    filter.FilterValue = filterValues;
                    filters.Add(filter);
                }
            }

            Session["Emails_uc_Filters"] = filters;
        }
        else
        {
            Session["Emails_uc_FilterExpression"] = null;
            Session["Emails_uc_Filters"] = null;
        }

        ScriptManager.RegisterStartupScript(this, Page.GetType(), "bindingClickCheckbox_uc", "BindClickEventForGridCheckBox_uc();", true);
    }

    protected void RadGrid_Emails_uc_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        RadGrid_Emails.AllowCustomPaging = !ShouldApplySortFilterOrGroup();
        if (!IsPostBack)
        {

            if (Session["Emails_uc_FilterExpression"] != null && Convert.ToString(Session["Emails_uc_FilterExpression"]) != "" && Session["Emails_uc_Filters"] != null)
            {
                RadGrid_Emails_uc.MasterTableView.FilterExpression = Convert.ToString(Session["Emails_uc_FilterExpression"]);
                var filtersGet = Session["Emails_Filters"] as List<RetainFilter>;
                if (filtersGet != null)
                {
                    foreach (var _filter in filtersGet)
                    {
                        GridColumn column = RadGrid_Emails_uc.MasterTableView.GetColumnSafe(_filter.FilterColumn);
                        column.CurrentFilterValue = _filter.FilterValue;
                    }
                }
            }

        }

        InitTeamMemberGridView_uc();

    }

    private void InitTeamMemberGridView_uc()
    {
        User objPropUser = new User();
        DataSet ds = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();
        //objPropUser.Status = 0;
        ds = objBL_User.GetUsersAndRolesForTeamMemberList(objPropUser);
        var teamMembers = ds.Tables[0];

        // Get contacts list from exchange server       
        DataTable contactList = WebBaseUtility.GetContactListOnExchangeServer();
        if (contactList != null && contactList.Rows.Count > 0)
        {
            // Merge this list to teamMembers
            foreach (DataRow item in contactList.Rows)
            {
                DataRow _dr = teamMembers.NewRow();
                if (string.IsNullOrEmpty(item["GroupName"].ToString()))
                {
                    _dr["memberkey"] = "3_" + item["Type"] + "|" + item["MemberEmail"] + "|" + item["MemberName"];
                    _dr["usertype"] = "Exchange " + item["Type"];
                }
                else
                {
                    _dr["memberkey"] = "4_" + item["GroupName"] + "|" + item["MemberEmail"] + "|" + item["MemberName"];
                    _dr["usertype"] = "Exchange Group: " + item["GroupName"];
                }
                _dr["fUser"] = item["MemberName"];
                _dr["email"] = item["MemberEmail"];
                teamMembers.Rows.Add(_dr);
            }
        }

        RadGrid_Emails_uc.DataSource = teamMembers;
        if (teamMembers != null && teamMembers.Rows.Count > 0)
        {
            RadGrid_Emails_uc.VirtualItemCount = teamMembers.Rows.Count;
        }
        else
        {
            RadGrid_Emails_uc.VirtualItemCount = 0;
        }

    }

    private void UpdateUserCustomFields()
    {
        try
        {
            GetDataOnUserCustomGrid();
            DataTable dtUserCustom = (DataTable)ViewState["UserCustomTable"];
            DataTable dtCustomVal = (DataTable)ViewState["UserCustomValues"];
            DataTable dtDeleted = dtUserCustom.Clone();

            if (ViewState["UserCustomDeletedRows"] != null)
            {
                dtDeleted = (DataTable)ViewState["UserCustomDeletedRows"];
            }

            DataColumnCollection UserCustomCols = dtUserCustom.Columns;
            if (UserCustomCols.Contains("MaxLineNo"))
            {
                dtUserCustom.Columns.Remove("MaxLineNo");
                dtUserCustom.AcceptChanges();
            }

            DataColumnCollection dtDeletedCol = dtDeleted.Columns;
            if (dtDeletedCol.Contains("MaxLineNo"))
            {
                dtDeleted.Columns.Remove("MaxLineNo");
                dtDeleted.AcceptChanges();
            }

            UserCustom obj = new UserCustom();
            obj.ConnConfig = Session["config"].ToString();
            obj.UserCustomItem = dtUserCustom;
            obj.UserCustomItemDelete = dtDeleted;
            obj.UserCustomValue = dtCustomVal;
            BL_UserCustom _blUserCustom = new BL_UserCustom();
            _blUserCustom.CreateAndUpdateUserCustom(obj);

            GetUserCustomTable();
            FillUserCustomFormat();
            FillMembers("");
            BindUserCustomGrid();

            //ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccessMCPS", "noty({text: 'User custom updated successfully', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyAddWarehousetype1", "noty({text: '" + str + "', dismissQueue: true,  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
    }

    #endregion

}
