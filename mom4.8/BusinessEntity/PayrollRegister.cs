using System;
using System.Data;

namespace BusinessEntity
{
    public class PayrollRegisterModel
    {
        public int? PayrollRegisterId { get; set; }
        public string PayrollData { get; set; }
        public int? FrequencyId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int? SupervisorId { get; set; }
        public int? DeparptmentId { get; set; }
        public bool ProcessOtherDeduction { get; set; }
        public string Description { get; set; }
        public string ProcessMethod { get; set; }
        public string ConnConfig { get; set; }
        public int? Status { get; set; }
    }
}
