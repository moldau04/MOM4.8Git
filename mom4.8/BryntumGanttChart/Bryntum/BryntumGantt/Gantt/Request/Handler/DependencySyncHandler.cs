﻿
using BryntumGanttChart.Bryntum.BryntumCRUD.CRUD.Request;
using System;
using System.Collections.Generic;

namespace BryntumGanttChart.Bryntum.BryntumGantt.Gantt.Request.Handler
{

    public class DependencySyncHandler : SyncStoreRequestHandler<Dependency>
    {

        private GanttChart gantt;

        public DependencySyncHandler(GanttChart gantt)
        {
            this.gantt = gantt;
        }

        public override Dependency GetEntity(IDictionary<String, Object> changes)
        {
            return gantt.getDependency(Convert.ToInt32(changes["Id"]));
        }

        protected IDictionary<String, Object> PrepareData(Dependency dependency)
        {
            // initialize response part related to the record
            IDictionary<String, Object> response = new Dictionary<String, Object>();

            String phantomFrom = dependency.PhantomFromId;
            if (dependency.FromIdRaw == 0 && !String.IsNullOrEmpty(phantomFrom))
            {
                int? from = gantt.getTaskIdByPhantom(phantomFrom);
                dependency.FromIdRaw = from.Value;
                // put updated From to response
                response.Add("From", from);
            }

            String phantomTo = dependency.PhantomToId;
            if (dependency.ToIdRaw == 0 && !String.IsNullOrEmpty(phantomTo))
            {
                int? to = gantt.getTaskIdByPhantom(phantomTo);
                dependency.ToIdRaw = to.Value;
                // put updated To to response
                response.Add("To", to);
            }

            return response;
        }

        public override IDictionary<String, Object> Add(Dependency dependency)
        {
            IDictionary<String, Object> response = PrepareData(dependency);
            gantt.saveDependency(dependency);
            return response;
        }

        public override IDictionary<String, Object> Update(Dependency dependency, IDictionary<String, Object> changes)
        {
            // apply changes to tthe entity
            if (changes.ContainsKey("Cls")) dependency.Cls = Convert.ToString(changes["Cls"]);
            if (changes.ContainsKey("From")) dependency.FromId = Convert.ToString(changes["From"]);
            if (changes.ContainsKey("To")) dependency.ToId = Convert.ToString(changes["To"]);
            if (changes.ContainsKey("Type")) dependency.Type = Convert.ToInt32(changes["Type"]);
            if (changes.ContainsKey("Lag")) dependency.Lag = Convert.ToInt32(changes["Lag"]);
            if (changes.ContainsKey("LagUnit")) dependency.LagUnit = Convert.ToString(changes["LagUnit"]);
            if (changes.ContainsKey(dependency.PhantomIdField))
                dependency.PhantomId = Convert.ToString(changes[dependency.PhantomIdField]);

            IDictionary<String, Object> response = PrepareData(dependency);
            gantt.saveDependency(dependency);
            return response;
        }

        public override IDictionary<String, Object> Remove(Dependency dependency)
        {
            IDictionary<String, Object> response = new Dictionary<String, Object>();
            gantt.removeDependency(dependency);
            return response;
        }
    }
}