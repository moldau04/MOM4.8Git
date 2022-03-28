using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.Odbc;

public partial class testodbc : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        connect();
    }

    private void connect()
    {
        OdbcDataAdapter da = new OdbcDataAdapter("Select * from master_arm_customer where date_stamp >= '{d 2015-04-27}' and time_stamp >= '{t 05:09:00.000}'", "Dsn=sagedb");
        DataTable dt = new DataTable();
        da.Fill(dt);
        gvResults.DataSource = dt;
        gvResults.DataBind();

        //string connetionString = null;

        //OdbcConnection cnn;

        //connetionString = "Dsn=MOM Kencor Data";

        //cnn = new OdbcConnection(connetionString);

        //try
        //{

        //    cnn.Open();

        //    Response.Write("Connection Open ! ");

        //    cnn.Close();

        //}

        //catch (Exception ex)
        //{

        //    Response.Write(ex.Message);

        //}

    }

}
