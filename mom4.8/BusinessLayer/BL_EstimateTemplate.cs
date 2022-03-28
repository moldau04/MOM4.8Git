using System;
using DataLayer;
using System.Data;
using BusinessEntity;

namespace BusinessLayer
{

    public class BL_EstimateTemplate
    {
        DL_EstimateTemplate dlef = new DL_EstimateTemplate();

        public DataSet GetEstimateFormsByJobTId(EstimateTemplate et)
        {
            return dlef.GetEstimateTemplatesByJobTId(et);
        }

        public void GetEstimateTemplateById(EstimateTemplate et)
        {
            dlef.GetEstimateTemplateById(et);
            PopulateFields(et, et.ds.Tables[0].Rows[0]);

        }

        public void PopulateFields(EstimateTemplate et, DataRow dr)
        {
            et.JobTID = dr.Field<int>("JobTID");
            et.Name = dr.Field<string>("Name");
            et.FileName = dr.Field<string>("FileName");
            et.FilePath = dr.Field<string>("FilePath");
            et.MIME = dr.Field<string>("MIME");
            et.AddedBy = dr.Field<string>("AddedBy");
            et.AddedOn = dr.Field<DateTime>("AddedOn");
            et.UpdatedBy = dr.Field<string>("UpdatedBy");
            et.UpdatedOn = dr.Field<DateTime>("UpdatedOn");
        }

        public void AddEstimateTemplate(EstimateTemplate et)
        {
            dlef.AddEstimateTemplate(et);
        }
        public void DeleteEstimateTemplate(EstimateTemplate et)
        {
            dlef.DeleteEstimateTemplate(et);
        }
    }
}
