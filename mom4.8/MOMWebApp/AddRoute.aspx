<%@ Page Title="" Language="C#" MasterPageFile="~/MOMRadWindow.Master" AutoEventWireup="true" Inherits="AddRoute" CodeBehind="AddRoute.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <!--Grid Control-->
    <link href="Design/css/grid.css" rel="stylesheet" />
    <style>
        a[disabled=disabled] {
            color: gray;
        }

        .form-section3 {
            margin-right: 10px;
        }

        select {
            margin-top: 8px;
        }

        .rcpPalette {
            position: fixed !important;
        }
    .card {
        min-height: 100% !important;
    }

        .card{
            overflow: unset;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script src="js/jscolor.js"></script>
    <asp:HiddenField ID="hdnDefaultVal" runat="server" />
    <div class="divbutton-container">
        <div id="divButtons">
            <div id="breadcrumbs-wrapper">
                <header>
                    <div class="container row-color-grey">
                        <div class="row">
                            <div class="col s12 m12 l12">
                                <div class="row">
                                    <div class="page-title"><i class="mdi-communication-contacts"></i>&nbsp;<asp:Label runat="server" ID="lblHeader">Route</asp:Label></div>
                                    <div class="buttonContainer">
                                        <div class="btnlinks">
                                            <asp:LinkButton ID="lnkSave" runat="server" ToolTip="Save" Text="Save" OnClick="lnkSave_Click"></asp:LinkButton>
                                        </div>
                                    </div>
                                    <div class="rght-content">
                                        <div class="editlabel">
                                            <span id="">
                                                <asp:Label runat="server" ID="lblRouteWorkerName"></asp:Label></span>
                                        </div>
                                        <div class="btnclosewrap">
                                            <asp:LinkButton ID="lnkClose" ToolTip="Close" runat="server" CausesValidation="false"
                                                OnClick="lnkClose_Click"><i class="mdi-content-clear"></i></asp:LinkButton>
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
                            <div class="tblnksright">
                                <div id="ctl00_ContentPlaceHolder1_divNavigate" class="nextprev" style="display: block;">
                                    <span class="angleicons">
                                        <asp:LinkButton ID="lnkFirst" ToolTip="First" runat="server" CausesValidation="False"
                                            OnClick="lnkFirst_Click">
                                                <i class="fa fa-angle-double-left"></i>
                                        </asp:LinkButton>
                                    </span>
                                    <span class="angleicons">
                                        <asp:LinkButton ID="lnkPrevious" ToolTip="Previous" runat="server" CausesValidation="False"
                                            OnClick="lnkPrevious_Click">
                                                <i class="fa fa-angle-left"></i>
                                        </asp:LinkButton>
                                    </span>
                                    <span class="angleicons">
                                        <asp:LinkButton ID="lnkNext" ToolTip="Next" runat="server" CausesValidation="False"
                                            OnClick="lnkNext_Click">
                                                <i class="fa fa-angle-right"></i>
                                        </asp:LinkButton>
                                    </span>
                                    <span class="angleicons">
                                        <asp:LinkButton ID="lnkLast" ToolTip="Last" runat="server" CausesValidation="False"
                                            OnClick="lnkLast_Click">
                                                <i class="fa fa-angle-double-right"></i>
                                        </asp:LinkButton>
                                    </span>
                                </div>
                            </div>
                        </div>
                    </div>

                </div>
            </div>
        </div>
    </div>
    <div class="container">
        <div class="row">
            <div class="card cardradius ">
                <div class="card-content" style="min-height:300px;">
                    <div class="form-content-wrap">
                        <div class="form-content-pd">
                            <div class="section-ttle" id="dvRoutesInfo" runat="server">Route Info</div>
                            <div class="form-section3">
                                <div class="input-field col s12">
                                    <div class="row">
                                        <label for="name">Name <span class="reqd">*</span></label>
                                        <asp:TextBox ID="txtName" runat="server" AutoCompleteType="None" CssClass="form-control"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                                            ControlToValidate="txtName" Display="None" ErrorMessage="Name Required" SetFocusOnError="True">
                                        </asp:RequiredFieldValidator>

                                        <asp:ValidatorCalloutExtender
                                            ID="ValidatorCalloutExtender1" runat="server" Enabled="True"
                                            TargetControlID="RequiredFieldValidator1">
                                        </asp:ValidatorCalloutExtender>
                                    </div>
                                </div>
                                <div class="input-field col s12">
                                    <div class="row">
                                        <label class="drpdwn-label">Worker <span class="reqd">*</span></label>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server"
                                            ControlToValidate="ddlRoute" Display="None" ErrorMessage="Worker Required" SetFocusOnError="True">
                                        </asp:RequiredFieldValidator>
                                        <asp:ValidatorCalloutExtender
                                            ID="RequiredFieldValidator9_ValidatorCalloutExtender" runat="server" Enabled="True"
                                            TargetControlID="RequiredFieldValidator9">
                                        </asp:ValidatorCalloutExtender>
                                        <asp:DropDownList ID="ddlRoute" runat="server" CssClass="browser-default" TabIndex="5">
                                        </asp:DropDownList>
                                    </div>
                                </div>

                                <div class="input-field col s12">
                                    <div class="row">
                                        <label class="drpdwn-label">Status <span class="reqd">*</span></label>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                                            ControlToValidate="ddlStatus" Display="None" ErrorMessage="Status Required" SetFocusOnError="True">
                                        </asp:RequiredFieldValidator>
                                        <asp:ValidatorCalloutExtender
                                            ID="RequiredFieldValidator2_Extender" runat="server" Enabled="True"
                                            TargetControlID="RequiredFieldValidator2">
                                        </asp:ValidatorCalloutExtender>
                                        <asp:DropDownList ID="ddlStatus" runat="server" CssClass="browser-default" TabIndex="5">
                                            <asp:ListItem Value="" Text=":: Select ::"></asp:ListItem>
                                            <asp:ListItem Value="1" Text="Active"></asp:ListItem>
                                            <asp:ListItem Value="0" Text="Inactive"></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:HiddenField ID="hdnStatus" runat="server" Value="" />
                                    </div>
                                </div>
                            </div>
                            <div class="form-section3">
                                <div class="input-field col s12">
                                    <div class="row">
                                        <label for="remarks">Remarks</label>
                                        <asp:TextBox ID="txtremarks" runat="server" CssClass="materialize-textarea"
                                            Height="50px" MaxLength="200" TextMode="MultiLine"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="form-section3" style="margin-left: 5px; margin-top: -12px">
                                <div class="col s12">
                                    <div class="row">
                                        <label class="drpdwn-label">Color</label>
                                        <telerik:RadColorPicker RenderMode="Auto" EnableCustomColor="false" runat="server" ID="txtColor" Preset="Office" ShowIcon="true" PaletteModes="HSB"
                                            KeepInScreenBounds="true">
                                        </telerik:RadColorPicker>
                                    </div>
                                </div>
                                <div id="dvLocCount">
                                    <div class="input-field col s12" style="margin-top: 6px">
                                        <div class="row">
                                            <label>Location</label>
                                            <asp:TextBox ID="lblLocCount" runat="server" ReadOnly="true" CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="input-field col s12">
                                        <div class="row">
                                            <label>Contract</label>
                                            <asp:TextBox ID="lblContCount" runat="server" ReadOnly="true" CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div id="tbLogs" runat="server" visible="false" class="collapsible-header accrd accordian-text-custom" style="padding: 0px;">
                <i class="mdi-content-content-paste"></i>Logs
                <div class="grid_container">
                    <div class="form-section-row" style="margin-bottom: 0 !important;">
                        <div class="RadGrid RadGrid_Material">
                            <telerik:RadCodeBlock ID="RadCodeBlock6" runat="server">
                                <script type="text/javascript">
                                    function pageLoad() {
                                        try {
                                            var grid = $find("<%= RadGrid_gvLogs.ClientID %>");
                                            var columns = grid.get_masterTableView().get_columns();
                                            for (var i = 0; i < columns.length; i++) {
                                                columns[i].resizeToFit(false, true);
                                            }
                                        } catch (e) {

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
                            <telerik:RadAjaxPanel ID="RadAjaxPanel_gvLogs" runat="server" LoadingPanelID="RadAjaxLoadingPanel_gvLogs" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">
                                <telerik:RadGrid RenderMode="Auto" ID="RadGrid_gvLogs" AllowFilteringByColumn="true" ShowFooter="True" PageSize="50" PagerStyle-AlwaysVisible="true" OnItemCreated="RadGrid_gvLogs_ItemCreated"
                                    ShowStatusBar="true" runat="server" AllowPaging="True" AllowSorting="true" Width="100%" FilterType="CheckList" AllowCustomPaging="True" OnNeedDataSource="RadGrid_gvLogs_NeedDataSource">
                                    <CommandItemStyle />
                                    <GroupingSettings CaseSensitive="false" />
                                    <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                                        <Selecting AllowRowSelect="True"></Selecting>
                                        <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                        <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                    </ClientSettings>
                                    <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="True" ShowFooter="True" AllowNaturalSort="false" DataKeyNames="fUser">
                                        <Columns>
                                            <telerik:GridTemplateColumn DataField="CreatedStamp" SortExpression="CreatedStamp" AutoPostBackOnFilter="true" DataType="System.String"
                                                CurrentFilterFunction="Contains" HeaderText="Date" ShowFilterIcon="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbldate" runat="server" Text='<%# Eval("CreatedStamp", "{0:M/d/yyyy}")%>'></asp:Label>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn DataField="CreatedStamp" SortExpression="CreatedStamp" AutoPostBackOnFilter="true" DataType="System.String"
                                                CurrentFilterFunction="Contains" HeaderText="Time" ShowFilterIcon="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbltime" runat="server" Text='<%# Eval("CreatedStamp","{0: hh:mm tt}") %>'></asp:Label>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn DataField="fUser" SortExpression="fUser" AutoPostBackOnFilter="true"
                                                CurrentFilterFunction="Contains" HeaderText="User" ShowFilterIcon="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblUpdBy" runat="server" Text='<%# Eval("fUser") %>'></asp:Label>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn DataField="Field" SortExpression="Field" AutoPostBackOnFilter="true"
                                                CurrentFilterFunction="Contains" HeaderText="Field" ShowFilterIcon="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblField" runat="server" Text='<%# Eval("field") %>'></asp:Label>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn DataField="OldVal" SortExpression="OldVal" AutoPostBackOnFilter="true"
                                                CurrentFilterFunction="Contains" HeaderText="Old Value" ShowFilterIcon="false" HeaderStyle-Width="305">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblOldVal" runat="server" Text='<%# Eval("OldVal") %>'></asp:Label>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn DataField="NewVal" SortExpression="NewVal" AutoPostBackOnFilter="true"
                                                CurrentFilterFunction="Contains" HeaderText="New Value" ShowFilterIcon="false" HeaderStyle-Width="305">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblNewVal" runat="server" Text='<%# Eval("NewVal") %>'></asp:Label>
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
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="javascriptHolder" runat="Server">
    <script type="text/javascript">
        $(document).ready(function () {

            $('#<%=hdnStatus.ClientID %>').val($('#<%=ddlStatus.ClientID %>').val());

            $('#<%=ddlStatus.ClientID %>').change(function () {
                $('#<%=hdnStatus.ClientID %>').val($(this).val());
            });

            if ($('#<%=lblLocCount.ClientID %>').val() == '') {
                $('#dvLocCount').html('');
            }

            //$(window).scroll(function () {
            //    if ($(window).scrollTop() >= 0) {
            //        $('#divButtons').addClass('fixed-header');
            //    }
            //    if ($(window).scrollTop() <= 0) {
            //        $('#divButtons').removeClass('fixed-header');
            //    }
            //});
            $("#content").attr('style', 'margin-left:-170px;');
            $('#divButtons').attr('style', 'left:0px');
        });
    </script>
</asp:Content>
