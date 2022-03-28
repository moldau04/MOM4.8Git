using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLayer;
using BusinessEntity;
using System.Data;
using Telerik.Web.UI;
using System.Web.Services;
using System.Collections.Generic;
using Microsoft.ApplicationBlocks.Data;
using System.Web;
using BusinessLayer.Programs;
using BusinessEntity.Programs;
using System.Linq;

namespace MOMWebApp
{
    public partial class ServiceType : Page
    {

        BL_User objBL_User = new BL_User();
        User objProp_User = new User();
        //consult
        tblConsult objProp_Consult = new tblConsult();
        BL_General objBL_General = new BL_General();
        General objGeneral = new General();
        BL_Customer objBL_Customer = new BL_Customer();
        BL_Vendor objBL_Vendor = new BL_Vendor();
        Customer objCustomer = new Customer();
        BL_ReportsData objBL_ReportData = new BL_ReportsData();
        GeneralFunctions objGeneralFunctions = new GeneralFunctions();
        public static bool IsAddEdit = false;
        public static bool IsDelete = false;
        Wage _objWage = new Wage();
        bool api = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
              
                FillSetupRadComboBox(ddlRoute, "", "route");
                FillSetupRadComboBox(ddldepartment1, "", "Department");


            }
        }
        protected void RadGrid_ServiceType_PreRender(object sender, EventArgs e)
        {
            #region Save the Grid Filter
            String filterExpression = Convert.ToString(RadGrid_ServiceType.MasterTableView.FilterExpression);
            if (filterExpression != "")
            {
                Session["ServiceType_FilterExpression"] = filterExpression;
                List<RetainFilter> filters = new List<RetainFilter>();

                foreach (GridColumn column in RadGrid_ServiceType.MasterTableView.OwnerGrid.Columns)
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
                Session["ServiceType_Filters"] = filters;
            }
            else
            {
                Session["ServiceType_FilterExpression"] = null;
                Session["ServiceType_Filters"] = null;
            }

            GeneralFunctions obj = new GeneralFunctions();
            obj.CorrectTelerikPager(RadGrid_ServiceType);
            #endregion
            RowSelect();
        }
        private void RowSelect()
        {
            foreach (GridDataItem gr in RadGrid_ServiceType.Items)
            {
                Label lblID = (Label)gr.FindControl("lblId");
                //HyperLink lnkName = (HyperLink)gr.FindControl("lnkRef");
                gr.Attributes["ondblclick"] = "OpenServiceTypeWindowEditDoubleclick('" + lblID.Text + "');";
                
            }
        }
        
        private const int ItemsPerRequest = 10;





        #region Service Type


        private void FillSetupRadComboBox(RadComboBox ddl, string SearchBy, string Case)
        {
            List<ServiceTypeDDLData> _DDLDATA = new BL_ServiceType().GetSetupServiceTypeDropDownValue(Session["config"].ToString(), SearchBy, Case);


            foreach (var items in _DDLDATA)
            {
                var cboItem = new RadComboBoxItem() { Text = items.Name, Value = items.Value };
                ddl.Items.Add(cboItem);
            }


        }
        private void FillSetupDropDown(DropDownList ddl, string SearchBy, string Case)
        {
            List<ServiceTypeDDLData> _DDLDATA = new BL_ServiceType().GetSetupServiceTypeDropDownValue(Session["config"].ToString(), SearchBy, Case);

            ddl.Items.Clear();

            ddl.Items.Add(new ListItem() { Text = "Please select", Value = "0", Selected = true });

            foreach (var items in _DDLDATA)
            {
                ddl.Items.Add(new ListItem() { Text = items.Name, Value = items.Value });
            }


        }


        private void FillServiceType()
        {
            DataSet ds = new DataSet();
            objProp_User.ConnConfig = Session["config"].ToString();

            ds = new BL_ServiceType().GetServiceType(objProp_User.ConnConfig);

            RadGrid_ServiceType.VirtualItemCount = ds.Tables[0].Rows.Count;
            RadGrid_ServiceType.DataSource = ds.Tables[0];
        }

        bool isGroupingServiceType = false;
        public bool ShouldApplySortServiceType()
        {
            return RadGrid_ServiceType.MasterTableView.FilterExpression != "" ||
                (RadGrid_ServiceType.MasterTableView.GroupByExpressions.Count > 0 || isGroupingServiceType) ||
                RadGrid_ServiceType.MasterTableView.SortExpressions.Count > 0;
        }
        protected void RadGrid_ServiceType_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            RadGrid_ServiceType.AllowCustomPaging = !ShouldApplySortServiceType();
            FillServiceType();
        }

        private string GetSelectedCategory()
        {

            string selectedvals = string.Empty;

            foreach (RadComboBoxItem item in ddlRoute.CheckedItems)
            {
                if (item.Checked == true)
                {
                    selectedvals += "" + item.Value + ",";
                }
            }
            return selectedvals.TrimEnd(',');
        }
        private string GetSelectedDepartment()
        {

            string selectedvals = string.Empty;

            foreach (RadComboBoxItem item in ddldepartment1.CheckedItems)
            {
                if (item.Checked == true)
                {
                    selectedvals += "" + item.Value + ",";
                }
            }
            return selectedvals.TrimEnd(',');
        }

        protected void lnkServiceSave_Click(object sender, EventArgs e)
        {
            //IsAddEdit = false;
            try
            {
                if (!Page.IsValid) { return; }

                //int RT = Convert.ToInt32(ddlRT.SelectedValue);
                //int OT = Convert.ToInt32(DDlot.SelectedValue);
                //int NT = Convert.ToInt32(ddl1Point7.SelectedValue);
                //int DT = Convert.ToInt32(ddlDT.SelectedValue);

                int RT = 0;
                int OT = 0;
                int NT = 0;
                int DT = 0;
                int STATUS = 0;
                int ExpenseGL = 0;
                int InterestGL = 0;
                int LaborWageC = 0;
                int InvID = 0;
                string strddldepartment = "";
                if (ddlRT1.SelectedValue != "" && ddlRT1.SelectedValue != null)
                {
                    RT = Convert.ToInt32(ddlRT1.SelectedValue);
                }
                if (DDlot1.SelectedValue != "" && DDlot1.SelectedValue != null)
                {
                     OT = Convert.ToInt32(DDlot1.SelectedValue);
                }
                if (ddl1Point71.SelectedValue != "" && ddl1Point71.SelectedValue != null)
                {
                     NT = Convert.ToInt32(ddl1Point71.SelectedValue);
                }
                if (ddlDT1.SelectedValue != "" && ddlDT1.SelectedValue != null)
                {
                     DT = Convert.ToInt32(ddlDT1.SelectedValue);
                }
                string ConnConfig = Session["config"].ToString();
                string TYPE = txtServiceType.Text;
                string FDESC = txtServiceTypeDesc.Text;
                string REMARKS = txtServRemarks.Text;
                if (ddlServiceTypeStatus.SelectedValue != "" && ddlServiceTypeStatus.SelectedValue != null)
                {
                     STATUS = Convert.ToInt32(ddlServiceTypeStatus.SelectedValue);
                }
         
                string LocType = ddlLocationtype1.SelectedValue;
                if (DDLEGL1.SelectedValue != "" && DDLEGL1.SelectedValue != null)
                {
                     ExpenseGL = Convert.ToInt32(DDLEGL1.SelectedValue);
                }
                if (DDLIGL1.SelectedValue != "" && DDLIGL1.SelectedValue != null)
                {
                     InterestGL = Convert.ToInt32(DDLIGL1.SelectedValue);
                }
                if (ddlWC1.SelectedValue != "" && ddlWC1.SelectedValue != null)
                {
                     LaborWageC = Convert.ToInt32(ddlWC1.SelectedValue);
                }
                if (ddlBillingCode1.SelectedValue != "" && ddlBillingCode1.SelectedValue != null)
                {
                     InvID = Convert.ToInt32(ddlBillingCode1.SelectedValue);
                }
                string route = GetSelectedCategory();
                string msg = string.Empty;
                strddldepartment = GetSelectedDepartment();
                STATUS = ddlServiceTypeStatus.SelectedIndex;
                if (hdnAddEdit.Value == "0")
                {

                    msg = "Added";

                    objBL_User.AddServiceType(ConnConfig, TYPE, FDESC, REMARKS, RT, OT, NT, DT, STATUS, LocType, ExpenseGL, InterestGL, LaborWageC, InvID, route, strddldepartment);

                }
                else
                {
                    int Flage = Convert.ToInt32(hdnFlage.Value);  //If Yes for Update =1 else 0
                    var userName = Session["username"].ToString();
                    objBL_User.UpdateServiceType(ConnConfig, TYPE, FDESC, REMARKS, RT, OT, NT, DT, STATUS, LocType, ExpenseGL, InterestGL, LaborWageC, InvID, route,Flage, strddldepartment, userName);
                    msg = "Updated";
                }

                //ScriptManager.RegisterStartupScript(Page, typeof(Page), "closeScript", "", true);
              
                if (!string.IsNullOrEmpty(msg))
                {
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccAddServicetype", "noty({text: 'Service Type " + msg + " successfully.', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : true});  window.setTimeout(function () { window.location.href = 'ServiceType.aspx'; }, 500); ", true);
                }
                FillServiceType();
                RadGrid_ServiceType.Rebind();
            }
            catch (Exception ex)
            {
                string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
                string strerror = "warning";
     
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyAddServicetype", "noty({text: '" + str + "',dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            }
        }
        protected void lnkDeleteService_Click(object sender, EventArgs e)
        {
            IsDelete = false;
            try
            {
                foreach (GridDataItem di in RadGrid_ServiceType.SelectedItems)
                {
                    IsDelete = true;
                    TableCell cell = di["chkSelect"];
                    CheckBox chkSelect = (CheckBox)cell.Controls[0];
                    Label lblId = (Label)di.FindControl("lblId");
                    if (chkSelect.Checked == true)
                    {
                        objProp_User.ConnConfig = Session["config"].ToString();
                        objProp_User.EquipType = lblId.Text;
                        objBL_User.DeleteServicetype(objProp_User);
                        FillServiceType();
                        RadGrid_ServiceType.Rebind();
                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccAddCusttype", "noty({text: 'Service Type " + lblId.Text + " Deleted Successfully!', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                    }
                }
                if (!IsDelete)
                {
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarningServyp", "noty({text: 'Please select Service Type to Delete!', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                }
            }
            catch (Exception ex)
            {
                string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErrDelServyp", "noty({text: '" + str + "',  type : 'warning', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);

            }
        }

        #endregion
    
        protected void lnkEditService_Click(object sender, EventArgs e)
        {

            if (hdnAddEdit.Value != "")
            {
                string route = "";
                string Department = "";

                DataSet ds = objBL_User.GetServiceType(Session["config"].ToString(), hdnAddEdit.Value);

                foreach (RadComboBoxItem itm in ddlRoute.Items)
                {
                    itm.Checked = false;
                }

                foreach (DataRow dr in ds.Tables[0].Rows)
                {

                    ddlRT1.Text = dr["RTNAME"].ToString();
                    DDlot1.Text = dr["OTNAME"].ToString();
                    ddl1Point71.Text = dr["NTNAME"].ToString();
                    ddlDT1.Text = dr["DTNAME"].ToString();
                    DDLEGL1.Text = dr["ExpenseGLNAME"].ToString();
                    DDLIGL1.Text = dr["InterestGLNAME"].ToString();
                    ddlBillingCode1.Text = dr["InvIDNAME"].ToString();
                    ddlWC1.Text = dr["LaborWageCName"].ToString();

                    ddlServiceTypeStatus.SelectedValue = dr["status"].ToString();
                    ddlRT1.SelectedValue = dr["RT"].ToString();
                    DDlot1.SelectedValue = dr["OT"].ToString();
                    ddl1Point71.SelectedValue = dr["NT"].ToString();
                    ddlDT1.SelectedValue = dr["DT"].ToString();
                    DDLEGL1.SelectedValue = dr["ExpenseGL"].ToString();
                    DDLIGL1.SelectedValue = dr["InterestGL"].ToString();
                    ddlBillingCode1.SelectedValue = dr["InvID"].ToString();
                    ddlWC1.SelectedValue = dr["LaborWageC"].ToString();



                    txtServiceType.Text = dr["TYPE"].ToString();
                    txtServiceType.ReadOnly = true;
                    txtServiceTypeDesc.Text = dr["FDESC"].ToString();
                    txtServRemarks.Text = dr["REMARKS"].ToString();

                    string Ltype = dr["LocTypeNAME"].ToString();

                    if (!string.IsNullOrEmpty(Ltype))
                    {
                        ddlLocationtype1.Text = dr["LocTypeNAME"].ToString();
                        ddlLocationtype1.SelectedValue = dr["LocType"].ToString();
                    }

                    route = dr["route"].ToString();
                    Department = dr["Department"].ToString();
                    string routelabel = dr["routelabel"].ToString();
                    lblroutelabel.InnerText = routelabel == string.Empty ? "Route" : routelabel;
                    break;
                }

                try
                {

                    if (!string.IsNullOrEmpty(route))
                    {
                        List<string> result = route.ToString().Split(',').ToList();

                        foreach (RadComboBoxItem itm in ddlRoute.Items)
                        {
                            string s1 = itm.Value;

                            foreach (var item in result)
                            {
                                string s2 = item.ToString().Replace("'", "");

                                if (s1 == s2)
                                {
                                    itm.Checked = true;
                                }
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(Department))
                    {
                        List<string> result = Department.ToString().Split(',').ToList();

                        foreach (RadComboBoxItem itm in ddldepartment1.Items)
                        {
                            string s1 = itm.Value;

                            foreach (var item in result)
                            {
                                string s2 = item.ToString().Replace("'", "");

                                if (s1 == s2)
                                {
                                    itm.Checked = true;
                                }
                            }
                        }
                    }


                }
                catch
                {

                }
                hdnFlage.Value = "0";
                ServiceTypeWindow.Title = "Edit Service Type";

                string script = "function f(){$find(\"" + ServiceTypeWindow.ClientID + "\").show(); Sys.Application.remove_load(f);}Sys.Application.add_load(f); Materialize.updateTextFields();";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "keyedit", script, true);
            }

        }

        protected void lnkAddService_Click(object sender, EventArgs e)
        {
            try
            {

              
                DDlot1.SelectedIndex = 0;
                ddl1Point71.SelectedIndex = 0;
                ddlDT1.SelectedIndex = 0;
                txtServiceType.Text = "";
                txtServiceType.ReadOnly = false;
                txtServiceTypeDesc.Text = "";
                txtServRemarks.Text = "";
              
                DDLEGL1.SelectedIndex = 0;
                DDLIGL1.SelectedIndex = 0;
                ddlWC1.SelectedIndex = 0;
                ddlBillingCode1.SelectedIndex = 0;
                ddlLocationtype1.SelectedIndex = 0;
                
                foreach (RadComboBoxItem itm in ddlRoute.Items)
                {
                    itm.Checked = false;
                }
                foreach (RadComboBoxItem itm in ddldepartment1.Items)
                {
                    itm.Checked = false;
                }

                ddlRT1.Items.Clear();
                DDlot1.Items.Clear();
                ddl1Point71.Items.Clear();
                ddlDT1.Items.Clear();
                DDLEGL1.Items.Clear();
                DDLIGL1.Items.Clear();
                ddlWC1.Items.Clear();
                ddlBillingCode1.Items.Clear();
                ddlLocationtype1.Items.Clear();
                
                ddlRT1.Text = "";
                DDlot1.Text = "";
                ddl1Point71.Text = "";
                ddlDT1.Text = "";
                DDLEGL1.Text = "";
                DDLIGL1.Text = "";
                ddlWC1.Text = "";
                ddlBillingCode1.Text = "";
                ddlLocationtype1.Text = "";
                
                DataSet ds = objBL_User.GetServiceType(Session["config"].ToString(), "0");

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    string routelabel = dr["routelabel"].ToString();
                    lblroutelabel.InnerText = routelabel == string.Empty ? "Route" : routelabel;
                }

                ServiceTypeWindow.Title = "Add Service Type";

                hdnAddEdit.Value = "0";
                hdnFlage.Value = "0";
                string script = "function f(){$find(\"" + ServiceTypeWindow.ClientID + "\").show(); Sys.Application.remove_load(f);}Sys.Application.add_load(f); Materialize.updateTextFields();";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "keyaddqq", script, true);


            }
            catch { }
        }

        protected void RadGrid_ServiceType_ItemCreated(object sender, GridItemEventArgs e)
        {
            try
            {
                if (e.Item is GridPagerItem)
                {
                    var dropDown = (RadComboBox)e.Item.FindControl("PageSizeComboBox");
                    var totalCount = ((GridPagerItem)e.Item).Paging.DataSourceCount; 
                    if (totalCount == 0) totalCount = 1000;
                    GeneralFunctions obj = new GeneralFunctions();
                    var sizes = obj.TelerikPageSize(totalCount);
                    dropDown.Items.Clear();

                    foreach (var size in sizes)
                    {
                        var cboItem = new RadComboBoxItem() { Text = size.Key, Value = size.Value };
                        cboItem.Attributes.Add("ownerTableViewId", e.Item.OwnerTableView.ClientID);
                        if (e.Item.OwnerTableView.PageSize.ToString() == size.Value) cboItem.Selected = true;
                        dropDown.Items.Add(cboItem);
                    }
                }
            }
            catch { }
        }
    }
}
 