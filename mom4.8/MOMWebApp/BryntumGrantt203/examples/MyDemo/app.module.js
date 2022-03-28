import { Button, Toolbar, Toast, DateHelper, CSSHelper, PercentDoneColumn, ColumnStore, ResourceAssignmentColumn, Column, TaskModel, Gantt, Panel, ProjectModel, EffectResolutionResult, WidgetHelper } from '../../build/gantt.module.js?439331';
import shared from '../_shared/shared.module.js?439331';

new Button({
    type: 'button',
    text: 'Export',
    ref: 'excelExportBtn',
    color: 'b-blue b-raised',
    insertFirst: document.getElementById('tools') || document.body,
    onAction: () => {
        const filename = gantt.project.taskStore.first && gantt.project.taskStore.first.name;
        gantt.features.excelExporter.export({
            filename
        });
    }
});

new Button({
    type: 'button',
    text: 'Save',
    ref: 'saveBtn',
    color: 'b-blue b-raised',
    insertFirst: document.getElementById('tools') || document.body,
    onAction: () => {
        debugger
        function dtaa() {
            this.TaskData = null;
            this.DepData = null;
            // this.con = null;
            //this.con = null;
        }
        var dtaaa = new dtaa();
        var taskDataChanged = '';
        var dependencies = '';
        $(gantt.data).each(function (index, item) {
            debugger
            if (item.isModified) {
                if (Object.keys(item.modificationData).length > 1) {
                    if (item.id.indexOf("_generated") < 0)//Updated
                        taskDataChanged += JSON.stringify(item.modificationData) + ',';
                    else//Add new
                    {
                        var newItem = item.data;
                        // remove id
                        newItem.id = 0;
                        newItem.calendar.id = 0;
                        taskDataChanged += JSON.stringify(newItem) + ',';
                    }
                }
            }

            if (item.allDependencies.length > 0) {

                $(item.allDependencies).each(function (idx, de) {
                    var str1 = '';
                    str1 += '{"from":' + de.from + ',"to":' + de.to + ',"type":' + de.data.type;
                    if (de.data.lag != null && de.data.lag != '') str1 += ',"lag":' + de.data.lag;
                    if (de.data.lagUnit != null && de.data.lagUnit != '') str1 += ',"lagUnit":"' + de.data.lagUnit + '"';
                    str1 += '}';
                    dependencies += str1 + ',';
                });
            }
        });
        if (taskDataChanged.length > 0) {
            taskDataChanged = '[' + taskDataChanged.substring(0, taskDataChanged.length - 1) + ']';
        }
        if (dependencies.length > 0) {
            dependencies = '[' + dependencies.substring(0, dependencies.length - 1) + ']';
        }

        dtaaa.TaskData = taskDataChanged;
        dtaaa.DepData = dependencies;
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "/GanttCrud/GanttChartModifyData",
            data: JSON.stringify(dtaaa),
            dataType: "json",
            async: true,
            success: function (data) {
                //response($.parseJSON(data.d));
                if(data.success) project.load();
                return false;
            },
            error: function (result) {
                alert("errorr");
                return false;
                //noty({ text: 'Due to unexpected errors we were unable to load vendor name', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
            }
        });
    }
});

/**
 * @module GanttToolbar
 */

/**
 * @extends Core/widget/Toolbar
 */
class GanttToolbar extends Toolbar {

    static get $name() {
        return 'GanttToolbar';
    }

    construct(config) {
        const
            me      = this,
            gantt   = me.gantt = me.owner = config.gantt,
            project = gantt.project;

        project.on({
            load                : me.updateStartDateField,
            propagationComplete : me.updateStartDateField,
            thisObj             : me
        });

        const stm = project.stm;

        stm.on({
            recordingstop : me.updateUndoRedoButtons,
            restoringstop : me.updateUndoRedoButtons,
            queueReset    : me.updateUndoRedoButtons,
            thisObj       : me
        });

        super.construct(config);

        me.styleNode = document.createElement('style');
        document.head.appendChild(me.styleNode);
    }

    static get defaultConfig() {
        return {
            items : [
                {
                    type  : 'buttonGroup',
                    items : [
                        {
                            type     : 'button',
                            color    : 'b-green',
                            ref      : 'addTaskButton',
                            icon     : 'b-fa b-fa-plus',
                            text     : 'Create',
                            tooltip  : 'Create new task',
                            onAction : 'up.onAddTaskClick'
                        }
                    ]
                },
                {
                    type  : 'buttonGroup',
                    items : [
                        {
                            type     : 'button',
                            color    : 'b-blue',
                            ref      : 'editTaskButton',
                            icon     : 'b-fa b-fa-pen',
                            text     : 'Edit',
                            tooltip  : 'Edit selected task',
                            onAction : 'up.onEditTaskClick'
                        },
                        {
                            type     : 'button',
                            color    : 'b-blue',
                            ref      : 'undoBtn',
                            icon     : 'b-icon b-fa b-fa-undo',
                            tooltip  : 'Undo',
                            disabled : true,
                            width    : '2em',
                            onAction : 'up.onUndoClick'
                        },
                        {
                            type     : 'button',
                            color    : 'b-blue',
                            ref      : 'redoBtn',
                            icon     : 'b-icon b-fa b-fa-redo',
                            tooltip  : 'Redo',
                            disabled : true,
                            width    : '2em',
                            onAction : 'up.onRedoClick'
                        }
                    ]
                },
                {
                    type  : 'buttonGroup',
                    items : [
                        {
                            type     : 'button',
                            color    : 'b-blue',
                            ref      : 'expandAllButton',
                            icon     : 'b-fa b-fa-angle-double-down',
                            tooltip  : 'Expand all',
                            onAction : 'up.onExpandAllClick'
                        },
                        {
                            type     : 'button',
                            color    : 'b-blue',
                            ref      : 'collapseAllButton',
                            icon     : 'b-fa b-fa-angle-double-up',
                            tooltip  : 'Collapse all',
                            onAction : 'up.onCollapseAllClick'
                        }
                    ]
                },
                {
                    type  : 'buttonGroup',
                    items : [
                        {
                            type     : 'button',
                            color    : 'b-blue',
                            ref      : 'zoomInButton',
                            icon     : 'b-fa b-fa-search-plus',
                            tooltip  : 'Zoom in',
                            onAction : 'up.onZoomInClick'
                        },
                        {
                            type     : 'button',
                            color    : 'b-blue',
                            ref      : 'zoomOutButton',
                            icon     : 'b-fa b-fa-search-minus',
                            tooltip  : 'Zoom out',
                            onAction : 'up.onZoomOutClick'
                        },
                        {
                            type     : 'button',
                            color    : 'b-blue',
                            ref      : 'zoomToFitButton',
                            icon     : 'b-fa b-fa-compress-arrows-alt',
                            tooltip  : 'Zoom to fit',
                            onAction : 'up.onZoomToFitClick'
                        },
                        {
                            type     : 'button',
                            color    : 'b-blue',
                            ref      : 'previousButton',
                            icon     : 'b-fa b-fa-angle-left',
                            tooltip  : 'Previous time span',
                            onAction : 'up.onShiftPreviousClick'
                        },
                        {
                            type     : 'button',
                            color    : 'b-blue',
                            ref      : 'nextButton',
                            icon     : 'b-fa b-fa-angle-right',
                            tooltip  : 'Next time span',
                            onAction : 'up.onShiftNextClick'
                        }
                    ]
                },
                {
                    type  : 'buttonGroup',
                    items : [
                        {
                            type       : 'button',
                            color      : 'b-blue',
                            ref        : 'featuresButton',
                            icon       : 'b-fa b-fa-tasks',
                            text       : 'Features',
                            tooltip    : 'Toggle features',
                            toggleable : true,
                            menu       : {
                                onItem       : 'up.onFeaturesClick',
                                onBeforeShow : 'up.onFeaturesShow',
                                items        : [
                                    {
                                        text    : 'Draw dependencies',
                                        feature : 'dependencies',
                                        checked : false
                                    },
                                    {
                                        text    : 'Task labels',
                                        feature : 'labels',
                                        checked : false
                                    },
                                    {
                                        text    : 'Project lines',
                                        feature : 'projectLines',
                                        checked : false
                                    },
                                    {
                                        text    : 'Highlight non-working time',
                                        feature : 'nonWorkingTime',
                                        checked : false
                                    },
                                    {
                                        text    : 'Enable cell editing',
                                        feature : 'cellEdit',
                                        checked : false
                                    },
                                    {
                                        text    : 'Show baselines',
                                        feature : 'baselines',
                                        checked : false
                                    },
                                    {
                                        text    : 'Show rollups',
                                        feature : 'rollups',
                                        checked : false
                                    },
                                    {
                                        text    : 'Show progress line',
                                        feature : 'progressLine',
                                        checked : false
                                    },
                                    {
                                        text    : 'Hide schedule',
                                        cls     : 'b-separator',
                                        subGrid : 'normal',
                                        checked : false
                                    }
                                ]
                            }
                        },
                        {
                            type       : 'button',
                            color      : 'b-blue',
                            ref        : 'settingsButton',
                            icon       : 'b-fa b-fa-cogs',
                            text       : 'Settings',
                            tooltip    : 'Adjust settings',
                            toggleable : true,
                            menu       : {
                                type        : 'popup',
                                anchor      : true,
                                layoutStyle : {
                                    flexDirection : 'column'
                                },
                                onBeforeShow : 'up.onSettingsShow',

                                items : [
                                    {
                                        type      : 'slider',
                                        ref       : 'rowHeight',
                                        text      : 'Row height',
                                        width     : '12em',
                                        showValue : true,
                                        min       : 30,
                                        max       : 70,
                                        style     : 'margin-bottom: .5em',
                                        onInput   : 'up.onSettingsRowHeightChange'
                                    },
                                    {
                                        type      : 'slider',
                                        ref       : 'barMargin',
                                        text      : 'Bar margin',
                                        width     : '12em',
                                        showValue : true,
                                        min       : 0,
                                        max       : 10,
                                        onInput   : 'up.onSettingsMarginChange'
                                    },
                                    {
                                        type      : 'slider',
                                        ref       : 'duration',
                                        text      : 'Animation duration ',
                                        width     : '12em',
                                        min       : 0,
                                        max       : 2000,
                                        step      : 100,
                                        showValue : true,
                                        onInput   : 'up.onSettingsDurationChange'
                                    }
                                ]
                            }
                        },
                        {
                            type       : 'button',
                            color      : 'b-blue',
                            ref        : 'criticalPathsButton',
                            icon       : 'b-fa b-fa-fire',
                            text       : 'Critical paths',
                            tooltip    : 'Highlight critical paths',
                            toggleable : true,
                            onAction   : 'up.onCriticalPathsClick'
                        }
                    ]
                },
                {
                    type      : 'datefield',
                    ref       : 'startDateField',
                    label     : 'Project start',
                    // required  : true, (done on load)
                    flex      : '1 2 17em',
                    listeners : {
                        change : 'up.onStartDateChange'
                    }
                },
                {
                    type                 : 'textfield',
                    ref                  : 'filterByName',
                    cls                  : 'filter-by-name',
                    flex                 : '1 1 12.5em',
                    // Label used for material, hidden in other themes
                    label                : 'Find tasks by name',
                    // Placeholder for others
                    placeholder          : 'Find tasks by name',
                    clearable            : true,
                    keyStrokeChangeDelay : 100,
                    triggers             : {
                        filter : {
                            align : 'end',
                            cls   : 'b-fa b-fa-filter'
                        }
                    },
                    onChange : 'up.onFilterChange'
                }
            ]
        };
    }

    updateUndoRedoButtons() {
        const
            { stm }              = this.gantt.project,
            { undoBtn, redoBtn } = this.widgetMap,
            redoCount            = stm.length - stm.position;

        undoBtn.badge = stm.position || '';
        redoBtn.badge = redoCount || '';

        undoBtn.disabled = !stm.canUndo;
        redoBtn.disabled = !stm.canRedo;
    }

    setAnimationDuration(value) {
        const
            me      = this,
            cssText = `.b-animating .b-gantt-task-wrap { transition-duration: ${value / 1000}s !important; }`;

        me.gantt.transitionDuration = value;

        if (me.transitionRule) {
            me.transitionRule.cssText = cssText;
        }
        else {
            me.transitionRule = CSSHelper.insertRule(cssText);
        }
    }

    updateStartDateField() {
        const startDateField = this.widgetMap.startDateField;

        startDateField.value = this.gantt.project.startDate;

        // This handler is called on project.load/propagationComplete, so now we have the
        // initial start date. Prior to this time, the empty (default) value would be
        // flagged as invalid.
        startDateField.required = true;
    }

    // region controller methods

    async onAddTaskClick() {
        const
            { gantt } = this,
            added = gantt.taskStore.rootNode.appendChild({ name : 'New task', duration : 1 });

        // run propagation to calculate new task fields
        await gantt.project.propagate();

        // scroll to the added task
        await gantt.scrollRowIntoView(added);
        debugger
        gantt.features.cellEdit.startEditing({
            record : added,
            field  : 'name'
        });
    }

    onEditTaskClick() {
        const { gantt } = this;
        if (gantt.selectedRecord) {
            gantt.editTask(gantt.selectedRecord);
            debugger
        }
        else {
            Toast.show('First select the task you want to edit');
        }
    }

    onExpandAllClick() {
        this.gantt.expandAll();
    }

    onCollapseAllClick() {
        this.gantt.collapseAll();
    }

    onZoomInClick() {
        this.gantt.zoomIn();
    }

    onZoomOutClick() {
        this.gantt.zoomOut();
    }

    onZoomToFitClick() {
        this.gantt.zoomToFit({
            leftMargin  : 50,
            rightMargin : 50
        });
    }

    onShiftPreviousClick() {
        this.gantt.shiftPrevious();
    }

    onShiftNextClick() {
        this.gantt.shiftNext();
    }

    onStartDateChange({ value, oldValue }) {
        if (!oldValue) { // ignore initial set
            return;
        }

        this.gantt.startDate = DateHelper.add(value, -1, 'week');

        this.gantt.project.setStartDate(value);
    }

    onFilterChange({ value }) {
        if (value === '') {
            this.gantt.taskStore.clearFilters();
        }
        else {
            value = value.replace(/[.*+?^${}()|[\]\\]/g, '\\$&');

            this.gantt.taskStore.filter(task => task.name && task.name.match(new RegExp(value, 'i')));
        }
    }

    onFeaturesClick({ source : item }) {
        const { gantt } = this;

        if (item.feature) {
            const feature = gantt.features[item.feature];
            feature.disabled = !feature.disabled;
        }
        else if (item.subGrid) {
            const subGrid = gantt.subGrids[item.subGrid];
            subGrid.collapsed = !subGrid.collapsed;
        }
    }

    onFeaturesShow({ source : menu }) {
        const { gantt } = this;

        menu.items.map(item => {
            const { feature } = item;

            if (feature) {
                // a feature might be not presented in the gantt
                // (the code is shared between "advanced" and "php" demos which use a bit different set of features)
                if (gantt.features[feature]) {
                    item.checked = !gantt.features[feature].disabled;
                }
                // hide not existing features
                else {
                    item.hide();
                }
            }
            else {
                item.checked = gantt.subGrids[item.subGrid].collapsed;
            }
        });
    }

    onSettingsShow({ source : menu }) {
        const { gantt } = this,
            { widgetMap } = menu;

        widgetMap.rowHeight.value = gantt.rowHeight;
        widgetMap.barMargin.value = gantt.barMargin;
        widgetMap.barMargin.max = (gantt.rowHeight / 2) - 5;
        widgetMap.duration.value = gantt.transitionDuration;
    }

    onSettingsRowHeightChange({ value }) {
        this.gantt.rowHeight = value;
        this.widgetMap.settingsButton.menu.widgetMap.barMargin.max = (value / 2) - 5;
    }

    onSettingsMarginChange({ value }) {
        this.gantt.barMargin = value;
    }

    onSettingsDurationChange({ value }) {
        this.gantt.transitionDuration = value;
        this.styleNode.innerHTML = `.b-animating .b-gantt-task-wrap { transition-duration: ${value / 1000}s !important; }`;
    };

    onCriticalPathsClick({ source }) {
        this.gantt.features.criticalPaths.disabled = !source.pressed;
    }

    onUndoClick() {
        this.gantt.project.stm.canUndo && this.gantt.project.stm.undo();
    }

    onRedoClick() {
        this.gantt.project.stm.canRedo && this.gantt.project.stm.redo();
    }

    // endregion
};


/**
 * @module PercentDonePieColumn
 */

/**
 * A column drawing a pie chart of the `percentDone` value
 *
 * @extends Gantt/column/PercentDoneColumn
 * @classType percentdonepie
 */
class PercentDonePieColumn extends PercentDoneColumn {
    static get type() {
        return 'percentdonepie';
    }

    static get isGanttColumn() {
        return true;
    }

    static get defaults() {
        return {
            // Set your default instance config properties here
            htmlEncode : false,
            align      : 'center'
        };
    }

    //endregion

    renderer({ value }) {
        return value ? {
            className : 'b-pie',
            style     : {
                animationDelay : `-${value}s`
            }
        } : '';
    }
}

ColumnStore.registerColumnType(PercentDonePieColumn);


/**
 * @module ResourceAvatarColumn
 */

const defaultImagePath = '../_shared/images/users/';

/**
 * Column showing avatars of the assigned resource
 *
 * @extends Grid/column/Column
 * @classType resourceavatar
 */
class ResourceAvatarColumn extends ResourceAssignmentColumn {
    static get type() {
        return 'resourceavatar';
    }

    static get isGanttColumn() {
        return true;
    }

    static get fields() {
        return [
            /**
             * The path for the resource images
             * @config {String}
             * @category Common
             */
            'imageFolderPath'
        ];
    }

    static get defaults() {
        return {
            repaintOnResize : true,
            htmlEncode      : false,
            cellCls         : 'b-resource-avatar-cell',
            imageFolderPath : defaultImagePath,
            autoSyncHtml    : true
        };
    }

    renderer({ value }) {
        const
            imgSize = 30,
            nbrVisible = Math.floor((this.width - 20) / (imgSize + 2)),
            lastIndex = nbrVisible - 1,
            overflowCount = value.length - nbrVisible;

        return '<div class="b-resource-avatar-container">' + Array.from(value).map((assignment, i) => {
            const { resource } = assignment;

            let markup = '';

            if (resource && i < nbrVisible) {
                const
                    isLastOverflowing = i === lastIndex && overflowCount > 0,
                    imgBtip = `${resource.name} ${assignment.units}%${isLastOverflowing ? ` (+${overflowCount} more resources)` : ''}`,
                    imgMarkup = `<img data-btip-scroll-action="realign" data-btip="${imgBtip}" class="b-resource-avatar" src="${this.imageFolderPath}${resource.name.toLowerCase() || 'none'}.jpg">`;

                if (isLastOverflowing) {
                    markup = `<div class="b-overflow-img">${imgMarkup}<span class="b-overflow-count">+${overflowCount}</span></div>`;
                }
                else {
                    markup = imgMarkup;
                }
            }

            return markup;
        }).join('') + '</div>';
    }
}

ColumnStore.registerColumnType(ResourceAvatarColumn);


/**
 * @module StatusColumn
 */

/**
 * A column showing the status of a task
 *
 * @extends Gantt/column/Column
 * @classType statuscolumn
 */
class StatusColumn extends Column {
    static get type() {
        return 'statuscolumn';
    }

    static get isGanttColumn() {
        return true;
    }

    static get defaults() {
        return {
            // Set your default instance config properties here
            text       : 'Status',
            editor     : false,
            cellCls    : 'b-status-column-cell',
            htmlEncode : false
        };
    }

    //endregion

    renderer({ record }) {
        let status = '';

        if (record.isCompleted) {
            status = 'Completed';
        }
        else if (record.endDate < Date.now()) {
            status = 'Late';
        }
        else if (record.isStarted) {
            status = 'Started';
        }

        return status ? {
            tag       : 'i',
            className : `b-fa b-fa-circle ${status}`,
            html      : status
        } : '';
    }
}

ColumnStore.registerColumnType(StatusColumn);


// here you can extend our default Task class with your additional fields, methods and logic
class Task extends TaskModel {

    static get fields() {
        return [
            { name : 'deadline', type : 'date' }
        ];
    }

    get isLate() {
        return this.deadline && Date.now() > this.deadline;
    }
}
/* eslint-disable no-unused-vars */
//var 
const project = new ProjectModel({

    // Let the Project know we want to use our own Task model with custom fields / methods
    taskModelClass : Task,
    transport: {
        //load: {
        //    url: '../_datasets/launch-saas.json'
        //}
        load: {
            //url: '/GanttCrud/GetGanttChartDataStaticJSON'
            url: '/GanttCrud/GetGanttChartData'
            //url: '../_datasets/launch-saas.json'
        }
        //, sync: {
        //    requestConfig: {
        //        url: '/GanttChart/GanttChartModifyData',
        //        method: 'POST',
        //        // get rid of cache-buster parameter
        //        disableCaching: false,
        //        params: {
        //            TaskData : 'dfdfdf'
        //        }
                
        //    }
        //}
    },
    autoSync: false
});

const gantt = new Gantt({
    project : project,

    //startDate : '2019-01-12',
    //endDate   : '2019-03-24',

    columns : [
        { type : 'wbs' },
        { type : 'name', width : 250 },
        { type : 'startdate' },
        { type : 'duration' },
        { type : 'resourceavatar', width : 120 },
        { type : 'percentdone', showCircle : true, width : 70 },
        {
            type  : 'predecessor',
            width : 112
        },
        {
            type  : 'successor',
            width : 112
        },
        { type : 'schedulingmodecolumn' },
        { type : 'calendar' },
        { type : 'constrainttype' },
        { type : 'constraintdate' },
        { type : 'statuscolumn' },
        {
            type  : 'date',
            text  : 'Deadline',
            field : 'deadline'
        },
        { type : 'addnew' }
    ],

    subGridConfigs : {
        locked : {
            flex : 3
        },
        normal : {
            flex : 4
        }
    },

    columnLines : false,

    features: {
        excelExporter: {
            // Choose the date format for date fields
            dateFormat: 'YYYY-MM-DD HH:mm'
        },
        rollups : {
            disabled : true
        },
        baselines : {
            disabled : true
        },
        progressLine : {
            disabled   : true,
            statusDate : new Date(2019, 0, 25)
        },
        taskContextMenu : {
            // Our items is merged with the provided defaultItems
            // So we add the provided convertToMilestone option.
            items : {
                convertToMilestone : true
            },
            processItems({ taskRecord, items }) {
                if (taskRecord.isMilestone) {
                    items.convertToMilestone = false;
                }
            }
        },
        filter         : true,
        dependencyEdit : true,
        timeRanges     : {
            showCurrentTimeLine : true
        },
        labels : {
            left : {
                field  : 'name',
                editor : {
                    type : 'textfield'
                }
            }
        }
    }
});

const panel = new Panel({
    adopt  : 'container',
    layout : 'fit',
    items  : [
        gantt
    ],
    tbar : new GanttToolbar({ gantt })
});

// console.time("load data");
project.load().then(() => {
    // console.timeEnd("load data");
    const stm = gantt.project.stm;
    debugger
    stm.enable();
    stm.autoRecord = true;

    // let's track scheduling conflicts happened
    project.on('schedulingconflict', context => {
        // show notification to user
        Toast.show('Scheduling conflict has happened ..recent changes were reverted');
        // as the conflict resolution approach let's simply cancel the changes
        context.continueWithResolutionResult(EffectResolutionResult.Cancel);
    });

    project.on('beforesync', function () {
        // cannot persist changes before at least one record is added
        // to the `someStore` store
        //alert("beforesync");
        debugger
        function dtaa() {
            this.TaskData = null;
            // this.con = null;
            //this.con = null;
        }
        var dtaaa = new dtaa();
        var taskDataChanged = '';
        $(gantt.data).each(function (index, item) {
            if (item.modificationData != null) {
                taskDataChanged += JSON.stringify(item.modificationData);
            }
        });

        dtaaa.TaskData = taskDataChanged;
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "/GanttCrud/GanttChartModifyData",
            data: JSON.stringify(dtaaa),
            dataType: "json",
            async: true,
            success: function (data) {
                //response($.parseJSON(data.d));
                return false;
            },
            error: function (result) {
                alert("errorr");
                return false;
                //noty({ text: 'Due to unexpected errors we were unable to load vendor name', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
            }
        });
        
        //if (!.getCount()) return false;
    });
    
});
//project.transport.sync(() => {
//    alert("sync");
//});
//project.sync().then(() => {
//    alert("sync");
//});