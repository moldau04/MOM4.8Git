Ext.define('Gnt.examples.advanced.store.Locales', {
    extend  : 'Ext.data.Store',
    alias   : 'store.advanced-locales-store',
    storeId : 'locales',
    fields  : ['id', 'cls', 'title'],
    proxy   : 'memory',
    data    : [
        { id : 'en',    cls : 'En',   title : 'English' },
        { id : 'sv_SE', cls : 'SvSE', title : 'Swedish' },
        { id : 'de',    cls : 'De',   title : 'German'  },
        { id : 'fr',    cls : 'Fr',   title : 'French'  },
        { id : 'it',    cls : 'It',   title : 'Italian' },
        { id : 'ru',    cls : 'RuRU', title : 'Russian' },
        { id : 'pl',    cls : 'Pl',   title : 'Polish'  },
        { id : 'nl',    cls : 'Nl',   title : 'Dutch'   }
    ]
});
