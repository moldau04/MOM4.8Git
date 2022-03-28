using System;
//using System.Collections;
//using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
//using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
//using System.Web.UI.WebControls.WebParts;
//using System.Xml.Linq;
using BusinessLayer;
using BusinessEntity;
using System.Web.Script.Serialization;
using System.Collections.Generic;
using System.IO;
using System.Text;
//using AjaxControlToolkit;
//using System.Globalization;
using System.Drawing;
using Telerik.Web.UI;
using Microsoft.Reporting.WebForms;
using Stimulsoft.Report;
using BusinessLayer.Schedule;
using BusinessLayer.Billing;

public partial class addProject_New : System.Web.UI.Page
{
    #region "Variables"
    String PageSize = "10";
    int count_inv = 0;
    bool IsGst = false;
    string reportPath = "";
    byte[] array = null;
    BL_Job objBL_Job = new BL_Job();
    JobT objJob = new JobT();
    protected DataTable dtBomType = new DataTable();
    protected DataTable dtBomItem = new DataTable();
    protected DataTable dtApplicationStatus = new DataTable();
    protected DataTable dtBillingCodeData = new DataTable();

    //protected DataTable dtLabourMaterial = new DataTable();
    //protected DataTable dtInventoryItem = new DataTable();

    private string headerJobType = "";

    BL_Contracts objBL_Contracts = new BL_Contracts();
    Contracts objProp_Contracts = new Contracts();

    BL_Customer objBL_Customer = new BL_Customer();
    Customer objProp_Customer = new Customer();

    BL_MapData objBL_MapData = new BL_MapData();
    MapData objMapData = new MapData();

    GeneralFunctions objgn = new GeneralFunctions();
    BL_BankAccount objBL_Bank = new BL_BankAccount();

    State objState = new State();

    User objPropUser = new User();
    BL_User objBL_User = new BL_User();

    BL_General objBL_General = new BL_General();
    General objGeneral = new General();

    BusinessEntity.Planner objPlanner = new BusinessEntity.Planner();
    BL_Planner objBL_Planner = new BL_Planner();

    protected DataTable dtCustomField = new DataTable();
    protected DataTable dtMat = new DataTable();
    protected DataTable dtLab = new DataTable();

    Wage objWage = new Wage();
    private const string ASCENDING = " ASC";
    private const string DESCENDING = " DESC";

    #endregion

    #region Events
    public bool IsCustomExist
    {
        get
        {
            if (ViewState["IsCustomExist"] == null)
                ViewState["IsCustomExist"] = false;
            return (bool)ViewState["IsCustomExist"];
        }
        set
        {
            ViewState["IsCustomExist"] = value;
        }
    }
    #region PAGELOAD

    protected void Page_Load(object sender, EventArgs e)
    {
        //try
        //{

        //gvBudget.RowDataBound += new GridViewRowEventHandler(gvBudget_RowDataBound);

        if (Session["userid"] == null)
        {
            Response.Redirect("login.aspx");
        }
        if (!CheckAddEditPermission()) { Response.Redirect("Home.aspx?permission=no"); return; }
        FillBomType();
        FillApplicationStatus();
        //FillInventory();
        FillWage();
        FillBillCodes();
        if (!IsPostBack)
        {
            if (!System.IO.Directory.Exists(Server.MapPath(Request.ApplicationPath) + "/TempPDF/SendInvoice"))
                System.IO.Directory.CreateDirectory(Server.MapPath(Request.ApplicationPath) + "/TempPDF/SendInvoice");

            Fillterritory();
            GetControl();
            FillProjectsTemplate();
            FillState();
            FillContractType();
            FillJobType();
            FillJobStatus();
            FillPosting();
            FillCategory();
            FillWorker();
            getDiagnosticCategory();
            FillTerms();

            txtProjCreationDate.Text = DateTime.Now.ToShortDateString();
            DisableControl();
            Initialize();
            Fill_gvContacts();
            if (Request.QueryString["locid"] != null)
            {
                hdnLocID.Value = Request.QueryString["locid"].ToString();
                btnSelectLoc_Click(sender, e);
            }

            if (Request.QueryString["uid"] != null)
            {
                hdnprojectID.Value = Request.QueryString["uid"];
                if (Request.QueryString["tab"] != null)
                {
                    if (Request.QueryString["tab"] == "budget")
                    {
                        //TabContainerHeader.ActiveTab = tbpnlFinance;
                        //TabContainerFinance.ActiveTab = tbpnlBudgets;
                        tbpnlFinance.Visible = true;
                        tbpnlBudgets.Visible = true;
                        tbpnlBudgets.Focus();
                    }
                }
                //pnlNext.Visible = true;
                lblHeader.Text = "Edit Project";
                GetData();
                BindBudget(1);
                BindExpense(1);
                GetAttachment();

                if (!ddlTemplate.SelectedValue.Equals("Select Template"))
                {
                    ddlTemplate.Enabled = false;
                }
                objJob.ConnConfig = Session["config"].ToString();
                objJob.Job = Convert.ToInt32(Request.QueryString["uid"]);

                DataSet ds = objBL_Job.GetBudgetSummaryGridDataByJob(objJob);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    gvBudgetGrid.DataSource = ds.Tables[0];
                    gvBudgetGrid.DataBind();
                    GridFooterItem footeritem = (GridFooterItem)gvBudgetGrid.MasterTableView.GetItems(GridItemType.Footer)[0];
                    if (footeritem != null)
                    {
                        Label lblRevFooter = footeritem.FindControl("lblRevFooter") as Label;
                        Label lblLaborExpFooter = footeritem.FindControl("lblLaborExpFooter") as Label;
                        Label lblMaterialExpFooter = footeritem.FindControl("lblMatExpFooter") as Label;
                        Label lblOtherExpFooter = footeritem.FindControl("lblOtherExpFooter") as Label;
                        Label lblTotalExpFooter = footeritem.FindControl("lblCostFooter") as Label;
                        Label lblProfitAmtFooter = footeritem.FindControl("lblProfitFooter") as Label;
                        Label lblNetProfitFooter = footeritem.FindControl("lblRatioFooter") as Label;
                        Label lblHoursFooter = footeritem.FindControl("lblHoursFooter") as Label;
                        Label lblTotalOnOrderFooter = footeritem.FindControl("lblOnOrderFooter") as Label;

                        DataRow dr1 = ds.Tables[0].Rows[0];
                        DataRow dr2 = ds.Tables[0].Rows[1];

                        lblRevFooter.Text = string.Format("{0:c}", Convert.ToDouble(dr1["Rev"]) - Convert.ToDouble(dr2["Rev"]));
                        lblLaborExpFooter.Text = string.Format("{0:c}", Convert.ToDouble(dr1["Labor"]) - Convert.ToDouble(dr2["Labor"]));
                        lblMaterialExpFooter.Text = string.Format("{0:c}", Convert.ToDouble(dr1["Mat"]) - Convert.ToDouble(dr2["Mat"]));
                        lblOtherExpFooter.Text = string.Format("{0:c}", Convert.ToDouble(dr1["OtherExp"]) - Convert.ToDouble(dr2["OtherExp"]));
                        lblTotalExpFooter.Text = string.Format("{0:c}", Convert.ToDouble(dr1["Cost"]) - Convert.ToDouble(dr2["Cost"]));
                        lblProfitAmtFooter.Text = string.Format("{0:c}", Convert.ToDouble(dr1["Profit"]) - Convert.ToDouble(dr2["Profit"]));
                        lblNetProfitFooter.Text = string.Format("{0:0.00}", Convert.ToDouble(dr1["Ratio"]) - Convert.ToDouble(dr2["Ratio"]));
                        lblHoursFooter.Text = string.Format("{0:0.00}", Convert.ToDouble(dr1["hour"]) - Convert.ToDouble(dr2["hour"]));
                        lblTotalOnOrderFooter.Text = string.Format("{0:c}", Convert.ToDouble(dr1["OnOrder"]) - Convert.ToDouble(dr2["OnOrder"]));
                    }

                }

                #region Check on Planner Tab Data
                PlannerCheck();
                #endregion

            }
            else
            {
                ddlJobStatus.SelectedValue = "0";
                divAP.Visible = false;
                divTickets.Visible = false;
                divInvoices.Visible = false;
                divJC.Visible = false;
                tbpnAttachment.Visible = false;
                liaccrdDocument.Visible = false;
                tbpnContact.Visible = false;
                liaccrdContacts.Visible = false;
                btnSave.Visible = false;
                btnWIPCancel.Visible = false;
                #region Hide The Planner Tab
                TabPanel3.Visible = false;
                liaccrdPlanner.Visible = false;
                #endregion
            }

            BindEquip();
            divJC.Visible = false;

            ddlJobType.Enabled = false;
            Permission();
            #region Check IsGstRate
            objGeneral.ConnConfig = Session["config"].ToString();
            objGeneral.CustomName = "Country";
            DataSet dsCustom = objBL_General.getCustomFields(objGeneral);

            if (dsCustom.Tables[0].Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(dsCustom.Tables[0].Rows[0]["Label"].ToString()) && dsCustom.Tables[0].Rows[0]["Label"].ToString().Equals("1"))
                {
                    IsGst = true;
                }
            }
            #endregion
            ViewState["IsGst"] = IsGst;
        }
        if (Page.IsPostBack)                // comment 8.16.16
        {
            #region rebind custom fields
            if (IsCustomExist)                    // commented by Mayuri on 28th july, 16 incomplete custom field func.
            {
                bool IsTemplateDdl = false;
                Control ctrl = null;

                string ctrlName = Page.Request.Params.Get("__EVENTTARGET");

                if (!String.IsNullOrEmpty(ctrlName))
                {
                    ctrl = Page.FindControl(ctrlName);
                    if (ctrl.ID == ddlTemplate.ID)       //if ddltemplate caused postback then do not update previous template's custom fields
                    {
                        IsTemplateDdl = true;
                    }
                    if (ctrl.ID == lnkSaveTemplate.ID)
                    {
                        IsTemplateDdl = true;
                    }
                }
                if (!IsTemplateDdl)
                {
                    objJob.ConnConfig = Session["config"].ToString();
                    if (ddlTemplate.SelectedValue.ToString() != "Select Template")
                    {
                        objJob.ID = Convert.ToInt32(ddlTemplate.SelectedValue);
                    }
                    if (Request.QueryString["uid"] != null)
                    {
                        objJob.Job = Convert.ToInt32(Request.QueryString["uid"]);
                    }
                    DataSet dsTemp = new DataSet();
                    dsTemp = objBL_Job.GetProjectTemplateCustomFields(objJob);
                    DataTable dtCustom = GetCustomItems();
                    if (dtCustom == null)
                        dtCustom = dsTemp.Tables[0];
                    if (dsTemp.Tables[0].Rows.Count > 0)
                    {
                        CreateCustomTable();
                        DisplayCustomByTab(dtCustom, dsTemp.Tables[1], objJob.ID);
                    }
                }
            }
            #endregion
        }
        CompanyPermission();
        //}
        //catch (Exception ex)
        //{
        //    throw ex;
        //}
    }
    #endregion

    #region BOM
    //protected void gvBOM_RowDataBound(object sender, GridViewRowEventArgs e)
    //{
    //    //BOM Item list
    //    switch (e.Row.RowType)
    //    {
    //        case DataControlRowType.DataRow:

    //            DropDownList ddlBType = (e.Row.FindControl("ddlBType") as DropDownList);
    //            DropDownList ddlItem = (e.Row.FindControl("ddlItem") as DropDownList);
    //            //TextBox txtScrapFactor = (e.Row.FindControl("txtScrapFactor") as TextBox);
    //            Label lblBudgetExt = (e.Row.FindControl("lblBudgetExt") as Label);
    //            TextBox txtBudgetUnit = (e.Row.FindControl("txtBudgetUnit") as TextBox);
    //            TextBox txtQtyReq = (e.Row.FindControl("txtQtyReq") as TextBox);
    //            HiddenField hdnItem = (e.Row.FindControl("hdnItem") as HiddenField);

    //            double _budgetExt = 0.0;
    //            double _qtyReq = 0.0;
    //            if (ddlBType.SelectedValue.Equals("1"))       // on select of Materials, fill inv data
    //            {
    //                FillInventory(ddlItem);
    //                //txtScrapFactor.Enabled = true;

    //                //if (!string.IsNullOrEmpty(txtScrapFactor.Text) && !string.IsNullOrEmpty(txtQtyReq.Text))
    //                //{
    //                //    _qtyReq = Convert.ToDouble(txtQtyReq.Text) + Convert.ToDouble(txtScrapFactor.Text);
    //                //}
    //            }
    //            else if (ddlBType.SelectedValue.Equals("2"))  // on select of Labor, fill wage data
    //            {
    //                FillWage(ddlItem);
    //                //txtScrapFactor.Enabled = false;
    //            }
    //            else
    //            {
    //                FillItems(ddlItem);
    //                //txtScrapFactor.Enabled = false;
    //            }

    //            if (!string.IsNullOrEmpty(hdnItem.Value))
    //            {
    //                ddlItem.SelectedValue = hdnItem.Value;
    //            }
    //            if (!string.IsNullOrEmpty(txtBudgetUnit.Text))
    //            {
    //                if (!string.IsNullOrEmpty(txtQtyReq.Text))
    //                {
    //                    if (_qtyReq.Equals(0))
    //                        _qtyReq = Convert.ToDouble(txtQtyReq.Text);
    //                }
    //                _budgetExt = _qtyReq * Convert.ToDouble(txtBudgetUnit.Text);
    //            }
    //            //lblBudgetExt.Text = _budgetExt.ToString("0.00", CultureInfo.InvariantCulture);

    //            //if (ViewState["TempBOM"] != null)
    //            //{
    //            //    DataTable _dtBom = (DataTable)ViewState["TempBOM"];
    //            //    if (_dtBom.Rows.Count > 0)
    //            //    {
    //            //        foreach (var item in _dtBom.Rows)
    //            //        {
    //            //            var itemVal = _dtBom.Rows[0]["BItem"].ToString();
    //            //            if (!string.IsNullOrEmpty(itemVal))
    //            //            {
    //            //                if (e.Row.RowType == DataControlRowType.DataRow)
    //            //                {
    //            //                    itemVal = DataBinder.Eval(e.Row.DataItem, "BItem").ToString();
    //            //                    ddlItem.SelectedValue = itemVal.ToString();
    //            //                }
    //            //            }
    //            //        }
    //            //    }
    //            //}

    //            break;
    //    }
    //}
    protected void gvBOM_RowCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        if (e.CommandName.Equals("AddProject"))
        {
            DataTable dt = GetBOMGridItems();

            string maxvalue = dt.AsEnumerable().Max(row => row["Line"]).ToString();
            Int32 _line = Convert.ToInt32(maxvalue) + 1;
            for (int j = 0; j < 1; j++)
            {
                DataRow dr = dt.NewRow();
                dr["Line"] = _line;
                dt.Rows.Add(dr);
                _line = _line + 1;
            }

            //ViewState["ProjectTemplate"] = dt;
            ViewState["TempBOM"] = dt;
            BindgvBOM(dt);

            ScriptManager.RegisterStartupScript(this, Page.GetType(), "funMaterialLabor", "funMaterialLabor()", true);
            //Session["gvBOM"] = dt;
        }
    }
    //protected void ddlBType_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    DropDownList ddlBomType = (DropDownList)sender;
    //    GridViewRow gridrow = (GridViewRow)ddlBomType.NamingContainer;
    //    int rowIndex = gridrow.RowIndex;

    //    foreach (GridViewRow gr in gvBOM.Rows)
    //    {
    //        if (gr.RowIndex == rowIndex)
    //        {
    //            DropDownList ddlBType = (DropDownList)gr.FindControl("ddlBType");
    //            DropDownList ddlItem = (DropDownList)gr.FindControl("ddlItem");
    //            TextBox txtBudgetUnit = (TextBox)gr.FindControl("txtBudgetUnit");
    //            //TextBox txtScrapFactor = (TextBox)gr.FindControl("txtScrapFactor");
    //            TextBox txtQtyReq = (TextBox)gr.FindControl("txtQtyReq");
    //            Label lblBudgetExt = (Label)gr.FindControl("lblBudgetExt");
    //            HiddenField hdnBudgetExt = (HiddenField)gr.FindControl("hdnBudgetExt");

    //            if (ddlBType.SelectedValue.Equals("1"))       // on select of Materials, fill inv data
    //            {
    //                //txtScrapFactor.Enabled = true;
    //                FillInventory(ddlItem);
    //            }
    //            else if (ddlBType.SelectedValue.Equals("2"))  // on select of Labor, fill wage data
    //            {
    //                //txtScrapFactor.Enabled = false;
    //                FillWage(ddlItem);
    //            }
    //            else
    //            {
    //                //txtScrapFactor.Enabled = false;
    //                FillItems(ddlItem);
    //            }
    //            double _budgetExt = 0;
    //            if (ddlBType.SelectedValue.Equals("1")) // if Materials
    //            {
    //                double _qtyReq = 0.0;
    //                //double _budgetUnit = 0.0;
    //                //double _scrapFact = 0.0;


    //                if (!string.IsNullOrEmpty(txtBudgetUnit.Text) && !string.IsNullOrEmpty(txtQtyReq.Text))
    //                {
    //                    //if (!string.IsNullOrEmpty(txtScrapFactor.Text))
    //                    //{
    //                    //    //_budgetExt = _budgetExt * Convert.ToDouble(txtScrapFactor.Text);
    //                    //    _qtyReq = Convert.ToDouble(txtQtyReq.Text) + Convert.ToDouble(txtScrapFactor.Text);
    //                    //}
    //                    //else
    //                    _qtyReq = Convert.ToDouble(txtQtyReq.Text);
    //                    _budgetExt = _qtyReq * Convert.ToDouble(txtBudgetUnit.Text);
    //                    //_budgetExt = Convert.ToDouble(txtBudgetUnit.Text) * Convert.ToDouble(txtQtyReq.Text);

    //                }
    //                lblBudgetExt.Text = _budgetExt.ToString("0.00", CultureInfo.InvariantCulture);
    //                hdnBudgetExt.Value = _budgetExt.ToString("0.00", CultureInfo.InvariantCulture);
    //            }
    //            else
    //            {
    //                //txtScrapFactor.Text = "";
    //                if (!string.IsNullOrEmpty(txtBudgetUnit.Text) && !string.IsNullOrEmpty(txtQtyReq.Text))
    //                {
    //                    _budgetExt = Convert.ToDouble(txtBudgetUnit.Text) * Convert.ToDouble(txtQtyReq.Text);
    //                }
    //                lblBudgetExt.Text = _budgetExt.ToString("0.00", CultureInfo.InvariantCulture);
    //                hdnBudgetExt.Value = _budgetExt.ToString("0.00", CultureInfo.InvariantCulture);
    //            }

    //        }
    //    }

    //    foreach (GridViewRow gr in gvBOM.Rows)
    //    {
    //        Label lblBudgetExt = (Label)gr.FindControl("lblBudgetExt");
    //        HiddenField hdnBudgetExt = (HiddenField)gr.FindControl("hdnBudgetExt");

    //        lblBudgetExt.Text = hdnBudgetExt.Value;
    //    }
    //}
    #endregion


    public void PlannerCheck()
    {
        objPlanner.ConnConfig = Session["config"].ToString();
        objPlanner.ProjectID = Convert.ToInt32(Request.QueryString["uid"]);
        DataSet dsPlanner = new DataSet();
        dsPlanner = objBL_Planner.GetPlannerByProjectID(objPlanner);
        if (dsPlanner.Tables[0] != null && dsPlanner.Tables[0].Rows.Count > 0)
        {
            if (Convert.ToString(dsPlanner.Tables[0].Rows[0]["ID"]) != "" && Convert.ToString(dsPlanner.Tables[0].Rows[0]["ID"]) != "0")
            {
                lnkAddPlanner.Visible = false;
                lnkPlannerID.Visible = true;

                //TO DO -- Need to make this Information Dynamic.
                lnkPlannerID.Text = "#" + Convert.ToString(dsPlanner.Tables[0].Rows[0]["ID"]);
                lblPlannerDescription.Text = Convert.ToString(dsPlanner.Tables[0].Rows[0]["Desc"]);
                if (Convert.ToString(dsPlanner.Tables[0].Rows[0]["StartDate"]) != "")
                {
                    lblStartDate.Text = Convert.ToDateTime(dsPlanner.Tables[0].Rows[0]["StartDate"]).ToShortDateString();
                }

                if (Convert.ToString(dsPlanner.Tables[0].Rows[0]["EndDate"]) != "")
                {
                    lblFinishDate.Text = Convert.ToDateTime(dsPlanner.Tables[0].Rows[0]["EndDate"]).ToShortDateString();
                }
                lblNumberofTasks.Text = Convert.ToString(dsPlanner.Tables[0].Rows[0]["NoOfTask"]);
                lblNextDueTask.Text = Convert.ToString(dsPlanner.Tables[0].Rows[0]["NextDueTask"]);
                lblInProgressTask.Text = Convert.ToString(dsPlanner.Tables[0].Rows[0]["ProgressTask"]);
                lblTotalHours.Text = Convert.ToString(dsPlanner.Tables[0].Rows[0]["TotalHours"]);
                lblTotalDays.Text = Convert.ToString(dsPlanner.Tables[0].Rows[0]["TotalDays"]);
            }
            else
            {
                lnkPlannerID.Visible = false;
                PlannerInfo.Visible = false;
            }

        }
        else
        {
            lnkPlannerID.Visible = false;
            PlannerInfo.Visible = false;
        }
    }
    public bool CheckAddEditPermission()
    {
        bool result = true;
        if (Session["type"].ToString() != "am" && Session["type"].ToString() != "c")
        {
            DataTable ds = new DataTable();
            ds = (DataTable)Session["userinfo"];

            /// job ///////////////////------->

            string jobPermission = ds.Rows[0]["job"] == DBNull.Value ? "YYYYYY" : ds.Rows[0]["job"].ToString();

            string stAddejob = jobPermission.Length < 1 ? "Y" : jobPermission.Substring(0, 1);
            string stEditejob = jobPermission.Length < 2 ? "Y" : jobPermission.Substring(1, 1);
            string stDeleteJob = jobPermission.Length < 3 ? "Y" : jobPermission.Substring(2, 1);
            string stViewJob = jobPermission.Length < 4 ? "Y" : jobPermission.Substring(3, 1);


            if (Request.QueryString["uid"] != null)
            {
                if (stViewJob == "N")
                {
                    result = false;
                }
                else if (stViewJob == "Y" && stEditejob == "N")
                {
                    lnkSaveTemplate.Visible = false;
                }
            }
            else
            {
                if (stAddejob == "N")
                {
                    result = false;
                }
            }


            //Finance
            string FinancePermission = ds.Rows[0]["FinancePermission"] == DBNull.Value ? "Y" : ds.Rows[0]["FinancePermission"].ToString();
            gvBudgetGrid.Visible = FinancePermission == "N" ? false : true;
            hdnFinancePermission.Value = FinancePermission;
            //bom
            string BOMPermission = ds.Rows[0]["BOMPermission"] == DBNull.Value ? "YYYY" : ds.Rows[0]["BOMPermission"].ToString();
            string stViewBOM = BOMPermission.Length < 4 ? "Y" : BOMPermission.Substring(3, 1);

            string stEditBOM = BOMPermission.Length < 2 ? "Y" : BOMPermission.Substring(1, 1);

            gvBOM.Visible = stViewBOM == "N" ? false : true;
            hdnBOMPermission.Value = BOMPermission;

            chkMaterial.Visible = chkLabor.Visible = stViewBOM == "N" ? false : true;

            chkMaterial.Enabled = chkLabor.Enabled = stEditBOM == "N" ? false : true;

            //Milestones
            string MilestonesPermission = ds.Rows[0]["MilestonesPermission"] == DBNull.Value ? "YYYY" : ds.Rows[0]["MilestonesPermission"].ToString();
            string stViewMilestones = MilestonesPermission.Length < 4 ? "Y" : MilestonesPermission.Substring(3, 1);

            hdnMilestonesPermission.Value = MilestonesPermission;
            gvMilestones.Visible = stViewMilestones == "N" ? false : true;

        }

        return result;
    }
    protected void lnkSaveTemplate_Click(object sender, EventArgs e)
    {
        try
        {
            //objProp_Customer.TemplateID = Convert.ToInt32(ddlEstimates.SelectedValue);
            //objProp_Customer.Description = txtREPdesc.Text.Trim();
            //objProp_Customer.DtCustom = GetCustomItems();
            objProp_Customer.ConnConfig = Session["config"].ToString();

            #region add custom fields
            DataTable dtCustom = GetCustomItems();                // comment 8.16.16
            if (dtCustom != null)
            {
                if (dtCustom.Rows.Count > 0)
                {
                    dtCustom.Columns.Remove("ControlID");
                    dtCustom.Select("Format = 5 and Value = 'on'")
                        .AsEnumerable().ToList()
                        .ForEach(t => t["Value"] = true);
                    dtCustom.AcceptChanges();

                    dtCustom.Select("Format = 5 and Value = 'true'")
                       .AsEnumerable().ToList()
                       .ForEach(t => t["UpdatedDate"] = DateTime.Now);
                    dtCustom.AcceptChanges();

                    dtCustom.Select("Format = 5 and Value = 'true'")
                       .AsEnumerable().ToList()
                       .ForEach(t => t["Username"] = Session["username"].ToString());
                    dtCustom.AcceptChanges();

                    dtCustom.Select("Format = 5 and Value = 'off'")
                        .AsEnumerable().ToList()
                        .ForEach(t => t["Value"] = false);
                    dtCustom.AcceptChanges();

                    dtCustom.Select("Format = 5 and Value = ''")
                      .AsEnumerable().ToList()
                      .ForEach(t => t["Value"] = false);
                    dtCustom.AcceptChanges();

                    objProp_Customer.DtCustom = dtCustom;
                }
            }
            #endregion


            objProp_Customer.DtTeam = GetTeamItems();
            if (!string.IsNullOrEmpty(hdnCustID.Value))
            {
                objProp_Customer.CustomerID = Convert.ToInt32(hdnCustID.Value);
            }
            if (!string.IsNullOrEmpty(hdnLocID.Value))
            {
                objProp_Customer.LocID = Convert.ToInt32(hdnLocID.Value);
            }
            objProp_Customer.Name = txtREPdesc.Text.Trim();
            if (!ddlJobStatus.SelectedValue.Equals("Select Status"))
            {
                objProp_Customer.Status = Convert.ToInt32(ddlJobStatus.SelectedValue);
            }
            if (!ddlJobType.SelectedValue.Equals("Select Type"))
            {
                objProp_Customer.Type = ddlJobType.SelectedValue;
            }
            objProp_Customer.Remarks = txtREPremarks.Text.Trim();
            //add job.address

            if (!ddlTemplate.SelectedValue.Equals("Select Template"))
            {
                objProp_Customer.TemplateID = Convert.ToInt32(ddlTemplate.SelectedValue);
            }

            if (!string.IsNullOrEmpty(txtProjCreationDate.Text))
            {
                objProp_Customer.ProjectCreationDate = Convert.ToDateTime(txtProjCreationDate.Text);
            }
            else
            {
                objProp_Customer.ProjectCreationDate = DateTime.Now;
            }

            objProp_Customer.PO = txtPO.Text;
            objProp_Customer.SO = txtSalesOrder.Text;
            //add attach PO
            objProp_Customer.Certified = Convert.ToInt16(chkCertifiedJob.Checked);
            objProp_Customer.Custom1 = txtCustom1.Text;
            objProp_Customer.Custom2 = txtCustom2.Text;
            objProp_Customer.Custom3 = txtCustom3.Text;
            objProp_Customer.Custom4 = txtCustom4.Text;
            if (!string.IsNullOrEmpty(txtCustom5.Text))
            {
                objProp_Customer.Custom5 = Convert.ToDateTime(txtCustom5.Text);
            }

            #region rol detail
            objProp_Customer.RolName = txtName.Text;
            objProp_Customer.City = txtCity.Text;
            objProp_Customer.Address = GctxtAddress.Text;

            if (!ddlState.SelectedValue.Equals("Select State"))
            {
                objProp_Customer.State = ddlState.SelectedValue;
            }
            objProp_Customer.Zip = txtPostalCode.Text;
            objProp_Customer.Country = txtCountry.Text;
            objProp_Customer.Phone = txtPhone.Text;
            objProp_Customer.Cellular = txtMobile.Text;
            objProp_Customer.Fax = txtFax.Text;
            objProp_Customer.Contact = txtContactName.Text;
            objProp_Customer.Email = txtEmailWeb.Text;
            objProp_Customer.RolRemarks = txtRemarks.Text;
            objProp_Customer.RolType = 8;

            #endregion

            #region finance-general

            if (!string.IsNullOrEmpty(uc_InvExpGL._hdnAcctID.Value))
            {
                objProp_Customer.InvExp = Convert.ToInt32(uc_InvExpGL._hdnAcctID.Value);
            }
            if (!string.IsNullOrEmpty(uc_InterestGL._hdnAcctID.Value))
            {
                objProp_Customer.GLInt = Convert.ToInt32(uc_InterestGL._hdnAcctID.Value);
            }
            if (!string.IsNullOrEmpty(hdnInvServiceID.Value))
            {
                objProp_Customer.InvServ = Convert.ToInt32(hdnInvServiceID.Value);
            }
            if (!string.IsNullOrEmpty(hdnPrevilWageID.Value))
            {
                objProp_Customer.Wage = Convert.ToInt32(hdnPrevilWageID.Value);
            }
            //if (!ddlContractType1.SelectedValue.Equals("0"))
            //{
            //    objProp_Customer.JobTempCtype = ddlContractType1.SelectedValue;
            //}
            if (!ddlContractType1.SelectedValue.Equals("0"))
            {
                objProp_Customer.ctypeName = ddlContractType1.SelectedValue;
            }
            if (!ddlPostingMethod.SelectedValue.Equals("0"))
            {
                objProp_Customer.Post = Convert.ToInt16(ddlPostingMethod.SelectedValue);
            }
            if (chkChargeInt.Checked.Equals(true))
            {
                objProp_Customer.fInt = 1;
            }
            else
                objProp_Customer.fInt = 0;

            if (chkInvoicing.Checked.Equals(true))
            {
                objProp_Customer.JobClose = 1;
            }
            else
                objProp_Customer.JobClose = 0;

            if (chkChargeable.Checked.Equals(true))
            {
                objProp_Customer.Charge = 1;
            }
            else
                objProp_Customer.Charge = 0;

            #endregion

            #region Milestones
            DataTable dtMilestone = new DataTable();
            dtMilestone = GetMilestoneItems();
            dtMilestone.Columns.Remove("Department");

            int mline = 1;

            if (ViewState["mLine"] == null)
            {
                dtMilestone.AsEnumerable().ToList()
                    .ForEach(t => t["Line"] = mline++);
                dtMilestone.AcceptChanges();
            }
            else
            {
                mline = (Int16)ViewState["mLine"];
                mline++;
                dtMilestone.Select("Line = 0")
                    .AsEnumerable().ToList()
                    .ForEach(t => t["Line"] = mline++);
                dtMilestone.AcceptChanges();
            }
            #endregion

            #region BOM

            DataTable dtBom = GetBomItems();

            int bline = 1;

            if (ViewState["bLine"] == null)
            {
                dtBom.AsEnumerable().ToList()
                    .ForEach(t => t["Line"] = bline++);
                dtBom.AcceptChanges();
            }
            else
            {
                bline = (Int16)ViewState["bLine"];
                bline++;
                dtBom.Select("Line = 0")
                    .AsEnumerable().ToList()
                    .ForEach(t => t["Line"] = bline++);
                dtBom.AcceptChanges();
            }
            #endregion

            if (Request.QueryString["uid"] != null)
            {
                objProp_Customer.ProjectJobID = Convert.ToInt32(Request.QueryString["uid"].ToString());
            }

            objProp_Customer.DtBOM = dtBom;
            objProp_Customer.DtMilestone = dtMilestone;
            if (!string.IsNullOrEmpty(txtBillRate.Text))
            {
                objProp_Customer.BillRate = Convert.ToDouble(txtBillRate.Text);
            }
            if (!string.IsNullOrEmpty(txtOt.Text))
            {
                objProp_Customer.RateOT = Convert.ToDouble(txtOt.Text);
            }
            if (!string.IsNullOrEmpty(txtNt.Text))
            {
                objProp_Customer.RateNT = Convert.ToDouble(txtNt.Text);
            }
            if (!string.IsNullOrEmpty(txtDt.Text))
            {
                objProp_Customer.RateDT = Convert.ToDouble(txtDt.Text);
            }
            if (!string.IsNullOrEmpty(txtTravel.Text))
            {
                objProp_Customer.RateTravel = Convert.ToDouble(txtTravel.Text);
            }
            if (!string.IsNullOrEmpty(txtMileage.Text))
            {
                objProp_Customer.Mileage = Convert.ToDouble(txtMileage.Text);
            }
            if (hdnGCID.Value != string.Empty)
                objProp_Customer.RoleID = Convert.ToInt32(hdnGCID.Value);

            objProp_Customer.dtTaskCode = TaskCodes();
            objProp_Customer.taskcategory = ddlCodeCat.SelectedValue;
            objProp_Customer.SRemarks = txtSpecialInstructions.Text;
            objProp_Customer.Handle = chkspnotes.Checked ? 1 : 0;
            objProp_Customer.IsRenewalNotes = chkRenew.Checked ? 1 : 0;
            objProp_Customer.RenewalNotes = txtRenew.Text;

            #region  add GC and HomeOwner in Role Table

            DataTable tblGCandHomeOwner = new DataTable();

            tblGCandHomeOwner.Columns.Add("ID", typeof(int));
            tblGCandHomeOwner.Columns.Add("NAME", typeof(string));
            tblGCandHomeOwner.Columns.Add("City", typeof(string));
            tblGCandHomeOwner.Columns.Add("State", typeof(string));
            tblGCandHomeOwner.Columns.Add("Zip", typeof(string));
            tblGCandHomeOwner.Columns.Add("Phone", typeof(string));
            tblGCandHomeOwner.Columns.Add("Fax", typeof(string));
            tblGCandHomeOwner.Columns.Add("Contact", typeof(string));
            tblGCandHomeOwner.Columns.Add("Remarks", typeof(string));
            tblGCandHomeOwner.Columns.Add("Country", typeof(string));
            tblGCandHomeOwner.Columns.Add("Cellular", typeof(string));
            tblGCandHomeOwner.Columns.Add("EMail", typeof(string));
            tblGCandHomeOwner.Columns.Add("Type", typeof(int));
            tblGCandHomeOwner.Columns.Add("Address", typeof(string));
            if (!string.IsNullOrEmpty(txtName.Text))
            {
                DataRow drGC = tblGCandHomeOwner.NewRow();

                if (!string.IsNullOrEmpty(hdnGContractorID.Value))
                { drGC["ID"] = hdnGContractorID.Value; }
                else { drGC["ID"] = "0"; }

                drGC["Type"] = 1;
                drGC["NAME"] = txtName.Text;
                drGC["City"] = txtCity.Text;
                drGC["Address"] = GctxtAddress.Text;
                if (!ddlState.SelectedValue.Equals("Select State"))
                    drGC["State"] = ddlState.SelectedValue;
                else drGC["State"] = "0";

                drGC["Zip"] = txtPostalCode.Text;
                drGC["Phone"] = txtPhone.Text;
                drGC["Fax"] = txtFax.Text;
                drGC["Contact"] = txtContactName.Text;
                drGC["Remarks"] = txtRemarks.Text;
                drGC["Country"] = txtCountry.Text;
                drGC["Cellular"] = txtMobile.Text;
                drGC["EMail"] = txtEmailWeb.Text;
                tblGCandHomeOwner.Rows.Add(drGC);
            }
            if (!string.IsNullOrEmpty(hotxtname.Text))
            {
                DataRow drHomeOwner = tblGCandHomeOwner.NewRow();
                if (!string.IsNullOrEmpty(hdnHomeOwnerID.Value))
                { drHomeOwner["ID"] = hdnHomeOwnerID.Value; }
                else { drHomeOwner["ID"] = "0"; }
                drHomeOwner["Type"] = 2;
                drHomeOwner["NAME"] = hotxtname.Text;
                drHomeOwner["City"] = hotxtcity.Text;
                drHomeOwner["Address"] = HotxtAddress.Text;
                if (!hotddlstate.SelectedValue.Equals("Select State"))
                    drHomeOwner["State"] = hotddlstate.SelectedValue;
                else drHomeOwner["State"] = "0";

                drHomeOwner["Zip"] = hotxtZIP.Text;
                drHomeOwner["Phone"] = hotxtPhone.Text;
                drHomeOwner["Fax"] = HotxtFax.Text;
                drHomeOwner["Contact"] = hotxtContactName.Text;
                drHomeOwner["Remarks"] = hotxtRemarks.Text;
                drHomeOwner["Country"] = hotxtCountry.Text;
                drHomeOwner["Cellular"] = hotxtMobile.Text;
                drHomeOwner["EMail"] = HotxtEmailWeb.Text;
                tblGCandHomeOwner.Rows.Add(drHomeOwner);


            }
            #endregion

            objProp_Customer.tblGCandHomeOwner = tblGCandHomeOwner;
            objProp_Customer.PWIP = chkProgressBilling.Checked;
            if (!string.IsNullOrEmpty(hdnUnrecognizedRevenue.Value))
                objProp_Customer.UnrecognizedRevenue = Convert.ToInt32(hdnUnrecognizedRevenue.Value);
            if (!string.IsNullOrEmpty(hdnUnrecognizedExpense.Value))
                objProp_Customer.UnrecognizedExpense = Convert.ToInt32(hdnUnrecognizedExpense.Value);
            if (!string.IsNullOrEmpty(hdnRetainageReceivable.Value))
                objProp_Customer.RetainageReceivable = Convert.ToInt32(hdnRetainageReceivable.Value);
            objProp_Customer.ArchitectName = txtArchitectName.Text;
            objProp_Customer.ArchitectAdress = txtArchitectAdress.Text;

            int jobid = objBL_Customer.AddProject(objProp_Customer,"");

            if (Request.QueryString["uid"] != null)
            {
                GetData();
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyProj", "noty({text: 'Project Updated Successfully! <BR/>Project# " + jobid.ToString() + "', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : false});", true);
            }
            else
            {
                //objgn.ResetFormControlValues(this);
                ////Response.Redirect(Page.Request.RawUrl, false);
                //Initialize();

                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyProj", "noty({text: 'Project Added Successfully! <BR/>Project# " + jobid.ToString() + "', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : false, modal : true}); setTimeout(function(){ window.location = 'addproject.aspx?uid=" + jobid.ToString() + "'; }, 3000);", true);
            }
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "updateparent", "if(window.opener && !window.opener.closed) { if(window.opener.document.getElementById('ctl00_ContentPlaceHolder1_lnkSearch')) window.opener.document.getElementById('ctl00_ContentPlaceHolder1_lnkSearch').click();}", true);

        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErrProj", "noty({text: '" + str + "',  type : 'error', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void lnkCloseTemplate_Click(object sender, EventArgs e)
    {
        Response.Redirect("project.aspx", false);
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
    protected void btnSelectCustomer_Click(object sender, EventArgs e)
    {
        FillLoc();
        FillGCCustomer();
    }

    protected void btnSelectLoc_Click(object sender, EventArgs e)
    {
        FillAddress();
        BindEquip();
    }

    protected void ddlTemplate_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (!ddlTemplate.SelectedValue.Equals("Select Template"))
        {
            EnableControl();
            objJob.ConnConfig = Session["config"].ToString();
            objJob.ID = Convert.ToInt32(ddlTemplate.SelectedValue);
            GetJobTemplate(objJob.ID);
            //ddlTemplate.SelectedValue = ddlTemplate.SelectedValue;
            objProp_Customer.ProjectJobID = Convert.ToInt32(ddlTemplate.SelectedValue);
            objProp_Customer.ConnConfig = Session["config"].ToString();
            DataSet ds = objBL_Customer.getJobTemplateByID(objProp_Customer);

            //GetTemplateData(ddlTemplate.SelectedValue); // Fill details into BOM tab
            bool IsExistsb = IsExistsBOM();
            bool IsExistsm = IsExistsMilestone();

            if (!IsExistsb)
            {
                if (ds.Tables[1].Rows.Count > 0)
                {
                    BindgvBOM(ds.Tables[1]);


                    //Session["gvBOM"] = ds.Tables[1];
                }
            }

            if (!IsExistsm)
            {
                if (ds.Tables[2].Rows.Count > 0)
                {
                    BindgvMilestones(ds.Tables[2]);

                }
            }

            #region Bind Custom fields
            // comment 8.16.16
            DataSet dsTemp = new DataSet();                   //commented by Mayuri on 28th july,16 incomplete custom field functionality
            dsTemp = objBL_Job.GetProjectTemplateCustomFields(objJob);
            if (dsTemp.Tables[0].Rows.Count > 0)
            {
                ViewState["IsCustomExist"] = true;
                CreateCustomTable();
                DisplayCustomByTab(dsTemp.Tables[0], dsTemp.Tables[1], objJob.ID);
            }

            #endregion
            DataSet dsJ = objBL_Job.GetJobTById(objJob);

            if (dsJ.Tables[0].Rows.Count > 0)
            {
                ddlJobType.SelectedValue = dsJ.Tables[0].Rows[0]["Type"].ToString();
            }
            else
            {
                ddlJobType.SelectedValue = "Select Type";
            }
            if (IsExistsb || IsExistsm)
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "WarnTemplate", "WarningTemplate();", true);
            }

        }
        else
        {
            DisableControl();
        }
    }

    #region Budget

    #region Tickets
    protected void ddlPagesOpenCall_SelectedIndexChanged(Object sender, EventArgs e)
    {
        GridDataItem gvrPager = (GridDataItem)gvTickets.NamingContainer;
        DropDownList ddlPages = (DropDownList)gvrPager.Cells[0].FindControl("ddlPages");
        gvTickets.CurrentPageIndex = ddlPages.SelectedIndex;
        GetOpenCalls(string.Empty);
        //FillGridPaged();
    }
    protected void gvOpenCalls_DataBound(object sender, EventArgs e)
    {
        //GridDataItem gvrPager = (GridDataItem)gvTickets.SelectedItems[0];
        //if (gvrPager == null) return;

        //// get your controls from the gridview
        //DropDownList ddlPages = (DropDownList)gvrPager.Cells[0].FindControl("ddlPages");
        //Label lblPageCount = (Label)gvrPager.Cells[0].FindControl("lblPageCount");

        //if (ddlPages != null)
        //{
        //    // populate pager
        //    for (int i = 0; i < gvTickets.PageCount; i++)
        //    {

        //        int intPageNumber = i + 1;
        //        ListItem lstItem = new ListItem(intPageNumber.ToString());

        //        if (i == gvTickets.CurrentPageIndex)
        //            lstItem.Selected = true;

        //        ddlPages.Items.Add(lstItem);
        //    }
        //}

        //// populate page count
        //if (lblPageCount != null)
        //    lblPageCount.Text = gvTickets.PageCount.ToString();
    }
    protected void PaginateOpencalls(object sender, CommandEventArgs e)
    {
        // get the current page selected
        int intCurIndex = gvTickets.CurrentPageIndex;

        switch (e.CommandArgument.ToString().ToLower())
        {
            case "first":
                gvTickets.CurrentPageIndex = 0;
                break;
            case "prev":
                gvTickets.CurrentPageIndex = intCurIndex - 1;
                break;
            case "next":
                gvTickets.CurrentPageIndex = intCurIndex + 1;
                break;
            case "last":
                gvTickets.CurrentPageIndex = gvTickets.PageCount;
                break;
        }

        // popultate the gridview control
        //GetOpenCalls(string.Empty);
        //  FillGridPaged();
    }
    protected void gvOpenCalls_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        PaginateOpencalls(sender, e);
    }
    public string ShowHoverText(object desc, object reason, object ticket)
    {
        string result = string.Empty;

        result = "<span style='font-size:18px;text-decoration: underline'><B>Ticket#</B> " + ticket + "</B></span><br/>";

        result += "<i>Reason</i>: " + Convert.ToString(reason).Replace("\n", "<br/>");

        if (!string.IsNullOrEmpty(Convert.ToString(desc)))
            result += "<br/><i>Resolution</i>: " + Convert.ToString(desc).Replace("\n", "<br/>");

        return result;
    }
    //protected void gvOpenCalls_RowCreated(object sender, GridViewRowEventArgs e)
    //{
    //    if (e.Row.RowType == DataControlRowType.DataRow)
    //    {
    //        AjaxControlToolkit.HoverMenuExtender ajxHover = (AjaxControlToolkit.HoverMenuExtender)e.Row.FindControl("hmeRes");
    //        e.Row.ID = e.Row.RowIndex.ToString();
    //        ajxHover.TargetControlID = e.Row.ID;
    //    }
    //}
    protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        //GetOpenCalls(string.Empty);
        BindTicketList(string.Empty, 1);
        string[] array = { "-2", "0", "5" };
        if (array.Contains(ddlStatus.SelectedValue))
            pnlTicketButtons.Visible = true;
        else
            pnlTicketButtons.Visible = false;
    }
    #endregion

    #region Invoice
    protected void ddlPagesInvoice_SelectedIndexChanged(Object sender, EventArgs e)
    {
        GridDataItem gvrPager = (GridDataItem)gvInvoice.NamingContainer;
        DropDownList ddlPages = (DropDownList)gvrPager.Cells[0].FindControl("ddlPages");
        gvInvoice.CurrentPageIndex = ddlPages.SelectedIndex;
        GetInvoices();
    }
    //protected void ddlArPages_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    GridDataItem gvrPager = gvArInvoice.BottomPagerRow;
    //    DropDownList ddlArPages = (DropDownList)gvrPager.Cells[0].FindControl("ddlArPages");
    //    gvArInvoice.PageIndex = ddlArPages.SelectedIndex;
    //    GetInvoices();
    //}
    protected void gvInvoice_DataBound(object sender, EventArgs e)
    {
        //GridDataItem gvrPager = (GridDataItem)gvInvoice.SelectedItems[0];

        //if (gvrPager == null) return;

        //// get your controls from the gridview
        //DropDownList ddlPages = (DropDownList)gvrPager.Cells[0].FindControl("ddlPages");
        //Label lblPageCount = (Label)gvrPager.Cells[0].FindControl("lblPageCount");

        //if (ddlPages != null)
        //{
        //    // populate pager
        //    for (int i = 0; i < gvInvoice.PageCount; i++)
        //    {

        //        int intPageNumber = i + 1;
        //        ListItem lstItem = new ListItem(intPageNumber.ToString());

        //        if (i == gvInvoice.CurrentPageIndex)
        //            lstItem.Selected = true;

        //        ddlPages.Items.Add(lstItem);
        //    }
        //}

        //// populate page count
        //if (lblPageCount != null)
        //    lblPageCount.Text = gvInvoice.PageCount.ToString();
    }
    protected void PaginateInvoice(object sender, CommandEventArgs e)
    {
        // get the current page selected
        int intCurIndex = gvInvoice.CurrentPageIndex;

        switch (e.CommandArgument.ToString().ToLower())
        {
            case "first":
                gvInvoice.CurrentPageIndex = 0;
                break;
            case "prev":
                gvInvoice.CurrentPageIndex = intCurIndex - 1;
                break;
            case "next":
                gvInvoice.CurrentPageIndex = intCurIndex + 1;
                break;
            case "last":
                gvInvoice.CurrentPageIndex = gvInvoice.PageCount;
                break;
        }

        // popultate the gridview control
        GetInvoices();
    }
    protected void gvInvoice_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        PaginateInvoice(sender, e);
    }

    #endregion

    protected void ddlInvoiceStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        GetInvoices();
    }

    #endregion

    #region Attributes
    protected void gvTeamItems_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.Equals("AddTeam"))
        {
            int rowIndex = gvTeamItems.Items.Count - 1;

            HiddenField hdnIndex = gvTeamItems.Items[rowIndex].Cells[0].FindControl("hdnIndex") as HiddenField;

            DataTable dt = GetTeamItems();
            if (dt.Rows.Count < 1)
            {
                for (int j = 0; j <= rowIndex; j++)
                {
                    DataRow dr = dt.NewRow();
                    dt.Rows.Add(dr);
                }
            }
            DataRow dr2 = dt.NewRow();
            dt.Rows.Add(dr2);

            gvTeamItems.DataSource = dt;
            gvTeamItems.DataBind();
        }
    }
    #endregion

    #region Finance
    protected void lnkUploadDoc_Click(object sender, EventArgs e)
    {
        try
        {
            string filename = string.Empty;
            string fullpath = string.Empty;
            string MIME = string.Empty;
            if (fuAttachPO.HasFile)
            {
                string savepathconfig = System.Web.Configuration.WebConfigurationManager.AppSettings["AttachmentPath"].Trim();
                string savepath = savepathconfig + @"\" + Session["dbname"] + @"\ld_" + Request.QueryString["uid"].ToString() + @"\";
                filename = fuAttachPO.FileName;
                filename = filename.Replace(",", "");
                fullpath = savepath + filename;
                MIME = System.IO.Path.GetExtension(fuAttachPO.PostedFile.FileName).Substring(1);

                if (File.Exists(fullpath))
                {
                    for (int i = 1; i < 100; i++)
                    {
                        string tmpFileName = string.Empty;
                        if (MIME != null)
                        {
                            tmpFileName = filename.Replace("." + MIME, "(" + i + ")." + MIME);
                        }
                        else
                        {
                            tmpFileName = filename + "(" + i + ")";
                        }
                        fullpath = savepath + tmpFileName;
                        if (!File.Exists(fullpath))
                        {
                            filename = tmpFileName;
                            fullpath = savepath + filename;
                            break;
                        }
                    }
                }

                if (!Directory.Exists(savepath))
                {
                    Directory.CreateDirectory(savepath);
                }

                fuAttachPO.SaveAs(fullpath);
            }

            objMapData.Screen = "Project";
            objMapData.TicketID = Convert.ToInt32(Request.QueryString["uid"].ToString());
            objMapData.TempId = "0";
            objMapData.FileName = filename;
            objMapData.DocTypeMIME = MIME;
            objMapData.FilePath = fullpath;
            objMapData.DocID = 0;
            objMapData.Mode = 0;
            objMapData.ConnConfig = Session["config"].ToString();
            objBL_MapData.AddFile(objMapData);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyUploadSuccess1", "noty({text: 'File uploaded successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            string extension = Path.GetExtension(FU_Project.FileName);
            if (extension == "")
            {
                ClientScript.RegisterStartupScript(Page.GetType(), "keyUploadextension", "noty({text: 'Invalid File!',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            }
            else
            {
                ClientScript.RegisterStartupScript(Page.GetType(), "keyUploadErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            }
        }
    }


    #endregion

    protected void gvArInvoice_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        //PaginateInvoice(sender, e);
    }
    protected void ddlJobType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (!ddlJobType.SelectedValue.Equals("Select Template"))
            {
                objJob.ConnConfig = Session["config"].ToString();

                objJob.ID = Convert.ToInt32(ddlTemplate.SelectedValue);
                objJob.Type = Convert.ToInt16(ddlJobType.SelectedValue);
                objJob.IsExist = objBL_Job.IsExistProjectTempByType(objJob);
                if (!objJob.IsExist.Equals(true))
                {
                    ddlTemplate.SelectedValue = "0";
                }
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    protected void gvMilestones_RowCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        if (e.CommandName.Equals("AddMilestone"))
        {
            DataTable dt = GetMilestoneGridItems();
            string maxvalue = dt.AsEnumerable().Max(row => row["Line"]).ToString();
            Int32 _line = Convert.ToInt32(maxvalue) + 1;

            for (int j = 0; j < 1; j++)
            {
                DataRow dr = dt.NewRow();
                dr["Line"] = _line;
                dt.Rows.Add(dr);
                _line = _line + 1;
            }

            BindgvMilestones(dt);

        }
    }
    protected void ibDeleteBom_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            //DataTable dt = GetBOMGridItems();
            foreach (GridDataItem gr in gvBOM.Items)
            {
                CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");
                if (chkSelect.Checked.Equals(true))
                {
                    Label lblLine = gr.FindControl("lblLine") as Label;

                    if (Request.QueryString["uid"] != null)
                    {
                        objJob.Job = Convert.ToInt32(Request.QueryString["uid"].ToString());
                        objJob.Phase = Convert.ToInt32(lblLine.Text);
                        bool IsExist = objBL_Job.IsExistExpJobItemByJob(objJob);
                        if (IsExist.Equals(true))
                        {
                            ScriptManager.RegisterStartupScript(this, Page.GetType(), "removebomLine", "noty({text: 'Selected job item is in use, it cannot be deleted!', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : false});", true);
                        }
                        else
                        {
                            //ScriptManager.RegisterStartupScript(this, Page.GetType(), "removebomLine", "removeLine('" + gvBOM.ClientID + "')", true);
                            //DataTable dtBOM = Session["gvBOM"] as DataTable;

                            DataTable dtBOM = GetBOMGridItems();
                            foreach (DataRow row in dtBOM.Rows)
                            {
                                if (Convert.ToString(row["Line"]) == lblLine.Text)
                                {
                                    dtBOM.Rows.Remove(row);
                                    break;
                                }
                            }
                            BindgvBOM(dtBOM);


                        }
                    }
                    else
                    {
                        //ScriptManager.RegisterStartupScript(this, Page.GetType(), "removebomLine", "removeLine('" + gvBOM.ClientID + "')", true);
                        //DataTable dtBOM = Session["gvBOM"] as DataTable;

                        DataTable dtBOM = GetBOMGridItems();
                        foreach (DataRow row in dtBOM.Rows)
                        {
                            if (Convert.ToString(row["Line"]) == lblLine.Text)
                            {
                                dtBOM.Rows.Remove(row);
                                break;
                            }
                        }
                        BindgvBOM(dtBOM);

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
    protected void ibDeleteMilestone_Click(object sender, ImageClickEventArgs e)
    {
        //DataTable dt = GetMilestoneGridItems();
        try
        {
            foreach (GridDataItem gr in gvMilestones.Items)
            {
                CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");
                if (chkSelect.Checked.Equals(true))
                {
                    Label lblLine = gr.FindControl("lblLine") as Label;

                    if (Request.QueryString["uid"] != null)
                    {
                        objJob.Job = Convert.ToInt32(Request.QueryString["uid"].ToString());
                        objJob.Phase = Convert.ToInt32(lblLine.Text);
                        bool IsExist = objBL_Job.IsExistRevJobItemByJob(objJob);
                        if (IsExist.Equals(true))
                        {
                            ScriptManager.RegisterStartupScript(this, Page.GetType(), "removebomLine", "noty({text: 'Selected job item is in use, it cannot be deleted!', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : false});", true);
                        }
                        else
                        {
                            //ScriptManager.RegisterStartupScript(this, Page.GetType(), "removebomLine", "removeLine('" + gvMilestones.ClientID + "')", true);

                            DataTable dtBOM = GetMilestoneGridItems();
                            foreach (DataRow row in dtBOM.Rows)
                            {
                                if (Convert.ToString(row["Line"]) == lblLine.Text)
                                {
                                    dtBOM.Rows.Remove(row);
                                    break;
                                }
                            }
                            BindgvMilestones(dtBOM);

                        }
                    }
                    else
                    {
                        //ScriptManager.RegisterStartupScript(this, Page.GetType(), "removebomLine", "removeLine('" + gvMilestones.ClientID + "')", true);

                        DataTable dtBOM = GetMilestoneGridItems();
                        foreach (DataRow row in dtBOM.Rows)
                        {
                            if (Convert.ToString(row["Line"]) == lblLine.Text)
                            {
                                dtBOM.Rows.Remove(row);
                                break;
                            }
                        }
                        BindgvMilestones(dtBOM);

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
    //protected void ddlItem_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        DropDownList ddlItem = (DropDownList)sender;
    //        GridViewRow row = (GridViewRow)ddlItem.NamingContainer;
    //        TextBox txtScope = (TextBox)row.FindControl("txtScope");
    //        DropDownList ddlBType = (DropDownList)row.FindControl("ddlBType");

    //        DataSet ds = new DataSet();
    //        if (ddlItem.SelectedValue != "0")
    //        {
    //            if (ddlBType.SelectedValue == "1" || ddlBType.SelectedValue == "2")       // select bom type Material
    //            {
    //                if (dtBomItem.Rows.Count.Equals(0) && ddlBType.SelectedValue == "1")
    //                {
    //                    objJob.ConnConfig = Session["config"].ToString();
    //                    ds = objBL_Job.GetInventoryItem(objJob);
    //                    dtBomItem = ds.Tables[0];
    //                }
    //                else if (dtBomItem.Rows.Count.Equals(0) && ddlBType.SelectedValue == "2")
    //                {
    //                    objWage.ConnConfig = Session["config"].ToString();
    //                    ds = objBL_User.GetAllWage(objWage);
    //                    dtBomItem = ds.Tables[0];
    //                }
    //                if (dtBomItem.Rows.Count > 0)
    //                {
    //                    DataRow dr = dtBomItem.Select("Value =" + ddlItem.SelectedValue).FirstOrDefault();
    //                    if (dr != null)
    //                    {
    //                        txtScope.Text = dr["fDesc"].ToString();
    //                    }
    //                    else
    //                    {
    //                        txtScope.Text = "";
    //                    }
    //                }
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
    //        ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
    //    }
    //}
    #endregion

    #region Custom Functions
    private void GetData()
    {
        try
        {
            objProp_Customer.ConnConfig = Session["config"].ToString();
            objProp_Customer.ProjectJobID = Convert.ToInt32(Request.QueryString["uid"].ToString());
            objProp_Customer.Type = string.Empty;
            DataSet ds = objBL_Customer.getJobProjectByJobID(objProp_Customer);
            if (ds.Tables[0].Rows.Count > 0)
            {
                //ddlEstimates.SelectedValue = ds.Tables[0].Rows[0]["estimateid"].ToString();
                //txtName.Text = ds.Tables[0].Rows[0]["name"].ToString();
                lblProjectNo.Text = "# " + ds.Tables[0].Rows[0]["ID"].ToString();
                txtREPdesc.Text = ds.Tables[0].Rows[0]["fdesc"].ToString();
                txtREPremarks.Text = ds.Tables[0].Rows[0]["remarks"].ToString();
                if (ds.Tables[0].Rows[0]["template"].ToString() != string.Empty)
                    ddlTemplate.SelectedValue = ds.Tables[0].Rows[0]["template"].ToString();
                //uc_LocationSearch1._txtLocation.Text = ds.Tables[0].Rows[0]["locname"].ToString();
                //uc_LocationSearch1._hdnLocId.Value = ds.Tables[0].Rows[0]["loc"].ToString();
                txtLocation.Text = ds.Tables[0].Rows[0]["locname"].ToString();
                hdnLocID.Value = ds.Tables[0].Rows[0]["loc"].ToString();
                //Set Hyperlink  For Loc / Customer
                if (hdnLocID.Value != "0")
                {
                    lnkLocationID.NavigateUrl = "addlocation.aspx?uid=" + hdnLocID.Value;
                    GetDataEquip();
                }
                else
                    lnkLocationID.NavigateUrl = "";
                hdnCustID.Value = ds.Tables[0].Rows[0]["owner"].ToString();
                if (hdnCustID.Value != "0")
                    lnkCustomerID.NavigateUrl = "addcustomer.aspx?uid=" + hdnCustID.Value;
                else
                    lnkCustomerID.NavigateUrl = "";
                txtCustomer.Text = ds.Tables[0].Rows[0]["customerName"].ToString();
                //ddlTemplate.SelectedValue = ds.Tables[0].Rows[0]["template"].ToString();

                ddlJobType.SelectedValue = ds.Tables[0].Rows[0]["Type"].ToString();
                ddlJobStatus.SelectedValue = ds.Tables[0].Rows[0]["Status"].ToString();

                ddlCodeCat.SelectedValue = ds.Tables[0].Rows[0]["taskcategory"].ToString();
                SelectTaskCategory();
                if (Convert.ToBoolean(ds.Tables[0].Rows[0]["Certified"]))
                {
                    chkCertifiedJob.Checked = true;
                }
                txtPO.Text = ds.Tables[0].Rows[0]["PO"].ToString();
                txtSalesOrder.Text = ds.Tables[0].Rows[0]["SO"].ToString();
                txtCustom1.Text = ds.Tables[0].Rows[0]["Custom21"].ToString();
                txtCustom2.Text = ds.Tables[0].Rows[0]["Custom22"].ToString();
                txtCustom3.Text = ds.Tables[0].Rows[0]["Custom23"].ToString();
                txtCustom4.Text = ds.Tables[0].Rows[0]["Custom24"].ToString();
                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Custom25"].ToString()))
                {
                    txtCustom5.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["Custom25"]).ToString("MM/dd/yyyy");
                }

                FillAddress();
                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["ProjCreationDate"].ToString()))
                {
                    txtProjCreationDate.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["ProjCreationDate"]).ToString("MM/dd/yyyy");
                }

                GetJobtaskCategory();



                #region finance-general


                //if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["CType"].ToString()))
                if (!Convert.ToInt32(ds.Tables[0].Rows[0]["Template"]).Equals(0))
                {
                    EnableControl();
                    //if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["jobTempCtype"].ToString()))
                    //{
                    //    ddlContractType1.SelectedValue = ds.Tables[0].Rows[0]["jobTempCtype"].ToString();
                    //}
                    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["ctype"].ToString()))
                    {
                        ddlContractType1.SelectedValue = ds.Tables[0].Rows[0]["ctype"].ToString();
                    }
                    hdnInvServiceID.Value = ds.Tables[0].Rows[0]["InvServ"].ToString();
                    txtInvService.Text = ds.Tables[0].Rows[0]["InvServiceName"].ToString();
                    hdnPrevilWageID.Value = ds.Tables[0].Rows[0]["Wage"].ToString();
                    txtPrevilWage.Text = ds.Tables[0].Rows[0]["WageName"].ToString();
                    uc_InterestGL._txtGLAcct.Text = ds.Tables[0].Rows[0]["GLName"].ToString();
                    uc_InterestGL._hdnAcctID.Value = ds.Tables[0].Rows[0]["GLInt"].ToString();
                    uc_InvExpGL._txtGLAcct.Text = ds.Tables[0].Rows[0]["InvExpName"].ToString();
                    uc_InvExpGL._hdnAcctID.Value = ds.Tables[0].Rows[0]["InvExp"].ToString();
                    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Post"].ToString()))
                    {
                        ddlPostingMethod.SelectedValue = ds.Tables[0].Rows[0]["Post"].ToString();
                    }

                    if (ds.Tables[0].Rows[0]["Charge"].ToString().Equals("1"))
                    {
                        chkChargeable.Checked = true;
                    }
                    if (ds.Tables[0].Rows[0]["fInt"].ToString().Equals("1"))
                    {
                        chkChargeInt.Checked = true;
                    }
                    if (ds.Tables[0].Rows[0]["JobClose"].ToString().Equals("1"))
                    {
                        chkInvoicing.Checked = true;
                    }

                }
                else
                {
                    DisableControl();
                }

                #endregion

                #region Notes
                txtSpecialInstructions.Text = ds.Tables[0].Rows[0]["SRemarks"] != DBNull.Value ? Convert.ToString(ds.Tables[0].Rows[0]["SRemarks"]) : string.Empty;
                chkspnotes.Checked = ds.Tables[0].Rows[0]["SPHandle"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[0]["SPHandle"]) : false;
                txtRenew.Text = ds.Tables[0].Rows[0]["RenewalNotes"] != DBNull.Value ? Convert.ToString(ds.Tables[0].Rows[0]["RenewalNotes"]) : string.Empty;
                chkRenew.Checked = ds.Tables[0].Rows[0]["IsRenewalNotes"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[0]["IsRenewalNotes"]) : false;
                #endregion

                //ddlEstimates_SelectedIndexChanged(sender, e);
                if (ds.Tables[0].Rows[0]["estimateid"].ToString() != "")
                {
                    trEstimate.Visible = true;
                    lnkEstimate.Text = ds.Tables[0].Rows[0]["estimateid"].ToString() + " - " + ds.Tables[0].Rows[0]["estimate"].ToString();
                    lnkEstimate.NavigateUrl = "addestimate.aspx?uid=" + ds.Tables[0].Rows[0]["estimateid"].ToString();
                }

                if (ds.Tables[2].Rows.Count > 0)
                {
                    BindgvBOM(ds.Tables[2]);


                    //Session["gvBOM"] = ds.Tables[2];
                }
                if (ds.Tables[3].Rows.Count > 0)
                {
                    gvTeamItems.DataSource = ds.Tables[3];
                    gvTeamItems.DataBind();
                }
                if (ds.Tables[4].Rows.Count > 0)
                {
                    BindgvMilestones(ds.Tables[4]);

                }



                BindTicketList(string.Empty, 1);
                if (Convert.ToBoolean(ViewState["tasks"]) == true)
                {
                    DataTable dtopencall = GetOpenCalls(string.Empty);
                    rptTicketTask.DataSource = dtopencall;
                    rptTicketTask.DataBind();
                }
                //GetOpenCalls(string.Empty); 
                GetInvoices();
                GetAPInvoices();
                if (ds.Tables[0].Rows[0]["template"].ToString() != string.Empty)
                {
                    objJob.ID = Convert.ToInt32(ds.Tables[0].Rows[0]["template"].ToString());
                    objJob.Job = Convert.ToInt32(Request.QueryString["uid"].ToString());
                    DataSet dsCustom = objBL_Job.GetProjectTemplateCustomFields(objJob);
                    if (dsCustom.Tables[0].Rows.Count > 0)
                    {
                        ViewState["IsCustomExist"] = true;
                        CreateCustomTable();
                        DisplayCustomByTab(dsCustom.Tables[0], dsCustom.Tables[1], objJob.ID);
                    }
                }

                GetJobCost();

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

                txtBillRate.Text = string.Format("{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["BillRate"].ToString()));
                txtOt.Text = string.Format("{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["RateOT"].ToString()));
                txtNt.Text = string.Format("{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["RateNT"].ToString()));
                txtDt.Text = string.Format("{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["RateDT"].ToString()));
                txtMileage.Text = string.Format("{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["RateMileage"].ToString()));
                txtTravel.Text = string.Format("{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["RateTravel"].ToString()));

                DataSet ds2 = objBL_Job.GetBudgetSummaryGridDataByJob(objJob);
                if (ds2.Tables[0].Rows.Count > 0)
                {
                    gvBudgetGrid.DataSource = ds2.Tables[0];
                    gvBudgetGrid.DataBind();
                    GridFooterItem footeritem = (GridFooterItem)gvBudgetGrid.MasterTableView.GetItems(GridItemType.Footer)[0];
                    if (footeritem != null)
                    {
                        Label lblRevFooter = footeritem.FindControl("lblRevFooter") as Label;
                        Label lblLaborExpFooter = footeritem.FindControl("lblLaborExpFooter") as Label;
                        Label lblMaterialExpFooter = footeritem.FindControl("lblMatExpFooter") as Label;
                        Label lblOtherExpFooter = footeritem.FindControl("lblOtherExpFooter") as Label;
                        Label lblTotalExpFooter = footeritem.FindControl("lblCostFooter") as Label;
                        Label lblProfitAmtFooter = footeritem.FindControl("lblProfitFooter") as Label;
                        Label lblNetProfitFooter = footeritem.FindControl("lblRatioFooter") as Label;
                        Label lblHoursFooter = footeritem.FindControl("lblHoursFooter") as Label;
                        Label lblTotalOnOrderFooter = footeritem.FindControl("lblOnOrderFooter") as Label;

                        DataRow dr1 = ds2.Tables[0].Rows[0];
                        DataRow dr2 = ds2.Tables[0].Rows[1];

                        lblRevFooter.Text = string.Format("{0:c}", Convert.ToDouble(dr1["Rev"]) - Convert.ToDouble(dr2["Rev"]));
                        lblLaborExpFooter.Text = string.Format("{0:c}", Convert.ToDouble(dr1["Labor"]) - Convert.ToDouble(dr2["Labor"]));
                        lblMaterialExpFooter.Text = string.Format("{0:c}", Convert.ToDouble(dr1["Mat"]) - Convert.ToDouble(dr2["Mat"]));
                        lblOtherExpFooter.Text = string.Format("{0:c}", Convert.ToDouble(dr1["OtherExp"]) - Convert.ToDouble(dr2["OtherExp"]));
                        lblTotalExpFooter.Text = string.Format("{0:c}", Convert.ToDouble(dr1["Cost"]) - Convert.ToDouble(dr2["Cost"]));
                        lblProfitAmtFooter.Text = string.Format("{0:c}", Convert.ToDouble(dr1["Profit"]) - Convert.ToDouble(dr2["Profit"]));
                        lblNetProfitFooter.Text = string.Format("{0:n}", Convert.ToDouble(dr1["Ratio"]) - Convert.ToDouble(dr2["Ratio"]));
                        lblHoursFooter.Text = string.Format("{0:n}", Convert.ToDouble(dr1["hour"]) - Convert.ToDouble(dr2["hour"]));
                        lblTotalOnOrderFooter.Text = string.Format("{0:c}", Convert.ToDouble(dr1["OnOrder"]) - Convert.ToDouble(dr2["OnOrder"]));
                    }

                }
                // Fill GC and Ho info 
                if (!string.IsNullOrEmpty(hdnLocID.Value))
                    GetGC_HOInfo(hdnLocID.Value);
                if (!string.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[0]["PWIP"])))
                    chkProgressBilling.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["PWIP"].ToString());
                else
                    chkProgressBilling.Checked = false;
                if (chkProgressBilling.Checked)
                {
                    liBilling_WIP.Style.Add("visibility", "visible");
                    tbpnlWIP.Style.Add("visibility", "visible");

                    liBilling_ProgressBillings.Style.Add("visibility", "visible");
                    tbpnlProgressBilling.Style.Add("visibility", "visible");

                    //liBilling_WIP.Visible = true;
                    //tbpnlWIP.Visible = true;
                    //liBilling_ProgressBillings.Visible = true;
                    //tbpnlProgressBilling.Visible = true;
                    //tbpnlWIP.Attributes.Add("style", "display:block");
                    //tbpnlProgressBilling.Attributes.Add("style", "display:block");
                    chkProgressBilling.Enabled = false;
                }
                else
                {
                    liBilling_WIP.Attributes.Remove("visibility");
                    tbpnlWIP.Attributes.Remove("visibility");
                    liBilling_ProgressBillings.Attributes.Remove("visibility");
                    tbpnlProgressBilling.Attributes.Remove("visibility");

                    //liBilling_WIP.Visible = false;
                    //tbpnlWIP.Visible = false;
                    //liBilling_ProgressBillings.Visible = false;
                    //tbpnlProgressBilling.Visible = false;
                    //tbpnlWIP.Attributes.Add("style", "display:none");
                    //tbpnlProgressBilling.Attributes.Add("style", "display:none");
                }
                hdnUnrecognizedRevenue.Value = ds.Tables[0].Rows[0]["UnrecognizedRevenue"].ToString();
                if (!String.IsNullOrEmpty(hdnUnrecognizedRevenue.Value))
                    AssignBillingCodeInGrid(hdnUnrecognizedRevenue.Value);
                txtUnrecognizedRevenue.Text = ds.Tables[0].Rows[0]["UnrecognizedRevenueName"].ToString();
                hdnUnrecognizedExpense.Value = ds.Tables[0].Rows[0]["UnrecognizedExpense"].ToString();
                txtUnrecognizedExpense.Text = ds.Tables[0].Rows[0]["UnrecognizedExpenseName"].ToString();
                hdnRetainageReceivable.Value = ds.Tables[0].Rows[0]["RetainageReceivable"].ToString();
                txtRetainageReceivable.Text = ds.Tables[0].Rows[0]["RetainageReceivableName"].ToString();
                txtArchitectName.Text = ds.Tables[0].Rows[0]["ArchitectName"].ToString();
                txtArchitectAdress.Text = ds.Tables[0].Rows[0]["ArchitectAdress"].ToString();
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    private void Permission()
    {
        HtmlGenericControl li = (HtmlGenericControl)Page.Master.FindControl("ProjectMgr");
        //li.Style.Add("background", "url(images/active_menu_bg.png) repeat-x");
        li.Attributes.Add("class", "start active open");

        HyperLink a = (HyperLink)Page.Master.FindControl("ProjectLink");
        //a.Style.Add("color", "#2382b2");

        HyperLink lnkUsersSmenu = (HyperLink)Page.Master.FindControl("lnkProject");
        //lnkUsersSmenu.Style.Add("color", "#FF7A0A");
        lnkUsersSmenu.Style.Add("color", "#316b9d");
        lnkUsersSmenu.Style.Add("font-weight", "normal");
        lnkUsersSmenu.Style.Add("background-color", "#e5e5e5");
        //AjaxControlToolkit.HoverMenuExtender hm = (AjaxControlToolkit.HoverMenuExtender)Page.Master.Master.FindControl("HoverMenuExtenderSales");
        //hm.Enabled = false;
        //HtmlGenericControl ul = (HtmlGenericControl)Page.Master.FindControl("SalesMgrSub");
        ////ul.Attributes.Remove("class");
        //ul.Style.Add("display", "block");

        if (Session["type"].ToString() != "am")
        {
            DataTable ds = new DataTable();
            ds = (DataTable)Session["userinfo"];

            /// Ticket ///////////////////------->

            string ticketPermission = ds.Rows[0]["TicketPermission"] == DBNull.Value ? "YYNYYY" : ds.Rows[0]["TicketPermission"].ToString();
            hdnAddeTicket.Value = ticketPermission.Length < 1 ? "Y" : ticketPermission.Substring(0, 1);
            hdnEditeTicket.Value = ticketPermission.Length < 2 ? "Y" : ticketPermission.Substring(1, 1);

            hdnviewTicket.Value = ticketPermission.Length < 4 ? "Y" : ticketPermission.Substring(3, 1);

            //Contact
            string ContactPermission = ds.Rows[0]["ContactPermission"] == DBNull.Value ? "YYYY" : ds.Rows[0]["ContactPermission"].ToString();
            hdnAddeContact.Value = ContactPermission.Length < 1 ? "Y" : ContactPermission.Substring(0, 1);
            hdnEditeContact.Value = ContactPermission.Length < 2 ? "Y" : ContactPermission.Substring(1, 1);
            hdnDeleteContact.Value = ContactPermission.Length < 3 ? "Y" : ContactPermission.Substring(2, 1);
            hdnViewContact.Value = ContactPermission.Length < 4 ? "Y" : ContactPermission.Substring(3, 1);

            if (hdnAddeContact.Value == "N")
            {
                imgAddContact.Enabled = false;
                btnEditcontact.Enabled = false;
            }

            gvContacts.Visible = panel1.Visible = hdnViewContact.Value == "N" ? false : true;

            //Document
            string DocumentPermission = ds.Rows[0]["DocumentPermission"] == DBNull.Value ? "YYYY" : ds.Rows[0]["DocumentPermission"].ToString();
            hdnAddeDocument.Value = DocumentPermission.Length < 1 ? "Y" : DocumentPermission.Substring(0, 1);
            hdnEditeDocument.Value = DocumentPermission.Length < 2 ? "Y" : DocumentPermission.Substring(1, 1);
            hdnDeleteDocument.Value = DocumentPermission.Length < 3 ? "Y" : DocumentPermission.Substring(2, 1);
            hdnViewDocument.Value = DocumentPermission.Length < 4 ? "Y" : DocumentPermission.Substring(3, 1);

            if (hdnAddeDocument.Value == "N")
            {
                lnkUploadDoc.Enabled = false;
            }

            pnlDocPermission.Visible = hdnViewDocument.Value == "N" ? false : true;
        }

    }
    private void Initialize()
    {
        CreateBOMTable();
        CreateMilestoneTable();
        CreateTeamTable();
        BindEquip();
    }

    #region Milestones
    private void CreateMilestoneTable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("JobT", typeof(int));
        dt.Columns.Add("Job", typeof(int));
        dt.Columns.Add("ID", typeof(int));
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

        DataRow dr = dt.NewRow();
        dr["Line"] = 1;
        dt.Rows.Add(dr);

        DataRow dr1 = dt.NewRow();
        dr1["Line"] = 2;
        dt.Rows.Add(dr1);

        ViewState["MProjectTemplate"] = dt;
        BindgvMilestones(dt);

    }
    private bool IsExistsMilestone()
    {
        string strItems = hdnMilestone.Value.Trim();
        try
        {
            if (strItems != string.Empty)
            {
                JavaScriptSerializer sr = new JavaScriptSerializer();
                List<Dictionary<object, object>> objEstimateItemData = new List<Dictionary<object, object>>();
                objEstimateItemData = sr.Deserialize<List<Dictionary<object, object>>>(strItems);
                int i = 0;
                foreach (Dictionary<object, object> dict in objEstimateItemData)
                {
                    if (dict["txtScope"].ToString().Trim() == string.Empty)
                    {
                        return false;
                    }
                    i++;
                    if (dict["hdnID"].ToString().Trim() != string.Empty)
                    {
                        if (Convert.ToInt16(dict["hdnID"].ToString()) > 0)
                        {
                            return true;
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
        return false;
    }
    private DataTable GetMilestoneItems()
    {
        DataTable dt = new DataTable();

        dt.Columns.Add("JobT", typeof(int));
        dt.Columns.Add("Job", typeof(int));
        dt.Columns.Add("ID", typeof(int));
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
        try
        {
            string strItems = hdnMilestone.Value.Trim();

            if (strItems != string.Empty)
            {
                JavaScriptSerializer sr = new JavaScriptSerializer();
                List<Dictionary<object, object>> objEstimateItemData = new List<Dictionary<object, object>>();
                objEstimateItemData = sr.Deserialize<List<Dictionary<object, object>>>(strItems);
                int i = 0;

                foreach (Dictionary<object, object> dict in objEstimateItemData)
                {
                    if (dict["txtScope"].ToString().Trim() == string.Empty)
                    {
                        return dt;
                    }
                    i++;
                    DataRow dr = dt.NewRow();
                    if (dict["hdnID"].ToString().Trim() != string.Empty)
                    {
                        dr["ID"] = Convert.ToInt32(dict["hdnID"].ToString());
                    }
                    else
                    {
                        dr["ID"] = 0;
                    }
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
                    if (dict["txtAmount"].ToString() != string.Empty)
                    {
                        dr["Amount"] = Convert.ToDouble(dict["txtAmount"]);
                    }
                    //dr["LeadTime"] = dict["txtLeadTime"].ToString();
                    if (!string.IsNullOrEmpty(dict["hdnType"].ToString()))
                    {
                        dr["Type"] = dict["hdnType"].ToString();
                        dr["Department"] = dict["txtSType"].ToString();
                    }
                    //if (!string.IsNullOrEmpty(dict["txtActAcquiDate"].ToString()))
                    //{
                    //    dr["ActAcquDate"] = dict["txtActAcquiDate"].ToString();
                    //}
                    //dr["Comments"] = dict["txtComments"].ToString();

                    dt.Rows.Add(dr);
                }
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
        return dt;
    }
    private DataTable GetMilestoneGridItems()       //get all items in milestone grid
    {
        DataTable dt = new DataTable();

        dt.Columns.Add("JobT", typeof(int));
        dt.Columns.Add("Job", typeof(int));
        dt.Columns.Add("ID", typeof(int));
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
        try
        {
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
                    if (dict["hdnID"].ToString().Trim() != string.Empty)
                    {
                        dr["ID"] = Convert.ToInt32(dict["hdnID"].ToString());
                    }
                    else
                    {
                        dr["ID"] = 0;
                    }
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
                    if (dict["txtAmount"].ToString() != string.Empty)
                    {
                        dr["Amount"] = Convert.ToDouble(dict["txtAmount"]);
                    }
                    //dr["LeadTime"] = dict["txtLeadTime"].ToString();
                    if (!string.IsNullOrEmpty(dict["hdnType"].ToString()))
                    {
                        dr["Type"] = dict["hdnType"].ToString();
                        dr["Department"] = dict["txtSType"].ToString();
                    }
                    //if (!string.IsNullOrEmpty(dict["txtActAcquiDate"].ToString()))
                    //{
                    //    dr["ActAcquDate"] = dict["txtActAcquiDate"].ToString();
                    //}
                    //dr["Comments"] = dict["txtComments"].ToString();

                    dt.Rows.Add(dr);
                }
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
        return dt;
    }

    #endregion

    #region BOM
    private void CreateBOMTable()
    {
        try
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
            DataRow dr = dt.NewRow();
            dr["JobTItemID"] = 0;
            dr["Line"] = 1;
            dt.Rows.Add(dr);

            DataRow dr1 = dt.NewRow();
            dr1["JobTItemID"] = 0;
            dr1["Line"] = 2;
            dt.Rows.Add(dr1);

            //ViewState["ProjectTemplate"] = dt;
            BindgvBOM(dt);


            //Session["gvBOM"] = dt;
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    private DataTable GetBomItems()
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

        string strItems = hdnItemJSON.Value.Trim();
        double budgetExt = 0;
        double _qtyReq = 0;
        double labExt = 0;
        try
        {
            if (strItems != string.Empty)
            {
                JavaScriptSerializer sr = new JavaScriptSerializer();
                List<Dictionary<object, object>> objEstimateItemData = new List<Dictionary<object, object>>();
                objEstimateItemData = sr.Deserialize<List<Dictionary<object, object>>>(strItems);
                int i = 0;
                foreach (Dictionary<object, object> dict in objEstimateItemData)
                {
                    _qtyReq = 0;
                    budgetExt = 0;
                    labExt = 0;
                    if (dict["txtScope"].ToString().Trim() == string.Empty)
                    {
                        return dt;
                    }
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
                    dr["fDesc"] = dict["txtScope"].ToString().Trim();
                    dr["Code"] = dict["txtCode"].ToString().Trim();
                    //dr["jtype"] = Convert.ToInt16(dict["ddlType"]);

                    if (dict["ddlBType"].ToString() != "Select Type" || string.IsNullOrEmpty(dict["ddlBType"].ToString()))
                    {
                        dr["BType"] = Convert.ToInt32(dict["ddlBType"]);
                    }
                    if (!Convert.ToInt32(dict["ddlLabItem"]).Equals(0))
                    {
                        dr["LabItem"] = Convert.ToInt32(dict["ddlLabItem"]);
                    }
                    //if (!Convert.ToInt32(dict["ddlMatItem"]).Equals(0))
                    //{
                    //    dr["MatItem"] = Convert.ToInt32(dict["ddlMatItem"]);
                    //}
                    if (!Convert.ToInt32(dict["hdnMatItem"]).Equals(0))
                    {
                        dr["MatItem"] = Convert.ToInt32(dict["hdnMatItem"]);
                    }
                    if (dict["txtMatItem"].ToString().Trim() != string.Empty)
                    {
                        dr["MatDesc"] = dict["txtMatItem"].ToString().Trim();
                    }
                    if (dict["txtQtyReq"].ToString().Trim() != string.Empty)
                    {
                        dr["QtyReq"] = Convert.ToDouble(dict["txtQtyReq"]);
                    }
                    else
                        dr["QtyReq"] = 0;
                    if (dict["txtUM"].ToString().Trim() != string.Empty)
                    {
                        dr["UM"] = dict["txtUM"].ToString().Trim();
                    }
                    if (dict["txtBudgetUnit"].ToString().Trim() != string.Empty)
                    {
                        dr["BudgetUnit"] = Convert.ToDouble(dict["txtBudgetUnit"]);
                    }
                    if (dict["txtBudgetUnit"].ToString().Trim() != string.Empty)
                    {
                        if (_qtyReq.Equals(0))
                        {
                            _qtyReq = Convert.ToDouble(dict["txtQtyReq"].ToString().Trim());
                        }
                        budgetExt = _qtyReq * Convert.ToDouble(dict["txtBudgetUnit"].ToString());
                        dr["BudgetExt"] = budgetExt;
                    }
                    if (!Convert.ToInt32(dict["ddlLabItem"]).Equals(0))
                    {
                        dr["LabItem"] = Convert.ToInt32(dict["ddlLabItem"]);
                    }
                    //if (!Convert.ToInt32(dict["ddlMatItem"]).Equals(0))
                    //{
                    //    dr["MatItem"] = Convert.ToInt32(dict["ddlMatItem"]);
                    //}
                    if (!Convert.ToInt32(dict["hdnMatItem"]).Equals(0))
                    {
                        dr["MatItem"] = Convert.ToInt32(dict["hdnMatItem"]);
                    }
                    if (dict["txtMatMod"].ToString().Trim() != string.Empty)
                    {
                        dr["MatMod"] = Convert.ToDouble(dict["txtMatMod"]);
                    }
                    if (dict["txtLabMod"].ToString().Trim() != string.Empty)
                    {
                        dr["LabMod"] = Convert.ToDouble(dict["txtLabMod"]);
                    }
                    if (dict["hdnVendorId"].ToString().Trim() != string.Empty)
                    {
                        dr["VendorId"] = Convert.ToInt32(dict["hdnVendorId"]);
                    }
                    if (dict["txtVendor"].ToString().Trim() != string.Empty)
                    {
                        dr["Vendor"] = dict["txtVendor"].ToString();
                    }
                    if (dict["txtHours"].ToString().Trim() != string.Empty)
                    {
                        dr["LabHours"] = Convert.ToDouble(dict["txtHours"]);
                    }
                    if (dict["txtLabRate"].ToString().Trim() != string.Empty)
                    {
                        dr["LabRate"] = Convert.ToDouble(dict["txtLabRate"]);
                        labExt = Convert.ToDouble(dict["txtLabRate"]);
                        if (dict["txtHours"].ToString().Trim() != string.Empty)
                        {
                            labExt = labExt * Convert.ToDouble(dict["txtHours"]);
                        }
                        dr["LabExt"] = labExt;
                    }
                    dr["TotalExt"] = labExt + budgetExt;
                    if (dict["txtSDate"].ToString().Trim() != string.Empty)
                    {
                        dr["SDate"] = Convert.ToDateTime(dict["txtSDate"]);
                    }
                    dt.Rows.Add(dr);
                }
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
        return dt;
    }
    private bool IsExistsBOM()
    {
        string strItems = hdnItemJSON.Value.Trim();
        try
        {
            if (strItems != string.Empty)
            {
                JavaScriptSerializer sr = new JavaScriptSerializer();
                List<Dictionary<object, object>> objEstimateItemData = new List<Dictionary<object, object>>();
                objEstimateItemData = sr.Deserialize<List<Dictionary<object, object>>>(strItems);
                int i = 0;
                foreach (Dictionary<object, object> dict in objEstimateItemData)
                {
                    if (dict["txtScope"].ToString().Trim() == string.Empty)
                    {
                        return false;
                    }
                    i++;
                    if (dict["hdnID"].ToString().Trim() != string.Empty)
                    {
                        if (Convert.ToInt16(dict["hdnID"].ToString()) > 0)
                        {
                            return true;
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
        return false;
    }
    private DataTable GetBOMGridItems()
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

        string strItems = hdnItemJSON.Value.Trim();
        double budgetExt = 0;
        double _qtyReq = 0;
        double labExt = 0;
        try
        {
            if (strItems != string.Empty)
            {

                JavaScriptSerializer sr = new JavaScriptSerializer();
                List<Dictionary<object, object>> objEstimateItemData = new List<Dictionary<object, object>>();
                objEstimateItemData = sr.Deserialize<List<Dictionary<object, object>>>(strItems);
                int i = 0;
                foreach (Dictionary<object, object> dict in objEstimateItemData)
                {
                    _qtyReq = 0;
                    budgetExt = 0;
                    labExt = 0;
                    if (dict["hdnLine"].ToString().Trim() == string.Empty)
                    {
                        return dt;
                    }
                    i++;
                    DataRow dr = dt.NewRow();
                    if (dict["hdnID"].ToString().Trim() != string.Empty)
                    {
                        dr["JobTItemID"] = Convert.ToInt32(dict["hdnID"].ToString());
                    }
                    else
                    {
                        dr["JobTItemID"] = 0;
                    }

                    if (dict["hdnLine"].ToString().Trim() != string.Empty)
                    {
                        dr["Line"] = Convert.ToInt32(dict["hdnLine"].ToString());
                    }
                    dr["fDesc"] = dict["txtScope"].ToString().Trim();
                    dr["Code"] = dict["txtCode"].ToString().Trim();

                    if (dict["ddlBType"].ToString() != "Select Type" || string.IsNullOrEmpty(dict["ddlBType"].ToString()))
                    {
                        dr["BType"] = Convert.ToInt32(dict["ddlBType"]);
                    }

                    if (dict["txtQtyReq"].ToString().Trim() != string.Empty)
                    {
                        dr["QtyReq"] = Convert.ToDouble(dict["txtQtyReq"]);
                    }
                    else
                        dr["QtyReq"] = 0;
                    if (dict["txtUM"].ToString().Trim() != string.Empty)
                    {
                        dr["UM"] = dict["txtUM"].ToString().Trim();
                    }
                    if (dict["txtBudgetUnit"].ToString().Trim() != string.Empty)
                    {
                        dr["BudgetUnit"] = Convert.ToDouble(dict["txtBudgetUnit"]);
                    }
                    if (dict["txtBudgetUnit"].ToString().Trim() != string.Empty)
                    {
                        if (_qtyReq.Equals(0))
                        {
                            _qtyReq = Convert.ToDouble(dict["txtQtyReq"].ToString().Trim());
                        }
                        budgetExt = _qtyReq * Convert.ToDouble(dict["txtBudgetUnit"].ToString());
                        dr["BudgetExt"] = budgetExt;
                    }
                    if (!Convert.ToInt32(dict["ddlLabItem"]).Equals(0))
                    {
                        dr["LabItem"] = Convert.ToInt32(dict["ddlLabItem"]);
                    }
                    //if (!Convert.ToInt32(dict["ddlMatItem"]).Equals(0))
                    //{
                    //    dr["MatItem"] = Convert.ToInt32(dict["ddlMatItem"]);
                    //}
                    if (!Convert.ToInt32(dict["hdnMatItem"]).Equals(0))
                    {
                        dr["MatItem"] = Convert.ToInt32(dict["hdnMatItem"]);
                    }
                    if (dict["txtMatItem"].ToString().Trim() != string.Empty)
                    {
                        dr["MatDesc"] = dict["txtMatItem"].ToString().Trim();
                    }
                    if (dict["txtMatMod"].ToString().Trim() != string.Empty)
                    {
                        dr["MatMod"] = Convert.ToDouble(dict["txtMatMod"]);
                    }
                    if (dict["txtLabMod"].ToString().Trim() != string.Empty)
                    {
                        dr["LabMod"] = Convert.ToDouble(dict["txtLabMod"]);
                    }
                    if (dict["hdnVendorId"].ToString().Trim() != string.Empty)
                    {
                        dr["VendorId"] = Convert.ToInt32(dict["hdnVendorId"]);
                    }
                    if (dict["txtVendor"].ToString().Trim() != string.Empty)
                    {
                        dr["Vendor"] = dict["txtVendor"].ToString();
                    }
                    if (dict["txtHours"].ToString().Trim() != string.Empty)
                    {
                        dr["LabHours"] = Convert.ToDouble(dict["txtHours"]);
                    }
                    if (dict["txtLabRate"].ToString().Trim() != string.Empty)
                    {
                        dr["LabRate"] = Convert.ToDouble(dict["txtLabRate"]);
                        labExt = Convert.ToDouble(dict["txtLabRate"]);
                        if (dict["txtHours"].ToString().Trim() != string.Empty)
                        {
                            labExt = labExt * Convert.ToDouble(dict["txtHours"]);
                        }
                        dr["LabExt"] = labExt;
                    }
                    dr["TotalExt"] = labExt + budgetExt;
                    if (dict["txtSDate"].ToString().Trim() != string.Empty)
                    {
                        dr["SDate"] = Convert.ToDateTime(dict["txtSDate"]);
                    }
                    dt.Rows.Add(dr);
                }
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
        return dt;
    }
    private void FillBomType()
    {
        try
        {
            DataSet ds = new DataSet();
            objJob.ConnConfig = Session["config"].ToString();
            ds = objBL_Job.GetBomType(objJob);

            //DataRow dr = ds.Tables[0].NewRow();
            //dr["ID"] = 0;
            //dr["Type"] = "Select Type";
            //ds.Tables[0].Rows.InsertAt(dr, 0);

            dtBomType = ds.Tables[0];

            #region Get Materila & Invenory Items
            //objJob.ConnConfig = Session["config"].ToString();
            //DataSet dsInv = objBL_Job.GetInventoryItem(objJob);
            //dtInventoryItem = dsInv.Tables[0];

            //objJob.ConnConfig = Session["config"].ToString();
            //DataSet dsLab = objBL_Job.GetLabourMaterial(objJob);
            //dtLabourMaterial = dsLab.Tables[0];
            #endregion
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    private void FillInventory()
    {
        try
        {
            objJob.ConnConfig = Session["config"].ToString();
            DataSet dsInv = objBL_Job.GetInventoryItem(objJob);

            DataRow dr = dsInv.Tables[0].NewRow();
            dr["MatItem"] = 0;
            dr["MatDesc"] = "Select Material";
            dsInv.Tables[0].Rows.InsertAt(dr, 0);

            dtMat = dsInv.Tables[0];
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    private void FillWage()
    {
        try
        {
            //objWage.ConnConfig = Session["config"].ToString();
            //DataSet dsWage = objBL_User.GetAllWage(objWage);
            objPropUser.ConnConfig = Session["config"].ToString();
            DataSet dsWage = objBL_User.getWage(objPropUser);

            DataRow dr = dsWage.Tables[0].NewRow();
            dr["LabItem"] = 0;
            dr["LabDesc"] = "Select Labor";
            dsWage.Tables[0].Rows.InsertAt(dr, 0);

            dtLab = dsWage.Tables[0];
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    private void FillItems(DropDownList ddlItem)
    {
        ddlItem.Items.Clear();
        ddlItem.Items.Add(new ListItem("No data found", "0"));
        ddlItem.DataBind();
    }
    #endregion

    private void FillLoc()
    {
        DataSet ds = new DataSet();
        objPropUser.SearchValue = "";
        objPropUser.ConnConfig = Session["config"].ToString();
        objPropUser.CustomerID = Convert.ToInt32(hdnCustID.Value);
        #region Company Check
        objPropUser.UserID = Convert.ToInt32(Session["UserID"].ToString());
        if (Convert.ToString(Session["CmpChkDefault"]) == "1")
        {
            objPropUser.EN = 1;
        }
        else
        {
            objPropUser.EN = 0;
        }
        #endregion
        ds = objBL_User.getLocationAutojquery(objPropUser);

        if (ds.Tables[0].Rows.Count == 1)
        {
            hdnLocID.Value = ds.Tables[0].Rows[0]["value"].ToString();
            txtLocation.Text = ds.Tables[0].Rows[0]["label"].ToString();
            FillAddress();
            BindEquip();
        }
        //Set Hyperlink  For Loc / Customer
        if (hdnLocID.Value != "0")
        {
            lnkLocationID.NavigateUrl = "addlocation.aspx?uid=" + hdnLocID.Value;
            GetDataEquip();
        }
        else
            lnkLocationID.NavigateUrl = "";

        if (hdnCustID.Value != "0")
            lnkCustomerID.NavigateUrl = "addcustomer.aspx?uid=" + hdnCustID.Value;


    }
    private void Fill_gvContacts()
    {
        int JobID = Request.QueryString["uid"] == null ? 0 : Convert.ToInt32(Request.QueryString["uid"]);
        ViewState["mode"] = "0";
        if (JobID > 0)
        {
            ViewState["mode"] = "1";
            objJob.ConnConfig = Session["config"].ToString();
            objJob.Job = JobID;
            DataSet ds = objBL_Job.GetContactForJob(objJob, new GeneralFunctions().GetSalesAsigned());

            if (ds.Tables[0].Rows.Count > 0)
            {

                gvContacts.DataSource = ds;
                gvContacts.DataBind();


                foreach (GridDataItem row in gvContacts.Items)
                {
                    CheckBox check = (CheckBox)row.FindControl("chkBoxIsRecd");
                    if (check.Checked) { row.BackColor = Color.Orange; }
                }
            }
            else
            {
                gvContacts.DataSource = null;
                gvContacts.DataBind();

            }
        }
    }

    private void FillGCCustomer()
    {
        DataSet ds = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();
        objPropUser.CustomerID = Convert.ToInt32(hdnCustID.Value);
        ds = objBL_User.getGCCustomer(objPropUser);

        if (ds.Tables[0].Rows.Count == 1)
        {
            if (ds.Tables[0].Rows[0]["type"].ToString().Equals("General Contractor", StringComparison.CurrentCultureIgnoreCase))
            {
                hdnGCID.Value = ds.Tables[0].Rows[0]["rol"].ToString();
                hdnGCIDtemp.Value = ds.Tables[0].Rows[0]["rol"].ToString();
                hdnGCName.Value = ds.Tables[0].Rows[0]["name"].ToString();
                txtName.Text = ds.Tables[0].Rows[0]["name"].ToString();
                txtCity.Text = ds.Tables[0].Rows[0]["city"].ToString();
                GctxtAddress.Text = ds.Tables[0].Rows[0]["Address"].ToString();
                ddlState.SelectedValue = ds.Tables[0].Rows[0]["state"].ToString();
                txtCountry.Text = ds.Tables[0].Rows[0]["country"].ToString();
                txtPostalCode.Text = ds.Tables[0].Rows[0]["zip"].ToString();
                txtRemarks.Text = ds.Tables[0].Rows[0]["remarks"].ToString();
                txtContactName.Text = ds.Tables[0].Rows[0]["contact"].ToString();
                txtPhone.Text = ds.Tables[0].Rows[0]["phone"].ToString();
                txtFax.Text = ds.Tables[0].Rows[0]["fax"].ToString();
                txtEmailWeb.Text = ds.Tables[0].Rows[0]["email"].ToString();
                txtMobile.Text = ds.Tables[0].Rows[0]["cellular"].ToString();
                hdnGContractorID.Value = ds.Tables[0].Rows[0]["Rol"].ToString();
                hdnGCNameupdate.Value = "0";
            }
            else
            {
                hdnGCID.Value = string.Empty;
                hdnGCIDtemp.Value = string.Empty;
                hdnGCName.Value = string.Empty;
                txtName.Text = string.Empty;
                txtCity.Text = string.Empty;
                GctxtAddress.Text = string.Empty;
                ddlState.SelectedValue = "Select State";
                txtCountry.Text = string.Empty;
                txtPostalCode.Text = string.Empty;
                txtRemarks.Text = string.Empty;
                txtContactName.Text = string.Empty;
                txtPhone.Text = string.Empty;
                txtFax.Text = string.Empty;
                txtEmailWeb.Text = string.Empty;
                txtMobile.Text = string.Empty;
                hdnGContractorID.Value = string.Empty;
                hdnGCNameupdate.Value = string.Empty;
            }

            if (ds.Tables[0].Rows[0]["type"].ToString().Equals("Homeowner", StringComparison.CurrentCultureIgnoreCase))
            {
                hotxtname.Text = ds.Tables[0].Rows[0]["name"].ToString();
                hotxtcity.Text = ds.Tables[0].Rows[0]["city"].ToString();
                HotxtAddress.Text = ds.Tables[0].Rows[0]["Address"].ToString();
                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["state"].ToString()))
                {
                    hotddlstate.SelectedValue = ds.Tables[0].Rows[0]["state"].ToString();
                }
                hotxtZIP.Text = ds.Tables[0].Rows[0]["zip"].ToString();
                hotxtCountry.Text = ds.Tables[0].Rows[0]["country"].ToString();
                hotxtPhone.Text = ds.Tables[0].Rows[0]["phone"].ToString();
                hotxtMobile.Text = ds.Tables[0].Rows[0]["cellular"].ToString();
                HotxtFax.Text = ds.Tables[0].Rows[0]["fax"].ToString();
                HotxtEmailWeb.Text = ds.Tables[0].Rows[0]["email"].ToString();
                hotxtRemarks.Text = ds.Tables[0].Rows[0]["remarks"].ToString();
                hotxtContactName.Text = ds.Tables[0].Rows[0]["contact"].ToString();
                hdnHomeOwnerID.Value = ds.Tables[0].Rows[0]["rol"].ToString();
                hdnHOName.Value = ds.Tables[0].Rows[0]["name"].ToString();
                hdnHONameupdate.Value = "0";
            }
            else
            {
                hotxtname.Text = string.Empty;
                hotxtcity.Text = string.Empty;
                HotxtAddress.Text = string.Empty;
                hotddlstate.SelectedValue = "Select State";
                hotxtZIP.Text = string.Empty;
                hotxtCountry.Text = string.Empty;
                hotxtPhone.Text = string.Empty;
                hotxtMobile.Text = string.Empty;
                HotxtFax.Text = string.Empty;
                HotxtEmailWeb.Text = string.Empty;
                hotxtRemarks.Text = string.Empty;
                hotxtContactName.Text = string.Empty;
                hdnHomeOwnerID.Value = string.Empty;
                hdnHOName.Value = string.Empty;
                hdnHONameupdate.Value = string.Empty;
            }
        }
    }
    private void FillJobStatus()
    {
        objJob.ConnConfig = Session["config"].ToString();
        DataSet _ds = objBL_Job.GetJobStatus(objJob);
        if (_ds.Tables[0].Rows.Count > 0)
        {
            ddlJobStatus.Items.Clear();
            ddlJobStatus.Items.Add(new ListItem("Select Status"));
            ddlJobStatus.AppendDataBoundItems = true;
            ddlJobStatus.DataSource = _ds;
            ddlJobStatus.DataValueField = "ID";
            ddlJobStatus.DataTextField = "Status";
            ddlJobStatus.DataBind();
        }
        else
        {
            ddlJobStatus.Items.Clear();
            ddlJobStatus.Items.Add(new ListItem("No data found", "Select Status"));
        }
    }
    private void FillAddress()
    {
        if (!string.IsNullOrEmpty(hdnLocID.Value))
        {
            objPropUser.DBName = Session["dbname"].ToString();
            objPropUser.LocID = Convert.ToInt32(hdnLocID.Value);
            DataSet ds = new DataSet();
            ds = objBL_User.getLocationByID(objPropUser);

            if (ds.Tables[0].Rows.Count > 0)
            {
                #region Salesperson1  and Salesperson2 
                string Terr = ds.Tables[0].Rows[0]["Terr"].ToString();
                string Terr2 = ds.Tables[0].Rows[0]["Terr2"].ToString();

                if (ddlTerr.Items.FindByValue(Terr) != null)
                {
                    ddlTerr.SelectedValue = Terr;
                }
                else
                {
                    ddlTerr.SelectedValue = "";
                }
                if (ddlTerr2.Items.FindByValue(Terr2) != null)
                {
                    ddlTerr2.SelectedValue = Terr2;
                }
                else
                {
                    ddlTerr.SelectedValue = "";
                }
                #endregion
                txtLocation.Text = ds.Tables[0].Rows[0]["tag"].ToString();
                txtAddress.Text = ds.Tables[0].Rows[0]["LocAddress"].ToString() + Environment.NewLine + ds.Tables[0].Rows[0]["Loccity"].ToString() + ", " + ds.Tables[0].Rows[0]["LocState"].ToString() + ", " + ds.Tables[0].Rows[0]["LocZip"].ToString();
                txtCompany.Text = ds.Tables[0].Rows[0]["Company"].ToString();
                if (string.IsNullOrEmpty(hdnCustID.Value))
                {
                    txtCustomer.Text = ds.Tables[0].Rows[0]["custname"].ToString();
                    hdnCustID.Value = ds.Tables[0].Rows[0]["owner"].ToString();
                }
                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["stax"].ToString()))
                {
                    txtSalesTax.Text = ds.Tables[0].Rows[0]["Rate"].ToString();
                    hdnGlobal_SalesTax.Value = Convert.ToString(ds.Tables[0].Rows[0]["Rate"]);
                    lblSalesTax.Text = ds.Tables[0].Rows[0]["stax"].ToString() + " - " + ds.Tables[0].Rows[0]["Rate"].ToString() + " %";
                }
                ddlTerms.SelectedValue = ds.Tables[0].Rows[0]["defaultterms"].ToString();
                hdnGlobal_Terms.Value = ds.Tables[0].Rows[0]["defaultterms"].ToString();
                //Set Hyperlink  For Loc / Customer
                if (hdnLocID.Value != "0")
                {
                    lnkLocationID.NavigateUrl = "addlocation.aspx?uid=" + hdnLocID.Value;
                    GetDataEquip();
                }
                else
                    lnkLocationID.NavigateUrl = "";

                if (hdnCustID.Value != "0")
                    lnkCustomerID.NavigateUrl = "addcustomer.aspx?uid=" + hdnCustID.Value;
                else
                    lnkCustomerID.NavigateUrl = "";

                if (ds.Tables[0].Columns.Contains("BillRate"))
                {
                    if (ds.Tables[0].Rows[0]["BillRate"].ToString() == null)
                        txtBillRate.Text = string.Empty;
                    else
                        txtBillRate.Text = string.Format("{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["BillRate"].ToString()));
                    if (ds.Tables[0].Rows[0]["RateOT"].ToString() == null)
                        txtOt.Text = string.Empty;
                    else
                        txtOt.Text = string.Format("{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["RateOT"].ToString()));
                    if (ds.Tables[0].Rows[0]["RateNT"].ToString() == null)
                        txtNt.Text = string.Empty;
                    else
                        txtNt.Text = string.Format("{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["RateNT"].ToString()));

                    if (ds.Tables[0].Rows[0]["RateDT"].ToString() == null)
                        txtDt.Text = string.Empty;
                    else
                        txtDt.Text = string.Format("{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["RateDT"].ToString()));
                    if (ds.Tables[0].Rows[0]["RateMileage"].ToString() == null)
                        txtMileage.Text = string.Empty;
                    else
                        txtMileage.Text = string.Format("{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["RateMileage"].ToString()));
                    if (ds.Tables[0].Rows[0]["RateTravel"].ToString() == null)
                        txtTravel.Text = string.Empty;
                    else
                        txtTravel.Text = string.Format("{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["RateTravel"].ToString()));
                    if (Request.QueryString["locid"] != null)
                    {
                        if (txtREPdesc.Text.Trim() == string.Empty)
                            txtREPdesc.Text = txtLocation.Text = ds.Tables[0].Rows[0]["tag"].ToString();
                    }
                    // Fill GC and Homeowner info 
                    if (!string.IsNullOrEmpty(hdnLocID.Value))
                        GetGC_HOInfo(hdnLocID.Value);
                }
            }
            else
            {
                txtAddress.Text = "0.00";
                txtBillRate.Text = "0.00";
                txtOt.Text = "0.00";
                txtNt.Text = "0.00";
                txtDt.Text = "0.00";
                txtMileage.Text = "0.00";
                txtTravel.Text = "0.00";
            }

        }
    }
    private void FillJobType()
    {
        try
        {
            DataSet _dsJob = new DataSet();
            objJob.ConnConfig = Session["config"].ToString();
            _dsJob = objBL_Job.GetAllJobType(objJob);
            if (_dsJob.Tables[0].Rows.Count > 0)
            {
                ddlJobType.Items.Add(new ListItem("Select Type", "Select Type"));
                ddlJobType.AppendDataBoundItems = true;
                ddlJobType.DataSource = _dsJob;
                ddlJobType.DataValueField = "ID";
                ddlJobType.DataTextField = "Type";
                ddlJobType.DataBind();
            }
            else
            {
                ddlJobType.Items.Add(new ListItem("No data found", "Select Type"));
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    private void FillProjectsTemplate()
    {
        DataSet ds = new DataSet();
        objProp_Customer.ConnConfig = Session["config"].ToString();
        ds = objBL_Customer.getJobProjectTemp(objProp_Customer);
        if (ds.Tables[0].Rows.Count > 0)
        {
            rfvTemplateType.InitialValue = "Select Template";
            if (Request.QueryString["uid"] == null)
            {
                DataRow dr = ds.Tables[0].Select("id = 0").FirstOrDefault();
                if (dr != null)
                {
                    ds.Tables[0].Rows.Remove(dr);
                }
            }
            ddlTemplate.DataSource = ds.Tables[0];
            ddlTemplate.DataTextField = "Fdesc";
            ddlTemplate.DataValueField = "id";
            ddlTemplate.DataBind();
            ddlTemplate.Items.Insert(0, new ListItem("Select Template"));
        }
        else
        {
            rfvTemplateType.InitialValue = "No data found";
            ddlTemplate.Items.Insert(0, new ListItem("No data found", "0"));
        }
    }

    #region Attribute
    private DataTable GetTeamItems()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Line", typeof(int));
        dt.Columns.Add("JobID", typeof(int));
        dt.Columns.Add("Title", typeof(string));
        dt.Columns.Add("UserID", typeof(string));
        dt.Columns.Add("FirstName", typeof(string));
        dt.Columns.Add("LastName", typeof(string));
        dt.Columns.Add("Email", typeof(string));
        dt.Columns.Add("Mobile", typeof(string));

        try
        {
            string strItems = hdnItemTeamJSON.Value.Trim();

            if (strItems != string.Empty)
            {
                JavaScriptSerializer sr = new JavaScriptSerializer();
                List<Dictionary<object, object>> objEstimateItemData = new List<Dictionary<object, object>>();
                objEstimateItemData = sr.Deserialize<List<Dictionary<object, object>>>(strItems);
                int i = 0;
                //if(objEstimateItemData.Count > 0)
                //{
                foreach (Dictionary<object, object> dict in objEstimateItemData)
                {
                    if (dict["txtTitle"].ToString().Trim() == string.Empty)
                    {
                        return dt;
                    }
                    i++;
                    DataRow dr = dt.NewRow();
                    dr["Line"] = i;
                    if (Request.QueryString["uid"] != null)
                    {
                        dr["JobID"] = Convert.ToInt32(Request.QueryString["uid"]);
                    }
                    else
                    {
                        dr["JobID"] = 0;
                    }
                    dr["Title"] = dict["txtTitle"].ToString().Trim();
                    dr["UserID"] = dict["txtUserID"].ToString().Trim();
                    dr["FirstName"] = dict["txtFirstName"].ToString().Trim();
                    dr["LastName"] = dict["txtLastName"].ToString().Trim();
                    dr["Email"] = dict["txtEmail"].ToString().Trim();
                    dr["Mobile"] = dict["txtMobile"].ToString().Trim();
                    dt.Rows.Add(dr);
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return dt;
    }
    private void FillState()
    {
        try
        {
            DataSet dsState = new DataSet();

            objState.ConnConfig = Session["config"].ToString();
            dsState = objBL_Bank.GetStates(objState);
            if (dsState.Tables[0].Rows.Count > 0)
            {
                ddlState.Items.Add(new ListItem("Select State"));
                ddlState.AppendDataBoundItems = true;

                ddlState.DataSource = dsState;
                ddlState.DataValueField = "Name";
                ddlState.DataTextField = "fDesc";
                ddlState.DataBind();


                hotddlstate.Items.Add(new ListItem("Select State"));
                hotddlstate.AppendDataBoundItems = true;

                hotddlstate.DataSource = dsState;
                hotddlstate.DataValueField = "Name";
                hotddlstate.DataTextField = "fDesc";
                hotddlstate.DataBind();
            }
            else
            {
                hotddlstate.Items.Add(new ListItem("No data found", "0"));
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    private void CreateTeamTable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Title", typeof(string));
        dt.Columns.Add("UserID", typeof(string));
        dt.Columns.Add("FirstName", typeof(string));
        dt.Columns.Add("LastName", typeof(string));
        dt.Columns.Add("Email", typeof(string));
        dt.Columns.Add("Mobile", typeof(string));

        DataRow dr = dt.NewRow();
        dt.Rows.Add(dr);

        DataRow dr1 = dt.NewRow();
        dt.Rows.Add(dr1);

        ViewState["TeamItems"] = dt;
        gvTeamItems.DataSource = dt;
        gvTeamItems.DataBind();
    }
    private void FillContractType()
    {
        try
        {
            DataSet _dsContract = new DataSet();
            //objJob.ConnConfig = Session["config"].ToString();
            //_dsContract = objBL_Job.GetContractType(objJob);
            objPropUser.ConnConfig = Session["config"].ToString();
            _dsContract = new BusinessLayer.Programs.BL_ServiceType().GetActiveServiceType(objPropUser.ConnConfig);
            if (_dsContract.Tables[0].Rows.Count > 0)
            {
                //ddlContractType.Items.Add(new ListItem("Select Service Type", "0"));
                //ddlContractType.AppendDataBoundItems = true;
                //ddlContractType.DataSource = _dsContract;
                //ddlContractType.DataValueField = "Type";
                //ddlContractType.DataTextField = "Type";
                //ddlContractType.DataBind();

                ddlContractType1.Items.Add(new ListItem("Select Service Type", "0"));
                ddlContractType1.AppendDataBoundItems = true;
                ddlContractType1.DataSource = _dsContract;
                ddlContractType1.DataValueField = "Type";
                ddlContractType1.DataTextField = "Type";
                ddlContractType1.DataBind();
            }
            else
            {
                //ddlContractType.Items.Add(new ListItem("No data found", "0"));
                ddlContractType1.Items.Add(new ListItem("No data found", "0"));
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    private void BindEquip()
    {
        objPropUser.ConnConfig = Session["config"].ToString();
        if (string.IsNullOrEmpty(hdnLocID.Value))
        {
            objPropUser.LocID = 0;
        }
        else
        {
            objPropUser.LocID = Convert.ToInt32(hdnLocID.Value);
        }

        DataSet ds = objBL_User.getElevByLoc(objPropUser);
        //gvEquip.DataSource = ds.Tables[0];
        //gvEquip.DataBind();
        rtEquips.DataSource = ds.Tables[0];
        rtEquips.DataBind();
    }
    #endregion

    #region Budget
    private DataTable GetOpenCalls(string sort)
    {

        DataSet ds = new DataSet();
        objMapData = new MapData();
        objMapData.ConnConfig = Session["config"].ToString();
        objMapData.jobid = Convert.ToInt32(Request.QueryString["uid"].ToString());
        objMapData.Assigned = Convert.ToInt32(ddlStatus.SelectedValue);
        if (sort == string.Empty)
            objMapData.OrderBy = "edate desc";
        else
            objMapData.OrderBy = sort;
        objMapData.Department = -1;
        objMapData.IsList = 1;

        ds = new BL_Tickets().getCallHistory(objMapData);
        return ds.Tables[0];
    }

    private void BindTicketList(String OrderBy, Int32 PageIndex)
    {
        DataSet ds = new DataSet();
        objMapData = new MapData();
        objMapData.ConnConfig = Session["config"].ToString();
        objMapData.jobid = Convert.ToInt32(Request.QueryString["uid"].ToString());
        objMapData.Assigned = Convert.ToInt32(ddlStatus.SelectedValue);
        objMapData.PageSize = Convert.ToInt32(PageSize);
        objMapData.PageIndex = PageIndex;

        if (OrderBy == string.Empty)
            objMapData.OrderBy = "edate desc";
        else
            objMapData.OrderBy = OrderBy;

        ViewState["TicketOrderBy"] = objMapData.OrderBy;

        ds = objBL_MapData.GetProjectTickets(objMapData);
        gvTickets.DataSource = ds.Tables[0];
        gvTickets.DataBind();

        //if the user does not have permission to view project finance then  hide the Labor Expense and Expenses column.

        if (hdnFinancePermission.Value == "N")
        {
            foreach (DataControlField col in gvTickets.Columns)
            {
                if (col.HeaderText == "Labor Expenses" || col.HeaderText == "Expenses")
                {
                    col.Visible = false;
                }
            }
        }

        Int32 TotalRecord = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
        this.PopulatePager(TotalRecord, PageIndex, rptTicketsPager);

        if (ds.Tables[1].Rows.Count > 0)
        {
            CalculateBalance(ds.Tables[2]);
        }
        lblTicketCount.Text = Convert.ToString(ds.Tables[0].Rows[0][0]) + " ticket(s) found.";


    }
    private void GetInvoices()
    {
        DataSet ds = new DataSet();
        objProp_Contracts.ConnConfig = Session["config"].ToString();
        objProp_Contracts.jobid = Convert.ToInt32(Request.QueryString["uid"].ToString());
        if (ddlInvoiceStatus.SelectedValue != "-1")
        {
            objProp_Contracts.SearchBy = "i.Status";
            objProp_Contracts.SearchValue = ddlInvoiceStatus.SelectedValue;
        }
        ds = objBL_Contracts.GetInvoices(objProp_Contracts,null,null,null);
        gvInvoice.DataSource = ds.Tables[0];
        gvInvoice.DataBind();
        calculateInvoice(ds.Tables[0]);
        //gvArInvoice.DataSource = ds.Tables[0];
        //gvArInvoice.DataBind();
    }
    private void GetAPInvoices()
    {
        DataSet ds = new DataSet();
        objProp_Contracts.ConnConfig = Session["config"].ToString();
        objProp_Contracts.jobid = Convert.ToInt32(Request.QueryString["uid"].ToString());

        ds = objBL_Contracts.GetAPInvoices(objProp_Contracts);
        gvAPInvoices.DataSource = ds.Tables[0];
        gvAPInvoices.DataBind();
    }
    private void GetJobCost()
    {
        DataSet ds = new DataSet();
        objProp_Contracts.ConnConfig = Session["config"].ToString();
        objProp_Contracts.jobid = Convert.ToInt32(Request.QueryString["uid"].ToString());

        ds = objBL_Contracts.GetJobCostItems(objProp_Contracts);
        gvJOBC.DataSource = ds.Tables[0];
        gvJOBC.DataBind();
    }
    private void CalculateBalance(DataTable dt)
    {
        //double dblBalTotal = 0;
        //double dblExpenses = 0;
        //double dblEST = 0;
        //double dblLabExpenses = 0;
        //double dblRT = 0;
        //double dblOT = 0;
        //double dblDT = 0;
        //double dblTT = 0;
        //double dblNT = 0;

        if (dt.Rows.Count > 0)
        {
            #region Commented Code
            //foreach (DataRow dr in dt.Rows)
            //{
            //    if (dr["tottime"].ToString() != string.Empty)
            //    {
            //        dblBalTotal += Convert.ToDouble(dr["tottime"].ToString());
            //    }
            //    if (dr["expenses"].ToString() != string.Empty)
            //    {
            //        dblExpenses += Convert.ToDouble(dr["expenses"].ToString());
            //    }
            //    if (dr["est"].ToString() != string.Empty)
            //    {
            //        dblEST += Convert.ToDouble(dr["est"].ToString());
            //    }
            //    if (dr["laborexp"].ToString() != string.Empty)
            //    {
            //        dblLabExpenses += Convert.ToDouble(dr["laborexp"].ToString());
            //    }
            //    if (dr["reg"].ToString() != string.Empty)
            //    {
            //        dblRT += Convert.ToDouble(dr["reg"].ToString());
            //    }
            //    if (dr["ot"].ToString() != string.Empty)
            //    {
            //        dblOT += Convert.ToDouble(dr["ot"].ToString());
            //    }
            //    if (dr["nt"].ToString() != string.Empty)
            //    {
            //        dblNT += Convert.ToDouble(dr["nt"].ToString());
            //    }
            //    if (dr["dt"].ToString() != string.Empty)
            //    {
            //        dblDT += Convert.ToDouble(dr["dt"].ToString());
            //    }
            //    if (dr["tt"].ToString() != string.Empty)
            //    {
            //        dblTT += Convert.ToDouble(dr["tt"].ToString());
            //    }
            //}
            #endregion  
            GridFooterItem footeritem = (GridFooterItem)gvTickets.MasterTableView.GetItems(GridItemType.Footer)[0];
            Label lblTotalFooter = (Label)footeritem.FindControl("lblTotalFooter");
            Label lblExpenseFooter = (Label)footeritem.FindControl("lblExpenseFooter");
            Label lblESTFooter = (Label)footeritem.FindControl("lblESTFooter");
            Label lblLabExpenseFooter = (Label)footeritem.FindControl("lblLabExpenseFooter");
            Label lblRTFooter = (Label)footeritem.FindControl("lblRTFooter");
            Label lblOTFooter = (Label)footeritem.FindControl("lblOTFooter");
            Label lblNTFooter = (Label)footeritem.FindControl("lblNTFooter");
            Label lblDTFooter = (Label)footeritem.FindControl("lblDTFooter");
            Label lblTTFooter = (Label)footeritem.FindControl("lblTTFooter");

            //lblTotalFooter.Text = string.Format("{0:n}", dblBalTotal);
            //lblExpenseFooter.Text = string.Format("{0:c}", dblExpenses);
            //lblESTFooter.Text = string.Format("{0:n}", dblEST);
            //lblLabExpenseFooter.Text = string.Format("{0:c}", dblLabExpenses);
            //lblRTFooter.Text = string.Format("{0:n}", dblRT);
            //lblOTFooter.Text = string.Format("{0:n}", dblOT);
            //lblNTFooter.Text = string.Format("{0:n}", dblNT);
            //lblDTFooter.Text = string.Format("{0:n}", dblDT);
            //lblTTFooter.Text = string.Format("{0:n}", dblTT);

            if (dt.Rows[0]["tottime"] != DBNull.Value)
            {
                lblTotalFooter.Text = string.Format("{0:n}", Convert.ToDouble(dt.Rows[0]["tottime"]));
            }
            if (dt.Rows[0]["expenses"] != DBNull.Value)
            {
                lblExpenseFooter.Text = string.Format("{0:c}", Convert.ToDouble(dt.Rows[0]["expenses"]));
            }
            if (dt.Rows[0]["EST"] != DBNull.Value)
            {
                lblESTFooter.Text = string.Format("{0:n}", Convert.ToDouble(dt.Rows[0]["EST"]));
            }
            if (dt.Rows[0]["laborexp"] != DBNull.Value)
            {
                lblLabExpenseFooter.Text = string.Format("{0:c}", Convert.ToDouble(dt.Rows[0]["laborexp"]));
            }
            if (dt.Rows[0]["Reg"] != DBNull.Value)
            {
                lblRTFooter.Text = string.Format("{0:n}", Convert.ToDouble(dt.Rows[0]["Reg"]));
            }
            if (dt.Rows[0]["OT"] != DBNull.Value)
            {
                lblOTFooter.Text = string.Format("{0:n}", Convert.ToDouble(dt.Rows[0]["OT"]));
            }
            if (dt.Rows[0]["NT"] != DBNull.Value)
            {
                lblNTFooter.Text = string.Format("{0:n}", Convert.ToDouble(dt.Rows[0]["NT"]));
            }
            if (dt.Rows[0]["DT"] != DBNull.Value)
            {
                lblDTFooter.Text = string.Format("{0:n}", Convert.ToDouble(dt.Rows[0]["DT"]));
            }
            if (dt.Rows[0]["TT"] != DBNull.Value)
            {
                lblTTFooter.Text = string.Format("{0:n}", Convert.ToDouble(dt.Rows[0]["TT"]));
            }
        }
    }
    private void CalculateItems(DataTable dt)
    {
        double dblActual = 0;
        double dblBudget = 0;
        double dblPercent = 0;

        if (dt.Rows.Count > 0)
        {
            foreach (DataRow dr in dt.Rows)
            {
                if (dr["actual"].ToString() != string.Empty)
                {
                    dblActual += Convert.ToDouble(dr["actual"].ToString());
                }
                if (dr["budget"].ToString() != string.Empty)
                {
                    dblBudget += Convert.ToDouble(dr["budget"].ToString());
                }
                if (dr["Percent"].ToString() != string.Empty)
                {
                    dblPercent += Convert.ToDouble(dr["Percent"].ToString());
                }
            }
            GridFooterItem footeritem = (GridFooterItem)gvBOM.MasterTableView.GetItems(GridItemType.Footer)[0];

            TextBox txtTActual = (TextBox)footeritem.FindControl("txtTActual");
            TextBox txtTBudget = (TextBox)footeritem.FindControl("txtTBudget");
            TextBox txtTPercent = (TextBox)footeritem.FindControl("txtTPercent");

            txtTActual.Text = string.Format("{0:n}", dblActual);
            txtTBudget.Text = string.Format("{0:n}", dblBudget);
            txtTPercent.Text = string.Format("{0:n}", dblPercent);
        }
    }
    private void calculateInvoice(DataTable dt)
    {
        lblCountInvoice.Text = dt.Rows.Count.ToString() + " Record(s) Found.";

        if (dt.Rows.Count > 0)
        {
            GridFooterItem footeritem = (GridFooterItem)gvInvoice.MasterTableView.GetItems(GridItemType.Footer)[0];

            Label lblTotalPretaxAmt = (Label)footeritem.FindControl("lblTotalPretaxAmt");
            Label lblTotalSalesTax = (Label)footeritem.FindControl("lblTotalSalesTax");
            Label lblTotalInvoice = (Label)footeritem.FindControl("InvTotalInvoice");
            Label lblTotalDue = (Label)footeritem.FindControl("InvTotalDue");

            double TotalPretaxAmt = 0;
            double TotalSalesTax = 0;
            double TotalInvoice = 0;
            double TotalDue = 0;

            double PretaxAmt = 0;
            double SalesTax = 0;
            double Invoice = 0;
            double Due = 0;

            foreach (DataRow dr in dt.Rows)
            {
                if (dr["amount"] != DBNull.Value && dr["amount"].ToString() != "")
                {
                    PretaxAmt = Convert.ToDouble(dr["amount"]);
                }

                if (dr["stax"] != DBNull.Value && dr["stax"].ToString() != "")
                {
                    SalesTax = Convert.ToDouble(dr["stax"]);
                }

                if (dr["total"] != DBNull.Value && dr["total"].ToString() != "")
                {
                    Invoice = Convert.ToDouble(dr["total"]);
                }

                Due = Convert.ToDouble(dr["balance"]);

                TotalPretaxAmt += PretaxAmt;
                TotalSalesTax += SalesTax;
                TotalInvoice += Invoice;
                TotalDue += Due;
            }

            lblTotalPretaxAmt.Text = string.Format("{0:c}", TotalPretaxAmt);
            lblTotalSalesTax.Text = string.Format("{0:c}", TotalSalesTax);
            lblTotalInvoice.Text = string.Format("{0:c}", TotalInvoice);
            lblTotalDue.Text = string.Format("{0:c}", TotalDue);
        }
    }
    #endregion

    #region Finance

    #region Budget
    private void BindBudget(int pageIndex)
    {
        try
        {
            #region Budget
            DataSet ds = new DataSet();

            objJob.Job = Convert.ToInt32(Request.QueryString["uid"].ToString());
            objJob.PageSize = Convert.ToInt32(PageSize);
            objJob.PageIndex = pageIndex;

            if (Request.QueryString["uid"] != null)
            {
                if (Request.QueryString["s"] != null && Request.QueryString["e"] != null)
                {
                    objJob.StartDate = Convert.ToDateTime(System.Web.HttpUtility.UrlDecode(Request.QueryString["s"].ToString()));
                    objJob.EndDate = Convert.ToDateTime(System.Web.HttpUtility.UrlDecode(Request.QueryString["e"].ToString()));
                }
            }

            ds = objBL_Job.GetJobCostByJob(objJob);

            if (ds.Tables[0].Rows.Count > 0)
            {
                ViewState["Budget"] = ds.Tables[0];

                gvBudget.DataSource = ds.Tables[0];
                gvBudget.DataBind();

                DataTable dtTotal = ds.Tables[1];

                GridFooterItem footeritem = (GridFooterItem)gvBudget.MasterTableView.GetItems(GridItemType.Footer)[0];

                Label lblFooterActual = footeritem.FindControl("lblFooterActual") as Label;
                Label lblFooterComm = footeritem.FindControl("lblFooterComm") as Label;
                Label lblFooterTotalActual = footeritem.FindControl("lblFooterTotalActual") as Label;
                Label lblFooterBudget = footeritem.FindControl("lblFooterBudget") as Label;
                Label lblFooterVariance = footeritem.FindControl("lblFooterVariance") as Label;
                Label lblFooterRatio = footeritem.FindControl("lblFooterRatio") as Label;

                lblFooterActual.Text = string.Format("{0:c}", Convert.ToDouble(dtTotal.Rows[0]["Actual"]));
                lblFooterComm.Text = string.Format("{0:c}", Convert.ToDouble(dtTotal.Rows[0]["Comm"]));
                lblFooterTotalActual.Text = string.Format("{0:c}", Convert.ToDouble(dtTotal.Rows[0]["Total"]));
                lblFooterBudget.Text = string.Format("{0:c}", Convert.ToDouble(dtTotal.Rows[0]["Budget"]));
                lblFooterVariance.Text = string.Format("{0:c}", Convert.ToDouble(dtTotal.Rows[0]["Variance"]));
                lblFooterRatio.Text = string.Format("{0:0.00}%", Convert.ToDouble(dtTotal.Rows[0]["Ratio"]));

                Int32 TotalRecord = Convert.ToInt32(ds.Tables[2].Rows[0][0]);
                this.PopulatePager(TotalRecord, pageIndex, rptBudgetPager);
            }

            #endregion

        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErrProj", "noty({text: '" + str + "',  type : 'error', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    private void BindExpense(int pageIndex)
    {
        try
        {
            #region Expense
            DataSet dsExp = new DataSet();

            objJob.Job = Convert.ToInt32(Request.QueryString["uid"].ToString());
            objJob.PageSize = Convert.ToInt32(PageSize);
            objJob.PageIndex = pageIndex;

            if (Request.QueryString["s"] != null && Request.QueryString["e"] != null)
            {
                dsExp = objBL_Job.GetExpenseJobCostByDate(objJob);
            }
            else
            {
                dsExp = objBL_Job.GetExpenseJobCost(objJob);
            }

            if (dsExp.Tables[0].Rows.Count > 0)
            {
                gvExpenses.DataSource = dsExp.Tables[0];
                gvExpenses.DataBind();

                GridFooterItem footeritem = (GridFooterItem)gvExpenses.MasterTableView.GetItems(GridItemType.Footer)[0];

                Label lblFooterMatAct = footeritem.FindControl("lblFooterMatAct") as Label;
                Label lblFooterMatBgt = footeritem.FindControl("lblFooterMatBgt") as Label;
                Label lblFooterMatMod = footeritem.FindControl("lblFooterMatMod") as Label;
                Label lblFooterMatDiff = footeritem.FindControl("lblFooterMatDiff") as Label;
                Label lblFooterHours = footeritem.FindControl("lblFooterHours") as Label;
                Label lblFooterHoursBgt = footeritem.FindControl("lblFooterHoursBgt") as Label;
                Label lblFooterLabAct = footeritem.FindControl("lblFooterLabAct") as Label;
                Label lblFooterLabBgt = footeritem.FindControl("lblFooterLabBgt") as Label;
                Label lblFooterLabMod = footeritem.FindControl("lblFooterLabMod") as Label;
                Label lblFooterLabDiff = footeritem.FindControl("lblFooterLabDiff") as Label;

                lblFooterMatAct.Text = string.Format("{0:c}", Convert.ToDouble(dsExp.Tables[0].Compute("SUM(MatAct)", string.Empty)));
                lblFooterMatBgt.Text = string.Format("{0:c}", Convert.ToDouble(dsExp.Tables[0].Compute("SUM(MatBgt)", string.Empty)));
                lblFooterMatMod.Text = string.Format("{0:c}", Convert.ToDouble(dsExp.Tables[0].Compute("SUM(MatMod)", string.Empty)));
                lblFooterMatDiff.Text = string.Format("{0:c}", Convert.ToDouble(dsExp.Tables[0].Compute("SUM(MatDiff)", string.Empty)));
                lblFooterHours.Text = string.Format("{0:c}", Convert.ToDouble(dsExp.Tables[0].Compute("SUM(HourAct)", string.Empty)));
                lblFooterHoursBgt.Text = string.Format("{0:c}", Convert.ToDouble(dsExp.Tables[0].Compute("SUM(HourBgt)", string.Empty)));
                lblFooterLabAct.Text = string.Format("{0:c}", Convert.ToDouble(dsExp.Tables[0].Compute("SUM(LaborAct)", string.Empty)));
                lblFooterLabBgt.Text = string.Format("{0:c}", Convert.ToDouble(dsExp.Tables[0].Compute("SUM(LaborBgt)", string.Empty)));
                lblFooterLabMod.Text = string.Format("{0:c}", Convert.ToDouble(dsExp.Tables[0].Compute("SUM(LaborMod)", string.Empty)));
                lblFooterLabDiff.Text = string.Format("{0:c}", Convert.ToDouble(dsExp.Tables[0].Compute("SUM(LabDiff)", string.Empty)));

                Int32 TotalRecord = Convert.ToInt32(dsExp.Tables[1].Rows[0][0]);
                this.PopulatePager(TotalRecord, pageIndex, rptExpensesPager);
            }

            #endregion
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErrProj", "noty({text: '" + str + "',  type : 'error', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    //protected void gvBudget_RowCreated(object sender, GridViewRowEventArgs e)
    //{
    //    if (e.Row.RowType == DataControlRowType.DataRow)
    //    {


    //    }
    //}
    protected void Page_Init(object sender, EventArgs e)
    {
        //gvBudget.RowDataBound();
    }
    //protected override void OnInit(EventArgs e)
    //{
    //    base.OnInit(e);
    //} 
    //private void CreateCell()
    //{
    //    //Int16 typeCount = 0;

    //    //headerJobType = "Revenues";
    //    foreach (GridViewRow gr in gvBudget.Rows)
    //    {
    //        Label lblType = (Label)gr.FindControl("lblJobType");

    //        if (gr.RowType == DataControlRowType.DataRow)
    //        {
    //            //DataRowView drv = (DataRowView)gr.DataItem;
    //            //if (drv != null)
    //            //{
    //                if (this.gvBudget.Columns.Count > 0)
    //                {
    //                    if (lblType.Text.ToString() != null)
    //                    {
    //                        if (headerJobType != lblType.Text.ToString())
    //                        {
    //                            headerJobType = lblType.Text.ToString();

    //                            Table tbl = gr.Parent as Table;
    //                            if (tbl != null)
    //                            {
    //                                GridViewRow row = new GridViewRow(-1, -1, DataControlRowType.DataRow, DataControlRowState.Normal);
    //                                TableCell cell = new TableCell();

    //                                // Span the row across all of the columns in the Gridview
    //                                cell.ColumnSpan = this.gvBudget.Columns.Count;

    //                                cell.Width = Unit.Percentage(100);
    //                                cell.Style.Add("font-weight", "bold");
    //                                cell.Style.Add("background-color", "#2b6394");
    //                                cell.Style.Add("color", "white");

    //                                HtmlGenericControl span = new HtmlGenericControl("span");
    //                                span.InnerHtml = headerJobType;

    //                                cell.Controls.Add(span);
    //                                row.Cells.Add(cell);

    //                                tbl.Rows.AddAt(tbl.Rows.Count - 1, row);
    //                            }
    //                        }
    //                    }
    //                }
    //            //}
    //        }
    //    }
    //}
    //protected void gvBudget_DataBound(object sender, EventArgs e)
    //{
    //    //CreateCell();
    //}
    //protected void gvBudget_RowCreated(object sender, GridViewRowEventArgs e)
    //{
    //    //int indexCount = 0;
    //    //int temp = 0;
    //    //GridView gBudget= (GridView)sender;
    //    if (e.Row.RowType == DataControlRowType.DataRow)
    //    {
    //        //Label lblJobType = (Label)e.Row.FindControl("lblJobType");
    //        if (this.gvBudget.Columns.Count > 0)
    //        {
    //            //if(e.Row.DataItem != null)
    //            //{
    //            //    if (DataBinder.Eval(e.Row.DataItem, "JobType").ToString() != null)
    //            //    {
    //            //        //var temp = DataBinder.Eval(e.Row.DataItem, "JobType").ToString();
    //            //        if (headerJobType != DataBinder.Eval(e.Row.DataItem, "JobType").ToString())
    //            //        {
    //            //            headerJobType = DataBinder.Eval(e.Row.DataItem, "JobType").ToString();
    //            //            //if(headerJobType == "Revenues")
    //            //            //{
    //            //            //    index = e.Row.RowIndex;
    //            //            //}
    //            //            //else
    //            //            //{
    //            //            //    index = e.Row.RowIndex + 1;
    //            //            //}
    //            //            //index = e.Row.RowIndex;
    //            //            //Table tbl = e.Row.Parent as Table;
    //            //            //if (tbl != null)
    //            //            //{
    //            //            Table tbl = e.Row.Parent as Table;
    //            //            if (tbl == null)
    //            //                temp = 1;
    //            //            else
    //            //                temp = tbl.Rows.Count;
    //            //                GridViewRow row = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal);
    //            //                TableCell cell = new TableCell();

    //            //                // Span the row across all of the columns in the Gridview
    //            //                cell.ColumnSpan = this.gvBudget.Columns.Count;

    //            //                cell.Width = Unit.Percentage(100);
    //            //                cell.Style.Add("font-weight", "bold");
    //            //                cell.Style.Add("background-color", "#2b6394");
    //            //                cell.Style.Add("color", "white");

    //            //                HtmlGenericControl span = new HtmlGenericControl("span");
    //            //                span.InnerHtml = headerJobType;

    //            //                cell.Controls.Add(span);
    //            //                row.Cells.Add(cell);

    //            //                //tbl.Rows.AddAt(tbl.Rows.Count - 1, row);

    //            //            if(tbl != null)
    //            //            {
    //            //                //tbl.Rows.AddAt(tbl.Rows.Count - 1, row);
    //            //            }
    //            //            else
    //            //            {
    //            //            //    e.Row.Controls[0].Controls.AddAt(temp + 1, row);
    //            //            }
    //            //                indexCount++;
    //            //                //index = e.Row.RowIndex + 1;
    //            //            //}
    //            //        }
    //            //    }
    //            //}
    //            GridView HeaderGrid = (GridView)sender;
    //            //GridViewRow HeaderGridRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);
    //            GridViewRow HeaderGridRow = new GridViewRow(-1, -1, DataControlRowType.DataRow, DataControlRowState.Normal);
    //            HeaderGridRow.Font.Bold = true;
    //            HeaderGridRow.HorizontalAlign = HorizontalAlign.Center;

    //            TableCell HeaderCell = new TableCell();

    //            DataTable dt = (DataTable)ViewState["Budget"];
    //            DataRow dr = dt.Rows[e.Row.RowIndex];
    //            Table tbl = e.Row.Parent as Table;
    //            if(tbl != null)
    //            {
    //                if (e.Row.DataItem != null)
    //                {
    //                    var temp = gvBudget.DataKeys[e.Row.RowIndex].Value;
    //                    if (headerJobType != gvBudget.DataKeys[e.Row.RowIndex].Value)
    //                    {
    //                        headerJobType = dr["JobType"].ToString();
    //                        if (dr["JobType"].ToString() == "Revenues")
    //                        {
    //                            HeaderCell.Text = "Revenues";
    //                            HeaderCell.Style.Add("font-weight", "bold");
    //                            HeaderCell.Style.Add("background-color", "#2b6394");
    //                            HeaderCell.Style.Add("color", "white");
    //                            HeaderCell.ColumnSpan = this.gvBudget.Columns.Count;
    //                            HeaderGridRow.Cells.Add(HeaderCell);
    //                            //e.Row.Controls[0].Controls.AddAt(1, HeaderGridRow);
    //                            tbl.Rows.AddAt(1, HeaderGridRow);
    //                        }
    //                        if (dr["JobType"].ToString() == gvBudget.DataKeys[e.Row.RowIndex].Value)
    //                        {
    //                            HeaderCell = new TableCell();
    //                            HeaderCell.Text = "Costs";
    //                            HeaderCell.Style.Add("font-weight", "bold");
    //                            HeaderCell.Style.Add("background-color", "#2b6394");
    //                            HeaderCell.Style.Add("color", "white");
    //                            HeaderCell.ColumnSpan = this.gvBudget.Columns.Count;
    //                            HeaderGridRow.Cells.Add(HeaderCell);
    //                            //e.Row.Controls[0].Controls.AddAt(e.Row.RowIndex, HeaderGridRow);
    //                            tbl.Rows.AddAt(3, HeaderGridRow);
    //                        }
    //                    }
    //                }
    //            }

    //        }

    //    }
    //}

    //protected void gvChildGrid_RowDataBound(object sender, GridViewRowEventArgs e)
    //{
    //    try
    //    {
    //        ////if (e.Row.RowType == DataControlRowType.DataRow)
    //        ////{

    //        ////    GridView gv = (GridView)e.Row.FindControl("gvChildItemGrid");
    //        ////    HiddenField hdnType = (HiddenField)e.Row.FindControl("hdnType");
    //        ////    Label lblOpSeq = (Label)e.Row.FindControl("lblOpSeq");
    //        ////    objJob.Job = Convert.ToInt32(Request.QueryString["uid"]);
    //        ////    objJob.Type = Convert.ToInt16(hdnType.Value);
    //        ////    objJob.Code = lblOpSeq.Text;

    //        ////    DataSet ds = new DataSet();
    //        ////    ds = objBL_Job.GetJobCostTypeByJob(objJob);

    //        ////    gv.DataSource = ds.Tables[0];
    //        ////    gv.DataBind();
    //        ////}

    //    }
    //    catch (Exception ex)
    //    {
    //        string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
    //        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErrProj", "noty({text: '" + str + "',  type : 'error', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
    //    }
    //}
    //protected void gvChildItemGrid_RowDataBound(object sender, GridViewRowEventArgs e)
    //{
    //    try
    //    {
    //        if (e.Row.RowType == DataControlRowType.DataRow)
    //        {

    //            GridView gv = (GridView)e.Row.FindControl("gvInnerChildItemGrid");
    //            GridView gvInnerTicket = (GridView)e.Row.FindControl("gvInnerChildTicket");

    //            HiddenField hdnType = (HiddenField)e.Row.FindControl("hdnType");
    //            Label lblOpSeq = (Label)e.Row.FindControl("lblOpSeq");
    //            HiddenField hdnItemType = (HiddenField)e.Row.FindControl("hdnItemType");

    //            objJob.Job = Convert.ToInt32(Request.QueryString["uid"]);
    //            objJob.Type = Convert.ToInt16(hdnType.Value);
    //            objJob.Code = lblOpSeq.Text;
    //            objJob.TypeId = Convert.ToInt16(hdnItemType.Value);

    //            DataSet ds = new DataSet();

    //            ds = objBL_Job.GetJobCostInvoicesByJob(objJob);

    //            if (objJob.Type.Equals(1))
    //            {
    //                if (ds.Tables[0].Rows.Count > 1)
    //                {
    //                    gv.DataSource = ds.Tables[0];
    //                    gv.DataBind();

    //                    if (ds.Tables[0].Rows.Count > 0)
    //                    {
    //                        Label lblTotalAmount = (Label)gv.FooterRow.FindControl("lblTotalAmount");
    //                        Label lblTotalBudgetAmt = (Label)gv.FooterRow.FindControl("lblTotalBudgetAmt");
    //                        Label lblTotalActualAmt = (Label)gv.FooterRow.FindControl("lblTotalActualAmt");

    //                        lblTotalAmount.Text = string.Format("{0:c}", ds.Tables[0].Compute("SUM(Amount)", string.Empty));
    //                        lblTotalBudgetAmt.Text = string.Format("{0:c}", ds.Tables[0].Compute("SUM(Budget)", string.Empty));
    //                        lblTotalActualAmt.Text = string.Format("{0:c}", ds.Tables[0].Compute("SUM(Actual)", string.Empty));
    //                    }
    //                }

    //                DataSet dsTicket = new DataSet();
    //                dsTicket = objBL_Job.GetJobCostTicketsByJob(objJob);
    //                if (dsTicket.Tables[0].Rows.Count > 0)
    //                {
    //                    gvInnerTicket.DataSource = dsTicket.Tables[0];
    //                    gvInnerTicket.DataBind();

    //                    if (dsTicket.Tables[0].Rows.Count > 0)
    //                    {
    //                        DataTable dtTicket = dsTicket.Tables[0];
    //                        Label lblTotalEstHr = (Label)gvInnerTicket.FooterRow.FindControl("lblTotalEstHr");
    //                        Label lblTotalBudgetHr = (Label)gvInnerTicket.FooterRow.FindControl("lblTotalBudgetHr");
    //                        Label lblTotalActualHr = (Label)gvInnerTicket.FooterRow.FindControl("lblTotalActualHr");
    //                        Label lblTotalLaborExp = (Label)gvInnerTicket.FooterRow.FindControl("lblTotalLaborExp");
    //                        Label lblOtherExp = (Label)gvInnerTicket.FooterRow.FindControl("lblOtherExp");
    //                        Label lblTotalExp = (Label)gvInnerTicket.FooterRow.FindControl("lblTotalExp");

    //                        lblTotalEstHr.Text = string.Format("{0:n}", dtTicket.Compute("SUM(Est)", string.Empty));
    //                        lblTotalBudgetHr.Text = string.Format("{0:n}", dtTicket.Compute("SUM(BudgetHr)", string.Empty));
    //                        lblTotalActualHr.Text = string.Format("{0:n}", dtTicket.Compute("SUM(ActualHr)", string.Empty));
    //                        lblTotalLaborExp.Text = string.Format("{0:c}", dtTicket.Compute("SUM(LaborExp)", string.Empty));
    //                        lblOtherExp.Text = string.Format("{0:c}", dtTicket.Compute("SUM(Expenses)", string.Empty));
    //                        lblTotalExp.Text = string.Format("{0:c}", dtTicket.Compute("SUM(TotalExp)", string.Empty));
    //                    }
    //                }

    //            }
    //            else
    //            {

    //                gv.DataSource = ds.Tables[0];
    //                gv.DataBind();
    //                if(objJob.Type.Equals(0))
    //                {
    //                    gv.Columns[2].Visible = false;
    //                }

    //                if (ds.Tables[0].Rows.Count > 0)
    //                {
    //                    Label lblTotalAmount = (Label)gv.FooterRow.FindControl("lblTotalAmount");
    //                    Label lblTotalBudgetAmt = (Label)gv.FooterRow.FindControl("lblTotalBudgetAmt");
    //                    Label lblTotalActualAmt = (Label)gv.FooterRow.FindControl("lblTotalActualAmt");

    //                    lblTotalAmount.Text = string.Format("{0:c}", ds.Tables[0].Compute("SUM(Amount)", string.Empty));
    //                    lblTotalBudgetAmt.Text = string.Format("{0:c}", ds.Tables[0].Compute("SUM(Budget)", string.Empty));
    //                    lblTotalActualAmt.Text = string.Format("{0:c}", ds.Tables[0].Compute("SUM(Actual)", string.Empty));
    //                }
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
    //        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErrProj", "noty({text: '" + str + "',  type : 'error', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
    //    }
    //}
    #endregion

    private void GetJobTemplate(int tid)
    {
        try
        {
            objJob.ConnConfig = Session["config"].ToString();
            objJob.ID = tid;
            DataSet ds = objBL_Job.GetJobTFinanceByID(objJob);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow _dr = ds.Tables[0].Rows[0];
                hdnInvServiceID.Value = _dr["InvServ"].ToString();
                txtInvService.Text = _dr["InvServiceName"].ToString();
                hdnPrevilWageID.Value = _dr["Wage"].ToString();
                txtPrevilWage.Text = _dr["WageName"].ToString();
                uc_InterestGL._txtGLAcct.Text = _dr["GLName"].ToString();
                uc_InterestGL._hdnAcctID.Value = _dr["GLInt"].ToString();
                uc_InvExpGL._txtGLAcct.Text = _dr["InvExpName"].ToString();
                uc_InvExpGL._hdnAcctID.Value = _dr["InvExp"].ToString();
                if (!string.IsNullOrEmpty(_dr["CType"].ToString()))
                {
                    ddlContractType1.SelectedValue = _dr["CType"].ToString();
                }
                if (!string.IsNullOrEmpty(_dr["Post"].ToString()))
                {
                    ddlPostingMethod.SelectedValue = _dr["Post"].ToString();
                }
                hdnUnrecognizedRevenue.Value = _dr["UnrecognizedRevenue"].ToString();
                if (!String.IsNullOrEmpty(hdnUnrecognizedRevenue.Value))
                    AssignBillingCodeInGrid(hdnUnrecognizedRevenue.Value);
                txtUnrecognizedRevenue.Text = _dr["UnrecognizedRevenueName"].ToString();
                hdnUnrecognizedExpense.Value = _dr["UnrecognizedExpense"].ToString();
                txtUnrecognizedExpense.Text = _dr["UnrecognizedExpenseName"].ToString();
                hdnRetainageReceivable.Value = _dr["RetainageReceivable"].ToString();
                txtRetainageReceivable.Text = _dr["RetainageReceivableName"].ToString();
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
            objJob.ConnConfig = Session["config"].ToString();
            _dsPost = objBL_Job.GetPosting(objJob);
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
                ddlPostingMethod.Items.Add(new ListItem("No data found", "0"));
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    private void DisableControl()       // Finance > General
    {
        ClearTempControl();

        uc_InvExpGL._txtGLAcct.Enabled = false;
        uc_InterestGL._txtGLAcct.Enabled = false;
        txtInvService.Enabled = false;
        txtPrevilWage.Enabled = false;
        ddlPostingMethod.Enabled = false;
        ddlContractType1.Enabled = false;
        chkChargeInt.Enabled = false;
        chkInvoicing.Enabled = false;
        chkChargeable.Enabled = false;
    }
    private void ClearTempControl()     //clear project template field's controls. Finance > General
    {
        uc_InvExpGL._txtGLAcct.Text = "";
        uc_InterestGL._txtGLAcct.Text = "";
        txtInvService.Text = "";
        txtPrevilWage.Text = "";
        ddlPostingMethod.SelectedIndex = 0;
        ddlContractType1.SelectedIndex = 0;
        chkChargeInt.Checked = false;
        chkInvoicing.Checked = false;
        chkChargeable.Checked = false;
    }
    private void EnableControl()       // Finance > General
    {
        ClearTempControl();

        uc_InvExpGL._txtGLAcct.Enabled = true;
        uc_InterestGL._txtGLAcct.Enabled = true;
        txtInvService.Enabled = true;
        txtPrevilWage.Enabled = true;
        //ddlpostingMethod.Enabled = true;
        ddlContractType1.Enabled = true;
        chkChargeInt.Enabled = true;
        chkInvoicing.Enabled = true;
        chkChargeable.Enabled = true;
    }
    #endregion

    #endregion

    #region Custom Field

    protected override void OnInit(EventArgs e)       // comment 8.16.16
    {
        base.OnInit(e);
        // dt = (DataTable)ViewState["CustomFields"];
    }
    //protected override object SaveViewState()
    //{
    //    TrackViewState();
    //    object viewState = base.SaveViewState();
    //    return viewState;
    //}
    protected override void LoadViewState(object savedState)
    {
        TrackViewState();
        base.LoadViewState(savedState);

        if (!ddlTemplate.SelectedValue.Equals("0") && !ddlTemplate.SelectedValue.Equals(""))      // comment 8.16.16
        {
            objJob.ConnConfig = Session["config"].ToString();
            objJob.ID = Convert.ToInt32(ddlTemplate.SelectedValue);

            DataSet ds = new DataSet();
            ds = objBL_Job.GetProjectTemplateCustomFields(objJob);
            DisplayCustomByTab(ds.Tables[0], ds.Tables[1], objJob.ID);
        }
    }
    private void CreateCustomTable()
    {
        dtCustomField = new DataTable();
        dtCustomField.Columns.Add("ID", typeof(int));
        dtCustomField.Columns.Add("tblTabID", typeof(int));
        dtCustomField.Columns.Add("Label", typeof(string));
        dtCustomField.Columns.Add("Line", typeof(Int16));
        dtCustomField.Columns.Add("Value", typeof(string));
        dtCustomField.Columns.Add("Format", typeof(Int16));
        dtCustomField.Columns.Add("ControlID", typeof(string));
        dtCustomField.Columns.Add("UpdatedDate", typeof(DateTime));
        dtCustomField.Columns.Add("Username", typeof(string));
    }
    private string DisplayCustomField(DataTable dtCust, DataTable dtValue, PlaceHolder tbContainer, int isEquipmentTab = 0)
    {
        try
        {
            StringBuilder html = new StringBuilder();
            string varControl = "";
            DataTable dtItem = new DataTable();
            DataTable dtc = dtCust;
            DataTable dtCustomValue = dtValue;

            int rowCount = dtc.Rows.Count;
            int rowcount1 = rowCount / 2;
            int count = 0;

            if (isEquipmentTab != null && isEquipmentTab == 1)
                html.Append("   <div class='col-md-4 col-lg-4'> ");
            else
            {
                if (ViewState["TabId"].ToString().Equals("1"))
                {
                    html.Append("   <div class='col-md-6 col-lg-6'> ");
                }
                else
                {
                    html.Append("   <div class='col-md-6 col-lg-6'> ");
                }
            }
            int i = 0;
            foreach (DataRow drc in dtc.Rows)
            {
                if (rowcount1.Equals(count))
                {
                    html.Append("   </div>                          \n");
                    if (isEquipmentTab != null && isEquipmentTab == 1)
                        html.Append("   <div class='col-md-4 col-lg-4'> \n");
                    else
                    {
                        if (ViewState["TabId"].ToString().Equals("1"))
                        {
                            html.Append("   <div class='col-md-4 col-lg-4'> \n");
                        }
                        else
                        {
                            html.Append("   <div class='col-md-6 col-lg-6'> \n");
                        }
                    }
                }

                string ctrlValue = drc["Value"].ToString();
                string ctrlName = drc["Line"].ToString();
                html.Append(" <div class='form-group'>      \n");
                html.Append("   <div class='form-col'>      \n");
                html.Append("      <div class='fc-label'>   \n");
                html.Append(drc["Label"].ToString());
                html.Append("      </div> \n");
                html.Append("      <div class='fc-input'>   \n");

                StringWriter sw = new StringWriter(html);
                HtmlTextWriter writer = new HtmlTextWriter(sw);

                CustomTextBox txt = new CustomTextBox();

                #region append with control

                switch (Convert.ToInt16(drc["Format"]))
                {
                    case 1:                                 ////////////////////////// Currency

                        txt = new CustomTextBox();
                        txt.ID = "txt" + ctrlName;
                        txt.Text = ctrlValue;
                        txt.CssClass = "form-control custom currency";
                        txt.MaxLength = 14;
                        varControl = "txt" + ctrlName;

                        txt.RenderControl(writer);

                        break;

                    case 2:                                 ////////////////////////// Date
                        txt = new CustomTextBox();

                        txt.ID = "txt" + ctrlName;
                        txt.Text = ctrlValue;
                        txt.CssClass = "form-control custom date-picker";
                        txt.MaxLength = 12;
                        varControl = "txt" + ctrlName;

                        txt.RenderControl(writer);

                        //CalendarExtender ce = new CalendarExtender();
                        //ce.Enabled = true;
                        //ce.TargetControlID = txt.ID;
                        //ce.ID = varControl + "_CalendarExtender";

                        #region comment
                        //writer = new HtmlTextWriter(sw);
                        //sw = new StringWriter(html);

                        //uc_Datepicker ucDate = (uc_Datepicker)LoadControl("uc_Datepicker.ascx");

                        //ucDate.ID = "txt" + ctrlName;
                        //ucDate._txtDate.Text = ctrlValue;
                        //ucDate.RenderControl(writer);

                        #endregion

                        break;

                    case 3:                                 ////////////////////////// Text

                        txt = new CustomTextBox();
                        txt.ID = "txt" + ctrlName;
                        txt.Text = ctrlValue;
                        txt.CssClass = "form-control custom";
                        txt.MaxLength = 50;
                        varControl = "txt" + ctrlName;

                        txt.RenderControl(writer);

                        break;

                    case 4:                                 ////////////////////////// Dropdown

                        CustomDropDownList ddl = new CustomDropDownList();
                        ddl.ID = "ddl" + ctrlName;
                        ddl.CssClass = "form-control custom";
                        ddl.Items.Add("Select");
                        ddl.SelectedValue = "Select";


                        dtItem = new DataTable();
                        var rows = dtCustomValue.AsEnumerable().Where(x => ((Int16)x["Line"]) == Convert.ToInt16(drc["Line"]));
                        if (rows.Any())
                            dtItem = rows.CopyToDataTable();
                        //dtItem = dtCustomValue.Select("Line="+drc["Format"].ToString()).CopyToDataTable();
                        foreach (DataRow drItem in dtItem.Rows)
                        {
                            ddl.Items.Add(drItem["Value"].ToString());
                        }

                        if (drc.ItemArray.Length > 6)
                        {
                            if (!string.IsNullOrEmpty(drc["Value"].ToString()))
                            {
                                ddl.SelectedValue = drc["Value"].ToString();
                            }
                        }
                        varControl = "ddl" + ctrlName;

                        ddl.RenderControl(writer);

                        break;


                    case 5:                                 ////////////////////////// Checkbox
                        CustomCheckBox chk = new CustomCheckBox();
                        chk.CssClass = "custom";
                        chk.ID = "chk" + ctrlName;

                        if (!string.IsNullOrEmpty(drc["Value"].ToString()))
                        {
                            if (drc["Value"].ToString().ToLower().Equals("true"))
                            {
                                chk.Checked = true;
                            }
                            else if (drc["Value"].ToString().ToLower().Equals("on"))
                            {
                                chk.Checked = true;
                            }
                        }
                        varControl = "chk" + ctrlName;

                        chk.RenderControl(writer);

                        if (drc["Value"].ToString().ToLower().Equals("true"))
                        {
                            if (drc["UpdatedDate"] != DBNull.Value && drc["Username"] != DBNull.Value)
                            {
                                string username = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; " + Convert.ToDateTime(drc["UpdatedDate"]).ToString("MM/dd/yy hh:mm tt") + " " + drc["Username"].ToString();
                                Label lbl = new Label();
                                lbl.ID = "custom";
                                lbl.ID = "lbl" + ctrlName;
                                lbl.Text = username;

                                sw = new StringWriter(html);
                                writer = new HtmlTextWriter(sw);
                                lbl.RenderControl(writer);
                            }
                        }
                        break;

                }

                DataRow dr = dtCustomField.NewRow();
                dr["ID"] = Convert.ToInt32(drc["ID"]);
                dr["tblTabID"] = Convert.ToInt32(drc["tblTabID"]);
                dr["Line"] = Convert.ToInt16(drc["Line"]);
                dr["Label"] = drc["Label"].ToString();
                dr["Value"] = drc["Value"].ToString();
                dr["Format"] = Convert.ToInt16(drc["Format"]);
                dr["ControlID"] = varControl;

                dtCustomField.Rows.Add(dr);

                #endregion

                html.Append("      </div>                        \n");
                html.Append("   </div>                           \n");
                html.Append(" </div>                             \n");

                count++;
                i++;
            }
            html.Append("   </div>                               \n");

            ViewState["CustomFields"] = dtCust;
            return html.ToString();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    private void DisplayCustomByTab(DataTable dtCustom, DataTable dtVal, int jobId)
    {
        try
        {
            DataTable dtCustomTab = new DataTable();
            DataTable dtItem = new DataTable();
            //fetch all custom field from tblCustomTab and tblCustom
            DataTable dtCust = new DataTable();
            DataTable dtValue = new DataTable();

            DataTable dtc = dtCustom;
            DataTable dtv = dtVal;
            objJob.ConnConfig = Session["config"].ToString();
            objJob.PageUrl = "addproject.aspx";

            DataSet _ds = objBL_Job.GetTabByPageUrl(objJob);

            objJob.ConnConfig = Session["config"].ToString();
            objJob.ID = jobId;
            DataSet dsTab = objBL_Job.GetProjectCustomTab(objJob);
            string html = string.Empty;
            ViewState["TabId"] = 0;
            foreach (DataRow drCus in dsTab.Tables[0].Rows)
            {
                ViewState["TabId"] = 0;
                html = string.Empty;
                switch (drCus["tblTabID"].ToString())
                {
                    case "1":                           //  Header

                        dtCust = new DataTable();
                        dtValue = new DataTable();
                        if (dtCustom.Rows.Count > 0)
                        {
                            var rowsdt = dtCustom.AsEnumerable().Where(x => ((int)x["tblTabID"]) == 1);
                            if (rowsdt.Any())
                            {
                                dtCust = rowsdt.CopyToDataTable();

                                var rows = dtVal.AsEnumerable().Where(x => ((int)x["tblTabID"]) == 1);
                                if (rows.Any())
                                {
                                    dtValue = rows.CopyToDataTable();
                                }
                            }
                        }
                        ViewState["TabId"] = 1;

                        html = DisplayCustomField(dtCust, dtValue, PlaceHolderHeader);


                        PlaceHolderHeader.Controls.Add(new Literal { Text = html.ToString() });

                        break;
                    case "2":                           //  Attributes - General

                        dtCust = new DataTable();
                        dtValue = new DataTable();
                        if (dtCustom.Rows.Count > 0)
                        {
                            var rowsdt = dtCustom.AsEnumerable().Where(x => ((int)x["tblTabID"]) == 2);
                            if (rowsdt.Any())
                            {
                                dtCust = rowsdt.CopyToDataTable();

                                var rows = dtVal.AsEnumerable().Where(x => ((int)x["tblTabID"]) == 2);
                                if (rows.Any())
                                {
                                    dtValue = rows.CopyToDataTable();
                                }
                            }
                        }

                        html = DisplayCustomField(dtCust, dtValue, PlaceHolderAttrGeneral);
                        PlaceHolderAttrGeneral.Controls.Add(new Literal { Text = html.ToString() });

                        break;
                    case "3":                           //  Attributes - GC Info

                        dtCust = new DataTable();
                        dtValue = new DataTable();

                        if (dtCustom.Rows.Count > 0)
                        {
                            var rowsdt = dtCustom.AsEnumerable().Where(x => ((int)x["tblTabID"]) == 3);
                            if (rowsdt.Any())
                            {
                                dtCust = rowsdt.CopyToDataTable();

                                var rows = dtVal.AsEnumerable().Where(x => ((int)x["tblTabID"]) == 3);
                                if (rows.Any())
                                {
                                    dtValue = rows.CopyToDataTable();
                                }
                            }
                        }

                        html = DisplayCustomField(dtCust, dtValue, PlaceHolderAttriGC);
                        PlaceHolderAttriGC.Controls.Add(new Literal { Text = html.ToString() });

                        break;
                    case "4":                           //  Attributes - Equipment

                        dtCust = new DataTable();
                        dtValue = new DataTable();
                        if (dtCustom.Rows.Count > 0)
                        {
                            var rowsdt = dtCustom.AsEnumerable().Where(x => ((int)x["tblTabID"]) == 4);
                            if (rowsdt.Any())
                            {
                                dtCust = rowsdt.CopyToDataTable();

                                var rows = dtVal.AsEnumerable().Where(x => ((int)x["tblTabID"]) == 4);
                                if (rows.Any())
                                {
                                    dtValue = rows.CopyToDataTable();
                                }
                            }
                        }

                        html = DisplayCustomField(dtCust, dtValue, PlaceHolderAttriEquip, 1);
                        PlaceHolderAttriEquip.Controls.Add(new Literal { Text = html.ToString() });

                        break;
                    case "5":                           //  Finance - General

                        dtCust = new DataTable();
                        dtValue = new DataTable();
                        if (dtCustom.Rows.Count > 0)
                        {
                            var rowsdt = dtCustom.AsEnumerable().Where(x => ((int)x["tblTabID"]) == 5);
                            if (rowsdt.Any())
                            {
                                dtCust = rowsdt.CopyToDataTable();

                                var rows = dtVal.AsEnumerable().Where(x => ((int)x["tblTabID"]) == 5);
                                if (rows.Any())
                                {
                                    dtValue = rows.CopyToDataTable();
                                }
                            }
                        }

                        html = DisplayCustomField(dtCust, dtValue, PlaceHolderFinceGeneral);
                        PlaceHolderFinceGeneral.Controls.Add(new Literal { Text = html.ToString() });

                        break;
                    case "6":                           //  Finance - Billing

                        dtCust = new DataTable();
                        dtValue = new DataTable();
                        if (dtCustom.Rows.Count > 0)
                        {
                            var rowsdt = dtCustom.AsEnumerable().Where(x => ((int)x["tblTabID"]) == 6);
                            if (rowsdt.Any())
                            {
                                dtCust = rowsdt.CopyToDataTable();

                                var rows = dtVal.AsEnumerable().Where(x => ((int)x["tblTabID"]) == 6);
                                if (rows.Any())
                                {
                                    dtValue = rows.CopyToDataTable();
                                }
                            }
                        }

                        html = DisplayCustomField(dtCust, dtValue, PlaceHolderFinceBill);
                        PlaceHolderFinceBill.Controls.Add(new Literal { Text = html.ToString() });

                        break;
                    case "7":                           //  Finance - Budgets

                        dtCust = new DataTable();
                        dtValue = new DataTable();
                        if (dtCustom.Rows.Count > 0)
                        {
                            var rowsdt = dtCustom.AsEnumerable().Where(x => ((int)x["tblTabID"]) == 7);
                            if (rowsdt.Any())
                            {
                                dtCust = rowsdt.CopyToDataTable();
                                var rows = dtVal.AsEnumerable().Where(x => ((int)x["tblTabID"]) == 7);
                                if (rows.Any())
                                {
                                    dtValue = rows.CopyToDataTable();
                                }
                            }
                        }

                        html = DisplayCustomField(dtCust, dtValue, PlaceHolderFinceBudget);
                        PlaceHolderFinceBudget.Controls.Add(new Literal { Text = html.ToString() });

                        break;
                    case "8":                           //  Ticketlist

                        dtCust = new DataTable();
                        dtValue = new DataTable();
                        if (dtCustom.Rows.Count > 0)
                        {
                            var rowsdt = dtCustom.AsEnumerable().Where(x => ((int)x["tblTabID"]) == 8);
                            if (rowsdt.Any())
                            {
                                dtCust = rowsdt.CopyToDataTable();

                                var rows = dtVal.AsEnumerable().Where(x => ((int)x["tblTabID"]) == 8);
                                if (rows.Any())
                                {
                                    dtValue = rows.CopyToDataTable();
                                }
                            }
                        }

                        html = DisplayCustomField(dtCust, dtValue, PlaceHolderTicket);
                        PlaceHolderTicket.Controls.Add(new Literal { Text = html.ToString() });

                        break;
                    case "9":                           //  BOM

                        dtCust = new DataTable();
                        dtValue = new DataTable();
                        if (dtCustom.Rows.Count > 0)
                        {
                            var rowsdt = dtCustom.AsEnumerable().Where(x => ((int)x["tblTabID"]) == 9);
                            if (rowsdt.Any())
                            {
                                dtCust = rowsdt.CopyToDataTable();
                                var rows = dtVal.AsEnumerable().Where(x => ((int)x["tblTabID"]) == 9);
                                if (rows.Any())
                                {
                                    dtValue = rows.CopyToDataTable();
                                }

                            }
                        }

                        html = DisplayCustomField(dtCust, dtValue, PlaceHolderBOM);
                        PlaceHolderBOM.Controls.Add(new Literal { Text = html.ToString() });

                        break;
                    case "10":                           //  Milestones

                        dtCust = new DataTable();
                        dtValue = new DataTable();
                        if (dtCustom.Rows.Count > 0)
                        {
                            var rowsdt = dtCustom.AsEnumerable().Where(x => ((int)x["tblTabID"]) == 10);
                            if (rowsdt.Any())
                            {
                                dtCust = rowsdt.CopyToDataTable();
                                var rows = dtVal.AsEnumerable().Where(x => ((int)x["tblTabID"]) == 10);
                                if (rows.Any())
                                {
                                    dtValue = rows.CopyToDataTable();
                                }
                            }
                        }

                        html = DisplayCustomField(dtCust, dtValue, PlaceHolderMilestone);
                        PlaceHolderMilestone.Controls.Add(new Literal { Text = html.ToString() });

                        break;

                    case "11":                           //  Task

                        dtCust = new DataTable();
                        dtValue = new DataTable();
                        if (dtCustom.Rows.Count > 0)
                        {
                            var rowsdt = dtCustom.AsEnumerable().Where(x => ((int)x["tblTabID"]) == 11);
                            if (rowsdt.Any())
                            {
                                dtCust = rowsdt.CopyToDataTable();

                                var rows = dtVal.AsEnumerable().Where(x => ((int)x["tblTabID"]) == 11);
                                if (rows.Any())
                                {
                                    dtValue = rows.CopyToDataTable();
                                }
                            }
                        }

                        html = DisplayCustomField(dtCust, dtValue, TaskPlaceholder);
                        TaskPlaceholder.Controls.Add(new Literal { Text = html.ToString() });
                        break;
                }
            }
            ViewState["CustomFields"] = dtCustomField;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    private DataTable GetCustomItems()
    {
        string strItems = hdnCustomJSON.Value;
        DataTable dt = (DataTable)ViewState["CustomFields"];
        try
        {
            if (strItems != string.Empty)
            {
                JavaScriptSerializer sr = new JavaScriptSerializer();
                List<Dictionary<object, object>> objItemData = new List<Dictionary<object, object>>();
                objItemData = sr.Deserialize<List<Dictionary<object, object>>>(strItems);
                if (objItemData.Count > 0)
                {
                    DataColumnCollection dt_col = dt.Columns;
                    if (dt_col.Contains("ControlID"))
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            var customDs = objItemData.Where(y => y.Keys.Contains(dr["ControlID"].ToString())).FirstOrDefault();
                            if (customDs != null)
                            {
                                if (customDs.Count > 0)
                                {
                                    dr["Value"] = customDs.Values.First();
                                }
                            }
                        }
                    }
                    //dt.Columns.Remove("ControlID");
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

    protected void lnkNext_Click(object sender, EventArgs e)
    {
        DataTable dt = (DataTable)Session["projids"];
        DataColumn[] keyColumns = new DataColumn[1];
        keyColumns[0] = dt.Columns["ID"];
        dt.PrimaryKey = keyColumns;

        DataRow d = dt.Rows.Find(Request.QueryString["uid"].ToString());
        int index = dt.Rows.IndexOf(d);
        int c = dt.Rows.Count - 1;
        if (index < c)
        {
            Response.Redirect("addproject.aspx?uid=" + dt.Rows[index + 1]["id"], false);
        }
    }
    protected void lnkPrevious_Click(object sender, EventArgs e)
    {
        DataTable dt = (DataTable)Session["projids"];
        DataColumn[] keyColumns = new DataColumn[1];
        keyColumns[0] = dt.Columns["ID"];
        dt.PrimaryKey = keyColumns;

        DataRow d = dt.Rows.Find(Request.QueryString["uid"].ToString());
        int index = dt.Rows.IndexOf(d);

        if (index > 0)
        {
            Response.Redirect("addproject.aspx?uid=" + dt.Rows[index - 1]["id"], false);
        }
    }
    protected void lnkLast_Click(object sender, EventArgs e)
    {
        DataTable dt = (DataTable)Session["projids"];
        Response.Redirect("addproject.aspx?uid=" + dt.Rows[dt.Rows.Count - 1]["id"], false);
    }
    protected void lnkFirst_Click(object sender, EventArgs e)
    {
        DataTable dt = (DataTable)Session["projids"];
        Response.Redirect("addproject.aspx?uid=" + dt.Rows[0]["id"], false);
    }

    protected void rtEquips_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            Repeater rtEquipCustom = e.Item.FindControl("rtEquipCustom") as Repeater;
            Label lblID = e.Item.FindControl("lblID") as Label;
            System.Web.UI.WebControls.Image imgPlus = e.Item.FindControl("imgPlus") as System.Web.UI.WebControls.Image;
            DataTable dt = getequipCustomItemsByElevID(Convert.ToInt32(lblID.Text));
            if (dt.Rows.Count > 0)
                imgPlus.Visible = true;
            else
                imgPlus.Visible = false;
            rtEquipCustom.DataSource = dt;
            rtEquipCustom.DataBind();
        }
    }

    private DataTable getequipCustomItemsByElevID(int equipid)
    {
        objPropUser.ConnConfig = Session["config"].ToString();
        objPropUser.EquipID = equipid;
        DataSet ds = new DataSet();
        ds = objBL_User.getequipCustomItemsByElevID(objPropUser);
        return ds.Tables[0];
    }
    protected void lnkEditEq_Click(object sender, EventArgs e)
    {
        foreach (RepeaterItem di in rtEquips.Items)
        {
            CheckBox chkSelected = (CheckBox)di.FindControl("chkSelect");
            Label lblUserID = (Label)di.FindControl("lblId");

            if (chkSelected.Checked == true)
            {
                Response.Redirect("addequipment.aspx?uid=" + lblUserID.Text);
            }
        }
    }
    protected void btnCopyEQ_Click(object sender, EventArgs e)
    {
        foreach (RepeaterItem di in rtEquips.Items)
        {
            CheckBox chkSelected = (CheckBox)di.FindControl("chkSelect");
            Label lblUserID = (Label)di.FindControl("lblId");

            if (chkSelected.Checked == true)
            {
                Response.Redirect("addequipment.aspx?t=c&uid=" + lblUserID.Text);
            }
        }
    }
    protected void lnkAddEQ_Click(object sender, EventArgs e)
    {
        string url = "addequipment.aspx";
        if (txtLocation.Text.Trim() != null & hdnLocID.Value.Trim() != null)
        {
            url += "?lid=" + hdnLocID.Value.Trim() + "&locname=" + txtLocation.Text.Trim();
        }
        Response.Redirect(url);
    }

    private void GetAttachment()
    {

        List<ListItem> lsttypes = new List<ListItem>();
        lsttypes.Add(new ListItem("Project", "Project"));
        lsttypes.Add(new ListItem("Tickets", "Tickets"));
        lsttypes.Add(new ListItem("Customer", "Customer"));
        lsttypes.Add(new ListItem("Location", "Location"));

        rptattachmenttype.DataSource = lsttypes;
        rptattachmenttype.DataBind();

    }

    private DataTable GetAttachmentByTypes(string type)
    {
        DataSet ds = new DataSet();
        objJob.Job = Convert.ToInt32(Request.QueryString["uid"] == null ? "0" : Request.QueryString["uid"].ToString());
        objJob.TypeName = type;
        objJob.sort = Convert.ToInt16(ddlSortAttachment.SelectedValue);
        ds = objBL_Job.GetAttachment(objJob);

        //List<ListItem> files = new List<ListItem>();
        DataTable dtAttach = new DataTable();
        dtAttach.Columns.Add("Text");
        dtAttach.Columns.Add("value");
        dtAttach.Columns.Add("ID");
        dtAttach.Columns.Add("content");
        dtAttach.Columns.Add("path");
        dtAttach.Columns.Add("msvisible");
        dtAttach.Columns.Add("thumb");
        dtAttach.Columns.Add("Screen");
        if (ds.Tables.Count > 0)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DataRow dr = dtAttach.NewRow();

                    bool exists = false;
                    string filename = ds.Tables[0].Rows[i]["Filename"] != DBNull.Value ? Convert.ToString(ds.Tables[0].Rows[i]["Filename"]) : string.Empty;
                    string localPath = ds.Tables[0].Rows[i]["Path"] != DBNull.Value ? Convert.ToString(ds.Tables[0].Rows[i]["Path"]).Replace(filename, "") : string.Empty;
                    if (Directory.Exists(localPath))
                    {
                        if (File.Exists(localPath + filename))
                        {
                            Byte[] bytes = null;
                            string contenttype = MimeMapping.GetMimeMapping(filename);
                            string content = contenttype.Split('/')[0].ToString().ToLower();
                            if (content != "image")
                            {
                                System.Drawing.Icon iconForFile = System.Drawing.SystemIcons.WinLogo;
                                iconForFile = System.Drawing.Icon.ExtractAssociatedIcon(localPath + filename);
                                System.Drawing.ImageConverter converter = new System.Drawing.ImageConverter();
                                bytes = (byte[])converter.ConvertTo(iconForFile.ToBitmap(), typeof(byte[]));
                                contenttype = "image/jpg";
                            }
                            else
                            {
                                //FileStream fs = new FileStream(localPath + filename, FileMode.Open, FileAccess.Read);
                                //BinaryReader br = new BinaryReader(fs);
                                //bytes = br.ReadBytes((Int32)fs.Length);
                                using (var ms = new MemoryStream())
                                {
                                    System.Drawing.Image image = System.Drawing.Image.FromFile(localPath + filename);
                                    Size thumbnailSize = GetThumbnailSize(image);
                                    Bitmap bitmap = CreateThumbnail(localPath + filename, thumbnailSize.Width, thumbnailSize.Height);
                                    bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                                    bytes = ms.ToArray();
                                }
                            }

                            string base64String = System.Convert.ToBase64String(bytes, 0, bytes.Length);


                            dr["Text"] = filename;
                            dr["ID"] = ds.Tables[0].Rows[i]["id"].ToString();
                            dr["content"] = content;
                            dr["path"] = localPath + filename;
                            dr["msvisible"] = Convert.ToBoolean(ds.Tables[0].Rows[i]["MSVisible"].ToString());
                            dr["value"] = "data:" + contenttype + ";base64," + base64String;
                            dr["thumb"] = "AttachmentImage.ashx?thumb=1&docid=" + ds.Tables[0].Rows[i]["id"].ToString();
                            dr["Screen"] = ds.Tables[0].Rows[i]["Screen"].ToString();
                            dtAttach.Rows.Add(dr);
                            exists = true;
                        }
                    }

                    if (!exists)
                    {
                        Byte[] bytes = null;
                        FileStream fs = new FileStream(Server.MapPath(@"~\images\NotFound.png"), FileMode.Open, FileAccess.Read);
                        BinaryReader br = new BinaryReader(fs);
                        bytes = br.ReadBytes((Int32)fs.Length);
                        string base64String;
                        base64String = System.Convert.ToBase64String(bytes, 0, bytes.Length);
                        //files.Add(new ListItem(localPath + filename, "data:" + string.Empty + ";base64," + base64String));
                        dr["Text"] = filename;
                        dr["ID"] = ds.Tables[0].Rows[i]["id"].ToString();
                        dr["content"] = "none";
                        dr["path"] = localPath + filename;
                        dr["msvisible"] = Convert.ToBoolean(ds.Tables[0].Rows[i]["MSVisible"].ToString());
                        dr["value"] = "data:image/png;base64," + base64String;
                        dtAttach.Rows.Add(dr);
                    }

                }
            }
        }


        return dtAttach;
    }

    protected void rptattachment_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "OpenAttachment")
        {
            try
            {
                string filePath = e.CommandArgument.ToString();
                System.IO.FileInfo FileName = new System.IO.FileInfo(filePath);
                FileStream myFile = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                BinaryReader _BinaryReader = new BinaryReader(myFile);
                try
                {
                    long startBytes = 0;
                    string lastUpdateTiemStamp = File.GetLastWriteTimeUtc(filePath).ToString("r");
                    string _EncodedData = HttpUtility.UrlEncode(myFile.Name, Encoding.UTF8) + lastUpdateTiemStamp;

                    Response.Clear();
                    Response.Buffer = false;
                    Response.AddHeader("Accept-Ranges", "bytes");
                    Response.AppendHeader("ETag", "\"" + _EncodedData + "\"");
                    Response.AppendHeader("Last-Modified", lastUpdateTiemStamp);
                    Response.ContentType = "application/octet-stream";
                    Response.AddHeader("Content-Disposition", "inline;filename=" + HttpUtility.UrlEncode(FileName.Name));
                    Response.AddHeader("Content-Length", (FileName.Length - startBytes).ToString());
                    Response.AddHeader("Connection", "Keep-Alive");
                    Response.ContentEncoding = Encoding.UTF8;

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
        else if (e.CommandName == "UpdateMS")
        {
            int docID = Convert.ToInt32(e.CommandArgument.ToString());
            objPropUser.ConnConfig = Session["config"].ToString();
            objPropUser.DocID = docID;
            objBL_User.UpdateDoc(objPropUser);
        }
        else if (e.CommandName == "DeleteAttachment")
        {

            int docID = Convert.ToInt32(e.CommandArgument.ToString());
            objMapData.ConnConfig = Session["config"].ToString();
            objMapData.DocumentID = docID;
            objBL_MapData.DeleteFile(objMapData);
            BindAttachmentGrid();
        }
        else if (e.CommandName == "RotatedImgright")
        {
            // get the full path of image url
            string path = e.CommandArgument.ToString();//Server.MapPath(Image1.ImageUrl);

            // creating image from the image url
            System.Drawing.Image i = System.Drawing.Image.FromFile(path);

            // rotate Image 90' Degree
            i.RotateFlip(RotateFlipType.Rotate270FlipXY);

            // save it to its actual path
            i.Save(path);

            // release Image File
            i.Dispose();

            GetAttachment();

        }
        else if (e.CommandName == "RotatedImgleft")
        {
            // get the full path of image url
            string path = e.CommandArgument.ToString();//Server.MapPath(Image1.ImageUrl);

            // creating image from the image url
            System.Drawing.Image i = System.Drawing.Image.FromFile(path);

            // rotate Image 90' Degree
            i.RotateFlip(RotateFlipType.Rotate270FlipNone);

            // save it to its actual path
            i.Save(path);

            // release Image File
            i.Dispose();

            GetAttachment();
        }
    }
    protected void rptattachmenttype_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            string hdntype = (e.Item.FindControl("hdntype") as HiddenField).Value;
            Repeater rptattachment = e.Item.FindControl("rptattachment") as Repeater;

            //HiddenField hdnpages = e.Item.FindControl("hdnpages") as HiddenField;
            //HiddenField hdnpagescount = e.Item.FindControl("hdnpagescount") as HiddenField;
            //LinkButton lnkprevious = e.Item.FindControl("lnkprevious") as LinkButton;
            //LinkButton lnknext = e.Item.FindControl("lnknext") as LinkButton;
            //lnknext.Enabled = true;
            //lnkprevious.Enabled = true;

            //rptattachment.DataSource = Paginate(GetAttachmentByTypes(hdntype), 1, hdnpages, hdnpagescount);
            rptattachment.DataSource = GetAttachmentByTypes(hdntype);
            rptattachment.DataBind();

            //if (Convert.ToInt32(hdnpages.Value) == Convert.ToInt32(hdnpagescount.Value) || Convert.ToInt32(hdnpagescount.Value) <= 0)
            //{

            //    lnknext.Enabled = false;
            //}
            //if (Convert.ToInt32(hdnpages.Value) <= 1)
            //    lnkprevious.Enabled = false;
        }
    }
    protected void ddlAttachment_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindAttachmentGrid();
        if (ddlAttachment.SelectedValue == "All" || ddlAttachment.SelectedValue == "Project")
        {
            fuspan.Visible = FU_Project.Visible = true;
        }
        else
        {
            fuspan.Visible = FU_Project.Visible = false;
        }
    }

    private void BindAttachmentGrid()
    {
        List<ListItem> lsttypes = new List<ListItem>();
        if (ddlAttachment.SelectedValue == "All")
        {
            lsttypes.Add(new ListItem("Project", "Project"));
            lsttypes.Add(new ListItem("Tickets", "Tickets"));
            lsttypes.Add(new ListItem("Customer", "Customer"));
            lsttypes.Add(new ListItem("Location", "Location"));

        }
        else
        {
            lsttypes.Add(new ListItem(ddlAttachment.SelectedValue, ddlAttachment.SelectedValue));
        }

        rptattachmenttype.DataSource = lsttypes;
        rptattachmenttype.DataBind();
    }
    private List<ListItem> Paginate(List<ListItem> items, int page, HiddenField hdnpages, HiddenField hdnpagescount)
    {
        int rows = 2;
        int cols = 4;
        int total = rows * cols;

        int noofrecordsperpage = total;
        int pagecount = 1;
        pagecount = (items.Count + noofrecordsperpage - 1) / noofrecordsperpage;

        //if (page > pagecount)
        //    page = Convert.ToInt32(lblpages.Text);

        int MaxRecord = (noofrecordsperpage * Convert.ToInt32(page)) - 1;

        int MinRecord = (MaxRecord - noofrecordsperpage) < 0 ? 0 : (MaxRecord - noofrecordsperpage) + 1;



        List<ListItem> cuurentlist = new List<ListItem>();

        for (int i = 0; i < items.Count; i++)
        {
            //Check If the records found are less than the no of reords that needs to be displayed in the gird
            if (pagecount > 0)
            {
                if (i >= MinRecord && i <= MaxRecord)
                    cuurentlist.Add(items[i]);
            }
            else
                cuurentlist.Add(items[i]);
        }

        if (hdnpages != null)
        {
            hdnpages.Value = page.ToString();
        }
        if (hdnpagescount != null)
        {
            hdnpagescount.Value = pagecount.ToString();
        }
        return cuurentlist;

    }

    //protected void rptattachmenttype_ItemCommand(object source, RepeaterCommandEventArgs e)
    //{

    //    string hdntype = string.Empty;
    //    Repeater rptattachment = null;
    //    HiddenField hdnpages = null;
    //    HiddenField hdnpagescount = null;
    //    LinkButton lnkprevious = null;
    //    LinkButton lnknext = null;

    //    if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
    //    {
    //        hdntype = (e.Item.FindControl("hdntype") as HiddenField).Value;
    //        rptattachment = e.Item.FindControl("rptattachment") as Repeater;
    //        hdnpages = e.Item.FindControl("hdnpages") as HiddenField;
    //        hdnpagescount = e.Item.FindControl("hdnpagescount") as HiddenField;
    //        lnkprevious = e.Item.FindControl("lnkprevious") as LinkButton;
    //        lnknext = e.Item.FindControl("lnknext") as LinkButton;
    //        lnknext.Enabled = true;
    //        lnkprevious.Enabled = true;
    //    }


    //    if (e.CommandName == "Previous")
    //    {
    //        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
    //        {

    //            int i = Convert.ToInt32(hdnpages.Value) - 1;
    //            rptattachment.DataSource = Paginate(GetAttachmentByTypes(hdntype), i, hdnpages, hdnpagescount);
    //            rptattachment.DataBind();


    //        }

    //    }
    //    if (e.CommandName == "Next")
    //    {

    //        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
    //        {

    //            int i = Convert.ToInt32(hdnpages.Value) + 1;
    //            rptattachment.DataSource = Paginate(GetAttachmentByTypes(hdntype), i, hdnpages, hdnpagescount);
    //            rptattachment.DataBind();
    //        }

    //    }


    //    if (Convert.ToInt32(hdnpages.Value) == Convert.ToInt32(hdnpagescount.Value) || Convert.ToInt32(hdnpagescount.Value) <= 0)
    //    {

    //        lnknext.Enabled = false;
    //    }
    //    if (Convert.ToInt32(hdnpages.Value) <= 1)
    //        lnkprevious.Enabled = false;
    //}

    protected void rptTicketTask_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            Repeater rtEquipCustom = e.Item.FindControl("rtTasks") as Repeater;
            Label lblID = e.Item.FindControl("lblID") as Label;
            DataTable dt = getTicketTaskByTicketID(Convert.ToInt32(lblID.Text));
            rtEquipCustom.DataSource = dt;
            rtEquipCustom.DataBind();
        }
    }

    private DataTable getTicketTaskByTicketID(int TicketID)
    {
        objMapData.ConnConfig = Session["config"].ToString();
        objMapData.TicketID = TicketID;
        DataSet ds = objBL_MapData.getTicketTaskByTicketID(objMapData);
        return ds.Tables[0];
    }

    private void GetControl()
    {
        DataSet ds = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();
        ds = objBL_User.getControl(objPropUser);
        if (ds.Tables[0].Rows.Count > 0)
        {
            bool tasks = Convert.ToBoolean(ds.Tables[0].Rows[0]["TaskCode"]);
            ViewState["tasks"] = tasks;
            tbpnlTicketTask.Visible = tasks;
            liaccrdTicketTask.Visible = tasks;

            int codes = 0;
            if (ds.Tables[0].Rows[0]["codes"] != DBNull.Value)
                codes = Convert.ToInt16(ds.Tables[0].Rows[0]["codes"]);

            if (codes == 0)
            {
                tbpnlTicketTask.Visible = false;
                liaccrdTicketTask.Visible = false;
                ViewState["tasks"] = false;
                rfvTaskCategory.Enabled = false;
            }
            else if (codes == 1)
            {
                tbpnlTicketTask.Visible = true;
                liaccrdTicketTask.Visible = true;
                ViewState["tasks"] = false;
                pnlCodes.Visible = true;
                rfvTaskCategory.Enabled = true;
            }
            else if (codes == 2)
            {
                tbpnlTicketTask.Visible = true;
                liaccrdTicketTask.Visible = true;
                ViewState["tasks"] = true;
                pnlCodes.Visible = false;
                rfvTaskCategory.Enabled = false;
            }

            //Show TabPaneHomeowner
            //TabPaneHomeowner.Visible = Convert.ToBoolean(ds.Tables[0].Rows[0]["ISshowHomeowner"]);
            TabPaneHomeowner.Visible = Convert.ToBoolean(ds.Tables[0].Rows[0]["ISshowHomeowner"] == DBNull.Value ? 0 : ds.Tables[0].Rows[0]["ISshowHomeowner"]);
            //added hidden value to execute the autocomplete for homeowner fields only if homeowner is visible 
            hdnIsHO.Value = Convert.ToBoolean(ds.Tables[0].Rows[0]["ISshowHomeowner"] == DBNull.Value ? 0 : ds.Tables[0].Rows[0]["ISshowHomeowner"]).ToString();


        }
    }
    private void FillCategory()
    {
        DataSet ds = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();
        ds = objBL_User.getCategory(objPropUser);
        ddlCategory.DataSource = ds.Tables[0];
        ddlCategory.DataTextField = "type";
        ddlCategory.DataValueField = "type";
        ddlCategory.DataBind();

        ddlCategory.Items.Insert(0, new ListItem(":: Select ::", ""));
        ddlCategory.Items.Insert(1, new ListItem("None", "None"));
    }
    private void FillWorker()
    {
        DataSet ds = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();
        objPropUser.Status = 0;
        objPropUser.Username = string.Empty;
        ds = objBL_User.getEMP(objPropUser);
        chkcatlist.DataSource = ds.Tables[0];
        chkcatlist.DataTextField = "fDesc";
        chkcatlist.DataValueField = "fDesc";
        chkcatlist.DataBind();
    }
    protected void lnkSaveTicket_Click(object sender, EventArgs e)
    {
        try
        {
            objMapData.ConnConfig = Session["config"].ToString();
            objMapData.Worker = txtWorkers.Text.Trim();
            if (txtDays.Text.Trim() != string.Empty)
                objMapData.days = Convert.ToInt16(txtDays.Text.Trim());
            objMapData.jobid = Convert.ToInt32(Request.QueryString["uid"].ToString());
            objMapData.CallDate = DateTime.Now;
            objMapData.SchDate = Convert.ToDateTime(txtSchDate.Text + " " + txtSchTime.Text);
            objMapData.Category = ddlCategory.SelectedValue;
            objMapData.Reason = txtReason.Text;
            objMapData.Who = Session["username"].ToString();
            objMapData.Status = (chkHold.Checked) ? 5 : ((txtWorkers.Text.Trim() != string.Empty) ? 1 : 0);
            objMapData.DispAlert = Convert.ToInt16(chkDispAlert.Checked);
            if (txtEST.Text.Trim() != string.Empty)
                objMapData.EST = Convert.ToDouble(txtEST.Text);
            else
                objMapData.EST = 00.50;
            objMapData.CreditReason = txtCreditReason.Text;
            if (hdnUnitID.Value != "")
            {
                objMapData.Unit = Convert.ToInt32(hdnUnitID.Value);
            }
            objMapData.dtEquips = GetElevData();
            string TicketID = objBL_MapData.AddTicketFromProject(objMapData);
            GetOpenCalls(string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyTicket", "noty({text: 'Ticket(s) Added Successfully!', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
            int WorkerCount = 0;
            foreach (ListItem item in chkcatlist.Items)
            {
                if (item.Selected) { WorkerCount += 1; }
            }
            if (txtDays.Text.Trim() == "1" && WorkerCount == 1)
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyNotyConfirm", "notyConfirm('" + TicketID + "');", true);
            }
            objgn.ResetFormControlValues(panelTicketAdd);
            foreach (ListItem item in chkcatlist.Items)
            {
                item.Selected = false;
            }

        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }

    }

    protected void gvTickets_Sorting(object sender, GridViewSortEventArgs e)
    {
        string sortExpression = e.SortExpression;
        String _OrderBy = "";
        if (hdTicketListOrderBy.Value == ASCENDING)
        {
            _OrderBy = ASCENDING;
        }
        else
        {
            _OrderBy = Convert.ToString(hdTicketListOrderBy.Value);
        }

        if (_OrderBy == ASCENDING)
        {
            String OrderBy = sortExpression + " " + DESCENDING;
            hdTicketListOrderBy.Value = DESCENDING;
            BindTicketList(OrderBy, 1);

        }
        else
        {
            String OrderBy = sortExpression + " " + ASCENDING;
            hdTicketListOrderBy.Value = ASCENDING;
            BindTicketList(OrderBy, 1);
        }


    }

    public SortDirection GridViewSortDirection
    {
        get
        {
            if (ViewState["sortDirection"] == null)
                ViewState["sortDirection"] = SortDirection.Ascending;

            return (SortDirection)ViewState["sortDirection"];
        }
        set { ViewState["sortDirection"] = value; }
    }

    //protected void ddlMatItem_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        DropDownList ddlMatItem = (DropDownList)sender;
    //        GridViewRow row = (GridViewRow)ddlMatItem.NamingContainer;
    //        TextBox txtScope = (TextBox)row.FindControl("txtScope");

    //        objJob.ConnConfig = Session["config"].ToString();
    //        DataSet ds = objBL_Job.GetInventoryItem(objJob);

    //        DataRow dr = ds.Tables[0].Select("Value =" + ddlMatItem.SelectedValue).FirstOrDefault();
    //        if (dr != null)
    //        {
    //            txtScope.Text = dr["fDesc"].ToString();
    //        }
    //        else
    //        {
    //            txtScope.Text = "";
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
    //        ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
    //    }
    //}


    #region Ticket List Commented Code

    //private void SortGridView(string sortExpression, string direction)
    //{
    //    ViewState["sortexp"] = sortExpression + direction;
    //    DataTable dt = PageSortData(sortExpression + direction);
    //    gvTickets.DataSource = dt;
    //    gvTickets.DataBind();
    //    CalculateBalance(dt);
    //    lblTicketCount.Text = dt.Rows.Count + " ticket(s) found.";
    //}

    //private DataTable PageSortData(string sortExpression)
    //{
    //    DataTable dt = new DataTable();
    //    dt = GetOpenCalls(sortExpression);
    //    return dt;
    //}

    //private void FillGridPaged()
    //{
    //    DataTable dt = new DataTable();
    //    string sort = string.Empty;
    //    if (ViewState["sortexp"] != null)
    //        sort = ViewState["sortexp"].ToString();

    //    dt = PageSortData(sort);
    //    gvTickets.DataSource = dt;
    //    gvTickets.DataBind();
    //    CalculateBalance(dt);
    //    lblTicketCount.Text = dt.Rows.Count + " ticket(s) found.";
    //}
    #endregion


    //protected void Page_PreRender(Object o, EventArgs e)
    //{
    //    foreach (GridViewRow gr in gvTickets.Rows)
    //    {
    //        Label lblRes = (Label)gr.FindControl("lblRes");
    //        gr.Attributes["onmousemove"] = "HoverMenutext('" + gr.ClientID + "','" + lblRes.ClientID + "',event);";
    //        gr.Attributes["onmouseout"] = " $('#" + lblRes.ClientID + "').hide();";
    //    }
    //}


    protected void Page_PreRender(Object o, EventArgs e)
    {
        foreach (GridDataItem gr in gvEquip.Items)
        {
            CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");
            chkSelect.Attributes["onclick"] = "SelectRows('" + gvEquip.ClientID + "','" + txtUnit.ClientID + "','" + hdnUnitID.ClientID + "');";
        }
        foreach (GridDataItem gr in gvBudget.Items)
        {
            CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");
            Label lblPhase = (Label)gr.FindControl("lblPhase");
            Label lblType = (Label)gr.FindControl("lblType");
            AjaxControlToolkit.HoverMenuExtender hmeRes = (AjaxControlToolkit.HoverMenuExtender)gr.FindControl("hmeRes");
            gr.Attributes["onclick"] = "divExpandCollapse('div" + lblType.Text + lblPhase.Text + "');";

        }
        foreach (GridDataItem gr in gvExpenses.Items)
        {
            CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");
            Label lblPhase = (Label)gr.FindControl("lblPhase");
            Label lblType = (Label)gr.FindControl("lblType");
            AjaxControlToolkit.HoverMenuExtender hmeRes = (AjaxControlToolkit.HoverMenuExtender)gr.FindControl("hmeRes");
            gr.Attributes["onclick"] = "divExpandCollapse('divExp" + lblType.Text + lblPhase.Text + "');";

        }


        //ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyedit", "SelectedItemStyle('" + gvBudget.ClientID + "');", true);
    }
    private void getDiagnosticCategory()
    {
        DataSet ds = new DataSet();
        objGeneral.ConnConfig = Session["config"].ToString();
        ds = objBL_General.getDiagnosticCategory(objGeneral);
        ddlCodeCat.DataSource = ds.Tables[0];
        ddlCodeCat.DataTextField = "category";
        ddlCodeCat.DataValueField = "category";
        ddlCodeCat.DataBind();

        ddlCodeCat.Items.Insert(0, new ListItem("Select", ""));
    }

    //protected void ddlCodeCat_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    objGeneral.ConnConfig = Session["config"].ToString();
    //    objGeneral.CodeCategory = ddlCodeCat.SelectedValue;
    //    objGeneral.CodeType = 1;
    //    DataSet ds = new DataSet();
    //    ds = objBL_General.getDiagnostic(objGeneral);
    //    chklstCodes.DataSource = ds.Tables[0];
    //    chklstCodes.DataTextField = "fdesc";
    //    chklstCodes.DataValueField = "category";
    //    chklstCodes.DataBind();
    //}

    private void getDiagnosticCodes()
    {
        objGeneral.ConnConfig = Session["config"].ToString();
        objGeneral.CodeCategory = ddlCodeCat.SelectedValue;
        objGeneral.CodeType = 1;
        DataSet ds = new DataSet();
        ds = objBL_General.getDiagnostic(objGeneral);
        rptCodesList.DataSource = ds.Tables[0];
        rptCodesList.DataBind();

        //chklstCodes.DataSource = ds.Tables[0];
        //chklstCodes.DataTextField = "fdesc";
        //chklstCodes.DataValueField = "Category";
        //chklstCodes.DataBind();
    }
    private DataTable TaskCodes()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("ID", typeof(int));
        dt.Columns.Add("ticket_id", typeof(int));
        dt.Columns.Add("task_code", typeof(string));
        dt.Columns.Add("Category", typeof(string));
        dt.Columns.Add("Type", typeof(int));
        dt.Columns.Add("job", typeof(int));
        dt.Columns.Add("username", typeof(string));
        dt.Columns.Add("dateupdated", typeof(DateTime));

        foreach (RepeaterItem lst in rptCodesList.Items)
        {
            CheckBox chkCode = (CheckBox)lst.FindControl("chkCode");
            HiddenField hdnCodeCat = (HiddenField)lst.FindControl("hdnCodeCat");
            HiddenField hdnTicket = (HiddenField)lst.FindControl("hdnTicket");
            HiddenField hdnChecked = (HiddenField)lst.FindControl("hdnChecked");
            HiddenField hdnUsername = (HiddenField)lst.FindControl("hdnUsername");
            HiddenField hdnDate = (HiddenField)lst.FindControl("hdnDate");

            if (chkCode.Checked)
            {
                DataRow dr = dt.NewRow();
                dr["ID"] = 0;
                if (hdnTicket.Value != string.Empty)
                    dr["ticket_id"] = Convert.ToInt32(hdnTicket.Value);
                //dr["ticket_id"] = (hdnTicket.Value != string.Empty) ? Convert.ToInt32(hdnTicket.Value) : 0;
                dr["task_code"] = chkCode.Text;
                dr["Category"] = hdnCodeCat.Value;
                if (hdnChecked.Value == "1")
                {
                    dr["username"] = Session["username"].ToString();
                    dr["dateupdated"] = System.DateTime.Now;
                }
                else
                {
                    dr["username"] = hdnUsername.Value;
                    dr["dateupdated"] = Convert.ToDateTime(hdnDate.Value);
                }
                dt.Rows.Add(dr);
            }
        }
        return dt;
    }

    private void getProjectTasks()
    {
        if (Request.QueryString["uid"] != null)
        {
            objProp_Customer.ConnConfig = Session["config"].ToString();
            objProp_Customer.job = Convert.ToInt32(Request.QueryString["uid"].ToString());
            DataSet ds = objBL_Customer.getJobTasks(objProp_Customer);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                foreach (RepeaterItem rt in rptCodesList.Items)
                {
                    CheckBox chkCode = (CheckBox)rt.FindControl("chkCode");
                    HiddenField hdnCodeCat = (HiddenField)rt.FindControl("hdnCodeCat");
                    HiddenField hdnTicket = (HiddenField)rt.FindControl("hdnTicket");
                    HiddenField hdnUsername = (HiddenField)rt.FindControl("hdnUsername");
                    HiddenField hdnDate = (HiddenField)rt.FindControl("hdnDate");
                    Label lblDesc = (Label)rt.FindControl("lblDesc");
                    HyperLink lnkTicket = (HyperLink)rt.FindControl("lnkTicket");

                    if (hdnCodeCat.Value.ToLower() == dr["category"].ToString().ToLower() && chkCode.Text.ToLower() == dr["task_code"].ToString().ToLower())
                    {
                        chkCode.Checked = true;
                        lblDesc.Text = dr["username"].ToString() + "  " + String.Format("{0:MM/dd/yyyy hh:mm tt}", dr["dateupdated"]);
                        hdnTicket.Value = dr["ticket_id"].ToString();
                        hdnUsername.Value = dr["username"].ToString();
                        hdnDate.Value = dr["dateupdated"].ToString();
                        lnkTicket.Text = dr["ticket_id"].ToString();
                        lnkTicket.NavigateUrl = "addticket.aspx?comp=0&id=" + dr["ticket_id"].ToString();
                    }
                }
            }
        }
    }
    protected void ddlCodeCat_SelectedIndexChanged(object sender, EventArgs e)
    {
        SelectTaskCategory();
    }

    private void SelectTaskCategory()
    {
        getDiagnosticCodes();
        getProjectTasks();
    }

    private void GetJobtaskCategory()
    {
        objProp_Customer.ConnConfig = Session["config"].ToString();
        objProp_Customer.job = Convert.ToInt32(Request.QueryString["uid"].ToString());
        string cat = objBL_Customer.getJobTasksCategory(objProp_Customer);
        if (cat.Trim() != string.Empty)
        {
            //ddlCodeCat.SelectedValue = cat;
            ddlCodeCat.Enabled = false;
            getDiagnosticCodes();
            getProjectTasks();
        }
    }

    //protected void gvInnerChildItemGrid_RowDataBound(object sender, GridViewRowEventArgs e)
    //{
    //if (e.Row.RowType == DataControlRowType.DataRow)
    //{

    //    GridView gv = (GridView)e.Row.FindControl("gvChildGrid");
    //    HiddenField hdnType = (HiddenField)e.Row.FindControl("hdnType");
    //    objJob.Type = Convert.ToInt16(hdnType.Value);
    //    if(objJob.Type == 1)
    //    {
    //        var num = e.Row.Cells;
    //        //e.Row.Cells[2].Visible = true;

    //    }
    //}
    //}

    protected void ddlSortAttachment_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindAttachmentGrid();
    }


    private Size GetThumbnailSize(System.Drawing.Image original)
    {
        // Maximum size of any dimension.
        const int maxPixels = 800;

        // Width and height.
        int originalWidth = original.Width;
        int originalHeight = original.Height;

        // Compute best factor to scale entire image based on larger dimension.
        double factor;
        if (originalWidth > originalHeight)
        {
            factor = (double)maxPixels / originalWidth;
        }
        else
        {
            factor = (double)maxPixels / originalHeight;
        }

        // Return thumbnail size.
        return new Size((int)(originalWidth * factor), (int)(originalHeight * factor));
    }

    private Bitmap CreateThumbnail(string lcFilename, int lnWidth, int lnHeight)
    {

        System.Drawing.Bitmap bmpOut = null;
        try
        {
            Bitmap loBMP = new Bitmap(lcFilename);
            System.Drawing.Imaging.ImageFormat loFormat = loBMP.RawFormat;

            decimal lnRatio;
            int lnNewWidth = 0;
            int lnNewHeight = 0;

            //*** If the image is smaller than a thumbnail just return it
            if (loBMP.Width < lnWidth && loBMP.Height < lnHeight)
                return loBMP;


            if (loBMP.Width > loBMP.Height)
            {
                lnRatio = (decimal)lnWidth / loBMP.Width;
                lnNewWidth = lnWidth;
                decimal lnTemp = loBMP.Height * lnRatio;
                lnNewHeight = (int)lnTemp;
            }
            else
            {
                lnRatio = (decimal)lnHeight / loBMP.Height;
                lnNewHeight = lnHeight;
                decimal lnTemp = loBMP.Width * lnRatio;
                lnNewWidth = (int)lnTemp;
            }

            bmpOut = new Bitmap(lnNewWidth, lnNewHeight);
            Graphics g = Graphics.FromImage(bmpOut);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            g.FillRectangle(Brushes.White, 0, 0, lnNewWidth, lnNewHeight);
            g.DrawImage(loBMP, 0, 0, lnNewWidth, lnNewHeight);

            loBMP.Dispose();
        }
        catch
        {
            return null;
        }

        return bmpOut;
    }


    protected void rptattachment_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        LinkButton lnkDownload = e.Item.FindControl("lnkDownload") as LinkButton;
        ScriptManager.GetCurrent(this).RegisterPostBackControl(lnkDownload);


        ImageButton imgattachment = e.Item.FindControl("imgattachment") as ImageButton;
        ScriptManager.GetCurrent(this).RegisterPostBackControl(imgattachment);
    }

    private void GetGC_HOInfo(string LocID)
    {
        objPropUser.DBName = Session["dbname"].ToString();
        objPropUser.LocID = Convert.ToInt32(LocID);
        objPropUser.ConnConfig = Session["config"].ToString();
        var dsGCandHower = objBL_User.GetGCandHowerLocID(objPropUser);

        #region gc information-------------------------------------------------------->

        // hdnGCID.Value = ds.Tables[0].Rows[0]["rol"].ToString();
        //hdnGCIDtemp.Value = ds.Tables[0].Rows[0]["rol"].ToString();
        //hdnGCName.Value = ds.Tables[0].Rows[0]["gcName"].ToString();
        //txtName.Text = ds.Tables[0].Rows[0]["gcName"].ToString();
        //txtCity.Text = ds.Tables[0].Rows[0]["gcCity"].ToString();
        //if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["gcState"].ToString()))
        //{
        //    ddlState.SelectedValue = ds.Tables[0].Rows[0]["gcState"].ToString();
        //}
        //txtPostalCode.Text = ds.Tables[0].Rows[0]["gcZip"].ToString();
        //txtCountry.Text = ds.Tables[0].Rows[0]["gcCountry"].ToString();
        //txtPhone.Text = ds.Tables[0].Rows[0]["gcPhone"].ToString();
        //txtMobile.Text = ds.Tables[0].Rows[0]["gcCellular"].ToString();
        //txtFax.Text = ds.Tables[0].Rows[0]["gcFax"].ToString();
        //txtEmailWeb.Text = ds.Tables[0].Rows[0]["gcEmail"].ToString();
        //txtRemarks.Text = ds.Tables[0].Rows[0]["gcRemarks"].ToString();
        //txtContactName.Text = ds.Tables[0].Rows[0]["gcContact"].ToString();

        // Fill GC Infomation---------------------------------------------------------->

        // Fill GC Infomation---------------------------------------------------------->
        objgn.ResetFormControlValues(tbpnlGCInfo);
        objgn.ResetFormControlValues(TabPaneHomeowner);
        if (dsGCandHower.Tables[0].Rows.Count > 0)
        {
            if (!string.IsNullOrEmpty(dsGCandHower.Tables[0].Rows[0]["RolID"].ToString()))
            {
                if (!Convert.ToInt32(dsGCandHower.Tables[0].Rows[0]["RolID"]).Equals(0) && Convert.ToInt32(dsGCandHower.Tables[0].Rows[0]["LocContactType"]).Equals(1))
                {
                    hdnGCID.Value = dsGCandHower.Tables[0].Rows[0]["RolID"].ToString();
                    hdnGCIDtemp.Value = dsGCandHower.Tables[0].Rows[0]["RolID"].ToString();
                    txtName.Text = dsGCandHower.Tables[0].Rows[0]["RolName"].ToString();
                    txtCity.Text = dsGCandHower.Tables[0].Rows[0]["city"].ToString();
                    GctxtAddress.Text = dsGCandHower.Tables[0].Rows[0]["Address"].ToString();
                    if (!string.IsNullOrEmpty(dsGCandHower.Tables[0].Rows[0]["state"].ToString()))
                    {
                        ddlState.SelectedValue = dsGCandHower.Tables[0].Rows[0]["state"].ToString();
                    }
                    txtPostalCode.Text = dsGCandHower.Tables[0].Rows[0]["zip"].ToString();
                    txtCountry.Text = dsGCandHower.Tables[0].Rows[0]["country"].ToString();
                    txtPhone.Text = dsGCandHower.Tables[0].Rows[0]["phone"].ToString();
                    txtMobile.Text = dsGCandHower.Tables[0].Rows[0]["cellular"].ToString();
                    txtFax.Text = dsGCandHower.Tables[0].Rows[0]["fax"].ToString();
                    txtEmailWeb.Text = dsGCandHower.Tables[0].Rows[0]["email"].ToString();
                    txtRemarks.Text = dsGCandHower.Tables[0].Rows[0]["rolRemarks"].ToString();
                    txtContactName.Text = dsGCandHower.Tables[0].Rows[0]["contact"].ToString();
                    hdnGContractorID.Value = dsGCandHower.Tables[0].Rows[0]["RolID"].ToString();
                    hdnGCNameupdate.Value = "0";
                }
            }
        }
        // Fill Homeowner Infomation---------------------------------------------------------->
        if (dsGCandHower.Tables[1].Rows.Count > 0)
        {
            if (!string.IsNullOrEmpty(dsGCandHower.Tables[1].Rows[0]["RolID"].ToString()))
            {
                if (!Convert.ToInt32(dsGCandHower.Tables[1].Rows[0]["RolID"]).Equals(0) && Convert.ToInt32(dsGCandHower.Tables[1].Rows[0]["LocContactType"]).Equals(2))
                {
                    hotxtname.Text = dsGCandHower.Tables[1].Rows[0]["RolName"].ToString();
                    hotxtcity.Text = dsGCandHower.Tables[1].Rows[0]["city"].ToString();
                    HotxtAddress.Text = dsGCandHower.Tables[1].Rows[0]["Address"].ToString();
                    if (!string.IsNullOrEmpty(dsGCandHower.Tables[1].Rows[0]["state"].ToString()))
                    {
                        hotddlstate.SelectedValue = dsGCandHower.Tables[1].Rows[0]["state"].ToString();
                    }
                    hotxtZIP.Text = dsGCandHower.Tables[1].Rows[0]["zip"].ToString();
                    hotxtCountry.Text = dsGCandHower.Tables[1].Rows[0]["country"].ToString();
                    hotxtPhone.Text = dsGCandHower.Tables[1].Rows[0]["phone"].ToString();
                    hotxtMobile.Text = dsGCandHower.Tables[1].Rows[0]["cellular"].ToString();
                    HotxtFax.Text = dsGCandHower.Tables[1].Rows[0]["fax"].ToString();
                    HotxtEmailWeb.Text = dsGCandHower.Tables[1].Rows[0]["email"].ToString();
                    hotxtRemarks.Text = dsGCandHower.Tables[1].Rows[0]["rolRemarks"].ToString();
                    hotxtContactName.Text = dsGCandHower.Tables[1].Rows[0]["contact"].ToString();
                    hdnHomeOwnerID.Value = dsGCandHower.Tables[1].Rows[0]["RolID"].ToString();
                    hdnHOName.Value = dsGCandHower.Tables[1].Rows[0]["RolName"].ToString();
                    hdnHONameupdate.Value = "0";
                }
            }
        }

        #endregion
    }
    protected void lnkDelete_Click(object sender, EventArgs e)
    {
        string ticketid = string.Empty;
        try
        {
            foreach (GridDataItem di in gvTickets.Items)
            {
                CheckBox chkSelect = (CheckBox)di.FindControl("chkSelect");
                Label lblTicketId = (Label)di.FindControl("lblTicketId");

                if (chkSelect.Checked == true)
                {
                    if (ticketid != string.Empty)
                        ticketid += ", " + lblTicketId.Text;
                    else
                        ticketid = lblTicketId.Text;
                    DeleteTicket(Convert.ToInt32(lblTicketId.Text));
                    GetOpenCalls(string.Empty);
                }
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "keyDelete", "noty({text: 'Ticket # " + ticketid + " Deleted Successfully', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false}); ", true);
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErrDelete", "noty({text: '" + str + "', dismissQueue: true,  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
    }
    private void DeleteTicket(int TicketID)
    {
        objMapData.ConnConfig = Session["config"].ToString();
        objMapData.TicketID = TicketID;
        objMapData.Worker = Session["username"].ToString();
        objBL_MapData.DeleteTicket(objMapData);
    }

    #region Add Attachments

    /// <summary>
    /// Allow the user to add Attachments in the Project screen 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>



    protected void lnkUploadProjectDoc_Click(object sender, EventArgs e)
    {
        try
        {
            if (Request.QueryString["uid"] != null)
            {
                string filename = string.Empty;
                string fullpath = string.Empty;
                string MIME = string.Empty;
                if (FU_Project.HasFile)
                {
                    string savepathconfig = System.Web.Configuration.WebConfigurationManager.AppSettings["AttachmentPath"].Trim();
                    string savepath = savepathconfig + @"\" + Session["dbname"] + @"\ld_" + Request.QueryString["uid"].ToString() + @"\";
                    filename = FU_Project.FileName;
                    fullpath = savepath + filename;
                    MIME = System.IO.Path.GetExtension(FU_Project.PostedFile.FileName).Substring(1);

                    if (File.Exists(fullpath))
                    {
                        GeneralFunctions objGeneralFunctions = new GeneralFunctions();
                        filename = objGeneralFunctions.generateRandomString(4) + "_" + filename;
                        fullpath = savepath + filename;
                    }

                    if (!Directory.Exists(savepath))
                    {
                        Directory.CreateDirectory(savepath);
                    }

                    FU_Project.SaveAs(fullpath);
                }

                objMapData.Screen = "Project";
                objMapData.TicketID = Convert.ToInt32(Request.QueryString["uid"].ToString());
                objMapData.TempId = "0";
                objMapData.FileName = filename;
                objMapData.DocTypeMIME = MIME;
                objMapData.FilePath = fullpath;
                objMapData.DocID = 0;
                objMapData.Mode = 0;
                objMapData.ConnConfig = Session["config"].ToString();
                objBL_MapData.AddFile(objMapData);
                BindAttachmentGrid();
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyUploadErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    #endregion

    protected void lbtnTypeSubmit_Click(object sender, EventArgs e)
    {
        try
        {
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

    #region ddlBType_SelectedIndexChanged
    //protected void ddlBType_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    GridViewRow gvrow = (GridViewRow)((DropDownList)sender).NamingContainer;

    //    DropDownList ddlBType = (DropDownList)gvrow.FindControl("ddlBType");
    //    DropDownList ddlddlMatItem = (DropDownList)gvrow.FindControl("ddlMatItem");

    //    if (ddlBType.SelectedValue == "1")
    //    {
    //        objJob.ConnConfig = Session["config"].ToString();
    //        DataSet dsInv = objBL_Job.GetInventoryItem(objJob);

    //        DataRow dr = dsInv.Tables[0].NewRow();
    //        dr["MatItem"] = 0;
    //        dr["MatDesc"] = "Select Material";
    //        dsInv.Tables[0].Rows.InsertAt(dr, 0);


    //        ddlddlMatItem.DataTextField = "MatDesc";
    //        ddlddlMatItem.DataValueField = "MatItem";
    //        ddlddlMatItem.DataSource = dsInv.Tables[0];
    //        ddlddlMatItem.DataBind();
    //    }
    //    else if (ddlBType.SelectedValue == "2")
    //    {
    //        objJob.ConnConfig = Session["config"].ToString();
    //        DataSet dsInv = objBL_Job.GetLabourMaterial(objJob);

    //        DataRow dr = dsInv.Tables[0].NewRow();
    //        dr["MatItem"] = 0;
    //        dr["MatDesc"] = "Select Material";
    //        dsInv.Tables[0].Rows.InsertAt(dr, 0);


    //        ddlddlMatItem.DataTextField = "MatDesc";
    //        ddlddlMatItem.DataValueField = "MatItem";
    //        ddlddlMatItem.DataSource = dsInv.Tables[0];
    //        ddlddlMatItem.DataBind();
    //    }
    //    else
    //    {
    //        DataTable dt = new DataTable();
    //        dt.Clear();
    //        dt.Columns.Add("MatItem");
    //        dt.Columns.Add("MatDesc");
    //        DataRow first = dt.NewRow();
    //        first["MatItem"] = 0;
    //        first["MatDesc"] = "Select Material";
    //        dt.Rows.Add(first);

    //        ddlddlMatItem.DataTextField = "MatDesc";
    //        ddlddlMatItem.DataValueField = "MatItem";
    //        ddlddlMatItem.DataSource = dt;
    //        ddlddlMatItem.DataBind();
    //    }

    //    DataTable dtBOM = GetBOMGridItems();
    //    BindgvBOM(dtBOM);


    //    ScriptManager.RegisterStartupScript(this, Page.GetType(), "funMaterialLabor", "funMaterialLabor()", true);
    //}
    #endregion

    protected void gvMilestones_RowDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item.ItemType == GridItemType.Item)
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
    protected void gvBOM_RowDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item.ItemType == GridItemType.Item)
        {
            #region BOM Type
            HiddenField hdnBType = (HiddenField)e.Item.FindControl("hdnBType");
            DropDownList drpBType = (DropDownList)e.Item.FindControl("ddlBType");
            drpBType.DataTextField = "Type";
            drpBType.DataValueField = "ID";
            drpBType.DataSource = dtBomType;
            drpBType.DataBind();

            drpBType.Items.Insert(0, new ListItem("Select Type", "Select Type"));
            drpBType.Items.Insert(1, new ListItem(" < Add New > ", "0"));
            drpBType.SelectedValue = hdnBType.Value;
            #endregion

            #region Material Item
            //HiddenField hdnMatItem = (HiddenField)e.Row.FindControl("hdnMatItem");
            //DropDownList drpMatItem = (DropDownList)e.Row.FindControl("ddlMatItem");
            //if (hdnBType.Value == "1")
            //{
            //    DataRow dr = dtInventoryItem.NewRow();
            //    dr["MatItem"] = 0;
            //    dr["MatDesc"] = "Select Material";
            //    dtInventoryItem.Rows.InsertAt(dr, 0);


            //    drpMatItem.DataTextField = "MatDesc";
            //    drpMatItem.DataValueField = "MatItem";
            //    drpMatItem.DataSource = dtInventoryItem;
            //    drpMatItem.DataBind();
            //}

            //else if (hdnBType.Value == "2")
            //{
            //    DataRow dr = dtLabourMaterial.NewRow();
            //    dr["MatItem"] = 0;
            //    dr["MatDesc"] = "Select Material";
            //    dtLabourMaterial.Rows.InsertAt(dr, 0);


            //    drpMatItem.DataTextField = "MatDesc";
            //    drpMatItem.DataValueField = "MatItem";
            //    drpMatItem.DataSource = dtLabourMaterial;
            //    drpMatItem.DataBind();
            //}

            //else
            //{
            //    DataTable dt = new DataTable();
            //    dt.Clear();
            //    dt.Columns.Add("MatItem");
            //    dt.Columns.Add("MatDesc");
            //    DataRow first = dt.NewRow();
            //    first["MatItem"] = 0;
            //    first["MatDesc"] = "Select Material";
            //    dt.Rows.Add(first);

            //    drpMatItem.DataTextField = "MatDesc";
            //    drpMatItem.DataValueField = "MatItem";
            //    drpMatItem.DataSource = dt;
            //    drpMatItem.DataBind();
            //}

            //drpMatItem.SelectedValue = hdnMatItem.Value;
            #endregion
        }
    }


    #region ddlMatItem_SelectedIndexChanged
    //protected void ddlMatItem_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    GridViewRow gvrow = (GridViewRow)((DropDownList)sender).NamingContainer;

    //    DropDownList ddlBType = (DropDownList)gvrow.FindControl("ddlBType");
    //    DropDownList ddlddlMatItem = (DropDownList)gvrow.FindControl("ddlMatItem");

    //    TextBox txtScope = (TextBox)gvrow.FindControl("txtScope");

    //    if (ddlBType.SelectedValue == "1")
    //    {
    //        Int32 ID = Convert.ToInt32(ddlddlMatItem.SelectedValue);
    //        objJob.ConnConfig = Session["config"].ToString();
    //        objJob.ID = ID;
    //        DataSet dsInv = objBL_Job.GetInvById(objJob);

    //        txtScope.Text = Convert.ToString(dsInv.Tables[0].Rows[0]["fDesc"]);

    //    }
    //    else if (ddlBType.SelectedValue == "2")
    //    {
    //        txtScope.Text = ddlddlMatItem.SelectedItem.Text;
    //    }
    //    else
    //    {
    //        //do nothing
    //    }

    //    ScriptManager.RegisterStartupScript(this, Page.GetType(), "funMaterialLabor", "funMaterialLabor()", true);
    //}

    #endregion
    protected void chkBoxIsRecd_OnCheckedChanged(object sender, EventArgs e)
    {
        int JobID = Request.QueryString["uid"] == null ? 0 : Convert.ToInt32(Request.QueryString["uid"]);
        int IsHighLighted = 0;
        if (JobID > 0)
        {
            foreach (GridDataItem row in gvContacts.Items)
            {
                CheckBox check = (CheckBox)row.FindControl("chkBoxIsRecd");
                IsHighLighted = check.Checked == true ? 1 : 0;
                Label PhoneID = (Label)row.FindControl("PhoneID");
                objJob.ConnConfig = Session["config"].ToString();
                objJob.Job = JobID;
                objBL_Job.AddJoinPhoneJob(objJob, Convert.ToInt32(PhoneID.Text), Convert.ToInt32(IsHighLighted));
            }
        }
        Fill_gvContacts();
    }




    private void BindgvMilestones(DataTable dt)
    {
        gvMilestones.DataSource = dt;
        gvMilestones.DataBind();
        GetWIP();
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
                //DropDownList ddlMatItem = (DropDownList)item.FindControl("ddlMatItem");
                DropDownList drpBType = (DropDownList)item.FindControl("ddlBType");


                //bom
                string BOMPermission = ds.Rows[0]["BOMPermission"] == DBNull.Value ? "YYYY" : ds.Rows[0]["BOMPermission"].ToString();
                string stEditBOM = BOMPermission.Length < 2 ? "Y" : BOMPermission.Substring(1, 1);

                DropDownList ddlLabItem = (DropDownList)item.FindControl("ddlLabItem");
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

                TextBox txtMatItem = (TextBox)item.FindControl("txtMatItem");

                if (stEditBOM == "N")
                {
                    txtVendor.Enabled = txtHours.Enabled = txtLabRate.Enabled = txtLabMod.Enabled = txtSDate.Enabled = txtMatMod.Enabled = txtBudgetUnit.Enabled = txtUM.Enabled = txtQtyReq.Enabled = txtScope.Enabled = txtCode.Enabled = IsYes;

                    //ddlLabItem.Enabled = drpBType.Enabled = ddlMatItem.Enabled = IsYes;
                    ddlLabItem.Enabled = drpBType.Enabled = IsYes;
                    txtMatItem.Enabled = IsYes;

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

    public string Truncate(string Value, int length)
    {
        if (Value.Length > length)
        {
            Value = Value.Substring(0, length);
        }
        return Value;
    }



    #region TicketEquipment

    private void GetDataEquip()
    {
        if (hdnLocID.Value != "")
        {
            DataSet ds = new DataSet();
            objPropUser.ConnConfig = Session["config"].ToString();
            objPropUser.SearchBy = string.Empty;
            objPropUser.LocID = Convert.ToInt32(hdnLocID.Value);
            objPropUser.InstallDate = string.Empty;
            objPropUser.ServiceDate = string.Empty;
            objPropUser.Price = string.Empty;
            objPropUser.Manufacturer = string.Empty;
            objPropUser.Status = -1;
            ds = objBL_User.getElev(objPropUser);
            gvEquip.DataSource = ds.Tables[0];
            gvEquip.DataBind();
        }
        else
        {
            gvEquip.DataSource = null;
            gvEquip.DataBind();
        }
    }

    private DataTable GetElevData()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("ticket_id", typeof(int));
        dt.Columns.Add("elev_id", typeof(int));
        dt.Columns.Add("labor_percentage", typeof(double));

        foreach (GridDataItem gvr in gvEquip.Items)
        {
            CheckBox chkSelect = (CheckBox)gvr.FindControl("chkSelect");

            if (chkSelect.Checked == true)
            {
                DataRow dr = dt.NewRow();
                Label lblUnit = (Label)gvr.FindControl("lblID");
                TextBox txtHours = (TextBox)gvr.FindControl("txtHours");
                dr["ticket_id"] = 0;
                dr["elev_id"] = Convert.ToInt32(lblUnit.Text);
                if (txtHours.Text.Trim() != string.Empty)
                {
                    dr["labor_percentage"] = Convert.ToDouble(txtHours.Text);
                }
                dt.Rows.Add(dr);
            }
        }
        return dt;
    }

    #endregion

    private void PopulatePager(int recordCount, int currentPage, Repeater rptPager)
    {
        double dblPageCount = (double)((decimal)recordCount / decimal.Parse(PageSize));
        int pageCount = (int)Math.Ceiling(dblPageCount);
        List<ListItem> pages = new List<ListItem>();
        if (pageCount > 0)
        {
            pages.Add(new ListItem("First", "1", currentPage > 1));
            if (currentPage == 1 && pageCount <= 10)
            {
                for (int i = 1; i <= pageCount; i++)
                {
                    pages.Add(new ListItem(i.ToString(), i.ToString(), i != currentPage));
                }
            }
            else if (currentPage == 1 && pageCount > 10)
            {
                for (int i = 1; i <= 10; i++)
                {
                    pages.Add(new ListItem(i.ToString(), i.ToString(), i != currentPage));
                }
            }
            else if (currentPage != 1 && pageCount <= 10)
            {
                for (int i = 1; i <= pageCount; i++)
                {
                    pages.Add(new ListItem(i.ToString(), i.ToString(), i != currentPage));
                }
            }
            else if (currentPage != 1 && pageCount > 10 && currentPage != pageCount)
            {
                int z = currentPage - 5;
                int x, y;
                if (z < 1)
                {
                    z = 1;
                    for (int i = z; i <= 10; i++)
                    {
                        pages.Add(new ListItem(i.ToString(), i.ToString(), i != currentPage));
                    }
                }
                else if (z >= 1)
                {
                    if (pageCount < currentPage + 4)
                    {
                        y = (z - ((currentPage + 4) - pageCount));
                        x = pageCount;
                    }
                    else
                    {
                        x = currentPage + 4;
                        y = z;
                    }
                    for (int i = y; i <= x; i++)
                    {
                        pages.Add(new ListItem(i.ToString(), i.ToString(), i != currentPage));
                    }
                }
            }
            else if (currentPage == pageCount)
            {
                for (int i = pageCount - 9; i <= pageCount; i++)
                {
                    pages.Add(new ListItem(i.ToString(), i.ToString(), i != currentPage));
                }
            }
            pages.Add(new ListItem("Last", pageCount.ToString(), currentPage < pageCount));
        }
        rptPager.DataSource = pages;
        rptPager.DataBind();
    }

    protected void BudgetPage_Changed(object sender, EventArgs e)
    {
        int pageIndex = int.Parse((sender as LinkButton).CommandArgument);
        this.BindBudget(pageIndex);
    }

    protected void ExpensesPage_Changed(object sender, EventArgs e)
    {
        int pageIndex = int.Parse((sender as LinkButton).CommandArgument);
        this.BindExpense(pageIndex);
    }

    protected void lnkTicketPage_Click(object sender, EventArgs e)
    {
        String OrderBy = Convert.ToString(ViewState["TicketOrderBy"]);
        int pageIndex = int.Parse((sender as LinkButton).CommandArgument);
        this.BindTicketList(OrderBy, pageIndex);
    }


    #region Contact
    protected void btnEditcontact_Click(object sender, EventArgs e)
    {
        if (Convert.ToInt32(ViewState["mode"]) == 1)
        {
            int Countrow = 0;
            foreach (GridDataItem row in gvContacts.Items)
            {

                CheckBox check = (CheckBox)row.FindControl("chkEditcontact");
                if (check.Checked)
                {
                    Countrow += 1;
                    ClearContactControl();
                    divAddContact.Visible = lnkContactClose.Visible = lnkContactSave.Visible = true;
                    imgAddContact.Visible = btnEditcontact.Visible = false;
                    lblconinfo.Text = "Edit Contact";

                    Label PhoneID = (Label)row.FindControl("PhoneID");
                    Label lblName = (Label)row.FindControl("lblName");
                    Label lblTitle = (Label)row.FindControl("lblTitle");
                    Label lblPhn = (Label)row.FindControl("lblPhn");
                    Label lblFx = (Label)row.FindControl("lblFx");
                    Label lblCell = (Label)row.FindControl("lblCell");
                    Label lblEmail = (Label)row.FindControl("lblEmail");
                    Label lblContactType = (Label)row.FindControl("lblContactType");

                    txtContcName.Text = lblName.Text;
                    txtTitle.Text = lblName.Text;
                    txtContPhone.Text = lblPhn.Text;
                    txtContFax.Text = lblFx.Text;
                    txtContCell.Text = lblCell.Text;
                    txtContEmail.Text = lblEmail.Text;
                    hdncontactID.Value = PhoneID.Text == null ? "0" : PhoneID.Text;
                    ddlContactType.SelectedIndex = lblContactType.Text == "Location" ? 0 : 1;
                    chkEmailTicket.Checked = Convert.ToBoolean(lblEmail.Text == "True" ? 1 : 0);
                }
            }
            if (Countrow == 0)
            {
                string str = "Please select contact. ";
                ScriptManager.RegisterStartupScript(this, GetType(),
             "error1212", "noty({text: '" + str + "',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 3000,theme : 'noty_theme_default',  closable : true});", true);
            }
        }

    }

    protected void imgAddContact_Click(object sender, EventArgs e)
    {
        if (Convert.ToInt32(ViewState["mode"]) == 1)
        {
            ClearContactControl();
            divAddContact.Visible = lnkContactClose.Visible = lnkContactSave.Visible = true;
            imgAddContact.Visible = btnEditcontact.Visible = false;
            lblconinfo.Text = "Add Contact";
        }
    }

    protected void lnkContactSave_Click(object sender, EventArgs e)
    {
        if (txtContcName.Text != string.Empty)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ContactID", typeof(int));
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("Phone", typeof(string));
            dt.Columns.Add("Fax", typeof(string));
            dt.Columns.Add("Cell", typeof(string));
            dt.Columns.Add("Email", typeof(string));
            dt.Columns.Add("Title", typeof(string));
            dt.Columns.Add("EmailTicket", typeof(bool));

            DataRow dr = dt.NewRow();
            dr["ContactID"] = Convert.ToInt16(hdncontactID.Value == null ? "0" : hdncontactID.Value);
            dr["Name"] = Truncate(txtContcName.Text, 50);
            dr["Phone"] = Truncate(txtContPhone.Text, 22);
            dr["Fax"] = Truncate(txtContFax.Text, 22);
            dr["Cell"] = Truncate(txtContCell.Text, 22);
            dr["Email"] = Truncate(txtContEmail.Text, 50);
            dr["Title"] = Truncate(txtTitle.Text, 50);
            dr["EmailTicket"] = chkEmailTicket.Checked;
            if (ViewState["mode"].ToString() == "1")
            {
                dt.Rows.Add(dr);
                dt.AcceptChanges();
                SubmitContact(dt, Convert.ToInt16(hdncontactID.Value == null ? "0" : hdncontactID.Value));
                Fill_gvContacts();
            }
        }
    }

    private void SubmitContact(DataTable dt, int ContactID = 0)
    {
        try
        {

            if (Convert.ToInt32(ViewState["mode"]) == 1)
            {
                objPropUser.ContactData = dt;
                objPropUser.ConnConfig = Session["config"].ToString();
                string ContactType = ddlContactType.SelectedItem.ToString();
                int ContactTypeID = ddlContactType.SelectedItem.ToString() == "Customer" ? Convert.ToInt32(hdnCustID.Value) : Convert.ToInt32(hdnLocID.Value);
                int JobID = Request.QueryString["uid"] == null ? 0 : Convert.ToInt32(Request.QueryString["uid"]);
                objBL_User.AddContactFromProjectScreen(objPropUser, ContactType, ContactTypeID, JobID);
                ClearContactControl();
                divAddContact.Visible = lnkContactClose.Visible = lnkContactSave.Visible = false;
                imgAddContact.Visible = btnEditcontact.Visible = true;
                lblconinfo.Text = "";
                string str = ContactID == 0 ? "Contact added successfully!" : "Contact updated successfully!";
                ScriptManager.RegisterStartupScript(this, GetType(),
            "error1212", "noty({text: '" + str + "',  type : 'success', layout:'topCenter',closeOnSelfClick:false, timeout : 3000,theme : 'noty_theme_default',  closable : true});", true);
            }
        }
        catch (Exception ex)
        {
            string str1 = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErrContct", "noty({text: '" + str1 + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);

        }
    }

    private void ClearContactControl()
    {
        hdncontactID.Value = "0";
        txtContcName.Text = "";
        txtContPhone.Text = "";
        txtContFax.Text = "";
        txtContCell.Text = "";
        txtContEmail.Text = "";
        txtTitle.Text = "";
        chkEmailTicket.Checked = false;
    }


    protected void LinkButton4_Click(object sender, EventArgs e)
    {
        ClearContactControl();
        divAddContact.Visible = lnkContactClose.Visible = lnkContactSave.Visible = false;
        imgAddContact.Visible = btnEditcontact.Visible = true;
        lblconinfo.Text = "";
    }

    #endregion Contact

    private void Fillterritory()
    {
        DataSet ds = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();
        ds = objBL_User.getTerritory(objPropUser, 0);
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            //Default Salesperson
            ddlTerr.DataSource = ds.Tables[0];
            ddlTerr.DataTextField = "Name";
            ddlTerr.DataValueField = "ID";
            ddlTerr.DataBind();
            ddlTerr.Items.Insert(0, new ListItem("", ""));
            // Second Salesperson  
            ddlTerr2.DataSource = ds.Tables[0];
            ddlTerr2.DataTextField = "Name";
            ddlTerr2.DataValueField = "ID";
            ddlTerr2.DataBind();
            ddlTerr2.Items.Insert(0, new ListItem("", ""));
        }
    }


    protected void lnkAddPlanner_Click(object sender, EventArgs e)
    {
        //CalculatePlannerData();
        Session["PlannerProjectID"] = Convert.ToInt32(Request.QueryString["uid"]);
        Response.Redirect("AddPlanner.aspx?uid=" + Request.QueryString["uid"]);
        //Response.Redirect("AddPlannerNew.aspx?uid=" + Request.QueryString["uid"] + "&type=e");
    }

    protected void lnkPlannerID_Click(object sender, EventArgs e)
    {
        Session["PlannerProjectID"] = Convert.ToInt32(Request.QueryString["uid"]);
        Response.Redirect("AddPlanner.aspx?uid=" + Request.QueryString["uid"]);
        //Response.Redirect("AddPlannerNew.aspx?uid=" + Request.QueryString["uid"] + "&type=e");
    }


    //private void CalculatePlannerData()
    //{
    //    #region Planner
    //    objProp_Customer.ConnConfig = Session["config"].ToString();
    //    objProp_Customer.ProjectJobID = Convert.ToInt32(Request.QueryString["uid"].ToString());
    //    objProp_Customer.Type = string.Empty;
    //    DataSet ds = objBL_Customer.getJobProjectByJobID(objProp_Customer);

    //    DataTable dttBOM = new DataTable();
    //    dttBOM = ds.Tables[2];

    //    DataTable dttMilestone = new DataTable();
    //    dttMilestone = ds.Tables[4];

    //    List<PlannerModel> listPlanner = new List<PlannerModel>();

    //    foreach (DataRow dr in dttBOM.Rows)
    //    {
    //        PlannerModel data = new PlannerModel();
    //        data.ID = Convert.ToInt32(dr["ID"]);
    //        data.Code = Convert.ToString(dr["code"]);
    //        data.fDesc = Convert.ToString(dr["fDesc"]);
    //        data.Type = "BOM";

    //        listPlanner.Add(data);
    //    }

    //    foreach (DataRow dr in dttMilestone.Rows)
    //    {
    //        PlannerModel data = new PlannerModel();
    //        data.ID = Convert.ToInt32(dr["ID"]);
    //        data.Code = Convert.ToString(dr["jcode"]);
    //        data.fDesc = Convert.ToString(dr["fDesc"]);
    //        data.Type = "Milestone";


    //        listPlanner.Add(data);
    //    }

    //    #region Save Data In Planner Table
    //    objPlanner.ConnConfig = Session["config"].ToString();
    //    objPlanner.ProjectID = Convert.ToInt32(Request.QueryString["uid"]);
    //    objPlanner.Desc = "Project " + Convert.ToString(Request.QueryString["uid"]) + " Planner";
    //    objBL_Planner.AddPlanner(objPlanner);
    //    //ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyProj", "noty({text: 'Planner Created Successfully! ', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : false});", true);
    //    #endregion  

    //    var results = (from p in listPlanner
    //                   group p.Code by p.Code into g
    //                   select new { Code = g.Key }).ToList();


    //    Int32 Pidx = 0;
    //    foreach (var dt in results)
    //    {
    //        #region Add Parent Data
    //        String OppSequ = dt.Code;
    //        objPlanner.ParentID = Convert.ToInt32("0");
    //        objPlanner.TaskName = OppSequ;
    //        objPlanner.idx = Pidx;
    //        objPlanner.TaskType = "OpSequence";
    //        objPlanner.ProjectID = Convert.ToInt32(Request.QueryString["uid"]);
    //        String ParentID = objBL_Planner.AddTaskToPlanner(objPlanner);
    //        Pidx = Pidx + 1;
    //        #endregion

    //        #region Add Sub Task Data
    //        var subTaskList = (from p in listPlanner
    //                           where p.Code == OppSequ
    //                           select p).ToList();
    //        Int32 idx = 0;
    //        foreach (var sdt in subTaskList)
    //        {
    //            objPlanner.TaskName = sdt.fDesc;
    //            objPlanner.ParentID = Convert.ToInt32(ParentID);
    //            objPlanner.idx = idx;
    //            objPlanner.TaskType = sdt.Type;
    //            objPlanner.ProjectID = Convert.ToInt32(Request.QueryString["uid"]);
    //            objBL_Planner.AddTaskToPlanner(objPlanner);
    //            idx = idx + 1;
    //        }
    //        #endregion  

    //    }

    //    #endregion
    //}
    protected void btnConfirmEmail_Click(object sender, EventArgs e)
    {
        btnGenerateReport.Visible = false;
        btnOpenEmail.Visible = true;
        ModalPopupGenerateReport.Show();
        pnlGenerateReport.Visible = true;
    }
    protected void btnOpenEmail_Click(object sender, EventArgs e)
    {
        GenerateSendPDF(sender, e);
        string filename = HttpContext.Current.Server.MapPath("~/TempPDF/SendInvoice/Invoice.pdf");
        if (File.Exists(filename))
        {
            myframe.Attributes["src"] = "Handler.ashx";
            ModalPopupOpenEmail.Show();
            pnlOpenEmail.Visible = true;
        }
    }
    protected void GenerateSendPDF(object sender, EventArgs e)
    {
        if (!chkPrintInvoice.Checked && !chkInvoiceWithTicket.Checked && !chkBillingInvoice.Checked && !chkAIAReport.Checked)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "GenerateReport", "noty({text: 'Please select atleast one to generate report.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            return;
        }

        byte[] PrintByte = null;
        byte[] Byte = null;
        byte[] Byte2 = null;
        byte[] Byte3 = null;
        byte[] Byte4 = null;
        if (chkPrintInvoice.Checked)
        {
            reportPath = "Reports/Invoices.rdlc";
            Byte = lnkPDF_Click(sender, e);
        }
        if (chkInvoiceWithTicket.Checked)
        {
            reportPath = "Reports/InvoicesForAdamMaintenance.rdlc";
            Byte2 = lnkPDFTI_Click(sender, e);
        }
        if (chkBillingInvoice.Checked)
        {
            reportPath = "Reports/InvoicesForAdamBill.rdlc";
            Byte3 = lnkPDF_Click(sender, e);
        }
        if (chkAIAReport.Checked)
        {
            reportPath = HttpContext.Current.Request.MapPath(HttpContext.Current.Request.ApplicationPath) + "/StimulsoftReports/AIAReport.mrt";
            Byte4 = AIAReport(sender, e, null);
        }
        // Delete Invoice File
        string filename = Path.Combine(Server.MapPath(Request.ApplicationPath) + "/TempPDF/SendInvoice", "Invoice.pdf");
        if (File.Exists(filename))
            File.Delete(filename);

        string filename1 = Path.Combine(Server.MapPath(Request.ApplicationPath) + "/TempPDF/SendInvoice", "InvoiceReport.pdf");
        string filename2 = Path.Combine(Server.MapPath(Request.ApplicationPath) + "/TempPDF/SendInvoice", "InvoiceWithTicket.pdf");
        string filename3 = Path.Combine(Server.MapPath(Request.ApplicationPath) + "/TempPDF/SendInvoice", "BillingInvoice.pdf");
        string filename4 = Path.Combine(Server.MapPath(Request.ApplicationPath) + "/TempPDF/SendInvoice", "AIAStatmentReport.pdf");
        if (Byte != null)
        {
            if (File.Exists(filename1))
                File.Delete(filename1);
            using (var fs = new FileStream(filename1, FileMode.Create))
            {
                fs.Write(Byte, 0, Byte.Length);
                fs.Close();
            }
        }
        if (Byte2 != null)
        {
            if (File.Exists(filename2))
                File.Delete(filename2);
            using (var fs = new FileStream(filename2, FileMode.Create))
            {
                fs.Write(Byte2, 0, Byte2.Length);
                fs.Close();
            }
        }
        if (Byte3 != null)
        {
            if (File.Exists(filename3))
                File.Delete(filename3);
            using (var fs = new FileStream(filename3, FileMode.Create))
            {
                fs.Write(Byte3, 0, Byte3.Length);
                fs.Close();
            }
        }
        if (Byte4 != null)
        {
            if (File.Exists(filename4))
                File.Delete(filename4);
            using (var fs = new FileStream(filename4, FileMode.Create))
            {
                fs.Write(Byte4, 0, Byte4.Length);
                fs.Close();
            }
        }
        List<byte[]> pdfByteContent = new List<byte[]>();
        if (File.Exists(filename1))
            pdfByteContent.Add(System.IO.File.ReadAllBytes(Server.MapPath(Request.ApplicationPath) + "/TempPDF/SendInvoice/InvoiceReport.pdf"));
        if (File.Exists(filename2))
            pdfByteContent.Add(System.IO.File.ReadAllBytes(Server.MapPath(Request.ApplicationPath) + "/TempPDF/SendInvoice/InvoiceWithTicket.pdf"));
        if (File.Exists(filename3))
            pdfByteContent.Add(System.IO.File.ReadAllBytes(Server.MapPath(Request.ApplicationPath) + "/TempPDF/SendInvoice/BillingInvoice.pdf"));
        if (File.Exists(filename4))
            pdfByteContent.Add(System.IO.File.ReadAllBytes(Server.MapPath(Request.ApplicationPath) + "/TempPDF/SendInvoice/AIAStatmentReport.pdf"));

        if (File.Exists(filename1))
            File.Delete(filename1);
        if (File.Exists(filename2))
            File.Delete(filename3);
        if (File.Exists(filename3))
            File.Delete(filename3);
        if (File.Exists(filename4))
            File.Delete(filename4);

        if (pdfByteContent.Count > 0)
        {
            PrintByte = concatAndAddContent(pdfByteContent);
            using (var fs = new FileStream(filename, FileMode.Create))
            {
                fs.Write(PrintByte, 0, PrintByte.Length);
                fs.Close();
            }
        }
    }
    protected void btnSendEmail_Click(object sender, EventArgs e)
    {
        txtFrom.Text = "info@mom.com";
        string subject = string.Empty;
        objPropUser.DBName = Session["dbname"].ToString();
        objPropUser.LocID = Convert.ToInt32(hdnLocID.Value);
        DataSet dsloc = new DataSet();
        dsloc = objBL_User.getLocationByID(objPropUser);
        if (dsloc.Tables[0].Rows.Count > 0)
        {
            txtTo.Text = dsloc.Tables[0].Rows[0]["custom12"].ToString();
            txtCC.Text = dsloc.Tables[0].Rows[0]["custom13"].ToString();

            subject = dsloc.Tables[0].Rows[0]["tag"].ToString();

            string address = dsloc.Tables[0].Rows[0]["name"].ToString() + Environment.NewLine;
            address += dsloc.Tables[0].Rows[0]["locAddress"].ToString() + Environment.NewLine;
            address += dsloc.Tables[0].Rows[0]["locCity"].ToString() + ", " + dsloc.Tables[0].Rows[0]["locState"].ToString() + ", " + dsloc.Tables[0].Rows[0]["locZip"].ToString() + Environment.NewLine;
            address += "Phone: " + dsloc.Tables[0].Rows[0]["Phone"].ToString() + Environment.NewLine;
            address += "Fax: " + dsloc.Tables[0].Rows[0]["Fax"].ToString() + Environment.NewLine;
            address += "Email: " + dsloc.Tables[0].Rows[0]["EMail"].ToString() + Environment.NewLine;
            address = "Please review the attached invoice from: " + Environment.NewLine + Environment.NewLine + address;
            ViewState["Company"] = address;
            txtBody.Text = address;
        }
        ViewState["subject"] = subject;
        txtSubject.Text = "Invoice - " + subject;

        ModalPopupSendEmail.Show();
        pnlSendEmail.Visible = true;

    }
    protected void btnEmailInvoice_Click(object sender, EventArgs e)
    {
        if (txtTo.Text.Trim() != string.Empty)
        {
            try
            {
                Mail mail = new Mail();
                mail.From = txtFrom.Text.Trim();
                mail.To = txtTo.Text.Split(';', ',').OfType<string>().ToList();
                if (txtCC.Text.Trim() != string.Empty)
                {
                    mail.Cc = txtCC.Text.Split(';', ',').OfType<string>().ToList();
                }
                mail.Title = txtSubject.Text.Trim(); //"Invoice " + Request.QueryString["uid"].ToString() + " - " + ViewState["subject"].ToString();
                if (txtBody.Text.Trim() != string.Empty)
                {
                    mail.Text = txtBody.Text.Replace("\n", "<BR/>");
                }
                else
                {
                    mail.Text = ViewState["company"].ToString().Replace(Environment.NewLine, "<BR/>");
                }
                //mail.AttachmentFiles.Add(ExportReportToPDF("Report_" + objGen.generateRandomString(10) + ".pdf"));
                mail.FileName = "Invoice.pdf";
                string filename = Path.Combine(Server.MapPath(Request.ApplicationPath) + "/TempPDF/SendInvoice", "Invoice.pdf");
                mail.attachmentBytes = System.IO.File.ReadAllBytes(filename);

                mail.DeleteFilesAfterSend = true;
                mail.RequireAutentication = false;

                mail.Send();
                if (File.Exists(filename))
                {
                    myframe.Attributes["src"] = "";
                    File.Delete(filename);
                }

                // Save Email fields in wip header
                DataTable dtNew = (DataTable)Session["InvoiceSrch"];
                string _ref = "";
                foreach (DataRow _dr in dtNew.Rows)
                {
                    if (dtNew.Rows.IndexOf(_dr) == 0)
                        _ref = _dr["Ref"].ToString();
                    else
                        _ref = _ref + ", " + _dr["Ref"].ToString();
                }
                DataTable tblWIPHeader = new DataTable();
                tblWIPHeader.Columns.Add("Id", typeof(string));
                tblWIPHeader.Columns.Add("JobId", typeof(int));
                tblWIPHeader.Columns.Add("SendTo", typeof(string));
                tblWIPHeader.Columns.Add("SendBy", typeof(string));
                tblWIPHeader.Columns.Add("SendOn", typeof(DateTime));

                DataRow drWIP = tblWIPHeader.NewRow();

                drWIP["Id"] = _ref;
                drWIP["JobId"] = Convert.ToInt32(Request.QueryString["uid"].ToString());
                drWIP["SendTo"] = txtTo.Text;
                drWIP["SendBy"] = Session["username"].ToString();
                drWIP["SendOn"] = DateTime.Now;
                tblWIPHeader.Rows.Add(drWIP);

                DataSet tblWIPr = new DataSet();
                tblWIPr.Tables.Add(tblWIPHeader);

                objProp_Customer.WIP = tblWIPr;
                objProp_Customer.ConnConfig = Session["config"].ToString();

                DataSet ds = objBL_Customer.UpdateMailFields(objProp_Customer);
                DataTable tblProgressBilling = ds.Tables[0];
                if (tblProgressBilling.Rows.Count > 0)
                {
                    gvProgressBilling.DataSource = tblProgressBilling;
                    gvProgressBilling.DataBind();
                }
                this.ModalPopupSendEmail.Hide();
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "noty({text: 'Mail sent successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);

            }
            catch (Exception ex)
            {
                string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            }
        }
    }
    protected void btnGenerate_Click(object sender, EventArgs e)
    {
        btnGenerateReport.Visible = true;
        btnOpenEmail.Visible = false;
        ModalPopupGenerateReport.Show();
        pnlGenerateReport.Visible = true;
    }
    protected void btnGenerateReport_Click(object sender, EventArgs e)
    {
        byte[] PrintByte = null;
        byte[] Byte = null;
        byte[] Byte2 = null;
        byte[] Byte3 = null;
        byte[] Byte4 = null;
        if (chkPrintInvoice.Checked)
        {
            reportPath = "Reports/Invoices.rdlc";
            Byte = lnkPDF_Click(sender, e);
        }
        if (chkInvoiceWithTicket.Checked)
        {
            reportPath = "Reports/InvoicesForAdamMaintenance.rdlc";
            Byte2 = lnkPDFTI_Click(sender, e);
        }
        if (chkBillingInvoice.Checked)
        {
            reportPath = "Reports/InvoicesForAdamBill.rdlc";
            Byte3 = lnkPDF_Click(sender, e);
        }
        if (chkAIAReport.Checked)
        {
            reportPath = HttpContext.Current.Request.MapPath(HttpContext.Current.Request.ApplicationPath) + "/StimulsoftReports/AIAReport.mrt";
            Byte4 = AIAReport(sender, e, null);
        }
        // Delete Invoice File
        string filename = Path.Combine(Server.MapPath(Request.ApplicationPath) + "/TempPDF/SendInvoice", "Invoice.pdf");
        if (File.Exists(filename))
            File.Delete(filename);

        string filename1 = Path.Combine(Server.MapPath(Request.ApplicationPath) + "/TempPDF/SendInvoice", "InvoiceReport.pdf");
        string filename2 = Path.Combine(Server.MapPath(Request.ApplicationPath) + "/TempPDF/SendInvoice", "InvoiceWithTicket.pdf");
        string filename3 = Path.Combine(Server.MapPath(Request.ApplicationPath) + "/TempPDF/SendInvoice", "BillingInvoice.pdf");
        string filename4 = Path.Combine(Server.MapPath(Request.ApplicationPath) + "/TempPDF/SendInvoice", "AIAStatmentReport.pdf");
        if (Byte != null)
        {
            if (File.Exists(filename1))
                File.Delete(filename1);
            using (var fs = new FileStream(filename1, FileMode.Create))
            {
                fs.Write(Byte, 0, Byte.Length);
                fs.Close();
            }
        }
        if (Byte2 != null)
        {
            if (File.Exists(filename2))
                File.Delete(filename2);
            using (var fs = new FileStream(filename2, FileMode.Create))
            {
                fs.Write(Byte2, 0, Byte2.Length);
                fs.Close();
            }
        }
        if (Byte3 != null)
        {
            if (File.Exists(filename3))
                File.Delete(filename3);
            using (var fs = new FileStream(filename3, FileMode.Create))
            {
                fs.Write(Byte3, 0, Byte3.Length);
                fs.Close();
            }
        }
        if (Byte4 != null)
        {
            if (File.Exists(filename4))
                File.Delete(filename4);
            using (var fs = new FileStream(filename4, FileMode.Create))
            {
                fs.Write(Byte4, 0, Byte4.Length);
                fs.Close();
            }
        }
        List<byte[]> pdfByteContent = new List<byte[]>();
        if (File.Exists(filename1))
            pdfByteContent.Add(System.IO.File.ReadAllBytes(Server.MapPath(Request.ApplicationPath) + "/TempPDF/SendInvoice/InvoiceReport.pdf"));
        if (File.Exists(filename2))
            pdfByteContent.Add(System.IO.File.ReadAllBytes(Server.MapPath(Request.ApplicationPath) + "/TempPDF/SendInvoice/InvoiceWithTicket.pdf"));
        if (File.Exists(filename3))
            pdfByteContent.Add(System.IO.File.ReadAllBytes(Server.MapPath(Request.ApplicationPath) + "/TempPDF/SendInvoice/BillingInvoice.pdf"));
        if (File.Exists(filename4))
            pdfByteContent.Add(System.IO.File.ReadAllBytes(Server.MapPath(Request.ApplicationPath) + "/TempPDF/SendInvoice/AIAStatmentReport.pdf"));

        if (pdfByteContent.Count > 0)
            PrintByte = concatAndAddContent(pdfByteContent);

        // Delete all 
        if (File.Exists(filename))
            File.Delete(filename);
        if (File.Exists(filename1))
            File.Delete(filename1);
        if (File.Exists(filename2))
            File.Delete(filename2);
        if (File.Exists(filename3))
            File.Delete(filename3);
        if (File.Exists(filename4))
            File.Delete(filename4);

        Response.Clear();
        Response.ClearContent();
        Response.ClearHeaders();
        Response.Buffer = true;
        Response.AddHeader("Content-Disposition", "attachment;filename=Invoices.pdf");
        Response.ContentType = "application/pdf";
        Response.AddHeader("Content-Length", (PrintByte.Length).ToString());
        Response.BinaryWrite(PrintByte);

        if (!chkPrintInvoice.Checked && !chkInvoiceWithTicket.Checked && !chkBillingInvoice.Checked)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "GenerateReport", "noty({text: 'Please select atleast one to generate report.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "GenerateReport", "noty({text: 'Report Generated successfully!', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : false});", true);
        }

    }
    protected byte[] AIAReport(object sender, EventArgs e, int? InvoiceId)
    {
        byte[] buffer = null;
        DataTable dt = new DataTable();

        DataTable dtInv = new DataTable();
        dtInv.Columns.Add("Ref", typeof(int));

        if (InvoiceId != null)
        {
            DataRow drInv = dtInv.NewRow();
            drInv["Ref"] = InvoiceId;
            dtInv.Rows.Add(drInv);
        }
        else
        {
            foreach (GridDataItem row in gvProgressBilling.Items)
            {
                if (row.ItemType == GridItemType.Item)
                {
                    CheckBox chkRow = (row.Cells[0].FindControl("chkSelect") as CheckBox);
                    if (chkRow.Checked)
                    {
                        DataRow drInv = dtInv.NewRow();
                        Label lblIn = (Label)row.FindControl("lblInvoiceId");
                        if (lblIn != null && !String.IsNullOrEmpty(lblIn.Text))
                        {
                            drInv["Ref"] = Convert.ToInt32(lblIn.Text);
                            dtInv.Rows.Add(drInv);
                        }
                    }
                }

            }
        }
        if (dtInv.Rows.Count > 0)
        {
            if (Request.QueryString["uid"] != null)
                objProp_Customer.job = Convert.ToInt32(Request.QueryString["uid"].ToString());
            objProp_Customer.ConnConfig = Session["config"].ToString();

            for (int i = 0; i < dtInv.Rows.Count; i++)
            {
                objProp_Customer.InvoiceNo = Convert.ToInt32(dtInv.Rows[i][0]);
                DataSet ds = new DataSet();
                ds = objBL_Customer.GetAIAReportData(objProp_Customer);
                StiReport report = new StiReport();
                report.Load(reportPath);
                report.Compile();

                report["JobId"] = objProp_Customer.job;
                report["InvoiceId"] = Convert.ToInt32(dtInv.Rows[i][0]);

                DataSet AIAHeader = new DataSet();
                DataTable hTable = ds.Tables[0].Copy();
                hTable.TableName = "AIAHeader";
                AIAHeader.Tables.Add(hTable);
                AIAHeader.DataSetName = "AIAHeader";

                DataSet AIADetails = new DataSet();
                DataTable dTable = ds.Tables[1].Copy();
                dTable.TableName = "AIADetails";
                AIADetails.Tables.Add(dTable);
                AIADetails.DataSetName = "AIADetails";

                report.RegData("AIAHeader", AIAHeader);
                report.RegData("AIADetails", AIADetails);
                report.Dictionary.Synchronize();
                report.Render();

                var settings = new Stimulsoft.Report.Export.StiPdfExportSettings();
                var service = new Stimulsoft.Report.Export.StiPdfExportService();
                System.IO.MemoryStream stream = new System.IO.MemoryStream();
                service.ExportTo(report, stream, settings);
                buffer = stream.ToArray();

                string filename = Path.Combine(Server.MapPath(Request.ApplicationPath) + "/TempPDF/SendInvoice", "AIA_" + i.ToString() + ".pdf");

                if (buffer != null)
                {
                    if (File.Exists(filename))
                        File.Delete(filename);
                    using (var fs = new FileStream(filename, FileMode.Create))
                    {
                        fs.Write(buffer, 0, buffer.Length);
                        fs.Close();
                    }
                }
            }
            List<byte[]> pdfByteContent = new List<byte[]>();
            for (int i = 0; i < dtInv.Rows.Count; i++)
            {
                string filename = Path.Combine(Server.MapPath(Request.ApplicationPath) + "/TempPDF/SendInvoice", "AIA_" + i.ToString() + ".pdf");
                if (File.Exists(filename))
                    pdfByteContent.Add(System.IO.File.ReadAllBytes(Server.MapPath(Request.ApplicationPath) + "/TempPDF/SendInvoice/AIA_" + i.ToString() + ".pdf"));

                if (File.Exists(filename))
                    File.Delete(filename);
            }
            if (pdfByteContent.Count > 0)
            {
                buffer = concatAndAddContent(pdfByteContent);
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "myFunction", "noty({text: 'No Invoice(s) found to print.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
        return buffer;
    }
    protected byte[] lnkPDF_Click(object sender, EventArgs e)
    {
        byte[] buffer = null;
        try
        {
            DataTable dt = new DataTable();

            DataTable dtInv = new DataTable();
            dtInv.Columns.Add("Ref", typeof(int));

            foreach (GridDataItem row in gvProgressBilling.Items)
            {
                //if (row.RowType == DataControlRowType.DataRow)
                if (row.ItemType == GridItemType.Item)
                {
                    CheckBox chkRow = (row.Cells[0].FindControl("chkSelect") as CheckBox);
                    if (chkRow.Checked)
                    {
                        DataRow drInv = dtInv.NewRow();
                        Label lblIn = (Label)row.FindControl("lblInvoiceId");
                        if (lblIn != null && !String.IsNullOrEmpty(lblIn.Text))
                        {
                            drInv["Ref"] = Convert.ToInt32(lblIn.Text);
                            dtInv.Rows.Add(drInv);
                        }
                    }
                }

            }
            Session["InvoiceSrch"] = dtInv;
            if (dtInv.Rows.Count > 0)
            {
                ReportViewer rvInvoices = new ReportViewer();

                PrintInvoices(rvInvoices);

                buffer = ExportReportToPDF("", rvInvoices);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "myFunction", "noty({text: 'No Invoice(s) found to print.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
        return buffer;
    }
    private void PrintInvoices(ReportViewer rvInvoices)
    {
        // Export to PDF
        try
        {
            DataSet ds = new DataSet();
            DataSet dsInv = new DataSet();
            objProp_Contracts.ConnConfig = Session["config"].ToString();

            if (Session["MSM"].ToString() == "TS")
            {
                if (Session["type"].ToString() != "c")
                    objProp_Contracts.isTS = 1;
            }

            DataTable dtNew = (DataTable)Session["InvoiceSrch"];
            DataTable _dtInvoice = new DataTable();
            DataSet _dsInvoice = new DataSet();
            int j = 0;

            foreach (DataRow _dr in dtNew.Rows)
            {
                int _ref = Convert.ToInt32(_dr["Ref"]);

                objProp_Contracts.InvoiceID = _ref;
                ds = objBL_Contracts.GetInvoicesByRef(objProp_Contracts);

                if (j > 0)
                {
                    _dtInvoice.Merge(ds.Tables[0], true);
                }
                else
                {
                    _dtInvoice = ds.Tables[0];
                }
                j++;
            }
            DataSet dsC = new DataSet();
            objPropUser.ConnConfig = Session["config"].ToString();
            if (Session["MSM"].ToString() != "TS")
            {
                dsC = objBL_User.getControl(objPropUser);
            }
            else
            {
                objPropUser.LocID = Convert.ToInt32(ds.Tables[0].Rows[0]["loc"]);
                dsC = objBL_User.getControlBranch(objPropUser);
            }

            int _days = 0;
            for (int i = 0; i < _dtInvoice.Rows.Count; i++)
            {

                #region Determine Pay Terms
                if (_dtInvoice.Rows[i]["payterms"].ToString() == "0")
                {
                    _days = 0;
                }
                else if (_dtInvoice.Rows[i]["payterms"].ToString() == "1")
                {
                    _days = 10;
                }
                else if (_dtInvoice.Rows[i]["payterms"].ToString() == "2")
                {
                    _days = 15;
                }
                else if (_dtInvoice.Rows[i]["payterms"].ToString() == "3")
                {
                    _days = 30;
                }
                else if (_dtInvoice.Rows[i]["payterms"].ToString() == "4")
                {
                    _days = 45;
                }
                else if (_dtInvoice.Rows[i]["payterms"].ToString() == "5")
                {
                    _days = 60;
                }
                else if (_dtInvoice.Rows[i]["payterms"].ToString() == "6")
                {
                    _days = 30;
                }
                else if (_dtInvoice.Rows[i]["payterms"].ToString() == "7")
                {
                    _days = 90;
                }
                else if (_dtInvoice.Rows[i]["payterms"].ToString() == "8")
                {
                    _days = 180;
                }
                else if (_dtInvoice.Rows[i]["payterms"].ToString() == "9")
                {
                    _days = 0;
                }
                #endregion
                if (!string.IsNullOrEmpty(_dtInvoice.Rows[i]["IDate"].ToString()))
                {
                    _dtInvoice.Rows[i]["DueDate"] = DateTime.Parse(_dtInvoice.Rows[i]["IDate"].ToString()).AddDays(_days);
                    _dtInvoice.Rows[i]["fDate"] = DateTime.Parse(_dtInvoice.Rows[i]["IDate"].ToString());
                }
            }
            #region Get Company Address

            string address = dsC.Tables[0].Rows[0]["name"].ToString() + Environment.NewLine;
            address += dsC.Tables[0].Rows[0]["Address"].ToString() + Environment.NewLine;
            address += dsC.Tables[0].Rows[0]["city"].ToString() + ", " + dsC.Tables[0].Rows[0]["state"].ToString() + ", " + dsC.Tables[0].Rows[0]["zip"].ToString() + Environment.NewLine;
            address += "Phone: " + dsC.Tables[0].Rows[0]["Phone"].ToString() + Environment.NewLine;
            address += "Fax: " + dsC.Tables[0].Rows[0]["fax"].ToString() + Environment.NewLine;
            address += "Email: " + dsC.Tables[0].Rows[0]["email"].ToString() + Environment.NewLine;
            if (Session["dbname"].ToString() == "adams" || Session["dbname"].ToString() == "adamstest")
            {
                address = "Cher client : " + Environment.NewLine + "Veuillez consulter la facture ci-jointe pour paiement. " + Environment.NewLine +
    "Veuillez noter qu’il peut y avoir plusieurs factures contenues " + Environment.NewLine +
    "dans chaque pièce jointe. Si vous avez besoin de clarifications, " + Environment.NewLine +
    "n’hésitez pas à nous contacter.  " + Environment.NewLine + Environment.NewLine +

    "Nous vous remercions d'avoir fair affaire avec notre entreprise." + Environment.NewLine + Environment.NewLine +


    "Dear Valued Customer: " + Environment.NewLine + Environment.NewLine +

    "Please review the attached invoice(s) for processing." + Environment.NewLine +
    "Please note there may be multiple invoices contained " + Environment.NewLine +
    "in each attachment. Should you have any questions, " + Environment.NewLine +
    "Please feel free to contact us." + Environment.NewLine + Environment.NewLine +
    "We appreciate your business!" + Environment.NewLine + Environment.NewLine + address;

            }
            else
            {
                address = "Please review the attached invoice from: " + Environment.NewLine + Environment.NewLine + address;
            }

            ViewState["CompanyAddress"] = address;

            ViewState["EmailFrom"] = "";
            if (Session["MSM"].ToString() != "TS")
            {
                ViewState["EmailFrom"] = dsC.Tables[0].Rows[0]["Email"].ToString();
            }
            #endregion
            ViewState["InvoiceReport"] = _dtInvoice;
            ViewState["CompanyReport"] = dsC.Tables[0];
            Session["InvoiceReportDetails"] = _dtInvoice;

            rvInvoices.LocalReport.DataSources.Clear();
            rvInvoices.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(ItemDetailsSubReportProcessing);
            rvInvoices.LocalReport.DataSources.Add(new ReportDataSource("Invoice_dtInvoice", _dtInvoice));
            rvInvoices.LocalReport.DataSources.Add(new ReportDataSource("Ticket_Company", dsC.Tables[0]));
            rvInvoices.LocalReport.ReportPath = reportPath;
            rvInvoices.LocalReport.EnableExternalImages = true;
            List<Microsoft.Reporting.WebForms.ReportParameter> param1 = new List<Microsoft.Reporting.WebForms.ReportParameter>();
            string strPath = "file:///" + Server.MapPath(Request.ApplicationPath).Replace("\\", "/");
            param1.Add(new Microsoft.Reporting.WebForms.ReportParameter("Path", strPath + "/images/Company_logo.jpg"));
            if (reportPath == "Reports/Invoices.rdlc" || reportPath == "Reports/InvoicesForAdamMaintenance.rdlc" || reportPath == "Reports/InvoicesForAdamBill.rdlc")
            {
                param1.Add(new ReportParameter("IsGstTax", ViewState["IsGst"].ToString()));// ViewState["IsGst"].ToString() ##Needtochange
            }
            rvInvoices.LocalReport.SetParameters(param1);
            rvInvoices.LocalReport.Refresh();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    public void ItemDetailsSubReportProcessing(object sender, SubreportProcessingEventArgs e)
    {
        try
        {
            DataTable dt = (DataTable)ViewState["InvoiceReport"];
            DataTable dtCompany = (DataTable)ViewState["CompanyReport"];
            int refId = Convert.ToInt32(dt.Rows[count_inv]["Ref"]);
            DataTable _dtInvItems = GetInvoiceItems(refId);

            if (_dtInvItems.Rows.Count > 0)
            {
                ReportDataSource rdsItems = new ReportDataSource("dtInvoiceItems", _dtInvItems);

                e.DataSources.Add(rdsItems);
            }
            if (count_inv == dt.Rows.Count - 1)
            {
                ViewState["InvoiceReport"] = null;
                ViewState["CompanyReport"] = null;
            }
            count_inv++;
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    private DataTable GetInvoiceItems(int _refId)
    {
        DataTable _dtItem = new DataTable();
        try
        {
            objProp_Contracts.InvoiceID = _refId;
            objProp_Contracts.ConnConfig = Session["config"].ToString();
            DataSet _dsItemDetails = objBL_Contracts.GetInvoiceItemByRef(objProp_Contracts);
            if (_dsItemDetails.Tables[0].Rows.Count < 1)
            {
                _dtItem = LoadInvoiceDetails(_dsItemDetails.Tables[0], _refId);
            }
            else
                _dtItem = _dsItemDetails.Tables[0];
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }

        return _dtItem;
    }
    private DataTable LoadInvoiceDetails(DataTable _dt, int _idRef)
    {
        DataRow _dr = _dt.NewRow();
        _dr["Ref"] = _idRef;
        _dr["Acct"] = 0;
        _dr["Quan"] = 0;
        _dr["fDesc"] = string.Empty;
        _dr["Price"] = 0.00;
        _dr["Amount"] = 0.00;
        _dr["STax"] = 0.00;
        _dr["billcode"] = string.Empty;
        _dr["staxAmt"] = 0.00;
        _dr["balance"] = 0.00;
        _dr["amtpaid"] = 0.00;
        _dr["total"] = 0.00;
        _dt.Rows.Add(_dr);
        return _dt;
    }
    private byte[] ExportReportToPDF(string reportName, ReportViewer ReportViewer1)
    {
        Warning[] warnings;
        string[] streamids;
        string mimeType;
        string encoding;
        string filenameExtension;

        byte[] bytes = ReportViewer1.LocalReport.Render(
            "PDF", null, out mimeType, out encoding, out filenameExtension,
             out streamids, out warnings);
        return bytes;
    }
    List<byte[]> lstbyte = new List<byte[]>();
    public static byte[] concatAndAddContent(List<byte[]> pdfByteContent)
    {
        MemoryStream ms = new MemoryStream();
        iTextSharp.text.Document doc = new iTextSharp.text.Document();
        iTextSharp.text.pdf.PdfSmartCopy copy = new iTextSharp.text.pdf.PdfSmartCopy(doc, ms);

        doc.Open();

        //Loop through each byte array
        foreach (var p in pdfByteContent)
        {
            iTextSharp.text.pdf.PdfReader reader = new iTextSharp.text.pdf.PdfReader(p);
            int n = reader.NumberOfPages;

            for (int i = 1; i <= n; i++)
            {
                byte[] red = reader.GetPageContent(i);
                if (red.Length < 500)
                {
                    n = n - 1;
                }
            }
            for (int page = 0; page < n;)
            {
                copy.AddPage(copy.GetImportedPage(reader, ++page));
            }
        }
        doc.Close();
        //Return just before disposing
        return ms.ToArray();
    }
    protected byte[] lnkPDFTI_Click(object sender, EventArgs e)
    {
        byte[] allbyte = null;
        try
        {
            DataSet ds = new DataSet();
            DataSet dsInv = new DataSet();
            objProp_Contracts.ConnConfig = Session["config"].ToString();

            if (Session["MSM"].ToString() == "TS")
            {
                if (Session["type"].ToString() != "c")
                    objProp_Contracts.isTS = 1;
            }

            DataTable dtSelectedInv = new DataTable();
            dtSelectedInv.Columns.Add("Ref", typeof(int));
            foreach (GridDataItem row in gvProgressBilling.Items)
            {
                if (row.ItemType == GridItemType.Item)
                {
                    CheckBox chkRow = (row.Cells[0].FindControl("chkSelect") as CheckBox);
                    if (chkRow.Checked)
                    {
                        DataRow drInv = dtSelectedInv.NewRow();
                        Label lblIn = (Label)row.FindControl("lblInvoiceId");
                        if (lblIn != null && !String.IsNullOrEmpty(lblIn.Text))
                        {
                            drInv["Ref"] = Convert.ToInt32(lblIn.Text);
                            dtSelectedInv.Rows.Add(drInv);
                        }
                    }
                }
            }
            Session["InvoiceSrch"] = dtSelectedInv;

            DataTable dtNew = (DataTable)Session["InvoiceSrch"];
            DataTable _dtInvoice = new DataTable();
            DataSet _dsInvoice = new DataSet();
            int j = 0;

            foreach (DataRow _dr in dtNew.Rows)
            {
                int _ref = Convert.ToInt32(_dr["Ref"]);

                objProp_Contracts.InvoiceID = _ref;
                ds = objBL_Contracts.GetInvoicesByRef(objProp_Contracts);

                if (j > 0)
                {
                    _dtInvoice.Merge(ds.Tables[0], true);
                }
                else
                {
                    _dtInvoice = ds.Tables[0];
                }
                j++;
            }
            Session["InvoiceReportDetails"] = _dtInvoice;
            ViewState["InvoicesSubReportResult"] = _dtInvoice;

            DataTable dt = new DataTable();

            DataTable dtInv = (DataTable)Session["InvoiceSrch"];
            if (dtInv.Rows.Count > 0)
            {

                foreach (DataRow drow in _dtInvoice.Rows)
                {
                    ReportViewer rvInvoices = new ReportViewer();
                    int invoiceNo = (int)drow[1];
                    ViewState["invoiceNo"] = invoiceNo;
                    PrintInvoicesTicket(rvInvoices, invoiceNo);
                    array = ExportReportToPDF("", rvInvoices);

                    lstbyte.Add(array);

                }
                allbyte = addProject_New.concatAndAddContent(lstbyte);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "myFunction", "noty({text: 'No Invoice(s) found to print.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
        return allbyte;
    }
    private void PrintInvoicesTicket(ReportViewer rv, int invoiceNo)
    {
        DataTable dtCompany = new DataTable();
        if (ViewState["RecurCompany"] == null)
        {
            DataSet dsCompany = new DataSet();
            objPropUser.ConnConfig = Session["config"].ToString();

            dsCompany = objBL_User.getControl(objPropUser);

            ViewState["RecurCompany"] = dsCompany.Tables[0];
            dtCompany = dsCompany.Tables[0];
        }
        else
        {
            dtCompany = (DataTable)ViewState["RecurCompany"];
        }
        DataTable dtInvoice = (DataTable)Session["InvoiceReportDetails"];
        dtInvoice = dtInvoice.Select("Ref=" + invoiceNo).CopyToDataTable();
        string Report = string.Empty;

        Report = System.Web.Configuration.WebConfigurationManager.AppSettings["InvoiceLNYWithTicket"].Trim();

        DataTable dtEquip = new DataTable();
        DataTable dtTicket = new DataTable();
        DataTable dtTicketPO = new DataTable();
        DataTable dtTicketI = new DataTable();
        DataTable dtDetails = new DataTable();

        if (Report == "Invoice_Ticket-LNY.rdlc")
        {
            int i = 0;

            objProp_Contracts.ConnConfig = Session["config"].ToString();
            objProp_Contracts.InvoiceID = invoiceNo;

            if (Session["MSM"].ToString() == "TS")
            {
                if (Session["type"].ToString() != "c")
                    objProp_Contracts.isTS = 1;
            }

            DataSet TicketID = objBL_Contracts.GetTicketID(objProp_Contracts);

            foreach (DataRow item in TicketID.Tables[0].Rows)
            {
                objMapData.ConnConfig = Session["config"].ToString();
                objMapData.TicketID = (int)item[0];
                DataSet dsEquip = objBL_MapData.getElevByTicket(objMapData);
                DataSet dsTicket = objBL_MapData.GetTicketByID(objMapData);
                objPropUser.ConnConfig = Session["config"].ToString();
                objPropUser.EquipID = 0;
                objPropUser.SearchBy = "rd.ticketID";
                objPropUser.SearchValue = item[0].ToString();
                DataSet dsDetails = objBL_User.getequipREPDetails(objPropUser);
                if (i == 0)
                {
                    dtEquip = dsEquip.Tables[0];
                    dtTicket = dsTicket.Tables[0];
                    dtTicketPO = dsTicket.Tables[1];
                    dtTicketI = dsTicket.Tables[2];
                    dtDetails = dsDetails.Tables[0];
                    i++;
                }
                else
                {
                    if (dtEquip.Rows.Count > 0)
                    {
                        dtEquip.Rows.Add(dsEquip.Tables[0].Rows[0].ItemArray);
                    }

                    dtTicket.Rows.Add(dsTicket.Tables[0].Rows[0].ItemArray);
                    i++;
                }
            }
        }

        rv.LocalReport.DataSources.Clear();  //added by dev 15th march, 16

        rv.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(ItemSubInvoiceTicketReportProcessing);

        if (Report == "Invoice_Ticket-LNY.rdlc")
        {
            if (!string.IsNullOrEmpty(Report.Trim()))
            {
                rv.LocalReport.DataSources.Add(new ReportDataSource("Invoice_PESdtInvoice", dtInvoice));
                rv.LocalReport.DataSources.Add(new ReportDataSource("dtEquipDetails", dtEquip));
                rv.LocalReport.DataSources.Add(new ReportDataSource("Ticket_dtTicket", dtTicket));
                rv.LocalReport.DataSources.Add(new ReportDataSource("Ticket_dtMCP", dtDetails));
                rv.LocalReport.DataSources.Add(new ReportDataSource("Ticket_dtPOItem", dtTicketPO));
                rv.LocalReport.DataSources.Add(new ReportDataSource("Ticket_dtTicketI", dtTicketI));
            }
        }
        else
        {
            rv.LocalReport.DataSources.Add(new ReportDataSource("Ticket_dtTicket", dtTicket));
        }

        rv.LocalReport.DataSources.Add(new ReportDataSource("Ticket_Company", dtCompany));

        DataTable rowCount = (DataTable)Session["InvoiceReportDetails"];

        //if (count == rowCount.Rows.Count - 1)
        //{

        string reportPath = string.Empty;


        if (Report == "Invoice_Ticket-LNY.rdlc")
        {
            if (!string.IsNullOrEmpty(Report.Trim()))
            {
                reportPath = "Reports/" + Report.Trim();
            }
        }


        rv.LocalReport.ReportPath = reportPath;

        rv.LocalReport.EnableExternalImages = true;
        List<Microsoft.Reporting.WebForms.ReportParameter> param1 = new List<Microsoft.Reporting.WebForms.ReportParameter>();
        string strPath = "file:///" + Server.MapPath(Request.ApplicationPath).Replace("\\", "/");
        param1.Add(new Microsoft.Reporting.WebForms.ReportParameter("Path", strPath + "/images/Company_logo.jpg"));
        if (Report == "")
        {
            param1.Add(new ReportParameter("IsGstTax", ViewState["IsGst"].ToString())); //##Needtochange
        }
        rv.LocalReport.SetParameters(param1);

        rv.LocalReport.Refresh();
        //}
    }
    private void ItemSubInvoiceTicketReportProcessing(object sender, SubreportProcessingEventArgs e)
    {
        try
        {
            DataTable dt = (DataTable)ViewState["InvoicesSubReportResult"];
            DataTable dtItems = new DataTable();
            objProp_Contracts.InvoiceID = (int)ViewState["invoiceNo"];
            objProp_Contracts.ConnConfig = Session["config"].ToString();
            DataSet ds = objBL_Contracts.GetInvoiceItemByRef(objProp_Contracts);
            if (ds.Tables[0].Rows.Count < 1)
            {
                dtItems = LoadInvoiceDetails(ds.Tables[0], objProp_Contracts.InvoiceID);    // if none line item exists of invoice
            }
            else
            {
                dtItems = ds.Tables[0];
            }

            DataTable dtEquip = new DataTable();

            int i = 0;
            objProp_Contracts.ConnConfig = Session["config"].ToString();
            objProp_Contracts.InvoiceID = (int)ViewState["invoiceNo"];

            if (Session["MSM"].ToString() == "TS")
            {
                if (Session["type"].ToString() != "c")
                    objProp_Contracts.isTS = 1;
            }

            DataSet TicketID = objBL_Contracts.GetTicketID(objProp_Contracts);
            foreach (DataRow item in TicketID.Tables[0].Rows)
            {
                objMapData.ConnConfig = Session["config"].ToString();
                objMapData.TicketID = (int)item[0];
                DataSet dsEquip = objBL_MapData.getElevByTicketID(objMapData);
                if (i == 0)
                {
                    dtEquip = dsEquip.Tables[0];
                    i++;
                }
                else
                {
                    if (dtEquip.Rows.Count > 0)
                    {
                        dtEquip.Rows.Add(dsEquip.Tables[0].Rows[0].ItemArray);
                    }
                    i++;
                }
            }
            ReportDataSource rdsItems = null;
            if (dtItems.Rows.Count > 0)
            {
                string Report = string.Empty;

                Report = System.Web.Configuration.WebConfigurationManager.AppSettings["InvoiceLNYWithTicket"].Trim();

                if (Report == "Invoice_Ticket-LNY.rdlc")
                {
                    if (!string.IsNullOrEmpty(Report.Trim()))
                    {
                        e.DataSources.Add(rdsItems = new ReportDataSource("dtPESInvoiceItems", dtItems));
                        e.DataSources.Add(rdsItems = new ReportDataSource("dtEquipDetailsID", dtEquip));
                    }
                }
                else
                {
                    e.DataSources.Add(rdsItems = new ReportDataSource("dtInvoiceItems", dtItems));
                }
            }
            if (count_inv == dtItems.Rows.Count - 1)
            {
                ViewState["InvoicesSubReportResult"] = null;
            }
            count_inv++;
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    private void ClearWIPHeader()
    {
        txtProgressBilling.Text = "";
        txtInvoiceNO.Text = "";
        txtBillingDate.Text = DateTime.Now.ToString("M/dd/yyyy");
        //txtArchitectName.Text = "";
        //txtArchitectAdress.Text = "";
        btnSave.Text = " Save WIP ";
        if (!String.IsNullOrEmpty(hdnGlobal_Terms.Value))
            ddlTerms.SelectedValue = hdnGlobal_Terms.Value;
        txtSalesTax.Text = hdnGlobal_SalesTax.Value;
    }
    private void FillBillCodes()
    {
        DataSet ds = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();

        if (Request.QueryString["uid"] != null)
        {
            ds = new BL_BillCodes().GetAllBillCodes(objPropUser);
        }
        else
        {
            ds = new BL_BillCodes().getBillCodes(objPropUser);
        }

        DataRow dr = ds.Tables[0].NewRow();
        ds.Tables[0].Rows.InsertAt(dr, 0);

        dtBillingCodeData = ds.Tables[0];
    }
    private void AssignBillingCodeInGrid(string ID)
    {
        foreach (GridDataItem item in gvWIPs.Items)
        {
            DropDownList _ddlBillingCode = ((DropDownList)item.FindControl("ddlBillingCode"));
            _ddlBillingCode.SelectedIndex = _ddlBillingCode.Items.IndexOf(_ddlBillingCode.Items.FindByValue(ID));
        }
    }
    //protected void StiWebViewerAIA_Click(object sender, Stimulsoft.Report.Web.StiReportDataEventArgs e)
    //{
    //    string reportPath = Server.MapPath("StimulsoftReports/AIADocumentReport.mrt");

    //    StiReport report = new StiReport();
    //    report.Load(reportPath);
    //    report.Compile();

    //    //e.Report = report;
    //    var settings = new Stimulsoft.Report.Export.StiPdfExportSettings();
    //    var service = new Stimulsoft.Report.Export.StiPdfExportService();
    //    System.IO.MemoryStream stream = new System.IO.MemoryStream();
    //    service.ExportTo(report, stream, settings);
    //    Byte[] data = stream.ToArray();
    //    if (data != null)
    //    {
    //        string filename = Path.Combine(Server.MapPath(Request.ApplicationPath) + "/TempPDF/SendInvoice", "AIAStatmentReport.pdf");
    //        if (File.Exists(filename))
    //            File.Delete(filename);
    //        using (var fs = new FileStream(filename, FileMode.Create))
    //        {
    //            fs.Write(data, 0, data.Length);
    //            fs.Close();
    //        }
    //    }
    //}

    private void GetWIP()
    {
        ClearWIPHeader();
        DataSet ds = new DataSet();
        objProp_Customer.ConnConfig = Session["config"].ToString();

        if (Request.QueryString["uid"] != null)
            objProp_Customer.job = Convert.ToInt32(Request.QueryString["uid"].ToString());

        if (!String.IsNullOrEmpty(hdnWIPID.Value))
            objProp_Customer.WIPID = Convert.ToInt32(hdnWIPID.Value);
        else
            objProp_Customer.WIPID = null;

        ds = objBL_Customer.GetWIP(objProp_Customer);
        DataTable tblWIPHeader = ds.Tables[0];
        DataTable tblWIPDetails = ds.Tables[1];

        if (String.IsNullOrEmpty(hdnWIPID.Value)) // Add Mode
        {
            DataTable tblWIPDetailsClone = ds.Tables[1].Clone();
            foreach (GridDataItem gr in gvMilestones.Items)
            {
                DataRow drWIPDet = tblWIPDetailsClone.NewRow();
                if (((HiddenField)gr.FindControl("hdnID")).Value != "")
                    drWIPDet["Line"] = Convert.ToInt16(((HiddenField)gr.FindControl("hdnID")).Value);
                else
                    drWIPDet["Line"] = gr.RowIndex + 1;
                drWIPDet["RowNo"] = gr.RowIndex + 1;
                var PreTotalBilled = tblWIPDetails.Compute("SUM(TotalBilled)", "Line = " + Convert.ToString(drWIPDet["Line"]));

                TextBox fdec = (TextBox)gr.FindControl("txtScope");
                if (fdec != null)
                {
                    if (!string.IsNullOrEmpty(Convert.ToString(fdec.Text)))
                        drWIPDet["WIPDesc"] = Convert.ToString(fdec.Text);
                }
                drWIPDet["ContractAmount"] = 0;
                TextBox txtAmount = (TextBox)gr.FindControl("txtAmount");
                if (txtAmount != null)
                {
                    if (!string.IsNullOrEmpty(Convert.ToString(txtAmount.Text)))
                        drWIPDet["ContractAmount"] = Convert.ToDecimal(txtAmount.Text);
                }

                drWIPDet["PresentlyStored"] = 0;
                drWIPDet["ChangeOrder"] = 0;
                drWIPDet["ScheduledValues"] = 0;
                drWIPDet["PreviousBilled"] = PreTotalBilled;
                drWIPDet["CompletedThisPeriod"] = 0;
                drWIPDet["TotalCompletedAndStored"] = 0;
                drWIPDet["PerComplete"] = 0;
                drWIPDet["BalanceToFinsh"] = 0;
                drWIPDet["RetainagePer"] = 0;
                drWIPDet["RetainageAmount"] = 0;
                drWIPDet["TotalBilled"] = 0;
                if (!String.IsNullOrEmpty(hdnUnrecognizedRevenue.Value))
                    drWIPDet["BillingCode"] = Convert.ToInt32(hdnUnrecognizedRevenue.Value);
                drWIPDet["Taxable"] = false;
                tblWIPDetailsClone.Rows.Add(drWIPDet);
            }
            gvWIPs.DataSource = tblWIPDetailsClone;
            gvWIPs.DataBind();

            // Progress Billings Tab
            if (tblWIPHeader.Rows.Count > 0)
            {
                gvProgressBilling.DataSource = tblWIPHeader;
                gvProgressBilling.DataBind();
            }
            else
            {
                gvProgressBilling.DataSource = null;
                gvProgressBilling.DataBind();
            }
        }
        else // Edit Mode
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                hdnWIPID.Value = ds.Tables[0].Rows[0]["Id"].ToString();
                txtProgressBilling.Text = ds.Tables[0].Rows[0]["ProgressBillingNo"].ToString();
                txtInvoiceNO.Text = ds.Tables[0].Rows[0]["InvoiceId"].ToString();
                txtBillingDate.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["BillingDate"].ToString()).ToString("M/dd/yyyy");
                //ddlApplicationStatus.SelectedValue = ds.Tables[0].Rows[0]["ApplicationStatusId"].ToString();
                ddlTerms.SelectedValue = ds.Tables[0].Rows[0]["Terms"].ToString();
                txtSalesTax.Text = ds.Tables[0].Rows[0]["SalesTax"].ToString();
                //txtArchitectName.Text = ds.Tables[0].Rows[0]["ArchitectName"].ToString().Trim();
                //txtArchitectAdress.Text = ds.Tables[0].Rows[0]["ArchitectAddress"].ToString();

                btnSave.Text = " Edit WIP ";
            }
            if (ds.Tables[1].Rows.Count > 0)
            {
                gvWIPs.DataSource = ds.Tables[1];
                gvWIPs.DataBind();
            }
            else
            {
                tblWIPDetails = ds.Tables[1];
                foreach (GridDataItem gr in gvMilestones.Items)
                {
                    DataRow drWIPDet = tblWIPDetails.NewRow();
                    if (((HiddenField)gr.FindControl("hdnID")).Value != "")
                        drWIPDet["Line"] = Convert.ToInt16(((HiddenField)gr.FindControl("hdnID")).Value);
                    else
                        drWIPDet["Line"] = gr.RowIndex + 1;
                    drWIPDet["RowNo"] = gr.RowIndex + 1;
                    TextBox fdec = (TextBox)gr.FindControl("txtScope");
                    if (!string.IsNullOrEmpty(Convert.ToString(fdec)))
                        drWIPDet["WIPDesc"] = Convert.ToString(fdec.Text);
                    TextBox txtAmount = (TextBox)gr.FindControl("txtAmount");
                    if (!string.IsNullOrEmpty(Convert.ToString(txtAmount)))
                        drWIPDet["ContractAmount"] = Convert.ToDecimal(txtAmount.Text);
                    else
                        drWIPDet["ContractAmount"] = 0;

                    drWIPDet["ChangeOrder"] = 0;
                    drWIPDet["ScheduledValues"] = 0;
                    drWIPDet["PreviousBilled"] = 0;
                    drWIPDet["CompletedThisPeriod"] = 0;
                    drWIPDet["TotalCompletedAndStored"] = 0;
                    drWIPDet["PerComplete"] = 0;
                    drWIPDet["BalanceToFinsh"] = 0;
                    drWIPDet["RetainagePer"] = 0;
                    drWIPDet["RetainageAmount"] = 0;
                    drWIPDet["TotalBilled"] = 0;
                    if (!String.IsNullOrEmpty(hdnUnrecognizedRevenue.Value))
                        drWIPDet["BillingCode"] = Convert.ToInt32(hdnUnrecognizedRevenue.Value);
                    drWIPDet["Taxable"] = false;
                    tblWIPDetails.Rows.Add(drWIPDet);
                }
                gvWIPs.DataSource = tblWIPDetails;
                gvWIPs.DataBind();
            }
        }
    }
    protected void SaveWIP_Click(object sender, EventArgs e)
    {
        try
        {
            DataSet tblWIPr = new DataSet();
            string ValidationMsg = "";
            // WIP Header Part
            DataTable tblWIPHeader = new DataTable();
            tblWIPHeader.Columns.Add("Id", typeof(int));
            tblWIPHeader.Columns.Add("JobId", typeof(int));
            tblWIPHeader.Columns.Add("ProgressBillingNo", typeof(string));
            tblWIPHeader.Columns.Add("InvoiceId", typeof(int)).AllowDBNull = true;
            tblWIPHeader.Columns.Add("BillingDate", typeof(DateTime));
            tblWIPHeader.Columns.Add("ApplicationStatusId", typeof(int));
            tblWIPHeader.Columns.Add("Terms", typeof(Int16));
            tblWIPHeader.Columns.Add("SalesTax", typeof(Decimal));
            //tblWIPHeader.Columns.Add("ArchitectName", typeof(string));
            //tblWIPHeader.Columns.Add("ArchitectAddress", typeof(string));

            DataRow drWIP = tblWIPHeader.NewRow();
            if (!string.IsNullOrEmpty(hdnWIPID.Value))
            {
                drWIP["Id"] = Convert.ToInt32(hdnWIPID.Value);
            }
            drWIP["JobId"] = Convert.ToInt32(Request.QueryString["uid"].ToString());
            drWIP["ProgressBillingNo"] = txtProgressBilling.Text;
            drWIP["InvoiceId"] = DBNull.Value;
            drWIP["BillingDate"] = Convert.ToDateTime(txtBillingDate.Text);
            //drWIP["ApplicationStatusId"] = 1;// Convert.ToInt32(ddlApplicationStatus.SelectedValue);
            drWIP["Terms"] = Convert.ToInt32(ddlTerms.SelectedValue);
            if (!string.IsNullOrEmpty(txtSalesTax.Text))
                drWIP["SalesTax"] = Convert.ToDecimal(txtSalesTax.Text);
            //drWIP["ArchitectName"] = txtArchitectName.Text;
            //drWIP["ArchitectAddress"] = txtArchitectAdress.Text;

            tblWIPHeader.Rows.Add(drWIP);
            tblWIPr.Tables.Add(tblWIPHeader);

            // WIP Details Part
            DataTable tblWIPDetails = new DataTable();
            tblWIPDetails.Columns.Add("Id", typeof(int));
            tblWIPDetails.Columns.Add("WIPId", typeof(int));
            tblWIPDetails.Columns.Add("Line", typeof(int));
            tblWIPDetails.Columns.Add("WIPDesc", typeof(string));
            tblWIPDetails.Columns.Add("ContractAmount", typeof(decimal));
            tblWIPDetails.Columns.Add("ChangeOrder", typeof(decimal));
            tblWIPDetails.Columns.Add("ScheduledValues", typeof(decimal));
            tblWIPDetails.Columns.Add("PreviousBilled", typeof(decimal));
            tblWIPDetails.Columns.Add("CompletedThisPeriod", typeof(decimal));
            tblWIPDetails.Columns.Add("PresentlyStored", typeof(decimal));
            tblWIPDetails.Columns.Add("TotalCompletedAndStored", typeof(decimal));
            tblWIPDetails.Columns.Add("PerComplete", typeof(decimal));
            tblWIPDetails.Columns.Add("BalanceToFinsh", typeof(decimal));
            tblWIPDetails.Columns.Add("RetainagePer", typeof(decimal));
            tblWIPDetails.Columns.Add("RetainageAmount", typeof(decimal));
            tblWIPDetails.Columns.Add("TotalBilled", typeof(decimal));
            tblWIPDetails.Columns.Add("BillingCode", typeof(int));
            tblWIPDetails.Columns.Add("Taxable", typeof(bool));

            foreach (GridDataItem item in gvWIPs.Items)
            {
                DataRow drWIPDet = tblWIPDetails.NewRow();

                if ((HiddenField)item.FindControl("hdnID") != null && ((HiddenField)item.FindControl("hdnID")).Value != "")
                    drWIPDet["Id"] = Convert.ToInt32(((HiddenField)item.FindControl("hdnID")).Value);
                else
                    drWIPDet["Id"] = 0;
                if (!string.IsNullOrEmpty(hdnWIPID.Value))
                    drWIPDet["WIPId"] = Convert.ToInt32(hdnWIPID.Value);
                drWIPDet["Line"] = Convert.ToInt16(((HiddenField)item.FindControl("hdnLine")).Value);
                drWIPDet["WIPDesc"] = Convert.ToString(((TextBox)item.FindControl("txtWIPDesc")).Text);
                drWIPDet["ContractAmount"] = Convert.ToDecimal(((TextBox)item.FindControl("txtContractAmount")).Text);
                drWIPDet["ChangeOrder"] = Convert.ToDecimal(((TextBox)item.FindControl("txtChangeOrder")).Text);
                drWIPDet["ScheduledValues"] = Convert.ToDecimal(((TextBox)item.FindControl("txtScheduledValues")).Text);
                drWIPDet["PreviousBilled"] = Convert.ToDecimal(((TextBox)item.FindControl("txtPreviousBilled")).Text);
                drWIPDet["CompletedThisPeriod"] = Convert.ToDecimal(((TextBox)item.FindControl("txtCompletedThisPeriod")).Text);
                drWIPDet["PresentlyStored"] = Convert.ToDecimal(((TextBox)item.FindControl("txtPresentlyStored")).Text);
                drWIPDet["TotalCompletedAndStored"] = Convert.ToDecimal(((TextBox)item.FindControl("txtTotalCompletedAndStored")).Text);
                drWIPDet["PerComplete"] = Convert.ToDecimal(((TextBox)item.FindControl("txtPerComplete")).Text);
                drWIPDet["BalanceToFinsh"] = Convert.ToDecimal(((TextBox)item.FindControl("txtBalanceToFinsh")).Text);
                drWIPDet["RetainagePer"] = Convert.ToDecimal(((TextBox)item.FindControl("txtRetainagePer")).Text);
                drWIPDet["RetainageAmount"] = Convert.ToDecimal(((TextBox)item.FindControl("txtRetainageAmount")).Text);
                drWIPDet["TotalBilled"] = Convert.ToDecimal(((TextBox)item.FindControl("txtTotalBilled")).Text);
                if (((DropDownList)item.FindControl("ddlBillingCode")).SelectedValue == "")
                {
                    ValidationMsg = "Billing Code Required";
                    ((DropDownList)item.FindControl("ddlBillingCode")).Focus();
                    break;
                }
                drWIPDet["BillingCode"] = Convert.ToInt32(((DropDownList)item.FindControl("ddlBillingCode")).SelectedValue);
                drWIPDet["Taxable"] = Convert.ToBoolean(((CheckBox)item.FindControl("chkTaxable")).Checked);

                tblWIPDetails.Rows.Add(drWIPDet);
            }
            tblWIPr.Tables.Add(tblWIPDetails);

            if (ValidationMsg == "")
            {
                objProp_Customer.WIP = tblWIPr;
                objProp_Customer.ConnConfig = Session["config"].ToString();

                int WIPId = objBL_Customer.SaveWIP(objProp_Customer);

                if (WIPId > 0)
                {
                    hdnWIPID.Value = "";// WIPId.ToString();
                    GetWIP();
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyProj", "noty({text: 'WIP details are saved Successfully!', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : false});", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyValidation", "noty({text: '" + ValidationMsg + "', dismissQueue: true,  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
            }

        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErrDelete", "noty({text: '" + str + "', dismissQueue: true,  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
    }
    protected void lnkProgressBillingNo_Click(object sender, EventArgs e)
    {
        LinkButton lnkProgressBillingNo = (LinkButton)sender;
        hdnWIPID.Value = lnkProgressBillingNo.Attributes["data-id"].ToString();
        GetWIP();
    }
    protected void lnkForm_Click(object sender, EventArgs e)
    {
        LinkButton lnkForm = (LinkButton)sender;
        hdnWIPID.Value = lnkForm.Attributes["data-id"].ToString();
        reportPath = HttpContext.Current.Request.MapPath(HttpContext.Current.Request.ApplicationPath) + "/StimulsoftReports/AIAReport.mrt";
        byte[] PrintByte = AIAReport(sender, e, Convert.ToInt32(hdnWIPID.Value));

        Response.Clear();
        Response.ClearContent();
        Response.ClearHeaders();
        Response.Buffer = true;
        Response.AddHeader("Content-Disposition", "attachment;filename=Invoices.pdf");
        Response.ContentType = "application/pdf";
        Response.AddHeader("Content-Length", (PrintByte.Length).ToString());
        Response.BinaryWrite(PrintByte);

    }
    protected void ibDelete_Click(object sender, EventArgs e)
    {
        ImageButton ibDelete = (ImageButton)sender;
        hdnWIPID.Value = ibDelete.Attributes["data-id"].ToString();
        DeleteWIP();
    }
    protected void DeleteWIP()
    {
        objProp_Customer.ConnConfig = Session["config"].ToString();
        objProp_Customer.job = Convert.ToInt32(Request.QueryString["uid"].ToString());
        if (!String.IsNullOrEmpty(hdnWIPID.Value))
            objProp_Customer.WIPID = Convert.ToInt32(hdnWIPID.Value);
        int count = objBL_Customer.DeleteWIP(objProp_Customer);
        if (count > 0)
        {
            hdnWIPID.Value = "";
            GetWIP();
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyProj", "noty({text: 'WIP record deleted Successfully!', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : false});", true);
        }
    }
    protected void ddlApplicationStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddlApplicationStatus = (DropDownList)sender;
        hdnWIPID.Value = ddlApplicationStatus.Attributes["data-id"].ToString();
        UpdateStatusWIP(Convert.ToInt32((ddlApplicationStatus.SelectedValue)));
    }
    protected void UpdateStatusWIP(int WIPStatus)
    {
        objProp_Customer.ConnConfig = Session["config"].ToString();
        objProp_Customer.job = Convert.ToInt32(Request.QueryString["uid"].ToString());
        if (!String.IsNullOrEmpty(hdnWIPID.Value))
            objProp_Customer.WIPID = Convert.ToInt32(hdnWIPID.Value);
        objProp_Customer.WIPStatus = WIPStatus;
        objProp_Customer.Username = Session["username"].ToString();
        int count = objBL_Customer.UpdateWIPStatus(objProp_Customer);
        if (count > 0)
        {
            objProp_Customer.WIPStatus = null;
            hdnWIPID.Value = "";
            GetWIP();
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyProj", "noty({text: 'Status updated Successfully!', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : false});", true);
        }
    }
    private void FillApplicationStatus()
    {
        try
        {
            DataSet _dsAS = new DataSet();
            objJob.ConnConfig = Session["config"].ToString();
            _dsAS = objBL_Job.GetApplicationStatus(objJob);
            if (_dsAS.Tables[0].Rows.Count > 0)
            {
                dtApplicationStatus = _dsAS.Tables[0];
            }
            else
            {
                DataRow toInsert = _dsAS.Tables[0].NewRow();
                toInsert[0] = "0";
                toInsert[1] = "No data found";
                _dsAS.Tables[0].Rows.Add(toInsert);
                dtApplicationStatus = _dsAS.Tables[0];
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    private void FillTerms()
    {
        try
        {
            DataSet ds = new DataSet();
            objPropUser.ConnConfig = Session["config"].ToString();

            ds = objBL_User.getTerms(objPropUser);

            ddlTerms.DataSource = ds.Tables[0];
            ddlTerms.DataTextField = "name";
            ddlTerms.DataValueField = "id";
            ddlTerms.DataBind();

            ddlTerms.Items.Insert(0, new ListItem(":: Select ::", "0"));
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    protected void gvProgressBilling_PreRender(object sender, EventArgs e)
    {
        int count = gvProgressBilling.Items.Count;
        for (int i = 0; i < count; i++)
        {
            if (i != (count - 1))
            {
                LinkButton lnkProgressBillingNo = (LinkButton)gvProgressBilling.Items[i].FindControl("lnkProgressBillingNo");
                lnkProgressBillingNo.Enabled = false;
                lnkProgressBillingNo.Attributes.CssStyle[HtmlTextWriterStyle.Cursor] = "default";
                lnkProgressBillingNo.OnClientClick = null;

                ImageButton ibDeleteWIP = (ImageButton)gvProgressBilling.Items[i].FindControl("ibDeleteWIP");
                ibDeleteWIP.Visible = false;
            }
        }
    }

    protected void btnWIPCancel_Click(object sender, EventArgs e)
    {
        hdnWIPID.Value = "";
        GetWIP();
    }

    protected void gvBudget_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridGroupHeaderItem)
        {
            GridGroupHeaderItem item = (GridGroupHeaderItem)e.Item;
            DataRowView groupDataRow = (DataRowView)e.Item.DataItem;
            item.DataCell.Text = (groupDataRow["JobType"].ToString() == "Revenue") ? "REVENUES" : "COSTS";
            (item.Cells[0].Controls[0] as Button).Visible = false;
        }
    }
}


[Serializable]
public class PlannerModel1
{
    public int ID { get; set; }
    public String Code { get; set; }
    public String fDesc { get; set; }
    public String Type { get; set; }

}

