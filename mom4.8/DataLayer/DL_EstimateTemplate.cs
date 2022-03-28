using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Data;
using BusinessEntity;
using System.Data.SqlClient;

namespace DataLayer
{
    public class DL_EstimateTemplate
    {
        public void GetEstimateTemplateById(EstimateTemplate ef)
        {
            try
            {
                string sql = "Select ID, JobTID, Name, FileName, FilePath, MIME, AddedBy, AddedOn, UpdatedBy, UpdatedOn from EstimateTemplate where ID = @ID;";
                var para = new List<SqlParameter>();
                para.Add(new SqlParameter("@ID", ef.Id));
                ef.ds = SqlHelper.ExecuteDataset(ef.ConnConfig, CommandType.Text, sql, para.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetEstimateTemplatesByJobTId(EstimateTemplate ef)
        {
            try
            {
                string sql = "Select ID, JobTID, Name, FileName, FilePath, MIME, AddedBy, AddedOn, UpdatedBy, UpdatedOn from EstimateTemplate where JobTID = @JobTID;";
                var para = new List<SqlParameter>();
                para.Add(new SqlParameter("@JobTID", ef.JobTID));
                return SqlHelper.ExecuteDataset(ef.ConnConfig, CommandType.Text, sql, para.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddEstimateTemplate(EstimateTemplate ef)
        {
            try
            {
                string sql = "spAddEstimateTemplateForm";

                var para = new List<SqlParameter>();

                para.Add(new SqlParameter("@ID", ef.Id));
                para.Add(new SqlParameter("@JobTID", ef.JobTID));
                para.Add(new SqlParameter("@Name", ef.Name));
                para.Add(new SqlParameter("@FileName", ef.FileName));
                para.Add(new SqlParameter("@FilePath", ef.FilePath));
                para.Add(new SqlParameter("@MIME", ef.MIME));
                para.Add(new SqlParameter("@user", ef.UpdatedBy));

                ef.Id = SqlHelper.ExecuteNonQuery(ef.ConnConfig, CommandType.StoredProcedure, sql, para.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void DeleteEstimateTemplate(EstimateTemplate ef)
        {
            try
            {
                string sql = "Delete from EstimateTemplate where ID = @ID";
                var para = new List<SqlParameter>();
                para.Add(new SqlParameter("@ID", ef.Id));
                ef.Id = SqlHelper.ExecuteNonQuery(ef.ConnConfig, CommandType.Text, sql, para.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
