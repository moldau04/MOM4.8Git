<%@ Page Title="" Language="C#" MasterPageFile="~/Mom.master" AutoEventWireup="true" Inherits="ReceivePOReport" Codebehind="ReceivePOReport.aspx.cs" %>



<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="Stimulsoft.Report.WebDesign" Namespace="Stimulsoft.Report.Web" TagPrefix="cc1" %>

<%@ Register Assembly="Stimulsoft.Report.Web" Namespace="Stimulsoft.Report.Web" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
     <!--Grid Control-->
    <link href="Design/css/grid.css" rel="stylesheet" />
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
     <div class="divbutton-container">
        <div id="divButtons" class="">
            <div id="breadcrumbs-wrapper">

                <header>
                    <div class="container row-color-grey">
                        <div class="row">
                            <div class="col s12 m12 l12">
                                <div class="row">
                                    <div class="page-title"><i class="mdi-communication-contacts"></i>&nbsp;<asp:Label CssClass="title_text" ID="lblHeader" runat="server">Receive Item Report</asp:Label></div>

                                    <div class="btnlinks">
                                        <%--<asp:LinkButton ID="lnkEmail" ToolTip="Email" runat="server" CausesValidation="false" OnClientClick="reloadP();return false"
                                                OnClick="lnkEmail_Click">Mail Report</asp:LinkButton>--%>
                                    </div>

                                    <div class="rght-content">
                                        <div class="editlabel">
                                            <asp:Label ID="lblInv" Visible="false" runat="server"></asp:Label>
                                        </div>

                                        <div class="btnclosewrap">
                                            <asp:LinkButton ID="lnkClose" ToolTip="Close" runat="server" CausesValidation="false"
                                                OnClick="lnkClose_Click"><i class="mdi-content-clear"></i></asp:LinkButton>
                                        </div>

                                    </div>

                                </div>
                            </div>

                        </div>
                    </div>
                </header>
            </div>
        </div>
    </div>
    <cc1:StiWebViewer ID="StiWebViewerReceivePO" Height="800px" runat="server" ScrollbarsMode="true" CacheMode="None" RequestTimeout="900000" Zoom="100" BackgroundColor="White"
        OnGetReport="StiWebViewerReceivePO_GetReport" OnGetReportData="StiWebViewerReceivePO_GetReportData" ViewMode="Continuous" />

    <div class="clearfix"></div>

    
    <!-- END DASHBOARD STATS -->
    <div class="clearfix"></div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="javascriptHolder" Runat="Server">
</asp:Content>

