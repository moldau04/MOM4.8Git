<%@ Page Title="" Language="C#" MasterPageFile="~/Mom.master" AutoEventWireup="true" Inherits="AddInventoryWarehouse" Codebehind="AddInventoryWarehouse.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <!--Grid Control-->
    <link href="Design/css/grid.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <telerik:RadAjaxManager ID="RadAjaxManager_Warehouse" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="lnkSaveWarehouselocation">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_WarehouseLocation" LoadingPanelID="RadAjaxLoadingPanel_Warehouse" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="lnkDeleteWarehouseLocation">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_WarehouseLocation" LoadingPanelID="RadAjaxLoadingPanel_Warehouse" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>

    <telerik:RadWindowManager ID="RadWindowManager1" runat="server">
        <Windows>
            <telerik:RadWindow ID="CompanyWindow" Skin="Material" VisibleTitlebar="true" Behaviors="Default" CenterIfModal="true"
                Animation="FlyIn" AnimationDuration="200" RenderMode="Auto" VisibleStatusbar="false" MinWidth="450"
                runat="server" Modal="true" Width="450" Height="185">
                <ContentTemplate>
                    <div style="margin-top: 15px;">
                        <div class="input-field col s12">
                            <div class="row">
                                <label class="drpdwn-label">Select Company</label>
                                <asp:DropDownList ID="ddlCompanyEdit" runat="server" CssClass="browser-default">
                                </asp:DropDownList>
                            </div>
                        </div>


                        <div style="clear: both;"></div>

                        <footer style="float: left; padding-left: 0 !important;">
                            <div class="btnlinks">
                                <asp:LinkButton ID="lnkCompanyEdit" runat="server" OnClick="btnCompanyEdit_Click">Save</asp:LinkButton>
                            </div>
                        </footer>
                    </div>
                </ContentTemplate>

            </telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>
    <telerik:RadWindowManager ID="RadWindowManager2" runat="server">
        <Windows>
            <telerik:RadWindow ID="WarehouseLocationWindow" Skin="Material" VisibleTitlebar="true" Behaviors="Default" CenterIfModal="true"
                Animation="FlyIn" AnimationDuration="200" RenderMode="Auto" VisibleStatusbar="false" MinWidth="450"
                runat="server" Modal="true" Width="450" Height="185">
                <ContentTemplate>
                    <div style="margin-top: 15px;">
                        <div class="input-field col s12">
                            <div class="row">
                                <label for="txtLocationName">Location</label>
                                <asp:TextBox ID="txtLocationName" EnableViewState="true" ViewStateMode="Enabled" runat="server" MaxLength="25"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rqLocationName" runat="server" ControlToValidate="txtLocationName"
                                    Display="None" ErrorMessage="Location Required" SetFocusOnError="True" ValidationGroup="invware"></asp:RequiredFieldValidator>
                                <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender19" runat="server" Enabled="True"
                                    TargetControlID="rqLocationName">
                                </asp:ValidatorCalloutExtender>
                            </div>
                        </div>


                        <div style="clear: both;"></div>

                        <footer style="float: left; padding-left: 0 !important;">
                            <div class="btnlinks">
                                <asp:LinkButton ID="lnkSaveWarehouselocation" runat="server" OnClick="lnkSaveWarehouselocation_Click" ValidationGroup="invware">Save</asp:LinkButton>
                            </div>
                        </footer>
                    </div>
                </ContentTemplate>

            </telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>

    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel_Warehouse" runat="server">
    </telerik:RadAjaxLoadingPanel>
    <div style="height: 65px !important;">
        <div id="divButtons">
            <div id="breadcrumbs-wrapper">
                <header>
                    <div class="container row-color-grey">
                        <div class="row">
                            <div class="col s12 m12 l12">
                                <div class="row">
                                    <div class="page-title"><i class="mdi-social-location-city"></i>&nbsp;<asp:Label ID="lblHeader" runat="server">Add Warehouse</asp:Label></div>
                                    <div class="btnlinks">
                                        <asp:LinkButton ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" ValidationGroup="general, rep">Save</asp:LinkButton>
                                    </div>
                                    <div class="rght-content">
                                        <div class="btnclosewrap">
                                            <asp:LinkButton ID="lnkClose" runat="server" CausesValidation="false" OnClick="lnkClose_Click"><i class="mdi-content-clear"></i></asp:LinkButton>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </header>
            </div>
            <div class="container breadcrumbs-bg-custom">
                <div class="row">
                    <div class="col s12 m12 l12">
                        <div class="row">
                            <div class="tblnks">
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="container">
        <div class="row">
            <div class="card cardradius cardnegate">
                <div class="card-content">
                    <div class="form-content-wrap">
                        <div class="form-content-pd">
                            <div class="form-section-row">
                                <div class="form-sectioncustom1">
                                    <div class="section-ttle">Warehouse Details</div>
                                    <div class="form-section2">
                                        <div class="input-field col s12">
                                            <div class="row">
                                                <label for="txtWarehouseID">ID</label>
                                                <asp:TextBox ID="txtWarehouseID" runat="server" CssClass="validate" MaxLength="10"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="reqWarehouseID" runat="server"
                                                    ControlToValidate="txtWarehouseID" Display="None" ErrorMessage="ID Required" SetFocusOnError="True" ValidationGroup="general, rep"></asp:RequiredFieldValidator>
                                                <asp:ValidatorCalloutExtender ID="RequiredFieldValidator1_ValidatorCalloutExtender" runat="server"
                                                    Enabled="True" TargetControlID="reqWarehouseID">
                                                </asp:ValidatorCalloutExtender>
                                            </div>
                                        </div>
                                        <div class="input-field col s12">
                                            <div class="row">
                                                <label class="drpdwn-label">Type</label>
                                                <asp:RequiredFieldValidator ID="reqddlType" runat="server"
                                                    ControlToValidate="ddlType" Display="None" ErrorMessage="Type Required" InitialValue="select" SetFocusOnError="True" ValidationGroup="general, rep"></asp:RequiredFieldValidator>
                                                <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender2" runat="server"
                                                    Enabled="True" TargetControlID="reqddlType">
                                                </asp:ValidatorCalloutExtender>
                                                <asp:DropDownList ID="ddlType" runat="server" CssClass="browser-default" AutoPostBack="true" OnSelectedIndexChanged="ddlType_SelectedIndexChanged">
                                                    <asp:ListItem Value="select">Select</asp:ListItem>
                                                    <asp:ListItem Value="0">Truck/Employee</asp:ListItem>
                                                    <asp:ListItem Value="1">Location</asp:ListItem>
                                                    <asp:ListItem Value="2">Office</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>

                                        <div class="input-field col s12" id="TruckEmployeeDiv" runat="server" visible="false">
                                            <div class="row">
                                                <label class="drpdwn-label">Truck/Employee</label>
                                                <asp:RequiredFieldValidator ID="rqddlTruckEmployee" runat="server"
                                                    ControlToValidate="ddlTruckEmployee" Display="None" InitialValue="0" ErrorMessage="Location Required" SetFocusOnError="True" ValidationGroup="general, rep"></asp:RequiredFieldValidator>
                                                <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender3" runat="server"
                                                    Enabled="True" TargetControlID="rqddlTruckEmployee">
                                                </asp:ValidatorCalloutExtender>
                                                <asp:DropDownList ID="ddlTruckEmployee" runat="server" CssClass="browser-default"></asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="input-field col s12" id="LocationDiv" runat="server" visible="false">
                                            <div class="row">
                                                <label class="drpdwn-label">Location</label>
                                                <asp:RequiredFieldValidator ID="rqddlLocation" runat="server"
                                                    ControlToValidate="ddlLocation" Display="None" InitialValue="0" ErrorMessage="Location Required" SetFocusOnError="True" ValidationGroup="general, rep"></asp:RequiredFieldValidator>
                                                <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtenderhg" runat="server"
                                                    Enabled="True" TargetControlID="rqddlLocation">
                                                </asp:ValidatorCalloutExtender>
                                                <asp:DropDownList ID="ddlLocation" runat="server" CssClass="browser-default"></asp:DropDownList>
                                            </div>
                                        </div>


                                        
                                        <div class="input-field col s12">
                                            <div class="row">
                                                <label class="drpdwn-label">Status</label>
                                               <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                                                    ControlToValidate="ddlStatus" Display="None" ErrorMessage="Status Required" InitialValue="select" SetFocusOnError="True" ValidationGroup="general, rep"></asp:RequiredFieldValidator>
                                                <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender4" runat="server"
                                                    Enabled="True" TargetControlID="RequiredFieldValidator1">
                                                </asp:ValidatorCalloutExtender>--%>
                                                <asp:DropDownList ID="ddlStatus" runat="server" CssClass="browser-default">
                                                   <%-- <asp:ListItem Value="select">Select</asp:ListItem>--%>
                                                    <asp:ListItem Value="0"  >Active</asp:ListItem>
                                                    <asp:ListItem Value="1">Inactive</asp:ListItem>                                                    
                                                </asp:DropDownList>
                                            </div>
                                        </div>

                                        <div class="input-field col s12">
                                            <div class="checkrow">
                                                <asp:CheckBox ID="chkMultiSelect" runat="server" CssClass="css-checkbox" Visible="false" Text="Multi-Stocking" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-section3-blank">
                                        &nbsp;
                                    </div>
                                    <div class="form-section2">
                                        <div class="input-field col s12">
                                            <div class="row">
                                                <label for="txtRemarks">Remarks</label>
                                                <asp:TextBox ID="txtRemarks" runat="server" class="materialize-textarea" TextMode="MultiLine"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="input-field col s12">
                                            <div class="row">
                                                <label for="txtWarehouseName">Warehouse Name</label>
                                                <asp:TextBox ID="txtWarehouseName" runat="server" MaxLength="25"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="reqWarehouseName" runat="server"
                                                    ControlToValidate="txtWarehouseName" Display="None" ErrorMessage="Name Required" SetFocusOnError="True" ValidationGroup="general, rep"></asp:RequiredFieldValidator>
                                                <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender1" runat="server"
                                                    Enabled="True" TargetControlID="reqWarehouseName">
                                                </asp:ValidatorCalloutExtender>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="form-sectioncustom2" runat="server" id="WarehouselocationDiv" visible="false">
                                    <div class="section-ttle">
                                        Warehouse Locations
                                        <span style="float: right; margin-top: -14px;">
                                            <div class="btnlinksicon mgnleftneg">
                                                <asp:LinkButton ID="lnkAddWarehouseLocation" ToolTip="Add" runat="server" OnClientClick="OpenWarehouseLocationWindow();return false"><i class="mdi-content-add"></i></asp:LinkButton>
                                            </div>
                                            <div class="btnlinksicon mgnleftneg">
                                                <asp:LinkButton ID="lnkEditWarehouseLocation" ToolTip="Edit" runat="server" OnClientClick="OpenWarehouseLocationEdit(this);return false"><i class="mdi-content-create"></i></asp:LinkButton>
                                            </div>
                                            <div class="btnlinksicon mgnleftneg">
                                                <asp:LinkButton ID="lnkDeleteWarehouseLocation" ToolTip="Delete" runat="server" OnClick="lnkDeleteWarehouseLocation_Click" OnClientClick="return confirm('Do you really want to delete this Warehouse ?');"><i class="mdi-action-delete"></i></asp:LinkButton>
                                            </div>
                                        </span>
                                    </div>
                                    <div class="col s12">

                                        <div class="grid_container">
                                            <div class="form-section-row" style="margin-bottom: 0 !important;">
                                                <div class="RadGrid RadGrid_Material FormGrid containedGrid">
                                                    <telerik:RadCodeBlock ID="RadCodeBlock22" runat="server">
                                                        <script type="text/javascript">
                                                            function pageLoad() {
                                                                var grid = $find("<%= RadGrid_WarehouseLocation.ClientID %>");
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
                                                    <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel_Warehouse" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">
                                                        <telerik:RadGrid RenderMode="Auto" ID="RadGrid_WarehouseLocation" AllowFilteringByColumn="true" ShowFooter="True" PageSize="50"
                                                            OnNeedDataSource="RadGrid_WarehouseLocation_NeedDataSource" PagerStyle-AlwaysVisible="false"
                                                            ShowStatusBar="true" runat="server" AllowPaging="false" AllowSorting="true" Width="100%" AllowCustomPaging="False">
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
                                                                    <telerik:GridTemplateColumn UniqueName="lblId" FilterDelay="5" DataField="ID" HeaderText="ID" SortExpression="ID"
                                                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblId" runat="server" Text='<%# Eval("ID") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn UniqueName="lblLocationName" FilterDelay="5" DataField="Name" HeaderText="Location Name" SortExpression="Name"
                                                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblLocationName" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>

                                                                </Columns>
                                                            </MasterTableView>
                                                        </telerik:RadGrid>
                                                    </telerik:RadAjaxPanel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                            </div>
                            <div class="form-section-row" id="dvCompanyPermission" runat="server">
                                <div class="form-section9">
                                    <div class="form-section3half">
                                        <div>
                                            <div class="input-field col s12">
                                                <div class="row">
                                                    <asp:Label ID="lblCompany" runat="server" AssociatedControlID="ddlCompany" CssClass="drpdwn-label">Company</asp:Label>
                                                    <asp:DropDownList ID="ddlCompany" runat="server" CssClass="browser-default"></asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-section3half-blank">
                                    </div>
                                    <div class="form-section3half">
                                        <div class="input-field col s12">
                                            <div class="row">
                                                <asp:TextBox ID="txtCompany" runat="server" Enabled="false"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="srchclr btnlinksicon rowbtn">
                                            <asp:HyperLink ID="btnCompanyPopUp" runat="server" Style="cursor: pointer;" onclick="OpenCompanyPopUp(this);" ToolTip="Change Company"><i class="mdi-action-autorenew"></i></asp:HyperLink>

                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="cf"></div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField runat="server" ID="hdnEditeOwner" Value="Y" />
    <asp:HiddenField ID="hdnAddEditWarehouse" runat="server" Value="0" />
    <asp:HiddenField ID="hdnWHLocId" runat="server" Value="0" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="javascriptHolder" runat="Server">

    <script type="text/javascript">
        $(window).load(function () {
        //$(document).ready(function () {            
            var valueFun = '';
            var pageURL = window.location.search.substring(1);
            var urlQS = pageURL.split('&');
            
            for (var i = 0; i < urlQS.length; i++) {
                var paramName = urlQS[i].split('=');
                if (paramName[0] == 'Fun') {
                    //replace the special char like "+","&" etc from value
                    var valueFun = paramName[1];
                    if (valueFun != 'OFC') {
                        noty({
                            text: 'Warehouse ' + valueFun + ' successfully',
                            type: 'success',
                            layout: 'topCenter',
                            closeOnSelfClick: false,
                            timeout: 5000,
                            theme: 'noty_theme_default',
                            closable: true
                        });
                    }
                    //if (valueFun != 'Added' || valueFun != 'Updated') {
                    else {
                        noty({
                            text: 'This is a default warehouse OFC and cannot be made inactive.',
                            type: 'warning',
                            layout: 'topCenter',
                            closeOnSelfClick: false,
                            timeout: 5000,
                            theme: 'noty_theme_default',
                            closable: true
                        });
                    }                  
                }             
            }          
        });


    </script>

    <script type="text/javascript">
        function OpenCompanyPopUp() {
            var wnd = $find('<%=CompanyWindow.ClientID %>');
            wnd.set_title("Select Company");
            wnd.Show();
        }
        function CloseCompanyPopUp() {
            var wnd = $find('<%=CompanyWindow.ClientID %>');
            wnd.Close();
        }
        function OpenWarehouseLocationWindow() {
            $('#<%=txtLocationName.ClientID%>').val("");
            $('#<%=hdnAddEditWarehouse.ClientID%>').val("0");

            var wnd = $find('<%=WarehouseLocationWindow.ClientID %>');
            wnd.set_title("Add Warehouse Location");
            wnd.Show();
        }
        function CloseWarehouseLocationWindow() {
            var wnd = $find('<%=WarehouseLocationWindow.ClientID %>');
            wnd.Close();
        }
        function OpenWarehouseLocationEdit() {
            var grid = $find("<%=RadGrid_WarehouseLocation.ClientID %>");
            var MasterTable = grid.get_masterTableView();
            var selectedRows = MasterTable.get_selectedItems();
            var ID = "";
            var Location = "";
            for (var i = 0; i < selectedRows.length; i++) {
                var row = selectedRows[i];
                ID = MasterTable.getCellByColumnUniqueName(row, "lblId").innerHTML;
                ID = $(ID).html();
                Location = MasterTable.getCellByColumnUniqueName(row, "lblLocationName").innerHTML.replace(/&nbsp;/g, '');
                Location = $(Location).html();
            }
            if (ID != "") {
                $('#<%=hdnWHLocId.ClientID%>').val(ID);
                $('#<%=txtLocationName.ClientID%>').val(Location);
                $('#<%=hdnAddEditWarehouse.ClientID%>').val("1");
                var wnd = $find('<%=WarehouseLocationWindow.ClientID %>');
                wnd.set_title("Edit Warehouse Location");

                wnd.Show();
                Materialize.updateTextFields();
            }
            else {
                ChkWarning();
            }
        }
        function ChkWarning() {
            noty({
                text: 'Please select any one to edit.',
                type: 'warning',
                layout: 'topCenter',
                closeOnSelfClick: false,
                timeout: 5000,
                theme: 'noty_theme_default',
                closable: true
            });
            return false;
        }
    </script>
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
        $(document).ready(function () {
            $('[id*=chkMultiSelect]').change(function () {
                var chk = this.checked
                if (chk == true) {
                    $('[id*=WarehouselocationDiv]').css("display", "inline");
                }
                else {
                    $('[id*=WarehouselocationDiv]').css("display", "none");
                }

            });
        });
    </script>
</asp:Content>

