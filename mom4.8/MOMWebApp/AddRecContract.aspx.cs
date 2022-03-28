using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BusinessEntity;
using BusinessLayer;
using System.Globalization;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;
using System.Web;

public partial class AddRecContract : System.Web.UI.Page
{


    #region Property

    User objPropUser = new User();

    BL_User objBL_User = new BL_User();

    BL_Contracts objBL_Contracts = new BL_Contracts();

    Contracts objProp_Contracts = new Contracts();

    Loc _objLoc = new Loc();

    Customer objProp_Customer = new Customer();

    BL_Customer objBL_Customer = new BL_Customer();

    JobT _objJob = new JobT();

    BL_Job objBL_Job = new BL_Job();

    string defaultDate = "12/30/1899";

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userid"] == null)
        {
            Response.Redirect("login.aspx");
        }

        if (Request.QueryString["rt"] != null)
        {
            Session["rt"] = "2";
        }
        else
        {
            Session["rt"] = null;
        }

        WebBaseUtility.UpdatePageTitle(this, "Contract", Request.QueryString["uid"], Request.QueryString["t"]);

        //hdnCon.Value = Session["config"].ToString();
        txtUnit.Text = hdnUnit.Value;
        if (!IsPostBack)
        {
            FillCategory();
            ViewState["DepartmentID"] = 0;
            SetDefaultWorker();
            getDiagnosticCategory();
            GetControlData();
            rfvGLAcct.Enabled = false;
            FillRoute();
            FillLocationType();
            Fillterritory();
            //FillServiceTypeEdit("");
            FillContractBill();
            FillBillDetailLevel();
            BindCustomField();
            ViewState["mode"] = 0;
            ViewState["editcon"] = 0;

            if (Request.QueryString["uid"] != null)
            {
                //Page.Title = "Edit Contract || MOM";
                pnlNext.Visible = true;
                liLogs.Style["display"] = "inline-block";
                tbLogs.Style["display"] = "block";
                if (Request.QueryString["t"] != null)
                {
                    ViewState["mode"] = 0;
                }
                else
                {
                    ViewState["mode"] = 1;
                    lblHeader.Text = "Edit Contract";
                }
                objProp_Contracts.ConnConfig = Session["config"].ToString();

                objProp_Contracts.JobId = Convert.ToInt32(Request.QueryString["uid"]);

                DataSet ds = new DataSet();

                ds = objBL_Contracts.GetContract(objProp_Contracts);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    ViewState["DepartmentID"] = Convert.ToInt32(ds.Tables[0].Rows[0]["DepartmentID"].ToString());
                    lblContrName.Text = ds.Tables[0].Rows[0]["id"].ToString();
                    txtLocation.Text = ds.Tables[0].Rows[0]["locname"].ToString();
                    hdnLocId.Value = ds.Tables[0].Rows[0]["loc"].ToString();
                    FillLocInfo();
                    ddlRoute.SelectedValue = ds.Tables[0].Rows[0]["Route"].ToString();
                    ddlTerr.SelectedValue = ds.Tables[0].Rows[0]["Terr"].ToString();
                    ddlTerr2.SelectedValue = ds.Tables[0].Rows[0]["Terr2"].ToString();
                    ddlStatus.SelectedValue = ds.Tables[0].Rows[0]["status"].ToString();
                    hdnddlJobStatus.Value = ddlStatus.SelectedValue;
                    if(ds.Tables[0].Rows[0]["BStart"]  !=DBNull.Value)
                    txtBillStartDt.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["BStart"]).ToShortDateString();
                    ddlBillFreq.SelectedValue = ds.Tables[0].Rows[0]["BCycle"].ToString();
                    txtBillAmt.Text = String.Format("{0:C}", Convert.ToDouble(ds.Tables[0].Rows[0]["bamt"]));
                    hdnBillAmt.Value = txtBillAmt.Text;
                    if (ds.Tables[0].Rows[0]["sStart"] != DBNull.Value)
                    txtScheduleStartDt.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["sStart"]).ToShortDateString();
                    ddlSchFreq.SelectedValue = ds.Tables[0].Rows[0]["sCycle"].ToString();
                    // When scheduling frequency is set to never we hide all the scheduling fields, like date, time, hours,
                    ddlSchFreq_SelectedIndexChanged(sender, e);
                    chkWeekends.Checked = Convert.ToBoolean(Convert.ToInt32(ds.Tables[0].Rows[0]["swe"]));
                    txtDay.Text = ds.Tables[0].Rows[0]["sday"].ToString();
                    ddlDay.SelectedValue = ds.Tables[0].Rows[0]["sdate"].ToString();
                    txtsTime.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["stime"]).ToShortTimeString().Replace("12:00 AM", "");
                    chkCredit.Checked = Convert.ToBoolean(Convert.ToInt32(ds.Tables[0].Rows[0]["creditcard"]));
                    txtRemarks.Text = ds.Tables[0].Rows[0]["remarks"].ToString();
                    txtBillHours.Text = ds.Tables[0].Rows[0]["hours"].ToString();
                    txtDescription.Text = ds.Tables[0].Rows[0]["fdesc"].ToString();

                    /////------------------------------>
                    txtOriginalContract.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["OriginalContract"]).ToShortDateString();
                    txtLastrenew.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["LastRenew"]).ToShortDateString();

                    string TicketCategory = ds.Tables[0].Rows[0]["TicketCategory"].ToString();

                    ListItem item = ddlTicketCat.Items.FindByValue(TicketCategory);
                    if (item != null)
                    {
                        ddlTicketCat.SelectedValue = TicketCategory;
                    }

                    txtContractLength.Text = ds.Tables[0].Rows[0]["ContractLength"].ToString();
                    hdnContractLength.Value = ds.Tables[0].Rows[0]["ContractLength"].ToString();

                    /////------------------------------->

                    if (ddlRoute.SelectedValue != "")
                        FillServiceTypeEdit(ddlType.SelectedValue.ToString(), ds.Tables[0].Rows[0]["ctype"].ToString(), Convert.ToInt32(ViewState["DepartmentID"].ToString()), Convert.ToInt32(ddlRoute.SelectedValue.ToString()));
                    else
                    {
                        ClientScript.RegisterStartupScript(Page.GetType(), "keyerrr", "noty({text: 'Please select an active " + lblDefaultWorker.InnerText + " for this location',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                    }

                    if (ds.Tables[0].Rows[0]["ctype"].ToString() != "")
                    {

                        ddlServiceType.SelectedValue = ds.Tables[0].Rows[0]["ctype"].ToString();
                    }

                    txtUnitExpiration.Text = ds.Tables[0].Rows[0]["ExpirationDate"].ToString();
                    txtNumFreq.Text = ds.Tables[0].Rows[0]["frequencies"].ToString();
                    ddlExpiration.SelectedValue = ds.Tables[0].Rows[0]["Expiration"].ToString();



                    txtPO.Text = ds.Tables[0].Rows[0]["PO"].ToString();
                    ddlBillDetailLevel.SelectedValue = ds.Tables[0].Rows[0]["detail"].ToString();

                    if (Convert.ToString(ds.Tables[0].Rows[0]["credit"]) == "1")
                        imgCreditHold.Visible = true;
                    else
                        imgCreditHold.Visible = false;
                    lblHeaderLabel.Text = "Contract# " + Convert.ToString(Request.QueryString["uid"]);
                    lblHeaderLabeldf.NavigateUrl = "Addproject?uid=" + Convert.ToString(Request.QueryString["uid"]);
                    lblHeaderLabeldf.Text = Convert.ToString(Request.QueryString["uid"]);
                    lblHyperlinklabel.Text = "Project#";


                    if (Convert.ToString(ddlServiceType.SelectedValue) != "")
                    {
                        lblHeaderLabel.Text = lblHeaderLabel.Text + " | " + Convert.ToString(ddlServiceType.SelectedItem.Text);
                    }
                    if (Convert.ToString(ds.Tables[0].Rows[0]["locname"]) != "")
                    {
                        lblHeaderLabel.Text = lblHeaderLabel.Text + " | " + Convert.ToString(ds.Tables[0].Rows[0]["locname"]);
                    }

                    //TaskCategory
                    string TaskCategory = ds.Tables[0].Rows[0]["TaskCategory"].ToString();
                    if (ddlCodeCat.Items.FindByValue(TaskCategory) != null)
                    {
                        ddlCodeCat.SelectedValue = TaskCategory;
                    }
                    else
                    {
                        ddlCodeCat.SelectedValue = "";
                    }

                    #region Notes
                    txtSpecialInstructions.Text = ds.Tables[0].Rows[0]["SRemarks"] != DBNull.Value ? Convert.ToString(ds.Tables[0].Rows[0]["SRemarks"]) : string.Empty;
                    chkspnotes.Checked = ds.Tables[0].Rows[0]["SPHandle"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[0]["SPHandle"]) : false;
                    #endregion

                    #region Renewal Notes

                    txtRenewalNotes.Text = ds.Tables[0].Rows[0]["RenewalNotes"] != DBNull.Value ? Convert.ToString(ds.Tables[0].Rows[0]["RenewalNotes"]) : string.Empty;
                    chkRenewalNotes.Checked = ds.Tables[0].Rows[0]["IsRenewalNotes"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[0]["IsRenewalNotes"]) : false;

                    #endregion
                    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Billing"].ToString()))     // added by Mayuri 25th dec, 15
                    {                                                                          // Location billing details
                        ddlContractBill.SelectedValue = ds.Tables[0].Rows[0]["Billing"].ToString();
                    }

                    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["CustBilling"].ToString())) // Customer billing details
                    {
                        ddlBilling.SelectedValue = ds.Tables[0].Rows[0]["CustBilling"].ToString();
                    }

                    FillSpecifyLocation();
                    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Central"].ToString()))
                    {
                        ddlSpecifiedLocation.SelectedValue = ds.Tables[0].Rows[0]["Central"].ToString();
                    }
                    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Chart"].ToString()))
                    {
                        hdnGLAcct.Value = ds.Tables[0].Rows[0]["Chart"].ToString();
                    }
                    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["GLAcct"].ToString()))
                    {
                        txtGLAcct.Text = ds.Tables[0].Rows[0]["GLAcct"].ToString();
                    }

                    ddlEscType.SelectedValue = ds.Tables[0].Rows[0]["BEscType"].ToString();
                    txtEscCycle.Text = ds.Tables[0].Rows[0]["BEscCycle"].ToString();
                    txtEscFactor.Text = ds.Tables[0].Rows[0]["BEscFact"].ToString();
                    txtEscdue.Text = (ds.Tables[0].Rows[0]["EscLast"].ToString() != string.Empty) ? Convert.ToDateTime(ds.Tables[0].Rows[0]["EscLast"].ToString()).ToShortDateString() : "";

                    DataSet dsElev = new DataSet();
                    dsElev = objBL_Contracts.GetElevContract(objProp_Contracts);
                    if (dsElev.Tables[0].Rows.Count > 0)
                    {
                        foreach (GridViewRow gr in gvEquip.Rows)
                        {
                            Label lblID = (Label)gr.FindControl("lblID");
                            CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");
                            TextBox txtPrice = (TextBox)gr.FindControl("txtPrice");
                            TextBox txtHours = (TextBox)gr.FindControl("txtHours");
                            Label lblname = (Label)gr.FindControl("lblUnit");

                            foreach (DataRow dr in dsElev.Tables[0].Rows)
                            {
                                if (lblID.Text == dr["elev"].ToString())
                                {
                                    if (txtUnit.Text != string.Empty)
                                    {
                                        txtUnit.Text = txtUnit.Text + ", " + lblname.Text;
                                    }
                                    else
                                    {
                                        txtUnit.Text = lblname.Text;
                                    }
                                    chkSelect.Checked = true;
                                    txtPrice.Text = String.Format("{0:C}", Convert.ToDouble((dr["price"] != DBNull.Value) ? dr["price"] : 0));
                                    txtHours.Text = dr["hours"].ToString();
                                }
                            }
                        }
                    }
                    txtBillRate.Text = string.Format("{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["BillRate"].ToString()));
                    txtOt.Text = string.Format("{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["RateOT"].ToString()));
                    txtNt.Text = string.Format("{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["RateNT"].ToString()));
                    txtDt.Text = string.Format("{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["RateDT"].ToString()));
                    txtMileage.Text = string.Format("{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["RateMileage"].ToString()));
                    txtTravel.Text = string.Format("{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["RateTravel"].ToString()));


                }
            }
            else
            {
                //Page.Title = "Add Contract || MOM";
                if (Request.QueryString["eid"] != null)
                {
                    btnSubmit.Text = "Next";
                    DataSet ds = new DataSet();
                    objProp_Customer.ConnConfig = Session["config"].ToString();
                    objProp_Customer.estimateno = Convert.ToInt32(Request.QueryString["eid"].ToString());
                    //ds = objBL_Customer.getEstimateTemplateByID(objProp_Customer);
                    ds = objBL_Customer.GetEstimateByID(objProp_Customer);
                    txtLocation.Text = ds.Tables[0].Rows[0]["locationname"].ToString();
                    hdnLocId.Value = ds.Tables[0].Rows[0]["locid"].ToString();
                    var tempBillAmt = 0.0;
                    if (ds.Tables.Count > 3 && ds.Tables[3].Rows.Count > 0)
                    {
                        foreach (DataRow item in ds.Tables[3].Rows)
                        {
                            tempBillAmt += Convert.ToDouble(item["Amount"]);
                        }
                    }
                    hdnBillAmt.Value = tempBillAmt.ToString();
                    txtBillAmt.Text = tempBillAmt.ToString();
                    FillLocInfo();
                    txtCustomer.Enabled = false;
                    txtLocation.Enabled = false;
                    txtAddress.Enabled = false;
                }

                FillSpecifyLocation();
                ddlSchFreq_SelectedIndexChanged(sender, e);
                ddlCodeCat.Enabled = true;
            }
        }
        FillEquiptype();
        FillEquipCategory();
        FillBuilding();
        CompanyPermission();
        Permission();
        HighlightSideMenu("cntractsMgr", "lnkContractsMenu", "recurMgrSub");
    }

    #region :: Method
    private void FillLocationType()
    {
        DataSet ds = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();
        ds = objBL_User.getlocationType(objPropUser);
        ddlType.DataSource = ds.Tables[0];
        ddlType.DataTextField = "Type";
        ddlType.DataValueField = "Type";
        ddlType.DataBind();

        ddlType.Items.Insert(0, new ListItem(":: Select ::", ""));
    }

    private void Fillterritory()
    {
        DataSet ds = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();

        //ds = objBL_User.getTerritory(objPropUser, new GeneralFunctions().GetSalesAsigned());
        int refID = Request.QueryString["uid"] != null ? Convert.ToInt32(Request.QueryString["uid"].ToString()) : 0;
        ds = objBL_User.GetSalesPerson(objPropUser, new GeneralFunctions().GetSalesAsigned(), refID, "JOB", "t.Name");

        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            ddlTerr.DataSource = ds.Tables[0];
            ddlTerr.DataTextField = "Name";
            ddlTerr.DataValueField = "ID";
            ddlTerr.DataBind();
            ddlTerr.Items.Insert(0, new ListItem(":: Select ::", ""));

            // Second Salesperson  
            ddlTerr2.DataSource = ds.Tables[0];
            ddlTerr2.DataTextField = "Name";
            ddlTerr2.DataValueField = "ID";
            ddlTerr2.DataBind();
            ddlTerr2.Items.Insert(0, new ListItem(":: Select ::", ""));
        }
    }

    private void HighlightSideMenu(string MenuParent, string PageLink, string SubMenuDiv)
    {
        HyperLink aNav = (HyperLink)Page.Master.FindControl(MenuParent);
        aNav.CssClass = "active collapsible-header waves-effect waves-cyan collapsible-height-nl";

        HyperLink lnkUsersSmenu = (HyperLink)Page.Master.FindControl(PageLink);
        lnkUsersSmenu.Style.Add("color", "#316b9d");
        lnkUsersSmenu.Style.Add("font-weight", "normal");
        lnkUsersSmenu.Style.Add("background-color", "#e5e5e5");
        HtmlGenericControl div = (HtmlGenericControl)Page.Master.FindControl(SubMenuDiv);
        div.Style.Add("display", "block");
    }

    private void BindCustomField()
    {
        try
        {
            DataSet ds = new DataSet();

            _objJob.ConnConfig = Session["config"].ToString();
            if (Request.QueryString["uid"] != null)
            {
                _objJob.Job = Convert.ToInt32(Request.QueryString["uid"].ToString());
            }
            ds = objBL_Job.GetRecurringCustom(_objJob);

            gvCustom.DataSource = ds;
            gvCustom.DataBind();

            int jobTId = 0;
            if (ds.Tables[2].Rows.Count > 0)
            {
                jobTId = Convert.ToInt32(ds.Tables[2].Rows[0]["JobT"].ToString());
            }

            ViewState["JobT"] = jobTId;

            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ((Label)gvCustom.FooterRow.FindControl("lblRowCount")).Text = "Total Line Items: " + Convert.ToString(ds.Tables[0].Rows.Count - 0);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        BindCustomItemGrid(ds);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void CompanyPermission()
    {
        if (Session["COPer"].ToString() == "1")
        {
            dvCompanyPermission.Visible = true;
        }
        else
        {
            dvCompanyPermission.Visible = false;
        }
    }

    private void BindCustomItemGrid(DataSet dsCustom)
    {
        DataTable dtCust = dsCustom.Tables[0];
        DataTable dtValues = dsCustom.Tables[1];
        foreach (GridViewRow gr in gvCustom.Rows)
        {
            Label lblFormat = (Label)gr.FindControl("lblFormat");
            if (lblFormat.Text == "Dropdown")
            {
                DropDownList ddlCustomValue = (DropDownList)gr.FindControl("ddlFormat");
                ddlCustomValue.Visible = true;
                Label lblID = (Label)gr.FindControl("lblID");
                Label lblIndex = (Label)gr.FindControl("lblIndex");
                Label lblValue = (Label)gr.FindControl("lblValue");

                DataTable dt = dtValues.Clone();
                //ItemID = " + Convert.ToInt32(lblID.Text) + " AND
                if (dtValues.Rows.Count > 0)
                {
                    DataRow[] result = dtValues.Select("Line = " + Convert.ToInt32(lblIndex.Text) + "");
                    foreach (DataRow row in result)
                    {
                        dt.ImportRow(row);
                    }

                    if (dt.Rows.Count > 0)
                    {
                        dt.DefaultView.Sort = "Value  ASC";
                        dt = dt.DefaultView.ToTable();
                    }
                    ddlCustomValue.DataSource = dt;
                    ddlCustomValue.DataTextField = "Value";
                    ddlCustomValue.DataValueField = "Value";
                    ddlCustomValue.DataBind();
                }

                ddlCustomValue.Items.Insert(0, (new ListItem("", "")));

                if (ddlCustomValue.Items.Contains(new ListItem(lblValue.Text, lblValue.Text)))
                {
                    ddlCustomValue.SelectedValue = lblValue.Text;
                }
                else
                {
                    ddlCustomValue.Items.Add(new ListItem(lblValue.Text, lblValue.Text));
                    ddlCustomValue.SelectedValue = lblValue.Text;
                }
            }
            else if (lblFormat.Text == "Checkbox")
            {
                CheckBox chkValue = (CheckBox)gr.FindControl("chkValue");
                chkValue.Visible = true;
            }
            else
            {
                TextBox txtValue = (TextBox)gr.FindControl("txtValue");
                txtValue.Visible = true;
            }
        }
    }

    private DataTable GetCustomTemplate()
    {
        DataTable dt = new DataTable();
        try
        {
            dt.Columns.Add("ID", typeof(int));
            dt.Columns.Add("tblTabID", typeof(int));
            dt.Columns.Add("Label", typeof(string));
            dt.Columns.Add("Line", typeof(Int16));
            dt.Columns.Add("Value", typeof(string));
            dt.Columns.Add("Format", typeof(Int16));
            dt.Columns.Add("IsTask", typeof(bool));
            foreach (GridViewRow gr in gvCustom.Rows)
            {
                DataRow dr = dt.NewRow();
                dr["ID"] = Convert.ToInt32(((Label)gr.FindControl("lblID")).Text);
                dr["tblTabID"] = 0;
                dr["Label"] = ((Label)gr.FindControl("lblDesc")).Text;
                dr["Line"] = dt.Rows.Count + 1;
                if (((Label)gr.FindControl("lblFormat")).Text == "Dropdown")
                    dr["value"] = ((DropDownList)gr.FindControl("ddlFormat")).Text.Trim();
                else if (((Label)gr.FindControl("lblFormat")).Text == "Checkbox")
                {
                    CheckBox chk = ((CheckBox)gr.FindControl("chkValue"));
                    if (chk.Checked.Equals(true))
                    {
                        dr["value"] = '1';
                    }
                    else
                    {
                        dr["value"] = '0';
                    }
                }
                else
                    dr["value"] = ((TextBox)gr.FindControl("txtValue")).Text.Trim();
                dr["Format"] = ((Label)gr.FindControl("lblFormatID")).Text;
                dt.Rows.Add(dr);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return dt;
    }

    protected void Page_PreRender(Object o, EventArgs e)
    {
        string txtPriceValues = string.Empty;
        foreach (GridViewRow gr in gvEquip.Rows)
        {
            Label lblID = (Label)gr.FindControl("lblID");
            Label lblname = (Label)gr.FindControl("lblUnit");
            CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");
            TextBox txtPrice = (TextBox)gr.FindControl("txtPrice");
            txtPriceValues = txtPriceValues + txtPrice.Text;
            TextBox txtHours = (TextBox)gr.FindControl("txtHours");
            Label lblStatus = (Label)gr.FindControl("lblStatus");


            //chkSelect.Attributes["onclick"] = "SelectRowsEq('" + gvEquip.ClientID + "','" + txtUnit.ClientID + "','" + hdnUnit.ClientID + "'); CalculateAmount(); CalculateHours();";
            chkSelect.Attributes["onclick"] = "ChkSelectEqup('" + chkSelect.ClientID + "','" + lblStatus.ClientID + "');";
            txtPrice.Attributes["onblur"] = "$('#" + txtPrice.ClientID + "').formatCurrency(); CalculateAmount();";
            txtHours.Attributes["onblur"] = "CalculateHours();";
        }
    }

    private void Permission()
    {


        if (Session["type"].ToString() == "c")
        {
            Response.Redirect("home.aspx");
        }

        if (Session["MSM"].ToString() == "TS")
        {
            Response.Redirect("home.aspx");

        }
        if (Convert.ToInt32(Session["ISsupervisor"]) == 1)
        {
            Response.Redirect("home.aspx");
        }

        // User Permission 
        if (Session["type"].ToString() != "am" && Session["type"].ToString() != "c")
        {
            DataTable ds = new DataTable();
            ds = GetUserById();

            /// RCmodulePermission ///////////////////------->

            string RCmodulePermission = ds.Rows[0]["RCmodulePermission"] == DBNull.Value ? "Y" : ds.Rows[0]["RCmodulePermission"].ToString();

            if (RCmodulePermission == "N")
            {
                Response.Redirect("Home.aspx?permission=no"); return;
            }

            /////////////JOB STATUS PERMISSION

            //JobClosePermission
            string JobClosePermission = ds.Rows[0]["JobClosePermission"] == DBNull.Value ? "YYYYYY" : ds.Rows[0]["JobClosePermission"].ToString();

            if (!JobClosePermission.Contains("Y"))
            {
                hdnClosePermission.Value = "N";
            }

            //CompletedJObPermission
            string CompletedJObPermission = ds.Rows[0]["JobCompletedPermission"] == DBNull.Value ? "Y" : ds.Rows[0]["JobCompletedPermission"].ToString();

            if (!CompletedJObPermission.Contains("Y"))
            {
                hdnCompletedJObPermission.Value = "N";
            }

            //JobReopenPermission
            string JobReopenPermission = ds.Rows[0]["JobReopenPermission"] == DBNull.Value ? "Y" : ds.Rows[0]["JobReopenPermission"].ToString();

            if (!JobReopenPermission.Contains("Y"))
            {
                hdnJobReopenPermission.Value = "N";
            }





            /// RC ///////////////////------->

            string ProcessRCPermission = ds.Rows[0]["ProcessRCPermission"] == DBNull.Value ? "YYYYYY" : ds.Rows[0]["ProcessRCPermission"].ToString();
            string ADD = ProcessRCPermission.Length < 1 ? "Y" : ProcessRCPermission.Substring(0, 1);
            string Edit = ProcessRCPermission.Length < 2 ? "Y" : ProcessRCPermission.Substring(1, 1);
            string Delete = ProcessRCPermission.Length < 3 ? "Y" : ProcessRCPermission.Substring(2, 1);
            string View = ProcessRCPermission.Length < 4 ? "Y" : ProcessRCPermission.Substring(3, 1);



            if (Request.QueryString["uid"] != null)
            {
                //aImport.Visible = false;
            }
            if (View == "N")
            {
                Response.Redirect("Home.aspx?permission=no"); return;
            }
            else if (Request.QueryString["uid"] == null)
            {
                if (ADD == "N")
                {
                    Response.Redirect("Home.aspx?permission=no"); return;
                }
            }
            else if (Edit == "N")
            {
                if (View == "Y")
                {
                    btnSubmit.Visible = false;
                    //btnSubmitJob.Visible = false;
                }
                else
                {
                    Response.Redirect("Home.aspx?permission=no"); return;
                }
            }


        }
    }
    private DataTable GetUserById()
    {
        User objPropUser = new User();
        objPropUser.TypeID = Convert.ToInt32(Session["usertypeid"]);
        objPropUser.UserID = Convert.ToInt32(Session["userid"]);
        objPropUser.ConnConfig = Session["config"].ToString();
        objPropUser.DBName = Session["dbname"].ToString();
        DataSet ds = new DataSet();
        ds = objBL_User.GetUserPermissionByUserID(objPropUser);
        return ds.Tables[0];
    }
   
    private void FillServiceTypeEdit(string LocType, string EditSType, int department = -1, int route = -1)
    {
        DataSet ds = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();
         
        ds = new BusinessLayer.Programs.BL_ServiceType().GetActiveServiceTypeContract(objPropUser.ConnConfig, LocType, EditSType, department, route);
       
        ddlServiceType.Items.Clear();
        ddlServiceType.DataSource = ds.Tables[0];
        ddlServiceType.DataTextField = "type";
        ddlServiceType.DataValueField = "type";
        ddlServiceType.DataBind();

        ddlServiceType.Items.Insert(0, new ListItem(":: Select ::", ""));
    }
    private void FillEquiptype()
    {
        Session["EquipTypeLabel"] = null;
        DataSet ds = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();
        ds = objBL_User.getEquiptype(objPropUser);
        if (ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Rows[0]["Label"].ToString() != "")
        {
            Session["EquipTypeLabel"] = ds.Tables[0].Rows[0]["Label"].ToString();
            gvEquip.Columns[4].HeaderText = ds.Tables[0].Rows[0]["Label"].ToString();
        }
    }

    private void FillEquipCategory()
    {
        Session["EquipCatLabel"] = null;
        DataSet ds = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();
        ds = objBL_User.getEquipmentCategory(objPropUser);
        if (ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Rows[0]["Label"].ToString() != "")
        {
            Session["EquipCatLabel"] = ds.Tables[0].Rows[0]["Label"].ToString();
            gvEquip.Columns[5].HeaderText = ds.Tables[0].Rows[0]["Label"].ToString();
        }
    }

    private void FillBuilding()
    {
        Session["EquipLabel"] = null;
        DataSet ds = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();
        ds = objBL_User.getBuildingElev(objPropUser);
        if (ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Rows[0]["Label"].ToString() != "")
        {
            Session["EquipLabel"] = ds.Tables[0].Rows[0]["Label"].ToString();
            gvEquip.Columns[7].HeaderText = ds.Tables[0].Rows[0]["Label"].ToString();
        }
    }

    public void GetJstatus()
    {
        DataSet ds = new DataSet();
        objProp_Contracts.ConnConfig = Session["config"].ToString();
        ds = objBL_Contracts.getJstatus(objProp_Contracts);
        ddlStatus.DataSource = ds.Tables[0];
        ddlStatus.DataTextField = "status";
        ddlStatus.DataValueField = "status";
        ddlStatus.DataBind();
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
        objPropUser.building = "All";
        ds = objBL_User.getElev(objPropUser);
        gvEquip.DataSource = ds.Tables[0];
        gvEquip.DataBind();
        foreach (GridViewRow gr in gvEquip.Rows)
        {
            Label lblID = (Label)gr.FindControl("lblID");
            CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");
            TextBox txtPrice = (TextBox)gr.FindControl("txtPrice");
            TextBox txtHours = (TextBox)gr.FindControl("txtHours");
            Label lblname = (Label)gr.FindControl("lblUnit");

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (lblID.Text == dr["id"].ToString())
                {

                    txtPrice.Text = String.Format("{0:C}", Convert.ToDouble((dr["price"] != DBNull.Value) ? dr["price"] : 0));
                }
            }
        }
    }

    protected void gvEquip_DataBound(object sender, EventArgs e)
    {
        TemplateField bType = (TemplateField)gvEquip.Columns[4];
        bType.HeaderText = "Type";
        if (Session["EquipTypeLabel"] != null)
        {
            bType.HeaderText = Convert.ToString(Session["EquipTypeLabel"]);
        }
        TemplateField bCat = (TemplateField)gvEquip.Columns[5];
        bCat.HeaderText = "Category";
        if (Session["EquipCatLabel"] != null)
        {
            bCat.HeaderText = Convert.ToString(Session["EquipCatLabel"]);
        }
        TemplateField bf = (TemplateField)gvEquip.Columns[7];
        bf.HeaderText = "Building";
        if (Session["EquipLabel"] != null)
        {
            bf.HeaderText = Convert.ToString(Session["EquipLabel"]);
        }
    }

    private void FillRoute()
    {
        DataSet ds = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();
        Int32 ContractID = 0;
        ContractID = Request.QueryString["uid"] == null ? 0 : Convert.ToInt32(Request.QueryString["uid"].ToString());
        ds = objBL_User.getRoute(objPropUser, 1, 0, ContractID);//IsActive=1 :- Get Only Active Workers
        ddlRoute.DataSource = ds.Tables[0];
        ddlRoute.DataTextField = "label";
        ddlRoute.DataValueField = "ID";
        ddlRoute.DataBind();

        ddlRoute.Items.Insert(0, new ListItem(":: Select ::", ""));
        ddlRoute.Items.Insert(1, new ListItem("Unassigned", "0"));
    }

    protected void lnkFirst_Click(object sender, EventArgs e)
    {
        //DataTable dt = (DataTable)Session["ContrSrch"];
        // Response.Redirect("addreccontract.aspx?uid=" + dt.Rows[0]["job"]);
        DataTable dt = new DataTable();
        dt = (DataTable)Session["ContractJobID"];
        string url = "addreccontract.aspx?uid=" + dt.Rows[0]["Job"];
        Response.Redirect(url);
    }

    protected void lnkPrevious_Click(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();
        dt = (DataTable)Session["ContractJobID"];
        DataColumn[] keyColumns = new DataColumn[1];
        keyColumns[0] = dt.Columns["Job"];
        dt.PrimaryKey = keyColumns;

        DataRow d = dt.Rows.Find(Request.QueryString["uid"].ToString());
        int index = dt.Rows.IndexOf(d);
        if (index > 0)
        {
            string url = "addreccontract.aspx?uid=" + dt.Rows[index - 1]["Job"];
            Response.Redirect(url);
        }
    }

    protected void lnkNext_Click(object sender, EventArgs e)
    {
  
        DataTable dt = new DataTable();
        dt = (DataTable)Session["ContractJobID"];
        DataColumn[] keyColumns = new DataColumn[1];
        keyColumns[0] = dt.Columns["Job"];
        dt.PrimaryKey = keyColumns;
        DataRow d = dt.Rows.Find(Request.QueryString["uid"].ToString());
        int index = dt.Rows.IndexOf(d);
        int c = dt.Rows.Count - 1;
        if (index < c)
        {
            string url = "addreccontract.aspx?uid=" + dt.Rows[index + 1]["Job"];
            Response.Redirect(url);
        }
    }

    protected void lnkLast_Click(object sender, EventArgs e)
    {
        // DataTable dt = (DataTable)Session["ContrSrch"];
        // Response.Redirect("addreccontract.aspx?uid=" + dt.Rows[dt.Rows.Count - 1]["job"]);
        DataTable dt = new DataTable();
        dt = (DataTable)Session["ContractJobID"];
        string url = "addreccontract.aspx?uid=" + dt.Rows[dt.Rows.Count - 1]["Job"];
        Response.Redirect(url);
    }

    private DataTable GetElevData()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("ElevUnit", typeof(int));
        dt.Columns.Add("Price", typeof(double));
        dt.Columns.Add("Hours", typeof(double));

        foreach (GridViewRow gvr in gvEquip.Rows)
        {
            CheckBox chkSelect = (CheckBox)gvr.FindControl("chkSelect");

            if (chkSelect.Checked == true)
            {
                DataRow dr = dt.NewRow();
                Label lblUnit = (Label)gvr.FindControl("lblID");
                TextBox txtPrice = (TextBox)gvr.FindControl("txtPrice");
                TextBox txtHours = (TextBox)gvr.FindControl("txtHours");
                dr["ElevUnit"] = Convert.ToInt32(lblUnit.Text);
                if (txtPrice.Text.Trim() != string.Empty)
                {
                    dr["Price"] = double.Parse(txtPrice.Text, NumberStyles.Currency);
                }
                if (txtHours.Text.Trim() != string.Empty)
                {
                    dr["Hours"] = Convert.ToDouble(txtHours.Text);
                }
                dt.Rows.Add(dr);
            }
        }
        return dt;
    }

    private bool ValidateRecContract()
    {
        bool _isValid = true;

        int _status = 0;

        if (ddlContractBill.SelectedValue == "1")           // validate location level contract billing
        {
            if (string.IsNullOrEmpty(hdnLocId.Value))
                hdnLocId.Value = "0";

            objProp_Contracts.ConnConfig = Session["config"].ToString();
            objProp_Contracts.Loc = Convert.ToInt32(hdnLocId.Value);
            objProp_Contracts.IsExistContract = objBL_Contracts.IsExistContractByLoc(objProp_Contracts);
            if (!objProp_Contracts.IsExistContract)
            {
                _status = 1;
                _isValid = false;
            }
            else
                _isValid = true;
        }

        if (_isValid.Equals(true))
        {
            if (ddlBilling.SelectedValue == "1")           // validate customer level billing
            {
                int _count = ddlSpecifiedLocation.Items.Count - 1;
                if (_count <= 0)
                {
                    _status = 2;
                    _isValid = false;
                }
                else
                {
                    if (ddlSpecifiedLocation.SelectedValue == "0")
                    {
                        _status = 3;
                        _isValid = false;
                    }
                    else
                    {
                        _isValid = true;
                    }
                }
            }
        }


        if (_isValid.Equals(false))
        {
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "validateRecContr", "validateRecContr('" + _status + "');", true);
        }
        return _isValid;
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            //if (Page.IsValid)
            //{
            if (ValidateRecContract())
            {
                objProp_Contracts.ConnConfig = Session["config"].ToString();
                objProp_Contracts.BAMT = double.Parse(txtBillAmt.Text, NumberStyles.Currency);
                objProp_Contracts.BCycle = Convert.ToInt32(ddlBillFreq.SelectedValue);
                objProp_Contracts.BStart = Convert.ToDateTime(txtBillStartDt.Text);
                objProp_Contracts.CreditCard = Convert.ToInt32(chkCredit.Checked);
                objProp_Contracts.Cycle = Convert.ToInt32(ddlSchFreq.SelectedValue);
                objProp_Contracts.Date = Convert.ToDateTime(txtScheduleStartDt.Text != string.Empty ? txtScheduleStartDt.Text : DateTime.Now.ToString());
                objProp_Contracts.Loc = Convert.ToInt32(hdnLocId.Value);
                objProp_Contracts.Owner = Convert.ToInt32(hdnPatientId.Value);
                objProp_Contracts.Remarks = txtRemarks.Text;
                objProp_Contracts.Sdate = Convert.ToInt32(ddlDay.SelectedValue);
                objProp_Contracts.Sday = Convert.ToInt32((txtDay.Text.Trim() != string.Empty) ? txtDay.Text : "0");
                objProp_Contracts.STime = Convert.ToDateTime(defaultDate + " " + (txtsTime.Text != string.Empty ? txtsTime.Text : DateTime.Now.ToShortTimeString()));
                objProp_Contracts.Status = Convert.ToInt32(ddlStatus.SelectedValue);
                objProp_Contracts.SStart = Convert.ToDateTime(txtScheduleStartDt.Text != string.Empty ? txtScheduleStartDt.Text : DateTime.Now.ToString());
                objProp_Contracts.DtElevJob = GetElevData();
                objProp_Contracts.Route = ddlRoute.SelectedValue;
                objProp_Contracts.SWE = Convert.ToInt32(chkWeekends.Checked);
                objProp_Contracts.Hours = Convert.ToDouble(txtBillHours.Text != string.Empty ? txtBillHours.Text : "0");
                objProp_Contracts.Ctype = ddlServiceType.SelectedValue;
                objProp_Contracts.Description = txtDescription.Text;
                objProp_Contracts.Handel = chkspnotes.Checked ? 1 : 0;
                objProp_Contracts.Notes = txtSpecialInstructions.Text;

                #region Renewal Notes

                objProp_Contracts.IsRenewalNotes = chkRenewalNotes.Checked ? 1 : 0;

                objProp_Contracts.RenewalNotes = txtRenewalNotes.Text;

                int ContractLength = 0;

                int.TryParse(txtContractLength.Text, out ContractLength);

                objProp_Contracts.ContractLength = ContractLength;


                #endregion
                objProp_Contracts.Detail = ddlBillDetailLevel.SelectedIndex;

                if (string.IsNullOrEmpty(txtGLAcct.Text))
                {
                    objProp_Contracts.Chart = 0;
                }
                else
                {
                    if (!string.IsNullOrEmpty(hdnGLAcct.Value))
                        objProp_Contracts.Chart = Convert.ToInt32(hdnGLAcct.Value);
                }


                DateTime unitexp = System.DateTime.MinValue;
                if (DateTime.TryParse(txtUnitExpiration.Text.Trim(), out unitexp))
                    objProp_Contracts.ExpirationDate = Convert.ToDateTime(txtUnitExpiration.Text.Trim());
                if (ddlExpiration.SelectedValue != string.Empty)
                    objProp_Contracts.Expiration = Convert.ToInt16(ddlExpiration.SelectedValue);
                if (txtNumFreq.Text.Trim() != string.Empty)
                    objProp_Contracts.expirationfreq = Convert.ToInt16(txtNumFreq.Text.Trim());

                objProp_Contracts.EscalationType = Convert.ToInt16(ddlEscType.SelectedValue);
                objProp_Contracts.EscalationCycle = (txtEscCycle.Text != string.Empty) ? Convert.ToInt32(txtEscCycle.Text) : 1;
                objProp_Contracts.EscalationFactor = (txtEscFactor.Text != string.Empty) ? Convert.ToDouble(txtEscFactor.Text) : 0;
                objProp_Contracts.EscalationLast = (txtEscdue.Text != string.Empty) ? Convert.ToDateTime(txtEscdue.Text) : DateTime.MinValue;

                if ((ddlSpecifiedLocation.Items.Count - 1) > 0)                                     // Customer billing details
                {                                                                                   // added by Mayuri 25th dec, 15
                    objProp_Contracts.CustBilling = Convert.ToInt16(ddlBilling.SelectedValue);
                    objProp_Contracts.Central = Convert.ToInt16(ddlSpecifiedLocation.SelectedValue);
                }
                else
                {
                    objPropUser.Billing = 0;
                }
                objProp_Contracts.JobTID = (int)ViewState["JobT"];
                DataTable dtCustom = GetCustomTemplate();
                dtCustom.Columns.Add("UpdateDate", typeof(DateTime));
                dtCustom.Columns.Add("Username", typeof(string));

                // Fixed ES-1455  TEI - Recurring Contracts getting an error saving
                dtCustom.Columns.Add("IsAlert", typeof(byte));
                dtCustom.Columns.Add("TeamMember", typeof(string));
                dtCustom.Columns.Add("TeamMemberDisplay", typeof(string));
                dtCustom.Columns.Add("UserRole", typeof(string));
                dtCustom.Columns.Add("UserRoleDisplay", typeof(string));
                objProp_Contracts.DtCustom = dtCustom;
                if (!string.IsNullOrEmpty(txtBillRate.Text))
                {
                    objProp_Contracts.BillRate = Convert.ToDouble(txtBillRate.Text);
                }
                if (!string.IsNullOrEmpty(txtOt.Text))
                {
                    objProp_Contracts.RateOT = Convert.ToDouble(txtOt.Text);
                }
                if (!string.IsNullOrEmpty(txtNt.Text))
                {
                    objProp_Contracts.RateNT = Convert.ToDouble(txtNt.Text);
                }
                if (!string.IsNullOrEmpty(txtDt.Text))
                {
                    objProp_Contracts.RateDT = Convert.ToDouble(txtDt.Text);
                }
                if (!string.IsNullOrEmpty(txtTravel.Text))
                {
                    objProp_Contracts.RateTravel = Convert.ToDouble(txtTravel.Text);
                }
                if (!string.IsNullOrEmpty(txtMileage.Text))
                {
                    objProp_Contracts.Mileage = Convert.ToDouble(txtMileage.Text);
                }
                if (!string.IsNullOrEmpty(txtPO.Text))
                {
                    objProp_Contracts.PO = txtPO.Text;
                }



                objProp_Contracts.taskcategory = ddlCodeCat.SelectedValue;

                objProp_Contracts.MOMUSer = Session["User"].ToString();

                if (Convert.ToInt32(ViewState["mode"]) == 1)
                {

                    objProp_Contracts.ContractBill = Convert.ToInt16(ddlContractBill.SelectedValue); // added by Mayuri 25th dec,15

                    objProp_Contracts.JobId = Convert.ToInt32(Request.QueryString["uid"].ToString());






                    if (!string.IsNullOrEmpty(txtOriginalContract.Text))
                    {
                        objProp_Contracts.OriginalContract = Convert.ToDateTime(txtOriginalContract.Text);
                    }
                    else
                    {
                        objProp_Contracts.OriginalContract = Convert.ToDateTime(txtBillStartDt.Text);
                    }


                    if (!string.IsNullOrEmpty(txtLastrenew.Text))
                    {
                        objProp_Contracts.LastRenew = Convert.ToDateTime(txtLastrenew.Text);
                    }
                    else
                    {
                        objProp_Contracts.LastRenew = objProp_Contracts.OriginalContract;
                    }

                    if (ddlTicketCat.SelectedIndex != 0)
                    {
                        objProp_Contracts.TicketCategory = ddlTicketCat.SelectedItem.Text;
                    }

                    if (ddlTerr.SelectedValue != string.Empty)
                    {
                        objPropUser.Territory = Convert.ToInt32(ddlTerr.SelectedValue);
                    }
                    //Second Salesperson
                    if (ddlTerr2.SelectedValue != string.Empty)
                    {
                        objPropUser.Territory2 = Convert.ToInt32(ddlTerr2.SelectedValue);
                    }
                    objPropUser.LocID= Convert.ToInt32(hdnLocId.Value);
                    objBL_User.Update_Loc_Terr(objPropUser);


                    objBL_Contracts.UpdateContract(objProp_Contracts);
                    ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: 'Contract updated successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                    RadGrid_gvLogs.Rebind();
                }
                else
                {                                                                                 // location billing details
                    objProp_Contracts.ConnConfig = Session["config"].ToString();                  // added by Mayuri 25th dec,15
                    objProp_Contracts.IsExistContract = objBL_Contracts.IsExistContractByLoc(objProp_Contracts);
                    if (objProp_Contracts.IsExistContract)
                    {
                        objProp_Contracts.ContractBill = Convert.ToInt16(ddlContractBill.SelectedValue);
                    }
                    else
                    {
                        objProp_Contracts.ContractBill = 0;
                    }

                    if (Request.QueryString["eid"] != null)
                    {
                        objProp_Contracts.EstimateId = Convert.ToInt32(Request.QueryString["eid"]);
                    }
                    else
                    {
                        objProp_Contracts.EstimateId = 0;
                    }




                    if (!string.IsNullOrEmpty(txtOriginalContract.Text))
                    {
                        objProp_Contracts.OriginalContract = Convert.ToDateTime(txtOriginalContract.Text);
                    }
                    else
                    {
                        objProp_Contracts.OriginalContract = Convert.ToDateTime(txtBillStartDt.Text);
                    }


                    if (!string.IsNullOrEmpty(txtLastrenew.Text))
                    {
                        objProp_Contracts.LastRenew = Convert.ToDateTime(txtLastrenew.Text);
                    }
                    else
                    {
                        objProp_Contracts.LastRenew = objProp_Contracts.OriginalContract;
                    }

                    if (ddlTicketCat.SelectedIndex != 0)
                    {
                        objProp_Contracts.TicketCategory = ddlTicketCat.SelectedItem.Text;
                    }

                    objBL_Contracts.AddContract(objProp_Contracts);

                    if (Request.QueryString["eid"] != null)
                    {
                        Session["ConvertToRecContractSucc"] = "1";
                        if (!string.IsNullOrEmpty(Request.QueryString["redirect"]))
                        {
                            Response.Redirect(HttpUtility.UrlDecode(Request.QueryString["redirect"]));
                        }
                        else
                        {
                            Response.Redirect("addestimate.aspx?uid=" + Request.QueryString["eid"]);
                        }
                    }

                    if (ddlTerr.SelectedValue != string.Empty)
                    {
                        objPropUser.Territory = Convert.ToInt32(ddlTerr.SelectedValue);
                    }
                    //Second Salesperson
                    if (ddlTerr2.SelectedValue != string.Empty)
                    {
                        objPropUser.Territory2 = Convert.ToInt32(ddlTerr2.SelectedValue);
                    }

                    objPropUser.LocID = Convert.ToInt32(hdnLocId.Value);
                    objBL_User.Update_Loc_Terr(objPropUser);


                    ClientScript.RegisterStartupScript(Page.GetType(), "keySucc", "noty({text: 'Contract added successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                    ResetFormControlValues(this);
                    gvEquip.DataBind();
                    RadGrid_gvLogs.Rebind();
                }

                if (Request.QueryString["rt"] != null)
                {
                    if (Request.QueryString["rt"] == "1")
                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "updateparent", "if(window.opener && !window.opener.closed) { if(window.opener.document.getElementById('ctl00_ContentPlaceHolder1_lnkSearch')) window.opener.document.getElementById('ctl00_ContentPlaceHolder1_lnkSearch').click();setTimeout(function(){window.close();},2000);}", true);
                    else if (Request.QueryString["rt"] == "2")
                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "updateparent", "if(window.opener && !window.opener.closed) { if(window.opener.document.getElementById('ctl00_ContentPlaceHolder1_lnkRefreshEsc')) window.opener.document.getElementById('ctl00_ContentPlaceHolder1_lnkRefreshEsc').click();setTimeout(function(){window.close();},2000);}", true);

                }
            }
            //}
        }
        catch (Exception ex)
        {

            string _er = "'error'";

            if (ex.Message.Contains("Please set up the labor wage at the project template level."))
            {
                _er = "'warring'";
            }

            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : " + _er + ", layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
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
                    //case "System.Web.UI.WebControls.DropDownList":
                    //    ((DropDownList)c).SelectedIndex = -1;
                    //    break;
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

    protected void lnkClose_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(Request.QueryString["redirect"]))
        {
            Response.Redirect(HttpUtility.UrlDecode(Request.QueryString["redirect"]));
        }
        else
        {
            Response.Redirect("reccontracts.aspx?fil=c", false);
        }
    }

    protected void btnSelectLoc_Click(object sender, EventArgs e)
    {
        FillLocInfo();
    }

    private void FillLocInfo()
    {
        if (hdnLocId.Value == "")
        {
            return;
        }
        objPropUser.DBName = Session["dbname"].ToString();
        objPropUser.ConnConfig = Session["config"].ToString();
        objPropUser.LocID = Convert.ToInt32(hdnLocId.Value);
        objPropUser.ConnConfig = Session["config"].ToString();
        DataSet ds = new DataSet();
        ds = objBL_User.getLocationByID(objPropUser);

        if (ds.Tables[0].Rows.Count > 0)
        {
            txtAddress.Text = ds.Tables[0].Rows[0]["LocAddress"].ToString();

            txtCompany.Text = ds.Tables[0].Rows[0]["Company"].ToString();
            txtCustomer.Text = ds.Tables[0].Rows[0]["custname"].ToString();
            hdnPatientId.Value = ds.Tables[0].Rows[0]["owner"].ToString();
            //ddlRoute.SelectedValue = Convert.ToString(ds.Tables[0].Rows[0]["route"]);

            var drpval = ddlRoute.Items.FindByValue(Convert.ToString(ds.Tables[0].Rows[0]["route"]));
            if (drpval != null)
            {
                ddlRoute.SelectedValue = drpval.Value;
            }
            else
            {
                ddlRoute.SelectedValue = "";
            }

            var drptype = ddlType.Items.FindByValue(Convert.ToString(ds.Tables[0].Rows[0]["Type"]));
            if (drptype != null)
            {
                ddlType.SelectedValue = drptype.Value;
            }
            else
            {
                ddlType.SelectedValue = "";
            }

            var drpTerr = ddlTerr.Items.FindByValue(Convert.ToString(ds.Tables[0].Rows[0]["Terr"]));
            if (drpTerr != null)
            {
                ddlTerr.SelectedValue = drpTerr.Value;
            }
            else
            {
                ddlTerr.SelectedValue = "";
            }

            var drpTerr2 = ddlTerr2.Items.FindByValue(Convert.ToString(ds.Tables[0].Rows[0]["Terr2"]));
            if (drpTerr2 != null)
            {
                ddlTerr2.SelectedValue = drpTerr2.Value;
            }
            else
            {
                ddlTerr2.SelectedValue = "";
            }

            if (Page.IsPostBack || Request.QueryString["eid"] != null)
            {
                if (ddlRoute.SelectedValue != "")
                    FillServiceTypeEdit(drptype.Value, "", Convert.ToInt32(ViewState["DepartmentID"].ToString()), Convert.ToInt32(ddlRoute.SelectedValue.ToString()));
            }

            txtBillRate.Text = string.Format("{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["BillRate"].ToString()));
            txtOt.Text = string.Format("{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["RateOT"].ToString()));
            txtNt.Text = string.Format("{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["RateNT"].ToString()));
            txtDt.Text = string.Format("{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["RateDT"].ToString()));
            txtMileage.Text = string.Format("{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["RateMileage"].ToString()));
            txtTravel.Text = string.Format("{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["RateTravel"].ToString()));
            if (Convert.ToString(ds.Tables[0].Rows[0]["credit"]) == "1")
                imgCreditHold.Visible = true;
            else
                imgCreditHold.Visible = false;


            string LoCStatus = ds.Tables[0].Rows[0]["status"].ToString();
            if (LoCStatus == "1")
            {
                btnSubmit.Visible = false;
                ClientScript.RegisterStartupScript(Page.GetType(), "keyerrrinactive", "noty({text: 'Location is inactive!.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            }
        }
        GetDataEquip();

        if (ddlRoute.SelectedValue == "")
        {
            ClientScript.RegisterStartupScript(Page.GetType(), "keyerrr", "noty({text: 'Please select an active " + lblDefaultWorker.InnerText + " for this location',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
        FillSpecifyLocation();
    }

    protected void btnSelectCustomer_Click(object sender, EventArgs e)
    {
        FillLoc();
        FillSpecifyLocation();
    }

    public void FillLoc()
    {
        DataSet ds = new DataSet();
        objPropUser.SearchValue = "";
        objPropUser.ConnConfig = Session["config"].ToString();
        objPropUser.CustomerID = Convert.ToInt32(hdnPatientId.Value);
        ds = objBL_User.getLocationAutojquery(objPropUser);

        if (ds.Tables[0].Rows.Count == 1)
        {
            hdnLocId.Value = ds.Tables[0].Rows[0]["value"].ToString();
            txtLocation.Text = ds.Tables[0].Rows[0]["label"].ToString();
            FillLocInfo();
        }
        else if (ds.Tables[0].Rows.Count == 0)
        {
            string str = "Please note this customer does not have a location.";
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }

        if (hdnLocId.Value != "")
        {
            lnkLocationID.NavigateUrl = "addlocation.aspx?uid=" + hdnLocId.Value;
            GetDataEquip();
        }
        else
        {
            lnkLocationID.NavigateUrl = "";
        }


        if (hdnPatientId.Value != "")
        {
            lnkCustomerID.NavigateUrl = "addcustomer.aspx?uid=" + hdnPatientId.Value;
        }
        else
        {
            lnkCustomerID.NavigateUrl = "";
        }

    }

    //protected void ddlExpiration_SelectedIndexChanged(object sender, EventArgs e)
    //{

    //    if (ddlExpiration.SelectedValue == "1")
    //    {
    //        txtUnitExpiration.Visible = true;
    //        txtNumFreq.Visible = false;
    //        RequiredFieldValidator4.Enabled = false;
    //        RequiredFieldValidator3.Enabled = true;
    //        txtContractLength.Text = hdnContractLength.Value;
    //    }
    //    else if (ddlExpiration.SelectedValue == "2")
    //    {
    //        txtUnitExpiration.Visible = false;
    //        txtNumFreq.Visible = true;
    //        //RequiredFieldValidator4.Enabled = true;
    //        RequiredFieldValidator3.Enabled = false;
    //        txtContractLength.Text = hdnContractLength.Value;
    //    }
    //    else
    //    {

    //        txtUnitExpiration.Visible = false;
    //        txtNumFreq.Visible = false;
    //        RequiredFieldValidator4.Enabled = false;
    //        RequiredFieldValidator3.Enabled = false;
    //        txtContractLength.Text = "999";
    //    }
    //}

    private void FillContractBill()
    {
        try
        {
            List<ContractBill> _lstBill = new List<ContractBill>();
            _lstBill = ContractBilling.GetAll();

            ddlContractBill.DataSource = _lstBill;
            ddlContractBill.DataValueField = "ID";
            ddlContractBill.DataTextField = "Name";
            ddlContractBill.DataBind();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void ddlContractBill_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlContractBill.SelectedValue == "1")
        {
            if (string.IsNullOrEmpty(hdnLocId.Value))
                hdnLocId.Value = "0";
            objProp_Contracts.ConnConfig = Session["config"].ToString();
            if (!string.IsNullOrEmpty(txtLocation.Text))
            {
                objProp_Contracts.Loc = Convert.ToInt32(hdnLocId.Value);
            }
            else
                objProp_Contracts.Loc = Convert.ToInt32(hdnLocId.Value);

            objProp_Contracts.IsExistContract = objBL_Contracts.IsExistContractByLoc(objProp_Contracts);
            if (!objProp_Contracts.IsExistContract)
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "dispWarningContract", "dispWarningContract();", true);
            }
        }
    }

    protected void txtCustomer_TextChanged(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(txtCustomer.Text))
        {
            FillSpecifyLocation();
        }
    }

    private void FillSpecifyLocation()
    {
        try
        {
            _objLoc.ConnConfig = Session["config"].ToString();

            DataSet _dsLocation = new DataSet();
            if (string.IsNullOrEmpty(hdnPatientId.Value))
                hdnPatientId.Value = "0";
            _dsLocation = objBL_Customer.getAllLocationOnCustomer(_objLoc, Convert.ToInt32(hdnPatientId.Value));

            ddlSpecifiedLocation.Items.Clear();
            if (_dsLocation.Tables[0].Rows.Count > 0)
            {
                ddlSpecifiedLocation.Items.Add(new ListItem(":: Select ::", "0"));
                ddlSpecifiedLocation.AppendDataBoundItems = true;

                ddlSpecifiedLocation.DataSource = _dsLocation;
                ddlSpecifiedLocation.DataValueField = "Loc";
                ddlSpecifiedLocation.DataTextField = "Tag";
                ddlSpecifiedLocation.DataBind();
            }
            else
            {
                ddlSpecifiedLocation.Items.Add(new ListItem("No Locations Available", "0"));
            }

            if (hdnLocId.Value != "")
            {
                lnkLocationID.NavigateUrl = "addlocation.aspx?uid=" + hdnLocId.Value;
                GetDataEquip();
            }
            else
            {
                lnkLocationID.NavigateUrl = "";
            }


            if (hdnPatientId.Value != "")
            {
                lnkCustomerID.NavigateUrl = "addcustomer.aspx?uid=" + hdnPatientId.Value;
            }
            else
            {
                lnkCustomerID.NavigateUrl = "";
            }

            //if (lblContrName.Text != "")
            //{
            //    lnkCustomerID.NavigateUrl = "addcustomer.aspx?uid=" + lblContrName.Text;
            //}
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void ddlServiceType_SelectedIndexChanged(object sender, EventArgs e)
    {
       
            if (ddlServiceType.SelectedValue != "")
            {

            string ServiceType = ddlServiceType.SelectedValue;

            objPropUser.ConnConfig = Session["config"].ToString();

            objPropUser.Type = ddlServiceType.SelectedValue;

            DataSet _ds = objBL_User.GetServiceTypeByType(objPropUser);

            if (_ds.Tables[0].Rows.Count > 0)
            {
                txtGLAcct.Text = _ds.Tables[0].Rows[0]["GLAcct"].ToString();
                hdnGLAcct.Value = _ds.Tables[0].Rows[0]["Sacct"].ToString();
                rfvGLAcct.ControlToValidate = "txtGLAcct";
                rfvGLAcct.Enabled = true;
            }


            DataSet _dsContract = new DataSet();
            
            _dsContract = new BusinessLayer.Programs.BL_ServiceType().spGetProjectServiceTypeinfo(Session["config"].ToString(), ddlServiceType.SelectedValue.ToString(), Convert.ToInt32(ViewState["DepartmentID"].ToString()), ddlType.SelectedValue.ToString(), Convert.ToInt32(ddlRoute.SelectedValue));
            if (_dsContract.Tables[0].Rows.Count > 0)
            {     

                if (Request.QueryString["uid"] != null)
                {
                    #region finance-general 
                     

                    int ExpenseGLValue = Convert.ToInt32(_dsContract.Tables[0].Rows[0]["ExpenseGLValue"].ToString()); 

                    int InterestGLValue = Convert.ToInt32(_dsContract.Tables[0].Rows[0]["InterestGLValue"].ToString()); 

                    int BillingValue = Convert.ToInt32(_dsContract.Tables[0].Rows[0]["BillingValue"].ToString()); 

                    int LaborWageValue = Convert.ToInt32(_dsContract.Tables[0].Rows[0]["LaborWageValue"].ToString()); 

                    int jobID = Convert.ToInt32(Request.QueryString["uid"].ToString());

                    objBL_User.updateServiceTypeByjobType(Session["config"].ToString(), jobID, ServiceType, ExpenseGLValue, InterestGLValue, BillingValue, LaborWageValue);
                }

                #endregion
            }

               
            }
            else
            {
                rfvGLAcct.Enabled = false;
            }
        
    }

    protected void cvSpecLoc_ServerValidate(object source, ServerValidateEventArgs args)
    {
        try
        {
            if (ddlBilling.SelectedValue == "1")
            {
                if (ddlSpecifiedLocation.SelectedValue == "0")
                {
                    args.IsValid = false;
                }
                else
                    args.IsValid = true;
            }
            else
            {
                args.IsValid = true;
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    private void FillBillDetailLevel()
    {

        ddlBillDetailLevel.Items.Add(new System.Web.UI.WebControls.ListItem("Summary", "0"));
        ddlBillDetailLevel.Items.Add(new System.Web.UI.WebControls.ListItem("Detailed", "1"));
        ddlBillDetailLevel.Items.Add(new System.Web.UI.WebControls.ListItem("Detailed w/Price", "2"));

    }

    protected void ddlSchFreq_SelectedIndexChanged(object sender, EventArgs e)
    {
        //When scheduling frequency is set to never we hide all the scheduling fields, like date, time, hours,

        if (ddlSchFreq.SelectedItem.Value == "-1")
        {
            Div_ScheStartDate.Visible = false;
            Div_ScheduledTime.Visible = false;
            Div_ScheTotalHours.Visible = false;
        }
        else
        {
            Div_ScheStartDate.Visible = true;
            Div_ScheduledTime.Visible = true;
            Div_ScheTotalHours.Visible = true;
        }
    }

    private void getDiagnosticCategory()
    {
        BL_General objBL_General = new BL_General();
        General objGeneral = new General();
        DataSet ds = new DataSet();
        objGeneral.ConnConfig = Session["config"].ToString();
        ds = objBL_General.getDiagnosticCategory(objGeneral);
        ddlCodeCat.DataSource = ds.Tables[0];
        ddlCodeCat.DataTextField = "category";
        ddlCodeCat.DataValueField = "category";
        ddlCodeCat.DataBind();

        ddlCodeCat.Items.Insert(0, new ListItem("Select", ""));
    }

    #endregion

    #region Control table Data
    private void GetControlData()
    {
        DataSet ds = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();
        ds = objBL_User.getControl(objPropUser);
        string Codes = ds.Tables[0].Rows[0]["codes"].ToString();
        if (Codes == "1") { DivTaskCategory.Visible = true; }
    }
    #endregion

    //protected void lblEqGrid_Click(object sender, EventArgs e)
    //{
    //    //string script = "function f(){$find(\"" + RadWindowEqGrid.ClientID + "\").show(); Sys.Application.remove_load(f);}Sys.Application.add_load(f); Materialize.updateTextFields();";
    //    //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
    //}

    #region logs
    protected void RadGrid_gvLogs_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        RadGrid_gvLogs.AllowCustomPaging = !ShouldApplySortFilterOrGroupLogs();
        if (Request.QueryString["uid"] != null)
        {
            DataSet dsLog = new DataSet();
            objProp_Contracts.ConnConfig = Session["config"].ToString();
            objProp_Contracts.JobId = Convert.ToInt32(Request.QueryString["uid"]);
            dsLog = objBL_Contracts.GetRecurringContractLogs(objProp_Contracts);
            if (dsLog.Tables[0].Rows.Count > 0)
            {
                RadGrid_gvLogs.VirtualItemCount = dsLog.Tables[0].Rows.Count;
                RadGrid_gvLogs.DataSource = dsLog.Tables[0];
            }
            else
            {
                RadGrid_gvLogs.DataSource = string.Empty;
            }
        }
    }
    bool isGroupLog = false;
    public bool ShouldApplySortFilterOrGroupLogs()
    {
        return RadGrid_gvLogs.MasterTableView.FilterExpression != "" ||
            (RadGrid_gvLogs.MasterTableView.GroupByExpressions.Count > 0 || isGroupLog) ||
            RadGrid_gvLogs.MasterTableView.SortExpressions.Count > 0;
    }
    protected void RadGrid_gvLogs_ItemCreated(object sender, GridItemEventArgs e)
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

    private void FillCategory()
    {
        DataSet ds = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();
        ds = objBL_User.getCategory(objPropUser);
        ddlTicketCat.DataSource = ds.Tables[0];
        ddlTicketCat.DataTextField = "type";
        ddlTicketCat.DataValueField = "type";
        ddlTicketCat.DataBind();
        ddlTicketCat.Items.Insert(0, new ListItem("select", ""));
    }

    private void SetDefaultWorker()
    {
        Customer objCustomer = new Customer();
        BL_Customer objBL_Customer = new BL_Customer();
        objCustomer.ConnConfig = Session["config"].ToString();
        string getValue = objBL_Customer.GetDefaultWorkerHeader(objCustomer);
        if (!string.IsNullOrEmpty(getValue))
        {
            lblDefaultWorker.InnerText = getValue;
        }
        else
        {
            lblDefaultWorker.InnerText = "Default Worker";
        }
    }
}
