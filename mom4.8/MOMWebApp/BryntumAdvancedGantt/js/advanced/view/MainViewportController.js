Ext.define('Gnt.examples.advanced.view.MainViewportController', {
    extend : 'Ext.app.ViewController',
    alias  : 'controller.advanced-viewport',

    requires : ['Gnt.widget.calendar.CalendarManagerWindow'],

    control : {
        '#'                              : { afterrender : 'onAfterRender' },
        'tool[reference]'                : { click : 'onButtonClick', priority : 1000 },
        '[reference=gantt]'              : { selectionchange : 'onGanttSelectionChange' },
        '[reference=shiftPrevious]'      : { click : 'onShiftPrevious' },
        '[reference=shiftNext]'          : { click : 'onShiftNext' },
        '[reference=collapseAll]'        : { click : 'onCollapseAll' },
        '[reference=expandAll]'          : { click : 'onExpandAll' },
        '[reference=zoomOut]'            : { click : 'onZoomOut' },
        '[reference=zoomIn]'             : { click : 'onZoomIn' },
        '[reference=zoomToFit]'          : { click : 'onZoomToFit' },
        '[reference=undo]'               : { click : 'onUndo' },
        '[reference=redo]'               : { click : 'onRedo' },
        '[reference=viewFullScreen]'     : { click : 'onFullScreen' },
        '[reference=criticalPath]'       : { click : 'onHighlightCriticalPath' },
        '[reference=addTask]'            : { click : 'onAddTask' },
        '[reference=removeSelected]'     : { click : 'onRemoveSelectedTasks' },
        '[reference=indentTask]'         : { click : 'onIndent' },
        '[reference=outdentTask]'        : { click : 'onOutdent' },
        '[reference=manageCalendars]'    : { click : 'onManageCalendars' },
        '[reference=saveChanges]'        : { click : 'onSaveChanges' },
        '[reference=toggleGrouping]'     : { click : 'onToggleGrouping' },
        '[reference=toggleProgressLine]' : { click : 'onToggleProgressLine' },
        '[reference=toggleRollup]'       : { click : 'onToggleRollup' },
        '[reference=highlightLong]'      : { click : 'onHighlightLongTasks' },
        '[reference=filterTasks]'        : { click : 'onFilterTasks' },
        '[reference=clearTasksFilter]'   : { click : 'onClearTasksFilter' },
        '[reference=scrollToLast]'       : { click : 'onScrollToLast' },
        '[reference=tryMore]'            : { click : 'onTryMore' },
        '[reference=print]'              : { click : 'onPrint' },
        'combo[reference=langSelector]'  : { select : 'onLanguageSelected' },
        'gantt_timelinescheduler'        : {
            eventclick : 'onTimelineEventClick',
            eventdblclick : 'onTimelineEventDblClick'
        }
    },

    getSelectedTasks    : function () {
        var tasks = [];

        if (this.getViewModel().get('hasSelection')) {
            var selected = this.getGantt().getSelectionModel().getSelected();

            if (selected instanceof Ext.grid.selection.Rows) {
                tasks = selected.getRecords();
            } else if (selected instanceof Ext.grid.selection.Cells) {
                selected.eachRow(function (record) {
                    tasks.push(record);
                });
            }
        }

        return tasks;
    },

    getGantt : function () {
        return this.getView().lookupReference('gantt');
    },

    onButtonClick : function() {
        // stop editing process before changing the UI
        this.getGantt().cancelEdit();
    },

    onGanttSelectionChange : function (grid, selection) {
        this.getViewModel().set('hasSelection', !!selection.getCount());

        var tasks = this.getView().getController().getSelectedTasks();

        var allowIndentationChange = Ext.Array.every(tasks, function (record) {
            return !(record instanceof Gnt.model.Project);
        });

        this.getViewModel().set('allowIndentationChange', allowIndentationChange);
    },

    onShiftPrevious : function () {
        this.getGantt().shiftPrevious();
    },

    onShiftNext : function () {
        this.getGantt().shiftNext();
    },

    onCollapseAll : function () {
        this.getGantt().collapseAll();
    },

    onExpandAll : function () {
        this.getGantt().expandAll();
    },

    onZoomOut : function () {
        this.getGantt().zoomOut();
    },

    onZoomIn : function () {
        this.getGantt().zoomIn();
    },

    onZoomToFit : function () {
        this.getGantt().zoomToFit(null, { leftMargin : 100, rightMargin : 100 });
    },

    onUndo : function() {
        this.getViewModel().get('undoManager').undo();
    },

    onRedo : function() {
        this.getViewModel().get('undoManager').redo();
    },

    onFullScreen : function () {
        var bodyNode = this.getGantt().getEl().down('.x-panel-body', true);

        bodyNode[this.getFullscreenFn()](Element.ALLOW_KEYBOARD_INPUT);
    },

    // Experimental, not X-browser
    getFullscreenFn : function () {
        var docElm = document.documentElement,
            fn;

        if (docElm.requestFullscreen) {
            fn = "requestFullscreen";
        }
        else if (docElm.mozRequestFullScreen) {
            fn = "mozRequestFullScreen";
        }
        else if (docElm.webkitRequestFullScreen) {
            fn = "webkitRequestFullScreen";
        }
        else if (docElm.msRequestFullscreen) {
            fn = "msRequestFullscreen";
        }

        return fn;
    },

    onHighlightCriticalPath : function (btn) {
        var v = this.getGantt().getSchedulingView();

        btn.pressed = !btn.pressed;

        if (btn.pressed) {
            v.highlightCriticalPaths(true);
        } else {
            v.unhighlightCriticalPaths(true);
        }
    },

    onAddTask : function () {
        var gantt            = this.getGantt(),
            tasks            = this.getSelectedTasks(),
            selectedTask     = tasks[0] || gantt.getTaskStore().getRoot(),
            editingInterface = gantt.lockedGrid.getPlugin('editingInterface'),
            node             = selectedTask.isLeaf() ? selectedTask.parentNode : selectedTask;

        var record = node.appendChild({
            Name : 'New Task',
            leaf : true
        });

        editingInterface.completeEdit();

        gantt.getSchedulingView().scrollEventIntoView(record, false, false, function () {
            gantt.getSelectionModel().select(record);

            // HACK: Ext JS editing process doesn't align cell editor properly after scrolling/selecting
            setTimeout(function () {
                gantt.lockedGrid.getPlugin('editingInterface').startEdit(record, 2);
            }, 20);
        });
    },

    onRemoveSelectedTasks : function () {
        this.getGantt().getTaskStore().removeTasks(this.getSelectedTasks());
    },

    onIndent : function () {
        var gantt = this.getGantt();

        // filter out attempts to get into a readonly task
        var tasks = Ext.Array.filter(this.getSelectedTasks(), function (task) {
            return task.previousSibling && !task.previousSibling.isReadOnly();
        });

        gantt.getTaskStore().indent([].concat(tasks));
    },

    onOutdent : function () {
        var gantt = this.getGantt();

        // filter out readonly tasks
        var tasks = Ext.Array.filter(this.getSelectedTasks(), function (task) { return !task.isReadOnly(); });

        gantt.getTaskStore().outdent([].concat(tasks));
    },

    onSaveChanges : function () {
        this.getGantt().crudManager.sync();
    },

    onLanguageSelected : function (field, record) {
        this.fireEvent('locale-change', record.get('id'), record);
    },

    onToggleProgressLine : function () {
        var plugin = this.getGantt().getPlugin('progressline');

        if (plugin.disabled) {
            plugin.enable();
        } else {
            plugin.disable();
        }
    },

    onToggleGrouping : function () {
        var taPlugin = this.getGantt().getPlugin('taskarea');
        taPlugin.setEnabled(!taPlugin.getEnabled());
    },

    onToggleRollup : function () {
        var gantt = this.getGantt();
        gantt.setShowRollupTasks(!gantt.showRollupTasks);
    },

    onHighlightLongTasks : function () {
        var gantt = this.getGantt();

        gantt.taskStore.queryBy(function (task) {
            if (task.data.leaf && task.getDuration() > 8) {
                var elements = gantt.getSchedulingView().getElementsFromEventRecord(task),
                    el = elements && elements[0];
                el && el.frame('lime');
            }
        });
    },

    onFilterTasks : function () {
        this.getGantt().taskStore.filterTreeBy(function (task) {
            return task.getPercentDone() <= 30;
        });
    },

    onClearTasksFilter : function () {
        this.getGantt().taskStore.clearTreeFilter();
    },

    onScrollToLast : function () {
        var gantt = this.getGantt();
        var tasks = gantt.taskStore.getRange();

        if (tasks.length > 0) {
            gantt.getSchedulingView().scrollEventIntoView(tasks[ tasks.length - 1 ], true);
        }
    },

    onAfterRender : function () {
        var me        = this,
            viewModel = me.getViewModel(),
            taskStore = viewModel.get('taskStore');

        viewModel.set('fullscreenEnabled', !!this.getFullscreenFn());

        me.mon(taskStore, 'filter-set', function () {
            viewModel.set('filterSet', true);
        });
        me.mon(taskStore, 'filter-clear', function () {
            viewModel.set('filterSet', false);
        });

        // track CRUD manager changes
        me.mon(viewModel.get('crud'), {
            haschanges : function () {
                me.getViewModel().set('hasChanges', true);
            },
            nochanges : function () {
                me.getViewModel().set('hasChanges', false);
            }
        });

        // track UNDO manager changes
        me.mon(viewModel.get('undoManager'), {
            undoqueuechange : function(undoManager, queue) {
                me.getViewModel().set('canUndo', queue.length > 0);
            },
            redoqueuechange : function(undoManager, queue) {
                me.getViewModel().set('canRedo', queue.length > 0);
            }
        });

        viewModel.get('undoManager').start();
    },

    onManageCalendars : function () {
        var gantt = this.getGantt();

        this.calendarsWindow = new Gnt.widget.calendar.CalendarManagerWindow({
            calendarManager : gantt.getTaskStore().calendarManager,
            modal           : true
        });

        this.calendarsWindow.show();
    },

    onTryMore : function () {
        var tbar = this.getView().down('gantt-secondary-toolbar');

        tbar.setVisible(!tbar.isVisible());
    },

    onTimelineEventClick : function(panel, task) {
        this.getGantt().getSchedulingView().scrollEventIntoView(task, true, true);
    },

    onTimelineEventDblClick : function(panel, task) {
        this.getGantt().getTaskEditor(task).showTask(task);
    },

    onPrint : function () {
        this.getGantt().print();
    }
});
