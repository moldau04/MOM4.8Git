﻿<%@ Page Language="C#" AutoEventWireup="true" Codebehind="Designer.aspx.cs" Inherits="Designer" %>
<%@ Register Assembly="Stimulsoft.Report.WebDesign" Namespace="Stimulsoft.Report.Web" TagPrefix="cc1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <h2>đâsdasdasdas</h2>
    <form id="form1" runat="server">
        <div>
            <cc1:StiWebDesigner ID="StiWebDesigner1" runat="server" ShowFileMenuExit="true"
            OnPreviewReport="StiWebDesigner1_PreviewReport"
            OnSaveReport="StiWebDesigner1_SaveReport"
            OnExit="StiWebDesigner1_Exit" />
        </div>
    </form>
</body>
</html>
