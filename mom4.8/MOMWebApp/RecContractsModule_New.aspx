
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RecContractsModule_New.aspx.cs" MasterPageFile="~/Mom.master" Inherits="RecContractsModule_New" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script>
        function resizeIframe(obj) {
            obj.style.height = obj.contentWindow.document.body.scrollHeight + 'px';
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
        <div class="page-title"><i class="mdi-calender"></i>&nbsp; Recurring contract Preview </div>
    <div class="btnclosewrap"><a href="RecContracts.aspx"><i class="mdi-content-clear"></i></a></div>
    <iframe id="scheduleiframe" src="RecContractsModule?type=Recurring" width="100%" frameborder="0" scrolling="no" onload="resizeIframe(this)"></iframe>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="javascriptHolder" runat="Server">

</asp:Content>