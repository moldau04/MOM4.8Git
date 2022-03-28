using BusinessEntity;
using BusinessLayer;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class AddCOA : System.Web.UI.Page
{
    #region "Variables"

    Chart _objChart = new Chart();
    BL_Chart _objBLChart = new BL_Chart();

    AccountType _objAcType = new AccountType();
    Central _objCentral = new Central();
    BL_AccountType _objBLAcType = new BL_AccountType();
    MapData objMapData = new MapData();
    BL_MapData objBL_MapData = new BL_MapData();

    Bank _objBank = new Bank();
    Rol _objRol = new Rol();
    BL_BankAccount _objBLBank = new BL_BankAccount();

    User _objPropUser = new User();
    BL_User _objBLUser = new BL_User();

    BusinessEntity.CompanyOffice objCompany = new BusinessEntity.CompanyOffice();
    BL_Company objBL_Company = new BL_Company();
    General _objPropGeneral = new General();
    BL_General _objBLGeneral = new BL_General();
    #endregion

    #region "events"

    #region PAGELOAD
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["userid"] == null)
            {
                Response.Redirect("login.aspx", false);
            }
            string _connectionString = Session["config"].ToString();

            if (!IsPostBack)
            {
                userpermissions();
                CompanyPermission();
                _objChart.ConnConfig = _connectionString;
                pnlBankAccount2.Visible = false;
                FillAccountType();
                FillStatus();

                FillState();        //for Bank account panel
                txtBal.ReadOnly = true;
                txtReconciled.ReadOnly = true;
                txtBal.Text = "0.00";
                txtReconciled.Text = "0.00";
                txtRate.Text = "0.00";
                pnlBankAccount.Visible = false;

                //pnlBankInfo.Visible = false;
                liGeneral.Style["display"] = "none";
                adGeneral.Style["display"] = "none";

                //pnlBankAccount2.Visible = false;
                if (Request.QueryString["c"] != null)       //Copy COA
                {
                    if (Request.QueryString["id"] != null)
                    {
                        SetDataForEdit();
                        ddlCompany.Visible = false;
                        txtCompany.Visible = true;
                        btnCompanyPopUp.Visible = true;
                        lblAcctType.Text = ddlType.SelectedItem.Text;
                        lblAcctNum.Text = txtAcctNum.Text;
                        lblAcctName.Text = txtAcName.Text;
                    }
                }
                else if (Request.QueryString["id"] != null)  //Edit COA
                {
                    SetDataForEdit();
                    ddlCompany.Visible = false;
                    txtCompany.Visible = true;
                    btnCompanyPopUp.Visible = true;
                    lblAcctType.Text = ddlType.SelectedItem.Text;
                    lblAcctNum.Text = txtAcctNum.Text;
                    lblAcctName.Text = txtAcName.Text;
                    pnlNext.Visible = true;
                }
                else                                        //Add COA   
                {
                    //lblAddEditUser.Text = "Add New Account ";
                    ViewState["mode"] = 1;
                    lblHeader.Text = "Add New Account ";
                    FillSubAccount();
                    ddlCompany.Visible = true;
                    txtCompany.Visible = false;
                    btnCompanyPopUp.Visible = false;
                    //ddlSubAcCategory.Enabled = false;

                }
                FillCentral();
                txtBal.Enabled = false;
                Permission();
                HighlightSideMenu("financeMgr", "lnkCOA", "financeMgrSub");
            }
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
    private void Permission()
    {
        //AjaxControlToolkit.HoverMenuExtender hm = (AjaxControlToolkit.HoverMenuExtender)Page.Master.FindControl("HoverMenuExtenderCstm");
        //hm.Enabled = false;
        //HtmlGenericControl ul = (HtmlGenericControl)Page.Master.FindControl("cstmMgrSub");
        //ul.Style.Add("display", "block");
        //ul.Style.Add("visibility", "visible");

        if (Session["type"].ToString() == "c")
        {
            Response.Redirect("home.aspx", false);
            //Response.Redirect("addcustomer.aspx?uid=" + Session["userid"].ToString());
        }

        if (Session["MSM"].ToString() == "TS")
        {
            Response.Redirect("home.aspx", false);
            //pnlGridButtons.Visible = false;
        }
        if (Convert.ToInt32(Session["ISsupervisor"]) == 1)
        {
            Response.Redirect("home.aspx", false);
        }

        //DataTable dt = new DataTable();
        //dt = (DataTable)Session["userinfo"];

        //string ProgFunc = dt.Rows[0]["Control"].ToString().Substring(0, 1);
        //if (ProgFunc == "N")
        //{
        //    Response.Redirect("home.aspx");
        //}
    }

    private void CompanyPermission()
    {
        if (Session["COPer"].ToString() == "1")
        {
            ViewState["CompPermission"] = 1;
            dvCompanyPermission.Visible = true;
            FillCompany();
        }
        else
        {
            ViewState["CompPermission"] = 0;
            dvCompanyPermission.Visible = false;
        }
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
            ddlCompany.Items.Insert(0, new ListItem("Select", "0"));

            ddlCompanyEdit.DataSource = ds.Tables[0];
            ddlCompanyEdit.DataTextField = "Name";
            ddlCompanyEdit.DataValueField = "CompanyID";
            ddlCompanyEdit.DataBind();
            ddlCompanyEdit.Items.Insert(0, new ListItem("Select", "0"));

        }
    }
    #endregion

    #region Close COA
    protected void lnkClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("chartofaccount.aspx", false);
    }
    #endregion

    #region Save COA
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        Submit();
    }

    private void Submit()
    {
        int coaid = 0;
        try
        {
            if (Page.IsValid)
            {



                _objChart.ConnConfig = Session["config"].ToString();
                _objChart.Acct = txtAcctNum.Text;
                _objChart.fDesc = txtAcName.Text;
                if (ddlCentral.SelectedValue != "")
                {
                    _objChart.Department = Convert.ToInt32(ddlCentral.SelectedValue);
                }
                else
                {
                    _objChart.Department = null;
                }

                _objChart.AcType = Convert.ToInt32(ddlType.SelectedValue);

                if (ddlSubAcCategory.SelectedValue != " Select Sub Category ")
                    _objChart.Sub = ddlSubAcCategory.SelectedItem.Text;
                else
                    _objChart.Sub = "";

                _objChart.Sub2 = "";
                _objChart.Remarks = txtDescription.Text;
                _objChart.InUse = 1;
                _objChart.Detail = 0;
                _objChart.Status = Convert.ToInt32(ddlStatus.SelectedIndex);
                //_objChart.ConnConfig = Session["config"].ToString();
                //_objChart.Balance = Convert.ToDouble(0);
                //_objRol.Name = txtAcName.Text;
                _objChart.Contact = txtContact.Text;
                _objChart.Address = txtAddress.Text;
                _objChart.Phone = txtPhone.Text;
                _objChart.Fax = txtFax.Text;
                _objChart.City = txtCity.Text;
                _objChart.Cellular = txtCellular.Text;
                _objChart.BankName = txtBankName.Text;
                _objChart.Lat = lat.Text;
                _objChart.Long = lng.Text;

                //_objRol.State = txtState.Text;
                //if (!ddlState.SelectedValue.Equals("Select State"))
                //{
                //    _objChart.State = ddlState.SelectedValue;
                //}
                _objChart.State = ddlState.Text;
                _objChart.Zip = txtZip.Text;
                _objChart.EMail = txtEmail.Text;
                // _objChart.Country = txtCountry.Text;
                _objChart.Country = ddlCountry.SelectedValue;
                _objChart.Website = txtWebsite.Text;
                //_objChart.Type = 2; // Bank Type number

                //_objBank.fDesc = txtAcName.Text;
                _objChart.NBranch = txtBranch.Text;
                _objChart.NAcct = txtAcct.Text;
                _objChart.NRoute = txtRoute.Text;
                if (!string.IsNullOrEmpty(txtCreditLimit.Text))
                    _objChart.CLimit = Convert.ToDouble(txtCreditLimit.Text);

                if (!string.IsNullOrEmpty(txtNCheck.Text))
                    _objChart.NextC = Convert.ToInt32(txtNCheck.Text);
                if (!string.IsNullOrEmpty(txtNDeposit.Text))
                    _objChart.NextD = Convert.ToInt32(txtNDeposit.Text);
                if (!string.IsNullOrEmpty(txtNEPay.Text))
                    _objChart.NextE = Convert.ToInt32(txtNEPay.Text);
                if (!string.IsNullOrEmpty(txtRate.Text))
                    _objChart.Rate = Convert.ToDouble(txtRate.Text);
                if (!string.IsNullOrEmpty(ddlStatus.SelectedValue))
                    _objChart.Status = Convert.ToInt32(ddlStatus.SelectedValue);
                if (chkWarn.Checked == true)
                    _objChart.Warn = 1;
                else
                    _objChart.Warn = 0;
                if (chkNoJE.Checked == true)
                    _objChart.NoJE = 1;
                else
                    _objChart.NoJE = 0;
                _objBank.ACHBatchControlString1 = txtACHBatchControlString1.Text;
                _objBank.ACHBatchControlString2 = txtACHBatchControlString2.Text;
                _objBank.ACHBatchControlString3 = txtACHBatchControlString3.Text;
                _objBank.ACHCompanyHeaderString1 = txtACHCompanyHeaderString1.Text;
                _objBank.ACHCompanyHeaderString2 = txtACHCompanyHeaderString2.Text;
                _objBank.ACHFileControlString1 = txtACHFileControlString1.Text;
                _objBank.ACHFileHeaderStringA = txtACHFileHeaderStringA.Text;
                _objBank.ACHFileHeaderStringB = txtACHFileHeaderStringB.Text;
                _objBank.ACHFileHeaderStringC = txtACHFileHeaderStringC.Text;
                _objBank.APACHCompanyID = txtAPACHCompanyID.Text;
                _objBank.APImmediateOrigin = txtAPImmediateOrigin.Text;
                if (txtNextACH.Text != null && txtNextACH.Text.Trim() != "")
                {
                    _objBank.NextACH = txtNextACH.Text.Trim();
                }
                else
                {
                    _objBank.NextACH = "0";
                }
                _objBank.TraceNo1 = txtTraceNo1.Text;
                _objBank.TraceNo2 = txtTraceNo2.Text;
                _objBank.RecordTypeCode1 = txtRecordTypeCode1.Text;
                _objBank.RecordTypeCode2 = txtRecordTypeCode2.Text;
                _objBank.TransactionCode1 = txtTransactionCode1.Text;
                _objBank.TransactionCode2 = txtTransactionCode2.Text;
                _objBank.EndRecordIndicator1 = txtEndRecordIndicator1.Text;
                _objBank.EndRecordIndicator2 = txtEndRecordIndicator2.Text;
                _objBank.OriginatorStatusCode = txtOriginatorStatusCode.Text;
                _objBank.RecordTypeCode3 = txtRecordTypeCode3.Text;
                _objBank.BatchNumber = txtBatchNumber.Text;
                _objBank.JulianDate = txtJulianDate.Text;

                if (Request.QueryString["c"] != null)   // COPY
                {
                    #region "COPY"
                    if (Request.QueryString["id"] != null)
                    {
                        if (Convert.ToInt32(ViewState["CompPermission"]) == 1)
                            _objChart.EN = Convert.ToInt32(ddlCompany.SelectedValue);
                        else
                            _objChart.EN = 0;

                        //var dsChart=
                            _objBLChart.AddChart(_objChart, _objBank);
                        //Update  Attachment Doc INFO                 
                        //UpdateTempDateWhenCreatingNewJE(jeRef.ToString());
                        //17-11-21
                        coaid = _objBLChart.GetCharID(_objChart);

                        //Update  Attachment Doc INFO 
                        UpdateTempDateWhenCreatingNewCOA(coaid);
                        UpdateDocInfo();
                        

                        ClientScript.RegisterStartupScript(Page.GetType(), "keySucc", "noty({text: 'Chart of Account Added Successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);

                        if (ddlType.SelectedItem.Text.Equals("Bank"))
                        {
                            ScriptManager.RegisterStartupScript(this, Page.GetType(), "resize controls", "ResizeControls('true');", true);
                        }
                    }
                    #endregion
                }
                else if (Request.QueryString["id"] != null)   // EDIT
                {
                    _objChart.ID = Convert.ToInt32(Request.QueryString["id"]);
                    if (ddlType.SelectedItem.Text == "Bank")
                    {
                        if (!string.IsNullOrEmpty(hdnRol.Value))
                        {
                            _objChart.Rol = Convert.ToInt32(hdnRol.Value);
                        }
                        if (!string.IsNullOrEmpty(hdnBank.Value))
                        {
                            _objChart.Bank = Convert.ToInt32(hdnBank.Value);
                        }
                    }
                    if (Convert.ToInt32(ViewState["CompPermission"]) == 1)
                        _objChart.EN = Convert.ToInt32(ddlCompanyEdit.SelectedValue);
                    else
                        _objChart.EN = 0;
                    //UpdateChart();
                    UpdateDocInfo();

                    _objPropGeneral.ConnConfig = Session["config"].ToString();
                    DataSet _dsCustom = _objBLGeneral.getCustomFieldsControl(_objPropGeneral);
                    if (_dsCustom.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow _dr in _dsCustom.Tables[0].Rows)
                        {
                            if (_dr["Name"].ToString().Equals("DefaultInvGLAcct"))
                            {
                                if (!string.IsNullOrEmpty(_dr["Label"].ToString()))
                                {
                                    if (Convert.ToInt32(_dr["Label"].ToString()) == Convert.ToInt32(Request.QueryString["id"]))
                                    {
                                        if (Convert.ToInt32(ddlStatus.SelectedIndex) == 1)
                                        {
                                            ClientScript.RegisterStartupScript(Page.GetType(), "keySuccc", "noty({text: 'This account is already in use, cannot be made inactive.',  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                                            return;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    /* ES-3855 */
                    DataSet _cDataset = new DataSet();
                    _cDataset = _objBLChart.GetChartDetail(_objChart);
                    if (_cDataset.Tables[0].Rows.Count > 0)
                    {
                        if (Convert.ToInt32(_cDataset.Tables[0].Rows[0]["JobInAcct"]) > 0)
                        {
                            if (Convert.ToInt32(ddlStatus.SelectedIndex) == 1)
                            {
                                ClientScript.RegisterStartupScript(Page.GetType(), "keySuccc", "noty({text: 'This account is already in use, cannot be made inactive.',  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                                return;
                            }
                        }

                        if (Convert.ToInt32(_cDataset.Tables[0].Rows[0]["TransInAcct"]) > 0)
                        {
                            if (Convert.ToInt32(ddlType.SelectedValue) != Convert.ToInt32(_cDataset.Tables[0].Rows[0]["Type"]))
                            {
                                ClientScript.RegisterStartupScript(Page.GetType(), "keySuccc", "noty({text: 'This account has transactions you can not change account type.',  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                                return;
                            }
                        }

                    }
                    /* ES-3855  */                   

                    UpdateDocInfo();
                    _objBLChart.UpdateChart(_objChart, _objBank);
                    ClientScript.RegisterStartupScript(Page.GetType(), "keySucc", "noty({text: 'Chart of Account Updated Successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);

                    if (ddlType.SelectedItem.Text.Equals("Bank"))
                    {
                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "resize controls", "ResizeControls('true');", true);
                    }
                }
                else   // ADD
                {
                    
                    if (Convert.ToInt32(ViewState["CompPermission"]) == 1)
                        _objChart.EN = Convert.ToInt32(ddlCompany.SelectedValue);
                    else
                        _objChart.EN = 0;
                    _objBLChart.AddChart(_objChart, _objBank);

                    //17-11-21
                    coaid = _objBLChart.GetCharID(_objChart);
                   
                    //Update  Attachment Doc INFO 
                    UpdateTempDateWhenCreatingNewCOA(coaid);
                    UpdateDocInfo();

                    ResetFormControlValues(this);
                    ClientScript.RegisterStartupScript(Page.GetType(), "keySucc", "noty({text: 'Chart of Account Added Successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);

                    pnlBankAccount.Visible = false;
                    pnlBankAccount2.Visible = false;
                    //pnlBankInfo.Visible = false;
                    liGeneral.Style["display"] = "none";
                    adGeneral.Style["display"] = "none";
                }
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true})", true);
        }
    }
    #endregion
    private void SetBankData(DataRow _drBank, DataRow _drRol)
    {
        try
        {
            hdnRol.Value = _drRol["ID"].ToString();
            // txtAcName.Text = _drRol["Name"].ToString();
            txtBankName.Text = _drRol["Name"].ToString();
            lat.Text = Convert.ToString(_drRol["Lat"]);
            lng.Text = Convert.ToString(_drRol["Lng"]);
            txtContact.Text = _drRol["Contact"].ToString();
            txtAddress.Text = _drRol["Address"].ToString();
            txtPhone.Text = _drRol["Phone"].ToString();
            txtFax.Text = _drRol["Fax"].ToString();
            txtCity.Text = _drRol["City"].ToString();
            txtCellular.Text = _drRol["Cellular"].ToString();
            ddlState.Text = _drRol["State"].ToString();
            txtZip.Text = _drRol["Zip"].ToString();
            txtEmail.Text = _drRol["EMail"].ToString();
            //txtCountry.Text = _drRol["Country"].ToString();
            ddlCountry.SelectedValue = Convert.ToString(_drRol["Country"]);
            txtWebsite.Text = _drRol["Website"].ToString();

            hdnBank.Value = _drBank["ID"].ToString();
            txtAcName.Text = _drBank["fDesc"].ToString();

            txtBranch.Text = _drBank["NBranch"].ToString();
            txtAcct.Text = _drBank["NAcct"].ToString();
            txtRoute.Text = _drBank["NRoute"].ToString();
            txtCreditLimit.Text = _drBank["CLimit"].ToString();

            if (_drBank["NextC"] != null)
                txtNCheck.Text = _drBank["NextC"].ToString();
            if (_drBank["NextD"] != null)
                txtNDeposit.Text = _drBank["NextD"].ToString();
            if (_drBank["NextE"] != null)
                txtNEPay.Text = _drBank["NextE"].ToString();

            txtRate.Text = _drBank["Rate"].ToString();
            if (!string.IsNullOrEmpty(ddlStatus.SelectedValue))
                ddlStatus.SelectedValue = _drBank["Status"].ToString();
            if (Convert.ToInt16(_drBank["Warn"]).Equals(1))
                chkWarn.Checked = true;
            else
                chkWarn.Checked = false;

            if (Request.QueryString["c"] != null)
            {
                if (Request.QueryString["c"].ToString().Equals("1"))
                {
                    txtCreditLimit.Text = "";
                    txtNCheck.Text = "";
                    txtNDeposit.Text = "";
                    txtNEPay.Text = "";
                }
            }


            txtACHBatchControlString1.Text = _drBank["ACHBatchControlString1"].ToString();
            txtACHBatchControlString2.Text = _drBank["ACHBatchControlString2"].ToString();
            txtACHBatchControlString3.Text = _drBank["ACHBatchControlString3"].ToString();

            txtACHCompanyHeaderString1.Text = _drBank["ACHCompanyHeaderString1"].ToString();
            txtACHCompanyHeaderString2.Text = _drBank["ACHCompanyHeaderString2"].ToString();
            txtACHFileControlString1.Text = _drBank["ACHFileControlString1"].ToString();
            txtACHFileHeaderStringA.Text = _drBank["ACHFileHeaderStringA"].ToString();
            txtACHFileHeaderStringB.Text = _drBank["ACHFileHeaderStringB"].ToString();
            txtACHFileHeaderStringC.Text = _drBank["ACHFileHeaderStringC"].ToString();

            txtAPACHCompanyID.Text = _drBank["APACHCompanyID"].ToString();
            txtAPImmediateOrigin.Text = _drBank["APImmediateOrigin"].ToString();
            txtNextACH.Text = _drBank["NextACH"].ToString();

            txtTraceNo1.Text = _drBank["TraceNo1"].ToString();
            txtTraceNo2.Text = _drBank["TraceNo2"].ToString();
            txtRecordTypeCode1.Text = _drBank["RecordTypeCode1"].ToString();
            txtRecordTypeCode2.Text = _drBank["RecordTypeCode2"].ToString();
            txtTransactionCode1.Text = _drBank["TransactionCode1"].ToString();
            txtTransactionCode2.Text = _drBank["TransactionCode2"].ToString();

            txtEndRecordIndicator1.Text = _drBank["EndRecordIndicator1"].ToString();
            txtEndRecordIndicator2.Text = _drBank["EndRecordIndicator2"].ToString();
            txtOriginatorStatusCode.Text = _drBank["OriginatorStatusCode"].ToString();
            txtRecordTypeCode3.Text = _drBank["RecordTypeCode3"].ToString();
            txtBatchNumber.Text = _drBank["BatchNumber"].ToString();
            txtJulianDate.Text = _drBank["JulianDate"].ToString();


        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    protected void ddlSubAcCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlSubAcCategory.SelectedValue == "0")
        {
            ddlSubAcCategory.SelectedIndex = -1;

            string script = "function f(){$find(\"" + RadWindowSubCategory.ClientID + "\").show(); Sys.Application.remove_load(f);}Sys.Application.add_load(f); Materialize.updateTextFields();";
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);

            ScriptManager.RegisterStartupScript(this, GetType(), "SetSubCategory", "SetSubCategoryData();", true);
        }
    }
    protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FillSubAccount();
            lblAcctType.Text = ddlType.SelectedItem.Text;
            if (ddlType.SelectedItem.Text.Equals("Bank"))
            {
                pnlBankAccount.Visible = true;
                //pnlBankInfo.Visible = true;
                liGeneral.Style["display"] = "inline-block";
                adGeneral.Style["display"] = "block";
                pnlBankAccount2.Visible = true;
                chkWarn.Checked = true;
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "resize controls", "ResizeControls('true');", true);
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "resize Textbox", "reSizeTextbox();", true);
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "updateTextField", "updateTextField();", true);

                #region "EDIT COA"
                if (Request.QueryString["id"] != null)  // In case, if bank details are previously added by user then one can easy 
                {                                       // fetch those details from database by changing his/her Account type to "Bank" again.
                    DataSet _dsIsExist = new DataSet();
                    _objBank.ConnConfig = Session["config"].ToString();
                    _objBank.Chart = Convert.ToInt32(Request.QueryString["id"]);
                    _dsIsExist = _objBLBank.IsExistBankAcct(_objBank);  //check if user have previously added Bank details.
                    int count = 0;
                    if (_dsIsExist.Tables[0].Columns.Contains("CBANK"))
                    {
                        count = Convert.ToInt32(_dsIsExist.Tables[0].Rows[0]["CBANK"]);
                        if (count > 0)
                        {
                            _objBank.ConnConfig = Session["config"].ToString();
                            _objBank.Chart = Convert.ToInt32(Request.QueryString["id"]);
                            DataSet _dsBank = new DataSet();
                            _dsBank = _objBLBank.GetBankByChart(_objBank);      // Get Bank details by Chart ID
                            DataRow _drBank = _dsBank.Tables[0].Rows[0];
                            if (_dsBank != null)
                            {
                                DataSet _dsRol = new DataSet();
                                _objRol.ConnConfig = Session["config"].ToString();
                                _objRol.ID = Convert.ToInt32(_dsBank.Tables[0].Rows[0]["Rol"]);
                                _dsRol = _objBLBank.GetRolByID(_objRol);        // Get Rol details by Rol ID
                                DataRow _drRol = _dsRol.Tables[0].Rows[0];
                                SetBankData(_drBank, _drRol);
                            }
                        }
                    }
                }
                #endregion
            }
            else
            {
                pnlBankAccount.Visible = false;
                pnlBankAccount2.Visible = false;
                //pnlBankInfo.Visible = false;
                liGeneral.Style["display"] = "none";
                adGeneral.Style["display"] = "none";
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "resize controls", "ResizeControls('false');", true);
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true})", true);
        }
    }

    #region Add Sub Account Detail

    protected void lbtnSubAcctSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            GetSubAcctData();
            _objBLAcType.AddSubAccount(_objAcType);

            FillSubAccount();
            ddlSubAcCategory.Items.FindByText(txtSubAcct.Text).Selected = true;
            txtSubAcct.Text = "";
            //mpeAddSubAccount.Hide();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    protected void lbtnCentralSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            _objCentral.ConnConfig = Session["config"].ToString();
            _objCentral.CentralName = txtCentral.Text;
            if (!String.IsNullOrEmpty(txtCentral.Text))
            {
                DataSet DS = _objBLAcType.AddCentral(_objCentral);
                ddlCentral.Items.Clear();
                if (DS.Tables[0].Rows.Count > 0)
                {
                    ddlCentral.Items.Insert(0, new ListItem(" Select Center ", ""));
                    ddlCentral.AppendDataBoundItems = true;
                    ddlCentral.DataSource = DS.Tables[0];
                    ddlCentral.DataValueField = "ID";
                    ddlCentral.DataTextField = "CentralName";
                    ddlCentral.DataBind();
                }
                else
                {
                    ddlCentral.Items.Insert(0, new ListItem(" No Center Available ", ""));
                }

                ddlCentral.Items.FindByText(txtCentral.Text).Selected = true;
                txtCentral.Text = "";
            }
            else
            {
                ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: 'Please enter Center',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    #endregion
    protected void lnkPrevious_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = (DataTable)Session["Chart"];
            DataColumn[] keyColumns = new DataColumn[1];
            keyColumns[0] = dt.Columns["ID"];
            //dt.PrimaryKey = keyColumns;

            DataRow d = dt.Select("ID=" + Request.QueryString["id"].ToString()).FirstOrDefault();
            int index = dt.Rows.IndexOf(d);

            if (index > 0)
            {
                Response.Redirect("addcoa.aspx?id=" + dt.Rows[index - 1]["ID"], false);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void lnkNext_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = (DataTable)Session["Chart"];
            DataColumn[] keyColumns = new DataColumn[1];
            keyColumns[0] = dt.Columns["ID"];
            //dt.PrimaryKey = keyColumns;

            DataRow d = dt.Select("ID=" + Request.QueryString["id"].ToString()).FirstOrDefault();
            int index = dt.Rows.IndexOf(d);
            int c = dt.Rows.Count - 1;

            if (index < c)
            {
                Response.Redirect("addcoa.aspx?id=" + dt.Rows[index + 1]["ID"], false);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void lnkLast_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = (DataTable)Session["Chart"];
            Response.Redirect("addcoa.aspx?id=" + dt.Rows[dt.Rows.Count - 1]["ID"], false);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void lnkFirst_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = (DataTable)Session["Chart"];
            Response.Redirect("addcoa.aspx?id=" + dt.Rows[0]["ID"], false);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void cvAccountNum_ServerValidate(object source, ServerValidateEventArgs args)
    {
        try
        {
            bool IsExists = false;
            _objChart.ConnConfig = Session["config"].ToString();
            _objChart.Acct = txtAcctNum.Text;

            //int _count = Convert.ToInt32(_dsIsAcctExit.Tables[0].Rows[0]["CountAcct"]);

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
                _objChart.SearchValue = ddlType.SelectedItem.Text;

                IsAcNameExists = _objBLChart.IsExistAcctORBANKNameForEdit(_objChart);
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
    #endregion

    #region Custom Functions
    private void FillStatus()
    {
        try
        {
            DataSet ds = new DataSet();
            ds = _objBLChart.GetAllStatus(_objChart);
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

                //_dsSubType.Tables[0] = _dsSubType.Tables[0].DefaultView.ToTable();
                if (_dsSubType.Tables.Count > 0)
                {
                    ddlSubAcCategory.Items.Add(new ListItem(" Select Sub Category "));
                    ddlSubAcCategory.Items.Add(new ListItem(" < Add New > ", "0"));
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
    private void ResetFormControlValues(Control parent)
    {
        try
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
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    #region Fetch Sub Account
    private void GetSubAcctData()
    {
        try
        {
            GetMaxSortValue();
            _objAcType.ConnConfig = Session["config"].ToString();
            _objAcType.CType = Convert.ToInt32(ddlType.SelectedValue);
            _objAcType.SubType = txtSubAcct.Text;
            _objAcType.SortOrder = _objAcType.MaxSortValue + 1;
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    #endregion

    #region Set data for Edit
    public void SetDataForEdit()
    {
        try
        {
            if (Request.QueryString["c"] != null) //COPY
            {
                if (Request.QueryString["c"] != "1")
                {
                    lblHeader.Text = "Add Account";
                }
                else
                    lblHeader.Text = "Edit Account";
            }
            else
                lblHeader.Text = "Edit Account";

            DataSet _dsChart = new DataSet();

            _objChart.ID = Convert.ToInt32(Request.QueryString["id"]);
            _dsChart = _objBLChart.GetChart(_objChart);


            ddlType.SelectedValue = _dsChart.Tables[0].Rows[0]["Type"].ToString();
            txtAcName.Text = _dsChart.Tables[0].Rows[0]["fDesc"].ToString();

            if (Convert.ToString(_dsChart.Tables[0].Rows[0]["Acct"]) != "")
            {
                lblAccountName.Text = "Account# " + _dsChart.Tables[0].Rows[0]["Acct"].ToString() + " | ";
            }
            lblAccountName.Text = lblAccountName.Text + _dsChart.Tables[0].Rows[0]["fDesc"].ToString();
            ddlCentral.SelectedValue = _dsChart.Tables[0].Rows[0]["Department"].ToString();
            FillSubAccount();
            try
            {
                if (!string.IsNullOrEmpty(_dsChart.Tables[0].Rows[0]["Sub"].ToString()))
                {
                    ddlSubAcCategory.Items.FindByText(_dsChart.Tables[0].Rows[0]["Sub"].ToString()).Selected = true;
                }
            }
            catch
            {

            }

            txtDescription.Text = _dsChart.Tables[0].Rows[0]["Remarks"].ToString();
            ddlStatus.SelectedValue = _dsChart.Tables[0].Rows[0]["Status"].ToString();

            ddlCompany.SelectedValue = _dsChart.Tables[0].Rows[0]["EN"].ToString();
            txtCompany.Text = _dsChart.Tables[0].Rows[0]["Company"].ToString();
            ddlCompanyEdit.SelectedValue = _dsChart.Tables[0].Rows[0]["EN"].ToString();
            if (Request.QueryString["c"] == null)
            {
                if (Request.QueryString["c"] != "1")
                {
                    txtAcctNum.Text = _dsChart.Tables[0].Rows[0]["Acct"].ToString();
                    txtBal.Text = _dsChart.Tables[0].Rows[0]["Balance"].ToString();
                }
            }
            if (Convert.ToInt16(_dsChart.Tables[0].Rows[0]["NoJE"]).Equals(1))
                chkNoJE.Checked = true;
            else
                chkNoJE.Checked = false;

            if (ddlType.SelectedItem.Text == "Bank")
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "resize controls", "ResizeControls('true');", true);
                pnlBankAccount.Visible = true;
                //pnlBankInfo.Visible = true;
                liGeneral.Style["display"] = "";
                adGeneral.Style["display"] = "";
                pnlBankAccount2.Visible = true;
                //_objChart.ID
                DataSet _dsBank = new DataSet();
                _objBank.ConnConfig = Session["config"].ToString();
                _objBank.Chart = _objChart.ID;
                _dsBank = _objBLBank.GetBankByChart(_objBank);
                if (_dsBank.Tables[0].Rows.Count > 0)
                {
                    DataRow _drBank = _dsBank.Tables[0].Rows[0];
                    if (_dsBank != null)
                    {
                        DataSet _dsRol = new DataSet();
                        _objRol.ConnConfig = Session["config"].ToString();
                        _objRol.ID = Convert.ToInt32(_dsBank.Tables[0].Rows[0]["Rol"]);
                        _dsRol = _objBLBank.GetRolByID(_objRol);
                        DataRow _drRol = _dsRol.Tables[0].Rows[0];
                        SetBankData(_drBank, _drRol);
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
    #endregion
    private void GetMaxSortValue()
    {
        try
        {
            _objAcType.ConnConfig = Session["config"].ToString();
            DataSet _dsAcType = new DataSet();
            _objAcType.CType = Convert.ToInt32(ddlType.SelectedValue);
            _dsAcType = _objBLAcType.GetTypeByAccount(_objAcType);
            _objAcType.MaxSortValue = 0;
            if (_dsAcType != null)
            {
                var strr = _dsAcType.Tables[0].AsEnumerable().Max(m => m["SortOrder"]);
                _objAcType.MaxSortValue = Convert.ToInt32(strr);
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    private void FillState()
    {
        //try
        //{

        //    DataSet _dsState = new DataSet();
        //    State _objState = new State();

        //    _objState.ConnConfig = Session["config"].ToString();

        //    _dsState = _objBLBank.GetStates(_objState);

        //    ddlState.Items.Add(new ListItem("Select State"));
        //    ddlState.AppendDataBoundItems = true;

        //    ddlState.DataSource = _dsState;
        //    ddlState.DataValueField = "Name";
        //    ddlState.DataTextField = "fDesc";
        //    ddlState.DataBind();
        //}
        //catch (Exception ex)
        //{
        //    string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
        //    ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        //}
    }
    private void userpermissions()
    {
        //if (Session["type"].ToString() != "c")
        //{
        //    if (Session["type"].ToString() != "am")
        //    {
        //        _objPropUser.ConnConfig = Session["config"].ToString();
        //        _objPropUser.Username = Session["username"].ToString();
        //        _objPropUser.PageName = "addcoa.aspx";
        //        DataSet dspage = _objBLUser.getScreensByUser(_objPropUser);
        //        if (dspage.Tables[0].Rows.Count > 0)
        //        {
        //            if (Convert.ToBoolean(dspage.Tables[0].Rows[0]["access"].ToString()) == false)
        //            {
        //                Response.Redirect("home.aspx", false);
        //            }
        //        }
        //        else
        //        {
        //            Response.Redirect("home.aspx", false);
        //        }
        //    }
        //}
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

            /// Vendor  ///////////////////------->

            string chartPermission = dtUserPermission.Rows[0]["chart"] == DBNull.Value ? "YYYYYY" : dtUserPermission.Rows[0]["chart"].ToString();
            string ADD = chartPermission.Length < 1 ? "Y" : chartPermission.Substring(0, 1);
            string Edit = chartPermission.Length < 2 ? "Y" : chartPermission.Substring(1, 1);
            string Delete = chartPermission.Length < 3 ? "Y" : chartPermission.Substring(2, 1);
            string View = chartPermission.Length < 4 ? "Y" : chartPermission.Substring(3, 1);

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
                    btnSubmit.Visible = false;
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


    protected void btnCompanyEdit_Click(object sender, EventArgs e)
    {

        Submit();
        if (Convert.ToInt32(ViewState["CompPermission"]) == 1)
        {
            Response.Redirect(Request.RawUrl);
        }
    }

    protected void btnCompanyPopUp_Click(object sender, EventArgs e)
    {
        string script = "function f(){$find(\"" + RadWindowCompany.ClientID + "\").show(); Sys.Application.remove_load(f);}Sys.Application.add_load(f); Materialize.updateTextFields();";
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
    }
    protected void btnAddCentral_Click(object sender, EventArgs e)
    {
        string script = "function f(){$find(\"" + RadWindowCentral.ClientID + "\").show(); Sys.Application.remove_load(f);}Sys.Application.add_load(f); Materialize.updateTextFields();";
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
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
    private void UpdateDocInfo()
    {
        _objPropUser.ConnConfig = Session["config"].ToString();
        _objPropUser.dtDocs = SaveDocInfo();
        _objPropUser.Username = Session["User"].ToString();
        _objBLUser.UpdateDocInfo(_objPropUser);
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


    private string GetUploadDirectory(string mainDirectory)
    {
        var savepathconfig = System.Web.Configuration.WebConfigurationManager.AppSettings["AttachmentPath"].Trim();
        return savepathconfig + @"\" + Session["dbname"] + @"\" + mainDirectory + @"\";
    }
    protected void lnkUploadDoc_Click(object sender, EventArgs e)
    {
        try
        {
            string filename = string.Empty;
            string fullpath = string.Empty;
            string mime = string.Empty;
            var savepath = string.Empty;

            var mainDirectory = "COADocs";

            if (!string.IsNullOrEmpty(Request.QueryString["id"]))
            {
                if (Request.QueryString["r"] != null && Request.QueryString["r"].ToString() == "1")
                {
                    mainDirectory += "\\RE_" + Request.QueryString["id"];
                }
                else
                {
                    mainDirectory += "\\COA_" + Request.QueryString["id"];
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
                    objMapData.Screen = "COA";
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

                    tempTable = SaveAttachedFilesWhenAddingCOA(filename, fullpath, mime);
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


    private DataTable SaveAttachedFilesWhenAddingCOA(string fileName, string fullPath, string doctype)
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

    protected void RadGrid_Documents_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        GetDocuments();

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
                objMapData.Screen = "COA";
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

   
    protected void RadGrid_Documents_PreRender(object sender, EventArgs e)
    {
        RowSelectDocuments();
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
                    objProp_Customer.LogScreen = "Chart of Account";
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

    private void UpdateTempDateWhenCreatingNewCOA(Int32 coaid)
    {
        //var JEId = Convert.ToInt32(strJEId);
        if (ViewState["TempUploadDirectory"] == null)
        {
            return;
        }
        var tempAttachedFiles = ViewState["AttachedFiles"] as DataTable;
        var tempDirectory = "COADocs\\" + ViewState["TempUploadDirectory"] as string;
        var newDirectory = "COADocs\\";
        // "JEDocs\\" + "JE_" + strJEId;
        if (Request.QueryString["r"] != null && Request.QueryString["r"].ToString() == "1")
        {
            newDirectory += "COA_"+ coaid;
        }
        else
        {
            newDirectory += "COA_"+ coaid;
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
            objMapData.Screen = "COA";
        }
        else
        {
            objMapData.Screen = "COA";
        }
        foreach (DataRow row in tempAttachedFiles.Rows)
        {
            objMapData.Screen = "COA";
            objMapData.TicketID = coaid;
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
            objMapData.Screen = "COA";
        }
        else
        {
            objMapData.Screen = "COA";
        }
        //objMapData.TicketID = JEId;
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
}