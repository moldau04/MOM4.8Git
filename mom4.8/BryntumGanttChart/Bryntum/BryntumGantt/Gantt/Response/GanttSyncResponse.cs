using BryntumGanttChart.Bryntum.BryntumCRUD.CRUD.Response;
using System;

namespace BryntumGanttChart.Bryntum.BryntumGantt.Gantt.Response
{
    [Serializable]
    public class GanttSyncResponse : SyncResponse
    {

        public SyncStoreResponse calendars { get; set; }

        public SyncStoreResponse resources { get; set; }

        public SyncStoreResponse tasks { get; set; }

        public SyncStoreResponse assignments { get; set; }

        public SyncStoreResponse dependencies { get; set; }

        public GanttSyncResponse()
            : base()
        {
        }

        public GanttSyncResponse(ulong? requestId) : base(requestId)
        {
        }
    }
}