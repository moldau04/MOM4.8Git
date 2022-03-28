using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLayer;
using System.Data.SqlClient;
using System.IO;
using Microsoft.ApplicationBlocks.Data;

public partial class AddCompany : System.Web.UI.Page
{
    BusinessEntity.User objPropUser = new BusinessEntity.User();
    BL_User objBL_User = new BL_User();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["MSM"] == null)
        {
            Response.Redirect("login.aspx");
        }
        if (Session["MSM"].ToString() != "ADMIN")
        {
            Response.Redirect("home.aspx");
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            objPropUser.FirstName = txtCompany.Text;
            objPropUser.Address = txtAddress.InnerText;
            objPropUser.City = txtCity.Text;
            objPropUser.State = txtState.Text;
            objPropUser.Zip = txtZip.Text;
            objPropUser.Tele = txtTele.Text;
            objPropUser.Fax = txtFax.Text;
            objPropUser.Email = txtEmail.Text;
            objPropUser.Website = txtWebAdd.Text;
            objPropUser.MSM = ddlDBType.SelectedValue;
            objPropUser.DSN = string.Empty; //txtDSN.Text.Trim();
            objPropUser.DBName = txtDB.Text.Trim();
            objPropUser.Password = string.Empty; //txtDpass.Text.Trim();
            objPropUser.Username = string.Empty; //txtDuser.Text.Trim();
            objPropUser.Type = ddlDBType.SelectedValue;
            objPropUser.ConnConfig = Connectionstr(txtDB.Text.Trim());
            objPropUser.ContactName = txtContName.Text;
            objPropUser.Remarks = txtRemarks.InnerText;
            objPropUser.Lat = txtLat.Text;
            objPropUser.Lng = txtLng.Text;
            objPropUser.Country = ddlCountry.SelectedValue;
            objPropUser.Cell = txtCell.Text;
            //DataSet dsDbname = new DataSet();
            //dsDbname = objBL_User.getDatabases(objPropUser);

            //if (dsDbname.Tables[0].Rows.Count == 0)
            //{
            //objBL_User.CreateDatabase(objPropUser);
            //CreateDatabaseObjects(txtDB.Text.Trim());
            objBL_User.AddCompany(objPropUser);
            objBL_User.AddDatabaseName(objPropUser);
            //lblMsg.Text = "Database created successfully.";
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "noty({text: Database created successfully.,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
            ClearControls();
            //}
            //else
            //{
            //    lblMsg.Text = "Database already exists, please use different database name.";
            //    return;
            //}
        }
        catch (Exception ex)
        {
            //lblMsg.Text = ex.Message;
            //ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "noty({text: '" + ex.Message + "',  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    private void ClearControls()
    {
        ResetFormControlValues(this);
    }

    private void ResetFormControlValues(Control parent)
    {
        foreach (Control c in parent.Controls)
        {
            if (c.Controls.Count > 0)
            {
                ResetFormControlValues(c);
            }
            else
            {
                switch (c.GetType().ToString())
                {
                    case "System.Web.UI.WebControls.DropDownList":
                        ((DropDownList)c).SelectedIndex = -1;
                        break;
                    case "System.Web.UI.WebControls.TextBox":
                        ((TextBox)c).Text = "";
                        break;
                    case "System.Web.UI.WebControls.CheckBox":
                        ((CheckBox)c).Checked = false;
                        break;
                    case "System.Web.UI.WebControls.RadioButton":
                        ((RadioButton)c).Checked = false;
                        break;
                }
            }
        }
    }

    protected void lnkClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("adminpanel.aspx");
    }

    private void CreateDatabaseObjects(string dbname)
    {
        FileInfo fileInfo = new FileInfo(Server.MapPath(Request.ApplicationPath) + "/scripts/CreateDBObjects.sql");

        string script = fileInfo.OpenText().ReadToEnd();

        string constr = Connectionstr(dbname);

        SqlConnection connection = new SqlConnection(constr);
        //Server serversql = new Server(new ServerConnection(connection));
        //serversql.ConnectionContext.ExecuteNonQuery(script);
        ////serversql.ConnectionContext.Disconnect();
        ////connection.Close();
    }

    ////private void DropDatabase(string dbname)
    ////{
    ////    string constr = Connectionstr(dbname);
    ////    SqlConnection connection = new SqlConnection(constr);
    ////    Server serversql = new Server(new ServerConnection(connection));
    ////    serversql.Databases[dbname].Drop();
    ////    //connection.Close();
    ////}

    private string Connectionstr(string dbname)
    {
        string server = Config.MS.Split(';')[0].Split('=')[1];
        string database = dbname;
        string user = Config.MS.Split(';')[2].Split('=')[1];
        string pass = Config.MS.Split(';')[3].Split('=')[1];

        string constr = "server=" + server + ";database=" + database + ";user=" + user + ";password=" + pass + "";
        return constr;
    }

}
