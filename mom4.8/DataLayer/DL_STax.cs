using BusinessEntity;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Data;
using System.Data.SqlClient;

namespace DataLayer
{
    public class DL_STax
    {
        public DataSet GetAllSTaxByUType(STax info)
        {
            try
            {
                SqlParameter para;
                para = new SqlParameter
                {
                    ParameterName = "UType",
                    SqlDbType = SqlDbType.VarChar,
                    Value = info.UType
                };

                //string sql = "select Name +' / ' + CONVERT(VARCHAR(50),Rate) as NameRate, * from STax where UType<>" + info.UType;
                return SqlHelper.ExecuteDataset(info.ConnConfig, CommandType.StoredProcedure, "spGetEstimateTax", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
