<%@ Page Title="" Language="C#" MasterPageFile="~/Mom.master" EnableEventValidation="false" AutoEventWireup="true" Inherits="IncomeStatement" CodeBehind="IncomeStatement.aspx.cs" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="Stimulsoft.Report.WebDesign" Namespace="Stimulsoft.Report.Web" TagPrefix="cc1" %>

<%@ Register Assembly="Stimulsoft.Report.Web" Namespace="Stimulsoft.Report.Web" TagPrefix="cc1" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <!--Grid Control-->
    <link href="Design/css/grid.css" rel="stylesheet" />

    <link href="Design/css/pikaday.css" rel="stylesheet" />

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

        .ajax__validatorcallout_popup_table {
            width: 300px;
        }
    </style>

    <script type="text/javascript">
        $(document).ready(function () {
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

            hideShowPanels();
        });

        function showMailReport() {
            $('#mainWindow').attr('style', 'display:none');
            $('#dvEmailOpen').attr('style', 'display:block');

            var val = $('#<%=ddlReport.ClientID%> :selected').val();
            if (val == '4' || val == '6' || val == '7' || val == '8' || val == '9') {
                if ($('#<%=hdnFirstAttachement.ClientID%>').val() == 'IncomeStatement.pdf') {
                    $('#ctl00_ContentPlaceHolder1_dlAttachmentsDelete').find('tr').find('td').html('');
                    noty({ text: 'No attachment available.', type: 'warning', layout: 'topCenter', closeOnSelfClick: false, timeout: false, theme: 'noty_theme_default', closable: true });
                }
            }
        }

        function cancel() {
            $('#mainWindow').attr('style', 'display:block');
            $('#dvEmailOpen').attr('style', 'display:none');
            return false;
        }

        function hideShowPanels() {
            var selectedVal = $('#<%=ddlReport.ClientID%> option:selected').attr('value');
            $('#divOfficeCenterSelect').hide();
            $('#divDetailOrSummary').show();
            $('#divDetailOrSummary').show();
            $('.divbudgets').hide();
            $('#<%=btnSearch.ClientID%>').show();
            $('#<%=btnSearch1.ClientID%>').hide();

            // Checkbox Include Zero Balance Accounts
            if (selectedVal != "3" && selectedVal != "4" && selectedVal != "6" && selectedVal != "8") {
                $('.chk-include-zero').show();
            }
            else {
                $('.chk-include-zero').hide();
            }

            if (selectedVal == "0" || selectedVal == "7") {
                $('.standard-profit-loss').show()
                $('.12-period').hide()
                $('.budgets').hide()
                $('.profit-loss-report').show()
                $('.divcenter').hide()
            }
            else if (selectedVal == "2" || selectedVal == "5") {
                $('.standard-profit-loss').hide()
                $('.budgets').show()
                $('.12-period').hide()
                $('.profit-loss-report').hide()
                $('#<%=btnSearch.ClientID%>').hide();
                $('#<%=btnSearch1.ClientID%>').show();
            }
            else if (selectedVal == "3") {
                $('.standard-profit-loss').hide()
                $('#divDetailOrSummary').hide()
                $('.budgets').show()
                $('.divbudgetcenter').show()
                $('.12-period').hide()
                $('.profit-loss-report').hide()
                $('#<%=btnSearch.ClientID%>').hide();
                $('#<%=btnSearch1.ClientID%>').show();
            }
            else if (selectedVal == "4") {
                $('.standard-profit-loss').show()
                $('.12-period').hide()
                $('.budgets').hide()
                $('.profit-loss-report').show()
                $('.divcenter').show()
            }
            else if (selectedVal == "8") {
                $('.standard-profit-loss').show()
                $('.12-period').hide()
                $('.budgets').hide()
                $('.profit-loss-report').show()
                $('.divcenter').show()
                $('.divbudgets').show();
            }
            else if (selectedVal == "6") {
                $('.standard-profit-loss').show();
                $('.12-period').hide();
                $('.budgets').hide();
                $('#divDetailOrSummary').hide();
                $('.profit-loss-report').show();
                $('.divcenter').show();
                $('#divOfficeCenterSelect').show();
            }
            else {
                $('.standard-profit-loss').hide()
                $('.12-period').show()
                $('.budgets').hide()
                $('.profit-loss-report').show()
            }
        }
    </script>

    <script type="text/javascript">
        //Sys.Application.add_init(appl_init);

        //function appl_init() {
        //    var pgRegMgr = Sys.WebForms.PageRequestManager.getInstance();
        //    pgRegMgr.add_beginRequest(BlockUI);
        //    pgRegMgr.add_endRequest(UnblockUI);
        //}

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
            var txt = $('#<%=txtTo.ClientID %>').val();
            var matches = splitEmail(txt);
            if (emailVal != '') {
                emailVal = emailVal + ',' + matches[1];
            } else {
                emailVal = matches[1];
            }
            $('#<%=txtTo.ClientID %>').val(emailVal);
            $('#<%=hdnEmail.ClientID %>').val(emailVal);
        }

        function SetSelectedValueCc() {
            var emailVal = $('#<%=hdnEmailCc.ClientID %>').val();
            var txt = $('#<%=txtCC.ClientID %>').val();
            var matches = splitEmail(txt);
            if (emailVal != '') {
                emailVal = emailVal + ',' + matches[1];
            } else {
                emailVal = matches[1];
            }
            $('#<%=txtCC.ClientID %>').val(emailVal);
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
            var textbox = document.getElementById('<%= txtEmailBCC.ClientID %>');
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="overlay">
        <img src="images/wheel.GIF" alt="Be patient..." style="position: fixed; margin-top: 25%; margin-left: 50%;" />
    </div>
    <div id="mainWindow">
        <div class="divbutton-container">
            <div id="divButtons">
                <div id="breadcrumbs-wrapper">
                    <header>
                        <div class="col s12 m12 l12">
                            <div class="row">
                                <div class="page-title"><i class="mdi-action-swap-vert-circle"></i>&nbsp; Profit and Loss</div>
                                <div class="buttonContainer">
                                    <div class="btnlinks">
                                        <a id="mailReport" href="javascript:void(0);" onclick="showMailReport();return false;">Email</a>
                                    </div>

                                    <ul class="nomgn hideMenu menuList">
                                        <li>
                                            <asp:Label CssClass="title_text" ID="lblHeader" runat="server"></asp:Label></li>
                                        <li>
                                            <asp:LinkButton CssClass="icon-closed" ID="lnkClose" runat="server" CausesValidation="false" ToolTip="close"
                                                OnClick="lnkClose_Click"></asp:LinkButton></li>
                                    </ul>
                                </div>
                                <div class="btnclosewrap">
                                    <a href="Home.aspx"><i class="mdi-content-clear"></i></a>
                                </div>
                                <div class="rght-content">
                                </div>
                            </div>
                        </div>
                    </header>
                </div>
            </div>
        </div>
        <telerik:RadAjaxManager ID="RadAjaxManager_Budget" runat="server">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="lnkSearch">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="RadAjaxPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="datePickerFromDate" EventName="SelectedDateChanged">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="drpBudgetsList" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="datePickerToDate" EventName="SelectedDateChanged">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="drpBudgetsList" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="txtFromDt" EventName="TextChanged">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="drpBudgetsList" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="txtToDt" EventName="TextChanged">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="drpBudgetsList" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="txtStartDate" EventName="TextChanged">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="rcBudget" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="txtEndDate" EventName="TextChanged">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="rcBudget" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="drpBudgetsList" EventName="SelectedIndexChanged">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="StiWebViewerBudgetVsActual" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
        <div class="container">
            <div class="row">
                <div class="srchpane">
                    <div class="srchpaneinner">
                        <div class="srchtitle srchtitlecustomwidth" style="padding-left: 15px;">
                            Report
                        </div>
                        <div class="srchinputwrap">
                            <asp:DropDownList ID="ddlReport" runat="server" CssClass="browser-default selectsml selectst" TabIndex="1" Style="min-width: 360px !important;" onchange="hideShowPanels();">
                                <asp:ListItem Value="0">Standard Income Statement </asp:ListItem>
                                <asp:ListItem Value="7">Standard Income Statement With YTD</asp:ListItem>
                                <asp:ListItem Value="4">Standard Income Statement With Center</asp:ListItem>
                                <asp:ListItem Value="8">Standard Income Statement With Center With Budgets</asp:ListItem>
                                <asp:ListItem Value="1">12 Period Report </asp:ListItem>
                                <asp:ListItem Value="2">Actual vs Budget Standard Report With Variance</asp:ListItem>
                                <asp:ListItem Value="5">Actual vs Budget Standard Report Without Variance</asp:ListItem>
                                <asp:ListItem Value="3">Actual vs Budget 12 Period Report</asp:ListItem>
                                <asp:ListItem Value="6">Standard Income Statement Comparative FS with Center</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="srchinputwrap chk-include-zero" style="display: none;">
                            <div class="rdleftmgn">
                                <asp:CheckBox runat="server" ID="chkIncludeZero" Text="Include Zero Balance Accounts" CssClass="css-checkbox" />
                            </div>
                        </div>
                        <div class="srchinputwrap rdleftmgn" id="divDetailOrSummary" style="display: none;">
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
                        <div class="srchinputwrap btnlinksicon srchclr">
                            <asp:LinkButton ID="btnSearch" runat="server" CausesValidation="true" ToolTip="Refresh" ValidationGroup="search"
                                OnClick="btnSearch_Click"><i class="fa fa-refresh"></i></asp:LinkButton>
                            <asp:LinkButton ID="btnSearch1" runat="server" CausesValidation="true" ToolTip="Refresh" ValidationGroup="search1"
                                OnClick="btnSearch_Click" Style="display: none;"><i class="fa fa-refresh"></i></asp:LinkButton>
                        </div>
                    </div>

                    <div class="srchpaneinner 12-period" style="display: none;">
                        <div class="srchtitle srchtitlecustomwidth 12-period" style="padding-left: 15px;">
                            Year Ending
                        </div>
                        <div class="srchinputwrap">
                            <asp:Label ID="lblYearEnd" Style="margin-left: 39px; margin-bottom: 19px; display: none;" runat="server" Text="Year Ending" Visible="False" class="12-period"></asp:Label>
                            <asp:TextBox ID="txtYearEnd" runat="server" CssClass="12-period" Style="display: none;"
                                MaxLength="50" autocomplete="off"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True"
                                TargetControlID="txtYearEnd">
                            </asp:CalendarExtender>
                            <asp:RegularExpressionValidator ID="revYearEnd" ControlToValidate="txtYearEnd" ValidationGroup="search"
                                ValidationExpression="^(0?[1-9]|1[012])[\/\-](0?[1-9]|[12][0-9]|3[01])[\/\-]\d{4}$"
                                runat="server" ErrorMessage="Invalid Date format. Valid Date Format 01/12/2001" Display="None">
                            </asp:RegularExpressionValidator>
                            <asp:ValidatorCalloutExtender ID="vceYearEnd" runat="server" Enabled="True" CssClass="form-control 12-period"
                                PopupPosition="Right" TargetControlID="revYearEnd" />

                        </div>
                    </div>
                    <div class="srchpaneinner standard-profit-loss">
                        <asp:UpdatePanel ID="updcsa" runat="server" UpdateMode="Always">
                            <ContentTemplate>
                                <div class="srchtitle srchtitlecustomwidth" style="padding-left: 15px;">
                                    Start Date
                                </div>
                                <div class="srchinputwrap">
                                    <asp:TextBox ID="txtStartDate" runat="server" placeholder="Start" OnTextChanged="txtStartDate_TextChanged" AutoPostBack="true"
                                        MaxLength="50" autocomplete="off"></asp:TextBox>
                                    <asp:CalendarExtender ID="txtStartDate_CalendarExtender" runat="server" Enabled="True"
                                        TargetControlID="txtStartDate">
                                    </asp:CalendarExtender>
                                    <asp:RequiredFieldValidator ID="rfvStartDt"
                                        runat="server" ControlToValidate="txtStartDate" Display="None" ErrorMessage="Start date is required" ValidationGroup="search"
                                        SetFocusOnError="True"></asp:RequiredFieldValidator>
                                    <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender1" runat="server" Enabled="True"
                                        PopupPosition="Right" TargetControlID="rfvStartDt" />
                                    <asp:RegularExpressionValidator ID="rfvStartDt1" ControlToValidate="txtStartDate" ValidationGroup="search"
                                        ValidationExpression="^(0?[1-9]|1[012])[\/\-](0?[1-9]|[12][0-9]|3[01])[\/\-]\d{4}$"
                                        runat="server" ErrorMessage="Invalid Date format. Valid Date Format 01/12/2001" Display="None">
                                    </asp:RegularExpressionValidator>
                                    <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender2" runat="server" Enabled="True" PopupPosition="Right"
                                        TargetControlID="rfvStartDt1" />

                                </div>
                                <div class="srchtitle srchtitlecustomwidth" style="padding-left: 15px;">
                                    End Date
                                </div>
                                <div class="srchinputwrap">
                                    <asp:Label ID="lblEnd" Style="margin-left: 39px; margin-bottom: 19px; display: none;" runat="server" Text="End" Visible="false"></asp:Label>
                                    <asp:TextBox ID="txtEndDate" runat="server" CssClass="form-control" OnTextChanged="txtEndDate_TextChanged" AutoPostBack="true"
                                        MaxLength="50" Width="130px" autocomplete="off"></asp:TextBox>
                                    <asp:CalendarExtender ID="txtEndDate_CalendarExtender" runat="server" Enabled="True"
                                        TargetControlID="txtEndDate">
                                    </asp:CalendarExtender>
                                    <asp:RequiredFieldValidator ID="rfvEndDt"
                                        runat="server" ControlToValidate="txtEndDate" Display="None" ErrorMessage="End date is required" ValidationGroup="search"
                                        SetFocusOnError="True"></asp:RequiredFieldValidator>
                                    <asp:ValidatorCalloutExtender ID="vceEndDt" runat="server" Enabled="True"
                                        PopupPosition="Right" TargetControlID="rfvEndDt" />
                                    <asp:RegularExpressionValidator ID="rfvEndDt1" ControlToValidate="txtEndDate" ValidationGroup="search"
                                        ValidationExpression="^(0?[1-9]|1[012])[\/\-](0?[1-9]|[12][0-9]|3[01])[\/\-]\d{4}$"
                                        runat="server" ErrorMessage="Invalid Date format. Valid Date Format 01/12/2001" Display="None">
                                    </asp:RegularExpressionValidator>
                                    <asp:ValidatorCalloutExtender ID="vceEndDt1" runat="server" Enabled="True" PopupPosition="Right"
                                        TargetControlID="rfvEndDt1" />
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>

                        <div class="srchinputwrap">
                            <ul class="tabselect accrd-tabselect" id="testradiobutton">
                                <li>
                                    <asp:LinkButton AutoPostBack="False" ID="LinkButton1" runat="server" Text="<i class='mdi-hardware-keyboard-arrow-left'></i>" OnClientClick="dec_date('dec','ctl00_ContentPlaceHolder1_txtEndDate','ctl00_ContentPlaceHolder1_txtStartDate','rdCal');return false;"></asp:LinkButton>
                                </li>
                                <li>
                                    <label id="lblDay" runat="server">
                                        <input type="radio" id="rdDay" name="rdCal" value="rdDay" onclick="SelectDate('Day', 'ctl00_ContentPlaceHolder1_txtStartDate', 'ctl00_ContentPlaceHolder1_txtEndDate', '#lblDay', 'hdnTicketListDate', 'rdCal')" />
                                        Day
                                    </label>
                                </li>
                                <li>
                                    <label id="lblWeek" runat="server">
                                        <input type="radio" id="rdWeek" name="rdCal" value="rdWeek" onclick="SelectDate('Week', 'ctl00_ContentPlaceHolder1_txtStartDate', 'ctl00_ContentPlaceHolder1_txtEndDate', '#ctl00_ContentPlaceHolder1_lblWeek', 'hdnTicketListDate', 'rdCal')" />
                                        Week
                                    </label>
                                </li>
                                <li>
                                    <label id="lblMonth" runat="server">
                                        <input type="radio" id="rdMonth" name="rdCal" value="rdMonth" onclick="SelectDate('Month', 'ctl00_ContentPlaceHolder1_txtStartDate', 'ctl00_ContentPlaceHolder1_txtEndDate', '#ctl00_ContentPlaceHolder1_lblMonth', 'hdnTicketListDate', 'rdCal')" />
                                        Month
                                    </label>
                                </li>
                                <li>
                                    <label id="lblQuarter" runat="server">
                                        <input type="radio" id="rdQuarter" name="rdCal" value="rdQuarter" onclick="SelectDate('Quarter', 'ctl00_ContentPlaceHolder1_txtStartDate', 'ctl00_ContentPlaceHolder1_txtEndDate', '#ctl00_ContentPlaceHolder1_lblQuarter', 'hdnTicketListDate', 'rdCal')" />
                                        Quarter
                                    </label>
                                </li>
                                <li>
                                    <label id="lblYear" runat="server">
                                        <input type="radio" id="rdYear" name="rdCal" value="rdYear" onclick="SelectDate('Year', 'ctl00_ContentPlaceHolder1_txtStartDate', 'ctl00_ContentPlaceHolder1_txtEndDate', '#ctl00_ContentPlaceHolder1_lblYear', 'hdnTicketListDate', 'rdCal')" />
                                        Year
                                    </label>
                                </li>
                                <li>
                                    <asp:LinkButton ID="incDate" runat="server" OnClientClick="dec_date('inc','ctl00_ContentPlaceHolder1_txtEndDate','ctl00_ContentPlaceHolder1_txtStartDate','rdCal');return false" Text="<i class='mdi-hardware-keyboard-arrow-right'></i>"></asp:LinkButton>
                                </li>
                            </ul>
                        </div>

                        <div class="divcenter" style="display: none;">
                            <div class="srchtitle srchtitlecustomwidth" style="padding-left: 15px;">
                                Center
                            </div>
                            <div class="srchinputwrap">
                                <telerik:RadComboBox RenderMode="Auto" Skin="Metro" ID="rcCenter" runat="server" Filter="StartsWith" CheckBoxes="true" EnableCheckAllItemsCheckBox="true"
                                    EmptyMessage="--Select Type--" CssClass="inGrid">
                                </telerik:RadComboBox>
                            </div>
                        </div>

                        <div class="divbudgets" style="display: none;">
                            <div class="srchtitle srchtitlecustomwidth" style="padding-left: 15px;">
                                Budget
                            </div>
                                <div class="srchinputwrap">
                                    <telerik:RadComboBox RenderMode="Auto" Skin="Metro" ID="rcBudget" runat="server" Filter="StartsWith" CheckBoxes="true" EnableCheckAllItemsCheckBox="true"
                                        EmptyMessage="--Select Budget--" CssClass="inGrid">
                                    </telerik:RadComboBox>
                                </div>
                        </div>

                        <div class="srchinputwrap rdleftmgn" id="divOfficeCenterSelect" style="display: none;">
                            <div class="srchtitle srchtitlecustomwidth" style="padding-left: 15px;">
                                Office Center
                            </div>
                            <asp:DropDownList ID="ddlOfficeCenter" runat="server" CssClass="browser-default selectsml selectst" TabIndex="1" Style="min-width: 60px !important;">
                            </asp:DropDownList>
                        </div>

                    </div>
                    <div class="budgets" style="display: none; margin-bottom: 15px;">
                        <div class="srchpaneinner">
                            <div class="srchtitle srchtitlecustomwidth" style="padding-left: 15px;">
                                Start Date
                            </div>
                            <div class="srchinputwrap">
                                <asp:Label ID="lblStartDate" Style="margin-left: 39px; margin-bottom: 19px;" runat="server" Text="Start" Visible="false"></asp:Label>
                                <asp:TextBox ID="txtFromDt" runat="server" Width="150px" CssClass="browser-default" MaxLength="20" OnTextChanged="txtFromDt_TextChanged" AutoPostBack="true"
                                    TabIndex="4"></asp:TextBox>
                                <asp:CalendarExtender ID="txtFromDt_CalendarExtender" runat="server" Enabled="True"
                                    TargetControlID="txtFromDt">
                                </asp:CalendarExtender>
                                <asp:RequiredFieldValidator ID="rfvFromDt"
                                    runat="server" ControlToValidate="txtFromDt" Display="None" ErrorMessage="Start date is required" ValidationGroup="search1"
                                    SetFocusOnError="True"></asp:RequiredFieldValidator>
                                <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender6" runat="server" Enabled="True"
                                    PopupPosition="Right" TargetControlID="rfvFromDt" />
                                <asp:RegularExpressionValidator ID="rfvFromDt1" ControlToValidate="txtFromDt" ValidationGroup="search1"
                                    ValidationExpression="^(0?[1-9]|1[012])[\/\-](0?[1-9]|[12][0-9]|3[01])[\/\-]\d{4}$"
                                    runat="server" ErrorMessage="Invalid Date format. Valid Date Format 01/12/2001" Display="None">
                                </asp:RegularExpressionValidator>
                                <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender7" runat="server" Enabled="True" PopupPosition="Right"
                                    TargetControlID="rfvFromDt1" />
                            </div>

                            <asp:Label ID="lblEndDate" Style="margin-left: 39px; margin-bottom: 19px;" runat="server" Text="End" Visible="false"></asp:Label>
                            <div class="srchtitle srchtitlecustomwidth" style="padding-left: 15px;">
                                End Date
                            </div>
                            <div class="srchinputwrap">
                                <asp:TextBox ID="txtToDt" runat="server" Width="150px" CssClass="browser-default" MaxLength="20" OnTextChanged="txtToDt_TextChanged" AutoPostBack="true"
                                    TabIndex="4"></asp:TextBox>
                                <asp:CalendarExtender ID="txtToDt_CalendarExtender" runat="server" Enabled="True"
                                    TargetControlID="txtToDt">
                                </asp:CalendarExtender>
                                <asp:RequiredFieldValidator ID="rfvToDt"
                                    runat="server" ControlToValidate="txtToDt" Display="None" ErrorMessage="End date is required" ValidationGroup="search1"
                                    SetFocusOnError="True"></asp:RequiredFieldValidator>
                                <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender3" runat="server" Enabled="True"
                                    PopupPosition="Right" TargetControlID="rfvToDt" />
                                <asp:RegularExpressionValidator ID="rfvToDt1" ControlToValidate="txtToDt" ValidationGroup="search1"
                                    ValidationExpression="^(0?[1-9]|1[012])[\/\-](0?[1-9]|[12][0-9]|3[01])[\/\-]\d{4}$"
                                    runat="server" ErrorMessage="Invalid Date format. Valid Date Format 01/12/2001" Display="None">
                                </asp:RegularExpressionValidator>
                                <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender5" runat="server" Enabled="True" PopupPosition="Right"
                                    TargetControlID="rfvToDt1" />
                            </div>

                            <div class="divbudgetcenter" style="display: none;">
                                <div class="srchtitle srchtitlecustomwidth" style="padding-left: 15px;">
                                    Center
                                </div>
                                <div class="srchinputwrap">
                                    <telerik:RadComboBox RenderMode="Auto" Skin="Metro" ID="rcCenter1" runat="server" Filter="StartsWith" CheckBoxes="true" EnableCheckAllItemsCheckBox="true"
                                        EmptyMessage="--Select Type--" CssClass="inGrid">
                                    </telerik:RadComboBox>
                                </div>
                            </div>

                            <div class="srchtitle srchtitlecustomwidth" style="padding-left: 15px;">
                                Budget
                            </div>
                            <div class="srchinputwrap">
                                <asp:DropDownList ID="drpBudgetsList" Visible="false" runat="server" CssClass="browser-default inGrid" OnSelectedIndexChanged="drpBudgetsList_SelectedIndexChanged" AutoPostBack="true" Width="200px"></asp:DropDownList>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="grid_container">
                    <div class="form-section-row" style="margin-bottom: 0 !important;">
                        <cc1:StiWebViewer ID="StiWebViewerBudgetVsActual" Height="800px" RequestTimeout="20000" runat="server" ViewMode="Continuous" ScrollbarsMode="true"
                            OnGetReport="StiWebViewerBudgetVsActual_GetReport" OnGetReportData="StiWebViewerBudgetVsActual_GetReportData" Visible="false" />
                        <cc1:StiWebViewer ID="StiWebViewerBudgetVsActual2" runat="server" ScrollbarsMode="true" RequestTimeout="900000"
                            OnGetReport="StiWebViewerBudgetVsActual2_GetReport" OnGetReportData="StiWebViewerBudgetVsActual2_GetReportData" ViewMode="Continuous" Visible="false" />
                        <cc1:StiWebViewer ID="StiWebViewerIncomeStatemnet" Height="800px" runat="server" ScrollbarsMode="true" RequestTimeout="20000"
                            OnGetReport="StiWebViewerIncomeStatemnet_GetReport" OnGetReportData="StiWebViewerIncomeStatemnet_GetReportData" Visible="false" ViewMode="Continuous" />
                        <cc1:StiWebViewer ID="StiWebViewerIncomeStatement12Period" RequestTimeout="20000" Height="800px" runat="server" ViewMode="Continuous" ScrollbarsMode="true"
                            OnGetReport="StiWebViewerIncomeStatement12Period_GetReport" OnGetReportData="StiWebViewerIncomeStatement12Period_GetReportData" Visible="false" />
                        <cc1:StiWebViewer ID="StiWebViewerIncomeStatementWithCenters" Height="800px" runat="server" ScrollbarsMode="true" RequestTimeout="900000"
                            OnGetReport="StiWebViewerIncomeStatementWithCenters_GetReport" OnGetReportData="StiWebViewerIncomeStatementWithCenters_GetReportData" Visible="false" ViewMode="Continuous" />
                        <cc1:StiWebViewer ID="StiWebViewerIncomeStatementWithCentersBudgets" Height="800px" runat="server" ScrollbarsMode="true" RequestTimeout="900000"
                            OnGetReport="StiWebViewerIncomeStatementWithCentersBudgets_GetReport" OnGetReportData="StiWebViewerIncomeStatementWithCentersBudgets_GetReportData" Visible="false" ViewMode="Continuous" />
                        <cc1:StiWebViewer ID="StiWebViewerStandardIncomeStatementComparativeFsWithCenter" Height="800px" runat="server" ScrollbarsMode="true" RequestTimeout="900000"
                            OnGetReport="StiWebViewerStandardIncomeStatementComparativeFsWithCenter_GetReport" OnGetReportData="StiWebViewerStandardIncomeStatementComparativeFsWithCenter_GetReportData"
                            Visible="false" ViewMode="Continuous" OnExportReport="StiWebViewerStandardIncomeStatementComparativeFsWithCenter_ExportReport" />
                        <cc1:StiWebViewer ID="StiWebViewerProfitAndLossYTD" Height="800px" RequestTimeout="9000000" runat="server" ViewMode="Continuous" ScrollbarsMode="true"
                            OnGetReport="StiWebViewerProfitAndLossYTD_GetReport" OnGetReportData="StiWebViewerProfitAndLossYTD_GetReportData" Visible="false" />
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div id="dvEmailOpen" style="display: none;">
        <div class="divbutton-container">
            <header>
                <div class="container row-color-grey">
                    <div class="row">
                        <div class="col s12 m12 l12">
                            <div class="row">
                                <div class="col s12 m12 l12">
                                    <div class="row">
                                        <div class="page-title">
                                            <i class="mdi-action-swap-vert-circle"></i>&nbsp; Profit and Loss
                                        </div>
                                        <div class="buttonContainer">
                                            <div class="btnlinks">
                                                <asp:LinkButton runat="server" ID="LnkSend" Text="Send" OnClick="LnkSend_Click" ValidationGroup="mail" />
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
                                <asp:TextBox ID="txtFrom" runat="server" CssClass="form-control"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="txtFrom_FilteredTextBoxExtender"
                                    runat="server" Enabled="True" FilterMode="InvalidChars" InvalidChars=" "
                                    TargetControlID="txtFrom">
                                </asp:FilteredTextBoxExtender>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server"
                                    ControlToValidate="txtFrom" Display="None"
                                    ErrorMessage="Invalid E-Mail Address"
                                    ValidationExpression="^((\s*[a-zA-Z0-9\._%-]+@[a-zA-Z0-9\.-]+\.[a-zA-Z]{2,6}\s*[,;:]){1,100}?)?(\s*[a-zA-Z0-9\._%-]+@[a-zA-Z0-9\.-]+\.[a-zA-Z]{2,6})*$"
                                    ValidationGroup="mail"></asp:RegularExpressionValidator>
                                <asp:ValidatorCalloutExtender ID="RegularExpressionValidator3_ValidatorCalloutExtender"
                                    runat="server" Enabled="True" TargetControlID="RegularExpressionValidator3">
                                </asp:ValidatorCalloutExtender>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                                    ControlToValidate="txtFrom" Display="None"
                                    ErrorMessage="Please Enter E-Mail Address" SetFocusOnError="True"
                                    ValidationGroup="mail"></asp:RequiredFieldValidator>
                                <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender4"
                                    runat="server" Enabled="True" TargetControlID="RequiredFieldValidator2">
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
                                <asp:TextBox ID="txtTo" runat="server" CssClass="form-control" Placeholder="To"
                                    onfocus="ValueBackup(); redisplayAutocompleteExtender();  "
                                    onclick="ValueBackup(); redisplayAutocompleteExtender(); "
                                    onkeydown="ValueBackup(); redisplayAutocompleteExtender();">
                                </asp:TextBox>
                                <asp:HiddenField ID="hdnEmail" runat="server" Value="" />
                                <asp:FilteredTextBoxExtender ID="txtTo_FilteredTextBoxExtender" runat="server"
                                    Enabled="True" FilterMode="InvalidChars" InvalidChars=" "
                                    TargetControlID="txtTo">
                                </asp:FilteredTextBoxExtender>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                                    ControlToValidate="txtTo" Display="None" ErrorMessage="Invalid E-Mail Address"
                                    ValidationExpression="^((\s*[a-zA-Z0-9\._%-]+@[a-zA-Z0-9\.-]+\.[a-zA-Z]{2,6}\s*[,;:]){1,100}?)?(\s*[a-zA-Z0-9\._%-]+@[a-zA-Z0-9\.-]+\.[a-zA-Z]{2,6})*$"
                                    ValidationGroup="mail"></asp:RegularExpressionValidator>
                                <asp:ValidatorCalloutExtender ID="RegularExpressionValidator1_ValidatorCalloutExtender"
                                    runat="server" Enabled="True" TargetControlID="RegularExpressionValidator1">
                                </asp:ValidatorCalloutExtender>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                                    ControlToValidate="txtTo" Display="None"
                                    ErrorMessage="Please Enter E-Mail Address" SetFocusOnError="True"
                                    ValidationGroup="mail"></asp:RequiredFieldValidator>
                                <asp:ValidatorCalloutExtender ID="RequiredFieldValidator1_ValidatorCalloutExtender"
                                    runat="server" Enabled="True" TargetControlID="RequiredFieldValidator1">
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
                                    <asp:HyperLink ID="lnkCC" runat="server" Style="cursor: pointer; text-decoration: underline; color: #105099;" OnClick="return OpenEmailsSelectionWindow_CC();" Text="Cc"></asp:HyperLink>
                                </label>
                            </div>
                            <div class="fc-input">
                                <asp:TextBox ID="txtCC" runat="server" CssClass="form-control" Placeholder="Cc"
                                    onfocus="ValueBackupCC(); redisplayAutocompleteExtenderCC();"
                                    onclick="ValueBackupCC(); redisplayAutocompleteExtenderCC();"
                                    onkeydown="ValueBackupCC(); redisplayAutocompleteExtenderCC();"></asp:TextBox>
                                <asp:HiddenField ID="hdnEmailCc" runat="server" />
                                <asp:FilteredTextBoxExtender ID="txtCC_FilteredTextBoxExtender" runat="server"
                                    Enabled="True" FilterMode="InvalidChars" InvalidChars=" "
                                    TargetControlID="txtCC">
                                </asp:FilteredTextBoxExtender>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server"
                                    ControlToValidate="txtCC" Display="None" ErrorMessage="Invalid E-Mail Address"
                                    ValidationExpression="^((\s*[a-zA-Z0-9\._%-]+@[a-zA-Z0-9\.-]+\.[a-zA-Z]{2,6}\s*[,;:]){1,100}?)?(\s*[a-zA-Z0-9\._%-]+@[a-zA-Z0-9\.-]+\.[a-zA-Z]{2,6})*$"
                                    ValidationGroup="mail"></asp:RegularExpressionValidator>
                                <asp:ValidatorCalloutExtender ID="RegularExpressionValidator2_ValidatorCalloutExtender"
                                    runat="server" Enabled="True" TargetControlID="RegularExpressionValidator2">
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
                                <%--<asp:FilteredTextBoxExtender ID="txtEmailBCC_FilteredTextBoxExtender" runat="server"
                                    Enabled="True" FilterMode="InvalidChars" InvalidChars=" " TargetControlID="txtEmailBCC">
                                </asp:FilteredTextBoxExtender>--%>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator8" runat="server" ControlToValidate="txtEmailBCC"
                                    Display="None" ErrorMessage="Invalid E-Mail Address" ValidationExpression="^((\s*[a-zA-Z0-9\._%-]+@[a-zA-Z0-9\.-]+\.[a-zA-Z]{2,6}\s*[,;:]){1,100}?)?(\s*[a-zA-Z0-9\._%-]+@[a-zA-Z0-9\.-]+\.[a-zA-Z]{2,6})*$"
                                    ValidationGroup="compose"></asp:RegularExpressionValidator>
                                <%--<asp:ValidatorCalloutExtender ID="RegularExpressionValidator8_ValidatorCalloutExtender"
                                    runat="server" Enabled="True" TargetControlID="RegularExpressionValidator8">
                                </asp:ValidatorCalloutExtender>--%>
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
    </div>
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
                              
                    </contenttemplate>
                </telerik:RadAjaxPanel>
            </div>
        </ContentTemplate>
    </telerik:RadWindow>

    <asp:LinkButton ID="lnkUploadDoc" runat="server" Style="display: none" OnClick="lnkUploadDoc_Click"
        CausesValidation="false"></asp:LinkButton>
    <asp:HiddenField runat="server" ID="hdnReportSelectDtRange" Value="" />
    <asp:HiddenField runat="server" ID="hdnCssActive" Value="1" />
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

        function OpenSendEmailWindow() {
            $('#dvPreview').attr('style', 'display:none');
            $('#dvEmailOpen').attr('style', 'display:block');
            <%--var wnd = $find('<%=RadCreateWindow.ClientID %>');
            wnd.Show();--%>
        }

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
                CheckEmailsCheckBox('<%=RadGrid_Emails.ClientID%>', '<%=txtEmailBCC.ClientID%>');
            });

            var wnd = $find('<%=EmailsSelectionWindow.ClientID %>');
            wnd.set_title("BCC: Email Selection");
            UpdateSelectedRows('<%=RadGrid_Emails.ClientID%>', '<%=txtEmailBCC.ClientID%>');
            wnd.Show();
        }

        <%--function UpdateSelectedRowsForGrid() {
            debugger
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
                if ($(this).is(":checked")) {
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
            //CheckUncheckAllCheckBoxAsNeeded('<%=txtTo.ClientID%>');
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
                $('#<%=txtEmailBCC.ClientID%>').val(valBcc);
            }
            CloseEmailsSelectionWindow();
        }
        function getEmails() {
            $('#inputTo').val($('#<%=txtTo.ClientID%>').val());
            $('#inputCc').val($('#<%=txtCC.ClientID%>').val());
            $('#inputBcc').val($('#<%=txtEmailBCC.ClientID%>').val());
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
                $('#<%=txtEmailBCC.ClientID%>').val(valBcc);
            }
            CloseEmailsSelectionWindow();
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

    <script type="text/javascript">
        $(document).ready(function () {

            $('label input[type=radio]').click(function () {
                $('input[name="' + this.name + '"]').each(function () {
                    $(this.parentNode).toggleClass('labelactive', this.checked);
                });
            });
            if (typeof (Storage) !== "undefined") {
                // Retrieve
                var SesVar = '<%= Convert.ToString(Session["lblReportDateActive"])%>';
                var val = '<%= Convert.ToString(Session["DateRangeType"]) %>';

                if (SesVar == '2') {
                    $("#<%=lblDay.ClientID%>").addClass("");
                    $("#<%=lblWeek.ClientID%>").addClass("");
                    $("#<%=lblMonth.ClientID%>").addClass("");
                    $("#<%=lblQuarter.ClientID%>").addClass("");
                    $("#<%=lblYear.ClientID%>").addClass("");
                }
                else {
                    if (val == 'Day') {
                        $("#<%=lblDay.ClientID%>").addClass("labelactive");
                        document.getElementById("rdDay").checked = true;
                    }
                    else if (val == 'Week') {
                        $("#<%=lblWeek.ClientID%>").addClass("labelactive");
                        document.getElementById("rdWeek").checked = true;
                    }
                    else if (val == 'Month') {
                        $("#<%=lblMonth.ClientID%>").addClass("labelactive");
                        document.getElementById("rdMonth").checked = true;
                    }
                    else if (val == 'Quarter') {
                        $("#<%=lblQuarter.ClientID%>").addClass("labelactive");
                        document.getElementById("rdQuarter").checked = true;
                    }
                    else if (val == 'Year') {
                        $("#<%=lblYear.ClientID%>").addClass("labelactive");
                        document.getElementById("rdYear").checked = true;
                    }
                }
            }
        });
    </script>
    <script type="text/javascript">
        function dec_date(select, txtDateTo, txtDateFrom, rdGroup) {
            var select = select;
            var txtDateTo = txtDateTo;
            var txtDateFrom = txtDateFrom;
            var rdGroup = rdGroup;
            var xday;
            var xWeek;
            var xMonth;
            var xYear;
            var xQuarter;
            if (select == "dec") {
                xday = -1;
                xWeek = -7;
                xMonth = -1;
                xQuarter = -3;
                xYear = -1;
            }
            if (select == "inc") {

                xday = 1;
                xWeek = 7;
                xMonth = 1;
                xQuarter = 3;
                xYear = 1;
            }

            var selected = '<%= Convert.ToString(Session["DateRangeType"]) %>';
            if (document.getElementById('<%= hdnReportSelectDtRange.ClientID%>').value) {
                selected = document.getElementById('<%= hdnReportSelectDtRange.ClientID%>').value;
            }

            if (selected == 'Day') {
                //dec the from date 
                var tt = document.getElementById(txtDateFrom).value;
                var date = new Date(tt);
                var newdate = new Date(date);

                newdate.setDate(newdate.getDate() + xday);

                var dd = newdate.getDate();
                var mm = newdate.getMonth() + 1;
                var y = newdate.getFullYear();

                var someFormattedDate = mm + '/' + dd + '/' + y;
                document.getElementById(txtDateFrom).value = someFormattedDate;
                //dec the to date 

                var TT = document.getElementById(txtDateTo).value;
                var DATE = new Date(TT);
                var NEWDATE = new Date(DATE);

                NEWDATE.setDate(NEWDATE.getDate() + xday);

                var DD = NEWDATE.getDate();
                var MM = NEWDATE.getMonth() + 1;
                var Y = NEWDATE.getFullYear();
                var someFormattedDATE = MM + '/' + DD + '/' + Y;

                document.getElementById(txtDateTo).value = someFormattedDATE;
            }
            else if (selected == 'Week') {
                //dec the from date 
                var tt = document.getElementById(txtDateFrom).value;
                var date = new Date(tt);
                var newdate = new Date(date);

                newdate.setDate(newdate.getDate() + xWeek);

                var dd = newdate.getDate();
                var mm = newdate.getMonth() + 1;
                var y = newdate.getFullYear();

                var someFormattedDate = mm + '/' + dd + '/' + y;
                document.getElementById(txtDateFrom).value = someFormattedDate;
                //dec the to date 

                var TT = document.getElementById(txtDateTo).value;
                var DATE = new Date(TT);
                var NEWDATE = new Date(DATE);

                NEWDATE.setDate(NEWDATE.getDate() + xWeek);

                var DD = NEWDATE.getDate();
                var MM = NEWDATE.getMonth() + 1;
                var Y = NEWDATE.getFullYear();
                var someFormattedDATE = MM + '/' + DD + '/' + Y;

                document.getElementById(txtDateTo).value = someFormattedDATE;
            }
            else if (selected == 'Month') {
                //dec the from date
                Date.isLeapYear = function (year) {
                    return (((year % 4 === 0) && (year % 100 !== 0)) || (year % 400 === 0));
                };

                Date.getDaysInMonth = function (year, month) {
                    return [31, (Date.isLeapYear(year) ? 29 : 28), 31, 30, 31, 30, 31, 31, 30, 31, 30, 31][month];
                };

                Date.prototype.isLeapYear = function () {
                    return Date.isLeapYear(this.getFullYear());
                };

                Date.prototype.getDaysInMonth = function () {
                    return Date.getDaysInMonth(this.getFullYear(), this.getMonth());
                };

                Date.prototype.addMonths = function (value) {
                    var n = this.getDate();
                    this.setDate(1);
                    this.setMonth(this.getMonth() + value);
                    this.setDate(Math.min(n, this.getDaysInMonth()));
                    return this;
                };

                Date.prototype.addMonthsLast = function (value) {
                    var n = this.getDate();
                    this.setDate(1);
                    this.setMonth(this.getMonth() + 1);
                    if (this.getDaysInMonth() == 31) {
                        this.setDate(Math.max(n, this.getDaysInMonth()));
                    }
                    else {
                        this.setDate(Math.min(n, this.getDaysInMonth()));
                    }

                    return this;
                };

                Date.prototype.addMonthsLastDec = function (value) {
                    var n = this.getDate();
                    this.setDate(1);
                    this.setMonth(this.getMonth() - 1);
                    if (this.getDaysInMonth() == 31) {
                        this.setDate(Math.max(n, this.getDaysInMonth()));
                    }
                    else {
                        this.setDate(Math.min(n, this.getDaysInMonth()));
                    }

                    return this;
                };
                var tt = document.getElementById(txtDateFrom).value;

                var date = new Date(tt).toDateString();
                var newdate = new Date(date);

                newdate.addMonths(xMonth);

                var dd = newdate.getDate();
                var mm = newdate.getMonth() + 1;
                var y = newdate.getFullYear();
                var someFormattedDate = mm + '/' + dd + '/' + y;

                document.getElementById(txtDateFrom).value = someFormattedDate;

                //dec the to date 
                if (select == 'dec') {
                    var ti = document.getElementById(txtDateTo).value;
                    var date = new Date(ti).toDateString();
                    var newdateti = new Date(date);
                    newdateti.addMonthsLastDec(xMonth);

                    var dd = newdateti.getDate();
                    var mm = newdateti.getMonth() + 1;
                    var y = newdateti.getFullYear();
                    var someFormattedDate = mm + '/' + dd + '/' + y;

                    document.getElementById(txtDateTo).value = someFormattedDate;
                }

                else {
                    var ti = document.getElementById(txtDateTo).value;

                    var date = new Date(ti).toDateString();
                    var newdateti = new Date(date);
                    newdateti.addMonthsLast(xMonth);

                    var dd = newdateti.getDate();
                    var mm = newdateti.getMonth() + 1;
                    var y = newdateti.getFullYear();
                    var someFormattedDate = mm + '/' + dd + '/' + y;

                    document.getElementById(txtDateTo).value = someFormattedDate;
                }
            }
            else if (selected == 'Quarter') {
                //dec the from date 
                var tt = document.getElementById(txtDateFrom).value;
                var date = new Date(tt);
                var newdate = new Date(date);

                newdate.setMonth(newdate.getMonth() + xQuarter);

                var dd = newdate.getDate();
                var mm = newdate.getMonth() + 1;
                var y = newdate.getFullYear();

                var someFormattedDate = mm + '/' + dd + '/' + y;
                document.getElementById(txtDateFrom).value = someFormattedDate;

                //dec the to date 
                var TT = document.getElementById(txtDateTo).value;
                var DATE = new Date(TT);
                var NEWDATE = new Date(DATE);

                //decrease date range 
                if (select == 'dec') {
                    xQuarter = -3;

                    if (DATE.getMonth() == 11) {
                        NEWDATE.setDate(NEWDATE.getDate() - 1);
                        NEWDATE.setMonth(NEWDATE.getMonth() + xQuarter);
                    }
                    else if (DATE.getMonth() == 5) {
                        NEWDATE.setMonth(NEWDATE.getMonth() + xQuarter);
                        NEWDATE.setDate(NEWDATE.getDate() + 1);
                    }
                    else {
                        NEWDATE.setMonth(NEWDATE.getMonth() + xQuarter);
                    }
                }
                else {
                    xQuarter = 3;
                    NEWDATE.setDate(NEWDATE.getDate() - 1);
                    NEWDATE.setMonth(NEWDATE.getMonth() + xQuarter);
                    if (NEWDATE.getMonth() == 11 || NEWDATE.getMonth() == 12 || DATE.getMonth() == 11) {
                        NEWDATE.setDate(31);
                    } else {
                        if (DATE.getMonth() == 5) { NEWDATE.setDate(NEWDATE.getDate() + 1); }
                        else { NEWDATE.setDate(NEWDATE.getDate()); }
                    }
                }

                var DD = NEWDATE.getDate();
                var MM = NEWDATE.getMonth() + 1;
                var Y = NEWDATE.getFullYear();
                var someFormattedDATE = MM + '/' + DD + '/' + Y;

                document.getElementById(txtDateTo).value = someFormattedDATE;
            }
            else if (selected == 'Year') {
                var tt = document.getElementById(txtDateFrom).value;
                var date = new Date(tt);
                var newdate = new Date(date);

                newdate.setFullYear(newdate.getFullYear() + xYear);

                var dd = newdate.getDate();
                var mm = newdate.getMonth() + 1;
                var y = newdate.getFullYear();
                var someFormattedDate = mm + '/' + dd + '/' + y;

                document.getElementById(txtDateFrom).value = someFormattedDate;
                //dec the to date 

                var TT = document.getElementById(txtDateTo).value;
                var DATE = new Date(TT);
                var NEWDATE = new Date(DATE);

                NEWDATE.setFullYear(NEWDATE.getFullYear() + xYear);

                var DD = NEWDATE.getDate();
                var MM = NEWDATE.getMonth() + 1;
                var Y = NEWDATE.getFullYear();
                var someFormattedDATE = MM + '/' + DD + '/' + Y;

                document.getElementById(txtDateTo).value = someFormattedDATE;
            }

            var selectedVal = $('#<%=ddlReport.ClientID%> option:selected').attr('value');

            if (selectedVal == "8") {
                $('#ctl00_ContentPlaceHolder1_txtStartDate').trigger('change');
            }

            return false;
        }

        function SelectDate(type, txtDateFrom, txtdateTo, label, UniqueVal, rdGroup) {
            var type = type;
            var txtDateFrom = txtDateFrom;
            var txtdateTo = txtdateTo;
            var UniqueVal = UniqueVal;
            var label = label;
            if (type == 'Day') {
                var todaydate = new Date();
                var day = todaydate.getDate();
                var month = todaydate.getMonth() + 1;
                var year = todaydate.getFullYear();
                var datestring = month + "/" + day + "/" + year;
                document.getElementById(txtdateTo).value = datestring;
                document.getElementById(txtDateFrom).value = datestring;
                $(label).addClass("labelactive");
                document.getElementById('<%= hdnReportSelectDtRange.ClientID%>').value = "Day";
            }
            if (type == 'Week') {

                Date.prototype.GetFirstDayOfWeek = function () {
                    return (new Date(this.setDate(this.getDate() - this.getDay())));
                }

                Date.prototype.GetLastDayOfWeek = function () {
                    return (new Date(this.setDate(this.getDate() - this.getDay() + 6)));
                }
                var today = new Date();
                var Firstdate = today.GetFirstDayOfWeek();
                var day = Firstdate.getDate();
                var month = Firstdate.getMonth() + 1;
                var year = Firstdate.getFullYear();
                var datestring = month + "/" + day + "/" + year;
                document.getElementById(txtDateFrom).value = datestring;
                var Lastdate = today.GetLastDayOfWeek();
                var day = Lastdate.getDate();
                var month = Lastdate.getMonth() + 1;
                var year = Lastdate.getFullYear();
                var dateString = month + "/" + day + "/" + year;
                document.getElementById(txtdateTo).value = dateString;
                $(label).addClass("labelactive");
                document.getElementById('<%= hdnReportSelectDtRange.ClientID%>').value = "Week";
            }
            if (type == 'Month') {
                var date = new Date(), y = date.getFullYear(), m = date.getMonth();
                var FirstDayOfMonth = new Date(y, m, 1);
                var lastDayOfMonth = new Date(y, m + 1, 0);
                var day = FirstDayOfMonth.getDate();
                var month = FirstDayOfMonth.getMonth() + 1;
                var year = FirstDayOfMonth.getFullYear();
                var datestring = month + "/" + day + "/" + year;
                document.getElementById(txtDateFrom).value = datestring;
                var day = lastDayOfMonth.getDate();
                var month = lastDayOfMonth.getMonth() + 1;
                var year = lastDayOfMonth.getFullYear();
                var dateString = month + "/" + day + "/" + year;
                document.getElementById(txtdateTo).value = dateString;
                $(label).addClass("labelactive");
                document.getElementById('<%= hdnReportSelectDtRange.ClientID%>').value = "Month";
            }
            if (type == 'Quarter') {
                var d = new Date();
                var quarter = Math.floor((d.getMonth() / 3));
                var firstDate = new Date(d.getFullYear(), quarter * 3, 1);
                var lastDate = new Date(firstDate.getFullYear(), firstDate.getMonth() + 3, 0);
                var day = firstDate.getDate();
                var month = firstDate.getMonth() + 1;
                var year = firstDate.getFullYear();
                var datestring = month + "/" + day + "/" + year;
                document.getElementById(txtDateFrom).value = datestring;
                var day = lastDate.getDate();
                var month = lastDate.getMonth() + 1;
                var year = lastDate.getFullYear();
                var dateString = month + "/" + day + "/" + year;
                document.getElementById(txtdateTo).value = dateString;
                $(label).addClass("labelactive");
                document.getElementById('<%= hdnReportSelectDtRange.ClientID%>').value = "Quarter";
            }
            if (type == 'Year') {
                var date = new Date(), y = date.getFullYear(), m = date.getMonth();
                var FirstDayOfYear = new Date(y, 1, 1);
                var lastDayOfYear = new Date(y, 11, 31);
                var day = FirstDayOfYear.getDate();
                var month = FirstDayOfYear.getMonth();
                var year = FirstDayOfYear.getFullYear();
                var datestring = month + "/" + day + "/" + year;
                document.getElementById(txtDateFrom).value = datestring;
                var day = lastDayOfYear.getDate();
                var month = lastDayOfYear.getMonth() + 1;
                var year = lastDayOfYear.getFullYear();
                var dateString = month + "/" + day + "/" + year;
                document.getElementById(txtdateTo).value = dateString;
                $(label).addClass("labelactive");
                document.getElementById('<%= hdnReportSelectDtRange.ClientID%>').value = "Year";
            }
            if (typeof (Storage) !== "undefined") {
                // Store
                localStorage.setItem(UniqueVal, document.getElementById('<%= hdnReportSelectDtRange.ClientID%>').value);
            }

            document.getElementById('<%= hdnCssActive.ClientID%>').value = "CssActive";
            var clickSearchButton = document.getElementById("<%= lnkSearch.ClientID %>");
            document.getElementById('<%= hdnCssActive.ClientID%>').value = "1";

            var selectedVal = $('#<%=ddlReport.ClientID%> option:selected').attr('value');

            if (selectedVal == "8") {
               $('#ctl00_ContentPlaceHolder1_txtStartDate').trigger('change');
            }
        }
    </script>
</asp:Content>

