Ext.define("Gnt.examples.advanced.view.MainViewport", {
    extend: 'Ext.Viewport',
    //extend: 'Ext.panel.Panel',
    alias: 'widget.advanced-viewport',

    requires: [
        'Gnt.examples.advanced.view.MainViewportController',
        'Gnt.examples.advanced.view.MainViewportModel',
        'Gnt.examples.advanced.view.GanttSecondaryToolbar',
        'Gnt.examples.advanced.view.ControlHeader',
        /* @cut-if-gantt-> */
        'Gnt.examples.advanced.view.Timeline',
        /* <-@ */
        'Gnt.examples.advanced.view.Gantt'
    ],

    viewModel: 'advanced-viewport',
    controller: 'advanced-viewport',

    layout: 'border',

    initComponent: function () {
        this.items = [
            {
                xtype: 'container',
                region: 'center',
                layout: 'border',
                items: [
                    /* @cut-if-gantt-> */
                    {
                        xtype: 'advanced-timeline',
                        height: 180,
                        region: 'north',
                        taskStore: this.crudManager.getTaskStore()
                    },
                    /* <-@ */
                    {
                        xtype: 'advanced-gantt',
                        region: 'center',
                        reference: 'gantt',
                        crudManager: this.crudManager,
                        startDate: this.startDate,
                        endDate: this.endDate,
                        header: Gnt.panel.Timeline ? null : { xtype: 'controlheader' },
                        bbar: {
                            xtype: 'gantt-secondary-toolbar'
                        }
                    }
                ]
            }
            //,
            //{
            //    xtype  : 'details'
            //}
        ];

        this.callParent(arguments);
    }
});
