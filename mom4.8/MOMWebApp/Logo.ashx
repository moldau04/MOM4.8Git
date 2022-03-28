<%@ WebHandler Language="C#" Class="Logo" %>

using System;
using System.Web;
using System.Data;
using BusinessEntity;
using BusinessLayer;

public class Logo : IHttpHandler {

    User objPropUser = new User();
    BL_User objBL_User = new BL_User();

    public void ProcessRequest(HttpContext context)
    {
        try
        {

            DataSet ds = new DataSet();
            objPropUser.DBName = context.Request.QueryString["db"].ToString();
            if (objPropUser.DBName != "ahe")
            {
                ds = objBL_User.getLogo(objPropUser);

                context.Response.ContentType = "image/jpg";
                if (ds.Tables[0].Rows[0]["logo"] != DBNull.Value)
                {
                    context.Response.BinaryWrite((byte[])ds.Tables[0].Rows[0]["logo"]);
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}