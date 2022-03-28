using BusinessEntity;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DataLayer
{
    public class DL_Lead
    {
        //public DataSet GetAllStage(Lead info)
        //{
        //    try
        //    {
        //        string sql = "select * from Stage";
        //        return SqlHelper.ExecuteDataset(info.ConnConfig, CommandType.Text, sql);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public DataSet GetLeadByEstimateID(Lead info)
        //{
        //    try
        //    {
        //        string sql = "select * from Lead WHERE EstimateID=" + info.EstimateID;
        //        return SqlHelper.ExecuteDataset(info.ConnConfig, CommandType.Text, sql);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public DataSet GetLeadByID(Lead info)
        //{
        //    try
        //    {
        //        string sql = "select * from Lead WHERE ID=" + info.ID;
        //        return SqlHelper.ExecuteDataset(info.ConnConfig, CommandType.Text, sql);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public DataSet GetStageByID(Lead info)
        {
            try
            {
                string sql = "select * from Stage WHERE ID=" + info.ID;
                return SqlHelper.ExecuteDataset(info.ConnConfig, CommandType.Text, sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetSalespersonByID(Lead info)
        {
            try
            {
                var para = new List<SqlParameter>();
                para.Add(new SqlParameter("@ID", info.ID));
                // string sql = "select u.fUser, e.fFirst,e.[Last] AS lLast from tblUser u left outer join emp e on u.fUser = e.CallSign where u.ID = " + info.ID;
                //string sql = "select * from Terr Where ID= " + info.ID;
                return SqlHelper.ExecuteDataset(info.ConnConfig, CommandType.StoredProcedure, "spGetSalespersonByID", para.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public Int32 AddOpportunity(Lead info)
        //{
        //    Int32 _id = 0;
        //    try
        //    {
        //        string sql = "spAddLeadByEstimate";

        //        var para = new List<SqlParameter>();

        //        para.Add(new SqlParameter("@OpportunityName", info.OpportunityName));
        //        para.Add(new SqlParameter("@OpportunityStageID", info.OpportunityStageID));
        //        para.Add(new SqlParameter("@Rol", info.Rol));
        //        para.Add(new SqlParameter("@AssignedToID", info.AssignedToID));
        //        para.Add(new SqlParameter("@CloseDate", info.closedate));
        //        para.Add(new SqlParameter("@fDesc", info.Desc));
        //        para.Add(new SqlParameter("@Status", info.Status));
        //        para.Add(new SqlParameter("@CompanyName", info.CompanyName));
        //        para.Add(new SqlParameter("@LocationName ", info.LocationName));
        //        para.Add(new SqlParameter("@Amount", info.Amount));
        //        para.Add(new SqlParameter("@UpdateUser", info.UpdateUser));

        //        _id = Convert.ToInt32(SqlHelper.ExecuteScalar(info.ConnConfig, CommandType.StoredProcedure, sql, para.ToArray()));
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //    return _id;
        //}

        //public Int32 UpdateOpportunity(Lead info)
        //{
        //    Int32 _id = 0;
        //    try
        //    {
        //        string sql = "spUpdateLeadByEstimate";

        //        var para = new List<SqlParameter>();
        //        para.Add(new SqlParameter("@ID", info.ID));
        //        para.Add(new SqlParameter("@OpportunityName", info.OpportunityName));
        //        para.Add(new SqlParameter("@OpportunityStageID", info.OpportunityStageID));
        //        para.Add(new SqlParameter("@Rol", info.Rol));
        //        para.Add(new SqlParameter("@AssignedToID", info.AssignedToID));
        //        para.Add(new SqlParameter("@CloseDate", info.closedate));
        //        para.Add(new SqlParameter("@fDesc", info.Desc));
        //        para.Add(new SqlParameter("@Status", info.Status));
        //        para.Add(new SqlParameter("@CompanyName", info.CompanyName));
        //        para.Add(new SqlParameter("@LocationName ", info.LocationName));
        //        para.Add(new SqlParameter("@Amount", info.Amount));
        //        para.Add(new SqlParameter("@UpdateUser", info.UpdateUser));
        //        para.Add(new SqlParameter("@EstimateID", info.EstimateID));
        //        _id = SqlHelper.ExecuteNonQuery(info.ConnConfig, CommandType.StoredProcedure, sql, para.ToArray());
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //    return _id;
        //}

        //public Int32 UpdateOpportunityAmount(Lead info)
        //{
        //    Int32 _id = 0;
        //    try
        //    {
        //        string sql = "spUpdateLeadRevenueByEstimate";

        //        var para = new List<SqlParameter>();
        //        para.Add(new SqlParameter("@ID", info.ID));
        //        para.Add(new SqlParameter("@Amount", info.Amount));
        //        para.Add(new SqlParameter("@UpdateUser", info.UpdateUser));
        //        para.Add(new SqlParameter("@EstimateID", info.EstimateID));
        //        para.Add(new SqlParameter("@OpportunityStageID", info.OpportunityStageID));
        //        _id = SqlHelper.ExecuteNonQuery(info.ConnConfig, CommandType.StoredProcedure, sql, para.ToArray());
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //    return _id;
        //}


        public Int32 DeleteTaskByID(Lead info)
        {
            Int32 _id = 0;
            try
            {
                string sql = "delete from ToDo where ID=" + info.ID;
                _id = SqlHelper.ExecuteNonQuery(info.ConnConfig, CommandType.Text, sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return _id;
        }

    }
}
