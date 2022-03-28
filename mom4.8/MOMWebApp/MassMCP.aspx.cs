using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BusinessLayer;
using BusinessEntity;
using System.Collections.Generic;
using BusinessEntity.CustomersModel;
using BusinessEntity.Utility;
using System.Web.Script.Serialization;
using MOMWebApp;

public partial class MassMCP : System.Web.UI.Page
{
    BL_User objBL_User = new BL_User();
    BusinessEntity.User objProp_User = new BusinessEntity.User();
    BL_Customer objBL_Customer = new BL_Customer();
    Customer objPropCustomer = new Customer();
    GeneralFunctions objgn = new GeneralFunctions();

    //API Variables 
    string IsAPIIntegrationEnable = System.Web.Configuration.WebConfigurationManager.AppSettings["IsAPIIntegrationEnable"].Trim();
    GetEquipmentCategoryParam _GetEquipmentCategory = new GetEquipmentCategoryParam();
    GetEquiptypeParam _GetEquiptype = new GetEquiptypeParam();
    GetActiveServiceTypeParam _GetActiveServiceType = new GetActiveServiceTypeParam();
    GetElevParam _GetElev = new GetElevParam();
    GetRepTemplateNameParam _GetRepTemplateName = new GetRepTemplateNameParam();
    GetTemplateItemByIDParam _GetTemplateItemByID = new GetTemplateItemByIDParam();
    AddMassMCPParam _AddMassMCP = new AddMassMCPParam();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userid"] == null)
        {
            Response.Redirect("login.aspx");
        }
        Permission();
        if (!IsPostBack)
        {
            FillEquipCategory();
            FillEquiptype();
            FillServiceType();
            GetDataAll();
            FillRepTemplate();
        }
    }

    private void Permission()
    {
        HtmlGenericControl li = (HtmlGenericControl)Page.Master.FindControl("cstmMgr");
        //li.Style.Add("background", "url(images/active_menu_bg.png) repeat-x");
        li.Attributes.Add("class", "start active open");

        HyperLink a = (HyperLink)Page.Master.FindControl("cstmlink");
        //a.Style.Add("color", "#2382b2");

        HyperLink lnkUsersSmenu = (HyperLink)Page.Master.FindControl("lnkEquipmentsSmenu");
        //lnkUsersSmenu.Style.Add("color", "#FF7A0A");
        lnkUsersSmenu.Style.Add("color", "#316b9d");
        lnkUsersSmenu.Style.Add("font-weight", "normal");
        lnkUsersSmenu.Style.Add("background-color", "#e5e5e5");
        //AjaxControlToolkit.HoverMenuExtender hm = (AjaxControlToolkit.HoverMenuExtender)Page.Master.FindControl("HoverMenuExtenderCstm");
        //hm.Enabled = false;
        //HtmlGenericControl ul = (HtmlGenericControl)Page.Master.FindControl("cstmMgrSub");
        //ul.Style.Add("display", "block");
        //ul.Style.Add("visibility", "visible");

        if (Session["type"].ToString() == "c")
        {
            Response.Redirect("home.aspx");
        }
        if (Session["MSM"].ToString() == "TS")
        { }
        if (Convert.ToInt32(Session["ISsupervisor"]) == 1)
        { }
    }

    private void FillEquipCategory()
    {
        DataSet ds = new DataSet();
        objProp_User.ConnConfig = Session["config"].ToString();
        _GetEquipmentCategory.ConnConfig = Session["config"].ToString();

        List<GetEquiptypeViewModel> _lstGetEquiptype = new List<GetEquiptypeViewModel>();

        if (IsAPIIntegrationEnable == "YES")
        {
            string APINAME = "EquipmentAPI/EquipmentsList_GetEquipmentCategory";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetEquipmentCategory);

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            serializer.MaxJsonLength = Int32.MaxValue;

            _lstGetEquiptype = serializer.Deserialize<List<GetEquiptypeViewModel>>(_APIResponse.ResponseData);
            ds = CommonMethods.ToDataSet<GetEquiptypeViewModel>(_lstGetEquiptype);
        }
        else
        {
            ds = objBL_User.getEquipmentCategory(objProp_User);
        }

        ddlCategory.DataSource = ds.Tables[0];
        ddlCategory.DataTextField = "edesc";
        ddlCategory.DataValueField = "edesc";
        ddlCategory.DataBind();
        ddlCategory.Items.Insert(0, new ListItem("None", "None"));
        ddlCategory.Items.Add(new ListItem("New", "New"));
        ddlCategory.Items.Add(new ListItem("Refurbished", "Refurbished"));
    }

    private void FillEquiptype()
    {
        DataSet ds = new DataSet();
        objProp_User.ConnConfig = Session["config"].ToString();
        _GetEquiptype.ConnConfig = Session["config"].ToString();

        List<GetEquiptypeViewModel> _lstGetEquiptype = new List<GetEquiptypeViewModel>();

        if (IsAPIIntegrationEnable == "YES")
        {
            string APINAME = "EquipmentAPI/EquipmentsList_GetEquiptype";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetEquiptype);

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            serializer.MaxJsonLength = Int32.MaxValue;

            _lstGetEquiptype = serializer.Deserialize<List<GetEquiptypeViewModel>>(_APIResponse.ResponseData);
            ds = CommonMethods.ToDataSet<GetEquiptypeViewModel>(_lstGetEquiptype);
        }
        else
        {
            ds = objBL_User.getEquiptype(objProp_User);
        }

        ddlType.DataSource = ds.Tables[0];
        ddlType.DataTextField = "edesc";
        ddlType.DataValueField = "edesc";
        ddlType.DataBind();

        ddlType.Items.Insert(0, new ListItem("None", "None"));
    }

    private void FillServiceType()
    {
        DataSet ds = new DataSet();
        objProp_User.ConnConfig = Session["config"].ToString();
        _GetActiveServiceType.ConnConfig = Session["config"].ToString();

        List<GetActiveServiceTypeViewModel> _lstGetActiveServiceType = new List<GetActiveServiceTypeViewModel>();

        if (IsAPIIntegrationEnable == "YES")
        {
            string APINAME = "EquipmentAPI/EquipmentsList_GetActiveServiceType";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetActiveServiceType);

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            serializer.MaxJsonLength = Int32.MaxValue;

            _lstGetActiveServiceType = serializer.Deserialize<List<GetActiveServiceTypeViewModel>>(_APIResponse.ResponseData);
            ds = CommonMethods.ToDataSet<GetActiveServiceTypeViewModel>(_lstGetActiveServiceType);
        }
        else
        {
            ds = new BusinessLayer.Programs.BL_ServiceType().GetActiveServiceType(objProp_User.ConnConfig);
        }

        //ds = objBL_User.GetServiceType(objProp_User);

        ddlServiceType.DataSource = ds.Tables[0];
        ddlServiceType.DataTextField = "type";
        ddlServiceType.DataValueField = "type";
        ddlServiceType.DataBind();
        ddlServiceType.Items.Insert(0, new ListItem("None", "None"));
    }

    private void GetDataAll()
    {
        DataSet ds = new DataSet();
        objProp_User.ConnConfig = Session["config"].ToString();
        objProp_User.SearchBy = ddlSearch.SelectedValue;

        _GetElev.ConnConfig = Session["config"].ToString();
        _GetElev.SearchBy = ddlSearch.SelectedValue;
        if (ddlSearch.SelectedValue == "e.Status")
        {
            objProp_User.SearchValue = rbStatus.SelectedValue;
            _GetElev.SearchValue = rbStatus.SelectedValue;
        }
        else if (ddlSearch.SelectedValue == "e.type")
        {
            objProp_User.SearchValue = ddlType.SelectedValue;
            _GetElev.SearchValue = ddlType.SelectedValue;
        }
        else if (ddlSearch.SelectedValue == "e.Cat")
        {
            objProp_User.SearchValue = ddlServiceType.SelectedValue;
            _GetElev.SearchValue = ddlServiceType.SelectedValue;
        }
        else if (ddlSearch.SelectedValue == "e.Category")
        {
            objProp_User.SearchValue = ddlCategory.SelectedValue;
            _GetElev.SearchValue = ddlCategory.SelectedValue;
        }
        else
        {
            objProp_User.SearchValue = txtSearch.Text.Replace("'","''");
            _GetElev.SearchValue = txtSearch.Text.Replace("'", "''");
        }

        string strInstall = string.Empty;
        if (txtInstallDt.Text.Trim() != string.Empty)
        {
            if (ddlComareI.SelectedValue == "0")
                strInstall = " = '" + txtInstallDt.Text.Trim();
            else if (ddlComareI.SelectedValue == "1")
                strInstall = " >= '" + txtInstallDt.Text.Trim();
            else if (ddlComareI.SelectedValue == "2")
                strInstall = " <= '" + txtInstallDt.Text.Trim();
            else if (ddlComareI.SelectedValue == "3")
                strInstall = " > '" + txtInstallDt.Text.Trim();
            else if (ddlComareI.SelectedValue == "4")
                strInstall = " < '" + txtInstallDt.Text.Trim();
            else
                strInstall = " = '" + txtInstallDt.Text.Trim();
        }
        objProp_User.InstallDateString = strInstall;
        objProp_User.InstallDate = string.Empty;

        _GetElev.InstallDateString = strInstall;
        _GetElev.InstallDate = string.Empty;

        string strDays = string.Empty;
        if (txtLastServiceDt.Text.Trim() != string.Empty)
        {
            if (ddlcompare.SelectedValue == "0")
                strDays = " = '" + txtLastServiceDt.Text.Trim();
            else if (ddlcompare.SelectedValue == "1")
                strDays = " >= '" + txtLastServiceDt.Text.Trim();
            else if (ddlcompare.SelectedValue == "2")
                strDays = " <= '" + txtLastServiceDt.Text.Trim();
            else if (ddlcompare.SelectedValue == "3")
                strDays = " > '" + txtLastServiceDt.Text.Trim();
            else if (ddlcompare.SelectedValue == "4")
                strDays = " < '" + txtLastServiceDt.Text.Trim();
            else
                strDays = " = '" + txtLastServiceDt.Text.Trim();
        }
        objProp_User.ServiceDate = string.Empty;
        objProp_User.ServiceDateString = strDays;

        _GetElev.ServiceDate = string.Empty;
        _GetElev.ServiceDateString = strDays;

        string strPrice = string.Empty;
        if (txtPrice.Text.Trim() != string.Empty)
        {
            if (ddlCompareP.SelectedValue == "0")
                strPrice = " = '" + txtPrice.Text.Trim();
            else if (ddlCompareP.SelectedValue == "1")
                strPrice = " >= '" + txtPrice.Text.Trim();
            else if (ddlCompareP.SelectedValue == "2")
                strPrice = " <= '" + txtPrice.Text.Trim();
            else if (ddlCompareP.SelectedValue == "3")
                strPrice = " > '" + txtPrice.Text.Trim();
            else if (ddlCompareP.SelectedValue == "4")
                strPrice = " < '" + txtPrice.Text.Trim();
            else
                strPrice = " = '" + txtPrice.Text.Trim();
        }
        objProp_User.Price = string.Empty;
        objProp_User.PriceString = strPrice;

        objProp_User.Manufacturer = txtManufact.Text;
        objProp_User.CustomerID = Convert.ToInt32(Session["custid"].ToString());

        _GetElev.Price = string.Empty;
        _GetElev.PriceString = strPrice;

        _GetElev.Manufacturer = txtManufact.Text;
        _GetElev.CustomerID = Convert.ToInt32(Session["custid"].ToString());
        if (Session["type"].ToString() == "c")
        {
            DataTable dtcust = new DataTable();
            dtcust = (DataTable)Session["userinfo"];
            int RoleID = 0;
            if (dtcust.Rows.Count > 0)
            {
                RoleID = Convert.ToInt32(dtcust.Rows[0]["roleid"]);
                objProp_User.RoleID = RoleID;
                _GetElev.RoleID = RoleID;
            }
        }
        objProp_User.Status = -1;
        _GetElev.Status = -1;

        List<GetElevViewModel> _lstGetElev = new List<GetElevViewModel>();

        if (IsAPIIntegrationEnable == "YES")
        {
            string APINAME = "EquipmentAPI/MassMCP_GetElev";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetElev);

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            serializer.MaxJsonLength = Int32.MaxValue;

            _lstGetElev = serializer.Deserialize<List<GetElevViewModel>>(_APIResponse.ResponseData);
            ds = CommonMethods.ToDataSet<GetElevViewModel>(_lstGetElev);
        }
        else
        {
            ds = objBL_User.getElev(objProp_User);
        }

        lblRecordCount.Text = ds.Tables[0].Rows.Count.ToString() + " Record(s) Found.";

        gvEquip.DataSource = ds.Tables[0];
        gvEquip.DataBind();

    }
    protected void lnkSearch_Click(object sender, EventArgs e)
    {
        GetDataAll();
    }
    protected void lnkClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("equipments.aspx");
    }
    protected void ddlSearch_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlSearch.SelectedValue == "e.Status")
        {
            rbStatus.Visible = true;
            txtSearch.Visible = false;
            ddlType.Visible = false;
            ddlServiceType.Visible = false;
            ddlCategory.Visible = false;
        }
        else if (ddlSearch.SelectedValue == "e.type")
        {
            rbStatus.Visible = false;
            txtSearch.Visible = false;
            ddlType.Visible = true;
            ddlServiceType.Visible = false;
            ddlCategory.Visible = false;
        }
        else if (ddlSearch.SelectedValue == "e.Cat")
        {
            rbStatus.Visible = false;
            txtSearch.Visible = false;
            ddlType.Visible = false;
            ddlServiceType.Visible = true;
            ddlCategory.Visible = false;
        }
        else if (ddlSearch.SelectedValue == "e.Category")
        {
            rbStatus.Visible = false;
            txtSearch.Visible = false;
            ddlType.Visible = false;
            ddlServiceType.Visible = false;
            ddlCategory.Visible = true;
        }
        else
        {
            txtSearch.Visible = true;
            rbStatus.Visible = false;
            ddlType.Visible = false;
            ddlServiceType.Visible = false;
            ddlCategory.Visible = false;
        }
    }

    protected void lnkShowAll_Click(object sender, EventArgs e)
    {
        ddlSearch.SelectedIndex = 0;
        ddlType.SelectedIndex = 0;
        ddlServiceType.SelectedIndex = 0;
        txtSearch.Text = string.Empty;
        txtManufact.Text = string.Empty;
        txtPrice.Text = string.Empty;
        txtInstallDt.Text = string.Empty;
        txtLastServiceDt.Text = string.Empty;
        ddlSearch_SelectedIndexChanged(sender, e);
        GetDataAll();
    }
    protected void lnkClear_Click(object sender, EventArgs e)
    {
        objgn.ResetFormControlValues(this);
        ddlSearch_SelectedIndexChanged(sender, e);
    }
    private void FillRepTemplate()
    {
        DataSet ds = new DataSet();
        objPropCustomer.ConnConfig = Session["config"].ToString();
        _GetRepTemplateName.ConnConfig = Session["config"].ToString();

        List<GetRepTemplateNameViewModel> _lstGetRepTemplateName = new List<GetRepTemplateNameViewModel>();

        if (IsAPIIntegrationEnable == "YES")
        {
            string APINAME = "EquipmentAPI/MassMCP_GetRepTemplateName";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetRepTemplateName);

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            serializer.MaxJsonLength = Int32.MaxValue;

            _lstGetRepTemplateName = serializer.Deserialize<List<GetRepTemplateNameViewModel>>(_APIResponse.ResponseData);
            ds = CommonMethods.ToDataSet<GetRepTemplateNameViewModel>(_lstGetRepTemplateName);
        }
        else
        {
            ds = objBL_Customer.getRepTemplateName(objPropCustomer);
        }

        gvSelectTemplate.DataSource = ds;
        gvSelectTemplate.DataBind();
    }
    protected void cbRepTemplate_SelectedIndexChanged(object sender, EventArgs e)
    {
        LinkButton lnk = (LinkButton)sender;
        GridViewRow gvRow = (GridViewRow)lnk.Parent.Parent;
        Label lblRepTempID = (Label)gvRow.FindControl("lblRepTempId");
        TextBox txtStartDate = (TextBox)gvRow.FindControl("txtStartDate");

        AppendTemplateItemstoGrid(Convert.ToInt32(lblRepTempID.Text), txtStartDate.Text.Trim(), true);
    }
    private void AppendTemplateItemstoGrid(int TemplateID, string Startdate, bool Unique)
    {
        DataTable dtItems = CreateTableFromGrid();
        //DataTable dt = (DataTable)Session["templtableEquipment"];
        //Session["templtableEquipment"] = null;

        DataSet dsNewItems = new DataSet();
        objPropCustomer.ConnConfig = Session["config"].ToString();
        int Elev = 0;
        objPropCustomer.TemplateID = TemplateID;

        _GetTemplateItemByID.ConnConfig = Session["config"].ToString();
        _GetTemplateItemByID.TemplateID = TemplateID;

        List<GetTemplateItemByIDViewModel> _lstGetTemplateItemByID = new List<GetTemplateItemByIDViewModel>();

        if (IsAPIIntegrationEnable == "YES")
        {
            string APINAME = "EquipmentAPI/MassMCP_GetTemplateItemByID";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetTemplateItemByID);

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            serializer.MaxJsonLength = Int32.MaxValue;

            _lstGetTemplateItemByID = serializer.Deserialize<List<GetTemplateItemByIDViewModel>>(_APIResponse.ResponseData);
            dsNewItems = CommonMethods.ToDataSet<GetTemplateItemByIDViewModel>(_lstGetTemplateItemByID);
        }
        else
        {
            dsNewItems = objBL_Customer.getTemplateItemByID(objPropCustomer);
        }


        foreach (DataRow dr in dsNewItems.Tables[0].Rows)
        {
            //DataRow[] drSelect = dtItems.Select("equipt=" + Convert.ToInt32(dr["EquipT"]) + " and fdesc='" + dr["fDesc"].ToString() + "'");
            int count = 0;

            if (Unique)
            {
                DataRow[] drSelect = dtItems.Select("Code='" + dr["Code"].ToString() + "'");
                count = drSelect.Count();
            }

            if (count == 0)
            {
                DataRow drNew = dtItems.NewRow();
                drNew["Code"] = dr["Code"].ToString();
                drNew["Name"] = dr["Name"].ToString();
                drNew["EquipT"] = dr["EquipT"].ToString();
                drNew["Elev"] = Elev;
                drNew["fDesc"] = dr["fDesc"].ToString();
                drNew["Line"] = dtItems.Rows.Count;
                if (Startdate != string.Empty)
                {
                    DateTime dtst = new DateTime();
                    if (DateTime.TryParse(Startdate, out dtst))
                    {
                        drNew["Lastdate"] = dtst;
                        if (Convert.ToInt32(dr["Frequency"].ToString()) > -1)
                            drNew["NextDateDue"] = calculateNextDueDate(dtst, Convert.ToInt32(dr["Frequency"].ToString()));
                    }
                    else
                    {
                        drNew["Lastdate"] = DBNull.Value;
                        drNew["NextDateDue"] = DBNull.Value;
                    }
                }
                else
                {
                    drNew["Lastdate"] = DBNull.Value;
                    drNew["NextDateDue"] = DBNull.Value;
                }

                drNew["Frequency"] = dr["Frequency"].ToString();
                drNew["Section"] = dr["Section"].ToString();
                drNew["Notes"] = dr["Notes"].ToString();
                dtItems.Rows.InsertAt(drNew, 0);
            }
        }

        //Session["templtableEquipment"] = dt;
        gvTemplateItems.DataSource = dtItems;
        gvTemplateItems.DataBind();
    }
    private DataTable CreateTableFromGrid()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Code", typeof(string));
        dt.Columns.Add("Name", typeof(string));
        dt.Columns.Add("EquipT", typeof(int));
        dt.Columns.Add("Elev", typeof(int));
        dt.Columns.Add("fDesc", typeof(string));
        dt.Columns.Add("Line", typeof(int));
        dt.Columns.Add("Lastdate", typeof(DateTime));
        dt.Columns.Add("NextDateDue", typeof(DateTime));
        dt.Columns.Add("Frequency", typeof(int));
        dt.Columns.Add("Section", typeof(string));
        dt.Columns.Add("Notes", typeof(string));

        foreach (GridViewRow gr in gvTemplateItems.Rows)
        {
            //if (((TextBox)gr.FindControl("lblDesc")).Text.Trim() != string.Empty)
            //{
            DataRow dr = dt.NewRow();
            dr["Code"] = ((Label)gr.FindControl("lblCode")).Text.Trim();
            dr["Name"] = ((Label)gr.FindControl("lblName")).Text.Trim();
            dr["EquipT"] = ((Label)gr.FindControl("lblEquipT")).Text.Trim();
            dr["Elev"] = 0;
            dr["fDesc"] = ((TextBox)gr.FindControl("lblDesc")).Text.Trim();
            dr["Line"] = dt.Rows.Count;
            if (((TextBox)gr.FindControl("txtLdate")).Text.Trim() == string.Empty)
            {
                dr["Lastdate"] = DBNull.Value;
            }
            else
            {
                dr["Lastdate"] = Convert.ToDateTime(((TextBox)gr.FindControl("txtLdate")).Text.Trim()).ToShortDateString();
            }

            if (((TextBox)gr.FindControl("txtDuedate")).Text.Trim() == string.Empty)
            {
                dr["NextDateDue"] = DBNull.Value;
            }
            else
            {
                dr["NextDateDue"] = Convert.ToDateTime(((TextBox)gr.FindControl("txtDuedate")).Text.Trim()).ToShortDateString();
            }

            dr["Frequency"] = ((DropDownList)gr.FindControl("ddlFreq")).SelectedItem.Value;
            dr["Section"] = ((TextBox)gr.FindControl("txtSection")).Text.Trim();
            dr["Notes"] = ((TextBox)gr.FindControl("txtNotes")).Text.Trim();
            dt.Rows.Add(dr);
            //}
        }

        //Session["templtableEquipment"] = dt;
        return dt;
    }
    private DataTable CreateItemsForEquipments()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Code", typeof(string));
        dt.Columns.Add("Name", typeof(string));
        dt.Columns.Add("EquipT", typeof(int));
        dt.Columns.Add("Elev", typeof(int));
        dt.Columns.Add("fDesc", typeof(string));
        dt.Columns.Add("Line", typeof(int));
        dt.Columns.Add("Lastdate", typeof(DateTime));
        dt.Columns.Add("NextDateDue", typeof(DateTime));
        dt.Columns.Add("Frequency", typeof(int));
        dt.Columns.Add("Section", typeof(string));
        dt.Columns.Add("Notes", typeof(string));

        foreach (GridViewRow grequip in gvEquip.Rows)
        {
            foreach (GridViewRow gr in gvTemplateItems.Rows)
            {
                //if (((TextBox)gr.FindControl("lblDesc")).Text.Trim() != string.Empty)
                //{
                DataRow dr = dt.NewRow();
                dr["Code"] = ((Label)gr.FindControl("lblCode")).Text.Trim();
                dr["Name"] = ((Label)gr.FindControl("lblName")).Text.Trim();
                dr["EquipT"] = ((Label)gr.FindControl("lblEquipT")).Text.Trim();
                dr["Elev"] = ((Label)grequip.FindControl("lblID")).Text.Trim();
                dr["fDesc"] = ((TextBox)gr.FindControl("lblDesc")).Text.Trim();
                dr["Line"] = dt.Rows.Count;
                if (((TextBox)gr.FindControl("txtLdate")).Text.Trim() == string.Empty)
                {
                    dr["Lastdate"] = DBNull.Value;
                }
                else
                {
                    dr["Lastdate"] = Convert.ToDateTime(((TextBox)gr.FindControl("txtLdate")).Text.Trim()).ToShortDateString();
                }

                if (((TextBox)gr.FindControl("txtDuedate")).Text.Trim() == string.Empty)
                {
                    dr["NextDateDue"] = DBNull.Value;
                }
                else
                {
                    dr["NextDateDue"] = Convert.ToDateTime(((TextBox)gr.FindControl("txtDuedate")).Text.Trim()).ToShortDateString();
                }

                dr["Frequency"] = ((DropDownList)gr.FindControl("ddlFreq")).SelectedItem.Value;
                dr["Section"] = ((TextBox)gr.FindControl("txtSection")).Text.Trim();
                dr["Notes"] = ((TextBox)gr.FindControl("txtNotes")).Text.Trim();
                dt.Rows.Add(dr);
                //}
            }
        }

        //Session["templtableEquipment"] = dt;
        return dt;
    }

    //API
    private DataTable ItemsForEquipEmptyDatatable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Code", typeof(string));
        dt.Columns.Add("Name", typeof(string));
        dt.Columns.Add("EquipT", typeof(int));
        dt.Columns.Add("Elev", typeof(int));
        dt.Columns.Add("fDesc", typeof(string));
        dt.Columns.Add("Line", typeof(int));
        dt.Columns.Add("Lastdate", typeof(DateTime));
        dt.Columns.Add("NextDateDue", typeof(DateTime));
        dt.Columns.Add("Frequency", typeof(int));
        dt.Columns.Add("Section", typeof(string));
        dt.Columns.Add("Notes", typeof(string));

        DataRow dr = dt.NewRow();
        dr["Code"] = "";
        dr["Name"] = "";
        dr["EquipT"] = "0";
        dr["Elev"] = "";
        dr["fDesc"] = "";
        dr["Line"] = "0";
        dr["Lastdate"] = null;
        dr["NextDateDue"] = null;
        dr["Frequency"] = "0";
        dr["Section"] = "";
        dr["Notes"] = "";
        dt.Rows.Add(dr);

        return dt;
    }
    
    protected DateTime calculateNextDueDate(DateTime dt, int frequencyIndex)
    {
        switch (frequencyIndex)
        {
            case 0: dt = dt.AddDays(1); break;
            case 1: dt = dt.AddDays(7); break;
            case 2: dt = dt.AddDays(14); break;
            case 3: dt = dt.AddMonths(1); break;
            case 4: dt = dt.AddMonths(2); break;
            case 5: dt = dt.AddMonths(3); break;
            case 6: dt = dt.AddMonths(6); break;
            case 7: dt = dt.AddYears(1); break;
            case 8: dt = dt; break;
            case 9: dt = dt.AddMonths(4); break;
            case 10: dt = dt.AddYears(2); break;
            case 11: dt = dt.AddYears(3); break;
            case 12: dt = dt.AddYears(5); break;
            case 13: dt = dt.AddYears(7); break;
            default:
                //default stuff
                break;
        }
        return dt;
    }
    protected void txtLdate_TextChanged(object sender, EventArgs e)
    {

        TextBox txtLastDate = (TextBox)sender;

        GridViewRow gvRow = (GridViewRow)txtLastDate.Parent.Parent;
        DropDownList ddlFrequency = (DropDownList)gvRow.FindControl("ddlFreq");
        TextBox txtDueDate = (TextBox)gvRow.FindControl("txtDuedate");

        if (Convert.ToInt32(ddlFrequency.SelectedValue) > -1 && txtLastDate.Text != "")
        {
            string[] arr = txtLastDate.Text.Split('/');
            DateTime dt = new DateTime(Convert.ToInt32(arr[2]), Convert.ToInt32(arr[0]), Convert.ToInt32(arr[1]));

            txtDueDate.Text = calculateNextDueDate(dt, Convert.ToInt32(ddlFrequency.SelectedItem.Value)).ToShortDateString();
        }
        else
        {
            txtDueDate.Text = "";
        }
    }
    protected void ddlFreq_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddlFrequency = (DropDownList)sender;
        GridViewRow gvRow = (GridViewRow)ddlFrequency.Parent.Parent;

        TextBox txtLastDate = (TextBox)gvRow.FindControl("txtLdate");
        TextBox txtDueDate = (TextBox)gvRow.FindControl("txtDuedate");


        if (Convert.ToInt32(ddlFrequency.SelectedValue) > -1 && txtLastDate.Text != "")
        {
            string[] arr = txtLastDate.Text.Split('/');
            DateTime dt = new DateTime(Convert.ToInt32(arr[2]), Convert.ToInt32(arr[0]), Convert.ToInt32(arr[1]));

            txtDueDate.Text = calculateNextDueDate(dt, Convert.ToInt32(ddlFrequency.SelectedItem.Value)).ToShortDateString();
        }
        else
        {
            txtDueDate.Text = "";
        }

    }
    protected void btnDeleteItem_Click(object sender, EventArgs e)
    {
        //DataTable dt = (DataTable)Session["templtableEquipment"];
        DataTable dt = CreateTableFromGrid();
        int index = 0;
        foreach (GridViewRow gr in gvTemplateItems.Rows)
        {
            CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");
            if (chkSelect.Checked == true)
            {
                dt.Rows.RemoveAt(index);
            }
            else
            {
                index++;
            }
        }
        //Session["templtableEquipment"] = dt;
        gvTemplateItems.DataSource = dt;
        gvTemplateItems.DataBind();
    }
    protected void btnProcess_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = CreateItemsForEquipments();
            dt.Columns.Remove("Name");
            objProp_User.DtItems = dt;
            objProp_User.ConnConfig = Session["config"].ToString();

            if (dt.Rows.Count == 0)
            {
                DataTable returnVal = ItemsForEquipEmptyDatatable();
                _AddMassMCP.DtItems = returnVal;
            }
            else
            {
                _AddMassMCP.DtItems = dt;
            }

            _AddMassMCP.ConnConfig = Session["config"].ToString();

            if (IsAPIIntegrationEnable == "YES")
            {
                string APINAME = "EquipmentAPI/MassMCP_AddMassMCP";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _AddMassMCP);
            }
            else
            {
                objBL_User.AddMassMCP(objProp_User);
            }

            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccUp", "noty({text: 'MCP Processed Successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);

        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
}
