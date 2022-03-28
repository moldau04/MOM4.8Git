<%@ Page Title="" Language="C#" MasterPageFile="~/Popup.master" AutoEventWireup="true" Inherits="PrintTicketEIR" Codebehind="PrintTicketEIR.aspx.cs" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
 <script type="text/javascript">

        function cancel() {
            window.parent.document.getElementById('ctl00_ContentPlaceHolder1_hideModalPopupViaServer').click();
        }
        </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
 <div>
        <div class="title_bar_popup" runat="server" id="lnkCancelContact">
           
            <a runat="server" id="A1" href="#" onclick="cancel();" style="float: right; margin-right: 20px;
                margin-left: 10px; height: 16px; color: #fff;" tabindex="24">Close</a>
                 <%--<asp:LinkButton ID="LinkButton1" runat="server"  style="float: right; margin-right: 20px; color: #fff;
            margin-left: 10px; height: 16px;" onclick="LinkButton1_Click">Mail Report</asp:LinkButton>--%>
        </div>
        <div style="margin-left:5px;" align="center">
        <rsweb:ReportViewer ID="ReportViewer1" runat="server" Height="783px" 
            Width="1115px"
            BorderColor="Gray" BorderStyle="None" BorderWidth="1px" ShowPageNavigationControls="False" AsyncRendering="false"
            ShowZoomControl="False">
        </rsweb:ReportViewer>
        </div>
    </div>
</asp:Content>

