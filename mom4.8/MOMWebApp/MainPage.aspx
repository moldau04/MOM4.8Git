<%@ Page Title="" Language="C#" MasterPageFile="~/MainShow.master" AutoEventWireup="true" Inherits="MainPage" Codebehind="MainPage.aspx.cs" %>
<%@ Register Src="~/ChatCtrl.ascx" TagPrefix="uc1" TagName="ChatCtrl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
        <script type="text/javascript">
            var srp = '<%=Page.ResolveUrl("~") %>';
    </script>
    <%--<link href="<%=Page.ResolveUrl("~") %>Styles/bootstrap.css" rel="stylesheet" />--%>
     <link href="<%=Page.ResolveUrl("~") %>Styles/jquery.ui.chatbox.css" rel="stylesheet" />

    <link href="<%=Page.ResolveUrl("~") %>Styles/styleChat.css" rel="stylesheet" />        

    <link href="<%=Page.ResolveUrl("~") %>fonts/font-awesome/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="<%=Page.ResolveUrl("~") %>Scripts/jquery/jquery-ui/jquery-ui.css" rel="stylesheet" />
    <script src="<%=Page.ResolveUrl("~") %>Scripts/jquery.js"></script>    
    <script src="<%=Page.ResolveUrl("~") %>Scripts/jquery/jquery-ui/jquery-ui.js" type="text/javascript"></script>
    <script src="<%=Page.ResolveUrl("~") %>Scripts/bootstrap.min.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <uc1:ChatCtrl runat="server" ID="ChatCtrl" />

</asp:Content>

