<%@ WebHandler Language="C#" Class="Email_EmbedImg" %>

using System;
using System.Web;
using System.IO;

public class Email_EmbedImg : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        
        if (context.Request.QueryString["imgid"] != null)
        {
            string savepathconfig = System.Web.Configuration.WebConfigurationManager.AppSettings["AttachmentPath"].Trim();
            string savepath = savepathconfig + @"\Email_Embedded\";
            string filename = context.Request.QueryString["imgid"].ToString();
            string fullpath = savepath + filename;
            byte[] imageArray = File.ReadAllBytes(fullpath);                        
            context.Response.ContentType = "image/png";
            context.Response.BinaryWrite(imageArray);
        }
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}