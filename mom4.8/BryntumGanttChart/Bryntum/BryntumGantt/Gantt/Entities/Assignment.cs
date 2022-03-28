using BryntumGanttChart.Bryntum.BryntumCRUD.CRUD.Entities;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

//namespace GanttChart.Bryntum.BryntumGantt
namespace BryntumGanttChart.Bryntum.BryntumGantt
{
    [MetadataType(typeof(AssignmentMetadata))]
    public partial class Assignment : GeneralBryntum
    {
        [JsonIgnore]
        public string PhantomTaskId { get; set; }

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

        [JsonIgnore]
        public string PhantomResourceId { get; set; }

        public string ResourceId
        {
            set
            {
                PhantomResourceId = value;
                if (value == null) return;

                try
                {
                    ResourceIdRaw = Convert.ToInt32(value);
                    PhantomResourceId = value;
                }
                catch (System.Exception)
                {
                    ResourceIdRaw = 0;
                }
            }

            get
            {
                return ResourceIdRaw > 0 ? Convert.ToString(ResourceIdRaw) : PhantomResourceId;
            }
        }
    }

    public class AssignmentMetadata
    {
        [JsonIgnore]
        public int TaskIdRaw { get; set; }
        [JsonIgnore]
        public int ResourceIdRaw { get; set; }
        [JsonIgnore]
        public virtual Resource Resource { get; set; }
        [JsonIgnore]
        public virtual Task Task { get; set; }
    }
}