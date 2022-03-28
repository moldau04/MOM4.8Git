"use strict";

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function _createClass(Constructor, protoProps, staticProps) { if (protoProps) _defineProperties(Constructor.prototype, protoProps); if (staticProps) _defineProperties(Constructor, staticProps); return Constructor; }

/* eslint-disable no-new */
// TODO: convert to ES6. Originally wanted to avoid babeling both doing that now anyway so...
var ExamplesApp =
/*#__PURE__*/
function () {
  function ExamplesApp() {
    _classCallCheck(this, ExamplesApp);

    var me = this,
        product = bryntum.gantt.StringHelper.capitalizeFirstLetter(shared.productName),
        version = bryntum.gantt.VersionHelper.getVersion(shared.productName);
    me.examples = window.examples || [];
    me.currentTipLoadPromiseByURL = {}; // remove prerendered examples

    me.examplesContainerEl = document.getElementById('scroller');
    me.examplesContainerEl.innerHTML = '';
    document.body.classList.add(shared.theme);
    document.getElementById('title').innerHTML = "Bryntum ".concat(product, " ").concat(version);
    bryntum.gantt.GlobalEvents.on({
      theme: function theme() {
        document.body.classList.remove(shared.prevTheme);
        document.body.classList.add(shared.theme);
        if (me.rendered) me.updateThumbs();
      }
    });
    me.isOnline = /bryntum.com|^lh$|^local$/.test(location.host);
    me.buildTip = me.isOnline ? 'This demo is not viewable online, but included when you download the trial. ' : 'This demo needs to be built before it can be viewed. '; // Do not display documentation button online, paths will be wrong

    if (!me.isOnline) {
      bryntum.gantt.WidgetHelper.append([{
        type: 'button',
        text: 'Documentation',
        icon: 'b-fa b-fa-question',
        cls: 'b-blue b-raised',
        style: 'margin-right: 1em',
        onClick: function onClick() {
          return document.location = '../docs';
        }
      }], {
        insertFirst: document.getElementById('tools') || document.body,
        cls: 'b-bright'
      });
    }

    if (location.search.match('prerender')) {
      me.embedDescriptions().then(me.render.bind(me));
    } else {
      me.render();
    }

    if (location.hash) {
      var exampleId = window.location.hash.split('#')[1];

      if (exampleId) {
        var exampleThumb = document.getElementById(exampleId);

        if (exampleThumb) {
          var scrollable = new bryntum.gantt.Scroller({
            element: exampleThumb.closest('#scroller')
          });
          scrollable.scrollIntoView(exampleThumb, {
            animate: true,
            highlight: true,
            focus: true
          });
        }
      }
    }
  }

  _createClass(ExamplesApp, [{
    key: "onCloseClick",
    value: function onCloseClick() {
      document.getElementById('intro').style.maxHeight = '0';
    }
  }, {
    key: "updateThumbs",
    value: function updateThumbs() {
      // replace thumb with new theme
      Array.from(document.querySelectorAll('.image img')).forEach(function (img) {
        img.src = img.src.replace(/thumb\..*?\.png/, "thumb.".concat(shared.theme, ".png").toLowerCase());
      });
    }
  }, {
    key: "template",
    value: function template(data) {
      var _this = this;

      var theme = shared.theme,
          version = bryntum.gantt.VersionHelper.getVersion(shared.productName),
          isNew = function isNew(example) {
        return version && example.since && version.startsWith(example.since);
      },
          isUpdated = function isUpdated(example) {
        return version && example.updated && version.startsWith(example.updated);
      };

      return data.examples.map(function (example) {
        // Show build popup for examples marked as offline and for those who need building when demo browser is offline
        var hasPopup = example.build && !_this.isOnline || example.offline;
        return "".concat(example.firstInGroup ? "\n                <h2 id=\"".concat(example.group, "\" class=\"group-header ").concat(example.group, "\">").concat(example.group, "</h2>\n                <section class=\"examples\">\n                ") : '', "\n                <a\n                    class=\"example ").concat(isNew(example) ? 'new' : '', " ").concat(isUpdated(example) ? 'updated' : '', "\"\n                    id=\"").concat(example.folder, "\"\n                    href=\"").concat(example.folder, "\"\n                    ").concat(hasPopup ? "data-popup=\"".concat(example.folder, "\"") : '', "\n                    >\n                    <div class=\"image\">\n                        <img src=\"").concat(example.folder, "/meta/thumb.").concat(theme.toLowerCase(), ".png\" alt=\"").concat(example.tooltip || example.title || '', "\">\n                        <i class=\"").concat(hasPopup ? 'tooltip b-fa b-fa-cog build' : 'tooltip b-fa b-fa-info', "\"></i>\n                        ").concat(example.version ? "<div class=\"version\">".concat(example.version, "</div>") : '', "\n                    </div>\n                    <label class=\"title\">").concat(example.title, "</label>\n                </a>\n                ").concat(example.lastInGroup ? '</section>' : '', "\n            ");
      }).join('');
    }
  }, {
    key: "render",
    value: function render() {
      var me = this,
          examples = me.examples,
          groupOrder = {
        'Basic': 0,
        'Intermediate': 1,
        'Advanced': 2,
        'Integration': 3,
        'Integration/Angular': 4,
        'Integration/React': 5,
        'Integration/Vue': 6
      };

      if (me.examplesContainerEl.children.length === 0) {
        examples.sort(function (a, b) {
          var first = groupOrder[a.group] + a.title.trim(),
              second = groupOrder[b.group] + b.title.trim();
          if (first < second) return -1;
          if (first > second) return 1;
          return 0;
        });
        var group = '',
            last = null;
        examples.forEach(function (r) {
          if (r.group !== group) {
            group = r.group;
            r.firstInGroup = true;
            if (last) last.lastInGroup = true;
          }

          last = r;
        });
        me.examplesContainerEl.innerHTML = me.template({
          examples: examples
        });
      } else {
        // We need this to correctly process thumbs in Edge
        me.updateThumbs();
      } // A singleton tooltip which displays example info on hover of (i) icons.


      bryntum.gantt.WidgetHelper.attachTooltip(me.examplesContainerEl, {
        forSelector: 'i.tooltip',
        header: true,
        scrollAction: 'realign',
        textContent: true,
        getHtml: function getHtml(_ref) {
          var tip = _ref.tip;
          var activeTarget = tip.activeTarget;

          if (activeTarget.dataset['tooltip']) {
            tip.titleElement.innerHTML = activeTarget.dataset['tooltipTitle'];
            return activeTarget.dataset['tooltip'];
          }

          var url = "".concat(activeTarget.closest('a').href, "/app.config.json"); // Cancel all ongoing ajax loads (except for the URL we are interested in)
          // before fetching tip content

          for (var u in me.currentTipLoadPromiseByURL) {
            if (u != url) {
              me.currentTipLoadPromiseByURL[u].abort();
            }
          } // if we don't have ongoing requests to the URL


          if (!me.currentTipLoadPromiseByURL[url]) {
            var requestPromise = me.currentTipLoadPromiseByURL[url] = bryntum.gantt.AjaxHelper.get(url, {
              parseJson: true
            });
            requestPromise.then(function (response) {
              var json = response.parsedJson;
              activeTarget.dataset['tooltipTitle'] = tip.titleElement.innerHTML = json.title.replace(/[\n\r]/g, '');
              activeTarget.dataset['tooltip'] = tip.html = json.description.replace(/[\n\r]/g, '') + (/build/.test(activeTarget.className) ? "<br><b>".concat(me.buildTip, "</b>") : '');
              tip.realign();
              delete me.currentTipLoadPromiseByURL[url];
            }).catch(function (reason) {// Ajax request aborted
            });
          }

          return false;
        }
      });
      document.getElementById('intro').style.display = 'block';
      document.getElementById('close-button').addEventListener('click', me.onCloseClick.bind(me));
      bryntum.gantt.EventHelper.on({
        element: me.examplesContainerEl,
        click: function click(event) {
          var el = bryntum.gantt.DomHelper.up(event.target, '[data-popup]');
          new bryntum.gantt.Popup({
            forElement: el,
            header: '<i class="b-fa b-fa-info-circle"></i> ' + (me.isOnline ? 'Download needed' : 'Needs building'),
            html: me.buildTip + "Browse to <b>examples/".concat(el.dataset.popup, "</b> to find it"),
            closeAction: 'destroy',
            width: '18em',
            anchor: true,
            scrollAction: 'realign'
          });
          event.preventDefault();
        },
        delegate: '[data-popup]'
      });
      var demoDiv = document.getElementById('live-example'),
          widgetConfig = window.introWidget; // taken from `examples/_shared/data/examples.js`

      if (demoDiv && widgetConfig) {
        bryntum.gantt.WidgetHelper.append(widgetConfig, 'live-example');
      }

      me.rendered = true;
    }
  }, {
    key: "embedDescriptions",
    value: function embedDescriptions() {
      var _this2 = this;

      return new Promise(function (resolve) {
        var me = _this2,
            promises = me.examples.map(function (example) {
          return bryntum.gantt.AjaxHelper.get(example.folder + '/app.config.json', {
            parseJson: true
          }).then(function (response) {
            if (response.parsedJson) {
              example.tooltip = response.parsedJson.title + ' - ' + response.parsedJson.description.replace(/[\n\r]/g, ' ').replace(/"/g, '\'');
            }
          });
        });
        Promise.all(promises).then(resolve);
      });
    }
  }]);

  return ExamplesApp;
}();

new ExamplesApp();