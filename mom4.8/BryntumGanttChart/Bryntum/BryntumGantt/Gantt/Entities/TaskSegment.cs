using BryntumGanttChart.Bryntum.BryntumCRUD.CRUD.Entities;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace BryntumGanttChart.Bryntum.BryntumGantt
{

    [MetadataType(typeof(TaskSegmentMetadata))]
    public partial class TaskSegment : GeneralBryntum
    {
        [JsonIgnore]
        public string PhantomTaskId { get; set; }

        [JsonIgnore]
        public override String PhantomIdField { get { return "PhantomId"; } }

        [JsonProperty("PhantomId")]
        public override string PhantomId { get; set; }

        public string TaskId
        {
            set
            {
                PhantomTaskId = value;
                if (value == null) return;

                try
                {
                    TaskIdRaw = Convert.ToInt32(value);
                    PhantomTaskId = value;
                }
                catch (System.Exception)
                {
                    TaskIdRaw = 0;
                }
            }

            get
            {
                return TaskIdRaw > 0 ? Convert.ToString(TaskIdRaw) : PhantomTaskId;
            }
        }
    }

    public class TaskSegmentMetadata
    {
        [JsonIgnore]
        public int TaskIdRaw { get; set; }
        [JsonIgnore]
        public virtual Task Task { get; set; }
    }
}