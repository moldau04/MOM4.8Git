Ext.define('Gnt.examples.advanced.view.Gantt', {
    extend : 'Gnt.panel.Gantt',
    xtype  : 'advanced-gantt',

    requires : [
        'Ext.grid.filters.Filters',
        'Ext.form.field.Text',
        'Sch.plugin.TreeCellEditing',
        'Sch.plugin.Pan',
        'Gnt.plugin.taskeditor.TaskEditor',
        'Gnt.plugin.taskeditor.ProjectEditor',
        'Gnt.plugin.DependencyEditor',
        'Gnt.plugin.Clipboard',
        'Gnt.plugin.ProgressLine',
        'Gnt.column.Sequence',
        'Gnt.column.Name',
        'Gnt.column.StartDate',
        'Gnt.column.EndDate',
        'Gnt.column.Duration',
        'Gnt.column.ConstraintType',
        'Gnt.column.ConstraintDate',
        'Gnt.column.PercentDone',
        'Gnt.column.Predecessor',
        'Gnt.column.ManuallyScheduled',
        'Gnt.column.AddNew',
        'Gnt.column.DeadlineDate',
        'Gnt.column.DragDrop',
        'Gnt.selection.SpreadsheetModel',
        /*  */
        'Gnt.examples.advanced.plugin.TaskArea',
        'Gnt.examples.advanced.plugin.TaskContextMenu',
        'Gnt.examples.advanced.field.Filter',
        'Gnt.examples.advanced.view.TaskEditorGeneralForm'//,
        //'Gnt.plugin.Export'
    ],

    showTodayLine           : true,
    loadMask                : true,
    enableProgressBarResize : true,
    showRollupTasks         : true,
    rowHeight               : 30,
    viewPreset              : 'weekAndDayLetter',

    projectLinesConfig : {
        // Configure the gantt to mark project start dates w/ lines.
        // Options are:
        // 'start' - to show lines for start dates,
        // 'end' - to show lines for end dates,
        // 'both' - to show lines for both start and end dates.
        linesFor : 'start'
    },

    allowDeselect : true,

    selModel : {
        type : 'gantt_spreadsheet',
        // The WBS column width
        rowNumbererHeaderWidth : 70
    },

    // Define properties for the left 'locked' and scrollable tree grid
    lockedGridConfig : {
        width : 400
    },

    // Define properties for the left 'locked' and scrollable tree view
    lockedViewConfig : {
        // Adds a CSS class returned to each row element
        getRowClass : function (rec) {
            return rec.isRoot() ? 'root-row' : '';
        }
    },

    // Define a custom HTML template for regular tasks
    taskBodyTemplate : '<div class="sch-gantt-progress-bar" style="width:{progressBarWidth}px;{progressBarStyle}" unselectable="on">' +
        '<span class="sch-gantt-progress-bar-label">{[Gnt.column.PercentDone.prototype.defaultRenderer(values.percentDone)]}%</span>' +
        '</div>',

    // Define what should be shown in the left label field, along with the type of editor
    leftLabelField : {
        dataIndex : 'Name',
        editor    : { xtype : 'textfield' }
    },

    // Uncomment to show labels above rollup elements
    // rollupLabelField : 'Name',

    plugins : [
        'advanced_taskcontextmenu',
        'scheduler_pan',
        {
            ptype : 'gantt_taskeditor',
            height              : 450,
            // tell editor to use our custom TaskEditorGeneralForm for the "General" tab
            taskFormClass : 'Gnt.examples.advanced.view.TaskEditorGeneralForm'
            // https://app.assembla.com/spaces/bryntum/tickets/6846
            // dependencyGridConfig : {
            //     useSequenceNumber : true
            // }
        },
        'gantt_projecteditor',
        'gridfilters',
        {
            ptype      : 'gantt_progressline',
            disabled   : true,
            statusDate : new Date(2017, 1, 6), // Optional
            id         : 'progressline'
        },
        {
            ptype : 'gantt_dependencyeditor',
            width : 320
        },
        {
            pluginId : 'taskarea',
            ptype    : 'taskarea'
        },
        {
            ptype        : 'scheduler_treecellediting',
            clicksToEdit : 2,
            pluginId     : 'editingInterface'
        },
        {
            ptype : 'gantt_clipboard',
            // data copied in raw format can be copied and pasted to gantt
            // data in text format is copied to system clipboard and can be pasted anywhere
            source : ['raw','text']
        },
        //{
        //    ptype: 'gantt_export',
        //    pluginId: 'export',
        //    // You can easily define your own custom HTML header (or footer) to include logo etc
        //    headerTpl: '<div class="sch-export-header" style="width:{width}px">' +
        //        '<img src="resources/your-logo.png"/>' +
        //        '<dl>' +
        //        '<dt>Date: {[Ext.Date.format(new Date(), "M d Y")]}</dt>' +
        //        '<dd>Page: {pageNo}/{totalPages}</dd>' +
        //        '</dl>' +
        //        '</div>',
        //    // translateURLsToAbsolute : 'http://localhost:8080/resources',
        //    printServer: 'http://localhost:8080'
        //},
        'gantt_selectionreplicator'
    ],

    // Define the static columns
    // Any regular Ext JS columns are ok too
    columns : [
        {   xtype     : 'dragdropcolumn' },
        {
            xtype     : 'namecolumn',
            width     : 200,
            cls       : 'namecolumn',
            layout    : 'vbox',
            items     : {
                xtype : 'gantt-filter-field'
            }
        },
        {
            xtype     : 'startdatecolumn',
            width     : 130,
            dataIndex : 'StartDate',
            filter    : {
                type : 'date'
            }
        },
        {
            xtype     : 'enddatecolumn',
            width     : 130,
            dataIndex : 'EndDate',
            filter    : {
                type : 'date'
            }
        },
        {
            xtype     : 'durationcolumn',
            width     : 100
        },
        // Uncomment to try this column
        //{
        //    xtype : 'deadlinecolumn'
        //},
        {
            xtype     : 'constrainttypecolumn'
        },
        {
            xtype     : 'constraintdatecolumn'
        },
        {
            xtype     : 'percentdonecolumn',
            width     : 100,
            dataIndex : 'PercentDone',
            filter    : {
                type : 'number'
            }
        },
        {
            xtype             : 'predecessorcolumn'
            // https://app.assembla.com/spaces/bryntum/tickets/6846
            //useSequenceNumber : true
        },
        {
            xtype : 'addnewcolumn'
        }
    ],

    eventRenderer : function (task, tplData) {
        var style,
            segments, i,
            result;

        if (task.get('Color')) {
            style = Ext.String.format('background-color: #{0};border-color:#{0}', task.get('Color'));

            if (!tplData.segments) {
                result = {
                    // Here you can add custom per-task styles
                    style : style
                };
            }
            // if task is segmented we cannot use above code
            // since it will set color of background visible between segments
            // in this case instead we need to provide "style" for each individual segment
            else {
                segments = tplData.segments;
                for (i = 0; i < segments.length; i++) {
                    segments[i].style = style;
                }
            }
        }

        return result;
    }
});

// Prevent Ext grid header key navigation reacting to arrow keys inside a text input
Ext.grid.header.Container.override({
    privates : {
        onFocusableContainerLeftKey     : function (e) {
            if (e.target.tagName.toLowerCase() !== 'input') {
                e.preventDefault();
                return this.moveChildFocus(e, false);
            }
        }, onFocusableContainerRightKey : function (e) {
            if (e.target.tagName.toLowerCase() !== 'input') {
                e.preventDefault();
                return this.moveChildFocus(e, true);
            }
        }
    }
});
