<%@ WebHandler Language="C#" Class="SignatureHandler" %>

using System;
using System.Web;
using System.Web.SessionState;

public class SignatureHandler : IHttpHandler, IReadOnlySessionState
{

    BusinessLayer.BL_MapData objBL_MapData = new BusinessLayer.BL_MapData();
    BusinessEntity.MapData objMapData = new BusinessEntity.MapData();
    GeneralFunctions objgn = new GeneralFunctions();
    
    public void ProcessRequest (HttpContext context) {

        try
        {
            context.Response.ContentType = "image/png";
            int count = 0;
            string base64 = "R0lGODlhAQABAIAAAAAAAP///yH5BAEAAAAALAAAAAABAAEAAAIBRAA7";   
            System.Data.DataSet ds = new System.Data.DataSet();
            if (context.Request.QueryString["tid"] != null && context.Request.QueryString["wid"] != null)
            {
                if (context.Request.QueryString["wid"].Trim() != string.Empty)
                {
                    objMapData.ConnConfig = context.Session["config"].ToString();
                    objMapData.TicketID = Convert.ToInt32(context.Request.QueryString["tid"]);
                    objMapData.WorkID = Convert.ToInt32(context.Request.QueryString["wid"]);
                    ds = objBL_MapData.GetSignature(objMapData);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        count = Convert.ToInt32(ds.Tables[0].Rows[0]["signatureCount"].ToString());
                        if (count > 0)
                        {
                            count = 1;
                        }

                        if (count == 1)
                        {
                            //base64 = "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";
                            base64 = "iVBORw0KGgoAAAANSUhEUgAAACAAAAAgCAYAAABzenr0AAAEiElEQVR4XsWVbWxTVRjH/+fe29tu68Zmx5jbsmomDLrJNmOiH5iJDiMbiODLF6ISTFg/TEMkmch4MSE6iPIB5lAwgArGxGjcIMICYxoDakKUubEXcSzbpBswq+zlbu29be/xSXM+NHSBda3hn/xyk3uS/p7nOefcMs45YklFRQUSkba2NlCgxPEDdypSBbBbkqTq5ORkv9Pp/KShoWHbTMVLSHCE/GuSb05JSbEXFBRk5ufn17nd7nWYIdL/IZdlebXdbkdhYSGys7PR3d0Nj8fzcAwFxCdPTU2Fy+VCeno6urq6MDo6avj9/i8TXEC0XFGU1WlpaSguLgaNH729vfB6vTAMYyOdmcsJLiBaTh2jpKQEtP/hsZMcuq5vaG1tPYbozO0WnGp8CBT4e91Y+fpVlTEWHrvD4QiPnbpFX18fxsfHEQgEXqPOP8PMie8a+g3zVasqvcShrCI5ioqKMDk5if7+fkxMTIAK2SDkuHsBoqPZJhDktTZVen9HdQ6OfMewqNAVHvfg4GBYTp1HyWOagG3JIcwYUejhb723XlmVgbLFNtTel4yjp25Q50PQNA3BYPBO8vgPoWX4QtamR37a5Bg+CYR0OLOm8cKyMRj6VFhOZyRKnrAzEDpXvEXSRnYh6FNBUQdaYDxQiaULrWh8O+fyAoflK4gkdAKBn59cGPy+5BLTb+0Bk1UoNkC2gAU0qENneSBoaiTXAMgEEjoB4+LKLcwMvgMmJXGLHZAUIGQAZgD0HmDsokWR1gO4QWji25DALZDkN6EkJXFQmAQEFUDygwV1cBgmt85zA7gS9U8ZawEfHOiNsD5BAG+5nS7I1gUAQ8dVjt//tKDsQRtKnRoACbCktMvLezpkcTvE7UnkFtBoJRUcDL/9oWHaD7QPWFFaYIIDnIqrRUQYY+EHkUIkExbhCRJjnHN91gXU1iyZx83ARgbAc1PHtG4i2SahdJECrgCQlF8ty87/IMQMgJVIihBzASNMQo5tAtzcCiZlXLvpR9MZL55/Zj7ysmSwkEFLOsBt9UKcTmSIZkwBF/iICUKPaQuo+1yA13hGfGhquY61lTnIy1YBMzTGGWvWJnlnZnlzJ4BCQiVCAk4EhFgj/HM9A/s81332ptPDWFuVi7z7bb5QyKyv29d+cP+nnaro2CGkJsHFc1jI+ZwPIXW/7trI9IvNLR6sqcpDpkO90Hi8Z0ftnl/GRce4bdRewhBMz/0WiNEPeaY+PnlmGGsq82C1svcyyo6eE6LU2w6WLN4/RuSKA7g7rk/xwF/auyRPe25Fnm/036mXneXHW8USE0jECqKeOEEcI54llhJvEKlxTWD5+hP6F3ufHvy8qWd7/UeXRoTQRiwmlovnj8QB4qy473ViApuJKcQaznkYynyih6ggHiWeIg4L0YdEFZG0q6Ycvp5qUHYS/cR2Ip3ezcYVBRNyMMYeFz/6DVFGuIhW4ggV+HfE1y4fwH5x4HbSWj8iEs8W9BEdQn6F2BUWR+cQcZ7YS3IDcUaJGM8/ALbi7jlInCZ5APFHbME9jIR7nP8A2+XsN1R+MpAAAAAASUVORK5CYII=";
                        }
                    }
                }
            }
            context.Response.BinaryWrite(Convert.FromBase64String(base64));
        }
        catch (Exception ex)
        {
            throw ex;
        }        
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}