using BusinessEntity;
using BusinessEntity.APModels;
using BusinessEntity.CustomersModel;
using BusinessEntity.InventoryModel;
using BusinessEntity.payroll;
using BusinessEntity.Payroll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MOMWebAPI.Areas.Customers.Controllers
{
    public interface ICustomersRepository
    {

        /// <summary>
        /// For Customers List Screen : Customers.aspx / Customers.aspx.cs
        /// </summary>
        /// API's Naming Conventions : CustomersList_Method Name(Parameter)
        /// 
        public List<CustomViewModel> CustomersList_GetCustomFields(getCustomFieldsParam _getCustomFields, string ConnectionString);
        public List<GetControlViewModel> CustomersList_GetControl(getConnectionConfigParam _getConnectionConfig, string ConnectionString);
        //public List<UserViewModel> CustomersList_GetUserPermissionByUserID(GetUserByIdParam _GetUserById, string ConnectionString);
        public List<CompanyOfficeViewModel> CustomersList_GetCompanyByCustomer(GetCompanyByCustomerParam _GetCompanyByCustomer, string ConnectionString);
        public List<CustomerReportViewModel> CustomersList_GetStockReports(GetStockReportsParam _GetStockReports, string ConnectionString);
        public List<GetCustomerTypeViewModel> CustomersList_GetCustomerType(getCustomerTypeParam _getCustomerType, string ConnectionString);
        public void CustomersList_DeleteCustomer(DeleteCustomerParam _DeleteCustomer, string ConnectionString);
        public void CustomersList_AddQBCustomerType(AddQBCustomerTypeParam _AddQBCustomerType, string ConnectionString);
        public List<GetMSMCustomertypeViewModel> CustomersList_GetMSMCustomertype(GetMSMCustomertypeParam _GetMSMCustomertype, string ConnectionString);
        public void CustomersList_UpdateQBcustomertypeID(UpdateQBcustomertypeIDParam _UpdateQBcustomertypeID, string ConnectionString);
        public void CustomersList_AddQBLocType(AddQBLocTypeParam _AddQBLocType, string ConnectionString);
        public List<GetMSMLoctypeViewModel> CustomersList_GetMSMLoctype(GetMSMLoctypeParam _GetMSMLoctype, string ConnectionString);
        public void CustomersList_UpdateQBJobtypeID(UpdateQBJobtypeIDParam _UpdateQBJobtypeID, string ConnectionString);
        public void CustomersList_AddQBSalesTax(AddQBSalesTaxParam _AddQBSalesTax, string ConnectionString);
        public List<GetMSMSalesTaxViewModel> CustomersList_GetMSMSalesTax(GetMSMSalesTaxParam _GetMSMSalesTax, string ConnectionString);
        public void CustomersList_UpdateQBsalestaxID(UpdateQBsalestaxIDParam _UpdateQBsalestaxID, string ConnectionString);
        public void CustomersList_AddCustomerQB(AddCustomerQBParam _AddCustomerQB, string ConnectionString);
        public void CustomersList_AddQBLocation(AddQBLocationParam _AddQBLocation, string ConnectionString);
        public List<GetMSMCustomersViewModel> CustomersList_GetMSMCustomers(GetMSMCustomersParam _GetMSMCustomers, string ConnectionString);
        public void CustomersList_UpdateQBCustomerID(UpdateQBCustomerIDParam _UpdateQBCustomerID, string ConnectionString);
        public List<GetQBCustomersViewModel> CustomersList_GetQBCustomers(GetQBCustomersParam _GetQBCustomers, string ConnectionString);
        public List<GetMSMLocationViewModel> CustomersList_GetMSMLocation(GetMSMLocationParam _GetMSMLocation, string ConnectionString);
        public void CustomersList_UpdateQBLocationID(UpdateQBLocationIDParam _UpdateQBLocationID, string ConnectionString);
        public List<GetQBLocationViewModel> CustomersList_GetQBLocation(GetQBLocationParam _GetQBLocation, string ConnectionString);
        public void CustomersList_UpdateQBLastSync(UpdateQBLastSyncParam _UpdateQBLastSync, string ConnectionString);
        public List<GeneralViewModel> CustomersList_GetSagelatsync(getConnectionConfigParam _getConnectionConfig, string ConnectionString);
        public List<GetCustomerSearchViewModel> CustomersList_GetCustomerSearch(GetCustomerSearchParam _GetCustomerSearch, Int32 IsSalesAsigned, string ConnectionString);
        public List<GetCustomersViewModel> CustomersList_GetCustomers(GetCustomersParam _GetCustomers, Int32 IsSalesAsigned, string ConnectionString);


        /// <summary>
        /// For AddCustomer Screen : AddCustomer.aspx / AddCustomer.aspx.cs
        /// </summary>
        /// API's Naming Conventions : AddCustomer_Method Name(Parameter)
        /// 

        public ListGetCustomerByID AddCustomer_GetCustomerByID(GetCustomerByIDParam _GetCustomerByID, Int32 IsSalesAsigned, string ConnectionString);
        public List<GetAllLocationOnCustomerViewModel> AddCustomer_GetAllLocationOnCustomer(GetAllLocationOnCustomerParam _GetAllLocationOnCustomer, int _ownerId, string ConnectionString);

        public void AddCustomer_UpdateCustomer(UpdateCustomerParam _UpdateCustomer, bool CopyToLocAndJob, string ConnectionString);
        public void AddCustomer_AddCustomer(AddCustomerParam _AddCustomer, string ConnectionString);
        public void AddCustomer_UpdateCustomerContactRecordLog(UpdateCustomerContactRecordLogParam _UpdateCustomerContactRecordLog, string ConnectionString);
        public void AddCustomer_DeleteLocation(DeleteLocationParam _DeleteLocation, string ConnectionString);
        public List<GetElevViewModel> AddCustomer_GetElev(GetElevParam _GetElev, string ConnectionString, Int32 IsSalesAsigned = 0);
        public void AddCustomer_DeleteEquipment(DeleteEquipmentParam _DeleteEquipment, string ConnectionString);

        public List<GetCategoryViewModel> AddCustomer_GetCategory(GetCategoryParam _GetCategory, string ConnectionString);

        public List<GetCallHistoryViewModel> AddCustomer_GetCallHistory(GetCallHistoryParam _GetCallHistory, string ConnectionString, Int32 IsSalesAsigned = 0, int IsCallForTicketReport = 0);

        public ListGetARRevenueCustShowAll AddCustomer_GetARRevenueCust(GetARRevenueCustParam _GetARRevenueCust, string ConnectionString);

        public ListGetARRevenueCustShowAll AddCustomer_GetARRevenueCustShowAll(GetARRevenueCustShowAllParam _GetARRevenueCustShowAll, string ConnectionString);

        public ListGetProspectByID AddCustomer_GetProspectByID(GetProspectByIDParam _GetProspectByID, string ConnectionString);
        public List<JobTypeViewModel> AddCustomer_GetDepartment(GetDepartmentParam _GetDepartment, string ConnectionString);

        public List<GetLocationByCustomerIDViewModel> AddCustomer_GetLocationByCustomerID(GetLocationByCustomerIDParam _GetLocationByCustomerID, string ConnectionString);

        public void AddCustomer_AddFile(AddFileParam _AddFile, string ConnectionString);

        public void AddCustomer_UpdateDocInfo(UpdateDocInfoParam _UpdateDocInfo, string ConnectionString);

        public List<GetDocumentsViewModel> AddCustomer_GetDocuments(GetDocumentsParam _GetDocuments, string ConnectionString);

        public void AddCustomer_DeleteFile(DeleteFileParam _DeleteFile, string ConnectionString);

        public List<GetCustomersLogsViewModel> AddCustomer_GetCustomersLogs(GetCustomersLogsParam _GetCustomersLogs, string ConnectionString);

        public List<GetContactLogByCustomerIDViewModel> AddCustomer_GetContactLogByCustomerID(GetContactLogByCustomerIDParam _GetContactLogByCustomerID, string ConnectionString);

        public List<GetContactByRolIDViewModel> AddCustomer_GetContactByRolID(GetContactByRolIDParam _GetContactByRolID, string ConnectionString);

        public void AddCustomer_DeleteOpportunity(DeleteOpportunityParam _DeleteOpportunity, string ConnectionString);

        public List<GetOpportunityOfCustomerViewModel> AddCustomer_GetOpportunityOfCustomer(GetOpportunityOfCustomerParam _GetOpportunityOfCustomer, string ConnectionString);

        public List<GetJobProjectViewModel> AddCustomer_GetJobProject(GetJobProjectParam _GetJobProject, string ConnectionString, Int32 IsSalesAsigned = 0, int IncludeClose = 1);


        /// <summary>
        /// For CompletedTicketReport Screen : CompletedTicketReport.aspx / CompletedTicketReport.aspx.cs
        /// </summary>
        /// API's Naming Conventions : AddCustomer_ServiceHistoryReport_Method Name(Parameter)
        /// 
        public List<GetCompanyDetailsViewModel> AddCustomer_ServiceHistoryReport_GetCompanyDetails(GetCompanyDetailsParam _GetCompanyDetails, string ConnectionString);

        public List<GetCompletedTicketViewModel> AddCustomer_ServiceHistoryReport_GetCompletedTicket(GetCompletedTicketParam _GetCompletedTicket, List<RetainFilter> filters, string ConnectionString);

        public List<SMTPEmailViewModel> AddCustomer_ServiceHistoryReport_GetSMTPByUserID(GetSMTPByUserIDParam _GetSMTPByUserID, string ConnectionString);


        /// <summary>
        /// For CustomerLabel5160/CustomerLabel5163 Screen : CustomerLabel5160.aspx / CustomerLabel5160.aspx.cs/CustomerLabel5163.aspx / CustomerLabel5163.aspx.cs
        /// </summary>
        /// API's Naming Conventions : CustomerReport_Method Name(Parameter)
        /// 

        public List<GetCustomerLabelViewModel> CustomerReport_GetCustomerLabel(GetCustomerLabelParam _GetCustomerLabel, string ConnectionString, Int32 IsSalesAsigned = 0);




    }
}
