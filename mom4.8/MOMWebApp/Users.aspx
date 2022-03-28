<%@ Page Title="Users || MOM" Language="C#" MasterPageFile="~/Mom.master" AutoEventWireup="true" Inherits="Users" EnableEventValidation="false" CodeBehind="Users.aspx.cs" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <!--Grid Control-->
    <link href="Design/css/grid.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <telerik:RadAjaxManager ID="RadAjaxManager_Uesrs" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="lnkDelete">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_Uesrs" LoadingPanelID="RadAjaxLoadingPanel_Uesrs" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="lnkSearch">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_Uesrs" LoadingPanelID="RadAjaxLoadingPanel_Uesrs" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="lnkChk">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_Uesrs" LoadingPanelID="RadAjaxLoadingPanel_Uesrs" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="lnkShowAll">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_Uesrs" LoadingPanelID="RadAjaxLoadingPanel_Uesrs" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="lnkClear">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_Uesrs" LoadingPanelID="RadAjaxLoadingPanel_Uesrs" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnSearchRole">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_Roles" LoadingPanelID="RadAjaxLoadingPanel_Uesrs" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="chkIncInactiveRole">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_Roles" LoadingPanelID="RadAjaxLoadingPanel_Uesrs" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="lnkShowAllRole">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_Roles" LoadingPanelID="RadAjaxLoadingPanel_Uesrs" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel_Uesrs" runat="server">
    </telerik:RadAjaxLoadingPanel>

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
                                            <div class="page-title"><i class="mdi-social-people"></i>&nbsp;Users</div>
                                            <asp:Panel runat="server" ID="pnlGridButtons">
                                                <div class="buttonContainer">
                                                    <div class="btnlinks">
                                                        <asp:LinkButton ID="lnkAddnew" runat="server" OnClick="lnkAddnew_Click">Add</asp:LinkButton>
                                                    </div>
                                                    <div class="btnlinks">
                                                        <asp:LinkButton ID="lnkEdit" runat="server" OnClick="lnkEdit_Click">Edit</asp:LinkButton>
                                                    </div>
                                                    <div class="btnlinks menuAction">
                                                        <a onclick="DropdownMenu()" class="dropbtn" style="cursor: pointer;">Actions
                                                        </a>
                                                    </div>

                                                    <ul id="drpMenu" class="nomgn hideMenu menuList">
                                                        <li>
                                                            <div class="btnlinks">
                                                                <asp:LinkButton ID="lnkCopy" runat="server" OnClick="lnkCopy_Click">Copy</asp:LinkButton>
                                                            </div>
                                                        </li>
                                                        <li runat="server" visible="false">
                                                            <div class="btnlinks">
                                                                <asp:LinkButton ID="lnkDelete" runat="server" OnClick="lnkDelete_Click" OnClientClick="return confirm('Do you really want to delete this user ?');">Delete</asp:LinkButton>
                                                            </div>
                                                        </li>
                                                        <li>
                                                            <div class="btnlinks">
                                                                <a class="dropdown-button" data-beloworigin="true" href="#" data-activates="dropdownExcel">Export to Excel
                                                                </a>
                                                            </div>
                                                            <ul id="dropdownExcel" class="dropdown-content">
                                                                <li>
                                                                    <asp:LinkButton ID="lnkExcel" runat="server" OnClick="lnkExcel_Click">Export Users to Excel</asp:LinkButton>
                                                                </li>
                                                                <li>
                                                                    <asp:LinkButton ID="lnkUserRoleExcel" runat="server" OnClick="lnkUserRoleExcel_Click">Export User Roles to Excel</asp:LinkButton>
                                                                </li>
                                                            </ul>
                                                        </li>
                                                        <li>
                                                            <div class="btnlinks">
                                                                <a class="dropdown-button" data-beloworigin="true" href="customersreport.aspx" data-activates="dropdown1">Reports
                                                                </a>
                                                            </div>
                                                            <ul id="dropdown1" class="dropdown-content">
                                                                <li>
                                                                    <asp:LinkButton ID="lnkUserReport" runat="server" OnClick="lnkUserReport_Click">User Report</asp:LinkButton>
                                                                </li>
                                                                <li>
                                                                    <asp:LinkButton ID="lnkUserPayment" runat="server" OnClick="lnkUserPayment_Click">User Payment Report</asp:LinkButton>
                                                                </li>
                                                            </ul>
                                                        </li>
                                                    </ul>
                                                </div>
                                            </asp:Panel>
                                            <div class="btnclosewrap">
                                                <asp:LinkButton ID="lnkClose" runat="server" OnClick="lnkClose_Click"><i class="mdi-content-clear"></i></asp:LinkButton>
                                            </div>
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
            <div class="card" style="min-height: 70vh !important; border-radius: 6px; margin-top: -10px;">
                <div class="card-content">
                    <ul class="tabs tab-demo-active white w-100p"  id="tabProject">
                        <li class="tab col s2">
                            <a class="white-text waves-effect waves-light active" id="1" href="#activeone" onclick="SetTabActiveValue(1);"><i class="mdi-action-verified-user"></i>&nbsp;Users</a>
                        </li>
                        <li class="tab col s2">
                            <a class="white-text waves-effect waves-light" id="2" href="#two" onclick="SetTabActiveValue(2);"><i class="mdi-action-group-work"></i>&nbsp;User Roles</a>
                        </li>

                    </ul>
                    <div id="activeone" class="col s12 tab-container-border lighten-4" style="display: block;">
                        <div class="row">
                            <div class="srchpane">
                                <asp:UpdatePanel ID="upPannelSearch" runat="server">
                                    <ContentTemplate>
                                        <div class="srchtitle srchtitlecustomwidth ser-css2">
                                            Search
                                        </div>
                                        <div class="srchinputwrap">
                                            <asp:DropDownList ID="ddlSearch" runat="server" onchange="showfilter();return false;"
                                                class="browser-default selectsml selectst">
                                                <asp:ListItem>Select</asp:ListItem>
                                                <asp:ListItem Value="fUser">Username</asp:ListItem>
                                                <asp:ListItem Value="fFirst">First Name</asp:ListItem>
                                                <asp:ListItem Value="e.Last">Last Name</asp:ListItem>
                                                <asp:ListItem Value="usertype">Type</asp:ListItem>
                                                <asp:ListItem Value="u.Status">Status</asp:ListItem>
                                                <asp:ListItem Value="w.super">Supervisor</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                        <div class="srchinputwrap" id="div_rbStatus" style="display: none" runat="server">
                                            <asp:DropDownList ID="rbStatus" runat="server" CssClass="browser-default selectst" Visible="True">
                                                <asp:ListItem Value="0">Active</asp:ListItem>
                                                <asp:ListItem Value="1">Inactive</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                        <div class="srchinputwrap" id="div_ddlUserType" style="display: none" runat="server">
                                            <asp:DropDownList ID="ddlUserType" runat="server" CssClass="browser-default selectst" Visible="True">
                                                <asp:ListItem Value="0">Office</asp:ListItem>
                                                <asp:ListItem Value="1">Field</asp:ListItem>
                                                <asp:ListItem Value="2">Customer</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                        <div class="srchinputwrap" id="div_ddlSuper" style="display: none" runat="server">
                                            <asp:DropDownList ID="ddlSuper" runat="server" CssClass="browser-default selectst" Visible="True">
                                            </asp:DropDownList>
                                        </div>
                                        <div class="srchinputwrap" id="div_txtSearch" style="display: block" runat="server">
                                            <asp:TextBox ID="txtSearch" runat="server" class="srchcstm" placeholder="Search"></asp:TextBox>
                                        </div>
                                        <div class="srchinputwrap srchclr btnlinksicon">
                                            <asp:LinkButton ID="lnkSearch" runat="server" OnClick="lnkSearch_Click" ToolTip="Search"><i class="mdi-action-search"></i></asp:LinkButton>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <div class="col lblsz2 lblszfloat">
                                    <div class="row">
                                        <span class="tro trost">
                                            <asp:Label ID="lblChkSelect" runat="server" CssClass="title-check-text" For="lnkChk">
                                                <asp:CheckBox ID="lnkChk" runat="server" OnCheckedChanged="lnkchk_Click" AutoPostBack="True" CssClass="filled-in"></asp:CheckBox>
                                                Incl. Inactive</asp:Label>
                                        </span>
                                        <span class="tro trost">
                                            <asp:LinkButton ID="lnkShowAll" runat="server" OnClick="lnkShowAll_Click" OnClientClick="resetShowAll();">Show All </asp:LinkButton>
                                        </span>
                                        <span class="tro trost">
                                            <asp:LinkButton ID="lnkClear" runat="server" OnClick="lnkClear_Click" OnClientClick="resetShowAll();">Clear</asp:LinkButton>
                                        </span>
                                        <span class="tro trost">
                                            <asp:UpdatePanel ID="updpnl" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:Label ID="lblRecordCount" runat="server">0 Record(s) found.</asp:Label>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </span>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="grid_container">
                            <div class="form-section-row m-b-0" >
                                <div class="RadGrid RadGrid_Material">
                                    <telerik:RadCodeBlock ID="codeBlock1" runat="server">
                                        <script type="text/javascript">
                                            function pageLoad() {
                                                var grid = $find("<%= RadGrid_Uesrs.ClientID %>");
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
                                    <telerik:RadAjaxPanel ID="RadAjaxPanel_Uesrs" runat="server" LoadingPanelID="RadAjaxLoadingPanel_Uesrs" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">


                                        <telerik:RadGrid RenderMode="Auto" ID="RadGrid_Uesrs" AllowFilteringByColumn="true" ShowFooter="True" PageSize="50"
                                            ShowStatusBar="true" runat="server" AllowPaging="True" AllowSorting="true" Width="100%" PagerStyle-AlwaysVisible="true" FilterType="CheckList"
                                            AllowCustomPaging="True" OnNeedDataSource="RadGrid_Uesrs_NeedDataSource" OnPreRender="RadGrid_Uesrs_PreRender" OnItemCreated="RadGrid_Uesrs_ItemCreated" OnItemEvent="RadGrid_Uesrs_ItemEvent">
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
                                                    <telerik:GridTemplateColumn AllowFiltering="false" ShowFilterIcon="false" Visible="false" UniqueName="UserID">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblId" runat="server" Text='<%# Bind("UserID") %>'></asp:Label>
                                                            <asp:Label ID="lblTypeid" runat="server" Text='<%# Bind("usertypeid") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn UniqueName="fuser" SortExpression="fuser" AutoPostBackOnFilter="true" DataField="fuser"
                                                        CurrentFilterFunction="Contains" HeaderText="Username" ShowFilterIcon="false">
                                                        <ItemTemplate>
                                                            <asp:HyperLink ID="lnkName" runat="server" Text='<%# Bind("fuser") %>'></asp:HyperLink>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridBoundColumn UniqueName="ffirst" DataField="ffirst" HeaderText="First Name" SortExpression="ffirst"
                                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn UniqueName="lLast" DataField="lLast" HeaderText="Last Name" SortExpression="lLast"
                                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn UniqueName="usertype" DataField="usertype" HeaderText="Type" SortExpression="usertype"
                                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false" HeaderStyle-Width="120">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridTemplateColumn UniqueName="status" SortExpression="status" AutoPostBackOnFilter="true" DataField="status"
                                                        CurrentFilterFunction="Contains" HeaderText="Status" ShowFilterIcon="false" HeaderStyle-Width="100">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblStatus" runat="server"><%# Convert.ToInt32( Eval("status")) == 0 ? "Active" : "Inactive"%></asp:Label>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridBoundColumn DataField="super" HeaderText="Supervisor" SortExpression="super" UniqueName="super"
                                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="RoleName" HeaderText="User Role" SortExpression="RoleName" UniqueName="RoleName"
                                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                                    </telerik:GridBoundColumn>

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
                    <div id="two" class="col s12 tab-container-border lighten-4">
                        <div class="row">
                            <div class="srchpane">
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <ContentTemplate>
                                        <div class="srchtitle srchtitlecustomwidth ser-css2" >
                                            Search
                                        </div>
                                        <div class="srchinputwrap">
                                            <asp:DropDownList ID="ddlSearchRole" runat="server" onchange="showRolefilter();return false;"
                                                class="browser-default selectsml selectst">
                                                <asp:ListItem>Select</asp:ListItem>
                                                <asp:ListItem Value="rolename">Role Name</asp:ListItem>
                                                <asp:ListItem Value="status">Status</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                        
                                        <div class="srchinputwrap" id="div_txtSearchRole" style="display: block" runat="server">
                                            <asp:TextBox ID="txtSearchRole" runat="server" class="srchcstm" placeholder="Search"></asp:TextBox>
                                        </div>

                                        <div class="srchinputwrap" id="div_ddlRoleStatus" style="display: none" runat="server">
                                            <asp:DropDownList ID="ddlRoleStatus" runat="server" CssClass="browser-default selectst" Visible="True">
                                                <asp:ListItem Value="0">Active</asp:ListItem>
                                                <asp:ListItem Value="1">Inactive</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                        <div class="srchinputwrap srchclr btnlinksicon">
                                            <asp:LinkButton ID="btnSearchRole" runat="server" OnClick="btnSearchRole_Click" ToolTip="Search"><i class="mdi-action-search"></i></asp:LinkButton>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <div class="col lblsz2 lblszfloat">
                                    <div class="row">
                                        <span class="tro trost">
                                            <asp:Label ID="Label1" runat="server" CssClass="title-check-text" For="chkIncInactiveRole">
                                                <asp:CheckBox ID="chkIncInactiveRole" runat="server" OnCheckedChanged="chkIncInactiveRole_CheckedChanged" AutoPostBack="True" CssClass="filled-in"></asp:CheckBox>
                                                Incl. Inactive</asp:Label>
                                        </span>
                                        <span class="tro trost">
                                            <asp:LinkButton ID="lnkShowAllRole" runat="server" OnClick="lnkShowAllRole_Click" OnClientClick="resetRoleShowAll();">Show All </asp:LinkButton>
                                        </span>
                                        <%--<span class="tro trost">
                                            <asp:LinkButton ID="lnkClearRole" runat="server" OnClick="lnkClearRole_Click" OnClientClick="resetShowAll();">Clear</asp:LinkButton>
                                        </span>--%>
                                        <span class="tro trost">
                                            <asp:UpdatePanel ID="upnlRecordRoleCount" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:Label ID="lblRecordRoleCount" runat="server">0 Record(s) found.</asp:Label>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </span>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="grid_container">
                            <div class="form-section-row m-b-0" >
                                <div class="RadGrid RadGrid_Material">
                                    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
                                        <script type="text/javascript">
                                            function pageLoad() {
                                                var grid = $find("<%= RadGrid_Roles.ClientID %>");
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
                                    <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel_Uesrs" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">
                                        <telerik:RadGrid RenderMode="Auto" ID="RadGrid_Roles" AllowFilteringByColumn="true" ShowFooter="True" PageSize="50"
                                            ShowStatusBar="true" runat="server" AllowPaging="True" AllowSorting="true" Width="100%" PagerStyle-AlwaysVisible="true" FilterType="CheckList"
                                            AllowCustomPaging="True" 
                                            OnNeedDataSource="RadGrid_Roles_NeedDataSource" 
                                            OnPreRender="RadGrid_Roles_PreRender" 
                                            OnItemCreated="RadGrid_Roles_ItemCreated" 
                                            OnItemEvent="RadGrid_Roles_ItemEvent">
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
                                                    <telerik:GridTemplateColumn AllowFiltering="false" ShowFilterIcon="false" Visible="false" UniqueName="RoleID">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblId" runat="server" Text='<%# Bind("ID") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn UniqueName="RoleName" SortExpression="RoleName" AutoPostBackOnFilter="true" DataField="RoleName"
                                                        CurrentFilterFunction="Contains" HeaderText="Role Name" ShowFilterIcon="false" HeaderStyle-Width="350">
                                                        <%--<ItemTemplate>
                                                            <asp:HyperLink ID="lnkRoleName" runat="server" Text='<%# Bind("RoleName") %>'></asp:HyperLink>
                                                        </ItemTemplate>--%>
                                                        <ItemTemplate>
                                                            <asp:HyperLink ID="lnkRoleName" runat="server" NavigateUrl='<%# "addUserRole.aspx?uid=" + Eval("ID") %>'><%#Eval("RoleName")%></asp:HyperLink>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn UniqueName="Status" DataField="Status" HeaderText="Status" SortExpression="Status" HeaderStyle-Width="100"
                                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblStatus" runat="server"><%# Convert.ToInt32( Eval("status")) == 0 ? "Active" : "Inactive"%></asp:Label>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridBoundColumn UniqueName="Description" DataField="Desc" HeaderText="Description" SortExpression="Desc"
                                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                                    </telerik:GridBoundColumn>

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

            </div>
        </div>
    </div>
    <asp:HiddenField ID="hdnActiveTab" runat="server" Value="1" />
    <asp:HiddenField ID="hdnFormularFieldID" runat="server" />
    <asp:HiddenField ID="hdnFormularValue" runat="server" />
    <asp:HiddenField ID="hdnFireTestDate" runat="server" />

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="javascriptHolder" runat="Server">
    <script>
        $(document).ready(function () {

            $("#<%=RadGrid_Uesrs.ClientID%>").attr("autocomplete", "off");
            $(window).scroll(function () {
                if ($(window).scrollTop() >= 0) {
                    $('#divButtons').addClass('fixed-header');
                }
                if ($(window).scrollTop() <= 0) {
                    $('#divButtons').removeClass('fixed-header');
                }
            });
        });
        function resetShowAll() {
            var ddlSearch = $('#<%=ddlSearch.ClientID%>');
            var txtSearch = document.getElementById('<%=div_txtSearch.ClientID%>');
            var rbStatus = document.getElementById('<%=div_rbStatus.ClientID%>');
            var ddlUserType = document.getElementById('<%=div_ddlUserType.ClientID%>');
            var ddlSuper = document.getElementById('<%=div_ddlSuper.ClientID%>');

            txtSearch.style.display = "block";
            rbStatus.style.display = "none";
            ddlUserType.style.display = "none";
            ddlSuper.style.display = "none";

            try {
                $('#<%=rbStatus.ClientID%>').val($("#<%=rbStatus.ClientID%> option:first").val());
                $('#<%=ddlUserType.ClientID%>').val($("#<%=ddlUserType.ClientID%> option:first").val());
                $('#<%=ddlSuper.ClientID%>').val($("#<%=ddlSuper.ClientID%> option:first").val());
            } catch (ex) { }
        }

        function showfilter() {
            var ddlSearch = $('#<%=ddlSearch.ClientID%>');
            var txtSearch = document.getElementById('<%=div_txtSearch.ClientID%>');
            var rbStatus = document.getElementById('<%=div_rbStatus.ClientID%>');
            var ddlUserType = document.getElementById('<%=div_ddlUserType.ClientID%>');
            var ddlSuper = document.getElementById('<%=div_ddlSuper.ClientID%>');
 
            txtSearch.style.display = "none";
            rbStatus.style.display = "none";
            ddlUserType.style.display = "none";
            ddlSuper.style.display = "none";

            switch (String(ddlSearch.val())) {
                case "u.Status":
                    rbStatus.style.display = "block";
                    break;
                case "usertype":
                    ddlUserType.style.display = "block";
                    break;
                case "w.super":
                    ddlSuper.style.display = "block";
                    break;
                default:
                    txtSearch.style.display = "block";
                    var txt = $('#<%=txtSearch.ClientID%>');
                    txt.val("");
                    break;
            }

            try {
                $('#<%=rbStatus.ClientID%>').val($("#<%=rbStatus.ClientID%> option:first").val());
                $('#<%=ddlUserType.ClientID%>').val($("#<%=ddlUserType.ClientID%> option:first").val());
                $('#<%=ddlSuper.ClientID%>').val($("#<%=ddlSuper.ClientID%> option:first").val());
            } catch (ex) { }
        }

        function SetTabActiveValue(tabno) {
            $("#<%= hdnActiveTab.ClientID%>").val(tabno);
        }

        function showRolefilter() {
            var ddlSearch = $('#<%=ddlSearchRole.ClientID%>');
            var txtSearch = document.getElementById('<%=div_txtSearchRole.ClientID%>');
            var rbStatus = document.getElementById('<%=div_ddlRoleStatus.ClientID%>');
            
            txtSearch.style.display = "none";
            rbStatus.style.display = "none";
            
            switch (String(ddlSearch.val())) {
                case "status":
                    rbStatus.style.display = "block";
                    break;
                default:
                    txtSearch.style.display = "block";
                    var txt = $('#<%=txtSearchRole.ClientID%>');
                    txt.val("");
                    break;
            }

            try {
                $('#<%=ddlRoleStatus.ClientID%>').val($("#<%=ddlRoleStatus.ClientID%> option:first").val());
            } catch (ex) { }
        }

        function resetRoleShowAll() {
            var ddlSearch = $('#<%=ddlSearchRole.ClientID%>');
            var txtSearch = document.getElementById('<%=div_txtSearchRole.ClientID%>');
            var rbStatus = document.getElementById('<%=div_ddlRoleStatus.ClientID%>');

            txtSearch.style.display = "block";
            rbStatus.style.display = "none";

            try {
                $('#<%=ddlRoleStatus.ClientID%>').val($("#<%=ddlRoleStatus.ClientID%> option:first").val());
            } catch (ex) { }
        }
        
    </script>
</asp:Content>

