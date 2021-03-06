<%@ Page Title="" Language="C#" MasterPageFile="~/CustomerReportMaster.master" AutoEventWireup="true" Inherits="MonthlyMaintenanceTicketReport" Codebehind="MonthlyMaintenanceTicketReport.aspx.cs" %>


<%@ Register assembly="Stimulsoft.Report.Web" namespace="Stimulsoft.Report.Web" tagprefix="cc1" %>

<%@ Register Assembly="Stimulsoft.Report.WebDesign" Namespace="Stimulsoft.Report.Web" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="page-content setMargin">
        <div class="page-cont-top">
            
        </div>
        <div class="clearfix"></div>
        <div class="row">
            <div class="col-lg-12 col-md-12">
                <div class="pc-title">
                    <ul class="lnklist-header">
                        <li>
                            <asp:Label CssClass="title_text" ID="lblHeader" runat="server">Completed Maintenance Report</asp:Label></li>
                        <li><a href="<%=ResolveUrl("~/TicketListView.aspx") %>" class="icon-closed" data-original-title="Close" title="Close" data-placement="bottom"></a></li>

                    </ul>
                </div>
            </div>
            <div class="col-lg-12 col-md-12">
             <cc1:StiWebViewer ID="StiWebViewerMonthlyTickets" runat="server"    OnDesignReport="StiWebViewerMonthlyTickets_DesignReport"
            OnGetReport="StiWebViewerMonthlyTickets_GetReport" OnGetReportData="StiWebViewerMonthlyTickets_GetReportData"  />


                
                </div>
            <!-- edit-tab start -->
            
            <!-- edit-tab end -->
            <div class="clearfix"></div>
        </div>
        <!-- END DASHBOARD STATS -->
        <div class="clearfix"></div>
    </div>
</asp:Content>


