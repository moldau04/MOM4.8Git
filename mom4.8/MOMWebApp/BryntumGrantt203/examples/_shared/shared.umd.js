"use strict";

function _get(target, property, receiver) { if (typeof Reflect !== "undefined" && Reflect.get) { _get = Reflect.get; } else { _get = function _get(target, property, receiver) { var base = _superPropBase(target, property); if (!base) return; var desc = Object.getOwnPropertyDescriptor(base, property); if (desc.get) { return desc.get.call(receiver); } return desc.value; }; } return _get(target, property, receiver || target); }

function _superPropBase(object, property) { while (!Object.prototype.hasOwnProperty.call(object, property)) { object = _getPrototypeOf(object); if (object === null) break; } return object; }

function _typeof(obj) { "@babel/helpers - typeof"; if (typeof Symbol === "function" && typeof Symbol.iterator === "symbol") { _typeof = function _typeof(obj) { return typeof obj; }; } else { _typeof = function _typeof(obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; }; } return _typeof(obj); }

function asyncGeneratorStep(gen, resolve, reject, _next, _throw, key, arg) { try { var info = gen[key](arg); var value = info.value; } catch (error) { reject(error); return; } if (info.done) { resolve(value); } else { Promise.resolve(value).then(_next, _throw); } }

function _asyncToGenerator(fn) { return function () { var self = this, args = arguments; return new Promise(function (resolve, reject) { var gen = fn.apply(self, args); function _next(value) { asyncGeneratorStep(gen, resolve, reject, _next, _throw, "next", value); } function _throw(err) { asyncGeneratorStep(gen, resolve, reject, _next, _throw, "throw", err); } _next(undefined); }); }; }

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function _createClass(Constructor, protoProps, staticProps) { if (protoProps) _defineProperties(Constructor.prototype, protoProps); if (staticProps) _defineProperties(Constructor, staticProps); return Constructor; }

function _possibleConstructorReturn(self, call) { if (call && (_typeof(call) === "object" || typeof call === "function")) { return call; } return _assertThisInitialized(self); }

function _getPrototypeOf(o) { _getPrototypeOf = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) { return o.__proto__ || Object.getPrototypeOf(o); }; return _getPrototypeOf(o); }

function _assertThisInitialized(self) { if (self === void 0) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return self; }

function _inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function"); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, writable: true, configurable: true } }); if (superClass) _setPrototypeOf(subClass, superClass); }

function _setPrototypeOf(o, p) { _setPrototypeOf = Object.setPrototypeOf || function _setPrototypeOf(o, p) { o.__proto__ = p; return o; }; return _setPrototypeOf(o, p); }

var earlyErrorEvent;

var errorListener = function errorListener(errorEvent) {
  return earlyErrorEvent = errorEvent;
};

window.addEventListener('error', errorListener);

if (location.protocol === 'file:') {
  alert('ERROR: You must run examples on a webserver (not using the file: protocol)');
} // needed for tests


window.__BRYNTUM_EXAMPLE = true;

if (bryntum.gantt.VersionHelper.isTestEnv) {
  window.__applyTestConfigs = true;
}

var hintKey = 'preventhints-' + document.location.href,
    productName = 'gantt',
    //LEAVE AS IS, DEFAULT PRODUCT NAME
defaultTheme = 'Stockholm',
    browserPaths = ['/examples/', // In our source structure
'/grid/', // On bryntum.com...
'/scheduler/', '/gantt/'],
    themes = {
  stockholm: 'Stockholm',
  default: 'Default',
  light: 'Light',
  dark: 'Dark',
  material: 'Material'
},
    // Demos that should not use RC online
disableRootCause = ['bigdataset', 'csp'],
    pathName = location.pathname,
    isDemoBrowser = browserPaths.some(function (path) {
  return pathName.endsWith(path) || Boolean(pathName.match(path + 'index.*html$'));
}),
    isOnline = location.href.includes('bryntum.com'),
    moduleTag = document.querySelector('script[type=module]'),
    isModule = pathName.endsWith('module.html') || moduleTag && moduleTag.src.includes('app.module.js') || pathName.endsWith('index.html') && isOnline,
    isUmd = pathName.endsWith('umd.html');
var isSiesta = null;

try {
  isSiesta = window.parent.Siesta;
} catch (e) {}

document.body.classList.add('b-initially-hidden');

var Shared =
/*#__PURE__*/
function (_bryntum$gantt$Locali) {
  _inherits(Shared, _bryntum$gantt$Locali);

  //region Init
  function Shared() {
    var _this;

    _classCallCheck(this, Shared);

    _this = _possibleConstructorReturn(this, _getPrototypeOf(Shared).call(this));

    var me = _assertThisInitialized(_this),
        reset = document.location.href.match(/(\?|&)reset/),
        themeInfo = bryntum.gantt.DomHelper.themeInfo;

    if (reset) {
      bryntum.gantt.BrowserHelper.removeLocalStorageItem('exampleLanguage');
      bryntum.gantt.BrowserHelper.removeLocalStorageItem('bryntumExampleTheme');
    } // me.onResize = me.onResize.bind(me);


    me.destroyTooltips = me.destroyTooltips.bind(me); //me.onWindowScroll  = me.onWindowScroll.bind(me);
    // Module bundle is used by default online

    me.developmentMode = location.href.match(/(\?|&)develop/);
    me.productName = productName; // Only do theme restoration if we are using a standard theme

    if (themeInfo && themes[themeInfo.name.toLowerCase()]) {
      var theme = me.qs('theme', bryntum.gantt.BrowserHelper.getLocalStorageItem('bryntumExampleTheme') || defaultTheme); // Apply default theme first time when the page is loading

      me.applyTheme(theme, true);
    } else {
      document.body.classList.remove('b-initially-hidden');
    } // Enables special styling when generating thumbs


    if (document.location.href.match(/(\?|&)thumb/)) {
      document.body.classList.add('b-generating-thumb');
    } // Subscribe on locale update to save it into the localStorage


    me.localeManager.on('locale', function (localeConfig) {
      return bryntum.gantt.BrowserHelper.setLocalStorageItem('exampleLanguage', localeConfig.locale.localeName);
    }); // Apply default locale first time when the page is loading

    me.localeManager.applyLocale(bryntum.gantt.BrowserHelper.getLocalStorageItem('exampleLanguage') || bryntum.gantt.LocaleManager.locale.localeName, false, true); //}

    var overrideRowCount = document.location.search.match(/(?:\?|&)rowCount=([^&]*)/);

    if (overrideRowCount) {
      var parts = overrideRowCount[1].split(',');

      if (parts.length === 1) {
        bryntum.gantt.DataGenerator.overrideRowCount = parseInt(parts[0]);
      } else {
        bryntum.gantt.DataGenerator.overrideRowCount = parts.map(function (p) {
          return parseInt(p);
        });
      }
    } //<debug>
    // const
    //     positionMode    = me.qs('position', 'translate'),
    //     testPerformance = me.qs('testPerformance'),
    //     rowScrollMode   = me.qs('rowScrollMode', 'move');
    //
    // let defaultConfig = Grid.defaultConfig;
    // Object.defineProperty(Grid, 'defaultConfig', {
    //     get : () => {
    //         return Object.assign(defaultConfig, {
    //             testPerformance : testPerformance,
    //             positionMode    : positionMode,
    //             rowScrollMode   : rowScrollMode,
    //             destroyStore    : true
    //         });
    //     }
    // });
    //</debug>


    me.insertHeader(); // window.addEventListener('resize', me.onResize.bind(me));
    // me.onResize();

    me.loadDescription(); // Don't load hints for the example browser (or if viewing with ?develop)

    if (!isDemoBrowser && !me.developmentMode) {
      me.loadHints();
    }

    me.initRootCause();

    if (!isDemoBrowser) {
      me.injectFavIcon();
    }

    document.body.style.paddingRight = '0';
    return _this;
  }
  /**
   * Registers the passed URL to return the passed mocked up Fetch Response object to the
   * bryntum.gantt.AjaxHelper's promise resolve function.
   * @param {String} url The url to return mock data for
   * @param {Object|Function} response A mocked up Fetch Response object which must contain
   * at least a `responseText` property, or a function to which the `url` and a `params` object
   * and the `Fetch` `options` object is passed which returns that.
   * @param {String} response.responseText The data to return.
   * @param {Boolean} [response.synchronous] resolve the Promise immediately
   * @param {Number} [response.delay=100] resolve the Promise after this number of milliseconds.
   */


  _createClass(Shared, [{
    key: "mockUrl",
    value: function mockUrl(url, response) {
      bryntum.gantt.AjaxHelper.mockUrl.apply(bryntum.gantt.AjaxHelper, arguments);
    }
  }, {
    key: "injectFavIcon",
    value: function injectFavIcon() {
      bryntum.gantt.DomHelper.createElement({
        tag: 'link',
        parent: document.head,
        rel: 'icon',
        href: '../_shared/images/favicon.png',
        sizes: '32x32'
      });
    } // onResize() {
    //     const container = document.getElementById('container') || document.querySelector('.b-gridbase');
    //
    //     // Demos using  `adopt` will take over container
    //     if (container) {
    //         if (document.body.matches('.b-size-phone')) {
    //             const contentHeight = 667 + 60 + 20;
    //
    //             if (contentHeight > document.body.offsetHeight) {
    //                 const scale = document.body.offsetHeight / contentHeight;
    //                 container.style.transform = `translate(-50%, -50%) scale(${scale})`;
    //             }
    //         }
    //         else {
    //             container.style.transform = '';
    //         }
    //     }
    // }
    //endregion
    //region Header with tools

  }, {
    key: "insertHeader",
    value: function insertHeader() {
      var pathElements = document.location.pathname.split('/').reduce(function (result, value) {
        if (value) {
          result.push(value);
        }

        return result;
      }, []),
          exampleId = pathElements[pathElements.length - 1];
      bryntum.gantt.DomHelper.insertFirst(document.getElementById('container'), {
        tag: 'header',
        className: 'demo-header',
        html: "\n            <div id=\"title-container\">\n                <a id=\"title\" href=\"".concat(isDemoBrowser ? '#' : '../').concat(!isDemoBrowser && isUmd ? 'index.umd.html' : '', "#").concat(exampleId, "\">\n                    ").concat(document.title, "\n                </a>\n            </div>\n            <div id=\"tools\"></div>\n        ")
      });
      var tools = document.getElementById('tools') || document.body;

      if (bryntum.gantt.Fullscreen.enabled) {
        var fullscreenButton = bryntum.gantt.WidgetHelper.createWidget({
          type: 'button',
          id: 'fullscreen-button',
          icon: 'b-icon b-icon-fullscreen',
          tooltip: this.L('bryntum.gantt.Fullscreen'),
          toggleable: true,
          cls: 'b-blue b-raised',
          keep: true,
          appendTo: tools,
          onToggle: function onToggle(_ref) {
            var pressed = _ref.pressed;

            if (pressed) {
              bryntum.gantt.Fullscreen.request(document.body);
            } else {
              bryntum.gantt.Fullscreen.exit();
            }
          }
        });
        bryntum.gantt.Fullscreen.onFullscreenChange(function () {
          fullscreenButton.pressed = bryntum.gantt.Fullscreen.isFullscreen;
        });
      }

      this.codeButton = bryntum.gantt.WidgetHelper.createWidget({
        type: 'button',
        ref: 'codeButton',
        icon: 'b-icon b-icon-code',
        cls: 'b-blue b-raised keep',
        toggleable: true,
        tooltip: {
          html: this.L('Click to show the built in code editor'),
          align: 't100-b100'
        },
        preventTooltipOnTouch: true,
        appendTo: tools,
        hidden: true,
        keep: true,
        onToggle: function onToggle(_ref2) {
          var button = _ref2.source,
              pressed = _ref2.pressed;
          return _asyncToGenerator(
          /*#__PURE__*/
          regeneratorRuntime.mark(function _callee() {
            return regeneratorRuntime.wrap(function _callee$(_context) {
              while (1) {
                switch (_context.prev = _context.next) {
                  case 0:
                    if (!pressed) {
                      _context.next = 8;
                      break;
                    }

                    if (shared.codeEditor) {
                      _context.next = 5;
                      break;
                    }

                    _context.next = 4;
                    return CodeEditor.addToPage(button);

                  case 4:
                    shared.codeEditor = _context.sent;

                  case 5:
                    shared.codeEditor.show().then(function () {
                      shared.codeEditor.focus();
                    });
                    _context.next = 9;
                    break;

                  case 8:
                    shared.codeEditor.hide();

                  case 9:
                  case "end":
                    return _context.stop();
                }
              }
            }, _callee);
          }))();
        }
      });
      var button = this.infoButton = bryntum.gantt.WidgetHelper.createWidget({
        type: 'button',
        ref: 'infoButton',
        icon: 'b-icon b-icon-info',
        cls: 'b-blue b-raised keep',
        toggleable: true,
        tooltip: {
          html: this.L('Click to show info and switch theme or locale'),
          align: 't100-b100'
        },
        preventTooltipOnTouch: true,
        keep: true,
        appendTo: tools
      });
      var headerTools = document.getElementById('header-tools');

      if (headerTools) {
        Array.from(headerTools.children).forEach(function (child) {
          tools.insertBefore(child, button);
        });
        headerTools.remove();
      }
    } //endregion
    //region Hints

  }, {
    key: "initHints",
    value: function initHints() {
      var me = this;
      if (!me.hints || !bryntum.gantt.WidgetHelper.hasAdapter) return;
      me.toolTips = [];
      var delay = me.hints.delay || 0;
      setTimeout(function () {
        return Object.keys(me.hints).forEach(function (key, i) {
          if (key) {
            var target = bryntum.gantt.DomHelper.down(document.body, key);
            if (!target) return; //throw new Error(`Hint selector ${key} doesnt' match anything`);

            setTimeout(function () {
              if (!me.preventHints) {
                var hint = me.hints[key];
                me.toolTips.push(new bryntum.gantt.Tooltip({
                  forElement: target,
                  scrollAction: 'hide',
                  align: hint.align || 't-b',
                  html: "<div class=\"header\">".concat(hint.title, "</div><div class=\"description\">").concat(hint.content, "</div>"),
                  autoShow: true,
                  cls: 'b-hint',
                  textContent: true
                }));
              }
            }, (i + 1) * 500);
          }
        });
      }, delay); // Hide all hints on click anywhere, it also handles touch

      document.body.addEventListener('mousedown', this.destroyTooltips, true); //window.addEventListener('scroll', this.onWindowScroll, true);
    } // NOTE: this was commented out since it has negative effect on scrolling performance
    // onWindowScroll(e) {
    //     if (!e.target.matches('[class^=b-resize-monitor]')) {
    //         this.destroyTooltips();
    //     }
    // }

  }, {
    key: "destroyTooltips",
    value: function destroyTooltips() {
      var me = this;
      me.toolTips.forEach(function (tip) {
        return tip.destroy();
      });
      me.toolTips.length = 0;
      me.preventHints = true;
      document.body.removeEventListener('mousedown', me.destroyTooltips, true); //window.removeEventListener('scroll', me.onWindowScroll, true);
    }
  }, {
    key: "loadHints",
    value: function loadHints() {
      var _this2 = this;

      bryntum.gantt.AjaxHelper.get('meta/hints.json', {
        parseJson: true
      }).then(function (response) {
        _this2.hints = response.parsedJson;
        if (Object.keys(_this2.hints).length) _this2.hasHints = true;
        if (!localStorage.getItem(hintKey)) _this2.initHints();
      });
    } //endregion
    //region Description

  }, {
    key: "loadDescription",
    value: function loadDescription() {
      var me = this,
          button = me.infoButton,
          url = "".concat(isDemoBrowser ? '_shared/browser/' : '', "app.config.json");
      bryntum.gantt.AjaxHelper.get(url, {
        parseJson: true
      }).then(function (response) {
        var appConfig = response.parsedJson,
            themeInfo = bryntum.gantt.DomHelper.themeInfo,
            locales = [];
        Object.keys(me.localeManager.locales).forEach(function (key) {
          var locale = me.localeManager.locales[key];
          locales.push({
            value: key,
            text: locale.desc,
            data: locale
          });
        });
        var localeValue = me.localeManager.locale.localeName,
            storedLocaleValue = bryntum.gantt.BrowserHelper.getLocalStorageItem('exampleLanguage'),
            themeCombo; // check that stored locale is actually available among locales for this demo

        if (storedLocaleValue && locales.some(function (l) {
          return l.key === storedLocaleValue;
        })) localeValue = storedLocaleValue; // Leave as a config during app startup. `Button#get menu` will promote it to a widget
        // when the user clicks it.

        button.menu = {
          type: 'popup',
          anchor: true,
          align: 't100-b100',
          cls: 'info-popup',
          width: '22em',
          items: [{
            type: 'widget',
            html: "<div class=\"header\">".concat(appConfig.title, "</div><div class=\"description\">").concat(appConfig.description, "</div>")
          }].concat(themeInfo && themes[themeInfo.name.toLowerCase()] ? [themeCombo = {
            type: 'combo',
            ref: 'themeCombo',
            placeholder: me.L('Select theme'),
            editable: false,
            value: bryntum.gantt.StringHelper.capitalizeFirstLetter(bryntum.gantt.BrowserHelper.getLocalStorageItem('bryntumExampleTheme') || defaultTheme),
            items: themes,
            onAction: function onAction(_ref3) {
              var value = _ref3.value;
              me.applyTheme(value);
              button.menu.hide();
            }
          }] : []).concat([{
            type: 'combo',
            ref: 'localeCombo',
            placeholder: me.L('Select locale'),
            editable: false,
            store: {
              data: locales,
              sorters: [{
                field: 'text',
                ascending: true
              }]
            },
            displayField: 'text',
            valueField: 'value',
            value: localeValue,
            onAction: function onAction(_ref4) {
              var value = _ref4.value;
              me.localeManager.applyLocale(value);
              bryntum.gantt.Toast.show(me.L('Locale changed'));
              button.menu.hide();
            }
          }]).concat(isDemoBrowser ? [] : [{
            type: 'combo',
            ref: 'sizeCombo',
            placeholder: me.L('Select size'),
            editable: false,
            hidden: productName === 'scheduler',
            items: [{
              text: me.L('Full size'),
              value: 'b-size-full'
            }, {
              text: me.L('Phone size'),
              value: 'b-size-phone'
            }],
            value: 'b-size-full',
            onAction: function onAction(_ref5) {
              var value = _ref5.value;
              if (me.curSize) document.body.classList.remove(me.curSize);
              document.body.classList.add(value);
              document.body.classList.add('b-change-size');
              setTimeout(function () {
                return document.body.classList.remove('b-change-size');
              }, 400);
              me.curSize = value;
              button.menu.hide(); // TODO: should remove this at some point
              //     window.addEventListener('resize', me.onResize);
              //     me.onResize();
            }
          }, {
            type: 'button',
            ref: 'hintButton',
            text: me.L('Display hints'),
            cls: 'b-blue b-raised',
            onAction: function onAction() {
              button.menu.hide();
              me.preventHints = false;
              me.initHints();
            }
          }, {
            type: 'checkbox',
            ref: 'hintCheck',
            text: me.L('Automatically'),
            cls: 'b-blue',
            flex: '0 1 auto',
            tooltip: me.L('Check to automatically display hints when loading the example'),
            checked: !localStorage.getItem(hintKey),
            onAction: function onAction(_ref6) {
              var checked = _ref6.checked;

              if (checked) {
                localStorage.removeItem(hintKey);
              } else {
                localStorage.setItem(hintKey, true);
              }
            }
          }]),
          listeners: {
            beforeShow: function beforeShow() {
              var popup = this;
              themeCombo = popup.widgetMap.themeCombo;

              if (!isDemoBrowser) {
                if (!me.hasHints) {
                  popup.widgetMap.hintButton.hide();
                  popup.widgetMap.hintCheck.hide();
                } else {
                  popup.widgetMap.hintButton.show();
                  popup.widgetMap.hintCheck.show();
                }
              }
            }
          }
        }; // React to theme changes

        bryntum.gantt.GlobalEvents.on({
          theme: function theme(_ref7) {
            var _theme = _ref7.theme,
                prev = _ref7.prev;
            _theme = _theme.toLowerCase();
            themeCombo.value = _theme;
            bryntum.gantt.BrowserHelper.setLocalStorageItem('bryntumExampleTheme', _theme);
            document.body.classList.add("b-theme-".concat(_theme));
            document.body.classList.remove("b-theme-".concat(prev));
            me.prevTheme = prev;
            me.trigger('theme', {
              theme: _theme,
              prev: prev
            });
          },
          // call before other theme listeners
          prio: 1
        });
      });
    } //endregion
    //region QueryString

  }, {
    key: "qs",
    value: function qs(key) {
      var defaultValue = arguments.length > 1 && arguments[1] !== undefined ? arguments[1] : null;
      var regexp = new RegExp("(?:\\?|&)".concat(key, "=([^&]*)")),
          matches = document.location.href.match(regexp);
      if (!matches) return defaultValue;
      return matches[1];
    } //endregion
    //region Theme applying

  }, {
    key: "applyTheme",
    value: function applyTheme(newThemeName) {
      var initial = arguments.length > 1 && arguments[1] !== undefined ? arguments[1] : false;
      var _document = document,
          body = _document.body;
      newThemeName = newThemeName.toLowerCase(); // only want to block transition when doing initial apply of theme

      if (initial) {
        body.classList.add('b-notransition');
      }

      bryntum.gantt.DomHelper.setTheme(newThemeName).then(function () {
        // display after loading theme to not show initial transition from default theme
        document.body.classList.remove('b-initially-hidden');

        if (isDemoBrowser) {
          document.body.style.visibility = 'visible';
        }

        if (initial) {
          body.classList.add("b-theme-".concat(newThemeName));
          setTimeout(function () {
            body.classList.remove('b-notransition');
          }, 100);
        }
      });
    }
  }, {
    key: "fireMouseEvent",
    // Utility method for when creating thumbs.
    // Eg: shared.fireMouseEvent('mouseover', document.querySelector('.b-task-rollup'));
    value: function fireMouseEvent(type, target) {
      var targetRect = bryntum.gantt.Rectangle.from(target),
          center = targetRect.center;
      target.dispatchEvent(new MouseEvent(type, {
        clientX: center.x,
        clientY: center.y,
        bubbles: true
      }));
    } //endregion
    // region RootCause
    // Shared RootCause code for frameworks should be updated here scripts/grunt/tasks/templates/rootcause.ejs

  }, {
    key: "initRootCause",
    value: function initRootCause() {
      var recordVideo = location.search.includes('video=1'),
          disabled = disableRootCause.some(function (exclude) {
        return location.href.includes(exclude);
      }),
          isExcluded = !recordVideo && (disabled || isDemoBrowser),
          isRootCauseReplay = function () {
        try {
          // eslint-disable-next-line no-unused-vars
          var a = window.top.location.href;
        } catch (e) {
          return true;
        }

        return false;
      }();

      if ((isOnline || isRootCauseReplay) && !isExcluded && !isSiesta) {
        var script = document.createElement('script');
        script.async = true;
        script.crossOrigin = 'anonymous';
        script.src = 'https://app.therootcause.io/rootcause-full.js';
        script.addEventListener('load', this.startRootCause);
        document.head.appendChild(script);
      }
    }
  }, {
    key: "startRootCause",
    value: function startRootCause() {
      var appIds = {
        grid: '9ea6c8f84f179d4c4b7be11ff217bc29367357f8',
        scheduler: '3dcfabf52d4fa704fb95259a317c72a6ce376313',
        gantt: '9df03cbdc00b455de8bfe58d831e6fd76cc8881e'
      },
          //suiteRe     = /\/examples\/([a-z]+)/,
      product = productName,
          //location.href.match(suiteRe) && location.href.match(suiteRe)[1] || 'grid',
      appId = appIds[product] || 'unknown',
          version = bryntum.gantt.VersionHelper.getVersion(product),
          recordEvents = !('ontouchstart' in document.documentElement),
          // Skip event recording on touch devices as RC could cause lag
      recordVideo = location.search.includes('video=1');

      if (!window.RC) {
        console.log('RootCause not initialized');
        return;
      } // eslint-disable-next-line no-undef


      window.logger = new RC.Logger({
        captureScreenshot: true,
        recordUserActions: recordEvents && !location.href.match('bigdataset'),
        logAjaxRequests: true,
        applicationId: appId,
        maxNbrLogs: isOnline ? 1 : 0,
        autoStart: isOnline,
        treatFailedAjaxAsError: true,
        // enableArgumentsCapturing        : true,
        treatResourceLoadFailureAsError: true,
        showFeedbackButton: isOnline,
        recordSessionVideo: recordVideo,
        showIconWhileRecording: {
          tooltip: 'NOTE: This session is being recorded for debugging purposes'
        },
        recorderConfig: {
          recordScroll: bryntum.gantt.BrowserHelper.isChrome,
          // quite big overhead for this in FF
          // Ignore our own auto-generated ids since they are not stable
          shouldIgnoreDomElementId: function shouldIgnoreDomElementId(id) {
            return /^(b_|b-)/.test(id);
          },
          ignoreCssClasses: ['b-sch-event-hover', 'b-contains-focus', 'b-hover', 'b-dirty', 'b-focused']
        },
        version: version,
        ignoreErrorMessageRe: /Script error|Unexpected token var|ResizeObserver/i,
        ignoreFileRe: /^((?!bryntum).)*$/ // Ignore non-bryntum domain errors

      }); // Abort early error listener

      window.removeEventListener('error', errorListener);

      if (earlyErrorEvent) {
        window.logger.logException(earlyErrorEvent.error);
      }
    } // endregion

  }, {
    key: "themeInfo",
    get: function get() {
      return bryntum.gantt.DomHelper.themeInfo || {
        name: defaultTheme
      };
    }
  }, {
    key: "theme",
    get: function get() {
      return this.themeInfo.name;
    }
  }]);

  return Shared;
}(bryntum.gantt.Localizable(bryntum.gantt.Events()));

var keywords = ['import', 'if', 'switch', 'else', 'var', 'const', 'let', 'delete', 'true', 'false', 'from', 'return', 'new', 'function', '=>', 'class', 'get', 'set', 'break', 'return', 'export', 'default', 'static', 'extends'],
    jsSyntax = {
  string: /'.*?'|`.*?`|".*?"/g,
  comment: /[^"](\/\/.*)/g,
  keyword: new RegExp(keywords.join('[ ;,\n\t]|[ ;,\n\t]'), 'g'),
  tag: /&lt;.*?&gt;/g,
  curly: /[{}[\]()]/g
},
    cssSyntax = {
  keyword: /^\..*\b/gm,
  string: /:(.*);/g
};

var CodeEditor =
/*#__PURE__*/
function (_bryntum$gantt$Popup) {
  _inherits(CodeEditor, _bryntum$gantt$Popup);

  function CodeEditor() {
    _classCallCheck(this, CodeEditor);

    return _possibleConstructorReturn(this, _getPrototypeOf(CodeEditor).apply(this, arguments));
  }

  _createClass(CodeEditor, [{
    key: "construct",
    value: function construct(config) {
      _get(_getPrototypeOf(CodeEditor.prototype), "construct", this).call(this, config);

      var me = this;
      me.update = me.buffer('applyChanges', isOnline ? 1500 : 250);
      new bryntum.gantt.ResizeHelper({
        targetSelector: '.b-codeeditor',
        rightHandle: false,
        skipTranslate: true,
        minWidth: 190,
        listeners: {
          resizeStart: function resizeStart() {
            me.resizing = true;
          },
          resize: function resize() {
            me.resizing = false;
          },
          thisObj: me
        }
      });
    }
  }, {
    key: "show",
    value: function show() {
      if (this.showAnimation) {
        this.showAnimation.right.from = "-".concat(this.width, "px");
      }

      document.body.style.paddingRight = "".concat(this.width, "px");
      return _get(_getPrototypeOf(CodeEditor.prototype), "show", this).call(this);
    }
  }, {
    key: "hide",
    value: function hide() {
      if (this.hideAnimation) {
        this.hideAnimation.right.to = "-".concat(this.width, "px");
      }

      document.body.style.paddingRight = 0;
      return _get(_getPrototypeOf(CodeEditor.prototype), "hide", this).call(this);
    }
  }, {
    key: "onHide",
    value: function onHide() {
      shared.codeButton.pressed = false;
    }
  }, {
    key: "onElementResize",
    value: function onElementResize(resizedElement, lastRect, myRect) {
      _get(_getPrototypeOf(CodeEditor.prototype), "onElementResize", this).call(this, resizedElement, lastRect, myRect);

      if (this.resizing) {
        document.body.style.transition = 'none';
        document.body.style.paddingRight = "".concat(this.width, "px");
        requestAnimationFrame(function () {
          document.body.style.transition = '';
        });
      }
    }
  }, {
    key: "processJS",
    value: function processJS(code) {
      // Html encode tags used in strings
      code = code.replace(/</g, '&lt;').replace(/>/g, '&gt;'); // Wrap keywords etc in !keyword!

      Object.keys(jsSyntax).forEach(function (type) {
        code = code.replace(jsSyntax[type], "\xA7".concat(type, "\xA7$&</span>"));
      }); // Replace wrap from above with span (needs two steps to not think class="xx" is a keyword, etc)

      code = code.replace(/§(.*?)§/g, '<span class="$1">');
      return code;
    }
  }, {
    key: "processCSS",
    value: function processCSS(css) {
      // Wrap keywords etc in !keyword!
      Object.keys(cssSyntax).forEach(function (type) {
        css = css.replace(cssSyntax[type], function (match, p1) {
          // RegEx with group, use matched group
          if (typeof p1 === 'string') {
            return match.replace(p1, "\xA7".concat(type, "\xA7").concat(p1, "</span>"));
          } // No group, use entire match
          else {
              return "\xA7".concat(type, "\xA7").concat(match, "</span>");
            }
        });
      }); // Replace wrap from above with span (needs two steps to not think class="xx" is a keyword, etc)

      css = css.replace(/§(.*?)§/g, '<span class="$1">');
      return css;
    }
  }, {
    key: "onCloseClick",
    value: function onCloseClick() {
      this.hide();
    }
  }, {
    key: "onFilesComboChange",
    value: function onFilesComboChange(_ref8) {
      var value = _ref8.value;
      this.loadCode(value);
    }
  }, {
    key: "onAutoApplyAction",
    value: function onAutoApplyAction(_ref9) {
      var checked = _ref9.checked;
      this.widgetMap.applyButton.disabled = checked;

      if (checked) {
        this.applyChanges();
      }
    }
  }, {
    key: "applyChanges",
    value: function applyChanges() {
      switch (this.mode) {
        case 'js':
          this.updateJS();
          break;

        case 'css':
          this.updateCSS();
          break;
      }

      this.updateDownloadLink();
    }
  }, {
    key: "updateCSS",
    value: function updateCSS() {
      var me = this;

      if (!me.cssElement) {
        me.cssElement = bryntum.gantt.DomHelper.createElement({
          parent: document.head,
          tag: 'style',
          type: 'text/css'
        });
      }

      me.codeCache[me.filename] = me.cssElement.innerHTML = me.codeElement.innerText;
    }
  }, {
    key: "updateJS",
    value: function () {
      var _updateJS = _asyncToGenerator(
      /*#__PURE__*/
      regeneratorRuntime.mark(function _callee2() {
        var _this3 = this;

        var me, code, renderedElements, renderedWidgets, imports, pathParts, base, rewrittenCode, objectUrl;
        return regeneratorRuntime.wrap(function _callee2$(_context2) {
          while (1) {
            switch (_context2.prev = _context2.next) {
              case 0:
                me = this, code = me.codeElement.innerText + '\n//export default null;\n', renderedElements = new Set(document.querySelectorAll('[data-app-element=true]')), renderedWidgets = new Set();
                me.codeCache[me.filename] = me.codeElement.innerText; // Clean out styles from any copy-pasted IDE code snippets

                Array.from(me.codeElement.querySelectorAll('pre [style]')).forEach(function (el) {
                  return el.style = '';
                }); // Store all current uncontained widgets to be able to cleanup on import fail. If the import fails because of a
                // syntax error some code might have executed successfully and we might get unwanted widgets rendered

                bryntum.gantt.DomHelper.forEachSelector('.b-widget.b-outer', function (element) {
                  var widget = bryntum.gantt.IdHelper.fromElement(element, 'widget');

                  if (widget !== _this3) {
                    renderedWidgets.add(widget);
                  }
                });
                _context2.prev = 4;
                me.status = '<i class="b-icon b-icon-spinner">Applying changes'; // Keeping comment out code around in case we need it to later on support multi module editing
                // // Post to store in backend session
                // const response = await bryntum.gantt.AjaxHelper.post(`../_shared/module.php?file=${me.filename}`, code, { parseJson : true });
                //
                // // Safari doesn't send cookies in import requests, so we extract it and
                // // pass it as part of the URL.
                // if (!me.phpSessionId) {
                //     me.phpSessionId = /PHPSESSID=([^;]+)/.exec(document.cookie)[1];
                // }
                //
                // if (response.parsedJson.success) {

                imports = code.match(/import .*/gm), pathParts = document.location.pathname.split('/'), base = "".concat(document.location.protocol, "//").concat(document.location.host);
                rewrittenCode = code; // Rewrite relative imports as absolute, to work with createObjectURL approach below

                imports && imports.forEach(function (imp) {
                  var parts = imp.split('../');

                  if (parts.length) {
                    var // ../_shared needs Grid/examples, while ../../lib needs Grid/
                    absolute = pathParts.slice().splice(0, pathParts.length - parts.length).join('/'),
                        // import xx from 'http://lh/Grid/lib...'
                    statement = "".concat(parts[0]).concat(base).concat(absolute, "/").concat(parts[parts.length - 1]);
                    rewrittenCode = rewrittenCode.replace(imp, statement);
                  }
                }); // Retrieve module from session. Wrapped in eval() to hide it from FF, it refuses to load otherwise
                // eslint-disable-next-line no-eval,no-template-curly-in-string
                //await eval(`import('./module.php?file=${me.filename}&dt=${new Date().getTime()}&token=${me.phpSessionId}')`);
                // Retrieve module from object url. Wrapped in eval() to hide it from FF, it refuses to load otherwise

                objectUrl = URL.createObjectURL(new Blob([rewrittenCode], {
                  type: 'text/javascript'
                })); // eslint-disable-next-line no-eval

                _context2.next = 12;
                return eval("import(objectUrl)");

              case 12:
                URL.revokeObjectURL(objectUrl);
                document.body.style.paddingRight = "".concat(this.width, "px");
                bryntum.gantt.DomHelper.removeEachSelector(document, '#tools > .remove-widget');
                me.widgetMap.applyButton.disable(); // Destroy pre-existing demo tools, grids etc. after the import, to lessen flickering

                renderedWidgets.forEach(function (widget) {
                  return !widget.isDestroyed && !widget.keep && widget.destroy();
                }); // Destroy any additional elements added by the demo

                renderedElements.forEach(function (element) {
                  return element.remove();
                }); // If we have gotten this far the code is valid

                me.element.classList.remove('invalid');
                me.title = '<i class="b-fa b-fa-fw b-fa-code"></i> ' + me.L('Code editor');
                me.status = 'Idle'; // }

                _context2.next = 29;
                break;

              case 23:
                _context2.prev = 23;
                _context2.t0 = _context2["catch"](4);
                // Exception, either some network problem or invalid code
                me.title = '<i class="b-fa b-fa-fw b-fa-skull"></i> ' + me.L('Code editor');
                me.element.classList.add('invalid');
                me.status = _context2.t0.message; // Remove any widgets created by the failed import (might have successfully added some)

                bryntum.gantt.DomHelper.forEachSelector('.b-widget', function (element) {
                  var widget = bryntum.gantt.IdHelper.fromElement(element); // Only care about top level components

                  if (widget && !widget.isDestroyed && !widget.owner && !renderedWidgets.has(widget)) {
                    widget.destroy();
                  }
                });

              case 29:
              case "end":
                return _context2.stop();
            }
          }
        }, _callee2, this, [[4, 23]]);
      }));

      function updateJS() {
        return _updateJS.apply(this, arguments);
      }

      return updateJS;
    }()
  }, {
    key: "loadCode",
    value: function () {
      var _loadCode = _asyncToGenerator(
      /*#__PURE__*/
      regeneratorRuntime.mark(function _callee3() {
        var filename,
            me,
            code,
            exception,
            response,
            _args3 = arguments;
        return regeneratorRuntime.wrap(function _callee3$(_context3) {
          while (1) {
            switch (_context3.prev = _context3.next) {
              case 0:
                filename = _args3.length > 0 && _args3[0] !== undefined ? _args3[0] : isModule ? 'app.module.js' : isUmd ? 'app.umd.js' : 'app.js';
                me = this;
                code = me.codeCache[filename], exception = null;
                me.filename = filename;

                if (code) {
                  _context3.next = 18;
                  break;
                }

                _context3.prev = 5;
                _context3.next = 8;
                return bryntum.gantt.AjaxHelper.get(location.href.replace(/[^/]*$/, '') + filename);

              case 8:
                response = _context3.sent;
                _context3.next = 11;
                return response.text();

              case 11:
                code = me.codeCache[filename] = _context3.sent;
                _context3.next = 18;
                break;

              case 14:
                _context3.prev = 14;
                _context3.t0 = _context3["catch"](5);
                code = '';
                exception = _context3.t0;

              case 18:
                me.loadedCode = code;

                if (filename.endsWith('.js')) {
                  me.mode = 'js';
                  code = me.processJS(code);
                } else if (filename.endsWith('.css')) {
                  me.mode = 'css';
                  code = me.processCSS(code);
                }

                me.codeElement.innerHTML = code;
                me.status = "".concat(exception ? exception.message : 'Idle');
                me.toggleReadOnly();
                me.updateDownloadLink();
                me.contentElement.querySelector('code').setAttribute('spellcheck', 'false');

              case 25:
              case "end":
                return _context3.stop();
            }
          }
        }, _callee3, this, [[5, 14]]);
      }));

      function loadCode() {
        return _loadCode.apply(this, arguments);
      }

      return loadCode;
    }()
  }, {
    key: "updateDownloadLink",
    value: function updateDownloadLink() {
      var downloadLink = this.downloadLink;

      if (!downloadLink) {
        downloadLink = this.downloadLink = this.tools.download.element.firstElementChild;
      }

      downloadLink.download = this.filename;
      downloadLink.href = "data:text/".concat(this.filename.endsWith('.css') ? 'css' : 'javascript', ";charset=utf-8,").concat(escape(this.codeElement.innerText));
    }
  }, {
    key: "toggleReadOnly",
    value: function toggleReadOnly() {
      var me = this,
          widgetMap = me.widgetMap,
          contentElement = me.contentElement,
          readOnly = me.mode === 'js' && (me.hasImports || isUmd || !me.supportsImport);

      if (readOnly) {
        contentElement.classList.add('readonly');
        widgetMap.applyButton.disabled = true;
        widgetMap.autoApply.disabled = true;
        me.status = '<i class="b-fa b-fa-lock" /> Read only' + (!me.supportsImport ? ' (try it on Chrome or Firefox)' : '');
      } else {
        contentElement.classList.remove('readonly');
        widgetMap.autoApply.disabled = false;
        me.status = 'Idle';
      } // Have not figured out any easy way of editing additional modules, read only for now.
      // Ticket to resolve : https://app.assembla.com/spaces/bryntum/tickets/8429


      contentElement.querySelector('code').setAttribute('contenteditable', !readOnly);
    } // Find all imports in the code, extracting their filename to populate combo with

  }, {
    key: "extractImports",
    value: function extractImports(code) {
      var regex = /'\.\/(.*)';/g,
          imports = [];
      var result;

      while ((result = regex.exec(code)) !== null) {
        imports.push(result[1]);
      }

      return imports;
    }
  }, {
    key: "bodyConfig",
    get: function get() {
      var result = _get(_getPrototypeOf(CodeEditor.prototype), "bodyConfig", this);

      result.children = [{
        tag: 'pre',
        children: [{
          tag: 'code',
          reference: 'codeElement'
        }]
      }];
      return result;
    }
  }, {
    key: "focusElement",
    get: function get() {
      return this.codeElement;
    }
  }, {
    key: "status",
    set: function set(status) {
      this.widgetMap.status.html = status;
    }
  }, {
    key: "supportsImport",
    get: function get() {
      if (!this.hasOwnProperty('_supportsImports')) {
        try {
          eval('import(\'../_shared/dummy.js\')'); // eslint-disable-line no-eval

          this._supportsImports = true;
        } catch (e) {
          this._supportsImports = false;
        }
      }

      return this._supportsImports;
    }
  }], [{
    key: "addToPage",
    value: function () {
      var _addToPage = _asyncToGenerator(
      /*#__PURE__*/
      regeneratorRuntime.mark(function _callee4(button) {
        var editor, widgetMap, contentElement, filesStore, imports;
        return regeneratorRuntime.wrap(function _callee4$(_context4) {
          while (1) {
            switch (_context4.prev = _context4.next) {
              case 0:
                editor = new CodeEditor({
                  owner: button,
                  appendTo: bryntum.gantt.Widget.floatRoot
                }), widgetMap = editor.widgetMap, contentElement = editor.contentElement, filesStore = widgetMap.filesCombo.store;

                if (editor.showAnimation) {
                  editor.element.style.right = "-".concat(editor.width, "px");
                } else {
                  editor.element.style.right = 0;
                }

                bryntum.gantt.IdHelper.disableThrow = true;
                _context4.next = 5;
                return editor.loadCode();

              case 5:
                // Populate combo with imports. If we have imports, editing will be disabled for now #8429
                imports = editor.extractImports(editor.loadedCode);
                filesStore.add(imports.map(function (file) {
                  return {
                    text: file,
                    value: file
                  };
                }));
                editor.hasImports = imports.length > 0;
                editor.toggleReadOnly(); // Include css in combo

                if (document.head.querySelector('[href$="app.css"]')) {
                  filesStore.add({
                    text: 'resources/app.css',
                    value: 'resources/app.css'
                  });
                } // Only show combo if it has multiple items, no point otherwise :)


                widgetMap.filesCombo[filesStore.count > 1 ? 'show' : 'hide']();
                bryntum.gantt.EventHelper.on({
                  element: contentElement,
                  input: function input() {
                    if (widgetMap.autoApply.checked) {
                      editor.update();
                    } else {
                      widgetMap.applyButton.enable();
                    }
                  },
                  keydown: function keydown(event) {
                    if (event.key === 'Enter') {
                      document.execCommand('insertHTML', false, '<br>');
                      event.preventDefault();
                    }
                  },
                  thisObj: editor
                });
                return _context4.abrupt("return", editor);

              case 13:
              case "end":
                return _context4.stop();
            }
          }
        }, _callee4);
      }));

      function addToPage(_x) {
        return _addToPage.apply(this, arguments);
      }

      return addToPage;
    }()
  }, {
    key: "defaultConfig",
    get: function get() {
      return {
        textContent: false,
        autoClose: false,
        autoShow: false,
        scrollable: true,
        closable: true,
        hideAnimation: {
          right: {
            from: 0,
            to: 0,
            duration: '.3s',
            delay: '0s'
          }
        },
        showAnimation: {
          right: {
            from: 0,
            to: 0,
            duration: '.3s',
            delay: '0s'
          }
        },
        title: '<i class="b-fa b-fa-code"></i> ' + this.L('Code editor'),
        codeCache: {},
        tools: {
          download: {
            tooltip: this.L('Download code'),
            html: '<a class="b-fa b-fa-file-download" href=""></a>'
          }
        },
        tbar: [{
          type: 'combo',
          ref: 'filesCombo',
          editable: false,
          value: isModule ? 'app.module.js' : isUmd ? 'app.umd.js' : 'app.js',
          items: [isModule ? 'app.module.js' : isUmd ? 'app.umd.js' : 'app.js'],
          style: 'margin-right: .5em',
          onChange: 'up.onFilesComboChange'
        }, {
          type: 'checkbox',
          label: this.L('Auto apply'),
          ref: 'autoApply',
          checked: true,
          onAction: 'up.onAutoApplyAction'
        }, {
          type: 'button',
          text: this.L('Apply'),
          icon: 'b-fa b-fa-sync-alt',
          ref: 'applyButton',
          disabled: true,
          onAction: 'up.applyChanges'
        }],
        bbar: [{
          type: 'widget',
          ref: 'status',
          html: 'Idle'
        }]
      };
    }
  }]);

  return CodeEditor;
}(bryntum.gantt.Popup); //<debug>
// lazy debugging


setTimeout(
/*#__PURE__*/
_asyncToGenerator(
/*#__PURE__*/
regeneratorRuntime.mark(function _callee5() {
  return regeneratorRuntime.wrap(function _callee5$(_context5) {
    while (1) {
      switch (_context5.prev = _context5.next) {
        case 0:
          window.grid = bryntum.query('grid', true) || bryntum.query('treegrid', true);
          window.scheduler = bryntum.query('scheduler', true);
          window.gantt = bryntum.query('gantt', true);

          if ((window.grid || window.scheduler || window.gantt) && !isDemoBrowser) {
            shared.codeButton.show();
          } // Show code editor


          if (!document.location.search.match(/[?&]code/)) {
            _context5.next = 10;
            break;
          }

          _context5.next = 7;
          return CodeEditor.addToPage(shared.codeButton);

        case 7:
          shared.codeEditor = _context5.sent;
          shared.codeButton.pressed = true;
          shared.codeEditor.show().then(function () {
            shared.codeEditor.focus();
          });

        case 10:
        case "end":
          return _context5.stop();
      }
    }
  }, _callee5);
})), 100); //</debug>

var shared = new Shared(); // ugly, but needed for bundled demo browser to work

window.shared = shared; //export default shared;