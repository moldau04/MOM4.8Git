<%@ WebHandler Language="C#" Class="TestHandler" %>

using System;
using System.Web;
using System.IO;
using System.Web.Script.Serialization;
using BusinessLayer;
using BusinessEntity;
using System.Data;
using System.Collections.Generic;
using System.Web.SessionState;

public class deserialized
{
    public string prefixText { get; set; }
    public string con { get; set; }
    public object custID { get; set; }
}

public class TestHandler : IHttpHandler, IReadOnlySessionState
{
    
    public void ProcessRequest (HttpContext context) {

        StreamReader reader = new StreamReader(context.Request.InputStream, context.Request.ContentEncoding);
        string input = reader.ReadToEnd();

        var deserializer = new JavaScriptSerializer();

        deserialized desString = deserializer.Deserialize<deserialized>(input);
        
        User objPropUser = new User();
        BL_User objBL_User = new BL_User();
        GeneralFunctions objGeneral = new GeneralFunctions();

        string str;
        JavaScriptSerializer sr = new JavaScriptSerializer();
        List<Dictionary<object, object>> dictListEval = new List<Dictionary<object, object>>();

        DataSet ds = new DataSet();
        objPropUser.SearchValue = desString.prefixText;
        objPropUser.ConnConfig = context.Session["config"].ToString();
        ds = objBL_User.getCustomerAutojquery(objPropUser);
        dictListEval = objGeneral.RowsToDictionary(ds.Tables[0]);
        str = sr.Serialize(dictListEval);
                
        context.Response.ContentType = "text/plain";
        context.Response.StatusCode = 200;
        context.Response.Write(str);
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}