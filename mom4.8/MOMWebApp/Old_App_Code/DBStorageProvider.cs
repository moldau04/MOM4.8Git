using System;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using Telerik.Web.UI.PersistenceFramework;



/// <summary>
/// Summary description for DBStorageProvider
/// </summary>

public class DBStorageProvider: IStateStorageProvider
{



    SqlConnection Conn = new SqlConnection(HttpContext.Current.Session["config"].ToString());
  
    public SqlDataAdapter SqlDataAdapter = new SqlDataAdapter();
   
    public SqlCommand cmd = new SqlCommand();

    public void SaveStateToStorage(string userID, string serializedState)
    {
        
        string userSettings = serializedState;
        

        try
        {

            //Open the SqlConnection
            Conn.Open();
           //Update Query to update the Datatable  
            string updateQuery = "SET TRANSACTION ISOLATION LEVEL SERIALIZABLE; BEGIN TRANSACTION; UPDATE UserSetting SET UserSaveSettings='" + userSettings + "' where UserId=" +Convert.ToInt16(userID) + " IF @@ROWCOUNT = 0 BEGIN  INSERT into UserSetting(UserSaveSettings,UserId) values ('" + userSettings + "'," + userID + ") END COMMIT TRANSACTION;";
          
            cmd.CommandText = updateQuery;
            cmd.Connection = Conn;
            cmd.ExecuteNonQuery();
            //Close the SqlConnectio
            Conn.Close();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public string LoadStateFromStorage(string key)
    {
     
        string selectQuery = "SELECT  UserSaveSettings FROM UserSetting WHERE UserID = " + Convert.ToInt16(key) ;
        SqlDataAdapter adapter = new SqlDataAdapter();
        adapter.SelectCommand = new SqlCommand(selectQuery, Conn);
        DataTable myDataTable = new DataTable();
        Conn.Open();

        try
        {
            adapter.Fill(myDataTable);
        }
        finally
        {
            Conn.Close();
        }
        if (myDataTable.Rows.Count > 0)
        {
            return myDataTable.Rows[0]["UserSaveSettings"].ToString();
        }
        else
        {
            string str;
            return str = string.Empty;
        }
       
    }
    


}


