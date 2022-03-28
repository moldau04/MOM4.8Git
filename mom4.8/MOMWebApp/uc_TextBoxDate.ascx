<%@ Control Language="C#" AutoEventWireup="true" Inherits="uc_TextBoxDate" Codebehind="uc_TextBoxDate.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:TextBox ID="txt" runat="server" CssClass="form-control"></asp:TextBox>
<asp:MaskedEditExtender Enabled='true'
    TargetControlID="txt" ID="MaskedEditDate" runat="server" Mask="99/99/9999"
    MaskType="Date" UserDateFormat="MonthDayYear">
</asp:MaskedEditExtender>