<%@ Page Title="Billing Codes || MOM" Language="C#" MasterPageFile="~/Mom.master" AutoEventWireup="true" Inherits="BillingCodes" Codebehind="BillingCodes.aspx.cs" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <!--Grid Control-->
    <link href="Design/css/grid.css" rel="stylesheet" />
    <link href="Design/css/pikaday.css" rel="stylesheet" />
    <style>
        .autocomplete_completionListElement {
            margin: 0px !important;
            background-color: #fff;
            color: windowtext;
            border: buttonshadow;
            border-width: 1px;
            border-style: solid;
            cursor: default;
            overflow: auto;
            height: 200px;
            text-align: left;
            list-style-type: none;
        }

        .RadWindow_Material {
            height: 465px !important;
        }

        .rwContent {
            height: 411px !important;
        }
    </style>
     <style>
         [id$='PageSizeComboBox'] {
             width: 5.1em !important;
         }
     </style>
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

                                    <div class="page-title"><i class="mdi-action-payment"></i>&nbsp;Billing Codes</div>
                                    <div class="buttonContainer">
                                        <asp:Panel runat="server" ID="pnlGridButtons">

                                            <div class="btnlinks">
                                                <asp:LinkButton ID="lnkAddnew" runat="server" OnClientClick="OpenBillingCodetModal();return false">Add</asp:LinkButton>
                                                <telerik:RadWindowManager ID="RadWindowManager1" runat="server">
                                                    <Windows>
                                                        <telerik:RadWindow ID="BillingCodeWindow" Skin="Material" VisibleTitlebar="true" Behaviors="Default" CenterIfModal="true"
                                                            Animation="FlyIn" AnimationDuration="200" RenderMode="Auto" VisibleStatusbar="false" MinWidth="500"
                                                            runat="server" Modal="true" Width="650" Height="550">
                                                            <ContentTemplate>
                                                                <div class="m-t-15" >
                                                                    <div class="form-section-row">
                                                                        <div class="form-section2">
                                                                            <div class="input-field col s12">
                                                                                <div class="row">
                                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator40" runat="server" ControlToValidate="txtBillCode"
                                                                                        Display="None" ErrorMessage="Billing Code Required" SetFocusOnError="True" ValidationGroup="serv"></asp:RequiredFieldValidator>
                                                                                    <asp:ValidatorCalloutExtender ID="RequiredFieldValidator40_ValidatorCalloutExtender" PopupPosition="BottomLeft"
                                                                                        runat="server" Enabled="True" TargetControlID="RequiredFieldValidator40">
                                                                                    </asp:ValidatorCalloutExtender>

                                                                                    <asp:TextBox ID="txtBillCode" runat="server" CssClass="Contact-search" MaxLength="30"></asp:TextBox>

                                                                                    <label for="txtBillCode">Billing Code</label>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                        <div class="form-section2-blank">
                                                                            &nbsp;
                                                                        </div>
                                                                        <div class="form-section2">
                                                                            <div class="input-field col s12">
                                                                                <div class="row">
                                                                                    <label class="drpdwn-label">Status</label>

                                                                                    <asp:DropDownList ID="ddlStatus" runat="server" CssClass="browser-default">
                                                                                        <asp:ListItem Value="0">Active</asp:ListItem>
                                                                                        <asp:ListItem Value="1">Inactive</asp:ListItem>
                                                                                    </asp:DropDownList>
                                                                                </div>
                                                                            </div>

                                                                        </div>


                                                                    </div>

                                                                    <div class="form-section-row remarks-css" >

                                                                        <div class="form-section2">
                                                                            <div class="input-field col s12">
                                                                                <div class="row">
                                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator41" runat="server" ControlToValidate="txtBillBal"
                                                                                        Display="None" ErrorMessage="Rate Required" SetFocusOnError="True" ValidationGroup="serv"></asp:RequiredFieldValidator>
                                                                                    <asp:ValidatorCalloutExtender ID="RequiredFieldValidator41_ValidatorCalloutExtender" PopupPosition="BottomLeft"
                                                                                        runat="server" Enabled="True" TargetControlID="RequiredFieldValidator41">
                                                                                    </asp:ValidatorCalloutExtender>
                                                                                    <asp:TextBox ID="txtBillBal" runat="server" MaxLength="25" CssClass="Contact-search"></asp:TextBox>
                                                                                    <asp:FilteredTextBoxExtender ID="txtBillBal_FilteredTextBoxExtender" runat="server"
                                                                                        Enabled="True" TargetControlID="txtBillBal" ValidChars="1234567890.-">
                                                                                    </asp:FilteredTextBoxExtender>
                                                                                    <label for="txtBillBal">Rate</label>
                                                                                </div>
                                                                            </div>

                                                                        </div>
                                                                        <div class="form-section2-blank">
                                                                            &nbsp;
                                                                        </div>
                                                                        <div class="form-section2">
                                                                            <div class="input-field col s12">
                                                                                <div class="row">
                                                                                    <asp:TextBox ID="txtBillMeasure" runat="server" MaxLength="10" CssClass="Contact-search"></asp:TextBox>
                                                                                    <label for="txtBillMeasure">Measure</label>

                                                                                </div>
                                                                            </div>

                                                                        </div>
                                                                    </div>
                                                                    <div class="form-section-row remarks-css" >

                                                                        <div class="input-field col s12">
                                                                            <div class="row">
                                                                                <asp:RequiredFieldValidator ID="rfvGL" runat="server" ControlToValidate="txtAccount"
                                                                                    Display="None" ErrorMessage="GL Account is Required" SetFocusOnError="True" ValidationGroup="serv"></asp:RequiredFieldValidator>
                                                                                <asp:ValidatorCalloutExtender ID="vceGL" PopupPosition="BottomLeft"
                                                                                    runat="server" Enabled="True" TargetControlID="rfvGL">
                                                                                </asp:ValidatorCalloutExtender>
                                                                                <asp:CustomValidator ID="cvGL" runat="server" ClientValidationFunction="ChkGL" ValidationGroup="serv"
                                                                                    ControlToValidate="txtAccount" Display="None" ErrorMessage="Please select GL Account"
                                                                                    SetFocusOnError="True"></asp:CustomValidator>
                                                                                <asp:ValidatorCalloutExtender ID="vceGL1" runat="server" Enabled="True" PopupPosition="BottomLeft"
                                                                                    TargetControlID="cvGL">
                                                                                </asp:ValidatorCalloutExtender>
                                                                                <asp:TextBox ID="txtAccount" runat="server" CssClass="Contact-search searchinput"
                                                                                    onkeyup="EmptyValue(this);"
                                                                                    autocomplete="off" placeholder="Search by Account # or Description"></asp:TextBox>
                                                                                <asp:AutoCompleteExtender runat="server" Enabled="True" ServicePath="CustomerAuto.asmx" TargetControlID="txtAccount"
                                                                                    EnableCaching="False" ServiceMethod="GetAccounts" UseContextKey="True" MinimumPrefixLength="0"
                                                                                    CompletionListCssClass="autocomplete_completionListElement"
                                                                                    CompletionListItemCssClass="autocomplete_listItem"
                                                                                    CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                                    CompletionListElementID="ListDivisor"
                                                                                    OnClientItemSelected="ace_itemSelected"
                                                                                    ID="AutoCompleteExtender" DelimiterCharacters="" CompletionInterval="250">
                                                                                </asp:AutoCompleteExtender>
                                                                                <label for="txtAccount">GL Account</label>
                                                                                <div id="ListDivisor"></div>
                                                                            </div>
                                                                        </div>
                                                                    </div>


                                                                    <div class="form-section-row remarks-css" >
                                                                        <div class="input-field col s12">
                                                                            <div class="row">
                                                                                <asp:TextBox ID="txtBillDesc" runat="server" TextMode="MultiLine" MaxLength="8000" CssClass="materialize-textarea"></asp:TextBox>
                                                                                <label for="txtBillDesc">Billing&nbsp;Code&nbsp;Description</label>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                    <div class="form-section-row remarks-css " >
                                                                        <div class="input-field col s12">
                                                                            <div class="row">


                                                                                <label for="txtBillRemarks">Remarks</label>
                                                                                <asp:TextBox ID="txtBillRemarks" TextMode="MultiLine" runat="server" MaxLength="8000" CssClass="materialize-textarea"></asp:TextBox>
                                                                            </div>
                                                                        </div>
                                                                    </div>

                                                                    <div style="clear: both;"></div>

                                                                    <footer class="footer-css2">
                                                                        <div class="btnlinks">
                                                                            <asp:LinkButton ID="lnkBillingCodesSave" runat="server" ValidationGroup="serv" OnClick="lnkBillingCodesSave_Click">Save</asp:LinkButton>
                                                                        </div>
                                                                    </footer>
                                                                </div>
                                                            </ContentTemplate>

                                                        </telerik:RadWindow>
                                                    </Windows>
                                                </telerik:RadWindowManager>
                                            </div>
                                            <div class="btnlinks">
                                                <asp:HyperLink ID="lnkBtnEdit" runat="server" Style="cursor: pointer;" onclick="OpenContactPopupEdit(this);return false">Edit</asp:HyperLink>
                                            </div>
                                            <div class="btnlinks">
                                                <asp:LinkButton ID="lnkDeleteBillingCodes" runat="server" OnClientClick="return confirm('Do you really want to delete this Billing Code ?');"
                                                    OnClick="lnkDeleteBillingCodes_Click" CausesValidation="False">Delete</asp:LinkButton>
                                            </div>
                                        </asp:Panel>

                                        <ul id="dropdown1" class="dropdown-content">
                                            <li>
                                                <a href="CustomersReport.aspx?type=Customer">Add New Report</a>
                                            </li>

                                        </ul>
                                        <div class="btnlinks">
                                            <a class="dropdown-button" data-beloworigin="true" href="customersreport.aspx" data-activates="dropdown1">Reports
                                            </a>
                                        </div>
                                    </div>
                                    <div class="btnclosewrap">
                                        <asp:LinkButton ID="lnkClose" runat="server" CausesValidation="false"
                                            OnClick="lnkClose_Click"><i class="mdi-content-clear"></i></asp:LinkButton>
                                    </div>

                                <div class="btnlinks">
                             <asp:LinkButton ID="btnExcel" runat="server" OnClick="btnExcel_Click">Export to Excel</asp:LinkButton>

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
                            <span class="tro trost">

                                <%--<asp:Label ID="lblChkSelect" runat="server" CssClass="title-check-text" For="lnkChk">--%>
                                    <asp:CheckBox ID="lnkChk" Text=" Incl. Inactive" runat="server" OnCheckedChanged="lnkchk_Click" AutoPostBack="True" CssClass="css-checkbox"></asp:CheckBox>
                                   <%-- </asp:Label>--%>
                            </span>
                            <span class="tro trost">
                                <asp:LinkButton ID="lnkShowAll" runat="server" OnClick="lnkShowAll_Click">Show All </asp:LinkButton>
                            </span>
                            <span class="tro trost">
                                <asp:UpdatePanel ID="updpnl" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Label ID="lblRecordCount" runat="server"></asp:Label>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </span>
                        </div>
                    </div>
                </div>
            </div>

            <div class="grid_container">
                <div class="form-section-row m-b-0" ">
                    <telerik:RadAjaxManager ID="RadAjaxManager_BillCodes" runat="server">
                        <AjaxSettings>
                            <telerik:AjaxSetting AjaxControlID="lnkDeleteBillingCodes">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="RadGrid_BillCodes" LoadingPanelID="RadAjaxLoadingPanel_BillCodes" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                            <telerik:AjaxSetting AjaxControlID="lnkChk">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="RadGrid_BillCodes" LoadingPanelID="RadAjaxLoadingPanel_BillCodes" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                            <telerik:AjaxSetting AjaxControlID="lnkShowAll">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="RadGrid_BillCodes" LoadingPanelID="RadAjaxLoadingPanel_BillCodes" />
                                    <telerik:AjaxUpdatedControl ControlID="lnkChk"  />
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                            <telerik:AjaxSetting AjaxControlID="lnkBillingCodesSave">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="RadGrid_BillCodes" LoadingPanelID="RadAjaxLoadingPanel_BillCodes" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                        </AjaxSettings>
                    </telerik:RadAjaxManager>
                    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel_BillCodes" runat="server">
                    </telerik:RadAjaxLoadingPanel>

                    <div class="RadGrid RadGrid_Material">
                        <telerik:RadCodeBlock ID="codeBlock1" runat="server">
                            <script type="text/javascript">
                                function pageLoad() {
                                    var grid = $find("<%= RadGrid_BillCodes.ClientID %>");
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
                        <telerik:RadAjaxPanel ID="RadAjaxPanel_BillCodes" runat="server" LoadingPanelID="RadAjaxLoadingPanel_BillCodes" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">
                            <telerik:RadGrid RenderMode="Auto" ID="RadGrid_BillCodes" AllowFilteringByColumn="true" ShowFooter="True" PageSize="50"
                                ShowStatusBar="true" runat="server" AllowPaging="True" AllowSorting="true" Width="100%" PagerStyle-AlwaysVisible="true" FilterType="CheckList"
                                AllowCustomPaging="True" 
                                OnNeedDataSource="RadGrid_BillCodes_NeedDataSource" 
                                OnItemCreated="RadGrid_BillCodes_ItemCreated" 
                                OnItemEvent="RadGrid_BillCodes_ItemEvent"
                                OnPreRender="RadGrid_BillCodes_PreRender">
                                <CommandItemStyle />
                                <GroupingSettings CaseSensitive="false" />
                                <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                                    <Selecting AllowRowSelect="True"></Selecting>
                                    <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                    <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                </ClientSettings>
                                <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="True" ShowFooter="True">
                                    <PagerStyle AlwaysVisible="true" Mode="NextPrevAndNumeric" />
                                    <Columns>
                                        <telerik:GridClientSelectColumn UniqueName="chkSelect" HeaderStyle-Width="40">
                                        </telerik:GridClientSelectColumn>
                                        <telerik:GridTemplateColumn UniqueName="lblCommonID" Display="false" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblIndexID" runat="server" Text='<%# Eval("id") %>'></asp:Label>
                                                <asp:Label ID="lblStatusID" runat="server" Text='<%# Eval("Statusid") %>'></asp:Label>
                                                <asp:Label ID="lblAcctID" runat="server" Text='<%# Eval("sacct") %>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn DataField="Name" HeaderText="Billing Code" SortExpression="Name" HeaderStyle-Width="100"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                            ShowFilterIcon="false">
                                            <ItemTemplate>

                                                <asp:Label ID="lblId" runat="server" Text='<%# Eval("Name") %>' OnClick="OpenContactPopupEdit(this);return false"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label runat="server" Text="Total :-"></asp:Label>
                                            </FooterTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn DataField="fdesc" SortExpression="fdesc" AutoPostBackOnFilter="true" HeaderStyle-Width="220"
                                            CurrentFilterFunction="Contains" HeaderText="Description" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lbldesc" runat="server" Text='<%# Eval("fdesc") %>' OnClick="OpenContactPopupEdit(this);return false"></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn DataField="account" SortExpression="account" AutoPostBackOnFilter="true" HeaderStyle-Width="120"
                                            CurrentFilterFunction="Contains" HeaderText="GL Account" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblaccount" runat="server" Text='<%# Eval("account") %>' OnClick="OpenContactPopupEdit(this);return false"></asp:Label>

                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn DataField="Status" SortExpression="Status" AutoPostBackOnFilter="true" HeaderStyle-Width="60"
                                            CurrentFilterFunction="Contains" HeaderText="Status" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("Status") %>' OnClick="OpenContactPopupEdit(this);return false"></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn DataField="Price1" SortExpression="Price1" FooterAggregateFormatString="{0:c}" Aggregate="Sum" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" HeaderText="Rate" ShowFilterIcon="false" HeaderStyle-Width="60">
                                            <ItemTemplate>
                                                <asp:Label ID="lblbalance" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Price1", "{0:c}")%>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn DataField="Measure" SortExpression="Measure" AutoPostBackOnFilter="true" HeaderStyle-Width="60"
                                            CurrentFilterFunction="Contains" HeaderText="Measure" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblMeasure" runat="server" Text='<%# Eval("Measure") %>' OnClick="OpenContactPopupEdit(this);return false"></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn DataField="Remarks" SortExpression="Remarks" AutoPostBackOnFilter="true" HeaderStyle-Width="160"
                                            CurrentFilterFunction="Contains" HeaderText="Remarks" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblrem" runat="server" Text='<%# Eval("Remarks") %>' OnClick="OpenContactPopupEdit(this);return false"></asp:Label>
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
    <asp:HiddenField ID="hdnBillID" runat="server" />
     <asp:HiddenField ID="hdnBillName" runat="server" />
    <asp:HiddenField ID="hdnAddEdit" runat="server" />
    <asp:HiddenField ID="hdnIsEdit" runat="server" Value="Y" />
    <input id="hdnPatientId" runat="server" type="hidden" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="javascriptHolder" runat="Server">
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

        function OpenBillingCodetModal() {
            $('#<%=txtBillCode.ClientID%>').val("");
            $('#<%=txtBillDesc.ClientID%>').val("");
            $('#<%=txtAccount.ClientID%>').val("");
            $('#<%=ddlStatus.ClientID%>').val("");
            $('#<%=txtBillBal.ClientID%>').val("");
            $('#<%=txtBillMeasure.ClientID%>').val("");
            $('#<%=txtBillRemarks.ClientID%>').val("");
            $('#<%=hdnBillName.ClientID%>').val("");
            $('#<%=hdnAddEdit.ClientID%>').val("0");
            var wnd = $find('<%=BillingCodeWindow.ClientID %>');
            var wnd = $find('<%=BillingCodeWindow.ClientID %>');
            wnd.set_title("Add Bill Code");

            wnd.Show();

        }
        function OpenContactPopupEdit() {

            var ID = "";
            var Name = "";
            var Description = "";
            var Account = "";
            var Statusid = "";
            var Price1 = "";
            var Measure = "";
            var Remarks = "";
            var AcctID = "";
            $("#<%=RadGrid_BillCodes.ClientID %>").find('tr:not(:first, :last)').each(function () {
                var $tr = $(this);
                $tr.find('input[type="checkbox"]:checked').each(function (index, value) {
                    ID = $tr.find('span[id*=lblIndexID]').text();
                    Name = $tr.find('span[id*=lblId]').text();
                    Description = $tr.find('span[id*=lbldesc]').text();
                    Account = $tr.find('span[id*=lblaccount]').text();
                    Statusid = $tr.find('span[id*=lblStatusID]').text();
                    Price1 = $tr.find('span[id*=lblbalance]').text();
                    Measure = $tr.find('span[id*=lblMeasure]').text();
                    Remarks = $tr.find('span[id*=lblrem]').text();
                    AcctID = $tr.find('span[id*=lblAcctID]').text();
                });
            });
            if (ID != "") {
                $('#<%=txtBillCode.ClientID%>').val(Name);
                $('#<%=txtBillDesc.ClientID%>').val(Description);
                $('#<%=txtAccount.ClientID%>').val(Account);
                $('#<%=ddlStatus.ClientID%>').val(Statusid);
                $('#<%=txtBillBal.ClientID%>').val(Price1);
                $('#<%=txtBillMeasure.ClientID%>').val(Measure);
                $('#<%=txtBillRemarks.ClientID%>').val(Remarks);
                $('#<%=hdnBillID.ClientID%>').val(ID);
                $('#<%=hdnBillName.ClientID%>').val(Name);
                $('#<%=hdnPatientId.ClientID%>').val(AcctID);
                $('#<%=hdnAddEdit.ClientID%>').val("1");

                var wnd = $find('<%=BillingCodeWindow.ClientID %>');
                wnd.set_title("Edit Bill Code");
                var val = $('#<%=hdnIsEdit.ClientID%>').val();
                if (val == "Y") {
                    wnd.Show();
                }
                Materialize.updateTextFields();

            }

        }
        function CloseBillingCodeWindow() {
            var wnd = $find('<%=BillingCodeWindow.ClientID %>');
            wnd.Close();
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
        function ace_itemSelected(sender, e) {
            var hdnPatientId = document.getElementById('<%= hdnPatientId.ClientID %>');
            hdnPatientId.value = e.get_value();
        }
        function EmptyValue(txt) {
            if ($(txt).val() == '') { $('#<%= hdnPatientId.ClientID %>').val(''); }
        }
        function ChkGL(sender, args) {
            var hdnGL = document.getElementById('<%=hdnPatientId.ClientID%>');
            if (hdnGL.value == '') {
                args.IsValid = false;
            }
            else if (hdnGL.value == '0') {
                args.IsValid = false;
            }
        }
    </script>
</asp:Content>

