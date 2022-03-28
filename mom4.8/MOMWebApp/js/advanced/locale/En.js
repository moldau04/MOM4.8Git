Ext.define('Gnt.examples.advanced.locale.En', {
    extend    : 'Sch.locale.Locale',
    requires  : 'Gnt.locale.En',
    singleton : true,

    l10n : {
        'Gnt.examples.advanced.Application' : {
            error           : 'Error',
            requestError    : 'Request error'
        },

        'Gnt.examples.advanced.view.ControlHeader' : {
            previousTimespan        : 'Previous timespan',
            nextTimespan            : 'Next timespan',
            collapseAll             : 'Collapse all',
            expandAll               : 'Expand all',
            zoomOut                 : 'Zoom out',
            zoomIn                  : 'Zoom in',
            zoomToFit               : 'Zoom to fit',
            undo                    : 'Undo',
            redo                    : 'Redo',
            viewFullScreen          : 'View full screen',
            highlightCriticalPath   : 'Highlight critical path',
            addNewTask              : 'Add new task',
            newTask                 : 'New Task',
            removeSelectedTasks     : 'Remove selected task(s)',
            indent                  : 'Indent',
            outdent                 : 'Outdent',
            manageCalendars         : 'Manage calendars',
            saveChanges             : 'Save changes',
            language                : 'Language: ',
            selectLanguage          : 'Select a language...',
            tryMore                 : 'Try more features...',
            print                   : 'Print'
        },

        'Gnt.examples.advanced.plugin.TaskContextMenu' : {
            changeTaskColor         : 'Change task color'
        },

        'Gnt.examples.advanced.view.GanttSecondaryToolbar' : {
            toggleChildTasksGrouping        : 'Toggle task grouping',
            toggleRollupTasks               : 'Toggle rollup',
            highlightTasksLonger8           : 'Highlight tasks > 8 days',
            filterTasksWithProgressLess30   : 'Filter: Tasks < 30% done',
            clearFilter                     : 'Clear Filter',
            scrollToLastTask                : 'Scroll to last task',
            toggleProgressLine              : 'Toggle progress line'
        }

    },

    apply : function (classNames) {
        Gnt.locale.En.apply(classNames);
        this.callParent(arguments);
    }

});
