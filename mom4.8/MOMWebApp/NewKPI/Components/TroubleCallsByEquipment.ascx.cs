using BusinessEntity;
using BusinessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class NewKPI_Components_TroubleCallsByEquipment : System.Web.UI.UserControl
{
    static BL_User objBL_User = new BL_User();
    User objProp_User = new User();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadFilterTopAndDays();
        }

        objProp_User.ConnConfig = Session["config"].ToString();
        if (SelectCategoryDd.Items.Count == 0)
        {
            var ds = objBL_User.getCategory(objProp_User);

            SelectCategoryDd.DataSource = ds.Tables[0];
            SelectCategoryDd.DataTextField = "type";
            SelectCategoryDd.DataValueField = "type";
            SelectCategoryDd.DataBind();

            var listCategory = new List<string>();
            if (Session["TroubleCallsCategories"] != null)
            {
                listCategory = (List<string>)Session["TroubleCallsCategories"];               
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


    protected void TroubleCallsEquipmentGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {


    }

    protected void FilterTopAndDays_SelectedIndexChanged(object o, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
    {
        LoadData();
    }

    protected void btnLoadData_Click(object sender, EventArgs e)
    {
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

        Session["TroubleCallsCategories"] = categories;

        var days = FilterTopAndDays.SelectedValue.Split(':');
        if(days.Count() == 2)
        {
            BindingTroubleCallsEquipmentGrid(Convert.ToInt32(days[1]), Convert.ToInt32(days[0]), categories);
        }
    }

    private void BindingTroubleCallsEquipmentGrid(int days, int callTimes, List<string> categories)
    {
        TroubleCallsByEquipmentGraphRequest request = new TroubleCallsByEquipmentGraphRequest();

        request.StartDate = DateTime.Now.AddDays(-days);
        request.EndDate = DateTime.Now;
        request.CallTimes = callTimes;
        request.UserID = Convert.ToInt32(Session["UserID"].ToString());

        if (Convert.ToString(Session["CmpChkDefault"]) == "1" || Convert.ToString(Session["chkCompanyName"]) != "")
        {
            request.EN = 1;
        }
        else
        {
            request.EN = 0;
        }

        request.Categories = string.Join(",", categories);

        var data = objBL_User.GetTroubleCallsByEquipment(objProp_User, request);
        TroubleCallsEquipmentGrid.DataSource = data.Tables[0];
        TroubleCallsEquipmentGrid.DataBind();
    }

    private void LoadFilterTopAndDays()
    {
        string filterConfig = "5:30;5:90;10:180;20:360";
        string config = System.Web.Configuration.WebConfigurationManager.AppSettings["TroubleCallsByEquipmentFilter"];
        if (!string.IsNullOrEmpty(config))
        {
            filterConfig = config;
        }

        var filters = filterConfig.Split(';');

        foreach(var filter in filters)
        {
            var days = filter.Split(':');

            FilterTopAndDays.Items.Add(new RadComboBoxItem(string.Format("{0} trouble calls in the past {1} days", days[0], days[1]), filter));
        }

    }
}