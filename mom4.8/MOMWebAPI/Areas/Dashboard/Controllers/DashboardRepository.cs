using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessEntity;
using BusinessLayer;

namespace MOMWebAPI.Areas.DashBoard.Controllers
{
    public class DashBoardRepository : IDashBoardRepository
    {
        #region Dashboard Page
        public List<Dashboard> DashBoard_GetDashboard(GetDashboardParam _GetDashboardParam, string ConnectionString)
        {
            return new BusinessLayer.BL_User().GetDashboard(_GetDashboardParam, ConnectionString);
        }
        public List<Dashboard> DashBoard_GetDashboardByUserID(GetDashboardParam _GetDashboardParam, string ConnectionString)
        {
            return new BusinessLayer.BL_User().GetDashboardByUserId(_GetDashboardParam, ConnectionString);
        }

        public List<QBLastSyncResParam> DashBoard_GetQBlatSync(General _General)
        {
            return new BusinessLayer.BL_General().GetQBlatSync(_General);
        }
        
        public List<Dashboard> DashBoard_GetDashboardByUserId(GetDashboardParam _GetDashboardParam, string ConnectionString)
        {
            return new BusinessLayer.BL_User().GetDashboard(_GetDashboardParam, ConnectionString);
        }
        public List<KPI> DashBoard_GetListDashKPI(GetListDashKPIParam _GetListDashKPIParam, string ConnectionString)
        {
            return new BusinessLayer.BL_User().GetListDashKPI(_GetListDashKPIParam, ConnectionString);
        }
        public string DashBoard_GetGPSInterval(General _General)
        {  
            return new BusinessLayer.BL_General().GetGPSIntervalSP(_General);
        }
        public string DashBoard_GetDefaultCategory(User _User)
        {
            return new BusinessLayer.BL_User().getDefaultCategoryAPI(_User);
        }
        public List<CollectionResponseModel> DashBoard_GetCollectionSummary(GetDashboardCollectionParam _GetDashboardCollectionParam, string ConnectionString)
        {
            return new BusinessLayer.BL_Collection().GetCollectionsSummary(_GetDashboardCollectionParam, ConnectionString);
        }

        public void DashBoard_UpdateDashboardDockStates(UpdateDashboardDockStatesParam _UpdateDashboardDockStatesParam)
        {
            try
            {
                new BusinessLayer.BL_User().UpdateDashboardDockStatesySP(_UpdateDashboardDockStatesParam);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Sales Tab Chart
        public AvgEstimateResponse DashBoard_GetAvgEstimateConversionRate(User _User)
        {
            return new BusinessLayer.BL_User().GetAvgEstimateList(_User);
        }
        public List<EquipmentTypeCountResponse> DashBoard_GetEquipmentTypeChart(string _conString)
        {
            return new BusinessLayer.BL_User().GetEquipmentTypeCountList(_conString);
        }
        public List<EquipmentBuildingCountResponse> DashBoard_GetEquipmentBuildingChart(string _conString)
        {
            return new BusinessLayer.BL_User().GetEquipmentBuildingCountList(_conString);
        }
      
        public List<LeadAverageResponse> DashBoard_GetRecurringHoursChart(User _User)
        {
            return new BusinessLayer.BL_User().GetEstimatesBySalespersonAverageDays(_User);
        }
        public List<LeadAverageResponse> DashBoard_GetRecurringHours(string _ConString)
        {
            return new BusinessLayer.BL_User().GetEstimatesBySalespersonAverage(_ConString);
        }
        #endregion


        #region Opeartion Tab Chart
        public List<RecurringHoursRemainingResponse> DashBoard_GetTicketRecurringChart(User _User)
        {
            return new BusinessLayer.BL_User().GetTicketRecurringOpenAndCompletedList(_User);

        }
        public List<RecurringHoursRemainingResponse> DashBoard_GetRecurringHoursRemaining(User _User)
        {
            return new BusinessLayer.BL_User().GetRecurringHoursRemainingChart(_User);

        }
        public List<CategoryResponse> DashBoard_GetCategory(User _User)
        {
            return new BusinessLayer.BL_User().GetCategoryList(_User);

        }
        public List<TroubleCallsEquipmentResponse> DashBoard_GetTroubleCallsByEquipment(TroubleCallsByEquipmentGraphRequest _Request)
        {
            return new BusinessLayer.BL_User().GetTroubleCallsByEquipmentList(_Request);

        }
        public List<CategoryTicketCountResponse> DashBoard_GetTicketCountByCategoryAndDateRange(GetCategoryTicketCountParam _Obj)
        {
            return new BusinessLayer.BL_User().GetTicketCountByCategoryList(_Obj);

        }
        #endregion

        #region Financial Tab Chart

        public List<BudgetResponse> DashBoard_GetListBudgetName(string _conString)
        {
            return new BusinessLayer.BL_User().GetBudgetNameList(_conString);
        }      
        public int DashBoard_GetFiscalYear(string _conString)
        {
            User obj = new User();
            obj.ConnConfig = _conString;
            return new BusinessLayer.BL_Report().GetFiscalYear(obj);
        }       
        public List<ActualvsBudgetDataResponse> DashBoard_GetActualvsBudgetedRevenue(string _conString,string budgetID)
        {   
            return new BusinessLayer.BL_Report().GetActualvsBudgetedRevenue(_conString, budgetID);            
        }
        
        public List<CompanyResponse> DashBoard_GetListCompany(CompanyOffice _CompanyOffice)
        {
            return new BusinessLayer.BL_Company().getCompanyListByUserID(_CompanyOffice);
        }
        public List<RevenueResponse> DashBoard_GetRevenueByCompany(User _User)
        {
            return new BusinessLayer.BL_User().GetRevenueByCompany(_User);
        }
        #endregion
    }
}
