Ext.define('Gnt.examples.advanced.model.Project', {
    extend  : 'Gnt.model.Project',

    fields  : [
        { name : 'index', type : 'int', persist : true },
        { name : 'expanded', type : 'bool', persist : true },
        { name : 'Color', type : 'string' }
    ]
});
