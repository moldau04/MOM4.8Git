using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity
{
    public class UserRole
    {
        public string ConnConfig { get; set; }
        public int RoleID { get; set; }
        public string RoleName { get; set; }
        public string RoleDescription { get; set; }
        public int UserID { get; set; }
        public string UserName { get; set; }
        public int Status { get; set; }
        public DataTable Users { get; set; }
        //public Permission RolePermission { get; set; }
        public string SearchValue { get; set; }
        public string SearchBy { get; set; }
    }

    public class Permission
    {
        // Customer Module
        public bool CustomerMod { get; set; }
        public string Customer { get; set; }
        public string Location { get; set; }
        public string Equipment { get; set; }
        public string ReceivePayment { get; set; }
        public string MakeDeposit { get; set; }
        public string Collections { get; set; }
        public string CreditHold { get; set; }
        public string Writeoff { get; set; }

        // Recurring Module
        public bool RecurringMod { get; set; }
        public string RecurringContracts { get; set; }
        public string SafetyTests { get; set; }
        public string RecurringInvoices { get; set; }
        public string RecurringTickets { get; set; }
        public string RenewEscalate { get; set; }

        // Schedule Module
        public bool ScheduleMod { get; set; }
        public string Ticket { get; set; }
        public string CompletedTicket { get; set; }
        public string MassReviewTicket { get; set; }
        public string MassReviewTimesheet { get; set; }
        public string ScheduleBoard { get; set; }
        public string RouteBuilder { get; set; }
        public string TimestampsFixed { get; set; }
        public string TimesheetEntry { get; set; }
        public string eTimesheet { get; set; }
        public string Map { get; set; }
        public string Void { get; set; }
    }
}
