using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using BusinessEntity;

namespace DataLayer
{
    public class DL_UserCustom
    {
        public DataSet GetAllUserCustom(String conn, String dbName)
        {
            try
            {
                return SqlHelper.ExecuteDataset(conn, "spGetUserCustomFields", dbName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int CreateAndUpdateUserCustom(UserCustom user)
        {
            int success = 0;
            String strConnString = user.ConnConfig;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "spUpdateUserCustomFields";
            cmd.Parameters.Add("@UserCustomItem", SqlDbType.Structured).Value = user.UserCustomItem;
            cmd.Parameters.Add("@DeleteUserCustomItem", SqlDbType.Structured).Value = user.UserCustomItemDelete;
            cmd.Parameters.Add("@UserCustom", SqlDbType.Structured).Value = user.UserCustomValue;
            cmd.Connection = con;

            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                success = -1;
                throw ex;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }

            return success;
        }

        public DataSet GetUserCustomFieldValue(string conn, int userId)
        {
            DataSet ds = new DataSet();
            SqlConnection con = new SqlConnection(conn);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "spGetUserCustomFieldsValue";
            cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = userId;
            cmd.Connection = con;
            try
            {
                con.Open();
                var sqlda = new SqlDataAdapter(cmd);
                ds = new DataSet();
                sqlda.Fill(ds, "UserCustomFieldsValue");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
            return ds;
        }


    }
}
