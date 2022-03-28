using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.CommonModel
{
    [Serializable]
    public class APIIntegrationModel
    {
        public int ID { get; set; }
        public string ModelName { get; set; }
        public Int16 Integration { get; set; }
        public DateTime UpdateOn { get; set; }
        public bool IsAPIIntegrationForDashBoardModule { get; set; }
        public bool IsAPIIntegrationForCustomersModule { get; set; }
        public bool IsAPIIntegrationForRecurringModule { get; set; }
        public bool IsAPIIntegrationForScheduleModule { get; set; }
        public bool IsAPIIntegrationForBillingModule { get; set; }
        public bool IsAPIIntegrationForAPModule { get; set; }
        public bool IsAPIIntegrationForPurchasingModule { get; set; }
        public bool IsAPIIntegrationForSalesModule { get; set; }
        public bool IsAPIIntegrationForProjectsModule { get; set; }
        public bool IsAPIIntegrationForInventoryModule { get; set; }
        public bool IsAPIIntegrationForFinancialsModule { get; set; }
        public bool IsAPIIntegrationForStatementsModule { get; set; }
        public bool IsAPIIntegrationForProgramsModule { get; set; }
    }
}
