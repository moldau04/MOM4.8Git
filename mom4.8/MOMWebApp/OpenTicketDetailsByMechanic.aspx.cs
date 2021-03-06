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
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BusinessEntity;
using BusinessLayer;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dictionary;
using Telerik.Web.UI;

public partial class OpenTicketDetailsByMechanic : System.Web.UI.Page
{
    GeneralFunctions objgn = new GeneralFunctions();

    BL_MapData objBL_MapData = new BL_MapData();
    MapData objMapData = new MapData();

    BL_User objBL_User = new BL_User();
    User objPropUser = new User();

    BL_Report bL_Report = new BL_Report();
    BL_ReportsData objBL_ReportsData = new BL_ReportsData();

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
                HighlightSideMenu("schMgr", "lnkListView", "schdMgrSub");

                GetSMTPUser();
                SetAddress();
                string FileName = "OpenTicketsDetailsByMechanic.pdf";
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
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected DataTable BuildCompanyDetailsTable()
    {
        DataTable companyDetailsTable = new DataTable();
        companyDetailsTable.Columns.Add("CompanyAddress");
        companyDetailsTable.Columns.Add("CompanyName");
        companyDetailsTable.Columns.Add("ContactNo");
        companyDetailsTable.Columns.Add("Email");
        companyDetailsTable.Columns.Add("LogoURL");

        return companyDetailsTable;
    }

    protected DataTable BuildTicketDetailsTable()
    {
        DataSet dsCategories = objBL_MapData.GetCategories(objMapData);
        DataTable ticketDetailsTable = new DataTable();
        ticketDetailsTable.Columns.Add("Mechanic Name");
        ticketDetailsTable.Columns.Add("Department");

        for (int i = 0; i < dsCategories.Tables[0].Rows.Count; i++)
        {
            ticketDetailsTable.Columns.Add(dsCategories.Tables[0].Rows[i]["Category"].ToString());
        }

        if (!ticketDetailsTable.Columns.Contains("None"))
        {
            ticketDetailsTable.Columns.Add("None");
        }

        ticketDetailsTable.Columns.Add("Total");
        ticketDetailsTable.Columns.Add("Completed Tickets");

        return ticketDetailsTable;
    }

    protected void StiWebViewerOpenTicketDetailsByMechanic_GetReport(object sender, Stimulsoft.Report.Web.StiReportDataEventArgs e)
    {
        e.Report = LoadReport();    
    }

    protected void StiWebViewerOpenTicketDetailsByMechanic_GetReportData(object sender, Stimulsoft.Report.Web.StiReportDataEventArgs e)
    {
        
    }

    protected void lnkClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("TicketListView.aspx?fil=1");
    }

    private StiReport LoadReport()
    {
        try
        {
            string reportPathStimul = Server.MapPath("StimulsoftReports/OpenTicketsDetailByMechanicSubReport.mrt");

            StiReport report = new StiReport();
            report.Load(reportPathStimul);
            //report.Compile();

            //Get data
            DateTime StartDate = Convert.ToDateTime((Request.QueryString["StartDate"]));
            DateTime EndDate = Convert.ToDateTime((Request.QueryString["EndDate"]));
            string Worker = Request.QueryString["Worker"].ToString().Trim();

            DataSet dsGetMechanics = new DataSet();
            objPropUser.DBName = Session["dbname"].ToString();
            objPropUser.ConnConfig = Session["config"].ToString();
            objMapData.ConnConfig = Session["config"].ToString();
            objMapData.FromDate = StartDate.ToString();
            objMapData.Todate = EndDate.ToString();
            dsGetMechanics = objBL_MapData.GetOpenTicketsByMechanicReport(objMapData);
            DataSet dsGetCategories = objBL_MapData.GetCategories(objMapData);

            DataSet dsGetTicketDetails = new DataSet();
            objMapData.FromDate = StartDate.ToString();
            objMapData.Todate = EndDate.ToString();
            objMapData.Worker = Worker.ToString();
            DataSet dsCategories = objBL_MapData.GetCategoriesByMechanic(objMapData);
            for (int i = dsCategories.Tables[0].Rows.Count - 1; i >= 0; i--)
            {
                DataRow dr1 = dsCategories.Tables[0].Rows[i];
                if (dr1["Category"].ToString().Trim() == "Recurring")
                    dr1.Delete();
            }
            dsCategories.AcceptChanges();
            DataSet dsCombinedTicketData = new DataSet();
            DataTable dtCombined = new DataTable();
            dsCombinedTicketData.Tables.Add(dtCombined);
            for (int j = 0; j < dsCategories.Tables[0].Rows.Count; j++)
            {
                objMapData.Category = dsCategories.Tables[0].Rows[j]["Category"].ToString();
                dsGetTicketDetails = objBL_MapData.GetTicketDetailsByMechanic(objMapData);
                dsCombinedTicketData.Tables[0].Merge(dsGetTicketDetails.Tables[0]);
            }

            objMapData.Category = "Recurring";
            DataSet dsGetRecurringTicketDetails = objBL_MapData.GetTicketDetailsByMechanic(objMapData);

            DataSet dsGetEquipDetails = new DataSet();
            DataTable dtCombinedEquip = new DataTable();
            dsGetEquipDetails.Tables.Add(dtCombinedEquip);

            objPropUser.ConnConfig = Session["config"].ToString();
            objPropUser.EquipID = 0;
            objPropUser.EmName = Worker;

            DataSet dsGetEquipDetail = objBL_User.getMCPTemplatesByMechanic(objPropUser);
            if (dsGetEquipDetail.Tables[0].Rows.Count > 0)
            {
                dsGetEquipDetails.Tables[0].Merge(dsGetEquipDetail.Tables[0]);
            }

            DataTable sTable = BuildTicketDetailsTable();
            DataRow dr = sTable.NewRow();
            DataView dv = dsGetMechanics.Tables[0].DefaultView;
            dv.RowFilter = "MechanicName = '" + Worker.Trim() + "'";
            var filteredTable = dv.ToTable();
            for (int i = 0; i < filteredTable.Rows.Count; i++)
            {
                var newMechanicName = "";
                var oldMechanicName = filteredTable.Rows[i]["MechanicName"].ToString();
                if (i + 1 < filteredTable.Rows.Count)
                    newMechanicName = filteredTable.Rows[i + 1]["MechanicName"].ToString();

                dr["Mechanic Name"] = filteredTable.Rows[i]["MechanicName"];
                dr["Department"] = filteredTable.Rows[i]["Department"];
                if ((string.IsNullOrEmpty(filteredTable.Rows[i]["Category"].ToString()) || filteredTable.Rows[i]["Category"].ToString().Equals("None")))
                {
                    var prevNoneValue = dr["None"].ToString();
                    var noneVal = 0;
                    if (!string.IsNullOrEmpty(prevNoneValue))
                    {
                        noneVal = Convert.ToInt32(prevNoneValue) + Convert.ToInt32(filteredTable.Rows[i]["TicketCount"].ToString());
                    }
                    else
                        noneVal += Convert.ToInt32(filteredTable.Rows[i]["TicketCount"].ToString());
                    dr["None"] = noneVal;
                }
                for (int j = 0; j < dsGetCategories.Tables[0].Rows.Count; j++)
                {
                    if (filteredTable.Rows[i]["Category"].ToString().Trim().Equals(dsGetCategories.Tables[0].Rows[j]["Category"].ToString().Trim()) && filteredTable.Rows[i]["MechanicName"].ToString().Trim().Equals(oldMechanicName))
                    {
                        var prevValue = dr[dsGetCategories.Tables[0].Rows[j]["Category"].ToString().Trim()].ToString();
                        var val = 0;
                        if (!string.IsNullOrEmpty(prevValue))
                        {
                            val = Convert.ToInt32(prevValue) + Convert.ToInt32(filteredTable.Rows[i]["TicketCount"].ToString());
                        }
                        else
                            val += Convert.ToInt32(filteredTable.Rows[i]["TicketCount"].ToString());
                        dr[dsGetCategories.Tables[0].Rows[j]["Category"].ToString().Trim()] = val;

                    }

                }
                var totalTickets = 0;
                if (i == filteredTable.Rows.Count - 1)
                {
                    for (int j = 0; j < dsGetCategories.Tables[0].Rows.Count; j++)
                    {
                        var tickets = dr[dsGetCategories.Tables[0].Rows[j]["Category"].ToString()].ToString();
                        if (!string.IsNullOrEmpty(tickets))
                            totalTickets += Convert.ToInt32(tickets);
                    }
                    var nocatTickets = dr["None"].ToString();
                    if (!string.IsNullOrEmpty(nocatTickets))
                        totalTickets += Convert.ToInt32(nocatTickets);
                    dr["Total"] = totalTickets;
                    sTable.Rows.Add(dr);
                }
            }

            foreach (DataRow row in sTable.Rows)
            {
                var mechanicName = row["Mechanic Name"].ToString();
                objMapData.Worker = mechanicName;
                objMapData.FromDate = StartDate.ToString();
                objMapData.Todate = EndDate.ToString();
                var dsCompletedTickets = objBL_MapData.GetCompletedTicketsByMechanicReport(objMapData);

                row["Completed Tickets"] = dsCompletedTickets.Tables[0].Rows.Count;
            }

            DataSet finalDs = new DataSet();
            sTable.TableName = "OpenTickets";
            finalDs.Tables.Add(sTable);
            finalDs.DataSetName = "OpenTickets";

            if (dsCombinedTicketData.Tables[0].Rows.Count > 0)
            {
                DataSet companyLogo = new DataSet();
                companyLogo = bL_Report.GetCompanyLogo(Session["config"].ToString());
                var imageString = companyLogo.Tables[0].Rows[0]["Logo"].ToString();
                byte[] barrImg = (byte[])(companyLogo.Tables[0].Rows[0]["Logo"]);
                string strfn = Convert.ToString(Server.MapPath(Request.ApplicationPath) + "/TempImages/" + DateTime.Now.ToFileTime().ToString());
                FileStream fs = new FileStream(strfn,
                                  FileMode.CreateNew, FileAccess.Write);
                fs.Write(barrImg, 0, barrImg.Length);
                fs.Flush();
                fs.Close();

                StiImage stiImage = report.GetComponents()["Image1"] as StiImage;
                stiImage.Image = System.Drawing.Image.FromFile(strfn);

                System.Uri uri = new Uri(@"D:\btnLoad.png");
                DataTable cTable = BuildCompanyDetailsTable();
                var cRow = cTable.NewRow();
                cRow["LogoURL"] = uri.AbsolutePath;
                cRow["CompanyName"] = "";
                cRow["CompanyAddress"] = "";
                cRow["ContactNo"] = "";
                cRow["Email"] = "";

                cTable.Rows.Add(cRow);

                DataSet companyDs = new DataSet();
                cTable.TableName = "CompanyDetails";
                companyDs.Tables.Add(cTable);
                companyDs.DataSetName = "CompanyDetails";

                dsCategories.DataSetName = "Categories";
                dsCategories.Tables[0].TableName = "Categories";

                dsCombinedTicketData.DataSetName = "TicketDetails";
                dsCombinedTicketData.Tables[0].TableName = "TicketDetails";

                dsGetRecurringTicketDetails.DataSetName = "RecurringTicketDetails";
                dsGetRecurringTicketDetails.Tables[0].TableName = "RecurringTicketDetails";

                if (dsGetEquipDetails.Tables[0].Rows.Count > 0)
                {
                    dsGetEquipDetails.DataSetName = "EquipmentDetails";
                    dsGetEquipDetails.Tables[0].TableName = "EquipmentDetails";
                }

                report.RegData("OpenTickets", finalDs);
                report.RegData("CompanyDetails", companyDs);

                report.RegData("Categories", dsCategories);
                report.RegData("TicketDetails", dsCombinedTicketData);
                report.RegData("RecurringTicketDetails", dsGetRecurringTicketDetails);
                report.RegData("EquipmentDetails", dsGetEquipDetails);

                StiPage page = report.Pages[0];

                StiText txt = report.GetComponentByName("ReportHeader") as StiText;

                //Create TitleBand
                StiHeaderBand TitleBand = new StiHeaderBand();
                TitleBand.Height = 0.85;

                TitleBand.Name = "TitleBand";

                //Create Title text on header
                StiText headerText = new StiText(new RectangleD(0, 0, page.Width, 0.85));
                headerText.HorAlignment = StiTextHorAlignment.Center;
                headerText.VertAlignment = StiVertAlignment.Top;
                headerText.Name = "TitleHeader";
                headerText.Font = new System.Drawing.Font("Arial", 16F, System.Drawing.FontStyle.Bold);
                headerText.TextBrush = new StiSolidBrush(Color.White);
                headerText.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));

                //Create HeaderBand
                StiHeaderBand headerBand = new StiHeaderBand();
                headerBand.Height = 0.5;
                headerBand.Name = "HeaderBand";
                headerBand.Border.Style = StiPenStyle.None;
                StiReportTitleBand panel = (StiReportTitleBand)report.GetComponentByName("ReportTitleBand1");
                panel.Components.Add(headerBand);

                //Create Databand
                StiDataBand dataBand = new StiDataBand();
                dataBand.DataSourceName = "OpenTickets";
                dataBand.Height = 0.3;
                dataBand.Name = "OpenTickets";
                dataBand.Border.Style = StiPenStyle.None;

                StiDataSource dataSource = report.Dictionary.DataSources[0];

                double pos = 0;
                double columnWidth = page.Width / sTable.Columns.Count;
                double catWidth = 0.21;
                page.Orientation = StiPageOrientation.Landscape;
                panel.Components.Add(dataBand);
                List<StiDataBand> dataBands = new List<StiDataBand>();

                int nameIndex = 1;
                StiText hText, dataText;
                dataBand.ColumnGaps = 0;
                foreach (DataColumn dataColumn in sTable.Columns)
                {
                    if (dataColumn.ColumnName.Equals("Mechanic Name") || dataColumn.ColumnName.Equals("Department"))
                    {
                        columnWidth = StiAlignValue.AlignToMaxGrid(1.5, 0.1, true);
                        hText = new StiText(new RectangleD(pos, 0, columnWidth, 1.8));
                        hText.Angle = 0;

                    }
                    else if (dataColumn.ColumnName.Equals("Total") || dataColumn.ColumnName.Equals("Completed Tickets"))
                    {
                        columnWidth = StiAlignValue.AlignToMaxGrid(0.2, 0.1, true);
                        hText = new StiText(new RectangleD(pos, 0, columnWidth, 1.8));
                        hText.Angle = 90;
                    }
                    else
                    {
                        columnWidth = StiAlignValue.AlignToMaxGrid(catWidth, 0.1, true);
                        hText = new StiText(new RectangleD(pos, 0, columnWidth, 1.8));
                        hText.Angle = 90;
                    }

                    hText.Text.Value = dataColumn.ColumnName;
                    hText.HorAlignment = StiTextHorAlignment.Center;
                    hText.VertAlignment = StiVertAlignment.Center;

                    hText.Name = "HeaderText" + nameIndex.ToString();
                    hText.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
                    hText.TextBrush = new StiSolidBrush(Color.White);
                    hText.Border.Side = StiBorderSides.All;
                    hText.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
                    hText.Border.Style = StiPenStyle.None;
                    hText.WordWrap = true;
                    headerBand.Components.Add(hText);
                    if (dataColumn.ColumnName.Equals("Mechanic Name") || dataColumn.ColumnName.Equals("Department"))
                    {
                        columnWidth = StiAlignValue.AlignToMaxGrid(1.5, 0.1, true);
                        dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.5));
                    }
                    else if (dataColumn.ColumnName.Equals("Total") || dataColumn.ColumnName.Equals("Completed Tickets"))
                    {
                        columnWidth = StiAlignValue.AlignToMaxGrid(0.2, 0.1, true);
                        dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.5));
                    }
                    else
                    {
                        columnWidth = StiAlignValue.AlignToMaxGrid(catWidth, 0.1, true);
                        dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.5));
                    }
                    if (dataColumn.ColumnName.Equals("Total") || dataColumn.ColumnName.Equals("Completed Tickets"))
                    {
                        columnWidth = StiAlignValue.AlignToMaxGrid(0.2, 0.1, true);
                        dataText.Width = columnWidth;
                    }
                    else if (dataColumn.ColumnName.Equals("Mechanic Name") || dataColumn.ColumnName.Equals("Department"))
                    {
                        columnWidth = StiAlignValue.AlignToMaxGrid(1.5, 0.1, true);
                        dataText.Width = columnWidth;
                    }
                    else
                    {
                        columnWidth = StiAlignValue.AlignToMaxGrid(catWidth, 0.1, true);
                        dataText.Width = columnWidth;
                    }
                    dataText.Text.Value = "{OpenTickets." + Stimulsoft.Report.CodeDom.StiCodeDomSerializator.ReplaceSymbols(dataColumn.ColumnName) + "}";
                    dataText.HorAlignment = StiTextHorAlignment.Center;
                    dataText.VertAlignment = StiVertAlignment.Center;
                    dataText.WordWrap = true;
                    dataText.Name = "DataText" + nameIndex.ToString();
                    dataText.Border.Style = StiPenStyle.Solid;
                    dataText.OnlyText = false;
                    dataText.Border.Side = StiBorderSides.Left;
                    dataText.Border.Side = StiBorderSides.Right;
                    dataText.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
                    dataText.Border = new StiBorder(StiBorderSides.All, Color.Black, 1, StiPenStyle.Solid, false, 1,
                                new StiSolidBrush(Color.Black), false);
                    dataText.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);

                    dataBand.Components.Add(dataText);

                    if (dataColumn.ColumnName.Equals("Total") || dataColumn.ColumnName.Equals("Completed Tickets"))
                    {
                        columnWidth = StiAlignValue.AlignToMaxGrid(0.2, 0.1, true);
                        pos = pos + columnWidth;
                    }
                    else if (dataColumn.ColumnName.Equals("Mechanic Name") || dataColumn.ColumnName.Equals("Department"))
                    {
                        columnWidth = StiAlignValue.AlignToMaxGrid(1.5, 0.1, true);
                        pos = pos + columnWidth;
                    }
                    else
                    {
                        columnWidth = StiAlignValue.AlignToMaxGrid(catWidth, 0.1, true);
                        pos = pos + columnWidth;
                    }
                    nameIndex++;
                }
            }

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

                mail.Title = "Open Tickets Details By Mechanic Report";
                if (txtBody.Text.Trim() != string.Empty)
                {
                    mail.Text = txtBody.Text.Replace(Environment.NewLine, "<BR/>");
                }
                else
                {
                    mail.Text = "This is report email sent from Mobile Office Manager. Please find the Open Tickets Details By Mechanic Report attached.";
                }

                byte[] buffer1 = null;

                var settings = new Stimulsoft.Report.Export.StiPdfExportSettings();
                var service = new Stimulsoft.Report.Export.StiPdfExportService();
                System.IO.MemoryStream stream = new System.IO.MemoryStream();
                service.ExportTo(LoadReport(), stream, settings);
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
                        if (strpath != "OpenTicketsDetailsByMechanic.pdf")
                        {
                            mail.AttachmentFiles.Add(strpath);
                        }
                    }
                }

                mail.FileName = "OpenTicketsDetailsByMechanic.pdf";

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
            if (DownloadFileName == "OpenTicketsDetailsByMechanic.pdf")
            {
                byte[] buffer1 = null;

                var settings = new Stimulsoft.Report.Export.StiPdfExportSettings();
                var service = new Stimulsoft.Report.Export.StiPdfExportService();
                System.IO.MemoryStream stream = new System.IO.MemoryStream();
                service.ExportTo(LoadReport(), stream, settings);
                buffer1 = stream.ToArray();

                Response.Clear();
                MemoryStream ms = new MemoryStream(buffer1);
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=OpenTicketsDetailsByMechanic.pdf");
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

        string mailBody = "Please review the attached Open Tickets Details By Mechanic Report.";
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
}