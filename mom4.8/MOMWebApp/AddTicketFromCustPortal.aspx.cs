using System;
using System.Collections.Generic; 
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLayer;
using BusinessEntity;
using System.Data;
using AjaxControlToolkit; 

public partial class AddTicketFromCustPortal : Page
{
    User objPropUser = new User();
    BL_User objBL_User = new BL_User();

    BL_MapData objBL_MapData = new BL_MapData();
    MapData objMapData = new MapData();

    BL_General objBL_General = new BL_General();
    General objGeneral = new General();

    BL_Customer objBL_Customer = new BL_Customer();
    Customer objCustomer = new Customer();
    public GeneralFunctions objGeneralFunctions = new GeneralFunctions();    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userid"] == null)
        {

            Response.Redirect("login.aspx");
            return;
        }
        if (Session["type"].ToString() != "c")
        {
            Response.Redirect("login.aspx");
            return;
        }

        if (!IsPostBack)
        {  
            FillCategory(); 
            ViewState["title"] = lblHeader.Text + " : Mobile Office Manager"; 
            /******** Check for TS login**********IntegrateIntegrate*/ 
            hdnFocus.Value = txtLocation.ClientID;
            CustomerRequestforservice();
        }
    }  

    protected void Page_PreRender(Object o, EventArgs e)
    {
        foreach (GridViewRow gr in gvEquip.Rows)
        {
            Label lblID = (Label)gr.FindControl("lblID");
            Label lblname = (Label)gr.FindControl("lblUnit");
            CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");
            TextBox txtPrice = (TextBox)gr.FindControl("txtPrice");
            TextBox txtHours = (TextBox)gr.FindControl("txtHours");

            chkSelect.Attributes["onclick"] = "SelectRows('" + gvEquip.ClientID + "','" + txtUnit.ClientID + "','" + hdnUnitID.ClientID + "');";
        }
    }
   
    private void FillCategory()
    {
        DataSet ds = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();
        ds = objBL_User.getCategory(objPropUser);
        ddlCategory.DataSource = ds.Tables[0];
        ddlCategory.DataTextField = "type";
        ddlCategory.DataValueField = "type";
        ddlCategory.DataBind();
        ddlCategory.Items.Insert(0, new ListItem(":: Select ::", ""));
        ddlCategory.Items.Insert(1, new ListItem("None", "None"));
    }
      
    public void FillLoc()
    {
        DataSet ds = new DataSet();
        objPropUser.SearchValue = "";
        objPropUser.ConnConfig = Session["config"].ToString();
        objPropUser.CustomerID = Convert.ToInt32(hdnPatientId.Value);
        ds = objBL_User.getLocationAutojquery(objPropUser, new GeneralFunctions().GetSalesAsigned());
        if (ds.Tables[0].Rows.Count == 1)
        {
            hdnLocId.Value = ds.Tables[0].Rows[0]["value"].ToString();
            txtLocation.Text = ds.Tables[0].Rows[0]["label"].ToString();
            lblLocation.Text = ds.Tables[0].Rows[0]["desc"].ToString();
            lblLocation.Visible = false;
        } 
    }
     
     
    private void FillLocInfo()
    {
        if (hdnLocId.Value == "")
        {
            return;
        }
        else
        {
            RequiredFieldValidator1.Enabled = true;
            objPropUser.DBName = Session["dbname"].ToString();
            objPropUser.LocID = Convert.ToInt32(hdnLocId.Value);
            objPropUser.ConnConfig = Session["config"].ToString();
            DataSet ds = new DataSet();
            ds = objBL_User.getLocationByID(objPropUser);
            if (ds.Tables[0].Rows.Count > 0)
            {
                txtLocation.Text = ds.Tables[0].Rows[0]["tag"].ToString();                
                txtCustomer.Text = ds.Tables[0].Rows[0]["custname"].ToString();
                hdnPatientId.Value = ds.Tables[0].Rows[0]["owner"].ToString();               
            }
            lblLocation.Text = hdndesc.Value;
            lblLocation.Visible = false;
            GetDataEquip();
        }
    }

    private void ResetFormControlValues(Control parent)
    {
        foreach (Control c in parent.Controls)
        {
            if (c.Controls.Count > 0)
            {
                ResetFormControlValues(c);
            }
            else
            {
                switch (c.GetType().ToString())
                {
                    case "System.Web.UI.WebControls.DropDownList":
                        ((DropDownList)c).SelectedIndex = -1;
                        break;
                    case "System.Web.UI.WebControls.TextBox":
                        ((TextBox)c).Text = "";
                        break;
                    case "System.Web.UI.WebControls.CheckBox":
                        ((CheckBox)c).Checked = false;
                        break;
                    case "System.Web.UI.WebControls.RadioButton":
                        ((RadioButton)c).Checked = false;
                        break;
                }
            }
        }
    }

    protected void lnkSave_Click(object sender, EventArgs e)
    { 

        try
        {

            if (hdnLocId.Value == string.Empty)
            {
                objMapData.LocID = 0;
                return;
            }
            else
            {
                objMapData.LocID = Convert.ToInt32(hdnLocId.Value);
                objMapData.CustomerName = txtCustomer.Text;
                if (hdnPatientId.Value == string.Empty)
                {
                    objMapData.CustID = 0;
                }
                else
                {
                    objMapData.CustID = Convert.ToInt32(hdnPatientId.Value);
                }
                objMapData.Cell = txtCell.Text;
                objMapData.CallDate = objMapData.SchDate = Convert.ToDateTime(txtCallDt.Text + " " + txtCallTime.Text);
                objMapData.Assigned = Convert.ToInt32(0);
                objMapData.Category = ddlCategory.SelectedValue;
                if (hdnUnitID.Value != "")
                {
                    objMapData.Unit = Convert.ToInt32(hdnUnitID.Value);
                }
                objMapData.Reason = txtReason.Text;
                objMapData.EST = Convert.ToDouble("1.00");
                objMapData.ConnConfig = Session["config"].ToString();
                objMapData.Who = txtNameWho.Text;
                objMapData.Level = 99;               
                objMapData.IsRecurring = 0;
                objMapData.LastUpdatedBy = Session["username"].ToString(); 
                objMapData.fBy = "Portal";
                objMapData.dtEquips = GetElevData();
                /******** when mode is add new record **********/
                string TicketsID = string.Empty;
                objBL_MapData.AddticketfrmCustPortal(objMapData);
                TicketsID = "Thank you, your service request is received. The reference No is Ticket# : " + objMapData.TicketID.ToString() + ". We will soon get back to you!";
                ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + TicketsID + "',  type :'success', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});RefressTicketListContact();", true);
                ResetFormControlValues(this);
                txtCallTime.Text = DateTime.Now.ToShortTimeString();
                txtCallDt.Text = DateTime.UtcNow.ToShortDateString();
                gvEquip.DataSource = null;
                gvEquip.DataBind();
                lblHeader.Text = "Service Request";
                CustomerRequestforservice();
            }
        }
        catch (Exception ex)
        { 
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            string ErrorType = "error";
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : '" + ErrorType + "', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void btnSelectLoc_Click(object sender, EventArgs e)
    {
        FillLocInfo();       
    } 
     
    private void CustomerRequestforservice()
    {
        if (Session["type"].ToString() == "c")
        {
            txtCustomer.Enabled = false; txtCustomer.Text = Session["Username"].ToString();
            hdnPatientId.Value = Session["custid"].ToString();
            FillLoc();
            FillLocInfo();            
            txtCallDt.Text = DateTime.Now.ToShortDateString();
            txtCallTime.Text = DateTime.Now.ToShortTimeString();
        }
    } 

    private void GetDataEquip()
    {
        DataSet ds = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();
        objPropUser.SearchBy = string.Empty;
        objPropUser.LocID = Convert.ToInt32(hdnLocId.Value); 
        objPropUser.InstallDate = string.Empty;
        objPropUser.ServiceDate = string.Empty;
        objPropUser.Price = string.Empty;
        objPropUser.Manufacturer = string.Empty;
        objPropUser.Status = -1;
        ds = objBL_User.getElev(objPropUser);
        gvEquip.DataSource = ds.Tables[0];
        gvEquip.DataBind();
    }
    
    private DataTable GetElevData()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("ticket_id", typeof(int));
        dt.Columns.Add("elev_id", typeof(int));
        dt.Columns.Add("labor_percentage", typeof(double));

        foreach (GridViewRow gvr in gvEquip.Rows)
        {
            CheckBox chkSelect = (CheckBox)gvr.FindControl("chkSelect");

            if (chkSelect.Checked == true)
            {
                DataRow dr = dt.NewRow();
                Label lblUnit = (Label)gvr.FindControl("lblID");
                TextBox txtHours = (TextBox)gvr.FindControl("txtHours");
                dr["ticket_id"] = 0;
                dr["elev_id"] = Convert.ToInt32(lblUnit.Text);
                if (txtHours.Text.Trim() != string.Empty)
                {
                    dr["labor_percentage"] = Convert.ToDouble(txtHours.Text);
                }
                dt.Rows.Add(dr);
            }
        }
        return dt;
    }
 
}