using BusinessLayer;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dictionary;
using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.IO;

public partial class ReportDesignerLocations : System.Web.UI.Page
{
    BL_Report bL_Report = new BL_Report();
    BL_ReportsData objBL_ReportsData = new BL_ReportsData();
    BusinessEntity.User objProp_User = new BusinessEntity.User();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            #region Report Design Button Hide
            if (Convert.ToString(Session["type"]) == "am")
            {
                StiWebViewerLocations.ShowDesignButton = true;
            }
            else
            {
                StiWebViewerLocations.ShowDesignButton = false;
            }


            #endregion

            if (Request.QueryString["ReportType"] != null)
            {
                drpReportType.SelectedValue = Request.QueryString["ReportType"].ToString();
            }


            ArrayList columnsToRemove = (ArrayList)Session["UnSelecetdColumns"];
            if (columnsToRemove != null)
            {
                for (int i = 0; i < FromColumn.Items.Count; i++)
                {
                    bool addItem = true;
                    for (int j = 0; j < columnsToRemove.Count; j++)
                    {
                        if (FromColumn.Items[i].Text.ToString().Trim().Equals(columnsToRemove[j].ToString().Trim()))
                            addItem = false;
                    }
                    if (addItem)
                        ToColumn.Items.Add(FromColumn.Items[i].Text.ToString());
                }
            }
        }
    }

    protected void lnkClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("home.aspx");
    }

    protected void StiWebViewerLocations_GetReport(object sender, Stimulsoft.Report.Web.StiReportDataEventArgs e)
    {
        var reportType = "";
        if (Request.QueryString["ReportType"] != null)
            reportType = Request.QueryString["ReportType"].ToString();

        string reportPath = Server.MapPath("StimulsoftReports/LocationReport.mrt");

        StiReport report = new StiReport();
        report.Load(reportPath);
        report.Compile();


        e.Report = report;
        DataSet dsGetCustDetails = new DataSet();
        DataSet dsGetAccountSummaryListing = new DataSet();
        objProp_User.DBName = Session["dbname"].ToString();
        objProp_User.ConnConfig = Session["config"].ToString();
        dsGetCustDetails = objBL_ReportsData.GetLocationReport(objProp_User);

        DataTable locations = dsGetCustDetails.Tables[0];
        locations.TableName = "Locations";
        dsGetCustDetails.DataSetName = "Locations";
        ArrayList columnsToRemove = (ArrayList)Session["UnSelecetdColumns"];
        if (columnsToRemove != null)
        {
            for (int i = 0; i < columnsToRemove.Count; i++)
            {
                if (dsGetCustDetails.Tables[0].Columns.Contains(columnsToRemove[i].ToString().Trim()))
                    dsGetCustDetails.Tables[0].Columns.Remove(columnsToRemove[i].ToString().Trim());

            }
            if (dsGetCustDetails.Tables[0].Columns.Contains("Acct#"))
                dsGetCustDetails.Tables[0].Columns.Remove("Acct#");
        }


        report.RegData("Locations", dsGetCustDetails);
        report.Dictionary.Synchronize();
        //}
        //else 
        if (reportType == "2")
        {
            reportPath = Server.MapPath("StimulsoftReports/CustomReport.mrt");

            report = new StiReport();
            report.Load(reportPath);
            report.Compile();
            report["PrintedBy"] = "Tester";
            report["PrintedDate"] = DateTime.Now.ToString("MM/dd/yyyy");


            e.Report = report;
            dsGetCustDetails = new DataSet();
            dsGetAccountSummaryListing = new DataSet();
            objProp_User.DBName = Session["dbname"].ToString();
            objProp_User.ConnConfig = Session["config"].ToString();
            dsGetCustDetails = objBL_ReportsData.GetLocationReport(objProp_User);

            locations = dsGetCustDetails.Tables[0];
            locations.TableName = "Locations";
            dsGetCustDetails.DataSetName = "Locations";

            columnsToRemove = (ArrayList)Session["UnSelecetdColumns"];
            if (columnsToRemove != null)
            {
                for (int i = 0; i < columnsToRemove.Count; i++)
                {
                    if (dsGetCustDetails.Tables[0].Columns.Contains(columnsToRemove[i].ToString().Trim()))
                        dsGetCustDetails.Tables[0].Columns.Remove(columnsToRemove[i].ToString().Trim());
                }
                if (dsGetCustDetails.Tables[0].Columns.Contains("Acct#"))
                    dsGetCustDetails.Tables[0].Columns.Remove("Acct#");
            }

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

            report.RegData("Locations", dsGetCustDetails);
            report.Dictionary.Synchronize();
            StiPage page = report.Pages[0];


            StiText txt = report.GetComponentByName("ReportHeader") as StiText;


            //Create TitleBand
            StiHeaderBand TitleBand = new StiHeaderBand();
            TitleBand.Height = 0.85;

            TitleBand.Name = "TitleBand";
            //page.Components.Add(TitleBand);

            //Create Title text on header
            StiText headerText = new StiText(new RectangleD(0, 0, page.Width, 0.85));
            headerText.Text = "Locations Report";
            headerText.HorAlignment = StiTextHorAlignment.Center;
            headerText.VertAlignment = StiVertAlignment.Center;
            headerText.Name = "TitleHeader";
            headerText.Font = new System.Drawing.Font("Arial", 16F, System.Drawing.FontStyle.Bold);
            headerText.TextBrush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
            headerText.Brush = new StiSolidBrush(Color.WhiteSmoke);
            //TitleBand.Components.Add(headerText);

            //Create HeaderBand
            StiHeaderBand headerBand = new StiHeaderBand();
            headerBand.Height = 0.5;
            headerBand.Name = "HeaderBand";
            headerBand.Border.Style = StiPenStyle.None;
            page.Components.Add(headerBand);

            //Create Databand
            StiDataBand dataBand = new StiDataBand();
            dataBand.DataSourceName = "Locations";
            dataBand.Height = 0.3;
            dataBand.Name = "Locations";
            dataBand.Border.Style = StiPenStyle.None;
            page.Components.Add(dataBand);
            StiDataSource dataSource = report.Dictionary.DataSources[0];

            if (dsGetCustDetails.Tables[0].Columns.Count > 5 && dsGetCustDetails.Tables[0].Columns.Count <= 10)
            {
                page.Width = 18;
                page.Orientation = StiPageOrientation.Landscape;
            }

            if (dsGetCustDetails.Tables[0].Columns.Count > 10 && dsGetCustDetails.Tables[0].Columns.Count <= 15)
            {
                page.Width = 22;
                page.Orientation = StiPageOrientation.Landscape;
            }

            if (dsGetCustDetails.Tables[0].Columns.Count > 15)
            {
                page.Width = 28;
                page.Orientation = StiPageOrientation.Landscape;
            }


            double pos = 0;
            double columnWidth = page.Width / dsGetCustDetails.Tables[0].Columns.Count;
            int nameIndex = 1;
            foreach (DataColumn dataColumn in dsGetCustDetails.Tables[0].Columns)
            {
                //Create text on header
                StiText hText = new StiText(new RectangleD(pos, 0, columnWidth, 0.5));
                hText = new StiText(new RectangleD(pos, 0, columnWidth, 0.5));

                hText.Text.Value = dataColumn.ColumnName;
                hText.HorAlignment = StiTextHorAlignment.Left;
                hText.VertAlignment = StiVertAlignment.Center;
                hText.Name = "HeaderText" + nameIndex.ToString();
                hText.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
                hText.Border.Side = StiBorderSides.All;
                hText.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Bold);
                hText.Border.Style = StiPenStyle.None;
                hText.TextBrush = new StiSolidBrush(Color.White);
                headerBand.Components.Add(hText);

                StiText dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.2));

                dataText.Text.Value = "{Locations." + Stimulsoft.Report.CodeDom.StiCodeDomSerializator.ReplaceSymbols(dataColumn.ColumnName) + "}";
                dataText.HorAlignment = StiTextHorAlignment.Left;
                dataText.VertAlignment = StiVertAlignment.Center;
                dataText.Name = "DataText" + nameIndex.ToString();
                dataText.Border.Style = StiPenStyle.None;
                dataText.OnlyText = false;
                //dataText.ProcessingDuplicates = StiProcessingDuplicatesType.RemoveText;
                dataText.Border.Side = StiBorderSides.All;
                dataText.Font = new System.Drawing.Font("Arial", 9F);
                //dataBand.MinRowsInColumn = 6;

                dataBand.Components.Add(dataText);

                pos = pos + columnWidth;
                nameIndex++;
            }
            //page.Components.Add(headerBand);
            //page.Components.Add(dataBand);
            //report.RegData("Revenues", RevenuesDataSet);
            //report.Dictionary.Synchronize();

        }
    }

    protected void StiWebViewerLocations_GetReportData(object sender, Stimulsoft.Report.Web.StiReportDataEventArgs e)
    {
        var reportType = "";
        if (Request.QueryString["ReportType"] != null)
            reportType = Request.QueryString["ReportType"].ToString();

        string reportPath = Server.MapPath("StimulsoftReports/LocationReport.mrt");

        StiReport report = new StiReport();
        report.Load(reportPath);
        report.Compile();


        e.Report = report;
        DataSet dsGetCustDetails = new DataSet();
        DataSet dsGetAccountSummaryListing = new DataSet();
        objProp_User.DBName = Session["dbname"].ToString();
        objProp_User.ConnConfig = Session["config"].ToString();
        dsGetCustDetails = objBL_ReportsData.GetLocationReport(objProp_User);

        DataTable locations = dsGetCustDetails.Tables[0];
        locations.TableName = "Locations";
        dsGetCustDetails.DataSetName = "Locations";
        ArrayList columnsToRemove = (ArrayList)Session["UnSelecetdColumns"];
        if (columnsToRemove != null)
        {
            for (int i = 0; i < columnsToRemove.Count; i++)
            {
                if (dsGetCustDetails.Tables[0].Columns.Contains(columnsToRemove[i].ToString().Trim()))
                    dsGetCustDetails.Tables[0].Columns.Remove(columnsToRemove[i].ToString().Trim());

            }
            if (dsGetCustDetails.Tables[0].Columns.Contains("Acct#"))
                dsGetCustDetails.Tables[0].Columns.Remove("Acct#");
        }


        report.RegData("Locations", dsGetCustDetails);
        report.Dictionary.Synchronize();
        //}
        //else 
        if (reportType == "2")
        {
            reportPath = Server.MapPath("StimulsoftReports/CustomReport.mrt");

            report = new StiReport();
            report.Load(reportPath);
            report.Compile();
            report["PrintedBy"] = "Tester";
            report["PrintedDate"] = DateTime.Now.ToString("MM/dd/yyyy");


            e.Report = report;
            dsGetCustDetails = new DataSet();
            dsGetAccountSummaryListing = new DataSet();
            objProp_User.DBName = Session["dbname"].ToString();
            objProp_User.ConnConfig = Session["config"].ToString();
            dsGetCustDetails = objBL_ReportsData.GetLocationReport(objProp_User);

            locations = dsGetCustDetails.Tables[0];
            locations.TableName = "Locations";
            dsGetCustDetails.DataSetName = "Locations";

            columnsToRemove = (ArrayList)Session["UnSelecetdColumns"];
            if (columnsToRemove != null)
            {
                for (int i = 0; i < columnsToRemove.Count; i++)
                {
                    if (dsGetCustDetails.Tables[0].Columns.Contains(columnsToRemove[i].ToString().Trim()))
                        dsGetCustDetails.Tables[0].Columns.Remove(columnsToRemove[i].ToString().Trim());

                }
                if (dsGetCustDetails.Tables[0].Columns.Contains("Acct#"))
                    dsGetCustDetails.Tables[0].Columns.Remove("Acct#");
            }

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

            report.RegData("Locations", dsGetCustDetails);
            report.Dictionary.Synchronize();
            StiPage page = report.Pages[0];


            StiText txt = report.GetComponentByName("ReportHeader") as StiText;


            //Create TitleBand
            StiHeaderBand TitleBand = new StiHeaderBand();
            TitleBand.Height = 0.85;

            TitleBand.Name = "TitleBand";
            //page.Components.Add(TitleBand);

            //Create Title text on header
            StiText headerText = new StiText(new RectangleD(0, 0, page.Width, 0.85));
            headerText.Text = "Locations Report";
            headerText.HorAlignment = StiTextHorAlignment.Center;
            headerText.VertAlignment = StiVertAlignment.Center;
            headerText.Name = "TitleHeader";
            headerText.Font = new System.Drawing.Font("Arial", 16F, System.Drawing.FontStyle.Bold);
            headerText.TextBrush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
            headerText.Brush = new StiSolidBrush(Color.WhiteSmoke);
            //TitleBand.Components.Add(headerText);

            //Create HeaderBand
            StiHeaderBand headerBand = new StiHeaderBand();
            headerBand.Height = 0.5;
            headerBand.Name = "HeaderBand";
            headerBand.Border.Style = StiPenStyle.None;
            page.Components.Add(headerBand);

            //Create Databand
            StiDataBand dataBand = new StiDataBand();
            dataBand.DataSourceName = "Locations";
            dataBand.Height = 0.3;
            dataBand.Name = "Locations";
            dataBand.Border.Style = StiPenStyle.None;
            page.Components.Add(dataBand);
            StiDataSource dataSource = report.Dictionary.DataSources[0];

            if (dsGetCustDetails.Tables[0].Columns.Count > 5 && dsGetCustDetails.Tables[0].Columns.Count <= 10)
            {
                page.Width = 18;
                page.Orientation = StiPageOrientation.Landscape;
            }

            if (dsGetCustDetails.Tables[0].Columns.Count > 10 && dsGetCustDetails.Tables[0].Columns.Count <= 15)
            {
                page.Width = 22;
                page.Orientation = StiPageOrientation.Landscape;
            }

            if (dsGetCustDetails.Tables[0].Columns.Count > 15)
            {
                page.Width = 28;
                page.Orientation = StiPageOrientation.Landscape;
            }


            double pos = 0;
            double columnWidth = page.Width / dsGetCustDetails.Tables[0].Columns.Count;
            int nameIndex = 1;
            foreach (DataColumn dataColumn in dsGetCustDetails.Tables[0].Columns)
            {
                //Create text on header
                StiText hText = new StiText(new RectangleD(pos, 0, columnWidth, 0.5));
                hText = new StiText(new RectangleD(pos, 0, columnWidth, 0.5));

                hText.Text.Value = dataColumn.ColumnName;
                hText.HorAlignment = StiTextHorAlignment.Left;
                hText.VertAlignment = StiVertAlignment.Center;
                hText.Name = "HeaderText" + nameIndex.ToString();
                hText.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
                hText.Border.Side = StiBorderSides.All;
                hText.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Bold);
                hText.Border.Style = StiPenStyle.None;
                hText.TextBrush = new StiSolidBrush(Color.White);
                headerBand.Components.Add(hText);

                StiText dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.2));

                dataText.Text.Value = "{Locations." + Stimulsoft.Report.CodeDom.StiCodeDomSerializator.ReplaceSymbols(dataColumn.ColumnName) + "}";
                dataText.HorAlignment = StiTextHorAlignment.Left;
                dataText.VertAlignment = StiVertAlignment.Center;
                dataText.Name = "DataText" + nameIndex.ToString();
                dataText.Border.Style = StiPenStyle.None;
                dataText.OnlyText = false;
                //dataText.ProcessingDuplicates = StiProcessingDuplicatesType.RemoveText;
                dataText.Border.Side = StiBorderSides.All;
                dataText.Font = new System.Drawing.Font("Arial", 9F);
                //dataBand.MinRowsInColumn = 6;

                dataBand.Components.Add(dataText);

                pos = pos + columnWidth;
                nameIndex++;
            }
            //page.Components.Add(headerBand);
            //page.Components.Add(dataBand);
            //report.RegData("Revenues", RevenuesDataSet);
            //report.Dictionary.Synchronize();

        }
    }

    protected void lnkSearch_Click(object sender, EventArgs e)
    {
        ArrayList selectedItems = new ArrayList();
        for (int i = 0; i < FromColumn.Items.Count; i++)
        {
            selectedItems.Add(FromColumn.Items[i].Value.Trim());

        }
        Session["UnSelecetdColumns"] = selectedItems;
        Response.Redirect("ReportDesignerLocations.aspx?ReportType=2");
    }

    protected void drpReportType_SelectedIndexChanged(object sender, EventArgs e)
    {
        Response.Redirect("ReportDesignerLocations.aspx?ReportType=" + drpReportType.SelectedValue);
    }
}