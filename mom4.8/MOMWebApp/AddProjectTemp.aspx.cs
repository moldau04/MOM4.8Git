using System;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BusinessLayer;
using BusinessEntity;
using System.Web.Script.Serialization;
using System.Collections.Generic;
using System.IO;
using Telerik.Web.UI;

public partial class AddProjectTemp : System.Web.UI.Page
{
    #region Variables
    protected DataTable dtBomType = new DataTable();

    protected DataTable dtFormat = new DataTable();

    protected DataTable dtTab = new DataTable();

    protected DataTable dtBomItem = new DataTable();

    //protected DataTable dtMat = new DataTable();

    //protected DataTable dtLab = new DataTable();

    Customer objProp_Customer = new Customer();

    BL_Customer objBL_Customer = new BL_Customer();

    JobT _objJob = new JobT();

    BL_Job objBL_Job = new BL_Job();

    Wage _objWage = new Wage();

    BL_User objBL_User = new BL_User();

    GeneralFunctions objGeneral = new GeneralFunctions();

    User objPropUser = new User();

    BL_EstimateTemplate objBL_ET = new BL_EstimateTemplate();

    BusinessEntity.EstimateTemplate objET = new BusinessEntity.EstimateTemplate();

    #endregion

    #region Events

    #region PAGELOAD

    protected void Page_Load(object sender, EventArgs e)
    {
        //try
        //{
        if (Session["userid"] == null)
        {
            Response.Redirect("login.aspx");
        }
        if (!CheckAddEditPermission()) { Response.Redirect("Home.aspx?permission=no"); return; }
        ViewState["mode"] = 0;
        FillBomType();
        FillFormat();
        FillTab();
        //FillInventory();
        if (!IsPostBack)
        {
            BindSalesTax();
            FillJobType();
            FillContractType();
            FillPosting();
            CreateBOMTableEmpty();
            InitMilestoneGrid();
            CreateCustomTable();
            BindCustomGrid();
            //InitTeamMemberGridView();
            Page.Title = "Add Template || MOM";

            if (Session["PageEstimateTemplate"] != null && Session["PageEstimateTemplate"].ToString() == "1")
            {
                lblHeader.Text = "Add Template";
            }

            if (Request.QueryString["uid"] != null)
            {

                if (Session["PageEstimateTemplate"] != null && Session["PageEstimateTemplate"].ToString() == "1")
                {
                    lblHeader.Text = "Edit Template";
                }
                else
                {
                    lblHeader.Text = "Edit Project Template";
                }

                Page.Title = "Edit Template || MOM";
                GetData();
                GetEstimateForms();
                pnlNext.Visible = true;

            }
            else
            {
                tempRev.Visible = false;
                tempRemarks.Visible = false;
            }
            HighlightSideMenu();
            //Permission();
            GetControle();
        }
        if (Request.QueryString["uid"] != null)
        {
            if (Request.QueryString["t"] != null)
            {
                ViewState["mode"] = 0;
                lblHeader.Text = "Copy Project Template";
            }
            else
            {
                ViewState["mode"] = 1;
            }
        }


        #region Hide the Form Tab
        if (Request.QueryString["uid"] == null || Convert.ToString(Request.QueryString["uid"]) == "")
        {
            liForms.Style["display"] = "none";
            adForms.Style["display"] = "none";
        }
        else
        {
            liForms.Style["display"] = "";
            adForms.Style["display"] = "";
        }
        #endregion


        if (Session["pro_up"] != null)
        {
            if (Convert.ToString(Session["pro_up"]) == "a")
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyProj", "noty({text: 'Template Added Successfully!', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : false});", true);
            }
            else if (Convert.ToString(Session["pro_up"]) == "u")
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyProj", "noty({text: 'Template Updated Successfully!', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : false});", true);

            }
            Session["pro_up"] = null;
        }
    }

    #endregion


    private void HighlightSideMenu()
    {
        HyperLink aNav = (HyperLink)Page.Master.FindControl("SalesMgr");
        //li.Style.Add("background", "url(images/active_menu_bg.png) repeat-x");
        aNav.CssClass = "active collapsible-header waves-effect waves-cyan collapsible-height-nl";

        //HyperLink a = (HyperLink)Page.Master.Master.FindControl("SalesLink");
        //a.Style.Add("color", "#2382b2");

        HyperLink lnkUsersSmenu = (HyperLink)Page.Master.FindControl("lnkEstimateTempl");
        //lnkUsersSmenu.Style.Add("color", "#FF7A0A");
        lnkUsersSmenu.Style.Add("color", "#316b9d");
        lnkUsersSmenu.Style.Add("font-weight", "normal");
        lnkUsersSmenu.Style.Add("background-color", "#e5e5e5");
        //AjaxControlToolkit.HoverMenuExtender hm = (AjaxControlToolkit.HoverMenuExtender)Page.Master.Master.FindControl("HoverMenuExtenderSales");
        //hm.Enabled = false;
        HtmlGenericControl div = (HtmlGenericControl)Page.Master.FindControl("SalesMgrSub");
        div.Style.Add("display", "block");
    }

    protected void lnkSaveTemplate_Click(object sender, EventArgs e)
    {
        try
        {

            _objJob.ConnConfig = Session["config"].ToString();

            #region Header

            if (Request.QueryString["uid"] != null)
            {
                _objJob.ID = Convert.ToInt32(Request.QueryString["uid"].ToString());
                _objJob.TemplateRev = txtTempRev.Text;
                _objJob.RevRemarks = txtTempRemarks.Text;
            }

            _objJob.Remarks = txtREPremarks.Text.Trim();
            _objJob.fDesc = txtREPdesc.Text;
            if (!ddlJobType.SelectedValue.Equals("Select Department"))
            {
                _objJob.Type = Convert.ToInt16(ddlJobType.SelectedValue);
            }
            _objJob.Status = Convert.ToInt16(ddlStatus.SelectedValue);

            if (!ddlAlertType.SelectedValue.Equals("Select Type"))
            {
                _objJob.AlertType = Convert.ToInt16(ddlAlertType.SelectedValue);
            }
            if (chkAlert.Checked.Equals(true))
            {
                _objJob.AlertMgr = true;
            }
            else
            {
                _objJob.AlertMgr = false;
            }
            _objJob.MilestoneMgr = true;

            #endregion

            #region Finance
            if (!string.IsNullOrEmpty(uc_InvExpGL._hdnAcctID.Value))
            {
                _objJob.InvExp = Convert.ToInt32(uc_InvExpGL._hdnAcctID.Value);
            }
            if (!string.IsNullOrEmpty(uc_InterestGL._hdnAcctID.Value))
            {
                _objJob.GLInt = Convert.ToInt32(uc_InterestGL._hdnAcctID.Value);
            }

            if (!string.IsNullOrEmpty(hdnInvServiceID.Value))
                _objJob.InvServ = Convert.ToInt32(hdnInvServiceID.Value);
            if (!string.IsNullOrEmpty(hdnPrevilWageID.Value))
                _objJob.Wage = Convert.ToInt32(hdnPrevilWageID.Value);

            if (!string.IsNullOrEmpty(hdnUnrecognizedRevenue.Value))
                _objJob.UnrecognizedRevenue = Convert.ToInt32(hdnUnrecognizedRevenue.Value);
            if (!string.IsNullOrEmpty(hdnUnrecognizedExpense.Value))
                _objJob.UnrecognizedExpense = Convert.ToInt32(hdnUnrecognizedExpense.Value);
            if (!string.IsNullOrEmpty(hdnRetainageReceivable.Value))
                _objJob.RetainageReceivable = Convert.ToInt32(hdnRetainageReceivable.Value);


            if (!ddlContractType.SelectedValue.Equals("0"))
            {
                _objJob.CType = ddlContractType.SelectedValue;
            }
            if (!ddlPostingMethod.SelectedValue.Equals("No data found"))
            {
                _objJob.Post = Convert.ToInt16(ddlPostingMethod.SelectedValue);
            }


            if (chkChargeInt.Checked.Equals(true))
            {
                _objJob.fInt = 1;
            }
            else
                _objJob.fInt = 0;

            if (chkInvoicing.Checked.Equals(true))
            {
                _objJob.JobClose = 1;
            }
            else
                _objJob.JobClose = 0;

            if (chkChargeable.Checked.Equals(true))
            {
                _objJob.Charge = 1;
            }
            else
                _objJob.Charge = 0;

            if (!string.IsNullOrEmpty(txtOHPercentage.Text)) _objJob.OHPer = Convert.ToDouble(txtOHPercentage.Text);
            if (!string.IsNullOrEmpty(txtProfitPercentage.Text)) _objJob.MARKUPPer = Convert.ToDouble(txtProfitPercentage.Text);
            if (!string.IsNullOrEmpty(txtCommissionPercentage.Text)) _objJob.COMMSPer = Convert.ToDouble(txtCommissionPercentage.Text);
            if (ddlSalesTax.SelectedIndex != 0) _objJob.STaxName = ddlSalesTax.SelectedValue;
            _objJob.EstimateType = ddlEstimateType.SelectedValue;
            _objJob.IsSglBilAmt = chkSglBilAmt.Checked;
            _objJob.IsBilFrmBOM = chkBilFrmBOM.Checked;
            #endregion

            #region BOM
            DataTable dtB = GetBOMItems();

            int bline = 1;

            if (ViewState["bLine"] == null)
            {
                dtB.AsEnumerable().ToList()
                    .ForEach(t => t["Line"] = bline++);
                dtB.AcceptChanges();
            }
            else
            {
                bline = (Int16)ViewState["bLine"];
                bline++;
                dtB.Select("Line = 0")
                    .AsEnumerable().ToList()
                    .ForEach(t => t["Line"] = bline++);
                dtB.AcceptChanges();
            }
            for (int i = 0; i < dtB.Rows.Count; i++)
            {
                dtB.Rows[i]["OrderNo"] = i + 1;
            }
            #endregion

            #region Milestones

            DataTable dtM = GetMilestoneItems();
            dtM.Columns.Remove("Department");

            int mline = 1;

            if (ViewState["mLine"] == null)
            {
                dtM.AsEnumerable().ToList()
                    .ForEach(t => t["Line"] = mline++);
                dtM.AcceptChanges();
            }
            else
            {
                mline = (Int16)ViewState["mLine"];
                mline++;
                dtM.Select("Line = 0")
                    .AsEnumerable().ToList()
                    .ForEach(t => t["Line"] = mline++);
                dtM.AcceptChanges();
            }
            for (int i = 0; i < dtM.Rows.Count; i++)
            {
                dtM.Rows[i]["OrderNo"] = i + 1;
            }

            #endregion

            #region Custom

            CustTempGridData();
            DataTable dtCustom = (DataTable)ViewState["CustomTable"];

            DataTable dtCustomVal = (DataTable)ViewState["CustomValues"];

            if (!dtCustom.Columns.Contains("UpdatedDate"))
            {
                dtCustom.Columns.Add("UpdatedDate", typeof(DateTime));
            }
            if (!dtCustom.Columns.Contains("Username"))
            {
                dtCustom.Columns.Add("Username", typeof(string));
            }
            //dtCustom.AcceptChanges();
            _objJob.CustomTabItem = dtCustom;
            _objJob.CustomItem = dtCustomVal;

            #endregion

            if (dtM != null)
            {
                if (dtM.Rows.Count > 0)
                {
                    _objJob.NRev = Convert.ToInt16(dtM.Select("jType = 0").Count());
                    _objJob.NDed = Convert.ToInt16(dtM.Select("jType = 1").Count());
                }
            }
            //GetChecklist();
            int jobid = 0;

            _objJob.TargetHPermission = chkTargetHours.Checked == true ? 1 : 0;

            if (Convert.ToInt32(ViewState["mode"]) == 1)
            {
                DataTable dtDeleted = dtCustom.Clone();

                if (ViewState["CustomDeletedRows"] != null)
                    dtDeleted = (DataTable)ViewState["CustomDeletedRows"];

                if (!dtDeleted.Columns.Contains("UpdatedDate"))
                {
                    dtDeleted.Columns.Add("UpdatedDate", typeof(DateTime));
                }
                if (!dtDeleted.Columns.Contains("Username"))
                {
                    dtDeleted.Columns.Add("Username", typeof(string));
                }

                _objJob.CustomItemDelete = dtDeleted;

                _objJob.ProjectDt = dtB;
                _objJob.MilestoneDt = dtM;


                _objJob.ProjectDt.Columns.Remove("GroupName");
                _objJob.ProjectDt.Columns.Remove("CodeDesc");
                _objJob.ProjectDt.Columns.Remove("txtLabItem");

                _objJob.MilestoneDt.Columns.Remove("GroupName");
                _objJob.MilestoneDt.Columns.Remove("CodeDesc");

                jobid = objBL_Customer.UpdateProjectTemplate(_objJob);
                Session["pro_up"] = "u";
                Response.Redirect(Request.RawUrl, false);
            }
            else
            {
                _objJob.ProjectDt = dtB;
                _objJob.MilestoneDt = dtM;

                _objJob.ProjectDt.Columns.Remove("GroupName");
                _objJob.ProjectDt.Columns.Remove("CodeDesc");
                _objJob.ProjectDt.Columns.Remove("txtLabItem");

                _objJob.MilestoneDt.Columns.Remove("GroupName");
                _objJob.MilestoneDt.Columns.Remove("CodeDesc");

                jobid = objBL_Customer.AddProjectTemplate(_objJob);

                Session["pro_up"] = "a";
                String url = "addprojecttemp.aspx?uid=" + jobid;
                //Redirect by javascript  
                Response.Redirect(url);
            }


        }
        catch (Exception ex)
        {
            if (ex.Message == "Template name already exist. Please use a different template name."
                || ex.Message == "Workflow label cannot be empty"
                || ex.Message == "Please select a type for workflow tab"
                || ex.Message == "Please select a type for workflow format")
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningAddTemplate", "noty({text: '" + ex.Message + "', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : false});", true);
            }
            else
            {
                string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(ex.Message);
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErrProj", "noty({text: '" + str + "',  type : 'error', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            }
        }
    }

    protected void lnkLast_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = (DataTable)Session["ProjectTemp"];
            Response.Redirect("addprojecttemp.aspx?uid=" + dt.Rows[dt.Rows.Count - 1]["ID"]);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void lnkNext_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = (DataTable)Session["ProjectTemp"];
            DataColumn[] keyColumns = new DataColumn[1];
            keyColumns[0] = dt.Columns["ID"];
            dt.PrimaryKey = keyColumns;

            DataRow d = dt.Rows.Find(Request.QueryString["uid"].ToString());
            int index = dt.Rows.IndexOf(d);
            int c = dt.Rows.Count - 1;
            if (index < c)
            {
                Response.Redirect("addprojecttemp.aspx?uid=" + dt.Rows[index + 1]["ID"]);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void lnkPrevious_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = (DataTable)Session["ProjectTemp"];
            DataColumn[] keyColumns = new DataColumn[1];
            keyColumns[0] = dt.Columns["ID"];
            dt.PrimaryKey = keyColumns;

            DataRow d = dt.Rows.Find(Request.QueryString["uid"].ToString());
            int index = dt.Rows.IndexOf(d);

            if (index > 0)
            {
                Response.Redirect("addprojecttemp.aspx?uid=" + dt.Rows[index - 1]["ID"]);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void lnkFirst_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = (DataTable)Session["ProjectTemp"];
            Response.Redirect("addprojecttemp.aspx?uid=" + dt.Rows[0]["ID"]);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void lnkCloseTemplate_Click(object sender, EventArgs e)
    {
        Response.Redirect("projecttemplate.aspx");
    }

    #region Header



    #endregion

    #region BOM


    protected void gvBOM_RowCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        if (e.CommandName.Equals("AddBOMItem"))
        {
            DataTable dt = GetBOMItems();

            string maxvalueLine = "";
            string maxValueOrderNo = "";

            if (dt.Rows.Count == 0)
            {
                maxvalueLine = "1";
                maxValueOrderNo = "1";
            }
            else
            {
                maxvalueLine = dt.AsEnumerable().Max(row => row["Line"]).ToString();
                if (string.IsNullOrEmpty(dt.Rows[0]["OrderNo"].ToString()))
                {
                    maxValueOrderNo = "1";
                }
                else
                {
                    maxValueOrderNo = dt.AsEnumerable().Max(row => row["OrderNo"]).ToString();
                }
            }

            Int32 _line = Convert.ToInt32(maxvalueLine) + 1;

            Int32 _orderNo = Convert.ToInt32(maxValueOrderNo) + 1;

            for (int j = 0; j < 1; j++)
            {
                DataRow dr = dt.NewRow();
                dr["OrderNo"] = _orderNo;
                dr["Line"] = _line;
                dr["QtyReq"] = "0.00";
                dr["BudgetUnit"] = "0.00";
                dr["MatMod"] = "0.00";
                dr["BudgetExt"] = "0.00";
                dr["LabHours"] = "0.00";
                dr["LabRate"] = "0.00";
                dr["LabMod"] = "0.00";
                dr["LabExt"] = "0.00";
                dr["TotalExt"] = "0.00";
                dr["STax"] = 0;
                dr["LSTax"] = 0;
                dt.Rows.Add(dr);
                _line = _line + 1;
                _orderNo = _orderNo + 1;
            }

            ViewState["TempBOM"] = dt;

            BindgvBOM(dt);
        }
    }

    #endregion

    #region Milestone
    protected void gvMilestones_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.Equals("AddMilestoneItem"))
        {
            DataTable dt = GetMilestoneGridItems();


            string maxvalueLine = "";

            string maxValueOrderNo = "";

            if (dt.Rows.Count == 0)
            {
                maxvalueLine = "1";
                maxValueOrderNo = "1";
            }
            else
            {
                maxvalueLine = dt.AsEnumerable().Max(row => row["Line"]).ToString();
                if (string.IsNullOrEmpty(dt.Rows[0]["OrderNo"].ToString()))
                {
                    maxValueOrderNo = "1";
                }
                else
                {
                    maxValueOrderNo = dt.AsEnumerable().Max(row => row["OrderNo"]).ToString();
                }
            }
            Int32 _line = Convert.ToInt32(maxvalueLine) + 1;
            Int32 _orderNo = Convert.ToInt32(maxValueOrderNo) + 1;
            for (int j = 0; j < 1; j++)
            {
                DataRow dr = dt.NewRow();
                dr["OrderNo"] = _orderNo;
                dr["Line"] = _line;
                dr["Amount"] = "0.00";
                dt.Rows.Add(dr);
                _line = _line + 1;
                _orderNo = _orderNo + 1;
            }

            ViewState["TempMilestone"] = dt;
            BindgvMilestones(dt);

        }
    }
    #endregion

    #region Custom

    protected void gvCustom_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
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
        CustTempGridData();
        DataTable dt = new DataTable();
        dt = (DataTable)ViewState["CustomTable"];
        DataRow dr = dt.NewRow();

        dr["ID"] = 0;
        dr["tblTabID"] = 0;
        dr["Label"] = DBNull.Value;
        //dr["Line"] = dt.Rows.Count + 1;
        dr["Line"] = 0;
        dr["Value"] = DBNull.Value;
        dr["Format"] = 0;
        //dr["OrderNo"] = 0;
        dr["OrderNo"] = dt.Rows.Count + 1;
        dr["IsAlert"] = 0;
        dr["IsTask"] = 0;
        dr["TeamMember"] = DBNull.Value;
        dr["TeamMemberDisplay"] = DBNull.Value;
        dt.Rows.Add(dr);

        ViewState["CustomTable"] = dt;
        BindCustomGrid();
        //BindCustomDropDown();
        ReorderGridRow();
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
        DeleteCustItem();
        ReorderGridRow();
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

    #endregion

    protected void lbtnCodeSubmit_Click(object sender, EventArgs e)
    {

    }

    protected void lbtnTypeSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            JobT objJob = new JobT();
            objJob.ConnConfig = Session["config"].ToString();
            objJob.TypeName = txtBomType.Text;
            objJob.TypeId = objBL_Job.AddBOMType(objJob);

            DataSet ds = new DataSet();

            objJob.ConnConfig = Session["config"].ToString();

            ds = objBL_Job.GetBomType(objJob);

            dtBomType = ds.Tables[0];

            foreach (GridDataItem gr in gvBOM.Items)
            {
                DropDownList ddlBType = (DropDownList)gr.FindControl("ddlBType");
                ddlBType.Items.Add(new ListItem(objJob.TypeName.ToString(), objJob.TypeId.ToString()));
            }

            txtBomType.Text = string.Empty;
            mpeAddBomType.Hide();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErrdelete", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : 3000,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void ddlItem_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            DropDownList ddlItem = (DropDownList)sender;
            GridViewRow row = (GridViewRow)ddlItem.NamingContainer;
            TextBox txtScope = (TextBox)row.FindControl("txtScope");
            DropDownList ddlBType = (DropDownList)row.FindControl("ddlBType");

            DataSet ds = new DataSet();
            if (ddlItem.SelectedValue != "0" && !String.IsNullOrEmpty(ddlItem.SelectedValue))
            {
                if (ddlBType.SelectedValue == "1" || ddlBType.SelectedValue == "2")       // select bom type Material
                {
                    if (dtBomItem.Rows.Count.Equals(0) && ddlBType.SelectedValue == "1")
                    {
                        _objJob.ConnConfig = Session["config"].ToString();
                        ds = objBL_Job.GetInventoryItem(_objJob);
                        dtBomItem = ds.Tables[0];
                    }
                    else if (dtBomItem.Rows.Count.Equals(0) && ddlBType.SelectedValue == "2")
                    {
                        //_objWage.ConnConfig = Session["config"].ToString();
                        objPropUser.ConnConfig = Session["config"].ToString();
                        ds = objBL_User.getWage(objPropUser);
                        dtBomItem = ds.Tables[0];
                    }
                    if (dtBomItem.Rows.Count > 0)
                    {
                        DataRow dr = dtBomItem.Select("Value =" + ddlItem.SelectedValue).FirstOrDefault();
                        if (dr != null)
                        {
                            txtScope.Text = dr["fDesc"].ToString();
                        }
                        else
                        {
                            txtScope.Text = "";
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    #endregion

    #region Custom Functions

    public bool CheckAddEditPermission()
    {
        bool result = true;
        if (Session["type"].ToString() != "am" && Session["type"].ToString() != "c")
        {
            DataTable ds = new DataTable();
            // ds = (DataTable)Session["userinfo"];
            ds = GetUserById();
            ///  projecttemp ///////////////////------->

            //projecttemp

            string ProjecttempPermission = ds.Rows[0]["ProjecttempPermission"] == DBNull.Value ? "YYYY" : ds.Rows[0]["ProjecttempPermission"].ToString();
            string stAddPtemp = ProjecttempPermission.Length < 1 ? "Y" : ProjecttempPermission.Substring(0, 1);
            string stEditePtemp = ProjecttempPermission.Length < 2 ? "Y" : ProjecttempPermission.Substring(1, 1);
            string stDeletePtemp = ProjecttempPermission.Length < 3 ? "Y" : ProjecttempPermission.Substring(2, 1);
            string stViewPtemp = ProjecttempPermission.Length < 4 ? "Y" : ProjecttempPermission.Substring(3, 1);

            if (Request.QueryString["uid"] != null)
            {
                if (stViewPtemp == "N")
                {
                    result = false;
                }
                else if (stViewPtemp == "Y" && stEditePtemp == "N")
                {
                    lnkSaveTemplate.Visible = false;
                }
            }
            else
            {
                if (stAddPtemp == "N")
                {
                    result = false;
                }
            }



            //bom
            string BOMPermission = ds.Rows[0]["BOMPermission"] == DBNull.Value ? "YYYY" : ds.Rows[0]["BOMPermission"].ToString();
            string stViewBOM = BOMPermission.Length < 4 ? "Y" : BOMPermission.Substring(3, 1);

            string stEditBOM = BOMPermission.Length < 2 ? "Y" : BOMPermission.Substring(1, 1);

            gvBOM.Visible = stViewBOM == "N" ? false : true;
            hdnBOMPermission.Value = BOMPermission;


            //Milestones
            string MilestonesPermission = ds.Rows[0]["MilestonesPermission"] == DBNull.Value ? "YYYY" : ds.Rows[0]["MilestonesPermission"].ToString();
            string stViewMilestones = MilestonesPermission.Length < 4 ? "Y" : MilestonesPermission.Substring(3, 1);

            hdnMilestonesPermission.Value = MilestonesPermission;
            gvMilestones.Visible = stViewMilestones == "N" ? false : true;


            //Document
            string DocumentPermission = ds.Rows[0]["DocumentPermission"] == DBNull.Value ? "YYYY" : ds.Rows[0]["DocumentPermission"].ToString();
            hdnAddeDocument.Value = DocumentPermission.Length < 1 ? "Y" : DocumentPermission.Substring(0, 1);
            hdnEditeDocument.Value = DocumentPermission.Length < 2 ? "Y" : DocumentPermission.Substring(1, 1);
            hdnDeleteDocument.Value = DocumentPermission.Length < 3 ? "Y" : DocumentPermission.Substring(2, 1);
            hdnViewDocument.Value = DocumentPermission.Length < 4 ? "Y" : DocumentPermission.Substring(3, 1);

            pnlDocPermission.Visible = hdnViewDocument.Value == "N" ? false : true;
        }

        return result;
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
    private void Permission()
    {
        HtmlGenericControl li = (HtmlGenericControl)Page.Master.FindControl("ProjectMgr");
        //li.Style.Add("background", "url(images/active_menu_bg.png) repeat-x");
        li.Attributes.Add("class", "start active open");

        HyperLink a = (HyperLink)Page.Master.FindControl("ProjectLink");
        //a.Style.Add("color", "#2382b2");

        HyperLink lnkUsersSmenu = (HyperLink)Page.Master.FindControl("lnkProjectTempl");
        //lnkUsersSmenu.Style.Add("color", "#FF7A0A");
        lnkUsersSmenu.Style.Add("color", "#316b9d");
        lnkUsersSmenu.Style.Add("font-weight", "normal");
        lnkUsersSmenu.Style.Add("background-color", "#e5e5e5");
        //AjaxControlToolkit.HoverMenuExtender hm = (AjaxControlToolkit.HoverMenuExtender)Page.Master.Master.FindControl("HoverMenuExtenderSales");
        //hm.Enabled = false;
        //HtmlGenericControl ul = (HtmlGenericControl)Page.Master.FindControl("SalesMgrSub");
        ////ul.Attributes.Remove("class");
        //ul.Style.Add("display", "block");
    }

    private void GetData()
    {
        try
        {
            objProp_Customer.ConnConfig = Session["config"].ToString();
            objProp_Customer.ProjectJobID = Convert.ToInt32(Request.QueryString["uid"].ToString());
            DataSet ds = objBL_Customer.getJobTemplateByID(objProp_Customer);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow _dr = ds.Tables[0].Rows[0];
                lblProjectNo.Text = _dr["ID"].ToString();
                txtREPdesc.Text = _dr["fdesc"].ToString();
                lblHeaderLabel.Text = Convert.ToString(_dr["fdesc"]);
                txtREPremarks.Text = _dr["remarks"].ToString();
                ddlJobType.SelectedValue = _dr["Type"].ToString();
                if (Convert.ToString(ddlJobType.SelectedItem.Text) != "")
                {
                    lblHeaderLabel.Text = lblHeaderLabel.Text + " | " + (Convert.ToString(ddlJobType.SelectedItem.Text));
                }
                ddlStatus.SelectedValue = _dr["Status"].ToString();
                hdnInvServiceID.Value = _dr["InvServ"].ToString();
                txtInvService.Text = _dr["InvServiceName"].ToString();
                hdnPrevilWageID.Value = _dr["Wage"].ToString();
                txtPrevilWage.Text = _dr["WageName"].ToString();

                uc_InterestGL._txtGLAcct.Text = _dr["GLName"].ToString();
                uc_InterestGL._hdnAcctID.Value = _dr["GLInt"].ToString();
                uc_InvExpGL._txtGLAcct.Text = _dr["InvExpName"].ToString();
                uc_InvExpGL._hdnAcctID.Value = _dr["InvExp"].ToString();

                //hdnInvExpGLID.Value = _dr["InvExp"].ToString();
                //txtInvExpGL.Text = _dr["InvExpName"].ToString();
                //hdnInterestGLID.Value = _dr["GLInt"].ToString();
                //txtInterestGL.Text = _dr["GLName"].ToString();

                if (!string.IsNullOrEmpty(_dr["CType"].ToString()))
                {
                    ddlContractType.SelectedValue = _dr["CType"].ToString();
                }
                ddlPostingMethod.SelectedValue = _dr["Post"].ToString();

                hdnUnrecognizedRevenue.Value = _dr["UnrecognizedRevenue"].ToString();
                txtUnrecognizedRevenue.Text = _dr["UnrecognizedRevenueName"].ToString();
                hdnUnrecognizedExpense.Value = _dr["UnrecognizedExpense"].ToString();
                txtUnrecognizedExpense.Text = _dr["UnrecognizedExpenseName"].ToString();
                hdnRetainageReceivable.Value = _dr["RetainageReceivable"].ToString();
                txtRetainageReceivable.Text = _dr["RetainageReceivableName"].ToString();

                txtTempRev.Text = _dr["TemplateRev"].ToString();
                txtTempRemarks.Text = _dr["RevRemarks"].ToString();

                txtOHPercentage.Text = _dr["OHPer"].ToString();
                txtCommissionPercentage.Text = _dr["COMMSPer"].ToString();
                txtProfitPercentage.Text = _dr["MARKUPPer"].ToString();
                if (!string.IsNullOrEmpty(_dr["STaxName"].ToString()))
                {
                    ddlSalesTax.SelectedValue = _dr["STaxName"].ToString();
                }
                else
                {
                    ddlSalesTax.SelectedIndex = 0;
                }
                if (!string.IsNullOrEmpty(_dr["EstimateType"].ToString()))
                {
                    ddlEstimateType.SelectedValue = _dr["EstimateType"].ToString();
                }
                else
                {
                    ddlEstimateType.SelectedIndex = 0;
                }
                chkSglBilAmt.Checked = (bool)_dr["IsSglBilAmt"];

                chkBilFrmBOM.Checked = (bool)_dr["IsBilFrmBOM"];
                //txtOHPercentage.Text = _dr["OHPer"].ToString();

                if (_dr["Charge"].ToString().Equals("1"))
                {
                    chkChargeable.Checked = true;
                }
                if (_dr["fInt"].ToString().Equals("1"))
                {
                    chkChargeInt.Checked = true;
                }
                if (_dr["JobClose"].ToString().Equals("1"))
                {
                    chkInvoicing.Checked = true;
                }
                if (Convert.ToBoolean(_dr["AlertMgr"]).Equals(true))
                {
                    chkAlert.Checked = true;
                }
                if (!_dr["AlertType"].ToString().Equals("Select Type"))
                {
                    ddlAlertType.SelectedValue = _dr["AlertType"].ToString();
                }

                if (ds.Tables[0].Rows[0]["TargetHPermission"].ToString() == "1")
                    chkTargetHours.Checked = true;
                else
                    chkTargetHours.Checked = false;

                ViewState["TempBOM"] = ds.Tables[1];
                ViewState["TempMilestone"] = ds.Tables[2];

                if (ds.Tables[1].Rows.Count > 0)
                {
                    // gvBOM.DataSource = ds.Tables[1];
                    // gvBOM.DataBind();
                    BindgvBOM(ds.Tables[1]);
                }
                if (ds.Tables[2].Rows.Count > 0)
                {
                    //gvMilestones.DataSource = ds.Tables[2];
                    //gvMilestones.DataBind();
                    BindgvMilestones(ds.Tables[2]);
                }
                if (ds.Tables[3].Rows.Count > 0)
                {
                    ViewState["CustomTable"] = (DataTable)ds.Tables[3];
                    ViewState["CustomValues"] = (DataTable)ds.Tables[4];

                    gvCustom.DataSource = ds.Tables[3];
                    gvCustom.DataBind();
                }
                if (ds.Tables[5].Rows.Count > 0)
                {
                    if (!string.IsNullOrEmpty(ds.Tables[5].Rows[0]["bLine"].ToString()))
                    {
                        ViewState["bLine"] = Convert.ToInt16(ds.Tables[5].Rows[0]["bLine"].ToString());
                    }
                }
                if (ds.Tables[6].Rows.Count > 0)
                {
                    if (!string.IsNullOrEmpty(ds.Tables[6].Rows[0]["mLine"].ToString()))
                    {
                        ViewState["mLine"] = Convert.ToInt16(ds.Tables[6].Rows[0]["mLine"].ToString());
                    }
                }
                if (ds.Tables[7].Rows.Count > 0)
                {
                    if (!string.IsNullOrEmpty(ds.Tables[7].Rows[0]["cLine"].ToString()))
                    {
                        ViewState["cLine"] = Convert.ToInt16(ds.Tables[7].Rows[0]["cLine"].ToString());
                        ViewState["maxLine"] = Convert.ToInt16(ds.Tables[7].Rows[0]["cLine"].ToString());
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
            }
            else
            {
                ViewState["maxLine"] = 0;
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    #region Header

    private void FillJobType()
    {
        try
        {
            DataSet _dsJob = new DataSet();
            _objJob.ConnConfig = Session["config"].ToString();
            // is recurring job template exist or not
            if (Request.QueryString["uid"] != null)
            {
                _objJob.ID = Convert.ToInt32(Request.QueryString["uid"]);
            }

            _objJob.IsExistRecurr = objBL_Job.IsExistRecurrJobT(_objJob);

            _dsJob = objBL_Job.GetAllJobType(_objJob);
            DataTable jobDt = new DataTable();

            //ViewState["RecurrJobType"] = _dsJob.Tables[0];
            if (_objJob.IsExistRecurr.Equals(true))
            {
                jobDt = _dsJob.Tables[0].Select("ID <> 0").CopyToDataTable();
            }
            else
            {
                jobDt = _dsJob.Tables[0];
            }

            if (_dsJob.Tables[0].Rows.Count > 0)
            {
                ddlJobType.Items.Clear();
                ddlJobType.Items.Add(new ListItem("Select Department", "Select Department"));
                ddlJobType.AppendDataBoundItems = true;
                ddlJobType.DataSource = jobDt;
                ddlJobType.DataValueField = "ID";
                ddlJobType.DataTextField = "Type";
                ddlJobType.DataBind();
            }
            else
            {
                ddlJobType.Items.Add(new ListItem("No data found", "0"));
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    #endregion

    #region Finance

    private void FillContractType()
    {
        try
        {
            DataSet _dsContract = new DataSet();
            //_objJob.ConnConfig = Session["config"].ToString();
            //_dsContract = objBL_Job.GetContractType(_objJob);
            objPropUser.ConnConfig = Session["config"].ToString();
            _dsContract = new BusinessLayer.Programs.BL_ServiceType().GetActiveServiceType(objPropUser.ConnConfig);
            if (_dsContract.Tables[0].Rows.Count > 0)
            {
                ddlContractType.Items.Add(new ListItem("Select Service Type", "0"));
                ddlContractType.AppendDataBoundItems = true;
                ddlContractType.DataSource = _dsContract;
                ddlContractType.DataValueField = "Type";
                ddlContractType.DataTextField = "Type";
                ddlContractType.DataBind();
            }
            else
            {
                ddlContractType.Items.Add(new ListItem("No data found", "0"));
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    private void FillPosting()
    {
        try
        {
            DataSet _dsPost = new DataSet();
            _objJob.ConnConfig = Session["config"].ToString();
            _dsPost = objBL_Job.GetPosting(_objJob);
            if (_dsPost.Tables[0].Rows.Count > 0)
            {
                ddlPostingMethod.DataSource = _dsPost;
                ddlPostingMethod.DataBind();
                ddlPostingMethod.DataValueField = "ID";
                ddlPostingMethod.DataTextField = "Post";
                ddlPostingMethod.DataBind();
            }
            else
            {
                ddlPostingMethod.Items.Add(new ListItem("No data found"));
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    #endregion

    #region BOM

    private void CreateBOMTableEmpty()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("JobT", typeof(int));
        dt.Columns.Add("Job", typeof(int));
        dt.Columns.Add("JobTItemID", typeof(int));
        dt.Columns.Add("fDesc", typeof(string));
        dt.Columns.Add("Code", typeof(string));
        dt.Columns.Add("Line", typeof(int));
        dt.Columns.Add("BType", typeof(int));
        dt.Columns.Add("QtyReq", typeof(double));
        dt.Columns.Add("UM", typeof(string));
        dt.Columns.Add("BudgetUnit", typeof(double));
        dt.Columns.Add("BudgetExt", typeof(double));    // JobTItem.Budget
        dt.Columns.Add("LabItem", typeof(int));         // BOM.LabItem
        dt.Columns.Add("txtLabItem", typeof(string));         // BOM.LabItem
        dt.Columns.Add("MatItem", typeof(int));         // BOM.MatItem
        dt.Columns.Add("MatMod", typeof(double));       // JobTItem.Modifier
        dt.Columns.Add("LabMod", typeof(double));       // JobTItem.ETCMod
        dt.Columns.Add("LabExt", typeof(double));       // JobTItem.ETC
        dt.Columns.Add("LabRate", typeof(double));      // BOM.LabRate
        dt.Columns.Add("LabHours", typeof(double));        //JobTItem.BHours
        dt.Columns.Add("SDate", typeof(DateTime));      // BOM.SDate
        dt.Columns.Add("VendorId", typeof(int));
        dt.Columns.Add("Vendor", typeof(string));
        dt.Columns.Add("TotalExt", typeof(double));
        dt.Columns.Add("MatDesc", typeof(string));
        dt.Columns.Add("OrderNo", typeof(string));
        dt.Columns.Add("GroupId", typeof(int));
        dt.Columns.Add("GroupName", typeof(string));
        dt.Columns.Add("CodeDesc", typeof(string));
        dt.Columns.Add("STax", typeof(int));
        dt.Columns.Add("LSTax", typeof(int));

        //////////
        ///
        //////////

        JobT _objJob = new JobT(); BL_Job _objBLJob = new BL_Job(); DataSet ds = new DataSet(); int DeptID = 0;

        int.TryParse(ddlJobType.SelectedValue.ToString(), out DeptID);
        _objJob.ConnConfig = HttpContext.Current.Session["config"].ToString();
        ds = _objBLJob.GetJobCodebyDept(_objJob, DeptID);
        int count = 1;
        if (ds.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow dr in ds.Tables[0].Rows)
            {

                DataRow drNK = dt.NewRow();
                drNK["Code"] = dr["label"].ToString();
                drNK["CodeDesc"] = dr["CodeDesc"].ToString();
                drNK["Line"] = count; drNK["OrderNo"] = count; drNK["QtyReq"] = "0.00"; drNK["BudgetUnit"] = "0.00"; drNK["MatMod"] = "0.00"; drNK["BudgetExt"] = "0.00"; drNK["LabHours"] = "0.00"; drNK["LabRate"] = "0.00"; drNK["LabMod"] = "0.00"; drNK["LabExt"] = "0.00"; drNK["TotalExt"] = "0.00"; drNK["GroupId"] = 0;
                drNK["STax"] = 0;
                drNK["LSTax"] = 0;
                dt.Rows.Add(drNK);
                count++;


            }

        }
        else
        {
            DataRow dr = dt.NewRow();
            dr["Line"] = 0; dr["OrderNo"] = 1; dr["QtyReq"] = "0.00"; dr["BudgetUnit"] = "0.00"; dr["MatMod"] = "0.00"; dr["BudgetExt"] = "0.00"; dr["LabHours"] = "0.00"; dr["LabRate"] = "0.00"; dr["LabMod"] = "0.00"; dr["LabExt"] = "0.00"; dr["TotalExt"] = "0.00"; dr["GroupId"] = 0;
            dr["STax"] = 0;
            dr["LSTax"] = 0;
            dt.Rows.Add(dr);

            DataRow dr1 = dt.NewRow();
            dr1["Line"] = 0; dr1["OrderNo"] = 2; dr1["QtyReq"] = "0.00"; dr1["BudgetUnit"] = "0.00"; dr1["MatMod"] = "0.00"; dr1["BudgetExt"] = "0.00"; dr1["LabHours"] = "0.00"; dr1["LabRate"] = "0.00"; dr1["LabMod"] = "0.00"; dr1["LabExt"] = "0.00"; dr1["TotalExt"] = "0.00"; dr1["GroupId"] = 0;
            dr["STax"] = 0;
            dr["LSTax"] = 0;
            dt.Rows.Add(dr1);
        }

        BindgvBOM(dt);
    }


    //private DataTable GetBOMGridItems()
    //{
    //    DataTable dt = new DataTable();
    //    dt.Columns.Add("JobT", typeof(int));
    //    dt.Columns.Add("Job", typeof(int));
    //    dt.Columns.Add("JobTItemID", typeof(int));
    //    dt.Columns.Add("fDesc", typeof(string));
    //    dt.Columns.Add("Code", typeof(string));
    //    dt.Columns.Add("Line", typeof(int));
    //    dt.Columns.Add("BType", typeof(int));
    //    dt.Columns.Add("QtyReq", typeof(double));
    //    dt.Columns.Add("UM", typeof(string));
    //    dt.Columns.Add("BudgetUnit", typeof(double));
    //    dt.Columns.Add("BudgetExt", typeof(double));    // JobTItem.Budget
    //    dt.Columns.Add("LabItem", typeof(int));         // BOM.LabItem
    //    dt.Columns.Add("txtLabItem", typeof(string));         // BOM.LabItem
    //    dt.Columns.Add("MatItem", typeof(int));         // BOM.MatItem
    //    dt.Columns.Add("MatMod", typeof(double));       // JobTItem.Modifier
    //    dt.Columns.Add("LabMod", typeof(double));       // JobTItem.ETCMod
    //    dt.Columns.Add("LabExt", typeof(double));       // JobTItem.ETC
    //    dt.Columns.Add("LabRate", typeof(double));      // BOM.LabRate
    //    dt.Columns.Add("LabHours", typeof(double));        //JobTItem.BHours
    //    dt.Columns.Add("SDate", typeof(DateTime));      // BOM.SDate
    //    dt.Columns.Add("VendorId", typeof(int));
    //    dt.Columns.Add("Vendor", typeof(string));
    //    dt.Columns.Add("TotalExt", typeof(double));
    //    dt.Columns.Add("MatDesc", typeof(string));
    //    dt.Columns.Add("OrderNo", typeof(int));
    //    dt.Columns.Add("GroupId", typeof(int));
    //    dt.Columns.Add("GroupName", typeof(string));
    //    dt.Columns.Add("CodeDesc", typeof(string));

    //    string strItems = hdnItemJSON.Value.Trim();
    //    double budgetExt = 0;
    //    double qtyReq = 0;
    //    double labExt = 0;
    //    try
    //    {
    //        if (strItems != string.Empty)
    //        {
    //            JavaScriptSerializer sr = new JavaScriptSerializer();
    //            List<Dictionary<object, object>> objEstimateItemData = new List<Dictionary<object, object>>();
    //            objEstimateItemData = sr.Deserialize<List<Dictionary<object, object>>>(strItems);
    //            int i = 0;
    //            foreach (Dictionary<object, object> dict in objEstimateItemData)
    //            {

    //                qtyReq = 0;
    //                budgetExt = 0;
    //                labExt = 0;
    //                //if (dict["txtScope"].ToString().Trim() == string.Empty)
    //                //{
    //                //    return dt;
    //                //}
    //                i++;
    //                DataRow dr = dt.NewRow();
    //                if (dict["hdnLine"].ToString().Trim() != string.Empty)
    //                {
    //                    dr["Line"] = Convert.ToInt32(dict["hdnLine"].ToString());
    //                }
    //                if (dict["hdnID"].ToString().Trim() != string.Empty)
    //                {
    //                    dr["JobTItemID"] = Convert.ToInt32(dict["hdnID"].ToString());
    //                }
    //                else
    //                {
    //                    dr["JobTItemID"] = 0;
    //                }
    //                dr["fDesc"] = dict["txtScope"].ToString().Trim();
    //                dr["Code"] = dict["txtCode"].ToString().Trim();
    //                //dr["jtype"] = Convert.ToInt16(dict["ddlType"]);

    //                if (dict["ddlBType"].ToString() != "0")
    //                {
    //                    dr["BType"] = Convert.ToInt32(dict["ddlBType"]);
    //                }

    //                int hdntxtLabItem = 0; int.TryParse(dict["hdntxtLabItem"].ToString(), out hdntxtLabItem);

    //                if (!hdntxtLabItem.Equals(0))
    //                {
    //                    dr["LabItem"] = Convert.ToInt32(dict["hdntxtLabItem"]);
    //                }

    //                if ((dict["txtLabItem"]).ToString().Trim() != string.Empty)
    //                {
    //                    dr["txtLabItem"] = (dict["txtLabItem"].ToString().Trim());
    //                }

    //                int _hdnddlMatItemId = 0;

    //                int.TryParse(dict["hdnddlMatItemId"].ToString(), out _hdnddlMatItemId);

    //                if (!_hdnddlMatItemId.Equals(0))
    //                {
    //                    dr["MatItem"] = _hdnddlMatItemId;
    //                }
    //                if (dict["txtQtyReq"].ToString().Trim() != string.Empty && dict["txtQtyReq"].ToString().Trim() != "0.00")
    //                {
    //                    dr["QtyReq"] = Convert.ToDouble(dict["txtQtyReq"]);
    //                }
    //                else
    //                {
    //                    dr["QtyReq"] = 0;
    //                }
    //                if (dict["txtUM"].ToString().Trim() != string.Empty)
    //                {
    //                    dr["UM"] = dict["txtUM"].ToString().Trim();
    //                }
    //                if (!string.IsNullOrEmpty(dict["txtBudgetUnit"].ToString()) && dict["txtBudgetUnit"].ToString().Trim() != "0.00")
    //                {
    //                    dr["BudgetUnit"] = Convert.ToDouble(dict["txtBudgetUnit"]);
    //                }
    //                else
    //                {
    //                    dr["BudgetUnit"] = 0;
    //                }

    //                if (dict["txtBudgetUnit"].ToString().Trim() != string.Empty && !string.IsNullOrEmpty(dict["txtQtyReq"].ToString())
    //                    && dict["txtBudgetUnit"].ToString().Trim() != "0.00" && dict["txtQtyReq"].ToString().Trim() != "0.00")
    //                {
    //                    qtyReq = Convert.ToDouble(dict["txtQtyReq"].ToString());
    //                    if (qtyReq.Equals(0))
    //                    {
    //                        qtyReq = Convert.ToDouble(dict["txtQtyReq"].ToString().Trim());
    //                    }
    //                    budgetExt = qtyReq * Convert.ToDouble(dict["txtBudgetUnit"].ToString());
    //                    dr["BudgetExt"] = budgetExt;
    //                }
    //                else
    //                {
    //                    dr["BudgetExt"] = 0;
    //                }


    //                if (dict["txtMatMod"].ToString().Trim() != string.Empty && dict["txtMatMod"].ToString().Trim() != "0.00")
    //                {
    //                    dr["MatMod"] = Convert.ToDouble(dict["txtMatMod"]);
    //                }
    //                else
    //                {
    //                    dr["MatMod"] = 0;
    //                }
    //                if (dict["txtLabMod"].ToString().Trim() != string.Empty && dict["txtLabMod"].ToString().Trim() != "0.00")
    //                {
    //                    dr["LabMod"] = Convert.ToDouble(dict["txtLabMod"]);
    //                }
    //                else
    //                {
    //                    dr["LabMod"] = 0;
    //                }

    //                if (dict["hdnVendorId"].ToString().Trim() != string.Empty)
    //                {
    //                    dr["VendorId"] = Convert.ToInt32(dict["hdnVendorId"]);
    //                }

    //                if (dict["txtVendor"].ToString().Trim() != string.Empty)
    //                {
    //                    dr["Vendor"] = dict["txtVendor"].ToString();
    //                }

    //                if (dict["txtHours"].ToString().Trim() != string.Empty && dict["txtHours"].ToString().Trim() != "0.00")
    //                {
    //                    dr["LabHours"] = Convert.ToDouble(dict["txtHours"]);
    //                }
    //                else
    //                {
    //                    dr["LabHours"] = 0;
    //                }

    //                if (!string.IsNullOrEmpty(dict["txtLabRate"].ToString()) && dict["txtLabRate"].ToString().Trim() != "0.00")
    //                {
    //                    dr["LabRate"] = dict["txtLabRate"];
    //                }
    //                else
    //                {
    //                    dr["LabRate"] = 0;
    //                }

    //                if (!string.IsNullOrEmpty(dict["txtLabRate"].ToString()) && !string.IsNullOrEmpty(dict["txtHours"].ToString())
    //                    && dict["txtLabRate"].ToString().Trim() != "0.00" && dict["txtHours"].ToString().Trim() != "0.00")
    //                {
    //                    labExt = Convert.ToDouble(dict["txtLabRate"]);
    //                    if (dict["txtHours"].ToString().Trim() != string.Empty)
    //                    {
    //                        labExt = labExt * Convert.ToDouble(dict["txtHours"]);
    //                    }
    //                    dr["LabExt"] = labExt;
    //                }
    //                else
    //                {
    //                    dr["LabExt"] = 0;
    //                }
    //                dr["TotalExt"] = labExt + budgetExt;

    //                if (dict["txtSDate"].ToString().Trim() != string.Empty)
    //                {
    //                    dr["SDate"] = Convert.ToDateTime(dict["txtSDate"]);
    //                }

    //                dr["MatDesc"] = (dict["txtddlMatItem"].ToString());

    //                if (dict["hdnOrderNo"].ToString().Trim() != string.Empty)
    //                {
    //                    dr["OrderNo"] = Convert.ToInt32(dict["hdnOrderNo"].ToString());
    //                }

    //                if (dict["txtGroup"].ToString().Trim() != string.Empty)
    //                {
    //                    dr["GroupName"] = (dict["txtGroup"].ToString());
    //                }

    //                if (dict["hdnGroupID"].ToString().Trim() != string.Empty)
    //                {
    //                    dr["GroupID"] = Convert.ToInt32(dict["hdnGroupID"].ToString());
    //                }
    //                else { dr["GroupID"] = 0; }

    //                if (dict["lblCodeDesc"].ToString().Trim() != string.Empty)
    //                {
    //                    dr["CodeDesc"] = (dict["lblCodeDesc"].ToString());
    //                }

    //                dt.Rows.Add(dr);

    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
    //        ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
    //    }
    //    return dt;
    //}

    private DataTable GetBOMItems()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("JobT", typeof(int));
        dt.Columns.Add("Job", typeof(int));
        dt.Columns.Add("JobTItemID", typeof(int));
        dt.Columns.Add("fDesc", typeof(string));
        dt.Columns.Add("Code", typeof(string));
        dt.Columns.Add("Line", typeof(int));
        dt.Columns.Add("BType", typeof(int));
        dt.Columns.Add("QtyReq", typeof(double));
        dt.Columns.Add("UM", typeof(string));
        dt.Columns.Add("BudgetUnit", typeof(double));
        dt.Columns.Add("BudgetExt", typeof(double));
        dt.Columns.Add("LabItem", typeof(int));         // BOM.LabItem
        dt.Columns.Add("txtLabItem", typeof(string));         // BOM.LabItem
        dt.Columns.Add("MatItem", typeof(int));         // BOM.MatItem
        dt.Columns.Add("MatMod", typeof(double));       // JobTItem.Modifier
        dt.Columns.Add("LabMod", typeof(double));       // JobTItem.ETCMod
        dt.Columns.Add("LabExt", typeof(double));       // JobTItem.ETC
        dt.Columns.Add("LabRate", typeof(double));      // BOM.LabRate
        dt.Columns.Add("LabHours", typeof(double));        //JobTItem.BHours
        dt.Columns.Add("SDate", typeof(DateTime));      // BOM.SDate
        dt.Columns.Add("VendorId", typeof(int));
        dt.Columns.Add("Vendor", typeof(string));
        dt.Columns.Add("TotalExt", typeof(double));
        dt.Columns.Add("MatDesc", typeof(string));
        dt.Columns.Add("OrderNo", typeof(int));
        dt.Columns.Add("GroupId", typeof(int));
        dt.Columns.Add("GroupName", typeof(string));
        dt.Columns.Add("CodeDesc", typeof(string));
        dt.Columns.Add("STax", typeof(int));
        dt.Columns.Add("LSTax", typeof(int));

        string strItems = hdnItemJSON.Value.Trim();
        double budgetExt = 0;
        double _qtyReq = 0;
        double labExt = 0;
        //try
        //{
        if (strItems != string.Empty)
        {
            JavaScriptSerializer sr = new JavaScriptSerializer();
            List<Dictionary<object, object>> objEstimateItemData = new List<Dictionary<object, object>>();
            objEstimateItemData = sr.Deserialize<List<Dictionary<object, object>>>(strItems);
            int i = 0;
            foreach (Dictionary<object, object> dict in objEstimateItemData)
            {
                //bom items
                //DropDownList ddlBType = (DropDownList)item.FindControl("ddlBType");

                //TextBox txtScope = (TextBox)item.FindControl("txtScope");

                if (dict["txtScope"].ToString().Trim() != string.Empty && dict["ddlBType"].ToString() != "0" && dict["ddlBType"].ToString() != "Select Type")
                {
                    _qtyReq = 0;
                    budgetExt = 0;
                    labExt = 0;

                    i++;
                    DataRow dr = dt.NewRow();
                    if (dict["hdnLine"].ToString().Trim() != string.Empty)
                    {
                        dr["Line"] = Convert.ToInt32(dict["hdnLine"].ToString());
                    }

                    if (dict["hdnID"].ToString().Trim() != string.Empty)
                    {
                        dr["JobTItemID"] = Convert.ToInt32(dict["hdnID"].ToString());
                    }
                    else
                    {
                        dr["JobTItemID"] = 0;
                    }

                    //dr["fDesc"] = txtScope.Text;
                    dr["fDesc"] = dict["txtScope"].ToString().Trim();


                    //TextBox txtCode = (TextBox)item.FindControl("txtCode");
                    //dr["Code"] = txtCode.Text;
                    dr["Code"] = dict["txtCode"].ToString().Trim();

                    //if (ddlBType.SelectedValue != "0")
                    //{
                    //    dr["BType"] = Convert.ToInt32(ddlBType.SelectedValue);
                    //}
                    if (dict["ddlBType"].ToString() != "0")
                    {
                        dr["BType"] = Convert.ToInt32(dict["ddlBType"]);
                    }

                    //HiddenField hdntxtLabItem1 = (HiddenField)item.FindControl("hdntxtLabItem");
                    //int hdntxtLabItem = 0; int.TryParse(hdntxtLabItem1.Value.ToString(), out hdntxtLabItem);
                    int hdntxtLabItem = 0; int.TryParse(dict["hdntxtLabItem"].ToString(), out hdntxtLabItem);
                    if (!hdntxtLabItem.Equals(0))
                    {
                        dr["LabItem"] = hdntxtLabItem;
                    }

                    //TextBox txtLabItem = (TextBox)item.FindControl("txtLabItem");
                    //if ((txtLabItem.Text.ToString().Trim() != string.Empty))
                    //{
                    //    dr["txtLabItem"] = txtLabItem.Text;
                    //}
                    if ((dict["txtLabItem"]).ToString().Trim() != string.Empty)
                    {
                        dr["txtLabItem"] = (dict["txtLabItem"].ToString().Trim());
                    }

                    //HiddenField hdnddlMatItemId = (HiddenField)item.FindControl("hdnddlMatItemId");
                    int _hdnddlMatItemId = 0;

                    //int.TryParse(hdnddlMatItemId.Value, out _hdnddlMatItemId);
                    int.TryParse(dict["hdnddlMatItemId"].ToString(), out _hdnddlMatItemId);
                    if (!_hdnddlMatItemId.Equals(0)) dr["MatItem"] = _hdnddlMatItemId;

                    //TextBox txtQtyReq = (TextBox)item.FindControl("txtQtyReq");
                    //if (txtQtyReq.Text.Trim() != string.Empty && txtQtyReq.Text.Trim() != "0.00") dr["QtyReq"] = Convert.ToDouble(txtQtyReq.Text.Trim());
                    //else dr["QtyReq"] = 0;
                    if (dict["txtQtyReq"].ToString().Trim() != string.Empty && dict["txtQtyReq"].ToString().Trim() != "0.00")
                    {
                        dr["QtyReq"] = Convert.ToDouble(dict["txtQtyReq"]);
                    }
                    else
                    {
                        dr["QtyReq"] = 0;
                    }

                    //TextBox txtUM = (TextBox)item.FindControl("txtUM");
                    //if (txtUM.Text != string.Empty)
                    //{
                    //    dr["UM"] = txtUM.Text;
                    //}
                    if (dict["txtUM"].ToString().Trim() != string.Empty)
                    {
                        dr["UM"] = dict["txtUM"].ToString().Trim();
                    }

                    //TextBox txtBudgetUnit = (TextBox)item.FindControl("txtBudgetUnit");
                    //if (!string.IsNullOrEmpty(txtBudgetUnit.Text) && txtBudgetUnit.Text.Trim() != "0.00")
                    //{
                    //    dr["BudgetUnit"] = Convert.ToDouble(txtBudgetUnit.Text);
                    //}
                    //else
                    //{
                    //    dr["BudgetUnit"] = 0;
                    //}
                    if (!string.IsNullOrEmpty(dict["txtBudgetUnit"].ToString()) && dict["txtBudgetUnit"].ToString().Trim() != "0.00")
                    {
                        dr["BudgetUnit"] = Convert.ToDouble(dict["txtBudgetUnit"]);
                    }
                    else
                    {
                        dr["BudgetUnit"] = 0;
                    }

                    //if (txtBudgetUnit.Text != string.Empty && !string.IsNullOrEmpty(txtQtyReq.Text) && txtBudgetUnit.Text != "0.00" && txtQtyReq.Text != "0.00")
                    //{
                    //    _qtyReq = Convert.ToDouble(txtQtyReq.Text);
                    //    if (_qtyReq.Equals(0))
                    //    {
                    //        _qtyReq = Convert.ToDouble(txtQtyReq.Text);
                    //    }
                    //    budgetExt = _qtyReq * Convert.ToDouble(txtBudgetUnit.Text);
                    //    dr["BudgetExt"] = budgetExt;
                    //}
                    //else
                    //{
                    //    dr["BudgetExt"] = 0;
                    //}
                    if (dict["txtBudgetUnit"].ToString().Trim() != string.Empty && !string.IsNullOrEmpty(dict["txtQtyReq"].ToString())
                    && dict["txtBudgetUnit"].ToString().Trim() != "0.00" && dict["txtQtyReq"].ToString().Trim() != "0.00")
                    {
                        _qtyReq = Convert.ToDouble(dict["txtQtyReq"].ToString());
                        if (_qtyReq.Equals(0))
                        {
                            _qtyReq = Convert.ToDouble(dict["txtQtyReq"].ToString().Trim());
                        }
                        budgetExt = _qtyReq * Convert.ToDouble(dict["txtBudgetUnit"].ToString());
                        dr["BudgetExt"] = budgetExt;
                    }
                    else
                    {
                        dr["BudgetExt"] = 0;
                    }

                    //TextBox txtMatMod = (TextBox)item.FindControl("txtMatMod");
                    //if (txtMatMod.Text.Trim() != string.Empty && txtMatMod.Text.Trim() != "0.00")
                    //{
                    //    dr["MatMod"] = Convert.ToDouble(txtMatMod.Text);
                    //}
                    //else
                    //{
                    //    dr["MatMod"] = 0;
                    //}
                    if (dict["txtMatMod"].ToString().Trim() != string.Empty && dict["txtMatMod"].ToString().Trim() != "0.00")
                    {
                        dr["MatMod"] = Convert.ToDouble(dict["txtMatMod"]);
                    }
                    else
                    {
                        dr["MatMod"] = 0;
                    }


                    //TextBox txtLabMod = (TextBox)item.FindControl("txtLabMod");
                    //if (txtLabMod.Text.Trim() != string.Empty && txtLabMod.Text.Trim() != "0.00") dr["LabMod"] = Convert.ToDouble(txtLabMod.Text);
                    //else dr["LabMod"] = 0;
                    if (dict["txtLabMod"].ToString().Trim() != string.Empty && dict["txtLabMod"].ToString().Trim() != "0.00")
                    {
                        dr["LabMod"] = Convert.ToDouble(dict["txtLabMod"]);
                    }
                    else
                    {
                        dr["LabMod"] = 0;
                    }

                    //TextBox txtVendor = (TextBox)item.FindControl("txtVendor");
                    //HiddenField hdnVendorId = (HiddenField)item.FindControl("hdnVendorId");
                    //if (hdnVendorId.Value.Trim() != string.Empty && txtVendor.Text.Trim() != string.Empty) dr["VendorId"] = Convert.ToInt32(hdnVendorId.Value);
                    if (dict["hdnVendorId"].ToString().Trim() != string.Empty)
                    {
                        dr["VendorId"] = Convert.ToInt32(dict["hdnVendorId"]);
                    }

                    //if (txtVendor.Text.Trim() != string.Empty) dr["Vendor"] = txtVendor.Text;
                    if (dict["txtVendor"].ToString().Trim() != string.Empty)
                    {
                        dr["Vendor"] = dict["txtVendor"].ToString();
                    }

                    //TextBox txtHours = (TextBox)item.FindControl("txtHours");
                    //if (txtHours.Text != string.Empty && txtHours.Text != "0.00") dr["LabHours"] = Convert.ToDouble(txtHours.Text);
                    //else dr["LabHours"] = 0;
                    if (dict["txtHours"].ToString().Trim() != string.Empty && dict["txtHours"].ToString().Trim() != "0.00")
                    {
                        dr["LabHours"] = Convert.ToDouble(dict["txtHours"]);
                    }
                    else
                    {
                        dr["LabHours"] = 0;
                    }

                    //TextBox txtLabRate = (TextBox)item.FindControl("txtLabRate");
                    //if (!string.IsNullOrEmpty(txtLabRate.Text) && txtLabRate.Text != "0.00") dr["LabRate"] = txtLabRate.Text;
                    //else dr["LabRate"] = 0;
                    if (!string.IsNullOrEmpty(dict["txtLabRate"].ToString()) && dict["txtLabRate"].ToString().Trim() != "0.00")
                    {
                        dr["LabRate"] = dict["txtLabRate"];
                    }
                    else
                    {
                        dr["LabRate"] = 0;
                    }

                    //if (!string.IsNullOrEmpty(txtLabRate.Text) && !string.IsNullOrEmpty(txtHours.Text)
                    //        && txtLabRate.Text.Trim() != "0.00" && txtHours.Text.Trim() != "0.00")
                    //{
                    //    labExt = Convert.ToDouble(txtLabRate.Text);
                    //    if (txtHours.Text.Trim() != string.Empty) labExt = labExt * Convert.ToDouble(txtHours.Text.Trim());
                    //    dr["LabExt"] = labExt;
                    //}
                    //else dr["LabExt"] = 0;
                    if (!string.IsNullOrEmpty(dict["txtLabRate"].ToString()) && !string.IsNullOrEmpty(dict["txtHours"].ToString())
                        && dict["txtLabRate"].ToString().Trim() != "0.00" && dict["txtHours"].ToString().Trim() != "0.00")
                    {
                        labExt = Convert.ToDouble(dict["txtLabRate"]);
                        if (dict["txtHours"].ToString().Trim() != string.Empty)
                        {
                            labExt = labExt * Convert.ToDouble(dict["txtHours"]);
                        }
                        dr["LabExt"] = labExt;
                    }
                    else
                    {
                        dr["LabExt"] = 0;
                    }

                    dr["TotalExt"] = labExt + budgetExt;

                    //TextBox txtSDate = (TextBox)item.FindControl("txtSDate");
                    //if (txtSDate.Text != string.Empty) { dr["SDate"] = Convert.ToDateTime(txtSDate.Text); }
                    if (dict["txtSDate"].ToString().Trim() != string.Empty)
                    {
                        dr["SDate"] = Convert.ToDateTime(dict["txtSDate"]);
                    }

                    //TextBox txtddlMatItem = (TextBox)item.FindControl("txtddlMatItem");
                    //dr["MatDesc"] = (txtddlMatItem.Text);
                    dr["MatDesc"] = (dict["txtddlMatItem"].ToString());

                    //HiddenField hdnOrderNo = (HiddenField)item.FindControl("hdnOrderNo");
                    //if (hdnOrderNo.Value != string.Empty) dr["OrderNo"] = i;
                    //HiddenField hdnIndex = (HiddenField)item.FindControl("hdnIndex");
                    //if (hdnIndex.Value != string.Empty) dr["OrderNo"] = Convert.ToInt32(hdnIndex.Value);
                    if (dict["hdnOrderNo"].ToString().Trim() != string.Empty)
                    {
                        dr["OrderNo"] = Convert.ToInt32(dict["hdnIndex"].ToString());
                    }

                    //TextBox txtGroup = (TextBox)item.FindControl("txtGroup");
                    //if (txtGroup.Text != string.Empty) dr["GroupName"] = (txtGroup.Text);
                    if (dict["txtGroup"].ToString().Trim() != string.Empty)
                    {
                        dr["GroupName"] = (dict["txtGroup"].ToString());
                    }

                    //HiddenField hdnGroupID = (HiddenField)item.FindControl("hdnGroupID");
                    //if (hdnGroupID.Value != string.Empty) dr["GroupID"] = Convert.ToInt32(hdnGroupID.Value); else { dr["GroupID"] = 0; }
                    if (dict["hdnGroupID"].ToString().Trim() != string.Empty)
                    {
                        dr["GroupID"] = Convert.ToInt32(dict["hdnGroupID"].ToString());
                    }
                    else { dr["GroupID"] = 0; }

                    //TextBox lblCodeDesc = (TextBox)item.FindControl("lblCodeDesc");
                    if (dict["lblCodeDesc"].ToString().Trim() != string.Empty)
                    {
                        dr["CodeDesc"] = (dict["lblCodeDesc"].ToString());
                    }

                    //if (lblCodeDesc.Text != string.Empty) dr["CodeDesc"] = lblCodeDesc.Text;
                    //CheckBox chkMatSalestax = (CheckBox)item.FindControl("chkMatSalestax");
                    //dr["STax"] = chkMatSalestax.Checked;
                    //CheckBox chkLabSalestax = (CheckBox)item.FindControl("chkLabSalestax");
                    //dr["LSTax"] = chkLabSalestax.Checked;
                    if (dict["hdnMatChk"].ToString().Trim() != string.Empty)
                    {
                        if (dict["hdnMatChk"].ToString().ToLower() == "true")
                        {
                            dr["Stax"] = 1;
                        }
                        else
                        {
                            dr["Stax"] = 0;
                        }
                    }
                    if (dict["hdnLbChk"].ToString().Trim() != string.Empty)
                    {
                        if (dict["hdnLbChk"].ToString().ToLower() == "true")
                        {
                            dr["LStax"] = 1;
                        }
                        else
                        {
                            dr["LStax"] = 0;
                        }
                    }

                    dt.Rows.Add(dr);
                }
            }
        }
        //}
        //catch (Exception ex)
        //{
        //    string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
        //    ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        //}
        return dt;
    }

    //private DataTable GetBOMItems()
    //{
    //    DataTable dt = new DataTable();
    //    dt.Columns.Add("JobT", typeof(int));
    //    dt.Columns.Add("Job", typeof(int));
    //    dt.Columns.Add("JobTItemID", typeof(int));
    //    dt.Columns.Add("fDesc", typeof(string));
    //    dt.Columns.Add("Code", typeof(string));
    //    dt.Columns.Add("Line", typeof(int));
    //    dt.Columns.Add("BType", typeof(int));
    //    dt.Columns.Add("QtyReq", typeof(double));
    //    dt.Columns.Add("UM", typeof(string));
    //    dt.Columns.Add("BudgetUnit", typeof(double));
    //    dt.Columns.Add("BudgetExt", typeof(double));
    //    dt.Columns.Add("LabItem", typeof(int));         // BOM.LabItem
    //    dt.Columns.Add("txtLabItem", typeof(string));         // BOM.LabItem
    //    dt.Columns.Add("MatItem", typeof(int));         // BOM.MatItem
    //    dt.Columns.Add("MatMod", typeof(double));       // JobTItem.Modifier
    //    dt.Columns.Add("LabMod", typeof(double));       // JobTItem.ETCMod
    //    dt.Columns.Add("LabExt", typeof(double));       // JobTItem.ETC
    //    dt.Columns.Add("LabRate", typeof(double));      // BOM.LabRate
    //    dt.Columns.Add("LabHours", typeof(double));        //JobTItem.BHours
    //    dt.Columns.Add("SDate", typeof(DateTime));      // BOM.SDate
    //    dt.Columns.Add("VendorId", typeof(int));
    //    dt.Columns.Add("Vendor", typeof(string));
    //    dt.Columns.Add("TotalExt", typeof(double));
    //    dt.Columns.Add("MatDesc", typeof(string));
    //    dt.Columns.Add("OrderNo", typeof(int));
    //    dt.Columns.Add("GroupId", typeof(int));
    //    dt.Columns.Add("GroupName", typeof(string));
    //    dt.Columns.Add("CodeDesc", typeof(string));
    //    dt.Columns.Add("STax", typeof(int));
    //    dt.Columns.Add("LSTax", typeof(int));

    //    //string strItems = hdnItemJSON.Value.Trim();
    //    double budgetExt = 0;
    //    double _qtyReq = 0;
    //    double labExt = 0;
    //    try
    //    {
    //        int i = 0;
    //        foreach (GridDataItem item in gvBOM.Items)
    //        {
    //            //bom items
    //            DropDownList ddlBType = (DropDownList)item.FindControl("ddlBType");

    //            TextBox txtScope = (TextBox)item.FindControl("txtScope");

    //            if (txtScope.Text.Trim() != string.Empty && ddlBType.SelectedValue != "0" && ddlBType.SelectedValue != "Select Type")
    //            {
    //                _qtyReq = 0;
    //                budgetExt = 0;
    //                labExt = 0;

    //                i++;
    //                DataRow dr = dt.NewRow();
    //                HiddenField hdnLine = (HiddenField)item.FindControl("hdnLine");

    //                if (hdnLine.Value.Trim() != string.Empty)
    //                {
    //                    dr["Line"] = Convert.ToInt32(hdnLine.Value);
    //                }

    //                HiddenField hdnID = (HiddenField)item.FindControl("hdnID");
    //                if (hdnID.Value.Trim() != string.Empty)
    //                {
    //                    dr["JobTItemID"] = Convert.ToInt32(hdnID.Value);
    //                }
    //                else
    //                {
    //                    dr["JobTItemID"] = 0;
    //                }

    //                dr["fDesc"] = txtScope.Text;

    //                TextBox txtCode = (TextBox)item.FindControl("txtCode");

    //                dr["Code"] = txtCode.Text;


    //                if (ddlBType.SelectedValue != "0")
    //                {
    //                    dr["BType"] = Convert.ToInt32(ddlBType.SelectedValue);
    //                }

    //                HiddenField hdntxtLabItem1 = (HiddenField)item.FindControl("hdntxtLabItem");

    //                int hdntxtLabItem = 0; int.TryParse(hdntxtLabItem1.Value.ToString(), out hdntxtLabItem);

    //                if (!hdntxtLabItem.Equals(0))
    //                {
    //                    dr["LabItem"] = hdntxtLabItem;
    //                }

    //                TextBox txtLabItem = (TextBox)item.FindControl("txtLabItem");

    //                if ((txtLabItem.Text.ToString().Trim() != string.Empty))
    //                {
    //                    dr["txtLabItem"] = txtLabItem.Text;
    //                }

    //                HiddenField hdnddlMatItemId = (HiddenField)item.FindControl("hdnddlMatItemId");

    //                int _hdnddlMatItemId = 0;

    //                int.TryParse(hdnddlMatItemId.Value, out _hdnddlMatItemId);

    //                if (!_hdnddlMatItemId.Equals(0)) dr["MatItem"] = _hdnddlMatItemId;

    //                TextBox txtQtyReq = (TextBox)item.FindControl("txtQtyReq");

    //                if (txtQtyReq.Text.Trim() != string.Empty && txtQtyReq.Text.Trim() != "0.00") dr["QtyReq"] = Convert.ToDouble(txtQtyReq.Text.Trim());
    //                else dr["QtyReq"] = 0;

    //                TextBox txtUM = (TextBox)item.FindControl("txtUM");

    //                if (txtUM.Text != string.Empty)
    //                {
    //                    dr["UM"] = txtUM.Text;
    //                }

    //                TextBox txtBudgetUnit = (TextBox)item.FindControl("txtBudgetUnit");

    //                if (!string.IsNullOrEmpty(txtBudgetUnit.Text) && txtBudgetUnit.Text.Trim() != "0.00")
    //                {
    //                    dr["BudgetUnit"] = Convert.ToDouble(txtBudgetUnit.Text);
    //                }
    //                else
    //                {
    //                    dr["BudgetUnit"] = 0;
    //                }


    //                if (txtBudgetUnit.Text != string.Empty && !string.IsNullOrEmpty(txtQtyReq.Text) && txtBudgetUnit.Text != "0.00" && txtQtyReq.Text != "0.00")
    //                {
    //                    _qtyReq = Convert.ToDouble(txtQtyReq.Text);
    //                    if (_qtyReq.Equals(0))
    //                    {
    //                        _qtyReq = Convert.ToDouble(txtQtyReq.Text);
    //                    }
    //                    budgetExt = _qtyReq * Convert.ToDouble(txtBudgetUnit.Text);
    //                    dr["BudgetExt"] = budgetExt;
    //                }
    //                else
    //                {
    //                    dr["BudgetExt"] = 0;
    //                }


    //                TextBox txtMatMod = (TextBox)item.FindControl("txtMatMod");


    //                if (txtMatMod.Text.Trim() != string.Empty && txtMatMod.Text.Trim() != "0.00")
    //                {
    //                    dr["MatMod"] = Convert.ToDouble(txtMatMod.Text);
    //                }
    //                else
    //                {
    //                    dr["MatMod"] = 0;
    //                }

    //                TextBox txtLabMod = (TextBox)item.FindControl("txtLabMod");

    //                if (txtLabMod.Text.Trim() != string.Empty && txtLabMod.Text.Trim() != "0.00") dr["LabMod"] = Convert.ToDouble(txtLabMod.Text);

    //                else dr["LabMod"] = 0;


    //                TextBox txtVendor = (TextBox)item.FindControl("txtVendor");

    //                HiddenField hdnVendorId = (HiddenField)item.FindControl("hdnVendorId");

    //                if (hdnVendorId.Value.Trim() != string.Empty && txtVendor.Text.Trim() != string.Empty) dr["VendorId"] = Convert.ToInt32(hdnVendorId.Value);


    //                if (txtVendor.Text.Trim() != string.Empty) dr["Vendor"] = txtVendor.Text;


    //                TextBox txtHours = (TextBox)item.FindControl("txtHours");


    //                if (txtHours.Text != string.Empty && txtHours.Text != "0.00") dr["LabHours"] = Convert.ToDouble(txtHours.Text);

    //                else dr["LabHours"] = 0;

    //                TextBox txtLabRate = (TextBox)item.FindControl("txtLabRate");

    //                if (!string.IsNullOrEmpty(txtLabRate.Text) && txtLabRate.Text != "0.00") dr["LabRate"] = txtLabRate.Text;

    //                else dr["LabRate"] = 0;


    //                if (!string.IsNullOrEmpty(txtLabRate.Text) && !string.IsNullOrEmpty(txtHours.Text)
    //                        && txtLabRate.Text.Trim() != "0.00" && txtHours.Text.Trim() != "0.00")
    //                {
    //                    labExt = Convert.ToDouble(txtLabRate.Text);

    //                    if (txtHours.Text.Trim() != string.Empty) labExt = labExt * Convert.ToDouble(txtHours.Text.Trim());
    //                    dr["LabExt"] = labExt;
    //                }

    //                else dr["LabExt"] = 0;

    //                dr["TotalExt"] = labExt + budgetExt;

    //                TextBox txtSDate = (TextBox)item.FindControl("txtSDate");


    //                if (txtSDate.Text != string.Empty) { dr["SDate"] = Convert.ToDateTime(txtSDate.Text); }

    //                TextBox txtddlMatItem = (TextBox)item.FindControl("txtddlMatItem");

    //                dr["MatDesc"] = (txtddlMatItem.Text);

    //                //HiddenField hdnOrderNo = (HiddenField)item.FindControl("hdnOrderNo");
    //                //if (hdnOrderNo.Value != string.Empty) dr["OrderNo"] = i;
    //                HiddenField hdnIndex = (HiddenField)item.FindControl("hdnIndex");
    //                if (hdnIndex.Value != string.Empty) dr["OrderNo"] = Convert.ToInt32(hdnIndex.Value);

    //                TextBox txtGroup = (TextBox)item.FindControl("txtGroup");

    //                if (txtGroup.Text != string.Empty) dr["GroupName"] = (txtGroup.Text);

    //                HiddenField hdnGroupID = (HiddenField)item.FindControl("hdnGroupID");

    //                if (hdnGroupID.Value != string.Empty) dr["GroupID"] = Convert.ToInt32(hdnGroupID.Value); else { dr["GroupID"] = 0; }

    //                TextBox lblCodeDesc = (TextBox)item.FindControl("lblCodeDesc");

    //                if (lblCodeDesc.Text != string.Empty) dr["CodeDesc"] = lblCodeDesc.Text;
    //                CheckBox chkMatSalestax = (CheckBox)item.FindControl("chkMatSalestax");
    //                dr["STax"] = chkMatSalestax.Checked;
    //                CheckBox chkLabSalestax = (CheckBox)item.FindControl("chkLabSalestax");
    //                dr["LSTax"] = chkLabSalestax.Checked;

    //                dt.Rows.Add(dr);
    //            }
    //        }

    //    }
    //    catch (Exception ex)
    //    {
    //        string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
    //        ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
    //    }
    //    return dt;
    //}




    /// <summary>
    /// FillItems method is to fill drop down if none data available
    /// </summary>
    /// <param name="ddlItem"> A drop down control of gvBOM gridview</param>

    //private void FillItems(DropDownList ddlItem)
    //{
    //    ddlItem.Items.Clear();
    //    ddlItem.Items.Add(new ListItem("No data found", "0"));
    //    ddlItem.DataBind();
    //}

    /// <summary>
    /// Fill Inventory details
    /// </summary>
    /// <param name="ddlItem"></param>

    //private void FillInventory()
    //{
    //    try
    //    {
    //        _objJob.ConnConfig = Session["config"].ToString();
    //        DataSet dsInv = objBL_Job.GetInventoryItem(_objJob);

    //        DataRow dr = dsInv.Tables[0].NewRow();
    //        dr["MatItem"] = 0;
    //        dr["MatDesc"] = "Select Material";
    //        dsInv.Tables[0].Rows.InsertAt(dr, 0);

    //        dtMat = dsInv.Tables[0];
    //    }
    //    catch (Exception ex)
    //    {
    //        string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
    //        ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
    //    }
    //}


    private void FillBomType()
    {
        try
        {
            DataSet ds = new DataSet();
            _objJob.ConnConfig = Session["config"].ToString();
            ds = objBL_Job.GetBomType(_objJob);

            DataTable dt = ds.Tables[0];
            dt.Columns["ID"].AllowDBNull = true;
            DataRow dr = dt.NewRow();
            dr["ID"] = 0;
            dr["Type"] = "Select Type";
            dt.Rows.InsertAt(dr, 0);
            DataRow dr2 = dt.NewRow();
            dr2["ID"] = DBNull.Value;
            dr2["Type"] = " < Add New > ";
            dt.Rows.InsertAt(dr2, 1);
            dtBomType = dt;

        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    #endregion

    #region Milestones

    private void InitMilestoneGrid()
    {
        DataTable dt = CreateMilstoneDataTable();
        DataRow dr = dt.NewRow();
        dr["Line"] = 0;
        dr["OrderNo"] = 1;
        dr["Amount"] = "0.00";
        dr["GroupId"] = 0;
        dt.Rows.Add(dr);

        DataRow dr1 = dt.NewRow();
        dr1["Line"] = 0;
        dr1["OrderNo"] = 2;
        dr1["Amount"] = "0.00";
        dr["GroupId"] = 0;
        dt.Rows.Add(dr1);


        BindgvMilestones(dt);
    }

    private DataTable GetMilestoneGridItems()       //get all items in milestone grid
    {
        //DataTable dt = new DataTable();

        //dt.Columns.Add("JobT", typeof(int));
        //dt.Columns.Add("Job", typeof(int));
        //dt.Columns.Add("JobTItem", typeof(int));
        //dt.Columns.Add("jType", typeof(int));
        //dt.Columns.Add("fDesc", typeof(string));
        //dt.Columns.Add("jcode", typeof(string));
        //dt.Columns.Add("Line", typeof(int));
        //dt.Columns.Add("MilesName", typeof(string));
        //dt.Columns.Add("RequiredBy", typeof(DateTime));
        //dt.Columns.Add("LeadTime", typeof(double));
        //dt.Columns.Add("ProjAcquistDate", typeof(string));
        //dt.Columns.Add("ActAcquDate", typeof(string));
        //dt.Columns.Add("Comments", typeof(string));
        //dt.Columns.Add("Type", typeof(int));
        //dt.Columns.Add("Department", typeof(string));
        //dt.Columns.Add("Amount", typeof(double));
        //dt.Columns.Add("OrderNo", typeof(double));
        //dt.Columns.Add("GroupId", typeof(int));
        //dt.Columns.Add("GroupName", typeof(string));
        //dt.Columns.Add("CodeDesc", typeof(string));
        //dt.Columns.Add("Quantity", typeof(string));
        //dt.Columns.Add("Price", typeof(string));
        //dt.Columns.Add("ChangeOrder", typeof(int));

        DataTable dt = CreateMilstoneDataTable();
        //try
        //{
        string strItems = hdnMilestone.Value.Trim();

        if (strItems != string.Empty)
        {
            JavaScriptSerializer sr = new JavaScriptSerializer();
            List<Dictionary<object, object>> objEstimateItemData = new List<Dictionary<object, object>>();
            objEstimateItemData = sr.Deserialize<List<Dictionary<object, object>>>(strItems);
            int i = 0;

            foreach (Dictionary<object, object> dict in objEstimateItemData)
            {
                if (dict["hdnLine"].ToString().Trim() == string.Empty)
                {
                    return dt;
                }
                i++;
                DataRow dr = dt.NewRow();
                if (dict["hdnLine"].ToString().Trim() != string.Empty)
                {
                    dr["Line"] = Convert.ToInt32(dict["hdnLine"].ToString());
                }
                dr["fDesc"] = dict["txtScope"].ToString().Trim();
                dr["jcode"] = dict["txtCode"].ToString().Trim();
                dr["jtype"] = Convert.ToInt16(dict["ddlType"]);

                dr["MilesName"] = dict["txtName"].ToString().Trim();
                if (dict["txtRequiredBy"].ToString() != string.Empty)
                {
                    dr["RequiredBy"] = Convert.ToDateTime(dict["txtRequiredBy"]);
                }
                if (dict["txtAmount"].ToString() != string.Empty && dict["txtAmount"].ToString().Trim() != "0.00")
                {
                    dr["Amount"] = Convert.ToDouble(dict["txtAmount"]);
                }
                else
                {
                    dr["Amount"] = 0;
                }

                if (dict["txtQuantity"].ToString() != string.Empty && dict["txtQuantity"].ToString().Trim() != "0.00")
                {
                    dr["Quantity"] = Convert.ToDouble(dict["txtQuantity"]);
                }
                else
                {
                    dr["Quantity"] = 0;
                }

                if (dict["txtPrice"].ToString() != string.Empty && dict["txtPrice"].ToString().Trim() != "0.00")
                {
                    dr["Price"] = Convert.ToDouble(dict["txtPrice"]);
                }
                else
                {
                    dr["Price"] = 0;
                }

                if (!string.IsNullOrEmpty(dict["hdnType"].ToString()))
                {
                    dr["Type"] = dict["hdnType"].ToString();
                    dr["Department"] = dict["txtSType"].ToString();
                }
                if (dict["hdnOrderNoMil"].ToString().Trim() != string.Empty)
                {
                    dr["OrderNo"] = Convert.ToInt32(dict["hdnOrderNoMil"].ToString());
                }
                if (dict["txtGroup"].ToString().Trim() != string.Empty)
                {
                    dr["GroupName"] = (dict["txtGroup"].ToString());
                }

                if (dict["hdnGroupID"].ToString().Trim() != string.Empty)
                {
                    dr["GroupID"] = Convert.ToInt32(dict["hdnGroupID"].ToString());
                }
                else { dr["GroupID"] = 0; }
                if (dict["lblCodeDesc"].ToString().Trim() != string.Empty)
                {
                    dr["CodeDesc"] = (dict["lblCodeDesc"].ToString());
                }

                if (dict["hdnChangeOrderChk"].ToString().Trim() != string.Empty)
                {
                    if (dict["hdnChangeOrderChk"].ToString().ToLower() == "true")
                    {
                        dr["ChangeOrder"] = 1;
                    }
                    else
                    {
                        dr["ChangeOrder"] = 0;
                    }
                }
                dt.Rows.Add(dr);
            }
        }
        //}
        //catch (Exception ex)
        //{
        //    string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
        //    ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        //}
        return dt;
    }

    private DataTable GetMilestoneItems()
    {
        DataTable dt = new DataTable();

        dt.Columns.Add("JobT", typeof(int));
        dt.Columns.Add("Job", typeof(int));
        dt.Columns.Add("JobTItem", typeof(int));
        dt.Columns.Add("jType", typeof(int));
        dt.Columns.Add("fDesc", typeof(string));
        dt.Columns.Add("jcode", typeof(string));
        dt.Columns.Add("Line", typeof(int));
        dt.Columns.Add("MilesName", typeof(string));
        dt.Columns.Add("RequiredBy", typeof(DateTime));
        dt.Columns.Add("LeadTime", typeof(double));
        dt.Columns.Add("ProjAcquistDate", typeof(string));
        dt.Columns.Add("ActAcquDate", typeof(string));
        dt.Columns.Add("Comments", typeof(string));
        dt.Columns.Add("Type", typeof(int));
        dt.Columns.Add("Department", typeof(string));
        dt.Columns.Add("Amount", typeof(double));
        dt.Columns.Add("OrderNo", typeof(double));
        dt.Columns.Add("GroupId", typeof(int));
        dt.Columns.Add("GroupName", typeof(string));
        dt.Columns.Add("CodeDesc", typeof(string));
        dt.Columns.Add("Quantity", typeof(string));
        dt.Columns.Add("Price", typeof(string));
        dt.Columns.Add("ChangeOrder", typeof(int));
        //try
        //{
        string strItems = hdnMilestone.Value.Trim();

        if (strItems != string.Empty)
        {
            JavaScriptSerializer sr = new JavaScriptSerializer();
            List<Dictionary<object, object>> objEstimateItemData = new List<Dictionary<object, object>>();
            objEstimateItemData = sr.Deserialize<List<Dictionary<object, object>>>(strItems);
            int i = 0;

            foreach (Dictionary<object, object> dict in objEstimateItemData)
            {
                if (dict["txtScope"].ToString().Trim() != string.Empty)
                {
                    //    return dt;
                    //}
                    i++;
                    DataRow dr = dt.NewRow();
                    dr["Line"] = dict["hdnLine"].ToString().Trim();
                    dr["fDesc"] = dict["txtScope"].ToString().Trim();
                    dr["jcode"] = dict["txtCode"].ToString().Trim();
                    dr["jtype"] = Convert.ToInt16(dict["ddlType"]);
                    dr["MilesName"] = dict["txtName"].ToString().Trim();
                    if (dict["txtRequiredBy"].ToString() != string.Empty)
                    {
                        dr["RequiredBy"] = Convert.ToDateTime(dict["txtRequiredBy"]);
                    }
                    if (dict["txtAmount"].ToString() != string.Empty && dict["txtAmount"].ToString().Trim() != "0.00")
                    {
                        dr["Amount"] = Convert.ToDouble(dict["txtAmount"]);
                    }
                    else
                    {
                        dr["Amount"] = 0;
                    }

                    if (dict["txtQuantity"].ToString() != string.Empty && dict["txtQuantity"].ToString().Trim() != "0.00")
                    {
                        dr["Quantity"] = Convert.ToDouble(dict["txtQuantity"]);
                    }
                    else
                    {
                        dr["Quantity"] = 0;
                    }

                    if (dict["txtPrice"].ToString() != string.Empty && dict["txtPrice"].ToString().Trim() != "0.00")
                    {
                        dr["Price"] = Convert.ToDouble(dict["txtPrice"]);
                    }
                    else
                    {
                        dr["Price"] = 0;
                    }

                    if (!string.IsNullOrEmpty(dict["hdnType"].ToString()))
                    {
                        dr["Type"] = dict["hdnType"].ToString();
                        dr["Department"] = dict["txtSType"].ToString();
                    }
                    if (dict["hdnOrderNoMil"].ToString().Trim() != string.Empty)
                    {
                        dr["OrderNo"] = Convert.ToInt32(dict["hdnOrderNoMil"].ToString());
                    }
                    if (dict["txtGroup"].ToString().Trim() != string.Empty)
                    {
                        dr["GroupName"] = (dict["txtGroup"].ToString());
                    }

                    if (dict["hdnGroupID"].ToString().Trim() != string.Empty)
                    {
                        dr["GroupID"] = Convert.ToInt32(dict["hdnGroupID"].ToString());
                    }
                    else { dr["GroupID"] = 0; }
                    if (dict["lblCodeDesc"].ToString().Trim() != string.Empty)
                    {
                        dr["CodeDesc"] = (dict["lblCodeDesc"].ToString());
                    }
                    if (dict["hdnChangeOrderChk"].ToString().Trim() != string.Empty)
                    {
                        if (dict["hdnChangeOrderChk"].ToString().ToLower() == "true")
                        {
                            dr["ChangeOrder"] = 1;
                        }
                        else
                        {
                            dr["ChangeOrder"] = 0;
                        }
                    }
                    dt.Rows.Add(dr);
                }
            }
        }
        //}
        //catch (Exception ex)
        //{
        //    string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
        //    ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        //}
        return dt;
    }

    #endregion

    #region Checklist

    private void GetChecklist()
    {
        //GridView gvDepartment = (GridView)Page.FindControl("gvDepartment");
        //foreach (var uc in this.FindControl("PlaceHolder1").Controls.OfType<UserControl>())
        //{
        //    GridView gv1 = new GridView();
        //    GetDepartment(gv1);
        //}
        try
        {
            PlaceHolder p = (PlaceHolder)this.FindControl("PlaceHolder1");

            foreach (var uc in p.Controls)
            {
                GridView gv1 = new GridView();
                GetDepartment(gv1);
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErrProj", "noty({text: '" + str + "',  type : 'error', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    #region generate code for delegate

    protected override void OnInit(EventArgs e)
    {
        //InitializeComponent(_depQty);     //uncomment for checklist
        //base.OnInit(e);
    }

    private void InitializeComponent(int _dQty)
    {
        this.Load += new System.EventHandler(this.Page_Load);
        //_dQty = 0;        //uncomment for checklist
        //for (int i = 0; i < _dQty; i++)
        //{
        //    uc_gvChecklist ucGvChecklist = LoadControl("~/uc_gvChecklist.ascx") as uc_gvChecklist;
        //    PlaceHolder1.Controls.Add(ucGvChecklist);
        //    ucGvChecklist.GridRowCommand += new uc_gvChecklist.RowCommand(ucGvDepartment_GridRowCommand);
        //}
    }

    protected void ucGvDepartment_GridRowCommand(object sender, GridViewCommandEventArgs e)
    {
        DataTable dt = new DataTable();
        DataTable dtnew = new DataTable();
        GridView GvDepartment = (GridView)sender;

        switch (e.CommandName)
        {
            case "UpArr":
                dt = GetDepartment(GvDepartment);

                #region Up Row

                foreach (GridViewRow gr in GvDepartment.Rows)
                {
                    CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");
                    if (chkSelect.Checked.Equals(true))
                    {
                        int rowIndex = gr.RowIndex;
                        if (rowIndex == 0)
                        {
                            continue;
                        }
                        else
                        {
                            string fdesc = dt.Rows[rowIndex]["fdesc"].ToString();
                            string _format = dt.Rows[rowIndex]["Format"].ToString();
                            string _refFormat = dt.Rows[rowIndex]["RefFormat"].ToString();

                            dt.Rows[rowIndex]["fdesc"] = dt.Rows[rowIndex - 1]["fdesc"];
                            dt.Rows[rowIndex]["Format"] = dt.Rows[rowIndex - 1]["Format"];
                            dt.Rows[rowIndex]["RefFormat"] = dt.Rows[rowIndex - 1]["RefFormat"];

                            dt.Rows[rowIndex - 1]["fdesc"] = fdesc;
                            dt.Rows[rowIndex - 1]["Format"] = _format;
                            dt.Rows[rowIndex - 1]["RefFormat"] = _refFormat;
                            dt.DefaultView.Sort = "Line";
                            dt.AcceptChanges();
                            dtnew = dt.Copy();
                            dt.AcceptChanges();
                            ViewState["Dep1"] = dtnew;
                        }
                    }
                }
                GvDepartment.DataSource = dt;
                GvDepartment.DataBind();
                #endregion

                break;
            case "DownArr":

                int lastRow = GvDepartment.Rows.Count - 1;
                dt = GetDepartment(GvDepartment);

                #region Down Row

                foreach (GridViewRow gr in GvDepartment.Rows)
                {
                    CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");
                    if (chkSelect.Checked.Equals(true))
                    {
                        int rowIndex = gr.RowIndex;
                        if (rowIndex == lastRow)
                        {
                            continue;
                        }
                        else
                        {
                            string fdesc = dt.Rows[rowIndex]["fdesc"].ToString();
                            string _format = dt.Rows[rowIndex]["Format"].ToString();
                            string _refFormat = dt.Rows[rowIndex]["Format"].ToString();

                            dt.Rows[rowIndex]["fdesc"] = dt.Rows[rowIndex + 1]["fdesc"];
                            dt.Rows[rowIndex]["Format"] = dt.Rows[rowIndex + 1]["Format"];
                            dt.Rows[rowIndex]["RefFormat"] = dt.Rows[rowIndex + 1]["RefFormat"];

                            dt.Rows[rowIndex + 1]["fdesc"] = fdesc;
                            dt.Rows[rowIndex + 1]["Format"] = _format;
                            dt.Rows[rowIndex + 1]["RefFormat"] = _refFormat;
                            dt.DefaultView.Sort = "Line";
                            dt.AcceptChanges();
                            dtnew = dt.Copy();
                            dt.AcceptChanges();

                            ViewState["Dep"] = dtnew;
                        }
                    }
                }
                GvDepartment.DataSource = dt;
                GvDepartment.DataBind();
                #endregion

                break;
        }
    }
    #endregion

    private DataTable GetDepartment(GridView gvDepartment)
    {
        DataTable dt = new DataTable();

        dt.Columns.Add("line", typeof(int));
        dt.Columns.Add("fdesc", typeof(string));
        dt.Columns.Add("Format", typeof(string));
        dt.Columns.Add("RefFormat", typeof(string));

        DataTable dtDetails = dt.Clone();
        int i = 0;
        try
        {
            foreach (GridViewRow gr in gvDepartment.Rows)
            {
                DataRow dr = dt.NewRow();
                i++;
                dr["Line"] = i;

                TextBox txtDescription = (TextBox)gr.FindControl("txtDescription");
                DropDownList ddlControl = (DropDownList)gr.FindControl("ddlControl");
                DropDownList ddlRefControl = (DropDownList)gr.FindControl("ddlRefControl");
                if (i <= 10)
                {
                    if (!string.IsNullOrEmpty(txtDescription.Text))
                    {
                        dr["fdesc"] = txtDescription.Text;
                        dr["Format"] = Convert.ToInt32(ddlControl.SelectedValue);
                        dr["RefFormat"] = Convert.ToInt32(ddlRefControl.SelectedValue);
                    }
                    dt.Rows.Add(dr);
                }
                else
                {
                    break;
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return dt;
    }

    #endregion

    #region Custom

    /// <summary>
    /// Fill Format drop down in Custom tab which contain list of controls.
    /// </summary>

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

    /// <summary>
    /// Fill tab details of add/edit project page
    /// </summary>
    private void FillTab()
    {
        try
        {
            _objJob.ConnConfig = Session["config"].ToString();
            _objJob.PageUrl = "addproject.aspx";
            DataSet _ds = objBL_Job.GetTabByPageUrl(_objJob);
            if (_ds.Tables[0].Rows.Count > 0)
            {
                dtTab = _ds.Tables[0];
                DataRow _drTab = dtTab.NewRow();
                _drTab["ID"] = 0;
                _drTab["TabName"] = "";
                dtTab.Rows.InsertAt(_drTab, 0);
            }
            else
            {
                dtTab = _ds.Tables[0];
                DataRow _drTab = dtTab.NewRow();
                _drTab["ID"] = 0;
                _drTab["TabName"] = "No Data Found";
                dtTab.Rows.Add(_drTab);
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
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

    /// <summary>
    /// Fetch Custom field and Custom drop down details from gridview
    /// </summary>
    private void CustTempGridData()
    {
        try
        {
            DataTable dt = (DataTable)ViewState["CustomTable"];
            DataTable dtDetails = dt.Clone();
            //If Format type is Dropdown then we store  Default item values in  dtCustomValues table for Dropdown
            DataTable dtCustomValues = new DataTable();
            dtCustomValues.Columns.Add("ID", typeof(int));
            dtCustomValues.Columns.Add("tblTabID", typeof(int));
            dtCustomValues.Columns.Add("Label", typeof(string));
            dtCustomValues.Columns.Add("Line", typeof(Int16));
            dtCustomValues.Columns.Add("Value", typeof(string));
            dtCustomValues.Columns.Add("Format", typeof(Int16));

            //int line;
            //if (ViewState["cLine"] == null)
            //{
            //    //line = 0;
            //    ViewState["maxLine"] = 0;
            //}
            //else
            //{
            //    line = Convert.ToInt16(ViewState["cLine"]);
            //}

            int line = 0;
            var tempLine = 0;
            //var curLine = 0;
            foreach (GridViewRow gr in gvCustom.Rows)
            {
                //tempLine++;
                line++;
                Label lblID = (Label)gr.FindControl("lblID");
                Label lblIndex = (Label)gr.FindControl("lblIndex");
                Label lblLine = (Label)gr.FindControl("lblLine");
                TextBox lblDesc = (TextBox)gr.FindControl("lblDesc");
                HiddenField OrderNo = (HiddenField)gr.FindControl("txtRowLine");
                DropDownList ddlTab = (DropDownList)gr.FindControl("ddlTab");
                DropDownList ddlFormat = (DropDownList)gr.FindControl("ddlFormat");
                DropDownList ddlCustomValue = (DropDownList)gr.FindControl("ddlCustomValue");
                CheckBox chkSelectAlert = (CheckBox)gr.FindControl("chkSelectAlert");
                //CheckBox chkSelectTask = (CheckBox)gr.FindControl("chkSelectTask");
                HiddenField hdnMembers = (HiddenField)gr.FindControl("hdnMembers");
                TextBox txtMembers = (TextBox)gr.FindControl("txtMembers");
                //HiddenField hdnUserRoles = (HiddenField)gr.FindControl("hdnUserRoles");
                //TextBox txtUserRoles = (TextBox)gr.FindControl("txtUserRoles");

                if (!string.IsNullOrEmpty(lblDesc.Text.ToString())
                    && !ddlFormat.SelectedValue.Equals("0")
                    && !ddlTab.SelectedValue.Equals("0")
                    )
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
                            drCustomVal["tblTabID"] = ddlTab.SelectedValue;
                            drCustomVal["Label"] = lblDesc.Text;
                            drCustomVal["Line"] = tempLine;
                            //drCustomVal["Line"] = line;
                            drCustomVal["Value"] = li.Value;
                            drCustomVal["Format"] = ddlFormat.SelectedValue;
                            dtCustomValues.Rows.Add(drCustomVal);
                        }
                    }
                    //custom items values of Grid
                    DataRow dr = dtDetails.NewRow();
                    dr["ID"] = Convert.ToInt32(lblID.Text);
                    dr["tblTabID"] = ddlTab.SelectedValue;
                    dr["Label"] = lblDesc.Text.Trim();
                    //dr["Line"] = lblIndex.Text;
                    dr["Line"] = tempLine;
                    dr["Format"] = ddlFormat.SelectedValue;
                    dr["OrderNo"] = Convert.ToInt32(OrderNo.Value);
                    //dr["OrderNo"] = Convert.ToInt32(lblIndex.Text);
                    dr["IsAlert"] = chkSelectAlert.Checked;
                    //dr["IsTask"] = chkSelectTask.Checked;
                    dr["TeamMember"] = hdnMembers.Value;
                    dr["TeamMemberDisplay"] = txtMembers.Text;
                    //dr["UserRole"] = hdnUserRoles.Value;
                    //dr["UserRoleDisplay"] = txtUserRoles.Text;
                    dtDetails.Rows.Add(dr);
                    //line++;
                }
                else if (string.IsNullOrEmpty(lblDesc.Text.ToString())
                    && (!ddlFormat.SelectedValue.Equals("0") || !ddlTab.SelectedValue.Equals("0"))
                    )
                {
                    throw new Exception("Workflow label cannot be empty");
                }
                else if (ddlTab.SelectedValue.Equals("0")
                    && (!string.IsNullOrEmpty(lblDesc.Text.ToString()) || !ddlFormat.SelectedValue.Equals("0"))
                    )
                {
                    throw new Exception("Please select a type for workflow tab");
                }
                else if (ddlFormat.SelectedValue.Equals("0")
                    && (!string.IsNullOrEmpty(lblDesc.Text.ToString()) || !ddlTab.SelectedValue.Equals("0"))
                    )
                {
                    throw new Exception("Please select a type for workflow format");
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
            CustTempGridData();

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
                dr["tblTabID"] = 0;
                dr["Label"] = DBNull.Value;
                dr["Line"] = dt.Rows.Count + 1;
                dr["Value"] = DBNull.Value;
                dr["Format"] = 0;
                dr["OrderNo"] = 0;
                dr["IsAlert"] = 0;
                dr["IsTask"] = 0;
                dr["TeamMember"] = DBNull.Value;
                dr["TeamMemberDisplay"] = DBNull.Value;
                dr["UserRole"] = DBNull.Value;
                dr["UserRoleDisplay"] = DBNull.Value;
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
    /// Bind Custom gridview
    /// </summary>

    private void BindCustomGrid()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = (DataTable)ViewState["CustomTable"];

            gvCustom.DataSource = dt;
            gvCustom.DataBind();

            ((Label)gvCustom.FooterRow.FindControl("lblRowCount")).Text = "Total Line Items: " + Convert.ToString(dt.Rows.Count);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void CreateCustomTable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("ID", typeof(int));
        dt.Columns.Add("tblTabID", typeof(int));
        dt.Columns.Add("Label", typeof(string));
        dt.Columns.Add("Line", typeof(Int16));
        dt.Columns.Add("Value", typeof(string));
        dt.Columns.Add("Format", typeof(Int16));
        dt.Columns.Add("OrderNo", typeof(int));
        dt.Columns.Add("IsAlert", typeof(int));
        dt.Columns.Add("IsTask", typeof(int));
        dt.Columns.Add("TeamMember", typeof(string));
        dt.Columns.Add("TeamMemberDisplay", typeof(string));
        dt.Columns.Add("UserRole", typeof(string));
        dt.Columns.Add("UserRoleDisplay", typeof(string));
        DataRow dr = dt.NewRow();

        dr["ID"] = 0;
        dr["tblTabID"] = 0;
        dr["Label"] = DBNull.Value;
        dr["Line"] = 0;
        dr["Value"] = DBNull.Value;
        dr["Format"] = 0;
        dr["OrderNo"] = 0;
        dr["IsAlert"] = 0;
        dr["IsTask"] = 0;
        dr["TeamMember"] = DBNull.Value;
        dr["TeamMemberDisplay"] = DBNull.Value;
        dr["UserRole"] = DBNull.Value;
        dr["UserRoleDisplay"] = DBNull.Value;

        dt.Rows.Add(dr);

        ViewState["CustomTable"] = dt;
    }
    #endregion
    #endregion

    protected void ddlJobType_SelectedIndexChanged(object sender, EventArgs e)
    {
        //if (ddlJobType.SelectedValue.Equals("0"))
        //{
        //    DisableTab();
        //}
        //else
        //{
        //    EnableTab();
        //}

        if (Request.QueryString["uid"] == null)
        {
            CreateBOMTableEmpty();
        }
    }

    //private void DisableTab()
    //{
    //    foreach (GridViewRow gr in gvCustom.Rows)
    //    {
    //        DropDownList ddlTab = (DropDownList)gr.FindControl("ddlTab");
    //        ddlTab.Enabled = false;
    //        ddlTab.SelectedValue = "0";
    //    }
    //}

    //private void EnableTab()
    //{
    //    foreach (GridViewRow gr in gvCustom.Rows)
    //    {
    //        DropDownList ddlTab = (DropDownList)gr.FindControl("ddlTab");
    //        ddlTab.Enabled = true;
    //    }
    //}

    protected void lnkEditForm_Click(object sender, EventArgs e)
    {
        //objGeneral.ResetFormControlValues(pnlAttach);
        foreach (GridViewRow di in gvDocuments.Rows)
        {
            CheckBox chkSelect = (CheckBox)di.FindControl("chkSelect");
            Label lblID = (Label)di.FindControl("lblID");
            Label lblName = (Label)di.FindControl("lblName");
            Label lblBody = (Label)di.FindControl("lblBody");

            if (chkSelect.Checked == true)
            {

                hdnEstimateFormId.Value = lblID.Text;
                txtEstimateName.Text = lblName.Text;
                //ViewState["notesmode"] = 1;
                break;
            }
        }

        string script = "function f(){$find(\"" + RadWindowForms.ClientID + "\").show(); Sys.Application.remove_load(f);}Sys.Application.add_load(f); Materialize.updateTextFields();";
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
    }

    protected void lnkDeleteDoc_Click(object sender, EventArgs e)
    {
        foreach (GridViewRow di in gvDocuments.Rows)
        {
            CheckBox chkSelected = (CheckBox)di.FindControl("chkSelect");
            Label lblID = (Label)di.FindControl("lblId");

            if (chkSelected.Checked == true)
            {
                DeleteFileFromFolder(string.Empty, Convert.ToInt32(lblID.Text));
            }
        }
        GetEstimateForms();
    }

    protected void lnkAddForm_Click(object sender, EventArgs e)
    {
        txtEstimateName.Text = "";

        string script = "function f(){$find(\"" + RadWindowForms.ClientID + "\").show(); Sys.Application.remove_load(f);}Sys.Application.add_load(f); Materialize.updateTextFields();";
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);

        ViewState["notesmode"] = 0;
    }

    protected void lnkFileName_Click(object sender, EventArgs e)
    {
        objET.ConnConfig = Session["config"].ToString();

        LinkButton btn = (LinkButton)sender;
        objET.Id = Convert.ToInt32("0" + btn.CommandArgument);
        objBL_ET.GetEstimateTemplateById(objET);
        DownloadDocument(objET.FilePath, objET.FileName);
    }

    protected void lnkUploadDoc_Click(object sender, EventArgs e)
    {

        try
        {
            string fullpath = string.Empty;
            string MIME = string.Empty;
            objET.Id = Convert.ToInt32("0" + hdnEstimateFormId.Value);
            objET.ConnConfig = Session["config"].ToString();
            if (objET.Id > 0)
            {
                objBL_ET.GetEstimateTemplateById(objET);
            }
            if (FileUpload1.HasFile)
            {
                string savepathconfig = System.Web.Configuration.WebConfigurationManager.AppSettings["AttachmentPath"].Trim();
                string savepath = savepathconfig + @"\" + Session["dbname"] + @"\EstimateTemplates" + @"\";
                objET.FileName = FileUpload1.FileName;
                MIME = System.IO.Path.GetExtension(FileUpload1.PostedFile.FileName).Substring(1);
                if (MIME.ToLower() != "docx")
                {
                    throw new Exception("MS Word 2007 or later .docx format is the only supported file type.");
                }

                if (objET.FilePath != "")
                {
                    if (System.IO.File.Exists(objET.FilePath))
                        System.IO.File.Delete(objET.FilePath);
                }

                fullpath = savepath + Guid.NewGuid() + "." + MIME;
                objET.FilePath = fullpath;

                if (!Directory.Exists(savepath))
                {
                    Directory.CreateDirectory(savepath);
                }
                objET.FilePath = fullpath;
                objET.MIME = MIME;
                FileUpload1.SaveAs(objET.FilePath);
            }
            objET.Name = txtEstimateName.Text;
            objET.JobTID = Convert.ToInt32(Request.QueryString["uid"].ToString());
            objET.UpdatedBy = Session["username"].ToString();

            objBL_ET.AddEstimateTemplate(objET);
            GetEstimateForms();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyUploadErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    public void DeleteFileFromFolder(string StrFilename, int DocumentID)
    {
        try
        {
            //File.Delete(StrFilename);
            DeleteFile(DocumentID);
        }
        catch (FileNotFoundException ex)
        {
            DeleteFile(DocumentID);
        }
        catch (UnauthorizedAccessException ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(),
            "FileDeleteAccessWarning", "noty({text: 'Please provide delete permissions to the file path.',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : 3000,theme : 'noty_theme_default',  closable : true});", true);
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);

            ScriptManager.RegisterStartupScript(this, GetType(),
            "FileDeleteErrorWarning", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : 3000,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    private void GetEstimateForms()
    {
        if (Request.QueryString["uid"] != null)
        {
            liForms.Style["display"] = "";
            adForms.Style["display"] = "";
            objET.JobTID = Convert.ToInt32(Request.QueryString["uid"].ToString());
            objET.ConnConfig = Session["config"].ToString();
            DataSet ds = new DataSet();
            ds = objBL_ET.GetEstimateFormsByJobTId(objET);
            gvDocuments.DataSource = ds.Tables[0];
            gvDocuments.DataBind();
        }
        else
        {
            liForms.Style["display"] = "none";
            adForms.Style["display"] = "none";
        }
    }

    private void DeleteFile(int ID)
    {
        try
        {
            objET.ConnConfig = Session["config"].ToString();
            objET.Id = ID;
            objBL_ET.GetEstimateTemplateById(objET);

            objBL_ET.DeleteEstimateTemplate(objET);
            if (objET.FilePath != "")
            {
                if (System.IO.File.Exists(objET.FilePath))
                    System.IO.File.Delete(objET.FilePath);
            }
            //UpdateDocInfo();
            GetEstimateForms();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErrdelete", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : 3000,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    private void DownloadDocument(string filePath, string DownloadFileName)
    {
        try
        {
            System.IO.FileInfo FileName = new System.IO.FileInfo(filePath);
            FileStream myFile = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            BinaryReader _BinaryReader = new BinaryReader(myFile);

            try
            {
                long startBytes = 0;
                string lastUpdateTiemStamp = File.GetLastWriteTimeUtc(filePath).ToString("r");
                string _EncodedData = HttpUtility.UrlEncode(DownloadFileName, System.Text.Encoding.UTF8) + lastUpdateTiemStamp;

                Response.Clear();
                Response.Buffer = false;
                Response.AddHeader("Accept-Ranges", "bytes");
                Response.AppendHeader("ETag", "\"" + _EncodedData + "\"");
                Response.AppendHeader("Last-Modified", lastUpdateTiemStamp);
                Response.ContentType = "application/octet-stream";
                Response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(DownloadFileName));
                Response.AddHeader("Content-Length", (FileName.Length - startBytes).ToString());
                Response.AddHeader("Connection", "Keep-Alive");
                Response.ContentEncoding = System.Text.Encoding.UTF8;

                //Send data
                _BinaryReader.BaseStream.Seek(startBytes, SeekOrigin.Begin);

                //Dividing the data in 1024 bytes package
                int maxCount = (int)Math.Ceiling((FileName.Length - startBytes + 0.0) / 1024);

                //Download in block of 1024 bytes
                int i;
                for (i = 0; i < maxCount && Response.IsClientConnected; i++)
                {
                    Response.BinaryWrite(_BinaryReader.ReadBytes(1024));
                    Response.Flush();
                }
                ////if blocks transfered not equals total number of blocks
                //if (i < maxCount)
                //    return false;
                //return true; 
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Response.End();
                _BinaryReader.Close();
                myFile.Close();
            }
        }
        catch (FileNotFoundException ex)
        {
            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(),
            "FileaccessWarning", "alert('File not found.');", true);
        }
        catch (UnauthorizedAccessException ex)
        {
            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(),
            "FileaccessWarning", "alert('Please provide access permissions to the file path.');", true);
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);

            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(),
            "FileerrorWarning", "alert('" + str + "');", true);
        }
    }

    private void BindgvMilestones(DataTable dt)
    {
        gvMilestones.DataSource = dt;
        gvMilestones.DataBind();
    }

    private void BindgvBOM(DataTable dt)
    {
        gvBOM.DataSource = dt;
        gvBOM.DataBind();
        SetgvBOMEnable(false);
    }

    private void SetgvBOMEnable(bool IsYes)
    {
        DataTable ds = new DataTable();
        ds = (DataTable)Session["userinfo"];

        foreach (GridDataItem item in gvBOM.Items)
        {
            if (Session["type"].ToString() != "am")
            {

                DropDownList drpBType = (DropDownList)item.FindControl("ddlBType");

                //bom
                string BOMPermission = ds.Rows[0]["BOMPermission"] == DBNull.Value ? "YYYY" : ds.Rows[0]["BOMPermission"].ToString();
                string stEditBOM = BOMPermission.Length < 2 ? "Y" : BOMPermission.Substring(1, 1);

                TextBox txtLabItem = (TextBox)item.FindControl("txtLabItem");
                TextBox txtCode = (TextBox)item.FindControl("txtCode");
                TextBox txtScope = (TextBox)item.FindControl("txtScope");
                TextBox txtQtyReq = (TextBox)item.FindControl("txtQtyReq");
                TextBox txtUM = (TextBox)item.FindControl("txtUM");
                TextBox txtBudgetUnit = (TextBox)item.FindControl("txtBudgetUnit");
                TextBox txtMatMod = (TextBox)item.FindControl("txtMatMod");
                TextBox txtVendor = (TextBox)item.FindControl("txtVendor");
                TextBox txtHours = (TextBox)item.FindControl("txtHours");
                TextBox txtLabRate = (TextBox)item.FindControl("txtLabRate");
                TextBox txtLabMod = (TextBox)item.FindControl("txtLabMod");
                TextBox txtSDate = (TextBox)item.FindControl("txtSDate");

                // DropDownList ddlMatItem = (DropDownList)item.FindControl("ddlMatItem");

                TextBox txtddlMatItem = (TextBox)item.FindControl("txtddlMatItem");

                if (stEditBOM == "N")
                {
                    txtLabItem.Enabled = txtddlMatItem.Enabled = txtVendor.Enabled = txtHours.Enabled = txtLabRate.Enabled = txtLabMod.Enabled = txtSDate.Enabled = txtMatMod.Enabled = txtBudgetUnit.Enabled = txtUM.Enabled = txtQtyReq.Enabled = txtScope.Enabled = txtCode.Enabled = IsYes;

                    txtLabItem.Enabled = drpBType.Enabled = IsYes;


                    if (!IsYes)
                        item.Attributes["ondblclick"] = "   noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false,dismissQueue:true });";
                    else { item.Attributes["ondblclick"] = ""; }
                }
                else
                {
                    item.Attributes["ondblclick"] = "";
                }
            }
        }
    }

    protected void gvMilestones_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            DataTable ds = new DataTable();
            ds = (DataTable)Session["userinfo"];

            if (Session["type"].ToString() != "am")
            {
                string MilestonesPermission = ds.Rows[0]["MilestonesPermission"] == DBNull.Value ? "YYYY" : ds.Rows[0]["MilestonesPermission"].ToString();
                string stEditMilestones = MilestonesPermission.Length < 2 ? "Y" : MilestonesPermission.Substring(1, 1);

                if (stEditMilestones == "N")
                {
                    DropDownList ddlType = (DropDownList)e.Item.FindControl("ddlType");
                    TextBox txtCode = (TextBox)e.Item.FindControl("txtCode");
                    TextBox txtSType = (TextBox)e.Item.FindControl("txtSType");
                    TextBox txtName = (TextBox)e.Item.FindControl("txtName");
                    TextBox txtAmount = (TextBox)e.Item.FindControl("txtAmount");
                    TextBox txtRequiredBy = (TextBox)e.Item.FindControl("txtRequiredBy");
                    TextBox txtScope = (TextBox)e.Item.FindControl("txtScope");
                    txtScope.Enabled = txtSType.Enabled = txtName.Enabled = txtAmount.Enabled = txtRequiredBy.Enabled = txtCode.Enabled = false;
                    ddlType.Enabled = false;
                    e.Item.Attributes["ondblclick"] = "   noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false,dismissQueue:true });";
                }

            }
        }
    }

    protected void RadMenuBomGrid_ItemClick(object sender, RadMenuEventArgs e)
    {
        DataTable dt = GetBOMItems();

        switch (e.Item.Text)
        {
            case "Add Row Above":
                AddNewRowGrid(dt, "above", "Bom");
                break;

            case "Add Row Below":
                AddNewRowGrid(dt, "below", "Bom");
                break;
        }
    }

    private void AddNewRowGrid(DataTable dt, string position, string gridName)
    {
        Int32 _orderNo = 0;
        DataRow dr = dt.NewRow();
        string maxvalue = dt.AsEnumerable().Max(row => row["Line"]).ToString();
        Int32 _line = Convert.ToInt32(maxvalue) + 1;
        if (gridName == "Bom")
        {
            _orderNo = Int32.Parse(radGridClickedRowIndex.Value);
            dr["OrderNo"] = _orderNo;
            dr["Line"] = _line;
            dr["QtyReq"] = "0.00";
            dr["BudgetUnit"] = "0.00";
            dr["MatMod"] = "0.00";
            dr["BudgetExt"] = "0.00";
            dr["LabHours"] = "0.00";
            dr["LabRate"] = "0.00";
            dr["LabMod"] = "0.00";
            dr["LabExt"] = "0.00";
            dr["TotalExt"] = "0.00";
            dr["STax"] = 0;
            dr["LSTax"] = 0;
        }
        else if (gridName == "Milestones")
        {
            _orderNo = Int32.Parse(radMilGridClickedRowIndex.Value);
            dr["OrderNo"] = _orderNo;
            dr["Line"] = _line;
            dr["Amount"] = "0.00";
        }


        if (position == "above")
        {
            dt.Rows.InsertAt(dr, _orderNo);
        }
        else if (position == "below")
        {
            dt.Rows.InsertAt(dr, _orderNo + 1);
        }

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            dt.Rows[i]["OrderNo"] = i + 1;
        }

        if (gridName == "Bom")
        {
            ViewState["TempBOM"] = dt;
            BindgvBOM(dt);
        }
        else if (gridName == "Milestones")
        {
            ViewState["TempMilestone"] = dt;
            BindgvMilestones(dt);

        }
    }

    protected void RadMenuMilGrid_ItemClick(object sender, RadMenuEventArgs e)
    {
        DataTable dt = GetMilestoneGridItems();
        switch (e.Item.Text)
        {
            case "Add Row Above":
                AddNewRowGrid(dt, "above", "Milestones");
                break;

            case "Add Row Below":
                AddNewRowGrid(dt, "below", "Milestones");
                break;
        }
    }

    protected void lnkBompostback_Click(object sender, EventArgs e)
    {
        if (hdnloadBom.Value == "0")
        {
            hdnloadBom.Value = "1";
            //gvBOM.Visible = true;
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


    private void GetControle()
    {
        DataSet ds = new DataSet();

        objPropUser.ConnConfig = Session["config"].ToString();

        ds = objBL_User.getControl(objPropUser);

        if (ds.Tables[0].Rows[0]["TargetHPermission"].ToString() == "1")

            chkTargetHours.Visible = true;
        else
            chkTargetHours.Visible = false;
    }

    private void BindSalesTax()
    {
        STax staxData = new STax();
        BL_STax objBL_STax = new BL_STax();
        staxData.ConnConfig = Session["config"].ToString();
        staxData.UType = 1;
        var ds = objBL_STax.GetAllSTaxByUType(staxData);
        if (ds != null && ds.Tables[0] != null)
        {
            ddlSalesTax.DataSource = ds.Tables[0];
            ddlSalesTax.DataTextField = "NameRate";
            ddlSalesTax.DataValueField = "Name";
            ddlSalesTax.DataBind();
            ddlSalesTax.Items.Insert(0, new ListItem("Select Sales Tax", "0"));
        }
    }

    private DataTable CreateMilstoneDataTable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("JobT", typeof(int));
        dt.Columns.Add("Job", typeof(int));
        dt.Columns.Add("JobTItem", typeof(int));
        dt.Columns.Add("jType", typeof(int));
        dt.Columns.Add("fDesc", typeof(string));
        dt.Columns.Add("jcode", typeof(string));
        dt.Columns.Add("Line", typeof(int));
        dt.Columns.Add("MilesName", typeof(string));
        dt.Columns.Add("RequiredBy", typeof(DateTime));
        dt.Columns.Add("LeadTime", typeof(double));
        dt.Columns.Add("ProjAcquistDate", typeof(string));
        dt.Columns.Add("ActAcquistDate", typeof(string));
        dt.Columns.Add("Comments", typeof(string));
        dt.Columns.Add("Type", typeof(int));
        dt.Columns.Add("Department", typeof(string));
        dt.Columns.Add("Amount", typeof(double));
        dt.Columns.Add("OrderNo", typeof(double));
        dt.Columns.Add("GroupId", typeof(int));
        dt.Columns.Add("GroupName", typeof(string));
        dt.Columns.Add("CodeDesc", typeof(string));
        dt.Columns.Add("Quantity", typeof(string));
        dt.Columns.Add("Price", typeof(string));
        dt.Columns.Add("ChangeOrder", typeof(int));

        return dt;
    }

    protected void chkBilFrmBOM_CheckedChanged(object sender, EventArgs e)
    {
        if (chkBilFrmBOM.Checked)
        {
            //var dtBOM = ViewState["TempBOM"] != null ? (DataTable)ViewState["TempBOM"] : null;
            var dtBOM = GetBOMItems();
            if (dtBOM != null)
            {
                var dtBilling = CreateMilstoneDataTable();
                var i = 0;
                var j = 1;
                foreach (DataRow drBOM in dtBOM.Rows)
                {
                    DataRow drBil = dtBilling.NewRow();
                    drBil["Line"] = i;
                    drBil["OrderNo"] = j;
                    drBil["Amount"] = drBOM["TotalExt"];
                    drBil["GroupId"] = 0;
                    drBil["fDesc"] = drBOM["fDesc"];
                    drBil["CodeDesc"] = drBOM["CodeDesc"];
                    drBil["jcode"] = drBOM["Code"];
                    drBil["Quantity"] = 1;
                    drBil["Price"] = drBOM["TotalExt"];
                    
                    dtBilling.Rows.Add(drBil);
                    i++;
                    j++;
                }

                BindgvMilestones(dtBilling);
            }

            ddlEstimateType.SelectedValue = "quote";
        }
    }

    //protected void RadGrid_UserRoles_PreRender(object sender, EventArgs e)
    //{
    //    String filterExpression = Convert.ToString(RadGrid_UserRoles.MasterTableView.FilterExpression);
    //    if (filterExpression != "")
    //    {
    //        Session["PrjTemp_UserRoles_FilterExpression"] = filterExpression;
    //        List<RetainFilter> filters = new List<RetainFilter>();

    //        foreach (GridColumn column in RadGrid_UserRoles.MasterTableView.OwnerGrid.Columns)
    //        {
    //            String filterValues = column.CurrentFilterValue;
    //            if (filterValues != "")
    //            {
    //                String columnName = column.UniqueName;
    //                RetainFilter filter = new RetainFilter();
    //                filter.FilterColumn = columnName;
    //                filter.FilterValue = filterValues;
    //                filters.Add(filter);
    //            }
    //        }

    //        Session["PrjTemp_UserRoles_Filters"] = filters;
    //    }
    //    else
    //    {
    //        Session["PrjTemp_UserRoles_FilterExpression"] = null;
    //        Session["PrjTemp_UserRoles_Filters"] = null;
    //    }

    //    ScriptManager.RegisterStartupScript(this, Page.GetType(), "bindingClickCheckbox", "BindClickEventForGridCheckBox_UR();", true);
    //}

    //protected void RadGrid_UserRoles_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    //{
    //    RadGrid_UserRoles.AllowCustomPaging = !ShouldApplySortFilterOrGroup();
    //    if (!IsPostBack)
    //    {

    //        if (Session["PrjTemp_UserRoles_FilterExpression"] != null && Convert.ToString(Session["PrjTemp_UserRoles_FilterExpression"]) != "" && Session["PrjTemp_UserRoles_Filters"] != null)
    //        {
    //            RadGrid_UserRoles.MasterTableView.FilterExpression = Convert.ToString(Session["PrjTemp_UserRoles_FilterExpression"]);
    //            var filtersGet = Session["PrjTemp_UserRoles_Filters"] as List<RetainFilter>;
    //            if (filtersGet != null)
    //            {
    //                foreach (var _filter in filtersGet)
    //                {
    //                    GridColumn column = RadGrid_UserRoles.MasterTableView.GetColumnSafe(_filter.FilterColumn);
    //                    column.CurrentFilterValue = _filter.FilterValue;
    //                }
    //            }
    //        }

    //    }

    //    InitUserRoleGridView();
    //}

    //private void InitUserRoleGridView()
    //{
    //    DataSet ds = new DataSet();
    //    UserRole userRole = new UserRole();
    //    userRole.ConnConfig = Session["config"].ToString();
    //    userRole.SearchBy = "";
    //    userRole.SearchValue = "";

    //    ds = objBL_User.GetRoleSearch(userRole, false);
    //    var userRoles = ds.Tables[0];

    //    RadGrid_UserRoles.DataSource = userRoles;
    //    if (userRoles != null && userRoles.Rows.Count > 0)
    //    {
    //        RadGrid_UserRoles.VirtualItemCount = userRoles.Rows.Count;
    //    }
    //    else
    //    {
    //        RadGrid_UserRoles.VirtualItemCount = 0;
    //    }

    //}
}
