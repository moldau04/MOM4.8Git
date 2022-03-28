using System;
using System.Web.UI;
using BusinessEntity;
using BusinessLayer;
public partial class LocationReportPreview : System.Web.UI.Page
{
    CustomerReport objCustReport = new CustomerReport();
    BL_ReportsData objBL_ReportsData = new BL_ReportsData();
    BL_User objBL_User = new BL_User();
    BusinessEntity.User objProp_User = new BusinessEntity.User();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                ShowFile(Session["FilePath"].ToString());
            }
            catch (Exception exp)
            {
                ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: '" + exp.Message.ToString() + "',  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
            }
        }
    }
    public void ShowFile(string filename)
    {
        //Clears all content output from Buffer Stream
        Response.ClearContent();
        //Clears all headers from Buffer Stream
        Response.ClearHeaders();
        //Adds an HTTP header to the output stream
        Response.AddHeader("Content-Disposition", "inline;filename=" + filename);
        //Gets or Sets the HTTP MIME type of the output stream
        if (filename.Contains(".xls"))
        {
            Response.ContentType = "application/xls";
        }
        else
        {
            Response.ContentType = "application/pdf";
        }
        //Writes the content of the specified file directory to an HTTP response output stream as a file block
        Response.WriteFile(filename);
        //sends all currently buffered output to the client
        Response.Flush();
        //Clears all content output from Buffer Stream
        Response.Clear();
    }

}
