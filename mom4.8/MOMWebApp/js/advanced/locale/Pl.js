Ext.define('Gnt.examples.advanced.locale.Pl', {
    extend    : 'Sch.locale.Locale',
    requires  : 'Gnt.locale.Pl',
    singleton : true,

    l10n : {
        'Gnt.examples.advanced.Application' : {
            error           : 'Error',
            requestError    : 'Request error'
        },

        'Gnt.examples.advanced.view.ControlHeader' : {
            previousTimespan        : 'Poprzedni skumulowanie',
            nextTimespan            : 'Następny skumulowanie',
            collapseAll             : 'Zwinięcie wszystkich sekcji.',
            expandAll               : 'Pokaż wszystko',
            zoomOut                 : 'Pomniejsz',
            zoomIn                  : 'Powiększ',
            zoomToFit               : 'Pokazać wszystkie zadania',
            undo                    : 'Cofnij',
            redo                    : 'Ponów',
            viewFullScreen          : 'Pełnego ekranu',
            highlightCriticalPath   : 'Zadania krytyczne',
            addNewTask              : 'Dodać nowe zadanie',
            newTask                 : 'Nowe zadanie',
            removeSelectedTasks     : 'Usuń wybrane zadania',
            indent                  : 'Zwiększ wcięcie',
            outdent                 : 'Zmniejsz wcięcie',
            manageCalendars         : 'Kalendarze',
            saveChanges             : 'Zapisz',
            language                : 'Język:',
            selectLanguage          : 'Wybierz język...',
            tryMore                 : 'Spróbuj więcej funkcji...',
            print                   : 'Drukuj'
        },

        'Gnt.examples.advanced.plugin.TaskContextMenu' : {
            changeTaskColor         : 'Zmiana kolor'
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
        Gnt.locale.Pl.apply(classNames);
        this.callParent(arguments);
    }

});
