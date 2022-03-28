Ext.define('Gnt.examples.advanced.locale.It', {
    extend    : 'Sch.locale.Locale',
    requires  : 'Gnt.locale.It',
    singleton : true,

    l10n : {
        'Gnt.examples.advanced.Application' : {
            error           : 'Error',
            requestError    : 'Request error'
        },

        'Gnt.examples.advanced.view.ControlHeader' : {
            previousTimespan        : 'Periodo precedente',
            nextTimespan            : 'Prossimo periodo',
            collapseAll             : 'Chiudi tutto',
            expandAll               : 'Espandi tutto',
            zoomOut                 : 'Ridurre',
            zoomIn                  : 'Ingrandire',
            zoomToFit               : 'Zoom per adattarsi',
            undo                    : 'Annulla',
            redo                    : 'Ripeti',
            viewFullScreen          : 'A schermo intero',
            highlightCriticalPath   : 'Visualizzazione del percorso critico',
            addNewTask              : 'Aggiungere attività',
            newTask                 : 'Nuova attività',
            removeSelectedTasks     : 'Cancellare attività selezionate',
            indent                  : 'Imposta',
            outdent                 : 'Imposta come livello di struttura superiore.',
            manageCalendars         : 'Gestire i calendari',
            saveChanges             : 'Salvare',
            language                : 'Lingua: ',
            selectLanguage          : 'Scegli una lingua...',
            tryMore                 : 'Prova più funzioni...',
            print                   : 'Stampa'
        },

        'Gnt.examples.advanced.plugin.TaskContextMenu' : {
            changeTaskColor         : 'Cambia colore'
        },

        'Gnt.examples.advanced.view.GanttSecondaryToolbar' : {
            toggleChildTasksGrouping        : 'Toggle child tasks grouping on/off',
            toggleRollupTasks               : 'Toggle rollup tasks',
            highlightTasksLonger8           : 'Highlight tasks longer than 8 days',
            filterTasksWithProgressLess30   : 'Filter: Tasks with progress < 30%',
            clearFilter                     : 'Clear Filter',
            scrollToLastTask                : 'Scroll to last task',
            toggleProgressLine              : 'Toggle progress line on/off'
        }
    },

    apply : function (classNames) {
        Gnt.locale.It.apply(classNames);
        this.callParent(arguments);
    }

});
