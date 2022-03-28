using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BusinessEntity;
using BusinessLayer;
using Telerik.Web.UI;
public partial class ETimesheetpay : System.Web.UI.Page
{
    GeneralFunctions objGeneral = new GeneralFunctions();

    User objPropUser = new User();

    BL_User objBL_User = new BL_User();

    DateTime fromDate;

    DateTime toDate;

    private static int intExportExcel = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        // Response.Redirect("home.aspx");

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

            txtFromDate.Text = DateTime.Today.AddDays(-1 * (int)(DateTime.Today.DayOfWeek)).ToShortDateString();
            txtToDate.Text = DateTime.Now.AddDays(DayOfWeek.Saturday - (DateTime.Now.DayOfWeek)).Date.ToShortDateString();
            lblWeek.Attributes["class"] = "labelactive";
            rdWeek.Checked = true;
            Session["fromDate"] = txtFromDate.Text;
            Session["ToDate"] = txtToDate.Text;
            CheckUserPermission();
            ViewState["saved"] = 0;
            ViewState["update"] = 0;
            FillDepartment();
            FillSupervisor();
            FillWorker();
            
            //GetTimesheetEmp();
            hdnCoCode.Value = getCoCode();
            try
            {
                if (ConfigurationManager.AppSettings["displayPayRollExportButton"].ToString().ToLower() != "true".ToLower())
                {
                    divExportPayRoll.Attributes.Add("style", "display:none");
                }
            }
            catch (Exception ex)
            {
                divExportPayRoll.Attributes.Add("style", "display:none");
            }
            if (Request.QueryString["f"] != null)
            {
                UpdateControl();
            }
        }
        CompanyPermission();
        //tbldrop.Visible = false;
        lnkSave.Visible = false;
        //get the radio button clicked
        HighlightSideMenu("prID", "ETimecardlink", "payrollmenutab");

        // foreach (GridViewRow gr in gvTimesheet.MasterTableView.Items)
        //{
        //GridView gvTickets = (GridView)gr.FindControl("gvTickets"); 
        //foreach (GridViewRow grTic in gvTickets.Rows)
        //{ 
        //   HyperLink lnkTick = (HyperLink)grTic.FindControl("lnkTick");

        // }
        // }
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


        if (Session["MSM"].ToString() == "TS")
        {
            //lnkDelete.Visible = false;
            //lnkReview.Visible = false;
        }
        else
        {
            if (Convert.ToInt32(Session["ISsupervisor"]) != 1)
            {
                if (Session["type"].ToString() != "am")
                {
                    DataTable dt = new DataTable();
                    dt = (DataTable)Session["userinfo"];
                    if (dt.Rows[0]["massreview"].ToString() == "0")
                    {
                        //lnkReview.Visible = false;
                    }
                }
            }
        }
        if (Session["type"].ToString() == "c")
        {
            //lnkReview.Visible = false;
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

    private void CheckUserPermission()
    {
        BL_General objBL_General = new BL_General();
        General objGeneral = new General();
        objGeneral.ConnConfig = Session["config"].ToString();
        DataSet dsLastSync = objBL_General.getSagelatsync(objGeneral);
        int intintegration = Convert.ToInt32(dsLastSync.Tables[0].Rows[0]["sageintegration"]);
        if (intintegration == 1)
        {
            lnkExport.Visible = true;
            //ddlExport.Visible = true;
        }
        else
        {
            lnkExport.Visible = false;
            //ddlExport.Visible = false;
        }

        if (Session["type"].ToString() != "c" && Session["type"].ToString() != "am")
        {

            DataTable ds = new DataTable();
            ds = (DataTable)Session["userinfo"];
            ds = GetUserById();
            /// ETimesheet Permission ///////////////////------->

            string ETimesheetPermission = ds.Rows[0]["ETimesheet"] == DBNull.Value ? "YYYYYY" : ds.Rows[0]["ETimesheet"].ToString();
            string View = ETimesheetPermission.Length < 4 ? "Y" : ETimesheetPermission.Substring(3, 1);

            if (View == "N")
            {
                Response.Redirect("Home.aspx?permission=no");
            }
        }
    }

    private void CompanyPermission()
    {
        if (Session["COPer"].ToString() == "1")
        {
            //gvTimesheet.Columns.FindByUniqueName("Company").Visible = true;
        }
        else
        {
            //gvTimesheet.Columns.FindByUniqueName("Company").Visible = false;
        }
    }

    private void FillDepartment()
    {
        DataSet ds = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();

        ds = objBL_User.getDepartment(objPropUser);

        ddlDepartment.DataSource = ds.Tables[0];
        ddlDepartment.DataTextField = "type";
        ddlDepartment.DataValueField = "id";
        ddlDepartment.DataBind();

        ddlDepartment.Items.Insert(0, new ListItem("All", "-1"));
    }

    private void FillSupervisor()
    {
        DataSet ds = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();
        ds = objBL_User.getEMPSuper(objPropUser);
        ddlSuper.DataSource = ds.Tables[0];
        ddlSuper.DataTextField = "super";
        ddlSuper.DataValueField = "super";
        ddlSuper.DataBind();

        ddlSuper.Items.Insert(0, new ListItem("All", ""));
    }

    private void FillWorker()
    {
        try
        {

            ddlworker.Items.Clear();
            DataSet ds = new DataSet();
            objPropUser.ConnConfig = Session["config"].ToString();
            objPropUser.Supervisor = ddlSuper.SelectedValue;
            objPropUser.Status = 1;
            ds = objBL_User.getEMPwithDeviceID(objPropUser);

            ddlworker.DataSource = ds.Tables[0];
            ddlworker.DataTextField = "fdesc";
            ddlworker.DataValueField = "id";
            ddlworker.DataBind();

            ddlworker.Items.Insert(0, new ListItem("All", "0"));
        }
        catch { }
    }

    private void GetTimesheetEmp()
    {
        string stdate = txtFromDate.Text + " 00:00:00";
        string enddate = txtToDate.Text + " 23:59:59";
        objPropUser.ConnConfig = Session["config"].ToString();
        objPropUser.Startdt = Convert.ToDateTime(stdate);
        objPropUser.Enddt = Convert.ToDateTime(enddate);
        double weeks = (objPropUser.Startdt - objPropUser.Enddt).TotalDays / 7;
        hdnWeeks.Value = weeks.ToString();
        DataSet dsSaved = objBL_User.getSavedTimesheet(objPropUser);
        if (dsSaved.Tables[0].Rows.Count > 0)
        {
            ViewState["saved"] = 1;
            GetSavedTimesheetEmp();

        }
        else
        {
            ViewState["saved"] = 0;
            GetTimesheetEmpTicket();
        }

        ////gvTimesheet.DataBind();
        //RadPersistenceManager_Timesheet.SaveState();
    }

    private DataSet GetTimesheetData()
    {
        string stdate = txtFromDate.Text + " 00:00:00";
        string enddate = txtToDate.Text + " 23:59:59";
        objPropUser.ConnConfig = Session["config"].ToString();
        DataSet ds = new DataSet();
        objPropUser.Startdt = Convert.ToDateTime(stdate);
        objPropUser.Enddt = Convert.ToDateTime(enddate);
        objPropUser.DepartmentID = Convert.ToInt32(ddlDepartment.SelectedValue);
        objPropUser.Supervisor = ddlSuper.SelectedValue;
        objPropUser.WorkId = Convert.ToInt32(ddlworker.SelectedValue);
        int Etimesheet = Convert.ToInt32(ddlTimesheet.SelectedValue);
        #region Company Check
        objPropUser.UserID = Convert.ToInt32(Session["UserID"].ToString());
        if (Convert.ToString(Session["CmpChkDefault"]) == "1")
        {
            objPropUser.EN = 1;
        }
        else
        {
            objPropUser.EN = 0;
        }
        #endregion
        ds = objBL_User.getTimesheetEmp(objPropUser, Etimesheet);

        return ds;
    }

    private void GetTimesheetEmpTicket()
    {
        lnkSaved.Visible = false;
        lnkBack.Visible = false;
        //gvTimesheet.Enabled = true;
        lblProcessed.Visible = false;
        lblSaved.Visible = false;
        lnkMerge.Visible = false;

        DataSet ds = GetTimesheetData();
        //gvTimesheet.DataSource = ds.Tables[0];
        //gvTimesheet.VirtualItemCount = ds.Tables[0].Rows.Count;  
        

        lblRecordCount.Text = ds.Tables[0].Rows.Count.ToString() + " Record(s) found";
    }

    private DataTable GetSavedTimedata()
    {
        string stdate = txtFromDate.Text + " 00:00:00";
        string enddate = txtToDate.Text + " 23:59:59";
        objPropUser.ConnConfig = Session["config"].ToString();
        DataSet ds = new DataSet();
        objPropUser.Startdt = Convert.ToDateTime(stdate);
        objPropUser.Enddt = Convert.ToDateTime(enddate);
        objPropUser.DepartmentID = Convert.ToInt32(ddlDepartment.SelectedValue);
        objPropUser.Supervisor = ddlSuper.SelectedValue;
        ds = objBL_User.getSavedTimesheetEmp(objPropUser);

        return ds.Tables[0];
    }

    private void GetSavedTimesheetEmp()
    {
        objPropUser.ConnConfig = Session["config"].ToString();
        DataTable dt = GetSavedTimedata();

        lblRecordCount.Text = dt.Rows.Count.ToString() + " Record(s) found.";

        if (dt.Rows.Count > 0)
        {
            if (dt.Rows[0]["processed"].ToString() == "1")
            {
               // gvTimesheet.Enabled = false;
                lnkProcess.Visible = false;
                lblProcessed.Visible = true;
                lnkSaved.Visible = false;
                lnkMerge.Visible = false;
            }
            else
            {
                //gvTimesheet.Enabled = true;
                lnkSave.Visible = true;
                lblProcessed.Visible = false;
                lblSaved.Visible = true;
            }
            lnkBack.Visible = false;
        }

        //gvTimesheet.DataSource = dt;
        //gvTimesheet.VirtualItemCount = dt.Rows.Count;
       
    }

    private DataTable GetTimesheetTicketsByEmp(int EmpID)
    {
        string stdate = txtFromDate.Text + " 00:00:00";
        string enddate = txtToDate.Text + " 23:59:59";
        objPropUser.ConnConfig = Session["config"].ToString();
        DataSet ds = new DataSet();
        objPropUser.Startdt = Convert.ToDateTime(stdate);
        objPropUser.Enddt = Convert.ToDateTime(enddate);
        objPropUser.EmpId = EmpID;
        objPropUser.saved = (int)ViewState["saved"];
        objPropUser.unsaved = (int)ViewState["update"];
        int Etimesheet = Convert.ToInt32(ddlTimesheet.SelectedValue);
        ds = objBL_User.GetTimesheetTicketsByEmp(objPropUser, Etimesheet);
        return ds.Tables[0];
    }

    //private DataTable GetTicketsData()
    //{
    //    DataTable dt = new DataTable();
    //    dt.Columns.Add("TicketID", typeof(int));
    //    dt.Columns.Add("Reg", typeof(double));
    //    dt.Columns.Add("OT", typeof(double));
    //    dt.Columns.Add("DT", typeof(double));
    //    dt.Columns.Add("TT", typeof(double));
    //    dt.Columns.Add("NT", typeof(double));
    //    dt.Columns.Add("Zone", typeof(double));
    //    dt.Columns.Add("Mileage", typeof(double));
    //    dt.Columns.Add("Misc", typeof(double));
    //    dt.Columns.Add("Toll", typeof(double));
    //    dt.Columns.Add("HourlyRate", typeof(double));
    //    dt.Columns.Add("Extra", typeof(double));
    //    dt.Columns.Add("Empid", typeof(int));
    //    dt.Columns.Add("Custom", typeof(double));
    //    dt.Columns.Add("CustomTick3", typeof(int));
    //    dt.Columns.Add("CustomTick2", typeof(double));
    //    dt.Columns.Add("CustomTick1", typeof(double));

    //    foreach (GridViewRow gr in gvTimesheet.MasterTableView.Items)
    //    {
    //        GridView gvTickets = (GridView)gr.FindControl("gvTickets");
    //        Label lblID = (Label)gr.FindControl("lblID");
    //        //int extra = 0;
    //        foreach (GridViewRow grTic in gvTickets.Rows)
    //        {
    //            //if (extra != 0)
    //            //{
    //            HyperLink lnkTick = (HyperLink)grTic.FindControl("lnkTick");
    //            TextBox txtRate = (TextBox)grTic.FindControl("txtTRate");
    //            TextBox txtReg = (TextBox)grTic.FindControl("txtTReg");
    //            TextBox txtOT = (TextBox)grTic.FindControl("txtTOT");
    //            TextBox txtDT = (TextBox)grTic.FindControl("txtTDT");
    //            TextBox txtNT = (TextBox)grTic.FindControl("txtTNT");
    //            TextBox txtTT = (TextBox)grTic.FindControl("txtTravel");
    //            TextBox txtZone = (TextBox)grTic.FindControl("txtTZone");
    //            TextBox txtMileage = (TextBox)grTic.FindControl("txtTMileage");
    //            TextBox txtExtra = (TextBox)grTic.FindControl("txtTExtra");
    //            TextBox txtTMisc = (TextBox)grTic.FindControl("txtTMisc");
    //            TextBox txtPToll = (TextBox)grTic.FindControl("txtPToll");
    //            TextBox txtCustom = (TextBox)grTic.FindControl("txtTCustom");
    //            TextBox txtTJobRate = (TextBox)grTic.FindControl("txtTJobRate");
    //            TextBox txtTCustomTick2 = (TextBox)grTic.FindControl("txtTCustomTick2");
    //            CheckBox chkTJobRate = (CheckBox)grTic.FindControl("chkTJobRate");

    //            DataRow dr = dt.NewRow();
    //            dr["TicketID"] = Convert.ToInt32(objGeneral.IsNull(lnkTick.Text, "0"));
    //            dr["Reg"] = Convert.ToDouble(objGeneral.IsNull(txtReg.Text.Trim(), "0"));
    //            dr["OT"] = Convert.ToDouble(objGeneral.IsNull(txtOT.Text.Trim(), "0"));
    //            dr["DT"] = Convert.ToDouble(objGeneral.IsNull(txtDT.Text.Trim(), "0"));
    //            dr["TT"] = Convert.ToDouble(objGeneral.IsNull(txtTT.Text.Trim(), "0"));
    //            dr["NT"] = Convert.ToDouble(objGeneral.IsNull(txtNT.Text.Trim(), "0"));
    //            dr["Zone"] = Convert.ToDouble(objGeneral.IsNull(txtZone.Text.Trim(), "0"));
    //            dr["Mileage"] = Convert.ToDouble(objGeneral.IsNull(txtMileage.Text.Trim(), "0"));
    //            dr["HourlyRate"] = Convert.ToDouble(objGeneral.IsNull(txtRate.Text.Trim(), "0"));
    //            dr["Extra"] = Convert.ToDouble(objGeneral.IsNull(txtExtra.Text.Trim(), "0"));
    //            dr["Misc"] = Convert.ToDouble(objGeneral.IsNull(txtTMisc.Text.Trim(), "0"));
    //            dr["Toll"] = Convert.ToDouble(objGeneral.IsNull(txtPToll.Text.Trim(), "0"));
    //            dr["Empid"] = Convert.ToInt32(objGeneral.IsNull(lblID.Text.Trim(), "0"));
    //            dr["Custom"] = Convert.ToDouble(objGeneral.IsNull(txtCustom.Text.Trim(), "0"));
    //            dr["CustomTick3"] = Convert.ToInt32(chkTJobRate.Checked);
    //            dr["CustomTick1"] = Convert.ToDouble(objGeneral.IsNull(txtTJobRate.Text.Trim(), "0"));
    //            dr["CustomTick2"] = Convert.ToDouble(objGeneral.IsNull(txtTCustomTick2.Text.Trim(), "0"));
    //            dt.Rows.Add(dr);
    //            //}
    //            //extra++;
    //        }
    //    }
    //    return dt;
    //}

    //private void Submit(int processed)
    //{
    //    DataTable dt = new DataTable();
    //    dt.Columns.Add("TimesheetID", typeof(int));
    //    dt.Columns.Add("EmpID", typeof(int));
    //    dt.Columns.Add("Pay", typeof(int));
    //    dt.Columns.Add("PayMethod", typeof(int));
    //    dt.Columns.Add("Reg", typeof(double));
    //    dt.Columns.Add("OT", typeof(double));
    //    dt.Columns.Add("DT", typeof(double));
    //    dt.Columns.Add("TT", typeof(double));
    //    dt.Columns.Add("NT", typeof(double));
    //    dt.Columns.Add("Holiday", typeof(double));
    //    dt.Columns.Add("Vacation", typeof(double));
    //    dt.Columns.Add("SickTime", typeof(double));
    //    dt.Columns.Add("Zone", typeof(double));
    //    dt.Columns.Add("Reimb", typeof(double));
    //    dt.Columns.Add("Mileage", typeof(double));
    //    dt.Columns.Add("Bonus", typeof(double));
    //    dt.Columns.Add("Extra", typeof(double));
    //    dt.Columns.Add("Misc", typeof(double));
    //    dt.Columns.Add("Toll", typeof(double));
    //    dt.Columns.Add("Total", typeof(double));

    //    dt.Columns.Add("FixedHours", typeof(double));
    //    dt.Columns.Add("Salary", typeof(double));
    //    dt.Columns.Add("MileRate", typeof(double));
    //    dt.Columns.Add("HourRate", typeof(double));
    //    dt.Columns.Add("DollarAmount", typeof(double));
    //    dt.Columns.Add("Custom", typeof(double));

    //    foreach (GridViewRow gr in gvTimesheet.MasterTableView.Items)
    //    {
    //        Label lblID = (Label)gr.FindControl("lblID");
    //        CheckBox chkPay = (CheckBox)gr.FindControl("chkPay");
    //        Label lblMethod = (Label)gr.FindControl("lblMID");
    //        Label txtReg = (Label)gr.FindControl("txtReg");
    //        Label txtOT = (Label)gr.FindControl("txtOT");
    //        Label txtDT = (Label)gr.FindControl("txtDT");
    //        Label txtoneseven = (Label)gr.FindControl("txtoneseven");
    //        Label txtTT = (Label)gr.FindControl("txtPTravel");
    //        Label txtHoliday = (Label)gr.FindControl("txtHoliday");
    //        Label txtVacation = (Label)gr.FindControl("txtVacation");
    //        Label txtSick = (Label)gr.FindControl("txtSick");
    //        Label txtZone = (Label)gr.FindControl("txtZone");
    //        Label txtReimb = (Label)gr.FindControl("txtReimb");
    //        Label txtMileage = (Label)gr.FindControl("txtMileage");
    //        Label txtBonus = (Label)gr.FindControl("txtBonus");
    //        Label txtExtra = (Label)gr.FindControl("txtExtra");
    //        Label txtTotal = (Label)gr.FindControl("txtTotal");
    //        Label txtMisc = (Label)gr.FindControl("txtMisc");
    //        Label txtToll = (Label)gr.FindControl("txtToll");
    //        Label txtAmount = (Label)gr.FindControl("txtAmount");
    //        Label txtCustom = (Label)gr.FindControl("txtCustom");

    //        //TextBox txtFixedH = (TextBox)gr.FindControl("txtFixedH");
    //        Label txtSalary = (Label)gr.FindControl("txtSalary");
    //        Label lblHourlyRate = (Label)gr.FindControl("lblHourlyRate");
    //        Label lblMlRate = (Label)gr.FindControl("lblMlRate");

    //        DataRow dr = dt.NewRow();
    //        dr["EmpID"] = lblID.Text;
    //        dr["Pay"] = Convert.ToInt16(chkPay.Checked);
    //        if (lblMethod.Text != string.Empty)
    //            dr["PayMethod"] = Convert.ToInt16(lblMethod.Text);
    //        dr["Reg"] = Convert.ToDouble(objGeneral.IsNull(txtReg.Text.Trim(), "0"));
    //        dr["OT"] = Convert.ToDouble(objGeneral.IsNull(txtOT.Text.Trim(), "0"));
    //        dr["DT"] = Convert.ToDouble(objGeneral.IsNull(txtDT.Text.Trim(), "0"));
    //        dr["TT"] = Convert.ToDouble(objGeneral.IsNull(txtTT.Text.Trim(), "0"));
    //        dr["NT"] = Convert.ToDouble(objGeneral.IsNull(txtoneseven.Text.Trim(), "0"));
    //        dr["Holiday"] = Convert.ToDouble(objGeneral.IsNull(txtHoliday.Text.Trim(), "0"));
    //        dr["Vacation"] = Convert.ToDouble(objGeneral.IsNull(txtVacation.Text.Trim(), "0"));
    //        dr["SickTime"] = Convert.ToDouble(objGeneral.IsNull(txtSick.Text.Trim(), "0"));
    //        dr["Zone"] = Convert.ToDouble(objGeneral.IsNull(txtZone.Text.Trim(), "0"));
    //        dr["Reimb"] = Convert.ToDouble(objGeneral.IsNull(txtReimb.Text.Trim(), "0"));
    //        dr["Mileage"] = Convert.ToDouble(objGeneral.IsNull(txtMileage.Text.Trim(), "0"));
    //        dr["Bonus"] = Convert.ToDouble(objGeneral.IsNull(txtBonus.Text.Trim(), "0"));
    //        dr["Extra"] = Convert.ToDouble(objGeneral.IsNull(txtExtra.Text.Trim(), "0"));
    //        dr["Total"] = Convert.ToDouble(objGeneral.IsNull(txtTotal.Text.Trim(), "0"));
    //        dr["Misc"] = Convert.ToDouble(objGeneral.IsNull(txtMisc.Text.Trim(), "0"));
    //        dr["Toll"] = Convert.ToDouble(objGeneral.IsNull(txtToll.Text.Trim(), "0"));

    //        dr["Salary"] = Convert.ToDouble(objGeneral.IsNull(txtSalary.Text.Trim(), "0"));
    //        dr["MileRate"] = Convert.ToDouble(objGeneral.IsNull(lblMlRate.Text.Trim(), "0"));
    //        dr["HourRate"] = Convert.ToDouble(objGeneral.IsNull(lblHourlyRate.Text.Trim(), "0"));
    //        dr["DollarAmount"] = Convert.ToDouble(objGeneral.IsNull(txtAmount.Text.Trim(), "0"));
    //        dr["Custom"] = Convert.ToDouble(objGeneral.IsNull(txtCustom.Text.Trim(), "0"));

    //        dt.Rows.Add(dr);
    //    }

    //    objPropUser.ConnConfig = Session["config"].ToString();
    //    objPropUser.Startdt = Convert.ToDateTime(txtFromDate.Text.Trim());
    //    objPropUser.Enddt = Convert.ToDateTime(txtToDate.Text.Trim());
    //    objPropUser.EmpData = dt;
    //    objPropUser.dtTicketData = GetTicketsData();
    //    objPropUser.IsSuper = processed;
    //    objBL_User.AddTimesheet(objPropUser);
    //}

    protected void lnkProcess_Click(object sender, EventArgs e)
    {
        //Submit(1);

        //gvTimesheet.Rebind();
    }

    protected void lnkClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("Ticketlistview.aspx");
    }

    protected void lnkSearch_Click(object sender, EventArgs e)
    {
        RemoveActiveLabel();
        rdDay.Checked = false;
        rdWeek.Checked = false;
        rdMonth.Checked = false;
        rdQuarter.Checked = false;
        rdYear.Checked = false;
        timeSelectionPanel.Update();
        timeSelectionPanel.Visible = true;

        if (!IsDateSelectionValid())
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "showMessage", string.Format("ShowMessage('{0}','{1}');", "Invalid date format.", "error"), true);
            //gvTimesheet.Rebind();
            return;
        }
        SaveFilter();
        
        //gvTimesheet.Rebind();
        
    }

    public string BindList(object Field)
    {
        string ret = string.Empty;
        if (Field != DBNull.Value)
        {
            if (Convert.ToDouble(Field) != 0)
                ret = Field.ToString();
        }
        return ret;
    }

    protected void lnkSave_Click(object sender, EventArgs e)
    {
        //Submit(0);
        //gvTimesheet.Rebind();

        
    }


    protected void lnkExport_Click(object sender, EventArgs e)
    {
        //objPropUser.ConnConfig = Session["config"].ToString();
        //objPropUser.Startdt = Convert.ToDateTime(txtFromDate.Text.Trim());
        //objPropUser.Enddt = Convert.ToDateTime(txtToDate.Text.Trim());
        //DataSet dsExport = objBL_User.getGetSageExportTickets(objPropUser);

        //if (ddlExport.SelectedItem.Text == "CSV")
        //    ExportToCSV(dsExport.Tables[0]);
        //else if (ddlExport.SelectedItem.Text == "Excel")
        //    ExportToExcel(dsExport.Tables[0]);
        //else if (ddlExport.SelectedItem.Text == "Text")
        //    ExportToText(dsExport.Tables[0]);
    }

    private void ExportToCSV(DataTable dt)
    {
        Response.ClearContent();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "JobExpense.csv"));
        Response.ContentType = "text/csv";

        StringBuilder sb = new StringBuilder();

        foreach (DataRow row in dt.Rows)
        {
            IEnumerable<string> fields = row.ItemArray.Select(field => field.ToString());
            sb.AppendLine(string.Join(",", fields));
        }

        Response.Write(sb);
        Response.End();
    }

    private void ExportToExcel(DataTable dt)
    {
        Response.ClearContent();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "JobExpense.xls"));
        Response.ContentType = "application/ms-excel";

        string str = string.Empty;
        foreach (DataRow dr in dt.Rows)
        {
            str = "";
            for (int j = 0; j < dt.Columns.Count; j++)
            {
                Response.Write(str + Convert.ToString(dr[j]));
                str = "\t";
            }
            Response.Write("\n");
        }
        Response.End();
    }

    private void ExportToText(DataTable dt)
    {
        Response.ClearContent();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "JobExpense.txt"));
        Response.ContentType = "text/plain";

        string str = string.Empty;
        foreach (DataRow dr in dt.Rows)
        {
            str = "";
            for (int j = 0; j < dt.Columns.Count; j++)
            {
                Response.Write(str + Convert.ToString(dr[j]));
                str = "\t";
            }
            Response.Write("\n");
        }
        Response.End();
    }

    private DateTime GetLastDayOfMonth(DateTime dateTime)
    {
        return new DateTime(dateTime.Year, dateTime.Month, DateTime.DaysInMonth(dateTime.Year, dateTime.Month));
    }

    protected void incDate_Click(object sender, EventArgs e)
    {
        if (rdDay.Checked)
        {
            txtFromDate.Text = Convert.ToDateTime(txtFromDate.Text).AddDays(1).ToShortDateString();
            txtToDate.Text = Convert.ToDateTime(txtToDate.Text).AddDays(1).ToShortDateString();
            Session["ToDate"] = txtToDate.Text;
            Session["fromDate"] = txtFromDate.Text;

        }

        if (rdWeek.Checked)
        {

            txtFromDate.Text = Convert.ToDateTime(txtFromDate.Text).AddDays(7).ToShortDateString();
            txtToDate.Text = Convert.ToDateTime(txtToDate.Text).AddDays(7).ToShortDateString();
            Session["ToDate"] = txtToDate.Text;
            Session["fromDate"] = txtFromDate.Text;
        }
        if (rdMonth.Checked)
        {

            txtFromDate.Text = Convert.ToDateTime(txtFromDate.Text).AddMonths(1).ToShortDateString();

            txtToDate.Text = GetLastDayOfMonth(Convert.ToDateTime(txtFromDate.Text)).ToShortDateString();//Convert.ToDateTime(txtToDate.Text).AddMonths(1).ToShortDateString();
            Session["ToDate"] = txtToDate.Text;
            Session["fromDate"] = txtFromDate.Text;
        }
        if (rdQuarter.Checked)
        {
            txtFromDate.Text = Convert.ToDateTime(txtFromDate.Text).AddMonths(3).ToShortDateString();
            txtToDate.Text = GetLastDayOfMonth(Convert.ToDateTime(txtToDate.Text).AddMonths(3)).ToShortDateString();
            Session["ToDate"] = txtToDate.Text;
            Session["fromDate"] = txtFromDate.Text;
        }
        if (rdYear.Checked)
        {
            txtFromDate.Text = Convert.ToDateTime(txtFromDate.Text).AddYears(1).ToShortDateString();
            txtToDate.Text = Convert.ToDateTime(txtToDate.Text).AddYears(1).ToShortDateString();
            Session["ToDate"] = txtToDate.Text;
            Session["fromDate"] = txtFromDate.Text;
        }

    }

    protected void decDate_Click(object sender, EventArgs e)
    {
        if (rdDay.Checked)
        {
            txtFromDate.Text = Convert.ToDateTime(txtFromDate.Text).AddDays(-1).ToShortDateString();
            txtToDate.Text = Convert.ToDateTime(txtToDate.Text).AddDays(-1).ToShortDateString();
            Session["ToDate"] = txtToDate.Text;
            Session["fromDate"] = txtFromDate.Text;

        }

        if (rdWeek.Checked)
        {

            txtFromDate.Text = Convert.ToDateTime(txtFromDate.Text).AddDays(-7).ToShortDateString();
            txtToDate.Text = Convert.ToDateTime(txtToDate.Text).AddDays(-7).ToShortDateString();
            Session["ToDate"] = txtToDate.Text;
            Session["fromDate"] = txtFromDate.Text;

        }
        if (rdMonth.Checked)
        {

            txtFromDate.Text = Convert.ToDateTime(txtFromDate.Text).AddMonths(-1).ToShortDateString();

            txtToDate.Text = GetLastDayOfMonth(Convert.ToDateTime(txtFromDate.Text)).ToShortDateString();//Convert.ToDateTime(txtToDate.Text).AddMonths(-1).ToShortDateString();
            Session["ToDate"] = txtToDate.Text;
            Session["fromDate"] = txtFromDate.Text;
        }
        if (rdQuarter.Checked)
        {
            txtFromDate.Text = Convert.ToDateTime(txtFromDate.Text).AddMonths(-3).ToShortDateString();
            txtToDate.Text = GetLastDayOfMonth(Convert.ToDateTime(txtToDate.Text).AddMonths(-3)).ToShortDateString();
            Session["ToDate"] = txtToDate.Text;
            Session["fromDate"] = txtFromDate.Text;
        }
        if (rdYear.Checked)
        {
            txtFromDate.Text = Convert.ToDateTime(txtFromDate.Text).AddYears(-1).ToShortDateString();
            txtToDate.Text = Convert.ToDateTime(txtToDate.Text).AddYears(-1).ToShortDateString();
            Session["ToDate"] = txtToDate.Text;
            Session["fromDate"] = txtFromDate.Text;
        }

    }

    protected void rdDay_CheckedChanged(object sender, EventArgs e)
    {
        RemoveActiveLabel();
        lblDay.Attributes["class"] = "labelactive";
        txtFromDate.Text = DateTime.Now.ToShortDateString();
        txtToDate.Text = DateTime.Now.ToShortDateString();
        Session["ToDate"] = txtToDate.Text;
        Session["fromDate"] = txtFromDate.Text;

    }

    protected void rdWeek_CheckedChanged(object sender, EventArgs e)
    {
        RemoveActiveLabel();
        lblWeek.Attributes["class"] = "labelactive";
        var now = System.DateTime.Now;
        var FisrtDay = now.AddDays(-((now.DayOfWeek - System.Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat.FirstDayOfWeek + 7) % 7)).Date;
        txtFromDate.Text = FisrtDay.ToShortDateString();
        var LastDay = FisrtDay.AddDays(7).AddSeconds(-1);

        txtToDate.Text = LastDay.ToShortDateString();
        Session["ToDate"] = txtToDate.Text;
        Session["fromDate"] = txtFromDate.Text;

    }

    protected void rdMonth_CheckedChanged(object sender, EventArgs e)
    {
        RemoveActiveLabel();
        lblMonth.Attributes["class"] = "labelactive";
        var Date = System.DateTime.Now;
        var firstDayOfMonth = new DateTime(Date.Year, Date.Month, 1);
        var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
        txtFromDate.Text = firstDayOfMonth.ToShortDateString();
        txtToDate.Text = lastDayOfMonth.ToShortDateString();
        Session["ToDate"] = txtToDate.Text;
        Session["fromDate"] = txtFromDate.Text;


    }

    protected void rdQuarter_CheckedChanged(object sender, EventArgs e)
    {
        RemoveActiveLabel();
        lblQuarter.Attributes["class"] = "labelactive";
        var date = System.DateTime.Now;
        int quarterNumber = (date.Month - 1) / 3 + 1;
        DateTime firstDayOfQuarter = new DateTime(date.Year, (quarterNumber - 1) * 3 + 1, 1);
        DateTime lastDayOfQuarter = firstDayOfQuarter.AddMonths(3).AddDays(-1);
        txtFromDate.Text = firstDayOfQuarter.ToShortDateString();
        txtToDate.Text = lastDayOfQuarter.ToShortDateString();
        Session["ToDate"] = txtToDate.Text;
        Session["fromDate"] = txtFromDate.Text;

    }

    protected void rdYear_CheckedChanged(object sender, EventArgs e)
    {
        RemoveActiveLabel();
        lblYear.Attributes["class"] = "labelactive";
        int year = DateTime.Now.Year;
        DateTime firstDay = new DateTime(year, 1, 1);
        DateTime lastDay = new DateTime(year, 12, 31);
        txtFromDate.Text = firstDay.ToShortDateString();
        txtToDate.Text = lastDay.ToShortDateString();
        Session["ToDate"] = txtToDate.Text;
        Session["fromDate"] = txtFromDate.Text;
    }

    private void RemoveActiveLabel()
    {
        lblDay.Attributes["class"] = string.Empty;
        lblWeek.Attributes["class"] = string.Empty;
        lblMonth.Attributes["class"] = string.Empty;
        lblQuarter.Attributes["class"] = string.Empty;
        lblYear.Attributes["class"] = string.Empty;
    }

    private bool IsDateSelectionValid()
    {
        DateTime result;
        return DateTime.TryParse(txtFromDate.Text, out result) && DateTime.TryParse(txtToDate.Text, out result);
    }

    protected void gvTimesheet_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        if (e.IsFromDetailTable)
        {
            return;
        }

        if (!IsDateSelectionValid())
        {
            //gvTimesheet.DataSource = string.Empty;
            //gvTimesheet.VirtualItemCount = 0;
            return;
        }
        try
        {
            GetTimesheetEmp();
        }
        catch
        {
            //gvTimesheet.DataSource = string.Empty;
            //gvTimesheet.VirtualItemCount = 0;
            //TODO: Log error
        }
    }

    protected void gvTimesheet_DetailTableDataBind(object sender, GridDetailTableDataBindEventArgs e)
    {
        var dataItem = e.DetailTableView.ParentItem;
        switch (e.DetailTableView.Name)
        {
            case "EditTickets":
                {
                    string id = dataItem.GetDataKeyValue("ID").ToString();
                    DataTable dt = GetTimesheetTicketsByEmp(Convert.ToInt32(id));
                    var row = dt.Rows.Count;
                    if (row > 0)
                    {
                        var asa = dt.Rows;
                        var colums = dt.Columns.OfType<DataColumn>().Select(t => t.ColumnName).ToList();
                    }
                    e.DetailTableView.DataSource = dt;
                    break;
                }
        }
    }

    //protected void gvTimesheet_PreRender(object sender, EventArgs e)
    //{

    //    if (intExportExcel == 1)
    //    {
    //        GeneralFunctions obj = new GeneralFunctions();
    //        obj.CorrectTelerikPager(gvTimesheet);
    //        intExportExcel = 0;
    //    }
    //    var nestedViewItems = gvTimesheet.MasterTableView.GetItems(GridItemType.NestedView);


    //    foreach (GridNestedViewItem nestedViewItem in nestedViewItems)
    //    {
    //        var parentItem = nestedViewItem.ParentItem;
    //        Label lblMID = parentItem.FindControl("lblMID") as Label;
    //        if (lblMID.Text == "0")
    //        {
    //            Label lblDlrH = (Label)parentItem.FindControl("lblDlrH");
    //            Label lblDlrV = (Label)parentItem.FindControl("lblDlrV");
    //            Label lblDlrS = (Label)parentItem.FindControl("lblDlrS");
    //            lblDlrH.Visible = true;
    //            lblDlrV.Visible = true;
    //            lblDlrS.Visible = true;
    //        }

    //        if (lblProcessed.Visible == false)
    //        {

    //            string EmpID = parentItem.GetDataKeyValue("ID").ToString();
    //            double countDetail = Convert.ToDouble(objGeneral.IsNull(parentItem.GetDataKeyValue("countDetail").ToString(), "0"));
    //            double Reg1 = Convert.ToDouble(objGeneral.IsNull(parentItem.GetDataKeyValue("Reg1").ToString(), "0"));
    //            double OT1 = Convert.ToDouble(objGeneral.IsNull(parentItem.GetDataKeyValue("OT1").ToString(), "0"));
    //            double DT1 = Convert.ToDouble(objGeneral.IsNull(parentItem.GetDataKeyValue("DT1").ToString(), "0"));
    //            double TT1 = Convert.ToDouble(objGeneral.IsNull(parentItem.GetDataKeyValue("TT1").ToString(), "0"));
    //            double NT1 = Convert.ToDouble(objGeneral.IsNull(parentItem.GetDataKeyValue("NT1").ToString(), "0"));
    //            double Zone1 = Convert.ToDouble(objGeneral.IsNull(parentItem.GetDataKeyValue("Zone1").ToString(), "0"));
    //            double Mileage1 = Convert.ToDouble(objGeneral.IsNull(parentItem.GetDataKeyValue("Mileage1").ToString(), "0"));
    //            double Extra1 = Convert.ToDouble(objGeneral.IsNull(parentItem.GetDataKeyValue("Extra1").ToString(), "0"));
    //            double Misc1 = Convert.ToDouble(objGeneral.IsNull(parentItem.GetDataKeyValue("Misc1").ToString(), "0"));
    //            double Toll1 = Convert.ToDouble(objGeneral.IsNull(parentItem.GetDataKeyValue("Toll1").ToString(), "0"));
    //            double HourRate1 = Convert.ToDouble(objGeneral.IsNull(parentItem.GetDataKeyValue("HourRate1").ToString(), "0"));
    //            if (countDetail > 0)
    //            {

    //                var dt = GetTimesheetTicketsByEmp(Convert.ToInt32(EmpID));
    //                if (dt.Rows.Count > 0)
    //                {
    //                    DataRow drrow = dt.NewRow();
    //                    drrow["Reg"] = Reg1;
    //                    drrow["OT"] = OT1;
    //                    drrow["DT"] = DT1;
    //                    drrow["TT"] = TT1;
    //                    drrow["NT"] = NT1;
    //                    drrow["Zone"] = Zone1;
    //                    drrow["Mileage"] = Mileage1;
    //                    drrow["OtherE"] = Misc1;
    //                    drrow["Toll"] = Toll1;
    //                    drrow["Extra"] = Extra1;
    //                    drrow["HourlyRate"] = HourRate1;
    //                    dt.Rows.InsertAt(drrow, 0);
    //                    GridTableView nestedView = nestedViewItem.NestedTableViews.First();
    //                    nestedView.DataSource = dt;
    //                    nestedView.DataBind();
                        
    //                }
    //                else
    //                {
    //                    //Hide expand button
    //                    GridTableView nestedView = nestedViewItem.NestedTableViews.First();
    //                    TableCell cell = nestedView.ParentItem["ExpandColumn"];
    //                    cell.Controls[0].Visible = false;
    //                    cell.Text = "&nbsp";
    //                    nestedViewItem.Visible = false;
    //                }
    //            }
    //            else
    //            {
    //                //Hide expand button
    //                GridTableView nestedView = nestedViewItem.NestedTableViews.First();
    //                TableCell cell = nestedView.ParentItem["ExpandColumn"];
    //                cell.Controls[0].Visible = false;
    //                cell.Text = "&nbsp";
    //                nestedViewItem.Visible = false;
    //            }


    //        }
    //    }              

    //    if (Convert.ToString(gvTimesheet.MasterTableView.FilterExpression) != "")
    //    {
    //        lblRecordCount.Text = gvTimesheet.MasterTableView.Items.Count + " Record(s) found";
    //    }
    //    else
    //    {
    //        lblRecordCount.Text = gvTimesheet.VirtualItemCount + " Record(s) found";
    //    }
    //    ScriptManager.RegisterClientScriptBlock(Page, typeof(RadGrid), "OnTimesheetGvRendered",
    //                             "$(function () {{OnTimesheetGvRendered();}});", true);
    //    if (Session["rowExpand"] != null)
    //    {
    //        int index = Convert.ToInt16(Session["rowExpand"]);
    //        gvTimesheet.MasterTableView.Items[index].Expanded = true; 
    //        gvTimesheet.MasterTableView.Items[index].ChildItem.NestedTableViews[0].Items[0].Expanded = true;
    //    }
    //    Session["rowExpand"] = null;
    //}

    protected void gvTimesheet_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName == RadGrid.ExpandCollapseCommandName)
        {
            if (!e.Item.Expanded)
            {
                var gridItem = e.Item as GridDataItem;
                if (gridItem != null)
                {
                    var detailTable = gridItem.ChildItem.NestedTableViews[0];
                    ScriptManager.RegisterClientScriptBlock(Page, typeof(RadGrid), "calculateDetailTable",
                                    string.Format("$(function () {{calculateDetailTable('{0}');}});", detailTable.ClientID), true);

                }
            }
            return;
        }
        if (e.CommandName == "UpdateTicket")
        {
            var gridItem = e.Item as GridDataItem;
            if (gridItem != null)
            {
                UpdateTicketItem(e.CommandArgument.ToString(), gridItem);
                Session["rowExpand"] = e.Item.OwnerTableView.ParentItem.ItemIndex;
                DataSet ds = GetTimesheetData();
                
                //gvTimesheet.DataSource = ds.Tables[0];
                //gvTimesheet.Rebind();
                
            }
        }
    }

    private void UpdateTicketItem(string id, GridDataItem item)
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("TicketID", typeof(int));
        dt.Columns.Add("Reg", typeof(double));
        dt.Columns.Add("OT", typeof(double));
        dt.Columns.Add("DT", typeof(double));
        dt.Columns.Add("TT", typeof(double));
        dt.Columns.Add("NT", typeof(double));
        dt.Columns.Add("Zone", typeof(double));
        dt.Columns.Add("Mileage", typeof(double));
        dt.Columns.Add("Misc", typeof(double));
        dt.Columns.Add("Toll", typeof(double));
        dt.Columns.Add("HourlyRate", typeof(double));
        dt.Columns.Add("Extra", typeof(double));
        dt.Columns.Add("Empid", typeof(int));
        dt.Columns.Add("Custom", typeof(double));
        dt.Columns.Add("CustomTick3", typeof(int));
        dt.Columns.Add("CustomTick2", typeof(double));
        dt.Columns.Add("CustomTick1", typeof(double));

        if (id != "")
        {
            int index = Convert.ToInt32(id);
            var grTic = item;
            TextBox txtRate = (TextBox)grTic.FindControl("txtTRate");
            TextBox txtReg = (TextBox)grTic.FindControl("txtTReg");
            TextBox txtOT = (TextBox)grTic.FindControl("txtTOT");
            TextBox txtDT = (TextBox)grTic.FindControl("txtTDT");
            TextBox txtNT = (TextBox)grTic.FindControl("txtTNT");
            TextBox txtTT = (TextBox)grTic.FindControl("txtTravel");
            TextBox txtZone = (TextBox)grTic.FindControl("txtTZone");
            TextBox txtMileage = (TextBox)grTic.FindControl("txtTMileage");
            TextBox txtExtra = (TextBox)grTic.FindControl("txtTExtra");
            TextBox txtTMisc = (TextBox)grTic.FindControl("txtTMisc");
            TextBox txtPToll = (TextBox)grTic.FindControl("txtPToll");
            TextBox txtCustom = (TextBox)grTic.FindControl("txtTCustom");
            TextBox txtTJobRate = (TextBox)grTic.FindControl("txtTJobRate");
            TextBox txtTCustomTick2 = (TextBox)grTic.FindControl("txtTCustomTick2");
            CheckBox chkTJobRate = (CheckBox)grTic.FindControl("chkTJobRate");

            DataRow dr = dt.NewRow();
            dr["TicketID"] = Convert.ToInt32(objGeneral.IsNull(index.ToString(), "0"));
            dr["Reg"] = Convert.ToDouble(objGeneral.IsNull(txtReg.Text.Trim().Split(',')[0], "0"));
            dr["OT"] = Convert.ToDouble(objGeneral.IsNull(txtOT.Text.Trim().Split(',')[0], "0"));
            dr["DT"] = Convert.ToDouble(objGeneral.IsNull(txtDT.Text.Trim().Split(',')[0], "0"));
            dr["TT"] = Convert.ToDouble(objGeneral.IsNull(txtTT.Text.Trim().Split(',')[0], "0"));
            dr["NT"] = Convert.ToDouble(objGeneral.IsNull(txtNT.Text.Trim().Split(',')[0], "0"));
            dr["Zone"] = Convert.ToDouble(objGeneral.IsNull(txtZone.Text.Trim().Split(',')[0], "0"));
            dr["Mileage"] = Convert.ToDouble(objGeneral.IsNull(txtMileage.Text.Trim().Split(',')[0], "0"));
            dr["HourlyRate"] = Convert.ToDouble(objGeneral.IsNull(txtRate.Text.Trim().Split(',')[0], "0"));
            dr["Extra"] = Convert.ToDouble(objGeneral.IsNull(txtExtra.Text.Trim().Split(',')[0], "0"));
            dr["Misc"] = Convert.ToDouble(objGeneral.IsNull(txtTMisc.Text.Trim().Split(',')[0], "0"));
            dr["Toll"] = Convert.ToDouble(objGeneral.IsNull(txtPToll.Text.Trim().Split(',')[0], "0"));
            dr["Empid"] = Convert.ToInt32(objGeneral.IsNull(index.ToString(), "0"));
            dr["Custom"] = Convert.ToDouble(objGeneral.IsNull(txtCustom.Text.Trim().Split(',')[0], "0"));
            dr["CustomTick3"] = Convert.ToInt32(chkTJobRate.Checked);
            dr["CustomTick1"] = Convert.ToDouble(objGeneral.IsNull(txtTJobRate.Text.Trim().Split(',')[0], "0"));
            dr["CustomTick2"] = Convert.ToDouble(objGeneral.IsNull(txtTCustomTick2.Text.Trim().Split(',')[0], "0"));
            dt.Rows.Add(dr);

            SqlParameter[] paraEmpData = new SqlParameter[5];
            paraEmpData[4] = new SqlParameter();
            paraEmpData[4].ParameterName = "TicketData";
            paraEmpData[4].SqlDbType = SqlDbType.Structured;
            paraEmpData[4].Value = dt;

            Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteNonQuery(Session["config"].ToString(), CommandType.StoredProcedure, "spUpdateTicketTimesheet", paraEmpData);
        }

        var detailTable = item.Parent.Parent;
        ScriptManager.RegisterClientScriptBlock(Page, typeof(RadGrid), "calculateDetailTable",
                        string.Format("$(function () {{calculateDetailTable('{0}');}});", detailTable.ClientID), true);       
    }

    protected void lnkExportToCSV_Click(object sender, EventArgs e)
    {
        objPropUser.ConnConfig = Session["config"].ToString();
        objPropUser.Startdt = Convert.ToDateTime(txtFromDate.Text.Trim());
        objPropUser.Enddt = Convert.ToDateTime(txtToDate.Text.Trim());
        DataSet dsExport = objBL_User.getGetSageExportTickets(objPropUser);
        ExportToCSV(dsExport.Tables[0]);
    }

    protected void lnkExportToText_Click(object sender, EventArgs e)
    {
        objPropUser.ConnConfig = Session["config"].ToString();
        objPropUser.Startdt = Convert.ToDateTime(txtFromDate.Text.Trim());
        objPropUser.Enddt = Convert.ToDateTime(txtToDate.Text.Trim());
        DataSet dsExport = objBL_User.getGetSageExportTickets(objPropUser);
        ExportToText(dsExport.Tables[0]);
    }

    protected void lnkExportToExcel_Click(object sender, EventArgs e)
    {
        objPropUser.ConnConfig = Session["config"].ToString();
        objPropUser.Startdt = Convert.ToDateTime(txtFromDate.Text.Trim());
        objPropUser.Enddt = Convert.ToDateTime(txtToDate.Text.Trim());
        DataSet dsExport = objBL_User.getGetSageExportTickets(objPropUser);
        ExportToExcel(dsExport.Tables[0]);
    }

    protected void lnkClear_Click(object sender, EventArgs e)
    {
        Session["filterstateEtimesheet"] = null;
        ddlDepartment.SelectedIndex = 0;
        ddlSuper.SelectedIndex = 0;
        ddlworker.SelectedIndex = 0;
        ddlTimesheet.SelectedIndex = 0;
        ResetGridState();
        //gvTimesheet.Rebind();
    }

    protected void gvTimesheet_ItemCreated(object sender, GridItemEventArgs e)
    {
        try
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
        catch { }
    }

    protected void lnkExcel_Click(object sender, EventArgs e)
    {
        intExportExcel = 1;
        //gvTimesheet.ExportSettings.FileName = "Etimesheet";
        //gvTimesheet.ExportSettings.IgnorePaging = true;
        //gvTimesheet.ExportSettings.ExportOnlyData = true;
        //gvTimesheet.ExportSettings.OpenInNewWindow = true;
        //gvTimesheet.ExportSettings.HideStructureColumns = true;
        //gvTimesheet.MasterTableView.UseAllDataFields = true;
        //gvTimesheet.ExportSettings.Excel.Format = GridExcelExportFormat.ExcelML;
        //gvTimesheet.MasterTableView.ExportToExcel();
    }

    protected void lnkTimeRecap_Click(object sender, EventArgs e)
    {
        Response.Redirect("Schedule_TimeRecapReport.aspx?StartDate=" + txtFromDate.Text + "&EndDate=" + txtToDate.Text + "&ddlDeprt=" + ddlDepartment.SelectedValue + "&Screen=etimesheet");
    }

    protected void lnkTimeRecapAll_Click(object sender, EventArgs e)
    {
        Response.Redirect("Schedule_TimeRecapReport.aspx?StartDate=" + txtFromDate.Text + "&EndDate=" + txtToDate.Text + "&type=all&ddlDeprt=" + ddlDepartment.SelectedValue + "&Screen=etimesheet");
    }

    protected void lnkTimeRecapwithlaborcost_Click(object sender, EventArgs e)
    {
        Response.Redirect("Schedule_TimeRecapReport_LaborCost.aspx?StartDate=" + txtFromDate.Text + "&EndDate=" + txtToDate.Text + "&ddlDeprt=" + ddlDepartment.SelectedValue + "&Screen=etimesheet");
    }

    protected void lnkTimeRecapWithCostAll_Click(object sender, EventArgs e)
    {
        Response.Redirect("Schedule_TimeRecapReport_LaborCost.aspx?StartDate=" + txtFromDate.Text + "&EndDate=" + txtToDate.Text + "&type=all&ddlDeprt=" + ddlDepartment.SelectedValue + "&Screen=etimesheet");
    }

    #region "Export PayRol"

    private String getCoCode()
    {
        String code = "";
        objPropUser.ConnConfig = Session["config"].ToString();
        DataSet dsSaved = objBL_User.GetCoCode(objPropUser);
        if (dsSaved.Tables[0].Rows.Count > 0)
        {
            code = dsSaved.Tables[0].Rows[0]["CoCode"].ToString();

        }
        return code;
    }

    private void updateCoCode()
    {
        String code = txtADP.Text;
        objPropUser.ConnConfig = Session["config"].ToString();
        objPropUser.CoCode = code;
        objBL_User.UpdateCoCode(objPropUser);
    }

    private void exportPayRoll()
    {
        try
        {

            objPropUser.ConnConfig = Session["config"].ToString();
            objPropUser.FStart = Convert.ToDateTime(txtFromDate.Text.Trim());
            objPropUser.FEnd = Convert.ToDateTime(txtToDate.Text.Trim() + " 23:59:59");
            DataSet ds = objBL_User.getPayRoll(objPropUser);
            gvPayRoll.DataSource = ds;
            gvPayRoll.Rebind();

            gvPayRoll.ExportSettings.FileName = "EPI28400";
            gvPayRoll.ExportSettings.IgnorePaging = true;
            gvPayRoll.ExportSettings.ExportOnlyData = true;
            gvPayRoll.ExportSettings.OpenInNewWindow = true;
            gvPayRoll.ExportSettings.HideStructureColumns = true;
            gvPayRoll.MasterTableView.UseAllDataFields = true;
            gvPayRoll.MasterTableView.ExportToCSV();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }

    }

    #endregion

    protected void lnkExportPayRoll_Click(object sender, EventArgs e)
    {

        if (hdnCoCode.Value.Trim() != "")
        {
            exportPayRoll();
        }
    }

    protected void lnkCoCodeSave_Click(object sender, EventArgs e)
    {
        if (txtADP.Text.Trim() != "")
        {
            updateCoCode();
            hdnCoCode.Value = txtADP.Text;
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "closeScript", "CloseCoCodeModalWindow();", true);

        }

    }

    protected void btnHiddenExport_Click(object sender, EventArgs e)
    {
        exportPayRoll();
    }

    protected void gvPayRoll_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem item = e.Item as GridDataItem;
            if (item["OTHours"].Text == "0.00")
            {
                item["OTHours"].Text = "";
            }
            if (item["RegHours"].Text == "0.00")
            {
                item["RegHours"].Text = "";
            }
            if (item["Hours3Amount"].Text == "0.00")
            {
                item["Hours3Amount"].Text = "";
            }
            item["EmpRef"].Text = item["EmpRef"].Text.Trim();
        }
    }

    protected void gvPayRoll_GridExporting(object sender, GridExportingArgs e)
    {
        e.ExportOutput = e.ExportOutput.Replace("\"", "");
    }

    protected void ddlTimesheet_SelectedIndexChanged(object sender, EventArgs e)
    {
        SaveFilter();
        //gvTimesheet.Rebind();
    }

    protected void ddlworker_SelectedIndexChanged(object sender, EventArgs e)
    {
        SaveFilter();
        //gvTimesheet.Rebind();
    }


    protected void ddlSuper_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillWorker();
        SaveFilter();
        //gvTimesheet.Rebind();
    }

    protected void ddlDepartment_SelectedIndexChanged(object sender, EventArgs e)
    {
        SaveFilter();
        //gvTimesheet.Rebind();
    }

    private void ResetGridState()
    {       
        //gvTimesheet.CurrentPageIndex = 0;
        //gvTimesheet.MasterTableView.FilterExpression = String.Empty;
        //gvTimesheet.MasterTableView.PageSize = 25;        
        //gvTimesheet.Rebind();
    }
    private void SaveFilter()
    {

        Session["filterstateEtimesheet"] = ddlDepartment.SelectedValue + ";"
            + ddlSuper.SelectedValue + ";"
            + ddlworker.SelectedValue + ";"
            + ddlTimesheet.SelectedValue + ";"
            + txtFromDate.Text + ";"
            + txtToDate.Text+";";       
    }
    public void UpdateControl()    {
      
        if (Session["filterstateEtimesheet"] != null)
        {
            if (Session["filterstateEtimesheet"].ToString() != string.Empty)
            {
                string[] strFilter = Session["filterstateEtimesheet"].ToString().Split(';');
                ddlDepartment.SelectedValue = strFilter[0];
                ddlSuper.SelectedValue = strFilter[1];
                ddlworker.SelectedValue = strFilter[2];
                ddlTimesheet.SelectedValue = strFilter[3];
                txtFromDate.Text = strFilter[4];

                txtToDate.Text = strFilter[5];
            }
        }
    }
}