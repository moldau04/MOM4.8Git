using System;

namespace BusinessEntity.Projects
{
    public class ProjectListGridModel
    {
        
        public string ProjectID { get; set; }
        public string ProjectName { get; set; }
        public string CustomerName { get; set; }
        public string LocationName { get; set; }
        public string Status { get; set; }
        public DateTime DateCreated { get; set; }
        public string ServiceType { get; set; }
        public string TemplateType { get; set; }
        public string Department { get; set; }
        public string DefaultSalesPerson { get; set; }
        public string DefaultWorker { get; set; }
        public double ContractPrice { get; set; }
        public double NotBilledYet { get; set; }
        public double TotalBilled { get; set; }
        public double ActualHours { get; set; }
        public double LaborExpense { get; set; }
        public double MaterialExpense { get; set; }
        public double OtherExpense { get; set; }
        public double TotalExpense { get; set; }
        public double TotalPOOrder { get; set; }
        public double ReceivePO { get; set; }
        public double NetProfit { get; set; }
        public double perInProfit { get; set; }
        public string ProjectManager { get; set; }

    }


    public class ProjectListGridParam
    {


        public string SearchBy { get; set; }

        public string SearchValue { get; set; }

        public int Range { get; set; }

        public int JobType { get; set; }

        public string StartDate { get; set; }

        public string EndDate { get; set; }

        public int IncludeClose { get; set; }

        public int UserID { get; set; }

        public int EN { get; set; }

        public int IsSelesAsigned { get; set; }


    }
}
