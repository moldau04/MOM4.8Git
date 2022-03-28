<%@ Page Language="C#" MasterPageFile="~/Mom.master" AutoEventWireup="true" Inherits="PreviewInvoiceWithTicket" CodeBehind="PreviewInvoiceWithTicket.aspx.cs" %>


<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="Stimulsoft.Report.WebDesign" Namespace="Stimulsoft.Report.Web" TagPrefix="cc1" %>

<%@ Register Assembly="Stimulsoft.Report.Web" Namespace="Stimulsoft.Report.Web" TagPrefix="cc1" %>
<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <!--Grid Control-->
    <link href="Design/css/grid.css" rel="stylesheet" />
    <style>
        #overlay {
            position: fixed; /* Sit on top of the page content */
            display: none; /* Hidden by default */
            width: 100%; /* Full width (cover the whole page) */
            height: 100%; /* Full height (cover the whole page) */
            top: 0;
            left: 0;
            right: 0;
            bottom: 0;
            background-color: rgba(0,0,0,0.5); /* Black background with opacity */
            z-index: 1000000; /* Specify a stack order in case you're using a different order for other elements */
            cursor: pointer; /* Add a pointer on hover */
        }

        .RadWindow_Material .rwContent {
            padding-bottom: 0 !important;
        }
    </style>
    <script>
        function openMailForm() {
            //window.onload = window.radopen(null, "RadCreateWindow");
        }

        window.onload = function () {
            var reloading = sessionStorage.getItem("reloading");
            if (reloading) {
                sessionStorage.removeItem("reloading");
                openMailForm();
            }
        }
        function reloadP() {
            sessionStorage.setItem("reloading", "true");
            document.location.reload();
        }
        function cancel() {
            <%--var window1 = $find('<%=RadCreateWindow.ClientID %>');
            window1.Close();--%>
            $('#dvPreview').attr('style', 'display:block');
            $('#dvEmailOpen').attr('style', 'display:none');
            $("#<%=hdnActiveDiv.ClientID%>").val("0");
            return false;
        }

        function OpenSendEmailWindow() {
            $('#dvPreview').attr('style', 'display:none');
            $('#dvEmailOpen').attr('style', 'display:block');
            $("#<%=hdnActiveDiv.ClientID%>").val("1");
            <%--var wnd = $find('<%=RadCreateWindow.ClientID %>');
            wnd.Show();--%>
        }
        function refreshPage(url) {
            window.setTimeout(function () {
                window.location.href = url;
            }, 7500);
        }
        function UpdateSelectedRowsForGrid() {
            var wnd = $find('<%=EmailsSelectionWindow.ClientID %>');
            if (wnd != null) {
                if (wnd.get_title() == "TO: Email Selection") {
                    UpdateSelectedRows('<%=RadGrid_Emails.ClientID%>', '<%=txtEmail.ClientID%>');
                    $("#<%=RadGrid_Emails.ClientID%> input[id*='chkSelect']:checkbox").unbind('click').bind('click', CheckUncheckAllCheckBoxAsNeeded_To);
                    $("#<%=RadGrid_Emails.ClientID%> input[id*='chkAll']:checkbox").unbind('click').bind('click', function () {
                        if ($(this).is(':checked')) {
                            $("#<%=RadGrid_Emails.ClientID%> input[id*='chkSelect']:checkbox").prop('checked', true);
                        }
                        else {
                            $("#<%=RadGrid_Emails.ClientID%> input[id*='chkSelect']:checkbox").prop('checked', false);
                        }
                        CheckEmailsCheckBox('<%=RadGrid_Emails.ClientID%>', '<%=txtEmail.ClientID%>');
                    });
                } else if (wnd.get_title() == "CC: Email Selection") {
                    UpdateSelectedRows('<%=RadGrid_Emails.ClientID%>', '<%=txtEmailCc.ClientID%>');
                    $("#<%=RadGrid_Emails.ClientID%> input[id*='chkSelect']:checkbox").unbind('click').bind('click', CheckUncheckAllCheckBoxAsNeeded_CC);
                    $("#<%=RadGrid_Emails.ClientID%> input[id*='chkAll']:checkbox").unbind('click').bind('click', function () {
                        if ($(this).is(':checked')) {
                            $("#<%=RadGrid_Emails.ClientID%> input[id*='chkSelect']:checkbox").prop('checked', true);
                        }
                        else {
                            $("#<%=RadGrid_Emails.ClientID%> input[id*='chkSelect']:checkbox").prop('checked', false);
                        }
                        CheckEmailsCheckBox('<%=RadGrid_Emails.ClientID%>', '<%=txtEmailCc.ClientID%>');
                    });
                } else if (wnd.get_title() == "BCC: Email Selection") {

                }
            }
        }
        $(document).ready(function () {
            if (Sys.Extended && Sys.Extended.UI && Sys.Extended.UI.HtmlEditorExtenderBehavior && Sys.Extended.UI.HtmlEditorExtenderBehavior.prototype && Sys.Extended.UI.HtmlEditorExtenderBehavior.prototype._editableDiv_submit) {
                Sys.Extended.UI.HtmlEditorExtenderBehavior.prototype._editableDiv_submit = function () {
                    //html encode
                    var char = 3;
                    var sel = null;

                    setTimeout(function () {
                        if (this._editableDiv != null)
                            this._editableDiv.focus();
                    }, 0);
                    if (Sys.Browser.agent != Sys.Browser.Firefox) {
                        if (document.selection) {
                            sel = document.selection.createRange();
                            sel.moveStart('character', char);
                            sel.select();
                        }
                        else {
                            if (this._editableDiv.firstChild != null && this._editableDiv.firstChild != undefined) {
                                sel = window.getSelection();
                                sel.collapse(this._editableDiv.firstChild, char);
                            }
                        }
                    }

                    //Encode html tags
                    this._textbox._element.value = this._encodeHtml();
                }
            }
        });

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript">
        Sys.Application.add_init(appl_init);

        function appl_init() {
            var pgRegMgr = Sys.WebForms.PageRequestManager.getInstance();
            pgRegMgr.add_beginRequest(BlockUI);
            pgRegMgr.add_endRequest(UnblockUI);
        }

        function BlockUI(sender, args) {
            document.getElementById("overlay").style.display = "block";
        }
        function UnblockUI(sender, args) {
            document.getElementById("overlay").style.display = "none";
        }
        function splitEmail(txt) {
            var regExp = /\(([^)]+)\)/;
            return regExp.exec(txt);
        }
        function SetSelectedValue() {
            var emailVal = $('#<%=hdnEmail.ClientID %>').val();
            var txt = $('#<%=txtEmail.ClientID %>').val();
            var matches = splitEmail(txt);
            if (emailVal != '') {
                emailVal = emailVal + ',' + matches[1];
            } else {
                emailVal = matches[1];
            }
            $('#<%=txtEmail.ClientID %>').val(emailVal);
            $('#<%=hdnEmail.ClientID %>').val(emailVal);
        }

        function SetSelectedValueCc() {
            var emailVal = $('#<%=hdnEmailCc.ClientID %>').val();
            var txt = $('#<%=txtEmailCc.ClientID %>').val();
            var matches = splitEmail(txt);
            if (emailVal != '') {
                emailVal = emailVal + ',' + matches[1];
            } else {
                emailVal = matches[1];
            }
            $('#<%=txtEmailCc.ClientID %>').val(emailVal);
            $('#<%=hdnEmailCc.ClientID %>').val(emailVal);
        }

        function SetSelectedValueBcc() {
            var emailVal = $('#<%=hdnEmailBCC.ClientID %>').val();
            var txt = $('#<%=txtEmailBCC.ClientID %>').val();
            var matches = splitEmail(txt);
            if (emailVal != '') {
                emailVal = emailVal + ',' + matches[1];
            } else {
                emailVal = matches[1];
            }
            $('#<%=txtEmailBCC.ClientID %>').val(emailVal);
            $('#<%=hdnEmailBCC.ClientID %>').val(emailVal);
        }
        function redisplayAutocompleteExtender() {
            var extender = $find('<%=AutoCompleteExtender1.ClientID%>');
            var ev = { keyCode: 65, preventDefault: function () { }, stopPropagation: function () { } };
            extender._currentPrefix = "";
            extender._onKeyDown.call(extender, ev);
        }
        function redisplayAutocompleteExtenderCC() {
            var extender = $find('<%=AutoCompleteExtender2.ClientID%>');
            var ev = { keyCode: 65, preventDefault: function () { }, stopPropagation: function () { } };
            extender._currentPrefix = "";
            extender._onKeyDown.call(extender, ev);
        }
        function redisplayAutocompleteExtenderBCC() {
            var extender = $find('<%=AutoCompleteExtender3.ClientID%>');
            var ev = { keyCode: 65, preventDefault: function () { }, stopPropagation: function () { } };
            extender._currentPrefix = "";
            extender._onKeyDown.call(extender, ev);
        }

        function ValueBackup() {
            var textbox = document.getElementById('<%= txtEmail.ClientID %>');
            var hidden = document.getElementById('<%= hdnEmail.ClientID %>');
            hidden.value = textbox.value;
        }
        function ValueBackupCC() {
            var textbox = document.getElementById('<%= txtEmailCc.ClientID %>');
            var hidden = document.getElementById('<%= hdnEmailCc.ClientID %>');
            hidden.value = textbox.value;
        }
        function ValueBackupBCC() {
            var textbox = document.getElementById('<%= txtEmailBCC.ClientID %>');
            var hidden = document.getElementById('<%= hdnEmailBCC.ClientID %>');
            hidden.value = textbox.value;
        }
    </script>
    <asp:LinkButton ID="lnkUploadDoc" runat="server" Style="display: none" OnClick="lnkUploadDoc_Click"
        CausesValidation="false"></asp:LinkButton>
    <div id="overlay">
        <img src="images/wheel.GIF" alt="Be patient..." style="position: fixed; margin-top: 25%; margin-left: 50%;" />
    </div>
    <telerik:RadAjaxManager ID="RadAjaxManager_Setup" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="lnkSearch">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadAjaxPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="lnkClear">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadAjaxPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="lnkTo">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadAjaxPanel2" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <div id="dvPreview">
        <div class="divbutton-container">
            <div id="divButtons" class="">
                <div id="breadcrumbs-wrapper">

                    <header>
                        <div class="container row-color-grey">
                            <div class="row">
                                <div class="col s12 m12 l12">
                                    <div class="row">
                                        <div class="page-title"><i class="mdi-communication-contacts"></i>&nbsp;<asp:Label CssClass="title_text" ID="lblHeader" runat="server">Preview Invoice With Ticket</asp:Label></div>

                                        <div class="btnlinks">
                                            <a href="javascript:void(0);" onclick="OpenSendEmailWindow();return false;">Email</a>
                                        </div>

                                        <div class="rght-content">
                                            <div class="editlabel">
                                                <asp:Label ID="lblInv" Visible="false" runat="server"></asp:Label>
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
            </div>
        </div>
        <div class="container">
            <div class="row">
                <div class="srchpane" runat="server" id="divShowItems" visible="false">
                    <div class="srchpaneinner">
                        <div class="srchinputwrap chk-include-zero">
                            <asp:CheckBox runat="server" ID="chkShowItems" Text="Show details invoice item" OnCheckedChanged="chkShowItems_OnCheckedChanged" AutoPostBack="true" CssClass="css-checkbox" />
                        </div>
                    </div>
                </div>
                <div class="grid_container">
                    <div class="form-section-row" style="margin-bottom: 0 !important;">
                        <cc1:StiWebViewer ID="StiWebViewerInvoice" Height="800px" runat="server" ScrollbarsMode="true" CacheMode="None" RequestTimeout="900000" Zoom="100" BackgroundColor="White"
                            OnGetReport="StiWebViewerInvoice_GetReport" OnGetReportData="StiWebViewerInvoice_GetReportData" OnExportReport="StiWebViewerInvoice_ExportReport" ViewMode="Continuous" />
                        <div class="clearfix"></div>
                    </div>
                </div>
            </div>
        </div>

        <div class="clearfix"></div>

        <telerik:RadWindowManager ID="RadWindowManager1" runat="server">
            <Windows>
            </Windows>
        </telerik:RadWindowManager>
        <telerik:RadWindow ID="EmailsSelectionWindow" Skin="Material" VisibleTitlebar="true" Behaviors="Default" CenterIfModal="true"
            Animation="FlyIn" AnimationDuration="200" RenderMode="Auto" VisibleStatusbar="false"
            runat="server" Modal="true" Width="1050" Height="600">
            <ContentTemplate>
                <div>
                    <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" UpdateMode="Conditional">
                        <contenttemplate>
                                <div class="form-section">
                                    <div class="srchpaneinner" style="padding:20px 20px 0px 20px;">
                                        <div class="srchtitle  srchtitlecustomwidth">
                                            Search
                                        </div>

                                        <div class="srchinputwrap">
                                            <asp:DropDownList ID="ddlSearch" runat="server" CssClass="browser-default selectsml selectst">
                                                <asp:ListItem Value="0">Select</asp:ListItem>
                                                <asp:ListItem Value="1">Name</asp:ListItem>
                                                <asp:ListItem Value="2">Email</asp:ListItem>
                                                <asp:ListItem Value="3">Group Name</asp:ListItem>
                                                <asp:ListItem Value="4">Type</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>

                                        <div class="srchinputwrap">
                                            <asp:TextBox ID="txtSearch" runat="server" CssClass="srchcstm" placeholder="Search..."></asp:TextBox>
                                        </div>
                                        <div class="srchinputwrap srchclr btnlinksicon" style="margin-left: -10px; margin-top: -2px;">
                                            <asp:LinkButton ID="lnkSearch" runat="server" CausesValidation="false" ToolTip="Search" OnClick="lnkSearch_Click"><i class="mdi-action-search"></i></asp:LinkButton>
                                        </div>
                                        <div class="btnlinks" style="margin-left:5px;margin-top:10px;">
                                            <a id="aSelectAll" onclick="SetEmails();" href="javascript:void(0)">Save</a>
                                        </div>
                                        <div class="col lblsz2 lblszfloat">
                                            <div class="row">

                                                <span class="tro trost">
                                                    <a id="lnkClear" runat="server" onserverclick="lnkClear_Click">Clear </a>
                                                </span>

                                                <span class="tro trost">
                                                    <asp:Label ID="lblRecordCount" runat="server">0 Record(s) found</asp:Label>
                                                </span>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div style="clear: both;"></div>
                                <div class="form-section-row">
                                    <div class="form-section">
                                        <div class="input-field col s12">
                                            <div class="form-section-row">
                                                <div class="row">
                                                    <div class="input-field col s1" style="margin-top: 0px;">
                                                        <div class="srchtitle  srchtitlecustomwidth btnlinks">
                                                            <a id="aSelectTo" onclick="SelectEmailsFromGrid('inputTo');" href="javascript:void(0)"><span>To &nbsp;</span></a>
                                                        </div>
                                                    </div>
                                                    <div class="input-field col s11" style="margin-top: -10px;">
                                                        <input name="inputTo" type="text" value="" id="inputTo" class="txtUserName form-control validate" />
                                                    </div>
                                                    <div class="input-field col s1"style="margin-top: 0px;">
                                                        <div class="srchtitle  srchtitlecustomwidth btnlinks">
                                                            <a id="aSelectCc" onclick="SelectEmailsFromGrid('inputCc');" href="javascript:void(0)"><span>Cc &nbsp;</span></a>
                                                        </div>
                                                    </div>
                                                    <div class="input-field col s11" style="margin-top: -10px;">
                                                        <input name="inputTo" type="text" value="" id="inputCc" class="txtUserName form-control validate" />
                                                    </div>
                                                    <div class="input-field col s1" style="margin-top: 0px;">
                                                        <div class="srchtitle  srchtitlecustomwidth btnlinks">
                                                            <a id="aSelectBcc" onclick="SelectEmailsFromGrid('inputBcc');" href="javascript:void(0)"><span>Bcc</span></a>
                                                        </div>
                                                    </div>
                                                    <div class="input-field col s11" style="margin-top: -10px;">
                                                        <input name="inputTo" type="text" value="" id="inputBcc" class="txtUserName form-control validate" />
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row" style="margin-bottom: 0;">
                                                <div class="grid_container">
                                                    <div class="RadGrid RadGrid_Material RadGrid">
                                                        <telerik:RadGrid RenderMode="Auto" ID="RadGrid_Emails" AllowFilteringByColumn="true" 
                                                            ShowStatusBar="true" runat="server" AllowSorting="true" Width="100%" FilterType="CheckList" OnPreRender="RadGrid_Emails_PreRender"
                                                            OnNeedDataSource="RadGrid_Emails_NeedDataSource" PagerStyle-AlwaysVisible="true">
                                                            <CommandItemStyle />
                                                            <GroupingSettings CaseSensitive="false" />
                                                            <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                                                                <Selecting AllowRowSelect="True"></Selecting>

                                                                <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                                                <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                                                <%--<Resizing ResizeGridOnColumnResize="True" AllowColumnResize="True"></Resizing>--%>
                                                            </ClientSettings>
                                                            <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="True">
                                                                <Columns>
                                                                    <telerik:GridTemplateColumn AllowFiltering="false" HeaderStyle-Width="28" ShowFilterIcon="false">
                                                                        <ItemTemplate>
                                                                            <asp:CheckBox ID="chkSelect" runat="server" />
                                                                        </ItemTemplate>
                                                                        <HeaderTemplate>
                                                                            <asp:CheckBox ID="chkAll" runat="server" />
                                                                        </HeaderTemplate>
                                                                        <ItemStyle Width="0px"></ItemStyle>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn HeaderStyle-Width="200" DataType="System.String" 
                                                                        DataField="MemberName" SortExpression="MemberName" AutoPostBackOnFilter="true"
                                                                        CurrentFilterFunction="Contains" HeaderText="Name" ShowFilterIcon="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblCName" runat="server"><%#Eval("MemberName")%></asp:Label>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn 
                                                                        DataField="MemberEmail" SortExpression="MemberEmail" AutoPostBackOnFilter="true" DataType="System.String"
                                                                        CurrentFilterFunction="Contains" HeaderText="Member Email" ShowFilterIcon="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblMemberEmail" runat="server"><%#Eval("MemberEmail")%></asp:Label>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn HeaderStyle-Width="200" DataType="System.String" 
                                                                        DataField="GroupName" SortExpression="GroupName" AutoPostBackOnFilter="true"
                                                                        CurrentFilterFunction="Contains" HeaderText="Group Name" ShowFilterIcon="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblGroupName" runat="server"><%#Eval("GroupName")%></asp:Label>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridBoundColumn DataField="Type" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                                                        HeaderText="Type" SortExpression="Type" HeaderStyle-Width="200" DataType="System.String" 
                                                                         ShowFilterIcon="false">
                                                                    </telerik:GridBoundColumn>
                                                                    
                                                                </Columns>
                                                            </MasterTableView>
                                                            <FilterMenu CssClass="RadFilterMenu_CheckList">
                                                            </FilterMenu>
                                                        </telerik:RadGrid>
                                                    </div>
                                                </div>

                                            </div>
                                        </div>
                                    </div>
                                </div>
                               
                            </contenttemplate>
                    </telerik:RadAjaxPanel>
                </div>
            </ContentTemplate>
        </telerik:RadWindow>
    </div>
    <!-- END DASHBOARD STATS -->
    <div class="clearfix"></div>
    <div id="dvEmailOpen" style="display: none;">
        <div class="divbutton-container" runat="server" id="composeHeader">
            <header>
                <div class="container row-color-grey">
                    <div class="row">
                        <div class="col s12 m12 l12">
                            <div class="row">
                                <div class="col s12 m12 l12">
                                    <div class="row">
                                        <div class="page-title">
                                            <i class="mdi-editor-insert-drive-file"></i>&nbsp;
                                                 <asp:Label CssClass="title_text" ID="Label1" runat="server">Send Email</asp:Label>
                                        </div>
                                        <div class="buttonContainer">
                                            <div class="btnlinks">
                                                <asp:LinkButton ID="LnkSend" runat="server"
                                                    OnClick="LnkSend_Click">Send</asp:LinkButton>

                                            </div>
                                        </div>
                                        <div class="buttonContainer">
                                            <div class="btnlinks">
                                                <a href="javascript:void(0);" onclick="return cancel();">Close</a>

                                            </div>
                                        </div>
                                        <div class="btnclosewrap">

                                            <a href="javascript:void(0);" onclick="return cancel();"><i class="mdi-content-clear"></i></a>

                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </header>
        </div>
        <div class="container mailtitcketcontainer" runat="server" id="pnlCompose">
            <div class="row">
                <div class="srchpane-advanced" style="margin: 0 !important;">
                    <div class="srchpaneinner">
                        <div class="form-col">
                            <div class="fc-label">
                                <label>From</label>
                            </div>
                            <div class="fc-input">
                                <asp:TextBox ID="txtEmailFrom" runat="server"
                                    CssClass="form-control" Text="info@mom.com"
                                    TabIndex="9" ToolTip="From" Placeholder="From"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="txtEmailFrom_FilteredTextBoxExtender"
                                    runat="server" Enabled="True" FilterMode="InvalidChars" InvalidChars=" "
                                    TargetControlID="txtEmailFrom">
                                </asp:FilteredTextBoxExtender>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator7" runat="server"
                                    ControlToValidate="txtEmailFrom" Display="None"
                                    ErrorMessage="Invalid Email" SetFocusOnError="True"
                                    ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                                <asp:ValidatorCalloutExtender ID="RegularExpressionValidator7_ValidatorCalloutExtender"
                                    runat="server" Enabled="True"
                                    TargetControlID="RegularExpressionValidator7">
                                </asp:ValidatorCalloutExtender>
                            </div>
                        </div>
                        <div class="form-col">
                            <div class="fc-label">
                                <label>
                                    <asp:HyperLink ID="lnkTo" runat="server" Style="cursor: pointer; text-decoration: underline; color: #105099;"
                                        OnClick="OpenEmailsSelectionWindow_To();return true;" Text="To"></asp:HyperLink>
                                </label>
                            </div>
                            <div class="fc-input">
                                <asp:TextBox ID="txtEmail" runat="server"
                                    CssClass="form-control"
                                    TabIndex="9" ToolTip="To" Placeholder="To"
                                    onfocus="ValueBackup(); redisplayAutocompleteExtender();  "
                                    onclick="ValueBackup(); redisplayAutocompleteExtender(); "
                                    onkeydown="ValueBackup(); redisplayAutocompleteExtender();">
                                </asp:TextBox>
                                <asp:HiddenField ID="hdnEmail" runat="server" Value="" />
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                                    ControlToValidate="txtEmail" Display="None" ErrorMessage="Invalid E-Mail Address"
                                    ValidationExpression="^((\s*[a-zA-Z0-9\._%-]+@[a-zA-Z0-9\.-]+\.[a-zA-Z]{2,6}\s*[,;:]){1,100}?)?(\s*[a-zA-Z0-9\._%-]+@[a-zA-Z0-9\.-]+\.[a-zA-Z]{2,6})*$"
                                    ValidationGroup="mail"></asp:RegularExpressionValidator>

                                <asp:AutoCompleteExtender runat="server" Enabled="True" TargetControlID="txtEmail" ServicePath="CustomerAuto.asmx"
                                    EnableCaching="false" ServiceMethod="GetContactEmails" UseContextKey="True" MinimumPrefixLength="0"
                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListItemCssClass="autocomplete_listItem"
                                    CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" OnClientItemSelected="SetSelectedValue"
                                    ID="AutoCompleteExtender1" DelimiterCharacters="" CompletionInterval="250">
                                </asp:AutoCompleteExtender>
                            </div>
                        </div>
                        <div class="form-col">
                            <div class="fc-label">
                                <label>
                                    <asp:HyperLink ID="lnkCC" runat="server" Style="cursor: pointer; text-decoration: underline; color: #105099;" OnClick="return OpenEmailsSelectionWindow_CC();" Text="Cc"></asp:HyperLink>
                                </label>
                            </div>
                            <div class="fc-input">
                                <asp:TextBox ID="txtEmailCc" runat="server"
                                    CssClass="form-control"
                                    TabIndex="9" ToolTip="Cc" Placeholder="Cc"
                                    onfocus="ValueBackupCC(); redisplayAutocompleteExtenderCC();"
                                    onclick="ValueBackupCC(); redisplayAutocompleteExtenderCC();"
                                    onkeydown="ValueBackupCC(); redisplayAutocompleteExtenderCC();"></asp:TextBox>
                                <asp:HiddenField ID="hdnEmailCc" runat="server" />

                                <asp:RegularExpressionValidator ID="RegularExpressionValidator6" runat="server"
                                    ControlToValidate="txtEmailCc" Display="None" ErrorMessage="Invalid E-Mail Address"
                                    ValidationExpression="^((\s*[a-zA-Z0-9\._%-]+@[a-zA-Z0-9\.-]+\.[a-zA-Z]{2,6}\s*[,;:]){1,100}?)?(\s*[a-zA-Z0-9\._%-]+@[a-zA-Z0-9\.-]+\.[a-zA-Z]{2,6})*$"
                                    ValidationGroup="mail"></asp:RegularExpressionValidator>

                                <asp:AutoCompleteExtender runat="server" Enabled="True" TargetControlID="txtEmailCc" ServicePath="CustomerAuto.asmx"
                                    EnableCaching="false" ServiceMethod="GetContactEmails" UseContextKey="True" MinimumPrefixLength="0"
                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListItemCssClass="autocomplete_listItem"
                                    CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" OnClientItemSelected="SetSelectedValueCc"
                                    ID="AutoCompleteExtender2" DelimiterCharacters="" CompletionInterval="250">
                                </asp:AutoCompleteExtender>
                            </div>
                        </div>
                        <div class="form-col">
                            <div class="fc-label">
                                <label>
                                    <asp:HyperLink ID="lnkBCC" runat="server" Style="cursor: pointer; text-decoration: underline; color: #105099;" OnClick="return OpenEmailsSelectionWindow_BCC();" Text="Bcc"></asp:HyperLink>
                                </label>
                            </div>
                            <div class="fc-input">
                                <asp:TextBox ID="txtEmailBCC" runat="server" CssClass="form-control"
                                    TabIndex="9" ToolTip="Bcc" Placeholder="Bcc"
                                    onfocus="ValueBackupBCC(); redisplayAutocompleteExtenderBCC();"
                                    onclick="ValueBackupBCC(); redisplayAutocompleteExtenderBCC();"
                                    onkeydown="ValueBackupBCC(); redisplayAutocompleteExtenderBCC();"></asp:TextBox>
                                <asp:HiddenField ID="hdnEmailBCC" runat="server" />

                                <asp:RegularExpressionValidator ID="RegularExpressionValidator8" runat="server" ControlToValidate="txtEmailBCC"
                                    Display="None" ErrorMessage="Invalid E-Mail Address" ValidationExpression="^((\s*[a-zA-Z0-9\._%-]+@[a-zA-Z0-9\.-]+\.[a-zA-Z]{2,6}\s*[,;:]){1,100}?)?(\s*[a-zA-Z0-9\._%-]+@[a-zA-Z0-9\.-]+\.[a-zA-Z]{2,6})*$"
                                    ValidationGroup="compose"></asp:RegularExpressionValidator>

                                <asp:AutoCompleteExtender runat="server" Enabled="True" TargetControlID="txtEmailBCC" ServicePath="CustomerAuto.asmx"
                                    EnableCaching="false" ServiceMethod="GetContactEmails" UseContextKey="True" MinimumPrefixLength="0"
                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListItemCssClass="autocomplete_listItem"
                                    CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" OnClientItemSelected="SetSelectedValueBcc"
                                    ID="AutoCompleteExtender3" DelimiterCharacters="" CompletionInterval="250">
                                </asp:AutoCompleteExtender>
                            </div>
                        </div>
                        <div class="form-col">
                            <div class="fc-label">
                                <label>Subject</label>
                            </div>
                            <div class="fc-input">
                                <asp:TextBox ID="txtSubject" runat="server"
                                    CssClass="form-control"
                                    TabIndex="9" ToolTip="Subject" Placeholder="Subject"></asp:TextBox>
                            </div>
                        </div>

                        <div class="form-col">
                            <div class="fc-label">
                                <label>Attachment</label>
                            </div>
                            <div class="fc-input">
                                <asp:FileUpload ID="FileUpload1" runat="server" Width="500px" onchange="ConfirmUpload(this.value);" />
                            </div>
                        </div>
                        <div class="form-col">
                            <div class="fc-label">
                                &nbsp;
                            </div>
                            <div class="fc-input">
                                <ul class="brws-list">
                                    <asp:DataList ID="dlAttachmentsDelete" runat="server" CellPadding="0" CellSpacing="5"
                                        RepeatColumns="5" RepeatDirection="Horizontal">
                                        <ItemTemplate>
                                            <asp:LinkButton CommandArgument='<%# Container.DataItem %>' ID="btnAttachmentDel"
                                                Style="color: #000;" runat="server" OnClick="btnAttachmentDel_Click" Text='<%# System.IO.Path.GetFileName(Container.DataItem.ToString()) %>'></asp:LinkButton>
                                            <asp:ImageButton ID="imgDelAttach" CommandArgument='<%# Container.DataItem %>' Width="12px"
                                                runat="server" OnClick="imgDelAttach_Click" ImageUrl="images/delete-grid.png" />
                                        </ItemTemplate>
                                    </asp:DataList>
                                </ul>
                                <asp:HiddenField ID="hdnFirstAttachement" runat="server" Value="" />
                            </div>
                        </div>

                        <div class="form-col">
                            <div class="fc-label">
                                <label>Body</label>
                            </div>
                            <div class="fc-input">
                                <CKEditor:CKEditorControl ID="txtBodyCKE" runat="server" Width="100%" Height="357" Toolbar="Full"
                                    BasePath="js/ckeditor" TemplatesFiles="js/ckeditor/plugins/templates/templates/default.js"
                                    ContentsCss="js/ckeditor/contents.css" FilebrowserImageUploadUrl="CKimageupload.ashx"
                                    ExtraPlugins="imagepaste,preventdrop">
                                </CKEditor:CKEditorControl>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hdnActiveDiv" runat="server" Value="0" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="javascriptHolder" runat="Server">
    <script type="text/javascript">


        function CloseEmailsSelectionWindow() {
            var wnd = $find('<%=EmailsSelectionWindow.ClientID %>');
            wnd.Close();
        }
        function OpenEmailsSelectionWindow_To() {
            getEmails();
            $("#<%=RadGrid_Emails.ClientID%> input[id*='chkSelect']:checkbox").unbind('click').bind('click', CheckUncheckAllCheckBoxAsNeeded_To);

            $("#<%=RadGrid_Emails.ClientID%> input[id*='chkAll']:checkbox").unbind('click').bind('click', function () {
                if ($(this).is(':checked')) {
                    $("#<%=RadGrid_Emails.ClientID%> input[id*='chkSelect']:checkbox").prop('checked', true);
                }
                else {
                    $("#<%=RadGrid_Emails.ClientID%> input[id*='chkSelect']:checkbox").prop('checked', false);
                }
                CheckEmailsCheckBox('<%=RadGrid_Emails.ClientID%>', '<%=txtEmail.ClientID%>');
            });

            var wnd = $find('<%=EmailsSelectionWindow.ClientID %>');
            wnd.set_title("TO: Email Selection");
            UpdateSelectedRows('<%=RadGrid_Emails.ClientID%>', '<%=txtEmail.ClientID%>');
            wnd.Show();
        }

        function OpenEmailsSelectionWindow_CC() {
            getEmails();
            $("#<%=RadGrid_Emails.ClientID%> input[id*='chkSelect']:checkbox").unbind('click').bind('click', CheckUncheckAllCheckBoxAsNeeded_CC);
            $("#<%=RadGrid_Emails.ClientID%> input[id*='chkAll']:checkbox").unbind('click').bind('click', function () {
                if ($(this).is(':checked')) {
                    $("#<%=RadGrid_Emails.ClientID%> input[id*='chkSelect']:checkbox").prop('checked', true);
                }
                else {
                    $("#<%=RadGrid_Emails.ClientID%> input[id*='chkSelect']:checkbox").prop('checked', false);
                }
                CheckEmailsCheckBox('<%=RadGrid_Emails.ClientID%>', '<%=txtEmailCc.ClientID%>');
            });

            var wnd = $find('<%=EmailsSelectionWindow.ClientID %>');
            wnd.set_title("CC: Email Selection");
            UpdateSelectedRows('<%=RadGrid_Emails.ClientID%>', '<%=txtEmailCc.ClientID%>');
            wnd.Show();
        }

        function OpenEmailsSelectionWindow_BCC() {
            getEmails();
            $("#<%=RadGrid_Emails.ClientID%> input[id*='chkSelect']:checkbox").unbind('click').bind('click', CheckUncheckAllCheckBoxAsNeeded_BCC);
            $("#<%=RadGrid_Emails.ClientID%> input[id*='chkAll']:checkbox").unbind('click').bind('click', function () {
                if ($(this).is(':checked')) {
                    $("#<%=RadGrid_Emails.ClientID%> input[id*='chkSelect']:checkbox").prop('checked', true);
                }
                else {
                    $("#<%=RadGrid_Emails.ClientID%> input[id*='chkSelect']:checkbox").prop('checked', false);
                }
                CheckEmailsCheckBox('<%=RadGrid_Emails.ClientID%>', '<%=txtEmailBCC.ClientID%>');
            });

            var wnd = $find('<%=EmailsSelectionWindow.ClientID %>');
            wnd.set_title("BCC: Email Selection");
            UpdateSelectedRows('<%=RadGrid_Emails.ClientID%>', '<%=txtEmailBCC.ClientID%>');
            wnd.Show();
        }



        function UpdateSelectedRows(gridview, names) {
            var Name = document.getElementById(names);
            var isCheckAll = false;
            var i = 0;
            $("#" + gridview + " input[id*='chkSelect']:checkbox").each(function (index) {
                if ($(this).is(":checked")) {
                    i = i + 1;
                }
            });

            var getCount = $("#" + gridview + " input[id*='chkSelect']:checkbox").length;

            if (getCount == i) {
                isCheckAll = true;
            }

            $("#" + gridview + " input[id*='chkAll']:checkbox").prop('checked', isCheckAll);
        }

        function CheckEmailsCheckBox(gridview, names) {
            var Name = document.getElementById(names);
            var tempArray = [];
            tempArray.length = 0;
            $("#" + gridview + " input[id*='chkSelect']:checkbox").each(function (index) {
                if ($(this).is(":checked")) {
                    tempArray.push($(this).closest('tr').find('td:eq(2)').find('span').html());
                }
            });
            Name.value = tempArray.join(", ");
        }

        function CheckUncheckAllCheckBoxAsNeeded_To() {
            CheckUncheckAllCheckBoxAsNeeded('<%=txtEmail.ClientID%>');
        }

        function CheckUncheckAllCheckBoxAsNeeded_CC() {
            CheckUncheckAllCheckBoxAsNeeded('<%=txtEmailCc.ClientID%>');
        }

        function CheckUncheckAllCheckBoxAsNeeded_BCC() {
            //CheckUncheckAllCheckBoxAsNeeded('<%=txtEmail.ClientID%>');
        }

        function CheckUncheckAllCheckBoxAsNeeded(names) {
            debugger
            var totalCheckboxes = $("#<%=RadGrid_Emails.ClientID%> input[id*='chkSelect']:checkbox").size();

            var checkedCheckboxes = $("#<%=RadGrid_Emails.ClientID%> input[id*='chkSelect']:checkbox:checked").size();

            if (totalCheckboxes == checkedCheckboxes) {

                $("#<%=RadGrid_Emails.ClientID %> input[id*='chkAll']:checkbox").prop('checked', true);//.each(function () { this.checked = true; });
            }
            else {
                $("#<%=RadGrid_Emails.ClientID %> input[id*='chkAll']:checkbox").prop('checked', false);//.attr('checked', false);
            }

            if ($('#<%=RadGrid_Emails.ClientID%>').length > 0) {
                CheckEmailsCheckBox('<%=RadGrid_Emails.ClientID%>', names);
            }
        }

        function SelectEmailsFromGrid(eid) {
            CheckEmailsCheckBox('<%=RadGrid_Emails.ClientID%>', eid);
        }
        function SetEmails() {
            var valTo = $('#inputTo').val();
            var valCc = $('#inputCc').val();
            var valBcc = $('#inputBcc').val();

            if (valTo != "") {
                $('#<%=txtEmail.ClientID%>').val(valTo);
            }
            if (valCc != "") {
                $('#<%=txtEmailCc.ClientID%>').val(valCc);
            }
            if (valBcc != "") {
                $('#<%=txtEmailBCC.ClientID%>').val(valBcc);
            }
            CloseEmailsSelectionWindow();
        }
        function getEmails() {
            $('#inputTo').val($('#<%=txtEmail.ClientID%>').val());
            $('#inputCc').val($('#<%=txtEmailCc.ClientID%>').val());
            $('#inputBcc').val($('#<%=txtEmailBCC.ClientID%>').val());
        }


        function ConfirmUpload(value) {
            var filename;
            var fullPath = value;
            if (fullPath) {
                var startIndex = (fullPath.indexOf('\\') >= 0 ? fullPath.lastIndexOf('\\') : fullPath.lastIndexOf('/'));
                filename = fullPath.substring(startIndex);
                if (filename.indexOf('\\') === 0 || filename.indexOf('/') === 0) {
                    filename = filename.substring(1);
                }
            }

            if (confirm('Attach ' + filename + '?')) {
                document.getElementById('<%= lnkUploadDoc.ClientID %>').click();
            }
        }
    </script>
</asp:Content>
