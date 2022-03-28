<%@ WebHandler Language="C#" Class="AttachmentImage" %>

using System;
using System.Web;
using BusinessLayer;
using BusinessEntity;
using System.Data;
using System.IO;
using System.Web.SessionState;
using System.Drawing;

public class AttachmentImage : IHttpHandler, IReadOnlySessionState
{
    BL_Job objBL_Job = new BL_Job();
    JobT objJob = new JobT();

    public void ProcessRequest(HttpContext context)
    {
        string imgPath = string.Empty;
        if (context.Request.QueryString["docid"] != null)
        {
            objJob.ConnConfig = context.Session["config"].ToString();
            objJob.docid = Convert.ToInt32(context.Request.QueryString["docid"].ToString());
            imgPath = objBL_Job.GetAttachmentByID(objJob);
        }
        context.Response.ContentType = "image/png";
        int success = 0;

        if (imgPath != string.Empty)
        {
            if (context.Request.QueryString["thumb"] == null)
            {
                FileStream fs = new FileStream(imgPath, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);
                context.Response.BinaryWrite(br.ReadBytes((Int32)fs.Length));
                success = 1;
            }
            else
            {
                using (var ms = new MemoryStream())
                {
                    Image image = Image.FromFile(imgPath);
                    Size thumbnailSize = GetThumbnailSize(image);
                    Bitmap bitmap = CreateThumbnail(imgPath, thumbnailSize.Width, thumbnailSize.Height);
                    bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    context.Response.BinaryWrite(ms.ToArray());
                    success = 1;
                }
            }

        }
        if (success == 0)
        {
            context.Response.BinaryWrite(Convert.FromBase64String("R0lGODlhAQABAIAAAAAAAP///yH5BAEAAAAALAAAAAABAAEAAAIBRAA7"));
        }
    }
    static Size GetThumbnailSize(Image original)
    {
        // Maximum size of any dimension.
        const int maxPixels = 100;

        // Width and height.
        int originalWidth = original.Width;
        int originalHeight = original.Height;

        // Compute best factor to scale entire image based on larger dimension.
        double factor;
        if (originalWidth > originalHeight)
        {
            factor = (double)maxPixels / originalWidth;
        }
        else
        {
            factor = (double)maxPixels / originalHeight;
        }

        // Return thumbnail size.
        return new Size((int)(originalWidth * factor), (int)(originalHeight * factor));
    }
    public bool ThumbnailCallback()
    {
        return false;
    }
    private Bitmap CreateThumbnail(string lcFilename, int lnWidth, int lnHeight)
    {

        System.Drawing.Bitmap bmpOut = null;
        try
        {
            Bitmap loBMP = new Bitmap(lcFilename);
            System.Drawing.Imaging.ImageFormat loFormat = loBMP.RawFormat;

            decimal lnRatio;
            int lnNewWidth = 0;
            int lnNewHeight = 0;

            //*** If the image is smaller than a thumbnail just return it
            if (loBMP.Width < lnWidth && loBMP.Height < lnHeight)
                return loBMP;


            if (loBMP.Width > loBMP.Height)
            {
                lnRatio = (decimal)lnWidth / loBMP.Width;
                lnNewWidth = lnWidth;
                decimal lnTemp = loBMP.Height * lnRatio;
                lnNewHeight = (int)lnTemp;
            }
            else
            {
                lnRatio = (decimal)lnHeight / loBMP.Height;
                lnNewHeight = lnHeight;
                decimal lnTemp = loBMP.Width * lnRatio;
                lnNewWidth = (int)lnTemp;
            }

            // System.Drawing.Image imgOut =
            //      loBMP.GetThumbnailImage(lnNewWidth,lnNewHeight,
            //                              null,IntPtr.Zero);

            // *** This code creates cleaner (though bigger) thumbnails and properly
            // *** and handles GIF files better by generating a white background for
            // *** transparent images (as opposed to black)
            bmpOut = new Bitmap(lnNewWidth, lnNewHeight);
            Graphics g = Graphics.FromImage(bmpOut);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            g.FillRectangle(Brushes.White, 0, 0, lnNewWidth, lnNewHeight);
            g.DrawImage(loBMP, 0, 0, lnNewWidth, lnNewHeight);

            loBMP.Dispose();
        }
        catch
        {
            return null;
        }

        return bmpOut;
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}