using BusinessEntity;
using BusinessEntity.APModels;
using BusinessEntity.CustomersModel;
using BusinessEntity.Payroll;
using BusinessEntity.Recurring;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MOMWebAPI.Areas.Recurring.Controllers
{
    public interface IRecurringContractsRepository
    {
        /// <summary>
        /// For Recurring Contracts List Screen : RecContracts.aspx / RecContracts.aspx.cs
        /// </summary>
        /// API's Naming Conventions : RecContractsList_Method Name(Parameter)
        ///

        public ListGetRouteViewModel RecContractsList_GetRoute(GetRouteParam _GetRoute, string ConnectionString, int IsActive = 0, Int32 LocID = 0, Int32 ContractID = 0);
        public List<GetActiveServiceTypeViewModel> RecContractsList_GetActiveServiceType(GetActiveServiceTypeParam _GetActiveServiceType, string ConnectionString);
        public List<GetCompanyByUserIDViewModel> RecContractsList_GetCompanyByUserID(GetCompanyByUserIDParam _GetCompanyByUserID, string ConnectionString);
        public List<GetContractsDataViewModel> RecContractsList_GetContractsData(GetContractsDataParam _GetContractsData, string ConnectionString);
        public void RecContractsList_DeleteContract(DeleteContractParam _DeleteContract, string ConnectionString);
        public void RecContractsList_UpdateExpirationDate(UpdateExpirationDateParam _UpdateExpirationDate, string ConnectionString);
        public List<GetMultipleStockReportsViewModel> RecContractsList_GetMultipleStockReports(GetMultipleStockReportsParam _GetMultipleStockReports, string ConnectionString);
        public List<GetEscalationContractsViewModel> RecContractsList_GetEscalationContracts(GetEscalationContractsParam _GetEscalationContracts, string ConnectionString);
        public void RecContractsList_EscalateContract(EscalateContractParam _EscalateContract, string ConnectionString);
        public string RecContractsList_GetDefaultWorkerHeader(GetDefaultWorkerHeaderParam _GetDefaultWorkerHeader, string ConnectionString);


        /// <summary>
        /// For Add Recurring Contracts Screen : AddRecContract.aspx / AddRecContract.aspx.cs
        /// </summary>
        /// API's Naming Conventions : AddRecContract_Method Name(Parameter)
        ///

        public List<GetContractViewModel> AddRecContract_GetContract(GetContractParam _GetContract, string ConnectionString);
        public List<GetElevContractViewModel> AddRecContract_GetElevContract(GetElevContractParam _GetElevContract, string ConnectionString);
        public ListGetEstimateByID AddRecContract_GetEstimateByID(GetEstimateByIDParam _GetEstimateByID, string ConnectionString);
        public List<GetLocationTypeViewModel> AddRecContract_getlocationType(getlocationTypeParam _getlocationType, string ConnectionString);
        public ListGetRecurringCustom AddRecContract_GetRecurringCustom(GetRecurringCustomParam _GetRecurringCustom, string ConnectionString);
        public List<GetActiveServiceTypeViewModel> AddRecContract_GetActiveServiceTypeContract(GetActiveServiceTypeContractParam _GetActiveServiceTypeContract, string ConnectionString, string LocType, string EditSType, int department = -1, int route = -1);
        public List<GetEquiptypeViewModel> AddRecContract_GetEquiptype(GetEquiptypeParam _GetEquiptype, string ConnectionString);
        public List<GetEquiptypeViewModel> AddRecContract_GetEquipmentCategory(GetEquipmentCategoryParam _GetEquipmentCategory, string ConnectionString);
        public List<GetBuildingElevViewModel> AddRecContract_GetBuildingElev(GetBuildingElevParam _GetBuildingElev, string ConnectionString);
        public List<GetJstatusViewModel> AddRecContract_GetJstatus(GetJstatusParam _GetJstatus, string ConnectionString);
        public List<GetElevViewModel> AddRecContract_GetElev(GetElevParam _GetElev, string ConnectionString, Int32 IsSalesAsigned = 0);
        public bool AddRecContract_IsExistContractByLoc(IsExistContractByLocParam _IsExistContractByLoc, string ConnectionString);
        public void AddRecContract_UpdateContract(UpdateContractParam _UpdateContract, string ConnectionString);
        public void AddRecContract_AddContract(AddContractParam _AddContract, string ConnectionString);
        public ListGetLocationByID AddRecContract_GetLocationByID(GetLocationByIDParam _GetLocationByID, string ConnectionString);
        public List<GetLocationAutojqueryViewModel> AddRecContract_GetLocationAutojquery(GetLocationAutojqueryParam _GetLocationAutojquery, string ConnectionString, Int32 IsSalesAsigned = 0);
        public List<GetAllLocationOnCustomerViewModel> AddRecContract_GetAllLocationOnCustomer(GetAllLocationOnCustomerParam _GetAllLocationOnCustomer, int _ownerId, string ConnectionString);
        public List<GetServiceTypeByTypeViewModel> AddRecContract_GetServiceTypeByType(GetServiceTypeByTypeParam _GetServiceTypeByType, string ConnectionString);
        public List<GetDiagnosticCategoryViewModel> AddRecContract_GetDiagnosticCategory(GetDiagnosticCategoryParam _GetDiagnosticCategory, string ConnectionString);
        public List<GetControlViewModel> AddRecContract_GetControl(getConnectionConfigParam _getConnectionConfig, string ConnectionString);
        public List<GetRecurringContractLogsViewModel> AddRecContract_GetRecurringContractLogs(GetRecurringContractLogsParam _GetRecurringContractLogs, string ConnectionString);
        public List<GetTerritoryViewModel> AddRecContract_GetTerritory(GetTerritoryParam _GetTerritory, string ConnectionString);


        /// <summary>
        /// For Recurring Reports List Screen : RecContractsModule.aspx / RecContractsModule.aspx.cs
        /// </summary>
        /// API's Naming Conventions : RecContractsReport_Method Name(Parameter)
        ///

        public List<CustomerReportViewModel> RecContractsReport_GetReportDetailById(GetReportDetailByIdParam _GetReportDetailByIdParam, string ConnectionString);
        public List<CustomerReportViewModel> RecContractsReport_GetCustomerType(GetCustomerTypeParam _GetCustomerType, string ConnectionString);
        //public List<GetRecReportFiltersValueViewModel> RecContractsReport_GetRecReportFiltersValue(GetRecReportFiltersValueParam _GetRecReportFiltersValue, string ConnectionString);
        public List<CustomerFilterViewModel> RecContractsReport_GetCustomerName(GetCustomerNameParam _GetCustomerNameParam, string ConnectionString);
        public List<CustomerFilterViewModel> RecContractsReport_GetCustomerAddress(GetCustomerAddressParam _GetCustomerAddressParam, string ConnectionString);
        public List<CustomerFilterViewModel> RecContractsReport_GetCustomerCity(GetCustomerCityParam _GetCustomerCityParam, string ConnectionString);
        public List<CustomerReportViewModel> RecContractsReport_GetDynamicReports(GetDynamicReportsParam _GetDynamicReportsParam, string ConnectionString);
        public List<CustomerFilterViewModel> RecContractsReport_GetReportColByRepId(GetReportColByRepIdParam _GetReportColByRepId, string ConnectionString);
        public List<CustomerFilterViewModel> RecContractsReport_GetReportFiltersByRepId(GetReportFiltersByRepIdParam _GetReportFiltersByRepId, string ConnectionString);
        public List<GetRecurringDetailsViewModel> RecContractsReport_GetRecurringDetails(GetRecurringDetailsParam _GetRecurringDetails, string ConnectionString);
        public List<UserViewModel> RecContractsReport_GetAccountSummaryListingDetail(GetAccountSummaryListingDetailParam _GetAccountSummaryListingDetailParam, string ConnectionString);
        public bool RecContractsReport_CheckExistingReport(CheckExistingReportParam _CheckExistingReportParam, string ConnectionString);
        public List<CustomerReportViewModel> RecContractsReport_InsertCustomerReport(InsertCustomerReportParam _InsertCustomerReportParam, string ConnectionString);
        public bool RecContractsReport_IsStockReportExist(IsStockReportExistParam _IsStockReportExistParam, string ConnectionString);
        public void RecContractsReport_UpdateCustomerReport(UpdateCustomerReportParam _UpdateCustomerReportParam, string ConnectionString);
        public void RecContractsReport_DeleteCustomerReport(DeleteCustomerReportParam _DeleteCustomerReport, string ConnectionString);
        public List<CustomerFilterViewModel> RecContractsReport_GetControlForReports(getConnectionConfigParam _getConnectionConfigParam, string ConnectionString);
        public List<HeaderFooterDetailViewModel> RecContractsReport_GetHeaderFooterDetail(GetHeaderFooterDetailParam _CustomerReportParam, string ConnectionString);
        public List<CustomerFilterViewModel> RecContractsReport_GetOwners(GetOwnersParam _GetOwnersParam, string ConnectionString);
        public List<CustomerReportViewModel> RecContractsReport_GetColumnWidthByReportId(GetColumnWidthByReportIdParam _customerreport, string ConnectionString);
        public void RecContractsReport_UpdateCustomerReportResizedWidth(UpdateCustomerReportResizedWidthParam _customerreport, string ConnectionString);


        /// <summary>
        /// For Recurring Reports List Screen : EscalationListingReport.aspx / EscalationListingReport.aspx.cs
        /// </summary>
        /// API's Naming Conventions : RecContractsReport_Method Name(Parameter)
        ///
        public string RecContractsReport_GetUserEmail(GetUserEmailParam _GetUserEmail, string ConnectionString);
        public List<GetEscalationDetailsViewModel> RecContractsReport_GetEscalationDetails(GetEscalationDetailsParam _GetEscalationDetails, string ConnectionString);
        public ListGetEscalationReportFiltersValue RecContractsReport_GetEscalationReportFiltersValue(GetEscalationReportFiltersValueParam _GetEscalationReportFiltersValue, string ConnectionString);

        /// <summary>
        /// For Recurring Reports List Screen : MonthlyRecurringHoursByRouteReport.aspx / MonthlyRecurringHoursByRouteReport.aspx.cs AND MonthlyRecurringHoursReport.aspx / MonthlyRecurringHoursReport.aspx.cs
        /// </summary>
        /// API's Naming Conventions : RecContractsReport_Method Name(Parameter)
        ///

        public List<GetMonthlyRecurringHoursTEIViewModel> RecContractsReport_GetMonthlyRecurringHoursTEI(GetMonthlyRecurringHoursTEIParam _GetMonthlyRecurringHoursTEI, string ConnectionString, string routes);
        public List<GetMonthlyRecurringHoursViewModel> RecContractsReport_GetMonthlyRecurringHours(GetMonthlyRecurringHoursParam _GetMonthlyRecurringHours, string ConnectionString, string routes);
        public List<GetRouteActiveViewModel> RecContractsReport_GetRouteActive(GetRouteActiveParam _GetRouteActive, string ConnectionString);

        /// <summary>
        /// For Recurring Reports List Screen : OpenMaintenanceByEquipmentReport.aspx / OpenMaintenanceByEquipmentReport.aspx.cs
        /// </summary>
        /// API's Naming Conventions : RecContractsReport_Method Name(Parameter)
        ///

        public List<GetCompanyDetailsViewModel> RecContractsReport_GetCompanyDetails(GetCompanyDetailsParam _GetCompanyDetails, string ConnectionString);
        public String RecContractsReport_GetDefaultCategory(GetDefaultCategoryParam _GetDefaultCategory, string ConnectionString);
        public List<GetOpenMaintenanceByEquipmentViewModel> RecContractsReport_GetOpenMaintenanceByEquipment(GetOpenMaintenanceByEquipmentParam _GetOpenMaintenanceByEquipment, string ConnectionString, string defaultCategory);
        public List<SMTPEmailViewModel> RecContractsReport_GetSMTPByUserID(GetSMTPByUserIDParam _GetSMTPByUserID, string ConnectionString);
    }
}
