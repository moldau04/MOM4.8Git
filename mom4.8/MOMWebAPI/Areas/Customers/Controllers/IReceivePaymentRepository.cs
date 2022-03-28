using BusinessEntity;
using BusinessEntity.APModels;
using BusinessEntity.CustomersModel;
using BusinessEntity.Payroll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MOMWebAPI.Areas.Customers.Controllers
{
    public interface IReceivePaymentRepository
    {
        /// <summary>
        /// For ReceivePayment List Screen : ReceivePayment.aspx / ReceivePayment.aspx.cs
        /// </summary>
        /// API's Naming Conventions : ReceivePaymentList_Method Name(Parameter)
        /// 

        //public List<UserViewModel> ReceivePaymentList_GetUserPermissionByUserID(GetUserByIdParam _GetUserById, string ConnectionString);
        public void ReceivePaymentList_UpdateCustomerBalance(UpdateCustomerBalanceParam _UpdateCustomerBalance, string ConnectionString);
        public void ReceivePaymentList_DeletePayment(DeletePaymentParam _DeletePayment, string ConnectionString);
        //public List<GetAllReceivePaymentViewModel> ReceivePaymentList_GetAllReceivePayment(GetAllReceivePaymentParam _GetAllReceivePayment, string ConnectionString, List<RetainFilter> filters, int intEN);


        /// <summary>
        /// For AddReceivePayment Screen : AddReceivePayment.aspx / AddReceivePayment.aspx.cs
        /// </summary>
        /// API's Naming Conventions : AddReceivePayment_Method Name(Parameter)
        /// 

        public ListGetInvoiceByCustomerID AddReceivePayment_GetInvoiceByCustomerID(GetInvoiceByCustomerIDParam _GetInvoiceByCustomerID, string ConnectionString);

        public List<GetInvoiceNosChangeViewModel> AddReceivePayment_GetInvoiceNosChange(GetInvoiceNosChangeParam _GetInvoiceNosChange, string ConnectionString);

        public void AddReceivePayment_UpdateReceivePayment(UpdateReceivePaymentParam _UpdateReceivePayment, string ConnectionString);

        public int AddReceivePayment_AddReceivePayment(AddReceivePaymentParam _AddReceivePayment, string ConnectionString);

        public ListGetInvoicesByReceivedPay AddReceivePayment_GetInvoicesByReceivedPay(GetInvoicesByReceivedPayParam _GetInvoicesByReceivedPay, string ConnectionString);

        public List<GetUndepositeAcctViewModel> AddReceivePayment_GetUndepositeAcct(GetUndepositeAcctParam _GetUndepositeAcct, string ConnectionString);

        public List<GetReceivePaymentByIDViewModel> AddReceivePayment_GetReceivePaymentByID(GetReceivePaymentByIDParam _GetReceivePaymentByID, string ConnectionString);

        public List<GetScreensByUserViewModel> AddReceivePayment_GetScreensByUser(GetScreensByUserParam _GetScreensByUser, string ConnectionString);

        public List<GetLocationLogViewModel> AddReceivePayment_GetReceivePaymentLogs(GetReceivePaymentLogsParam _GetReceivePaymentLogs, string ConnectionString);

        public List<GetCustomerUnAppliedCreditViewModel> AddReceivePayment_GetCustomerUnAppliedCredit(GetCustomerUnAppliedCreditParam _GetCustomerUnAppliedCredit, string ConnectionString, int userId, int filter);

        public ListGetInvoicesByID AddReceivePayment_GetInvoicesByID(GetInvoicesByIDParam _GetInvoicesByID, string ConnectionString);

        public List<GetActiveBillingCodeViewModel> AddReceivePayment_GetActiveBillingCode(GetActiveBillingCodeParam _GetActiveBillingCode, string ConnectionString);

        public void AddReceivePayment_writeOffInvoiceMulti(writeOffInvoiceMultiParam _writeOffInvoiceMulti, string ConnectionString);

        public void AddReceivePayment_writeOffInvoice(writeOffInvoiceParam _writeOffInvoice, string ConnectionString);

        public ListGetLocationByID  AddReceivePayment_GetLocationByID(GetLocationByIDParam _GetLocationByID, string ConnectionString);

        public void AddReceivePayment_TransferPayment(TransferPaymentParam _TransferPayment, string ConnectionString, string strRef, int newLoc);

        public void AddReceivePayment_UnapplyPayment(UnapplyPaymentParam _UnapplyPayment, string ConnectionString, int Ref);


        /// <summary>
        /// For AddMultiReceivePayment Screen : AddMultiReceivePayment.aspx / AddMultiReceivePayment.aspx.cs
        /// </summary>
        /// API's Naming Conventions : AddMultiReceivePayment_Method Name(Parameter)
        /// 

        public List<GetAllBankNamesViewModel> AddMultiReceivePayment_GetAllBankNames(GetAllBankNamesParam _GetAllBankNames, string ConnectionString);
        //public List<AddBatchReceivePaymentViewModel> AddMultiReceivePayment_AddBatchReceivePayment(AddBatchReceivePaymentParam _AddBatchReceivePayment, string ConnectionString);
        public List<GetDepByIDViewModel> AddMultiReceivePayment_GetDepByID(GetDepByIDParam _GetDepByID, string ConnectionString);
        public List<GetDepHeadByIDViewModel> AddMultiReceivePayment_GetDepHeadByID(GetDepHeadByIDParam _GetDepHeadByID, string ConnectionString);
        public ListGetReceivedPaymentByDep AddMultiReceivePayment_GetReceivedPaymentByDep(GetReceivedPaymentByDepParam _GetReceivedPaymentByDep, string ConnectionString);
        public List<GetInvoiceByListViewModel> AddMultiReceivePayment_GetInvoiceByList(GetInvoiceByListParam _GetInvoiceByList, string ConnectionString, string invoiceId, String checkNumber, Boolean isSeparate);
        public List<GetInvoicesByReceivedPayMultiViewModel> AddMultiReceivePayment_GetInvoicesByReceivedPayMulti(GetInvoicesByReceivedPayMultiParam _GetInvoicesByReceivedPayMulti, string ConnectionString, int owner, string loc, string invoice);


        /// <summary>
        /// For ReceivePaymentListReport Screen : ReceivePaymentListReport.aspx / ReceivePaymentListReport.aspx.cs
        /// </summary>
        /// API's Naming Conventions : ReceivePaymentListReport_Method Name(Parameter)
        /// 

        public List<GetCompanyDetailsViewModel> ReceivePaymentListReport_GetCompanyDetails(GetCompanyDetailsParam _GetCompanyDetails, string ConnectionString); 
        public List<GetControlViewModel> ReceivePaymentListReport_GetControl(getConnectionConfigParam _user, string ConnectionString);
        public List<SMTPEmailViewModel> ReceivePaymentListReport_GetSMTPByUserID(GetSMTPByUserIDParam _GetSMTPByUserID, string ConnectionString);

        //public List<GetReceivePaymentReportViewModel> ReceivePaymentListReport_GetReceivePaymentReport(GetReceivePaymentReportParam _GetReceivePaymentReport, string ConnectionString, List<RetainFilter> filters, int intEN);
    }
}
