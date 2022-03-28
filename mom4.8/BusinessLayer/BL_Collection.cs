using BusinessEntity;
using BusinessEntity.CustomersModel;
using DataLayer;
using System;
using System.Collections.Generic;
using System.Data;

namespace BusinessLayer
{
    public class BL_Collection
    {
        DL_Collection objDL_Collection = new DL_Collection();

        public DataSet GetCollections(CollectionModel objCollectionModel)
        {
            return objDL_Collection.GetCollections(objCollectionModel);
        }

        //API
        public List<GetCollectionsViewModel> GetCollections(GetCollectionsParam _GetCollections, string ConnectionString)
        {
            DataSet ds = objDL_Collection.GetCollections(_GetCollections, ConnectionString);

            List<GetCollectionsViewModel> _lstGetCollections = new List<GetCollectionsViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetCollections.Add(
                    new GetCollectionsViewModel()
                    {
                        TransID = Convert.ToInt32(DBNull.Value.Equals(dr["TransID"]) ? 0 : dr["TransID"]),
                        Department = Convert.ToString(dr["Department"]),
                        Type = Convert.ToInt32(DBNull.Value.Equals(dr["Type"]) ? 0 : dr["Type"]),
                        cid = Convert.ToString(dr["cid"]),
                        Owner = Convert.ToInt32(DBNull.Value.Equals(dr["Owner"]) ? 0 : dr["Owner"]),
                        Loc = Convert.ToInt32(DBNull.Value.Equals(dr["Loc"]) ? 0 : dr["Loc"]),
                        credit = Convert.ToInt32(DBNull.Value.Equals(dr["credit"]) ? 0 : dr["credit"]),
                        CustomerName = Convert.ToString(dr["CustomerName"]),
                        LocID = Convert.ToString(dr["LocID"]),
                        LocName = Convert.ToString(dr["LocName"]),
                        LocIID = Convert.ToString(dr["LocIID"]),
                        fDate = Convert.ToDateTime(DBNull.Value.Equals(dr["fDate"]) ? null : dr["fDate"]),
                        Due = Convert.ToDateTime(DBNull.Value.Equals(dr["Due"]) ? null : dr["Due"]),
                        Original = Convert.ToDouble(DBNull.Value.Equals(dr["Original"]) ? 0 : dr["Original"]),
                        Total = Convert.ToDouble(DBNull.Value.Equals(dr["Total"]) ? 0 : dr["Total"]),
                        Paid = Convert.ToDouble(DBNull.Value.Equals(dr["Paid"]) ? 0 : dr["Paid"]),
                        fDesc = Convert.ToString(dr["fDesc"]),
                        Ref = Convert.ToInt32(DBNull.Value.Equals(dr["Ref"]) ? 0 : dr["Ref"]),
                        RefURL = Convert.ToString(dr["RefURL"]),
                        DueIn = Convert.ToInt32(DBNull.Value.Equals(dr["DueIn"]) ? 0 : dr["DueIn"]),
                        CurrentDay = Convert.ToDouble(DBNull.Value.Equals(dr["CurrentDay"]) ? 0 : dr["CurrentDay"]),
                        CurrSevenDay = Convert.ToDouble(DBNull.Value.Equals(dr["CurrSevenDay"]) ? 0 : dr["CurrSevenDay"]),
                        ThirtyDay = Convert.ToDouble(DBNull.Value.Equals(dr["ThirtyDay"]) ? 0 : dr["ThirtyDay"]),
                        SixtyDay = Convert.ToDouble(DBNull.Value.Equals(dr["SixtyDay"]) ? 0 : dr["SixtyDay"]),
                        NintyDay = Convert.ToDouble(DBNull.Value.Equals(dr["NintyDay"]) ? 0 : dr["NintyDay"]),
                        NintyOneDay = Convert.ToDouble(DBNull.Value.Equals(dr["NintyOneDay"]) ? 0 : dr["NintyOneDay"]),
                        OneTwentyDay = Convert.ToDouble(DBNull.Value.Equals(dr["OneTwentyDay"]) ? 0 : dr["OneTwentyDay"]),
                        sel = Convert.ToInt32(DBNull.Value.Equals(dr["sel"]) ? 0 : dr["sel"]),
                        Status = Convert.ToString(dr["Status"]),
                    }
                    );
            }

            return _lstGetCollections;
        }
        public DataSet GetCustomerList(CollectionModel objCollectionModel)
        {
            return objDL_Collection.GetCustomerList(objCollectionModel);
        }
        public DataSet GetLocationList(CollectionModel objCollectionModel)
        {
            return objDL_Collection.GetLocationList(objCollectionModel);
        }
        public DataSet GetCustomerListByOwner(CollectionModel objCollectionModel)
        {
            return objDL_Collection.GetCustomerListByOwner(objCollectionModel);
        }

        public DataSet GetCustomerLocationIDs(CollectionModel objCollectionModel)
        {
            return objDL_Collection.GetCustomerLocationIDs(objCollectionModel);
        }
        public DataSet GetCollectionsSummary(CollectionModel objCollectionModel)
        {
            return objDL_Collection.GetCollectionsSummary(objCollectionModel);
        }

        //API
        public List<CollectionResponseModel> GetCollectionsSummary(GetDashboardCollectionParam _GetDashboardCollectionParam,string ConnectionString)
        {
            return objDL_Collection.GetCollectionsSummary(_GetDashboardCollectionParam, ConnectionString);
        }

        public DataSet GetCollectionCustomerNote(String conn, DateTime fdate)
        {
            return objDL_Collection.GetCollectionCustomerNotes(conn, fdate);
        }

        public DataSet GetCollectionLocationNote(String conn,DateTime fdate)
        {
            return objDL_Collection.GetCollectionLocationNotes(conn,fdate);
        }

    }
}
