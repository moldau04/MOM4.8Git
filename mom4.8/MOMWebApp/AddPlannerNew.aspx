<%@ Page Language="C#" AutoEventWireup="true" Inherits="AddPlannerNew" Codebehind="AddPlannerNew.aspx.cs" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="content-type" content="text/html; charset=UTF-8" />

    <!--Ext and ux styles -->
    <link href="https://www.bryntum.com/examples/extjs-6.7.0/build/classic/theme-triton/resources/theme-triton-all.css" rel="stylesheet" type="text/css"/>
    <%--<link href="BryntumAdvancedGantt/resources/lib/theme-triton-all_1.css" rel="stylesheet" />
    <link href="BryntumAdvancedGantt/resources/lib/theme-triton-all_2.css" rel="stylesheet" />--%>
    <!-- Gantt styles -->
    <link href="BryntumAdvancedGantt/resources/css/sch-gantt-triton-all.css?ver=6.1.5" rel="stylesheet" type="text/css" />
    <!-- Application styles -->
    <link href="BryntumAdvancedGantt/resources/css/examples.css?ver=6.1.5" rel="stylesheet" type="text/css" />
    <link href="BryntumAdvancedGantt/resources/advanced/resources/app.css?ver=6.1.5" rel="stylesheet" type="text/css" />

    <!--Ext lib -->
    <%--<script src="https://www.bryntum.com/examples/extjs-6.7.0/build/ext-all.js" crossorigin="anonymous" type="text/javascript"></script>--%>
    <script src="BryntumAdvancedGantt/lib/ext-all.js"></script>
    <script src="https://www.bryntum.com/examples/extjs-6.7.0/build/classic/theme-triton/theme-triton.js" crossorigin="anonymous" type="text/javascript"></script>
    <%--<script src="BryntumAdvancedGantt/lib/theme-triton.js"></script>--%>
    <!--Logging scripts-->
    <%--<script src="https://app.therootcause.io/rootcause-full-extjs.js" crossorigin="anonymous" type="text/javascript"></script>--%>
    <script src="BryntumAdvancedGantt/lib/rootcause-full-extjs.js"></script>
    <script src="BryntumAdvancedGantt/lib/examples_logging.js?ver=6.1.5" type="text/javascript"></script>

    <!-- Gantt components -->
    <script src="BryntumAdvancedGantt/js/gnt-all-debug.js?ver=6.1.5" type="text/javascript"></script>
    
    <!-- Shared examples code -->
    <script src="BryntumAdvancedGantt/js/lib/prism.js" type="text/javascript"></script>
    <script src="BryntumAdvancedGantt/js/lib/DetailsPanel.js?ver=6.1.5" type="text/javascript"></script>
    <script src="BryntumAdvancedGantt/js/shared/examples-shared.js?ver=6.1.5" type="text/javascript"></script>
    <script>
        Ext.define('Sch.examples.DetailsOverride', {
            override  : 'Sch.examples.DetailsPanel',
            appJSPath : 'js/app.js'
        });
    </script>

    <!-- Application -->
    <script src="BryntumAdvancedGantt/js/app.js?ver=6.1.5" type="text/javascript"></script>

    <title>MOM Demo</title>
</head>
<body>
  
</body>
</html>
