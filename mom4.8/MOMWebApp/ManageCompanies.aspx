<%@ Page Title="" Language="C#" MasterPageFile="~/Mom.master" EnableEventValidation="false" AutoEventWireup="true" Inherits="ManageCompanies" Codebehind="ManageCompanies.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <!--Grid Control-->
    <link href="Design/css/grid.css" rel="stylesheet" />
    <!--File Upload Control-->
    <link href="Design/js/plugins/dropify/css/dropify.css" type="text/css" rel="stylesheet" media="screen,projection">

    <link href="Design/css/pikaday.css" rel="stylesheet" />

    <script type="text/javascript">
        function AddCompanyClick(hyperlink) {
            var id = document.getElementById('<%= hdnAddeOwner.ClientID%>').value;
            if (id == "Y") {
                return true;
            } else {
                noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                return false;
            }
        }
        function EditCompanyClick(hyperlink) {

            var id = document.getElementById('<%= hdnEditeOwner.ClientID%>').value;
            if (id == "Y") { return true; } else {
                noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                return false;
            }
        }
        function DeleteCompanyClick(hyperlink) {
            var id = document.getElementById('<%= hdnDeleteeOwner.ClientID%>').value;
            if (id == "Y") {
                return SelectedRowDelete('<%= gvUsers.ClientID%>', 'Company');
            } else {
                noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                return false;
            }
        }
        function CopyCompanyClick(hyperlink) {
            var id = document.getElementById('<%= hdnAddeOwner.ClientID%>').value;
            if (id == "Y") {
                return true;
            } else {
                noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                return false;
            }
        }
        function SearchClick() {
            var searchBy = document.getElementById('<%= ddlSearch.ClientID%>').value;
            var searchText = document.getElementById('<%= txtSearch.ClientID%>').value;
            if (searchBy == "Select" && (searchText != "" && searchText != undefined && searchText != null)) {                
                noty({ text: 'Please select a field to search', type: 'warning', layout: 'topCenter', closeOnSelfClick: false, timeout: 1000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                return false;
            } else {                
                return true;
            }
        }
    </script>
    <style type="text/css">
        .gvUsers [id$='_PageSizeComboBox'] {
            width: 6.1em !important;
        }
        .grid-select{
            border-bottom:solid 1px #000 !important;
            margin-bottom:10px !important;
        }
        .grid-select .rcbInner{
            border-color:#fff !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div style="height: 65px !important;">
        <div id="divButtons">
            <div id="breadcrumbs-wrapper">
                <header>
                    <div class="container row-color-grey">
                        <div class="row">
                            <div class="col s12 m12 l12">
                                <div class="row">
                                    <div class="page-title"><i class="mdi-communication-business"></i>&nbsp;Manage Companies</div>
                                    <div class="btnlinks">
                                        <asp:HyperLink ID="lnkAddnew" Text="Add" runat="server" onclick='return AddCompanyClick(this)' NavigateUrl="~/AddManageCompanies.aspx"></asp:HyperLink>
                                    </div>
                                    <div class="btnlinks">
                                        <div id="liEditCompany" runat="server">
                                            <asp:LinkButton CssClass="icon-edit" ID="btnEdit" runat="server" OnClientClick='return EditCompanyClick(this)' Text="Edit" OnClick="btnSubmit_Click"></asp:LinkButton>

                                        </div>
                                        <div id="liEditOffice" runat="server" visible="false">
                                            <asp:LinkButton CssClass="icon-edit" ID="btnEditOffice" runat="server" Text="Edit" OnClick="btnSubmitOffice_Click"></asp:LinkButton>

                                        </div>

                                    </div>
                                    <div class="btnlinks">
                                        <asp:LinkButton CssClass="icon-copy" ID="btnCopy" runat="server" OnClientClick='return AddCompanyClick(this)' ToolTip="Copy" Text="Copy" OnClick="btnCopy_Click"></asp:LinkButton>
                                    </div>

                                    <div class="rght-content">
                                        <div class="btnclosewrap">
                                            <asp:LinkButton ID="lnkClose" runat="server" CausesValidation="false" OnClick="lnkClose_Click">  <i class="mdi-content-clear"></i></asp:LinkButton>
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
            <div class="srchpane">
                <asp:UpdatePanel runat="server" ID="pnlHeader" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="srchtitle srchtitlecustomwidth" style="padding-left: 15px;">
                            Search
                        </div>
                        <div class="srchinputwrap pd-negatenw input-field">
                            <asp:DropDownList ID="ddlSearch" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlSearch_SelectedIndexChanged"
                                CssClass="browser-default selectsml selectst">
                                <asp:ListItem>Select</asp:ListItem>
                                <asp:ListItem Value="B.ID">Company ID</asp:ListItem>
                                <asp:ListItem Value="B.Name">Name</asp:ListItem>
                                <asp:ListItem Value="BR.ID" Enabled="false">Office ID</asp:ListItem>
                                <asp:ListItem Value="BR.Name" Enabled="false">Office Name</asp:ListItem>
                                <asp:ListItem Value="B.Status">Status</asp:ListItem>
                                <asp:ListItem Value="B.Manager">Manager</asp:ListItem>
                                <asp:ListItem Value="B.Address">Address</asp:ListItem>
                                <asp:ListItem Value="B.City">City</asp:ListItem>
                                <asp:ListItem Value="B.Phone">Phone</asp:ListItem>
                                <asp:ListItem Value="B.Zip">Zip Code</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="srchinputwrap">
                            <asp:TextBox ID="txtSearch" runat="server" CssClass="srchcstm"></asp:TextBox>                                      
                        </div>
                        <div class="srchinputwrap">
                            <asp:DropDownList ID="rbStatus" runat="server" CssClass="browser-default selectst selectsml" Visible="False">                                
                                <asp:ListItem Value="0">Active</asp:ListItem>
                                <asp:ListItem Value="1">Closed</asp:ListItem>
                                <asp:ListItem Value="2">Hold</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="srchinputwrap srchclr btnlinksicon">
                            <asp:LinkButton ID="lnkSearch" runat="server" CausesValidation="false" OnClientClick="SearchClick()"
                                OnClick="lnkSearch_Click"><i class="mdi-action-search"></i></asp:LinkButton>
                        </div>
                        <asp:DropDownList ID="ddlUserType" runat="server" CssClass="srchcstm"
                                Visible="False">
                            </asp:DropDownList>
                    </ContentTemplate>
                </asp:UpdatePanel>


                <div class="col lblsz2 lblszfloat">
                    <div class="row">

                        <span class="tro trost">

                            <asp:Label ID="lblChkSelect" runat="server" CssClass="title-check-text" For="lnkChk">
                                <asp:CheckBox ID="lnkChk" runat="server" CssClass="filled-in" OnCheckedChanged="lnkchk_Click" AutoPostBack="True"></asp:CheckBox>
                                Incl. Inactive</asp:Label>
                        </span>
                        <span class="tro trost">
                            <asp:LinkButton ID="lnkShowAll" runat="server" OnClick="lnkShowAll_Click">Show All </asp:LinkButton>
                        </span>
                        <span class="tro trost">
                            <asp:LinkButton ID="lnkClear" runat="server" OnClick="lnkClear_Click">Clear</asp:LinkButton>
                        </span>
                        <span class="tro trost">
                            <asp:UpdatePanel runat="server" ID="pnlRecordCount" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Label ID="lblRecordCount" runat="server"></asp:Label>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </span>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="grid_container">
                <div class="form-section-row" style="margin-bottom: 0 !important;">
                    <telerik:RadAjaxManager ID="RadAjaxManager_User" runat="server">
                        <AjaxSettings>
                            <telerik:AjaxSetting AjaxControlID="lnkSearch">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="gvUsers" LoadingPanelID="RadAjaxLoadingPanel_User" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>

                            <telerik:AjaxSetting AjaxControlID="lnkClear">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="gvUsers" LoadingPanelID="RadAjaxLoadingPanel_User" />
                                    <telerik:AjaxUpdatedControl ControlID="pnlHeader" LoadingPanelID="RadAjaxLoadingPanel_User" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>


                            <telerik:AjaxSetting AjaxControlID="lnkShowAll">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="gvUsers" LoadingPanelID="RadAjaxLoadingPanel_User" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                            <%--<telerik:AjaxSetting AjaxControlID="btnDelete">
                                    <UpdatedControls>
                                        <telerik:AjaxUpdatedControl ControlID="radWindowDelete" LoadingPanelID="RadAjaxLoadingPanel_User" />
                                    </UpdatedControls>
                                </telerik:AjaxSetting>
                                <telerik:AjaxSetting AjaxControlID="btnOk">
                                    <UpdatedControls>
                                        <telerik:AjaxUpdatedControl ControlID="gvUsers" LoadingPanelID="RadAjaxLoadingPanel_User" />
                                    </UpdatedControls>
                                </telerik:AjaxSetting>
                                <telerik:AjaxSetting AjaxControlID="lnkProcess">
                                    <UpdatedControls>
                                        <telerik:AjaxUpdatedControl ControlID="gvUsers" LoadingPanelID="RadAjaxLoadingPanel_User" />
                                    </UpdatedControls>
                                </telerik:AjaxSetting>
                                <telerik:AjaxSetting AjaxControlID="lnkSearch">
                                    <UpdatedControls>
                                        <telerik:AjaxUpdatedControl ControlID="gvUsers" LoadingPanelID="RadAjaxLoadingPanel_User" />                                        
                                    </UpdatedControls>
                                </telerik:AjaxSetting>--%>
                        </AjaxSettings>
                    </telerik:RadAjaxManager>
                    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel_User" runat="server">
                    </telerik:RadAjaxLoadingPanel>
                    <div class="RadGrid RadGrid_Material">
                        <telerik:RadCodeBlock ID="codeBlock1" runat="server">
                            <script type="text/javascript">
                                function pageLoad() {
                                    var grid = $find("<%= gvUsers.ClientID %>");
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
                        <telerik:RadAjaxPanel ID="RadAjaxPanel_User" runat="server" LoadingPanelID="RadAjaxLoadingPanel_User" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">
                            <telerik:RadPersistenceManager ID="RadPersistenceManager1" runat="server">
                                <PersistenceSettings>
                                    <telerik:PersistenceSetting ControlID="gvUsers" />
                                </PersistenceSettings>
                            </telerik:RadPersistenceManager>

                            <telerik:RadGrid RenderMode="Auto" ID="gvUsers" runat="server" AutoGenerateColumns="False" Width="100%" FilterType="CheckList"
                                PageSize="50" CssClass="table table-bordered table-striped table-condensed flip-content gvUsers" AllowSorting="True"
                                ShowFooter="True" PagerStyle-AlwaysVisible="true" ShowStatusBar="true"
                                OnNeedDataSource="gvUsers_NeedDataSource"
                                OnPreRender="gvUsers_PreRender"
                                OnItemCreated="gvUsers_ItemCreated"
                                AllowPaging="true"
                                MasterTableView-CanRetrieveAllData="false">
                                <PagerStyle AlwaysVisible="true" Mode="NextPrevAndNumeric" />
                                <CommandItemStyle />
                                <GroupingSettings CaseSensitive="false" />
                                <ActiveItemStyle CssClass="evenrowcolor" />
                                <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                                    <Selecting AllowRowSelect="True"></Selecting>
                                    <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                    <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                </ClientSettings>
                                <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="True" ShowFooter="True">
                                    <Columns>
                                        <telerik:GridClientSelectColumn UniqueName="chkSelect" HeaderStyle-Width="40">
                                        </telerik:GridClientSelectColumn>
                                        <telerik:GridTemplateColumn AllowFiltering="false" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hdnSelected" runat="server" />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn HeaderText="ID" Visible="False">
                                            <ItemTemplate>
                                                <asp:Label ID="lblId" runat="server" Text='<%# Bind("ID") %>'></asp:Label>
                                                <asp:HyperLink ID="lnkId" runat="server"></asp:HyperLink>
                                                <%--<asp:HiddenField ID="hdnSelected" runat="server" /> --%>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn HeaderText="CompanyID" SortExpression="ID" DataField="ID" UniqueName="ID" HeaderStyle-Width="125px" ShowFilterIcon="false" CurrentFilterFunction="EqualTo" FilterDelay="5">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCompanyID" runat="server" Text='<%# Bind("ID") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn HeaderText="Company Name" UniqueName="Name" DataField="Name" SortExpression="Name" FilterDelay="5"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblName" runat="server"><%#Eval("Name")%></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn HeaderText="Type" UniqueName="OType" DataField="OType" SortExpression="OType" FilterDelay="5"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblType" runat="server"><%#Eval("OType")%></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn HeaderText="Address" UniqueName="Address" DataField="Address" SortExpression="address" FilterDelay="5" ShowFilterIcon="false" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAddress" runat="server"><%#Eval("Address")%></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn HeaderText="City" UniqueName="City" DataField="City" SortExpression="City" ShowFilterIcon="false" FilterDelay="5" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCity" runat="server"><%#Eval("City")%></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn HeaderText="Phone" SortExpression="Phone" DataField="Phone" UniqueName="Phone" ShowFilterIcon="false" FilterDelay="5" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPhone" runat="server"><%#Eval("Phone")%></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn HeaderText="Zip Code" SortExpression="Zip" DataField="Zip" UniqueName="Zip" ShowFilterIcon="false" FilterDelay="5" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                            <ItemTemplate>
                                                <asp:Label ID="lblZip" runat="server"><%#Eval("Zip")%></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Manager" SortExpression="Manager" DataField="Manager" UniqueName="Manager" ShowFilterIcon="false" FilterDelay="5" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                            <ItemTemplate>
                                                <asp:Label ID="lblManager" runat="server"><%#Eval("Manager")%></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn HeaderText="Status" SortExpression="status" DataField="status" UniqueName="status" FilterDelay="5" ShowFilterIcon="false" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo">
                                            <ItemTemplate>
                                                <asp:Label ID="lblStatus" runat="server"><%# Convert.ToInt32( Eval("status")) == 0 ? "Active" : Convert.ToInt32( Eval("status")) == 1? "Closed":"Hold"%></asp:Label>
                                            </ItemTemplate>
                                            <FilterTemplate>
                                                <telerik:RadComboBox RenderMode="Auto" CssClass="browser-default selectsml selectst grid-select" ID="ddlFStatus" Width="100%" AppendDataBoundItems="true" SelectedValue='<%# ((GridItem)Container).OwnerTableView.GetColumn("status").CurrentFilterValue %>'
                                                    runat="server" OnClientSelectedIndexChanged="CountryIndexChanged">
                                                    <Items>
                                                        <telerik:RadComboBoxItem Text="All" />
                                                        <telerik:RadComboBoxItem Value="0" Text="Active" />
                                                        <telerik:RadComboBoxItem Value="1" Text="Closed" />
                                                        <telerik:RadComboBoxItem Value="2" Text="Hold" />
                                                    </Items>
                                                </telerik:RadComboBox>
                                                
                                                <telerik:RadScriptBlock ID="RadScriptBlock3" runat="server">
                                                    <script type="text/javascript">
                                                        function CountryIndexChanged(sender, args) {
                                                            var tableView = $find("<%# ((GridItem)Container).OwnerTableView.ClientID %>");
                                                            tableView.filter("status", args.get_item().get_value(), "EqualTo");
                                                        }
                                                    </script>
                                                </telerik:RadScriptBlock>
                                            </FilterTemplate>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn Visible="false" HeaderText="# No. of Offices" SortExpression="NoOfOffices" DataField="NoOfOffices" UniqueName="NoOfOffices" ShowFilterIcon="false" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" FilterDelay="5">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lblNoOfOffices" runat="server" OnLoad="btnSubmitOffice_Click" OnClick="lnkOffice_Click"><%#Eval("NoOfOffices")%></asp:LinkButton>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>


                                    </Columns>
                                </MasterTableView>
                                <SelectedItemStyle CssClass="selectedrowcolor" />
                                <FilterMenu CssClass="RadFilterMenu_CheckList">
                                </FilterMenu>
                            </telerik:RadGrid>

                            <telerik:RadGrid RenderMode="Auto" Visible="false" ID="gvOffice" runat="server" AutoGenerateColumns="False" Width="100%" FilterType="CheckList"
                                PageSize="50" CssClass="table table-bordered table-striped table-condensed flip-content gvUsers" AllowSorting="True" OnSorting="gvOffice_Sorting"
                                ShowFooter="True" PagerStyle-AlwaysVisible="true" ShowStatusBar="true"
                                OnNeedDataSource="gvOffice_NeedDataSource"
                                AllowPaging="true"
                                MasterTableView-CanRetrieveAllData="false">
                                <PagerStyle AlwaysVisible="true" Mode="NextPrevAndNumeric" />
                                <CommandItemStyle />
                                <GroupingSettings CaseSensitive="false" />
                                <ActiveItemStyle CssClass="evenrowcolor" />
                                <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                                    <Selecting AllowRowSelect="True"></Selecting>
                                    <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                    <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                </ClientSettings>
                                <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="True" ShowFooter="True">
                                    <Columns>
                                        <telerik:GridTemplateColumn AllowFiltering="false" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hdnSelected" runat="server" />
                                                <asp:CheckBox ID="chkSelect" runat="server" />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn HeaderText="ID" Visible="False">
                                            <ItemTemplate>
                                                <asp:Label ID="lblId" runat="server" Text='<%# Bind("ID") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn HeaderText="CompanyID" SortExpression="CompanyID" DataField="CompanyID" UniqueName="CompanyID" HeaderStyle-Width="125px" ShowFilterIcon="false" CurrentFilterFunction="Contains" FilterDelay="5">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCompanyID" runat="server" Text='<%# Bind("CompanyID") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn HeaderText="Company Name" UniqueName="CompanyName" DataField="CompanyName" SortExpression="CompanyName" FilterDelay="5"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCompanyName" runat="server"><%#Eval("CompanyName")%></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn HeaderText="OfficeID" UniqueName="ID" DataField="ID" SortExpression="ID" FilterDelay="5"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblOfficeID" runat="server" Text='<%# Bind("ID") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn HeaderText="Office Name" UniqueName="Name" DataField="Name" SortExpression="Name" FilterDelay="5" ShowFilterIcon="false" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                            <ItemTemplate>
                                                <asp:Label ID="lblName" runat="server"><%#Eval("Name")%></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn HeaderText="Type" UniqueName="OType" DataField="OType" SortExpression="OType" ShowFilterIcon="false" FilterDelay="5" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                            <ItemTemplate>
                                                <asp:Label ID="lblType" runat="server"><%#Eval("OType")%></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn HeaderText="Address" SortExpression="Address" DataField="Address" UniqueName="Address" ShowFilterIcon="false" FilterDelay="5" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAddress" runat="server"><%#Eval("Address")%></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn HeaderText="City" SortExpression="City" DataField="City" UniqueName="City" ShowFilterIcon="false" FilterDelay="5" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCity" runat="server"><%#Eval("City")%></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn HeaderText="Phone" SortExpression="Phone" DataField="Phone" UniqueName="Phone" ShowFilterIcon="false" FilterDelay="5" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPhone" runat="server"><%#Eval("Phone")%></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn HeaderText="Zip Code" SortExpression="Zip" DataField="Zip" UniqueName="Zip" FilterDelay="5" ShowFilterIcon="false" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo">
                                            <ItemTemplate>
                                                <asp:Label ID="lblZipGd" runat="server"><%#Eval("Zip")%></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn HeaderText="Manager" SortExpression="Manager" DataField="Manager" UniqueName="Manager" FilterDelay="5" ShowFilterIcon="false" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo">
                                            <ItemTemplate>
                                                <asp:Label ID="lblManager" runat="server"><%#Eval("Manager")%></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn HeaderText="Status" SortExpression="status" DataField="status" UniqueName="status" FilterDelay="5" ShowFilterIcon="false" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo">
                                            <ItemTemplate>
                                                <asp:Label ID="lblStatus" runat="server"><%# Convert.ToInt32( Eval("status")) == 0 ? "Active" : "Closed"%></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </telerik:GridTemplateColumn>




                                    </Columns>
                                </MasterTableView>
                                <SelectedItemStyle CssClass="selectedrowcolor" />
                                <FilterMenu CssClass="RadFilterMenu_CheckList">
                                </FilterMenu>
                            </telerik:RadGrid>
                        </telerik:RadAjaxPanel>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField runat="server" ID="hdnAddeOwner" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnEditeOwner" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnDeleteeOwner" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnViewOwner" Value="Y" />

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="javascriptHolder" runat="Server">
    <script src="Design/js/moment.js"></script>
    <script src="Design/js/pikaday.js"></script>
    <script type="text/javascript">
        function SwitchIncChk() {
            var elm = document.getElementById('<%=lnkChk.ClientID%>');
            if (true != elm.checked) {
                elm.click();
            }
        }
        $(document).ready(function () {

            $('a[href^="#accrd"]').on('click', function (e) {
                e.preventDefault();

                var target = this.hash;
                var $target = $(target);
                if ($(target).hasClass('active') || target == "") {

                }
                else {
                    $(target).click();
                }

                $('html, body').stop().animate({
                    'scrollTop': $target.offset().top - 125
                }, 900, 'swing');
            });


            var picker = new Pikaday(
                {
                    field: document.getElementById('datepicker'),
                    firstDay: 0,
                    format: 'MM/DD/YYYY',
                    minDate: new Date(2000, 1, 1),
                    maxDate: new Date(2020, 12, 31),
                    yearRange: [2000, 2020]
                });

            var picker = new Pikaday(
                {
                    field: document.getElementById('datepicker2'),
                    firstDay: 0,
                    format: 'MM/DD/YYYY',
                    minDate: new Date(2000, 1, 1),
                    maxDate: new Date(2020, 12, 31),
                    yearRange: [2000, 2020]
                });

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


