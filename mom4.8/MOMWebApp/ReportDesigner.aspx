<%@ Page Title="" Language="C#" MasterPageFile="~/CustomerReportMaster.master" AutoEventWireup="true" Inherits="ReportDesigner" Codebehind="ReportDesigner.aspx.cs" %>



<%@ Register Assembly="Stimulsoft.Report.WebDesign" Namespace="Stimulsoft.Report.Web" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
   
            <cc1:StiWebDesigner ID="StiWebDesigner1" runat="server"  ShowFileMenuExit="true" OnPreviewReport="StiWebDesigner1_PreviewReport"  OnSaveReport="StiWebDesigner1_SaveReport" 
                  OnExit="StiWebDesigner1_Exit"  ></cc1:StiWebDesigner>
      
</asp:Content>

