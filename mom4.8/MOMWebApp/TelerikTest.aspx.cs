using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MOMWebApp
{
    public partial class TelerikTest : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            System.Threading.Thread.Sleep(3000);
            Label1.Text = DateTime.Now.ToString();
        }

        /////////////////////////////////////////////

        //protected void RadGrid_Emails_uc_PreRender(object sender, EventArgs e)
        //{
        //    String filterExpression = Convert.ToString(RadGrid_Emails_uc.MasterTableView.FilterExpression);
        //    if (filterExpression != "")
        //    {
        //        Session["Emails_uc_FilterExpression"] = filterExpression;
        //        List<RetainFilter> filters = new List<RetainFilter>();

        //        foreach (GridColumn column in RadGrid_Emails_uc.MasterTableView.OwnerGrid.Columns)
        //        {
        //            String filterValues = column.CurrentFilterValue;
        //            if (filterValues != "")
        //            {
        //                String columnName = column.UniqueName;
        //                RetainFilter filter = new RetainFilter();
        //                filter.FilterColumn = columnName;
        //                filter.FilterValue = filterValues;
        //                filters.Add(filter);
        //            }
        //        }

        //        Session["Emails_uc_Filters"] = filters;
        //    }
        //    else
        //    {
        //        Session["Emails_uc_FilterExpression"] = null;
        //        Session["Emails_uc_Filters"] = null;
        //    }

        //    ScriptManager.RegisterStartupScript(this, Page.GetType(), "bindingClickCheckbox_uc", "BindClickEventForGridCheckBox_uc();", true);
        //}

        //protected void RadGrid_Emails_uc_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        //{
        //    RadGrid_Emails_uc.AllowCustomPaging = !ShouldApplySortFilterOrGroup();
        //    if (!IsPostBack)
        //    {
        //        if (Session["Emails_uc_FilterExpression"] != null && Convert.ToString(Session["Emails_uc_FilterExpression"]) != "" && Session["Emails_uc_Filters"] != null)
        //        {
        //            RadGrid_Emails_uc.MasterTableView.FilterExpression = Convert.ToString(Session["Emails_uc_FilterExpression"]);
        //            var filtersGet = Session["Emails_Filters"] as List<RetainFilter>;
        //            if (filtersGet != null)
        //            {
        //                foreach (var _filter in filtersGet)
        //                {
        //                    GridColumn column = RadGrid_Emails_uc.MasterTableView.GetColumnSafe(_filter.FilterColumn);
        //                    column.CurrentFilterValue = _filter.FilterValue;
        //                }
        //            }
        //        }
        //    }

        //    InitTeamMemberGridView_uc();
        //    //FillDistributionList("", "");
        //}

        //private void InitTeamMemberGridView_uc()
        //{
        //    User objPropUser = new User();
        //    DataSet ds = new DataSet();
        //    objPropUser.ConnConfig = Session["config"].ToString();
        //    //objPropUser.Status = 0;
        //    // ds = objBL_User.GetUsersForTeamMemberList(objPropUser);
        //    ds = objBL_User.GetUsersAndRolesForTeamMemberList(objPropUser);
        //    var teamMembers = ds.Tables[0];

        //    // Get contacts list from exchange server
        //    //DataTable contactList = GetContactsForExchServer();
        //    DataTable contactList = WebBaseUtility.GetContactListOnExchangeServer();
        //    if (contactList != null && contactList.Rows.Count > 0)
        //    {
        //        // Merge this list to teamMembers
        //        foreach (DataRow item in contactList.Rows)
        //        {
        //            DataRow _dr = teamMembers.NewRow();
        //            if (string.IsNullOrEmpty(item["GroupName"].ToString()))
        //            {
        //                _dr["memberkey"] = "3_" + item["Type"] + "|" + item["MemberEmail"] + "|" + item["MemberName"];
        //                _dr["usertype"] = "Exchange " + item["Type"];
        //            }
        //            else
        //            {
        //                _dr["memberkey"] = "4_" + item["GroupName"] + "|" + item["MemberEmail"] + "|" + item["MemberName"];
        //                _dr["usertype"] = "Exchange Group: " + item["GroupName"];
        //            }
        //            _dr["fUser"] = item["MemberName"];
        //            _dr["email"] = item["MemberEmail"];
        //            _dr["roleName"] = "";
        //            teamMembers.Rows.Add(_dr);
        //        }
        //    }
        //    ViewState["AllProjectTeamMemberList"] = teamMembers;
        //    RadGrid_Emails_uc.DataSource = teamMembers;
        //    if (teamMembers != null && teamMembers.Rows.Count > 0)
        //    {
        //        RadGrid_Emails_uc.VirtualItemCount = teamMembers.Rows.Count;
        //    }
        //    else
        //    {
        //        RadGrid_Emails_uc.VirtualItemCount = 0;
        //    }
        //}

        //public bool ShouldApplySortFilterOrGroup()
        //{
        //    return RadGrid_Emails_uc.MasterTableView.FilterExpression != "" ||
        //        (RadGrid_Emails_uc.MasterTableView.GroupByExpressions.Count > 0 || isGrouping) ||
        //        RadGrid_Emails_uc.MasterTableView.SortExpressions.Count > 0;
        //}




        ////////////////////////////////////////

    }
}