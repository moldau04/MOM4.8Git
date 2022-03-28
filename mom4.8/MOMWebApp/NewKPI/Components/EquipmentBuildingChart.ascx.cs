using BusinessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Demo_Components_EquipmentBuildingChart : System.Web.UI.UserControl
{
    static BL_User objBL_User = new BL_User();
    static BusinessEntity.User objPropUser = new BusinessEntity.User();

    protected void Page_Load(object sender, EventArgs e)
    {
        var equipmentBuilding = GetEquipmentBuildingCount();
        TotalEquipmentLabel.Text = equipmentBuilding.Sum(x => x.value).ToString();

        EquipmentBuildingChart.DataSource = equipmentBuilding;
        EquipmentBuildingChart.DataBind();
    }

    public EquipmentType[] GetEquipmentBuildingCount()
    {
        objPropUser.ConnConfig = HttpContext.Current.Session["config"].ToString();

        var dSet = objBL_User.GetEquipmentBuildingCount(objPropUser);
        DataTable dt = dSet.Tables[0];

        var result = new EquipmentType[dt.Rows.Count];

        for (var i = 0; i < dt.Rows.Count; i++)
        {
            result[i] = new EquipmentType
            {
                value = double.Parse(dt.Rows[i]["Total"].ToString()),
                category = dt.Rows[i]["Building"].ToString()
            };
        }

        return result;
    }
}