using BusinessEntity;
using BusinessEntity.APModels;
using BusinessEntity.CustomersModel;
using BusinessEntity.Payroll;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MOMWebAPI.Areas.Customers.Controllers
{
    public interface IMakeDepositRepository
    {
        /// <summary>
        /// For ManageDeposit List Screen : ManageDeposit.aspx / ManageDeposit.aspx.cs
        /// </summary>
        /// API's Naming Conventions : ManageDepositList_Method Name(Parameter)
        /// 

        public List<GetAllDepositsViewModel> ManageDepositList_GetAllDeposits(GetAllDepositsParam _GetAllDeposits, string ConnectionString);
        public void ManageDepositList_DeleteDeposit(DeleteDepositParam _DeleteDeposit, string ConnectionString);

        /// <summary>
        /// For AddDeposit List Screen : AddDeposit.aspx / AddDeposit.aspx.cs
        /// </summary>
        /// API's Naming Conventions : AddDeposit_Method Name(Parameter)
        /// 

        public List<GetDepByIDViewModel> AddDeposit_GetDepByID(GetDepByIDParam _GetDepByID, string ConnectionString);
        public List<GetDepHeadByIDViewModel> AddDeposit_GetDepHeadByID(GetDepHeadByIDParam _GetDepHeadByID, string ConnectionString);
        public void AddDeposit_UpdateDeposit(UpdateDepositParam _UpdateDeposit, string ConnectionString, int depId, DataTable dtDelete, DataTable dtNew, DataTable dtDeleteGL, DataTable dtNewGL, String UpdatedBy);
        public ListGetAllInvoiceByDep AddDeposit_GetAllInvoiceByDep(GetAllInvoiceByDepParam _GetAllInvoiceByDep, string ConnectionString, int depId);
        public ListGetReceivedPaymentByDep AddDeposit_GetReceivedPaymentByDep(GetReceivedPaymentByDepParam _GetReceivedPaymentByDep, string ConnectionString);
        public List<GetAllReceivePaymentForDepViewModel> AddDeposit_GetAllReceivePaymentForDep(GetAllReceivePaymentForDepParam _GetAllReceivePaymentForDep, string ConnectionString);
        public List<GetAllBankNamesViewModel> AddDeposit_GetAllBankNames(GetAllBankNamesParam _GetAllBankNamesParam, string ConnectionString);
        public void AddDeposit_UpdateReceivedPayStatus(UpdateReceivedPayStatusParam _UpdateReceivedPayStatus, string ConnectionString);
        public void AddDeposit_UpdateDepositTransBank(UpdateDepositTransBankParam _UpdateDepositTransBank, string ConnectionString);
        public Int32 AddDeposit_GetBankAcctID(GetBankAcctIDParam _GetBankAcctIDParam, string ConnectionString);
        public int AddDeposit_AddDepositWithGL(AddDepositWithGLParam _AddDepositWithGL, string ConnectionString);
        public void AddDeposit_UpdateChartBalance(UpdateChartBalanceParam _UpdateChartBalance, string ConnectionString);
        public List<GetScreensByUserViewModel> AddDeposit_GetScreensByUser(GetScreensByUserParam _GetScreensByUser, string ConnectionString);
        public void AddDeposit_DepositInfor_UpdateDeposit(DepositInfor_UpdateDepositParam _DepositInfor_UpdateDeposit, string ConnectionString);


        /// <summary>
        /// For DepositListReport Screen : DepositListReport.aspx / DepositListReport.aspx.cs
        /// </summary>
        /// API's Naming Conventions : DepositReport_Method Name(Parameter)
        /// 

        public List<GetCompanyDetailsViewModel> DepositReport_GetCompanyDetails(GetCompanyDetailsParam _GetCompanyDetails, string ConnectionString);
        public List<GetDepositListByDateViewModel> DepositReport_GetDepositListByDate(GetDepositListByDateParam _GetDepositListByDate, string ConnectionString, bool incZeroAmount);
        public List<GetControlViewModel> DepositReport_GetControl(getConnectionConfigParam _getConnectionConfig, string ConnectionString);


        /// <summary>
        /// For DepositListBySalepersonReport Screen : DepositListBySalepersonReport.aspx / DepositListBySalepersonReport.aspx.cs
        /// </summary>
        /// API's Naming Conventions : DepositReport_Method Name(Parameter)
        /// 
        public List<SMTPEmailViewModel> DepositReport_GetSMTPByUserID(GetSMTPByUserIDParam _GetSMTPByUserID, string ConnectionString);

    }
}
