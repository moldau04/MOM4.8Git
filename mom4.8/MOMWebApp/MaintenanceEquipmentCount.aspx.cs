﻿using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Stimulsoft.Report;
using Stimulsoft.Report.Web;
using System.Configuration;
using Microsoft.ApplicationBlocks.Data;
using System.IO;
using System.Text;
using System.Web;
using System.Collections;
using System.Net.Configuration;
using System.Collections.Generic;
using AjaxControlToolkit;
using BusinessEntity;
using BusinessLayer;
using Microsoft.Reporting.WebForms;
using Telerik.Web.UI;
using System.Web.Configuration;
using BusinessEntity.Payroll;
using MOMWebApp;
using BusinessEntity.Utility;
using System.Web.Script.Serialization;
using BusinessEntity.APModels;
using BusinessEntity.CustomersModel;

public partial class MaintenanceEquipmentCount : System.Web.UI.Page
{
    GeneralFunctions objgn = new GeneralFunctions();

    User objPropUser = new User();
    BL_User objBL_User = new BL_User();

    Customer objPropCustomer = new Customer();
    BL_Customer objBL_Customer = new BL_Customer();

    BL_Report bL_Report = new BL_Report();
    BL_Budgets bL_Budgets = new BL_Budgets();

    Chart objChart = new Chart();

    //API Variables
    string IsAPIIntegrationEnable = System.Web.Configuration.WebConfigurationManager.AppSettings["IsAPIIntegrationEnable"].Trim();
    getConnectionConfigParam _getConnectionConfig = new getConnectionConfigParam();
    GetCompanyDetailsParam _getCompanyDetails = new GetCompanyDetailsParam();
    GetSMTPByUserIDParam _getSMTPByUserID = new GetSMTPByUserIDParam();
    GetMaintenanceUnitCountRouteParam _GetMaintenanceUnitCountRoute = new GetMaintenanceUnitCountRouteParam();
    GetMaintenanceUnitCountParam _GetMaintenanceUnitCount = new GetMaintenanceUnitCountParam();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userid"] == null)
        {
            Response.Redirect("login.aspx");
        }

        if (!IsPostBack)
        {
            HighlightSideMenu("cstmMgr", "lnkEquipmentsSMenu", "cstmMgrSub");

            objChart.ConnConfig = Session["config"].ToString();
            _GetMaintenanceUnitCountRoute.ConnConfig = Session["config"].ToString();

            List<GetMaintenanceUnitCountRouteViewModel> _lstGetMaintenanceUnitCountRoute = new List<GetMaintenanceUnitCountRouteViewModel>();

            if (IsAPIIntegrationEnable == "YES")
            {
                string APINAME = "EquipmentAPI/EquipmentReport_GetMaintenanceUnitCountRoute";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetMaintenanceUnitCountRoute, true);
                JavaScriptSerializer serializer = new JavaScriptSerializer();

                serializer.MaxJsonLength = Int32.MaxValue;
                _lstGetMaintenanceUnitCountRoute = serializer.Deserialize<List<GetMaintenanceUnitCountRouteViewModel>>(_APIResponse.ResponseData);

                rcRoute.DataSource = CommonMethods.ToDataSet<GetMaintenanceUnitCountRouteViewModel>(_lstGetMaintenanceUnitCountRoute);
            }
            else
            {
                rcRoute.DataSource = bL_Report.GetMaintenanceUnitCountRoute(objChart);
            }

            rcRoute.DataTextField = "Area";
            rcRoute.DataValueField = "Area";
            rcRoute.DataBind();
            rcRoute.Items.Add("Unassigned");

            if (Request["routes"] != null)
            {
                StiWebViewerUnitCount.Visible = true;
            }
            else
            {
                StiWebViewerUnitCount.Visible = false;
            }

            if (!string.IsNullOrEmpty(Request["routes"]))
            {
                if (Request["routes"] == "all")
                {
                    foreach (RadComboBoxItem rcbItem in rcRoute.Items)
                    {
                        rcbItem.Checked = true;
                    }
                }
                else
                {
                    var routes = HttpUtility.UrlDecode(Request["routes"]).Replace("'", "").Split(',');
                    foreach (var route in routes)
                    {
                        var item = rcRoute.Items.FindItem(x => x.Text == route);
                        if (item != null)
                        {
                            item.Checked = true;
                        }
                    }
                }
            }

            GetSMTPUser();
            SetAddress();
            string FileName = "MaintenanceUnitCountReport.pdf";
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
        Response.Redirect("Equipments.aspx");
    }

    protected void lnkLoad_Click(object sender, EventArgs e)
    {
        StiWebViewerUnitCount.Visible = true;
        var routes = GetRadComboBoxSelectedItems(rcRoute);

        Response.Redirect("MaintenanceEquipmentCount.aspx?routes=" + HttpUtility.UrlEncode(routes));
    }

    protected void StiWebViewerUnitCount_GetReport(object sender, Stimulsoft.Report.Web.StiReportDataEventArgs e)
    {
        e.Report = LoadReport();
    }

    protected void StiWebViewerUnitCount_GetReportData(object sender, Stimulsoft.Report.Web.StiReportDataEventArgs e)
    {

    }

    private StiReport LoadReport()
    {
        try
        {
            string reportPathStimul = Server.MapPath("StimulsoftReports/MaintenanceUnitCountReport.mrt");
            if (!string.IsNullOrEmpty(WebConfigurationManager.AppSettings["MaintenanceUnitCountReport"]) && WebConfigurationManager.AppSettings["MaintenanceUnitCountReport"].ToLower().Contains(".mrt"))
            {
                reportPathStimul = Server.MapPath($"StimulsoftReports/{WebConfigurationManager.AppSettings["MaintenanceUnitCountReport"]}");
            }

            StiReport report = new StiReport();
            report.Load(reportPathStimul);
            //report.Compile();

            //Get data
            DataSet dsC = new DataSet();

            var connString = Session["config"].ToString();
            objPropUser.ConnConfig = connString;
            objChart.ConnConfig = connString;

            _getConnectionConfig.ConnConfig = connString;

            List<GetControlViewModel> _GetControlViewModel = new List<GetControlViewModel>();

            if (IsAPIIntegrationEnable == "YES")
            {
                string APINAME = "EquipmentAPI/EquipmentReport_GetControl";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getConnectionConfig, true);
                JavaScriptSerializer serializer = new JavaScriptSerializer();

                serializer.MaxJsonLength = Int32.MaxValue;
                _GetControlViewModel = serializer.Deserialize<List<GetControlViewModel>>(_APIResponse.ResponseData);
                dsC = CommonMethods.ToDataSet<GetControlViewModel>(_GetControlViewModel);
            }
            else
            {
                dsC = objBL_User.getControl(objPropUser);
            }

            DataSet companyInfo = new DataSet();

            List<GetCompanyDetailsViewModel> _GetCompanyDetailsViewModel = new List<GetCompanyDetailsViewModel>();

            if (IsAPIIntegrationEnable == "YES")
            {
                string APINAME = "EquipmentAPI/EquipmentReport_GetCompanyDetails";

                _getCompanyDetails.ConnConfig = Session["config"].ToString();

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getCompanyDetails, true);
                JavaScriptSerializer serializer = new JavaScriptSerializer();

                serializer.MaxJsonLength = Int32.MaxValue;
                _GetCompanyDetailsViewModel = serializer.Deserialize<List<GetCompanyDetailsViewModel>>(_APIResponse.ResponseData);
                companyInfo = CommonMethods.ToDataSet<GetCompanyDetailsViewModel>(_GetCompanyDetailsViewModel);
            }
            else
            {
                companyInfo = bL_Report.GetCompanyDetails(Session["config"].ToString());
            }


            report.RegData("CompanyDetails", companyInfo.Tables[0]);
            report.Dictionary.Variables["Username"].Value = Session["Username"].ToString();

            var routes = string.Empty;
            if (!string.IsNullOrEmpty(Request["routes"]))
            {
                routes = HttpUtility.UrlDecode(Request.QueryString["routes"]);
            }
            else if (Request["routes"] != null)
            {
                routes = "''";
            }

            DataSet ds = new DataSet();
            List<GetMaintenanceUnitCountViewModel> _lstGetMaintenanceUnitCount = new List<GetMaintenanceUnitCountViewModel>();

            if (IsAPIIntegrationEnable == "YES")
            {
                string APINAME = "EquipmentAPI/EquipmentReport_GetMaintenanceUnitCount";

                _GetMaintenanceUnitCount.routes = routes;

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetMaintenanceUnitCount, true);
                JavaScriptSerializer serializer = new JavaScriptSerializer();

                serializer.MaxJsonLength = Int32.MaxValue;
                _lstGetMaintenanceUnitCount = serializer.Deserialize<List<GetMaintenanceUnitCountViewModel>>(_APIResponse.ResponseData);
                ds = CommonMethods.ToDataSet<GetMaintenanceUnitCountViewModel>(_lstGetMaintenanceUnitCount);
            }
            else
            {
                ds = bL_Report.GetMaintenanceUnitCount(objChart, routes);
            }

            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                DataTable dtDetail = ds.Tables[0];

                report.Dictionary.Variables["TotalCount"].Value = dtDetail.AsEnumerable().Sum(r => Convert.ToInt32(r["UnitCount"])).ToString();

                DataTable dtFilter = GroupByAreaAndRouteTable();
                dtFilter = dtDetail.AsEnumerable()
                .Select(g =>
                {
                    var row = dtFilter.NewRow();
                    row["Area"] = g.Field<string>("Area");
                    row["Route"] = g.Field<string>("Route");
                    row["Cat"] = g.Field<string>("Cat");
                    row["UnitType"] = g.Field<string>("UnitType");
                    row["UnitCount"] = g.Field<int>("UnitCount");

                    return row;
                })
                .CopyToDataTable();

                DataTable dtFilterData = GroupByAreaAndRouteTable();
                dtFilterData = dtFilter.AsEnumerable()
                .GroupBy(r => new { Col1 = r["Cat"], Col2 = r["UnitType"] })
                .Select(g =>
                {
                    var row = dtFilter.NewRow();
                    row["Area"] = "";
                    row["Route"] = "";
                    row["Cat"] = g.Key.Col1;
                    row["UnitType"] = g.Key.Col2;
                    row["UnitCount"] = g.Sum(r => Convert.ToInt32(r["UnitCount"]));

                    return row;
                })
                .CopyToDataTable();

                // Route filter
                DataTable dtRouteFilterData = GroupByAreaAndRouteTable();
                dtRouteFilterData = dtFilter.AsEnumerable()
                .GroupBy(r => new { Col1 = r["Area"], Col2 = r["Route"], Col3 = r["Cat"], Col4 = r["UnitType"] })
                .Select(g =>
                {
                    var row = dtFilter.NewRow();
                    row["Area"] = g.Key.Col1;
                    row["Route"] = g.Key.Col2;
                    row["Cat"] = g.Key.Col3;
                    row["UnitType"] = g.Key.Col4;
                    row["UnitCount"] = g.Sum(r => Convert.ToInt32(r["UnitCount"]));

                    return row;
                })
                .CopyToDataTable();

                // Area filter
                DataTable dtAreaFilterData = GroupByAreaAndRouteTable();
                dtAreaFilterData = dtFilter.AsEnumerable()
                .GroupBy(r => new { Col1 = r["Area"], Col2 = r["Cat"], Col3 = r["UnitType"] })
                .Select(g =>
                {
                    var row = dtFilter.NewRow();
                    row["Area"] = g.Key.Col1;
                    row["Route"] = "";
                    row["Cat"] = g.Key.Col2;
                    row["UnitType"] = g.Key.Col3;
                    row["UnitCount"] = g.Sum(r => Convert.ToInt32(r["UnitCount"]));

                    return row;
                })
                .CopyToDataTable();

                report.RegData("ReportData", ds.Tables[0]);
                report.RegData("FilterData", dtFilterData);
                report.RegData("RouteFilterData", dtRouteFilterData);
                report.RegData("AreaFilterData", dtAreaFilterData);
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

        _getSMTPByUserID.ConnConfig = Session["config"].ToString();
        _getSMTPByUserID.UserID = Convert.ToInt32(Session["UserID"]);

        DataSet ds = new DataSet();

        List<SMTPEmailViewModel> _lstSMTPEmailViewModel = new List<SMTPEmailViewModel>();

        if (IsAPIIntegrationEnable == "YES")
        {
            string APINAME = "EquipmentAPI/EquipmentReport_GetSMTPByUserID";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getSMTPByUserID, true);
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            serializer.MaxJsonLength = Int32.MaxValue;
            _lstSMTPEmailViewModel = serializer.Deserialize<List<SMTPEmailViewModel>>(_APIResponse.ResponseData);
            ds = CommonMethods.ToDataSet<SMTPEmailViewModel>(_lstSMTPEmailViewModel);
        }
        else
        {
            ds = objBL_User.getSMTPByUserID(objPropUser);
        }

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

                mail.Title = "Maintenance Unit Count Report";
                if (txtBody.Text.Trim() != string.Empty)
                {
                    mail.Text = txtBody.Text.Replace(Environment.NewLine, "<BR/>");
                }
                else
                {
                    mail.Text = "This is report email sent from Mobile Office Manager. Please find the Maintenance Unit Count Report attached.";
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
                        if (strpath != "MaintenanceUnitCountReport.pdf")
                        {
                            mail.AttachmentFiles.Add(strpath);
                        }
                    }
                }

                mail.FileName = "MaintenanceUnitCountReport.pdf";

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
            if (DownloadFileName == "MaintenanceUnitCountReport.pdf")
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
                Response.AddHeader("content-disposition", "attachment;filename=MaintenanceUnitCountReport.pdf");
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

        string mailBody = "Please review the attached Maintenance Unit Count Report.";
        address = mailBody + Environment.NewLine + "<br />" + Environment.NewLine + "<br />" + address;

        txtBody.Text = address;

        
    }

    protected DataTable GroupByAreaAndRouteTable()
    {
        DataTable table = new DataTable();
        table.Columns.Add("Area");
        table.Columns.Add("Route");
        table.Columns.Add("Cat");
        table.Columns.Add("UnitType");
        table.Columns.Add("UnitCount");

        return table;
    }

    private string GetRadComboBoxSelectedItems(RadComboBox radComboBox)
    {
        if (radComboBox.CheckedItems.Count == radComboBox.Items.Count)
        {
            return "all";
        }
        else
        {
            int itemschecked = radComboBox.CheckedItems.Count;
            String[] serviceTypesArray = new String[itemschecked];

            var collection = radComboBox.CheckedItems;
            int i = 0;
            foreach (var item in collection)
            {
                String value = string.Format("'{0}'", item.Text);
                serviceTypesArray[i] = value;
                i++;
            }

            return String.Join(",", serviceTypesArray);
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
}
