<%@ Page Language="C#" AutoEventWireup="true" Inherits="Ticketpdf" Codebehind="Ticketpdf.aspx.cs" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
     <rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="85%" Height="727px"
                                BorderColor="Gray" BorderStyle="None" BorderWidth="1px" ShowPageNavigationControls="False"
                                AsyncRendering="false" ShowZoomControl="False">
                            </rsweb:ReportViewer>
    </div>
    </form>
</body>
</html>
