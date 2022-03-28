using System;
using System.Data;

namespace BusinessEntity
{
    public class Lead
    {
        public int ID { get; set; }

        public int EstimateID { get; set; }

        public string OpportunityName { get; set; }

        public int OpportunityStageID { get; set; }

        public int Rol { get; set; }

        public string CompanyName { get; set; }
        public string LocationName { get; set; }

        public string Desc { get; set; }
        public int Status { get; set; }

        public DateTime closedate { get; set; }

        public int AssignedToID { get; set; }
        public decimal Amount { get; set; }
        public string ConnConfig { get; set; }

        public string UpdateUser { get; set; }

        public DataSet ds { get; set; }
    }
}
