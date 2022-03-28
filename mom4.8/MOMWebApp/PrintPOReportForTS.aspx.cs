using BusinessEntity;
using BusinessLayer;
using System;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Stimulsoft.Report;
using System.Configuration;

public partial class PrintPOReportForTS : System.Web.UI.Page
{
    #region Variable
    BL_Bills _objBLBills = new BL_Bills();
    PO _objPO = new PO();
    ApprovePOStatus _objApprovePOStatus = new ApprovePOStatus();
    BusinessEntity.User objPropUser = new BusinessEntity.User();
    BL_User objBL_User = new BL_User();
    #endregion

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
                if (Request.QueryString["id"] != "")
                    DisplayPOReport();

            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    #endregion
    protected void lnkClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("addpo.aspx?id=" + Request.QueryString["id"]);
    }
    private void DisplayPOReport()
    {
        try
        {
            //byte[] buffer = null;
            string loc = ""; ;
            string reportPath = HttpContext.Current.Request.MapPath(HttpContext.Current.Request.ApplicationPath) + "/StimulsoftReports/Purchase Order - Box Shadow.mrt";
            StiReport report = new StiReport();
            report.Load(reportPath);
            //report.Compile();

            _objApprovePOStatus.ConnConfig = Session["config"].ToString();
            _objApprovePOStatus.POIDs = Request.QueryString["id"];
            _objApprovePOStatus.UserID = Convert.ToInt32(Session["userid"]);

            DataSet DS = _objBLBills.GetPODetailsForMailALL(_objApprovePOStatus);
            DataTable dt_PO = DS.Tables[0];
            DataTable dt_POItem = DS.Tables[1];
            DataTable dt_Vender = DS.Tables[2];

            foreach (DataRow V_dr in dt_Vender.Rows)
            {
                int VenderID = Convert.ToInt32(V_dr["VenderID"]);
                txtTo.Text = Convert.ToString(V_dr["Email"]);
                foreach (DataRow PO_dr in dt_PO.Select("VenderID = " + VenderID))
                {
                    int POID = Convert.ToInt32(PO_dr["PO"]);
                    loc = Convert.ToString(PO_dr["RollName"]);
                    DataTable dt_POHead = dt_PO.Clone();
                    dt_POHead.ImportRow(PO_dr);

                    DataTable dt_PODetail = dt_POItem.Clone();
                    foreach (DataRow POIt_dr in dt_POItem.Select("PO = " + POID))
                    {
                        dt_PODetail.Rows.Add(POIt_dr.ItemArray);
                    }
                    // Report Start
                    DataSet POHead = new DataSet();
                    DataTable hTable = dt_POHead;
                    hTable.TableName = "POHead";
                    POHead.Tables.Add(hTable);
                    POHead.DataSetName = "POHead";

                    DataSet POItem = new DataSet();
                    DataTable dTable = dt_PODetail;
                    dTable.TableName = "POItem";
                    POItem.Tables.Add(dTable);
                    POItem.DataSetName = "POItem";

                    report.RegData("AIAHeader", POHead);
                    report.RegData("AIADetails", POItem);
                    report.Dictionary.Synchronize();
                    report.Render();
                    StiWebDesigner1.Report = report;
                }
            }
            DataSet dsC = new DataSet();
            objPropUser.ConnConfig = Session["config"].ToString();
            if (Session["MSM"].ToString() != "TS")
            {
                dsC = objBL_User.getControl(objPropUser);
            }
            else
            {
                objPropUser.LocID = Convert.ToInt32(loc);
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
                txtFrom.Text = "info@mom.com";
                txtSubject.Text = ConfigurationManager.AppSettings["CustomerName"] + " PO Report - " + Request.QueryString["id"];
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
            if (dt_Vender.Rows.Count == 1 && String.IsNullOrEmpty(dt_Vender.Rows[0]["Email"].ToString()))
            {
                string str = "No contact found for Vender.";
                ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            }
        }
        catch (Exception exp)
        {
            string str = exp.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    protected void SendMail(byte[] buffer, string FileName)
    {
        Mail mail = new Mail();
        mail.From = txtFrom.Text == "" ? "info@mom.com" : txtFrom.Text;
        mail.To = txtTo.Text.ToString().Split(';', ',').OfType<string>().ToList();
        mail.Cc = txtCC.Text.ToString().Split(';', ',').OfType<string>().ToList();
        mail.Title = txtSubject.Text;
        mail.Text = txtBody.Text;
        mail.FileName = FileName;
        mail.attachmentBytes = buffer;
        mail.DeleteFilesAfterSend = true;
        mail.RequireAutentication = false;
        mail.Send();
    }
    protected void hideModalPopupViaServerConfirm_Click(object sender, EventArgs e)
    {
        try
        {
            byte[] buffer = null;

            string reportPath = HttpContext.Current.Request.MapPath(HttpContext.Current.Request.ApplicationPath) + "/StimulsoftReports/Purchase Order - Box Shadow.mrt";
            StiReport report = new StiReport();
            report.Load(reportPath);
            //report.Compile();

            _objApprovePOStatus.ConnConfig = Session["config"].ToString();
            _objApprovePOStatus.POIDs = Request.QueryString["id"];
            _objApprovePOStatus.UserID = Convert.ToInt32(Session["userid"]);

            DataSet DS = _objBLBills.GetPODetailsForMailALL(_objApprovePOStatus);
            DataTable dt_PO = DS.Tables[0];
            DataTable dt_POItem = DS.Tables[1];
            DataTable dt_Vender = DS.Tables[2];
            foreach (DataRow V_dr in dt_Vender.Rows)
            {
                int VenderID = Convert.ToInt32(V_dr["VenderID"]);

                foreach (DataRow PO_dr in dt_PO.Select("VenderID = " + VenderID))
                {
                    int POID = Convert.ToInt32(PO_dr["PO"]);

                    DataTable dt_POHead = dt_PO.Clone();
                    dt_POHead.ImportRow(PO_dr);

                    DataTable dt_PODetail = dt_POItem.Clone();
                    foreach (DataRow POIt_dr in dt_POItem.Select("PO = " + POID))
                    {
                        dt_PODetail.Rows.Add(POIt_dr.ItemArray);
                    }
                    // Report Start
                    DataSet POHead = new DataSet();
                    DataTable hTable = dt_POHead;
                    hTable.TableName = "POHead";
                    POHead.Tables.Add(hTable);
                    POHead.DataSetName = "POHead";

                    DataSet POItem = new DataSet();
                    DataTable dTable = dt_PODetail;
                    dTable.TableName = "POItem";
                    POItem.Tables.Add(dTable);
                    POItem.DataSetName = "POItem";

                    report.RegData("AIAHeader", POHead);
                    report.RegData("AIADetails", POItem);
                    report.Dictionary.Synchronize();
                    report.Render();

                    var settings = new Stimulsoft.Report.Export.StiPdfExportSettings();
                    var service = new Stimulsoft.Report.Export.StiPdfExportService();
                    System.IO.MemoryStream stream = new System.IO.MemoryStream();
                    service.ExportTo(report, stream, settings);
                    buffer = stream.ToArray();
                    string FileName = "NewPOReport_" + POID + ".pdf";
                    SendMail(buffer, FileName);
                    this.programmaticModalPopup.Hide();
                }
            }
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "noty({text: 'Mail sent successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
        catch (Exception exp)
        {
            string str = exp.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
}