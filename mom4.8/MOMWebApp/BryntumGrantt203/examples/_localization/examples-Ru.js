import LocaleManager from '../../lib/Core/localization/LocaleManager.js';
import '../../lib/Gantt/localization/Ru.js';

const examplesLocale = {
    extends : 'Ru',

    TaskTooltip : {
        'Scheduling Mode' : 'Тип планирования',
        'Calendar'        : 'Календарь',
        'Critical'        : 'Критично',
        'Yes'             : 'Да',
        'No'              : 'Нет'
    },

    Baselines : {
        Start    : 'Начинается',
        End      : 'Заканчивается',
        Duration : 'Длительность',
        Complete : 'Выполнено',

        baseline           : 'базовая линия',
        'Delayed start by' : 'Задержка старта на',
        'Overrun by'       : 'Переполнен на'
    },

    Shared : {
        'Locale changed'                                                : 'Язык изменен',
        'Fullscreen'                                                    : 'На весь экран',
        'Click to show the built in code editor'                        : 'Показать редактор кода',
        'Click to show info and switch theme or locale'                 : 'Показать информацию, переключить тему или язык',
        'Select theme'                                                  : 'Выбрать тему',
        'Select locale'                                                 : 'Выбрать язык',
        'Select size'                                                   : 'Выбрать размер',
        'Full size'                                                     : 'Полный размер',
        'Phone size'                                                    : 'Экран смартфона',
        'Display hints'                                                 : 'Показать подсказки',
        'Automatically'                                                 : 'Автоматически',
        'Check to automatically display hints when loading the example' : 'Автоматически показывать подсказки при загрузке примера'
    },

    CodeEditor : {
        'Code editor'   : 'Редактор кода',
        'Download code' : 'Скачать код',
        'Auto apply'    : 'Применять автоматически',
        'Apply'         : 'Применить'
    }
};

export default examplesLocale;

LocaleManager.extendLocale('Ru', examplesLocale);
