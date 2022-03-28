using BusinessEntity;
using BusinessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class NewKPI_Components_MonthlyRevenueByDeptChart : System.Web.UI.UserControl
{
    static BL_User objBL_User = new BL_User();
    static BusinessEntity.User objPropUser = new BusinessEntity.User();

    protected void Page_Load(object sender, EventArgs e)
    {
        DataTable data = new DataTable();

        if (SelectCompanyDd.Items.Count == 0)
        {
            var listCompanies = GetListCompany();

            if (listCompanies.Rows.Count > 1)
            {
                SelectCompanyDd.DataSource = listCompanies;
                SelectCompanyDd.DataTextField = "Name";
                SelectCompanyDd.DataValueField = "CompanyID";
                SelectCompanyDd.DataBind();
                // ddlCompany.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select", "0"));

                data = GetRevenueByCompany(Convert.ToInt32(SelectCompanyDd.SelectedValue));
            }
            else
            {
                data = GetRevenueByCompany(null);
                SelectCompanyContent.Visible = false;
                MonthlyRevenueByDeptChart.Height = Unit.Pixel(437);
            }
        }
        else
        {
            data = GetRevenueByCompany(Convert.ToInt32(SelectCompanyDd.SelectedValue));
        }

        MonthlyRevenueByDeptChart.DataSource = data;
        MonthlyRevenueByDeptChart.DataBind();
    }

    protected void SelectCompanyDd_SelectedIndexChanged(object sender, EventArgs e)
    {
        var data = GetRevenueByCompany(Convert.ToInt32(SelectCompanyDd.SelectedValue));
        MonthlyRevenueByDeptChart.DataSource = data;
        MonthlyRevenueByDeptChart.DataBind();
    }


    public DataTable GetRevenueByCompany(int? companyId)
    {
        objPropUser.ConnConfig = HttpContext.Current.Session["config"].ToString();
        var dSet = objBL_User.GetMonthlyRevenueByCompany(objPropUser, companyId);
        var dtTable = dSet.Tables[0];

        double total = 0;
        for (var i = 0; i < dtTable.Rows.Count; i++)
        {
            total += Convert.ToDouble(dtTable.Rows[i]["Revenue"].ToString());
        }

        TotalRevenueLabel.Text = total.ToString("C2");

        return dtTable;
    }

    private DataTable GetListCompany()
    {
        BusinessEntity.CompanyOffice objCompany = new BusinessEntity.CompanyOffice();
        BL_Company objBL_Company = new BL_Company();
        objCompany.UserID = Convert.ToInt32(Session["UserID"].ToString());
        objCompany.DBName = Session["dbname"].ToString();
        objCompany.ConnConfig = Session["config"].ToString();

        DataSet ds = new DataSet();
        ds = objBL_Company.getCompanyByUserID(objCompany);
        
        return ds.Tables[0];
    }
}