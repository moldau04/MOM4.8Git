using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessEntity;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Drawing.Imaging;
using BusinessLayer;

public partial class AddManageCompanies : System.Web.UI.Page
{
    User objPropUser = new User();
    BL_User objBL_User = new BL_User();
    General _objPropGeneral = new General();
    BL_General _objBLGeneral = new BL_General();
    BusinessEntity.CompanyOffice objCompany = new BusinessEntity.CompanyOffice();
    BL_Company objBL_Company = new BL_Company();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userid"] == null)
        {
            Response.Redirect("login.aspx");
        }
        if (!IsPostBack)
        {
            PanelOffice();
            FillTerritory();
            GetAllChartAcct();
            FillRoute();
            FillZone();
            FillSalesTax();
            FillUseTax();
            FillLoctaionType();
            FillARTerms();
            ViewState["mode"] = 0;
            GetCompanyData();
        }
    }

    public void PanelOffice()
    {
        _objPropGeneral.ConnConfig = Session["config"].ToString();
        DataSet _dsCustomBranch = _objBLGeneral.getCustomFieldsControlBranch(_objPropGeneral);
        if (_dsCustomBranch.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow _dr in _dsCustomBranch.Tables[0].Rows)
            {
                if (_dr["Name"].ToString().Equals("MultiOffice"))
                {
                    if (_dr["Label"].ToString() == "1")
                    {
                        tdCompanyOffice.Visible = true;
                        lblCompany.Visible = false;
                        ddlCompany.Visible = false;
                    }
                    else
                    {
                        tdCompanyOffice.Visible = false;
                        lblCompany.Visible = false;
                        ddlCompany.Visible = false;
                    }
                }
                else
                {
                    tdCompanyOffice.Visible = false;
                    lblCompany.Visible = false;
                    ddlCompany.Visible = false;
                }
            }
        }
    }

    protected void rbCompany_CheckedChanged(object sender, System.EventArgs e)
    {
        lblHeader.Text = "Add Manage Company";
        //TabPanel1.HeaderText = "Company Info";
        //Label lblCompOfficeID = (Label)Page.FindControl("lblCompOfficeID");
        //lblCompOfficeID.Text = "Company ID";
        lblCompany.Visible = false;
        ddlCompany.Visible = false;
    }
    protected void rbOffice_CheckedChanged(object sender, System.EventArgs e)
    {
        lblHeader.Text = "Add Manage Office";
        //TabPanel1.HeaderText = "Office Info";
        //Label lblCompOfficeID = (Label)Page.FindControl("lblCompOfficeID");
        //lblCompOfficeID.Text = "Office ID";
        lblCompany.Visible = true;
        ddlCompany.Visible = true;
        FillCompany();
    }
    #region Fill All Dropdown
    private void FillCompany()
    {
        objCompany.ID = 0;
        objCompany.DBName = Session["dbname"].ToString();
        objCompany.ConnConfig = Session["config"].ToString();
        DataSet ds = new DataSet();
        ds = objBL_Company.getCompanyByID(objCompany);
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddlCompany.DataSource = ds.Tables[0];
            ddlCompany.DataTextField = "Name";
            ddlCompany.DataValueField = "ID";
            ddlCompany.DataBind();
            ddlCompany.Items.Insert(0, new ListItem("Select", "0"));
        }
    }

    private void GetAllChartAcct()
    {
        Chart _objChart = new Chart();
        BL_Chart _objBLChart = new BL_Chart();

        DataSet dsChart = new DataSet();
        _objChart.ConnConfig = Session["config"].ToString();

        dsChart = _objBLChart.GetAllChartByAsset(_objChart);


        dsChart.Tables[0].Columns.Add("ChartDesc", typeof(string), "fDesc + ' - ' + Acct");

        DrpAllChartAcct.DataSource = dsChart.Tables[0];
        DrpAllChartAcct.DataTextField = "ChartDesc";
        DrpAllChartAcct.DataValueField = "ID";
        DrpAllChartAcct.DataBind();

        DrpAllChartAcct.Items.Insert(0, new ListItem(":: Select ::", "0"));
    }

    private void FillTerritory()
    {
        DataSet ds = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();
        ds = objBL_User.getTerritory(objPropUser, new GeneralFunctions().GetSalesAsigned());
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddlSalePerson.DataSource = ds.Tables[0];
            ddlSalePerson.DataTextField = "Name";
            ddlSalePerson.DataValueField = "ID";
            ddlSalePerson.DataBind();
            ddlSalePerson.Items.Insert(0, new ListItem("Select", "0"));
        }
    }
    private void FillRoute()
    {
        Int32 LocID = 0;
        LocID = Request.QueryString["uid"] == null ? 0 : Convert.ToInt32(Request.QueryString["uid"].ToString());
        DataSet ds = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();
        ds = objBL_User.getRoute(objPropUser, 1, LocID, 0);//IsActive=1 :- Get Only Active Workers
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddlWorker.DataSource = ds.Tables[0];
            ddlWorker.DataTextField = "Name";
            ddlWorker.DataValueField = "ID";
            ddlWorker.DataBind();
            if (ds.Tables[1].Rows.Count > 0)
            {
                if (ddlWorker.Items.Contains(new ListItem(ds.Tables[1].Rows[0][0].ToString())))
                    ddlWorker.Items.FindByText(ds.Tables[1].Rows[0][0].ToString()).Selected = true;
            }
            // ddlRoute.Items.Insert(0, new ListItem(":: Select ::", ""));
            ddlWorker.Items.Insert(0, new ListItem("Select", "0"));
            ddlWorker.Items.Insert(1, new ListItem("Unassigned", "0"));

        }
    }
    private void FillZone()
    {
        objCompany.ID = 0;
        objCompany.DBName = Session["dbname"].ToString();
        objCompany.ConnConfig = Session["config"].ToString();
        DataSet ds = new DataSet();
        ds = objBL_Company.getZone(objCompany);
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddlZone.DataSource = ds.Tables[0];
            ddlZone.DataTextField = "Name";
            ddlZone.DataValueField = "ID";
            ddlZone.DataBind();
            ddlZone.Items.Insert(0, new ListItem("Select", "0"));
        }
    }

    private void FillSalesTax()
    {
        objCompany.ID = 0;
        objCompany.DBName = Session["dbname"].ToString();
        objCompany.ConnConfig = Session["config"].ToString();
        DataSet ds = new DataSet();
        ds = objBL_Company.getSTax(objCompany);
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddlSalesTax.DataSource = ds.Tables[0];
            ddlSalesTax.DataTextField = "Name";
            ddlSalesTax.DataValueField = "Name";
            ddlSalesTax.DataBind();
            ddlSalesTax.Items.Insert(0, new ListItem("Select", "0"));
        }
    }

    private void FillUseTax()
    {
        objCompany.ID = 0;
        objCompany.DBName = Session["dbname"].ToString();
        objCompany.ConnConfig = Session["config"].ToString();
        DataSet ds = new DataSet();
        ds = objBL_Company.getUseTax(objCompany);
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddlUseTax.DataSource = ds.Tables[0];
            ddlUseTax.DataTextField = "Name";
            ddlUseTax.DataValueField = "Name";
            ddlUseTax.DataBind();
            ddlUseTax.Items.Insert(0, new ListItem("Select", "0"));
        }
    }

    private void FillLoctaionType()
    {
        objCompany.ID = 0;
        objCompany.DBName = Session["dbname"].ToString();
        objCompany.ConnConfig = Session["config"].ToString();
        DataSet ds = new DataSet();
        ds = objBL_Company.getLocType(objCompany);
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddlLocationType.DataSource = ds.Tables[0];
            ddlLocationType.DataTextField = "Type";
            ddlLocationType.DataValueField = "Type";
            ddlLocationType.DataBind();
            ddlLocationType.Items.Insert(0, new ListItem("Select", "0"));
        }
    }

    private void FillARTerms()
    {
        objCompany.ID = 0;
        objCompany.DBName = Session["dbname"].ToString();
        objCompany.ConnConfig = Session["config"].ToString();
        DataSet ds = new DataSet();
        ds = objBL_Company.getARTerms(objCompany);
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddlARTerms.DataSource = ds.Tables[0];
            ddlARTerms.DataTextField = "Name";
            ddlARTerms.DataValueField = "ID";
            ddlARTerms.DataBind();
            ddlARTerms.Items.Insert(0, new ListItem("Select", "0"));
        }
    }

    #endregion
    #region Getdata
    public void GetCompanyData()
    {
        imgLogo.ImageUrl = null;
        imgLogo.Visible = false;
        Session["Logo"] = null;
        if (Request.QueryString["uid"] != null)
        {
            PanelOffice();
            if (Request.QueryString["t"] != null)
            {
                ViewState["mode"] = 0;
            }
            else
            {
                ViewState["mode"] = 1;

            }

            objCompany.DBName = Session["dbname"].ToString();
            objCompany.ConnConfig = Session["config"].ToString();
            DataSet ds = new DataSet();
            if (Convert.ToInt32(Request.QueryString["off"]) == 1)
            {
                lblHeader.Text = "Edit Office";
                rbOffice.Checked = true;
                rbOffice_CheckedChanged(null, null);
                objCompany.ID = Convert.ToInt32(Request.QueryString["uid"]);
                ds = objBL_Company.getOfficeByID(objCompany);
                ddlCompany.SelectedValue = ds.Tables[0].Rows[0]["Company"].ToString();
            }
            else
            {
                lblHeader.Text = "Edit Company";
                rbOffice.Checked = false;
                rbCompany_CheckedChanged(null, null);
                objCompany.ID = Convert.ToInt32(Request.QueryString["uid"]);
                ds = objBL_Company.getCompanyByID(objCompany);
            }
            if (ds.Tables[0].Rows.Count > 0)
            {
                lblHeader.Text = "Edit Company";
                if (Request.QueryString["t"] == "c")
                {
                    txtCompanyID.Enabled = true;
                    txtName.Enabled = true;
                    txtCompanyID.Text = string.Empty;
                    txtName.Text = string.Empty;
                    pnlSave.Visible = false;
                    lblHeader.Text = "Copy Company";
                    lblCompanyId.Visible = false;
                }
                else
                {
                    txtCompanyID.Enabled = false;
                    txtName.Enabled = false;
                    txtCompanyID.Text = ds.Tables[0].Rows[0]["ID"].ToString();
                    txtName.Text = ds.Tables[0].Rows[0]["Name"].ToString();
                }
                lblCompanyId.Text = String.Format("Company ID: #{0} | {1}", ds.Tables[0].Rows[0]["ID"].ToString(), ds.Tables[0].Rows[0]["Name"].ToString()); 
                //lblCompanyName.Text = ds.Tables[0].Rows[0]["Name"].ToString();
                txtContactName.Text = ds.Tables[0].Rows[0]["Manager"].ToString();
                txtAddress.Value = ds.Tables[0].Rows[0]["Address"].ToString();
                txtCity.Text = ds.Tables[0].Rows[0]["City"].ToString();
                ddlState.SelectedValue = ds.Tables[0].Rows[0]["State"].ToString();
                txtZip.Text = ds.Tables[0].Rows[0]["Zip"].ToString();
                txtPhone.Text = ds.Tables[0].Rows[0]["Phone"].ToString();
                txtFax.Text = ds.Tables[0].Rows[0]["Fax"].ToString();
                txtCostCenter.Text = ds.Tables[0].Rows[0]["CostCenter"].ToString();
                txtInvRemarks.Text = ds.Tables[0].Rows[0]["InvRemarks"].ToString();
                txtLongitude.Text = ds.Tables[0].Rows[0]["Longitude"].ToString();
                txtLatitude.Text = ds.Tables[0].Rows[0]["Latitude"].ToString();
                ddlCountry.SelectedValue = ds.Tables[0].Rows[0]["Country"].ToString();
                //Logo
                //txtLogoPath.Text = ds.Tables[0].Rows[0]["LogoPath"].ToString();
                txtBillRemit.Text = ds.Tables[0].Rows[0]["BillRemit"].ToString();
                txtPORemit.Text = ds.Tables[0].Rows[0]["PORemit"].ToString();
                ddlSalePerson.SelectedValue = ds.Tables[0].Rows[0]["LocDTerr"].ToString();
                ddlWorker.SelectedValue = ds.Tables[0].Rows[0]["LocDRoute"].ToString();
                ddlZone.SelectedValue = ds.Tables[0].Rows[0]["LocDZone"].ToString();
                ddlSalesTax.SelectedValue = ds.Tables[0].Rows[0]["LocDSTax"].ToString();
                ddlLocationType.SelectedValue = ds.Tables[0].Rows[0]["LocType"].ToString();
                ddlARTerms.SelectedValue = ds.Tables[0].Rows[0]["ARTerms"].ToString();
                txtADP.Text = ds.Tables[0].Rows[0]["ADP"].ToString();
                txtCB.Text = string.Format("{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["CB"].ToString()));
                txtARContact.Text = ds.Tables[0].Rows[0]["ARContact"].ToString();
                txtDArea.Text = ds.Tables[0].Rows[0]["DArea"].ToString();
                ddlDefState.SelectedValue = ds.Tables[0].Rows[0]["DState"].ToString();
                txtMileRate.Text = string.Format("{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["MileRate"]== DBNull.Value?"0" : ds.Tables[0].Rows[0]["MileRate"].ToString()));
                ddlUseTax.SelectedValue = ds.Tables[0].Rows[0]["UTax"].ToString();
                DrpAllChartAcct.SelectedValue = ds.Tables[0].Rows[0]["DInvAcct"].ToString();
                if (ds.Tables[0].Rows[0]["UTaxR"].ToString() == "1")
                    chkUTaxR.Checked = true;
                else
                    chkUTaxR.Checked = false;
                ddlStatus.SelectedValue = ds.Tables[0].Rows[0]["Status"].ToString();
                Session["logo"] = null;
                if (ds.Tables[0].Rows[0]["logo"] != DBNull.Value)
                {
                    imgLogo.Visible = true;
                    byte[] myByteArray = (byte[])ds.Tables[0].Rows[0]["logo"];
                    MemoryStream ms = new MemoryStream(myByteArray, 0, myByteArray.Length);
                    ms.Write(myByteArray, 0, myByteArray.Length);
                    System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true);
                    Session["logo"] = ResizeImage(image);
                    string img = "data:image/png;base64," + Convert.ToBase64String((byte[])ds.Tables[0].Rows[0]["logo"]);
                    imgLogo.ImageUrl = img;
                }
                else
                {
                    //imgLogo.ImageUrl = "images/blankimage.png";
                }
                if (isFirstItem())
                {
                    lnkFirst.Enabled = false;
                    lnkPrevious.Enabled = false;
                }
                if (isLastItem())
                {
                    lnkNext.Enabled = false;
                    lnkLast.Enabled = false;
                }
            }
        }
        else
        {
            lblCompanyId.Visible = false;
            pnlSave.Visible = false;
            imgLogo.Visible = false;
        }
    }
    private bool isLastItem()
    {
        DataTable dt = new DataTable();

        dt = (DataTable)Session["companyData"];

        DataColumn[] keyColumns = new DataColumn[1];
        keyColumns[0] = dt.Columns["ID"];
        dt.PrimaryKey = keyColumns;

        DataRow d = dt.Rows.Find(Request.QueryString["uid"].ToString());
        int index = dt.Rows.IndexOf(d);
        int c = dt.Rows.Count - 1;
        if (index == c)
        {
            return true;
        }
        return false;
    }
    private bool isFirstItem()
    {
        DataTable dt = new DataTable();
        dt = (DataTable)Session["companyData"];
        DataColumn[] keyColumns = new DataColumn[1];
        keyColumns[0] = dt.Columns["ID"];
        dt.PrimaryKey = keyColumns;

        DataRow d = dt.Rows.Find(Request.QueryString["uid"].ToString());
        int index = dt.Rows.IndexOf(d);

        if (index == 0)
        {
            return true;
        }
        return false;
    }
    #endregion
    protected void lnkFirst_Click(object sender, EventArgs e)
    {
        DataTable dt = (DataTable)Session["companyData"];
        Response.Redirect("AddManageCompanies.aspx?uid=" + dt.Rows[0]["id"]);
    }
    protected void lnkPrevious_Click(object sender, EventArgs e)
    {
        DataTable dt = (DataTable)Session["companyData"];
        DataColumn[] keyColumns = new DataColumn[1];
        keyColumns[0] = dt.Columns["ID"];
        dt.PrimaryKey = keyColumns;

        DataRow d = dt.Rows.Find(Request.QueryString["uid"].ToString());
        int index = dt.Rows.IndexOf(d);

        if (index > 0)
        {
            Response.Redirect("AddManageCompanies.aspx?uid=" + dt.Rows[index - 1]["id"]);
        }
    }
    protected void lnkNext_Click(object sender, EventArgs e)
    {
        DataTable dt = (DataTable)Session["companyData"];
        DataColumn[] keyColumns = new DataColumn[1];
        keyColumns[0] = dt.Columns["ID"];
        dt.PrimaryKey = keyColumns;

        DataRow d = dt.Rows.Find(Request.QueryString["uid"].ToString());
        int index = dt.Rows.IndexOf(d);
        int c = dt.Rows.Count - 1;
        if (index < c)
        {
            Response.Redirect("AddManageCompanies.aspx?uid=" + dt.Rows[index + 1]["id"]);
        }
    }
    protected void lnkLast_Click(object sender, EventArgs e)
    {
        DataTable dt = (DataTable)Session["companyData"];
        Response.Redirect("AddManageCompanies.aspx?uid=" + dt.Rows[dt.Rows.Count - 1]["id"]);
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        Submit();
    }
    private void Submit()
    {
        try
        {
            objCompany.ID = Convert.ToInt32(txtCompanyID.Text);
            objCompany.Name = txtName.Text;
            objCompany.Manager = txtContactName.Text;
            objCompany.Address = txtAddress.Value.Trim();
            objCompany.City = txtCity.Text.Trim();
            objCompany.State = ddlState.SelectedValue;
            objCompany.Zip = txtZip.Text;
            objCompany.Phone = txtPhone.Text;
            objCompany.Fax = txtFax.Text;
            objCompany.CostCenter = txtCostCenter.Text;
            objCompany.InvRemarks = txtInvRemarks.Text;
            objCompany.Logo = (byte[])Session["logo"];
            objCompany.LogoPath = txtLogoPath.Text;
            objCompany.Longitude = txtLongitude.Text;
            objCompany.Latitude = txtLatitude.Text;
            objCompany.Country = ddlCountry.SelectedValue;
            objCompany.BillRemit = txtBillRemit.Text;
            objCompany.PORemit = txtPORemit.Text;
            objCompany.LocDTerr = ddlSalePerson.SelectedValue;
            objCompany.LocDRoute = ddlWorker.SelectedValue;
            objCompany.LocDZone = ddlZone.SelectedValue;
            objCompany.LocDSTax = ddlSalesTax.SelectedValue;
            objCompany.LocType = ddlLocationType.SelectedValue;
            objCompany.ARTerms = ddlARTerms.SelectedValue;
            objCompany.ADP = txtADP.Text.Trim();
            objCompany.DInvAcct = Convert.ToInt32(DrpAllChartAcct.SelectedValue);
            if (!string.IsNullOrEmpty(txtCB.Text))
                objCompany.CB = Convert.ToDouble(txtCB.Text);
            else
                objCompany.CB = 0;
            objCompany.ARContact = txtARContact.Text.Trim();
            objCompany.DArea = txtDArea.Text.Trim();
            objCompany.DState = ddlDefState.SelectedValue;
            if (!string.IsNullOrEmpty(txtMileRate.Text))
                objCompany.MileRate = Convert.ToDouble(txtMileRate.Text);
            else
                objCompany.MileRate = 0;
            objCompany.PriceD1 = 0.00;
            objCompany.PriceD2 = 0.00;
            objCompany.PriceD3 = 0.00;
            objCompany.PriceD4 = 0.00;
            objCompany.PriceD5 = 0.00;
            if (chkUTaxR.Checked == true)
                objCompany.UTaxR = 1;
            else
                objCompany.UTaxR = 0;
            objCompany.UTax = ddlUseTax.SelectedValue;
            objCompany.Status = Convert.ToInt16(ddlStatus.SelectedValue);
            objCompany.ConnConfig = Session["config"].ToString();
            if (Convert.ToInt32(ViewState["mode"]) == 1)
            {
                objCompany.ID = Convert.ToInt32(Request.QueryString["uid"].ToString());

                if (rbOffice.Checked == true)
                {
                    objCompany.OType = "Office";
                    objCompany.Company = ddlCompany.SelectedValue;
                    objCompany.ChargeInt = null;
                    objBL_Company.UpdateOffice(objCompany);
                }
                else
                {
                    objCompany.OType = "Company";
                    objBL_Company.UpdateCompany(objCompany);
                }
                // ClientScript.RegisterStartupScript(Page.GetType(), "alert", "alert('Company Updated successfully!');", true);
                ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: 'Company updated successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
            }
            else
            {
                if (rbOffice.Checked == true)
                {
                    objCompany.OType = "Office";
                    objCompany.Company = ddlCompany.SelectedValue;
                    objCompany.ChargeInt = null;
                    objBL_Company.AddOffice(objCompany);
                    imgLogo.Visible = false;
                }
                else
                {
                    objCompany.OType = "Company";
                    objBL_Company.AddCompany(objCompany);
                    imgLogo.Visible = false;
                }

                ViewState["mode"] = 0;
                ClientScript.RegisterStartupScript(Page.GetType(), "keySucc", "noty({text: 'Company added successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                ResetFormControlValues(this);
                Session["Logo"] = null;
            }
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "updateparent", "if(window.opener && !window.opener.closed) { if(window.opener.document.getElementById('ctl00_ContentPlaceHolder1_lnkSearch')) window.opener.document.getElementById('ctl00_ContentPlaceHolder1_lnkSearch').click();}", true);
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            //ClientScript.RegisterStartupScript(Page.GetType(), "alert", "alert('"+ str +"');", true);
        }
    }

    protected void btnUpload_Click(object sender, EventArgs e)
    {
        if (FileUpload1.HasFile)
        {
            imgLogo.Visible = true;
            System.Drawing.Image imgfile = System.Drawing.Image.FromStream(FileUpload1.PostedFile.InputStream);
            Session["logo"] = null;
            Session["logo"] = ResizeImage(imgfile);
            string img = "data:image/png;base64," + Convert.ToBase64String(ResizeImage(imgfile));
            imgLogo.ImageUrl = img;
        }
    }

    private byte[] ResizeImage(System.Drawing.Image stImage)
    {
        byte[] bmpBytes = null;
        if (stImage != null)
        {
            // Create a bitmap of the content of the fileUpload control in memory
            Bitmap originalBMP = new Bitmap(stImage);
            double sngRatioraw = 0;
            int sngRatio = 0;
            int newWidth = 0;
            int newHeight = 0;
            // Calculate the new image dimensions
            int origWidth = originalBMP.Width;
            int origHeight = originalBMP.Height;
            if (origWidth > origHeight)
            {
                sngRatioraw = Convert.ToDouble(origWidth) / Convert.ToDouble(origHeight);
                newWidth = 225;
                sngRatio = Convert.ToInt32(Math.Round(sngRatioraw));
                newHeight = newWidth / sngRatio;
            }
            else
            {
                sngRatioraw = Convert.ToDouble(origHeight) / Convert.ToDouble(origWidth);
                newHeight = 225;
                sngRatio = Convert.ToInt32(Math.Round(sngRatioraw));
                newWidth = newHeight / sngRatio;
            }

            // Create a new bitmap which will hold the previous resized bitmap
            Bitmap newBMP = new Bitmap(originalBMP, newWidth, newHeight);

            // Create a graphic based on the new bitmap
            Graphics oGraphics = Graphics.FromImage(newBMP);
            // Set the properties for the new graphic file
            oGraphics.SmoothingMode = SmoothingMode.AntiAlias; oGraphics.InterpolationMode = InterpolationMode.HighQualityBicubic;

            // Draw the new graphic based on the resized bitmap
            oGraphics.DrawImage(originalBMP, 0, 0, newWidth, newHeight);



            bmpBytes = BmpToBytes_MemStream(newBMP);

            // Once finished with the bitmap objects, we deallocate them.
            originalBMP.Dispose();
            newBMP.Dispose();
            oGraphics.Dispose();
        }
        return bmpBytes;
    }

    private byte[] BmpToBytes_MemStream(Bitmap bmp)
    {
        MemoryStream ms = new MemoryStream();
        // Save to memory using the Jpeg format
        bmp.Save(ms, ImageFormat.Png);

        // read to end
        byte[] bmpBytes = ms.GetBuffer();
        bmp.Dispose();
        ms.Close();

        return bmpBytes;
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
                    case "System.Web.UI.WebControls.Image":
                        ((System.Web.UI.WebControls.Image)c).ImageUrl = null;
                        break;
                }
            }
        }
    }
    protected void lnkClose_Click(object sender, EventArgs e)
    {
        int intCompOffID = Convert.ToInt32(Request.QueryString["uid"]);
        int intCompOff = Convert.ToInt32(Request.QueryString["off"]);
        //Response.Redirect("ManageCompanies.aspx?uid=" + intCompOffID + "&off=" + intCompOff);
        Response.Redirect("ManageCompanies.aspx");
    }
}

