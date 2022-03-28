<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GanttChart.aspx.cs" Inherits="MOMWebApp.GanttChart" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="content-type" content="text/html; charset=UTF-8">
    <!--Ext and ux styles -->
    <%--<link href="https://www.bryntum.com/examples/extjs-6.7.0/build/classic/theme-triton/resources/theme-triton-all.css" rel="stylesheet" type="text/css"/>--%>
    <link href="resources/ext/theme-triton-all.css" rel="stylesheet" />
    <!-- Gantt styles -->
    <link href="resources/css/ext/sch-gantt-triton-all.css?ver=6.1.5" rel="stylesheet" type="text/css" />
    <!-- Application styles -->
    <link href="resources/css/examples.css?ver=6.1.5" rel="stylesheet" type="text/css" />
    <link href="resources/advanced/resources/app.css?ver=6.1.5" rel="stylesheet" type="text/css" />
    <!--Ext lib -->
    <%--<script src="https://www.bryntum.com/examples/extjs-6.7.0/build/ext-all.js" crossorigin="anonymous" type="text/javascript"></script>--%>
    <script src="js/ext/ext-all.js"></script>
    <%--<script src="https://www.bryntum.com/examples/extjs-6.7.0/build/classic/theme-triton/theme-triton.js" crossorigin="anonymous" type="text/javascript"></script>--%>
    <script src="js/ext/theme-triton.js"></script>
    <!--Logging scripts-->
    <%--<script src="https://app.therootcause.io/rootcause-full-extjs.js" crossorigin="anonymous" type="text/javascript"></script>--%>
    <script src="js/ext/rootcause-full-extjs.js"></script>
    <!-- Gantt components -->
    <script src="js/gnt-all-debug.js?ver=6.1.5" type="text/javascript"></script>
    <!-- Shared examples code -->
    <script src="js/lib/prism.js" type="text/javascript"></script>
    <script src="js/lib/DetailsPanel.js?ver=6.1.5" type="text/javascript"></script>
    <script src="js/shared/examples-shared.js?ver=6.1.5" type="text/javascript"></script>
    <script>
        Ext.define('Sch.examples.DetailsOverride', {
            override: 'Sch.examples.DetailsPanel',
            appJSPath: 'js/app.js'
        });
    </script>
    <!-- Application -->
    <script src="js/app.js?ver=6.1.5" type="text/javascript"></script>
    <%--<title>ASP.NET CRUD Demo</title>--%>
</head>
<body>
    
</body>
</html>
