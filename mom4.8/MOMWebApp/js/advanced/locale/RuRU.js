Ext.define('Gnt.examples.advanced.locale.RuRU', {
    extend    : 'Sch.locale.Locale',
    requires  : 'Gnt.locale.RuRU',
    singleton : true,

    l10n : {
        'Gnt.examples.advanced.Application' : {
            error           : 'Ошибка',
            requestError    : 'Ошибка запроса'
        },

        'Gnt.examples.advanced.view.ControlHeader' : {
            previousTimespan        : 'Предыдущий интервал',
            nextTimespan            : 'Следующий интервал',
            collapseAll             : 'Свернуть все',
            expandAll               : 'Раскрыть все',
            zoomOut                 : 'Мельче',
            zoomIn                  : 'Крупнее',
            zoomToFit               : 'Масштаб по содержимому',
            undo                    : 'Отменить последнюю команду',
            redo                    : 'Вернуть последнюю команду',
            viewFullScreen          : 'Показать на весь экран',
            highlightCriticalPath   : 'Показать критические задачи',
            addNewTask              : 'Добавить задачу',
            newTask                 : 'Новая задача',
            removeSelectedTasks     : 'Удалить выделенные задачи',
            indent                  : 'Понизить уровень',
            outdent                 : 'Повысить уровень',
            manageCalendars         : 'Управление календарями',
            saveChanges             : 'Сохранить изменения',
            language                : 'Язык: ',
            selectLanguage          : 'Выберите язык...',
            tryMore                 : 'Попробовать другие опции...',
            print                   : 'Печать'
        },

        'Gnt.examples.advanced.plugin.TaskContextMenu' : {
            changeTaskColor         : 'Изменить цвет задачи'
        },

        'Gnt.examples.advanced.view.GanttSecondaryToolbar' : {
            toggleChildTasksGrouping        : 'Переключить подсветку групп',
            toggleRollupTasks               : 'Переключить сведение задач',
            highlightTasksLonger8           : 'Показать задачи длиннее 8 дней',
            filterTasksWithProgressLess30   : 'Фильтровать: Задачи с % завершения < 30%',
            clearFilter                     : 'Очистить фильтр',
            scrollToLastTask                : 'Перейти к последней задаче',
            toggleProgressLine              : 'Переключить показ линии хода выполнения'
        }
    },

    apply : function (classNames) {
        Gnt.locale.RuRU.apply(classNames);
        this.callParent(arguments);
    }

});
