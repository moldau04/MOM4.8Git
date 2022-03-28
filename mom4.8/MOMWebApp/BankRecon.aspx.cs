using BusinessEntity;
using BusinessLayer;
using Microsoft.Reporting.WebForms;
using Stimulsoft.Report;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class BankRecon : System.Web.UI.Page
{
    #region Variables

    Bank objBank = new Bank();
    BL_BankAccount objBLBank = new BL_BankAccount();

    Dep _objDep = new Dep();
    BL_Deposit _objBL_Deposit = new BL_Deposit();

    CD _objCD = new CD();
    BL_Bills _objBL_Bills = new BL_Bills();

    User _objPropUser = new User();
    Transaction _objTrans = new Transaction();
    BL_User _objBL_User = new BL_User();

    Journal _objJournal = new Journal();
    BL_JournalEntry _objBLJournal = new BL_JournalEntry();

    Chart _objChart = new Chart();
    BL_Chart _objBLChart = new BL_Chart();
    User objPropUser = new User();
    BL_Report objBL_Report = new BL_Report();
    Customer objCust = new Customer();
    TransBankAdj _objTransBank = new TransBankAdj();
    BL_ReportsData _objBLReportsData = new BL_ReportsData();
    GetCustomerLabelParam _GetCustomerLabel = new GetCustomerLabelParam();

    BusinessEntity.CompanyOffice objCompany = new BusinessEntity.CompanyOffice();
    BL_Company objBL_Company = new BL_Company();

    private const string asc = " ASC";
    private const string desc = " DESC";
    private int controlChange = 0;
    #endregion

    #region Events

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
                string SSL = System.Web.Configuration.WebConfigurationManager.AppSettings["SSL"].Trim();

                if (Request.Url.Scheme == "http" && SSL == "1")
                {
                    string URL = Request.Url.ToString();
                    URL = URL.Replace("http://", "https://");
                    Response.Redirect(URL);
                }

                CompanyPermission();
                userPermission();
                FillBank();
                SetBankRecon();
                HighlightSideMenu("financeMgr", "lnkBankRecon", "financeMgrSub");
                lnkReprint.Visible = false;

                DateTime statementDate = SetStatementDate();

                TriggerUpdateGrid();

            }

            // ClientScript.RegisterStartupScript(Page.GetType(), "addAutoComplete", "addAutoComplete();", true);
            ScriptManager.RegisterClientScriptBlock(Page, typeof(RadGrid), "addAutoComplete",
                                 "$(function () {{addAutoComplete();}});", true);
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }


    }
    private void HighlightSideMenu(string MenuParent, string PageLink, string SubMenuDiv)
    {
        HyperLink aNav = (HyperLink)Page.Master.FindControl(MenuParent);
        //li.Style.Add("background", "url(images/active_menu_bg.png) repeat-x");
        aNav.CssClass = "active collapsible-header waves-effect waves-cyan collapsible-height-nl";

        //HyperLink a = (HyperLink)Page.Master.Master.FindControl("SalesLink");
        //a.Style.Add("color", "#2382b2");

        HyperLink lnkUsersSmenu = (HyperLink)Page.Master.FindControl(PageLink);
        //lnkUsersSmenu.Style.Add("color", "#FF7A0A");
        lnkUsersSmenu.Style.Add("color", "#316b9d");
        lnkUsersSmenu.Style.Add("font-weight", "normal");
        lnkUsersSmenu.Style.Add("background-color", "#e5e5e5");
        //AjaxControlToolkit.HoverMenuExtender hm = (AjaxControlToolkit.HoverMenuExtender)Page.Master.Master.FindControl("HoverMenuExtenderSales");
        //hm.Enabled = false;
        HtmlGenericControl div = (HtmlGenericControl)Page.Master.FindControl(SubMenuDiv);
        div.Style.Add("display", "block");
    }

    private void CompanyPermission()
    {
        if (Session["COPer"].ToString() == "1")
        {

            dvCompanyPermission.Visible = true;
            FillCompany();
        }
        else
        {

            dvCompanyPermission.Visible = false;
        }
    }
    private void userPermission()
    {
        if (Session["type"].ToString() != "am" && Session["type"].ToString() != "c")
        {
            DataTable ds = new DataTable();
            //ds = (DataTable)Session["userinfo"];
            ds = GetUserById();
            //Equipment

            string bankrec = ds.Rows[0]["bankrec"] == DBNull.Value ? "YYYYYY" : ds.Rows[0]["bankrec"].ToString();

            string Add = bankrec.Length < 1 ? "Y" : bankrec.Substring(0, 1);
            string Edit = bankrec.Length < 2 ? "Y" : bankrec.Substring(1, 1);
            string Delete = bankrec.Length < 3 ? "Y" : bankrec.Substring(2, 1);
            string View = bankrec.Length < 4 ? "Y" : bankrec.Substring(3, 1);

            if (Edit == "N")
            {
                lnkBtnSave.Visible = false;
                lnkBtnRecon.Visible = false;
                lnkReport.Visible = false;
            }
            //if (Edit == "N")
            //{
            //    btnEdit.Visible = false;
            //    lnkCopy.Visible = false;
            //}
            //if (Delete == "N")
            //{
            //    lnkDelete.Visible = false;

            //}
            if (View == "N")
            {
                Response.Redirect("Home.aspx?permission=no"); return;
            }
        }
    }
    private DataTable GetUserById()
    {
        User objPropUser = new User();
        objPropUser.TypeID = Convert.ToInt32(Session["usertypeid"]);
        objPropUser.UserID = Convert.ToInt32(Session["userid"]);
        objPropUser.ConnConfig = Session["config"].ToString();
        objPropUser.DBName = Session["dbname"].ToString();
        DataSet ds = new DataSet();
        ds = _objBL_User.GetUserPermissionByUserID(objPropUser);
        return ds.Tables[0];
    }

    private void FillCompany()
    {
        objCompany.UserID = Convert.ToInt32(Session["UserID"].ToString());
        objCompany.DBName = Session["dbname"].ToString();
        objCompany.ConnConfig = Session["config"].ToString();
        DataSet ds = new DataSet();
        ds = objBL_Company.getCompanyByCustomer(objCompany);
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddlCompany.DataSource = ds.Tables[0];
            ddlCompany.DataTextField = "Name";
            ddlCompany.DataValueField = "CompanyID";
            ddlCompany.DataBind();
            ddlCompany.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select", "0"));

            //ddlCompanyEdit.DataSource = ds.Tables[0];
            //ddlCompanyEdit.DataTextField = "Name";
            //ddlCompanyEdit.DataValueField = "CompanyID";
            //ddlCompanyEdit.DataBind();
            //ddlCompanyEdit.Items.Insert(0, new ListItem("Select", "0"));

        }
    }
    #endregion
    protected void Page_PreRender(Object o, EventArgs e)
    {
        foreach (GridDataItem gr in gvDeposit.Items)
        {
            //Label lblId = (Label)gr.FindControl("lblId");

            //AjaxControlToolkit.HoverMenuExtender hmeRes = (AjaxControlToolkit.HoverMenuExtender)gr.FindControl("hmeRes");
            //gr.Attributes["onclick"] = "SelectedRowStyle('" + gr.ClientID + "','" + chkSelect.ClientID + "','" + gvDeposit.ClientID + "',event);calculateDepositAmount();";
            //gr.Attributes["onclick"] = "calculateDepositAmount();";
            var hdnStatus = (HiddenField)gr.FindControl("hdnStatus");
            gr.Selected = hdnStatus.Value == "T";
        }

        foreach (GridDataItem gr in gvCheck.Items)
        {
            //
            //Label lblId = (Label)gr.FindControl("lblId");

            //AjaxControlToolkit.HoverMenuExtender hmeRes = (AjaxControlToolkit.HoverMenuExtender)gr.FindControl("hmeRes");
            //gr.Attributes["onclick"] = "SelectedRowStyle('" + gr.ClientID + "','" + chkSelect.ClientID + "','" + gvCheck.ClientID + "',event);calculateCheckAmount();";
            //gr.Attributes["onclick"] = "calculateCheckAmount();";
            var hdnStatus = (HiddenField)gr.FindControl("hdnStatus");
            gr.Selected = hdnStatus.Value == "T";
        }
    }
    protected void ddlBank_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            controlChange = 1;
            UpdateBankName();
            lblCheckAmount.Text = "$0.00";
            lblCheckCount.Text = "0";
            lblDepositAmount.Text = "$0.00";
            lblDepositCount.Text = "0";
            hdnListDebit.Value = "";
            hdnListCredit.Value = "";
            if (IsValidDate())
            {
                if ((ddlBank.SelectedValue) != "0" && (ddlBank.SelectedValue != ""))
                {
                    lblBankName.Text = string.Format("Bank Name: {0}", ddlBank.SelectedItem.Text);
                    GetBankDetail();
                }
                else
                {
                    txtEndingBalance.ReadOnly = true;
                    lblBeginBalance.Text = "$0.00";
                }

                txtEndingBalance.Text = "$0.00";
                DateTime _statementDate = Convert.ToDateTime(txtStatementDate.Text);
                TriggerUpdateGrid();
                controlChange = 0;
                ScriptManager.RegisterStartupScript(this, GetType(), "calDifference", "calculateDifference();", true);
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    private void UpdateBankName()
    {
        lblBankName.Text = string.Empty;

        if ((ddlBank.SelectedValue) != "0" && (ddlBank.SelectedValue != ""))
        {
            lblBankName.Text = string.Format("Bank Name: {0}", ddlBank.SelectedItem.Text);
        }

        udpBankName.Update();

    }

    private void TriggerUpdateGrid()
    {
        gvCheck.Rebind();
        gvDeposit.Rebind();
    }
    protected void txtStatementDate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            TextBox txtStatementDate = (TextBox)sender;
            if (!string.IsNullOrEmpty(txtStatementDate.Text))
            {
                if (IsValidDate())
                {
                    //controlChange = 2;
                    DateTime _statementDate = Convert.ToDateTime(txtStatementDate.Text);
                    TriggerUpdateGrid();
                    //controlChange = 0;
                }
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    protected void lnkBtnClose_Click(object sender, EventArgs e)
    {
        if (ViewState["Recon"] == null)    // on close, save bank reconciliation if it is not reconciled.
        {
            StoreBankRec();
        }
        Response.Redirect("home.aspx");
    }
    protected void lnkBtnRecon_Click(object sender, EventArgs e)
    {
        if (txtEndingBalance.Text != "$0.00")
        {
            try
            {
                if (Page.IsValid)
                {
                    if (IsValidDate())
                    {
                        StoreBankRec();
                        ViewState["Recon"] = true;
                        int j = 0;
                        double totalOutCheck = 0.00; double totalOutDep = 0.00;      // total outstanding check, deposit
                        SetInitialRow();
                        DataTable _dt = (DataTable)ViewState["OpenTrans"];
                        DataTable dtBank = (DataTable)ViewState["BankRecon"];

                        objBank.ConnConfig = Session["config"].ToString();
                        objBank.ID = Convert.ToInt32(ddlBank.SelectedValue);
                        objBank.Balance = double.Parse(txtEndingBalance.Text.Replace('$', '0'), NumberStyles.AllowParentheses |
                                                                                NumberStyles.AllowThousands |
                                                                                NumberStyles.AllowDecimalPoint);
                        objBank.LastReconDate = DateTime.Now;
                        if (!string.IsNullOrEmpty(hdnServiceAcct.Value))
                        {
                            objBank.ServiceAcct = Convert.ToInt32(hdnServiceAcct.Value);
                        }
                        if (!string.IsNullOrEmpty(txtServiceChargeDate.Text))
                        {
                            objBank.ServiceDate = Convert.ToDateTime(txtServiceChargeDate.Text);
                        }
                        if (!string.IsNullOrEmpty(txtServiceChrgAmount.Text))
                        {
                            objBank.ServiceCharge = Convert.ToDouble(txtServiceChrgAmount.Text);
                        }
                        if (!string.IsNullOrEmpty(hdnInterestAcct.Value))
                        {
                            objBank.InterestAcct = Convert.ToInt32(hdnInterestAcct.Value);
                        }
                        if (!string.IsNullOrEmpty(txtInterestDate.Text))
                        {
                            objBank.InterestDate = Convert.ToDateTime(txtInterestDate.Text);
                        }
                        if (!string.IsNullOrEmpty(txtInterestAmount.Text))
                        {
                            objBank.InterestCharge = Convert.ToDouble(txtInterestAmount.Text);
                        }
                        if (!string.IsNullOrEmpty(txtStatementDate.Text))
                        {
                            objBank.fDate = Convert.ToDateTime(txtStatementDate.Text);
                        }

                        #region Update Checks and Deposit

                        foreach (GridDataItem gr in gvCheck.Items)
                        {

                            Label lblfDate = (Label)gr.FindControl("lblfDate");
                            Label lblfDesc = (Label)gr.FindControl("lblfDesc");
                            Label lblAmount = (Label)gr.FindControl("lblAmount");
                            Label lblRef = (Label)gr.FindControl("lblRef");
                            Label lblType = (Label)gr.FindControl("lblType1");
                            HiddenField hdnAmount = (HiddenField)gr.FindControl("hdnAmount");
                            if (gr.Selected)
                            {
                                HiddenField hdnBatch = (HiddenField)gr.FindControl("hdnBatch");
                                HiddenField hdnTypeNum = (HiddenField)gr.FindControl("hdnTypeNum");
                                HiddenField hdnID = (HiddenField)gr.FindControl("hdnID");

                                DataRow drbank = null;
                                drbank = dtBank.NewRow();
                                drbank["ID"] = Convert.ToInt32(hdnID.Value);
                                drbank["fDate"] = Convert.ToDateTime(lblfDate.Text);
                                drbank["TypeNum"] = Convert.ToInt32(hdnTypeNum.Value);
                                drbank["Ref"] = lblRef.Text;
                                drbank["Amount"] = Convert.ToDouble(hdnAmount.Value);
                                drbank["Batch"] = Convert.ToInt32(hdnBatch.Value);
                                dtBank.Rows.Add(drbank);
                            }
                            else
                            {
                                DataRow dr = null;
                                dr = _dt.NewRow();
                                dr["RowID"] = j + 1;
                                dr["ID"] = 0;
                                dr["fDate"] = Convert.ToDateTime(lblfDate.Text);
                                dr["Type"] = "Check/Credit";
                                dr["Ref"] = lblRef.Text;
                                dr["fDesc"] = lblfDesc.Text;

                                dr["Amount"] = (double.Parse(lblAmount.Text.Replace('$', '0'), NumberStyles.AllowParentheses |
                                          NumberStyles.AllowThousands |
                                          NumberStyles.AllowDecimalPoint) * -1);
                                totalOutCheck = totalOutCheck + double.Parse(lblAmount.Text.Replace('$', '0'), NumberStyles.AllowParentheses |
                                                                                          NumberStyles.AllowThousands |
                                                                                          NumberStyles.AllowDecimalPoint);
                                _dt.Rows.Add(dr);
                            }
                        }

                        foreach (GridDataItem gr in gvDeposit.Items)
                        {

                            Label lblfDate = (Label)gr.FindControl("lblfDate");
                            Label lblfDesc = (Label)gr.FindControl("lblfDesc");
                            Label lblAmount = (Label)gr.FindControl("lblAmount");
                            Label lblRef = (Label)gr.FindControl("lblRef");
                            Label lblType = (Label)gr.FindControl("lblType");
                            HiddenField hdnAmount = (HiddenField)gr.FindControl("hdnAmount");
                            if (gr.Selected)
                            {
                                HiddenField hdnBatch = (HiddenField)gr.FindControl("hdnBatch");
                                HiddenField hdnTypeNum = (HiddenField)gr.FindControl("hdnTypeNum");
                                HiddenField hdnID = (HiddenField)gr.FindControl("hdnID");

                                DataRow drbank = null;
                                drbank = dtBank.NewRow();
                                drbank["ID"] = Convert.ToInt32(hdnID.Value);
                                drbank["fDate"] = Convert.ToDateTime(lblfDate.Text);
                                drbank["TypeNum"] = Convert.ToInt32(hdnTypeNum.Value);
                                drbank["Ref"] = lblRef.Text;
                                drbank["Amount"] = Convert.ToDouble(hdnAmount.Value);
                                drbank["Batch"] = Convert.ToInt32(hdnBatch.Value);
                                //drbank["TypeNum"] = hdnTypeNum.Value;
                                dtBank.Rows.Add(drbank);
                            }
                            else
                            {
                                DataRow dr = null;
                                dr = _dt.NewRow();
                                dr["RowID"] = j + 1;
                                dr["ID"] = 0;
                                dr["fDate"] = Convert.ToDateTime(lblfDate.Text);
                                dr["Type"] = "Deposit/Debit";
                                dr["Ref"] = lblRef.Text;
                                dr["fDesc"] = lblfDesc.Text;
                                dr["Amount"] = double.Parse(lblAmount.Text.Replace('$', '0'), NumberStyles.AllowParentheses |
                                                                                      NumberStyles.AllowThousands |
                                                                                      NumberStyles.AllowDecimalPoint);
                                totalOutDep = totalOutDep + double.Parse(lblAmount.Text.Replace('$', '0'), NumberStyles.AllowParentheses |
                                                                                      NumberStyles.AllowThousands |
                                                                                      NumberStyles.AllowDecimalPoint);
                                _dt.Rows.Add(dr);
                            }
                        }
                        _dt.DefaultView.Sort = "fDate ASC";
                        ViewState["OpenTrans"] = _dt.DefaultView.ToTable();
                        #endregion
                        objBank.DtBank = dtBank;
                        int ID = objBLBank.BankReconID(objBank);
                        txtRecId.Text = ID.ToString();

                        //ReportViewer rv = GetBankReconReport(totalOutCheck, totalOutDep);

                        objBank.ConnConfig = Session["config"].ToString();
                        objBank.ID = Convert.ToInt32(ddlBank.SelectedValue);
                        objBank.fDate = Convert.ToDateTime(txtStatementDate.Text);
                        DataSet dsBank = objBLBank.GetBankDetailByDate(objBank);    //Get Bank details

                        ReportViewer rv = GetBankReconReport(dsBank, totalOutCheck, totalOutDep);

                        double StBalance = 0;
                        double PrBalance = 0;
                        StBalance = Math.Round(Convert.ToDouble(dsBank.Tables[0].Rows[0]["Recon"].ToString()), 2);
                        PrBalance = Math.Round(Convert.ToDouble(dsBank.Tables[0].Rows[0]["Balance"].ToString()) + totalOutCheck - totalOutDep, 2);
                        if (StBalance != PrBalance)
                        {
                            // Sending email here
                            //ScriptManager.RegisterStartupScript(this,Page.GetType(), "keyNeedEmail", "noty({text: 'Need to send email here',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                            try
                            {
                                string toEmail = ConfigurationManager.AppSettings["BankReconAlertAddress"].ToString();
                                var fromEmail = WebBaseUtility.GetFromEmailAddress();

                                Mail mail = new Mail();
                                mail.From = fromEmail;
                                Boolean IsMailSend = false;

                                foreach (var toaddress in toEmail.Split(new[] { ";", "," }, StringSplitOptions.RemoveEmptyEntries))
                                {
                                    if (WebBaseUtility.IsValidEmailAddress1(toaddress.Trim()))
                                    {
                                        IsMailSend = true;
                                        mail.To.Add(toaddress.Trim());
                                    }
                                }
                                if (IsMailSend == true)
                                {
                                    var clientName = ConfigurationManager.AppSettings["CustomerName"].ToString();
                                    var bankName = dsBank.Tables[0].Rows[0]["BankName"].ToString();
                                    var bankAcct = dsBank.Tables[0].Rows[0]["fDesc"].ToString();
                                    var stDate = Convert.ToDateTime(txtStatementDate.Text).ToString("MM/dd/yyyy");

                                    mail.Title = clientName + " Bank Rec Mismatch " + stDate;

                                    System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
                                    stringBuilder.Append("Hi,<br/><br/>");
                                    stringBuilder.AppendFormat("There is a difference of {0} between Proof Balance and Statement Balance on Bank Reconciliation Report<br/><br/>", (StBalance - PrBalance).ToString("C"));
                                    stringBuilder.AppendFormat("Bank Account: {0}<br/>", bankAcct);
                                    stringBuilder.AppendFormat("Bank Name: {0}<br/>", bankName);
                                    stringBuilder.AppendFormat("Statement Date: {0}<br/>", stDate);
                                    stringBuilder.AppendFormat("Proof of Balance: {0}<br/>", PrBalance.ToString("C"));
                                    stringBuilder.AppendFormat("Statement Balance: {0}<br/>", StBalance.ToString("C"));
                                    stringBuilder.AppendFormat("Difference: {0}<br/>", (StBalance - PrBalance).ToString("C"));

                                    stringBuilder.AppendFormat("Service Charge Amount: {0}<br/>", string.IsNullOrEmpty(txtServiceChrgAmount.Text) ? "" : objBank.ServiceCharge.ToString("C"));
                                    stringBuilder.AppendFormat("Service Charge Date: {0}<br/>", string.IsNullOrEmpty(txtServiceChargeDate.Text) ? "" : objBank.ServiceDate.ToString("MM/dd/yyyy"));
                                    stringBuilder.AppendFormat("Service Charge Account: {0}<br/>", txtServiceAccount.Text);
                                    stringBuilder.AppendFormat("Interest Date: {0}<br/>", string.IsNullOrEmpty(txtInterestDate.Text) ? "" : objBank.InterestDate.ToString("MM/dd/yyyy"));
                                    stringBuilder.AppendFormat("Interest Amount: {0}<br/>", string.IsNullOrEmpty(txtInterestAmount.Text) ? "" : objBank.InterestCharge.ToString("C"));
                                    stringBuilder.AppendFormat("Interest Account: {0}<br/>", txtInterestAccount.Text);
                                    stringBuilder.AppendFormat("User: {0}<br/><br/>", Session["Username"].ToString());

                                    stringBuilder.Append("Please see attached report for more details<br/><br/>");
                                    stringBuilder.Append("Thank you");

                                    mail.Text = stringBuilder.ToString();
                                    byte[] buffer = null;
                                    buffer = ExportReportToPDF("BankReconciliation", rv);
                                    mail.attachmentBytes = buffer;
                                    mail.FileName = "BankReconciliation.pdf";
                                    mail.DeleteFilesAfterSend = true;
                                    mail.RequireAutentication = true;

                                    mail.Send();
                                }

                            }
                            catch (Exception)
                            {
                            }
                        }

                        StoreBankRec();
                        ResetBankRecon();
                        TriggerUpdateGrid();
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayBankRecon", "displayBankRecon();", true);
                        //lnkExportPdf_Click(lnkExportPdf, null);
                        //byte[] buffer = null;
                        //buffer = ExportReportToPDF("", rv);
                        //Response.ClearContent();
                        //Response.ClearHeaders();
                        //Response.AddHeader("Content-Disposition", "attachment;filename=Bank reconciliation " + Convert.ToDateTime(txtStatementDate.Text).ToString("yyyy-MM-dd") + ".pdf");
                        //Response.ContentType = "application/pdf";
                        //Response.AddHeader("Content-Length", (buffer.Length).ToString());
                        //Response.BinaryWrite(buffer);
                        //Response.Flush();
                        //Response.Close();
                    }
                }

            }
            catch (Exception ex)
            {
                string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
                ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true})", true);
            }
        }
        else
        {
            ClientScript.RegisterStartupScript(Page.GetType(), "keySucc", "noty({text: 'Ending Balance Is Zero,You can't Reconcile.!',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void lnkReprint_Click(object sender, EventArgs e)
    {
        try
        {
            List<byte[]> lstbyte = new List<byte[]>();
            List<byte[]> lstbyteNew = new List<byte[]>();
            byte[] buffer1 = null;
            string reportPathStimul = Server.MapPath("StimulsoftReports/BANKRECONCILIATIONITEMSCLEAREDREPORT.mrt");

            StiReport report = new StiReport();
            report.Load(reportPathStimul);
            report.Compile();

            //Get data
            DataSet dsC = new DataSet();
            var connString = Session["config"].ToString();
            objPropUser.ConnConfig = connString;
            DataSet companyInfo = new DataSet();
            companyInfo = objBL_Report.GetCompanyDetails(Session["config"].ToString());
            report.RegData("CompanyDetails", companyInfo.Tables[0]);
            report.Dictionary.Variables["Username"].Value = Session["Username"].ToString();

            objCust.ConnConfig = connString;
            objCust.CustomerID = Convert.ToInt32(txtRecId.Text);
            objCust.ItemID = Convert.ToInt32(ddlBank.SelectedValue);
            objCust.StartDate = txtStatementDate.Text;
            _GetCustomerLabel.ConnConfig = connString;

            DataSet ds = new DataSet();
            DataSet dsItems = new DataSet();

            ds = objBL_Report.GetBankReconciliationItemsCleared(objCust);
            dsItems = objBL_Report.GetBankReconciliationItemsClearedList(objCust);


            if (ds != null && ds.Tables.Count > 0)
            {
                report.RegData("ReportData", ds.Tables[0]);
            }

            if (dsItems != null && dsItems.Tables.Count > 0)
            {
                report.RegData("ReportItemList", dsItems.Tables[0]);
            }

            report.Render();

            var settings = new Stimulsoft.Report.Export.StiPdfExportSettings();
            var service = new Stimulsoft.Report.Export.StiPdfExportService();
            System.IO.MemoryStream stream = new System.IO.MemoryStream();
            service.ExportTo(report, stream, settings);
            buffer1 = stream.ToArray();
            lstbyte.Add(buffer1);

            byte[] finalbyte = null;

            if (lstbyteNew.Count != 0)
            {
                finalbyte = WriteChecks.concatAndAddContentFinal(lstbyte, lstbyteNew);
            }
            else
            {
                finalbyte = WriteChecks.concatAndAddContent(lstbyte);
            }

            Response.Clear();
            MemoryStream ms = new MemoryStream(buffer1);
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=BANKRECONCILIATIONITEMSCLEAREDREPORT.pdf");
            Response.Buffer = true;
            ms.WriteTo(Response.OutputStream);
            //Response.End();
            //HttpContext.Current.ApplicationInstance.CompleteRequest();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    public void Test(int ID)
    {

    }

    #endregion

    #region Custom Functions
    private void ResetBankRecon()
    {
        try
        {
            objBank.ConnConfig = Session["config"].ToString();
            objBank.InterestAcct = 0;
            objBank.ServiceAcct = 0;
            objBank.InterestCharge = 0;
            objBank.ServiceCharge = 0;
            objBank.ID = 0;
            objBank.Balance = 0;
            objBank.InterestDate = DateTime.MinValue;
            objBank.ServiceDate = DateTime.MinValue;
            objBank.LastReconDate = DateTime.MinValue;
            objBank.DtBank = null;
            objBLBank.StoreBankRecon(objBank);
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true})", true);
        }
    }
    private void GetBankDetail()
    {
        txtEndingBalance.ReadOnly = false;
        objBank.ConnConfig = Session["config"].ToString();
        objBank.ID = Convert.ToInt32(ddlBank.SelectedValue);
        DataSet _dsBankBal = objBLBank.GetBankByID(objBank);
        if (_dsBankBal.Tables[0].Rows.Count > 0)
        {
            double balance = Convert.ToDouble(_dsBankBal.Tables[0].Rows[0]["Recon"]);
            lblBeginBalance.Text = string.Format("{0:c}", balance);
            hdnChartID.Value = _dsBankBal.Tables[0].Rows[0]["Chart"].ToString();
        }
        else
        {
            lblBeginBalance.Text = "$0.00";
        }
    }
    //private ReportViewer GetBankReconReport(double _outstandingCheck = 0.00, double _outstandingDep = 0.00)
    //{
    //    try
    //    {
    //        lnkBtnRecon.Visible = false;
    //        lnkBtnSave.Visible = false;
    //        _objPropUser.ConnConfig = Session["config"].ToString();
    //        DataSet _dsUser = _objBLReportsData.GetControlForReports(_objPropUser);     //Get loggedin user details

    //        objBank.ConnConfig = Session["config"].ToString();
    //        objBank.ID = Convert.ToInt32(ddlBank.SelectedValue);
    //        objBank.fDate = Convert.ToDateTime(txtStatementDate.Text);
    //        DataSet dsBank = objBLBank.GetBankDetailByDate(objBank);    //Get Bank details

    //        DataTable _dtOpenTrans = (DataTable)ViewState["OpenTrans"];
    //        rvBankRecon.LocalReport.DataSources.Clear();
    //        rvBankRecon.LocalReport.DataSources.Add(new ReportDataSource("dsAddress", _dsUser.Tables[0]));
    //        rvBankRecon.LocalReport.DataSources.Add(new ReportDataSource("dsBank", dsBank.Tables[0]));
    //        rvBankRecon.LocalReport.DataSources.Add(new ReportDataSource("dsOpenTrans", _dtOpenTrans));
    //        var customerName = ConfigurationManager.AppSettings["CustomerName"].ToString();
    //        if (customerName.Equals("SECO"))
    //            rvBankRecon.LocalReport.ReportPath = "Reports/SECOBankRecon.rdlc";
    //        else
    //            rvBankRecon.LocalReport.ReportPath = "Reports/BankRecon.rdlc";

    //        //double _totalOutstanding = 0.00;
    //        ReportParameter[] rptParams = new ReportParameter[]{
    //            new ReportParameter("paramUsername",Session["Username"].ToString()),
    //            new ReportParameter("paramStatementDate",txtStatementDate.Text),
    //            new ReportParameter("paramOutstandingCredits",_outstandingCheck.ToString("0.00", CultureInfo.InvariantCulture)),
    //            new ReportParameter("paramOutstandingDebits",_outstandingDep.ToString("0.00", CultureInfo.InvariantCulture)),
    //            new ReportParameter("paramBankName",ddlBank.SelectedItem.Text)
    //        };
    //        rvBankRecon.LocalReport.EnableExternalImages = false;
    //        rvBankRecon.LocalReport.SetParameters(rptParams);
    //        rvBankRecon.LocalReport.DisplayName = "Bank reconciliation " + Convert.ToDateTime(txtStatementDate.Text).ToString("yyyy-MM-dd");
    //        rvBankRecon.LocalReport.Refresh();

    //    }
    //    catch (Exception ex)
    //    {
    //        string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
    //        ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true})", true);
    //    }
    //    return rvBankRecon;
    //}

    private ReportViewer GetBankReconReport(DataSet dsBank, double _outstandingCheck = 0.00, double _outstandingDep = 0.00)
    {
        try
        {
            lnkBtnRecon.Visible = false;
            lnkBtnSave.Visible = false;
            lnkReport.Visible = false;
            lnkReprint.Visible = true;
            _objPropUser.ConnConfig = Session["config"].ToString();
            DataSet _dsUser = _objBLReportsData.GetControlForReports(_objPropUser);     //Get loggedin user details

            DataTable _dtOpenTrans = (DataTable)ViewState["OpenTrans"];
            rvBankRecon.LocalReport.DataSources.Clear();
            rvBankRecon.LocalReport.DataSources.Add(new ReportDataSource("dsAddress", _dsUser.Tables[0]));
            rvBankRecon.LocalReport.DataSources.Add(new ReportDataSource("dsBank", dsBank.Tables[0]));
            rvBankRecon.LocalReport.DataSources.Add(new ReportDataSource("dsOpenTrans", _dtOpenTrans));
            var customerName = ConfigurationManager.AppSettings["CustomerName"].ToString();
            if (customerName.Equals("SECO"))
                rvBankRecon.LocalReport.ReportPath = "Reports/SECOBankRecon.rdlc";
            else
                rvBankRecon.LocalReport.ReportPath = "Reports/BankRecon.rdlc";

            //double _totalOutstanding = 0.00;
            ReportParameter[] rptParams = new ReportParameter[]{
                new ReportParameter("paramUsername",Session["Username"].ToString()),
                new ReportParameter("paramStatementDate",txtStatementDate.Text),
                new ReportParameter("paramOutstandingCredits",_outstandingCheck.ToString("0.00", CultureInfo.InvariantCulture)),
                new ReportParameter("paramOutstandingDebits",_outstandingDep.ToString("0.00", CultureInfo.InvariantCulture)),
                new ReportParameter("paramBankName",ddlBank.SelectedItem.Text)
            };
            rvBankRecon.LocalReport.EnableExternalImages = false;
            rvBankRecon.LocalReport.SetParameters(rptParams);
            rvBankRecon.LocalReport.DisplayName = "Bank reconciliation " + Convert.ToDateTime(txtStatementDate.Text).ToString("yyyy-MM-dd");
            rvBankRecon.LocalReport.Refresh();

        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true})", true);
        }
        return rvBankRecon;
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
    private void SetBankRecon()
    {
        try
        {
            int bank = 0;
            int user = Convert.ToInt32(Session["userid"]);
            if (user > 0)
            {
                _objPropUser.ConnConfig = Session["config"].ToString();
                _objPropUser.UserID = user;

                DataSet _ds = _objBL_User.GetUserAddress(_objPropUser);
            }
            lblBeginBalance.Text = "$0.00";
            lblDifference.Text = "$0.00";
            objBank.ConnConfig = Session["config"].ToString();
            DataSet dsBank = objBLBank.GetStoredBankRecon(objBank);
            if (dsBank.Tables[0].Rows.Count > 0)
            {
                DataRow dr = dsBank.Tables[0].Rows[0];
                if (!Convert.ToInt32(dr["Bank"]).Equals(0))
                {
                    ddlBank.SelectedValue = dr["Bank"].ToString();
                }
                if (!string.IsNullOrEmpty(dr["SCDate"].ToString()))
                {
                    txtServiceChargeDate.Text = Convert.ToDateTime(dr["SCDate"].ToString()).ToShortDateString();
                }
                if (!string.IsNullOrEmpty(dr["IntDate"].ToString()))
                {
                    txtInterestDate.Text = Convert.ToDateTime(dr["IntDate"].ToString()).ToShortDateString();
                }
                hdnInterestAcct.Value = dr["IntGL"].ToString();
                hdnServiceAcct.Value = dr["SCGL"].ToString();
                txtInterestAccount.Text = dr["IntGLName"].ToString();
                txtServiceAccount.Text = dr["SCGLName"].ToString();
                txtServiceChrgAmount.Text = string.Format("{0:n}", Convert.ToDouble(dr["SCAmount"]));
                txtInterestAmount.Text = string.Format("{0:n}", Convert.ToDouble(dr["IntAmount"]));
                txtEndingBalance.Text = string.Format("{0:n}", Convert.ToDouble(dr["EndBalance"]));
                if (!string.IsNullOrEmpty(dr["StatementDate"].ToString()))
                {
                    txtStatementDate.Text = Convert.ToDateTime(dr["StatementDate"]).ToShortDateString();
                }
                if (!Convert.ToInt32(dr["Bank"]).Equals(0))
                {
                    bank = Convert.ToInt32(dr["Bank"].ToString());
                    if ((ddlBank.SelectedValue) != "0" && (ddlBank.SelectedValue != ""))
                    {
                        GetBankDetail();
                    }
                }
                UpdateBankName();
            }
            else
            {
                txtEndingBalance.ReadOnly = true;
                txtEndingBalance.Text = "0.00";
                txtServiceChrgAmount.Text = "0.00";
                txtInterestAmount.Text = "0.00";
            }
            if (bank.Equals(0))
            {
                txtEndingBalance.ReadOnly = true;
            }
            lblDepositAmount.Text = "$0.00";
            lblCheckAmount.Text = "$0.00";
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true})", true);
        }
    }
    private DateTime SetStatementDate()
    {
        DateTime statementDate = DateTime.Now.AddMonths(-1);
        try
        {
            if (string.IsNullOrEmpty(txtStatementDate.Text))
            {
                statementDate = DateTime.Now.AddMonths(-1);
                statementDate = new DateTime(statementDate.Year, statementDate.Month, 1);
                statementDate = statementDate.AddMonths(1);
                statementDate = statementDate.AddDays(-1);
                txtStatementDate.Text = statementDate.ToString("MM/dd/yyyy");
            }
            else
            {
                statementDate = Convert.ToDateTime(txtStatementDate.Text);
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true})", true);
        }
        return statementDate;
    }
    private void FillBank()
    {
        try
        {
            ddlBank.Items.Clear();
            if (Session["COPer"].ToString() == "1")
            {
                //do nothing
            }
            else
            {
                objBank.ConnConfig = Session["config"].ToString();
                DataSet dsBank = new DataSet();
                dsBank = objBLBank.GetAllBankNames(objBank);

                ddlBank.Items.Add(new ListItem(":: Select ::", "0"));

                if (dsBank.Tables[0].Rows.Count > 0)
                {
                    ddlBank.AppendDataBoundItems = true;
                    ddlBank.DataSource = null;
                    ddlBank.DataSource = dsBank;

                    ddlBank.DataValueField = "ID";
                    ddlBank.DataTextField = "fDesc";
                    ddlBank.DataBind();
                }
                else
                {
                    ddlBank.Items.Add(new ListItem("No data found", "0"));
                }
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true})", true);
        }
    }

    private void SetInitialRow()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add(new DataColumn("RowID", typeof(string)));
        dt.Columns.Add(new DataColumn("ID", typeof(Int32)));
        dt.Columns.Add(new DataColumn("fDate", typeof(DateTime)));
        dt.Columns.Add(new DataColumn("Type", typeof(string)));
        dt.Columns.Add(new DataColumn("Ref", typeof(string)));
        dt.Columns.Add(new DataColumn("fDesc", typeof(string)));
        dt.Columns.Add(new DataColumn("Amount", typeof(double)));
        //dt.Columns.Add(new DataColumn("Selected", typeof(bool)));

        ViewState["OpenTrans"] = dt;

        DataTable dtbank = new DataTable();
        dtbank.Columns.Add(new DataColumn("ID", typeof(Int32)));
        dtbank.Columns.Add(new DataColumn("fDate", typeof(DateTime)));
        dtbank.Columns.Add(new DataColumn("Type", typeof(string)));
        dtbank.Columns.Add(new DataColumn("Ref", typeof(string)));
        //dtbank.Columns.Add(new DataColumn("fDesc", typeof(string)));
        dtbank.Columns.Add(new DataColumn("Amount", typeof(double)));
        dtbank.Columns.Add(new DataColumn("Batch", typeof(Int32)));
        dtbank.Columns.Add(new DataColumn("TypeNum", typeof(Int32)));
        dtbank.Columns.Add(new DataColumn("Selected", typeof(bool)));
        //dtbank.Columns.Add(new DataColumn("Credits", typeof(double)));
        //dtbank.Columns.Add(new DataColumn("Debits", typeof(double)));

        ViewState["BankRecon"] = dtbank;
    }

    private void CreateBankRecReportRow()
    {
        DataTable dtbank = new DataTable();
        dtbank.Columns.Add(new DataColumn("ID", typeof(Int32)));
        dtbank.Columns.Add(new DataColumn("fDate", typeof(DateTime)));
        dtbank.Columns.Add(new DataColumn("Type", typeof(string)));
        dtbank.Columns.Add(new DataColumn("Ref", typeof(string)));
        dtbank.Columns.Add(new DataColumn("fDesc", typeof(string)));
        dtbank.Columns.Add(new DataColumn("Amount", typeof(double)));
        dtbank.Columns.Add(new DataColumn("Batch", typeof(Int32)));
        dtbank.Columns.Add(new DataColumn("TypeNum", typeof(Int32)));
        dtbank.Columns.Add(new DataColumn("Selected", typeof(bool)));
        dtbank.Columns.Add(new DataColumn("Credits", typeof(double)));
        dtbank.Columns.Add(new DataColumn("Debits", typeof(double)));
        dtbank.Columns.Add(new DataColumn("Short", typeof(Int32)));
        ViewState["BankReconReport"] = dtbank;
    }
    private bool IsValidDate()
    {
        DateTime dateValue;
        string[] formats = {"M/d/yyyy", "M/d/yyyy",
                "MM/dd/yyyy", "M/d/yyyy",
                "M/d/yyyy", "M/d/yyyy",
                "M/d/yyyy", "M/d/yyyy",
                "MM/dd/yyyy", "M/dd/yyyy"};
        var dt = DateTime.TryParseExact(txtStatementDate.Text.ToString(), formats,
                            new CultureInfo("en-US"),
                            DateTimeStyles.None,
                            out dateValue);

        if (dt)
        {
            return true;
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyedit", "noty({text: 'Please enter valid date.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            return false;
        }
    }
    #endregion
    protected void gvCheck_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {
            string sortExpression = e.SortExpression;
            GetCheckState();
            if (GvCheckSortDirection == SortDirection.Ascending)
            {
                GvCheckSortDirection = SortDirection.Descending;
                SortCheckGridView(sortExpression, desc);
            }
            else
            {
                GvCheckSortDirection = SortDirection.Ascending;
                SortCheckGridView(sortExpression, asc);
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    public SortDirection GvCheckSortDirection
    {
        get
        {
            if (ViewState["sortDirection1"] == null)
                ViewState["sortDirection1"] = SortDirection.Ascending;

            return (SortDirection)ViewState["sortDirection1"];
        }
        set { ViewState["sortDirection1"] = value; }
    }
    public SortDirection GvDepSortDirection
    {
        get
        {
            if (ViewState["sortDirection2"] == null)
                ViewState["sortDirection2"] = SortDirection.Ascending;

            return (SortDirection)ViewState["sortDirection2"];
        }
        set { ViewState["sortDirection2"] = value; }
    }
    private void SortCheckGridView(string sortExpression, string direction)
    {
        DataTable dt = PageCheckSortData();

        DataView dvCheck = new DataView(dt);
        dvCheck.Sort = sortExpression + direction;

        BindCheckDatatable(dvCheck.ToTable());
    }
    private void SortDepositGridView(string sortExpression, string direction)
    {
        DataTable dt = PageDepositSortData();

        DataView dvDep = new DataView(dt);
        dvDep.Sort = sortExpression + direction;

        BindDepositDatatable(dvDep.ToTable());
    }
    private DataTable PageCheckSortData()
    {
        DataTable dt = new DataTable();
        try
        {
            if (ViewState["Check"] != null)
            {
                dt = (DataTable)ViewState["Check"];
            }
            else
            {
                DateTime statementDate;
                if (string.IsNullOrEmpty(txtStatementDate.Text))
                {
                    statementDate = SetStatementDate();
                }
                else
                    statementDate = Convert.ToDateTime(txtStatementDate.Text);

                _objCD.ConnConfig = Session["config"].ToString();
                _objCD.fDate = statementDate;
                _objCD.fDateYear = statementDate.Year;
                if ((ddlBank.SelectedValue) != "0" && (ddlBank.SelectedValue != ""))
                {
                    _objCD.Bank = Convert.ToInt32(ddlBank.SelectedValue);
                }
                DataSet ds = _objBL_Bills.GetChecksDetails(_objCD);
                ViewState["Check"] = ds.Tables[0];
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
        return dt;
    }
    private DataTable PageDepositSortData()
    {
        DataTable dt = new DataTable();
        try
        {
            if (ViewState["Deposit"] != null)
            {
                dt = (DataTable)ViewState["Deposit"];
            }
            else
            {
                DateTime statementDate;
                if (string.IsNullOrEmpty(txtStatementDate.Text))
                {
                    statementDate = SetStatementDate();
                }
                else
                    statementDate = Convert.ToDateTime(txtStatementDate.Text);

                _objDep.ConnConfig = Session["config"].ToString();
                _objDep.fDate = statementDate;
                _objDep.fDateYear = statementDate.Year;
                if ((ddlBank.SelectedValue) != "0" && (ddlBank.SelectedValue != ""))
                {
                    _objDep.Bank = Convert.ToInt32(ddlBank.SelectedValue);
                }
                DataSet ds = _objBL_Deposit.GetDepositDetails(_objDep);

                ViewState["Deposit"] = ds.Tables[0];
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
        return dt;
    }
    private void BindCheckDatatable(DataTable dt)
    {
        try
        {
            ViewState["Check"] = dt;
            gvCheck.DataSource = dt;
            gvCheck.DataBind();
            populateCheck();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    private void BindDepositDatatable(DataTable dt)
    {
        try
        {
            ViewState["Deposit"] = dt;
            gvDeposit.DataSource = dt;
            gvDeposit.DataBind();
            populateDep();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    protected void gvDeposit_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {
            string sortExpression = e.SortExpression;
            GetDepositState();
            if (GvDepSortDirection == SortDirection.Ascending)
            {
                GvDepSortDirection = SortDirection.Descending;
                SortDepositGridView(sortExpression, desc);
            }
            else
            {
                GvDepSortDirection = SortDirection.Ascending;
                SortDepositGridView(sortExpression, asc);
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    private void GetCheckState()
    {
        ArrayList checkindex = new ArrayList();
        int index = -1;
        foreach (GridDataItem gr in gvCheck.Items)
        {
            index = Convert.ToInt32(((HiddenField)gr.FindControl("hdnID")).Value);
            bool result = gr.Selected;
            if (result)
            {
                if (!checkindex.Contains(index))
                    checkindex.Add(index);
            }
        }
        if (checkindex != null && checkindex.Count > 0)
            ViewState["CHECK_ITEMS"] = checkindex;
    }
    private void GetDepositState()
    {
        ArrayList depindex = new ArrayList();
        int index = -1;
        foreach (GridDataItem gr in gvDeposit.Items)
        {
            index = Convert.ToInt32(((HiddenField)gr.FindControl("hdnID")).Value);
            bool result = gr.Selected;
            if (result)
            {
                if (!depindex.Contains(index))
                    depindex.Add(index);
            }
        }
        if (depindex != null && depindex.Count > 0)
            ViewState["DEP_ITEMS"] = depindex;
    }
    private void populateCheck()
    {
        ArrayList checkidex = (ArrayList)ViewState["CHECK_ITEMS"];
        if (checkidex != null && checkidex.Count > 0)
        {
            foreach (GridDataItem gr in gvCheck.Items)
            {
                int index = Convert.ToInt32(((HiddenField)gr.FindControl("hdnID")).Value); ;
                if (checkidex.Contains(index))
                {
                    gr.Selected = true;
                }
            }
        }
        //ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyedit", "chkSelected();", true);
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyedit", "calculateDepChk();", true);
    }
    private void populateDep()
    {
        ArrayList depindex = (ArrayList)ViewState["DEP_ITEMS"];
        if (depindex != null && depindex.Count > 0)
        {
            foreach (GridDataItem gr in gvDeposit.Items)
            {
                int index = Convert.ToInt32(((HiddenField)gr.FindControl("hdnID")).Value); ;
                if (depindex.Contains(index))
                {
                    gr.Selected = true;
                    //gr.Attributes["onclick"] = "SelectRowChk('" + gr.ClientID + "','" + myCheckBox.ClientID + "','" + gvDeposit.ClientID + "',event);";
                }
            }
        }
        //ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyedit", "chkSelected();", true);
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyedit", "calculateDepChk();", true);
    }
    protected void lnkBtnSave_Click(object sender, EventArgs e)
    {
        StoreBankRec();
        TriggerUpdateGrid();
        ScriptManager.RegisterStartupScript(this, GetType(), "calDifference", "calculateAmt();", true);
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "BankRecMsg", "noty({text: 'Data saved successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
    }
    private void StoreBankRec()
    {
        try
        {
            SetInitialRow();
            DataTable dt = (DataTable)ViewState["BankRecon"];


            if (txtEndingBalance.Text == "")
            {
                objBank.Balance = 0;
            }
            else
            {
                objBank.Balance = double.Parse(txtEndingBalance.Text.Replace('$', '0'), NumberStyles.AllowParentheses | NumberStyles.AllowThousands | NumberStyles.AllowDecimalPoint);
            }


            if (!string.IsNullOrEmpty(hdnInterestAcct.Value))
            {
                objBank.InterestAcct = Convert.ToInt32(hdnInterestAcct.Value);
            }
            if (!string.IsNullOrEmpty(hdnServiceAcct.Value))
            {
                objBank.ServiceAcct = Convert.ToInt32(hdnServiceAcct.Value);
            }
            if (!string.IsNullOrEmpty(txtInterestAmount.Text))
            {
                objBank.InterestCharge = Convert.ToDouble(txtInterestAmount.Text);
            }
            if (!string.IsNullOrEmpty(txtServiceChrgAmount.Text))
            {
                objBank.ServiceCharge = Convert.ToDouble(txtServiceChrgAmount.Text);
            }
            if (!string.IsNullOrEmpty(txtInterestDate.Text))
            {
                objBank.InterestDate = Convert.ToDateTime(txtInterestDate.Text);
            }
            if (!string.IsNullOrEmpty(txtServiceChargeDate.Text))
            {
                objBank.ServiceDate = Convert.ToDateTime(txtServiceChargeDate.Text);
            }
            if ((ddlBank.SelectedValue) != "0" && (ddlBank.SelectedValue != ""))
            {
                objBank.ID = Convert.ToInt32(ddlBank.SelectedValue);
            }
            if (!string.IsNullOrEmpty(txtStatementDate.Text))
            {
                objBank.LastReconDate = Convert.ToDateTime(txtStatementDate.Text);
            }

            foreach (GridDataItem gr in gvDeposit.Items)
            {

                HiddenField hdnID = (HiddenField)gr.FindControl("hdnID");
                HiddenField hdnBatch = (HiddenField)gr.FindControl("hdnBatch");

                DataRow dr = null;
                dr = dt.NewRow();
                dr["ID"] = Convert.ToInt32(hdnID.Value);
                if (gr.Selected)
                {
                    dr["Selected"] = 1;
                }
                else
                {
                    dr["Selected"] = 0;
                }
                if (!string.IsNullOrEmpty(hdnBatch.Value))
                {
                    dr["Batch"] = Convert.ToInt32(hdnBatch.Value);
                }

                dt.Rows.Add(dr);
            }
            foreach (GridDataItem gr in gvCheck.Items)
            {

                HiddenField hdnID = (HiddenField)gr.FindControl("hdnID");
                HiddenField hdnBatch = (HiddenField)gr.FindControl("hdnBatch");

                DataRow dr = null;
                dr = dt.NewRow();
                dr["ID"] = Convert.ToInt32(hdnID.Value);
                if (gr.Selected)
                {
                    dr["Selected"] = 1;
                }
                else
                {
                    dr["Selected"] = 0;
                }
                if (!string.IsNullOrEmpty(hdnBatch.Value))
                {
                    dr["Batch"] = Convert.ToInt32(hdnBatch.Value);
                }

                dt.Rows.Add(dr);
            }
            objBank.ConnConfig = Session["config"].ToString();
            objBank.DtBank = dt;
            objBLBank.StoreBankRecon(objBank);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void cvInterestDt_ServerValidate(object source, ServerValidateEventArgs args)
    {
        if (!string.IsNullOrEmpty(txtInterestAmount.Text))
        {
            double interestChrg = Convert.ToDouble(txtInterestAmount.Text);
            if (!interestChrg.Equals(0))
            {
                if (string.IsNullOrEmpty(txtInterestDate.Text))
                {
                    args.IsValid = false;
                }
                else
                    args.IsValid = true;
            }
            else
                args.IsValid = true;
        }
        else
            args.IsValid = true;
    }
    //if (!string.IsNullOrEmpty(txtInterestAmount.Text))
    //    {
    //        double interestChrg = Convert.ToDouble(txtInterestAmount.Text);
    //        if (!interestChrg.Equals(0))
    //        {
    //            args.IsValid = true;
    //        }
    //        else
    //            args.IsValid = false;
    //    }
    //    else
    //        args.IsValid = false;
    protected void cvSCDt_ServerValidate(object source, ServerValidateEventArgs args)
    {
        if (!string.IsNullOrEmpty(txtServiceChrgAmount.Text))
        {
            double SC = Convert.ToDouble(txtServiceChrgAmount.Text);
            if (!SC.Equals(0))
            {
                if (string.IsNullOrEmpty(txtServiceChargeDate.Text))
                {
                    args.IsValid = false;
                }
                else
                    args.IsValid = true;
            }
            else
                args.IsValid = true;
        }
        else
            args.IsValid = true;
    }
    protected void lnkRefresh_Click(object sender, EventArgs e)
    {
        try
        {
            StoreBankRec();
            DateTime statementDate = SetStatementDate();
            TriggerUpdateGrid();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    protected void lnkExportPdf_Click(object sender, EventArgs e)
    {
        byte[] buffer = null;
        buffer = ExportReportToPDF("", rvBankRecon);
        Response.ClearContent();
        Response.ClearHeaders();
        Response.AddHeader("Content-Disposition", "attachment;filename=Bank reconciliation " + Convert.ToDateTime(txtStatementDate.Text).ToString("yyyy-MM-dd") + ".pdf");
        Response.ContentType = "application/pdf";
        Response.AddHeader("Content-Length", (buffer.Length).ToString());
        Response.BinaryWrite(buffer);
        Response.Flush();
        Response.Close();
    }

    protected void ddlCompany_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlBank.Items.Clear();
        objBank.ConnConfig = Session["config"].ToString();

        DataSet _dsBank = new DataSet();

        if (Session["COPer"].ToString() == "1")
        {
            if (ddlCompany.SelectedValue != "0")
            {

                objBank.EN = Convert.ToInt32(ddlCompany.SelectedValue);
                _dsBank = objBLBank.GetAllBankNamesByCompany(objBank);

            }
            else
            {


                _dsBank = objBLBank.GetAllBankNames(objBank);
            }

        }
        else
        {


            _dsBank = objBLBank.GetAllBankNames(objBank);
        }

        if (_dsBank.Tables[0].Rows.Count > 0)
        {
            ddlBank.Items.Add(new System.Web.UI.WebControls.ListItem(":: Select ::", "0"));
            ddlBank.AppendDataBoundItems = true;
            ddlBank.DataSource = _dsBank;
            ddlBank.DataValueField = "ID";
            ddlBank.DataTextField = "fDesc";
            ddlBank.DataBind();
        }
        else
        {
            ddlBank.Items.Add(new System.Web.UI.WebControls.ListItem("No data found", "0"));
        }

        ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "UpdateLabel", "$(function () {{UpdateLabel();}});", true);

    }

    protected void gvCheck_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            var statementDate = DateTime.Parse(txtStatementDate.Text);
            _objCD.ConnConfig = Session["config"].ToString();
            _objCD.fDate = statementDate;
            //_objCD.fDateYear = _statementDate.Year;
            if ((ddlBank.SelectedValue) != "0" && (ddlBank.SelectedValue != ""))
            {
                _objCD.Bank = Convert.ToInt32(ddlBank.SelectedValue);
            }
            DataSet ds = _objBL_Bills.GetChecksDetails(_objCD);





            ViewState["Check"] = ds.Tables[0];
            var dt = ds.Tables[0];
            Double checkAmount = 0;
            int countCheck = 0;

            //indexChange =1: ddlBank change
            if (!IsPostBack)
            {
                hdnListCredit.Value = "";
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["Status"].ToString() == "T")
                    {
                        if (hdnListCredit.Value == "")
                        {
                            hdnListCredit.Value = hdnListCredit.Value + dt.Rows[i]["ID"].ToString();

                        }
                        else
                        {
                            hdnListCredit.Value = hdnListCredit.Value + "," + dt.Rows[i]["ID"].ToString();
                        }
                        checkAmount = checkAmount + Convert.ToDouble(dt.Rows[i]["Amount"]) * (-1);
                        countCheck = countCheck + 1;
                    }
                }

            }
            else
            {
                if (controlChange != 0)
                {
                    hdnListCredit.Value = "";
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i]["Status"].ToString() == "T")
                        {
                            if (hdnListCredit.Value == "")
                            {
                                hdnListCredit.Value = hdnListCredit.Value + dt.Rows[i]["ID"].ToString();

                            }
                            else
                            {
                                hdnListCredit.Value = hdnListCredit.Value + "," + dt.Rows[i]["ID"].ToString();
                            }
                            checkAmount = checkAmount + Convert.ToDouble(dt.Rows[i]["Amount"]) * (-1);
                            countCheck = countCheck + 1;
                        }
                    }

                }
                else
                {
                    String[] ls = hdnListCredit.Value.Split(',');
                    List<int> lsID = new List<int>();
                    foreach (string str in ls)
                    {
                        if (str != "")
                        {
                            lsID.Add(Convert.ToInt32(str));
                        }
                    }


                    foreach (DataRow dr in dt.Rows)
                    {
                        if (lsID.Contains(Convert.ToInt32(dr["ID"])))
                        {
                            dr["Status"] = "T";
                            checkAmount = checkAmount + Convert.ToDouble(dr["Amount"]) * (-1);
                            countCheck = countCheck + 1;
                        }
                        else
                        {
                            dr["Status"] = "";
                        }
                    }
                }


            }
            lblCheckAmount.Text = string.Format("{0:c}", Convert.ToDouble(checkAmount));
            lblCheckCount.Text = countCheck.ToString();
            gvCheck.DataSource = dt;
            gvCheck.VirtualItemCount = dt.Rows.Count;
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyedit", "calculateDepChk();", true);

        }
        catch (Exception ex)
        {
            gvCheck.DataSource = string.Empty;
            gvCheck.VirtualItemCount = 0;
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true})", true);
        }
    }

    protected void gvDeposit_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            var statementDate = DateTime.Parse(txtStatementDate.Text);

            _objDep.ConnConfig = Session["config"].ToString();
            _objDep.fDate = statementDate;
            //_objDep.fDateYear = _statementDate.Year;
            if ((ddlBank.SelectedValue) != "0" && (ddlBank.SelectedValue != ""))
            {
                _objDep.Bank = Convert.ToInt32(ddlBank.SelectedValue);
            }
            DataSet ds = _objBL_Deposit.GetDepositDetails(_objDep);
            var dt = ds.Tables[0];

            Double depositAmount = 0;
            int countDeposit = 0;
            if (!IsPostBack)
            {
                hdnListDebit.Value = "";
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["Status"].ToString() == "T")
                    {
                        if (hdnListDebit.Value == "")
                        {
                            hdnListDebit.Value = hdnListDebit.Value + dt.Rows[i]["ID"].ToString();

                        }
                        else
                        {
                            hdnListDebit.Value = hdnListDebit.Value + "," + dt.Rows[i]["ID"].ToString();
                        }
                        depositAmount = depositAmount + Convert.ToDouble(dt.Rows[i]["Amount"]);
                        countDeposit = countDeposit + 1;
                    }
                }
            }
            else
            {
                if (controlChange != 0)
                {
                    hdnListDebit.Value = "";
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i]["Status"].ToString() == "T")
                        {
                            if (hdnListDebit.Value == "")
                            {
                                hdnListDebit.Value = hdnListDebit.Value + dt.Rows[i]["ID"].ToString();

                            }
                            else
                            {
                                hdnListDebit.Value = hdnListDebit.Value + "," + dt.Rows[i]["ID"].ToString();
                            }
                            depositAmount = depositAmount + Convert.ToDouble(dt.Rows[i]["Amount"]);
                            countDeposit = countDeposit + 1;
                        }
                    }
                }
                else
                {
                    String[] ls = hdnListDebit.Value.Split(',');
                    List<int> lsID = new List<int>();
                    foreach (string str in ls)
                    {
                        if (str != "")
                        {
                            lsID.Add(Convert.ToInt32(str));
                        }
                    }


                    foreach (DataRow dr in dt.Rows)
                    {
                        if (lsID.Contains(Convert.ToInt32(dr["ID"])))
                        {
                            dr["Status"] = "T";
                            depositAmount = depositAmount + Convert.ToDouble(dr["Amount"]);
                            countDeposit = countDeposit + 1;
                        }
                        else
                        {
                            dr["Status"] = "";
                        }
                    }
                }

            }
            lblDepositAmount.Text = string.Format("{0:c}", Convert.ToDouble(depositAmount));
            lblDepositCount.Text = countDeposit.ToString();
            ViewState["Deposit"] = dt;
            gvDeposit.DataSource = dt;
            gvDeposit.VirtualItemCount = dt.Rows.Count;
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyedit", "calculateDepChk();", true);
        }
        catch (Exception ex)
        {
            gvDeposit.DataSource = string.Empty;
            gvDeposit.VirtualItemCount = 0;
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true})", true);
        }
    }

    protected void gvCheck_ItemCreated(object sender, GridItemEventArgs e)
    {
        HandlePagination(e);
    }

    protected void gvDeposit_ItemCreated(object sender, GridItemEventArgs e)
    {
        HandlePagination(e);
    }

    private void HandlePagination(GridItemEventArgs e)
    {
        if (e.Item is GridPagerItem)
        {
            var dropDown = (RadComboBox)e.Item.FindControl("PageSizeComboBox");
            var totalCount = ((GridPagerItem)e.Item).Paging.DataSourceCount;
            GeneralFunctions obj = new GeneralFunctions();
            var sizes = obj.TelerikPageSize(totalCount);
            dropDown.Items.Clear();

            foreach (var size in sizes)
            {
                var cboItem = new RadComboBoxItem() { Text = size.Key, Value = size.Value };
                cboItem.Attributes.Add("ownerTableViewId", e.Item.OwnerTableView.ClientID);
                dropDown.Items.Add(cboItem);
            }

            dropDown.FindItemByValue(e.Item.OwnerTableView.PageSize.ToString()).Selected = true;
        }
    }

    protected void gvDeposit_PreRender(object sender, EventArgs e)
    {
        CorrectGrid(gvDeposit);
    }

    protected void gvCheck_PreRender(object sender, EventArgs e)
    {
        CorrectGrid(gvCheck);
    }

    private void CorrectGrid(RadGrid radGrid)
    {
        GeneralFunctions obj = new GeneralFunctions();
        //TODO Quant: Waiting for the feedback from client
        //obj.CorrectTelerikPager(radGrid);        
    }

    protected void lnkBankRecReport_Click(object sender, EventArgs e)
    {
        try
        {
            CreateBankRecReportRow();

            DataTable dtBank = (DataTable)ViewState["BankReconReport"];
            foreach (GridDataItem gr in gvCheck.Items)
            {

                Label lblfDate = (Label)gr.FindControl("lblfDate");
                Label lblfDesc = (Label)gr.FindControl("lblfDesc");
                Label lblAmount = (Label)gr.FindControl("lblAmount");
                Label lblRef = (Label)gr.FindControl("lblRef");
                Label lblType = (Label)gr.FindControl("lblType1");
                HiddenField hdnAmount = (HiddenField)gr.FindControl("hdnAmount");
                Label Isselected = (Label)gr.FindControl("lblType1");
                HiddenField hdnBatch = (HiddenField)gr.FindControl("hdnBatch");
                HiddenField hdnTypeNum = (HiddenField)gr.FindControl("hdnTypeNum");
                HiddenField hdnID = (HiddenField)gr.FindControl("hdnID");

                DataRow drbank = null;
                drbank = dtBank.NewRow();
                drbank["ID"] = Convert.ToInt32(hdnID.Value);
                drbank["fDate"] = Convert.ToDateTime(lblfDate.Text);
                drbank["TypeNum"] = Convert.ToInt32(hdnTypeNum.Value);
                drbank["Ref"] = lblRef.Text;
                drbank["fDesc"] = lblfDesc.Text;
                if (Convert.ToDouble(hdnAmount.Value) > 0)
                {
                    drbank["Type"] = "Deposit/Debit";
                }
                else
                {
                    drbank["Type"] = "Check/Credit";
                }
                if (Convert.ToDouble(hdnAmount.Value) > 0)
                {
                    drbank["Debits"] = Convert.ToDouble(hdnAmount.Value);
                    drbank["Amount"] = Convert.ToDouble(hdnAmount.Value);
                }
                else
                {
                    drbank["Credits"] = Convert.ToDouble(hdnAmount.Value) * -1;
                    drbank["Amount"] = Convert.ToDouble(hdnAmount.Value) * -1;
                }

                drbank["Batch"] = Convert.ToInt32(hdnBatch.Value);
                drbank["Selected"] = gr.Selected;
                if (gr.Selected)
                {
                    drbank["Short"] = 1;
                }
                else
                {
                    drbank["Short"] = 0;
                }
                dtBank.Rows.Add(drbank);
            }

            foreach (GridDataItem gr in gvDeposit.Items)
            {

                Label lblfDate = (Label)gr.FindControl("lblfDate");
                Label lblfDesc = (Label)gr.FindControl("lblfDesc");
                Label lblAmount = (Label)gr.FindControl("lblAmount");
                Label lblRef = (Label)gr.FindControl("lblRef");
                Label lblType = (Label)gr.FindControl("lblType");
                HiddenField hdnAmount = (HiddenField)gr.FindControl("hdnAmount");

                HiddenField hdnBatch = (HiddenField)gr.FindControl("hdnBatch");
                HiddenField hdnTypeNum = (HiddenField)gr.FindControl("hdnTypeNum");
                HiddenField hdnID = (HiddenField)gr.FindControl("hdnID");

                DataRow drbank = null;
                drbank = dtBank.NewRow();
                drbank["ID"] = Convert.ToInt32(hdnID.Value);
                drbank["fDate"] = Convert.ToDateTime(lblfDate.Text);
                drbank["fDesc"] = lblfDesc.Text;
                if (Convert.ToDouble(hdnAmount.Value) > 0)
                {
                    drbank["Type"] = "Deposit/Debit";
                }
                else
                {
                    drbank["Type"] = "Check/Credit";
                }

                if (Convert.ToDouble(hdnAmount.Value) > 0)
                {
                    drbank["Debits"] = Convert.ToDouble(hdnAmount.Value);
                }
                else
                {
                    drbank["Credits"] = Convert.ToDouble(hdnAmount.Value) * -1;
                }

                drbank["TypeNum"] = Convert.ToInt32(hdnTypeNum.Value);
                drbank["Ref"] = lblRef.Text;
                drbank["Amount"] = Convert.ToDouble(hdnAmount.Value);
                drbank["Batch"] = Convert.ToInt32(hdnBatch.Value);
                drbank["Selected"] = gr.Selected;
                if (gr.Selected)
                {
                    drbank["Short"] = 1;
                }
                else
                {
                    drbank["Short"] = 0;
                }
                dtBank.Rows.Add(drbank);
            }
            Session["BankReconCheck"] = dtBank;

            DataTable dtBankDetail = (DataTable)ViewState["BankRecon"];
            objBank.ConnConfig = Session["config"].ToString();
            objBank.ID = Convert.ToInt32(ddlBank.SelectedValue);
            objBank.Balance = double.Parse(txtEndingBalance.Text.Replace('$', '0'), NumberStyles.AllowParentheses |
                                                                    NumberStyles.AllowThousands |
                                                                    NumberStyles.AllowDecimalPoint);
            objBank.LastReconDate = DateTime.Now;
            if (!string.IsNullOrEmpty(hdnServiceAcct.Value))
            {
                objBank.ServiceAcct = Convert.ToInt32(hdnServiceAcct.Value);
            }
            if (!string.IsNullOrEmpty(txtServiceChargeDate.Text))
            {
                objBank.ServiceDate = Convert.ToDateTime(txtServiceChargeDate.Text);
            }
            if (!string.IsNullOrEmpty(txtServiceChrgAmount.Text))
            {
                objBank.ServiceCharge = Convert.ToDouble(txtServiceChrgAmount.Text);
            }
            if (!string.IsNullOrEmpty(hdnInterestAcct.Value))
            {
                objBank.InterestAcct = Convert.ToInt32(hdnInterestAcct.Value);
            }
            if (!string.IsNullOrEmpty(txtInterestDate.Text))
            {
                objBank.InterestDate = Convert.ToDateTime(txtInterestDate.Text);
            }
            if (!string.IsNullOrEmpty(txtInterestAmount.Text))
            {
                objBank.InterestCharge = Convert.ToDouble(txtInterestAmount.Text);
            }
            if (!string.IsNullOrEmpty(txtStatementDate.Text))
            {
                objBank.fDate = Convert.ToDateTime(txtStatementDate.Text);
            }
            objBank.DtBank = dtBank;
            objBank.ConnConfig = Session["config"].ToString();
            objBank.ID = Convert.ToInt32(ddlBank.SelectedValue);
            objBank.fDate = Convert.ToDateTime(txtStatementDate.Text);
            DataSet dsBank = objBLBank.GetBankDetailByDate(objBank);

            Session["BankReconDetail"] = dsBank.Tables[0];
            string urlString = "BankRecReport.aspx";
            Response.Redirect(urlString, true);
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
}