<%@ Page Language="C#" Title="Equipment Shut Down Report || MOM" MasterPageFile="~/Mom.master" AutoEventWireup="true" Inherits="EquipmentShutdownReport" Codebehind="EquipmentShutdownReport.aspx.cs" %>

<%@ Register Assembly="Stimulsoft.Report.WebDesign" Namespace="Stimulsoft.Report.Web" TagPrefix="cc1" %>
<%@ Register Assembly="Stimulsoft.Report.Web" Namespace="Stimulsoft.Report.Web" TagPrefix="cc1" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
       <link href="Design/css/grid.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <%--<telerik:RadAjaxManager ID="RadAjaxManager_EqShutdownReport" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="lnkSearch">
                 <updatedcontrols>
                     <telerik:AjaxUpdatedControl ControlID="StiWebViewer_EquipmentShutdownReport" LoadingPanelID="RadAjaxLoadingPanel_EqShutdownReport" />
                     <telerik:AjaxUpdatedControl ControlID="txtStartDate" />
                     <telerik:AjaxUpdatedControl ControlID="txtEndDate" />
                </updatedcontrols>
            </telerik:AjaxSetting>          
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel_EqShutdownReport" runat="server">
    </telerik:RadAjaxLoadingPanel>--%>
    <div class="divbutton-container">
        <div id="divButtons">
            <div id="breadcrumbs-wrapper">
                <header>
                    <div class="container row-color-grey">
                        <div class="row" style="height: 40px;">
                            <div class="col s12 m12 l12">
                                <div class="row">
                                    <div class="page-title"><i class="mdi-communication-contacts"></i>&nbsp;<asp:Label CssClass="title_text" ID="lblHeader" runat="server">Equipment Shut Down Report</asp:Label></div>

                                    <div class="btnclosewrap">
                                        <asp:LinkButton CssClass="icon-closed" ID="lnkClose" runat="server" CausesValidation="false" ToolTip="close"
                                            OnClick="lnkClose_Click"><i class="mdi-content-clear"></i></asp:LinkButton>
                                    </div>
                                </div>
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
                <div class="srchpaneinner" id="srchPanel" runat="server">
                      <div class="col-lg-12 col-md-12">
                            <div class="search-customer">
                                <div class="sc-form">
                                    <%--<div class="srchinputwrap">--%>
                                        <div runat="server" id="divStartDate" class="srchinputwrap">
                                            <div class="srchtitle srchtitlecustomwidth" style="padding-left: 15px;padding-top:18px;">
                                                <asp:Label Id="lblStartDate" Text="Start" runat="server" />
                                            </div>
                                            <div class="srchinputwrap">
                                                <asp:TextBox ID="txtStartDate" runat="server" CssClass="form-control"
                                                    MaxLength="50" Width="130px" autocomplete="off"></asp:TextBox>
                                                <asp:CalendarExtender ID="txtStartDate_CalendarExtender" runat="server" Enabled="True"
                                                    TargetControlID="txtStartDate">
                                                </asp:CalendarExtender>
                                                <asp:RequiredFieldValidator ID="rfvStartDt"
                                                    runat="server" ControlToValidate="txtStartDate" Display="None" ErrorMessage="Start date is Required" ValidationGroup="search"
                                                    SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                <asp:ValidatorCalloutExtender ID="vceStartDt" runat="server" Enabled="True"
                                                    PopupPosition="Right" TargetControlID="rfvStartDt" />
                                                <asp:RegularExpressionValidator ID="rfvStartDt1" ControlToValidate="txtStartDate" ValidationGroup="search"
                                                    ValidationExpression="^(0?[1-9]|1[012])[\/\-](0?[1-9]|[12][0-9]|3[01])[\/\-]\d{4}$"
                                                    runat="server" ErrorMessage="Invalid Date format. Valid Date Format 01/12/2001" Display="None">
                                                </asp:RegularExpressionValidator>
                                                <asp:ValidatorCalloutExtender ID="vceStartDt1" runat="server" Enabled="True" PopupPosition="Right"
                                                    TargetControlID="rfvStartDt1" />
                                            </div>
                                        </div>
                                        <div runat="server" id="divEndDate" class="srchinputwrap">
                                            <div class="srchtitle srchtitlecustomwidth" style="padding-left: 15px;padding-top:18px;">
                                                <asp:Label Id="lblEndDate" Text="End" runat="server" />
                                            </div>
                                             <div class="srchinputwrap">
                                                 <asp:TextBox ID="txtEndDate" runat="server" CssClass="form-control"
                                                MaxLength="50" Width="130px" autocomplete="off"></asp:TextBox>
                                            <asp:CalendarExtender ID="txtEndDate_CalendarExtender" runat="server" Enabled="True"
                                                TargetControlID="txtEndDate">
                                            </asp:CalendarExtender>
                                            <asp:RequiredFieldValidator ID="rfvEndDt"
                                                runat="server" ControlToValidate="txtEndDate" Display="None" ErrorMessage="End date is Required"  ValidationGroup="search"
                                                SetFocusOnError="True"></asp:RequiredFieldValidator>
                                            <asp:ValidatorCalloutExtender ID="vceEndDt" runat="server" Enabled="True"
                                                PopupPosition="Right" TargetControlID="rfvEndDt" />
                                            <asp:RegularExpressionValidator ID="rfvEndDt1" ControlToValidate = "txtEndDate" ValidationGroup="search" 
                                                ValidationExpression = "^(0?[1-9]|1[012])[\/\-](0?[1-9]|[12][0-9]|3[01])[\/\-]\d{4}$"
                                                runat="server" ErrorMessage="Invalid Date format. Valid Date Format 01/12/2001" Display="None">
                                            </asp:RegularExpressionValidator>
                                            <asp:ValidatorCalloutExtender ID="vceEndDt1" runat="server" Enabled="True" PopupPosition="Right"
                                                TargetControlID="rfvEndDt1" />
                                                 </div>

                                            <%--  <label for="" style="margin-left: 39px; margin-bottom: 19px;">End</label>--%>
                                       
                                        </div>
                                                                                                             </div>
                                <%--</div>--%>
                                <div class="srchinputwrap btnlinksicon srchclr">
                                    <asp:LinkButton ID="lnkSearch"  runat="server" CausesValidation="true" ToolTip="Refresh" ValidationGroup="search"
                                           OnClick="lnkSearch_Click" ><i class="fa fa-refresh"></i></asp:LinkButton>
                                </div>
                            </div>
                        </div>
                </div>
            </div>
            <div class="grid_container">
                <div class="form-section-row" style="margin-bottom: 0 !important;">
                    <%--view report--%>
                    <div class="col-lg-12 col-md-12">
                        <telerik:RadAjaxPanel ID="RadAjaxPanel21" runat="server" LoadingPanelID="RadAjaxLoadingPanel_EqShutdownReport">
                            <cc1:StiWebViewer ID="StiWebViewer_EquipmentShutdownReport" Height="800px" RequestTimeout="20000" runat="server" ViewMode="Continuous" ScrollbarsMode="true" />
                        </telerik:RadAjaxPanel>
                    </div>
                </div>
            </div>
        </div>
    </div>
    
</asp:Content>
