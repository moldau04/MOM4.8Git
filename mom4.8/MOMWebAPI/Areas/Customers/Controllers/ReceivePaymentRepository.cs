using BusinessEntity;
using BusinessEntity.APModels;
using BusinessEntity.CustomersModel;
using BusinessEntity.Payroll;
using BusinessLayer.Billing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MOMWebAPI.Areas.Customers.Controllers
{
    public class ReceivePaymentRepository : IReceivePaymentRepository
    {

        /// <summary>
        /// For ReceivePayment List Screen : ReceivePayment.aspx / ReceivePayment.aspx.cs
        /// </summary>
        /// API's Naming Conventions : ReceivePaymentList_Method Name(Parameter)
        /// 

        public void ReceivePaymentList_UpdateCustomerBalance(UpdateCustomerBalanceParam _UpdateCustomerBalance, string ConnectionString)
        {
            new BusinessLayer.BL_Contracts().UpdateCustomerBalance(_UpdateCustomerBalance, ConnectionString);
        }
        public void ReceivePaymentList_DeletePayment(DeletePaymentParam _DeletePayment, string ConnectionString)
        {
            new BusinessLayer.BL_Deposit().DeletePayment(_DeletePayment, ConnectionString);
        }
        //public List<GetAllReceivePaymentViewModel> ReceivePaymentList_GetAllReceivePayment(GetAllReceivePaymentParam _GetAllReceivePayment, string ConnectionString, List<RetainFilter> filters, int intEN)
        //{
        //    return new BusinessLayer.BL_Deposit().GetAllReceivePayment(_GetAllReceivePayment, ConnectionString, filters, intEN);
        //}

        /// <summary>
        /// For AddReceivePayment Screen : AddReceivePayment.aspx / AddReceivePayment.aspx.cs
        /// </summary>
        /// API's Naming Conventions : AddReceivePayment_Method Name(Parameter)
        /// 

        public ListGetInvoiceByCustomerID AddReceivePayment_GetInvoiceByCustomerID(GetInvoiceByCustomerIDParam _GetInvoiceByCustomerID, string ConnectionString)
        {
            return new BusinessLayer.BL_Deposit().GetInvoiceByCustomerID(_GetInvoiceByCustomerID, ConnectionString);
        }

        public List<GetInvoiceNosChangeViewModel> AddReceivePayment_GetInvoiceNosChange(GetInvoiceNosChangeParam _GetInvoiceNosChange, string ConnectionString)
        {
            return new BusinessLayer.BL_Deposit().GetInvoiceNosChange(_GetInvoiceNosChange, ConnectionString);
        }

        public void AddReceivePayment_UpdateReceivePayment(UpdateReceivePaymentParam _UpdateReceivePayment, string ConnectionString)
        {
            new BusinessLayer.BL_Deposit().UpdateReceivePayment(_UpdateReceivePayment, ConnectionString);
        }

        public int AddReceivePayment_AddReceivePayment(AddReceivePaymentParam _AddReceivePayment, string ConnectionString)
        {
            return new BusinessLayer.BL_Deposit().AddReceivePayment(_AddReceivePayment, ConnectionString);
        }

        public ListGetInvoicesByReceivedPay AddReceivePayment_GetInvoicesByReceivedPay(GetInvoicesByReceivedPayParam _GetInvoicesByReceivedPay, string ConnectionString)
        {
            return new BusinessLayer.BL_Deposit().GetInvoicesByReceivedPay(_GetInvoicesByReceivedPay, ConnectionString);
        }

        public List<GetUndepositeAcctViewModel> AddReceivePayment_GetUndepositeAcct(GetUndepositeAcctParam _GetUndepositeAcct, string ConnectionString)
        {
            return new BusinessLayer.BL_Chart().GetUndepositeAcct(_GetUndepositeAcct, ConnectionString);
        }

        public List<GetReceivePaymentByIDViewModel> AddReceivePayment_GetReceivePaymentByID(GetReceivePaymentByIDParam _GetReceivePaymentByID, string ConnectionString)
        {
            return new BusinessLayer.BL_Deposit().GetReceivePaymentByID(_GetReceivePaymentByID, ConnectionString);
        }

        public List<GetScreensByUserViewModel> AddReceivePayment_GetScreensByUser(GetScreensByUserParam _GetScreensByUser, string ConnectionString)
        {
            return new BusinessLayer.BL_User().getScreensByUser(_GetScreensByUser, ConnectionString);
        }

        public List<GetLocationLogViewModel> AddReceivePayment_GetReceivePaymentLogs(GetReceivePaymentLogsParam _GetReceivePaymentLogs, string ConnectionString)
        {
            return new BusinessLayer.BL_Deposit().GetReceivePaymentLogs(_GetReceivePaymentLogs, ConnectionString);
        }

        public List<GetCustomerUnAppliedCreditViewModel> AddReceivePayment_GetCustomerUnAppliedCredit(GetCustomerUnAppliedCreditParam _GetCustomerUnAppliedCredit, string ConnectionString, int userId, int filter)
        {
            return new BusinessLayer.BL_Deposit().GetCustomerUnAppliedCredit(_GetCustomerUnAppliedCredit, ConnectionString, userId, filter);
        }

        public ListGetInvoicesByID AddReceivePayment_GetInvoicesByID(GetInvoicesByIDParam _GetInvoicesByID, string ConnectionString)
        {
            return new BusinessLayer.BL_Contracts().GetInvoicesByID(_GetInvoicesByID, ConnectionString);
        }

        public List<GetActiveBillingCodeViewModel> AddReceivePayment_GetActiveBillingCode(GetActiveBillingCodeParam _GetActiveBillingCode, string ConnectionString)
        {
            return new BL_BillCodes().GetActiveBillingCode(_GetActiveBillingCode, ConnectionString);
        }

        public void AddReceivePayment_writeOffInvoiceMulti(writeOffInvoiceMultiParam _writeOffInvoiceMulti, string ConnectionString)
        {
            new BusinessLayer.BL_Deposit().writeOffInvoiceMulti(_writeOffInvoiceMulti, ConnectionString);
        }

        public void AddReceivePayment_writeOffInvoice(writeOffInvoiceParam _writeOffInvoice, string ConnectionString)
        {
            new BusinessLayer.BL_Deposit().writeOffInvoice(_writeOffInvoice, ConnectionString);
        }

        public ListGetLocationByID AddReceivePayment_GetLocationByID(GetLocationByIDParam _GetLocationByID, string ConnectionString)
        {
            return new BusinessLayer.BL_User().getLocationByID(_GetLocationByID, ConnectionString);
        }

        public void AddReceivePayment_TransferPayment(TransferPaymentParam _TransferPayment, string ConnectionString, string strRef, int newLoc)
        {
            new BusinessLayer.BL_Deposit().TransferPayment(_TransferPayment, ConnectionString, strRef, newLoc);
        }

        public void AddReceivePayment_UnapplyPayment(UnapplyPaymentParam _UnapplyPayment, string ConnectionString, int Ref)
        {
            new BusinessLayer.BL_Deposit().UnapplyPayment(_UnapplyPayment, ConnectionString, Ref);
        }



        /// <summary>
        /// For AddMultiReceivePayment Screen : AddMultiReceivePayment.aspx / AddMultiReceivePayment.aspx.cs
        /// </summary>
        /// API's Naming Conventions : AddMultiReceivePayment_Method Name(Parameter)
        /// 

        public List<GetAllBankNamesViewModel> AddMultiReceivePayment_GetAllBankNames(GetAllBankNamesParam _GetAllBankNames, string ConnectionString)
        {
            return new BusinessLayer.BL_BankAccount().GetAllBankNames(_GetAllBankNames, ConnectionString);
        }
        //public List<AddBatchReceivePaymentViewModel> AddMultiReceivePayment_AddBatchReceivePayment(AddBatchReceivePaymentParam _AddBatchReceivePayment, string ConnectionString){}
        public List<GetDepByIDViewModel> AddMultiReceivePayment_GetDepByID(GetDepByIDParam _GetDepByID, string ConnectionString)
        {
            return new BusinessLayer.BL_Deposit().GetDepByID(_GetDepByID, ConnectionString);
        }
        public List<GetDepHeadByIDViewModel> AddMultiReceivePayment_GetDepHeadByID(GetDepHeadByIDParam _GetDepHeadByID, string ConnectionString)
        {
            return new BusinessLayer.BL_Deposit().GetDepHeadByID(_GetDepHeadByID, ConnectionString);
        }
        public ListGetReceivedPaymentByDep AddMultiReceivePayment_GetReceivedPaymentByDep(GetReceivedPaymentByDepParam _GetReceivedPaymentByDep, string ConnectionString)
        {
            return new BusinessLayer.BL_Deposit().GetReceivedPaymentByDep(_GetReceivedPaymentByDep, ConnectionString);
        }
        public List<GetInvoiceByListViewModel> AddMultiReceivePayment_GetInvoiceByList(GetInvoiceByListParam _GetInvoiceByList, string ConnectionString, string invoiceId, String checkNumber, Boolean isSeparate)
        {
            return new BusinessLayer.BL_Deposit().GetInvoiceByList(_GetInvoiceByList, ConnectionString, invoiceId, checkNumber, isSeparate);
        }
        public List<GetInvoicesByReceivedPayMultiViewModel> AddMultiReceivePayment_GetInvoicesByReceivedPayMulti(GetInvoicesByReceivedPayMultiParam _GetInvoicesByReceivedPayMulti, string ConnectionString, int owner, string loc, string invoice)
        {
            return new BusinessLayer.BL_Deposit().GetInvoicesByReceivedPayMulti(_GetInvoicesByReceivedPayMulti, ConnectionString, owner, loc, invoice);
        }


        /// <summary>
        /// For ReceivePaymentListReport Screen : ReceivePaymentListReport.aspx / ReceivePaymentListReport.aspx.cs
        /// </summary>
        /// API's Naming Conventions : ReceivePaymentListReport_Method Name(Parameter)
        /// 

        public List<GetCompanyDetailsViewModel> ReceivePaymentListReport_GetCompanyDetails(GetCompanyDetailsParam _GetCompanyDetails, string ConnectionString)
        {
            return new BusinessLayer.BL_Report().GetCompanyDetails(_GetCompanyDetails, ConnectionString);
        }
        public List<GetControlViewModel> ReceivePaymentListReport_GetControl(getConnectionConfigParam _getConnectionConfigParam, string ConnectionString)
        {
            return new BusinessLayer.BL_User().getControl(_getConnectionConfigParam, ConnectionString);
        }

        public List<SMTPEmailViewModel> ReceivePaymentListReport_GetSMTPByUserID(GetSMTPByUserIDParam _GetSMTPByUserID, string ConnectionString)
        {
            return new BusinessLayer.BL_User().getSMTPByUserID(_GetSMTPByUserID, ConnectionString);
        }
    }
}
