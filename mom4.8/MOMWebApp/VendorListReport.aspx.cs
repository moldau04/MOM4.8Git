﻿using System;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessEntity;
using BusinessLayer;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using System.Text;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;

public partial class VendorListReport : System.Web.UI.Page
{
    BL_ReportsData objBL_ReportsData = new BL_ReportsData();
    CustomerReport objCustReport = new CustomerReport();
    BL_User objBL_User = new BL_User();
    BusinessEntity.User objProp_User = new BusinessEntity.User();
    public static int pubReportId = 0;
    public static string sortBy = string.Empty;
    public static string getPrintData = string.Empty;
    string reportType;
    protected void Page_Load(object sender, EventArgs e)
    {
        reportType = Request.QueryString["type"];
    }
    protected void Page_Init(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                reportType = Request.QueryString["type"];
                DeleteExcelFiles();
                DeletePDFFiles();
            }
            catch
            {
                //
            }
            dvSaveReport.Attributes.Add("style", "display:none");
            if (Session["userid"] == null)
            {
                Response.Redirect("login.aspx");
            }

            if (Request.QueryString["reportId"] != null || Convert.ToInt32(Request.QueryString["reportId"]) != 0)
            {
                objCustReport.ReportId = Convert.ToInt32(Request.QueryString["reportId"]);
            }


            //else
            //{
            //    Response.Redirect("customers.aspx", false);
            //    return;
            //}
            if (Request.QueryString["reportName"] != null)
            {
                objCustReport.ReportName = Request.QueryString["reportName"];
                hdnCustomizeReportName.Value = objCustReport.ReportName;
            }
            pubReportId = objCustReport.ReportId;

            sortBy = string.Empty;
            GetCustomerDetails();
            GetReportsName();
            if (pubReportId != 0)
            {
                GetReportDetailByRptId();
                GetReportColumnsByRepId();
            }


            GetCustReportFiltersValue();
            ConvertToJSON();
            //GetUserEmail();
            txtFrom.Text = WebBaseUtility.GetFromEmailAddress();
            //GetPreviewFields();           
        }
        reportType = Request.QueryString["type"];
    }

    //private void GetUserEmail()
    //{
    //    objProp_User.ConnConfig = Session["config"].ToString();
    //    objProp_User.Username = Session["username"].ToString();
    //    txtFrom.Text = objBL_User.getUserEmail(objProp_User);

    //    DataSet dsC = new DataSet();

    //    dsC = objBL_User.getControl(objProp_User);

    //    if (dsC.Tables[0].Rows.Count > 0)
    //    {
    //        if (txtFrom.Text.Trim() == string.Empty)
    //        {
    //            if (Session["MSM"].ToString() != "TS")
    //            {
    //                txtFrom.Text = dsC.Tables[0].Rows[0]["Email"].ToString();
    //            }
    //        }
    //    }

    //    if (txtFrom.Text.Trim() == string.Empty)
    //    {
    //        System.Configuration.Configuration configurationFile = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration(Request.ApplicationPath);
    //        MailSettingsSectionGroup mailSettings = configurationFile.GetSectionGroup("system.net/mailSettings") as MailSettingsSectionGroup;
    //        string username = mailSettings.Smtp.Network.UserName;
    //        txtFrom.Text = username;
    //    }
    //}

    private void GetReportDetailByRptId()
    {
        try
        {
            DataSet dsGetRptDetails = new DataSet();
            objCustReport.DBName = Session["dbname"].ToString();
            objCustReport.ConnConfig = Session["config"].ToString();
            objCustReport.ReportId = pubReportId;
            if (pubReportId != 0)
            {
                dsGetRptDetails = objBL_ReportsData.GetReportDetailById(objCustReport);
                if (dsGetRptDetails.Tables.Count > 0)
                {
                    bool isGlobal = Convert.ToBoolean(dsGetRptDetails.Tables[0].Rows[0]["IsGlobal"]);
                    bool isAscending = Convert.ToBoolean(dsGetRptDetails.Tables[0].Rows[0]["IsAscendingOrder"]);

                    if (isGlobal)
                    {
                        chkIsGlobal.Checked = true;
                    }
                    else
                    {
                        chkIsGlobal.Checked = false;
                    }

                    if (isAscending)
                    {
                        rdbOrders.SelectedValue = "1";
                    }
                    else
                    {
                        rdbOrders.SelectedValue = "2";
                    }

                    hdnDrpSortBy.Value = dsGetRptDetails.Tables[0].Rows[0]["SortBy"].ToString();
                    hdnIsStock.Value = dsGetRptDetails.Tables[0].Rows[0]["IsStock"].ToString();
                    sortBy = dsGetRptDetails.Tables[0].Rows[0]["SortBy"].ToString() + " " + (isAscending == true ? "Asc" : "Desc");
                }
            }
        }
        catch (Exception exp)
        {
            ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: '" + exp.Message.ToString() + "',  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
    }

    private void GetCustomerType()
    {
        try
        {
            DataSet dsGetCustType = new DataSet();
            objCustReport.DBName = Session["dbname"].ToString();
            objCustReport.ConnConfig = Session["config"].ToString();
            dsGetCustType = objBL_ReportsData.GetCustomerType(objCustReport);
            if (dsGetCustType.Tables[0].Rows.Count > 0)
            {
                drpType.DataSource = dsGetCustType.Tables[0];
                drpType.DataTextField = "Type";
                drpType.DataValueField = "Type";
                drpType.DataBind();

            }
        }
        catch (Exception exp)
        {
            ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: '" + exp.Message.ToString() + "',  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
    }

    private void GetCustReportFiltersValue()
    {
        DataSet dsGetVendorReportFiltersValue = new DataSet();
        objProp_User.DBName = Session["dbname"].ToString();
        objProp_User.ConnConfig = Session["config"].ToString();
        dsGetVendorReportFiltersValue = objBL_ReportsData.GetVendorReportFiltersValue(objProp_User);


        if (dsGetVendorReportFiltersValue.Tables[2].Rows.Count > 0)
        {
            drpName.DataSource = dsGetVendorReportFiltersValue.Tables[2];
            drpName.DataTextField = "Name";
            drpName.DataValueField = "Name";
            drpName.DataBind();
        }

        if (dsGetVendorReportFiltersValue.Tables[8].Rows.Count > 0)
        {
            drp1099.DataSource = dsGetVendorReportFiltersValue.Tables[8];
            drp1099.DataTextField = "Checking";
            drp1099.DataValueField = "Checking";
            drp1099.DataBind();
        }

        if (dsGetVendorReportFiltersValue.Tables[9].Rows.Count > 0)
        {
            drpFID.DataSource = dsGetVendorReportFiltersValue.Tables[9];
            drpFID.DataTextField = "FID";
            drpFID.DataValueField = "FID";
            drpFID.DataBind();
        }
        if (dsGetVendorReportFiltersValue.Tables[10].Rows.Count > 0)
        {
            drpDA.DataSource = dsGetVendorReportFiltersValue.Tables[10];
            drpDA.DataTextField = "DA";
            drpDA.DataValueField = "DA";
            drpDA.DataBind();
        }

        if (dsGetVendorReportFiltersValue.Tables[7].Rows.Count > 0)
        {
            drpCLimit.DataSource = dsGetVendorReportFiltersValue.Tables[7];
            drpCLimit.DataTextField = "CLimit";
            drpCLimit.DataValueField = "CLimit";
            drpCLimit.DataBind();
        }

        if (dsGetVendorReportFiltersValue.Tables[11].Rows.Count > 0)
        {
            drpAcc.DataSource = dsGetVendorReportFiltersValue.Tables[11];
            drpAcc.DataTextField = "Acct#";
            drpAcc.DataValueField = "Acct#";
            drpAcc.DataBind();
        }

        if (dsGetVendorReportFiltersValue.Tables[12].Rows.Count > 0)
        {
            drpTerms.DataSource = dsGetVendorReportFiltersValue.Tables[12];
            drpTerms.DataTextField = "Terms";
            drpTerms.DataValueField = "Terms";
            drpTerms.DataBind();
        }

        if (dsGetVendorReportFiltersValue.Tables[13].Rows.Count > 0)
        {
            drpDisc.DataSource = dsGetVendorReportFiltersValue.Tables[13];
            drpDisc.DataTextField = "Disc";
            drpDisc.DataValueField = "Disc";
            drpDisc.DataBind();
        }

        if (dsGetVendorReportFiltersValue.Tables[14].Rows.Count > 0)
        {
            drpDays.DataSource = dsGetVendorReportFiltersValue.Tables[14];
            drpDays.DataTextField = "Days";
            drpDays.DataValueField = "Days";
            drpDays.DataBind();
        }

        if (dsGetVendorReportFiltersValue.Tables[15].Rows.Count > 0)
        {
            drpInUse.DataSource = dsGetVendorReportFiltersValue.Tables[15];
            drpInUse.DataTextField = "InUse";
            drpInUse.DataValueField = "InUse";
            drpInUse.DataBind();
        }

        if (dsGetVendorReportFiltersValue.Tables[17].Rows.Count > 0)
        {
            drpOnePer.DataSource = dsGetVendorReportFiltersValue.Tables[17];
            drpOnePer.DataTextField = "OnePer";
            drpOnePer.DataValueField = "OnePer";
            drpOnePer.DataBind();
        }

        if (dsGetVendorReportFiltersValue.Tables[18].Rows.Count > 0)
        {
            drpDBank.DataSource = dsGetVendorReportFiltersValue.Tables[18];
            drpDBank.DataTextField = "DBank";
            drpDBank.DataValueField = "DBank";
            drpDBank.DataBind();
        }

        if (dsGetVendorReportFiltersValue.Tables[19].Rows.Count > 0)
        {
            drpCustom1.DataSource = dsGetVendorReportFiltersValue.Tables[19];
            drpCustom1.DataTextField = "Custom1";
            drpCustom1.DataValueField = "Custom1";
            drpCustom1.DataBind();
        }

        if (dsGetVendorReportFiltersValue.Tables[20].Rows.Count > 0)
        {
            drpCustom2.DataSource = dsGetVendorReportFiltersValue.Tables[20];
            drpCustom2.DataTextField = "Custom2";
            drpCustom2.DataValueField = "Custom2";
            drpCustom2.DataBind();
        }

        if (dsGetVendorReportFiltersValue.Tables[21].Rows.Count > 0)
        {
            drpCustom3.DataSource = dsGetVendorReportFiltersValue.Tables[21];
            drpCustom3.DataTextField = "Custom3";
            drpCustom3.DataValueField = "Custom3";
            drpCustom3.DataBind();
        }

        if (dsGetVendorReportFiltersValue.Tables[22].Rows.Count > 0)
        {
            drpCustom4.DataSource = dsGetVendorReportFiltersValue.Tables[22];
            drpCustom4.DataTextField = "Custom4";
            drpCustom4.DataValueField = "Custom4";
            drpCustom4.DataBind();
        }

        if (dsGetVendorReportFiltersValue.Tables[23].Rows.Count > 0)
        {
            drpCustom5.DataSource = dsGetVendorReportFiltersValue.Tables[23];
            drpCustom5.DataTextField = "Custom5";
            drpCustom5.DataValueField = "Custom6";
            drpCustom5.DataBind();
        }

        if (dsGetVendorReportFiltersValue.Tables[24].Rows.Count > 0)
        {
            drpCustom6.DataSource = dsGetVendorReportFiltersValue.Tables[24];
            drpCustom6.DataTextField = "Custom6";
            drpCustom6.DataValueField = "Custom6";
            drpCustom6.DataBind();
        }

        if (dsGetVendorReportFiltersValue.Tables[25].Rows.Count > 0)
        {
            drpCustom7.DataSource = dsGetVendorReportFiltersValue.Tables[25];
            drpCustom7.DataTextField = "Custom7";
            drpCustom7.DataValueField = "Custom7";
            drpCustom7.DataBind();
        }

        if (dsGetVendorReportFiltersValue.Tables[26].Rows.Count > 0)
        {
            drpCustom8.DataSource = dsGetVendorReportFiltersValue.Tables[26];
            drpCustom8.DataTextField = "Custom8";
            drpCustom8.DataValueField = "Custom8";
            drpCustom8.DataBind();
        }

        if (dsGetVendorReportFiltersValue.Tables[27].Rows.Count > 0)
        {
            drpCustom9.DataSource = dsGetVendorReportFiltersValue.Tables[27];
            drpCustom9.DataTextField = "Custom9";
            drpCustom9.DataValueField = "Custom9";
            drpCustom9.DataBind();
        }

        if (dsGetVendorReportFiltersValue.Tables[28].Rows.Count > 0)
        {
            drpCustom10.DataSource = dsGetVendorReportFiltersValue.Tables[28];
            drpCustom10.DataTextField = "Custom10";
            drpCustom10.DataValueField = "Custom10";
            drpCustom10.DataBind();
        }

        if (dsGetVendorReportFiltersValue.Tables[29].Rows.Count > 0)
        {
            drpShipVia.DataSource = dsGetVendorReportFiltersValue.Tables[29];
            drpShipVia.DataTextField = "ShipVia";
            drpShipVia.DataValueField = "ShipVia";
            drpShipVia.DataBind();
        }

        if (dsGetVendorReportFiltersValue.Tables[30].Rows.Count > 0)
        {
            drpQBVendorID.DataSource = dsGetVendorReportFiltersValue.Tables[30];
            drpQBVendorID.DataTextField = "QBVendorID";
            drpQBVendorID.DataValueField = "QBVendorID";
            drpQBVendorID.DataBind();
        }

        if (dsGetVendorReportFiltersValue.Tables[3].Rows.Count > 0)
        {
            drpAcct.DataSource = dsGetVendorReportFiltersValue.Tables[3];
            drpAcct.DataTextField = "Acct";
            drpAcct.DataValueField = "Acct";
            drpAcct.DataBind();
        }

        if (dsGetVendorReportFiltersValue.Tables[16].Rows.Count > 0)
        {
            drpRemit.DataSource = dsGetVendorReportFiltersValue.Tables[16];
            drpRemit.DataTextField = "Remit";
            drpRemit.DataValueField = "Remit";
            drpRemit.DataBind();
        }


        if (dsGetVendorReportFiltersValue.Tables[4].Rows.Count > 0)
        {
            drpType.DataSource = dsGetVendorReportFiltersValue.Tables[4];
            drpType.DataTextField = "Type";
            drpType.DataValueField = "Type";
            drpType.DataBind();
        }
    }

    private void GetCustomerName()
    {
        try
        {
            DataSet dsGetCustName = new DataSet();
            objCustReport.DBName = Session["dbname"].ToString();
            objCustReport.ConnConfig = Session["config"].ToString();
            dsGetCustName = objBL_ReportsData.GetCustomerName(objCustReport);
            if (dsGetCustName.Tables[0].Rows.Count > 0)
            {
                drpName.DataSource = dsGetCustName.Tables[0];
                drpName.DataTextField = "Name";
                drpName.DataValueField = "Name";
                drpName.DataBind();

            }
        }
        catch (Exception exp)
        {
            ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: '" + exp.Message.ToString() + "',  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
    }

    private void GetCustomerAddress()
    {
        try
        {
            DataSet dsGetCustAddress = new DataSet();
            objCustReport.DBName = Session["dbname"].ToString();
            objCustReport.ConnConfig = Session["config"].ToString();
            dsGetCustAddress = objBL_ReportsData.GetCustomerAddress(objCustReport);
            if (dsGetCustAddress.Tables[0].Rows.Count > 0)
            {
                drpAddress.DataSource = dsGetCustAddress.Tables[0];
                drpAddress.DataTextField = "Address";
                drpAddress.DataValueField = "Address";
                drpAddress.DataBind();

            }
        }
        catch (Exception exp)
        {
            ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: '" + exp.Message.ToString() + "',  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
    }

    private void GetCustomerCity()
    {
        try
        {
            DataSet dsGetCustCity = new DataSet();
            objCustReport.DBName = Session["dbname"].ToString();
            objCustReport.ConnConfig = Session["config"].ToString();
            dsGetCustCity = objBL_ReportsData.GetCustomerCity(objCustReport);
            if (dsGetCustCity.Tables[0].Rows.Count > 0)
            {
                drpCity.DataSource = dsGetCustCity.Tables[0];
                drpCity.DataTextField = "City";
                drpCity.DataValueField = "City";
                drpCity.DataBind();

            }
        }
        catch (Exception exp)
        {
            ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: '" + exp.Message.ToString() + "',  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
    }


    private void GetReportsName()
    {
        try
        {
            string globalImageURL = "images/Globel_Report.png";
            string privateImageURL = "images/Private_Report.png";

            DataSet dsGetReports = new DataSet();
            objProp_User.DBName = Session["dbname"].ToString();
            objProp_User.ConnConfig = Session["config"].ToString();
            objProp_User.UserID = Convert.ToInt32(Session["UserID"].ToString());
            dsGetReports = objBL_ReportsData.GetDynamicReports(objProp_User, reportType);
            if (dsGetReports.Tables.Count > 0)
            {
                drpReports.DataSource = dsGetReports.Tables[0];
                drpReports.DataTextField = "ReportName";
                drpReports.DataValueField = "Id";
                drpReports.DataBind();
                drpReports.Items.Insert(0, new System.Web.UI.WebControls.ListItem { Text = "-Select-", Value = "0" });
                // drpReports.Items.Insert(0, new System.Web.UI.WebControls.ListItem { Text = "Customer Detail", Value = "0" });
                System.Web.UI.WebControls.ListItem itemCD = drpReports.Items[0];
                itemCD.Attributes["style"] = "background: url(" + globalImageURL + ");background-repeat:no-repeat;";
                drpReports.SelectedValue = pubReportId.ToString();

                if (dsGetReports.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsGetReports.Tables[0].Rows.Count; i++)
                    {
                        if (Convert.ToBoolean(dsGetReports.Tables[0].Rows[i]["IsGlobal"].ToString()) == true)
                        {
                            System.Web.UI.WebControls.ListItem item = drpReports.Items[i + 1];
                            item.Attributes["style"] = "background: url(" + globalImageURL + ");background-repeat:no-repeat;";
                        }
                        else
                        {
                            System.Web.UI.WebControls.ListItem item = drpReports.Items[i + 1];
                            item.Attributes["style"] = "background: url(" + privateImageURL + ");background-repeat:no-repeat;";
                        }
                    }
                }

            }
        }
        catch (Exception exp)
        {
            ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: '" + exp.Message.ToString() + "',  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
    }

    private void GetReportColumnsByRepId()
    {
        try
        {
            DataSet dsGetColumns = new DataSet();
            objCustReport.DBName = Session["dbname"].ToString();
            objCustReport.ConnConfig = Session["config"].ToString();
            objCustReport.ReportId = pubReportId;
            if (pubReportId != 0)
            {
                dsGetColumns = objBL_ReportsData.GetReportColByRepId(objCustReport);
                string[] checkedColumns = null;
                string[] selectedFiltersColumns = null;
                string[] selectedFiltersValues = null;
                if (dsGetColumns.Tables[0].Rows.Count > 0)
                {
                    checkedColumns = dsGetColumns.Tables[0].AsEnumerable().Select(s => s.Field<string>("ColumnName")).ToArray<string>();
                    hdnColumnList.Value = string.Join(",", checkedColumns);
                }

                DataSet dsSelectedFilters = new DataSet();
                dsSelectedFilters = objBL_ReportsData.GetReportFiltersByRepId(objCustReport);
                if (dsSelectedFilters.Tables[0].Rows.Count > 0)
                {
                    selectedFiltersColumns = dsSelectedFilters.Tables[0].AsEnumerable().Select(s => s.Field<string>("FilterColumn")).ToArray<string>();
                    selectedFiltersValues = dsSelectedFilters.Tables[0].AsEnumerable().Select(s => s.Field<string>("FilterSet")).ToArray<string>();
                }

                //if (drpReports.SelectedItem.ToString().ToLower() != "resize and reorder")
                //{
                //   // BindReport(checkedColumns, selectedFiltersColumns, selectedFiltersValues, sortBy);
                //   // dvGridReport.Attributes.Add("style", "display:none");
                //}
                //else
                //{
                BindGridReport(checkedColumns, selectedFiltersColumns, selectedFiltersValues, sortBy);
                dvGridReport.Attributes.Add("style", "display:block;height:350px;overflow:auto;");
                //}
            }
            else
            {
                // GetGroupedCustomersLocation();
                dvGridReport.Attributes.Add("style", "display:none");
            }

        }
        catch (Exception exp)
        {
            ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: '" + exp.Message.ToString() + "',  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
    }

    private List<CustomerReport> GetReportFilters()
    {
        List<CustomerReport> lstCustomerReport = new List<CustomerReport>();
        try
        {
            DataSet dsGetFilters = new DataSet();
            objCustReport.DBName = Session["dbname"].ToString();
            objCustReport.ConnConfig = Session["config"].ToString();
            objCustReport.ReportId = pubReportId;
            dsGetFilters = objBL_ReportsData.GetReportFiltersByRepId(objCustReport);
            for (int i = 0; i <= dsGetFilters.Tables[0].Rows.Count - 1; i++)
            {
                CustomerReport objCustmerReport = new CustomerReport();
                objCustmerReport.FilterColumns = dsGetFilters.Tables[0].Rows[i]["FilterColumn"].ToString();
                objCustmerReport.FilterValues = dsGetFilters.Tables[0].Rows[i]["FilterSet"].ToString();

                lstCustomerReport.Add(objCustmerReport);
            }
        }
        catch (Exception exp)
        {
            ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: '" + exp.Message.ToString() + "',  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
        return lstCustomerReport;
    }

    public void ConvertToJSON()
    {
        JavaScriptSerializer jss1 = new JavaScriptSerializer();
        string _myJSONstring = jss1.Serialize(GetReportFilters());
        string filters = "var filters=" + _myJSONstring + ";";
        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "reportsr123", filters, true);
    }


    private void GetCustomerDetails()
    {
        try
        {
            DataSet dsGetCustDetails = new DataSet();
            DataSet dsGetAccountSummaryListing = new DataSet();
            objProp_User.DBName = Session["dbname"].ToString();
            objProp_User.ConnConfig = Session["config"].ToString();
            //dsGetCustDetails = objBL_ReportsData.getCustomerDetails(objProp_User);
            dsGetCustDetails = objBL_ReportsData.GetVendorDetails(objProp_User);
            dsGetAccountSummaryListing = objBL_ReportsData.GetAccountSummaryListingDetail(objProp_User);

            if (dsGetCustDetails.Tables.Count > 0)
            {
                //List<string> lstHeaders = new List<string>();
                //string[] columnNames = (from dc in dsGetCustDetails.Tables[0].Columns.Cast<DataColumn>()
                //                        select dc.ColumnName).ToArray();

                //lstHeaders = (from dc in dsGetCustDetails.Tables[0].Columns.Cast<DataColumn>()
                //              //orderby dc.ColumnName
                //              select dc.ColumnName).ToList();

                //if (dsGetAccountSummaryListing.Tables.Count > 0)
                //{
                //    var _accSummary = (from dc in dsGetAccountSummaryListing.Tables[0].Columns.Cast<DataColumn>()
                //                       select dc.ColumnName).ToList();
                //    lstHeaders.AddRange(_accSummary);
                //}

                dsGetCustDetails.Tables[0].TableName = "Vendor";
                //dsGetCustDetails.Tables[1].TableName = "Locations";
                //dsGetCustDetails.Tables[2].TableName = "Equipments";
                //dsGetCustDetails.Tables[3].TableName = "Bill";
                foreach (DataTable _table in dsGetCustDetails.Tables)
                {
                    List<System.Web.UI.WebControls.ListItem> lstHeaders = new List<System.Web.UI.WebControls.ListItem>();
                    List<string> lstHeaders1 = (from dc in _table.Columns.Cast<DataColumn>()
                                                select dc.ColumnName).OrderBy(d => d).ToList();

                    System.Web.UI.WebControls.ListItem _newLstBox = new System.Web.UI.WebControls.ListItem();
                    _newLstBox.Text += "<label id='lblText' style='font-weight:bolder'>" + _table.TableName + "</label>";
                    chkColumnList.Items.Add(_newLstBox);

                    System.Web.UI.WebControls.ListItem _newLstBox1 = new System.Web.UI.WebControls.ListItem();
                    _newLstBox1.Text = _table.TableName;
                    _newLstBox1.Attributes.CssStyle.Add("font-size", "15px");
                    _newLstBox1.Attributes.CssStyle.Add("font-weight", "bolder !important");
                    _newLstBox1.Attributes.CssStyle.Add("padding", "7px 0");
                    lstFilter.Items.Add(_newLstBox1);

                    foreach (var _header in lstHeaders1)
                    {
                        chkColumnList.Items.Add(_header);
                        lstFilter.Items.Add(_header);
                    }
                    //chkColumnList.Items.AddRange(lstHeaders);
                }

                //foreach (var _header in lstHeaders)
                //{
                //    switch (_header)
                //    {
                //        case "Name":
                //            System.Web.UI.WebControls.ListItem _newLstBox = new System.Web.UI.WebControls.ListItem();
                //            _newLstBox.Text += "<label id='lblText' style='font-weight:bolder'>Customer</label>";

                //            chkColumnList.Items.Add(_newLstBox);
                //            chkColumnList.Items.Add(_header);
                //            break;

                //        case "LocationId":
                //            System.Web.UI.WebControls.ListItem _newLstBox1 = new System.Web.UI.WebControls.ListItem();
                //            _newLstBox1.Text += "<label id='lblText' style='font-weight:bolder'>Location</label>";

                //            chkColumnList.Items.Add(_newLstBox1);
                //            chkColumnList.Items.Add(_header);
                //            break;

                //        case "EquipmentName":
                //            System.Web.UI.WebControls.ListItem _newLstBox2 = new System.Web.UI.WebControls.ListItem();
                //            _newLstBox2.Text += "<label id='lblText' style='font-weight:bolder'>Equipment</label>";

                //            chkColumnList.Items.Add(_newLstBox2);
                //            chkColumnList.Items.Add(_header);
                //            break;

                //        default:
                //            chkColumnList.Items.Add(_header);
                //            break;
                //    }
                //}


                //var _lstHeaders = lstHeaders.OrderBy(d => d);
                //chkColumnList.DataSource = _lstHeaders;

                //chkColumnList.Items. = "sasdsad";
                //from dc in lstHeaders orderby 
                //chkColumnList.Items.Add(new System.Web.UI.WebControls.ListBox());
                chkColumnList.DataBind();

                //lstFilter.DataSource = _lstHeaders;
                lstFilter.DataBind();
                ClientScript.RegisterStartupScript(Page.GetType(), "removeCheckbox", "removeCheckbox();", true);
            }
        }
        catch (Exception exp)
        {
            ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: '" + exp.Message.ToString() + "',  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
    }

    protected void btnSaveReport_Click(object sender, EventArgs e)
    {
        try
        {
            //string[] checkedColumns = chkColumnList.Items.Cast<ListItem>().Where(li => li.Selected).Select(li => li.Value).ToArray();

            objCustReport.DBName = Session["dbname"].ToString();
            objCustReport.ConnConfig = Session["config"].ToString();
            //  objCustReport.ReportId = Convert.ToInt32(ViewState["ReportId"]);
            if (drpReports.Items.Count > 0)
            {
                objCustReport.ReportId = Convert.ToInt32(drpReports.SelectedValue);
            }
            objCustReport.ReportName = txtReportName.Text;

            //Changed by Yashasvi Jadav
            objCustReport.ReportType = !string.IsNullOrEmpty(Request.QueryString["type"]) ? objCustReport.ReportType = Request.QueryString["type"].ToString() : objCustReport.ReportType = "Customer";
            objCustReport.UserId = Convert.ToInt32(Session["UserID"].ToString());
            objCustReport.IsGlobal = chkIsGlobal.Checked ? true : false;
            // objCustReport.ColumnName = string.Join(",", checkedColumns);
            objCustReport.ColumnName = hdnLstColumns.Value.TrimEnd('^');
            objCustReport.ColumnWidth = hdnColumnWidth.Value.TrimEnd('^');
            objCustReport.FilterColumns = hdnFilterColumns.Value.TrimEnd('^');
            objCustReport.FilterValues = HttpUtility.HtmlDecode(hdnFilterValues.Value.Trim().TrimEnd('^').TrimEnd('|'));
            hdnCustomizeReportName.Value = objCustReport.ReportName;
            objCustReport.IsAscending = rdbOrders.SelectedItem.Value == "1" ? true : false;
            //objCustReport.SortBy = drpSortBy.Text;
            objCustReport.SortBy = hdnDrpSortBy.Value;
            objCustReport.MainHeader = chkMainHeader.Checked ? true : false;
            objCustReport.CompanyName = txtCompanyName.Text;
            objCustReport.ReportTitle = txtReportTitle.Text;
            objCustReport.SubTitle = txtSubtitle.Text;
            objCustReport.IsStock = false;
            if (chkDatePrepared.Checked)
            {
                objCustReport.DatePrepared = drpDatePrepared.SelectedValue.ToString();
            }
            else
            {
                objCustReport.DatePrepared = "";
            }
            objCustReport.TimePrepared = chkTimePrepared.Checked ? true : false;
            if (chkPageNumber.Checked)
            {
                objCustReport.PageNumber = drpPageNumber.SelectedValue.ToString();
            }
            else
            {
                objCustReport.PageNumber = "";
            }
            objCustReport.ExtraFooterLine = txtExtraFooterLine.Text;
            objCustReport.Alignment = drpAlignment.SelectedValue.ToString();
            objCustReport.PDFSize = drpPDFPageSize.SelectedValue.ToString();

            if (objBL_ReportsData.CheckExistingReport(objCustReport, hdnReportAction.Value) == true)
            {
                dvSaveReport.Attributes.Add("style", "display:block");
                ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: 'Report with this name already exists!',  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                return;
            }
            DataSet ds = new DataSet();
            if (hdnReportAction.Value == "Save")
            {
                ds = objBL_ReportsData.InsertCustomerReport(objCustReport);
                pubReportId = Convert.ToInt32(ds.Tables[0].Rows[0]["ReportId"]);
                ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: 'Report added successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);

            }
            else
            {
                if (objBL_ReportsData.IsStockReportExist(objCustReport, hdnReportAction.Value) == true)
                {
                    // if (objBL_ReportsData.CheckForDelete(objCustReport) == true && drpReports.SelectedItem.ToString().ToLower() != "default report")
                    //if (objBL_ReportsData.CheckForDelete(objCustReport) == true)
                    //{
                    objBL_ReportsData.UpdateCustomerReport(objCustReport);
                    pubReportId = objCustReport.ReportId;
                    ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: 'Report customized successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                    //}
                    //  else if (drpReports.SelectedItem.ToString().ToLower() == "default report")
                    //{
                    //  ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: 'Default Report can not be updated!',  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                    //}
                    //else
                    //{
                    //    ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: 'You dont have permission to update this report!',  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                    //}
                }
                else
                {
                    pubReportId = objCustReport.ReportId;
                    ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: 'You dont have permission to update this report. Please choose another title for this report',  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                }

            }
            dvSaveReport.Attributes.Add("style", "display:none");
            GetReportDetailByRptId();
            GetReportsName();
            GetReportColumnsByRepId();
            ConvertToJSON();
            ClientScript.RegisterStartupScript(Page.GetType(), "removeCheckbox", "removeCheckbox();", true);
        }
        catch (Exception exp)
        {
            ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: '" + exp.Message.ToString() + "',  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
    }


    protected void btnDeleteReport_Click(object sender, EventArgs e)
    {
        try
        {
            objCustReport.DBName = Session["dbname"].ToString();
            objCustReport.ConnConfig = Session["config"].ToString();
            if (drpReports.Items.Count > 0)
            {
                objCustReport.ReportId = Convert.ToInt32(drpReports.SelectedValue);
            }
            else
            {
                ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: 'Please select report to delete.',  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                return;
            }
            objCustReport.UserId = Convert.ToInt32(Session["UserID"].ToString());
            //if (objBL_ReportsData.CheckForDelete(objCustReport) == true && drpReports.SelectedItem.ToString().ToLower() != "default report")
            //if (objBL_ReportsData.CheckForDelete(objCustReport) == true)
            //{
            objBL_ReportsData.DeleteCustomerReport(objCustReport);
            ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: 'Report deleted successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);

            GetReportsName();
            if (drpReports.Items.Count > 0)
            {
                drpReports.SelectedIndex = 0;
                pubReportId = Convert.ToInt32(drpReports.SelectedValue);
                hdnCustomizeReportName.Value = drpReports.SelectedItem.ToString();

                GetReportDetailByRptId();
                GetReportsName();
                GetReportColumnsByRepId();
                ConvertToJSON();
            }
            else
            {
                // //CrystalReportViewer1.ReportSource = null;
            }
            // return;
            //}
            //else if (drpReports.SelectedItem.ToString().ToLower() == "default report")
            //{
            //    ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: 'Default Report can not be deleted!',  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);

            //}
            //else
            //{
            //    ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: 'You dont have permission to delete this report!',  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);

            //}
            // GetReportsName();
            //drpReports.SelectedValue = pubReportId.ToString();
            ClientScript.RegisterStartupScript(Page.GetType(), "removeCheckbox", "removeCheckbox();", true);

        }
        catch (Exception exp)
        {
            ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: '" + exp.Message.ToString() + "',  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
    }


    protected void drpReports_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            // if (drpReports.SelectedIndex != 0)
            // {
            pubReportId = Convert.ToInt32(drpReports.SelectedValue);
            hdnCustomizeReportName.Value = drpReports.SelectedItem.ToString();
            GetReportsName();
            drpReports.SelectedValue = pubReportId.ToString();

            if (pubReportId != 0)
            {
                //grdCustomerReportData.PageIndex = 0;
                // dvGridReport.Attributes.Add("style", "display:block");
                GetReportDetailByRptId();
                GetReportColumnsByRepId();
            }
            else
            {
                // GetGroupedCustomersLocation();
                // grdCustomerReportData.DataSource = null;
                dvGridReport.Attributes.Add("style", "display:none");
            }


            //  GetReportDetailByRptId();
            // GetReportColumnsByRepId();
            ConvertToJSON();
            ClientScript.RegisterStartupScript(Page.GetType(), "removeCheckbox", "removeCheckbox();", true);
            // }
        }
        catch (Exception exp)
        {
            ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: '" + exp.Message.ToString() + "',  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
    }

    protected void btnClose_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("Vendors.aspx", false);
        }
        catch
        {
            //
        }
    }


    private void BindGridReport(string[] checkedColumns, string[] selectedFiltersColumns, string[] selectedFiltersValues, string sortBy)
    {
        if (sortBy == "1099" || sortBy == "1099 Asc")
        {
            sortBy = "1";
        }
        string query = "SELECT distinct ";

        foreach (var item in checkedColumns)
        {
            if (item=="1099")
            {
                query += "v.[" + item + "],";
            }
            else
            {
                query += "v." + item + ",";
            }
            
        }

        query = query.Substring(0, query.Length - 1);
        if (selectedFiltersColumns == null)
        {
            query += " FROM VendorReportDetails v order by " + sortBy;
        }
        else
        {
            string filters = string.Empty;
            if (selectedFiltersColumns != null)
            {
                for (int i = 0; i <= selectedFiltersColumns.Count() - 1; i++)
                {
                    if (selectedFiltersColumns[i].ToLower() != "balance" && selectedFiltersColumns[i].ToLower() != "loc" && selectedFiltersColumns[i].ToLower() != "equip" && selectedFiltersColumns[i].ToLower() != "opencall" && selectedFiltersColumns[i].ToLower() != "equipmentprice" && selectedFiltersColumns[i].ToLower() != "equipmentcounts")
                    {
                        if (!selectedFiltersValues[i].Contains("'") && !selectedFiltersValues[i].Contains("|"))
                        {
                            filters += selectedFiltersColumns[i] + "=" + "'" + selectedFiltersValues[i] + "'" + " AND ";
                        }
                        else
                        {
                            int indexOfSingleQuote = selectedFiltersValues[i].IndexOf("'");
                            if (indexOfSingleQuote == 0)
                            {
                                filters += selectedFiltersColumns[i] + " in (" + selectedFiltersValues[i].Replace('|', ',') + ")" + " AND ";
                            }
                            else
                            {
                                filters += selectedFiltersColumns[i] + " in ('" + selectedFiltersValues[i].Replace('|', ',') + ")" + " AND ";
                            }
                        }
                    }
                    else
                    {
                        if (selectedFiltersValues[i].Contains("and"))
                        {
                            filters += selectedFiltersColumns[i] + selectedFiltersValues[i].Replace("and", "and " + selectedFiltersColumns[i] + "") + " AND ";
                        }
                        else
                        {
                            filters += selectedFiltersColumns[i] + selectedFiltersValues[i] + " AND ";
                        }
                    }
                }
            }
            filters = filters.Substring(0, filters.Length - 4);
            query += " FROM VendorReportDetails v where " + filters + " order by " + sortBy;
        }


        BindHeaderDetails();
        GetGridData(query);

    }

    private void BindHeaderDetails()
    {
        try
        {
            DataSet dsCompDetail = new DataSet();
            objProp_User.DBName = Session["dbname"].ToString();
            objProp_User.ConnConfig = Session["config"].ToString();
            dsCompDetail = objBL_ReportsData.GetControlForReports(objProp_User);
            if (dsCompDetail.Tables[0].Rows.Count > 0)
            {
                byte[] compLogo = null;
                if (!Convert.IsDBNull(dsCompDetail.Tables[0].Rows[0]["Logo"]))
                {
                    compLogo = (byte[])dsCompDetail.Tables[0].Rows[0]["Logo"];
                    imgLogo.ImageUrl = "data:image/png;base64," + Convert.ToBase64String(compLogo);
                }
                //MemoryStream ms = new MemoryStream(compLogo, 0, compLogo.Length);     //As asked by Anand, commented on 9th dec
                //// Convert byte[] to Image
                //ms.Write(compLogo, 0, compLogo.Length);
                //System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true);

                //string imagePathToSave = ConfigurationManager.AppSettings["ReportImagePath"].ToString() + Server.MapPath("ReportFiles/Images/logo" + Convert.ToInt32(Session["UserID"].ToString()) + ".png");
                //string imagePathToShow = ConfigurationManager.AppSettings["ReportImagePath"].ToString() + "ReportFiles/Images/logo" + Convert.ToInt32(Session["UserID"].ToString()) + ".png";

                //System.Drawing.Image resizedImage = image.GetThumbnailImage(130, 130, null, IntPtr.Zero);
                //resizedImage.Save(imagePathToSave, System.Drawing.Imaging.ImageFormat.Png);
                //// imgLogo.ImageUrl = imagePathToShow;

                lblCompanyName.Text = dsCompDetail.Tables[0].Rows[0]["Name"].ToString();
                lblCompAddress.Text = dsCompDetail.Tables[0].Rows[0]["Address"].ToString();
                lblCompEmail.Text = dsCompDetail.Tables[0].Rows[0]["Email"].ToString();
            }

            objCustReport.ReportId = pubReportId;
            DataSet dsGetHeaderFooterDetail = new DataSet();
            dsGetHeaderFooterDetail = objBL_ReportsData.GetHeaderFooterDetail(objCustReport);

            if (dsGetHeaderFooterDetail.Tables[0].Rows.Count > 0)
            {
                hdnMainHeader.Value = dsGetHeaderFooterDetail.Tables[0].Rows[0]["MainHeader"].ToString();
                if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["MainHeader"].ToString() == "True")
                {
                    chkMainHeader.Checked = true;
                }
                else
                {

                    chkMainHeader.Checked = false;
                }
                if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["CompanyName"].ToString() != "")
                {
                    txtCompanyName.Enabled = true;
                    txtCompanyName.Text = dsGetHeaderFooterDetail.Tables[0].Rows[0]["CompanyName"].ToString();
                    lblCompanyName2.Text = dsGetHeaderFooterDetail.Tables[0].Rows[0]["CompanyName"].ToString();
                    chkCompanyName.Checked = true;
                }
                else
                {
                    txtCompanyName.Enabled = false;
                    txtCompanyName.Text = "";
                    lblCompanyName2.Text = "";
                    chkCompanyName.Checked = false;
                }

                if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["ReportTitle"].ToString() != "")
                {
                    txtReportTitle.Enabled = true;
                    txtReportTitle.Text = dsGetHeaderFooterDetail.Tables[0].Rows[0]["ReportTitle"].ToString();
                    lblReportTitle.Text = dsGetHeaderFooterDetail.Tables[0].Rows[0]["ReportTitle"].ToString();
                    chkReportTitle.Checked = true;
                }
                else
                {
                    txtReportTitle.Enabled = false;
                    txtReportTitle.Text = "";
                    lblReportTitle.Text = "";
                    chkReportTitle.Checked = false;
                }

                if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["SubTitle"].ToString() != "")
                {
                    txtSubtitle.Enabled = true;
                    txtSubtitle.Text = dsGetHeaderFooterDetail.Tables[0].Rows[0]["SubTitle"].ToString();
                    lblSubTitle.Text = dsGetHeaderFooterDetail.Tables[0].Rows[0]["SubTitle"].ToString();
                    chkSubtitle.Checked = true;
                }
                else
                {
                    txtSubtitle.Enabled = false;
                    txtSubtitle.Text = "";
                    lblSubTitle.Text = "";
                    chkSubtitle.Checked = false;
                }

                if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["DatePrepared"].ToString() != "")
                {
                    if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["DatePrepared"].ToString() == "12/31/01")
                    {
                        lblDate.Text = DateTime.Now.Date.ToString("MM/dd/yy");
                    }
                    else if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["DatePrepared"].ToString() == "Dec 31, 01")
                    {
                        lblDate.Text = DateTime.Now.Date.ToString("MMM dd, yy");
                    }
                    else if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["DatePrepared"].ToString() == "December 31, 01")
                    {
                        lblDate.Text = DateTime.Now.Date.ToString("MMMM dd, yy");
                    }
                    else if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["DatePrepared"].ToString() == "Dec 31, 2001")
                    {
                        lblDate.Text = DateTime.Now.Date.ToString("MMM dd, yyyy");
                    }
                    else if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["DatePrepared"].ToString() == "December 31, 2001")
                    {
                        lblDate.Text = DateTime.Now.Date.ToString("MMMM dd, yyyy");
                    }
                    else
                    {
                        lblDate.Text = DateTime.Now.Date.ToString("MM/dd/yyyy");
                    }

                    drpDatePrepared.Enabled = true;
                    drpDatePrepared.SelectedValue = dsGetHeaderFooterDetail.Tables[0].Rows[0]["DatePrepared"].ToString();
                    chkDatePrepared.Checked = true;
                }
                else
                {
                    lblDate.Text = "";
                    drpDatePrepared.Enabled = false;
                    drpDatePrepared.SelectedIndex = 0;
                    chkDatePrepared.Checked = false;
                }

                if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["TimePrepared"].ToString() == "True")
                {
                    lblTime.Text = DateTime.Now.ToString("hh:mm tt");
                    chkTimePrepared.Checked = true;
                }
                else
                {
                    lblTime.Text = "";
                    chkTimePrepared.Checked = false;
                }

                if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["PageNumber"].ToString() != "")
                {
                    drpPageNumber.Enabled = true;
                    drpPageNumber.SelectedValue = dsGetHeaderFooterDetail.Tables[0].Rows[0]["PageNumber"].ToString();
                    chkPageNumber.Checked = true;
                }
                else
                {
                    drpPageNumber.Enabled = false;
                    drpPageNumber.SelectedIndex = 0;
                    chkPageNumber.Checked = false;
                }

                if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["ExtraFooterLine"].ToString() != "")
                {
                    txtExtraFooterLine.Enabled = true;
                    txtExtraFooterLine.Text = dsGetHeaderFooterDetail.Tables[0].Rows[0]["ExtraFooterLine"].ToString();
                    chkExtraFootLine.Checked = true;
                }
                else
                {
                    txtExtraFooterLine.Enabled = false;
                    txtExtraFooterLine.Text = "";
                    chkExtraFootLine.Checked = false;
                }

                if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["Alignment"].ToString() != "")
                {
                    drpAlignment.SelectedValue = dsGetHeaderFooterDetail.Tables[0].Rows[0]["Alignment"].ToString();
                    if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["Alignment"].ToString() == "Left")
                    {
                        dvSubHeader.Attributes.Add("Style", "text-align:-moz-left");
                        dvSubHeader.Attributes.Add("align", "left");
                    }
                    else if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["Alignment"].ToString() == "Right")
                    {
                        dvSubHeader.Attributes.Add("Style", "text-align:-moz-right");
                        dvSubHeader.Attributes.Add("align", "right");
                    }
                    else
                    {
                        dvSubHeader.Attributes.Add("Style", "text-align:-moz-center");
                        dvSubHeader.Attributes.Add("align", "center");
                    }
                }
                else
                {
                    drpDatePrepared.SelectedIndex = 0;
                }

                if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["PDFSize"].ToString() != "")
                {
                    drpPDFPageSize.SelectedValue = dsGetHeaderFooterDetail.Tables[0].Rows[0]["PDFSize"].ToString();
                }
                Session["ReportDate"] = lblDate.Text;
                Session["ReportTime"] = lblTime.Text;
            }
        }
        catch (Exception e)
        {
        }
    }

    private void GetGridData(string query)
    {
        try
        {
            DataSet dsGetCustDetails = new DataSet();
            objProp_User.DBName = Session["dbname"].ToString();
            objProp_User.ConnConfig = Session["config"].ToString();
            dsGetCustDetails = objBL_ReportsData.GetOwners(query, objProp_User);
            Session["DsGetCustomerDetails"] = dsGetCustDetails;
            Session["Query"] = query;
            if (dsGetCustDetails.Tables[0].Rows.Count > 0)
            {
                BindReportTable(dsGetCustDetails);

            }
            else
            {
            }


            ConvertToJSON();

        }
        catch (Exception exp)
        {
            ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: '" + exp.Message.ToString() + "',  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
    }

    private void BindReportTable(DataSet dsGetCustDetails)
    {
        try
        {
            objCustReport.DBName = Session["dbname"].ToString();
            objCustReport.ConnConfig = Session["config"].ToString();
            DataSet dsGetColumnWidth = new DataSet();
            objCustReport.ReportId = pubReportId;
            dsGetColumnWidth = objBL_ReportsData.GetColumnWidthByReportId(objCustReport);
            hdnColumnWidth.Value = "";
            //var _list = dsGetCustDetails.Tables[0].Rows;
            if (dsGetColumnWidth.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i <= dsGetColumnWidth.Tables[0].Rows.Count - 1; i++)
                {
                    hdnColumnWidth.Value += dsGetColumnWidth.Tables[0].Rows[i]["ColumnWidth"].ToString() + ",";
                }

                hdnColumnWidth.Value = hdnColumnWidth.Value.TrimEnd(',');
            }

            //Building an HTML string.
            StringBuilder html = new StringBuilder();
            string footer = string.Empty;
            //Table start.
            html.Append("<table id='tblResize' border = '0'>");

            //Building the Header row.
            html.Append("<thead><tr>");

            foreach (DataColumn column in dsGetCustDetails.Tables[0].Columns)
            {
                html.Append("<th class='resize-header'>");
                //html.Append("<th style='border:13px solid transparent;color:black;font-size:11px;width:150px; border-image: url(images/icons_big/list-bullet2.PNG) " + b + " '>");
                //html.Append("<th style='border:1;color:black;font-size:11px;'>");
                html.Append(column.ColumnName);
                html.Append("</th>");
            }
            html.Append("</tr></thead>");


            //Building the Data rows.
            foreach (DataRow row in dsGetCustDetails.Tables[0].Rows)
            {
                html.Append("<tr>");
                foreach (DataColumn column in dsGetCustDetails.Tables[0].Columns)
                {
                    html.Append("<td style='border:0;padding:10px 20px 3px 10px;color:black;'>");
                    html.Append(row[column.ColumnName]);
                    html.Append("</td>");
                }
                html.Append("</tr>");

            }

            // html.Append("<tr><td>&nbsp;</td><td>5000</td><td>5000</td><td>5000</td><td>5000</td><td>5000</td></tr>");

            for (int i = 0; i <= dsGetCustDetails.Tables[0].Columns.Count - 1; i++)
            {
                if (dsGetCustDetails.Tables[0].Columns[i].ToString() == "Balance")
                {
                    footer += "<td style='border:0;padding:20px 20px 3px 10px;color:black;font-weight:bold;'>" + dsGetCustDetails.Tables[0].Compute("SUM(Balance)", string.Empty).ToString() + "</td>";
                }
                else if (dsGetCustDetails.Tables[0].Columns[i].ToString() == "loc")
                {
                    footer += "<td style='border:0;padding:20px 20px 3px 10px;color:black;font-weight:bold;'>" + dsGetCustDetails.Tables[0].Compute("SUM(loc)", string.Empty).ToString() + "</td>";
                }
                else if (dsGetCustDetails.Tables[0].Columns[i].ToString() == "equip")
                {
                    footer += "<td style='border:0;padding:20px 20px 3px 10px;color:black;font-weight:bold;'>" + dsGetCustDetails.Tables[0].Compute("SUM(equip)", string.Empty).ToString() + "</td>";
                }
                else if (dsGetCustDetails.Tables[0].Columns[i].ToString() == "opencall")
                {
                    footer += "<td style='border:0;padding:20px 20px 3px 10px;color:black;font-weight:bold;'>" + dsGetCustDetails.Tables[0].Compute("SUM(opencall)", string.Empty).ToString() + "</td>";
                }
                else
                {
                    footer += "<td style='border:0;padding:10px 20px 3px 10px;color:black;'>&nbsp;</td>";
                }
            }

            if (footer != "")
            {
                html.Append("<tr>" + footer + "</tr>");
            }

            //Table end.
            html.Append("</table>");
            html.Append("<div><b>Total Counts: </b>" + dsGetCustDetails.Tables[0].Rows.Count + "</div>");

            //Append the HTML string to Placeholder.
            PlaceHolder1.Controls.Add(new Literal { Text = html.ToString() });

        }
        catch (Exception exp)
        {
            ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: '" + exp.Message.ToString() + "',  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
    }

    private void GeneratePdfTable(DataSet dsGetCustDetails)
    {
        try
        {
            DeletePDFFiles();
        }
        catch
        {
            //
        }
        //server folder path which is stored your PDF documents         
        string serverPath = Server.MapPath("ReportFiles/PDF");
        if (!Directory.Exists(serverPath))
        {
            Directory.CreateDirectory(serverPath);
        }

        //string filename = path + "/Doc1.pdf";
        int userId = Convert.ToInt32(Session["UserID"].ToString());
        string fileName = hdnCustomizeReportName.Value.Replace(" ", "") + "-" + userId + "-" + DateTime.Now.Ticks + ".pdf";
        //string filePath = serverPath + "/" + fileName;
        string filePath = Path.Combine(serverPath, fileName); ;

        //File.Delete(filePath);
        Session["FilePath"] = filePath;
        // File.Create(filePath);
        // lblAttachedFile.Text = fileName;


        objCustReport.DBName = Session["dbname"].ToString();
        objCustReport.ConnConfig = Session["config"].ToString();

        int[] getColumnsWidth = new int[dsGetCustDetails.Tables[0].Columns.Count];
        int countTotalWidth = 0;
        DataSet dsGetColumnWidth = new DataSet();
        objCustReport.ReportId = pubReportId;
        dsGetColumnWidth = objBL_ReportsData.GetColumnWidthByReportId(objCustReport);
        if (dsGetColumnWidth.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i <= dsGetColumnWidth.Tables[0].Rows.Count - 1; i++)
            {
                getColumnsWidth[i] = Convert.ToInt32(dsGetColumnWidth.Tables[0].Rows[i]["ColumnWidth"].ToString().Replace("px", ""));
                countTotalWidth = countTotalWidth + getColumnsWidth[i];
            }
        }

        DataSet dsGetHeaderFooterDetail = new DataSet();
        dsGetHeaderFooterDetail = objBL_ReportsData.GetHeaderFooterDetail(objCustReport);

        Rectangle rcPageSize = PageSize.A4;
        //Create new PDF document  
        if (dsGetHeaderFooterDetail.Tables[0].Rows.Count > 0)
        {
            rcPageSize = PageSize.GetRectangle(dsGetHeaderFooterDetail.Tables[0].Rows[0]["PDFSize"].ToString());
        }
        // Rectangle rcPageSize = PageSize.GetRectangle("A1");
        //Document document = new Document(PageSize.A4, 20f, 20f, 20f, 20f);
        Document document = new Document(rcPageSize, 20f, 20f, 20f, 20f);

        try
        {
            PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.Create, FileAccess.Write));

            PdfPTable headerTable = new PdfPTable(2);
            PdfPTable tblDateTime = new PdfPTable(1);
            PdfPTable tblExtraFooter = new PdfPTable(1);
            tblExtraFooter.TotalWidth = countTotalWidth;
            tblExtraFooter.LockedWidth = true;
            tblExtraFooter.SpacingBefore = 30f;

            if (dsGetHeaderFooterDetail.Tables[0].Rows.Count > 0)
            {
                if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["MainHeader"].ToString() == "True")
                {

                    headerTable.TotalWidth = 300;
                    headerTable.LockedWidth = true;

                    //  headerTable.SpacingBefore = 20f;
                    //  headerTable.SpacingAfter = 30f;
                    headerTable.HorizontalAlignment = 0;

                    tblDateTime.TotalWidth = 200;
                    tblDateTime.LockedWidth = true;
                    tblDateTime.HorizontalAlignment = 2;

                    DataSet dsCompDetail = new DataSet();
                    objProp_User.DBName = Session["dbname"].ToString();
                    objProp_User.ConnConfig = Session["config"].ToString();
                    dsCompDetail = objBL_ReportsData.GetControlForReports(objProp_User);

                    byte[] compLogo = (byte[])dsCompDetail.Tables[0].Rows[0]["Logo"];

                    iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(compLogo);
                    logo.ScaleAbsolute(110, 110);
                    PdfPCell imageCell = new PdfPCell(logo);
                    imageCell.Border = 0;
                    headerTable.AddCell(imageCell);

                    Font compNameStyle = FontFactory.GetFont("Arial", 13, iTextSharp.text.Font.BOLD);
                    Font compStyle = FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.NORMAL);

                    PdfPTable tblCompDetails = new PdfPTable(1);

                    PdfPCell companyName = new PdfPCell(new Phrase(dsCompDetail.Tables[0].Rows[0]["Name"].ToString(), compNameStyle));
                    companyName.Border = 0;
                    companyName.PaddingTop = 20;
                    companyName.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                    tblCompDetails.AddCell(companyName);

                    PdfPCell compAddress = new PdfPCell(new Phrase(dsCompDetail.Tables[0].Rows[0]["Address"].ToString(), compStyle));
                    compAddress.Border = 0;
                    compAddress.PaddingTop = 10;
                    compAddress.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                    tblCompDetails.AddCell(compAddress);

                    PdfPCell compEmail = new PdfPCell(new Phrase(dsCompDetail.Tables[0].Rows[0]["Email"].ToString(), compStyle));
                    compEmail.Border = 0;
                    compEmail.PaddingTop = 10;
                    compEmail.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                    tblCompDetails.AddCell(compEmail);

                    headerTable.DefaultCell.Border = Rectangle.NO_BORDER;
                    headerTable.AddCell(tblCompDetails);
                }
            }
            Font header2ompNameStyle = FontFactory.GetFont("Arial", 12, iTextSharp.text.Font.BOLD);
            Font header2ReportTitleStyle = FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.BOLD);
            Font header2SubTitleStyle = FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL);
            PdfPTable tblCompDetails2 = new PdfPTable(1);
            if (dsGetHeaderFooterDetail.Tables[0].Rows.Count > 0)
            {
                tblCompDetails2.DefaultCell.Border = Rectangle.NO_BORDER;
                tblCompDetails2.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                tblCompDetails2.SpacingBefore = 15;
                tblCompDetails2.SpacingAfter = 10;

                if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["CompanyName"].ToString() != "")
                {
                    PdfPCell compName = new PdfPCell(new Phrase(dsGetHeaderFooterDetail.Tables[0].Rows[0]["CompanyName"].ToString(), header2ompNameStyle));
                    compName.Border = 0;
                    if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["Alignment"].ToString() == "Right")
                    {
                        compName.HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT;
                    }
                    else if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["Alignment"].ToString() == "Left")
                    {
                        compName.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                    }
                    else
                    {
                        compName.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                    }
                    tblCompDetails2.AddCell(compName);
                }
                if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["ReportTitle"].ToString() != "")
                {
                    PdfPCell reportTitle = new PdfPCell(new Phrase(dsGetHeaderFooterDetail.Tables[0].Rows[0]["ReportTitle"].ToString(), header2ReportTitleStyle));
                    reportTitle.Border = 0;
                    if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["Alignment"].ToString() == "Right")
                    {
                        reportTitle.HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT;
                    }
                    else if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["Alignment"].ToString() == "Left")
                    {
                        reportTitle.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                    }
                    else
                    {
                        reportTitle.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                    }
                    tblCompDetails2.AddCell(reportTitle);
                }
                if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["SubTitle"].ToString() != "")
                {
                    PdfPCell subTitle = new PdfPCell(new Phrase(dsGetHeaderFooterDetail.Tables[0].Rows[0]["SubTitle"].ToString(), header2SubTitleStyle));
                    subTitle.Border = 0;
                    if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["Alignment"].ToString() == "Right")
                    {
                        subTitle.HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT;
                    }
                    else if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["Alignment"].ToString() == "Left")
                    {
                        subTitle.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                    }
                    else
                    {
                        subTitle.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                    }
                    tblCompDetails2.AddCell(subTitle);
                }
                Font dateTimeStyle = FontFactory.GetFont("Arial", 7, iTextSharp.text.Font.BOLD);
                if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["TimePrepared"].ToString() == "True")
                {
                    if (Session["ReportTime"] != null)
                    {
                        PdfPCell time = new PdfPCell(new Phrase(Session["ReportTime"].ToString(), dateTimeStyle));
                        time.Border = 0;
                        time.HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT;
                        tblDateTime.AddCell(time);
                    }
                }

                if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["DatePrepared"].ToString() != "")
                {
                    if (Session["ReportDate"] != null)
                    {
                        PdfPCell date = new PdfPCell(new Phrase(Session["ReportDate"].ToString(), dateTimeStyle));
                        date.Border = 0;
                        date.HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT;
                        tblDateTime.AddCell(date);
                    }
                }

                if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["ExtraFooterLine"].ToString() != "")
                {
                    PdfPCell extraFooter = new PdfPCell(new Phrase(dsGetHeaderFooterDetail.Tables[0].Rows[0]["ExtraFooterLine"].ToString(), header2ReportTitleStyle));
                    extraFooter.Border = 0;
                    extraFooter.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                    tblExtraFooter.AddCell(extraFooter);
                }
            }

            PdfPTable table = new PdfPTable(dsGetCustDetails.Tables[0].Columns.Count);
            table.TotalWidth = countTotalWidth;

            //fix the absolute width of the table
            table.LockedWidth = true;

            table.SetWidths(getColumnsWidth);
            table.HorizontalAlignment = 0;
            table.DefaultCell.Border = Rectangle.NO_BORDER;
            //leave a gap before and after the table
            table.SpacingBefore = 20f;
            table.SpacingAfter = 30f;
            Font headerStyle = FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.BOLD);
            Font rowsStyle = FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.NORMAL);
            PdfPTable footer = new PdfPTable(dsGetCustDetails.Tables[0].Columns.Count);
            if (dsGetCustDetails.Tables[0].Rows.Count > 0)
            {
                for (int j = 0; j <= dsGetCustDetails.Tables[0].Columns.Count - 1; j++)
                {
                    PdfPCell columns = new PdfPCell(new Phrase(dsGetCustDetails.Tables[0].Columns[j].ToString(), headerStyle));
                    columns.Border = 0;
                    table.AddCell(columns);
                }

                for (int j = 0; j <= dsGetCustDetails.Tables[0].Rows.Count - 1; j++)
                {
                    for (int i = 0; i <= dsGetCustDetails.Tables[0].Columns.Count - 1; i++)
                    {
                        PdfPCell rows = new PdfPCell(new Phrase(dsGetCustDetails.Tables[0].Rows[j][i].ToString().Trim(), rowsStyle));
                        rows.Border = 0;
                        table.AddCell(rows);
                    }
                }

                Font footerStyle = FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.BOLD);
                footer.TotalWidth = countTotalWidth;
                footer.LockedWidth = true;
                footer.SetWidths(getColumnsWidth);
                footer.HorizontalAlignment = 0;
                footer.DefaultCell.Border = Rectangle.NO_BORDER;


                for (int i = 0; i <= dsGetCustDetails.Tables[0].Columns.Count - 1; i++)
                {
                    if (dsGetCustDetails.Tables[0].Columns[i].ToString() == "Balance")
                    {
                        PdfPCell rows = new PdfPCell(new Phrase(dsGetCustDetails.Tables[0].Compute("SUM(Balance)", string.Empty).ToString(), footerStyle));
                        rows.Border = 0;
                        footer.AddCell(rows);
                    }
                    else if (dsGetCustDetails.Tables[0].Columns[i].ToString() == "loc")
                    {
                        PdfPCell rows = new PdfPCell(new Phrase(dsGetCustDetails.Tables[0].Compute("SUM(loc)", string.Empty).ToString(), footerStyle));
                        rows.Border = 0;
                        footer.AddCell(rows);
                    }
                    else if (dsGetCustDetails.Tables[0].Columns[i].ToString() == "equip")
                    {
                        PdfPCell rows = new PdfPCell(new Phrase(dsGetCustDetails.Tables[0].Compute("SUM(equip)", string.Empty).ToString(), footerStyle));
                        rows.Border = 0;
                        footer.AddCell(rows);
                    }
                    else if (dsGetCustDetails.Tables[0].Columns[i].ToString() == "opencall")
                    {
                        PdfPCell rows = new PdfPCell(new Phrase(dsGetCustDetails.Tables[0].Compute("SUM(opencall)", string.Empty).ToString(), footerStyle));
                        rows.Border = 0;
                        footer.AddCell(rows);
                    }
                    else
                    {
                        PdfPCell rows = new PdfPCell(new Phrase(""));
                        rows.Border = 0;
                        footer.AddCell(rows);
                    }
                }
            }


            document.Open();
            document.Add(tblDateTime);
            document.Add(headerTable);
            if (dsGetHeaderFooterDetail.Tables[0].Rows.Count > 0)
            {
                document.Add(tblCompDetails2);
            }
            document.Add(table);
            document.Add(footer);
            document.Add(tblExtraFooter);
        }
        catch (Exception ex)
        {
            document.Close();
        }
        finally
        {
            document.Close();
        }
    }

    public void ShowPdf(string filename)
    {
        //Clears all content output from Buffer Stream
        Response.ClearContent();
        //Clears all headers from Buffer Stream
        Response.ClearHeaders();
        //Adds an HTTP header to the output stream
        Response.AddHeader("Content-Disposition", "inline;filename=" + filename);
        //Gets or Sets the HTTP MIME type of the output stream
        Response.ContentType = "application/pdf";
        //Writes the content of the specified file directory to an HTTP response output stream as a file block
        Response.WriteFile(filename);
        //sends all currently buffered output to the client
        Response.Flush();
        //Clears all content output from Buffer Stream
        Response.Clear();
    }

    private void SaveResizedAndReorderReport()
    {
        try
        {
            objCustReport.DBName = Session["dbname"].ToString();
            objCustReport.ConnConfig = Session["config"].ToString();
            objCustReport.UserId = Convert.ToInt32(Session["UserID"].ToString());
            objCustReport.ReportId = pubReportId;
            objCustReport.ColumnName = hdnLstColumns.Value.TrimEnd('^');
            objCustReport.ColumnWidth = hdnColumnWidth.Value.TrimEnd('^');
            //if (objBL_ReportsData.CheckForDelete(objCustReport) == true)
            //{
            objBL_ReportsData.UpdateCustomerReportResizedWidth(objCustReport);
            //}
            //else
            //{
            //    ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: 'You dont have permission to update this report!',  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
            //}
        }
        catch (Exception exp)
        {
            ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: '" + exp.Message.ToString() + "',  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
    }
    protected void btnExportPDF_Click(object sender, EventArgs e)
    {
        DataSet dsGetCustomerDetails = new DataSet();
        dsGetCustomerDetails = (DataSet)Session["DsGetCustomerDetails"];

        if (dsGetCustomerDetails != null)
        {
            SaveResizedAndReorderReport();
            GetReportColumnsByRepId();

            GetReportsName();
            if (pubReportId != 0)
            {
                drpReports.SelectedValue = pubReportId.ToString();
            }

            if (dsGetCustomerDetails.Tables.Count > 0)
            {
                if (dsGetCustomerDetails.Tables[0].Rows.Count > 0)
                {
                    GeneratePdfTable(dsGetCustomerDetails);
                    Session["ReportId"] = pubReportId;

                    string script = String.Format("window.open('CustomerReportPreview.aspx');");
                    this.Page.ClientScript.RegisterStartupScript(this.GetType(), "123", script, true);
                }
            }
        }
    }


    public void GetPDFData()
    {
        GetReportColumnsByRepId();
        DataSet dsGetCustomerDetails = new DataSet();
        dsGetCustomerDetails = (DataSet)Session["DsGetCustomerDetails"];
        Session["ReportId"] = pubReportId;
        GeneratePdfTable(dsGetCustomerDetails);
        //BindReportTable(dsGetCustomerDetails);
    }

    protected void btnSendReport_Click(object sender, EventArgs e)
    {
        SaveResizedAndReorderReport();
        GetReportColumnsByRepId();

        if (hdnSendReportType.Value == "btnSendPDFReport")
        {
            DataSet dsGetCustomerDetails = new DataSet();
            dsGetCustomerDetails = (DataSet)Session["DsGetCustomerDetails"];
            GeneratePdfTable(dsGetCustomerDetails);
        }
        else
        {
            GenerateExcelFile();
        }
        if (txtTo.Text.Trim() != string.Empty)
        {
            Mail mail = new Mail();
            try
            {
                mail.From = txtFrom.Text.Trim();
                mail.To = txtTo.Text.Split(';', ',').OfType<string>().ToList();
                if (txtCc.Text.Trim() != string.Empty)
                {
                    mail.Cc = txtCc.Text.Split(';', ',').OfType<string>().ToList();
                }
                mail.Title = txtSubject.Text;
                if (txtBody.Text.Trim() != string.Empty)
                {
                    mail.Text = txtBody.Text.Replace(Environment.NewLine, "<BR/>");
                }
                else
                {
                    mail.Text = "This is report email sent from Mobile Office Manager. Please find the Report attached.";
                }

                string filePath = Session["FilePath"].ToString();
                mail.AttachmentFiles.Add(filePath);
                // mail.attachmentBytes = ExportReportToPDF("");                    

                mail.DeleteFilesAfterSend = true;
                mail.RequireAutentication = false;
                // ES-33:Task#2: add 
                WebBaseUtility.UpdateMailConfigurationFromLoginUser(mail);
                mail.Send();
                GetReportsName();
                if (pubReportId != 0)
                {
                    drpReports.SelectedValue = pubReportId.ToString();
                }
                //  this.programmaticModalPopup.Hide();
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "noty({text: 'Email sent successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);

            }
            catch (Exception ex)
            {
                //string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
                string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(ex.Message);
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);

            }
        }
    }


    private void GenerateExcelFile()
    {
        try
        {
            try
            {
                DeleteExcelFiles();
            }
            catch
            {
                //
            }
            string htmlToExport = string.Empty;
            StringBuilder sbHeader = new StringBuilder();
            StringBuilder sbExtraFooter = new StringBuilder();
            DataSet dsCompDetail = new DataSet();
            objProp_User.DBName = Session["dbname"].ToString();
            objProp_User.ConnConfig = Session["config"].ToString();
            dsCompDetail = objBL_ReportsData.GetControlForReports(objProp_User);

            objCustReport.DBName = Session["dbname"].ToString();
            objCustReport.ConnConfig = Session["config"].ToString();
            objCustReport.ReportId = pubReportId;
            DataSet dsGetHeaderFooterDetail = new DataSet();
            dsGetHeaderFooterDetail = objBL_ReportsData.GetHeaderFooterDetail(objCustReport);

            string excelServerPath = Server.MapPath("ReportFiles/Excel");
            if (!Directory.Exists(excelServerPath))
            {
                Directory.CreateDirectory(excelServerPath);
            }
            int userId = Convert.ToInt32(Session["UserID"].ToString());

            string excelFileName = hdnCustomizeReportName.Value.Replace(" ", "") + DateTime.Now.ToString("MM-dd-yyyy") + ".xls";
            string filePath = excelServerPath + "\\" + excelFileName + ".xls";


            DataSet ds = (DataSet)Session["DsGetCustomerDetails"];
            //Write the HTML back to the browser.
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Disposition", "attachment; filename=" + excelFileName + "");
            StringWriter stringWrite = new StringWriter();
            HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);
            DataGrid dgrExport = new DataGrid();
            dgrExport.DataSource = ds.Tables[0];
            dgrExport.DataBind();

            byte[] compLogo = (byte[])dsCompDetail.Tables[0].Rows[0]["Logo"];
            sbHeader.Append("<html><body><div><table border = '0'>");
            string baseUrl = Request.Url.Scheme + "://" + Request.Url.Authority +
                    Request.ApplicationPath.TrimEnd('/') + "/";
            if (dsCompDetail.Tables[0].Rows.Count > 0)
            {
                string imagePathToShow = baseUrl + "Logo.ashx?db=" + dsCompDetail.Tables[0].Rows[0]["DBName"].ToString();
                if (dsGetHeaderFooterDetail.Tables[0].Rows.Count > 0)
                {

                    if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["MainHeader"].ToString() == "True")
                    {
                        sbHeader.Append("<tr><td><table><tr><td rowspan='4' style='height:150px;vertical-align:center;text-align:center;'><img src=" + imagePathToShow + "></img></td><td>&nbsp;</td></tr><tr><td style='height:20px;text-align:left;color:Black;font-size:18px;font-weight:bold;'>" + dsCompDetail.Tables[0].Rows[0]["Name"].ToString() + "</td></tr>");
                        sbHeader.Append("<tr><td style='height:20px;text-align:left;color:Black;font-size:15px;font-weight:bold;'>" + dsCompDetail.Tables[0].Rows[0]["Address"].ToString() + "</td></tr>");
                        sbHeader.Append("<tr><td style='height:20px;text-align:left;color:Black;font-size:15px;font-weight:normal;'>" + dsCompDetail.Tables[0].Rows[0]["Email"].ToString() + "</td></tr>");
                        sbHeader.Append("<tr><td style='height:60px;'>&nbsp;</td></tr>");
                        sbHeader.Append("</table></td><td style='vertical-align:top;font-weight:bold;font-size:10px;color:black;'>" + lblTime.Text + " <br/> " + lblDate.Text + "</td></tr>");
                    }
                    string alignment = string.Empty;
                    alignment = dsGetHeaderFooterDetail.Tables[0].Rows[0]["Alignment"].ToString();
                    if (alignment.ToLower() == "standard" || alignment.ToLower() == "centered")
                    {
                        alignment = "center";
                    }

                    if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["CompanyName"].ToString() != "")
                    {
                        sbHeader.Append("<tr><td colspan='2' style='height:20px;text-align:" + alignment + ";color:Black;font-size:16px;font-weight:bold;'>" + lblCompanyName2.Text + "</td></tr>");
                    }
                    if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["ReportTitle"].ToString() != "")
                    {
                        sbHeader.Append("<tr><td colspan='2' style='height:20px;text-align:" + alignment + ";color:Black;font-size:14px;font-weight:bold;'>" + lblReportTitle.Text + "</td></tr>");
                    }
                    if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["SubTitle"].ToString() != "")
                    {
                        sbHeader.Append("<tr><td colspan='2' style='height:20px;text-align:" + alignment + ";color:Black;font-size:12px;font-weight:bold;'>" + lblSubTitle.Text + "</td></tr>");
                    }
                    sbHeader.Append("<tr><td colspan='2' style='height:15px;'>&nbsp;</td></tr>");
                    sbHeader.Append("<tr><td colspan='2'>");

                    if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["ExtraFooterLine"].ToString() != "")
                    {
                        sbExtraFooter.Append("<tr><td colspan='2' style='height:80px;text-align:center;color:Black;font-size:14px;font-weight:bold;'>" + dsGetHeaderFooterDetail.Tables[0].Rows[0]["ExtraFooterLine"].ToString() + "</td></tr>");
                    }
                }
            }
            dgrExport.RenderControl(htmlWrite);

            Response.Write(sbHeader);
            Response.Write(stringWrite.ToString());
            Response.Write(sbExtraFooter.ToString());
            Response.End();

            //commented by Mayuri 20th july, 16
            //string headerTable = @"<Table><tr><td>Report Header</td></tr><tr><td><img src=""D:\\Folder\\Report Header.jpg"" \></td></tr></Table>";
            //string excelFileName = hdnCustomizeReportName.Value.Replace(" ", "") + "-" + userId + "-" + DateTime.Now.Ticks + ".xls";
            //if (dsCompDetail.Tables[0].Rows.Count > 0)
            //{
            //   // string imagePathToShow = ConfigurationManager.AppSettings["ReportImagePath"].ToString() + "ReportFiles/Images/logo" + Convert.ToInt32(Session["UserID"].ToString()) + ".png";
            //    string imagePathToShow = ConfigurationManager.AppSettings["ReportImagePath"].ToString() +"/logo.ashx?db=" + dsCompDetail.Tables[0].Rows[0]["DBName"].ToString();
            //    sbHeader.Append(@"<html xmlns:o='urn:schemas-microsoft-com:office:office' xmlns:w='urn:schemas-microsoft-com:office:excel' xmlns='http://www.w3.org/TR/REC-html40'><head><title>Time</title>");
            //    sbHeader.Append(@"<body lang=EN-US style='mso-element:header' id=h1><span style='mso--code:DATE'></span><div>");
            //    sbHeader.Append("<table border = '0'>");

            //    if (dsGetHeaderFooterDetail.Tables[0].Rows.Count > 0)
            //    {
            //        if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["MainHeader"].ToString() == "True")
            //        {
            //            sbHeader.Append("<tr><td><table><tr><td rowspan='4' style='height:150px;vertical-align:center;text-align:center;'><img src=" + imagePathToShow + "></img></td><td>&nbsp;</td></tr><tr><td style='height:20px;text-align:left;color:Black;font-size:18px;font-weight:bold;'>" + dsCompDetail.Tables[0].Rows[0]["Name"].ToString() + "</td></tr>");
            //            sbHeader.Append("<tr><td style='height:20px;text-align:left;color:Black;font-size:15px;font-weight:bold;'>" + dsCompDetail.Tables[0].Rows[0]["Address"].ToString() + "</td></tr>");
            //            sbHeader.Append("<tr><td style='height:20px;text-align:left;color:Black;font-size:15px;font-weight:normal;'>" + dsCompDetail.Tables[0].Rows[0]["Email"].ToString() + "</td></tr>");
            //            sbHeader.Append("<tr><td style='height:60px;'>&nbsp;</td></tr>");
            //            sbHeader.Append("</table></td><td style='vertical-align:top;font-weight:bold;font-size:10px;color:black;'>" + lblTime.Text + " <br/> " + lblDate.Text + "</td></tr>");
            //        }
            //        string alignment = string.Empty;
            //        alignment = dsGetHeaderFooterDetail.Tables[0].Rows[0]["Alignment"].ToString();
            //        if (alignment.ToLower() == "standard" || alignment.ToLower() == "centered")
            //        {
            //            alignment = "center";
            //        }

            //        if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["CompanyName"].ToString() != "")
            //        {
            //            sbHeader.Append("<tr><td colspan='2' style='height:20px;text-align:" + alignment + ";color:Black;font-size:16px;font-weight:bold;'>" + lblCompanyName2.Text + "</td></tr>");
            //        }
            //        if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["ReportTitle"].ToString() != "")
            //        {
            //            sbHeader.Append("<tr><td colspan='2' style='height:20px;text-align:" + alignment + ";color:Black;font-size:14px;font-weight:bold;'>" + lblReportTitle.Text + "</td></tr>");
            //        }
            //        if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["SubTitle"].ToString() != "")
            //        {
            //            sbHeader.Append("<tr><td colspan='2' style='height:20px;text-align:" + alignment + ";color:Black;font-size:12px;font-weight:bold;'>" + lblSubTitle.Text + "</td></tr>");
            //        }
            //        sbHeader.Append("<tr><td colspan='2' style='height:15px;'>&nbsp;</td></tr>");
            //        sbHeader.Append("<tr><td colspan='2'>");

            //        if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["ExtraFooterLine"].ToString() != "")
            //        {
            //            sbExtraFooter.Append("<tr><td colspan='2' style='height:80px;text-align:center;color:Black;font-size:14px;font-weight:bold;'>" + dsGetHeaderFooterDetail.Tables[0].Rows[0]["ExtraFooterLine"].ToString() + "</td></tr>");
            //        }
            //    }
            //}

            //string html = hdnDivToExport.Value;

            //html = html.Replace("&gt;", ">");
            //html = html.Replace("&lt;", "<");

            //htmlToExport = sbHeader + "<br /> " + html + "</td></tr>" + sbExtraFooter + "</table></div></body></html>";

            //string excelServerPath = Server.MapPath("ReportFiles/Excel");
            //if (!Directory.Exists(excelServerPath))
            //{
            //    Directory.CreateDirectory(excelServerPath);
            //}
            //int userId = Convert.ToInt32(Session["UserID"].ToString());
            ////commented by Mayuri 20th july, 16
            ////string excelFileName = hdnCustomizeReportName.Value.Replace(" ", "") + "-" + userId + "-" + DateTime.Now.Ticks + ".xls";
            //string excelFileName = hdnCustomizeReportName.Value.Replace(" ", "") + DateTime.Now.ToString("MM-dd-yyyy") + ".xlsx";
            //string filePath = excelServerPath + "\\" + excelFileName;
            //FileStream fStream = new FileStream(filePath, FileMode.Create);
            //BinaryWriter BWriter = new BinaryWriter(fStream);
            //BWriter.Write(htmlToExport);
            //BWriter.Close();
            //fStream.Close();

            Session["FilePath"] = filePath;
        }
        catch
        {

        }
    }

    protected void ExportToExcel(object sender, EventArgs e)
    {
        SaveResizedAndReorderReport();
        GetReportColumnsByRepId();

        GetReportsName();
        if (pubReportId != 0)
        {
            drpReports.SelectedValue = pubReportId.ToString();
        }

        GenerateExcelFile();

        string script = String.Format("window.open('CustomerReportPreview.aspx');");
        this.Page.ClientScript.RegisterStartupScript(this.GetType(), "123", script, true);
    }

    private void DeletePDFFiles()
    {
        if (Directory.Exists(Server.MapPath("ReportFiles/PDF")))
        {
            string[] filePaths = Directory.GetFiles(Server.MapPath("ReportFiles/PDF"));
            foreach (string filePath in filePaths)
            {
                try
                {
                    File.Delete(filePath);
                }
                catch
                {
                    //
                }
            }
        }
    }

    private void DeleteExcelFiles()
    {
        string[] filePaths = Directory.GetFiles(Server.MapPath("ReportFiles/Excel"));
        string[] imagesPaths = Directory.GetFiles(Server.MapPath("ReportFiles/Images"));
        foreach (string filePath in filePaths)
        {
            try
            {
                File.Delete(filePath);
            }
            catch
            {
                //
            }
        }
    }
    protected void btnSaveReport2_Click(object sender, EventArgs e)
    {
        try
        {
            SaveResizedAndReorderReport();
            GetReportColumnsByRepId();
            if (pubReportId != 0)
            {
                drpReports.SelectedValue = pubReportId.ToString();
            }
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "noty({text: 'Report updated successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
        catch (Exception exp)
        {
            ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: '" + exp.Message.ToString() + "',  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
    }

    //Changed by Yashasvi Jadav
    //private void GetPreviewFields()
    //{
    //    Session["imgPreview"] = imgPreview;
    //    Session["lblPreviewCompName"] = lblPreviewCompanyName;
    //    Session["lblPreviewCompAddress"] = lblPreviewCompAddress;
    //    Session["lblPreviewCompEmail"] = lblPreviewCompEmail;
    //}

    [System.Web.Services.WebMethod]
    public static string GetVendorPreviewDetails(int reportId, string FilterColumn, string FilterValues, string ColumnWidth, string SortColumn, string DataSortBy)
    {
        try
        {
            if (DataSortBy == "1099" || DataSortBy == "1099 Asc")
            {
                DataSortBy = "1";
            }
            VendorListReport _a = new VendorListReport();
            BL_ReportsData objBL_ReportsData = new BL_ReportsData();
            BusinessEntity.User objProp_User = new BusinessEntity.User();
            CustomerReport objCustReport = new CustomerReport();
            DataSet dsCompDetail = new DataSet();
            objProp_User.DBName = HttpContext.Current.Session["dbname"].ToString();
            objProp_User.ConnConfig = HttpContext.Current.Session["config"].ToString();
            objCustReport.DBName = HttpContext.Current.Session["dbname"].ToString();
            objCustReport.ConnConfig = HttpContext.Current.Session["config"].ToString();
            dsCompDetail = objBL_ReportsData.GetControlForReports(objProp_User);
            string myJsonString = string.Empty;
            int pubReportId = 0;
            if (reportId != 0)
            {
                pubReportId = reportId;
            }
            objCustReport.ReportId = pubReportId;
            string lblDate = string.Empty;
            string lblTime = string.Empty;
            DataSet dsGetHeaderFooterDetail = new DataSet();

            #region Get Preview Header Details
            dsGetHeaderFooterDetail = objBL_ReportsData.GetHeaderFooterDetail(objCustReport);

            if (dsGetHeaderFooterDetail.Tables[0].Rows.Count > 0)
            {
                if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["DatePrepared"].ToString() != "")
                {
                    if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["DatePrepared"].ToString() == "12/31/01")
                    {
                        lblDate = DateTime.Now.Date.ToString("MM/dd/yy");
                    }
                    else if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["DatePrepared"].ToString() == "Dec 31, 01")
                    {
                        lblDate = DateTime.Now.Date.ToString("MMM dd, yy");
                    }
                    else if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["DatePrepared"].ToString() == "December 31, 01")
                    {
                        lblDate = DateTime.Now.Date.ToString("MMMM dd, yy");
                    }
                    else if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["DatePrepared"].ToString() == "Dec 31, 2001")
                    {
                        lblDate = DateTime.Now.Date.ToString("MMM dd, yyyy");
                    }
                    else if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["DatePrepared"].ToString() == "December 31, 2001")
                    {
                        lblDate = DateTime.Now.Date.ToString("MMMM dd, yyyy");
                    }
                    else
                    {
                        lblDate = DateTime.Now.Date.ToString("MM/dd/yyyy");
                    }
                }
                else
                {
                    lblDate = "";
                }

                if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["TimePrepared"].ToString() == "True")
                {
                    lblTime = DateTime.Now.ToString("hh:mm tt");
                }
                else
                {
                    lblTime = "";
                }

            }
            else
            {
                lblDate = DateTime.Now.Date.ToString("MM/dd/yyyy");
                lblTime = lblTime = DateTime.Now.ToString("hh:mm tt");
            }
            #endregion

            Dictionary<string, string> _compList = new Dictionary<string, string>();
            if (dsCompDetail.Tables[0].Rows.Count > 0)
            {
                #region Get Columns
                if (!Convert.IsDBNull(dsCompDetail.Tables[0].Rows[0]["Logo"]))
                {
                    byte[] compLogo = (byte[])dsCompDetail.Tables[0].Rows[0]["Logo"];
                    _compList.Add("Image", "data:image/png;base64," + Convert.ToBase64String(compLogo));
                }
                _compList.Add("Name", dsCompDetail.Tables[0].Rows[0]["Name"].ToString());
                _compList.Add("Email", dsCompDetail.Tables[0].Rows[0]["Email"].ToString());
                _compList.Add("Address", dsCompDetail.Tables[0].Rows[0]["Address"].ToString());
                _compList.Add("Date", lblDate);
                _compList.Add("Time", lblTime);
                myJsonString = (new JavaScriptSerializer()).Serialize(_compList);
                #endregion
            }

            string decodedFilterValues = string.IsNullOrEmpty(FilterValues) ? null : HttpUtility.HtmlDecode(FilterValues);
            string[] checkedColumns = string.IsNullOrEmpty(SortColumn) ? null : SortColumn.TrimEnd('^').Split('^');
            string[] selectedFiltersColumns = string.IsNullOrEmpty(FilterColumn) ? null : FilterColumn.TrimEnd('^').Split('^');
            string[] selectedFiltersValues = string.IsNullOrEmpty(decodedFilterValues) ? null : decodedFilterValues.TrimEnd('^').Split('^');

            #region Bind Grid Report
            string query = "SELECT distinct ";
            foreach (var item in checkedColumns)
            {
                if (item=="1099")
                {
                    query += "v.[" + item + "],";
                }
                else
                {
                    query += "v." + item + ",";
                }
                
            }

            query = query.Substring(0, query.Length - 1);
            if (selectedFiltersColumns == null)
            {
                query += " FROM VendorReportDetails v order by " + DataSortBy;
            }
            else
            {
                string filters = string.Empty;
                if (selectedFiltersColumns != null)
                {
                    for (int i = 0; i <= selectedFiltersColumns.Count() - 1; i++)
                    {
                        if (selectedFiltersColumns[i].ToLower() != "balance" && selectedFiltersColumns[i].ToLower() != "loc" && selectedFiltersColumns[i].ToLower() != "equip" && selectedFiltersColumns[i].ToLower() != "opencall" && selectedFiltersColumns[i].ToLower() != "equipmentprice" && selectedFiltersColumns[i].ToLower() != "equipmentcounts")
                        {
                            if (!selectedFiltersValues[i].Contains("'") && !selectedFiltersValues[i].Contains("|"))
                            {
                                filters += selectedFiltersColumns[i] + "=" + "'" + selectedFiltersValues[i] + "'" + " AND ";
                            }
                            else
                            {
                                int indexOfSingleQuote = selectedFiltersValues[i].IndexOf("'");
                                if (indexOfSingleQuote == 0)
                                {
                                    filters += selectedFiltersColumns[i] + " in (" + selectedFiltersValues[i].Replace('|', ',') + ")" + " AND ";
                                }
                                else
                                {
                                    filters += selectedFiltersColumns[i] + " in ('" + selectedFiltersValues[i].Replace('|', ',') + ")" + " AND ";
                                }
                            }
                        }
                        else
                        {
                            if (selectedFiltersValues[i].Contains("and"))
                            {
                                filters += selectedFiltersColumns[i] + selectedFiltersValues[i].Replace("and", "and " + selectedFiltersColumns[i] + "") + " AND ";
                            }
                            else
                            {
                                filters += selectedFiltersColumns[i] + selectedFiltersValues[i] + " AND ";
                            }
                        }
                    }
                }
                filters = filters.Substring(0, filters.Length - 4);
                query += " FROM VendorReportDetails v where " + filters + " order by " + DataSortBy;
            }
            #endregion

            #region Get Grid Data
            StringBuilder html = new StringBuilder();
            DataSet dsGetCustDetails = new DataSet();
            dsGetCustDetails = objBL_ReportsData.GetOwners(query, objProp_User);
            if (dsGetCustDetails.Tables[0].Rows.Count > 0)
            {
                #region Bind Report Table

                DataSet dsGetColumnWidth = new DataSet();

                string footer = string.Empty;
                //Table start.
                html.Append("<table id='tblResize' border = '0'>");

                //Building the Header row.
                html.Append("<thead><tr>");

                foreach (DataColumn column in dsGetCustDetails.Tables[0].Columns)
                {
                    html.Append("<th class='resize-header'>");
                    //html.Append("<th style='border:13px solid transparent;color:black;font-size:11px;width:150px; border-image: url(images/icons_big/list-bullet2.PNG) " + b + " '>");
                    //html.Append("<th style='border:1;color:black;font-size:11px;'>");
                    html.Append(column.ColumnName);
                    html.Append("</th>");
                }
                html.Append("</tr></thead>");


                //Building the Data rows.
                foreach (DataRow row in dsGetCustDetails.Tables[0].Rows)
                {
                    html.Append("<tr>");
                    foreach (DataColumn column in dsGetCustDetails.Tables[0].Columns)
                    {
                        html.Append("<td style='border:0;padding:10px 20px 3px 10px;color:black;'>");
                        html.Append(row[column.ColumnName]);
                        html.Append("</td>");
                    }
                    html.Append("</tr>");

                }

                // html.Append("<tr><td>&nbsp;</td><td>5000</td><td>5000</td><td>5000</td><td>5000</td><td>5000</td></tr>");

                for (int i = 0; i <= dsGetCustDetails.Tables[0].Columns.Count - 1; i++)
                {
                    if (dsGetCustDetails.Tables[0].Columns[i].ToString() == "Balance")
                    {
                        footer += "<td style='border:0;padding:20px 20px 3px 10px;color:black;font-weight:bold;'>" + dsGetCustDetails.Tables[0].Compute("SUM(Balance)", string.Empty).ToString() + "</td>";
                    }
                    else if (dsGetCustDetails.Tables[0].Columns[i].ToString() == "loc")
                    {
                        footer += "<td style='border:0;padding:20px 20px 3px 10px;color:black;font-weight:bold;'>" + dsGetCustDetails.Tables[0].Compute("SUM(loc)", string.Empty).ToString() + "</td>";
                    }
                    else if (dsGetCustDetails.Tables[0].Columns[i].ToString() == "equip")
                    {
                        footer += "<td style='border:0;padding:20px 20px 3px 10px;color:black;font-weight:bold;'>" + dsGetCustDetails.Tables[0].Compute("SUM(equip)", string.Empty).ToString() + "</td>";
                    }
                    else if (dsGetCustDetails.Tables[0].Columns[i].ToString() == "opencall")
                    {
                        footer += "<td style='border:0;padding:20px 20px 3px 10px;color:black;font-weight:bold;'>" + dsGetCustDetails.Tables[0].Compute("SUM(opencall)", string.Empty).ToString() + "</td>";
                    }
                    else
                    {
                        footer += "<td style='border:0;padding:10px 20px 3px 10px;color:black;'>&nbsp;</td>";
                    }
                }

                if (footer != "")
                {
                    html.Append("<tr>" + footer + "</tr>");
                }

                //Table end.
                html.Append("</table>");
                html.Append("<div><b>Total Counts: </b>" + dsGetCustDetails.Tables[0].Rows.Count + "</div>");
                #endregion
            }
            #endregion

            _compList.Add("PreviewData", html.ToString());
            myJsonString = (new JavaScriptSerializer()).Serialize(_compList);
            return myJsonString;
        }
        catch (Exception exp)
        {
            throw exp;
        }
    }
}

