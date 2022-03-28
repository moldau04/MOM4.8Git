using BusinessEntity;
using BusinessEntity.APModels;
using BusinessEntity.CommonModel;
using BusinessEntity.Utility;
using BusinessLayer;
using MOMWebApp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class VendorTransactionHistory : System.Web.UI.Page
{
   
    protected void Page_Load(object sender, EventArgs e)
    {
        Page.Title = "Payment History";
    }
       
    private void BindData()
    {
        try
        {
            //APIIntegrationModel _objAPIIntegration = new APIIntegrationModel();
            string IsAPIIntegrationEnable = System.Web.Configuration.WebConfigurationManager.AppSettings["IsAPIIntegrationEnable"].Trim();
            GetHistoryTransactionParam _GetHistoryTransaction = new GetHistoryTransactionParam();

            string ConnConfig = Session["config"].ToString();
             string TypeID = Convert.ToString(Request.QueryString["type"]);
             string Ref = Convert.ToString(Request.QueryString["uid"]);
            int tid = Convert.ToInt32(Request.QueryString["tid"]);
            int vendor = Convert.ToInt32(Request.QueryString["vendor"]);
            int loc = Convert.ToInt32(Request.QueryString["loc"]);
            string status = Request.QueryString["status"];

            //API
            _GetHistoryTransaction.ConnConfig = Session["config"].ToString();
            _GetHistoryTransaction.type = Convert.ToString(Request.QueryString["type"]);
            _GetHistoryTransaction.id = Convert.ToString(Request.QueryString["uid"]);
            _GetHistoryTransaction.tid = Convert.ToInt32(Request.QueryString["tid"]);
            _GetHistoryTransaction.vendor = Convert.ToInt32(Request.QueryString["vendor"]);
            _GetHistoryTransaction.loc = Convert.ToInt32(Request.QueryString["loc"]);
            _GetHistoryTransaction.status = Request.QueryString["status"];

            DataSet ds = new DataSet();
            BL_Bills objBL_Bills = new BL_Bills();

            
            List<GetHistoryTransactionViewModel> _lstGetHistoryTransaction = new List<GetHistoryTransactionViewModel>();
            //_objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];

            if (IsAPIIntegrationEnable == "YES")
            //if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
            {
                string APINAME = "VendorAPI/VendorTransaction_GetHistoryTransaction";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetHistoryTransaction);

                JavaScriptSerializer serializer = new JavaScriptSerializer();

                serializer.MaxJsonLength = Int32.MaxValue;

                _lstGetHistoryTransaction = serializer.Deserialize<List<GetHistoryTransactionViewModel>>(_APIResponse.ResponseData);
                ds = CommonMethods.ToDataSet<GetHistoryTransactionViewModel>(_lstGetHistoryTransaction);
            }
            else
            {
                ds = objBL_Bills.GetHistoryTransaction(ConnConfig, Ref, TypeID, vendor, loc, status, tid);
            }


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