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
    public class MakeDepositRepository : IMakeDepositRepository
    {
        /// <summary>
        /// For ManageDeposit List Screen : ManageDeposit.aspx / ManageDeposit.aspx.cs
        /// </summary>
        /// API's Naming Conventions : ManageDepositList_Method Name(Parameter)
        /// 

        public List<GetAllDepositsViewModel> ManageDepositList_GetAllDeposits(GetAllDepositsParam _GetAllDeposits, string ConnectionString)
        {
            try
            {
                return new BusinessLayer.BL_Deposit().GetAllDeposits(_GetAllDeposits, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ManageDepositList_DeleteDeposit(DeleteDepositParam _DeleteDeposit, string ConnectionString)
        {
            try
            {
                new BusinessLayer.BL_Deposit().DeleteDeposit(_DeleteDeposit, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// For AddDeposit List Screen : AddDeposit.aspx / AddDeposit.aspx.cs
        /// </summary>
        /// API's Naming Conventions : AddDeposit_Method Name(Parameter)
        /// 

        public List<GetDepByIDViewModel> AddDeposit_GetDepByID(GetDepByIDParam _GetDepByID, string ConnectionString)
        {
            return new BusinessLayer.BL_Deposit().GetDepByID(_GetDepByID, ConnectionString);
        }
        public List<GetDepHeadByIDViewModel> AddDeposit_GetDepHeadByID(GetDepHeadByIDParam _GetDepHeadByID, string ConnectionString)
        {
            return new BusinessLayer.BL_Deposit().GetDepHeadByID(_GetDepHeadByID, ConnectionString);
        }

        public void AddDeposit_UpdateDeposit(UpdateDepositParam _UpdateDeposit, string ConnectionString, int depId, DataTable dtDelete, DataTable dtNew, DataTable dtDeleteGL, DataTable dtNewGL, String UpdatedBy)
        {
            new BusinessLayer.BL_Deposit().UpdateDeposit(_UpdateDeposit, ConnectionString, depId, dtDelete, dtNew,  dtDeleteGL, dtNewGL, UpdatedBy);
        }

        public ListGetAllInvoiceByDep AddDeposit_GetAllInvoiceByDep(GetAllInvoiceByDepParam _GetAllInvoiceByDep, string ConnectionString, int depId)
        {
            return new BusinessLayer.BL_Deposit().GetAllInvoiceByDep(_GetAllInvoiceByDep, ConnectionString, depId);
        }
        public ListGetReceivedPaymentByDep AddDeposit_GetReceivedPaymentByDep(GetReceivedPaymentByDepParam _GetReceivedPaymentByDep, string ConnectionString)
        {
            return new BusinessLayer.BL_Deposit().GetReceivedPaymentByDep(_GetReceivedPaymentByDep, ConnectionString);
        }
        public List<GetAllReceivePaymentForDepViewModel> AddDeposit_GetAllReceivePaymentForDep(GetAllReceivePaymentForDepParam _GetAllReceivePaymentForDep, string ConnectionString)
        {
            return new BusinessLayer.BL_Deposit().GetAllReceivePaymentForDep(_GetAllReceivePaymentForDep, ConnectionString);
        }
        public List<GetAllBankNamesViewModel> AddDeposit_GetAllBankNames(GetAllBankNamesParam _GetAllBankNamesParam, string ConnectionString)
        {
            return new BusinessLayer.BL_BankAccount().GetAllBankNames(_GetAllBankNamesParam, ConnectionString);
        }

        public void AddDeposit_UpdateReceivedPayStatus(UpdateReceivedPayStatusParam _UpdateReceivedPayStatus, string ConnectionString)
        {
            new BusinessLayer.BL_Deposit().UpdateReceivedPayStatus(_UpdateReceivedPayStatus, ConnectionString);
        }
        public void AddDeposit_UpdateDepositTransBank(UpdateDepositTransBankParam _UpdateDepositTransBank, string ConnectionString)
        {
            new BusinessLayer.BL_Deposit().UpdateDepositTransBank(_UpdateDepositTransBank, ConnectionString);
        }
        public Int32 AddDeposit_GetBankAcctID(GetBankAcctIDParam _GetBankAcctIDParam, string ConnectionString)
        {
            return new BusinessLayer.BL_Chart().GetBankAcctID(_GetBankAcctIDParam, ConnectionString);
        }
        public int AddDeposit_AddDepositWithGL(AddDepositWithGLParam _AddDepositWithGL, string ConnectionString)
        {
            return new BusinessLayer.BL_Deposit().AddDepositWithGL(_AddDepositWithGL, ConnectionString);
        }
        public void AddDeposit_UpdateChartBalance(UpdateChartBalanceParam _UpdateChartBalance, string ConnectionString)
        {
            new BusinessLayer.BL_Chart().UpdateChartBalance(_UpdateChartBalance, ConnectionString);
        }
        public List<GetScreensByUserViewModel> AddDeposit_GetScreensByUser(GetScreensByUserParam _GetScreensByUser, string ConnectionString)
        {
            return new BusinessLayer.BL_User().getScreensByUser(_GetScreensByUser, ConnectionString);
        }
        public void AddDeposit_DepositInfor_UpdateDeposit(DepositInfor_UpdateDepositParam _DepositInfor_UpdateDeposit, string ConnectionString)
        {
            new BusinessLayer.BL_Deposit().UpdateDeposit(_DepositInfor_UpdateDeposit, ConnectionString);
        }


        /// <summary>
        /// For DepositListReport Screen : DepositListReport.aspx / DepositListReport.aspx.cs
        /// </summary>
        /// API's Naming Conventions : DepositReport_Method Name(Parameter)
        /// 

        public List<GetCompanyDetailsViewModel> DepositReport_GetCompanyDetails(GetCompanyDetailsParam _GetCompanyDetails, string ConnectionString)
        {
            return new BusinessLayer.BL_Report().GetCompanyDetails(_GetCompanyDetails, ConnectionString);
        }
        public List<GetDepositListByDateViewModel> DepositReport_GetDepositListByDate(GetDepositListByDateParam _GetDepositListByDate, string ConnectionString, bool incZeroAmount)
        {
            return new BusinessLayer.BL_Deposit().GetDepositListByDate(_GetDepositListByDate, ConnectionString, incZeroAmount);
        }
        public List<GetControlViewModel> DepositReport_GetControl(getConnectionConfigParam _getConnectionConfig, string ConnectionString)
        {
            return new BusinessLayer.BL_User().getControl(_getConnectionConfig, ConnectionString);
        }

        /// <summary>
        /// For DepositListBySalepersonReport Screen : DepositListBySalepersonReport.aspx / DepositListBySalepersonReport.aspx.cs
        /// </summary>
        /// API's Naming Conventions : DepositReport_Method Name(Parameter)
        /// 
        public List<SMTPEmailViewModel> DepositReport_GetSMTPByUserID(GetSMTPByUserIDParam _GetSMTPByUserID, string ConnectionString)
        {
            return new BusinessLayer.BL_User().getSMTPByUserID(_GetSMTPByUserID, ConnectionString);
        }
    }
}
