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
    public class EquipmentRepository : IEquipmentRepository
    {
        /// <summary>
        /// For Equipment List Screen : Equipments.aspx / Equipments.aspx.cs
        /// </summary>
        /// API's Naming Conventions : EquipmentsList_Method Name(Parameter)
        /// 

        public List<CustomerReportViewModel> EquipmentsList_GetStockReports(GetStockReportsParam _GetStockReportsParam, string connectionString)
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
        public List<GetEquiptypeViewModel> EquipmentsList_GetEquiptype(GetEquiptypeParam _GetEquiptype, string ConnectionString)
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

        public List<GetEquiptypeViewModel> EquipmentsList_GetEquipmentCategory(GetEquipmentCategoryParam _GetEquipmentCategory, string ConnectionString)
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
        public List<GetActiveServiceTypeViewModel> EquipmentsList_GetActiveServiceType(GetActiveServiceTypeParam _GetActiveServiceType, string ConnectionString)
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

        public List<GetCompanyByUserIDViewModel> EquipmentsList_GetCompanyByUserID(GetCompanyByUserIDParam _GetCompanyByUserID, string ConnectionString)
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

        public List<GetBuildingElevViewModel> EquipmentsList_GetBuildingElev(GetBuildingElevParam _GetBuildingElev, string ConnectionString)
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

        public List<GetEquipmentViewModel> EquipmentsList_GetEquipment(GetEquipmentParam _GetEquipment, string ConnectionString, Int32 IsSalesAsigned = 0)
        {
            try
            {
                return new BusinessLayer.BL_User().GetEquipment(_GetEquipment, ConnectionString, IsSalesAsigned);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void EquipmentsList_DeleteEquipment(DeleteEquipmentParam _DeleteEquipment, string ConnectionString)
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

        public List<GetEquiptypeViewModel> EquipmentsList_GetEquipClassification(GetEquipClassificationParam _GetEquipClassification, string ConnectionString)
        {
            try
            {
                return new BusinessLayer.BL_User().getEquipClassification(_GetEquipClassification, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// For QRPrint List Screen : QRPrint.aspx / QRPrint.aspx.cs
        /// </summary>
        /// API's Naming Conventions : EquipmentReport_Method Name(Parameter)
        /// 

        public List<GetCompanyDetailsViewModel> EquipmentReport_GetCompanyDetails(GetCompanyDetailsParam _GetCompanyDetails, string ConnectionString)
        {
            return new BusinessLayer.BL_Report().GetCompanyDetails(_GetCompanyDetails, ConnectionString);
        }
        public List<GetControlViewModel> EquipmentReport_GetControl(getConnectionConfigParam _getConnectionConfigParam, string ConnectionString)
        {
            return new BusinessLayer.BL_User().getControl(_getConnectionConfigParam, ConnectionString);
        }

        public List<SMTPEmailViewModel> EquipmentReport_GetSMTPByUserID(GetSMTPByUserIDParam _GetSMTPByUserID, string ConnectionString)
        {
            return new BusinessLayer.BL_User().getSMTPByUserID(_GetSMTPByUserID, ConnectionString);
        }


        /// <summary>
        /// For MassMCP Screen : MassMCP.aspx / MassMCP.aspx.cs
        /// </summary>
        /// API's Naming Conventions : MassMCP_Method Name(Parameter)
        /// 

        public List<GetElevViewModel> MassMCP_GetElev(GetElevParam _GetElev, string ConnectionString, Int32 IsSalesAsigned = 0)
        {
            return new BusinessLayer.BL_User().getElev(_GetElev, ConnectionString, IsSalesAsigned);
        }

        public List<GetRepTemplateNameViewModel> MassMCP_GetRepTemplateName(GetRepTemplateNameParam _GetRepTemplateName, string ConnectionString)
        {
            return new BusinessLayer.BL_Customer().getRepTemplateName(_GetRepTemplateName, ConnectionString);
        }

        public List<GetTemplateItemByIDViewModel> MassMCP_GetTemplateItemByID(GetTemplateItemByIDParam _GetTemplateItemByID, string ConnectionString)
        {
            return new BusinessLayer.BL_Customer().getTemplateItemByID(_GetTemplateItemByID, ConnectionString);
        }
        public void MassMCP_AddMassMCP(AddMassMCPParam _AddMassMCP, string ConnectionString)
        {
            new BusinessLayer.BL_User().AddMassMCP(_AddMassMCP, ConnectionString);
        }


        /// <summary>
        /// For Add Equipment Screen : AddEquipment.aspx / AddEquipment.aspx.cs
        /// </summary>
        /// API's Naming Conventions : AddEquipment_Method Name(Parameter)
        /// 

        public ListGetLeadEquipByID AddEquipment_GetLeadEquipByID(GetLeadEquipByIDParam _GetLeadEquipByID, string ConnectionString)
        {
            return new BusinessLayer.BL_User().getLeadEquipByID(_GetLeadEquipByID, ConnectionString);
        }

        public ListGetequipByID AddEquipment_GetequipByID(GetequipByIDParam _GetequipByID, string ConnectionString)
        {
            return new BusinessLayer.BL_User().getequipByID(_GetequipByID, ConnectionString);
        }
        public List<GetBuildingElevViewModel> AddEquipment_GetBuildingLeadEquip(GetBuildingLeadEquipParam _GetBuildingLeadEquip, string ConnectionString)
        {
            return new BusinessLayer.BL_User().getBuildingLeadEquip(_GetBuildingLeadEquip, ConnectionString);
        }
        public List<GetequipREPDetailsViewModel> AddEquipment_GetequipREPDetails(GetequipREPDetailsParam _GetequipREPDetails, string ConnectionString)
        {
            return new BusinessLayer.BL_User().getequipREPDetails(_GetequipREPDetails, ConnectionString);
        }
        public List<GetCustomTemplateViewModel> AddEquipment_GetCustomTemplate(GetCustomTemplateParam _GetCustomTemplate, string ConnectionString)
        {
            return new BusinessLayer.BL_Customer().getCustomTemplate(_GetCustomTemplate, ConnectionString);
        }
        public List<GetEquiptypeViewModel> AddEquipment_GetLeadEquipmentCategory(GetLeadEquipmentCategoryParam _GetLeadEquipmentCategory, string ConnectionString)
        {
            return new BusinessLayer.BL_User().getLeadEquipmentCategory(_GetLeadEquipmentCategory, ConnectionString);
        }
        public List<GetEquiptypeViewModel> AddEquipment_GetLeadEquiptype(GetLeadEquiptypeParam _GetLeadEquiptype, string ConnectionString)
        {
            return new BusinessLayer.BL_User().getLeadEquiptype(_GetLeadEquiptype, ConnectionString);
        }
        public void AddEquipment_UpdateDocInfo(UpdateDocInfoParam _UpdateDocInfo, string ConnectionString)
        {
            new BusinessLayer.BL_User().UpdateDocInfo(_UpdateDocInfo, ConnectionString);
        }
        public void AddEquipment_UpdateLeadEquipment(UpdateLeadEquipmentParam _UpdateLeadEquipment, string ConnectionString)
        {
            new BusinessLayer.BL_User().UpdateLeadEquipment(_UpdateLeadEquipment, ConnectionString);
        }
        public void AddEquipment_UpdateEquipment(UpdateEquipmentParam _UpdateEquipment, string ConnectionString)
        {
            new BusinessLayer.BL_User().UpdateEquipment(_UpdateEquipment, ConnectionString);
        }
        public Int32 AddEquipment_AddEquipmentForLead(AddEquipmentForLeadParam _AddEquipmentForLead, string ConnectionString)
        {
            return new BusinessLayer.BL_User().AddEquipmentForLead(_AddEquipmentForLead, ConnectionString);
        }
        public Int32 AddEquipment_AddEquipment(AddEquipmentParam _AddEquipment, string ConnectionString)
        {
            return new BusinessLayer.BL_User().AddEquipment(_AddEquipment, ConnectionString);
        }
        public ListGetCustTemplateItemByID AddEquipment_GetCustTemplateItemByID(GetCustTemplateItemByIDParam _GetCustTemplateItemByID, string ConnectionString)
        {
            return new BusinessLayer.BL_Customer().getCustTemplateItemByID(_GetCustTemplateItemByID, ConnectionString);
        }
        public string AddEquipment_GetContractType(GetContractTypeParam _GetContractType, string ConnectionString)
        {
            return new BusinessLayer.BL_User().getContractType(_GetContractType, ConnectionString);
        }
        public List<GetEquipmentTestsViewModel> AddEquipment_GetEquipmentTests(GetEquipmentTestsParam _GetEquipmentTests, string ConnectionString)
        {
            return new BusinessLayer.BL_User().GetEquipmentTests(_GetEquipmentTests, ConnectionString);
        }
        public ListGetLocationByID AddEquipment_GetLocationByID(GetLocationByIDParam _GetLocationByID, string ConnectionString)
        {
            return new BusinessLayer.BL_User().getLocationByID(_GetLocationByID, ConnectionString);
        }
        public void AddEquipment_AddFile(AddFileParam _AddFile, string ConnectionString)
        {
            new BusinessLayer.BL_MapData().AddFile(_AddFile, ConnectionString);
        }
        public void AddEquipment_DeleteFile(DeleteFileParam _DeleteFile, string ConnectionString)
        {
            new BusinessLayer.BL_MapData().DeleteFile(_DeleteFile, ConnectionString);
        }
        public List<GetDocumentsViewModel> AddEquipment_GetDocuments(GetDocumentsParam _GetDocuments, string ConnectionString)
        {
            return new BusinessLayer.BL_MapData().GetDocuments(_GetDocuments, ConnectionString);
        }
        public List<GetEquiptypeViewModel> AddEquipment_GetLeadEquipClassification(GetLeadEquipClassificationParam _GetLeadEquipClassification, string ConnectionString)
        {
            return new BusinessLayer.BL_User().getLeadEquipClassification(_GetLeadEquipClassification, ConnectionString);
        }
        public List<GetShutdownReasonsViewModel> AddEquipment_GetShutdownReasons(GetShutdownReasonsParam _GetShutdownReasons, string ConnectionString)
        {
            return new BusinessLayer.BL_User().GetShutdownReasons(_GetShutdownReasons, ConnectionString);
        }
        public List<GetShutdownReasonsViewModel> AddEquipment_GetShutdownReasonByID(GetShutdownReasonByIDParam _GetShutdownReasonByID, string ConnectionString, int eqsdReasonID)
        {
            return new BusinessLayer.BL_User().GetShutdownReasonByID(_GetShutdownReasonByID, ConnectionString, eqsdReasonID);
        }
        public void AddEquipment_AddShutdownReason(AddShutdownReasonParam _AddShutdownReason, string ConnectionString, string eqsdReason, bool eqsdPlanned)
        {
            new BusinessLayer.BL_User().AddShutdownReason(_AddShutdownReason, ConnectionString, eqsdReason, eqsdPlanned);
        }
        public void AddEquipment_EditShutdownReason(EditShutdownReasonParam _EditShutdownReason, string ConnectionString, int eqsdID, string eqsdReason, bool eqsdPlanned)
        {
            new BusinessLayer.BL_User().EditShutdownReason(_EditShutdownReason, ConnectionString, eqsdID, eqsdReason, eqsdPlanned);
        }
        public List<GetEquipShutdownLogsViewModel> AddEquipment_GetEquipShutdownLogs(GetEquipShutdownLogsParam _GetEquipShutdownLogs, string ConnectionString)
        {
            return new BusinessLayer.BL_User().GetEquipShutdownLogs(_GetEquipShutdownLogs, ConnectionString);
        }
        public List<GetLocationByCustomerIDViewModel> AddEquipment_GetLocationByCustomerID(GetLocationByCustomerIDParam _GetLocationByCustomerID, string ConnectionString)
        {
            return new BusinessLayer.BL_User().getLocationByCustomerID(_GetLocationByCustomerID, ConnectionString);
        }


        /// <summary>
        /// For Reports Equipment Screen : EquipmentShutdownReport.aspx / EquipmentShutdownReport.aspx.cs
        /// </summary>
        /// API's Naming Conventions : EquipmentReport_Method Name(Parameter)
        /// 
        public List<GetEquipmentShutdownForReportViewModel> EquipmentReport_GetEquipmentShutdownForReport(GetEquipmentShutdownForReportParam _GetEquipmentShutdownForReport, string ConnectionString, DateTime endDate)
        {
            return new BusinessLayer.BL_Customer().GetEquipmentShutdownForReport(_GetEquipmentShutdownForReport, ConnectionString, endDate);
        }
        public ListGetEquipShutdownActivityForReport EquipmentReport_GetEquipmentShutdownActivityForReport(GetEquipShutdownActivityForReportParam _GetEquipShutdownActivityForReport, string ConnectionString, DateTime startDate, DateTime endDate, string eqId, bool filtered)
        {
            return new BusinessLayer.BL_Customer().GetEquipmentShutdownActivityForReport(_GetEquipShutdownActivityForReport, ConnectionString, startDate, endDate, eqId, filtered);
        }


        /// <summary>
        /// For Reports Equipment Screen : MaintenanceEquipmentCount.aspx / MaintenanceEquipmentCount.aspx.cs
        /// </summary>
        /// API's Naming Conventions : EquipmentReport_Method Name(Parameter)
        /// 

        public List<GetMaintenanceUnitCountRouteViewModel> EquipmentReport_GetMaintenanceUnitCountRoute(GetMaintenanceUnitCountRouteParam _GetMaintenanceUnzitCountRoute, string ConnectionString)
        {
            return new BusinessLayer.BL_Report().GetMaintenanceUnitCountRoute(_GetMaintenanceUnzitCountRoute, ConnectionString);
        }
        public List<GetMaintenanceUnitCountViewModel> EquipmentReport_GetMaintenanceUnitCount(GetMaintenanceUnitCountParam _GetMaintenanceUnitCount, string ConnectionString, string routes)
        {
            return new BusinessLayer.BL_Report().GetMaintenanceUnitCount(_GetMaintenanceUnitCount, ConnectionString, routes);
        }

        /// <summary>
        /// For Reports Equipment Screen : PastDueMCPReport.aspx / PastDueMCPReport.aspx.cs
        /// </summary>
        /// API's Naming Conventions : EquipmentReport_Method Name(Parameter)
        /// 
        public List<GetPastDueMCPDataViewModel> EquipmentReport_GetPastDueMCPData(GetPastDueMCPDataParam _GetPastDueMCPData, string ConnectionString)
        {
            return new BusinessLayer.BL_Report().GetPastDueMCPData(_GetPastDueMCPData, ConnectionString);
        }

        /// <summary>
        /// For Reports Equipment Screen : EquipmentReport.aspx / EquipmentReport.aspx.cs
        /// </summary>
        /// API's Naming Conventions : EquipmentReport_Method Name(Parameter)
        /// 

        public string EquipmentReport_GetUserEmail(GetUserEmailParam _GetUserEmail, string ConnectionString)
        {
            return new BusinessLayer.BL_User().getUserEmail(_GetUserEmail, ConnectionString);
        }
        public List<CustomerReportViewModel> EquipmentReport_GetReportDetailById(GetReportDetailByIdParam _GetReportDetailByIdParam, string ConnectionString)
        {
            return new BusinessLayer.BL_ReportsData().GetReportDetailById(_GetReportDetailByIdParam, ConnectionString);
        }
        public List<CustomerReportViewModel> EquipmentReport_GetCustomerType(GetCustomerTypeParam _getCustomerType, string ConnectionString)
        {
            return new BusinessLayer.BL_ReportsData().GetCustomerType(_getCustomerType, ConnectionString);
        }
        public ListGetEquipReportFiltersValue EquipmentReport_GetEquipReportFiltersValue(GetEquipReportFiltersValueParam _GetEquipReportFiltersValue, string ConnectionString)
        {
            return new BusinessLayer.BL_ReportsData().GetEquipReportFiltersValue(_GetEquipReportFiltersValue, ConnectionString);
        }
        public List<CustomerFilterViewModel> EquipmentReport_GetCustomerName(GetCustomerNameParam _GetCustomerNameParam, string ConnectionString)
        {
            return new BusinessLayer.BL_ReportsData().GetCustomerName(_GetCustomerNameParam, ConnectionString);
        }
        public List<CustomerFilterViewModel> EquipmentReport_GetCustomerAddress(GetCustomerAddressParam _GetCustomerAddressParam, string ConnectionString)
        {
            return new BusinessLayer.BL_ReportsData().GetCustomerAddress(_GetCustomerAddressParam, ConnectionString);
        }
        public List<CustomerFilterViewModel> EquipmentReport_GetCustomerCity(GetCustomerCityParam _GetCustomerCityParam, string ConnectionString) 
        {
            return new BusinessLayer.BL_ReportsData().GetCustomerCity(_GetCustomerCityParam, ConnectionString);
        }
        public List<CustomerReportViewModel> EquipmentReport_GetDynamicReports(GetDynamicReportsParam _GetDynamicReportsParam, string ConnectionString) 
        {
            return new BusinessLayer.BL_ReportsData().GetDynamicReports(_GetDynamicReportsParam, ConnectionString);
        }
        public List<CustomerFilterViewModel> EquipmentReport_GetReportColByRepId(GetReportColByRepIdParam _GetReportColByRepId, string ConnectionString)
        {
            return new BusinessLayer.BL_ReportsData().GetReportColByRepId(_GetReportColByRepId, ConnectionString);
        }
        public List<CustomerFilterViewModel> EquipmentReport_GetReportFiltersByRepId(GetReportFiltersByRepIdParam _GetReportFiltersByRepId, string ConnectionString) 
        {
            return new BusinessLayer.BL_ReportsData().GetReportFiltersByRepId(_GetReportFiltersByRepId, ConnectionString);
        }
        public ListGetEquipmentInspection EquipmentReport_GetEquipmentInspection(GetEquipmentInspectionParam _GetEquipmentInspection, string ConnectionString)
        {
            return new BusinessLayer.BL_ReportsData().getEquipmentInspection(_GetEquipmentInspection, ConnectionString);
        }
        public List<UserViewModel> EquipmentReport_GetAccountSummaryListingDetail(GetAccountSummaryListingDetailParam _GetAccountSummaryListingDetail, string ConnectionString) 
        {
            return new BusinessLayer.BL_ReportsData().GetAccountSummaryListingDetail(_GetAccountSummaryListingDetail, ConnectionString);
        }
        public bool EquipmentReport_CheckExistingReport(CheckExistingReportParam _CheckExistingReport, string ConnectionString) 
        {
            return new BusinessLayer.BL_ReportsData().CheckExistingReport(_CheckExistingReport, ConnectionString);
        }
        public List<CustomerReportViewModel> EquipmentReport_InsertCustomerReport(InsertCustomerReportParam _InsertCustomerReport, string ConnectionString)
        {
            return new BusinessLayer.BL_ReportsData().InsertCustomerReport(_InsertCustomerReport, ConnectionString);
        }
        public bool EquipmentReport_IsStockReportExist(IsStockReportExistParam _IsStockReportExist, string ConnectionString)
        {
            return new BusinessLayer.BL_ReportsData().IsStockReportExist(_IsStockReportExist, ConnectionString);
        }
        public void EquipmentReport_UpdateCustomerReport(UpdateCustomerReportParam _UpdateCustomerReport, string ConnectionString)
        {
            new BusinessLayer.BL_ReportsData().UpdateCustomerReport(_UpdateCustomerReport, ConnectionString);
        }
        public void EquipmentReport_DeleteCustomerReport(DeleteCustomerReportParam _DeleteCustomerReport, string ConnectionString) 
        {
            new BusinessLayer.BL_ReportsData().DeleteCustomerReport(_DeleteCustomerReport, ConnectionString);
        }
        public List<CustomerFilterViewModel> EquipmentReport_GetControlForReports(getConnectionConfigParam _getConnectionConfig, string ConnectionString)
        {
            return new BusinessLayer.BL_ReportsData().GetControlForReports(_getConnectionConfig, ConnectionString);
        }
        public List<HeaderFooterDetailViewModel> EquipmentReport_GetHeaderFooterDetail(GetHeaderFooterDetailParam _GetHeaderFooterDetail, string ConnectionString) 
        {
            return new BusinessLayer.BL_ReportsData().GetHeaderFooterDetail(_GetHeaderFooterDetail, ConnectionString);
        }
        public List<CustomerFilterViewModel> EquipmentReport_GetOwners(GetOwnersParam _GetOwnersParam, string ConnectionString)
        {
            return new BusinessLayer.BL_ReportsData().GetOwners(_GetOwnersParam, ConnectionString);
        }
        public List<CustomerReportViewModel> EquipmentReport_GetColumnWidthByReportId(GetColumnWidthByReportIdParam _GetColumnWidthByReportId, string ConnectionString)
        {
            return new BusinessLayer.BL_ReportsData().GetColumnWidthByReportId(_GetColumnWidthByReportId, ConnectionString);
        }
        public void EquipmentReport_UpdateCustomerReportResizedWidth(UpdateCustomerReportResizedWidthParam _UpdateCustomerReportResizedWidth, string ConnectionString) 
        {
            new BusinessLayer.BL_ReportsData().UpdateCustomerReportResizedWidth(_UpdateCustomerReportResizedWidth, ConnectionString);
        }
    }
}
