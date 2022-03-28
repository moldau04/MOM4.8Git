using BusinessEntity;
using BusinessEntity.APModels;
using BusinessEntity.CustomersModel;
using BusinessEntity.payroll;
using BusinessEntity.Payroll;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MOMWebAPI.Areas.Customers.Controllers
{
    public interface ILocationsRepository
    {

        /// <summary>
        /// For Locations List Screen : Locations.aspx / Locations.aspx.cs
        /// </summary>
        /// API's Naming Conventions : LocationsList_Method Name(Parameter)
        /// 

        public List<CompanyOfficeViewModel> LocationsList_GetCompanyByCustomer(GetCompanyByCustomerParam _GetCompanyByCustomer, string ConnectionString);
        public List<GetZoneViewModel> LocationsList_GetZone(GetZoneParam _GetZone, string ConnectionString);
        public List<GetTerritoryViewModel> LocationsList_GetTerritory(GetTerritoryParam _GetTerritory, string ConnectionString);
        public List<CustomViewModel> LocationsList_GetCustomFieldsControl(getCustomFieldsControlParam _getCustomFieldsControlParam, string ConnectionString);
        public List<CustomerReportViewModel> LocationsList_GetStockReports(GetStockReportsParam _GetStockReports, string ConnectionString);
        public void LocationsList_DeleteLocation(DeleteLocationParam _DeleteLocation, string ConnectionString);
        public List<GetLocationDataSearchViewModel> LocationsList_GetLocationDataSearch(GetLocationDataSearchParam _GetLocationDataSearch, string ConnectionString, Int32 IsSalesAsigned = 0);
        public List<GetLocationDataSearchViewModel> LocationsList_GetLocationsData(GetLocationsDataParam _GetLocationsData, string ConnectionString, Int32 IsSalesAsigned = 0);
        public List<GetLocationTypeViewModel> LocationsList_GridGetLocationType(GetLocationTypeParam _GetLocationType, string ConnectionString);
        public void LocationsList_ImportDataForMassAttachDocuments(ImportDataForMassAttachDocumentsParam _ImportDataForMassAttachDocuments, string ConnectionString, DataTable dataTable);
        public string LocationsList_GetDefaultWorkerHeader(GetDefaultWorkerHeaderParam _GetDefaultWorkerHeader, string ConnectionString);
        public List<GetLocationTypeViewModel> LocationsList_getlocationType(getlocationTypeParam _getlocationType, string ConnectionString);
        public ListGetRouteViewModel LocationsList_GetRoute(GetRouteParam _GetRoute, string ConnectionString, int IsActive = 0, Int32 LocID = 0, Int32 ContractID = 0);
        public List<GetBTViewModel> LocationsList_GetBT(GetBTParam _GetBT, string ConnectionString);


        /// <summary>
        /// For Add Locations Screen : AddLocation.aspx / AddLocation.aspx.cs
        /// </summary>
        /// API's Naming Conventions : AddLocation_Method Name(Parameter)
        /// 

        public List<GeneralViewModel> AddLocation_GetSagelatsync(getConnectionConfigParam _getConnectionConfig, string ConnectionString);
        public List<GetSingleConsultantViewModel> AddLocation_GetSingleConsultant(GetSingleConsultantParam _GetSingleConsultant, string ConnectionString);
        public ListGetLocationByID AddLocation_GetLocationByID(GetLocationByIDParam _GetLocationByID, string ConnectionString);
        public ListGetCustomerByID AddLocation_GetCustomerByID(GetCustomerByIDParam _GetCustomerByID, Int32 IsSalesAsigned, string ConnectionString);
        public ListGetGCandHowerLocID AddLocation_GetGCandHowerLocID(GetGCandHowerLocIDParam _GetGCandHowerLocID, string ConnectionString);

        //public List<UserViewModel> CustomersList_GetUserPermissionByUserID(GetUserByIdParam _GetUserById, string ConnectionString);
        public List<TermsViewModel> AddLocation_GetTerms(GetTermsParam _GetTermsParam, string ConnectionString);
        public List<GetControlViewModel> AddLocation_GetControl(getConnectionConfigParam _user, string ConnectionString);
        public ListGetProspectByID AddLocation_GetProspectByID(GetProspectByIDParam _GetProspectByID, string ConnectionString);
        public List<GetCategoryViewModel> AddLocation_GetCategory(GetCategoryParam _GetCategory, string ConnectionString);
        public List<GetCustomersViewModel> AddLocation_GetCustomers(GetCustomersParam _GetCustomers, Int32 IsSalesAsigned, string ConnectionString);
        public List<STaxViewModel> AddLocation_GetSTax(getSTaxParam _getSTaxParam, string ConnectionString);
        public List<getSalesTax2ViewModel> AddLocation_getSalesTax2(getSalesTax2Param _getSalesTax2, string ConnectionString);
        public List<STaxViewModel> AddLocation_GetUseTax(getUseTaxParam _getUseTaxParam, string ConnectionString);
        public List<CustomViewModel> AddLocation_GetCustomFields(getCustomFieldsParam _getCustomFieldsParam, string ConnectionString);
        public bool AddLocation_IsExistContractByLoc(IsExistContractByLocParam _IsExistContractByLoc, string ConnectionString);
        public void AddLocation_UpdateLocation(UpdateLocationParam _UpdateLocation, string ConnectionString, bool CopyToLocAndJob = false, int ApplyServiceTypeRule = 0, string ServiceTypeName = "", int ProjectPerDepartmentCount = 0);
        public int AddLocation_AddLocation(AddLocationParam _AddLocation, string ConnectionString);
        public void AddLocation_ConvertLeadEquipment(ConvertLeadEquipmentParam _ConvertLeadEquipment, string ConnectionString);
        public void AddLocation_UpdateLocationContactRecordLog(UpdateLocationContactRecordLogParam _UpdateLocationContactRecordLog, string ConnectionString);
        public void AddLocation_DeleteEquipment(DeleteEquipmentParam _DeleteEquipment, string ConnectionString);
        public List<JobTypeViewModel> AddLocation_GetDepartment(GetDepartmentParam _GetDepartment, string ConnectionString);
        public void AddLocation_AddFile(AddFileParam _AddFile, string ConnectionString);
        public void AddLocation_UpdateDocInfo(UpdateDocInfoParam _UpdateDocInfo, string ConnectionString);
        public List<GetLocationDocumentsViewModel> AddLocation_GetLocationDocuments(GetLocationDocumentsParam _GetLocationDocuments, string ConnectionString, bool isShowAll, bool isLocation);
        public void AddLocation_DeleteFile(DeleteFileParam _DeleteFile, string ConnectionString);
        public List<GetAlertTypeViewModel> AddLocation_GetAlertType(GetAlertTypeParam _GetAlertType, string ConnectionString);
        public ListGetAlerts AddLocation_GetAlerts(GetAlertsParam _GetAlerts, string ConnectionString);
        public ListGetDefaultRouteTerr AddLocation_GetDefaultRouteTerr(GetDefaultRouteTerrParam _GetDefaultRouteTerr, string ConnectionString);
        public List<GetGCCustomerViewModel> AddLocation_GetGCCustomer(GetGCCustomerParam _GetGCCustomer, string ConnectionString);
        public List<GetElevViewModel> AddLocation_GetElev(GetElevParam _GetElev, string ConnectionString, Int32 IsSalesAsigned = 0);
        public List<GetCallHistoryViewModel> AddLocation_GetCallHistory(GetCallHistoryParam _GetCallHistory, string ConnectionString, Int32 IsSalesAsigned = 0, int IsCallForTicketReport = 0);
        public ListGetARRevenue AddLocation_GetARRevenue(GetARRevenueParam _GetARRevenue, string ConnectionString);
        public List<GetJobProjectViewModel> AddLocation_GetJobProject(GetJobProjectParam _GetJobProject, string ConnectionString, Int32 IsSalesAsigned = 0, int IncludeClose = 1);
        public List<GetLocationLogViewModel> AddLocation_GetLocationLog(GetLocationLogParam _GetLocationLog, string ConnectionString); 
        public List<GetContactLogByLocIDViewModel> AddLocation_GetContactLogByLocID(GetContactLogByLocIDParam _GetContactLogByLocID, string ConnectionString);
        public List<GetLocContactByRolIDViewModel> AddLocation_GetLocContactByRolID(GetLocContactByRolIDParam _GetLocContactByRolID, string ConnectionString);
        public void AddLocation_DeleteOpportunity(DeleteOpportunityParam _DeleteOpportunity, string ConnectionString);
        public List<GetOpportunityNewViewModel> AddLocation_GetOpportunityNew(GetOpportunityNewParam _GetOpportunityNew, string ConnectionString, Int32 IsSalesAsigned = 0);
        public List<GetLocationServiceTypeinfoViewModel> AddLocation_spGetLocationServiceTypeinfo(spGetLocationServiceTypeinfoParam _GetLocationServiceTypeinfo, string ConnectionString);



        /// <summary>
        /// For Locations Report Screen : AcctLabels5160.aspx / AcctLabels5160.aspx.cs /AcctLabels5163.aspx /AcctLabels5163.aspx.cs
        /// </summary>
        /// API's Naming Conventions : LocationReport_Method Name(Parameter)
        /// 

        public List<GetCompanyDetailsViewModel> LocationReport_GetCompanyDetails(GetCompanyDetailsParam _GetCompanyDetails, string ConnectionString);
        public List<GetAccountLabelViewModel> LocationReport_GetAccountLabel(GetAccountLabelParam _GetAccountLabel, string ConnectionString, Int32 IsSalesAsigned = 0);

        public List<SMTPEmailViewModel> LocationReport_GetSMTPByUserID(GetSMTPByUserIDParam _GetSMTPByUserID, string ConnectionString);


        /// <summary>
        /// For Location Equipment List Report Screen : LocationEquipmentListReport.aspx / LocationEquipmentListReport.aspx.cs 
        /// </summary>
        /// API's Naming Conventions : LocationReport_Method Name(Parameter)
        /// 

        public List<GetLocationEquipmentListViewModel> LocationReport_GetLocationEquipmentList(GetLocationEquipmentListParam _GetLocationEquipmentList, string ConnectionString, List<RetainFilter> filters, bool includeInactive);


        /// <summary>
        /// For Location Business Type Report Screen : LocationBusinessTypeReport.aspx / LocationBusinessTypeReport.aspx.cs 
        /// </summary>
        /// API's Naming Conventions : LocationReport_Method Name(Parameter)
        /// 

        public ListGetLocationByBusinessType LocationReport_GetLocationByBusinessType(GetLocationByBusinessTypeParam _GetLocationByBusinessType, string ConnectionString, List<RetainFilter> filters, bool includeInactive);



        /// <summary>
        /// For Location Details Report Screen : LocationDetailsReport.aspx / LocationDetailsReport.aspx.cs 
        /// </summary>
        /// API's Naming Conventions : LocationReport_Method Name(Parameter)
        /// 

        public List<GetLocationDetailsReportViewModel> LocationReport_GetLocationDetailsReport(GetLocationDetailsReportParam _GetLocationDetailsReport, string ConnectionString, List<RetainFilter> filters, bool includeInactive);


        /// <summary>
        /// For Location Report Screen : LocationReport.aspx / LocationReport.aspx.cs 
        /// </summary>
        /// API's Naming Conventions : LocationReport_Method Name(Parameter)
        /// 

        public string LocationReport_GetUserEmail(GetUserEmailParam _GetUserEmail, string ConnectionString);
        public List<CustomerReportViewModel> LocationReport_GetReportDetailById(GetReportDetailByIdParam _GetReportDetailByIdParam, string ConnectionString);
        public ListGetLocationReportFiltersValue LocationReport_GetLocationReportFiltersValue(GetLocationReportFiltersValueParam _GetLocationReportFiltersValue, string ConnectionString);
        public List<CustomerReportViewModel> LocationReport_GetDynamicReports(GetDynamicReportsParam _GetDynamicReportsParam, string ConnectionString);
        public List<CustomerFilterViewModel> LocationReport_GetReportColByRepId(GetReportColByRepIdParam _GetReportColByRepId, string ConnectionString);
        public List<CustomerFilterViewModel> LocationReport_GetReportFiltersByRepId(GetReportFiltersByRepIdParam _GetReportFiltersByRepId, string ConnectionString);
        public ListGetLocationDetails LocationReport_GetLocationDetails(GetLocationDetailsParam _GetLocationDetails, string ConnectionString);
        public List<UserViewModel> LocationReport_GetAccountSummaryListingDetail(GetAccountSummaryListingDetailParam _GetAccountSummaryListingDetailParam, string ConnectionString);
        public bool LocationReport_CheckExistingReport(CheckExistingReportParam _CheckExistingReportParam, string ConnectionString);
        public List<CustomerReportViewModel> LocationReport_InsertCustomerReport(InsertCustomerReportParam _InsertCustomerReportParam, string ConnectionString);
        public bool LocationReport_IsStockReportExist(IsStockReportExistParam _IsStockReportExistParam, string ConnectionString);
        public void LocationReport_UpdateCustomerReport(UpdateCustomerReportParam _UpdateCustomerReportParam, string ConnectionString);
        public void LocationReport_DeleteCustomerReport(DeleteCustomerReportParam _DeleteCustomerReport, string ConnectionString);
        public List<CustomerFilterViewModel> LocationReport_GetControlForReports(getConnectionConfigParam _getConnectionConfigParam, string ConnectionString);
        public List<HeaderFooterDetailViewModel> LocationReport_GetHeaderFooterDetail(GetHeaderFooterDetailParam _CustomerReportParam, string ConnectionString);
        public List<CustomerFilterViewModel> LocationReport_GetOwners(GetOwnersParam _GetOwnersParam, string ConnectionString);
        public List<CustomerReportViewModel> LocationReport_GetColumnWidthByReportId(GetColumnWidthByReportIdParam _GetColumnWidthByReportId, string ConnectionString);
        public void LocationReport_UpdateCustomerReportResizedWidth(UpdateCustomerReportResizedWidthParam _UpdateCustomerReportResizedWidth, string ConnectionString);
    }
}
