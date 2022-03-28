<%@ WebHandler Language="C#" Class="Handler" %>

using System;
using System.Web;
using System.IO;

public class Handler : IHttpHandler
{
    public void ProcessRequest(HttpContext context)
    {
        string filename = string.Empty;
        if (context.Request.QueryString["filename"] != null)
        {
            filename = context.Request.QueryString["filename"];

        }
        else
        {
            filename = "Invoice.pdf";
        }
        string filepath = Path.Combine(HttpContext.Current.Request.MapPath(HttpContext.Current.Request.ApplicationPath) + "/TempPDF/SendInvoice", filename);
        if (File.Exists(filepath))
        {
            HttpContext.Current.Response.ContentType = "Application/pdf";
            HttpContext.Current.Response.WriteFile("~/TempPDF/SendInvoice/" + filename);
        }
        HttpContext.Current.Response.End();
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
}