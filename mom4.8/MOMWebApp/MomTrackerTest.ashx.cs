using BusinessEntity;
using BusinessLayer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;

namespace MOMWebApp
{
    /// <summary>
    /// Summary description for MomTracker
    /// </summary>
    public class MomTrackerTest : IHttpHandler
    {

        BL_MapData objBL_MapData = new BL_MapData();
        MapData objMData = new MapData();

        General objgeneral = new General();
        BL_General objBL_General = new BL_General();


        public HttpContext context;
        public HttpRequest request;
        public HttpResponse response;
        public bool sc;
        public string msg, Msgs, dtl, RtnRlt, strJson, MemID = "", LoginPwd = "", TXNPwd = "", txtsdescription = "", Text, SpID, MngrDtl, UserID, Created_On
            , txtuploadby, txtdescription, txtblogimage, Country_input, Region_input, invite_input,
            parent_email, parent_phone, how_find_us, captcha_input_field, password_input, CountryCode = "", txtcustomerphone = "", txtcustomername = "", txtcustomeremail = "", varificationCode = "", txtcompanyemail = "", txtsubtotal = "", txttotal = "", txtvattotal = "",
            txtcustid = "", txtcustname = "", txttableid = "", txtwaiterid = "", txtwaitername = "", txttransno = "", txtheading = "", txtDoj = "", txtDob = "", txtGender = "",
            txtnicknm = "", UseNickName = "", select_Education = "", txtpersonality = "", txtpersonalitysize = "", txtpersonalityHeight = "", HideProfile = "",
            fname, chkfileEx, SDbFilePath = "", AcFileName, FileNameDB = "", txtdatabase="", txtbattery="", txtspeed="";

        //DBConnectHP objgdb = new DBConnectHP();
        DataSet ds;
        public class test
        {
            public bool Success { get; set; }
            public string timestamp { get; set; }
            public string GPSinterval { get; set; }
            //public string aid { get; set; }
            public test(bool sc, string msg, string dtl)
            {
                Success = sc;
                timestamp = msg;
                GPSinterval = dtl;
            }
            //public test(string msg)
            //{

            //    Message = msg;

            //}
        }

        public void handleRequest()
        {
            writeJson(new test(sc, msg, dtl));


        }
        public void writeJson(object _object)
        {
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();


            string jsondata = "";
            jsondata += "{";
            jsondata += "\"";
            jsondata += "records";
            jsondata += "\"";
            jsondata += ":";

            jsondata += javaScriptSerializer.Serialize(_object);

            jsondata += "}";

            writeRaw(jsondata);
        }

        public void writeRaw(string text)
        {
            context.Response.Write(text);
        }
        public void ProcessRequest(HttpContext context)
        {
        //context.Response.ContentType = "text/plain";
        //context.Response.Write("Hello World");
        //////// FORMAT OF INPUT PASS IN RAW BODY //////
        //[{"id": "596", "description": "","qty": 1,"price":50.00,"amount":50.00,"vatrate":10,"vatid":1221,"vatamount":5,"companyemail":"raghu@gmail.com","subtotal":250.00,"vat":25.00,"total":275.00,"custid":118,"custname":"First Cust","tableid":5,"waiterid":186,"waitername":"azhar"},{"id": "597", "description": "","qty": 1,"price":200.00,"amount":200.00,"vatrate":10,"vatid":1221,"vatamount":20,"companyemail":"raghu@gmail.com","subtotal":250.00,"vat":25.00,"total":275.00,"custid":118,"custname":"First Cust","tableid":5,"waiterid":186,"waitername":"azhar"}]
        //
            

            var jsonSerializer = new JavaScriptSerializer();
            var jsonString = String.Empty;

            context.Request.InputStream.Position = 0;
            using (var inputStream = new StreamReader(context.Request.InputStream))
            {
                jsonString = inputStream.ReadToEnd();
            }

            var itmlList = jsonSerializer.Deserialize<List<Items>>(jsonString);
            
            foreach (var itm in itmlList)
            {
                CrateLogFile(itm.fUser.ToString(), jsonString);
            }

            foreach (var itm in itmlList)
            {
                txtcompanyemail = itm.deviceId;
                txtsubtotal = itm.latitude;
                txttotal = itm.longitude;
                txtvattotal = itm.date.ToString();

                txtcustid = itm.fake.ToString();
                if (itm.accuracy == null)
                {
                    txtcustname = "0";
                    
                }
                else {
                    txtcustname = itm.accuracy.ToString();

                }
                txttableid = itm.fUser.ToString();
                txtwaiterid = itm.userId.ToString();
                txtdatabase = itm.database.ToString();
                txtbattery = itm.battery.ToString();
                txtspeed = itm.speed.ToString();

            }

            GiveHelpOrder();

            context.Response.ContentType = "application/json";
            context.Response.ContentEncoding = Encoding.UTF8;
            context.Response.Write(jsonSerializer.Serialize(new test(sc, msg, dtl)));

            //var resp = String.Empty;

            //foreach (var emp in emplList)
            //{
            //    resp += emp.id + " \\ ";
            //    resp += emp.name + " \\ ";
            //}

            //context.Response.ContentType = "application/json";
            //context.Response.ContentEncoding = Encoding.UTF8;
            //context.Response.Write(jsonSerializer.Serialize(resp));

            //dictionary2.Add("Success", "1");
            //dictionary2.Add("timestamp", System.DateTime.Now.ToString());
            //dictionary2.Add("GPSinterval", strGPSping);
            //dictionary.Add(dictionary2);
        }
        public void GiveHelpOrder()
        {

            string connstr = "";
            string strGPSping = string.Empty;
            try
            {
                DataTable dtData = new DataTable();
                dtData.Columns.Add("deviceId", typeof(string));
                dtData.Columns.Add("latitude", typeof(string));
                dtData.Columns.Add("longitude", typeof(string));
                dtData.Columns.Add("date", typeof(DateTime));
                dtData.Columns.Add("fake", typeof(int));
                dtData.Columns.Add("accuracy", typeof(string));
                dtData.Columns.Add("fUser", typeof(string));
                dtData.Columns.Add("userId", typeof(string));
                dtData.Columns.Add("battery", typeof(string));
                dtData.Columns.Add("speed", typeof(string));
                //dtData.Columns.Add("database", typeof(string));
                strGPSping = objBL_General.GetGPSInterval(objgeneral);

                //dictionary1 = geoData[0];

                //deviceId = txtcompanyemail;
                //date = txtvattotal;
                //fake = txtcustid;
                DataRow row;
                row = dtData.NewRow();
                row["deviceId"] = txtcompanyemail;
                row["latitude"] = txtsubtotal;
                row["longitude"] = txttotal;

                //row["date"] = Convert.ToDateTime(date.Substring(0,10).Replace(".", string.Empty));
                row["date"] = Convert.ToDateTime(txtvattotal.Replace("T", " ").Replace("Z", " "));
                int fake = 0;                
                string accuracy = string.Empty;
                if ((txtcustid != null) && (txtcustid != ""))
                {
                    row["fake"] = txtcustid.ToString();
                    fake = Convert.ToInt32(txtcustid.ToString());
                }
                else
                {
                    row["fake"] = "0";                    
                }

                if ((txtcustname != null) && (txtcustname != ""))
                {
                    row["accuracy"] = txtcustname;
                    accuracy = txtcustname;
                }
                else
                {
                    row["accuracy"] = "0";
                    accuracy = "0";
                }
                row["fUser"] = txttableid;
                row["userId"] = txtwaiterid;
                row["battery"] = txtbattery;
                row["speed"] = txtspeed;

                dtData.Rows.Add(row);


                objMData.Database = txtdatabase;
                objMData.ConnConfig = connstr;
                objMData.LocData = new DataTable();
                objMData.LocData = dtData;
                objBL_MapData.InsertMapDataNew(objMData);
                
                
                //objBL_MapData.InsertMapDataNew(objMData);
                // objBL_MapData.AddMapDataTest(objMData);

                //dictionary2.Add("Success", "1");
                //dictionary2.Add("timestamp", System.DateTime.Now.ToString());
                //dictionary2.Add("GPSinterval", strGPSping);
                //dictionary.Add(dictionary2);

                //str = sr.Serialize(dictionary);

                sc = true;
                msg = System.DateTime.Now.ToString();
                dtl = strGPSping;

            }
            catch (Exception ex)
            {
                //DB.WriteLog(this.ToString() + " Error Msg :" + ex.Message + "\n" + "Event Info :" + ex.StackTrace);
                sc = false;
                //msg = "<span style='color:#FF0000; font-wight:bold; font-size:13px;'> " +ex.Message+ "</span><br/>";
                msg = ex.Message;
            }
            finally
            {
                if (ds != null) { ds.Dispose(); }
            }
        }


        public void CrateLogFile(string user ,string content)
        {
            string filePath = ConfigurationManager.AppSettings["MomTrackerLogPath"].Trim();
            //string filename = DateTime.Now.ToString("yyyy-MM-dd_HH_mm_ss") + "_" + user + "_MomTrackerAPI.txt";
            string filename = user + "_MomTrackerAPI.txt";
            if (!Directory.Exists(filePath))
                Directory.CreateDirectory(filePath);
            StringBuilder sb = new StringBuilder();
            sb.Append(content);
            bool fileExist = File.Exists(filePath + "\\" + filename);
            if (!fileExist)
            {
                File.AppendAllText(filePath + filename, sb.ToString());
                sb.Clear();
            }
            else
            {
                File.AppendAllText(filePath + filename, sb.ToString());
                sb.Clear();
            }

        }

        public class Items
        {

            public string deviceId { get; set; }
            public string latitude { get; set; }
            public string longitude { get; set; }
            public DateTime date { get; set; }
            public int fake { get; set; }
            public string accuracy { get; set; }
            public string fUser { get; set; }
            public string userId { get; set; }
            public string database { get; set; }
            public string battery { get; set; }
            public string speed { get; set; }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}