using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLayer;
using BusinessEntity;
using System.Data;
using Telerik.Web.UI;
using System.Web.Services;
using System.Collections.Generic;
using Microsoft.ApplicationBlocks.Data;
using System.Web;
using BusinessLayer.Programs;
using BusinessEntity.Programs;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Reflection;

public partial class WageCategory : System.Web.UI.Page
{

    BL_User objBL_User = new BL_User();
    BL_Wage objBL_Wage = new BL_Wage();
    User objProp_User = new User();
    PRDed objProp_PRDed = new PRDed();
    BL_Company objBL_Company = new BL_Company();
    BusinessEntity.CompanyOffice objCompany = new BusinessEntity.CompanyOffice();
    //consult
    tblConsult objProp_Consult = new tblConsult();
    BL_General objBL_General = new BL_General();
    General objGeneral = new General();
    BL_Customer objBL_Customer = new BL_Customer();
    BL_Vendor objBL_Vendor = new BL_Vendor();
    Customer objCustomer = new Customer();
    BL_ReportsData objBL_ReportData = new BL_ReportsData();
    GeneralFunctions objGeneralFunctions = new GeneralFunctions();
    public static bool IsAddEdit = false;
    public static bool IsDelete = false;
    Wage _objWage = new Wage();
    bool api = false;


    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userid"] == null)
        {
            Response.Redirect("login.aspx");
        }
        string _connectionString = Session["config"].ToString();
        try
        {
            if (this.IsPostBack) //just write this
                return;

            if (!IsPostBack)
            {
                userpermissions();
                CompanyPermission();
                ViewState["editcon"] = 0;
                ViewState["mode"] = 0;

                //FillCountry();
                if (Request.QueryString["id"] == null)
                {
                    Page.Title = "Add Wage Category || MOM";
                    ViewState["edit"] = "0";
                    // ClearControl();
                    try
                    {
                        DataSet ds = new DataSet();
                        _objWage.ConnConfig = Session["config"].ToString();
                        ds = new BL_Wage().getWageTypes(_objWage);

                        List<WageRateType> wageRateTypes = new List<WageRateType>();
                        wageRateTypes = (from DataRow dr in ds.Tables[0].Rows
                                         select new WageRateType()
                                         {
                                             WageRateTypeId = Convert.ToInt32(dr["WageRateTypeId"]),
                                             WageRateTypeName = dr["WageRateTypeName"].ToString(),
                                         }).ToList();

                        rptPayRate.DataSource = wageRateTypes;
                        rptPayRate.DataBind();
                        rptBurdenRate.DataSource = wageRateTypes;
                        rptBurdenRate.DataBind();
                    }
                    catch (Exception ex)
                    {
                        string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
                        ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                    }
                }
                if (Request.QueryString["id"] != null)  //Edit COA
                {
                    if (Request.QueryString["t"] != null)
                    {
                        Page.Title = "Add Wage Category || MOM";

                        ViewState["mode"] = 0;
                        lblHeader.Text = "Add Wage Category";
                        ViewState["edit"] = "0";
                        GetEditData(Request.QueryString["id"].ToString());
                        hdnWageID.Value = "";
                    }
                    else
                    {
                        Page.Title = "Edit Wage Category || MOM";
                        ViewState["mode"] = 1;
                        lblHeader.Text = "Edit Wage Category";
                        ViewState["edit"] = "1";
                        GetEditData(Request.QueryString["id"].ToString());
                    }
                }
                Permission();
                HighlightSideMenu("prID", "wagecategorylink", "payrollmenutab");

            }

            pnlNext.Visible = false;
            if (Request.QueryString["id"] != null)
            {
                pnlNext.Visible = true;
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }

        //DefineGridStructure();
    }
    private void userpermissions()
    {
        if (Session["type"].ToString() != "c")
        {
            if (Session["type"].ToString() != "am")
            {
                objProp_User.ConnConfig = Session["config"].ToString();
                objProp_User.Username = Session["username"].ToString();
                objProp_User.PageName = "WageCategory.aspx";
                //DataSet dspage = _objBLUser.getScreensByUser(_objPropUser);
                //if (dspage.Tables[0].Rows.Count > 0)
                //{
                //    if (Convert.ToBoolean(dspage.Tables[0].Rows[0]["access"].ToString()) == false)
                //    {
                //        Response.Redirect("home.aspx");
                //    }
                //}
                //else
                //{
                //    Response.Redirect("home.aspx");
                //}
                if (Session["type"].ToString() != "am" && Session["type"].ToString() != "c")
                {
                    DataTable dtUserPermission = new DataTable();
                    dtUserPermission = GetUserById();
                    /// AccountPayablemodulePermission ///////////////////------->

                    string AccountPayablemodulePermission = dtUserPermission.Rows[0]["AccountPayablemodulePermission"] == DBNull.Value ? "Y" : dtUserPermission.Rows[0]["AccountPayablemodulePermission"].ToString();

                    if (AccountPayablemodulePermission == "N")
                    {
                        Response.Redirect("Home.aspx?permission=no"); return;
                    }

                    /// Vendor  ///////////////////------->

                    string VendorPermission = dtUserPermission.Rows[0]["Vendor"] == DBNull.Value ? "YYYYYY" : dtUserPermission.Rows[0]["Vendor"].ToString();
                    string ADD = VendorPermission.Length < 1 ? "Y" : VendorPermission.Substring(0, 1);
                    string Edit = VendorPermission.Length < 2 ? "Y" : VendorPermission.Substring(1, 1);
                    string Delete = VendorPermission.Length < 3 ? "Y" : VendorPermission.Substring(2, 1);
                    string View = VendorPermission.Length < 4 ? "Y" : VendorPermission.Substring(3, 1);

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
    protected void lnkFirst_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = new DataTable();
            dt = (DataTable)Session["WageCategoryList"];
            string url = "WageCategory.aspx?id=" + dt.Rows[0]["ID"];
            Response.Redirect(url);
        }
        catch (Exception ex)
        {

        }
    }
    private void GetEditData(string id)
    {
        try
        {
            DataSet ds = new DataSet();
            _objWage.ConnConfig = Session["config"].ToString();
            _objWage.ID = Convert.ToInt32(id);
            ds = objBL_Wage.GetWageByID(_objWage);
            DataRow _dr = ds.Tables[0].Rows[0];
            SetWage(_dr);
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "closeScript", "Materialize.updateTextFields();", true);

        }
        catch (Exception ex)
        {
            string type = "error";
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            if (str.Contains("Wage Deduction already exists"))
            {
                type = "warning";
            }
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyAddDepttype", "noty({text: '" + str + "', dismissQueue: true,  type : '" + type + "', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
    }
    private void SetWage(DataRow _dr)
    {
        if (Convert.ToInt16(_dr["Field"]).Equals(1))
        {
            chkField.Checked = true;
        }
        else
            chkField.Checked = false;

        if (Convert.ToInt16(_dr["FIT"]).Equals(1))
        {
            chkFIT.Checked = true;
        }
        else
            chkFIT.Checked = false;

        if (Convert.ToInt16(_dr["FICA"]).Equals(1))
        {
            chkFICA.Checked = true;
        }
        else
            chkFICA.Checked = false;

        if (Convert.ToInt16(_dr["MEDI"]).Equals(1))
        {
            chkMEDI.Checked = true;
        }
        else
            chkMEDI.Checked = false;

        if (Convert.ToInt16(_dr["FUTA"]).Equals(1))
        {
            chkFUTA.Checked = true;
        }
        else
            chkFUTA.Checked = false;

        if (Convert.ToInt16(_dr["SIT"]).Equals(1))
        {
            chkSIT.Checked = true;
        }
        else
            chkSIT.Checked = false;

        if (Convert.ToInt16(_dr["Vac"]).Equals(1))
        {
            chkVacation.Checked = true;
        }
        else
            chkVacation.Checked = false;

        if (Convert.ToInt16(_dr["WC"]).Equals(1))
        {
            chkWorkComp.Checked = true;
        }
        else
            chkWorkComp.Checked = false;

        if (Convert.ToInt16(_dr["Uni"]).Equals(1))
        {
            chkUnion.Checked = true;
        }
        else
            chkUnion.Checked = false;
        //if (Convert.ToInt16(_dr["SICK"]).Equals(1))
        //{
        //    chkSick.Checked = true;
        //}
        //else
        //    chkSick.Checked = false;
        txtDesc.Text = _dr["fDesc"].ToString();
        txtRemark.Text = _dr["Remarks"].ToString();
        ddlGlobal.SelectedValue = _dr["Globe"].ToString();
        ddlWageStatus.SelectedValue = _dr["Status"].ToString();

        hdnGLAcct.Value = _dr["GL"].ToString();
        hdnMilegAcct.Value = _dr["MileageGL"].ToString();
        hdnReimbAcct.Value = _dr["ReimburseGL"].ToString();
        hdnZoneAcct.Value = _dr["ZoneGL"].ToString();
        txtGLAcct.Text = _dr["GLName"].ToString();
        txtMileageAcct.Text = _dr["MileageGLName"].ToString();
        txtReimbAcct.Text = _dr["ReimGLName"].ToString();
        txtZoneAcct.Text = _dr["ZoneGLName"].ToString();

        hdnWageID.Value = _dr["ID"].ToString();

        List<WageRateType> wagePayRateTypes = new List<WageRateType>();

        wagePayRateTypes.Add(new WageRateType
        {
            WageRateTypeId = 1,
            WageRateTypeName = "Regular",
            WageReateTypeValue = Convert.ToInt32(_dr["Reg"])
        });
        wagePayRateTypes.Add(new WageRateType
        {
            WageRateTypeId = 3,
            WageRateTypeName = "Overtime",
            WageReateTypeValue = Convert.ToInt32(_dr["OT1"])
        });
        wagePayRateTypes.Add(new WageRateType
        {
            WageRateTypeId = 4,
            WageRateTypeName = "Double Time",
            WageReateTypeValue = Convert.ToInt32(_dr["OT2"])
        });
        wagePayRateTypes.Add(new WageRateType
        {
            WageRateTypeId = 6,
            WageRateTypeName = "1.7 Time",
            WageReateTypeValue = Convert.ToInt32(_dr["NT"])
        });
        wagePayRateTypes.Add(new WageRateType
        {
            WageRateTypeId = 7,
            WageRateTypeName = "Travel Time",
            WageReateTypeValue = Convert.ToInt32(_dr["TT"])
        });

        rptPayRate.DataSource = wagePayRateTypes;
        rptPayRate.DataBind();


        List<WageRateType> wageBurdenRateTypes = new List<WageRateType>();
        wageBurdenRateTypes.Add(new WageRateType
        {
            WageRateTypeId = 1,
            WageRateTypeName = "Regular",
            WageReateTypeValue = Convert.ToInt32(_dr["CReg"]),
            WageRateGLId = (Convert.IsDBNull(_dr["RegGL"]) ? null : (int?)Convert.ToInt32(_dr["RegGL"])),
            WageRateGLName = (Convert.IsDBNull(_dr["RegGL"]) ? null : _dr["RegGLName"].ToString())
        });
        wageBurdenRateTypes.Add(new WageRateType
        {
            WageRateTypeId = 3,
            WageRateTypeName = "Overtime",
            WageReateTypeValue = Convert.ToInt32(_dr["COT"]),
            WageRateGLId = (Convert.IsDBNull(_dr["OTGL"]) ? null : (int?)Convert.ToInt32(_dr["OTGL"])),
            WageRateGLName = (Convert.IsDBNull(_dr["OTGLName"]) ? null : _dr["OTGLName"].ToString())
        });
        wageBurdenRateTypes.Add(new WageRateType
        {
            WageRateTypeId = 4,
            WageRateTypeName = "Double Time",
            WageReateTypeValue = Convert.ToInt32(_dr["CDT"]),
            WageRateGLId = (Convert.IsDBNull(_dr["DTGL"]) ? null : (int?)Convert.ToInt32(_dr["DTGL"])),
            WageRateGLName = (Convert.IsDBNull(_dr["DTGLName"]) ? null : _dr["DTGLName"].ToString())
        });
        wageBurdenRateTypes.Add(new WageRateType
        {
            WageRateTypeId = 6,
            WageRateTypeName = "1.7 Time",
            WageReateTypeValue = Convert.ToInt32(_dr["CNT"]),
            WageRateGLId = (Convert.IsDBNull(_dr["NTGL"]) ? null : (int?)Convert.ToInt32(_dr["NTGL"])),
            WageRateGLName = (Convert.IsDBNull(_dr["NTGLName"]) ? null : _dr["NTGLName"].ToString())
        });
        wageBurdenRateTypes.Add(new WageRateType
        {
            WageRateTypeId = 7,
            WageRateTypeName = "Travel Time",
            WageReateTypeValue = Convert.ToInt32(_dr["CTT"]),
            WageRateGLId = (Convert.IsDBNull(_dr["TTGL"]) ? null : (int?)Convert.ToInt32(_dr["TTGL"])),
            WageRateGLName = (Convert.IsDBNull(_dr["TTGLName"]) ? null : _dr["TTGLName"].ToString())
        });
        rptBurdenRate.DataSource = wageBurdenRateTypes;
        rptBurdenRate.DataBind();
    }
    private void ClearControl()
    {
        chkFIT.Checked = true;
        chkFICA.Checked = true;
        chkMEDI.Checked = true;
        chkFUTA.Checked = true;
        chkSIT.Checked = true;
        chkVacation.Checked = true;
        chkWorkComp.Checked = true;
        chkUnion.Checked = true;
        //chkSick.Checked = true;
        //txtRegularRate.Text = "0.00";
        //txtOvertimeRate.Text = "0.00";
        //txtTime.Text = "0.00";
        //txtDoubleTime.Text = "0.00";
        //txtTravelTime.Text = "0.00";
        //txtCReg.Text = "0.00";
        //txtCOT.Text = "0.00";
        //txtCNT.Text = "0.00";
        //txtCDT.Text = "0.00";
        //txtCTT.Text = "0.00";
        ddlGlobal.SelectedValue = "1";
        chkField.Checked = true;

        txtDesc.Text = string.Empty;
        txtGLAcct.Text = string.Empty;
        txtMileageAcct.Text = string.Empty;
        txtReimbAcct.Text = string.Empty;
        txtZoneAcct.Text = string.Empty;
        hdnGLAcct.Value = "0";
        hdnMilegAcct.Value = "0";
        hdnReimbAcct.Value = "0";
        hdnZoneAcct.Value = "0";

        ddlWageStatus.SelectedValue = "0";

        //txtRegGL.Text = string.Empty;
        //txtOTGL.Text = string.Empty;
        //txtNTGL.Text = string.Empty;
        //txtDTGL.Text = string.Empty;
        //txtTTGL.Text = string.Empty;
        //txtRemark.Text = string.Empty;
        //hdnRegGL.Value = "0";
        //hdnOTGL.Value = "0";
        //hdnNTGL.Value = "0";
        //hdnDTGL.Value = "0";
        //hdnTTGL.Value = "0";
        ViewState["edit"] = "0";
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "closeScript", "Materialize.updateTextFields();", true);
    }
    private static PropertyInfo[] GetProperties(object obj)
    {
        return obj.GetType().GetProperties();
    }

    public DataTable CreateWageRateTable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("WageId", typeof(Int32));
        dt.Columns.Add("WageRateTypeId", typeof(Int32));
        dt.Columns.Add("Rate", typeof(Double));
        dt.Columns.Add("BurdenRate", typeof(Double));
        dt.Columns.Add("GeneralLedgerId", typeof(Int32));
        dt.Columns.Add("CalenderId", typeof(Int32));
        dt.Columns.Add("ParentId", typeof(Int32));
        dt.Columns.Add("EffectiveFrom", typeof(DateTime));
        dt.Columns.Add("EffectiveTo", typeof(DateTime));
        dt.Columns.Add("Status", typeof(Int32));
        dt.Columns.Add("Deleted", typeof(string));
        return dt;
    }

    public DataTable CreateWageTaxTable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("WageId", typeof(Int32));
        dt.Columns.Add("TaxId", typeof(Int32));
        dt.Columns.Add("ParentId", typeof(Int32));
        dt.Columns.Add("EffectiveFrom", typeof(DateTime));
        dt.Columns.Add("EffectiveTo", typeof(DateTime));
        dt.Columns.Add("Status", typeof(Int32));
        dt.Columns.Add("Deleted", typeof(string));
        return dt;
    }
  
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        //Submit();
        try
        {
            DataTable dtWageRate = CreateWageRateTable();
            dtWageRate.Clear();

            if (rptPayRate.Items.Count > 0)
            {
                foreach (RepeaterItem item in rptPayRate.Items)
                {

                    //var properties = GetProperties(_objWage);
                    //foreach (var p in properties)
                    //{
                    //    string name = p.Name;
                    //    string lblName = (item.FindControl("hdnayRateType") as HiddenField).Value;

                    //    if (name.ToUpper() == lblName.ToUpper())
                    //    {
                    //        double value = Convert.ToDouble(((TextBox)item.FindControl("txtPayRateType")).Text);
                    //        var propertyInfo = _objWage.GetType().GetProperty(name);
                    //        propertyInfo.SetValue(_objWage, value);
                    //    }
                    //}

                    string RateType = (item.FindControl("hdnWageRateType") as HiddenField).Value;

                    //DataRow newRow = dtWageRate.NewRow();
                    //newRow["WageId"] = null;
                    //newRow["WageRateTypeId"] = Convert.ToInt32(RateType);
                    //newRow["CalenderId"] = 1;
                    //newRow["ParentId"] = null;
                    //newRow["EffectiveFrom"] = "2001, 1, 1";
                    //newRow["EffectiveTo"] = "9999, 11, 31";
                    //newRow["Status"] = 1;
                    //newRow["Deleted"] = 0;

                    switch (RateType)
                    {
                        case "Regular":
                            _objWage.Reg = Convert.ToDouble(((TextBox)item.FindControl("txtPayRateType")).Text);
                            //newRow["Rate"] = Convert.ToDouble(((TextBox)item.FindControl("txtPayRateType")).Text);
                            break;
                        case "Overtime":
                            _objWage.OT1 = Convert.ToDouble(((TextBox)item.FindControl("txtPayRateType")).Text);
                            //newRow["Rate"] = Convert.ToDouble(((TextBox)item.FindControl("txtPayRateType")).Text);
                            break;
                        case "Double Time":
                            _objWage.OT2 = Convert.ToDouble(((TextBox)item.FindControl("txtPayRateType")).Text);
                            //newRow["Rate"] = Convert.ToDouble(((TextBox)item.FindControl("txtPayRateType")).Text);
                            break;
                        case "1.7 Time":
                            _objWage.NT = Convert.ToDouble(((TextBox)item.FindControl("txtPayRateType")).Text);
                            //newRow["Rate"] = Convert.ToDouble(((TextBox)item.FindControl("txtPayRateType")).Text);
                            break;
                        case "Travel Time":
                            _objWage.TT = Convert.ToDouble(((TextBox)item.FindControl("txtPayRateType")).Text);
                            //newRow["Rate"] = Convert.ToDouble(((TextBox)item.FindControl("txtPayRateType")).Text);
                            break;
                    }

                    //dtWageRate.Rows.Add(newRow);
                }
            }

            if (rptBurdenRate.Items.Count > 0)
            {
                foreach (RepeaterItem item in rptBurdenRate.Items)
                {
                    string RateType = (item.FindControl("hdnWageRateType") as HiddenField).Value;

                    //DataRow row = dtWageRate.Select("WageRateTypeName =" + Convert.ToInt32(RateType)).FirstOrDefault();

                    switch (RateType)
                    {
                        case "Regular":
                            _objWage.CReg = Convert.ToDouble(((TextBox)item.FindControl("txtBurdenRateType")).Text);
                            _objWage.RegGL = Convert.ToInt32((item.FindControl("hdnGL") as HiddenField).Value);
                            //row["BurdenRate"] = Convert.ToDouble(((TextBox)item.FindControl("txtBurdenRateType")).Text);
                            //row["GeneralLedgerId"] = Convert.ToInt32((item.FindControl("hdnGL") as HiddenField).Value); ;
                            break;
                        case "Overtime":
                            _objWage.COT = Convert.ToDouble(((TextBox)item.FindControl("txtBurdenRateType")).Text);
                            _objWage.OTGL = Convert.ToInt32((item.FindControl("hdnGL") as HiddenField).Value);
                            //row["BurdenRate"] = Convert.ToDouble(((TextBox)item.FindControl("txtBurdenRateType")).Text);
                            //row["GeneralLedgerId"] = Convert.ToInt32((item.FindControl("hdnGL") as HiddenField).Value); ;
                            break;
                        case "Double Time":
                            _objWage.CDT = Convert.ToDouble(((TextBox)item.FindControl("txtBurdenRateType")).Text);
                            _objWage.NTGL = Convert.ToInt32((item.FindControl("hdnGL") as HiddenField).Value);
                            //row["BurdenRate"] = Convert.ToDouble(((TextBox)item.FindControl("txtBurdenRateType")).Text);
                            //row["GeneralLedgerId"] = Convert.ToInt32((item.FindControl("hdnGL") as HiddenField).Value); ;
                            break;
                        case "1.7 Time":
                            _objWage.CNT = Convert.ToDouble(((TextBox)item.FindControl("txtBurdenRateType")).Text);
                            _objWage.DTGL = Convert.ToInt32((item.FindControl("hdnGL") as HiddenField).Value);
                            //row["BurdenRate"] = Convert.ToDouble(((TextBox)item.FindControl("txtBurdenRateType")).Text);
                            //row["GeneralLedgerId"] = Convert.ToInt32((item.FindControl("hdnGL") as HiddenField).Value); ;
                            break;
                        case "Travel Time":
                            _objWage.CTT = Convert.ToDouble(((TextBox)item.FindControl("txtBurdenRateType")).Text);
                            _objWage.TTGL = Convert.ToInt32((item.FindControl("hdnGL") as HiddenField).Value);
                            //row["BurdenRate"] = Convert.ToDouble(((TextBox)item.FindControl("txtBurdenRateType")).Text);
                            //row["GeneralLedgerId"] = Convert.ToInt32((item.FindControl("hdnGL") as HiddenField).Value); ;
                            break;
                    }
                }
            }


            _objWage.ConnConfig = Session["config"].ToString();
            _objWage.Name = txtDesc.Text;
            _objWage.GL = Convert.ToInt32(hdnGLAcct.Value);
            _objWage.MileageGL = Convert.ToInt32(hdnMilegAcct.Value);
            _objWage.ReimGL = Convert.ToInt32(hdnReimbAcct.Value);
            _objWage.ZoneGL = Convert.ToInt32(hdnZoneAcct.Value);
            _objWage.Status = Convert.ToInt16(ddlWageStatus.SelectedValue);
            _objWage.Globe = Convert.ToInt16(ddlGlobal.SelectedValue);

            //DataTable dtWageTax = CreateWageTaxTable();
            //dtWageTax.Clear();

            if (chkField.Checked.Equals(true))
            {
                _objWage.Field = 1;
            }
            else
            {
                _objWage.Field = 0;
            }
            if (chkFIT.Checked.Equals(true))
            {
                _objWage.FIT = 1;

                //DataRow newRow = dtWageTax.NewRow();
                //newRow["WageId"] = null;
                //newRow["TaxId"] = 4;
                //newRow["ParentId"] = null;
                //newRow["EffectiveFrom"] = "2001, 1, 1";
                //newRow["EffectiveTo"] = "9999, 11, 31";
                //newRow["Status"] = 1;
                //newRow["Deleted"] = 0;
                //dtWageTax.Rows.Add(newRow);
            }
            else
            {
                _objWage.FIT = 0;
            }
            if (chkFICA.Checked.Equals(true))
            {
                _objWage.FICA = 1;
                //DataRow newRow = dtWageTax.NewRow();
                //newRow["WageId"] = null;
                //newRow["TaxId"] = 6;
                //newRow["ParentId"] = null;
                //newRow["EffectiveFrom"] = "2001, 1, 1";
                //newRow["EffectiveTo"] = "9999, 11, 31";
                //newRow["Status"] = 1;
                //newRow["Deleted"] = 0;
                //dtWageTax.Rows.Add(newRow);
            }
            else
            {
                _objWage.FICA = 0;
            }
            if (chkMEDI.Checked.Equals(true))
            {
                _objWage.MEDI = 1;
                //DataRow newRow = dtWageTax.NewRow();
                //newRow["WageId"] = null;
                //newRow["TaxId"] = 8;
                //newRow["ParentId"] = null;
                //newRow["EffectiveFrom"] = "2001, 1, 1";
                //newRow["EffectiveTo"] = "9999, 11, 31";
                //newRow["Status"] = 1;
                //newRow["Deleted"] = 0;
                //dtWageTax.Rows.Add(newRow);
            }
            else
            {
                _objWage.MEDI = 0;
            }
            if (chkFUTA.Checked.Equals(true))
            {
                _objWage.FUTA = 1;
                //DataRow newRow = dtWageTax.NewRow();
                //newRow["WageId"] = null;
                //newRow["TaxId"] = 7;
                //newRow["ParentId"] = null;
                //newRow["EffectiveFrom"] = "2001, 1, 1";
                //newRow["EffectiveTo"] = "9999, 11, 31";
                //newRow["Status"] = 1;
                //newRow["Deleted"] = 0;
                //dtWageTax.Rows.Add(newRow);
            }
            else
            {
                _objWage.FUTA = 0;
            }
            if (chkSIT.Checked.Equals(true))
            {
                _objWage.SIT = 1;
                //DataRow newRow = dtWageTax.NewRow();
                //newRow["WageId"] = null;
                //newRow["TaxId"] = 9;
                //newRow["ParentId"] = null;
                //newRow["EffectiveFrom"] = "2001, 1, 1";
                //newRow["EffectiveTo"] = "9999, 11, 31";
                //newRow["Status"] = 1;
                //newRow["Deleted"] = 0;
                //dtWageTax.Rows.Add(newRow);
            }
            else
            {
                _objWage.SIT = 0;
            }
            if (chkVacation.Checked.Equals(true))
            {
                _objWage.Vac = 1;
            }
            else
            {
                _objWage.Vac = 0;
            }
            if (chkWorkComp.Checked.Equals(true))
            {
                _objWage.WC = 1;
            }
            else
            {
                _objWage.WC = 0;
            }
            if (chkUnion.Checked.Equals(true))
            {
                _objWage.Uni = 1;
            }
            else
            {
                _objWage.Uni = 0;
            }
            //if (chkSick.Checked.Equals(true))
            //{
            //    _objWage.Sick = 1;
            //}
            //else
            //{
            //    _objWage.Sick = 0;
            //}
            _objWage.Remarks = txtRemark.Text;
            string msg = "Added";
            if (ViewState["edit"].ToString() == "0")
            {
                objBL_Wage.AddWage(_objWage);
            }
            else if (ViewState["edit"].ToString() == "1")
            {
                msg = "Updated";
                _objWage.ID = Convert.ToInt32(hdnWageID.Value);
                objBL_Wage.UpdateWage(_objWage);
            }

            //this.programmaticModalPopup.Hide();
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccAddDeparmenttype", "noty({text: 'Wage " + msg + " Successfully', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
            string url = "WageCategoryList.aspx";
            Response.Redirect(url);

        }
        catch (Exception ex)
        {
            string type = "error";
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            if (str.Contains("Wage Category already exists, please use different name"))
            {
                type = "warning";
            }
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyAddDepttype", "noty({text: '" + str + "', dismissQueue: true,  type : '" + type + "', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }


    }
    protected void lnkClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("WageCategoryList.aspx");
    }
    protected void lnkPrevious_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = new DataTable();
            dt = (DataTable)Session["WageCategoryList"];
            DataColumn[] keyColumns = new DataColumn[1];
            keyColumns[0] = dt.Columns["ID"];
            dt.PrimaryKey = keyColumns;

            DataRow d = dt.Rows.Find(Request.QueryString["id"].ToString());
            int index = dt.Rows.IndexOf(d);
            int c = dt.Rows.Count - 1;
            if (index <= c)
            {
                string url = "WageCategory.aspx?id=" + dt.Rows[index - 1]["ID"];
                Response.Redirect(url);
            }
        }
        catch (Exception ex)
        {

        }
    }

    protected void lnkNext_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = new DataTable();
            dt = (DataTable)Session["WageCategoryList"];
            DataColumn[] keyColumns = new DataColumn[1];
            keyColumns[0] = dt.Columns["ID"];
            dt.PrimaryKey = keyColumns;

            DataRow d = dt.Rows.Find(Request.QueryString["id"].ToString());
            int index = dt.Rows.IndexOf(d);
            int c = dt.Rows.Count - 1;
            if (index < c)
            {
                string url = "WageCategory.aspx?id=" + dt.Rows[index + 1]["ID"];
                Response.Redirect(url);
            }
        }
        catch (Exception ex)
        {

        }
    }

    protected void lnkLast_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = new DataTable();
            dt = (DataTable)Session["WageCategoryList"];
            string url = "WageCategory.aspx?id=" + dt.Rows[dt.Rows.Count - 1]["ID"];
            Response.Redirect(url);
        }
        catch (Exception ex)
        {

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
    public bool CheckAddEditPermission()
    {
        bool result = true;
        if (Session["type"].ToString() != "am" && Session["type"].ToString() != "c")
        {
            DataTable ds = new DataTable();
            ds = (DataTable)Session["userinfo"];
            /// Vendor ///////////////////------->
            string VendorPermission = ds.Rows[0]["Vendor"] == DBNull.Value ? "YYYY" : ds.Rows[0]["Vendor"].ToString();
            string ViewVendor = VendorPermission.Length < 4 ? "Y" : VendorPermission.Substring(3, 1);
            if (ViewVendor == "N")
            {
                result = false;
            }
        }
        return result;
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
            ViewState["CompPermission"] = 1;
            //dvCompanyPermission.Visible = true;
        }
        else
        {
            ViewState["CompPermission"] = 0;
            //dvCompanyPermission.Visible = false;
        }
    }
}

