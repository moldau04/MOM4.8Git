using BusinessEntity;
using BusinessLayer;
using Microsoft.Reporting.WebForms;
using System;
using System.Configuration;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class SalesTax2Report : System.Web.UI.Page
{
    #region "Variables"

    BusinessEntity.Invoices _objInvoices = new BusinessEntity.Invoices();
    BL_Invoice _objBLInvoices = new BL_Invoice();

    User objPropUser = new User();
    BL_Report objBL_Report = new BL_Report();
    BL_User objBL_User = new BL_User();

    Transaction _objTrans = new Transaction();

    #endregion
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

                int year = DateTime.Now.Year;
                DateTime firstDay = new DateTime(year, 1, 1);
                txtStartDate.Text = firstDay.Date.ToString("MM/dd/yyyy");
                txtEndDate.Text = DateTime.Now.Date.ToString("MM/dd/yyyy");
                GetSalesTaxReport();
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
        Response.Redirect("invoices.aspx?fil=1");
    }
    protected void lnkSearch_Click(object sender, EventArgs e)
    {
        GetSalesTaxReport();
    }
    protected void rvSalesTax_ReportRefresh(object sender, System.ComponentModel.CancelEventArgs e)
    {
        GetSalesTaxReport();
    }

    private void GetSalesTaxReport()
    {
        try
        {
            _objInvoices.ConnConfig = Session["config"].ToString();

            #region start-end date

            DateTime _startDate = DateTime.Now;
            DateTime _endDate = DateTime.Now;

            if (string.IsNullOrEmpty(txtStartDate.Text.ToString()))
            {
                if (!string.IsNullOrEmpty(txtStartDate.Text.ToString()))
                    _objInvoices.StartDate = Convert.ToDateTime(txtStartDate.Text);
                else
                    _objInvoices.StartDate = DateTime.Now.Date;
            }
            else
            {
                _objInvoices.StartDate = Convert.ToDateTime(txtStartDate.Text);
            }
            if (string.IsNullOrEmpty(txtEndDate.Text.ToString()))
            {
                if (!string.IsNullOrEmpty(txtEndDate.Text.ToString()))
                    _objInvoices.EndDate = Convert.ToDateTime(txtEndDate.Text);
                else
                    _objInvoices.EndDate = DateTime.Now.Date;
            }
            else
            {
                _objInvoices.EndDate = Convert.ToDateTime(txtEndDate.Text);
            }
            #endregion

            _startDate = _objInvoices.StartDate;
            _endDate = _objInvoices.EndDate;
            DataSet dsC = new DataSet();
            objPropUser.ConnConfig = Session["config"].ToString();
            dsC = objBL_User.getControl(objPropUser);


            DataSet ds = _objBLInvoices.GetSalesTax2(_objInvoices);
            rvSalesTax.LocalReport.DataSources.Clear();

            ReportParameter rpUser = new ReportParameter("paramUser", Session["User"].ToString());
            ReportParameter rpStartDate = new ReportParameter("paramStartDate", _startDate.ToString());
            ReportParameter rpEndDate = new ReportParameter("paramEndDate", _endDate.ToString());

            var customerName = ConfigurationManager.AppSettings["CustomerName"].ToString();

            rvSalesTax.LocalReport.DataSources.Add(new ReportDataSource("dsSTax", ds.Tables[0]));
            rvSalesTax.LocalReport.DataSources.Add(new ReportDataSource("dsCompany", dsC.Tables[0]));
            if (customerName.Equals("SECO"))
            {
                rvSalesTax.Width = Unit.Pixel(1050);
                rvSalesTax.LocalReport.ReportPath = "Reports/SecoSalesTax2Report.rdlc";
            }
            else
            {
                rvSalesTax.Width = Unit.Pixel(1050);
                rvSalesTax.LocalReport.ReportPath = "Reports/SecoSalesTax2Report.rdlc";
            }
            rvSalesTax.LocalReport.EnableExternalImages = true;
            rvSalesTax.LocalReport.SetParameters((new ReportParameter[] { rpUser, rpStartDate, rpEndDate }));
            rvSalesTax.LocalReport.DisplayName = "Sales Tax Summary Report " + Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd");
            //rvBills.LocalReport.SetParameters(new ReportParameter[] { rpStartDate, rpEndDate });
            rvSalesTax.LocalReport.Refresh();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
}