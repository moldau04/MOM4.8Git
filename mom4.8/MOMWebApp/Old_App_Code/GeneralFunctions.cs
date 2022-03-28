using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.IO;
using System.Net;
using System.Web.Script.Serialization;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Security.Cryptography;
using System.Text;
using Microsoft.ApplicationBlocks.Data;
using OpenPop.Mime.Header;
using OpenPop.Pop3;
using OpenPop.Pop3.Exceptions;
using Message = OpenPop.Mime.Message;

using BusinessEntity;
using BusinessLayer;
using ImapX;
using System.Globalization;
using Telerik.Web.UI;

/// <summary>
/// Summary description for GeneralFunctions
/// </summary>
public class GeneralFunctions
{
    public GeneralFunctions()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    /// <summary>
    /// Encodes a string to be represented as a string literal. The format
    /// is essentially a JSON string.
    /// 
    /// The string returned includes outer quotes 
    /// Example Output: "Hello \"Rick\"!\r\nRock on"
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public string EncodeJsString(string s)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("\"");
        foreach (char c in s)
        {
            switch (c)
            {
                case '\"':
                    sb.Append("\\\"");
                    break;
                case '\\':
                    sb.Append("\\\\");
                    break;
                case '\b':
                    sb.Append("\\b");
                    break;
                case '\f':
                    sb.Append("\\f");
                    break;
                case '\n':
                    sb.Append("\\n");
                    break;
                case '\r':
                    sb.Append("\\r");
                    break;
                case '\t':
                    sb.Append("\\t");
                    break;
                default:
                    int i = (int)c;
                    if (i < 32 || i > 127)
                    {
                        sb.AppendFormat("\\u{0:X04}", i);
                    }
                    else
                    {
                        sb.Append(c);
                    }
                    break;
            }
        }
        sb.Append("\"");

        return sb.ToString();
    }

    /// <summary>
    /// Converts datatable into a serialised json string format.
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    public string GetJson(DataSet ds)
    {
        System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();

        Dictionary<string, object> table = new Dictionary<string, object>();
        foreach (DataTable dt in ds.Tables)
        {
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            foreach (DataRow dr in dt.Rows)
            {
                Dictionary<string, object> row = new Dictionary<string, object>();
                foreach (DataColumn col in dt.Columns)
                {
                    row.Add(col.ColumnName, dr[col]);
                }
                rows.Add(row);
            }
            table.Add(dt.TableName, rows);
        }
        return serializer.Serialize(table);
    }

    public List<Dictionary<object, object>> RowsToDictionary(DataTable table)
    {
        List<Dictionary<object, object>> objs = new List<Dictionary<object, object>>();

        foreach (DataRow dr in table.Rows)
        {
            Dictionary<object, object> drow = new Dictionary<object, object>();
            for (int i = 0; i < table.Columns.Count; i++)
            {
                drow.Add(table.Columns[i].ColumnName, dr[i]);
            }
            objs.Add(drow);
        }

        return objs;
    }

    public Dictionary<string, object> ToJson(DataTable table)
    {
        System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();

        Dictionary<string, object> d = new Dictionary<string, object>();
        d.Add(table.TableName, RowsToDictionary(table));
        //return serializer.Serialize(d);
        return d;
    }

    public Dictionary<string, object> ToJson(DataSet data)
    {
        System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();

        Dictionary<string, object> d = new Dictionary<string, object>();
        foreach (DataTable table in data.Tables)
        {
            d.Add(table.TableName, RowsToDictionary(table));
        }
        //return serializer.Serialize(d);
        return d;
    }

    public string[] StringArray(DataSet ds)
    {
        string[] stringArray = new string[ds.Tables[0].Columns.Count];

        for (int col = 0; col < ds.Tables[0].Columns.Count; ++col)
        {
            stringArray[col] = ds.Tables[0].Rows[0][col].ToString();
        }

        return stringArray;
    }



    /// <summary>
    /// Function to save byte array to a file
    /// </summary>
    /// <param name="_FileName">File name to save byte array</param>
    /// <param name="_ByteArray">Byte array to save to external file</param>
    /// <returns>Return true if byte array save successfully, if not return false</returns>
    public bool ByteArrayToFile(string _FileName, byte[] _ByteArray)
    {
        try
        {
            // Open file for reading
            System.IO.FileStream _FileStream = new System.IO.FileStream("ImageData/" + _FileName, System.IO.FileMode.Create, System.IO.FileAccess.Write);

            // Writes a block of bytes to this stream using data from a byte array.
            _FileStream.Write(_ByteArray, 0, _ByteArray.Length);

            // close file stream
            _FileStream.Close();

            return true;
        }
        catch (Exception _Exception)
        {
            // Error
            Console.WriteLine("Exception caught in process: {0}", _Exception.ToString());
        }

        // error occured, return false
        return false;
    }

    public string Truncate(string Value, int length)
    {
        if (Value.Length > length)
        {
            Value = Value.Substring(0, length);
            Value = Value.Replace("'", "`");
        }
        return Value;
    }
    public string QBEncode(string Value, int length)
    {
        Value = RemoveDiacritics(Value);
        return Truncate(Value, length);
    }
    public string RemoveDiacritics(string text)
    {
        var normalizedString = text.Normalize(NormalizationForm.FormD);
        var stringBuilder = new StringBuilder();
        foreach (var c in normalizedString)
        {
            var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
            if (unicodeCategory != UnicodeCategory.NonSpacingMark)
            { stringBuilder.Append(c); }
        }
        return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
    }
    public string TruncateWithText(string Value, int length)
    {
        if (Value.Length > length)
        {
            Value = Value.Substring(0, length) + "...";
        }
        return Value;
    }

    public String generateRandomString(int length)
    {
        //Initiate objects & vars    
        Random random = new Random();
        String randomString = "";
        int randNumber;

        //Loop ‘length’ times to generate a random number or character
        for (int i = 0; i < length; i++)
        {
            if (random.Next(1, 3) == 1)
                randNumber = random.Next(97, 123); //char {a-z}
            else
                randNumber = random.Next(48, 58); //int {0-9}

            //append random char or digit to random string
            randomString = randomString + (char)randNumber;
        }
        //return the random string
        return randomString;
    }

    public double GetDistance(string latitude1, string longitude1, string latitude2, string longitude2)
    {
        double lat1 = Convert.ToDouble(latitude1);
        double lng1 = Convert.ToDouble(longitude1);
        double lat2 = Convert.ToDouble(latitude2);
        double lng2 = Convert.ToDouble(longitude2);

        double e = (3.1415926538 * lat1 / 180);
        double f = (3.1415926538 * lng1 / 180);
        double g = (3.1415926538 * lat2 / 180);
        double h = (3.1415926538 * lng2 / 180);
        double i = (Math.Cos(e) * Math.Cos(g) * Math.Cos(f) * Math.Cos(h) + Math.Cos(e) * Math.Sin(f) * Math.Cos(g) * Math.Sin(h) + Math.Sin(e) * Math.Sin(g));
        double j = (Math.Acos(i));
        double k = (6371 * j);
        return k;
    }

    public GeoJsonData GeoRequest(string lat, string longi , string GoogleMAPAPIKey)
    {
        WebRequest request = WebRequest.Create("https://maps.googleapis.com/maps/api/geocode/json?latlng=" + lat + "," + longi + "&sensor=false&key="+GoogleMAPAPIKey);
        request.Method = "GET";
        var response = request.GetResponse();
        string result;
        using (var stream = response.GetResponseStream())
        {
            using (var reader = new StreamReader(stream))
            {
                result = reader.ReadToEnd();
            }
        }
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        return serializer.Deserialize<GeoJsonData>(result);
    }

    public GeoJsonData GeoRequest(string address, string GoogleMAPAPIKey)
    {
        WebRequest request = WebRequest.Create("http://maps.googleapis.com/maps/api/geocode/json?address=" + address + "&sensor=false&key="+ GoogleMAPAPIKey);
        request.Method = "GET";
        var response = request.GetResponse();
        string result;
        using (var stream = response.GetResponseStream())
        {
            using (var reader = new StreamReader(stream))
            {
                result = reader.ReadToEnd();
            }
        }
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        return serializer.Deserialize<GeoJsonData>(result);
    }

    public double bearing(double Alat1, double Alon1, double Alat2, double Alon2)
    {
        double lat1 = ConvertToRadians(Alat1);
        double lon1 = ConvertToRadians(Alon1);
        double lat2 = ConvertToRadians(Alat2);
        double lon2 = ConvertToRadians(Alon2);

        double degreesPerRadian = 180.0 / Math.PI;

        // Compute the angle.
        double angle = -Math.Atan2(Math.Sin(lon1 - lon2) * Math.Cos(lat2), Math.Cos(lat1) * Math.Sin(lat2) - Math.Sin(lat1) * Math.Cos(lat2) * Math.Cos(lon1 - lon2));

        if (angle < 0.0)
        {
            angle += Math.PI * 2.0;
        }

        // And convert result to degrees.
        angle = angle * degreesPerRadian;
        //angle = angle.toFixed(1);

        return angle;
    }

    public double ConvertToRadians(double angle)
    {
        return (Math.PI / 180) * angle;
    }

    public string IsNull(string input, string replacement)
    {
        string output = string.Empty;

        if (!string.IsNullOrEmpty(input.Trim()))
        {
            output = input.Trim();
        }
        else
        {
            output = replacement.Trim();
        }

        return output;
    }

    public byte[] ResizeImage(System.Drawing.Image stImage, int Width, int Height)
    {
        byte[] bmpBytes = null;
        if (stImage != null)
        {
            // Create a bitmap of the content of the fileUpload control in memory
            Bitmap originalBMP = new Bitmap(stImage);
            double sngRatioraw = 0;
            int sngRatio = 0;
            int newWidth = 0;
            int newHeight = 0;
            // Calculate the new image dimensions
            int origWidth = originalBMP.Width;
            int origHeight = originalBMP.Height;
            if (origWidth > origHeight)
            {
                sngRatioraw = Convert.ToDouble(origWidth) / Convert.ToDouble(origHeight);
                newWidth = Width;
                sngRatio = Convert.ToInt32(Math.Round(sngRatioraw));
                newHeight = newWidth / sngRatio;
            }
            else
            {
                sngRatioraw = Convert.ToDouble(origHeight) / Convert.ToDouble(origWidth);
                newHeight = Height;
                sngRatio = Convert.ToInt32(Math.Round(sngRatioraw));
                newWidth = newHeight / sngRatio;
            }

            // Create a new bitmap which will hold the previous resized bitmap
            Bitmap newBMP = new Bitmap(originalBMP, newWidth, newHeight);

            // Create a graphic based on the new bitmap
            Graphics oGraphics = Graphics.FromImage(newBMP);
            // Set the properties for the new graphic file
            oGraphics.SmoothingMode = SmoothingMode.AntiAlias; oGraphics.InterpolationMode = InterpolationMode.HighQualityBicubic;

            // Draw the new graphic based on the resized bitmap
            oGraphics.DrawImage(originalBMP, 0, 0, newWidth, newHeight);



            bmpBytes = BmpToBytes_MemStream(newBMP);

            // Once finished with the bitmap objects, we deallocate them.
            originalBMP.Dispose();
            newBMP.Dispose();
            oGraphics.Dispose();
        }
        return bmpBytes;
    }

    private byte[] BmpToBytes_MemStream(Bitmap bmp)
    {
        MemoryStream ms = new MemoryStream();
        // Save to memory using the Jpeg format
        bmp.Save(ms, ImageFormat.Png);

        // read to end
        byte[] bmpBytes = ms.GetBuffer();
        bmp.Dispose();
        ms.Close();

        return bmpBytes;
    }

    public void ResetFormControlValues(Control parent)
    {
        foreach (Control c in parent.Controls)
        {
            if (c.Controls.Count > 0)
            {
                ResetFormControlValues(c);
            }
            else
            {
                switch (c.GetType().ToString())
                {
                    case "System.Web.UI.WebControls.DropDownList":
                        ((DropDownList)c).SelectedIndex = -1;
                        break;
                    case "System.Web.UI.WebControls.TextBox":
                        ((TextBox)c).Text = "";
                        break;
                    case "System.Web.UI.WebControls.CheckBox":
                        ((CheckBox)c).Checked = false;
                        break;
                    case "System.Web.UI.WebControls.RadioButton":
                        ((RadioButton)c).Checked = false;
                        break;
                    case "System.Web.UI.WebControls.HiddenField":
                        ((HiddenField)c).Value = "";
                        break;
                }
            }
        }
    }

    public string RNGCharacterMask(int maxSize)
    {
        //int maxSize = 8;
        int minSize = 5;
        char[] chars = new char[62];
        string a;
        a = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
        chars = a.ToCharArray();
        int size = maxSize;
        byte[] data = new byte[1];
        RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider();
        crypto.GetNonZeroBytes(data);
        size = maxSize;
        data = new byte[size];
        crypto.GetNonZeroBytes(data);
        StringBuilder result = new StringBuilder(size);
        foreach (byte b in data)
        { result.Append(chars[b % (chars.Length - 1)]); }
        return result.ToString();
    }

    public string ConnectionStr(string dbname)
    {
        string server = Config.MS.Split(';')[0].Split('=')[1];
        string database = dbname;
        string user = Config.MS.Split(';')[2].Split('=')[1];
        string pass = Config.MS.Split(';')[3].Split('=')[1];

        string constr = "server=" + server + ";database=" + database + ";user=" + user + ";password=" + pass + "";
        return constr;
    }

    public string[] toStringArray(List<RfcMailAddress> lst)
    {
        RfcMailAddress[] strRFC = lst.ToArray();
        string[] str = new string[lst.Count];
        int i = 0;
        foreach (RfcMailAddress rfc in lst)
        {
            str[i] = Convert.ToString(rfc.Address);
            i++;
        }
        return str;
    }

    public string[] toStringArray(List<MailAddress> lst)
    {
        MailAddress[] strRFC = lst.ToArray();
        string[] str = new string[lst.Count];
        int i = 0;
        foreach (MailAddress rfc in lst)
        {
            str[i] = Convert.ToString(rfc.Address);
            i++;
        }
        return str;
    }

    public void DownloadMails(string host, string user, string pass, string port, int userid, string connconfig)
    {
        BL_General objBL_General = new BL_General();
        General objGeneral = new General();

        Pop3Client pop3Client = new Pop3Client();

        try
        {
            if (pop3Client.Connected)
                pop3Client.Disconnect();

            pop3Client.Connect(host.Trim(), int.Parse(port.Trim()), true);
            pop3Client.Authenticate(user.Trim(), pass.Trim());

            int count = pop3Client.GetMessageCount();
            List<string> uids = pop3Client.GetMessageUids();

            objGeneral.ConnConfig = connconfig;
            objGeneral.AccountID = user.Trim();
            DataSet ds = objBL_General.GetMsgUID(objGeneral);
            List<string> seenUids = ds.Tables[0].AsEnumerable()
                                   .Select(r => r.Field<string>("UID"))
                                   .ToList();

            for (int i = 0; i < uids.Count; i++)
            {
                string currentUidOnServer = uids[i];
                if (!seenUids.Contains(currentUidOnServer))
                {
                    try
                    {
                        Message unseenMessage = pop3Client.GetMessage(i + 1);

                        var AID = System.Guid.NewGuid();
                        objGeneral.From = Convert.ToString(unseenMessage.Headers.From.Address);
                        objGeneral.to = Convert.ToString(string.Join(",", toStringArray(unseenMessage.Headers.To)));
                        objGeneral.cc = Convert.ToString(string.Join(",", toStringArray(unseenMessage.Headers.Cc)));
                        objGeneral.bcc = Convert.ToString(string.Join(",", toStringArray(unseenMessage.Headers.Bcc)));
                        objGeneral.subject = Convert.ToString(unseenMessage.Headers.Subject);
                        objGeneral.sentdate = unseenMessage.Headers.DateSent;
                        //objGeneral.date = Convert.ToString(unseenMessage.Headers.Date);
                        objGeneral.Attachments = unseenMessage.FindAllAttachments().Count();
                        objGeneral.msgid = Convert.ToString(unseenMessage.Headers.MessageId);
                        objGeneral.uid = Convert.ToInt32(currentUidOnServer);
                        objGeneral.GUID = AID;
                        objGeneral.type = 0;
                        objGeneral.userid = userid;
                        objGeneral.AccountID = user.Trim();
                        objGeneral.ConnConfig = connconfig;
                        int success = objBL_General.AddEmails(objGeneral);

                        if (success == 1)
                        {
                            string savepathconfig = System.Web.Configuration.WebConfigurationManager.AppSettings["AttachmentPath"].Trim();
                            string savepath = savepathconfig + @"\mails\";
                            if (!Directory.Exists(savepath))
                            {
                                Directory.CreateDirectory(savepath);
                            }
                            string filename = AID.ToString() + ".eml";
                            FileInfo file = new FileInfo(savepath + filename);
                            unseenMessage.Save(file);
                        }
                    }
                    catch (Exception ex)
                    {
                        //string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
                        //ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, theme : 'noty_theme_default',  closable : true});", true);
                        throw ex;
                    }
                }
            }
        }
        catch (InvalidLoginException)
        {
            //ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccUp", "The server did not accept the user credentials!", true);
            throw new Exception("The server did not accept the user credentials!");
        }
        catch (PopServerNotFoundException)
        {
            //ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccUp", "The server could not be found", true);
            throw new Exception("The server could not be found");
        }
        catch (PopServerLockedException)
        {
            //ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccUp", "The mailbox is locked. It might be in use or under maintenance. Are you connected elsewhere?", true);
            throw new Exception("The mailbox is locked. It might be in use or under maintenance. Are you connected elsewhere?");
        }
        catch (LoginDelayException)
        {
            //ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccUp", "Login not allowed. Server enforces delay between logins. Have you connected recently?", true);
            throw new Exception("Login not allowed. Server enforces delay between logins. Have you connected recently?");
        }
        catch (Exception ex)
        {
            throw ex;
            //string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            //ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    public void DownloadMailsIMAP(string hostname, string username, string password, string port, int userid, string connconfig, int MAXUID, DataSet CRMEmails)
    {
        //BL_General objBL_General = new BL_General();
        //General objGeneral = new General();
        try
        {
            using (ImapClient client = new ImapClient())
            {
                if (client.Connect())
                    client.Disconnect();

                if (client.Connect(hostname, Convert.ToInt32(port), true, false))
                {
                    if (client.Login(username, password))
                    {
                        DateTime dateSince = DateTime.Now;
                        //if (lastfetch.Trim() != string.Empty)
                        //    dateSince = Convert.ToDateTime(lastfetch.Trim());

                        dateSince = dateSince.AddDays(-1);
                        string strDate = String.Format("{0:d-MMM-yyyy}", dateSince);
                        int r = 0;
                        foreach (DataRow dr in CRMEmails.Tables[0].Rows)
                        {
                            string strEmail = dr["email"].ToString().Trim();
                            string searchQuery = string.Empty;
                            if (MAXUID != 0)
                                //searchQuery = "UID " + (MAXUID + 1).ToString() + ":*";
                                searchQuery = "UID " + (MAXUID + 1).ToString() + ":* OR (OR (FROM " + strEmail + ") (TO " + strEmail + ")) (CC " + strEmail + ")";
                            else
                                searchQuery = "SINCE " + strDate + " OR (OR (FROM " + strEmail + ") (TO " + strEmail + ")) (CC " + strEmail + ")";

                            //searchQuery = "FROM \"" + strEmail+"\"";
                            //if (r == 0)
                            //{
                            //    IEnumerable<ImapX.Message> messages = client.Folders.Inbox.Search(searchQuery, ImapX.Enums.MessageFetchMode.Full, Int32.MaxValue);
                            //}
                            //    r = 1;

                            IEnumerable<ImapX.Message> messages = client.Folders.Inbox.Search(searchQuery, ImapX.Enums.MessageFetchMode.Full, Int32.MaxValue);
                            savemail(messages, userid, username, connconfig);

                            //IEnumerable<ImapX.Message> messagesSent = client.Folders.Sent.Search(searchQuery, ImapX.Enums.MessageFetchMode.Full, Int32.MaxValue);
                            //savemail(messagesSent, userid, username, connconfig);
                        }
                    }
                }
                client.Disconnect();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }

    private void savemail(IEnumerable<ImapX.Message> messages, int userid, string username, string connconfig)
    {
        BL_General objBL_General = new BL_General();
        General objGeneral = new General();

        foreach (ImapX.Message msg in messages)
        {
            var AID = System.Guid.NewGuid();
            objGeneral.From = Convert.ToString(msg.From.Address);
            objGeneral.to = Convert.ToString(string.Join(",", toStringArray(msg.To)));
            objGeneral.cc = Convert.ToString(string.Join(",", toStringArray(msg.Cc)));
            objGeneral.bcc = Convert.ToString(string.Join(",", toStringArray(msg.Bcc)));
            objGeneral.subject = Convert.ToString(msg.Subject);
            objGeneral.sentdate = msg.Date;
            objGeneral.Attachments = msg.Attachments.Count();
            objGeneral.msgid = Convert.ToString(msg.MessageId);
            objGeneral.uid = msg.UId;
            objGeneral.GUID = AID;
            objGeneral.type = 0;
            objGeneral.userid = userid;
            objGeneral.AccountID = username.Trim();
            objGeneral.ConnConfig = connconfig;
            int success = objBL_General.AddEmails(objGeneral);

            if (success == 1)
            {
                string savepathconfig = System.Web.Configuration.WebConfigurationManager.AppSettings["AttachmentPath"].Trim();
                string savepath = savepathconfig + @"\mails\";
                if (!Directory.Exists(savepath))
                {
                    Directory.CreateDirectory(savepath);
                }
                string filename = AID.ToString() + ".eml";
                string str = msg.DownloadRawMessage();
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(savepath + filename, true))
                {
                    file.WriteLine(str);
                }
            }
        }
    }

    public Int32 GetSalesAsigned()
    {
        Int32 IsSalesAsigned = 0;
        if (HttpContext.Current.Session["type"] != null && HttpContext.Current.Session["MSM"] != null
            && HttpContext.Current.Session["type"].ToString() != "am"
            && HttpContext.Current.Session["type"].ToString() != "c"
            && HttpContext.Current.Session["MSM"].ToString() != "TS")
        {
            DataTable ds = new DataTable();
            ds = (DataTable)HttpContext.Current.Session["userinfo"];
            string SalesAssigned = ds.Rows[0]["SalesAssigned"] == DBNull.Value ? "0" : ds.Rows[0]["SalesAssigned"].ToString();
            if (SalesAssigned == "1")
            {
                IsSalesAsigned = Convert.ToInt32(HttpContext.Current.Session["userid"]);
            }
        }

        return IsSalesAsigned;
    }

    public string Getwebconfig()
    {  
        return HttpContext.Current.Session["config"].ToString();
    }
    public Dictionary<string, string> TelerikPageSize(Int32 TotalCount)
    {
        var sizes = new Dictionary<string, string>() { { "25", "25" }, { "50", "50" }, { "100", "100" } , { "500","500"}, { "1000", "1000" } };
        sizes.Add("All", Convert.ToString(TotalCount));
        return sizes;
    }

    public void CorrectTelerikPager(RadGrid grid)
    {
        //Fix page count in pager
        if (grid.MasterTableView.Items.Count == 0)
        {
            grid.MasterTableView.PagerStyle.PagerTextFormat = "{4}<strong>0</strong> items in <strong>0</strong> pages";
        }
        else if (grid.MasterTableView.Items.Count == 1)
        {
            grid.MasterTableView.PagerStyle.PagerTextFormat = "{4}<strong>{5}</strong> item in <strong>1</strong> page";
        }
        else if (grid.PageSize == int.MaxValue || grid.PageCount <= 1)
        {
            grid.MasterTableView.PagerStyle.PagerTextFormat = "{4}<strong>{5}</strong> items in <strong>1</strong> page";
        }
        else
        {
            grid.MasterTableView.PagerStyle.PagerTextFormat = "{4}<strong>{5}</strong> items in <strong>{1}</strong> pages";
        }
        grid.Rebind();
    }

}




