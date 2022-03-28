<%@ WebService Language="C#" Class="GanttService" %>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.SessionState;
//using Telerik.Web.UI;
using Telerik.Web.UI.Gantt;

[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class GanttService : System.Web.Services.WebService, IRequiresSessionState
{
    private WebServiceController _controller;

    public WebServiceController Controller
    {
        get
        {
            if (_controller == null)
            {
                _controller = new WebServiceController(new GanttProvider());
            }

            return _controller;
        }
    }
    [WebMethod(EnableSession = true)]
    public IEnumerable<CustomTaskData> GetTasks()
    {
        var ddd = Controller.GetTasks<CustomTaskData>();
        var provider = new GanttProvider();
        foreach (var item in ddd)
        {
            //item.Dependency = provider.GetSuccessorByTaskID((int)item.ID);
            item.Dependency = provider.GetPredecessorByTaskID((int)item.ID);
        }

        return ddd;
    }
    [WebMethod(EnableSession = true)]
    public IEnumerable<CustomTaskData> InsertTasks(IEnumerable<CustomTaskData> models)
    {
        var plannerRefId = HttpContext.Current.Session["GanttPlannerRefId"] != null ? Convert.ToInt32(HttpContext.Current.Session["GanttPlannerRefId"]) : 0;
        var plannerId = HttpContext.Current.Session["GanttPlannerID"] != null ? Convert.ToInt32(HttpContext.Current.Session["GanttPlannerID"]) : 0;
        var plannerType = HttpContext.Current.Session["GanttPlannerType"] != null ? HttpContext.Current.Session["GanttPlannerType"].ToString() : "project";
        if (plannerType.Equals("vendor", StringComparison.InvariantCultureIgnoreCase))
        {
            foreach (var item in models)
            {
                item.RootVendorID = plannerRefId;
                item.PlannerID = plannerId;
            }
        }else
        {
            foreach (var item in models)
            {
                item.ProjectID = plannerRefId;
                item.PlannerID = plannerId;
            }
        }
        return Controller.InsertTasks<CustomTaskData>(models);
    }
    [WebMethod(EnableSession = true)]
    public IEnumerable<CustomTaskData> UpdateTasks(IEnumerable<CustomTaskData> models)
    {
        var plannerRefId = HttpContext.Current.Session["GanttPlannerRefId"] != null ? Convert.ToInt32(HttpContext.Current.Session["GanttPlannerRefId"]) : 0;
        var plannerId = HttpContext.Current.Session["GanttPlannerID"] != null ? Convert.ToInt32(HttpContext.Current.Session["GanttPlannerID"]) : 0;
        var plannerType = HttpContext.Current.Session["GanttPlannerType"] != null ? HttpContext.Current.Session["GanttPlannerType"].ToString() : "project";
        if (plannerType.Equals("vendor", StringComparison.InvariantCultureIgnoreCase))
        {
            foreach (var item in models)
            {
                item.RootVendorID = plannerRefId;
                item.PlannerID = plannerId;
            }
        }else
        {
            foreach (var item in models)
            {
                item.ProjectID = plannerRefId;
                item.PlannerID = plannerId;
            }
        }
        return Controller.UpdateTasks<CustomTaskData>(models);
    }
    [WebMethod(EnableSession = true)]
    public IEnumerable<CustomTaskData> DeleteTasks(IEnumerable<CustomTaskData> models)
    {
        return Controller.DeleteTasks<CustomTaskData>(models);
    }
    [WebMethod(EnableSession = true)]
    public IEnumerable<DependencyData> GetDependencies()
    {
        return Controller.GetDependencies();
    }
    [WebMethod(EnableSession = true)]
    public IEnumerable<DependencyData> InsertDependencies(IEnumerable<DependencyData> models)
    {
        return Controller.InsertDependencies(models);
    }
    [WebMethod(EnableSession = true)]
    public IEnumerable<DependencyData> DeleteDependencies(IEnumerable<DependencyData> models)
    {
        return Controller.DeleteDependencies(models);
    }

    [WebMethod(EnableSession = true)]
    public IEnumerable<Telerik.Web.UI.Gantt.ResourceData> GetResources()
    {
        return Controller.GetResources();
    }

    [WebMethod(EnableSession = true)]
    public IEnumerable<AssignmentData> GetAssignments()
    {
        return Controller.GetAssignments();
    }
    [WebMethod(EnableSession = true)]
    public IEnumerable<AssignmentData> InsertAssignments(IEnumerable<AssignmentData> models)
    {
        return Controller.InsertAssignments(models);
    }
    [WebMethod(EnableSession = true)]
    public IEnumerable<AssignmentData> UpdateAssignments(IEnumerable<AssignmentData> models)
    {
        return Controller.UpdateAssignments(models);
    }
    [WebMethod(EnableSession = true)]
    public IEnumerable<AssignmentData> DeleteAssignments(IEnumerable<AssignmentData> models)
    {
        return Controller.DeleteAssignments(models);
    }
}


[System.Runtime.Serialization.DataContract]
public class CustomTaskData : TaskData
{
    [System.Runtime.Serialization.DataMember]
    public string Description
    {
        get;
        set;
    }

    [System.Runtime.Serialization.DataMember]
    public int? ProjectID
    {
        get;
        set;
    }
    [System.Runtime.Serialization.DataMember]
    public int? PlannerID
    {
        get;
        set;
    }

    [System.Runtime.Serialization.DataMember]
    public string CusTaskType
    {
        get;
        set;
    }

    [System.Runtime.Serialization.DataMember]
    public double? CusDuration
    {
        get;
        set;
    }
    [System.Runtime.Serialization.DataMember]
    public double? CusActualHour
    {
        get;
        set;
    }
    [System.Runtime.Serialization.DataMember]
    public int? VendorID
    {
        get;
        set;
    }

    [System.Runtime.Serialization.DataMember]
    public string Vendor
    {
        get;
        set;
    }
    [System.Runtime.Serialization.DataMember]
    public int? RootVendorID
    {
        get;
        set;
    }

    [System.Runtime.Serialization.DataMember]
    public string RootVendorName
    {
        get;
        set;
    }
    [System.Runtime.Serialization.DataMember]
    public string ProjectName
    {
        get;
        set;
    }
    [System.Runtime.Serialization.DataMember]
    public string Dependency
    {
        get;
        set;
    }

    [System.Runtime.Serialization.DataMember]
    public int? PlannerTaskID
    {
        get;
        set;
    }
    [System.Runtime.Serialization.DataMember]
    public int? ItemRefID
    {
        get;
        set;
    }
    public override void CopyFrom(ITask srcTask)
    {
        base.CopyFrom(srcTask);

        Description = ((CustomTask)srcTask).Description;
        ProjectID = ((CustomTask)srcTask).ProjectID;
        PlannerID = ((CustomTask)srcTask).PlannerID;
        CusTaskType = ((CustomTask)srcTask).CusTaskType;
        CusDuration = ((CustomTask)srcTask).CusDuration;
        CusActualHour = ((CustomTask)srcTask).CusActualHour;
        VendorID = ((CustomTask)srcTask).VendorID;
        Vendor = ((CustomTask)srcTask).Vendor;
        RootVendorID = ((CustomTask)srcTask).RootVendorID;
        RootVendorName = ((CustomTask)srcTask).RootVendorName;
        ProjectName = ((CustomTask)srcTask).ProjectName;
        PlannerTaskID = ((CustomTask)srcTask).PlannerTaskID;
        ItemRefID = ((CustomTask)srcTask).ItemRefID;
        Dependency = ((CustomTask)srcTask).Dependency;
    }

    public override void CopyTo(ITask destTask)
    {
        base.CopyTo(destTask);

        ((CustomTask)destTask).Description = Description;
        ((CustomTask)destTask).ProjectID = ProjectID;
        ((CustomTask)destTask).PlannerID = PlannerID;
        ((CustomTask)destTask).CusTaskType = CusTaskType;
        ((CustomTask)destTask).CusDuration = CusDuration;
        ((CustomTask)destTask).CusActualHour = CusActualHour;
        ((CustomTask)destTask).VendorID = VendorID;
        ((CustomTask)destTask).Vendor = Vendor;
        ((CustomTask)destTask).RootVendorID = RootVendorID;
        ((CustomTask)destTask).RootVendorName = RootVendorName;
        ((CustomTask)destTask).ProjectName = ProjectName;
        ((CustomTask)destTask).PlannerTaskID = PlannerTaskID;
        ((CustomTask)destTask).ItemRefID = ItemRefID;
        ((CustomTask)destTask).Dependency = Dependency;
    }
}