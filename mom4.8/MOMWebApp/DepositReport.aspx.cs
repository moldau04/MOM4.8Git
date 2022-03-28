using BusinessEntity;
using BusinessLayer;
using Microsoft.Reporting.WebForms;
using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
public partial class DepositReport : System.Web.UI.Page
{
    Invoices _objInvoices = new Invoices();
    BL_Invoice _objBLInvoices = new BL_Invoice();

    User objPropUser = new User();
    BL_Report objBL_Report = new BL_Report();
    BL_User objBL_User = new BL_User();

    Chart _objChart = new Chart();
    BL_Chart _objBLChart = new BL_Chart();
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
                GetDepositReport();
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    protected void hideModalPopupViaServerConfirm_Click(object sender, EventArgs e)
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
                mail.Title = "COMPLETED TICKET  LISTING - PAYROLL DOLLARS - Summary";
                if (txtBody.Text.Trim() != string.Empty)
                {
                    mail.Text = txtBody.Text.Replace(Environment.NewLine, "<BR/>");
                }
                else
                {
                    mail.Text = "This is report email sent from Mobile Office Manager. Please find the COMPLETED TICKET  LISTING - PAYROLL DOLLARS - Summary attached.";
                }
                //mail.AttachmentFiles.Add(ExportReportToPDF("Report_" + objGen.generateRandomString(10) + ".pdf"));
                mail.attachmentBytes = ExportReportToPDF("");
                mail.FileName = "COMPLETEDTICKETLISTING.pdf";

                mail.DeleteFilesAfterSend = true;
                mail.RequireAutentication = false;

                mail.Send();
                //this.programmaticModalPopup.Hide();
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "noty({text: 'Mail sent successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);

            }
            catch (Exception ex)
            {
                string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);

            }
        }
    }
    protected void lnkClose_Click(object sender, EventArgs e)
    {
        //Response.Redirect("home.aspx");
        Response.Redirect("AddDeposit.aspx");
    }
    protected void lnkSearch_Click(object sender, EventArgs e)
    {
        GetDepositReport();
    }
    protected void rvDepositReport_ReportRefresh(object sender, System.ComponentModel.CancelEventArgs e)
    {
        GetDepositReport();
    }

    private void GetDepositReport()
    {
        try
        {          

            DateTime _startDate = DateTime.Now;
            DateTime _endDate = DateTime.Now;

            string RefNo = (string)Session["RefNo"];

            //_startDate = (DateTime)Session["stardate"];
            //_endDate = (DateTime)Session["enddate"];
            DataSet dsC = new DataSet();
            objPropUser.ConnConfig = Session["config"].ToString();
            dsC = objBL_User.getControl(objPropUser);


            //DataSet ds = _objBLChart.GetAccountLedgerReport(_objChart);
            DataSet _depositDetails = (DataSet)Session["Depositdetails"];
            DataSet _depositHead = (DataSet)Session["DepositHead"];
            //DataSet ds = (DataSet)Session["AccountLedger"];

            rvDepositReport.LocalReport.DataSources.Clear();

            ReportParameter rpUser = new ReportParameter("paramUser", Session["User"].ToString());
            ReportParameter rpStartDate = new ReportParameter("paramSdate", _startDate.ToString("MM/dd/yyyy"));
            ReportParameter rpEndDate = new ReportParameter("paramEdate", _endDate.ToString("MM/dd/yyyy"));
            ReportParameter rpRefNo = new ReportParameter("RefNo", RefNo);

            rvDepositReport.LocalReport.DataSources.Add(new ReportDataSource("dsGLAccount", _depositDetails.Tables[1]));
            rvDepositReport.LocalReport.DataSources.Add(new ReportDataSource("dsDepositDetails", _depositDetails.Tables[0]));
            rvDepositReport.LocalReport.DataSources.Add(new ReportDataSource("dsDepositHead", _depositHead.Tables[0]));
            rvDepositReport.LocalReport.DataSources.Add(new ReportDataSource("dsCompany", dsC.Tables[0]));


            String department = "";
            if (_depositDetails.Tables[0].Rows.Count == 1)
            {
                BL_Report _objBLReport = new BL_Report();
                DataSet ds = new DataSet();
                ds = _objBLReport.GetDepartmentByReceiptID(Session["config"].ToString(), Convert.ToInt32(_depositDetails.Tables[0].Rows[0]["ID"]));
                if (ds.Tables.Count > 0)
                {
                    department = "";
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        department = ds.Tables[0].Rows[0]["type"].ToString();
                    }                 

                }
            }
            ReportParameter paramDepartment = new ReportParameter("paramDepartment", department);
            rvDepositReport.LocalReport.ReportPath = "Reports/DepositReport.rdlc";
            rvDepositReport.LocalReport.EnableExternalImages = true;
            rvDepositReport.LocalReport.SetParameters((new ReportParameter[] { rpUser, rpStartDate, rpEndDate,rpRefNo, paramDepartment }));
            //rvDepositReport.LocalReport.SetParameters((new ReportParameter[] { rpUser }));
            rvDepositReport.LocalReport.DisplayName = "Deposit Report " + Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd");
            //rvBills.LocalReport.SetParameters(new ReportParameter[] { rpStartDate, rpEndDate });
            rvDepositReport.LocalReport.Refresh();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    private byte[] ExportReportToPDF(string reportName)
    {
        Warning[] warnings;
        string[] streamids;
        string mimeType;
        string encoding;
        string filenameExtension;
        byte[] bytes = rvDepositReport.LocalReport.Render(
            "PDF", null, out mimeType, out encoding, out filenameExtension,
             out streamids, out warnings);

        return bytes;
    }
}