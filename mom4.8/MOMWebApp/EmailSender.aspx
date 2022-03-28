<%@ Page Title="" Language="C#" MasterPageFile="~/Mom.master" AutoEventWireup="true" Inherits="EmailSender" Codebehind="EmailSender.aspx.cs" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    
    <style>
        .mgn-btm-frm {
            margin-bottom: 10px;
        }

        .frm-title {
            padding-top: 8px;
        }

        .frm-btn-wrap {
            float: left;
            margin-right: 10px;
        }

        .mgn-tp-frm {
            margin-top: 20px;
        }

        .btn-frm-blue {
            height: 40px !important;
            width: 100px !important;
            text-align: center !important;
            border-radius: 4px !important;
        }

        .btn-frm-grey {
            height: 40px !important;
            width: 100px !important;
            text-align: center !important;
            border-radius: 4px !important;
        }

        .list-title {
            float: left;
            margin-right: 20px;
            font-style: italic;
            padding-top: 3px;
            font-size: 1.1em;
        }

        .list-delete {
            float: left;
        }

        ul.brws-list {
            list-style-type: none !important;
            padding: 0;
            margin: 0;
        }

            ul.brws-list li {
                display: block !important;
                float: left !important;
                clear: left !important;
                width: 100% !important;
                margin-bottom: 5px !important;
            }
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

        .ajax__validatorcallout_popup_table{
            width:200px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
     <script type="text/javascript">
        $(function () { 
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
        });
        function splitEmail(txt) {
            var regExp = /\(([^)]+)\)/;
            return regExp.exec(txt);
        }
        function SetSelectedValue() {
            var myVal = document.getElementById('<%=RegularExpressionValidator1.ClientID%>');
            ValidatorEnable(myVal, false); 
            //debugger;
            var emailVal = $('#<%=hdnEmail.ClientID %>').val();
            var txt = $('#<%=txtTo.ClientID %>').val();
            var matches = splitEmail(txt);
            if (emailVal != '') {
                emailVal = emailVal + ',' + matches[1];
            } else {
                emailVal=matches[1];
            }
            $('#<%=txtTo.ClientID %>').val(emailVal);
            $('#<%=hdnEmail.ClientID %>').val(emailVal);
            ValidatorEnable(myVal, true);
        }

        function SetSelectedValueCc() {
            var myVal = document.getElementById('<%=RegularExpressionValidator6.ClientID%>');
            ValidatorEnable(myVal, false);
            var emailVal = $('#<%=hdnEmailCc.ClientID %>').val();
            var txt = $('#<%=txtCC.ClientID %>').val();
            var matches = splitEmail(txt);
            if (emailVal != '') {
                emailVal = emailVal + ',' + matches[1];
            } else {
                emailVal=matches[1];
            }
            $('#<%=txtCC.ClientID %>').val(emailVal);
             $('#<%=hdnEmailCc.ClientID %>').val(emailVal);
             ValidatorEnable(myVal, true);
        }

        function SetSelectedValueBcc() {
            var myVal = document.getElementById('<%=RegularExpressionValidator8.ClientID%>');
            ValidatorEnable(myVal, false);
            var emailVal = $('#<%=hdnEmailBCC.ClientID %>').val();
            var txt = $('#<%=txtBCC.ClientID %>').val();
            var matches = splitEmail(txt);
            if (emailVal != '') {
                emailVal = emailVal + ',' + matches[1];
            } else {
                emailVal=matches[1];
            }
            $('#<%=txtBCC.ClientID %>').val(emailVal);
            $('#<%=hdnEmailBCC.ClientID %>').val(emailVal);
            ValidatorEnable(myVal, true);
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
            var textbox = document.getElementById('<%= txtTo.ClientID %>');
            var hidden = document.getElementById('<%= hdnEmail.ClientID %>');
            hidden.value = textbox.value;
        }
        function ValueBackupCC() {
            var textbox = document.getElementById('<%= txtCC.ClientID %>');
            var hidden = document.getElementById('<%= hdnEmailCc.ClientID %>');
            hidden.value = textbox.value;
        }
        function ValueBackupBCC() {
            var textbox = document.getElementById('<%= txtBCC.ClientID %>');
            var hidden = document.getElementById('<%= hdnEmailBCC.ClientID %>');
            hidden.value = textbox.value;
        }
        function UpdateSelectedRowsForGrid() {
            var wnd = $find('<%=EmailsSelectionWindow.ClientID %>');
            if (wnd != null) {
                if (wnd.get_title() == "TO: Email Selection") {
                    UpdateSelectedRows('<%=RadGrid_Emails.ClientID%>', '<%=txtTo.ClientID%>');
                    $("#<%=RadGrid_Emails.ClientID%> input[id*='chkSelect']:checkbox").unbind('click').bind('click', CheckUncheckAllCheckBoxAsNeeded_To);
                    $("#<%=RadGrid_Emails.ClientID%> input[id*='chkAll']:checkbox").unbind('click').bind('click', function () {
                        if ($(this).is(':checked')) {
                            $("#<%=RadGrid_Emails.ClientID%> input[id*='chkSelect']:checkbox").prop('checked', true);
                        }
                        else {
                            $("#<%=RadGrid_Emails.ClientID%> input[id*='chkSelect']:checkbox").prop('checked', false);
                        }
                        CheckEmailsCheckBox('<%=RadGrid_Emails.ClientID%>', '<%=txtTo.ClientID%>');
                    });
                } else if (wnd.get_title() == "CC: Email Selection") {
                    UpdateSelectedRows('<%=RadGrid_Emails.ClientID%>', '<%=txtCC.ClientID%>');
                    $("#<%=RadGrid_Emails.ClientID%> input[id*='chkSelect']:checkbox").unbind('click').bind('click', CheckUncheckAllCheckBoxAsNeeded_CC);
                    $("#<%=RadGrid_Emails.ClientID%> input[id*='chkAll']:checkbox").unbind('click').bind('click', function () {
                        if ($(this).is(':checked')) {
                            $("#<%=RadGrid_Emails.ClientID%> input[id*='chkSelect']:checkbox").prop('checked', true);
                        }
                        else {
                            $("#<%=RadGrid_Emails.ClientID%> input[id*='chkSelect']:checkbox").prop('checked', false);
                        }
                        CheckEmailsCheckBox('<%=RadGrid_Emails.ClientID%>', '<%=txtCC.ClientID%>');
                    });
                } else if (wnd.get_title() == "BCC: Email Selection") {

                }
            }
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
                    <telerik:AjaxUpdatedControl ControlID="RadAjaxPanel1"/>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="lnkClear">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadAjaxPanel1"/>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="radOpen">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadAjaxPanel1"/>
                </UpdatedControls>
            </telerik:AjaxSetting>
             <telerik:AjaxSetting AjaxControlID="RadGrid_Emails">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadAjaxPanel1"/>
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanelAE" runat="server">
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
                                            <div class="page-title">
                                                <i class="mdi-editor-insert-drive-file"></i>&nbsp;
                                                 <asp:Label CssClass="title_text" ID="lblHeader" runat="server">Send Email</asp:Label>
                                            </div>
                                            <div class="buttonContainer">
                                                <div class="btnlinks">
                                                    <asp:LinkButton ID="btnSendMail" runat="server" Text="Send" OnClick="btnSendMail_Click" ValidationGroup="compose" />
                                                </div>
                                            </div>
                                            <div class="buttonContainer">
                                                <div class="btnlinks">
                                                    <asp:LinkButton ID="btnDiscard" runat="server" Text="Clear" CausesValidation="false" OnClick="btnDiscard_Click" />
                                                </div>
                                            </div>
                                            <div class="btnclosewrap">
                                                <asp:LinkButton runat="server" OnClick="lnkClose_Click" OnClientClick="return confirm('Do you want to discard the mail?');" ID="lnkClose" ToolTip="Close">
                                                    <i class="mdi-content-clear"></i>
                                                </asp:LinkButton>
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


    <div class="container mailtitcketcontainer">
        <div class="row">
            <div class="srchpane-advanced" style="margin: 0 !important;">
                <div class="srchpaneinner">
                    <div class="form-col">
                        <div class="fc-label">
                            <label>From</label>
                        </div>
                        <div class="fc-input">
                            <asp:TextBox ID="txtFrom" runat="server" ToolTip="From"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtFrom"
                                Display="None" ErrorMessage="Email Required" SetFocusOnError="True" ValidationGroup="compose"></asp:RequiredFieldValidator>
                            <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender1" runat="server" Enabled="True"
                                PopupPosition="Right" TargetControlID="RequiredFieldValidator1">
                            </asp:ValidatorCalloutExtender>
                            <asp:FilteredTextBoxExtender ID="txtEmailFrom_FilteredTextBoxExtender" runat="server"
                                Enabled="True" FilterMode="InvalidChars" InvalidChars=" " TargetControlID="txtFrom">
                            </asp:FilteredTextBoxExtender>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator7" runat="server" ControlToValidate="txtFrom"
                                Display="None" ErrorMessage="Invalid Email" SetFocusOnError="True" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                ValidationGroup="compose" Enabled="False"></asp:RegularExpressionValidator>
                            <asp:ValidatorCalloutExtender ID="RegularExpressionValidator7_ValidatorCalloutExtender"
                                runat="server" Enabled="True" TargetControlID="RegularExpressionValidator7">
                            </asp:ValidatorCalloutExtender>
                        </div>
                    </div>
                    <div class="form-col">
                        <div class="fc-label">
                            <label>
                                <%--<a href="javascript:void(0);" id="aOpenEmailContacts">To</a>--%>
                                <%--<asp:HyperLink ID="lnkTo" runat="server" Style="cursor: pointer; text-decoration: underline;" OnClick="OpenEmailsSelectionWindow();" Text="To"></asp:HyperLink>--%>

                                <asp:HyperLink ID="lnkTo" runat="server" Style="cursor: pointer; text-decoration: underline; color: #105099;" 
                                                        OnClick="OpenEmailsSelectionWindow_To();return true;" Text="To" 
                                                        ></asp:HyperLink>
                            <%--<asp:Button runat="server" ID="radOpen" OnClick="radOpen_Click" style ="display:none;" />--%>
                            </label>
                        </div>
                        <div class="fc-input">
                            <asp:TextBox ID="txtTo" runat="server" ToolTip="To" Placeholder="To"
                                onfocus="ValueBackup(); redisplayAutocompleteExtender();  "
                                    onclick="ValueBackup(); redisplayAutocompleteExtender(); "
                                    onkeydown="ValueBackup(); redisplayAutocompleteExtender();"></asp:TextBox>
                            <asp:HiddenField ID="hdnEmail" runat="server" value=""/>
                            <asp:FilteredTextBoxExtender ID="txtEmail_FilteredTextBoxExtender" runat="server"
                                Enabled="True" FilterMode="InvalidChars" InvalidChars=" " TargetControlID="txtTo">
                            </asp:FilteredTextBoxExtender>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="txtTo"
                                Display="None" Enabled="True" ErrorMessage="Email Required" SetFocusOnError="True" ValidationGroup="compose"></asp:RequiredFieldValidator>
                            <asp:ValidatorCalloutExtender ID="RequiredFieldValidator9_ValidatorCalloutExtender"
                                runat="server" Enabled="True" PopupPosition="BottomLeft" TargetControlID="RequiredFieldValidator9">
                            </asp:ValidatorCalloutExtender>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtTo"
                                Display="None" Enabled="True" ErrorMessage="Invalid E-Mail Address" ValidationExpression="^((\s*[a-zA-Z0-9\._%-]+@[a-zA-Z0-9\.-]+\.[a-zA-Z]{2,6}\s*[,;:]){1,100}?)?(\s*[a-zA-Z0-9\._%-]+@[a-zA-Z0-9\.-]+\.[a-zA-Z]{2,6})*$"
                                ValidationGroup="compose"></asp:RegularExpressionValidator>
                            <asp:ValidatorCalloutExtender ID="RegularExpressionValidator1_ValidatorCalloutExtender"
                                runat="server" Enabled="True" PopupPosition="BottomLeft" TargetControlID="RegularExpressionValidator1">
                            </asp:ValidatorCalloutExtender>
                            <asp:AutoCompleteExtender runat="server" Enabled="True" TargetControlID="txtTo" ServicePath="CustomerAuto.asmx"
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
                            <asp:HyperLink ID="lnkCC" runat="server" Style="cursor: pointer; text-decoration: underline;color:#105099;" OnClick="return OpenEmailsSelectionWindow_CC();" Text="Cc"></asp:HyperLink>
                            </label>
                                </div>
                        <div class="fc-input">
                            <asp:TextBox ID="txtCC" runat="server" ToolTip="Cc" Placeholder="Cc"
                                onfocus="ValueBackupCC(); redisplayAutocompleteExtenderCC();"
                                    onclick="ValueBackupCC(); redisplayAutocompleteExtenderCC();"
                                    onkeydown="ValueBackupCC(); redisplayAutocompleteExtenderCC();"></asp:TextBox>
                            <asp:HiddenField ID="hdnEmailCc" runat="server" />
                            <asp:FilteredTextBoxExtender ID="txtEmailCc_FilteredTextBoxExtender" runat="server"
                                Enabled="True" FilterMode="InvalidChars" InvalidChars=" " TargetControlID="txtCC">
                            </asp:FilteredTextBoxExtender>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator6" runat="server" ControlToValidate="txtCC"
                                Display="None" ErrorMessage="Invalid E-Mail Address" ValidationExpression="^((\s*[a-zA-Z0-9\._%-]+@[a-zA-Z0-9\.-]+\.[a-zA-Z]{2,6}\s*[,;:]){1,100}?)?(\s*[a-zA-Z0-9\._%-]+@[a-zA-Z0-9\.-]+\.[a-zA-Z]{2,6})*$"
                                ValidationGroup="compose"></asp:RegularExpressionValidator>
                            <asp:ValidatorCalloutExtender ID="RegularExpressionValidator6_ValidatorCalloutExtender"
                                runat="server" Enabled="True" PopupPosition="BottomLeft" TargetControlID="RegularExpressionValidator6">
                            </asp:ValidatorCalloutExtender>
                            <asp:AutoCompleteExtender runat="server" Enabled="True" TargetControlID="txtCC" ServicePath="CustomerAuto.asmx"
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
                            <asp:HyperLink ID="lnkBCC" runat="server" Style="cursor: pointer; text-decoration: underline;color:#105099;" OnClick="return OpenEmailsSelectionWindow_BCC();" Text="Bcc"></asp:HyperLink>
                            </label>
                                </div>
                        <div class="fc-input">
                            <asp:TextBox ID="txtBCC" runat="server" ToolTip="Bcc" Placeholder="Bcc"
                                onfocus="ValueBackupBCC(); redisplayAutocompleteExtenderBCC();"
                                    onclick="ValueBackupBCC(); redisplayAutocompleteExtenderBCC();"
                                    onkeydown="ValueBackupBCC(); redisplayAutocompleteExtenderBCC();"></asp:TextBox>
                            <asp:HiddenField ID="hdnEmailBCC" runat="server" />
                            <asp:FilteredTextBoxExtender ID="txtEmailBCC_FilteredTextBoxExtender" runat="server"
                                Enabled="True" FilterMode="InvalidChars" InvalidChars=" " TargetControlID="txtBCC">
                            </asp:FilteredTextBoxExtender>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator8" runat="server" ControlToValidate="txtBCC"
                                Display="None" ErrorMessage="Invalid E-Mail Address" ValidationExpression="^((\s*[a-zA-Z0-9\._%-]+@[a-zA-Z0-9\.-]+\.[a-zA-Z]{2,6}\s*[,;:]){1,100}?)?(\s*[a-zA-Z0-9\._%-]+@[a-zA-Z0-9\.-]+\.[a-zA-Z]{2,6})*$"
                                ValidationGroup="compose"></asp:RegularExpressionValidator>
                            <asp:ValidatorCalloutExtender ID="RegularExpressionValidator8_ValidatorCalloutExtender"
                                runat="server" Enabled="True" PopupPosition="BottomLeft" TargetControlID="RegularExpressionValidator8">
                            </asp:ValidatorCalloutExtender>
                            <asp:AutoCompleteExtender runat="server" Enabled="True" TargetControlID="txtBCC" ServicePath="CustomerAuto.asmx"
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
                            <asp:TextBox ID="txtSubject" runat="server" ToolTip="Subject" Placeholder="Subject"></asp:TextBox>
                        </div>
                    </div>

                    <div class="form-col">
                        <div class="fc-label">
                            <label>Attachment</label>
                        </div>
                        <div class="fc-input">
                            <asp:FileUpload ID="flpFile" runat="server" Width="500px" onchange="ConfirmUpload(this.value);"/>
                        </div>
                    </div>
                    <div class="form-col">
                        <div class="fc-label">
                            &nbsp;
                        </div>
                        <div class="fc-input">
                            <ul class="brws-list">
                               <table cellspacing="5" cellpadding="0" border="0" style="width:auto;">
	                            <tbody><tr>
                                <asp:Repeater runat="server" ID="rptAttachments" OnItemCommand="rptAttachments_ItemCommand">
                                    <ItemTemplate>
		                                    <td>
                                            <%--<div class="list-title">--%>
                                                <asp:LinkButton runat="server" style="color:#000;" ID="lnkFileNameDownload" CommandArgument='<%#Eval("ID")%>' OnClick="lnkFileNameDownload_Click">
                                                              <%# Eval("FileName").ToString()%>
                                                </asp:LinkButton>
                                            <%--</div>
                                            <div class="list-delete">--%>
                                               <%-- <asp:LinkButton ID="lnkFileName" runat="server" CommandName="Delete" CausesValidation="false" CommandArgument='<%#Eval("ID")%>' Width="12px">
                                                             <img src="images/delete-grid.png" />
                                                </asp:LinkButton>--%>
                                                <asp:ImageButton ID="lnkFileName" runat="server" CommandName="Delete" CausesValidation="false" CommandArgument='<%#Eval("ID")%>' Width="12px" ImageUrl="images/delete-grid.png" />
                                           </td>
                                                <%-- </div>--%>
                                    </ItemTemplate>
                                </asp:Repeater>
                                    </tr></tbody>
                                   </table>
                            </ul>
                        </div>
                    </div>

                    <div class="form-col">
                        <div class="fc-label">
                            <label>Body</label>
                        </div>
                        <div class="fc-input">
                            <CKEditor:CKEditorControl ID="txtBody" runat="server" Width="100%" Height="357" Toolbar="Full"
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

    <asp:Label runat="server" ForeColor="Red" ID="lblError"></asp:Label>

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
                                                    </ClientSettings>
                                                    <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="True" DataKeyNames="MemberEmail">
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
                                                            <telerik:GridTemplateColumn HeaderStyle-Width="200"
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
                                                            <telerik:GridTemplateColumn HeaderStyle-Width="200"
                                                                DataField="GroupName" SortExpression="GroupName" AutoPostBackOnFilter="true"
                                                                CurrentFilterFunction="Contains" HeaderText="Group Name" ShowFilterIcon="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblGroupName" runat="server"><%#Eval("GroupName")%></asp:Label>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderStyle-Width="150"
                                                                DataField="Type" SortExpression="Type" AutoPostBackOnFilter="true"
                                                                CurrentFilterFunction="Contains" HeaderText="Type" ShowFilterIcon="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblType" runat="server"><%#Eval("Type")%></asp:Label>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <%--<telerik:GridBoundColumn DataField="MemberEmail" AutoPostBackOnFilter="true" ShowFilterIcon="false" CurrentFilterFunction="Contains"
                                                                HeaderText="Email" SortExpression="MemberEmail"
                                                                UniqueName="MemberEmail">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="GroupName" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                                                HeaderText="Group Name" SortExpression="GroupName" HeaderStyle-Width="200"
                                                                UniqueName="GroupName" ShowFilterIcon="false">
                                                            </telerik:GridBoundColumn>--%>
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
                        <%--<div style="clear: both;"></div>
                        <footer style="float: left; padding-left: 0 !important;">
                            <div class="btnlinks">
                                <asp:LinkButton ID="lnkPopupOK" runat="server" ValidationGroup="popupShutdownReasonDesc" >OK</asp:LinkButton>
                            </div>
                        </footer>--%>
                    </contenttemplate>
                </telerik:RadAjaxPanel>
            </div>
        </ContentTemplate>
    </telerik:RadWindow>

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
                CheckEmailsCheckBox('<%=RadGrid_Emails.ClientID%>', '<%=txtTo.ClientID%>');
            });

            var wnd = $find('<%=EmailsSelectionWindow.ClientID %>');
            wnd.set_title("TO: Email Selection");
            UpdateSelectedRows('<%=RadGrid_Emails.ClientID%>', '<%=txtTo.ClientID%>');
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
                CheckEmailsCheckBox('<%=RadGrid_Emails.ClientID%>', '<%=txtCC.ClientID%>');
            });

            var wnd = $find('<%=EmailsSelectionWindow.ClientID %>');
            wnd.set_title("CC: Email Selection");
            UpdateSelectedRows('<%=RadGrid_Emails.ClientID%>', '<%=txtCC.ClientID%>');
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
                CheckEmailsCheckBox('<%=RadGrid_Emails.ClientID%>', '<%=txtBCC.ClientID%>');
            });

            var wnd = $find('<%=EmailsSelectionWindow.ClientID %>');
            wnd.set_title("BCC: Email Selection");
            UpdateSelectedRows('<%=RadGrid_Emails.ClientID%>', '<%=txtBCC.ClientID%>');
            wnd.Show();
        }

        <%--function UpdateSelectedRowsForGrid() {
            var wnd = $find('<%=EmailsSelectionWindow.ClientID %>');
            if (wnd.get_title() == "To Emails Selection") {
                UpdateSelectedRows('<%=RadGrid_Emails.ClientID%>', '<%=txtTo.ClientID%>');
                $("#<%=RadGrid_Emails.ClientID%> input[id*='chkSelect']:checkbox").unbind('click').bind('click', CheckUncheckAllCheckBoxAsNeeded_To);
                $("#<%=RadGrid_Emails.ClientID%> input[id*='chkAll']:checkbox").unbind('click').bind('click', function () {
                    if ($(this).is(':checked')) {
                        $("#<%=RadGrid_Emails.ClientID%> input[id*='chkSelect']:checkbox").prop('checked', true);
                    }
                    else {
                        $("#<%=RadGrid_Emails.ClientID%> input[id*='chkSelect']:checkbox").prop('checked', false);
                    }
                    CheckEmailsCheckBox('<%=RadGrid_Emails.ClientID%>', '<%=txtTo.ClientID%>');
                });
            } else if (wnd.get_title() == "CC Emails Selection") {
                UpdateSelectedRows('<%=RadGrid_Emails.ClientID%>', '<%=txtCC.ClientID%>');
                $("#<%=RadGrid_Emails.ClientID%> input[id*='chkSelect']:checkbox").unbind('click').bind('click', CheckUncheckAllCheckBoxAsNeeded_CC);
                $("#<%=RadGrid_Emails.ClientID%> input[id*='chkAll']:checkbox").unbind('click').bind('click', function () {
                    if ($(this).is(':checked')) {
                        $("#<%=RadGrid_Emails.ClientID%> input[id*='chkSelect']:checkbox").prop('checked', true);
                    }
                    else {
                        $("#<%=RadGrid_Emails.ClientID%> input[id*='chkSelect']:checkbox").prop('checked', false);
                    }
                    CheckEmailsCheckBox('<%=RadGrid_Emails.ClientID%>', '<%=txtCC.ClientID%>');
                });
            } else if (wnd.get_title() == "BCC Emails Selection") {

            }
        }--%>

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
                if ($(this).is(":checked"))
                {
                    tempArray.push($(this).closest('tr').find('td:eq(2)').find('span').html());
                }
            });
            Name.value = tempArray.join(", ");
        }

        function CheckUncheckAllCheckBoxAsNeeded_To() {
            CheckUncheckAllCheckBoxAsNeeded('<%=txtTo.ClientID%>');
        }

        function CheckUncheckAllCheckBoxAsNeeded_CC() {
            CheckUncheckAllCheckBoxAsNeeded('<%=txtCC.ClientID%>');
        }

        function CheckUncheckAllCheckBoxAsNeeded_BCC() {
            CheckUncheckAllCheckBoxAsNeeded('<%=txtBCC.ClientID%>');
        }

        function CheckUncheckAllCheckBoxAsNeeded(names) {
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
                $('#<%=txtTo.ClientID%>').val(valTo);
            }
            if (valCc != "") {
                $('#<%=txtCC.ClientID%>').val(valCc);
            }
            if (valBcc != "") {
                $('#<%=txtBCC.ClientID%>').val(valBcc);
            }
            CloseEmailsSelectionWindow();
        }
        function getEmails() {
            $('#inputTo').val($('#<%=txtTo.ClientID%>').val());
            $('#inputCc').val($('#<%=txtCC.ClientID%>').val());
            $('#inputBcc').val($('#<%=txtBCC.ClientID%>').val());
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

            if (confirm('Attach ' + filename + '?'))
            {
               document.getElementById('<%= lnkUploadDoc.ClientID %>').click();
            }
        }

    </script>
</asp:Content>

