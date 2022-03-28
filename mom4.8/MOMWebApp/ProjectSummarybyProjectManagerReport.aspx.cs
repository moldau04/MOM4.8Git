using BusinessEntity;
using BusinessLayer;
using Stimulsoft.Report;
using Stimulsoft.Report.Web;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Configuration;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace MOMWebApp
{
    public partial class ProjectSummarybyProjectManagerReport : System.Web.UI.Page
    {
        GeneralFunctions objgn = new GeneralFunctions();

        User objPropUser = new User();
        BL_User objBL_User = new BL_User();

        Vendor objVendor = new Vendor();
        BL_Report objBL_Report = new BL_Report();

        Customer objPropCustomer = new Customer();
        BL_Customer objBL_Customer = new BL_Customer();

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
                    FillDepartment();

                    if (!string.IsNullOrEmpty(Request.QueryString["show"]))
                    {
                        StiWebViewerProject.Visible = true;
                    }

                    if (Request.QueryString["depts"] != null && !string.IsNullOrEmpty(Request.QueryString["depts"]))
                    {
                        var depts = HttpUtility.UrlDecode(Request.QueryString["depts"].Trim());
                        var deptArray = depts.Split(',');

                        for (int i = 0; i < deptArray.Length; i++)
                        {
                            RadComboBoxItem item = rcDepartment.FindItemByValue(deptArray[i]);
                            if (item != null)
                                item.Checked = true;
                        }
                    }
                    else if (Session["ProjectDepartmentFilters"] != null)
                    {
                        var Departments = (string[])Session["ProjectDepartmentFilters"];

                        for (int i = 0; i < Departments.Length; i++)
                        {
                            RadComboBoxItem item = rcDepartment.FindItemByText(Departments[i]);
                            if (item != null)
                            {
                                item.Checked = true;
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(Request.QueryString["close"]))
                    {
                        chkIncludeClose.Checked = Convert.ToBoolean(Request.QueryString["close"]);
                    }

                    HighlightSideMenu("ProjectMgr", "lnkProject", "ProjectMgrSub");

                    GetSMTPUser();
                    SetAddress();
                    string FileName = "ProjectSummarybyProjectManagerReport.pdf";
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

        protected void StiWebViewerProject_GetReport(object sender, StiReportDataEventArgs e)
        {
            e.Report = LoadReport();
        }

        protected void StiWebViewerProject_GetReportData(object sender, StiReportDataEventArgs e)
        {

        }

        protected void lnkClose_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["page"]) && Request.QueryString["page"] == "addProject")
            {
                var projectID = 0;
                int.TryParse(Request.QueryString["sv"], out projectID);

                if (projectID > 0)
                {
                    Response.Redirect("addProject.aspx?uid=" + projectID);
                }
            }

            Response.Redirect("project.aspx?fil=1");
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Session.Remove("ProjectDepartmentFilters");
            string depts = string.Empty;

            List<string> selectedItem = new List<string>();
            foreach (RadComboBoxItem item in rcDepartment.Items)
            {
                if (item.Checked == true)
                {
                    selectedItem.Add(item.Value);
                }
            }

            if (selectedItem.Count > 0)
            {
                depts = string.Join(",", selectedItem);
            }

            var url = Request.Url.ToString();
            url = RemoveQueryStringByKey(url, "depts");
            url = RemoveQueryStringByKey(url, "close");
            url = RemoveQueryStringByKey(url, "show");
            url += "&depts=" + depts + "&close=" + chkIncludeClose.Checked + "&show=true";

            Response.Redirect(url);
        }

        protected void lnkProjectSummarybyProjectManagerReport_Click(object sender, EventArgs e)
        {
            try
            {
                Customer objPropCustomer = new Customer();
                objPropCustomer.ConnConfig = Session["config"].ToString();
                objPropCustomer.UserID = Convert.ToInt32(System.Web.HttpContext.Current.Session["UserID"].ToString());

                if (HttpContext.Current.Session["CmpChkDefault"].ToString() == "1")
                {
                    objPropCustomer.EN = 1;
                }
                else
                {
                    objPropCustomer.EN = 0;
                }

                if (!string.IsNullOrEmpty(Request.QueryString["sb"]))
                {
                    objPropCustomer.SearchBy = Request.QueryString["sb"];
                }

                if (!string.IsNullOrEmpty(Request.QueryString["sv"]))
                {
                    objPropCustomer.SearchValue = Request.QueryString["sv"];
                }

                if (!string.IsNullOrEmpty(Request.QueryString["sMember"]))
                {
                    objPropCustomer.Username = Request.QueryString["sMember"];
                }

                if (!string.IsNullOrEmpty(Request.QueryString["df"]))
                {
                    var startDate = Convert.ToDateTime(Request.QueryString["df"]);
                    objPropCustomer.StartDate = startDate.ToShortDateString();
                }

                if (!string.IsNullOrEmpty(Request.QueryString["dt"]))
                {
                    var endDate = Convert.ToDateTime(Request.QueryString["dt"]);
                    objPropCustomer.EndDate = endDate.ToShortDateString();
                }

                if (!string.IsNullOrEmpty(Request.QueryString["rng"]))
                {
                    objPropCustomer.Range = Convert.ToInt16(Request.QueryString["rng"]);
                }

                int includeClose = 0;
                if (!string.IsNullOrEmpty(Request.QueryString["close"]))
                {
                    if (Convert.ToBoolean(Request.QueryString["close"]))
                    {
                        includeClose = 1;
                    }
                }

                string depts = string.Empty;
                if (Request.QueryString["depts"] != null && !string.IsNullOrEmpty(Request.QueryString["depts"]))
                {
                    depts = HttpUtility.UrlDecode(Request.QueryString["depts"].Trim());
                }

                // Get grid filter
                List<RetainFilter> filters = new List<RetainFilter>();
                if (Session["GridFilters"] != null)
                {
                    filters = (List<RetainFilter>)Session["GridFilters"];
                }

                var ds = objBL_Report.GetProjectSummary(objPropCustomer, filters, depts, includeClose, true);


                var data = ProcessingData(ds.Tables[0], ds.Tables[1]);


                //DataTable dt2 = ds.Tables[1];
                //dt.Merge(dt2);
                //dt.AcceptChanges();


                var tempFile = string.Format("ProjectSummarybyProjectManagerReport{0}.xlsx", DateTime.UtcNow.ToString("yyyyMMddHHmmssfff"));
                var fileName = Server.MapPath(string.Format("ReportFiles/ExportExcel/{0}", tempFile));

                ExportToOxml(true, fileName, data);

                var strFileName = fileName;
                if (strFileName.Length > 0)
                {
                    try
                    {
                        DownloadDocument(strFileName, "ProjectSummarybyProjectManagerReport.xlsx");
                        // Delete after downloaded
                        if (File.Exists(strFileName))
                            File.Delete(strFileName);
                    }
                    catch (Exception)
                    {
                        // Delete after downloaded
                        if (File.Exists(strFileName))
                            File.Delete(strFileName);
                    }
                }
                else
                {
                    string str = "Export failed!";
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErrExportExcel", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false,dismissQueue: true, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private StiReport LoadReport()
        {
            try
            {
                string reportPathStimul = Server.MapPath("StimulsoftReports/Project/ProjectSummarybyProjectManagerReport.mrt");
                //("StimulsoftReports/ProjectSummarybyProjectManagerReport.mrt");
                //("StimulsoftReports/Project/ProjectSummaryReport-Gable.mrt");


                StiReport report = new StiReport();
                report.Load(reportPathStimul);
                //report.Compile();

                //Get data
                var connString = Session["config"].ToString();
                objPropUser.ConnConfig = connString;

                DataSet companyInfo = new DataSet();
                companyInfo = objBL_Report.GetCompanyDetails(Session["config"].ToString());

                report.RegData("CompanyDetails", companyInfo.Tables[0]);

                objPropCustomer.ConnConfig = connString;
                objPropCustomer.UserID = Convert.ToInt32(System.Web.HttpContext.Current.Session["UserID"].ToString());
                if (HttpContext.Current.Session["CmpChkDefault"].ToString() == "1")
                {
                    objPropCustomer.EN = 1;
                }
                else
                {
                    objPropCustomer.EN = 0;
                }

                if (!string.IsNullOrEmpty(Request.QueryString["sb"]))
                {
                    objPropCustomer.SearchBy = Request.QueryString["sb"];
                }

                if (!string.IsNullOrEmpty(Request.QueryString["sv"]))
                {
                    objPropCustomer.SearchValue = Request.QueryString["sv"];
                }

                if (!string.IsNullOrEmpty(Request.QueryString["sMember"]))
                {
                    objPropCustomer.Username = Request.QueryString["sMember"];
                }

                if (!string.IsNullOrEmpty(Request.QueryString["df"]))
                {
                    var startDate = Convert.ToDateTime(Request.QueryString["df"]);
                    objPropCustomer.StartDate = startDate.ToShortDateString();
                }

                if (!string.IsNullOrEmpty(Request.QueryString["dt"]))
                {
                    var endDate = Convert.ToDateTime(Request.QueryString["dt"]);
                    objPropCustomer.EndDate = endDate.ToShortDateString();
                }

                if (!string.IsNullOrEmpty(Request.QueryString["rng"]))
                {
                    objPropCustomer.Range = Convert.ToInt16(Request.QueryString["rng"]);
                }

                int includeClose = 0;
                if (!string.IsNullOrEmpty(Request.QueryString["close"]))
                {
                    if (Convert.ToBoolean(Request.QueryString["close"]))
                    {
                        includeClose = 1;
                    }
                }

                // Get grid filter
                List<RetainFilter> filters = new List<RetainFilter>();
                if (Session["GridFilters"] != null)
                {
                    filters = (List<RetainFilter>)Session["GridFilters"];
                }

                string depts = string.Empty;
                if (Request.QueryString["depts"] != null && !string.IsNullOrEmpty(Request.QueryString["depts"]))
                {
                    depts = HttpUtility.UrlDecode(Request.QueryString["depts"].Trim());
                }
                else if (Session["ProjectDepartmentFilters"] != null)
                {
                    var depArray = (string[])Session["ProjectDepartmentFilters"];
                    var departments = string.Join(",", depArray.Select(x => $"'{x}'"));

                    filters.Add(new RetainFilter { FilterColumn = "DepartmentList", FilterValue = departments });
                }

                var isDBTotalService = false;
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["isDBTotalService"]))
                {
                    isDBTotalService = Convert.ToBoolean(ConfigurationManager.AppSettings["isDBTotalService"]);
                }

                var ds = objBL_Report.GetProjectSummary(objPropCustomer, filters, depts, includeClose, false, isDBTotalService);
                if (ds != null)
                {
                    var data = ProcessingData(ds.Tables[0], ds.Tables[1]);
                    report.RegData("ReportData", data);
                }

                report.Dictionary.Variables["Username"].Value = Session["Username"].ToString();
                report.Dictionary.Variables["StartDate"].Value = objPropCustomer.StartDate;
                report.Dictionary.Variables["EndDate"].Value = objPropCustomer.EndDate;
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

        private DataTable ProcessingData(DataTable dtProject, DataTable dtARAging)
        {
            dtProject.Columns.Add("Balance");
            dtProject.Columns.Add("CurrentDay");
            dtProject.Columns.Add("ThirtyDay");
            dtProject.Columns.Add("SixtyDay");
            dtProject.Columns.Add("NintyDay");
            dtProject.Columns.Add("OverNintyDay");
            dtProject.Columns.Add("OneTwentyDay");

            foreach (DataRow dr in dtProject.Rows)
            {
                var drAR = dtARAging.AsEnumerable().Where(r => r.Field<int>("Loc") == dr.Field<int>("Loc")).FirstOrDefault();
                if (drAR != null)
                {
                    dr["Balance"] = drAR["Balance"];
                    dr["CurrentDay"] = drAR["CurrentDay"];
                    dr["ThirtyDay"] = drAR["ThirtyDay"];
                    dr["SixtyDay"] = drAR["SixtyDay"];
                    dr["NintyDay"] = drAR["NintyDay"];
                    dr["OverNintyDay"] = drAR["OverNintyDay"];
                    dr["OneTwentyDay"] = drAR["OneTwentyDay"];
                }
            }

            return dtProject;
        }

        //new
        private DataTable ProcessingData1(DataTable dtProject, DataTable dtARAging)
        {
            dtProject.Columns.Add("Balance11");
            dtProject.Columns.Add("CurrentDay");
            dtProject.Columns.Add("ThirtyDay");
            dtProject.Columns.Add("SixtyDay");
            dtProject.Columns.Add("NintyDay");
            dtProject.Columns.Add("OverNintyDay");
            dtProject.Columns.Add("OneTwentyDay");

            //foreach (DataRow dr in dtProject.Rows)
            //{
            //    var drAR = dtARAging;//.AsEnumerable().Where(r => r.Field<int>("Loc") == dr.Field<int>("Loc")).FirstOrDefault();
            //                         //if (drAR != null)
            //                         //{
            //    dr["Balance11"] = dtARAging.Rows["Balance"];//drAR["Balance"];
            //        dr["CurrentDay"] = dtARAging.Rows["CurrentDay"];
            //        dr["ThirtyDay"] = dtARAging.Rows["ThirtyDay"];
            //        dr["SixtyDay"] = dtARAging.Rows["SixtyDay"];
            //        dr["NintyDay"] = dtARAging.Rows["NintyDay"];
            //        dr["OverNintyDay"] = dtARAging.Rows["OverNintyDay"];
            //        dr["OneTwentyDay"] = dtARAging.Rows["OneTwentyDay"];
            //    //}
            //}

            return dtProject;
        }

        private void FillDepartment()
        {
            try
            {
                BusinessEntity.User objPropUser = new BusinessEntity.User();
                BL_User objBL_User = new BL_User();
                DataSet ds = new DataSet();
                objPropUser.ConnConfig = Session["config"].ToString();

                ds = objBL_User.getDepartment(objPropUser);

                rcDepartment.DataSource = ds.Tables[0];
                rcDepartment.DataTextField = "type";
                rcDepartment.DataValueField = "id";
                rcDepartment.DataBind();
            }
            catch (Exception ex)
            {
                string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
                ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
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

                    mail.Title = "Project Summary Report";
                    if (txtBody.Text.Trim() != string.Empty)
                    {
                        mail.Text = txtBody.Text.Replace(Environment.NewLine, "<BR/>");
                    }
                    else
                    {
                        mail.Text = "This is report email sent from Mobile Office Manager. Please find the Project Summary Report attached.";
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
                            if (strpath != "ProjectSummarybyProjectManagerReport.pdf")
                            {
                                mail.AttachmentFiles.Add(strpath);
                            }
                        }
                    }

                    mail.FileName = "ProjectSummaryReport.pdf";

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
                if (DownloadFileName == "ProjectSummarybyProjectManagerReport.pdf")
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
                    Response.AddHeader("content-disposition", "attachment;filename=ProjectSummaryReport.pdf");
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
            DataSet dsC = new DataSet();
            objPropUser.ConnConfig = Session["config"].ToString();

            dsC = objBL_User.getControl(objPropUser);

            if (dsC.Tables[0].Rows.Count > 0)
            {
                string address = string.Empty;
                if (!string.IsNullOrEmpty(Convert.ToString(dsC.Tables[0].Rows[0]["name"])))
                {
                    address += dsC.Tables[0].Rows[0]["name"].ToString() + Environment.NewLine + "</br>";
                }
                if (!string.IsNullOrEmpty(Convert.ToString(dsC.Tables[0].Rows[0]["Address"])))
                {
                    address += dsC.Tables[0].Rows[0]["Address"].ToString() + Environment.NewLine;
                }
                if (!string.IsNullOrEmpty(Convert.ToString(dsC.Tables[0].Rows[0]["city"])))
                {
                    address += dsC.Tables[0].Rows[0]["city"].ToString() + ", ";
                }
                if (!string.IsNullOrEmpty(Convert.ToString(dsC.Tables[0].Rows[0]["state"])))
                {
                    address += dsC.Tables[0].Rows[0]["state"].ToString() + ", ";
                }
                if (!string.IsNullOrEmpty(Convert.ToString(dsC.Tables[0].Rows[0]["zip"])))
                {
                    address += dsC.Tables[0].Rows[0]["zip"].ToString() + Environment.NewLine + "</br>";
                }
                if (!string.IsNullOrEmpty(Convert.ToString(dsC.Tables[0].Rows[0]["Phone"])))
                {
                    address += "Phone: " + dsC.Tables[0].Rows[0]["Phone"].ToString() + Environment.NewLine + "</br>";
                }
                if (!string.IsNullOrEmpty(Convert.ToString(dsC.Tables[0].Rows[0]["fax"])))
                {
                    address += "Fax: " + dsC.Tables[0].Rows[0]["fax"].ToString() + Environment.NewLine + "</br>";
                }
                if (!string.IsNullOrEmpty(Convert.ToString(dsC.Tables[0].Rows[0]["email"])))
                {
                    if (!ConfigurationManager.AppSettings["CustomerName"].ToString().Equals("QAE"))
                        address += "Email: " + dsC.Tables[0].Rows[0]["email"].ToString() + Environment.NewLine + "<br />";
                }
                string mailBody = "Please review the attached Project Summary Report.";
                address = mailBody + Environment.NewLine + "<br />" + Environment.NewLine + "<br />" + address;

                txtBody.Text = address;

            }
        }

        public string RemoveQueryStringByKey(string url, string key)
        {
            var uri = new Uri(url);

            // this gets all the query string key value pairs as a collection
            var newQueryString = HttpUtility.ParseQueryString(uri.Query);

            // this removes the key if exists
            newQueryString.Remove(key);

            // this gets the page path from root without QueryString
            string pagePathWithoutQueryString = uri.GetLeftPart(UriPartial.Path);

            return newQueryString.Count > 0
                ? String.Format("{0}?{1}", pagePathWithoutQueryString, newQueryString)
                : pagePathWithoutQueryString;
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

        private void ExportToOxml(bool firstTime, string fileName, DataTable ResultsData)
        {
            //Delete the file if it exists. 
            if (firstTime && File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            uint sheetId = 1; //Start at the first sheet in the Excel workbook.

            if (firstTime)
            {
                //This is the first time of creating the excel file and the first sheet.
                // Create a spreadsheet document by supplying the filepath.
                // By default, AutoSave = true, Editable = true, and Type = xlsx.
                DocumentFormat.OpenXml.Packaging.SpreadsheetDocument spreadsheetDocument = DocumentFormat.OpenXml.Packaging.SpreadsheetDocument.
                    Create(fileName, DocumentFormat.OpenXml.SpreadsheetDocumentType.Workbook);

                // Add a WorkbookPart to the document.
                DocumentFormat.OpenXml.Packaging.WorkbookPart workbookpart = spreadsheetDocument.AddWorkbookPart();
                workbookpart.Workbook = new DocumentFormat.OpenXml.Spreadsheet.Workbook();

                // Add a WorksheetPart to the WorkbookPart.
                var worksheetPart = workbookpart.AddNewPart<DocumentFormat.OpenXml.Packaging.WorksheetPart>();
                var sheetData = new DocumentFormat.OpenXml.Spreadsheet.SheetData();
                worksheetPart.Worksheet = new DocumentFormat.OpenXml.Spreadsheet.Worksheet(sheetData);


                var bold1 = new DocumentFormat.OpenXml.Spreadsheet.Bold();
                DocumentFormat.OpenXml.Spreadsheet.CellFormat cf = new DocumentFormat.OpenXml.Spreadsheet.CellFormat();


                // Add Sheets to the Workbook.
                DocumentFormat.OpenXml.Spreadsheet.Sheets sheets;
                sheets = spreadsheetDocument.WorkbookPart.Workbook.
                    AppendChild<DocumentFormat.OpenXml.Spreadsheet.Sheets>(new DocumentFormat.OpenXml.Spreadsheet.Sheets());

                // Append a new worksheet and associate it with the workbook.
                var sheet = new DocumentFormat.OpenXml.Spreadsheet.Sheet()
                {
                    Id = spreadsheetDocument.WorkbookPart.
                        GetIdOfPart(worksheetPart),
                    SheetId = sheetId,
                    Name = "Sheet" + sheetId
                };
                sheets.Append(sheet);

                //Add Header Row.
                var headerRow = new DocumentFormat.OpenXml.Spreadsheet.Row();
                foreach (DataColumn column in ResultsData.Columns)
                {
                    var cell = new DocumentFormat.OpenXml.Spreadsheet.Cell { DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.String, CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(column.ColumnName) };

                    cell.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(column.ColumnName);
                    headerRow.AppendChild(cell);
                }
                sheetData.AppendChild(headerRow);

                foreach (DataRow row in ResultsData.Rows)
                {
                    var newRow = new DocumentFormat.OpenXml.Spreadsheet.Row();
                    foreach (DataColumn col in ResultsData.Columns)
                    {
                        var cell = new DocumentFormat.OpenXml.Spreadsheet.Cell();

                        if (col.ColumnName.ToUpper() == "PROJECT#")
                        {
                            cell.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.Number;
                        }
                        else
                        {
                            if (col.DataType == typeof(Decimal)
                                || col.DataType == typeof(Double)
                                || col.DataType == typeof(Int16)
                                || col.DataType == typeof(Int32)
                                || col.DataType == typeof(Int64)
                                || col.DataType == typeof(Single)
                                || col.DataType == typeof(UInt16)
                                || col.DataType == typeof(UInt32)
                                || col.DataType == typeof(UInt64)
                                )
                                cell.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.Number;
                            else
                                cell.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.String;
                        }

                        if (col.DataType == typeof(DateTime))
                        {
                            if (row[col] != null && !string.IsNullOrEmpty(row[col].ToString()))
                            {
                                cell.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(((DateTime)row[col]).ToString("MM/dd/yyyy HH:mm tt"));
                            }
                        }
                        else
                        {
                            cell.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(row[col].ToString());
                        }

                        newRow.AppendChild(cell);
                    }

                    sheetData.AppendChild(newRow);
                }
                workbookpart.Workbook.Save();

                spreadsheetDocument.Close();
            }
            else
            {
                // Open the Excel file that we created before, and start to add sheets to it.
                var spreadsheetDocument = DocumentFormat.OpenXml.Packaging.SpreadsheetDocument.Open(fileName, true);

                var workbookpart = spreadsheetDocument.WorkbookPart;
                if (workbookpart.Workbook == null)
                    workbookpart.Workbook = new DocumentFormat.OpenXml.Spreadsheet.Workbook();

                var worksheetPart = workbookpart.AddNewPart<DocumentFormat.OpenXml.Packaging.WorksheetPart>();
                var sheetData = new DocumentFormat.OpenXml.Spreadsheet.SheetData();
                worksheetPart.Worksheet = new DocumentFormat.OpenXml.Spreadsheet.Worksheet(sheetData);
                var sheets = spreadsheetDocument.WorkbookPart.Workbook.Sheets;

                if (sheets.Elements<DocumentFormat.OpenXml.Spreadsheet.Sheet>().Any())
                {
                    //Set the new sheet id
                    sheetId = sheets.Elements<DocumentFormat.OpenXml.Spreadsheet.Sheet>().Max(s => s.SheetId.Value) + 1;
                }
                else
                {
                    sheetId = 1;
                }

                // Append a new worksheet and associate it with the workbook.
                var sheet = new DocumentFormat.OpenXml.Spreadsheet.Sheet()
                {
                    Id = spreadsheetDocument.WorkbookPart.
                        GetIdOfPart(worksheetPart),
                    SheetId = sheetId,
                    Name = "Sheet" + sheetId
                };
                sheets.Append(sheet);

                //Add the header row here.
                var headerRow = new DocumentFormat.OpenXml.Spreadsheet.Row();

                foreach (DataColumn column in ResultsData.Columns)
                {
                    var cell = new DocumentFormat.OpenXml.Spreadsheet.Cell { DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.String, CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(column.ColumnName) };

                    cell.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(column.ColumnName);
                    headerRow.AppendChild(cell);
                }
                sheetData.AppendChild(headerRow);

                foreach (DataRow row in ResultsData.Rows)
                {
                    var newRow = new DocumentFormat.OpenXml.Spreadsheet.Row();

                    foreach (DataColumn col in ResultsData.Columns)
                    {
                        var cell = new DocumentFormat.OpenXml.Spreadsheet.Cell();

                        if (col.ColumnName.ToUpper() == "PROJECT#")
                        {
                            cell.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.Number;
                        }
                        else
                        {
                            if (col.DataType == typeof(Decimal)
                                || col.DataType == typeof(Double)
                                || col.DataType == typeof(Int16)
                                || col.DataType == typeof(Int32)
                                || col.DataType == typeof(Int64)
                                || col.DataType == typeof(Single)
                                || col.DataType == typeof(UInt16)
                                || col.DataType == typeof(UInt32)
                                || col.DataType == typeof(UInt64)
                                )
                                cell.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.Number;
                            else
                                cell.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.String;
                        }

                        if (col.DataType == typeof(DateTime))
                        {
                            if (row[col] != null && !string.IsNullOrEmpty(row[col].ToString()))
                            {
                                cell.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(((DateTime)row[col]).ToString("MM/dd/yyyy HH:mm tt"));
                            }
                        }
                        else
                        {
                            cell.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(row[col].ToString());
                        }
                        newRow.AppendChild(cell);
                    }

                    sheetData.AppendChild(newRow);
                }

                workbookpart.Workbook.Save();

                // Close the document.
                spreadsheetDocument.Close();
            }
        }


    }
}