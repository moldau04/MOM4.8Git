using System;
using System.Collections.Generic;
using System.Data;
//using System.Drawing;

namespace BusinessEntity.Utility
{
    public class APIUtility
    {
        public string Token { get; set; } 
        public string Param { get; set; } 
        public string DecryptedToken { get; set; } 
        public string ConnectionString { get; set; }

    }

    public class APIRequest
    {
        public string Token { get; set; }  
        public string Param { get; set; }
  
    }
     

    public class APIResponse
    { 
        public string contentType { get; set; }
        public string statusCode { get; set; }
        public string value { get; set; } 
        public string ResponseData { get; set; }

    }
     

    public class UserAuthentication
    {
        public int User_Id { get; set; }
        public string Token { get; set; }
        public string company { get; set; } 
        public string Domain_Name { get; set; }
        public string Connectionstring { get; set; }
        public bool IsValid { get; set; }

}


    public class _Error
    {
        public string errorText { get; set; }

    }

    public static class CommonMethods
    {
        public static DataSet ToDataSet<T>(this IList<T> list)
        {
            Type elementType = typeof(T);
            DataSet ds = new DataSet();
            DataTable t = new DataTable();
            ds.Tables.Add(t);

            //add a column to table for each public property on T
            foreach (var propInfo in elementType.GetProperties())
            {
                Type ColType = Nullable.GetUnderlyingType(propInfo.PropertyType) ?? propInfo.PropertyType;
                
                t.Columns.Add(propInfo.Name, ColType);
            }

            //go through each property on T and add each value to the table
            foreach (T item in list)
            {
                DataRow row = t.NewRow();

                foreach (var propInfo in elementType.GetProperties())
                {
                    row[propInfo.Name] = propInfo.GetValue(item, null) ?? DBNull.Value;
                }

                t.Rows.Add(row);
            }

            return ds;
        }

        //public static byte[] imageToByteArray(object ImgParam)
        //{
        //    byte[] bytes = (byte[])ImgParam;
        //    return bytes;
        //}
    }


    public class Core_Session_Data
    {
        public int User_ID { get; set; }
        public string User_Token { get; set; }
        public string Session_Key { get; set; }
        public string Session_Data { get; set; }

    }


    public enum Session_Key
    {
        Vendor_List,
        Bill_List
    }

}
