using BusinessEntity;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace DataLayer
{
    public class DL_Collection
    {
        public DataSet GetCollections(CollectionModel objCollectionModel)
        {
            try
            {
                SqlParameter paraDate = new SqlParameter();
                paraDate.ParameterName = "fDate";
                paraDate.SqlDbType = SqlDbType.DateTime;
                paraDate.Value = objCollectionModel.Date;

                SqlParameter paraCustomDay = new SqlParameter();
                paraCustomDay.ParameterName = "CustomDay";
                paraCustomDay.SqlDbType = SqlDbType.Int;
                paraCustomDay.Value = objCollectionModel.CustomDay;

                SqlParameter LocationIDs = new SqlParameter();
                LocationIDs.ParameterName = "LocationIDs";
                LocationIDs.SqlDbType = SqlDbType.VarChar;
                LocationIDs.Value = objCollectionModel.LocationIDs;

                SqlParameter CustomerIDs = new SqlParameter();
                CustomerIDs.ParameterName = "CustomerIDs";
                CustomerIDs.SqlDbType = SqlDbType.VarChar;
                CustomerIDs.Value = objCollectionModel.CustomerIDs;

                SqlParameter DepartmentIDs = new SqlParameter();
                DepartmentIDs.ParameterName = "DepartmentIDs";
                DepartmentIDs.SqlDbType = SqlDbType.VarChar;
                DepartmentIDs.Value = objCollectionModel.DepartmentIDs;

                SqlParameter EN = new SqlParameter();
                EN.ParameterName = "EN";
                EN.SqlDbType = SqlDbType.Int;
                EN.Value = objCollectionModel.EN;

                SqlParameter UserID = new SqlParameter();
                UserID.ParameterName = "UserID";
                UserID.SqlDbType = SqlDbType.Int;
                UserID.Value = objCollectionModel.UserID;

                SqlParameter PrintEmail = new SqlParameter();
                PrintEmail.ParameterName = "PrintEmail";
                PrintEmail.SqlDbType = SqlDbType.Int;
                PrintEmail.Value = objCollectionModel.PrintEmail;

                SqlParameter HidePartial = new SqlParameter();
                HidePartial.ParameterName = "HidePartial";
                HidePartial.SqlDbType = SqlDbType.Bit;
                HidePartial.Value = objCollectionModel.HidePartial;

                DataSet ds = new DataSet();
                if (objCollectionModel.isDBTotalService)
                {
                    ds = SqlHelper.ExecuteDataset(objCollectionModel.ConnConfig, "spGetCollectionCust", paraDate, paraCustomDay, LocationIDs, CustomerIDs, DepartmentIDs, EN, UserID, PrintEmail, HidePartial);
                }
                else
                {
                     ds = SqlHelper.ExecuteDataset(objCollectionModel.ConnConfig, "spGetCollectionCustNoneTS", paraDate, paraCustomDay, LocationIDs, CustomerIDs, DepartmentIDs, EN, UserID, PrintEmail);
                }
              
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet GetCollections(GetCollectionsParam _GetCollections, string ConnectionString)
        {
            try
            {
                SqlParameter paraDate = new SqlParameter();
                paraDate.ParameterName = "fDate";
                paraDate.SqlDbType = SqlDbType.DateTime;
                paraDate.Value = _GetCollections.Date;

                SqlParameter paraCustomDay = new SqlParameter();
                paraCustomDay.ParameterName = "CustomDay";
                paraCustomDay.SqlDbType = SqlDbType.Int;
                paraCustomDay.Value = _GetCollections.CustomDay;

                SqlParameter LocationIDs = new SqlParameter();
                LocationIDs.ParameterName = "LocationIDs";
                LocationIDs.SqlDbType = SqlDbType.VarChar;
                LocationIDs.Value = _GetCollections.LocationIDs;

                SqlParameter CustomerIDs = new SqlParameter();
                CustomerIDs.ParameterName = "CustomerIDs";
                CustomerIDs.SqlDbType = SqlDbType.VarChar;
                CustomerIDs.Value = _GetCollections.CustomerIDs;

                SqlParameter DepartmentIDs = new SqlParameter();
                DepartmentIDs.ParameterName = "DepartmentIDs";
                DepartmentIDs.SqlDbType = SqlDbType.VarChar;
                DepartmentIDs.Value = _GetCollections.DepartmentIDs;

                SqlParameter EN = new SqlParameter();
                EN.ParameterName = "EN";
                EN.SqlDbType = SqlDbType.Int;
                EN.Value = _GetCollections.EN;

                SqlParameter UserID = new SqlParameter();
                UserID.ParameterName = "UserID";
                UserID.SqlDbType = SqlDbType.Int;
                UserID.Value = _GetCollections.UserID;

                SqlParameter PrintEmail = new SqlParameter();
                PrintEmail.ParameterName = "PrintEmail";
                PrintEmail.SqlDbType = SqlDbType.Int;
                PrintEmail.Value = _GetCollections.PrintEmail;

                SqlParameter HidePartial = new SqlParameter();
                HidePartial.ParameterName = "HidePartial";
                HidePartial.SqlDbType = SqlDbType.Bit;
                HidePartial.Value = _GetCollections.HidePartial;

                DataSet ds = new DataSet();
                if (_GetCollections.isDBTotalService)
                {
                    ds = SqlHelper.ExecuteDataset(ConnectionString, "spGetCollectionCust", paraDate, paraCustomDay, LocationIDs, CustomerIDs, DepartmentIDs, EN, UserID, PrintEmail, HidePartial);
                }
                else
                {
                    ds = SqlHelper.ExecuteDataset(ConnectionString, "spGetCollectionCustNoneTS", paraDate, paraCustomDay, LocationIDs, CustomerIDs, DepartmentIDs, EN, UserID, PrintEmail);
                }

                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetCustomerList(CollectionModel objCollectionModel)
        {
            StringBuilder varCust = new StringBuilder();
            varCust.Append("select Distinct  O.ID, LTRIM(RTRIM(r.Name)) as Name, r.EN  \n");
            varCust.Append(" FROM  Owner O Inner Join Rol r on o.Rol=r.ID  \n");
            if (objCollectionModel.EN == 1)
            {
                varCust.Append("       LEFT OUTER JOIN tblUserCo UC on UC.CompanyID = r.EN  \n");
            }
            varCust.Append(" Where r.Type = 0 ");
            if (objCollectionModel.EN == 1)
            {
                varCust.Append(" and UC.IsSel = 1 and UC.UserID =" + objCollectionModel.UserID + "\n");
            }
            varCust.Append("  order by Name ASC");
            try
            {
                return SqlHelper.ExecuteDataset(objCollectionModel.ConnConfig, CommandType.Text, varCust.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetLocationList(CollectionModel objCollectionModel)
        {
            StringBuilder varLoc = new StringBuilder();
            varLoc.Append("select Distinct  l.Loc, l.Tag, LTRIM(RTRIM(r.Name)) as Name, r.EN   \n");
            varLoc.Append("from Loc l Inner Join Rol r on l.Rol=r.ID   \n");
            if (objCollectionModel.EN == 1)
            {
                varLoc.Append("       LEFT OUTER JOIN tblUserCo UC on UC.CompanyID = r.EN  \n");
            }
            varLoc.Append(" Where r.Type = 4 and l.Owner = " + objCollectionModel.OwnerID + "\n");
            if (objCollectionModel.EN == 1)
            {
                varLoc.Append(" and UC.IsSel = 1 and UC.UserID =" + objCollectionModel.UserID + "\n");
            }
            varLoc.Append("  order by Name ASC");
            try
            {
                return SqlHelper.ExecuteDataset(objCollectionModel.ConnConfig, CommandType.Text, varLoc.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetCustomerListByOwner(CollectionModel objCollectionModel)
        {
            StringBuilder varCust = new StringBuilder();
            varCust.Append("select Distinct  O.ID, LTRIM(RTRIM(r.Name)) as Name, r.EN  \n");
            varCust.Append(" FROM  Owner O Inner Join Rol r on o.Rol=r.ID  \n");
            if (objCollectionModel.EN == 1)
            {
                varCust.Append("       LEFT OUTER JOIN tblUserCo UC on UC.CompanyID = r.EN  \n");
            }
            varCust.Append(" Where r.Type = 0 and O.ID = " + objCollectionModel.OwnerID + "\n");
            if (objCollectionModel.EN == 1)
            {
                varCust.Append(" and UC.IsSel = 1 and UC.UserID =" + objCollectionModel.UserID + "\n");
            }
            varCust.Append("  order by Name ASC");
            try
            {
                return SqlHelper.ExecuteDataset(objCollectionModel.ConnConfig, CommandType.Text, varCust.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetCustomerLocationIDs(CollectionModel objCollectionModel)
        {
            try
            {
              
                SqlParameter CustomerName = new SqlParameter();
                CustomerName.ParameterName = "CustomerName";
                CustomerName.SqlDbType = SqlDbType.VarChar;
                CustomerName.Value = objCollectionModel.CustomerName;

                SqlParameter LocationName = new SqlParameter();
                LocationName.ParameterName = "CustomerIDs";
                LocationName.SqlDbType = SqlDbType.VarChar;
                LocationName.Value = objCollectionModel.LocationName;

                DataSet ds = new DataSet();
                ds = SqlHelper.ExecuteDataset(objCollectionModel.ConnConfig, "GetCustomerLocationIDs", CustomerName, LocationName);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetCollectionsSummary(CollectionModel objCollectionModel)
        {
            try
            {
                SqlParameter paraDate = new SqlParameter();
                paraDate.ParameterName = "fDate";
                paraDate.SqlDbType = SqlDbType.DateTime;
                paraDate.Value = objCollectionModel.Date;

                SqlParameter paraCustomDay = new SqlParameter();
                paraCustomDay.ParameterName = "CustomDay";
                paraCustomDay.SqlDbType = SqlDbType.Int;
                paraCustomDay.Value = objCollectionModel.CustomDay;

                SqlParameter LocationIDs = new SqlParameter();
                LocationIDs.ParameterName = "LocationIDs";
                LocationIDs.SqlDbType = SqlDbType.VarChar;
                LocationIDs.Value = objCollectionModel.LocationIDs;

                SqlParameter CustomerIDs = new SqlParameter();
                CustomerIDs.ParameterName = "CustomerIDs";
                CustomerIDs.SqlDbType = SqlDbType.VarChar;
                CustomerIDs.Value = objCollectionModel.CustomerIDs;

                SqlParameter DepartmentIDs = new SqlParameter();
                DepartmentIDs.ParameterName = "DepartmentIDs";
                DepartmentIDs.SqlDbType = SqlDbType.VarChar;
                DepartmentIDs.Value = objCollectionModel.DepartmentIDs;

                SqlParameter EN = new SqlParameter();
                EN.ParameterName = "EN";
                EN.SqlDbType = SqlDbType.Int;
                EN.Value = objCollectionModel.EN;

                SqlParameter UserID = new SqlParameter();
                UserID.ParameterName = "UserID";
                UserID.SqlDbType = SqlDbType.Int;
                UserID.Value = objCollectionModel.UserID;

                DataSet ds = new DataSet();
                ds = SqlHelper.ExecuteDataset(objCollectionModel.ConnConfig, "spGetCollectionSum", paraDate, paraCustomDay, LocationIDs, CustomerIDs, DepartmentIDs, EN, UserID);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        //API
        public List<CollectionResponseModel> GetCollectionsSummary(GetDashboardCollectionParam objCollectionModel, string connectionString)
        {
            try
            {
                List<CollectionResponseModel> _lstCollectionResponseModel = new List<CollectionResponseModel>();
                SqlParameter paraDate = new SqlParameter();
                paraDate.ParameterName = "fDate";
                paraDate.SqlDbType = SqlDbType.DateTime;
                paraDate.Value = objCollectionModel.Date;

                SqlParameter paraCustomDay = new SqlParameter();
                paraCustomDay.ParameterName = "CustomDay";
                paraCustomDay.SqlDbType = SqlDbType.Int;
                paraCustomDay.Value = objCollectionModel.CustomDay;

                SqlParameter LocationIDs = new SqlParameter();
                LocationIDs.ParameterName = "LocationIDs";
                LocationIDs.SqlDbType = SqlDbType.VarChar;
                LocationIDs.Value = objCollectionModel.LocationIDs;

                SqlParameter CustomerIDs = new SqlParameter();
                CustomerIDs.ParameterName = "CustomerIDs";
                CustomerIDs.SqlDbType = SqlDbType.VarChar;
                CustomerIDs.Value = objCollectionModel.CustomerIDs;

                SqlParameter DepartmentIDs = new SqlParameter();
                DepartmentIDs.ParameterName = "DepartmentIDs";
                DepartmentIDs.SqlDbType = SqlDbType.VarChar;
                DepartmentIDs.Value = objCollectionModel.DepartmentIDs;

                SqlParameter EN = new SqlParameter();
                EN.ParameterName = "EN";
                EN.SqlDbType = SqlDbType.Int;
                EN.Value = objCollectionModel.EN;

                SqlParameter UserID = new SqlParameter();
                UserID.ParameterName = "UserID";
                UserID.SqlDbType = SqlDbType.Int;
                UserID.Value = objCollectionModel.UserID;

                DataSet ds = new DataSet();
                ds = SqlHelper.ExecuteDataset(objCollectionModel.ConnConfig, "spGetCollectionSum", paraDate, paraCustomDay, LocationIDs, CustomerIDs, DepartmentIDs, EN, UserID);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    _lstCollectionResponseModel.Add(new CollectionResponseModel()
                    {  
                        NinetyDay = Convert.ToString(dr["NinetyDay"].ToString()),
                        OneTwentyDay = Convert.ToString(dr["OneTwentyDay"].ToString()),
                        SixtyDay = Convert.ToString(dr["SixtyDay"].ToString()),
                        ThirtyDay = Convert.ToString(dr["ThirtyDay"].ToString()),

                    });
                }
                return _lstCollectionResponseModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetCollectionCustomerNotes(string connectionString, DateTime fdate)
        {
            try
            {
                SqlParameter paraDate = new SqlParameter();
                paraDate.ParameterName = "fDate";
                paraDate.SqlDbType = SqlDbType.DateTime;
                paraDate.Value = fdate;

                DataSet ds = new DataSet();
                ds = SqlHelper.ExecuteDataset(connectionString, "spGetCollectionCustomerNotes", paraDate);           
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetCollectionLocationNotes(string connectionString, DateTime fdate)
        {
            try
            {
                SqlParameter paraDate = new SqlParameter();
                paraDate.ParameterName = "fDate";
                paraDate.SqlDbType = SqlDbType.DateTime;
                paraDate.Value = fdate;

                DataSet ds = new DataSet();
                ds = SqlHelper.ExecuteDataset(connectionString, "spGetCollectionLocationNotes", paraDate);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
