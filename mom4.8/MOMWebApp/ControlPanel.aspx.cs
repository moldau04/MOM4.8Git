using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessEntity;
using BusinessLayer;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Drawing.Imaging;
using BusinessEntity.CommonModel;

public partial class ControlPanel : System.Web.UI.Page
{
    User objPropUser = new User();
    BL_User objBL_User = new BL_User();
    General _objPropGeneral = new General();
    BL_General _objBLGeneral = new BL_General();
    Chart _objChart = new Chart();
    BL_Chart _objBLChart = new BL_Chart();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userid"] == null)
        {
            Response.Redirect("login.aspx");
        }

        if (!IsPostBack)
        {
            string SSL = System.Web.Configuration.WebConfigurationManager.AppSettings["SSL"].Trim();

            if (Request.Url.Scheme == "http" && SSL == "1")
            {
                string URL = Request.Url.ToString();
                URL = URL.Replace("http://", "https://");
                Response.Redirect(URL);
            }

            FillYearEnd();
            GetBillcodesforTimeSheet();
            GetAllChartAcct();
            FillAdminUsers();

            DataSet ds = new DataSet();
            objPropUser.ConnConfig = Session["config"].ToString();
            ds = objBL_User.getControl(objPropUser);

            if (ds.Tables[0].Rows.Count > 0)
            {

                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["YE"].ToString()))
                {
                    ddlYearEnd.SelectedValue = ds.Tables[0].Rows[0]["YE"].ToString();
                }
                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["GSTreg"].ToString()))    //change by dev for sales tax on 3rd feb, 16
                {
                    txtGSTReg.Text = ds.Tables[0].Rows[0]["GSTreg"].ToString();
                }
                _objPropGeneral.ConnConfig = Session["config"].ToString();
                DataSet _dsCustomBranch = _objBLGeneral.getCustomFieldsControlBranch(_objPropGeneral);
                if (_dsCustomBranch.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow _dr in _dsCustomBranch.Tables[0].Rows)
                    {
                        if (_dr["Name"].ToString().Equals("Branch"))
                        {
                            if (_dr["Label"].ToString() == "1")
                                ChkMultiCompany.Checked = true;
                            else
                                ChkMultiCompany.Checked = false;
                        }
                        else if (_dr["Name"].ToString().Equals("MultiOffice"))
                        {
                            if (_dr["Label"].ToString() == "1")
                                chkMultiOffice.Checked = true;
                            else
                                chkMultiOffice.Checked = false;
                        }
                    }
                }
                _objPropGeneral.ConnConfig = Session["config"].ToString();
                DataSet _dsCustom = _objBLGeneral.getCustomFieldsControl(_objPropGeneral);
                if (_dsCustom.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow _dr in _dsCustom.Tables[0].Rows)
                    {
                        if (_dr["Name"].ToString().Equals("Country"))
                        {
                            ddlCountry.SelectedValue = _dr["Label"].ToString();
                            if (ddlCountry.SelectedValue == 1.ToString())
                            {
                                divStateID.Visible = false;
                                divFederalID.Visible = false;
                            }
                            else
                            {
                                divProvincialID.Visible = false;
                            }
                        }

                        if (_dr["Name"].ToString().Equals("NextInv"))
                        {
                            txtNextInvoiceNumber.Text = _dr["Label"].ToString();
                        }

                        if (_dr["Name"].ToString().Equals("NextPO"))
                        {
                            txtNextPONumber.Text = _dr["Label"].ToString();
                            hdnNextPONumber.Value = _dr["Label"].ToString();
                        }

                        if (_dr["Name"].ToString().Equals("NextEst"))
                        {
                            txtNextEstimateNumber.Text = _dr["Label"].ToString();
                        }
                        if (_dr["Name"].ToString().Equals("StateID"))
                        {
                            txtStateID.Text = _dr["Label"].ToString();
                        }
                        if (_dr["Name"].ToString().Equals("FederalID"))
                        {
                            txtFederalID.Text = _dr["Label"].ToString();
                        }
                        if (_dr["Name"].ToString().Equals("ProvincialID"))
                        {
                            txtProvincialID.Text = _dr["Label"].ToString();
                        }

                        else if (_dr["Name"].ToString().Equals("GSTGL"))
                        {
                            if (!string.IsNullOrEmpty(_dr["Label"].ToString()))
                            {
                                _objChart.ConnConfig = Session["config"].ToString();
                                _objChart.ID = Convert.ToInt32(_dr["Label"].ToString());
                                DataSet _dsChart = _objBLChart.GetChart(_objChart);

                                if (_dsChart.Tables[0].Rows.Count > 0)
                                {
                                    txtGSTGL.Text = _dsChart.Tables[0].Rows[0]["fDesc"].ToString();
                                    hdnGSTGL.Value = _dr["Label"].ToString();
                                }

                            }
                        }
                        else if (_dr["Name"].ToString().Equals("GSTRate"))
                        {
                            if (!string.IsNullOrEmpty(_dr["Label"].ToString()))
                            {
                                txtGSTRate.Text = _dr["Label"].ToString();
                            }
                            else
                            {
                                txtGSTRate.Text = "0";
                            }
                        }

                        else if (_dr["Name"].ToString().Equals("InvGL"))
                        {
                            if (!string.IsNullOrEmpty(_dr["Label"].ToString()) && _dr["Label"].ToString() != "0")
                            {
                                ChkInventoryTracking.Checked = Convert.ToBoolean(_dr["Label"]);
                            }

                        }
                        else if (_dr["Name"].ToString().Equals("DefaultInvGLAcct"))
                        {
                            if (!string.IsNullOrEmpty(_dr["Label"].ToString()) && _dr["Label"].ToString() != "0")
                            {
                                DrpAllChartAcct.SelectedValue = _dr["Label"].ToString();
                            }

                        }
                        else if (_dr["Name"].ToString().Equals("SalesTax2"))
                        {
                            if (!string.IsNullOrEmpty(_dr["Label"].ToString()) && _dr["Label"].ToString() == "1")
                            {
                                chkSalesTax2.Checked = true;
                            }

                        }
                        else if (_dr["Name"].ToString().Equals("UseTax"))
                        {
                            if (!string.IsNullOrEmpty(_dr["Label"].ToString()) && _dr["Label"].ToString() == "1")
                            {
                                chkUseTax.Checked = true;
                            }

                        }
                        else if (_dr["Name"].ToString().Equals("Zone"))
                        {
                            if (!string.IsNullOrEmpty(_dr["Label"].ToString()) && _dr["Label"].ToString() == "1")
                            {
                                chkZone.Checked = true;
                            }

                        }
                    }
                }

                // For password settings
                chkApplyPasswordRules.Checked = string.IsNullOrEmpty(ds.Tables[0].Rows[0]["ApplyPasswordRules"].ToString()) ? false : Convert.ToBoolean(ds.Tables[0].Rows[0]["ApplyPasswordRules"].ToString());
                chkApplyFieldUser.Checked = string.IsNullOrEmpty(ds.Tables[0].Rows[0]["ApplyPwRulesToFieldUser"].ToString()) ? false : Convert.ToBoolean(ds.Tables[0].Rows[0]["ApplyPwRulesToFieldUser"].ToString());
                chkApplyOfficeUsers.Checked = string.IsNullOrEmpty(ds.Tables[0].Rows[0]["ApplyPwRulesToOfficeUser"].ToString()) ? false : Convert.ToBoolean(ds.Tables[0].Rows[0]["ApplyPwRulesToOfficeUser"].ToString());
                chkApplyCustomerUsers.Checked = string.IsNullOrEmpty(ds.Tables[0].Rows[0]["ApplyPwRulesToCustomerUser"].ToString()) ? false : Convert.ToBoolean(ds.Tables[0].Rows[0]["ApplyPwRulesToCustomerUser"].ToString());
                chkPwResetDays.Checked = string.IsNullOrEmpty(ds.Tables[0].Rows[0]["ApplyPwReset"].ToString()) ? false : Convert.ToBoolean(ds.Tables[0].Rows[0]["ApplyPwReset"].ToString());
                txtPwResetDays.Text = ds.Tables[0].Rows[0]["PwResetDays"].ToString();
                if (string.IsNullOrEmpty(ds.Tables[0].Rows[0]["PwResetting"].ToString())) ddlPasswordResetOption.SelectedIndex = 0;
                else ddlPasswordResetOption.SelectedValue = ds.Tables[0].Rows[0]["PwResetting"].ToString();
                //txtAdminEmail.Text = ds.Tables[0].Rows[0]["EmailAdministrator"].ToString();
                var pwResetUserID = ds.Tables[0].Rows[0]["PwResetUserID"].ToString();
                if (!string.IsNullOrEmpty(pwResetUserID)) ddlAdminUser.SelectedValue = pwResetUserID;

                ddlService.SelectedValue = ds.Tables[0].Rows[0]["QBserviceItem"].ToString();
                ddlServiceExpense.SelectedValue = ds.Tables[0].Rows[0]["QBServiceItemExp"].ToString();
                ddlServicelabor.SelectedValue = ds.Tables[0].Rows[0]["QBServiceItemLabor"].ToString();
                //if (ds.Tables[0].Rows[0]["SyncTimesheet"]!=DBNull.Value)
                //chkSyncTimesheet.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["SyncTimesheet"]);
                //if (ds.Tables[0].Rows[0]["SyncInvoice"] != DBNull.Value)
                //chkSyncInvoice.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["SyncInvoice"]);

                txtCompany.Text = ds.Tables[0].Rows[0]["Name"].ToString();
                txtAddress.Value = ds.Tables[0].Rows[0]["Address"].ToString();
                txtCity.Text = ds.Tables[0].Rows[0]["City"].ToString();
                ddlState.SelectedValue = ds.Tables[0].Rows[0]["state"].ToString();
                txtZip.Text = ds.Tables[0].Rows[0]["zip"].ToString();
                txtTele.Text = ds.Tables[0].Rows[0]["phone"].ToString();
                txtFax.Text = ds.Tables[0].Rows[0]["fax"].ToString();
                txtEmail.Text = ds.Tables[0].Rows[0]["email"].ToString();
                txtWebAdd.Text = ds.Tables[0].Rows[0]["webaddress"].ToString();
                ddlDBType.SelectedValue = ds.Tables[0].Rows[0]["msm"].ToString();
                txtDB.Text = ds.Tables[0].Rows[0]["dbname"].ToString();
                ddlDBType.Enabled = false;
                txtDB.Enabled = false;
                if (ds.Tables[0].Rows[0]["custweb"].ToString() == "")
                    chkCustRegistrn.Checked = false;
                else
                    chkCustRegistrn.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["custweb"]);
                if (ds.Tables[0].Rows[0]["consultAPI"].ToString() == "")
                {
                    ChkConsultantAPI.Checked = false;
                }
                else
                {
                    ChkConsultantAPI.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["consultAPI"]);
                }
                if (ds.Tables[0].Rows[0]["PR"].ToString() == "")
                {
                    chkpayroll.Checked = false;
                }
                else
                {
                    chkpayroll.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["PR"]);
                }
                txtFilePath.Text = ds.Tables[0].Rows[0]["QBPath"].ToString();
                if (ds.Tables[0].Rows[0]["multilang"].ToString() == "")
                    chkMultilang.Checked = false;
                else
                    chkMultilang.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["multilang"]);
                if (ds.Tables[0].Rows[0]["msemailnull"].ToString() == "")
                    chkMSEmail.Checked = false;
                else
                    chkMSEmail.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["msemailnull"]);
                if (ds.Tables[0].Rows[0]["EmpSync"].ToString() == "")
                    chkSyncEmp.Checked = false;
                else
                    chkSyncEmp.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["EmpSync"]);
                if (ds.Tables[0].Rows[0]["QBIntegration"].ToString() == "")
                    chkAcctIntegration.Checked = false;
                else
                    chkAcctIntegration.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["QBIntegration"]);

                chkAcctIntegration_CheckedChanged(sender, e);
                lat.Value = ds.Tables[0].Rows[0]["Lat"].ToString();
                lng.Value = ds.Tables[0].Rows[0]["Lng"].ToString();
                if (ds.Tables[0].Columns.Contains("Contact") && ds.Tables[0].Columns.Contains("Remarks"))
                {
                    txtRemarks.Value = ds.Tables[0].Rows[0]["remarks"].ToString();
                    txtContName.Text = ds.Tables[0].Rows[0]["Contact"].ToString();
                }
                Session["logo"] = null;
                if (ds.Tables[0].Rows[0]["logo"] != DBNull.Value)
                {
                    try
                    {
                        byte[] myByteArray = (byte[])ds.Tables[0].Rows[0]["logo"];

                        MemoryStream ms = new MemoryStream(myByteArray, 0, myByteArray.Length);
                        ms.Write(myByteArray, 0, myByteArray.Length);
                        System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true);
                        Session["logo"] = ResizeImage(image);
                        string img = "data:image/png;base64," + Convert.ToBase64String((byte[])ds.Tables[0].Rows[0]["logo"]);
                        imgLogo.ImageUrl = img;
                    }
                    catch (Exception ex)
                    {

                    }
                }
                else
                {
                    //imgLogo.ImageUrl = "images/blankimage.png";
                }
                //ScriptManager.RegisterStartupScript(Page, this.GetType(), "showFirstTab", "showFirstTab();", true);
            }

            //Start-- API Changes : Juily:04/06/2020 --//
            if (Session["Username"].ToString() == "Maintenance")
            {
                //accrdAPI.Visible = true;
                //API.Visible = true;
                chkpayroll.Visible = true;
                GetAPIIntegrationEnable();
            }
            else
            {
                chkpayroll.Visible = false;
                liAPI.Visible = false;
                //accrdAPI.Visible = false;
                API.Visible = false;
            }
            //End-- API Changes : Juily:04/06/2020 --//



        }

        Permission();
        HighlightSideMenu("progMgr", "lnkCntrlPnl", "progMgrSub");

    }

    private void HighlightSideMenu(string MenuParent, string PageLink, string SubMenuDiv)
    {
        HyperLink aNav = (HyperLink)Page.Master.FindControl(MenuParent);
        //li.Style.Add("background", "url(images/active_menu_bg.png) repeat-x");
        aNav.CssClass = "active collapsible-header waves-effect waves-cyan collapsible-height-nl";

        //HyperLink a = (HyperLink)Page.Master.Master.FindControl("SalesLink");
        //a.Style.Add("color", "#2382b2");

        HyperLink lnkUsersSmenu = (HyperLink)Page.Master.FindControl(PageLink);
        //lnkUsersSmenu.Style.Add("color", "#FF7A0A");
        lnkUsersSmenu.Style.Add("color", "#316b9d");
        lnkUsersSmenu.Style.Add("font-weight", "normal");
        lnkUsersSmenu.Style.Add("background-color", "#e5e5e5");
        //AjaxControlToolkit.HoverMenuExtender hm = (AjaxControlToolkit.HoverMenuExtender)Page.Master.Master.FindControl("HoverMenuExtenderSales");
        //hm.Enabled = false;
        HtmlGenericControl div = (HtmlGenericControl)Page.Master.FindControl(SubMenuDiv);
        div.Style.Add("display", "block");
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
                newWidth = 400;
                sngRatio = Convert.ToInt32(Math.Round(sngRatioraw));
                newHeight = newWidth / sngRatio;
            }
            else
            {
                sngRatioraw = Convert.ToDouble(origHeight) / Convert.ToDouble(origWidth);
                newHeight = 400;
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

    private void Permission()
    {

        //AjaxControlToolkit.HoverMenuExtender hm = (AjaxControlToolkit.HoverMenuExtender)Page.Master.FindControl("HoverMenuExtenderProg");
        //hm.Enabled = false;
        //HtmlGenericControl ul = (HtmlGenericControl)Page.Master.FindControl("progMgrSub");
        //ul.Style.Add("display", "block");
        //ul.Style.Add("visibility", "visible");

        if (Session["user"].ToString() != "Maintenance")
        {
            chkpayroll.Visible = false;
        }
        if (Session["type"].ToString() != "am")
        {
            lblCustReg.Visible = false;
            chkCustRegistrn.Visible = false;
            lblConAPI.Visible = false;
            ChkConsultantAPI.Visible = false;
            DataTable dt = new DataTable();
            dt = (DataTable)Session["userinfo"];

            string ProgFunc = dt.Rows[0]["Control"].ToString().Substring(0, 1);
            if (ProgFunc == "N")
            {
                Response.Redirect("home.aspx");
            }
        }

        if (Session["MSM"].ToString() == "TS")
        {
            //btnSubmit.Visible = false;
            Response.Redirect("home.aspx");
        }
        if (Convert.ToInt32(Session["ISsupervisor"]) == 1)
        {
            Response.Redirect("home.aspx");
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtNextPONumber.Text != "")
            {
                int nextPO = Convert.ToInt32(txtNextPONumber.Text);
                int LastPO = 0;
                if (hdnNextPONumber.Value != "")
                {
                    LastPO = Convert.ToInt32(hdnNextPONumber.Value);
                }
                if (LastPO > nextPO)
                {
                    //string strScripts = string.Empty;
                    //strScripts += "noty({text: 'NextPO number can not be less then '" + Convert.ToString(LastPO)+"',  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});";
                    //ScriptManager.RegisterStartupScript(Page, this.GetType(), "keySuccUps", strScripts, true);
                    //return;
                    string strScripts = "NextPO number can not be less then " + Convert.ToString(LastPO) ;
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningInvInactives", "noty({text: '"+strScripts+"',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
                    return;
                }
                
            }
            

            if (ddlCountry.SelectedValue.Equals("1"))
            {
                if (txtGSTGL.Text.Trim() == "")
                {
                    string strScripts = string.Empty;
                    strScripts += "noty({text: 'GST GL Acct is missing , Please add the acct.',  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});";
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "keySuccUp", strScripts, true);
                    return;
                }


            }


            objPropUser.FirstName = txtCompany.Text;
            objPropUser.Address = txtAddress.Value;
            objPropUser.City = txtCity.Text;
            objPropUser.State = ddlState.SelectedValue;
            objPropUser.Zip = txtZip.Text;
            objPropUser.Tele = txtTele.Text;
            objPropUser.Fax = txtFax.Text;
            objPropUser.Email = txtEmail.Text;
            objPropUser.Website = txtWebAdd.Text;
            objPropUser.MSM = ddlDBType.SelectedValue;
            objPropUser.DSN = "";//txtDSN.Text.Trim();
            objPropUser.DBName = txtDB.Text.Trim();
            objPropUser.Password = "";//txtDpass.Text.Trim();
            objPropUser.Username = "";// txtDuser.Text.Trim();
            objPropUser.ConnConfig = Session["config"].ToString();
            objPropUser.ContactName = txtContName.Text;
            objPropUser.CustWeb = Convert.ToInt16(chkCustRegistrn.Checked);
            objPropUser.ConsultAPI = Convert.ToInt16(ChkConsultantAPI.Checked);
            objPropUser.Payroll = Convert.ToInt16(chkpayroll.Checked);
            objPropUser.QBPath = txtFilePath.Text;
            //System.Drawing.Image st = (System.Drawing.Image)Session["logo"] ;
            //objPropUser.Logo = ResizeImage(st);
            objPropUser.Logo = (byte[])Session["logo"];
            objPropUser.MultiLang = Convert.ToInt16(chkMultilang.Checked);
            objPropUser.QBInteg = Convert.ToInt16(chkAcctIntegration.Checked);
            objPropUser.EmailMS = Convert.ToInt16(chkMSEmail.Checked);
            objPropUser.QBFirstSync = Convert.ToInt16(chkSyncEmp.Checked);
            objPropUser.QBSalesTaxID = ddlService.SelectedValue;
            objPropUser.QbserviceItemlabor = ddlServicelabor.SelectedValue;
            objPropUser.QBserviceItemExp = ddlServiceExpense.SelectedValue;
            objPropUser.Lat = lat.Value;
            objPropUser.Lng = lng.Value;

            // Password settings
            objPropUser.ApplyPasswordRules = chkApplyPasswordRules.Checked;
            if (chkApplyPasswordRules.Checked)
            {
                objPropUser.ApplyPwRulesToFieldUser = chkApplyFieldUser.Checked;
                objPropUser.ApplyPwRulesToOfficeUser = chkApplyOfficeUsers.Checked;
                objPropUser.ApplyPwRulesToCustomerUser = chkApplyCustomerUsers.Checked;
                if (!chkApplyFieldUser.Checked && !chkApplyOfficeUsers.Checked && !chkApplyCustomerUsers.Checked)
                {
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarning", "noty({text: 'The policy should be applied for as-least an user type',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                    return;
                }

                objPropUser.ApplyPwResetDays = chkPwResetDays.Checked;
                if (chkPwResetDays.Checked)
                {
                    var resetDays = 0;
                    int.TryParse(txtPwResetDays.Text, out resetDays);
                    objPropUser.PwResetDays = resetDays;
                }
                else
                {
                    objPropUser.PwResetDays = 0;
                }
                objPropUser.PwResetting = Convert.ToInt16(ddlPasswordResetOption.SelectedValue);
                //objPropUser.EmailAdministrator = txtAdminEmail.Text;
                objPropUser.UserID = string.IsNullOrEmpty(ddlAdminUser.SelectedValue) ? 0 : Convert.ToInt32(ddlAdminUser.SelectedValue);
                if (objPropUser.UserID == 0)
                {
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarning", "noty({text: 'Please select an administrator user from the list.  If your list is empty, you can add it by updating an user account including email settings info.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                    return;
                }
            }
            else
            {
                objPropUser.ApplyPwRulesToFieldUser = objPropUser.ApplyPwRulesToOfficeUser = objPropUser.ApplyPwRulesToCustomerUser = false;
                objPropUser.ApplyPwResetDays = false;
                objPropUser.PwResetDays = null;
                objPropUser.PwResetting = null;
                objPropUser.EmailAdministrator = "";
                objPropUser.UserID = 0;

                chkApplyFieldUser.Checked = chkApplyOfficeUsers.Checked = chkApplyCustomerUsers.Checked = false;
                chkPwResetDays.Checked = false;
                txtPwResetDays.Text = "";
                ddlPasswordResetOption.SelectedIndex = 0;
                //txtAdminEmail.Text = "";
                ddlAdminUser.SelectedIndex = 0;
            }

            if (!ddlYearEnd.SelectedValue.Equals(":: Select ::"))
            {
                objPropUser.YE = Convert.ToInt32(ddlYearEnd.SelectedValue);
            }
            objPropUser.GSTReg = txtGSTReg.Text;            // change by dev 3rd Feb, 16

            //objPropUser.TransferInvoice = Convert.ToInt16(chkSyncInvoice.Checked);
            //objPropUser.TransferTimeSheet= Convert.ToInt16(chkSyncTimesheet.Checked);

            //objPropUser.MerchantID = txtMerchantID.Text.Trim();
            //objPropUser.LoginID = txtLoginID.Text.Trim();
            //objPropUser.PaymentUser = txtPayUser.Text.Trim();
            //objPropUser.PaymentPass = AES_Algo.Encrypt(txtPayPass.Text.Trim(), "MSMPAY", "4Bvq75DG", "SHA1", 1000, "pOWaTbO92LfXbh69JkYzfT7P465TNc0h", 256);

            //if (Convert.ToInt32(ViewState["mode"]) == 1)
            //{
            objPropUser.IsOnlinePaymentApply = chkOnllinePaymentSetting.Checked;
            objBL_User.UpdateCompany(objPropUser);

            //ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: 'Company info. updated successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
            string strScript = string.Empty;
            strScript += "noty({text: 'Settings updated successfully',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});";
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "keySuccUp", strScript, true);
            //lblMsg.Text = "User updated successfully.";
            //ClearControls();
            //}
            //else
            //{
            //    objBL_User.AddCompany(objPropUser);
            //    ViewState["mode"] = 0;
            //    lblMsg.Text = "Company added successfully.";
            //    ClearControls();
            //}
            _objPropGeneral.ConnConfig = Session["config"].ToString();  // change by dev on 3rd feb, 16
            _objPropGeneral.CustomLabel = ddlCountry.SelectedValue;
            _objPropGeneral.CustomName = "Country";
            _objBLGeneral.UpdateCustom(_objPropGeneral);

            #region Next Invoice Number
            _objPropGeneral.CustomLabel = txtNextInvoiceNumber.Text == "" ? "1" : txtNextInvoiceNumber.Text;
            _objPropGeneral.CustomName = "NextInv";
            _objBLGeneral.UpdateCustom(_objPropGeneral);
            #endregion

            #region Next PO Number
            _objPropGeneral.CustomLabel = txtNextPONumber.Text == "" ? "1" : txtNextPONumber.Text;
            _objPropGeneral.CustomName = "NextPO";
            _objBLGeneral.UpdateCustom(_objPropGeneral);
            hdnNextPONumber.Value = txtNextPONumber.Text;
            #endregion

            #region State ID Number
            _objPropGeneral.CustomLabel = txtStateID.Text;
            _objPropGeneral.CustomName = "StateID";
            _objBLGeneral.UpdateCustom(_objPropGeneral);
            #endregion

            #region Federal ID Number
            _objPropGeneral.CustomLabel = txtFederalID.Text;
            _objPropGeneral.CustomName = "FederalID";
            _objBLGeneral.UpdateCustom(_objPropGeneral);
            #endregion

            #region Provincial ID Number
            _objPropGeneral.CustomLabel = txtProvincialID.Text;
            _objPropGeneral.CustomName = "ProvincialID";
            _objBLGeneral.UpdateCustom(_objPropGeneral);
            #endregion

            #region Next Estimate Number
            _objPropGeneral.CustomLabel = txtNextEstimateNumber.Text == "" ? "1" : txtNextEstimateNumber.Text;
            _objPropGeneral.CustomName = "NextEst";
            _objBLGeneral.UpdateCustom(_objPropGeneral);
            #endregion

            if (ChkMultiCompany.Checked == true)
            {
                if (chkMultiOffice.Checked == true)
                    _objPropGeneral.CustomLabel = "1";
                else
                    _objPropGeneral.CustomLabel = "0";
                _objPropGeneral.CustomName = "MultiOffice";
                _objBLGeneral.UpdateCustom(_objPropGeneral);
                _objPropGeneral.CustomLabel = "1";
            }
            else
                _objPropGeneral.CustomLabel = "0";
            _objPropGeneral.CustomName = "Branch";
            _objBLGeneral.UpdateCustom(_objPropGeneral);
            if (ddlCountry.SelectedValue.Equals("1"))
            {
                if (txtGSTRate.Text.Trim() == "")
                {
                    txtGSTRate.Text = "0";
                }
                _objPropGeneral.CustomLabel = txtGSTRate.Text;
                _objPropGeneral.CustomName = "GSTRate";
                _objBLGeneral.UpdateCustom(_objPropGeneral);


                _objPropGeneral.CustomLabel = hdnGSTGL.Value;
                _objPropGeneral.CustomName = "GSTGL";
                _objBLGeneral.UpdateCustom(_objPropGeneral);
            }
            if (ChkInventoryTracking.Checked)
            {
                _objPropGeneral.CustomLabel = true.ToString();
                _objPropGeneral.CustomName = "InvGL";
                _objBLGeneral.UpdateCustom(_objPropGeneral);
            }
            else
            {
                _objPropGeneral.CustomLabel = false.ToString();
                _objPropGeneral.CustomName = "InvGL";
                _objBLGeneral.UpdateCustom(_objPropGeneral);
            }
            if (ChkInventoryTracking.Checked)
            {
                _objPropGeneral.CustomLabel = DrpAllChartAcct.SelectedValue;
                _objPropGeneral.CustomName = "DefaultInvGLAcct";
                _objBLGeneral.UpdateCustom(_objPropGeneral);

            }
            if (chkSalesTax2.Checked)
            {
                _objPropGeneral.CustomLabel = "1";
                _objPropGeneral.CustomName = "SalesTax2";
                _objBLGeneral.UpdateCustom(_objPropGeneral);
            }
            else
            {
                _objPropGeneral.CustomLabel = "0";
                _objPropGeneral.CustomName = "SalesTax2";
                _objBLGeneral.UpdateCustom(_objPropGeneral);
            }
            if (chkUseTax.Checked)
            {
                _objPropGeneral.CustomLabel = "1";
                _objPropGeneral.CustomName = "UseTax";
                _objBLGeneral.UpdateCustom(_objPropGeneral);
            }
            else
            {
                _objPropGeneral.CustomLabel = "0";
                _objPropGeneral.CustomName = "UseTax";
                _objBLGeneral.UpdateCustom(_objPropGeneral);
            }
            if (chkZone.Checked)
            {
                _objPropGeneral.CustomLabel = "1";
                _objPropGeneral.CustomName = "Zone";
                _objBLGeneral.UpdateCustom(_objPropGeneral);
            }
            else
            {
                _objPropGeneral.CustomLabel = "0";
                _objPropGeneral.CustomName = "Zone";
                _objBLGeneral.UpdateCustom(_objPropGeneral);
            }

            //DashBoard Module
            if (chkDashBoard.Checked == true)
            {
                objBL_User.UpdateForAPIIntegrationEnable(Session["config"].ToString(), 1, 1);
            }
            else
            {
                objBL_User.UpdateForAPIIntegrationEnable(Session["config"].ToString(), 1, 0);
            }

            //Customers Module
            if (chkCustomers.Checked == true)
            {
                objBL_User.UpdateForAPIIntegrationEnable(Session["config"].ToString(), 2, 1);
            }
            else
            {
                objBL_User.UpdateForAPIIntegrationEnable(Session["config"].ToString(), 2, 0);
            }

            //Recurring Module
            if (chkRecurring.Checked == true)
            {
                objBL_User.UpdateForAPIIntegrationEnable(Session["config"].ToString(), 3, 1);
            }
            else
            {
                objBL_User.UpdateForAPIIntegrationEnable(Session["config"].ToString(), 3, 0);
            }

            //Schedule Module
            if (chkSchedule.Checked == true)
            {
                objBL_User.UpdateForAPIIntegrationEnable(Session["config"].ToString(), 4, 1);
            }
            else
            {
                objBL_User.UpdateForAPIIntegrationEnable(Session["config"].ToString(), 4, 0);
            }

            //Billing Module
            if (chkBilling.Checked == true)
            {
                objBL_User.UpdateForAPIIntegrationEnable(Session["config"].ToString(), 5, 1);
            }
            else
            {
                objBL_User.UpdateForAPIIntegrationEnable(Session["config"].ToString(), 5, 0);
            }

            //AP Module
            if (chkAP.Checked == true)
            {
                objBL_User.UpdateForAPIIntegrationEnable(Session["config"].ToString(), 6, 1);
            }
            else
            {
                objBL_User.UpdateForAPIIntegrationEnable(Session["config"].ToString(), 6, 0);
            }

            //Purchasing Module
            if (chkPurchasing.Checked == true)
            {
                objBL_User.UpdateForAPIIntegrationEnable(Session["config"].ToString(), 7, 1);
            }
            else
            {
                objBL_User.UpdateForAPIIntegrationEnable(Session["config"].ToString(), 7, 0);
            }

            //Sales Module
            if (chkSales.Checked == true)
            {
                objBL_User.UpdateForAPIIntegrationEnable(Session["config"].ToString(), 8, 1);
            }
            else
            {
                objBL_User.UpdateForAPIIntegrationEnable(Session["config"].ToString(), 8, 0);
            }

            //Projects Module
            if (chkProjects.Checked == true)
            {
                objBL_User.UpdateForAPIIntegrationEnable(Session["config"].ToString(), 9, 1);
            }
            else
            {
                objBL_User.UpdateForAPIIntegrationEnable(Session["config"].ToString(), 9, 0);
            }

            //Inventory Module
            if (chkInventory.Checked == true)
            {
                objBL_User.UpdateForAPIIntegrationEnable(Session["config"].ToString(), 10, 1);
            }
            else
            {
                objBL_User.UpdateForAPIIntegrationEnable(Session["config"].ToString(), 10, 0);
            }

            //Financials Module
            if (chkFinancials.Checked == true)
            {
                objBL_User.UpdateForAPIIntegrationEnable(Session["config"].ToString(), 11, 1);
            }
            else
            {
                objBL_User.UpdateForAPIIntegrationEnable(Session["config"].ToString(), 11, 0);
            }

            //Statements Module
            if (chkStatements.Checked == true)
            {
                objBL_User.UpdateForAPIIntegrationEnable(Session["config"].ToString(), 12, 1);
            }
            else
            {
                objBL_User.UpdateForAPIIntegrationEnable(Session["config"].ToString(), 12, 0);
            }

            //Programs Module
            if (chkPrograms.Checked == true)
            {
                objBL_User.UpdateForAPIIntegrationEnable(Session["config"].ToString(), 13, 1);
            }
            else
            {
                objBL_User.UpdateForAPIIntegrationEnable(Session["config"].ToString(), 13, 0);
            }

            GetAPIIntegrationEnable();
            //ScriptManager.RegisterStartupScript(Page, this.GetType(), "showFirstTab", "showFirstTab();", true);
        }
        catch (Exception ex)
        {
            //lblMsg.Text = ex.Message;
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
        //finally
        //{ Response.Redirect(Request.RawUrl); }
    }

    private void ClearControls()
    {
        ResetFormControlValues(this);
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

    protected void lnkClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("home.aspx");
    }

    protected void btnUpload_Click(object sender, EventArgs e)
    {
        if (FileUpload1.HasFile)
        {
            System.Drawing.Image imgfile = System.Drawing.Image.FromStream(FileUpload1.PostedFile.InputStream);
            Session["logo"] = null;
            Session["logo"] = ResizeImage(imgfile);
            string img = "data:image/png;base64," + Convert.ToBase64String(ResizeImage(imgfile));
            imgLogo.ImageUrl = img;
        }
    }
    protected void chkAcctIntegration_CheckedChanged(object sender, EventArgs e)
    {
        if (chkAcctIntegration.Checked == true)
        {
            txtFilePath.Enabled = true;
        }
        else
        {
            txtFilePath.Enabled = false;
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
        DataRow[] dr = dsChart.Tables[0].Select("Status=0");
        DataTable filterdt = new DataTable();
        if (dr.Length > 0)
        {
            filterdt = dr.CopyToDataTable();
        }
        DrpAllChartAcct.DataSource = filterdt;


        //DrpAllChartAcct.DataSource = dsChart.Tables[0];
        DrpAllChartAcct.DataTextField = "ChartDesc";
        DrpAllChartAcct.DataValueField = "ID";
        DrpAllChartAcct.DataBind();

        //DrpAllChartAcct.Items.Insert(0, new ListItem(":: Select ::", ""));
    }

    private void GetBillcodesforTimeSheet()
    {
        BL_Contracts objBL_Contracts = new BL_Contracts();
        Contracts objProp_Contracts = new Contracts();

        objProp_Contracts.ConnConfig = Session["config"].ToString();
        DataSet ds = new DataSet();
        ds = objBL_Contracts.GetBillcodesforTimeSheet(objProp_Contracts);

        ddlService.DataSource = ds.Tables[0];
        ddlService.DataTextField = "billcode";
        ddlService.DataValueField = "QBinvid";
        ddlService.DataBind();

        ddlService.Items.Insert(0, new ListItem(":: Select ::", ""));

        ddlServiceExpense.DataSource = ds.Tables[0];
        ddlServiceExpense.DataTextField = "billcode";
        ddlServiceExpense.DataValueField = "QBinvid";
        ddlServiceExpense.DataBind();

        ddlServiceExpense.Items.Insert(0, new ListItem(":: Select ::", ""));

        ddlServicelabor.DataSource = ds.Tables[0];
        ddlServicelabor.DataTextField = "billcode";
        ddlServicelabor.DataValueField = "QBinvid";
        ddlServicelabor.DataBind();

        ddlServicelabor.Items.Insert(0, new ListItem(":: Select ::", ""));
    }
    private void FillYearEnd()
    {
        try
        {
            var _varMonth = Enum.GetValues(typeof(CommonHelper.Months));
            var values = Enum.GetValues(typeof(CommonHelper.Months)).Cast<CommonHelper.Months>();

            ddlYearEnd.Items.Add(new ListItem(":: Select ::"));
            int i = 0;
            foreach (var v in values)
            {
                ddlYearEnd.Items.Add(new ListItem(v.Description(), i.ToString()));
                i++;
            }

            //ddlYearEnd.Items.Add(new ListItem(":: Select ::"));
            //ddlYearEnd.AppendDataBoundItems = true;
            //ddlYearEnd.DataSource = _varMonth;
            //ddlYearEnd.DataBind();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    private void FillAdminUsers()
    {
        try
        {
            User objPropUser = new User();
            //objPropUser.DBName = txtDB.Text.Trim();
            objPropUser.ConnConfig = Session["config"].ToString();

            DataSet ds = new DataSet();
            ds = objBL_User.GetUsersForResetPwAdmin(objPropUser);

            ddlAdminUser.DataSource = ds.Tables[0];
            ddlAdminUser.DataTextField = "fuser";
            ddlAdminUser.DataValueField = "userid";
            ddlAdminUser.DataBind();

            ddlAdminUser.Items.Insert(0, new ListItem(":: Select ::", ""));
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    //Start-- API Changes : Juily:04/06/2020 --//
    protected void chkDashBoard_CheckedChanged(object sender, EventArgs e)
    {
        if (chkDashBoard.Checked == true)
        {
            chkDashBoard.Checked = true;
        }
        else
        {
            chkDashBoard.Checked = false;
        }
        //GetAPIIntegrationEnable();
    }
    protected void chkCustomers_CheckedChanged(object sender, EventArgs e)
    {
        if (chkCustomers.Checked == true)
        {
            chkCustomers.Checked = true;
        }
        else
        {
            chkCustomers.Checked = false;
        }
        //GetAPIIntegrationEnable();
    }
    protected void chkRecurring_CheckedChanged(object sender, EventArgs e)
    {
        if (chkRecurring.Checked == true)
        {
            chkRecurring.Checked = true;
        }
        else
        {
            chkRecurring.Checked = false;
        }
        //GetAPIIntegrationEnable();
    }
    protected void chkSchedule_CheckedChanged(object sender, EventArgs e)
    {
        if (chkSchedule.Checked == true)
        {
            chkSchedule.Checked = true;
        }
        else
        {
            chkSchedule.Checked = false;
        }
        //GetAPIIntegrationEnable();
    }
    protected void chkBilling_CheckedChanged(object sender, EventArgs e)
    {
        if (chkBilling.Checked == true)
        {
            chkBilling.Checked = true;
        }
        else
        {
            chkBilling.Checked = false;
        }
        //GetAPIIntegrationEnable();
    }
    protected void chkAP_CheckedChanged(object sender, EventArgs e)
    {
        if (chkAP.Checked == true)
        {
            chkAP.Checked = true;
        }
        else
        {
            chkAP.Checked = false;
        }
        //GetAPIIntegrationEnable();
    }
    protected void chkPurchasing_CheckedChanged(object sender, EventArgs e)
    {
        if (chkPurchasing.Checked == true)
        {
            chkPurchasing.Checked = true;
        }
        else
        {
            chkPurchasing.Checked = false;
        }
        // GetAPIIntegrationEnable();
    }
    protected void chkSales_CheckedChanged(object sender, EventArgs e)
    {
        if (chkSales.Checked == true)
        {
            chkSales.Checked = true;
        }
        else
        {
            chkSales.Checked = false;
        }
        //GetAPIIntegrationEnable();
    }
    protected void chkProjects_CheckedChanged(object sender, EventArgs e)
    {
        if (chkProjects.Checked == true)
        {
            chkProjects.Checked = true;
        }
        else
        {
            chkProjects.Checked = false;
        }
        //GetAPIIntegrationEnable();
    }
    protected void chkInventory_CheckedChanged(object sender, EventArgs e)
    {
        if (chkInventory.Checked == true)
        {
            chkInventory.Checked = true;
        }
        else
        {
            chkInventory.Checked = false;
        }
        //GetAPIIntegrationEnable();
    }
    protected void chkFinancials_CheckedChanged(object sender, EventArgs e)
    {
        if (chkFinancials.Checked == true)
        {
            chkFinancials.Checked = true;
        }
        else
        {
            chkFinancials.Checked = false;
        }
        //GetAPIIntegrationEnable();
    }
    protected void chkStatements_CheckedChanged(object sender, EventArgs e)
    {
        if (chkStatements.Checked == true)
        {
            chkStatements.Checked = true;
        }
        else
        {
            chkStatements.Checked = false;
        }
        //GetAPIIntegrationEnable();
    }
    protected void chkPrograms_CheckedChanged(object sender, EventArgs e)
    {
        if (chkPrograms.Checked == true)
        {
            chkPrograms.Checked = true;
        }
        else
        {
            chkPrograms.Checked = false;
        }
        //GetAPIIntegrationEnable();
    }

    #region Comment
    //public void GetAPIIntegrationEnable()
    //{
    //    DataSet ds = new DataSet();

    //    ds = objBL_User.GetAPIIntegrationEnable(Session["config"].ToString());

    //    if (ds.Tables[0].Rows[0]["ModuleName"].ToString() == "DashBoard")
    //    {
    //        if (ds.Tables[0].Rows[0]["Integration"].ToString() == "1")
    //        {
    //            chkDashBoard.Checked = true;
    //            Session["DashBoardAPIEnable"] = "YES";
    //        }
    //        else
    //        {
    //            chkDashBoard.Checked = false;
    //            Session["DashBoardAPIEnable"] = "NO";
    //        }
    //    }

    //    if (ds.Tables[0].Rows[1]["ModuleName"].ToString() == "Customers")
    //    {
    //        if (ds.Tables[0].Rows[1]["Integration"].ToString() == "1")
    //        {
    //            chkCustomers.Checked = true;
    //            Session["CustomersAPIEnable"] = "YES";
    //        }
    //        else
    //        {
    //            chkCustomers.Checked = false;
    //            Session["CustomersAPIEnable"] = "NO";
    //        }
    //    }
    //    if (ds.Tables[0].Rows[2]["ModuleName"].ToString() == "Recurring")
    //    {
    //        if (ds.Tables[0].Rows[2]["Integration"].ToString() == "1")
    //        {
    //            chkRecurring.Checked = true;
    //            Session["RecurringAPIEnable"] = "YES";
    //        }
    //        else
    //        {
    //            chkRecurring.Checked = false;
    //            Session["RecurringAPIEnable"] = "NO";
    //        }
    //    }
    //    if (ds.Tables[0].Rows[3]["ModuleName"].ToString() == "Schedule")
    //    {
    //        if (ds.Tables[0].Rows[3]["Integration"].ToString() == "1")
    //        {
    //            chkSchedule.Checked = true;
    //            Session["ScheduleAPIEnable"] = "YES";
    //        }
    //        else
    //        {
    //            chkSchedule.Checked = false;
    //            Session["ScheduleAPIEnable"] = "NO";
    //        }
    //    }
    //    if (ds.Tables[0].Rows[4]["ModuleName"].ToString() == "Billing")
    //    {
    //        if (ds.Tables[0].Rows[4]["Integration"].ToString() == "1")
    //        {
    //            chkBilling.Checked = true;
    //            Session["BillingAPIEnable"] = "YES";
    //        }
    //        else
    //        {
    //            chkBilling.Checked = false;
    //            Session["BillingAPIEnable"] = "NO";
    //        }
    //    }
    //    if (ds.Tables[0].Rows[5]["ModuleName"].ToString() == "AP")
    //    {
    //        if (ds.Tables[0].Rows[5]["Integration"].ToString() == "1")
    //        {
    //            chkAP.Checked = true;
    //            Session["APAPIEnable"] = "YES";
    //        }
    //        else
    //        {
    //            chkAP.Checked = false;
    //            Session["APAPIEnable"] = "NO";
    //        }
    //    }
    //    if (ds.Tables[0].Rows[6]["ModuleName"].ToString() == "Purchasing")
    //    {
    //        if (ds.Tables[0].Rows[6]["Integration"].ToString() == "1")
    //        {
    //            chkPurchasing.Checked = true;
    //            Session["PurchasingAPIEnable"] = "YES";
    //        }
    //        else
    //        {
    //            chkPurchasing.Checked = false;
    //            Session["PurchasingAPIEnable"] = "NO";
    //        }
    //    }
    //    if (ds.Tables[0].Rows[7]["ModuleName"].ToString() == "Sales")
    //    {
    //        if (ds.Tables[0].Rows[7]["Integration"].ToString() == "1")
    //        {
    //            chkSales.Checked = true;
    //            Session["SalesAPIEnable"] = "YES";
    //        }
    //        else
    //        {
    //            chkSales.Checked = false;
    //            Session["SalesAPIEnable"] = "NO";
    //        }
    //    }
    //    if (ds.Tables[0].Rows[8]["ModuleName"].ToString() == "Projects")
    //    {
    //        if (ds.Tables[0].Rows[8]["Integration"].ToString() == "1")
    //        {
    //            chkProjects.Checked = true;
    //            Session["ProjectsAPIEnable"] = "YES";
    //        }
    //        else
    //        {
    //            chkProjects.Checked = false;
    //            Session["ProjectsAPIEnable"] = "NO";
    //        }
    //    }
    //    if (ds.Tables[0].Rows[9]["ModuleName"].ToString() == "Inventory")
    //    {
    //        if (ds.Tables[0].Rows[9]["Integration"].ToString() == "1")
    //        {
    //            chkInventory.Checked = true;
    //            Session["InventoryAPIEnable"] = "YES";
    //        }
    //        else
    //        {
    //            chkInventory.Checked = false;
    //            Session["InventoryAPIEnable"] = "NO";
    //        }
    //    }
    //    if (ds.Tables[0].Rows[10]["ModuleName"].ToString() == "Financials")
    //    {
    //        if (ds.Tables[0].Rows[10]["Integration"].ToString() == "1")
    //        {
    //            chkFinancials.Checked = true;
    //            Session["FinancialsAPIEnable"] = "YES";
    //        }
    //        else
    //        {
    //            chkFinancials.Checked = false;
    //            Session["FinancialsAPIEnable"] = "NO";
    //        }
    //    }
    //    if (ds.Tables[0].Rows[11]["ModuleName"].ToString() == "Statements")
    //    {
    //        if (ds.Tables[0].Rows[11]["Integration"].ToString() == "1")
    //        {
    //            chkStatements.Checked = true;
    //            Session["StatementsAPIEnable"] = "YES";
    //        }
    //        else
    //        {
    //            chkStatements.Checked = false;
    //            Session["StatementsAPIEnable"] = "NO";
    //        }
    //    }
    //    if (ds.Tables[0].Rows[12]["ModuleName"].ToString() == "Programs")
    //    {
    //        if (ds.Tables[0].Rows[12]["Integration"].ToString() == "1")
    //        {
    //            chkPrograms.Checked = true;
    //            Session["ProgramsAPIEnable"] = "YES";
    //        }
    //        else
    //        {
    //            chkPrograms.Checked = false;
    //            Session["ProgramsAPIEnable"] = "NO";
    //        }
    //    }
    //}
    #endregion
    public void GetAPIIntegrationEnable()
    {
        DataSet ds = new DataSet();

        ds = objBL_User.GetAPIIntegrationEnable(Session["config"].ToString());

        APIIntegrationModel _objAPIIntegration = new APIIntegrationModel();

        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            if (dr["ModuleName"].ToString() == "DashBoard")
            {
                _objAPIIntegration.IsAPIIntegrationForDashBoardModule = Convert.ToBoolean(dr["Integration"]);

                if (Convert.ToBoolean(dr["Integration"]) == true)
                {
                    chkDashBoard.Checked = true;
                }
                else
                {
                    chkDashBoard.Checked = false;
                }
            }
            if (dr["ModuleName"].ToString() == "Customers")
            {
                _objAPIIntegration.IsAPIIntegrationForCustomersModule = Convert.ToBoolean(dr["Integration"]);

                if (Convert.ToBoolean(dr["Integration"]) == true)
                {
                    chkCustomers.Checked = true;
                }
                else
                {
                    chkCustomers.Checked = false;
                }
            }
            if (dr["ModuleName"].ToString() == "Recurring")
            {
                _objAPIIntegration.IsAPIIntegrationForRecurringModule = Convert.ToBoolean(dr["Integration"]);

                if (Convert.ToBoolean(dr["Integration"]) == true)
                {
                    chkRecurring.Checked = true;
                }
                else
                {
                    chkRecurring.Checked = false;
                }
            }
            if (dr["ModuleName"].ToString() == "Schedule")
            {
                _objAPIIntegration.IsAPIIntegrationForScheduleModule = Convert.ToBoolean(dr["Integration"]);

                if (Convert.ToBoolean(dr["Integration"]) == true)
                {
                    chkSchedule.Checked = true;
                }
                else
                {
                    chkSchedule.Checked = false;
                }
            }
            if (dr["ModuleName"].ToString() == "Billing")
            {
                _objAPIIntegration.IsAPIIntegrationForBillingModule = Convert.ToBoolean(dr["Integration"]);

                if (Convert.ToBoolean(dr["Integration"]) == true)
                {
                    chkBilling.Checked = true;
                }
                else
                {
                    chkBilling.Checked = false;
                }
            }
            if (dr["ModuleName"].ToString() == "AP")
            {
                _objAPIIntegration.IsAPIIntegrationForAPModule = Convert.ToBoolean(dr["Integration"]);

                if (Convert.ToBoolean(dr["Integration"]) == true)
                {
                    chkAP.Checked = true;
                }
                else
                {
                    chkAP.Checked = false;
                }
            }
            if (dr["ModuleName"].ToString() == "Purchasing")
            {
                _objAPIIntegration.IsAPIIntegrationForPurchasingModule = Convert.ToBoolean(dr["Integration"]);

                if (Convert.ToBoolean(dr["Integration"]) == true)
                {
                    chkPurchasing.Checked = true;
                }
                else
                {
                    chkPurchasing.Checked = false;
                }
            }
            if (dr["ModuleName"].ToString() == "Sales")
            {
                _objAPIIntegration.IsAPIIntegrationForSalesModule = Convert.ToBoolean(dr["Integration"]);

                if (Convert.ToBoolean(dr["Integration"]) == true)
                {
                    chkSales.Checked = true;
                }
                else
                {
                    chkSales.Checked = false;
                }
            }
            if (dr["ModuleName"].ToString() == "Projects")
            {
                _objAPIIntegration.IsAPIIntegrationForProjectsModule = Convert.ToBoolean(dr["Integration"]);

                if (Convert.ToBoolean(dr["Integration"]) == true)
                {
                    chkProjects.Checked = true;
                }
                else
                {
                    chkProjects.Checked = false;
                }
            }
            if (dr["ModuleName"].ToString() == "Inventory")
            {
                _objAPIIntegration.IsAPIIntegrationForInventoryModule = Convert.ToBoolean(dr["Integration"]);

                if (Convert.ToBoolean(dr["Integration"]) == true)
                {
                    chkInventory.Checked = true;
                }
                else
                {
                    chkInventory.Checked = false;
                }
            }
            if (dr["ModuleName"].ToString() == "Financials")
            {
                _objAPIIntegration.IsAPIIntegrationForFinancialsModule = Convert.ToBoolean(dr["Integration"]);

                if (Convert.ToBoolean(dr["Integration"]) == true)
                {
                    chkFinancials.Checked = true;
                }
                else
                {
                    chkFinancials.Checked = false;
                }
            }
            if (dr["ModuleName"].ToString() == "Statements")
            {
                _objAPIIntegration.IsAPIIntegrationForStatementsModule = Convert.ToBoolean(dr["Integration"]);

                if (Convert.ToBoolean(dr["Integration"]) == true)
                {
                    chkStatements.Checked = true;
                }
                else
                {
                    chkStatements.Checked = false;
                }
            }
            if (dr["ModuleName"].ToString() == "Programs")
            {
                _objAPIIntegration.IsAPIIntegrationForProgramsModule = Convert.ToBoolean(dr["Integration"]);

                if (Convert.ToBoolean(dr["Integration"]) == true)
                {
                    chkPrograms.Checked = true;
                }
                else
                {
                    chkPrograms.Checked = false;
                }
            }
        }

        Session["IsAPIIntegration"] = _objAPIIntegration;
    }

    //End-- API Changes : Juily:04/06/2020 --//

    //Country Dropdown Select Change Event
    protected void OnSelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlCountry.SelectedValue == 1.ToString())
        {
            divProvincialID.Visible = true;
            divStateID.Visible = false;
            divFederalID.Visible = false;
        }
        else
        {
            divProvincialID.Visible = false;
            divStateID.Visible = true;
            divFederalID.Visible = true;
        }
    }
}
