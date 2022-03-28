using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.Payroll
{
    [Serializable]
    public class ControlViewModel
    {
        public string Name { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public int fLong { get; set; }
        public int Latt { get; set; }
        public int GeoLock { get; set; }
        public int YE { get; set; }
        public int Version { get; set; }
        public string CDesc { get; set; }
        public int Build { get; set; }
        public int Minor { get; set; }
        public string Address { get; set; }
        public string AgeRemark { get; set; }
        public string SDate { get; set; }
        public string EDate { get; set; }
        public string YDate { get; set; }
        public string GSTreg { get; set; }
        public string IDesc { get; set; }
        public int PortalsID { get; set; }
        public string PrContractRemark { get; set; }
        public string RepUser { get; set; }
        public string RepTitle { get; set; }
        public string Logo { get; set; }
        public string LogoPath { get; set; }
        public DateTime ExeBuildDate_Max { get; set; }
        public DateTime ExeBuildDate_Min { get; set; }
        public string ExeVersion_Min { get; set; }
        public string ExeVersion_Max { get; set; }
        public string MerchantServicesConfig { get; set; }
        public string Email { get; set; }
        public string WebAddress { get; set; }
        public string MSM { get; set; }
        public string DSN { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string DBName { get; set; }
        public string Contact { get; set; }
        public string Remarks { get; set; }
        public int Map { get; set; }
        public int Custweb { get; set; }
        public string QBPath { get; set; }
        public int MultiLang { get; set; }
        public int  QBIntegration { get; set; }
        public DateTime  QBLastSync { get; set; }
        public int QBFirstSync { get; set; }
        public int MSEmail { get; set; }
        public int MSREP { get; set; }
        public int MSSignTime { get; set; }
        public int GrossInc { get; set; }
        public int Month { get; set; }
        public int SalesAnnual { get; set; }
        public int Payment { get; set; }
        public string QBServiceItem { get; set; }
        public string QBServiceItemLabor { get; set; }
        public string QBServiceItemExp { get; set; }
        public int GPS { get; set; }
        public DateTime  SageLastSync { get; set; }
        public int SageIntegration { get; set; }
        public int MSAttachReport { get; set; }
        public string MSRTLabel { get; set; }
        public string MSOTLabel { get; set; }
        public string MSNTLabel { get; set; }
        public string MSDTLabel { get; set; }
        public string MSTTLabel { get; set; }
        public string MSTRTLabel { get; set; }
        public string MSTOTLabel { get; set; }
        public string MSTNTLabel { get; set; }
        public string MSTDTLabel { get; set; }
        public string MSTimeDataFieldVisibility { get; set; }
        public int TsIntegration { get; set; }
        public int TInternet { get; set; }
        public DateTime SyncLast { get; set; }
        public DateTime SCDate { get; set; }
        public DateTime IntDate { get; set; }
        public int SCAmount { get; set; }
        public int IntAmount { get; set; }
        public int EndBalance { get; set; }
        public DateTime StatementDate { get; set; }
        public int bank { get; set; }
        public DateTime BusinessStart { get; set; }
        public DateTime BusinessEnd { get; set; }
        public int JobCostLabor { get; set; }
        public bool MSIsTaskCodesRequired { get; set; }
        public int Codes { get; set; }
        public bool ISshowHomeowner { get; set; }
        public bool IsLocAddressBlank { get; set; }
        public string PGUsername { get; set; }
        public string PGPassword { get; set; }
        public string PGSecretKey { get; set; }
        public bool MSAppendMCPText { get; set; }
        public bool MSSHAssignedTicket { get; set; }
        public bool MSHistoryShowLastTenTickets { get; set; }
        public bool MS { get; set; }
        public int ContactType { get; set; }
        public string Lat { get; set; }
        public string Lng { get; set; }
        public bool MSFollowupTicket { get; set; }
        public int consultAPI { get; set; }
        public DateTime businesssEnd { get; set; }
        public string CoCode { get; set; }
        public int ShutdownAlert { get; set; }
        public int MSCategoryPermission { get; set; }
       
        public bool IsSalesTaxAPBill { get; set; }
        public bool IsUseTaxAPBill { get; set; }
        public bool ApplyPasswordRules { get; set; }
        public bool ApplyPwRulesToFieldUser { get; set; }
        public bool ApplyPwRulesToOfficeUser { get; set; }
        public bool  ApplyPwRulesToCustomerUser { get; set; }
        public bool ApplyPwReset { get; set; }
        public int PwResetDays { get; set; }
        public int PwResetting { get; set; }
        public int PwResetUserID { get; set; }
        public int JobCostLabor1 { get; set; }
        public int msemailnull { get; set; }
        public int EmpSync { get; set; }
        public int msreptemp { get; set; }
        public int tinternett { get; set; }
        public int businessstart { get; set; }
        public int businesssend { get; set; }
        public int TaskCode { get; set; }
        public int Year { get; set; }
        public int IsUseTaxAPBills { get; set; }
        public int IsSalesTaxAPBills { get; set; }
        public int TargetHPermission { get; set; }
        public string PwResetAdminEmail { get; set; }
        public string PwResetUsername { get; set; }


    }
}
