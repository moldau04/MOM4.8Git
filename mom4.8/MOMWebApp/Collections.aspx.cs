using BusinessEntity;
using BusinessLayer;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Collections : System.Web.UI.Page
{
    #region Variables
    
    Contracts objContract = new Contracts();
    BL_Contracts objBL_Contracts = new BL_Contracts();

    Customer objCustomer = new Customer();
    BL_Customer objBL_Customer = new BL_Customer();

    User objPropUser = new User();
    BL_User objBL_User = new BL_User();

    BL_General objBL_General = new BL_General();
    General objGeneral = new General();

    private const string asc = " ASC";
    private const string desc = " DESC";
    public int loc = 0;
    int count = 0;
    bool IsGst = false;

    #endregion

    #region events

    #region #PAGELOAD
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
                //DateTime _now = DateTime.Now;
                //var _startDate = new DateTime(_now.Year, _now.Month, 1);
                //var _endDate = _startDate.AddMonths(1).AddDays(-1);

                BindInvoices();
                ShowGstRate();
                Session["InvoiceName"] = "Invoice";
                //txtSearchDate.Visible = false;
            }
            CompanyPermission();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    #endregion
   
    protected void Page_PreRender(Object o, EventArgs e)
    {
        foreach (GridViewRow gr in gvOverDueInvoice.Rows)
        {
            CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");
            Label lblTransID = (Label)gr.FindControl("lblTransID");
            Label lblUrl = (Label)gr.FindControl("lblUrl");

            if(lblTransID.Text != "0")
            {
                AjaxControlToolkit.HoverMenuExtender hmeRes = (AjaxControlToolkit.HoverMenuExtender)gr.FindControl("hmeRes");
                gr.Attributes["onclick"] = "SelectRowChk('" + gr.ClientID + "','" + chkSelect.ClientID + "','" + gvOverDueInvoice.ClientID + "',event);";
                gr.Attributes["ondblclick"] = "window.open('" + lblUrl.Text + "');";
            }
        }
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyedit", "SelectedRowStyle('" + gvOverDueInvoice.ClientID + "');", true);
    }
    protected void lnkClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("home.aspx");
    }
    protected void gvOverDueInvoice_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            int Index = e.Row.RowIndex;
            objContract.ConnConfig = Session["config"].ToString();

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                GridViewRow selectedRow = e.Row;
                Label lblLoc = (Label)e.Row.FindControl("lblLoc");
                Label lblTransID = (Label)e.Row.FindControl("lblTransID");

                if (lblTransID.Text.ToString() == "0")
                {
                    int tbrowcount = e.Row.RowIndex - 1;

                    //GridViewRow row = new GridViewRow(-1, -1, DataControlRowType.DataRow, DataControlRowState.Normal);
                    //GridViewRow previousRow = new GridViewRow(e.Row.RowIndex - 1, -1, DataControlRowType.DataRow, DataControlRowState.Normal);
                    //if (e.Row.RowIndex != 0)
                    //{
                    //    add blank row
                    //    Table ntable = previousRow.Parent as Table;
                    //    TableCell newcell = new TableCell();
                    //    previousRow.ID = "NewRow" + tbrowcount.ToString();
                    //    newcell.ColumnSpan = 11;
                    //    newcell.Font.Size = 8;
                    //    newcell.BackColor = System.Drawing.Color.White;
                    //    newcell.Font.Bold = true;
                    //    newcell.HorizontalAlign = HorizontalAlign.Left;
                    //    newcell.Controls.Add(new LiteralControl(""));
                    //    //previousRow.Cells.Add(newcell);
                    //    //ntable.Rows.Add(previousRow);
                    //}

                    e.Row.Font.Bold = true;
                    //e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#e9f0f6");
                    //e.Row.Attributes["style"] = "background-color: #e9f0f6";
                    e.Row.CssClass = "customer-row";
                    CreateDetailRow(e.Row);
                }
                else
                {
                    e.Row.BackColor = System.Drawing.Color.White;
                    e.Row.Attributes["style"] = "background-color: white";
                    e.Row.Cells[12].Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    protected void ddlPages_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridViewRow gvrPager = gvOverDueInvoice.BottomPagerRow;
        DropDownList ddlPages = (DropDownList)gvrPager.Cells[0].FindControl("ddlPages");

        gvOverDueInvoice.PageIndex = ddlPages.SelectedIndex;

        // a method to populate your grid
        //FillGridPaged();
    }
    protected void gvOverDueInvoice_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {
            string _sortExpression = e.SortExpression;

            if (GvSortDirection == SortDirection.Ascending)
            {
                GvSortDirection = SortDirection.Descending;
                SortGridView(_sortExpression, desc);
            }
            else
            {
                GvSortDirection = SortDirection.Ascending;
                SortGridView(_sortExpression, asc);
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    protected void gvOverDueInvoice_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvOverDueInvoice.PageIndex = e.NewPageIndex;

            if (ViewState["Overdue"] != null)
            {
                gvOverDueInvoice.DataSource = (DataTable)ViewState["Overdue"];
                gvOverDueInvoice.DataBind();
                SetDueInvoiceTotal();
            }
            else
            {
                BindInvoices();
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    protected void gvOverDueInvoice_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            PaginateDueInvoice(sender, e);
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    protected void lnkSaveNote_Click(object sender, EventArgs e)
    {
        try
        {
            if (!string.IsNullOrEmpty(hdnOwner.Value))
            {
                objCustomer.ConnConfig = Session["config"].ToString();
                objCustomer.CustomerID = Convert.ToInt32(hdnOwner.Value);
                objCustomer.RenewalNotes = txtCollectionNote.Text;
                objBL_Customer.UpdateCustomerCollectionNote(objCustomer);
                BindInvoices();
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    protected void lnkSaveContact_Click(object sender, EventArgs e)
    {
        try
        {
            if(!string.IsNullOrEmpty(hdnOwner1.Value))
            {
                objCustomer.ConnConfig = Session["config"].ToString();
                objCustomer.CustomerID = Convert.ToInt32(hdnOwner1.Value);
                objCustomer.Contact = txtContact.Text;
                objCustomer.Email = txtEmail.Text;
                objCustomer.Cellular = txtCell.Text;
                objCustomer.Phone = txtPhone.Text;
                objCustomer.Fax = txtFax.Text;
                objBL_Customer.UpdateCustomerContact(objCustomer);
                BindInvoices();
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    protected void lnkMailCustomerStatement_Click(object sender, EventArgs e)
    {
        try
        {
            GetCustomerStatement();
            string _todayDate = DateTime.Now.Date.ToString("MM-dd-yyyy");
            DataTable dtInv = (DataTable)ViewState["CustomerStatementResult"];
            string funcString = string.Empty;
            if (dtInv.Rows.Count > 0)
            {
                DataTable dtC = (DataTable)ViewState["Company"];
                string strCompany = dtC.Rows[0]["name"].ToString() + Environment.NewLine;
                strCompany += dtC.Rows[0]["Address"].ToString() + Environment.NewLine;
                strCompany += dtC.Rows[0]["city"].ToString() + ", " + dtC.Rows[0]["state"].ToString() + ", " + dtC.Rows[0]["zip"].ToString() + Environment.NewLine;
                strCompany += "Phone: " + dtC.Rows[0]["Phone"].ToString() + Environment.NewLine;
                strCompany += "Fax: " + dtC.Rows[0]["fax"].ToString() + Environment.NewLine;
                strCompany += "Email: " + dtC.Rows[0]["email"].ToString() + Environment.NewLine;
                strCompany = "Please review the attached customer statement from: " + Environment.NewLine + Environment.NewLine + strCompany;


                string fromEmail;
                if (ViewState["EmailFrom"] == null)
                {
                    fromEmail = WebBaseUtility.GetFromEmailAddress();
                }
                else
                {
                    fromEmail = ViewState["EmailFrom"].ToString();
                }
                List<string> lstLoc = new List<string>();
                //string strLoc;

                DataTable dtFilter = new DataTable();
                var rows = dtInv.AsEnumerable()
                  .Where(x => x.Field<int>("IsExistsEmail").Equals(1));
                if (rows.Any())
                    dtFilter = rows.CopyToDataTable();

                var groupLoc = (
                                from DataRow row in dtFilter.AsEnumerable()
                                select new
                                {
                                    loc = row.Field<Int32>("Loc")
                                }
                             ).Distinct().AsEnumerable();

                foreach (var g in groupLoc)
                {
                    count = 0;
                    string toEmail = "";
                    string ccEmail = "";

                    #region Generate Report

                    int loc = Convert.ToInt32(g.loc);
                    DataTable dtLoc = dtFilter
                                        .Select("Loc = " + loc)
                                        .CopyToDataTable();

                    ViewState["CustomerStatementSub"] = dtLoc;   // Dataset for subreport
                    ReportViewer rvCs = new ReportViewer();

                    rvCs.LocalReport.DataSources.Clear();

                    rvCs.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(ItemDetailsSubReportProcessing);
                    rvCs.LocalReport.DataSources.Add(new ReportDataSource("dsInvoice", dtLoc));
                    rvCs.LocalReport.DataSources.Add(new ReportDataSource("dsCompany", dtC));

                    string reportPath = "Reports/CustomerStatement.rdlc";

                    string Report = string.Empty;
                    if (!string.IsNullOrEmpty(Report.Trim()))
                    {
                        reportPath = "Reports/" + Report.Trim();
                    }
                    rvCs.LocalReport.ReportPath = reportPath;
                    rvCs.LocalReport.EnableExternalImages = true;
                    List<Microsoft.Reporting.WebForms.ReportParameter> param1 = new List<Microsoft.Reporting.WebForms.ReportParameter>();
                    string strPath = "file:///" + Server.MapPath(Request.ApplicationPath).Replace("\\", "/");
                    param1.Add(new Microsoft.Reporting.WebForms.ReportParameter("path", strPath + "/images/Company_logo.jpg"));

                    rvCs.LocalReport.SetParameters(param1);

                    rvCs.LocalReport.Refresh();

                    #endregion

                    #region Email

                    toEmail = dtLoc.Rows[0]["custom12"].ToString();

                    if (!string.IsNullOrEmpty(dtLoc.Rows[0]["custom13"].ToString()))
                    {
                        ccEmail = dtLoc.Rows[0]["custom13"].ToString();
                    }

                    List<string> toEmaillst = new List<string>();
                    toEmaillst.Add(toEmail);
                    List<string> ccEmaillst = new List<string>();
                    ccEmaillst.Add(ccEmail);

                    Mail mail = new Mail();
                    mail.From = fromEmail;
                    mail.To = toEmaillst;
                    mail.Cc = ccEmaillst;

                    mail.Title = "Customer Statement - " + dtLoc.Rows[0]["LocID"].ToString() + " " + dtLoc.Rows[0]["locname"].ToString();

                    //mail.Text = ViewState["CompanyAddress"].ToString().Replace(Environment.NewLine, "<BR/>");

                    mail.Text = strCompany.Replace(Environment.NewLine, "<BR/>");
                    mail.attachmentBytes = ExportReportToPDF1("", rvCs);
                    mail.FileName = "CustomerStatement_" + _todayDate + ".pdf";

                    mail.DeleteFilesAfterSend = true;
                    mail.RequireAutentication = false;
                    // ES-33
                    WebBaseUtility.UpdateMailConfigurationFromLoginUser(mail);
                    mail.Send();

                    #endregion
                    funcString = "noty({text: 'Email sent successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});";
                }
                #region comment
                //var rows1 = dtInv.AsEnumerable()
                //                .Where(x => x.Field<int>("IsExistsEmail").Equals(0));
                //string strLoc;
                //strLoc = string.Join(", ", rows1.AsEnumerable()
                //                     .Select(x => x["locname"].ToString())
                //                     .ToArray());

                //if (!string.IsNullOrEmpty(strLoc))
                //{
                //    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "unsuccessMesg('" + strLoc + "');", true);
                //} 
                #endregion


                #region Check Loc Email Not Available

                DataTable dtEMail = new DataTable();
                var rows1 = dtInv.AsEnumerable()
                        .Where(x => x.Field<int>("IsExistsEmail").Equals(0));
                if (rows1.Any())
                    dtEMail = rows1.CopyToDataTable();

                
                if (dtEMail.Rows.Count > 0)
                {
                    funcString = "dispWarningMesg();";
                }

                #endregion

                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", funcString, true);

            }
            else
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "noty({text: 'No Invoice(s) found to email!',  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
            }
            
            //ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "noty({text: 'Customer statement report mail sent successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});dispWarningMesg();", true);
        }
        catch (Exception ex)
        {
            //string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(ex.Message);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    protected void lnkMailInvoice_Click(object sender, EventArgs e)
    {
        try
        {
            Session["InvoiceName"] = "Invoice";
            MailInvoices();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    protected void lnkMailInvoiceMain_Click(object sender, EventArgs e)
    {
        try
        {
            Session["InvoiceName"] = "InvoiceMaint";
            MailInvoices();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    protected void lnkMailInvoiceException_Click(object sender, EventArgs e)
    {
        try
        {
            Session["InvoiceName"] = "InvoiceException";
            MailInvoices();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    protected void btnYes_Click(object sender, EventArgs e)
    {
        try
        {
            PrintOnlyCustomerStatement();
            //ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "hideModel();", true);
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    protected void btnYesInv_Click(object sender, EventArgs e)
    {
        try
        {
            PrintOnlyInvoice();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
   
    #endregion

    
    #region Custom functions
   
    private void BindInvoices()
    {
        try
        {
            objContract.UserID = Session["UserID"].ToString();
            objContract.ConnConfig = Session["config"].ToString();
            DataSet ds = new DataSet();
            if (Session["CmpChkDefault"].ToString() == "1")
            {               
                ds = objBL_Contracts.GetCollectionInvoicesCompany(objContract);
            }
            else
            {
                ds = objBL_Contracts.GetCollectionInvoices(objContract);
            }
            gvOverDueInvoice.DataSource = ds.Tables[0];
            gvOverDueInvoice.DataBind();
            ViewState["Overdue"] = ds.Tables[0];
            SetDueInvoiceTotal();

            //gvUpcomingInv.DataSource = ds.Tables[1];
            //gvUpcomingInv.DataBind();
            //ViewState["Upcoming"] = ds.Tables[1];
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    private void CompanyPermission()
    {
        if (Session["COPer"].ToString() == "1")
        {
           gvOverDueInvoice.Columns[11].Visible = true;           
        }
        else
        {
            gvOverDueInvoice.Columns[11].Visible = false;
            Session["CmpChkDefault"] = "2";
        }
    }
    private void SetDueInvoiceTotal()
    {
        try
        {
            DataTable dt = (DataTable)ViewState["Overdue"];
            if (dt.Rows.Count > 0)
            {
                double Balance = Convert.ToDouble(dt.Compute("Sum(AgingAmount)", string.Empty));
                double Balance0 = Convert.ToDouble(dt.Compute("Sum(Amount0)", string.Empty));
                double Balance30 = Convert.ToDouble(dt.Compute("Sum(Amount30)", string.Empty));
                double Balance60 = Convert.ToDouble(dt.Compute("Sum(Amount60)", string.Empty));
                double Balance90 = Convert.ToDouble(dt.Compute("Sum(Amount90)", string.Empty));
                double Balance121 = Convert.ToDouble(dt.Compute("Sum(Amount121)", string.Empty));

                if (gvOverDueInvoice.FooterRow != null)
                {
                    Label lblTotalAgingAmt = gvOverDueInvoice.FooterRow.FindControl("lblTotalAgingAmt") as Label;
                    Label lblTotal0 = gvOverDueInvoice.FooterRow.FindControl("lblTotal0") as Label;
                    Label lblTotal30 = gvOverDueInvoice.FooterRow.FindControl("lblTotal30") as Label;
                    Label lblTotal60 = gvOverDueInvoice.FooterRow.FindControl("lblTotal60") as Label;
                    Label lblTotal90 = gvOverDueInvoice.FooterRow.FindControl("lblTotal90") as Label;
                    Label lblTotal121 = gvOverDueInvoice.FooterRow.FindControl("lblTotal121") as Label;

                    lblTotalAgingAmt.Text = string.Format("{0:c}", Balance);
                    lblTotal0.Text = string.Format("{0:c}", Balance0);
                    lblTotal30.Text = string.Format("{0:c}", Balance30);
                    lblTotal60.Text = string.Format("{0:c}", Balance60);
                    lblTotal90.Text = string.Format("{0:c}", Balance90);
                    lblTotal121.Text = string.Format("{0:c}", Balance121);
                }
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    protected void CreateDetailRow(GridViewRow gr)
    {
        try
        {
            int rowIndex = gr.RowIndex;
            Table table = gr.Parent as Table;
            if (table != null)
            {
                CreateRow(table, rowIndex);
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    private void CreateRow(Table table, int rowIndex)
    {
        try
        {
            int tbrowcount = table.Rows.Count;
            GridViewRow row = new GridViewRow(-1, -1, DataControlRowType.DataRow, DataControlRowState.Normal);
            row.ID = "NewRow" + rowIndex.ToString();
            //row.Cells.Add(CreateColumn(rowIndex));
            CreateColumn(rowIndex, row);
            table.Rows.Add(row);
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    private GridViewRow CreateColumn(int rowIndex, GridViewRow row)
    {
        try
        {
            TableCell cell1 = new TableCell();
            cell1.ColumnSpan = 0;
            //cell1.Width = Unit.Percentage(10);
            cell1.Font.Size = 8;
            cell1.BackColor = System.Drawing.Color.FromName("#f4f7f9");
            cell1.Font.Bold = true;
            cell1.HorizontalAlign = HorizontalAlign.Left;
            cell1.Controls.Add(new LiteralControl(""));
            row.Cells.Add(cell1);

            TableCell cell2 = new TableCell();
            cell2.Font.Size = 8;
            cell2.BackColor = System.Drawing.Color.FromName("#f4f7f9");
            cell2.Font.Bold = true;
            cell2.HorizontalAlign = HorizontalAlign.Left;
            cell2.Controls.Add(new LiteralControl("Location"));
            row.Cells.Add(cell2);

            TableCell cell3 = new TableCell();
            cell3.Font.Size = 8;
            cell3.BackColor = System.Drawing.Color.FromName("#f4f7f9");
            cell3.Font.Bold = true;
            cell3.HorizontalAlign = HorizontalAlign.Left;
            cell3.Controls.Add(new LiteralControl("Invoice#"));
            row.Cells.Add(cell3);

            TableCell cell4 = new TableCell();
            cell4.Font.Size = 8;
            cell4.BackColor = System.Drawing.Color.FromName("#f4f7f9");
            cell4.Font.Bold = true;
            cell4.HorizontalAlign = HorizontalAlign.Left;
            cell4.Controls.Add(new LiteralControl("Date"));
            row.Cells.Add(cell4);

            TableCell cell5 = new TableCell();
            cell5.Font.Size = 8;
            cell5.BackColor = System.Drawing.Color.FromName("#f4f7f9");
            cell5.Font.Bold = true;
            cell5.HorizontalAlign = HorizontalAlign.Left;
            cell5.Controls.Add(new LiteralControl("Due Days"));
            row.Cells.Add(cell5);

            TableCell cell6 = new TableCell();
            cell6.Font.Size = 8;
            cell6.BackColor = System.Drawing.Color.FromName("#f4f7f9");
            cell6.Font.Bold = true;
            cell6.HorizontalAlign = HorizontalAlign.Left;
            cell6.Controls.Add(new LiteralControl("Balance"));
            row.Cells.Add(cell6);

            TableCell cell7 = new TableCell();
            cell7.Font.Size = 8;
            cell7.BackColor = System.Drawing.Color.FromName("#f4f7f9");
            cell7.Font.Bold = true;
            cell7.HorizontalAlign = HorizontalAlign.Left;
            cell7.Controls.Add(new LiteralControl("0 - 30"));
            row.Cells.Add(cell7);

            TableCell cell8 = new TableCell();
            cell8.Font.Size = 8;
            cell8.BackColor = System.Drawing.Color.FromName("#f4f7f9");
            cell8.Font.Bold = true;
            cell8.HorizontalAlign = HorizontalAlign.Left;
            cell8.Controls.Add(new LiteralControl("31 - 60"));
            row.Cells.Add(cell8);

            TableCell cell9 = new TableCell();
            cell9.Font.Size = 8;
            cell9.BackColor = System.Drawing.Color.FromName("#f4f7f9");
            cell9.Font.Bold = true;
            cell9.HorizontalAlign = HorizontalAlign.Left;
            cell9.Controls.Add(new LiteralControl("61 - 90"));
            row.Cells.Add(cell9);

            TableCell cell10 = new TableCell();
            cell10.Font.Size = 8;
            cell10.BackColor = System.Drawing.Color.FromName("#f4f7f9");
            cell10.Font.Bold = true;
            cell10.HorizontalAlign = HorizontalAlign.Left;
            cell10.Controls.Add(new LiteralControl("91 - 120"));
            row.Cells.Add(cell10);

            TableCell cell11 = new TableCell();
            cell11.Font.Size = 8;
            cell11.BackColor = System.Drawing.Color.FromName("#f4f7f9");
            cell11.Font.Bold = true;
            cell11.HorizontalAlign = HorizontalAlign.Left;
            cell11.Controls.Add(new LiteralControl("121+"));
            row.Cells.Add(cell11);

            if (Session["COPer"].ToString() == "1")
            {
                TableCell cell12 = new TableCell();
                cell12.Font.Size = 8;
                cell12.BackColor = System.Drawing.Color.FromName("#f4f7f9");
                cell12.Font.Bold = true;
                cell12.HorizontalAlign = HorizontalAlign.Left;
                cell12.Controls.Add(new LiteralControl("Company"));
                row.Cells.Add(cell12);
            }
        }
        catch(Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
        return row;
    }
    private void SortGridView(string sortExpression, string direction)
    {
        try
        {
            DataTable dt = PageSortData();

            DataView dvDueInvoice = new DataView(dt);
            dvDueInvoice.Sort = sortExpression + direction;

            BindDueGridDatatable(dvDueInvoice.ToTable());
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    private DataTable PageSortData()
    {
        DataTable dt = new DataTable();
        try
        {
            if (ViewState["Overdue"] != null)
            {
                dt = (DataTable)ViewState["Overdue"];
            }
            else
            {
                objContract.ConnConfig = Session["config"].ToString();

                DataSet ds = new DataSet();
                ds = objBL_Contracts.GetCollectionInvoices(objContract);
                ViewState["Overdue"] = ds.Tables[0];
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
        return dt;
    }
    private void BindDueGridDatatable(DataTable dt)
    {
        try
        {
            ViewState["Overdue"] = dt;
            gvOverDueInvoice.DataSource = dt;
            gvOverDueInvoice.DataBind();
            SetDueInvoiceTotal();

        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    public SortDirection GvSortDirection
    {
        get
        {
            if (ViewState["sortDirection"] == null)
                ViewState["sortDirection"] = SortDirection.Ascending;

            return (SortDirection)ViewState["sortDirection"];
        }
        set { ViewState["sortDirection"] = value; }
    }
    protected void PaginateDueInvoice(object sender, CommandEventArgs e)
    {
        // get the current page selected
        int intCurIndex = gvOverDueInvoice.PageIndex;

        switch (e.CommandArgument.ToString().ToLower())
        {
            case "first":
                gvOverDueInvoice.PageIndex = 0;
                break;
            case "prev":
                gvOverDueInvoice.PageIndex = intCurIndex - 1;
                break;
            case "next":
                gvOverDueInvoice.PageIndex = intCurIndex + 1;
                break;
            case "last":
                gvOverDueInvoice.PageIndex = gvOverDueInvoice.PageCount;
                break;
        }

        // popultate the gridview control
        FillGridPagedDue();
    }
    private void FillGridPagedDue()
    {
        DataTable dt = new DataTable();

        dt = PageSortData();

        BindDueGridDatatable(dt);
    }

    #region Customer Statement Mail All
    private void GetCustomerStatement()
    {
        try
        {
            DataSet dsC = new DataSet();
            objPropUser.ConnConfig = Session["config"].ToString();
            dsC = objBL_User.getControl(objPropUser);
            ViewState["Company"] = dsC.Tables[0];
            
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();

            objContract.ConnConfig = Session["config"].ToString();
            objContract.strOwners = GetCheckedOwner();
            objContract.IsOverDue = true;    //for overdue tab
            if (!string.IsNullOrEmpty(objContract.strOwners))
            {
                ds = objBL_Contracts.GetCustomerStatement(objContract, true, true);
                dt = ds.Tables[0];
            }
            
            ViewState["CustomerStatementResult"] = dt;                      //contains customer statement result dataset 
            
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    //private string GetFromEmailAddress()
    //{
    //    string fromEmail = "";
    //    objPropUser.ConnConfig = Session["config"].ToString();
    //    objPropUser.Username = Session["username"].ToString();
    //    try
    //    {
    //        fromEmail = objBL_User.getUserEmail(objPropUser);

    //        if (fromEmail == string.Empty)
    //        {
    //            System.Configuration.Configuration configurationFile = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration(Request.ApplicationPath);
    //            MailSettingsSectionGroup mailSettings = configurationFile.GetSectionGroup("system.net/mailSettings") as MailSettingsSectionGroup;
    //            string username = mailSettings.Smtp.Network.UserName;
    //            fromEmail = username;
    //            ////txtFrom.ReadOnly = true;
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
    //        ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
    //    }
    //    return fromEmail;
    //}
    private void ItemDetailsSubReportProcessing(object sender, SubreportProcessingEventArgs e)
    {
        //throw new NotImplementedException();
        try
        {
            DataTable dt = (DataTable)ViewState["CustomerStatementSub"];
            int loc = Convert.ToInt32(dt.Rows[count]["Loc"]);

            objContract.Loc = loc;
            objContract.IsOverDue = true;       // for overdue tab
            objContract.ConnConfig = Session["config"].ToString();
            DataSet ds = objBL_Contracts.GetCustomerStatementInvoices(objContract, true);

            if (dt.Rows.Count > 0)
            {
                ReportDataSource rdsItems = new ReportDataSource("dsInvoiceItem", ds.Tables[0]);

                e.DataSources.Add(rdsItems);
            }
            if (count == dt.Rows.Count - 1)
            {
                ViewState["CustomerStatementSub"] = null;
            }
            count++;
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    private byte[] ExportReportToPDF1(string reportName, ReportViewer reportviewer1)
    {
        Warning[] warnings;
        string[] streamids;
        string mimeType;
        string encoding;
        string filenameExtension;
        reportviewer1.ProcessingMode = ProcessingMode.Local;
        byte[] bytes = reportviewer1.LocalReport.Render(
            "PDF", null, out mimeType, out encoding, out filenameExtension,
             out streamids, out warnings);

        return bytes;
    }
    private void PrintOnlyCustomerStatement()
    {
        try
        {
            string filename = "PrintOnly_CustomerStatement";
            DataTable dtFilter = new DataTable();
            objContract.ConnConfig = Session["config"].ToString();
            objContract.strOwners = GetCheckedOwner();
            objContract.IsOverDue = true;    //for overdue tab
            DataSet ds = objBL_Contracts.GetCustomerStatement(objContract, true, true);
            DataTable dt = ds.Tables[0];
            var rows = dt.AsEnumerable()
                .Where(x => x.Field<int>("IsExistsEmail").Equals(0));
            if (rows.Any())
                dtFilter = rows.CopyToDataTable();

            count = 0;
            if (dtFilter != null)
            {
                if (dtFilter.Rows.Count > 0)
                {
                    //ViewState["FilterInvoice"] = dtFilter;
                    ReportViewer rvCs = new ReportViewer();

                    #region Generate Report
                    ViewState["CustomerStatementSub"] = dtFilter;   // Dataset for subreport
                    DataSet dsC = new DataSet();
                    objPropUser.ConnConfig = Session["config"].ToString();
                    dsC = objBL_User.getControl(objPropUser);
                    ViewState["Company"] = dsC.Tables[0];

                    rvCs.LocalReport.DataSources.Clear();

                    rvCs.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(ItemDetailsSubReportProcessing);
                    rvCs.LocalReport.DataSources.Add(new ReportDataSource("dsInvoice", dtFilter));
                    rvCs.LocalReport.DataSources.Add(new ReportDataSource("dsCompany", dsC.Tables[0]));

                    string reportPath = "Reports/CustomerStatement.rdlc";

                    string Report = string.Empty;
                    if (!string.IsNullOrEmpty(Report.Trim()))
                    {
                        reportPath = "Reports/" + Report.Trim();
                    }
                    rvCs.LocalReport.ReportPath = reportPath;
                    rvCs.LocalReport.EnableExternalImages = true;
                    List<Microsoft.Reporting.WebForms.ReportParameter> param1 = new List<Microsoft.Reporting.WebForms.ReportParameter>();
                    string strPath = "file:///" + Server.MapPath(Request.ApplicationPath).Replace("\\", "/");
                    param1.Add(new Microsoft.Reporting.WebForms.ReportParameter("path", strPath + "/images/Company_logo.jpg"));

                    rvCs.LocalReport.SetParameters(param1);

                    rvCs.LocalReport.Refresh();
                    #endregion

                    byte[] getPDF = ExportReportToPDF1("", rvCs);

                    Response.ClearContent();
                    Response.ClearHeaders();
                    Response.AddHeader("Content-Disposition", "attachment;filename=" + filename + ".pdf");
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("Content-Length", (getPDF.Length).ToString());
                    Response.BinaryWrite(getPDF);
                    Response.Flush();
                    Response.Close();
                    
                    //ScriptManager.RegisterStartupScript(this, GetType(), "myFunction", "hideModelCS();", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "myFunction", "noty({text: 'No customer statement found to print.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});hideModelCS();", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "myFunction", "noty({text: 'No customer statement found to print.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});hideModelCS();", true);
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    #endregion

    #region Invoice Mail All
    private void GetOutstandingInvoice()
    {
        try
        {
            DataSet dsC = new DataSet();
            objPropUser.ConnConfig = Session["config"].ToString();
            dsC = objBL_User.getControl(objPropUser);
            ViewState["Company"] = dsC.Tables[0];

            objContract.ConnConfig = Session["config"].ToString();
            objContract.strOwners = GetCheckedOwner();
            objContract.IsOverDue = true;    //for overdue tab
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            if (!string.IsNullOrEmpty(objContract.strOwners))
            {
                ds = objBL_Contracts.GetOutstandingInvoice(objContract);
                dt = ds.Tables[0];
            }

            ViewState["DueInvoices"] = dt;
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    private string GetCheckedOwner()
    {
        string strOwner = string.Empty;
        int ncount = 0;
        foreach (GridViewRow gr in gvOverDueInvoice.Rows)
        {
            CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");
            Label lblOwner = (Label)gr.FindControl("lblOwner");
            if(chkSelect.Checked == true)
            {
                Label lblTransId = (Label)gr.FindControl("lblTransID");
                if(lblTransId.Text == "0")
                {
                    if (ncount == 0)
                    {
                        strOwner += lblOwner.Text.ToString();
                    }
                    else
                    {
                        strOwner += "," + lblOwner.Text;
                    }
                    ncount++;
                }
            }
        }
        return strOwner;
    }
    private void GenerateInvoiceReport(ReportViewer rv, DataTable dtInvoice)
    {
        try
        {
            DataTable dtCompany = new DataTable();
            if (ViewState["Company"] == null)
            {
                DataSet dsCompany = new DataSet();
                objPropUser.ConnConfig = Session["config"].ToString();
                dsCompany = objBL_User.getControl(objPropUser);
                ViewState["Company"] = dsCompany.Tables[0];
                dtCompany = dsCompany.Tables[0];
            }
            else
            {
                dtCompany = (DataTable)ViewState["Company"];
            }

            foreach (DataRow dr in dtInvoice.Rows)
            {
                //billTo = Regex.Replace(billTo, @"( |\r?\n)\1+", "$1");  // to remove first new line.
                string billTo = Regex.Replace(dr["Billto"].ToString(), @"\t|\n|\r", "");          // to remove all new lines.
                billTo = Regex.Replace(billTo, @"^,+|,+$|,+(,\w)", "$1");
                billTo = billTo.Split(new[] { ',' }, 2).First() + ",\n" + billTo.Split(new[] { ',' }, 2).Last();
                dr["Billto"] = billTo;
            }

            rv.LocalReport.DataSources.Clear();

            rv.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(ItemInvoiceSubReportProcessing);

            string sessval = (string)Session["InvoiceName"];
            string Report = string.Empty;

            if (sessval == "Invoice")
            {
                Report = System.Web.Configuration.WebConfigurationManager.AppSettings["InvoicesReport"].Trim();
            }

            if (sessval == "InvoiceMaint")
            {
                Report = System.Web.Configuration.WebConfigurationManager.AppSettings["InvoicesMaintReport"].Trim();
            }

            if (sessval == "InvoiceException")
            {
                Report = System.Web.Configuration.WebConfigurationManager.AppSettings["InvoicesExceptionReport"].Trim();
            }


            if (Report == "Madden_Invoices.rdlc" || Report == string.Empty || Report == "InvoicesInFrench.rdlc")
            {
                if (!string.IsNullOrEmpty(Report.Trim()))
                {
                    rv.LocalReport.DataSources.Add(new ReportDataSource("Invoice_dtInvoice", dtInvoice));
                }
                else
                {
                    rv.LocalReport.DataSources.Add(new ReportDataSource("Invoice_dtInvoice", dtInvoice));
                }
            }
            else if (Report == "PESMTC_InvoicesMaint.rdlc" || Report == "PESMTC_InvoicesExceptions.rdlc")
            {
                if (!string.IsNullOrEmpty(Report.Trim()))
                {
                    rv.LocalReport.DataSources.Add(new ReportDataSource("Invoice_PESdtInvoice", dtInvoice));
                }
            }

            rv.LocalReport.DataSources.Add(new ReportDataSource("Ticket_Company", dtCompany));

            string reportPath = string.Empty;

            if (sessval == "Invoice")
            {
                reportPath = "Reports/Invoices.rdlc";

                if (Report == "Madden_Invoices.rdlc" || Report == "InvoicesInFrench.rdlc")
                {
                    if (!string.IsNullOrEmpty(Report.Trim()))
                    {
                        reportPath = "Reports/" + Report.Trim();
                    }
                }
            }
            else if (sessval == "InvoiceMaint")
            {
                if (Report == "PESMTC_InvoicesMaint.rdlc")
                {
                    if (!string.IsNullOrEmpty(Report.Trim()))
                    {
                        reportPath = "Reports/" + Report.Trim();
                    }
                }
            }
            else if (sessval == "InvoiceException")
            {
                if (Report == "PESMTC_InvoicesExceptions.rdlc")
                {
                    if (!string.IsNullOrEmpty(Report.Trim()))
                    {
                        reportPath = "Reports/" + Report.Trim();
                    }
                }
            }
            if(reportPath == "")
            {
                reportPath = "Reports/Invoices.rdlc";
            }
            rv.LocalReport.ReportPath = reportPath;

            rv.LocalReport.EnableExternalImages = true;
            List<Microsoft.Reporting.WebForms.ReportParameter> param1 = new List<Microsoft.Reporting.WebForms.ReportParameter>();
            string strPath = "file:///" + Server.MapPath(Request.ApplicationPath).Replace("\\", "/");
            param1.Add(new Microsoft.Reporting.WebForms.ReportParameter("Path", strPath + "/images/Company_logo.jpg"));
            if (Report == "InvoicesInFrench.rdlc" || Report == "")
            {
                param1.Add(new ReportParameter("IsGstTax", ViewState["IsGst"].ToString()));
            }
            rv.LocalReport.SetParameters(param1);

            rv.LocalReport.Refresh();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    private void ItemInvoiceSubReportProcessing(object sender, SubreportProcessingEventArgs e)
    {
        try
        {
            DataTable dt = (DataTable)ViewState["InvoicesSubReportResult"];
            DataTable dtItems = new DataTable();

            objContract.InvoiceID = Convert.ToInt32(dt.Rows[count]["Ref"]);
            objContract.ConnConfig = Session["config"].ToString();
            DataSet ds = objBL_Contracts.GetInvoiceItemByRef(objContract);
            if (ds.Tables[0].Rows.Count < 1)
            {
                dtItems = LoadInvoiceDetails(ds.Tables[0], objContract.InvoiceID);    // if none line item exists of invoice
            }
            else
                dtItems = ds.Tables[0];

            ReportDataSource rdsItems = null;
            if (dtItems.Rows.Count > 0)
            {
                //string Report = System.Web.Configuration.WebConfigurationManager.AppSettings["InvoicesReport"].Trim();
                string sessval = (string)Session["InvoiceName"];
                string Report = string.Empty;

                if (sessval == "Invoice")
                {
                    Report = System.Web.Configuration.WebConfigurationManager.AppSettings["InvoicesReport"].Trim();
                }
                if (sessval == "InvoiceMaint")
                {
                    Report = System.Web.Configuration.WebConfigurationManager.AppSettings["InvoicesMaintReport"].Trim();
                }

                if (sessval == "InvoiceException")
                {
                    Report = System.Web.Configuration.WebConfigurationManager.AppSettings["InvoicesExceptionReport"].Trim();
                }


                if (sessval == "Invoice")
                {
                    if (Report == "Madden_Invoices.rdlc" || Report == string.Empty || Report == "InvoicesInFrench.rdlc")
                    {
                        if (!string.IsNullOrEmpty(Report.Trim()))
                        {
                            rdsItems = new ReportDataSource("dtInvoiceItems", dtItems);
                        }
                        else
                        {
                            rdsItems = new ReportDataSource("dtInvoiceItems", dtItems);
                        }
                    }
                }
                else if (sessval == "InvoiceMaint")
                {
                    if (Report == "PESMTC_InvoicesMaint.rdlc")
                    {
                        if (!string.IsNullOrEmpty(Report.Trim()))
                        {
                            rdsItems = new ReportDataSource("dtPESInvoiceItems", dtItems);
                        }
                    }
                }
                else if (sessval == "InvoiceException")
                {
                    if (Report == "PESMTC_InvoicesExceptions.rdlc")
                    {
                        if (!string.IsNullOrEmpty(Report.Trim()))
                        {
                            rdsItems = new ReportDataSource("dtPESInvoiceItems", dtItems);
                        }
                    }

                }
                e.DataSources.Add(rdsItems);
            }
            if (count == dt.Rows.Count - 1)
            {
                ViewState["InvoicesSubReportResult"] = null;
            }
            count++;
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    private DataTable LoadInvoiceDetails(DataTable dt, int idRef)
    {
        DataRow dr = dt.NewRow();

        try
        {
            dr["Ref"] = idRef;
            dr["Acct"] = 0;
            dr["Quan"] = 0;
            dr["fDesc"] = string.Empty;
            dr["Price"] = 0.00;
            dr["Amount"] = 0.00;
            dr["STax"] = 0.00;
            dr["billcode"] = string.Empty;
            dr["staxAmt"] = 0.00;
            dr["balance"] = 0.00;
            dr["amtpaid"] = 0.00;
            dr["total"] = 0.00;
            dt.Rows.Add(dr);
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }

        return dt;
    }
    private void PrintOnlyInvoice()
    {
        try
        {
            string filename = "PrintOnly_Invoices";

            DataTable dtFilter = new DataTable();
            objContract.ConnConfig = Session["config"].ToString();
            objContract.strOwners = GetCheckedOwner();
            DataSet ds = objBL_Contracts.GetOutstandingInvoice(objContract);

            ViewState["DueInvoices"] = ds.Tables[0];

            DataTable dt = ds.Tables[0];
            var rows = dt.AsEnumerable()
                .Where(x => x.Field<int>("IsExistsEmail").Equals(0));
            if (rows.Any())
                dtFilter = rows.CopyToDataTable();

            count = 0;
            if (dtFilter != null)
            {
                if (dtFilter.Rows.Count > 0)
                {
                    //ViewState["FilterInvoice"] = dtFilter;
                    ReportViewer rvInvoices = new ReportViewer();

                    #region Generate Report
                    ViewState["InvoicesSubReportResult"] = dtFilter;   // Dataset for subreport
                    DataSet dsC = new DataSet();
                    objPropUser.ConnConfig = Session["config"].ToString();
                    dsC = objBL_User.getControl(objPropUser);
                    ViewState["Company"] = dsC.Tables[0];

                    GenerateInvoiceReport(rvInvoices, dtFilter);

                    #region comments
                    //rvInvoices.LocalReport.DataSources.Clear();

                    //rvInvoices.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(ItemInvoiceSubReportProcessing);
                    //rvInvoices.LocalReport.DataSources.Add(new ReportDataSource("dsInvoice", dtFilter));
                    //rvInvoices.LocalReport.DataSources.Add(new ReportDataSource("dsCompany", dsC.Tables[0]));

                    //string reportPath = "Reports/Invoice.rdlc";

                    //string Report = string.Empty;
                    //if (!string.IsNullOrEmpty(Report.Trim()))
                    //{
                    //    reportPath = "Reports/" + Report.Trim();
                    //}
                    //rvInvoices.LocalReport.ReportPath = reportPath;
                    //rvInvoices.LocalReport.EnableExternalImages = true;
                    //List<Microsoft.Reporting.WebForms.ReportParameter> param1 = new List<Microsoft.Reporting.WebForms.ReportParameter>();
                    //string strPath = "file:///" + Server.MapPath(Request.ApplicationPath).Replace("\\", "/");
                    //param1.Add(new Microsoft.Reporting.WebForms.ReportParameter("path", strPath + "/images/Company_logo.jpg"));

                    //rvInvoices.LocalReport.SetParameters(param1);

                    //rvInvoices.LocalReport.Refresh();
                    #endregion

                    #endregion

                    byte[] getPDF = ExportReportToPDF1("", rvInvoices);

                    Response.ClearContent();
                    Response.ClearHeaders();
                    Response.AddHeader("Content-Disposition", "attachment;filename=" + filename + ".pdf");
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("Content-Length", (getPDF.Length).ToString());
                    Response.BinaryWrite(getPDF);
                    Response.Flush();
                    Response.Close();
                    ScriptManager.RegisterStartupScript(this, GetType(), "myFunction", "hideModelInvoice();", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "myFunction", "noty({text: 'No invoice(s) found to print.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});hideModelInvoice();", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "myFunction", "noty({text: 'No invoice(s) found to print.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});hideModelInvoice();", true);
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    #endregion

    private void ShowGstRate()
    {
        try
        {
            // For canadian company show GST rate in Invoice template.
            objGeneral.ConnConfig = Session["config"].ToString();
            objGeneral.CustomName = "Country";
            DataSet dsCustom = objBL_General.getCustomFields(objGeneral);

            if (dsCustom.Tables[0].Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(dsCustom.Tables[0].Rows[0]["Label"].ToString()) && dsCustom.Tables[0].Rows[0]["Label"].ToString().Equals("1"))
                {
                    IsGst = true;
                }
            }
            ViewState["IsGst"] = IsGst;
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    private void MailInvoices()
    {
        try
        {
            GetOutstandingInvoice();    //get invoice report result
            DataTable dtDue = (DataTable)ViewState["DueInvoices"];
            if(dtDue != null)
            {
                if (dtDue.Rows.Count > 0)
                {
                    string _todayDate = DateTime.Now.Date.ToString("MM-dd-yyyy");
                    
                    DataTable dtC = (DataTable)ViewState["Company"];
                    string strCompany = dtC.Rows[0]["name"].ToString() + Environment.NewLine;
                    strCompany += dtC.Rows[0]["Address"].ToString() + Environment.NewLine;
                    strCompany += dtC.Rows[0]["city"].ToString() + ", " + dtC.Rows[0]["state"].ToString() + ", " + dtC.Rows[0]["zip"].ToString() + Environment.NewLine;
                    strCompany += "Phone: " + dtC.Rows[0]["Phone"].ToString() + Environment.NewLine;
                    strCompany += "Fax: " + dtC.Rows[0]["fax"].ToString() + Environment.NewLine;
                    strCompany += "Email: " + dtC.Rows[0]["email"].ToString() + Environment.NewLine;
                    strCompany = "Please review the attached invoice from: " + Environment.NewLine + Environment.NewLine + strCompany;

                    DataTable dt = dtDue.AsEnumerable()                 // Group by location to send invoices
                          .GroupBy(r => r.Field<int>("Loc"))
                          .Select(g => g.First())
                          .CopyToDataTable();

                    //string fromEmail = GetFromEmailAddress();
                    string fromEmail = WebBaseUtility.GetFromEmailAddress();

                    List<string> lstLoc = new List<string>();

                    //string strLoc;
                    //bool isUnscuss = false;

                    DataTable dtFilter = new DataTable();
                    var rows = dtDue.AsEnumerable()
                                  .Where(x => x.Field<int>("IsExistsEmail").Equals(1));
                    if (rows.Any())
                        dtFilter = rows.CopyToDataTable();

                    var groupLoc = (
                                    from DataRow row in dtFilter.AsEnumerable()
                                    select new
                                    {
                                        loc = row.Field<Int32>("Loc")
                                    }
                                  ).Distinct().AsEnumerable();

                    foreach (var g in groupLoc)
                    {
                        count = 0;
                        string toEmail = "";
                        string ccEmail = "";

                        #region GenerateInvoice

                        int loc = Convert.ToInt32(g.loc);
                        DataTable dtLoc = dtFilter
                                            .Select("Loc = " + loc)
                                            .CopyToDataTable();

                        ViewState["InvoicesSubReportResult"] = dtLoc;   // Dataset for subreport
                        ReportViewer rvInvoices = new ReportViewer();
                        GenerateInvoiceReport(rvInvoices, dtLoc);

                        #endregion

                        #region Email

                        toEmail = dtLoc.Rows[0]["custom12"].ToString();

                        if (!string.IsNullOrEmpty(dtLoc.Rows[0]["custom13"].ToString()))
                        {
                            ccEmail = dtLoc.Rows[0]["custom13"].ToString();
                        }

                        List<string> toEmaillst = new List<string>();
                        toEmaillst.Add(toEmail);
                        List<string> ccEmaillst = new List<string>();
                        ccEmaillst.Add(ccEmail);

                        Mail mail = new Mail();
                        mail.From = fromEmail;
                        mail.To = toEmaillst;
                        mail.Cc = ccEmaillst;

                        mail.Title = "Invoices - " + dtLoc.Rows[0]["ID"].ToString() + " " + dtLoc.Rows[0]["locname"].ToString();

                        //mail.Text = ViewState["CompanyAddress"].ToString().Replace(Environment.NewLine, "<BR/>");

                        mail.Text = strCompany.Replace(Environment.NewLine, "<BR/>");
                        mail.attachmentBytes = ExportReportToPDF1("", rvInvoices);
                        mail.FileName = "Invoices_" + _todayDate + ".pdf";

                        mail.DeleteFilesAfterSend = true;
                        mail.RequireAutentication = false;
                        // ES-33
                        WebBaseUtility.UpdateMailConfigurationFromLoginUser(mail);
                        mail.Send();

                        #endregion
                    }

                    #region Check Loc Email Not Available

                    DataTable dtEMail = new DataTable();
                    var rows1 = dtDue.AsEnumerable()
                            .Where(x => x.Field<int>("IsExistsEmail").Equals(0));
                    if (rows1.Any())
                        dtEMail = rows1.CopyToDataTable();

                    string funcString = string.Empty;
                    if (dtEMail.Rows.Count > 0)
                    {
                        funcString = "displayWarnInvoice();";
                    }

                    #endregion

                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "noty({text: 'Email sent successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});" + funcString, true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "noty({text: 'No Invoices found to email!',  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "noty({text: 'No Invoices found to email!',  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
            }
            
        }
        catch (Exception ex)
        {
            //string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(ex.Message);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    #endregion
    
}