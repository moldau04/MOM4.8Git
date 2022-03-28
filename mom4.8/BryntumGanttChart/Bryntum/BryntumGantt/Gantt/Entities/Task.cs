﻿using BryntumGanttChart.Bryntum.BryntumCRUD.CRUD.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BryntumGanttChart.Bryntum.BryntumGantt
{
    [MetadataType(typeof(TaskMetadata))]
    public partial class Task : Node<Task>
    {
        [JsonIgnore]
        public string PhantomCalendarId { get; set; }

        public string CalendarId
        {
            set
            {
                PhantomCalendarId = value;
                if (value == null) return;

                try
                {
                    CalendarIdRaw = Convert.ToInt32(value);
                }
                catch (System.Exception)
                {
                    CalendarIdRaw = null;
                }
            }

            get
            {
                return CalendarIdRaw > 0 ? Convert.ToString(CalendarIdRaw) : PhantomCalendarId;
            }
        }

        public override bool leaf { get { return ChildrenRaw.Count == 0; } }

        public virtual ICollection<Task> children
        {
            get { if (!leaf) return ChildrenRaw; return null; }
            set { ChildrenRaw = value; }
        }
    }

    public class TaskMetadata : NodeMetadata<Task>
    {
        [JsonIgnore]
        public Nullable<int> CalendarIdRaw { get; set; }
        [JsonIgnore]
        public virtual ICollection<Task> ChildrenRaw { get; set; }
        [JsonIgnore]
        public virtual ICollection<Assignment> Assignments { get; set; }
        [JsonIgnore]
        public virtual Calendar Calendar { get; set; }
        [JsonIgnore]
        public virtual ICollection<Dependency> Predecessors { get; set; }
        [JsonIgnore]
        public virtual ICollection<Dependency> Successors { get; set; }
    }
}