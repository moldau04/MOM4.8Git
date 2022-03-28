using BusinessEntity;
using BusinessLayer;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class HistoryTransaction : System.Web.UI.Page
{
   
    protected void Page_Load(object sender, EventArgs e)
    {
        Page.Title = "Payment History";
    }
       
    private void BindData()
    {
        try
        {
             string ConnConfig = Session["config"].ToString();
             int TypeID = Convert.ToInt16(Request.QueryString["type"]);
             int Ref = Convert.ToInt32(Request.QueryString["uid"]);
            int tid = Convert.ToInt32(Request.QueryString["tid"]);
            int owner = Convert.ToInt32(Request.QueryString["owner"]);
            int loc = Convert.ToInt32(Request.QueryString["loc"]);
            string status = Request.QueryString["status"];


            DataSet ds = new DataSet();
            BL_Invoice objBL_Invoice = new BL_Invoice();

            ds = objBL_Invoice.GetHistoryTransaction(ConnConfig, Ref, TypeID, owner,loc, status, tid);


            /// AR Invoice //////////////////>
            DataTable dtARInvoice = ds.Tables[0];

            RadGridARInvoice.DataSource = ds.Tables[0];
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErrProj", "noty({text: '" + str + "',  type : 'error', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void RadGridARInvoice_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }
}