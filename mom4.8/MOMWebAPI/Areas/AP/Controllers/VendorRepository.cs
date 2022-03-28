using BusinessEntity;
using BusinessEntity.APModels;
using BusinessEntity.Payroll;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace MOMWebAPI.Areas.AP.Controllers
{
    public class VendorRepository : IVendorRepository
    {
        public List<GetVendorTypeViewModel> VendorList_getVendorType(getVendorTypeParam _getVendorTypeParam, string connectionString)
        {
            try
            {
                return new BusinessLayer.BL_Vendor().getVendorType(_getVendorTypeParam, connectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public List<UserViewModel> VendorList_GetUserById(GetUserByIdParam _GetUserByIdParam, string connectionString)
        {
            return new BusinessLayer.BL_User().getUserByID(_GetUserByIdParam, connectionString);
        }


        public List<CustomerReportViewModel> VendorList_GetStockReports(GetStockReportsParam _GetStockReportsParam, string connectionString)
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

        public List<GetAllVenderAjaxSearchModel> VendorList_GetAllVenderAjaxSearch(GetAllVenderAjaxSearchParam _GetAllVenderAjaxSearchParam, string ConnectionString)
        {
            try
            {
                return new BusinessLayer.BL_Vendor().GetAllVenderAjaxSearch(_GetAllVenderAjaxSearchParam, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        public bool VendorList_IsExistVendorDetails(IsExistVendorDetailsParam _IsExistVendorDetailsParam, string connectionString)
        {
            try
            {
                return new BusinessLayer.BL_Vendor().IsExistVendorDetails(_IsExistVendorDetailsParam, connectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void VendorList_DeleteRolByID(DeleteRolByIDParam _DeleteRolByIDParam, string connectionString)
        {
            try
            {
                new BusinessLayer.BL_BankAccount().DeleteRolByID(_DeleteRolByIDParam, connectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void VendorList_DeleteVendor(DeleteVendorParam _DeleteVendorParam, string connectionString)
        {
            try
            {
                new BusinessLayer.BL_Vendor().DeleteVendor(_DeleteVendorParam, connectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        /// <summary>
        /// For Add Vendor Screen : AddVendor.aspx / AddVendor.aspx.cs
        /// </summary>


        public List<CompanyOfficeViewModel> AddVendor_GetCompanyByCustomer(GetCompanyByCustomerParam _GetCompanyByCustomerParam, string ConnectionString)
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

        public List<StateViewModel> AddVendor_GetStates(GetStatesParam _GetStatesParam, string ConnectionString)
        {
            try
            {
                return new BusinessLayer.BL_BankAccount().GetStates(_GetStatesParam, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<TermsViewModel> AddVendor_GetTerms(GetTermsParam _GetTermsParam, string ConnectionString)
        {
            try
            {
                return new BusinessLayer.BL_User().getTerms(_GetTermsParam, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public List<STaxViewModel> AddVendor_GetUseTax(getUseTaxParam _getUseTaxParam, string ConnectionString)
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


        public List<STaxViewModel> AddVendor_GetSTax(getSTaxParam _getSTaxParam, string ConnectionString)
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


        public List<CustomViewModel> AddVendor_GetCustomFields(getCustomFieldsParam _getCustomFieldsParam, string ConnectionString)
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


        public List<CustomViewModel> AddVendor_GetCustomFieldsControl(getCustomFieldsControlParam _getCustomFieldsControlParam, string ConnectionString)
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


        public List<ChartViewModel> AddVendor_GetChart(GetChartParam _GetChartParam, string ConnectionString)
        {
            try
            {
                return new BusinessLayer.BL_Chart().GetChart(_GetChartParam, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<VendorViewModel> AddVendor_GetVendorEdit(GetVendorEditParam _GetVendorEditParam, string ConnectionString)
        {
            try
            {
                return new BusinessLayer.BL_Vendor().GetVendorEdit(_GetVendorEditParam, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<GetVendorContactByRolIDViewModel> AddVendor_GetVendorContactByRolID(getVendorContactByRolIDParam _getVendorContactByRolIDParam, string ConnectionString)
        {
            try
            {
                return new BusinessLayer.BL_Vendor().getVendorContactByRolID(_getVendorContactByRolIDParam, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ListGetAPExpenses AddVendor_GetAPExpenses(GetAPExpensesParam _GetAPExpensesParam, string ConnectionString)
        {
            try
            {
                return new BusinessLayer.BL_Bills().GetAPExpenses(_GetAPExpensesParam, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void AddVendor_UpdateRol(UpdateRolParam _UpdateRolParam, string ConnectionString)
        {
            try
            {
                new BusinessLayer.BL_BankAccount().UpdateRol(_UpdateRolParam, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddVendor_UpdateRoles(UpdateRolesParam _UpdateRolesParam, string ConnectionString)
        {
            try
            {
                new BusinessLayer.BL_BankAccount().UpdateRoles(_UpdateRolesParam, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public List<VendorViewModel> AddVendor_isExistForUpdateVendor(IsExistForUpdateVendorParam _IsExistForUpdateVendorParam, string ConnectionString)
        {
            try
            {
                return new BusinessLayer.BL_Vendor().IsExistForUpdateVendor(_IsExistForUpdateVendorParam, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<LogViewModel> AddVendor_GetVendorLogs(GetVendorLogsParam _GetVendorLogsParam, string ConnectionString)
        {
            try
            {
                return new BusinessLayer.BL_Vendor().GetVendorLogs(_GetVendorLogsParam, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void AddVendor_UpdateVendor(UpdateVendorParam _UpdateVendorParam, string ConnectionString)
        {
            try
            {
                 new BusinessLayer.BL_Vendor().UpdateVendor(_UpdateVendorParam, ConnectionString);
                //SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, "spUpdateVendor", para);
                //SqlHelper.ExecuteNonQuery(objVendor.ConnConfig, CommandType.StoredProcedure, "spUpdateVendor", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        public void AddVendor_UpdateVendorTax(UpdateVendorTaxParam _UpdateVendorTaxParam, string ConnectionString)
        {
            try
            {
                 new BusinessLayer.BL_Vendor().UpdateVendorTax(_UpdateVendorTaxParam, ConnectionString);
                
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        public int AddVendor_AddRol(AddRolParam _AddRolParam, string ConnectionString)
        {
            try
            {
                return new BusinessLayer.BL_BankAccount().AddRol(_AddRolParam, ConnectionString);
                
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public List<VendorViewModel> AddVendor_isExistsForInsertVendor(IsExistsForInsertVendorParam _IsExistsForInsertVendorParam, string ConnectionString)
        {
            try
            {
                return new BusinessLayer.BL_Vendor().IsExistsForInsertVendor(_IsExistsForInsertVendorParam, ConnectionString);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public int AddVendor_AddVendor(AddVendorParam _AddVendorParam, string ConnectionString)
        {
            
            try
            {
                return new BusinessLayer.BL_Vendor().AddVendor(_AddVendorParam, ConnectionString);
               
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        public void AddVendor_UpdateVendorContact(UpdateVendorContactParam _UpdateVendorContactParam, string ConnectionString)
        {
            try
            {
                 new BusinessLayer.BL_Vendor().UpdateVendorContact(_UpdateVendorContactParam, ConnectionString);
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// For VendorLbl5160 Page : VendorLbl5160.aspx / VendorLbl5160.aspx.cs
        /// </summary>
        /// 

        public List<GetCompanyDetailsViewModel> VendorReport_GetCompanyDetails(GetCompanyDetailsParam _GetCompanyDetailsParam, string ConnectionString)
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

        public List<RolViewModel> VendorReport_GetVendorLabel(GetVendorLabelParam _GetVendorLabelParam, string ConnectionString)
        {
            try
            {
                return new BusinessLayer.BL_Report().GetVendorLabel(_GetVendorLabelParam, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<GetControlViewModel> VendorReport_GetControl(getConnectionConfigParam _getConnectionConfigParam, string ConnectionString)
        {
            return new BusinessLayer.BL_User().getControl(_getConnectionConfigParam, ConnectionString);
        }

        public List<SMTPEmailViewModel> VendorReport_GetSMTPByUserID(GetSMTPByUserIDParam _user, string ConnectionString)
        {
            return new BusinessLayer.BL_User().getSMTPByUserID(_user, ConnectionString);
        }

        /// <summary>
        /// For Vendor1099 Page : Vendor1099.aspx / Vendor1099.aspx.cs
        /// </summary>
        ///

        public List<VendorViewModel> VendorReport_GetFederalReport(GetFederalReportParam _GetFederalReportParam, string ConnectionString)
        {
            try
            {
                return new BusinessLayer.BL_Report().GetFederalReport(_GetFederalReportParam, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// For Vendor Transaction History Tab : VendorTransactionHistory.aspx / VendorTransactionHistory.aspx.cs
        /// </summary>
        /// 
        
        public List<GetHistoryTransactionViewModel> VendorTransaction_GetHistoryTransaction(GetHistoryTransactionParam _GetHistoryTransaction, string ConnectionString)
        {
            try
            {
                return new BusinessLayer.BL_Bills().GetHistoryTransaction(_GetHistoryTransaction, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
