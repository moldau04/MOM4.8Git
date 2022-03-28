Ext.define('Gnt.examples.advanced.view.Timeline', {
    extend   : 'Gnt.panel.Timeline',
    xtype    : 'advanced-timeline',
    requires : ['Gnt.examples.advanced.view.ControlHeader'],

    //split  : true,
    border : false,

    header   : {
        xtype : 'controlheader'
    }
});
