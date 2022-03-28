using System;
using System.Collections.Generic;
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

public partial class Etimesheet_v2 : System.Web.UI.Page
{
    GeneralFunctions objGeneral = new GeneralFunctions();
    User objPropUser = new User();
    BL_User objBL_User = new BL_User();
    DateTime fromDate;
    DateTime toDate;
    protected void Page_Load(object sender, EventArgs e)
    {
        // Response.Redirect("home.aspx");

        if (Session["userid"] == null)
        {
            Response.Redirect("login.aspx");
        }

        if (!IsPostBack)
        {
            if (Session["fromDate"] == null && Session["ToDate"] == null)
            {
                txtfromDate.Text = DateTime.Today.AddDays(-1 * (int)(DateTime.Today.DayOfWeek)).ToShortDateString();
                txtToDate.Text = DateTime.Now.AddDays(DayOfWeek.Friday - (DateTime.Now.DayOfWeek)).Date.ToShortDateString();
            }
            else
            {
                txtfromDate.Text = Session["fromDate"].ToString();
                txtToDate.Text = Session["ToDate"].ToString();
            }

            fillDates();
            userpermissions();
            ViewState["saved"] = 0;
            ViewState["update"] = 0;
            var Now = System.DateTime.Now;
            var lastSunday = Now.DayOfWeek == DayOfWeek.Sunday ? Now.AddDays(0) : Now.AddDays(-(int)Now.DayOfWeek);
            var thisSaturday = lastSunday.AddDays(6);

            txtfromDate.Text = lastSunday.ToShortDateString();
            txtToDate.Text = thisSaturday.ToShortDateString();
            FillDepartment();
            FillSupervisor();
            GetTimesheetEmp();
        }
        CompanyPermission();
        //tbldrop.Visible = false;
        lnkSave.Visible = false;
        //get the radio button clicked
        string eventTarget = this.Request.Params.Get("__EVENTTARGET");
        switch (eventTarget)
        {
            case "ctl00_ContentPlaceHolder1_rdDay":
                AddClass();
                rdDay_CheckedChanged();
                break;
            case "ctl00_ContentPlaceHolder1_rdWeek":
                AddClass();
                rdWeek_CheckedChanged();
                break;
            case "ctl00_ContentPlaceHolder1_rdMonth":
                AddClass();
                rdMonth_CheckedChanged();
                break;
            case "ctl00_ContentPlaceHolder1_rdQuarter":
                AddClass();
                rdQuarter_CheckedChanged();
                break;
            case "ctl00_ContentPlaceHolder1_rdYear":
                AddClass();
                rdYear_CheckedChanged();
                break;

        }
    }
    private void fillDates()
    {
        DateTime firstDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        int DaysinMonth = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month) - 1;
        DateTime lastDay = firstDay.AddDays(DaysinMonth);
        txtfromDate.Text = firstDay.ToShortDateString();
        txtToDate.Text = lastDay.ToShortDateString();
    }
    private void Permission()
    {
        HtmlGenericControl li = (HtmlGenericControl)Page.Master.FindControl("schMgr");
        li.Attributes.Add("class", "start active open");

        HyperLink a = (HyperLink)Page.Master.FindControl("lnkSchd");

        HyperLink lnkUsersSmenu = (HyperLink)Page.Master.FindControl("lnkTimesheet");
        lnkUsersSmenu.Style.Add("color", "#316b9d");
        lnkUsersSmenu.Style.Add("font-weight", "normal");
        lnkUsersSmenu.Style.Add("background-color", "#e5e5e5");

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
    private void userpermissions()
    {
        BL_General objBL_General = new BL_General();
        General objGeneral = new General();
        objGeneral.ConnConfig = Session["config"].ToString();
        DataSet dsLastSync = objBL_General.getSagelatsync(objGeneral);
        int intintegration = Convert.ToInt32(dsLastSync.Tables[0].Rows[0]["sageintegration"]);
        if (intintegration == 1)
        {
            lnkExport.Visible = true;
            ddlExport.Visible = true;
        }
        else
        {
            lnkExport.Visible = false;
            ddlExport.Visible = false;
        }

        if (Session["type"].ToString() != "c")
        {
            if (Session["type"].ToString() != "am")
            {
                objPropUser.ConnConfig = Session["config"].ToString();
                objPropUser.Username = Session["username"].ToString();
                objPropUser.PageName = "etimesheet.aspx";
                DataSet dspage = objBL_User.getScreensByUser(objPropUser);
                if (dspage.Tables[0].Rows.Count > 0)
                {
                    if (Convert.ToBoolean(dspage.Tables[0].Rows[0]["access"].ToString()) == false)
                    {
                        Response.Redirect("home.aspx");
                    }
                }
                else
                {
                    Response.Redirect("home.aspx");
                }
            }
        }
    }
    private void CompanyPermission()
    {
        if (Session["COPer"].ToString() == "1")
        {
            gvTimesheet.Columns[3].Visible = true;
        }
        else
        {
            gvTimesheet.Columns[3].Visible = false;
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

        ddlDepartment.Items.Insert(0, new ListItem("All", "0"));
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

    private void GetTimesheetEmp()
    {
        objPropUser.ConnConfig = Session["config"].ToString();
        objPropUser.Startdt = Convert.ToDateTime(txtfromDate.Text.Trim());
        objPropUser.Enddt = Convert.ToDateTime(txtToDate.Text.Trim());
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
    }
    private DataSet GetTimesheetData()
    {
        objPropUser.ConnConfig = Session["config"].ToString();
        DataSet ds = new DataSet();
        objPropUser.Startdt = Convert.ToDateTime(txtfromDate.Text.Trim());
        objPropUser.Enddt = Convert.ToDateTime(txtToDate.Text.Trim());
        objPropUser.DepartmentID = Convert.ToInt32(ddlDepartment.SelectedValue);
        objPropUser.Supervisor = ddlSuper.SelectedValue;
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
        ds = objBL_User.getTimesheetEmp(objPropUser,-1);

        return ds;
    }

    //private void GetTimesheetEmpTicket(int unsaved)
    private void GetTimesheetEmpTicket()
    {
        //lnkSaved.Visible = false;
        //lnkBack.Visible = false;
        gvTimesheet.Enabled = true;
        lblProcessed.Visible = false;
        lblSaved.Visible = false;
        //lnkMerge.Visible = false;

        DataSet ds = GetTimesheetData();
        gvTimesheet.DataSource = ds.Tables[0];
        gvTimesheet.DataBind();
        //CalculateTotals();

        lblRecordCount.Text = ds.Tables[0].Rows.Count.ToString() + " Record(s) Found.";
    }

    private DataTable GetSavedTimedata()
    {
        objPropUser.ConnConfig = Session["config"].ToString();
        DataSet ds = new DataSet();
        objPropUser.Startdt = Convert.ToDateTime(txtfromDate.Text.Trim());
        objPropUser.Enddt = Convert.ToDateTime(txtToDate.Text.Trim());
        objPropUser.DepartmentID = Convert.ToInt32(ddlDepartment.SelectedValue);
        objPropUser.Supervisor = ddlSuper.SelectedValue;
        ds = objBL_User.getSavedTimesheetEmp(objPropUser);

        return ds.Tables[0];
    }

    private void GetSavedTimesheetEmp()
    {
        objPropUser.ConnConfig = Session["config"].ToString();
        DataTable dt = GetSavedTimedata();

        lblRecordCount.Text = dt.Rows.Count.ToString() + " Record(s) Found.";

        if (dt.Rows.Count > 0)
        {
            if (dt.Rows[0]["processed"].ToString() == "1")
            {
                gvTimesheet.Enabled = false;
                lnkProcess.Visible = false;
                lblProcessed.Visible = true;
                //lnkSaved.Visible = false;
                //lnkMerge.Visible = false;
            }
            else
            {
                gvTimesheet.Enabled = true;
                lnkSave.Visible = true;
                lblProcessed.Visible = false;
                lblSaved.Visible = true;
            }
            //lnkBack.Visible = false;
        }

        gvTimesheet.DataSource = dt;
        gvTimesheet.DataBind();
    }

    private DataTable GetTimesheetTicketsByEmp(int EmpID)
    {
        objPropUser.ConnConfig = Session["config"].ToString();
        DataSet ds = new DataSet();
        objPropUser.Startdt = Convert.ToDateTime(txtfromDate.Text.Trim());
        objPropUser.Enddt = Convert.ToDateTime(txtToDate.Text.Trim());
        objPropUser.EmpId = EmpID;
        objPropUser.saved = (int)ViewState["saved"];
        objPropUser.unsaved = (int)ViewState["update"];
        ds = objBL_User.GetTimesheetTicketsByEmp(objPropUser,-1);

        return ds.Tables[0];
    }

    private DataTable GetTicketsData()
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

        foreach (GridViewRow gr in gvTimesheet.MasterTableView.Items)
        {
            GridView gvTickets = (GridView)gr.FindControl("gvTickets");
            Label lblID = (Label)gr.FindControl("lblID");
            //int extra = 0;
            foreach (GridViewRow grTic in gvTickets.Rows)
            {
                //if (extra != 0)
                //{
                HyperLink lnkTick = (HyperLink)grTic.FindControl("lnkTick");
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
                dr["TicketID"] = Convert.ToInt32(objGeneral.IsNull(lnkTick.Text, "0"));
                dr["Reg"] = Convert.ToDouble(objGeneral.IsNull(txtReg.Text.Trim(), "0"));
                dr["OT"] = Convert.ToDouble(objGeneral.IsNull(txtOT.Text.Trim(), "0"));
                dr["DT"] = Convert.ToDouble(objGeneral.IsNull(txtDT.Text.Trim(), "0"));
                dr["TT"] = Convert.ToDouble(objGeneral.IsNull(txtTT.Text.Trim(), "0"));
                dr["NT"] = Convert.ToDouble(objGeneral.IsNull(txtNT.Text.Trim(), "0"));
                dr["Zone"] = Convert.ToDouble(objGeneral.IsNull(txtZone.Text.Trim(), "0"));
                dr["Mileage"] = Convert.ToDouble(objGeneral.IsNull(txtMileage.Text.Trim(), "0"));
                dr["HourlyRate"] = Convert.ToDouble(objGeneral.IsNull(txtRate.Text.Trim(), "0"));
                dr["Extra"] = Convert.ToDouble(objGeneral.IsNull(txtExtra.Text.Trim(), "0"));
                dr["Misc"] = Convert.ToDouble(objGeneral.IsNull(txtTMisc.Text.Trim(), "0"));
                dr["Toll"] = Convert.ToDouble(objGeneral.IsNull(txtPToll.Text.Trim(), "0"));
                dr["Empid"] = Convert.ToInt32(objGeneral.IsNull(lblID.Text.Trim(), "0"));
                dr["Custom"] = Convert.ToDouble(objGeneral.IsNull(txtCustom.Text.Trim(), "0"));
                dr["CustomTick3"] = Convert.ToInt32(chkTJobRate.Checked);
                dr["CustomTick1"] = Convert.ToDouble(objGeneral.IsNull(txtTJobRate.Text.Trim(), "0"));
                dr["CustomTick2"] = Convert.ToDouble(objGeneral.IsNull(txtTCustomTick2.Text.Trim(), "0"));
                dt.Rows.Add(dr);
                //}
                //extra++;
            }
        }
        return dt;
    }

    private void Submit(int processed)
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("TimesheetID", typeof(int));
        dt.Columns.Add("EmpID", typeof(int));
        dt.Columns.Add("Pay", typeof(int));
        dt.Columns.Add("PayMethod", typeof(int));
        dt.Columns.Add("Reg", typeof(double));
        dt.Columns.Add("OT", typeof(double));
        dt.Columns.Add("DT", typeof(double));
        dt.Columns.Add("TT", typeof(double));
        dt.Columns.Add("NT", typeof(double));
        dt.Columns.Add("Holiday", typeof(double));
        dt.Columns.Add("Vacation", typeof(double));
        dt.Columns.Add("SickTime", typeof(double));
        dt.Columns.Add("Zone", typeof(double));
        dt.Columns.Add("Reimb", typeof(double));
        dt.Columns.Add("Mileage", typeof(double));
        dt.Columns.Add("Bonus", typeof(double));
        dt.Columns.Add("Extra", typeof(double));
        dt.Columns.Add("Misc", typeof(double));
        dt.Columns.Add("Toll", typeof(double));
        dt.Columns.Add("Total", typeof(double));

        dt.Columns.Add("FixedHours", typeof(double));
        dt.Columns.Add("Salary", typeof(double));
        dt.Columns.Add("MileRate", typeof(double));
        dt.Columns.Add("HourRate", typeof(double));
        dt.Columns.Add("DollarAmount", typeof(double));
        dt.Columns.Add("Custom", typeof(double));

        foreach (GridViewRow gr in gvTimesheet.MasterTableView.Items)
        {
            Label lblID = (Label)gr.FindControl("lblID");
            CheckBox chkPay = (CheckBox)gr.FindControl("chkPay");
            Label lblMethod = (Label)gr.FindControl("lblMID");
            TextBox txtReg = (TextBox)gr.FindControl("txtReg");
            TextBox txtOT = (TextBox)gr.FindControl("txtOT");
            TextBox txtDT = (TextBox)gr.FindControl("txtDT");
            TextBox txtoneseven = (TextBox)gr.FindControl("txtoneseven");
            TextBox txtTT = (TextBox)gr.FindControl("txtPTravel");
            TextBox txtHoliday = (TextBox)gr.FindControl("txtHoliday");
            TextBox txtVacation = (TextBox)gr.FindControl("txtVacation");
            TextBox txtSick = (TextBox)gr.FindControl("txtSick");
            TextBox txtZone = (TextBox)gr.FindControl("txtZone");
            TextBox txtReimb = (TextBox)gr.FindControl("txtReimb");
            TextBox txtMileage = (TextBox)gr.FindControl("txtMileage");
            TextBox txtBonus = (TextBox)gr.FindControl("txtBonus");
            TextBox txtExtra = (TextBox)gr.FindControl("txtExtra");
            TextBox txtTotal = (TextBox)gr.FindControl("txtTotal");
            TextBox txtMisc = (TextBox)gr.FindControl("txtMisc");
            TextBox txtToll = (TextBox)gr.FindControl("txtToll");
            TextBox txtAmount = (TextBox)gr.FindControl("txtAmount");
            TextBox txtCustom = (TextBox)gr.FindControl("txtCustom");

            //TextBox txtFixedH = (TextBox)gr.FindControl("txtFixedH");
            TextBox txtSalary = (TextBox)gr.FindControl("txtSalary");
            Label lblHourlyRate = (Label)gr.FindControl("lblHourlyRate");
            Label lblMlRate = (Label)gr.FindControl("lblMlRate");

            DataRow dr = dt.NewRow();
            dr["EmpID"] = lblID.Text;
            dr["Pay"] = Convert.ToInt16(chkPay.Checked);
            if (lblMethod.Text != string.Empty)
                dr["PayMethod"] = Convert.ToInt16(lblMethod.Text);
            dr["Reg"] = Convert.ToDouble(objGeneral.IsNull(txtReg.Text.Trim(), "0"));
            dr["OT"] = Convert.ToDouble(objGeneral.IsNull(txtOT.Text.Trim(), "0"));
            dr["DT"] = Convert.ToDouble(objGeneral.IsNull(txtDT.Text.Trim(), "0"));
            dr["TT"] = Convert.ToDouble(objGeneral.IsNull(txtTT.Text.Trim(), "0"));
            dr["NT"] = Convert.ToDouble(objGeneral.IsNull(txtoneseven.Text.Trim(), "0"));
            dr["Holiday"] = Convert.ToDouble(objGeneral.IsNull(txtHoliday.Text.Trim(), "0"));
            dr["Vacation"] = Convert.ToDouble(objGeneral.IsNull(txtVacation.Text.Trim(), "0"));
            dr["SickTime"] = Convert.ToDouble(objGeneral.IsNull(txtSick.Text.Trim(), "0"));
            dr["Zone"] = Convert.ToDouble(objGeneral.IsNull(txtZone.Text.Trim(), "0"));
            dr["Reimb"] = Convert.ToDouble(objGeneral.IsNull(txtReimb.Text.Trim(), "0"));
            dr["Mileage"] = Convert.ToDouble(objGeneral.IsNull(txtMileage.Text.Trim(), "0"));
            dr["Bonus"] = Convert.ToDouble(objGeneral.IsNull(txtBonus.Text.Trim(), "0"));
            dr["Extra"] = Convert.ToDouble(objGeneral.IsNull(txtExtra.Text.Trim(), "0"));
            dr["Total"] = Convert.ToDouble(objGeneral.IsNull(txtTotal.Text.Trim(), "0"));
            dr["Misc"] = Convert.ToDouble(objGeneral.IsNull(txtMisc.Text.Trim(), "0"));
            dr["Toll"] = Convert.ToDouble(objGeneral.IsNull(txtToll.Text.Trim(), "0"));

            dr["Salary"] = Convert.ToDouble(objGeneral.IsNull(txtSalary.Text.Trim(), "0"));
            dr["MileRate"] = Convert.ToDouble(objGeneral.IsNull(lblMlRate.Text.Trim(), "0"));
            dr["HourRate"] = Convert.ToDouble(objGeneral.IsNull(lblHourlyRate.Text.Trim(), "0"));
            dr["DollarAmount"] = Convert.ToDouble(objGeneral.IsNull(txtAmount.Text.Trim(), "0"));
            dr["Custom"] = Convert.ToDouble(objGeneral.IsNull(txtCustom.Text.Trim(), "0"));

            dt.Rows.Add(dr);
        }

        objPropUser.ConnConfig = Session["config"].ToString();
        objPropUser.Startdt = Convert.ToDateTime(txtfromDate.Text.Trim());
        objPropUser.Enddt = Convert.ToDateTime(txtToDate.Text.Trim());
        objPropUser.EmpData = dt;
        objPropUser.dtTicketData = GetTicketsData();
        objPropUser.IsSuper = processed;
        objBL_User.AddTimesheet(objPropUser);
    }

    protected void lnkProcess_Click(object sender, EventArgs e)
    {
        Submit(1);

        GetTimesheetEmp();
    }
    protected void lnkClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("Ticketlistview.aspx");
    }
    protected void lnkSearch_Click(object sender, EventArgs e)
    {
        GetTimesheetEmp();
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

    protected void gvTimesheet_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            foreach (GridDataItem item in gvTimesheet.MasterTableView.Items)
            {
                HtmlAnchor lnkExpand = (HtmlAnchor)item.FindControl("lnkExpand");
                Label lblMID = item.FindControl("lblMID") as Label;
                if (lblMID.Text == "0")
                {
                    Label lblDlrH = (Label)item.FindControl("lblDlrH");
                    Label lblDlrV = (Label)item.FindControl("lblDlrV");
                    Label lblDlrS = (Label)item.FindControl("lblDlrS");
                    lblDlrH.Visible = true;
                    lblDlrV.Visible = true;
                    lblDlrS.Visible = true;
                }
                lnkExpand.Visible = false;


                if (lblProcessed.Visible == false)
                {

                    string EmpID = item.GetDataKeyValue("ID").ToString();
                    double Reg1 = Convert.ToDouble(objGeneral.IsNull(item.GetDataKeyValue("Reg1").ToString(), "0"));
                    double OT1 = Convert.ToDouble(objGeneral.IsNull(item.GetDataKeyValue("OT1").ToString(), "0"));
                    double DT1 = Convert.ToDouble(objGeneral.IsNull(item.GetDataKeyValue("DT1").ToString(), "0"));
                    double TT1 = Convert.ToDouble(objGeneral.IsNull(item.GetDataKeyValue("TT1").ToString(), "0"));
                    double NT1 = Convert.ToDouble(objGeneral.IsNull(item.GetDataKeyValue("NT1").ToString(), "0"));
                    double Zone1 = Convert.ToDouble(objGeneral.IsNull(item.GetDataKeyValue("Zone1").ToString(), "0"));
                    double Mileage1 = Convert.ToDouble(objGeneral.IsNull(item.GetDataKeyValue("Mileage1").ToString(), "0"));
                    double Extra1 = Convert.ToDouble(objGeneral.IsNull(item.GetDataKeyValue("Extra1").ToString(), "0"));
                    double Misc1 = Convert.ToDouble(objGeneral.IsNull(item.GetDataKeyValue("Misc1").ToString(), "0"));
                    double Toll1 = Convert.ToDouble(objGeneral.IsNull(item.GetDataKeyValue("Toll1").ToString(), "0"));
                    double HourRate1 = Convert.ToDouble(objGeneral.IsNull(item.GetDataKeyValue("HourRate1").ToString(), "0"));

                    GridView gvTickets = (GridView)e.Item.FindControl("gvTickets");
                    DataTable dt = GetTimesheetTicketsByEmp(Convert.ToInt32(EmpID));

                    if (dt.Rows.Count > 0)
                    {
                        DataRow drrow = dt.NewRow();
                        drrow["Reg"] = Reg1;
                        drrow["OT"] = OT1;
                        drrow["DT"] = DT1;
                        drrow["TT"] = TT1;
                        drrow["NT"] = NT1;
                        drrow["Zone"] = Zone1;
                        drrow["Mileage"] = Mileage1;
                        drrow["OtherE"] = Misc1;
                        drrow["Toll"] = Toll1;
                        drrow["Extra"] = Extra1;
                        drrow["HourlyRate"] = HourRate1;
                        dt.Rows.InsertAt(drrow, 0);

                        gvTickets.DataSource = dt;
                        gvTickets.DataBind();
                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "calc" + gvTickets.ClientID, "calculate('" + gvTickets.ClientID + "');", true);

                        TextBox txtReg = (TextBox)e.Item.FindControl("txtReg");
                        txtReg.Attributes.Add("onfocus", "this.blur();");
                        txtReg.Attributes.Add("class", "texttransparent");

                        TextBox txtOT = (TextBox)e.Item.FindControl("txtOT");
                        txtOT.Attributes.Add("onfocus", "this.blur();");
                        txtOT.Attributes.Add("class", "texttransparent");

                        TextBox txtoneseven = (TextBox)e.Item.FindControl("txtoneseven");
                        txtoneseven.Attributes.Add("onfocus", "this.blur();");
                        txtoneseven.Attributes.Add("class", "texttransparent");

                        TextBox txtDT = (TextBox)e.Item.FindControl("txtDT");
                        txtDT.Attributes.Add("onfocus", "this.blur();");
                        txtDT.Attributes.Add("class", "texttransparent");

                        TextBox txtPTravel = (TextBox)e.Item.FindControl("txtPTravel");
                        txtPTravel.Attributes.Add("onfocus", "this.blur();");
                        txtPTravel.Attributes.Add("class", "texttransparent");

                        TextBox txtZone = (TextBox)e.Item.FindControl("txtZone");
                        txtZone.Attributes.Add("onfocus", "this.blur();");
                        txtZone.Attributes.Add("class", "texttransparent");

                        TextBox txtMileage = (TextBox)e.Item.FindControl("txtMileage");
                        txtMileage.Attributes.Add("onfocus", "this.blur();");
                        txtMileage.Attributes.Add("class", "texttransparent");

                        TextBox txtMisc = (TextBox)e.Item.FindControl("txtMisc");
                        txtMisc.Attributes.Add("onfocus", "this.blur();");
                        txtMisc.Attributes.Add("class", "texttransparent");

                        TextBox txtToll = (TextBox)e.Item.FindControl("txtToll");
                        txtToll.Attributes.Add("onfocus", "this.blur();");
                        txtToll.Attributes.Add("class", "texttransparent");

                        TextBox txtExtra = (TextBox)e.Item.FindControl("txtExtra");
                        txtExtra.Attributes.Add("onfocus", "this.blur();");
                        txtExtra.Attributes.Add("class", "texttransparent");

                        lnkExpand.Visible = true;

                    }
                }
            }
            }
    }
    protected void lnkSave_Click(object sender, EventArgs e)
    {
        Submit(0);
        GetTimesheetEmp();
    }
    protected void ddlSuper_SelectedIndexChanged(object sender, EventArgs e)
    {
        GetTimesheetEmp();
    }
    protected void ddlDepartment_SelectedIndexChanged(object sender, EventArgs e)
    {
        GetTimesheetEmp();
    }
    protected void lnkExport_Click(object sender, EventArgs e)
    {
        objPropUser.ConnConfig = Session["config"].ToString();
        objPropUser.Startdt = Convert.ToDateTime(txtfromDate.Text.Trim());
        objPropUser.Enddt = Convert.ToDateTime(txtToDate.Text.Trim());
        DataSet dsExport = objBL_User.getGetSageExportTickets(objPropUser);

        if (ddlExport.SelectedItem.Text == "CSV")
            ExportToCSV(dsExport.Tables[0]);
        else if (ddlExport.SelectedItem.Text == "Excel")
            ExportToExcel(dsExport.Tables[0]);
        else if (ddlExport.SelectedItem.Text == "Text")
            ExportToText(dsExport.Tables[0]);
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

    protected void gvTickets_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "UpdateTicket")
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
            if (e.CommandArgument.ToString() != "")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow grTic = (GridViewRow)(((Control)e.CommandSource).NamingContainer);
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
        }
    }
    protected void incDate_Click(object sender, EventArgs e)
    {
        if (rdDay.Checked)
        {
            txtfromDate.Text = Convert.ToDateTime(txtfromDate.Text).AddDays(1).ToShortDateString();
            txtToDate.Text = Convert.ToDateTime(txtToDate.Text).AddDays(1).ToShortDateString();
            Session["ToDate"] = txtToDate.Text;
            Session["fromDate"] = txtfromDate.Text;

        }

        if (rdWeek.Checked)
        {

            txtfromDate.Text = Convert.ToDateTime(txtfromDate.Text).AddDays(7).ToShortDateString();
            txtToDate.Text = Convert.ToDateTime(txtToDate.Text).AddDays(7).ToShortDateString();
            Session["ToDate"] = txtToDate.Text;
            Session["fromDate"] = txtfromDate.Text;
        }
        if (rdMonth.Checked)
        {

            txtfromDate.Text = Convert.ToDateTime(txtfromDate.Text).AddMonths(1).ToShortDateString();

            txtToDate.Text = Convert.ToDateTime(txtToDate.Text).AddMonths(1).ToShortDateString();
            Session["ToDate"] = txtToDate.Text;
            Session["fromDate"] = txtfromDate.Text;
        }
        if (rdQuarter.Checked)
        {
            txtfromDate.Text = Convert.ToDateTime(txtfromDate.Text).AddMonths(3).ToShortDateString();
            txtToDate.Text = Convert.ToDateTime(txtToDate.Text).AddMonths(3).ToShortDateString();
            Session["ToDate"] = txtToDate.Text;
            Session["fromDate"] = txtfromDate.Text;
        }
        if (rdYear.Checked)
        {
            txtfromDate.Text = Convert.ToDateTime(txtfromDate.Text).AddYears(1).ToShortDateString();
            txtToDate.Text = Convert.ToDateTime(txtToDate.Text).AddYears(1).ToShortDateString();
            Session["ToDate"] = txtToDate.Text;
            Session["fromDate"] = txtfromDate.Text;
        }

    }
    protected void decDate_Click(object sender, EventArgs e)
    {
        if (rdDay.Checked)
        {
            txtfromDate.Text = Convert.ToDateTime(txtfromDate.Text).AddDays(-1).ToShortDateString();
            txtToDate.Text = Convert.ToDateTime(txtToDate.Text).AddDays(-1).ToShortDateString();
            Session["ToDate"] = txtToDate.Text;
            Session["fromDate"] = txtfromDate.Text;

        }

        if (rdWeek.Checked)
        {

            txtfromDate.Text = Convert.ToDateTime(txtfromDate.Text).AddDays(-7).ToShortDateString();
            txtToDate.Text = Convert.ToDateTime(txtToDate.Text).AddDays(7).ToShortDateString();
            Session["ToDate"] = txtToDate.Text;
            Session["fromDate"] = txtfromDate.Text;

        }
        if (rdMonth.Checked)
        {

            txtfromDate.Text = Convert.ToDateTime(txtfromDate.Text).AddMonths(-1).ToShortDateString();

            txtToDate.Text = Convert.ToDateTime(txtToDate.Text).AddMonths(-1).ToShortDateString();
            Session["ToDate"] = txtToDate.Text;
            Session["fromDate"] = txtfromDate.Text;
        }
        if (rdQuarter.Checked)
        {
            txtfromDate.Text = Convert.ToDateTime(txtfromDate.Text).AddMonths(-3).ToShortDateString();
            txtToDate.Text = Convert.ToDateTime(txtToDate.Text).AddMonths(-3).ToShortDateString();
            Session["ToDate"] = txtToDate.Text;
            Session["fromDate"] = txtfromDate.Text;
        }
        if (rdYear.Checked)
        {
            txtfromDate.Text = Convert.ToDateTime(txtfromDate.Text).AddYears(-1).ToShortDateString();
            txtToDate.Text = Convert.ToDateTime(txtToDate.Text).AddYears(-1).ToShortDateString();
            Session["ToDate"] = txtToDate.Text;
            Session["fromDate"] = txtfromDate.Text;
        }

    }
    protected void rdDay_CheckedChanged()
    {
        txtfromDate.Text = DateTime.Now.ToShortDateString();
        txtToDate.Text = DateTime.Now.ToShortDateString();
        Session["ToDate"] = txtToDate.Text;
        Session["fromDate"] = txtfromDate.Text;

    }
    protected void rdWeek_CheckedChanged()
    {
        var now = System.DateTime.Now;

        var FisrtDay = now.AddDays(-((now.DayOfWeek - System.Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat.FirstDayOfWeek + 7) % 7)).Date;
        txtfromDate.Text = FisrtDay.ToShortDateString();
        var LastDay = FisrtDay.AddDays(7).AddSeconds(-1);

        txtToDate.Text = LastDay.ToShortDateString();
        Session["ToDate"] = txtToDate.Text;
        Session["fromDate"] = txtfromDate.Text;

    }

    protected void rdMonth_CheckedChanged()
    {
        var Date = System.DateTime.Now;
        var firstDayOfMonth = new DateTime(Date.Year, Date.Month, 1);
        var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
        txtfromDate.Text = firstDayOfMonth.ToShortDateString();
        txtToDate.Text = lastDayOfMonth.ToShortDateString();
        Session["ToDate"] = txtToDate.Text;
        Session["fromDate"] = txtfromDate.Text;


    }
    protected void rdQuarter_CheckedChanged()
    {
        var date = System.DateTime.Now;
        int quarterNumber = (date.Month - 1) / 3 + 1;
        DateTime firstDayOfQuarter = new DateTime(date.Year, (quarterNumber - 1) * 3 + 1, 1);
        DateTime lastDayOfQuarter = firstDayOfQuarter.AddMonths(3).AddDays(-1);
        txtfromDate.Text = firstDayOfQuarter.ToShortDateString();
        txtToDate.Text = lastDayOfQuarter.ToShortDateString();
        Session["ToDate"] = txtToDate.Text;
        Session["fromDate"] = txtfromDate.Text;

    }
    protected void rdYear_CheckedChanged()
    {
        int year = DateTime.Now.Year;
        DateTime firstDay = new DateTime(year, 1, 1);
        DateTime lastDay = new DateTime(year, 12, 31);
        txtfromDate.Text = firstDay.ToShortDateString();
        txtToDate.Text = lastDay.ToShortDateString();
        Session["ToDate"] = txtToDate.Text;
        Session["fromDate"] = txtfromDate.Text;

    }
    private void AddClass()
    {

        if (rdDay.Checked)
        {
            lblDay.Attributes.Add("class", "btn btn-primary btn-sm active ");
            lblWeek.Attributes.Add("class", "btn btn-primary btn-sm ");
            lblMonth.Attributes.Add("class", "btn btn-primary btn-sm ");
            lblQuarter.Attributes.Add("class", "btn btn-primary btn-sm ");
            lblYear.Attributes.Add("class", "btn btn-primary btn-sm ");

        }

        if (rdWeek.Checked)
        {
            lblDay.Attributes.Add("class", "btn btn-primary btn-sm ");
            lblWeek.Attributes.Add("class", "btn btn-primary btn-sm  active");
            lblMonth.Attributes.Add("class", "btn btn-primary btn-sm ");
            lblQuarter.Attributes.Add("class", "btn btn-primary btn-sm ");
            lblYear.Attributes.Add("class", "btn btn-primary btn-sm ");

        }
        if (rdMonth.Checked)
        {
            lblDay.Attributes.Add("class", "btn btn-primary btn-sm ");
            lblWeek.Attributes.Add("class", "btn btn-primary btn-sm ");
            lblMonth.Attributes.Add("class", "btn btn-primary btn-sm active");
            lblQuarter.Attributes.Add("class", "btn btn-primary btn-sm ");
            lblYear.Attributes.Add("class", "btn btn-primary btn-sm ");
        }
        if (rdQuarter.Checked)
        {
            lblDay.Attributes.Add("class", "btn btn-primary btn-sm ");
            lblWeek.Attributes.Add("class", "btn btn-primary btn-sm ");
            lblMonth.Attributes.Add("class", "btn btn-primary btn-sm ");
            lblQuarter.Attributes.Add("class", "btn btn-primary btn-sm active");
            lblYear.Attributes.Add("class", "btn btn-primary btn-sm ");

        }
        if (rdYear.Checked)
        {
            lblDay.Attributes.Add("class", "btn btn-primary btn-sm ");
            lblWeek.Attributes.Add("class", "btn btn-primary btn-sm ");
            lblMonth.Attributes.Add("class", "btn btn-primary btn-sm ");
            lblQuarter.Attributes.Add("class", "btn btn-primary btn-sm ");
            lblYear.Attributes.Add("class", "btn btn-primary btn-sm active ");
        }
    }
}