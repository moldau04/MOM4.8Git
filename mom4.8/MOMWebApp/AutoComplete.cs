using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using System.Data;
using BusinessEntity;
using BusinessLayer;

/// <summary>
/// Summary description for AutoComplete
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class AutoComplete : System.Web.Services.WebService
{
    User objPropUser = new User();
    BL_User objBL_User = new BL_User();

    public AutoComplete()
    {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    public string HelloWorld()
    {
        return "Hello World";
    }

    [System.Web.Services.WebMethod(EnableSession = true)]
    public string[] GetCustomers(string prefixText, int count)
    {
        DataSet ds = new DataSet();
        objPropUser.DBName = HttpContext.Current.Session["dbname"].ToString();
        objPropUser.ConnConfig = Session["config"].ToString();
        ds = objBL_User.getCustomers(objPropUser);

        DataTable dt = ds.Tables[0];

        List<string> txtItems = new List<string>();
        String dbValues;

        foreach (DataRow row in dt.Rows)
        {
            dbValues = row["Name"].ToString();
            dbValues = dbValues.ToLower();
            txtItems.Add(dbValues);
        }

        return txtItems.ToArray();
    } 

}

