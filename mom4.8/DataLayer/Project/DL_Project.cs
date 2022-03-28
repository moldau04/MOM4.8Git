using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Project
{
    public class DL_Project
    {
         
        public DataSet GetProjectServiceTypeinfo(string ConnectionString, string servicetype)
        {
            try
            {
             
                return SqlHelper.ExecuteDataset(ConnectionString, "spGetProjectServiceTypeinfo", CommandType.StoredProcedure, servicetype);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataSet GetProjectServiceType(string ConnectionString, string servicetype)
        {
            try
            {
                var strdb = new StringBuilder();

                strdb.AppendLine(" SELECT"); 
                strdb.AppendLine(" l.type Name,");
                strdb.AppendLine(" l.type Value "); 
                strdb.AppendLine(" FROM ltype l  ");              
                return SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, strdb.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
