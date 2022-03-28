using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.Recurring
{
    [Serializable]
    public class GetContractViewModel
    {
        public int Loc { get; set; }
        public int Owner { get; set; }
        public string Custom20 { get; set; }
        public Int16 Status { get; set; }
        public DateTime BStart { get; set; }
        public Int16 BCycle { get; set; }
        public double BAmt { get; set; }
        public DateTime SStart { get; set; }
        public Int16 sCycle { get; set; }
        public Int16 SDate { get; set; }
        public Int16 SDay { get; set; }
        public DateTime STime { get; set; }
        public Int16 CreditCard { get; set; }
        public string Remarks { get; set; }
        public string locname { get; set; }
        public Int16 credit { get; set; }
        public Int16 swe { get; set; }
        public double hours { get; set; }
        public string ctype { get; set; }
        public string fdesc { get; set; }
        public int id { get; set; }
        public DateTime ExpirationDate { get; set; }
        public Int16 Expiration { get; set; }
        public Int16 frequencies { get; set; }
        public Int16 Billing { get; set; }
        public Int16 CustBilling { get; set; }
        public int Central { get; set; }
        public int Chart { get; set; }
        public string GLAcct { get; set; }
        public Int16 BEscType { get; set; }
        public Int16 BEscCycle { get; set; }
        public double BEscFact { get; set; }
        public DateTime EscLast { get; set; }
        public double BillRate { get; set; }
        public double RateOT { get; set; }
        public double RateNT { get; set; }
        public double RateMileage { get; set; }
        public double RateDT { get; set; }
        public double RateTravel { get; set; }
        public string PO { get; set; }
        public Int16 SPHandle { get; set; }
        public string SRemarks { get; set; }
        public Int16 IsRenewalNotes { get; set; }
        public string RenewalNotes { get; set; }
        public Int16 Detail { get; set; }
        public Int16 DepartmentID { get; set; }
        public string TaskCategory { get; set; }
        public int Route { get; set; }
    }
}
