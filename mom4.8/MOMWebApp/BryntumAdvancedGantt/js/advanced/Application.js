Ext.define('Gnt.examples.advanced.Application', {
    extend: 'Ext.app.Application',

    mixins: ['Gnt.mixin.Localizable'],

    requires: [
        'Sch.app.CrudManagerDomain',
        'Gnt.examples.advanced.locale.En',
        'Gnt.examples.advanced.store.Calendars',
        'Gnt.examples.advanced.store.Tasks',
        'Gnt.examples.advanced.crud.CrudManager',
        'Gnt.data.undoredo.Manager',
        'Ext.window.MessageBox'
    ],

    namespaces: 'Gnt.examples.advanced',

    paths: {
        'Gnt.locale': './js/Gnt/locale',
        'Sch.locale': './js/Sch/locale'
    },

    stores: [
        'Locales'
    ],

    views: [
        'MainViewport'
    ],

    routes: {
        ':lc': {
            before: 'onBeforeLocaleEstablished',
            action: 'onLocaleEstablished'
        }
    },

    defaultToken: 'en',

    listen: {
        // Right now we just listen to locale-change on controllers domain, any controller fired that event might
        // initiate a locale change procedure
        controller: {
            '*': {
                'locale-change': 'onLocaleChange'
            }
        },

        crudmanager: {
            'advanced-crudmanager': {
                'loadfail': 'onCrudError',
                'syncfail': 'onCrudError'
            }
        }
    },

    glyphFontFamily: 'FontAwesome',

    mainView: null,

    crudManager: null,

    undoManager: null,

    currentLocale: 'en',

    /**
     * Mapping for the startDate config of the gantt panel
     */
    startDate: null,

    /**
     * Mapping for the endDate config of the gantt panel
     */
    endDate: null,

    constructor: function (config) {
        var me = this;

        me.crudManager = new Gnt.examples.advanced.crud.CrudManager({
            taskStore: new Gnt.examples.advanced.store.Tasks({
                calendarManager: new Gnt.examples.advanced.store.Calendars()
            })
        });

        // Creating undo/redo manager
        me.undoManager = new Gnt.data.undoredo.Manager({
            transactionBoundary: 'timeout',
            stores: [
                me.crudManager.getCalendarManager(),
                me.crudManager.getTaskStore(),
                me.crudManager.getResourceStore(),
                me.crudManager.getAssignmentStore(),
                me.crudManager.getDependencyStore()
            ]
        });

        this.callParent(arguments);
    },

    /**
     * This method will be called on CRUD manager exception and display a message box with error details.
     */
    onCrudError: function (crud, response, responseOptions) {
        Ext.Msg.show({
            title: this.L('error'),
            msg: response && response.message || this.L('requestError'),
            icon: Ext.Msg.ERROR,
            buttons: Ext.Msg.OK,
            minWidth: Ext.Msg.minWidth
        });
    },

    /**
     * When we've got a request to change locale we simply use redirectTo() for locale changing route handlers
     * fired, which in their turn know how to properly change locale.
     */
    onLocaleChange: function (lc, lcRecord) {
        this.redirectTo(lc);
    },

    /**
     * This method will be executed upon location has change and upon application startup with default token in case
     * location hash is empty. This method is called *before* corresponding route change action handler, and it's
     * cappable of stopping/resument the switch action, thus we use it to load locale required script files.
     */
    onBeforeLocaleEstablished: function (lc, action) {
        var me = this,
            lcRecord = me.getLocalesStore().getById(lc);

        switch (true) {
            case lcRecord && !me.mainView && me.currentLocale != lc:

                Ext.Loader.loadScript({
                    // load Ext JS locale for the chosen language
                    url: 'https://www.bryntum.com/examples/extjs-6.5.0/build/classic/locale/locale-' + lc + '.js',
                    onLoad: function () {
                        var cls = lcRecord.get('cls');
                        // load the gantt locale for the chosen language
                        Ext.require('Gnt.examples.advanced.locale.' + cls, function () {
                            // Some of Ext JS localization wrapped with Ext.onReady(...)
                            // so we have to do the same to instantiate UI after Ext JS localization is applied
                            Ext.onReady(function () { action.resume(); });
                        });
                    }
                });

                break;

            case lcRecord && !me.mainView && me.currentLocale == lc:

                action.resume();
                break;

            case lcRecord && me.mainView && true:

                // Main view is already created thus we have to execute hard reload otherwise locale related
                // scripts won't be properly applied.
                me.deactivate();
                action.stop();
                window.location.hash = '#' + lc;
                window.location.reload(true);
                break;

            default:
                action.stop();
        }
    },

    /**
     * Since we are supporting such locale management we can't use application's autoCreateViewport option, since
     * we have to load all required locale JS files before any GUI creation. Loading is done in the 'before' handler,
     * so here in 'action' handler we are ready to create our main view.
     */
    onLocaleEstablished: function (lc) {
        var me = this,
            crud = me.crudManager,
            undo = me.undoManager;

        me.currentLocale = lc;

        me.mainView = me.getMainViewportView().create({
            //renderTo: 'bryntum_planner_gantt_chart',
            //height: 500,
            //width: 1200,
            viewModel: {
                type: 'advanced-viewport',
                data: {
                    crud: crud,
                    undoManager: undo,
                    taskStore: crud.getTaskStore(),
                    calendarManager: crud.getCalendarManager(),
                    currentLocale: me.currentLocale,
                    availableLocales: me.getLocalesStore()
                }
            },
            crudManager: crud,
            undoManager: undo,
            startDate: me.startDate,
            endDate: me.endDate
        });
    }
});
