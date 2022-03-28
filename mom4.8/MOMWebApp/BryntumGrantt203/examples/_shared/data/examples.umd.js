"use strict";

/**
 * folder  : example folder under ./examples
 * group   : browser page group
 * title   : example title
 * build   : true if example needs building
 * offline : true if example is not available online
 * since   : package version since example is available
 * ie      : false if not supported for IE
 * edge    : false if not supported for Edge
 */
// eslint-disable-next-line no-unused-vars
var examples = [{
  folder: 'advanced',
  group: 'Advanced',
  title: 'Advanced'
}, {
  folder: 'aggregation-column',
  group: 'Basic',
  title: 'Aggregation column'
}, {
  folder: 'exporttoexcel',
  group: 'Advanced',
  title: 'Export to Excel'
}, {
  folder: 'export',
  group: 'Advanced',
  title: 'Export to PDF/PNG',
  since: '2.0'
}, {
  folder: 'angular/advanced',
  group: 'Integration/Angular',
  title: 'Angular Advanced demo',
  version: 'Angular 7',
  build: true
}, {
  folder: 'angular/taskeditor',
  group: 'Integration/Angular',
  title: 'Task editor customization Angular demo',
  version: 'Angular 7',
  build: true,
  since: '1.0.2'
}, {
  folder: 'angular/pdf-export',
  group: 'Integration/Angular',
  title: 'Angular PDF/PNG //export demo',
  version: 'Angular 8',
  build: true,
  since: '2.0'
}, {
  folder: 'angular/rollups',
  group: 'Integration/Angular',
  title: 'Rollups Angular demo',
  version: 'Angular 8',
  build: true,
  since: '2.0.1'
}, {
  folder: 'angular/timeranges',
  group: 'Integration/Angular',
  title: 'Time ranges Angular demo',
  version: 'Angular 8',
  build: true,
  since: '2.0.1'
}, {
  folder: 'baselines',
  group: 'Basic',
  title: 'Baselines demo',
  since: '1.1.0'
}, {
  folder: 'basic',
  group: 'Basic',
  title: 'Basic'
}, {
  folder: 'criticalpaths',
  group: 'Basic',
  title: 'Critical paths demo',
  since: '1.1.0'
}, {
  folder: 'custom-build',
  group: 'Integration',
  title: 'Custom build using WebPack',
  version: 'WebPack 4',
  since: '1.2',
  offline: true
}, // { folder : 'bigdataset', group : 'Intermediate', title : 'Big data set' },
{
  folder: 'esmodule',
  group: 'Integration',
  title: 'Include using EcmaScript module'
}, {
  folder: 'extjsmodern',
  group: 'Integration',
  title: 'Ext JS Modern demo',
  since: '1.0.2'
}, {
  folder: 'labels',
  group: 'Basic',
  title: 'Labels'
}, {
  folder: 'localization',
  group: 'Intermediate',
  title: 'Localization'
}, {
  folder: 'msprojectimport',
  group: 'Advanced',
  title: 'Import project data from MPP files',
  since: '1.1.3'
}, {
  folder: 'php',
  group: 'Integration',
  title: 'Backend in PHP',
  offline: true
}, {
  folder: 'progressline',
  group: 'Basic',
  title: 'Progress line demo',
  since: '1.1.0'
}, {
  folder: 'react/javascript/advanced',
  group: 'Integration/React',
  title: 'React Advanced demo',
  version: 'React 16',
  build: true
}, {
  folder: 'react/javascript/basic',
  group: 'Integration/React',
  title: 'React Basic demo',
  version: 'React 16',
  build: true,
  since: '1.1.2',
  updated: '1.3'
}, {
  folder: 'react/javascript/pdf-export',
  group: 'Integration/React',
  title: 'React PDF/PNG //export demo',
  version: 'React 16',
  build: true,
  since: '2.0'
}, {
  folder: 'react/javascript/rollups',
  group: 'Integration/React',
  title: 'React Rollups demo',
  version: 'React 16.12',
  build: true,
  since: '2.0.1'
}, {
  folder: 'react/javascript/taskeditor',
  group: 'Integration/React',
  title: 'React Task editor customization demo',
  version: 'React 16.12',
  build: true,
  since: '2.0.1'
}, {
  folder: 'react/javascript/timeranges',
  group: 'Integration/React',
  title: 'React Time ranges demo',
  version: 'React 16.12',
  build: true,
  since: '2.0.1'
}, {
  folder: 'react/typescript/basic',
  group: 'Integration/React',
  title: 'React Basic demo with TypeScript',
  version: 'React 16',
  build: true,
  since: '1.1.3'
}, {
  folder: 'requirejs',
  group: 'Integration',
  title: 'Include with RequireJS'
}, {
  folder: 'resourceassignment',
  group: 'Intermediate',
  title: 'Customizing the resource assignment picker',
  since: '1.0.1'
}, {
  folder: 'responsive',
  group: 'Basic',
  title: 'Responsive'
}, {
  folder: 'rollups',
  group: 'Basic',
  title: 'Rollups demo',
  since: '1.2.0'
}, {
  folder: 'scripttag',
  group: 'Integration',
  title: 'Include using a script tag'
}, {
  folder: 'taskcontextmenu',
  group: 'Intermediate',
  title: 'Customizing the task context menu'
}, {
  folder: 'taskeditor',
  group: 'Intermediate',
  title: 'Customizing the task editor'
}, {
  folder: 'theme',
  group: 'Basic',
  title: 'Theme browser'
}, {
  folder: 'timeline',
  group: 'Intermediate',
  title: 'The timeline widget'
}, {
  folder: 'timeranges',
  group: 'Basic',
  title: 'Time Ranges',
  updated: '1.3'
}, {
  folder: 'tooltips',
  group: 'Basic',
  title: 'Tooltips',
  since: '1.1.4'
}, {
  folder: 'undoredo',
  group: 'Intermediate',
  title: 'Undo/Redo'
}, {
  folder: 'vue/javascript/advanced',
  group: 'Integration/Vue',
  title: 'Vue Advanced demo',
  version: 'Vue 2',
  build: true
}, {
  folder: 'vue/javascript/pdf-export',
  group: 'Integration/Vue',
  title: 'Vue PDF/PNG //export demo',
  version: 'Vue 2',
  build: true,
  since: '2.0'
}, {
  folder: 'vue/javascript/rollups',
  group: 'Integration/Vue',
  title: 'Vue Rollups demo',
  version: 'Vue 2',
  build: true,
  since: '2.0.1'
}, {
  folder: 'vue/javascript/taskeditor',
  group: 'Integration/Vue',
  title: 'Task editor Vue customization demo',
  version: 'Vue 2',
  build: true,
  since: '2.0.1'
}, {
  folder: 'vue/javascript/timeranges',
  group: 'Integration/Vue',
  title: 'Time ranges Vue demo',
  version: 'Vue 2',
  build: true,
  since: '2.0.1'
}, {
  folder: 'webcomponents',
  group: 'Integration',
  title: 'Use as web component (WebKit only)',
  ie: false,
  edge: false,
  since: '1.0.2'
}]; // Filter out demos by browser

var isIE11 = /rv:11/.test(navigator.userAgent),
    isEdge = /Edge/.test(navigator.userAgent),
    browser = isIE11 ? 'ie' : isEdge ? 'edge' : null;

if (browser) {
  window.examples = window.examples.filter(function (e) {
    return e[browser] !== false;
  });
} // eslint-disable-next-line no-unused-vars


window.introWidget = {
  type: 'gantt',
  startDate: new Date(2018, 3, 1),
  endDate: new Date(2018, 4, 1),
  viewPreset: 'weekAndDayLetter',
  minHeight: 250,
  columns: [{
    type: 'name',
    text: 'Name',
    width: 220
  }],
  tasks: [{
    name: 'My Project',
    expanded: true,
    startDate: '2018-04-02',
    percentDone: 50,
    duration: 11,
    children: [{
      id: 1,
      name: 'Develop New Cool Product',
      percentDone: 50,
      startDate: '2018-04-02',
      duration: 5
    }, {
      id: 2,
      name: 'Launch Product',
      percentDone: 50,
      startDate: '2018-04-09',
      duration: 4
    }]
  }],
  dependencies: [{
    id: 1,
    fromEvent: 1,
    toEvent: 2
  }]
};