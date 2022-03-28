using BusinessEntity;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace DataLayer.Programs
{
    public class DL_ServiceType
    {
        public DataSet GetSetupServiceTypeDropDownValue(string ConnectionString, string SearchBy, string Case)
        {
            try
            {
                var para = new SqlParameter[2];

                para[0] = new SqlParameter
                {
                    ParameterName = "@SearchBy",
                    SqlDbType = SqlDbType.VarChar,
                    Value = SearchBy
                };
                para[1] = new SqlParameter
                {
                    ParameterName = "@Case",
                    SqlDbType = SqlDbType.VarChar,
                    Value = Case
                };
                return SqlHelper.ExecuteDataset(ConnectionString, "GetServiceTypeDDLData", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetServiceType(string ConnectionString)
        {
            try
            {
                var strdb = new StringBuilder();

                strdb.AppendLine(" SELECT");
                strdb.AppendLine("     ISNULL(l.Reg, 0) AS RT,");
                strdb.AppendLine("     ISNULL(l.OT, 0) AS OT,");
                strdb.AppendLine("     ISNULL(l.NT, 0) AS NT,");
                strdb.AppendLine("     ISNULL(l.DT, 0) AS DT,");
                strdb.AppendLine("     l.type,");
                strdb.AppendLine("     l.fdesc,");
                strdb.AppendLine("     l.remarks,");
                strdb.AppendLine("     (SELECT COUNT(1) FROM elev WHERE cat = l.type) AS Count,");
                strdb.AppendLine("     l.InvID,");
                strdb.AppendLine("     ISNULL(i.Name, '') AS Name,");
                strdb.AppendLine("     ISNULL(l.Status,0) AS Status,");
                strdb.AppendLine("     CASE WHEN ISNULL(l.Status,0) = 0 THEN 'Active'");
                strdb.AppendLine("     ELSE 'Inactive' END AS StatusLabel");
                strdb.AppendLine(" FROM ltype l");
                strdb.AppendLine(" LEFT JOIN Inv i ON l.InvID = i.ID");
                strdb.AppendLine(" ORDER BY fdesc");
                return SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, strdb.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetActiveServiceType(string ConnectionString)
        {
            try
            {
                var strdb = new StringBuilder();

                strdb.AppendLine(" SELECT");
                strdb.AppendLine("     ISNULL(l.Reg, 0) AS RT,");
                strdb.AppendLine("     ISNULL(l.OT, 0) AS OT,");
                strdb.AppendLine("     ISNULL(l.NT, 0) AS NT,");
                strdb.AppendLine("     ISNULL(l.DT, 0) AS DT,");
                strdb.AppendLine("     l.type,");
                strdb.AppendLine("     l.fdesc,");
                strdb.AppendLine("     l.remarks,");
                strdb.AppendLine("     (SELECT COUNT(1) FROM elev WHERE cat = l.type) AS Count,");
                strdb.AppendLine("     l.InvID,");
                strdb.AppendLine("     ISNULL(i.Name, '') AS Name,");
                strdb.AppendLine("     ISNULL(l.Status,0) AS Status,");
                strdb.AppendLine("     CASE WHEN ISNULL(l.Status,0) = 0 THEN 'Active'");
                strdb.AppendLine("     ELSE 'Inactive' END AS StatusLabel");
                strdb.AppendLine(" FROM ltype l");
                strdb.AppendLine(" LEFT JOIN Inv i ON l.InvID = i.ID");
                strdb.AppendLine(" WHERE ISNULL(l.Status,0) != 1");
                strdb.AppendLine(" ORDER BY fdesc");
                return SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, strdb.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet GetActiveServiceType(GetActiveServiceTypeParam _GetActiveServiceType, string ConnectionString)
        {
            try
            {
                var strdb = new StringBuilder();

                strdb.AppendLine(" SELECT");
                strdb.AppendLine("     ISNULL(l.Reg, 0) AS RT,");
                strdb.AppendLine("     ISNULL(l.OT, 0) AS OT,");
                strdb.AppendLine("     ISNULL(l.NT, 0) AS NT,");
                strdb.AppendLine("     ISNULL(l.DT, 0) AS DT,");
                strdb.AppendLine("     l.type,");
                strdb.AppendLine("     l.fdesc,");
                strdb.AppendLine("     l.remarks,");
                strdb.AppendLine("     (SELECT COUNT(1) FROM elev WHERE cat = l.type) AS Count,");
                strdb.AppendLine("     l.InvID,");
                strdb.AppendLine("     ISNULL(i.Name, '') AS Name,");
                strdb.AppendLine("     ISNULL(l.Status,0) AS Status,");
                strdb.AppendLine("     CASE WHEN ISNULL(l.Status,0) = 0 THEN 'Active'");
                strdb.AppendLine("     ELSE 'Inactive' END AS StatusLabel");
                strdb.AppendLine(" FROM ltype l");
                strdb.AppendLine(" LEFT JOIN Inv i ON l.InvID = i.ID");
                strdb.AppendLine(" WHERE ISNULL(l.Status,0) != 1");
                strdb.AppendLine(" ORDER BY fdesc");
                return SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, strdb.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetActiveServiceTypeContract(string ConnectionString, string LocType, string EditSType, int department = -1, int route = -1)
        {
            

            try
            {
                var para = new SqlParameter[4];

                para[0] = new SqlParameter
                {
                    ParameterName = "@LocType",
                    SqlDbType = SqlDbType.VarChar,
                    Value = LocType
                };
                para[1] = new SqlParameter
                {
                    ParameterName = "@EditSType",
                    SqlDbType = SqlDbType.VarChar,
                    Value = EditSType
                };
                para[2] = new SqlParameter
                {
                    ParameterName = "@department",
                    SqlDbType = SqlDbType.Int,
                    Value = department
                };
                para[3] = new SqlParameter
                {
                    ParameterName = "@route",
                    SqlDbType = SqlDbType.Int,
                    Value = route
                };
                return SqlHelper.ExecuteDataset(ConnectionString, "spGetActiveServiceTypeContract", para);

            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

        //API
        public DataSet GetActiveServiceTypeContract(GetActiveServiceTypeContractParam _GetActiveServiceTypeContract, string ConnectionString, string LocType, string EditSType, int department = -1, int route = -1)
        {


            try
            {
                var para = new SqlParameter[4];

                para[0] = new SqlParameter
                {
                    ParameterName = "@LocType",
                    SqlDbType = SqlDbType.VarChar,
                    Value = LocType
                };
                para[1] = new SqlParameter
                {
                    ParameterName = "@EditSType",
                    SqlDbType = SqlDbType.VarChar,
                    Value = EditSType
                };
                para[2] = new SqlParameter
                {
                    ParameterName = "@department",
                    SqlDbType = SqlDbType.Int,
                    Value = department
                };
                para[3] = new SqlParameter
                {
                    ParameterName = "@route",
                    SqlDbType = SqlDbType.Int,
                    Value = route
                };
                return SqlHelper.ExecuteDataset(ConnectionString, "spGetActiveServiceTypeContract", para);

            }
            catch (Exception ex)
            {
                throw ex;
            }


        }
        public DataSet spGetProjectServiceTypeinfo(string ConnectionString, string ServiceType, int DepartmentID, string LocType, int RoutID)
        {
           
            try
            {
                var para = new SqlParameter[4];

                para[0] = new SqlParameter
                {
                    ParameterName = "@ServiceType",
                    SqlDbType = SqlDbType.VarChar,
                    Value = ServiceType
                };
                para[1] = new SqlParameter
                {
                    ParameterName = "@DepartmentID",
                    SqlDbType = SqlDbType.Int,
                    Value = DepartmentID
                };
                para[2] = new SqlParameter
                {
                    ParameterName = "@LocType",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = LocType
                };
                para[3] = new SqlParameter
                {
                    ParameterName = "@RoutID",
                    SqlDbType = SqlDbType.Int,
                    Value = RoutID
                };
                return SqlHelper.ExecuteDataset(ConnectionString, "spGetProjectServiceTypeinfo", para);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataSet spGetLocationServiceTypeinfo(string ConnectionString,string LocType, int RoutID , int LocationID)
        {

            try
            {
                var para = new SqlParameter[3]; 
             
                para[0] = new SqlParameter
                {
                    ParameterName = "@LocType",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = LocType
                };

                para[1] = new SqlParameter
                {
                    ParameterName = "@RoutID",
                    SqlDbType = SqlDbType.Int,
                    Value = RoutID
                };

                para[2] = new SqlParameter
                {
                    ParameterName = "@LocationID",
                    SqlDbType = SqlDbType.Int,
                    Value = LocationID
                };


                return SqlHelper.ExecuteDataset(ConnectionString, "spGetlocationServiceTypeinfo", para);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet spGetLocationServiceTypeinfo(spGetLocationServiceTypeinfoParam _GetLocationServiceTypeinfo, string ConnectionString)
        {

            try
            {
                var para = new SqlParameter[3];

                para[0] = new SqlParameter
                {
                    ParameterName = "@LocType",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = _GetLocationServiceTypeinfo.LocType
                };

                para[1] = new SqlParameter
                {
                    ParameterName = "@RoutID",
                    SqlDbType = SqlDbType.Int,
                    Value = _GetLocationServiceTypeinfo.RoutID
                };

                para[2] = new SqlParameter
                {
                    ParameterName = "@LocationID",
                    SqlDbType = SqlDbType.Int,
                    Value = _GetLocationServiceTypeinfo.LocationID
                };


                return SqlHelper.ExecuteDataset(ConnectionString, "spGetlocationServiceTypeinfo", para);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
