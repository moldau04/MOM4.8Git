<%@ Control Language="C#" AutoEventWireup="true" Inherits="uc_TextBoxCurrency" Codebehind="uc_TextBoxCurrency.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:TextBox ID="txt" runat="server" CssClass="form-control"></asp:TextBox>

<asp:FilteredTextBoxExtender Enabled='true'
    ID="FilteredTextBoxExtender1" TargetControlID="txt" runat="server" ValidChars="0123456789.-+">
</asp:FilteredTextBoxExtender>