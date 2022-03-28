<%@ Page Title="" Language="C#" MasterPageFile="~/MainMaster.master" AutoEventWireup="true" Inherits="testmail1" Codebehind="testmail1.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>    
    <asp:Button ID="btnDownload" runat="server" OnClick="btnDownload_Click" 
                            Text="Download mails from mail server to MOM db" />
    </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>

