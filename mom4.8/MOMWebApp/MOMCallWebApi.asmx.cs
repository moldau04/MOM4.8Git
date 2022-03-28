using BusinessEntity.Programs;
using BusinessEntity.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using Telerik.Web.UI;
using System.Data;
using BusinessLayer.Programs;

namespace MOMWebApp
{
    /// <summary>
    /// Summary description for MOMCallWebApi
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
     [System.Web.Script.Services.ScriptService]
    public class MOMCallWebApi : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        [WebMethod(EnableSession = true)]
        public RadComboBoxData GetAddEditServiceTypeViewDLLData(RadComboBoxContext context)
        {
            //string sql = "SELECT * from Customers WHERE CompanyName LIKE @text + '%'";

            //SqlDataAdapter adapter = new SqlDataAdapter(sql,
            //    ConfigurationManager.ConnectionStrings["NorthwindConnectionString"].ConnectionString);
            //DataTable data = new DataTable();

            //adapter.SelectCommand.Parameters.AddWithValue("@text", context.Text.Replace("%", "[%]"));
            //adapter.Fill(data);


            DataSet ds = new DataSet();
            DataTable data = new DataTable();
            ServiceTypeDDL _ServiceTypeDDL = new ServiceTypeDDL();
            _ServiceTypeDDL.SearchBy = context.Text.Replace("%", "[%]");
            _ServiceTypeDDL.Case = "Route";


            List<ServiceTypeDDLData> _ServiceTypeDDLData = new List<ServiceTypeDDLData>();

            string IsAPIIntegrationEnable = System.Web.Configuration.WebConfigurationManager.AppSettings["IsAPIIntegrationEnable"].Trim();

            if (IsAPIIntegrationEnable == "YES")
            {

                string APINAME = "APISetup/GetAddEditServiceTypeViewDLLData";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _ServiceTypeDDL);

                _ServiceTypeDDLData = (new JavaScriptSerializer()).Deserialize<List<ServiceTypeDDLData>>(_APIResponse.ResponseData);
                ds = CommonMethods.ToDataSet<ServiceTypeDDLData>(_ServiceTypeDDLData);
                
            }
            else
            {
                List<ServiceTypeDDLData> _DDLDATA = new BL_ServiceType().GetSetupServiceTypeDropDownValue(Session["config"].ToString(), _ServiceTypeDDL.SearchBy, _ServiceTypeDDL.Case);
                ds = CommonMethods.ToDataSet<ServiceTypeDDLData>(_DDLDATA);
                
            }
            data = ds.Tables[0];



            List<RadComboBoxItemData> result = new List<RadComboBoxItemData>(context.NumberOfItems);
            RadComboBoxData comboData = new RadComboBoxData();
            try
            {
                int itemsPerRequest = 10;
                int itemOffset = context.NumberOfItems;
                int endOffset = itemOffset + itemsPerRequest;
                if (endOffset > data.Rows.Count)
                {
                    endOffset = data.Rows.Count;
                }
                if (endOffset == data.Rows.Count)
                {
                    comboData.EndOfItems = true;
                }
                else
                {
                    comboData.EndOfItems = false;
                }
                result = new List<RadComboBoxItemData>(endOffset - itemOffset);
                for (int i = itemOffset; i < endOffset; i++)
                {
                    RadComboBoxItemData itemData = new RadComboBoxItemData();
                    itemData.Text = data.Rows[i]["NAME"].ToString();
                    itemData.Value = data.Rows[i]["value"].ToString();

                    result.Add(itemData);
                }

                if (data.Rows.Count > 0)
                {
                    comboData.Message = String.Format("Items <b>1</b>-<b>{0}</b> out of <b>{1}</b>", endOffset.ToString(), data.Rows.Count.ToString());
                }
                else
                {
                    comboData.Message = "No matches";
                }
            }
            catch (Exception e)
            {
                comboData.Message = e.Message;
            }

            comboData.Items = result.ToArray();
            return comboData;
        }

        [WebMethod(EnableSession = true)]
        public RadComboBoxData GetAddEditRTDLLData(RadComboBoxContext context)
        {

            DataSet ds = new DataSet();
            DataTable data = new DataTable();
            ServiceTypeDDL _ServiceTypeDDL = new ServiceTypeDDL();
            _ServiceTypeDDL.SearchBy = context.Text.Replace("%", "[%]");
            _ServiceTypeDDL.Case = "INV";

            List<ServiceTypeDDLData> _ServiceTypeDDLData = new List<ServiceTypeDDLData>();

            string IsAPIIntegrationEnable = System.Web.Configuration.WebConfigurationManager.AppSettings["IsAPIIntegrationEnable"].Trim();

            if (IsAPIIntegrationEnable == "YES")
            {

                string APINAME = "APISetup/GetAddEditServiceTypeViewDLLData";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _ServiceTypeDDL);

                _ServiceTypeDDLData = (new JavaScriptSerializer()).Deserialize<List<ServiceTypeDDLData>>(_APIResponse.ResponseData);
                ds = CommonMethods.ToDataSet<ServiceTypeDDLData>(_ServiceTypeDDLData);

            }
            else
            {
                List<ServiceTypeDDLData> _DDLDATA = new BL_ServiceType().GetSetupServiceTypeDropDownValue(Session["config"].ToString(), _ServiceTypeDDL.SearchBy, _ServiceTypeDDL.Case);
                ds = CommonMethods.ToDataSet<ServiceTypeDDLData>(_DDLDATA);
            }
            data = ds.Tables[0];
            

            List<RadComboBoxItemData> result = new List<RadComboBoxItemData>(context.NumberOfItems);
            RadComboBoxData comboData = new RadComboBoxData();
            try
            {
                int itemsPerRequest = 10;
                int itemOffset = context.NumberOfItems;
                int endOffset = itemOffset + itemsPerRequest;
                if (endOffset > data.Rows.Count)
                {
                    endOffset = data.Rows.Count;
                }
                if (endOffset == data.Rows.Count)
                {
                    comboData.EndOfItems = true;
                }
                else
                {
                    comboData.EndOfItems = false;
                }
                result = new List<RadComboBoxItemData>(endOffset - itemOffset);
                for (int i = itemOffset; i < endOffset; i++)
                {
                    RadComboBoxItemData itemData = new RadComboBoxItemData();
                    itemData.Text = data.Rows[i]["NAME"].ToString();
                    itemData.Value = data.Rows[i]["value"].ToString();

                    result.Add(itemData);
                }

                if (data.Rows.Count > 0)
                {
                    comboData.Message = String.Format("Items <b>1</b>-<b>{0}</b> out of <b>{1}</b>", endOffset.ToString(), data.Rows.Count.ToString());
                }
                else
                {
                    comboData.Message = "No matches";
                }
            }
            catch (Exception e)
            {
                comboData.Message = e.Message;
            }

            comboData.Items = result.ToArray();
            return comboData;
        }

        [WebMethod(EnableSession = true)]
        public RadComboBoxData GetAddEditDDlotDLLData(RadComboBoxContext context)
        {

            DataSet ds = new DataSet();
            DataTable data = new DataTable();
            ServiceTypeDDL _ServiceTypeDDL = new ServiceTypeDDL();
            _ServiceTypeDDL.SearchBy = context.Text.Replace("%", "[%]");
            _ServiceTypeDDL.Case = "INV";

            List<ServiceTypeDDLData> _ServiceTypeDDLData = new List<ServiceTypeDDLData>();

            string IsAPIIntegrationEnable = System.Web.Configuration.WebConfigurationManager.AppSettings["IsAPIIntegrationEnable"].Trim();

            if (IsAPIIntegrationEnable == "YES")
            {

                string APINAME = "APISetup/GetAddEditServiceTypeViewDLLData";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _ServiceTypeDDL);

                _ServiceTypeDDLData = (new JavaScriptSerializer()).Deserialize<List<ServiceTypeDDLData>>(_APIResponse.ResponseData);
                ds = CommonMethods.ToDataSet<ServiceTypeDDLData>(_ServiceTypeDDLData);

            }
            else
            {
                List<ServiceTypeDDLData> _DDLDATA = new BL_ServiceType().GetSetupServiceTypeDropDownValue(Session["config"].ToString(), _ServiceTypeDDL.SearchBy, _ServiceTypeDDL.Case);
                ds = CommonMethods.ToDataSet<ServiceTypeDDLData>(_DDLDATA);
            }
            data = ds.Tables[0];


            List<RadComboBoxItemData> result = new List<RadComboBoxItemData>(context.NumberOfItems);
            RadComboBoxData comboData = new RadComboBoxData();
            try
            {
                int itemsPerRequest = 10;
                int itemOffset = context.NumberOfItems;
                int endOffset = itemOffset + itemsPerRequest;
                if (endOffset > data.Rows.Count)
                {
                    endOffset = data.Rows.Count;
                }
                if (endOffset == data.Rows.Count)
                {
                    comboData.EndOfItems = true;
                }
                else
                {
                    comboData.EndOfItems = false;
                }
                result = new List<RadComboBoxItemData>(endOffset - itemOffset);
                for (int i = itemOffset; i < endOffset; i++)
                {
                    RadComboBoxItemData itemData = new RadComboBoxItemData();
                    itemData.Text = data.Rows[i]["NAME"].ToString();
                    itemData.Value = data.Rows[i]["value"].ToString();

                    result.Add(itemData);
                }

                if (data.Rows.Count > 0)
                {
                    comboData.Message = String.Format("Items <b>1</b>-<b>{0}</b> out of <b>{1}</b>", endOffset.ToString(), data.Rows.Count.ToString());
                }
                else
                {
                    comboData.Message = "No matches";
                }
            }
            catch (Exception e)
            {
                comboData.Message = e.Message;
            }

            comboData.Items = result.ToArray();
            return comboData;
        }

        [WebMethod(EnableSession = true)]
        public RadComboBoxData GetAddEditLocationtypeDLLData(RadComboBoxContext context)
        {

            DataSet ds = new DataSet();
            DataTable data = new DataTable();
            ServiceTypeDDL _ServiceTypeDDL = new ServiceTypeDDL();
            _ServiceTypeDDL.SearchBy = context.Text.Replace("%", "[%]");
            _ServiceTypeDDL.Case = "LTYPE";

            List<ServiceTypeDDLData> _ServiceTypeDDLData = new List<ServiceTypeDDLData>();

            string IsAPIIntegrationEnable = System.Web.Configuration.WebConfigurationManager.AppSettings["IsAPIIntegrationEnable"].Trim();

            if (IsAPIIntegrationEnable == "YES")
            {

                string APINAME = "APISetup/GetAddEditServiceTypeViewDLLData";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _ServiceTypeDDL);

                _ServiceTypeDDLData = (new JavaScriptSerializer()).Deserialize<List<ServiceTypeDDLData>>(_APIResponse.ResponseData);
                ds = CommonMethods.ToDataSet<ServiceTypeDDLData>(_ServiceTypeDDLData);

            }
            else
            {
                List<ServiceTypeDDLData> _DDLDATA = new BL_ServiceType().GetSetupServiceTypeDropDownValue(Session["config"].ToString(), _ServiceTypeDDL.SearchBy, _ServiceTypeDDL.Case);
                ds = CommonMethods.ToDataSet<ServiceTypeDDLData>(_DDLDATA);
            }
            data = ds.Tables[0];


            List<RadComboBoxItemData> result = new List<RadComboBoxItemData>(context.NumberOfItems);
            RadComboBoxData comboData = new RadComboBoxData();
            try
            {
                int itemsPerRequest = 10;
                int itemOffset = context.NumberOfItems;
                int endOffset = itemOffset + itemsPerRequest;
                if (endOffset > data.Rows.Count)
                {
                    endOffset = data.Rows.Count;
                }
                if (endOffset == data.Rows.Count)
                {
                    comboData.EndOfItems = true;
                }
                else
                {
                    comboData.EndOfItems = false;
                }
                result = new List<RadComboBoxItemData>(endOffset - itemOffset);
                for (int i = itemOffset; i < endOffset; i++)
                {
                    RadComboBoxItemData itemData = new RadComboBoxItemData();
                    itemData.Text = data.Rows[i]["NAME"].ToString();
                    itemData.Value = data.Rows[i]["value"].ToString();

                    result.Add(itemData);
                }

                if (data.Rows.Count > 0)
                {
                    comboData.Message = String.Format("Items <b>1</b>-<b>{0}</b> out of <b>{1}</b>", endOffset.ToString(), data.Rows.Count.ToString());
                }
                else
                {
                    comboData.Message = "No matches";
                }
            }
            catch (Exception e)
            {
                comboData.Message = e.Message;
            }

            comboData.Items = result.ToArray();
            return comboData;
        }

        [WebMethod(EnableSession = true)]
        public RadComboBoxData GetAddEditddl1Point7DLLData(RadComboBoxContext context)
        {

            DataSet ds = new DataSet();
            DataTable data = new DataTable();
            ServiceTypeDDL _ServiceTypeDDL = new ServiceTypeDDL();
            _ServiceTypeDDL.SearchBy = context.Text.Replace("%", "[%]");
            _ServiceTypeDDL.Case = "INV";

            List<ServiceTypeDDLData> _ServiceTypeDDLData = new List<ServiceTypeDDLData>();

            string IsAPIIntegrationEnable = System.Web.Configuration.WebConfigurationManager.AppSettings["IsAPIIntegrationEnable"].Trim();

            if (IsAPIIntegrationEnable == "YES")
            {

                string APINAME = "APISetup/GetAddEditServiceTypeViewDLLData";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _ServiceTypeDDL);

                _ServiceTypeDDLData = (new JavaScriptSerializer()).Deserialize<List<ServiceTypeDDLData>>(_APIResponse.ResponseData);
                ds = CommonMethods.ToDataSet<ServiceTypeDDLData>(_ServiceTypeDDLData);

            }
            else
            {
                List<ServiceTypeDDLData> _DDLDATA = new BL_ServiceType().GetSetupServiceTypeDropDownValue(Session["config"].ToString(), _ServiceTypeDDL.SearchBy, _ServiceTypeDDL.Case);
                ds = CommonMethods.ToDataSet<ServiceTypeDDLData>(_DDLDATA);
            }
            data = ds.Tables[0];


            List<RadComboBoxItemData> result = new List<RadComboBoxItemData>(context.NumberOfItems);
            RadComboBoxData comboData = new RadComboBoxData();
            try
            {
                int itemsPerRequest = 10;
                int itemOffset = context.NumberOfItems;
                int endOffset = itemOffset + itemsPerRequest;
                if (endOffset > data.Rows.Count)
                {
                    endOffset = data.Rows.Count;
                }
                if (endOffset == data.Rows.Count)
                {
                    comboData.EndOfItems = true;
                }
                else
                {
                    comboData.EndOfItems = false;
                }
                result = new List<RadComboBoxItemData>(endOffset - itemOffset);
                for (int i = itemOffset; i < endOffset; i++)
                {
                    RadComboBoxItemData itemData = new RadComboBoxItemData();
                    itemData.Text = data.Rows[i]["NAME"].ToString();
                    itemData.Value = data.Rows[i]["value"].ToString();

                    result.Add(itemData);
                }

                if (data.Rows.Count > 0)
                {
                    comboData.Message = String.Format("Items <b>1</b>-<b>{0}</b> out of <b>{1}</b>", endOffset.ToString(), data.Rows.Count.ToString());
                }
                else
                {
                    comboData.Message = "No matches";
                }
            }
            catch (Exception e)
            {
                comboData.Message = e.Message;
            }

            comboData.Items = result.ToArray();
            return comboData;
        }

        [WebMethod(EnableSession = true)]
        public RadComboBoxData GetAddEditddlDTDLLData(RadComboBoxContext context)
        {

            DataSet ds = new DataSet();
            DataTable data = new DataTable();
            ServiceTypeDDL _ServiceTypeDDL = new ServiceTypeDDL();
            _ServiceTypeDDL.SearchBy = context.Text.Replace("%", "[%]");
            _ServiceTypeDDL.Case = "INV";

            List<ServiceTypeDDLData> _ServiceTypeDDLData = new List<ServiceTypeDDLData>();

            string IsAPIIntegrationEnable = System.Web.Configuration.WebConfigurationManager.AppSettings["IsAPIIntegrationEnable"].Trim();

            if (IsAPIIntegrationEnable == "YES")
            {

                string APINAME = "APISetup/GetAddEditServiceTypeViewDLLData";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _ServiceTypeDDL);

                _ServiceTypeDDLData = (new JavaScriptSerializer()).Deserialize<List<ServiceTypeDDLData>>(_APIResponse.ResponseData);
                ds = CommonMethods.ToDataSet<ServiceTypeDDLData>(_ServiceTypeDDLData);

            }
            else
            {
                List<ServiceTypeDDLData> _DDLDATA = new BL_ServiceType().GetSetupServiceTypeDropDownValue(Session["config"].ToString(), _ServiceTypeDDL.SearchBy, _ServiceTypeDDL.Case);
                ds = CommonMethods.ToDataSet<ServiceTypeDDLData>(_DDLDATA);
            }
            data = ds.Tables[0];


            List<RadComboBoxItemData> result = new List<RadComboBoxItemData>(context.NumberOfItems);
            RadComboBoxData comboData = new RadComboBoxData();
            try
            {
                int itemsPerRequest = 10;
                int itemOffset = context.NumberOfItems;
                int endOffset = itemOffset + itemsPerRequest;
                if (endOffset > data.Rows.Count)
                {
                    endOffset = data.Rows.Count;
                }
                if (endOffset == data.Rows.Count)
                {
                    comboData.EndOfItems = true;
                }
                else
                {
                    comboData.EndOfItems = false;
                }
                result = new List<RadComboBoxItemData>(endOffset - itemOffset);
                for (int i = itemOffset; i < endOffset; i++)
                {
                    RadComboBoxItemData itemData = new RadComboBoxItemData();
                    itemData.Text = data.Rows[i]["NAME"].ToString();
                    itemData.Value = data.Rows[i]["value"].ToString();

                    result.Add(itemData);
                }

                if (data.Rows.Count > 0)
                {
                    comboData.Message = String.Format("Items <b>1</b>-<b>{0}</b> out of <b>{1}</b>", endOffset.ToString(), data.Rows.Count.ToString());
                }
                else
                {
                    comboData.Message = "No matches";
                }
            }
            catch (Exception e)
            {
                comboData.Message = e.Message;
            }

            comboData.Items = result.ToArray();
            return comboData;
        }

        [WebMethod(EnableSession = true)]
        public RadComboBoxData GetAddEditddlBillingCodeDLLData(RadComboBoxContext context)
        {

            DataSet ds = new DataSet();
            DataTable data = new DataTable();
            ServiceTypeDDL _ServiceTypeDDL = new ServiceTypeDDL();
            _ServiceTypeDDL.SearchBy = context.Text.Replace("%", "[%]");
            _ServiceTypeDDL.Case = "INV";

            List<ServiceTypeDDLData> _ServiceTypeDDLData = new List<ServiceTypeDDLData>();

            string IsAPIIntegrationEnable = System.Web.Configuration.WebConfigurationManager.AppSettings["IsAPIIntegrationEnable"].Trim();

            if (IsAPIIntegrationEnable == "YES")
            {

                string APINAME = "APISetup/GetAddEditServiceTypeViewDLLData";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _ServiceTypeDDL);

                _ServiceTypeDDLData = (new JavaScriptSerializer()).Deserialize<List<ServiceTypeDDLData>>(_APIResponse.ResponseData);
                ds = CommonMethods.ToDataSet<ServiceTypeDDLData>(_ServiceTypeDDLData);

            }
            else
            {
                List<ServiceTypeDDLData> _DDLDATA = new BL_ServiceType().GetSetupServiceTypeDropDownValue(Session["config"].ToString(), _ServiceTypeDDL.SearchBy, _ServiceTypeDDL.Case);
                ds = CommonMethods.ToDataSet<ServiceTypeDDLData>(_DDLDATA);
            }
            data = ds.Tables[0];


            List<RadComboBoxItemData> result = new List<RadComboBoxItemData>(context.NumberOfItems);
            RadComboBoxData comboData = new RadComboBoxData();
            try
            {
                int itemsPerRequest = 10;
                int itemOffset = context.NumberOfItems;
                int endOffset = itemOffset + itemsPerRequest;
                if (endOffset > data.Rows.Count)
                {
                    endOffset = data.Rows.Count;
                }
                if (endOffset == data.Rows.Count)
                {
                    comboData.EndOfItems = true;
                }
                else
                {
                    comboData.EndOfItems = false;
                }
                result = new List<RadComboBoxItemData>(endOffset - itemOffset);
                for (int i = itemOffset; i < endOffset; i++)
                {
                    RadComboBoxItemData itemData = new RadComboBoxItemData();
                    itemData.Text = data.Rows[i]["NAME"].ToString();
                    itemData.Value = data.Rows[i]["value"].ToString();

                    result.Add(itemData);
                }

                if (data.Rows.Count > 0)
                {
                    comboData.Message = String.Format("Items <b>1</b>-<b>{0}</b> out of <b>{1}</b>", endOffset.ToString(), data.Rows.Count.ToString());
                }
                else
                {
                    comboData.Message = "No matches";
                }
            }
            catch (Exception e)
            {
                comboData.Message = e.Message;
            }

            comboData.Items = result.ToArray();
            return comboData;
        }

        [WebMethod(EnableSession = true)]
        public RadComboBoxData GetAddEditddlWCDLLData(RadComboBoxContext context)
        {

            DataSet ds = new DataSet();
            DataTable data = new DataTable();
            ServiceTypeDDL _ServiceTypeDDL = new ServiceTypeDDL();
            _ServiceTypeDDL.SearchBy = context.Text.Replace("%", "[%]");
            _ServiceTypeDDL.Case = "PRWage";

            List<ServiceTypeDDLData> _ServiceTypeDDLData = new List<ServiceTypeDDLData>();

            string IsAPIIntegrationEnable = System.Web.Configuration.WebConfigurationManager.AppSettings["IsAPIIntegrationEnable"].Trim();

            if (IsAPIIntegrationEnable == "YES")
            {

                string APINAME = "APISetup/GetAddEditServiceTypeViewDLLData";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _ServiceTypeDDL);

                _ServiceTypeDDLData = (new JavaScriptSerializer()).Deserialize<List<ServiceTypeDDLData>>(_APIResponse.ResponseData);
                ds = CommonMethods.ToDataSet<ServiceTypeDDLData>(_ServiceTypeDDLData);

            }
            else
            {
                List<ServiceTypeDDLData> _DDLDATA = new BL_ServiceType().GetSetupServiceTypeDropDownValue(Session["config"].ToString(), _ServiceTypeDDL.SearchBy, _ServiceTypeDDL.Case);
                ds = CommonMethods.ToDataSet<ServiceTypeDDLData>(_DDLDATA);
            }
            data = ds.Tables[0];


            List<RadComboBoxItemData> result = new List<RadComboBoxItemData>(context.NumberOfItems);
            RadComboBoxData comboData = new RadComboBoxData();
            try
            {
                int itemsPerRequest = 10;
                int itemOffset = context.NumberOfItems;
                int endOffset = itemOffset + itemsPerRequest;
                if (endOffset > data.Rows.Count)
                {
                    endOffset = data.Rows.Count;
                }
                if (endOffset == data.Rows.Count)
                {
                    comboData.EndOfItems = true;
                }
                else
                {
                    comboData.EndOfItems = false;
                }
                result = new List<RadComboBoxItemData>(endOffset - itemOffset);
                for (int i = itemOffset; i < endOffset; i++)
                {
                    RadComboBoxItemData itemData = new RadComboBoxItemData();
                    itemData.Text = data.Rows[i]["NAME"].ToString();
                    itemData.Value = data.Rows[i]["value"].ToString();

                    result.Add(itemData);
                }

                if (data.Rows.Count > 0)
                {
                    comboData.Message = String.Format("Items <b>1</b>-<b>{0}</b> out of <b>{1}</b>", endOffset.ToString(), data.Rows.Count.ToString());
                }
                else
                {
                    comboData.Message = "No matches";
                }
            }
            catch (Exception e)
            {
                comboData.Message = e.Message;
            }

            comboData.Items = result.ToArray();
            return comboData;
        }

        [WebMethod(EnableSession = true)]
        public RadComboBoxData GetAddEditDDLEGLDLLData(RadComboBoxContext context)
        {

            DataSet ds = new DataSet();
            DataTable data = new DataTable();
            ServiceTypeDDL _ServiceTypeDDL = new ServiceTypeDDL();
            _ServiceTypeDDL.SearchBy = context.Text.Replace("%", "[%]");
            _ServiceTypeDDL.Case = "Chart";

            List<ServiceTypeDDLData> _ServiceTypeDDLData = new List<ServiceTypeDDLData>();

            string IsAPIIntegrationEnable = System.Web.Configuration.WebConfigurationManager.AppSettings["IsAPIIntegrationEnable"].Trim();

            if (IsAPIIntegrationEnable == "YES")
            {

                string APINAME = "APISetup/GetAddEditServiceTypeViewDLLData";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _ServiceTypeDDL);

                _ServiceTypeDDLData = (new JavaScriptSerializer()).Deserialize<List<ServiceTypeDDLData>>(_APIResponse.ResponseData);
                ds = CommonMethods.ToDataSet<ServiceTypeDDLData>(_ServiceTypeDDLData);

            }
            else
            {
                List<ServiceTypeDDLData> _DDLDATA = new BL_ServiceType().GetSetupServiceTypeDropDownValue(Session["config"].ToString(), _ServiceTypeDDL.SearchBy, _ServiceTypeDDL.Case);
                ds = CommonMethods.ToDataSet<ServiceTypeDDLData>(_DDLDATA);
            }
            data = ds.Tables[0];


            List<RadComboBoxItemData> result = new List<RadComboBoxItemData>(context.NumberOfItems);
            RadComboBoxData comboData = new RadComboBoxData();
            try
            {
                int itemsPerRequest = 10;
                int itemOffset = context.NumberOfItems;
                int endOffset = itemOffset + itemsPerRequest;
                if (endOffset > data.Rows.Count)
                {
                    endOffset = data.Rows.Count;
                }
                if (endOffset == data.Rows.Count)
                {
                    comboData.EndOfItems = true;
                }
                else
                {
                    comboData.EndOfItems = false;
                }
                result = new List<RadComboBoxItemData>(endOffset - itemOffset);
                for (int i = itemOffset; i < endOffset; i++)
                {
                    RadComboBoxItemData itemData = new RadComboBoxItemData();
                    itemData.Text = data.Rows[i]["NAME"].ToString();
                    itemData.Value = data.Rows[i]["value"].ToString();

                    result.Add(itemData);
                }

                if (data.Rows.Count > 0)
                {
                    comboData.Message = String.Format("Items <b>1</b>-<b>{0}</b> out of <b>{1}</b>", endOffset.ToString(), data.Rows.Count.ToString());
                }
                else
                {
                    comboData.Message = "No matches";
                }
            }
            catch (Exception e)
            {
                comboData.Message = e.Message;
            }

            comboData.Items = result.ToArray();
            return comboData;
        }

        [WebMethod(EnableSession = true)]
        public RadComboBoxData GetAddEditDDLIGLDLLData(RadComboBoxContext context)
        {

            DataSet ds = new DataSet();
            DataTable data = new DataTable();
            ServiceTypeDDL _ServiceTypeDDL = new ServiceTypeDDL();
            _ServiceTypeDDL.SearchBy = context.Text.Replace("%", "[%]");
            _ServiceTypeDDL.Case = "Chart";

            List<ServiceTypeDDLData> _ServiceTypeDDLData = new List<ServiceTypeDDLData>();

            string IsAPIIntegrationEnable = System.Web.Configuration.WebConfigurationManager.AppSettings["IsAPIIntegrationEnable"].Trim();

            if (IsAPIIntegrationEnable == "YES")
            {

                string APINAME = "APISetup/GetAddEditServiceTypeViewDLLData";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _ServiceTypeDDL);

                _ServiceTypeDDLData = (new JavaScriptSerializer()).Deserialize<List<ServiceTypeDDLData>>(_APIResponse.ResponseData);
                ds = CommonMethods.ToDataSet<ServiceTypeDDLData>(_ServiceTypeDDLData);

            }
            else
            {
                List<ServiceTypeDDLData> _DDLDATA = new BL_ServiceType().GetSetupServiceTypeDropDownValue(Session["config"].ToString(), _ServiceTypeDDL.SearchBy, _ServiceTypeDDL.Case);
                ds = CommonMethods.ToDataSet<ServiceTypeDDLData>(_DDLDATA);
            }
            data = ds.Tables[0];


            List<RadComboBoxItemData> result = new List<RadComboBoxItemData>(context.NumberOfItems);
            RadComboBoxData comboData = new RadComboBoxData();
            try
            {
                int itemsPerRequest = 10;
                int itemOffset = context.NumberOfItems;
                int endOffset = itemOffset + itemsPerRequest;
                if (endOffset > data.Rows.Count)
                {
                    endOffset = data.Rows.Count;
                }
                if (endOffset == data.Rows.Count)
                {
                    comboData.EndOfItems = true;
                }
                else
                {
                    comboData.EndOfItems = false;
                }
                result = new List<RadComboBoxItemData>(endOffset - itemOffset);
                for (int i = itemOffset; i < endOffset; i++)
                {
                    RadComboBoxItemData itemData = new RadComboBoxItemData();
                    itemData.Text = data.Rows[i]["NAME"].ToString();
                    itemData.Value = data.Rows[i]["value"].ToString();

                    result.Add(itemData);
                }

                if (data.Rows.Count > 0)
                {
                    comboData.Message = String.Format("Items <b>1</b>-<b>{0}</b> out of <b>{1}</b>", endOffset.ToString(), data.Rows.Count.ToString());
                }
                else
                {
                    comboData.Message = "No matches";
                }
            }
            catch (Exception e)
            {
                comboData.Message = e.Message;
            }

            comboData.Items = result.ToArray();
            return comboData;
        }

        [WebMethod(EnableSession = true)]
        public RadComboBoxData GetAddEditDepartmentDLLData(RadComboBoxContext context)
        {

            DataSet ds = new DataSet();
            DataTable data = new DataTable();
            ServiceTypeDDL _ServiceTypeDDL = new ServiceTypeDDL();
            _ServiceTypeDDL.SearchBy = context.Text.Replace("%", "[%]");
            _ServiceTypeDDL.Case = "Department";

            List<ServiceTypeDDLData> _ServiceTypeDDLData = new List<ServiceTypeDDLData>();

            string IsAPIIntegrationEnable = System.Web.Configuration.WebConfigurationManager.AppSettings["IsAPIIntegrationEnable"].Trim();

            if (IsAPIIntegrationEnable == "YES")
            {

                string APINAME = "APISetup/GetAddEditServiceTypeViewDLLData";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _ServiceTypeDDL);

                _ServiceTypeDDLData = (new JavaScriptSerializer()).Deserialize<List<ServiceTypeDDLData>>(_APIResponse.ResponseData);
                ds = CommonMethods.ToDataSet<ServiceTypeDDLData>(_ServiceTypeDDLData);

            }
            else
            {
                List<ServiceTypeDDLData> _DDLDATA = new BL_ServiceType().GetSetupServiceTypeDropDownValue(Session["config"].ToString(), _ServiceTypeDDL.SearchBy, _ServiceTypeDDL.Case);
                ds = CommonMethods.ToDataSet<ServiceTypeDDLData>(_DDLDATA);
            }
            data = ds.Tables[0];


            List<RadComboBoxItemData> result = new List<RadComboBoxItemData>(context.NumberOfItems);
            RadComboBoxData comboData = new RadComboBoxData();
            try
            {
                int itemsPerRequest = 10;
                int itemOffset = context.NumberOfItems;
                int endOffset = itemOffset + itemsPerRequest;
                if (endOffset > data.Rows.Count)
                {
                    endOffset = data.Rows.Count;
                }
                if (endOffset == data.Rows.Count)
                {
                    comboData.EndOfItems = true;
                }
                else
                {
                    comboData.EndOfItems = false;
                }
                result = new List<RadComboBoxItemData>(endOffset - itemOffset);
                for (int i = itemOffset; i < endOffset; i++)
                {
                    RadComboBoxItemData itemData = new RadComboBoxItemData();
                    itemData.Text = data.Rows[i]["NAME"].ToString();
                    itemData.Value = data.Rows[i]["value"].ToString();

                    result.Add(itemData);
                }

                if (data.Rows.Count > 0)
                {
                    comboData.Message = String.Format("Items <b>1</b>-<b>{0}</b> out of <b>{1}</b>", endOffset.ToString(), data.Rows.Count.ToString());
                }
                else
                {
                    comboData.Message = "No matches";
                }
            }
            catch (Exception e)
            {
                comboData.Message = e.Message;
            }

            comboData.Items = result.ToArray();
            return comboData;
        }

    }
}
