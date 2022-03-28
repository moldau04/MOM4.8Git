// extend standard TaskForm class
Ext.define('Gnt.examples.advanced.view.TaskEditorGeneralForm', {
    extend : 'Gnt.widget.taskeditor.TaskForm',

    constructor : function(config) {
        this.callParent(arguments);

        // add a custom field
        this.add({
            fieldLabel  : 'Custom field',
            name        : 'CustomField'
        });
    }
});
