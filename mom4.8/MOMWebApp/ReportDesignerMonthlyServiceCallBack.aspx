<%@ Page Title="" Language="C#" MasterPageFile="~/CustomerReportMaster.master" AutoEventWireup="true" Inherits="ReportDesignerMonthlyServiceCallBack" Codebehind="ReportDesignerMonthlyServiceCallBack.aspx.cs" %>

<%@ Register Assembly="Stimulsoft.Report.WebDesign" Namespace="Stimulsoft.Report.Web" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
   
            <cc1:StiWebDesigner ID="StiWebDesignerMonthlyServiceCallBack" runat="server"  ShowFileMenuExit="true" OnPreviewReport="StiWebDesignerMonthlyServiceCallBack_PreviewReport" OnSaveReport="StiWebDesignerMonthlyServiceCallBack_SaveReport"
                  OnExit="StiWebDesignerMonthlyServiceCallBack_Exit" ></cc1:StiWebDesigner>
      
</asp:Content>

