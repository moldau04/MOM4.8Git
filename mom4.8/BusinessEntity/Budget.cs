namespace BusinessEntity
{
    public class Budget
    {

        public string BudgetValue { get; set; }

        public string ConnConfig { get; set; }

        public int BudgetId { get; set; }

        public int Year { get; set; }
    }

    public class BudgetData
    {
        public string BudgetValue { get; set; }

        public int BudgetId { get; set; }

        public int AccountId { get; set; }

        public int AccountDetailId { get; set; }

        public int Period { get; set; }

        public long Credit { get; set; }

        public long Debit { get; set; }

        public long Amount { get; set; }

        public string Acct { get; set; }
        public string fDesc { get; set; }
        public string Balance { get; set; }
        public string Type { get; set; }
        public string Sub { get; set; }
        public string Remarks { get; set; }
        public string Control { get; set; }
        public string InUse { get; set; }
        public string Detail { get; set; }
        public string CAlias { get; set; }
        public string Status { get; set; }
        public string Sub2 { get; set; }
        public string DAT { get; set; }
        public string Branch { get; set; }
        public string CostCenter { get; set; }
        public string AccRoot { get; set; }
    }

    public class BudgetColumns
    {
        public static readonly string[] columns = { "Credit", "Debit", "Amount", "Acct", "Balance" };


    }
}
