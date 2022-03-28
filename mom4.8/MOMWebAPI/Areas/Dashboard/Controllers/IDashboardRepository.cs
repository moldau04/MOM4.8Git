using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessEntity;
using BusinessEntity.APModels;
using BusinessEntity.Payroll;
using BusinessEntity.payroll;

namespace MOMWebAPI.Areas.DashBoard.Controllers
{
    public interface IDashBoardRepository
    {
        //Dashboard
        /// <summary>
        /// This API retrive Dashboard Detail By Id and Without Id
        /// </summary>
        /// <param name="GetDashboardParam"></param>
        /// <param name="ConnectionString"></param>
        /// <returns></returns>
        List<Dashboard> DashBoard_GetDashboard(GetDashboardParam _GetDashboardParam, string ConnectionString);

        //Dashboard
        /// <summary>
        /// This API retrive Dashboard Detail By User Id 
        /// </summary>
        /// <param name="GetDashboardParam"></param>
        /// <param name="ConnectionString"></param>
        /// <returns></returns>
        List<Dashboard> DashBoard_GetDashboardByUserID(GetDashboardParam _GetDashboardParam, string ConnectionString);

        List<QBLastSyncResParam> DashBoard_GetQBlatSync(General _General);
        public string DashBoard_GetGPSInterval(General _User);
        public string DashBoard_GetDefaultCategory(User _User);
        List<KPI> DashBoard_GetListDashKPI(GetListDashKPIParam _GetListDashKPIParam, string ConnectionString);
     
        List<CollectionResponseModel> DashBoard_GetCollectionSummary(GetDashboardCollectionParam _GetDashboardCollectionParam, string ConnectionString);

        public void DashBoard_UpdateDashboardDockStates(UpdateDashboardDockStatesParam _UpdateDashboardDockStatesParam);
        //Sales Tab Chart
        AvgEstimateResponse DashBoard_GetAvgEstimateConversionRate(User _User);
        List<EquipmentTypeCountResponse> DashBoard_GetEquipmentTypeChart(string _conString);
        List<EquipmentBuildingCountResponse> DashBoard_GetEquipmentBuildingChart(string _conString);        
        //RecurringChartResponse DashBoard_GetRecurringHoursChart(User _User);
        List<LeadAverageResponse> DashBoard_GetRecurringHoursChart(User _User);
        List<LeadAverageResponse> DashBoard_GetRecurringHours(string conString);
        //Opeartion Tab Chart
        List<RecurringHoursRemainingResponse> DashBoard_GetTicketRecurringChart(User _User);
        List<RecurringHoursRemainingResponse> DashBoard_GetRecurringHoursRemaining(User _User);
        List<CategoryResponse> DashBoard_GetCategory(User _User);
        List<TroubleCallsEquipmentResponse> DashBoard_GetTroubleCallsByEquipment(TroubleCallsByEquipmentGraphRequest _obj);
        List<CategoryTicketCountResponse> DashBoard_GetTicketCountByCategoryAndDateRange(GetCategoryTicketCountParam _obj);

        //Finanacial Tab Chart
        public List<BudgetResponse> DashBoard_GetListBudgetName(string _conString);        
        public int DashBoard_GetFiscalYear(string _conString);
        public List<ActualvsBudgetDataResponse> DashBoard_GetActualvsBudgetedRevenue(string _conString, string budgetID);
        List<CompanyResponse> DashBoard_GetListCompany(CompanyOffice _CompanyOffice);
        List<RevenueResponse> DashBoard_GetRevenueByCompany(User _User);
        
    }
}
