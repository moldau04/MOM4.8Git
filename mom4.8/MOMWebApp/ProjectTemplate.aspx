<%@ Page Title="Templates || MOM" Language="C#" MasterPageFile="~/Mom.master" AutoEventWireup="true" Inherits="ProjectTemplate" Codebehind="ProjectTemplate.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <!--Grid Control-->
    <link href="Design/css/grid.css" rel="stylesheet" />
    <link href="Design/css/pikaday.css" rel="stylesheet" />
    <%--Receive Payment GRID--%>

    <script type="text/javascript">
        function AddPTempClick(hyperlink) {
            var id = document.getElementById('<%= hdnAddePTemp.ClientID%>').value;
            if (id == "Y") {
                return true;
            } else {
                noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                return false;
            }
        }
        function EditPTempClick(hyperlink) {

            var id = document.getElementById('<%= hdnEditePTemp.ClientID%>').value;
            if (id == "Y") { return true; } else {
                noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                return false;
            }
        }
        function DeletePTempClick(hyperlink) {
            var id = document.getElementById('<%= hdnDeletePTemp.ClientID%>').value;
            if (id == "Y") { return CheckDelete();; } else {
                noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                return false;
            }
        }

        function CopyProjectTemplateClick(hyperlink) {
            var id = document.getElementById('<%= hdnEditePTemp.ClientID%>').value;
            if (id == "Y") { return true; } else {
                noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                return false;
            }
        }

    </script>


    <script type="text/javascript">

        function CheckDelete() {
            var result = false;
            $("#<%=RadGrid_ProjectTemplate.ClientID%> tr").each(function () {
                var checkBox = $(this).find("input[type='checkbox']");
                if (checkBox.is(":checked")) {
                    result = true;
                }
            });

            if (result == true) {
                return confirm('Do you really want to delete this template?');
            }
            else {
                alert('Please select a template to delete.')
                return false;
            }
        }

    </script>
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
                                    <div class="page-title">
                                        <i class="mdi-action-payment"></i>&nbsp;
                                        <asp:Label CssClass="title_text" ID="lblHeader" runat="server">Templates</asp:Label>
                                    </div>
                                    <div class="buttonContainer">
                                        <div class="btnlinks">
                                            <asp:LinkButton ID="lnkAdd" runat="server" OnClientClick='return AddPTempClick(this)' CausesValidation="False" OnClick="lnkAdd_Click">Add</asp:LinkButton>
                                        </div>

                                        <div class="btnlinks">
                                            <asp:LinkButton ID="lnkEdit" runat="server" CausesValidation="False" OnClientClick='return EditPTempClick(this)' OnClick="lnkEdit_Click">Edit</asp:LinkButton>
                                        </div>
                                        <div class="btnlinks">
                                            <asp:LinkButton ID="lnkCopy" runat="server" CausesValidation="False"  OnClientClick='return CopyProjectTemplateClick(this)'  OnClick="lnkCopy_Click">Copy</asp:LinkButton>
                                        </div>
                                        <div class="btnlinks">
                                            <asp:LinkButton ID="lnkDelete" runat="server" OnClientClick='return DeletePTempClick(this)' CausesValidation="False" OnClick="lnkDelete_Click">Delete</asp:LinkButton>
                                        </div>
                                        <ul id="dropdown1" class="dropdown-content">
                                            <li>
                                                <a href="CustomersReport.aspx?type=Customer">Add New Report</a>
                                            </li>
                                        </ul>
                                        <div class="btnlinks">
                                            <a class="dropdown-button" data-beloworigin="true" href="customersreport.aspx" data-activates="dropdown1">Reports</a>
                                        </div>
                                    </div>
                                    <div class="btnclosewrap">
                                        <asp:LinkButton CssClass="icon-closed" ID="lnkClose" runat="server" CausesValidation="false" ToolTip="Close" OnClick="lnkClose_Click"><i class="mdi-content-clear"></i></asp:LinkButton>
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
                <div class="srchpaneinner">
                    <div class="col lblsz2 lblszfloat">
                        <div class="row">
                            <span class="tro trost" >
                                <asp:CheckBox ID="lnkChk" runat="server" Text="Incl. Inactive" OnCheckedChanged="lnkChk_CheckedChanged" AutoPostBack="True" CssClass="css-checkbox"></asp:CheckBox>

                            </span>
                             <span class="tro trost">
                                <asp:LinkButton ID="lnkShowAll" runat="server" OnClick="lnkShowAll_Click">Show All </asp:LinkButton>
                            </span>
                            <span class="tro trost">
                                <asp:LinkButton ID="lnkClear" runat="server" OnClick="lnkClear_Click">Clear</asp:LinkButton>
                            </span>
                            <span class="tro trost">
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                    <ContentTemplate>
                                        <span>
                                            <asp:Label ID="lblRecordCount" runat="server" Style="font-style: italic;"></asp:Label>
                                        </span>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </span>
                        </div>
                    </div>
                </div>

            </div>
            <div class="grid_container">
                <div class="form-section-row pmd-card" >
                    <div class="RadGrid RadGrid_Material">
                        <telerik:RadAjaxManager ID="RadAjaxManager_ProjectTemplate" runat="server">
                            <AjaxSettings>
                                <telerik:AjaxSetting AjaxControlID="lnkChk">
                                    <UpdatedControls>
                                        <telerik:AjaxUpdatedControl ControlID="RadGrid_ProjectTemplate" LoadingPanelID="RadAjaxLoadingPanel_ProjectTemplate" />
                                    </UpdatedControls>
                                </telerik:AjaxSetting>
                                   <telerik:AjaxSetting AjaxControlID="lnkDelete">
                                    <UpdatedControls>
                                        <telerik:AjaxUpdatedControl ControlID="RadGrid_ProjectTemplate" LoadingPanelID="RadAjaxLoadingPanel_ProjectTemplate" />
                                    </UpdatedControls>
                                </telerik:AjaxSetting>
                            </AjaxSettings>
                        </telerik:RadAjaxManager>
                        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel_ProjectTemplate" runat="server">
                        </telerik:RadAjaxLoadingPanel>

                        <telerik:RadCodeBlock ID="RadCodeBlock_ProjectTemplate" runat="server">
                            <script type="text/javascript">
                                function pageLoad() {
                                    try {
                                        var grid = $find("<%= RadGrid_ProjectTemplate.ClientID %>");
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
                        <telerik:RadAjaxPanel ID="RadAjaxPanel_ProjectTemplate" runat="server" LoadingPanelID="RadAjaxLoadingPanel_ProjectTemplate" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">


                            <telerik:RadGrid RenderMode="Auto" ID="RadGrid_ProjectTemplate" AllowFilteringByColumn="true" ShowFooter="True" PageSize="50"
                                ShowStatusBar="true" runat="server" AllowPaging="True" AllowSorting="true" Width="100%" PagerStyle-AlwaysVisible="true" OnPreRender="RadGrid_ProjectTemplate_PreRender" OnItemCreated="RadGrid_ProjectTemplate_ItemCreated"
                                AllowCustomPaging="True" OnNeedDataSource="RadGrid_ProjectTemplate_NeedDataSource" OnItemCommand="RadGrid_ProjectTemplate_ItemCommand" OnItemDataBound="RadGrid_ProjectTemplate_ItemDataBound">
                                <CommandItemStyle />
                                <GroupingSettings CaseSensitive="false" />
                                <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                                    <Selecting AllowRowSelect="True"></Selecting>
                                    <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                    <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                </ClientSettings>
                                <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="True" ShowFooter="True">
                                    <Columns>

                                        <telerik:GridClientSelectColumn UniqueName="ClientSelectColumn" HeaderStyle-Width="28">
                                        </telerik:GridClientSelectColumn>

                                        <telerik:GridTemplateColumn DataField="id" SortExpression="id" AutoPostBackOnFilter="true" DataType="System.String"
                                            CurrentFilterFunction="Contains" HeaderText="Template No. #" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblID" runat="server" Text='<%# Eval("id") %>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn DataField="TemplateRev" SortExpression="TemplateRev" AutoPostBackOnFilter="true"
                                            CurrentFilterFunction="Contains" HeaderText="Template Rev" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTemplateRev" runat="server" Text='<%# Eval("TemplateRev") %>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn DataField="fdesc" SortExpression="fdesc" AutoPostBackOnFilter="true"
                                            CurrentFilterFunction="Contains" HeaderText="Template Name" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDesc" runat="server" Text='<%# Eval("fdesc") %>'></asp:Label>
                                                <%# Convert.ToInt16(Eval("Type")).Equals(0) ? "<span class='label label-sm label-warning'>Recurring</span>" : "" %>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn DataField="Dept" SortExpression="Dept" AutoPostBackOnFilter="true"
                                            CurrentFilterFunction="Contains" HeaderText="Department" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDept" runat="server" Text='<%# Eval("Dept") %>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn DataField="count" SortExpression="count" AutoPostBackOnFilter="true"
                                            CurrentFilterFunction="EqualTo" HeaderText="Open Projects" ShowFilterIcon="false" DataType="System.Int32">
                                            <ItemTemplate>
                                                <asp:Label ID="lblProjCount" runat="server" Text='<%# Eval("count") %>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn DataField="status" SortExpression="statusw" AutoPostBackOnFilter="true"
                                            CurrentFilterFunction="Contains" HeaderText="Status" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("status") %>'></asp:Label>
                                                <%--<asp:LinkButton ID="btnStatus" runat="server" CommandArgument="<%# Container.ItemIndex %>" CommandName="UpdateStatus">
                                                    <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("status") %>'></asp:Label>
                                                </asp:LinkButton>--%>
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

    <!-- Hidden Field -->
    <asp:HiddenField runat="server" ID="hdnAddePTemp" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnEditePTemp" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnDeletePTemp" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnViewPTemp" Value="Y" />
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="javascriptHolder" runat="Server">
</asp:Content>

