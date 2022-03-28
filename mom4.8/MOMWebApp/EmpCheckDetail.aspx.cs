using BusinessEntity;
using BusinessEntity.APModels;
using BusinessEntity.CommonModel;
using BusinessEntity.Payroll;
using BusinessEntity.Utility;
using BusinessLayer;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Reporting.WebForms;
using MOMWebApp;
using Stimulsoft.Report;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class EmpCheckDetail : System.Web.UI.Page
{
    #region Variables
    Chart _objChart = new Chart();
    BL_Chart _objBLChart = new BL_Chart();
    BusinessEntity.User objProp_User = new BusinessEntity.User();
    //CD _objCD = new CD();
    PRReg _objPRReg = new PRReg();
    BL_Wage _objBLWage = new BL_Wage();
    BL_Bills _objBLBill = new BL_Bills();

    Paid _objPaid = new Paid();

    Transaction _objTrans = new Transaction();
    BL_JournalEntry _objBLJournal = new BL_JournalEntry();

    Vendor _objVendor = new Vendor();
    BL_Vendor _objBLVendor = new BL_Vendor();

    User objPropUser = new User();
    BL_User objBL_User = new BL_User();

    Bank _objBank = new Bank();
    BL_BankAccount _objBL_Bank = new BL_BankAccount();

    protected DataTable dti = new DataTable();
    protected DataTable dtpay = new DataTable();
    protected DataTable dtBank = new DataTable();
    List<byte[]> lstbyte = new List<byte[]>();
    List<byte[]> lstbyteNew = new List<byte[]>();
    byte[] array = null;
    byte[] arrayNew = null;
    bool isEdit = false;




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
                imgVoid.Visible = false;
                imgCleared.Visible = false;
                CompanyPermission();
                UserPermission();
                if (Request.QueryString["id"] != null)
                {
                    lblCheck.Visible = true;
                    lblCheckNo.Visible = true;
                    pnlNext.Visible = true;
                    SetDataForEdit();
                    tbLogs.Style["display"] = "block";
                }
            }
            if (!IsPostBack)
            {
                string path = Server.MapPath("StimulsoftReports/APChecks/APTopCheck/");
                DirectoryInfo d = new DirectoryInfo(path);
                FileInfo[] Files = d.GetFiles("*.mrt");
                foreach (FileInfo file in Files)
                {

                    string FileName = string.Empty;
                    if (file.Name.Contains(".mrt"))
                        FileName = file.Name.Replace(".mrt", " ");

                    ddlApTopCheckForLoad.Items.Add((FileName));
                }

                ddlApTopCheckForLoad.DataBind();

                string MidCheckpath = Server.MapPath("StimulsoftReports/APChecks/APMidCheck/");
                DirectoryInfo dirMidPath = new DirectoryInfo(MidCheckpath);
                FileInfo[] FilesMid = dirMidPath.GetFiles("*.mrt");
                foreach (FileInfo fileMid in FilesMid)
                {

                    string FileName = string.Empty;
                    if (fileMid.Name.Contains(".mrt"))
                        FileName = fileMid.Name.Replace(".mrt", " ");
                    ddlApMiddleCheckForLoad.Items.Add((FileName));
                }

                ddlApMiddleCheckForLoad.DataBind();


                string TopCheckpath = Server.MapPath("StimulsoftReports/APChecks/TopChecks/");
                DirectoryInfo dirTopcheckPath = new DirectoryInfo(TopCheckpath);
                FileInfo[] FilesTop = dirTopcheckPath.GetFiles("*.mrt");
                foreach (FileInfo fileTop in FilesTop)
                {
                    string FileName = string.Empty;
                    if (fileTop.Name.Contains(".mrt"))
                        FileName = fileTop.Name.Replace(".mrt", " ");
                    ddlTopChecksForLoad.Items.Add((FileName));
                }
                ddlTopChecksForLoad.DataBind();

                //objProp_User.ConnConfig = Session["config"].ToString();
                //DataSet dsControl = new DataSet();
                //dsControl = objBL_User.getControl(objProp_User);


                //if (dsControl.Tables[0].Rows.Count > 0)
                //{
                //    lbltopcom.Text = dsControl.Tables[0].Rows[0]["Name"].ToString();
                //    lblmidcom.Text = dsControl.Tables[0].Rows[0]["Name"].ToString();
                //    lbldetailcom.Text = dsControl.Tables[0].Rows[0]["Name"].ToString();
                //    lbltopdd.Text = dsControl.Tables[0].Rows[0]["Address"].ToString() + " " + dsControl.Tables[0].Rows[0]["City"].ToString() + " " + dsControl.Tables[0].Rows[0]["State"].ToString() + ", " + dsControl.Tables[0].Rows[0]["Zip"].ToString();
                //    lblmidadd.Text = dsControl.Tables[0].Rows[0]["Address"].ToString() + " " + dsControl.Tables[0].Rows[0]["City"].ToString() + " " + dsControl.Tables[0].Rows[0]["State"].ToString() + ", " + dsControl.Tables[0].Rows[0]["Zip"].ToString();
                //    lbldetailadd.Text = dsControl.Tables[0].Rows[0]["Address"].ToString() + " " + dsControl.Tables[0].Rows[0]["City"].ToString() + " " + dsControl.Tables[0].Rows[0]["State"].ToString() + ", " + dsControl.Tables[0].Rows[0]["Zip"].ToString();
                //    lbltopemail.Text = dsControl.Tables[0].Rows[0]["Email"].ToString();
                //    lblmidemail.Text = dsControl.Tables[0].Rows[0]["Email"].ToString();
                //    lbldetailemail.Text = dsControl.Tables[0].Rows[0]["Email"].ToString();
                //}
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "+({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
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

        ds = objBL_User.GetUserPermissionByUserID(objPropUser);


        return ds.Tables[0];
    }
    private void UserPermission()
    {
        // User Permission 
        if (Session["type"].ToString() != "am" && Session["type"].ToString() != "c")
        {
            DataTable ds = new DataTable();
            ds = GetUserById();

            /// AccountPayablemodulePermission ///////////////////------->

            string AccountPayablemodulePermission = ds.Rows[0]["AccountPayablemodulePermission"] == DBNull.Value ? "Y" : ds.Rows[0]["AccountPayablemodulePermission"].ToString();

            if (AccountPayablemodulePermission == "N")
            {
                Response.Redirect("Home.aspx?permission=no"); return;
            }

            /// BillPay ///////////////////------->

            string BillPayPermission = ds.Rows[0]["BillPay"] == DBNull.Value ? "YYYYYY" : ds.Rows[0]["BillPay"].ToString();
            string ADD = BillPayPermission.Length < 1 ? "Y" : BillPayPermission.Substring(0, 1);
            string Edit = BillPayPermission.Length < 2 ? "Y" : BillPayPermission.Substring(1, 1);
            string Delete = BillPayPermission.Length < 2 ? "Y" : BillPayPermission.Substring(2, 1);
            string View = BillPayPermission.Length < 4 ? "Y" : BillPayPermission.Substring(3, 1);
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
                    //lnkSave.Visible = btnPrintCheck.Visible = false;
                }
                else
                {
                    Response.Redirect("Home.aspx?permission=no"); return;
                }

            }



        }
    }
    private void CompanyPermission()
    {
        if (Session["COPer"] != null)
        {
            if (Session["COPer"].ToString() == "1")
            {

                // dvCompanyPermission.Visible = true;

            }
            else
            {

                //dvCompanyPermission.Visible = false;
            }
        }
    }
    #endregion
    protected void lnkClose_Click(object sender, EventArgs e)
    {

        if (Session["alId"] != null)
        {
            if (Request.QueryString["frm"] != null)
            {
                if (Request.QueryString["frm"].ToString() == "MNG")
                {
                    Response.Redirect("managechecks.aspx");
                }
                else if (Request.QueryString["frm"].ToString() == "MNG1")
                {
                    Response.Redirect("ManageBills.aspx");
                }
                else
                    Response.Redirect("AccountLedger.aspx?id=" + Session["alId"]);
            }
            else
            {
                Response.Redirect("AccountLedger.aspx?id=" + Session["alId"]);
            }
        }
        else
        {
            if (Request.QueryString["frm"] == null)
            {
                if (Request.QueryString["page"] != null)
                {
                    if (Request.QueryString["page"].ToString() == "bankrecon")
                    {
                        Response.Redirect(Request.QueryString["page"].ToString() + ".aspx");
                    }
                    else
                    {
                        Response.Redirect("managechecks.aspx");
                    }
                }
                else
                {
                    Response.Redirect("managechecks.aspx");
                }

            }
            else
            {


                if (Request.QueryString["frm"].ToString() == "MNG")
                {
                    Response.Redirect("managechecks.aspx");
                }
                else if (Request.QueryString["frm"].ToString() == "MNG1")
                {
                    Response.Redirect("ManageBills.aspx");
                }
                else if (Request.QueryString["frm"].ToString() == "Register")
                {
                    Response.Redirect("PayrollRegister.aspx");
                }

                else
                    Response.Redirect("managechecks.aspx");
            }
        }
        //else if (Request.QueryString["frm"].ToString() == "MNG1")
        //{
        //    Response.Redirect("ManageBills.aspx");
        //}
        //else
        //Response.Redirect("managechecks.aspx");
    }
    public bool CheckNumValidOnEdit(string checkno, string bank, string cdId, string PaymentType)
    {
        bool IsExistCheckNo = false;
        if (!string.IsNullOrEmpty(checkno) && !string.IsNullOrEmpty(bank))
        {
            _objPRReg.ConnConfig = Session["config"].ToString();
            _objPRReg.ID = Convert.ToInt32(cdId);
            _objPRReg.Ref = Convert.ToInt32(checkno);
            _objPRReg.Bank = Convert.ToInt32(bank);
            IsExistCheckNo = _objBLWage.IsExistCheckNumOnEdit(_objPRReg);

        }

        return IsExistCheckNo;
    }
    protected void lnkChangeCheckDate_Click(object sender, EventArgs e)
    {
        try
        {
            _objPRReg.ConnConfig = Session["config"].ToString();
            _objPRReg.ID = Convert.ToInt32(Request.QueryString["id"]);
            _objPRReg.Checkno = Convert.ToInt32(txtCheck.Text);
            _objPRReg.Bank = Convert.ToInt32(hdnBankID.Value.ToString());
            _objPRReg.CDate = Convert.ToDateTime(txtCheckDate.Text);
            _objPRReg.MOMUSer = Convert.ToString(Session["Username"].ToString());
            _objPRReg.ProcessMethod = "CheckDate";

            _objBLWage.UpdateCheckNo(_objPRReg);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: 'Check date changed successfully.',  type : 'success', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            Response.Redirect(Request.RawUrl, false);
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    protected void lnkChangeCheckNo_Click(object sender, EventArgs e)
    {
        try
        {
            bool checknoexist = CheckNumValidOnEdit(txtCheck.Text, hdnBankID.Value.ToString(), Request.QueryString["id"].ToString(), hdnPaymentType.Value);
            if (checknoexist == false)
            {
                _objPRReg.ConnConfig = Session["config"].ToString();
                _objPRReg.ID = Convert.ToInt32(Request.QueryString["id"]);
                _objPRReg.Checkno = Convert.ToInt32(txtCheck.Text);
                _objPRReg.Bank = Convert.ToInt32(hdnBankID.Value.ToString());
                _objPRReg.CDate = Convert.ToDateTime(txtCheckDate.Text);
                _objPRReg.MOMUSer = Convert.ToString(Session["Username"].ToString());
                _objPRReg.ProcessMethod = "CheckNo";

                _objBLWage.UpdateCheckNo(_objPRReg);

                Response.Redirect(Request.RawUrl, false);

            }
            else
            {
                //ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: 'Check #'" + txtCheck.Text + "' is already in exists in bank account. Since duplicate check numbers are not allowed, the check generation process cannot continue.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
                string str = "Check #" + txtCheck.Text + " already exists. Since duplicate check numbers are not supported, the check generation process cannot continue.";
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningVendor", "noty({text: '" + str + "',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true}); $('#MOMloading').hide();", true);
                return;
            }

        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }


    protected void lnkSave_Click(object sender, EventArgs e)
    {
        try
        {
            bool checknoexist = CheckNumValidOnEdit(txtCheck.Text, hdnBankID.Value.ToString(), Request.QueryString["id"].ToString(), hdnPaymentType.Value);
            //if (checknoexist == false)
            //{

            //    _objCD.ConnConfig = Session["config"].ToString();
            //    _objCD.ID = Convert.ToInt32(Request.QueryString["id"]);
            //    _objCD.fDate = Convert.ToDateTime(txtCheckDate.Text);
            //    _objCD.Ref = long.Parse(txtCheck.Text);
            //    _objCD.Vendor = Convert.ToInt32(hdnEmpID.Value);
            //    _objCD.MOMUSer = Session["Username"].ToString();


            //        _objBLBill.UpdateAPCDDate(_objCD);




            //    _objBank.ConnConfig = Session["config"].ToString();
            //    _objBank.ID = Convert.ToInt32(hdnBankID.Value);
            //    _objBank.NextC = long.Parse(txtCheck.Text) + 1;


            //        _objBL_Bank.UpdateNextCheck(_objBank);




            //    _objTrans.ConnConfig = Session["config"].ToString();
            //    _objTrans.BatchID = Convert.ToInt32(hdnBatch.Value);
            //    _objTrans.TransDate = Convert.ToDateTime(txtCheckDate.Text);
            //    _objTrans.Ref = long.Parse(txtCheck.Text);


            //        _objBLJournal.UpdateTransDateByBatch(_objTrans);


            //    Response.Redirect(Request.RawUrl, false);

            //}
            //else
            //{
            //    //ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: 'Check #'" + txtCheck.Text + "' is already in exists in bank account. Since duplicate check numbers are not allowed, the check generation process cannot continue.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
            //    string str = "Check #" + txtCheck.Text + " already exists. Since duplicate check numbers are not supported, the check generation process cannot continue.";
            //    ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningVendor", "noty({text: '" + str + "',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true}); $('#MOMloading').hide();", true);
            //    return;
            //}
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void lnkPrevious_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = (DataTable)Session["PayRegister"];
            if (dt != null)
            {
                DataColumn[] keyColumns = new DataColumn[1];
                keyColumns[0] = dt.Columns["ID"];
                dt.PrimaryKey = keyColumns;

                DataRow d = dt.Rows.Find(Request.QueryString["id"].ToString());
                int index = dt.Rows.IndexOf(d);

                if (index > 0)
                {
                    //Response.Redirect("editcheck.aspx?id=" + dt.Rows[index - 1]["ID"]);

                    if (Request.QueryString["frm"].ToString() != null)
                    {
                        if (Request.QueryString["frm"].ToString() == "MNG")
                        {
                            //Response.Redirect("managechecks.aspx");
                            //Response.Redirect("editcheck.aspx?id=" + dt.Rows[index + 1]["ID"].ToString() + "&frm=MNG");
                            Response.Redirect("editcheck.aspx?id=" + dt.Rows[index - 1]["ID"].ToString() + "&frm=MNG");
                        }
                        else if (Request.QueryString["frm"].ToString() == "MNG1")
                        {
                            //Response.Redirect("ManageBills.aspx");
                            //Response.Redirect("editcheck.aspx?id=" + dt.Rows[index + 1]["ID"].ToString() + "&frm=MNG");
                            Response.Redirect("editcheck.aspx?id=" + dt.Rows[index - 1]["ID"].ToString() + "&frm=MNG");
                        }
                        else if (Request.QueryString["frm"].ToString() == "Register")
                        {
                            //Response.Redirect("ManageBills.aspx");
                            Response.Redirect("EmpCheckDetail.aspx?id=" + dt.Rows[index - 1]["ID"].ToString() + "&frm=Register");
                        }
                    }
                    else
                        Response.Redirect("editcheck.aspx?id=" + dt.Rows[index + 1]["ID"]);

                }
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
            DataTable dt = (DataTable)Session["PayRegister"];
            if (dt != null)
            {
                DataColumn[] keyColumns = new DataColumn[1];
                keyColumns[0] = dt.Columns["ID"];
                dt.PrimaryKey = keyColumns;

                DataRow d = dt.Rows.Find(Request.QueryString["id"].ToString());
                int index = dt.Rows.IndexOf(d);
                int c = dt.Rows.Count - 1;

                if (index < c)
                {
                    //Response.Redirect("editcheck.aspx?id=" + dt.Rows[index + 1]["ID"]);

                    if (Request.QueryString["frm"].ToString() != null)
                    {
                        if (Request.QueryString["frm"].ToString() == "MNG")
                        {
                            //Response.Redirect("managechecks.aspx");
                            Response.Redirect("editcheck.aspx?id=" + dt.Rows[index + 1]["ID"].ToString() + "&frm=MNG");
                        }
                        else if (Request.QueryString["frm"].ToString() == "MNG1")
                        {
                            //Response.Redirect("ManageBills.aspx");
                            Response.Redirect("editcheck.aspx?id=" + dt.Rows[index + 1]["ID"].ToString() + "&frm=MNG");
                        }
                        else if (Request.QueryString["frm"].ToString() == "Register")
                        {
                            //Response.Redirect("ManageBills.aspx");
                            Response.Redirect("EmpCheckDetail.aspx?id=" + dt.Rows[index + 1]["ID"].ToString() + "&frm=Register");
                        }
                    }
                    else
                        Response.Redirect("editcheck.aspx?id=" + dt.Rows[index + 1]["ID"]);

                }
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
            DataTable dt = (DataTable)Session["PayRegister"];
            if (dt != null)
            {
                if (Request.QueryString["frm"].ToString() != null)
                {
                    if (Request.QueryString["frm"].ToString() == "MNG")
                    {
                        //Response.Redirect("managechecks.aspx");
                        Response.Redirect("editcheck.aspx?id=" + dt.Rows[dt.Rows.Count - 1]["ID"].ToString() + "&frm=MNG");
                    }
                    else if (Request.QueryString["frm"].ToString() == "MNG1")
                    {
                        //Response.Redirect("ManageBills.aspx");
                        Response.Redirect("editcheck.aspx?id=" + dt.Rows[dt.Rows.Count - 1]["ID"].ToString() + "&frm=MNG");
                    }
                    else if (Request.QueryString["frm"].ToString() == "Register")
                    {
                        //Response.Redirect("ManageBills.aspx");
                        Response.Redirect("EmpCheckDetail.aspx?id=" + dt.Rows[dt.Rows.Count - 1]["ID"].ToString() + "&frm=Register");
                    }
                }
                else
                    Response.Redirect("editcheck.aspx?id=" + dt.Rows[dt.Rows.Count - 1]["ID"]);
            }

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
            DataTable dt = (DataTable)Session["PayRegister"];
            if (dt != null)
            {
                if (Request.QueryString["frm"].ToString() != null)
                {
                    if (Request.QueryString["frm"].ToString() == "MNG")
                    {
                        //Response.Redirect("managechecks.aspx");
                        Response.Redirect("editcheck.aspx?id=" + dt.Rows[0]["ID"].ToString() + "&frm=MNG");
                    }
                    else if (Request.QueryString["frm"].ToString() == "MNG1")
                    {
                        //Response.Redirect("ManageBills.aspx");
                        Response.Redirect("editcheck.aspx?id=" + dt.Rows[0]["ID"].ToString() + "&frm=MNG");
                    }
                    else if (Request.QueryString["frm"].ToString() == "Register")
                    {
                        //Response.Redirect("ManageBills.aspx");
                        Response.Redirect("EmpCheckDetail.aspx?id=" + dt.Rows[0]["ID"].ToString() + "&frm=Register");
                    }
                }
                else
                    Response.Redirect("editcheck.aspx?id=" + dt.Rows[0]["ID"]);
            }

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    #endregion

    #region Custom Functions
    public void SetDataForEdit()
    {
        try
        {
            _objPRReg.ConnConfig = Session["config"].ToString();
            _objPRReg.ID = Convert.ToInt32(Request.QueryString["id"]);

            DataSet _dsCD = new DataSet();
            _dsCD = _objBLWage.GetPayrollCheckDetail(_objPRReg);


            if (_dsCD.Tables[0].Rows.Count > 0)
            {
                DataRow _dr = _dsCD.Tables[0].Rows[0];


                lblCheckNo.Text = _dr["Ref"].ToString();
                if (Convert.ToString(_dr["Name"]) != "")
                {
                    lblCheckNo.Text = lblCheckNo.Text + " | " + Convert.ToString(_dr["Name"]);
                }
                hdnEmpID.Value = _dr["EmpID"].ToString();
                txtPayee.Text = _dr["Name"].ToString() + "\n" + _dr["Address"].ToString() + "," + _dr["City"].ToString() + "\n" + _dr["State"].ToString() + "," + _dr["Zip"].ToString();
                txtMemo.Text = _dr["fDesc"].ToString();
                txtCheckDate.Text = Convert.ToDateTime(_dr["fDate"]).ToString("MM/dd/yyyy");
                txtCheck.Text = _dr["Ref"].ToString();
                hdnCDID.Value = _dr["ID"].ToString();
                hdnBankID.Value = _dr["Bank"].ToString();
                txtBank.Text = _dr["BankName"].ToString();
                lblTotalVal.Text = string.Format("{0:c}", Convert.ToDouble(_dr["Net"]));
                lblTotlIncomeVal.Text = string.Format("{0:c}", Convert.ToDouble(_dr["GrossAmt"]));
                lblTotalDeductionVal.Text = string.Format("{0:c}", Convert.ToDouble(_dr["TDed"]));
                //txtPaymentType.Text = _dr["Type"].ToString();
                txtPaymentType.Text = "Check";
                txtPaymentType.ReadOnly = true;
                //txtCompany.Text = _dr["Company"].ToString();
                //hdnPaymentType.Value = _dr["PaymentType"].ToString();
                BindCheckGrid();
                ViewState["CheckStatus"] = "0";
                _objTrans.ConnConfig = Session["config"].ToString();
                _objTrans.Type = 90;
                _objTrans.BatchID = Convert.ToInt32(_dr["Batch"]);


                DataSet _dsTran = new DataSet();

                _dsTran = _objBLJournal.GetTransByBatchType(_objTrans);

                if (_dsTran.Tables[0].Rows.Count > 0)
                {
                    if (Convert.ToInt16(_dsTran.Tables[0].Rows[0]["Sel"]).Equals(1))            //Voided check
                    {

                        imgVoid.Visible = true;
                        //lnkSave.Visible = false;
                        ViewState["CheckStatus"] = "1";
                        txtCheckDate.ReadOnly = true;
                        txtCheck.ReadOnly = true;
                    }
                    else if (Convert.ToInt16(_dsTran.Tables[0].Rows[0]["Sel"]).Equals(0))      //Cleared check
                    {
                        if (Convert.ToString(_dsTran.Tables[0].Rows[0]["Status"]) == "T")      //Cleared check
                        {
                            imgCleared.Visible = true;
                            //lnkSave.Visible = false;
                            ViewState["CheckStatus"] = "2";
                            txtCheckDate.ReadOnly = true;
                            txtCheck.ReadOnly = true;
                        }
                    }
                }

                txtPayee.ReadOnly = true;
                txtBank.ReadOnly = true;
                txtMemo.ReadOnly = true;


            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", false);
        }
    }
    private void BindCheckGrid()
    {
        try
        {
            _objPRReg.ConnConfig = Session["config"].ToString();
            _objPRReg.ID = Convert.ToInt32(Request.QueryString["id"]);
            DataSet _ds = new DataSet();
            _ds = _objBLWage.GetPayrollCheckDetail(_objPRReg);

            DataTable IncomeDt = _ds.Tables[1].DefaultView.ToTable();
            ViewState["IncomeDT"] = IncomeDt;
            gvIncome.DataSource = IncomeDt;
            gvIncome.DataBind();

            DataTable DeductionDt = _ds.Tables[2].DefaultView.ToTable();
            ViewState["DeductionDt"] = DeductionDt;
            gvDeduction.DataSource = DeductionDt;
            gvDeduction.DataBind();

            DataTable BreakDownDt = _ds.Tables[3].DefaultView.ToTable();
            ViewState["BreakDownDt"] = BreakDownDt;
            gvBreakdown.DataSource = BreakDownDt;
            gvBreakdown.DataBind();

            //CalculateBalance();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    private void CalculateBalance()
    {
        try
        {
            DataTable dt = (DataTable)ViewState["IncomeDT"];
            double dblOrigTotal = 0;
            double dblBalTotal = 0;
            double dblDiscTotal = 0;
            double dblPaidTotal = 0;

            if (dt.Rows.Count > 0)
            {
                //foreach (GridViewRow gr in gvUsers.Rows)
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["Original"].ToString() != string.Empty)
                    {
                        dblOrigTotal = dblOrigTotal + Convert.ToDouble(dr["Original"].ToString());
                    }
                    if (dr["Balance"].ToString() != string.Empty)
                    {
                        dblBalTotal = dblBalTotal + Convert.ToDouble(dr["Balance"].ToString());
                    }
                    if (dr["Disc"].ToString() != string.Empty)
                    {
                        dblDiscTotal = dblDiscTotal + Convert.ToDouble(dr["Disc"].ToString());
                    }
                    if (dr["Paid"].ToString() != string.Empty)
                    {
                        dblPaidTotal = dblPaidTotal + Convert.ToDouble(dr["Paid"].ToString());
                    }
                }
                //Label lblOrigTotal = (Label)gvIncome.FooterRow.FindControl("lblOrigTotal");
                //Label lblBalanceTotal = (Label)gvIncome.FooterRow.FindControl("lblBalanceTotal");
                //Label lblDiscTotal = (Label)gvIncome.FooterRow.FindControl("lblDiscTotal");
                //Label lblPaidTotal = (Label)gvIncome.FooterRow.FindControl("lblPaidTotal");

                GridFooterItem footerItem = (GridFooterItem)gvIncome.MasterTableView.GetItems(GridItemType.Footer)[0];
                Label lblOrigTotal = (Label)footerItem.FindControl("lblOrigTotal");
                Label lblBalanceTotal = (Label)footerItem.FindControl("lblBalanceTotal");
                Label lblDiscTotal = (Label)footerItem.FindControl("lblDiscTotal");
                Label lblPaidTotal = (Label)footerItem.FindControl("lblPaidTotal");

                if (dblOrigTotal < 0)
                {
                    lblOrigTotal.ForeColor = System.Drawing.Color.Red;
                    lblOrigTotal.Text = string.Format("{0:c}", dblOrigTotal * -1);
                }
                else
                {
                    lblOrigTotal.Text = string.Format("{0:c}", dblOrigTotal);
                }

                if (dblBalTotal < 0)
                {
                    lblBalanceTotal.ForeColor = System.Drawing.Color.Red;
                    lblBalanceTotal.Text = string.Format("{0:c}", dblBalTotal * -1);
                }
                else
                {
                    lblBalanceTotal.Text = string.Format("{0:c}", dblBalTotal);
                }

                if (dblDiscTotal < 0)
                {
                    lblDiscTotal.ForeColor = System.Drawing.Color.Red;
                    lblDiscTotal.Text = string.Format("{0:c}", dblDiscTotal * -1);
                }
                else
                {
                    lblDiscTotal.Text = string.Format("{0:c}", dblDiscTotal);
                }

                if (dblPaidTotal < 0)
                {
                    lblPaidTotal.ForeColor = System.Drawing.Color.Red;
                    lblPaidTotal.Text = string.Format("{0:c}", dblPaidTotal * -1);
                }
                else
                {
                    lblPaidTotal.Text = string.Format("{0:c}", dblPaidTotal);
                }
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
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
    private string ConvertNumberToCurrency(double _amount)
    {
        string _currencyInWord = ConvertNumbertoWords(Convert.ToInt32(Math.Truncate(_amount)));
        double d = _amount - Math.Truncate(_amount);
        if (d > 0)
        {
            d = Math.Round(d * 100);
            _currencyInWord = _currencyInWord + " And " + d.ToString() + " / 100";
        }

        //if (_amount < 1000)
        //{
        //     _currencyInWord = "*** " + _currencyInWord + "**************** Dollar";
        //}
        //else
        //{
        _currencyInWord = "*** " + _currencyInWord + "****************";
        //}

        // _currencyInWord = "*** " + _currencyInWord + "****************";
        return _currencyInWord;
    }
    public static string ConvertNumbertoWords(int number)
    {
        if (number == 0)
            return "Zero";
        if (number < 0)
            return "minus " + ConvertNumbertoWords(Math.Abs(number));
        string words = "";
        if ((number / 1000000) > 0)
        {
            words += ConvertNumbertoWords(number / 1000000) + " Million ";
            number %= 1000000;
        }
        if ((number / 1000) > 0)
        {
            words += ConvertNumbertoWords(number / 1000) + " Thousand ";
            number %= 1000;
        }
        if ((number / 100) > 0)
        {
            words += ConvertNumbertoWords(number / 100) + " Hundred ";
            number %= 100;
        }
        if (number > 0)
        {
            if (words != "")
                words += "And ";
            //var unitsMap = new[] { "ZERO", "ONE", "TWO", "THREE", "FOUR", "FIVE", "SIX", "SEVEN", "EIGHT", "NINE", "TEN", "ELEVEN", "TWELVE", "THIRTEEN", "FOURTEEN", "FIFTEEN", "SIXTEEN", "SEVENTEEN", "EIGHTEEN", "NINETEEN" };
            //var tensMap = new[] { "ZERO", "TEN", "TWENTY", "THIRTY", "FORTY", "FIFTY", "SIXTY", "SEVENTY", "EIGHTY", "NINETY" };
            var unitsMap = new[] { "Zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };
            var tensMap = new[] { "Zero", "Ten", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };


            if (number < 20)
                words += unitsMap[number];
            else
            {
                words += tensMap[number / 10];
                if ((number % 10) > 0)
                    words += " " + unitsMap[number % 10];
            }
        }
        return words;
    }
    #endregion

    //rahil's implementation
    private void CreateTableInvoice()
    {

        dti.Columns.Add(new DataColumn("Ref", typeof(string)));
        dti.Columns.Add(new DataColumn("InvoiceDate", typeof(string)));
        dti.Columns.Add(new DataColumn("Reference", typeof(string)));
        dti.Columns.Add(new DataColumn("Total", typeof(string)));
        dti.Columns.Add(new DataColumn("Disc", typeof(string)));
        dti.Columns.Add(new DataColumn("AmountPay", typeof(string)));
        dti.Columns.Add(new DataColumn("PayDate", typeof(string)));
        dti.Columns.Add(new DataColumn("CheckNo", typeof(string)));
        dti.Columns.Add(new DataColumn("VendorID", typeof(Int32)));
        dti.Columns.Add(new DataColumn("VendorName", typeof(string)));
        dti.Columns.Add(new DataColumn("Type", typeof(Int32)));
        dti.Columns.Add(new DataColumn("Description", typeof(string)));
    }
    private void CreateTablePayee()
    {
        dtpay.Columns.Add(new DataColumn("Pay", typeof(string)));
        dtpay.Columns.Add(new DataColumn("ToOrder", typeof(string)));
        dtpay.Columns.Add(new DataColumn("Date", typeof(string)));
        dtpay.Columns.Add(new DataColumn("CheckAmount", typeof(string)));
        dtpay.Columns.Add(new DataColumn("ToOrderAddress", typeof(string)));
        dtpay.Columns.Add(new DataColumn("State", typeof(string)));
        dtpay.Columns.Add(new DataColumn("VendorAddress", typeof(string)));
        dtpay.Columns.Add(new DataColumn("RemitAddress", typeof(string)));
    }
    private void CreateTableBank()
    {
        dtBank.Columns.Add(new DataColumn("Name", typeof(string)));
        dtBank.Columns.Add(new DataColumn("Address", typeof(string)));
        dtBank.Columns.Add(new DataColumn("City", typeof(string)));
        dtBank.Columns.Add(new DataColumn("State", typeof(string)));
        dtBank.Columns.Add(new DataColumn("Zip", typeof(string)));
        dtBank.Columns.Add(new DataColumn("NBranch", typeof(string)));
        dtBank.Columns.Add(new DataColumn("NAcct", typeof(string)));
        dtBank.Columns.Add(new DataColumn("NRoute", typeof(string)));
        dtBank.Columns.Add(new DataColumn("Ref", typeof(string)));
    }
    protected void imgPrintTemp8_Click(object sender, ImageClickEventArgs e)
    {
        //      BNN – check top
        string IsAPIIntegrationEnable;
        try
        {
            var SumAmountpay = 0.00;
            DataTable _dti = new DataTable();
            DataRow _dri = null;
            _dti.Columns.Add(new DataColumn("Ref", typeof(string)));
            _dti.Columns.Add(new DataColumn("InvoiceDate", typeof(string)));
            _dti.Columns.Add(new DataColumn("Reference", typeof(string)));
            _dti.Columns.Add(new DataColumn("Total", typeof(string)));
            _dti.Columns.Add(new DataColumn("Disc", typeof(string)));
            _dti.Columns.Add(new DataColumn("AmountPay", typeof(string)));
            _dti.Columns.Add(new DataColumn("PayDate", typeof(string)));
            _dti.Columns.Add(new DataColumn("CheckNo", typeof(string)));
            _dti.Columns.Add(new DataColumn("VendorID", typeof(Int32)));
            _dti.Columns.Add(new DataColumn("VendorName", typeof(string)));
            _dti.Columns.Add(new DataColumn("VendorAcct", typeof(string)));


            DataTable _dtCheck = new DataTable();
            DataRow _drC = null;
            _dtCheck.Columns.Add(new DataColumn("Pay", typeof(string)));
            _dtCheck.Columns.Add(new DataColumn("ToOrder", typeof(string)));
            _dtCheck.Columns.Add(new DataColumn("Date", typeof(string)));
            _dtCheck.Columns.Add(new DataColumn("CheckAmount", typeof(string)));
            _dtCheck.Columns.Add(new DataColumn("ToOrderAddress", typeof(string)));
            _dtCheck.Columns.Add(new DataColumn("State", typeof(string)));
            int vid = Convert.ToInt32(hdnEmpID.Value);
            string vacct = hdnVendorAcct.Value.ToString();
            double totalAmount = 0;
            //foreach (GridViewRow gr in gvIncome.Rows)
            foreach (GridDataItem gr in gvIncome.Items)
            {
                Label lblPaid = (Label)gr.FindControl("lblPaid");
                Label lblBalance = (Label)gr.FindControl("lblBalance");
                Label lblDiscount = (Label)gr.FindControl("lblDiscount");
                Label lblOrig = (Label)gr.FindControl("lblOrig");
                Label lblRef = (Label)gr.FindControl("lblRef");
                Label lblfDate = (Label)gr.FindControl("lblfDate");
                Label lblDesc = (Label)gr.FindControl("lblDesc");

                _dri = _dti.NewRow();
                _dri["Ref"] = lblRef.Text;
                _dri["InvoiceDate"] = lblfDate.Text;
                _dri["Reference"] = lblDesc.Text;

                _dri["Description"] = lblDesc.Text;
                _dri["Total"] = double.Parse(lblOrig.Text.Replace('$', '0'), NumberStyles.AllowParentheses |
                                NumberStyles.AllowThousands |
                                NumberStyles.AllowDecimalPoint);
                _dri["Disc"] = double.Parse(lblDiscount.Text.Replace('$', '0'), NumberStyles.AllowParentheses |
                                NumberStyles.AllowThousands |
                                NumberStyles.AllowDecimalPoint);
                _dri["AmountPay"] = double.Parse(lblPaid.Text.Replace('$', '0'), NumberStyles.AllowParentheses |
                                NumberStyles.AllowThousands |
                                NumberStyles.AllowDecimalPoint);
                SumAmountpay = SumAmountpay + Convert.ToDouble(_dri["AmountPay"]);
                _dri["PayDate"] = txtCheckDate.Text;
                _dri["CheckNo"] = txtCheck.Text;
                if (!string.IsNullOrEmpty(hdnEmpID.Value))
                {
                    _dri["VendorID"] = Convert.ToInt32(hdnEmpID.Value);
                }
                _dri["VendorName"] = txtPayee.Text;
                _dri["VendorAcct"] = hdnVendorAcct.Value.ToString();
                _dti.Rows.Add(_dri);
                totalAmount = totalAmount + double.Parse(lblPaid.Text.Replace('$', '0'), NumberStyles.AllowParentheses |
                                  NumberStyles.AllowThousands |
                                  NumberStyles.AllowDecimalPoint);
            }
            _objVendor.ConnConfig = Session["config"].ToString();
            _objVendor.ID = vid;



            DataSet _dsV = new DataSet();

            _dsV = _objBLVendor.GetVendorRolDetails(_objVendor);


            // DataSet _dsV = _objBLVendor.GetVendorRolDetails(_objVendor);
            string vendAddress = "";
            string vendAddress2 = "";
            if (_dsV.Tables[0].Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(_dsV.Tables[0].Rows[0]["Address"].ToString()))
                {
                    string add1 = Regex.Replace(_dsV.Tables[0].Rows[0]["Address"].ToString(), @"( |\r?\n)\1+", "$1");
                    vendAddress = add1 + ", ";
                }

                if (!string.IsNullOrEmpty(_dsV.Tables[0].Rows[0]["City"].ToString()))
                {
                    vendAddress2 += _dsV.Tables[0].Rows[0]["City"].ToString();
                }
                if (!string.IsNullOrEmpty(_dsV.Tables[0].Rows[0]["State"].ToString()) || !string.IsNullOrEmpty(_dsV.Tables[0].Rows[0]["Zip"].ToString()))
                {
                    if (!string.IsNullOrEmpty(_dsV.Tables[0].Rows[0]["City"].ToString()))
                    {
                        vendAddress2 += ", ";
                    }
                    vendAddress2 += _dsV.Tables[0].Rows[0]["State"].ToString() + " " + _dsV.Tables[0].Rows[0]["Zip"].ToString();
                }
            }
            string _currencyInWord = ConvertNumberToCurrency(totalAmount);
            string _amount = String.Format("{0:c}", totalAmount);
            _amount = _amount.Replace("$", string.Empty);
            _drC = _dtCheck.NewRow();
            _drC["Pay"] = _currencyInWord;
            _drC["ToOrder"] = txtPayee.Text;
            _drC["Date"] = txtCheckDate.Text;
            _drC["CheckAmount"] = _amount;
            _drC["ToOrderAddress"] = vendAddress;
            _drC["VendorAddress"] = vendAddress;
            _drC["State"] = vendAddress2;
            _dtCheck.Rows.Add(_drC);

            DataSet dsC = new DataSet();

            objPropUser.ConnConfig = Session["config"].ToString();


            if (Session["MSM"].ToString() != "TS")
            {

                dsC = objBL_User.getControl(objPropUser);

            }
            else
            {
                objPropUser.LocID = Convert.ToInt32(0);


                dsC = objBL_User.getControlBranch(objPropUser);



            }
            ReportViewer rvChecks = new ReportViewer();
            rvChecks.LocalReport.DataSources.Clear();

            rvChecks.LocalReport.DataSources.Add(new ReportDataSource("dsInvoices", _dti));
            rvChecks.LocalReport.DataSources.Add(new ReportDataSource("dsCheck", _dtCheck));

            rvChecks.LocalReport.DataSources.Add(new ReportDataSource("dsTicket", dsC.Tables[0]));

            string reportPath = "Reports/MaddReportCheck.rdlc";


            rvChecks.LocalReport.ReportPath = reportPath;

            rvChecks.LocalReport.EnableExternalImages = true;
            List<Microsoft.Reporting.WebForms.ReportParameter> param1 = new List<Microsoft.Reporting.WebForms.ReportParameter>();
            string strPath = "file:///" + Server.MapPath(Request.ApplicationPath).Replace("\\", "/");
            param1.Add(new Microsoft.Reporting.WebForms.ReportParameter("Path", strPath + "/images/Company_logo.jpg"));

            rvChecks.LocalReport.SetParameters(param1);

            rvChecks.LocalReport.Refresh();

            byte[] buffer = null;
            buffer = ExportReportToPDF("", rvChecks);
            Response.ClearContent();
            Response.ClearHeaders();
            Response.AddHeader("Content-Disposition", "attachment;filename=MaddCheckBBN.pdf");
            Response.ContentType = "application/pdf";
            Response.AddHeader("Content-Length", (buffer.Length).ToString());
            Response.BinaryWrite(buffer);
            Response.Flush();
            Response.Close();

        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true})", true);
        }
    }

    protected void imgPrintTemp9_Click(object sender, ImageClickEventArgs e)
    {
        //  MADDEN – check top
        string IsAPIIntegrationEnable;
        try
        {
            var SumAmountpay = 0.00;
            CreateTableInvoice();
            CreateTablePayee();
            CreateTableBank();
            DataTable _dtAcct = new DataTable();
            _dtAcct.Columns.Add(new DataColumn("VendorID", typeof(Int32)));
            _dtAcct.Columns.Add(new DataColumn("VendorAcct", typeof(string)));

            DataRow _dri = null;
            DataRow _drC = null;
            //RAHIL'S IMPLEMENTATION
            DataRow _drB = null;
            DataRow _drA = null;
            int vid = Convert.ToInt32(hdnEmpID.Value);
            string vendorAcct = hdnVendorAcct.Value.ToString();
            double totalAmount = 0;
            //foreach (GridViewRow gr in gvIncome.Rows)
            foreach (GridDataItem gr in gvIncome.Items)
            {
                Label lblPaid = (Label)gr.FindControl("lblPaid");
                Label lblBalance = (Label)gr.FindControl("lblBalance");
                Label lblDiscount = (Label)gr.FindControl("lblDiscount");
                Label lblOrig = (Label)gr.FindControl("lblOrig");
                Label lblRef = (Label)gr.FindControl("lblRef");
                Label lblfDate = (Label)gr.FindControl("lblfDate");
                Label lblDesc = (Label)gr.FindControl("lblDesc");

                _dri = dti.NewRow();
                _dri["Ref"] = lblRef.Text;
                _dri["InvoiceDate"] = lblfDate.Text;
                _dri["Reference"] = lblDesc.Text;

                _dri["Description"] = lblDesc.Text;
                _dri["Total"] = double.Parse(lblOrig.Text.Replace('$', '0'), NumberStyles.AllowParentheses |
                                NumberStyles.AllowThousands |
                                NumberStyles.AllowDecimalPoint);
                _dri["Disc"] = double.Parse(lblDiscount.Text.Replace('$', '0'), NumberStyles.AllowParentheses |
                                NumberStyles.AllowThousands |
                                NumberStyles.AllowDecimalPoint);
                _dri["AmountPay"] = double.Parse(lblPaid.Text.Replace('$', '0'), NumberStyles.AllowParentheses |
                                NumberStyles.AllowThousands |
                                NumberStyles.AllowDecimalPoint);
                SumAmountpay = SumAmountpay + Convert.ToDouble(_dri["AmountPay"]);
                _dri["PayDate"] = txtCheckDate.Text;
                _dri["CheckNo"] = txtCheck.Text;
                if (!string.IsNullOrEmpty(hdnEmpID.Value))
                {
                    _dri["VendorID"] = Convert.ToInt32(hdnEmpID.Value);
                }
                _dri["VendorName"] = txtPayee.Text;
                _dri["VendorAcct"] = hdnVendorAcct.Value.ToString();
                dti.Rows.Add(_dri);
                totalAmount = totalAmount + double.Parse(lblPaid.Text.Replace('$', '0'), NumberStyles.AllowParentheses |
                                  NumberStyles.AllowThousands |
                                  NumberStyles.AllowDecimalPoint);
            }

            _objVendor.ConnConfig = Session["config"].ToString();
            _objVendor.ID = vid;


            DataSet _dsV = new DataSet();

            _dsV = _objBLVendor.GetVendorRolDetails(_objVendor);




            string _currencyInWord;
            if (totalAmount > 1000)
            {
                _currencyInWord = ConvertNumberToCurrency(totalAmount);
            }
            else
            {
                _currencyInWord = ConvertNumberToCurrency(totalAmount);
                _currencyInWord = _currencyInWord + " Dollars";
            }

            string _amount = String.Format("{0:c}", totalAmount);
            _amount = _amount.Replace("$", string.Empty);
            _drC = dtpay.NewRow();
            _drC["Pay"] = _currencyInWord;
            _drC["ToOrder"] = txtPayee.Text;
            _drC["Date"] = txtCheckDate.Text;
            _drC["CheckAmount"] = _amount;
            _drC["VendorAddress"] = _dsV.Tables[0].Rows[0]["VendorAddress"];                // change by Mayuri on 8th nov,16
            _drC["RemitAddress"] = _dsV.Tables[0].Rows[0]["RemitAddress"];                  // change by Mayuri on 8th nov,16
            dtpay.Rows.Add(_drC);

            _objBank.ConnConfig = Session["config"].ToString();
            _objBank.ID = Convert.ToInt32(hdnBankID.Value);



            DataSet _dsB = new DataSet();

            _dsB = _objBLBill.GetBankCD(_objBank);



            _drB = dtBank.NewRow();
            _drB["Name"] = _dsB.Tables[0].Rows[0]["Name"].ToString();
            _drB["Address"] = _dsB.Tables[0].Rows[0]["Address"].ToString();
            _drB["State"] = _dsB.Tables[0].Rows[0]["State"].ToString();
            _drB["City"] = _dsB.Tables[0].Rows[0]["City"].ToString();
            _drB["Zip"] = _dsB.Tables[0].Rows[0]["Zip"].ToString();
            _drB["NBranch"] = _dsB.Tables[0].Rows[0]["NBranch"].ToString();
            _drB["NAcct"] = _dsB.Tables[0].Rows[0]["NAcct"].ToString();
            _drB["NRoute"] = _dsB.Tables[0].Rows[0]["NRoute"].ToString();
            //_drB["Ref"] = _dsB.Tables[0].Rows[0]["Ref"].ToString();

            if (txtCheck.Text.Length == 1)
            {
                _drB["Ref"] = "00000000" + txtCheck.Text;
            }
            else if (txtCheck.Text.Length == 2)
            {
                _drB["Ref"] = "0000000" + txtCheck.Text;
            }
            else if (txtCheck.Text.Length == 3)
            {
                _drB["Ref"] = "000000" + txtCheck.Text;
            }
            else if (txtCheck.Text.Length == 4)
            {
                _drB["Ref"] = "00000" + txtCheck.Text;
            }
            else if (txtCheck.Text.Length == 5)
            {
                _drB["Ref"] = "0000" + txtCheck.Text;
            }
            else if (txtCheck.Text.Length == 6)
            {
                _drB["Ref"] = "000" + txtCheck.Text;
            }
            else if (txtCheck.Text.Length == 7)
            {
                _drB["Ref"] = "00" + txtCheck.Text;
            }
            else if (txtCheck.Text.Length == 8)
            {
                _drB["Ref"] = "0" + txtCheck.Text;
            }
            else
            {
                _drB["Ref"] = "000000000";
            }

            dtBank.Rows.Add(_drB);

            _objVendor.ConnConfig = Session["config"].ToString();
            _objVendor.ID = vid;


            DataSet _dsA = new DataSet();

            _dsA = _objBLVendor.GetVendorAcct(_objVendor);


            _drA = _dtAcct.NewRow();
            _drA["VendorID"] = _dsA.Tables[0].Rows[0]["ID"].ToString();
            _drA["VendorAcct"] = _dsA.Tables[0].Rows[0]["Acct#"].ToString();
            _dtAcct.Rows.Add(_drA);
            //-----------------------------------------------

            DataSet dsC = new DataSet();
            ReportViewer rvChecks = new ReportViewer();
            rvChecks.LocalReport.DataSources.Clear();
            rvChecks.ProcessingMode = ProcessingMode.Local;
            rvChecks.LocalReport.DataSources.Add(new ReportDataSource("dsInvoices", dti));
            rvChecks.LocalReport.DataSources.Add(new ReportDataSource("dsCheck", dtpay));

            //RAHIL'S
            rvChecks.LocalReport.DataSources.Add(new ReportDataSource("dsBank", dtBank));
            rvChecks.LocalReport.DataSources.Add(new ReportDataSource("dsAcct", _dtAcct));
            //--------------------------------


            string reportPath = "Reports/MaddCheckTemplate2.rdlc";

            rvChecks.LocalReport.ReportPath = reportPath;
            rvChecks.LocalReport.Refresh();

            byte[] buffer = null;
            buffer = ExportReportToPDF("", rvChecks);

            Response.ClearContent();
            Response.ClearHeaders();
            Response.AddHeader("Content-Disposition", "attachment;filename=TopDetailCheckCub.pdf");
            Response.ContentType = "application/pdf";
            Response.AddHeader("Content-Length", (buffer.Length).ToString());
            Response.BinaryWrite(buffer);
            Response.Flush();
            Response.Close();
            //---------------------------------------------------
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true})", true);
        }
    }

    #region Print check
    protected void imgPrintTemp6_Click(object sender, ImageClickEventArgs e)
    {                                                                   //              MADDEN – check top 
        try
        {
            string reportPathStimul = Server.MapPath("StimulsoftReports/APChecks/TopChecks/" + ddlTopChecksForLoad.SelectedItem.Text.Trim() + ".mrt");
            StiReport report = new StiReport();
            report = FillMaddenDataSetForReport(ddlTopChecksForLoad.SelectedItem.Text.Trim(), false);
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true})", true);
        }
    }

    protected void imgPrintTemp1_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (ddlApTopCheckForLoad.SelectedItem.Text.Trim() != null)
            {
                string reportApTopCheckPathStimul = Server.MapPath("StimulsoftReports/APChecks/APTopCheck/" + ddlApTopCheckForLoad.SelectedItem.Text.Trim() + ".mrt");
                StiReport report = new StiReport();
                report = FillDataSetToReport(ddlApTopCheckForLoad.SelectedItem.Text.Trim(), false);
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true})", true);
        }
    }
    protected void imgPrintTemp2_Click(object sender, ImageClickEventArgs e)
    {                                                                        //                AP – check middle 
        try
        {
            string reportPathStimul = Server.MapPath("StimulsoftReports/APChecks/APMidCheck/" + ddlApMiddleCheckForLoad.SelectedItem.Text.Trim() + ".mrt");
            StiReport report = new StiReport();
            report = FillMiddleDataSetReport(ddlApMiddleCheckForLoad.SelectedItem.Text.Trim(), false);
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true})", true);
        }
    }
    private StiReport FillMaddenDataSetForReport(string reportName, bool isedit)
    {

        double SumAmountpay = 0.00;
        DataTable _dti = new DataTable();
        DataRow _dri = null;
        _dti.Columns.Add(new DataColumn("Ref", typeof(string)));
        _dti.Columns.Add(new DataColumn("InvoiceDate", typeof(string)));
        _dti.Columns.Add(new DataColumn("Reference", typeof(string)));
        _dti.Columns.Add(new DataColumn("Total", typeof(double)));
        _dti.Columns.Add(new DataColumn("Disc", typeof(double)));
        _dti.Columns.Add(new DataColumn("AmountPay", typeof(double)));
        _dti.Columns.Add(new DataColumn("PayDate", typeof(string)));
        _dti.Columns.Add(new DataColumn("CheckNo", typeof(string)));
        _dti.Columns.Add(new DataColumn("VendorID", typeof(Int32)));
        _dti.Columns.Add(new DataColumn("VendorName", typeof(string)));
        _dti.Columns.Add(new DataColumn("VendorAcct", typeof(string)));

        //RAHIL
        _dti.Columns.Add(new DataColumn("Type", typeof(Int32)));
        _dti.Columns.Add(new DataColumn("Description", typeof(string)));

        //New column
        _dti.Columns.Add(new DataColumn("State", typeof(string)));
        _dti.Columns.Add(new DataColumn("ToOrderAddress", typeof(string)));
        _dti.Columns.Add(new DataColumn("CheckAmount", typeof(string)));
        _dti.Columns.Add(new DataColumn("Pay", typeof(string)));
        _dti.Columns.Add(new DataColumn("TotalAmount", typeof(string)));

        DataTable _dtCheck = new DataTable();
        DataRow _drC = null;
        _dtCheck.Columns.Add(new DataColumn("Pay", typeof(string)));
        _dtCheck.Columns.Add(new DataColumn("ToOrder", typeof(string)));
        _dtCheck.Columns.Add(new DataColumn("Date", typeof(string)));
        _dtCheck.Columns.Add(new DataColumn("CheckAmount", typeof(double)));
        _dtCheck.Columns.Add(new DataColumn("ToOrderAddress", typeof(string)));
        _dtCheck.Columns.Add(new DataColumn("State", typeof(string)));


        _dtCheck.Columns.Add(new DataColumn("VendorAddress", typeof(string)));
        _dtCheck.Columns.Add(new DataColumn("RemitAddress", typeof(string)));
        _dtCheck.Columns.Add(new DataColumn("Zip", typeof(string)));

        _dtCheck.Columns.Add(new DataColumn("TotalAmountPay", typeof(double)));
        _dtCheck.Columns.Add(new DataColumn("Memo", typeof(string)));
        int vid = Convert.ToInt32(hdnEmpID.Value);
        string vAcct = hdnVendorAcct.Value.ToString();
        double totalAmount = 0;
        //foreach (GridViewRow gr in gvIncome.Rows)
        foreach (GridDataItem gr in gvIncome.Items)
        {
            Label lblPaid = (Label)gr.FindControl("lblPaid");
            Label lblBalance = (Label)gr.FindControl("lblBalance");
            Label lblDiscount = (Label)gr.FindControl("lblDiscount");
            Label lblOrig = (Label)gr.FindControl("lblOrig");
            Label lblRef = (Label)gr.FindControl("lblRef");
            Label lblfDate = (Label)gr.FindControl("lblfDate");
            Label lblDesc = (Label)gr.FindControl("lblDesc");

            _dri = _dti.NewRow();
            _dri["Ref"] = lblRef.Text;
            _dri["InvoiceDate"] = lblfDate.Text;
            _dri["Reference"] = lblDesc.Text;

            _dri["Description"] = lblDesc.Text;
            _dri["Total"] = double.Parse(lblOrig.Text.Replace('$', '0'), NumberStyles.AllowParentheses |
                            NumberStyles.AllowThousands |
                            NumberStyles.AllowDecimalPoint);
            _dri["Disc"] = double.Parse(lblDiscount.Text.Replace('$', '0'), NumberStyles.AllowParentheses |
                            NumberStyles.AllowThousands |
                            NumberStyles.AllowDecimalPoint);
            _dri["AmountPay"] = double.Parse(lblPaid.Text.Replace('$', '0'), NumberStyles.AllowParentheses |
                            NumberStyles.AllowThousands |
                            NumberStyles.AllowDecimalPoint);
            SumAmountpay = SumAmountpay + Convert.ToDouble(_dri["AmountPay"]);
            _dri["PayDate"] = txtCheckDate.Text;
            _dri["CheckNo"] = txtCheck.Text;
            if (!string.IsNullOrEmpty(hdnEmpID.Value))
            {
                _dri["VendorID"] = Convert.ToInt32(hdnEmpID.Value);
            }
            _dri["VendorAcct"] = hdnVendorAcct.Value.ToString();
            _dri["VendorName"] = txtPayee.Text;
            _dti.Rows.Add(_dri);
            totalAmount = totalAmount + double.Parse(lblPaid.Text.Replace('$', '0'), NumberStyles.AllowParentheses |
                              NumberStyles.AllowThousands |
                              NumberStyles.AllowDecimalPoint);
        }
        _objVendor.ConnConfig = Session["config"].ToString();
        _objVendor.ID = vid;



        DataSet _dsV = new DataSet();

        _dsV = _objBLVendor.GetVendorRolDetails(_objVendor);


        string vendAddress = "";
        string vendAddress2 = "";
        if (_dsV.Tables[0].Rows.Count > 0)
        {
            if (!string.IsNullOrEmpty(_dsV.Tables[0].Rows[0]["Address"].ToString()))
            {
                string add1 = Regex.Replace(_dsV.Tables[0].Rows[0]["Address"].ToString(), @"( |\r?\n)\1+", "$1");
                vendAddress = add1 + ", ";
            }

            if (!string.IsNullOrEmpty(_dsV.Tables[0].Rows[0]["City"].ToString()))
            {
                vendAddress2 += _dsV.Tables[0].Rows[0]["City"].ToString();
            }
            if (!string.IsNullOrEmpty(_dsV.Tables[0].Rows[0]["State"].ToString()) || !string.IsNullOrEmpty(_dsV.Tables[0].Rows[0]["Zip"].ToString()))
            {
                if (!string.IsNullOrEmpty(_dsV.Tables[0].Rows[0]["City"].ToString()))
                {
                    vendAddress2 += ", ";
                }
                vendAddress2 += _dsV.Tables[0].Rows[0]["State"].ToString() + " " + _dsV.Tables[0].Rows[0]["Zip"].ToString();
            }
        }
        string _currencyInWord = ConvertNumberToCurrency(totalAmount);
        string _amount = String.Format("{0:c}", totalAmount);
        _amount = _amount.Replace("$", string.Empty);
        _drC = _dtCheck.NewRow();
        _drC["Pay"] = _currencyInWord;
        _drC["ToOrder"] = txtPayee.Text;
        _drC["Date"] = txtCheckDate.Text;
        _drC["CheckAmount"] = _amount;
        _drC["ToOrderAddress"] = vendAddress;
        _drC["VendorAddress"] = _dsV.Tables[0].Rows[0]["VendorAddress"];
        _drC["State"] = vendAddress2;
        _drC["TotalAmountPay"] = SumAmountpay;
        _drC["Memo"] = txtMemo.Text;
        _dtCheck.Rows.Add(_drC);

        DataSet dsC = new DataSet();

        objPropUser.ConnConfig = Session["config"].ToString();



        if (Session["MSM"].ToString() != "TS")
        {

            dsC = objBL_User.getControl(objPropUser);

        }
        else
        {
            objPropUser.LocID = Convert.ToInt32(0);


            dsC = objBL_User.getControlBranch(objPropUser);


        }
        //dsBank

        CreateTableBank();

        DataRow _drB = null;
        DataRow _drA = null;
        _objBank.ConnConfig = Session["config"].ToString();
        _objBank.ID = Convert.ToInt32(hdnBankID.Value);



        DataSet _dsB = new DataSet();

        _dsB = _objBLBill.GetBankCD(_objBank);


        _drB = dtBank.NewRow();
        if (_dsB.Tables[0].Rows.Count > 0)
        {
            _drB["Name"] = _dsB.Tables[0].Rows[0]["Name"].ToString();
            _drB["Address"] = _dsB.Tables[0].Rows[0]["Address"].ToString();
            _drB["State"] = _dsB.Tables[0].Rows[0]["State"].ToString();
            _drB["City"] = _dsB.Tables[0].Rows[0]["City"].ToString();
            _drB["Zip"] = _dsB.Tables[0].Rows[0]["Zip"].ToString();
            _drB["NBranch"] = _dsB.Tables[0].Rows[0]["NBranch"].ToString();
            _drB["NAcct"] = _dsB.Tables[0].Rows[0]["NAcct"].ToString();
            _drB["NRoute"] = _dsB.Tables[0].Rows[0]["NRoute"].ToString();

        }

        string checkNumber = string.Empty;
        if (!string.IsNullOrEmpty(txtCheck.Text))
        {
            checkNumber = txtCheck.Text;
        }
        else
        {
            checkNumber = ViewState["Checkno"].ToString();
        }

        if (checkNumber.Length == 1)
        {
            _drB["Ref"] = "00000000" + checkNumber;
        }
        else if (checkNumber.Length == 2)
        {
            _drB["Ref"] = "0000000" + checkNumber;
        }
        else if (checkNumber.Length == 3)
        {
            _drB["Ref"] = "000000" + checkNumber;
        }
        else if (checkNumber.Length == 4)
        {
            _drB["Ref"] = "00000" + checkNumber;
        }
        else if (checkNumber.Length == 5)
        {
            _drB["Ref"] = "0000" + checkNumber;
        }
        else if (checkNumber.Length == 6)
        {
            _drB["Ref"] = "000" + checkNumber;
        }
        else if (checkNumber.Length == 7)
        {
            _drB["Ref"] = "00" + checkNumber;
        }
        else if (checkNumber.Length == 8)
        {
            _drB["Ref"] = "0" + checkNumber;
        }
        else
        {
            _drB["Ref"] = "000000000";
        }

        dtBank.Rows.Add(_drB);

        _objVendor.ConnConfig = Session["config"].ToString();
        _objVendor.ID = vid;



        DataSet _dsA = new DataSet();

        _dsA = _objBLVendor.GetVendorAcct(_objVendor);


        DataTable _dtAcct = new DataTable();
        _dtAcct.Columns.Add(new DataColumn("VendorID", typeof(Int32)));
        _dtAcct.Columns.Add(new DataColumn("VendorAcct", typeof(string)));

        _drA = _dtAcct.NewRow();
        _drA["VendorID"] = _dsA.Tables[0].Rows[0]["ID"].ToString();
        _drA["VendorAcct"] = _dsA.Tables[0].Rows[0]["Acct#"].ToString();
        _dtAcct.Rows.Add(_drA);


        var rowCount = 0;
        var totalRows = _dti.Rows.Count;
        if (reportName.Contains("-"))
        {
            try
            {
                string[] reportNameArr = reportName.Split('-');
                rowCount = Convert.ToInt32(reportNameArr[1].ToString().Trim().TrimStart());
                if (totalRows < rowCount)
                    rowCount = totalRows;
            }
            catch (Exception ex) { rowCount = totalRows; }
        }
        else
            rowCount = 9;
        DataView dv = _dti.DefaultView;
        dv.Sort = "Ref asc";
        DataTable sortedDT = dv.ToTable();
        var dtCopy = sortedDT.Copy();
        var firstHalf = dtCopy;
        var secondHalf = dtCopy;
        if (dtCopy.Rows.Count > rowCount)
        {
            firstHalf = dtCopy.AsEnumerable().Take(rowCount).CopyToDataTable();
            secondHalf = dtCopy.Clone();
            if (totalRows > rowCount)
            {
                secondHalf = dtCopy.AsEnumerable().Skip(rowCount).Take(totalRows - rowCount).CopyToDataTable();
            }
        }
        else
        {
            firstHalf = dtCopy;
        }


        //STIMULSOFT 
        byte[] buffer1 = null;
        string reportPathStimul = Server.MapPath("StimulsoftReports/APChecks/TopChecks/" + reportName.Trim() + ".mrt");
        StiReport report = new StiReport();
        report.Load(reportPathStimul);
        if (Convert.ToInt16(ViewState["CheckStatus"]).Equals(2))
        {
            report.Pages[0].Watermark.Enabled = true;
            //report.Pages[0].Watermark.Angle = 0;
            //report.Pages[0].Watermark.Text = "Void";
            string imagepath = Server.MapPath("images/icons/voidcheck.png");
            report.Pages[0].Watermark.Image = System.Drawing.Image.FromFile(imagepath);
            report.Pages[0].Watermark.ImageAlignment = System.Drawing.ContentAlignment.TopCenter;
            report.Pages[0].Watermark.ShowImageBehind = true;
        }
        report.Compile();
        report["TotalAmountPay"] = SumAmountpay;
        report["Memo"] = txtMemo.Text;

        report["InvoiceCount"] = totalRows;
        if (dsC.Tables[0].Rows.Count > 0)
        {
            report["CompanyName"] = dsC.Tables[0].Rows[0]["Name"].ToString();
            report["CompanyAddress"] = dsC.Tables[0].Rows[0]["Address"].ToString();
            report["CompanyCity"] = dsC.Tables[0].Rows[0]["City"].ToString() + "' " + dsC.Tables[0].Rows[0]["State"].ToString() + " - " + dsC.Tables[0].Rows[0]["Zip"].ToString();
        }
        DataSet Invoice = new DataSet();
        DataTable dtInvoice = firstHalf.Copy();
        dtInvoice.TableName = "Invoice";
        Invoice.Tables.Add(dtInvoice);
        Invoice.DataSetName = "Invoice";


        DataSet Check = new DataSet();
        DataTable dtCheck = _dtCheck;
        _dtCheck.TableName = "Check";
        Check.Tables.Add(_dtCheck);
        Check.DataSetName = "Check";

        DataSet ControlBranch = new DataSet();
        DataTable dtControlBranch = new DataTable();
        dtControlBranch = dsC.Tables[0].Copy();
        ControlBranch.Tables.Add(dtControlBranch);
        dtControlBranch.TableName = "ControlBranch";
        ControlBranch.DataSetName = "ControlBranch";


        DataSet Bank = new DataSet();
        DataTable _dtBank = dtBank;
        dtBank.TableName = "Bank";
        Bank.Tables.Add(dtBank);
        Bank.DataSetName = "Bank";

        DataSet Account = new DataSet();
        DataTable dtAccount = _dtAcct;
        _dtAcct.TableName = "Account";
        Account.Tables.Add(_dtAcct);
        Account.DataSetName = "Account";


        report.RegData("dsInvoices", Invoice);
        report.RegData("dsCheck", Check);
        report.RegData("dsTicket", ControlBranch);
        report.RegData("ControlBranch", ControlBranch);
        report.RegData("dsBank", Bank);
        report.RegData("dsAccount", Account);
        report.Render();
        if (isedit == false)
        {
            var settings = new Stimulsoft.Report.Export.StiPdfExportSettings();
            var service = new Stimulsoft.Report.Export.StiPdfExportService();
            System.IO.MemoryStream stream = new System.IO.MemoryStream();
            service.ExportTo(report, stream, settings);
            buffer1 = stream.ToArray();
            lstbyte.Add(buffer1);
        }



        if (totalRows > 9)
        {
            byte[] bufferNew = null;
            reportPathStimul = Server.MapPath("StimulsoftReports/TopCheckSubReport.mrt");
            StiReport subreport = new StiReport();
            subreport.Load(reportPathStimul);
            subreport.Compile();
            subreport["TotalAmountPay"] = SumAmountpay;
            subreport["AccountNo"] = hdnVendorAcct.Value.ToString();
            subreport["Memo"] = txtMemo.Text;

            subreport["InvoiceCount"] = totalRows;
            Invoice = new DataSet();
            var dtInvoice1 = new DataTable();
            dtInvoice1 = secondHalf.Copy();
            dtInvoice1.TableName = "Invoice";
            Invoice.Tables.Add(dtInvoice1);
            Invoice.DataSetName = "Invoice";


            Check = new DataSet();
            var dtCheck1 = new DataTable();
            dtCheck1 = _dtCheck.Copy();
            dtCheck1.TableName = "Check";
            Check.Tables.Add(dtCheck1);
            Check.DataSetName = "Check";

            ControlBranch = new DataSet();
            dtControlBranch = new DataTable();
            dtControlBranch = dsC.Tables[0].Copy();
            ControlBranch.Tables.Add(dtControlBranch);
            dtControlBranch.TableName = "ControlBranch";
            ControlBranch.DataSetName = "ControlBranch";


            Bank = new DataSet();
            var dtBank1 = new DataTable();
            dtBank1 = dtBank.Copy();
            dtBank1.TableName = "Bank";
            Bank.Tables.Add(dtBank1);
            Bank.DataSetName = "Bank";

            Account = new DataSet();
            var dtAccount1 = new DataTable();
            dtAccount1 = _dtAcct.Copy();
            dtAccount1.TableName = "Account";
            Account.Tables.Add(dtAccount1);
            Account.DataSetName = "Account";


            subreport.RegData("dsInvoices", Invoice);
            subreport.RegData("dsCheck", Check);
            subreport.RegData("dsTicket", ControlBranch);
            subreport.RegData("ControlBranch", ControlBranch);
            subreport.RegData("dsBank", Bank);
            subreport.RegData("dsAccount", Account);
            subreport.Render();
            if (isedit == false)
            {
                var settings = new Stimulsoft.Report.Export.StiPdfExportSettings();
                var service = new Stimulsoft.Report.Export.StiPdfExportService();
                System.IO.MemoryStream stream = new System.IO.MemoryStream();
                service.ExportTo(subreport, stream, settings);
                bufferNew = stream.ToArray();

                lstbyteNew.Add(bufferNew);
            }



        }
        if (isedit == false)
        {
            byte[] finalbyte = null;

            if (lstbyteNew.Count != 0)
            {
                finalbyte = EditCheck.concatAndAddContentFinal(lstbyte, lstbyteNew);
            }
            else
            {
                finalbyte = EditCheck.concatAndAddContent(lstbyte);
            }
            Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();
            Response.Buffer = true;
            Response.AddHeader("Content-Disposition", "attachment;filename=TopDetailCheckCub.pdf");
            Response.ContentType = "application/pdf";
            Response.AddHeader("Content-Length", (finalbyte.Length).ToString());
            Response.BinaryWrite(finalbyte);
        }

        return report;
    }
    private StiReport FillMiddleDataSetReport(string reportName, bool isedit)
    {

        double SumAmountpay = 0.00;
        DataTable _dti = new DataTable();
        DataRow _dri = null;
        _dti.Columns.Add(new DataColumn("Ref", typeof(string)));
        _dti.Columns.Add(new DataColumn("InvoiceDate", typeof(string)));
        _dti.Columns.Add(new DataColumn("Reference", typeof(string)));
        _dti.Columns.Add(new DataColumn("Total", typeof(double)));
        _dti.Columns.Add(new DataColumn("Disc", typeof(double)));
        _dti.Columns.Add(new DataColumn("AmountPay", typeof(double)));
        _dti.Columns.Add(new DataColumn("PayDate", typeof(string)));
        _dti.Columns.Add(new DataColumn("CheckNo", typeof(string)));
        _dti.Columns.Add(new DataColumn("VendorID", typeof(Int32)));
        _dti.Columns.Add(new DataColumn("VendorName", typeof(string)));
        _dti.Columns.Add(new DataColumn("VendorAcct", typeof(string)));

        //RAHIL
        _dti.Columns.Add(new DataColumn("Type", typeof(Int32)));
        _dti.Columns.Add(new DataColumn("Description", typeof(string)));

        //New column
        _dti.Columns.Add(new DataColumn("State", typeof(string)));
        _dti.Columns.Add(new DataColumn("ToOrderAddress", typeof(string)));
        _dti.Columns.Add(new DataColumn("CheckAmount", typeof(string)));
        _dti.Columns.Add(new DataColumn("Pay", typeof(string)));
        _dti.Columns.Add(new DataColumn("TotalAmount", typeof(string)));

        DataTable _dtCheck = new DataTable();
        DataRow _drC = null;
        _dtCheck.Columns.Add(new DataColumn("Pay", typeof(string)));
        _dtCheck.Columns.Add(new DataColumn("ToOrder", typeof(string)));
        _dtCheck.Columns.Add(new DataColumn("Date", typeof(string)));
        _dtCheck.Columns.Add(new DataColumn("CheckAmount", typeof(string)));
        _dtCheck.Columns.Add(new DataColumn("ToOrderAddress", typeof(string)));
        _dtCheck.Columns.Add(new DataColumn("State", typeof(string)));


        _dtCheck.Columns.Add(new DataColumn("VendorAddress", typeof(string)));
        _dtCheck.Columns.Add(new DataColumn("RemitAddress", typeof(string)));
        _dtCheck.Columns.Add(new DataColumn("Zip", typeof(string)));
        _dtCheck.Columns.Add(new DataColumn("TotalAmountPay", typeof(double)));
        _dtCheck.Columns.Add(new DataColumn("Memo", typeof(string)));


        int vid = Convert.ToInt32(hdnEmpID.Value);
        double totalAmount = 0;
        //foreach (GridViewRow gr in gvIncome.Rows)
        foreach (GridDataItem gr in gvIncome.Items)
        {
            Label lblPaid = (Label)gr.FindControl("lblPaid");
            Label lblBalance = (Label)gr.FindControl("lblBalance");
            Label lblDiscount = (Label)gr.FindControl("lblDiscount");
            Label lblOrig = (Label)gr.FindControl("lblOrig");
            Label lblRef = (Label)gr.FindControl("lblRef");
            Label lblfDate = (Label)gr.FindControl("lblfDate");
            Label lblDesc = (Label)gr.FindControl("lblDesc");

            _dri = _dti.NewRow();
            _dri["Ref"] = lblRef.Text;
            _dri["InvoiceDate"] = lblfDate.Text;
            _dri["Reference"] = lblDesc.Text;
            _dri["Description"] = lblDesc.Text;
            _dri["Total"] = double.Parse(lblOrig.Text.Replace('$', '0'), NumberStyles.AllowParentheses |
                            NumberStyles.AllowThousands |
                            NumberStyles.AllowDecimalPoint);
            _dri["Disc"] = double.Parse(lblDiscount.Text.Replace('$', '0'), NumberStyles.AllowParentheses |
                            NumberStyles.AllowThousands |
                            NumberStyles.AllowDecimalPoint);
            _dri["AmountPay"] = double.Parse(lblPaid.Text.Replace('$', '0'), NumberStyles.AllowParentheses |
                            NumberStyles.AllowThousands |
                            NumberStyles.AllowDecimalPoint);
            SumAmountpay = SumAmountpay + Convert.ToDouble(_dri["AmountPay"]);
            _dri["PayDate"] = txtCheckDate.Text;
            _dri["CheckNo"] = txtCheck.Text;
            if (!string.IsNullOrEmpty(hdnEmpID.Value))
            {
                _dri["VendorID"] = Convert.ToInt32(hdnEmpID.Value);
            }
            _dri["VendorName"] = txtPayee.Text;
            _dri["VendorAcct"] = hdnVendorAcct.Value.ToString();
            _dti.Rows.Add(_dri);
            totalAmount = totalAmount + double.Parse(lblPaid.Text.Replace('$', '0'), NumberStyles.AllowParentheses |
                              NumberStyles.AllowThousands |
                              NumberStyles.AllowDecimalPoint);
        }

        _objVendor.ConnConfig = Session["config"].ToString();
        _objVendor.ID = vid;



        DataSet _dsV = new DataSet();

        _dsV = _objBLVendor.GetVendorRolDetails(_objVendor);



        string vendAddress = "";
        string vendAddress2 = "";
        if (_dsV.Tables[0].Rows.Count > 0)
        {
            if (!string.IsNullOrEmpty(_dsV.Tables[0].Rows[0]["Address"].ToString()))
            {

                string add1 = Regex.Replace(_dsV.Tables[0].Rows[0]["Address"].ToString(), @"( |\r?\n)\1+", "$1");
                vendAddress = add1 + ", ";
            }

            if (!string.IsNullOrEmpty(_dsV.Tables[0].Rows[0]["City"].ToString()))
            {
                vendAddress2 += _dsV.Tables[0].Rows[0]["City"].ToString();
            }
            if (!string.IsNullOrEmpty(_dsV.Tables[0].Rows[0]["State"].ToString()) || !string.IsNullOrEmpty(_dsV.Tables[0].Rows[0]["Zip"].ToString()))
            {
                if (!string.IsNullOrEmpty(_dsV.Tables[0].Rows[0]["City"].ToString()))
                {
                    vendAddress2 += ", ";
                }
                vendAddress2 += _dsV.Tables[0].Rows[0]["State"].ToString() + " " + _dsV.Tables[0].Rows[0]["Zip"].ToString();
            }
        }
        string _currencyInWord = ConvertNumberToCurrency(totalAmount);
        string _amount = String.Format("{0:c}", totalAmount);
        _amount = _amount.Replace("$", string.Empty);
        _drC = _dtCheck.NewRow();
        _drC["Pay"] = _currencyInWord;
        _drC["ToOrder"] = txtPayee.Text;
        _drC["Date"] = txtCheckDate.Text;
        _drC["CheckAmount"] = _amount;
        _drC["ToOrderAddress"] = vendAddress;
        _drC["VendorAddress"] = vendAddress;
        _drC["State"] = vendAddress2;
        _drC["TotalAmountPay"] = SumAmountpay;
        _drC["Memo"] = txtMemo.Text;

        _dtCheck.Rows.Add(_drC);

        DataSet dsC = new DataSet();


        objPropUser.ConnConfig = Session["config"].ToString();

        if (Session["MSM"].ToString() != "TS")
        {

            dsC = objBL_User.getControl(objPropUser);

        }
        else
        {
            objPropUser.LocID = Convert.ToInt32(0);

            dsC = objBL_User.getControlBranch(objPropUser);

            //dsC = objBL_User.getControlBranch(objPropUser);
        }
        ReportViewer rvChecks = new ReportViewer();
        rvChecks.LocalReport.DataSources.Clear();
        //STIMULSOFT 
        var rowCount = 0;
        var totalRows = _dti.Rows.Count;
        if (reportName.Contains("-"))
        {
            try
            {
                string[] reportNameArr = reportName.Split('-');
                rowCount = Convert.ToInt32(reportNameArr[1].ToString().Trim().TrimStart());
                if (totalRows < rowCount)
                    rowCount = totalRows;
            }
            catch (Exception ex) { rowCount = totalRows; }
        }
        else
            rowCount = 6;
        DataView dv = _dti.DefaultView;
        dv.Sort = "Ref asc";
        DataTable sortedDT = dv.ToTable();
        var dtCopy = sortedDT.Copy();
        var firstHalf = dtCopy;
        var secondHalf = dtCopy;
        if (dtCopy.Rows.Count > rowCount)
        {
            firstHalf = dtCopy.AsEnumerable().Take(rowCount).CopyToDataTable();
            secondHalf = dtCopy.Clone();
            if (totalRows > rowCount)
            {
                secondHalf = dtCopy.AsEnumerable().Skip(rowCount).Take(totalRows - rowCount).CopyToDataTable();
            }
        }
        else
        {
            firstHalf = dtCopy;
        }




        byte[] buffer1 = null;
        string reportPathStimul = Server.MapPath("StimulsoftReports/APChecks/APMidCheck/" + reportName.Trim() + ".mrt");
        StiReport report = new StiReport();
        report.Load(reportPathStimul);
        if (Convert.ToInt16(ViewState["CheckStatus"]).Equals(2))
        {
            report.Pages[0].Watermark.Enabled = true;
            //report.Pages[0].Watermark.Angle = 0;
            //report.Pages[0].Watermark.Text = "Void";
            string imagepath = Server.MapPath("images/icons/voidcheck.png");
            report.Pages[0].Watermark.Image = System.Drawing.Image.FromFile(imagepath);
            report.Pages[0].Watermark.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            report.Pages[0].Watermark.ShowImageBehind = true;
        }
        report.Compile();
        report["TotalAmountPay"] = SumAmountpay;
        report["Memo"] = txtMemo.Text;

        report["InvoiceCount"] = totalRows;
        DataSet Invoice = new DataSet();
        DataTable dtInvoice = firstHalf.Copy();
        dtInvoice.TableName = "Invoice";
        Invoice.Tables.Add(dtInvoice);
        Invoice.DataSetName = "Invoice";

        DataSet Check = new DataSet();
        DataTable dtCheck = _dtCheck.Copy();
        dtCheck.TableName = "Check";
        Check.Tables.Add(dtCheck);
        Check.DataSetName = "Check";







        report.RegData("Invoice", Invoice);
        report.RegData("Check", Check);

        report.Render();
        if (isedit == false)
        {
            var settings = new Stimulsoft.Report.Export.StiPdfExportSettings();
            var service = new Stimulsoft.Report.Export.StiPdfExportService();
            System.IO.MemoryStream stream = new System.IO.MemoryStream();
            service.ExportTo(report, stream, settings);
            buffer1 = stream.ToArray();
            lstbyte.Add(buffer1);
        }



        if (totalRows > 6)
        {
            byte[] bufferNew = null;
            reportPathStimul = Server.MapPath("StimulsoftReports/TopCheckSubReport.mrt");
            StiReport subReport = new StiReport();
            subReport.Load(reportPathStimul);
            subReport.Compile();
            subReport["TotalAmountPay"] = SumAmountpay;
            subReport["AccountNo"] = hdnVendorAcct.Value.ToString();
            subReport["Memo"] = txtMemo.Text;

            subReport["InvoiceCount"] = totalRows;
            Invoice = new DataSet();
            dtInvoice = secondHalf.Copy();
            dtInvoice.TableName = "Invoice";
            Invoice.Tables.Add(dtInvoice);
            Invoice.DataSetName = "Invoice";

            Check = new DataSet();
            dtCheck = _dtCheck.Copy();
            dtCheck.TableName = "Check";
            Check.Tables.Add(dtCheck);
            Check.DataSetName = "Check";





            subReport.RegData("Invoice", Invoice);
            subReport.RegData("Check", Check);
            subReport.Render();
            if (isedit == false)
            {

                var settings = new Stimulsoft.Report.Export.StiPdfExportSettings();
                var service = new Stimulsoft.Report.Export.StiPdfExportService();
                MemoryStream stream = new System.IO.MemoryStream();
                service.ExportTo(subReport, stream, settings);
                bufferNew = stream.ToArray();

                lstbyteNew.Add(bufferNew);
            }
        }
        if (isedit == false)
        {

            byte[] finalbyte = null;
            if (lstbyteNew.Count != 0)
            {
                finalbyte = EditCheck.concatAndAddContentFinal(lstbyte, lstbyteNew);
            }
            else
            {
                finalbyte = EditCheck.concatAndAddContent(lstbyte);
            }
            Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();
            Response.Buffer = true;
            Response.AddHeader("Content-Disposition", "attachment;filename=MidCheckCub.pdf");
            Response.ContentType = "application/pdf";
            Response.AddHeader("Content-Length", (finalbyte.Length).ToString());
            Response.BinaryWrite(finalbyte);
        }


        return report;





    }
    private StiReport FillDataSetToReport(string reportName, bool isedit)
    {

        double SumAmountpay = 0.00;

        DataTable _dti = new DataTable();
        DataRow _dri = null;
        _dti.Columns.Add(new DataColumn("Ref", typeof(string)));
        _dti.Columns.Add(new DataColumn("InvoiceDate", typeof(string)));
        _dti.Columns.Add(new DataColumn("Reference", typeof(string)));
        _dti.Columns.Add(new DataColumn("Total", typeof(double)));
        _dti.Columns.Add(new DataColumn("Disc", typeof(double)));
        _dti.Columns.Add(new DataColumn("AmountPay", typeof(double)));
        _dti.Columns.Add(new DataColumn("PayDate", typeof(string)));
        _dti.Columns.Add(new DataColumn("CheckNo", typeof(string)));
        _dti.Columns.Add(new DataColumn("VendorID", typeof(Int32)));
        _dti.Columns.Add(new DataColumn("VendorName", typeof(string)));
        _dti.Columns.Add(new DataColumn("VendorAcct", typeof(string)));

        //RAHIL
        _dti.Columns.Add(new DataColumn("Type", typeof(Int32)));
        _dti.Columns.Add(new DataColumn("Description", typeof(string)));

        //New column
        _dti.Columns.Add(new DataColumn("State", typeof(string)));
        _dti.Columns.Add(new DataColumn("ToOrderAddress", typeof(string)));
        _dti.Columns.Add(new DataColumn("CheckAmount", typeof(string)));
        _dti.Columns.Add(new DataColumn("Pay", typeof(string)));
        _dti.Columns.Add(new DataColumn("TotalAmount", typeof(string)));

        DataTable _dtCheck = new DataTable();
        DataRow _drC = null;
        _dtCheck.Columns.Add(new DataColumn("Pay", typeof(string)));
        _dtCheck.Columns.Add(new DataColumn("ToOrder", typeof(string)));
        _dtCheck.Columns.Add(new DataColumn("Date", typeof(string)));
        _dtCheck.Columns.Add(new DataColumn("CheckAmount", typeof(string)));
        _dtCheck.Columns.Add(new DataColumn("ToOrderAddress", typeof(string)));
        _dtCheck.Columns.Add(new DataColumn("State", typeof(string)));
        _dtCheck.Columns.Add(new DataColumn("TotalAmountPay", typeof(double)));
        _dtCheck.Columns.Add(new DataColumn("Memo", typeof(string)));


        _dtCheck.Columns.Add(new DataColumn("VendorAddress", typeof(string)));
        _dtCheck.Columns.Add(new DataColumn("RemitAddress", typeof(string)));
        _dtCheck.Columns.Add(new DataColumn("Zip", typeof(string)));
        //_dtCheck.Columns.Add(new DataColumn("CheckStaus", typeof(string)));

        int vid = Convert.ToInt32(hdnEmpID.Value);
        double totalAmount = 0;
        //foreach (GridViewRow gr in gvIncome.Rows)
        foreach (GridDataItem gr in gvIncome.Items)
        {
            Label lblPaid = (Label)gr.FindControl("lblPaid");
            Label lblBalance = (Label)gr.FindControl("lblBalance");
            Label lblDiscount = (Label)gr.FindControl("lblDiscount");
            Label lblOrig = (Label)gr.FindControl("lblOrig");
            Label lblRef = (Label)gr.FindControl("lblRef");
            Label lblfDate = (Label)gr.FindControl("lblfDate");
            Label lblDesc = (Label)gr.FindControl("lblDesc");

            _dri = _dti.NewRow();
            _dri["Ref"] = lblRef.Text;
            _dri["InvoiceDate"] = lblfDate.Text;
            _dri["Reference"] = lblDesc.Text;
            _dri["Description"] = lblDesc.Text;
            _dri["Total"] = double.Parse(lblOrig.Text.Replace('$', '0'), NumberStyles.AllowParentheses |
                            NumberStyles.AllowThousands |
                            NumberStyles.AllowDecimalPoint);
            _dri["Disc"] = double.Parse(lblDiscount.Text.Replace('$', '0'), NumberStyles.AllowParentheses |
                            NumberStyles.AllowThousands |
                            NumberStyles.AllowDecimalPoint);
            _dri["AmountPay"] = double.Parse(lblPaid.Text.Replace('$', '0'), NumberStyles.AllowParentheses |
                            NumberStyles.AllowThousands |
                            NumberStyles.AllowDecimalPoint);
            SumAmountpay = SumAmountpay + Convert.ToDouble(_dri["AmountPay"]);
            _dri["PayDate"] = txtCheckDate.Text;
            _dri["CheckNo"] = txtCheck.Text;
            if (!string.IsNullOrEmpty(hdnEmpID.Value))
            {
                _dri["VendorID"] = Convert.ToInt32(hdnEmpID.Value);
            }
            _dri["VendorName"] = txtPayee.Text;
            _dri["VendorAcct"] = hdnVendorAcct.Value.ToString();
            _dti.Rows.Add(_dri);
            totalAmount = totalAmount + double.Parse(lblPaid.Text.Replace('$', '0'), NumberStyles.AllowParentheses |
                              NumberStyles.AllowThousands |
                              NumberStyles.AllowDecimalPoint);
        }
        _objVendor.ConnConfig = Session["config"].ToString();
        _objVendor.ID = vid;


        DataSet _dsV = new DataSet();

        _dsV = _objBLVendor.GetVendorRolDetails(_objVendor);


        string vendAddress = "";
        string vendAddress2 = "";
        if (_dsV.Tables[0].Rows.Count > 0)
        {
            if (!string.IsNullOrEmpty(_dsV.Tables[0].Rows[0]["Address"].ToString()))
            {
                string add1 = Regex.Replace(_dsV.Tables[0].Rows[0]["Address"].ToString(), @"( |\r?\n)\1+", "$1");
                vendAddress = add1 + ", ";
            }

            if (!string.IsNullOrEmpty(_dsV.Tables[0].Rows[0]["City"].ToString()))
            {
                vendAddress2 += _dsV.Tables[0].Rows[0]["City"].ToString();
            }
            if (!string.IsNullOrEmpty(_dsV.Tables[0].Rows[0]["State"].ToString()) || !string.IsNullOrEmpty(_dsV.Tables[0].Rows[0]["Zip"].ToString()))
            {
                if (!string.IsNullOrEmpty(_dsV.Tables[0].Rows[0]["City"].ToString()))
                {
                    vendAddress2 += ", ";
                }
                vendAddress2 += _dsV.Tables[0].Rows[0]["State"].ToString() + " " + _dsV.Tables[0].Rows[0]["Zip"].ToString();
            }
        }
        string _currencyInWord = ConvertNumberToCurrency(totalAmount);
        string _amount = String.Format("{0:c}", totalAmount);
        _amount = _amount.Replace("$", string.Empty);
        _drC = _dtCheck.NewRow();
        _drC["Pay"] = _currencyInWord;
        _drC["ToOrder"] = txtPayee.Text;
        _drC["Date"] = txtCheckDate.Text;
        _drC["CheckAmount"] = _amount;
        _drC["ToOrderAddress"] = vendAddress;
        _drC["VendorAddress"] = vendAddress;
        _drC["State"] = vendAddress2;
        _drC["TotalAmountPay"] = SumAmountpay;
        _drC["Memo"] = txtMemo.Text;
        _drC["RemitAddress"] = _dsV.Tables[0].Rows[0]["RemitAddress"];
        //_drC["CheckStaus"] = "Void";
        _dtCheck.Rows.Add(_drC);

        DataSet dsC = new DataSet();

        objPropUser.ConnConfig = Session["config"].ToString();


        if (Session["MSM"].ToString() != "TS")
        {

            dsC = objBL_User.getControl(objPropUser);

        }
        else
        {
            objPropUser.LocID = Convert.ToInt32(0);

            dsC = objBL_User.getControlBranch(objPropUser);

        }
        ReportViewer rvChecks = new ReportViewer();
        rvChecks.LocalReport.DataSources.Clear();
        var rowCount = 0;
        var totalRows = _dti.Rows.Count;
        if (reportName.Contains("-"))
        {
            try
            {
                string[] reportNameArr = reportName.Split('-');
                rowCount = Convert.ToInt32(reportNameArr[1].ToString().Trim().TrimStart());
                if (totalRows < rowCount)
                    rowCount = totalRows;
            }
            catch (Exception ex) { rowCount = totalRows; }
        }
        else
            rowCount = 6;
        DataView dv = _dti.DefaultView;
        dv.Sort = "Ref asc";
        DataTable sortedDT = dv.ToTable();
        var dtCopy = sortedDT.Copy();
        var firstHalf = dtCopy;
        var secondHalf = dtCopy;
        if (dtCopy.Rows.Count > rowCount)
        {
            firstHalf = dtCopy.AsEnumerable().Take(rowCount).CopyToDataTable();
            secondHalf = dtCopy.Clone();
            if (totalRows > rowCount)
            {
                secondHalf = dtCopy.AsEnumerable().Skip(rowCount).Take(totalRows - rowCount).CopyToDataTable();
            }
        }
        else
        {
            firstHalf = dtCopy;
        }


        //STIMULSOFT 
        byte[] buffer1 = null;
        string reportPathStimul = Server.MapPath("StimulsoftReports/APChecks/APTopCheck/" + reportName.Trim() + ".mrt");
        StiReport report = new StiReport();
        report.Load(reportPathStimul);

        if (Convert.ToInt16(ViewState["CheckStatus"]).Equals(2))
        {
            report.Pages[0].Watermark.Enabled = true;
            //report.Pages[0].Watermark.Angle = 0;
            //report.Pages[0].Watermark.Text = "Void";
            string imagepath = Server.MapPath("images/icons/voidcheck.png");
            report.Pages[0].Watermark.Image = System.Drawing.Image.FromFile(imagepath);
            report.Pages[0].Watermark.ImageAlignment = System.Drawing.ContentAlignment.TopCenter;
            report.Pages[0].Watermark.ShowImageBehind = true;
        }


        report.Compile();
        report["TotalAmountPay"] = SumAmountpay;
        report["Memo"] = txtMemo.Text;
        //report["CheckStaus"] = "Void";

        report["InvoiceCount"] = totalRows;
        DataSet Invoice = new DataSet();
        DataTable dtInvoice = firstHalf.Copy();
        dtInvoice.TableName = "Invoice";
        Invoice.Tables.Add(dtInvoice);
        Invoice.DataSetName = "Invoice";

        DataSet Check = new DataSet();
        DataTable dtCheck = _dtCheck;
        _dtCheck.TableName = "Check";
        Check.Tables.Add(_dtCheck);
        Check.DataSetName = "Check";

        DataSet ControlBranch = new DataSet();
        DataTable dtControlBranch = new DataTable();
        dtControlBranch = dsC.Tables[0].Copy();
        ControlBranch.Tables.Add(dtControlBranch);
        dtControlBranch.TableName = "ControlBranch";
        ControlBranch.DataSetName = "ControlBranch";

        report.RegData("dsInvoices", Invoice);
        report.RegData("dsCheck", Check);
        report.RegData("dsTicket", ControlBranch);
        report.Render();


        if (isedit == false)
        {
            var settings = new Stimulsoft.Report.Export.StiPdfExportSettings();
            var service = new Stimulsoft.Report.Export.StiPdfExportService();
            System.IO.MemoryStream stream = new System.IO.MemoryStream();
            service.ExportTo(report, stream, settings);
            buffer1 = stream.ToArray();
            lstbyte.Add(buffer1);
        }




        if (totalRows > rowCount)
        {
            byte[] bufferNew = null;
            reportPathStimul = Server.MapPath("StimulsoftReports/TopCheckSubReport.mrt");
            StiReport subreport = new StiReport();
            subreport.Load(reportPathStimul);
            subreport.Compile();
            subreport["TotalAmountPay"] = SumAmountpay;
            subreport["AccountNo"] = hdnVendorAcct.Value.ToString();
            subreport["Memo"] = txtMemo.Text;

            subreport["InvoiceCount"] = totalRows;
            Invoice = new DataSet();
            dtInvoice = secondHalf.Copy();
            dtInvoice.TableName = "Invoice";
            Invoice.Tables.Add(dtInvoice);
            Invoice.DataSetName = "Invoice";

            Check = new DataSet();
            DataTable dtCheck1 = new DataTable();
            dtCheck1 = _dtCheck.Copy();
            dtCheck1.TableName = "Check";
            Check.Tables.Add(dtCheck1);
            Check.DataSetName = "Check";

            ControlBranch = new DataSet();
            dtControlBranch = new DataTable();
            dtControlBranch = dsC.Tables[0].Copy();
            ControlBranch.Tables.Add(dtControlBranch);
            dtControlBranch.TableName = "ControlBranch";
            ControlBranch.DataSetName = "ControlBranch";


            subreport.RegData("dsInvoices", Invoice);
            subreport.RegData("dsCheck", Check);
            subreport.RegData("dsTicket", ControlBranch);
            subreport.Render();
            if (isedit == false)
            {
                var settings = new Stimulsoft.Report.Export.StiPdfExportSettings();
                var service = new Stimulsoft.Report.Export.StiPdfExportService();
                System.IO.MemoryStream stream = new System.IO.MemoryStream();
                service.ExportTo(subreport, stream, settings);
                bufferNew = stream.ToArray();

                lstbyteNew.Add(bufferNew);

            }


        }
        if (isedit == false)
        {
            byte[] finalbyte = null;

            if (lstbyteNew.Count != 0)
            {
                finalbyte = EditCheck.concatAndAddContentFinal(lstbyte, lstbyteNew);
            }
            else
            {
                finalbyte = EditCheck.concatAndAddContent(lstbyte);
            }
            Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();
            Response.Buffer = true;
            Response.AddHeader("Content-Disposition", "attachment;filename=ApTopCheckCub.pdf");
            Response.ContentType = "application/pdf";
            Response.AddHeader("Content-Length", (finalbyte.Length).ToString());
            Response.BinaryWrite(finalbyte);
        }

        return report;


    }
    protected void ImageButton14_Click(object sender, ImageClickEventArgs e)
    {
        string filePath = Server.MapPath("StimulsoftReports/APChecks/TopChecks");

        string selValue = ddlTopChecksForLoad.Text.Trim();
        if (selValue != null)
        {
            filePath = filePath + "\\" + selValue + ".mrt";
            if (!selValue.Equals("TopCheckReportDefault"))
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                ddlTopChecksForLoad.Items.Clear();

                string TopCheckpath = Server.MapPath("StimulsoftReports/APChecks/TopChecks/");
                DirectoryInfo dirTopcheckPath = new DirectoryInfo(TopCheckpath);
                FileInfo[] FilesTop = dirTopcheckPath.GetFiles("*.mrt");
                foreach (FileInfo fileTop in FilesTop)
                {
                    string FileName = string.Empty;
                    if (fileTop.Name.Contains(".mrt"))
                        FileName = fileTop.Name.Replace(".mrt", " ");
                    ddlTopChecksForLoad.Items.Add((FileName));
                }

                ddlTopChecksForLoad.DataBind();
                string str = "Template " + selValue + " Deleted!--";
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyProj", " window.parent.document.getElementById('btnCancel').click(); noty({text: '" + str + " </br> <b>', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : false});", true);
            }
        }
    }

    protected void imgPrintTemp5_Click(object sender, ImageClickEventArgs e)
    {                                                                       //                  BNN – check top
        try
        {
            var SumAmountpay = 0.00;
            DataTable _dti = new DataTable();
            DataRow _dri = null;
            _dti.Columns.Add(new DataColumn("Ref", typeof(string)));
            _dti.Columns.Add(new DataColumn("InvoiceDate", typeof(string)));
            _dti.Columns.Add(new DataColumn("Reference", typeof(string)));
            _dti.Columns.Add(new DataColumn("Total", typeof(string)));
            _dti.Columns.Add(new DataColumn("Disc", typeof(string)));
            _dti.Columns.Add(new DataColumn("AmountPay", typeof(string)));
            _dti.Columns.Add(new DataColumn("PayDate", typeof(string)));
            _dti.Columns.Add(new DataColumn("CheckNo", typeof(string)));
            _dti.Columns.Add(new DataColumn("VendorID", typeof(Int32)));
            _dti.Columns.Add(new DataColumn("VendorName", typeof(string)));
            _dti.Columns.Add(new DataColumn("VendorAcct", typeof(string)));

            //RAHIL
            _dti.Columns.Add(new DataColumn("Type", typeof(Int32)));
            _dti.Columns.Add(new DataColumn("Description", typeof(string)));


            DataTable _dtCheck = new DataTable();
            DataRow _drC = null;
            _dtCheck.Columns.Add(new DataColumn("Pay", typeof(string)));
            _dtCheck.Columns.Add(new DataColumn("ToOrder", typeof(string)));
            _dtCheck.Columns.Add(new DataColumn("Date", typeof(string)));
            _dtCheck.Columns.Add(new DataColumn("CheckAmount", typeof(string)));
            _dtCheck.Columns.Add(new DataColumn("ToOrderAddress", typeof(string)));
            _dtCheck.Columns.Add(new DataColumn("State", typeof(string)));
            int vid = Convert.ToInt32(hdnEmpID.Value);
            //foreach (GridViewRow gr in gvIncome.Rows)
            foreach (GridDataItem gr in gvIncome.Items)
            {
                CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");
                if (chkSelect.Checked == true)
                {
                    TextBox txtGvPay = (TextBox)gr.FindControl("txtGvPay");
                    Label lblBalance = (Label)gr.FindControl("lblBalance");
                    TextBox txtGvDisc = (TextBox)gr.FindControl("txtGvDisc");
                    Label lblOrig = (Label)gr.FindControl("lblOrig");
                    HiddenField hdnRef = (HiddenField)gr.FindControl("hdnRef");
                    Label lblfDate = (Label)gr.FindControl("lblfDate");
                    Label lblRef = (Label)gr.FindControl("lblRef");
                    Label lblDesc = (Label)gr.FindControl("lblDesc");

                    _dri = _dti.NewRow();
                    _dri["Ref"] = hdnRef.Value;
                    _dri["Description"] = lblDesc.Text;
                    //RAHIL
                    //_objOpenAP.Ref = hdnRef.Value;
                    //DataSet _dsCheck = _objBLBill.GetCheckDetails(_objOpenAP);

                    //if (_dsCheck.Tables[0].Rows.Count > 0)
                    //{
                    //    _dri["Description"] = _dsCheck.Tables[0].Rows[0]["fDesc"].ToString();
                    //}

                    //DataRow[] dr = gds.Tables[0].Select("Ref='" + hdnRef.Value + "'");                   

                    _dri["InvoiceDate"] = lblfDate.Text;
                    _dri["Reference"] = lblRef.Text;

                    _dri["Total"] = double.Parse(lblOrig.Text.Replace('$', '0'), NumberStyles.AllowParentheses |
                                  NumberStyles.AllowThousands |
                                  NumberStyles.AllowDecimalPoint);
                    _dri["Disc"] = Convert.ToDouble(txtGvDisc.Text).ToString();
                    _dri["AmountPay"] = Convert.ToDouble(txtGvPay.Text).ToString();
                    SumAmountpay = SumAmountpay + Convert.ToDouble(_dri["AmountPay"]);
                    _dri["PayDate"] = txtCheckDate.Text;
                    if (!string.IsNullOrEmpty(txtCheck.Text))
                    {
                        _dri["CheckNo"] = txtCheck.Text;
                    }
                    else
                    {
                        _dri["CheckNo"] = ViewState["Checkno"].ToString();
                    }
                    _dri["VendorID"] = Convert.ToInt32(hdnEmpID.Value);
                    _dri["VendorName"] = txtPayee.Text;
                    _dri["VendorAcct"] = hdnVendorAcct.Value.ToString();
                    _dti.Rows.Add(_dri);
                }
            }
            _objVendor.ConnConfig = Session["config"].ToString();
            _objVendor.ID = vid;


            DataSet _dsV = new DataSet();

            _dsV = _objBLVendor.GetVendorRolDetails(_objVendor);

            string vendAddress = "";
            string vendAddress2 = "";
            if (_dsV.Tables[0].Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(_dsV.Tables[0].Rows[0]["Address"].ToString()))
                {
                    vendAddress = _dsV.Tables[0].Rows[0]["Address"].ToString() + ", ";
                }

                if (!string.IsNullOrEmpty(_dsV.Tables[0].Rows[0]["City"].ToString()))
                {
                    vendAddress2 += _dsV.Tables[0].Rows[0]["City"].ToString();
                }
                if (!string.IsNullOrEmpty(_dsV.Tables[0].Rows[0]["State"].ToString()) || !string.IsNullOrEmpty(_dsV.Tables[0].Rows[0]["Zip"].ToString()))
                {
                    if (!string.IsNullOrEmpty(_dsV.Tables[0].Rows[0]["City"].ToString()))
                    {
                        vendAddress2 += ", ";
                    }
                    vendAddress2 += _dsV.Tables[0].Rows[0]["State"].ToString() + " " + _dsV.Tables[0].Rows[0]["Zip"].ToString();
                }
            }
            _drC = _dtCheck.NewRow();
            _drC["Pay"] = ViewState["Dollar"].ToString();
            _drC["ToOrder"] = ViewState["Vendor"].ToString();
            _drC["Date"] = txtCheckDate.Text;
            _drC["CheckAmount"] = Convert.ToDouble(ViewState["Amount"]).ToString("0.00", CultureInfo.InvariantCulture);
            _drC["ToOrderAddress"] = vendAddress;
            _drC["VendorAddress"] = vendAddress;
            _drC["State"] = vendAddress2;
            _dtCheck.Rows.Add(_drC);

            DataSet dsC = new DataSet();


            objPropUser.ConnConfig = Session["config"].ToString();



            if (Session["MSM"].ToString() != "TS")
            {

                dsC = objBL_User.getControl(objPropUser);

            }
            else
            {
                objPropUser.LocID = Convert.ToInt32(0);


                dsC = objBL_User.getControlBranch(objPropUser);


            }
            ReportViewer rvChecks = new ReportViewer();
            rvChecks.LocalReport.DataSources.Clear();

            rvChecks.LocalReport.DataSources.Add(new ReportDataSource("dsInvoices", _dti));
            rvChecks.LocalReport.DataSources.Add(new ReportDataSource("dsCheck", _dtCheck));

            rvChecks.LocalReport.DataSources.Add(new ReportDataSource("dsTicket", dsC.Tables[0]));

            string reportPath = "Reports/MaddReportCheck.rdlc";

            rvChecks.LocalReport.ReportPath = reportPath;

            rvChecks.LocalReport.EnableExternalImages = true;
            List<Microsoft.Reporting.WebForms.ReportParameter> param1 = new List<Microsoft.Reporting.WebForms.ReportParameter>();
            string strPath = "file:///" + Server.MapPath(Request.ApplicationPath).Replace("\\", "/");
            param1.Add(new Microsoft.Reporting.WebForms.ReportParameter("Path", strPath + "/images/Company_logo.jpg"));

            rvChecks.LocalReport.SetParameters(param1);

            rvChecks.LocalReport.Refresh();

            byte[] buffer = null;
            buffer = ExportReportToPDF("", rvChecks);
            Response.ClearContent();
            Response.ClearHeaders();
            Response.AddHeader("Content-Disposition", "attachment;filename=MaddCheckBBN.pdf");
            Response.ContentType = "application/pdf";
            Response.AddHeader("Content-Length", (buffer.Length).ToString());
            Response.BinaryWrite(buffer);
            Response.Flush();
            Response.Close();

        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true})", true);
        }

    }

    public static byte[] concatAndAddContent(List<byte[]> pdfByteContent)
    {
        MemoryStream ms = new MemoryStream();
        Document doc = new Document();
        PdfSmartCopy copy = new PdfSmartCopy(doc, ms);

        doc.Open();

        //Loop through each byte array
        foreach (var p in pdfByteContent)
        {
            PdfReader reader = new PdfReader(p);
            int n = reader.NumberOfPages;

            for (int i = 1; i <= n; i++)
            {
                byte[] red = reader.GetPageContent(i);
                if (red.Length < 1000)
                {
                    n = n - 1;
                }
            }
            for (int page = 0; page < n;)
            {
                copy.AddPage(copy.GetImportedPage(reader, ++page));
            }
        }
        doc.Close();
        //Return just before disposing
        return ms.ToArray();
    }

    public static byte[] concatAndAddContentFinal(List<byte[]> pdfByteContentA, List<byte[]> pdfByteContentB)
    {
        MemoryStream ms = new MemoryStream();
        Document doc = new Document();
        PdfSmartCopy copy = new PdfSmartCopy(doc, ms);

        doc.Open();

        //Loop through each byte array
        foreach (var p in pdfByteContentA)
        {
            PdfReader reader = new PdfReader(p);
            int n = reader.NumberOfPages;

            for (int i = 1; i <= n; i++)
            {
                byte[] red = reader.GetPageContent(i);
                if (red.Length < 1000)
                {
                    n = n - 1;
                }
                for (int page = 0; page < n;)
                {
                    copy.AddPage(copy.GetImportedPage(reader, ++page));
                }
            }
        }
        foreach (var p1 in pdfByteContentB)
        {
            PdfReader reader1 = new PdfReader(p1);
            int n = reader1.NumberOfPages;

            for (int i = 1; i <= n; i++)
            {
                byte[] red = reader1.GetPageContent(i);
                if (red.Length < 1000)
                {
                    n = n - 1;
                }
            }
            for (int page = 0; page < n;)
            {
                copy.AddPage(copy.GetImportedPage(reader1, ++page));
            }

            doc.Close();
        }
        //Return just before disposing
        return ms.ToArray();

    }

    protected void StiWebDesigner1_SaveReport(object sender, Stimulsoft.Report.Web.StiSaveReportEventArgs e)
    {

        //ReportModalPopupExtender.Hide();
        StiReport oRep = e.Report;
        e.Report.Save(Server.MapPath("StimulsoftReports/APChecks/APTopCheck/" + e.FileName));

    }

    protected void StiWebDesigner2_SaveReport(object sender, Stimulsoft.Report.Web.StiSaveReportEventArgs e)
    {

        //ReportModalPopupExtender1.Hide();
        StiReport oRep = e.Report;
        e.Report.Save(Server.MapPath("StimulsoftReports/APChecks/APMidCheck/" + e.FileName));

    }

    protected void StiWebDesigner3_SaveReport(object sender, Stimulsoft.Report.Web.StiSaveReportEventArgs e)
    {

        //ReportModalPopupExtender2.Hide();
        StiReport oRep = e.Report;
        e.Report.Save(Server.MapPath("StimulsoftReports/APChecks/TopChecks/" + e.FileName));

    }

    protected void ImageButton7_Click(object sender, ImageClickEventArgs e)
    {

        //mpeTemplate.Hide();
        string reportName = ddlApTopCheckForLoad.SelectedItem.Text.Trim();
        StiReport report = FillDataSetToReport(reportName, true);
        StiWebDesigner1.Report = report;
        // ReportModalPopupExtender.Show();
        Session["wc_first_edit"] = "true";
        StiWebDesigner1.Visible = true;
    }

    protected void ImageButton8_Click(object sender, ImageClickEventArgs e)
    {
        //mpeTemplate.Hide();

        string reportName = ddlApMiddleCheckForLoad.SelectedItem.Text.Trim();
        StiReport report = FillMiddleDataSetReport(reportName, true);
        StiWebDesigner2.Report = report;
        //ReportModalPopupExtender1.Show();
        Session["wc_second_edit"] = "true";
        StiWebDesigner2.Visible = true;
    }

    protected void ImageButton9_Click(object sender, ImageClickEventArgs e)
    {

        string reportName = ddlTopChecksForLoad.SelectedItem.Text.Trim();
        //mpeTemplate.Hide();
        StiReport report = FillMaddenDataSetForReport(reportName, true);
        StiWebDesigner3.Report = report;
        //ReportModalPopupExtender2.Show();
        Session["wc_third_edit"] = "true";
        StiWebDesigner3.Visible = true;


    }
    protected void ImageButton3_Click(object sender, ImageClickEventArgs e)
    {

        string filePath = Server.MapPath("StimulsoftReports/APChecks/APTopCheck");

        string selValue = ddlApTopCheckForLoad.Text.Trim();
        if (selValue != null)
        {
            filePath = filePath + "\\" + selValue + ".mrt";
            if (!selValue.Equals("ApTopCheckDefault"))
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                ddlApTopCheckForLoad.Items.Clear();
                string path = Server.MapPath("StimulsoftReports/APChecks/APTopCheck/");
                DirectoryInfo d = new DirectoryInfo(path);
                FileInfo[] Files = d.GetFiles("*.mrt");
                foreach (FileInfo file in Files)
                {
                    string FileName = string.Empty;
                    if (file.Name.Contains(".mrt"))
                        FileName = file.Name.Replace(".mrt", " ");
                    ddlApTopCheckForLoad.Items.Add((FileName));
                }
                ddlApTopCheckForLoad.Items.Remove((selValue));
                ddlApTopCheckForLoad.DataBind();
                string str = "Template " + selValue + " Deleted!--";
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyProj", " window.parent.document.getElementById('btnCancel').click(); noty({text: '" + str + " </br> <b>', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : false});", true);
            }
        }
    }
    protected void lnkTopChecks_Click(object sender, EventArgs e)
    {

        try
        {
            string defaultpath = Server.MapPath("StimulsoftReports/APChecks/TopChecks/TopCheckReportDefault.mrt");
            string filePath = Server.MapPath("StimulsoftReports/APChecks/TopChecks");
            string tempPath = Server.MapPath("StimulsoftReports/APChecks/TopChecks");
            string selValue = ddlTopChecksForLoad.Text.TrimEnd();
            if (selValue != null)
            {
                filePath = filePath + "\\" + selValue + ".mrt";
                tempPath = tempPath + "\\" + selValue + "temp.mrt";
                if (File.Exists(defaultpath))
                {
                    string[] lines = System.IO.File.ReadAllLines(defaultpath);
                    var myfile = File.Create(tempPath);
                    myfile.Close();
                    using (TextWriter tw = new StreamWriter(tempPath))
                        foreach (string line in lines)
                        {
                            tw.WriteLine(line);
                        }
                    File.Delete(defaultpath);
                    if (File.Exists(filePath))
                    {
                        string[] lines1 = System.IO.File.ReadAllLines(filePath);
                        var myfile1 = File.Create(defaultpath);
                        myfile1.Close();
                        using (TextWriter tw1 = new StreamWriter(defaultpath))
                            foreach (string line1 in lines1)
                            {
                                tw1.WriteLine(line1);
                            }
                        File.Delete(filePath);
                    }
                    if (File.Exists(tempPath))
                    {
                        string[] lines2 = System.IO.File.ReadAllLines(tempPath);
                        var myfile2 = File.Create(filePath);
                        myfile2.Close();
                        using (TextWriter tw2 = new StreamWriter(filePath))
                            foreach (string line2 in lines2)
                            {
                                tw2.WriteLine(line2);
                            }
                        File.Delete(tempPath);
                    }
                    if (Request.QueryString["id"] != null)
                    {
                        Response.Redirect("EditCheck.aspx?id=" + Request.QueryString["id"].ToString());
                    }

                }
                else
                    throw new Exception("TopCheckReportDefault.mrt is not available");

            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true})", true);
        }

    }
    protected void lnkSaveDefault_Click(object sender, EventArgs e)
    {
        try
        {
            string defaultpath = Server.MapPath("StimulsoftReports/APChecks/APTopCheck/ApTopCheckDefault.mrt");
            string filePath = Server.MapPath("StimulsoftReports/APChecks/APTopCheck");
            string tempPath = Server.MapPath("StimulsoftReports/APChecks/APTopCheck");
            string selValue = ddlApTopCheckForLoad.Text.TrimEnd();
            if (selValue != null)
            {
                filePath = filePath + "\\" + selValue + ".mrt";
                tempPath = tempPath + "\\" + selValue + "temp.mrt";
                if (File.Exists(defaultpath))
                {
                    string[] lines = System.IO.File.ReadAllLines(defaultpath);
                    var myfile = File.Create(tempPath);
                    myfile.Close();
                    using (TextWriter tw = new StreamWriter(tempPath))
                        foreach (string line in lines)
                        {
                            tw.WriteLine(line);
                        }
                    File.Delete(defaultpath);
                    if (File.Exists(filePath))
                    {
                        string[] lines1 = System.IO.File.ReadAllLines(filePath);
                        var myfile1 = File.Create(defaultpath);
                        myfile1.Close();
                        using (TextWriter tw1 = new StreamWriter(defaultpath))
                            foreach (string line1 in lines1)
                            {
                                tw1.WriteLine(line1);
                            }
                        File.Delete(filePath);
                    }
                    if (File.Exists(tempPath))
                    {
                        string[] lines2 = System.IO.File.ReadAllLines(tempPath);
                        var myfile2 = File.Create(filePath);
                        myfile2.Close();
                        using (TextWriter tw2 = new StreamWriter(filePath))
                            foreach (string line2 in lines2)
                            {
                                tw2.WriteLine(line2);
                            }
                        File.Delete(tempPath);
                    }
                    if (Request.QueryString["id"] != null)
                    {
                        Response.Redirect("EditCheck.aspx?id=" + Request.QueryString["id"].ToString());
                    }

                }
                else
                    throw new Exception("ApTopCheckDefault.mrt is not available");

            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true})", true);
        }


    }
    protected void ImageButton6_Click(object sender, ImageClickEventArgs e)
    {

        string filePath = Server.MapPath("StimulsoftReports/APChecks/APMidCheck");

        string selValue = ddlApMiddleCheckForLoad.Text.Trim();
        if (selValue != null)
        {
            filePath = filePath + "\\" + selValue + ".mrt";
            if (!selValue.Equals("APMidCheckDefault"))
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                ddlApMiddleCheckForLoad.Items.Clear();
                string MidCheckpath = Server.MapPath("StimulsoftReports/APChecks/APMidCheck/");
                DirectoryInfo dirMidPath = new DirectoryInfo(MidCheckpath);
                FileInfo[] FilesMid = dirMidPath.GetFiles("*.mrt");
                foreach (FileInfo fileMid in FilesMid)
                {
                    string FileName = string.Empty;
                    if (fileMid.Name.Contains(".mrt"))
                        FileName = fileMid.Name.Replace(".mrt", " ");
                    ddlApMiddleCheckForLoad.Items.Add((FileName));
                }
                ddlApMiddleCheckForLoad.Items.Remove((selValue));
                ddlApMiddleCheckForLoad.DataBind();
                string str = "Template " + selValue + " Deleted!--";
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyProj", " window.parent.document.getElementById('btnCancel').click(); noty({text: '" + str + " </br> <b>', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : false});", true);
            }
        }
    }
    protected void lnkSaveApMiddleCheck_Click(object sender, EventArgs e)
    {

        try
        {
            string defaultpath = Server.MapPath("StimulsoftReports/APChecks/APMidCheck/APMidCheckDefault.mrt");
            string filePath = Server.MapPath("StimulsoftReports/APChecks/APMidCheck");
            string tempPath = Server.MapPath("StimulsoftReports/APChecks/APMidCheck");

            string selValue = ddlApMiddleCheckForLoad.Text.TrimEnd();
            if (selValue != null)
            {
                filePath = filePath + "\\" + selValue + ".mrt";
                tempPath = tempPath + "\\" + selValue + "temp.mrt";
                if (File.Exists(defaultpath))
                {
                    string[] lines = System.IO.File.ReadAllLines(defaultpath);
                    var myfile = File.Create(tempPath);
                    myfile.Close();
                    using (TextWriter tw = new StreamWriter(tempPath))
                        foreach (string line in lines)
                        {
                            tw.WriteLine(line);
                        }
                    File.Delete(defaultpath);
                    if (File.Exists(filePath))
                    {
                        string[] lines1 = System.IO.File.ReadAllLines(filePath);
                        var myfile1 = File.Create(defaultpath);
                        myfile1.Close();
                        using (TextWriter tw1 = new StreamWriter(defaultpath))
                            foreach (string line1 in lines1)
                            {
                                tw1.WriteLine(line1);
                            }
                        File.Delete(filePath);
                    }
                    if (File.Exists(tempPath))
                    {
                        string[] lines2 = System.IO.File.ReadAllLines(tempPath);
                        var myfile2 = File.Create(filePath);
                        myfile2.Close();
                        using (TextWriter tw2 = new StreamWriter(filePath))
                            foreach (string line2 in lines2)
                            {
                                tw2.WriteLine(line2);
                            }
                        File.Delete(tempPath);
                    }
                    if (Request.QueryString["id"] != null)
                    {
                        Response.Redirect("EditCheck.aspx?id=" + Request.QueryString["id"].ToString());
                    }
                }
                else
                    throw new Exception("ApMiddleCheckDefault.mrt is not available");

            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true})", true);
        }


    }

    protected void StiWebDesigner1_SaveReportAs(object sender, Stimulsoft.Report.Web.StiSaveReportEventArgs e)
    {
        StiReport oRep = e.Report;
        e.Report.Save(Server.MapPath("StimulsoftReports/APChecks/APTopCheck/" + e.FileName));
    }

    protected void StiWebDesigner2_SaveReportAs(object sender, Stimulsoft.Report.Web.StiSaveReportEventArgs e)
    {
        StiReport oRep = e.Report;
        e.Report.Save(Server.MapPath("StimulsoftReports/APChecks/APMidCheck/" + e.FileName));
    }

    protected void StiWebDesigner3_SaveReportAs(object sender, Stimulsoft.Report.Web.StiSaveReportEventArgs e)
    {
        StiReport oRep = e.Report;
        e.Report.Save(Server.MapPath("StimulsoftReports/APChecks/TopChecks/" + e.FileName));
    }

    protected void StiWebDesigner1_Exit(object sender, Stimulsoft.Report.Web.StiReportDataEventArgs e)
    {
        if (Request.QueryString["id"] != null)
        {
            Response.Redirect("EditCheck.aspx?id=" + Request.QueryString["id"].ToString());
        }
    }

    protected void StiWebDesigner2_Exit(object sender, Stimulsoft.Report.Web.StiReportDataEventArgs e)
    {
        if (Request.QueryString["id"] != null)
        {
            Response.Redirect("EditCheck.aspx?id=" + Request.QueryString["id"].ToString());
        }

    }

    protected void StiWebDesigner3_Exit(object sender, Stimulsoft.Report.Web.StiReportDataEventArgs e)
    {
        if (Request.QueryString["id"] != null)
        {
            Response.Redirect("EditCheck.aspx?id=" + Request.QueryString["id"].ToString());
        }
    }

    protected void lnkEditApTopCheck_Click(object sender, EventArgs e)
    {
        //mpeTemplate.Hide();
        StiReport report = FillDataSetToReport("APTopCheckDefault", true);
        StiWebDesigner1.Report = report;
        //ReportModalPopupExtender.Show();
        Session["wc_first_edit"] = "true";
        StiWebDesigner1.Visible = true;
    }

    #endregion
    protected void btnPrintCheck_Click(object sender, EventArgs e)
    {

        //if (Convert.ToInt16(ViewState["CheckStatus"]).Equals(2))            //Void check
        //{
        //    //ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: 'Check is voided,',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", false);
        //    string script = "noty({text: 'Check is voided.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});";
        //    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
        //}
        //else
        //{
        string script = "function f(){$find(\"" + RadWindowTemplates.ClientID + "\").show(); Sys.Application.remove_load(f);}Sys.Application.add_load(f); Materialize.updateTextFields();";
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
        //}
    }
    #region logs
    protected void gvBreakdown_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        //try
        //{
        //    gvBreakdown.AllowCustomPaging = !ShouldApplySortFilterOrGroupLogs();
        //    if (Request.QueryString["id"] != null)
        //    {
        //        DataSet dsLog = new DataSet();
        //        _objCD.ConnConfig = Session["config"].ToString();
        //        //_objCD.Ref = long.Parse(txtCheck.Text);
        //        _objCD.Ref = long.Parse(Request.QueryString["id"]);


        //            dsLog = _objBLBill.GetAPCheckLogs(_objCD);

        //        if (dsLog.Tables[0].Rows.Count > 0)
        //        {
        //            gvBreakdown.VirtualItemCount = dsLog.Tables[0].Rows.Count;
        //            gvBreakdown.DataSource = dsLog.Tables[0];
        //        }
        //        else
        //        {
        //            gvBreakdown.DataSource = string.Empty;
        //        }
        //    }
        //}
        //catch { }
    }
    bool isGroupLog = false;
    public bool ShouldApplySortFilterOrGroupLogs()
    {
        return gvBreakdown.MasterTableView.FilterExpression != "" ||
            (gvBreakdown.MasterTableView.GroupByExpressions.Count > 0 || isGroupLog) ||
            gvBreakdown.MasterTableView.SortExpressions.Count > 0;
    }
    protected void gvBreakdown_ItemCreated(object sender, GridItemEventArgs e)
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
    #endregion


    double totalQuan = 0;
    double totalRate = 0;
    double totalAmt = 0;
    double totalAmtYTD = 0;

    protected void gvIncome_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem dataItem = e.Item as GridDataItem;
                Label lblRQuan = (Label)dataItem.FindControl("lblQuan");
                Label lblRAmt = (Label)dataItem.FindControl("lblAmount");
                Label lblRAmtYTD = (Label)dataItem.FindControl("lblYTD");
                totalQuan += Convert.ToDouble(lblRQuan.Text);
                totalAmt += Convert.ToDouble(lblRAmt.Text);
                totalAmtYTD += Convert.ToDouble(lblRAmtYTD.Text);
            }
            if (e.Item is GridFooterItem)
            {
                GridFooterItem footerItem = e.Item as GridFooterItem;
                footerItem["Amount"].Text = totalAmt.ToString();
                footerItem["YTD"].Text = totalAmtYTD.ToString();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }

    protected void gvDeduction_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem dataItem = e.Item as GridDataItem;
                Label lblDAmt = (Label)dataItem.FindControl("lblAmount");
                Label lblDAmtYTD = (Label)dataItem.FindControl("lblYTD");
                totalAmt += Convert.ToDouble(lblDAmt.Text);
                totalAmtYTD += Convert.ToDouble(lblDAmtYTD.Text);
            }
            if (e.Item is GridFooterItem)
            {
                GridFooterItem footerItem = e.Item as GridFooterItem;
                footerItem["Amount"].Text = totalAmt.ToString();
                footerItem["YTD"].Text = totalAmtYTD.ToString();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }
}
