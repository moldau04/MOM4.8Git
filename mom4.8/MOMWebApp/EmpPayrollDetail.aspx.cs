using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLayer;
using BusinessEntity;
using System.Data;
using Telerik.Web.UI;
using System.Web.Services;
using System.Collections.Generic;
using Microsoft.ApplicationBlocks.Data;
using System.Web;
using BusinessLayer.Programs;
using BusinessEntity.Programs;
using System.Linq;
using System.Net.Http;
using System.Xml;
using System.Xml.Linq;
using System.Threading.Tasks;
using System.Globalization;
using System.Text;
using System.Configuration;
using Convert = System.Convert;
using Microsoft.Ajax.Utilities;

namespace MOMWebApp
{
    public partial class EmpPayrollDetail : Page
    {
        BL_User objBL_User = new BL_User();
        User objProp_User = new User();
        //consult
        tblConsult objProp_Consult = new tblConsult();
        BL_General objBL_General = new BL_General();
        General objGeneral = new General();
        BL_Customer objBL_Customer = new BL_Customer();
        BL_Vendor objBL_Vendor = new BL_Vendor();
        Customer objCustomer = new Customer();
        BL_ReportsData objBL_ReportData = new BL_ReportsData();
        GeneralFunctions objGeneralFunctions = new GeneralFunctions();
        public static bool IsAddEdit = false;
        public static bool IsDelete = false;
        Wage _objWage = new Wage();
        Emp _objEmp = new Emp();
        BL_Wage objBL_Wage = new BL_Wage();
        bool api = false;

        double holiday;
        double vacation;
        double zone;
        double reimb;
        double mileage;
        double bonus;
        double sick;
        int payrollResigerId = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Request.QueryString["id"] != null)
                {
                    holiday = ConvertCurrentCurrencyFormatToDbl(Request.QueryString["sHol"].ToString());
                    vacation = ConvertCurrentCurrencyFormatToDbl(Request.QueryString["sVac"].ToString());
                    zone = ConvertCurrentCurrencyFormatToDbl(Request.QueryString["sZone"].ToString());
                    reimb = ConvertCurrentCurrencyFormatToDbl(Request.QueryString["sReimb"].ToString());
                    mileage = ConvertCurrentCurrencyFormatToDbl(Request.QueryString["sMilage"].ToString());
                    bonus = ConvertCurrentCurrencyFormatToDbl(Request.QueryString["sBonus"].ToString());
                    sick = ConvertCurrentCurrencyFormatToDbl(Request.QueryString["sSick"].ToString());
                }
                if (Session["PayrollRegisterId"] != null)
                {
                    payrollResigerId = Convert.ToInt32(Session["PayrollRegisterId"]);
                }
            }
        }
        private void GetHourList(int EmpID)
        {
            try
            {
                User objPropUser = new User();
                DataSet ds = new DataSet();
                objPropUser.ConnConfig = Session["config"].ToString();
                objPropUser.FStart = Convert.ToDateTime(Request.QueryString["sDt"].ToString());
                objPropUser.Edate = Convert.ToDateTime(Request.QueryString["eDt"].ToString());
                objPropUser.Supervisor = Request.QueryString["Super"].ToString();
                objPropUser.DepartmentID = 0;
                objPropUser.EN = 0;
                objPropUser.ID = EmpID;
                objPropUser.WorkId = 0;
                objPropUser.Bonus = bonus;
                objPropUser.HolidayAm = holiday;
                objPropUser.ZoneAm = zone;
                objPropUser.ReimbAm = reimb;
                objPropUser.MilageAm = mileage;
                objPropUser.SickAm = sick;

                //int registerId = payrollResigerId;
                //ds = new BL_Wage().GetPayrollHour(objPropUser);
                ds = new BL_Wage().GetPayrollHourByEmpId(objPropUser, payrollResigerId);

                //ds = new BL_Wage().GetPayrollHourByEmpId(objPropUser, registerId);

                RadGridPayrollHours.VirtualItemCount = ds.Tables[0].Rows.Count;
                RadGridPayrollHours.DataSource = ds.Tables[0];

                RadGridOtherWage.VirtualItemCount = ds.Tables[1].Rows.Count;
                RadGridOtherWage.DataSource = ds.Tables[1];
                //RadGridPayrollHours.DataBind();
                ViewState["VirtualItemCountHour"] = ds.Tables[0].Rows.Count;
                Session["PayrollHourList"] = ds.Tables[0];

                //return ds;
            }
            catch (Exception ex)
            {
                string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
                ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            }
        }
        private double ConvertCurrentCurrencyFormatToDbl(string strCurrency)
        {
            if (!string.IsNullOrEmpty(strCurrency))
            {
                var dblReturn = double.Parse(strCurrency.Replace('$', '0'), NumberStyles.AllowParentheses | NumberStyles.AllowThousands | NumberStyles.AllowDecimalPoint | NumberStyles.AllowTrailingSign | NumberStyles.Float);
                return dblReturn;
            }
            else
            {
                return 0;
            }
        }
        private void GetRevenues(int EmpID)
        {
            try
            {
                User objPropUser = new User();
                DataSet ds = new DataSet();
                objPropUser.ConnConfig = Session["config"].ToString();
                objPropUser.FStart = Convert.ToDateTime(Request.QueryString["sDt"].ToString());
                objPropUser.Edate = Convert.ToDateTime(Request.QueryString["eDt"].ToString());
                objPropUser.Supervisor = Request.QueryString["Super"].ToString();
                objPropUser.DepartmentID = 0;
                objPropUser.EN = 0;
                objPropUser.ID = EmpID;
                objPropUser.WorkId = 0;

                objPropUser.HolidayAm = holiday;
                objPropUser.VacAm = vacation;
                objPropUser.ZoneAm = zone;
                objPropUser.ReimbAm = reimb;
                objPropUser.MilageAm = mileage;
                objPropUser.BonusAm = bonus;
                objPropUser.SickAm = sick;

                ds = new BL_Wage().GetPayrollRevenues(objPropUser);

                lblname.Text = ds.Tables[0].Rows[0]["Name"].ToString();
                lblAddress.Text = ds.Tables[0].Rows[0]["Address"].ToString();
                lblCityStateZip.Text = ds.Tables[0].Rows[0]["City"].ToString() + " , " + ds.Tables[0].Rows[0]["State"].ToString() + " " + ds.Tables[0].Rows[0]["ZIP"].ToString();

                RadGrid_Revenues.VirtualItemCount = ds.Tables[1].Rows.Count;
                RadGrid_Revenues.DataSource = ds.Tables[1];
                //RadGridPayrollHours.DataBind();
                ViewState["VirtualItemCountRevenue"] = ds.Tables[1].Rows.Count;

                Session["PayrollRevenue"] = ds.Tables[1];
            }
            catch (Exception ex)
            {
                string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
                ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            }
        }
        private string GetWorkGeoCode()
        {
            string workgeo = "";
            string code = "";
            //////////////////////////////////////////
            //Credentials for Payroll OnDemand
            string username = ConfigurationManager.AppSettings["vertexApiUsername"].ToString(); // "dread@1000";
            string passWord = ConfigurationManager.AppSettings["vertexApiPassword"].ToString(); // "K3CHccxQ";
            // Setting up the URI for Payroll
            string uri = "https://payrollsandbox.ondemand.vertexinc.com:443/EiWebSvc/AddressWebService";

            DataSet dswork = new DataSet();
            objProp_User.ConnConfig = Session["config"].ToString();
            dswork = objBL_User.getControl(objProp_User);
            DataRow _dr = dswork.Tables[0].Rows[0];

            string addrClnXML = "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:eic = \"http://EiCalc/\">"
                + "<soapenv:Header/>"
                + "<soapenv:Body>"
                + "<eic:AddrCleanse>"
                + "<Request>"
                + "<![CDATA["
                    + "<ADDRESS_CLEANSE_REQUEST>"
                        + "<StreetAddress1>" + _dr["Address"].ToString() + "</StreetAddress1>"
                        + "<CityName>" + _dr["City"].ToString() + "</CityName>"
                        + "<StateName>" + _dr["state"].ToString() + "</StateName>"
                        + "<ZipCode>" + _dr["zip"].ToString() + "</ZipCode>"
                    + "</ADDRESS_CLEANSE_REQUEST>]]>"
                + "</Request>"
                + "</eic:AddrCleanse>"
                + "</soapenv:Body>"
                + "</soapenv:Envelope>";

            try
            {
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                HttpClient cl = new HttpClient();
                cl.BaseAddress = new Uri(uri);
                cl.DefaultRequestHeaders.Clear();
                cl.DefaultRequestHeaders.Add("javax.xml.ws.security.auth.username", username);
                cl.DefaultRequestHeaders.Add("javax.xml.ws.security.auth.password", passWord);
                HttpContent soapAddressEnvelope = new StringContent(addrClnXML);
                soapAddressEnvelope.Headers.Clear();
                soapAddressEnvelope.Headers.Add("Content-Type", "text/xml");
                using (HttpResponseMessage response = cl.PostAsync(uri, soapAddressEnvelope).Result)
                {
                    string rawString = getMessage(response).Result;

                    XmlDocument responseXML = new XmlDocument();
                    responseXML.LoadXml(rawString);
                    XDocument responseXMLPretty = XDocument.Parse(responseXML.InnerText.ToString());

                    string responseXMLPrettystr = responseXMLPretty.ToString();
                    responseXMLPrettystr = responseXMLPrettystr.Replace("\"", "'");
                    XmlDocument Doc = new XmlDocument();
                    Doc.LoadXml(responseXMLPrettystr);
                    XmlNode nodedoc = Doc.GetElementsByTagName("GeoCode").Item(0);
                    workgeo = nodedoc.ChildNodes.Item(0).InnerText;

                    soapAddressEnvelope.Dispose();
                    cl.Dispose();
                }
            }

            catch (AggregateException aggEx)
            {
                //If an error occurs outside of the service call capture and show below
                Console.WriteLine("A Connection error occurred");
                Console.WriteLine("----------------------------------------------------");
                Console.WriteLine("Error Code: " + aggEx.Message.ToString() + Environment.NewLine + "Message: " + aggEx.InnerException.Message.ToString());
            }

            // Below call to read the message back from server from stream to string
            async Task<string> getMessage(HttpResponseMessage messageFromServer)
            {
                code = await messageFromServer.Content.ReadAsStringAsync();
                return code;
            }
            //////////////////////////////////////////
            return workgeo;
        }
        public DataTable GetDeductionTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ID", typeof(int));
            dt.Columns.Add("fDesc", typeof(string));
            dt.Columns.Add("Amount", typeof(double));
            dt.Columns.Add("PaidBy", typeof(int));
            dt.Columns.Add("YTDAmount", typeof(double));

            return dt;
        }
        public DataTable GetGetDeductionsListDB(int EmpID)
        {
            //try
            //{
            User objPropUser = new User();
            DataSet ds = new DataSet();
            objPropUser.ConnConfig = Session["config"].ToString();
            objPropUser.FStart = Convert.ToDateTime(Request.QueryString["sDt"].ToString());
            objPropUser.Edate = Convert.ToDateTime(Request.QueryString["eDt"].ToString());

            int ProcessDed = Convert.ToInt32(Request.QueryString["sProcessDed"].ToString());

            objPropUser.HolidayAm = holiday;
            objPropUser.VacAm = vacation;
            objPropUser.ZoneAm = zone;
            objPropUser.ReimbAm = reimb;
            objPropUser.MilageAm = mileage;
            objPropUser.BonusAm = bonus;
            objPropUser.SickAm = sick;

            objPropUser.ID = EmpID;

            ds = new BL_Wage().GetPayrollDeductions(objPropUser, ProcessDed);

            return ds.Tables[0];

            //}
            //catch (Exception ex)
            //{
            //    string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            //    ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            //}
        }

        private void GetDeductions(int EmpID)
        {
            try
            {
                DataSet dsemp = new DataSet();
                _objEmp.ConnConfig = Session["config"].ToString();
                _objEmp.ID = Convert.ToInt32(EmpID);
                dsemp = objBL_Wage.GetEmployeeListByID(_objEmp);
                DataRow _dr = dsemp.Tables[0].Rows[0];

                if (Convert.ToInt32(_dr["FAllow"].ToString()) == -1)
                {
                    _dr["FAllow"] = 0;
                }

                DataTable dtDedTablePre = GetGetDeductionsListDB(EmpID);
                                
                int Filing_Stat = 1;
                if (_dr["FStatus"].ToString() == "1") //Married
                {
                    Filing_Stat = 2; //Married
                }
                else if (_dr["FStatus"].ToString() == "0") //Single
                {
                    Filing_Stat = 1; //Single
                }
                DataTable dtDedTableYTD = dtDedTablePre;
                StringBuilder sbDeduct = new StringBuilder();
                StringBuilder sbCityJUR = new StringBuilder();
                double dedAmt401K = 0;
                double dedAmt401KID = 0;
                DataRow drtemp = dtDedTablePre.Select("ID=1").FirstOrDefault();
                if (drtemp != null)
                {
                    dedAmt401K = Convert.ToDouble(drtemp["Amount"]);
                    dedAmt401KID = Convert.ToDouble(drtemp["ID"]);
                }
                if (dtDedTablePre.Rows.Count > 0)
                {
                    dtDedTablePre.DefaultView.RowFilter = "PaidBy = 1";
                    dtDedTablePre = (dtDedTablePre.DefaultView).ToTable();

                    DataTable dtDed = dtDedTablePre;
                    var rowsDed = dtDed.Select("VertexDedutionId = 0 and PaidBy = 1");
                    foreach (var row in rowsDed)
                        row.Delete();
                    dtDed.AcceptChanges();
                    Session.Remove("EmployeeDeductions");
                    Session.Add("EmployeeDeductions", dtDed);

                    foreach (DataRow row in dtDedTablePre.Rows)
                    {

                        double dedAmt = 0;
                        double? dedID = null;
                        if (row["VertexDedutionId"] != DBNull.Value)
                        {
                            dedID = Convert.ToDouble(row["VertexDedutionId"]);
                        }
                        dedAmt = Convert.ToDouble(row["Amount"]);
                        if (dedID != null && dedAmt > 0)
                        {
                            sbDeduct.Append("<DEDUCT>" +
                                " <ID>" + dedID + "</ID> " +
                                "<Amt>" + dedAmt + "</Amt> " +
                                "</DEDUCT>");
                        }
                        else if (dedID == null && dedAmt > 0)
                        {
                            sbDeduct.Append("<DEDUCT>" +
                               " <ID>46</ID> " +
                               "<Amt>" + dedAmt + "</Amt> " +
                               "</DEDUCT>");
                        }
                    }
                }
                else
                {
                    sbDeduct.Append("<DEDUCT>" +
                       " <ID>46</ID> " +
                       "<Amt>" + dedAmt401K + "</Amt> " +
                       "</DEDUCT>");
                }
                string workgeocode = GetWorkGeoCode();
                double totalemp = Convert.ToDouble(((DataTable)Session["PayrollRevenue"]).Compute("SUM(Amount)", string.Empty));
                DataSet dsHourWithLocation = GetPayrollHourWithLocation(EmpID);
                StringBuilder sbWork = new StringBuilder();
                StringBuilder sbWorkInfo = new StringBuilder();
                StringBuilder sbCMP = new StringBuilder();
                StringBuilder sbAggCMP = new StringBuilder();

                List<State> lstStateGeoCode = new List<State>();
                List<Loc> lstGeo = new List<Loc>();
                int cmpId = 0;

                StringBuilder sbQuantum = new StringBuilder();
                StringBuilder sbVprtCompensation = new StringBuilder();

                if (dsHourWithLocation.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in dsHourWithLocation.Tables[0].Rows)
                    {
                        var state = new State();
                        state.geoCode = dr["sGeocode"].ToString();
                        state.Name = dr["lState"].ToString();
                        lstStateGeoCode.Add(state);

                        double Amount = Convert.ToDouble(dr["Amount"]);
                        if (Amount > 0)
                        {
                            if (dr["lCity"].ToString() != "" && dr["lState"].ToString() != "" && dr["lZip"].ToString() != "")
                            {
                                sbWorkInfo.Clear();
                                sbCMP.Clear();
                                string geocode = GetWorkGeo(dr);
                                if (geocode == null)
                                {
                                    geocode = _dr["Geocode"].ToString();
                                }

                                var loc = new Loc();
                                loc.geoCode = geocode;
                                loc.City = dr["lCity"].ToString();
                                lstStateGeoCode.Add(state);
                                lstGeo.Add(loc);

                                sbWorkInfo.Append("<WRKINFO>" +
                                                       "<GEO>" + geocode + "</GEO>" +
                                                   "</WRKINFO>");

                                if (Convert.ToBoolean(dr["FIT"]) == false)
                                {
                                    cmpId = 1100 + Convert.ToInt32(dr["ID"]) + dsHourWithLocation.Tables[0].Rows.IndexOf(dr);
                                    sbVprtCompensation.Append("<VPRT_COMPENSATION>" +
                                                       "<TAXID>400</TAXID>" +
                                                       "<GEO>0</GEO>" +
                                                        "<TAX_TYPE>2</TAX_TYPE>" +
                                                       "<ID>" + cmpId + "</ID>" +
                                                       "<TYPE>r</TYPE>" +
                                                       "<OVTYPE>3</OVTYPE>" +
                                                   "</VPRT_COMPENSATION>");
                                }
                                if (Convert.ToBoolean(dr["FICA"]) == false)
                                {
                                    cmpId = 1100 + Convert.ToInt32(dr["ID"]) + dsHourWithLocation.Tables[0].Rows.IndexOf(dr);
                                    sbVprtCompensation.Append("<VPRT_COMPENSATION>" +
                                                      "<TAXID>403</TAXID>" +
                                                      "<GEO>0</GEO>" +
                                                       "<TAX_TYPE>2</TAX_TYPE>" +
                                                      "<ID>" + cmpId + "</ID>" +
                                                      "<TYPE>r</TYPE>" +
                                                      "<OVTYPE>3</OVTYPE>" +
                                                  "</VPRT_COMPENSATION>");
                                }
                                if (Convert.ToBoolean(dr["MEDI"]) == false)
                                {
                                    cmpId = 1100 + Convert.ToInt32(dr["ID"]) + dsHourWithLocation.Tables[0].Rows.IndexOf(dr);
                                    sbVprtCompensation.Append("<VPRT_COMPENSATION>" +
                                                     "<TAXID>406</TAXID>" +
                                                     "<GEO>0</GEO>" +
                                                      "<TAX_TYPE>2</TAX_TYPE>" +
                                                     "<ID>" + cmpId + "</ID>" +
                                                     "<TYPE>r</TYPE>" +
                                                     "<OVTYPE>3</OVTYPE>" +
                                                 "</VPRT_COMPENSATION>");
                                }
                                if (Convert.ToBoolean(dr["SIT"]) == false)
                                {
                                    cmpId = 1100 + Convert.ToInt32(dr["ID"]) + dsHourWithLocation.Tables[0].Rows.IndexOf(dr);
                                    sbVprtCompensation.Append("<VPRT_COMPENSATION>" +
                                                  "<TAXID>450</TAXID>" +
                                                  "<GEO>" + dr["sGeocode"].ToString() + "</GEO>" +
                                                   "<TAX_TYPE>2</TAX_TYPE>" +
                                                  "<ID>" + cmpId + "</ID>" +
                                                  "<TYPE>r</TYPE>" +
                                                  "<OVTYPE>3</OVTYPE>" +
                                              "</VPRT_COMPENSATION>");
                                }
                                if (Convert.ToBoolean(dr["FUTA"]) == false)
                                {
                                    cmpId = 1100 + Convert.ToInt32(dr["ID"]) + dsHourWithLocation.Tables[0].Rows.IndexOf(dr);
                                    sbVprtCompensation.Append("<VPRT_COMPENSATION>" +
                                                   "<TAXID>401</TAXID>" +
                                                   "<GEO>0</GEO>" +
                                                    "<TAX_TYPE>2</TAX_TYPE>" +
                                                   "<ID>" + cmpId + "</ID>" +
                                                   "<TYPE>r</TYPE>" +
                                                   "<OVTYPE>3</OVTYPE>" +
                                               "</VPRT_COMPENSATION>");
                                }
                                //cmpId = 1100 + Convert.ToInt32(dr["ID"]) + dsHourWithLocation.Tables[0].Rows.IndexOf(dr);
                                sbCMP.Append("<CMP>" +
                                                   "<ID>" + cmpId + "</ID>" +
                                                   "<TYPE>r</TYPE>" +
                                                   "<Amt>" + Amount.ToString() + "</Amt>" +
                                               //"<OVTYPE>1</OVTYPE>" +
                                               "</CMP>");

                                sbWork.Append("<WRK>" + sbWorkInfo + "<CMPARRAY>" + sbCMP + "</CMPARRAY>" + "</WRK>");
                            }
                            else
                            {
                                sbWorkInfo.Clear();
                                sbCMP.Clear();
                                sbWorkInfo.Append("<WRKINFO>" +
                                                    "<GEO>" + _dr["Geocode"].ToString() + "</GEO>" +
                                                "</WRKINFO>");

                                sbCMP.Append("<CMP>" +
                                                 "<ID>0</ID>" +
                                                 "<TYPE>r</TYPE>" +
                                                 "<Amt>" + Amount.ToString() + "</Amt>" +
                                             "</CMP>");

                                sbWork.Append("<WRK>" + sbWorkInfo + "<CMPARRAY>" + sbCMP + "</CMPARRAY>" + "</WRK>");
                            }
                        }
                    }
                }
                else
                {
                    sbWorkInfo.Clear();
                    sbCMP.Clear();
                    sbWorkInfo.Append("<WRKINFO>" +
                                                "<GEO>" + _dr["Geocode"].ToString() + "</GEO>" +
                                            "</WRKINFO>");
                    sbCMP.Append("<CMP>" +
                                     "<ID>0</ID>" +
                                     "<TYPE>r</TYPE>" +
                                     "<Amt>" + totalemp.ToString() + "</Amt>" +
                                 "</CMP>");
                    sbWork.Append("<WRK>" + sbWorkInfo + "<CMPARRAY>" + sbCMP + "</CMPARRAY>" + "</WRK>");
                }

                if (dsHourWithLocation.Tables[1].Rows.Count > 0)
                {
                    double TotalAmt = Convert.ToDouble((dsHourWithLocation.Tables[1]).Compute("SUM(Amount)", string.Empty));
                    foreach (DataRow dr in dsHourWithLocation.Tables[1].Rows)
                    {
                        double Amount = Convert.ToDouble(dr["Amount"]);
                        if (Amount > 0)
                        {
                            sbWorkInfo.Clear();
                            sbCMP.Clear();
                            string geocode = dr["sGeocode"].ToString();
                            sbWorkInfo.Append("<WRKINFO>" +
                                                   "<GEO>" + geocode + "</GEO>" +
                                               "</WRKINFO>");

                            if (Convert.ToBoolean(dr["FIT"]) == false)
                            {
                                cmpId = 1100 + Convert.ToInt32(dr["ID"]) + dsHourWithLocation.Tables[1].Rows.IndexOf(dr);
                                sbVprtCompensation.Append("<VPRT_COMPENSATION>" +
                                                   "<TAXID>400</TAXID>" +
                                                   "<GEO>0</GEO>" +
                                                    "<TAX_TYPE>2</TAX_TYPE>" +
                                                   "<ID>" + cmpId + "</ID>" +
                                                   "<TYPE>r</TYPE>" +
                                                   "<OVTYPE>3</OVTYPE>" +
                                               "</VPRT_COMPENSATION>");
                            }
                            if (Convert.ToBoolean(dr["FICA"]) == false)
                            {
                                cmpId = 1100 + Convert.ToInt32(dr["ID"]) + dsHourWithLocation.Tables[1].Rows.IndexOf(dr);
                                sbVprtCompensation.Append("<VPRT_COMPENSATION>" +
                                                  "<TAXID>403</TAXID>" +
                                                  "<GEO>0</GEO>" +
                                                   "<TAX_TYPE>2</TAX_TYPE>" +
                                                  "<ID>" + cmpId + "</ID>" +
                                                  "<TYPE>r</TYPE>" +
                                                  "<OVTYPE>3</OVTYPE>" +
                                              "</VPRT_COMPENSATION>");
                            }
                            if (Convert.ToBoolean(dr["MEDI"]) == false)
                            {
                                cmpId = 1100 + Convert.ToInt32(dr["ID"]) + dsHourWithLocation.Tables[1].Rows.IndexOf(dr);
                                sbVprtCompensation.Append("<VPRT_COMPENSATION>" +
                                                 "<TAXID>406</TAXID>" +
                                                 "<GEO>0</GEO>" +
                                                  "<TAX_TYPE>2</TAX_TYPE>" +
                                                 "<ID>" + cmpId + "</ID>" +
                                                 "<TYPE>r</TYPE>" +
                                                 "<OVTYPE>3</OVTYPE>" +
                                             "</VPRT_COMPENSATION>");
                            }
                            if (Convert.ToBoolean(dr["SIT"]) == false)
                            {
                                cmpId = 1100 + Convert.ToInt32(dr["ID"]) + dsHourWithLocation.Tables[1].Rows.IndexOf(dr);
                                sbVprtCompensation.Append("<VPRT_COMPENSATION>" +
                                              "<TAXID>450</TAXID>" +
                                              "<GEO>" + dr["sGeocode"].ToString() + "</GEO>" +
                                               "<TAX_TYPE>2</TAX_TYPE>" +
                                              "<ID>" + cmpId + "</ID>" +
                                              "<TYPE>r</TYPE>" +
                                              "<OVTYPE>3</OVTYPE>" +
                                          "</VPRT_COMPENSATION>");
                            }
                            if (Convert.ToBoolean(dr["FUTA"]) == false)
                            {
                                cmpId = 1100 + Convert.ToInt32(dr["ID"]) + dsHourWithLocation.Tables[1].Rows.IndexOf(dr);
                                sbVprtCompensation.Append("<VPRT_COMPENSATION>" +
                                               "<TAXID>401</TAXID>" +
                                               "<GEO>0</GEO>" +
                                                "<TAX_TYPE>2</TAX_TYPE>" +
                                               "<ID>" + cmpId + "</ID>" +
                                               "<TYPE>r</TYPE>" +
                                               "<OVTYPE>3</OVTYPE>" +
                                           "</VPRT_COMPENSATION>");
                            }
                            sbCMP.Append("<CMP>" +
                                                   "<ID>" + cmpId + "</ID>" +
                                                   "<TYPE>r</TYPE>" +
                                                   "<Amt>" + Amount.ToString() + "</Amt>" +
                                               //"<OVTYPE>1</OVTYPE>" +
                                               "</CMP>");

                            sbWork.Append("<WRK>" + sbWorkInfo + "<CMPARRAY>" + sbCMP + "</CMPARRAY>" + "</WRK>");
                        }
                    }
                }

                StringBuilder sbStateJUR = new StringBuilder();
                if (lstStateGeoCode != null)
                {
                    foreach (State state in lstStateGeoCode.DistinctBy(x => x.geoCode).ToList())
                    {
                        string sGeo = state.geoCode;
                        if (!String.IsNullOrEmpty(sGeo))
                        {
                            sbStateJUR.Append(
                                              "<JUR>"
                                                  + "<TAXID>450</TAXID>"
                                                  + "<GEO>" + sGeo + "</GEO>"
                                                   + "<FILING_STAT>" + Filing_Stat + "</FILING_STAT>"
                                                   + "<PRI_EXEMPT>" + _dr["SAllow"].ToString() + "</PRI_EXEMPT>"
                                                  + "<CALCMETH>0</CALCMETH>"
                                                  + "<SUPL_METH>0</SUPL_METH>"
                                              + "</JUR>");
                        }
                    }
                }
                else
                {
                    sbStateJUR.Append(
                        "<JUR>"
                            + "<TAXID>450</TAXID>"
                            + "<GEO>" + _dr["VertexGeocode"].ToString() + "</GEO>"
                             + "<FILING_STAT>" + Filing_Stat + "</FILING_STAT>"
                                 + "<PRI_EXEMPT>" + _dr["SAllow"].ToString() + "</PRI_EXEMPT>"
                            + "<CALCMETH>0</CALCMETH>"
                            + "<SUPL_METH>0</SUPL_METH>"
                        + "</JUR>");
                }

                if (lstGeo != null)
                {
                    //lstGeo = lstGeo.DistinctBy(x => x.geoCode).ToList();
                    foreach (Loc loc in lstGeo.DistinctBy(x => x.geoCode).ToList())
                    {
                        string cGeo = loc.geoCode;
                        if (!String.IsNullOrEmpty(cGeo))
                        {
                            sbCityJUR.Append("<JUR>" +
                                               "<TAXID>530</TAXID>" +
                                               "<GEO>" + cGeo + "</GEO>" +
                                               "<FILING_STAT>" + Filing_Stat + "</FILING_STAT>" +
                                               "<PRI_EXEMPT>0</PRI_EXEMPT>" +
                                               "<CALCMETH>0</CALCMETH>" +
                                               "<SUPL_METH>0</SUPL_METH>" +
                                           "</JUR>");
                        }
                    }
                }

                //////////////////////////////////////////
                DataTable dtDeduction = GetDeductionTable();
                string empgeocode = "000000000";
                string strerrorMessage = "";
                //string workgeocode = GetWorkGeoCode();

                double FITdouble = 0;
                double SITdouble = 0;
                double Localdouble = 0;
                double Citydouble = 0;
                double Medidouble = 0;
                double Ficadouble = 0;
                string code = "";
                //Credentials for Payroll OnDemand
                string username = ConfigurationManager.AppSettings["vertexApiUsername"].ToString(); // "dread@1000";
                string passWord = ConfigurationManager.AppSettings["vertexApiPassword"].ToString(); // "K3CHccxQ";
                // Setting up the URI for Payroll
                //string uri = "https://payrollsandbox.ondemand.vertexinc.com:443/EiWebSvc/AddressWebService";
                string uri = ConfigurationManager.AppSettings["vertexAddressURL"].ToString();
                string payrolluri = ConfigurationManager.AppSettings["vertexPayrollURL"].ToString(); //"https://payrollsandbox.ondemand.vertexinc.com:443/EiWebSvc/PayTaxCalcWebService";
                var chkdateString = DateTime.Today.ToString("yyyyMMdd");

                string payrollXML = "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:eic=\"http://EiCalc/\">"
                + "<soapenv:Header/>"
        + "<soapenv:Body>"
            + "<eic:EiOperation>"
                + "<Request>"
                    + "<![CDATA["
                        + "<EMP>"
                            + "<EMPINFO>"
                            + "<EMPSTRUCT>"
                                //+ "<EMPID>" + _dr["fFirst"].ToString() + "</EMPID>"
                                + "<EMPID>ESS</EMPID>"
                                + "<PAYDATE>" + chkdateString + "</PAYDATE>";
                if (_dr["PayPeriod"].ToString() == "0") //Weekly
                {
                    payrollXML = payrollXML + "<PAYPERIODS>52</PAYPERIODS>";
                }
                else if (_dr["PayPeriod"].ToString() == "1") //Bi-Weekly
                {
                    payrollXML = payrollXML + "<PAYPERIODS>26</PAYPERIODS>";
                }
                else
                {
                    payrollXML = payrollXML + "<PAYPERIODS>1</PAYPERIODS>";
                }
                payrollXML = payrollXML + "<CURPERIOD>1</CURPERIOD>"
            + "<RES_GEO>" + _dr["Geocode"].ToString() + "</RES_GEO>"
            //+ "<PRIMARY_WORK_GEO>" + workgeocode + "</PRIMARY_WORK_GEO>"
            + "<PRIMARY_WORK_GEO>" + _dr["Geocode"].ToString() + "</PRIMARY_WORK_GEO>"
        + "</EMPSTRUCT>"
        + "<DEDUCTARRAY>"
            + sbDeduct
        + "</DEDUCTARRAY>"
        + "</EMPINFO>"
        + sbWork
        //+sbQuantum
        + "<JURARRAY>"
            + "<JUR>"
                + "<TAXID>400</TAXID>"
                + "<GEO>0</GEO>"
                + "<FILING_STAT>" + Filing_Stat + "</FILING_STAT>"
                 + "<PRI_EXEMPT>" + _dr["FAllow"].ToString() + "</PRI_EXEMPT>"
            + "<CALCMETH>0</CALCMETH>"
            + "<SUPL_METH>0</SUPL_METH>"
        + "</JUR>"
        + sbStateJUR
        + sbCityJUR
    + "</JURARRAY>"
                //if (sbAggCMP != null)
                //{
                //    payrollXML = payrollXML + "<AGGCMPARRAY>" + sbAggCMP + "</AGGCMPARRAY>";
                //}
                //payrollXML = payrollXML
        + "<TAXAMTARRAY>"
        + "<AGGTAX>"
            + "<TAXID>400</TAXID>"
            + "<GEO>0</GEO>"
            + "<TAX_AMT>0</TAX_AMT>"
            + "<AGG_TYPE>Y</AGG_TYPE>"
            + "<TYPE>r</TYPE>"
            + "<AGG_ADJ_GROSS>0</AGG_ADJ_GROSS>"
        + "</AGGTAX>"
    + "</TAXAMTARRAY>"
    + "<QUANTUM>"
    + "<VPRT_COMPENSATION_ARRAY>";
                //+sbVprtCompensation
                if (sbVprtCompensation != null)
                {
                    payrollXML = payrollXML + sbVprtCompensation;
                }
                payrollXML = payrollXML
                + "</VPRT_COMPENSATION_ARRAY>"
    + "</QUANTUM>"
    + "</EMP>"
    + "]]>"
    + "</Request>"
    + "</eic:EiOperation>"
    + "</soapenv:Body>"
    + "</soapenv:Envelope> ";

                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                HttpClient clpay = new HttpClient();
                clpay.BaseAddress = new Uri(payrolluri);
                clpay.DefaultRequestHeaders.Clear();
                clpay.DefaultRequestHeaders.Add("javax.xml.ws.security.auth.username", username);
                clpay.DefaultRequestHeaders.Add("javax.xml.ws.security.auth.password", passWord);
                HttpContent soapPayrollCalcTax = new StringContent(payrollXML);
                soapPayrollCalcTax.Headers.Clear();
                soapPayrollCalcTax.Headers.Add("Content-Type", "text/xml");
                AddVertexLog(EmpID.ToString(), payrollXML, "");
                using (HttpResponseMessage response = clpay.PostAsync(payrolluri, soapPayrollCalcTax).Result)
                {
                    string rawString = getMessage(response).Result;

                    XmlDocument responseXML = new XmlDocument();
                    responseXML.LoadXml(rawString);
                    XDocument responseXMLPretty = XDocument.Parse(responseXML.InnerText.ToString());

                    string responseXMLPrettystrr = responseXMLPretty.ToString();
                    responseXMLPrettystrr = responseXMLPrettystrr.Replace("\"", "'");
                    XmlDocument Docc = new XmlDocument();
                    Docc.LoadXml(responseXMLPrettystrr);
                    //XmlNode nodedocc = Docc.GetElementsByTagName("TaxOut").Item(0);

                    XmlNode Errornode = Docc.GetElementsByTagName("Error").Item(0);
                    if (Errornode == null)
                    {
                        XmlNodeList elemList = Docc.GetElementsByTagName("TaxOut");
                        for (int j = 0; j < elemList.Count; j++)
                        {
                            if (elemList[j]["TAXID"].InnerText == "400")
                            {
                                FITdouble = Convert.ToDouble(elemList[j]["TAX_AMT"].InnerText);
                                DataRow dr = dtDeduction.NewRow();
                                dr["ID"] = 50006;
                                dr["fDesc"] = "Federal Tax";
                                dr["Amount"] = FITdouble;
                                dr["PaidBy"] = 1;
                                dr["YTDAmount"] = 0.00;
                                dtDeduction.Rows.Add(dr);
                            }
                            if (elemList[j]["TAXID"].InnerText == "403")
                            {
                                Ficadouble = Convert.ToDouble(elemList[j]["TAX_AMT"].InnerText);
                                DataRow dr = dtDeduction.NewRow();
                                dr["ID"] = 50002;
                                dr["fDesc"] = "FICA";
                                dr["Amount"] = Ficadouble;
                                dr["PaidBy"] = 1;
                                dr["YTDAmount"] = 0.00;
                                dtDeduction.Rows.Add(dr);
                            }
                            if (elemList[j]["TAXID"].InnerText == "406")
                            {
                                Medidouble = Convert.ToDouble(elemList[j]["TAX_AMT"].InnerText);
                                DataRow dr = dtDeduction.NewRow();
                                dr["ID"] = 50003;
                                dr["fDesc"] = "MEDI";
                                dr["Amount"] = Medidouble;
                                dr["PaidBy"] = 1;
                                dr["YTDAmount"] = 0.00;
                                dtDeduction.Rows.Add(dr);
                            }
                            if (elemList[j]["TAXID"].InnerText == "450")
                            {
                                string state = "";
                                foreach (State st in lstStateGeoCode)
                                {
                                    if (st.geoCode == elemList[j]["GEO"].InnerText)
                                        state = st.Name;
                                }
                                SITdouble = Convert.ToDouble(elemList[j]["TAX_AMT"].InnerText);
                                DataRow dr = dtDeduction.NewRow();
                                dr["ID"] = 50005;
                                dr["fDesc"] = "SIT-" + state;
                                dr["Amount"] = SITdouble;
                                dr["PaidBy"] = 1;
                                dr["YTDAmount"] = 0.00;
                                dtDeduction.Rows.Add(dr);
                            }
                            if (elemList[j]["TAXID"].InnerText == "530")
                            {
                                string city = "";
                                foreach (Loc loc in lstGeo)
                                {
                                    if (loc.geoCode == elemList[j]["GEO"].InnerText)
                                        city = loc.City;
                                }
                                Citydouble = Convert.ToDouble(elemList[j]["TAX_AMT"].InnerText);
                                DataRow dr = dtDeduction.NewRow();
                                dr["ID"] = 3;
                                dr["fDesc"] = "City-" + city;
                                dr["Amount"] = Citydouble;
                                dr["PaidBy"] = 1;
                                dtDeduction.Rows.Add(dr);
                            }
                        }
                    }
                    else
                    {
                        strerrorMessage = "Error in Address API for " + _dr["Name"].ToString();
                    }

                    AddVertexLog(EmpID.ToString(), payrollXML, responseXMLPrettystrr);

                    soapPayrollCalcTax.Dispose();
                    clpay.Dispose();
                }

                // Below call to read the message back from server from stream to string
                async Task<string> getMessage(HttpResponseMessage messageFromServer)
                {
                    code = await messageFromServer.Content.ReadAsStringAsync();
                    return code;
                }

                //////////////////////////////////////////
                //DataTable dtDedTable = GetGetDeductionsListDB(EmpID);
                foreach (DataRow drYTD in dtDedTableYTD.Rows)
                {
                    if (Convert.ToInt32(drYTD["ID"].ToString()) == 50002 || Convert.ToInt32(drYTD["ID"].ToString()) == 50003 || Convert.ToInt32(drYTD["ID"].ToString()) == 50004 || Convert.ToInt32(drYTD["ID"].ToString()) == 50005 || Convert.ToInt32(drYTD["ID"].ToString()) == 50006)
                    {
                        DataRow dr = dtDeduction.Select("ID=" + Convert.ToInt32(drYTD["ID"].ToString()) + "").FirstOrDefault();
                        if (dr != null)
                        {
                            dr["YTDAmount"] = Convert.ToDouble(drYTD["YTDAmount"].ToString()) + Convert.ToDouble(dr["Amount"].ToString());
                        }
                    }
                    else
                    {
                        dtDeduction.ImportRow(drYTD);
                    }

                }

                DataRow drrr = dtDeduction.NewRow();
                //dr["ID"] = elemList[j]["TAXID"].InnerText;
                drrr["ID"] = 50001;
                drrr["fDesc"] = "Local";
                drrr["Amount"] = Localdouble;
                drrr["PaidBy"] = 1;
                drrr["YTDAmount"] = 0.00;
                dtDeduction.Rows.Add(drrr);

                //dtDeduction.DefaultView.RowFilter = "PaidBy = 1";
                //dtDeduction = (dtDeduction.DefaultView).ToTable();

                dtDeduction.DefaultView.Sort = "ID DESC";
                RadGrid_Deduction.VirtualItemCount = dtDeduction.Rows.Count;
                RadGrid_Deduction.DataSource = dtDeduction;
                ////RadGridPayrollHours.DataBind();
                ViewState["VirtualItemCountDeductions"] = dtDeduction.Rows.Count;

                Session["PayrollDedutions"] = dtDeduction;

                double totaldeduction = Convert.ToDouble(dtDeduction.Compute("SUM(Amount)", "PaidBy = 1"));
                double totalnet = totalemp - totaldeduction;
                txttotalWages.Text = String.Format("{0:C}", totalemp);
                txttotaldeductions.Text = String.Format("{0:C}", totaldeduction);
                txtnetpay.Text = String.Format("{0:C}", totalnet);

            }

            catch (Exception ex)
            {
                string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
                ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            }
        }

        protected void RadGridPayrollHours_PreRender(object sender, EventArgs e)
        {
            #region Save the Grid Filter
            String filterExpression = Convert.ToString(RadGridPayrollHours.MasterTableView.FilterExpression);
            if (filterExpression != "")
            {
                Session["ServiceType_FilterExpression"] = filterExpression;
                List<RetainFilter> filters = new List<RetainFilter>();

                foreach (GridColumn column in RadGridPayrollHours.MasterTableView.OwnerGrid.Columns)
                {
                    String filterValues = column.CurrentFilterValue;
                    if (filterValues != "")
                    {
                        String columnName = column.UniqueName;
                        RetainFilter filter = new RetainFilter();
                        filter.FilterColumn = columnName;
                        filter.FilterValue = filterValues;
                        filters.Add(filter);
                    }
                }
                Session["ServiceType_Filters"] = filters;
            }
            else
            {
                Session["ServiceType_FilterExpression"] = null;
                Session["ServiceType_Filters"] = null;
            }

            GeneralFunctions obj = new GeneralFunctions();
            obj.CorrectTelerikPager(RadGridPayrollHours);
            #endregion
            RowSelect();
        }
        private void RowSelect()
        {
            foreach (GridDataItem gr in RadGridPayrollHours.Items)
            {
                Label lblID = (Label)gr.FindControl("lblId");
                //HyperLink lnkName = (HyperLink)gr.FindControl("lnkRef");
                gr.Attributes["ondblclick"] = "OpenServiceTypeWindowEditDoubleclick('" + lblID.Text + "');";

            }
        }

        private const int ItemsPerRequest = 10;
        private void FillServiceType()
        {
            DataSet ds = new DataSet();
            objProp_User.ConnConfig = Session["config"].ToString();

            ds = new BL_ServiceType().GetServiceType(objProp_User.ConnConfig);

            RadGridPayrollHours.VirtualItemCount = ds.Tables[0].Rows.Count;
            RadGridPayrollHours.DataSource = ds.Tables[0];
        }

        bool isGroupingServiceType = false;
        public bool ShouldApplySortServiceType()
        {
            return RadGridPayrollHours.MasterTableView.FilterExpression != "" ||
                (RadGridPayrollHours.MasterTableView.GroupByExpressions.Count > 0 || isGroupingServiceType) ||
                RadGridPayrollHours.MasterTableView.SortExpressions.Count > 0;
        }
        protected void RadGridPayrollHours_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            RadGridPayrollHours.AllowCustomPaging = !ShouldApplySortServiceType();
            //FillServiceType();
            GetHourList(Convert.ToInt32(Request.QueryString["id"].ToString()));
        }
        protected void RadGrid_Revenues_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            RadGrid_Revenues.AllowCustomPaging = !ShouldApplySortServiceType();
            GetRevenues(Convert.ToInt32(Request.QueryString["id"].ToString()));
        }
        protected void RadGrid_Deduction_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            RadGrid_Deduction.AllowCustomPaging = !ShouldApplySortServiceType();

            GetDeductions(Convert.ToInt32(Request.QueryString["id"].ToString()));
        }
        protected void RadGridPayrollHours_ItemCreated(object sender, GridItemEventArgs e)
        {
            try
            {
                if (e.Item is GridPagerItem)
                {
                    var dropDown = (RadComboBox)e.Item.FindControl("PageSizeComboBox");
                    var totalCount = ((GridPagerItem)e.Item).Paging.DataSourceCount;
                    if (totalCount == 0) totalCount = 1000;
                    GeneralFunctions obj = new GeneralFunctions();
                    var sizes = obj.TelerikPageSize(totalCount);
                    dropDown.Items.Clear();

                    foreach (var size in sizes)
                    {
                        var cboItem = new RadComboBoxItem() { Text = size.Key, Value = size.Value };
                        cboItem.Attributes.Add("ownerTableViewId", e.Item.OwnerTableView.ClientID);
                        if (e.Item.OwnerTableView.PageSize.ToString() == size.Value) cboItem.Selected = true;
                        dropDown.Items.Add(cboItem);
                    }
                }
            }
            catch { }
        }
        protected void lnkUpdateHours_Click(object sender, EventArgs e)
        {
            try
            {
                string msg = string.Empty;
                _objWage.ConnConfig = Session["config"].ToString();
                DateTime FStart = Convert.ToDateTime(Request.QueryString["sDt"].ToString());
                DateTime Edate = Convert.ToDateTime(Request.QueryString["eDt"].ToString());
                int Eid = Convert.ToInt32(Request.QueryString["id"].ToString());
                string _Perioddesc = Convert.ToString(Request.QueryString["sPeriodDesc"].ToString());
                int _WeekNo = Convert.ToInt32(Request.QueryString["sWeekNo"].ToString());
                foreach (GridDataItem di in RadGridPayrollHours.Items)
                {
                    Label lblDedID1 = (Label)di.FindControl("lblDedID");
                    TextBox lblRegular = (TextBox)di.FindControl("lblRegular");
                    TextBox lblOvertime = (TextBox)di.FindControl("lblOvertime");
                    TextBox lbl17Cat = (TextBox)di.FindControl("lbl17Cat");
                    TextBox lblDoubleTime = (TextBox)di.FindControl("lblDoubleTime");
                    TextBox lblTravelTime = (TextBox)di.FindControl("lblTravelTime");

                    _objWage.ID = Convert.ToInt32(lblDedID1.Text);
                    _objWage.Reg = Convert.ToDouble(lblRegular.Text);
                    _objWage.OT1 = Convert.ToDouble(lblOvertime.Text);
                    _objWage.OT2 = Convert.ToDouble(lblDoubleTime.Text);
                    _objWage.NT = Convert.ToDouble(lbl17Cat.Text);
                    _objWage.TT = Convert.ToDouble(lblTravelTime.Text);

                    objBL_Wage.UpdatePayrollCalculation(_objWage, payrollResigerId, Eid);
                }
                foreach (GridDataItem di in RadGridOtherWage.Items)
                {
                    Label lblRateCategoryId = (Label)di.FindControl("lblRateCategoryId");
                    Label lblOtherWageId = (Label)di.FindControl("lblOtherWageId");
                    TextBox lblQuantity = (TextBox)di.FindControl("lblQuantity");
                    TextBox lblAmount = (TextBox)di.FindControl("lblAmount");

                    double OtherWageValue = 0.0;
                    char? calculationType = null;
                    int OtherWageId = Convert.ToInt32(lblOtherWageId.Text);
                    switch (Convert.ToInt32(lblRateCategoryId.Text))
                    {
                        case 6:
                            calculationType = 'F';
                            bonus = Convert.ToDouble(lblAmount.Text);
                            break;
                        case 7:
                            calculationType = 'F';
                            holiday = OtherWageValue = Convert.ToDouble(lblQuantity.Text);
                            break;
                        case 8:
                            calculationType = 'F';
                            vacation = OtherWageValue = Convert.ToDouble(lblQuantity.Text);
                            break;
                        case 9:
                            calculationType = 'F';
                            zone = OtherWageValue = Convert.ToDouble(lblAmount.Text);
                            break;
                        case 10:
                            calculationType = 'F';
                            reimb = OtherWageValue = Convert.ToDouble(lblAmount.Text);
                            break;
                        case 11:
                            calculationType = 'F';
                            mileage = OtherWageValue = Convert.ToDouble(lblQuantity.Text);
                            break;
                        case 12:
                            calculationType = 'F';
                            sick = OtherWageValue = Convert.ToDouble(lblAmount.Text);
                            break;
                    }

                    objBL_Wage.UpdateOtherWage(_objWage, OtherWageId, OtherWageValue, calculationType, payrollResigerId, Eid);
                }
                //objBL_Wage.AddPRWItemSession(_objWage, FStart, Edate, Eid, _Perioddesc, _WeekNo);
                //msg = "Updated";

                GetHourList(Convert.ToInt32(Request.QueryString["id"].ToString()));
                GetRevenues(Convert.ToInt32(Request.QueryString["id"].ToString()));
                GetDeductions(Convert.ToInt32(Request.QueryString["id"].ToString()));

                RadGridPayrollHours.Rebind();
                RadGrid_Revenues.Rebind();
                RadGrid_Deduction.Rebind();

                ScriptManager.RegisterStartupScript(Page, typeof(Page), "RefreshScript", "RefreshParent();", true);
                //ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccAddCusttype", "noty({text: 'Customer Type " + msg + " Successfully', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
            }
            catch (Exception ex)
            {
                string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyAddCusttype", "noty({text: '" + str + "', dismissQueue: true,  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
            }
        }

        #region Total in Footer         
        //Author.-   Arvindra Singh             Jun-22-2021 
        double totalReg = 0;
        double totalOvertime = 0;
        double total17 = 0;
        double totalDouble = 0;
        double totalTravelTime = 0;
        double totalRQuan = 0;
        double totalRRate = 0;
        double totalRAmt = 0;
        double totalRAmtYTD = 0;
        double totalDAmt = 0;
        double totalDAmtYTD = 0;
        protected void RadGridPayrollHours_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                if (e.Item is GridDataItem)
                {
                    GridDataItem dataItem = e.Item as GridDataItem;
                    TextBox regValue = (TextBox)dataItem.FindControl("lblRegular");
                    TextBox overValue = (TextBox)dataItem.FindControl("lblOvertime");
                    TextBox _17Value = (TextBox)dataItem.FindControl("lbl17Cat");
                    TextBox doubleValue = (TextBox)dataItem.FindControl("lblDoubleTime");
                    TextBox travelValue = (TextBox)dataItem.FindControl("lblTravelTime");
                    totalReg += Convert.ToDouble(regValue.Text);
                    totalOvertime += Convert.ToDouble(overValue.Text);
                    total17 += Convert.ToDouble(_17Value.Text);
                    totalDouble += Convert.ToDouble(doubleValue.Text);
                    totalTravelTime += Convert.ToDouble(travelValue.Text);
                }
                if (e.Item is GridFooterItem)
                {
                    GridFooterItem footerItem = e.Item as GridFooterItem;
                    footerItem["Reg"].Text = String.Format("{0:N2}", totalReg);
                    footerItem["OT"].Text = String.Format("{0:N2}", totalOvertime);
                    footerItem["NT"].Text = String.Format("{0:N2}", total17);
                    footerItem["DT"].Text = String.Format("{0:N2}", totalDouble);
                    footerItem["TT"].Text = String.Format("{0:N2}", totalTravelTime);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        protected void RadGrid_Revenues_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                if (e.Item is GridDataItem)
                {
                    GridDataItem dataItem = e.Item as GridDataItem;
                    Label lblRQuan = (Label)dataItem.FindControl("lblRQuan");
                    Label lblRRate = (Label)dataItem.FindControl("lblRRate");
                    Label lblRAmt = (Label)dataItem.FindControl("lblRAmt");
                    Label lblRAmtYTD = (Label)dataItem.FindControl("lblRAmtYTD");
                    totalRQuan += Convert.ToDouble(lblRQuan.Text);
                    totalRRate += Convert.ToDouble(lblRRate.Text);
                    totalRAmt += Convert.ToDouble(lblRAmt.Text);
                    totalRAmtYTD += Convert.ToDouble(lblRAmtYTD.Text);
                }
                if (e.Item is GridFooterItem)
                {
                    GridFooterItem footerItem = e.Item as GridFooterItem;
                    footerItem["Quan"].Text = totalRQuan.ToString();
                    footerItem["Rate"].Text = totalRRate.ToString();
                    footerItem["Amount"].Text = totalRAmt.ToString();
                    footerItem["YTDAmount"].Text = totalRAmtYTD.ToString();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        protected void RadGrid_Deduction_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                if (e.Item is GridDataItem)
                {
                    GridDataItem dataItem = e.Item as GridDataItem;
                    Label lblDAmt = (Label)dataItem.FindControl("lblDAmt");
                    Label lblDAmtYTD = (Label)dataItem.FindControl("lblDAmtYTD");
                    totalDAmt += Convert.ToDouble(lblDAmt.Text);
                    totalDAmtYTD += Convert.ToDouble(lblDAmtYTD.Text);
                }
                if (e.Item is GridFooterItem)
                {
                    GridFooterItem footerItem = e.Item as GridFooterItem;
                    footerItem["Amount"].Text = totalDAmt.ToString();
                    footerItem["YTDAmount"].Text = totalDAmtYTD.ToString();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        #endregion Total in Footer

        #region GetGeocode for work location
        private string GetWorkGeo(DataRow _dr)
        {
            string workgeo = "";
            string code = "";
            //////////////////////////////////////////
            //Credentials for Payroll OnDemand
            string username = ConfigurationManager.AppSettings["vertexApiUsername"].ToString();  //"nmishra@986057068";
            string passWord = ConfigurationManager.AppSettings["vertexApiPassword"].ToString(); // "fkl8TM2E";

            // Setting up the URI for Payroll
            string uri = ConfigurationManager.AppSettings["vertexAddressURL"].ToString(); // "https://payrollsandbox.ondemand.vertexinc.com:443/EiWebSvc/AddressWebService";

            string addrClnXML = "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:eic = \"http://EiCalc/\">"
                + "<soapenv:Header/>"
                + "<soapenv:Body>"
                + "<eic:AddrCleanse>"
                + "<Request>"
                + "<![CDATA["
                    + "<ADDRESS_CLEANSE_REQUEST>"
                        + "<StreetAddress1></StreetAddress1>"
                        + "<CityName>" + _dr["lCity"].ToString() + "</CityName>"
                        + "<StateName>" + _dr["lState"].ToString() + "</StateName>"
                        + "<ZipCode>" + _dr["lZip"].ToString() + "</ZipCode>"
                    + "</ADDRESS_CLEANSE_REQUEST>]]>"
                + "</Request>"
                + "</eic:AddrCleanse>"
                + "</soapenv:Body>"
                + "</soapenv:Envelope>";
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                HttpClient cl = new HttpClient();
                cl.BaseAddress = new Uri(uri);
                cl.DefaultRequestHeaders.Clear();
                cl.DefaultRequestHeaders.Add("javax.xml.ws.security.auth.username", username);
                cl.DefaultRequestHeaders.Add("javax.xml.ws.security.auth.password", passWord);
                HttpContent soapAddressEnvelope = new StringContent(addrClnXML);
                soapAddressEnvelope.Headers.Clear();
                soapAddressEnvelope.Headers.Add("Content-Type", "text/xml");
                using (HttpResponseMessage response = cl.PostAsync(uri, soapAddressEnvelope).Result)
                {
                    try
                    {
                        string rawString = getMessage(response).Result;
                        XmlDocument responseXML = new XmlDocument();
                        responseXML.LoadXml(rawString);
                        XDocument responseXMLPretty = XDocument.Parse(responseXML.InnerText.ToString());
                        string responseXMLPrettystr = responseXMLPretty.ToString();
                        responseXMLPrettystr = responseXMLPrettystr.Replace("\"", "'");
                        XmlDocument Doc = new XmlDocument();
                        Doc.LoadXml(responseXMLPrettystr);
                        XmlNode Errornode = Doc.GetElementsByTagName("Error").Item(0);
                        if (Errornode == null)
                        {
                            XmlNode nodedoc = Doc.GetElementsByTagName("GeoCode").Item(0);
                            workgeo = nodedoc.ChildNodes.Item(0).InnerText;
                        }
                        else
                        {
                            workgeo = null;
                        }
                    }
                    catch (Exception ex)
                    {
                        workgeo = null;
                    }
                    soapAddressEnvelope.Dispose();
                    cl.Dispose();
                }
            }

            catch (AggregateException aggEx)
            {
                //If an error occurs outside of the service call capture and show below
                //Console.WriteLine("A Connection error occurred");
                //Console.WriteLine("----------------------------------------------------");
                //Console.WriteLine("Error Code: " + aggEx.Message.ToString() + Environment.NewLine + "Message: " + aggEx.InnerException.Message.ToString());

            }
            async Task<string> getMessage(HttpResponseMessage messageFromServer)
            {
                code = await messageFromServer.Content.ReadAsStringAsync();
                return code;
            }
            return workgeo;
        }
        #endregion
        public DataSet GetPayrollHourWithLocation(int EmpID)
        {
            //try
            //{
            User objPropUser = new User();
            DataSet ds = new DataSet();
            objPropUser.ConnConfig = Session["config"].ToString();
            objPropUser.FStart = Convert.ToDateTime(Request.QueryString["sDt"].ToString());
            objPropUser.Edate = Convert.ToDateTime(Request.QueryString["eDt"].ToString());
            objPropUser.ID = EmpID;
            objPropUser.HolidayAm = holiday;
            objPropUser.VacAm = vacation;
            objPropUser.ZoneAm = zone;
            objPropUser.ReimbAm = reimb;
            objPropUser.MilageAm = mileage;
            objPropUser.Bonus = bonus;
            objPropUser.SickAm = sick;


            ds = new BL_Wage().GetPayrollHourWithLocation(objPropUser);
            return ds;
            //}
            //catch (Exception ex)
            //{
            //    string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            //    ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            //}
        }

        public void AddVertexLog(string EmpName, string request, string response)
        {
            //try
            //{
            string ConnConfig = Session["config"].ToString();
            new BL_Wage().AddVertexLog(ConnConfig, EmpName, request, response);
            //}
            //catch (Exception ex)
            //{
            //    string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            //    ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            //}
        }

        protected void RadGridOtherWage_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                if (e.Item is GridDataItem)
                {
                    GridDataItem dataItem = e.Item as GridDataItem;

                    Label lblOtherWageId = (Label)dataItem.FindControl("lblOtherWageId");
                    Label lblRateCategoryId = (Label)dataItem.FindControl("lblRateCategoryId");
                    TextBox txtQuantity = (TextBox)dataItem.FindControl("lblQuantity");
                    TextBox txtRate = (TextBox)dataItem.FindControl("lblRate");
                    TextBox txtAmount = (TextBox)dataItem.FindControl("lblAmount");

                    txtRate.ReadOnly = true;

                    int rateCategoryId = Convert.ToInt32(lblRateCategoryId.Text.Trim().ToString());

                    if (rateCategoryId == 6 || rateCategoryId == 9 || rateCategoryId == 10)
                    {
                        txtQuantity.ReadOnly = true;
                    }
                    else if (rateCategoryId == 7 || rateCategoryId == 8 || rateCategoryId == 11 || rateCategoryId == 12)
                    {
                        //txtAmount.Text = (Convert.ToDouble(txtQuantity.Text) + Convert.ToDouble(txtRate.Text)).ToString();
                        txtAmount.ReadOnly = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        protected void lnkView_Click(object sender, EventArgs e)
        {
            int empId = Convert.ToInt32(Request.QueryString["id"].ToString());
            //string URL = "~/PayrollCalculationDetials.aspx?empId=" + "empId";
            //Page.ClientScript.RegisterStartupScript(this.GetType(), "OpenWindow", "window.open(" + URL + ",'_newtab');", true);
            Response.Redirect(string.Format("~/PayrollCalculationDetials.aspx?empId={0}", empId));
        }
    }
}
