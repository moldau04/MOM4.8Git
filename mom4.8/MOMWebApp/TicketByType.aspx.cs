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
public partial class TicketByType : System.Web.UI.Page
{

    PJ _objPJ = new PJ();
    BL_ReportsData _objBLTicket = new BL_ReportsData();

    User objPropUser = new User();
    BL_Report objBL_Report = new BL_Report();
    BL_User objBL_User = new BL_User();
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
                DateTime today = DateTime.Today;
                int year = DateTime.Now.Year;
                DateTime firstDay = new DateTime(year, today.Month, 1);
                txtStartDate.Text = firstDay.Date.ToString("MM/dd/yyyy");
                txtEndDate.Text = DateTime.Now.Date.ToString("MM/dd/yyyy");
                GetTicketReport();
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
        Response.Redirect("Etimesheet.aspx?f=c");
    }
    protected void lnkSearch_Click(object sender, EventArgs e)
    {
        GetTicketReport();
    }
    protected void rvTicketList_ReportRefresh(object sender, System.ComponentModel.CancelEventArgs e)
    {
        GetTicketReport();
    }

    private void GetTicketReport()
    {
        try
        {
            _objPJ.ConnConfig = Session["config"].ToString();

            #region start-end date

            DateTime _startDate = DateTime.Now;
            DateTime _endDate = DateTime.Now;

            if (string.IsNullOrEmpty(txtStartDate.Text.ToString()))
            {
                if (!string.IsNullOrEmpty(txtStartDate.Text.ToString()))
                    _objPJ.StartDate = Convert.ToDateTime(txtStartDate.Text);
                else
                    _objPJ.StartDate = DateTime.Now.Date;
            }
            else
            {
                _objPJ.StartDate = Convert.ToDateTime(txtStartDate.Text);
            }
            if (string.IsNullOrEmpty(txtEndDate.Text.ToString()))
            {
                if (!string.IsNullOrEmpty(txtEndDate.Text.ToString()))
                    _objPJ.EndDate = Convert.ToDateTime(txtEndDate.Text);
                else
                    _objPJ.EndDate = DateTime.Now.Date;
            }
            else
            {
                _objPJ.EndDate = Convert.ToDateTime(txtEndDate.Text);
            }
            #endregion

            _startDate = _objPJ.StartDate;
            _endDate = _objPJ.EndDate;
            DataSet dsC = new DataSet();
            objPropUser.ConnConfig = Session["config"].ToString();
            dsC = objBL_User.getControl(objPropUser);


            DataSet ds = _objBLTicket.GetTicketList(_objPJ);
            int valExpCol = 0;
            if (rdExpandAll.Checked.Equals(true))
            {
                valExpCol = 1;
            }
            rvTicketList.LocalReport.DataSources.Clear();

            ReportParameter rpUser = new ReportParameter("paramUser", Session["User"].ToString());
            ReportParameter rpStartDate = new ReportParameter("paramStartDate", _startDate.ToString());
            ReportParameter rpEndDate = new ReportParameter("paramEndDate", _endDate.ToString());
            ReportParameter rpExpColl = new ReportParameter("paramExpCllAll", valExpCol.ToString());

            rvTicketList.LocalReport.DataSources.Add(new ReportDataSource("ds_CTL", ds.Tables[0]));
            rvTicketList.LocalReport.DataSources.Add(new ReportDataSource("dsCompany", dsC.Tables[0]));
            rvTicketList.LocalReport.ReportPath = "Reports/TicketListing.rdlc";
            rvTicketList.LocalReport.EnableExternalImages = true;
            rvTicketList.LocalReport.SetParameters((new ReportParameter[] { rpUser, rpStartDate, rpEndDate, rpExpColl }));
            //rvBills.LocalReport.SetParameters(new ReportParameter[] { rpStartDate, rpEndDate });
            rvTicketList.LocalReport.Refresh();
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
        byte[] bytes = rvTicketList.LocalReport.Render(
            "PDF", null, out mimeType, out encoding, out filenameExtension,
             out streamids, out warnings);

        return bytes;
    }
    protected void rdExpCollAll_CheckedChanged(object sender, EventArgs e)
    {
        GetTicketReport();
    }

}
