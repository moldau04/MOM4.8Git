using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.CommonModel
{
    [Serializable]
    public class ListGetUserPermissionByUserID
    {
        public List<GetUserPermissionByUserIDTable1> lstTable1 { get; set; }
        public List<GetUserPermissionByUserIDTable2> lstTable2 { get; set; }
        public List<GetUserPermissionByUserIDTable3> lstTable3 { get; set; }
    }

    [Serializable]
    public class GetUserPermissionByUserIDTable1
    {
        public DateTime fStart { get; set; }
        public DateTime fEnd { get; set; }
        public string PDASerialNumber { get; set; }
        public int Empid { get; set; }
        public int userid { get; set; }
        public int rolid { get; set; }
        public int workID { get; set; }
        public string fUser { get; set; }
        public string Password { get; set; }
        public string Dispatch { get; set; }
        public string Location { get; set; }
        public string PO { get; set; }
        public string Control { get; set; }
        public string UserS { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string EMail { get; set; }
        public string Cellular { get; set; }
        public int Field { get; set; }
        public string fFirst { get; set; }
        public string Middle { get; set; }
        public string Last { get; set; }
        public DateTime DHired { get; set; }
        public DateTime DFired { get; set; }
        public string CallSign { get; set; }
        public int Rol { get; set; }
        public int fWork { get; set; }
        public int Status { get; set; }
        public string Remarks { get; set; }
        public string ticketo { get; set; }
        public string ticketd { get; set; }
        public string ticket { get; set; }
        public string UserSales { get; set; }
        public string employeeMaint { get; set; }
        public string TC { get; set; }
        public string pager { get; set; }
        public string super { get; set; }
        public Int16 sales { get; set; }
        public string Lang { get; set; }
        public int merchantinfoid { get; set; }
        public Int16 dboard { get; set; }
        public int DefaultWorker { get; set; }
        public int massreview { get; set; }
        public string msmpass { get; set; }
        public string msmuser { get; set; }
        public Int16 emailaccount { get; set; }
        public double hourlyrate { get; set; }
        public Int16 pmethod { get; set; }
        public double phour { get; set; }
        public double salary { get; set; }
        public Int16 payperiod { get; set; }
        public double mileagerate { get; set; }
        public string Ref { get; set; }
        public string elevator { get; set; }
        public string Chart { get; set; }
        public string GLAdj { get; set; }
        public string CustomerPayment { get; set; }
        public string Deposit { get; set; }
        public string Financial { get; set; }
        public string Vendor { get; set; }
        public string Bill { get; set; }
        public string BillSelect { get; set; }
        public string BillPay { get; set; }
        public string Owner { get; set; }
        public string Job { get; set; }
        public int MSAuthorisedDeviceOnly { get; set; }
        public string ProjectListPermission { get; set; }
        public string FinancePermission { get; set; }
        public string BOMPermission { get; set; }
        public string MilestonesPermission { get; set; }
        public string Item { get; set; }
        public string InvAdj { get; set; }
        public string Warehouse { get; set; }
        public string InvSetup { get; set; }
        public string InvViewer { get; set; }
        public string DocumentPermission { get; set; }
        public string ContactPermission { get; set; }
        public bool SalesAssigned { get; set; }
        public string ProjecttempPermission { get; set; }
        public bool NotificationOnAddOpportunity { get; set; }
        public decimal POLimit { get; set; }
        public Int16 POApprove { get; set; }
        public Int16? POApproveAmt { get; set; }
        public decimal? MinAmount { get; set; }
        public decimal? MaxAmount { get; set; }
        public string Lng { get; set; }
        public string Lat { get; set; }
        public string Country { get; set; }
        public string MSDeviceId { get; set; }
        public string Website { get; set; }
        public string Contact { get; set; }
        public string Title { get; set; }
        public string ProfileImage { get; set; }
        public string CoverImage { get; set; }
        public string BillingCodesPermission { get; set; }
        public string Invoice { get; set; }
        public string PurchasingmodulePermission { get; set; }
        public string BillingmodulePermission { get; set; }
        public string RPO { get; set; }
        public string AccountPayablemodulePermission { get; set; }
        public string PaymentHistoryPermission { get; set; }
        public string CustomermodulePermission { get; set; }
        public string Apply { get; set; }
        public string Collection { get; set; }
        public string bankrec { get; set; }
        public string FinancialmodulePermission { get; set; }
        public string RCmodulePermission { get; set; }
        public string ProcessRCPermission { get; set; }
        public string ProcessC { get; set; }
        public string ProcessT { get; set; }
        public string SafetyTestsPermission { get; set; }
        public string RCRenewEscalatePermission { get; set; }
        public string SchedulemodulePermission { get; set; }
        public string Resolve { get; set; }
        public string TicketPermission { get; set; }
        public string MTimesheet { get; set; }
        public string ETimesheet { get; set; }
        public string MapR { get; set; }
        public string RouteBuilder { get; set; }
        public string MassTimesheetCheck { get; set; }
        public string CreditHold { get; set; }
        public int LocCount { get; set; }
        public string salesmanager { get; set; }
        public string Sales { get; set; }
        public Int16 ToDo { get; set; }
        public Int16 ToDoC { get; set; }
        public string FU { get; set; }
        public string Proposal { get; set; }
        public string Estimates { get; set; }
        public string AwardEstimates { get; set; }
        public string salessetup { get; set; }
        public string PONotification { get; set; }
        public string WriteOff { get; set; }
        public string SSN { get; set; }
        public string Sex { get; set; }
        public DateTime DBirth { get; set; }
        public string Race { get; set; }
        public string ProjectModulePermission { get; set; }
        public string InventoryModulePermission { get; set; }
        public string JobClosePermission { get; set; }
        public string JobCompletedPermission { get; set; }
        public string JobReopenPermission { get; set; }
        public bool IsProjectManager { get; set; }
        public bool IsAssignedProject { get; set; }
        public int TicketVoidPermission { get; set; }
        public string Employee { get; set; }
        public string PRProcess { get; set; }
        public string PRRegister { get; set; }
        public string PRReport { get; set; }
        public string PRWage { get; set; }
        public string PRDeduct { get; set; }
        public bool PR { get; set; }
        public int RoleId { get; set; }
        public int ApplyUserRolePermission { get; set; }
    }

    [Serializable]
    public class GetUserPermissionByUserIDTable2
    {
        public int ID { get; set; }
        public string InServer { get; set; }
        public string InServerType { get; set; }
        public string InUsername { get; set; }
        public string InPassword { get; set; }
        public int InPort { get; set; }
        public string OutServer { get; set; }
        public string OutUsername { get; set; }
        public string OutPassword { get; set; }
        public int OutPort { get; set; }
        public bool SSL { get; set; }
        public bool TakeASentEmailCopy { get; set; }
        public string BccEmail { get; set; }
        public int UserId { get; set; }

    }

    [Serializable]
    public class GetUserPermissionByUserIDTable3
    {
        public int department { get; set; }
    }
}
