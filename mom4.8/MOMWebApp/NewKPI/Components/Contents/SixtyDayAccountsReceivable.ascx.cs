using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class NewKPI_Components_Contents_SixtyDayAccountsReceivable : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        DataTable arSummary = (DataTable)Session["ARCollectionSummayry"];
        if (arSummary.Rows.Count > 0 && !string.IsNullOrEmpty(Convert.ToString(arSummary.Rows[0]["SixtyDay"])))
        {
            CountsSixtyPlus.InnerText = double.Parse(arSummary.Rows[0]["SixtyDay"].ToString()).ToString("C2");
        }
    }
}