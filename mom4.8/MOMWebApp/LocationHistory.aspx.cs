using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Configuration;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using AjaxControlToolkit;
using BusinessEntity;
using BusinessLayer;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.Table;
using System.Web.UI.HtmlControls;

public partial class LocationHistory : System.Web.UI.Page
{
    GeneralFunctions objgn = new GeneralFunctions();

    User objPropUser = new User();
    BL_User objBL_User = new BL_User();

    Customer objPropCustomer = new Customer();
    BL_Customer objBL_Customer = new BL_Customer();

    BL_Report bL_Report = new BL_Report();
    Chart objChart = new Chart();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userid"] == null)
        {
            Response.Redirect("login.aspx");
        }

        if (!IsPostBack)
        {
            StiWebViewerLocationHistory.Report = LoadLocationHistoryReport();
            StiWebViewerLocationHistory.Visible = true;

            HighlightSideMenu("cstmMgr", "lnkLocationsSMenu", "cstmMgrSub");

            GetSMTPUser();
            SetAddress();
            string FileName = "LocationHistoryReport.pdf";
            ArrayList lstPath = new ArrayList();
            if (ViewState["pathmailatt"] != null)
            {
                lstPath = (ArrayList)ViewState["pathmailatt"];
                lstPath.Add(FileName);
            }
            else
            {
                lstPath.Add(FileName);
            }

            ViewState["pathmailatt"] = lstPath;
            dlAttachmentsDelete.DataSource = lstPath;
            dlAttachmentsDelete.DataBind();

            hdnFirstAttachement.Value = FileName;
        }
    }

    protected void lnkClose_Click(object sender, EventArgs e)
    {
        Session.Remove("locations");

        if (!string.IsNullOrEmpty(Request["redirect"]))
        {
            Response.Redirect(HttpUtility.UrlDecode(Request.QueryString["redirect"]));
        }
        else
        {
            Response.Redirect("Home.aspx");
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        StiWebViewerLocationHistory.Report = LoadLocationHistoryReport();
        StiWebViewerLocationHistory.Visible = true;
    }

    protected void StiWebViewerLocationHistory_GetReport(object sender, Stimulsoft.Report.Web.StiReportDataEventArgs e)
    {

    }

    protected void StiWebViewerLocationHistory_GetReportData(object sender, Stimulsoft.Report.Web.StiReportDataEventArgs e)
    {

    }

    private StiReport LoadLocationHistoryReport()
    {
        try
        {
            string reportPathStimul = string.Empty;
            reportPathStimul = Server.MapPath("StimulsoftReports/LocationHistoryReport.mrt");
            StiReport report = new StiReport();
            report.Load(reportPathStimul);

            StiPage page = report.Pages[0];
            page.CanGrow = true;
            page.CanShrink = true;

            if (Request.QueryString["lid"] != null)
            {
                CreateEquipmentInfoComponent(report);
            }

            //report.Compile();

            DataSet dsC = new DataSet();

            var connString = Session["config"].ToString();
            objPropUser.ConnConfig = connString;
            dsC = objBL_User.getControl(objPropUser);

            DataTable cTable = BuildCompanyDetailsTable();
            var cRow = cTable.NewRow();

            DataSet companyInfo = new DataSet();
            companyInfo = bL_Report.GetCompanyDetails(Session["config"].ToString());

            cRow["CompanyName"] = string.IsNullOrEmpty(companyInfo.Tables[0].Rows[0]["Name"].ToString()) ? "" : companyInfo.Tables[0].Rows[0]["Name"].ToString();
            cRow["CompanyAddress"] = string.IsNullOrEmpty(companyInfo.Tables[0].Rows[0]["Address"].ToString()) ? "" : companyInfo.Tables[0].Rows[0]["Address"].ToString();
            cRow["ContactNo"] = string.IsNullOrEmpty(companyInfo.Tables[0].Rows[0]["Contact"].ToString()) ? "" : companyInfo.Tables[0].Rows[0]["Contact"].ToString();
            cRow["Email"] = string.IsNullOrEmpty(companyInfo.Tables[0].Rows[0]["Email"].ToString()) ? "" : companyInfo.Tables[0].Rows[0]["Email"].ToString();
            cRow["City"] = string.IsNullOrEmpty(companyInfo.Tables[0].Rows[0]["City"].ToString()) ? "" : companyInfo.Tables[0].Rows[0]["City"].ToString();
            cRow["State"] = string.IsNullOrEmpty(companyInfo.Tables[0].Rows[0]["State"].ToString()) ? "" : companyInfo.Tables[0].Rows[0]["State"].ToString();
            cRow["Phone"] = string.IsNullOrEmpty(companyInfo.Tables[0].Rows[0]["Phone"].ToString()) ? "" : companyInfo.Tables[0].Rows[0]["Phone"].ToString();
            cRow["Fax"] = string.IsNullOrEmpty(companyInfo.Tables[0].Rows[0]["Fax"].ToString()) ? "" : companyInfo.Tables[0].Rows[0]["Fax"].ToString();
            cRow["Zip"] = string.IsNullOrEmpty(companyInfo.Tables[0].Rows[0]["Zip"].ToString()) ? "" : companyInfo.Tables[0].Rows[0]["Zip"].ToString();
            cTable.Rows.Add(cRow);

            if (Request.QueryString["lid"] != null)
            {
                var locID = Convert.ToInt32(Request.QueryString["lid"]);
                objChart.ConnConfig = Session["config"].ToString();

                if (!string.IsNullOrEmpty(txtfromDate.Text))
                {
                    objChart.StartDate = Convert.ToDateTime(txtfromDate.Text);
                }

                if (!string.IsNullOrEmpty(txtToDate.Text))
                {
                    var endDate = Convert.ToDateTime(txtToDate.Text);
                    objChart.EndDate = endDate.AddDays(1).AddSeconds(-1);
                }

                var dsTicketHistory = bL_Report.GetRecurringMaintenanceHistory(objChart, locID);
                var dsLocationInfo = bL_Report.GetLocationAndContractDetail(objChart, locID);

                if (dsLocationInfo.Tables[0].Rows.Count > 0)
                {
                    var locRow = dsLocationInfo.Tables[0].Rows[0];
                    if (locRow["Lat"] != null && !string.IsNullOrEmpty(locRow["Lat"].ToString()) && locRow["Lng"] != null && !string.IsNullOrEmpty(locRow["Lng"].ToString()))
                    {
                        var apiKey = "AIzaSyCOEFmWyvAHmHlSxhCUHFjppWvgZCh62Ik";
                        if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["GoogleAPIKey"]))
                        {
                            apiKey = ConfigurationManager.AppSettings["GoogleAPIKey"];
                        }

                        report["MapURL"] = string.Format("https://maps.googleapis.com/maps/api/staticmap?center={0},{1}&zoom=13&size=500x250&maptype=roadmap&markers=color:red%7C{0},{1}&key={2}",
                            locRow["Lat"].ToString(), locRow["Lng"].ToString(), apiKey);
                    }
                }

                report.RegData("LocationDetails", dsLocationInfo.Tables[0]);
                report.RegData("LocationContracts", dsLocationInfo.Tables[1]);
                report.RegData("RecurringHistory", dsTicketHistory.Tables[0]);
                report.RegData("TicketCategoriesHistory", dsTicketHistory.Tables[1]);
            }

            report.RegData("CompanyDetails", cTable);
            report["Username"] = Session["Username"].ToString();

            report.Render();

            return report;
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);

            return null;
        }
    }

    private void CreateEquipmentInfoComponent(StiReport report)
    {
        var locationID = Convert.ToInt32(Request.QueryString["lid"]);
        var ds = bL_Report.GetLocationEquipment(Session["config"].ToString(), locationID);

        StiPage page = report.Pages[0];
        var pageWidth = page.Width;

        #region Equipment info

        StiColumnHeaderBand equipNoTitle = new StiColumnHeaderBand();
        equipNoTitle.Height = 0.3;
        equipNoTitle.PrintIfEmpty = true;
        equipNoTitle.PrintOn = StiPrintOnType.OnlyFirstPage;
        page.Components.Add(equipNoTitle);

        StiText equipNoTitleText = new StiText(new RectangleD(0, 0, pageWidth, 0.2));
        equipNoTitleText.Text.Value = string.Format("No. of Equipment: {0}", ds.Tables[0].Rows.Count);
        equipNoTitleText.HorAlignment = StiTextHorAlignment.Left;
        equipNoTitleText.VertAlignment = StiVertAlignment.Center;
        equipNoTitleText.Font = new Font("Arial", 9F, FontStyle.Bold);
        equipNoTitleText.WordWrap = true;
        equipNoTitle.Components.Add(equipNoTitleText);

        if (ds.Tables[0].Rows.Count > 0)
        {
            // Show 6 Equipments on table
            var dtEquipment = BuildEquipmentTable(ds);
            var totalGrid = Math.Ceiling((decimal)(dtEquipment.Columns.Count - 1) / 6);
            var startEquip = 1;
            var endEquip = totalGrid == 1 ? dtEquipment.Columns.Count - 1 : 6;

            double descWidth = pageWidth / 4;
            double cellWidth = pageWidth / 8;

            for (int i = 0; i < totalGrid; i++)
            {
                int indexCell = 0;

                //Equipment table
                StiTable equipmentTable = new StiTable();
                equipmentTable.Width = descWidth + (endEquip - startEquip + 1) * cellWidth;
                equipmentTable.ColumnCount = endEquip - startEquip + 2;
                equipmentTable.RowCount = dtEquipment.Rows.Count + 1;
                equipmentTable.AutoWidth = StiTableAutoWidth.Page;
                equipmentTable.AutoWidthType = StiTableAutoWidthType.FullTable;
                equipmentTable.CanBreak = true;
                equipmentTable.NewPageAfter = true;
                page.Components.Add(equipmentTable);
                equipmentTable.CreateCell();

                StiTableCell headerDesc = equipmentTable.Components[indexCell] as StiTableCell;
                headerDesc.VertAlignment = StiVertAlignment.Center;
                headerDesc.Font = new System.Drawing.Font("Arial", 9F, FontStyle.Bold);
                headerDesc.Border = new StiBorder(StiBorderSides.All, Color.FromArgb(89, 89, 89), 1, StiPenStyle.Solid);
                headerDesc.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
                headerDesc.TextBrush = new StiSolidBrush(Color.White);
                headerDesc.WordWrap = true;
                headerDesc.Text.Value = "Equipment ID";
                headerDesc.Width = descWidth;
                headerDesc.HorAlignment = StiTextHorAlignment.Left;
                headerDesc.Margins = new StiMargins(5, 5, 0, 0);

                indexCell++;

                for (int j = startEquip; j <= endEquip; j++)
                {
                    DataRow drHeader = ds.Tables[0].AsEnumerable()
                            .FirstOrDefault(row => row.Field<Int32>("ID").ToString() == dtEquipment.Columns[j].ColumnName);

                    StiTableCell headerCell = equipmentTable.Components[indexCell] as StiTableCell;
                    headerCell.Text.Value = drHeader != null ? drHeader["Unit"].ToString() : "";
                    headerCell.VertAlignment = StiVertAlignment.Center;
                    headerCell.Font = new System.Drawing.Font("Arial", 9F, FontStyle.Bold);
                    headerCell.Border = new StiBorder(StiBorderSides.All, Color.FromArgb(89, 89, 89), 1, StiPenStyle.Solid);
                    headerCell.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
                    headerCell.TextBrush = new StiSolidBrush(Color.White);
                    headerCell.Width = cellWidth;
                    headerCell.HorAlignment = StiTextHorAlignment.Center;

                    indexCell++;
                }

                foreach (DataRow row in dtEquipment.Rows)
                {
                    StiTableCell dataDesc = equipmentTable.Components[indexCell] as StiTableCell;
                    dataDesc.Text.Value = row[0].ToString();
                    dataDesc.Width = descWidth;
                    dataDesc.HorAlignment = StiTextHorAlignment.Left;
                    dataDesc.VertAlignment = StiVertAlignment.Center;
                    dataDesc.Font = new System.Drawing.Font("Arial", 9F);
                    dataDesc.Margins = new StiMargins(5, 5, 0, 0);
                    dataDesc.Border = new StiBorder(StiBorderSides.All, Color.FromArgb(89, 89, 89), 1, StiPenStyle.Solid);
                    dataDesc.WordWrap = true;
                    dataDesc.CanGrow = true;

                    indexCell++;

                    for (int c = startEquip; c <= endEquip; c++)
                    {
                        StiTableCell dataCell = equipmentTable.Components[indexCell] as StiTableCell;
                        dataCell.Width = cellWidth;
                        dataCell.Text.Value = row[c].ToString();
                        dataCell.HorAlignment = StiTextHorAlignment.Left;
                        dataCell.VertAlignment = StiVertAlignment.Center;
                        dataCell.Font = new System.Drawing.Font("Arial", 9F);
                        dataCell.Margins = new StiMargins(5, 5, 0, 0);
                        dataCell.Border = new StiBorder(StiBorderSides.All, Color.FromArgb(89, 89, 89), 1, StiPenStyle.Solid);
                        dataCell.WordWrap = true;
                        dataCell.CanGrow = true;

                        indexCell++;
                    }
                }

                startEquip = startEquip + 6;
                endEquip = endEquip + 6;
                if (endEquip > dtEquipment.Columns.Count - 1)
                {
                    endEquip = dtEquipment.Columns.Count - 1;
                }
            }

            // MCP History title
            StiHeaderBand mcpTitle = new StiHeaderBand();
            mcpTitle.Height = 0.5;
            mcpTitle.PrintIfEmpty = true;
            page.Components.Add(mcpTitle);

            StiText mcpTitleText = new StiText(new RectangleD(0, 0, pageWidth, 0.25));
            mcpTitleText.Text.Value = "MAINTENANCE CONTROL PLAN";
            mcpTitleText.HorAlignment = StiTextHorAlignment.Center;
            mcpTitleText.VertAlignment = StiVertAlignment.Center;
            mcpTitleText.Font = new Font("Arial", 20F, System.Drawing.FontStyle.Bold);
            mcpTitleText.WordWrap = true;
            mcpTitle.Components.Add(mcpTitleText);

            // MCP History sorted by date
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                objChart.ConnConfig = Session["config"].ToString();
                var dsEquips = bL_Report.GetMCPBuildingHistory(objChart, Convert.ToInt32(row["ID"]));

                for (int i = 0; i < dsEquips.Tables[0].Rows.Count; i++)
                {
                    bool newPage = false;
                    var isLast = (i == dsEquips.Tables[0].Rows.Count - 1);
                    DataRow temp = dsEquips.Tables[0].Rows[i];

                    var dtEquipItem = dsEquips.Tables[1].AsEnumerable()
                        .Where(r => r.Field<Int32>("EquipT").ToString() == temp["EquipT"].ToString())
                        .GroupBy(x => x.Field<string>("Code")).Select(x => x.FirstOrDefault());

                    var dtRepDetail = dsEquips.Tables[2].AsEnumerable()
                       .Where(r => r.Field<Int32>("EquipT").ToString() == temp["EquipT"].ToString());

                    if (!isLast)
                    {
                        DataRow afterRow = dsEquips.Tables[0].Rows[i + 1];
                        var afterRepDetail = dsEquips.Tables[2].AsEnumerable()
                       .Where(r => r.Field<Int32>("EquipT").ToString() == afterRow["EquipT"].ToString());

                        newPage = afterRepDetail.Count() > 0;
                    }

                    if (dtEquipItem.Count() > 0 && dtRepDetail.Count() > 0)
                    {
                        CreateEquipmentMCPComponent(dtEquipItem, dtRepDetail, page, temp["EquipDesc"].ToString(), row["Unit"].ToString(), !isLast && newPage);
                    }
                }
            }
        }
        else
        {
            equipNoTitle.NewPageAfter = true;
        }

        #endregion

        #region Shut Down History

        if (ds.Tables[0].Rows.Count > 0)
        {
            objChart.ConnConfig = Session["config"].ToString();
            if (!string.IsNullOrEmpty(txtfromDate.Text))
            {
                objChart.StartDate = Convert.ToDateTime(txtfromDate.Text);
            }

            if (!string.IsNullOrEmpty(txtToDate.Text))
            {
                var endDate = Convert.ToDateTime(txtToDate.Text);
                objChart.EndDate = endDate.AddDays(1).AddSeconds(-1);
            }

            var dsShutDownLogs = bL_Report.GetShutDownHistory(objChart, locationID);
            StiPage shutdownPage = report.Pages[3];

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                var dtShutDown = dsShutDownLogs.Tables[0].AsEnumerable()
                    .Where(r => r.Field<Int32>("elev_id").ToString() == row["ID"].ToString());

                CreateShutDownComponent(dtShutDown, shutdownPage, row["Unit"].ToString());
            }
        }

        #endregion
    }

    private DataTable BuildEquipmentTable(DataSet ds)
    {
        DataTable equipmentTable = new DataTable();

        equipmentTable.Columns.Add("Desc");

        foreach (DataRow row in ds.Tables[0].Rows)
        {
            equipmentTable.Columns.Add(row["ID"].ToString());
        }

        // Add Type, Unique# and Status
        var rowType = equipmentTable.NewRow();
        var rowUnique = equipmentTable.NewRow();
        var rowStatus = equipmentTable.NewRow();

        rowType["Desc"] = "Type";
        rowUnique["Desc"] = "Unique #";
        rowStatus["Desc"] = "Status";

        foreach (DataRow dr in ds.Tables[1].Rows)
        {
            rowType[dr["ID"].ToString()] = dr["Type"].ToString();
            rowUnique[dr["ID"].ToString()] = dr["State"].ToString();
            rowStatus[dr["ID"].ToString()] = dr["Status"].ToString();
        }

        equipmentTable.Rows.Add(rowType);
        equipmentTable.Rows.Add(rowUnique);
        equipmentTable.Rows.Add(rowStatus);

        // Add Equipment Template
        foreach (DataRow dr in ds.Tables[2].Rows)
        {
            var dataRow = equipmentTable.NewRow();
            dataRow["Desc"] = dr[0].ToString();

            foreach (DataRow drEqui in ds.Tables[1].Rows)
            {
                DataRow drFiltered = ds.Tables[3].AsEnumerable()
                    .FirstOrDefault(row => row.Field<Int32>("Elev").ToString() == drEqui["ID"].ToString()
                           && row.Field<String>("fDesc") == dr[0].ToString());

                if (drFiltered != null)
                {
                    dataRow[drEqui["ID"].ToString()] = drFiltered["Value"].ToString();
                }
            }

            equipmentTable.Rows.Add(dataRow);
        }

        return equipmentTable;
    }

    private void CreateEquipmentMCPComponent(IEnumerable<DataRow> dtEquipItem, IEnumerable<DataRow> dtRepDetail, StiPage page, string tempName, string unit, bool newPage)
    {
        int indexDataCell = 0;
        var pageWidth = page.Width;
        double cellWidth = pageWidth / (dtEquipItem.Count() + 2);
        if (cellWidth > 1)
        {
            cellWidth = 1;
        }

        //Equipment Table
        if (dtEquipItem.Count() > 0)
        {
            var dtEquipTicket = BuildEquipmentTicketTable(dtEquipItem, dtRepDetail);

            StiColumnHeaderBand tempTitle = new StiColumnHeaderBand();
            tempTitle.Height = 0.9;
            tempTitle.PrintIfEmpty = true;
            page.Components.Add(tempTitle);

            StiText equipTitleText = new StiText(new RectangleD(0.1, 0, pageWidth, 0.2));
            equipTitleText.Text.Value = string.Format("Equipment ID: {0}", unit);
            equipTitleText.HorAlignment = StiTextHorAlignment.Left;
            equipTitleText.VertAlignment = StiVertAlignment.Center;
            equipTitleText.Font = new Font("Arial", 14F, FontStyle.Underline | FontStyle.Bold);
            equipTitleText.WordWrap = true;
            tempTitle.Components.Add(equipTitleText);

            StiText tempTitleText = new StiText(new RectangleD(0, 0.4, pageWidth, 0.2));
            tempTitleText.Text.Value = tempName.ToUpper();
            tempTitleText.HorAlignment = StiTextHorAlignment.Center;
            tempTitleText.VertAlignment = StiVertAlignment.Center;
            tempTitleText.Font = new Font("Arial", 16F, System.Drawing.FontStyle.Bold);
            tempTitleText.WordWrap = true;
            tempTitle.Components.Add(tempTitleText);

            StiTable equipmentTable = new StiTable();
            equipmentTable.ColumnCount = dtEquipItem.Count() + 2;
            equipmentTable.RowCount = dtEquipTicket.Rows.Count + 4;
            equipmentTable.Width = cellWidth * (dtEquipItem.Count() + 2);
            equipmentTable.AutoWidth = StiTableAutoWidth.Page;
            equipmentTable.AutoWidthType = StiTableAutoWidthType.FullTable;
            equipmentTable.CanBreak = true;
            if (newPage)
            {
                equipmentTable.NewPageAfter = true;
            }

            page.Components.Add(equipmentTable);
            equipmentTable.CreateCell();

            // Insert Code row
            StiTableCell codeHeaderCell = equipmentTable.Components[indexDataCell + 1] as StiTableCell;
            codeHeaderCell.Text.Value = "Code";
            codeHeaderCell.Width = cellWidth;
            codeHeaderCell.VertAlignment = StiVertAlignment.Center;
            codeHeaderCell.Font = new System.Drawing.Font("Arial", 8F, FontStyle.Bold);
            codeHeaderCell.Border = new StiBorder(StiBorderSides.All, Color.FromArgb(89, 89, 89), 1, StiPenStyle.Solid);
            codeHeaderCell.Brush = new StiSolidBrush(Color.FromArgb(217, 217, 217));
            codeHeaderCell.TextBrush = new StiSolidBrush(Color.Black);
            codeHeaderCell.HorAlignment = StiTextHorAlignment.Left;
            codeHeaderCell.Margins = new StiMargins(2, 2, 0, 0);

            indexDataCell = indexDataCell + 2;

            foreach (DataRow row in dtEquipItem)
            {
                StiTableCell dataCell = equipmentTable.Components[indexDataCell] as StiTableCell;
                dataCell.Text.Value = row["Code"].ToString();
                dataCell.Width = cellWidth;
                dataCell.HorAlignment = StiTextHorAlignment.Left;
                dataCell.VertAlignment = StiVertAlignment.Center;
                dataCell.Font = new System.Drawing.Font("Arial", 7F);
                dataCell.Margins = new StiMargins(2, 2, 0, 0);
                dataCell.Border = new StiBorder(StiBorderSides.All, Color.FromArgb(89, 89, 89), 1, StiPenStyle.Solid);
                dataCell.Brush = new StiSolidBrush(Color.FromArgb(217, 217, 217));
                dataCell.WordWrap = true;
                dataCell.CanGrow = true;

                indexDataCell++;
            }

            // Insert Description row
            StiTableCell descHeaderCell = equipmentTable.Components[indexDataCell + 1] as StiTableCell;
            descHeaderCell.Text.Value = "Description";
            descHeaderCell.Width = cellWidth;
            descHeaderCell.VertAlignment = StiVertAlignment.Center;
            descHeaderCell.Font = new System.Drawing.Font("Arial", 8F, FontStyle.Bold);
            descHeaderCell.Border = new StiBorder(StiBorderSides.All, Color.FromArgb(89, 89, 89), 1, StiPenStyle.Solid);
            descHeaderCell.Brush = new StiSolidBrush(Color.FromArgb(217, 217, 217));
            descHeaderCell.TextBrush = new StiSolidBrush(Color.Black);
            descHeaderCell.HorAlignment = StiTextHorAlignment.Left;
            descHeaderCell.Margins = new StiMargins(2, 2, 0, 0);
            descHeaderCell.Height = 1.8;

            indexDataCell = indexDataCell + 2;

            foreach (DataRow row in dtEquipItem)
            {
                StiTableCell dataCell = equipmentTable.Components[indexDataCell] as StiTableCell;
                dataCell.Text.Value = row["fDesc"].ToString();
                dataCell.Width = cellWidth;
                dataCell.HorAlignment = StiTextHorAlignment.Left;
                dataCell.VertAlignment = StiVertAlignment.Center;
                dataCell.Font = new System.Drawing.Font("Arial", 8F);
                dataCell.Angle = 90;
                dataCell.Margins = new StiMargins(2, 2, 0, 0);
                dataCell.Border = new StiBorder(StiBorderSides.All, Color.FromArgb(89, 89, 89), 1, StiPenStyle.Solid);
                dataCell.Brush = new StiSolidBrush(Color.FromArgb(217, 217, 217));
                dataCell.WordWrap = true;
                dataCell.Height = 1.8;

                indexDataCell++;
            }

            // Insert Frequency row
            StiTableCell frequencyHeaderCell = equipmentTable.Components[indexDataCell + 1] as StiTableCell;
            frequencyHeaderCell.Text.Value = "Frequency";
            frequencyHeaderCell.Width = cellWidth;
            frequencyHeaderCell.VertAlignment = StiVertAlignment.Center;
            frequencyHeaderCell.Font = new System.Drawing.Font("Arial", 8F, FontStyle.Bold);
            frequencyHeaderCell.Border = new StiBorder(StiBorderSides.All, Color.FromArgb(89, 89, 89), 1, StiPenStyle.Solid);
            frequencyHeaderCell.Brush = new StiSolidBrush(Color.FromArgb(217, 217, 217));
            frequencyHeaderCell.TextBrush = new StiSolidBrush(Color.Black);
            frequencyHeaderCell.HorAlignment = StiTextHorAlignment.Left;
            frequencyHeaderCell.Margins = new StiMargins(2, 2, 0, 0);

            indexDataCell = indexDataCell + 2;

            foreach (DataRow row in dtEquipItem)
            {
                StiTableCell dataCell = equipmentTable.Components[indexDataCell] as StiTableCell;
                dataCell.Text.Value = row["FrequencyName"].ToString();
                dataCell.Width = cellWidth;
                dataCell.HorAlignment = StiTextHorAlignment.Left;
                dataCell.VertAlignment = StiVertAlignment.Center;
                dataCell.Font = new System.Drawing.Font("Arial", 7F);
                dataCell.Margins = new StiMargins(2, 2, 0, 0);
                dataCell.Border = new StiBorder(StiBorderSides.All, Color.FromArgb(89, 89, 89), 1, StiPenStyle.Solid);
                dataCell.Brush = new StiSolidBrush(Color.FromArgb(217, 217, 217));
                dataCell.WordWrap = true;
                dataCell.CanGrow = true;

                indexDataCell++;
            }

            // Insert Ticket list
            StiTableCell ticketHeaderCell = equipmentTable.Components[indexDataCell] as StiTableCell;
            ticketHeaderCell.Text.Value = "Ticket #";
            ticketHeaderCell.Width = cellWidth;
            ticketHeaderCell.VertAlignment = StiVertAlignment.Center;
            ticketHeaderCell.Font = new System.Drawing.Font("Arial", 8F, FontStyle.Bold);
            ticketHeaderCell.Border = new StiBorder(StiBorderSides.All, Color.FromArgb(89, 89, 89), 1, StiPenStyle.Solid);
            ticketHeaderCell.Brush = new StiSolidBrush(Color.FromArgb(198, 215, 238));
            ticketHeaderCell.TextBrush = new StiSolidBrush(Color.Black);
            ticketHeaderCell.HorAlignment = StiTextHorAlignment.Left;
            ticketHeaderCell.Margins = new StiMargins(2, 2, 0, 0);

            StiTableCell dateHeaderCell = equipmentTable.Components[indexDataCell + 1] as StiTableCell;
            dateHeaderCell.Text.Value = "Date";
            dateHeaderCell.Width = cellWidth;
            dateHeaderCell.VertAlignment = StiVertAlignment.Center;
            dateHeaderCell.Font = new System.Drawing.Font("Arial", 8F, FontStyle.Bold);
            dateHeaderCell.Border = new StiBorder(StiBorderSides.All, Color.FromArgb(89, 89, 89), 1, StiPenStyle.Solid);
            dateHeaderCell.Brush = new StiSolidBrush(Color.FromArgb(198, 215, 238));
            dateHeaderCell.TextBrush = new StiSolidBrush(Color.Black);
            dateHeaderCell.HorAlignment = StiTextHorAlignment.Left;
            dateHeaderCell.Margins = new StiMargins(2, 2, 0, 0);

            indexDataCell = indexDataCell + 2;

            foreach (DataRow row in dtEquipItem)
            {
                StiTableCell dataCell = equipmentTable.Components[indexDataCell] as StiTableCell;
                dataCell.Width = cellWidth;
                dataCell.HorAlignment = StiTextHorAlignment.Left;
                dataCell.VertAlignment = StiVertAlignment.Center;
                dataCell.Font = new System.Drawing.Font("Arial", 7F);
                dataCell.Margins = new StiMargins(2, 2, 0, 0);
                dataCell.Border = new StiBorder(StiBorderSides.All, Color.FromArgb(89, 89, 89), 1, StiPenStyle.Solid);
                dataCell.Brush = new StiSolidBrush(Color.FromArgb(198, 215, 238));
                dataCell.WordWrap = true;
                dataCell.CanGrow = true;

                indexDataCell++;
            }

            foreach (DataRow row in dtEquipTicket.Rows)
            {
                foreach (DataColumn col in dtEquipTicket.Columns)
                {
                    StiTableCell dataCell = equipmentTable.Components[indexDataCell] as StiTableCell;
                    dataCell.Width = cellWidth;
                    dataCell.VertAlignment = StiVertAlignment.Center;
                    dataCell.Font = new System.Drawing.Font("Arial", 7F);
                    dataCell.Margins = new StiMargins(2, 2, 0, 0);
                    dataCell.Border = new StiBorder(StiBorderSides.All, Color.FromArgb(89, 89, 89), 1, StiPenStyle.Solid);
                    dataCell.WordWrap = true;
                    dataCell.CanGrow = true;

                    if (col.ColumnName == "TicketID")
                    {
                        dataCell.Text.Value = row[col.ColumnName].ToString();
                        dataCell.HorAlignment = StiTextHorAlignment.Right;
                        dataCell.Brush = new StiSolidBrush(Color.FromArgb(198, 215, 238));
                    }
                    else if (col.ColumnName == "Date")
                    {
                        dataCell.Text.Value = Convert.ToDateTime(row[col.ColumnName].ToString()).ToString("MM/dd/yyyy");
                        dataCell.HorAlignment = StiTextHorAlignment.Center;
                        dataCell.Brush = new StiSolidBrush(Color.FromArgb(198, 215, 238));
                    }
                    else
                    {
                        if (row[col.ColumnName].ToString() == "1")
                        {
                            dataCell.Brush = new StiSolidBrush(Color.Black);
                        }
                    }

                    indexDataCell++;
                }
            }
        }
    }

    private void CreateShutDownComponent(IEnumerable<DataRow> dtShutDown, StiPage page, string unit)
    {
        int indexDataCell = 7;
        var pageWidth = page.Width;

        //Shut Down log table
        if (dtShutDown.Count() > 0)
        {
            StiColumnHeaderBand equipTitle = new StiColumnHeaderBand();
            equipTitle.Height = 0.4;
            equipTitle.PrintIfEmpty = true;
            page.Components.Add(equipTitle);

            StiText equipTitleText = new StiText(new RectangleD(0.1, 0, pageWidth, 0.2));
            equipTitleText.Text.Value = string.Format("Equipment ID: {0}", unit);
            equipTitleText.HorAlignment = StiTextHorAlignment.Left;
            equipTitleText.VertAlignment = StiVertAlignment.Center;
            equipTitleText.Font = new Font("Arial", 14F, FontStyle.Underline | FontStyle.Bold);
            equipTitleText.WordWrap = true;
            equipTitle.Components.Add(equipTitleText);

            StiTable shutdownTable = new StiTable();
            shutdownTable.ColumnCount = 7;
            shutdownTable.RowCount = dtShutDown.Count() + 1;
            shutdownTable.Width = page.Width;
            shutdownTable.AutoWidth = StiTableAutoWidth.Page;
            shutdownTable.AutoWidthType = StiTableAutoWidthType.FullTable;
            shutdownTable.CanBreak = true;
            shutdownTable.NewPageAfter = true;
            page.Components.Add(shutdownTable);
            shutdownTable.CreateCell();

            // Table header
            StiTableCell dateHeaderCell = shutdownTable.Components[0] as StiTableCell;
            dateHeaderCell.Text.Value = "Date";
            dateHeaderCell.VertAlignment = StiVertAlignment.Center;
            dateHeaderCell.Font = new System.Drawing.Font("Arial", 8F, FontStyle.Bold);
            dateHeaderCell.Border = new StiBorder(StiBorderSides.All, Color.FromArgb(89, 89, 89), 1, StiPenStyle.Solid);
            dateHeaderCell.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
            dateHeaderCell.TextBrush = new StiSolidBrush(Color.Black);
            dateHeaderCell.HorAlignment = StiTextHorAlignment.Center;
            dateHeaderCell.Margins = new StiMargins(2, 2, 0, 0);

            StiTableCell timeHeaderCell = shutdownTable.Components[1] as StiTableCell;
            timeHeaderCell.Text.Value = "Time";
            timeHeaderCell.VertAlignment = StiVertAlignment.Center;
            timeHeaderCell.Font = new System.Drawing.Font("Arial", 8F, FontStyle.Bold);
            timeHeaderCell.Border = new StiBorder(StiBorderSides.All, Color.FromArgb(89, 89, 89), 1, StiPenStyle.Solid);
            timeHeaderCell.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
            timeHeaderCell.TextBrush = new StiSolidBrush(Color.Black);
            timeHeaderCell.HorAlignment = StiTextHorAlignment.Center;
            timeHeaderCell.Margins = new StiMargins(2, 2, 0, 0);

            StiTableCell userHeaderCell = shutdownTable.Components[2] as StiTableCell;
            userHeaderCell.Text.Value = "User";
            userHeaderCell.VertAlignment = StiVertAlignment.Center;
            userHeaderCell.Font = new System.Drawing.Font("Arial", 8F, FontStyle.Bold);
            userHeaderCell.Border = new StiBorder(StiBorderSides.All, Color.FromArgb(89, 89, 89), 1, StiPenStyle.Solid);
            userHeaderCell.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
            userHeaderCell.TextBrush = new StiSolidBrush(Color.Black);
            userHeaderCell.HorAlignment = StiTextHorAlignment.Center;
            userHeaderCell.Margins = new StiMargins(2, 2, 0, 0);

            StiTableCell ticketHeaderCell = shutdownTable.Components[3] as StiTableCell;
            ticketHeaderCell.Text.Value = "Ticket ID";
            ticketHeaderCell.VertAlignment = StiVertAlignment.Center;
            ticketHeaderCell.Font = new System.Drawing.Font("Arial", 8F, FontStyle.Bold);
            ticketHeaderCell.Border = new StiBorder(StiBorderSides.All, Color.FromArgb(89, 89, 89), 1, StiPenStyle.Solid);
            ticketHeaderCell.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
            ticketHeaderCell.TextBrush = new StiSolidBrush(Color.Black);
            ticketHeaderCell.HorAlignment = StiTextHorAlignment.Center;
            ticketHeaderCell.Margins = new StiMargins(2, 2, 0, 0);

            StiTableCell shutHeaderCell = shutdownTable.Components[4] as StiTableCell;
            shutHeaderCell.Text.Value = "Shut Down";
            shutHeaderCell.VertAlignment = StiVertAlignment.Center;
            shutHeaderCell.Font = new System.Drawing.Font("Arial", 8F, FontStyle.Bold);
            shutHeaderCell.Border = new StiBorder(StiBorderSides.All, Color.FromArgb(89, 89, 89), 1, StiPenStyle.Solid);
            shutHeaderCell.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
            shutHeaderCell.TextBrush = new StiSolidBrush(Color.Black);
            shutHeaderCell.HorAlignment = StiTextHorAlignment.Center;
            shutHeaderCell.Margins = new StiMargins(2, 2, 0, 0);

            StiTableCell reasonHeaderCell = shutdownTable.Components[5] as StiTableCell;
            reasonHeaderCell.Text.Value = "Reason";
            reasonHeaderCell.VertAlignment = StiVertAlignment.Center;
            reasonHeaderCell.Font = new System.Drawing.Font("Arial", 8F, FontStyle.Bold);
            reasonHeaderCell.Border = new StiBorder(StiBorderSides.All, Color.FromArgb(89, 89, 89), 1, StiPenStyle.Solid);
            reasonHeaderCell.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
            reasonHeaderCell.TextBrush = new StiSolidBrush(Color.Black);
            reasonHeaderCell.HorAlignment = StiTextHorAlignment.Center;
            reasonHeaderCell.Margins = new StiMargins(2, 2, 0, 0);

            StiTableCell descHeaderCell = shutdownTable.Components[6] as StiTableCell;
            descHeaderCell.Text.Value = "Long Description";
            descHeaderCell.VertAlignment = StiVertAlignment.Center;
            descHeaderCell.Font = new System.Drawing.Font("Arial", 8F, FontStyle.Bold);
            descHeaderCell.Border = new StiBorder(StiBorderSides.All, Color.FromArgb(89, 89, 89), 1, StiPenStyle.Solid);
            descHeaderCell.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
            descHeaderCell.TextBrush = new StiSolidBrush(Color.Black);
            descHeaderCell.HorAlignment = StiTextHorAlignment.Center;
            descHeaderCell.Margins = new StiMargins(2, 2, 0, 0);

            // Table body
            foreach (var row in dtShutDown)
            {
                StiTableCell dateRowCell = shutdownTable.Components[indexDataCell] as StiTableCell;
                dateRowCell.Text.Value = Convert.ToDateTime(row["created_on"].ToString()).ToString("MM/dd/yyyy");
                dateRowCell.VertAlignment = StiVertAlignment.Center;
                dateRowCell.Font = new System.Drawing.Font("Arial", 8F);
                dateRowCell.Border = new StiBorder(StiBorderSides.All, Color.FromArgb(89, 89, 89), 1, StiPenStyle.Solid);
                dateRowCell.TextBrush = new StiSolidBrush(Color.Black);
                dateRowCell.HorAlignment = StiTextHorAlignment.Center;
                dateRowCell.Margins = new StiMargins(2, 2, 0, 0);

                StiTableCell timeRowCell = shutdownTable.Components[indexDataCell + 1] as StiTableCell;
                timeRowCell.Text.Value = Convert.ToDateTime(row["created_on"].ToString()).ToString("hh:mm tt");
                timeRowCell.VertAlignment = StiVertAlignment.Center;
                timeRowCell.Font = new System.Drawing.Font("Arial", 8F);
                timeRowCell.Border = new StiBorder(StiBorderSides.All, Color.FromArgb(89, 89, 89), 1, StiPenStyle.Solid);
                timeRowCell.TextBrush = new StiSolidBrush(Color.Black);
                timeRowCell.HorAlignment = StiTextHorAlignment.Center;
                timeRowCell.Margins = new StiMargins(2, 2, 0, 0);

                StiTableCell userRowCell = shutdownTable.Components[indexDataCell + 2] as StiTableCell;
                userRowCell.Text.Value = row["created_by"].ToString();
                userRowCell.VertAlignment = StiVertAlignment.Center;
                userRowCell.Font = new System.Drawing.Font("Arial", 8F);
                userRowCell.Border = new StiBorder(StiBorderSides.All, Color.FromArgb(89, 89, 89), 1, StiPenStyle.Solid);
                userRowCell.TextBrush = new StiSolidBrush(Color.Black);
                userRowCell.HorAlignment = StiTextHorAlignment.Left;
                userRowCell.Margins = new StiMargins(2, 2, 0, 0);

                StiTableCell ticketRowCell = shutdownTable.Components[indexDataCell + 3] as StiTableCell;
                ticketRowCell.Text.Value = row["ticket_id"].ToString();
                ticketRowCell.VertAlignment = StiVertAlignment.Center;
                ticketRowCell.Font = new System.Drawing.Font("Arial", 8F, FontStyle.Bold);
                ticketRowCell.Border = new StiBorder(StiBorderSides.All, Color.FromArgb(89, 89, 89), 1, StiPenStyle.Solid);
                ticketRowCell.TextBrush = new StiSolidBrush(Color.Black);
                ticketRowCell.HorAlignment = StiTextHorAlignment.Left;
                ticketRowCell.Margins = new StiMargins(2, 2, 0, 0);

                StiTableCell shutRowCell = shutdownTable.Components[indexDataCell + 4] as StiTableCell;
                shutRowCell.Text.Value = row["status"].ToString();
                shutRowCell.VertAlignment = StiVertAlignment.Center;
                shutRowCell.Font = new System.Drawing.Font("Arial", 8F);
                shutRowCell.Border = new StiBorder(StiBorderSides.All, Color.FromArgb(89, 89, 89), 1, StiPenStyle.Solid);
                shutRowCell.TextBrush = new StiSolidBrush(Color.Black);
                shutRowCell.HorAlignment = StiTextHorAlignment.Center;
                shutRowCell.Margins = new StiMargins(2, 2, 0, 0);

                StiTableCell reasonRowCell = shutdownTable.Components[indexDataCell + 5] as StiTableCell;
                reasonRowCell.Text.Value = row["reason"].ToString();
                reasonRowCell.VertAlignment = StiVertAlignment.Center;
                reasonRowCell.Font = new System.Drawing.Font("Arial", 8F);
                reasonRowCell.Border = new StiBorder(StiBorderSides.All, Color.FromArgb(89, 89, 89), 1, StiPenStyle.Solid);
                reasonRowCell.TextBrush = new StiSolidBrush(Color.Black);
                reasonRowCell.HorAlignment = StiTextHorAlignment.Left;
                reasonRowCell.Margins = new StiMargins(2, 2, 0, 0);

                StiTableCell descRowCell = shutdownTable.Components[indexDataCell + 6] as StiTableCell;
                descRowCell.Text.Value = row["longdesc"].ToString();
                descRowCell.VertAlignment = StiVertAlignment.Center;
                descRowCell.Font = new System.Drawing.Font("Arial", 8F);
                descRowCell.Border = new StiBorder(StiBorderSides.All, Color.FromArgb(89, 89, 89), 1, StiPenStyle.Solid);
                descRowCell.TextBrush = new StiSolidBrush(Color.Black);
                descRowCell.HorAlignment = StiTextHorAlignment.Left;
                descRowCell.Margins = new StiMargins(2, 2, 0, 0);

                indexDataCell = indexDataCell + 7;
            }

        }
    }

    private DataTable BuildEquipmentTicketTable(IEnumerable<DataRow> dtEquipItem, IEnumerable<DataRow> dtRepDetail)
    {
        DataTable equipmentTable = new DataTable();

        equipmentTable.Columns.Add("TicketID");
        equipmentTable.Columns.Add("Date");

        foreach (DataRow row in dtEquipItem)
        {
            if (!equipmentTable.Columns.Contains(row["Code"].ToString()))
            {
                equipmentTable.Columns.Add(row["Code"].ToString());
            }
        }

        // Add Equipment Ticket
        foreach (DataRow dr in dtRepDetail)
        {
            DataRow drFiltered = equipmentTable.AsEnumerable()
                    .FirstOrDefault(row => row.Field<String>("TicketID") == dr["TicketID"].ToString()
                           && row.Field<String>("Date") == dr["Lastdate"].ToString());

            if (drFiltered != null)
            {
                drFiltered[dr["Code"].ToString()] = "1";
            }
            else
            {
                var dataRow = equipmentTable.NewRow();
                dataRow["TicketID"] = dr["TicketID"].ToString();
                dataRow["Date"] = dr["Lastdate"].ToString();
                dataRow[dr["Code"].ToString()] = "1";

                equipmentTable.Rows.Add(dataRow);
            }
        }

        return equipmentTable;
    }

    private DataTable BuildCompanyDetailsTable()
    {
        DataTable companyDetailsTable = new DataTable();
        companyDetailsTable.Columns.Add("CompanyAddress");
        companyDetailsTable.Columns.Add("CompanyName");
        companyDetailsTable.Columns.Add("ContactNo");
        companyDetailsTable.Columns.Add("Email");
        companyDetailsTable.Columns.Add("LogoURL");
        companyDetailsTable.Columns.Add("City");
        companyDetailsTable.Columns.Add("State");
        companyDetailsTable.Columns.Add("Zip");
        companyDetailsTable.Columns.Add("Fax");
        companyDetailsTable.Columns.Add("Phone");

        return companyDetailsTable;
    }

    #region Send email

    protected void lnkSearch_Click(object sender, EventArgs e)
    {
        FillDistributionList(ddlSearch.SelectedValue, txtSearch.Text);
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "UpdateSelectedRows", "UpdateSelectedRowsForGrid();", true);
        RadGrid_Emails.Rebind();
    }

    protected void lnkClear_Click(object sender, EventArgs e)
    {
        ddlSearch.SelectedIndex = 0;
        txtSearch.Text = string.Empty;
        FillDistributionList(ddlSearch.SelectedValue, txtSearch.Text);
        RadGrid_Emails.Rebind();
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "UpdateSelectedRows", "UpdateSelectedRowsForGrid();", true);
    }

    //public static string[] GetContactEmails(string prefixText, int count, string contextKey)
    //{
    //    //DataTable dt = (DataTable)HttpContext.Current.Session["DistributionList"];
    //    DataTable dt = WebBaseUtility.GetContactListOnExchangeServer();
    //    List<string> txtItems = new List<string>();
    //    String dbValues;

    //    foreach (DataRow row in dt.Rows)
    //    {
    //        dbValues = AutoCompleteExtender.CreateAutoCompleteItem(row["MemberName"].ToString() + "(" + row["MemberEmail"].ToString() + ")", row["MemberEmail"].ToString());
    //        txtItems.Add(dbValues);
    //    }

    //    return txtItems.ToArray();
    //}

    private void FillDistributionList(string searchType, string searchValue)
    {
        DataTable distributionList = new DataTable();
        DataTable distributionList1 = new DataTable();
        if (!string.IsNullOrEmpty(txtTo.Text))
        {
            distributionList1.Columns.Add("MemberEmail");
            distributionList1.Columns.Add("MemberName");
            distributionList1.Columns.Add("GroupName");
            distributionList1.Columns.Add("Type");
            DataRow dr = distributionList1.NewRow();
            dr[0] = txtTo.Text;
            dr[1] = txtTo.Text;
            dr[2] = "";
            dr[3] = "";
            distributionList1.Rows.InsertAt(dr, 0);
        }
        distributionList = WebBaseUtility.GetContactListOnExchangeServer();
        distributionList.Merge(distributionList1);
        IEnumerable<DataRow> rowSources;

        var emailList = distributionList.Clone();
        switch (searchType)
        {
            case "1":
                if (searchValue != "")
                    rowSources = (from myRow in distributionList.AsEnumerable()
                                  where myRow.Field<string>("MemberName").ToLower().Contains(searchValue.ToLower())
                                  select myRow).Distinct().OrderBy(e => e.Field<string>("MemberEmail")).OrderBy(e => e.Field<string>("GroupName"))
                                        .OrderBy(e => e.Field<string>("Type"));
                else
                    rowSources = (from myRow in distributionList.AsEnumerable()
                                  where myRow.Field<string>("MemberName").ToLower().Equals(searchValue.ToLower())
                                  select myRow).Distinct().OrderBy(e => e.Field<string>("MemberEmail")).OrderBy(e => e.Field<string>("GroupName"))
                                        .OrderBy(e => e.Field<string>("Type"));
                break;
            case "2":
                if (searchValue != "")
                    rowSources = (from myRow in distributionList.AsEnumerable()
                                  where myRow.Field<string>("MemberEmail").ToLower().Contains(searchValue.ToLower())
                                  select myRow).Distinct().OrderBy(e => e.Field<string>("GroupName")).OrderBy(e => e.Field<string>("MemberEmail"))
                                        .OrderBy(e => e.Field<string>("Type"));
                else
                    rowSources = (from myRow in distributionList.AsEnumerable()
                                  where myRow.Field<string>("MemberEmail").ToLower().Equals(searchValue.ToLower())
                                  select myRow).Distinct().OrderBy(e => e.Field<string>("GroupName")).OrderBy(e => e.Field<string>("MemberEmail"))
                                    .OrderBy(e => e.Field<string>("Type"));
                break;
            case "3":
                if (searchValue != "")
                    rowSources = (from myRow in distributionList.AsEnumerable()
                                  where myRow.Field<string>("GroupName").ToLower().Contains(searchValue.ToLower())
                                  select myRow).Distinct().OrderBy(e => e.Field<string>("MemberEmail")).OrderBy(e => e.Field<string>("GroupName"))
                                        .OrderBy(e => e.Field<string>("Type"));
                else
                    rowSources = (from myRow in distributionList.AsEnumerable()
                                  where myRow.Field<string>("GroupName").ToLower().Equals(searchValue.ToLower())
                                  select myRow).Distinct().OrderBy(e => e.Field<string>("MemberEmail")).OrderBy(e => e.Field<string>("GroupName"))
                                    .OrderBy(e => e.Field<string>("Type"));
                break;
            case "4":
                if (searchValue != "")
                    rowSources = (from myRow in distributionList.AsEnumerable()
                                  where myRow.Field<string>("Type").ToLower().Contains(searchValue.ToLower())
                                  select myRow).Distinct().OrderBy(e => e.Field<string>("MemberEmail")).OrderBy(e => e.Field<string>("GroupName"))
                                        .OrderBy(e => e.Field<string>("Type"));
                else
                    rowSources = (from myRow in distributionList.AsEnumerable()
                                  where myRow.Field<string>("Type").ToLower().Equals(searchValue.ToLower())
                                  select myRow).Distinct().OrderBy(e => e.Field<string>("MemberEmail")).OrderBy(e => e.Field<string>("GroupName"))
                                        .OrderBy(e => e.Field<string>("Type"));
                break;
            default:
                rowSources = (from myRow in distributionList.AsEnumerable()
                              where myRow.Field<string>("GroupName").ToLower().Contains(searchValue.ToLower())
                                  || myRow.Field<string>("MemberEmail").ToLower().Contains(searchValue.ToLower())
                                  || myRow.Field<string>("MemberName").ToLower().Contains(searchValue.ToLower())
                                  || myRow.Field<string>("Type").ToLower().Contains(searchValue.ToLower())
                              select myRow).Distinct().OrderBy(e => e.Field<string>("MemberEmail"))
                                        .OrderBy(e => e.Field<string>("GroupName"))
                                        .OrderBy(e => e.Field<string>("Type"));
                break;
        }

        if (rowSources.Any())
        {
            emailList = rowSources.CopyToDataTable();
        }
        else
        {
            emailList = distributionList.Clone();
        }

        lblRecordCount.Text = emailList.Rows.Count + " Record(s) found";
        RadGrid_Emails.DataSource = emailList;
        RadGrid_Emails.VirtualItemCount = emailList.Rows.Count;

    }

    protected void RadGrid_Emails_PreRender(object sender, EventArgs e)
    {
        String filterExpression = Convert.ToString(RadGrid_Emails.MasterTableView.FilterExpression);
        if (filterExpression != "")
        {
            Session["Emails_FilterExpression"] = filterExpression;
            List<RetainFilter> filters = new List<RetainFilter>();

            foreach (GridColumn column in RadGrid_Emails.MasterTableView.OwnerGrid.Columns)
            {
                String filterValues = column.CurrentFilterValue;
                if (filterValues != "")
                {
                    String columnName = column.UniqueName;
                    RetainFilter filter = new RetainFilter();
                    filter.FilterColumn = columnName;
                    filter.FilterValue = filterValues;
                    filters.Add(filter);
                }
            }

            Session["Emails_Filters"] = filters;
        }
        else
        {
            Session["Emails_FilterExpression"] = null;
            Session["Emails_Filters"] = null;
        }

        ScriptManager.RegisterStartupScript(this, Page.GetType(), "UpdateSelectedRows", "UpdateSelectedRowsForGrid();", true);
    }

    protected void RadGrid_Emails_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        if (!IsPostBack)
        {

            if (Session["Emails_FilterExpression"] != null && Convert.ToString(Session["Emails_FilterExpression"]) != "" && Session["Emails_Filters"] != null)
            {
                RadGrid_Emails.MasterTableView.FilterExpression = Convert.ToString(Session["Emails_FilterExpression"]);
                var filtersGet = Session["Emails_Filters"] as List<RetainFilter>;
                if (filtersGet != null)
                {
                    foreach (var _filter in filtersGet)
                    {
                        GridColumn column = RadGrid_Emails.MasterTableView.GetColumnSafe(_filter.FilterColumn);
                        column.CurrentFilterValue = _filter.FilterValue;
                    }
                }
            }
        }

        FillDistributionList(ddlSearch.SelectedValue, txtSearch.Text);
    }

    private void GetSMTPUser()
    {
        objPropUser.ConnConfig = Session["config"].ToString();
        objPropUser.UserID = Convert.ToInt32(Session["UserID"]);
        DataSet ds = new DataSet();
        ds = objBL_User.getSMTPByUserID(objPropUser);
        if (ds.Tables[0].Rows.Count > 0)
        {
            String emailFrom = "";
            emailFrom = Convert.ToString(ds.Tables[0].Rows[0]["From"]);
            if (emailFrom == "")
            {
                SmtpSection section = (SmtpSection)ConfigurationManager.GetSection("system.net/mailSettings/smtp");

                string user = section.Network.UserName;
                txtFrom.Text = user;
            }
            else
            {
                txtFrom.Text = emailFrom;
            }
            txtEmailBCC.Text = Convert.ToString(ds.Tables[0].Rows[0]["BCCEmail"]);
            //txtFrom.ReadOnly = true;
        }
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

                if (txtEmailBCC.Text.Trim() != string.Empty)
                {
                    mail.Bcc = txtEmailBCC.Text.Split(';', ',').OfType<string>().ToList();
                }

                mail.Title = "Location History Report";
                if (txtBody.Text.Trim() != string.Empty)
                {
                    mail.Text = txtBody.Text.Replace(Environment.NewLine, "<BR/>");
                }
                else
                {
                    mail.Text = "This is report email sent from Mobile Office Manager. Please find the Location History Report attached.";
                }

                byte[] buffer1 = null;

                var settings = new Stimulsoft.Report.Export.StiPdfExportSettings();
                var service = new Stimulsoft.Report.Export.StiPdfExportService();
                System.IO.MemoryStream stream = new System.IO.MemoryStream();
                service.ExportTo(LoadLocationHistoryReport(), stream, settings);
                buffer1 = stream.ToArray();

                if (hdnFirstAttachement.Value != "-1")
                {
                    mail.attachmentBytes = buffer1;
                }

                ArrayList lst = new ArrayList();
                if (ViewState["pathmailatt"] != null)
                {
                    lst = (ArrayList)ViewState["pathmailatt"];
                    foreach (string strpath in lst)
                    {
                        if (strpath != "LocationHistoryReport.pdf")
                        {
                            mail.AttachmentFiles.Add(strpath);
                        }
                    }
                }

                mail.FileName = "LocationHistoryReport.pdf";

                mail.DeleteFilesAfterSend = true;
                mail.RequireAutentication = false;
                WebBaseUtility.UpdateMailConfigurationFromLoginUser(mail);
                mail.Send();

                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "noty({text: 'Email sent successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);

            }
            catch (Exception ex)
            {
                string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(ex.Message);
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            }
        }
    }

    protected void lnkUploadDoc_Click(object sender, EventArgs e)
    {
        string savepathconfig = System.Web.Configuration.WebConfigurationManager.AppSettings["AttachmentPath"].Trim();
        string savepath = savepathconfig + @"\mailattach\";
        string filename = FileUpload1.FileName;
        string fullpath = savepath + filename;

        if (File.Exists(fullpath))
        {
            filename = objgn.generateRandomString(4) + "_" + filename;
            fullpath = savepath + filename;
        }

        if (!Directory.Exists(savepath))
        {
            Directory.CreateDirectory(savepath);
        }
        FileUpload1.SaveAs(fullpath);


        ArrayList lstPath = new ArrayList();
        if (ViewState["pathmailatt"] != null)
        {
            lstPath = (ArrayList)ViewState["pathmailatt"];
            lstPath.Add(fullpath);
        }
        else
        {
            lstPath.Add(fullpath);
        }

        ViewState["pathmailatt"] = lstPath;
        dlAttachmentsDelete.DataSource = lstPath;
        dlAttachmentsDelete.DataBind();

        txtBody.Focus();
    }

    protected void imgDelAttach_Click(object sender, EventArgs e)
    {
        ImageButton btn = (ImageButton)sender;
        string path = btn.CommandArgument;
        if (hdnFirstAttachement.Value == path)
        {
            hdnFirstAttachement.Value = "-1";
        }
        ArrayList lstPath = (ArrayList)ViewState["pathmailatt"];
        lstPath.Remove(path);
        ViewState["pathmailatt"] = lstPath;
        dlAttachmentsDelete.DataSource = lstPath;
        dlAttachmentsDelete.DataBind();
        DeleteFile(path);
    }

    protected void btnAttachmentDel_Click(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)sender;
        string path = btn.CommandArgument;
        DownloadDocument(path, Path.GetFileName(path));
    }

    private void DeleteFile(string filepath)
    {
        ////this should delete the file in the next reboot, not now.
        if (System.IO.File.Exists(filepath))
        {
            // Use a try block to catch IOExceptions, to 
            // handle the case of the file already being 
            // opened by another process. 
            try
            {
                System.IO.File.Delete(filepath);
            }
            catch (System.IO.IOException e)
            {
                Console.WriteLine(e.Message);
                //return;
            }
        }
    }

    private void DownloadDocument(string filePath, string DownloadFileName)
    {
        try
        {
            System.IO.FileInfo FileName = new System.IO.FileInfo(filePath);
            FileStream myFile = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            BinaryReader _BinaryReader = new BinaryReader(myFile);

            try
            {
                long startBytes = 0;
                string lastUpdateTiemStamp = File.GetLastWriteTimeUtc(filePath).ToString("r");
                string _EncodedData = HttpUtility.UrlEncode(DownloadFileName, Encoding.UTF8) + lastUpdateTiemStamp;

                Response.Clear();
                Response.Buffer = false;
                Response.AddHeader("Accept-Ranges", "bytes");
                Response.AppendHeader("ETag", "\"" + _EncodedData + "\"");
                Response.AppendHeader("Last-Modified", lastUpdateTiemStamp);
                Response.ContentType = "application/octet-stream";
                Response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(DownloadFileName));
                Response.AddHeader("Content-Length", (FileName.Length - startBytes).ToString());
                Response.AddHeader("Connection", "Keep-Alive");
                Response.ContentEncoding = Encoding.UTF8;

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
        catch (FileNotFoundException ex)
        {
            if (DownloadFileName == "LocationHistoryReport.pdf")
            {
                byte[] buffer1 = null;

                var settings = new Stimulsoft.Report.Export.StiPdfExportSettings();
                var service = new Stimulsoft.Report.Export.StiPdfExportService();
                System.IO.MemoryStream stream = new System.IO.MemoryStream();
                service.ExportTo(LoadLocationHistoryReport(), stream, settings);
                buffer1 = stream.ToArray();

                Response.Clear();
                MemoryStream ms = new MemoryStream(buffer1);
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=LocationHistoryReport.pdf");
                Response.Buffer = true;
                ms.WriteTo(Response.OutputStream);
                Response.End();
            }
            else
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "FileaccessWarning", "alert('File not found.');", true);
            }
        }
        catch (UnauthorizedAccessException ex)
        {
            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "FileaccessWarning", "alert('Please provide access permissions to the file path.');", true);
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);

            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "FileerrorWarning", "alert('" + str + "');", true);
        }
    }

    private void SetAddress()
    {
        var address = WebBaseUtility.GetSignature();

        string mailBody = "Please review the attached Location History Report.";
        address = mailBody + Environment.NewLine + "<br />" + Environment.NewLine + "<br />" + address;

        txtBody.Text = address;

        
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

    #endregion
}