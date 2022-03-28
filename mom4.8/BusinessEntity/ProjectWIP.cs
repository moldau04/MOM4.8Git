using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity
{
    public class ProjectWIP
    {
        public string ConnConfig { get; set; }

        public int WIPID { get; set; }

        public int Job { get; set; }

        public string Type { get; set; }

        public int Department { get; set; }

        public string fDesc { get; set; }

        public string Tag { get; set; }

        public string Status { get; set; }

        public double? ContractPrice { get; set; }

        public double? ConstModAdjmts { get; set; }

        public double? AccountingAdjmts { get; set; }

        public double? TotalBudgetedExpense { get; set; }

        public double? TotalEstimatedCost { get; set; }

        public double? EstimatedProfit { get; set; }

        public double? ContractCosts { get; set; }

        public double? CostToComplete { get; set; }

        public double? PercentageComplete { get; set; }

        public double? RevenuesEarned { get; set; }

        public double? GrossProfit { get; set; }

        public double? BilledToDate { get; set; }

        public double? ToBeBilled { get; set; }

        public double? OpenARAmount { get; set; }

        public double? RetainageBilling { get; set; }

        public bool IsUpdateRetainage { get; set; }

        public double? TotalBilling { get; set; }

        public double? Billings { get; set; }

        public double? Earnings { get; set; }

        public double? NPer { get; set; }

        public double? NPerLastMonth { get; set; }

        public double? NPerLastYear { get; set; }

        public double? NPerLastMonthYear { get; set; }

        public double? BillingContract { get; set; }

        public double? JobBorrow { get; set; }

        public DateTime? fDate { get; set; }

        public DateTime? CloseDate { get; set; }

        public DateTime? ExpectedClosingDate { get; set; }
    }
}
