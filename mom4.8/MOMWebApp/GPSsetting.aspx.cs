using BusinessEntity;
using BusinessLayer;
using System;
using System.Web.UI;

namespace MOMWebApp
{
    public partial class GPSsetting : System.Web.UI.Page
	{
        BusinessEntity.User objPropUser = new BusinessEntity.User();
        BL_User objBL_User = new BL_User();
        General objgeneral = new General();
        BL_General objBL_General = new BL_General();

        protected void Page_Load(object sender, EventArgs e)
		{
            GetGPSInfo();
        }
        private void GetGPSInfo()
        {
            string strGPSPing = objBL_General.GetGPSInterval(objgeneral);

            if (strGPSPing != string.Empty)
            {
                ddlGPSPing.SelectedValue = strGPSPing;
            }
            lblMSgGPS.Text = "";
        }
        protected void lnkGPS_Click(object sender, EventArgs e)
        {
            try
            {
                objgeneral.GPSInterval = Convert.ToInt32(ddlGPSPing.SelectedValue);
                objBL_General.InsertGPSInterval(objgeneral);
                lblMSgGPS.Text = "GPS settings updated successfully.";
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "noty({text: 'GPS settings updated successfully.',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
            }
            catch (Exception ex)
            {
                lblMSgGPS.Text = ex.Message;
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "noty({text: '" + ex.Message + "',  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
            }
        }
    }
}