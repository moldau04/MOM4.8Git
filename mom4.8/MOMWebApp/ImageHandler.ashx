<%@ WebHandler Language="C#" Class="ImageHandler" %>

using System;
using System.Web;
using System.Data;
using BusinessLayer;
using System.Web.SessionState;

public class ImageHandler : IHttpHandler, IReadOnlySessionState
{

    BL_User objBL_User = new BL_User();
    BusinessEntity.User objProp_User = new BusinessEntity.User();

    public void ProcessRequest(HttpContext context)
    {
        try
        {
            DataSet ds = new DataSet();
            if (context.Request.QueryString["catid"] != null)
            {
                objProp_User.ConnConfig = context.Session["config"].ToString();
                objProp_User.Cat = context.Request.QueryString["catid"].ToString();
                ds = objBL_User.getcategoryAll(objProp_User);
            }

            context.Response.ContentType = "image/png";
            int success = 0;
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["icon"] != DBNull.Value)
                {
                    context.Response.BinaryWrite((byte[])ds.Tables[0].Rows[0]["icon"]);
                    success = 1;
                }
            }

            if (success == 0)
            {
                if (context.Request.QueryString["assign"] == null)                
                    context.Response.BinaryWrite(Convert.FromBase64String("R0lGODlhAQABAIAAAAAAAP///yH5BAEAAAAALAAAAAABAAEAAAIBRAA7"));
                else if (context.Request.QueryString["assign"] == "U")
                    context.Response.BinaryWrite(Convert.FromBase64String("iVBORw0KGgoAAAANSUhEUgAAABUAAAAiCAYAAACwaJKDAAAABmJLR0QA/wD/AP+gvaeTAAADdUlEQVRIia2UYWhVZRjHf+859+zuygIXyzRniBKOCYYMaehofZBYKiL0oVjhl2mhVCvEog/pmGHkl1pFhMQgqRS9wpBMkOWYzsKtzDmntbJNXdnaZdbd8HbvOed9+nDPXfeee3fvnez58nKe9/n/zvM87/O+ivy2EqgDFgMGoIHfgR5gsIA2y9YDvYBUVihpqDHlmcdNaagxpbJCCSDe/vpcYpXj+x2leP3ptaZq3hJg1VID5YvqH9a0dTgc/9YVEQ4Ab3o/yoIq4L2yEM3tr5bQUGMWLOfU9y5NbQmmYrQBr6XA6crmUouWjreCPLHK5KdRwXGhLJSZ5lQMBm9qFpYrHllssK7a5Ng5t9bR3AEuQLL5AA8B+/c0WtRWJV1N7yc49I2blV3fL5r6N+LEneR3bZXB3ucsgP0eZxq6Y8kDat6LGwIFS85lLzwV4OEFah6wIx26ubHexCrcxpxmmdBYbwJsTkFNoGrdynskera22gSoAkwDsICSJRX+6ZqdefoSwEqVj8iM8UVZut4AEkDi1ngmtVDe/n1PnwASqft8redq5viU3weRaHb64/8IpRYErUy/p78G6FT5Jw53uySc/4M2rjE50u1y4Wc97RsZEw6EbTY9lnmotguHu12AE+n+RcBk6/OWRMMhiYZDcudoSJqeDIhSyIL5SpY+qMQ0kI1rTLn5Wel0XDQckre3WgJMepyM1rxUEuDDjj1B6qqnz4+xv4WBYY1SsKLSoNI3JeevarbsixO3eRn4yA8FaCsL8crJliCrlxsUsv7fNBv2xpmM8QHQnPLnevo+X3S/aux+N8jC8plnYGJSqNsdZzQiR4FnSXv6/OkIsO32hAzs/NjOO7u7PrUZjcgQ0JQOzAUFiAFbO390nWM92a8UQNdlzfHzLsB2YMq/P1PjLgGftHxhE7d9pQi0fmkDhIGzucT5TmPfaERiR846Gc5zg5offtUCtM4kzAf9Czh08FRmC9pPOwBngIF7gQIcHBjRXLmRvFXRu8LXfS5Aez5RIehFYOir3iS085LmX5sYvus4WyjAya7+ZAvOJNcucpz4bKFdF69rEg70DGqAzkKCYqB9cRt6hzTDYwLwXRGaomx856aAAA4Qmito36PLDAGuFxNcTPkAkeE/BeDGXEInoncFkg/xnEHHvPWPuYTe9taRYoL/A8ZvSUCZVlicAAAAAElFTkSuQmCC"));
                else if (context.Request.QueryString["assign"] == "A")
                    context.Response.BinaryWrite(Convert.FromBase64String("iVBORw0KGgoAAAANSUhEUgAAABUAAAAiCAYAAACwaJKDAAAABmJLR0QA/wD/AP+gvaeTAAADqUlEQVRIia2VW2xURRjHf3PO3npJLdUmuysFQ6xATDBGDas00SgPfSIQYpA+8AJKIJrGaEx8UQJJTXwxqLGGhz4YE6RQg43iS0MVQQOxQFmUuCbS0nW3lHZrd3vZy5kZH/bC7p5ttzT9ks3Jzvz/v++bmfPNESwdTwJtwKOAASjgX+Ai8EcVry22A1cALTxrtdncrk3/Hm02t2vhWasBnZvfXsksKvz/CMR7pm+3cDzWidGwxSZT8WGskePIaJ8G/THwfi6RDSqAT3DUd7q29GA2t1ddjrz3I+kb+8GaPQ68nQebRZpODM8R97NnMR9+qdStkqhEEJSFcD5UGDbqWjGbtiGjpwNoaxq4DNnNB/ADXc7WDzAaA/aKon2kfnuR9LW9tjmjMYDziQ8BunKcAvSQqGmpdaw/WHGZVvQURuNWVOIGei5km3esewNRs64WOFQM3WH6O0A4bQadGkfFLuDceAxRuwEretqeVTgx/R0AO/JQE9hkrtlWsUoZPYPwtGA0BjC9u5CVoIC55gWATYBpAE7AJWpaFoGewvTvyRq9u9Dz/6BmhuzFZv0uwGkURrW2CfXcX6j4MEbD0+i5EMLwINy+ytUW+R1AGkjr5JhL1D1eorMivSAcZIIHi7wZ5Pi3ODd2gSiqKTlGnpXv51sydrE8NTLai2PDu3heCRd+7sBPhcMr2aas/xag8qn6ZeQkqHRBpP67jF4YxeF7tcRs1G/GqN9c+hboDDJyEqAf7r9S3ToZnrVGv7ifOdKL0fAUoq7Vtn2mdzfqbj+oVHabRrvRyfAs0A2lvf8mhusz9zNnMZrabKDFQk1fIvX7TlCpt4DPobT3r6Blk5z4bqv5yMsIt686MD5MamgnyPlPgaP58UpX39fC7etwP/8zwu1dFKgzMVK/tqGT4V7gNYquPqNcCxzQqWgwc/Nwsc4WmT/fQSfDIWB/ubAcCrAA7JOTA9ZiLammBpHjfQCvA7Pl85WgANeBLzOhI4UTLl5M5u+jAGeAC+XGpaAAx3QyvGBFvimtMvYLamZIU3QwDwKdAL6Sd06UDFpjPQDngeBKoAAnVCKIStwEQFtx5MQ5gJ6lTNWgV4GQmvgeADU5ACq5QK4dVwoF+EFODQIgp84DDFLhxB8UOqhmroJKo7I30cAyPFXDC2j3c+c0CA3YP7crjHuO9Yc1YAE11cTLWT7AiJq+BDBKtuNWBTqp52/noVVjudCYtuIAidWE3s09I6sJjeaeI8sR/w+7iG96YmT6GgAAAABJRU5ErkJggg=="));
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