"use strict";

function _typeof(obj) { "@babel/helpers - typeof"; if (typeof Symbol === "function" && typeof Symbol.iterator === "symbol") { _typeof = function _typeof(obj) { return typeof obj; }; } else { _typeof = function _typeof(obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; }; } return _typeof(obj); }

function asyncGeneratorStep(gen, resolve, reject, _next, _throw, key, arg) { try { var info = gen[key](arg); var value = info.value; } catch (error) { reject(error); return; } if (info.done) { resolve(value); } else { Promise.resolve(value).then(_next, _throw); } }

function _asyncToGenerator(fn) { return function () { var self = this, args = arguments; return new Promise(function (resolve, reject) { var gen = fn.apply(self, args); function _next(value) { asyncGeneratorStep(gen, resolve, reject, _next, _throw, "next", value); } function _throw(err) { asyncGeneratorStep(gen, resolve, reject, _next, _throw, "throw", err); } _next(undefined); }); }; }

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function _createClass(Constructor, protoProps, staticProps) { if (protoProps) _defineProperties(Constructor.prototype, protoProps); if (staticProps) _defineProperties(Constructor, staticProps); return Constructor; }

function _possibleConstructorReturn(self, call) { if (call && (_typeof(call) === "object" || typeof call === "function")) { return call; } return _assertThisInitialized(self); }

function _assertThisInitialized(self) { if (self === void 0) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return self; }

function _get(target, property, receiver) { if (typeof Reflect !== "undefined" && Reflect.get) { _get = Reflect.get; } else { _get = function _get(target, property, receiver) { var base = _superPropBase(target, property); if (!base) return; var desc = Object.getOwnPropertyDescriptor(base, property); if (desc.get) { return desc.get.call(receiver); } return desc.value; }; } return _get(target, property, receiver || target); }

function _superPropBase(object, property) { while (!Object.prototype.hasOwnProperty.call(object, property)) { object = _getPrototypeOf(object); if (object === null) break; } return object; }

function _getPrototypeOf(o) { _getPrototypeOf = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) { return o.__proto__ || Object.getPrototypeOf(o); }; return _getPrototypeOf(o); }

function _inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function"); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, writable: true, configurable: true } }); if (superClass) _setPrototypeOf(subClass, superClass); }

function _setPrototypeOf(o, p) { _setPrototypeOf = Object.setPrototypeOf || function _setPrototypeOf(o, p) { o.__proto__ = p; return o; }; return _setPrototypeOf(o, p); }

/**
 * @module GanttToolbar
 */

/**
 * @extends Core/widget/bryntum.gantt.Toolbar
 */
var GanttToolbar =
/*#__PURE__*/
function (_bryntum$gantt$Toolba) {
  _inherits(GanttToolbar, _bryntum$gantt$Toolba);

  function GanttToolbar() {
    _classCallCheck(this, GanttToolbar);

    return _possibleConstructorReturn(this, _getPrototypeOf(GanttToolbar).apply(this, arguments));
  }

  _createClass(GanttToolbar, [{
    key: "construct",
    value: function construct(config) {
      var me = this,
          gantt = me.gantt = me.owner = config.gantt,
          project = gantt.project;
      project.on({
        load: me.updateStartDateField,
        propagationComplete: me.updateStartDateField,
        thisObj: me
      });
      var stm = project.stm;
      stm.on({
        recordingstop: me.updateUndoRedoButtons,
        restoringstop: me.updateUndoRedoButtons,
        queueReset: me.updateUndoRedoButtons,
        thisObj: me
      });

      _get(_getPrototypeOf(GanttToolbar.prototype), "construct", this).call(this, config);

      me.styleNode = document.createElement('style');
      document.head.appendChild(me.styleNode);
    }
  }, {
    key: "updateUndoRedoButtons",
    value: function updateUndoRedoButtons() {
      var stm = this.gantt.project.stm,
          _this$widgetMap = this.widgetMap,
          undoBtn = _this$widgetMap.undoBtn,
          redoBtn = _this$widgetMap.redoBtn,
          redoCount = stm.length - stm.position;
      undoBtn.badge = stm.position || '';
      redoBtn.badge = redoCount || '';
      undoBtn.disabled = !stm.canUndo;
      redoBtn.disabled = !stm.canRedo;
    }
  }, {
    key: "setAnimationDuration",
    value: function setAnimationDuration(value) {
      var me = this,
          cssText = ".b-animating .b-gantt-task-wrap { transition-duration: ".concat(value / 1000, "s !important; }");
      me.gantt.transitionDuration = value;

      if (me.transitionRule) {
        me.transitionRule.cssText = cssText;
      } else {
        me.transitionRule = bryntum.gantt.CSSHelper.insertRule(cssText);
      }
    }
  }, {
    key: "updateStartDateField",
    value: function updateStartDateField() {
      var startDateField = this.widgetMap.startDateField;
      startDateField.value = this.gantt.project.startDate; // This handler is called on project.load/propagationComplete, so now we have the
      // initial start date. Prior to this time, the empty (default) value would be
      // flagged as invalid.

      startDateField.required = true;
    } // region controller methods

  }, {
    key: "onAddTaskClick",
    value: function () {
      var _onAddTaskClick = _asyncToGenerator(
      /*#__PURE__*/
      regeneratorRuntime.mark(function _callee() {
        var gantt, added;
        return regeneratorRuntime.wrap(function _callee$(_context) {
          while (1) {
            switch (_context.prev = _context.next) {
              case 0:
                gantt = this.gantt, added = gantt.taskStore.rootNode.appendChild({
                  name: 'New task',
                  duration: 1
                }); // run propagation to calculate new task fields

                _context.next = 3;
                return gantt.project.propagate();

              case 3:
                _context.next = 5;
                return gantt.scrollRowIntoView(added);

              case 5:
                gantt.features.cellEdit.startEditing({
                  record: added,
                  field: 'name'
                });

              case 6:
              case "end":
                return _context.stop();
            }
          }
        }, _callee, this);
      }));

      function onAddTaskClick() {
        return _onAddTaskClick.apply(this, arguments);
      }

      return onAddTaskClick;
    }()
  }, {
    key: "onEditTaskClick",
    value: function onEditTaskClick() {
      var gantt = this.gantt;

      if (gantt.selectedRecord) {
        gantt.editTask(gantt.selectedRecord);
      } else {
        bryntum.gantt.Toast.show('First select the task you want to edit');
      }
    }
  }, {
    key: "onExpandAllClick",
    value: function onExpandAllClick() {
      this.gantt.expandAll();
    }
  }, {
    key: "onCollapseAllClick",
    value: function onCollapseAllClick() {
      this.gantt.collapseAll();
    }
  }, {
    key: "onZoomInClick",
    value: function onZoomInClick() {
      this.gantt.zoomIn();
    }
  }, {
    key: "onZoomOutClick",
    value: function onZoomOutClick() {
      this.gantt.zoomOut();
    }
  }, {
    key: "onZoomToFitClick",
    value: function onZoomToFitClick() {
      this.gantt.zoomToFit({
        leftMargin: 50,
        rightMargin: 50
      });
    }
  }, {
    key: "onShiftPreviousClick",
    value: function onShiftPreviousClick() {
      this.gantt.shiftPrevious();
    }
  }, {
    key: "onShiftNextClick",
    value: function onShiftNextClick() {
      this.gantt.shiftNext();
    }
  }, {
    key: "onStartDateChange",
    value: function onStartDateChange(_ref) {
      var value = _ref.value,
          oldValue = _ref.oldValue;

      if (!oldValue) {
        // ignore initial set
        return;
      }

      this.gantt.startDate = bryntum.gantt.DateHelper.add(value, -1, 'week');
      this.gantt.project.setStartDate(value);
    }
  }, {
    key: "onFilterChange",
    value: function onFilterChange(_ref2) {
      var value = _ref2.value;

      if (value === '') {
        this.gantt.taskStore.clearFilters();
      } else {
        value = value.replace(/[.*+?^${}()|[\]\\]/g, '\\$&');
        this.gantt.taskStore.filter(function (task) {
          return task.name && task.name.match(new RegExp(value, 'i'));
        });
      }
    }
  }, {
    key: "onFeaturesClick",
    value: function onFeaturesClick(_ref3) {
      var item = _ref3.source;
      var gantt = this.gantt;

      if (item.feature) {
        var feature = gantt.features[item.feature];
        feature.disabled = !feature.disabled;
      } else if (item.subGrid) {
        var subGrid = gantt.subGrids[item.subGrid];
        subGrid.collapsed = !subGrid.collapsed;
      }
    }
  }, {
    key: "onFeaturesShow",
    value: function onFeaturesShow(_ref4) {
      var menu = _ref4.source;
      var gantt = this.gantt;
      menu.items.map(function (item) {
        var feature = item.feature;

        if (feature) {
          // a feature might be not presented in the gantt
          // (the code is shared between "advanced" and "php" demos which use a bit different set of features)
          if (gantt.features[feature]) {
            item.checked = !gantt.features[feature].disabled;
          } // hide not existing features
          else {
              item.hide();
            }
        } else {
          item.checked = gantt.subGrids[item.subGrid].collapsed;
        }
      });
    }
  }, {
    key: "onSettingsShow",
    value: function onSettingsShow(_ref5) {
      var menu = _ref5.source;
      var gantt = this.gantt,
          widgetMap = menu.widgetMap;
      widgetMap.rowHeight.value = gantt.rowHeight;
      widgetMap.barMargin.value = gantt.barMargin;
      widgetMap.barMargin.max = gantt.rowHeight / 2 - 5;
      widgetMap.duration.value = gantt.transitionDuration;
    }
  }, {
    key: "onSettingsRowHeightChange",
    value: function onSettingsRowHeightChange(_ref6) {
      var value = _ref6.value;
      this.gantt.rowHeight = value;
      this.widgetMap.settingsButton.menu.widgetMap.barMargin.max = value / 2 - 5;
    }
  }, {
    key: "onSettingsMarginChange",
    value: function onSettingsMarginChange(_ref7) {
      var value = _ref7.value;
      this.gantt.barMargin = value;
    }
  }, {
    key: "onSettingsDurationChange",
    value: function onSettingsDurationChange(_ref8) {
      var value = _ref8.value;
      this.gantt.transitionDuration = value;
      this.styleNode.innerHTML = ".b-animating .b-gantt-task-wrap { transition-duration: ".concat(value / 1000, "s !important; }");
    }
  }, {
    key: "onCriticalPathsClick",
    value: function onCriticalPathsClick(_ref9) {
      var source = _ref9.source;
      this.gantt.features.criticalPaths.disabled = !source.pressed;
    }
  }, {
    key: "onUndoClick",
    value: function onUndoClick() {
      this.gantt.project.stm.canUndo && this.gantt.project.stm.undo();
    }
  }, {
    key: "onRedoClick",
    value: function onRedoClick() {
      this.gantt.project.stm.canRedo && this.gantt.project.stm.redo();
    } // endregion

  }], [{
    key: "$name",
    get: function get() {
      return 'GanttToolbar';
    }
  }, {
    key: "defaultConfig",
    get: function get() {
      return {
        items: [{
          type: 'buttonGroup',
          items: [{
            type: 'button',
            color: 'b-green',
            ref: 'addTaskButton',
            icon: 'b-fa b-fa-plus',
            text: 'Create',
            tooltip: 'Create new task',
            onAction: 'up.onAddTaskClick'
          }]
        }, {
          type: 'buttonGroup',
          items: [{
            type: 'button',
            color: 'b-blue',
            ref: 'editTaskButton',
            icon: 'b-fa b-fa-pen',
            text: 'Edit',
            tooltip: 'Edit selected task',
            onAction: 'up.onEditTaskClick'
          }, {
            type: 'button',
            color: 'b-blue',
            ref: 'undoBtn',
            icon: 'b-icon b-fa b-fa-undo',
            tooltip: 'Undo',
            disabled: true,
            width: '2em',
            onAction: 'up.onUndoClick'
          }, {
            type: 'button',
            color: 'b-blue',
            ref: 'redoBtn',
            icon: 'b-icon b-fa b-fa-redo',
            tooltip: 'Redo',
            disabled: true,
            width: '2em',
            onAction: 'up.onRedoClick'
          }]
        }, {
          type: 'buttonGroup',
          items: [{
            type: 'button',
            color: 'b-blue',
            ref: 'expandAllButton',
            icon: 'b-fa b-fa-angle-double-down',
            tooltip: 'Expand all',
            onAction: 'up.onExpandAllClick'
          }, {
            type: 'button',
            color: 'b-blue',
            ref: 'collapseAllButton',
            icon: 'b-fa b-fa-angle-double-up',
            tooltip: 'Collapse all',
            onAction: 'up.onCollapseAllClick'
          }]
        }, {
          type: 'buttonGroup',
          items: [{
            type: 'button',
            color: 'b-blue',
            ref: 'zoomInButton',
            icon: 'b-fa b-fa-search-plus',
            tooltip: 'Zoom in',
            onAction: 'up.onZoomInClick'
          }, {
            type: 'button',
            color: 'b-blue',
            ref: 'zoomOutButton',
            icon: 'b-fa b-fa-search-minus',
            tooltip: 'Zoom out',
            onAction: 'up.onZoomOutClick'
          }, {
            type: 'button',
            color: 'b-blue',
            ref: 'zoomToFitButton',
            icon: 'b-fa b-fa-compress-arrows-alt',
            tooltip: 'Zoom to fit',
            onAction: 'up.onZoomToFitClick'
          }, {
            type: 'button',
            color: 'b-blue',
            ref: 'previousButton',
            icon: 'b-fa b-fa-angle-left',
            tooltip: 'Previous time span',
            onAction: 'up.onShiftPreviousClick'
          }, {
            type: 'button',
            color: 'b-blue',
            ref: 'nextButton',
            icon: 'b-fa b-fa-angle-right',
            tooltip: 'Next time span',
            onAction: 'up.onShiftNextClick'
          }]
        }, {
          type: 'buttonGroup',
          items: [{
            type: 'button',
            color: 'b-blue',
            ref: 'featuresButton',
            icon: 'b-fa b-fa-tasks',
            text: 'Features',
            tooltip: 'Toggle features',
            toggleable: true,
            menu: {
              onItem: 'up.onFeaturesClick',
              onBeforeShow: 'up.onFeaturesShow',
              items: [{
                text: 'Draw dependencies',
                feature: 'dependencies',
                checked: false
              }, {
                text: 'Task labels',
                feature: 'labels',
                checked: false
              }, {
                text: 'Project lines',
                feature: 'projectLines',
                checked: false
              }, {
                text: 'Highlight non-working time',
                feature: 'nonWorkingTime',
                checked: false
              }, {
                text: 'Enable cell editing',
                feature: 'cellEdit',
                checked: false
              }, {
                text: 'Show baselines',
                feature: 'baselines',
                checked: false
              }, {
                text: 'Show rollups',
                feature: 'rollups',
                checked: false
              }, {
                text: 'Show progress line',
                feature: 'progressLine',
                checked: false
              }, {
                text: 'Hide schedule',
                cls: 'b-separator',
                subGrid: 'normal',
                checked: false
              }]
            }
          }, {
            type: 'button',
            color: 'b-blue',
            ref: 'settingsButton',
            icon: 'b-fa b-fa-cogs',
            text: 'Settings',
            tooltip: 'Adjust settings',
            toggleable: true,
            menu: {
              type: 'popup',
              anchor: true,
              layoutStyle: {
                flexDirection: 'column'
              },
              onBeforeShow: 'up.onSettingsShow',
              items: [{
                type: 'slider',
                ref: 'rowHeight',
                text: 'Row height',
                width: '12em',
                showValue: true,
                min: 30,
                max: 70,
                style: 'margin-bottom: .5em',
                onInput: 'up.onSettingsRowHeightChange'
              }, {
                type: 'slider',
                ref: 'barMargin',
                text: 'Bar margin',
                width: '12em',
                showValue: true,
                min: 0,
                max: 10,
                onInput: 'up.onSettingsMarginChange'
              }, {
                type: 'slider',
                ref: 'duration',
                text: 'Animation duration ',
                width: '12em',
                min: 0,
                max: 2000,
                step: 100,
                showValue: true,
                onInput: 'up.onSettingsDurationChange'
              }]
            }
          }, {
            type: 'button',
            color: 'b-blue',
            ref: 'criticalPathsButton',
            icon: 'b-fa b-fa-fire',
            text: 'Critical paths',
            tooltip: 'Highlight critical paths',
            toggleable: true,
            onAction: 'up.onCriticalPathsClick'
          }]
        }, {
          type: 'datefield',
          ref: 'startDateField',
          label: 'Project start',
          // required  : true, (done on load)
          flex: '1 2 17em',
          listeners: {
            change: 'up.onStartDateChange'
          }
        }, {
          type: 'textfield',
          ref: 'filterByName',
          cls: 'filter-by-name',
          flex: '1 1 12.5em',
          // Label used for material, hidden in other themes
          label: 'Find tasks by name',
          // Placeholder for others
          placeholder: 'Find tasks by name',
          clearable: true,
          keyStrokeChangeDelay: 100,
          triggers: {
            filter: {
              align: 'end',
              cls: 'b-fa b-fa-filter'
            }
          },
          onChange: 'up.onFilterChange'
        }]
      };
    }
  }]);

  return GanttToolbar;
}(bryntum.gantt.Toolbar);

;
/**
 * @module PercentDonePieColumn
 */

/**
 * A column drawing a pie chart of the `percentDone` value
 *
 * @extends bryntum.gantt.Gantt/column/bryntum.gantt.PercentDoneColumn
 * @classType percentdonepie
 */

var PercentDonePieColumn =
/*#__PURE__*/
function (_bryntum$gantt$Percen) {
  _inherits(PercentDonePieColumn, _bryntum$gantt$Percen);

  function PercentDonePieColumn() {
    _classCallCheck(this, PercentDonePieColumn);

    return _possibleConstructorReturn(this, _getPrototypeOf(PercentDonePieColumn).apply(this, arguments));
  }

  _createClass(PercentDonePieColumn, [{
    key: "renderer",
    //endregion
    value: function renderer(_ref10) {
      var value = _ref10.value;
      return value ? {
        className: 'b-pie',
        style: {
          animationDelay: "-".concat(value, "s")
        }
      } : '';
    }
  }], [{
    key: "type",
    get: function get() {
      return 'percentdonepie';
    }
  }, {
    key: "isGanttColumn",
    get: function get() {
      return true;
    }
  }, {
    key: "defaults",
    get: function get() {
      return {
        // Set your default instance config properties here
        htmlEncode: false,
        align: 'center'
      };
    }
  }]);

  return PercentDonePieColumn;
}(bryntum.gantt.PercentDoneColumn);

bryntum.gantt.ColumnStore.registerColumnType(PercentDonePieColumn);
/**
 * @module ResourceAvatarColumn
 */

var defaultImagePath = '../_shared/images/users/';
/**
 * bryntum.gantt.Column showing avatars of the assigned resource
 *
 * @extends Grid/column/bryntum.gantt.Column
 * @classType resourceavatar
 */

var ResourceAvatarColumn =
/*#__PURE__*/
function (_bryntum$gantt$Resour) {
  _inherits(ResourceAvatarColumn, _bryntum$gantt$Resour);

  function ResourceAvatarColumn() {
    _classCallCheck(this, ResourceAvatarColumn);

    return _possibleConstructorReturn(this, _getPrototypeOf(ResourceAvatarColumn).apply(this, arguments));
  }

  _createClass(ResourceAvatarColumn, [{
    key: "renderer",
    value: function renderer(_ref11) {
      var _this = this;

      var value = _ref11.value;
      var imgSize = 30,
          nbrVisible = Math.floor((this.width - 20) / (imgSize + 2)),
          lastIndex = nbrVisible - 1,
          overflowCount = value.length - nbrVisible;
      return '<div class="b-resource-avatar-container">' + Array.from(value).map(function (assignment, i) {
        var resource = assignment.resource;
        var markup = '';

        if (resource && i < nbrVisible) {
          var isLastOverflowing = i === lastIndex && overflowCount > 0,
              imgBtip = "".concat(resource.name, " ").concat(assignment.units, "%").concat(isLastOverflowing ? " (+".concat(overflowCount, " more resources)") : ''),
              imgMarkup = "<img data-btip-scroll-action=\"realign\" data-btip=\"".concat(imgBtip, "\" class=\"b-resource-avatar\" src=\"").concat(_this.imageFolderPath).concat(resource.name.toLowerCase() || 'none', ".jpg\">");

          if (isLastOverflowing) {
            markup = "<div class=\"b-overflow-img\">".concat(imgMarkup, "<span class=\"b-overflow-count\">+").concat(overflowCount, "</span></div>");
          } else {
            markup = imgMarkup;
          }
        }

        return markup;
      }).join('') + '</div>';
    }
  }], [{
    key: "type",
    get: function get() {
      return 'resourceavatar';
    }
  }, {
    key: "isGanttColumn",
    get: function get() {
      return true;
    }
  }, {
    key: "fields",
    get: function get() {
      return [
      /**
       * The path for the resource images
       * @config {String}
       * @category Common
       */
      'imageFolderPath'];
    }
  }, {
    key: "defaults",
    get: function get() {
      return {
        repaintOnResize: true,
        htmlEncode: false,
        cellCls: 'b-resource-avatar-cell',
        imageFolderPath: defaultImagePath,
        autoSyncHtml: true
      };
    }
  }]);

  return ResourceAvatarColumn;
}(bryntum.gantt.ResourceAssignmentColumn);

bryntum.gantt.ColumnStore.registerColumnType(ResourceAvatarColumn);
/**
 * @module StatusColumn
 */

/**
 * A column showing the status of a task
 *
 * @extends bryntum.gantt.Gantt/column/bryntum.gantt.Column
 * @classType statuscolumn
 */

var StatusColumn =
/*#__PURE__*/
function (_bryntum$gantt$Column) {
  _inherits(StatusColumn, _bryntum$gantt$Column);

  function StatusColumn() {
    _classCallCheck(this, StatusColumn);

    return _possibleConstructorReturn(this, _getPrototypeOf(StatusColumn).apply(this, arguments));
  }

  _createClass(StatusColumn, [{
    key: "renderer",
    //endregion
    value: function renderer(_ref12) {
      var record = _ref12.record;
      var status = '';

      if (record.isCompleted) {
        status = 'Completed';
      } else if (record.endDate < Date.now()) {
        status = 'Late';
      } else if (record.isStarted) {
        status = 'Started';
      }

      return status ? {
        tag: 'i',
        className: "b-fa b-fa-circle ".concat(status),
        html: status
      } : '';
    }
  }], [{
    key: "type",
    get: function get() {
      return 'statuscolumn';
    }
  }, {
    key: "isGanttColumn",
    get: function get() {
      return true;
    }
  }, {
    key: "defaults",
    get: function get() {
      return {
        // Set your default instance config properties here
        text: 'Status',
        editor: false,
        cellCls: 'b-status-column-cell',
        htmlEncode: false
      };
    }
  }]);

  return StatusColumn;
}(bryntum.gantt.Column);

bryntum.gantt.ColumnStore.registerColumnType(StatusColumn); // here you can extend our default Task class with your additional fields, methods and logic

var Task =
/*#__PURE__*/
function (_bryntum$gantt$TaskMo) {
  _inherits(Task, _bryntum$gantt$TaskMo);

  function Task() {
    _classCallCheck(this, Task);

    return _possibleConstructorReturn(this, _getPrototypeOf(Task).apply(this, arguments));
  }

  _createClass(Task, [{
    key: "isLate",
    get: function get() {
      return this.deadline && Date.now() > this.deadline;
    }
  }], [{
    key: "fields",
    get: function get() {
      return [{
        name: 'deadline',
        type: 'date'
      }];
    }
  }]);

  return Task;
}(bryntum.gantt.TaskModel);
/* eslint-disable no-unused-vars */


var project = new bryntum.gantt.ProjectModel({
  // Let the Project know we want to use our own Task model with custom fields / methods
  taskModelClass: Task,
  transport: {
    load: {
      url: '../_datasets/launch-saas.json'
    }
  }
});
var gantt = new bryntum.gantt.Gantt({
  project: project,
  startDate: '2019-01-12',
  endDate: '2019-03-24',
  columns: [{
    type: 'wbs'
  }, {
    type: 'name',
    width: 250
  }, {
    type: 'startdate'
  }, {
    type: 'duration'
  }, {
    type: 'resourceavatar',
    width: 120
  }, {
    type: 'percentdone',
    showCircle: true,
    width: 70
  }, {
    type: 'predecessor',
    width: 112
  }, {
    type: 'successor',
    width: 112
  }, {
    type: 'schedulingmodecolumn'
  }, {
    type: 'calendar'
  }, {
    type: 'constrainttype'
  }, {
    type: 'constraintdate'
  }, {
    type: 'statuscolumn'
  }, {
    type: 'date',
    text: 'Deadline',
    field: 'deadline'
  }, {
    type: 'addnew'
  }],
  subGridConfigs: {
    locked: {
      flex: 3
    },
    normal: {
      flex: 4
    }
  },
  columnLines: false,
  features: {
    rollups: {
      disabled: true
    },
    baselines: {
      disabled: true
    },
    progressLine: {
      disabled: true,
      statusDate: new Date(2019, 0, 25)
    },
    taskContextMenu: {
      // Our items is merged with the provided defaultItems
      // So we add the provided convertToMilestone option.
      items: {
        convertToMilestone: true
      },
      processItems: function processItems(_ref13) {
        var taskRecord = _ref13.taskRecord,
            items = _ref13.items;

        if (taskRecord.isMilestone) {
          items.convertToMilestone = false;
        }
      }
    },
    filter: true,
    dependencyEdit: true,
    timeRanges: {
      showCurrentTimeLine: true
    },
    labels: {
      left: {
        field: 'name',
        editor: {
          type: 'textfield'
        }
      }
    }
  }
});
var panel = new bryntum.gantt.Panel({
  adopt: 'container',
  layout: 'fit',
  items: [gantt],
  tbar: new GanttToolbar({
    gantt: gantt
  })
}); // console.time("load data");

project.load().then(function () {
  // console.timeEnd("load data");
  var stm = gantt.project.stm;
  stm.enable();
  stm.autoRecord = true; // let's track scheduling conflicts happened

  project.on('schedulingconflict', function (context) {
    // show notification to user
    bryntum.gantt.Toast.show('Scheduling conflict has happened ..recent changes were reverted'); // as the conflict resolution approach let's simply cancel the changes

    context.continueWithResolutionResult(bryntum.gantt.EffectResolutionResult.Cancel);
  });
});