<%@ Page Title="" Language="C#" MasterPageFile="~/CustomerReportMaster.master" AutoEventWireup="true" Inherits="ReportDesignerMonthlyTicketReport" Codebehind="ReportDesignerMonthlyTicketReport.aspx.cs" %>

<%@ Register Assembly="Stimulsoft.Report.WebDesign" Namespace="Stimulsoft.Report.Web" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
   
            <cc1:StiWebDesigner ID="StiWebDesignerMonthlyTicket" runat="server"  ShowFileMenuExit="true" OnPreviewReport="StiWebDesignerMonthlyTicket_PreviewReport"  OnSaveReport="StiWebDesignerMonthlyTicket_SaveReport"
                  OnExit="StiWebDesignerMonthlyTicket_Exit" ></cc1:StiWebDesigner>
      
</asp:Content>

