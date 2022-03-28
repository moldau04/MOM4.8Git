<%@ WebHandler Language="C#" Class="QRhandler" %>

using System;
using System.Web;
using ZXing;
using ZXing.Common;
using System.IO;

public class QRhandler : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {

        if (context.Request.QueryString["id"] != null)
        {
            var qrValue = context.Request.QueryString["id"].ToString();
            if (qrValue.ToString().Trim() != string.Empty)
            {
                var barcodeWriter = new BarcodeWriter
                {
                    Format = BarcodeFormat.QR_CODE,
                    Options = new EncodingOptions
                    {
                        Height = 200,
                        Width = 200,
                        Margin = 1
                    }
                };

                using (var bitmap = barcodeWriter.Write(qrValue))
                using (var stream = new MemoryStream())
                {
                    bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                    context.Response.ContentType = "image/png";
                    context.Response.BinaryWrite(stream.GetBuffer());
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