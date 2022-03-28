using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.CustomersModel
{
    [Serializable]
    public class GetOpportunityOfCustomerViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string fDesc { get; set; }
        public Int16 RolType { get; set; }
        public int EN { get; set; }
        public string Company { get; set; }
        public string Status { get; set; }
        public string Probability { get; set; }
        public string Product { get; set; }
        public double Profit { get; set; }
        public DateTime CreateDate { get; set; }
        public int Rol { get; set; }
        public DateTime closedate { get; set; }
        public string Remarks { get; set; }
        public string closed { get; set; }
        public double revenue { get; set; }
        public string fuser { get; set; }
        public string CompanyName { get; set; }
        public string defsales { get; set; }
        public int DocumentCount { get; set; }
        public string Estimate { get; set; }
        public string Referral { get; set; }
        public string Stage { get; set; }
        public string job { get; set; }
        public string fFor { get; set; }
        public string EstimateDiscounted { get; set; }
        public double BidPrice { get; set; }
        public double FinalBid { get; set; }
        public string Dept { get; set; }
    }
}
