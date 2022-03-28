using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MOMWebApp
{
    public partial class PayrollCalculationDetials : System.Web.UI.Page
    {
        DataTable dtEmployeeDeduction = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
            hdnEmpId.Value = Request.QueryString["empId"];
            dtEmployeeDeduction = (DataTable)Session["EmployeeDeductions"];
        }

        protected void RadGrid_Deduction_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            RadGrid_Deduction.VirtualItemCount = dtEmployeeDeduction.Rows.Count;
            RadGrid_Deduction.DataSource = dtEmployeeDeduction;
        }
    }
}