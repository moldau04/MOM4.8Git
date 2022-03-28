"use strict";

/* eslint-disable no-unused-vars */
var project = new bryntum.gantt.ProjectModel({
  transport: {
    load: {
        url: '../../_datasets/launch-saas.json'
    }
  }
});
new bryntum.gantt.Gantt({
  adopt: 'container',
  project: project,
  columns: [{
    type: 'name',
    field: 'name',
    width: 250
  }],
  // Custom task content, display task name on child tasks
  taskRenderer: function taskRenderer(_ref) {
    var taskRecord = _ref.taskRecord;

    if (taskRecord.isLeaf && !taskRecord.isMilestone) {
      return taskRecord.name;
    }
  }
});
project.load();