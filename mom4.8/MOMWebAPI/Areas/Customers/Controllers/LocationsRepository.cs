using BusinessEntity;
using BusinessEntity.APModels;
using BusinessEntity.CustomersModel;
using BusinessEntity.payroll;
using BusinessEntity.Payroll;
using BusinessLayer.Schedule;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MOMWebAPI.Areas.Customers.Controllers
{
    public class LocationsRepository : ILocationsRepository
    {
        /// <summary>
        /// For Locations List Screen : Locations.aspx / Locations.aspx.cs
        /// </summary>
        /// API's Naming Conventions : LocationsList_Method Name(Parameter)
        ///

        public List<CompanyOfficeViewModel> LocationsList_GetCompanyByCustomer(GetCompanyByCustomerParam _GetCompanyByCustomerParam, string ConnectionString)
        {
            try
            {
                return new BusinessLayer.BL_Company().getCompanyByCustomer(_GetCompanyByCustomerParam, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<GetZoneViewModel> LocationsList_GetZone(GetZoneParam _GetZone, string ConnectionString)
        {
            try
            {
                return new BusinessLayer.BL_User().getZone(_GetZone, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<GetTerritoryViewModel> LocationsList_GetTerritory(GetTerritoryParam _GetTerritory, string ConnectionString)
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

        public List<CustomViewModel> LocationsList_GetCustomFieldsControl(getCustomFieldsControlParam _getCustomFieldsControlParam, string ConnectionString)
        {
            try
            {
                return new BusinessLayer.BL_General().getCustomFieldsControl(_getCustomFieldsControlParam, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CustomerReportViewModel> LocationsList_GetStockReports(GetStockReportsParam _GetStockReportsParam, string connectionString)
        {
            try
            {
                return new BusinessLayer.BL_ReportsData().GetStockReports(_GetStockReportsParam, connectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void LocationsList_DeleteLocation(DeleteLocationParam _DeleteLocation, string ConnectionString)
        {
            try
            {
                new BusinessLayer.BL_User().DeleteLocation(_DeleteLocation, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<GetLocationDataSearchViewModel> LocationsList_GetLocationDataSearch(GetLocationDataSearchParam _GetLocationDataSearch, string ConnectionString, Int32 IsSalesAsigned = 0)
        {
            try
            {
                return new BusinessLayer.BL_User().getLocationDataSearch(_GetLocationDataSearch, ConnectionString, IsSalesAsigned);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<GetLocationDataSearchViewModel> LocationsList_GetLocationsData(GetLocationsDataParam _GetLocationsData, string ConnectionString, Int32 IsSalesAsigned = 0)
        {
            try
            {
                return new BusinessLayer.BL_User().getLocationsData(_GetLocationsData, ConnectionString, IsSalesAsigned);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<GetLocationTypeViewModel> LocationsList_GridGetLocationType(GetLocationTypeParam _GetLocationType, string ConnectionString)
        {
            try
            {
                return new BusinessLayer.BL_User().GetLocationType(_GetLocationType, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void LocationsList_ImportDataForMassAttachDocuments(ImportDataForMassAttachDocumentsParam _ImportDataForMassAttachDocuments, string ConnectionString, DataTable dataTable)
        {
            try
            {
                 new BusinessLayer.BL_MapData().ImportDataForMassAttachDocuments(_ImportDataForMassAttachDocuments, ConnectionString, dataTable);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string LocationsList_GetDefaultWorkerHeader(GetDefaultWorkerHeaderParam _GetDefaultWorkerHeader, string ConnectionString)
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

        public List<GetLocationTypeViewModel> LocationsList_getlocationType(getlocationTypeParam _getlocationType, string ConnectionString)
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

        public ListGetRouteViewModel LocationsList_GetRoute(GetRouteParam _GetRoute, string ConnectionString, int IsActive = 0, Int32 LocID = 0, Int32 ContractID = 0)
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

        public List<GetBTViewModel> LocationsList_GetBT(GetBTParam _GetBT, string ConnectionString)
        {
            try
            {
                return new BusinessLayer.BL_Customer().getBT(_GetBT, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        /// <summary>
        /// For Add Locations Screen : AddLocation.aspx / AddLocation.aspx.cs
        /// </summary>
        /// API's Naming Conventions : AddLocation_Method Name(Parameter)
        /// 

        public List<GeneralViewModel> AddLocation_GetSagelatsync(getConnectionConfigParam _getConnectionConfig, string ConnectionString)
        {
            return new BusinessLayer.BL_General().getSagelatsync(_getConnectionConfig, ConnectionString);
        }

        public List<GetSingleConsultantViewModel> AddLocation_GetSingleConsultant(GetSingleConsultantParam _GetSingleConsultant, string ConnectionString)
        {
            return new BusinessLayer.BL_User().getSingleConsultant(_GetSingleConsultant, ConnectionString);
        }

        public ListGetLocationByID AddLocation_GetLocationByID(GetLocationByIDParam _GetLocationByID, string ConnectionString)
        {
            return new BusinessLayer.BL_User().getLocationByID(_GetLocationByID, ConnectionString);
        }

        public ListGetCustomerByID AddLocation_GetCustomerByID(GetCustomerByIDParam _GetCustomerByID, Int32 IsSalesAsigned, string ConnectionString)
        {
            return new BusinessLayer.BL_User().getCustomerByID(_GetCustomerByID, IsSalesAsigned, ConnectionString);
        }
        public ListGetGCandHowerLocID AddLocation_GetGCandHowerLocID(GetGCandHowerLocIDParam _GetGCandHowerLocID, string ConnectionString)
        {
            return new BusinessLayer.BL_User().GetGCandHowerLocID(_GetGCandHowerLocID, ConnectionString);
        }

        public List<TermsViewModel> AddLocation_GetTerms(GetTermsParam _GetTermsParam, string ConnectionString)
        {
            return new BusinessLayer.BL_User().getTerms(_GetTermsParam, ConnectionString);
        }

        public List<GetControlViewModel> AddLocation_GetControl(getConnectionConfigParam _getConnectionConfigParam, string ConnectionString)
        {
            return new BusinessLayer.BL_User().getControl(_getConnectionConfigParam, ConnectionString);
        }
        public ListGetProspectByID AddLocation_GetProspectByID(GetProspectByIDParam _GetProspectByID, string ConnectionString)
        {
            return new BusinessLayer.BL_Customer().getProspectByID(_GetProspectByID, ConnectionString);
        }
        public List<GetCategoryViewModel> AddLocation_GetCategory(GetCategoryParam _GetCategory, string ConnectionString)
        {
            return new BusinessLayer.BL_User().getCategory(_GetCategory, ConnectionString);
        }
        public List<GetCustomersViewModel> AddLocation_GetCustomers(GetCustomersParam _GetCustomers, Int32 IsSalesAsigned, string ConnectionString)
        {
            return new BusinessLayer.BL_User().getCustomers(_GetCustomers, IsSalesAsigned, ConnectionString);
        }
        public List<STaxViewModel> AddLocation_GetSTax(getSTaxParam _getSTaxParam, string ConnectionString)
        {
            try
            {
                return new BusinessLayer.BL_User().getSTax(_getSTaxParam, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<getSalesTax2ViewModel> AddLocation_getSalesTax2(getSalesTax2Param _getSalesTax2, string ConnectionString)
        {
            try
            {
                return new BusinessLayer.BL_User().getSalesTax2(_getSalesTax2, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<STaxViewModel> AddLocation_GetUseTax(getUseTaxParam _getUseTaxParam, string ConnectionString)
        {
            try
            {
                return new BusinessLayer.BL_User().getUseTax(_getUseTaxParam, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CustomViewModel> AddLocation_GetCustomFields(getCustomFieldsParam _getCustomFieldsParam, string ConnectionString)
        {
            try
            {
                return new BusinessLayer.BL_General().getCustomFields(_getCustomFieldsParam, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool AddLocation_IsExistContractByLoc(IsExistContractByLocParam _IsExistContractByLoc, string ConnectionString)
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
        public void AddLocation_UpdateLocation(UpdateLocationParam _UpdateLocation, string ConnectionString, bool CopyToLocAndJob = false, int ApplyServiceTypeRule = 0, string ServiceTypeName = "", int ProjectPerDepartmentCount = 0)
        {
            try
            {
                new BusinessLayer.BL_User().UpdateLocation(_UpdateLocation, ConnectionString, CopyToLocAndJob, ApplyServiceTypeRule, ServiceTypeName, ProjectPerDepartmentCount);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int AddLocation_AddLocation(AddLocationParam _AddLocation, string ConnectionString)
        {
            try
            {
                return new BusinessLayer.BL_User().AddLocation(_AddLocation, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AddLocation_ConvertLeadEquipment(ConvertLeadEquipmentParam _ConvertLeadEquipment, string ConnectionString)
        {
            try
            {
                new BusinessLayer.BL_Customer().ConvertLeadEquipment(_ConvertLeadEquipment, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AddLocation_UpdateLocationContactRecordLog(UpdateLocationContactRecordLogParam _UpdateLocationContactRecordLog, string ConnectionString)
        {
            try
            {
                new BusinessLayer.BL_User().UpdateLocationContactRecordLog(_UpdateLocationContactRecordLog, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddLocation_DeleteEquipment(DeleteEquipmentParam _DeleteEquipment, string ConnectionString)
        {
            try
            {
                new BusinessLayer.BL_User().DeleteEquipment(_DeleteEquipment, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<JobTypeViewModel> AddLocation_GetDepartment(GetDepartmentParam _GetDepartment, string ConnectionString)
        {
            return new BusinessLayer.BL_User().getDepartment(_GetDepartment, ConnectionString);
        }
        public void AddLocation_AddFile(AddFileParam _AddFile, string ConnectionString)
        {
            new BusinessLayer.BL_MapData().AddFile(_AddFile, ConnectionString);
        }
        public void AddLocation_UpdateDocInfo(UpdateDocInfoParam _UpdateDocInfo, string ConnectionString)
        {
            new BusinessLayer.BL_User().UpdateDocInfo(_UpdateDocInfo, ConnectionString);
        }
        public List<GetLocationDocumentsViewModel> AddLocation_GetLocationDocuments(GetLocationDocumentsParam _GetLocationDocuments, string ConnectionString, bool isShowAll, bool isLocation)
        {
            return new BusinessLayer.BL_MapData().GetLocationDocuments(_GetLocationDocuments, ConnectionString, isShowAll, isLocation);
        }
        public void AddLocation_DeleteFile(DeleteFileParam _DeleteFile, string ConnectionString)
        {
            new BusinessLayer.BL_MapData().DeleteFile(_DeleteFile, ConnectionString);
        }
        public List<GetAlertTypeViewModel> AddLocation_GetAlertType(GetAlertTypeParam _GetAlertType, string ConnectionString)
        {
            return new BusinessLayer.BL_Alerts().GetAlertType(_GetAlertType, ConnectionString);
        }
        public ListGetAlerts AddLocation_GetAlerts(GetAlertsParam _GetAlerts, string ConnectionString)
        {
            return new BusinessLayer.BL_Alerts().getAlerts(_GetAlerts, ConnectionString);
        }
        public ListGetDefaultRouteTerr AddLocation_GetDefaultRouteTerr(GetDefaultRouteTerrParam _GetDefaultRouteTerr, string ConnectionString)
        {
            return new BusinessLayer.BL_User().getDefaultRouteTerr(_GetDefaultRouteTerr, ConnectionString);
        }
        public List<GetGCCustomerViewModel> AddLocation_GetGCCustomer(GetGCCustomerParam _GetGCCustomer, string ConnectionString)
        {
            return new BusinessLayer.BL_User().getGCCustomer(_GetGCCustomer, ConnectionString);
        }
        public List<GetElevViewModel> AddLocation_GetElev(GetElevParam _GetElev, string ConnectionString, Int32 IsSalesAsigned = 0)
        {
            return new BusinessLayer.BL_User().getElev(_GetElev, ConnectionString, IsSalesAsigned);
        }
        public List<GetCallHistoryViewModel> AddLocation_GetCallHistory(GetCallHistoryParam _GetCallHistory, string ConnectionString, Int32 IsSalesAsigned = 0, int IsCallForTicketReport = 0)
        {
            return new BL_Tickets().getCallHistory(_GetCallHistory, ConnectionString, IsSalesAsigned, IsCallForTicketReport);
        }
        public ListGetARRevenue AddLocation_GetARRevenue(GetARRevenueParam _GetARRevenue, string ConnectionString)
        {
            return new BusinessLayer.BL_Invoice().GetARRevenue(_GetARRevenue, ConnectionString);
        }
        public List<GetJobProjectViewModel> AddLocation_GetJobProject(GetJobProjectParam _GetJobProject, string ConnectionString, Int32 IsSalesAsigned = 0, int IncludeClose = 1)
        {
            return new BusinessLayer.BL_Customer().getJobProject(_GetJobProject, ConnectionString, IsSalesAsigned, IncludeClose);
        }

        public List<GetLocationLogViewModel> AddLocation_GetLocationLog(GetLocationLogParam _GetLocationLog, string ConnectionString)
        {
            return new BusinessLayer.BL_User().getLocationLog(_GetLocationLog, ConnectionString);
        }

        public List<GetContactLogByLocIDViewModel> AddLocation_GetContactLogByLocID(GetContactLogByLocIDParam _GetContactLogByLocID, string ConnectionString)
        {
            return new BusinessLayer.BL_User().getContactLogByLocID(_GetContactLogByLocID, ConnectionString);
        }
        public List<GetLocContactByRolIDViewModel> AddLocation_GetLocContactByRolID(GetLocContactByRolIDParam _GetLocContactByRolID, string ConnectionString)
        {
            return new BusinessLayer.BL_User().getLocContactByRolID(_GetLocContactByRolID, ConnectionString);
        }
        public void AddLocation_DeleteOpportunity(DeleteOpportunityParam _DeleteOpportunity, string ConnectionString)
        {
            new BusinessLayer.BL_Customer().DeleteOpportunity(_DeleteOpportunity, ConnectionString);
        }
        public List<GetOpportunityNewViewModel> AddLocation_GetOpportunityNew(GetOpportunityNewParam _GetOpportunityNew, string ConnectionString, Int32 IsSalesAsigned = 0)
        {
            return new BusinessLayer.BL_Customer().getOpportunityNew(_GetOpportunityNew, ConnectionString, IsSalesAsigned);
        }
        public List<GetLocationServiceTypeinfoViewModel> AddLocation_spGetLocationServiceTypeinfo(spGetLocationServiceTypeinfoParam _GetLocationServiceTypeinfo, string ConnectionString)
        {
            return new BusinessLayer.Programs.BL_ServiceType().spGetLocationServiceTypeinfo(_GetLocationServiceTypeinfo, ConnectionString);
        }


        /// <summary>
        /// For Locations Report Screen : AcctLabels5160.aspx / AcctLabels5160.aspx.cs
        /// </summary>
        /// API's Naming Conventions : LocationReport_Method Name(Parameter)
        /// 

        public List<GetCompanyDetailsViewModel> LocationReport_GetCompanyDetails(GetCompanyDetailsParam _GetCompanyDetails, string ConnectionString)
        {
            return new BusinessLayer.BL_Report().GetCompanyDetails(_GetCompanyDetails, ConnectionString);
        }

        public List<GetAccountLabelViewModel> LocationReport_GetAccountLabel(GetAccountLabelParam _GetAccountLabel, string ConnectionString, Int32 IsSalesAsigned = 0)
        {
            return new BusinessLayer.BL_Report().GetAccountLabel(_GetAccountLabel, ConnectionString, IsSalesAsigned);
        }

        public List<SMTPEmailViewModel> LocationReport_GetSMTPByUserID(GetSMTPByUserIDParam _GetSMTPByUserID, string ConnectionString)
        {
            return new BusinessLayer.BL_User().getSMTPByUserID(_GetSMTPByUserID, ConnectionString);
        }



        /// <summary>
        /// For Locations Report Screen : LocationEquipmentListReport.aspx / LocationEquipmentListReport.aspx.cs 
        /// </summary>
        /// API's Naming Conventions : LocationReport_Method Name(Parameter)
        /// 

        public List<GetLocationEquipmentListViewModel> LocationReport_GetLocationEquipmentList(GetLocationEquipmentListParam _GetLocationEquipmentList, string ConnectionString, List<RetainFilter> filters, bool includeInactive)
        {
            return new BusinessLayer.BL_Report().GetLocationEquipmentList(_GetLocationEquipmentList, ConnectionString, filters, includeInactive);
        }


        /// <summary>
        /// For Location Business Type Report Screen : LocationBusinessTypeReport.aspx / LocationBusinessTypeReport.aspx.cs 
        /// </summary>
        /// API's Naming Conventions : LocationReport_Method Name(Parameter)
        /// 

        public ListGetLocationByBusinessType LocationReport_GetLocationByBusinessType(GetLocationByBusinessTypeParam _GetLocationByBusinessType, string ConnectionString, List<RetainFilter> filters, bool includeInactive)
        {
            return new BusinessLayer.BL_Report().GetLocationByBusinessType(_GetLocationByBusinessType, ConnectionString, filters, includeInactive);
        }

        /// <summary>
        /// For Location Business Type Report Screen : LocationBusinessTypeReport.aspx / LocationBusinessTypeReport.aspx.cs 
        /// </summary>
        /// API's Naming Conventions : LocationReport_Method Name(Parameter)
        /// 

        public List<GetLocationDetailsReportViewModel> LocationReport_GetLocationDetailsReport(GetLocationDetailsReportParam _GetLocationDetailsReport, string ConnectionString, List<RetainFilter> filters, bool includeInactive)
        {
            return new BusinessLayer.BL_Report().GetLocationDetailsReport(_GetLocationDetailsReport, ConnectionString, filters, includeInactive);
        }


        /// <summary>
        /// For Location Report Screen : LocationReport.aspx / LocationReport.aspx.cs 
        /// </summary>
        /// API's Naming Conventions : LocationReport_Method Name(Parameter)
        /// 

        public string LocationReport_GetUserEmail(GetUserEmailParam _GetUserEmail, string ConnectionString)
        {
            return new BusinessLayer.BL_User().getUserEmail(_GetUserEmail, ConnectionString);
        }

        public List<CustomerReportViewModel> LocationReport_GetReportDetailById(GetReportDetailByIdParam _GetReportDetailByIdParam, string ConnectionString)
        {
            return new BusinessLayer.BL_ReportsData().GetReportDetailById(_GetReportDetailByIdParam, ConnectionString);
        }
        public ListGetLocationReportFiltersValue LocationReport_GetLocationReportFiltersValue(GetLocationReportFiltersValueParam _GetLocationReportFiltersValue, string ConnectionString)
        {
            return new BusinessLayer.BL_ReportsData().GetLocationReportFiltersValue(_GetLocationReportFiltersValue, ConnectionString);
        }
        public List<CustomerReportViewModel> LocationReport_GetDynamicReports(GetDynamicReportsParam _GetDynamicReportsParam, string ConnectionString)
        {
            return new BusinessLayer.BL_ReportsData().GetDynamicReports(_GetDynamicReportsParam, ConnectionString);
        }
        public List<CustomerFilterViewModel> LocationReport_GetReportColByRepId(GetReportColByRepIdParam _GetReportColByRepId, string ConnectionString)
        {
            return new BusinessLayer.BL_ReportsData().GetReportColByRepId(_GetReportColByRepId, ConnectionString);
        }
        public List<CustomerFilterViewModel> LocationReport_GetReportFiltersByRepId(GetReportFiltersByRepIdParam _GetReportFiltersByRepId, string ConnectionString)
        {
            return new BusinessLayer.BL_ReportsData().GetReportFiltersByRepId(_GetReportFiltersByRepId, ConnectionString);
        }

        public ListGetLocationDetails LocationReport_GetLocationDetails(GetLocationDetailsParam _GetLocationDetails, string ConnectionString)
        {
            return new BusinessLayer.BL_ReportsData().GetLocationDetails(_GetLocationDetails, ConnectionString);
        }
        public List<UserViewModel> LocationReport_GetAccountSummaryListingDetail(GetAccountSummaryListingDetailParam _GetAccountSummaryListingDetail, string ConnectionString)
        {
            return new BusinessLayer.BL_ReportsData().GetAccountSummaryListingDetail(_GetAccountSummaryListingDetail, ConnectionString);
        }
        public bool LocationReport_CheckExistingReport(CheckExistingReportParam _CheckExistingReport, string ConnectionString)
        {
            return new BusinessLayer.BL_ReportsData().CheckExistingReport(_CheckExistingReport, ConnectionString);
        }
        public List<CustomerReportViewModel> LocationReport_InsertCustomerReport(InsertCustomerReportParam _InsertCustomerReport, string ConnectionString)
        {
            return new BusinessLayer.BL_ReportsData().InsertCustomerReport(_InsertCustomerReport, ConnectionString);
        }
        public bool LocationReport_IsStockReportExist(IsStockReportExistParam _IsStockReportExist, string ConnectionString)
        {
            return new BusinessLayer.BL_ReportsData().IsStockReportExist(_IsStockReportExist, ConnectionString);
        }
        public void LocationReport_UpdateCustomerReport(UpdateCustomerReportParam _UpdateCustomerReport, string ConnectionString)
        {
            new BusinessLayer.BL_ReportsData().UpdateCustomerReport(_UpdateCustomerReport, ConnectionString);
        }
        public void LocationReport_DeleteCustomerReport(DeleteCustomerReportParam _DeleteCustomerReport, string ConnectionString)
        {
            new BusinessLayer.BL_ReportsData().DeleteCustomerReport(_DeleteCustomerReport, ConnectionString);
        }
        public List<CustomerFilterViewModel> LocationReport_GetControlForReports(getConnectionConfigParam _getConnectionConfig, string ConnectionString)
        {
            return new BusinessLayer.BL_ReportsData().GetControlForReports(_getConnectionConfig, ConnectionString);
        }
        public List<HeaderFooterDetailViewModel> LocationReport_GetHeaderFooterDetail(GetHeaderFooterDetailParam _GetHeaderFooterDetail, string ConnectionString)
        {
            return new BusinessLayer.BL_ReportsData().GetHeaderFooterDetail(_GetHeaderFooterDetail, ConnectionString);
        }
        public List<CustomerFilterViewModel> LocationReport_GetOwners(GetOwnersParam _GetOwnersParam, string ConnectionString)
        {
            return new BusinessLayer.BL_ReportsData().GetOwners(_GetOwnersParam, ConnectionString);
        }
        public List<CustomerReportViewModel> LocationReport_GetColumnWidthByReportId(GetColumnWidthByReportIdParam _GetColumnWidthByReportId, string ConnectionString)
        {
            return new BusinessLayer.BL_ReportsData().GetColumnWidthByReportId(_GetColumnWidthByReportId, ConnectionString);
        }
        public void LocationReport_UpdateCustomerReportResizedWidth(UpdateCustomerReportResizedWidthParam _UpdateCustomerReportResizedWidth, string ConnectionString)
        {
            new BusinessLayer.BL_ReportsData().UpdateCustomerReportResizedWidth(_UpdateCustomerReportResizedWidth, ConnectionString);
        }












    }

}
