using BusinessEntity;
using DataLayer;
using System.Data;

namespace BusinessLayer
{
    public class BL_Planner
    {
        DL_Planner _objPlanner = new DL_Planner();
        public void AddPlanner(Planner _planner)
        {
            _objPlanner.AddPlanner(_planner);
        }

        public int AddPlannerNew(Planner _planner)
        {
            return _objPlanner.AddPlannerNew(_planner);
        }

        //public string AddTaskToPlanner(Planner _planner)
        //{
        //    return _objPlanner.AddTaskToPlanner(_planner);
        //}

        public string AddGanttTasksFromMOM(Planner _planner)
        {
            return _objPlanner.AddGanttTasksFromMOM(_planner);
        }


        public DataSet GetPlannerByProjectID(Planner _planner)
        {
            return _objPlanner.GetPlannerByProjectID(_planner);
        }

        public DataSet GetAllVendorSchedules(Planner _planner)
        {
            return _objPlanner.GetAllVendorSchedules(_planner);
        }

        public void UpdatePlannerTitle(Planner _planner)
        {
            _objPlanner.UpdatePlannerTitle(_planner);
        }

        public void DeletePlanner(Planner _planner)
        {
            _objPlanner.DeletePlanner(_planner);
        }

        public void UpdateProjBOMItemVendor(Planner _planner)
        {
            _objPlanner.UpdateProjBOMItemVendor(_planner);
        }

        public DataSet GetGanttTasksByPlannerID(Planner _planner)
        {
            return _objPlanner.GetGanttTasksByPlannerID(_planner);
        }

        public bool IsProjectPlannerExisted(Planner _planner)
        {
            return _objPlanner.IsProjectPlannerExisted(_planner);
        }

        public void UpdateDependencyByPlannerTaskIDs(Planner _planner, string oldList, string newList)
        {
            _objPlanner.UpdateDependencyByPlannerTaskIDs(_planner, oldList, newList);
        }
        
        public DataSet GetProjectPOs(Planner _planner)
        {
            return _objPlanner.GetProjectPOs(_planner);
        }


        public DataSet GetPOsForGanttTask(Planner _planner)
        {
            return _objPlanner.GetPOsForGanttTask(_planner);
        }

        public void DeleteGanttTaskDocs(Planner _planner)
        {
            _objPlanner.DeleteGanttTaskDocs(_planner);
        }
    }
}
