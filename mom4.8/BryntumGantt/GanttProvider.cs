using BusinessEntity;
using BusinessLayer;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Telerik.Web.UI;
using Telerik.Web.UI.Gantt;

//namespace App_Code
//{
public class GanttProvider : GanttProviderBase
{
    public string connStrGantt = string.Empty;

    public GanttProvider()
    {
        connStrGantt = "metadata=res://*/;provider=System.Data.SqlClient;provider connection string=\"" + Convert.ToString(HttpContext.Current.Session["config"]) + ";MultipleActiveResultSets=True;App=EntityFramework;\"";
    }

    #region Tasks
    public override ITaskFactory TaskFactory
    {
        get
        {
            return new CustomGanttTaskFactory();
        }
    }
    //public override List<ITask> GetTasks()
    //{
    //    var tasks = new List<ITask>();

    //    var plannerRefId = HttpContext.Current.Session["GanttPlannerRefId"] != null ? Convert.ToInt32(HttpContext.Current.Session["GanttPlannerRefId"]) : 0;
    //    var plannerId = HttpContext.Current.Session["GanttPlannerID"] != null ? Convert.ToInt32(HttpContext.Current.Session["GanttPlannerID"]) : 0;
    //    var plannerType = HttpContext.Current.Session["GanttPlannerType"] != null ? HttpContext.Current.Session["GanttPlannerType"].ToString() : "project";



    //    if (plannerType.Equals("project", StringComparison.InvariantCultureIgnoreCase))
    //    {
    //        var isPlannerExisted = true;
    //        if (plannerId != 0 && plannerRefId != 0)
    //        {
    //            Planner objPlanner = new Planner();
    //            BL_Planner objBL_Planner = new BL_Planner();
    //            objPlanner.ConnConfig = HttpContext.Current.Session["config"].ToString();
    //            objPlanner.ProjectID = plannerRefId;
    //            //objPlanner.PlannerID = plannerId;
    //            isPlannerExisted = objBL_Planner.IsProjectPlannerExisted(objPlanner);
    //            if (isPlannerExisted)
    //            {
    //                using (var db = new GanttResourcesEntities(connStrGantt))
    //                {
    //                    tasks.AddRange(db.GanttTasks.Where(x => x.ProjectID == plannerRefId && x.PlannerID == plannerId).ToList().Select(task => new CustomTask
    //                    {
    //                        ID = task.ID,
    //                        ParentID = task.ParentID,
    //                        OrderID = task.OrderID,
    //                        Start = task.Start,
    //                        End = task.End,
    //                        PercentComplete = task.PercentComplete,
    //                        Summary = task.Summary,
    //                        Title = task.Title,
    //                        Expanded = task.Expanded.HasValue && task.Expanded.Value,
    //                        Description = task.Description,
    //                        ProjectID = task.ProjectID,
    //                        PlannerID = task.PlannerID,
    //                        CusTaskType = task.CusTaskType,
    //                        CusDuration = task.CusDuration,
    //                        CusActualHour = task.CusActualHour,
    //                        VendorID = task.VendorID,
    //                        Vendor = task.Vendor,
    //                        RootVendorID = task.RootVendorID,
    //                        RootVendorName = task.RootVendorName,
    //                        ProjectName = task.ProjectName,
    //                        PlannerTaskID = task.PlannerTaskID,
    //                        ItemRefID = task.ItemRefID,
    //                        Dependency = task.Dependency
    //                    }));
    //                }
    //            }
    //        }
    //    }
    //    else if (plannerType.Equals("vendor", StringComparison.InvariantCultureIgnoreCase))
    //    {
    //        using (var db = new GanttResourcesEntities(connStrGantt))
    //        {
    //            tasks.AddRange(db.GanttTasks.Where(x => x.RootVendorID == plannerRefId && x.PlannerID == plannerId).ToList().Select(task => new CustomTask
    //            {
    //                ID = task.ID,
    //                ParentID = task.ParentID,
    //                OrderID = task.OrderID,
    //                Start = task.Start,
    //                End = task.End,
    //                PercentComplete = task.PercentComplete,
    //                Summary = task.Summary,
    //                Title = task.Title,
    //                Expanded = task.Expanded.HasValue && task.Expanded.Value,
    //                Description = task.Description,
    //                ProjectID = task.ProjectID,
    //                PlannerID = task.PlannerID,
    //                CusTaskType = task.CusTaskType,
    //                CusDuration = task.CusDuration,
    //                CusActualHour = task.CusActualHour,
    //                VendorID = task.VendorID,
    //                Vendor = task.Vendor,
    //                RootVendorID = task.RootVendorID,
    //                RootVendorName = task.RootVendorName,
    //                ProjectName = task.ProjectName,
    //                PlannerTaskID = task.PlannerTaskID,
    //                ItemRefID = task.ItemRefID,
    //                Dependency = task.Dependency
    //            }));
    //        }
    //    }



    //    return tasks;
    //}

    //public override ITask UpdateTask(ITask task)
    //{
    //    bool isVendorUpdate = false;
    //    bool isDependencyUpdate = false;
    //    CustomTask cusTask = (CustomTask)task;
    //    var connStr = HttpContext.Current.Session["config"].ToString();
    //    BL_Vendor bL_Vendor = new BL_Vendor();
    //    var vendorID = cusTask.VendorID.HasValue ? cusTask.VendorID.Value : 0;
    //    var vendorNameByID = string.Empty;
    //    var curDependency = string.Empty;
    //    if (vendorID != 0)
    //    {
    //        vendorNameByID = bL_Vendor.GetVendorNameById(vendorID, connStr);
    //    }

    //    using (var db = new GanttResourcesEntities(connStrGantt))
    //    {
    //        int? pId = null;
    //        int id = int.Parse(task.ID.ToString());
    //        var t = db.GanttTasks.First(r => r.ID == id);

    //        if (task.ParentID != null)
    //        {
    //            pId = int.Parse(task.ParentID.ToString());
    //        }

    //        t.ID = id;
    //        t.ParentID = pId;
    //        t.OrderID = int.Parse(task.OrderID.ToString());
    //        t.Summary = task.Summary;
    //        t.Title = task.Title;
    //        t.Expanded = task.Expanded;
    //        var cusDura = ((CustomTask)task).CusDuration;
    //        t.Start = task.Start;
    //        if (t.CusDuration.HasValue && cusDura.HasValue && t.CusDuration.Value != cusDura.Value)
    //        {
    //            t.CusDuration = cusDura;
    //            // Re-calulate EndDate
    //            var days = (int)cusDura / 8;
    //            double hour = (double)cusDura % 8;
    //            //var endDate = task.Start.AddDays(days).AddHours(hour);
    //            var endDate = CalculateBusinessDaysFromInputDate(t.Start, days).AddHours(hour);
    //            t.End = endDate;
    //        }
    //        else
    //        {
    //            t.End = task.End;
    //            t.CusDuration = (double)CalculateBusinessHours(t.Start.ToLocalTime(), t.End.ToLocalTime());
    //        }

    //        var percent = task.PercentComplete;
    //        if (t.PercentComplete != percent)
    //        {
    //            t.PercentComplete = percent;
    //            //t.CusActualHour = percent * t.CusDuration;
    //        }
    //        //else
    //        //{
    //        //    var cusActual = ((CustomTask)task).CusActualHour;
    //        //    t.CusActualHour = cusActual;

    //        //    if (cusActual.Value >= t.CusDuration.Value)
    //        //    {
    //        //        t.PercentComplete = 1;
    //        //        t.CusActualHour = t.CusDuration.Value;
    //        //    }
    //        //    else
    //        //    {
    //        //        if (t.CusDuration.HasValue && t.CusDuration.Value != 0)
    //        //        {
    //        //            t.PercentComplete = (cusActual.Value / t.CusDuration.Value);
    //        //        }
    //        //    }
    //        //}

    //        if (t.VendorID != cusTask.VendorID) isVendorUpdate = true;

    //        t.Description = ((CustomTask)task).Description;
    //        t.PlannerID = ((CustomTask)task).PlannerID;
    //        t.CusTaskType = ((CustomTask)task).CusTaskType;
    //        t.VendorID = ((CustomTask)task).VendorID;
    //        //t.Vendor = ((CustomTask)task).Vendor;
    //        if (string.IsNullOrEmpty(vendorNameByID))
    //        {
    //            t.Vendor = cusTask.Vendor;
    //        }
    //        else
    //        {
    //            t.Vendor = vendorNameByID;
    //        }

    //        t.RootVendorID = ((CustomTask)task).RootVendorID;
    //        t.RootVendorName = ((CustomTask)task).RootVendorName;
    //        t.ProjectID = ((CustomTask)task).ProjectID;
    //        t.ProjectName = ((CustomTask)task).ProjectName;

    //        if (t.Dependency != ((CustomTask)task).Dependency) isDependencyUpdate = true;
    //        curDependency = t.Dependency;

    //        db.SaveChanges();
    //        //t.Start = task.Start;
    //        //t.End = task.End;
    //        //t.PercentComplete = task.PercentComplete;
    //        //t.Summary = task.Summary;
    //        //t.Title = task.Title;
    //        //t.Expanded = task.Expanded;
    //        //db.SaveChanges();
    //        //var currTask = db.GanttTasks.Find(task.ID);
    //        //GanttTask entityTask = ToEntityTask(task);
    //        //db.GanttTasks.Attach(entityTask);
    //        //db.Entry(entityTask).State = EntityState.Modified;
    //        //db.SaveChanges();
    //    }

    //    // Updated BOM items once update Task vendor
    //    var projectID = cusTask.ProjectID.HasValue ? cusTask.ProjectID.Value : 0;
    //    if (isVendorUpdate && cusTask.CusTaskType.Equals("BOM", StringComparison.InvariantCultureIgnoreCase))
    //    {
    //        //var itemRefId = cusTask.ItemRefID.HasValue ? cusTask.ItemRefID.Value : 0;
    //        if (projectID != 0)// && itemRefId != 0)
    //        {
    //            Planner objPlanner = new Planner();
    //            BL_Planner objBL_Planner = new BL_Planner();
    //            objPlanner.ConnConfig = HttpContext.Current.Session["config"].ToString();
    //            objPlanner.ProjectID = projectID;
    //            //objPlanner.ItemRefID = itemRefId;
    //            objPlanner.VendorID = cusTask.VendorID.HasValue ? cusTask.VendorID.Value : 0;
    //            objPlanner.ItemRefID = (int)cusTask.ID;
    //            objBL_Planner.UpdateProjBOMItemVendor(objPlanner);
    //        }
    //    }

    //    if (isDependencyUpdate)
    //    {
    //        var plannerID = cusTask.PlannerID.HasValue ? cusTask.PlannerID.Value : 0;
    //        var newDependency = ((CustomTask)task).Dependency;

    //        Planner objPlanner = new Planner();
    //        BL_Planner objBL_Planner = new BL_Planner();
    //        objPlanner.ConnConfig = HttpContext.Current.Session["config"].ToString();
    //        objPlanner.ProjectID = projectID;
    //        objPlanner.PlannerID = plannerID;
    //        objPlanner.idx = (int)cusTask.ID;
    //        objBL_Planner.UpdateDependencyByPlannerTaskIDs(objPlanner, curDependency, newDependency);
    //    }

    //    return task;
    //}

    //public override ITask DeleteTask(ITask task)
    //{
    //    using (var db = new GanttResourcesEntities(connStrGantt))
    //    {
    //        int id = int.Parse(task.ID.ToString());
    //        db.GanttTasks.Remove(db.GanttTasks.First(r => r.ID == id));
    //        db.SaveChanges();
    //    }

    //    Planner objPlanner = new Planner();
    //    BL_Planner objBL_Planner = new BL_Planner();
    //    objPlanner.ConnConfig = HttpContext.Current.Session["config"].ToString();
    //    objPlanner.idx = (int)task.ID;
    //    objBL_Planner.DeleteGanttTaskDocs(objPlanner);

    //    return task;
    //}

    //public override ITask InsertTask(ITask task)
    //{
    //    //using (var db = new GanttResourcesEntities(connStr))
    //    //{
    //    //    int? pId = null;
    //    //    int id = string.IsNullOrEmpty(task.ID.ToString()) ? 0 : int.Parse(task.ID.ToString());
    //    //    task.ID = null;

    //    //    if (task.ParentID != null)
    //    //    {
    //    //        pId = int.Parse(task.ParentID.ToString());
    //    //    }

    //    //    //GanttTask result = new GanttTask
    //    //    //{
    //    //    //    ID = id,
    //    //    //    ParentID = pId,
    //    //    //    OrderID = int.Parse(task.OrderID.ToString()),
    //    //    //    Start = task.Start,
    //    //    //    End = task.End,
    //    //    //    PercentComplete = task.PercentComplete,
    //    //    //    Summary = task.Summary,
    //    //    //    Title = task.Title,
    //    //    //    Expanded = task.Expanded
    //    //    //};
    //    //    //db.GanttTasks.Add(result);
    //    //    //db.SaveChanges();

    //    //    GanttTask result = ToEntityTask(task);
    //    //    db.GanttTasks.Add(result);
    //    //    db.SaveChanges();
    //    //    task.ID = result.ID;
    //    //    task.ParentID = result.ParentID;
    //    //    task.OrderID = result.OrderID;
    //    //}

    //    //return task;

    //    using (var db = new GanttResourcesEntities(connStrGantt))
    //    {
    //        task.ID = 0; // Value will be updated from DB

    //        GanttTask entityTask = ToEntityTask(task);
    //        db.GanttTasks.Add(entityTask);
    //        db.SaveChanges();

    //        task.ID = entityTask.ID;
    //    }

    //    return task;
    //}

    #endregion

    #region Dependencies

    //public override List<IDependency> GetDependencies()
    //{
    //    var dependencies = new List<IDependency>();
    //    using (var db = new GanttResourcesEntities(connStrGantt))
    //    {
    //        dependencies.AddRange(db.GanttDependencies.ToList().Select(dependency => new Dependency()
    //        {
    //            ID = dependency.ID,
    //            PredecessorID = dependency.PredecessorID,
    //            SuccessorID = dependency.SuccessorID,
    //            Type = (DependencyType)dependency.Type
    //        }));
    //    }
    //    return dependencies;
    //}

    //public string GetSuccessorByTaskID(int taskID)
    //{
    //    var dependencies = new List<IDependency>();
    //    string rt = string.Empty;
    //    using (var db = new GanttResourcesEntities(connStrGantt))
    //    {
    //        //var temp = db.GanttDependencies.Where(d => d.PredecessorID == taskID).ToList();
    //        var temp = from d in db.GanttDependencies
    //                   join t in db.GanttTasks on d.SuccessorID equals t.ID
    //                   where d.PredecessorID == taskID
    //                   select new
    //                   {
    //                       t.PlannerTaskID
    //                   };
    //        if (temp.ToList().Count > 0)
    //        {
    //            foreach (var item in temp.ToList())
    //            {
    //                rt += item.PlannerTaskID + ",";
    //            }
    //            if (rt.Length > 0) rt = rt.Trim(',');
    //        }
    //    }
    //    return rt;
    //}

    //public string GetPredecessorByTaskID(int taskID)
    //{
    //    var dependencies = new List<IDependency>();
    //    string rt = string.Empty;
    //    using (var db = new GanttResourcesEntities(connStrGantt))
    //    {
    //        //var temp = db.GanttDependencies.Where(d => d.SuccessorID == taskID).ToList();
    //        //if (temp.Count > 0)
    //        //{
    //        //    foreach (var item in temp)
    //        //    {
    //        //        rt += item.PredecessorID + ",";
    //        //    }
    //        //    if (rt.Length > 0) rt = rt.Trim(',');
    //        //}

    //        var temp = from d in db.GanttDependencies
    //                   join t in db.GanttTasks on d.PredecessorID equals t.ID
    //                   where d.SuccessorID == taskID
    //                   select new
    //                   {
    //                       t.PlannerTaskID
    //                   };

    //        if (temp.ToList().Count > 0)
    //        {
    //            foreach (var item in temp.ToList())
    //            {
    //                rt += item.PlannerTaskID + ",";
    //            }
    //            if (rt.Length > 0) rt = rt.Trim(',');
    //        }
    //    }
    //    return rt;
    //}

    //private int GetMaxPlannerTaskID(int plannerId)
    //{
    //    int rtVal = 0;
    //    using (var db = new GanttResourcesEntities(connStrGantt))
    //    {
    //        var temp = db.GanttTasks.Where(t => t.PlannerID == plannerId).OrderByDescending(t => t.PlannerTaskID).FirstOrDefault();
    //        if (temp != null)
    //        {
    //            rtVal = temp.PlannerTaskID.HasValue ? temp.PlannerTaskID.Value : 0;
    //        }
    //    }
    //    return rtVal;
    //}

    //public override IDependency UpdateDependency(IDependency dependency)
    //{
    //    return dependency;
    //}

    //public override IDependency DeleteDependency(IDependency dependency)
    //{
    //    using (var db = new GanttResourcesEntities(connStrGantt))
    //    {
    //        int id = int.Parse(dependency.ID.ToString());
    //        db.GanttDependencies.Remove(db.GanttDependencies.First(r => r.ID == id));
    //        db.SaveChanges();
    //    }

    //    return dependency;
    //}

    //public override IDependency InsertDependency(IDependency dependency)
    //{
    //    using (var db = new GanttResourcesEntities(connStrGantt))
    //    {
    //        //int id = int.Parse(dependency.ID.ToString());
    //        //int id = string.IsNullOrEmpty(dependency.ID.ToString()) ? 0 : int.Parse(dependency.ID.ToString());
    //        int pId = int.Parse(dependency.PredecessorID.ToString());
    //        int sId = int.Parse(dependency.SuccessorID.ToString());

    //        var result = new GanttDependency { PredecessorID = pId, SuccessorID = sId, Type = (int)dependency.Type };
    //        db.GanttDependencies.Add(result);

    //        db.SaveChanges();

    //        dependency.ID = result.ID;
    //        dependency.PredecessorID = result.ID;
    //        dependency.SuccessorID = result.SuccessorID;
    //    }

    //    return dependency;
    //}

    #endregion

    #region Resources
    public override List<IResource> GetResources()
    {
        var resources = new List<IResource>();
        using (var db = new GanttResourcesEntities(connStrGantt))
        {
            resources.AddRange(db.GanttResources.ToList().Select(resource => new Telerik.Web.UI.Gantt.Resource()
            {
                ID = resource.ID,
                Text = resource.Name,
                Color = System.Drawing.ColorTranslator.FromHtml(resource.Color)
            }));
        }
        return resources;
    }
    #endregion

    #region Assignments
    public override List<IAssignment> GetAssignments()
    {
        var assignments = new List<IAssignment>();
        using (var db = new GanttResourcesEntities(connStrGantt))
        {
            assignments.AddRange(db.GanttResourceAssignments.ToList().Select(assignment => new Assignment()
            {
                ID = assignment.ID,
                ResourceID = assignment.ResourceID,
                TaskID = assignment.TaskID,
                Units = assignment.Units
            }));
        }
        return assignments;
    }

    public override IAssignment UpdateAssignment(IAssignment assignment)
    {
        int id = int.Parse(assignment.ID.ToString()),
            rId = int.Parse(assignment.ResourceID.ToString()),
            tId = int.Parse(assignment.TaskID.ToString());

        decimal units = decimal.Parse(assignment.Units.ToString());

        using (var db = new GanttResourcesEntities(connStrGantt))
        {
            var dbAssignment = db.GanttResourceAssignments.Where(x => x.ID == id).FirstOrDefault();
            dbAssignment.ResourceID = rId;
            dbAssignment.TaskID = tId;
            dbAssignment.Units = units;

            db.SaveChanges();
        }
        return assignment;
    }

    public override IAssignment InsertAssignment(IAssignment assignment)
    {
        using (var db = new GanttResourcesEntities(connStrGantt))
        {
            int rId = int.Parse(assignment.ResourceID.ToString());
            int tId = int.Parse(assignment.TaskID.ToString());
            decimal units = decimal.Parse(assignment.Units.ToString());

            var result = new GanttResourceAssignment { ResourceID = rId, TaskID = tId, Units = units };
            db.GanttResourceAssignments.Add(result);
            db.SaveChanges();

            assignment.ID = result.ID;
            assignment.ResourceID = result.ResourceID;
            assignment.TaskID = result.TaskID;
        }
        return assignment;
    }

    public override IAssignment DeleteAssignment(IAssignment assignment)
    {
        using (var db = new GanttResourcesEntities(connStrGantt))
        {
            int id = int.Parse(assignment.ID.ToString());
            db.GanttResourceAssignments.Remove(db.GanttResourceAssignments.First(r => r.ID == id));
            db.SaveChanges();
        }

        return assignment;
    }
    #endregion
    #region Helpers

    //private GanttTask ToEntityTask(ITask srcTask)
    //{
    //    var plannerRefId = HttpContext.Current.Session["GanttPlannerRefId"] != null ? Convert.ToInt32(HttpContext.Current.Session["GanttPlannerRefId"]) : 0;
    //    var plannerId = HttpContext.Current.Session["GanttPlannerID"] != null ? Convert.ToInt32(HttpContext.Current.Session["GanttPlannerID"]) : 0;
    //    var plannerType = HttpContext.Current.Session["GanttPlannerType"] != null ? HttpContext.Current.Session["GanttPlannerType"].ToString() : "project";
    //    if (plannerType.Equals("vendor", StringComparison.InvariantCultureIgnoreCase))
    //    {
    //        var tasks = new List<CustomTask>();
    //        using (var db = new GanttResourcesEntities(connStrGantt))
    //        {
    //            if (srcTask.ParentID != null && (int?)srcTask.ParentID != 0)
    //            {
    //                tasks.AddRange(db.GanttTasks.Where(x => x.RootVendorID == plannerRefId && x.PlannerID == plannerId && x.ID == (int?)srcTask.ParentID
    //                ).ToList().Select(task => new CustomTask
    //                {
    //                    ID = task.ID,
    //                    ParentID = task.ParentID,
    //                    OrderID = task.OrderID,
    //                    Start = task.Start,
    //                    End = task.End,
    //                    PercentComplete = task.PercentComplete,
    //                    Summary = task.Summary,
    //                    Title = task.Title,
    //                    Expanded = task.Expanded.HasValue && task.Expanded.Value,
    //                    Description = task.Description,
    //                    ProjectID = task.ProjectID,
    //                    PlannerID = task.PlannerID,
    //                    CusTaskType = task.CusTaskType,
    //                    CusDuration = task.CusDuration,
    //                    CusActualHour = task.CusActualHour,
    //                    VendorID = task.VendorID,
    //                    Vendor = task.Vendor,
    //                    RootVendorID = task.RootVendorID,
    //                    RootVendorName = task.RootVendorName,
    //                    ProjectName = task.ProjectName,
    //                    PlannerTaskID = task.PlannerTaskID,
    //                    ItemRefID = task.ItemRefID
    //                }));
    //            }
    //            else
    //            {
    //                tasks.AddRange(db.GanttTasks.Where(x => x.RootVendorID == plannerRefId && x.PlannerID == plannerId && (!x.ParentID.HasValue || x.ParentID.Value == 0)
    //                ).ToList().Select(task => new CustomTask
    //                {
    //                    ID = task.ID,
    //                    ParentID = task.ParentID,
    //                    OrderID = task.OrderID,
    //                    Start = task.Start,
    //                    End = task.End,
    //                    PercentComplete = task.PercentComplete,
    //                    Summary = task.Summary,
    //                    Title = task.Title,
    //                    Expanded = task.Expanded.HasValue && task.Expanded.Value,
    //                    Description = task.Description,
    //                    ProjectID = task.ProjectID,
    //                    PlannerID = task.PlannerID,
    //                    CusTaskType = task.CusTaskType,
    //                    CusDuration = task.CusDuration,
    //                    CusActualHour = task.CusActualHour,
    //                    VendorID = task.VendorID,
    //                    Vendor = task.Vendor,
    //                    RootVendorID = task.RootVendorID,
    //                    RootVendorName = task.RootVendorName,
    //                    ProjectName = task.ProjectName,
    //                    PlannerTaskID = task.PlannerTaskID,
    //                    ItemRefID = task.ItemRefID
    //                }));
    //            }
    //        }
    //        CustomTask parentTask = tasks.FirstOrDefault();
    //        return new GanttTask
    //        {
    //            ID = (int)srcTask.ID,
    //            ParentID = (int?)srcTask.ParentID,
    //            OrderID = (int)srcTask.OrderID,
    //            Start = srcTask.Start,
    //            End = srcTask.End,
    //            PercentComplete = srcTask.PercentComplete,
    //            Summary = srcTask.Summary,
    //            Title = srcTask.Title,
    //            Expanded = srcTask.Expanded,
    //            Description = ((CustomTask)srcTask).Description,
    //            ProjectID = parentTask.ProjectID,//((CustomTask)srcTask).ProjectID,
    //            PlannerID = parentTask.PlannerID,//((CustomTask)srcTask).PlannerID,
    //            CusTaskType = ((CustomTask)srcTask).CusTaskType,
    //            CusDuration = ((CustomTask)srcTask).CusDuration,
    //            CusActualHour = ((CustomTask)srcTask).CusActualHour,
    //            VendorID = ((CustomTask)srcTask).VendorID,
    //            Vendor = ((CustomTask)srcTask).Vendor,
    //            RootVendorID = parentTask.RootVendorID,//((CustomTask)srcTask).RootVendorID,
    //            RootVendorName = parentTask.RootVendorName,//((CustomTask)srcTask).RootVendorName,
    //            ProjectName = parentTask.ProjectName,//((CustomTask)srcTask).ProjectName
    //            PlannerTaskID = parentTask.PlannerTaskID,
    //            ItemRefID = parentTask.ItemRefID

    //        };
    //    }
    //    else
    //    {
    //        var plannerTaskId = GetMaxPlannerTaskID(plannerId) + 1;
    //        return new GanttTask
    //        {
    //            ID = (int)srcTask.ID,
    //            ParentID = (int?)srcTask.ParentID,
    //            OrderID = (int)srcTask.OrderID,
    //            Start = srcTask.Start,
    //            End = srcTask.End,
    //            PercentComplete = srcTask.PercentComplete,
    //            Summary = srcTask.Summary,
    //            Title = srcTask.Title,
    //            Expanded = srcTask.Expanded,
    //            Description = ((CustomTask)srcTask).Description,
    //            ProjectID = ((CustomTask)srcTask).ProjectID,
    //            PlannerID = ((CustomTask)srcTask).PlannerID,
    //            CusTaskType = ((CustomTask)srcTask).CusTaskType,
    //            CusDuration = ((CustomTask)srcTask).CusDuration,
    //            CusActualHour = ((CustomTask)srcTask).CusActualHour,
    //            VendorID = ((CustomTask)srcTask).VendorID,
    //            Vendor = ((CustomTask)srcTask).Vendor,
    //            RootVendorID = ((CustomTask)srcTask).RootVendorID,
    //            RootVendorName = ((CustomTask)srcTask).RootVendorName,
    //            ProjectName = ((CustomTask)srcTask).ProjectName,
    //            PlannerTaskID = plannerTaskId,
    //            ItemRefID = ((CustomTask)srcTask).ItemRefID
    //        };
    //    }
    //}

    //private GanttDependency ToEntityDependency(IDependency srcDependency)
    //{
    //    return new GanttDependency
    //    {
    //        ID = (int)srcDependency.ID,
    //        PredecessorID = (int)srcDependency.PredecessorID,
    //        SuccessorID = (int)srcDependency.SuccessorID,
    //        Type = (int)srcDependency.Type
    //    };
    //}

    #endregion

    public System.DateTime CalculateBusinessDaysFromInputDate(System.DateTime StartDate, int NumberOfBusinessDays)
    {
        //Knock the start date down one day if it is on a weekend.
        if (StartDate.DayOfWeek == DayOfWeek.Saturday |
            StartDate.DayOfWeek == DayOfWeek.Sunday)
        {
            NumberOfBusinessDays -= 1;
        }

        int index = 0;

        for (index = 1; index <= NumberOfBusinessDays; index++)
        {
            switch (StartDate.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    StartDate = StartDate.AddDays(2);
                    break;
                case DayOfWeek.Monday:
                case DayOfWeek.Tuesday:
                case DayOfWeek.Wednesday:
                case DayOfWeek.Thursday:
                case DayOfWeek.Friday:
                    StartDate = StartDate.AddDays(1);
                    break;
                case DayOfWeek.Saturday:
                    StartDate = StartDate.AddDays(3);
                    break;
            }
        }

        //check to see if the end date is on a weekend.
        //If so move it ahead to Monday.
        //You could also bump it back to the Friday before if you desired to. 
        //Just change the code to -2 and -1.
        if (StartDate.DayOfWeek == DayOfWeek.Saturday)
        {
            StartDate = StartDate.AddDays(2);
        }
        else if (StartDate.DayOfWeek == DayOfWeek.Sunday)
        {
            StartDate = StartDate.AddDays(1);
        }

        return StartDate;
    }

    /// <summary>
    /// Calculates number of business days, taking into account:
    ///  - weekends (Saturdays and Sundays)
    ///  - bank holidays in the middle of the week
    /// </summary>
    /// <param name="firstDay">First day in the time interval</param>
    /// <param name="lastDay">Last day in the time interval</param>
    /// <param name="bankHolidays">List of bank holidays excluding weekends</param>
    /// <returns>Number of business days during the 'span'</returns>
    private int BusinessDaysUntil(DateTime firstDay, DateTime lastDay, params DateTime[] bankHolidays)
    {
        firstDay = firstDay.Date;
        lastDay = lastDay.Date;
        if (firstDay > lastDay)
            throw new ArgumentException("Incorrect last day " + lastDay);

        TimeSpan span = lastDay - firstDay;
        int businessDays = span.Days + 1;
        int fullWeekCount = businessDays / 7;
        // find out if there are weekends during the time exceedng the full weeks
        if (businessDays > fullWeekCount * 7)
        {
            // we are here to find out if there is a 1-day or 2-days weekend
            // in the time interval remaining after subtracting the complete weeks
            //int firstDayOfWeek = (int)firstDay.DayOfWeek;
            //int lastDayOfWeek = (int)lastDay.DayOfWeek;
            int firstDayOfWeek = firstDay.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)firstDay.DayOfWeek;
            int lastDayOfWeek = lastDay.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)lastDay.DayOfWeek;
            if (lastDayOfWeek < firstDayOfWeek)
                lastDayOfWeek += 7;
            if (firstDayOfWeek <= 6)
            {
                if (lastDayOfWeek >= 7)// Both Saturday and Sunday are in the remaining time interval
                    businessDays -= 2;
                else if (lastDayOfWeek >= 6)// Only Saturday is in the remaining time interval
                    businessDays -= 1;
            }
            else if (firstDayOfWeek <= 7 && lastDayOfWeek >= 7)// Only Sunday is in the remaining time interval
                businessDays -= 1;
        }

        // subtract the weekends during the full weeks in the interval
        businessDays -= fullWeekCount + fullWeekCount;

        // subtract the number of bank holidays during the time interval
        foreach (DateTime bankHoliday in bankHolidays)
        {
            DateTime bh = bankHoliday.Date;
            if (firstDay <= bh && bh <= lastDay)
                --businessDays;
        }

        return businessDays;
    }

    public double CalculateBusinessHours(DateTime dtStart, DateTime dtEnd)
    {
        int StartingHour = 8;
        int EndingHour = 17;
        int luncnStart = 12;
        int luncnEnd = 13;


        // initialze our return value
        double OverAllMinutes = 0.0;

        // start time must be less than end time
        if (dtStart > dtEnd)
        {
            return OverAllMinutes;
        }
        DateTime ctTempEnd = new DateTime(dtEnd.Year, dtEnd.Month, dtEnd.Day, 0, 0, 0);
        DateTime ctTempStart = new DateTime(dtStart.Year, dtStart.Month, dtStart.Day, 0, 0, 0);

        // check if startdate and enddate are the same day
        bool bSameDay = (ctTempStart == ctTempEnd);

        // calculate the business days between the dates
        int iBusinessDays = BusinessDaysUntil(ctTempStart, ctTempEnd);

        // now add the time values to our temp times
        TimeSpan CTimeSpan = new TimeSpan(0, dtStart.Hour, dtStart.Minute, 0);
        ctTempStart += CTimeSpan;
        CTimeSpan = new TimeSpan(0, dtEnd.Hour, dtEnd.Minute, 0);
        ctTempEnd += CTimeSpan;

        // set our workingday time range and correct the first day
        DateTime ctMaxTime = new DateTime(ctTempStart.Year, ctTempStart.Month, ctTempStart.Day, EndingHour, 0, 0);
        DateTime ctMinTime = new DateTime(ctTempStart.Year, ctTempStart.Month, ctTempStart.Day, StartingHour, 0, 0);
        Int32 FirstDaySec = CorrectFirstDayTime(ctTempStart, ctMaxTime, ctMinTime, luncnStart, luncnEnd);

        // set our workingday time range and correct the last day
        DateTime ctMaxTime1 = new DateTime(ctTempEnd.Year, ctTempEnd.Month, ctTempEnd.Day, EndingHour, 0, 0);
        DateTime ctMinTime1 = new DateTime(ctTempEnd.Year, ctTempEnd.Month, ctTempEnd.Day, StartingHour, 0, 0);
        Int32 LastDaySec = CorrectLastDayTime(ctTempEnd, ctMaxTime1, ctMinTime1, luncnStart, luncnEnd);
        Int32 OverAllSec = 0;

        // now sum-up all values
        if (bSameDay)
        {
            if (iBusinessDays != 0)
            {
                DateTime lunchTimeStart = new DateTime(dtStart.Year, dtStart.Month, dtStart.Day, luncnStart, 0, 0);
                DateTime lunchTimeEnd = new DateTime(dtStart.Year, dtStart.Month, dtStart.Day, luncnEnd, 0, 0);
                TimeSpan cts = (ctMaxTime - lunchTimeEnd) + (lunchTimeStart - ctMinTime);
                Int32 dwBusinessDaySeconds = (cts.Days * 24 * 60 * 60) + (cts.Hours * 60 * 60) + (cts.Minutes * 60) + cts.Seconds;
                OverAllSec = FirstDaySec + LastDaySec - dwBusinessDaySeconds;
            }
        }
        else
        {
            if (iBusinessDays > 1)
            {
                int iStartDay = (Int32)Enum.Parse(typeof(DayOfWeek), dtStart.DayOfWeek.ToString());
                if (iStartDay == 0 || iStartDay == 6)
                {
                    ++iBusinessDays;
                }

                int iEndDay = (Int32)Enum.Parse(typeof(DayOfWeek), dtEnd.DayOfWeek.ToString());
                if (iEndDay == 0 || iEndDay == 6)
                {
                    ++iBusinessDays;
                }

                OverAllSec = ((iBusinessDays - 2) * 8 * 60 * 60) + FirstDaySec + LastDaySec;
            }

        }
        OverAllMinutes = OverAllSec / 60;

        return OverAllMinutes / 60;


    }
    private Int32 CorrectFirstDayTime(DateTime ctStart, DateTime ctMaxTime, DateTime ctMinTime, int lunchStart, int lunchEnd)
    {
        Int32 daysec = 0;
        DateTime lunchTimeStart = new DateTime(ctStart.Year, ctStart.Month, ctStart.Day, lunchStart, 0, 0);
        DateTime lunchTimeEnd = new DateTime(ctStart.Year, ctStart.Month, ctStart.Day, lunchEnd, 0, 0);

        if (ctMaxTime < ctStart) // start time is after max time
        {
            return 0; // zero seconds for the first day
        }
        int iStartDay = (Int32)Enum.Parse(typeof(DayOfWeek), ctStart.DayOfWeek.ToString());
        if (iStartDay == 0 || iStartDay == 6)
        {
            return 0;
        }
        TimeSpan ctSpan = new TimeSpan();
        if (ctStart < ctMinTime) // start time is befor min time
        {
            ctSpan = (ctMaxTime - lunchTimeEnd) + (lunchTimeStart - ctMinTime);
        }
        else if (ctStart < lunchTimeStart)
        {
            ctSpan = (ctMaxTime - lunchTimeEnd) + (lunchTimeStart - ctStart);

        }
        else if (ctStart >= lunchTimeStart && ctStart <= lunchTimeEnd)
        {
            ctSpan = ctMaxTime - lunchTimeEnd;
        }
        else if (ctStart > lunchTimeEnd)
        {
            ctSpan = ctMaxTime - ctStart;
        }
        daysec = (ctSpan.Days * 24 * 60 * 60) + (ctSpan.Hours * 60 * 60) + (ctSpan.Minutes * 60) + ctSpan.Seconds;
        return daysec;
    }
    private Int32 CorrectLastDayTime(DateTime ctEnd, DateTime ctMaxTime, DateTime ctMinTime, int lunchStart, int lunchEnd)
    {
        Int32 daysec = 0;
        DateTime lunchTimeStart = new DateTime(ctEnd.Year, ctEnd.Month, ctEnd.Day, lunchStart, 0, 0);
        DateTime lunchTimeEnd = new DateTime(ctEnd.Year, ctEnd.Month, ctEnd.Day, lunchEnd, 0, 0);

        if (ctMinTime > ctEnd) // start time is after max time
        {
            return 0; // zero seconds for the last day
        }

        int iEndDay = (Int32)Enum.Parse(typeof(DayOfWeek), ctEnd.DayOfWeek.ToString());
        if (iEndDay == 0 || iEndDay == 6)
        {
            return 0;
        }
        //if (ctEnd > ctMaxTime) // start time is befor min time
        //{
        //    ctEnd = ctMaxTime; // set start time to min time
        //}
        //TimeSpan ctSpan = ctEnd - ctMinTime;
        //daysec = (ctSpan.Days * 24 * 60 * 60) + (ctSpan.Hours * 60 * 60) + (ctSpan.Minutes * 60) + ctSpan.Seconds;
        TimeSpan ctSpan = new TimeSpan();
        if (ctEnd <= lunchTimeStart) // start time is befor min time
        {
            ctSpan = ctEnd - ctMinTime;
        }
        else if (ctEnd > lunchTimeStart && ctEnd <= lunchTimeEnd)
        {
            ctSpan = lunchTimeStart - ctMinTime;
        }
        else if (ctEnd > lunchTimeEnd && ctEnd <= ctMaxTime)
        {
            ctSpan = (ctEnd - lunchTimeEnd) + (lunchTimeStart - ctMinTime);
        }
        else if (ctEnd > ctMaxTime)
        {
            ctSpan = (ctMaxTime - lunchTimeEnd) + (lunchTimeStart - ctMinTime);
        }
        daysec = (ctSpan.Days * 24 * 60 * 60) + (ctSpan.Hours * 60 * 60) + (ctSpan.Minutes * 60) + ctSpan.Seconds;
        return daysec;
    }
}
public class CustomGanttTaskFactory : ITaskFactory
{
    Task ITaskFactory.CreateTask()
    {
        return new CustomTask();
    }
}

public class CustomTask : Task
{
    public CustomTask()
        : base()
    {
    }

    public string Description
    {
        get { return (string)(ViewState["Description"] ?? ""); }
        set { ViewState["Description"] = value; }
    }

    public int? ProjectID
    {
        get { return (int)(ViewState["ProjectID"] ?? 0); }
        set { ViewState["ProjectID"] = value; }
    }

    public int? PlannerID
    {
        get { return (int)(ViewState["PlannerID"] ?? 0); }
        set { ViewState["PlannerID"] = value; }
    }

    public string CusTaskType
    {
        get { return (string)(ViewState["CusTaskType"] ?? ""); }
        set { ViewState["CusTaskType"] = value; }
    }

    public int? VendorID
    {
        get { return (int)(ViewState["VendorID"] ?? 0); }
        set { ViewState["VendorID"] = value; }
    }

    public string Vendor
    {
        get { return (string)(ViewState["Vendor"] ?? ""); }
        set { ViewState["Vendor"] = value; }
    }

    public int? RootVendorID
    {
        get { return (int)(ViewState["RootVendorID"] ?? 0); }
        set { ViewState["RootVendorID"] = value; }
    }

    public int? PlannerTaskID
    {
        get { return (int)(ViewState["PlannerTaskID"] ?? 0); }
        set { ViewState["PlannerTaskID"] = value; }
    }

    public int? ItemRefID
    {
        get { return (int)(ViewState["ItemRefID"] ?? 0); }
        set { ViewState["ItemRefID"] = value; }
    }

    public string RootVendorName
    {
        get { return (string)(ViewState["RootVendorName"] ?? ""); }
        set { ViewState["RootVendorName"] = value; }
    }

    public string ProjectName
    {
        get { return (string)(ViewState["ProjectName"] ?? ""); }
        set { ViewState["ProjectName"] = value; }
    }

    public double? CusDuration
    {
        get
        {
            if (ViewState["CusDuration"] != null)
            {
                return Convert.ToDouble(ViewState["CusDuration"].ToString());
            }
            return 0;
        }
        set { ViewState["CusDuration"] = value; }
    }

    public double? CusActualHour
    {
        get
        {
            if (ViewState["CusActualHour"] != null)
            {
                return Convert.ToDouble(ViewState["CusActualHour"].ToString());
            }
            return 0;
        }
        set { ViewState["CusActualHour"] = value; }
    }
    public string Dependency
    {
        get { return (string)(ViewState["Dependency"] ?? ""); }
        set { ViewState["Dependency"] = value; }
    }

    protected override IDictionary<string, object> GetSerializationData()
    {
        var dict = base.GetSerializationData();
        dict["Description"] = Description;
        dict["ProjectID"] = ProjectID;
        dict["PlannerID"] = PlannerID;
        dict["CusTaskType"] = CusTaskType;
        dict["CusDuration"] = CusDuration;
        dict["CusActualHour"] = CusActualHour;
        dict["VendorID"] = VendorID;
        dict["Vendor"] = Vendor;
        dict["RootVendorID"] = RootVendorID;
        dict["RootVendorName"] = RootVendorName;
        dict["ProjectName"] = ProjectName;
        dict["Dependency"] = Dependency;
        dict["PlannerTaskID"] = PlannerTaskID;
        dict["ItemRefID"] = ItemRefID;
        return dict;
    }

    public override void LoadFromDictionary(System.Collections.IDictionary values)
    {
        base.LoadFromDictionary(values);

        Description = (string)values["Description"];
        ProjectID = (int)values["ProjectID"];
        PlannerID = (int)values["PlannerID"];
        CusTaskType = (string)values["CusTaskType"];
        CusDuration = (double)values["CusDuration"];
        CusActualHour = (double)values["CusActualHour"];
        VendorID = (int)values["VendorID"];
        Vendor = (string)values["Vendor"];
        RootVendorID = (int)values["RootVendorID"];
        RootVendorName = (string)values["RootVendorName"];
        ProjectName = (string)values["ProjectName"];
        Dependency = (string)values["Dependency"];
        PlannerTaskID = (int)values["PlannerTaskID"];
        ItemRefID = (int)values["ItemRefID"];
    }
}
//}
