using BusinessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Demo_Components_EquipmentTypeChart : System.Web.UI.UserControl
{
    static BL_User objBL_User = new BL_User();
    static BusinessEntity.User objPropUser = new BusinessEntity.User();

    protected void Page_Load(object sender, EventArgs e)
    {
        var equipmentType = GetEquipmentTypeCount();
        TotalEquipmentLabel.Text = equipmentType.Sum(x => x.value).ToString();

        EquipmentTypeChart.DataSource = equipmentType;
        EquipmentTypeChart.DataBind();
    }

    public EquipmentType[] GetEquipmentTypeCount()
    {
        objPropUser.ConnConfig = HttpContext.Current.Session["config"].ToString();

        var dSet = objBL_User.GetEquipmentTypeCount(objPropUser);
        DataTable dt = dSet.Tables[0];

        var result = new EquipmentType[dt.Rows.Count];

        for (var i = 0; i < dt.Rows.Count; i++)
        {
            result[i] = new EquipmentType
            {
                value = double.Parse(dt.Rows[i]["Total"].ToString()),
                category = dt.Rows[i]["TypeName"].ToString()
            };
        }

        return result;
    }
}