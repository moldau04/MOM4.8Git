<%@ Page Language="C#" MasterPageFile="~/Mom.master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="ComparativeStatement.aspx.cs" Inherits="ComparativeStatement" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="Stimulsoft.Report.WebDesign" Namespace="Stimulsoft.Report.Web" TagPrefix="cc1" %>
<%@ Register Assembly="Stimulsoft.Report.Web" Namespace="Stimulsoft.Report.Web" TagPrefix="cc1" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <!--Grid Control-->
    <link href="Design/css/grid.css" rel="stylesheet" />

    <style>
        .rgDataDiv, .stiJsViewerPageShadow {
            height: auto !important;
        }

        .RadGrid_Material .rgHeader {
            font-weight: bold !important;
        }

        .rgFooterWrapper .rgFooterDiv {
            padding-right: 0 !important;
        }

        .dropdown-content {
            width: auto !important;
        }

        .comparative-grid-column table.ajax__validatorcallout_popup_table {
            width: 250px !important;
        }

        .type-dropdown label {
            padding-right: 10px;
            width: 185px;
            display: inline-block;
        }

        .type-dropdown .RadButton {
            margin-top: 20px;
        }

        /** Columns */
        .rcbHeader ul,
        .rcbFooter ul,
        .rcbItem ul,
        .rcbHovered ul,
        .rcbDisabled ul {
            margin: 0;
            padding: 0;
            width: 100%;
            display: inline-block;
            list-style-type: none;
        }

        .exampleRadComboBox.RadComboBoxDropDown .rcbHeader {
            padding: 5px 27px 4px 7px;
        }

        .col1,
        .col2 {
            margin: 0;
            padding: 0 5px 0 0;
            line-height: 14px;
            float: left;
        }

        .rcbHeader .col1, .rcbHeader .col2{
            font-weight: bold;
        }

        .col1 {
            width: 30%;
        }

        .col2 {
            width: 70%;
        }
    </style>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="divbutton-container">
        <div id="divButtons">
            <div id="breadcrumbs-wrapper">
                <header>
                    <div class="col s12 m12 l12">
                        <div class="row">
                            <div class="page-title">
                                <i class="mdi-action-swap-vert-circle"></i>&nbsp;
                                <asp:Label runat="server" ID="pageTitle" Text="Comparative Profit and Loss Statement"></asp:Label>
                            </div>
                            <div class="buttonContainer">
                                <div class="btnlinks">
                                    <asp:LinkButton ID="lnkAddNew" runat="server" OnClick="lnkAddNew_Click">Add</asp:LinkButton>
                                </div>

                                <ul id="drpMenu" class="nomgn hideMenu menuList">
                                    <li>
                                        <div class="btnlinks" id="LI2pnlGridButtons" runat="server">
                                            <a class="dropdown-button" data-beloworigin="true" href="#!" data-activates="dynamicReport">Report
                                            </a>
                                        </div>
                                        <ul id="dynamicReport" class="dropdown-content po-report-dropdown">
                                            <asp:ListView ID="listComparativeReport" runat="server" Visible="true">
                                                <LayoutTemplate>
                                                    <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                                                </LayoutTemplate>
                                                <ItemTemplate>
                                                    <li id="poTemplateItem" runat="server" class="border-btm">
                                                        <asp:HyperLink runat="server" NavigateUrl='<%# String.Format("~/ComparativeStatement.aspx?id={0}&type={1}&showReport=true", Eval("ID"), Eval("States"))%>' Enabled="true">
                                                            <%#Eval("Name")%>
                                                        </asp:HyperLink>
                                                    </li>
                                                </ItemTemplate>
                                            </asp:ListView>
                                        </ul>
                                    </li>
                                </ul>
                            </div>
                            <div class="btnclosewrap">
                                <asp:LinkButton ID="lnkClose" runat="server" OnClick="lnkClose_Click"><i class="mdi-content-clear"></i></asp:LinkButton>
                            </div>
                            <div class="rght-content">
                                <div class="srchinputwrap">
                                    <asp:DropDownList ID="ddlStates" runat="server" TabIndex="12" CssClass="browser-default" Width="250"
                                        AutoPostBack="True" OnSelectedIndexChanged="ddlStates_SelectedIndexChanged">
                                        <asp:ListItem Value="ProfitAndLoss">Profit and Loss Statement</asp:ListItem>
                                        <asp:ListItem Value="BalanceSheet">Balance Sheet</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                    </div>
                </header>
            </div>
        </div>
    </div>

    <div class="container">
        <div class="row srchpane">
            <div class="col s12 m12 l12">
                <div class="srchpaneinner 12-period">
                    <div class="group-control" style="display: inline-block;">
                        <div class="srchtitle srchtitlecustomwidth 12-period">
                            Name
                        </div>
                        <div class="srchinputwrap pd-negatenw input-field">
                            <input type="text" id="txtReportTitle" runat="server" maxlength="100" cssclass="srchcstm" style="width: 300px;" autocomplete="off" />
                        </div>
                        <div class="srchtitle srchtitlecustomwidth 12-period">
                            Center
                        </div>
                        <div class="srchinputwrap">
                            <telerik:RadComboBox RenderMode="Auto" Skin="Metro" ID="rcCenter" runat="server" Filter="StartsWith" CheckBoxes="true" EnableCheckAllItemsCheckBox="true"
                                EmptyMessage="--Select Center--" CssClass="inGrid">
                            </telerik:RadComboBox>
                        </div>
                        <div class="srchinputwrap rdleftmgn">
                            <div class="rdpairing">
                                <div class="rd-flt">
                                    <asp:RadioButton ID="rdExpandAll" Text="Detail" runat="server" GroupName="rdExpColl" OnCheckedChanged="rdExpCollAll_CheckedChanged" AutoPostBack="true" />
                                </div>
                                <div class="rd-flt">
                                    <asp:RadioButton ID="rdDetailWithSub" Text="Detail with Sub" runat="server" GroupName="rdExpColl" OnCheckedChanged="rdExpCollAll_CheckedChanged" AutoPostBack="true" />
                                </div>
                                <div class="rd-flt">
                                    <asp:RadioButton ID="rdCollapseAll" Text="Summary" runat="server" GroupName="rdExpColl" OnCheckedChanged="rdExpCollAll_CheckedChanged" AutoPostBack="true" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel_Comp" runat="server">
                </telerik:RadAjaxLoadingPanel>
                <asp:UpdatePanel ID="columnsPanel" runat="server" EnableViewState="true">
                    <ContentTemplate>
                        <div class="srchpaneinner 12-period">
                            <div class="form-section-row">
                                <div class="section-ttle">
                                    Report columns
                                </div>
                            </div>
                            <div class="form-section-row RadGrid RadGrid_Material comparative-grid-column" style="max-width: 100%">
                                <telerik:RadCodeBlock ID="RadCodeBlockCol" runat="server">
                                    <script type="text/javascript">
                                        function pageLoad() {
                                            var grid = $find("<%= gvListColumns.ClientID %>");
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

                                <telerik:RadAjaxPanel ID="RadAjaxPanelCol" runat="server" LoadingPanelID="RadAjaxLoadingPanel_Comp" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">
                                    <telerik:RadGrid ID="gvListColumns" OnDeleteCommand="gvListColumns_DeleteCommand" OnItemDataBound="gvListColumns_ItemDataBound"
                                        ShowFooter="True" AllowSorting="true" AllowPaging="false" AllowCustomPaging="false" runat="server" Width="100%">
                                        <CommandItemStyle />
                                        <GroupingSettings CaseSensitive="false" />
                                        <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                                            <Selecting AllowRowSelect="True"></Selecting>
                                            <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                            <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                        </ClientSettings>
                                        <MasterTableView AutoGenerateColumns="false" ShowFooter="true" ShowHeader="true" ShowHeadersWhenNoRecords="true" DataKeyNames="Index">
                                            <Columns>
                                                <%--<telerik:GridButtonColumn CommandName="Delete" Text="Delete" UniqueName="DeleteColumn" HeaderStyle-Width="5%" ItemStyle-Width="5%" />--%>
                                                <telerik:GridTemplateColumn HeaderStyle-Width="40" AllowFiltering="false">
                                                    <ItemTemplate>
                                                        <asp:HiddenField ID="txtRowLine" Value='<%# Eval("Line") %>' runat="server"></asp:HiddenField>
                                                        <div class="handle" style="cursor: move" title="Move Up/Down">
                                                            <img src="images/Dragdrop.png" style="width: 20px" />
                                                        </div>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderStyle-Width="60" AllowFiltering="false" ItemStyle-Width="60" FooterStyle-Width="60">
                                                    <ItemTemplate>
                                                        <asp:LinkButton CommandName="Delete" ID="ibtnDeleteRow" OnClientClick="return confirm('Are you sure you want to delete this column')"
                                                            CausesValidation="false" ToolTip="Delete" runat="server" Style="color: #000; font-size: 1.5em; top: 0px; padding-left: 20px;" Width="40px">
                                                        <i class="mdi-navigation-cancel" style=" color: #f00;font-size: 1.2em; font-weight: bold;"></i>
                                                        </asp:LinkButton>
                                                    </ItemTemplate>

                                                    <FooterTemplate>
                                                        <asp:LinkButton ID="lnkAddnewRow" runat="server" CausesValidation="False" Style="color: #000; font-size: 1.5em;" Width="40px"
                                                            ToolTip="Add New Row" OnClick="lnkAddnewRow_Click"><i class="mdi-content-add-circle" style="color: #2bab54;font-size: 1.2em; font-weight: bold;"></i>
                                                        </asp:LinkButton>
                                                    </FooterTemplate>
                                                </telerik:GridTemplateColumn>

                                                <telerik:GridTemplateColumn HeaderText="Type" DataField="Type" UniqueName="Type" HeaderStyle-Width="40%" ItemStyle-Width="40%">
                                                    <ItemTemplate>
                                                        <div class="type-dropdown" style="display: inline-flex;">
                                                            <asp:DropDownList ID="lblDbType" runat="server" CssClass="browser-default selectst" Width="120" AutoPostBack="true" OnSelectedIndexChanged="lblDbType_SelectedIndexChanged" style="margin-right:10px;">
                                                            </asp:DropDownList>

                                                            <telerik:RadComboBox runat="server" ID="rcbDbBudget" Width="250" EmptyMessage="--Select budget--"
                                                                HighlightTemplatedItems="true" Filter="Contains" OnItemDataBound="rcbDbBudget_ItemDataBound" Text='<%#DataBinder.Eval(Container.DataItem,"Budget")%>' style="display:none;">
                                                                <HeaderTemplate>
                                                                    <ul>
                                                                        <li class="col1">Year</li>
                                                                        <li class="col2">Budget Name</li>
                                                                    </ul>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <ul>
                                                                        <li class="col1">
                                                                            <%# DataBinder.Eval(Container.DataItem, "Year") %></li>
                                                                        <li class="col2">
                                                                            <%# DataBinder.Eval(Container.DataItem, "Budget") %></li>
                                                                    </ul>
                                                                </ItemTemplate>
                                                            </telerik:RadComboBox>
                                                        </div>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>

                                                <telerik:GridTemplateColumn HeaderText="Label" DataField="Name" UniqueName="Label" HeaderStyle-Width="20%" ItemStyle-Width="20%" HeaderStyle-CssClass="text_center" ItemStyle-CssClass="text_center">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="lblDbLabel" runat="server" Text='<%# Bind("Label") %>' OnTextChanged="lblDbLabel_TextChanged" AutoPostBack="true" autocomplete="off"></asp:TextBox>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Start Date/Column" DataField="FromDate" UniqueName="FromDate" HeaderStyle-Width="20%" ItemStyle-Width="20%" HeaderStyle-CssClass="text_center" ItemStyle-CssClass="text_center">
                                                    <ItemTemplate>
                                                        <asp:Panel runat="server" ID="panelFromDate">
                                                            <asp:TextBox ID="lblDbFromDate" runat="server" Text='<%# Eval("FromDate")!=DBNull.Value? (!(Eval("FromDate").Equals(DateTime.MinValue)) ? (String.Format("{0:MM/dd/yyyy}", Eval("FromDate"))) : "" ) : "" %>'
                                                                ValidationGroup="comp" CssClass="datepicker_mom" Style="width: 100%!important; font-size: inherit;" autocomplete="off"></asp:TextBox>

                                                            <asp:RegularExpressionValidator ID="revFromDate" ControlToValidate="lblDbFromDate" ValidationGroup="comp"
                                                                ValidationExpression="^(0?[1-9]|1[012])[\/\-](0?[1-9]|[12][0-9]|3[01])[\/\-]\d{4}$"
                                                                runat="server" ErrorMessage="Invalid Date format!" Display="None">
                                                            </asp:RegularExpressionValidator>
                                                            <asp:ValidatorCalloutExtender ID="vceFromDate" runat="server" Enabled="True" PopupPosition="Right" Width="200" TargetControlID="revFromDate" />

                                                            <asp:RequiredFieldValidator ID="reqFromDate" runat="server" ControlToValidate="lblDbFromDate" ValidationGroup="comp"
                                                                ErrorMessage="Start date is required" Display="None" SetFocusOnError="true">
                                                            </asp:RequiredFieldValidator>
                                                            <asp:ValidatorCalloutExtender ID="vceReqFromDate" runat="server" Enabled="True" PopupPosition="Right" Width="200" TargetControlID="reqFromDate" />
                                                        </asp:Panel>
                                                        <asp:Panel runat="server" ID="panelColumn1">
                                                            <asp:DropDownList ID="lblDbColumn1" runat="server" CssClass="browser-default selectst" Width="100%"></asp:DropDownList>
                                                        </asp:Panel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="End Date/Column" DataField="ToDate" UniqueName="ToDate" HeaderStyle-Width="20%" ItemStyle-Width="20%" HeaderStyle-CssClass="text_center" ItemStyle-CssClass="text_center">
                                                    <ItemTemplate>
                                                        <asp:Panel runat="server" ID="panelToDate">
                                                            <asp:TextBox ID="lblDbToDate" runat="server" Text='<%# Eval("ToDate")!=DBNull.Value? (!(Eval("ToDate").Equals(DateTime.MinValue)) ? (String.Format("{0:MM/dd/yyyy}", Eval("ToDate"))) : "" ) : "" %>'
                                                                CssClass="datepicker_mom" Style="width: 100%!important; font-size: inherit;" autocomplete="off"></asp:TextBox>

                                                            <asp:RegularExpressionValidator ID="revToDate" ControlToValidate="lblDbToDate" ValidationGroup="comp"
                                                                ValidationExpression="^(0?[1-9]|1[012])[\/\-](0?[1-9]|[12][0-9]|3[01])[\/\-]\d{4}$"
                                                                runat="server" ErrorMessage="Invalid Date format!" Display="None">
                                                            </asp:RegularExpressionValidator>
                                                            <asp:ValidatorCalloutExtender ID="vceToDate" runat="server" Enabled="True" PopupPosition="Right" Width="200" TargetControlID="revToDate" />

                                                            <asp:RequiredFieldValidator ID="reqToDate" runat="server" ControlToValidate="lblDbToDate" ValidationGroup="comp"
                                                                ErrorMessage="End date is required" Display="None" SetFocusOnError="true">
                                                            </asp:RequiredFieldValidator>
                                                            <asp:ValidatorCalloutExtender ID="vceReqToDate" runat="server" Enabled="True" PopupPosition="Right" Width="200" TargetControlID="reqToDate" />
                                                        </asp:Panel>
                                                        <asp:Panel runat="server" ID="panelColumn2">
                                                            <asp:DropDownList ID="lblDbColumn2" runat="server" CssClass="browser-default selectst" Width="100%"></asp:DropDownList>
                                                        </asp:Panel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>

                                            </Columns>
                                        </MasterTableView>
                                    </telerik:RadGrid>
                                </telerik:RadAjaxPanel>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="col s12 m12 l12">
                <div class="btnlinks">
                    <asp:LinkButton ID="lnkSaveReport" runat="server" OnClick="lnkSaveReport_Click" ToolTip="Save Report">Save</asp:LinkButton>
                </div>
                <div class="btnlinks">
                    <asp:LinkButton ID="lnkCopyReport" runat="server" OnClick="lnkCopyReport_Click" ToolTip="Copy Report" Visible="false">Copy</asp:LinkButton>
                </div>
                <div class="btnlinks">
                    <asp:LinkButton ID="linkDeleteReport" runat="server" OnClick="linkDeleteReport_Click" OnClientClick="return deleteReportConfirm();" ToolTip="Delete Report" Visible="false">Delete</asp:LinkButton>
                </div>
                <div class="srchinputwrap btnlinksicon srchclr" style="padding-top: 2px;">
                    <asp:LinkButton ID="lnkLoadReport" runat="server" OnClick="lnkLoadReport_Click" ToolTip="Refresh"><i class="fa fa-refresh"></i></asp:LinkButton>
                </div>
            </div>
        </div>
    </div>

    <div class="grid_container">
        <div class="form-section-row" style="margin-bottom: 0 !important;">
            <cc1:StiWebViewer ID="StiWebViewerComparative" Height="800px" RequestTimeout="20000" runat="server" ViewMode="Continuous" ScrollbarsMode="true" CacheMode="None"
                OnGetReport="StiWebViewerComparative_GetReport" OnGetReportData="StiWebViewerComparative_GetReportData" Visible="false" />
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="javascriptHolder" runat="Server">
    <script type="text/javascript">
        function deleteReportConfirm() {
            var mess = "Are you sure you want to delete this report?";
            if (confirm(mess)) { return true; }
            else { return false; }
        }

        function pageLoad() {
            $("#<%= gvListColumns.ClientID %>").sortable({
                items: 'tr',
                handle: ".handle",
                cursor: 'move',
                axis: 'y',
                dropOnEmpty: false,
                start: function (e, ui) {
                    ui.item.addClass("selected");
                },
                stop: function (e, ui) {
                    ui.item.removeClass("selected", 1000, "swing");
                    updategridindex();
                },
                receive: function (e, ui) {
                    $(this).find("tbody").append(ui.item);
                }
            });
        }

        function updategridindex() {
            var grid1 = 1;
            $('#<%= gvListColumns.ClientID %>  tbody  > tr').not(':first').not(':last').each(function () {
                var cat = $(this).find('input[id*="txtRowLine"]');
                cat.val(grid1);
                grid1 = grid1 + 1;
                console.log(cat.val());
            });
        }
    </script>
</asp:Content>
