Ext.define('Gnt.examples.advanced.model.Task', {
    extend  : 'Gnt.model.Task',

    fields  : [
        { name : 'index', type : 'int', persist : true },
        { name : 'expanded', type : 'bool', persist : false },
        { name : 'Color', type : 'string' },
        { name : 'ShowInTimeline', type : 'bool' }
    ],

    showInTimelineField : 'ShowInTimeline'
});
