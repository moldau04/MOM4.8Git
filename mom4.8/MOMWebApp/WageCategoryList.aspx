<%@ Page Title="Wage Category || MOM" Language="C#" MasterPageFile="~/Mom.master" AutoEventWireup="true" Inherits="WageCategoryList" CodeBehind="WageCategoryList.aspx.cs" EnableEventValidation="false" %>

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
                                    <div class="page-title"><i class="mdi-communication-contacts"></i>&nbsp;Wage Categoreis</div>
                                    <asp:Panel runat="server" ID="pnlGridButtons">
                                        <div class="buttonContainer">
                                            <div class="btnlinks">
                                                <asp:LinkButton ID="lnkAddnew" runat="server" OnClientClick='return AddWageClick();'>Add</asp:LinkButton>
                                            </div>
                                            <div class="btnlinks">
                                                <asp:LinkButton ID="btnEdit" runat="server" OnClientClick='return EditWageClick();'>Edit</asp:LinkButton>
                                            </div>
                                            <div class="btnlinks menuAction">
                                                <a onclick="DropdownMenu()" class="dropbtn" style="cursor: pointer;">Actions</a>
                                            </div>
                                            <ul id="drpMenu" class="nomgn hideMenu menuList">
                                                <li>
                                                    <div class="btnlinks">
                                                        <a id="btnCopy" runat="server" OnClientClick='return CopyWageClick();'>Copy</a>
                                                    </div>
                                                </li>
                                                <li>
                                                    <div class="btnlinks">
                                                        <asp:LinkButton ID="lnkDelete" runat="server" OnClientClick='return DeleteWageClick(this)'>Delete</asp:LinkButton>
                                                    </div>
                                                </li>
                                                <li>
                                                    <div class="btnlinks">
                                                        <asp:LinkButton ID="lnkExcel" runat="server">Export to Excel</asp:LinkButton>
                                                    </div>
                                                </li>
                                            </ul>
                                        </div>
                                    </asp:Panel>

                                    <div class="btnclosewrap">
                                        <asp:LinkButton ID="lnkClose" runat="server"><i class="mdi-content-clear" onclick="javascript:window.location.href='/Home.aspx';"></i></asp:LinkButton>
                                    </div>
                                    <div class="rght-content">
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
                <%-- <asp:UpdatePanel ID="upPannelSearch" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="srchtitle" style="padding-left: 15px; display: none;">
                            Search
                        </div>
                        <div class="srchinputwrap">
                            <asp:DropDownList ID="ddlSearch" runat="server" class="browser-default selectst selectsml" AutoPostBack="true" Style="display: none;">
                            </asp:DropDownList>

                        </div>
                        <div class="srchinputwrap">
                            <asp:DropDownList ID="ddlType" runat="server" CssClass="browser-default selectst" Visible="false" Style="display: none;">
                            </asp:DropDownList>
                        </div>
                        <div class="srchinputwrap">
                            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="browser-default selectst" Visible="false" Style="display: none;">
                                <asp:ListItem Value="Active">Active</asp:ListItem>
                                <asp:ListItem Value="InActive">InActive</asp:ListItem>
                                <asp:ListItem Value="Hold">Hold</asp:ListItem>
                            </asp:DropDownList>
                        </div>

                        <div class="srchinputwrap">
                            <asp:TextBox ID="txtSearch" runat="server" CssClass="srchcstm" placeholder="Search..." Style="display: none;"></asp:TextBox>
                        </div>
                        <div class="srchinputwrap srchclr btnlinksicon">

                            <asp:LinkButton ID="lnkSearch" runat="server"><i class="mdi-action-search"  style="display:none;"></i>
                            </asp:LinkButton>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>--%>
                <div class="col lblsz2 lblszfloat">
                    <div class="row">
                        <span class="tro trost">
                            <asp:CheckBox ID="lnkChk" runat="server" CssClass="css-checkbox" Text="Incl. Inactive" onclick="javascript:return pageLoad();"></asp:CheckBox>
                        </span>
                        <%-- <span class="tro trost">
                            <asp:LinkButton ID="lnkShowAll" runat="server">Show All </asp:LinkButton>
                        </span>--%>
                        <span class="tro trost">
                            <asp:LinkButton ID="lnkClear" runat="server" OnClientClick="javascript:return pageLoad();">Clear</asp:LinkButton>
                        </span>
                        <span class="tro trost">
                            <asp:Label ID="lblRecordCount" runat="server">0 Record(s) found.</asp:Label>
                        </span>
                    </div>
                </div>
            </div>
        </div>

        <div class="grid_container">
            <div class="form-section-row" style="margin-bottom: 0 !important;">
                <%-- <telerik:RadAjaxManager ID="RadAjaxManager_WageDeduction" runat="server">
                    <AjaxSettings>
                        <telerik:AjaxSetting AjaxControlID="lnkDelete">
                            <UpdatedControls>
                                <telerik:AjaxUpdatedControl ControlID="RadGrid_Wage" LoadingPanelID="RadAjaxLoadingPanel_WageDeduction" />
                                <telerik:AjaxUpdatedControl ControlID="lblRecordCount" />
                            </UpdatedControls>
                        </telerik:AjaxSetting>
                        <telerik:AjaxSetting AjaxControlID="lnkSearch">
                            <UpdatedControls>
                                <telerik:AjaxUpdatedControl ControlID="RadGrid_Wage" LoadingPanelID="RadAjaxLoadingPanel_WageDeduction" />
                                <telerik:AjaxUpdatedControl ControlID="lblRecordCount" />
                            </UpdatedControls>
                        </telerik:AjaxSetting>
                        <telerik:AjaxSetting AjaxControlID="lnkChk">
                            <UpdatedControls>
                                <telerik:AjaxUpdatedControl ControlID="RadGrid_Wage" LoadingPanelID="RadAjaxLoadingPanel_WageDeduction" />
                                <telerik:AjaxUpdatedControl ControlID="lblRecordCount" />
                            </UpdatedControls>
                        </telerik:AjaxSetting>
                        <telerik:AjaxSetting AjaxControlID="RadGrid_Wage">
                            <UpdatedControls>
                                <telerik:AjaxUpdatedControl ControlID="RadGrid_Wage" LoadingPanelID="RadAjaxLoadingPanel_WageDeduction" />
                                <telerik:AjaxUpdatedControl ControlID="lblRecordCount" />
                            </UpdatedControls>
                        </telerik:AjaxSetting>
                    </AjaxSettings>
                </telerik:RadAjaxManager>--%>
                <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel_WageDeduction" runat="server">
                </telerik:RadAjaxLoadingPanel>
                <div class="RadGrid RadGrid_Material">
                    <telerik:RadAjaxPanel ID="RadAjaxPanel_Wage" runat="server" LoadingPanelID="RadAjaxLoadingPanel_Wage" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">
                        <telerik:RadGrid RenderMode="Auto" runat="server" ID="RadGrid_Wage" AllowFilteringByColumn="true" ShowFooter="True" FilterType="CheckList"
                            PagerStyle-AlwaysVisible="true" ShowStatusBar="true" AllowPaging="True" AllowSorting="true" Width="100%"
                            PagerStyle-EnableAllOptionInPagerComboBox="true" AllowCustomPaging="True" AllowCustomSorting="true" PagerStyle-PageSizes="10,20,25,50,100,500" PageSize="1000">
                            <CommandItemStyle />
                            <GroupingSettings CaseSensitive="false" />
                            <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                                <Selecting AllowRowSelect="True"></Selecting>
                                <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                            </ClientSettings>
                            <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="True" ShowFooter="True" AllowSorting="true" AllowPaging="true" AllowCustomSorting="true">
                                <Columns>
                                    <telerik:GridClientSelectColumn UniqueName="chkSelect" HeaderStyle-Width="40">
                                    </telerik:GridClientSelectColumn>
                                    <telerik:GridBoundColumn UniqueName="WageId" FilterDelay="5" DataField="ID" HeaderText="ID" SortExpression="ID" Visible="false" ShowFilterIcon="false">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridHyperLinkColumn DataNavigateUrlFields="ID" DataNavigateUrlFormatString="WageCategory.aspx?id={0}" DataTextField="fDesc" HeaderText="Description" UniqueName="fDesc" SortExpression="fDesc" AutoPostBackOnFilter="true" ShowFilterIcon="false">
                                    </telerik:GridHyperLinkColumn>
                                    <telerik:GridBoundColumn UniqueName="Field" DataField="Field" HeaderText="Field" AutoPostBackOnFilter="true" SortExpression="Field" ShowFilterIcon="false">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn UniqueName="Reg" DataField="Reg" HeaderText="Reg Rate" AutoPostBackOnFilter="true" SortExpression="Reg"
                                        ShowFilterIcon="false" DataType="System.Decimal" DataFormatString="{0:C}">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn UniqueName="OT1" DataField="OT1" HeaderText="OT Rate" AutoPostBackOnFilter="true" SortExpression="OT"
                                        ShowFilterIcon="false" DataType="System.Decimal" DataFormatString="{0:C}">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn UniqueName="NT" DataField="NT" HeaderText="1.7 Rate" AutoPostBackOnFilter="true" SortExpression="NT"
                                        ShowFilterIcon="false" DataType="System.Decimal" DataFormatString="{0:C}">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn UniqueName="OT2" DataField="OT2" HeaderText="DT Rate" AutoPostBackOnFilter="true"
                                        ShowFilterIcon="false" DataType="System.Decimal" DataFormatString="{0:C}">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn UniqueName="sStatus" DataField="sStatus" HeaderText="Status" AutoPostBackOnFilter="true" SortExpression="sStatus" ShowFilterIcon="false">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn UniqueName="Count" DataField="Count" HeaderText="Count" AutoPostBackOnFilter="true" SortExpression="Count"
                                        ShowFilterIcon="false">
                                    </telerik:GridBoundColumn>
                                </Columns>
                            </MasterTableView>
                            <ClientSettings>
                                <ClientEvents OnCommand="RadGrid_Wage_OnCommand"></ClientEvents>
                            </ClientSettings>
                        </telerik:RadGrid>
                    </telerik:RadAjaxPanel>
                </div>
            </div>
        </div>
    </div>

    <!-- Hidden Field -->
    <asp:HiddenField runat="server" ID="hdnAddDedcutions" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnEditDedcutions" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnDeleteDedcutions" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnViewDedcutions" Value="Y" />
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="javascriptHolder" runat="Server">
    <script src="js/commonAPI.js"></script>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script type="text/javascript">
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

            var cbIncludeInActive = $('#<%= lnkChk.ClientID %>');
            var gridData;
            var Filters = new Array();
            var currentPageIndex = 0;
            var PageSize = 25;
            var SortField = null;
            var SortOrder = 1;
            var totalRecords = 0;
            var totalPages = 0;
            var Active = null;
            function pageLoad() {
                currentPageIndex = 0;
                var grid = $find("<%= RadGrid_Wage.ClientID %>");
                var gridView = grid.get_masterTableView();
                gridView.clearFilter();
                if (gridView != null) {
                    var columns = gridView.get_columns();
                    for (var i = 0; i < columns.length; i++) {
                        columns[i].resizeToFit(false, true);
                    }
                    //BindRedGrid(gridView);
                }
            }

            function RadGrid_Wage_OnCommand(sender, args) {
                internalDeselect = true;
                var commandName = args.get_commandName();
                var shouldAllowOperation = commandName == "RebindGrid";
                args.set_cancel(!shouldAllowOperation);
                var gridView = sender.get_masterTableView();
                currentPageIndex = gridView.get_currentPageIndex();
                if (args.get_commandName() === "PageSize" || args.get_commandName() === "Page") {
                    PageSize = gridView.get_pageSize();
                }
                var filterExpressions = gridView.get_filterExpressions();
                filterExpressions._array.forEach(function (data) {
                    if (data._fieldValue.toString() != '') {
                        var col = data._fieldName.toString();
                        var filterValue;
                        if (data._fieldValue.toString().toLowerCase() === 'no' || data._fieldValue.toString().toLowerCase() === 'active') {
                            filterValue = 0;
                        }
                        else if (data._fieldValue.toString().toLowerCase() === 'yes' || data._fieldValue.toString().toLowerCase() === 'inactive') {
                            filterValue = 1;
                        }
                        else {
                            filterValue = data._fieldValue.toString();
                        }
                        var filter = {
                            key: col,
                            value: filterValue
                        }
                        Filters.push(filter);
                    }
                });

                if (args.get_commandName() == "Sort") {
                    gridView.set_currentPageIndex(0);
                }
                var sortExpressions = gridView.get_sortExpressions();
                if (sortExpressions._array.length > 0) {
                    SortField = sortExpressions.getItem(0).get_fieldName();
                    SortOrder = sortExpressions.getItem(0).get_sortOrder();
                }
                BindRedGrid(gridView);
                while (Filters.length) {
                    Filters.pop();
                }
                SortField = null;
                SortOrder = 1;
                Active = null;
                internalDeselect = false;
            }
            function BindRedGrid(gridView) {
                if ($(cbIncludeInActive).is(":checked")) {
                    Active = null;
                }
                else if (!$(cbIncludeInActive).is(":checked")) {
                    Active = 0;
                }
                var Model = {
                    PageNumber: currentPageIndex + 1,
                    PageSize: PageSize,
                    SortField: SortField,
                    SortOrder: SortOrder,
                    Active: Active,
                    Filters: Filters
                };
                AccessControler.Post(Model, 30000, "./api/PayrollApi/GetWageDeductionList", function (response) {
                    if (response != null) {
                        gridData = JSON.parse(response);
                        gridView.set_dataSource(gridData);
                        gridView.dataBind();
                        totalRecords = gridData[0].totalRecords;
                        gridView.set_virtualItemCount(totalRecords);
                        totalPages = Math.ceil(totalRecords / PageSize);
                        gridView.set_pageSize(PageSize);
                        $('#<%=lblRecordCount.ClientID%>').text(totalRecords + " Record(s) found");
                    }
                });
            }
        </script>
    </telerik:RadCodeBlock>


    <script type="text/javascript">
        function AddWageClick() {
            var id = document.getElementById('<%= hdnAddDedcutions.ClientID%>').value;
            if (id == "Y") {
                window.location.href = "WageCategory.aspx";
                return false;
            } else {
                noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                return false;
            }
        }

        var WageId = '';
        function EditWageClick() {
            var id = document.getElementById('<%= hdnEditDedcutions.ClientID%>').value;
            if (id == "Y") {
                var grid = $find("<%= RadGrid_Wage.ClientID %>");
                var gridView = grid.get_masterTableView();
                WageId = gridView.get_selectedItems()[0]._dataItem['ID'];
                window.location.href = "WageCategory.aspx?id=" + WageId;
                return false;
            }
            else {
                noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                return false;
            }
        }
        function CopyWageClick() {
            var id = document.getElementById('<%= hdnEditDedcutions.ClientID%>').value;
            if (id == "Y") {
                var grid = $find("<%= RadGrid_Wage.ClientID %>");
                var gridView = grid.get_masterTableView();
                WageId = gridView.get_selectedItems()[0]._dataItem['ID'];
                window.location.href = "WageCategory.aspx?id=" + WageId + "&t=c";
                return false;
            }
            else {
                noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                return false;
            }
        }
        function DeleteWageClick(hyperlink) {
            var id = document.getElementById('<%= hdnDeleteDedcutions.ClientID%>').value;
            if (id == "Y") {
                return SelectedRowDelete('<%= RadGrid_Wage.ClientID%>', 'Vendor');
            } else {
                noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                return false;
            }
        }

      <%--  function clear() {
            ("#ddlType");
        }
        function jsFunction(value) {
            if (value == "Rol.Type") {
                document.getElementById('<%=txtSearch.ClientID%>').value = '';
                document.getElementById('<%=ddlType.ClientID%>').style.display = 'block';
                document.getElementById('<%=ddlStatus.ClientID%>').style.display = 'none';
                document.getElementById('<%=txtSearch.ClientID%>').style.display = 'none';

            }
            else if (value == "Vendor.Status") {
                document.getElementById('<%=txtSearch.ClientID%>').value = '';
                document.getElementById('<%=ddlType.ClientID%>').style.display = 'none';
                document.getElementById('<%=ddlStatus.ClientID%>').style.display = 'block';
                document.getElementById('<%=txtSearch.ClientID%>').style.display = 'none';

            }
            else {
                document.getElementById('<%=ddlType.ClientID%>').style.display = 'none';
                document.getElementById('<%=ddlStatus.ClientID%>').style.display = 'none';
                document.getElementById('<%=txtSearch.ClientID%>').style.display = 'block';
            }
        }--%>
        jQuery(document).ready(function () {
            $('#colorNav #dynamicUI li').remove();
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
</asp:Content>
