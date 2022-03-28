<%@ Control Language="C#" AutoEventWireup="true" Inherits="uc_Datepicker" Codebehind="uc_Datepicker.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

    <asp:TextBox ID="txtDate" runat="server" CssClass="form-control"> </asp:TextBox>
    <asp:CalendarExtender ID="txtDate_CalendarExtender" runat="server" Enabled="True"
                TargetControlID="txtDate"> </asp:CalendarExtender>
