using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessEntity;
using BusinessLayer;
using System.Data;
using Telerik.Web.UI;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public partial class CollectionPopup : System.Web.UI.Page
{
    BusinessEntity.CompanyOffice objCompany = new BusinessEntity.CompanyOffice();
    BL_Company objBL_Company = new BL_Company();
    User objPropUser = new User();
    BL_User objBL_User = new BL_User();
    Customer objCustomer = new Customer();
    BL_Customer objBL_Customer = new BL_Customer();
    private static int intCount = 0;
    protected void Page_PreInit(object sender, System.EventArgs e)

    {
       String paramO= HttpUtility.ParseQueryString(Request.RawUrl).Get("o");

        if (Request.QueryString["o"] != null)
        {
            Control header = Page.Master.FindControl("divHeader");
            header.Visible = false;
            Control menu = Page.Master.FindControl("menu");
            menu.Visible = false;

        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userid"] == null)
        {
            Response.Redirect("login.aspx");
        }
        PagePermission();
        if (!IsPostBack)
        {
            ViewState["mode"] = 0;
            ViewState["editcon"] = 0;
            Session["contacttable"] = null;
            #region Get Customer and Location ID
            liLocationRemarks.Style["display"] = "none";
            try
            {
                //string customerName = Convert.ToString(Request.QueryString["uid"]);
                //string locationName = Convert.ToString(Request.QueryString["Loc"]);
                string customerName = Convert.ToString(HttpUtility.ParseQueryString(Request.RawUrl).Get("uid"));
                string locationName = Convert.ToString(HttpUtility.ParseQueryString(Request.RawUrl).Get("Loc"));
                string Ref = Convert.ToString(Request.QueryString["Ref"]);
                ViewState["refIDs"] = Ref;
                if (locationName != "")
                {
                    this.Title = customerName + " / " + locationName;
                    liLocationRemarks.Style["display"] = "block";
                    divPrintInvoice.Visible = true;
                    divEmailInvoice.Visible = true;
                    divCustStatement.Visible = true;
                    lnkSave.Visible = true;
                    divInvoiceStatements.Visible = true;
                    chkShowAllNote.Text = "Show Customer Notes";

                }
                else
                {
                    this.Title = customerName;
                    divPrintInvoice.Visible = false;
                    divEmailInvoice.Visible = false;
                    divCustStatement.Visible = false;
                    lnkSave.Visible = false;
                    divInvoiceStatements.Visible = false;
                    chkShowAllNote.Text = "Show all Location notes";
                }

                CollectionModel objCollectionModel = new CollectionModel();
                BL_Collection objBL_Collection = new BL_Collection();
                objCollectionModel.ConnConfig = Convert.ToString(Session["config"]);
                objCollectionModel.CustomerName = customerName;
                objCollectionModel.LocationName = locationName;
                DataSet ds = objBL_Collection.GetCustomerLocationIDs(objCollectionModel);
                if (ds != null && ds.Tables.Count > 0)
                {
                    ViewState["uid"] = Convert.ToString(ds.Tables[0].Rows[0][0]);

                    if (ds.Tables[1].Rows.Count >0)
                    {
                        ViewState["loc"] = Convert.ToString(ds.Tables[1].Rows[0]["Loc"]);

                        txtLocationRemarks.Text = Convert.ToString(ds.Tables[1].Rows[0]["Remarks"]);


                        objPropUser.RolId = Convert.ToInt32(ViewState["loc"]);
                        objPropUser.ConnConfig = Session["config"].ToString();
                        DataSet dsLoc = objBL_User.GetLocByID(objPropUser);
                        if (dsLoc != null)
                        {
                            if (Convert.ToString(dsLoc.Tables[0].Rows[0]["PrintInvoice"]) == "True")
                            {
                                chkPrintOnly.Checked = true;
                            }
                            if (Convert.ToString(dsLoc.Tables[0].Rows[0]["EmailInvoice"]) == "True")
                            {
                                chkEmail.Checked = true;
                            }

                            if (Convert.ToBoolean(dsLoc.Tables[0].Rows[0]["NoCustomerStatement"]))
                            {
                                chkNoCustStatement.Checked = true;
                            }

                            txtEmailToInv.Text = Convert.ToString(dsLoc.Tables[0].Rows[0]["custom12"]);
                            txtEmailCCInv.Text = Convert.ToString(dsLoc.Tables[0].Rows[0]["custom13"]);
                        }
                    }
                    
                }
                DataSet dsNote = getCollectionNote();
                if (dsNote != null && dsNote.Tables.Count > 1)
                {
                    txtDefaultNotes.Text = dsNote.Tables[1].Rows[0]["CNotes"].ToString();
                }


            }
            catch
            {
            }

            #endregion

            BindInvoiceTemplate();
        }
    }

    private void BindInvoiceTemplate()
    {
        drpInvoiceTemplate.Items.Clear();

        string Invoicepath = Server.MapPath("StimulsoftReports/Invoices/");
        DirectoryInfo dirPath = new DirectoryInfo(Invoicepath);
        FileInfo[] Files = dirPath.GetFiles("*.mrt");
        foreach (FileInfo file in Files)
        {
            string FileName = string.Empty;
            if (file.Name.Contains(".mrt"))
                FileName = file.Name.Replace(".mrt", " ");
            if (!FileName.Contains("Preview"))
            {
                drpInvoiceTemplate.Items.Add((FileName));
            }
        }

        drpInvoiceTemplate.DataBind();
    }
    private void PagePermission()
    {

        if (Convert.ToString(Session["type"]) != "am" && Convert.ToString(Session["type"]) != "c")
        {
            DataTable ds = new DataTable();
            ds = (DataTable)Session["userinfo"];
            //Contact---------------------->
            string ContactPermission = ds.Rows[0]["ContactPermission"] == DBNull.Value ? "YYYY" : ds.Rows[0]["ContactPermission"].ToString();
            hdnAddeContact.Value = ContactPermission.Length < 1 ? "Y" : ContactPermission.Substring(0, 1);
            hdnEditeContact.Value = ContactPermission.Length < 2 ? "Y" : ContactPermission.Substring(1, 1);
            hdnDeleteContact.Value = ContactPermission.Length < 3 ? "Y" : ContactPermission.Substring(2, 1);
            hdnViewContact.Value = ContactPermission.Length < 4 ? "Y" : ContactPermission.Substring(3, 1);

            if (hdnAddeContact.Value == "N")
            {
                lnkAddnew.Enabled = false;
            }
            if (hdnEditeContact.Value == "N")
            {
                btnEdit.Enabled = false;
            }
            if (hdnDeleteContact.Value == "N")
            {
                btnDelete.Enabled = false;
            }

            string CollectionPermission = ds.Rows[0]["Collection"] == DBNull.Value ? "YYYYYY" : ds.Rows[0]["Collection"].ToString();

            //string RCollectionView = CollectionPermission.Length < 4 ? "Y" : CollectionPermission.Substring(3, 1);
            string RCollectionEdit = CollectionPermission.Length < 2 ? "Y" : CollectionPermission.Substring(1, 1);
            //string RCollectionReport = CollectionPermission.Length < 6 ? "Y" : CollectionPermission.Substring(5, 1);

            if (RCollectionEdit == "N")
            {
                lnkSaveNote.Visible = false;
            }
        }
    }
    protected void lnkAddnew_Click(object sender, EventArgs e)
    {
        if (hdnAddeContact.Value == "Y")
        {
            contactWindow.Title = "Add Contact";
            txtContcName.Text = "";
            txtTitle.Text = "";
            txtContPhone.Text = "";
            txtContFax.Text = "";
            txtContCell.Text = "";
            txtContEmail.Text = "";
            chkEmailTicket.Checked = false;
            chkEmailInvoice.Checked = false;
            chkShutdownA.Checked = false;
            chkTestProposals.Checked = false;
            ViewState["editcon"] = 0;
            ViewState["ColContactEmail"] = "";
            string script = "function f(){$find(\"" + contactWindow.ClientID + "\").show(); Sys.Application.remove_load(f);}Sys.Application.add_load(f); Materialize.updateTextFields();";
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
        }
    }
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        contactWindow.Title = "Edit Contact";
        foreach (GridDataItem di in RadGrid_gvContacts.SelectedItems)
        {
            DataTable dt = (DataTable)Session["contacttable"];
            HiddenField hdnSelected = (HiddenField)di.Cells[0].FindControl("hdnSelected");
            Label lblindex = (Label)di.Cells[0].FindControl("lblindex");
            HiddenField hdContactID = (HiddenField)di.FindControl("hdContactID");
            HiddenField hdCType = (HiddenField)di.FindControl("hdCType");
            DataRow dr = dt.Rows[Convert.ToInt32(lblindex.Text)];


            txtContcName.Text = dr["Name"].ToString();
            txtContPhone.Text = dr["Phone"].ToString();
            txtContFax.Text = dr["Fax"].ToString();
            txtContCell.Text = dr["Cell"].ToString();
            txtContEmail.Text = dr["Email"].ToString();
            txtTitle.Text = dr["Title"].ToString();
            chkEmailTicket.Checked = Convert.ToBoolean(dr["EmailTicket"]);
            chkEmailInvoice.Checked = Convert.ToBoolean(dr["EmailRecInvoice"]);
            chkShutdownA.Checked = Convert.ToBoolean(dr["ShutdownAlert"]);
            chkTestProposals.Checked = Convert.ToBoolean(dr["EmailRecTestProp"]);
            ViewState["ColContactID"] = hdContactID.Value;
            ViewState["ColCType"] = hdCType.Value;
            ViewState["editcon"] = 1;
            ViewState["index"] = lblindex.Text;
            ViewState["ColContactEmail"] = txtContEmail.Text;

        }

        string script = "function f(){$find(\"" + contactWindow.ClientID + "\").show(); Sys.Application.remove_load(f);}Sys.Application.add_load(f); Materialize.updateTextFields();";
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
    }
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = (DataTable)Session["contacttable"];

            foreach (GridDataItem di in RadGrid_gvContacts.Items)
            {
                CheckBox chkSelected = (CheckBox)di["ClientSelectColumn"].Controls[0];
                Label lblindex = (Label)di.FindControl("lblIndex");

                if (chkSelected.Checked == true)
                {
                    dt.Rows.RemoveAt(Int32.Parse(lblindex.Text));

                    #region Delete From Database
                    HiddenField hdContactID = (HiddenField)di.FindControl("hdContactID");
                    HiddenField hdEmail = (HiddenField)di.FindControl("hdEmail");
                    objPropUser.ConnConfig = Session["config"].ToString();
                    objPropUser.ID = Convert.ToInt32(hdContactID.Value);
                    objBL_User.DeleteContactByID(objPropUser);
                    #endregion

                    #region Add InvoiceStatement to Custom12
                    String locID = Convert.ToString(ViewState["loc"]);
                    if (locID != "")
                    {
                        String MainMail = txtEmailToInv.Text;

                        MainMail = MainMail.Replace("," + hdEmail.Value, "");

                        txtEmailToInv.Text = MainMail;

                        objPropUser.Custom1 = MainMail;
                        objPropUser.RolId = Convert.ToInt32(locID);
                        objPropUser.ConnConfig = Session["config"].ToString();
                        objBL_User.UpdateLocCustom12(objPropUser);
                    }
                    #endregion
                }
            }



            dt.AcceptChanges();
            ClientScript.RegisterStartupScript(Page.GetType(), "keySucc", "noty({text: 'Contact deleted successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
            Session["contacttable"] = dt;
            RadGrid_gvContacts.VirtualItemCount = dt.Rows.Count;
            RadGrid_gvContacts.DataSource = dt;
            RadGrid_gvContacts.Rebind();

            //if (ViewState["mode"].ToString() == "1")
            //{
            //    SubmitContact();
            //}
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }

    }
    protected void lnkContactSave_Click(object sender, EventArgs e)
    {
        DataTable dt = (DataTable)Session["contacttable"];

        DataRow dr = dt.NewRow();
        dr["contactid"] = Convert.ToInt32(ViewState["ColContactID"]);
        dr["Name"] = Truncate(txtContcName.Text, 50);
        dr["Phone"] = Truncate(txtContPhone.Text, 50);
        dr["Fax"] = Truncate(txtContFax.Text, 22);
        dr["Cell"] = Truncate(txtContCell.Text, 22);
        dr["Email"] = Truncate(txtContEmail.Text, 50);
        dr["Title"] = Truncate(txtTitle.Text, 50);
        dr["EmailTicket"] = chkEmailTicket.Checked;
        dr["EmailRecInvoice"] = chkEmailInvoice.Checked;
        dr["ShutdownAlert"] = chkShutdownA.Checked;
        dr["EmailRecTestProp"] = chkTestProposals.Checked;

        #region Data Object

        CollectionContacts data = new CollectionContacts();
        data.ConnConfig = Session["config"].ToString();
        data.Name = Truncate(txtContcName.Text, 50);
        data.Phone = Truncate(txtContPhone.Text, 50);
        data.Fax = Truncate(txtContFax.Text, 22);
        data.Cell = Truncate(txtContCell.Text, 22);
        data.Email = Truncate(txtContEmail.Text, 50);
        data.Title = Truncate(txtTitle.Text, 50);
        data.Tickets = chkEmailTicket.Checked;
        data.InvoiceStatements = chkEmailInvoice.Checked;
        data.Shutdown = chkShutdownA.Checked;
        data.Tests = chkTestProposals.Checked;
        #endregion

        if (ViewState["editcon"].ToString() == "1")
        {
            dr["ctype"] = Convert.ToString(ViewState["ColCType"]);

            dt.Rows.RemoveAt(Convert.ToInt32(ViewState["index"]));
            dt.Rows.InsertAt(dr, Convert.ToInt32(ViewState["index"]));
            ViewState["editcon"] = 0;

            #region Edit Contact In Phone Table 
            data.ID = Convert.ToInt32(ViewState["ColContactID"]);
            data.IsUpdate = true;
            objBL_User.UpdateCollectionContact(data);
            #endregion


        }
        else
        {
            #region Edit Contact In Phone Table 
            String loc = Convert.ToString(ViewState["loc"]);
            String cus = Convert.ToString(ViewState["uid"]);
            if (loc == "")
            {
                data.CType = "Customer";
                data.LocID = 0;
                data.CustID = Convert.ToInt32(cus);
            }
            else
            {
                data.CType = "Location";
                data.LocID = Convert.ToInt32(loc);
                data.CustID = Convert.ToInt32(cus);
            }
            data.IsUpdate = false;
            objBL_User.UpdateCollectionContact(data);
            #endregion

            dr["ctype"] = data.CType;
            dt.Rows.Add(dr);
        }

        #region Add InvoiceStatement to Custom12
        String locID = Convert.ToString(ViewState["loc"]);
        if (locID != "")
        {
            String MainMail = txtEmailToInv.Text;
            if (chkEmailInvoice.Checked == true)
            {
                //MainMail = MainMail + "," + txtContEmail.Text;
                MainMail = getNewListEmail(MainMail, txtContEmail.Text, false);
            }
            else
            {
                //MainMail = MainMail.Replace("," + txtContEmail.Text, "");
                MainMail = getNewListEmail(MainMail, txtContEmail.Text, true);
            }
            txtEmailToInv.Text = MainMail;

            objPropUser.Custom1 = MainMail;
            objPropUser.RolId = Convert.ToInt32(locID);
            objPropUser.ConnConfig = Session["config"].ToString();
            objBL_User.UpdateLocCustom12(objPropUser);
        }
        #endregion

        dt.AcceptChanges();

        Session["contacttable"] = dt;
        RadGrid_gvContacts.VirtualItemCount = dt.Rows.Count;
        RadGrid_gvContacts.DataSource = dt;
        RadGrid_gvContacts.Rebind();


        ClearContact();
        //TogglePopup();

        //if (ViewState["mode"].ToString() == "1")
        //{
        //    SubmitContact();
        //}

        string script = "function f(){$find(\"" + contactWindow.ClientID + "\").close(); Sys.Application.remove_load(f);}Sys.Application.add_load(f); Materialize.updateTextFields();";
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
    }
    public string Truncate(string Value, int length)
    {
        if (Value.Length > length)
        {
            Value = Value.Substring(0, length);
        }
        return Value;
    }
    private void ClearContact()
    {
        txtContcName.Text = string.Empty;
        txtContPhone.Text = string.Empty;
        txtContFax.Text = string.Empty;
        txtContCell.Text = string.Empty;
        txtContEmail.Text = string.Empty;
        txtTitle.Text = string.Empty;
    }

    #region Contacts
    protected void RadGrid_gvContacts_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            RadGrid_gvContacts.AllowCustomPaging = !ShouldApplySortFilterOrGroup();
            if (ViewState["uid"] != null)
            {
                BindCollectionpop();
            }
            else
            {
                RadGrid_gvContacts.DataSource = string.Empty;
            }
        }
        catch { }
    }
    protected void RadGrid_gvContacts_PreRender(object sender, EventArgs e)
    {
        RowSelect();
    }
    bool isGrouping = false;
    public bool ShouldApplySortFilterOrGroup()
    {
        return RadGrid_gvContacts.MasterTableView.FilterExpression != "" ||
            (RadGrid_gvContacts.MasterTableView.GroupByExpressions.Count > 0 || isGrouping) ||
            RadGrid_gvContacts.MasterTableView.SortExpressions.Count > 0;
    }
    private void RowSelect()
    {
        foreach (GridDataItem item in RadGrid_gvContacts.Items)
        {

            HiddenField hdnSelected = (HiddenField)item.FindControl("hdnSelected");
            Label lblMail = (Label)item.FindControl("lblEmail");
            CheckBox chkSelect = (CheckBox)item.FindControl("chkSelect");
            CheckBox chkShutdown = (CheckBox)item.FindControl("chkShutdown");
            if (hdnEditeContact.Value == "Y")
            {
                item.Attributes["ondblclick"] = "clickEdit('" + hdnSelected.ClientID + "','" + chkSelect.ClientID + "','" + btnEdit.ClientID + "');";
            }
            else
            {
                chkSelect.Enabled = chkShutdown.Enabled = false;
                item.Attributes["ondblclick"] = "   noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false,dismissQueue:true });";
            }

        }
    }
    #endregion
    #region Collection Notes
    protected void lnkSaveNote_Click(object sender, EventArgs e)
    {
        

        try
        {
            Customer objProp_Customer = new Customer();

            objProp_Customer.ConnConfig = Session["config"].ToString();
            objProp_Customer.RevisionNotes = txtCollectionNote.Text.Trim();
            objProp_Customer.RevisionCreated = DateTime.Now;
            objProp_Customer.RevisionUser = Convert.ToString(Session["username"]);
            objProp_Customer.DefaultNote = txtDefaultNotes.Text.Trim();

            //objProp_Customer.OwnerID = Convert.ToInt32(Request.QueryString["uid"]);
            objProp_Customer.OwnerID = Convert.ToInt32(ViewState["uid"]);
            objProp_Customer.LocID = ViewState["loc"] == null ? 0 : Convert.ToInt32(ViewState["loc"]);
            objBL_Customer.AddCollectionNotes(objProp_Customer);
            txtCollectionNote.Text = "";
            RadGrid_CollectionNotes.Rebind();
            // getAllCollectionNotes();
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyNoteSucc", "noty({text: 'Notes added successfully', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "updateparent", "updateparent();", true);
        }
        catch (Exception ex)
        {

        }
    }

    private void BindCollectionpop()
    {
        try
        {
            // if (Request.QueryString["uid"] != null)
            if (ViewState["uid"] != null)
            {
                // objPropUser.CustomerID = Convert.ToInt32(Request.QueryString["uid"]);
                objPropUser.CustomerID = Convert.ToInt32(ViewState["uid"]);
                if (ViewState["loc"] == null)
                {
                    objPropUser.LocID = 0;
                }
                else
                {
                    objPropUser.LocID = Convert.ToInt32(ViewState["loc"]);
                }

                objPropUser.DBName = Session["dbname"].ToString();
                objPropUser.ConnConfig = Session["config"].ToString();
                DataSet ds = new DataSet();
                ds = objBL_User.getCustomerByID(objPropUser, new GeneralFunctions().GetSalesAsigned());
                ViewState["rolid"] = ds.Tables[0].Rows[0]["rol"].ToString();
                txtCustomerRemarks.Text = ds.Tables[0].Rows[0]["Remarks"].ToString();
                ViewState["mode"] = "1";

                DataSet dss = objBL_User.GetCustomerLocationContacts(objPropUser);
                RadGrid_gvContacts.VirtualItemCount = dss.Tables[0].Rows.Count;
                RadGrid_gvContacts.DataSource = dss.Tables[0];
                Session["contacttable"] = dss.Tables[0];
            }
        }
        catch { }
    }
    protected void RadGrid_gvContacts_ItemCreated(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridPagerItem)
        {
            var dropDown = (RadComboBox)e.Item.FindControl("PageSizeComboBox");
            var totalCount = ((GridPagerItem)e.Item).Paging.DataSourceCount;

            if (totalCount == 0) totalCount = 1000;

            GeneralFunctions obj = new GeneralFunctions();

            var sizes = obj.TelerikPageSize(totalCount);

            dropDown.Items.Clear();

            foreach (var size in sizes)
            {
                var cboItem = new RadComboBoxItem() { Text = size.Key, Value = size.Value };
                cboItem.Attributes.Add("ownerTableViewId", e.Item.OwnerTableView.ClientID);
                if (e.Item.OwnerTableView.PageSize.ToString() == size.Value) cboItem.Selected = true;
                dropDown.Items.Add(cboItem);
            }
        }
    }
    #endregion

    protected void lnkCustomerRemarks_Click(object sender, EventArgs e)
    {
        try
        {
            if (ViewState["uid"] != null)
            {
                Customer objProp_Customer = new Customer();

                objProp_Customer.ConnConfig = Session["config"].ToString();
                objProp_Customer.OwnerID = Convert.ToInt32(ViewState["uid"]);
                objProp_Customer.LocID = 0;
                objProp_Customer.Remarks = txtCustomerRemarks.Text;
                objProp_Customer.LastUpdateUser = Convert.ToString(Session["username"]);
                objBL_Customer.UpdateCustomerLocationRemarks(objProp_Customer);
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "noty({text: 'Remarks updated Successfully', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
            }
        }
        catch (Exception ex)
        {

        }
    }

    protected void lnkLocationRemarks_Click(object sender, EventArgs e)
    {
        try
        {
            if (ViewState["loc"] != null)
            {
                Customer objProp_Customer = new Customer();

                objProp_Customer.ConnConfig = Session["config"].ToString();
                objProp_Customer.OwnerID = Convert.ToInt32(ViewState["uid"]);
                objProp_Customer.LocID = Convert.ToInt32(ViewState["loc"]);
                objProp_Customer.Remarks = txtLocationRemarks.Text;
                objProp_Customer.LastUpdateUser = Convert.ToString(Session["username"]);
                objBL_Customer.UpdateCustomerLocationRemarks(objProp_Customer);
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "noty({text: 'Remarks updated Successfully', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
            }
        }
        catch (Exception ex)
        {

        }
    }

    protected void RadGrid_CollectionNotes_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {

        //if (Request.QueryString["uid"] != null)
        if (ViewState["uid"] != null)
        {
           
            DataSet ds = getCollectionNote();
            RadGrid_CollectionNotes.VirtualItemCount = ds.Tables[0].Rows.Count;
            RadGrid_CollectionNotes.DataSource = ds;
        }
    }
    public DataSet getCollectionNote()
    {
        Customer objProp_Customer = new Customer();
        objProp_Customer.ConnConfig = Session["config"].ToString();       
        objProp_Customer.OwnerID = Convert.ToInt32(ViewState["uid"]);
        objProp_Customer.LocID = ViewState["loc"] == null ? 0 : Convert.ToInt32(ViewState["loc"]);
        objProp_Customer.ShowAllNote = chkShowAllNote.Checked;
        DataSet ds = objBL_Customer.GetCollectionNotes(objProp_Customer);
        return ds;
    }
    protected void RadGrid_CollectionNotes_PreRender(object sender, EventArgs e)
    {
        GeneralFunctions obj = new GeneralFunctions();
        obj.CorrectTelerikPager(RadGrid_CollectionNotes);
    }

    protected void RadGrid_CollectionNotes_ItemCreated(object sender, GridItemEventArgs e)
    {

        if (e.Item is GridPagerItem)
        {
            var dropDown = (RadComboBox)e.Item.FindControl("PageSizeComboBox");
            var totalCount = ((GridPagerItem)e.Item).Paging.DataSourceCount;

            GeneralFunctions obj = new GeneralFunctions();
            var sizes = obj.TelerikPageSize(totalCount);

            dropDown.Items.Clear();
            foreach (var size in sizes)
            {
                var cboItem = new RadComboBoxItem() { Text = size.Key, Value = size.Value };
                cboItem.Attributes.Add("ownerTableViewId", e.Item.OwnerTableView.ClientID);
                dropDown.Items.Add(cboItem);
            }
            dropDown.FindItemByValue(e.Item.OwnerTableView.PageSize.ToString()).Selected = true;

        }
    }

    protected void lnkEmail_Command(object sender, CommandEventArgs e)
    {
        if (e.CommandName == "MailPopup")
        {
            string shipmentNumber = Convert.ToString(e.CommandArgument);
            ViewState["SendMail"] = shipmentNumber;
            string script = "function f(){$find(\"" + mailWindow.ClientID + "\").show(); Sys.Application.remove_load(f);}Sys.Application.add_load(f);";
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
        }
    }

    protected void lnkSave_Click(object sender, EventArgs e)
    {
        try
        {
            objPropUser.PrintInvoice = chkPrintOnly.Checked;
            objPropUser.EmailInvoice = chkEmail.Checked;
            objPropUser.NoCustomerStatement = chkNoCustStatement.Checked;
            objPropUser.Custom1 = txtEmailToInv.Text;
            objPropUser.Custom2 = txtEmailCCInv.Text;
            objPropUser.RolId = Convert.ToInt32(ViewState["loc"]);
            objPropUser.ConnConfig = Session["config"].ToString();
            objBL_User.UpdateLocPrintEmail(objPropUser);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "noty({text: 'Loc updated Successfully', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);

        }
        catch (Exception ex)
        {

            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErrContct", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);

        }
    }

    protected void lnkSendMail_Click(object sender, EventArgs e)
    {
        try
        {
            String templateName = Convert.ToString(drpInvoiceTemplate.SelectedValue);
            Session["CollectionInvoiceTemplate"] = templateName;
            String RefIDs = Convert.ToString(ViewState["refIDs"]);
            String SendMail = Convert.ToString(ViewState["SendMail"]);
            Session["CollectionSendMail"] = SendMail;
            String CustomerID = Convert.ToString(ViewState["uid"]);
            Session["CollectionCustID"] = CustomerID;
            String LocID = Convert.ToString(ViewState["loc"]);
            Session["CollectionLocID"] = LocID;
            if (RefIDs != "" || chkInvoiceMail.Checked == false)
            {
                List<BusinessEntity.MailSender> list = new List<MailSender>();
                Int32 ID = 1;
                if (RefIDs.Length > 1)
                {
                    RefIDs = RefIDs.Remove(RefIDs.Length - 1);
                }

                if (chkInvoiceMail.Checked == true)
                {
                    string[] values = RefIDs.Split(',');

                    for (int i = 0; i < values.Length; i++)
                    {
                        String InvoiceID = values[i].Trim();
                        BusinessEntity.MailSender data = new MailSender();
                        data.ID = ID;
                        data.Name = InvoiceID;
                        data.FileName = "Invoice_" + InvoiceID + ".pdf";
                        data.PDFFilePath = "Invoice";
                        list.Add(data);
                        ID = ID + 1;
                    }

                }
                if (chkCustomerStatementMail.Checked == true)
                {
                    BusinessEntity.MailSender data = new MailSender();
                    data.ID = ID;
                    data.Name = "";
                    data.FileName = "CustomerStatement.pdf";
                    data.PDFFilePath = "CustomerStatement";
                    list.Add(data);
                    ID = ID + 1;
                }

                Session["SelectedEstimateTemplate"] = list;
                Response.Redirect("EmailSender.aspx?pagetype=collection");
            }
            else
            {
                ClientScript.RegisterStartupScript(Page.GetType(), "keyErrContct", "noty({text: 'Please select at least one row.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            }
        }
        catch (Exception ex)
        {

        }
    }

    protected void lnkInvoiceStatementsEmailCC_Click(object sender, EventArgs e)
    {

        ViewState["SendMail"] = txtEmailToInv.Text;
        string script = "function f(){$find(\"" + mailWindow.ClientID + "\").show(); Sys.Application.remove_load(f);}Sys.Application.add_load(f);";
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
    }

    protected void lnkInvoiceStatementsEmailTo_Click(object sender, EventArgs e)
    {
        ViewState["SendMail"] = txtEmailCCInv.Text;
        string script = "function f(){$find(\"" + mailWindow.ClientID + "\").show(); Sys.Application.remove_load(f);}Sys.Application.add_load(f);";
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
    }
    private String getNewListEmail(String strEmail, string newEmail, Boolean isDel)
    {
        String[] lsOld = strEmail.Split(',');
        List<string> lsNews = new List<string>();
        foreach (String item in lsOld)
        {
            if (!lsNews.Contains(item) && item.Trim() != "")
            {
                lsNews.Add(item);
            }
        }


        if (isDel)
        {
            var itemToRemove = lsNews.FirstOrDefault(item => item == newEmail);
            lsNews.Remove(itemToRemove);
        }
        else
        {
            if (!lsNews.Contains(newEmail))
            {
                lsNews.Add(newEmail);
            }
        }
        if (ViewState["ColContactEmail"].ToString() != newEmail)
        {
            var itemToRemove = lsNews.Where(item => item == ViewState["ColContactEmail"].ToString()).FirstOrDefault();
            lsNews.Remove(itemToRemove);
        }

        return string.Join(",", lsNews);
    }

    protected void showAllNote_CheckedChanged(object sender, EventArgs e)
    {

    }

    private void getAllCollectionNotes()
    {
        if (HttpUtility.ParseQueryString(Request.RawUrl).Get("fDate") != null)
        {
            DateTime fdate = Convert.ToDateTime(HttpUtility.ParseQueryString(Request.RawUrl).Get("fDate"));
       //Get Customer Note
         DataSet dsCusNote = new DataSet();
            BL_Collection objCusNote = new BL_Collection();
            dsCusNote = objCusNote.GetCollectionCustomerNote(Convert.ToString(Session["config"]), fdate);
            Session["dsCusNote"] = dsCusNote;
            //Get Location Note
            DataSet dsLocNote = new DataSet();
            BL_Collection objLocNote = new BL_Collection();
            dsLocNote = objLocNote.GetCollectionLocationNote(Convert.ToString(Session["config"]), fdate);
            Session["dsLocNote"] = dsLocNote;
        }       

    }

    protected void ibDeleteNote_Click(object sender, ImageClickEventArgs e)
    {
        try
        {           
            ImageButton ibDelete = (ImageButton)sender;
            objBL_Customer.DeleteCollectionNotes(Session["config"].ToString(), Convert.ToInt32(ibDelete.Attributes["data-id"].ToString()));         
            RadGrid_CollectionNotes.Rebind();
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyNoteSucc", "noty({text: 'Notes deleted successfully', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "updateparent", "updateparent();", true);
        }
        catch (Exception ex)
        {

        }
    }    

    protected void RadGrid_CollectionNotes_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            GridEditableItem editedItem = e.Item as GridEditableItem;
            TextBox note = ((TextBox)e.Item.FindControl("txtNotesUpdate"));
            HiddenField hdnNoteID = ((HiddenField)e.Item.FindControl("hdnNoteID"));
            objBL_Customer.UpdateCollectionNotes(Session["config"].ToString(), Convert.ToInt32(hdnNoteID.Value), note.Text, Convert.ToString(Session["username"]));
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyNoteSucc", "noty({text: 'Notes updated successfully', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "updateparent", "updateparent();", true);
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyNoteErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
       
    }

   
}