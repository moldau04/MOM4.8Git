<%@ Page Title="Vendors || MOM" Language="C#" MasterPageFile="~/Mom.master" AutoEventWireup="true" Inherits="Vendors" Codebehind="Vendors.aspx.cs" EnableEventValidation="false" %>

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
                                    <div class="page-title"><i class="mdi-communication-contacts"></i>&nbsp;Vendors</div>
                                    <asp:Panel runat="server" ID="pnlGridButtons">
                                        <div class="buttonContainer">
                                            <div class="btnlinks">
                                                <asp:LinkButton ID="lnkAddnew" runat="server" OnClientClick='return AddVendorsClick(this)' OnClick="lnkAddnew_Click">Add</asp:LinkButton>
                                            </div>
                                            <div class="btnlinks">
                                                <asp:LinkButton ID="btnEdit" runat="server" OnClientClick='return EditVendorsClick(this)' OnClick="btnEdit_Click">Edit</asp:LinkButton>
                                            </div>
                                            <div class="btnlinks menuAction">
                                                <a onclick="DropdownMenu()" class="dropbtn" style="cursor: pointer;">Actions
                                                </a>
                                            </div>

                                            <ul id="drpMenu" class="nomgn hideMenu menuList">
                                                 <li>
                                                    <div class="btnlinks">
                                                        <a id="btnCopy" runat="server" onclientclick='return AddVendorsClick(this)' onserverclick="btnCopy_Click">Copy
                                                        </a>
                                                    </div>
                                                </li>
                                                <li>
                                                    <div class="btnlinks">
                                                        <asp:LinkButton ID="lnkDelete" runat="server" OnClientClick='return DeleteVendorsClick(this)' OnClick="lnkDelete_Click">Delete</asp:LinkButton>
                                                    </div>
                                                </li>
                                                <li>
                                                    <div class="btnlinks">
                                                        <asp:LinkButton ID="lnkExcel" runat="server" OnClick="lnkExcel_Click">Export to Excel</asp:LinkButton>
                                                    </div>
                                                </li>
                                                <li>
                                                    <div class="btnlinks">
                                                        <a class="dropdown-button" data-beloworigin="true" data-activates="dropdown1">Reports
                                                        </a>
                                                    </div>
                                                    <ul id="dropdown1" class="dropdown-content">
                                                        <li>
                                                            <%--<a href="VendorListReport.aspx?type=Vendor" class="-text">Add New Report</a>--%>
                                                        </li>
                                                        <li>
                                                            <a href="VendorLbl5160.aspx" class="-text">Vendor Label 5160</a>
                                                        </li>
                                                        <li>
                                                            <a href="VendorLbl5163.aspx" class="-text">Vendor Label 5163</a>
                                                        </li>
                                                        <li>
                                                            <a href="Vendor1099.aspx" class="-text">Federal 1099</a>
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
                </header>
            </div>
        </div>
    </div>
    <div class="container">
        <div class="row">
            <div class="srchpane">
                <asp:UpdatePanel ID="upPannelSearch" runat="server" UpdateMode="Conditional">

                    <ContentTemplate>
                        <div class="srchtitle ser-css2" style="padding-left: 15px;">
                            Search
                        </div>
                        <div class="srchinputwrap">
                            <asp:DropDownList ID="ddlSearch" runat="server" class="browser-default selectst selectsml" AutoPostBack="true" OnSelectedIndexChanged="ddlSearch_SelectedIndexChanged">
                            </asp:DropDownList>
                            <%--<asp:DropDownList ID="ddlSearch" runat="server" class="browser-default selectst selectsml" onchange="jsFunction(this.value);">
                            </asp:DropDownList>--%>
                        </div>
                        <div class="srchinputwrap">
                            <asp:DropDownList ID="ddlType" runat="server" CssClass="browser-default selectst" Visible="false" >
                                <%--<asp:ListItem Value="Cost Of Sales">Cost Of Sales</asp:ListItem>
                                <asp:ListItem Value="Overhead">Overhead</asp:ListItem>--%>
                            </asp:DropDownList>
                        </div>
                        <div class="srchinputwrap">
                            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="browser-default selectst" Visible="false" >
                                <asp:ListItem Value="Active">Active</asp:ListItem>
                                <asp:ListItem Value="InActive">InActive</asp:ListItem>
                                <asp:ListItem Value="Hold">Hold</asp:ListItem>
                            </asp:DropDownList>
                        </div>

                        <div class="srchinputwrap">
                            <asp:TextBox ID="txtSearch" runat="server" CssClass="srchcstm" placeholder="Search..." ></asp:TextBox>
                        </div>
                        <div class="srchinputwrap srchclr btnlinksicon">

                            <asp:LinkButton ID="lnkSearch" runat="server" OnClick="lnkSearch_Click"><i class="mdi-action-search"></i>
                            </asp:LinkButton>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <div class="col lblsz2 lblszfloat">
                    <div class="row">
                        <span class="tro trost">
                            <asp:CheckBox ID="lnkChk" runat="server" OnCheckedChanged="lnkchk_Click" AutoPostBack="True" CssClass="css-checkbox" Text="Incl. Inactive"></asp:CheckBox>
                        </span>
                        <span class="tro trost">
                            <asp:LinkButton ID="lnkShowAll" runat="server" OnClick="lnkShowAll_Click">Show All </asp:LinkButton>
                        </span>
                        <span class="tro trost">
                            <asp:LinkButton ID="lnkClear" runat="server" OnClick="lnkClear_Click">Clear</asp:LinkButton>
                        </span>
                        <span class="tro trost">
                            <%--<asp:UpdatePanel ID="updpnl" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>--%>
                                    <asp:Label ID="lblRecordCount" runat="server">0 Record(s) found.</asp:Label>
                                <%--</ContentTemplate>
                            </asp:UpdatePanel>--%>
                        </span>
                    </div>
                </div>
            </div>
        </div>

        <div class="grid_container">
            <div class="form-section-row m-b-0" style="margin-bottom: 0 !important;">

                <telerik:RadAjaxManager ID="RadAjaxManager_Vendor" runat="server">
                    <AjaxSettings>
                        <telerik:AjaxSetting AjaxControlID="lnkDelete">
                            <UpdatedControls>
                                <telerik:AjaxUpdatedControl ControlID="RadGrid_Vendor" LoadingPanelID="RadAjaxLoadingPanel_Vendor" />
                                <telerik:AjaxUpdatedControl ControlID="lblRecordCount"  />
                            </UpdatedControls>
                        </telerik:AjaxSetting>
                        <telerik:AjaxSetting AjaxControlID="lnkSearch">
                            <UpdatedControls>
                                <telerik:AjaxUpdatedControl ControlID="RadGrid_Vendor" LoadingPanelID="RadAjaxLoadingPanel_Vendor" />
                                <telerik:AjaxUpdatedControl ControlID="lblRecordCount"  />
                                
                            </UpdatedControls>
                        </telerik:AjaxSetting>
                        <telerik:AjaxSetting AjaxControlID="lnkChk">
                            <UpdatedControls>
                                <telerik:AjaxUpdatedControl ControlID="RadGrid_Vendor" LoadingPanelID="RadAjaxLoadingPanel_Vendor" />
                                <telerik:AjaxUpdatedControl ControlID="lblRecordCount"  />
                            </UpdatedControls>
                        </telerik:AjaxSetting>
                        <telerik:AjaxSetting AjaxControlID="lnkShowAll">
                            <UpdatedControls>
                                <telerik:AjaxUpdatedControl ControlID="RadGrid_Vendor" LoadingPanelID="RadAjaxLoadingPanel_Vendor" />
                                <telerik:AjaxUpdatedControl ControlID="lblRecordCount"  />
                            </UpdatedControls>
                        </telerik:AjaxSetting>
                        <telerik:AjaxSetting AjaxControlID="RadGrid_Vendor">
                            <UpdatedControls>   
                                <telerik:AjaxUpdatedControl ControlID="RadGrid_Vendor" LoadingPanelID="RadAjaxLoadingPanel_Vendor" />
                                <telerik:AjaxUpdatedControl ControlID="lblRecordCount"  />
                            </UpdatedControls>
                        </telerik:AjaxSetting>
                        <%--<telerik:AjaxSetting AjaxControlID="ddlSearch">
                            <UpdatedControls>
                                <telerik:AjaxUpdatedControl ControlID="RadGrid_Vendor" LoadingPanelID="RadAjaxLoadingPanel_Vendor" />
                                <telerik:AjaxUpdatedControl ControlID="ddlType"  />
                                <telerik:AjaxUpdatedControl ControlID="ddlStatus"  />
                                <telerik:AjaxUpdatedControl ControlID="txtSearch"  />
                                
                                
                            </UpdatedControls>
                        </telerik:AjaxSetting>--%>
                        
                        <%--<telerik:AjaxSetting AjaxControlID="lnkClear">
                            <UpdatedControls>
                                <telerik:AjaxUpdatedControl ControlID="RadGrid_Vendor" LoadingPanelID="RadAjaxLoadingPanel_Vendor" />
                            </UpdatedControls>
                        </telerik:AjaxSetting>--%>
                    </AjaxSettings>
                </telerik:RadAjaxManager>
                <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel_Vendor" runat="server">
                </telerik:RadAjaxLoadingPanel>

                <div class="RadGrid RadGrid_Material">
                    <telerik:RadCodeBlock ID="codeBlock1" runat="server">
                        <script type="text/javascript">
                            function pageLoad() {
                                var grid = $find("<%= RadGrid_Vendor.ClientID %>");
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
                    <telerik:RadAjaxPanel ID="RadAjaxPanel_Vendor" runat="server" LoadingPanelID="RadAjaxLoadingPanel_Vendor" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">

                        <telerik:RadGrid RenderMode="Auto" ID="RadGrid_Vendor" AllowFilteringByColumn="true" ShowFooter="True" PageSize="50"
                            ShowStatusBar="true" runat="server" AllowPaging="True" AllowSorting="true" Width="100%" PagerStyle-AlwaysVisible="true" FilterType="CheckList"
                            AllowCustomPaging="True" OnNeedDataSource="RadGrid_Vendor_NeedDataSource" OnItemCreated="RadGrid_Vendor_ItemCreated" OnItemEvent="RadGrid_Vendor_ItemEvent" OnExcelMLExportRowCreated="RadGrid_Vendor_ExcelMLExportRowCreated" OnPreRender="RadGrid_Vendor_PreRender"   >
                            <CommandItemStyle />
                            <GroupingSettings CaseSensitive="false" />
                            <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                                <Selecting AllowRowSelect="True"></Selecting>

                                <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                            </ClientSettings>
                            <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="True" ShowFooter="True">
                                <Columns>
                                    <telerik:GridTemplateColumn AllowFiltering="false" ShowFilterIcon="false" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblId" runat="server" Text='<%# Bind("ID") %>'></asp:Label>
                                            <asp:Label ID="lblRol" runat="server" Text='<%# Bind("Rol") %>'></asp:Label>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridClientSelectColumn UniqueName="chkSelect" HeaderStyle-Width="40">
                                    </telerik:GridClientSelectColumn>
                                    <telerik:GridTemplateColumn DataField="Acct" HeaderText="ID" SortExpression="Acct" UniqueName="Acct"
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                        ShowFilterIcon="false" HeaderStyle-Width="200" >
                                        <ItemTemplate>
                                            <asp:Label ID="lblAcct" runat="server" Text='<%# Eval("Acct") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label runat="server" Text="Total :-"></asp:Label>
                                        </FooterTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn SortExpression="Name" AutoPostBackOnFilter="true" DataField="Name" UniqueName="Name"
                                        CurrentFilterFunction="Contains" HeaderText="Name" ShowFilterIcon="false" HeaderStyle-Width="200">
                                        <ItemTemplate>
                                            <asp:HyperLink ID="lnkName" runat="server" Text='<%# Bind("Name") %>'></asp:HyperLink>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridBoundColumn DataField="Address" HeaderText="Address" SortExpression="Address"
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false" HeaderStyle-Width="140">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Contact" HeaderText="Contact" SortExpression="Contact"
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false" HeaderStyle-Width="100">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Phone" HeaderText="Phone#" SortExpression="Phone"
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false" HeaderStyle-Width="100">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Email" HeaderText="Email" SortExpression="Email"
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false" HeaderStyle-Width="140">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Status" HeaderText="Status" SortExpression="Status"
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false" HeaderStyle-Width="70">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Company" HeaderText="Company" SortExpression="Company"
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false" HeaderStyle-Width="100">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Type" HeaderText="Type" SortExpression="Type"
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false" HeaderStyle-Width="100">
                                    </telerik:GridBoundColumn>
                            <%--        <telerik:GridTemplateColumn DataField="Balance" SortExpression="Balance" AutoPostBackOnFilter="true" HeaderText="Balance" ShowFilterIcon="false" UniqueName="Template1">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAmount" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Balance", "{0:c}")%>'
                                                ForeColor='<%# Convert.ToDouble(Eval("Balance"))>=0?System.Drawing.Color.Black:System.Drawing.Color.Red %>'></asp:Label>
                                            
                                        </ItemTemplate>--%>
                                             <%--<FooterTemplate>
                                             <asp:Label ID="lblTotalAmount" runat="server" ></asp:Label>
                                        </FooterTemplate>--%>
                                 <%--   </telerik:GridTemplateColumn>--%>

                                    <telerik:GridTemplateColumn DataField="Balance" FooterAggregateFormatString="{0:c}" Aggregate="Sum" SortExpression="Balance" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" HeaderText="Balance" ShowFilterIcon="false" HeaderStyle-Width="90">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAmount" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Balance", "{0:c}")%>'
                                                ForeColor='<%# Convert.ToDouble(Eval("Balance"))>=0?System.Drawing.Color.Black:System.Drawing.Color.Red %>'></asp:Label>
                                            
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                      <telerik:GridBoundColumn DataField="City" HeaderText="City" SortExpression="City"
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false" HeaderStyle-Width="110">
                                    </telerik:GridBoundColumn>
                                         <telerik:GridBoundColumn DataField="State" HeaderText="State" SortExpression="State"
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false" HeaderStyle-Width="100">
                                    </telerik:GridBoundColumn>
                                       <telerik:GridBoundColumn DataField="Zip" HeaderText="Zip" SortExpression="Zip"
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false" HeaderStyle-Width="100">
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

    <!-- Hidden Field -->
    <asp:HiddenField runat="server" ID="hdnAddVendors" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnEditVendors" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnDeleteVendors" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnViewVendors" Value="Y" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="javascriptHolder" runat="Server">
    <script type="text/javascript">
        function AddVendorsClick(hyperlink) {
            var id = document.getElementById('<%= hdnAddVendors.ClientID%>').value;
            if (id == "Y") {
                return true;
            } else {
                noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                return false;
            }
        }
        function EditVendorsClick(hyperlink) {

            var id = document.getElementById('<%= hdnEditVendors.ClientID%>').value;
            if (id == "Y") { return true; } else {
                noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                return false;
            }
        }
        function DeleteVendorsClick(hyperlink) {
            var id = document.getElementById('<%= hdnDeleteVendors.ClientID%>').value;
            if (id == "Y") {
                return SelectedRowDelete('<%= RadGrid_Vendor.ClientID%>', 'Vendor');
            } else {
                noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                return false;
            }
        }

        function clear() {
            ("#ddlType");
        }
        function jsFunction(value) {
            alert(value);

            if (value == "Rol.Type") {
                //$(txtSearch).val("");
                //$('#searchField').attr("value", "");
                document.getElementById('<%=txtSearch.ClientID%>').value = '';
                document.getElementById('<%=ddlType.ClientID%>').style.display = 'block';  
                document.getElementById('<%=ddlStatus.ClientID%>').style.display = 'none';  
                document.getElementById('<%=txtSearch.ClientID%>').style.display = 'none';  
                
            }
            else if (value == "Vendor.Status") {
                //$(txtSearch).val("");
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

        }
        
    </script>


    <script type="text/javascript">
        jQuery(document).ready(function () {
            $('#colorNav #dynamicUI li').remove();
            $(reports).each(function (index, report) {
                var imagePath = null;
                if (report.IsGlobal == true) {
                    imagePath = "images/globe.png";
                }
                else {
                    imagePath = "images/blog_private.png";
                }

                $('#dynamicUI').append('<li><a href="VendorListReport.aspx?reportId=' + report.ReportId + '&reportName=' + report.ReportName + '&type=Vendor"><span><img src=images/reportfolder.png> ' + report.ReportName + '</span><div style="clear:both;"></div></a></li>')

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
</asp:Content>


