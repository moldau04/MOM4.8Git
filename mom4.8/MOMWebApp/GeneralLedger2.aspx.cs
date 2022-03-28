using BusinessEntity;
using BusinessLayer;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
public partial class GeneralLedger2 : System.Web.UI.Page
{
    #region "Variables"

    Chart _objChart = new Chart();
    BL_Chart _objBLChart = new BL_Chart();

    User objPropUser = new User();
    BL_Report objBL_Report = new BL_Report();
    BL_User objBL_User = new BL_User();

    Transaction _objTrans = new Transaction();
    int count = 0;
    #endregion

    #region Events
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
                FillAccount();
                //int year = DateTime.Now.Year;
                //DateTime firstDay = new DateTime(year, 1, 1);
                //DateTime lastDay = new DateTime(year, 1, 31);
                //txtStartDate.Text = firstDay.Date.ToString("MM/dd/yyyy");
                DateTime _now = DateTime.Now;
                var _startDate = new DateTime(_now.Year, _now.Month, 1);
                txtStartDate.Text = _startDate.ToShortDateString();

                txtEndDate.Text = DateTime.Now.Date.ToString("MM/dd/yyyy");
                GetGLReport();
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    protected void lnkClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("home.aspx");
    }
    protected void lnkSearch_Click(object sender, EventArgs e)
    {
        GetGLReport();
    }
    protected void rdSumDetail_CheckedChanged(object sender, EventArgs e)
    {
        GetGLReport();
    }
    #endregion

    #region Custom Functions
    private void GetGLReport()
    {
        try
        {
            _objChart.ConnConfig = Session["config"].ToString();

            #region start-end date

            int AcctID;
            if (ddlAccount.SelectedItem.Value != null)
            {
                AcctID = Convert.ToInt32(ddlAccount.SelectedItem.Value);
            }
            else
            {
                AcctID = 0;
            }

            _objChart.ID = AcctID;
            DateTime sdate = DateTime.Now;
            DateTime edate = DateTime.Now;

            if (string.IsNullOrEmpty(txtStartDate.Text.ToString()))
            {
                if (!string.IsNullOrEmpty(txtStartDate.Text.ToString()))
                    _objChart.StartDate = Convert.ToDateTime(txtStartDate.Text);
                else
                    _objChart.StartDate = DateTime.Now.Date;
            }
            else
            {
                _objChart.StartDate = Convert.ToDateTime(txtStartDate.Text);
            }
            if (string.IsNullOrEmpty(txtEndDate.Text.ToString()))
            {
                if (!string.IsNullOrEmpty(txtEndDate.Text.ToString()))
                    _objChart.EndDate = Convert.ToDateTime(txtEndDate.Text);
                else
                    _objChart.EndDate = DateTime.Now.Date;
            }
            else
            {
                _objChart.EndDate = Convert.ToDateTime(txtEndDate.Text);
            }
            //_objChart.GroupBy = Convert.ToInt32(ddlGroupBy.SelectedValue);
            #endregion

            sdate = _objChart.StartDate;
            edate = _objChart.EndDate;
            DataSet dsC = new DataSet();
            objPropUser.ConnConfig = Session["config"].ToString();
            dsC = objBL_User.getControl(objPropUser);

            DataSet ds = _objBLChart.GetGeneralLedgerByDate(_objChart);
            //DataSet ds = _objBLChart.GetGL(_objChart);
            rvGeneralLedger.LocalReport.DataSources.Clear();

            ViewState["GL"] = ds.Tables[0];
            int valSumDetail = 0;

            if (rdDetail.Checked.Equals(true))
            {
                valSumDetail = 1;
            }
            ReportParameter rpUser = new ReportParameter("paramUser", Session["User"].ToString());
            ReportParameter rpAccount = new ReportParameter("paramAccount", AcctID.ToString());
            ReportParameter rpStartDate = new ReportParameter("paramStartDate", sdate.ToString());
            ReportParameter rpEndDate = new ReportParameter("paramEndDate", edate.ToString());
            ReportParameter rpSumDetail = new ReportParameter("paramSummaryDetail", valSumDetail.ToString());
            //ReportParameter rpGroup = new ReportParameter("paramGroup", _objChart.GroupBy.ToString());

            //rvGeneralLedger.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(GLItemSubReportProcessing);
            rvGeneralLedger.LocalReport.DataSources.Add(new ReportDataSource("dsGL", ds.Tables[0]));
            //rvGeneralLedger.LocalReport.DataSources.Add(new ReportDataSource("dsGL", ds.Tables[1]));
            rvGeneralLedger.LocalReport.DataSources.Add(new ReportDataSource("dsCompany", dsC.Tables[0]));
            rvGeneralLedger.LocalReport.ReportPath = "Reports/GeneralLedger2.rdlc";
            rvGeneralLedger.LocalReport.DisplayName = "General Ledger " + sdate.ToString("MMM") + " to " + edate.ToString("MMM") + " " + edate.Year.ToString();
            rvGeneralLedger.LocalReport.EnableExternalImages = true;
            rvGeneralLedger.LocalReport.SetParameters((new ReportParameter[] { rpUser, rpAccount, rpStartDate, rpEndDate, rpSumDetail }));
            rvGeneralLedger.LocalReport.Refresh();

        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    //private void GLItemSubReportProcessing(object sender, SubreportProcessingEventArgs e)
    //{
    //    try
    //    {
    //        DataTable dt = (DataTable)ViewState["GL"];

    //        _objChart.ConnConfig = Session["config"].ToString();
    //        if (string.IsNullOrEmpty(txtStartDate.Text.ToString()))
    //        {
    //            if (!string.IsNullOrEmpty(txtStartDate.Text.ToString()))
    //                _objChart.StartDate = Convert.ToDateTime(txtStartDate.Text);
    //            else
    //                _objChart.StartDate = DateTime.Now.Date;
    //        }
    //        else
    //        {
    //            _objChart.StartDate = Convert.ToDateTime(txtStartDate.Text);
    //        }
    //        if (string.IsNullOrEmpty(txtEndDate.Text.ToString()))
    //        {
    //            if (!string.IsNullOrEmpty(txtEndDate.Text.ToString()))
    //                _objChart.EndDate = Convert.ToDateTime(txtEndDate.Text);
    //            else
    //                _objChart.EndDate = DateTime.Now.Date;
    //        }
    //        else
    //        {
    //            _objChart.EndDate = Convert.ToDateTime(txtEndDate.Text);
    //        }
    //        //_objChart.GroupBy = Convert.ToInt32(ddlGroupBy.SelectedValue);
    //        _objChart.ID = Convert.ToInt32(dt.Rows[count]["ID"]);
    //        DataSet ds = _objBLChart.GetGeneralLedgerByDate(_objChart);

    //        if (dt.Rows.Count > 0)
    //        {
    //            ReportDataSource rdsGL = new ReportDataSource("dsGLA", ds.Tables[0]);
    //            //ReportDataSource rdsGroup = new ReportDataSource("dsGLA", ds.Tables[0]);

    //            e.DataSources.Add(rdsGL);
    //        }
    //        if (count == dt.Rows.Count - 1)
    //        {
    //            ViewState["GL"] = null;
    //        }
    //        count++;
    //    }
    //    catch (Exception ex)
    //    {
    //        string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
    //        ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
    //    }
    //}
    protected void rvGeneralLedger_ReportRefresh(object sender, System.ComponentModel.CancelEventArgs e)
    {
        GetGLReport();
    }
    private void FillAccount()
    {
        try
        {
            _objChart.ConnConfig = Session["config"].ToString();
            DataSet _dsAccount = _objBLChart.GetAllAccountDetails(_objChart);

            //ddlVendor.Items.Add(new ListItem(" "));
            if (_dsAccount.Tables[0].Rows.Count > 0)
            {
                ddlAccount.Items.Add(new ListItem("All", "0"));
                // ddlAccount.Items.Add(new ListItem("All", "1"));
                ddlAccount.AppendDataBoundItems = true;

                ddlAccount.DataSource = _dsAccount;
                ddlAccount.DataValueField = "ID";
                ddlAccount.DataTextField = "fDesc";
                ddlAccount.DataBind();
            }
            else
            {
                ddlAccount.Items.Add(new ListItem("No data found", "0"));
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true})", true);
        }
    }
    #endregion

}