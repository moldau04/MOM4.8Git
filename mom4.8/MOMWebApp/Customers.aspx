<%@ Page Title="Customers || MOM" Language="C#" MasterPageFile="~/Mom.master" AutoEventWireup="true" Inherits="NewCustomers" EnableViewState="true" CodeBehind="Customers.aspx.cs" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        .selectedna v {
            background-color: #1565C0;
            color: #fff;
        }

        [id$='PageSizeComboBox'] {
            width: 5.1em !important;
        }
    </style>
    <link href="Design/css/grid.css" rel="stylesheet" />
    <script type="text/javascript">
        function AddCustomerClick(hyperlink) {
            i
            var id = document.getElementById('<%= hdnAddeOwner.ClientID%>').value;
            if (id == "Y") {
                return true;
            } else {
                noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                return false;
            }
        }
        function EditCustomerClick(hyperlink) {

            var id = document.getElementById('<%= hdnEditeOwner.ClientID%>').value;
            if (id == "Y") { return true; } else {
                noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                return false;
            }
        }
        function DeleteCustomerClick(hyperlink) {
            var id = document.getElementById('<%= hdnDeleteeOwner.ClientID%>').value;
            if (id == "Y") {
                return SelectedRowDelete('<%= RadGrid_Customer.ClientID%>', 'customer');
            } else {
                noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                return false;
            }
        }
        function CopyCustomerClick(hyperlink) {
            var id = document.getElementById('<%= hdnAddeOwner.ClientID%>').value;
            if (id == "Y") {
                return true;
            } else {
                noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                return false;
            }
        }
        $(document).keypress(function (e) {
            if (e.keyCode === 13) {
                e.preventDefault();
                return false;
            }
        });


    </script>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div>
        <div class="divbutton-container">
            <div id="divButtons">
                <div id="breadcrumbs-wrapper">

                    <header>
                        <div class="container row-color-grey">
                            <div class="row">
                                <div class="col s12 m12 l12">
                                    <div class="row">

                                        <div class="page-title"><i class="mdi-social-people"></i>&nbsp;Customers</div>
                                        <div class="buttonContainer">

                                            <div class="btnlinks">
                                                <a id="lnkAddnew" runat="server" onclick='return AddCustomerClick(this)' href="addcustomer.aspx">Add
                                                </a>
                                            </div>
                                            <div class="btnlinks">
                                                <a id="btnEdit" runat="server" onclientclick='return EditCustomerClick(this)' onserverclick="btnSubmit_Click">Edit
                                                </a>
                                            </div>

                                            <div class="btnlinks menuAction">
                                                <a onclick="DropdownMenu()" class="dropbtn" style="cursor: pointer;">Actions
                                                </a>
                                            </div>

                                            <ul id="drpMenu" class="nomgn hideMenu menuList">
                                                <li>
                                                    <div class="btnlinks">
                                                        <a id="btnCopy" runat="server" onclientclick='return AddCustomerClick(this)' onserverclick="btnCopy_Click">Copy
                                                        </a>
                                                    </div>
                                                </li>
                                                <li>
                                                    <div class="btnlinks">
                                                        <a id="btnDelete" onclientclick='return DeleteCustomerClick(this)' runat="server" onserverclick="btnDelete_Click">Delete
                                                        </a>
                                                    </div>
                                                </li>
                                                <li>
                                                    <div class="btnlinks">
                                                        <asp:LinkButton ID="btnExcel" runat="server" OnClick="btnExcel_Click">Export to Excel</asp:LinkButton>

                                                    </div>
                                                </li>
                                                <li>
                                                    <ul id="dropdown1" class="dropdown-content">
                                                        <li>
                                                            <a href="CustomersReport.aspx?type=Customer" class="-text">Add New</a>
                                                        </li>
                                                        <li>
                                                            <a href="CustomerLabel5160.aspx" class="-text">Customer Label 5160</a>
                                                        </li>
                                                        <%--                                                        <li>
                                                            <asp:LinkButton ID="lnkCustomerLabel5160" OnClick="lnkCustomerLabel5160_Click" runat="server">Customer Label 5160
                                                            </asp:LinkButton>
                                                        </li>--%>
                                                        <li>
                                                            <a href="CustomerLabel5163.aspx" class="-text">Customer Label 5163</a>
                                                        </li>
                                                        <li>
                                                            <asp:LinkButton ID="lnkCustomerCategoryDetail" OnClick="lnkCustomerCategoryDetail_Click" runat="server">Customer Ticket Category by Last Service Date
                                                            </asp:LinkButton>
                                                        </li>
                                                    </ul>
                                                    <div class="btnlinks">
                                                        <a class="dropdown-button" data-beloworigin="true" href="#!" data-activates="dropdown1">Reports
                                                        </a>
                                                    </div>
                                                </li>
                                                <li>
                                                    <div class="btnlinks">
                                                        <asp:LinkButton ID="lnkSyncQB" runat="server" OnClick="lnkSyncQB_Click"
                                                            Visible="False">Sync with QB</asp:LinkButton>
                                                    </div>
                                                </li>
                                            </ul>
                                        </div>
                                        <div class="btnclosewrap">
                                            <asp:LinkButton ID="lnkClose" runat="server" ToolTip="Close" CausesValidation="false" OnClick="lnkClose_Click">
                                                <i class="mdi-content-clear"></i> </asp:LinkButton>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </header>
                </div>
            </div>
        </div>
    </div>
    <div class="container">
        <div class="row">

            <div class="srchpane">
                <%--<asp:UpdatePanel runat="server" ID="udpSearch" UpdateMode="Conditional">
                    <ContentTemplate>--%>
                <div class="srchtitle" >
                    Search
                </div>
                <div class="srchinputwrap">
                    <asp:DropDownList ID="ddlSearch" runat="server"
                        CssClass="browser-default selectsml selectst" AutoPostBack="True" onchange="showfilter();return false;"
                        OnSelectedIndexChanged="ddlSearch_SelectedIndexChanged">
                        <asp:ListItem>Select</asp:ListItem>
                        <asp:ListItem Value="r.name">Name</asp:ListItem>
                        <asp:ListItem Value="r.Address">Address</asp:ListItem>
                        <asp:ListItem Value="o.Status">Status</asp:ListItem>
                        <asp:ListItem Value="o.type">Type</asp:ListItem>
                        <asp:ListItem Value="r.City">City</asp:ListItem>
                        <asp:ListItem Value="r.Phone">Phone</asp:ListItem>
                        <asp:ListItem Value="Website">Website</asp:ListItem>
                        <asp:ListItem Value="Email">Email</asp:ListItem>
                        <asp:ListItem Value="Cellular">Cellular</asp:ListItem>
                        <asp:ListItem Value="sageid">Sage ID</asp:ListItem>
                        <asp:ListItem Value="r.Zip">Zip Code</asp:ListItem>
                        <asp:ListItem Value="B.Name">Company</asp:ListItem>
                        <asp:ListItem Value="r.State">State</asp:ListItem>
                        <asp:ListItem Value="o.Custom1">Custom 1</asp:ListItem>
                        <asp:ListItem Value="o.Custom2">Custom 2</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="srchinputwrap" id="div_rbStatus" style="display: none" runat="server">
                    <asp:DropDownList ID="rbStatus" runat="server" CssClass="browser-default selectst" Visible="true">
                    </asp:DropDownList>
                </div>
                <div class="srchinputwrap" id="div_ddlUserType" style="display: none" runat="server">
                    <asp:DropDownList ID="ddlUserType" runat="server" CssClass="browser-default selectst"
                        Visible="true">
                    </asp:DropDownList>
                </div>
                <div class="srchinputwrap" id="div_ddlCompany" style="display: none" runat="server">
                    <asp:DropDownList ID="ddlCompany" runat="server" CssClass="browser-default selectst"
                        Visible="true">
                    </asp:DropDownList>
                </div>
                <div class="srchinputwrap" id="div_txtSearch" style="display: block" runat="server">
                    <asp:TextBox ID="txtSearch" runat="server" CssClass="srchcstm" placeholder="Search..."></asp:TextBox>
                </div>
                <div class="srchinputwrap srchclr btnlinksicon" style="margin-left: -10px;">
                    <asp:LinkButton ID="lnkSearch" runat="server" CausesValidation="false"
                        OnClick="lnkSearch_Click">
                                                                <i class="mdi-action-search"></i>
                    </asp:LinkButton>
                    <%-- </ContentTemplate>
                </asp:UpdatePanel>--%>
                </div>

                <div class="col lblsz2 lblszfloat">
                    <div class="row">

                        <span class="tro trost">
                            <asp:CheckBox ID="lnkChk" runat="server" OnCheckedChanged="lnkchk_Click" AutoPostBack="True" CssClass="css-checkbox" Text="Incl. Inactive"></asp:CheckBox>
                        </span>
                        <span class="tro trost">
                            <asp:LinkButton ID="lnkShowAll" runat="server" OnClick="lnkShowAll_Click" OnClientClick="resetShowAll();">Show All </asp:LinkButton>
                        </span>
                        <span class="tro trost">
                            <asp:LinkButton ID="lnkClear" runat="server" OnClick="lnkClear_Click" OnClientClick="resetShowAll();">Clear</asp:LinkButton>
                        </span>

                        <span class="tro trost">

                            <asp:Label ID="lblRecordCount" runat="server"></asp:Label>

                        </span>

                    </div>
                </div>



            </div>
            <div class="grid_container">
                <div class="form-section-row mb" aria-autocomplete="none">


                    <telerik:RadAjaxManager ID="RadAjaxManager_Customer" runat="server">
                        <AjaxSettings>
                            <telerik:AjaxSetting AjaxControlID="lnkChk">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="RadGrid_Customer" LoadingPanelID="RadAjaxLoadingPanel_Customer" />
                                    <telerik:AjaxUpdatedControl ControlID="lblRecordCount" LoadingPanelID="RadAjaxLoadingPanel_Customer" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                            <telerik:AjaxSetting AjaxControlID="lnkSearch">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="RadGrid_Customer" LoadingPanelID="RadAjaxLoadingPanel_Customer" />
                                    <telerik:AjaxUpdatedControl ControlID="lblRecordCount" LoadingPanelID="RadAjaxLoadingPanel_Customer" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                            <telerik:AjaxSetting AjaxControlID="lnkShowAll">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="RadGrid_Customer" LoadingPanelID="RadAjaxLoadingPanel_Customer" />
                                    <telerik:AjaxUpdatedControl ControlID="lblRecordCount" LoadingPanelID="RadAjaxLoadingPanel_Customer" />
                                    <telerik:AjaxUpdatedControl ControlID="lnkChk" LoadingPanelID="RadAjaxLoadingPanel_Customer" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                            <telerik:AjaxSetting AjaxControlID="lnkClear">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="RadGrid_Customer" LoadingPanelID="RadAjaxLoadingPanel_Customer" />
                                    <telerik:AjaxUpdatedControl ControlID="lblRecordCount" LoadingPanelID="RadAjaxLoadingPanel_Customer" />
                                    <telerik:AjaxUpdatedControl ControlID="lnkChk" LoadingPanelID="RadAjaxLoadingPanel_Customer" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>

                            <telerik:AjaxSetting AjaxControlID="RadGrid_Customer">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="lblRecordCount" LoadingPanelID="RadAjaxLoadingPanel_Customer" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>

                        </AjaxSettings>
                    </telerik:RadAjaxManager>
                    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel_Customer" runat="server">
                    </telerik:RadAjaxLoadingPanel>

                    <div class="RadGrid RadGrid_Material">
                        <telerik:RadCodeBlock ID="codeBlock1" runat="server">
                            <script type="text/javascript">
                                function pageLoad() {
                                    var grid = $find("<%= RadGrid_Customer.ClientID %>");
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
                        <telerik:RadAjaxPanel ID="RadAjaxPanel_Customer" runat="server" LoadingPanelID="RadAjaxLoadingPanel_Customer" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">

                            <telerik:RadGrid RenderMode="Auto" ID="RadGrid_Customer" AllowFilteringByColumn="true" ShowFooter="True" PageSize="50" PagerStyle-AlwaysVisible="true"
                                ShowStatusBar="true" runat="server" AllowPaging="True" AllowSorting="true" Width="100%" FilterType="CheckList" OnPreRender="RadGrid_Customer_PreRender" OnItemCreated="RadGrid_Customer_ItemCreated" OnItemCommand="RadGrid_Customer_ItemCommand"
                                AllowCustomPaging="True"
                                OnNeedDataSource="RadGrid_Customer_NeedDataSource"
                                OnExcelMLExportRowCreated="RadGrid_Customer_ExcelMLExportRowCreated"
                                OnFilterCheckListItemsRequested="RadGrid_Customer_FilterCheckListItemsRequested"
                                OnPageIndexChanged="RadGrid_Customer_PageIndexChanged"
                                OnPageSizeChanged="RadGrid_Customer_PageSizeChanged">
                                <CommandItemStyle />
                                <GroupingSettings CaseSensitive="false" />

                                <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                                    <Selecting AllowRowSelect="True"></Selecting>
                                    <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                    <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                </ClientSettings>

                                <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="True" ShowFooter="True" DataKeyNames="Name">
                                    <Columns>

                                        <telerik:GridTemplateColumn Visible="false" AllowFiltering="false" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblId" runat="server" Text='<%# Eval("id") %>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridClientSelectColumn UniqueName="ClientSelectColumn" HeaderStyle-Width="28">
                                        </telerik:GridClientSelectColumn>

                                        <telerik:GridTemplateColumn AllowFiltering="false" ShowFilterIcon="false" HeaderStyle-Width="40" UniqueName="ImageQB">
                                            <ItemTemplate>
                                                <asp:Image ID="imgQB" runat="server" Width="16px" ToolTip="Synced in QB" ImageUrl='<%# Eval("qbcustomerid").ToString() != "" ? "images/qb32.png" : "data:image/png;base64,R0lGODlhAQABAIAAAAAAAP///yH5BAEAAAAALAAAAAABAAEAAAIBRAA7"%>' />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn AllowFiltering="false" ShowFilterIcon="false" HeaderStyle-Width="40" UniqueName="sageid">
                                            <ItemTemplate>
                                                <asp:Image ID="imgsageid" runat="server" Width="16px" ToolTip="Synced in Sage300" ImageUrl='<%# Eval("sageid").ToString() != "" && Eval("SageID").ToString() != "NA" ? "images/sage300.png" : "data:image/png;base64,R0lGODlhAQABAIAAAAAAAP///yH5BAEAAAAALAAAAAABAAEAAAIBRAA7"%>' />
                                                <%#Eval("sageid")%>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn DataField="Name" UniqueName="Name" SortExpression="Name" AutoPostBackOnFilter="true" DataType="System.String"
                                            CurrentFilterFunction="Contains" HeaderText="Name" ShowFilterIcon="false" HeaderStyle-Width="110">
                                            <ItemTemplate>
                                                <asp:HyperLink ID="lnkName" runat="server" Text='<%#Eval("name")%>'></asp:HyperLink>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label runat="server" Text="Total :-"></asp:Label>
                                            </FooterTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn SortExpression="Address" UniqueName="Address" DataField="Address" HeaderText="Address" HeaderStyle-Width="110" DataType="System.String"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAddress" runat="server" Text='<%# Eval("Address") + "-" + Eval("Zip") %>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn DataField="Type" AutoPostBackOnFilter="true" FilterCheckListEnableLoadOnDemand="true" CurrentFilterFunction="Contains"
                                            FilterControlAltText="Filter Type" HeaderText="Type" SortExpression="Type" HeaderStyle-Width="110" UniqueName="Type">
                                            <ItemTemplate>
                                                <asp:Label ID="lblType" runat="server" Text='<%#Eval("Type")%>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn DataField="City" UniqueName="City" HeaderText="City" HeaderStyle-Width="110" DataType="System.String"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="City" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCity" runat="server" Text='<%#Eval("City")%>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn DataField="State" UniqueName="State" HeaderText="State" Visible="false"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="City" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblState" runat="server" Text='<%#Eval("State")%>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn DataField="Zip" AutoPostBackOnFilter="false" AllowFiltering="false" CurrentFilterFunction="Contains" UniqueName="Zip" ShowFilterIcon="false" Visible="false" HeaderText="Zip Code" SortExpression="Zip">
                                            <ItemTemplate>
                                                <asp:Label ID="lblZip" runat="server" Text='<%# Eval("Zip") %>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn DataField="Phone" UniqueName="Phone" HeaderText="Phone" HeaderStyle-Width="170" DataType="System.String"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="Phone" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPhone" runat="server" Text='<%#Eval("Phone")%>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn DataField="Status" UniqueName="Status" HeaderText="Status" HeaderStyle-Width="100" DataType="System.String"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="Status"
                                            ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblStatus" runat="server" Text='<%#Eval("Status")%>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn DataField="Company" UniqueName="Company" HeaderText="Company" HeaderStyle-Width="130"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="Company"
                                            ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCompany" runat="server" Text='<%#Eval("Company")%>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn DataField="loc" SortExpression="loc" UniqueName="loc" HeaderText="Locations" HeaderStyle-Width="70" DataType="System.Int32"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" ShowFilterIcon="false" FooterAggregateFormatString="{0}" Aggregate="Sum">
                                            <ItemTemplate>
                                                <asp:Label ID="lblLocations" runat="server" Text='<%#Eval("loc")%>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn DataField="equip" SortExpression="equip" UniqueName="equip" HeaderText="Equipment" HeaderStyle-Width="75" DataType="System.Int32"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" ShowFilterIcon="false" FooterAggregateFormatString="{0}" Aggregate="Sum">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEquipment" runat="server" Text='<%#Eval("equip")%>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn DataField="ActiveEquip" SortExpression="ActiveEquip" UniqueName="ActiveEquip" HeaderText="Active Equipment" HeaderStyle-Width="75" DataType="System.Int32"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" ShowFilterIcon="false" FooterAggregateFormatString="{0}" Aggregate="Sum">
                                            <ItemTemplate>
                                                <asp:Label ID="lblActiveEquipment" runat="server" Text='<%#Eval("ActiveEquip")%>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn DataField="opencall" SortExpression="opencall" UniqueName="opencall" HeaderText="Tickets" HeaderStyle-Width="60"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" ShowFilterIcon="false" FooterAggregateFormatString="{0}" Aggregate="Sum">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTickets" runat="server" Text='<%#Eval("opencall")%>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn DataField="balance" SortExpression="balance" UniqueName="balance" HeaderStyle-Width="110" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" HeaderText="Balance" ShowFilterIcon="false" FooterAggregateFormatString="{0:c}" Aggregate="Sum">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBalance" runat="server" ForeColor='<%# Convert.ToDouble(Eval("balance"))>=0?System.Drawing.Color.Black:System.Drawing.Color.Red %>' Text='<%# DataBinder.Eval(Container.DataItem, "Balance", "{0:c}")%>'></asp:Label>
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

    <asp:HiddenField runat="server" ID="hdnAddeOwner" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnEditeOwner" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnDeleteeOwner" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnViewOwner" Value="Y" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="javascriptHolder" runat="Server">

    <script>
        $(document).ready(function () {

            $("#<%=RadGrid_Customer.ClientID%>").attr("autocomplete", "off");

            $(window).scroll(function () {
                if ($(window).scrollTop() >= 0) {
                    $('#divButtons').addClass('fixed-header');
                }
                if ($(window).scrollTop() <= 0) {
                    $('#divButtons').removeClass('fixed-header');
                }
            });


            $('.dropdown-button').dropdown({
                inDuration: 300,
                outDuration: 225,
                constrainWidth: false, // Does not change width of dropdown to that of the activator
                hover: true, // Activate on hover
                gutter: 0, // Spacing from edge
                belowOrigin: true, // Displays dropdown below the button
                alignment: 'left', // Displays dropdown with edge aligned to the left of button
                stopPropagation: false // Stops event propagation
            }
            );
        });
    </script>
    <script>
        function showfilter() {

            var ddlSearch = $('#<%=ddlSearch.ClientID%>');


            var div_rbStatus = document.getElementById("<%=div_rbStatus.ClientID%>");
            var div_txtSearch = document.getElementById("<%=div_txtSearch.ClientID%>");
            var div_ddlCompany = document.getElementById("<%=div_ddlCompany.ClientID%>");
            var div_ddlUserType = document.getElementById("<%=div_ddlUserType.ClientID%>");
            var div_ddlUserType = document.getElementById("<%=div_ddlUserType.ClientID%>");

            try {

                $('#<%=rbStatus.ClientID%>').val($("#<%=rbStatus.ClientID%> option:first").val());
                $('#<%=ddlUserType.ClientID%>').val($("#<%=ddlUserType.ClientID%> option:first").val());

            } catch (ex) { }

            div_rbStatus.style.display = "none";
            div_txtSearch.style.display = "none";
            div_ddlCompany.style.display = "none";
            div_ddlUserType.style.display = "none";

            switch (String(ddlSearch.val())) {
                case "o.Status":
                    div_rbStatus.style.display = "block";
                    break;
                case "o.type":
                    div_ddlUserType.style.display = "block";
                    break;
                case "B.Name":
                    div_ddlCompany.style.display = "block";
                    break;

                default:
                    div_txtSearch.style.display = "block";
                    var txt = $('#<%=txtSearch.ClientID%>');
                    txt.val("");
                    break;
            }
        }

        function resetShowAll() {

            try {

                $('#<%=rbStatus.ClientID%>').val($("#<%=rbStatus.ClientID%> option:first").val());
                $('#<%=ddlUserType.ClientID%>').val($("#<%=ddlUserType.ClientID%> option:first").val());

            } catch (ex) { }

            var ddlSearch = $('#<%=ddlSearch.ClientID%>');
            ddlSearch.val("Select");
            var txt = $('#<%=txtSearch.ClientID%>');
            txt.val("");
            var rbStatus = document.getElementById("<%=div_rbStatus.ClientID%>");
            var txtSearch = document.getElementById("<%=div_txtSearch.ClientID%>");
            var ddlCompany = document.getElementById("<%=div_ddlCompany.ClientID%>");
            var ddlUserType = document.getElementById("<%=div_ddlUserType.ClientID%>");

            rbStatus.style.display = "none";
            ddlCompany.style.display = "none";
            ddlUserType.style.display = "none";
            txtSearch.style.display = "block";
        }
    </script>
</asp:Content>
