using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bryntum.CRUD.Response;
using Bryntum.Gantt;
using Bryntum.Gantt.Request;
using Bryntum.Gantt.Request.Handler;
using Bryntum.Gantt.Response;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using System.Text;

namespace MOMWebApp.Controllers
{
    public class GanttCrudController : Controller
    {
        public const string dateFormat = "yyyy-MM-dd\\THH:mm:ss";

        /// <summary>
        /// Helper method to get POST request body.
        /// </summary>
        /// <returns>POST request body.</returns>
        private string getPostBody()
        {
            Stream req = Request.InputStream;
            req.Seek(0, System.IO.SeekOrigin.Begin);
            return new StreamReader(req).ReadToEnd();
        }

        /// <summary>
        /// Load request handler.
        /// </summary>
        /// <returns>JSON encoded response.</returns>
        [System.Web.Mvc.HttpGet]
        public ActionResult Load()
        {
            GanttLoadRequest loadRequest = null;
            ulong? requestId = null;

            try
            {
                string json = Request.QueryString.Get("q");
                Int32 ProjectID = Convert.ToInt32(Session["PlannerProjectID"]);
                //if(ProjectID == 0) ProjectID = 1111;
                var gantt = new Gantt();

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
                //var dt = gantt.getTasks().ToList();

                if (loadRequest.calendars != null) loadResponse.setCalendars(gantt.getCalendars(), 1);
                if (loadRequest.assignments != null) loadResponse.setAssignments(gantt.getAssignments());
                if (loadRequest.tasks != null) loadResponse.setTasks(gantt.getTasks(ProjectID));
                if (loadRequest.resources != null) loadResponse.setResources(gantt.getResources());
                if (loadRequest.dependencies != null) loadResponse.setDependencies(gantt.getDependencies());

                // put current server revision to the response
                loadResponse.revision = gantt.getRevision();

                // just in case we make any changes during load request processing
                gantt.context.SaveChanges();

                return Content(JsonConvert.SerializeObject(loadResponse), "application/json");
            }
            catch (Exception e)
            {
                return Content(JsonConvert.SerializeObject(new ErrorResponse(e, requestId)), "application/json");
            }
        }

        protected SyncStoreResponse AddModifiedRows(Gantt gantt, string table, SyncStoreResponse response)
        {
            var resp = response ?? new SyncStoreResponse();

            if (gantt.HasUpdatedRows(table))
            {
                var rows = gantt.GetUpdatedRows(table);
                resp.rows = resp.rows != null ? resp.rows.Concat(rows).ToList() : rows;
            }

            if (gantt.HasRemovedRows(table))
            {
                resp.removed = gantt.GetRemovedRows(table);
            }

            return resp;
        }

        /// <summary>
        /// Sync response handler.
        /// </summary>
        /// <returns>JSON encoded response.</returns>
        [System.Web.Mvc.HttpPost]
        public ActionResult Sync()
        {
            ulong? requestId = null;
            GanttSyncRequest syncRequest = null;

            try
            {
                string json = getPostBody();
                Int32 ProjectID = Convert.ToInt32(Session["PlannerProjectID"]);
                var gantt = new Gantt();

                // decode request object
                try
                {
                    syncRequest = JsonConvert.DeserializeObject<GanttSyncRequest>(json, new Newtonsoft.Json.Converters.IsoDateTimeConverter { DateTimeFormat = dateFormat });
                    if(syncRequest.tasks.added != null && syncRequest.tasks.added.Count > 0)
                    {
                        foreach (var item in syncRequest.tasks.added)
                        {
                            item.ProjectID = ProjectID;
                        }
                    }

                    //if (syncRequest.tasks.updated.Count > 0)
                    //{
                    //    foreach (var item in syncRequest.tasks.updated)
                    //    {
                    //        item.ProjectID = ProjectID;
                    //    }
                    //}
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
                gantt.checkRevision(syncRequest.revision);

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

                // remove records in bulk
                gantt.doRemove();

                // we also return implicit modifications made by server
                // (implicit records updates/removals caused by data references)
                gantt.updateRevision();
                gantt.context.SaveChanges();

                syncResponse.calendars = AddModifiedRows(gantt, "calendars", syncResponse.calendars);
                syncResponse.tasks = AddModifiedRows(gantt, "tasks", syncResponse.tasks);
                syncResponse.resources = AddModifiedRows(gantt, "resources", syncResponse.resources);
                syncResponse.assignments = AddModifiedRows(gantt, "assignments", syncResponse.assignments);
                syncResponse.dependencies = AddModifiedRows(gantt, "dependencies", syncResponse.dependencies);

                // put current server revision to the response
                syncResponse.revision = gantt.getRevision();

                return Content(JsonConvert.SerializeObject(syncResponse), "application/json");
            }
            catch (Exception e)
            {
                return Content(JsonConvert.SerializeObject(new ErrorResponse(e, requestId)), "application/json");
            }
        }

        /// <summary>
        /// Back-end test handler providing database cleanup.
        /// TODO: WARNING! This code clears the database. Please get rid of this code before running it on production.
        /// </summary>
        /// <returns>Empty string.</returns>
        //public string Reset()
        //{
        //    var gantt = new Gantt();

        //    gantt.Reset();
        //    gantt.context.SaveChanges();

        //    return "";
        //}

        [System.Web.Mvc.HttpGet]
        public ActionResult GetGanttChartDataStaticJSON()
        {
            string str = "{                                                                                           " +
"    \"success\" : true,                                                                     " +
"                                                                                            " +
"                                                                                            " +
"    \"tasks\" : {                                                                           " +
"        \"rows\" : [                                                                        " +
"            {                                                                               " +
"                \"id\": 1000,                                                               " +
"                \"name\": \"Launch SaaS Product\",                                          " +
"                \"percentDone\": 50,                                                        " +
"                \"startDate\": \"2019-01-14\",                                              " +
"                \"expanded\": true,                                                         " +
"                \"children\": [                                                             " +
"                    {                                                                       " +
"                        \"id\": 1,                                                          " +
"                        \"name\": \"Setup web server\",                                     " +
"                        \"percentDone\": 50,                                                " +
"                        \"duration\": 10,                                                   " +
"                        \"startDate\": \"2019-01-14\",                                      " +
"                        \"expanded\": true,                                                 " +
"                        \"children\": [                                                     " +
"                            {                                                               " +
"                                \"id\": 11,                                                 " +
"                                \"name\": \"Install Apache\",                               " +
"                                \"percentDone\": 50,                                        " +
"                                \"startDate\": \"2019-01-14\",                              " +
"                                \"rollup\": true,                                           " +
"                                \"duration\": 3,                                            " +
"                                \"color\": \"teal\",                                        " +
"                                \"endDate\": \"2019-01-17\",                                " +
"                                \"cost\": 200,                                              " +
"                                \"baselines\": [                                            " +
"                                    {                                                       " +
"                                        \"startDate\": \"2019-01-13T23:00:00\",             " +
"                                        \"endDate\": \"2019-01-16T23:00:00\"                " +
"                                    },                                                      " +
"                                    {                                                       " +
"                                        \"startDate\": \"2019-01-13T23:00:00\",             " +
"                                        \"endDate\": \"2019-01-16T23:00:00\"                " +
"                                    },                                                      " +
"                                    {                                                       " +
"                                        \"startDate\": \"2019-01-13T23:00:00\",             " +
"                                        \"endDate\": \"2019-01-16T23:00:00\"                " +
"                                    }                                                       " +
"                                ]                                                           " +
"                            },                                                              " +
"                            {                                                               " +
"                                \"id\": 12,                                                 " +
"                                \"name\": \"Configure firewall\",                           " +
"                                \"percentDone\": 50,                                        " +
"                                \"startDate\": \"2019-01-14\",                              " +
"                                \"duration\": 3,                                            " +
"                                \"endDate\": \"2019-01-17\",                                " +
"                                \"showInTimeline\": true,                                   " +
"                                \"cost\": 1000,                                             " +
"                                \"baselines\": [                                            " +
"                                    {                                                       " +
"                                        \"startDate\": \"2019-01-13T23:00:00\",             " +
"                                        \"endDate\": \"2019-01-16T23:00:00\"                " +
"                                    },                                                      " +
"                                    {                                                       " +
"                                        \"startDate\": \"2019-01-13T23:00:00\",             " +
"                                        \"endDate\": \"2019-01-16T23:00:00\"                " +
"                                    },                                                      " +
"                                    {                                                       " +
"                                        \"startDate\": \"2019-01-13T23:00:00\",             " +
"                                        \"endDate\": \"2019-01-16T23:00:00\"                " +
"                                    }                                                       " +
"                                ]                                                           " +
"                            },                                                              " +
"                            {                                                               " +
"                                \"id\": 13,                                                 " +
"                                \"name\": \"Setup load balancer\",                          " +
"                                \"percentDone\": 50,                                        " +
"                                \"startDate\": \"2019-01-14\",                              " +
"                                \"rollup\": true,                                           " +
"                                \"duration\": 3,                                            " +
"                                \"endDate\": \"2019-01-17\",                                " +
"                                \"cost\": 1200,                                             " +
"                                \"baselines\": [                                            " +
"                                    {                                                       " +
"                                        \"startDate\": \"2019-01-13T23:00:00\",             " +
"                                        \"endDate\": \"2019-01-16T23:00:00\"                " +
"                                    },                                                      " +
"                                    {                                                       " +
"                                        \"startDate\": \"2019-01-13T23:00:00\",             " +
"                                        \"endDate\": \"2019-01-16T23:00:00\"                " +
"                                    },                                                      " +
"                                    {                                                       " +
"                                        \"startDate\": \"2019-01-13T23:00:00\",             " +
"                                        \"endDate\": \"2019-01-16T23:00:00\"                " +
"                                    }                                                       " +
"                                ]                                                           " +
"                            },                                                              " +
"                            {                                                               " +
"                                \"id\": 14,                                                 " +
"                                \"name\": \"Configure ports\",                              " +
"                                \"percentDone\": 50,                                        " +
"                                \"startDate\": \"2019-01-14\",                              " +
"                                \"duration\": 2,                                            " +
"                                \"endDate\": \"2019-01-16\",                                " +
"                                \"cost\": 750,                                              " +
"                                \"baselines\": [                                            " +
"                                    {                                                       " +
"                                        \"startDate\": \"2019-01-13T23:00:00\",             " +
"                                        \"endDate\": \"2019-01-15T23:00:00\"                " +
"                                    },                                                      " +
"                                    {                                                       " +
"                                        \"startDate\": \"2019-01-13T23:00:00\",             " +
"                                        \"endDate\": \"2019-01-15T23:00:00\"                " +
"                                    },                                                      " +
"                                    {                                                       " +
"                                        \"startDate\": \"2019-01-13T23:00:00\",             " +
"                                        \"endDate\": \"2019-01-15T23:00:00\"                " +
"                                    }                                                       " +
"                                ]                                                           " +
"                            },                                                              " +
"                            {                                                               " +
"                                \"id\": 15,                                                 " +
"                                \"name\": \"Run tests\",                                    " +
"                                \"percentDone\": 0,                                         " +
"                                \"startDate\": \"2019-01-21\",                              " +
"                                \"duration\": 2,                                            " +
"                                \"endDate\": \"2019-01-23\",                                " +
"                                \"cost\": 5000,                                             " +
"                                \"baselines\": [                                            " +
"                                    {                                                       " +
"                                        \"startDate\": \"2019-01-20T23:00:00\",             " +
"                                        \"endDate\": \"2019-01-22T23:00:00\"                " +
"                                    },                                                      " +
"                                    {                                                       " +
"                                        \"startDate\": \"2019-01-20T23:00:00\",             " +
"                                        \"endDate\": \"2019-01-22T23:00:00\"                " +
"                                    },                                                      " +
"                                    {                                                       " +
"                                        \"startDate\": \"2019-01-20T23:00:00\",             " +
"                                        \"endDate\": \"2019-01-22T23:00:00\"                " +
"                                    }                                                       " +
"                                ]                                                           " +
"                            }                                                               " +
"                        ],                                                                  " +
"                        \"endDate\": \"2019-01-23\",                                        " +
"                        \"baselines\": [                                                    " +
"                            {                                                               " +
"                                \"startDate\": \"2019-01-13T23:00:00\",                     " +
"                                \"endDate\": \"2019-01-22T23:00:00\"                        " +
"                            },                                                              " +
"                            {                                                               " +
"                                \"startDate\": \"2019-01-13T23:00:00\",                     " +
"                                \"endDate\": \"2019-01-22T23:00:00\"                        " +
"                            },                                                              " +
"                            {                                                               " +
"                                \"startDate\": \"2019-01-13T23:00:00\",                     " +
"                                \"endDate\": \"2019-01-22T23:00:00\"                        " +
"                            }                                                               " +
"                        ]                                                                   " +
"                    },                                                                      " +
"                    {                                                                       " +
"                        \"id\": 2,                                                          " +
"                        \"name\": \"Website Design\",                                       " +
"                        \"percentDone\": 60,                                                " +
"                        \"startDate\": \"2019-01-23\",                                      " +
"                        \"expanded\": true,                                                 " +
"                        \"children\": [                                                     " +
"                            {                                                               " +
"                                \"id\": 21,                                                 " +
"                                \"name\": \"Contact designers\",                            " +
"                                \"percentDone\": 70,                                        " +
"                                \"startDate\": \"2019-01-23\",                              " +
"                                \"duration\": 5,                                            " +
"                                \"endDate\": \"2019-01-30\",                                " +
"                                \"cost\": 500,                                              " +
"                                \"baselines\": [                                            " +
"                                    {                                                       " +
"                                        \"startDate\": \"2019-01-22T23:00:00\",             " +
"                                        \"endDate\": \"2019-01-25T23:00:00\"                " +
"                                    },                                                      " +
"                                    {                                                       " +
"                                        \"startDate\": \"2019-01-22T23:00:00\",             " +
"                                        \"endDate\": \"2019-01-28T23:00:00\"                " +
"                                    },                                                      " +
"                                    {                                                       " +
"                                        \"startDate\": \"2019-01-22T23:00:00\",             " +
"                                        \"endDate\": \"2019-01-29T23:00:00\"                " +
"                                    }                                                       " +
"                                ]                                                           " +
"                            },                                                              " +
"                            {                                                               " +
"                                \"id\": 22,                                                 " +
"                                \"name\": \"Create shortlist of three designers\",          " +
"                                \"percentDone\": 60,                                        " +
"                                \"startDate\": \"2019-01-30\",                              " +
"                                \"duration\": 1,                                            " +
"                                \"endDate\": \"2019-01-31\",                                " +
"                                \"cost\": 1000,                                             " +
"                                \"baselines\": [                                            " +
"                                    {                                                       " +
"                                        \"startDate\": \"2019-01-27T23:00:00\",             " +
"                                        \"endDate\": \"2019-01-28T23:00:00\"                " +
"                                    },                                                      " +
"                                    {                                                       " +
"                                        \"startDate\": \"2019-01-28T23:00:00\",             " +
"                                        \"endDate\": \"2019-01-29T23:00:00\"                " +
"                                    },                                                      " +
"                                    {                                                       " +
"                                        \"startDate\": \"2019-01-29T23:00:00\",             " +
"                                        \"endDate\": \"2019-01-30T23:00:00\"                " +
"                                    }                                                       " +
"                                ]                                                           " +
"                            },                                                              " +
"                            {                                                               " +
"                                \"id\": 23,                                                 " +
"                                \"name\": \"Select & review final design\",                 " +
"                                \"percentDone\": 50,                                        " +
"                                \"startDate\": \"2019-01-31\",                              " +
"                                \"duration\": 2,                                            " +
"                                \"showInTimeline\": true,                                   " +
"                                \"endDate\": \"2019-02-02\",                                " +
"                                \"cost\": 1000,                                             " +
"                                \"baselines\": [                                            " +
"                                    {                                                       " +
"                                        \"startDate\": \"2019-01-28T23:00:00\",             " +
"                                        \"endDate\": \"2019-01-30T23:00:00\"                " +
"                                    },                                                      " +
"                                    {                                                       " +
"                                        \"startDate\": \"2019-01-29T23:00:00\",             " +
"                                        \"endDate\": \"2019-01-31T23:00:00\"                " +
"                                    },                                                      " +
"                                    {                                                       " +
"                                        \"startDate\": \"2019-01-30T23:00:00\",             " +
"                                        \"endDate\": \"2019-02-01T23:00:00\"                " +
"                                    }                                                       " +
"                                ]                                                           " +
"                            },                                                              " +
"                            {                                                               " +
"                                \"id\": 24,                                                 " +
"                                \"name\": \"Inform management about decision\",             " +
"                                \"percentDone\": 100,                                       " +
"                                \"startDate\": \"2019-02-04\",                              " +
"                                \"duration\": 0,                                            " +
"                                \"cost\": 500,                                              " +
"                                \"baselines\": [                                            " +
"                                    {                                                       " +
"                                        \"startDate\": \"2019-01-30T23:00:00\",             " +
"                                        \"endDate\": \"2019-01-30T23:00:00\"                " +
"                                    },                                                      " +
"                                    {                                                       " +
"                                        \"startDate\": \"2019-01-31T23:00:00\",             " +
"                                        \"endDate\": \"2019-01-31T23:00:00\"                " +
"                                    },                                                      " +
"                                    {                                                       " +
"                                        \"startDate\": \"2019-02-01T23:00:00\",             " +
"                                        \"endDate\": \"2019-02-01T23:00:00\"                " +
"                                    }                                                       " +
"                                ]                                                           " +
"                            },                                                              " +
"                            {                                                               " +
"                                \"id\": 25,                                                 " +
"                                \"name\": \"Apply design to web site\",                     " +
"                                \"percentDone\": 0,                                         " +
"                                \"startDate\": \"2019-02-04\",                              " +
"                                \"duration\": 7,                                            " +
"                                \"endDate\": \"2019-02-13\",                                " +
"                                \"cost\": 11000,                                            " +
"                                \"baselines\": [                                            " +
"                                    {                                                       " +
"                                        \"startDate\": \"2019-01-30T23:00:00\",             " +
"                                        \"endDate\": \"2019-02-08T23:00:00\"                " +
"                                    },                                                      " +
"                                    {                                                       " +
"                                        \"startDate\": \"2019-01-31T23:00:00\",             " +
"                                        \"endDate\": \"2019-02-11T23:00:00\"                " +
"                                    },                                                      " +
"                                    {                                                       " +
"                                        \"startDate\": \"2019-02-03T23:00:00\",             " +
"                                        \"endDate\": \"2019-02-12T23:00:00\"                " +
"                                    }                                                       " +
"                                ]                                                           " +
"                            }                                                               " +
"                        ],                                                                  " +
"                        \"endDate\": \"2019-02-13\",                                        " +
"                        \"baselines\": [                                                    " +
"                            {                                                               " +
"                                \"startDate\": \"2019-01-22T23:00:00\",                     " +
"                                \"endDate\": \"2019-02-08T23:00:00\"                        " +
"                            },                                                              " +
"                            {                                                               " +
"                                \"startDate\": \"2019-01-22T23:00:00\",                     " +
"                                \"endDate\": \"2019-02-11T23:00:00\"                        " +
"                            },                                                              " +
"                            {                                                               " +
"                                \"startDate\": \"2019-01-22T23:00:00\",                     " +
"                                \"endDate\": \"2019-02-12T23:00:00\"                        " +
"                            }                                                               " +
"                        ]                                                                   " +
"                    },                                                                      " +
"                    {                                                                       " +
"                        \"id\": 3,                                                          " +
"                        \"name\": \"Setup Test Strategy\",                                  " +
"                        \"percentDone\": 20,                                                " +
"                        \"startDate\": \"2019-01-14\",                                      " +
"                        \"expanded\": true,                                                 " +
"                        \"children\": [                                                     " +
"                            {                                                               " +
"                                \"id\": 31,                                                 " +
"                                \"name\": \"Hire QA staff\",                                " +
"                                \"percentDone\": 40,                                        " +
"                                \"startDate\": \"2019-01-14\",                              " +
"                                \"duration\": 5,                                            " +
"                                \"endDate\": \"2019-01-19\",                                " +
"                                \"cost\": 6000,                                             " +
"                                \"baselines\": [                                            " +
"                                    {                                                       " +
"                                        \"startDate\": \"2019-01-13T23:00:00\",             " +
"                                        \"endDate\": \"2019-01-18T23:00:00\"                " +
"                                    },                                                      " +
"                                    {                                                       " +
"                                        \"startDate\": \"2019-01-13T23:00:00\",             " +
"                                        \"endDate\": \"2019-01-18T23:00:00\"                " +
"                                    },                                                      " +
"                                    {                                                       " +
"                                        \"startDate\": \"2019-01-13T23:00:00\",             " +
"                                        \"endDate\": \"2019-01-18T23:00:00\"                " +
"                                    }                                                       " +
"                                ]                                                           " +
"                            },                                                              " +
"                            {                                                               " +
"                                \"id\": 33,                                                 " +
"                                \"name\": \"Write test specs\",                             " +
"                                \"percentDone\": 9,                                         " +
"                                \"duration\": 5,                                            " +
"                                \"startDate\": \"2019-01-21\",                              " +
"                                \"expanded\": true,                                         " +
"                                \"children\": [                                             " +
"                                    {                                                       " +
"                                        \"id\": 331,                                        " +
"                                        \"name\": \"Unit tests\",                           " +
"                                        \"percentDone\": 20,                                " +
"                                        \"startDate\": \"2019-01-21\",                      " +
"                                        \"duration\": 10,                                   " +
"                                        \"endDate\": \"2019-02-02\",                        " +
"                                        \"showInTimeline\": true,                           " +
"                                        \"cost\": 7000,                                     " +
"                                        \"baselines\": [                                    " +
"                                            {                                               " +
"                                                \"startDate\": \"2019-01-20T23:00:00\",     " +
"                                                \"endDate\": \"2019-02-01T23:00:00\"        " +
"                                            },                                              " +
"                                            {                                               " +
"                                                \"startDate\": \"2019-01-20T23:00:00\",     " +
"                                                \"endDate\": \"2019-02-01T23:00:00\"        " +
"                                            },                                              " +
"                                            {                                               " +
"                                                \"startDate\": \"2019-01-20T23:00:00\",     " +
"                                                \"endDate\": \"2019-02-01T23:00:00\"        " +
"                                            }                                               " +
"                                        ]                                                   " +
"                                    },                                                      " +
"                                    {                                                       " +
"                                        \"id\": 332,                                        " +
"                                        \"name\": \"UI unit tests / individual screens\",   " +
"                                        \"percentDone\": 10,                                " +
"                                        \"startDate\": \"2019-01-21\",                      " +
"                                        \"duration\": 5,                                    " +
"                                        \"endDate\": \"2019-01-26\",                        " +
"                                        \"showInTimeline\": true,                           " +
"                                        \"cost\": 5000,                                     " +
"                                        \"baselines\": [                                    " +
"                                            {                                               " +
"                                                \"startDate\": \"2019-01-20T23:00:00\",     " +
"                                                \"endDate\": \"2019-01-25T23:00:00\"        " +
"                                            },                                              " +
"                                            {                                               " +
"                                                \"startDate\": \"2019-01-20T23:00:00\",     " +
"                                                \"endDate\": \"2019-01-25T23:00:00\"        " +
"                                            },                                              " +
"                                            {                                               " +
"                                                \"startDate\": \"2019-01-20T23:00:00\",     " +
"                                                \"endDate\": \"2019-01-25T23:00:00\"        " +
"                                            }                                               " +
"                                        ]                                                   " +
"                                    },                                                      " +
"                                    {                                                       " +
"                                        \"id\": 333,                                        " +
"                                        \"name\": \"Application tests\",                    " +
"                                        \"percentDone\": 0,                                 " +
"                                        \"startDate\": \"2019-01-21\",                      " +
"                                        \"duration\": 10,                                   " +
"                                        \"endDate\": \"2019-02-02\",                        " +
"                                        \"cost\": 2500,                                     " +
"                                        \"baselines\": [                                    " +
"                                            {                                               " +
"                                                \"startDate\": \"2019-01-20T23:00:00\",     " +
"                                                \"endDate\": \"2019-02-01T23:00:00\"        " +
"                                            },                                              " +
"                                            {                                               " +
"                                                \"startDate\": \"2019-01-20T23:00:00\",     " +
"                                                \"endDate\": \"2019-02-01T23:00:00\"        " +
"                                            },                                              " +
"                                            {                                               " +
"                                                \"startDate\": \"2019-01-20T23:00:00\",     " +
"                                                \"endDate\": \"2019-02-01T23:00:00\"        " +
"                                            }                                               " +
"                                        ]                                                   " +
"                                    },                                                      " +
"                                    {                                                       " +
"                                        \"id\": 334,                                        " +
"                                        \"name\": \"Monkey tests\",                         " +
"                                        \"percentDone\": 0,                                 " +
"                                        \"startDate\": \"2019-01-21\",                      " +
"                                        \"duration\": 1,                                    " +
"                                        \"endDate\": \"2019-01-22\",                        " +
"                                        \"cost\": 250,                                      " +
"                                        \"baselines\": [                                    " +
"                                            {                                               " +
"                                                \"startDate\": \"2019-01-20T23:00:00\",     " +
"                                                \"endDate\": \"2019-01-21T23:00:00\"        " +
"                                            },                                              " +
"                                            {                                               " +
"                                                \"startDate\": \"2019-01-20T23:00:00\",     " +
"                                                \"endDate\": \"2019-01-21T23:00:00\"        " +
"                                            },                                              " +
"                                            {                                               " +
"                                                \"startDate\": \"2019-01-20T23:00:00\",     " +
"                                                \"endDate\": \"2019-01-21T23:00:00\"        " +
"                                            }                                               " +
"                                        ]                                                   " +
"                                    }                                                       " +
"                                ],                                                          " +
"                                \"endDate\": \"2019-02-02\",                                " +
"                                \"baselines\": [                                            " +
"                                    {                                                       " +
"                                        \"startDate\": \"2019-01-20T23:00:00\",             " +
"                                        \"endDate\": \"2019-02-01T23:00:00\"                " +
"                                    },                                                      " +
"                                    {                                                       " +
"                                        \"startDate\": \"2019-01-20T23:00:00\",             " +
"                                        \"endDate\": \"2019-02-01T23:00:00\"                " +
"                                    },                                                      " +
"                                    {                                                       " +
"                                        \"startDate\": \"2019-01-20T23:00:00\",             " +
"                                        \"endDate\": \"2019-02-01T23:00:00\"                " +
"                                    }                                                       " +
"                                ]                                                           " +
"                            }                                                               " +
"                        ],                                                                  " +
"                        \"endDate\": \"2019-02-02\",                                        " +
"                        \"baselines\": [                                                    " +
"                            {                                                               " +
"                                \"startDate\": \"2019-01-13T23:00:00\",                     " +
"                                \"endDate\": \"2019-02-01T23:00:00\"                        " +
"                            },                                                              " +
"                            {                                                               " +
"                                \"startDate\": \"2019-01-13T23:00:00\",                     " +
"                                \"endDate\": \"2019-02-01T23:00:00\"                        " +
"                            },                                                              " +
"                            {                                                               " +
"                                \"startDate\": \"2019-01-13T23:00:00\",                     " +
"                                \"endDate\": \"2019-02-01T23:00:00\"                        " +
"                            }                                                               " +
"                        ]                                                                   " +
"                    },                                                                      " +
"                    {                                                                       " +
"                        \"id\": 4,                                                          " +
"                        \"name\": \"Application Implementation\",                           " +
"                        \"percentDone\": 60,                                                " +
"                        \"startDate\": \"2019-02-04\",                                      " +
"                        \"expanded\": true,                                                 " +
"                        \"children\": [                                                     " +
"                            {                                                               " +
"                                \"id\": 400,                                                " +
"                                \"name\": \"Phase #1\",                                     " +
"                                \"expanded\": true,                                         " +
"                                \"children\": [                                             " +
"                                    {                                                       " +
"                                        \"id\": 41,                                         " +
"                                        \"name\": \"Authentication module\",                " +
"                                        \"percentDone\": 100,                               " +
"                                        \"duration\": 5,                                    " +
"                                        \"startDate\": \"2019-02-04\",                      " +
"                                        \"endDate\": \"2019-02-09\",                        " +
"                                        \"cost\": 8000,                                     " +
"                                        \"baselines\": [                                    " +
"                                            {                                               " +
"                                                \"startDate\": \"2019-02-03T23:00:00\",     " +
"                                                \"endDate\": \"2019-02-08T23:00:00\"        " +
"                                            },                                              " +
"                                            {                                               " +
"                                                \"startDate\": \"2019-02-03T23:00:00\",     " +
"                                                \"endDate\": \"2019-02-08T23:00:00\"        " +
"                                            },                                              " +
"                                            {                                               " +
"                                                \"startDate\": \"2019-02-03T23:00:00\",     " +
"                                                \"endDate\": \"2019-02-08T23:00:00\"        " +
"                                            }                                               " +
"                                        ]                                                   " +
"                                    },                                                      " +
"                                    {                                                       " +
"                                        \"id\": 42,                                         " +
"                                        \"name\": \"Single sign on\",                       " +
"                                        \"percentDone\": 100,                               " +
"                                        \"duration\": 3,                                    " +
"                                        \"startDate\": \"2019-02-04\",                      " +
"                                        \"endDate\": \"2019-02-07\",                        " +
"                                        \"cost\": 4700,                                     " +
"                                        \"baselines\": [                                    " +
"                                            {                                               " +
"                                                \"startDate\": \"2019-02-03T23:00:00\",     " +
"                                                \"endDate\": \"2019-02-06T23:00:00\"        " +
"                                            },                                              " +
"                                            {                                               " +
"                                                \"startDate\": \"2019-02-03T23:00:00\",     " +
"                                                \"endDate\": \"2019-02-06T23:00:00\"        " +
"                                            },                                              " +
"                                            {                                               " +
"                                                \"startDate\": \"2019-02-03T23:00:00\",     " +
"                                                \"endDate\": \"2019-02-06T23:00:00\"        " +
"                                            }                                               " +
"                                        ]                                                   " +
"                                    },                                                      " +
"                                    {                                                       " +
"                                        \"id\": 43,                                         " +
"                                        \"name\": \"Implement role based access\",          " +
"                                        \"percentDone\": 0,                                 " +
"                                        \"duration\": 4,                                    " +
"                                        \"startDate\": \"2019-02-04\",                      " +
"                                        \"endDate\": \"2019-02-08\",                        " +
"                                        \"cost\": 5800,                                     " +
"                                        \"baselines\": [                                    " +
"                                            {                                               " +
"                                                \"startDate\": \"2019-02-03T23:00:00\",     " +
"                                                \"endDate\": \"2019-02-07T23:00:00\"        " +
"                                            },                                              " +
"                                            {                                               " +
"                                                \"startDate\": \"2019-02-03T23:00:00\",     " +
"                                                \"endDate\": \"2019-02-07T23:00:00\"        " +
"                                            },                                              " +
"                                            {                                               " +
"                                                \"startDate\": \"2019-02-03T23:00:00\",     " +
"                                                \"endDate\": \"2019-02-07T23:00:00\"        " +
"                                            }                                               " +
"                                        ]                                                   " +
"                                    },                                                      " +
"                                    {                                                       " +
"                                        \"id\": 44,                                         " +
"                                        \"name\": \"Basic test coverage\",                  " +
"                                        \"showInTimeline\": true,                           " +
"                                        \"percentDone\": 0,                                 " +
"                                        \"duration\": 3,                                    " +
"                                        \"startDate\": \"2019-02-04\",                      " +
"                                        \"endDate\": \"2019-02-07\",                        " +
"                                        \"cost\": 7000,                                     " +
"                                        \"baselines\": [                                    " +
"                                            {                                               " +
"                                                \"startDate\": \"2019-02-03T23:00:00\",     " +
"                                                \"endDate\": \"2019-02-06T23:00:00\"        " +
"                                            },                                              " +
"                                            {                                               " +
"                                                \"startDate\": \"2019-02-03T23:00:00\",     " +
"                                                \"endDate\": \"2019-02-06T23:00:00\"        " +
"                                            },                                              " +
"                                            {                                               " +
"                                                \"startDate\": \"2019-02-03T23:00:00\",     " +
"                                                \"endDate\": \"2019-02-06T23:00:00\"        " +
"                                            }                                               " +
"                                        ]                                                   " +
"                                    },                                                      " +
"                                    {                                                       " +
"                                        \"id\": 45,                                         " +
"                                        \"name\": \"Verify high test coverage\",            " +
"                                        \"percentDone\": 0,                                 " +
"                                        \"duration\": 2,                                    " +
"                                        \"startDate\": \"2019-02-11\",                      " +
"                                        \"endDate\": \"2019-02-13\",                        " +
"                                        \"cost\": 16000,                                    " +
"                                        \"baselines\": [                                    " +
"                                            {                                               " +
"                                                \"startDate\": \"2019-02-11\",              " +
"                                                \"endDate\": \"2019-02-13\"                 " +
"                                            },                                              " +
"                                            {                                               " +
"                                                \"startDate\": \"2019-02-11\",              " +
"                                                \"endDate\": \"2019-02-13\"                 " +
"                                            },                                              " +
"                                            {                                               " +
"                                                \"startDate\": \"2019-02-11\",              " +
"                                                \"endDate\": \"2019-02-13\"                 " +
"                                            }                                               " +
"                                        ]                                                   " +
"                                    },                                                      " +
"                                    {                                                       " +
"                                        \"id\": 46,                                         " +
"                                        \"name\": \"Make backup\",                          " +
"                                        \"percentDone\": 0,                                 " +
"                                        \"duration\": 0,                                    " +
"                                        \"startDate\": \"2019-02-13\",                      " +
"                                        \"endDate\": \"2019-02-13\",                        " +
"                                        \"showInTimeline\": true,                           " +
"                                        \"rollup\": true,                                   " +
"                                        \"cost\": 500,                                      " +
"                                        \"baselines\": [                                    " +
"                                            {                                               " +
"                                                \"startDate\": \"2019-02-11\",              " +
"                                                \"endDate\": \"2019-02-11\"                 " +
"                                            },                                              " +
"                                            {                                               " +
"                                                \"startDate\": \"2019-02-12\",              " +
"                                                \"endDate\": \"2019-02-12\"                 " +
"                                            },                                              " +
"                                            {                                               " +
"                                                \"startDate\": \"2019-02-13\",              " +
"                                                \"endDate\": \"2019-02-13\"                 " +
"                                            }                                               " +
"                                        ]                                                   " +
"                                    }                                                       " +
"                                ],                                                          " +
"                                \"startDate\": \"2019-02-04\",                              " +
"                                \"endDate\": \"2019-02-09\",                                " +
"                                \"baselines\": [                                            " +
"                                    {                                                       " +
"                                        \"startDate\": \"2019-02-03T23:00:00\",             " +
"                                        \"endDate\": \"2019-02-08T23:00:00\"                " +
"                                    },                                                      " +
"                                    {                                                       " +
"                                        \"startDate\": \"2019-02-03T23:00:00\",             " +
"                                        \"endDate\": \"2019-02-08T23:00:00\"                " +
"                                    },                                                      " +
"                                    {                                                       " +
"                                        \"startDate\": \"2019-02-03T23:00:00\",             " +
"                                        \"endDate\": \"2019-02-08T23:00:00\"                " +
"                                    }                                                       " +
"                                ]                                                           " +
"                            },                                                              " +
"                            {                                                               " +
"                                \"id\": 401,                                                " +
"                                \"name\": \"Phase #2\",                                     " +
"                                \"expanded\": true,                                         " +
"                                \"children\": [                                             " +
"                                    {                                                       " +
"                                        \"id\": 4011,                                       " +
"                                        \"name\": \"Authentication module\",                " +
"                                        \"percentDone\": 70,                                " +
"                                        \"duration\": 15,                                   " +
"                                        \"startDate\": \"2019-02-11\",                      " +
"                                        \"endDate\": \"2019-03-02\",                        " +
"                                        \"cost\": 1200,                                     " +
"                                        \"baselines\": [                                    " +
"                                            {                                               " +
"                                                \"startDate\": \"2019-02-10T23:00:00\",     " +
"                                                \"endDate\": \"2019-03-01T23:00:00\"        " +
"                                            },                                              " +
"                                            {                                               " +
"                                                \"startDate\": \"2019-02-10T23:00:00\",     " +
"                                                \"endDate\": \"2019-03-01T23:00:00\"        " +
"                                            },                                              " +
"                                            {                                               " +
"                                                \"startDate\": \"2019-02-10T23:00:00\",     " +
"                                                \"endDate\": \"2019-03-01T23:00:00\"        " +
"                                            }                                               " +
"                                        ]                                                   " +
"                                    },                                                      " +
"                                    {                                                       " +
"                                        \"id\": 4012,                                       " +
"                                        \"name\": \"Single sign on\",                       " +
"                                        \"percentDone\": 60,                                " +
"                                        \"duration\": 5,                                    " +
"                                        \"startDate\": \"2019-02-11\",                      " +
"                                        \"endDate\": \"2019-02-16\",                        " +
"                                        \"cost\": 2500,                                     " +
"                                        \"baselines\": [                                    " +
"                                            {                                               " +
"                                                \"startDate\": \"2019-02-10T23:00:00\",     " +
"                                                \"endDate\": \"2019-02-15T23:00:00\"        " +
"                                            },                                              " +
"                                            {                                               " +
"                                                \"startDate\": \"2019-02-10T23:00:00\",     " +
"                                                \"endDate\": \"2019-02-15T23:00:00\"        " +
"                                            },                                              " +
"                                            {                                               " +
"                                                \"startDate\": \"2019-02-10T23:00:00\",     " +
"                                                \"endDate\": \"2019-02-15T23:00:00\"        " +
"                                            }                                               " +
"                                        ]                                                   " +
"                                    },                                                      " +
"                                    {                                                       " +
"                                        \"id\": 4013,                                       " +
"                                        \"name\": \"Implement role based access\",          " +
"                                        \"percentDone\": 50,                                " +
"                                        \"duration\": 21,                                   " +
"                                        \"startDate\": \"2019-02-11\",                      " +
"                                        \"endDate\": \"2019-03-12\",                        " +
"                                        \"cost\": 4100,                                     " +
"                                        \"baselines\": [                                    " +
"                                            {                                               " +
"                                                \"startDate\": \"2019-02-10T23:00:00\",     " +
"                                                \"endDate\": \"2019-03-11T23:00:00\"        " +
"                                            },                                              " +
"                                            {                                               " +
"                                                \"startDate\": \"2019-02-10T23:00:00\",     " +
"                                                \"endDate\": \"2019-03-11T23:00:00\"        " +
"                                            },                                              " +
"                                            {                                               " +
"                                                \"startDate\": \"2019-02-10T23:00:00\",     " +
"                                                \"endDate\": \"2019-03-11T23:00:00\"        " +
"                                            }                                               " +
"                                        ]                                                   " +
"                                    },                                                      " +
"                                    {                                                       " +
"                                        \"id\": 4014,                                       " +
"                                        \"name\": \"Basic test coverage\",                  " +
"                                        \"percentDone\": 0,                                 " +
"                                        \"duration\": 20,                                   " +
"                                        \"startDate\": \"2019-02-11\",                      " +
"                                        \"endDate\": \"2019-03-09\",                        " +
"                                        \"cost\": 1100,                                     " +
"                                        \"baselines\": [                                    " +
"                                            {                                               " +
"                                                \"startDate\": \"2019-02-10T23:00:00\",     " +
"                                                \"endDate\": \"2019-03-08T23:00:00\"        " +
"                                            },                                              " +
"                                            {                                               " +
"                                                \"startDate\": \"2019-02-10T23:00:00\",     " +
"                                                \"endDate\": \"2019-03-08T23:00:00\"        " +
"                                            },                                              " +
"                                            {                                               " +
"                                                \"startDate\": \"2019-02-10T23:00:00\",     " +
"                                                \"endDate\": \"2019-03-08T23:00:00\"        " +
"                                            }                                               " +
"                                        ]                                                   " +
"                                    },                                                      " +
"                                    {                                                       " +
"                                        \"id\": 4015,                                       " +
"                                        \"name\": \"Verify high test coverage\",            " +
"                                        \"percentDone\": 0,                                 " +
"                                        \"duration\": 4,                                    " +
"                                        \"startDate\": \"2019-02-11\",                      " +
"                                        \"endDate\": \"2019-02-15\",                        " +
"                                        \"cost\": 3000,                                     " +
"                                        \"baselines\": [                                    " +
"                                            {                                               " +
"                                                \"startDate\": \"2019-02-10T23:00:00\",     " +
"                                                \"endDate\": \"2019-02-14T23:00:00\"        " +
"                                            },                                              " +
"                                            {                                               " +
"                                                \"startDate\": \"2019-02-10T23:00:00\",     " +
"                                                \"endDate\": \"2019-02-14T23:00:00\"        " +
"                                            },                                              " +
"                                            {                                               " +
"                                                \"startDate\": \"2019-02-10T23:00:00\",     " +
"                                                \"endDate\": \"2019-02-14T23:00:00\"        " +
"                                            }                                               " +
"                                        ]                                                   " +
"                                    }                                                       " +
"                                ],                                                          " +
"                                \"startDate\": \"2019-02-11\",                              " +
"                                \"endDate\": \"2019-03-12\",                                " +
"                                \"baselines\": [                                            " +
"                                    {                                                       " +
"                                        \"startDate\": \"2019-02-10T23:00:00\",             " +
"                                        \"endDate\": \"2019-03-11T23:00:00\"                " +
"                                    },                                                      " +
"                                    {                                                       " +
"                                        \"startDate\": \"2019-02-10T23:00:00\",             " +
"                                        \"endDate\": \"2019-03-11T23:00:00\"                " +
"                                    },                                                      " +
"                                    {                                                       " +
"                                        \"startDate\": \"2019-02-10T23:00:00\",             " +
"                                        \"endDate\": \"2019-03-11T23:00:00\"                " +
"                                    }                                                       " +
"                                ]                                                           " +
"                            },                                                              " +
"                            {                                                               " +
"                                \"id\": 402,                                                " +
"                                \"name\": \"Acceptance phase\",                             " +
"                                \"expanded\": true,                                         " +
"                                \"children\": [                                             " +
"                                    {                                                       " +
"                                        \"id\": 4031,                                       " +
"                                        \"name\": \"Company bug bash\",                     " +
"                                        \"percentDone\": 70,                                " +
"                                        \"duration\": 3,                                    " +
"                                        \"startDate\": \"2019-03-12\",                      " +
"                                        \"endDate\": \"2019-03-15\",                        " +
"                                        \"cost\": 10000,                                    " +
"                                        \"baselines\": [                                    " +
"                                            {                                               " +
"                                                \"startDate\": \"2019-03-11T23:00:00\",     " +
"                                                \"endDate\": \"2019-03-14T23:00:00\"        " +
"                                            },                                              " +
"                                            {                                               " +
"                                                \"startDate\": \"2019-03-11T23:00:00\",     " +
"                                                \"endDate\": \"2019-03-14T23:00:00\"        " +
"                                            },                                              " +
"                                            {                                               " +
"                                                \"startDate\": \"2019-03-11T23:00:00\",     " +
"                                                \"endDate\": \"2019-03-14T23:00:00\"        " +
"                                            }                                               " +
"                                        ]                                                   " +
"                                    },                                                      " +
"                                    {                                                       " +
"                                        \"id\": 4032,                                       " +
"                                        \"name\": \"Test all web pages\",                   " +
"                                        \"percentDone\": 60,                                " +
"                                        \"duration\": 2,                                    " +
"                                        \"startDate\": \"2019-03-12\",                      " +
"                                        \"endDate\": \"2019-03-14\",                        " +
"                                        \"cost\": 5000,                                     " +
"                                        \"baselines\": [                                    " +
"                                            {                                               " +
"                                                \"startDate\": \"2019-03-11T23:00:00\",     " +
"                                                \"endDate\": \"2019-03-13T23:00:00\"        " +
"                                            },                                              " +
"                                            {                                               " +
"                                                \"startDate\": \"2019-03-11T23:00:00\",     " +
"                                                \"endDate\": \"2019-03-13T23:00:00\"        " +
"                                            },                                              " +
"                                            {                                               " +
"                                                \"startDate\": \"2019-03-11T23:00:00\",     " +
"                                                \"endDate\": \"2019-03-13T23:00:00\"        " +
"                                            }                                               " +
"                                        ]                                                   " +
"                                    },                                                      " +
"                                    {                                                       " +
"                                        \"id\": 4033,                                       " +
"                                        \"name\": \"Verify no broken links\",               " +
"                                        \"percentDone\": 50,                                " +
"                                        \"duration\": 4,                                    " +
"                                        \"startDate\": \"2019-03-12\",                      " +
"                                        \"endDate\": \"2019-03-16\",                        " +
"                                        \"cost\": 1000,                                     " +
"                                        \"baselines\": [                                    " +
"                                            {                                               " +
"                                                \"startDate\": \"2019-03-11T23:00:00\",     " +
"                                                \"endDate\": \"2019-03-15T23:00:00\"        " +
"                                            },                                              " +
"                                            {                                               " +
"                                                \"startDate\": \"2019-03-11T23:00:00\",     " +
"                                                \"endDate\": \"2019-03-15T23:00:00\"        " +
"                                            },                                              " +
"                                            {                                               " +
"                                                \"startDate\": \"2019-03-11T23:00:00\",     " +
"                                                \"endDate\": \"2019-03-15T23:00:00\"        " +
"                                            }                                               " +
"                                        ]                                                   " +
"                                    },                                                      " +
"                                    {                                                       " +
"                                        \"id\": 4034,                                       " +
"                                        \"name\": \"Make test release\",                    " +
"                                        \"percentDone\": 0,                                 " +
"                                        \"duration\": 3,                                    " +
"                                        \"startDate\": \"2019-03-12\",                      " +
"                                        \"endDate\": \"2019-03-15\",                        " +
"                                        \"cost\": 1200,                                     " +
"                                        \"baselines\": [                                    " +
"                                            {                                               " +
"                                                \"startDate\": \"2019-03-11T23:00:00\",     " +
"                                                \"endDate\": \"2019-03-14T23:00:00\"        " +
"                                            },                                              " +
"                                            {                                               " +
"                                                \"startDate\": \"2019-03-11T23:00:00\",     " +
"                                                \"endDate\": \"2019-03-14T23:00:00\"        " +
"                                            },                                              " +
"                                            {                                               " +
"                                                \"startDate\": \"2019-03-11T23:00:00\",     " +
"                                                \"endDate\": \"2019-03-14T23:00:00\"        " +
"                                            }                                               " +
"                                        ]                                                   " +
"                                    },                                                      " +
"                                    {                                                       " +
"                                        \"id\": 4035,                                       " +
"                                        \"name\": \"Send invitation email\",                " +
"                                        \"percentDone\": 0,                                 " +
"                                        \"duration\": 0,                                    " +
"                                        \"startDate\": \"2019-03-15\",                      " +
"                                        \"endDate\": \"2019-03-16\",                        " +
"                                        \"cost\": 250,                                      " +
"                                        \"baselines\": [                                    " +
"                                            {                                               " +
"                                                \"startDate\": \"2019-03-14T23:00:00\",     " +
"                                                \"endDate\": \"2019-03-14T23:00:00\"        " +
"                                            },                                              " +
"                                            {                                               " +
"                                                \"startDate\": \"2019-03-13T00:00:00\",     " +
"                                                \"endDate\": \"2019-03-13T00:00:00\"        " +
"                                            },                                              " +
"                                            {                                               " +
"                                                \"startDate\": \"2019-03-12T00:00:00\",     " +
"                                                \"endDate\": \"2019-03-12T00:00:00\"        " +
"                                            }                                               " +
"                                        ]                                                   " +
"                                    },                                                      " +
"                                    {                                                       " +
"                                        \"id\": 4036,                                       " +
"                                        \"name\": \"Celebrate launch\",                     " +
"                                        \"iconCls\": \"b-fa b-fa-glass-cheers\",            " +
"                                        \"percentDone\": 0,                                 " +
"                                        \"duration\": 1,                                    " +
"                                        \"startDate\": \"2019-03-12\",                      " +
"                                        \"endDate\": \"2019-03-13\",                        " +
"                                        \"cost\": 2500,                                     " +
"                                        \"baselines\": [                                    " +
"                                            {                                               " +
"                                                \"startDate\": \"2019-03-11T23:00:00\",     " +
"                                                \"endDate\": \"2019-03-12T23:00:00\"        " +
"                                            },                                              " +
"                                            {                                               " +
"                                                \"startDate\": \"2019-03-11T23:00:00\",     " +
"                                                \"endDate\": \"2019-03-12T23:00:00\"        " +
"                                            },                                              " +
"                                            {                                               " +
"                                                \"startDate\": \"2019-03-11T23:00:00\",     " +
"                                                \"endDate\": \"2019-03-12T23:00:00\"        " +
"                                            }                                               " +
"                                        ]                                                   " +
"                                    }                                                       " +
"                                ],                                                          " +
"                                \"startDate\": \"2019-03-12\",                              " +
"                                \"endDate\": \"2019-03-16\",                                " +
"                                \"baselines\": [                                            " +
"                                    {                                                       " +
"                                        \"startDate\": \"2019-03-11T23:00:00\",             " +
"                                        \"endDate\": \"2019-03-15T23:00:00\"                " +
"                                    },                                                      " +
"                                    {                                                       " +
"                                        \"startDate\": \"2019-03-11T23:00:00\",             " +
"                                        \"endDate\": \"2019-03-15T23:00:00\"                " +
"                                    },                                                      " +
"                                    {                                                       " +
"                                        \"startDate\": \"2019-03-11T23:00:00\",             " +
"                                        \"endDate\": \"2019-03-15T23:00:00\"                " +
"                                    }                                                       " +
"                                ]                                                           " +
"                            }                                                               " +
"                        ],                                                                  " +
"                        \"endDate\": \"2019-03-16\",                                        " +
"                        \"baselines\": [                                                    " +
"                            {                                                               " +
"                                \"startDate\": \"2019-02-03T23:00:00\",                     " +
"                                \"endDate\": \"2019-03-15T23:00:00\"                        " +
"                            },                                                              " +
"                            {                                                               " +
"                                \"startDate\": \"2019-02-03T23:00:00\",                     " +
"                                \"endDate\": \"2019-03-15T23:00:00\"                        " +
"                            },                                                              " +
"                            {                                                               " +
"                                \"startDate\": \"2019-02-03T23:00:00\",                     " +
"                                \"endDate\": \"2019-03-15T23:00:00\"                        " +
"                            }                                                               " +
"                        ]                                                                   " +
"                    }                                                                       " +
"                ],                                                                          " +
"                \"endDate\": \"2019-03-16\",                                                " +
"                \"baselines\": [                                                            " +
"                    {                                                                       " +
"                        \"startDate\": \"2019-01-13T23:00:00\",                             " +
"                        \"endDate\": \"2019-03-15T23:00:00\"                                " +
"                    },                                                                      " +
"                    {                                                                       " +
"                        \"startDate\": \"2019-01-13T23:00:00\",                             " +
"                        \"endDate\": \"2019-03-15T23:00:00\"                                " +
"                    },                                                                      " +
"                    {                                                                       " +
"                        \"startDate\": \"2019-01-13T23:00:00\",                             " +
"                        \"endDate\": \"2019-03-15T23:00:00\"                                " +
"                    }                                                                       " +
"                ]                                                                           " +
"            }                                                                               " +
"        ]                                                                                   " +
"    }                                                                                       " +
"}                                                                                           ";

            
            JavaScriptSerializer j = new JavaScriptSerializer();
            object a = j.Deserialize(str, typeof(object));
            return Json(a, JsonRequestBehavior.AllowGet);

        }

        //[System.Web.Mvc.HttpGet]
        //public ActionResult GetGanttChartData()
        //{
        //    Int32 ProjectID = Convert.ToInt32(Session["PlannerProjectID"]);
        //    //if(ProjectID == 0) ProjectID = 1111;
        //    var gantt = new Gantt();
        //    //StringBuilder sbdJSON = new StringBuilder();
        //    //sbdJSON.Append("{\"success\":true,\"project\":{\"calendar\":\"general\"}");
        //    string strJSON = "{\"success\":true,\"project\":{\"calendar\":\"general\"}";
        //    // Calendar
        //    var calen = gantt.getCalendars();
        //    string strCalen = JsonConvert.SerializeObject(calen, new JsonSerializerSettings
        //    {
        //        ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver()
        //    });
        //    //sbdJSON.AppendFormat(",\"calendars\":{\"rows\" :{0}}", strCalen);
        //    strJSON += ",\"calendars\":{\"rows\" :" + strCalen + "}";
        //    // decode request object
        //    // Task
        //    var e = gantt.getTasks(ProjectID).ToList<Task>();
        //    string str = JsonConvert.SerializeObject(e, new JsonSerializerSettings
        //    {
        //        ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver()
        //    });
        //    //str = "{\"success\":true,\"tasks\":{\"rows\" :" + str + "}";
        //    //sbdJSON.AppendFormat(",\"tasks\":{\"rows\" :{0}}",str);
        //    strJSON += ",\"tasks\":{\"rows\" :" + str + "}";
        //    var de = gantt.getDependencies();
        //    if (de.Count() > 0)
        //    {
        //        string strDe = JsonConvert.SerializeObject(de, new JsonSerializerSettings
        //        {
        //            ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver()
        //        });
        //        strDe = strDe.Replace("from", "fromTask").Replace("to", "toTask");

        //        //str = str + ",\"dependencies\" : {\"rows\" : " + strDe + "}";
        //        //sbdJSON.AppendFormat(",\"dependencies\" : {\"rows\" : {0}}", strDe);
        //        strJSON += ",\"dependencies\" : {\"rows\" : " + strDe + "}";
        //    }
        //    //str += "}";
        //    //sbdJSON.Append("}");
        //    strJSON += "}";
        //    JavaScriptSerializer j = new JavaScriptSerializer();
        //    //object a = j.Deserialize(str, typeof(object));
        //    object a = j.Deserialize(strJSON, typeof(object));
        //    return Json(a, JsonRequestBehavior.AllowGet);

        //    //return Content(str, "application/json");

        //}

        //[HttpPost]
        //public JsonResult GanttChartModifyData(string TaskData, string DepData)
        //{
        //    try
        //    {
        //        var gantt = new Gantt();
        //        Int32 ProjectID = Convert.ToInt32(Session["PlannerProjectID"]);
        //        if (!string.IsNullOrEmpty(TaskData))
        //        {
        //            TaskData = TaskData.Replace(",\"id\":\"_generated_0x422c2b1\"","").Replace("_generated_0x422c2b1", "");
        //            var lstTasks = new List<Task>();
        //            try
        //            {
        //                lstTasks = JsonConvert.DeserializeObject<List<Task>>(TaskData);
        //            }
        //            catch (Exception es)
        //            {
        //            }
                    
        //            gantt.UpdateTasks(lstTasks);
        //        }

        //        if (!string.IsNullOrEmpty(DepData))
        //        {
        //            var lstDependencies = JsonConvert.DeserializeObject<List<Dependency>>(DepData);

        //            gantt.UpdateDependencies(lstDependencies.Distinct().ToList(), ProjectID);
        //        }

        //        gantt.context.SaveChanges();
        //        return Json(new { success = true });
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { success = false });
        //    }
        //}
    }
}