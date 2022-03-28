using System;
using System.Data;
using System.Web.UI.WebControls;

public partial class uc_gvChecklist : System.Web.UI.UserControl
{
    public delegate void RowCommand(object sender, GridViewCommandEventArgs e);
    public event RowCommand GridRowCommand;
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            CreateDepartment();
        }
    }
    public DataControlFieldCollection Columns
    {
        get { return gvDepartment.Columns;  }
    }
    public void BindData(DataTable dt)
    {
        gvDepartment.DataSource = dt;
        gvDepartment.DataBind();
    }
    //public HiddenField _hdnDepJSON
    //{
    //    get { return hdnDepJSON; }
    //}
    protected void OnRowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (GridRowCommand != null)
            GridRowCommand(sender, e);
    }
    protected void gvDepartment_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        GridView GvDep = (GridView)sender;
        OnRowCommand(GvDep,e);
    }
    //private DataTable GetDepartment()
    //{
    //    DataTable dt = new DataTable();
    //    dt.Columns.Add("line", typeof(int));
    //    dt.Columns.Add("fdesc", typeof(string));
    //    dt.Columns.Add("Format", typeof(string));
    //    dt.Columns.Add("RefFormat", typeof(string));
    //    //string strItems = hdnDepJSON.Value.Trim();
    //    try
    //    {
    //        if (strItems != string.Empty)
    //        {
    //            JavaScriptSerializer sr = new JavaScriptSerializer();
    //            List<Dictionary<object, object>> objEstimateItemData = new List<Dictionary<object, object>>();
    //            objEstimateItemData = sr.Deserialize<List<Dictionary<object, object>>>(strItems);
    //            int i = 0;
    //            foreach (Dictionary<object, object> dict in objEstimateItemData)
    //            {
    //                if (i < 10)
    //                {
    //                    i++;
    //                    DataRow dr = dt.NewRow();
    //                    dr["Line"] = i;
    //                    if (dict["txtDescription"].ToString().Trim() == string.Empty)
    //                    {
    //                        dt.Rows.Add(dr);
    //                        continue;
    //                    }

    //                    dr["fdesc"] = dict["txtDescription"];
    //                    if (dict["ddlControl"].ToString().Trim() != string.Empty)
    //                    {
    //                        dr["Format"] = Convert.ToInt32(dict["ddlControl"]);
    //                    }
    //                    if (dict["ddlRefControl"].ToString().Trim() != string.Empty)
    //                    {
    //                        dr["RefFormat"] = Convert.ToInt32(dict["ddlRefControl"]);
    //                    }

    //                    dt.Rows.Add(dr);
    //                }
    //                else
    //                {
    //                    break;
    //                }
    //            }
    //        }
    //        ViewState["Dep"] = dt;
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //    return dt;
    //}
    
    private void CreateDepartment()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("line", typeof(int));
        dt.Columns.Add("fdesc", typeof(string));
        dt.Columns.Add("Format", typeof(string));
        dt.Columns.Add("RefFormat", typeof(string));

        for (int i = 1; i <= 10; i++)
        {
            DataRow dr = dt.NewRow();
            dr["line"] = i;
            dt.Rows.Add(dr);
        }
        gvDepartment.DataSource = dt;
        gvDepartment.DataBind();
        ViewState["Dep"] = dt;
    }
}