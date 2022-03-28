using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using BusinessLayer;
using BusinessEntity;
using System.Data;
using System.Web.Script.Services;
using System.Web.Script.Serialization;
using AjaxControlToolkit;
using System.Linq;
using BusinessEntity.Recurring;
using System.Text;

/// <summary>
/// Summary description for CustomerAuto
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class CustomerAuto : System.Web.Services.WebService
{
    User objPropUser = new User();
    BL_User objBL_User = new BL_User();

    BL_MapData objBL_MapData = new BL_MapData();
    MapData objMapData = new MapData();

    GeneralFunctions objGeneral = new GeneralFunctions();

    public CustomerAuto()
    {
        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }
    /// <summary>
    /// GetCustomer
    /// </summary>
    /// <param name="prefixText"></param>
    /// <param name="con"></param>
    /// <returns></returns>
    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string GetCustomer(string prefixText, string con)
    {
        User objPropUser = new User();
        BL_User objBL_User = new BL_User();

        string str;
        JavaScriptSerializer sr = new JavaScriptSerializer();
        List<Dictionary<object, object>> dictListEval = new List<Dictionary<object, object>>();

        DataSet ds = new DataSet();
        objPropUser.SearchValue = prefixText;
        objPropUser.ConnConfig = HttpContext.Current.Session["config"].ToString();
        #region Company Check
        objPropUser.UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"].ToString());
        if (Convert.ToString(HttpContext.Current.Session["CmpChkDefault"]) == "1")
        {
            objPropUser.EN = 1;
        }
        else
        {
            objPropUser.EN = 0;
        }
        #endregion
        ds = objBL_User.getCustomerAutojquery(objPropUser, new GeneralFunctions().GetSalesAsigned());
        dictListEval = objGeneral.RowsToDictionary(ds.Tables[0]);
        str = sr.Serialize(dictListEval);
        return str;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string GetCustomerWithInactive(string prefixText, string con)
    {
        User objPropUser = new User();
        BL_User objBL_User = new BL_User();

        string str;
        JavaScriptSerializer sr = new JavaScriptSerializer();
        List<Dictionary<object, object>> dictListEval = new List<Dictionary<object, object>>();

        DataSet ds = new DataSet();
        objPropUser.SearchValue = prefixText;
        objPropUser.ConnConfig = HttpContext.Current.Session["config"].ToString();
        #region Company Check
        objPropUser.UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"].ToString());
        if (Convert.ToString(HttpContext.Current.Session["CmpChkDefault"]) == "1")
        {
            objPropUser.EN = 1;
        }
        else
        {
            objPropUser.EN = 0;
        }
        #endregion
        ds = objBL_User.getCustomerWithInactive(objPropUser, new GeneralFunctions().GetSalesAsigned());
        dictListEval = objGeneral.RowsToDictionary(ds.Tables[0]);
        str = sr.Serialize(dictListEval);
        return str;
    }


    /// <summary>
    /// GetCustomerProspect
    /// </summary>
    /// <param name="prefixText"></param>
    /// <param name="con"></param>
    /// <returns></returns>
    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string GetCustomerProspect(string prefixText, string con)
    {
        User objPropUser = new User();
        BL_User objBL_User = new BL_User();

        string str;
        JavaScriptSerializer sr = new JavaScriptSerializer();
        List<Dictionary<object, object>> dictListEval = new List<Dictionary<object, object>>();

        DataSet ds = new DataSet();
        objPropUser.SearchValue = prefixText;
        objPropUser.ConnConfig = HttpContext.Current.Session["config"].ToString();
        #region Company Check
        objPropUser.UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"].ToString());
        if (Convert.ToString(HttpContext.Current.Session["CmpChkDefault"]) == "1")
        {
            objPropUser.EN = 1;
        }
        else
        {
            objPropUser.EN = 0;
        }
        #endregion
        ds = objBL_User.getCustomerProspectAutojquery(objPropUser, new GeneralFunctions().GetSalesAsigned());
        dictListEval = objGeneral.RowsToDictionary(ds.Tables[0]);
        str = sr.Serialize(dictListEval);
        return str;
    }


    /// <summary>
    /// GetLocation
    /// </summary>
    /// <param name="prefixText"></param>
    /// <param name="con"></param>
    /// <param name="custID"></param>
    /// <returns></returns>
    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string GetLocation(string prefixText, string con, int custID)
    {
        User objPropUser = new User();
        BL_User objBL_User = new BL_User();

        string str;
        JavaScriptSerializer sr = new JavaScriptSerializer();
        List<Dictionary<object, object>> dictListEval = new List<Dictionary<object, object>>();

        DataSet ds = new DataSet();
        objPropUser.SearchValue = prefixText;
        objPropUser.ConnConfig = HttpContext.Current.Session["config"].ToString(); ;
        objPropUser.CustomerID = custID;
        #region Company Check
        objPropUser.UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"].ToString());
        if (Convert.ToString(HttpContext.Current.Session["CmpChkDefault"]) == "1")
        {
            objPropUser.EN = 1;
        }
        else
        {
            objPropUser.EN = 0;
        }
        #endregion
        ds = objBL_User.getLocationAutojquery(objPropUser, new GeneralFunctions().GetSalesAsigned());
        dictListEval = objGeneral.RowsToDictionary(ds.Tables[0]);
        str = sr.Serialize(dictListEval);
        return str;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string GetLocationWithInactive(string prefixText, string con, int custID)
    {
        User objPropUser = new User();
        BL_User objBL_User = new BL_User();

        string str;
        JavaScriptSerializer sr = new JavaScriptSerializer();
        List<Dictionary<object, object>> dictListEval = new List<Dictionary<object, object>>();

        DataSet ds = new DataSet();
        objPropUser.SearchValue = prefixText;
        objPropUser.ConnConfig = HttpContext.Current.Session["config"].ToString(); ;
        objPropUser.CustomerID = custID;
        #region Company Check
        objPropUser.UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"].ToString());
        if (Convert.ToString(HttpContext.Current.Session["CmpChkDefault"]) == "1")
        {
            objPropUser.EN = 1;
        }
        else
        {
            objPropUser.EN = 0;
        }
        #endregion
        ds = objBL_User.getLocationWithInactive(objPropUser, new GeneralFunctions().GetSalesAsigned());
        dictListEval = objGeneral.RowsToDictionary(ds.Tables[0]);
        str = sr.Serialize(dictListEval);
        return str;
    }

    /// <summary>
    /// GetLocationProspect
    /// </summary>
    /// <param name="prefixText"></param>
    /// <param name="con"></param>
    /// <param name="custID"></param>
    /// <returns></returns>
    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string GetLocationProspect(string prefixText, string con, int custID, int isProspect)
    {
        User objPropUser = new User();
        BL_User objBL_User = new BL_User();

        string str;
        JavaScriptSerializer sr = new JavaScriptSerializer();
        List<Dictionary<object, object>> dictListEval = new List<Dictionary<object, object>>();

        DataSet ds = new DataSet();
        objPropUser.SearchValue = prefixText;
        objPropUser.ConnConfig = HttpContext.Current.Session["config"].ToString(); ;
        objPropUser.CustomerID = custID;
        #region Company Check
        objPropUser.UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"].ToString());
        if (Convert.ToString(HttpContext.Current.Session["CmpChkDefault"]) == "1")
        {
            objPropUser.EN = 1;
        }
        else
        {
            objPropUser.EN = 0;
        }
        #endregion
        ds = objBL_User.GetLocationProspectSearch(objPropUser, new GeneralFunctions().GetSalesAsigned(), isProspect);
        dictListEval = objGeneral.RowsToDictionary(ds.Tables[0]);
        str = sr.Serialize(dictListEval);
        return str;
    }

    /// <summary>
    /// GetLocationProspect
    /// </summary>
    /// <param name="prefixText"></param>
    /// <param name="con"></param>
    /// <param name="custID"></param>
    /// <returns></returns>
    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string MainSearch(string prefixText)
    {
        User objPropUser = new User();
        BL_User objBL_User = new BL_User();

        string str;
        JavaScriptSerializer sr = new JavaScriptSerializer();
        List<Dictionary<object, object>> dictListEval = new List<Dictionary<object, object>>();

        DataSet ds = new DataSet();
        objPropUser.SearchValue = prefixText;
        objPropUser.ConnConfig = HttpContext.Current.Session["config"].ToString(); ;
        
        ds = objBL_User.MainSearch(objPropUser);
        dictListEval = objGeneral.RowsToDictionary(ds.Tables[0]);
        str = sr.Serialize(dictListEval);
        return str;
    }

    /// <summary>
    /// getEquipment
    /// </summary>
    /// <param name="prefixText"></param>
    /// <returns></returns>
    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string getTaskContacts(string prefixText)
    {
        User objPropUser = new User();
        BL_User objBL_User = new BL_User();

        string str;
        JavaScriptSerializer sr = new JavaScriptSerializer();
        List<Dictionary<object, object>> dictListEval = new List<Dictionary<object, object>>();

        DataSet ds = new DataSet();
        objPropUser.SearchValue = prefixText;
        objPropUser.ConnConfig = HttpContext.Current.Session["config"].ToString();
        #region Company Check
        objPropUser.UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"].ToString());
        if (Convert.ToString(HttpContext.Current.Session["CmpChkDefault"]) == "1")
        {
            objPropUser.EN = 1;
        }
        else
        {
            objPropUser.EN = 0;
        }
        #endregion
        ds = objBL_User.getTaskContactsSearch(objPropUser, new GeneralFunctions().GetSalesAsigned());
        dictListEval = objGeneral.RowsToDictionary(ds.Tables[0]);
        str = sr.Serialize(dictListEval);
        return str;
    }

    /// <summary>
    /// GetContactsSearchbyRolid
    /// </summary>
    /// <param name="prefixText"></param>
    /// <returns></returns>
    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string GetContactsSearchbyRolid(string prefixText, string RoleID)
    {
        User objPropUser = new User();
        BL_User objBL_User = new BL_User();

        string str;
        JavaScriptSerializer sr = new JavaScriptSerializer();
        List<Dictionary<object, object>> dictListEval = new List<Dictionary<object, object>>();

        DataSet ds = new DataSet();
        objPropUser.SearchValue = prefixText;
        objPropUser.ConnConfig = HttpContext.Current.Session["config"].ToString();
        if (RoleID != null && RoleID != "")
        {
            objPropUser.RoleID = Convert.ToInt32(RoleID);
            ds = objBL_User.GetContactsSearchbyRolid(objPropUser);
            DataTable dt = ds.Tables[0];

            if (dt.Rows.Count.Equals(0))
            {
                DataRow dr = dt.NewRow();
                dr["value"] = 0;
                dr["label"] = "No Record Found!";
                dt.Rows.InsertAt(dr, 0);
            }
            dictListEval = objGeneral.RowsToDictionary(dt);
        }
        str = sr.Serialize(dictListEval);
        return str;
    }


    /// <summary>
    /// getEquipment
    /// </summary>
    /// <param name="prefixText"></param>
    /// <returns></returns>
    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string getEquipment(string prefixText)
    {
        User objPropUser = new User();
        BL_User objBL_User = new BL_User();

        string str;
        JavaScriptSerializer sr = new JavaScriptSerializer();
        List<Dictionary<object, object>> dictListEval = new List<Dictionary<object, object>>();

        DataSet ds = new DataSet();
        objPropUser.SearchValue = prefixText;
        objPropUser.ConnConfig = HttpContext.Current.Session["config"].ToString();
        ds = objBL_User.getElevSearch(objPropUser);
        dictListEval = objGeneral.RowsToDictionary(ds.Tables[0]);
        str = sr.Serialize(dictListEval);
        return str;
    }

    //[WebMethod(EnableSession = true)]
    //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    //public List<Dictionary<object, object>> GetCustomerWOSerialize(string prefixText, string con)
    //{
    //    User objPropUser = new User();
    //    BL_User objBL_User = new BL_User();

    //    List<Dictionary<object, object>> dictListEval = new List<Dictionary<object, object>>();

    //    DataSet ds = new DataSet();
    //    objPropUser.SearchValue = prefixText;
    //    objPropUser.ConnConfig = HttpContext.Current.Session["config"].ToString(); ;
    //    ds = objBL_User.getCustomerAutojquery(objPropUser);
    //    dictListEval = objGeneral.RowsToDictionary(ds.Tables[0]);
    //    return dictListEval;
    //}

    /// <summary>
    /// getAlertContact
    /// </summary>
    /// <param name="prefixText"></param>
    /// <returns></returns>
    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string getAlertContact(string prefixText)
    {
        BL_Alerts objBL_Alerts = new BL_Alerts();
        Alerts objAlerts = new Alerts();

        string str;
        JavaScriptSerializer sr = new JavaScriptSerializer();
        List<Dictionary<object, object>> dictListEval = new List<Dictionary<object, object>>();

        DataSet ds = new DataSet();
        objAlerts.SearchValue = prefixText;
        objAlerts.ConnConfig = HttpContext.Current.Session["config"].ToString(); ;
        ds = objBL_Alerts.getAlertContactSearch(objAlerts);
        dictListEval = objGeneral.RowsToDictionary(ds.Tables[0]);
        str = sr.Serialize(dictListEval);
        return str;
    }

    /// <summary>
    /// GetGC
    /// </summary>
    /// <param name="prefixText"></param>
    /// <param name="con"></param>
    /// <returns></returns>
    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string GetGC(string prefixText, string con)
    {
        User objPropUser = new User();
        BL_User objBL_User = new BL_User();

        string str;
        JavaScriptSerializer sr = new JavaScriptSerializer();
        List<Dictionary<object, object>> dictListEval = new List<Dictionary<object, object>>();

        DataSet ds = new DataSet();
        objPropUser.SearchValue = prefixText;
        objPropUser.ConnConfig = HttpContext.Current.Session["config"].ToString(); ;
        ds = objBL_User.getGCAutojquery(objPropUser);
        dictListEval = objGeneral.RowsToDictionary(ds.Tables[0]);
        str = sr.Serialize(dictListEval);
        return str;
    }


    /// <summary>
    /// GetGCorHomeOwner
    /// </summary>
    /// <param name="prefixText"></param>
    /// <param name="con"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string GetGCorHomeOwner(string prefixText, string con, string type)
    {
        User objPropUser = new User();
        BL_User objBL_User = new BL_User();

        string str;
        JavaScriptSerializer sr = new JavaScriptSerializer();
        List<Dictionary<object, object>> dictListEval = new List<Dictionary<object, object>>();

        DataSet ds = new DataSet();
        objPropUser.SearchValue = prefixText;
        objPropUser.ConnConfig = HttpContext.Current.Session["config"].ToString();
        objPropUser.Type = type;
        ds = objBL_User.getHomeOwnerAutojquery(objPropUser);
        dictListEval = objGeneral.RowsToDictionary(ds.Tables[0]);
        str = sr.Serialize(dictListEval);
        return str;
    }


    /// <summary>
    /// GetRequestForServiceCall
    /// </summary>
    /// <returns></returns>
    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string GetRequestForServiceCall()
    {

        string str = "0";
        if (HttpContext.Current.Session["config"] != null)
        {
            objMapData.ConnConfig = HttpContext.Current.Session["config"].ToString();
            DataSet ds1 = new DataSet();
            ds1 = objBL_MapData.GetRequestForServiceCall(objMapData, new GeneralFunctions().GetSalesAsigned());
            if (ds1.Tables[1].Rows.Count > 0)
            {
                str = (ds1.Tables[1].Rows[0]["RequestForServiceCount"].ToString());
            }
            else
            {
                str = "0";
            }
        }
        return str;
    }


    /// <summary>
    /// Get Contact Auto jquery
    /// </summary>
    /// <param name="prefixText"></param>
    /// <param name="con"></param>
    /// <param name="custID"></param>
    /// <param name="LocID"></param>
    /// <param name="JobId"></param>
    /// <returns></returns>
    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string GetContactAutojquery(string prefixText, string con, int custID, int LocID, int JobId)
    {
        User objPropUser = new User();
        BL_Contact objBL_User = new BL_Contact();

        string str;
        JavaScriptSerializer sr = new JavaScriptSerializer();
        List<Dictionary<object, object>> dictListEval = new List<Dictionary<object, object>>();

        DataSet ds = new DataSet();
        objPropUser.SearchValue = prefixText;
        objPropUser.ConnConfig = HttpContext.Current.Session["config"].ToString(); ;
        objPropUser.CustomerID = custID;
        objPropUser.LocID = LocID;
        objPropUser.JobId = JobId;
        ds = objBL_User.getContactAutojquery(objPropUser, new GeneralFunctions().GetSalesAsigned());
        dictListEval = objGeneral.RowsToDictionary(ds.Tables[0]);
        str = sr.Serialize(dictListEval);
        return str;
    }


    [WebMethod(EnableSession = true)]
    public string[] GetCustomers11(string prefixText, int count, string contextKey)
    {

        Int32 IsSelesAsigned = 0;
        if (HttpContext.Current.Session["type"].ToString() != "am" && HttpContext.Current.Session["type"].ToString() != "c" && HttpContext.Current.Session["MSM"].ToString() != "TS")
        {
            DataTable dsSalesAssigned = new DataTable();
            dsSalesAssigned = (DataTable)HttpContext.Current.Session["userinfo"];
            string SalesAssigned = dsSalesAssigned.Rows[0]["SalesAssigned"] == DBNull.Value ? "0" : dsSalesAssigned.Rows[0]["SalesAssigned"].ToString();
            if (SalesAssigned == "1")
            {
                IsSelesAsigned = Convert.ToInt32(HttpContext.Current.Session["userid"]);
            }
        }
        User objPropUser = new User();
        BL_User objBL_User = new BL_User();

        DataSet ds = new DataSet();
        //objPropUser.DBName = HttpContext.Current.Session["dbname"].ToString();
        //objPropUser.SearchBy = "Name";
        #region Company Check
        objPropUser.UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"].ToString());
        if (Convert.ToString(HttpContext.Current.Session["CmpChkDefault"]) == "1")
        {
            objPropUser.EN = 1;
        }
        else
        {
            objPropUser.EN = 0;
        }
        #endregion
        objPropUser.SearchValue = prefixText.Replace("'","''");
        objPropUser.ConnConfig = HttpContext.Current.Session["config"].ToString();
        ds = objBL_User.getCustomerAuto(objPropUser, IsSelesAsigned);
        //ds = objBL_User.getCustomerSearch(objPropUser);

        DataTable dt = ds.Tables[0];

        List<string> txtItems = new List<string>();
        String dbValues;

        foreach (DataRow row in dt.Rows)
        {
            dbValues = AutoCompleteExtender.CreateAutoCompleteItem(row["Name"].ToString(), row["id"].ToString());
            txtItems.Add(dbValues);
        }

        return txtItems.ToArray();
    }

    [WebMethod(EnableSession = true)]
    public  string[] GetAccounts(string prefixText, int count, string contextKey)
    {
        User objPropUser = new User();
        BL_User objBL_User = new BL_User();

        DataSet ds = new DataSet();
        objPropUser.SearchValue = prefixText;
        objPropUser.ConnConfig = HttpContext.Current.Session["config"].ToString();
        ds = objBL_User.getAccountAuto(objPropUser);

        DataTable dt = ds.Tables[0];

        List<string> txtItems = new List<string>();
        String dbValues;

        foreach (DataRow row in dt.Rows)
        {
            dbValues = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(row["Account"].ToString(), row["id"].ToString());
            txtItems.Add(dbValues);
        }

        return txtItems.ToArray();
    }

    [WebMethod(EnableSession = true)]
    public  string[] GetBillingCodes(string prefixText, int count, string contextKey)
    {
        User _objPropUser = new User();
        BL_User _objBLUser = new BL_User();


        DataSet ds = new DataSet();
        _objPropUser.SearchValue = prefixText;
        _objPropUser.ConnConfig = HttpContext.Current.Session["config"].ToString();

        ds = _objBLUser.GetBillCodeSearch(_objPropUser);

        List<string> txtItems = new List<string>();
        String dbValues;

        foreach (DataRow row in ds.Tables[0].Rows)
        {
            dbValues = AutoCompleteExtender.CreateAutoCompleteItem(row["Name"].ToString(), row["id"].ToString());
            txtItems.Add(dbValues);
        }

        return txtItems.ToArray();
    }

    [WebMethod(EnableSession = true)]
    public string[] GetPageName(string prefixText, int count, string contextKey)
    {
        User objPropUser = new User();
        BL_User objBL_User = new BL_User();

        DataSet ds = new DataSet();
        objPropUser.SearchValue = prefixText;
        objPropUser.ConnConfig = HttpContext.Current.Session["config"].ToString();
        ds = objBL_User.GetSearchPages(objPropUser);

        DataTable dt = ds.Tables[0];

        List<string> txtItems = new List<string>();
        String dbValues;

        foreach (DataRow row in dt.Rows)
        {
            dbValues = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(row["PageName"].ToString(), row["ID"].ToString());
            txtItems.Add(dbValues);
        }

        return txtItems.ToArray();
    }

    [WebMethod(EnableSession = true)]
    public string[] ServiceGetEquipClassification(string prefixText, int count, string contextKey)
    {
        BL_User objBL_User = new BL_User();
        User objPropUser = new User();
        DataSet ds = new DataSet();
        objPropUser.ConnConfig = HttpContext.Current.Session["config"].ToString();
        objPropUser.Classification = prefixText;
        ds = objBL_User.getEquipClassificationLikeName(objPropUser);
        DataTable dt = ds.Tables[0];

        List<string> txtItems = new List<string>();
        String dbValues;

        foreach (DataRow row in dt.Rows)
        {
            dbValues = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(row["edesc"].ToString(), row["edesc"].ToString());
            txtItems.Add(dbValues);
        }

        return txtItems.ToArray();
    }

    [WebMethod(EnableSession = true)]
    public string[] ServiceGetTestType(string prefixText, int count, string contextKey)
    {
        TestTypes _Objprptt = new TestTypes();
        BL_SafetyTest _bltest = new BL_SafetyTest();
        DataSet ds = new DataSet();
        _Objprptt.ConnConfig = HttpContext.Current.Session["config"].ToString();
        _Objprptt.Name = prefixText;
        if (prefixText == "")
        {
            ds = _bltest.GetAllTestTypes(_Objprptt);
        }
        else
        {
            ds = _bltest.GetTestTypesLikeName(_Objprptt);
        }

        DataTable dt = ds.Tables[0];

        List<string> txtItems = new List<string>();
        String dbValues;

        foreach (DataRow row in dt.Rows)
        {
            dbValues = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(row["Name"].ToString(), row["ID"].ToString());
            txtItems.Add(dbValues);
        }

        return txtItems.ToArray();
    }

    [WebMethod(EnableSession = true)]
    public static string[] GetCodes11(string prefixText, int count, string contextKey)
    {
        Customer objPropCustomer = new Customer();
        BL_Customer objBL_Customer = new BL_Customer();

        DataSet ds = new DataSet();
        objPropCustomer.SearchValue = prefixText;
        objPropCustomer.ConnConfig = HttpContext.Current.Session["config"].ToString();
        objPropCustomer.TemplateID = 0;
        ds = objBL_Customer.getTemplateItemCodes(objPropCustomer);

        DataTable dt = ds.Tables[0];

        List<string> lstItems = new List<string>();
        String dbValues;

        foreach (DataRow row in dt.Rows)
        {
            dbValues = AutoCompleteExtender.CreateAutoCompleteItem(row["code"].ToString(), row["code"].ToString());
            lstItems.Add(dbValues);
        }

        return lstItems.ToArray();
    }


    [WebMethod(EnableSession = true)]
    public static string[] GetCodeAPBILL(string prefixText, int count, string contextKey)
    {
        JobT objJob = new JobT();
        BL_Job objBL_Job = new BL_Job();

        DataSet ds = new DataSet();
        //objJob.SearchValue = prefixText
        objJob.ConnConfig = HttpContext.Current.Session["config"].ToString();
        ds = objBL_Job.GetJobCode(objJob);
        DataTable dt = ds.Tables[0];

        List<string> txtValue = new List<string>();
        String dbValues;

        foreach (DataRow row in dt.Rows)
        {
            dbValues = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(row["label"].ToString(), row["value"].ToString());
            txtValue.Add(dbValues);
        }
        return txtValue.ToArray();
    }

    [WebMethod(EnableSession = true)]
    public static string[] GetUMAPBILL(string prefixText, int count)
    {
        JobT _objJob = new JobT();
        BL_Job _objBLJob = new BL_Job();

        DataSet ds = new DataSet();
        _objJob.ConnConfig = HttpContext.Current.Session["config"].ToString();
        ds = _objBLJob.GetAllUM(_objJob);
        DataTable dt = ds.Tables[0];

        List<string> txtValue = new List<string>();
        String dbValues;
        if (dt.Rows.Count.Equals(0))
        {
            DataRow dr = dt.NewRow();
            dr["value"] = 0;
            dr["label"] = "No Record Found!";
            dt.Rows.InsertAt(dr, 0);
        }

        foreach (DataRow row in dt.Rows)
        {
            dbValues = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(row["label"].ToString(), row["value"].ToString());
            txtValue.Add(dbValues);
        }

        return txtValue.ToArray();
    }

    [WebMethod(EnableSession = true)]
    public static string[] GetItemsAPBILL(string prefixText, int count, string contextKey)
    {
        Wage objWage = new Wage();
        BL_User objBL_User = new BL_User();
        JobT objJob = new JobT();
        BL_Job objBL_Job = new BL_Job();

        List<string> txtValue = new List<string>();
        DataTable dtval = new DataTable();
        //objJob.SearchValue = prefixText
        objJob.ConnConfig = HttpContext.Current.Session["config"].ToString();
        objWage.ConnConfig = HttpContext.Current.Session["config"].ToString();
        if (contextKey == null)
            contextKey = string.Empty;

        if (contextKey.Equals("1"))  // on select of Materials, fill inv data
        {
            DataSet ds = objBL_Job.GetAllInvDetails(objJob);
            dtval = ds.Tables[0];
        }
        else if (contextKey.Equals("2"))   // on select of Labor, fill wage data
        {
            DataSet ds = objBL_User.GetAllWage(objWage);
            dtval = ds.Tables[0];
        }
        else
        {
            dtval.Columns.Add("label", typeof(string));
            dtval.Columns.Add("value", typeof(int));

            DataRow drval = dtval.NewRow();
            drval["value"] = 0;
            drval["label"] = "No data found";
            dtval.Rows.Add(drval);
        }

        DataTable dt = dtval;
        String dbValues;

        foreach (DataRow row in dt.Rows)
        {
            dbValues = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(row["label"].ToString(), row["value"].ToString());
            txtValue.Add(dbValues);
        }
        return txtValue.ToArray();
    }
    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public  string GetBillRefExistAPBILL(string Ref, string VendorID)
    {
        string IsExist;
        PJ _objPJNew = new PJ();
        BL_Bills _objBLBillsNew = new BL_Bills();
        _objPJNew.ConnConfig = HttpContext.Current.Session["config"].ToString();
        _objPJNew.Ref = Ref;
        _objPJNew.Vendor = Convert.ToInt32(VendorID);
        IsExist = Convert.ToString(_objBLBillsNew.IsBillExistForInsert(_objPJNew));
        return IsExist;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public  string GetBillRefExistEditAPBILL(string Ref, string VendorID, string PJID)
    {
        string IsExist;
        PJ _objPJNew = new PJ();
        BL_Bills _objBLBillsNew = new BL_Bills();
        _objPJNew.ConnConfig = HttpContext.Current.Session["config"].ToString();
        _objPJNew.Ref = Ref;
        _objPJNew.ID = Convert.ToInt32(PJID);
        _objPJNew.Vendor = Convert.ToInt32(VendorID);
        IsExist = Convert.ToString(_objBLBillsNew.IsBillExistForEdit(_objPJNew));
        return IsExist;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string GetBillRecurrRefExistAPBILL(string Ref, string VendorID)
    {
        string IsExist;
        PJ _objPJNew = new PJ();
        BL_Bills _objBLBillsNew = new BL_Bills();
        _objPJNew.ConnConfig = HttpContext.Current.Session["config"].ToString();
        _objPJNew.Ref = Ref;
        _objPJNew.Vendor = Convert.ToInt32(VendorID);
        IsExist = Convert.ToString(_objBLBillsNew.IsBillRecurrExistForInsert(_objPJNew));
        return IsExist;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string GetBillRecurrRefExistEditAPBILL(string Ref, string VendorID, string PJID)
    {
        string IsExist;
        PJ _objPJNew = new PJ();
        BL_Bills _objBLBillsNew = new BL_Bills();
        _objPJNew.ConnConfig = HttpContext.Current.Session["config"].ToString();
        _objPJNew.Ref = Ref;
        _objPJNew.ID = Convert.ToInt32(PJID);
        _objPJNew.Vendor = Convert.ToInt32(VendorID);
        IsExist = Convert.ToString(_objBLBillsNew.IsBillRecurrExistForEdit(_objPJNew));
        return IsExist;
    }

    [WebMethod(EnableSession = true)]

    public static string[] GetCodeADDPO(string prefixText, int count, string contextKey)
    {
        JobT objJob = new JobT();
        BL_Job objBL_Job = new BL_Job();

        DataSet ds = new DataSet();
        //objJob.SearchValue = prefixText
        objJob.ConnConfig = HttpContext.Current.Session["config"].ToString();
        ds = objBL_Job.GetJobCode(objJob);
        DataTable dt = ds.Tables[0];

        List<string> txtValue = new List<string>();
        String dbValues;

        foreach (DataRow row in dt.Rows)
        {
            dbValues = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(row["label"].ToString(), row["value"].ToString());
            txtValue.Add(dbValues);
        }
        return txtValue.ToArray();
    }
    [WebMethod(EnableSession = true)]

    public static string[] GetUMADDPO(string prefixText, int count)
    {
        JobT _objJob = new JobT();
        BL_Job _objBLJob = new BL_Job();

        DataSet ds = new DataSet();
        _objJob.ConnConfig = HttpContext.Current.Session["config"].ToString();
        ds = _objBLJob.GetAllUM(_objJob);
        DataTable dt = ds.Tables[0];

        List<string> txtValue = new List<string>();
        String dbValues;
        if (dt.Rows.Count.Equals(0))
        {
            DataRow dr = dt.NewRow();
            dr["value"] = 0;
            dr["label"] = "No Record Found!";
            dt.Rows.InsertAt(dr, 0);
        }

        foreach (DataRow row in dt.Rows)
        {
            dbValues = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(row["label"].ToString(), row["value"].ToString());
            txtValue.Add(dbValues);
        }

        return txtValue.ToArray();
    }
    [WebMethod(EnableSession = true)]

    public static string[] GetItemsADDPO(string prefixText, int count, string contextKey)
    {
        Wage objWage = new Wage();
        BL_User objBL_User = new BL_User();
        JobT objJob = new JobT();
        BL_Job objBL_Job = new BL_Job();

        List<string> txtValue = new List<string>();
        DataTable dtval = new DataTable();
        //objJob.SearchValue = prefixText
        objJob.ConnConfig = HttpContext.Current.Session["config"].ToString();
        objWage.ConnConfig = HttpContext.Current.Session["config"].ToString();
        if (contextKey == null)
            contextKey = string.Empty;

        if (contextKey.Equals("1"))  // on select of Materials, fill inv data
        {
            DataSet ds = objBL_Job.GetAllInvDetails(objJob);
            dtval = ds.Tables[0];
        }
        else if (contextKey.Equals("2"))   // on select of Labor, fill wage data
        {
            DataSet ds = objBL_User.GetAllWage(objWage);
            dtval = ds.Tables[0];
        }
        else
        {
            dtval.Columns.Add("label", typeof(string));
            dtval.Columns.Add("value", typeof(int));

            DataRow drval = dtval.NewRow();
            drval["value"] = 0;
            drval["label"] = "No data found";
            dtval.Rows.Add(drval);
        }

        DataTable dt = dtval;
        String dbValues;

        foreach (DataRow row in dt.Rows)
        {
            dbValues = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(row["label"].ToString(), row["value"].ToString());
            txtValue.Add(dbValues);
        }
        return txtValue.ToArray();
    }

    [System.Web.Services.WebMethod(EnableSession = true)]
    public string[] ServiceGetOpenInvoice(string prefixText, int count, string contextKey)
    {
        BL_Deposit objBL_Deposit = new BL_Deposit();

        DataTable dt = new DataTable();
        DataSet ds = new DataSet();
        ds = objBL_Deposit.GetInvoicesByReceivedPayMulti(HttpContext.Current.Session["config"].ToString(), 0, "", prefixText);

        dt = ds.Tables[0];

        List<string> txtItems = new List<string>();
        String dbValues;

        foreach (DataRow row in dt.Rows)
        {
            dbValues = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(row["Invoice"].ToString(), row["Invoice"].ToString());
            txtItems.Add(dbValues);
        }
        return txtItems.ToArray();
    }

    [System.Web.Services.WebMethod(EnableSession = true)]
    public string[] ServiceGetInvoicePay(string prefixText, int count, string contextKey)
    {

        BL_Deposit objBL_Deposit = new BL_Deposit();
        PaymentDetails objPayment = new PaymentDetails();

        DataTable dt = new DataTable();
        DataSet ds = new DataSet();
        string contextHdnOwner = contextKey;
        List<String> ls = new List<string>();
        int owner = 0;
        String loc = "";
        List<string> txtItems = new List<string>();
        String dbValues;
        if (!String.IsNullOrEmpty(contextKey))
        {
            ls = contextKey.Split('$').ToList();
            owner = Convert.ToInt32(ls[0].ToString().Trim());
            loc = ls[1].ToString().Trim();
            ds = objBL_Deposit.GetInvoicesByReceivedPayMulti(HttpContext.Current.Session["config"].ToString(), owner, loc, prefixText);


            dt = ds.Tables[0];
            foreach (DataRow row in dt.Rows)
            {
                dbValues = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(row["Invoice"].ToString(), row["Amount"].ToString() + "-" + row["AmountDue"].ToString());
                txtItems.Add(dbValues);
            }
        }
        return txtItems.ToArray();
    }

    [System.Web.Services.WebMethod(EnableSession = true)]
    public string[] GetContactEmails(string prefixText, int count, string contextKey)
    {
        DataTable dt = WebBaseUtility.GetContactListOnExchangeServer();

        List<string> txtItems = new List<string>();
        String dbValues;

        foreach (DataRow row in dt.Rows)
        {
            dbValues = AutoCompleteExtender.CreateAutoCompleteItem(row["MemberName"].ToString() + "(" + row["MemberEmail"].ToString() + ")", row["MemberEmail"].ToString());
            txtItems.Add(dbValues);
        }

        return txtItems.ToArray();
    }
    #region Service

    [System.Web.Services.WebMethod(EnableSession = true)]
    public  string ServiceGetListOpenInvoice(string prefixText, int count, string contextKey)
    {

        BL_Deposit objBL_Deposit = new BL_Deposit();
        PaymentDetails objPayment = new PaymentDetails();

        string str;
        JavaScriptSerializer sr = new JavaScriptSerializer();
        List<Dictionary<object, object>> dictListEval = new List<Dictionary<object, object>>();

        DataSet ds = new DataSet();
        try
        {

            DataTable dt = new DataTable();
            ds = objBL_Deposit.GetInvoicesByReceivedPayMulti(HttpContext.Current.Session["config"].ToString(), 0, "", prefixText);

            dt = ds.Tables[0];
            GeneralFunctions objGeneral = new GeneralFunctions();
            dictListEval = objGeneral.RowsToDictionary(dt);

            str = sr.Serialize(dictListEval);
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return str;
    }


    [System.Web.Services.WebMethod(EnableSession = true)]
    public  string ServiceGLAccount(string prefixText, int count, string contextKey)
    {

        BL_Deposit objBL_Deposit = new BL_Deposit();
        PaymentDetails objPayment = new PaymentDetails();

        string str;
        JavaScriptSerializer sr = new JavaScriptSerializer();
        List<Dictionary<object, object>> dictListEval = new List<Dictionary<object, object>>();

        DataSet ds = new DataSet();
        try
        {

            DataTable dt = new DataTable();
            ds = objBL_Deposit.GetGLAccount(HttpContext.Current.Session["config"].ToString(), prefixText);

            dt = ds.Tables[0];
            GeneralFunctions objGeneral = new GeneralFunctions();
            dictListEval = objGeneral.RowsToDictionary(dt);

            str = sr.Serialize(dictListEval);
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return str;
    }
    #endregion




    #region AddTest


    [System.Web.Services.WebMethod(EnableSession = true)]
    public  string GetEquipment(string prefixText, string con, int custID)
    {
        string str;
        JavaScriptSerializer sr = new JavaScriptSerializer();
        List<Dictionary<object, object>> dictListEval = new List<Dictionary<object, object>>();
        BL_User objBL_User = new BL_User();
        BusinessEntity.User objPropUser = new BusinessEntity.User();
        GeneralFunctions objGeneral = new GeneralFunctions();

        DataSet ds = new DataSet();
        objPropUser.SearchBy = "e.unit";
        objPropUser.SearchValue = prefixText;
        objPropUser.ConnConfig = WebBaseUtility.ConnectionString;
        objPropUser.LocID = custID;
        ds = ds = objBL_User.getElev(objPropUser);
        dictListEval = objGeneral.RowsToDictionary(ds.Tables[0]);
        str = sr.Serialize(dictListEval);

        return str;
    }


    [System.Web.Services.WebMethod(EnableSession = true)]
    public  string GetJobsByLocation(string SearchValue)
    {
        string str;
        JavaScriptSerializer sr = new JavaScriptSerializer();
        List<Dictionary<object, object>> dictListEval = new List<Dictionary<object, object>>();
        BL_Customer objBL_Customer = new BL_Customer();

        Customer objProp_Customer = new Customer();

        GeneralFunctions objGeneral = new GeneralFunctions();

        DataSet ds = new DataSet();
        objProp_Customer.SearchBy = "j.Loc";
        objProp_Customer.SearchValue = SearchValue;
        objProp_Customer.JobType = -1;
        objProp_Customer.StartDate = "";


        objProp_Customer.EndDate = "";
        objProp_Customer.ConnConfig = WebBaseUtility.ConnectionString;
        objProp_Customer.Range = 1;

        DataTable dtFilters = CreateFiltersToDataTable();

        ds = objBL_Customer.getJobProject(objProp_Customer, dtFilters, 0, 0);

        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            if (dr["Status"].ToString() == "Closed")
                dr.Delete();
        }

        ds.AcceptChanges();

        dictListEval = objGeneral.RowsToDictionary(ds.Tables[0]);


        str = sr.Serialize(dictListEval);

        return str;
    }


    private static DataTable CreateFiltersToDataTable()
    {
        //create new table to add filter values.
        DataTable dtFilters = new DataTable();
        dtFilters.Clear();
        dtFilters.Columns.Add("Customer");
        dtFilters.Columns.Add("Tag");
        dtFilters.Columns.Add("ID");
        dtFilters.Columns.Add("fdesc");
        dtFilters.Columns.Add("Status");
        dtFilters.Columns.Add("Stage");
        dtFilters.Columns.Add("Company");
        dtFilters.Columns.Add("CType");
        dtFilters.Columns.Add("TemplateDesc");
        dtFilters.Columns.Add("Type");
        dtFilters.Columns.Add("SalesPerson");
        dtFilters.Columns.Add("Route");
        dtFilters.Columns.Add("NHour");
        dtFilters.Columns.Add("ContractPrice");
        dtFilters.Columns.Add("NotBilledYet");
        dtFilters.Columns.Add("NComm");
        dtFilters.Columns.Add("NRev");
        dtFilters.Columns.Add("NLabor");
        dtFilters.Columns.Add("NMat");
        dtFilters.Columns.Add("NOMat");
        dtFilters.Columns.Add("NCost");
        dtFilters.Columns.Add("NProfit");
        dtFilters.Columns.Add("NRatio");
        dtFilters.Columns.Add("RouteFilters");
        dtFilters.Columns.Add("StageFilters");
        dtFilters.Columns.Add("DepartmentFilters");
        dtFilters.Columns.Add("ProjectManagerUserName");
        dtFilters.Columns.Add("LocationType");
        dtFilters.Columns.Add("BuildingType");
        dtFilters.Columns.Add("TotalBudgetedExpense");
        dtFilters.Columns.Add("SupervisorUserName");
        dtFilters.Columns.Add("OpenARBalance");
        dtFilters.Columns.Add("OpenAPBalance");
        dtFilters.Columns.Add("ExpectedClosingDate");
        dtFilters.Columns.Add("Estimate");
        DataRow dtFiltersRow = dtFilters.NewRow();
        dtFilters.Rows.Add(dtFiltersRow);

        return dtFilters;
    }

    [System.Web.Services.WebMethod(EnableSession = true)]
    public  string GetWorkers(string SearchValue)
    {
        string str;
        JavaScriptSerializer sr = new JavaScriptSerializer();
        List<Dictionary<object, object>> dictListEval = new List<Dictionary<object, object>>();
        BL_Customer objBL_Customer = new BL_Customer();

        Customer objProp_Customer = new Customer();

        GeneralFunctions objGeneral = new GeneralFunctions();

        DataSet ds = new DataSet();
        objProp_Customer.SearchBy = "fDesc";
        objProp_Customer.SearchValue = SearchValue;

        objProp_Customer.ConnConfig = WebBaseUtility.ConnectionString;

        ds = objBL_Customer.GetWorker(objProp_Customer);
        dictListEval = objGeneral.RowsToDictionary(ds.Tables[0]);
        str = sr.Serialize(dictListEval);

        return str;
    }
    [WebMethod(EnableSession = true)]
    public string[] ServiceGetWorkers(string prefixText, int count, string contextKey)
    {
        BL_Customer objBL_Customer = new BL_Customer();

        Customer objProp_Customer = new Customer();

        GeneralFunctions objGeneral = new GeneralFunctions();

        DataSet ds = new DataSet();
        objProp_Customer.SearchBy = "fDesc";
        objProp_Customer.SearchValue = prefixText;

        objProp_Customer.ConnConfig = WebBaseUtility.ConnectionString;

        ds = objBL_Customer.GetWorker(objProp_Customer);    

        DataTable dt = ds.Tables[0];

        List<string> txtItems = new List<string>();
        String dbValues;

        foreach (DataRow row in dt.Rows)
        {
            dbValues = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(row["fDesc"].ToString(), row["ID"].ToString());
            txtItems.Add(dbValues);
        }

        return txtItems.ToArray();
      
        
    }


    [System.Web.Services.WebMethod(EnableSession = true)]
    public  string ReceivePricing(string equipClassification, int testtypeId, int priceYear = 0)
    {
        string str;
        JavaScriptSerializer sr = new JavaScriptSerializer();
        List<Dictionary<object, object>> dictListEval = new List<Dictionary<object, object>>();
        BL_User objBL_User = new BL_User();
        BusinessEntity.User objPropUser = new BusinessEntity.User();
        GeneralFunctions objGeneral = new GeneralFunctions();

        DataSet ds = new DataSet();
        ds = objBL_User.ValidateEquipmentTestPricing(HttpContext.Current.Session["config"].ToString(), equipClassification, testtypeId, priceYear);
        dictListEval = objGeneral.RowsToDictionary(ds.Tables[0]);
        str = sr.Serialize(dictListEval);
        return str;
    }

    [System.Web.Services.WebMethod(EnableSession = true)]
    public  string ReceiveTestType(int testtypeId)
    {
        string str;
        JavaScriptSerializer sr = new JavaScriptSerializer();
        List<Dictionary<object, object>> dictListEval = new List<Dictionary<object, object>>();

        BusinessEntity.User objPropUser = new BusinessEntity.User();
        GeneralFunctions objGeneral = new GeneralFunctions();
        BL_SafetyTest _objbltesttypes = new BL_SafetyTest();
        DataSet ds = new DataSet();
        ds = _objbltesttypes.GetTestTypeById(HttpContext.Current.Session["config"].ToString(), Convert.ToInt32(testtypeId));
        dictListEval = objGeneral.RowsToDictionary(ds.Tables[0]);
        str = sr.Serialize(dictListEval);
        return str;
    }

    [System.Web.Services.WebMethod(EnableSession = true)]
    public  string[] GetAlertEmail(string prefixText)
    {
        List<string> txtItems = new List<string>();
        DataSet ds = new DataSet();
        BL_SafetyTest bl_SafetyTest = new BL_SafetyTest();
        ds = bl_SafetyTest.SearchEmailTeam(WebBaseUtility.ConnectionString, prefixText);
        if (ds.Tables.Count > 0)
        {
            DataTable dt = ds.Tables[0];
            String dbValues;
            foreach (DataRow row in dt.Rows)
            {
                dbValues = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(row["Email"].ToString(), row["Email"].ToString());
                txtItems.Add(dbValues);
            }

        }
        return txtItems.ToArray();

    }
    #endregion

    [System.Web.Services.WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public WebMethodResponse<Dictionary<string, object>> GetPhoneByID(string PhoneID, string RolID)
    {
        WebMethodResponse<Dictionary<string, object>> jsonInformation = new BusinessEntity.WebMethodResponse<Dictionary<string, object>>();
        Dictionary<string, object> dictionary = new Dictionary<string, object>();
        jsonInformation.Header = new BusinessEntity.WebMethodHeader();

        Customer objProp_Customer = new Customer();
        BL_Customer objBL_Customer = new BL_Customer();
        objProp_Customer.ConnConfig = HttpContext.Current.Session["config"].ToString();
        objProp_Customer.PhoneID = Convert.ToInt32(PhoneID);
        objProp_Customer.RoleID = Convert.ToInt32(RolID);


        try
        {
            DataSet ds = new DataSet();
            ds = objBL_Customer.getPhoneByID(objProp_Customer);


            if (ds != null)
            {

                if (ds.Tables.Count > 0)
                {
                    dictionary.Add("Email", Convert.ToString(ds.Tables[0].Rows[0]["Email"]));
                    dictionary.Add("Phone", Convert.ToString(ds.Tables[0].Rows[0]["Phone"]));
                    dictionary.Add("Cell", Convert.ToString(ds.Tables[0].Rows[0]["Cell"]));
                    dictionary.Add("Fax", Convert.ToString(ds.Tables[0].Rows[0]["Fax"]));
                }

                if (dictionary.Count > 0)
                    jsonInformation.Header.HasError = false;
                else
                    jsonInformation.Header.HasError = true;


                jsonInformation.ReponseObject = dictionary;
            }
            else
            {
                jsonInformation.Header.HasError = true;

            }
        }
        catch (Exception ex)
        {
            string errormsg = ex.Message;
            List<string> strmsg = new List<string>();
            strmsg.Add(errormsg);

            jsonInformation.Header.HasError = true;
            jsonInformation.Header.ErrorMessages = strmsg;
        }
        return jsonInformation;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public WebMethodResponse<Dictionary<string, object>> GetEstimateRoleSpecificDetails(string RoleId)
    {
        WebMethodResponse<Dictionary<string, object>> jsonPOInformation = new BusinessEntity.WebMethodResponse<Dictionary<string, object>>();
        Dictionary<string, object> dictionary = new Dictionary<string, object>();
        jsonPOInformation.Header = new BusinessEntity.WebMethodHeader();

        BL_User objBL_User = new BL_User();
        Rol objrol = new Rol();
        objrol.ID = string.IsNullOrEmpty(RoleId) ? 0 : Convert.ToInt32(RoleId);

        try
        {
            DataSet dt = objBL_User.GetEstimateRoleSpecificDetails(objrol);

            if (dt != null)
            {
                if (dt.Tables.Count > 0)
                {
                    if (dt.Tables[0].Rows.Count > 0)
                    {
                        dictionary.Add("Address", Convert.ToString(dt.Tables[0].Rows[0]["Address"]));
                        dictionary.Add("Phone", dt.Tables[0].Rows[0]["Phone"].ToString());
                        dictionary.Add("Fax", dt.Tables[0].Rows[0]["Fax"].ToString());
                        dictionary.Add("EMail", dt.Tables[0].Rows[0]["EMail"].ToString());
                        //dictionary.Add("Contact", dt.Tables[0].Rows[0]["Contact"].ToString());
                        dictionary.Add("Cell", dt.Tables[0].Rows[0]["Cellular"].ToString());
                        dictionary.Add("STax", dt.Tables[0].Rows[0]["STax"].ToString());
                        dictionary.Add("STaxRate", dt.Tables[0].Rows[0]["STaxRate"].ToString());
                    }
                    if (dt.Tables[1].Rows.Count > 0)
                    {
                        dictionary.Add("BillAddress", Convert.ToString(dt.Tables[1].Rows[0]["BillAddress"]));
                        dictionary.Add("LocId", Convert.ToString(dt.Tables[1].Rows[0]["LocID"]));
                    }
                    if (dt.Tables[2].Rows.Count > 0)
                    {
                        var dr = dt.Tables[2].Rows.Cast<DataRow>().ToList().Select(x => x.ItemArray);
                        dictionary.Add("Contacts", dr.ToArray());
                    }
                }

                if (dictionary.Count > 0)
                    jsonPOInformation.Header.HasError = false;
                else
                    jsonPOInformation.Header.HasError = true;


                jsonPOInformation.ReponseObject = dictionary;
            }
            else
            {
                jsonPOInformation.Header.HasError = true;

            }
        }
        catch (Exception ex)
        {
            string errormsg = ex.Message;
            List<string> strmsg = new List<string>();
            strmsg.Add(errormsg);

            jsonPOInformation.Header.HasError = true;
            jsonPOInformation.Header.ErrorMessages = strmsg;
        }
        return jsonPOInformation;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public WebMethodResponse<Dictionary<string, object>> GetEstimatePhoneContactSpecificDetails(string contactId, string estimateNo)
    {
        WebMethodResponse<Dictionary<string, object>> jsonPOInformation = new BusinessEntity.WebMethodResponse<Dictionary<string, object>>();
        Dictionary<string, object> dictionary = new Dictionary<string, object>();
        jsonPOInformation.Header = new BusinessEntity.WebMethodHeader();

        BL_User objBL_User = new BL_User();
        Rol objrol = new Rol();
        int id = contactId == "null" ? 0 : Convert.ToInt32(contactId);
        int estimateId = string.IsNullOrEmpty(estimateNo) ? 0 : Convert.ToInt32(estimateNo);

        try
        {
            DataSet dt = objBL_User.GetEstimatePhoneContactSpecificDetails(id, estimateId);



            if (dt != null)
            {

                if (dt.Tables.Count > 0)
                {
                    if (dt.Tables[0].Rows.Count > 0)
                    {
                        dictionary.Add("EstimateEmail", Convert.ToString(dt.Tables[0].Rows[0]["EstimateEmail"]));
                        dictionary.Add("Phone", Convert.ToString(dt.Tables[0].Rows[0]["Phone"]));
                        dictionary.Add("EstimateCell", Convert.ToString(dt.Tables[0].Rows[0]["EstimateCell"]));
                        dictionary.Add("Fax", Convert.ToString(dt.Tables[0].Rows[0]["Fax"]));
                        dictionary.Add("LOCID", Convert.ToString(dt.Tables[0].Rows[0]["LOCID"]));
                    }

                }



                if (dictionary.Count > 0)
                    jsonPOInformation.Header.HasError = false;
                else
                    jsonPOInformation.Header.HasError = true;


                jsonPOInformation.ReponseObject = dictionary;
            }
            else
            {
                jsonPOInformation.Header.HasError = true;

            }
        }
        catch (Exception ex)
        {
            string errormsg = ex.Message;
            List<string> strmsg = new List<string>();
            strmsg.Add(errormsg);

            jsonPOInformation.Header.HasError = true;
            jsonPOInformation.Header.ErrorMessages = strmsg;
        }
        return jsonPOInformation;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public WebMethodResponse<EstimateCalculation[]> GetEstimateExpenseItems(string id, string tempId, string factor)
    {
        WebMethodResponse<EstimateCalculation[]> jsonPOInformation = new BusinessEntity.WebMethodResponse<EstimateCalculation[]>();
        Dictionary<string, object> dictionary = new Dictionary<string, object>();
        jsonPOInformation.Header = new BusinessEntity.WebMethodHeader();

        try
        {
            BL_EstimateCalculation objBL_estcal = new BL_EstimateCalculation();
            EstimateCalculation objProp_estcal = new EstimateCalculation();
            EstimateCalculation[] objProp_estcalist = null;
            objProp_estcal.ConnConfig = WebBaseUtility.ConnectionString;
            objProp_estcal.EstimateId = Convert.ToInt32(id);
            objProp_estcal.ID = string.IsNullOrEmpty(tempId) ? 0 : Convert.ToInt32(tempId);
            objProp_estcal.EstimateCalculationTemplateInputBasedCalculationfactor = string.IsNullOrEmpty(factor) ? 0 : Convert.ToDecimal(factor);


            objProp_estcalist = objProp_estcal.ID == 0 ? objBL_estcal.GetEstimateExpenseItems(objProp_estcal) : objBL_estcal.GetEstimateExpenseItemsByExpenseItem(objProp_estcal);

            if (objProp_estcalist != null)
            {
                jsonPOInformation.Header.HasError = false;
            }
            else
            {
                jsonPOInformation.Header.HasError = true;
            }
            jsonPOInformation.ReponseObject = objProp_estcalist;
        }
        catch (Exception ex)
        {
            string errormsg = ex.Message;
            List<string> strmsg = new List<string>();
            strmsg.Add(errormsg);

            jsonPOInformation.Header.HasError = true;
            jsonPOInformation.Header.ErrorMessages = strmsg;
        }
        return jsonPOInformation;
    }

    [System.Web.Services.WebMethod(EnableSession = true)]
    [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
    public string CheckEmail(string rol, int type, int uid)
    {
        int mails = 0;
        //DataSet ds = null;
        BL_General objBL_General = new BL_General();
        General objGeneral = new General();
        if (rol.Trim() != string.Empty)
        {
            objGeneral.OrderBy = "";

            objGeneral.ConnConfig = HttpContext.Current.Session["config"].ToString();
            objGeneral.type = type;
            objGeneral.rol = Convert.ToInt32(rol);
            objGeneral.userid = Convert.ToInt32(HttpContext.Current.Session["userid"].ToString());
            if (type == -2)
            {
                objGeneral.RegID = "[OP-" + uid.ToString() + "]";
                objGeneral.rol = 0;
            }
            mails = objBL_General.GetMailsCount(objGeneral);
        }
        return mails.ToString();
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public  string GetAmountInvoice(string prefixText, string con)
    {
        string str;
        JavaScriptSerializer sr = new JavaScriptSerializer();
        List<Dictionary<object, object>> dictListEval = new List<Dictionary<object, object>>();
        BL_User objBL_User = new BL_User();
        BusinessEntity.User objPropUser = new BusinessEntity.User();
        GeneralFunctions objGeneral = new GeneralFunctions();

        BL_Deposit objBL_Deposit = new BL_Deposit();
        PaymentDetails objPayment = new PaymentDetails();

        DataSet ds = new DataSet();

        ds = objBL_Deposit.GetInvoiceByList(HttpContext.Current.Session["config"].ToString(), prefixText, "", false);

        dictListEval = objGeneral.RowsToDictionary(ds.Tables[0]);
        str = sr.Serialize(dictListEval);

        return str;


    }

    [WebMethod(EnableSession = true)]
    public string[] GetActiveLocation(string prefixText, int count, string contextKey)
    {
       
        User objPropUser = new User();
        BL_User objBL_User = new BL_User();

     
        JavaScriptSerializer sr = new JavaScriptSerializer();
        List<Dictionary<object, object>> dictListEval = new List<Dictionary<object, object>>();

        DataSet ds = new DataSet();
        objPropUser.SearchValue = prefixText;
        objPropUser.ConnConfig = HttpContext.Current.Session["config"].ToString();
        objPropUser.CustomerID = 0;
        #region Company Check
        objPropUser.UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"].ToString());
        if (Convert.ToString(HttpContext.Current.Session["CmpChkDefault"]) == "1")
        {
            objPropUser.EN = 1;
        }
        else
        {
            objPropUser.EN = 0;
        }
        #endregion
        ds = objBL_User.getLocationWithInactive(objPropUser, new GeneralFunctions().GetSalesAsigned());
        dictListEval = objGeneral.RowsToDictionary(ds.Tables[0]);
        DataTable dt = ds.Tables[0];

        List<string> txtItems = new List<string>();
        String dbValues;

        foreach (DataRow row in dt.Rows)
        {
            dbValues = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(row["label"].ToString(), row["value"].ToString() + "^" + row["desc"].ToString());
            txtItems.Add(dbValues);
        }
        return txtItems.ToArray();
    }


    [System.Web.Services.WebMethod(EnableSession = true)]
    public string GetDefaultTestPriceByYear(int elevId, int testtypeId, int priceYear = 0)
    {
        string str;
        JavaScriptSerializer sr = new JavaScriptSerializer();
        List<Dictionary<object, object>> dictListEval = new List<Dictionary<object, object>>();
        BL_User objBL_User = new BL_User();
        BusinessEntity.User objPropUser = new BusinessEntity.User();
        GeneralFunctions objGeneral = new GeneralFunctions();

        DataSet ds = new DataSet();
        ds = objBL_User.GetDefaultTestPricingForEquipment(HttpContext.Current.Session["config"].ToString(), elevId, testtypeId, priceYear);
        dictListEval = objGeneral.RowsToDictionary(ds.Tables[0]);
        str = sr.Serialize(dictListEval);
        return str;
    }

    [System.Web.Services.WebMethod(EnableSession = true)]
    public string GetTestPriceByYear(String classification, int testtypeId, int priceYear = 0)
    {
        string str;
        JavaScriptSerializer sr = new JavaScriptSerializer();
        List<Dictionary<object, object>> dictListEval = new List<Dictionary<object, object>>();
        BL_User objBL_User = new BL_User();
        BusinessEntity.User objPropUser = new BusinessEntity.User();
        GeneralFunctions objGeneral = new GeneralFunctions();

        DataSet ds = new DataSet();
        ds = objBL_User.ValidateEquipmentTestPricing(HttpContext.Current.Session["config"].ToString(), classification, testtypeId, priceYear);
        dictListEval = objGeneral.RowsToDictionary(ds.Tables[0]);
        str = sr.Serialize(dictListEval);
        return str;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string SaveTestCustomer(string TestID, string EquipmentID, string TestCustomFieldID, string Value, String OldValue, string strUrl)
    {
        String str;
        DataTable dt = new DataTable();
        dt.Columns.Add("TestID", typeof(int));
        dt.Columns.Add("EquipmentID", typeof(int));
        dt.Columns.Add("TestCustomFieldID", typeof(int));
        dt.Columns.Add("Value", typeof(String));
        dt.Columns.Add("OldValue", typeof(String));
        dt.Rows.Add(Convert.ToInt32(TestID), Convert.ToInt32(EquipmentID), Convert.ToInt32(TestCustomFieldID), Value, OldValue);
        BL_SafetyTest objtestbl = new BL_SafetyTest();
        BusinessEntity.Recurring.SafetyTest objproptest = new BusinessEntity.Recurring.SafetyTest();
        objproptest.ConnConfig = Convert.ToString(HttpContext.Current.Session["config"].ToString());
        objproptest.UserName = Convert.ToString(HttpContext.Current.Session["Username"].ToString());
        objproptest.Cus_TestItemValue = dt;      
        objtestbl.UpdateTestCustomItemValue(objproptest);

        // get List Customer need alert
        DataSet lsAlert = new DataSet();
        objtestbl = new BL_SafetyTest();
        objproptest = new BusinessEntity.Recurring.SafetyTest();
        objproptest.ConnConfig = Convert.ToString(HttpContext.Current.Session["config"].ToString());
        objproptest.Cus_TestItemValue = dt;
        lsAlert = objtestbl.GetCustomFieldAlert(objproptest);
        if (lsAlert.Tables.Count > 0)
        {
            DataTable team= GetTeamMemberNew();
            processSendMail(lsAlert.Tables[0], team, strUrl);
            processCreateTask(lsAlert.Tables[0], team, strUrl);
        }

        return "Test update successfull";
    }
    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string SaveTestCustomerByYear(string TestID, string EquipmentID, string TestCustomFieldID, string Value, String OldValue, string strUrl, String TestYear)
    {
        String str;
        DataTable dt = new DataTable();
        dt.Columns.Add("TestID", typeof(int));
        dt.Columns.Add("EquipmentID", typeof(int));
        dt.Columns.Add("TestCustomFieldID", typeof(int));
        dt.Columns.Add("Value", typeof(String));
        dt.Columns.Add("OldValue", typeof(String));
        dt.Rows.Add(Convert.ToInt32(TestID), Convert.ToInt32(EquipmentID), Convert.ToInt32(TestCustomFieldID), Value, OldValue);
        BL_SafetyTest objtestbl = new BL_SafetyTest();
        BusinessEntity.Recurring.SafetyTest objproptest = new BusinessEntity.Recurring.SafetyTest();
        objproptest.ConnConfig = Convert.ToString(HttpContext.Current.Session["config"].ToString());
        objproptest.UserName = Convert.ToString(HttpContext.Current.Session["Username"].ToString());
        objproptest.Cus_TestItemValue = dt;      
        objproptest.PriceYear = Convert.ToInt32(TestYear);
        objtestbl.UpdateTestCustomItemValueByYear(objproptest);

        // get List Customer need alert
        DataSet lsAlert = new DataSet();
        objtestbl = new BL_SafetyTest();
        objproptest = new BusinessEntity.Recurring.SafetyTest();
        objproptest.ConnConfig = Convert.ToString(HttpContext.Current.Session["config"].ToString());
        objproptest.Cus_TestItemValue = dt;
        lsAlert = objtestbl.GetCustomFieldAlert(objproptest);
        if (lsAlert.Tables.Count > 0)
        {
            DataTable team = GetTeamMemberNew();
            processSendMail(lsAlert.Tables[0], team, strUrl);
            processCreateTask(lsAlert.Tables[0], team, strUrl);
        }

        return "Test update successfull";
    }

    public DataTable GetTeamMemberNew()
    {
        BL_User objBL_User = new BL_User();
        User objPropUser = new User();
        DataSet ds = new DataSet();
        objPropUser.ConnConfig = Convert.ToString(HttpContext.Current.Session["config"].ToString());
        ds = objBL_User.GetUsersAndRolesForTeamMemberList(objPropUser);
        var teamMembers = ds.Tables[0];

        DataTable contactList = WebBaseUtility.GetContactListOnExchangeServer();
        if (contactList != null && contactList.Rows.Count > 0)
        {
            // Merge this list to teamMembers
            foreach (DataRow item in contactList.Rows)
            {
                DataRow _dr = teamMembers.NewRow();
                if (string.IsNullOrEmpty(item["GroupName"].ToString()))
                {
                    _dr["memberkey"] = "3_" + item["Type"] + "|" + item["MemberEmail"] + "|" + item["MemberName"];
                    _dr["usertype"] = "Exchange " + item["Type"];
                }
                else
                {
                    _dr["memberkey"] = "4_" + item["GroupName"] + "|" + item["MemberEmail"] + "|" + item["MemberName"];
                    _dr["usertype"] = "Exchange Group: " + item["GroupName"];
                }
                _dr["fUser"] = item["MemberName"];
                _dr["email"] = item["MemberEmail"];
                _dr["roleName"] = "";
                teamMembers.Rows.Add(_dr);
            }
        }
        return teamMembers;

    }

    private  void processSendMail(DataTable obj, DataTable team, string strUrl)
    {

        DataTable lstProjectTeamMember = team;
        List<NotificationCustomChange> lsNotificationTask = new List<NotificationCustomChange>();
        List<String> lsEmail = new List<string>();
        DataSet ds = new DataSet();
        BusinessEntity.User objProp_User = new BusinessEntity.User();
        foreach (DataRow r in obj.Rows)
        {
            if (!String.IsNullOrEmpty(r["TeamMember"].ToString()) && r["IsAlert"].ToString() == "True")
            {
                NotificationCustomChange notification = new NotificationCustomChange();
                notification.SubjectEmail = r["Account"].ToString() + " - Equip ID " + r["EquipmentID"].ToString() + "Test Type " + r["TestType"].ToString() + " Alert";
                notification.UserName = Convert.ToString(HttpContext.Current.Session["username"].ToString());
                notification.label = r["label"].ToString();
                notification.EquipmentName = r["EquipmentName"].ToString();
                notification.EquipmentDesc = r["EquipmentDesc"].ToString();

                List<String> ls = r["TeamMember"].ToString().Split(';').ToList();
                foreach (string iEmail in ls)
                {
                    string[] arr = iEmail.Split('_');
                    //check create task
                    if (arr.Length > 2)
                    {
                        if (arr[2] == "0")
                        {

                            if (arr[0] == "6")
                            {
                                var changedItem = lstProjectTeamMember.Select("memberkey='" + arr[0] + "_" + arr[1] + "'").FirstOrDefault();
                                var role = changedItem["RoleName"].ToString();
                                var projTeamUsers = lstProjectTeamMember.Select("RoleName='" + role.Trim() + "'").ToList();
                                foreach (var projUser in projTeamUsers)
                                {
                                    lsEmail.Add((string)projUser["email"]);
                                }
                            }
                            else
                            {
                                var changedItem = lstProjectTeamMember.Select("memberkey='" + arr[0] + "_" + arr[1] + "'").FirstOrDefault();
                                lsEmail.Add((string)changedItem["email"]);

                            }
                        }
                    }
                    else
                    {
                        if (arr[0] == "6")
                        {
                            var changedItem = lstProjectTeamMember.Select("memberkey='" + arr[0] + "_" + arr[1] + "'").FirstOrDefault();
                            var role = changedItem["RoleName"].ToString();
                            var projTeamUsers = lstProjectTeamMember.Select("RoleName='" + role.Trim() + "'").ToList();
                            foreach (var projUser in projTeamUsers)
                            {
                                lsEmail.Add((string)projUser["email"]);
                            }
                        }
                        else
                        {
                            var changedItem = lstProjectTeamMember.Select("memberkey='" + arr[0] + "_" + arr[1] + "'").FirstOrDefault();
                            lsEmail.Add((string)changedItem["email"]);

                        }
                    }

                }
                if (lsEmail.Count > 0)
                {
                    Mail mail = new Mail();
                    mail.From = WebBaseUtility.GetFromEmailAddress();
                    mail.To = lsEmail;
                    mail.Title = notification.SubjectEmail;
                    mail.Text = notification.EmailContent();

                    WebBaseUtility.UpdateMailConfigurationFromLoginUser(mail);
                    mail.Send();
                }

            }

        }




    }

    private void processCreateTask(DataTable obj, DataTable team, string strUrl)
    {
       
        List<String> ls = new List<string>();
        DataTable lstProjectTeamMember = team;
        List<NotificationCustomChange> lsNotificationTask = new List<NotificationCustomChange>();
       
            foreach (DataRow r in obj.Rows)
        {

            if (!String.IsNullOrEmpty(r["TeamMember"].ToString()))
            {
                ls = new List<string>();
                ls = r["TeamMember"].ToString().Split(';').ToList();
                String subject = r["Account"].ToString() + " - Equip ID " + r["EquipmentID"].ToString() + "Test Type " + r["TestType"].ToString() + " Alert";
                String contend = Convert.ToString(HttpContext.Current.Session["username"].ToString()) + " has edited " + r["label"].ToString() + " for " + r["EquipmentName"].ToString() + " and " + r["EquipmentDesc"].ToString();

                foreach (string item in ls)
                {
                    string[] arr = item.Split('_');
                    if (arr.Length > 2)
                    {
                        if (arr[2] == "1")
                        {

                            if (arr[0] == "6")
                            {


                                var changedItem = lstProjectTeamMember.Select("memberkey='" + arr[0] + "_" + arr[1] + "'").FirstOrDefault();
                                if (changedItem != null)
                                {
                                    var role = changedItem["RoleName"].ToString();
                                    var projTeamUsers = lstProjectTeamMember.Select("RoleName='" + role.Trim() + "'").ToList();
                                    foreach (var projUser in projTeamUsers)
                                    {
                                         CreateTaskOnWorkflowChange(Convert.ToInt32(r["RolID"]), Convert.ToInt32(r["EquipmentID"]), r["LocName"].ToString(), subject, contend,strUrl, (string)projUser["fUser"], (string)projUser["email"]);
                                    }
                                }


                            }
                            else
                            {

                                var changedItem = lstProjectTeamMember.Select("memberkey='" + arr[0] + "_" + arr[1] + "'").FirstOrDefault();
                                if (changedItem != null)
                                {
                                    CreateTaskOnWorkflowChange(Convert.ToInt32(r["RolID"]), Convert.ToInt32(r["EquipmentID"]), r["LocName"].ToString(), subject, contend, strUrl, (string)changedItem["fUser"], (string)changedItem["email"]);
                                }


                            }

                        }
                    }

                }
            }

        }
        
        
    }

    private String CreateTaskOnWorkflowChange(int RolID, int equiID, String LocName, string strSubject, string strRemarks, string strUrl, string assignedTo, string strMailTo = "")
    {
        BL_Customer objBL_Customer = new BL_Customer();
        var objCustomer = new Customer();
        objCustomer.ConnConfig = Convert.ToString(HttpContext.Current.Session["config"].ToString());
        objCustomer.ROL = RolID;
        objCustomer.DueDate = DateTime.Now;
        objCustomer.TimeDue = Convert.ToDateTime("01/01/1900 " + DateTime.Now.ToShortTimeString());
        objCustomer.Subject = strSubject;
        objCustomer.Remarks = strRemarks;
        objCustomer.AssignedTo = assignedTo;
        double dblDuration = 0.5;
        objCustomer.Duration = dblDuration;
        objCustomer.Name =Convert.ToString(HttpContext.Current.Session["Username"].ToString());
        //objProp_Customer.Contact = txtContact.Text;
        objCustomer.Status = 0;//Open
        objCustomer.Resolution = "";
        objCustomer.LastUpdateUser = Convert.ToString(HttpContext.Current.Session["Username"].ToString());
        objCustomer.Category = "To Do";
        objCustomer.IsAlert = true;

        try
        {
            objCustomer.TaskID = 0;
            objCustomer.Mode = 0;
            objCustomer.Screen = "Equipment";

            objCustomer.Ref = Convert.ToInt32(equiID);
            objBL_Customer.AddTask(objCustomer);

            #region Thomas: Send email with a appointment to login user 
            if (objCustomer.IsAlert)
            {
                // Create
                BusinessEntity.User objPropUser = new BusinessEntity.User();
                BL_User objBL_User = new BL_User();
                objPropUser.ConnConfig = Convert.ToString(HttpContext.Current.Session["config"].ToString());
                objPropUser.Username = assignedTo;

                var mailTo = string.Empty;
                if (!string.IsNullOrEmpty(strMailTo))
                {
                    mailTo = strMailTo;
                }
                else
                {
                    mailTo = objBL_User.getUserEmail(objPropUser);
                }

                if (!string.IsNullOrEmpty(mailTo))
                {
                    Mail mail = new Mail();
                    try
                    {
                        var uri = strUrl + "/addTask?uid=" + objCustomer.TaskID.ToString();
                        foreach (var toaddress in mailTo.Split(new[] { ";", "," }, StringSplitOptions.RemoveEmptyEntries))
                        {
                            mail.To.Add(toaddress.Trim());
                        }
                        //mail.To.Add(mailTo);
                        mail.From = WebBaseUtility.GetFromEmailAddress();
                        //mail.Title = "Task Appointment";
                        mail.Title = LocName + ": " + objCustomer.Subject;

                        StringBuilder stringBuilder = new StringBuilder();
                        stringBuilder.AppendFormat("Dear {0}<br><br>", objCustomer.AssignedTo);
                        stringBuilder.Append("You are receiving an appointment task from MOM-->Sales-->Tasks<br><br>");
                        stringBuilder.AppendFormat("Location Name: {0}<br>", LocName);

                        stringBuilder.AppendFormat("Subject: {0}<br>", objCustomer.Subject);
                        stringBuilder.AppendFormat("Description: {0}<br>", objCustomer.Remarks);
                        stringBuilder.AppendFormat("Due on: {0} {1}<br><br>", objCustomer.DueDate.ToString("MM/dd/yyyy"), DateTime.Now.ToShortTimeString());
                        stringBuilder.Append("Attached files is a task appointment assigned to you.<br>");
                        stringBuilder.Append("To add this appointment to your calendar, please open and save it<br><br>");
                        stringBuilder.AppendFormat("<a href={0}>{0}</a><br><br>", uri);
                        stringBuilder.Append("Thanks");

                        mail.Text = stringBuilder.ToString();

                        //todo
                        WebBaseUtility.UpdateMailConfigurationFromLoginUser(mail);
                        var apSubject = string.Format("Task name: {0}", LocName);

                        StringBuilder apBody = new StringBuilder();
                        var _strRemarks = objCustomer.Remarks.Replace("\r\n", "=0D=0A").Replace("\n", "=0D=0A");
                        apBody.AppendFormat("{0}.=0D=0A", _strRemarks);
                        apBody.AppendFormat("Due on: {0} {1}. =0D=0A ", objCustomer.DueDate.ToString("MM/dd/yyyy"), DateTime.Now.ToShortTimeString());
                        apBody.Append("Attached files is a task appointment assigned to you.  =0D=0A");
                        apBody.Append("To add this appointment to your calendar, please open and save it.=0D=0A");
                        apBody.Append("Thanks");


                        var strStartDate = string.Format("{0} {1}", objCustomer.DueDate.ToString("MM/dd/yyyy"), DateTime.Now.ToShortTimeString());
                        var apStart = Convert.ToDateTime(strStartDate);
                        var apEnd = apStart.AddHours(objCustomer.Duration);
                        //todo
                        var icsAttachmentContentsStr = WebBaseUtility.CreateICSAttachmentCalendarStr(apSubject
                            , apBody.ToString()
                            , LocName
                            , apStart
                            , apEnd
                            , 60
                            );
                        var myByteArray = System.Text.Encoding.UTF8.GetBytes(icsAttachmentContentsStr);
                        mail.attachmentBytes = myByteArray;
                        mail.FileName = "TaskAppointment.ics";
                        mail.Send();
                    }
                    catch (Exception ex)
                    {

                        string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(ex.Message);
                        return str;
                       // ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, theme : 'noty_theme_default',  closable : true});", true);
                    }
                }
            }
            #endregion
            return "";
        }
        catch (Exception ex)
        {

            string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(ex.Message);
            return str;
            //ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, theme : 'noty_theme_default',  closable : true});", true);
        }


    }
    [System.Web.Services.WebMethod(EnableSession = true)]
    public string ServiceGetTicketByLocation(string prefixText, int loc)
    {
        string str;
        JavaScriptSerializer sr = new JavaScriptSerializer();
        List<Dictionary<object, object>> dictListEval = new List<Dictionary<object, object>>();

        DataSet ds = new DataSet();
        objMapData.ConnConfig = Session["config"].ToString();
        objMapData.LocID = loc;
        ds = objBL_MapData.GetTicketbyLocation(objMapData);

        if (prefixText == "")
        {
            
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                //List<string> txtItems = new List<string>();
                //String dbValues;

                //foreach (DataRow row in dt.Rows)
                //{
                //    dbValues = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(row["ID"].ToString(), row["ID"].ToString());
                //    txtItems.Add(dbValues);
                //}
                //return txtItems.ToArray();

                dictListEval = objGeneral.RowsToDictionary(dt);
                str = sr.Serialize(dictListEval);
                return str;
            }
            else
            {
                return null;

            }
            
        }
        else
        {
           
            if(ds.Tables[0].Select("Convert(ID,'System.String') like '" + prefixText + "%'").Count() > 0)
            {
                
                DataTable dt = ds.Tables[0].Select("Convert(ID,'System.String') like '" + prefixText + "%'").CopyToDataTable();
                //List<string> txtItems = new List<string>();
                //String dbValues;

                //foreach (DataRow row in dt.Rows)
                //{
                //    dbValues = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(row["ID"].ToString(), row["ID"].ToString());
                //    txtItems.Add(dbValues);
                //}
                //return txtItems.ToArray();
               
                dictListEval = objGeneral.RowsToDictionary(dt);
                str = sr.Serialize(dictListEval);
                return str;
            }
            else
            {
                return null;
            }
        }
    }

    /// <summary>
    /// Get Opportunity
    /// </summary>
    /// <param name="prefixText"></param>
    /// <param name="con"></param>
    /// <param name="custID"></param>
    /// <returns></returns>
    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string GetOpportunitiesForProjectLinking(string prefixText, int projectID)
    {
        User objPropUser = new User();
        BL_User objBL_User = new BL_User();

        string str;
        JavaScriptSerializer sr = new JavaScriptSerializer();
        List<Dictionary<object, object>> dictListEval = new List<Dictionary<object, object>>();

        DataSet ds = new DataSet();
        objPropUser.SearchValue = prefixText;
        objPropUser.ConnConfig = HttpContext.Current.Session["config"].ToString(); ;
        objPropUser.JobId = projectID;
        
        ds = objBL_User.GetOpportunitiesForProjectLinking(objPropUser);
        dictListEval = objGeneral.RowsToDictionary(ds.Tables[0]);
        str = sr.Serialize(dictListEval);
        return str;
    }

    /// <summary>
    /// Get Opportunity
    /// </summary>
    /// <param name="prefixText"></param>
    /// <param name="con"></param>
    /// <param name="custID"></param>
    /// <returns></returns>
    [WebMethod(EnableSession = true)]
    public string[] GetOpportunitiesForProjectLinkingNew(string prefixText, int count, string contextKey)
    {
        User objPropUser = new User();
        BL_User objBL_User = new BL_User();
        int projectID = 0;
        //string str;
        //JavaScriptSerializer sr = new JavaScriptSerializer();
        //List<Dictionary<object, object>> dictListEval = new List<Dictionary<object, object>>();
        if (!string.IsNullOrEmpty(contextKey))
        {
            projectID = Convert.ToInt32(contextKey);
        }

        DataSet ds = new DataSet();
        objPropUser.SearchValue = prefixText;
        objPropUser.ConnConfig = HttpContext.Current.Session["config"].ToString(); ;
        objPropUser.JobId = projectID;

        ds = objBL_User.GetOpportunitiesForProjectLinking(objPropUser);
        DataTable dt = ds.Tables[0];

        List<string> txtItems = new List<string>();
        String dbValues;

        foreach (DataRow row in dt.Rows)
        {
            var desc = row["value"].ToString()+ ", Type: " + row["Type"].ToString() + ", Customer: " + row["CusName"].ToString() + ", Location: " + row["LocName"].ToString() + ", Opportunity: " + row["label"].ToString();
            if (desc.Length > 150) desc = desc.Substring(0, 150) + " ...";
            dbValues = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(desc, row["value"].ToString());
            txtItems.Add(dbValues);
        }

        return txtItems.ToArray();
    }
}

