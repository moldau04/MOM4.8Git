using Microsoft.Reporting.WebForms;
using System;
using System.Data;
using System.Web.UI;
using BusinessLayer;
using BusinessEntity;
public partial class MaddenServiceAgreement : System.Web.UI.Page
{
    Customer objProp_Customer = new Customer();
    BL_Customer objBL_Customer = new BL_Customer();

    BusinessEntity.User objPropUser = new BusinessEntity.User();
    BL_User objBL_User = new BL_User();

    Rol _objJob = new Rol();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userid"] == null)
        {
            Response.Redirect("login.aspx");
        }
        if (!IsPostBack)
        {
            GetEstimateAgreement();
        }
    }
    protected void rvServiceAgreement_ReportRefresh(object sender, System.ComponentModel.CancelEventArgs e)
    {
        GetEstimateAgreement();
    }

    protected void hideModalPopupViaServerConfirm_Click(object sender, EventArgs e)
    {

    }
    protected void lnkClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("home.aspx");
    }
    private void GetEstimateAgreement()
    {
        try
        {

            objProp_Customer.ConnConfig = Session["config"].ToString();
            //objContract.StartDate = Convert.ToDateTime(txtStartDate.Text);
            DataSet ds = new DataSet();
            if (Request.QueryString["uid"] != null)
            {
                objProp_Customer.TemplateID = Convert.ToInt32(Request.QueryString["uid"].ToString());
                ds = objBL_Customer.getEstimateAgreement(objProp_Customer);
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
            if (dsC.Tables[0].Rows.Count > 0)
            {
                if (Session["MSM"].ToString() != "TS")
                {
                    if (txtFrom.Text.Trim() == string.Empty)
                    {
                        txtFrom.Text = dsC.Tables[0].Rows[0]["Email"].ToString();
                    }
                }

                string address = dsC.Tables[0].Rows[0]["name"].ToString() + Environment.NewLine;
                address += dsC.Tables[0].Rows[0]["Address"].ToString() + Environment.NewLine;
                address += dsC.Tables[0].Rows[0]["city"].ToString() + ", " + dsC.Tables[0].Rows[0]["state"].ToString() + ", " + dsC.Tables[0].Rows[0]["zip"].ToString() + Environment.NewLine;
                address += "Phone: " + dsC.Tables[0].Rows[0]["Phone"].ToString() + Environment.NewLine;
                address += "Fax: " + dsC.Tables[0].Rows[0]["fax"].ToString() + Environment.NewLine;
                address += "Email: " + dsC.Tables[0].Rows[0]["email"].ToString() + Environment.NewLine;
                address = "Please review the attached invoice from: " + Environment.NewLine + Environment.NewLine + address;
                ViewState["company"] = address;
                txtBody.Text = address;
            }

            //ReportParameter rpUser = new ReportParameter("paramUser", Session["User"].ToString());           
            string imagePath = new Uri(Server.MapPath("~/images/T&C.PNG")).AbsoluteUri;
            ReportParameter parameter = new ReportParameter("ImagePath", imagePath);

            rvServiceAgreement.LocalReport.DataSources.Clear();
            rvServiceAgreement.LocalReport.DataSources.Add(new ReportDataSource("dsEstimate", ds.Tables[0]));
            rvServiceAgreement.LocalReport.DataSources.Add(new ReportDataSource("dsRol", ds.Tables[1]));
            rvServiceAgreement.LocalReport.DataSources.Add(new ReportDataSource("dsEmp", ds.Tables[2]));
            rvServiceAgreement.LocalReport.DataSources.Add(new ReportDataSource("dsCompany", dsC.Tables[0]));
            rvServiceAgreement.LocalReport.ReportPath = "Reports/MaddenServiceAgreement.rdlc";
            rvServiceAgreement.LocalReport.EnableExternalImages = true;
            rvServiceAgreement.LocalReport.EnableHyperlinks = true;           
            rvServiceAgreement.LocalReport.SetParameters(parameter);
            //rvEstimate.LocalReport.SetParameters((new ReportParameter[] { rpUser }));
            rvServiceAgreement.LocalReport.Refresh();

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
        byte[] bytes = rvServiceAgreement.LocalReport.Render(
            "PDF", null, out mimeType, out encoding, out filenameExtension,
             out streamids, out warnings);

        return bytes;
    }


}