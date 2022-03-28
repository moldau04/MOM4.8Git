


var getQueryString = function (field, url) {
    var href = url ? url : window.location.href;
    var reg = new RegExp('[?&]' + field + '=([^&#]*)', 'i');
    var string = reg.exec(href);
    return string ? string[1] : null;
};

var _title = getQueryString('uid');

Ext.define('Gnt.examples.advanced.view.ControlHeader', {
    extend: 'Ext.panel.Header',
    xtype: 'controlheader',

    mixins: ['Gnt.mixin.Localizable'],
    requires: [
        'Ext.form.field.ComboBox'
    ],
    title: "Edit Planner for Project #" + _title,

    //split  : true,
    border: '0 0 1 0',

    initComponent: function () {
        this.tools = [
            {
                tooltip: this.L('previousTimespan'),
                reference: 'shiftPrevious',
                cls: 'icon-previous'
            },
            {
                tooltip: this.L('nextTimespan'),
                reference: 'shiftNext',
                cls: 'icon-next'
            },
            {
                tooltip: this.L('collapseAll'),
                reference: 'collapseAll',
                cls: 'icon-collapse-all',
                bind: {
                    disabled: '{filterSet}'
                }
            },
            {
                tooltip: this.L('expandAll'),
                reference: 'expandAll',
                cls: 'icon-expand-all',
                bind: {
                    disabled: '{filterSet}'
                }
            },
            {
                tooltip: this.L('zoomOut'),
                reference: 'zoomOut',
                cls: 'icon-zoom-out'
            },
            {
                tooltip: this.L('zoomIn'),
                reference: 'zoomIn',
                cls: 'icon-zoom-in'
            },
            {
                tooltip: this.L('zoomToFit'),
                reference: 'zoomToFit',
                cls: 'icon-zoom-to-fit'
            },
            {
                tooltip: this.L('undo'),
                reference: 'undo',
                cls: 'icon-undo',
                bind: {
                    disabled: '{!canUndo}'
                }
            },
            {
                tooltip: this.L('redo'),
                reference: 'redo',
                cls: 'icon-redo',
                bind: {
                    disabled: '{!canRedo}'
                }
            },
            {
                tooltip: this.L('viewFullScreen'),
                reference: 'viewFullScreen',
                cls: 'icon-fullscreen',
                bind: {
                    hidden: '{!fullscreenEnabled}'
                }
            },
            {
                tooltip: this.L('highlightCriticalPath'),
                reference: 'criticalPath',
                cls: 'icon-critical-path'
            },
            {
                tooltip: this.L('addNewTask'),
                reference: 'addTask',
                cls: 'icon-add-task'
            },
            {
                tooltip: this.L('removeSelectedTasks'),
                reference: 'removeSelected',
                cls: 'icon-remove-task',
                bind: {
                    disabled: '{!hasSelection}'
                }
            },
            {
                tooltip: this.L('indent'),
                reference: 'indentTask',
                cls: 'icon-indent',
                bind: {
                    disabled: '{!allowIndentationChange}'
                }
            },
            {
                tooltip: this.L('outdent'),
                reference: 'outdentTask',
                cls: 'icon-outdent',
                bind: {
                    disabled: '{!allowIndentationChange}'
                }
            },
            {
                tooltip: this.L('manageCalendars'),
                reference: 'manageCalendars',
                cls: 'icon-calendar',
                bind: {
                    hidden: '{!calendarManager}'
                }
            },
            {
                tooltip: this.L('saveChanges'),
                reference: 'saveChanges',
                cls: 'icon-save',
                bind: {
                    hidden: '{!crudPersistable}',
                    disabled: '{!hasChanges}'
                }
            },
            {
                tooltip: this.L('tryMore'),
                reference: 'tryMore',
                cls: 'icon-settings'
            }
            //{
            //    xtype         : 'combo',
            //    reference     : 'langSelector',
            //    margin        : '0 10',
            //    width         : 120,
            //    bind          : {
            //        store : '{availableLocales}',
            //        value : '{currentLocale}'
            //    },
            //    editable      : false,
            //    displayField  : 'title',
            //    valueField    : 'id',
            //    mode          : 'local',
            //    triggerAction : 'all',
            //    emptyText     : this.L('selectLanguage')
            //}
        ];

        Ext.Array.forEach(this.tools, function (cmp) {
            if (cmp.reference) cmp.itemId = cmp.reference;
        });

        this.callParent(arguments);
    }
});
