using BusinessEntity;
using BusinessLayer;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Web;
using System.Web.UI;

public partial class JobActualBudget : System.Web.UI.Page
{
    User objPropUser = new User();

    BL_User objBL_User = new BL_User();

    Customer objProp_Customer = new Customer();

    BL_Customer objBL_Customer = new BL_Customer();

    JobT objJob = new JobT();

    BL_Job objBL_Job = new BL_Job();

    #region event

    #region PAGELOAD

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

                GetJobActualBudgetReport();
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    #endregion

    protected void lnkClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("Project.aspx");
    }

    #endregion

    #region Custom functions

    private void GetJobActualBudgetReport()
    {
        try
        {
            DataSet dsCompany = new DataSet();
            DataSet dsJob = new DataSet();

            objProp_Customer.ConnConfig = Session["config"].ToString();

            if (Session["PrintJobActualBudgetserachBy"] != null)
            {
                if(Session["PrintJobActualBudgetserachBy"].ToString() != "")
                {
                    objProp_Customer.SearchBy = Session["serachBy"].ToString();
                }
                else
                {
                    objProp_Customer.SearchBy = null;
                }
            }
            else
            {
                objProp_Customer.SearchBy = null;
            }

            if (Session["PrintJobActualBudgetsearchValue"] != null)
            {
                if (Session["PrintJobActualBudgetsearchValue"].ToString() != "")
                {
                    objProp_Customer.SearchValue = Session["searchValue"].ToString();
                }
                else
                {
                    objProp_Customer.SearchValue = null;
                }
            }
            else
            {
                objProp_Customer.SearchValue = null;
            }

            objProp_Customer.Range = Convert.ToInt16(Session["ddlDateRangeFieldforEditJob"].ToString());

            if (Session["txtfrmDtValforEditjob"].ToString() != "")
            {
                objProp_Customer.StartDate = Session["txtfrmDtValforEditjob"].ToString();
            }
            else
            {
                objProp_Customer.StartDate = null;

            }

            if (Session["txttoDtValforEditJob"].ToString() != "")
            {
                objProp_Customer.EndDate = Session["txttoDtValforEditJob"].ToString();
            }
            else
            {
                objProp_Customer.EndDate = null;

                objProp_Customer.JobType = null;
            }

            try
            {
                if (Session["JobListobjBL_Customer"] != null)
                {
                    objProp_Customer = (Customer)Session["JobListobjBL_Customer"];
                }

            }
            catch { }

            List<RetainFilter> filters = new List<RetainFilter>();

            if (Session["GridFilters"] != null)
            {
                filters = (List<RetainFilter>)Session["GridFilters"];
            }

            DataTable dtFilters = CreateFiltersToDataTable(filters);
            dsJob = objBL_Customer.getJobProject(objProp_Customer, dtFilters, new GeneralFunctions().GetSalesAsigned(), objProp_Customer.Status);

            objPropUser.ConnConfig = Session["config"].ToString();
            dsCompany = objBL_User.getControl(objPropUser);

            DataTable dtJob = dsJob.Tables[0];

            rvJob.LocalReport.DataSources.Clear();
            rvJob.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(ItemDetailsSubReportProcessing);
            rvJob.LocalReport.DataSources.Add(new ReportDataSource("dtJob", dtJob));
            rvJob.LocalReport.DataSources.Add(new ReportDataSource("dtCompany", dsCompany.Tables[0]));

            string reportPath = "Reports/JobActualBudget.rdlc";
            rvJob.LocalReport.ReportPath = reportPath;

            ReportParameter rpUser = new ReportParameter("Username", Session["User"].ToString());
            rvJob.LocalReport.SetParameters((new ReportParameter[] { rpUser }));
            rvJob.LocalReport.DisplayName = "Projects Result Actual Vs Budget Report " + Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd");
            rvJob.LocalReport.Refresh();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    private DataTable CreateFiltersToDataTable(List<RetainFilter> filters)
    {
        //create new table to add filter values.
        DataTable dtFilters = new DataTable();
        dtFilters.Clear();
        dtFilters.Columns.Add("Customer");
        dtFilters.Columns.Add("Tag");
        dtFilters.Columns.Add("ID");
        dtFilters.Columns.Add("fdesc");
        dtFilters.Columns.Add("Status");
        dtFilters.Columns.Add("Stage");
        dtFilters.Columns.Add("Company");
        dtFilters.Columns.Add("CType");
        dtFilters.Columns.Add("TemplateDesc");
        dtFilters.Columns.Add("Type");
        dtFilters.Columns.Add("SalesPerson");
        dtFilters.Columns.Add("Route");
        dtFilters.Columns.Add("NHour");
        dtFilters.Columns.Add("ContractPrice");
        dtFilters.Columns.Add("NotBilledYet");
        dtFilters.Columns.Add("NComm");
        dtFilters.Columns.Add("NRev");
        dtFilters.Columns.Add("NLabor");
        dtFilters.Columns.Add("NMat");
        dtFilters.Columns.Add("NOMat");
        dtFilters.Columns.Add("NCost");
        dtFilters.Columns.Add("NProfit");
        dtFilters.Columns.Add("NRatio");
        dtFilters.Columns.Add("RouteFilters");
        dtFilters.Columns.Add("StageFilters");
        dtFilters.Columns.Add("DepartmentFilters");
        dtFilters.Columns.Add("ProjectManagerUserName");
        dtFilters.Columns.Add("LocationType");
        dtFilters.Columns.Add("BuildingType");
        dtFilters.Columns.Add("TotalBudgetedExpense");
        dtFilters.Columns.Add("SupervisorUserName");
        dtFilters.Columns.Add("OpenARBalance");
        dtFilters.Columns.Add("OpenAPBalance");
        dtFilters.Columns.Add("ExpectedClosingDate");
        dtFilters.Columns.Add("Estimate");

        DataRow dtFiltersRow = dtFilters.NewRow();

        if (filters != null && filters.Count > 0)
        {
            decimal filterValue = -1900;

            dtFiltersRow["NHour"] = filterValue;
            dtFiltersRow["TotalBudgetedExpense"] = filterValue;
            dtFiltersRow["NRatio"] = filterValue;
            dtFiltersRow["ContractPrice"] = filterValue;
            dtFiltersRow["NComm"] = filterValue;
            dtFiltersRow["NotBilledYet"] = filterValue;
            dtFiltersRow["NRev"] = filterValue;
            dtFiltersRow["NLabor"] = filterValue;
            dtFiltersRow["NMat"] = filterValue;
            dtFiltersRow["NOMat"] = filterValue;
            dtFiltersRow["NCost"] = filterValue;
            dtFiltersRow["NProfit"] = filterValue;
            foreach (var items in filters)
            {
                if (!string.IsNullOrEmpty(items.FilterValue) && !string.IsNullOrWhiteSpace(items.FilterValue))
                {

                    if (items.FilterColumn == "LocationType")
                    {
                        dtFiltersRow["LocationType"] = items.FilterValue;
                    }

                    if (items.FilterColumn == "BuildingType")
                    {
                        dtFiltersRow["BuildingType"] = items.FilterValue;
                    }

                    if (items.FilterColumn == "Customer")
                    {
                        dtFiltersRow["Customer"] = items.FilterValue;
                    }

                    if (items.FilterColumn == "Tag")
                    {
                        dtFiltersRow["Tag"] = items.FilterValue;
                    }

                    if (items.FilterColumn == "ProjectManagerUserName")
                    {
                        dtFiltersRow["ProjectManagerUserName"] = items.FilterValue;
                    }

                    if (items.FilterColumn == "SupervisorUserName")
                    {
                        dtFiltersRow["SupervisorUserName"] = items.FilterValue;
                    }
                    if (items.FilterColumn == "OpenARBalance")
                    {
                        dtFiltersRow["OpenARBalance"] = items.FilterValue;
                    }
                    if (items.FilterColumn == "OpenAPBalance")
                    {
                        dtFiltersRow["OpenAPBalance"] = items.FilterValue;
                    }
                    /// Int Filter
                    int FilterValue = 0; string[] filterArrayValue;

                    StringBuilder filteredQuery = new StringBuilder();

                    if (items.FilterColumn == "ID")
                    {
                        filterArrayValue = items.FilterValue.ToString().Split(',');
                        foreach (var filtered in filterArrayValue)
                        {
                            if (int.TryParse(filtered, out FilterValue))
                            {
                                if (filteredQuery.Length == 0)
                                {
                                    filteredQuery.Append(filtered);
                                }
                                else
                                {
                                    filteredQuery.Append("," + filtered);
                                }
                            }


                        }
                        dtFiltersRow["ID"] = filteredQuery.ToString();
                    }

                    if (items.FilterColumn == "fdesc")
                    {
                        dtFiltersRow["fdesc"] = items.FilterValue;
                    }
                    if (items.FilterColumn == "Status")
                    {
                        dtFiltersRow["Status"] = items.FilterValue;
                    }
                    if (items.FilterColumn == "Stage")
                    {
                        dtFiltersRow["Stage"] = items.FilterValue;
                    }
                    if (items.FilterColumn == "Company")
                    {
                        dtFiltersRow["Company"] = items.FilterValue;
                    }
                    if (items.FilterColumn == "CType")
                    {
                        dtFiltersRow["CType"] = items.FilterValue;
                    }
                    if (items.FilterColumn == "TemplateDesc")
                    {
                        dtFiltersRow["TemplateDesc"] = items.FilterValue;
                    }
                    if (items.FilterColumn == "Department")
                    {
                        dtFiltersRow["Type"] = items.FilterValue;
                    }
                    if (items.FilterColumn == "Salesperson")
                    {
                        dtFiltersRow["SalesPerson"] = items.FilterValue;
                    }
                    if (items.FilterColumn == "DRoute")
                    {
                        dtFiltersRow["Route"] = items.FilterValue;
                    }

                    if (items.FilterColumn == "LocationType")
                    {
                        dtFiltersRow["LocationType"] = items.FilterValue;
                    }

                    if (items.FilterColumn == "BuildingType")
                    {
                        dtFiltersRow["BuildingType"] = items.FilterValue;
                    }

                    if (items.FilterColumn == "ExpectedClosingDate")
                    {
                        dtFiltersRow["ExpectedClosingDate"] = items.FilterValue;
                    }

                    filterValue = -1900;

                    if (items.FilterColumn == "ContractPrice" && decimal.TryParse(items.FilterValue, out filterValue))
                    {
                        dtFiltersRow["ContractPrice"] = filterValue;
                    }

                    filterValue = -1900;

                    if (items.FilterColumn == "NotBilledYet" && decimal.TryParse(items.FilterValue, out filterValue))
                    {
                        dtFiltersRow["NotBilledYet"] = filterValue;
                    }

                    filterValue = -1900;

                    if (items.FilterColumn == "NComm" && decimal.TryParse(items.FilterValue, out filterValue))
                    {
                        dtFiltersRow["NComm"] = filterValue;
                    }

                    filterValue = -1900;

                    if (items.FilterColumn == "NRev" && decimal.TryParse(items.FilterValue, out filterValue))
                    {
                        dtFiltersRow["NRev"] = filterValue;
                    }

                    filterValue = -1900;

                    if (items.FilterColumn == "NLabor" && decimal.TryParse(items.FilterValue, out filterValue))
                    {
                        dtFiltersRow["NLabor"] = filterValue;
                    }

                    filterValue = -1900;

                    if (items.FilterColumn == "NMat" && decimal.TryParse(items.FilterValue, out filterValue))
                    {
                        dtFiltersRow["NMat"] = filterValue;
                    }

                    filterValue = -1900;

                    if (items.FilterColumn == "NOMat" && decimal.TryParse(items.FilterValue, out filterValue))
                    {
                        dtFiltersRow["NOMat"] = filterValue;
                    }

                    filterValue = -1900;

                    if (items.FilterColumn == "NCost" && decimal.TryParse(items.FilterValue, out filterValue))
                    {
                        dtFiltersRow["NCost"] = filterValue;
                    }

                    filterValue = -1900;

                    if (items.FilterColumn == "NProfit" && decimal.TryParse(items.FilterValue, out filterValue))
                    {
                        dtFiltersRow["NProfit"] = filterValue;
                    }

                    filterValue = -1900;

                    if (items.FilterColumn == "NRatio" && decimal.TryParse(items.FilterValue, out filterValue))
                    {
                        dtFiltersRow["NRatio"] = filterValue;
                    }

                    filterValue = -1900;

                    if (items.FilterColumn == "TotalBudgetedExpense" && decimal.TryParse(items.FilterValue, out filterValue))
                    {
                        dtFiltersRow["TotalBudgetedExpense"] = filterValue;
                    }

                    filterValue = -1900;

                    if (items.FilterColumn == "NHour" && decimal.TryParse(items.FilterValue, out filterValue))
                    {
                        dtFiltersRow["NHour"] = filterValue;
                    }

                    filterValue = -1900;

                    if (items.FilterColumn == "OpenARBalance" && decimal.TryParse(items.FilterValue, out filterValue))
                    {
                        dtFiltersRow["OpenARBalance"] = filterValue;
                    }

                    if (items.FilterColumn == "OpenAPBalance" && decimal.TryParse(items.FilterValue, out filterValue))
                    {
                        dtFiltersRow["OpenAPBalance"] = filterValue;
                    }
                }
            }
        }

        if (Session["PROJ_RouteFilters"] != null && Session["PROJ_RouteFilters"].ToString() != string.Empty)
        {
            dtFiltersRow["RouteFilters"] = Session["PROJ_RouteFilters"].ToString();
        }

        if (Session["PROJ_StageFilters"] != null && Session["PROJ_StageFilters"].ToString() != string.Empty)
        {
            dtFiltersRow["StageFilters"] = Session["PROJ_StageFilters"].ToString();
        }

        if (Session["PROJ_DepartmentFilters"] != null && Session["PROJ_DepartmentFilters"].ToString() != string.Empty)
        {
            dtFiltersRow["DepartmentFilters"] = Session["PROJ_DepartmentFilters"].ToString();
        }

        dtFilters.Rows.Add(dtFiltersRow);

        return dtFilters;
    }

    private void ItemDetailsSubReportProcessing(object sender, SubreportProcessingEventArgs e)
    {
        try
        {
            DataTable dtItem = new DataTable();

            objJob.ConnConfig = Session["config"].ToString();
            objJob.Job = Convert.ToInt32(e.Parameters[0].Values[0]);
            objJob.PageIndex = 1;
            objJob.PageSize = 1000;

            if (!string.IsNullOrEmpty(txtfromDate.Text) && !string.IsNullOrEmpty(txtToDate.Text))
            {
                objJob.StartDate = Convert.ToDateTime(txtfromDate.Text);
                objJob.EndDate = Convert.ToDateTime(txtToDate.Text);
            }

            DataSet dsItems = objBL_Job.GetJobCostByJob(objJob);

            if (dsItems.Tables[0].Rows.Count == 0)
            {
                #region Add Blank row               

                DataRow dr = dsItems.Tables[0].NewRow();
                dr["ID"] = objJob.Job;
                dr["fDesc"] = string.Empty;
                dr["JobType"] = string.Empty;
                dr["Code"] = string.Empty;
                dr["Phase"] = 0;
                dr["BHours"] = 0;
                dr["Actual"] = 0;
                dr["Comm"] = 0;
                dr["Total"] = 0;
                dr["Budget"] = 0;
                dr["Variance"] = 0;
                dr["Ratio"] = 0;

                dsItems.Tables[0].Rows.Add(dr);

                dsItems.Tables[0].AcceptChanges();
                #endregion
            }

            dtItem = dsItems.Tables[0];

            ReportDataSource rdsItems = new ReportDataSource("dtJobItems", dtItem);

            e.DataSources.Add(rdsItems);
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);

            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    #endregion

    protected void lnkSearch_Click(object sender, EventArgs e)
    {
        GetJobActualBudgetReport();
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
}