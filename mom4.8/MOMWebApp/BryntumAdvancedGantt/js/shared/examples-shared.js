(function () {
    window.__BRYNTUM_EXAMPLE = true;

    Ext.Loader.setConfig({
        enabled : true,
        paths   : {
            'Sch.locale'   : 'js/Sch/locale',
            'Gnt.locale'   : 'js/Gnt/locale',
            'Gnt.examples' : '../',
            'Sch.examples' : '../', // for DetailsPanel
            'Ext.ux'       : 'https://www.bryntum.com/examples/extjs-6.5.0/packages/ux/classic/src',
            'Robo'         : '../../packages/bryntum-gantt-pro/src/Robo',
            'Sch'          : '../../packages/bryntum-gantt-pro/src/Sch',
            'Gnt'          : '../../packages/bryntum-gantt-pro/src/Gnt',
            'Cal'          : '../../../Calendar1.x/packages/bryntum-calendar/src/Cal',
            'interact'     : '../../../Calendar1.x/packages/bryntum-calendar/src/interact.js',
            'Kanban'       : '../../../TaskBoard2.x/packages/bryntum-taskboard/src/Kanban'
        }
    });

    // if "bundle" specified in query string we switch to the "bundle-mode"
    // when we:
    // 1) load gnt-all-debug.js
    // 2) do not provide paths for on-demand classes loading (except for classes that are always meant to be loaded on demand, like locales)
    // 3) can load few extra bundles provided via "bundles=" request parameter
    if (document.location.href.match(/\?.*bundle/g)) {

        // include gnt-all-debug
        var bundles = ['../../gnt-all-debug.js'],
            match;

        // and extra bundle urls from "bundles" parameter (if provided)
        if (match = window.location.href.match(/bundles=(.+)/)) {
            bundles = bundles.concat(match[1].split(','));
        }

        /* jshint ignore:start */
        Ext.Array.forEach(bundles, function (url) {
            document.write('<script src="' + url + '" type="text/javascript"></script>');
        });
        /* jshint ignore:end */
    }

    // override XTemplate to not hide exceptions
    Ext.XTemplate.override({ strict : true });

    // mute ARIA warnings
    Ext.ariaWarn         = Ext.emptyFn;
    Ext.enableAriaPanels = false;

    // override loadScripts to log LOD files
    var oldLoadScripts = Ext.Loader.loadScripts;

    window.Sch      = window.Sch || {};
    Sch.loadedFiles = [];

    Ext.require([
        // load before logging starts to avoid logging...
        'Sch.examples.lib.DetailsPanel'
    ]);

    Ext.Loader.loadScripts = function (params) {
        Sch.loadedFiles = Ext.Array.union(Sch.loadedFiles, params.url);

        return oldLoadScripts.apply(this, arguments);
    };
    // EOF override loadScripts to log LOD files

    if (window.location.href.match(/^file:\/\/\//)) {
        alert('ERROR: You must run the examples in a web server (not using the file:/// protocol)');
    }
}());
