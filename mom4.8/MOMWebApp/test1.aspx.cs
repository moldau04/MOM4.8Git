using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using BusinessEntity;
using BusinessLayer;
using Microsoft.ApplicationBlocks.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Data;
using System.Net;
using System.Net.Mail;
using System.Text;
using AjaxControlToolkit;

public partial class test1 : System.Web.UI.Page
{
    General objgeneral = new General();
    BL_General objBL_General = new BL_General();

    GeneralFunctions objGeneralFunctions = new GeneralFunctions();

    
    
    protected void Page_Load(object sender, EventArgs e)
    {
        //objgeneral.ServiceName = "IOSGetLog";
        //objgeneral.Error = "test";
        //objBL_General.LogError(objgeneral);
       // string str="not null";
       //Response.Write( IsNull(str, "null value passed"));

        //Response.Write(Server.MapPath("~")+"----------");
        ////Response.Write("-----"+Request.ApplicationPath);

        //string webRootPath = Server.MapPath("~");
        //string docPath = Path.GetFullPath(Path.Combine(webRootPath, "../../../../../MyDocument.xml"));
        //Response.Write(docPath);

       Response.Write( objGeneralFunctions.RNGCharacterMask(10));
    }

    private string IsNull(string input, string replacement)
    {
        string output = string.Empty;

        if (!string.IsNullOrEmpty(input))
        {
            output = input;
        }
        else
        {
            output = replacement;
        }

        return output;
    }

    public void GetIP()
    {
        string strHostIP = String.Empty;
        using (WebClient objWebClient = new WebClient())
        {
            using (StreamReader objStreamReader = new
            StreamReader(objWebClient.OpenRead("http://www.whatismyip.org/")))
            {
                strHostIP = objStreamReader.ReadToEnd();
                objStreamReader.Close();
                Response.Write(strHostIP);
                //Response.Write(EncryptDES(strHostIP, "1"));
                //Response.Write(DecryptDES(EncryptDES(strHostIP, "1"), "1"));
            }
        }
    }
   
    public void SendMail()
    {
        string From = "admin@test.com";
        string str = "";
        string To = "kunal.panchal@ideavate.com";
        //int success = 0;

        //if (From == string.Empty && To == string.Empty)
        //{
        //    success = 0;
        //    return success;
        //}
        char[] separator = new char[] { ';' };
        string[] strSplitArr = To.Split(separator);

        try
        {
            MailMessage mm = new MailMessage();
            mm.Subject = "Storz Evaluator forgot password information.";
            string strBody = "Hi";            
            mm.Body = strBody;
            mm.IsBodyHtml = true;
            mm.Priority = MailPriority.High;

            SmtpClient smtp = new SmtpClient();

            MailAddress ma = new MailAddress(From);
            mm.From = ma;

            //if (CC != string.Empty)
            //{
            //    string[] strSplitArrCC = CC.Split(separator);
            //    foreach (string arrStrCC in strSplitArrCC)
            //    {
            //        mm.CC.Add(arrStrCC);
            //    }
            //}
            foreach (string arrStrTo in strSplitArr)
            {
                mm.To.Add(arrStrTo);
            }

            smtp.Send(mm);
            //str = "Mail sent successfully.";
            //success = 1;
        }
        catch (Exception ex)
        {
            str = "Error sending mail : " + ex.Message;
            Response.Write(str);
            //success = 0;
        }

        //return success;
    }

    protected void ajaxUpload1_OnUploadComplete(object sender, AjaxControlToolkit.AjaxFileUploadEventArgs e)
    {
        // Generate file path
        string filePath = "C:/Uploads/" + e.FileName;

        // Save upload file to the file system
        //ajaxUpload1.SaveAs(MapPath(filePath));
        ajaxUpload1.SaveAs(filePath);
        //byte[] byt= e.GetContents();
    }

    protected void btnUpload_Click(object sender, EventArgs e)
    {
        //HttpPostedFile file;

        //foreach (string key in Request.Files.Keys)
        //{
        //    file = Request.Files[key];

        //    if (file != null && file.ContentLength > 0)
        //    {
        //        file.SaveAs("C:\\UploadedUserFiles\\" + file.FileName);
        //    }
        //}

        //for (int i = 0; i < Request.Files.Count; i++)
        //{
        //    HttpPostedFile file = Request.Files[i];
        //    if (file.ContentLength > 0)
        //    {
        //        file.SaveAs("C:\\UploadedUserFiles\\" + file.FileName);
        //    }
        //}


        lblStatus.Text = "Uploading...";
        System.Threading.Thread.Sleep(2000);
        string temp = "";
        HttpFileCollection uploadFiles = Request.Files;
        for (int i = 0; i <= uploadFiles.Count - 1; i++)
        {
            HttpPostedFile uploadFile = uploadFiles[i];
            string fileName = System.IO.Path.GetFileName(uploadFile.FileName);
            if (fileName.Trim().Length > 0)
            {
                //uploadFile.SaveAs(Server.MapPath("./Others/") + fileName)
                uploadFile.SaveAs("C:\\UploadedUserFiles\\" + fileName);
                temp = temp + fileName + " Successfully Uploaded <br/>";
            }
        }
        lblStatus.Text = temp;
    }

    protected  void AsyncFileUpload1_UploadedComplete(object sender, AsyncFileUploadEventArgs e)
    {
        string savePath = "C:\\UploadedUserFiles\\" +e.FileName;
        AsyncFileUpload1.SaveAs(savePath);
    }      
}
