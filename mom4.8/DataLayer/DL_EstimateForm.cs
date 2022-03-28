using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Data;
using BusinessEntity;
using System.Data.SqlClient;

namespace DataLayer
{
    public class DL_EstimateForm
    {
        public void GetEstimateFormById(EstimateForm ef)
        {
            try
            {
                string sql = "Select ID, Estimate, JobTID, Name, FileName, FilePath, PdfFilePath, Remark, MIME, AddedBy, AddedOn from EstimateForm where ID = @ID;";
                var para = new List<SqlParameter>();
                para.Add(new SqlParameter("@ID", ef.Id));
                ef.ds = SqlHelper.ExecuteDataset(ef.ConnConfig, CommandType.Text, sql, para.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetEstimateFormsByEstimateId(EstimateForm ef)
        {
            try
            {
                string sql = "Select ID, Estimate, JobTID, Name, FileName, FilePath, PdfFilePath, Remark, MIME, AddedBy, AddedOn,SendFrom,SendTo,SendOn " +
                    "from EstimateForm where Estimate = @Estimate Order By AddedOn desc, ID desc;";
                var para = new List<SqlParameter>();
                para.Add(new SqlParameter("@Estimate", ef.Estimate));
                return SqlHelper.ExecuteDataset(ef.ConnConfig, CommandType.Text, sql, para.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetEstimateLastProposalByEstimateId(EstimateForm ef)
        {
            try
            {
                string sql = "Select TOP 1 ID, JobTID, Estimate, FileName, FilePath, PdfFilePath, AddedOn from EstimateForm where Estimate = @Estimate ORDER BY AddedOn desc, ID desc;";
                var para = new List<SqlParameter>();
                para.Add(new SqlParameter("@Estimate", ef.Estimate));
                return SqlHelper.ExecuteDataset(ef.ConnConfig, CommandType.Text, sql, para.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddEstimateForm(EstimateForm ef)
        {
            try
            {
                string sql = "spAddEstimateForm";

                var para = new List<SqlParameter>();

                para.Add(new SqlParameter("@ID", ef.Id));
                para.Add(new SqlParameter("@Estimate", ef.Estimate));
                para.Add(new SqlParameter("@JobTID", ef.JobTID));
                para.Add(new SqlParameter("@Name", ef.Name));
                para.Add(new SqlParameter("@FileName", ef.FileName));
                para.Add(new SqlParameter("@FilePath", ef.FilePath));
                para.Add(new SqlParameter("@PdfFilePath", ef.PdfFilePath));
                para.Add(new SqlParameter("@Remark", ef.Remark));
                para.Add(new SqlParameter("@MIME", ef.MIME));
                para.Add(new SqlParameter("@user", ef.UpdatedBy));
                para.Add(new SqlParameter("@UpdatedDate", ef.UpdatedOn));
                ef.Id = SqlHelper.ExecuteNonQuery(ef.ConnConfig, CommandType.StoredProcedure, sql, para.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void DeleteEstimateForm(EstimateForm ef)
        {
            try
            {
                string sql = "Delete from EstimateForm where ID = @ID";
                var para = new List<SqlParameter>();
                para.Add(new SqlParameter("@ID", ef.Id));
                ef.Id = SqlHelper.ExecuteNonQuery(ef.ConnConfig, CommandType.Text, sql, para.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void UpdateEstimateForm(EstimateForm ef, String sendTO, String sendFrom, string sendBy)
        {
            try
            {
                string sql = "UPDATE EstimateForm SET SendTo=@SendTo, SendFrom=@SendFrom,SendOn=GETDATE(),SendBy=@SendBy Where ID=@ID";
                var para = new List<SqlParameter>();
                para.Add(new SqlParameter("@ID", ef.Id));
                para.Add(new SqlParameter("@SendTO", sendTO));
                para.Add(new SqlParameter("@SendFrom", sendFrom));
                para.Add(new SqlParameter("@SendBy", sendBy));
                ef.Id = SqlHelper.ExecuteNonQuery(ef.ConnConfig, CommandType.Text, sql, para.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
