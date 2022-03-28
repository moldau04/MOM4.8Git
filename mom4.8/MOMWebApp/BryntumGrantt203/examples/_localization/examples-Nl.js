import LocaleManager from '../../lib/Core/localization/LocaleManager.js';
import '../../lib/Gantt/localization/Nl.js';

const examplesLocale = {
    extends : 'Nl',

    TaskTooltip : {
        'Scheduling Mode' : 'Planningsmodus',
        'Calendar'        : 'Kalender',
        'Critical'        : 'Kritisch',
        'Yes'             : 'Ja',
        'No'              : 'Nee'
    },

    Baselines : {
        Start    : 'Begin',
        End      : 'Einde',
        Duration : 'Duur',
        Complete : 'Gedaan',

        baseline           : 'baslinje',
        'Delayed start by' : 'Uitgestelde start met',
        'Overrun by'       : 'Overschreden met'
    },

    Shared : {
        'Locale changed' : 'Taal is veranderd'
    }
};

export default examplesLocale;

LocaleManager.extendLocale('Nl', examplesLocale);
