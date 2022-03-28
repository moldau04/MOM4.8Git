using BusinessEntity;
using BusinessLayer;
using BusinessLayer.Billing;
using BusinessLayer.Schedule;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;
//using Telerik.Web.UI;

/// <summary>
/// Summary description for AccountAutoFill
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class AccountAutoFill : System.Web.Services.WebService
{
    GeneralFunctions objGeneral = new GeneralFunctions();

    Wage _objWage = new Wage();
    BL_User _objBLUser = new BL_User();
    BL_Customer _objBLCustomer = new BL_Customer();
    BL_Job _objBLJob = new BL_Job();

    public AccountAutoFill()
    {

    }
    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string GetAccountNameJE(string prefixText, string con)
    {
        Chart _objChart = new Chart();
        BL_Chart _objBLChart = new BL_Chart();

        string str;
        JavaScriptSerializer sr = new JavaScriptSerializer();
        List<Dictionary<object, object>> dictListEval = new List<Dictionary<object, object>>();

        DataSet ds = new DataSet();
        _objChart.SearchValue = prefixText;
        _objChart.ConnConfig = HttpContext.Current.Session["config"].ToString();
        ds = _objBLChart.GetAutoFillAccountJE(_objChart);
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].NewRow();
            dr["value"] = 0;
            dr["acct"] = "";
            dr["label"] = " < Add New > ";
            dr["BankID"] = 0;
            ds.Tables[0].Rows.InsertAt(dr, 0);
        }

        dictListEval = objGeneral.RowsToDictionary(ds.Tables[0]);
        str = sr.Serialize(dictListEval);
        return str;
    }
    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string ManageBillCodeEdit(string lblIndex)    
    {
        String _Index = lblIndex.Replace("\n", "").Replace(" ", "");
        DataTable dt = (DataTable)HttpContext.Current.Session["Bills"];
        DataRow dr = dt.Rows[Convert.ToInt32(_Index)];
        //EditManageBillCodeModel data = new EditManageBillCodeModel();
        //data.ID = dr["ID"].ToString();
        //data.TRID = dr["TRID"].ToString();
        //data.Ref = dr["Ref"].ToString();
        //data.PostingDate = Convert.ToDateTime(dr["PostingDate"].ToString()).ToShortDateString();
        //data.Date = Convert.ToDateTime(dr["Date"].ToString()).ToShortDateString();
        //data.Due = Convert.ToDateTime(dr["Due"].ToString()).ToShortDateString();
        //data.Batch = dr["Batch"].ToString();
        //data.lblIndex = lblIndex;
        //return data;

        string str;
        JavaScriptSerializer sr = new JavaScriptSerializer();
        List<Dictionary<object, object>> dictListEval = new List<Dictionary<object, object>>();
        DataTable ds = new DataTable();
        ds.Columns.Add("ID", typeof(string));
        ds.Columns.Add("TRID", typeof(string));
        ds.Columns.Add("Ref", typeof(string));
        ds.Columns.Add("PostingDate", typeof(string));
        ds.Columns.Add("Date", typeof(string));
        ds.Columns.Add("Due", typeof(string));
        ds.Columns.Add("Batch", typeof(string));
        ds.Columns.Add("lblIndex", typeof(string));
        DataRow drr = ds.NewRow();
        drr["ID"] = dr["ID"].ToString();
        drr["TRID"] = dr["TRID"].ToString();
        drr["Ref"] = dr["Ref"].ToString();
        drr["PostingDate"] = Convert.ToDateTime(dr["PostingDate"].ToString()).ToShortDateString();
        drr["Date"] = Convert.ToDateTime(dr["Date"].ToString()).ToShortDateString();
        drr["Due"] = Convert.ToDateTime(dr["Due"].ToString()).ToShortDateString();
        drr["Batch"] = dr["Batch"].ToString();
        drr["lblIndex"] = lblIndex;
        ds.Rows.Add(drr);
        dictListEval = objGeneral.RowsToDictionary(ds);
        str = sr.Serialize(dictListEval);
        return str;

    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string GetAccountName(string prefixText, string con)
    {
        Chart _objChart = new Chart();
        BL_Chart _objBLChart = new BL_Chart();

        string str;
        JavaScriptSerializer sr = new JavaScriptSerializer();
        List<Dictionary<object, object>> dictListEval = new List<Dictionary<object, object>>();

        DataSet ds = new DataSet();
        _objChart.SearchValue = prefixText;
        _objChart.ConnConfig = HttpContext.Current.Session["config"].ToString();
        ds = _objBLChart.GetAutoFillAccount(_objChart);
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].NewRow();
            dr["value"] = 0;
            dr["acct"] = "";
            dr["label"] = " < Add New > ";
            dr["BankID"] = 0;
            ds.Tables[0].Rows.InsertAt(dr, 0);
        }

        dictListEval = objGeneral.RowsToDictionary(ds.Tables[0]);
        str = sr.Serialize(dictListEval);
        return str;
    }
    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string spGetAccountSearchAP(string prefixText, string con)
    {
        Chart _objChart = new Chart();
        BL_Chart _objBLChart = new BL_Chart();

        string str;
        JavaScriptSerializer sr = new JavaScriptSerializer();
        List<Dictionary<object, object>> dictListEval = new List<Dictionary<object, object>>();

        DataSet ds = new DataSet();
        _objChart.SearchValue = prefixText;
        _objChart.ConnConfig = HttpContext.Current.Session["config"].ToString();
        ds = _objBLChart.spGetAccountSearchAP(_objChart);
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].NewRow();
            dr["value"] = 0;
            dr["acct"] = "";
            dr["label"] = " < Add New > ";
            dr["BankID"] = 0;
            ds.Tables[0].Rows.InsertAt(dr, 0);
        }

        dictListEval = objGeneral.RowsToDictionary(ds.Tables[0]);
        str = sr.Serialize(dictListEval);
        return str;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string VoidCheckEdit(string lblIndex)
    {                          

        string str;
        JavaScriptSerializer sr = new JavaScriptSerializer();
        List<Dictionary<object, object>> dictListEval = new List<Dictionary<object, object>>();

        String _Index = lblIndex.Replace("\n", "").Replace(" ", "");
        DataTable dt = (DataTable)HttpContext.Current.Session["Checks"];
        DataRow dr = dt.AsEnumerable().Where(x => x.Field<int>("ID") == Convert.ToInt32(_Index)).FirstOrDefault();//dt.Rows[Convert.ToInt32(_Index)];
               

        DataTable dtt = GetTable();
        DataRow drr = dtt.NewRow();
        drr["ID"] = dr["ID"].ToString();
        drr["Sel"] = dr["Sel"].ToString();
        drr["Ref"] = dr["Ref"].ToString();
        drr["VoidDate"] = DateTime.Now.Date.ToString("MM/dd/yyyy");
        drr["fDate"] = dr["fDate"].ToString();
        drr["Bank"] = dr["Bank"].ToString();
        drr["lblIndex"] = lblIndex;
        dtt.Rows.Add(drr);

        dictListEval = objGeneral.RowsToDictionary(dtt);
        str = sr.Serialize(dictListEval);
        return str;

    }
    public DataTable GetTable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("ID", typeof(string));
        dt.Columns.Add("Sel", typeof(string));
        dt.Columns.Add("Ref", typeof(string));
        dt.Columns.Add("VoidDate", typeof(string));
        dt.Columns.Add("fDate", typeof(string));
        dt.Columns.Add("Bank", typeof(string));
        dt.Columns.Add("lblIndex", typeof(string));

        return dt;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string GetWarehouseName(string prefixText, string con, String InvID, string isShowAll="yes")
    {
        InvWarehouse _objInvWarehouse = new InvWarehouse();
        BL_Inventory _objBLInventory = new BL_Inventory();
        prefixText=prefixText.Replace("'", " ");
        string str;
        JavaScriptSerializer sr = new JavaScriptSerializer();
        List<Dictionary<object, object>> dictListEval = new List<Dictionary<object, object>>();

        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        //_objInvWarehouse.SearchValue = prefixText;
        //_objInvWarehouse.InvID = Convert.ToInt32(InvID);
        //ds = _objBLInventory.GetAutoFillWarehouse(_objInvWarehouse);
        //if (ds.Tables[0].Rows.Count > 0)
        //{
        //    DataRow dr = ds.Tables[0].NewRow();
        //    dr["ID"] = 0;
        //    dr["WarehouseID"] = 0;
        //    dr["WarehouseName"] = " < Add New > ";
        //    ds.Tables[0].Rows.InsertAt(dr, 0);
        //}
        isShowAll = "NO";
        if (!string.IsNullOrEmpty(InvID) && !InvID.Equals("0"))
        {
            _objInvWarehouse.SearchValue = prefixText;
            _objInvWarehouse.InvID = Convert.ToInt32(InvID);
            _objInvWarehouse.UserID = Convert.ToInt32(System.Web.HttpContext.Current.Session["UserID"].ToString());
            if (Session["CmpChkDefault"].ToString() == "1")
            {
                _objInvWarehouse.EN = 1;
            }
            else
            {
                _objInvWarehouse.EN = 0;
            }
            ds = _objBLInventory.GetAutoFillWarehouse(_objInvWarehouse);
            dt = ds.Tables[0];
            if (isShowAll != "yes")
            {
                var temp = dt.AsEnumerable().Where(t => t.Field<Int32>("ID") != 0);
                if (temp.Count() > 0)
                {
                    dt = temp.CopyToDataTable();
                }
                else
                {
                    dt = new DataTable();
                    dt.Columns.Add("WarehouseID");
                    dt.Columns.Add("WarehouseName");
                    var row = dt.NewRow();
                    row["WarehouseID"] = 0;
                    row["WarehouseName"] = "No Record Found!";
                    dt.Rows.InsertAt(row, 0);
                }
            }
        }
        if (ds.Tables.Count.Equals(0))
        {
            dt = new DataTable();
            dt.Columns.Add("WarehouseID");
            dt.Columns.Add("WarehouseName");
            var row = dt.NewRow();
            row["WarehouseID"] = 0;
            row["WarehouseName"] = "No Record Found!";
            dt.Rows.InsertAt(row, 0);
        }



        dictListEval = objGeneral.RowsToDictionary(dt);
        str = sr.Serialize(dictListEval);
        return str;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string GetWarehouseLocation(string prefixText, string con, String WarehouseID)
    {



        BL_User objBL_User = new BL_User();
        BusinessEntity.User objProp_User = new BusinessEntity.User();

        string str;
        JavaScriptSerializer sr = new JavaScriptSerializer();
        List<Dictionary<object, object>> dictListEval = new List<Dictionary<object, object>>();

        DataSet ds = new DataSet();






        // _objInvWarehouse.SearchValue = prefixText;
        objProp_User.ConnConfig = Session["config"].ToString();
        objProp_User.WarehouseID = WarehouseID;

        ds = objBL_User.GetWareHouseLocation(objProp_User);
        //if (ds.Tables[0].Rows.Count == 0)
        //{
        //    DataRow dr = ds.Tables[0].NewRow();
        //    dr["ID"] = 0;
        //    dr["Name"] = " < No Location > ";

        //    ds.Tables[0].Rows.InsertAt(dr, 0);
        //}

        dictListEval = objGeneral.RowsToDictionary(ds.Tables[0]);
        str = sr.Serialize(dictListEval);
        return str;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string GetLocationOnHand(String InvID, String WarehouseID, String locationID)
    {

        IWarehouseLocAdj _objIWarehouseLocAdj = new IWarehouseLocAdj();
        BL_Inventory _objBLInventory = new BL_Inventory();



        string str;
        JavaScriptSerializer sr = new JavaScriptSerializer();
        List<Dictionary<object, object>> dictListEval = new List<Dictionary<object, object>>();

        DataSet ds = new DataSet();


        _objIWarehouseLocAdj.InvID = Convert.ToInt32(InvID);
        _objIWarehouseLocAdj.WarehouseID = WarehouseID;
        _objIWarehouseLocAdj.locationID = Convert.ToInt32(locationID);

        ds = _objBLInventory.GetAutoFillOnHandBalance(_objIWarehouseLocAdj);


        dictListEval = objGeneral.RowsToDictionary(ds.Tables[0]);
        str = sr.Serialize(dictListEval);
        return str;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string GetInventory(string term)
    {
        string strOut = string.Empty;
        JavaScriptSerializer sr = new JavaScriptSerializer();
        Dictionary<string, object> dictionary = new Dictionary<string, object>();
        List<Dictionary<object, object>> dictionaryList = new List<Dictionary<object, object>>();
        DataSet dt = null;
        BL_Inventory objBL_JsonInventory = new BL_Inventory();
        BusinessEntity.Inventory objProp_JsonInventory = new BusinessEntity.Inventory();
        GeneralFunctions objGeneral = new GeneralFunctions();
        objProp_JsonInventory.ConnConfig = Convert.ToString(HttpContext.Current.Session["config"]);

        try
        {
            dt = objBL_JsonInventory.GetInvItems(objProp_JsonInventory, term);


            if (dt != null)
            {
                if (dt.Tables[0].Rows.Count > 0)
                {
                    dictionaryList = objGeneral.RowsToDictionary(dt.Tables[0]);

                }
            }


            strOut = sr.Serialize(dictionaryList);
        }


        catch (Exception ex)
        {
            dictionary.Add("Success", "0");
            dictionary.Add("ErrMsg", ex.Message);
            dictionary.Add("timestamp", System.DateTime.Now.ToString());

            strOut = sr.Serialize(dictionary);
        }
        return strOut;
    }

    //[WebMethod(EnableSession = true)]
    //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    //public string GetPhase(string jobID)
    //{
    //    Transaction _objTrans = new Transaction();
    //    BL_JournalEntry _objBLJe = new BL_JournalEntry();

    //    string str;
    //    JavaScriptSerializer sr = new JavaScriptSerializer();
    //    List<Dictionary<object, object>> dictListEval = new List<Dictionary<object, object>>();

    //    DataSet _ds = new DataSet();
    //    //_objTrans.ConnConfig = HttpContext.Current.Session["config"].ToString();
    //    _objTrans.ConnConfig = HttpContext.Current.Session["config"].ToString();
    //    _objTrans.JobInt = Convert.ToInt32(jobID);
    //    _ds = _objBLJe.GetPhaseByJobID(_objTrans);

    //    DataTable dt = new DataTable();

    //    dt.Columns.Add("ID");
    //    dt.Columns.Add("Desc");
    //    dt.Columns.Add("PhaseType");

    //    foreach (DataRow dr in _ds.Tables[0].Rows)
    //    {
    //        var row = dt.NewRow();
    //        row["ID"] = dr["ID"].ToString();
    //        row["Desc"] = dr["fDesc"].ToString();
    //        row["PhaseType"] = dr["Type"].ToString();
    //        dt.Rows.Add(row);
    //    }

    //    dictListEval = objGeneral.RowsToDictionary(dt);
    //    str = sr.Serialize(dictListEval);
    //    return str;
    //}

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string GetPhase(string jobID, string prefixText)
    {
        Transaction _objTrans = new Transaction();
        BL_JournalEntry objBL_Je = new BL_JournalEntry();

        General _objPropGeneral = new General();
        BL_General _objBLGeneral = new BL_General();

        _objPropGeneral.ConnConfig = Session["config"].ToString();
        DataSet _dsCustom = _objBLGeneral.getCustomField(_objPropGeneral, "InvGL");
        Boolean TrackingInventory = false;
        if (_dsCustom.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow _dr in _dsCustom.Tables[0].Rows)
            {
                if (!string.IsNullOrEmpty(_dr["Label"].ToString()) && _dr["Label"].ToString() != "0")
                {
                    TrackingInventory = Convert.ToBoolean(_dr["Label"]);
                }
            }
        }

        string str;
        JavaScriptSerializer sr = new JavaScriptSerializer();
        List<Dictionary<object, object>> dictListEval = new List<Dictionary<object, object>>();

        DataSet _ds = new DataSet();
        DataTable dt = new DataTable();
        _objTrans.ConnConfig = HttpContext.Current.Session["config"].ToString();
        if (!string.IsNullOrEmpty(jobID) && !jobID.Equals("0"))
        {
            _objTrans.JobInt = Convert.ToInt32(jobID);
            _objTrans.Type = 1;
            _objTrans.SearchValue = prefixText;
            _ds = objBL_Je.GetPhaseByJobId(_objTrans); // get job item type by job
            dt = _ds.Tables[0];                                       //  dt = _ds.Tables[0];

            foreach (DataRow drr in dt.Rows)
            {
                if (Convert.ToString(drr["TypeName"]).ToLower() == "labor")
                {
                    dt.Rows.Remove(drr);
                    break;
                }
            }
            //
            if (TrackingInventory == false)
            {
                foreach (DataRow drr in dt.Rows)
                {
                    if (Convert.ToString(drr["TypeName"]).ToLower() == "inventory")
                    {
                        dt.Rows.Remove(drr);
                        break;
                    }
                }
            }
        }
        else
        {
            if (string.IsNullOrEmpty(jobID))
            {
                if (TrackingInventory == true)
                {
                    dt = new DataTable();

                    _objPropGeneral.ConnConfig = Session["config"].ToString();
                    DataSet _dsDefaultAccount = _objBLGeneral.getInvDefaultAcct(_objPropGeneral);
                    DataSet _dsInventoryType = _objBLGeneral.GetInventoryByTypeName(_objPropGeneral);
                    if (_dsInventoryType.Tables[0].Rows.Count > 0)
                    {
                        if (_dsDefaultAccount.Tables[0].Rows.Count > 0)
                        {
                            dt.Columns.Add("Type");
                            dt.Columns.Add("TypeName");                            
                            dt.Columns.Add("AcctID");
                            dt.Columns.Add("AcctName");
                            dt.Columns.Add("GroupName");
                            dt.Columns.Add("Code");
                            dt.Columns.Add("CodeDesc");
                            dt.Columns.Add("Desc");
                            var row = dt.NewRow();
                            row["Type"] = Convert.ToInt32(_dsInventoryType.Tables[0].Rows[0]["ID"]);
                            row["TypeName"] = Convert.ToString(_dsInventoryType.Tables[0].Rows[0]["Type"]);
                            row["AcctID"] = Convert.ToString(_dsDefaultAccount.Tables[0].Rows[0]["ID"]);
                            row["AcctName"] = Convert.ToString(_dsDefaultAccount.Tables[0].Rows[0]["Acct"]);
                            dt.Rows.Add(row);
                        }
                        else
                        {
                            dt.Columns.Add("Type");
                            dt.Columns.Add("TypeName");
                            dt.Columns.Add("GroupName");
                            dt.Columns.Add("Code");
                            dt.Columns.Add("CodeDesc");
                            dt.Columns.Add("Desc");
                            var row = dt.NewRow();
                            row["Type"] = Convert.ToInt32(_dsInventoryType.Tables[0].Rows[0]["ID"]);
                            row["TypeName"] = Convert.ToString(_dsInventoryType.Tables[0].Rows[0]["Type"]);
                            dt.Rows.Add(row);
                        }
                    }
                }
                else
                {
                    //dt = new DataTable();
                    //dt.Columns.Add("Type");
                    //dt.Columns.Add("TypeName");
                    //dt.Columns.Add("GroupName");
                    //dt.Columns.Add("Code");
                    //dt.Columns.Add("CodeDesc");
                    //dt.Columns.Add("Desc");
                    //var row = dt.NewRow();
                    //row["Type"] = 1;
                    //row["TypeName"] = "Materials";

                    //dt.Rows.Add(row);

                    
                }
            }
            else
            {
                dt = new DataTable();
                dt.Columns.Add("Type");
                dt.Columns.Add("TypeName");
                dt.Columns.Add("GroupName");
                dt.Columns.Add("Code");
                dt.Columns.Add("CodeDesc");
                dt.Columns.Add("Desc");
                var row = dt.NewRow();
                row["Type"] = 0;
                row["TypeName"] = "No Record Found!";
            
                dt.Rows.Add(row);
            }
        }
        if (_ds.Tables.Count.Equals(0) && dt.Rows.Count <= 0)
        {
            dt = new DataTable();
            dt.Columns.Add("Type");
            dt.Columns.Add("TypeName");
            dt.Columns.Add("GroupName");
            dt.Columns.Add("Code");
            dt.Columns.Add("CodeDesc");
            dt.Columns.Add("Desc");
            var row = dt.NewRow();
            row["Type"] = 0;
            row["TypeName"] = "No Record Found!";
            dt.Rows.Add(row);
        }
        if (!String.IsNullOrEmpty(jobID))
        {
            DataRow[] rows = dt.Select("TypeName = 'Inventory'");
            foreach (DataRow row in rows)
                dt.Rows.Remove(row);
        }
        dictListEval = objGeneral.RowsToDictionary(dt);
        str = sr.Serialize(dictListEval);

        return str;
    }
    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string getUseTax()
    {
        BL_User objBL_User = new BL_User();
        BusinessEntity.User objProp_User = new BusinessEntity.User();

        string str;
        JavaScriptSerializer sr = new JavaScriptSerializer();
        List<Dictionary<object, object>> dictListEval = new List<Dictionary<object, object>>();

        DataSet ds = new DataSet();
        objProp_User.ConnConfig = Session["config"].ToString();

        ds = objBL_User.GetAllUseTax(objProp_User);

        DataTable dt = new DataTable();

        //dt.Columns.Add("ID");
        dt.Columns.Add("Name");
        dt.Columns.Add("fdesc");
        dt.Columns.Add("Rate");
        dt.Columns.Add("GL");

        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            var row = dt.NewRow();
            //row["ID"] = dr["ID"].ToString();
            row["Name"] = dr["Name"].ToString();
            row["fdesc"] = dr["fdesc"].ToString();
            row["Rate"] = dr["Rate"].ToString();
            row["GL"] = dr["GL"].ToString();
            dt.Rows.Add(row);
        }

        dictListEval = objGeneral.RowsToDictionary(dt); ;
        str = sr.Serialize(dictListEval);
        return str;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string getUseTaxSearch(String prefixText)
    {
        BL_User objBL_User = new BL_User();
        BusinessEntity.User objProp_User = new BusinessEntity.User();

        string str;
        JavaScriptSerializer sr = new JavaScriptSerializer();
        List<Dictionary<object, object>> dictListEval = new List<Dictionary<object, object>>();

        DataSet ds = new DataSet();
        objProp_User.ConnConfig = Session["config"].ToString();
        objProp_User.SearchValue = prefixText;
        ds = objBL_User.GetAllUseTaxSearch(objProp_User);

        DataTable dt = new DataTable();

        //dt.Columns.Add("ID");
        dt.Columns.Add("Name");
        dt.Columns.Add("fdesc");
        dt.Columns.Add("Rate");
        dt.Columns.Add("GL");

        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            var row = dt.NewRow();
            //row["ID"] = dr["ID"].ToString();
            row["Name"] = dr["Name"].ToString();
            row["fdesc"] = dr["fdesc"].ToString();
            row["Rate"] = dr["Rate"].ToString();
            row["GL"] = dr["GL"].ToString();
            dt.Rows.Add(row);
        }

        dictListEval = objGeneral.RowsToDictionary(dt); ;
        str = sr.Serialize(dictListEval);
        return str;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string getSaleTaxSearch(String prefixText)
    {
        BL_User objBL_User = new BL_User();
        BusinessEntity.User objProp_User = new BusinessEntity.User();

        string str;
        JavaScriptSerializer sr = new JavaScriptSerializer();
        List<Dictionary<object, object>> dictListEval = new List<Dictionary<object, object>>();

        //DataSet ds = new DataSet();
        //objProp_User.ConnConfig = Session["config"].ToString();
        //objProp_User.SearchValue = prefixText;
        //ds = objBL_User.GetAllUseTaxSearch(objProp_User);

        DataSet ds = new DataSet();
        objProp_User.ConnConfig = Session["config"].ToString();
        ds = objBL_User.getSTax(objProp_User);

        DataTable dt = new DataTable();

        //dt.Columns.Add("ID");
        dt.Columns.Add("Name");
        dt.Columns.Add("fdesc");
        dt.Columns.Add("Rate");
        dt.Columns.Add("GL");
        dt.Columns.Add("Type");
        dt.Columns.Add("State");
        

        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            var row = dt.NewRow();
            //row["ID"] = dr["ID"].ToString();
            row["Name"] = dr["Name"].ToString();
            row["fdesc"] = dr["fdesc"].ToString();
            row["Rate"] = dr["Rate"].ToString();
            row["GL"] = dr["GL"].ToString();
            row["Type"] = dr["Type"].ToString();
            row["State"] = dr["State"].ToString();
            dt.Rows.Add(row);
        }

        DataView dv = new DataView(dt);
        dv.RowFilter = "state='' OR state='" + prefixText.ToString().Trim() + "'"; // query example = "id = 10"
        dt = dv.ToTable();

        dictListEval = objGeneral.RowsToDictionary(dt); ;
        str = sr.Serialize(dictListEval);
        return str;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string getSaleTaxSearchByName(String prefixText)
    {
        BL_User objBL_User = new BL_User();
        BusinessEntity.User objProp_User = new BusinessEntity.User();

        string str;
        JavaScriptSerializer sr = new JavaScriptSerializer();
        List<Dictionary<object, object>> dictListEval = new List<Dictionary<object, object>>();

        //DataSet ds = new DataSet();
        //objProp_User.ConnConfig = Session["config"].ToString();
        //objProp_User.SearchValue = prefixText;
        //ds = objBL_User.GetAllUseTaxSearch(objProp_User);

        DataSet ds = new DataSet();
        objProp_User.ConnConfig = Session["config"].ToString();
        ds = objBL_User.getSTax(objProp_User);

        DataTable dt = new DataTable();

        //dt.Columns.Add("ID");
        dt.Columns.Add("Name");
        dt.Columns.Add("fdesc");
        dt.Columns.Add("Rate");
        dt.Columns.Add("GL");
        dt.Columns.Add("Type");
        dt.Columns.Add("State");


        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            var row = dt.NewRow();
            //row["ID"] = dr["ID"].ToString();
            row["Name"] = dr["Name"].ToString();
            row["fdesc"] = dr["fdesc"].ToString();
            row["Rate"] = dr["Rate"].ToString();
            row["GL"] = dr["GL"].ToString();
            row["Type"] = dr["Type"].ToString();
            row["State"] = dr["State"].ToString();
            dt.Rows.Add(row);
        }

        DataView dv = new DataView(dt);
        dv.RowFilter = "Name='' OR Name='" + prefixText.ToString().Trim() + "'"; // query example = "id = 10"
        dt = dv.ToTable();

        dictListEval = objGeneral.RowsToDictionary(dt); ;
        str = sr.Serialize(dictListEval);
        return str;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string GetIsSalesTaxAPBill()
    {

        string str;
        JavaScriptSerializer sr = new JavaScriptSerializer();
        List<Dictionary<object, object>> dictListEval = new List<Dictionary<object, object>>();

        BusinessEntity.User objProp_User = new BusinessEntity.User();
        DataSet ds = new DataSet();
        objProp_User.ConnConfig = Session["config"].ToString();
        ds = _objBLUser.getControl(objProp_User);
        dictListEval = objGeneral.RowsToDictionary(ds.Tables[0]);
        str = sr.Serialize(dictListEval);
        return str;


    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string GetJobLocations(string prefixText, string IsJob, string con)
    {



        Transaction _objTrans = new Transaction();
        BL_JournalEntry _objBLJournal = new BL_JournalEntry();

        string str;
        JavaScriptSerializer sr = new JavaScriptSerializer();
        List<Dictionary<object, object>> dictListEval = new List<Dictionary<object, object>>();


        DataSet ds = new DataSet();
        _objTrans.SearchValue = prefixText;
        _objTrans.ConnConfig = HttpContext.Current.Session["config"].ToString();
        _objTrans.UserID = Convert.ToInt32(System.Web.HttpContext.Current.Session["UserID"].ToString());

        if (System.Web.HttpContext.Current.Session["CmpChkDefault"].ToString() == "1")
        {
            _objTrans.EN = 1;
        }
        else
        {
            _objTrans.EN = 0;
        }
        if (!string.IsNullOrEmpty(IsJob))
        {
            _objTrans.IsJob = Convert.ToBoolean(IsJob);

        }
        ds = _objBLJournal.GetJobsLoc(_objTrans);

        dictListEval = objGeneral.RowsToDictionary(ds.Tables[0]);
        str = sr.Serialize(dictListEval);
        return str;
    }
    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string GetVendorName(string prefixText, string con)
    {
        Vendor _objVendor = new Vendor();
        BL_Vendor _objBLVendor = new BL_Vendor();

        string str;
        JavaScriptSerializer sr = new JavaScriptSerializer();
        List<Dictionary<object, object>> dictListEval = new List<Dictionary<object, object>>();

        DataSet ds = new DataSet();
        _objVendor.SearchValue = prefixText;
        _objVendor.ConnConfig = HttpContext.Current.Session["config"].ToString();
        if (Session["CmpChkDefault"].ToString() == "1")
        {
            _objVendor.EN = 1;
        }
        else
        {
            _objVendor.EN = 0;
        }
        ds = _objBLVendor.GetVendorSearch(_objVendor);
        //if (ds.Tables[0].Rows.Count > 0)
        //{
        //    DataRow dr = ds.Tables[0].NewRow();
        //    dr["ID"] = 0;
        //    dr["Name"] = "";
        //    ds.Tables[0].Rows.InsertAt(dr, 0);
        //}
        if (ds.Tables[0].Rows.Count.Equals(0))
        {
            DataRow dr = ds.Tables[0].NewRow();
            dr["ID"] = 0;
            dr["Name"] = "No Record Found!";
            ds.Tables[0].Rows.InsertAt(dr, 0);
        }

        dictListEval = objGeneral.RowsToDictionary(ds.Tables[0]);
        str = sr.Serialize(dictListEval);
        return str;
    }

    [WebMethod(EnableSession = true)]
    public string[] GetVendor(string prefixText, int count, string contextKey)
    {
        Vendor _objVendor = new Vendor();
        BL_Vendor _objBLVendor = new BL_Vendor();

        DataSet ds = new DataSet();
        _objVendor.SearchValue = prefixText;
        _objVendor.ConnConfig = HttpContext.Current.Session["config"].ToString();
        ds = _objBLVendor.GetVendorSearch(_objVendor);

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
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string GetVendorNameProject(string prefixText, string con)
    {
        Vendor _objVendor = new Vendor();
        BL_Vendor _objBLVendor = new BL_Vendor();

        string str;
        JavaScriptSerializer sr = new JavaScriptSerializer();
        List<Dictionary<object, object>> dictListEval = new List<Dictionary<object, object>>();

        DataSet ds = new DataSet();
        _objVendor.SearchValue = prefixText;
        _objVendor.ConnConfig = HttpContext.Current.Session["config"].ToString();
        ds = _objBLVendor.GetVendorSearchProject(_objVendor);
        //if (ds.Tables[0].Rows.Count > 0)
        //{
        //    DataRow dr = ds.Tables[0].NewRow();
        //    dr["ID"] = 0;
        //    dr["Name"] = "";
        //    ds.Tables[0].Rows.InsertAt(dr, 0);
        //}
        if (ds.Tables[0].Rows.Count.Equals(0))
        {
            DataRow dr = ds.Tables[0].NewRow();
            dr["ID"] = 0;
            dr["Name"] = "No Record Found!";
            ds.Tables[0].Rows.InsertAt(dr, 0);
        }

        dictListEval = objGeneral.RowsToDictionary(ds.Tables[0]);
        str = sr.Serialize(dictListEval);
        return str;
    }



    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string GetProjectMaterialType(string prefixText, string bTypeID)
    {

        BL_Job _objBL_Job = new BL_Job();

        string str;
        JavaScriptSerializer sr = new JavaScriptSerializer();
        List<Dictionary<object, object>> dictListEval = new List<Dictionary<object, object>>();

        DataSet ds = new DataSet();

        String ConnConfig = HttpContext.Current.Session["config"].ToString();

        if (bTypeID == "1")
        {
            ds = _objBL_Job.GetInventoryItemProject(ConnConfig, prefixText);
        }
        else if (bTypeID == "2")
        {
            ds = _objBL_Job.GetLabourMaterialProject(ConnConfig, prefixText);
        }
        else
        {
            //do nothing.
        }

        if (ds.Tables.Count <= 0)
        {
            DataTable table = new DataTable();
            table.Columns.Add("MatItem", typeof(int));
            table.Columns.Add("MatDesc", typeof(string));
            table.Columns.Add("fDesc", typeof(string));
            table.Rows.Add(0, "No Record Found!", "");
            ds.Tables.Add(table);
        }

        dictListEval = objGeneral.RowsToDictionary(ds.Tables[0]);
        str = sr.Serialize(dictListEval);
        return str;
    }


    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string GetInvService(string prefixText, string con)
    {
        JobT _objJob = new JobT();
        BL_Job _objBLJob = new BL_Job();

        string str;
        JavaScriptSerializer sr = new JavaScriptSerializer();
        List<Dictionary<object, object>> dictListEval = new List<Dictionary<object, object>>();
        DataSet ds = new DataSet();
        _objJob.ConnConfig = HttpContext.Current.Session["config"].ToString();
        _objJob.SearchValue = prefixText;
        ds = _objBLJob.GetInvService(_objJob);
        if (ds.Tables[0].Rows.Count.Equals(0))
        {
            DataRow dr = ds.Tables[0].NewRow();
            dr["value"] = 0;
            dr["label"] = "No Record Found!";
            ds.Tables[0].Rows.InsertAt(dr, 0);
        }

        dictListEval = objGeneral.RowsToDictionary(ds.Tables[0]);
        str = sr.Serialize(dictListEval);
        return str;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string GetInvService_TypeZero(string prefixText, string con)
    {
        JobT _objJob = new JobT();
        BL_Job _objBLJob = new BL_Job();

        string str;
        JavaScriptSerializer sr = new JavaScriptSerializer();
        List<Dictionary<object, object>> dictListEval = new List<Dictionary<object, object>>();
        DataSet ds = new DataSet();
        _objJob.ConnConfig = HttpContext.Current.Session["config"].ToString();
        ds = _objBLJob.GetInvService_TypeZero(_objJob);
        if (ds.Tables[0].Rows.Count.Equals(0))
        {
            DataRow dr = ds.Tables[0].NewRow();
            dr["value"] = 0;
            dr["label"] = "No Record Found!";
            ds.Tables[0].Rows.InsertAt(dr, 0);
        }

        dictListEval = objGeneral.RowsToDictionary(ds.Tables[0]);
        str = sr.Serialize(dictListEval);
        return str;
    }
    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string GetWageUserScreen(string prefixText, string con)
    { 
        string str;
        JavaScriptSerializer sr = new JavaScriptSerializer();
        List<Dictionary<object, object>> dictListEval = new List<Dictionary<object, object>>();
        DataSet ds = new DataSet();
        _objWage.ConnConfig = HttpContext.Current.Session["config"].ToString();
        ds = _objBLUser.GetAllWageRate(HttpContext.Current.Session["config"].ToString(), prefixText);
        if (ds.Tables[0].Rows.Count.Equals(0))
        {
            DataRow dr = ds.Tables[0].NewRow();
            dr["ID"] = 0;
            dr["NAME"] = "No Record Found!";
            ds.Tables[0].Rows.InsertAt(dr, 0);
        }

        dictListEval = objGeneral.RowsToDictionary(ds.Tables[0]);
        str = sr.Serialize(dictListEval);
        return str;
    }
    

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string GetWage(string prefixText, string con)
    {
        JobT _objJob = new JobT();
        BL_Job _objBLJob = new BL_Job();

        string str;
        JavaScriptSerializer sr = new JavaScriptSerializer();
        List<Dictionary<object, object>> dictListEval = new List<Dictionary<object, object>>();
        DataSet ds = new DataSet();
        _objWage.ConnConfig = HttpContext.Current.Session["config"].ToString();
        ds = _objBLUser.GetAllWage(_objWage);
        if (ds.Tables[0].Rows.Count.Equals(0))
        {
            DataRow dr = ds.Tables[0].NewRow();
            dr["value"] = 0;
            dr["label"] = "No Record Found!";
            ds.Tables[0].Rows.InsertAt(dr, 0);
        }

        dictListEval = objGeneral.RowsToDictionary(ds.Tables[0]);
        str = sr.Serialize(dictListEval);
        return str;
    }
    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string GetJobCode(string prefixText, string con)
    {
        JobT _objJob = new JobT();
        BL_Job _objBLJob = new BL_Job();

        string str;
        JavaScriptSerializer sr = new JavaScriptSerializer();
        List<Dictionary<object, object>> dictListEval = new List<Dictionary<object, object>>();
        DataSet ds = new DataSet();
        _objJob.ConnConfig = HttpContext.Current.Session["config"].ToString();
        ds = _objBLJob.GetJobCode(_objJob);
        if (ds.Tables[0].Rows.Count.Equals(0))
        {
            DataRow dr = ds.Tables[0].NewRow();
            dr["value"] = 0;
            dr["label"] = "No Record Found!";
            dr["CodeDesc"] = "";
            ds.Tables[0].Rows.InsertAt(dr, 0); 
        }      

        dictListEval = objGeneral.RowsToDictionary(ds.Tables[0]);
        str = sr.Serialize(dictListEval);
        return str;
    }



    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string GetJobCodeByDeptName(string prefixText, string DeptName)
    {
               BL_Job _objBLJob = new BL_Job();

               int DeptID = 0;

               string ConnConfig = HttpContext.Current.Session["config"].ToString();

               DeptID= _objBLJob.GetDeptID(ConnConfig, DeptName);

               return GetJobCodeByDeptID(prefixText, DeptID.ToString()); 

    }




    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string GetJobCodeByDeptID(string prefixText, string DeptID)
    {
        try
        {
            JobT _objJob = new JobT();
            BL_Job _objBLJob = new BL_Job();

            string str;

            JavaScriptSerializer sr = new JavaScriptSerializer();

            List<Dictionary<object, object>> dictListEval = new List<Dictionary<object, object>>();

            DataSet ds = new DataSet();

            _objJob.ConnConfig = HttpContext.Current.Session["config"].ToString();

            _objJob.SearchValue = prefixText.Trim();

            ds = _objBLJob.GetJobCodebyDept(_objJob, int.Parse(DeptID));

            if (ds.Tables[0].Rows.Count.Equals(0))
            {
                DataRow dr = ds.Tables[0].NewRow();
                dr["value"] = 0;
                dr["label"] = "No Record Found!";
                dr["CodeDesc"] = "";
                ds.Tables[0].Rows.InsertAt(dr, 0);
            }

            dictListEval = objGeneral.RowsToDictionary(ds.Tables[0]);
            str = sr.Serialize(dictListEval);
            return str;
        }
        catch { return null; }
    }


    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string GetLabor(string prefixText)
    {
       
        User objPropUser = new User();

        objPropUser.ConnConfig = Session["config"].ToString();
        objPropUser.SearchValue = prefixText.Trim();
        ////////////////////// 

        string str;
        JavaScriptSerializer sr = new JavaScriptSerializer();
        List<Dictionary<object, object>> dictListEval = new List<Dictionary<object, object>>();

        DataSet ds = new DataSet();

        ds =new BL_User().getWage(objPropUser);
 

        dictListEval = objGeneral.RowsToDictionary(ds.Tables[0]);
        str = sr.Serialize(dictListEval);
        return str;
    }
  


    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string GetGroup(string prefixText, string projectId, string con)
    {
        JobT _objJob = new JobT();
        BL_Job _objBLJob = new BL_Job();
        _objJob.ID = Convert.ToInt32(projectId);
        _objJob.SearchValue = prefixText;

        string str;
        JavaScriptSerializer sr = new JavaScriptSerializer();
        List<Dictionary<object, object>> dictListEval = new List<Dictionary<object, object>>();

        if (_objJob.ID != 0)
        {
            DataSet ds = new DataSet();
            _objJob.ConnConfig = HttpContext.Current.Session["config"].ToString();
            ds = _objBLJob.GetGroup(_objJob);
            if (ds.Tables[0].Rows.Count.Equals(0))
            {
                DataRow dr = ds.Tables[0].NewRow();
                dr["value"] = 0;
                dr["label"] = "No Record Found!";
                ds.Tables[0].Rows.InsertAt(dr, 0);
            }

            dictListEval = objGeneral.RowsToDictionary(ds.Tables[0]);
        }
        else
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("label", typeof(string)));
            dt.Columns.Add(new DataColumn("value", typeof(Int32)));
            if (Session["NewProjGroupList"] != null)
            {
                var projGroupList = (DataTable)Session["NewProjGroupList"];
                if (projGroupList.Rows.Count > 0)
                {
                    foreach (DataRow row in projGroupList.Rows)
                    {
                        DataRow dr = dt.NewRow();
                        dr["value"] = row["Id"];
                        dr["label"] = row["GroupName"];
                        dt.Rows.Add(dr);
                    }

                }
                else
                {
                    DataRow dr = dt.NewRow();
                    dr["value"] = 0;
                    dr["label"] = "No Record Found!";
                    dt.Rows.Add(dr);
                }
            }
            else
            {
                DataRow dr = dt.NewRow();
                dr["value"] = 0;
                dr["label"] = "No Record Found!";
                dt.Rows.Add(dr);
            }

            dictListEval = objGeneral.RowsToDictionary(dt);
        }
        
        str = sr.Serialize(dictListEval);
        return str;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string GetLocGroupNotInProj(string prefixText, string projectId, string locId, string con)
    {
        JobT _objJob = new JobT();
        BL_Job _objBLJob = new BL_Job();
        _objJob.ID = Convert.ToInt32(locId);
        _objJob.Job = Convert.ToInt32(projectId);
        _objJob.SearchValue = prefixText;

        string str;
        JavaScriptSerializer sr = new JavaScriptSerializer();
        List<Dictionary<object, object>> dictListEval = new List<Dictionary<object, object>>();

        //if (_objJob.ID != 0)
        {
            DataSet ds = new DataSet();
            _objJob.ConnConfig = HttpContext.Current.Session["config"].ToString();
            ds = _objBLJob.GetLocGroupNotInProj(_objJob);
            if (ds.Tables[0].Rows.Count.Equals(0))
            {
                DataRow dr = ds.Tables[0].NewRow();
                dr["value"] = 0;
                dr["label"] = "No Record Found!";
                ds.Tables[0].Rows.InsertAt(dr, 0);
            }

            dictListEval = objGeneral.RowsToDictionary(ds.Tables[0]);
        }
        //else
        //{
        //    DataTable dt = new DataTable();
        //    dt.Columns.Add(new DataColumn("label", typeof(string)));
        //    dt.Columns.Add(new DataColumn("value", typeof(Int32)));
        //    if (Session["NewProjGroupList"] != null)
        //    {
        //        var projGroupList = (DataTable)Session["NewProjGroupList"];
        //        if (projGroupList.Rows.Count > 0)
        //        {
        //            foreach (DataRow row in projGroupList.Rows)
        //            {
        //                DataRow dr = dt.NewRow();
        //                dr["value"] = row["Id"];
        //                dr["label"] = row["GroupName"];
        //                dt.Rows.Add(dr);
        //            }

        //        }
        //        else
        //        {
        //            DataRow dr = dt.NewRow();
        //            dr["value"] = 0;
        //            dr["label"] = "No Record Found!";
        //            dt.Rows.Add(dr);
        //        }
        //    }
        //    else
        //    {
        //        DataRow dr = dt.NewRow();
        //        dr["value"] = 0;
        //        dr["label"] = "No Record Found!";
        //        dt.Rows.Add(dr);
        //    }

        //    dictListEval = objGeneral.RowsToDictionary(dt);
        //}

        str = sr.Serialize(dictListEval);
        return str;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string GetUnitMeasure(string prefixText, string con)
    {
        JobT _objJob = new JobT();
        BL_Job _objBLJob = new BL_Job();

        string str;
        JavaScriptSerializer sr = new JavaScriptSerializer();
        List<Dictionary<object, object>> dictListEval = new List<Dictionary<object, object>>();
        DataSet ds = new DataSet();
        _objJob.ConnConfig = HttpContext.Current.Session["config"].ToString();
        ds = _objBLJob.GetAllUM(_objJob);
        if (ds.Tables[0].Rows.Count.Equals(0))
        {
            DataRow dr = ds.Tables[0].NewRow();
            dr["value"] = 0;
            dr["label"] = "No Record Found!";
            ds.Tables[0].Rows.InsertAt(dr, 0);
        }
        //DataRow dr1 = ds.Tables[0].NewRow();
        //dr1["value"] = "0";
        //dr1["label"] = " < Add New > ";
        //ds.Tables[0].Rows.InsertAt(dr1, 0);

        dictListEval = objGeneral.RowsToDictionary(ds.Tables[0]);
        str = sr.Serialize(dictListEval);
        return str;
    }
    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public bool CheckNumValidNew(string checkno, string bank,string paytype)
    {
        CD _objCD = new CD();
        BL_Bills _objBLBills = new BL_Bills();
        _objCD.IsExistCheckNo = false;
        if (!string.IsNullOrEmpty(checkno) && !string.IsNullOrEmpty(bank))
        {
            _objCD.ConnConfig = HttpContext.Current.Session["config"].ToString();
            _objCD.Ref = Convert.ToInt32(checkno);
            _objCD.Bank = Convert.ToInt32(bank);
            _objCD.Vendor = Convert.ToInt32(paytype);
            _objCD.IsExistCheckNo = _objBLBills.IsExistCheckNum(_objCD);
        }

        return _objCD.IsExistCheckNo;
    }
    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public bool CheckNumValid(string checkno, string bank)
    {
        CD _objCD = new CD();
        BL_Bills _objBLBills = new BL_Bills();
        _objCD.IsExistCheckNo = false;
        if (!string.IsNullOrEmpty(checkno) && !string.IsNullOrEmpty(bank))
        {
            _objCD.ConnConfig = HttpContext.Current.Session["config"].ToString();
            _objCD.Ref = long.Parse (checkno);
            _objCD.Bank = Convert.ToInt32(bank);
            _objCD.IsExistCheckNo = _objBLBills.IsExistCheckNum(_objCD);
        }

        return _objCD.IsExistCheckNo;
    }
    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public bool CheckNumValidOnEditNew(string checkno, string bank, string cdId,string paytype)
    {
        CD _objCD = new CD();
        BL_Bills _objBLBills = new BL_Bills();
        _objCD.IsExistCheckNo = false;
        if (!string.IsNullOrEmpty(checkno) && !string.IsNullOrEmpty(bank))
        {
            _objCD.ConnConfig = HttpContext.Current.Session["config"].ToString();
            _objCD.ID = Convert.ToInt32(cdId);
            _objCD.Ref = long.Parse(checkno);
            _objCD.Bank = Convert.ToInt32(bank);
            _objCD.Vendor = Convert.ToInt32(paytype);
            _objCD.IsExistCheckNo = _objBLBills.IsExistCheckNumOnEdit(_objCD);
        }

        return _objCD.IsExistCheckNo;
    }
    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public bool CheckNumValidOnEdit(string checkno, string bank, string cdId)
    {
        CD _objCD = new CD();
        BL_Bills _objBLBills = new BL_Bills();
        _objCD.IsExistCheckNo = false;
        if (!string.IsNullOrEmpty(checkno) && !string.IsNullOrEmpty(bank))
        {
            _objCD.ConnConfig = HttpContext.Current.Session["config"].ToString();
            _objCD.ID = Convert.ToInt32(cdId);
            _objCD.Ref = long.Parse(checkno);
            _objCD.Bank = Convert.ToInt32(bank);

            _objCD.IsExistCheckNo = _objBLBills.IsExistCheckNumOnEdit(_objCD);
        }

        return _objCD.IsExistCheckNo;
    }
    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string GetServiceType(string prefixText, string con)
    {
        JobT _objJob = new JobT();
        string str;
        JavaScriptSerializer sr = new JavaScriptSerializer();
        List<Dictionary<object, object>> dictListEval = new List<Dictionary<object, object>>();

        DataSet _dsService = new DataSet();
        _objJob.ConnConfig = HttpContext.Current.Session["config"].ToString();
        _dsService = _objBLJob.GetServiceType(_objJob);
        if (_dsService.Tables[0].Rows.Count.Equals(0))
        {
            DataRow dr = _dsService.Tables[0].NewRow();
            dr["value"] = 0;
            dr["label"] = "No Record Found!";
            _dsService.Tables[0].Rows.InsertAt(dr, 0);
        }

        dictListEval = objGeneral.RowsToDictionary(_dsService.Tables[0]);
        str = sr.Serialize(dictListEval);
        return str;
    }
    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string GetUsername(string prefixText, string con)
    {
        User _objUser = new User();
        BL_User _objBLUser = new BL_User();

        string str;
        JavaScriptSerializer sr = new JavaScriptSerializer();
        List<Dictionary<object, object>> dictListEval = new List<Dictionary<object, object>>();

        DataSet ds = new DataSet();
        _objUser.SearchValue = prefixText;
        _objUser.ConnConfig = HttpContext.Current.Session["config"].ToString();
        ds = _objBLUser.GetUserSearch(_objUser);

        dictListEval = objGeneral.RowsToDictionary(ds.Tables[0]);
        str = sr.Serialize(dictListEval);
        return str;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string GetUsernameOfUserRole(string prefixText, string con, string userRole)
    {
        User _objUser = new User();
        BL_User _objBLUser = new BL_User();

        string str;
        JavaScriptSerializer sr = new JavaScriptSerializer();
        List<Dictionary<object, object>> dictListEval = new List<Dictionary<object, object>>();

        DataSet ds = new DataSet();
        _objUser.SearchValue = prefixText;
        _objUser.ConnConfig = HttpContext.Current.Session["config"].ToString();
        ds = _objBLUser.GetUserSearch(_objUser, userRole);

        dictListEval = objGeneral.RowsToDictionary(ds.Tables[0]);
        str = sr.Serialize(dictListEval);
        return str;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string GetTeamMemberTitles(string prefixText, string con, string projectId)
    {
        User _objUser = new User();
        BL_User _objBLUser = new BL_User();
        int intProjectId = 0;
        if (!string.IsNullOrEmpty(projectId))
        {
            intProjectId = Convert.ToInt32(projectId);
        }
        string str;
        JavaScriptSerializer sr = new JavaScriptSerializer();
        List<Dictionary<object, object>> dictListEval = new List<Dictionary<object, object>>();

        DataSet ds = new DataSet();
        _objUser.SearchValue = prefixText;
        _objUser.JobId = intProjectId;
        _objUser.ConnConfig = HttpContext.Current.Session["config"].ToString();
        ds = _objBLUser.GetTeamMemberTitleSearch(_objUser);

        dictListEval = objGeneral.RowsToDictionary(ds.Tables[0]);
        str = sr.Serialize(dictListEval);
        return str;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string GetPOById(string prefixText)
    {
        PO objPO = new PO();
        BL_Bills objBLBills = new BL_Bills();

        string str;
        JavaScriptSerializer sr = new JavaScriptSerializer();
        List<Dictionary<object, object>> dictListEval = new List<Dictionary<object, object>>();
        //Dictionary<String, object> dictListEval = new Dictionary<string, object>();
        DataSet ds = new DataSet();
        try
        {
            objPO.ConnConfig = HttpContext.Current.Session["config"].ToString();
            objPO.POID = Convert.ToInt32(prefixText);
            //ds = objBLBills.GetPOItemByPO(objPO);
            #region Company Check
            objPO.UserID = Convert.ToInt32(Session["UserID"].ToString());
            if (Convert.ToString(Session["CmpChkDefault"]) == "1")
            {
                objPO.EN = 1;
            }
            else
            {
                objPO.EN = 0;
            }
            #endregion
            ds = objBLBills.GetPOById(objPO);

            dictListEval = objGeneral.RowsToDictionary(ds.Tables[0]);
            //dictListEval = objGeneral.ToJson(ds);

            str = sr.Serialize(dictListEval);
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return str;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string GetPOByIdAjax(string prefixText)
    {
        PO objPO = new PO();
        BL_Bills objBLBills = new BL_Bills();

        string str;
        JavaScriptSerializer sr = new JavaScriptSerializer();
        List<Dictionary<object, object>> dictListEval = new List<Dictionary<object, object>>();
        //Dictionary<String, object> dictListEval = new Dictionary<string, object>();
        DataSet ds = new DataSet();
        try
        {
            objPO.ConnConfig = HttpContext.Current.Session["config"].ToString();
            objPO.POID = string.IsNullOrEmpty(prefixText) ? 0 : Convert.ToInt32(prefixText);
            //ds = objBLBills.GetPOItemByPO(objPO);
            #region Company Check
            objPO.UserID = Convert.ToInt32(Session["UserID"].ToString());
            if (Convert.ToString(Session["CmpChkDefault"]) == "1")
            {
                objPO.EN = 1;
            }
            else
            {
                objPO.EN = 0;
            }
            #endregion
            ds = objBLBills.GetPOByIdAjax(objPO);

            dictListEval = objGeneral.RowsToDictionary(ds.Tables[0]);
            //dictListEval = objGeneral.ToJson(ds);

            str = sr.Serialize(dictListEval);
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return str;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string GetChartByAcct(string prefixText)
    {
        Chart objChart = new Chart();
        BL_Chart objBLChart = new BL_Chart();

        string str;
        JavaScriptSerializer sr = new JavaScriptSerializer();
        List<Dictionary<object, object>> dictListEval = new List<Dictionary<object, object>>();

        DataSet ds = new DataSet();
        try
        {
            int index = prefixText.IndexOf(" -");
            if (index > 0)
                prefixText = prefixText.Substring(0, index);
            objChart.ConnConfig = HttpContext.Current.Session["config"].ToString();
            objChart.Acct = prefixText;
            //ds = objBLBills.GetPOItemByPO(objPO);
            ds = objBLChart.GetChartByAcct(objChart);

            dictListEval = objGeneral.RowsToDictionary(ds.Tables[0]);
            str = sr.Serialize(dictListEval);
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return str;
    }
    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string GetGLbyVendor(string vendor)
    {
        Vendor objVendor = new Vendor();
        BL_Vendor objBLVendor = new BL_Vendor();

        string str;
        JavaScriptSerializer sr = new JavaScriptSerializer();
        List<Dictionary<object, object>> dictListEval = new List<Dictionary<object, object>>();

        DataSet ds = new DataSet();
        try
        {
            objVendor.ConnConfig = HttpContext.Current.Session["config"].ToString();
            objVendor.ID = Convert.ToInt32(vendor);
            ds = objBLVendor.GetVendorGLById(objVendor);

            dictListEval = objGeneral.RowsToDictionary(ds.Tables[0]);
            str = sr.Serialize(dictListEval);
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return str;
    }
    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string GetPO(string prefixText, string vendor)
    {
        PO objPO = new PO();
        BL_Bills objBLBills = new BL_Bills();

        string str;
        JavaScriptSerializer sr = new JavaScriptSerializer();
        List<Dictionary<object, object>> dictListEval = new List<Dictionary<object, object>>();

        DataSet ds = new DataSet();
        try
        {
            objPO.ConnConfig = HttpContext.Current.Session["config"].ToString();
            if (!string.IsNullOrEmpty(prefixText))
            {
                objPO.POID = Convert.ToInt32(prefixText);
            }
            if (!string.IsNullOrEmpty(vendor))
            {
                objPO.Vendor = Convert.ToInt32(vendor);
            }

            ds = objBLBills.GetPOList(objPO);
            if (ds.Tables[0].Rows.Count.Equals(0))
            {
                DataRow dr = ds.Tables[0].NewRow();
                dr["VendorName"] = "No Record Found!";
                ds.Tables[0].Rows.InsertAt(dr, 0);
            }

            dictListEval = objGeneral.RowsToDictionary(ds.Tables[0]);
            str = sr.Serialize(dictListEval);
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return str;
    }
    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string GetReceivePOList(string prefixText, string vendor)
    {
        PO objPO = new PO();
        BL_Bills objBLBills = new BL_Bills();

        string str;
        JavaScriptSerializer sr = new JavaScriptSerializer();
        List<Dictionary<object, object>> dictListEval = new List<Dictionary<object, object>>();

        DataSet ds = new DataSet();
        try
        {
            objPO.ConnConfig = HttpContext.Current.Session["config"].ToString();
            if (!string.IsNullOrEmpty(prefixText))
            {
                objPO.POID = Convert.ToInt32(prefixText);
            }
            if (!string.IsNullOrEmpty(vendor))
            {
                objPO.Vendor = Convert.ToInt32(vendor);
            }
            ds = objBLBills.GetReceivePOList(objPO);
            if (ds.Tables[0].Rows.Count.Equals(0))
            {
                DataRow dr = ds.Tables[0].NewRow();

                dr["ID"] = 0;
                dr["Value"] = "No Record Found!";
                ds.Tables[0].Rows.InsertAt(dr, 0);
            }

            dictListEval = objGeneral.RowsToDictionary(ds.Tables[0]);
            str = sr.Serialize(dictListEval);
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return str;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string GetReceivePOListSearch(string prefixText, string vendor, string PO)
    {
        PO objPO = new PO();
        BL_Bills objBLBills = new BL_Bills();

        string str;
        JavaScriptSerializer sr = new JavaScriptSerializer();
        List<Dictionary<object, object>> dictListEval = new List<Dictionary<object, object>>();

        DataSet ds = new DataSet();
        try
        {
            objPO.ConnConfig = HttpContext.Current.Session["config"].ToString();
            if (!string.IsNullOrEmpty(PO))
            {
                objPO.POID = Convert.ToInt32(PO);
            }
            if (!string.IsNullOrEmpty(vendor))
            {
                objPO.Vendor = Convert.ToInt32(vendor);
            }
            objPO.SearchValue = prefixText;
            ds = objBLBills.GetReceivePOListSearch(objPO);
            if (ds.Tables[0].Rows.Count.Equals(0))
            {
                DataRow dr = ds.Tables[0].NewRow();

                dr["ID"] = 0;
                dr["Value"] = "No Record Found!";
                ds.Tables[0].Rows.InsertAt(dr, 0);
            }

            dictListEval = objGeneral.RowsToDictionary(ds.Tables[0]);
            str = sr.Serialize(dictListEval);
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return str;
    }
    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string GetAutofillByUM(string prefixText)
    {
        JobT objJob = new JobT();
        BL_Job _objBLJob = new BL_Job();

        string str;
        JavaScriptSerializer sr = new JavaScriptSerializer();
        List<Dictionary<object, object>> dictListEval = new List<Dictionary<object, object>>();

        DataSet ds = new DataSet();
        try
        {
            objJob.ConnConfig = HttpContext.Current.Session["config"].ToString();
            objJob.UM = prefixText;
            ds = _objBLJob.GetDataByUM(objJob);

            dictListEval = objGeneral.RowsToDictionary(ds.Tables[0]);
            str = sr.Serialize(dictListEval);
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return str;
    }
    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string GetPhaseByItem(string typeId, string jobId, string prefixText)
    {
        BL_Job objBL_Job = new BL_Job();
        JobT objJob = new JobT();


        if (jobId == "")
        {
            jobId = "0";
        }

        string str;
        JavaScriptSerializer sr = new JavaScriptSerializer();
        List<Dictionary<object, object>> dictListEval = new List<Dictionary<object, object>>();

        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        objJob.ConnConfig = HttpContext.Current.Session["config"].ToString();
        if (!string.IsNullOrEmpty(typeId) && !typeId.Equals("0"))
        {
            objJob.Job = Convert.ToInt32(jobId);
            objJob.Type = Convert.ToInt16(typeId);
            objJob.SearchValue = prefixText;
            ds = objBL_Job.GetPhaseExpByJobType(objJob);
            dt = ds.Tables[0];
        }
        if (ds.Tables.Count.Equals(0))
        {
            dt = new DataTable();
            dt.Columns.Add("ItemID");
            dt.Columns.Add("ItemDesc");
            var row = dt.NewRow();
            row["ItemID"] = 0;
            row["ItemDesc"] = "No Record Found!";
            dt.Rows.InsertAt(row, 0);
        }

        DataTable fDT = RemoveDuplicateRows(dt, "ItemDesc");
        dictListEval = objGeneral.RowsToDictionary(dt);
        str = sr.Serialize(dictListEval);
        return str;
    }


    public DataTable RemoveDuplicateRows(DataTable dTable, string colName)
    {
        Hashtable hTable = new Hashtable();
        ArrayList duplicateList = new ArrayList();

        //Add list of all the unique item value to hashtable, which stores combination of key, value pair.
        //And add duplicate item value in arraylist.
        foreach (DataRow drow in dTable.Rows)
        {
            if (hTable.Contains(drow[colName]))
                duplicateList.Add(drow);
            else
                hTable.Add(drow[colName], string.Empty);
        }

        //Removing a list of duplicate items from datatable.
        foreach (DataRow dRow in duplicateList)
            dTable.Rows.Remove(dRow);

        //Datatable which contains unique records will be return as output.
        return dTable;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string IsItemOnHand(string INVitemID, string WareHouseID, string WHLocationID)
    {
        TicketI ticket = new TicketI();
        BL_Tickets _BL_Tickets = new BL_Tickets();
        ticket.ConnConfig = HttpContext.Current.Session["config"].ToString();
        string str;
        string result = "false";
        JavaScriptSerializer sr = new JavaScriptSerializer();
        List<Dictionary<object, object>> dictListEval = new List<Dictionary<object, object>>();

        DataSet ds = new DataSet();
        ticket.Item = Convert.ToInt32(INVitemID);
        ticket.WarehouseID = WareHouseID;
        int _WHLocationID = 0;
        int.TryParse(WHLocationID, out _WHLocationID);
        ticket.WHLocationID = _WHLocationID;
        ds = _BL_Tickets.IsItemOnHand(ticket);
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (ds.Tables[1].Rows.Count > 0)
            {
                result = ds.Tables[1].Rows[0]["LCost"].ToString();
            }
        }

        return result;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string GetPhaseByInventoryItem(string typeId, string jobId, string prefixText)
    {
        BL_Job objBL_Job = new BL_Job();
        JobT objJob = new JobT();


        if (jobId == "")
        {
            jobId = "0";
        }
        string str;
        JavaScriptSerializer sr = new JavaScriptSerializer();
        List<Dictionary<object, object>> dictListEval = new List<Dictionary<object, object>>();

        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        objJob.ConnConfig = HttpContext.Current.Session["config"].ToString();
        if (!string.IsNullOrEmpty(typeId) && !typeId.Equals("0"))
        {
            objJob.Job = Convert.ToInt32(jobId);
            objJob.Type = Convert.ToInt16(typeId);
            objJob.SearchValue = prefixText;
            ds = objBL_Job.GetPhaseExpByJobType(objJob);
            dt = ds.Tables[0];
        }
        if (ds.Tables.Count.Equals(0))
        {
            dt = new DataTable();
            dt.Columns.Add("ItemID");
            dt.Columns.Add("ItemDesc");
            var row = dt.NewRow();
            row["ItemID"] = 0;
            row["ItemDesc"] = "No Record Found!";
            dt.Rows.InsertAt(row, 0);
        }

        //DataTable fDT = RemoveDuplicateRows(dt, "ItemDesc");
        dictListEval = objGeneral.RowsToDictionary(dt);
        str = sr.Serialize(dictListEval);
        return str;
    }


    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string GetOpsqList(string jobId, string ItemID)
    {
        BL_Bills objBL_Bills = new BL_Bills();
        PO objPO = new PO();


        if (jobId == "")
        {
            jobId = "0";
        }
        string str;
        JavaScriptSerializer sr = new JavaScriptSerializer();
        List<Dictionary<object, object>> dictListEval = new List<Dictionary<object, object>>();

        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        objPO.ConnConfig = HttpContext.Current.Session["config"].ToString();

        objPO.jobID = Convert.ToInt32(jobId);
        objPO.ItemID = Convert.ToInt16(ItemID);

        ds = objBL_Bills.GetOpsqList(objPO);
        dt = ds.Tables[0];



        dictListEval = objGeneral.RowsToDictionary(dt);
        str = sr.Serialize(dictListEval);
        return str;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string GetPhaseExpByJobTypeOpSequence(string typeId, string jobId, string prefixText, String OpSq)
    {
        BL_Job objBL_Job = new BL_Job();
        JobT objJob = new JobT();


        if (jobId == "")
        {
            jobId = "0";
        }
        string str;
        JavaScriptSerializer sr = new JavaScriptSerializer();
        List<Dictionary<object, object>> dictListEval = new List<Dictionary<object, object>>();

        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        objJob.ConnConfig = HttpContext.Current.Session["config"].ToString();
        if (!string.IsNullOrEmpty(typeId) && !typeId.Equals("0"))
        {
            objJob.Job = Convert.ToInt32(jobId);
            objJob.Type = Convert.ToInt16(typeId);
            objJob.SearchValue = prefixText;
            objJob.Opsq = Convert.ToInt16(OpSq); ;
            ds = objBL_Job.GetPhaseExpByJobTypeOpSequence(objJob);
            dt = ds.Tables[0];
        }
        if (ds.Tables.Count.Equals(0))
        {
            dt = new DataTable();
            dt.Columns.Add("ItemID");
            dt.Columns.Add("ItemDesc");
            var row = dt.NewRow();
            row["ItemID"] = 0;
            row["ItemDesc"] = "No Record Found!";
            dt.Rows.InsertAt(row, 0);
        }

        DataTable fDT = RemoveDuplicateRows(dt, "ItemDesc");
        dictListEval = objGeneral.RowsToDictionary(dt);
        str = sr.Serialize(dictListEval);
        return str;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string GetAllPhase(string jobID, string prefixText)
    {
        Transaction _objTrans = new Transaction();
        BL_JournalEntry objBL_Je = new BL_JournalEntry();

        string str;
        JavaScriptSerializer sr = new JavaScriptSerializer();
        List<Dictionary<object, object>> dictListEval = new List<Dictionary<object, object>>();

        DataSet _ds = new DataSet();
        _objTrans.ConnConfig = HttpContext.Current.Session["config"].ToString();
        if (!string.IsNullOrEmpty(jobID))
        {
            _objTrans.JobInt = Convert.ToInt32(jobID);
        }
        _objTrans.Type = 1;
        _objTrans.SearchValue = prefixText;
        _ds = objBL_Je.GetAllPhaseByJobID(_objTrans);

        DataTable dt = new DataTable();
        dt.Columns.Add("ID");
        dt.Columns.Add("Desc");
        dt.Columns.Add("PhaseType");
        dt.Columns.Add("Code");
        dt.Columns.Add("Line");
        dt.Columns.Add("LocName");
        dt.Columns.Add("Job");
        dt.Columns.Add("JobName");
        dt.Columns.Add("bomtypeID");
        dt.Columns.Add("bomType");
        dt.Columns.Add("GroupName");

        foreach (DataRow dr in _ds.Tables[0].Rows)
        {
            var row = dt.NewRow();
            row["ID"] = dr["ID"].ToString();
            row["Desc"] = dr["fDesc"].ToString();
            row["PhaseType"] = dr["Type"].ToString();
            row["Code"] = dr["Code"].ToString();
            row["Line"] = dr["Line"].ToString();
            row["LocName"] = dr["LocName"].ToString();
            row["Job"] = dr["Job"].ToString();
            row["JobName"] = dr["JobName"].ToString();
            row["bomtypeID"] = dr["bomtypeID"].ToString();
            row["bomType"] = dr["bomType"].ToString();
            row["GroupName"] = dr["GroupName"];
            dt.Rows.Add(row);
        }

        dictListEval = objGeneral.RowsToDictionary(dt);
        str = sr.Serialize(dictListEval);
        return str;
    }
    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string GetAutoFillPhase(string prefixText)
    {
        JobT objJob = new JobT();
        BL_Job objBL_Job = new BL_Job();

        string str;
        JavaScriptSerializer sr = new JavaScriptSerializer();
        List<Dictionary<object, object>> dictListEval = new List<Dictionary<object, object>>();

        DataSet ds = new DataSet();
        try
        {
            objJob.ConnConfig = HttpContext.Current.Session["config"].ToString();
            objJob.TypeName = prefixText;
            ds = objBL_Job.GetBOMTByTypeName(objJob);
            if (prefixText == string.Empty)
            {
                ds.Tables[0].Clear();
            }
            dictListEval = objGeneral.RowsToDictionary(ds.Tables[0]);
            str = sr.Serialize(dictListEval);
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return str;
    }
    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string GetAutoFillItem(string prefixText, string typeId, string job)
    {
        JobT objJob = new JobT();
        BL_Job objBL_Job = new BL_Job();

        string str;
        JavaScriptSerializer sr = new JavaScriptSerializer();
        List<Dictionary<object, object>> dictListEval = new List<Dictionary<object, object>>();

        DataSet ds = new DataSet();
        try
        {
            if (string.IsNullOrEmpty(job)) job = "0";
            if (string.IsNullOrEmpty(typeId)) typeId = "0";

            objJob.ConnConfig = HttpContext.Current.Session["config"].ToString();
            objJob.Job = Convert.ToInt32(job);
            objJob.Type = Convert.ToInt16(typeId);
            objJob.SearchValue = prefixText;
            ds = objBL_Job.GetPhaseExpByJobType(objJob);
            if (prefixText == string.Empty)
            {
                ds.Tables[0].Clear();
            }
            DataTable dt = ds.Tables[0].Copy();

            if (dt.Rows.Count > 0)
            {
                dt = ds.Tables[0].AsEnumerable().Take(1).CopyToDataTable();
            }

            dictListEval = objGeneral.RowsToDictionary(dt);
            str = sr.Serialize(dictListEval);
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return str;
    }


    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string GetAutoFillItemPO(string prefixText, string typeId, string job)
    {
        JobT objJob = new JobT();
        BL_Job objBL_Job = new BL_Job();

        string str;
        JavaScriptSerializer sr = new JavaScriptSerializer();
        List<Dictionary<object, object>> dictListEval = new List<Dictionary<object, object>>();

        DataSet ds = new DataSet();
        try
        {
            if (!string.IsNullOrEmpty(typeId) && !typeId.Equals("0"))
            {
                if (string.IsNullOrEmpty(job)) job = "0";
                if (string.IsNullOrEmpty(typeId)) typeId = "0";

                objJob.ConnConfig = HttpContext.Current.Session["config"].ToString();
                objJob.Job = Convert.ToInt32(job);
                objJob.Type = Convert.ToInt16(typeId);
                objJob.SearchValue = prefixText;
                ds = objBL_Job.GetPhaseExpByJobType(objJob);
                if (prefixText == string.Empty)
                {
                    ds.Tables[0].Clear();
                }
                DataTable dt = ds.Tables[0].Copy();

                if (dt.Rows.Count > 0)
                {
                    dt = ds.Tables[0].AsEnumerable().Take(1).CopyToDataTable();
                }

                dictListEval = objGeneral.RowsToDictionary(dt);
                str = sr.Serialize(dictListEval);
            }
            else
            {
                str = "[]";
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return str;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string GetPhaseExpByJobTypePO(string prefixText, string typeId, string job)
    {
        JobT objJob = new JobT();
        BL_Job objBL_Job = new BL_Job();

        string str;
        JavaScriptSerializer sr = new JavaScriptSerializer();
        List<Dictionary<object, object>> dictListEval = new List<Dictionary<object, object>>();

        DataSet ds = new DataSet();
        try
        {
            if (!string.IsNullOrEmpty(typeId) && !typeId.Equals("0"))
            {
                if (string.IsNullOrEmpty(job)) job = "0";
                if (string.IsNullOrEmpty(typeId)) typeId = "0";

                objJob.ConnConfig = HttpContext.Current.Session["config"].ToString();
                objJob.Job = Convert.ToInt32(job);
                objJob.Type = Convert.ToInt16(typeId);
                objJob.SearchValue = prefixText;
                ds = objBL_Job.GetPhaseExpByJobTypePO(objJob);
                if (prefixText == string.Empty)
                {
                    ds.Tables[0].Clear();
                }
                DataTable dt = ds.Tables[0].Copy();

                if (dt.Rows.Count > 0)
                {
                    dt = ds.Tables[0].AsEnumerable().Take(1).CopyToDataTable();
                }

                dictListEval = objGeneral.RowsToDictionary(dt);
                str = sr.Serialize(dictListEval);
            }
            else
            {
                str = "[]";
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return str;
    }



    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string GetGLExpByProject(string Job)
    {
        JobT objJob = new JobT();
        BL_Job objBLJob = new BL_Job();

        string str;
        JavaScriptSerializer sr = new JavaScriptSerializer();
        List<Dictionary<object, object>> dictListEval = new List<Dictionary<object, object>>();

        DataSet ds = new DataSet();
        try
        {
            if (!string.IsNullOrEmpty(Job.ToString()))
            {
                objJob.ConnConfig = HttpContext.Current.Session["config"].ToString();
                objJob.Job = Convert.ToInt32(Job);
                ds = objBLJob.GetJobExpGLByJob(objJob);
            }
            dictListEval = objGeneral.RowsToDictionary(ds.Tables[0]);
            str = sr.Serialize(dictListEval);
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return str;
    }
    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string GetInvoiceNos(string prefixText, string Customer, string Loc)
    {
        BL_Deposit objBL_Deposit = new BL_Deposit();
        PaymentDetails objPayment = new PaymentDetails();

        string str;
        JavaScriptSerializer sr = new JavaScriptSerializer();
        List<Dictionary<object, object>> dictListEval = new List<Dictionary<object, object>>();

        DataSet ds = new DataSet();
        try
        {
            objPayment.ConnConfig = HttpContext.Current.Session["config"].ToString();

            objPayment.strInvoiceId = prefixText;
            if (!string.IsNullOrEmpty(Customer))
            {
                objPayment.Owner = Convert.ToInt32(Customer);
            }
            if (!string.IsNullOrEmpty(Loc))
            {
                objPayment.Loc = Convert.ToInt32(Loc);
            }
            ds = objBL_Deposit.GetInvoiceNos(objPayment);

            if (ds != null)
            {
                if (ds.Tables[0].Rows.Count.Equals(0))
                {
                    DataRow dr = ds.Tables[0].NewRow();
                    dr["Ref"] = 0;
                    ds.Tables[0].Rows.InsertAt(dr, 0);
                }
            }

            dictListEval = objGeneral.RowsToDictionary(ds.Tables[0]);
            str = sr.Serialize(dictListEval);
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return str;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string GetInvoiceNosChange(string prefixText, string Customer, string Loc)
    {
        BL_Deposit objBL_Deposit = new BL_Deposit();
        PaymentDetails objPayment = new PaymentDetails();

        string str;
        JavaScriptSerializer sr = new JavaScriptSerializer();
        List<Dictionary<object, object>> dictListEval = new List<Dictionary<object, object>>();

        DataSet ds = new DataSet();
        try
        {
            objPayment.ConnConfig = HttpContext.Current.Session["config"].ToString();

            objPayment.strInvoiceId = prefixText;
            if (!string.IsNullOrEmpty(Customer))
            {
                objPayment.Owner = Convert.ToInt32(Customer);
            }
            if (!string.IsNullOrEmpty(Loc))
            {
                objPayment.Loc = Convert.ToInt32(Loc);
            }
            ds = objBL_Deposit.GetInvoiceNosChange(objPayment);

            if (ds != null)
            {
                if (ds.Tables[0].Rows.Count.Equals(0))
                {
                    DataRow dr = ds.Tables[0].NewRow();
                    dr["Ref"] = 0;
                    ds.Tables[0].Rows.InsertAt(dr, 0);
                }
            }

            dictListEval = objGeneral.RowsToDictionary(ds.Tables[0]);
            str = sr.Serialize(dictListEval);
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return str;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string GetInventoryItemSearch(String prefixText)
    {
        JobT _objJob = new JobT();
        BL_Job objBL_Job = new BL_Job();

        string str;
        JavaScriptSerializer sr = new JavaScriptSerializer();
        List<Dictionary<object, object>> dictListEval = new List<Dictionary<object, object>>();

        DataSet ds = new DataSet();
        _objJob.ConnConfig = Session["config"].ToString();
        _objJob.SearchValue = prefixText;
        ds = objBL_Job.GetInventoryItemSearch(_objJob);

        //if (prefixText == string.Empty)
        //{
        //    ds.Tables[0].Clear();
        //}
        DataTable dt = ds.Tables[0].Copy();

        if (dt.Rows.Count > 0)
        {
            dt = ds.Tables[0].AsEnumerable().CopyToDataTable();
        }

        dictListEval = objGeneral.RowsToDictionary(dt);
        str = sr.Serialize(dictListEval);
        return str;
    }


    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string GetBillingCode(string prefixText, string jdata)
    {
        User objPropUser = new User();
        BL_User objBL_User = new BL_User();
        string str=""; 
        DataSet ds = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();
        objPropUser.Billingmodule = prefixText;

        ds =  new BL_BillCodes().GetAutoCompleteBillCodes(objPropUser,  prefixText); 
         
        List<Dictionary<object, object>> dictionary = new List<Dictionary<object, object>>();
        JavaScriptSerializer sr = new JavaScriptSerializer();

        if (ds.Tables[0].Rows.Count > 0)
        { 

        }
        else
        { 
            DataRow dr = ds.Tables[0].NewRow();
            ds.Tables[0].Rows.InsertAt(dr, 0);
        }

        dictionary = objGeneral.RowsToDictionary(ds.Tables[0]); 
        str = sr.Serialize(dictionary);


        return str;
    }


    [WebMethod(EnableSession = true)]
    public List<CheckListBoxItem> LoadProjectDepartments()
    {
        JobT objProp_job = new JobT();
        BL_Job objBL_Project = new BL_Job();
        objProp_job.ConnConfig = Session["config"].ToString();

        DataSet ds = new DataSet();
        ds = objBL_Project.GetJobType(objProp_job);
        List<CheckListBoxItem> lst = new List<CheckListBoxItem>();
        if (ds.Tables[0] != null)
        {
            foreach (DataRow item in ds.Tables[0].Rows)
            {
                lst.Add(new CheckListBoxItem() { Text = item["Type"].ToString() });
            }
        }
        return lst;
    }

    [WebMethod(EnableSession = true)]
    public List<CheckListBoxItem> LoadProjectRoutes()
    {
        JobT objProp_job = new JobT();
        BL_Job objBL_Project = new BL_Job();
        objProp_job.ConnConfig = Session["config"].ToString();

        DataSet ds = new DataSet();
        ds = objBL_Project.GetJobRoute(objProp_job);
        
        List<CheckListBoxItem> lst = new List<CheckListBoxItem>();
        if (ds.Tables[0] != null)
        {
            foreach (DataRow item in ds.Tables[0].Rows)
            {
                lst.Add(new CheckListBoxItem() { Text = item["Name1"].ToString() });
            }
        }
        return lst;
    }

    [WebMethod(EnableSession = true)]
    public List<CheckListBoxItem> LoadProjectStages()
    {
        //JobT objProp_job = new JobT();
        BL_Job objBL_Project = new BL_Job();
        //objProp_job.ConnConfig = Session["config"].ToString();

        DataSet ds = new DataSet();
        //ds = objBL_Project.GetJobStage(objProp_job);
        ds = objBL_Project.GetAllStageItems(Session["config"].ToString());

        List<CheckListBoxItem> lst = new List<CheckListBoxItem>();
        lst.Add(new CheckListBoxItem() { Text = "" });
        if (ds.Tables[0] != null)
        {
            foreach (DataRow item in ds.Tables[0].Rows)
            {
                lst.Add(new CheckListBoxItem() { Text = item["Stage"].ToString() });
            }
        }
        return lst;
    }

    public class CheckListBoxItem
    {
        public string Text { get; set; }
    }

    [WebMethod(EnableSession = true)]
    public List<CheckListBoxItem> LoadInvoiceStatus()
    {      
        List<CheckListBoxItem> lst = new List<CheckListBoxItem>();
        lst.Add(new CheckListBoxItem() { Text = "Open" });
        lst.Add(new CheckListBoxItem() { Text = "Paid" });
        lst.Add(new CheckListBoxItem() { Text = "Partially Paid" });
        lst.Add(new CheckListBoxItem() { Text = "Voided" });
        lst.Add(new CheckListBoxItem() { Text = "Marked as Pending" });
        lst.Add(new CheckListBoxItem() { Text = "Paid by Credit Card" });
        return lst;
    }
}
