using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity
{
    public class AccountDetail
    {
        public string ConnConfig { get; set; }
        public int AccountId { get; set; }

        public int AccountDetailId { get; set; }
        public int BudgetAccountDetailId { get; set; }
        public int BudgetId { get; set; }

        public int Period { get; set; }

        public double Credit { get; set; }

        public double Debit { get; set; }

        public double Amount { get; set; }

        public double Total { get; set; }

        public double Jan { get; set; }

        public double Feb { get; set; }
        public double Mar { get; set; }

        public double Apr { get; set; }

        public double May { get; set; }
        public double Jun { get; set; }

        public double Jul { get; set; }

        public double Aug { get; set; }
        public double Sep { get; set; }

        public double Oct { get; set; }

        public double Nov { get; set; }
        public double Dec { get; set; }

    }
}
