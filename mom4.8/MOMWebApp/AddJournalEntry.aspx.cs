using BusinessEntity;
using BusinessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using System.Web.Script.Serialization;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;
using System.IO;
using System.Data.OleDb;
using System.Text;
using System.Reflection;
using System.Web;

public partial class AddJournalEntry : System.Web.UI.Page
{

    #region Variables
    Chart _objChart = new Chart();
    BL_Chart _objBLChart = new BL_Chart();

    Journal _objJournal = new Journal();
    Transaction _objTrans = new Transaction();
    BL_JournalEntry _objBLJournal = new BL_JournalEntry();

    AccountType _objAcType = new AccountType();
    Central _objCentral = new Central();
    BL_AccountType _objBLAcType = new BL_AccountType();

    Bank _objBank = new Bank();
    Rol _objRol = new Rol();
    BL_BankAccount _objBLBank = new BL_BankAccount();

    BL_GLARecur _objBLGLARecur = new BL_GLARecur();

    User _objPropUser = new User();
    BL_User _objBLUser = new BL_User();

    CompanyOffice objCompany = new CompanyOffice();
    BL_Company _objBLCompany = new BL_Company();

    MapData objMapData = new MapData();
    BL_MapData objBL_MapData = new BL_MapData();
    JobT objJob = new JobT();
    BL_Job objBL_Job = new BL_Job();
    #endregion

    #region Events

    #region PAGELOAD

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            divNavigate.Style["display"] = "None";

            if (Session["userid"] == null)
            {
                Response.Redirect("login.aspx");
            }

            WebBaseUtility.UpdatePageTitle(this, "Journal Entry", Request.QueryString["id"], Request.QueryString["t"]);

            if (Request.QueryString["id"] != null)
            {
                divNavigate.Style["display"] = "block";
                btnReverse.Visible = true;
                btnExportExcel.Visible = true;
            }

            if (!IsPostBack)
            {
                userpermissions();

                FillFrequency();
                FillAccountType();
                FillStatus();
                FillSubAccount();
                FillCentral();
                FillState();

                btnSaveNew.Visible = true;
                imgCleared.Visible = false;

                txtProof.Text = "0";
                txtProof.Attributes.Add("readonly", "readonly");

                RadGrid_Journal.Columns[7].Display = false;
                RadGrid_Journal.Columns[8].Display = false;
                RadGrid_Journal.Columns[9].Display = false;

                if (Request.QueryString["c"] != null)
                {
                    if (Request.QueryString["id"] != null)
                    {
                        if (Request.QueryString["c"] == "1")
                        {
                            //Copy JE
                            lblHeader.Text = "Copy Journal Entry";
                            ToggleRecurring(false);
                            hdnIsRecurr.Value = "false";

                            //Update Journal entry
                            SetForUpdate(Convert.ToInt32(Request.QueryString["id"]));
                            chkIsRecurr.Attributes.Add("onclick", "return false;");
                            GetPeriodDetails(Convert.ToDateTime(txtTransDate.Text));

                            _objJournal.ConnConfig = Session["config"].ToString();
                            _objJournal.MaxTransID = _objBLJournal.GetMaxTransID(_objJournal);

                            txtEntryNo.Text = _objJournal.MaxTransID.ToString();
                            lblEntryNo.Text = string.Empty;
                        }
                        else if (Request.QueryString["c"] == "2")
                        {
                            //Copy RE
                            lblHeader.Text = "Copy Recurring Entry";
                            ToggleRecurring(true);
                            hdnIsRecurr.Value = "true";

                            lblEntryNo.Visible = true;
                            SetForUpdate(Convert.ToInt32(Request.QueryString["id"]), true);
                            chkIsRecurr.Checked = true;
                            chkIsRecurr.Attributes.Add("onclick", "return false;");

                            _objJournal.ConnConfig = Session["config"].ToString();
                            _objJournal.MaxTransID = _objBLJournal.GetMaxTransID(_objJournal);

                            txtEntryNo1.Text = _objJournal.MaxTransID.ToString();
                        }

                        hdnOriginalJE.Value = "0";
                        lnkOriginal.Visible = false;
                    }
                }
                else if (Request.QueryString["rid"] != null)
                {
                    if (Request.QueryString["r"] != null && Request.QueryString["r"] == "1")
                    {
                        //Reverse RE
                        lblHeader.Text = "Reverse Recurring Entry";
                        ToggleRecurring(true);
                        hdnIsRecurr.Value = "true";

                        lblEntryNo.Visible = true;
                        SetForReverse(Convert.ToInt32(Request.QueryString["rid"]), true);
                        chkIsRecurr.Checked = true;
                        chkIsRecurr.Attributes.Add("onclick", "return false;");

                        _objJournal.ConnConfig = Session["config"].ToString();
                        _objJournal.MaxTransID = _objBLJournal.GetMaxTransID(_objJournal);

                        txtEntryNo1.Text = _objJournal.MaxTransID.ToString();
                    }
                    else 
                    {
                        //Reverse JE
                        lblHeader.Text = "Reverse Journal Entry";
                        ToggleRecurring(false);
                        hdnIsRecurr.Value = "false";

                        //Update Journal entry
                        SetForReverse(Convert.ToInt32(Request.QueryString["rid"]));
                        chkIsRecurr.Attributes.Add("onclick", "return false;");
                        GetPeriodDetails(Convert.ToDateTime(txtTransDate.Text));

                        _objJournal.ConnConfig = Session["config"].ToString();
                        _objJournal.MaxTransID = _objBLJournal.GetMaxTransID(_objJournal);

                        txtEntryNo.Text = _objJournal.MaxTransID.ToString();
                        lblEntryNo.Text = string.Empty;
                    }
                }
                else if (Request.QueryString["id"] != null)
                {
                    liLogs.Style["display"] = "inline-block";
                    tbLogs.Style["display"] = "block";
                    #region Update

                    List<string> _lstDeletedTrans = new List<string>();
                    ViewState["DeletedTrans"] = _lstDeletedTrans;
                    if (Request.QueryString["r"] != null)
                    {
                        if (Request.QueryString["r"].ToString() == "1")
                        {
                            lblHeader.Text = "Edit Recurring Entry";
                            ToggleRecurring(true);
                            hdnIsRecurr.Value = "true";
                            lblEntryNo.Visible = true;
                            SetForUpdate(Convert.ToInt32(Request.QueryString["id"]), true);
                            chkIsRecurr.Checked = true;
                            chkIsRecurr.Attributes.Add("onclick", "return false;");

                            hdnOriginalJE.Value = "0";
                            lnkOriginal.Visible = false;
                        }
                    }
                    else
                    {
                        lblHeader.Text = "Edit Journal Entry";
                        ToggleRecurring(false);
                        hdnIsRecurr.Value = "false";
                        lblEntryNo.Visible = true;
                        SetForUpdate(Convert.ToInt32(Request.QueryString["id"]));
                        chkIsRecurr.Attributes.Add("onclick", "return false;");

                    }
                    #endregion

                    GetPeriodDetails(Convert.ToDateTime(txtTransDate.Text));
                }
                else
                {
                    // ADD NEW JOURNAL ENTRY
                    SetNewJournalEntry();
                }

                Permission();
                HighlightSideMenu("financeMgr", "lnkJournalEntry", "financeMgrSub");
                CompanyPermission();

                txtAcctDescription.Attributes.Add("maxlength", "75");
            }

            Session["startdate"] = Convert.ToDateTime(Session["startdate"]).ToShortDateString();
            Session["enddate"] = Convert.ToDateTime(Session["enddate"]).ToShortDateString();

            DocumentPermission();
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
        aNav.CssClass = "active collapsible-header waves-effect waves-cyan collapsible-height-nl";

        HyperLink lnkUsersSmenu = (HyperLink)Page.Master.FindControl(PageLink);
        lnkUsersSmenu.Style.Add("color", "#316b9d");
        lnkUsersSmenu.Style.Add("font-weight", "normal");
        lnkUsersSmenu.Style.Add("background-color", "#e5e5e5");
        HtmlGenericControl div = (HtmlGenericControl)Page.Master.FindControl(SubMenuDiv);
        div.Style.Add("display", "block");
    }

    private void ToggleRecurring(bool isRecurring)
    {
        if (!isRecurring)
        {
            lblFrequency.Visible = false;
            ddlFrequency.Visible = false;
            lblEntryNo1.Visible = false;
            txtEntryNo1.Visible = false;
            lblEntry.Visible = true;
            txtEntryNo.Visible = true;
            rfvEntryNo.Enabled = true;
            rfvEntryNo1.Enabled = false;
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "Init", "UpdateTextbox();", true);
        }
        else
        {
            lblFrequency.Visible = true;
            ddlFrequency.Visible = true;
            lblEntryNo1.Visible = true;
            txtEntryNo1.Visible = true;
            lblEntry.Visible = false;
            txtEntryNo.Visible = false;
            rfvEntryNo.Enabled = false;
            rfvEntryNo1.Enabled = true;
        }
    }

    private void GetPeriodDetails(DateTime _transDate, bool isAddNew = false)
    {
        bool _flag = CommonHelper.GetPeriodDetails(_transDate);
        ViewState["FlagPeriodClose"] = _flag;

        if (!_flag && !isAddNew)
        {
            btnSaveNew.Visible = false;
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keywarning", "noty({text: 'These month/year period is closed out. You do not have permission to add/update this record.',  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : 10000,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    private void Permission()
    {
        if (Session["type"].ToString() == "c")
        {
            Response.Redirect("home.aspx");
        }

        if (Session["MSM"].ToString() == "TS")
        {
            Response.Redirect("home.aspx");
        }

        if (Convert.ToInt32(Session["ISsupervisor"]) == 1)
        {
            Response.Redirect("home.aspx");
        }
    }

    private void CompanyPermission()
    {
        if (Session["COPer"].ToString() == "1")
        {
            RadGrid_Journal.Columns[3].Visible = true;
            dvCompanyPermission.Visible = true;
            FillCompany();
        }
        else
        {
            RadGrid_Journal.Columns[3].Visible = false;
            dvCompanyPermission.Visible = false;
        }
    }
    #endregion

    protected void RadGrid_Journal_PreRender(object sender, EventArgs e)
    {
        foreach (GridDataItem gr in RadGrid_Journal.Items)
        {
            TextBox txtGvAcctNo = (TextBox)gr.FindControl("txtGvAcctNo");

            if (txtGvAcctNo != null)
            {
                gr.Attributes["onclick"] = "VisibleRow('" + gr.ClientID + "','" + txtGvAcctNo.ClientID + "','" + RadGrid_Journal.ClientID + "',event);";
            }
        }
        try
        {
            if (RadGrid_Journal.Items.Count > 1)
            {
                int lastRow = RadGrid_Journal.Items.Count;

                GridDataItem gr = (GridDataItem)RadGrid_Journal.Items[lastRow - 1];
                TextBox txtGvAcctNo = (TextBox)gr.FindControl("txtGvAcctNo");

                if (txtGvAcctNo != null)
                {
                    txtGvAcctNo.Focus();
                }
            }
        }
        catch (Exception ex)
        {

        }
    }

    #region Save Data

    protected void btnSaveNew_Click(object sender, EventArgs e)
    {
        try
        {
            var isAddNew = Request.QueryString["id"] == null || (Request.QueryString["id"] != null && Request.QueryString["c"] != null);
            GetPeriodDetails(Convert.ToDateTime(txtTransDate.Text), isAddNew);
            bool flag = (bool)ViewState["FlagPeriodClose"];
            bool flagEntry = (bool)ViewState["FlagCheckEntryNo"];

            if (flag && flagEntry)
            {
                SaveJE();
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void lnkPrint_Click(object sender, EventArgs e)
    {
        try
        {
            if (Request.QueryString["id"] != null && Request.QueryString["c"] == null)
            {
                bool flag = (bool)ViewState["FlagPeriodClose"];
                bool flagEntry = (bool)ViewState["FlagCheckEntryNo"];

                if (flag && flagEntry)
                {
                    SaveJE(true);
                }
                else
                {
                    string reportUrl = "JournalEntryReport.aspx?id=" + Request.QueryString["id"];

                    if (chkIsRecurr.Checked)
                    {
                        reportUrl += "&r=1";
                    }

                    Response.Redirect(reportUrl);
                }
            }
            else
            {
                var isAddNew = Request.QueryString["id"] == null;
                GetPeriodDetails(Convert.ToDateTime(txtTransDate.Text), isAddNew);
                bool flag = (bool)ViewState["FlagPeriodClose"];
                bool flagEntry = (bool)ViewState["FlagCheckEntryNo"];

                if (flag && flagEntry)
                {
                    SaveJE(true);
                }
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    private void SaveJE(bool saveAndPrint = false)
    {
        DataTable dt = GetJEItems();
        dt.Select("JobID = 0")
                       .AsEnumerable().ToList()
                       .ForEach(t => t["JobID"] = null);
        dt.Select("PhaseID = 0")
                       .AsEnumerable().ToList()
                       .ForEach(t => t["PhaseID"] = null);
        //dt.Select("TypeID = 0")
        //               .AsEnumerable().ToList()
        //               .ForEach(t => t["TypeID"] = null);
        dt.AcceptChanges();

        foreach (DataRow rw in dt.Rows)
        {
            if (Convert.ToString(rw["JobID"]) != "0" && Convert.ToString(rw["JobID"]) != null && Convert.ToString(rw["JobID"]) != "")
            {

                ///////// Check Job Closed in JE ///////
                
                    DataSet _dsJobs = new DataSet();
                    objJob.ConnConfig = Session["config"].ToString();
                    objJob.ID = Convert.ToInt32(Convert.ToString(rw["JobID"]));
                    _dsJobs = objBL_Job.spGetJobStatus(objJob);

                    int jobstatus = Convert.ToInt32(_dsJobs.Tables[0].Rows[0]["STATUS"].ToString());
                    if (jobstatus == 1)
                    {
                        
                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningJobStatus", "noty({text: 'Project# " + Convert.ToString(rw["JobID"]).ToString() + "  is closed. Please change the project status before proceeding.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
                        return;
                    }
                
                ///////// Check Job Closed in JE ///////


                if (Convert.ToString(rw["PhaseID"]) == "0" || Convert.ToString(rw["PhaseID"]) == null || Convert.ToString(rw["PhaseID"]) == "")
                {

                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningInvInactive", "noty({text: 'Please enter code.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
                    return;
                }
            }
        }


        _objJournal.ConnConfig = Session["config"].ToString();
        _objJournal.DtTrans = dt;
        _objJournal.fDate = Convert.ToDateTime(txtTransDate.Text);
        _objJournal.fDesc = txtDescription.Text;
        _objJournal.Internal = chkIsRecurr.Checked ? txtEntryNo1.Text : txtEntryNo.Text;
        _objJournal.IsJobSpec = chkJobSpecific.Checked;
        _objJournal.IsRecurring = chkIsRecurr.Checked;
        _objJournal.OriginalJE = Convert.ToInt32(hdnOriginalJE.Value.ToString());
        _objJournal.UserName = Session["UserName"].ToString();
        if (chkIsRecurr.Checked)
        {
            _objJournal.Frequency = Convert.ToInt16(ddlFrequency.SelectedValue);
        }

        if (dt.Rows.Count < 2)
        {
            ShowMesg("You must fill out atleast 2 lines.", 0);
        }
        else
        {
            if (ValidateGridView())
            {
                double proof = dt.AsEnumerable().Sum(x => Convert.ToDouble(x["Debit"])) - dt.AsEnumerable().Sum(x => Convert.ToDouble(x["Credit"]));
                if (Math.Round((Double)proof, 2) == 0)
                {
                    int jeRef = 0;

                    if (Request.QueryString["id"] != null && Request.QueryString["c"] == null)
                    {
                        jeRef = Convert.ToInt32(Request.QueryString["id"]);
                        _objJournal.Ref = jeRef;

                        if (!string.IsNullOrEmpty(hdnBatchID.Value))
                        {
                            _objJournal.BatchID = Convert.ToInt32(hdnBatchID.Value);
                        }

                        _objBLJournal.UpdateJE(_objJournal);
                        ShowMesg("JE Updated Successfully! <BR/> Ref # " + _objJournal.Ref.ToString() + "", 1);

                        UpdateDocInfo();

                        RadGrid_gvLogs.Rebind();
                    }
                    else
                    {
                        jeRef = _objBLJournal.AddJE(_objJournal);
                        _objJournal.Ref = jeRef;

                        //Update  Attachment Doc INFO                 
                        UpdateTempDateWhenCreatingNewJE(jeRef.ToString());
                        UpdateDocInfo();

                        if (Request.QueryString["c"] == null)
                        {
                            ResetFormControlValues(this);
                            SetNewJournalEntry();
                            ShowMesg("JE Added Successfully! <BR/> Ref # " + jeRef + "", 1);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(),
                            "alert",
                            "alert('JE Added Successfully! Ref # " + jeRef + "');window.location ='addjournalentry.aspx';",
                            true);
                        }

                        RadGrid_Journal.Columns[7].Display = false;
                        RadGrid_Journal.Columns[8].Display = false;
                        RadGrid_Journal.Columns[9].Display = false;
                        RadGrid_gvLogs.Rebind();
                    }

                    if (saveAndPrint && jeRef > 0)
                    {
                        string reportUrl = "JournalEntryReport.aspx?id=" + jeRef;

                        if (_objJournal.IsRecurring)
                        {
                            reportUrl += "&r=1";
                        }

                        Response.Redirect(reportUrl);
                    }
                }
                else
                {
                    ShowMesg("Proof must be zero.", 0);
                }
            }
        }
    }

    protected void cvfDate_ServerValidate(object source, ServerValidateEventArgs args)
    {
        try
        {
            bool periodOut = true;

            if (!string.IsNullOrEmpty(txtTransDate.Text))
            {
                var transDate = Convert.ToDateTime(txtTransDate.Text);
                periodOut = CommonHelper.GetPeriodDetails(transDate);
            }

            args.IsValid = periodOut;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void cvEntryNo_ServerValidate(object source, ServerValidateEventArgs args)
    {
        try
        {
            bool periodOut = true;

            if (!string.IsNullOrEmpty(txtEntryNo.Text))
            {
                _objJournal.ConnConfig = Session["config"].ToString();
                _objJournal.IsRecurring = false;
                _objJournal.Internal = txtEntryNo.Text;

                var ds = _objBLJournal.GetJournalEntryByEntryNo(_objJournal);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (Request.QueryString["id"] != null && Request.QueryString["c"] == null)
                    {
                        if (ds.Tables[0].Rows[0]["Ref"].ToString() != Request.QueryString["id"])
                        {
                            periodOut = false;
                        }
                    }
                    else
                    {
                        periodOut = false;
                    }
                }
            }

            ViewState["FlagCheckEntryNo"] = periodOut;
            args.IsValid = periodOut;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void cvEntryNo1_ServerValidate(object source, ServerValidateEventArgs args)
    {
        try
        {
            bool periodOut = true;

            if (!string.IsNullOrEmpty(txtEntryNo1.Text))
            {
                _objJournal.ConnConfig = Session["config"].ToString();
                _objJournal.IsRecurring = true;
                _objJournal.Internal = txtEntryNo1.Text;

                var ds = _objBLJournal.GetJournalEntryByEntryNo(_objJournal);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (Request.QueryString["id"] != null && Request.QueryString["c"] == null)
                    {
                        if (ds.Tables[0].Rows[0]["Ref"].ToString() != Request.QueryString["id"])
                        {
                            periodOut = false;
                        }
                    }
                    else
                    {
                        periodOut = false;
                    }
                }
            }

            ViewState["FlagCheckEntryNo"] = periodOut;
            args.IsValid = periodOut;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    #endregion

    #region Add New lines

    protected void btnReverse_Click(object sender, EventArgs e)
    {
        string urlString = "AddJournalEntry.aspx?rid=" + Request.QueryString["id"];

        if (chkIsRecurr.Checked)
        {
            urlString += "&r=1";
        }

        Response.Redirect(urlString, true);
    }

    protected void btnExportExcel_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = (DataTable)ViewState["Transactions"];
            string totalDebit;
            string totalCredit;

            if (dt.Rows.Count > 0)
            {
                totalDebit = dt.Compute("Sum(Debit)", string.Empty).ToString();
                totalCredit = dt.Compute("Sum(Credit)", string.Empty).ToString();

                Response.ClearContent();
                Response.AddHeader("content-disposition", "attachment;filename=JournalEntry_" + Request.QueryString["id"] + ".xls");
                Response.AddHeader("Content-Type", "application/vnd.ms-excel");
                using (System.IO.StringWriter sw = new System.IO.StringWriter())
                {
                    using (System.Web.UI.HtmlTextWriter htw = new System.Web.UI.HtmlTextWriter(sw))
                    {
                        GridView grid = new GridView();

                        grid.AutoGenerateColumns = false;
                        grid.ShowFooter = true;

                        BoundField fieldAcctNo = new BoundField();
                        fieldAcctNo.HeaderText = "Acct No.";
                        fieldAcctNo.DataField = "AcctNo";
                        fieldAcctNo.FooterText = "Total";
                        fieldAcctNo.ItemStyle.HorizontalAlign = HorizontalAlign.Left;
                        fieldAcctNo.FooterStyle.Font.Bold = true;
                        grid.Columns.Add(fieldAcctNo);

                        BoundField fieldDescription = new BoundField();
                        fieldDescription.HeaderText = "Description";
                        fieldDescription.DataField = "Account";
                        fieldDescription.ItemStyle.HorizontalAlign = HorizontalAlign.Left;
                        grid.Columns.Add(fieldDescription);

                        BoundField fieldMemo = new BoundField();
                        fieldMemo.HeaderText = "Memo";
                        fieldMemo.DataField = "fDesc";
                        fieldMemo.ItemStyle.HorizontalAlign = HorizontalAlign.Left;
                        grid.Columns.Add(fieldMemo);

                        BoundField fieldDebit = new BoundField();
                        fieldDebit.HeaderText = "$ Debit";
                        fieldDebit.DataField = "Debit";
                        fieldDebit.FooterText = totalDebit;
                        fieldDebit.ItemStyle.HorizontalAlign = HorizontalAlign.Right;
                        fieldDebit.FooterStyle.Font.Bold = true;
                        grid.Columns.Add(fieldDebit);

                        BoundField fieldCredit = new BoundField();
                        fieldCredit.HeaderText = "$ Credit";
                        fieldCredit.DataField = "Credit";
                        fieldCredit.FooterText = totalCredit;
                        fieldCredit.ItemStyle.HorizontalAlign = HorizontalAlign.Right;
                        fieldCredit.FooterStyle.Font.Bold = true;
                        grid.Columns.Add(fieldCredit);

                        if (chkJobSpecific.Checked)
                        {
                            BoundField fieldLocation = new BoundField();
                            fieldLocation.HeaderText = "Location Name";
                            fieldLocation.DataField = "Loc";
                            fieldLocation.ItemStyle.HorizontalAlign = HorizontalAlign.Left;
                            grid.Columns.Add(fieldLocation);

                            BoundField fieldProject = new BoundField();
                            fieldProject.HeaderText = "Project";
                            fieldProject.DataField = "JobName";
                            fieldProject.ItemStyle.HorizontalAlign = HorizontalAlign.Left;
                            grid.Columns.Add(fieldProject);

                            BoundField fieldCode = new BoundField();
                            fieldCode.HeaderText = "Code";
                            fieldCode.DataField = "Phase";
                            fieldCode.ItemStyle.HorizontalAlign = HorizontalAlign.Left;
                            grid.Columns.Add(fieldCode);
                        }

                        grid.DataSource = dt;
                        grid.DataBind();
                        grid.RenderControl(htw);
                        Response.Write(sw.ToString());
                    }
                }

                Response.End();
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void lbtnAddNewLines_Click(object sender, EventArgs e)
    {
        try
        {
            if (RadGrid_Journal.Items.Count > 0)
            {
                GridDataItem lastRow = RadGrid_Journal.Items[RadGrid_Journal.Items.Count - 1];
                TextBox txtGvAcctNo = (TextBox)lastRow.FindControl("txtGvAcctNo");

                if (!string.IsNullOrEmpty(txtGvAcctNo.Text))
                {
                    DataTable dt = GetAllJEItems();
                    DataRow dr = dt.NewRow();
                    dt.Rows.Add(dr);

                    RadGrid_Journal.DataSource = dt;
                    RadGrid_Journal.DataBind();
                }
            }
            else
            {
                SetInitialRow();
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    private DataTable AddRowToCal(DataTable dt)
    {
        DataRow drLast = dt.Rows[dt.Rows.Count - 1];

        if (!string.IsNullOrEmpty(drLast["AcctNo"].ToString()))
        {
            var sumDebit = dt.AsEnumerable().Sum(x => Convert.ToDouble(x.Field<string>("Debit")));
            var sumCredit = dt.AsEnumerable().Sum(x => Convert.ToDouble(x.Field<string>("Credit")));
            var roofValue = Math.Round(sumDebit - sumCredit, 2);

            if (roofValue != 0)
            {
                DataRow dr = dt.NewRow();

                if (roofValue > 0)
                {
                    dr["Credit"] = roofValue;
                    dr["Debit"] = 0.00;
                }
                else
                {
                    dr["Debit"] = Math.Abs(roofValue);
                    dr["Credit"] = 0.00;
                }

                dt.Rows.Add(dr);
            }
        }
        else
        {
            var dtTemp = dt.Copy();
            dtTemp.Rows.RemoveAt(dtTemp.Rows.Count - 1);

            var sumDebit = dtTemp.AsEnumerable().Sum(x => Convert.ToDouble(x.Field<string>("Debit")));
            var sumCredit = dtTemp.AsEnumerable().Sum(x => Convert.ToDouble(x.Field<string>("Credit")));
            var roofValue = Math.Round(sumDebit - sumCredit, 2);

            if (roofValue > 0)
            {
                drLast["Credit"] = roofValue;
                drLast["Debit"] = 0.00;
            }
            else
            {
                drLast["Debit"] = Math.Abs(roofValue);
                drLast["Credit"] = 0.00;
            }
        }

        return dt;
    }

    #endregion

    private void ShowMesg(string mesg, Int16 type)
    {
        if (type == 0)            /// Warning message
        {
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keywarning", "noty({text: '" + mesg + "',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
        }
        else if (type == 1)       /// Success message
        {
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keysucc", "noty({text: '" + mesg + "',  type : 'success', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void RadGrid_Journal_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridDataItem)// to access a row 
            {
                GridDataItem item = (GridDataItem)e.Item;
                TextBox txtGvJob = (TextBox)item.FindControl("txtGvJob");
                txtGvJob.ReadOnly = true;

                TextBox txtGvAcctNo = (TextBox)item.FindControl("txtGvAcctNo");
                txtGvAcctNo.ReadOnly = true;
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void RadGrid_Journal_ItemCommand(object source, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == RadGrid.ExportToExcelCommandName)
            {
                RadGrid_Journal.AllowPaging = false;
                RadGrid_Journal.Rebind();
            }

            if (e.CommandName == "DeleteTransaction")
            {
                int indexRow = Convert.ToInt32(e.CommandArgument);
                if (indexRow >= 0)
                {
                    DataTable dt = GetAllJEItems(indexRow);

                    RadGrid_Journal.DataSource = dt;
                    RadGrid_Journal.DataBind();
                }

                ScriptManager.RegisterStartupScript(this, Page.GetType(), "CheckBalanceProof", "BalanceProof();", true);
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
        if (!string.IsNullOrEmpty(Request.QueryString["page"]))
        {
            if (Request.QueryString["page"].ToString() == "bankrecon")
            {
                Response.Redirect(Request.QueryString["page"].ToString() + ".aspx");
            }
            else
            {
                if (!string.IsNullOrEmpty(Request.QueryString["pid"]))
                {
                    Response.Redirect(Request.QueryString["page"].ToString() + ".aspx?uid=" + Request.QueryString["pid"].ToString() + "&tab=budget");
                }
            }

        }
        else
        {
            Response.Redirect("journalentry.aspx");
        }
    }

    protected void chkJobSpecific_CheckedChanged(object sender, EventArgs e)
    {
        if (chkJobSpecific.Checked == true)
        {
            RadGrid_Journal.Columns[7].Display = true;
            RadGrid_Journal.Columns[8].Display = true;
            RadGrid_Journal.Columns[9].Display = true;
        }
        else
        {
            RadGrid_Journal.Columns[7].Display = false;
            RadGrid_Journal.Columns[8].Display = false;
            RadGrid_Journal.Columns[9].Display = false;
        }
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyedit", "InitGridData();UpdateTextbox();", true);
    }

    protected void txtGvCredit_TextChanged(object sender, EventArgs e)
    {
        try
        {
            TextBox txtGvCredit = (TextBox)sender;
            double defaultVal;
            bool isFloat = Double.TryParse(txtGvCredit.Text, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.CurrentCulture, out defaultVal);

            if (isFloat)
            {
                txtGvCredit.Text = Convert.ToDouble(txtGvCredit.Text).ToString("0.00", CultureInfo.InvariantCulture);
                if (!Convert.ToDouble(txtGvCredit.Text).Equals(0))
                {
                    GridDataItem gridrow = (GridDataItem)txtGvCredit.Parent.Parent;
                    int rowIndex = gridrow.ItemIndex + 1;
                    foreach (GridDataItem row in RadGrid_Journal.Items)
                    {
                        if (row.ItemIndex == rowIndex)
                        {
                            TextBox nxtTxtGvCredit = (TextBox)row.FindControl("txtGvCredit");
                            TextBox nxtTxtGvDebit = (TextBox)row.FindControl("txtGvDebit");
                            if (!Convert.ToDouble(txtProof.Text).Equals(0))
                            {
                                if (string.IsNullOrEmpty(nxtTxtGvCredit.Text) && string.IsNullOrEmpty(nxtTxtGvDebit.Text))
                                {
                                    if (hdnIsPositive.Value.Equals("false"))
                                    {
                                        nxtTxtGvCredit.Text = "0.00";
                                        nxtTxtGvDebit.Text = txtProof.Text;
                                    }
                                    else
                                    {
                                        nxtTxtGvCredit.Text = txtProof.Text;
                                        nxtTxtGvDebit.Text = "0.00";
                                    }
                                }
                            }
                        }
                    }
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "BalanceProof", "BalanceProof();", true);
                }
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void txtGvDebit_TextChanged(object sender, EventArgs e)
    {
        try
        {
            TextBox txtGvDebit = (TextBox)sender;
            double defaultVal;
            bool isFloat = Double.TryParse(txtGvDebit.Text, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.CurrentCulture, out defaultVal);

            if (isFloat)
            {
                txtGvDebit.Text = Convert.ToDouble(txtGvDebit.Text).ToString("0.00", CultureInfo.InvariantCulture);
                if (!Convert.ToDouble(txtGvDebit.Text).Equals(0))
                {
                    GridDataItem gridrow = (GridDataItem)txtGvDebit.NamingContainer;
                    int rowIndex = gridrow.ItemIndex + 1;
                    foreach (GridDataItem row in RadGrid_Journal.Items)
                    {
                        if (row.ItemIndex == rowIndex)
                        {
                            TextBox nxtTxtGvCredit = (TextBox)row.FindControl("txtGvCredit");
                            TextBox nxtTxtGvDebit = (TextBox)row.FindControl("txtGvDebit");
                            if (!Convert.ToDouble(txtProof.Text).Equals(0))
                            {
                                if (string.IsNullOrEmpty(nxtTxtGvCredit.Text) && string.IsNullOrEmpty(nxtTxtGvDebit.Text))
                                {
                                    if (hdnIsPositive.Value.Equals("false"))
                                    {
                                        nxtTxtGvCredit.Text = "0.00";
                                        nxtTxtGvDebit.Text = txtProof.Text;
                                    }
                                    else
                                    {
                                        nxtTxtGvCredit.Text = txtProof.Text;
                                        nxtTxtGvDebit.Text = "0.00";
                                    }
                                }
                            }
                        }
                    }
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "BalanceProof", "BalanceProof();", true);
                }
            }
            else
                txtGvDebit.Text = "0.00";
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "Init", "InitGridData();UpdateTextbox();", true);
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "Init", "InitGridData();UpdateTextbox();", true);
        }
    }

    protected void chkIsRecurr_CheckedChanged(object sender, EventArgs e)
    {
        if (chkIsRecurr.Checked.Equals(true))
        {
            ToggleRecurring(true);
            ScriptManager.RegisterStartupScript(this, GetType(), "clearEntry", "clearEntry();", true);
        }
        else
        {
            ToggleRecurring(false);
            ScriptManager.RegisterStartupScript(this, GetType(), "clearEntry", "clearEntry();", true);
        }
    }

    protected void lnkNext_Click(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();
        if (dt != null)
        {
            dt = (DataTable)Session["Journal"];

            DataColumn[] keyColumns = new DataColumn[1];
            keyColumns[0] = dt.Columns["Ref"];
            dt.PrimaryKey = keyColumns;

            DataRow d = dt.Rows.Find(Request.QueryString["id"].ToString());
            int index = dt.Rows.IndexOf(d);
            int c = dt.Rows.Count - 1;
            if (index < c)
            {
                string url = "addjournalentry.aspx?id=" + dt.Rows[index + 1]["Ref"];
                if (Request.QueryString["r"] != null)
                {
                    url += "&r=" + Request.QueryString["r"].ToString();
                }
                Response.Redirect(url);
            }
        }

    }

    protected void lnkPrevious_Click(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();
        if (dt != null)
        {
            dt = (DataTable)Session["Journal"];
            DataColumn[] keyColumns = new DataColumn[1];
            keyColumns[0] = dt.Columns["Ref"];
            dt.PrimaryKey = keyColumns;

            DataRow d = dt.Rows.Find(Request.QueryString["id"].ToString());
            int index = dt.Rows.IndexOf(d);

            if (index > 0)
            {
                string url = "addjournalentry.aspx?id=" + dt.Rows[index - 1]["Ref"];
                if (Request.QueryString["r"] != null)
                {
                    url += "&r=" + Request.QueryString["r"].ToString();
                }
                Response.Redirect(url);
            }
        }

    }

    protected void lnkLast_Click(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();
        if (dt != null)
        {
            dt = (DataTable)Session["Journal"];
            string url = "addjournalentry.aspx?id=" + dt.Rows[dt.Rows.Count - 1]["Ref"];
            if (Request.QueryString["r"] != null)
            {
                url += "&r=" + Request.QueryString["r"].ToString();
            }
            Response.Redirect(url);
        }

    }

    protected void lnkFirst_Click(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();
        if (dt != null)
        {
            dt = (DataTable)Session["Journal"];
            string url = "addjournalentry.aspx?id=" + dt.Rows[0]["Ref"];
            if (Request.QueryString["r"] != null)
            {
                url += "&r=" + Request.QueryString["r"].ToString();
            }
            Response.Redirect(url);
        }

    }

    private bool isLastItem()
    {
        DataTable dt = new DataTable();
        dt = (DataTable)Session["Journal"];
        if (dt != null)
        {
            if (dt.Rows.Count > 0)
            {

                DataColumn[] keyColumns = new DataColumn[1];
                keyColumns[0] = dt.Columns["Ref"];
                dt.PrimaryKey = keyColumns;

                DataRow d = dt.Rows.Find(Request.QueryString["id"].ToString());
                int index = dt.Rows.IndexOf(d);
                int c = dt.Rows.Count - 1;
                if (index == c)
                {
                    return true;
                }
            }
        }
        else
        {
            lnkLast.Enabled = false;
            lnkNext.Enabled = false;
        }
        return false;
    }

    private bool isFirstItem()
    {
        DataTable dt = new DataTable();
        dt = (DataTable)Session["Journal"];
        if (dt != null)
        {
            if (dt.Rows.Count > 0)
            {


                DataColumn[] keyColumns = new DataColumn[1];
                keyColumns[0] = dt.Columns["Ref"];
                dt.PrimaryKey = keyColumns;

                DataRow d = dt.Rows.Find(Request.QueryString["id"].ToString());
                int index = dt.Rows.IndexOf(d);

                if (index == 0)
                {
                    return true;
                }
            }
        }
        else
        {
            lnkFirst.Enabled = false;
            lnkPrevious.Enabled = false;
        }
        return false;
    }

    private void SetInitialRow()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add(new DataColumn("ID", typeof(Int32)));
        dt.Columns.Add(new DataColumn("AcctID", typeof(Int32)));
        dt.Columns.Add(new DataColumn("AcctNo", typeof(string)));
        dt.Columns.Add(new DataColumn("Account", typeof(string)));
        dt.Columns.Add(new DataColumn("fDesc", typeof(string)));
        dt.Columns.Add(new DataColumn("Debit", typeof(string)));
        dt.Columns.Add(new DataColumn("Credit", typeof(string)));
        dt.Columns.Add(new DataColumn("Loc", typeof(string)));
        dt.Columns.Add(new DataColumn("JobName", typeof(string)));
        dt.Columns.Add(new DataColumn("JobID", typeof(string)));
        dt.Columns.Add(new DataColumn("Phase", typeof(string)));
        dt.Columns.Add(new DataColumn("PhaseID", typeof(Int32)));
        dt.Columns.Add(new DataColumn("Company", typeof(string)));
        dt.Columns.Add(new DataColumn("TypeID", typeof(Int32)));
        DataRow dr = dt.NewRow();
        dt.Rows.Add(dr);

        ViewState["Transactions"] = dt;
        RadGrid_Journal.DataSource = dt;
        RadGrid_Journal.DataBind();
    }

    private bool ValidateGridView()
    {
        bool _isValid = true;
        try
        {
            int _filledRows = 0;

            string _validationStr = "";
            for (int i = 0; i < RadGrid_Journal.Items.Count; i++)
            {
                List<TransactionModel> _lstValues = new List<TransactionModel>();
                HiddenField hdnAcctID = (HiddenField)RadGrid_Journal.Items[i].Cells[1].FindControl("hdnAcctID");
                TextBox txtGvAcctName = (TextBox)RadGrid_Journal.Items[i].Cells[1].FindControl("txtGvAcctName");
                TextBox txtGvtransDes = (TextBox)RadGrid_Journal.Items[i].Cells[2].FindControl("txtGvtransDes");
                TextBox txtGvDebit = (TextBox)RadGrid_Journal.Items[i].Cells[3].FindControl("txtGvDebit");
                TextBox txtGvCredit = (TextBox)RadGrid_Journal.Items[i].Cells[4].FindControl("txtGvCredit");

                HiddenField hdnJobID = (HiddenField)RadGrid_Journal.Items[i].Cells[3].FindControl("hdnJobID");
                TextBox txtGvPhase = (TextBox)RadGrid_Journal.Items[i].Cells[4].FindControl("txtGvPhase");
                HiddenField hdnPID = (HiddenField)RadGrid_Journal.Items[i].Cells[4].FindControl("hdnPID");
                bool _isJobSpec = false;
                if (!string.IsNullOrEmpty(hdnAcctID.Value) && !string.IsNullOrEmpty(txtGvtransDes.Text) && !string.IsNullOrEmpty(txtGvDebit.Text) && !string.IsNullOrEmpty(txtGvCredit.Text))
                {
                    if (chkJobSpecific.Checked == true)
                    {
                        _isJobSpec = true;
                        _filledRows++;
                    }
                    else
                    {
                        _filledRows++;
                    }
                }
                _lstValues.Add(new TransactionModel(0, txtGvAcctName.Text));
                _lstValues.Add(new TransactionModel(1, hdnAcctID.Value));
                _lstValues.Add(new TransactionModel(2, txtGvtransDes.Text));
                _lstValues.Add(new TransactionModel(3, txtGvDebit.Text));
                _lstValues.Add(new TransactionModel(4, txtGvCredit.Text));
                bool _isEntered = false;
                foreach (var lst in _lstValues)
                {
                    switch (lst.FieldValue)
                    {
                        case 0:
                            if (!lst.Field.Length.Equals(0))
                                _isEntered = true;
                            break;
                        case 2:
                            if (!lst.Field.Length.Equals(0))
                                _isEntered = true;
                            break;
                        case 3:
                            if (!lst.Field.Length.Equals(0))
                                _isEntered = true;

                            break;
                        case 4:
                            if (!lst.Field.Length.Equals(0))
                                _isEntered = true;
                            break;
                    }

                    if (lst.FieldValue.Equals(4))
                    {
                        if (_isEntered)
                        {
                            #region validate

                            foreach (var l in _lstValues)
                            {
                                switch (l.FieldValue)
                                {
                                    case 1:
                                        if (!l.Field.Length.Equals(0))
                                        {
                                            if (l.Field.Equals("0"))
                                            {
                                                _isValid = false;
                                                _validationStr = "Please select valid account name.";
                                            }
                                        }
                                        break;
                                    case 2:
                                        if (l.Field.Length.Equals(0))
                                        {
                                            _isValid = false;
                                            _validationStr = "Please enter transaction memo.";
                                        }
                                        break;
                                    case 3:
                                        if (l.Field.Length.Equals(0))
                                        {
                                            _isValid = false;
                                            _validationStr = "Please enter debit/credit amount.";
                                        }
                                        break;
                                    case 4:
                                        if (l.Field.Length.Equals(0))
                                        {
                                            _isValid = false;
                                            _validationStr = "Please enter debit/credit amount.";
                                        }
                                        break;
                                }
                                if (_isValid.Equals(false))
                                    break;
                            }
                            #endregion
                        }
                    }
                    if (_isValid.Equals(false)) break;
                }
            }

            if (!string.IsNullOrEmpty(_validationStr))
                ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + _validationStr + "',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }

        return _isValid;
    }

    private void SetForUpdate(int tRef, bool IsRecur = false)
    {
        try
        {
            if (isFirstItem())
            {
                lnkFirst.Enabled = false;
                lnkPrevious.Enabled = false;
            }
            if (isLastItem())
            {
                lnkLast.Enabled = false;
                lnkNext.Enabled = false;
            }
            _objTrans.ConnConfig = Session["config"].ToString();
            _objJournal.ConnConfig = Session["config"].ToString();
            DataSet dsGLA = new DataSet();
            _objJournal.Ref = tRef;
            _objJournal.IsRecurring = IsRecur;

            dsGLA = _objBLJournal.GetDataByRef(_objJournal);
            DataRow drGLA = dsGLA.Tables[0].Rows[0];
            _objJournal.IsJobSpec = Convert.ToBoolean(drGLA["IsJobSpec"]);
            if (_objJournal.IsRecurring.Equals(false))
            {
                hdnBatchID.Value = drGLA["Batch"].ToString();
            }
            if (_objJournal.IsRecurring.Equals(false))
            {

                if (drGLA["OriginalJE"].ToString().Trim() == "" || drGLA["OriginalJE"].ToString().Trim() == "0")
                {
                    lnkOriginal.Text = "";
                    hdnOriginalJE.Value = "0";
                    lnkOriginal.Visible = false;
                }
                else
                {
                    lnkOriginal.Text = "Original JE#" + drGLA["InternalJE"].ToString();
                    hdnOriginalJE.Value = drGLA["OriginalJE"].ToString();
                    lnkOriginal.Visible = true;
                }


            }
            else
            {
                lnkOriginal.Text = "";
                hdnOriginalJE.Value = "0";
                lnkOriginal.Visible = false;
            }

            if (Request.QueryString["c"] != null)
            {
                txtTransDate.Text = DateTime.Now.ToShortDateString();
            }
            else
            {

                txtTransDate.Text = Convert.ToDateTime(drGLA["fDate"]).ToShortDateString();
            }
            if (!IsRecur)
            {
                txtEntryNo.Text = drGLA["Internal"].ToString();
            }
            else
            {
                txtEntryNo1.Text = drGLA["Internal"].ToString();
            }
            txtDescription.Text = drGLA["fDesc"].ToString();
            txtProof.Text = "0";
            lblEntryNo.Visible = true;
            lblEntryNo.Text = string.Format("Entry Number: #{0}", drGLA["Internal"].ToString());
            if (IsRecur)
            {
                ddlFrequency.Items.FindByValue(drGLA["Frequency"].ToString()).Selected = true;
            }

            DataSet dsTrans = new DataSet();
            _objTrans.Ref = tRef;

            if (IsRecur.Equals(true))
            {
                dsTrans = _objBLGLARecur.GetTransDataByRef(_objTrans);
            }
            else
            {
                _objTrans.BatchID = Convert.ToInt32(drGLA["Batch"].ToString());
                dsTrans = _objBLJournal.GetTransDataByBatch(_objTrans);

                if (dsTrans.Tables[0].Rows.Count > 0)
                {
                    DataRow[] dr = dsTrans.Tables[0].Select("Type = 30 AND Sel = 1");
                    if (dr != null)
                    {
                        if (dr.Count() > 0)
                        {
                            if (Request.QueryString["c"] == null)
                            {
                                imgCleared.Visible = true;
                                btnSaveNew.Visible = false;
                            }
                        }
                    }
                }
            }

            DataTable dt = dsTrans.Tables[0];
            DataColumnCollection columns = dt.Columns;
            if (!columns.Contains("Company"))
            {
                dt.Columns.Add("Company");
            }
            if (dsTrans.Tables[0].Rows.Count < 1)
            {
                DataRow dr = null;

                for (int i = dt.Rows.Count; i < 1; i++)
                {
                    dr = dt.NewRow();
                    dt.Rows.Add(dr);
                }
            }
            ViewState["Transactions"] = dt;

            RadGrid_Journal.DataSource = dt;
            RadGrid_Journal.DataBind();

            if (_objJournal.IsJobSpec)
            {
                chkJobSpecific.Checked = true;
                RadGrid_Journal.Columns[7].Display = true;
                RadGrid_Journal.Columns[8].Display = true;
                RadGrid_Journal.Columns[9].Display = true;
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    private void SetForReverse(int tRef, bool IsRecur = false)
    {
        try
        {
            _objTrans.ConnConfig = Session["config"].ToString();
            _objJournal.ConnConfig = Session["config"].ToString();
            DataSet dsGLA = new DataSet();
            _objJournal.Ref = tRef;
            _objJournal.IsRecurring = IsRecur;

            dsGLA = _objBLJournal.GetDataByRef(_objJournal);
            DataRow drGLA = dsGLA.Tables[0].Rows[0];
            _objJournal.IsJobSpec = Convert.ToBoolean(drGLA["IsJobSpec"]);
            
            if (_objJournal.IsRecurring.Equals(false))
            {
                hdnBatchID.Value = drGLA["Batch"].ToString();

                if (drGLA["OriginalJE"].ToString().Trim() == "" || drGLA["OriginalJE"].ToString().Trim() == "0")
                {
                    lnkOriginal.Text = "";
                    hdnOriginalJE.Value = "0";
                    lnkOriginal.Visible = false;
                }
                else
                {
                    lnkOriginal.Text = "Original JE#" + drGLA["InternalJE"].ToString();
                    hdnOriginalJE.Value = drGLA["OriginalJE"].ToString();
                    lnkOriginal.Visible = true;
                }
            }
            else
            {
                lnkOriginal.Text = "";
                hdnOriginalJE.Value = "0";
                lnkOriginal.Visible = false;
            }

            if (IsRecur)
            {
                txtDescription.Text = "Reverse Recurring Entry";
            }
            else
            {
                txtDescription.Text = "Reverse Journal Entry";
            }

            txtTransDate.Text = DateTime.Now.ToShortDateString();
            txtProof.Text = "0";

            if (IsRecur)
            {
                ddlFrequency.Items.FindByValue(drGLA["Frequency"].ToString()).Selected = true;
            }

            DataSet dsTrans = new DataSet();
            _objTrans.Ref = tRef;

            if (IsRecur.Equals(true))
            {
                dsTrans = _objBLGLARecur.GetTransDataByRef(_objTrans);
            }
            else
            {
                _objTrans.BatchID = Convert.ToInt32(drGLA["Batch"].ToString());
                dsTrans = _objBLJournal.GetTransDataByBatch(_objTrans);
            }

            DataTable dt = dsTrans.Tables[0];
            DataColumnCollection columns = dt.Columns;
            if (!columns.Contains("Company"))
            {
                dt.Columns.Add("Company");
            }

            foreach (DataRow row in dt.Rows)
            {
                var temp = row["Credit"];
                row["Credit"] = row["Debit"];
                row["Debit"] = temp;
                row["Amount"] = -Convert.ToDouble(row["Amount"]);
            }

            RadGrid_Journal.DataSource = dt;
            RadGrid_Journal.DataBind();

            if (_objJournal.IsJobSpec)
            {
                chkJobSpecific.Checked = true;
                RadGrid_Journal.Columns[7].Display = true;
                RadGrid_Journal.Columns[8].Display = true;
                RadGrid_Journal.Columns[9].Display = true;
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    private void ResetFormControlValues(Control parent)
    {
        foreach (Control c in parent.Controls)
        {
            if (c.Controls.Count > 0)
            {
                ResetFormControlValues(c);
            }
            else
            {
                switch (c.GetType().ToString())
                {
                    case "System.Web.UI.WebControls.DropDownList":
                        ((DropDownList)c).SelectedIndex = -1;
                        break;
                    case "System.Web.UI.WebControls.TextBox":
                        ((TextBox)c).Text = "";
                        break;
                    case "System.Web.UI.WebControls.CheckBox":
                        ((CheckBox)c).Checked = false;
                        break;
                    case "System.Web.UI.WebControls.RadioButton":
                        ((RadioButton)c).Checked = false;
                        break;
                    case "System.Web.UI.WebControls.HiddenField":
                        ((HiddenField)c).Value = "";
                        break;
                }
            }
        }
        lnkOriginal.Text = "";
        hdnOriginalJE.Value = "0";
    }

    private void SetNewJournalEntry()
    {
        try
        {
            Page.Title = "Add Journal Entry || MOM";
            lblHeader.Text = "Add Journal Entry";
            //lblFrequency.Visible = false;
            //ddlFrequency.Visible = false;
            ToggleRecurring(false);
            hdnIsRecurr.Value = "false";
            SetInitialRow();

            _objJournal.ConnConfig = Session["config"].ToString();
            _objJournal.MaxTransID = _objBLJournal.GetMaxTransID(_objJournal);

            hdnTransID.Value = _objJournal.MaxTransID.ToString();
            txtEntryNo.Text = _objJournal.MaxTransID.ToString();

        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    private void SetGridViewData()
    {
        try
        {
            DataTable dt = (DataTable)ViewState["Transactions"];

            for (int i = 0; i < RadGrid_Journal.Items.Count; i++)
            {
                HiddenField hdnAcctID = (HiddenField)RadGrid_Journal.Items[i].Cells[1].FindControl("hdnAcctID");
                TextBox txtGvAcctNo = (TextBox)RadGrid_Journal.Items[i].Cells[1].FindControl("txtGvAcctNo");
                TextBox txtGvAcctName = (TextBox)RadGrid_Journal.Items[i].Cells[1].FindControl("txtGvAcctName");
                TextBox txtGvtransDes = (TextBox)RadGrid_Journal.Items[i].Cells[2].FindControl("txtGvtransDes");
                TextBox txtGvDebit = (TextBox)RadGrid_Journal.Items[i].Cells[3].FindControl("txtGvDebit");
                TextBox txtGvCredit = (TextBox)RadGrid_Journal.Items[i].Cells[4].FindControl("txtGvCredit");
                Label lblTID = (Label)RadGrid_Journal.Items[i].Cells[4].FindControl("lblTID");

                TextBox txtGvLoc = (TextBox)RadGrid_Journal.Items[i].Cells[3].FindControl("txtGvLoc");
                TextBox txtGvJob = (TextBox)RadGrid_Journal.Items[i].Cells[3].FindControl("txtGvJob");
                HiddenField hdnJobID = (HiddenField)RadGrid_Journal.Items[i].Cells[4].FindControl("hdnJobID");
                TextBox txtGvPhase = (TextBox)RadGrid_Journal.Items[i].Cells[4].FindControl("txtGvPhase");

                HiddenField hdnPID = (HiddenField)RadGrid_Journal.Items[i].Cells[4].FindControl("hdnPID");
                HiddenField hdntypeID = (HiddenField)RadGrid_Journal.Items[i].Cells[4].FindControl("hdntypeID");

                dt.Rows[i]["AcctNo"] = txtGvAcctNo.Text;
                if (!(string.IsNullOrEmpty(hdnAcctID.Value)))
                {
                    dt.Rows[i]["AcctID"] = hdnAcctID.Value;
                }
                dt.Rows[i]["Account"] = txtGvAcctName.Text;
                dt.Rows[i]["Description"] = txtGvtransDes.Text;
                if (!(string.IsNullOrEmpty(txtGvDebit.Text)))
                {
                    dt.Rows[i]["Debit"] = txtGvDebit.Text;
                }
                if (!(string.IsNullOrEmpty(txtGvCredit.Text)))
                {
                    dt.Rows[i]["Credit"] = txtGvCredit.Text;
                }

                dt.Rows[i]["Loc"] = txtGvLoc.Text;
                if (!string.IsNullOrEmpty(hdnJobID.Value))
                {
                    dt.Rows[i]["JobID"] = hdnJobID.Value;
                    dt.Rows[i]["JobName"] = txtGvJob.Text;
                }
                if (!string.IsNullOrEmpty(hdnPID.Value))
                {
                    dt.Rows[i]["Phase"] = txtGvPhase.Text;
                    dt.Rows[i]["PhaseID"] = hdnPID.Value;
                }
                if (!string.IsNullOrEmpty(hdntypeID.Value))
                {
                    dt.Rows[i]["TypeID"] = hdntypeID.Value;
                }
            }
            ViewState["Transactions"] = dt;
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    private DataTable GetAllJEItems(int indexRemove = -1)
    {
        DataTable dt = new DataTable();

        dt.Columns.Add(new DataColumn("ID", typeof(Int32)));
        dt.Columns.Add(new DataColumn("AcctID", typeof(Int32)));
        dt.Columns.Add(new DataColumn("AcctNo", typeof(string)));
        dt.Columns.Add(new DataColumn("Account", typeof(string)));
        dt.Columns.Add(new DataColumn("fDesc", typeof(string)));
        dt.Columns.Add(new DataColumn("Debit", typeof(string)));
        dt.Columns.Add(new DataColumn("Credit", typeof(string)));
        dt.Columns.Add(new DataColumn("Loc", typeof(string)));
        dt.Columns.Add(new DataColumn("JobName", typeof(string)));
        dt.Columns.Add(new DataColumn("JobID", typeof(Int32)));
        dt.Columns.Add(new DataColumn("Phase", typeof(string)));
        dt.Columns.Add(new DataColumn("PhaseID", typeof(Int32)));
        dt.Columns.Add(new DataColumn("LocID", typeof(Int32)));
        dt.Columns.Add(new DataColumn("Company", typeof(string)));
        dt.Columns.Add(new DataColumn("TypeID", typeof(Int32)));
        try
        {
            string strItems = hdnJournalJSON.Value.Trim();
            if (strItems != string.Empty)
            {
                JavaScriptSerializer sr = new JavaScriptSerializer();
                List<Dictionary<object, object>> objJournalItemData = new List<Dictionary<object, object>>();
                objJournalItemData = sr.Deserialize<List<Dictionary<object, object>>>(strItems);
                int i = 0;
                foreach (Dictionary<object, object> dict in objJournalItemData)
                {
                    if (indexRemove == -1 || indexRemove != i)
                    {
                        DataRow dr = dt.NewRow();
                        if (dict["hdnAcctID"].ToString().Trim() != string.Empty)
                        {
                            dr["AcctID"] = Convert.ToInt32(dict["hdnAcctID"].ToString());
                        }
                        dr["Account"] = dict["txtGvAcctName"].ToString();
                        dr["AcctNo"] = dict["txtGvAcctNo"].ToString();
                        dr["fDesc"] = dict["txtGvtransDes"].ToString();
                        dr["Debit"] = dict["txtGvDebit"].ToString() == "" ? "0.00" : dict["txtGvDebit"].ToString();
                        dr["Credit"] = dict["txtGvCredit"].ToString() == "" ? "0.00" : dict["txtGvCredit"].ToString();

                        if (chkJobSpecific.Checked)
                        {
                            dr["Loc"] = dict["txtGvLoc"].ToString();
                            dr["JobName"] = dict["txtGvJob"].ToString();
                            if (dict["hdnJobID"].ToString() != string.Empty)
                            {
                                dr["JobID"] = Convert.ToInt32(dict["hdnJobID"].ToString());
                            }
                            dr["Phase"] = dict["txtGvPhase"].ToString();
                            if (dict["hdnPID"].ToString() != string.Empty)
                            {
                                dr["PhaseID"] = Convert.ToInt32(dict["hdnPID"]);
                            }
                            if (dict["hdntypeID"].ToString() != string.Empty)
                            {
                                dr["TypeID"] = Convert.ToInt32(dict["hdntypeID"]);
                            }
                        }

                        dt.Rows.Add(dr);
                    }

                    i++;
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }

        return dt;
    }

    private DataTable GetJEItems()
    {
        DataTable dt = new DataTable();

        dt.Columns.Add(new DataColumn("ID", typeof(Int32)));
        dt.Columns.Add(new DataColumn("AcctID", typeof(Int32)));
        dt.Columns.Add(new DataColumn("AcctNo", typeof(string)));
        dt.Columns.Add(new DataColumn("Account", typeof(string)));
        dt.Columns.Add(new DataColumn("fDesc", typeof(string)));
        dt.Columns.Add(new DataColumn("Debit", typeof(string)));
        dt.Columns.Add(new DataColumn("Credit", typeof(string)));
        dt.Columns.Add(new DataColumn("Loc", typeof(string)));
        dt.Columns.Add(new DataColumn("JobName", typeof(string)));
        dt.Columns.Add(new DataColumn("JobID", typeof(Int32)));
        dt.Columns.Add(new DataColumn("Phase", typeof(string)));
        dt.Columns.Add(new DataColumn("PhaseID", typeof(Int32)));
        dt.Columns.Add(new DataColumn("LocID", typeof(Int32)));
        dt.Columns.Add(new DataColumn("TypeID", typeof(Int32)));

        try
        {
            string strItems = hdnJournalJSON.Value.Trim();
            if (strItems != string.Empty)
            {
                JavaScriptSerializer sr = new JavaScriptSerializer();
                List<Dictionary<object, object>> objJournalItemData = new List<Dictionary<object, object>>();
                objJournalItemData = sr.Deserialize<List<Dictionary<object, object>>>(strItems);
                int i = 0;
                foreach (Dictionary<object, object> dict in objJournalItemData)
                {
                    if ((dict["hdnAcctID"].ToString() == "0") || (dict["hdnAcctID"].ToString() == "") || (dict["hdnAcctID"].ToString() == string.Empty) || (dict["txtGvtransDes"].ToString() == string.Empty) || (dict["txtGvDebit"].ToString() == string.Empty) || (dict["txtGvCredit"].ToString() == string.Empty))
                    {
                        continue;
                    }

                    i++;
                    DataRow dr = dt.NewRow();

                    dr["AcctID"] = Convert.ToInt32(dict["hdnAcctID"].ToString());
                    dr["Account"] = dict["txtGvAcctName"].ToString();
                    dr["AcctNo"] = dict["txtGvAcctNo"].ToString();
                    dr["fDesc"] = dict["txtGvtransDes"].ToString();
                    dr["Debit"] = dict["txtGvDebit"].ToString().Replace(",", String.Empty) == "" ? "0.00" : dict["txtGvDebit"].ToString().Replace(",", String.Empty);
                    dr["Credit"] = dict["txtGvCredit"].ToString().Replace(",", String.Empty) == "" ? "0.00" : dict["txtGvCredit"].ToString().Replace(",", String.Empty);

                    if (chkJobSpecific.Checked == true)
                    {
                        dr["Loc"] = dict["txtGvLoc"].ToString();
                        dr["JobName"] = dict["txtGvJob"].ToString();
                        if (!string.IsNullOrEmpty(dict["hdnJobID"].ToString()) && dict["hdnJobID"].ToString() != "0")
                        {
                            dr["JobID"] = Convert.ToInt32(dict["hdnJobID"].ToString());
                        }
                        dr["Phase"] = dict["txtGvPhase"].ToString();
                        if (!string.IsNullOrEmpty(dict["hdnPID"].ToString()) && dict["hdnPID"].ToString() != "0")
                        {
                            dr["PhaseID"] = Convert.ToInt32(dict["hdnPID"].ToString());
                        }
                        if (!string.IsNullOrEmpty(dict["hdntypeID"].ToString()))
                        {
                            dr["TypeID"] = Convert.ToInt32(dict["hdntypeID"].ToString());
                        }
                        else
                        {
                            dr["TypeID"] = DBNull.Value;
                        }
                    }

                    dt.Rows.Add(dr);
                }
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
        return dt;
    }

    private void FillFrequency()
    {
        try
        {
            List<Frequency> _lstFrequency = new List<Frequency>();
            _lstFrequency = FrequencyHelper.GetAll();
            ddlFrequency.DataSource = _lstFrequency;
            ddlFrequency.DataValueField = "ID";
            ddlFrequency.DataTextField = "Name";
            ddlFrequency.DataBind();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    private void userpermissions()
    {
        if (Session["type"].ToString() != "am" && Session["type"].ToString() != "c")
        {
            DataTable dtUserPermission = new DataTable();
            dtUserPermission = GetUserById();
            /// AccountPayablemodulePermission ///////////////////------->

            string FinancialmodulePermission = dtUserPermission.Rows[0]["FinancialmodulePermission"] == DBNull.Value ? "Y" : dtUserPermission.Rows[0]["FinancialmodulePermission"].ToString();

            if (FinancialmodulePermission == "N")
            {
                Response.Redirect("Home.aspx?permission=no"); return;
            }

            /// Journal Entry  ///////////////////------->

            string GLAdjPermission = dtUserPermission.Rows[0]["GLAdj"] == DBNull.Value ? "YYYYYY" : dtUserPermission.Rows[0]["GLAdj"].ToString();
            string ADD = GLAdjPermission.Length < 1 ? "Y" : GLAdjPermission.Substring(0, 1);
            string Edit = GLAdjPermission.Length < 2 ? "Y" : GLAdjPermission.Substring(1, 1);
            string Delete = GLAdjPermission.Length < 3 ? "Y" : GLAdjPermission.Substring(2, 1);
            string View = GLAdjPermission.Length < 4 ? "Y" : GLAdjPermission.Substring(3, 1);

            if (Request.QueryString["id"] != null)
            {
                //aImport.Visible = false;
            }
            if (View == "N")
            {
                Response.Redirect("Home.aspx?permission=no"); return;
            }
            else if (Request.QueryString["id"] == null)
            {
                if (ADD == "N")
                {
                    Response.Redirect("Home.aspx?permission=no"); return;
                }
            }
            else if (Edit == "N")
            {
                if (View == "Y")
                {
                    btnSaveNew.Visible = false;
                    //btnSubmitJob.Visible = false;
                }
                else
                {
                    Response.Redirect("Home.aspx?permission=no"); return;
                }
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
        ds = _objBLUser.GetUserPermissionByUserID(objPropUser);
        return ds.Tables[0];
    }
    #endregion

    protected void RadMenuJournalEntryGrid_ItemClick(object sender, RadMenuEventArgs e)
    {
        DataTable dt = GetAllJEItems();
        switch (e.Item.Text)
        {
            case "Add Row Above":
                AddNewRowGrid(dt, "above");
                break;

            case "Add Row Below":
                AddNewRowGrid(dt, "below");
                break;
        }
    }

    private void AddNewRowGrid(DataTable dt, string position)
    {
        DataRow dr = dt.NewRow();
        Int32 _line = 0;

        _line = Int32.Parse(radGridClickedRowIndex.Value);
        if (position == "above")
        {
            dt.Rows.InsertAt(dr, _line);
        }
        else if (position == "below")
        {
            dt.Rows.InsertAt(dr, _line + 1);
        }

        RadGrid_Journal.DataSource = dt;
        RadGrid_Journal.DataBind();
    }

    protected void lnkFileUploaded_Click(object sender, EventArgs e)
    {
        try
        {
            string[] validFileTypes = { ".csv", ".xls", ".xlsx" };
            string ext = System.IO.Path.GetExtension(FileUploadControl.PostedFile.FileName).ToLower();
            var results = Array.FindAll(validFileTypes, s => s.Equals(ext));
            if (results.Length == 0)
            {
                ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: 'Please upload a csv or excel file.',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            }
            else
            {
                DataTable dt = new DataTable();
                if (FileUploadControl.HasFile)
                {
                    if (ext == ".csv")
                        dt = ReadCsvFile();
                    if (ext == ".xls" || ext == ".xlsx")
                        dt = ReadExcelFile();

                    _objJournal.ConnConfig = Session["config"].ToString();
                    _objJournal.DtTrans = dt;
                    DataSet ds = _objBLJournal.GetJournalEntryItemData(_objJournal);

                    var dt0 = ds.Tables[0];
                    var validRowsCount = ds.Tables[0].Rows.Count;
                    var errorRowsCount = ds.Tables[1].Rows.Count;
                    if (validRowsCount < 1)
                    {
                        DataRow dr = null;

                        for (int i = validRowsCount; i < 1; i++)
                        {
                            dr = dt0.NewRow();
                            dt0.Rows.Add(dr);
                        }
                    }

                    ViewState["ImportExcelData"] = dt0;

                    if (errorRowsCount > 0)
                    {
                        gv_Errorrows.DataSource = ds.Tables[1];
                        gv_Errorrows.Rebind();

                        btnContinue.Visible = true;
                        btnCancel.Visible = true;

                        lblTotalRows.Text = Convert.ToString(validRowsCount + errorRowsCount);
                        lblValidRows.Text = Convert.ToString(validRowsCount);
                        lblInvalidRows.Text = Convert.ToString(errorRowsCount);
                        string script = "function f(){OpenErrorModal(); Sys.Application.remove_load(f);}Sys.Application.add_load(f);";
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
                    }
                    else
                    {

                        //BINDGRID(ds.Tables[0]);
                        RadGrid_Journal.DataSource = dt0;
                        RadGrid_Journal.DataBind();
                        //ScriptManager.RegisterStartupScript(this, Page.GetType(), "CallMyFunction", "CalculateTotalAmt()", true);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void btnDownloadCSV_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/TempPDF/ImportJournalEntries/Sample.csv");
    }

    protected void btnDownloadExcel_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/TempPDF/ImportJournalEntries/Sample.xls");
    }

    public DataTable ReadCsvFile()
    {
        try
        {
            DataTable dtCsv = new DataTable();
            dtCsv.Columns.Add("AccNo", typeof(string));
            dtCsv.Columns.Add("Memo", typeof(string));
            dtCsv.Columns.Add("Amount", typeof(string));
            dtCsv.Columns.Add("RowNo", typeof(int));

            string Fulltext;
            string FileSaveWithPath = Server.MapPath("~\\TempPDF\\ImportJournalEntries\\Import" + System.DateTime.Now.ToString("ddMMyyyy_hhmmss") + ".csv");
            FileUploadControl.SaveAs(FileSaveWithPath);
            using (StreamReader sr = new StreamReader(FileSaveWithPath))
            {
                while (!sr.EndOfStream)
                {
                    Fulltext = sr.ReadToEnd().ToString();
                    Fulltext = Fulltext.TrimEnd();
                    string[] rows = Fulltext.Split('\n');

                    for (int i = 1; i < rows.Count(); i++)
                    {
                        System.Text.RegularExpressions.Regex CSVParser = new System.Text.RegularExpressions.Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
                        String[] rowValues = CSVParser.Split(rows[i]);

                        if (rowValues.Length == 3)
                        {
                            DataRow dr = dtCsv.NewRow();
                            for (int k = 0; k < rowValues.Count(); k++)
                            {
                                if (k != 2)
                                {
                                    if (string.IsNullOrEmpty(rowValues[k]))
                                        dr[k] = DBNull.Value;
                                    else
                                        dr[k] = rowValues[k].ToString();
                                }
                                else
                                {
                                    dr[k] = CleanUpCurrencyDollarFormat(rowValues[k].ToString());
                                }
                            }
                            dr[rowValues.Count()] = i;
                            dtCsv.Rows.Add(dr);
                        }
                        else
                        {
                            throw new Exception("The format of imported file is not correct. Please check your file again.");
                        }
                    }
                }
            }
            if (File.Exists(FileSaveWithPath))
                File.Delete(FileSaveWithPath);
            return dtCsv;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public DataTable ReadExcelFile()
    {
        try
        {
            string ext = System.IO.Path.GetExtension(FileUploadControl.PostedFile.FileName).ToLower();

            string FileSaveWithPath = Server.MapPath("~\\TempPDF\\ImportJournalEntries\\Import" + System.DateTime.Now.ToString("ddMMyyyy_hhmmss") + ext);
            FileUploadControl.SaveAs(FileSaveWithPath);

            OleDbConnection oledbConn = new OleDbConnection();
            if (ext == ".xls")
                oledbConn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + FileSaveWithPath + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"");
            else if (ext == ".xlsx")
                oledbConn = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + FileSaveWithPath + ";Extended Properties='Excel 12.0;HDR=YES;IMEX=1;';");

            oledbConn.Open();

            System.Data.DataTable dt = null;
            // Get the data table containg the schema guid.
            dt = oledbConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

            if (dt != null && dt.Rows.Count > 0)
            {
                //String[] excelSheets = new String[dt.Rows.Count];
                //int i = 0;

                //// Add the sheet name to the string array.
                //foreach (DataRow row in dt.Rows)
                //{
                //    excelSheets[i] = row["TABLE_NAME"].ToString();
                //    i++;
                //}

                string firstSheetName = dt.Rows[0]["TABLE_NAME"].ToString();

                OleDbCommand ocmd = new OleDbCommand("select * from [" + firstSheetName + "]", oledbConn);
                OleDbDataAdapter oleda = new OleDbDataAdapter(ocmd);
                DataSet ds = new DataSet();
                ds.Tables.Add("xlsImport");
                DataTable dtExcel = ds.Tables[0];
                dtExcel.Columns.Add("AccNo", typeof(string));
                dtExcel.Columns.Add("Memo", typeof(string));
                dtExcel.Columns.Add("Amount", typeof(string));

                oleda.Fill(ds, "xlsImport");
                //oleda.Fill(ds);
                oledbConn.Close();

                if (File.Exists(FileSaveWithPath))
                    File.Delete(FileSaveWithPath);



                //ds.Tables[0].AsEnumerable().CopyToDataTable(dtExcel, LoadOption.OverwriteChanges);

                //var isExisted_AccNo = false;
                //var isExisted_Memo = false;
                //var isExisted_Amount = false;
                if (dtExcel.Columns.Count != 3)
                {
                    throw new Exception("The format imported file is not correct. Please check your file again.");
                }
                else
                {
                    //foreach (DataColumn item in dtExcel.Columns)
                    //{
                    //    if (!isExisted_AccNo)
                    //        isExisted_AccNo = item.ColumnName.Equals("AccNo", StringComparison.InvariantCultureIgnoreCase);
                    //    if (!isExisted_Memo)
                    //        isExisted_Memo = item.ColumnName.Equals("Memo", StringComparison.InvariantCultureIgnoreCase);
                    //    if (!isExisted_Amount)
                    //        isExisted_Amount = item.ColumnName.Equals("Amount", StringComparison.InvariantCultureIgnoreCase);
                    //}
                    var colsError = new StringBuilder();

                    if (!dtExcel.Columns[0].ColumnName.Equals("AccNo", StringComparison.InvariantCultureIgnoreCase))
                    {
                        colsError.Append("The first column should be \"AccNo\". ");
                        //throw new Exception("The first column should be \"AccNo\". Please check your file again.");
                    }
                    if (!dtExcel.Columns[1].ColumnName.Equals("Memo", StringComparison.InvariantCultureIgnoreCase))
                    {
                        colsError.Append("The second column should be \"Memo\". ");
                        //throw new Exception("The first column should be \"Memo\". Please check your file again.");
                    }
                    if (!dtExcel.Columns[2].ColumnName.Equals("Amount", StringComparison.InvariantCultureIgnoreCase))
                    {
                        colsError.Append("The third column should be \"Amount\". ");
                        //throw new Exception("The first column should be \"Amount\". Please check your file again.");
                    }

                    if (colsError.Length > 0)
                    {
                        colsError.Append("Please check your file again.");
                        throw new Exception(colsError.ToString());
                    }
                }

                //if (!isExisted_AccNo)
                //{
                //    throw new Exception("Can not find AccNo column. Please check your file again.");
                //}

                //if (!isExisted_Memo)
                //{
                //    throw new Exception("Can not find Memo column. Please check your file again.");
                //}

                //if (!isExisted_Amount)
                //{
                //    throw new Exception("Can not find Amount column. Please check your file again.");
                //}

                dtExcel.Columns.Add("RowNo", typeof(int));

                for (int i = 0; dtExcel.Rows.Count > i; i++)
                {
                    dtExcel.Rows[i]["RowNo"] = i + 1;
                    //dtExcel.Rows[i]["Amount"] = dtExcel.Rows[i]["Amount"].ToString();//CleanUpCurrencyDollarFormat(dtExcel.Rows[i]["Amount"].ToString());
                }
                return dtExcel;
            }
            else
            {
                throw new Exception("Can't find a data sheet in import file. Please check your file again.");
            }

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private string CleanUpCurrencyDollarFormat(string strCurrency)
    {
        try
        {
            var dblReturn = double.Parse(strCurrency.Replace('$', '0'), NumberStyles.AllowParentheses |
                                                        NumberStyles.AllowThousands |
                                                        NumberStyles.AllowDecimalPoint | NumberStyles.AllowTrailingSign |
                                                        NumberStyles.Float);
            return dblReturn.ToString();
        }
        catch (Exception)
        {
            return strCurrency;
        }
    }

    private void ParseImportDataToDataTable()
    {
        DataTable dt = new DataTable();
        DataRow dr = null;
        dt.Columns.Add(new DataColumn("ID", typeof(Int32)));
        dt.Columns.Add(new DataColumn("AcctID", typeof(Int32)));
        dt.Columns.Add(new DataColumn("AcctNo", typeof(string)));
        dt.Columns.Add(new DataColumn("Account", typeof(string)));
        dt.Columns.Add(new DataColumn("fDesc", typeof(string)));
        dt.Columns.Add(new DataColumn("Debit", typeof(string)));
        dt.Columns.Add(new DataColumn("Credit", typeof(string)));
        dt.Columns.Add(new DataColumn("Loc", typeof(string)));
        dt.Columns.Add(new DataColumn("JobName", typeof(string)));
        dt.Columns.Add(new DataColumn("JobID", typeof(string)));
        dt.Columns.Add(new DataColumn("Phase", typeof(string)));
        dt.Columns.Add(new DataColumn("PhaseID", typeof(Int32)));
        dt.Columns.Add(new DataColumn("Company", typeof(string)));
        dt.Columns.Add(new DataColumn("TypeID", typeof(Int32)));

        int rowIndex = 0;
        for (int i = 0; i < 1; i++)
        {
            dr = dt.NewRow();
            dt.Rows.Add(dr);
            rowIndex++;
        }

        ViewState["Transactions"] = dt;
        RadGrid_Journal.DataSource = dt;
        RadGrid_Journal.DataBind();
    }

    protected void btnContinue_Click(object sender, EventArgs e)
    {
        try
        {
            btnContinue.Visible = false;
            btnCancel.Visible = false;
            //ScriptManager.RegisterStartupScript(this, Page.GetType(), "CloseModal", "CloseModal()", true);

            DataTable dt = (DataTable)ViewState["ImportExcelData"];
            if (dt != null && dt.Rows.Count > 0)
            {

                RadGrid_Journal.DataSource = dt;
                RadGrid_Journal.DataBind();
                //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "CallMyFunction", "CalculateTotalAmt()", true);
            }
            else
            {
                ParseImportDataToDataTable();
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "Allrows", "noty({text: 'All rows are invalid.',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        btnContinue.Visible = false;
        btnCancel.Visible = false;

        DataTable dt = (DataTable)ViewState["Transactions"];

        RadGrid_Journal.DataSource = dt;
        RadGrid_Journal.DataBind();
        //DataTable dt = GetTable();
        //for (int i = 0; i < 4; i++)
        //{
        //    DataRow drJob = dt.NewRow();
        //    dt.Rows.Add(drJob);
        //}
        //ViewState["Transactions_JobCost"] = dt;
        //BINDGRID(dt);
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "CloseModal", "CloseModal()", true);
    }

    protected void btnCopyPrevious_Click(object sender, EventArgs e)
    {
        try
        {
            var selectIndex = 0;

            if (!string.IsNullOrEmpty(hdnSelectPOIndex.Value))
            {
                selectIndex = Convert.ToInt32(hdnSelectPOIndex.Value);
            }
            else
            {
                var selectItem = RadGrid_Journal.MasterTableView.GetSelectedItems();
                if (selectItem.Count() > 0)
                {
                    selectIndex = selectItem[0].ClientRowIndex;
                }
            }


            var dt = GetAllJEItems(); ;
            if (dt.Rows.Count > 0 && selectIndex > 0)
            {
                var copyRow = dt.Rows[selectIndex - 1];
                var dr = dt.Rows[selectIndex];
                //int i_ref = Convert.ToInt32(dr["Ref"]);
                //int i_line = Convert.ToInt32(dr["line"]);
                dr.ItemArray = copyRow.ItemArray.Clone() as object[];
                //dr["Ref"] = i_ref;
                //dr["line"] = i_line;
                dt.AcceptChanges();

                ViewState["Transactions"] = dt;
                RadGrid_Journal.DataSource = dt;
                RadGrid_Journal.DataBind();

                // ScriptManager.RegisterStartupScript(this, Page.GetType(), "CalculateTotalUseTaxExpense", "CalculateTotalUseTaxExpense();", true);
            }

        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    #region Add Account popup

    protected void lnkSaveAccount_Click(object sender, EventArgs e)
    {
        try
        {
            if (Page.IsValid)
            {
                _objChart.ConnConfig = Session["config"].ToString();
                _objChart.Acct = txtAcctNum.Text;
                _objChart.fDesc = txtAcName.Text;
                _objChart.AcType = Convert.ToInt32(ddlType.SelectedValue);

                if (ddlCentral.SelectedValue != "")
                {
                    _objChart.Department = Convert.ToInt32(ddlCentral.SelectedValue);
                }
                else
                {
                    _objChart.Department = null;
                }

                if (ddlSubAcCategory.SelectedValue != " Select Sub Category ")
                {
                    _objChart.Sub = ddlSubAcCategory.SelectedItem.Text;
                }
                else
                {
                    _objChart.Sub = "";
                }

                _objChart.Sub2 = "";
                _objChart.Remarks = txtAcctDescription.Text;
                _objChart.InUse = 1;
                _objChart.Detail = 0;
                _objChart.Status = Convert.ToInt32(ddlStatus.SelectedIndex);
                _objChart.Contact = txtContact.Text;
                _objChart.Address = txtAddress.Text;
                _objChart.Phone = txtPhone.Text;
                _objChart.Fax = txtFax.Text;
                _objChart.City = txtCity.Text;
                _objChart.Cellular = txtCellular.Text;
                _objChart.BankName = txtBankName.Text;
                _objChart.Lat = lat.Text;
                _objChart.Long = lng.Text;

                if (!ddlState.SelectedValue.Equals("Select State"))
                {
                    _objChart.State = ddlState.SelectedValue;
                }

                _objChart.Zip = txtZip.Text;
                _objChart.EMail = txtEmail.Text;
                _objChart.Country = ddlCountry.SelectedValue;
                _objChart.Website = txtWebsite.Text;
                _objChart.NBranch = txtBranch.Text;
                _objChart.NAcct = txtAcct.Text;
                _objChart.NRoute = txtRoute.Text;

                if (!string.IsNullOrEmpty(txtCreditLimit.Text))
                {
                    _objChart.CLimit = Convert.ToDouble(txtCreditLimit.Text);
                }

                if (!string.IsNullOrEmpty(txtNCheck.Text))
                {
                    _objChart.NextC = Convert.ToInt32(txtNCheck.Text);
                }

                if (!string.IsNullOrEmpty(txtNDeposit.Text))
                {
                    _objChart.NextD = Convert.ToInt32(txtNDeposit.Text);
                }

                if (!string.IsNullOrEmpty(txtNEPay.Text))
                {
                    _objChart.NextE = Convert.ToInt32(txtNEPay.Text);
                }

                if (!string.IsNullOrEmpty(txtRate.Text))
                {
                    _objChart.Rate = Convert.ToDouble(txtRate.Text);
                }

                if (!string.IsNullOrEmpty(ddlStatus.SelectedValue))
                {
                    _objChart.Status = Convert.ToInt32(ddlStatus.SelectedValue);
                }

                if (chkWarn.Checked == true)
                {
                    _objChart.Warn = 1;
                }
                else
                {
                    _objChart.Warn = 0;
                }

                if (Convert.ToInt32(ViewState["CompPermission"]) == 1)
                {
                    _objChart.EN = Convert.ToInt32(ddlCompany.SelectedValue);
                }
                else
                {
                    _objChart.EN = 0;
                }
                _objBank.ACHBatchControlString1 = "";
                _objBank.ACHBatchControlString2 = "";
                _objBank.ACHBatchControlString3 = "";
                _objBank.ACHCompanyHeaderString1 = "";
                _objBank.ACHCompanyHeaderString2 = "";
                _objBank.ACHFileControlString1 = "";
                _objBank.ACHFileHeaderStringA = "";
                _objBank.ACHFileHeaderStringB = "";
                _objBank.ACHFileHeaderStringC = "";
                _objBank.APACHCompanyID = "";
                _objBank.APImmediateOrigin = "";
                //if (txtNextACH.Text != null && txtNextACH.Text.Trim() != "")
                //{
                //    _objBank.NextACH = txtNextACH.Text.Trim();
                //}
                //else
                //{
                    _objBank.NextACH = "0";
                //}
                _objBLChart.AddChart(_objChart, _objBank);

                var dsChart = _objBLChart.GetChartByAcct(_objChart);
                if (dsChart.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = GetAllJEItems();

                    if (!string.IsNullOrEmpty(hdnSelectPOIndex.Value))
                    {
                        var index = Convert.ToInt32(hdnSelectPOIndex.Value);
                        dt.Rows[index]["AcctID"] = dsChart.Tables[0].Rows[0]["ID"];
                        dt.Rows[index]["AcctNo"] = _objChart.Acct;
                        dt.Rows[index]["Account"] = _objChart.fDesc;
                        dt.Rows[index]["fDesc"] = txtDescription.Text;
                    }

                    Session["JEGridData"] = dt;
                }

                ResetFormControlValues(RadAddAccountWindow);
                ShowMesg("Chart of Account Added Successfully!", 1);
                ScriptManager.RegisterStartupScript(this, GetType(), "close", "CloseModal();", true);
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true})", true);
        }
    }

    protected void lbtnUpdateGridData_Click(object sender, EventArgs e)
    {
        DataTable dt = (DataTable)Session["JEGridData"];

        RadGrid_Journal.DataSource = dt;
        RadGrid_Journal.DataBind();

        Session.Remove("JEGridData");
    }

    protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FillSubAccount();
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "ShowBankPartial", "ShowBankPartial();", true);
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true})", true);
        }
    }

    protected void cvAccountNum_ServerValidate(object source, ServerValidateEventArgs args)
    {
        try
        {
            bool IsExists = false;
            _objChart.ConnConfig = Session["config"].ToString();
            _objChart.Acct = txtAcctNum.Text;

            if (Request.QueryString["id"] != null & Request.QueryString["c"] == null)
            {
                _objChart.ID = Convert.ToInt32(Request.QueryString["id"]);
                IsExists = _objBLChart.IsExistAcctForEdit(_objChart);
            }
            else
            {
                IsExists = _objBLChart.IsExistAcct(_objChart);
            }
            if (IsExists)
            {
                args.IsValid = false;
            }
            else
            {
                args.IsValid = true;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void cvAcName_ServerValidate(object source, ServerValidateEventArgs args)
    {
        try
        {
            bool IsAcNameExists = false;
            _objChart.ConnConfig = Session["config"].ToString();
            _objChart.fDesc = txtAcName.Text;
            if (Request.QueryString["id"] != null & Request.QueryString["c"] == null)
            {
                _objChart.ID = Convert.ToInt32(Request.QueryString["id"]);
                IsAcNameExists = _objBLChart.IsExistAcctNameForEdit(_objChart);
            }
            else
            {
                IsAcNameExists = _objBLChart.IsExistAcctNameExists(_objChart);
            }
            if (IsAcNameExists)
            {
                args.IsValid = false;
            }
            else
            {
                args.IsValid = true;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void FillState()
    {
        try
        {
            DataSet _dsState = new DataSet();
            State _objState = new State();

            _objState.ConnConfig = Session["config"].ToString();

            _dsState = _objBLBank.GetStates(_objState);

            ddlState.Items.Add(new ListItem("Select State"));
            ddlState.AppendDataBoundItems = true;

            ddlState.DataSource = _dsState;
            ddlState.DataValueField = "Name";
            ddlState.DataTextField = "fDesc";
            ddlState.DataBind();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    private void FillStatus()
    {
        try
        {
            _objChart.ConnConfig = Session["config"].ToString();
            DataSet ds = _objBLChart.GetAllStatus(_objChart);
            ddlStatus.DataSource = ds;
            ddlStatus.DataBind();
            ddlStatus.DataValueField = "ID";
            ddlStatus.DataTextField = "Status";
            ddlStatus.DataBind();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    private void FillAccountType()
    {
        try
        {
            _objAcType.ConnConfig = Session["config"].ToString();
            DataSet _dsType = new DataSet();
            _dsType = _objBLAcType.GetAllType(_objAcType);
            ddlType.DataSource = _dsType;
            ddlType.DataBind();
            ddlType.DataValueField = "ID";
            ddlType.DataTextField = "Type";
            ddlType.DataBind();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    private void FillCentral()
    {
        try
        {
            _objPropUser.ConnConfig = Session["config"].ToString();

            DataSet _dsDepartment = new DataSet();
            _dsDepartment = _objBLUser.getCentral(_objPropUser);

            if (_dsDepartment != null)
            {
                ddlCentral.Items.Clear();

                if (_dsDepartment.Tables.Count > 0)
                {
                    ddlCentral.Items.Insert(0, new ListItem(" Select Center ", ""));
                    ddlCentral.AppendDataBoundItems = true;
                    ddlCentral.DataSource = _dsDepartment.Tables[0];
                    ddlCentral.DataValueField = "ID";
                    ddlCentral.DataTextField = "CentralName";
                    ddlCentral.DataBind();
                }
                else
                {
                    ddlCentral.Items.Insert(0, new ListItem(" No Center Available ", ""));
                }
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    private void FillSubAccount()
    {
        try
        {
            _objAcType.ConnConfig = Session["config"].ToString();
            _objAcType.CType = Convert.ToInt32(ddlType.SelectedValue);

            DataSet _dsSubType = new DataSet();
            _dsSubType = _objBLAcType.GetTypeByAccount(_objAcType);

            if (_dsSubType != null)
            {
                ddlSubAcCategory.Items.Clear();

                if (_dsSubType.Tables.Count > 0)
                {
                    ddlSubAcCategory.Items.Add(new ListItem(" Select Sub Category "));
                    ddlSubAcCategory.AppendDataBoundItems = true;
                    ddlSubAcCategory.DataSource = _dsSubType;
                    ddlSubAcCategory.DataValueField = "ID";
                    ddlSubAcCategory.DataTextField = "SubType";

                    ddlSubAcCategory.DataBind();
                }
                else
                {
                    ddlSubAcCategory.Items.Insert(0, new ListItem(" No Sub Category Available ", "0"));
                }
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    private void FillCompany()
    {
        objCompany.UserID = Convert.ToInt32(Session["UserID"].ToString());
        objCompany.DBName = Session["dbname"].ToString();
        objCompany.ConnConfig = Session["config"].ToString();
        DataSet ds = new DataSet();
        ds = _objBLCompany.getCompanyByCustomer(objCompany);
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddlCompany.DataSource = ds.Tables[0];
            ddlCompany.DataTextField = "Name";
            ddlCompany.DataValueField = "CompanyID";
            ddlCompany.DataBind();
            ddlCompany.Items.Insert(0, new ListItem("Select", "0"));
        }
    }

    #endregion

    protected void RadGrid_gvLogs_ItemCreated(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridPagerItem)
        {
            var dropDown = (RadComboBox)e.Item.FindControl("PageSizeComboBox");
            var totalCount = ((GridPagerItem)e.Item).Paging.DataSourceCount;
            if (totalCount == 0) totalCount = 1000;

            GeneralFunctions obj = new GeneralFunctions();

            var sizes = obj.TelerikPageSize(totalCount);

            dropDown.Items.Clear();

            foreach (var size in sizes)
            {
                var cboItem = new RadComboBoxItem() { Text = size.Key, Value = size.Value };
                cboItem.Attributes.Add("ownerTableViewId", e.Item.OwnerTableView.ClientID);
                if (e.Item.OwnerTableView.PageSize.ToString() == size.Value) cboItem.Selected = true;
                dropDown.Items.Add(cboItem);
            }
        }
    }

    protected void RadGrid_gvLogs_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            RadGrid_gvLogs.AllowCustomPaging = !ShouldApplySortFilterOrGroupLogs();
            if (Request.QueryString["id"] != null)
            {
                DataSet dsLog = new DataSet();
                Customer objProp_Customer = new Customer();
                BL_Customer objBL_Customer = new BL_Customer();
                objProp_Customer.ConnConfig = Session["config"].ToString();
                objProp_Customer.LogRefId = Convert.ToInt32(Request.QueryString["id"]);
                if (Request.QueryString["r"] != null && Request.QueryString["r"].ToString() == "1")
                    objProp_Customer.LogScreen = "Recurring Entry";
                else
                    objProp_Customer.LogScreen = "Journal Entry";
                dsLog = objBL_Customer.GetLogs(objProp_Customer);
                if (dsLog.Tables[0].Rows.Count > 0)
                {
                    RadGrid_gvLogs.VirtualItemCount = dsLog.Tables[0].Rows.Count;
                    RadGrid_gvLogs.DataSource = dsLog.Tables[0];
                }
                else
                {
                    RadGrid_gvLogs.DataSource = string.Empty;
                }
            }
            else
            {
                RadGrid_gvLogs.DataSource = string.Empty;
            }
        }
        catch { }
    }
    bool isGroupLog = false;

    public bool ShouldApplySortFilterOrGroupLogs()
    {
        return RadGrid_gvLogs.MasterTableView.FilterExpression != "" ||
            (RadGrid_gvLogs.MasterTableView.GroupByExpressions.Count > 0 || isGroupLog) ||
            RadGrid_gvLogs.MasterTableView.SortExpressions.Count > 0;
    }
    protected void lnkOriginal_Click(object sender, EventArgs e)
    {
        Response.Redirect("addjournalentry.aspx?id=" + hdnOriginalJE.Value + "&frm=MNG");
    }

    #region Documents
    protected void lnkDeleteDoc_Click(object sender, EventArgs e)
    {
        foreach (GridDataItem item in RadGrid_Documents.SelectedItems)
        {
            Label lblID = (Label)item.FindControl("lblId");
            HiddenField hdnTempId = (HiddenField)item.FindControl("hdnTempId");
            if (lblID.Text == "0")
            {
                DeleteDocFromTempTable(hdnTempId.Value);
            }

            DeleteFileFromFolder(string.Empty, Convert.ToInt32(lblID.Text));
        }

        //ScriptManager.RegisterStartupScript(this, GetType(), "DeleteDoc", "$('.dropify').dropify();", true);
    }

    /// <summary>
    /// Add Attachment
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lnkUploadDoc_Click(object sender, EventArgs e)
    {
        try
        {
            string filename = string.Empty;
            string fullpath = string.Empty;
            string mime = string.Empty;
            var savepath = string.Empty;

            var mainDirectory = "JEDocs";
            
            if (!string.IsNullOrEmpty(Request.QueryString["id"]))
            {
                if (Request.QueryString["r"] != null && Request.QueryString["r"].ToString() == "1")
                {
                    mainDirectory += "\\RE_" + Request.QueryString["id"];
                }
                else
                {
                    mainDirectory += "\\JE_" + Request.QueryString["id"];
                }
            }
            else
            {

                if (ViewState["TempUploadDirectory"] == null)
                {
                    ViewState["TempUploadDirectory"] = Guid.NewGuid().ToString("N");
                }

                mainDirectory += "\\" + ViewState["TempUploadDirectory"] as string;
            }

            savepath = GetUploadDirectory(mainDirectory);
            if (Request.QueryString["id"] != null)
            {
                if (Request.QueryString["r"] != null && Request.QueryString["r"].ToString() == "1")
                {
                    objMapData.Screen = "RE";
                }
                else
                {
                    objMapData.Screen = "JE";
                }

                objMapData.TicketID = Convert.ToInt32(Request.QueryString["id"].ToString());
                objMapData.TempId = "0";

                foreach (HttpPostedFile postedFile in FileUpload1.PostedFiles)
                {
                    filename = postedFile.FileName;
                    fullpath = savepath + filename;
                    mime = System.IO.Path.GetExtension(FileUpload1.PostedFile.FileName).Substring(1);

                    if (File.Exists(fullpath))
                    {
                        GeneralFunctions objGeneralFunctions = new GeneralFunctions();
                        filename = objGeneralFunctions.generateRandomString(4) + "_" + filename;
                        fullpath = savepath + filename;
                    }

                    using (new NetworkConnection())
                    {
                        if (!Directory.Exists(savepath))
                        {
                            Directory.CreateDirectory(savepath);
                        }

                        postedFile.SaveAs(fullpath);
                    }

                    objMapData.FileName = filename;
                    objMapData.DocTypeMIME = mime;
                    objMapData.FilePath = fullpath;

                    objMapData.DocID = 0;
                    objMapData.Mode = 0;
                    objMapData.ConnConfig = Session["config"].ToString();
                    objMapData.Worker = Session["User"].ToString();
                    objBL_MapData.AddFile(objMapData);
                }

                UpdateDocInfo();
                RadGrid_Documents.Rebind();
            }
            else
            {
                var tempTable = new DataTable();
                foreach (HttpPostedFile postedFile in FileUpload1.PostedFiles)
                {
                    filename = postedFile.FileName;
                    fullpath = savepath + filename;
                    mime = System.IO.Path.GetExtension(FileUpload1.PostedFile.FileName).Substring(1);

                    if (File.Exists(fullpath))
                    {
                        GeneralFunctions objGeneralFunctions = new GeneralFunctions();
                        filename = objGeneralFunctions.generateRandomString(4) + "_" + filename;
                        fullpath = savepath + filename;
                    }

                    using (new NetworkConnection())
                    {
                        if (!Directory.Exists(savepath))
                        {
                            Directory.CreateDirectory(savepath);
                        }

                        postedFile.SaveAs(fullpath);
                    }

                    tempTable = SaveAttachedFilesWhenAddingJE(filename, fullpath, mime);
                }

                RadGrid_Documents.DataSource = tempTable;
                RadGrid_Documents.VirtualItemCount = tempTable.Rows.Count;
                RadGrid_Documents.DataBind();
            }


            /*
            if (!string.IsNullOrEmpty(FileUpload1.FileName))
            {
                filename = FileUpload1.FileName;
                fullpath = savepath + filename;
                mime = System.IO.Path.GetExtension(FileUpload1.PostedFile.FileName).Substring(1);

                if (File.Exists(fullpath))
                {
                    GeneralFunctions objGeneralFunctions = new GeneralFunctions();
                    filename = objGeneralFunctions.generateRandomString(4) + "_" + filename;
                    fullpath = savepath + filename;
                }
                //var savepathconfig = System.Web.Configuration.WebConfigurationManager.AppSettings["POAttachmentPath"].Trim();
                //var credential = new System.Net.NetworkCredential();
                //credential.Password = "enclaveit@123";
                //credential.UserName = "Turlock";
                //var sss = new NetworkConnection(savepathconfig, credential);
                using (new NetworkConnection())
                {
                    if (!Directory.Exists(savepath))
                    {
                        Directory.CreateDirectory(savepath);
                    }

                    FileUpload1.SaveAs(fullpath);
                }
            }
            
            if (Request.QueryString["id"] != null)
            {
                if (Request.QueryString["r"] != null && Request.QueryString["r"].ToString() == "1")
                {
                    objMapData.Screen = "RE";
                }
                else
                {
                    objMapData.Screen = "JE";
                }
                
                objMapData.TicketID = Convert.ToInt32(Request.QueryString["id"].ToString());
                objMapData.TempId = "0";
                objMapData.FileName = filename;
                objMapData.DocTypeMIME = mime;
                objMapData.FilePath = fullpath;

                objMapData.DocID = 0;
                objMapData.Mode = 0;
                objMapData.ConnConfig = Session["config"].ToString();
                objMapData.Worker = Session["User"].ToString();
                objBL_MapData.AddFile(objMapData);
                UpdateDocInfo();
                //GetDocuments();
                RadGrid_Documents.Rebind();
                //RadGrid_gvLogs.Rebind();
            }
            else
            {
                var tempTable = SaveAttachedFilesWhenAddingJE(filename, fullpath, mime);
                RadGrid_Documents.DataSource = tempTable;
                RadGrid_Documents.VirtualItemCount = tempTable.Rows.Count;
                RadGrid_Documents.DataBind();
            }
            */
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyUploadErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    private string GetUploadDirectory(string mainDirectory)
    {
        var savepathconfig = System.Web.Configuration.WebConfigurationManager.AppSettings["AttachmentPath"].Trim();
        return savepathconfig + @"\" + Session["dbname"] + @"\" + mainDirectory + @"\";
    }

    private void UpdateDocInfo()
    {
        _objPropUser.ConnConfig = Session["config"].ToString();
        _objPropUser.dtDocs = SaveDocInfo();
        _objPropUser.Username = Session["User"].ToString();
        _objBLUser.UpdateDocInfo(_objPropUser);
    }

    private void GetDocuments()
    {
        if (Request.QueryString["id"] != null)
        {
            if (Request.QueryString["r"] != null && Request.QueryString["r"].ToString() == "1")
            {
                objMapData.Screen = "RE";
            }
            else
            {
                objMapData.Screen = "JE";
            }
            //objMapData.Screen = "JE";
            objMapData.TicketID = Convert.ToInt32(Request.QueryString["id"].ToString());
            objMapData.TempId = "0";
            objMapData.Mode = 1;
            objMapData.ConnConfig = Session["config"].ToString();
            DataSet ds = new DataSet();
            ds = objBL_MapData.GetDocuments(objMapData);
            //gvDocuments.DataSource = ds.Tables[0];
            //gvDocuments.DataBind();
            RadGrid_Documents.DataSource = ds.Tables[0];
            RadGrid_Documents.VirtualItemCount = ds.Tables[0].Rows.Count;
            //RadGrid_Documents.DataBind();
        }
        else
        {
            var source = ViewState["AttachedFiles"] as DataTable;
            pnlDocumentButtons.Visible = true;
            RadGrid_Documents.DataSource = source;
            RadGrid_Documents.VirtualItemCount = source != null ? source.Rows.Count : 0;
            //RadGrid_Documents.DataBind();
        }
    }

    private DataTable SaveDocInfo()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("ID", typeof(int));
        dt.Columns.Add("Portal", typeof(int));
        dt.Columns.Add("Remarks", typeof(string));
        dt.Columns.Add("MSVisible", typeof(byte));


        foreach (GridDataItem item in RadGrid_Documents.Items)
        {
            Label lblID = (Label)item.FindControl("lblID");
            TextBox txtRemarks = (TextBox)item.FindControl("txtRemarks");
            //CheckBox chkMSVisible = (CheckBox)item.FindControl("chkMSVisible");
            DataRow dr = dt.NewRow();
            dr["ID"] = lblID.Text;
            dr["Portal"] = false;//chkPortal.Checked;
            dr["Remarks"] = txtRemarks.Text;
            //dr["MSVisible"] = chkMSVisible.Checked;
            dr["MSVisible"] = false;
            dt.Rows.Add(dr);
        }

        return dt;
    }

    private void DeleteFile(int DocumentID)
    {
        try
        {
            objMapData.ConnConfig = Session["config"].ToString();
            objMapData.DocumentID = DocumentID;
            objMapData.Worker = Session["User"].ToString();
            objBL_MapData.DeleteFile(objMapData);
            UpdateDocInfo();
            //GetDocuments();
            RadGrid_Documents.Rebind();
            //adGrid_gvLogs.Rebind();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErrdelete", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : 3000,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    public void DeleteFileFromFolder(string StrFilename, int DocumentID)
    {
        try
        {
            DeleteFile(DocumentID);
        }
        catch (FileNotFoundException ex)
        {
            DeleteFile(DocumentID);
        }
        catch (UnauthorizedAccessException ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(),
            "FileDeleteAccessWarning", "noty({text: 'Please provide delete permissions to the file path.',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : 3000,theme : 'noty_theme_default',  closable : true});", true);
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);

            ScriptManager.RegisterStartupScript(this, GetType(),
            "FileDeleteErrorWarning", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : 3000,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    private void DeleteDocFromTempTable(string tempId)
    {
        if (string.IsNullOrWhiteSpace(tempId))
        {
            return;
        }

        var tempAttachedFiles = ViewState["AttachedFiles"] as DataTable;

        if (tempAttachedFiles == null)
        {
            return;
        }

        var deleteFileRow = tempAttachedFiles.AsEnumerable().FirstOrDefault(t => t.Field<string>("TempId") == tempId);

        if (deleteFileRow != null)
        {
            tempAttachedFiles.Rows.Remove(deleteFileRow);
        }
    }

    private DataTable SaveAttachedFilesWhenAddingJE(string fileName, string fullPath, string doctype)
    {
        var tempAttachedFiles = ViewState["AttachedFiles"] as DataTable;

        if (tempAttachedFiles == null)
        {
            tempAttachedFiles = new DataTable();
            tempAttachedFiles.Columns.Add("id", typeof(int));
            tempAttachedFiles.Columns.Add("filename", typeof(string));
            tempAttachedFiles.Columns.Add("doctype", typeof(string));
            tempAttachedFiles.Columns.Add("Portal", typeof(bool));
            tempAttachedFiles.Columns.Add("Path", typeof(string));
            tempAttachedFiles.Columns.Add("remarks", typeof(string));
            tempAttachedFiles.Columns.Add("MSVisible", typeof(byte));
            tempAttachedFiles.Columns.Add("TempId", typeof(string));
            ViewState["AttachedFiles"] = tempAttachedFiles;
        }

        var row = tempAttachedFiles.NewRow();
        row["id"] = 0;
        row["filename"] = fileName;
        row["doctype"] = doctype;
        row["Portal"] = false;
        row["Path"] = fullPath;
        row["remarks"] = string.Empty;
        row["MSVisible"] = false;
        row["TempId"] = Guid.NewGuid().ToString("N");
        tempAttachedFiles.Rows.Add(row);
        return tempAttachedFiles;
    }

    protected void lblName_Click(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)sender;

        string[] CommandArgument = btn.CommandArgument.Split(',');

        string FileName = CommandArgument[0];

        string FilePath = CommandArgument[1];

        DownloadDocument(FilePath, FileName);
    }

    private void DownloadDocument(string filePath, string DownloadFileName)
    {
        try
        {
            using (new NetworkConnection())
            {
                System.IO.FileInfo FileName = new System.IO.FileInfo(filePath);
                FileStream myFile = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                BinaryReader _BinaryReader = new BinaryReader(myFile);

                try
                {
                    long startBytes = 0;
                    string lastUpdateTiemStamp = File.GetLastWriteTimeUtc(filePath).ToString("r");
                    string _EncodedData = HttpUtility.UrlEncode(DownloadFileName, System.Text.Encoding.UTF8) + lastUpdateTiemStamp;

                    Response.Clear();
                    Response.Buffer = false;
                    Response.AddHeader("Accept-Ranges", "bytes");
                    Response.AppendHeader("ETag", "\"" + _EncodedData + "\"");
                    Response.AppendHeader("Last-Modified", lastUpdateTiemStamp);
                    Response.ContentType = "application/octet-stream";
                    Response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(DownloadFileName));
                    Response.AddHeader("Content-Length", (FileName.Length - startBytes).ToString());
                    Response.AddHeader("Connection", "Keep-Alive");
                    Response.ContentEncoding = System.Text.Encoding.UTF8;

                    //Send data
                    _BinaryReader.BaseStream.Seek(startBytes, SeekOrigin.Begin);

                    //Dividing the data in 1024 bytes package
                    int maxCount = (int)Math.Ceiling((FileName.Length - startBytes + 0.0) / 1024);

                    //Download in block of 1024 bytes
                    int i;
                    for (i = 0; i < maxCount && Response.IsClientConnected; i++)
                    {
                        Response.BinaryWrite(_BinaryReader.ReadBytes(1024));
                        Response.Flush();
                    }
                    ////if blocks transfered not equals total number of blocks
                    //if (i < maxCount)
                    //    return false;
                    //return true; 
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    Response.End();
                    _BinaryReader.Close();
                    myFile.Close();
                }
            }
        }
        catch (FileNotFoundException ex)
        {
            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(),
            "FileaccessWarning", "alert('File not found.');", true);
        }
        catch (UnauthorizedAccessException ex)
        {
            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(),
            "FileaccessWarning", "alert('Please provide access permissions to the file path.');", true);
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);

            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(),
            "FileerrorWarning", "alert('" + str + "');", true);
        }
    }

    private void DocumentPermission()
    {

        if (Convert.ToString(Session["type"]) != "am" && Convert.ToString(Session["type"]) != "c")
        {
            DataTable ds = new DataTable();
            ds = (DataTable)Session["userinfo"];

            //Document--------------------->

            string DocumentPermission = ds.Rows[0]["DocumentPermission"] == DBNull.Value ? "YYYY" : ds.Rows[0]["DocumentPermission"].ToString();
            hdnAddeDocument.Value = DocumentPermission.Length < 1 ? "Y" : DocumentPermission.Substring(0, 1);
            hdnEditDocument.Value = DocumentPermission.Length < 2 ? "Y" : DocumentPermission.Substring(1, 1);
            hdnDeleteDocument.Value = DocumentPermission.Length < 3 ? "Y" : DocumentPermission.Substring(2, 1);
            hdnViewDocument.Value = DocumentPermission.Length < 4 ? "Y" : DocumentPermission.Substring(3, 1);

            if (hdnDeleteDocument.Value == "N")
            {
                lnkDeleteDoc.Enabled = false;
            }
            else
            {
                lnkDeleteDoc.Enabled = true;
            }

            if (hdnAddeDocument.Value == "N")
            {
                lnkUploadDoc.Enabled = false;
            }
            else
            {
                lnkUploadDoc.Enabled = true;
            }
            pnlDocPermission.Visible = hdnViewDocument.Value == "N" ? false : true;
        }
    }

    private void RowSelectDocuments()
    {
        if (hdnEditDocument.Value == "N")
        {
            foreach (GridDataItem item in RadGrid_Documents.Items)
            {
                //CheckBox chkSelected = (CheckBox)item.FindControl("chkSelect");
                //CheckBox chkPortal = (CheckBox)item.FindControl("chkPortal");
                TextBox txtremarks = (TextBox)item.FindControl("txtremarks");
                //chkSelected.Enabled = 
                //chkPortal.Enabled = false;
                txtremarks.Enabled = false;
                item.Attributes["ondblclick"] = "   noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false,dismissQueue:true });";
            }
        }
    }

    protected void RadGrid_Documents_PreRender(object sender, EventArgs e)
    {
        RowSelectDocuments();
    }

    protected void RadGrid_Documents_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        GetDocuments();

    }

    private void UpdateTempDateWhenCreatingNewJE(string strJEId)
    {
        var JEId = Convert.ToInt32(strJEId);
        if (ViewState["TempUploadDirectory"] == null)
        {
            return;
        }
        var tempAttachedFiles = ViewState["AttachedFiles"] as DataTable;
        var tempDirectory = "JEDocs\\" + ViewState["TempUploadDirectory"] as string;
        var newDirectory = "JEDocs\\";
        // "JEDocs\\" + "JE_" + strJEId;
        if (Request.QueryString["r"] != null && Request.QueryString["r"].ToString() == "1")
        {
            newDirectory += "RE_" + strJEId;
        }
        else
        {
            newDirectory += "JE_" + strJEId;
        }

        if (tempAttachedFiles == null)
        {
            return;
        }

        var sourceDirectory = GetUploadDirectory(tempDirectory);
        var destDirectory = GetUploadDirectory(newDirectory);
        Directory.Move(sourceDirectory, destDirectory);

        if (Request.QueryString["r"] != null && Request.QueryString["r"].ToString() == "1")
        {
            objMapData.Screen = "RE";
        }
        else
        {
            objMapData.Screen = "JE";
        }
        foreach (DataRow row in tempAttachedFiles.Rows)
        {
            //objMapData.Screen = "JE";
            objMapData.TicketID = JEId;
            objMapData.TempId = "0";
            objMapData.FileName = row.Field<string>("filename");
            objMapData.DocTypeMIME = row.Field<string>("doctype");
            objMapData.FilePath = row.Field<string>("Path").Replace(sourceDirectory, destDirectory);
            //objMapData.FilePath = row.Field<string>("Path");
            objMapData.DocID = 0;
            objMapData.Mode = 0;
            objMapData.ConnConfig = Session["config"].ToString();
            objMapData.Worker = Session["User"].ToString();
            objBL_MapData.AddFile(objMapData);
        }

        ViewState["TempUploadDirectory"] = null;
        ViewState["AttachedFiles"] = null;


        //get document     
        //objMapData.Screen = "JE";
        if (Request.QueryString["r"] != null && Request.QueryString["r"].ToString() == "1")
        {
            objMapData.Screen = "RE";
        }
        else
        {
            objMapData.Screen = "JE";
        }
        objMapData.TicketID = JEId;
        objMapData.TempId = "0";
        objMapData.Mode = 1;
        objMapData.ConnConfig = Session["config"].ToString();
        var ds = objBL_MapData.GetDocuments(objMapData);
        var saveDocsRows = ds.Tables[0].AsEnumerable();

        foreach (GridDataItem item in RadGrid_Documents.Items)
        {
            Label lblID = (Label)item.FindControl("lblID");
            HiddenField hdnTempId = (HiddenField)item.FindControl("hdnTempId");
            LinkButton lblName = (LinkButton)item.FindControl("lblName");

            var docRow = saveDocsRows.FirstOrDefault(t => t.Field<string>("Filename") == lblName.Text);
            if (docRow != null)
            {
                lblID.Text = docRow.Field<int>("ID").ToString();
            }

        }
    }

    #endregion
}
