using BusinessEntity;
using BusinessLayer;
using System.Data;
using System;
using Stimulsoft.Report;
using System.IO;

public partial class OpportunityReport : System.Web.UI.Page
{
    private const string ASCENDING = " ASC";
    private const string DESCENDING = " DESC";
    Customer objProp_Customer = new Customer();
    BL_Customer objBL_Customer = new BL_Customer();
    User objPropUser = new User();
    BL_User objBL_User = new BL_User();
    GeneralFunctions objGeneralFunctions = new GeneralFunctions();
    BL_Report bL_Report = new BL_Report();

    Contracts objContract = new Contracts();
    BL_Contracts objBL_Contracts = new BL_Contracts();

    BL_ReportsData objBL_ReportsData = new BL_ReportsData();
    BusinessEntity.User objProp_User = new BusinessEntity.User();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userid"] == null)
        {
            Response.Redirect("login.aspx");
        }
    }

    protected void StiWebViewerOpportunity_GetReport(object sender, Stimulsoft.Report.Web.StiReportDataEventArgs e)
    {
        e.Report = LoadReport();
    }


    private StiReport LoadReport()
    {
        try
        {
            string reportPathStimul = Server.MapPath("StimulsoftReports/OpenOpportunitiesReport.mrt");

            StiReport report = new StiReport();
            report.Load(reportPathStimul);
            //report.Compile();

            //Get data
            var connString = Session["config"].ToString();
            objPropUser.ConnConfig = connString;

            DataSet companyInfo = new DataSet();
            companyInfo = bL_Report.GetCompanyDetails(Session["config"].ToString());

            report.RegData("CompanyDetails", companyInfo.Tables[0]);

            DataTable ds = (DataTable)Session["opps"];
            if (ds != null)
            {
                DataTable dtOpps = ds;
                if (!dtOpps.Columns.Contains("Remarks1"))
                    dtOpps.Columns.Add("Remarks1");

                string todoListAsString = string.Empty;
                for (int i = 0; i < dtOpps.Rows.Count; i++)
                {
                    var opportunityId = Convert.ToInt32(dtOpps.Rows[i]["ID"]);
                    objProp_Customer.ConnConfig = Session["config"].ToString();
                    objProp_Customer.OpportunityID = opportunityId;
                    var todoItems = objBL_Customer.GetTasks(objProp_Customer);
                    if (todoItems != null)
                    {
                        todoListAsString = "";
                        for (int j = 0; j < todoItems.Tables[0].Rows.Count; j++)
                        {
                            todoListAsString += todoItems.Tables[0].Rows[j]["ToDoItem"].ToString() + " Due Date: " + DateTime.Parse(todoItems.Tables[0].Rows[j]["DueDate"].ToString()).ToString("MM/dd/yyyy");
                            todoListAsString += ", ";
                        }
                    }
                    if (todoListAsString.Length > 2)
                        todoListAsString = todoListAsString.Substring(0, todoListAsString.Length - 2);
                    if (!string.IsNullOrEmpty(todoListAsString))
                        dtOpps.Rows[i]["Remarks1"] = todoListAsString;
                    else
                        dtOpps.Rows[i]["Remarks1"] = "No open tasks found for this Opportunity";
                }

                objPropUser.ConnConfig = Session["config"].ToString();
                var dsC = objBL_User.getControl(objPropUser);

                DataSet dsOpportunities = new DataSet();
                DataTable dtOpportunities = dtOpps.Copy();
                dtOpportunities.TableName = "Opportunities";
                dsOpportunities.Tables.Add(dtOpportunities);
                dsOpportunities.DataSetName = "dsOpportunities";

                report.RegData("Opportunities", dsOpportunities);
                report.RegData("Ticket_Company", dsC.Tables[0]);
                report.Render();
            }
            return report;
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            return null;
        }
    }

    protected void StiWebViewerOpportunity_GetReportData(object sender, Stimulsoft.Report.Web.StiReportDataEventArgs e)
    {
        
    }

    protected void lnkClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("opportunity.aspx?fil=1");
    }
}