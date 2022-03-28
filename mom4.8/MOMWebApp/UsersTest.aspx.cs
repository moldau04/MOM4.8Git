using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessEntity;
using BusinessLayer;
using System.Data;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;
using AjaxControlToolkit;


public partial class UsersTest : System.Web.UI.Page
{
    #region "Variables"
    BL_User objBL_User = new BL_User();
    Wage _objWage = new Wage();
    BusinessEntity.User objProp_User = new BusinessEntity.User();

    public static bool IsAddEdit = false;
    public static bool IsDelete = false;
    public static bool IsCopy = false;

    #endregion

    #region PageLoad
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userid"] == null)
        {
            Response.Redirect("login.aspx");
        }
        pnlWageGv.Visible = true;
        
        if(IsCopy)
        {
            ViewState["edit"] = 0;
        }

        if (!IsPostBack)
        {
            HighlightSideMenu("prID", "wagedeductionslink", "payrollmenutab");
            ResetWageForm();
            ViewState["edit"] = 0;
        }

    }
    #endregion

    #region Schedule Wage Schedule
    private void FillWage()
    {
        DataSet ds = new DataSet();
        objProp_User.ConnConfig = Session["config"].ToString();
        ds = objBL_User.getWage(objProp_User);
        RadGrid_WageSchedules.VirtualItemCount = ds.Tables[0].Rows.Count;
        RadGrid_WageSchedules.DataSource = ds.Tables[0];
    }
     bool isGroupingWageSchedule = false;
    public bool ShouldApplySortFilterWageSchedule()
    {
        return RadGrid_WageSchedules.MasterTableView.FilterExpression != "" ||
            (RadGrid_WageSchedules.MasterTableView.GroupByExpressions.Count > 0 || isGroupingWageSchedule) ||
            RadGrid_WageSchedules.MasterTableView.SortExpressions.Count > 0;
    }
    protected void RadGrid_WageSchedule_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
       RadGrid_WageSchedules.AllowCustomPaging = !ShouldApplySortFilterWageSchedule();
        FillWage();
    }


    protected void lnkAddWage_Click(object sender, EventArgs e)
    {
        ResetWageForm();
        pnlWageAddEdit.Visible = true;
        pnlWageGv.Visible = false;
        RadCodeBlock5.Visible = false;

        lnkWageSave.Visible = true;
        lnkWageClose.Visible = true;
        lnkAddWage.Visible = false;
        lnkEditWage.Visible = false;
        lnkDeleteWage.Visible = false;
        lnkCopyBtn.Visible = false;
        ViewState["edit"] = "0";
        IsCopy = false;
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "closeScript", "Materialize.updateTextFields();", true);
    }
    protected void lnkEditWage_Click(object sender, EventArgs e)
     {
       
        RadCodeBlock5.Visible = false;
        pnlWageGv.Visible = false;
        ViewState["edit"] = 1;
        pnlWageAddEdit.Visible = true;
        lnkWageSave.Visible = true;
        lnkWageClose.Visible = true;
        lnkAddWage.Visible = false;
        lnkEditWage.Visible = false;
        lnkDeleteWage.Visible = false;
        lnkCopyBtn.Visible = false;
        IsCopy = false;
        ResetWageForm();
        foreach (GridDataItem di in RadGrid_WageSchedules.SelectedItems)
        {
            TableCell cell = di["chkSelect"];
            CheckBox chkSelect = (CheckBox)cell.Controls[0];
            Label lblWageid = (Label)di.FindControl("lblWageId");
            string lblFdesc = di["lblWageFdesc"].Text.Trim();
            string lblrem = di["lblrem"].Text.Trim();
            if (chkSelect.Checked == true)
            {
                _objWage.ConnConfig = Session["config"].ToString();
                _objWage.ID = Convert.ToInt32(lblWageid.Text);
                hdnWageID.Value = lblWageid.Text;
                DataSet _dsWage = objBL_User.GetWageByID(_objWage);
                DataRow _dr = _dsWage.Tables[0].Rows[0];
                SetWage(_dr);
                pnlWageAddEdit.Visible = true;
                ViewState["edit"] = "1";
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "closeScript", "Materialize.updateTextFields();", true);
            }
        }

    }

    protected void lnkCopyWage_Click(object sender, EventArgs e)
    {

        RadCodeBlock5.Visible = false;
        pnlWageGv.Visible = false;
        ViewState["edit"] = 1;
        pnlWageAddEdit.Visible = true;
        lnkWageSave.Visible = true;
        lnkWageClose.Visible = true;
        lnkAddWage.Visible = false;
        lnkEditWage.Visible = false;
        lnkDeleteWage.Visible = false;
        lnkCopyBtn.Visible = false;
        ResetWageForm();
        foreach (GridDataItem di in RadGrid_WageSchedules.SelectedItems)
        {
            TableCell cell = di["chkSelect"];
            CheckBox chkSelect = (CheckBox)cell.Controls[0];
            Label lblWageid = (Label)di.FindControl("lblWageId");
            string lblFdesc = di["lblWageFdesc"].Text.Trim();
            string lblrem = di["lblrem"].Text.Trim();
            if (chkSelect.Checked == true)
            {
                _objWage.ConnConfig = Session["config"].ToString();
                _objWage.ID = Convert.ToInt32(lblWageid.Text);
                hdnWageID.Value = lblWageid.Text;
                DataSet _dsWage = objBL_User.GetWageByID(_objWage);
                DataRow _dr = _dsWage.Tables[0].Rows[0];
                SetWage(_dr);
                pnlWageAddEdit.Visible = true;
                ViewState["edit"] = "1";
                IsCopy = true;
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "closeScript", "Materialize.updateTextFields();", true);
            }
        }

    }

    protected void lnkDeleteWage_Click(object sender, EventArgs e)
    {
        IsDelete = false;
        IsCopy = false;
        try
        {
            foreach (GridDataItem di in RadGrid_WageSchedules.SelectedItems)
            {
                IsDelete = true;
                TableCell cell = di["chkSelect"];
                CheckBox chkSelect = (CheckBox)cell.Controls[0];
                Label lblWageId = (Label)di.FindControl("lblWageId");
                if (chkSelect.Checked == true)
                {
                    _objWage.ConnConfig = Session["config"].ToString();
                    _objWage.ID = Convert.ToInt32(lblWageId.Text);
                    objBL_User.DeleteWageByID(_objWage);

                    FillWage();
                    //RadGrid_WageSchedules.Rebind();

                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccAddWage", "noty({text: 'Wage " + lblWageId.Text + " Deleted Successfully!', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                }
            }
            if (!IsDelete)
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarningWage", "noty({text: 'Please select Wage to delete!', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErrDeldep", "noty({text: '" + str + "',  type : 'error', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    protected void lnkWageSave_Click(object sender, EventArgs e)
    {
        try
        {
            _objWage.ConnConfig = Session["config"].ToString();
            _objWage.Name = txtDesc.Text;
            _objWage.GL = Convert.ToInt32(hdnGLAcct.Value);
            _objWage.MileageGL = Convert.ToInt32(hdnMilegAcct.Value);
            _objWage.ReimGL = Convert.ToInt32(hdnReimbAcct.Value);
            _objWage.ZoneGL = Convert.ToInt32(hdnZoneAcct.Value);
            _objWage.Status = Convert.ToInt16(ddlWageStatus.SelectedValue);
            _objWage.Globe = Convert.ToInt16(ddlGlobal.SelectedValue);
            _objWage.Reg = Convert.ToDouble(txtRegularRate.Text);
            _objWage.OT1 = Convert.ToDouble(txtOvertimeRate.Text);
            _objWage.NT = Convert.ToDouble(txtTime.Text);
            _objWage.OT2 = Convert.ToDouble(txtDoubleTime.Text);
            _objWage.TT = Convert.ToDouble(txtTravelTime.Text);
            _objWage.CReg = Convert.ToDouble(txtCReg.Text);
            _objWage.COT = Convert.ToDouble(txtCOT.Text);
            _objWage.CNT = Convert.ToDouble(txtCNT.Text);
            _objWage.CDT = Convert.ToDouble(txtCDT.Text);
            _objWage.CTT = Convert.ToDouble(txtCTT.Text);

            _objWage.RegGL = Convert.ToInt32(hdnRegGL.Value);
            _objWage.OTGL = Convert.ToInt32(hdnOTGL.Value);
            _objWage.NTGL = Convert.ToInt32(hdnNTGL.Value);
            _objWage.DTGL = Convert.ToInt32(hdnDTGL.Value);
            _objWage.TTGL = Convert.ToInt32(hdnTTGL.Value);

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
            }
            else
            {
                _objWage.FIT = 0;
            }
            if (chkFICA.Checked.Equals(true))
            {
                _objWage.FICA = 1;
            }
            else
            {
                _objWage.FICA = 0;
            }
            if (chkMEDI.Checked.Equals(true))
            {
                _objWage.MEDI = 1;
            }
            else
            {
                _objWage.MEDI = 0;
            }
            if (chkFUTA.Checked.Equals(true))
            {
                _objWage.FUTA = 1;
            }
            else
            {
                _objWage.FUTA = 0;
            }
            if (chkSIT.Checked.Equals(true))
            {
                _objWage.SIT = 1;
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
            _objWage.Remarks = txtRemark.Text;
            string msg = "Added";
            if (ViewState["edit"].ToString() == "0")
            {
                objBL_User.AddWage(_objWage);
            }
            else if (ViewState["edit"].ToString() == "1")
            {
                msg = "Updated";
                _objWage.ID = Convert.ToInt32(hdnWageID.Value);
                objBL_User.UpdateWage(_objWage);
            }
            pnlWageAddEdit.Visible = false;

            lnkWageSave.Visible = false;
            lnkWageClose.Visible = false;
            lnkAddWage.Visible = true;
            lnkEditWage.Visible = true;
            lnkDeleteWage.Visible = true;
            lnkCopyBtn.Visible = true;
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccAddDeparmenttype", "noty({text: 'Wage " + msg + " Successfully', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
            FillWage();
            RadGrid_WageSchedules.Rebind();
        }
        catch (Exception ex)
        {
            pnlWageGv.Visible = false;
            pnlWageAddEdit.Visible = true;
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyAddDepttype", "noty({text: '" + str + "', dismissQueue: true,  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
    }

    protected void lnkWageClose_Click(object sender, EventArgs e)
    {
        pnlWageAddEdit.Visible = false;
        
        lnkAddWage.Visible = true;
        lnkEditWage.Visible = true;
        lnkDeleteWage.Visible = true;
        lnkCopyBtn.Visible = true;

        FillWage();
    }
    private void SetWage(DataRow _dr)
    {
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
        txtDesc.Text = _dr["fDesc"].ToString();
        txtRemark.Text = _dr["Remarks"].ToString();
        txtRegularRate.Text = String.Format("{0:0.00}", _dr["Reg"]);
        txtOvertimeRate.Text = String.Format("{0:0.00}", _dr["OT1"]);
        txtTime.Text = String.Format("{0:0.00}", _dr["NT"]);
        txtDoubleTime.Text = String.Format("{0:0.00}", _dr["OT2"]);
        txtTravelTime.Text = String.Format("{0:0.00}", _dr["TT"]);
        txtCReg.Text = String.Format("{0:0.00}", _dr["CReg"]);
        txtCOT.Text = String.Format("{0:0.00}", _dr["COT"]);
        txtCNT.Text = String.Format("{0:0.00}", _dr["CNT"]);
        txtCDT.Text = String.Format("{0:0.00}", _dr["CDT"]);
        txtCTT.Text = String.Format("{0:0.00}", _dr["CTT"]);
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

        hdnRegGL.Value = _dr["RegGL"].ToString();
        hdnOTGL.Value = _dr["OTGL"].ToString();
        hdnNTGL.Value = _dr["NTGL"].ToString();
        hdnDTGL.Value = _dr["DTGL"].ToString();
        hdnTTGL.Value = _dr["TTGL"].ToString();

        txtRegGL.Text = _dr["RegGLName"].ToString();
        txtOTGL.Text = _dr["OTGLName"].ToString();
        txtNTGL.Text = _dr["NTGLName"].ToString();
        txtDTGL.Text = _dr["DTGLName"].ToString();
        txtTTGL.Text = _dr["TTGLName"].ToString();
    }
    private void ResetWageForm()
    {
        chkFIT.Checked = true;
        chkFICA.Checked = true;
        chkMEDI.Checked = true;
        chkFUTA.Checked = true;
        chkSIT.Checked = true;
        chkVacation.Checked = true;
        chkWorkComp.Checked = true;
        chkUnion.Checked = true;
        txtRegularRate.Text = "0.00";
        txtOvertimeRate.Text = "0.00";
        txtTime.Text = "0.00";
        txtDoubleTime.Text = "0.00";
        txtTravelTime.Text = "0.00";
        txtCReg.Text = "0.00";
        txtCOT.Text = "0.00";
        txtCNT.Text = "0.00";
        txtCDT.Text = "0.00";
        txtCTT.Text = "0.00";
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

        txtRegGL.Text = string.Empty;
        txtOTGL.Text = string.Empty;
        txtNTGL.Text = string.Empty;
        txtDTGL.Text = string.Empty;
        txtTTGL.Text = string.Empty;
        txtRemark.Text = string.Empty;
        hdnRegGL.Value = "0";
        hdnOTGL.Value = "0";
        hdnNTGL.Value = "0";
        hdnDTGL.Value = "0";
        hdnTTGL.Value = "0";
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


    #endregion







}
