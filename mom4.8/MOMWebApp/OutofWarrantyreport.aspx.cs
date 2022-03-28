using BusinessEntity;
using BusinessLayer;
using Microsoft.ApplicationBlocks.Data;
using Microsoft.Reporting.WebForms;
using System;
using System.Data;
using System.Web.UI;

public partial class OutofWarrantyreport : Page
{
    User objPropUser = new User();
    BL_User objBL_User = new BL_User();

    Customer objProp_Customer = new Customer();
    BL_Customer objBL_Customer = new BL_Customer();

    JobT objJob = new JobT();
    BL_Job objBL_Job = new BL_Job();
    int count = 0;

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

                txtfromDate.Text = DateTime.Now.AddDays(-30).Date.ToString("MM/dd/yyyy");
                txtToDate.Text = DateTime.Now.Date.ToString("MM/dd/yyyy");
                GetOutofWarrantyReport();

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
    private void GetOutofWarrantyReport()
    {
        try
        {
            DataSet dsCompany = new DataSet();
            DataSet dsJob = new DataSet();
            DateTime StartDate; DateTime EndDate; string ConnConfig;

            ConnConfig = Session["config"].ToString();

            if (txtfromDate.Text != "")
            {
                StartDate = Convert.ToDateTime(txtfromDate.Text);
            }
            else
            {
                StartDate = DateTime.Now.AddDays(-30).Date;

            }
            if (txtToDate.Text != "")
            {
                EndDate = Convert.ToDateTime(txtToDate.Text);
            }
            else
            {
                EndDate = DateTime.Now.Date;
            }

            dsJob = getOutofWarrantyreport(StartDate, EndDate, ConnConfig);
            objPropUser.ConnConfig = Session["config"].ToString();
            dsCompany = objBL_User.getControl(objPropUser);

            DataTable dtJob = dsJob.Tables[0];


            rvJob.LocalReport.DataSources.Clear();
            rvJob.LocalReport.DataSources.Add(new ReportDataSource("dsOutofWarrantyreport", dtJob));
            rvJob.LocalReport.DataSources.Add(new ReportDataSource("dtCompany", dsCompany.Tables[0]));

            string reportPath = "Reports/OutofWarrantyreport.rdlc";
            rvJob.LocalReport.ReportPath = reportPath;

            ReportParameter rpUser = new ReportParameter("Username", Session["User"].ToString());
            rvJob.LocalReport.SetParameters((new ReportParameter[] { rpUser }));
            rvJob.LocalReport.DisplayName = "Out of Warranty Report " + Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd");
            rvJob.LocalReport.Refresh();
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
        GetOutofWarrantyReport();
    }
    public DataSet getOutofWarrantyreport(DateTime StartDate, DateTime EndDate, string ConnConfig)
    {

        try
        {
            return SqlHelper.ExecuteDataset(ConnConfig, "spGetOutofWarrantyreport", StartDate, EndDate);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

}