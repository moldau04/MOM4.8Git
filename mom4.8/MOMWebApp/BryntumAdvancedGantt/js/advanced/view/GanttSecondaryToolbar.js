Ext.define('Gnt.examples.advanced.view.GanttSecondaryToolbar', {
    extend      : 'Ext.Toolbar',
    mixins      : ['Gnt.mixin.Localizable'],
    xtype       : 'gantt-secondary-toolbar',
    cls         : 'gantt-secondary-toolbar',
    reference   : 'secondaryToolbar',

    hidden      : true,

    defaults    : { scale : 'medium', margin : '0 3 0 3' },

    initComponent : function () {

        this.items  = [
            {
                text         : this.L('toggleProgressLine'),
                reference    : 'toggleProgressLine',
                pressed      : false,
                enableToggle : true
            },
            {
                text         : this.L('toggleChildTasksGrouping'),
                reference    : 'toggleGrouping',
                enableToggle : true
            },
            {
                text         : this.L('toggleRollupTasks'),
                reference    : 'toggleRollup',
                enableToggle : true
            },
            {
                text      : this.L('highlightTasksLonger8'),
                reference : 'highlightLong'
            },
            {
                text      : this.L('filterTasksWithProgressLess30'),
                reference : 'filterTasks'
            },
            {
                text      : this.L('clearFilter'),
                reference : 'clearTasksFilter'
            },
            {
                text      : this.L('scrollToLastTask'),
                reference : 'scrollToLast'
            }
        ];

        // For testing
        Ext.Array.forEach(this.items, function (cmp) {
            if (cmp.reference) cmp.itemId = cmp.reference;
        });

        this.callParent(arguments);
    }
});
