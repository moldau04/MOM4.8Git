using System;
using System.Web.UI;
using BusinessEntity;
using BusinessLayer;
using System.Data;

public partial class LocationAddress : System.Web.UI.Page
{
    User objPropUser = new User();
    BL_User objBL_User = new BL_User();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userid"] == null)
        {
            Response.Redirect("timeout.htm");
            return;
        }

        if (!IsPostBack)
        {
            GetLocationAddress();
        }
    }

    private void GetLocationAddress()
    {
        objPropUser.DBName = Session["dbname"].ToString();
        objPropUser.LocID = Convert.ToInt32(Request.QueryString["uid"]);
        DataSet ds = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();
        ds = objBL_User.getLocationByID(objPropUser);

        if (ds.Tables[0].Rows.Count > 0)
        {
            txtLocationName.Text = ds.Tables[0].Rows[0]["Tag"].ToString();
            txtGoogleAutoc.Text = ds.Tables[0].Rows[0]["LocAddress"].ToString();
            txtCity.Text = ds.Tables[0].Rows[0]["LocCity"].ToString(); ;
            txtState.Text = ds.Tables[0].Rows[0]["locstate"].ToString();
            txtZip.Text = ds.Tables[0].Rows[0]["locZip"].ToString();
            lat.Value = ds.Tables[0].Rows[0]["Lat"].ToString();
            lng.Value = ds.Tables[0].Rows[0]["Lng"].ToString();
            ViewState["rolid"] = ds.Tables[0].Rows[0]["rol"].ToString();
        }
    }

    protected void btnSubmitAddress_Click(object sender, EventArgs e)
    {
        try
        {
            objPropUser.ConnConfig = Session["config"].ToString();
            objPropUser.Address = txtGoogleAutoc.Text;
            objPropUser.City = txtCity.Text;
            objPropUser.State = txtState.Text;
            objPropUser.Zip = txtZip.Text;
            objPropUser.Lat = lat.Value;
            objPropUser.Lng = lng.Value;
            objPropUser.RolId = Convert.ToInt32(ViewState["rolid"].ToString());
            objPropUser.LocID = Convert.ToInt32(Request.QueryString["uid"]);

            objBL_User.UpdateLocationAddress(objPropUser);
            ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: 'Address updated successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
            hdnEdited.Value = "1";


        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);

        }
    }

    
}
