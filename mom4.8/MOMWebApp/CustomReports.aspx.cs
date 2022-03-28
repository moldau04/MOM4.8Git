using BusinessLayer;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dictionary;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class CustomReports : System.Web.UI.Page
{
    BL_Report bL_Report = new BL_Report();
    BL_ReportsData objBL_ReportsData = new BL_ReportsData();
    BusinessEntity.User objProp_User = new BusinessEntity.User();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (Request.QueryString["Generate"] != null)
            {
                if (Request.QueryString["Generate"].ToString() == "1")
                {
                    StiWebViewerCustomReport.Visible = true;
                    drpSelectedColumns.Visible = true;
                    drpSelectedColumns.DataSource = Session["SelectedColumns"];
                    drpSelectedColumns.DataBind();
                    drpSelectedColumns.Items.Insert(0, new Telerik.Web.UI.DropDownListItem("Select Criteria"));

                }
            }
            lblSelect.Style.Add("Display", "None");
        }


        if (Session["SelectedColumns"] != null && !IsPostBack)
        {
            var moduleName = drpModule.SelectedItem.Text.Trim();
            if (Session["Module"] != null)
            {
                drpModule.ClearSelection();
                moduleName = Session["Module"].ToString();
                drpModule.FindItemByText(moduleName).Selected = true;
            }
            else
                moduleName = "Locations";
            DataSet ds = bL_Report.GetChildTables(Session["config"].ToString(), moduleName);
            lstChild.Items.Clear();
            lstChild.DataSource = ds;
            lstChild.DataBind();
            lstChild.DataTextField = "TableName";
            lstChild.DataValueField = "ReportTableId";
            lstChild.DataBind();
            lstChild.Visible = true;
            lstColumns.Visible = true;
            var childTables = Session["ChildTables"].ToString();
            var cTables = childTables.Split(',');
            ds = new DataSet();
            DataSet ds1 = new DataSet();


            ds1 = bL_Report.GetReportColumns(Session["config"].ToString(), "Locations", moduleName);
            ds = ds1.Clone();
            ds.Merge(ds1);
            for (int i = 0; i < cTables.Length; i++)
            {
                if (lstChild.FindItemByText(cTables[i].ToString().Trim()) != null)
                    lstChild.FindItemByText(cTables[i].ToString().Trim()).Selected = true;


                string reportTable = cTables[i];


                ds1 = bL_Report.GetReportColumns(Session["config"].ToString(), reportTable, moduleName);
                ds.Merge(ds1);


            }
            lstColumns.Items.Clear();
            lstColumns.DataSource = ds;
            lstColumns.DataBind();
            lstColumns.DataTextField = "ColumnName";
            lstColumns.DataValueField = "ReportTableColumnId";
            lstColumns.DataBind();
            lstColumns.Visible = true;
            ArrayList selectedColumns = (ArrayList)Session["SelectedColumns"];
            for (int i = 0; i < selectedColumns.Count; i++)
            {
                if (lstColumns.FindItemByText(selectedColumns[i].ToString().Trim()) != null)
                    lstColumns.FindItemByText(selectedColumns[i].ToString().Trim()).Selected = true;
            }
            if (lstColumns.Visible)
            {
                ArrayList conditions = new ArrayList();
                if (Session["FilterConditions"] != null)
                {
                    conditions = (ArrayList)Session["FilterConditions"];
                    List<string> lConditions = new List<string>();
                    for (int i = 0; i < conditions.Count; i++)
                    {
                        lConditions.Add(conditions[i].ToString().Replace(",", " "));
                    }
                    if (conditions.Count > 0 && !IsPostBack)
                    {
                        lstMemo.DataSource = lConditions;
                        lstMemo.DataBind();
                        lstMemo.Visible = true;
                        lnkDelete.Visible = true;
                    }
                }
                else
                    conditions.Add("Test");
                ConditionsRepeater.DataSource = conditions;
                ConditionsRepeater.DataBind();
                ConditionsRepeater.Visible = false;
            }

        }
        CriteriaPanel.Visible = lstColumns.Visible;
    }
    protected void drpModule_SelectedIndexChanged(object sender, Telerik.Web.UI.DropDownListEventArgs e)
    {
        string module = drpModule.SelectedText;
        DataSet ds = bL_Report.GetChildTables(Session["config"].ToString(), module);
        lstChild.Items.Clear();
        lstChild.DataSource = ds;
        lstChild.DataBind();
        lstChild.DataTextField = "TableName";
        lstChild.DataValueField = "ReportTableId";
        lstChild.DataBind();
        lstChild.Visible = true;
    }

    protected void lstChild_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataSet ds = new DataSet();
        DataSet ds1 = new DataSet();
        Session["Module"] = drpModule.SelectedText.Trim();
        string reportTable = drpModule.SelectedText;
        ds1 = bL_Report.GetReportColumns(Session["config"].ToString(), reportTable, drpModule.SelectedItem.Text.Trim());
        ds = ds1.Clone();
        ds.Merge(ds1);
        var tables = "";
        for (int i = 0; i < lstChild.SelectedItems.Count; i++)
        {

            reportTable = lstChild.SelectedItems[i].Text;
            if (string.IsNullOrEmpty(tables))
                tables += reportTable;
            else
                tables += "," + reportTable;
            Session["ChildTables"] = tables;
            ds1 = bL_Report.GetReportColumns(Session["config"].ToString(), reportTable, drpModule.SelectedItem.Text.Trim());
            ds.Merge(ds1);
        }



        lstColumns.Items.Clear();
        lstColumns.DataSource = ds;
        lstColumns.DataBind();
        lstColumns.DataTextField = "ColumnName";
        lstColumns.DataValueField = "ReportTableColumnId";
        lstColumns.DataBind();
        lstColumns.Visible = true;
        if (Session["SelectedColumns"] != null)
        {
            ArrayList selectedColumns = (ArrayList)Session["SelectedColumns"];
            for (int i = 0; i < selectedColumns.Count; i++)
            {
                if (lstColumns.FindItemByText(selectedColumns[i].ToString().Trim()) != null)
                    lstColumns.FindItemByText(selectedColumns[i].ToString().Trim()).Selected = true;
            }
        }
    }


    protected void lnkSearch_Click(object sender, EventArgs e)
    {
        ArrayList selectedItems = new ArrayList();
        if (lstColumns.SelectedItems.Count > 0)
        {
            for (int i = 0; i < lstColumns.SelectedItems.Count; i++)
            {
                selectedItems.Add(lstColumns.SelectedItems[i].Text.Trim());

            }
            Session["SelectedColumns"] = selectedItems;
        }
        ConditionsRepeater.Visible = false;
        drpSelectedColumns.Visible = true;
        if (Session["FilterConditions"] != null)
        {
            Session["FilterConditions"] = null;
            ArrayList conditions = new ArrayList();
            for (int i = 0; i < ConditionsRepeater.Items.Count; i++)
            {

                var drpSelectedColumns2 = (RadDropDownList)ConditionsRepeater.Items[i].FindControl("drpSelectedColumns");
                var drpFilter = (RadDropDownList)ConditionsRepeater.Items[i].FindControl("drpFilter");
                var txtfilterText = (TextBox)ConditionsRepeater.Items[i].FindControl("txtfilterText");

                var filterCondition = drpSelectedColumns2.SelectedItem.Text.Trim() + "," + drpFilter.SelectedItem.Text.Trim() + "," + txtfilterText.Text.Trim();
                if (!string.IsNullOrEmpty(txtfilterText.Text))
                    conditions.Add(filterCondition);

            }
            Session["FilterConditions"] = conditions;
        }
        Response.Redirect("CustomReports.aspx?Generate=1");

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

    protected DataTable BuildSelectedColumnsTable()
    {
        ArrayList selectedColumns = (ArrayList)Session["SelectedColumns"];
        DataTable sTable = new DataTable();
        if (selectedColumns != null)
        {
            for (int i = 0; i < selectedColumns.Count; i++)
            {
                sTable.Columns.Add(selectedColumns[i].ToString());
            }
            return sTable;
        }
        else
            return null;
    }

    protected void StiWebViewerCustomReport_GetReport(object sender, Stimulsoft.Report.Web.StiReportDataEventArgs e)
    {
        string reportPath = Server.MapPath("StimulsoftReports/CustomReport.mrt");

        StiReport report = new StiReport();
        report.Load(reportPath);
        //report.Compile();

        report["PrintedBy"] = "Tester";
        report["PrintedDate"] = DateTime.Now.ToString("MM/dd/yyyy");

        e.Report = report;
        DataSet dsGetCustDetails = new DataSet();
        DataSet dsGetAccountSummaryListing = new DataSet();
        objProp_User.DBName = Session["dbname"].ToString();
        objProp_User.ConnConfig = Session["config"].ToString();
        dsGetCustDetails = bL_Report.GetCustomColumns(Session["config"].ToString());

        DataTable sTable = BuildSelectedColumnsTable();
        if (sTable == null)
        {
            StiWebViewerCustomReport.Visible = false;
            return;
        }

        for (int i = 0; i < dsGetCustDetails.Tables[0].Rows.Count; i++)
        {
            DataRow dr = sTable.NewRow();
            var columns = (ArrayList)Session["SelectedColumns"];
            for (int j = 0; j < columns.Count; j++)
            {
                if(dsGetCustDetails.Tables[0].Columns.Contains(columns[j].ToString()))
                    dr[columns[j].ToString().Trim()] = dsGetCustDetails.Tables[0].Rows[i][columns[j].ToString().Trim()].ToString();
            }
            sTable.Rows.Add(dr);
        }
        DataTable fTable = new DataTable();
        if (Session["FilterConditions"] != null)
        {
            ArrayList filterCondition = (ArrayList)Session["FilterConditions"];
            DataView sView = new DataView();
            fTable = sTable.Clone();
            var rowFilter = string.Empty;
            if (filterCondition != null)
            {
                sView = sTable.DefaultView;
                for (int i = 0; i < filterCondition.Count; i++)
                {
                    string[] filtered = filterCondition[i].ToString().Split(',');
                   
                    if (i > 0 && i <= filterCondition.Count - 1)
                        rowFilter += " AND ";
                    else
                        rowFilter += " ";
                    if (filtered.Length == 3)
                    {
                        if (filtered[1] == "Contains")
                        {

                            rowFilter += filtered[0] + " LIKE " + "'%" + filtered[2] + "%'";
                        }
                        else
                        {
                            if (filtered[0].Trim().Contains("PrintInvoice") || filtered[0].Trim().Contains("EmailInvoice") || filtered[0].Trim().Contains("LocationPrice"))
                            {
                                if (filtered[2] == "1")
                                    filtered[2] = "True";
                                else
                                    filtered[2] = "False";
                                rowFilter += filtered[0] + "=" + "'" + filtered[2] + "'";
                            }
                            else if (!filtered[0].EndsWith("No") || !filtered[0].Equals("Balance") || !filtered[0].Equals("Status") || filtered[0].Trim().Contains("PrintInvoice"))
                                rowFilter += filtered[0] + "=" + "'" + filtered[2] + "'";
                            else
                            {
                                rowFilter += filtered[0] + "=" + filtered[2];
                            }
                        }
                    }
                }

            }
            sView.RowFilter = rowFilter;
            fTable = sView.ToTable();
        }
        else
        {
            fTable = sTable.Copy();
        }

        DataSet finalDs = new DataSet();
        fTable.TableName = "Locations";
        finalDs.Tables.Add(fTable);
        finalDs.DataSetName = "Locations";

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
        //System.Drawing.Image myImage = System.Drawing.Image.FromFile(@"F:\\ESS\\ESSMOM\\MOM\\MobileService2\\MSWeb\\images\\1PDFIcon.png");
        //System.Drawing.Image myImage = System.Drawing.Image.FromStream(ms);
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

        report.RegData("CompanyDetails", companyDs);
        report.RegData("Locations", finalDs);
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
        //headerText.Text = "Locations Report";
        headerText.HorAlignment = StiTextHorAlignment.Center;
        headerText.VertAlignment = StiVertAlignment.Top;
        headerText.Name = "TitleHeader";
        headerText.Font = new System.Drawing.Font("Arial", 16F, System.Drawing.FontStyle.Bold);
        headerText.TextBrush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
        headerText.Brush = new StiSolidBrush(Color.WhiteSmoke);

        // TitleBand.Components.Add(headerText);

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

        if (finalDs.Tables[0].Columns.Count > 5 && finalDs.Tables[0].Columns.Count <= 10)
        {
            page.Width = 18;
            page.Orientation = StiPageOrientation.Landscape;
        }

        if (finalDs.Tables[0].Columns.Count > 10 && finalDs.Tables[0].Columns.Count <= 15)
        {
            page.Width = 22;
            page.Orientation = StiPageOrientation.Landscape;
        }

        if (finalDs.Tables[0].Columns.Count > 15)
        {
            page.Width = 28;
            page.Orientation = StiPageOrientation.Landscape;
        }

        double pos = 0;
        double columnWidth = page.Width / finalDs.Tables[0].Columns.Count;
        int nameIndex = 1;
        foreach (DataColumn dataColumn in finalDs.Tables[0].Columns)
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
    }

    protected void StiWebViewerCustomReport_GetReportData(object sender, Stimulsoft.Report.Web.StiReportDataEventArgs e)
    {
        string reportPath = Server.MapPath("StimulsoftReports/CustomReport.mrt");

        StiReport report = new StiReport();
        report.Load(reportPath);
        //report.Compile();

        report["PrintedBy"] = "Tester";
        report["PrintedDate"] = DateTime.Now.ToString("MM/dd/yyyy");

        e.Report = report;
        DataSet dsGetCustDetails = new DataSet();
        DataSet dsGetAccountSummaryListing = new DataSet();
        objProp_User.DBName = Session["dbname"].ToString();
        objProp_User.ConnConfig = Session["config"].ToString();
        dsGetCustDetails = bL_Report.GetCustomColumns(Session["config"].ToString());

        DataTable sTable = BuildSelectedColumnsTable();
        if (sTable == null)
        {
            StiWebViewerCustomReport.Visible = false;
            return;
        }
        
        for (int i = 0; i < dsGetCustDetails.Tables[0].Rows.Count; i++)
        {
            DataRow dr = sTable.NewRow();
            var columns = (ArrayList)Session["SelectedColumns"];
            for (int j = 0; j < columns.Count; j++)
            {
                if (dsGetCustDetails.Tables[0].Columns.Contains(columns[j].ToString()))
                    dr[columns[j].ToString().Trim()] = dsGetCustDetails.Tables[0].Rows[i][columns[j].ToString().Trim()].ToString();
            }
            sTable.Rows.Add(dr);
        }
        DataTable fTable = new DataTable();
        if (Session["FilterConditions"] != null)
        {
            ArrayList filterCondition = (ArrayList)Session["FilterConditions"];
            DataView sView = new DataView();
            fTable = sTable.Clone();
            var rowFilter = string.Empty;
            if (filterCondition != null)
            {
                sView = sTable.DefaultView;
                for (int i = 0; i < filterCondition.Count; i++)
                {
                    string[] filtered = filterCondition[i].ToString().Split(',');
                    
                    if (i > 0 && i <= filterCondition.Count - 1)
                        rowFilter += " AND ";
                    else
                        rowFilter += " ";
                    if (filtered.Length == 3)
                    {
                        if (filtered[1] == "Contains")
                        {

                            rowFilter += filtered[0] + " LIKE " + "'%" + filtered[2] + "%'";
                        }
                        else
                        {
                            if (filtered[0].Trim().Contains("PrintInvoice") || filtered[0].Trim().Contains("EmailInvoice") || filtered[0].Trim().Contains("LocationPrice"))
                            {
                                if (filtered[2] == "1")
                                    filtered[2] = "True";
                                else
                                    filtered[2] = "False";
                                rowFilter += filtered[0] + "=" + "'" + filtered[2] + "'";
                            }
                            else if (!filtered[0].EndsWith("No") || !filtered[0].Equals("Balance") || !filtered[0].Equals("Status") )
                                rowFilter += filtered[0] + "=" + "'" + filtered[2] + "'";
                            else
                            {
                                rowFilter += filtered[0] + "=" + filtered[2];
                            }
                        }
                    }
                }
                sView.RowFilter = rowFilter;
                fTable = sView.ToTable();
            }
        }
        else
        {
            fTable = sTable.Copy();
        }

        DataSet finalDs = new DataSet();
        fTable.TableName = "Locations";
        finalDs.Tables.Add(fTable);
        finalDs.DataSetName = "Locations";

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
        //System.Drawing.Image myImage = System.Drawing.Image.FromFile(@"F:\\ESS\\ESSMOM\\MOM\\MobileService2\\MSWeb\\images\\1PDFIcon.png");
        //System.Drawing.Image myImage = System.Drawing.Image.FromStream(ms);
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

        report.RegData("CompanyDetails", companyDs);
        report.RegData("Locations", finalDs);
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
        //headerText.Text = "Locations Report";
        headerText.HorAlignment = StiTextHorAlignment.Center;
        headerText.VertAlignment = StiVertAlignment.Top;
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

        if (finalDs.Tables[0].Columns.Count > 5 && finalDs.Tables[0].Columns.Count <= 10)
        {
            page.Width = 18;
            page.Orientation = StiPageOrientation.Landscape;
        }

        if (finalDs.Tables[0].Columns.Count > 10 && finalDs.Tables[0].Columns.Count <= 15)
        {
            page.Width = 22;
            page.Orientation = StiPageOrientation.Landscape;
        }

        if (finalDs.Tables[0].Columns.Count > 15)
        {
            page.Width = 28;
            page.Orientation = StiPageOrientation.Landscape;
        }

        double pos = 0;
        double columnWidth = page.Width / finalDs.Tables[0].Columns.Count;
        int nameIndex = 1;
        foreach (DataColumn dataColumn in finalDs.Tables[0].Columns)
        {
            //Create text on header
            StiText hText = new StiText(new RectangleD(pos, 0, columnWidth, 0.5));
            hText = new StiText(new RectangleD(pos, 0, columnWidth, 0.5));

            hText.Text.Value = dataColumn.ColumnName;
            hText.HorAlignment = StiTextHorAlignment.Center;
            hText.VertAlignment = StiVertAlignment.Center;
            hText.Name = "HeaderText" + nameIndex.ToString();
            hText.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
            hText.Border.Side = StiBorderSides.All;
            hText.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            hText.Border.Style = StiPenStyle.None;
            headerBand.Components.Add(hText);

            StiText dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.5));

            dataText.Text.Value = "{Locations." + Stimulsoft.Report.CodeDom.StiCodeDomSerializator.ReplaceSymbols(dataColumn.ColumnName) + "}";
            dataText.HorAlignment = StiTextHorAlignment.Center;
            dataText.VertAlignment = StiVertAlignment.Center;
            dataText.Name = "DataText" + nameIndex.ToString();
            dataText.Border.Style = StiPenStyle.None;
            dataText.OnlyText = false;
            //dataText.ProcessingDuplicates = StiProcessingDuplicatesType.RemoveText;
            dataText.Border.Side = StiBorderSides.All;
            dataText.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            //dataBand.MinRowsInColumn = 6;

            dataBand.Components.Add(dataText);
            pos = pos + columnWidth;
            nameIndex++;
        }
    }

    protected void lnkClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("Locations.aspx");
    }
    protected void lstColumns_SelectedIndexChanged(object sender, EventArgs e)
    {
        //ArrayList selectedColumns = (ArrayList)Session["SelectedColumns"];
        //if (selectedColumns != null)
        //{
        //    for (int i = 0; i < selectedColumns.Count; i++)
        //    {
        //        if (lstColumns.FindItemByText(selectedColumns[i].ToString().Trim()) != null)
        //            lstColumns.FindItemByText(selectedColumns[i].ToString().Trim()).Selected = true;
        //    }
        //}
        if (Session["FilterConditions"] != null)
        {
            ArrayList conditions = (ArrayList)Session["FilterConditions"];
            for (int i = 0; i < conditions.Count; i++)
            {
                string[] filtered = conditions[i].ToString().Split(',');
                if (filtered.Length == 3)
                {
                    if (lstColumns.FindItemByText(filtered[0].Trim()) != null)
                        lstColumns.FindItemByText(filtered[0].Trim()).Selected = true;
                }
            }
        }
                lnkSearch.Visible = true;
    }

    protected void drpSelectedColumns_SelectedIndexChanged(object sender, Telerik.Web.UI.DropDownListEventArgs e)
    {
        drpFilter.Visible = true;
    }

    protected void drpFilter_SelectedIndexChanged(object sender, Telerik.Web.UI.DropDownListEventArgs e)
    {
        txtfilterText.Visible = true;
        lnkFilter.Visible = true;
    }

    protected void lnkFilter_Click(object sender, EventArgs e)
    {
        var filterColumn = drpSelectedColumns.SelectedItem.Text.Trim();
        var filterType = drpFilter.SelectedItem.Text.Trim();
        var filterValue = txtfilterText.Text.Trim();

        var filterCondition = filterColumn + "," + filterType + "," + filterValue;
        ArrayList conditions = new ArrayList();
        if (!filterCondition.Contains("Select One"))
        {
            if (Session["FilterConditions"] == null)
            {
                conditions.Add(filterCondition);
                Session.Add("FilterConditions", conditions);
            }
            else
            {
                conditions = (ArrayList)Session["FilterConditions"];
                if (!conditions.Contains(filterCondition))
                    conditions.Add(filterCondition);
                Session["FilterConditions"] = conditions;
            }
            Session["FilterConditions"] = conditions;
        }

        Response.Redirect("CustomReports.aspx?Generate=1");
    }

    protected void ConditionsRepeater_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == System.Web.UI.WebControls.ListItemType.Item ||
            e.Item.ItemType == System.Web.UI.WebControls.ListItemType.AlternatingItem)
        {
            var drpSelectedColumns = (RadDropDownList)e.Item.FindControl("drpSelectedColumns");
            var drpFilter = (RadDropDownList)e.Item.FindControl("drpFilter");
            var txtfilterText = (TextBox)e.Item.FindControl("txtfilterText");
            drpSelectedColumns.Visible = true;
            drpSelectedColumns.DataSource = Session["SelectedColumns"];
            drpSelectedColumns.DataBind();
            drpSelectedColumns.Items.Insert(0, new Telerik.Web.UI.DropDownListItem("Select Criteria"));

            if (Session["FilterConditions"] != null)
            {
                ArrayList conditions = (ArrayList)Session["FilterConditions"];

                string[] filtered = conditions[e.Item.ItemIndex].ToString().Split(',');
                if (filtered.Length == 3)
                {
                    if (drpSelectedColumns.FindItemByText(filtered[0].Trim()) != null)
                        drpSelectedColumns.FindItemByText(filtered[0].Trim()).Selected = true;
                    if (drpFilter.FindItemByText(filtered[1].Trim()) != null)
                        drpFilter.FindItemByText(filtered[1].Trim()).Selected = true;
                    txtfilterText.Text = filtered[2].Trim();
                }

            }
        }
    }

    protected void ConditionsRepeater_ItemCommand(object source, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
    {
        if (e.CommandName.Equals("Filter"))
        {
            var drpSelectedColumns = (RadDropDownList)e.Item.FindControl("drpSelectedColumns");
            var drpFilter = (RadDropDownList)e.Item.FindControl("drpFilter");
            var txtfilterText = (TextBox)e.Item.FindControl("txtfilterText");

            var filterCondition = drpSelectedColumns.SelectedItem.Text.Trim() + "," + drpFilter.SelectedItem.Text.Trim() + "," + txtfilterText.Text.Trim();
            ArrayList conditions = new ArrayList();

            if (Session["FilterConditions"] == null)
            {
                conditions.Add(filterCondition);
                Session.Add("FilterConditions", conditions);
            }
            else
            {
                conditions = (ArrayList)Session["FilterConditions"];
                conditions.Remove(";;");
                if (!conditions.Contains(filterCondition))
                    conditions.Add(filterCondition);
                Session["FilterConditions"] = conditions;
            }

            //if (ConditionsRepeater.Items.Count <= 1)
            conditions.Add(";;");
            ConditionsRepeater.DataSource = conditions;
            ConditionsRepeater.DataBind();
        }
    }

    protected void lnkDelete_Click(object sender, EventArgs e)
    {
        ArrayList conditions = new ArrayList();

        IList<RadListBoxItem> selectedItems;
        selectedItems = lstMemo.SelectedItems;

        if (lstMemo.SelectedIndex != -1)
        {
            for (int i = selectedItems.Count - 1; i >= 0; i--)
            {
                lstMemo.Items.Remove(selectedItems[i]);
                if (Session["FilterConditions"] != null)
                {
                    conditions = (ArrayList)Session["FilterConditions"];
                    conditions.Remove(selectedItems[i].Text.Replace(" ", ","));
                }
            }
        }

        Session["FilterConditions"] = conditions;

        Response.Redirect("CustomReports.aspx?Generate=1");
    }

    protected void lnkStartOver_Click(object sender, EventArgs e)
    {
        Session["FilterConditions"] = null;
        Session["SelectedColumns"] = null;
        lstChild.ClearSelection();
        lstColumns.ClearSelection();
        CriteriaPanel.Visible = lstMemo.Visible = lnkDelete.Visible = lstChild.Visible = lstColumns.Visible = false;
        drpModule.ClearSelection();
        drpModule.Visible = true;
        drpFilter.ClearSelection();
        StiWebViewerCustomReport.Visible = false;
    }
}