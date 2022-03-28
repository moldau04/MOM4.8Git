Ext.define('Gnt.examples.advanced.locale.Fr', {
    extend    : 'Sch.locale.Locale',
    requires  : 'Gnt.locale.Fr',
    singleton : true,

    l10n : {
        'Gnt.examples.advanced.Application' : {
            error           : 'Erreur',
            requestError    : 'La requête contient une erreur'
        },

        'Gnt.examples.advanced.view.ControlHeader' : {
            previousTimespan        : 'Période précédente',
            nextTimespan            : 'Prochaine période',
            collapseAll             : 'Réduire tout',
            expandAll               : 'Développer tout',
            zoomOut                 : 'Zoom arrière',
            zoomIn                  : 'Zoom avant',
            zoomToFit               : 'Zoom pour ajuster',
            undo                    : 'Annuler',
            redo                    : 'Rétablir',
            viewFullScreen          : 'Plein écran',
            highlightCriticalPath   : 'Sélectionner le chemin critique',
            addNewTask              : 'Ajouter une nouvelle tâche',
            newTask                 : 'Nouvelle tâche',
            removeSelectedTasks     : 'Supprimer la(les) tâche(s) sélectionnées',
            indent                  : 'Augmenter',
            outdent                 : 'Diminuer',
            manageCalendars         : 'Gérer les calendriers',
            saveChanges             : 'Enregistrer les modifications',
            language                : 'Langue: ',
            selectLanguage          : 'Sélectionner une langue...',
            tryMore                 : 'Essayer plus de fonctionnalités...',
            print                   : 'Imprimer'
        },

        'Gnt.examples.advanced.plugin.TaskContextMenu' : {
            changeTaskColor         : 'Changer la couleur de la tâche'
        },

        'Gnt.examples.advanced.view.GanttSecondaryToolbar' : {
            toggleChildTasksGrouping        : 'Basculer le groupement des tâches',
            toggleRollupTasks               : 'Basculer le report',
            highlightTasksLonger8           : 'Sélectionner les tâches > 8 jours',
            filterTasksWithProgressLess30   : 'Filtrer: Tâches < 30% achévé',
            clearFilter                     : 'Effacer le filtre',
            scrollToLastTask                : 'Défiler à la dernière tâche',
            toggleProgressLine              : "Activer la ligne d'avancement"
        }

    },

    apply : function (classNames) {
        Gnt.locale.Fr.apply(classNames);
        this.callParent(arguments);
    }
});