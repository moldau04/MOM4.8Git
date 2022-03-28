/*global RC, Siesta, Kanban, Sch, Gnt, UberGrid, Cal */
//NOT FOR PUBLIC USE, FOR EVALUTION PURPOSES ONLY
(function () {
    var isOnline = location.href.indexOf('bryntum.com') >= 0;

    if (window.RC) {
        var suiteRe         = /\/examples\/([a-z]+)-latest\/examples/,
            mainApplication = location.href.match(suiteRe) && location.href.match(suiteRe)[ 1 ] || 'unknown',
            recordVideo     = location.search.indexOf('video=1') >= 0;

        window.logger = new RC.Logger({
            captureScreenshot               : true,
            recordUserActions               : true,
            logAjaxRequests                 : true,
            saveCookies                     : true,
            applicationId                   : mainApplication,
            maxNbrLogs                      : isOnline ? 1 : 0,
            autoStart                       : isOnline,
            treatFailedAjaxAsError          : true,
            enableArgumentsCapturing        : true,
            treatResourceLoadFailureAsError : true,
            showFeedbackButton              : isOnline,
            recordSessionVideo              : recordVideo,
            ignoreErrorMessageRe            : /Unexpected token var/i,
            recorderConfig                  : {
                extractorConfig : {
                    ignoreClasses : [
                        'sch-dd-ref',
                        'sch-dirty'
                    ]
                }
            },
            ignoreFileRe                    : /ga\.js/,
            frameworkVersion                : (Ext.versions.extjs || Ext.versions.touch || {}).version,

            onBeforeLog : function (data) {
                var stack = data.stack;
                var tags  = {},
                    mainVersion;

                if (window.Kanban && Kanban.VERSION) tags.taskboard = Kanban.VERSION;
                if (window.Cal && Cal.VERSION) tags.calendar = Cal.VERSION;
                if (window.Gnt && window.Gnt.panel && Sch.VERSION) tags.gantt = Sch.VERSION;
                if (window.Sch && Sch.VERSION) tags.scheduler = Sch.VERSION;
                if (window.UberGrid && window.UberGrid.VERSION) tags.ubergrid = UberGrid.VERSION;
                if (window.Siesta && Siesta.VERSION) tags.siesta = Siesta.VERSION;
                if (window.Robo && Robo.VERSION) tags.robo = Robo.VERSION;

                switch (mainApplication) {
                    case 'gantt':
                    case 'scheduler':
                        mainVersion = Sch.VERSION;
                        break;

                    case 'taskboard':
                        mainVersion = Kanban.VERSION;
                        break;

                    case 'siesta':
                        mainVersion = Siesta.VERSION;
                        break;

                    case 'ubergrid':
                        mainVersion = UberGrid.VERSION;
                        break;

                    case 'calendar':
                        mainVersion = Cal.VERSION;
                        break;

                    case 'robo':
                        mainVersion = Robo.VERSION;
                        break;
                }

                // Detect and assign Kitchensink bugs to the correct app id
                if (stack && stack.match('\/kitchensink\/')) {
                    data.applicationId = 'a37e444595168872f99d108d6bda96a2e26e0284';
                }

                data.tags    = tags;
                data.version = mainVersion;

                // Ignore failed loads if tab is inactivated which causes resource loading to fail sometimes due to timeout
                if (data.isFailedAjax || data.isResourceLoadError) {
                    var windowIsOrWasHidden = document.visibilityState === 'hidden';
                    var logs                = data.logEntries || [];

                    if (!windowIsOrWasHidden) {
                        for (var i = 0; i < logs.length; i++) {
                            if (logs[ i ].type === 'visibility') {
                                windowIsOrWasHidden = true;
                                break;
                            }
                        }
                    }

                    return !windowIsOrWasHidden;
                }
            }
        });
    }

    if (isOnline) {
        // Analytics
        window._gaq = window._gaq || [];
        window._gaq.push([ '_setAccount', 'UA-11046863-1' ]);
        window._gaq.push([ '_setDomainName', 'none' ]);
        window._gaq.push([ '_setAllowLinker', true ]);
        window._gaq.push([ '_trackPageview' ]);

        var ga   = document.createElement('script');
        ga.type  = 'text/javascript';
        ga.async = true;
        ga.src   = ('https:' == document.location.protocol ? 'https://ssl' : 'http://www') + '.google-analytics.com/ga.js';
        var s    = document.getElementsByTagName('script')[ 0 ];
        s.parentNode.insertBefore(ga, s);
        // EOF analytics
    }
})();


