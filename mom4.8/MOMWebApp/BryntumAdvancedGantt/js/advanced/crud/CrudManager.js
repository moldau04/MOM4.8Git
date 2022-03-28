Ext.define('Gnt.examples.advanced.crud.CrudManager', {
    extend: 'Gnt.data.CrudManager',
    alias: 'crudmanager.advanced-crudmanager',
    autoLoad: true,
    transport: {
        load: {
            method: 'GET',
            paramName: 'q',
           // url: 'api/Bryntum/Load'
            url: 'BryntumAdvanceGanttChart.asmx/Load'
           
          
        },
        sync: {
            method: 'POST',
            //url: 'ganttcrud/sync'
            url: 'BryntumAdvanceGanttChart.asmx/Sync'
        }
    }
});
