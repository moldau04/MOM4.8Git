using System;
using System.Linq;
using System.Web;
using System.Web.Services;
using BryntumGanttChart.Bryntum.BryntumCRUD.CRUD.Response;
using BryntumGanttChart.Bryntum.BryntumGantt.Gantt.Request;
using BryntumGanttChart.Bryntum.BryntumGantt.Gantt.Request.Handler;
using BryntumGanttChart.Bryntum.BryntumGantt.Gantt.Response;
using System.IO;
using System.Web.Script.Services;
using Newtonsoft.Json;
using System.Web.Script.Serialization;

/// <summary>
/// Summary description for BryntumAdvanceGanttChart
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class BryntumAdvanceGanttChart : System.Web.Services.WebService
{

    public const string dateFormat = "yyyy-MM-dd\\THH:mm:ss";

    /// <summary>
    /// Helper method to get POST request body.
    /// </summary>
    /// <returns>POST request body.</returns>
    private string getPostBody()
    {
        Stream req = this.Context.Request.InputStream;
        req.Seek(0, System.IO.SeekOrigin.Begin);
        return new StreamReader(req).ReadToEnd();
    }

    /// <summary>
    /// Load request handler.
    /// </summary>
    /// <returns>JSON encoded response.</returns>

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true, XmlSerializeString = false)]
    public void Load()
    {
        GanttLoadRequest loadRequest = null;
        ulong? requestId = null;

        try
        {
            string json = this.Context.Request.QueryString.Get("q");
            Int32 ProjectID = Convert.ToInt32(this.Context.Session["PlannerProjectID"]);

            var gantt = new BryntumGanttChart.Bryntum.BryntumGantt.GanttChart();
            

            // decode request object
            try
            {
                loadRequest = JsonConvert.DeserializeObject<GanttLoadRequest>(json);
            }
            catch (Exception)
            {
                throw new Exception("Invalid load JSON");
            }

            // get request identifier
            requestId = loadRequest.requestId;

            // initialize response object
            var loadResponse = new GanttLoadResponse(requestId);

            // if a corresponding store is requested then add it to the response object

            if (loadRequest.calendars != null) loadResponse.setCalendars(gantt.getCalendars(), 1);
            if (loadRequest.assignments != null) loadResponse.setAssignments(gantt.getAssignments());
            if (loadRequest.tasks != null) loadResponse.setTasks(gantt.getTasks(ProjectID));
            if (loadRequest.resources != null) loadResponse.setResources(gantt.getResources());
            if (loadRequest.dependencies != null) loadResponse.setDependencies(gantt.getDependencies());

            // put current server revision to the response
            loadResponse.revision = gantt.getRevision();

            // just in case we make any changes during load request processing
            gantt.context.SaveChanges();

            var dt = JsonConvert.SerializeObject(loadResponse);
            Context.Response.Clear();
            Context.Response.ContentType = "application/json";
            Context.Response.Write(dt);

        }
        catch (Exception e)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            Context.Response.ContentType = "application/json";
            Context.Response.Write(js.Serialize(new ErrorResponse(e, requestId)));
        }
    }

    protected SyncStoreResponse AddModifiedRows(BryntumGanttChart.Bryntum.BryntumGantt.GanttChart gantt, string table, SyncStoreResponse resp)
    {
        if (gantt.HasUpdatedRows(table))
        {
            if (resp == null) resp = new SyncStoreResponse();
            var rows = gantt.GetUpdatedRows(table);
            resp.rows = resp.rows != null ? resp.rows.Concat(rows).ToList() : rows;
        }
        if (gantt.HasRemovedRows(table))
        {
            if (resp == null) resp = new SyncStoreResponse();
            var removed = gantt.GetRemovedRows(table);
            resp.removed = resp.removed != null ? resp.removed.Concat(removed).ToList() : removed;
        }
        return resp;
    }

    /// <summary>
    /// Sync response handler.
    /// </summary>
    /// <returns>JSON encoded response.</returns>
    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json, XmlSerializeString = false)]
    public void Sync()
    {
        ulong? requestId = null;
        GanttSyncRequest syncRequest = null;

        try
        {
            string json = getPostBody();
         
            var gantt = new BryntumGanttChart.Bryntum.BryntumGantt.GanttChart();

            // decode request object
            try
            {
                syncRequest = JsonConvert.DeserializeObject<GanttSyncRequest>(json, new Newtonsoft.Json.Converters.IsoDateTimeConverter { DateTimeFormat = dateFormat });
            }
            catch (Exception)
            {
                throw new Exception("Invalid sync JSON");
            }

            // initialize phantom to real Id maps
            gantt.InitRowsHolders();

            // get request identifier
            requestId = syncRequest.requestId;

            // initialize response object
            var syncResponse = new GanttSyncResponse(requestId);

            // Here we reject client's changes if we suspect that they are out-dated
            // considering difference between server and client revisions.
            // You can get rid of this call if you don't need such behavior.

            //gantt.checkRevision(syncRequest.revision);

            // if a corresponding store modified data are provided then we handle them

            // first let's process added and updated records 

            CalendarSyncHandler calendarHandler = null;
            if (syncRequest.calendars != null)
            {
                calendarHandler = new CalendarSyncHandler(gantt, dateFormat);
                syncResponse.calendars = calendarHandler.Handle(syncRequest.calendars, CalendarSyncHandler.Rows.AddedAndUpdated);
            }
            ResourceSyncHandler resourcesHandler = null;
            if (syncRequest.resources != null)
            {
                resourcesHandler = new ResourceSyncHandler(gantt);
                syncResponse.resources = resourcesHandler.Handle(syncRequest.resources, ResourceSyncHandler.Rows.AddedAndUpdated);
            }
            TaskSyncHandler taskHandler = null;
            if (syncRequest.tasks != null)
            {
                taskHandler = new TaskSyncHandler(gantt, dateFormat);
                syncResponse.tasks = taskHandler.Handle(syncRequest.tasks, TaskSyncHandler.Rows.AddedAndUpdated);
            }
            AssignmentSyncHandler assignmentHandler = null;
            if (syncRequest.assignments != null)
            {
                assignmentHandler = new AssignmentSyncHandler(gantt);
                syncResponse.assignments = assignmentHandler.Handle(syncRequest.assignments, AssignmentSyncHandler.Rows.AddedAndUpdated);
            }
            DependencySyncHandler dependencyHandler = null;
            if (syncRequest.dependencies != null)
            {
                dependencyHandler = new DependencySyncHandler(gantt);
                syncResponse.dependencies = dependencyHandler.Handle(syncRequest.dependencies, DependencySyncHandler.Rows.AddedAndUpdated);
            }

            // then let's process records removals

            if (syncRequest.dependencies != null)
                syncResponse.dependencies = dependencyHandler.HandleRemoved(syncRequest.dependencies, syncResponse.dependencies);

            if (syncRequest.assignments != null)
                syncResponse.assignments = assignmentHandler.HandleRemoved(syncRequest.assignments, syncResponse.assignments);

            if (syncRequest.tasks != null)
                syncResponse.tasks = taskHandler.HandleRemoved(syncRequest.tasks, syncResponse.tasks);

            if (syncRequest.resources != null)
                syncResponse.resources = resourcesHandler.HandleRemoved(syncRequest.resources, syncResponse.resources);

            if (syncRequest.calendars != null)
                syncResponse.calendars = calendarHandler.HandleRemoved(syncRequest.calendars, syncResponse.calendars);

            // we also return implicit modifications made by server
            // (implicit records updates/removals caused by data references)

            syncResponse.calendars = AddModifiedRows(gantt, "calendars", syncResponse.calendars);
            syncResponse.tasks = AddModifiedRows(gantt, "tasks", syncResponse.tasks);
            syncResponse.resources = AddModifiedRows(gantt, "resources", syncResponse.resources);
            syncResponse.assignments = AddModifiedRows(gantt, "assignments", syncResponse.assignments);
            syncResponse.dependencies = AddModifiedRows(gantt, "dependencies", syncResponse.dependencies);

            // put current server revision to the response
            syncResponse.revision = gantt.getRevision();

            gantt.context.SaveChanges();

            var dt = JsonConvert.SerializeObject(syncResponse);

            Context.Response.Clear();
            Context.Response.ContentType = "application/json";
            Context.Response.AddHeader("content-length", dt.Length.ToString());
            Context.Response.Flush();
            Context.Response.Write(dt);
            HttpContext.Current.ApplicationInstance.CompleteRequest();

        }
        catch (Exception e)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            Context.Response.ContentType = "application/json";
            Context.Response.AddHeader("content-length", js.Serialize(new ErrorResponse(e, requestId).ToString().Length.ToString()));
            Context.Response.Flush();
            Context.Response.Write(js.Serialize(new ErrorResponse(e, requestId)));
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }
    }


    /// <summary>
    /// Back-end test handler providing database cleanup.
    /// TODO: WARNING! This code clears the database. Please get rid of this code before running it on production.
    /// </summary>
    /// <returns>Empty string.</returns>
    public string Reset()
    {
        var gantt = new BryntumGanttChart.Bryntum.BryntumGantt.GanttChart();

        gantt.Reset();
        gantt.context.SaveChanges();

        return "";
    }

}
