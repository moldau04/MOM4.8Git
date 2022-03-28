using BusinessEntity;
using BusinessEntity.APModels;
using BusinessEntity.CustomersModel;
using BusinessEntity.InventoryModel;
using BusinessEntity.Payroll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MOMWebAPI.Areas.Customers.Controllers
{
    public interface IEquipmentRepository
    {

        /// <summary>
        /// For Equipment List Screen : Equipments.aspx / Equipments.aspx.cs
        /// </summary>
        /// API's Naming Conventions : EquipmentsList_Method Name(Parameter)
        /// 

        //public List<UserViewModel> EquipmentsList_GetUserPermissionByUserID(GetUserByIdParam _GetUserById, string ConnectionString);
        public List<CustomerReportViewModel> EquipmentsList_GetStockReports(GetStockReportsParam _GetStockReports, string ConnectionString);
        public List<GetEquiptypeViewModel> EquipmentsList_GetEquiptype(GetEquiptypeParam _GetEquiptype, string ConnectionString);
        public List<GetEquiptypeViewModel> EquipmentsList_GetEquipmentCategory(GetEquipmentCategoryParam _GetEquipmentCategory, string ConnectionString);
        public List<GetActiveServiceTypeViewModel> EquipmentsList_GetActiveServiceType(GetActiveServiceTypeParam _GetActiveServiceType, string ConnectionString);
        public List<GetCompanyByUserIDViewModel> EquipmentsList_GetCompanyByUserID(GetCompanyByUserIDParam _GetCompanyByUserID, string ConnectionString);
        public List<GetBuildingElevViewModel> EquipmentsList_GetBuildingElev(GetBuildingElevParam _GetBuildingElev, string ConnectionString);
        public List<GetEquipmentViewModel> EquipmentsList_GetEquipment(GetEquipmentParam _GetEquipment, string ConnectionString, Int32 IsSalesAsigned = 0);
        public void EquipmentsList_DeleteEquipment(DeleteEquipmentParam _DeleteEquipment, string ConnectionString);
        public List<GetEquiptypeViewModel> EquipmentsList_GetEquipClassification(GetEquipClassificationParam _GetEquipClassification, string ConnectionString);


        /// <summary>
        /// For QRPrint Report Screen : QRPrint.aspx / QRPrint.aspx.cs
        /// </summary>
        /// API's Naming Conventions : EquipmentReport_Method Name(Parameter)
        /// 

        public List<GetCompanyDetailsViewModel> EquipmentReport_GetCompanyDetails(GetCompanyDetailsParam _GetCompanyDetails, string ConnectionString);
        public List<GetControlViewModel> EquipmentReport_GetControl(getConnectionConfigParam _user, string ConnectionString);
        public List<SMTPEmailViewModel> EquipmentReport_GetSMTPByUserID(GetSMTPByUserIDParam _GetSMTPByUserID, string ConnectionString);


        /// <summary>
        /// For MassMCP Screen : MassMCP.aspx / MassMCP.aspx.cs
        /// </summary>
        /// API's Naming Conventions : MassMCP_Method Name(Parameter)
        /// 

        public List<GetElevViewModel> MassMCP_GetElev(GetElevParam _GetElev, string ConnectionString, Int32 IsSalesAsigned = 0);
        public List<GetRepTemplateNameViewModel> MassMCP_GetRepTemplateName(GetRepTemplateNameParam _GetRepTemplateName, string ConnectionString);
        public List<GetTemplateItemByIDViewModel> MassMCP_GetTemplateItemByID(GetTemplateItemByIDParam _GetTemplateItemByID, string ConnectionString);
        public void MassMCP_AddMassMCP(AddMassMCPParam _AddMassMCP, string ConnectionString);


        /// <summary>
        /// For Add Equipment Screen : AddEquipment.aspx / AddEquipment.aspx.cs
        /// </summary>
        /// API's Naming Conventions : AddEquipment_Method Name(Parameter)
        /// 
        public ListGetLeadEquipByID AddEquipment_GetLeadEquipByID(GetLeadEquipByIDParam _GetLeadEquipByID, string ConnectionString);
        public ListGetequipByID AddEquipment_GetequipByID(GetequipByIDParam _GetequipByID, string ConnectionString);
        public List<GetBuildingElevViewModel> AddEquipment_GetBuildingLeadEquip(GetBuildingLeadEquipParam _GetBuildingLeadEquip, string ConnectionString);
        public List<GetequipREPDetailsViewModel> AddEquipment_GetequipREPDetails(GetequipREPDetailsParam _GetequipREPDetails, string ConnectionString);
        public List<GetCustomTemplateViewModel> AddEquipment_GetCustomTemplate(GetCustomTemplateParam _GetCustomTemplate, string ConnectionString);
        public List<GetEquiptypeViewModel> AddEquipment_GetLeadEquipmentCategory(GetLeadEquipmentCategoryParam _GetLeadEquipmentCategory, string ConnectionString);
        public List<GetEquiptypeViewModel> AddEquipment_GetLeadEquiptype(GetLeadEquiptypeParam _GetLeadEquiptype, string ConnectionString);
        public void AddEquipment_UpdateDocInfo(UpdateDocInfoParam _UpdateDocInfo, string ConnectionString);
        public void AddEquipment_UpdateLeadEquipment(UpdateLeadEquipmentParam _UpdateLeadEquipment, string ConnectionString);
        public void AddEquipment_UpdateEquipment(UpdateEquipmentParam _UpdateEquipment, string ConnectionString);
        public Int32 AddEquipment_AddEquipmentForLead(AddEquipmentForLeadParam _AddEquipmentForLead, string ConnectionString);
        public Int32 AddEquipment_AddEquipment(AddEquipmentParam _AddEquipment, string ConnectionString);
        public ListGetCustTemplateItemByID AddEquipment_GetCustTemplateItemByID(GetCustTemplateItemByIDParam _GetCustTemplateItemByID, string ConnectionString);
        public string AddEquipment_GetContractType(GetContractTypeParam _GetContractType, string ConnectionString);
        public List<GetEquipmentTestsViewModel> AddEquipment_GetEquipmentTests(GetEquipmentTestsParam _GetEquipmentTests, string ConnectionString);
        public ListGetLocationByID AddEquipment_GetLocationByID(GetLocationByIDParam _GetLocationByID, string ConnectionString);
        public void AddEquipment_AddFile(AddFileParam _AddFile, string ConnectionString);
        public void AddEquipment_DeleteFile(DeleteFileParam _DeleteFile, string ConnectionString);
        public List<GetDocumentsViewModel> AddEquipment_GetDocuments(GetDocumentsParam _GetDocuments, string ConnectionString);
        public List<GetEquiptypeViewModel> AddEquipment_GetLeadEquipClassification(GetLeadEquipClassificationParam _GetLeadEquipClassification, string ConnectionString);
        public List<GetShutdownReasonsViewModel> AddEquipment_GetShutdownReasons(GetShutdownReasonsParam _GetShutdownReasons, string ConnectionString);
        public List<GetShutdownReasonsViewModel> AddEquipment_GetShutdownReasonByID(GetShutdownReasonByIDParam _GetShutdownReasonByID, string ConnectionString, int eqsdReasonID);
        public void AddEquipment_AddShutdownReason(AddShutdownReasonParam _AddShutdownReason, string ConnectionString, string eqsdReason, bool eqsdPlanned);
        public void AddEquipment_EditShutdownReason(EditShutdownReasonParam _EditShutdownReason, string ConnectionString, int eqsdID, string eqsdReason, bool eqsdPlanned);
        public List<GetEquipShutdownLogsViewModel> AddEquipment_GetEquipShutdownLogs(GetEquipShutdownLogsParam _GetEquipShutdownLogs, string ConnectionString);
        public List<GetLocationByCustomerIDViewModel> AddEquipment_GetLocationByCustomerID(GetLocationByCustomerIDParam _GetLocationByCustomerID, string ConnectionString);



        /// <summary>
        /// For Reports Equipment Screen : EquipmentShutdownReport.aspx / EquipmentShutdownReport.aspx.cs
        /// </summary>
        /// API's Naming Conventions : EquipmentReport_Method Name(Parameter)
        /// 

        public List<GetEquipmentShutdownForReportViewModel> EquipmentReport_GetEquipmentShutdownForReport(GetEquipmentShutdownForReportParam _GetEquipmentShutdownForReport, string ConnectionString, DateTime endDate);
        public ListGetEquipShutdownActivityForReport EquipmentReport_GetEquipmentShutdownActivityForReport(GetEquipShutdownActivityForReportParam _GetEquipShutdownActivityForReport, string ConnectionString, DateTime startDate, DateTime endDate, string eqId, bool filtered);


        /// <summary>
        /// For Reports Equipment Screen : MaintenanceEquipmentCount.aspx / MaintenanceEquipmentCount.aspx.cs
        /// </summary>
        /// API's Naming Conventions : EquipmentReport_Method Name(Parameter)
        /// 

        public List<GetMaintenanceUnitCountRouteViewModel> EquipmentReport_GetMaintenanceUnitCountRoute(GetMaintenanceUnitCountRouteParam _GetMaintenanceUnzitCountRoute, string ConnectionString);
        public List<GetMaintenanceUnitCountViewModel> EquipmentReport_GetMaintenanceUnitCount(GetMaintenanceUnitCountParam _GetMaintenanceUnitCount, string ConnectionString, string routes);


        /// <summary>
        /// For Reports Equipment Screen : PastDueMCPReport.aspx / PastDueMCPReport.aspx.cs
        /// </summary>
        /// API's Naming Conventions : EquipmentReport_Method Name(Parameter)
        /// 
        public List<GetPastDueMCPDataViewModel> EquipmentReport_GetPastDueMCPData(GetPastDueMCPDataParam _GetPastDueMCPData, string ConnectionString);


        /// <summary>
        /// For Reports Equipment Screen : EquipmentReport.aspx / EquipmentReport.aspx.cs
        /// </summary>
        /// API's Naming Conventions : EquipmentReport_Method Name(Parameter)
        /// 
        public string EquipmentReport_GetUserEmail(GetUserEmailParam _GetUserEmail, string ConnectionString);
        public List<CustomerReportViewModel> EquipmentReport_GetReportDetailById(GetReportDetailByIdParam _GetReportDetailByIdParam, string ConnectionString);
        public List<CustomerReportViewModel> EquipmentReport_GetCustomerType(GetCustomerTypeParam _getCustomerType, string ConnectionString);
        public ListGetEquipReportFiltersValue EquipmentReport_GetEquipReportFiltersValue(GetEquipReportFiltersValueParam _GetEquipReportFiltersValue, string ConnectionString);
        public List<CustomerFilterViewModel> EquipmentReport_GetCustomerName(GetCustomerNameParam _GetCustomerNameParam, string ConnectionString);
        public List<CustomerFilterViewModel> EquipmentReport_GetCustomerAddress(GetCustomerAddressParam _GetCustomerAddressParam, string ConnectionString);
        public List<CustomerFilterViewModel> EquipmentReport_GetCustomerCity(GetCustomerCityParam _GetCustomerCityParam, string ConnectionString); 
        public List<CustomerReportViewModel> EquipmentReport_GetDynamicReports(GetDynamicReportsParam _GetDynamicReportsParam, string ConnectionString);
        public List<CustomerFilterViewModel> EquipmentReport_GetReportColByRepId(GetReportColByRepIdParam _GetReportColByRepId, string ConnectionString);
        public List<CustomerFilterViewModel> EquipmentReport_GetReportFiltersByRepId(GetReportFiltersByRepIdParam _GetReportFiltersByRepId, string ConnectionString);
        public ListGetEquipmentInspection EquipmentReport_GetEquipmentInspection(GetEquipmentInspectionParam _GetEquipmentInspection, string ConnectionString);
        public List<UserViewModel> EquipmentReport_GetAccountSummaryListingDetail(GetAccountSummaryListingDetailParam _GetAccountSummaryListingDetailParam, string ConnectionString);
        public bool EquipmentReport_CheckExistingReport(CheckExistingReportParam _CheckExistingReportParam, string ConnectionString);
        public List<CustomerReportViewModel> EquipmentReport_InsertCustomerReport(InsertCustomerReportParam _InsertCustomerReportParam, string ConnectionString);
        public bool EquipmentReport_IsStockReportExist(IsStockReportExistParam _IsStockReportExistParam, string ConnectionString);
        public void EquipmentReport_UpdateCustomerReport(UpdateCustomerReportParam _UpdateCustomerReportParam, string ConnectionString);
        public void EquipmentReport_DeleteCustomerReport(DeleteCustomerReportParam _DeleteCustomerReport, string ConnectionString);
        public List<CustomerFilterViewModel> EquipmentReport_GetControlForReports(getConnectionConfigParam _getConnectionConfigParam, string ConnectionString);
        public List<HeaderFooterDetailViewModel> EquipmentReport_GetHeaderFooterDetail(GetHeaderFooterDetailParam _GetHeaderFooterDetail, string ConnectionString);
        public List<CustomerFilterViewModel> EquipmentReport_GetOwners(GetOwnersParam _GetOwnersParam, string ConnectionString);
        public List<CustomerReportViewModel> EquipmentReport_GetColumnWidthByReportId(GetColumnWidthByReportIdParam _GetColumnWidthByReportId, string ConnectionString);
        public void EquipmentReport_UpdateCustomerReportResizedWidth(UpdateCustomerReportResizedWidthParam _UpdateCustomerReportResizedWidth, string ConnectionString);
    }
}
