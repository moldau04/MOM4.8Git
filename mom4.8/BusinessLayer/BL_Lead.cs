using BusinessEntity;
using DataLayer;
using System;
using System.Data;

namespace BusinessLayer
{
    public class BL_Lead
    {
        DL_Lead objLead = new DL_Lead();
        //public DataSet GetAllStage(Lead info)
        //{
        //    return objLead.GetAllStage(info);
        //}

        //public DataSet GetLeadByEstimateID(Lead info)
        //{
        //    return objLead.GetLeadByEstimateID(info);
        //}

        //public DataSet GetLeadByID(Lead info)
        //{
        //    return objLead.GetLeadByID(info);
        //}

        public DataSet GetStageByID(Lead info)
        {
            return objLead.GetStageByID(info);
        }

        public DataSet GetSalespersonByID(Lead info)
        {
            return objLead.GetSalespersonByID(info);
        }

        //public Int32 AddOpportunity(Lead info)
        //{
        //    return objLead.AddOpportunity(info);
        //}

        //public Int32 UpdateOpportunity(Lead info)
        //{
        //    return objLead.UpdateOpportunity(info);
        //}

        //public Int32 UpdateOpportunityAmount(Lead info)
        //{
        //    return objLead.UpdateOpportunityAmount(info);
        //}

        public Int32 DeleteTaskByID(Lead info)
        {
            return objLead.DeleteTaskByID(info);
        }
        
    }
}
