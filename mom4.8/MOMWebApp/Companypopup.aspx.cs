using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLayer;
using System.Text;
using System.Data;
using Telerik.Web.UI;
public partial class Companypopup : System.Web.UI.Page
{
    BusinessEntity.CompanyOffice objCompany = new BusinessEntity.CompanyOffice();
    BL_Company objBL_Company = new BL_Company();
    private static int intCount = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userid"] == null)
        {
            Response.Redirect("login.aspx");
        }
    }

    private void FillCompany()
    {
        objCompany.UserID = Convert.ToInt32(Session["UserID"].ToString());
        objCompany.DBName = Session["dbname"].ToString();
        objCompany.ConnConfig = Session["config"].ToString();
        DataSet ds = new DataSet();
        ds = objBL_Company.getCompanyByUserID(objCompany);
        if (ds.Tables[0].Rows.Count > 0)
        {
            RadGrid_Company.VirtualItemCount = ds.Tables[0].Rows.Count;
            RadGrid_Company.DataSource = ds.Tables[0];
            Session["searchdata"] = ds.Tables[0];
        }
    }
    bool isGroupingCompany = false;
    public bool ShouldApplySortFilterCompany()
    {
        return RadGrid_Company.MasterTableView.FilterExpression != "" ||
            (RadGrid_Company.MasterTableView.GroupByExpressions.Count > 0 || isGroupingCompany) ||
            RadGrid_Company.MasterTableView.SortExpressions.Count > 0;
    }
    protected void RadGrid_Company_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        RadGrid_Company.AllowCustomPaging = !ShouldApplySortFilterCompany();
        FillCompany();
    }

    private void GetSelectedRecord()
    {
        foreach (GridDataItem row in RadGrid_Company.Items)
        {
            RadioButton rb = (RadioButton)row.FindControl("rbDefaultCompany");
            if (rb != null)
            {
                if (rb.Checked)
                {
                    HiddenField hf = (HiddenField)row.FindControl("hdnDefaultCompanyID");
                    if (hf != null)
                    {
                        ViewState["SelectedContact"] = hf.Value;
                    }

                    break;
                }
            }
        }
    }
    private void SetSelectedRecord()
    {
        foreach (GridDataItem row in RadGrid_Company.Items)
        {
            RadioButton rb = (RadioButton)row.FindControl("rbDefaultCompany");
            CheckBox chk = (CheckBox)row.FindControl("chkBranchSelect");
            if (rb != null)
            {
                HiddenField hf = (HiddenField)row.FindControl("hdnDefaultCompanyID");
                if (hf != null && ViewState["SelectedContact"] != null)
                {
                    if (hf.Value.Equals(ViewState["SelectedContact"].ToString()))
                    {
                        rb.Checked = true;
                       // chk.Checked = true;
                        break;
                    }
                }
            }
        }
    }
    private void GetDefaultCompanySelected()
    {
        Int32 num = 0;
        foreach (GridDataItem row in RadGrid_Company.Items)
        {
            CheckBox chk = (CheckBox)row.FindControl("chkBranchSelect");
            if (chk.Checked == true)
            {
                num = num + 1;
            }
        }
        foreach (GridDataItem row in RadGrid_Company.Items)
        {
            RadioButton rb = (RadioButton)row.FindControl("rbDefaultCompany");
            CheckBox chk = (CheckBox)row.FindControl("chkBranchSelect");
            if (rb != null)
            {
                HiddenField hf = (HiddenField)row.FindControl("hdnDefaultCompanyID");
                if (hf != null && Session["DefaultCompID"] != null)
                {
                    if (hf.Value.Equals(Session["DefaultCompID"].ToString()))
                    {
                        rb.Checked = true;
                        if (intCount == 0)
                            chk.Checked = false;
                        break;
                    }
                }
            }
        }

        lblRecordCount.Text = Convert.ToString(num) + " Company(s) Selected.";
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        GetSelectedRecord();
        SetSelectedRecord();
        Session["CmpChkDefault"] = "2";
        Submit();

        Page page = HttpContext.Current.Handler as Page;
        ScriptManager.RegisterStartupScript(page, page.GetType(), "err_msg", "RefreshParentPage();", true);

    }
    private void Submit()
    {
        intCount = 0;
        int UserId = Convert.ToInt32(Session["UserID"].ToString());
        int intDefCompanyID = Convert.ToInt32(ViewState["SelectedContact"]);
        Session["NewDefCompanyID"] = intDefCompanyID;
        objCompany.DBName = Session["dbname"].ToString();
        objCompany.ConnConfig = Session["config"].ToString();
        StringBuilder _selectedCompany = new StringBuilder();
        StringBuilder _selectedCompanyIDs = new StringBuilder();
        try
        {
            foreach (GridDataItem row in RadGrid_Company.Items)
            {
                Label lblUCoID = (Label)row.FindControl("lblID");
                int UserCompanyId = Convert.ToInt32(lblUCoID.Text);
                objCompany.ID = UserCompanyId;
                if ((row.FindControl("chkBranchSelect") as CheckBox).Checked)
                {
                    intCount++;
                    Label lblCompName = (Label)row.FindControl("lblName");
                    objCompany.IsSel = true;
                    Session["CmpChkDefault"] = "1";
                    _selectedCompany.Append(lblCompName.Text + " , ");
                    _selectedCompanyIDs.Append(UserCompanyId + ",");
                    if (intCount == 1)
                    {
                        Session["chkCompanyName"] = lblCompName.Text;
                    }
                }
                else
                {
                    objCompany.IsSel = false;
                }
                objBL_Company.UserCompanyAccess(objCompany);
            }
            foreach (GridDataItem row in RadGrid_Company.Items)
            {
                Label lblCompName = (Label)row.FindControl("lblName");
                RadioButton rb = (RadioButton)row.FindControl("rbDefaultCompany");
                HiddenField hfCompanyID = (HiddenField)row.FindControl("hdnDefaultCompanyID");
                Label lblUCoID = (Label)row.FindControl("lblID");
                if (rb.Checked == true)
                {
                    objCompany.UserID = UserId;
                    objCompany.CompanyID = Convert.ToInt32(hfCompanyID.Value);
                    objBL_Company.UpdateUserCompany(objCompany);
                    if (intCount == 0)
                    {
                        objCompany.ID = Convert.ToInt32(lblUCoID.Text);
                        objCompany.IsSel = true;
                        objBL_Company.UserCompanyAccess(objCompany);
                        Session["chkCompanyName"] = lblCompName.Text;                      
                    }
                }
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
        }
        finally
        {
            if (_selectedCompany.Length > 1)
            {
                _selectedCompany.Remove(_selectedCompany.Length - 2, 2);
                Session["CompList"] = Convert.ToString(_selectedCompany);
                Session["CompIDs"] = Convert.ToString(_selectedCompanyIDs);
            }
            FillCompany();
        }
    }

    protected void RadGrid_Company_PreRender(object sender, EventArgs e)
    {
        GetDefaultCompanySelected();
    }
}