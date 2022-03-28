<%@ Page Language="C#" AutoEventWireup="true" Inherits="RecContractsModule" EnableEventValidation="false" Codebehind="RecContractsModule.aspx.cs" %>


<%--<%@ Register Assembly="CrystalDecisions.Web, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>--%>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagPrefix="asp" Namespace="Saplin.Controls" Assembly="DropDownCheckBoxes" %>
<%--<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">--%>
    <%--<script src="js/ResizeColumnsGrid/colResizable-1.5.min.js" type="text/javascript"></script>--%>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
  <meta charset="utf-8" />
    <title>Mobile Office Manager 4.0</title>
    <script src="js/jquery-1.7.1.js"></script>

    <!-- BEGIN GLOBAL MANDATORY STYLES -->
    <link href="http://fonts.googleapis.com/css?family=Open+Sans:400,300,600,700&subset=all" rel="stylesheet" type="text/css" />
    <link href="Appearance/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="Appearance/css/simple-line-icons.min.css" rel="stylesheet" type="text/css" />
    <link href="Appearance/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="Appearance/css/uniform.default.css" rel="stylesheet" type="text/css" />
    <link href="Appearance/css/bootstrap-switch.min.css" rel="stylesheet" type="text/css" />

   
    <link href="Appearance/css/daterangepicker-bs3.css" rel="stylesheet" type="text/css" />
    <link href="Appearance/css/fullcalendar.min.css" rel="stylesheet" type="text/css" />
  
  <link href="Appearance/css/tasks.css" rel="stylesheet" type="text/css" />
  
    <link href="Appearance/css/components.css" id="style_components" rel="stylesheet" type="text/css" />
    <link href="Appearance/css/plugins.css" rel="stylesheet" type="text/css" />
    <link href="Appearance/css/layout.css" rel="stylesheet" type="text/css" />
    <link href="Appearance/css/darkblue.css" rel="stylesheet" type="text/css" id="style_color" />
    <link href="Appearance/css/custom.css" rel="stylesheet" type="text/css" />
    
    <link type="text/css" href="css/smoothness/jquery-ui-1.8.17.custom.css" rel="stylesheet" />
   
    <script src="Appearance/js/jquery.min.js" type="text/javascript"></script>
    <script src="Appearance/js/jquery-ui.min.js"></script>
 
    <script type="text/javascript" src="js/ui/jquery-ui-1.8.17.custom.min.js"></script>

    <script type="text/javascript" src="js/ui/jquery.ui.autocomplete.min.js"></script>

    <script type="text/javascript" src="js/RowClick.js"></script>

    <script type="text/javascript" src="js/Notifyjs/jquery.noty.js"></script>

    <script type="text/javascript" src="js/Notifyjs/themes/default.js"></script>

    <script type="text/javascript" src="js/Notifyjs/layouts/topCenter.js"></script>

    <link rel="shortcut icon" href="favicon.ico" />

    <style>

         /*input, textarea, select { -webkit-appearance:none; -moz-appearance:none;  -o-appearance:none; appearance:none; font-family:Arial, Helvetica, sans-serif; font-size:16px; border-radius:0; -moz-border-radius:0; -webkit-border-radius:0; outline:0;}
input[type=checkbox]{ -webkit-appearance:checkbox; -moz-appearance:checkbox; appearance:checkbox;}*/
body {background-color:#fff !important; background-repeat: no-repeat; background-position:center 275px; font-size:12px}
.page-header.navbar {background:#fff; height:52px; min-height:52px; border-bottom: 1px solid #ccc; }
.page-header.navbar .page-logo .logo-default {width:230px; margin:7px 0;}
.dropdown-extended > a {padding:7px 5px 4px !important; margin:10px 0 0 20px !important; background:#009dc7;}
.dropdown-extended > a i {font-size:17px !important; color:#fff !important;}
.dropdown-extended > a .badge {background: #fff  !important; border: 2px solid #2f373e; color: #2f373e !important; font-size: 11px !important; font-weight: bold !important;  height: 20px;  right: -10px !important;  top: -5px !important; padding: 3px 5px !important;}
.dropdown-extended > a:hover {background:#316b9d !important;}
.page-header.navbar .top-menu .navbar-nav > li.dropdown.open .dropdown-toggle { background-color: #316b9d;}
.page-header.navbar .top-menu .navbar-nav > li:last-child a {background:none !important;}
.page-container {min-height:600px}
.page-container-bg-solid .page-bar {background:#ebebeb;}
.page-header.navbar .top-menu .navbar-nav > li.dropdown-user .dropdown-toggle {display:inline-block; padding: 14px 6px 0 8px;}
.page-header.navbar .top-menu .navbar-nav > li.dropdown-user .dropdown-toggle > img {border-radius:3px !important; margin-right:0; height:auto; width:35px}
.page-header-fixed .page-container {margin-top:52px;}
.page-sidebar-closed.page-sidebar-closed-hide-logo .page-header.navbar .page-logo .logo-default {display:block; margin-left:20px;}
.page-container-bg-solid .page-sidebar .page-sidebar-menu > li.active > a > .selected, .page-container-bg-solid .page-sidebar-closed.page-sidebar-fixed .page-sidebar:hover .page-sidebar-menu > li.active > a > .selected {border-color: transparent #ebebeb transparent transparent;}
.page-sidebar .page-sidebar-menu .sub-menu li, .page-sidebar-closed.page-sidebar-fixed .page-sidebar:hover .page-sidebar-menu .sub-menu li {background:#fff; margin-top:0 !important;}
.page-container-bg-solid .page-content {background:#ebebeb;}
.cont-top {float:left; width:100%;}
.cont-top .page-title {float:left; margin-top:0;}

.page-bar {background:none;}
*
{
    margin: 0px;
    padding: 0px;
    outline: none;
    list-style: none;
}

/*------ edit-location-location-info -------*/
.page-cont-top {float:left; width:100%;}
.page-breadcrumb {display: inline-block;  float: left;  list-style: outside none none;  margin:3px 0 0 0;  padding:0}
.page-breadcrumb > li {  display: inline-block;}
.page-breadcrumb > li > i { color: #aaa;  font-size: 13px;  text-shadow: none;}
.page-breadcrumb > li > a { color: #888; font-size: 13px;  text-shadow: none;}
.page-breadcrumb > li > span { color: #316b9d; font-size: 13px;  text-shadow: none;}
.page-breadcrumb > li:hover > a, .page-breadcrumb > li:hover > i { color: #588ebd;}

.page-bar-right {float:right;}
.page-bar-right a {background:#fff; padding:3px 6px; margin-left:10px; display:inline-block; border-radius:2px !important;}
.page-bar-right a i {font-size:16px !important;}
.page-bar-right .pbr-save i {color:#58e345;}
.page-bar-right .pbr-close i {color:#ff0000;}

.pc-title1 {background:#316b9d; padding:10px 15px; margin-top:15px; font-size:15px; color:#dadedf; line-height:20px !important}
.pc-title {background:#316b9d; padding:7px 15px; font-size:15px; color:#dadedf; line-height:20px !important}
.pc-title span {margin-right:25px;}
.pc-title span i {margin-right:5px; font-size:20px !important; /* vertical-align:middle */}
.pc-title a { display:inline-block; margin-right:5px; color:white; font-size:small }
.pc-title a > i {padding:2px 7px 0 7px; font-size:16px !important; font-weight:bold}
.pc-title a:last-child > i {padding:2px 9px 0}
.pc-title a:hover > i {color:#316b9d; background-color:white}

.edit-tab {float:left; width:100%; background:#fff; padding:20px 0 50px; }
.edit-tab .nav-tabs {margin-bottom:0;}
.edit-tab .nav-tabs > li > a {background:#ebebeb; border-radius:2px 2px 0 0 !important; color:#316b9d; border:1px solid #e1e1e1 !important;}
.edit-tab .nav-tabs > li.active > a, .edit-tab .nav-tabs > li.active > a:focus, .edit-tab .nav-tabs > li.active > a:hover {background:#fff; }

.edit-tab .tab-content {border-top:2px solid #316b9d !important; padding:15px;}
.form-col {float:left; width:100%; margin-bottom:20px;}
.form-col .fc-label {width:122px; margin-right:10px; float:left; margin-top:5px; vertical-align:top; text-align:right;}
.fc-label1 {width:122px; margin-right:10px; float:left; margin-top:5px; vertical-align:top;}
.fc-input1 {float: left; width: calc(100% - 85px);}
.form-col .fc-label label, .form-col .fc-input label {font-size:13px;}
.form-col .fc-input {width:calc(100% - 132px); float:left;}
.form-col .fc-input input, .form-col .fc-input select {font-size:12px; height:30px;} 
.form-col .fc-input textarea {font-size:13px;}
.credit-dispathc {text-align:right !important; display:inline-block; width:122px; margin-right:10px; vertical-align:top }
.credit-dispathc label { font-size:13px;}
.cd-textarea {width:calc(100% - 133px); display:inline-block; float:right}
.form-title {font-size:15px; font-weight:bold; color: #316b9d;}
.form-title1 {font-size:18px; font-weight:bold; color: #316b9d; margin:0 0 10px 0;}

.tab-contact {float:left; width:100%; margin-top:20px;}
.tab-cont-top {background:#316b9d; float:left; width:100%; padding:8px 15px 2px 15px;}
.tab-cont-top h2 {margin:0; font-size:18px; color:#fff; float:left;}
.tab-cont-top ul {float:right; margin: 0; padding:0;}
.tab-cont-top ul li {display:inline-block; list-style:none; margin-left:20px; color:#fff;}
.tab-cont-top ul li:first-child {margin-left:0}
.tab-cont-top ul li span{font-size:14px; color:#fff; line-height:16px}
.tab-cont-top ul li span i {font-size:16px !important; color:#fff; margin-left:2px; }
.tab-cont-top ul li a.btn {padding: 2px 4px;}
.tab-cont-top ul li a:hover, .tab-cont-top ul li a::active  {text-decoration:none}
.portlet-body { clear: both;}
.portlet-body.cont-table .table-bordered > thead > tr > th {background:#316b9d; color:#fff; padding:8px 5px; border:none; }
.portlet-body.cont-table .table-bordered > tbody > tr > td, .portlet-body.cont-table .table-bordered > tbody > tr > th {border:none;padding:8px 5px; }

.portlet-body.cont-table div.checker input {border-bottom:1px solid #b6b6b6;}
.cont-table .table-striped>tbody>tr div.checker input {background:#f7f7f7;}
.cont-table .table-striped>tbody>tr:nth-of-type(odd) div.checker input {background-color:#e9f0f6}
.portlet-body.cont-table .table-striped > tbody > tr.blue-bg {background:#4277a5; color:#fff;}
.portlet-body.cont-table .form-control {border:none; border-bottom:1px solid #b6b6b6;}
.portlet-body.cont-table select.form-control {border-bottom:none; }
.cont-table .table-striped>tbody>tr .form-control {background:#f7f7f7;}
.cont-table .table-striped>tbody>tr:nth-of-type(odd) .form-control {background-color:#e9f0f6}
.portlet-body.cont-table .cont-save {border:none;}
.cont-table .btn {padding: 2px 7px;}

.search-customer {float:left; width:100%;}
.sc-form {float:left;}
.sc-form label {display:inline-block; }
.sc-form .form-control {display:inline-block; margin-left:10px;}
.sc-form a.btn {padding:0; margin-left:5px; background:#316b9d; margin-top:-2px}
.sc-form a.btn i {padding:6px 7px 7px; font-size:16px; color:#fff;}
.sc-form a.btn:hover {background:#588ebd}


.search-customer ul {float:right; margin:8px 0 0 0; padding:0;}
.search-customer ul li {display:inline-block; margin-left:15px;}
.search-customer ul li:first-child {margin-left:0;}
.search-customer ul li span, .search-customer ul li a {color:#787878; font-size:13px;}
.search-customer ul li a:hover, .search-customer ul li a:focus {color:#316b9d; text-decoration:none}

.collapse-box {background: #fff; float: left; margin: 30px 0 0; padding: 15px; width: 100%;}
.tab-cont-top ul li a.collapse {display:block; background:#26a69a url(../images/portlet-collapse-icon-white.png) no-repeat center center ; padding:10px 12px;}
.tab-cont-top ul li a.expand {display:block; background:#26a69a url(../images/portlet-expand-icon-white.png) no-repeat center center ; padding:11px 12px;}
.collapse-box .cont-collaps {float:left; width:100%; float:left; padding:15px 0; }

/*-------- sales -manager-Estimate -----------*/
.recent-activity {float:left; width:100%; }
.ra-title {font-size:16px; color:#fff; background:#4277a5; padding:7px 15px;}
.recent-activity .ra-list {margin:0; padding:10px 10px; background:#f6f6f6; float:left; }
.recent-activity .ra-list li {list-style:none; padding:7px 0; float:left; width:100%;}
.recent-activity .ra-list li i {float:left; width:25px; text-align:left; margin-right:5px; padding-top:5px; color:#464e56}
.recent-activity .ra-list li span {width:calc(100% - 30px); float:left; color:#464e56;} 
.recent-activity .ra-list li a:hover i, .recent-activity .ra-list li a:hover span {color:#588ebd;}

.add-estimate {float:left; width:100%; background:#fff; border:1px solid #8d8d8d;}
.ae-content {float:left; width:100%; padding:15px;}
.cont-table .btn.cont-close {padding: 0 4px; background:#fff;}
.cont-table .btn.cont-close i {color:#ff0000;}
.com-cont {padding:15px; background:#fff;}
.select-chart > div > select {width:170px}
.pnl-codes { width: 450px; z-index: 9999; position: relative;border: 1px solid #316b9d; background-color:white}
.permisson {float:left; width:100%; padding-left:100px}
.permisson .roundCorner {padding-bottom:20px }
.permisson .roundCorner h3 {margin-top:0;    }
.merchant-input .form-control:first-child {width:calc(100% - 41px);   float:left; margin-right:10px }
.merchant-input .form-control:last-child {width:31px; }
.merchant-input .merchant-middle {padding:6px 10px}
.permission-box{ width:50%;}
.white-bg { background:#fff}

.ajax__tab_xp .ajax__tab_header .ajax__tab_inner {background:none !important; padding:0 !important }
.ajax__tab_xp .ajax__tab_header .ajax__tab_outer {height: 40px !important; background:#ebebeb !important; border: 1px solid #e1e1e1 !important; margin-right:2px; padding:0 !important }
.ajax__tab_xp .ajax__tab_header .ajax__tab_tab {padding:9px 15px !important; background:none !important; height:40px !important; color:#316b9d; font-size:14px !important;  }
.ajax__tab_xp .ajax__tab_header .ajax__tab_active .ajax__tab_outer {background:#fff !important }

.ajax__tab_xp .ajax__tab_body {border:none !important; border-top:2px solid #316b9d !important; }
.customizedPanel{
    border-style: solid solid none solid; background-position: #B8E5FC; background: #B8E5FC; width: 100%; height: 25px; color: #23AEE8; font-weight: bold; font-size: 12px; padding-top: 5px; border-width: 1px; border-color: #a9c6c9;
}
.portlet.white {border: 1px solid #316b9d; margin-bottom:20px}
.portlet.box.white > .portlet-title {background:#fff; border-top:4px solid #316b9d; color:#316b9d;}
.portlet.box.white > .portlet-title > .caption > i {color:#316b9d }
.portlet.box.white > .portlet-title > .tools > a.expand { background-image: url(../../Appearance/images/blue-collapse.png); }
.portlet.box.white > .portlet-title > .tools > a.collapse { background-image: url(../../Appearance/images/blue-expand.png); }
.open-calls { position:relative; z-index:9999}
.input-small1 { width: 115px !important; margin-right: 5px;}
.newClassTooltip{ background: #000 none repeat scroll 0 0; filter: alpha(opacity=80); -moz-opacity: 0.80; opacity: 0.80; border-radius: 0px !important; color: #fff; display: none; left: 0px; padding:10px; position: relative;  top: 0px; visibility: hidden; width: 400px;  z-index: 1000;}
.newClassTooltip:after {top: 0%; left: 0%; border: solid transparent; content: " "; height: 0; width: 0; position: absolute; pointer-events: none; border-color: rgba(0, 0, 0, 0); border-top-color: #000; border-width: 10px; margin-left: -10px;  }
.TicketlistTooltip{ background: #000 none repeat scroll 0 0; filter: alpha(opacity=80); -moz-opacity: 0.80; opacity: 0.80; border-radius: 0px !important; color: #fff; display: none;  padding:10px; position: absolute;  width: 300px;  z-index: 1000;}
.TicketlistTooltip:after {top: 0%; left: 0%; border: solid transparent; content: " "; height: 0; width: 0; position: absolute; pointer-events: none; border-color: rgba(0, 0, 0, 0); border-top-color: #000; border-width: 10px; margin-left: -10px;  }
.InvoicelistTooltip{ background: #000 none repeat scroll 0 0; filter: alpha(opacity=80); -moz-opacity: 0.80; opacity: 0.80; border-radius: 0px !important; color: #fff; display: none;  padding:10px; position: absolute;  width: 300px;  z-index: 1000;}

.location-search { background:#316b9d; margin-right:10px;  margin-top: -3px;  padding: 2px 6px; display:inline-block}
.location-search i {color:#fff;}
.location-close {background:#fff; margin-left:0 !important}
.location-close i { color: #ff0000; font-size:16px !important }
.location-find {color:#000; margin-top:5px; display:inline-block}
.model-popup{border:1px solid #316b9d}
.model-popup-body{ background-color: #316b9d; padding: 10px;}
.model-popup-body span{color:white}
.texttransparent {border: none; background: transparent;}
.title_bar_popup{ background-color: #316b9d; padding: 10px;}
.pnlBills .fc-label { width:80px; text-align:left}
.journal-entry-right .fc-label{width:65px; text-align:left}
.journal-entry-right .fc-input{width:150px !important}
/*============== Setup start ==================*/

.custsetup-popup { border: medium none !important; left: 0 !important; position: relative !important;  top: 0 !important;}
.custsetup-popup table tr {height:45px  }
.custsetup-btn {float:left ;width:100%; margin:15px 0; }

#wait {width:100%; float:left; text-align:center; margin-top:10px}
#wait span {padding-top:15px; display:block    }
.signature {float:left; width:100%; margin-top:25px}
.signature #signbg {width:100%; height:100px; border:1px solid #000; margin-top:7px    }
.signature #signbg img {width:100%; height:100%  }
.sigPad { float: left;  margin-top: 15px;  width: 100%; }
.sign-title {float:left; width:100%; padding:5px 10px; background:#316b9d}
.sign-title .sign-title-l {float:left; color:#fff  }
.sign-title .sign-title-r {float:right; color:#fff  }
.sigPad .pad {width:100%}
.register_lbl_TimeFileds {width:20px !important}

.time-spent {margin:0; padding:0; text-align:center}
.time-spent li {list-style:none; width:49%; float:left }
.ts-title {float:left; width:36px; margin-right:5px}
.ts-input {float:left; width:calc(100% - 40px);}

.cn-search {background:#316b9d; padding:4px 10px 5px}
.cn-search i {color:#fff  }
.bg-black {background-color:rgba(0, 0, 0, 0.5)}
#programmaticModalPopupBehavior_foregroundElement {top:-100px !important; left:15% !important}
.map-location {background-color: #E5E3DF; border: 1px solid; z-index: 9999; height: 196px; left: 100%; overflow: hidden; position: absolute; width: 300px; display: none;}
.merchant-input .map-btn {padding:4px 4px 5px; font-size:11px !important}

.ra-table.ra-table.table-bordered > tbody > tr > td {border:none !important}
.ra-table.table-striped>tbody>tr {background:#f6f6f6 !important;}
.ra-table.table-striped>tbody>tr:nth-of-type(odd) {background-color:#f6f6f6 !important}
.ra-table.ra-table.table-bordered > tbody > tr > td a {color:#333 !important}
.pending-reco {margin-top:20px; float:left; width:100% }
.pr-text {background: #f6f6f6 none repeat scroll 0 0; width:100%;  float: left; margin: 0;  padding: 10px; }
.pr-text table  {border:none !important    }
.pr-text table > tbody > tr > td {border:none !important    }

/*.autocomplete_completionListElement { background:#fcfcfc; margin:0; padding:0; box-shadow:0 0 5px #999; height:200px; overflow-y:scroll; overflow-x:hidden}
.autocomplete_completionListElement li {list-style:none; padding:2px 10px;     }
.autocomp-filter { background:#fcfcfc; margin:0; padding:0; box-shadow:0 0 5px #999; position:absolute; z-index:9999 !important; max-width:1200px; width:100%}
.autocomp-filter li {list-style:none; padding:2px 10px;     }*/

.autocomplete_completionListElement
{
    margin: 0px !important;
    background-color: #fff;
    color: windowtext;
    border: buttonshadow;
    border-width: 1px;
    border-style: solid;
    cursor: 'default';
    overflow: auto;
    height: 200px;
    text-align: left;
    list-style-type: none;
    
    
}

/* AutoComplete highlighted item */

.autocomplete_highlightedListItem
{
    background-color: #ffff99;
    color: black;
    padding: 1px;
       
}

/* AutoComplete item */

.autocomplete_listItem
{
    background-color: window;
    color: windowtext;
    padding: 1px;
        
}


.balance-condi { position:absolute; top:0; right:-160px}
.sat-select {margin-top:15px}
.sat-select .sc-form .form-control:first-child {margin-left:0 } 

.portlet-body.cont-table table .table-striped>tbody>tr {background:#fff;}
.portlet-body.cont-table table .table-striped>tbody>tr:nth-of-type(odd) {background-color:#fff}
.portlet-body.cont-table table .table-striped>tbody>tr .form-control {background:#fff; border-bottom:1px solid #999; height:30px !important}

.portlet-body.cont-table table .table-striped>tbody>tr:nth-of-type(odd) .form-control {background-color:#fff; }
.portlet-body.cont-table table .table-hover>tbody>tr:hover, .portlet-body.cont-table .table-hover>tbody>tr:focus {background-color:#d3e0f1}
.portlet-body.cont-table table .table-hover>tbody>tr:hover .form-control, .portlet-body.cont-table .table-hover>tbody>tr:focus .form-control {border:1px solid #999; height:30px !important}

.portlet-body.cont-table .table-scrollable {width:100% !important}



.eb-print {float:left; width:100%; background:#d3e0f1; margin-top:15px; border: 1px solid #e1e1e1 !important;  border-radius: 2px 2px 0 0 !important; padding:20px 15px 0; color: #333;}
.to-print {float:right; max-width:300px; margin-bottom:10px;}
.to-print .form-col {margin-bottom:0;}
.form-col.pay-to .fc-label {float:left; text-align:left; width:120px;}
.form-col.pay-to .fc-input {float:left; width:calc(100% - 130px);}
.form-col.pay-doller .fc-label {float:left; text-align:left; width:10px;}
.form-col.pay-doller .fc-input {float:left; width:calc(100% - 20px);}

.form-col.text-dollar .fc-input {float: left; width: calc(100% - 55px);}
.form-col.text-dollar .fc-input label { border-bottom:1px solid #686868; color: #333; font-size: 14px;  font-weight: normal; padding: 5px 10px; height:32px; width:100%;}
.form-col.text-dollar .fc-label {float: right; margin-left: 10px;  margin-right: 0;  text-align: left;  width: 45px}

.form-col.pay-memo .fc-label {float:left; text-align:left; width:45px;}
.form-col.pay-memo .fc-input {float:left; width:calc(100% - 55px);}



.pt-right {float:right;}
.pt-right .popup-anchor {background:none!important;  color: #fff; float: left; height: 16px; margin-left: 10px;  margin-right: 0; width:auto}

.project-col .form-col {float: left;  width: 50%;  }
.project-col .form-col .fc-label {width:60px; text-align:left}
.project-col .form-col:last-child .fc-label {text-align:right    }
.project-col .form-col .fc-input .form-control {width:95px }
#DayPilotCalendar1 div:last-child { left:203px; top:124px}
.map-leads-box{position:absolute; left:133px; top:-4px; z-index:100}
.map-leads{display: none; background-color: #E5E3DF; border: 1px solid; left: 0; top: 0; position: absolute; width: 300px;}
.map-prospect{background-color: #E5E3DF; border: 1px solid; height: 196px; left: 0; top: 50px; overflow: hidden; position: absolute; width: 300px;}
#Behavior_backgroundElement, #PMPBehaviour1_backgroundElement, #PMPBehaviour_backgroundElement {background:rgba(0,0,0,0.5)}
.footerclass{padding:10px 0 0 4px}

.menu {margin: 0px auto; font-family: Arial, Helvetica, sans-serif; font-weight: bold; font-size: 14px; float:left}
.menu tr td a:link, div tr td a:visited {background-color: #fff;border-bottom: 3px solid #fff;color: #000;display: block;font-size: 12px;font-weight: normal;padding: 4px;text-align: center;text-decoration: none;width: auto;}
.menu tr td a:hover {background-color: #ccc;}
.menu tr {list-style-type: none;margin: 0px;padding: 0px;}
.menu tr td {float: left;margin-left: 5px;}

.fc-label2 {width:75px; margin-right:10px; float:left; margin-top:5px; vertical-align:top;}
.iframe-bucket{width: 820px; height: 320px; overflow-y: hidden; overflow-x: hidden}

.iframe-bucket1{width:823px; height:327px; overflow-y:hidden; overflow-x:hidden}
.table-width {width:320px;height:300px;overflow-x:hidden; overflow-y:hidden;}
.table-billing {width:500px;height:450px;overflow-x:hidden;overflow-y:hidden}
.table-merchant {width:350px;height:255px;overflow-x:hidden;overflow-y:hidden}
.table-subcategory {width:380px;height:140px;overflow-x:hidden;overflow-y:hidden}
.iframe-eqipment {width:372px;height:140px;overflow-x:hidden;overflow-y:hidden}
.custuser-popup{width:740px;height:195px;overflow-y:hidden;overflow-x:hidden}
/*.ajax__validatorcallout div, .ajax__validatorcallout td {
    border: solid 1px #ddd !important;
    background-color: #ddd !important;
}*/


.ajax__validatorcallout div {background-color: #f9f9f9 !important; border:1px solid #ccc !important}
.ajax__validatorcallout_error_message_cell, .ajax__validatorcallout_icon_cell {background-color: #f9f9f9 !important; border:1px solid #ccc !important}
.ajax__validatorcallout_close_button_cell {background-color: #f9f9f9 !important; border:1px solid #ccc !important}
.ajax__validatorcallout_error_message_cell{font-size:11px !important}
.ajax__validatorcallout_close_button_cell .ajax__validatorcallout_innerdiv {padding-right:12px !important}
/*.ajax__validatorcallout_popup_table_row .ajax__validatorcallout_icon_cell > img { display: none !important; }*/
/*.ajax__validatorcallout_popup_table_row .ajax__validatorcallout_icon_cell {background-image:url(/images/warning2.png) !important; height: 35px !important;width: 35px !important;}*/
.scrollable_calendar { padding: 20px 15px; }
.daypilot_navigator{width:180px; float:left; padding:15px;}
.cal-daypilot {margin-left:180px; float:left}
.calendar_traditionalcellbackground, .calendar_traditionalhourhalfcellborder {width:150px !important;}
.calendar_traditionalcolheader {width:151px !important;}
#ctl00_ContentPlaceHolder1_DayPilotCalendar1 > div { overflow:visible !important}


.newRow{width:2200px;}
.resize{position:relative; z-index:9999}
.ResizeTextBox{width:520px}
.pnlAccounts {position:relative; z-index:999}
.title-button { margin-right:10px; margin-top: -3px; padding: 3px; display:inline-block; border:1px solid #fff; }
.title-button:hover{background-color:white}
.title-button i { color:white}
.calendar_traditionaltoday{background-color: #ffe794; color:black}
.title_text{color:white}
.setup-model{border: 1px solid #316b9d !important; width: 500px}
.setup-table{width:100%; height:85px}
.lnklist-header{margin: 0;padding:0}
.lnklist-header li{list-style:none; display:inline-block; margin-left:5px}
.lnklist-header li:first-child{margin-left:0px;margin-right:10px}
.lnklist-header .title_text{color:white; font-weight:bold; font-size:18px; margin-right: 0}
.lnklist-header li .dropdown-menu li {display:block; margin-right:0  }
.lnklist-header li .dropdown-menu li > a {width:100%; height:100%}
.lnklist-header li a {background-size: 100% auto; height: 23px; width: 23px;}
.icon-editnewSetUp {background: url(../../images/edit-24.png) no-repeat left top;}
.icon-cancelSetup {background: url(../../images/delete-2-24.png) no-repeat left top;}
.icon-saveSetup {background: url(../../images/check-mark-12-24.png) no-repeat left top;}
.icon-addnew {background: url(../../images/icons/Add.png) no-repeat left top;}
.icon-edit {background: url(../../images/icons/Edit.png) no-repeat left top;}
.icon-copy {background: url(../../images/icons/Copy.png) no-repeat left top;}
.icon-delete {background: url(../../images/icons/Delete.png) no-repeat left top;}
.icon-closed {background: url(../../images/icons/Close.png) no-repeat left top;}
.icon-save {background: url(../../images/icons/Save.png) no-repeat left top;}
.icon-first {background: url(../../images/icons/1.png) no-repeat left top;}
.icon-previous {background: url(../../images/icons/3.png) no-repeat left top;}
.icon-next {background: url(../../images/icons/4.png) no-repeat left top;}
.icon-last {background: url(../../images/icons/2.png) no-repeat left top;}
.icon-mail {background: url(../../images/icons/Mail.png) no-repeat left top;}
.icon-print {background: url(../../images/icons/print.png) no-repeat left top;}
.icon-pdf {background: url(../../images/icons/PDFIcon.png) no-repeat left top;}
.icon-refresh1 {background: url(../../images/icons/Refresh.png) no-repeat left top;}
.icon-changepassword {background: url('../../images/icons/change password.png') no-repeat left top;}
.icon-reset {background: url(../../images/icons/Reset.png) no-repeat left top;}
.icon-gpssetting {background: url(../../images/icons/gps.png) no-repeat left top;}
.icon-makepayment {background: url('../../images/icons/Make Payment.png') no-repeat left top;}
.icon-cutcheck {background: url('../../images/icons/Cutcheck.png') no-repeat left top;}
.lnklist-header li .nav .open > a,.lnklist-header li .nav .open > a:focus, .lnklist-header li .nav .open > a:hover {background-color: transparent !important}
.lnklist-header-span span{margin-right: 5px !important}
.lnklist-panel {padding:5px 10px}
.lnklist-panel .title_text{font-size:14px}
.lnklist-panel li a {height: 18px; width: 18px;}
.lnklist-panel li:first-child{margin-left: 0;margin-right: 0;}
.pnlRefresh{ /*background-color: #000000;*/ background-color:transparent; height: 100%; width: 100%; position: fixed; top: 0px; z-index: 10010; filter: alpha(opacity=50); -moz-opacity: 0.50; opacity: 0.50; }
.pnlLoading { height: 100%; width: 100%; position: fixed; top: 0px; z-index: 10010; }



/*----------- cousterm-receivable ------------*/

.recent-activity .cr-list a {color:#464e56;}
.recent-activity .cr-list a:hover {color:#4277a5; text-decoration:none;}
.recent-activity .cr-list li {padding:7px}
.recent-activity .cr-list li i {padding-top:3px;}
.recent-activity .cr-list li:hover, .recent-activity .cr-list li.active {background:#d3e0f1}
.recent-activity .cr-list li:hover a i, .recent-activity .cr-list li.active a i {color:#4277a5;}
.recent-activity .cr-list li:hover a span, .recent-activity .cr-list li.active a span {color:#4277a5;}

.cr-topTitle {font-size:18px; color:#4277a5; padding-bottom:15px;}
.cr-box {text-align:center; padding:10px 15px 20px; transition: all 0.8s ease; border:1px solid #fff;}
.cr-box:hover {box-shadow:0 0 5px #ccc; background:#f0f0f0; border:1px solid #ccc;}
.cr-box .cr-title {font-size:15px; color:#4277a5; font-weight:bold; }
.cr-box .cr-img {margin:12px 0;}
.cr-box .cr-img img {max-width:256px; margin:0 auto; width:100%;}
.cr-box .cr-date .date {display:inline-block; margin-left:10px; }
.cr-box .cr-date .date:first-child {margin-left:0;}
.cr-box .cr-date .date span {display:block; margin-top:5px;}
.cr-box .cr-iocn {margin-top:15px;}
.cr-box .cr-iocn a {display:inline-block; margin-left:5px;}
.cr-box .cr-iocn a:first-child {margin-left:0;}
.cr-box .cr-iocn a img {width:30px; transition: all 0.8s ease;}








/*============== responsive start ==================*/
@media (max-width: 1200px) {
.coaHidePanel {display:table}
.table-addjournal {overflow-x: scroll; max-width:680px }
.table-addjournal input[type="text"], .table-addjournal textarea {width:150px !important}
}
@media (max-width: 992px) {
    .permission-box {  width: 100%;float: left;}
    .search-customer ul {float:left; margin-top:5px    }
    .map-location {  left: 150px;    top: 360px; }
    .newRow { max-width: 940px; width: auto; }
    .scrollable_calendar {width:100%; max-width:765px; margin: 10px 0 !important; overflow-x: auto; overflow-y: hidden; }
    .project-col .form-col .fc-input .form-control {width:100%    }
    .project-col .form-col .fc-label {width:122px; text-align:right    }
    .form-col .fc-input input, .form-col .fc-input select {-webkit-appearance:none; -moz-appearance:none;  -o-appearance:none; appearance:none;}
}
@media (max-width: 767px) {
.page-header.navbar .page-logo .logo-default { width: 220px}
.page-header.navbar .top-menu .navbar-nav > li.dropdown-notification .dropdown-menu { margin-right: -120px;}
.page-header.navbar .top-menu .navbar-nav > li.dropdown-notification .dropdown-menu::after, .page-header.navbar .top-menu .navbar-nav > li.dropdown-notification .dropdown-menu::before { margin-right: 124px;}
.page-header.navbar .top-menu .navbar-nav > li.dropdown-inbox .dropdown-menu {  margin-right: -60px;}
.page-header.navbar .top-menu .navbar-nav > li.dropdown-inbox .dropdown-menu::after, .page-header.navbar .top-menu .navbar-nav > li.dropdown-inbox .dropdown-menu::before {   margin-right: 65px;}
.map-sm {margin-left:135px;}
.edit-tab .nav-tabs > li > a {padding: 10px 7px;}	
.form-col .fc-input .input-xlarge {width:100% !important;}
.portlet-body.cont-table .table-bordered > thead > tr > th {line-height:28px}
.portlet-body.cont-table .table-bordered > tbody > tr > td, .portlet-body.cont-table .table-bordered > tbody > tr > th {line-height:28px; padding:8px}
.sc-form label { display: block;}
.sc-form .form-control {width:100% !important; margin-bottom:5px;  margin-left: 0;    }
.search-customer ul {width:100%; float:left; margin-top:10px;}
.permisson {padding-left:0}

#programmaticModalPopupBehavior_foregroundElement {top:-100px !important; left:5% !important}
.balance-condi { left: 0; position: absolute; right: 0; top: 100%;}
.sat-select { margin-top: 45px;}
.iframe-bucket{width: 600px; height: 320px; overflow-y: hidden; overflow-x: hidden}
.iframe-bucket1{width:600px;height:320px; overflow-y:hidden; overflow-x:hidden}
.table-billing {width:500px;height:400px;overflow-x:hidden;overflow-y:hidden }
.custuser-popup{width:600px;min-height:230px; overflow-y:hidden;overflow-x:hidden}

 .table-addjournal {overflow-x: scroll; max-width:430px }
}

@media (max-width: 480px) {
.page-header.navbar .page-logo .logo-default { width: 250px;}
.pc-title span { display: block; margin-top:10px;}
.pc-title a { margin-top:10px;}
.map-sm {margin-left:0;}
.form-col {margin-bottom:10px;}
.form-col .fc-label {width:100%; margin-top:0; text-align:left; margin-right:0;}
.form-col .fc-input {width:100%; }
.tab-cont-top ul {width:100%; float:left; margin-top:10px;}
.credit-dispathc br {display:none}

.sc-form .form-control {margin-left:5px;}
.sc-form .form-control {width:125px !important;}
.search-customer ul li {width:100%; margin-left:0; margin-top:5px;}
.select-chart  {width:100%}

.map-location {  left: 0;    top: 435px; } 
.map-leads-box{left:0; top:50px;}
#ctl00_ctl00_ContentPlaceHolder1_ContentPlaceHolder1_pnlPop{width:300px !important}
.iframe-bucket{width: 300px; min-height: 320px; overflow-y: hidden; overflow-x: hidden}
.iframe-bucket1{width:300px; min-height:320px; overflow-y:hidden;overflow-x:hidden}
.pt-right {margin-top:-30px;}
.table-width {width:300px;min-height:320px; overflow-y:hidden;overflow-x:hidden}
.table-billing {width:300px;min-height:470px;overflow-x:hidden;overflow-y:hidden}
.table-merchant {width:300px;height:260px;overflow-x:hidden;overflow-y:hidden}
.table-subcategory {width:300px;height:450px;overflow-x:hidden;overflow-y:hidden}
.iframe-eqipment{width:300px;min-height:150px;overflow-x:hidden;overflow-y:hidden}
.custuser-popup{width:300px;min-height:235px; overflow-y:hidden;overflow-x:hidden}
.project-col .form-col:last-child .fc-label {text-align:left;}

.table-addjournal {overflow-x: scroll; max-width:270px }
.project-col .form-col {width:100%}
.project-col .form-col .fc-label {text-align:left}
.setup-model{border: 1px solid #316b9d !important; width: 300px;}
}


    </style>
    <style>
        .page-footer {background:#fff !important}
        .ui-autocomplete-loading {
            background: white url('images/autocomp.gif') right center no-repeat;
        }

        #headerfix {
            /*position: fixed;
            top: 0;
            left: 0;
            z-index:9999;  
            opacity:0.95;
            filter:alpha(opacity=95);   
            */
            width: 100%;
            background: url('images/header_bg.png') repeat-x scroll 0 0 #fff;
        }

        .grayscales {
            filter: gray; /* IE6-9 */
            filter: grayscale(1); /* Firefox 35+ */
            -webkit-filter: grayscale(1); /* Google Chrome, Safari 6+ & Opera 15+ */
        }
        /*#hover-content {
            display:none; 
        }
    
        #hover-me:hover #hover-content {
            display:block;
        }*/
        #hover-me {
            float: right;
        }

        #hover-content {
            opacity: 0;
            -webkit-transition: .5s;
            -moz-transition: .5s;
            -o-transition: .5s;
            -ms-transition: .5s;
            transition: .5s;
        }

        #hover-me:hover #hover-content {
            opacity: 1;
        }
    </style>
    
</head>


    <style>
        .ListControl input[type=radio] + label {
            width: 10em !important;
            background: none !important;
        }

        .ListControl input[type=checkbox] + label {
            margin-left: 0px !important;
            width: 10em !important;
            background: none !important;
            border: none !important;
        }

        /*.ListControl input[type=radio]:checked + label::before {
            margin-left: -16px !important;
        }*/

        /*.ListControl input[type=checkbox]:checked + label::before {
            margin-left: 0px !important;
        }*/
    </style>
    <script src="js/ColumnResizeWithReorder/jquery.dataTables.js" type="text/javascript"></script>

    <script src="js/ColumnResizeWithReorder/ColReorderWithResize.js" type="text/javascript"></script>

    <script src="js/ReportsJs/RecurringReport.js" type="text/javascript"></script>

    <%--<script type="text/javascript" src="http://cdn.rawgit.com/niklasvh/html2canvas/master/dist/html2canvas.min.js"></script>--%>

    <script type="text/javascript" src="js/Printdiv/jquery.print.js"></script>

    <link rel="stylesheet" href="css/css3-dropdown-menu/assets/css/styles.css" />
    <%--<link rel="stylesheet" href="css/css3-dropdown-menu/assets/font-awesome/css/font-awesome.css" />--%>

    <script type="text/javascript" src="js/BlockUI/jquery.blockUI.js"></script>

   <script type="text/javascript">

       //Changed by Yashasvi Jadav.
       function removeCheckbox() {
           var _data = $('[id=lblText]');
           $.each(_data, function (index, element) {
               $(this).closest('td').find('input[type="checkbox"]').remove();
           });
       }

       function setFilterStyle() {
           $('#lstFilter option').each(function (index, element) {
               if ($(element).val() == "Customers" || $(element).val() == "Locations" || $(element).val() == "Equipments" || $(element).val() == "Recurring") {
                   $(element).css({ "font-size": "15px", "padding": "7px 0" });
               }
           });
       }

       function ExportToExcel() {

           var lstColumn = '';
           var lstColumnWidth = '';

           if ($('#tblResize tr').length > 0) {
               $('#tblResize tr th').each(function () {
                   lstColumn += $(this).html() + "^";
                   lstColumnWidth += $(this).css('width') + "^";
               });
           }
           else {
               $('#lstColumnSort option').each(function (index, element) {
                   lstColumn += $(element).val() + "^";
                   lstColumnWidth += "125px" + "^";
               });
           }

           $("#hdnLstColumns").val(lstColumn);

           $("#hdnColumnWidth").val(lstColumnWidth);

           var html = $("#tblResize_wrapper").html();
           html = $.trim(html);
           html = html.replace(/>/g, '&gt;');
           html = html.replace(/</g, '&lt;');
           $("input[id$='hdnDivToExport']").val(html);
           //  console.log($("input[id$='hdnDivToExport']").val());

           document.getElementById('<%=btnExportExcel.ClientID %>').click();
           setFilterStyle();
       }


       function ExportToPDF() {
           debugger;
           var lstColumn = '';
           var lstColumnWidth = '';

           if ($('#tblResize tr').length > 0) {
               $('#tblResize tr th').each(function () {
                   lstColumn += $(this).html() + "^";
                   lstColumnWidth += $(this).css('width') + "^";
               });
           }
           else {
               $('#lstColumnSort option').each(function (index, element) {
                   lstColumn += $(element).val() + "^";
                   lstColumnWidth += "125px" + "^";
               });
           }

           $("#hdnLstColumns").val(lstColumn);

           $("#hdnColumnWidth").val(lstColumnWidth);

           document.getElementById('<%=btnExportPDF.ClientID %>').click();
           setFilterStyle();
       }


       function btnSendReport() {
           var lstColumn = '';
           var lstColumnWidth = '';

           if ($('#tblResize tr').length > 0) {
               $('#tblResize tr th').each(function () {
                   lstColumn += $(this).html() + "^";
                   lstColumnWidth += $(this).css('width') + "^";
               });
           }
           else {
               $('#lstColumnSort option').each(function (index, element) {
                   lstColumn += $(element).val() + "^";
                   lstColumnWidth += "125px" + "^";
               });
           }

           $("#hdnLstColumns").val(lstColumn);

           $("#hdnColumnWidth").val(lstColumnWidth);

           document.getElementById('<%=btnSendReport.ClientID %>').click();
       }

       function btnSaveReport() {
           var lstColumn = '';
           var lstColumnWidth = '';

           if ($('#tblResize tr').length > 0) {
               $('#tblResize tr th').each(function () {
                   lstColumn += $(this).html() + "^";
                   lstColumnWidth += $(this).css('width') + "^";
               });
           }
           else {
               $('#lstColumnSort option').each(function (index, element) {
                   lstColumn += $(element).val() + "^";
                   lstColumnWidth += "125px" + "^";
               });
           }

           $("#hdnLstColumns").val(lstColumn);

           $("#hdnColumnWidth").val(lstColumnWidth);

           document.getElementById('<%=btnSaveReport2.ClientID %>').click();
       }

       function SendPDFReport(Id) {
           $("#dvGridReport").block({ message: null, overlayCSS: { backgroundColor: '' } });
           $(".title_bar").block({ message: null, overlayCSS: { backgroundColor: '' } });
           $("#Menu").block({ message: null, overlayCSS: { backgroundColor: '' } });

           $("#txtTo").val('');
           $("#txtCc").val('');
           $("#txtSubject").val($("#hdnCustomizeReportName").val());
           $("#dvEmailPanel").show();

           var html = $("#tblResize_wrapper").html();
           html = $.trim(html);
           html = html.replace(/>/g, '&gt;');
           html = html.replace(/</g, '&lt;');
           $("input[id$='hdnDivToExport']").val(html);

           $("input[id$='hdnSendReportType']").val(Id);
           setFilterStyle();
       }
       function EmailCancel() {
           $("#dvGridReport").unblock();
           $(".title_bar").unblock();
           $("#Menu").unblock();
       }

       jQuery(document).ready(function () {
           jQuery('#popup').draggable();
           setFilterStyle();
           //jQuery('#dvSaveReport').draggable();

           //            $("#ctl00__grdCustomerReportData td,th").css("word-wrap", "break-word");

           //            $(".GridViewStyle").colResizable({
           //                liveDrag: true,
           //                fixed: true,
           //                gripInnerHtml: "<div class='grip'></div>",
           //                draggingClass: "dragging",
           //                onResize: onSampleResized

           //            });


           var oTable = $('#tblResize').dataTable({
               "sDom": 'Rplfrti',
               "oColReorder": {
                   "headerContextMenu": true
               },
               "bPaginate": false,
               "bFilter": false,
               "bSort": false,
               "bAutoWidth": true,
               "bInfo": false
           });
       });

        //        var onSampleResized = function(e) {
        //            var columns = $(e.currentTarget).find("th");
        //            var msg = "";
        //            columns.each(function() { msg += $(this).width() + ","; })
        //            // $.cookie("colWidth", msg);
        //        };

   </script>
  <script type="text/javascript">

      //Changed by Yashasvi Jadav.
      function removeCheckbox() {
          var _data = $('[id=lblText]');
          $.each(_data, function (index, element) {
              $(this).closest('td').find('input[type="checkbox"]').remove();
          });
      }

      function setFilterStyle() {
          $('#lstFilter option').each(function (index, element) {
              if ($(element).val() == "Customers" || $(element).val() == "Locations" || $(element).val() == "Equipments" || $(element).val() == "Recurring") {
                  $(element).css({ "font-size": "15px", "padding": "7px 0" });
              }
          });
      }

      function ExportToExcel() {

          var lstColumn = '';
          var lstColumnWidth = '';

          if ($('#tblResize tr').length > 0) {
              $('#tblResize tr th').each(function () {
                  lstColumn += $(this).html() + "^";
                  lstColumnWidth += $(this).css('width') + "^";
              });
          }
          else {
              $('#lstColumnSort option').each(function (index, element) {
                  lstColumn += $(element).val() + "^";
                  lstColumnWidth += "125px" + "^";
              });
          }

          $("#hdnLstColumns").val(lstColumn);

          $("#hdnColumnWidth").val(lstColumnWidth);

          var html = $("#tblResize_wrapper").html();
          html = $.trim(html);
          html = html.replace(/>/g, '&gt;');
          html = html.replace(/</g, '&lt;');
          $("input[id$='hdnDivToExport']").val(html);
          //  console.log($("input[id$='hdnDivToExport']").val());

          document.getElementById('<%=btnExportExcel.ClientID %>').click();
          setFilterStyle();
      }


      function ExportToPDF() {
          debugger;
          var lstColumn = '';
          var lstColumnWidth = '';

          if ($('#tblResize tr').length > 0) {
              $('#tblResize tr th').each(function () {
                  lstColumn += $(this).html() + "^";
                  lstColumnWidth += $(this).css('width') + "^";
              });
          }
          else {
              $('#lstColumnSort option').each(function (index, element) {
                  lstColumn += $(element).val() + "^";
                  lstColumnWidth += "125px" + "^";
              });
          }

          $("#hdnLstColumns").val(lstColumn);

          $("#hdnColumnWidth").val(lstColumnWidth);

          document.getElementById('<%=btnExportPDF.ClientID %>').click();
          setFilterStyle();
      }


      function btnSendReport() {
          var lstColumn = '';
          var lstColumnWidth = '';

          if ($('#tblResize tr').length > 0) {
              $('#tblResize tr th').each(function () {
                  lstColumn += $(this).html() + "^";
                  lstColumnWidth += $(this).css('width') + "^";
              });
          }
          else {
              $('#lstColumnSort option').each(function (index, element) {
                  lstColumn += $(element).val() + "^";
                  lstColumnWidth += "125px" + "^";
              });
          }

          $("#hdnLstColumns").val(lstColumn);

          $("#hdnColumnWidth").val(lstColumnWidth);

          document.getElementById('<%=btnSendReport.ClientID %>').click();
      }

      function btnSaveReport() {
          var lstColumn = '';
          var lstColumnWidth = '';

          if ($('#tblResize tr').length > 0) {
              $('#tblResize tr th').each(function () {
                  lstColumn += $(this).html() + "^";
                  lstColumnWidth += $(this).css('width') + "^";
              });
          }
          else {
              $('#lstColumnSort option').each(function (index, element) {
                  lstColumn += $(element).val() + "^";
                  lstColumnWidth += "125px" + "^";
              });
          }

          $("#hdnLstColumns").val(lstColumn);

          $("#hdnColumnWidth").val(lstColumnWidth);

          document.getElementById('<%=btnSaveReport2.ClientID %>').click();
      }

      function SendPDFReport(Id) {
          $("#dvGridReport").block({ message: null, overlayCSS: { backgroundColor: '' } });
          $(".title_bar").block({ message: null, overlayCSS: { backgroundColor: '' } });
          $("#Menu").block({ message: null, overlayCSS: { backgroundColor: '' } });

          $("#txtTo").val('');
          $("#txtCc").val('');
          $("#txtSubject").val($("#hdnCustomizeReportName").val());
          $("#dvEmailPanel").show();

          var html = $("#tblResize_wrapper").html();
          html = $.trim(html);
          html = html.replace(/>/g, '&gt;');
          html = html.replace(/</g, '&lt;');
          $("input[id$='hdnDivToExport']").val(html);

          $("input[id$='hdnSendReportType']").val(Id);
          setFilterStyle();
      }
      function EmailCancel() {
          $("#dvGridReport").unblock();
          $(".title_bar").unblock();
          $("#Menu").unblock();
      }

      jQuery(document).ready(function () {
          jQuery('#popup').draggable();
          setFilterStyle();
          //jQuery('#dvSaveReport').draggable();

          //            $("#ctl00__grdCustomerReportData td,th").css("word-wrap", "break-word");

          //            $(".GridViewStyle").colResizable({
          //                liveDrag: true,
          //                fixed: true,
          //                gripInnerHtml: "<div class='grip'></div>",
          //                draggingClass: "dragging",
          //                onResize: onSampleResized

          //            });


          //var oTable = $('#tblResize').dataTable({
          //    "sDom": 'Rplfrti',
          //    "oColReorder": {
          //        "headerContextMenu": true
          //    },
          //    "bPaginate": false,
          //    "bFilter": false,
          //    "bSort": false,
          //    "bAutoWidth": true,
          //    "bInfo": false
          //});
      });

        //        var onSampleResized = function(e) {
        //            var columns = $(e.currentTarget).find("th");
        //            var msg = "";
        //            columns.each(function() { msg += $(this).width() + ","; })
        //            // $.cookie("colWidth", msg);
        //        };

  </script>


    <style type="text/css">
        @import "js/ColumnResizeWithReorder/ColReorder.css";
        /*----- Buttons style -----*/ .myButton {
            -moz-box-shadow: inset 0px 1px 0px 0px #ffffff;
            -webkit-box-shadow: inset 0px 1px 0px 0px #ffffff;
            box-shadow: inset 0px 1px 0px 0px #ffffff;
            background: -webkit-gradient(linear, left top, left bottom, color-stop(0.05, #f9f9f9), color-stop(1, #e9e9e9));
            background: -moz-linear-gradient(top, #f9f9f9 5%, #e9e9e9 100%);
            background: -webkit-linear-gradient(top, #f9f9f9 5%, #e9e9e9 100%);
            background: -o-linear-gradient(top, #f9f9f9 5%, #e9e9e9 100%);
            background: -ms-linear-gradient(top, #f9f9f9 5%, #e9e9e9 100%);
            background: linear-gradient(to bottom, #f9f9f9 5%, #e9e9e9 100%);
            filter: progid:DXImageTransform.Microsoft.gradient(startColorstr='#f9f9f9', endColorstr='#e9e9e9',GradientType=0);
            background-color: #f9f9f9;
            -moz-border-radius: 6px;
            -webkit-border-radius: 6px;
            border-radius: 6px;
            border: 1px solid #dcdcdc;
            display: inline-block;
            cursor: pointer;
            color: #666666;
            font-family: Arial;
            font-size: 12px;
            font-weight: bold;
            padding: 3px 15px 3px 15px;
            text-decoration: none;
            text-shadow: 0px 1px 0px #ffffff;
        }

            .myButton:hover {
                background: -webkit-gradient(linear, left top, left bottom, color-stop(0.05, #e9e9e9), color-stop(1, #f9f9f9));
                background: -moz-linear-gradient(top, #e9e9e9 5%, #f9f9f9 100%);
                background: -webkit-linear-gradient(top, #e9e9e9 5%, #f9f9f9 100%);
                background: -o-linear-gradient(top, #e9e9e9 5%, #f9f9f9 100%);
                background: -ms-linear-gradient(top, #e9e9e9 5%, #f9f9f9 100%);
                background: linear-gradient(to bottom, #e9e9e9 5%, #f9f9f9 100%);
                filter: progid:DXImageTransform.Microsoft.gradient(startColorstr='#e9e9e9', endColorstr='#f9f9f9',GradientType=0);
                background-color: #e9e9e9;
            }

            .myButton:active {
                position: relative;
                top: 1px;
            }

        .BlueButton {
            -moz-box-shadow: 3px 4px 0px 0px #1564ad;
            -webkit-box-shadow: 3px 4px 0px 0px #1564ad;
            box-shadow: inset 0px 1px 0px 0px #ffffff;
            background: -webkit-gradient(linear, left top, left bottom, color-stop(0.05, #79bbff), color-stop(1, #378de5));
            background: -moz-linear-gradient(top, #79bbff 5%, #378de5 100%);
            background: -webkit-linear-gradient(top, #79bbff 5%, #378de5 100%);
            background: -o-linear-gradient(top, #79bbff 5%, #378de5 100%);
            background: -ms-linear-gradient(top, #79bbff 5%, #378de5 100%);
            background: linear-gradient(to bottom, #79bbff 5%, #378de5 100%);
            filter: progid:DXImageTransform.Microsoft.gradient(startColorstr='#79bbff', endColorstr='#378de5',GradientType=0);
            background-color: #79bbff;
            -moz-border-radius: 6px;
            -webkit-border-radius: 6px;
            border-radius: 6px;
            border: 1px solid #337bc4;
            display: inline-block;
            cursor: pointer;
            color: #ffffff;
            font-family: Arial;
            font-size: 12px;
            font-weight: bold;
            padding: 3px 15px 3px 15px;
            text-decoration: none;
            text-shadow: 0px 1px 0px #528ecc;
        }

            .BlueButton:hover {
                color: #ffffff;
                background: -webkit-gradient(linear, left top, left bottom, color-stop(0.05, #378de5), color-stop(1, #79bbff));
                background: -moz-linear-gradient(top, #378de5 5%, #79bbff 100%);
                background: -webkit-linear-gradient(top, #378de5 5%, #79bbff 100%);
                background: -o-linear-gradient(top, #378de5 5%, #79bbff 100%);
                background: -ms-linear-gradient(top, #378de5 5%, #79bbff 100%);
                background: linear-gradient(to bottom, #378de5 5%, #79bbff 100%);
                filter: progid:DXImageTransform.Microsoft.gradient(startColorstr='#378de5', endColorstr='#79bbff',GradientType=0);
                background-color: #378de5;
            }

            .BlueButton:active {
                position: relative;
                top: 1px;
            }
        /*----- Button style end here -----*/ /*----- Model -----*/

        .modal-box {
            display: none;
            position: absolute;
            z-index: 1000;
            width: 75%; /*60%*/
            background: white;
            border-bottom: 1px solid #aaa;
            border-radius: 4px;
            box-shadow: 0 3px 9px rgba(0, 0, 0, 0.5);
            border: 1px solid rgba(0, 0, 0, 0.1);
            background-clip: padding-box;
            top: -35px !important;
        }

            .modal-box header {
                background-color: #316b9d; /*#f2972b;*/
            }

            .modal-box header, .modal-box .modal-header {
                /*padding: 1.25em 1.5em;*/
                padding: 0.50em 0.5em;
                border-bottom: 1px solid #ddd;
            }

                .modal-box header h3, .modal-box header h4, .modal-box .modal-header h3, .modal-box .modal-header h4 {
                    margin: 0;
                    color: White;
                }

            .modal-box .modal-body {
                padding: 2em 1.5em;
            }

            .modal-box footer, .modal-box .modal-footer {
                padding: 1em;
                border-top: 1px solid #ddd;
                background: rgba(0, 0, 0, 0.02);
                text-align: right;
            }

        .modal-overlay {
            opacity: 0;
            filter: alpha(opacity=0);
            position: absolute;
            top: 0;
            left: 0;
            z-index: 900;
            width: 100%;
            height: 100%;
            background: rgba(0, 0, 0, 0.3) !important;
            color: black;
        }

        footer a {
            width: 68px;
            text-align: center;
        }

        a.close {
            line-height: 1;
            font-size: 1.7em;
            font-weight: bold;
            position: absolute;
            top: 1%;
            right: 2%;
            text-decoration: none;
            color: #fff;
        }

            a.close:hover {
                color: #222;
                -webkit-transition: color 1s ease;
                -moz-transition: color 1s ease;
                transition: color 1s ease;
            }
        /*----- Model end here -----*/ /*----- Tabs -----*/

        .tabs {
            width: 100%;
            display: inline-block;
        }
        /*----- Tab Links -----*/ /* Clearfix */

        .tab-links:after {
            display: block;
            clear: both;
            content: '';
        }

        .tab-links li {
            margin: 0px 1px;
            float: left;
            list-style: none;
        }

        .tab-links a {
            padding: 5px 20px;
            display: inline-block;
            border-radius: 4px 4px 0px 0px;
            border-bottom: 0px;
            background: #cdcdcd;
            font-size: 12px;
            font-weight: 600;
            color: #4c4c4c;
            transition: all linear 0.15s;
            text-align: center;
            width: 100%; /*100px*/
        }

            .tab-links a:hover {
                background: #a7cce5;
                text-decoration: none;
            }

        li.active a, li.active a:hover {
            background: #fff;
            color: #4c4c4c;
            border: 1px solid #CDCDCD;
            border-bottom: 0;
        }
        /*----- Content of Tabs -----*/ .tab-content {
            padding: 15px;
            border-radius: 3px;
            box-shadow: -1px 1px 5px rgba(0,0,0,0.15);
            background: #fff;
            height: 400px;
        }

        .tab {
            display: none;
        }

            .tab.active {
                display: block;
            }
        /*----- Tabs end here -----*/ /*Css design for checkbox list and radio button list*/

        .ListControl input[type=checkbox], input[type=radio] {
            display: none;
        }

        .ListControl label {
            display: inline;
            float: left;
            color: #000;
            cursor: pointer;
            text-indent: 20px;
            white-space: nowrap;
        }

        .ListControl input[type=checkbox] + label {
            background-position: 0% 0%;
            display: block;
            width: 1em !important;
            height: 1em;
            border: 0.0625em solid rgb(192,192,192) !important;
            border-radius: 0.25em !important;
            background-image: linear-gradient(rgb(240,240,240),rgb(240,240,240)) !important;
            vertical-align: middle;
            line-height: 1em !important;
            font-size: 12px;
            background-color: rgb(240,240,240) !important;
            background-repeat: repeat !important;
            background-attachment: scroll !important;
        }

        .ListControl input[type=checkbox]:checked + label::before {
            content: "\2714";
            color: #000;
            height: 1em;
            line-height: 1.1em;
            width: 1em;
            font-weight: normal;
            margin-right: 6px;
            margin-left: -20px;
        }

        .ListControl input[type=radio] + label {
            background-position: 0% 0%;
            display: block;
            width: 1em !important;
            height: 1em;
            border: 0.0625em solid rgb(192,192,192) !important;
            border-radius: 1em !important;
            background-image: linear-gradient(rgb(240,240,240),rgb(240, 240, 240)) !important;
            vertical-align: middle;
            line-height: 1em !important;
            font-size: 12px;
            background-color: black;
            background-repeat: repeat !important;
            background-attachment: scroll !important;
        }

        .ListControl input[type=radio]:checked + label::before {
            /*content: "\2716";*/
            content: "\2714";
            color: #000;
            display: inline;
            width: 1em !important;
            height: 1em !important;
            margin-right: 6px;
            margin-left: -20px;
        }
        /*end here*/ /*Single checkbox design*/

        .CheckBoxLabel {
            white-space: nowrap;
        }

        .SingleCheckbox input[type=checkbox] {
            display: none;
        }

        .SingleCheckbox label {
            display: block;
            float: left;
            color: #000;
            cursor: pointer;
        }

        .SingleCheckbox input[type=checkbox] + label {
            width: 1em;
            height: 1em;
            border: 0.0625em solid rgb(192,192,192);
            border-radius: 0.25em;
            background: rgb(211,168,255);
            background-image: -moz-linear-gradient(rgb(240,240,240),rgb(211,168,255));
            background-image: -ms-linear-gradient(rgb(240,240,240),rgb(211,168,255));
            background-image: -o-linear-gradient(rgb(240,240,240),rgb(211,168,255));
            background-image: -webkit-linear-gradient(rgb(240,240,240),rgb(211,168,255));
            background-image: linear-gradient(rgb(240,240,240),rgb(211,168,255));
            vertical-align: middle;
            line-height: 1em;
            text-indent: 20px;
            font-size: 14px;
        }

        .SingleCheckbox input[type=checkbox]:checked + label::before {
            content: "\2714";
            color: #fff;
            height: 1em;
            line-height: 1.1em;
            width: 1em;
            font-weight: 900;
            margin-right: 6px;
            margin-left: -20px;
        }

        .UpArrow {
            background: url(images/arrow_up.png) no-repeat;
            cursor: pointer;
            border: none;
            width: 25px;
            padding: 2px;
        }

        .DownArrow {
            background: url(images/Down_Arrow.png) no-repeat;
            cursor: pointer;
            border: none;
            width: 25px;
        }

        .ddlstyle {
            width: 200px;
        }

            .ddlstyle option {
                padding-left: 25px;
                padding-top: 3px;
                height: 20px;
                font-size: 12px;
            }

        .highlight {
            background-color: #3399FF !important;
            color: White;
        }

        #tblFilterChoices tr:nth-child(odd) {
            background: #FBF5EF;
        }

        #tblFilterChoices tr:nth-child(even) {
            background: #F5ECCE;
        }

        #tblFilterChoices tr:hover {
            cursor: pointer;
        }
        /*Resize and reorder column*/ #tblResize th {
            text-decoration: underline;
            text-align: center;
            font-family: Arial;
            font-size: 12px !important;
        }

            #tblResize th td {
                font-family: Arial;
                font-size: 11px !important;
            }

        .dataTable th, .dataTable td {
            overflow: hidden;
            white-space: nowrap;
        }

        #tblResize.dataTable {
            border-spacing: 0;
            margin: 0;
        }

        .dataTable th, .dataTable td {
            border-right: 1px solid black;
            max-width: 400px;
        }

            .dataTable th:last-child, .dataTable td:last-child {
                border-right: none;
            }

        #tblResize.display td {
            padding: 3px 4px 4px 3px;
        }

        .selcol1 {
            float: left;
            margin-right: 20px;
        }

        .selcol2 {
            float: left;
        }

        #tblResize .resize-header {
            position: relative;
            color: black;
            font-size: 11px;
            border: 1px solid transparent;
            text-align: left;
            padding-left: 25px;
        }

            #tblResize .resize-header:after {
                position: absolute;
                right: 0;
                top: 30%;
                width: 7px;
                height: 8px;
                content: " ";
                background: url("images/icons_big/list-bullet2.PNG") no-repeat;
            }

            #tblResize .resize-header:first-child:before {
                position: absolute;
                left: 0;
                top: 30%;
                width: 7px;
                height: 8px;
                content: " ";
                background: url("images/icons_big/list-bullet2.PNG") no-repeat;
            }
    </style>
    <style>
        /*Resize and reorder column*/
        .tblPreviewReport th {
            text-decoration: underline;
            text-align: center;
            font-family: Arial;
            font-size: 12px !important;
        }

            .tblPreviewReport th td {
                font-family: Arial;
                font-size: 11px !important;
            }

        .tblPreviewReport.dataTable {
            border-spacing: 0;
            margin: 0;
        }

        .tblPreviewReport .resize-header {
            position: relative;
            color: black;
            font-size: 11px;
            border: 1px solid transparent;
            text-align: left;
            padding: 0 50px;
        }

            .tblPreviewReport .resize-header:after {
                position: absolute;
                right: 0;
                top: 30%;
                width: 7px;
                height: 8px;
                content: " ";
                background: url("images/icons_big/list-bullet2.PNG") no-repeat;
            }

            .tblPreviewReport .resize-header:first-child:before {
                position: absolute;
                left: 0;
                top: 30%;
                width: 7px;
                height: 8px;
                content: " ";
                background: url("images/icons_big/list-bullet2.PNG") no-repeat;
            }
    </style>
<%--</asp:Content>--%>
<%--<asp:Content ID="Content2" ContentPlaceHolderID="" runat="Server">--%>

<body class="page-quick-sidebar-over-content page-sidebar-closed-hide-logo page-container-bg-solid">

    <form id="form1" runat="server" defaultbutton="Button1">
        <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" ScriptMode="Release"
            AsyncPostBackTimeout="360000">
        </asp:ToolkitScriptManager>
        <asp:Button ID="Button1" runat="server" Style="display: none;" Enabled="false" />
        <%-- <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" ScriptMode="Release"
        AsyncPostBackTimeout="360000">
    </asp:ToolkitScriptManager>--%>
        <!-- BEGIN HEADER -->
      
        <!-- END HEADER -->
        <div class="clearfix">
        </div>
        <!-- BEGIN CONTAINER -->
        <div class="page-container">
          
            <!-- BEGIN CONTENT -->
            <div class="page-content-wrapper">
               
                 <!-- BEGIN Manish -->
                
    <div class="page-content setMargin">
        <div class="page-cont-top">
            <%-- <ul class="page-breadcrumb">
                <li>
                    <i class="fa fa-home"></i>
                    <a href="<%=ResolveUrl("~/home.aspx") %>">Home</a>
                    <i class="fa fa-angle-right"></i>
                </li>
                <li>
                    <a href="#">Customer Manager</a>
                    <i class="fa fa-angle-right"></i>
                </li>
                <li>
                    <a href="<%=ResolveUrl("~/customers.aspx") %>">Customer</a>
                    <i class="fa fa-angle-right"></i>
                </li>
                <li>
                    <span>Customers Report</span>
                </li>
            </ul>--%>
        </div>
        <div class="clearfix"></div>
        <div class="row">
           <%-- <div class="col-lg-12 col-md-12">
                <div class="pc-title">
                    <ul class="lnklist-header">
                        <li>
                            <asp:Label CssClass="title_text" ID="lblHeader" runat="server">Recurring Contract Preview</asp:Label></li>
                        <li><a href="<%=ResolveUrl("~/Home.aspx") %>" class="icon-closed" data-original-title="Close" title="Close" data-placement="bottom"></a></li>
                    </ul>
                </div>
            </div>--%>

            <!-- edit-tab start -->
            <div class="col-lg-12 col-md-12">
                <div class="com-cont">
                    <div class="title_bar" style="height: 32px">
                        <div style="float: left">
                            <table>
                                <tr>
                                    <%-- <td>  //Comment By Viral
                                        <asp:Label CssClass="title_text" ID="lblHeader" runat="server">Customers Report</asp:Label>
                                    </td>--%>
                                    <td style="padding-left: 20px;">
                                        <asp:DropDownList ID="drpReports" runat="server" Width="250px" CssClass="form-control ddlstyle"
                                            AutoPostBack="true" OnSelectedIndexChanged="drpReports_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <%-- <td>
                        <select style="width: 200px" class="tech" name="tech" id="tech" onchange="showValue(this)">
                            <option value="calendar" data-image="images/globe.png">Calendar</option>
                            <option value="shopping_cart" data-image="images/globe.png">Shopping Cart</option>
                            <option value="cd" data-image="images/globe.png" name="cd">CD</option>
                        </select>
                    </td>--%>
                                    <td style="padding-left: 20px;">
                                        <a id="btnNewReport" class="myButton js-open-modal" href="#" data-modal-id="popup">New
                            Report</a>
                                    </td>
                                    <td style="padding-left: 2px;">
                                        <asp:Button runat="server" ID="btnSaveReport2" CssClass="myButton" Text="Save Report"
                                            OnClientClick="btnSaveReport();" OnClick="btnSaveReport2_Click" />
                                    </td>
                                    <td style="padding-left: 2px">
                                        <asp:Button ID="btnDeleteReport" runat="server" CssClass="myButton" Text="Delete Report"
                                            OnClientClick="if (!UserDeleteConfirmation()) return false;" OnClick="btnDeleteReport_Click" />
                                    </td>
                                    <td style="padding-left: 2px;">
                                        <a id="btnCustomizeReport" class="myButton js-open-modal" href="#" data-modal-id="popup">Customize Report</a>
                                    </td>
                                    <td style="padding-left: 2px;">
                                        <%--<input id="btnPrint" type="button" value="Print" class="myButton"  />--%>
                                        <a id="btnPrint" class="myButton" href="#">Print</a>
                                    </td>
                                    <td>
                                        <%--<input id="btnEmail" type="button" value="Email" class="myButton" />--%>
                                        <div id="dvBtnEmail">
                                            <ul style="width: 66px">
                                                <li class="green" style="position: relative;"><span class="">
                                                    <img src="images/email_btn.png" height="23px" style="padding-top: 1px;" />
                                                    <div id="dvEmail" style="left: 90%">
                                                        <ul id="dvEmailOptions">
                                                        </ul>
                                                    </div>
                                                </span></li>
                                            </ul>
                                        </div>
                                    </td>
                                    <td>
                                        <div id="colorNav">
                                            <ul style="width: 66px">
                                                <li class="green" style="position: relative;"><span class="">
                                                    <img src="images/export_btn.png" height="23px" style="padding-top: 1px;" />
                                                    <div id="dynamic-div" style="left: 90%">
                                                        <ul id="dynamicUI" style="">
                                                        </ul>
                                                    </div>
                                                </span></li>
                                            </ul>
                                        </div>
                                    </td>
                                    <td style="padding-left: 37px;">
                                        <asp:Button ID="btnClose" runat="server" Text="Close" class="myButton" OnClick="btnClose_Click" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div style="float: right; padding-right: 50px">
                            <table>
                                <tr>
                                    <%--<td>
                                        <a id="btnCustomizeReport" class="myButton js-open-modal" href="#" data-modal-id="popup">Customize Report</a>
                                    </td>--%>
                                    <%--<td>--%>
                                    <%--<input id="btnPrint" type="button" value="Print" class="myButton"  />--%>
                                    <%--     <a id="btnPrint" class="myButton" href="#">Print</a>
                                    </td>--%>
                                    <td>
                                        <%--<input id="btnEmail" type="button" value="Email" class="myButton" />--%>
                                        <%-- <div id="dvBtnEmail">
                                            <ul style="width: 66px">
                                                <li class="green" style="position: relative;"><span class="">
                                                    <img src="images/email_btn.png" height="23px" style="padding-top: 2px;" />
                                                    <div id="dvEmail" style="left: 90%">
                                                        <ul id="dvEmailOptions" style="">
                                                        </ul>
                                                    </div>
                                                </span></li>
                                            </ul>
                                        </div>--%>
                                    </td>
                                    <td>
                                        <%-- <div id="colorNav">
                                            <ul style="width: 66px">
                                                <li class="green" style="position: relative;"><span class="">
                                                    <img src="images/export_btn.png" height="23px" style="padding-top: 2px;" />
                                                    <div id="dynamic-div" style="left: 90%">
                                                        <ul id="dynamicUI" style="">
                                                        </ul>
                                                    </div>
                                                </span></li>
                                            </ul>
                                        </div>--%>
                                    </td>
                                    <%--<td>
                                        <asp:Button ID="btnClose" runat="server" Text="Close" class="myButton" OnClick="btnClose_Click" />
                                    </td>--%>
                                    <td>
                                        <div style="display: none">
                                            <asp:Button ID="btnExportPDF" runat="server" Text="Export" class="myButton" OnClick="btnExportPDF_Click" />
                                            <asp:Button ID="btnExportExcel" runat="server" Text="Export" class="myButton" OnClick="ExportToExcel" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <div style="margin-left: 5px; padding-top: 20px;" align="center">
                        <%-- <asp:Button ID="btnPrintReport" runat="server" Text="Print" OnClick="btnPrintReport_Click" />
                        <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true"
                            EnableDatabaseLogonPrompt="false" HasCrystalLogo="False" HasRefreshButton="True"
                            DisplayToolbar="True" BestFitPage="False" Width="1300px" />--%>
                        <div id="dvGridReport" runat="server" style="display: none;">
                            <%-- <div align="left" style="padding-left: 30px; display: none;">
                                <asp:ImageButton ID="btnFirst" runat="server" CommandArgument="First" ImageUrl="images/first.png"
                                    Width="22px" OnClick="btnFirst_Click" />
                                &nbsp &nbsp<asp:ImageButton ID="btnPrev" runat="server" CommandArgument="Prev" Width="22px"
                                    ImageUrl="~/images/Backward.png" OnClick="btnPrev_Click" />
                                <asp:ImageButton ID="btnForward" runat="server" CommandArgument="Next" ImageUrl="images/Forward.png"
                                    Width="22px" OnClick="btnForward_Click" />
                                &nbsp &nbsp
                                <asp:ImageButton ID="btnLast" runat="server" CommandArgument="Last" ImageUrl="images/last.png"
                                    Width="22px" OnClick="btnLast_Click" />
                                &nbsp; &nbsp; &nbsp; <span style="color: Black">Go to page:</span>
                                <asp:DropDownList ID="ddlPages" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlPages_SelectedIndexChanged">
                                </asp:DropDownList>
                                <span style="color: Black">of </span>
                                <asp:Label ID="lblPageCount" runat="server" ForeColor="Black"></asp:Label>
                                &nbsp &nbsp <span style="color: Black">Display:</span>
                                <asp:DropDownList ID="drpGridRow" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpGridRow_SelectedIndexChanged">
                                    <asp:ListItem Value="15">15</asp:ListItem>
                                    <asp:ListItem Value="20">20</asp:ListItem>
                                    <asp:ListItem Value="30">25</asp:ListItem>
                                </asp:DropDownList>
                                &nbsp; <span style="color: Black">records per page</span>
                            </div>
                            <br />--%>
                            <br />
                            <div id="dvHeader" align="left" style="padding-left: 40px;">
                                <div id="dvMainHeader" style="float: left;">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Image ID="imgLogo" runat="server" Width="150px" Height="150px" />
                                            </td>
                                            <td style="padding-left: 35px;" colspan="4">
                                                <table width="500px" style="height: 100px;">
                                                    <tr>
                                                        <td>
                                                            <asp:Label runat="server" ID="lblCompanyName" Font-Bold="true" Font-Size="17px" ForeColor="Black"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label runat="server" ID="lblCompAddress" ForeColor="Black" Font-Size="14px"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label runat="server" ID="lblCompEmail" ForeColor="Black" Font-Size="12px"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div style="float: right; padding-right: 50px;">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblTime" runat="server" Font-Bold="true" Font-Size="11px" ForeColor="Black"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblDate" runat="server" Font-Bold="true" Font-Size="11px" ForeColor="Black"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div style="clear: both">
                                </div>
                                <div id="dvSubHeader" style="display: block" runat="server" align="center">
                                    <table id="tblSubHeader">
                                        <tr>
                                            <td align="center">
                                                <asp:Label runat="server" ID="lblCompanyName2" Font-Bold="true" Font-Size="17px"
                                                    ForeColor="Black"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <asp:Label runat="server" ID="lblReportTitle" Font-Bold="true" Font-Size="14px" ForeColor="Black"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <asp:Label runat="server" ID="lblSubTitle" Font-Bold="false" Font-Size="12px" ForeColor="Black"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                            <div style="width: 1200px; overflow: auto; padding-top: 20px;" align="left">
                                <%--   <asp:GridView ID="grdCustomerReportData" runat="server" CssClass="GridViewStyle"
                    Visible="false" HeaderStyle-Font-Bold="true" HeaderStyle-Font-Size="13px" HeaderStyle-BorderColor="Black"
                    AllowPaging="true" PageSize="15" Width="900px" PagerSettings-Visible="false"
                    ForeColor="Black" Font-Size="12px" RowStyle-Height="30px" OnRowDataBound="grdCustomerReportData_RowDataBound"
                    OnDataBound="grdCustomerReportData_DataBound" OnPageIndexChanging="grdCustomerReportData_PageIndexChanging">
                </asp:GridView>--%>
                                <asp:PlaceHolder ID="PlaceHolder1" runat="server" />
                            </div>
                        </div>
                        <br />
                    </div>
                    <div id="popup" class="modal-box" style="top: -200px !important">
                        <header>
                            <a href="#" class="js-modal-close close">×</a>
                            <h3><span id="spnModelTitle"></span></h3>
                        </header>
                        <div class="modal-body">
                            <div class="tabs">
                                <ul class="tab-links">
                                    <li class="active"><a href="#tab1">Display</a></li>
                                    <li><a href="#tab2">Filters</a></li>
                                    <li><a href="#tab3">Header/Footer</a></li>
                                    <li><a href="#tab4">Setting</a></li>
                                </ul>
                                <div class="tab-content">
                                    <div id="tab1" class="tab active">
                                        <%-- <p>
                            Tab #1 content goes here!</p>
                        <p>
                            Donec pulvinar neque sed semper lacinia. Curabitur lacinia ullamcorper nibh; quis
                            imperdiet velit eleifend ac. Donec blandit mauris eget aliquet lacinia! Donec pulvinar
                            massa interdum risus ornare mollis.</p>--%>
                                        <fieldset style="border-color: #CDCDCD;">
                                            <legend style="color: Black; font-size: 11px;"><b>COLUMNS:</b></legend>
                                            <div>
                                                <div style="float: left;">
                                                    <div id="dvColumn" style="margin: 20px; padding: 10px; width: 170px; border: 1px solid #CDCDCD; height: 220px; overflow: auto; float: left">
                                                        <label>
                                                            <asp:CheckBoxList ID="chkColumnList" runat="server" ForeColor="Black" Font-Size="12px"
                                                                BorderStyle="Solid" BorderColor="Black" CssClass="ListControl">
                                                            </asp:CheckBoxList>
                                                        </label>
                                                    </div>
                                                    <div style="float: right;">
                                                        <table style="overflow: auto; margin-top: 20px;">
                                                            <tr>
                                                                <td>
                                                                    <%-- <input id="MoveRight" type="button" value=" >> " />
                                                    <br />
                                                    <input id="MoveLeft" type="button" value=" << " /><br />--%>
                                                                    <input id="MoveUp" type="button" class="UpArrow" />
                                                                    <br />
                                                                    <input id="MoveDown" type="button" class="DownArrow" />
                                                                    <%--  <input id="Delete" type="button" value=" Delete Item " />
                                                    <input id="ReadAll" type="button" value=" Read All " />--%>
                                                                </td>
                                                                <td style="padding-left: 10px;">
                                                                    <asp:ListBox ID="lstColumnSort" runat="server" SelectionMode="Multiple" Height="217px"
                                                                        Width="150px" CssClass="form-control">
                                                                        <%-- <asp:ListItem Value="to1">to list 1</asp:ListItem>
                                                        <asp:ListItem Value="to2">to list 2</asp:ListItem>
                                                        <asp:ListItem Value="to3">to list 3</asp:ListItem>
                                                        <asp:ListItem Value="to4">to list 4</asp:ListItem>
                                                        <asp:ListItem Value="to5">to list 5</asp:ListItem>--%>
                                                                    </asp:ListBox>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </div>
                                            </div>
                                            <div id="dvSort" style="float: right; margin: 20px;">
                                                <table>
                                                    <tr>
                                                        <td>Sort by
                                                        </td>
                                                        <td style="padding-left: 15px">
                                                            <asp:DropDownList ID="drpSortBy" runat="server" CssClass="form-control"
                                                                Width="150px">
                                                                <%--   <asp:ListItem>Name</asp:ListItem>
                                                                       <asp:ListItem>City</asp:ListItem>
                                                                       <asp:ListItem>State</asp:ListItem>
                                                                       <asp:ListItem>Type</asp:ListItem>--%>
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>&nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>Sort in
                                                        </td>
                                                        <td style="padding-left: 15px">
                                                            <asp:RadioButtonList ID="rdbOrders" runat="server" CssClass="ListControl">
                                                                <asp:ListItem Selected="True" Value="1">Ascending order</asp:ListItem>
                                                                <asp:ListItem Value="2">Descending order</asp:ListItem>
                                                            </asp:RadioButtonList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2" style="padding-top: 50px">Put a check mark next to each column that
                                            <br />
                                                            you want to appear in the report.
                                            <br />
                                                            <br />
                                                            And set the order of the columns
                                            <br />
                                                            with up and down arrow.
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </fieldset>
                                    </div>
                                    <div id="tab2" class="tab" style="height: 280px;">
                                        <div style="float: left;">
                                            <fieldset style="border-color: #CDCDCD; width: 460px;">
                                                <legend style="color: Black; font-size: 11px;"><b>CHOOSE FILTER:</b></legend>
                                                <div style="float: left; margin: 20px;">
                                                    <asp:ListBox ID="lstFilter" runat="server" CssClass="form-control" Height="217px" Width="150px" Font-Size="12px"></asp:ListBox>
                                                </div>
                                                <div id="Div2" style="float: right; margin: 20px; width: 220px;">
                                                    <table id="tblState" style="display: none">
                                                        <tr>
                                                            <td>State
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:DropDownCheckBoxes ID="ddlState" runat="server" AddJQueryReference="false" UseButtons="false"
                                                                    Width="150px" CssClass="form-control" UseSelectAllNode="false">
                                                                    <Style2 SelectBoxWidth="200" DropDownBoxBoxWidth="200" DropDownBoxBoxHeight="130"
                                                                        SelectBoxCssClass="ListControl" />
                                                                    <Texts SelectBoxCaption="Select" />
                                                                    <Items>
                                                                        <%--<asp:ListItem Value="All">All</asp:ListItem>--%>
                                                                        <asp:ListItem Value="AL">Alabama</asp:ListItem>
                                                                        <asp:ListItem Value="AK">Alaska</asp:ListItem>
                                                                        <asp:ListItem Value="AZ">Arizona</asp:ListItem>
                                                                        <asp:ListItem Value="AR">Arkansas</asp:ListItem>
                                                                        <asp:ListItem Value="CA">California</asp:ListItem>
                                                                        <asp:ListItem Value="CO">Colorado</asp:ListItem>
                                                                        <asp:ListItem Value="CT">Connecticut</asp:ListItem>
                                                                        <asp:ListItem Value="DC">District of Columbia</asp:ListItem>
                                                                        <asp:ListItem Value="DE">Delaware</asp:ListItem>
                                                                        <asp:ListItem Value="FL">Florida</asp:ListItem>
                                                                        <asp:ListItem Value="GA">Georgia</asp:ListItem>
                                                                        <asp:ListItem Value="HI">Hawaii</asp:ListItem>
                                                                        <asp:ListItem Value="ID">Idaho</asp:ListItem>
                                                                        <asp:ListItem Value="IL">Illinois</asp:ListItem>
                                                                        <asp:ListItem Value="IN">Indiana</asp:ListItem>
                                                                        <asp:ListItem Value="IA">Iowa</asp:ListItem>
                                                                        <asp:ListItem Value="KS">Kansas</asp:ListItem>
                                                                        <asp:ListItem Value="KY">Kentucky</asp:ListItem>
                                                                        <asp:ListItem Value="LA">Louisiana</asp:ListItem>
                                                                        <asp:ListItem Value="ME">Maine</asp:ListItem>
                                                                        <asp:ListItem Value="MD">Maryland</asp:ListItem>
                                                                        <asp:ListItem Value="MA">Massachusetts</asp:ListItem>
                                                                        <asp:ListItem Value="MI">Michigan</asp:ListItem>
                                                                        <asp:ListItem Value="MN">Minnesota</asp:ListItem>
                                                                        <asp:ListItem Value="MS">Mississippi</asp:ListItem>
                                                                        <asp:ListItem Value="MO">Missouri</asp:ListItem>
                                                                        <asp:ListItem Value="MT">Montana</asp:ListItem>
                                                                        <asp:ListItem Value="NE">Nebraska</asp:ListItem>
                                                                        <asp:ListItem Value="NV">Nevada</asp:ListItem>
                                                                        <asp:ListItem Value="NH">New Hampshire</asp:ListItem>
                                                                        <asp:ListItem Value="NJ">New Jersey</asp:ListItem>
                                                                        <asp:ListItem Value="NM">New Mexico</asp:ListItem>
                                                                        <asp:ListItem Value="NY">New York</asp:ListItem>
                                                                        <asp:ListItem Value="NC">North Carolina</asp:ListItem>
                                                                        <asp:ListItem Value="ND">North Dakota</asp:ListItem>
                                                                        <asp:ListItem Value="OH">Ohio</asp:ListItem>
                                                                        <asp:ListItem Value="OK">Oklahoma</asp:ListItem>
                                                                        <asp:ListItem Value="OR">Oregon</asp:ListItem>
                                                                        <asp:ListItem Value="PA">Pennsylvania</asp:ListItem>
                                                                        <asp:ListItem Value="RI">Rhode Island</asp:ListItem>
                                                                        <asp:ListItem Value="SC">South Carolina</asp:ListItem>
                                                                        <asp:ListItem Value="SD">South Dakota</asp:ListItem>
                                                                        <asp:ListItem Value="TN">Tennessee</asp:ListItem>
                                                                        <asp:ListItem Value="TX">Texas</asp:ListItem>
                                                                        <asp:ListItem Value="UT">Utah</asp:ListItem>
                                                                        <asp:ListItem Value="VT">Vermont</asp:ListItem>
                                                                        <asp:ListItem Value="VA">Virginia</asp:ListItem>
                                                                        <asp:ListItem Value="WA">Washington</asp:ListItem>
                                                                        <asp:ListItem Value="WV">West Virginia</asp:ListItem>
                                                                        <asp:ListItem Value="WI">Wisconsin</asp:ListItem>
                                                                        <asp:ListItem Value="WY">Wyoming</asp:ListItem>
                                                                        <asp:ListItem Value="AB">Alberta</asp:ListItem>
                                                                        <asp:ListItem Value="BC">British Columbia</asp:ListItem>
                                                                        <asp:ListItem Value="MB">Manitoba</asp:ListItem>
                                                                        <asp:ListItem Value="NB">New Brunswick</asp:ListItem>
                                                                        <asp:ListItem Value="NL">Newfoundland and Labrador</asp:ListItem>
                                                                        <asp:ListItem Value="NT">Northwest Territories</asp:ListItem>
                                                                        <asp:ListItem Value="NS">Nova Scotia</asp:ListItem>
                                                                        <asp:ListItem Value="NU">Nunavut</asp:ListItem>
                                                                        <asp:ListItem Value="PE">Prince Edward Island</asp:ListItem>
                                                                        <asp:ListItem Value="SK">Saskatchewan</asp:ListItem>
                                                                        <asp:ListItem Value="ON">Ontario</asp:ListItem>
                                                                        <asp:ListItem Value="QC">Quebec</asp:ListItem>
                                                                        <asp:ListItem Value="YT">Yukon</asp:ListItem>
                                                                    </Items>
                                                                </asp:DropDownCheckBoxes>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <table id="tblStateRef" style="display: none">
                                                        <tr>
                                                            <td>
                                                                <asp:DropDownList ID="ddlStateReference" runat="server" ToolTip="State" CssClass="register_input_bg"
                                                                    Width="150px">
                                                                    <asp:ListItem Value="All">All</asp:ListItem>
                                                                    <asp:ListItem Value="AL">Alabama</asp:ListItem>
                                                                    <asp:ListItem Value="AK">Alaska</asp:ListItem>
                                                                    <asp:ListItem Value="AZ">Arizona</asp:ListItem>
                                                                    <asp:ListItem Value="AR">Arkansas</asp:ListItem>
                                                                    <asp:ListItem Value="CA">California</asp:ListItem>
                                                                    <asp:ListItem Value="CO">Colorado</asp:ListItem>
                                                                    <asp:ListItem Value="CT">Connecticut</asp:ListItem>
                                                                    <asp:ListItem Value="DC">District of Columbia</asp:ListItem>
                                                                    <asp:ListItem Value="DE">Delaware</asp:ListItem>
                                                                    <asp:ListItem Value="FL">Florida</asp:ListItem>
                                                                    <asp:ListItem Value="GA">Georgia</asp:ListItem>
                                                                    <asp:ListItem Value="HI">Hawaii</asp:ListItem>
                                                                    <asp:ListItem Value="ID">Idaho</asp:ListItem>
                                                                    <asp:ListItem Value="IL">Illinois</asp:ListItem>
                                                                    <asp:ListItem Value="IN">Indiana</asp:ListItem>
                                                                    <asp:ListItem Value="IA">Iowa</asp:ListItem>
                                                                    <asp:ListItem Value="KS">Kansas</asp:ListItem>
                                                                    <asp:ListItem Value="KY">Kentucky</asp:ListItem>
                                                                    <asp:ListItem Value="LA">Louisiana</asp:ListItem>
                                                                    <asp:ListItem Value="ME">Maine</asp:ListItem>
                                                                    <asp:ListItem Value="MD">Maryland</asp:ListItem>
                                                                    <asp:ListItem Value="MA">Massachusetts</asp:ListItem>
                                                                    <asp:ListItem Value="MI">Michigan</asp:ListItem>
                                                                    <asp:ListItem Value="MN">Minnesota</asp:ListItem>
                                                                    <asp:ListItem Value="MS">Mississippi</asp:ListItem>
                                                                    <asp:ListItem Value="MO">Missouri</asp:ListItem>
                                                                    <asp:ListItem Value="MT">Montana</asp:ListItem>
                                                                    <asp:ListItem Value="NE">Nebraska</asp:ListItem>
                                                                    <asp:ListItem Value="NV">Nevada</asp:ListItem>
                                                                    <asp:ListItem Value="NH">New Hampshire</asp:ListItem>
                                                                    <asp:ListItem Value="NJ">New Jersey</asp:ListItem>
                                                                    <asp:ListItem Value="NM">New Mexico</asp:ListItem>
                                                                    <asp:ListItem Value="NY">New York</asp:ListItem>
                                                                    <asp:ListItem Value="NC">North Carolina</asp:ListItem>
                                                                    <asp:ListItem Value="ND">North Dakota</asp:ListItem>
                                                                    <asp:ListItem Value="OH">Ohio</asp:ListItem>
                                                                    <asp:ListItem Value="OK">Oklahoma</asp:ListItem>
                                                                    <asp:ListItem Value="OR">Oregon</asp:ListItem>
                                                                    <asp:ListItem Value="PA">Pennsylvania</asp:ListItem>
                                                                    <asp:ListItem Value="RI">Rhode Island</asp:ListItem>
                                                                    <asp:ListItem Value="SC">South Carolina</asp:ListItem>
                                                                    <asp:ListItem Value="SD">South Dakota</asp:ListItem>
                                                                    <asp:ListItem Value="TN">Tennessee</asp:ListItem>
                                                                    <asp:ListItem Value="TX">Texas</asp:ListItem>
                                                                    <asp:ListItem Value="UT">Utah</asp:ListItem>
                                                                    <asp:ListItem Value="VT">Vermont</asp:ListItem>
                                                                    <asp:ListItem Value="VA">Virginia</asp:ListItem>
                                                                    <asp:ListItem Value="WA">Washington</asp:ListItem>
                                                                    <asp:ListItem Value="WV">West Virginia</asp:ListItem>
                                                                    <asp:ListItem Value="WI">Wisconsin</asp:ListItem>
                                                                    <asp:ListItem Value="WY">Wyoming</asp:ListItem>
                                                                    <asp:ListItem Value="AB">Alberta</asp:ListItem>
                                                                    <asp:ListItem Value="BC">British Columbia</asp:ListItem>
                                                                    <asp:ListItem Value="MB">Manitoba</asp:ListItem>
                                                                    <asp:ListItem Value="NB">New Brunswick</asp:ListItem>
                                                                    <asp:ListItem Value="NL">Newfoundland and Labrador</asp:ListItem>
                                                                    <asp:ListItem Value="NT">Northwest Territories</asp:ListItem>
                                                                    <asp:ListItem Value="NS">Nova Scotia</asp:ListItem>
                                                                    <asp:ListItem Value="NU">Nunavut</asp:ListItem>
                                                                    <asp:ListItem Value="PE">Prince Edward Island</asp:ListItem>
                                                                    <asp:ListItem Value="SK">Saskatchewan</asp:ListItem>
                                                                    <asp:ListItem Value="ON">Ontario</asp:ListItem>
                                                                    <asp:ListItem Value="QC">Quebec</asp:ListItem>
                                                                    <asp:ListItem Value="YT">Yukon</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <table id="tblCustomer" style="display: none">
                                                        <tr>
                                                            <td>Customer
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <%--<asp:TextBox ID="txtName" runat="server" CssClass="register_input_bg" Width="150px"></asp:TextBox>--%>
                                                                <%--  <asp:DropDownList ID="drpName" runat="server" CssClass="register_input_bg" Width="150px">
                                                </asp:DropDownList>--%>
                                                                <asp:DropDownCheckBoxes ID="drpCustomer" runat="server" AddJQueryReference="false" UseButtons="false"
                                                                    Width="150px" CssClass="form-control" UseSelectAllNode="false">
                                                                    <Style2 SelectBoxWidth="200" DropDownBoxBoxWidth="200" DropDownBoxBoxHeight="170"
                                                                        SelectBoxCssClass="ListControl" />
                                                                    <Texts SelectBoxCaption="Select" />
                                                                </asp:DropDownCheckBoxes>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <table id="tblPreferredWorker" style="display: none">
                                                        <tr>
                                                            <td>Preferred Worker
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <%-- <asp:TextBox ID="txtCity" runat="server" CssClass="register_input_bg" Width="150px"></asp:TextBox>--%>
                                                                <asp:DropDownCheckBoxes ID="drpPreferredWorker" CssClass="form-control" runat="server" AddJQueryReference="false" UseButtons="false"
                                                                    Width="150px" UseSelectAllNode="false">
                                                                    <Style2 SelectBoxWidth="200" DropDownBoxBoxWidth="200" DropDownBoxBoxHeight="170"
                                                                        SelectBoxCssClass="ListControl" />
                                                                    <Texts SelectBoxCaption="Select" />
                                                                </asp:DropDownCheckBoxes>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <table id="tblZip" style="display: none">
                                                        <tr>
                                                            <td>Zip
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtZip" runat="server" CssClass="form-control" Width="150px"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <table id="tblPhone" style="display: none">
                                                        <tr>
                                                            <td>Phone
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtPhone" runat="server" CssClass="form-control" Width="150px"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <table id="tblFax" style="display: none">
                                                        <tr>
                                                            <td>Fax
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtFax" runat="server" CssClass="form-control" Width="150px"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <table id="tblContact" style="display: none">
                                                        <tr>
                                                            <td>Contact
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtContact" runat="server" CssClass="form-control" Width="150px"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <table id="tblDescription" style="display: none">
                                                        <tr>
                                                            <td>Description
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <%-- <asp:TextBox ID="txtAddress" runat="server" CssClass="register_input_bg" Width="150px"></asp:TextBox>--%>
                                                                <asp:DropDownCheckBoxes ID="drpDescription" runat="server" CssClass="form-control" AddJQueryReference="false"
                                                                    UseButtons="false" Width="150px" UseSelectAllNode="false">
                                                                    <Style2 SelectBoxWidth="200" DropDownBoxBoxWidth="200" DropDownBoxBoxHeight="170"
                                                                        SelectBoxCssClass="ListControl" />
                                                                    <Texts SelectBoxCaption="Select" />
                                                                </asp:DropDownCheckBoxes>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <table id="tblEmail" style="display: none">
                                                        <tr>
                                                            <td>Email
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" Width="150px"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <table id="tblCountry" style="display: none">
                                                        <tr>
                                                            <td>Country
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtCountry" runat="server" CssClass="form-control" Width="150px"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <table id="tblWebsite" style="display: none">
                                                        <tr>
                                                            <td>Website
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtWebsite" runat="server" CssClass="form-control" Width="150px"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <table id="tblCellular" style="display: none">
                                                        <tr>
                                                            <td>Cellular
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtCellular" runat="server" CssClass="form-control" Width="150px"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <table id="tblCategory" style="display: none">
                                                        <tr>
                                                            <td>Category
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:DropDownList ID="drpCategory" runat="server" CssClass="form-control" Width="150px">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <table id="tblBillStart" style="display: none">
                                                        <tr>
                                                            <td>Bill Start
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <%-- <asp:DropDownList ID="drpType" runat="server" CssClass="register_input_bg" Width="150px">
                                                </asp:DropDownList>--%>
                                                                <asp:DropDownCheckBoxes ID="drpBillStart" runat="server" AddJQueryReference="false" UseButtons="false"
                                                                    Width="150px" UseSelectAllNode="false" CssClass="form-control">
                                                                    <Style2 SelectBoxWidth="200" DropDownBoxBoxWidth="200" DropDownBoxBoxHeight="130"
                                                                        SelectBoxCssClass="ListControl" />
                                                                    <Texts SelectBoxCaption="Select" />
                                                                    <%-- <asp:ListItem Text="Mango" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="Apple" Value="2"></asp:ListItem>
                                                    <asp:ListItem Text="Banana" Value="3"></asp:ListItem>--%>
                                                                </asp:DropDownCheckBoxes>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <table id="tblBalance" style="display: none">
                                                        <tr>
                                                            <td colspan="4">Balance
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>&nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="4">
                                                                <asp:RadioButton ID="rdbAny" runat="server" GroupName="Balance" Text="Any" CssClass="ListControl" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:RadioButton ID="rdbEqual" runat="server" Width="40px" GroupName="Balance" Text="="
                                                                    CssClass="ListControl" />
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtBalEqual" runat="server" CssClass="form-control" Width="80px"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:RadioButton ID="rdbLessAndEqual" runat="server" Width="40px" GroupName="Balance"
                                                                    Text="&lt;=" CssClass="ListControl" />
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtBalLessAndEqual" runat="server" CssClass="form-control"
                                                                    Width="80px"></asp:TextBox>
                                                                <span style="padding-left: 10px;">and</span>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:RadioButton ID="rdbGreaterAndEqual" runat="server" Width="40px" GroupName="Balance"
                                                                    Text="&gt;=" CssClass="ListControl" />
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtBalGreaterAndEqual" runat="server" CssClass="form-control"
                                                                    Width="80px"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <table id="tblStatus" style="display: none">
                                                        <tr>
                                                            <td>Status
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:DropDownList ID="drpStatus" runat="server" CssClass="form-control" Width="150px">
                                                                    <asp:ListItem Value="Status">All</asp:ListItem>
                                                                    <asp:ListItem Value="0">Active</asp:ListItem>
                                                                    <asp:ListItem Value="1">Inactive</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <table id="tblLocationId" style="display: none">
                                                        <tr>
                                                            <td>Location Id
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:DropDownCheckBoxes ID="drpLocationId" runat="server" AddJQueryReference="false"
                                                                    UseButtons="false" Width="150px" UseSelectAllNode="false" CssClass="form-control">
                                                                    <Style2 SelectBoxWidth="200" DropDownBoxBoxWidth="200" DropDownBoxBoxHeight="130"
                                                                        SelectBoxCssClass="ListControl" />
                                                                    <Texts SelectBoxCaption="Select" />
                                                                </asp:DropDownCheckBoxes>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <table id="tblLocation" style="display: none">
                                                        <tr>
                                                            <td>Location Name
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:DropDownCheckBoxes ID="drpLocationName" runat="server" AddJQueryReference="false"
                                                                    UseButtons="false" Width="150px" CssClass="form-control" UseSelectAllNode="false">
                                                                    <Style2 SelectBoxWidth="200" DropDownBoxBoxWidth="200" DropDownBoxBoxHeight="130"
                                                                        SelectBoxCssClass="ListControl" />
                                                                    <Texts SelectBoxCaption="Select" />
                                                                </asp:DropDownCheckBoxes>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <table id="tblLocationAddress" style="display: none">
                                                        <tr>
                                                            <td>Location Address
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:DropDownCheckBoxes ID="drpLocationAddress" runat="server" AddJQueryReference="false"
                                                                    UseButtons="false" CssClass="form-control" Width="150px" UseSelectAllNode="false">
                                                                    <Style2 SelectBoxWidth="200" DropDownBoxBoxWidth="200" DropDownBoxBoxHeight="130"
                                                                        SelectBoxCssClass="ListControl" />
                                                                    <Texts SelectBoxCaption="Select" />
                                                                </asp:DropDownCheckBoxes>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <table id="tblTicketStart" style="display: none">
                                                        <tr>
                                                            <td>Ticket Start
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:DropDownCheckBoxes ID="drpTicketStart" runat="server" AddJQueryReference="false"
                                                                    UseButtons="false" Width="150px" CssClass="form-control" UseSelectAllNode="false">
                                                                    <Style2 SelectBoxWidth="200" DropDownBoxBoxWidth="200" DropDownBoxBoxHeight="130"
                                                                        SelectBoxCssClass="ListControl" />
                                                                    <Texts SelectBoxCaption="Select" />
                                                                </asp:DropDownCheckBoxes>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <table id="tblLocationState" style="display: none">
                                                        <tr>
                                                            <td>Location State
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:DropDownCheckBoxes ID="drpLocationState" runat="server" AddJQueryReference="false"
                                                                    UseButtons="false" Width="150px" CssClass="form-control" UseSelectAllNode="false">
                                                                    <Style2 SelectBoxWidth="200" DropDownBoxBoxWidth="200" DropDownBoxBoxHeight="130"
                                                                        SelectBoxCssClass="ListControl" />
                                                                    <Texts SelectBoxCaption="Select" />
                                                                    <Items>
                                                                        <%--<asp:ListItem Value="All">All</asp:ListItem>--%>
                                                                        <asp:ListItem Value="AL">Alabama</asp:ListItem>
                                                                        <asp:ListItem Value="AK">Alaska</asp:ListItem>
                                                                        <asp:ListItem Value="AZ">Arizona</asp:ListItem>
                                                                        <asp:ListItem Value="AR">Arkansas</asp:ListItem>
                                                                        <asp:ListItem Value="CA">California</asp:ListItem>
                                                                        <asp:ListItem Value="CO">Colorado</asp:ListItem>
                                                                        <asp:ListItem Value="CT">Connecticut</asp:ListItem>
                                                                        <asp:ListItem Value="DC">District of Columbia</asp:ListItem>
                                                                        <asp:ListItem Value="DE">Delaware</asp:ListItem>
                                                                        <asp:ListItem Value="FL">Florida</asp:ListItem>
                                                                        <asp:ListItem Value="GA">Georgia</asp:ListItem>
                                                                        <asp:ListItem Value="HI">Hawaii</asp:ListItem>
                                                                        <asp:ListItem Value="ID">Idaho</asp:ListItem>
                                                                        <asp:ListItem Value="IL">Illinois</asp:ListItem>
                                                                        <asp:ListItem Value="IN">Indiana</asp:ListItem>
                                                                        <asp:ListItem Value="IA">Iowa</asp:ListItem>
                                                                        <asp:ListItem Value="KS">Kansas</asp:ListItem>
                                                                        <asp:ListItem Value="KY">Kentucky</asp:ListItem>
                                                                        <asp:ListItem Value="LA">Louisiana</asp:ListItem>
                                                                        <asp:ListItem Value="ME">Maine</asp:ListItem>
                                                                        <asp:ListItem Value="MD">Maryland</asp:ListItem>
                                                                        <asp:ListItem Value="MA">Massachusetts</asp:ListItem>
                                                                        <asp:ListItem Value="MI">Michigan</asp:ListItem>
                                                                        <asp:ListItem Value="MN">Minnesota</asp:ListItem>
                                                                        <asp:ListItem Value="MS">Mississippi</asp:ListItem>
                                                                        <asp:ListItem Value="MO">Missouri</asp:ListItem>
                                                                        <asp:ListItem Value="MT">Montana</asp:ListItem>
                                                                        <asp:ListItem Value="NE">Nebraska</asp:ListItem>
                                                                        <asp:ListItem Value="NV">Nevada</asp:ListItem>
                                                                        <asp:ListItem Value="NH">New Hampshire</asp:ListItem>
                                                                        <asp:ListItem Value="NJ">New Jersey</asp:ListItem>
                                                                        <asp:ListItem Value="NM">New Mexico</asp:ListItem>
                                                                        <asp:ListItem Value="NY">New York</asp:ListItem>
                                                                        <asp:ListItem Value="NC">North Carolina</asp:ListItem>
                                                                        <asp:ListItem Value="ND">North Dakota</asp:ListItem>
                                                                        <asp:ListItem Value="OH">Ohio</asp:ListItem>
                                                                        <asp:ListItem Value="OK">Oklahoma</asp:ListItem>
                                                                        <asp:ListItem Value="OR">Oregon</asp:ListItem>
                                                                        <asp:ListItem Value="PA">Pennsylvania</asp:ListItem>
                                                                        <asp:ListItem Value="RI">Rhode Island</asp:ListItem>
                                                                        <asp:ListItem Value="SC">South Carolina</asp:ListItem>
                                                                        <asp:ListItem Value="SD">South Dakota</asp:ListItem>
                                                                        <asp:ListItem Value="TN">Tennessee</asp:ListItem>
                                                                        <asp:ListItem Value="TX">Texas</asp:ListItem>
                                                                        <asp:ListItem Value="UT">Utah</asp:ListItem>
                                                                        <asp:ListItem Value="VT">Vermont</asp:ListItem>
                                                                        <asp:ListItem Value="VA">Virginia</asp:ListItem>
                                                                        <asp:ListItem Value="WA">Washington</asp:ListItem>
                                                                        <asp:ListItem Value="WV">West Virginia</asp:ListItem>
                                                                        <asp:ListItem Value="WI">Wisconsin</asp:ListItem>
                                                                        <asp:ListItem Value="WY">Wyoming</asp:ListItem>
                                                                        <asp:ListItem Value="AB">Alberta</asp:ListItem>
                                                                        <asp:ListItem Value="BC">British Columbia</asp:ListItem>
                                                                        <asp:ListItem Value="MB">Manitoba</asp:ListItem>
                                                                        <asp:ListItem Value="NB">New Brunswick</asp:ListItem>
                                                                        <asp:ListItem Value="NL">Newfoundland and Labrador</asp:ListItem>
                                                                        <asp:ListItem Value="NT">Northwest Territories</asp:ListItem>
                                                                        <asp:ListItem Value="NS">Nova Scotia</asp:ListItem>
                                                                        <asp:ListItem Value="NU">Nunavut</asp:ListItem>
                                                                        <asp:ListItem Value="PE">Prince Edward Island</asp:ListItem>
                                                                        <asp:ListItem Value="SK">Saskatchewan</asp:ListItem>
                                                                        <asp:ListItem Value="ON">Ontario</asp:ListItem>
                                                                        <asp:ListItem Value="QC">Quebec</asp:ListItem>
                                                                        <asp:ListItem Value="YT">Yukon</asp:ListItem>
                                                                    </Items>
                                                                </asp:DropDownCheckBoxes>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <table id="tblBillAmount" style="display: none">
                                                        <tr>
                                                            <td>Bill Amount
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <%--  <asp:TextBox ID="txtLocationZip" runat="server" CssClass="register_input_bg" Width="150px"></asp:TextBox>--%>
                                                                <asp:DropDownCheckBoxes ID="drpBillAmount" runat="server" AddJQueryReference="false"
                                                                    UseButtons="false" Width="150px" CssClass="form-control" UseSelectAllNode="false">
                                                                    <Style2 SelectBoxWidth="200" DropDownBoxBoxWidth="200" DropDownBoxBoxHeight="130"
                                                                        SelectBoxCssClass="ListControl" />
                                                                    <Texts SelectBoxCaption="Select" />
                                                                </asp:DropDownCheckBoxes>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <table id="tblLocType" style="display: none">
                                                        <tr>
                                                            <td>Location Type
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:DropDownCheckBoxes ID="drpLocationType" runat="server" AddJQueryReference="false"
                                                                    UseButtons="false" Width="150px" CssClass="form-control" UseSelectAllNode="false">
                                                                    <Style2 SelectBoxWidth="200" DropDownBoxBoxWidth="200" DropDownBoxBoxHeight="130"
                                                                        SelectBoxCssClass="ListControl" />
                                                                    <Texts SelectBoxCaption="Select" />
                                                                </asp:DropDownCheckBoxes>
                                                            </td>
                                                        </tr>
                                                    </table>

                                                     <table id="tblTicketTime" style="display: none">
                                                        <tr>
                                                            <td>Ticket Time
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:DropDownCheckBoxes ID="drpTicketTime" runat="server" AddJQueryReference="false"
                                                                    UseButtons="false" Width="150px" CssClass="form-control" UseSelectAllNode="false">
                                                                    <Style2 SelectBoxWidth="200" DropDownBoxBoxWidth="200" DropDownBoxBoxHeight="130"
                                                                        SelectBoxCssClass="ListControl" />
                                                                    <Texts SelectBoxCaption="Select" />
                                                                </asp:DropDownCheckBoxes>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                   
                                                    <table id="tblHours" style="display: none">
                                                        <tr>
                                                            <td>Hours
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:DropDownCheckBoxes ID="drpHours" runat="server" AddJQueryReference="false" UseButtons="false"
                                                                    Width="150px" UseSelectAllNode="false" CssClass="form-control">
                                                                    <Style2 SelectBoxWidth="200" DropDownBoxBoxWidth="200" DropDownBoxBoxHeight="130"
                                                                        SelectBoxCssClass="ListControl" />
                                                                    <Texts SelectBoxCaption="Select" />
                                                                </asp:DropDownCheckBoxes>
                                                            </td>
                                                        </tr>
                                                    </table>

                                                     <table id="tblEquipment" style="display: none">
                                                        <tr>
                                                            <td>Equipment
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:DropDownCheckBoxes ID="drpEquipment" runat="server" AddJQueryReference="false" UseButtons="false"
                                                                    Width="150px" UseSelectAllNode="false" CssClass="form-control">
                                                                    <Style2 SelectBoxWidth="200" DropDownBoxBoxWidth="200" DropDownBoxBoxHeight="130"
                                                                        SelectBoxCssClass="ListControl" />
                                                                    <Texts SelectBoxCaption="Select" />
                                                                </asp:DropDownCheckBoxes>
                                                            </td>
                                                        </tr>
                                                    </table>

                                                     <table id="tblExpiration" style="display: none">
                                                        <tr>
                                                            <td>Expiration
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:DropDownCheckBoxes ID="drpExpiration" runat="server" AddJQueryReference="false" UseButtons="false"
                                                                    Width="150px" UseSelectAllNode="false" CssClass="form-control">
                                                                    <Style2 SelectBoxWidth="200" DropDownBoxBoxWidth="200" DropDownBoxBoxHeight="130"
                                                                        SelectBoxCssClass="ListControl" />
                                                                    <Texts SelectBoxCaption="Select" />
                                                                </asp:DropDownCheckBoxes>
                                                            </td>
                                                        </tr>
                                                    </table>

                                                    <table id="tblExpirationDate" style="display: none">
                                                        <tr>
                                                            <td>Expiration Date
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:DropDownCheckBoxes ID="drpExpirationDate" runat="server" AddJQueryReference="false" UseButtons="false"
                                                                    Width="150px" UseSelectAllNode="false" CssClass="form-control">
                                                                    <Style2 SelectBoxWidth="200" DropDownBoxBoxWidth="200" DropDownBoxBoxHeight="130"
                                                                        SelectBoxCssClass="ListControl" />
                                                                    <Texts SelectBoxCaption="Select" />
                                                                </asp:DropDownCheckBoxes>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <table id="tblPhoneMonitoring" style="display: none">
                                                        <tr>
                                                            <td>Phone Monitoring
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:DropDownCheckBoxes ID="drpPhoneMonitoring" runat="server" AddJQueryReference="false" UseButtons="false"
                                                                    Width="150px" UseSelectAllNode="false" CssClass="form-control">
                                                                    <Style2 SelectBoxWidth="200" DropDownBoxBoxWidth="200" DropDownBoxBoxHeight="130"
                                                                        SelectBoxCssClass="ListControl" />
                                                                    <Texts SelectBoxCaption="Select" />
                                                                </asp:DropDownCheckBoxes>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                     <table id="tblContractType" style="display: none">
                                                        <tr>
                                                            <td>Contract Type
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:DropDownCheckBoxes ID="drpContractType" runat="server" AddJQueryReference="false" UseButtons="false"
                                                                    Width="150px" UseSelectAllNode="false" CssClass="form-control">
                                                                    <Style2 SelectBoxWidth="200" DropDownBoxBoxWidth="200" DropDownBoxBoxHeight="130"
                                                                        SelectBoxCssClass="ListControl" />
                                                                    <Texts SelectBoxCaption="Select" />
                                                                </asp:DropDownCheckBoxes>
                                                            </td>
                                                        </tr>
                                                    </table>

                                                     <table id="tblOccupancyDiscount" style="display: none">
                                                        <tr>
                                                            <td>Occupancy Discount
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:DropDownCheckBoxes ID="drpOccupancyDiscount" runat="server" AddJQueryReference="false" UseButtons="false"
                                                                    Width="150px" UseSelectAllNode="false" CssClass="form-control">
                                                                    <Style2 SelectBoxWidth="200" DropDownBoxBoxWidth="200" DropDownBoxBoxHeight="130"
                                                                        SelectBoxCssClass="ListControl" />
                                                                    <Texts SelectBoxCaption="Select" />
                                                                </asp:DropDownCheckBoxes>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                      <table id="tblExclusions" style="display: none">
                                                        <tr>
                                                            <td>Exclusions
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:DropDownCheckBoxes ID="drpExclusions" runat="server" AddJQueryReference="false" UseButtons="false"
                                                                    Width="150px" UseSelectAllNode="false" CssClass="form-control">
                                                                    <Style2 SelectBoxWidth="200" DropDownBoxBoxWidth="200" DropDownBoxBoxHeight="130"
                                                                        SelectBoxCssClass="ListControl" />
                                                                    <Texts SelectBoxCaption="Select" />
                                                                </asp:DropDownCheckBoxes>
                                                            </td>
                                                        </tr>
                                                    </table>

                                                      <table id="tblTermOfContract" style="display: none">
                                                        <tr>
                                                            <td>Term Of Contract
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:DropDownCheckBoxes ID="drpTermOfContract" runat="server" AddJQueryReference="false" UseButtons="false"
                                                                    Width="150px" UseSelectAllNode="false" CssClass="form-control">
                                                                    <Style2 SelectBoxWidth="200" DropDownBoxBoxWidth="200" DropDownBoxBoxHeight="130"
                                                                        SelectBoxCssClass="ListControl" />
                                                                    <Texts SelectBoxCaption="Select" />
                                                                </asp:DropDownCheckBoxes>
                                                            </td>
                                                        </tr>
                                                    </table>

                                                     <table id="tblPriceAdjustmentCap" style="display: none">
                                                        <tr>
                                                            <td>Price AdjustmentCap
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:DropDownCheckBoxes ID="drpPriceAdjustmentCap" runat="server" AddJQueryReference="false" UseButtons="false"
                                                                    Width="150px" UseSelectAllNode="false" CssClass="form-control">
                                                                    <Style2 SelectBoxWidth="200" DropDownBoxBoxWidth="200" DropDownBoxBoxHeight="130"
                                                                        SelectBoxCssClass="ListControl" />
                                                                    <Texts SelectBoxCaption="Select" />
                                                                </asp:DropDownCheckBoxes>
                                                            </td>
                                                        </tr>
                                                    </table>

                                                     <table id="tblFireServiceTestingIncluded" style="display: none">
                                                        <tr>
                                                            <td>Fire Service Testing Included
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:DropDownCheckBoxes ID="drpFireServiceTestingIncluded" runat="server" AddJQueryReference="false" UseButtons="false"
                                                                    Width="150px" UseSelectAllNode="false" CssClass="form-control">
                                                                    <Style2 SelectBoxWidth="200" DropDownBoxBoxWidth="200" DropDownBoxBoxHeight="130"
                                                                        SelectBoxCssClass="ListControl" />
                                                                    <Texts SelectBoxCaption="Select" />
                                                                </asp:DropDownCheckBoxes>
                                                            </td>
                                                        </tr>
                                                    </table>

                                                     <table id="tblSpecialRates" style="display: none">
                                                        <tr>
                                                            <td>Special Rates
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:DropDownCheckBoxes ID="drpSpecialRates" runat="server" AddJQueryReference="false" UseButtons="false"
                                                                    Width="150px" UseSelectAllNode="false" CssClass="form-control">
                                                                    <Style2 SelectBoxWidth="200" DropDownBoxBoxWidth="200" DropDownBoxBoxHeight="130"
                                                                        SelectBoxCssClass="ListControl" />
                                                                    <Texts SelectBoxCaption="Select" />
                                                                </asp:DropDownCheckBoxes>
                                                            </td>
                                                        </tr>
                                                    </table>

                                                     <table id="tblContractExpiration" style="display: none">
                                                        <tr>
                                                            <td>Contract Expiration
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:DropDownCheckBoxes ID="drpContractExpiration" runat="server" AddJQueryReference="false" UseButtons="false"
                                                                    Width="150px" UseSelectAllNode="false" CssClass="form-control">
                                                                    <Style2 SelectBoxWidth="200" DropDownBoxBoxWidth="200" DropDownBoxBoxHeight="130"
                                                                        SelectBoxCssClass="ListControl" />
                                                                    <Texts SelectBoxCaption="Select" />
                                                                </asp:DropDownCheckBoxes>
                                                            </td>
                                                        </tr>
                                                    </table>

                                                    
                                                     <table id="tblAnnualTestIncluded" style="display: none">
                                                        <tr>
                                                            <td>Annual Test Included
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:DropDownCheckBoxes ID="drpAnnualTestIncluded" runat="server" AddJQueryReference="false" UseButtons="false"
                                                                    Width="150px" UseSelectAllNode="false" CssClass="form-control">
                                                                    <Style2 SelectBoxWidth="200" DropDownBoxBoxWidth="200" DropDownBoxBoxHeight="130"
                                                                        SelectBoxCssClass="ListControl" />
                                                                    <Texts SelectBoxCaption="Select" />
                                                                </asp:DropDownCheckBoxes>
                                                            </td>
                                                        </tr>
                                                    </table>

                                                      <table id="tblProratedItems" style="display: none">
                                                        <tr>
                                                            <td>Prorated Items
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:DropDownCheckBoxes ID="drpProratedItems" runat="server" AddJQueryReference="false" UseButtons="false"
                                                                    Width="150px" UseSelectAllNode="false" CssClass="form-control">
                                                                    <Style2 SelectBoxWidth="200" DropDownBoxBoxWidth="200" DropDownBoxBoxHeight="130"
                                                                        SelectBoxCssClass="ListControl" />
                                                                    <Texts SelectBoxCaption="Select" />
                                                                </asp:DropDownCheckBoxes>
                                                            </td>
                                                        </tr>
                                                    </table>

                                                     <table id="tblFiveYearStateTestIncluded" style="display: none">
                                                        <tr>
                                                            <td>Five Year State Test Included
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:DropDownCheckBoxes ID="drpFiveYearStateTestIncluded" runat="server" AddJQueryReference="false" UseButtons="false"
                                                                    Width="150px" UseSelectAllNode="false" CssClass="form-control">
                                                                    <Style2 SelectBoxWidth="200" DropDownBoxBoxWidth="200" DropDownBoxBoxHeight="130"
                                                                        SelectBoxCssClass="ListControl" />
                                                                    <Texts SelectBoxCaption="Select" />
                                                                </asp:DropDownCheckBoxes>
                                                            </td>
                                                        </tr>
                                                    </table>

                                                    
                                                     <table id="tblFireServiceTestedIncluded" style="display: none">
                                                        <tr>
                                                            <td>Fire Service Tested Included
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:DropDownCheckBoxes ID="drpFireServiceTestedIncluded" runat="server" AddJQueryReference="false" UseButtons="false"
                                                                    Width="150px" UseSelectAllNode="false" CssClass="form-control">
                                                                    <Style2 SelectBoxWidth="200" DropDownBoxBoxWidth="200" DropDownBoxBoxHeight="130"
                                                                        SelectBoxCssClass="ListControl" />
                                                                    <Texts SelectBoxCaption="Select" />
                                                                </asp:DropDownCheckBoxes>
                                                            </td>
                                                        </tr>
                                                    </table>

                                                        <table id="tblCancellationNotificationDays" style="display: none">
                                                        <tr>
                                                            <td>Cancellation Notification Days
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:DropDownCheckBoxes ID="drpCancellationNotificationDays" runat="server" AddJQueryReference="false" UseButtons="false"
                                                                    Width="150px" UseSelectAllNode="false" CssClass="form-control">
                                                                    <Style2 SelectBoxWidth="200" DropDownBoxBoxWidth="200" DropDownBoxBoxHeight="130"
                                                                        SelectBoxCssClass="ListControl" />
                                                                    <Texts SelectBoxCaption="Select" />
                                                                </asp:DropDownCheckBoxes>
                                                            </td>
                                                        </tr>
                                                    </table>

                                                      <table id="tblPriceAdjustmentNotificationDays" style="display: none">
                                                        <tr>
                                                            <td>Price Adjustment Notification Days
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:DropDownCheckBoxes ID="drpPriceAdjustmentNotificationDays" runat="server" AddJQueryReference="false" UseButtons="false"
                                                                    Width="150px" UseSelectAllNode="false" CssClass="form-control">
                                                                    <Style2 SelectBoxWidth="200" DropDownBoxBoxWidth="200" DropDownBoxBoxHeight="130"
                                                                        SelectBoxCssClass="ListControl" />
                                                                    <Texts SelectBoxCaption="Select" />
                                                                </asp:DropDownCheckBoxes>
                                                            </td>
                                                        </tr>
                                                    </table>

                                                    
                                                      <table id="tblAfterHoursCallsIncluded" style="display: none">
                                                        <tr>
                                                            <td>After Hours Calls Included
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:DropDownCheckBoxes ID="drpAfterHoursCallsIncluded" runat="server" AddJQueryReference="false" UseButtons="false"
                                                                    Width="150px" UseSelectAllNode="false" CssClass="form-control">
                                                                    <Style2 SelectBoxWidth="200" DropDownBoxBoxWidth="200" DropDownBoxBoxHeight="130"
                                                                        SelectBoxCssClass="ListControl" />
                                                                    <Texts SelectBoxCaption="Select" />
                                                                </asp:DropDownCheckBoxes>
                                                            </td>
                                                        </tr>
                                                    </table>

                                                    
                                                      <table id="tblOGServiceCallsIncluded" style="display: none">
                                                        <tr>
                                                            <td>OG Service Calls Included
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:DropDownCheckBoxes ID="drpOGServiceCallsIncluded" runat="server" AddJQueryReference="false" UseButtons="false"
                                                                    Width="150px" UseSelectAllNode="false" CssClass="form-control">
                                                                    <Style2 SelectBoxWidth="200" DropDownBoxBoxWidth="200" DropDownBoxBoxHeight="130"
                                                                        SelectBoxCssClass="ListControl" />
                                                                    <Texts SelectBoxCaption="Select" />
                                                                </asp:DropDownCheckBoxes>
                                                            </td>
                                                        </tr>
                                                    </table>

                                                    <table id="tblContractHours" style="display: none">
                                                        <tr>
                                                            <td>Contract Hours
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:DropDownCheckBoxes ID="drpContractHours" runat="server" AddJQueryReference="false" UseButtons="false"
                                                                    Width="150px" UseSelectAllNode="false" CssClass="form-control">
                                                                    <Style2 SelectBoxWidth="200" DropDownBoxBoxWidth="200" DropDownBoxBoxHeight="130"
                                                                        SelectBoxCssClass="ListControl" />
                                                                    <Texts SelectBoxCaption="Select" />
                                                                </asp:DropDownCheckBoxes>
                                                            </td>
                                                        </tr>
                                                    </table>

                                                     <table id="tblContractFormat" style="display: none">
                                                        <tr>
                                                            <td>Contract Format
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:DropDownCheckBoxes ID="drpContractFormat" runat="server" AddJQueryReference="false" UseButtons="false"
                                                                    Width="150px" UseSelectAllNode="false" CssClass="form-control">
                                                                    <Style2 SelectBoxWidth="200" DropDownBoxBoxWidth="200" DropDownBoxBoxHeight="130"
                                                                        SelectBoxCssClass="ListControl" />
                                                                    <Texts SelectBoxCaption="Select" />
                                                                </asp:DropDownCheckBoxes>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <table id="tblTicketFreq" style="display: none">
                                                        <tr>
                                                            <td>Ticket Type
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:DropDownCheckBoxes ID="drpTicketFreq" runat="server" AddJQueryReference="false"
                                                                    UseButtons="false" Width="150px" UseSelectAllNode="false" CssClass="form-control">
                                                                    <Style2 SelectBoxWidth="200" DropDownBoxBoxWidth="200" DropDownBoxBoxHeight="130"
                                                                        SelectBoxCssClass="ListControl" />
                                                                    <Texts SelectBoxCaption="Select" />
                                                                </asp:DropDownCheckBoxes>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <table id="tblServiceType" style="display: none">
                                                        <tr>
                                                            <td>Service Type
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:DropDownCheckBoxes ID="drpServiceType" runat="server" AddJQueryReference="false"
                                                                    UseButtons="false" Width="150px" UseSelectAllNode="false" CssClass="form-control">
                                                                    <Style2 SelectBoxWidth="200" DropDownBoxBoxWidth="200" DropDownBoxBoxHeight="130"
                                                                        SelectBoxCssClass="ListControl" />
                                                                    <Texts SelectBoxCaption="Select" />
                                                                </asp:DropDownCheckBoxes>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <table id="tblEquipmentPrice" style="display: none">
                                                        <tr>
                                                            <td colspan="4">Equipment Price
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>&nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="4">
                                                                <asp:RadioButton ID="rdbEquipmentPriceAny" runat="server" GroupName="ep" Text="Any"
                                                                    CssClass="ListControl" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:RadioButton ID="rdbEquipmentPriceEqual" runat="server" Width="40px" GroupName="ep"
                                                                    Text="=" CssClass="ListControl" />
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtEquipmentPriceEqual" runat="server" CssClass="form-control"
                                                                    Width="80px"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:RadioButton ID="rdbEquipmentPriceGreaterAndEqual" runat="server" Width="40px"
                                                                    GroupName="ep" Text="&gt;=" CssClass="ListControl" />
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtEquipmentPriceGreatAndEqual" runat="server" CssClass="form-control"
                                                                    Width="80px"></asp:TextBox>
                                                                <span style="padding-left: 10px;">and</span>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:RadioButton ID="rdbEquipmentPriceLessAndEqual" runat="server" Width="40px" GroupName="ep"
                                                                    Text="&lt;=" CssClass="ListControl" />
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtEquipmentPriceLessAndEqual" runat="server" CssClass="form-control"
                                                                    Width="80px"></asp:TextBox>
                                                            </td>
                                                        </tr>

                                                    </table>
                                                    <table id="tblLoc" style="display: none">
                                                        <tr>
                                                            <td colspan="4">Loc
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>&nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="4">
                                                                <asp:RadioButton ID="rdbLocAny" runat="server" GroupName="loc" Text="Any" CssClass="ListControl" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:RadioButton ID="rdbLocEqual" runat="server" Width="40px" GroupName="loc" Text="="
                                                                    CssClass="ListControl" />
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtLocEqual" runat="server" CssClass="form-control" Width="80px"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:RadioButton ID="rdbLocGreaterAndEqual" runat="server" Width="40px" GroupName="loc"
                                                                    Text="&gt;=" CssClass="ListControl" />
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtLocGreatAndEqual" runat="server" CssClass="form-control"
                                                                    Width="80px"></asp:TextBox>
                                                                <span style="padding-left: 10px;">and</span>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:RadioButton ID="rdbLocLessAndEqual" runat="server" Width="40px" GroupName="loc"
                                                                    Text="&lt;=" CssClass="ListControl" />
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtLocLessAndEqual" runat="server" CssClass="form-control"
                                                                    Width="80px"></asp:TextBox>
                                                            </td>
                                                        </tr>

                                                    </table>
                                                    <table id="tblEquip" style="display: none">
                                                        <tr>
                                                            <td colspan="4">Equipment
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>&nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="4">
                                                                <asp:RadioButton ID="rdbEquipAny" runat="server" GroupName="equip" Text="Any" CssClass="ListControl" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:RadioButton ID="rdbEquipEqual" runat="server" Width="40px" GroupName="equip"
                                                                    Text="=" CssClass="ListControl" />
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtEquipEqual" runat="server" CssClass="form-control" Width="80px"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:RadioButton ID="rdbEquipGreaterAndEqual" runat="server" Width="40px" GroupName="equip"
                                                                    Text="&gt;=" CssClass="ListControl" />
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtEquipGreatAndEqual" runat="server" CssClass="form-control"
                                                                    Width="80px"></asp:TextBox>
                                                                <span style="padding-left: 10px;">and</span>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:RadioButton ID="rdbEquipLessAndEqual" runat="server" Width="40px" GroupName="equip"
                                                                    Text="&lt;=" CssClass="ListControl" />
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtEquipLessAndEqual" runat="server" CssClass="form-control"
                                                                    Width="80px"></asp:TextBox>

                                                            </td>
                                                        </tr>

                                                    </table>
                                                    <table id="tblOpenCalls" style="display: none">
                                                        <tr>
                                                            <td colspan="4">Open Calls
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>&nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="4">
                                                                <asp:RadioButton ID="rdbOCAny" runat="server" GroupName="oc" Text="Any" CssClass="ListControl" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:RadioButton ID="rdbOCEqual" runat="server" Width="40px" GroupName="oc" Text="="
                                                                    CssClass="ListControl" />
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtOCEqual" runat="server" CssClass="form-control" Width="80px"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:RadioButton ID="rdbOCGreaterAndEqual" runat="server" Width="40px" GroupName="oc"
                                                                    Text="&gt;=" CssClass="ListControl" />
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtOCGreatAndEqual" runat="server" CssClass="form-control"
                                                                    Width="80px"></asp:TextBox>
                                                                <span style="padding-left: 10px;">and</span>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:RadioButton ID="rdbOCLessAndEqual" runat="server" Width="40px" GroupName="oc"
                                                                    Text="&lt;=" CssClass="ListControl" />
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtOCLessAndEqual" runat="server" CssClass="form-control" Width="80px"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <table id="tblBillFreqency" style="display: none">
                                                        <tr>
                                                            <td>Bill Freqency
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:DropDownCheckBoxes ID="drpBillFreqency" runat="server" CssClass="form-control" AddJQueryReference="false" UseButtons="false"
                                                                    Width="150px" UseSelectAllNode="false">
                                                                    <Style2 SelectBoxWidth="200" DropDownBoxBoxWidth="200" DropDownBoxBoxHeight="170"
                                                                        SelectBoxCssClass="ListControl" />
                                                                    <Texts SelectBoxCaption="Select" />
                                                                </asp:DropDownCheckBoxes>
                                                                <%--<asp:DropDownCheckBoxes ID="drpRoute" runat="server" AddJQueryReference="false" UseButtons="false"
                                                                    Width="150px" CssClass="form-control" UseSelectAllNode="false">
                                                                    <Style2 SelectBoxWidth="200" DropDownBoxBoxWidth="200" DropDownBoxBoxHeight="170"
                                                                        SelectBoxCssClass="ListControl" />
                                                                    <Texts SelectBoxCaption="Select" />
                                                                </asp:DropDownCheckBoxes>--%>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <table id="tblBuildingType" style="display: none">
                                                        <tr>
                                                            <td>Building Type
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:DropDownCheckBoxes ID="drpBuldingType" runat="server" CssClass="form-control" AddJQueryReference="false" UseButtons="false"
                                                                    Width="150px" UseSelectAllNode="false">
                                                                    <Style2 SelectBoxWidth="200" DropDownBoxBoxWidth="200" DropDownBoxBoxHeight="170"
                                                                        SelectBoxCssClass="ListControl" />
                                                                    <Texts SelectBoxCaption="Select" />
                                                                </asp:DropDownCheckBoxes>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <table id="tblEquipmentState" style="display: none">
                                                        <tr>
                                                            <td>Equipment State
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:DropDownCheckBoxes ID="drpEquipmentState" runat="server" CssClass="form-control" AddJQueryReference="false" UseButtons="false"
                                                                    Width="150px" UseSelectAllNode="false">
                                                                    <Style2 SelectBoxWidth="200" DropDownBoxBoxWidth="200" DropDownBoxBoxHeight="170"
                                                                        SelectBoxCssClass="ListControl" />
                                                                    <Texts SelectBoxCaption="Select" />
                                                                </asp:DropDownCheckBoxes>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <table id="tblInstalledOn" style="display: none">
                                                        <tr>
                                                            <td>Installed on
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtInstalledOn" runat="server" CssClass="form-control" MaxLength="50"
                                                                    TabIndex="2"></asp:TextBox>
                                                                <asp:CalendarExtender ID="txtInstalledOn_CalendarExtender" runat="server" Enabled="True"
                                                                    TargetControlID="txtInstalledOn">
                                                                </asp:CalendarExtender>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <table id="tblDefaultSalesPerson" style="display: none">
                                                        <tr>
                                                            <td>Default Sales Person
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:DropDownCheckBoxes ID="drpDefaultSalesPerson" runat="server" CssClass="form-control" AddJQueryReference="false" UseButtons="false"
                                                                    Width="150px" UseSelectAllNode="false">
                                                                    <Style2 SelectBoxWidth="200" DropDownBoxBoxWidth="200" DropDownBoxBoxHeight="170"
                                                                        SelectBoxCssClass="ListControl" />
                                                                    <Texts SelectBoxCaption="Select" />
                                                                </asp:DropDownCheckBoxes>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <table id="tblLocationSTax" style="display: none">
                                                        <tr>
                                                            <td>Location STax
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:DropDownCheckBoxes ID="drpLocationSTax" runat="server" CssClass="form-control" AddJQueryReference="false" UseButtons="false"
                                                                    Width="150px" UseSelectAllNode="false">
                                                                    <Style2 SelectBoxWidth="200" DropDownBoxBoxWidth="200" DropDownBoxBoxHeight="170"
                                                                        SelectBoxCssClass="ListControl" />
                                                                    <Texts SelectBoxCaption="Select" />
                                                                </asp:DropDownCheckBoxes>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <table id="tblEquipmentCounts" style="display: none">
                                                        <tr>
                                                            <td colspan="4">Equipment Counts
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>&nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="4">
                                                                <asp:RadioButton ID="rdbEquipmentCountsAny" runat="server" GroupName="equipmentcounts" Text="Any" CssClass="ListControl" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:RadioButton ID="rdbEquipmentCountsEqual" runat="server" Width="40px" GroupName="equipmentcounts"
                                                                    Text="=" CssClass="ListControl" />
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtEquipmentCountsEqual" runat="server" CssClass="form-control" Width="80px"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:RadioButton ID="rdbEquipmentCountsGreaterAndEqual" runat="server" Width="40px" GroupName="equipmentcounts"
                                                                    Text="&gt;=" CssClass="ListControl" />
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtEquipmentCountsGreatAndEqual" runat="server" CssClass="form-control"
                                                                    Width="80px"></asp:TextBox>
                                                                <span style="padding-left: 10px;">and</span>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:RadioButton ID="rdbEquipmentCountsLessAndEqual" runat="server" Width="40px" GroupName="equipmentcounts"
                                                                    Text="&lt;=" CssClass="ListControl" />
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtEquipmentCountsLessAndEqual" runat="server" CssClass="form-control"
                                                                    Width="80px"></asp:TextBox>

                                                            </td>
                                                        </tr>

                                                    </table>
                                                </div>
                                            </fieldset>
                                        </div>
                                        <div style="float: right;">
                                            <fieldset style="border-color: #CDCDCD; width: 260px;">
                                                <legend style="color: Black; font-size: 11px;"><b>CURRENT FILTER CHOICES</b></legend>
                                                <div style="margin-top: 20px; margin-left: 7px;">
                                                    <div style="height: 150px; border: solid 1px black; overflow: auto;">
                                                        <table width="247px">
                                                            <thead>
                                                                <tr style="background: gray; color: White;">
                                                                    <td width="100px" style="height: 25px;">FILTER
                                                                    </td>
                                                                    <td width="220px" style="height: 25px">SET TO
                                                                    </td>
                                                                </tr>
                                                            </thead>
                                                        </table>
                                                        <table id="tblFilterChoices" width="247px">
                                                            <tbody>
                                                            </tbody>
                                                        </table>
                                                    </div>
                                                </div>
                                                <br />
                                                <div style="text-align: center; padding-bottom: 10px;">
                                                    <input type="button" class="myButton" value="Remove Selected Filter" id="btnRemoveFilter" />
                                                </div>
                                            </fieldset>
                                        </div>
                                    </div>
                                    <div id="tab3" class="tab" style="height: 300px;">
                                        <div style="float: left;">
                                            <fieldset style="border-color: #CDCDCD; width: 460px;">
                                                <legend style="color: Black; font-size: 11px;"><b>SHOW HEADER INFORMATION:</b></legend>
                                                <table style="width: 400px;">
                                                    <tr>
                                                        <td colspan="2" style="width: 150px; padding-left: 25px; padding-top: 8px;">
                                                            <asp:CheckBox runat="server" ID="chkMainHeader" Text="Main Header" CssClass="ListControl"
                                                                Checked="true" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 150px; padding-left: 25px; padding-top: 12px;">
                                                            <asp:CheckBox runat="server" ID="chkCompanyName" Text="Company Name" CssClass="ListControl" />
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtCompanyName" runat="server" Width="215px" CssClass="form-control"
                                                                Enabled="false"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 150px; padding-left: 25px; padding-top: 8px;">
                                                            <asp:CheckBox runat="server" ID="chkReportTitle" Text="Report Title" CssClass="ListControl" />
                                                        </td>
                                                        <td style="padding-top: 8px;">
                                                            <asp:TextBox ID="txtReportTitle" runat="server" Width="215px" CssClass="form-control"
                                                                Enabled="false"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 150px; padding-left: 25px; padding-top: 8px;">
                                                            <asp:CheckBox runat="server" ID="chkSubtitle" Text="Subtitle" CssClass="ListControl" />
                                                        </td>
                                                        <td style="padding-top: 8px;">
                                                            <asp:TextBox ID="txtSubtitle" runat="server" Width="215px" CssClass="form-control"
                                                                Enabled="false"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 150px; padding-left: 25px; padding-top: 8px;">
                                                            <asp:CheckBox runat="server" ID="chkDatePrepared" Text="Date Prepared" CssClass="ListControl"
                                                                Checked="true" />
                                                        </td>
                                                        <td style="padding-top: 8px;">
                                                            <asp:DropDownList ID="drpDatePrepared" runat="server" Width="215px" CssClass="form-control"
                                                                Enabled="false">
                                                                <asp:ListItem Value="12/31/01" Selected="True">12/31/01</asp:ListItem>
                                                                <asp:ListItem Value="Dec 31, 01">Dec 31, 01</asp:ListItem>
                                                                <asp:ListItem Value="December 31, 01">December 31, 01</asp:ListItem>
                                                                <asp:ListItem Value="Dec 31, 2001">Dec 31, 2001</asp:ListItem>
                                                                <asp:ListItem Value="December 31, 2001">December 31, 2001</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2" style="width: 150px; padding-left: 25px; padding-top: 12px;">
                                                            <asp:CheckBox runat="server" ID="chkTimePrepared" Text="Time Prepared" CssClass="ListControl"
                                                                Checked="true" />
                                                        </td>
                                                    </tr>
                                                    <%-- <tr>
                                        <td colspan="2" style="width: 150px; padding-left: 25px; padding-top: 12px;">
                                            <asp:CheckBox runat="server" ID="chkReportBasis" Text="Report Basis" CssClass="ListControl" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" style="width: 150px; padding-left: 25px; padding-top: 12px;">
                                            <asp:CheckBox runat="server" ID="chkPrintHeaderOnFPage" Text="Print header on pages after first page"
                                                CssClass="ListControl" />
                                        </td>
                                    </tr>--%>
                                                </table>
                                            </fieldset>
                                            <br />
                                            <fieldset style="border-color: #CDCDCD; width: 460px;">
                                                <legend style="color: Black; font-size: 11px;"><b>SHOW FOOTER INFORMATION:</b></legend>
                                                <table style="width: 400px;">
                                                    <tr>
                                                        <td style="width: 150px; padding-left: 25px;">
                                                            <asp:CheckBox runat="server" ID="chkPageNumber" Text="Page Number" CssClass="ListControl" />
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="drpPageNumber" runat="server" Width="215px" CssClass="form-control"
                                                                Enabled="false">
                                                                <asp:ListItem Value="Page 1">Page 1</asp:ListItem>
                                                                <asp:ListItem Value="pg 1">pg 1</asp:ListItem>
                                                                <asp:ListItem Value="p 1">p 1</asp:ListItem>
                                                                <asp:ListItem Value="<1>"><1></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 170px; padding-left: 25px; padding-top: 2px">
                                                            <asp:CheckBox runat="server" ID="chkExtraFootLine" Text="Extra Footer Line" CssClass="ListControl" />
                                                        </td>
                                                        <td style="padding-top: 2px">
                                                            <asp:TextBox ID="txtExtraFooterLine" runat="server" Width="215px" CssClass="form-control"
                                                                Enabled="false"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <%-- <tr>
                                        <td colspan="2" style="width: 150px; padding-left: 25px; padding-top: 12px;">
                                            <asp:CheckBox runat="server" ID="chkPrtFootonFPage" Text="Print footer on pages page"
                                                CssClass="ListControl" />
                                        </td>
                                    </tr>--%>
                                                </table>
                                            </fieldset>
                                        </div>
                                        <div style="float: right">
                                            <fieldset style="border-color: #CDCDCD; width: 230px; height: 277px;">
                                                <legend style="color: Black; font-size: 11px;"><b>PAGE LAYOUT:</b></legend>
                                                <table style="width: 180px;">
                                                    <tr>
                                                        <td>&nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <span>Alignment</span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="padding-top: 8px; padding-left: 15px;">
                                                            <asp:DropDownList ID="drpAlignment" runat="server" Width="170px" CssClass="form-control">
                                                                <asp:ListItem Value="Standard" Selected="True">Standard</asp:ListItem>
                                                                <asp:ListItem Value="Left">Left</asp:ListItem>
                                                                <asp:ListItem Value="Right">Right</asp:ListItem>
                                                                <asp:ListItem Value="Centered">Centered</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </fieldset>
                                        </div>
                                    </div>
                                    <div id="tab4" class="tab" style="height: 150px;">
                                        <div style="float: left;">
                                            <fieldset style="border-color: #CDCDCD; width: 460px; height: 100px;">
                                                <legend style="color: Black; font-size: 11px;"><b>PDF Page Size:</b></legend>
                                                <table style="width: 400px; padding-top: 15px; padding-left: 15px;">
                                                    <tr>
                                                        <td>Select Size:
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="drpPDFPageSize" runat="server" Width="215px" CssClass="form-control">
                                                                <asp:ListItem Value="11X17">11X17</asp:ListItem>
                                                                <asp:ListItem Value="A0">A0</asp:ListItem>
                                                                <asp:ListItem Value="A1">A1</asp:ListItem>
                                                                <asp:ListItem Value="A10">A10</asp:ListItem>
                                                                <asp:ListItem Value="A2">A2</asp:ListItem>
                                                                <asp:ListItem Value="A3">A3</asp:ListItem>
                                                                <asp:ListItem Value="A4">A4</asp:ListItem>
                                                                <asp:ListItem Value="A4_LANDSCAPE">A4_LANDSCAPE</asp:ListItem>
                                                                <asp:ListItem Value="A5">A5</asp:ListItem>
                                                                <asp:ListItem Value="A6">A6</asp:ListItem>
                                                                <asp:ListItem Value="A7">A7</asp:ListItem>
                                                                <asp:ListItem Value="A8">A8</asp:ListItem>
                                                                <asp:ListItem Value="A9">A9</asp:ListItem>
                                                                <asp:ListItem Value="ARCH_A">ARCH_A</asp:ListItem>
                                                                <asp:ListItem Value="ARCH_B">ARCH_B</asp:ListItem>
                                                                <asp:ListItem Value="ARCH_C">ARCH_C</asp:ListItem>
                                                                <asp:ListItem Value="ARCH_D">ARCH_D</asp:ListItem>
                                                                <asp:ListItem Value="ARCH_E">ARCH_E</asp:ListItem>
                                                                <asp:ListItem Value="B0">B0</asp:ListItem>
                                                                <asp:ListItem Value="B1">B1</asp:ListItem>
                                                                <asp:ListItem Value="B10">B10</asp:ListItem>
                                                                <asp:ListItem Value="B2">B2</asp:ListItem>
                                                                <asp:ListItem Value="B3">B3</asp:ListItem>
                                                                <asp:ListItem Value="B4">B4</asp:ListItem>
                                                                <asp:ListItem Value="B5">B5</asp:ListItem>
                                                                <asp:ListItem Value="B6">B6</asp:ListItem>
                                                                <asp:ListItem Value="B7">B7</asp:ListItem>
                                                                <asp:ListItem Value="B8">B8</asp:ListItem>
                                                                <asp:ListItem Value="B9">B9</asp:ListItem>
                                                                <asp:ListItem Value="CROWN_OCTAVO">CROWN_OCTAVO</asp:ListItem>
                                                                <asp:ListItem Value="CROWN_QUARTO">CROWN_QUARTO</asp:ListItem>
                                                                <asp:ListItem Value="DEMY_OCTAVO">DEMY_OCTAVO</asp:ListItem>
                                                                <asp:ListItem Value="DEMY_QUARTO">DEMY_QUARTO</asp:ListItem>
                                                                <asp:ListItem Value="EXECUTIVE">EXECUTIVE</asp:ListItem>
                                                                <asp:ListItem Value="FLSA">FLSA</asp:ListItem>
                                                                <asp:ListItem Value="FLSE">FLSE</asp:ListItem>
                                                                <asp:ListItem Value="HALFLETTER">HALFLETTER</asp:ListItem>
                                                                <asp:ListItem Value="ID_1">ID_1</asp:ListItem>
                                                                <asp:ListItem Value="ID_2">ID_2</asp:ListItem>
                                                                <asp:ListItem Value="ID_3">ID_3</asp:ListItem>
                                                                <asp:ListItem Value="LARGE_CROWN_OCTAVO">LARGE_CROWN_OCTAVO</asp:ListItem>
                                                                <asp:ListItem Value="LARGE_CROWN_QUARTO">LARGE_CROWN_QUARTO</asp:ListItem>
                                                                <asp:ListItem Value="LEDGER">LEDGER</asp:ListItem>
                                                                <asp:ListItem Value="LEGAL">LEGAL</asp:ListItem>
                                                                <asp:ListItem Value="LEGAL_LANDSCAPE">LEGAL_LANDSCAPE</asp:ListItem>
                                                                <asp:ListItem Value="LETTER" Selected="True">LETTER</asp:ListItem>
                                                                <asp:ListItem Value="LETTER_LANDSCAPE">LETTER_LANDSCAPE</asp:ListItem>
                                                                <asp:ListItem Value="NOTE">NOTE</asp:ListItem>
                                                                <asp:ListItem Value="PENGUIN_LARGE_PAPERBACK">PENGUIN_LARGE_PAPERBACK</asp:ListItem>
                                                                <asp:ListItem Value="PENGUIN_SMALL_PAPERBACK">PENGUIN_SMALL_PAPERBACK</asp:ListItem>
                                                                <asp:ListItem Value="POSTCARD">POSTCARD</asp:ListItem>
                                                                <asp:ListItem Value="ROYAL_OCTAVO">ROYAL_OCTAVO</asp:ListItem>
                                                                <asp:ListItem Value="ROYAL_QUARTO">ROYAL_QUARTO</asp:ListItem>
                                                                <asp:ListItem Value="SMALL_PAPERBACK">SMALL_PAPERBACK</asp:ListItem>
                                                                <asp:ListItem Value="TABLOID">TABLOID</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </fieldset>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <footer>
                            <%--<asp:Button runat="server" ID="btnApply" CssClass="BlueButton" Text ="Apply" Width="100px"/>  --%>
                            <%--<asp:Button ID="btnPreview" runat="server" Text="Preview" CssClass="btn btn-success" OnClick="btnPreview_Click" />--%>
                            <a href="#" class="btn btn-success" style="width: 75px" id="btnPreview">Preview</a>
                            <a href="#" class="btn btn-primary" id="btnApply" data-modal-id="dvSaveReport">Apply</a>
                            <a href="#" class="js-modal-close btn btn-default" id="btnCancel">Cancel</a>
                        </footer>
                    </div>
                    <div id="dvSaveReport" class="modal-box" runat="Server">
                        <header>
                            <a href="#" class="js-modal-close close">×</a>
                            <h3>Save Report</h3>
                        </header>
                        <div class="modal-body">
                            <div class="alert alert-danger" runat="server" id="divInfo" style="display: none">
                                You don't have permission to customize this report. Please choose another title.
                            </div>
                            <table style="padding-left: 250px; height: 50px;">
                                <tr>
                                    <td style="text-align: right">
                                        <b style="font-size: 14px;">Report Name:</b>
                                    </td>
                                    <td style="padding-left: 10px;">
                                        <asp:TextBox ID="txtReportName" runat="server" Width="200px" Height="20px" Font-Size="14px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <b style="font-size: 14px;">Is Global:</b>
                                    </td>
                                    <td style="padding-left: 10px;">
                                        <div>
                                            <input type="checkbox" id="chkIsGlobal" runat="server" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <footer>
                            <asp:Button
                                ID="btnSaveReport" runat="server" CssClass="BlueButton" Text="Save Report" OnClientClick="return EmptyReportName();"
                                OnClick="btnSaveReport_Click" />
                            <a href="#" class="js-modal-close myButton" id="btnCancel2">Cancel</a>
                        </footer>
                    </div>
                    <div id="dvEmailPanel" class="modal-box">
                        <header>
                            <a href="#" class="js-modal-close close" onclick="EmailCancel();">×</a>
                            <h3><span id="Span1">Send Report:</span></h3>
                        </header>
                        <div class="modal-body">
                            <table style="padding-left: 150px; height: 50px;">
                                <tr>
                                    <td style="text-align: right">
                                        <b style="font-size: 14px;">From:</b>
                                    </td>
                                    <td style="padding-left: 10px;">
                                        <asp:TextBox ID="txtFrom" runat="server" Width="400px" Height="20px" Font-Size="14px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <b style="font-size: 14px;">To:</b>
                                    </td>
                                    <td style="padding-left: 10px;">
                                        <asp:TextBox ID="txtTo" runat="server" Width="400px" Height="20px" Font-Size="14px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <b style="font-size: 14px;">Cc:</b>
                                    </td>
                                    <td style="padding-left: 10px;">
                                        <asp:TextBox ID="txtCc" runat="server" Width="400px" Height="20px" Font-Size="14px"></asp:TextBox>
                                    </td>
                                </tr>
                                <%-- <tr> <td> &nbsp; </td> </tr> <tr> <td style="text-align: right">
    <b style="font-size: 14px;">Bcc:</b> </td> <td style="padding-left: 10px;"> <asp:TextBox
    ID="txtBcc" runat="server" Width="400px" Height="20px" Font-Size="14px"></asp:TextBox>
    </td> </tr>--%>
                                <tr>
                                    <td>&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <b style="font-size: 14px;">Subject:</b>
                                    </td>
                                    <td style="padding-left: 10px;">
                                        <asp:TextBox ID="txtSubject" runat="server" Width="400px" Height="20px" Font-Size="14px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>&nbsp;
                                    </td>
                                </tr>
                                <%-- <tr> <td style="text-align: right">
    <b style="font-size: 14px;">Attached File:</b> </td> <td style="padding-left: 10px;">
    <div> <asp:Label ID="lblAttachedFile" runat="server"></asp:Label> </div> </td> </tr>--%>
                                <tr>
                                    <td>&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <b style="font-size: 14px;"></b>
                                    </td>
                                    <td style="padding-left: 10px;">
                                        <div>
                                            <asp:TextBox TextMode="MultiLine" ID="txtBody" runat="server" Width="400px" Height="130px"
                                                Text="This is report email
    sent from Mobile Office Manager. Please find the Report attached."></asp:TextBox>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <footer>
                            <asp:Button ID="btnSendReport" runat="server"
                                CssClass="BlueButton" Text="Send Report" OnClientClick="btnSendReport();" OnClick="btnSendReport_Click" />
                            <a href="#" class="js-modal-close myButton" id="btnEmailCancel" onclick="EmailCancel();">Cancel</a>
                        </footer>
                    </div>
                    <asp:HiddenField runat="server" ID="hdnColumnList" />
                    <asp:HiddenField runat="server" ID="hdnColumnWidth" />
                    <asp:HiddenField runat="server" ID="hdnCustomizeReportName" />
                    <asp:HiddenField runat="server" ID="hdnIsStock" />
                    <asp:HiddenField runat="server" ID="hdnReportAction" />
                    <asp:HiddenField runat="server" ID="hdnDrpSortBy" />
                    <asp:HiddenField runat="server" ID="hdnLstColumns" />
                    <asp:HiddenField runat="server" ID="hdnFilterColumns" />
                    <asp:HiddenField runat="server" ID="hdnFilterValues" />
                    <asp:HiddenField runat="server" ID="hdnMainHeader" />
                    <asp:HiddenField runat="server" ID="hdnDivToExport" />
                    <asp:HiddenField runat="server" ID="hdnSendReportType" />
                </div>
            </div>
            <!-- edit-tab end -->
            <div class="clearfix"></div>
        </div>
        <!-- END DASHBOARD STATS -->
        <div class="clearfix"></div>
    </div>

    <div class="clearfix"></div>
   <%-- <div class="pnlLoading" style="z-index: 900000; display: none;" id="loading"
        align="center">
        <img id="Image1" alt="Loading..." src="images/loader_round.gif" style="position: absolute; left: 50%; top: 50%; margin-left: -32px; margin-top: -32px; display: block;" />
    </div>--%>
    <div id="myModal" class="modal">
        <div class="modal-dialog" style="width: 1200px !important">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <h4 class="modal-title">Report Preview</h4>
                </div>
                <div class="clearfix"></div>
                <div class="modal-body">
                    <div id="dvPreviewHeader" style="float: left;">
                        <table id="tblPreview">
                            <tr>
                                <td>
                                    <asp:Image ID="imgPreview" runat="server" />
                                </td>
                                <td style="padding-left: 35px;" colspan="4">
                                    <table width="500px" style="height: 100px;">
                                        <tr>
                                            <td>
                                                <asp:Label runat="server" ID="lblPreviewCompanyName" Font-Bold="true" Font-Size="17px" ForeColor="Black"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label runat="server" ID="lblPreviewCompAddress" ForeColor="Black" Font-Size="14px"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label runat="server" ID="lblPreviewCompEmail" ForeColor="Black" Font-Size="12px"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div style="float: right; padding-right: 50px;">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lblPreviewTime" runat="server" Font-Bold="true" Font-Size="11px" ForeColor="Black"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblPreviewDate" runat="server" Font-Bold="true" Font-Size="11px" ForeColor="Black"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div class="table-scrollable" style="width: 1175px; height: 250px; overflow: auto; border: none; padding: 20px">
                        <%-- <table id="tblPreviewReport" class="tblPreviewReport">
                            <tbody>
                            </tbody>
                        </table>--%>
                        <div id="tblPreviewReport" class="tblPreviewReport">
                        </div>
                    </div>


                </div>
                <div class="clearfix"></div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal" id="btnCancel3">Close</button>
                    <button type="button" id="btnPreviewApply" data-dismiss="modal" class="btn btn-primary">Apply</button>
                </div>
            </div>
        </div>
    </div>
 <!-- END Manish -->

            </div>
            <!-- END CONTENT -->
        </div>
        <!-- END CONTAINER -->
        <!-- BEGIN FOOTER -->
        <div class="page-footer">
            <div class="page-footer-inner">
               &copy; <%= System.DateTime.Now.Year.ToString() %> Mobile Office Manager. All Rights Reserved.
            </div>
            <div class="scroll-to-top">
                <i class="icon-arrow-up"></i>
            </div>
        </div>
    </form>
    <!-- END FOOTER -->
    <!-- BEGIN JAVASCRIPTS(Load javascripts at bottom, this will reduce page load time) -->
    <!-- BEGIN CORE PLUGINS -->
    <!--[if lt IE 9]>
<script src="../../assets/global/plugins/respond.min.js"></script>
<script src="../../assets/global/plugins/excanvas.min.js"></script> 
<![endif]-->


    <script src="Appearance/js/jquery-migrate.min.js" type="text/javascript"></script>
    <!-- IMPORTANT! Load jquery-ui.min.js before bootstrap.min.js to fix bootstrap tooltip conflict with jquery ui tooltip -->
    <script src="Appearance/js/jquery-ui.min.js" type="text/javascript"></script>
    <script src="Appearance/js/bootstrap.min.js" type="text/javascript"></script>
    <script src="Appearance/js/bootstrap-hover-dropdown.min.js" type="text/javascript"></script>
    <script src="Appearance/js/jquery.slimscroll.min.js" type="text/javascript"></script>
    <script src="Appearance/js/jquery.blockui.min.js" type="text/javascript"></script>
    <script src="Appearance/js/jquery.cokie.min.js" type="text/javascript"></script>
    <script src="Appearance/js/jquery.uniform.min.js" type="text/javascript"></script>
    <script src="Appearance/js/bootstrap-switch.min.js" type="text/javascript"></script>
    <!-- END CORE PLUGINS -->
    <!-- BEGIN PAGE LEVEL PLUGINS -->
    <%--<script src="Appearance/js/jquery.vmap.js" type="text/javascript"></script>
    <script src="Appearance/js/jquery.vmap.russia.js" type="text/javascript"></script>
    <script src="Appearance/js/jquery.vmap.world.js" type="text/javascript"></script>
    <script src="Appearance/js/jquery.vmap.europe.js" type="text/javascript"></script>
    <script src="Appearance/js/jquery.vmap.germany.js" type="text/javascript"></script>
    <script src="Appearance/js/jquery.vmap.usa.js" type="text/javascript"></script>
    <script src="Appearance/js/jquery.vmap.sampledata.js" type="text/javascript"></script>--%>
    <script src="Appearance/js/jquery.flot.min.js" type="text/javascript"></script>
    <script src="Appearance/js/jquery.flot.resize.min.js" type="text/javascript"></script>
    <script src="Appearance/js/jquery.flot.categories.min.js" type="text/javascript"></script>
    <script src="Appearance/js/jquery.pulsate.min.js" type="text/javascript"></script>
    <script src="Appearance/js/moment.min.js" type="text/javascript"></script>
    <script src="Appearance/js/daterangepicker.js" type="text/javascript"></script>
    <!-- IMPORTANT! fullcalendar depends on jquery-ui.min.js for drag & drop support -->
    <script src="Appearance/js/fullcalendar.min.js" type="text/javascript"></script>
    <script src="Appearance/js/jquery.easypiechart.min.js" type="text/javascript"></script>
    <script src="Appearance/js/jquery.sparkline.min.js" type="text/javascript"></script>
    <!-- END PAGE LEVEL PLUGINS -->
    <!-- BEGIN PAGE LEVEL SCRIPTS -->
    <script src="Appearance/js/metronic.js" type="text/javascript"></script>
    <script src="Appearance/js/layout.js" type="text/javascript"></script>
    <script src="Appearance/js/quick-sidebar.js" type="text/javascript"></script>
    <script src="Appearance/js/demo.js" type="text/javascript"></script>
    <script src="Appearance/js/index.js" type="text/javascript"></script>
    <script src="Appearance/js/tasks.js" type="text/javascript"></script>



    <!-- END PAGE LEVEL SCRIPTS -->
    <script>
        jQuery(document).ready(function () {
            //Metronic.init(); // init metronic core componets
            Layout.init(); // init layout
            QuickSidebar.init(); // init quick sidebar
            Demo.init(); // init demo features
            Index.init();
            Index.initDashboardDaterange();
            Index.initJQVMAP(); // init index page's custom scripts
            Index.initCalendar(); // init index page's custom scripts
            Index.initCharts(); // init index page's custom scripts
            Index.initChat();
            Index.initMiniCharts();
            Tasks.initDashboardWidget();
        });
    </script>
    <script>
        $(document).ready(function () {
            if ($(window).width() > 780) {
                document.body.classList.add('page-sidebar-closed');
                <%--document.getElementById('<%= toggleMenu.ClientID %>').classList.add('page-sidebar-menu-closed');--%>
            }
        });
    </script>
    <!-- END JAVASCRIPTS -->
    <script>
        (function (i, s, o, g, r, a, m) {
            i['GoogleAnalyticsObject'] = r; i[r] = i[r] || function () {
                (i[r].q = i[r].q || []).push(arguments)
            }, i[r].l = 1 * new Date(); a = s.createElement(o),
                m = s.getElementsByTagName(o)[0]; a.async = 1; a.src = g; m.parentNode.insertBefore(a, m)
        })(window, document, 'script', '//www.google-analytics.com/analytics.js', 'ga');
        ga('create', 'UA-37564768-1', 'keenthemes.com');
        ga('send', 'pageview');
    </script>
    <script type="text/javascript" src="js/Signature/jquery.signaturepad.js"></script>

    <script type="text/javascript">
        ///////////// Signature box handling  ////////////////////
        $(document).ready(function () {
            $('.sigPad').signaturePad();
        });
    </script>
</body>
</html>

  
<%--</asp:Content>--%>
