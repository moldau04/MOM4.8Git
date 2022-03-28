using BusinessEntity;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;

namespace DataLayer
{
    public class DL_Planner
    {
        public void AddPlanner(Planner _planner)
        {
            try
            {
                string query = "INSERT INTO Planner([PID],[Desc])"
                + "VALUES(@PID,@Desc)";

                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@PID", _planner.ProjectID));
                parameters.Add(new SqlParameter("@Desc", _planner.Desc));


                int rowsAffected = SqlHelper.ExecuteNonQuery(_planner.ConnConfig, CommandType.Text, query, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int AddPlannerNew(Planner _planner)
        {
            try
            {
                string query = "spAddPlanner";

                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@PID", _planner.ProjectID));
                parameters.Add(new SqlParameter("@Desc", _planner.Desc));
                parameters.Add(new SqlParameter("@Type", _planner.Type));

                DataSet ds = new DataSet();
                ds = SqlHelper.ExecuteDataset(_planner.ConnConfig, CommandType.StoredProcedure, query, parameters.ToArray());

                return Convert.ToInt32(ds.Tables[0].Rows[0][0]);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public String AddTaskToPlanner(Planner _planner)
        //{
        //    try
        //    {
        //        string query = "spAddTaskToPlanner";

        //        List<SqlParameter> parameters = new List<SqlParameter>();
        //        parameters.Add(new SqlParameter("@ParentID", _planner.ParentID));
        //        parameters.Add(new SqlParameter("@Name", _planner.TaskName));
        //        parameters.Add(new SqlParameter("@idx", _planner.idx));
        //        parameters.Add(new SqlParameter("@ProjectID", _planner.ProjectID));
        //        parameters.Add(new SqlParameter("@TaskType", _planner.TaskType));
        //        parameters.Add(new SqlParameter("@Duration", _planner.Duration));
        //        parameters.Add(new SqlParameter("@DurationUnit", _planner.DurationUnit));
        //        parameters.Add(new SqlParameter("@StartDate", _planner.StartDate));

        //        DataSet ds = new DataSet();
        //        ds = SqlHelper.ExecuteDataset(_planner.ConnConfig, CommandType.StoredProcedure, query, parameters.ToArray());

        //        return Convert.ToString(ds.Tables[0].Rows[0][0]);
        //    }
        //    catch (Exception ex)
        //    {
        //        return "0";
        //    }
        //}

        public String AddGanttTasksFromMOM(Planner _planner)
        {
            try
            {
                string query = "spAddGanttTasksFromMOM";

                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@ParentID", _planner.ParentID));
                parameters.Add(new SqlParameter("@Name", _planner.TaskName));
                parameters.Add(new SqlParameter("@OrderID", _planner.idx));
                parameters.Add(new SqlParameter("@ProjectID", _planner.ProjectID));
                parameters.Add(new SqlParameter("@PlannerID", _planner.PlannerID));
                parameters.Add(new SqlParameter("@TaskType", _planner.TaskType));
                parameters.Add(new SqlParameter("@Duration", _planner.Duration));
                parameters.Add(new SqlParameter("@StartDate", _planner.StartDate));
                parameters.Add(new SqlParameter("@EndDate", _planner.EndDate));
                parameters.Add(new SqlParameter("@Summary", _planner.Summary));
                parameters.Add(new SqlParameter("@Notes", _planner.Desc));
                parameters.Add(new SqlParameter("@VendorId", _planner.VendorID));
                parameters.Add(new SqlParameter("@VendorName", _planner.VendorName));
                parameters.Add(new SqlParameter("@RootVendorID", _planner.RootVendorID));
                parameters.Add(new SqlParameter("@RootVendorName", _planner.RootVendorName));
                parameters.Add(new SqlParameter("@ProjectName", _planner.ProjectName));
                parameters.Add(new SqlParameter("@ItemRefID", _planner.ItemRefID));
                parameters.Add(new SqlParameter("@ActualHours", _planner.ActualHours));

                DataSet ds = new DataSet();
                ds = SqlHelper.ExecuteDataset(_planner.ConnConfig, CommandType.StoredProcedure, query, parameters.ToArray());

                return Convert.ToString(ds.Tables[0].Rows[0][0]);
            }
            catch (Exception ex)
            {
                return "0";
            }
        }

        public DataSet GetPlannerByProjectID(Planner _planner)
        {
            DataSet ds = new DataSet();
            try
            {
                //String query = "Select * from Planner Where PID=" + _planner.ProjectID;
                String query = "spGetPlannerInfo";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@ProjectID", _planner.ProjectID));
                //parameters.Add(new SqlParameter("@PlannerID", _planner.PlannerID));
                ds = SqlHelper.ExecuteDataset(_planner.ConnConfig, CommandType.StoredProcedure, query, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ds;
        }

        public DataSet GetAllVendorSchedules(Planner _planner)
        {
            DataSet ds = new DataSet();
            try
            {
                String query = "spAllVendorSchedules";
                ds = SqlHelper.ExecuteDataset(_planner.ConnConfig, CommandType.StoredProcedure, query);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ds;
        }

        public void UpdatePlannerTitle(Planner _planner)
        {
            try
            {
                string query = "spUpdatePlannerTitle";

                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@ID", _planner.PlannerID));
                parameters.Add(new SqlParameter("@Desc", _planner.Desc));

                DataSet ds = new DataSet();
                ds = SqlHelper.ExecuteDataset(_planner.ConnConfig, CommandType.StoredProcedure, query, parameters.ToArray());

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeletePlanner(Planner _planner)
        {
            try
            {
                string query = "spDeletePlanner";

                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@ID", _planner.PlannerID));

                DataSet ds = new DataSet();
                ds = SqlHelper.ExecuteDataset(_planner.ConnConfig, CommandType.StoredProcedure, query, parameters.ToArray());

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateProjBOMItemVendor(Planner _planner)
        {
            string query = "spUpdateProjBOMItemVendor";

            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@jobId", _planner.ProjectID));
            //parameters.Add(new SqlParameter("@jobTItemId", _planner.ItemRefID));
            parameters.Add(new SqlParameter("@vendorId", _planner.VendorID));
            parameters.Add(new SqlParameter("@GanttTaskId", _planner.ItemRefID));


            SqlHelper.ExecuteNonQuery(_planner.ConnConfig, CommandType.StoredProcedure, query, parameters.ToArray());
        }

        public DataSet GetGanttTasksByPlannerID(Planner _planner)
        {
            try
            {
                string query = "spGetGanttTasksByPlannerID";

                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@plannerID", _planner.PlannerID));

                DataSet ds = new DataSet();
                ds = SqlHelper.ExecuteDataset(_planner.ConnConfig, CommandType.StoredProcedure, query, parameters.ToArray());
                return ds;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool IsProjectPlannerExisted(Planner _planner)
        {
            try
            {
                if (_planner.ProjectID != 0)
                {
                    string query = string.Format("SELECT 1 FROM PLANNER WHERE PID = {0} AND (Type is null Or Type = 'Project')", _planner.ProjectID);

                    DataSet ds = new DataSet();
                    ds = SqlHelper.ExecuteDataset(_planner.ConnConfig, CommandType.Text, query);

                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0) return true;
                    else return false;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateDependencyByPlannerTaskIDs(Planner _planner, string oldListStr, string newListStr)
        {
            string query = "spUpdateDependencyByPlannerTaskIDs";

            if (!string.IsNullOrWhiteSpace(newListStr))
            {
                //Regex _regex = new Regex(@"[0-9,]");
                if (Regex.IsMatch(newListStr, @"^[0-9,]+$"))
                {
                    if (string.IsNullOrEmpty(oldListStr)) oldListStr = string.Empty;
                    var oldArr = oldListStr.Split(',');
                    var oldList = new List<string>();
                    if (oldArr.Count() > 0)
                    {
                        foreach (var item in oldArr)
                        {
                            if (!string.IsNullOrWhiteSpace(item))
                            {
                                oldList.Add(item);
                            }
                        }
                    }

                    var newArr = newListStr.Split(',');
                    var newList = new List<string>();
                    if (newArr.Count() > 0)
                    {
                        foreach (var item in newArr)
                        {
                            if (!string.IsNullOrWhiteSpace(item))
                            {
                                newList.Add(item);
                            }
                        }
                    }
                    var delList = oldList.Where(l => newList.All(f => f != l));
                    var insertList = newList.Where(l => oldList.All(f => f != l));
                    var delListStr = string.Join(",", delList);
                    var insertListStr = string.Join(",", insertList);
                    newListStr = string.Join(",", newList);

                    List<SqlParameter> parameters = new List<SqlParameter>();
                    parameters.Add(new SqlParameter("@jobId", _planner.ProjectID));
                    parameters.Add(new SqlParameter("@plannerId", _planner.PlannerID));
                    parameters.Add(new SqlParameter("@taskId", _planner.idx));
                    parameters.Add(new SqlParameter("@delList", delListStr));
                    parameters.Add(new SqlParameter("@insertList", insertListStr));
                    parameters.Add(new SqlParameter("@newList", newListStr));

                    SqlHelper.ExecuteNonQuery(_planner.ConnConfig, CommandType.StoredProcedure, query, parameters.ToArray());
                }
                else
                {
                    throw new Exception("Your input list is not correct format");
                }
            }
            else
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@jobId", _planner.ProjectID));
                parameters.Add(new SqlParameter("@plannerId", _planner.PlannerID));
                parameters.Add(new SqlParameter("@taskId", _planner.idx));
                parameters.Add(new SqlParameter("@delList", ""));
                parameters.Add(new SqlParameter("@insertList", ""));
                parameters.Add(new SqlParameter("@newList", ""));

                SqlHelper.ExecuteNonQuery(_planner.ConnConfig, CommandType.StoredProcedure, query, parameters.ToArray());
            }
        }

        public DataSet GetProjectPOs(Planner _planner)
        {
            try
            {
                string query = "spGetProjectPOs";

                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@projectId", _planner.ProjectID));

                DataSet ds = new DataSet();
                ds = SqlHelper.ExecuteDataset(_planner.ConnConfig, CommandType.StoredProcedure, query, parameters.ToArray());
                return ds;
            }
            catch (Exception ex)
            {

                throw new Exception("GetProjectPOs error", ex);
            }
            
        }

        public DataSet GetPOsForGanttTask(Planner _planner)
        {
            try
            {
                string query = "spGetPOsForGanttTask";

                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@projectId", _planner.ProjectID));
                parameters.Add(new SqlParameter("@ganttTaskId", _planner.idx));

                DataSet ds = new DataSet();
                ds = SqlHelper.ExecuteDataset(_planner.ConnConfig, CommandType.StoredProcedure, query, parameters.ToArray());
                return ds;
            }
            catch (Exception ex)
            {

                throw new Exception("GetPOsForGanttTask error", ex);
            }

        }

        public void DeleteGanttTaskDocs(Planner _planner)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(_planner.ConnConfig, "spDeleteGanttTaskDocs", _planner.idx);
                //SqlHelper.ExecuteNonQuery(objPropMapData.ConnConfig, CommandType.Text, "delete from documents where id=" + objPropMapData.DocumentID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
