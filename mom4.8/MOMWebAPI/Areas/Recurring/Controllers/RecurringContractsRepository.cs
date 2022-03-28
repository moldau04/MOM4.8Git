using BusinessEntity;
using BusinessEntity.APModels;
using BusinessEntity.CustomersModel;
using BusinessEntity.Payroll;
using BusinessEntity.Recurring;
using BusinessLayer.Programs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MOMWebAPI.Areas.Recurring.Controllers
{
    public class RecurringContractsRepository : IRecurringContractsRepository
    {
        /// <summary>
        /// For Recurring Contracts List Screen : RecContracts.aspx / RecContracts.aspx.cs
        /// </summary>
        /// API's Naming Conventions : RecContractsList_Method Name(Parameter)
        ///

        public ListGetRouteViewModel RecContractsList_GetRoute(GetRouteParam _GetRoute, string ConnectionString, int IsActive = 0, Int32 LocID = 0, Int32 ContractID = 0)
        {
            try
            {
                return new BusinessLayer.BL_User().getRoute(_GetRoute, ConnectionString, IsActive, LocID, ContractID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<GetActiveServiceTypeViewModel> RecContractsList_GetActiveServiceType(GetActiveServiceTypeParam _GetActiveServiceType, string ConnectionString)
        {
            try
            {
                return new BusinessLayer.Programs.BL_ServiceType().GetActiveServiceType(_GetActiveServiceType, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<GetCompanyByUserIDViewModel> RecContractsList_GetCompanyByUserID(GetCompanyByUserIDParam _GetCompanyByUserID, string ConnectionString)
        {
            try
            {
                return new BusinessLayer.BL_Company().getCompanyByUserID(_GetCompanyByUserID, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<GetContractsDataViewModel> RecContractsList_GetContractsData(GetContractsDataParam _GetContractsData, string ConnectionString)
        {
            try
            {
                return new BusinessLayer.BL_Contracts().getContractsData(_GetContractsData, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void RecContractsList_DeleteContract(DeleteContractParam _DeleteContract, string ConnectionString)
        {
            try
            {
                new BusinessLayer.BL_Contracts().DeleteContract(_DeleteContract, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void RecContractsList_UpdateExpirationDate(UpdateExpirationDateParam _UpdateExpirationDate, string ConnectionString)
        {
            try
            {
                new BusinessLayer.BL_Contracts().UpdateExpirationDate(_UpdateExpirationDate, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<GetMultipleStockReportsViewModel> RecContractsList_GetMultipleStockReports(GetMultipleStockReportsParam _GetMultipleStockReports, string ConnectionString)
        {
            try
            {
                return new BusinessLayer.BL_ReportsData().GetMultipleStockReports(_GetMultipleStockReports, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<GetEscalationContractsViewModel> RecContractsList_GetEscalationContracts(GetEscalationContractsParam _GetEscalationContracts, string ConnectionString)
        {
            try
            {
                return new BusinessLayer.BL_Contracts().GetEscalationContracts(_GetEscalationContracts, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void RecContractsList_EscalateContract(EscalateContractParam _EscalateContract, string ConnectionString)
        {
            try
            {
                new BusinessLayer.BL_Contracts().EscalateContract(_EscalateContract, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string RecContractsList_GetDefaultWorkerHeader(GetDefaultWorkerHeaderParam _GetDefaultWorkerHeader, string ConnectionString)
        {
            try
            {
                return new BusinessLayer.BL_Customer().GetDefaultWorkerHeader(_GetDefaultWorkerHeader, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// For Add Recurring Contracts Screen : AddRecContract.aspx / AddRecContract.aspx.cs
        /// </summary>
        /// API's Naming Conventions : AddRecContract_Method Name(Parameter)
        ///

        public List<GetContractViewModel> AddRecContract_GetContract(GetContractParam _GetContract, string ConnectionString)
        {
            try
            {
                return new BusinessLayer.BL_Contracts().GetContract(_GetContract, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<GetElevContractViewModel> AddRecContract_GetElevContract(GetElevContractParam _GetElevContract, string ConnectionString)
        {

            try
            {
                return new BusinessLayer.BL_Contracts().GetElevContract(_GetElevContract, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ListGetEstimateByID AddRecContract_GetEstimateByID(GetEstimateByIDParam _GetEstimateByID, string ConnectionString)
        {

            try
            {
                return new BusinessLayer.BL_Customer().GetEstimateByID(_GetEstimateByID, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<GetLocationTypeViewModel> AddRecContract_getlocationType(getlocationTypeParam _getlocationType, string ConnectionString)
        {
            try
            {
                return new BusinessLayer.BL_User().getlocationType(_getlocationType, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ListGetRecurringCustom AddRecContract_GetRecurringCustom(GetRecurringCustomParam _GetRecurringCustom, string ConnectionString)
        {
            try
            {
                return new BusinessLayer.BL_Job().GetRecurringCustom(_GetRecurringCustom, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<GetActiveServiceTypeViewModel> AddRecContract_GetActiveServiceTypeContract(GetActiveServiceTypeContractParam _GetActiveServiceTypeContract, string ConnectionString, string LocType, string EditSType, int department = -1, int route = -1)
        {
            try
            {
                return new BL_ServiceType().GetActiveServiceTypeContract(_GetActiveServiceTypeContract, ConnectionString, LocType, EditSType, department = -1, route = -1);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<GetEquiptypeViewModel> AddRecContract_GetEquiptype(GetEquiptypeParam _GetEquiptype, string ConnectionString)
        {
            try
            {
                return new BusinessLayer.BL_User().getEquiptype(_GetEquiptype, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<GetEquiptypeViewModel> AddRecContract_GetEquipmentCategory(GetEquipmentCategoryParam _GetEquipmentCategory, string ConnectionString)
        {
            try
            {
                return new BusinessLayer.BL_User().getEquipmentCategory(_GetEquipmentCategory, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<GetBuildingElevViewModel> AddRecContract_GetBuildingElev(GetBuildingElevParam _GetBuildingElev, string ConnectionString)
        {
            try
            {
                return new BusinessLayer.BL_User().getBuildingElev(_GetBuildingElev, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<GetJstatusViewModel> AddRecContract_GetJstatus(GetJstatusParam _GetJstatus, string ConnectionString)
        {
            try
            {
                return new BusinessLayer.BL_Contracts().getJstatus(_GetJstatus, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<GetElevViewModel> AddRecContract_GetElev(GetElevParam _GetElev, string ConnectionString, Int32 IsSalesAsigned = 0)
        {
            try
            {
                return new BusinessLayer.BL_User().getElev(_GetElev, ConnectionString, IsSalesAsigned);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool AddRecContract_IsExistContractByLoc(IsExistContractByLocParam _IsExistContractByLoc, string ConnectionString)
        {
            try
            {
                return new BusinessLayer.BL_Contracts().IsExistContractByLoc(_IsExistContractByLoc, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddRecContract_UpdateContract(UpdateContractParam _UpdateContract, string ConnectionString)
        {
            try
            {
                new BusinessLayer.BL_Contracts().UpdateContract(_UpdateContract, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddRecContract_AddContract(AddContractParam _AddContract, string ConnectionString)
        {
            try
            {
                new BusinessLayer.BL_Contracts().AddContract(_AddContract, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ListGetLocationByID AddRecContract_GetLocationByID(GetLocationByIDParam _GetLocationByID, string ConnectionString)
        {
            try
            {
                return new BusinessLayer.BL_User().getLocationByID(_GetLocationByID, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<GetLocationAutojqueryViewModel> AddRecContract_GetLocationAutojquery(GetLocationAutojqueryParam _GetLocationAutojquery, string ConnectionString, Int32 IsSalesAsigned = 0)
        {
            try
            {
                return new BusinessLayer.BL_User().getLocationAutojquery(_GetLocationAutojquery, ConnectionString, IsSalesAsigned);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<GetAllLocationOnCustomerViewModel> AddRecContract_GetAllLocationOnCustomer(GetAllLocationOnCustomerParam _GetAllLocationOnCustomer, int _ownerId, string ConnectionString)
        {
            try
            {
                return new BusinessLayer.BL_Customer().getAllLocationOnCustomer(_GetAllLocationOnCustomer, _ownerId, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<GetServiceTypeByTypeViewModel> AddRecContract_GetServiceTypeByType(GetServiceTypeByTypeParam _GetServiceTypeByType, string ConnectionString)
        {
            try
            {
                return new BusinessLayer.BL_User().GetServiceTypeByType(_GetServiceTypeByType, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<GetDiagnosticCategoryViewModel> AddRecContract_GetDiagnosticCategory(GetDiagnosticCategoryParam _GetDiagnosticCategory, string ConnectionString)
        {
            try
            {
                return new BusinessLayer.BL_General().getDiagnosticCategory(_GetDiagnosticCategory, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<GetControlViewModel> AddRecContract_GetControl(getConnectionConfigParam _getConnectionConfigParam, string ConnectionString)
        {
            try
            {
                return new BusinessLayer.BL_User().getControl(_getConnectionConfigParam, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<GetRecurringContractLogsViewModel> AddRecContract_GetRecurringContractLogs(GetRecurringContractLogsParam _GetRecurringContractLogs, string ConnectionString)
        {
            try
            {
                return new BusinessLayer.BL_Contracts().GetRecurringContractLogs(_GetRecurringContractLogs, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<GetTerritoryViewModel> AddRecContract_GetTerritory(GetTerritoryParam _GetTerritory, string ConnectionString)
        {
            try
            {
                return new BusinessLayer.BL_User().getTerritory(_GetTerritory, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// For Recurring Reports List Screen : RecContractsModule.aspx / RecContractsModule.aspx.cs
        /// </summary>
        /// API's Naming Conventions : RecContractsReport_Method Name(Parameter)
        ///

        public List<CustomerReportViewModel> RecContractsReport_GetReportDetailById(GetReportDetailByIdParam _GetReportDetailByIdParam, string ConnectionString)
        {
            try
            {
                return new BusinessLayer.BL_ReportsData().GetReportDetailById(_GetReportDetailByIdParam, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CustomerReportViewModel> RecContractsReport_GetCustomerType(GetCustomerTypeParam _GetCustomerType, string ConnectionString)
        {
            try
            {
                return new BusinessLayer.BL_ReportsData().GetCustomerType(_GetCustomerType, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CustomerFilterViewModel> RecContractsReport_GetCustomerName(GetCustomerNameParam _GetCustomerNameParam, string ConnectionString)
        {
            try
            {
                return new BusinessLayer.BL_ReportsData().GetCustomerName(_GetCustomerNameParam, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CustomerFilterViewModel> RecContractsReport_GetCustomerAddress(GetCustomerAddressParam _GetCustomerAddressParam, string ConnectionString)
        {
            try
            {
                return new BusinessLayer.BL_ReportsData().GetCustomerAddress(_GetCustomerAddressParam, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CustomerFilterViewModel> RecContractsReport_GetCustomerCity(GetCustomerCityParam _GetCustomerCityParam, string ConnectionString)
        {
            try
            {
                return new BusinessLayer.BL_ReportsData().GetCustomerCity(_GetCustomerCityParam, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CustomerReportViewModel> RecContractsReport_GetDynamicReports(GetDynamicReportsParam _GetDynamicReportsParam, string ConnectionString)
        {
            try
            {
                return new BusinessLayer.BL_ReportsData().GetDynamicReports(_GetDynamicReportsParam, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CustomerFilterViewModel> RecContractsReport_GetReportColByRepId(GetReportColByRepIdParam _GetReportColByRepId, string ConnectionString)
        {
            try
            {
                return new BusinessLayer.BL_ReportsData().GetReportColByRepId(_GetReportColByRepId, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CustomerFilterViewModel> RecContractsReport_GetReportFiltersByRepId(GetReportFiltersByRepIdParam _GetReportFiltersByRepId, string ConnectionString)
        {
            try
            {
                return new BusinessLayer.BL_ReportsData().GetReportFiltersByRepId(_GetReportFiltersByRepId, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<GetRecurringDetailsViewModel> RecContractsReport_GetRecurringDetails(GetRecurringDetailsParam _GetRecurringDetails, string ConnectionString)
        {
            try
            {
                return new BusinessLayer.BL_ReportsData().GetRecurringDetails(_GetRecurringDetails, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<UserViewModel> RecContractsReport_GetAccountSummaryListingDetail(GetAccountSummaryListingDetailParam _GetAccountSummaryListingDetail, string ConnectionString)
        {
            try
            {
                return new BusinessLayer.BL_ReportsData().GetAccountSummaryListingDetail(_GetAccountSummaryListingDetail, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool RecContractsReport_CheckExistingReport(CheckExistingReportParam _CheckExistingReportParam, string ConnectionString)
        {
            try
            {
                return new BusinessLayer.BL_ReportsData().CheckExistingReport(_CheckExistingReportParam, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public List<CustomerReportViewModel> RecContractsReport_InsertCustomerReport(InsertCustomerReportParam _InsertCustomerReportParam, string ConnectionString)
        {
            try
            {
                return new BusinessLayer.BL_ReportsData().InsertCustomerReport(_InsertCustomerReportParam, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool RecContractsReport_IsStockReportExist(IsStockReportExistParam _IsStockReportExistParam, string ConnectionString)
        {
            try
            {
                return new BusinessLayer.BL_ReportsData().IsStockReportExist(_IsStockReportExistParam, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void RecContractsReport_UpdateCustomerReport(UpdateCustomerReportParam _UpdateCustomerReportParam, string ConnectionString)
        {
            try
            {
                new BusinessLayer.BL_ReportsData().UpdateCustomerReport(_UpdateCustomerReportParam, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void RecContractsReport_DeleteCustomerReport(DeleteCustomerReportParam _DeleteCustomerReport, string ConnectionString)
        {
            try
            {
                new BusinessLayer.BL_ReportsData().DeleteCustomerReport(_DeleteCustomerReport, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CustomerFilterViewModel> RecContractsReport_GetControlForReports(getConnectionConfigParam _getConnectionConfigParam, string ConnectionString)
        {
            try
            {
                return new BusinessLayer.BL_ReportsData().GetControlForReports(_getConnectionConfigParam, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<HeaderFooterDetailViewModel> RecContractsReport_GetHeaderFooterDetail(GetHeaderFooterDetailParam _GetHeaderFooterDetail, string ConnectionString)
        {
            try
            {
                return new BusinessLayer.BL_ReportsData().GetHeaderFooterDetail(_GetHeaderFooterDetail, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CustomerFilterViewModel> RecContractsReport_GetOwners(GetOwnersParam _GetOwnersParam, string ConnectionString)
            {
                try
                {
                    return new BusinessLayer.BL_ReportsData().GetOwners(_GetOwnersParam, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CustomerReportViewModel> RecContractsReport_GetColumnWidthByReportId(GetColumnWidthByReportIdParam _GetColumnWidthByReportId, string ConnectionString)
                {
                    try
                    {
                        return new BusinessLayer.BL_ReportsData().GetColumnWidthByReportId(_GetColumnWidthByReportId, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void RecContractsReport_UpdateCustomerReportResizedWidth(UpdateCustomerReportResizedWidthParam _UpdateCustomerReportResizedWidth, string ConnectionString)
        {
            try
            {
                new BusinessLayer.BL_ReportsData().UpdateCustomerReportResizedWidth(_UpdateCustomerReportResizedWidth, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// For Recurring Reports List Screen : EscalationListingReport.aspx / EscalationListingReport.aspx.cs
        /// </summary>
        /// API's Naming Conventions : RecContractsReport_Method Name(Parameter)
        ///
        public string RecContractsReport_GetUserEmail(GetUserEmailParam _GetUserEmail, string ConnectionString)
        {
            return new BusinessLayer.BL_User().getUserEmail(_GetUserEmail, ConnectionString);
        }
        public ListGetEscalationReportFiltersValue RecContractsReport_GetEscalationReportFiltersValue(GetEscalationReportFiltersValueParam _GetEscalationReportFiltersValue, string ConnectionString)
        {
            return new BusinessLayer.BL_ReportsData().GetEscalationReportFiltersValue(_GetEscalationReportFiltersValue, ConnectionString);
        }
        public List<GetEscalationDetailsViewModel> RecContractsReport_GetEscalationDetails(GetEscalationDetailsParam _GetEscalationDetails, string ConnectionString)
        {
            return new BusinessLayer.BL_ReportsData().GetEscalationDetails(_GetEscalationDetails, ConnectionString);
        }

        /// <summary>
        /// For Recurring Reports List Screen : MonthlyRecurringHoursReport.aspx / MonthlyRecurringHoursReport.aspx.cs
        /// </summary>
        /// API's Naming Conventions : RecContractsReport_Method Name(Parameter)
        ///

        /// <summary>
        /// For Recurring Reports List Screen : MonthlyRecurringHoursByRouteReport.aspx / MonthlyRecurringHoursByRouteReport.aspx.cs
        /// </summary>
        /// API's Naming Conventions : RecContractsReport_Method Name(Parameter)
        ///

        public List<GetMonthlyRecurringHoursTEIViewModel> RecContractsReport_GetMonthlyRecurringHoursTEI(GetMonthlyRecurringHoursTEIParam _GetMonthlyRecurringHoursTEI, string ConnectionString, string routes)
        {
            try
            {
                return new BusinessLayer.BL_Report().GetMonthlyRecurringHoursTEI(_GetMonthlyRecurringHoursTEI, ConnectionString, routes);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<GetMonthlyRecurringHoursViewModel> RecContractsReport_GetMonthlyRecurringHours(GetMonthlyRecurringHoursParam _GetMonthlyRecurringHours, string ConnectionString, string routes)
        {
            try
            {
                return new BusinessLayer.BL_Report().GetMonthlyRecurringHours(_GetMonthlyRecurringHours, ConnectionString, routes);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<GetRouteActiveViewModel> RecContractsReport_GetRouteActive(GetRouteActiveParam _GetRouteActive, string ConnectionString)
        {
            try
            {
                return new BusinessLayer.BL_User().getRouteActive(_GetRouteActive, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// For Recurring Reports List Screen : OpenMaintenanceByEquipmentReport.aspx / OpenMaintenanceByEquipmentReport.aspx.cs
        /// </summary>
        /// API's Naming Conventions : RecContractsReport_Method Name(Parameter)
        ///

        public List<GetCompanyDetailsViewModel> RecContractsReport_GetCompanyDetails(GetCompanyDetailsParam _GetCompanyDetailsParam, string ConnectionString)
        {
            try
            {
                return new BusinessLayer.BL_Report().GetCompanyDetails(_GetCompanyDetailsParam, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public String RecContractsReport_GetDefaultCategory(GetDefaultCategoryParam _GetDefaultCategory, string ConnectionString)
        {
            try
            {
                return new BusinessLayer.BL_User().getDefaultCategory(_GetDefaultCategory, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<GetOpenMaintenanceByEquipmentViewModel> RecContractsReport_GetOpenMaintenanceByEquipment(GetOpenMaintenanceByEquipmentParam _GetOpenMaintenanceByEquipment, string ConnectionString, string defaultCategory)
        {
            try
            {
                return new BusinessLayer.BL_Report().GetOpenMaintenanceByEquipment(_GetOpenMaintenanceByEquipment, ConnectionString, defaultCategory);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<SMTPEmailViewModel> RecContractsReport_GetSMTPByUserID(GetSMTPByUserIDParam _GetSMTPByUserID, string ConnectionString)
        {
            try
            {
                return new BusinessLayer.BL_User().getSMTPByUserID(_GetSMTPByUserID, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



    }
}
