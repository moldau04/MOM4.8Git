using BusinessEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessEntity.APModels;
using BusinessEntity.Payroll;
using BusinessEntity.payroll;

namespace MOMWebAPI.Areas.AP.Controllers
{
    public interface IBillRepository
    {
        /// <summary>
        /// For ManageBills Screen : ManageBills.aspx / ManageBills.aspx.cs
        /// </summary>
        /// 

        //List<ControlViewModel> GetControl(GetControlParam _GetControlParam, string ConnectionString);
        List<GetAllPJDetailsViewModel> BillsList_GetAllPJDetails(GetAllPJDetailsParam _GetAllPJDetailsParam, string ConnectionString);
        List<GetAllPJRecurrDetailsViewModel> BillsList_GetAllPJRecurrDetails(GetAllPJRecurrDetailsParam _GetAllPJRecurrDetailsParam, string ConnectionString);
        int BillsList_ProcessRecurBill(ProcessRecurBillParam __ProcessRecurBillParam, string ConnectionString);
        void BillsList_DeleteAPBillRecurr(DeleteAPBillRecurrParam _DeleteAPBillRecurrParam, string ConnectionString);
        List<CDViewModel> BillsList_GetProcessRecurrCount(GetProcessRecurrCountParam _GetProcessRecurrCountParam, string ConnectionString);
        void BillsList_DeleteAPBill(DeleteAPBillParam _DeleteAPBillParam, string ConnectionString);
        void BillsList_UpdateAPDates(UpdateAPDatesParam _UpdateAPDatesParam, string ConnectionString);
        List<GetPJAcctDetailByIDViewModel> BillsList_GetPJAcctDetailByID(GetPJAcctDetailByIDParam _GetPJAcctDetailByIDParam, string ConnectionString);
        public List<GetControlViewModel> BillsList_GetControl(getConnectionConfigParam _user, string ConnectionString);
        List<CustomerReportViewModel> BillsList_GetStockReports(GetStockReportsParam _GetStockReportsParam, string ConnectionString);

        /// <summary>
        /// For AddBills Screen : AddBills.aspx / AddBills.aspx.cs
        /// </summary>
        /// 
        ListGetOutStandingPOById AddBills_GetOutStandingPOById(string ConnectionString, GetOutStandingPOByIdParam _GetOutStandingPOByIdParam);
        List<VendorViewModel> AddBills_GetVendorSearch(string ConnectionString, GetVendorSearchParam _GetVendorSearchParam);
        List<GetReceivePOListViewModel> AddBills_GetReceivePOList(string ConnectionString, GetReceivePOListParam _GetReceivePOListParam);
        void AddBills_AddReceivePOItem(string ConnectionString, AddReceivePOItemParam _AddReceivePOItemParam);
        void AddBills_UpdatePOItemBalance(string ConnectionString,UpdatePOItemBalanceParam _UpdatePOItemBalanceParam);
        void AddBills_UpdatePOItemQuan(string ConnectionString,UpdatePOItemQuanParam _UpdatePOItemQuanParam);
        void AddBills_AddEditReceivePO(string ConnectionString, AddEditReceivePOParam _AddEditReceivePOParam);
        void AddBills_updateJobComm(string ConnectionString, updateJobCommParam _updateJobCommParam);
        ListGetPOReceivePOById AddBills_GetPOReceivePOById(string ConnectionString, GetPOReceivePOByIdParam _GetPOReceivePOByIdParam);
        void AddBills_UpdatePOItemWarehouseLocation(UpdatePOItemWarehouseLocationParam _UpdatePOItemWarehouseLocationParam, string ConnectionString);
        void AddBills_AddReceiveInventoryWHTrans(AddReceiveInventoryWHTransParam _AddReceiveInventoryWHTrans, string ConnectionString);
        void AddBills_CreateReceivePOInvWarehouse(CreateReceivePOInvWarehouseTransParam _CreateReceivePOInvWarehouseTransParam, string ConnectionString);
        void AddBills_CreateReceivePOInvWarehouseTrans(ReceivePOInvWarehouseTransParam _ReceivePOInvWarehouseTransParam, string ConnectionString);
        List<GetPJDetailByIDViewModel> AddBills_GetPJDetailByID(GetPJDetailByIDParam _GetPJDetailByIDParam, string ConnectionString);
        List<GetBillTransDetailsViewModel> AddBills_GetBillTransDetails(GetBillTransDetailsParam _GetBillTransDetailsParam, string ConnectionString);
        List<GetPJRecurrDetailByIDViewModel> AddBills_GetPJRecurrDetailByID(GetPJRecurrDetailByIDParam _GetPJRecurrDetailByIDParam, string ConnectionString);
        List<GetBillRecurrTransactionsViewModel> AddBills_GetBillRecurrTransactions(GetBillRecurrTransactionsParam _GetBillRecurrTransactionsParam, string ConnectionString);
        void AddBills_UpdatePOStatus(string ConnectionString, UpdatePOStatusParam _UpdatePOStatusParam);
        void AddBills_UpdateReceivePOStatusByPOID(string ConnectionString, UpdateReceivePOStatusByPOIDParam _UpdateReceivePOStatusByPOIDParam);
        void AddBills_UpdateReceivePOStatus(string ConnectionString, UpdateReceivePOStatusParam _UpdateReceivePOStatusParam);
        int AddBills_GetMaxReceivePOId(string ConnectionString, GetMaxReceivePOIdParam _GetMaxReceivePOIdParam);
        ListGetInventoryItemStatus AddBills_GetInventoryItemStatus(string ConnectionString, GetInventoryItemStatusParam _GetInventoryItemStatusParam);
        List<GetBillHistoryPaymentViewModel> AddBills_GetBillHistoryPayment(GetBillHistoryPaymentParam _GetBillHistoryPaymentParam, string ConnectionString);
        List<BOMTViewModel> AddBills_GetBomType(GetBomTypeParam _GetBomTypeParam, string ConnectionString);
        List<GeneralViewModel> AddBills_GetInvDefaultAcct(GetInvDefaultAcctParam _GetInvDefaultAcctParam, string ConnectionString);
        bool AddBills_IsExistPO(string ConnectionString, IsExistPOParam _IsExistPOParam);
        string AddBills_GetClosePOCheck(string ConnectionString, GetClosePOCheckParam _GetClosePOCheckParam);
        List<CustomViewModel> AddBills_GetCustomField(getCustomFieldParam _getCustomFieldParam, string ConnectionString);
        List<STaxViewModel> AddBills_GetSTax(getSTaxParam _getSTaxParam, string ConnectionString);
        List<CustomViewModel> AddBills_GetCustomFieldsControl(getCustomFieldsControlParam _getCustomFieldsControlParam, string ConnectionString);
        List<ChartViewModel> AddBills_GetChart(GetChartParam _GetChartParam, string ConnectionString);
        List<VendorViewModel> AddBills_GetVendorByName(GetVendorByNameParam _GetVendorByNameParam, string ConnectionString);
        List<GetAutoFillOnHandBalanceViewModel> AddBills_GetAutoFillOnHandBalance(GetAutoFillOnHandBalanceParam _GetAutoFillOnHandBalanceParam, string ConnectionString);
        string AddBills_AddBills(AddBillsParam _AddBillsParam, string ConnectionString);
        string AddBills_UpdateRecurrBills(UpdateRecurrBillsParam _UpdateRecurrBillsParam, string ConnectionString);
        void AddBills_UpdateBills(UpdateBillsParam _UpdateBillsParam, string ConnectionString);
        List<CustomViewModel> AddBills_GetCustomFields(getCustomFieldsParam _getCustomFieldsParam, string ConnectionString);
        void AddBills_UpdateVendorSTax(UpdateVendorSTaxParam _UpdateVendorSTaxParam, string ConnectionString);
        bool AddBills_ISINVENTORYTRACKINGISON(string ConnectionString);
        List<UserViewModel> AddBills_GetUserById(GetUserByIdParam _GetUserByIdParam, string ConnectionString);
        void AddBills_UpdateBillsJobDetails(UpdateBillsJobDetailsParam _UpdateBillsJobDetailsParam, string ConnectionString);

        ListGetBillingItems AddBills_GetBillingItems(GetBillingItemsParam _GetBillingItemsParam, string ConnectionString);

        public List<GetControlViewModel> AddBills_GetControl(getConnectionConfigParam _user, string ConnectionString);
        public List<LogViewModel> AddBills_GetBillsLogs(GetBillsLogsParam _GetBillsLogs, string ConnectionString);
        int AddBills_UpdateApplyCreditDate(UpdateApplyCreditDateParam _UpdateApplyCreditDateParam, string ConnectionString);




        /// <summary>
        /// For Reports API's : 
        /// </summary>


        /// <summary>
        /// 1) BillListingReport.aspx / BillListingReport.aspx.cs --pending
        /// </summary>

        ListGetBillReportFiltersValue BillsReport_GetBillReportFiltersValue(GetBillReportFiltersValueParam _GetBillReportFiltersValueParam, string ConnectionString);
        List<BillReportDetails> BillsReport_GetBillDetails(GetBillDetailsParam _GetBillDetailsParam, string ConnectionString);
        List<UserViewModel> BillsReport_GetAccountSummaryListingDetail(GetAccountSummaryListingDetailParam _GetAccountSummaryListingDetailParam, string ConnectionString);
        List<CustomerReportViewModel> BillsReport_GetReportDetailById(GetReportDetailByIdParam _GetReportDetailByIdParam, string ConnectionString);
        public List<CustomerReportViewModel> BillsReport_GetCustomerType(GetCustomerTypeParam _getCustomerType, string ConnectionString);
        public List<CustomerFilterViewModel> BillsReport_GetCustomerName(GetCustomerNameParam _GetCustomerNameParam, string ConnectionString);

        public List<CustomerFilterViewModel> BillsReport_GetCustomerAddress(GetCustomerAddressParam _GetCustomerAddressParam, string ConnectionString);
        public List<CustomerFilterViewModel> BillsReport_GetCustomerCity(GetCustomerCityParam _GetCustomerCityParam, string ConnectionString);
        public List<CustomerReportViewModel> BillsReport_GetDynamicReports(GetDynamicReportsParam _GetDynamicReportsParam, string ConnectionString);
        public List<CustomerFilterViewModel> BillsReport_GetReportColByRepId(GetReportColByRepIdParam _customerreport, string ConnectionString);
        public List<CustomerFilterViewModel> BillsReport_GetReportFiltersByRepId(GetReportFiltersByRepIdParam _customerreport, string ConnectionString);
        public bool BillsReport_CheckExistingReport(CheckExistingReportParam _CheckExistingReportParam, string ConnectionString);
        public List<CustomerReportViewModel> BillsReport_InsertCustomerReport(InsertCustomerReportParam _InsertCustomerReportParam, string ConnectionString);
        public bool BillsReport_IsStockReportExist(IsStockReportExistParam _IsStockReportExistParam, string ConnectionString);
        public void BillsReport_UpdateCustomerReport(UpdateCustomerReportParam _UpdateCustomerReportParam, string ConnectionString);
        public void BillsReport_DeleteCustomerReport(DeleteCustomerReportParam _customerreport, string ConnectionString);
        public List<CustomerFilterViewModel> BillsReport_GetControlForReports(getConnectionConfigParam _getConnectionConfigParam, string ConnectionString);
        public List<HeaderFooterDetailViewModel> BillsReport_GetHeaderFooterDetail(GetHeaderFooterDetailParam _CustomerReportParam, string ConnectionString);
        public List<CustomerFilterViewModel> BillsReport_GetOwners(GetOwnersParam _GetOwnersParam, string ConnectionString); 
        public List<CustomerReportViewModel> BillsReport_GetColumnWidthByReportId(GetColumnWidthByReportIdParam _customerreport, string ConnectionString);
        public void BillsReport_UpdateCustomerReportResizedWidth(UpdateCustomerReportResizedWidthParam _customerreport, string ConnectionString);



        /// <summary>
        /// 2) BillsReport.aspx / BillsReport.aspx.cs
        /// </summary>

        //List<PJViewModel> GetAllPJDetails(GetAllPJDetailsForReportsParam _GetAllPJDetailsForReportsParam, string ConnectionString);
        public List<SMTPEmailViewModel> BillsReport_GetSMTPByUserID(GetSMTPByUserIDParam _user, string ConnectionString);
        public List<GetCompanyDetailsViewModel> BillsReport_GetCompanyDetails(GetCompanyDetailsParam _GetCompanyDetailsParam, string ConnectionString);



        /// <summary>
        /// 3) APAgingReport.aspx / APAgingReport.aspx.cs
        /// </summary>

        public List<GetBillsDetailsByDueViewModel> BillsReport_GetBillsDetailsByDue(GetBillsDetailsByDueParam _GetBillsDetailsByDueParam, string ConnectionString);

        public List<GetAPAgingByDateViewModel> BillsReport_GetAPAgingByDate(GetAPAgingByDateParam _GetAPAgingByDateParam, string ConnectionString);



        /// <summary>
        /// 4) PurchaseJournalReport.aspx / PurchaseJournalReport.aspx.cs
        /// </summary>

        public List<OpenAPViewModel> BillsReport_GetPurchaseJournal(GetPurchaseJournalParam _GetPurchaseJournalParam, string ConnectionString);


        /// <summary>
        /// 5) UseTaxReport.aspx / UseTaxReport.aspx.cs
        /// </summary>

        public List<GetUseTaxViewModel> BillsReport_GetUseTax(GetUseTaxForReportsParam _GetUseTaxForReportsParam, string ConnectionString);

        /// <summary>
        /// 6) UTaxLocReport.aspx / UTaxLocReport.aspx.cs
        /// </summary>

        public List<GetUTaxLocReportViewModel> BillsReport_GetUTaxLocReport(GetUTaxLocReportParam _GetUTaxLocReportParam, string ConnectionString);


        /// <summary>
        /// 7) PrintBillRegisterGL.aspx / PrintBillRegisterGL.aspx.cs
        /// </summary>

        public List<GetAPGLRegViewModel> BillsReport_GetAPGLReg(GetAPGLRegParam _GetAPGLRegParam, string ConnectionString);


        /// <summary>
        /// 8) APAging360Report.aspx / APAging360Report.aspx.cs
        /// </summary>

        public List<GetBillsDetails360ByDueViewModel> BillsReport_GetBillsDetails360ByDue(GetBillsDetails360ByDueParam _GetBillsDetails360ByDue, string ConnectionString);
        public List<GetAPAging360ByDateViewModel> BillsReport_GetAPAging360ByDate(GetAPAging360ByDateParam _GetAPAging360ByDate, string ConnectionString);

    }
}
