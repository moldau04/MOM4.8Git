using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BusinessLayer;
using BusinessEntity;
using Microsoft.Reporting.WebForms;
using System.Web.UI.HtmlControls;
using System.Net.Configuration;
using System.Reflection;

public partial class PrintList : System.Web.UI.Page
{
    BL_MapData objBL_MapData = new BL_MapData();
    MapData objMapData = new MapData();

    User objPropUser = new User();
    BL_User objBL_User = new BL_User();

    General objPropGeneral = new General();
    BL_General objBL_General = new BL_General();

    GeneralFunctions objGen = new GeneralFunctions();
    int count = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userid"] == null)
        {
            Response.Redirect("login.aspx");
        }
        if (!IsPostBack)
        {
            if (Request.QueryString["ddlSearchReportID"] != null)
            {
                ddlReport.SelectedValue = Request.QueryString["ddlSearchReportID"].ToString();
                btnSearch_Click(sender, e);
            }
        }

        Permission();
    }

    private void Permission()
    {
        if (Session["type"].ToString() != "am")
        {
            DataTable dt = new DataTable();
            dt = (DataTable)Session["userinfo"];
            if (dt.Rows[0]["EmployeeMaint"].ToString().Substring(3, 1) == "N")
            {
                ddlReport.Items[2].Enabled = false;
            }
            else
            {
                ddlReport.Items[2].Enabled = true;
            }
        }
    }

    private void GetReportDatByQuery()
    {
        string TicketListQuery = Session["TicketListQuery"].ToString();

        DataSet dsrpt = new BusinessLayer.Schedule.BL_Tickets().GetTicketListReportDatabyQuery(TicketListQuery, Session["config"].ToString());
        DataTable dt = dsrpt.Tables[0];

        objPropUser.ConnConfig = Session["config"].ToString();
        objPropUser.Username = Session["username"].ToString();

        txtFrom.Text = objBL_User.getUserEmail(objPropUser);

        objPropUser.ConnConfig = Session["config"].ToString();
        DataSet dsC = objBL_User.getControl(objPropUser);

        if (dsC.Tables[0].Rows.Count > 0)
        {
            if (txtFrom.Text.Trim() == string.Empty)
            {
                if (Session["MSM"].ToString() != "TS")
                {
                    txtFrom.Text = dsC.Tables[0].Rows[0]["Email"].ToString();
                }
            }
        }

        if (txtFrom.Text.Trim() == string.Empty)
        {
            System.Configuration.Configuration configurationFile = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration(Request.ApplicationPath);
            MailSettingsSectionGroup mailSettings = configurationFile.GetSectionGroup("system.net/mailSettings") as MailSettingsSectionGroup;
            string username = mailSettings.Smtp.Network.UserName;
            txtFrom.Text = username;
        }

        if (ddlReport.SelectedValue == "4")
        {
            Detailreport(dt);
        }
        else
        {
            DataSet dccust = new DataSet();
            objPropUser.ConnConfig = Session["config"].ToString();
            objPropUser.LocID = Convert.ToInt32(Request.QueryString["uid"]);
            dccust = objBL_User.getLocationByIDReport(objPropUser);

            ReportViewer1.LocalReport.DataSources.Clear();
            if (ddlReport.SelectedValue == "6")
            {
                var Tickets = dt.AsEnumerable()
                    .OrderBy(o => o.Field<int>("id"))
                    .GroupBy(g => g.Field<string>("workorder"))
                    .Select(s => new { s, Count = s.Count() })
                    .SelectMany(sm => sm.s.Select(s => s)
                    .Zip(Enumerable.Range(1, sm.Count), (row, index) =>
                    new
                    {
                        RowNo = index,
                        who = row.Field<object>("who"),
                        lid = row.Field<object>("lid"),
                        locid = row.Field<object>("locid"),
                        assigned = row.Field<object>("assigned"),
                        fulladdress = row.Field<object>("fulladdress"),
                        WorkOrder = row.Field<object>("workorder"),
                        Reg = row.Field<object>("Reg"),
                        OT = row.Field<object>("OT"),
                        NT = row.Field<object>("NT"),
                        DT = row.Field<object>("DT"),
                        TT = row.Field<object>("TT"),
                        Total = row.Field<object>("Total"),
                        ClearCheck = row.Field<object>("ClearCheck"),
                        charge = row.Field<object>("charge"),
                        fDesc = row.Field<object>("fDesc"),
                        TimeRoute = row.Field<object>("TimeRoute"),
                        TimeSite = row.Field<object>("TimeSite"),
                        TimeComp = row.Field<object>("TimeComp"),
                        comp = row.Field<object>("comp"),
                        dwork = row.Field<object>("dwork"),
                        lastname = row.Field<object>("lastname"),
                        hourlyrate = row.Field<object>("hourlyrate"),
                        ID = row.Field<object>("ID"),
                        customername = row.Field<object>("customername"),
                        locname = row.Field<object>("locname"),
                        address = row.Field<object>("address"),
                        phone = row.Field<object>("phone"),
                        Cat = row.Field<object>("Cat"),
                        edate = row.Field<object>("edate"),
                        CDate = row.Field<object>("CDate"),
                        descres = row.Field<object>("descres"),
                        assignname = row.Field<object>("assignname"),
                        Est = row.Field<object>("Est"),
                        Tottime = row.Field<object>("Tottime"),
                        timediff = row.Field<object>("timediff"),
                        workorder = row.Field<object>("workorder"),
                        expenses = row.Field<object>("expenses"),
                        zone = row.Field<object>("zone"),
                        toll = row.Field<object>("toll"),
                        othere = row.Field<object>("othere"),
                        extraexp = row.Field<object>("extraexp"),
                        mileagetravel = row.Field<object>("mileagetravel"),
                        mileage = row.Field<object>("mileage"),
                        signatureCount = row.Field<object>("signatureCount"),
                        DocumentCount = row.Field<object>("DocumentCount"),
                        workerid = row.Field<object>("workerid"),
                        description = row.Field<object>("description"),
                        fdescreason = row.Field<object>("fdescreason"),
                        invoice = row.Field<object>("invoice"),
                        Confirmed = row.Field<object>("Confirmed"),
                        manualinvoice = row.Field<object>("manualinvoice"),
                        invoiceno = row.Field<object>("invoiceno"),
                        ownerid = row.Field<object>("ownerid"),
                        QBinvoiceid = row.Field<object>("QBinvoiceid"),
                        TransferTime = row.Field<object>("TransferTime"),
                        serviceitem = row.Field<object>("serviceitem"),
                        PayrollItem = row.Field<object>("PayrollItem"),
                        RTOTTT = row.Field<object>("RTOTTT"),
                        timesign = row.Field<object>("timesign"),
                        dispalert = row.Field<object>("dispalert"),
                        credithold = row.Field<object>("credithold"),
                        high = row.Field<object>("high"),
                        unitid = row.Field<object>("unitid"),
                        unit = row.Field<object>("unit"),
                        defaultworker = row.Field<object>("defaultworker"),
                        defaultmech = row.Field<object>("defaultmech"),
                        department = row.Field<object>("department"),
                        bremarks = row.Field<object>("bremarks"),
                        laborexp = row.Field<object>("laborexp"),
                        signature = row.Field<object>("signature"),
                        state = row.Field<object>("state"),
                        mileagepr = row.Field<object>("mileagepr"),
                        afterhours = row.Field<object>("afterhours"),
                        weekends = row.Field<object>("weekends"),
                        EmailNotified = row.Field<object>("EmailNotified"),
                        EmailTime = row.Field<object>("EmailTime"),
                        PartsUsed = row.Field<object>("PartsUsed")
                    }));

                DataTable dtWo = new DataTable();
                if (Tickets.Any())
                {
                    dtWo = ConvertToDataTable(Tickets);
                }

                ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("Ticket_dtTicket", dtWo));
            }
            else if (ddlReport.SelectedValue == "10")
            {
                if (!ContainColumn("DefaultMechCount", dt))
                {
                    dt.Columns.Add("DefaultMechCount", typeof(int));
                }

                foreach (DataRow row in dt.Rows)
                {
                    var count = dt.AsEnumerable().Where(a => (a.Field<string>("defaultmech") == null ? "" : a.Field<string>("defaultmech").ToLower()) == row["dwork"].ToString().ToLower()).Count();
                    row.SetField("DefaultMechCount", count);
                }

                dt.AcceptChanges();
                ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("Ticket_dtTicket", dt));
            }
            else
            {
                ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("Ticket_dtTicket", dt));
            }

            ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("Ticket_Company", dsC.Tables[0]));
            ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("Ticket_Customer", dccust.Tables[0]));
            string reportPath = string.Empty;
            if (ddlReport.SelectedValue == "0")
            {
                if (!chkGrp.Checked)
                    reportPath = "Reports/TicketList_grpworker.rdlc";
                else
                    reportPath = "Reports/TicketList.rdlc";
            }
            else if (ddlReport.SelectedValue == "1")
            {
                reportPath = "Reports/TicketExpense.rdlc";
            }
            else if (ddlReport.SelectedValue == "2")
            {
                reportPath = "Reports/TicketListTime.rdlc";
            }
            else if (ddlReport.SelectedValue == "13")
            {
                reportPath = "Reports/TicketListTimeByDepartment.rdlc";
            }
            else if (ddlReport.SelectedValue == "14")
            {
                reportPath = "Reports/TicketListTime_NO_TT.rdlc";
            }
            else if (ddlReport.SelectedValue == "3")
            {
                reportPath = "Reports/callbackreport.rdlc";
            }
            else if (ddlReport.SelectedValue == "5")
            {
                reportPath = "Reports/TicketListSign.rdlc";
            }
            else if (ddlReport.SelectedValue == "6")
            {
                reportPath = "Reports/TicketListPES.rdlc";
            }
            else if (ddlReport.SelectedValue == "7")
            {
                reportPath = "Reports/Ticketlistworkerstime.rdlc";
            }
            else if (ddlReport.SelectedValue == "8")
            {
                string Report = System.Web.Configuration.WebConfigurationManager.AppSettings["TicketListReport"].Trim();
                if (!string.IsNullOrEmpty(Report.Trim()))
                {
                    reportPath = "Reports/" + Report.Trim();
                }
                else
                {
                    reportPath = "Reports/TicketListReport.rdlc";
                }
            }
            else if (ddlReport.SelectedValue == "9")
            {
                reportPath = "Reports/installationschedule.rdlc";
            }
            else if (ddlReport.SelectedValue == "11")
            {
                reportPath = "Reports/ServiceSchedule.rdlc";
            }
            else if (ddlReport.SelectedValue == "10")
            {
                reportPath = "Reports/monthlymaintenance.rdlc";
            }

            else if (ddlReport.SelectedValue == "12")
            {
                reportPath = "Reports/LaborReport.rdlc";
            }
            else if (ddlReport.SelectedValue == "13")
            {
                reportPath = "Reports/TicketListwithWageCategory.rdlc";
            }

            ReportViewer1.LocalReport.ReportPath = reportPath;

            string stdate = "";
            if (Request.QueryString["sd"].ToString() != string.Empty)
            {
                stdate = " Date range from : " + Request.QueryString["sd"].ToString();
            }
            if (Request.QueryString["sd"].ToString() != string.Empty && Request.QueryString["ed"].ToString() != string.Empty)
            {
                stdate += " To : " + Request.QueryString["ed"].ToString();
            }
            if (Request.QueryString["sd"].ToString() == string.Empty && Request.QueryString["ed"].ToString() != string.Empty)
            {
                stdate += "Date range To : " + Request.QueryString["ed"].ToString();
            }
            if (Request.QueryString["sd"].ToString() == string.Empty && Request.QueryString["ed"].ToString() == string.Empty)
            {
                stdate += "Date range : All Tickets";
            }


            ReportViewer1.LocalReport.EnableExternalImages = true;
            List<Microsoft.Reporting.WebForms.ReportParameter> param1 = new List<Microsoft.Reporting.WebForms.ReportParameter>();
            param1.Add(new Microsoft.Reporting.WebForms.ReportParameter("From", stdate));
            if (ddlReport.SelectedValue != "9" && ddlReport.SelectedValue != "11")
            {
                param1.Add(new Microsoft.Reporting.WebForms.ReportParameter("Status", Request.QueryString["sn"].ToString()));
                param1.Add(new Microsoft.Reporting.WebForms.ReportParameter("Supervisor", Request.QueryString["Sup"].ToString()));
                param1.Add(new Microsoft.Reporting.WebForms.ReportParameter("Worker", Request.QueryString["Wor"].ToString()));
                param1.Add(new Microsoft.Reporting.WebForms.ReportParameter("Chargeable", Request.QueryString["chr"].ToString()));
                param1.Add(new Microsoft.Reporting.WebForms.ReportParameter("Reviewed", Request.QueryString["rev"].ToString()));
                string strPath = "file:///" + Server.MapPath(Request.ApplicationPath).Replace("\\", "/");
                param1.Add(new Microsoft.Reporting.WebForms.ReportParameter("Path", strPath + "/images/Company_logo.jpg"));
            }
            if (ddlReport.SelectedValue == "10")
            {
                DateTime dateCurrent = Convert.ToDateTime(Request.QueryString["sd"].ToString());

                int month = dateCurrent.Month;

                int year = dateCurrent.Year;

                DateTime SWeek1 = new DateTime(year, month, 1);

                DateTime EWeek1 = SWeek1.AddDays(((int)DayOfWeek.Friday - (int)SWeek1.DayOfWeek + 7) % 7);

                DateTime SWeek2 = EWeek1.AddDays(1);

                DateTime EWeek2 = EWeek1.AddDays(7);

                DateTime SWeek3 = EWeek2.AddDays(1);

                DateTime EWeek3 = EWeek2.AddDays(7);

                DateTime SWeek4 = EWeek3.AddDays(1);

                DateTime EWeek4 = EWeek3.AddDays(7);

                DateTime SWeek5 = EWeek4.AddDays(1);

                DateTime EWeek5 = new DateTime(year, month, DateTime.DaysInMonth(year, month));

                param1.Add(new Microsoft.Reporting.WebForms.ReportParameter("SWeek1", SWeek1.ToShortDateString()));
                param1.Add(new Microsoft.Reporting.WebForms.ReportParameter("SWeek2", SWeek2.ToShortDateString()));
                param1.Add(new Microsoft.Reporting.WebForms.ReportParameter("SWeek3", SWeek3.ToShortDateString()));
                param1.Add(new Microsoft.Reporting.WebForms.ReportParameter("SWeek4", SWeek4.ToShortDateString()));
                param1.Add(new Microsoft.Reporting.WebForms.ReportParameter("SWeek5", SWeek5.ToShortDateString()));

                param1.Add(new Microsoft.Reporting.WebForms.ReportParameter("EWeek1", EWeek1.ToShortDateString()));
                param1.Add(new Microsoft.Reporting.WebForms.ReportParameter("EWeek2", EWeek2.ToShortDateString()));
                param1.Add(new Microsoft.Reporting.WebForms.ReportParameter("EWeek3", EWeek3.ToShortDateString()));
                param1.Add(new Microsoft.Reporting.WebForms.ReportParameter("EWeek4", EWeek4.ToShortDateString()));
                param1.Add(new Microsoft.Reporting.WebForms.ReportParameter("EWeek5", EWeek5.ToShortDateString()));
            }

            ReportViewer1.LocalReport.SetParameters(param1);
            ReportViewer1.LocalReport.Refresh();
        }
    }

    private void Detailreport(DataTable dt)
    {
        count = 0;
        DataSet dsTicket = getSubReportdata(dt);
        Session["ticketdetailsr"] = dsTicket;
        if (dsTicket.Tables[0].Rows.Count > 0)
        {
            DataSet dsC = new DataSet();
            objPropUser.ConnConfig = Session["config"].ToString();
            dsC = objBL_User.getControl(objPropUser);
            Session["controldatarep"] = dsC;
            ReportViewer1.LocalReport.DataSources.Clear();
            ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("Ticket_dtTicket", dsTicket.Tables[0]));
            ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("Ticket_Company", dsC.Tables[0]));
            string reportPath = "Reports/TicketDetails.rdlc";
            string Report = System.Web.Configuration.WebConfigurationManager.AppSettings["TicketDetailsReport"].Trim();
            if (!string.IsNullOrEmpty(Report.Trim()) && Report.Contains(".rdlc"))
            {
                reportPath = "Reports/" + Report.Trim();
            }
            ReportViewer1.LocalReport.ReportPath = reportPath;
            ReportViewer1.LocalReport.EnableExternalImages = true;
            List<Microsoft.Reporting.WebForms.ReportParameter> param1 = new List<Microsoft.Reporting.WebForms.ReportParameter>();
            param1.Add(new Microsoft.Reporting.WebForms.ReportParameter("Path", "~/companylogo.ashx"));

            var paras = ReportViewer1.LocalReport.GetParameters();
            var dsCustom1 = GetCustomFields("Loc1");
            if (dsCustom1.Tables[0].Rows.Count > 0 && !string.IsNullOrEmpty(dsCustom1.Tables[0].Rows[0]["label"].ToString()) && paras.FirstOrDefault(x => x.Name == "Custom1Lable") != null)
            {
                param1.Add(new Microsoft.Reporting.WebForms.ReportParameter("Custom1Lable", dsCustom1.Tables[0].Rows[0]["label"].ToString()));
            }

            var dsCustom2 = GetCustomFields("Loc2");
            if (dsCustom2.Tables[0].Rows.Count > 0 && !string.IsNullOrEmpty(dsCustom2.Tables[0].Rows[0]["label"].ToString()) && paras.FirstOrDefault(x => x.Name == "Custom2Lable") != null)
            {
                param1.Add(new Microsoft.Reporting.WebForms.ReportParameter("Custom2Lable", dsCustom2.Tables[0].Rows[0]["label"].ToString()));
            }

            ReportViewer1.LocalReport.SetParameters(param1);
            ReportViewer1.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(SetSubDataSource);
            ReportViewer1.LocalReport.Refresh();
        }
    }

    private DataSet GetCustomFields(string name)
    {
        DataSet ds = new DataSet();
        objPropGeneral.CustomName = name;
        objPropGeneral.ConnConfig = Session["config"].ToString();
        ds = objBL_General.getCustomFields(objPropGeneral);

        return ds;
    }

    public void SetSubDataSource(object sender, SubreportProcessingEventArgs e)
    {
        DataSet ds = (DataSet)Session["ticketdetailsr"];
        DataSet dsC = (DataSet)Session["controldatarep"];
        int ticketid = Convert.ToInt32(ds.Tables[0].Rows[count]["id"]);
        DataTable dtTicket = getSubReportdata(ds.Tables[0], ticketid);
        DataTable dtPOitem = getSubReportTicketdata(ds.Tables[1], ticketid);
        DataTable dtTicketI = getSubReportTicketdata(ds.Tables[2], ticketid);

        objMapData.ConnConfig = Session["config"].ToString();
        objMapData.TicketID = ticketid;
        DataSet dsEquip = objBL_MapData.getElevByTicket(objMapData);

        e.DataSources.Add(new ReportDataSource("dtEquipDetails", dsEquip.Tables[0]));
        e.DataSources.Add(new ReportDataSource("Ticket_dtTicket", dtTicket));
        e.DataSources.Add(new ReportDataSource("Ticket_Company", dsC.Tables[0]));
        e.DataSources.Add(new ReportDataSource("Ticket_dtMCP", fillREPHistory(ticketid)));
        if (ds.Tables.Count > 1)
            e.DataSources.Add(new ReportDataSource("Ticket_dtPOItem", dtPOitem));
        if (ds.Tables.Count > 2)
            e.DataSources.Add(new ReportDataSource("Ticket_dtTicketI", dtTicketI));

        if (count == ds.Tables[0].Rows.Count - 1)
        {
            Session["ticketdetailsr"] = null;
            Session["controldatarep"] = null;
        }
        count++;
    }

    private DataTable fillREPHistory(int ticketid)
    {
        objPropUser.ConnConfig = Session["config"].ToString();
        objPropUser.EquipID = 0;
        objPropUser.SearchBy = "rd.ticketID";
        objPropUser.SearchValue = ticketid.ToString();

        DataSet ds = new DataSet();
        ds = objBL_User.getequipREPDetails(objPropUser);

        return ds.Tables[0];
    }

    private DataTable getSubReportdata(DataTable dt, int ticketid)
    {
        DataTable dtimport = dt.Clone();
        DataRow dr = dt.Rows[count];
        dtimport.ImportRow(dr);

        return dtimport;
    }

    private DataTable getSubReportTicketdata(DataTable dt, int ticketid)
    {
        IEnumerable<DataRow> query =
         from order in dt.AsEnumerable()
         where order.Field<int>("Ticket") == ticketid
         select order;
        DataTable dtc = dt.Clone();
        if (query.Count() > 0)
            dtc = query.CopyToDataTable<DataRow>();

        return dtc;
    }

    private DataSet getSubReportdata(DataTable dt)
    {
        DataTable dtTicket = new DataTable();
        dtTicket.Columns.Add("TicketID", typeof(int));

        foreach (DataRow dr in dt.Rows)
        {
            DataRow drTicket = dtTicket.NewRow();
            drTicket["TicketID"] = dr["ID"];
            dtTicket.Rows.Add(drTicket);
        }

        objMapData.ConnConfig = Session["config"].ToString();
        objMapData.dtTickets = dtTicket;
        DataSet ds = objBL_MapData.getTicketdetailsReport(objMapData);
        ds.Tables[0].Columns.Add("rtaddress");
        ds.Tables[0].Columns.Add("osaddress");
        ds.Tables[0].Columns.Add("ctaddress");

        return ds;
    }

    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        txtTo.Text = string.Empty;
        txtCC.Text = string.Empty;

        ScriptManager.RegisterStartupScript(this, GetType(), "showMailReport", "$('#setuppopup').modal();", true);
    }

    private byte[] ExportReportToPDF(string reportName)
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

    protected void hideModalPopupViaServerConfirm_Click(object sender, EventArgs e)
    {
        if (txtTo.Text.Trim() != string.Empty)
        {
            try
            {
                Mail mail = new Mail();
                mail.From = txtFrom.Text.Trim();
                mail.To = txtTo.Text.Split(';', ',').OfType<string>().ToList();
                if (txtCC.Text.Trim() != string.Empty)
                {
                    mail.Cc = txtCC.Text.Split(';', ',').OfType<string>().ToList();
                }
                mail.Title = "Ticket Report";
                if (txtBody.Text.Trim() != string.Empty)
                {
                    mail.Text = txtBody.Text.Replace(Environment.NewLine, "<BR/>");
                }
                else
                {
                    mail.Text = "This is report email sent from Mobile Office Manager. Please find the Ticket Report attached.";
                }
                //mail.AttachmentFiles.Add(ExportReportToPDF("Report_" + objGen.generateRandomString(10) + ".pdf"));
                mail.attachmentBytes = ExportReportToPDF("");
                mail.FileName = "TicketList.pdf";

                mail.DeleteFilesAfterSend = true;
                mail.RequireAutentication = false;

                mail.Send();
                //this.programmaticModalPopup.Hide();
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "noty({text: 'Mail sent successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
            }
            catch (Exception ex)
            {
                string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            }
        }
    }

    protected void chkGrp_CheckedChanged(object sender, EventArgs e)
    {
        ReportViewer1.Reset();
        GetReportDatByQuery();
    }

    protected void ReportViewer1_ReportRefresh(object sender, EventArgs e)
    {
        GetReportDatByQuery();
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if (ddlReport.SelectedValue == "0")
        {
            chkGrp.Visible = true;
        }
        else
        {
            chkGrp.Visible = false;
        }

        ReportViewer1.Reset();

        if (ddlReport.SelectedValue != "")
        {
            GetReportDatByQuery();
        }
    }

    private bool ContainColumn(string columnName, DataTable table)
    {
        bool contain = false;
        DataColumnCollection columns = table.Columns;
        if (columns.Contains(columnName))
        {
            contain = true;
        }

        return contain;
    }

    public DataTable ConvertToDataTable<T>(IEnumerable<T> varlist)
    {
        DataTable dtReturn = new DataTable();
        // column names 
        PropertyInfo[] oProps = null;
        if (varlist == null) return dtReturn;

        foreach (T rec in varlist)
        {
            // Use reflection to get property names, to create table, Only first time, others will follow 
            if (oProps == null)
            {
                oProps = ((Type)rec.GetType()).GetProperties();
                foreach (PropertyInfo pi in oProps)
                {
                    Type colType = pi.PropertyType;

                    if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                    {
                        colType = colType.GetGenericArguments()[0];
                    }

                    dtReturn.Columns.Add(new DataColumn(pi.Name, colType));
                }
            }

            DataRow dr = dtReturn.NewRow();

            foreach (PropertyInfo pi in oProps)
            {
                dr[pi.Name] = pi.GetValue(rec, null) == null ? DBNull.Value : pi.GetValue
                (rec, null);
            }

            dtReturn.Rows.Add(dr);
        }

        return dtReturn;
    }
}