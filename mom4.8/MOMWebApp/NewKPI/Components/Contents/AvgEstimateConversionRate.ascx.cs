using BusinessLayer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Demo_Components_Contents_AvgEstimateConversionRate : System.Web.UI.UserControl
{
    static BL_User objBL_User = new BL_User();
    static BusinessEntity.User objPropUser = new BusinessEntity.User();

    protected void Page_Load(object sender, EventArgs e)
    {
        var data = AvgEstimate();

        AvgEstimateValue.InnerText = double.Parse(data[0].ToString()).ToString("N0");
        if (data[2].ToString() == "Increment")
        {
            ImgUpAvgEstimate.Visible = true;
            ImgDownAvgEstimate.Visible = false;
        }
        else
        {
            ImgUpAvgEstimate.Visible = false;
            ImgDownAvgEstimate.Visible = true;
        }
    }

    public ArrayList AvgEstimate()
    {
        objPropUser.ConnConfig = HttpContext.Current.Session["config"].ToString();
        var avg = objBL_User.GetAvgEstimate(objPropUser);
        double avgFirstYear = double.Parse(avg[0].ToString());
        double avgSecondYear = double.Parse(avg[1].ToString());

        return avg;
    }
}