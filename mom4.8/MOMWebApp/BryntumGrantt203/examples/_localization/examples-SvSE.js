import LocaleManager from '../../lib/Core/localization/LocaleManager.js';
import '../../lib/Gantt/localization/SvSE.js';

const examplesLocale = {
    extends : 'SvSE',

    TaskTooltip : {
        'Scheduling Mode' : 'Läge',
        'Calendar'        : 'Kalender',
        'Critical'        : 'Kritisk',
        'Yes'             : 'Ja',
        'No'              : 'Nej'
    },

    Baselines : {
        Start    : 'Börjar',
        End      : 'Slutar',
        Duration : 'Längd',
        Complete : 'Färdig',

        baseline           : 'baslinje',
        'Delayed start by' : 'Försenad start med',
        'Overrun by'       : 'Överskridande med'
    },

    Shared : {
        'Locale changed' : 'Språk ändrat'
    }
};

export default examplesLocale;

LocaleManager.extendLocale('SvSE', examplesLocale);
