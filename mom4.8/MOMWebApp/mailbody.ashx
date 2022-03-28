<%@ WebHandler Language="C#" Class="mailbody" %>

using System;
using System.Web;
using System.IO;
using OpenPop.Mime;
using System.Text;
using HtmlAgilityPack;

public class mailbody : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {

        string savepathconfig = System.Web.Configuration.WebConfigurationManager.AppSettings["AttachmentPath"].Trim();
        string filepath = savepathconfig + @"\mails\" + context.Request.QueryString["aid"].ToString() + ".eml";
        FileInfo file = new FileInfo(filepath);        
        Message message = Message.Load(file);        
        MessagePart selectedMessagePart = message.MessagePart;

        context.Response.ContentType = "text/html; charset=utf-8";
        if (selectedMessagePart.IsText)
        {
            context.Response.BinaryWrite(selectedMessagePart.Body);
        }
        else
        {
            MessagePart HTMLTextPart = message.FindFirstHtmlVersion();
            MessagePart plainTextPart = message.FindFirstPlainTextVersion();

            if (HTMLTextPart != null)
            {
                 //context.Response.BinaryWrite(HTMLTextPart.Body);
                
                 HtmlDocument doc = new HtmlDocument();
                 doc.Load(new MemoryStream(Encoding.Convert(Encoding.UTF8, Encoding.GetEncoding("ISO-8859-1"), HTMLTextPart.Body)));
                 HtmlNodeCollection imgs = doc.DocumentNode.SelectNodes("//img[@src]");
                 if (imgs != null)
                 {
                     foreach (HtmlNode img in imgs)
                     {
                         if (img.Attributes["src"].Value.StartsWith("CID:", StringComparison.CurrentCultureIgnoreCase))
                         {
                             HtmlAttribute src = img.Attributes["src"];
                             string cid = src.Value.Split(':')[1];

                             foreach ( MessagePart m in message.FindAllAttachments())
                             {
                                 if (m.ContentId.Equals(cid, StringComparison.CurrentCultureIgnoreCase))
                                 {
                                     img.Attributes["src"].Value = "data:image/jpeg;base64," + Convert.ToBase64String(m.Body);
                                 }
                             }
                         }
                     }
                 }
                 context.Response.Write(doc.DocumentNode.OuterHtml);                                
            }
            else if (plainTextPart != null)
            {
                context.Response.BinaryWrite(plainTextPart.Body);
            }
            else
            {
                System.Collections.Generic.List<MessagePart> textVersions = message.FindAllTextVersions();
                if (textVersions.Count >= 1)
                {
                    context.Response.BinaryWrite(textVersions[0].Body);
                }
                else
                {
                    context.Response.Write("<<OpenPop>> Cannot find a text version body in this message to show <<OpenPop>>");
                }
            }
        }       
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

    

}