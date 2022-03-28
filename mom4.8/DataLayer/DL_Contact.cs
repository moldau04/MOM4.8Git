using BusinessEntity;
using Microsoft.ApplicationBlocks.Data;
using System; 
using System.Data;
using System.Data.SqlClient;
 

namespace DataLayer
{
    public class DL_Contact
    {
        /// <summary>
        /// Get Contact Autojquery
        /// </summary>
        /// <param name="objPropUser"></param>
        /// <param name="IsSalesAsigned"></param>
        /// <returns></returns>
        public DataSet getContactAutojquery(User objPropUser, Int32 IsSalesAsigned = 0)
        {
            try
            {


                var sqlpram = new SqlParameter[5];
                sqlpram[0] = new SqlParameter()
                {
                    ParameterName = "SearchValue",
                    Value = objPropUser.SearchValue
                };
                sqlpram[1] = new SqlParameter()
                {
                    ParameterName = "Customer",
                    Value = objPropUser.CustomerID
                };
                sqlpram[2] = new SqlParameter()
                {
                    ParameterName = "Location",
                    Value = objPropUser.LocID
                };
                sqlpram[3] = new SqlParameter()
                {
                    ParameterName = "Job",
                    Value = objPropUser.JobId
                };
                sqlpram[4] = new SqlParameter()
                {
                    ParameterName = "IsSalesAsigned",
                    Value = IsSalesAsigned
                };


                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, "spGetContactAutoSearch", sqlpram);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
