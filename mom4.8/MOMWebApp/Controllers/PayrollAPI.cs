using BusinessEntity;
using BusinessLayer;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Linq;

namespace MOMWebApp
{
    public class PayrollAPIController : ApiController
    {
        BL_User objBL_User = new BL_User();
        BL_Wage objBL_Wage = new BL_Wage();
        User objPropUser = new User();
        Emp _objEmp = new Emp();
        string sessionConfig = "";

        [HttpPost]
        public string GetWageDeductionList(FilterModel filterModel)
        {
            string JSONString = string.Empty;
            try
            {
                Wage objProp_Wage = new Wage();
                DataSet ds = new DataSet();
                var session = HttpContext.Current.Session;
                if (session != null)
                {
                    if (session["config"] != null)
                    {
                        objProp_Wage.ConnConfig = session["config"].ToString();
                    }
                }
                if (filterModel != null)
                {
                    if (filterModel.Active == null)
                    {
                        objProp_Wage.Status = null;
                    }
                    else
                    {
                        objProp_Wage.Status = Convert.ToInt16(filterModel.Active);
                    }

                    if (filterModel.Filters != null && filterModel.Filters.Count > 0)
                    {
                        foreach (var filter in filterModel.Filters)
                        {
                            switch (filter.Key.ToString())
                            {
                                case "Field":
                                    objProp_Wage.Field = Convert.ToInt16(filter.Value);
                                    break;
                                case "fDesc":
                                    objProp_Wage.Name = filter.Value;
                                    break;
                                case "Reg":
                                    objProp_Wage.Reg = Convert.ToInt32(filter.Value);
                                    break;
                                case "OT1":
                                    objProp_Wage.OT1 = Convert.ToInt32(filter.Value);
                                    break;
                                case "OT2":
                                    objProp_Wage.OT2 = Convert.ToInt32(filter.Value);
                                    break;
                                case "NT":
                                    objProp_Wage.NT = Convert.ToInt32(filter.Value);
                                    break;
                                case "TT":
                                    objProp_Wage.TT = Convert.ToInt32(filter.Value);
                                    break;
                                case "sStatus":
                                    objProp_Wage.Status = Convert.ToInt16(filter.Value);
                                    break;
                            }
                        }
                    }
                    else
                    {
                        objProp_Wage.Field = null;
                        objProp_Wage.Reg = null;
                        objProp_Wage.OT1 = null;
                        objProp_Wage.OT2 = null;
                        objProp_Wage.NT = null;
                        objProp_Wage.TT = null;
                        //objProp_Wage.Status = null;
                    }

                }
                ds = new BL_Wage().getWage(objProp_Wage, filterModel.PageNumber, filterModel.PageSize, filterModel.SortField, filterModel.SortOrder);
                JSONString = JsonConvert.SerializeObject(ds.Tables[0]);
            }
            catch (Exception ex)
            {
                JSONString = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
                //string st = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
                //ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            }
            return JSONString;
        }

        [HttpPost]
        public string GetPayrollList(FilterModel filterModel)
        {
            string JSONString = string.Empty;
            try
            {
                PayrollRegisterModel objRegister = new PayrollRegisterModel();
                DataSet ds = new DataSet();
                var session = HttpContext.Current.Session;
                if (session != null)
                {
                    if (session["config"] != null)
                    {
                        objRegister.ConnConfig = session["config"].ToString();
                    }
                }
                if (filterModel != null)
                {
                    if (filterModel.Active == null)
                    {
                        objRegister.Status = null;
                    }
                    else
                    {
                        objRegister.Status = Convert.ToInt16(filterModel.Active);
                    }

                    if (filterModel.Filters != null && filterModel.Filters.Count > 0)
                    {
                        foreach (var filter in filterModel.Filters)
                        {
                            switch (filter.Key.ToString())
                            {
                                case "PayFrequency":
                                    objRegister.FrequencyId = Convert.ToInt16(filter.Value);
                                    break;
                                case "StartDate":
                                    objRegister.StartDate = Convert.ToDateTime(filter.Value);
                                    break;
                                case "EndDate":
                                    objRegister.EndDate = Convert.ToDateTime(filter.Value);
                                    break;
                                case "Status":
                                    objRegister.Status = Convert.ToInt16(filter.Value);
                                    break;
                            }
                        }
                    }
                    else
                    {

                    }

                }
                ds = new BL_Wage().GetPayrollRegister(objRegister, filterModel.PageNumber, filterModel.PageSize, filterModel.SortField, filterModel.SortOrder);
                JSONString = JsonConvert.SerializeObject(ds.Tables[0]);
            }
            catch (Exception ex)
            {
                JSONString = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
                //string st = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
                //ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            }
            return JSONString;
        }

        [HttpPost]
        public string AddPayrollRagister(PayrollRegisterModel payroll)
        {
            string JSONString = string.Empty;
            PRReg _objPRReg = new PRReg();
            try
            {
                Wage objProp_Wage = new Wage();
                //DataSet ds = new DataSet();
                var session = HttpContext.Current.Session;
                if (session != null)
                {
                    if (session["config"] != null)
                    {
                        sessionConfig = session["config"].ToString();
                    }
                }
                objProp_Wage.ConnConfig = sessionConfig;
                // ExceptionLogging.SendMsgToText("Payroll Register Update Start: ");
                DataTable dt = GetTable();
                string code = "";
                string empgeocode = "000000000";
                // ExceptionLogging.SendMsgToText("Get Work Geo Code Start: ");
                string workgeocode = GetWorkGeoCode();
                // ExceptionLogging.SendMsgToText("Get Work Geo Code End: ");
                double FITdouble = 0;
                double SITdouble = 0;
                double Localdouble = 0;
                double Citydouble = 0;
                double Medidouble = 0;
                double Ficadouble = 0;
                string uname = "";
                string strerrorMessage = "";
                string strItems = payroll.PayrollData;
                if (strItems != string.Empty)
                {
                    JavaScriptSerializer sr = new JavaScriptSerializer();
                    List<Dictionary<object, object>> objEstimateItemData = new List<Dictionary<object, object>>();
                    objEstimateItemData = sr.Deserialize<List<Dictionary<object, object>>>(strItems);
                    int i = 0;
                    objEstimateItemData.RemoveAt(0);

                    // ExceptionLogging.SendMsgToText("Loop on all employee start : ");
                    foreach (Dictionary<object, object> dict in objEstimateItemData)
                    {
                        if (dict.ContainsKey("chkSelect"))
                        {
                            i++;
                            DataRow dr = dt.NewRow();
                            // ExceptionLogging.SendMsgToText("Data for Employee " + dict["hdnName"].ToString().Trim());
                            dr["ID"] = dict["hdnid"].ToString().Trim();
                            dr["Name"] = dict["hdnName"].ToString().Trim();
                            dr["Reg"] = double.Parse(dict["hdnlblReg"].ToString().Trim());
                            dr["OT"] = double.Parse(dict["hdnlblOT"].ToString().Trim());
                            dr["NT"] = double.Parse(dict["hdnlblNT"].ToString().Trim());
                            dr["DT"] = double.Parse(dict["hdnlblDT"].ToString().Trim());
                            dr["TT"] = double.Parse(dict["hdnlblTT"].ToString().Trim());

                            dr["Zone"] = ConvertCurrentCurrencyFormatToDbl(dict["hdnZone"].ToString().Trim());
                            dr["Milage"] = ConvertCurrentCurrencyFormatToDbl(dict["hdnMileage"].ToString());

                            //dr["Toll"] = ConvertCurrentCurrencyFormatToDbl(dict["hdnToll"].ToString());
                            dr["Toll"] = 0.00;
                            dr["OtherE"] = ConvertCurrentCurrencyFormatToDbl(dict["hdnOtherE"].ToString());
                            dr["pay"] = ConvertCurrentCurrencyFormatToDbl(dict["hdnpay"].ToString());

                            dr["holiday"] = ConvertCurrentCurrencyFormatToDbl(dict["hdnlblHoliday"].ToString().Trim());
                            dr["vacation"] = ConvertCurrentCurrencyFormatToDbl(dict["hdnlblVac"].ToString().Trim());
                            dr["sicktime"] = ConvertCurrentCurrencyFormatToDbl(dict["hdnsicktime"].ToString().Trim());
                            dr["reimb"] = ConvertCurrentCurrencyFormatToDbl(dict["hdnReimb"].ToString().Trim());
                            dr["bonus"] = ConvertCurrentCurrencyFormatToDbl(dict["hdnBonus"].ToString());

                            if (dict["hdnlblMethod"].ToString().Trim() != null)
                            {
                                dr["paymethod"] = dict["hdnlblMethod"].ToString();
                            }
                            else
                            {
                                dr["paymethod"] = "";
                            }
                            if (dict["hdnpmethod"].ToString().Trim() != null)
                            {
                                dr["pmethod"] = dict["hdnpmethod"].ToString();
                            }
                            else
                            {
                                dr["pmethod"] = "0";
                            }

                            dr["userid"] = dict["hdnuserid"].ToString();
                            dr["usertype"] = dict["hdnusertype"].ToString();
                            if (dict["hdntotal"].ToString().Trim() != null && dict["hdntotal"].ToString().Trim() != "")
                            {
                                dr["total"] = dict["hdntotal"].ToString();
                            }
                            else
                            {
                                dr["total"] = 0.00;
                            }
                            if (dict["hdnphour"].ToString().Trim() != null && dict["hdntotal"].ToString().Trim() != "")
                            {
                                dr["phour"] = dict["hdnphour"].ToString();
                            }
                            else
                            {
                                dr["phour"] = 0.00;
                            }
                            if (dict["hdnsalary"].ToString().Trim() != null && dict["hdntotal"].ToString().Trim() != "")
                            {
                                dr["salary"] = dict["hdnsalary"].ToString();
                            }
                            else
                            {
                                dr["salary"] = 0.00;
                            }
                            if (dict["hdnHourlyRate"].ToString().Trim() != null && dict["hdntotal"].ToString().Trim() != "")
                            {
                                dr["HourlyRate"] = dict["hdnHourlyRate"].ToString();
                            }
                            else
                            {
                                dr["HourlyRate"] = 0.00;
                            }
                            empgeocode = dict["hdnGeocode"].ToString().Trim();

                            string username = ConfigurationManager.AppSettings["vertexApiUsername"].ToString();
                            string passWord = ConfigurationManager.AppSettings["vertexApiPassword"].ToString();

                            // ExceptionLogging.SendMsgToText("Employee " + dict["hdnName"].ToString().Trim() + " Tax API Start : ");
                            string payrolluri = ConfigurationManager.AppSettings["vertexPayrollURL"].ToString();
                            //var chkdateString = Convert.ToDateTime(txtcheckdate.Text).ToString("yyyyMMdd");

                            double dedAmt401K = 0;
                            double dedAmt401KID = 0;
                            User objPropUser = new User();
                            DataSet ds = new DataSet();
                            objPropUser.ConnConfig = sessionConfig;
                            objPropUser.FStart = payroll.StartDate;
                            objPropUser.Edate = payroll.EndDate;
                            objPropUser.HolidayAm = ConvertCurrentCurrencyFormatToDbl(dict["hdnlblHoliday"].ToString().Trim());
                            objPropUser.VacAm = ConvertCurrentCurrencyFormatToDbl(dict["hdnlblVac"].ToString().Trim());
                            objPropUser.ZoneAm = ConvertCurrentCurrencyFormatToDbl(dict["hdnZone"].ToString().Trim());
                            objPropUser.ReimbAm = ConvertCurrentCurrencyFormatToDbl(dict["hdnReimb"].ToString().Trim());
                            objPropUser.MilageAm = ConvertCurrentCurrencyFormatToDbl(dict["hdnMileage"].ToString());
                            objPropUser.BonusAm = ConvertCurrentCurrencyFormatToDbl(dict["hdnBonus"].ToString());
                            int ProcessDed = 0;
                            if (payroll.ProcessOtherDeduction == true)
                            {
                                ProcessDed = Convert.ToInt32("1");
                            }
                            else
                            {
                                ProcessDed = Convert.ToInt32("0");
                            }
                            objPropUser.ID = Convert.ToInt32(dict["hdnid"].ToString().Trim());

                            ds = new BL_Wage().GetPayrollDeductions(objPropUser, ProcessDed);
                            DataTable dtDedTablePre = ds.Tables[0];

                            StringBuilder sbDeduct = new StringBuilder();

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
                                           "<ID>46</ID> " +
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

                            DataSet dsemp = new DataSet();
                            _objEmp.ConnConfig = sessionConfig;
                            _objEmp.ID = Convert.ToInt32(dict["hdnid"].ToString().Trim());

                            dsemp = objBL_Wage.GetEmployeeListByID(_objEmp);

                            DataRow _drEmpRow = dsemp.Tables[0].Rows[0];
                            if (Convert.ToInt32(_drEmpRow["FAllow"].ToString()) == -1)
                            {
                                _drEmpRow["FAllow"] = 0;
                            }
                            objPropUser.ID = Convert.ToInt32(dict["hdnid"].ToString().Trim());
                            DataSet dsHourWithLocation = GetPayrollHourWithLocation(objPropUser);
                            StringBuilder sbWork = new StringBuilder();
                            StringBuilder sbCityJUR = new StringBuilder();
                            int Filing_Stat = 1;
                            if (_drEmpRow["FStatus"].ToString() == "1") //Married
                            {
                                Filing_Stat = 2; //Married
                            }
                            else if (_drEmpRow["FStatus"].ToString() == "0") //Single
                            {
                                Filing_Stat = 1; //Single
                            }
                            List<string> lstStateGeoCode = new List<string>();
                            StringBuilder sbWorkInfo = new StringBuilder();
                            StringBuilder sbCMP = new StringBuilder();
                            StringBuilder sbAggCMP = new StringBuilder();
                            StringBuilder sbVprtCompensation = new StringBuilder();
                            List<string> lstGeo = new List<string>();
                            int cmpId = 0;

                            if (dsHourWithLocation.Tables[0].Rows.Count > 0)
                            {
                                lstStateGeoCode = (dsHourWithLocation.Tables[0].AsEnumerable().Select(x => x["sGeocode"].ToString()).Distinct()).ToList();
                                foreach (DataRow dRow in dsHourWithLocation.Tables[0].Rows)
                                {
                                    double Amount = Convert.ToDouble(dRow["Amount"]);
                                    if (Amount > 0)
                                    {
                                        if (dRow["lCity"].ToString() != "" && dRow["lState"].ToString() != "" && dRow["lZip"].ToString() != "")
                                        {
                                            sbWorkInfo.Clear();
                                            sbCMP.Clear();
                                            //sbAggCMP.Clear();
                                            string geocode = GetWorkGeo(dRow);

                                            if (geocode == null)
                                            {
                                                geocode = _drEmpRow["Geocode"].ToString();
                                            }

                                            lstGeo.Add(geocode);
                                            sbWorkInfo.Append("<WRKINFO>" +
                                                                   "<GEO>" + geocode + "</GEO>" +
                                                               "</WRKINFO>");
                                            if (Convert.ToBoolean(dRow["FIT"]) == false)
                                            {
                                                cmpId = 1100 + Convert.ToInt32(dRow["ID"]) + dsHourWithLocation.Tables[0].Rows.IndexOf(dRow);
                                                sbVprtCompensation.Append("<VPRT_COMPENSATION>" +
                                                                   "<TAXID>400</TAXID>" +
                                                                   "<GEO>0</GEO>" +
                                                                    "<TAX_TYPE>2</TAX_TYPE>" +
                                                                   "<ID>" + cmpId + "</ID>" +
                                                                   "<TYPE>r</TYPE>" +
                                                                   "<OVTYPE>3</OVTYPE>" +
                                                               "</VPRT_COMPENSATION>");
                                            }
                                            if (Convert.ToBoolean(dRow["FICA"]) == false)
                                            {
                                                cmpId = 1100 + Convert.ToInt32(dRow["ID"]) + dsHourWithLocation.Tables[0].Rows.IndexOf(dRow);
                                                sbVprtCompensation.Append("<VPRT_COMPENSATION>" +
                                                                  "<TAXID>403</TAXID>" +
                                                                  "<GEO>0</GEO>" +
                                                                   "<TAX_TYPE>2</TAX_TYPE>" +
                                                                  "<ID>" + cmpId + "</ID>" +
                                                                  "<TYPE>r</TYPE>" +
                                                                  "<OVTYPE>3</OVTYPE>" +
                                                              "</VPRT_COMPENSATION>");
                                            }
                                            if (Convert.ToBoolean(dRow["MEDI"]) == false)
                                            {
                                                cmpId = 1100 + Convert.ToInt32(dRow["ID"]) + dsHourWithLocation.Tables[0].Rows.IndexOf(dRow);
                                                sbVprtCompensation.Append("<VPRT_COMPENSATION>" +
                                                                 "<TAXID>406</TAXID>" +
                                                                 "<GEO>0</GEO>" +
                                                                  "<TAX_TYPE>2</TAX_TYPE>" +
                                                                 "<ID>" + cmpId + "</ID>" +
                                                                 "<TYPE>r</TYPE>" +
                                                                 "<OVTYPE>3</OVTYPE>" +
                                                             "</VPRT_COMPENSATION>");
                                            }
                                            if (Convert.ToBoolean(dRow["FUTA"]) == false)
                                            {
                                                cmpId = 1100 + Convert.ToInt32(dRow["ID"]) + dsHourWithLocation.Tables[0].Rows.IndexOf(dRow);
                                                sbVprtCompensation.Append("<VPRT_COMPENSATION>" +
                                                               "<TAXID>401</TAXID>" +
                                                               "<GEO>0</GEO>" +
                                                                "<TAX_TYPE>2</TAX_TYPE>" +
                                                               "<ID>" + cmpId + "</ID>" +
                                                               "<TYPE>r</TYPE>" +
                                                               "<OVTYPE>3</OVTYPE>" +
                                                           "</VPRT_COMPENSATION>");
                                            }
                                            if (Convert.ToBoolean(dRow["SIT"]) == false)
                                            {
                                                cmpId = 1100 + Convert.ToInt32(dRow["ID"]) + dsHourWithLocation.Tables[0].Rows.IndexOf(dRow);
                                                sbVprtCompensation.Append("<VPRT_COMPENSATION>" +
                                                              "<TAXID>450</TAXID>" +
                                                              "<GEO>" + dRow["sGeocode"].ToString() + "</GEO>" +
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
                                        else
                                        {
                                            sbWorkInfo.Clear();
                                            sbCMP.Clear();
                                            sbWorkInfo.Append("<WRKINFO>" +
                                                                "<GEO>" + _drEmpRow["Geocode"].ToString() + "</GEO>" +
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

                                if (dsHourWithLocation.Tables[1].Rows.Count > 0)
                                {
                                    foreach (DataRow dRow in dsHourWithLocation.Tables[1].Rows)
                                    {
                                        double Amount = Convert.ToDouble(dRow["Amount"]);

                                        if (Amount > 0)
                                        {
                                            sbWorkInfo.Clear();
                                            sbCMP.Clear();
                                            string geocode = _drEmpRow["Geocode"].ToString();
                                            sbWorkInfo.Append("<WRKINFO>" +
                                                                   "<GEO>" + geocode + "</GEO>" +
                                                               "</WRKINFO>");
                                            if (Convert.ToBoolean(dRow["FIT"]) == false)
                                            {
                                                cmpId = 1100 + Convert.ToInt32(dRow["ID"]) + dsHourWithLocation.Tables[1].Rows.IndexOf(dRow);
                                                sbVprtCompensation.Append("<VPRT_COMPENSATION>" +
                                                                   "<TAXID>400</TAXID>" +
                                                                   "<GEO>0</GEO>" +
                                                                    "<TAX_TYPE>2</TAX_TYPE>" +
                                                                   "<ID>" + cmpId + "</ID>" +
                                                                   "<TYPE>r</TYPE>" +
                                                                   "<OVTYPE>3</OVTYPE>" +
                                                               "</VPRT_COMPENSATION>");
                                            }
                                            if (Convert.ToBoolean(dRow["FICA"]) == false)
                                            {
                                                cmpId = 1100 + Convert.ToInt32(dRow["ID"]) + dsHourWithLocation.Tables[1].Rows.IndexOf(dRow);
                                                sbVprtCompensation.Append("<VPRT_COMPENSATION>" +
                                                                  "<TAXID>403</TAXID>" +
                                                                  "<GEO>0</GEO>" +
                                                                   "<TAX_TYPE>2</TAX_TYPE>" +
                                                                  "<ID>" + cmpId + "</ID>" +
                                                                  "<TYPE>r</TYPE>" +
                                                                  "<OVTYPE>3</OVTYPE>" +
                                                              "</VPRT_COMPENSATION>");
                                            }
                                            if (Convert.ToBoolean(dRow["MEDI"]) == false)
                                            {
                                                cmpId = 1100 + Convert.ToInt32(dRow["ID"]) + dsHourWithLocation.Tables[1].Rows.IndexOf(dRow);
                                                sbVprtCompensation.Append("<VPRT_COMPENSATION>" +
                                                                 "<TAXID>406</TAXID>" +
                                                                 "<GEO>0</GEO>" +
                                                                  "<TAX_TYPE>2</TAX_TYPE>" +
                                                                 "<ID>" + cmpId + "</ID>" +
                                                                 "<TYPE>r</TYPE>" +
                                                                 "<OVTYPE>3</OVTYPE>" +
                                                             "</VPRT_COMPENSATION>");
                                            }
                                            if (Convert.ToBoolean(dRow["FUTA"]) == false)
                                            {
                                                cmpId = 1100 + Convert.ToInt32(dRow["ID"]) + dsHourWithLocation.Tables[1].Rows.IndexOf(dRow);
                                                sbVprtCompensation.Append("<VPRT_COMPENSATION>" +
                                                               "<TAXID>401</TAXID>" +
                                                               "<GEO>0</GEO>" +
                                                                "<TAX_TYPE>2</TAX_TYPE>" +
                                                               "<ID>" + cmpId + "</ID>" +
                                                               "<TYPE>r</TYPE>" +
                                                               "<OVTYPE>3</OVTYPE>" +
                                                           "</VPRT_COMPENSATION>");
                                            }
                                            if (Convert.ToBoolean(dRow["SIT"]) == false)
                                            {
                                                cmpId = 1100 + Convert.ToInt32(dRow["ID"]) + dsHourWithLocation.Tables[1].Rows.IndexOf(dRow);
                                                sbVprtCompensation.Append("<VPRT_COMPENSATION>" +
                                                              "<TAXID>450</TAXID>" +
                                                              "<GEO>" + dRow["sGeocode"].ToString() + "</GEO>" +
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
                                                               "</CMP>");

                                            sbWork.Append("<WRK>" + sbWorkInfo + "<CMPARRAY>" + sbCMP + "</CMPARRAY>" + "</WRK>");
                                        }

                                    }
                                }
                            }
                            else
                            {
                                sbWork.Append("<WRK><WRKINFO>" +
                                    "<GEO>" + _drEmpRow["Geocode"].ToString() + "</GEO>" +
                                    "</WRKINFO>" +
                                    "<CMPARRAY>" +
                                        "<CMP>" +
                                            "<ID>0</ID>" +
                                            "<TYPE>r</TYPE>" +
                                            "<Amt>" + dict["hdntotal"].ToString() + "</Amt>" +
                                        "</CMP>" +
                                    "</CMPARRAY>" +
                                    "</WRK>");
                            }

                            StringBuilder stateJUR = new StringBuilder();
                            if (lstStateGeoCode != null)
                            {
                                foreach (string sGeo in lstStateGeoCode)
                                {
                                    if (!String.IsNullOrEmpty(sGeo))
                                    {
                                        stateJUR.Append(
                                                          "<JUR>"
                                                              + "<TAXID>450</TAXID>"
                                                              + "<GEO>" + sGeo + "</GEO>"
                                                               + "<FILING_STAT>" + Filing_Stat + "</FILING_STAT>"
                                                                   + "<PRI_EXEMPT>" + _drEmpRow["SAllow"].ToString() + "</PRI_EXEMPT>"
                                                              + "<CALCMETH>0</CALCMETH>"
                                                              + "<SUPL_METH>0</SUPL_METH>"
                                                          + "</JUR>");
                                    }
                                }
                            }
                            else
                            {
                                stateJUR.Append(
                                    "<JUR>"
                                        + "<TAXID>450</TAXID>"
                                        + "<GEO>" + _drEmpRow["VertexGeocode"].ToString() + "</GEO>"
                                         + "<FILING_STAT>" + Filing_Stat + "</FILING_STAT>"
                                             + "<PRI_EXEMPT>" + _drEmpRow["SAllow"].ToString() + "</PRI_EXEMPT>"
                                        + "<CALCMETH>0</CALCMETH>"
                                        + "<SUPL_METH>0</SUPL_METH>"
                                    + "</JUR>");
                            }

                            if (lstGeo != null)
                            {
                                foreach (string cGeo in lstGeo.Distinct())
                                {
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
                            string payDate = DateTime.Now.ToString("yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);

                            string payrollXML = "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:eic=\"http://EiCalc/\">"
                                + "<soapenv:Header/>"
                                + "<soapenv:Body>"
                                + "<eic:EiOperation>"
                                  + "<Request>"
                   + "<![CDATA["
            + "<EMP>"
               + "<EMPINFO>"
                   + "<EMPSTRUCT>"
                       + "<EMPID>ESS</EMPID>"
                       + "<PAYDATE>" + payDate + "</PAYDATE>";
                            if (dict["hdnPayPeriod"].ToString().Trim() == "0") //Weekly
                            {
                                payrollXML = payrollXML + "<PAYPERIODS>52</PAYPERIODS>";
                            }
                            else if (dict["hdnPayPeriod"].ToString().Trim() == "1") //Bi-Weekly
                            {
                                payrollXML = payrollXML + "<PAYPERIODS>26</PAYPERIODS>";
                            }
                            else
                            {
                                payrollXML = payrollXML + "<PAYPERIODS>1</PAYPERIODS>";
                            }
                            payrollXML = payrollXML + "<CURPERIOD>1</CURPERIOD>"
                   + "<RES_GEO>" + empgeocode + "</RES_GEO>"
                   + "<PRIMARY_WORK_GEO>" + empgeocode + "</PRIMARY_WORK_GEO>"
               + "</EMPSTRUCT>"

               + "<DEDUCTARRAY>"
                    + sbDeduct
                + "</DEDUCTARRAY>"

           + "</EMPINFO>"
           + sbWork
           + "<JURARRAY>"
            + "<JUR>"
                        + "<TAXID>400</TAXID>"
                        + "<GEO>0</GEO>"
                        + "<FILING_STAT>" + Filing_Stat + "</FILING_STAT>"
                         + "<PRI_EXEMPT>" + dict["hdnFAllow"].ToString().Trim() + "</PRI_EXEMPT>"
                    + "<CALCMETH>0</CALCMETH>"
                    + "<SUPL_METH>0</SUPL_METH>"
                + "</JUR>"
                 + stateJUR
                 + sbCityJUR
                   + "</JURARRAY>"

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
                                XmlNode Errornode = Docc.GetElementsByTagName("Error").Item(0);
                                if (Errornode == null)
                                {
                                    XmlNodeList elemList = Docc.GetElementsByTagName("TaxOut");
                                    for (int j = 0; j < elemList.Count; j++)
                                    {
                                        if (elemList[j]["TAXID"].InnerText == "400")
                                        {
                                            FITdouble = Convert.ToDouble(elemList[j]["TAX_AMT"].InnerText);
                                        }
                                        if (elemList[j]["TAXID"].InnerText == "450")
                                        {
                                            SITdouble = Convert.ToDouble(elemList[j]["TAX_AMT"].InnerText);
                                        }
                                        if (elemList[j]["TAXID"].InnerText == "530")
                                        {
                                            Citydouble = Convert.ToDouble(elemList[j]["TAX_AMT"].InnerText);
                                        }
                                        if (elemList[j]["TAXID"].InnerText == "406")
                                        {
                                            Medidouble = Convert.ToDouble(elemList[j]["TAX_AMT"].InnerText);
                                        }
                                        if (elemList[j]["TAXID"].InnerText == "403")
                                        {
                                            Ficadouble = Convert.ToDouble(elemList[j]["TAX_AMT"].InnerText);
                                        }

                                    }
                                }
                                else
                                {
                                    strerrorMessage = "Error in Address API for " + uname;
                                    break;
                                }
                                soapPayrollCalcTax.Dispose();
                                clpay.Dispose();
                            }
                            async Task<string> getMessage(HttpResponseMessage messageFromServer)
                            {
                                code = await messageFromServer.Content.ReadAsStringAsync();
                                return code;
                            }
                            //////////////////////////////////////////
                            dr["FIT"] = FITdouble;
                            dr["SIT"] = SITdouble;
                            dr["LOCAL"] = Localdouble;
                            dr["MEDI"] = Medidouble;
                            dr["FICA"] = Ficadouble;
                            dt.Rows.Add(dr);
                            //}
                            // ExceptionLogging.SendMsgToText("Data for Employee " + dict["hdnName"].ToString().Trim() + " End");
                        }
                    }
                }
                dt.AcceptChanges();
                if (dt.Rows.Count > 0)
                {
                    _objPRReg.Dt = dt;
                    _objPRReg.ConnConfig = sessionConfig;
                    _objPRReg.StartDate = payroll.StartDate;
                    _objPRReg.EndDate = payroll.EndDate;
                    _objPRReg.WeekNo = 21;
                    //_objPRReg.WeekNo = Convert.ToInt32(txtweek.Text);
                    _objPRReg.Description = payroll.Description;
                    _objPRReg.ProcessMethod = payroll.ProcessMethod;

                    if (payroll.ProcessOtherDeduction == true)
                    {
                        _objPRReg.PrcessDed = Convert.ToInt32("1");
                    }
                    else
                    {
                        _objPRReg.PrcessDed = Convert.ToInt32("0");
                    }
                    _objPRReg.SupervisorId = Convert.ToInt32(payroll.SupervisorId);
                    _objPRReg.DepartmentId = Convert.ToInt32(payroll.DeparptmentId);
                    _objPRReg.FrequencyId = Convert.ToInt32(payroll.FrequencyId);
                    _objPRReg.GrossPay = 0;
                    _objPRReg.TotalDeduction = 0;
                    _objPRReg.NetPay = 0;
                    _objPRReg.ID = Convert.ToInt32(HttpContext.Current.Session["PayrollRegisterId"]);

                    //DataSet _ReturnDS = new DataSet();
                    objBL_Wage.AddPayrollRagister(_objPRReg);
                    //ViewState["ReturnACHDetail"] = _ReturnDS;
                    if (HttpContext.Current.Session["PayrollRegisterId"] == null)
                    {
                        HttpContext.Current.Session["PayrollRegisterId"] = objBL_Wage.GetPayrollRegisterId(_objPRReg);
                    }
                    JSONString = "Payroll Calculation Updated";
                }
                return JsonConvert.SerializeObject(JSONString);
            }
            catch (Exception ex)
            {
                JSONString = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
                //string st = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
                //ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            }
            return JSONString;
        }

        [HttpPost]
        public string EditPayrollRagister(PayrollRegisterModel payroll)
        {
            string JSONString = string.Empty;
            try
            {
                var session = HttpContext.Current.Session;
                if (session != null)
                {
                    if (session["config"] != null)
                    {
                        sessionConfig = session["config"].ToString();
                    }
                }

                string strItems = payroll.PayrollData;
                if (strItems != string.Empty)
                {
                    JavaScriptSerializer sr = new JavaScriptSerializer();
                    List<Dictionary<object, object>> objEstimateItemData = new List<Dictionary<object, object>>();
                    objEstimateItemData = sr.Deserialize<List<Dictionary<object, object>>>(strItems);
                    List<int> lstEmployeeId = new List<int>();
                    objEstimateItemData.RemoveAt(0);
                    foreach (Dictionary<object, object> dict in objEstimateItemData)
                    {
                        if (!dict.ContainsKey("chkSelect"))
                        {
                            int Id = Convert.ToInt32(dict["hdnid"].ToString().Trim());
                            lstEmployeeId.Add(Id);
                        }
                    }
                    string Ids = string.Join<int>(",", lstEmployeeId);
                    objBL_Wage.EditPayrollRagister(sessionConfig, Ids);

                    JSONString = "Payroll Calculation Updated";
                }

                return JsonConvert.SerializeObject(JSONString);
            }
            catch (Exception ex)
            {
                JSONString = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
                //string st = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
                //ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            }
            return JSONString;
        }
        public DataSet GetPayrollHourWithLocation(User objPropUser)
        {
            //try
            //{
            //User objPropUser = new User();
            DataSet ds = new DataSet();
            objPropUser.ConnConfig = sessionConfig;
            ds = new BL_Wage().GetPayrollHourWithLocation(objPropUser);
            return ds;
            //}
            //catch (Exception ex)
            //{
            //    string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            //    return str;
            //    //ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            //}
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
            string uri = ConfigurationManager.AppSettings["vertexAddressURL"].ToString();
            //string uri = "https://payrollsandbox.ondemand.vertexinc.com:443/EiWebSvc/AddressWebService";

            DataSet dswork = new DataSet();
            var session = HttpContext.Current.Session;
            if (session != null)
            {
                if (session["config"] != null)
                {
                    objPropUser.ConnConfig = session["config"].ToString();
                }
            }
            //objPropUser.ConnConfig = Session["config"].ToString();
            dswork = objBL_User.getControl(objPropUser);
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
        private string GetWorkGeo(DataRow _dr)
        {
            string workgeo = "";
            string code = "";
            //////////////////////////////////////////
            //Credentials for Payroll OnDemand
            string username = ConfigurationManager.AppSettings["vertexApiUsername"].ToString(); // "dread@1000";
            string passWord = ConfigurationManager.AppSettings["vertexApiPassword"].ToString(); // "K3CHccxQ";
                                                                                                // Setting up the URI for Payroll
            string uri = ConfigurationManager.AppSettings["vertexAddressURL"].ToString();
            //string uri = "https://payrollsandbox.ondemand.vertexinc.com:443/EiWebSvc/AddressWebService";
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
        public DataTable GetTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ID", typeof(int));
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("Reg", typeof(double));
            dt.Columns.Add("OT", typeof(double));
            dt.Columns.Add("DT", typeof(double));
            dt.Columns.Add("TT", typeof(double));
            dt.Columns.Add("NT", typeof(double));
            dt.Columns.Add("Zone", typeof(double));
            dt.Columns.Add("Milage", typeof(double));
            dt.Columns.Add("Toll", typeof(double));
            dt.Columns.Add("OtherE", typeof(double));
            dt.Columns.Add("pay", typeof(double));
            dt.Columns.Add("holiday", typeof(double));
            dt.Columns.Add("vacation", typeof(double));
            dt.Columns.Add("sicktime", typeof(double));
            dt.Columns.Add("reimb", typeof(double));
            dt.Columns.Add("bonus", typeof(double));
            dt.Columns.Add("paymethod", typeof(string));
            dt.Columns.Add("pmethod", typeof(int));
            dt.Columns.Add("userid", typeof(string));
            dt.Columns.Add("usertype", typeof(string));
            dt.Columns.Add("total", typeof(double));
            dt.Columns.Add("phour", typeof(double));
            dt.Columns.Add("salary", typeof(double));
            dt.Columns.Add("HourlyRate", typeof(double));
            dt.Columns.Add("FIT", typeof(double));
            dt.Columns.Add("SIT", typeof(double));
            dt.Columns.Add("LOCAL", typeof(double));
            dt.Columns.Add("MEDI", typeof(double));
            dt.Columns.Add("FICA", typeof(double));
            return dt;
        }

        private double ConvertCurrentCurrencyFormatToDbl(string strCurrency)
        {
            if (!string.IsNullOrEmpty(strCurrency))
            {
                var dblReturn = double.Parse(strCurrency.Replace('$', '0'), NumberStyles.AllowParentheses |
                                                                            NumberStyles.AllowThousands |
                                                                            NumberStyles.AllowDecimalPoint |
                                                                            NumberStyles.AllowTrailingSign |
                                                                            NumberStyles.Float);
                return dblReturn;
            }
            else
            {
                return 0;
            }
        }

        //[HttpGet]
        //public string GetMonths()
        //{
        //    string JSONString = string.Empty;
        //    var itemList = new List<object>();
        //    for (int i = 1; i <= 12; i++)
        //    {
        //        ListItem item = new ListItem();
        //        item.Text = new DateTime(1900, i, 1).ToString("MMMM");
        //        item.Value = i.ToString();
        //        itemList.Add(item);
        //    }
        //    JSONString = JsonConvert.SerializeObject(itemList);
        //    return JSONString;
        //}

        //[HttpGet]
        //public string GetYrs()
        //{
        //    string JSONString = string.Empty;
        //    var itemList = new List<string>();
        //    int year = DateTime.Now.Year;
        //    itemList.Add((year--).ToString());
        //    itemList.Add((year).ToString());
        //    itemList.Add((year++).ToString());
        //    JSONString = JsonConvert.SerializeObject(itemList);
        //    return JSONString;
        //}

        //[HttpGet]
        //public string GetPayFrequency()
        //{
        //    string JSONString = string.Empty;
        //    var itemList = new List<object>();
        //    var PayFrequency = new List<string>() { "Weekly", "Bi-Weekly", "Monthly", "Quaterly", "Semi-Annual", "Annually" };
        //    int val = 1;
        //    foreach (string frequency in PayFrequency)
        //    {
        //        ListItem itm = new ListItem();
        //        itm.Text = frequency;
        //        itm.Value = val.ToString();
        //        itemList.Add(itm);
        //        val++;
        //    }
        //    JSONString = JsonConvert.SerializeObject(itemList);
        //    return JSONString;
        //}

        [HttpPost]
        public string GetVertexData(PayrollRegisterModel payroll)
        {
            string JSONString = string.Empty;
            try
            {
                DataSet ds = new DataSet();
                string Config = null;
                var session = HttpContext.Current.Session;
                if (session != null)
                {
                    if (session["config"] != null)
                    {
                        Config = session["config"].ToString();
                    }
                }
                ds = new BL_Wage().GetVertexData(Config, Convert.ToInt32(payroll.FrequencyId));
                JSONString = JsonConvert.SerializeObject(ds.Tables[0]);
            }
            catch (Exception ex)
            {
                JSONString = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            }
            return JSONString;
        }

    }
}