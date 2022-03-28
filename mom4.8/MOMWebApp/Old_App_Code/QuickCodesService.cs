using System.Web;
using System.Web.Services;
using System.Web.Script.Services;
using BusinessLayer;
using BusinessEntity;
using System.Web.Script.Serialization;

/// <summary>
/// Summary description for QuickCodesService
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class QuickCodesService : System.Web.Services.WebService
{

    public QuickCodesService()
    {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod(EnableSession = true)]    
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string QuickCodes(string code,string con)
    {
        General objPropGeneral = new General();
        BL_General objBL_General = new BL_General();

        string codeText=string.Empty ;
        objPropGeneral.Code = code;
        objPropGeneral.ConnConfig = HttpContext.Current.Session["config"].ToString(); 
        codeText = objBL_General.getCode(objPropGeneral);
        JavaScriptSerializer sr = new JavaScriptSerializer();
        string str = sr.Serialize(codeText);
        return str;
    }

}

