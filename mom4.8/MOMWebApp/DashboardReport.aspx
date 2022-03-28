<%@ Page Language="C#" MasterPageFile="~/Mom.master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="DashboardReport.aspx.cs" Inherits="DashboardReport" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="Stimulsoft.Report.WebDesign" Namespace="Stimulsoft.Report.Web" TagPrefix="cc1" %>
<%@ Register Assembly="Stimulsoft.Report.Web" Namespace="Stimulsoft.Report.Web" TagPrefix="cc1" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <!--Grid Control-->
    <link href="Design/css/grid.css" rel="stylesheet" />

    <style>
        .rgDataDiv, .stiJsViewerPageShadow {
            height: auto !important;
        }

        .RadGrid_Material .rgHeader {
            font-weight: bold !important;
        }

        .rgFooterWrapper .rgFooterDiv {
            padding-right: 0 !important;
        }

        .dropdown-content {
            width: auto !important;
        }

        .comparative-grid-column table.ajax__validatorcallout_popup_table {
            width: 250px !important;
        }

        .type-dropdown label {
            padding-right: 10px;
            width: 185px;
            display: inline-block;
        }

        .type-dropdown .RadButton {
            margin-top: 20px;
        }

        /** Columns */
        .rcbHeader ul,
        .rcbFooter ul,
        .rcbItem ul,
        .rcbHovered ul,
        .rcbDisabled ul {
            margin: 0;
            padding: 0;
            width: 100%;
            display: inline-block;
            list-style-type: none;
        }

        .exampleRadComboBox.RadComboBoxDropDown .rcbHeader {
            padding: 5px 27px 4px 7px;
        }

        .col1,
        .col2 {
            margin: 0;
            padding: 0 5px 0 0;
            line-height: 14px;
            float: left;
        }

        .rcbHeader .col1, .rcbHeader .col2 {
            font-weight: bold;
        }

        .col1 {
            width: 30%;
        }

        .col2 {
            width: 70%;
        }
    </style>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="divbutton-container">
        <div id="divButtons">
            <div id="breadcrumbs-wrapper">
                <header>
                    <div class="col s12 m12 l12">
                        <div class="row">
                            <div class="page-title">
                                <i class="mdi-action-swap-vert-circle"></i>&nbsp;
                                <asp:Label runat="server" ID="pageTitle" Text="Dashboard Report"></asp:Label>
                            </div>

                            <div class="btnclosewrap">
                                <asp:LinkButton ID="lnkClose" runat="server" OnClick="lnkClose_Click"><i class="mdi-content-clear"></i></asp:LinkButton>
                            </div>
                        </div>
                    </div>
                </header>
            </div>
        </div>
    </div>

    <div class="container">
        <div class="row">
            <div class="srchpane">
                <div class="srchpaneinnernegate">
                    <div class="srchpaneinner">
                        <div class="srchtitle srchtitlecustomwidth">
                            As of date
                        </div>

                        <div class="srchinputwrap">
                            <asp:TextBox ID="txtSearchDate" runat="server" CssClass="form-control" Height="30px"
                                MaxLength="50" Width="130px" autocomplete="off"></asp:TextBox>
                            <asp:CalendarExtender ID="txtSearchDate_CalendarExtender" runat="server" Enabled="True"
                                TargetControlID="txtSearchDate">
                            </asp:CalendarExtender>
                            <asp:RequiredFieldValidator ID="rfvEndDt"
                                runat="server" ControlToValidate="txtSearchDate" Display="None" ErrorMessage="End date is Required" ValidationGroup="search"
                                SetFocusOnError="True"></asp:RequiredFieldValidator>
                            <asp:ValidatorCalloutExtender ID="vceEndDt" runat="server" Enabled="True"
                                PopupPosition="Right" TargetControlID="rfvEndDt" />
                            <asp:RegularExpressionValidator ID="rfvEndDt1" ControlToValidate="txtSearchDate" ValidationGroup="search"
                                ValidationExpression="^(0?[1-9]|1[012])[\/\-](0?[1-9]|[12][0-9]|3[01])[\/\-]\d{4}$"
                                runat="server" ErrorMessage="Invalid Date format. Valid Date Format 01/12/2001" Display="None">
                            </asp:RegularExpressionValidator>
                            <asp:ValidatorCalloutExtender ID="vceEndDt1" runat="server" Enabled="True" PopupPosition="Right"
                                TargetControlID="rfvEndDt1" />
                        </div>

                        <div class="srchinputwrap btnlinksicon srchclr">
                            <asp:LinkButton ID="btnSearch" runat="server" CausesValidation="false"
                                OnClick="lnkLoadReport_Click"><i class="mdi-notification-sync"></i></asp:LinkButton>
                        </div>
                    </div>
                </div>
            </div>

            <div class="grid_container">
                <div class="form-section-row pmd-card" >
                    <cc1:StiWebViewer ID="StiWebViewerReport" Height="800px" RequestTimeout="20000" runat="server" ViewMode="Continuous" ScrollbarsMode="true" CacheMode="None"
                        OnGetReport="StiWebViewerReport_GetReport" OnGetReportData="StiWebViewerReport_GetReportData" Visible="false" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
