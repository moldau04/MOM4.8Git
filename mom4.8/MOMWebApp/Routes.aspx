<%@ Page Title="Routes || MOM" Language="C#" MasterPageFile="~/MOMRadWindow.Master" AutoEventWireup="true" Inherits="Routes" Codebehind="Routes.aspx.cs" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <!--Grid Control-->
    <link href="Design/css/grid.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="divbutton-container">
        <div id="divButtons">
            <div id="breadcrumbs-wrapper">
                <header>
                    <div class="container row-color-grey">
                        <div class="row">
                            <div class="col s12 m12 l12">
                                <div class="row">
                                    <div class="col s12 m12 l12">
                                        <div class="row">
                                            <div class="page-title"><i class="mdi-maps-directions"></i> <span id="spnHead" runat="server">Routes</span></div>
                                            <div class="buttonContainer">
                                                <div class="btnlinks">
                                                    <asp:LinkButton ID="lnkAdd" runat="server" OnClick="lnkAdd_Click">Add</asp:LinkButton>
                                                </div>
                                                <div class="btnlinks">
                                                    <asp:LinkButton ID="lnkEdit" runat="server" OnClick="lnkEdit_Click">Edit</asp:LinkButton>
                                                </div>
                                                <div class="btnlinks">
                                                    <asp:LinkButton ID="lnkDelete" runat="server" OnClientClick="return CheckDelete();" OnClick="lnkDelete_Click">Delete</asp:LinkButton>
                                                </div>
                                                <%--  <div class="btnlinks">
                                                    <a class="dropdown-button" data-beloworigin="true" href="Routes.aspx" data-activates="dynamicUI">Reports
                                                    </a>
                                                </div>
                                              <ul id="dynamicUI" class="dropdown-content">
                                                    <li>
                                                        <a href="RouteListingReport.aspx?type=Route" class="-text">Add New Report</a>
                                                    </li>

                                                </ul>--%>
                                            </div>
                                            <%--<div class="btnclosewrap">
                                                <asp:LinkButton ID="lnkClose" runat="server" OnClick="lnkClose_Click"><i class="mdi-content-clear"></i></asp:LinkButton>
                                            </div>--%>
                                            <div class="rght-content">
                                            </div>
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
            <div class="col lblsz2 lblszfloat">
                <span class="tro trost">
                            <asp:CheckBox ID="lnkChk" runat="server" OnCheckedChanged="lnkchk_Click" AutoPostBack="True" CssClass="css-checkbox" Text="Incl. Inactive"></asp:CheckBox>
                        </span>
                <span class="tro trost">
                    <asp:UpdatePanel ID="updpnl" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Label ID="lblRecordCount" runat="server">0 Record(s) found.</asp:Label>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </span>
            </div>
            <div class="grid_container" style="margin-bottom: 15px !important">
                <div class="form-section-row" style="margin-bottom: 0 !important;">

                    <telerik:RadAjaxManager ID="RadAjaxManager_Routes" runat="server">
                        <AjaxSettings>
                            <telerik:AjaxSetting AjaxControlID="lnkDelete">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="RadGrid_Routes" LoadingPanelID="RadAjaxLoadingPanel_Routes" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                            <telerik:AjaxSetting AjaxControlID="lnkChk">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="RadGrid_Routes" LoadingPanelID="RadAjaxLoadingPanel_Routes" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                            <telerik:AjaxSetting AjaxControlID="lnkSearch">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="RadGrid_Routes" LoadingPanelID="RadAjaxLoadingPanel_Routes" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                            <telerik:AjaxSetting AjaxControlID="lnkShowAll">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="RadGrid_Routes" LoadingPanelID="RadAjaxLoadingPanel_Routes" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                        </AjaxSettings>
                    </telerik:RadAjaxManager>
                    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel_Routes" runat="server">
                    </telerik:RadAjaxLoadingPanel>
                    <div class="RadGrid RadGrid_Material">
                        <telerik:RadCodeBlock ID="codeBlock1" runat="server">
                            <script type="text/javascript">
                                function pageLoad() {
                                    var grid = $find("<%= RadGrid_Routes.ClientID %>");
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
                        <telerik:RadAjaxPanel ID="RadAjaxPanel_Routes" runat="server" LoadingPanelID="RadAjaxLoadingPanel_Routes" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">
                            <telerik:RadPersistenceManager ID="RadPersistenceRoutes" runat="server">
                                <PersistenceSettings>
                                    <telerik:PersistenceSetting ControlID="RadGrid_Routes" />
                                </PersistenceSettings>
                            </telerik:RadPersistenceManager>
                            <telerik:RadGrid RenderMode="Auto" ID="RadGrid_Routes" AllowFilteringByColumn="true" ShowFooter="True" PageSize="50"
                                ShowStatusBar="true" runat="server" AllowPaging="True" AllowSorting="true" Width="100%" PagerStyle-AlwaysVisible="true" FilterType="CheckList" OnItemCreated="RadGrid_Routes_ItemCreated"
                                AllowCustomPaging="True" OnNeedDataSource="RadGrid_Routes_NeedDataSource" OnPreRender="RadGrid_Routes_PreRender" OnItemEvent="RadGrid_Routes_ItemEvent">
                                <CommandItemStyle />
                                <GroupingSettings CaseSensitive="false" />
                                <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                                    <Selecting AllowRowSelect="True"></Selecting>

                                    <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                    <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                </ClientSettings>
                                <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="True" ShowFooter="True">
                                    <Columns>
                                        <telerik:GridClientSelectColumn UniqueName="chkSelect" HeaderStyle-Width="40">
                                        </telerik:GridClientSelectColumn>
                                        <telerik:GridTemplateColumn AllowFiltering="false" ShowFilterIcon="false" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblID" runat="server" Text='<%# Eval("id") %>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn DataField="name" SortExpression="name" AutoPostBackOnFilter="true"
                                            CurrentFilterFunction="Contains" ShowFilterIcon="false" UniqueName="DRoute" HeaderStyle-Width="120">
                                            <ItemTemplate>
                                                <asp:HyperLink ID="lnkName" runat="server" Text='<%# Bind("name") %>'></asp:HyperLink>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridBoundColumn DataField="Mechname" HeaderText="Worker" SortExpression="Mechname" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false" HeaderStyle-Width="120">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="Company" HeaderText="Company" SortExpression="Company" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                            ShowFilterIcon="false" HeaderStyle-Width="120">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="remarks" HeaderText="Remarks" SortExpression="remarks" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" HeaderStyle-Width="120"
                                            ShowFilterIcon="false">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="Status" HeaderText="Status" SortExpression="Status" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" HeaderStyle-Width="120"
                                            ShowFilterIcon="false">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridTemplateColumn DataField="Color" HeaderText="Assigned Color" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" HeaderStyle-Width="120"
                                            ShowFilterIcon="false" >
                                            <ItemTemplate>
                                                <center> 
                                               <asp:Panel ID="txtColor" Width="18px" Height="18px"  runat="server" ></asp:Panel>
                                               <asp:Label ID="lblColor" Visible="false" runat="server" Text='<%# Eval("Color") %>'></asp:Label>
                                            </center>
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
        </div>
    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="javascriptHolder" runat="Server">
    <script type="text/javascript">
        function DeleteSuccessMesg() {
            noty({
                text: 'Route deleted successfully!',
                type: 'success',
                layout: 'topCenter',
                closeOnSelfClick: false,
                timeout: 5000,
                theme: 'noty_theme_default',
                closable: true
            });
        }
        function closedMesg() {
            noty({
                text: 'Please select a Route to delete.',
                type: 'warning',
                layout: 'topCenter',
                closeOnSelfClick: false,
                timeout: 5000,
                theme: 'noty_theme_default',
                closable: true
            });
        }
        $(document).ready(function () {

            $('#colorNav #dynamicUI li').remove();

            //efficient-&-compact JQuery way
            $(reports).each(function (index, report) {
                //                alert('ReportId: ' + report.ReportId +
                //      ' ReportName: ' + report.ReportName +
                //      ' IsGlobal: ' + report.IsGlobal
                //        );


                var imagePath = null;
                if (report.IsGlobal == true) {
                    imagePath = "images/globe.png";
                }
                else {
                    imagePath = "images/blog_private.png";
                }
                $('#dynamicUI').append('<li><a href="RouteListingReport.aspx?reportId=' + report.ReportId + '&reportName=' + report.ReportName + '&type=Route"><span> <img src=images/reportfolder.png>  ' + report.ReportName + '</span><div style="clear:both;"></div></a></li>')
            });
        });

        $(document).ready(function () {
            console.log(1);
            $("#content").attr('style', 'margin-left:-170px;');
        });

    </script>

</asp:Content>
