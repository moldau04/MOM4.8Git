using BusinessEntity;
using BusinessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class TicketCountByCategoryAndDateRange : System.Web.UI.UserControl
{
    static BL_User objBL_User = new BL_User();
    User objProp_User = new User();

    protected void Page_Load(object sender, EventArgs e)
    {
        objProp_User.ConnConfig = Session["config"].ToString();
        if (SelectCategoryDd.Items.Count == 0)
        {
            var ds = objBL_User.getCategory(objProp_User);

            SelectCategoryDd.DataSource = ds.Tables[0];
            SelectCategoryDd.DataTextField = "type";
            SelectCategoryDd.DataValueField = "type";
            SelectCategoryDd.DataBind();

            var listCategory = new List<string>();
            if (Session["TicketCountCategories"] != null)
            {
                listCategory = (List<string>)Session["TicketCountCategories"];
            }
            else
            {
                listCategory.Add("Trouble Call");
            }

            foreach (var category in listCategory)
            {
                var item = SelectCategoryDd.Items.FindItem(x => x.Text == category);
                if (item != null)
                {
                    item.Checked = true;
                }
            }
        }

        LoadData();
    }

    private void LoadData()
    {
        var categories = new List<string>();
        foreach (var item in SelectCategoryDd.Items.ToList())
        {
            if (item.Checked)
            {
                categories.Add(item.Value);
            }
        }

        if (categories.Count == 0)
        {
            categories.Add("Trouble Call");
        }

        Session["TicketCountCategories"] = categories;

        if (!string.IsNullOrEmpty(FilterDateRange.SelectedValue))
        {
            BindingTicketCountByCategory(Convert.ToInt32(FilterDateRange.SelectedValue), categories);
        }
    }

    private void BindingTicketCountByCategory(int days, List<string> categories)
    {

        objProp_User.StartDate = DateTime.Now.AddDays(-days).ToShortDateString();
        objProp_User.EndDate = DateTime.Now.AddDays(1).AddSeconds(-1).ToShortDateString();
        objProp_User.UserID = Convert.ToInt32(Session["UserID"].ToString());

        if (Convert.ToString(Session["CmpChkDefault"]) == "1" || Convert.ToString(Session["chkCompanyName"]) != "")
        {
            objProp_User.EN = 1;
        }
        else
        {
            objProp_User.EN = 0;
        }

        var data = objBL_User.GetTicketCountByCategory(objProp_User, string.Join(",", categories));
        TicketCountByCategoryAndDateRangeChart.DataSource = data.Tables[0];
        TicketCountByCategoryAndDateRangeChart.DataBind();
    }

    protected void FilterDateRange_SelectedIndexChanged(object o, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
    {
        LoadData();
    }

    protected void btnLoadData_Click(object sender, EventArgs e)
    {
        LoadData();
    }
}