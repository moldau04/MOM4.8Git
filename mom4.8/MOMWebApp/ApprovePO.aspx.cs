using BusinessEntity;
using BusinessLayer;
using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using Stimulsoft.Report;

public partial class ApprovePO : System.Web.UI.Page
{
    PO _objPO = new PO();

    ApprovePOStatus _objApprovePOStatus = new ApprovePOStatus();

    BL_Bills _objBLBills = new BL_Bills();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["config"] == null) { Response.Redirect("login.aspx"); }
        else if (Request.QueryString["PO"] == null) { Response.Redirect("login.aspx"); }
        else if (Request.QueryString["PageName"] == null) { Response.Redirect("login.aspx"); }
        if (!IsPostBack) { GetPOData(); }

        if (Request.QueryString["msg"] !=null) { Label1.Text = Request.QueryString["msg"].ToString(); }
    }

    protected void GetPOData()
    {
        hdnTotal.Value = "100000";
        _objPO.ConnConfig = Session["config"].ToString();
        _objPO.POID = Convert.ToInt32(Request.QueryString["PO"]);
        _objPO.UserID = Convert.ToInt32(Session["userid"]);
        DataSet ds = _objBLBills.GetPOById(_objPO);
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            txtPO.Text = dr["PO"].ToString();
            txtVendor.Text = dr["VendorName"].ToString();
            txtAddress.Text = dr["Address"].ToString();
            txtDate.Text = Convert.ToDateTime(dr["fDate"]).ToShortDateString();
            txtDueDate.Text = Convert.ToDateTime(dr["Due"]).ToShortDateString();
            ViewState["_accountName"] = dr["Acct"].ToString();
            if (!string.IsNullOrEmpty(dr["StatusName"].ToString()))
            {
                txtStatus.Text = dr["StatusName"].ToString();
            }
            lblTotalAmount.Text = dr["Amount"].ToString();
            hdnTotal.Value = dr["Amount"].ToString();
            ViewState["_EN"] = (int)(dr["EN"]);
            ViewState["_inventoryHasAvailable"] = (int)(dr["InventoryHasAvailable"]);
            ViewState["_customProgram"] = dr["CustomProgram"].ToString();

            _objPO.UserID = Convert.ToInt32(Session["userid"]);
            DataSet ApprovePOLimit = _objBLBills.GetPOApproveDetails(_objPO);
            if (ApprovePOLimit.Tables[0].Rows.Count > 0)
            {
                divStatusComments.Visible = false;
                divSignature.Visible = false;
                btnDecline.Visible = false;
                btnApprove.Visible = false;

                DataRow row = ApprovePOLimit.Tables[0].Rows[0];


                decimal MinAmount = Convert.ToDecimal(row["MinAmount"].ToString() );

                decimal MaxAmount = Convert.ToDecimal(row["MaxAmount"].ToString());

                int POApproveAmt = Convert.ToInt16(row["POApproveAmt"].ToString());

                int POApprove = Convert.ToInt16(row["POApprove"].ToString());

                decimal hTotal = Convert.ToDecimal(hdnTotal.Value);


                if (POApprove == 1)
                {
                    if (POApproveAmt == 0)
                    {
                        if (hTotal >= (MinAmount) && hTotal <= MaxAmount)
                        {
                            divStatusComments.Visible = true;
                            divSignature.Visible = true;
                            btnDecline.Visible = true;
                            btnApprove.Visible = true;
                        }
                    } 
                }
            }

            _objApprovePOStatus.ConnConfig = Session["config"].ToString();
            _objApprovePOStatus.POID = Convert.ToInt32(Request.QueryString["PO"]);

            DataSet ApprovalStatus = _objBLBills.GetPOSign(_objApprovePOStatus);
            if (ApprovalStatus.Tables[0].Rows.Count > 0)
            {
                //DataRow row = ApprovePOLimit.Tables[0].Rows[0];
                string signature = null;
                if (ApprovalStatus.Tables[0].Rows[0]["Signature"] != DBNull.Value)
                {
                    signature = "data:image/png;base64," + Convert.ToBase64String((byte[])ApprovalStatus.Tables[0].Rows[0]["Signature"]);
                }
                if (!string.IsNullOrEmpty(signature))
                {
                    imgSign.ImageUrl = signature;
                    hdnImg.Value = signature;
                }
            }
            if (dr["POApprovalStatus"].ToString() == "1")
            {
                divStatusComments.Visible = false;                
                btnDecline.Visible = false;
                btnApprove.Visible = false;
                txtStatus.Text = "Approved";
            }

        }
    }

    protected void btnApprove_Click(object sender, EventArgs e)


    {
        _objApprovePOStatus.ConnConfig = Session["config"].ToString();
        _objApprovePOStatus.POID = Convert.ToInt32(Request.QueryString["PO"]);
        _objApprovePOStatus.UserID = Convert.ToInt32(Session["userid"]);
        _objApprovePOStatus.Comments = txtStatusComment.Text;

        ////string reportPath = HttpContext.Current.Request.MapPath(HttpContext.Current.Request.ApplicationPath) + "/StimulsoftReports/Purchase Order - Box Shadow.mrt";
        ////string filename = "PO_" + _objApprovePOStatus.POID.ToString() + "_" + DateTime.Now.ToString("HH:mm:ss").Replace(":", string.Empty) + ".pdf";
        ////string savepathconfig = System.Web.Configuration.WebConfigurationManager.AppSettings["AttachmentPath"].Trim();
        ////string filepath = savepathconfig + filename;

        string reportPath = string.Empty, filename = string.Empty, savepathconfig, filepath = string.Empty;

        if (ViewState["_customProgram"].ToString() != string.Empty && ViewState["_customProgram"].ToString().ToLower() == "mitsu")
        {
            if (ViewState["_accountName"].ToString() == "MELTEC-OO")
            {
                ////ES - 1674 If the Vendor.ID = 'MELTEC' then use template "Purchase Order EED -HQ MELTEC.rpt/mrt
                reportPath = HttpContext.Current.Request.MapPath(HttpContext.Current.Request.ApplicationPath) + "/StimulsoftReports/Purchase Order/Purchase Order Eed -HQ MELTEC.mrt";
                filename = "PO_EedHQMeltec_" + _objApprovePOStatus.POID.ToString() + "_" + DateTime.Now.ToString("HH:mm:ss").Replace(":", string.Empty) + ".pdf";
            }
            else if (Convert.ToUInt16(ViewState["_EN"]) != 0 && Convert.ToUInt16(ViewState["_inventoryHasAvailable"]) == 0)
            {
                ////ES - 1674 If the Vendor.EN <> 00 and there are no inventory then use "Purchase Order EED Branch.rpt/.mrt            
                reportPath = HttpContext.Current.Request.MapPath(HttpContext.Current.Request.ApplicationPath) + "/StimulsoftReports/Purchase Order Eed Branch.mrt";
                filename = "PO_EedBranch_" + _objApprovePOStatus.POID.ToString() + "_" + DateTime.Now.ToString("HH:mm:ss").Replace(":", string.Empty) + ".pdf";
            }
            else if (Convert.ToUInt16(ViewState["_EN"]) != 0 && Convert.ToUInt16(ViewState["_inventoryHasAvailable"]) > 0)
            {
                ////ES - 1674 If the Vendor.EN <> 00 and there are inventory items use "Purchase Order Inv- Branch.rpt/.mrt
                reportPath = HttpContext.Current.Request.MapPath(HttpContext.Current.Request.ApplicationPath) + "/StimulsoftReports/Purchase Order/Purchase Order Inv- Branch.mrt";
                filename = "PO_InvBranch_" + _objApprovePOStatus.POID.ToString() + "_" + DateTime.Now.ToString("HH:mm:ss").Replace(":", string.Empty) + ".pdf";
            }
            else if (Convert.ToUInt16(ViewState["_EN"]) == 0 && Convert.ToUInt16(ViewState["_inventoryHasAvailable"]) == 0)
            {
                ////ES - 1674 If the Vendor.EN = 00 and no inventory items use "Purchase Order EED - HQ Only.rpt/.mrt
                reportPath = HttpContext.Current.Request.MapPath(HttpContext.Current.Request.ApplicationPath) + "/StimulsoftReports/Purchase Order Eed - HQ Only.mrt";
                filename = "PO_EedHQ_" + _objApprovePOStatus.POID.ToString() + "_" + DateTime.Now.ToString("HH:mm:ss").Replace(":", string.Empty) + ".pdf";
            }
            else if (Convert.ToUInt16(ViewState["_EN"]) == 0 && Convert.ToUInt16(ViewState["_inventoryHasAvailable"]) > 0)
            {
                ////ES - 1674 If the Vendor.EN = 00 and there are inventory items use "Purchase Order Inv-HQ.rpt/.mrt
                reportPath = HttpContext.Current.Request.MapPath(HttpContext.Current.Request.ApplicationPath) + "/StimulsoftReports/Purchase Order/Purchase Order Inv-HQ.mrt";
                filename = "PO_InvHQ_" + _objApprovePOStatus.POID.ToString() + "_" + DateTime.Now.ToString("HH:mm:ss").Replace(":", string.Empty) + ".pdf";
            }

            savepathconfig = System.Web.Configuration.WebConfigurationManager.AppSettings["POAttachmentPath"].Trim() + "/ApprovedPO/";
            filepath = savepathconfig + filename;
        }



        LinkButton b = (LinkButton)sender;
        string msg = "";
        if (b.Text == "Approve")
        {
            _objApprovePOStatus.Status = 1;
            msg = "PO approved successfully!";
        }
        else
        {
            _objApprovePOStatus.Status = 2;
            msg = "PO rejected successfully!";
        }
        if (hdnImg.Value != "")
        {
            string str = hdnImg.Value;
            string last = str.Substring(str.LastIndexOf(',') + 1);
            _objApprovePOStatus.Signature = Convert.FromBase64String(last);
        }
        _objApprovePOStatus.FileName = filename;
        _objApprovePOStatus.FilePath = filepath;
        DataSet ApprovePO = _objBLBills.POApproveDetails(_objApprovePOStatus);

        if (ApprovePO.Tables[0].Rows.Count > 0)
        {
            byte[] buffer = null;

            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyProj", "noty({text: " + msg + ", dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : false});", true);
            StiReport report = new StiReport();
            report.Load(reportPath);
            report.Compile();

            DataSet POHead = new DataSet();
            DataTable hTable = ApprovePO.Tables[0].Copy();
            hTable.TableName = "POHead";
            POHead.Tables.Add(hTable);
            POHead.DataSetName = "POHead";

            DataSet POItem = new DataSet();
            DataTable dTable = ApprovePO.Tables[1].Copy();
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

            if (buffer != null)
            {
                using (new NetworkConnection())
                {
                    if (File.Exists(filepath))
                        File.Delete(filepath);
                    using (var fs = new FileStream(filepath, FileMode.Create))
                    {
                        fs.Write(buffer, 0, buffer.Length);
                        fs.Close();
                    }
                }
            }
            Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();
            Response.Buffer = true;
            Response.AddHeader("Content-Disposition", "attachment;filename=" + filename);
            Response.ContentType = "application/pdf";
            Response.AddHeader("Content-Length", (buffer.Length).ToString());
            Response.BinaryWrite(buffer);

        }

        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyProj", "noty({text: " + msg + ", dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : false});", true);

        string sss = Request.Url.AbsoluteUri.ToString();
        Response.Redirect(sss+"&msg="+ msg);
    }
}