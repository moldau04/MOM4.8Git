<%@ Page Title="" Language="C#" MasterPageFile="~/Mom.master" AutoEventWireup="true" Inherits="Library" Codebehind="Library.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <!--Grid Control-->
    <link href="Design/css/grid.css" rel="stylesheet" />
    <style>
        .ajax__validatorcallout {
            max-width: 300px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div style="height: 65px !important;">
        <div id="divButtons">
            <div id="breadcrumbs-wrapper">
                <header>
                    <div class="container row-color-grey">
                        <div class="row">
                            <div class="col s12 m12 l12">
                                <div class="row">
                                    <div class="page-title"><i class="mdi-file-folder"></i>&nbsp;Shared Documents</div>

                                    <div class="rght-content">
                                        <div class="btnclosewrap">
                                            <asp:LinkButton ID="lnkClose" runat="server" ToolTip="Close" CausesValidation="false" OnClick="lnkClose_Click"><i class="mdi-content-clear"></i></asp:LinkButton>
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

    <div class="container">
        <div class="row">
            <asp:UpdatePanel ID="uplblcount" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="srchpane">
                        <div class="srchpaneinner">
                            <div class="srchtitle srchtitlecustomwidth" style="padding-left: 15px;">
                                Date
                            </div>
                            <div class="srchinputwrap">
                                <asp:TextBox ID="txtStartDate" runat="server" CssClass="srchcstm datepicker_mom" placeholder="From" MaxLength="50"></asp:TextBox>
                                <asp:CompareValidator
                                    ID="cvDateFormat1" runat="server"
                                    Type="Date"
                                    Operator="DataTypeCheck"
                                    ControlToValidate="txtStartDate"
                                    ErrorMessage="Invalid Date Format" Display="None">
                                </asp:CompareValidator>
                                <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender2"
                                    runat="server" Enabled="True" PopupPosition="Right" TargetControlID="cvDateFormat1">
                                </asp:ValidatorCalloutExtender>
                            </div>
                            <div class="srchinputwrap">
                                <asp:TextBox ID="txtEndDate" runat="server" CssClass="srchcstm datepicker_mom" placeholder="To" MaxLength="50"></asp:TextBox>
                                <asp:CompareValidator
                                    ID="cvDateFormat2" runat="server"
                                    Type="Date"
                                    Operator="DataTypeCheck"
                                    ControlToValidate="txtEndDate"
                                    ErrorMessage="Invalid Date Format" Display="None">
                                </asp:CompareValidator>
                                <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender3"
                                    runat="server" Enabled="True" PopupPosition="Right" TargetControlID="cvDateFormat2">
                                </asp:ValidatorCalloutExtender>
                                <asp:CompareValidator ID="cvSearchDate" runat="server" ControlToCompare="txtStartDate"
                                    ControlToValidate="txtEndDate" ErrorMessage="From Date cannot be greater than To Date" Operator="GreaterThanEqual"
                                    Type="Date" Display="None"></asp:CompareValidator>
                                <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender1"
                                    runat="server" Enabled="True" PopupPosition="Right" TargetControlID="cvSearchDate">
                                </asp:ValidatorCalloutExtender>                                
                            </div>
                            <div class="srchinputwrap btnlinksicon">
                                <asp:LinkButton ID="lnkSearch" runat="server" OnClick="lnkSearch_Click" CausesValidation="false"
                                    ToolTip="Search"><i class="mdi-action-search"></i></asp:LinkButton>
                            </div>
                            <div class="col lblsz2 lblszfloat">
                                <div class="row">


                                    <%--<span class="tro trost">
                                <asp:LinkButton ID="lnkShowAll" runat="server">Show All </asp:LinkButton>
                            </span>--%>
                                    <span class="tro trost">
                                        <asp:LinkButton ID="lnkClear" runat="server" OnClick="lnkClear_Click">Clear</asp:LinkButton>
                                    </span>
                                    <span class="tro trost">
                                        <asp:Label ID="lblRecordCount" runat="server">0 Record(s) found</asp:Label>

                                    </span>
                                </div>
                            </div>
                        </div>

                        <div class="srchpaneinner">
                            <div class="srchtitle srchtitlecustomwidth" style="padding-left: 15px;">
                                Search
                            </div>
                            <div class="srchinputwrap">
                                <asp:DropDownList ID="ddlSearch" runat="server" AutoPostBack="True" CssClass="browser-default select selectsml selectst"
                                    OnSelectedIndexChanged="ddlSearch_SelectedIndexChanged">
                                    <asp:ListItem Value="">Select</asp:ListItem>
                                    <asp:ListItem Value="loc">Location</asp:ListItem>
                                    <asp:ListItem Value="filename">File Name</asp:ListItem>
                                    <asp:ListItem Value="remarks">Description</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="srchinputwrap">
                                <asp:DropDownList Width="300px" ID="ddllocation" runat="server" CssClass="browser-default select selectsml selectst"
                                    Visible="False">
                                </asp:DropDownList>
                            </div>
                            <div class="srchinputwrap">
                                <asp:TextBox ID="txtSearch" placeholder="Search" runat="server" CssClass="srchcstm"></asp:TextBox>
                            </div>

                        </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>

        <div class="grid_container">
            <div class="form-section-row" style="margin-bottom: 0 !important;">
                <telerik:RadAjaxManager ID="RadAjaxManager_SharedDocument" runat="server">
                    <AjaxSettings>
                        <%--<telerik:AjaxSetting AjaxControlID="lnkDelete">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="RadGrid_SharedDocument" LoadingPanelID="RadAjaxLoadingPanel_SharedDocument" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>--%>
                        <telerik:AjaxSetting AjaxControlID="lnkSearch">
                            <UpdatedControls>
                                <telerik:AjaxUpdatedControl ControlID="RadGrid_SharedDocument" LoadingPanelID="RadAjaxLoadingPanel_SharedDocument" />
                                <telerik:AjaxUpdatedControl ControlID="pnlCount" />
                            </UpdatedControls>
                        </telerik:AjaxSetting>
                        <telerik:AjaxSetting AjaxControlID="lnkClear">
                            <UpdatedControls>
                                <telerik:AjaxUpdatedControl ControlID="RadGrid_SharedDocument" LoadingPanelID="RadAjaxLoadingPanel_SharedDocument" />
                            </UpdatedControls>
                        </telerik:AjaxSetting>
                        <%--<telerik:AjaxSetting AjaxControlID="lnkShowAll">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="RadGrid_SharedDocument" LoadingPanelID="RadAjaxLoadingPanel_SharedDocument" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                             <telerik:AjaxSetting AjaxControlID="lnkChk">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="RadGrid_SharedDocument" LoadingPanelID="RadAjaxLoadingPanel_SharedDocument" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>--%>
                    </AjaxSettings>
                </telerik:RadAjaxManager>
                <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel_SharedDocument" runat="server">
                </telerik:RadAjaxLoadingPanel>

                <div class="RadGrid RadGrid_Material">
                    <telerik:RadCodeBlock ID="codeBlock1" runat="server">
                        <script type="text/javascript">
                            function pageLoad() {
                                var grid = $find("<%= RadGrid_SharedDocument.ClientID %>");
                                var columns = grid.get_masterTableView().get_columns();
                                for (var i = 0; i < columns.length; i++) {
                                    columns[i].resizeToFit(false, true);
                                }
                            }
                            var requestInitiator = null;
                            var selectionStart = null;

                            function requestStart(sender, args) {
                                requestInitiator = document.activeElement.id;
                                if (document.activeElement.tagName == "INPUT") {
                                    selectionStart = document.activeElement.selectionStart;
                                }
                                var eventTarget = args.get_eventTarget();
                                if (eventTarget.indexOf("lblName") != -1) {
                                    args.set_enableAjax(false);
                                }
                            }

                            function responseEnd(sender, args) {
                                var element = document.getElementById(requestInitiator);
                                if (element && element.tagName == "INPUT") {
                                    element.focus();
                                    element.selectionStart = selectionStart;
                                }
                            }

                        </script>
                    </telerik:RadCodeBlock>
                    <telerik:RadAjaxPanel ID="RadAjaxPanel_SharedDocument" runat="server" LoadingPanelID="RadAjaxLoadingPanel_SharedDocument" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">

                        <telerik:RadPersistenceManager ID="RadPersistence_SharedDocument" runat="server">
                            <PersistenceSettings>
                                <telerik:PersistenceSetting ControlID="RadGrid_SharedDocument" />
                            </PersistenceSettings>
                        </telerik:RadPersistenceManager>
                        <telerik:RadGrid RenderMode="Auto" ID="RadGrid_SharedDocument" runat="server" CssClass="RadGrid_SharedDocument" AllowFilteringByColumn="true" ShowFooter="True" PageSize="50"
                            ShowStatusBar="false" AllowPaging="True" AllowSorting="true" Width="100%" PagerStyle-AlwaysVisible="true" FilterType="CheckList"
                            OnNeedDataSource="RadGrid_SharedDocument_NeedDataSource" OnItemCreated="RadGrid_SharedDocument_ItemCreated" OnPreRender="RadGrid_SharedDocument_PreRender" OnItemEvent="RadGrid_SharedDocument_ItemEvent">
                            <CommandItemStyle />
                            <GroupingSettings CaseSensitive="false" />
                            <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                                <Selecting AllowRowSelect="True"></Selecting>

                                <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                            </ClientSettings>
                            <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="True" ShowFooter="True">
                                <Columns>
                                    <telerik:GridTemplateColumn AllowFiltering="false">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="lblName" runat="server" CausesValidation="false"
                                                CommandArgument='<%#Eval("filename") + "," + Eval("Path") %>'
                                                OnClick="lblName_Click" Text='<%# Eval("filename") %>'
                                                ImageUrl='<%# ImageThumb(Eval("Path").ToString()) %>'
                                                Height="50px"></asp:ImageButton>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn FilterDelay="5" DataField="filename" HeaderText="File" SortExpression="filename" DataType="System.String"
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                        ShowFilterIcon="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblFileName" runat="server" Text='<%# Eval("filename") %>'></asp:Label>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn FilterDelay="5" DataField="remarks" HeaderText="Description" SortExpression="remarks" DataType="System.String"
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                        ShowFilterIcon="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRemarks" runat="server" Text='<%# Eval("remarks") %>'></asp:Label>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn FilterDelay="5" DataField="Location" HeaderText="Location" SortExpression="Location" DataType="System.String"
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                        ShowFilterIcon="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblLocation" runat="server" Text='<%# Eval("Location") %>'></asp:Label>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn FilterDelay="5" SortExpression="date" AutoPostBackOnFilter="true" DataField="date" DataType="System.String"
                                        CurrentFilterFunction="Contains" HeaderText="Date" ShowFilterIcon="false">
                                        <ItemTemplate>
                                            <%--<asp:Label ID="lblDate" runat="server" Text='<%# Eval("date", "{0:MM/dd/yyyy}") %>'></asp:Label>--%>
                                            <asp:Label ID="lblDate" runat="server" Text='<%# (String.IsNullOrEmpty(Eval("date").ToString())) ? "" : Eval("date", "{0:M/d/yyyy h:m:s tt}") %>'></asp:Label>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                </Columns>
                            </MasterTableView>
                            <FilterMenu CssClass="RadFilterMenu_CheckList">
                            </FilterMenu>
                        </telerik:RadGrid>
                    </telerik:RadAjaxPanel>
                </div>
            </div>
        </div>
        <div class="grid_container">
            <asp:ListView ID="lstDocs" runat="server" GroupItemCount="4">
                <EmptyDataTemplate>
                    <table>
                        <tr>
                            <td>No data was returned.</td>
                        </tr>
                    </table>
                </EmptyDataTemplate>
                <EmptyItemTemplate>
                    <td />
                </EmptyItemTemplate>
                <GroupTemplate>
                    <tr id="itemPlaceholderContainer" runat="server">
                        <td id="itemPlaceholder" runat="server"></td>
                    </tr>
                </GroupTemplate>
                <ItemTemplate>
                    <td runat="server">
                        <table>
                            <tr>
                                <td>
                                    <asp:ImageButton ID="lblName" runat="server" CausesValidation="false"
                                        CommandArgument='<%#Eval("filename") + "," + Eval("Path") %>'
                                        OnClick="lblName_Click" Text='<%# Eval("filename") %>'
                                        ImageUrl='<%# ImageThumb(Eval("Path").ToString()) %>'
                                        Height="50px"></asp:ImageButton>
                                </td>
                                <td>
                                    <span>
                                        <%# Eval("filename") %>
                                    </span>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                                <td>&nbsp;</td>
                            </tr>
                        </table>
                        </p>
                    </td>
                </ItemTemplate>
                <LayoutTemplate>
                    <table style="width: 100%;">
                        <tbody>
                            <tr>
                                <td>
                                    <table id="groupPlaceholderContainer" runat="server" style="width: 800px">
                                        <tr id="groupPlaceholder"></tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                            </tr>
                            <tr></tr>
                        </tbody>
                    </table>
                </LayoutTemplate>
            </asp:ListView>
        </div>
    </div>


    <asp:HiddenField runat="server" ID="hdnRcvPymtSelectDtRange" Value="" />
    <asp:HiddenField runat="server" ID="hdnCssActive" Value="1" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="javascriptHolder" runat="Server">
    <script type="text/javascript">

        $(document).ready(function () {


            $(window).scroll(function () {
                if ($(window).scrollTop() >= 0) {
                    $('#divButtons').addClass('fixed-header');
                }
                if ($(window).scrollTop() <= 0) {
                    $('#divButtons').removeClass('fixed-header');
                }
            });
        });
    </script>
    <script>

</script>
</asp:Content>
