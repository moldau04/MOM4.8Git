using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessEntity;
using BusinessLayer;
using System.Data;
using Telerik.Web.UI;

public partial class CustomerUser : System.Web.UI.Page
{
    User objPropUser = new User();
    BL_User objBL_User = new BL_User();

    Customer objPropCustomer = new Customer();
    BL_Customer objBL_Customer = new BL_Customer();
    protected DataTable dtGroupData = new DataTable();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userid"] == null)
        {
            Response.Redirect("login.aspx");
        }

        if (!IsPostBack)
        {
            FillGroup(0);
            FillGroup(1);
            FillCustomerType();
            if (Request.QueryString["uid"] != null)
            {
                objPropUser.CustomerID = Convert.ToInt32(Request.QueryString["uid"]);
                objPropUser.DBName = Session["dbname"].ToString();
                objPropUser.ConnConfig = Session["config"].ToString();
                DataSet ds = new DataSet();
                ds = objBL_User.getCustomerByID(objPropUser);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    txtAddress.Text = ds.Tables[0].Rows[0]["Address"].ToString();
                    txtCity.Text = ds.Tables[0].Rows[0]["City"].ToString();
                    txtCName.Text = ds.Tables[0].Rows[0]["name"].ToString();
                    lblCustomerName.Text = ds.Tables[0].Rows[0]["name"].ToString();
                    txtPassword.Text = ds.Tables[0].Rows[0]["password"].ToString();
                    ddlState.SelectedValue = ds.Tables[0].Rows[0]["State"].ToString();
                    txtUserName.Text = ds.Tables[0].Rows[0]["flogin"].ToString();
                    txtZip.Text = ds.Tables[0].Rows[0]["Zip"].ToString();
                    txtRemarks.Text = ds.Tables[0].Rows[0]["Remarks"].ToString();
                    txtMaincontact.Text = ds.Tables[0].Rows[0]["contact"].ToString();
                    txtPhoneCust.Text = ds.Tables[0].Rows[0]["phone"].ToString();
                    txtWebsite.Text = ds.Tables[0].Rows[0]["website"].ToString();
                    txtEmail.Text = ds.Tables[0].Rows[0]["email"].ToString();
                    txtCell.Text = ds.Tables[0].Rows[0]["cellular"].ToString();
                    ddlUserType.SelectedValue = ds.Tables[0].Rows[0]["type"].ToString();
                    ViewState["rolid"] = ds.Tables[0].Rows[0]["rol"].ToString();
                    ddlCustStatus.SelectedValue = ds.Tables[0].Rows[0]["status"].ToString();
                    chkEquipments.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["CPEquipment"]);
                    chkGrpWO.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["groupbyWO"]);
                    chkOpenTicket.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["openticket"]);

                    if (ds.Tables[0].Rows[0]["ledger"].ToString() == "1")
                    {
                        chkScheduleBrd.Checked = true;
                    }
                    if (ds.Tables[0].Rows[0]["ticketd"].ToString() == "1")
                    {
                        chkMap.Checked = true;
                    }
                }
                if (ds.Tables.Count > 2)
                {
                    RadGrid_Location.VirtualItemCount = ds.Tables[2].Rows.Count;
                    RadGrid_Location.DataSource = ds.Tables[2];
                    RadGrid_Location.Rebind();
                }
            }
        }
    }

    protected void lnkClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("users.aspx");
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        Submit();
    }
    private void FillCustomerType()
    {
        DataSet ds = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();
        ds = objBL_User.getCustomerType(objPropUser);
        ddlUserType.DataSource = ds.Tables[0];
        ddlUserType.DataTextField = "Type";
        ddlUserType.DataValueField = "Type";
        ddlUserType.DataBind();
    }

    private void FillGroup(int fillgv)
    {
        DataSet dtGroup = new DataSet();
        objPropCustomer.ConnConfig = Session["config"].ToString();
        objPropCustomer.CustomerID = Convert.ToInt32(Request.QueryString["uid"].ToString());
        dtGroup = objBL_Customer.getLocationRole(objPropCustomer);

        if (fillgv == 1)
        {
            if (dtGroup.Tables[0].Rows.Count > 0)
            {
                gvGroup.DataSource = dtGroup.Tables[0];
                gvGroup.DataBind();
            }
            else
            {
                ShowNoResultFound(dtGroup.Tables[0], gvGroup);
            }
        }
        else
        {
            dtGroupData = dtGroup.Tables[0].Copy();

            DataRow drGP = dtGroupData.NewRow();
            drGP["ID"] = 0;
            drGP["Role"] = "None";
            dtGroupData.Rows.InsertAt(drGP, 0);
        }
    }

    private void ShowNoResultFound(DataTable source, GridView gv)
    {
        source.Rows.Add(source.NewRow()); // create a new blank row to the DataTable
        // Bind the DataTable which contain a blank row to the GridView
        gv.DataSource = source;
        gv.DataBind();
        // Get the total number of columns in the GridView to know what the Column Span should be
        int columnsCount = gv.Columns.Count;
        gv.Rows[0].Cells.Clear();// clear all the cells in the row
        gv.Rows[0].Cells.Add(new TableCell()); //add a new blank cell
        gv.Rows[0].Cells[0].ColumnSpan = columnsCount; //set the column span to the new added cell

        //You can set the styles here
        gv.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
        gv.Rows[0].Cells[0].ForeColor = System.Drawing.Color.Black;
        gv.Rows[0].Cells[0].Font.Bold = true;
        //set No Results found to the new added cell
        gv.Rows[0].Cells[0].Text = "No Records Found.";
    }

    private void Submit()
    {
        try
        {
            objPropUser.FirstName = txtCName.Text;
            objPropUser.Address = txtAddress.Text;
            objPropUser.City = txtCity.Text;
            objPropUser.State = ddlState.SelectedValue;
            objPropUser.Zip = txtZip.Text;
            objPropUser.MainContact = txtMaincontact.Text;
            objPropUser.Phone = txtPhoneCust.Text;
            objPropUser.Website = txtWebsite.Text;
            objPropUser.Email = txtEmail.Text;
            objPropUser.Cell = txtCell.Text;
            objPropUser.Type = ddlUserType.SelectedValue;
            objPropUser.Status = Convert.ToInt16(ddlCustStatus.SelectedValue);
            objPropUser.Remarks = txtRemarks.Text;
            objPropUser.Username = txtUserName.Text.Trim();
            objPropUser.Password = txtPassword.Text.Trim();
            objPropUser.grpbyWO = Convert.ToInt16(chkGrpWO.Checked);
            objPropUser.openticket = Convert.ToInt16(chkOpenTicket.Checked);

            if (chkScheduleBrd.Checked == true)
            {
                objPropUser.Schedule = 1;
            }
            else
            {
                objPropUser.Schedule = 0;
            }

            if (chkMap.Checked == true)
            {
                objPropUser.Mapping = 1;
            }
            else
            {
                objPropUser.Mapping = 0;
            }

            objPropUser.ConnConfig = Session["config"].ToString();
            objPropUser.CustomerID = Convert.ToInt32(Request.QueryString["uid"].ToString());
            objPropUser.EquipID = Convert.ToInt16(chkEquipments.Checked);
            DataTable dtGroup = new DataTable();
            dtGroup.Columns.Add("RoleId", typeof(int));
            dtGroup.Columns.Add("Loc", typeof(int));

            foreach (GridDataItem gr in RadGrid_Location.Items)
            {
                Label lblLoc = (Label)gr.FindControl("lblloc");
                DropDownList ddlGroup = (DropDownList)gr.FindControl("ddlGroup");
                DataRow dr = dtGroup.NewRow();
                dr["Loc"] = Convert.ToInt32(lblLoc.Text);
                dr["RoleId"] = Convert.ToInt32(ddlGroup.SelectedValue);
                dtGroup.Rows.Add(dr);
            }

            objPropUser.dtGroupdata = dtGroup;
            objBL_User.UpdateCustomerUser(objPropUser);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccUp", "noty({text: 'Customer updated successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false,dismissQueue: true});", true);

        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true,dismissQueue: true});", true);
        }
    }

    protected void gvGroup_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gvGroup.EditIndex = e.NewEditIndex;
        FillGroup(1);
    }
    protected void gvGroup_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            TextBox txtName = (TextBox)gvGroup.Rows[e.RowIndex].FindControl("txtName");
            TextBox txtUser = (TextBox)gvGroup.Rows[e.RowIndex].FindControl("txtUser");
            TextBox txtPassword = (TextBox)gvGroup.Rows[e.RowIndex].FindControl("txtPassword");
            Label lblID = (Label)gvGroup.Rows[e.RowIndex].FindControl("lblId");

            objPropCustomer.ConnConfig = Session["config"].ToString();
            objPropCustomer.LocationRole = txtName.Text.Trim();
            objPropCustomer.Username = txtUser.Text.Trim();
            objPropCustomer.Password = txtPassword.Text.Trim();
            objPropCustomer.CustomerID = Convert.ToInt32(Request.QueryString["uid"].ToString());
            objPropCustomer.RoleID = Convert.ToInt32(lblID.Text);
            objBL_Customer.UpdateLocationRole(objPropCustomer);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keysave", "noty({text: 'Group updated successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false,dismissQueue: true});", true);

            gvGroup.EditIndex = -1;
            FillGroup(1);

            foreach (GridDataItem gr in RadGrid_Location.Items)
            {
                DropDownList ddlGroup = (DropDownList)gr.FindControl("ddlGroup");
                var item = ddlGroup.Items.FindByValue(lblID.Text);
                item.Text = txtName.Text.Trim();

                uplLocGroups.Update();
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true,dismissQueue: true});", true);
        }
    }
    protected void gvGroup_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            Label lblID = (Label)gvGroup.Rows[e.RowIndex].FindControl("lblId");
            objPropCustomer.ConnConfig = Session["config"].ToString();
            objPropCustomer.RoleID = Convert.ToInt32(lblID.Text);
            objBL_Customer.DeleteLocationRole(objPropCustomer);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keysave", "noty({text: 'Group deleted successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false,dismissQueue: true});", true);
            gvGroup.EditIndex = -1;
            FillGroup(1);

            foreach (GridDataItem gr in RadGrid_Location.Items)
            {
                DropDownList ddlGroup = (DropDownList)gr.FindControl("ddlGroup");
                var item = ddlGroup.Items.FindByValue(lblID.Text);
                ddlGroup.Items.Remove(item);

                uplLocGroups.Update();
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true,dismissQueue: true});", true);
        }
    }
    protected void gvGroup_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvGroup.EditIndex = -1;
        FillGroup(1);
    }
    protected void lnkAdd_Click(object sender, EventArgs e)
    {
        try
        {
            TextBox txtNameF = (TextBox)gvGroup.FooterRow.FindControl("txtNameF");
            TextBox txtUserF = (TextBox)gvGroup.FooterRow.FindControl("txtUserF");
            TextBox txtPasswordF = (TextBox)gvGroup.FooterRow.FindControl("txtPasswordF");

            objPropCustomer.ConnConfig = Session["config"].ToString();
            objPropCustomer.LocationRole = txtNameF.Text.Trim();
            objPropCustomer.Username = txtUserF.Text.Trim();
            objPropCustomer.Password = txtPasswordF.Text.Trim();
            objPropCustomer.CustomerID = Convert.ToInt32(Request.QueryString["uid"].ToString());
            objPropCustomer.RoleID = objBL_Customer.AddLocationRole(objPropCustomer);

            foreach (GridDataItem gr in RadGrid_Location.Items)
            {
                DropDownList ddlGroup = (DropDownList)gr.FindControl("ddlGroup");

                ddlGroup.Items.Add(new ListItem(objPropCustomer.LocationRole, objPropCustomer.RoleID.ToString()));

                //HiddenField hdnLoc = (HiddenField)gr.FindControl("hdnLoc");
                //if (hdnLoc.Value == hdnSelectLoc.Value)
                //{
                //    ddlGroup.SelectedValue = objPropCustomer.RoleID.ToString();
                //}

                uplLocGroups.Update();
            }

            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keysave", "noty({text: 'Group added successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false,dismissQueue: true});", true);
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true,dismissQueue: true});", true);
        }
        finally
        {
            FillGroup(1);
        }
    }

    protected void lnkAddGrp_Click(object sender, EventArgs e)
    {
        RadAddGroupWindow.Visible = true;
        FillGroup(1);
    }

    protected void RadGrid_Location_PreRender(object sender, EventArgs e)
    {
        RowSelect();
    }

    private void Locations()
    {
        DataSet ds = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();
        objPropUser.CustomerID = Convert.ToInt32(Request.QueryString["uid"].ToString());
        objPropUser.DBName = Session["dbname"].ToString();
        ds = objBL_User.getLocationByCustomerID(objPropUser);

        RadGrid_Location.VirtualItemCount = ds.Tables[0].Rows.Count;
        RadGrid_Location.DataSource = ds.Tables[0];
        RadGrid_Location.DataBind();
    }

    private void RowSelect()
    {
        DataTable dtLocID = new DataTable();
        dtLocID.Columns.Add("loc");
        DataRow dr = null;
        foreach (GridDataItem gr in RadGrid_Location.Items)
        {
            Label lblUserID = (Label)gr.FindControl("lblloc");
            HyperLink lnkName = (HyperLink)gr.FindControl("lnkName");
            dr = dtLocID.NewRow();

            //add values to each rows
            dr["loc"] = lblUserID.Text;
            //add the row to DataTable

            dtLocID.Rows.Add(dr);

            lnkName.Attributes["onclick"] = gr.Attributes["ondblclick"] = "window.open('addlocation.aspx?uid=" + lblUserID.Text + "','_self');";
        }
    }

    protected void RadGrid_Location_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        FillGroup(0);

        DataSet ds = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();
        objPropUser.CustomerID = Convert.ToInt32(Request.QueryString["uid"].ToString());
        objPropUser.DBName = Session["dbname"].ToString();
        ds = objBL_User.getLocationByCustomerID(objPropUser);

        RadGrid_Location.VirtualItemCount = ds.Tables[0].Rows.Count;
        RadGrid_Location.DataSource = ds.Tables[0];
    }

    protected void lnkNext_Click(object sender, EventArgs e)
    {
        DataTable dt = (DataTable)Session["usersdata"];
        DataColumn[] keyColumns = new DataColumn[1];
        keyColumns[0] = dt.Columns["userkey"];
        dt.PrimaryKey = keyColumns;

        DataRow d = dt.Rows.Find(Request.QueryString["type"].ToString() + "_" + Request.QueryString["uid"].ToString());
        int index = dt.Rows.IndexOf(d);
        int c = dt.Rows.Count - 1;

        if (index < c)
        {
            if (Convert.ToInt16(dt.Rows[index + 1]["usertypeid"].ToString()) == 2)
                Response.Redirect("customeruser.aspx?uid=" + dt.Rows[index + 1]["userid"] + "&type=" + dt.Rows[index + 1]["usertypeid"]);
            else
                Response.Redirect("adduser.aspx?uid=" + dt.Rows[index + 1]["userid"] + "&type=" + dt.Rows[index + 1]["usertypeid"]);
        }
    }

    protected void lnkPrevious_Click(object sender, EventArgs e)
    {
        DataTable dt = (DataTable)Session["usersdata"];
        DataColumn[] keyColumns = new DataColumn[1];
        keyColumns[0] = dt.Columns["userkey"];
        dt.PrimaryKey = keyColumns;

        DataRow d = dt.Rows.Find(Request.QueryString["type"].ToString() + "_" + Request.QueryString["uid"].ToString());
        int index = dt.Rows.IndexOf(d);

        if (index > 0)
        {
            if (Convert.ToInt16(dt.Rows[index - 1]["usertypeid"].ToString()) == 2)
                Response.Redirect("customeruser.aspx?uid=" + dt.Rows[index - 1]["userid"] + "&type=" + dt.Rows[index - 1]["usertypeid"]);
            else
                Response.Redirect("adduser.aspx?uid=" + dt.Rows[index - 1]["userid"] + "&type=" + dt.Rows[index - 1]["usertypeid"]);
        }
    }

    protected void lnkLast_Click(object sender, EventArgs e)
    {
        DataTable dt = (DataTable)Session["usersdata"];
        if (Convert.ToInt16(dt.Rows[dt.Rows.Count - 1]["usertypeid"].ToString()) == 2)
            Response.Redirect("customeruser.aspx?uid=" + dt.Rows[dt.Rows.Count - 1]["userid"] + "&type=" + dt.Rows[dt.Rows.Count - 1]["usertypeid"]);
        else
            Response.Redirect("adduser.aspx?uid=" + dt.Rows[dt.Rows.Count - 1]["userid"] + "&type=" + dt.Rows[dt.Rows.Count - 1]["usertypeid"]);
    }

    protected void lnkFirst_Click(object sender, EventArgs e)
    {
        DataTable dt = (DataTable)Session["usersdata"];
        if (Convert.ToInt16(dt.Rows[0]["usertypeid"].ToString()) == 2)
            Response.Redirect("customeruser.aspx?uid=" + dt.Rows[0]["userid"] + "&type=" + dt.Rows[0]["usertypeid"]);
        else
            Response.Redirect("adduser.aspx?uid=" + dt.Rows[0]["userid"] + "&type=" + dt.Rows[0]["usertypeid"]);
    }
}
