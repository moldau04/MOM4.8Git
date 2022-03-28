using BusinessEntity;
using BusinessLayer;
using Stimulsoft.Report;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using Telerik.Web.UI.GridExcelBuilder;

public partial class ProjectWIP : System.Web.UI.Page
{
    Customer objPropCustomer = new Customer();
    BL_Customer objBL_Customer = new BL_Customer();

    User objPropUser = new User();
    BL_User objBL_User = new BL_User();

    BL_ReportsData objBL_ReportsData = new BL_ReportsData();

    JobT objPropJob = new JobT();
    BL_Job objBL_Project = new BL_Job();

    BL_Report objBL_Report = new BL_Report();

    public bool _isPost = false;
    public bool _editAllow = false;
    private bool _isPeriodPost = false;

    protected void Page_Init(object sender, EventArgs e)
    {
        DefineGridStructure();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["userid"] == null)
            {
                Response.Redirect("login.aspx");
            }

            if (!IsPostBack)
            {
                HighlightSideMenu("ProjectMgr", "lnkProject", "ProjectMgrSub");

                if (!string.IsNullOrEmpty(Request["edate"]))
                {
                    txtToDate.Text = HttpUtility.UrlDecode(Request.QueryString["edate"]);
                }
                else
                {
                    txtToDate.Text = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1).AddDays(-1).ToShortDateString();
                }

                if (!string.IsNullOrEmpty(Request["inclClose"]))
                {
                    lnkChk.Checked = Convert.ToBoolean(Request["inclClose"]);
                }

                if (!string.IsNullOrEmpty(Request["noty"]))
                {
                    if (Request["noty"] == "postsucc")
                    {
                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccUp", "noty({text: 'WIP posted successfully!', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                    }

                    if (Request["noty"] == "savesucc")
                    {
                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccUp", "noty({text: 'WIP saved successfully!', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                    }

                    if (Request["noty"] == "updatesucc")
                    {
                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccUp", "noty({text: 'WIP updated successfully!', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                    }
                }

                LoadControl();
                BindProjectDepartments();
            }

            // Check Permission
            Permission();
        }
        catch { }
    }

    private void HighlightSideMenu(string MenuParent, string PageLink, string SubMenuDiv)
    {
        HyperLink aNav = (HyperLink)Page.Master.FindControl(MenuParent);
        aNav.CssClass = "active collapsible-header waves-effect waves-cyan collapsible-height-nl";
        HyperLink lnkUsersSmenu = (HyperLink)Page.Master.FindControl(PageLink);
        lnkUsersSmenu.Style.Add("color", "#316b9d");
        lnkUsersSmenu.Style.Add("font-weight", "normal");
        lnkUsersSmenu.Style.Add("background-color", "#e5e5e5");
        HtmlGenericControl div = (HtmlGenericControl)Page.Master.FindControl(SubMenuDiv);
        div.Style.Add("display", "block");
    }

    protected void lnkClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("Project.aspx?fil=1");
    }

    protected void lnkSearch_Click(object sender, EventArgs e)
    {
        var url = "ProjectWIP.aspx?edate=" + txtToDate.Text + "&inclClose=" + lnkChk.Checked;
        Response.Redirect(url);
    }

    protected void lnkChk_CheckedChanged(object sender, EventArgs e)
    {
        var url = "ProjectWIP.aspx?edate=" + txtToDate.Text + "&inclClose=" + lnkChk.Checked;
        Response.Redirect(url);
    }

    protected void lnkSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (!string.IsNullOrEmpty(txtToDate.Text))
            {
                var userId = Session["userid"];
                objPropJob.UserID = Convert.ToInt32(userId);
                objPropJob.ConnConfig = Session["config"].ToString();

                var fDate = Convert.ToDateTime(txtToDate.Text);
                var period = fDate.Year * 100 + fDate.Month;

                var projectWIPByPeriod = objBL_Project.GetProjectWIPByPeriod(objPropJob, period);

                if (projectWIPByPeriod != null && projectWIPByPeriod.Tables[0].Rows.Count > 0)
                {
                    var wipId = Convert.ToInt32(projectWIPByPeriod.Tables[0].Rows[0]["ID"]);

                    // Update Project WIP
                    objPropJob.ID = wipId;
                    objBL_Project.UpdateProjectWIP(objPropJob, fDate);

                    // Update Project WIP Detail
                    SaveWIPDetail(wipId);

                    var url = "ProjectWIP.aspx?edate=" + txtToDate.Text + "&inclClose=" + lnkChk.Checked + "&noty=updatesucc";
                    Response.Redirect(url);
                }
                else
                {
                    // Add Project WIP
                    var wipId = objBL_Project.AddProjectWIP(objPropJob, fDate);

                    // Add Project WIP Detail
                    SaveWIPDetail(wipId);

                    var url = "ProjectWIP.aspx?edate=" + txtToDate.Text + "&inclClose=" + lnkChk.Checked + "&noty=savesucc";
                    Response.Redirect(url);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarningSave", "noty({text: 'Please select As of Date!', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                txtToDate.Focus();
            }
        }
        catch (Exception ex)
        {
            string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(ex.Message);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyAddCattype", "noty({text: '" + str + "', dismissQueue: true,  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
    }

    protected void lnkPost_Click(object sender, EventArgs e)
    {
        try
        {
            if (!string.IsNullOrEmpty(txtToDate.Text))
            {
                var userId = Session["userid"];
                objPropJob.UserID = Convert.ToInt32(userId);
                objPropJob.ConnConfig = Session["config"].ToString();

                var fDate = Convert.ToDateTime(txtToDate.Text);
                var period = fDate.Year * 100 + fDate.Month;

                var validation = ValidationPost(fDate);

                if (string.IsNullOrEmpty(validation))
                {
                    var projectWIPByPeriod = objBL_Project.GetProjectWIPByPeriod(objPropJob, period);

                    if (projectWIPByPeriod != null && projectWIPByPeriod.Tables[0].Rows.Count > 0)
                    {
                        var wipId = Convert.ToInt32(projectWIPByPeriod.Tables[0].Rows[0]["ID"]);

                        // Update Project WIP with Post
                        objPropJob.ID = wipId;
                        objBL_Project.UpdateProjectWIP(objPropJob, fDate, true);

                        // Update Project WIP Detail
                        SaveWIPDetail(wipId);
                    }
                    else
                    {
                        // Add Project WIP with Post
                        var wipId = objBL_Project.AddProjectWIP(objPropJob, fDate, true);

                        // Add Project WIP Detail
                        SaveWIPDetail(wipId);
                    }

                    var url = "ProjectWIP.aspx?edate=" + txtToDate.Text + "&noty=postsucc";
                    Response.Redirect(url);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarningSave", "noty({text: '" + validation + "', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                    txtToDate.Focus();
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarningSave", "noty({text: 'Please select As of Date!', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                txtToDate.Focus();
            }
        }
        catch (Exception ex)
        {
            string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(ex.Message);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyAddCattype", "noty({text: '" + str + "', dismissQueue: true,  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
    }

    private void SaveWIPDetail(int wipId)
    {
        BusinessEntity.ProjectWIP projectWIP = new BusinessEntity.ProjectWIP();
        projectWIP.WIPID = wipId;
        projectWIP.ConnConfig = Session["config"].ToString();

        foreach (GridDataItem item in RadGrid_Project.MasterTableView.Items)
        {
            if (item.ItemType == GridItemType.Item || item.ItemType == GridItemType.AlternatingItem)
            {
                var lblType = (Label)item.FindControl("lblType");
                var lblDesc = (Label)item.FindControl("lblDesc");
                var lblTag = (Label)item.FindControl("lblTag");
                var lblStatus = (Label)item.FindControl("lblStatus");
                var txtBRev = (TextBox)item.FindControl("txtBRev");
                var txtConstModAdjmts = (TextBox)item.FindControl("txtConstModAdjmts");
                var txtAccountingAdjmts = (TextBox)item.FindControl("txtAccountingAdjmts");
                var lblTotalBudgetedExpense = (Label)item.FindControl("lblTotalBudgetedExpense");
                var lblTotalEstimatedCost = (Label)item.FindControl("lblTotalEstimatedCost");
                var lblEstimatedProfit = (Label)item.FindControl("lblEstimatedProfit");
                var lblNCost = (Label)item.FindControl("lblNCost");
                var lblCostToComplete = (Label)item.FindControl("lblCostToComplete");
                var lblPercentageComplete = (Label)item.FindControl("lblPercentageComplete");
                var lblRevenuesEarned = (Label)item.FindControl("lblRevenuesEarned");
                var lblGrossProfit = (Label)item.FindControl("lblGrossProfit");
                var lblNRev = (Label)item.FindControl("lblNRev");
                var lblToBeBilled = (Label)item.FindControl("lblToBeBilled");
                var lblOpenARAmount = (Label)item.FindControl("lblOpenARAmount");
                var txtRetainageBilling = (TextBox)item.FindControl("txtRetainageBilling");
                var lblTotalBilling = (Label)item.FindControl("lblTotalBilling");
                var lblBillings = (Label)item.FindControl("lblBillings");
                var lblEarnings = (Label)item.FindControl("lblEarnings");
                var lblNPer = (Label)item.FindControl("lblNPer");
                var lblNPerLastMonth = (Label)item.FindControl("lblNPerLastMonth");
                var lblNPerLastYear = (Label)item.FindControl("lblNPerLastYear");
                var lblNPerLastMonthYear = (Label)item.FindControl("lblNPerLastMonthYear");
                var lblBillingContract = (Label)item.FindControl("lblBillingContract");
                var lblJobBorrow = (Label)item.FindControl("lblJobBorrow");
                var hdnDate = (HiddenField)item.FindControl("hdnDate");
                var hdnCloseDate = (HiddenField)item.FindControl("hdnCloseDate");
                var hndIsUpdateRetainage = (HiddenField)item.FindControl("hndIsUpdateRetainage");
                var hndDepartment = (HiddenField)item.FindControl("hndDepartment");
                var txtExpectedClosingDate = (TextBox)item.FindControl("txtExpectedClosingDate");

                projectWIP.Job = Convert.ToInt32(item.GetDataKeyValue("ID").ToString());
                projectWIP.Type = lblType?.Text;
                projectWIP.fDesc = lblDesc?.Text;
                projectWIP.Tag = lblTag?.Text;
                projectWIP.Status = lblStatus?.Text;

                if (txtBRev != null && !string.IsNullOrEmpty(txtBRev.Text))
                {
                    projectWIP.ContractPrice = Convert.ToDouble(txtBRev.Text);
                }

                if (txtConstModAdjmts != null && !string.IsNullOrEmpty(txtConstModAdjmts.Text))
                {
                    projectWIP.ConstModAdjmts = Convert.ToDouble(txtConstModAdjmts.Text);
                }

                if (txtAccountingAdjmts != null && !string.IsNullOrEmpty(txtAccountingAdjmts.Text))
                {
                    projectWIP.AccountingAdjmts = Convert.ToDouble(txtAccountingAdjmts.Text);
                }

                if (lblTotalBudgetedExpense != null && !string.IsNullOrEmpty(lblTotalBudgetedExpense.Text))
                {
                    projectWIP.TotalBudgetedExpense = Convert.ToDouble(lblTotalBudgetedExpense.Text);
                }

                if (lblTotalEstimatedCost != null && !string.IsNullOrEmpty(lblTotalEstimatedCost.Text))
                {
                    projectWIP.TotalEstimatedCost = Convert.ToDouble(lblTotalEstimatedCost.Text);
                }

                if (lblEstimatedProfit != null && !string.IsNullOrEmpty(lblEstimatedProfit.Text))
                {
                    projectWIP.EstimatedProfit = Convert.ToDouble(lblEstimatedProfit.Text);
                }

                if (lblNCost != null && !string.IsNullOrEmpty(lblNCost.Text))
                {
                    projectWIP.ContractCosts = Convert.ToDouble(lblNCost.Text);
                }

                if (lblCostToComplete != null && !string.IsNullOrEmpty(lblCostToComplete.Text))
                {
                    projectWIP.CostToComplete = Convert.ToDouble(lblCostToComplete.Text);
                }

                if (lblPercentageComplete != null && !string.IsNullOrEmpty(lblPercentageComplete.Text))
                {
                    projectWIP.PercentageComplete = double.Parse(lblPercentageComplete.Text.Replace("%", "")) / 100;
                }

                if (lblRevenuesEarned != null && !string.IsNullOrEmpty(lblRevenuesEarned.Text))
                {
                    projectWIP.RevenuesEarned = Convert.ToDouble(lblRevenuesEarned.Text);
                }

                if (lblGrossProfit != null && !string.IsNullOrEmpty(lblGrossProfit.Text))
                {
                    projectWIP.GrossProfit = Convert.ToDouble(lblGrossProfit.Text);
                }

                if (lblNRev != null && !string.IsNullOrEmpty(lblNRev.Text))
                {
                    projectWIP.BilledToDate = Convert.ToDouble(lblNRev.Text);
                }

                if (lblToBeBilled != null && !string.IsNullOrEmpty(lblToBeBilled.Text))
                {
                    projectWIP.ToBeBilled = Convert.ToDouble(lblToBeBilled.Text);
                }

                if (lblOpenARAmount != null && !string.IsNullOrEmpty(lblOpenARAmount.Text))
                {
                    projectWIP.OpenARAmount = Convert.ToDouble(lblOpenARAmount.Text);
                }

                if (txtRetainageBilling != null && !string.IsNullOrEmpty(txtRetainageBilling.Text))
                {
                    projectWIP.RetainageBilling = Convert.ToDouble(txtRetainageBilling.Text);
                }

                if (lblTotalBilling != null && !string.IsNullOrEmpty(lblTotalBilling.Text))
                {
                    projectWIP.TotalBilling = Convert.ToDouble(lblTotalBilling.Text);
                }

                if (lblBillings != null && !string.IsNullOrEmpty(lblBillings.Text))
                {
                    projectWIP.Billings = Convert.ToDouble(lblBillings.Text);
                }

                if (lblEarnings != null && !string.IsNullOrEmpty(lblEarnings.Text))
                {
                    projectWIP.Earnings = Convert.ToDouble(lblEarnings.Text);
                }

                if (lblNPer != null && !string.IsNullOrEmpty(lblNPer.Text))
                {
                    projectWIP.NPer = double.Parse(lblNPer.Text.Replace("%", "")) / 100;
                }

                if (lblNPerLastMonth != null && !string.IsNullOrEmpty(lblNPerLastMonth.Text))
                {
                    projectWIP.NPerLastMonth = double.Parse(lblNPerLastMonth.Text.Replace("%", "")) / 100;
                }

                if (lblNPerLastYear != null && !string.IsNullOrEmpty(lblNPerLastYear.Text))
                {
                    projectWIP.NPerLastYear = double.Parse(lblNPerLastYear.Text.Replace("%", "")) / 100;
                }

                if (lblNPerLastMonthYear != null && !string.IsNullOrEmpty(lblNPerLastMonthYear.Text))
                {
                    projectWIP.NPerLastMonthYear = double.Parse(lblNPerLastMonthYear.Text.Replace("%", "")) / 100;
                }

                if (lblBillingContract != null && !string.IsNullOrEmpty(lblBillingContract.Text))
                {
                    projectWIP.BillingContract = Convert.ToDouble(lblBillingContract.Text);
                }

                if (lblJobBorrow != null && !string.IsNullOrEmpty(lblJobBorrow.Text))
                {
                    projectWIP.JobBorrow = Convert.ToDouble(lblJobBorrow.Text);
                }

                if (hdnDate != null && !string.IsNullOrEmpty(hdnDate.Value))
                {
                    projectWIP.fDate = Convert.ToDateTime(hdnDate.Value);
                }
                else
                {
                    projectWIP.fDate = null;
                }

                if (hdnCloseDate != null && !string.IsNullOrEmpty(hdnCloseDate.Value))
                {
                    projectWIP.CloseDate = Convert.ToDateTime(hdnCloseDate.Value);
                }
                else
                {
                    projectWIP.CloseDate = null;
                }

                if (hndIsUpdateRetainage != null && !string.IsNullOrEmpty(hndIsUpdateRetainage.Value))
                {
                    projectWIP.IsUpdateRetainage = Convert.ToBoolean(hndIsUpdateRetainage.Value);
                }
                else
                {
                    projectWIP.IsUpdateRetainage = false;
                }

                if (hndDepartment != null && !string.IsNullOrEmpty(hndDepartment.Value))
                {
                    projectWIP.Department = Convert.ToInt32(hndDepartment.Value);
                }

                if (txtExpectedClosingDate != null && !string.IsNullOrEmpty(txtExpectedClosingDate.Text))
                {
                    projectWIP.ExpectedClosingDate = Convert.ToDateTime(txtExpectedClosingDate.Text);
                }
                else
                {
                    projectWIP.ExpectedClosingDate = null;
                }

                objBL_Project.AddProjectWIPDetail(projectWIP);
            }
        }
    }

    protected void lnkExportExcel_Click(object sender, EventArgs e)
    {
        byte[] buffer = null;

        var settings = new Stimulsoft.Report.Export.StiExcel2007ExportSettings();
        settings.UseOnePageHeaderAndFooter = true;

        var service = new Stimulsoft.Report.Export.StiExcel2007ExportService();
        System.IO.MemoryStream stream = new System.IO.MemoryStream();
        service.ExportTo(LoadProjectWIPReport(), stream, settings);
        buffer = stream.ToArray();

        Response.Clear();
        MemoryStream ms = new MemoryStream(buffer);
        Response.ContentType = "text/csv";
        Response.AddHeader("content-disposition", "attachment;filename=ProjectWIP.xlsx");
        Response.Buffer = true;
        ms.WriteTo(Response.OutputStream);
        Response.End();
    }

    protected void lnkPostReport_Click(object sender, EventArgs e)
    {
        Response.Redirect("WIPPostReport.aspx?edate=" + txtToDate.Text);
    }

    private StiReport LoadProjectWIPReport()
    {
        try
        {
            string reportPathStimul = Server.MapPath("StimulsoftReports/ProjectWIPReport.mrt");
            StiReport report = new StiReport();
            report.Load(reportPathStimul);
            //report.Compile();

            //Get data
            DataSet companyInfo = new DataSet();
            companyInfo = objBL_Report.GetCompanyDetails(Session["config"].ToString());

            report.RegData("CompanyDetails", companyInfo.Tables[0]);

            objPropCustomer.ConnConfig = WebBaseUtility.ConnectionString;
            objPropCustomer.UserID = Convert.ToInt32(System.Web.HttpContext.Current.Session["UserID"].ToString());
            objPropCustomer.JobType = -1;
            if (HttpContext.Current.Session["CmpChkDefault"].ToString() == "1")
            {
                objPropCustomer.EN = 1;
            }
            else
            {
                objPropCustomer.EN = 0;
            }

            if (!string.IsNullOrEmpty(Request["edate"]))
            {
                objPropCustomer.EndDate = HttpUtility.UrlDecode(Request.QueryString["edate"]);
            }
            else
            {
                objPropCustomer.EndDate = txtToDate.Text;
            }

            var includeClose = lnkChk.Checked ? 1 : 0;
            DataSet ds = objBL_Customer.GetJobProjectWIP(objPropCustomer, includeClose, _isPeriodPost);

            if (ds != null)
            {
                report.RegData("ReportData", ds.Tables[0]);
            }

            var endDate = Convert.ToDateTime(objPropCustomer.EndDate);

            report.Dictionary.Variables["EndDate"].Value = objPropCustomer.EndDate;
            report.Dictionary.Variables["LastMonthDate"].Value = endDate.AddDays(-endDate.Day).ToShortDateString(); ;
            report.Dictionary.Variables["LastYearDate"].Value = new DateTime(endDate.Year - 1, 12, 31).ToShortDateString(); ;
            report.Dictionary.Variables["LastMonthYearDate"].Value = endDate.AddDays(-endDate.Day).AddYears(-1).ToShortDateString();
            report.Render();

            return report;
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            return null;
        }
    }

    private void PostPermission(bool editAllow)
    {
        objPropJob.ConnConfig = Session["config"].ToString();

        DateTime fDate = new DateTime();
        if (!string.IsNullOrEmpty(Request["edate"]))
        {
            fDate = Convert.ToDateTime(HttpUtility.UrlDecode(Request.QueryString["edate"]));
        }
        else
        {
            fDate = Convert.ToDateTime(txtToDate.Text);
        }

        var period = fDate.Year * 100 + fDate.Month;
        var projectWIPByPeriod = objBL_Project.GetLastProjectWIPPostedByPeriod(objPropJob, period);

        if (projectWIPByPeriod != null && projectWIPByPeriod.Tables[0].Rows.Count > 0)
        {
            _isPost = true;

            if (projectWIPByPeriod.Tables[0].Rows[0]["fDate"] != null)
            {
                var postDate = Convert.ToDateTime(projectWIPByPeriod.Tables[0].Rows[0]["fDate"]);
                var fUser = projectWIPByPeriod.Tables[0].Rows[0]["fUser"];
                lblMessage.Text = $"Project WIP has been posted on as of {postDate.ToShortDateString()} by {fUser}";
                lblMessage.Visible = true;
            }

            if (projectWIPByPeriod.Tables[1].Rows.Count > 0 && Convert.ToBoolean(projectWIPByPeriod.Tables[1].Rows[0]["IsPost"]))
            {
                _isPeriodPost = true;
            }
        }
        else if (projectWIPByPeriod != null && projectWIPByPeriod.Tables[1].Rows.Count > 0)
        {
            var updateDate = Convert.ToDateTime(projectWIPByPeriod.Tables[1].Rows[0]["LastUpdate"]);
            var fUser = projectWIPByPeriod.Tables[1].Rows[0]["fUser"];
            lblMessage.Text = $"This period was saved on {updateDate.ToShortDateString()} by {fUser}";
            lblMessage.Visible = true;
        }

        if (projectWIPByPeriod != null && projectWIPByPeriod.Tables[2].Rows.Count > 0)
        {
            var lastPostDate = Convert.ToDateTime(projectWIPByPeriod.Tables[2].Rows[0]["fDate"]);

            lblLastPost.Visible = true;
            lblLastPost.Text = $"Last posted date: {lastPostDate.ToString("d")}";
        }

        lnkSave.Visible = !_isPost && editAllow;
        lnkPost.Visible = !_isPost && editAllow;
    }

    private void Permission()
    {
        DataTable ds = GetUserById();

        //WIP
        string wipPermission = ds.Rows[0]["WIPPermission"] == DBNull.Value ? "NNNNNN" : ds.Rows[0]["WIPPermission"].ToString();
        string addWIP = wipPermission.Length < 6 ? "N" : wipPermission.Substring(0, 1);
        string editWIP = wipPermission.Length < 6 ? "N" : wipPermission.Substring(1, 1);
        string viewWIP = wipPermission.Length < 6 ? "N" : wipPermission.Substring(3, 1);
        string reportWIP = wipPermission.Length < 6 ? "N" : wipPermission.Substring(5, 1);

        if (viewWIP == "N")
        {
            Response.Redirect("Project.aspx?fil=1");
        }

        if (editWIP == "N" && addWIP == "N")
        {
            _editAllow = false;
            lnkSave.Visible = false;
            lnkPost.Visible = false;
        }
        else
        {
            _editAllow = true;
            lnkSave.Visible = true;
            lnkPost.Visible = true;
        }

        PostPermission(_editAllow);
    }

    private string ValidationPost(DateTime fDate)
    {
        var period = fDate.Year * 100 + fDate.Month;

        objPropJob.ConnConfig = Session["config"].ToString();
        var ds = objBL_Project.GetLastPostProjectWIP(objPropJob);

        if (ds.Tables[0].Rows.Count > 0)
        {
            var lastPostDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["fDate"]);
            var nextPostDate = lastPostDate.AddMonths(1);
            var nextPostPeriod = nextPostDate.Year * 100 + nextPostDate.Month;

            if (period != nextPostPeriod)
            {
                if (fDate.Year != nextPostDate.Year)
                {
                    return string.Format("Please Post {0} month data before {1}!", nextPostDate.ToString("y"), fDate.ToString("y"));
                }
                else
                {
                    return string.Format("Please Post {0} month data before {1}!", nextPostDate.ToString("MMMM"), fDate.ToString("MMMM"));
                }
            }
        }

        return string.Empty;
    }

    private DataTable GetUserById()
    {
        objPropUser.TypeID = Convert.ToInt32(Session["usertypeid"]);
        objPropUser.UserID = Convert.ToInt32(Session["userid"]);
        objPropUser.ConnConfig = Session["config"].ToString();
        objPropUser.DBName = Session["dbname"].ToString();
        DataSet ds = objBL_User.GetUserPermissionByUserID(objPropUser);

        return ds.Tables[0];
    }

    private void BindProjectDepartments()
    {
        DataSet ds = new DataSet();
        objPropJob.ConnConfig = Session["config"].ToString();

        if (!string.IsNullOrEmpty(Request["edate"]))
        {
            objPropJob.EndDate = Convert.ToDateTime(HttpUtility.UrlDecode(Request.QueryString["edate"]));
        }
        else
        {
            objPropJob.EndDate = Convert.ToDateTime(txtToDate.Text);
        }

        ds = objBL_Project.GetJobTypeForWIP(objPropJob);

        rptDepartmentTab.DataSource = ds.Tables[0];
        rptDepartmentTab.DataBind();
    }

    private void LoadControl()
    {
        DateTime toDate = new DateTime();
        if (!string.IsNullOrEmpty(Request["edate"]))
        {
            toDate = Convert.ToDateTime(HttpUtility.UrlDecode(Request.QueryString["edate"]));
        }
        else
        {
            toDate = Convert.ToDateTime(txtToDate.Text);
        }

        RadGrid_Project.Columns.FindByUniqueName("NPer").HeaderText = txtToDate.Text;
        RadGrid_Project.Columns.FindByUniqueName("NPerLastMonth").HeaderText = toDate.AddDays(-toDate.Day).ToShortDateString();
        RadGrid_Project.Columns.FindByUniqueName("NPerLastYear").HeaderText = new DateTime(toDate.Year - 1, 12, 31).ToShortDateString();
        RadGrid_Project.Columns.FindByUniqueName("NPerLastMonthYear").HeaderText = new DateTime(toDate.Year - 1, 12, 31).AddMonths(-6).ToShortDateString();
    }

    private void ProjectList()
    {
        #region Company Check

        objPropCustomer.ConnConfig = WebBaseUtility.ConnectionString;
        objPropCustomer.UserID = Convert.ToInt32(System.Web.HttpContext.Current.Session["UserID"].ToString());
        if (HttpContext.Current.Session["CmpChkDefault"].ToString() == "1")
        {
            objPropCustomer.EN = 1;
        }
        else
        {
            objPropCustomer.EN = 0;
        }

        #endregion

        objPropCustomer.JobType = Convert.ToInt16(hdDept.Value);

        if (!string.IsNullOrEmpty(Request["edate"]))
        {
            objPropCustomer.EndDate = HttpUtility.UrlDecode(Request.QueryString["edate"]);
        }
        else
        {
            objPropCustomer.EndDate = txtToDate.Text;
        }

        var includeClose = lnkChk.Checked ? 1 : 0;
        DataSet ds = objBL_Customer.GetJobProjectWIP(objPropCustomer, includeClose, _isPeriodPost);

        DataTable dt = ds.Tables[0].Clone();
        var filterString = RadGrid_Project.MasterTableView.FilterExpression;

        if (!string.IsNullOrEmpty(filterString))
        {
            var buildfiltersString = BuildFiltersString();
            var data = ds.Tables[0].Select(buildfiltersString, "");

            if (data.Count() > 0)
            {
                dt = data.CopyToDataTable();
            }

            RadGrid_Project.MasterTableView.FilterExpression = buildfiltersString;
        }
        else
        {
            dt = ds.Tables[0];
        }

        RadGrid_Project.VirtualItemCount = dt.Rows.Count;
        RadGrid_Project.DataSource = dt;
        //RadGrid_Project.MasterTableView.FilterExpression = string.Empty;

        lblRecordCount.Text = dt.Rows.Count.ToString() + " Record(s) found";
    }

    private string BuildFiltersString()
    {
        var filterString = string.Empty;

        foreach (GridColumn column in RadGrid_Project.MasterTableView.OwnerGrid.Columns)
        {
            string filterText = string.Empty;
            string filterValues = column.CurrentFilterValue;

            if (!string.IsNullOrEmpty(filterValues))
            {
                if (column.UniqueName == "Type" || column.UniqueName == "fDesc" || column.UniqueName == "Tag" || column.UniqueName == "Status")
                {
                    filterText = string.Format("({0} LIKE '%{1}%')", column.UniqueName, filterValues);
                }
                else if (column.UniqueName == "ExpectedClosingDate")
                {
                    filterText = string.Format("({0} = '{1}')", column.UniqueName, filterValues);
                }
                else
                {
                    double value;
                    if (double.TryParse(filterValues, out value))
                    {
                        if (column.UniqueName == "PercentageComplete")
                        {
                            value = value / 100;
                        }

                        filterText = string.Format("({0} = {1})", column.UniqueName, value);
                    }
                }

                if (!string.IsNullOrEmpty(filterText))
                {
                    if (!string.IsNullOrEmpty(filterString))
                    {
                        filterString += string.Format(" AND {0}", filterText);
                    }
                    else
                    {
                        filterString = filterText;
                    }
                }
            }
        }

        return filterString;
    }

    protected void RadGrid_Project_ItemCreated(object sender, GridItemEventArgs e)
    {
        try
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
        catch { }
    }

    protected void RadGrid_Project_ExcelMLExportRowCreated(object source, GridExportExcelMLRowCreatedArgs e)
    {
        int currentItem = 0;
        if (Convert.ToString(Session["CmpChkDefault"]) == "1")
            currentItem = 3;
        else
            currentItem = 4;
        if (e.Worksheet.Table.Rows.Count == RadGrid_Project.Items.Count + 1)
        {
            GridFooterItem footerItem = RadGrid_Project.MasterTableView.GetItems(GridItemType.Footer)[0] as GridFooterItem;
            RowElement row = new RowElement(); //create new row for the footer aggregates
            for (int i = currentItem; i < footerItem.Cells.Count; i++)
            {
                TableCell fcell = footerItem.Cells[i];
                CellElement cell = new CellElement();
                // cell.Data.DataItem =  fcell.Text == "&nbsp;" ? "" : fcell.Text;
                if (i == currentItem)
                    cell.Data.DataItem = "Total:-";
                else
                    cell.Data.DataItem = fcell.Text == "&nbsp;" ? "" : fcell.Text;
                row.Cells.Add(cell);
            }
            e.Worksheet.Table.Rows.Add(row);

        }
    }

    protected void RadGrid_Project_PreRender(object sender, EventArgs e)
    {
        try
        {
            DataSet ds = new DataSet();
            objPropUser.ConnConfig = HttpContext.Current.Session["config"].ToString();
            objPropUser.UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"].ToString());
            objPropUser.PageName = "ProjectWIP.aspx";
            objPropUser.GridId = "RadGrid_Project";
            ds = objBL_User.GetGridUserSettings(objPropUser);

            if (ds.Tables[0].Rows.Count > 0)
            {
                var columnSettings = ds.Tables[0].Rows[0][0].ToString();
                var columnsArr = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ColumnSettings>>(columnSettings);
                var colIndex = 0;

                foreach (GridColumn column in RadGrid_Project.MasterTableView.OwnerGrid.Columns)
                {
                    colIndex++;
                    var clSetting = columnsArr.Where(t => t.Name.Equals(column.UniqueName, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
                    if (clSetting != null)
                    {
                        column.Display = clSetting.Display;
                        if (colIndex >= 3 && clSetting.Width != 0)
                            column.HeaderStyle.Width = clSetting.Width;

                        column.OrderIndex = clSetting.OrderIndex;
                    }
                }

                ScriptManager.RegisterStartupScript(this, Page.GetType(), "showhidebutton", "ShowRestoreGridSettingsButton();", true);
            }
        }
        catch { }
    }

    protected void RadGrid_Project_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        ProjectList();
    }

    protected void RadGrid_Project_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName == "Filter")
        {

        }
    }

    #region show/hidden column

    protected void lnkSaveGridSettings_Click(object sender, EventArgs e)
    {
        var columnSettings = GetGridColumnSettings();
        objPropUser.ConnConfig = HttpContext.Current.Session["config"].ToString();
        objPropUser.UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"].ToString());
        objPropUser.PageName = "ProjectWIP.aspx";
        objPropUser.GridId = "RadGrid_Project";

        objBL_User.UpdateUserGridCustomSettings(objPropUser, columnSettings);
    }

    protected void lnkRestoreGridSettings_Click(object sender, EventArgs e)
    {
        var columnSettings = string.Empty;
        objPropUser.ConnConfig = HttpContext.Current.Session["config"].ToString();
        objPropUser.UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"].ToString());
        objPropUser.PageName = "ProjectWIP.aspx";
        objPropUser.GridId = "RadGrid_Project";

        var ds = objBL_User.DeleteUserGridCustomSettings(objPropUser);

        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            columnSettings = ds.Tables[0].Rows[0][0].ToString();
            var columnsArr = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ColumnSettings>>(columnSettings);

            foreach (GridColumn column in RadGrid_Project.MasterTableView.OwnerGrid.Columns)
            {
                var clSetting = columnsArr.Where(t => t.Name.Equals(column.UniqueName, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
                if (clSetting != null)
                {
                    column.Display = clSetting.Display;
                    if (clSetting.Width != 0)
                        column.HeaderStyle.Width = clSetting.Width;

                    column.OrderIndex = clSetting.OrderIndex;
                }
            }

            RadGrid_Project.MasterTableView.Rebind();
        }
        else
        {
            var colIndex = 0;

            foreach (GridColumn column in RadGrid_Project.MasterTableView.OwnerGrid.Columns)
            {
                colIndex++;
                column.Display = true;
                column.OrderIndex = colIndex;
            }

            RadGrid_Project.MasterTableView.SortExpressions.Clear();
            RadGrid_Project.MasterTableView.GroupByExpressions.Clear();
            RadGrid_Project.EditIndexes.Clear();
            RadGrid_Project.Rebind();
        }
    }

    private string GetGridColumnSettings()
    {
        var columnSettings = string.Empty;
        List<ColumnSettings> lstColSetts = new List<ColumnSettings>();

        foreach (GridColumn column in RadGrid_Project.MasterTableView.OwnerGrid.Columns)
        {
            var colSett = new ColumnSettings();
            colSett.Name = column.UniqueName;
            colSett.Display = column.Display;
            colSett.Width = (int)column.HeaderStyle.Width.Value;
            colSett.OrderIndex = column.OrderIndex;
            lstColSetts.Add(colSett);
        }

        columnSettings = Newtonsoft.Json.JsonConvert.SerializeObject(lstColSetts);

        return columnSettings;
    }

    private string GetDefaultGridColumnSettingsFromDb()
    {
        var columnSettings = string.Empty;
        objPropUser.ConnConfig = HttpContext.Current.Session["config"].ToString();
        objPropUser.PageName = "ProjectWIP.aspx";
        objPropUser.GridId = "RadGrid_Project";
        var ds = objBL_User.GetDefaultGridCustomSettings(objPropUser);
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            columnSettings = ds.Tables[0].Rows[0][0].ToString();
        }

        return columnSettings;
    }

    private void DefineGridStructure()
    {
        DataSet ds = new DataSet();
        objPropUser.ConnConfig = HttpContext.Current.Session["config"].ToString();
        objPropUser.UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"].ToString());
        objPropUser.PageName = "ProjectWIP.aspx";
        objPropUser.GridId = "RadGrid_Project";
        ds = objBL_User.GetGridUserSettings(objPropUser);

        if (ds.Tables[0].Rows.Count > 0)
        {
            var columnSettings = ds.Tables[0].Rows[0][0].ToString();
            var columnsArr = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ColumnSettings>>(columnSettings);

            foreach (GridColumn column in RadGrid_Project.MasterTableView.OwnerGrid.Columns)
            {
                var clSetting = columnsArr.Where(t => t.Name.Equals(column.UniqueName, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
                if (clSetting != null)
                {
                    column.Display = clSetting.Display;
                    if (clSetting.Width != 0)
                        column.HeaderStyle.Width = clSetting.Width;

                    column.OrderIndex = clSetting.OrderIndex;
                }
            }
        }
    }

    public class ColumnSettings
    {
        public string Name { get; set; }
        public bool Display { get; set; }
        public int Width { get; set; }
        public int OrderIndex { get; set; }
    }

    #endregion
}
