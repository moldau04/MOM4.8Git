<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Mom.master" Inherits="OpenJobReport" Title="" EnableEventValidation="false" CodeBehind="OpenJobReport.aspx.cs" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagPrefix="asp" Namespace="Saplin.Controls" Assembly="DropDownCheckBoxes" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="Design/css/custom-report.css" type="text/css" rel="stylesheet" media="screen,projection">

    <script src="js/ColumnResizeWithReorder/jquery.dataTables.js" type="text/javascript"></script>
    <script src="js/ColumnResizeWithReorder/ColReorderWithResize.js" type="text/javascript"></script>
    <script src="js/ReportsJs/OpenJobReport.js" type="text/javascript"></script>
    <script type="text/javascript" src="js/Printdiv/jquery.print.js"></script>
    <script type="text/javascript" src="js/BlockUI/jquery.blockUI.js"></script>
    <script src="Design/js/moment.js"></script>
    <script src="Design/js/pikaday.js"></script>
    <script src="Design/js/pikaday.jquery.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('.datepicker_mom').pikaday({
                firstDay: 0,
                format: 'MM/DD/YYYY',
                minDate: new Date(1900, 1, 1),
                maxDate: new Date(2100, 12, 31),
                yearRange: [1900, 2100]
            });

            jQuery('#popup').draggable();
            setFilterStyle();

            var oTable = $('#tblResize').dataTable({
                "sDom": 'Rplfrti',
                "oColReorder": {
                    "headerContextMenu": true
                },
                "bPaginate": false,
                "bFilter": false,
                "bSort": false,
                "bAutoWidth": true,
                "bInfo": false
            });
        });

        function removeCheckbox() {
            $("#ctl00_ContentPlaceHolder1_chkColumnList INPUT[type='checkbox']").prop('checked', false);
            $("#ctl00_ContentPlaceHolder1_drpSortBy option").remove();
            $("#ctl00_ContentPlaceHolder1_lstColumnSort option").remove();
        }

        function setFilterStyle() {
            $('#ctl00_ContentPlaceHolder1_lstFilter option').each(function (index, element) {
                if ($(element).val() == "Customers" || $(element).val() == "Locations" || $(element).val() == "Equipments") {
                    $(element).css({ "font-size": "15px", "padding": "7px 0" });
                }
            });
        }

        function btnSaveReport() {
            var lstColumn = '';
            var lstColumnWidth = '';

            if ($('#tblResize tr').length > 0) {
                $('#tblResize tr th').each(function () {
                    lstColumn += $(this).html() + "^";
                    lstColumnWidth += $(this).css('width') + "^";
                });
            }
            else {
                $('#ctl00_ContentPlaceHolder1_lstColumnSort option').each(function (index, element) {
                    lstColumn += $(element).val() + "^";
                    lstColumnWidth += "125px" + "^";
                });
            }

            $("#ctl00_ContentPlaceHolder1_hdnLstColumns").val(lstColumn);
            $("#ctl00_ContentPlaceHolder1_hdnColumnWidth").val(lstColumnWidth);

            document.getElementById('<%=btnSaveReport2.ClientID %>').click();
        }
    </script>
    <script type="text/javascript">
        function showMailReport() {
            $('#mainWindow').attr('style', 'display:none');
            $('#dvEmailOpen').attr('style', 'display:block');
        }

        function cancel() {
            $('#mainWindow').attr('style', 'display:block');
            $('#dvEmailOpen').attr('style', 'display:none');
            return false;
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

        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57) && charCode != 46)
                return false;
            else {
                var txtChar = document.getElementById("txtChar");
                if (txtChar != null) {
                    var len = document.getElementById("txtChar").value.length;
                    var index = document.getElementById("txtChar").value.indexOf('.');

                    if (index > 0 && charCode == 46) {
                        return false;
                    }
                    if (index > 0) {
                        var CharAfterdot = (len + 1) - index;
                        if (CharAfterdot > 3) {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        function ShowLoading() {
            if (Page_ClientValidate()) {
                document.getElementById("overlay").style.display = "block";
            }

            return;
        }
    </script>
    <style type="text/css">
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

        .RadGrid_Material .rgHeader {
            font-weight: bold !important;
        }

        .ajax__validatorcallout {
            left: 300px !important;
            width: 300px !important;
        }

        @media screen and (max-width: 1920px) {
            .rgDataDiv {
                height: 73vh !important;
            }
        }
    </style>
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
    </script>
    <div id="overlay">
        <img src="images/wheel.GIF" alt="Be patient..." style="position: fixed; margin-top: 25%; margin-left: 50%;" />
    </div>
    <telerik:RadAjaxManager ID="RadAjaxManager_Project" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="RadGrid_Project">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_Project" LoadingPanelID="RadAjaxLoadingPanel_Project" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel_Project" runat="server">
    </telerik:RadAjaxLoadingPanel>

    <div id="mainWindow">
        <div class="divbutton-container">
            <div id="divButtons">
                <div id="breadcrumbs-wrapper">
                    <header>
                        <div class="container row-color-grey">
                            <div class="row">
                                <div class="col s12 m12 l12">
                                    <div class="row">
                                        <div class="page-title"><i class="mdi-action-swap-vert-circle"></i>&nbsp; Open Job Report</div>
                                        <div class="buttonContainer">
                                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                <ContentTemplate>
                                                    <div class="btnlinks">
                                                        <a id="btnNewReport" href="#" data-modal-id="popup">New Report</a>
                                                    </div>
                                                    <div class="btnlinks">
                                                        <asp:LinkButton runat="server" ID="btnSaveReport2" Text="Save Report" OnClientClick="btnSaveReport();" OnClick="btnSaveReport2_Click"></asp:LinkButton>
                                                    </div>
                                                    <div class="btnlinks">
                                                        <asp:LinkButton runat="server" ID="btnDeleteReport" Text="Delete Report" OnClientClick="return UserDeleteConfirmation();" OnClick="btnDeleteReport_Click"></asp:LinkButton>
                                                    </div>
                                                    <div class="btnlinks">
                                                        <asp:LinkButton runat="server" ID="btnCustomizeReport" data-modal-id="popup">Customize Report</asp:LinkButton>
                                                    </div>
                                                    <div class="btnlinks">
                                                        <asp:LinkButton runat="server" ID="btnPrint">Print</asp:LinkButton>
                                                    </div>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                            <ul class="nomgn hideMenu menuList">
                                                <li>
                                                    <ul id="dropdown1" class="dropdown-content">
                                                        <li>
                                                            <asp:LinkButton ID="lnkEmailAsPDF" runat="server" OnClick="lnkEmailAsPDF_Click">Send report as PDF</asp:LinkButton>
                                                        </li>
                                                        <li>
                                                            <asp:LinkButton ID="lnkEmailAsExcel" runat="server" OnClick="lnkEmailAsExcel_Click">Send report as Excel</asp:LinkButton>
                                                        </li>
                                                    </ul>
                                                    <div class="btnlinks">
                                                        <asp:LinkButton runat="server" ID="btnEmailReport" class="dropdown-button" data-beloworigin="true" href="#" data-activates="dropdown1">Email
                                                        </asp:LinkButton>
                                                    </div>
                                                </li>
                                                <li>
                                                    <ul id="dropdown2" class="dropdown-content">
                                                        <li>
                                                            <asp:LinkButton ID="lnkExportPDF" OnClick="lnkExportPDF_Click" runat="server">Export to PDF</asp:LinkButton>
                                                        </li>
                                                        <li>
                                                            <asp:LinkButton ID="lnkExportExcel" OnClick="lnkExportExcel_Click" runat="server">Export to Excel</asp:LinkButton>
                                                        </li>
                                                    </ul>
                                                    <div class="btnlinks">
                                                        <asp:LinkButton runat="server" ID="btnExportReport" class="dropdown-button" data-beloworigin="true" href="#" data-activates="dropdown2">Export
                                                        </asp:LinkButton>
                                                    </div>
                                                </li>

                                                <li>
                                                    <asp:LinkButton CssClass="icon-closed" runat="server" CausesValidation="false" ToolTip="close"
                                                        OnClick="lnkClose_Click"></asp:LinkButton>
                                                </li>
                                            </ul>
                                        </div>
                                        <div class="btnclosewrap">
                                            <asp:LinkButton runat="server" OnClick="lnkClose_Click" ID="lnkClose"><i class="mdi-content-clear"></i></asp:LinkButton>
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

        <div class="container accordian-wrap">
            <div class="row">
                <div class="srchpane">
                    <div class="srchtitle srchtitlecustomwidth">
                        Report
                    </div>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <div class="srchinputwrap" style="display: block">
                                <asp:DropDownList ID="drpReports" runat="server" Width="300px" CssClass="browser-default selectst"
                                    AutoPostBack="true" OnSelectedIndexChanged="drpReports_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>

                            <div class="col lblsz2 lblszfloat">
                                <div class="row">
                                    <span class="tro trost accrd-trost">
                                        <span>
                                            <asp:Label ID="lblRecordCount" runat="server"></asp:Label>
                                        </span>
                                    </span>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>

        <div class="container accordian-wrap">
            <div class="clearfix"></div>
            <div class="row">
                <div class="col-lg-12 col-md-12">
                    <div class="grid_container">
                        <div class="form-section-row" style="margin-bottom: 0 !important;">
                            <div class="RadGrid RadGrid_Material">
                                <telerik:RadGrid RenderMode="Auto" ID="RadGrid_Project" CssClass="RadGrid_Project" AllowFilteringByColumn="false" ShowFooter="false" HeaderStyle-CssClass="cent"
                                    ShowStatusBar="true" runat="server" AllowPaging="false" AllowSorting="false" PagerStyle-AlwaysVisible="true" MasterTableView-TableLayout="Fixed" MasterTableView-AllowFilteringByColumn="false"
                                    AllowCustomPaging="false" FilterType="CheckList" EnableLinqExpressions="false" ShowGroupPanel="false" AutoGenerateColumns="false" AlternatingItemStyle-BackColor="White" ItemStyle-BackColor="White"
                                    OnGridExporting="RadGrid_Project_GridExporting"
                                    OnNeedDataSource="RadGrid_Project_NeedDataSource">
                                    <CommandItemStyle />
                                    <HeaderStyle Font-Bold="true" />
                                    <GroupingSettings CaseSensitive="false" />
                                    <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true" EnableAlternatingItems="false">
                                        <Selecting AllowRowSelect="false"></Selecting>
                                        <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                        <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                    </ClientSettings>
                                </telerik:RadGrid>
                            </div>
                        </div>
                    </div>
                </div>
                <!-- edit-tab start -->
                <div class="col-lg-12 col-md-12">
                    <div class="com-cont">
                        <div id="popup" class="modal-box">
                            <header>
                                <a href="#" class="js-modal-close close">×</a>
                                <h3><span id="spnModelTitle"></span></h3>
                            </header>
                            <div class="modal-body">
                                <div class="custom-tabs">
                                    <ul class="tab-links">
                                        <li class="active"><a href="#tab1">Display</a></li>
                                        <li><a href="#tab2">Filters</a></li>
                                        <li><a href="#tab3">Header/Footer</a></li>
                                        <li><a href="#tab4">Setting</a></li>
                                    </ul>
                                    <div class="tab-content">
                                        <div id="tab1" class="tab active">
                                            <fieldset style="border-color: #CDCDCD;">
                                                <legend><b>COLUMNS:</b></legend>
                                                <div>
                                                    <div style="float: left;">
                                                        <div id="dvColumn">
                                                            <label class="column-list-title">Open Job</label>
                                                            <label>
                                                                <asp:CheckBoxList ID="chkColumnList" runat="server" ForeColor="Black" Font-Size="12px"
                                                                    BorderStyle="Solid" BorderColor="Black" CssClass="column-list browser-default" Style="white-space: nowrap;">
                                                                </asp:CheckBoxList>
                                                            </label>
                                                        </div>
                                                        <div id="dvSetPosition">
                                                            <table style="overflow: auto; margin-top: 120px;">
                                                                <tr>
                                                                    <td>
                                                                        <input id="MoveUp" type="button" class="UpArrow" />
                                                                        <br />
                                                                        <input id="MoveDown" type="button" class="DownArrow" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                        <div id="dvColumnSelected">
                                                            <asp:ListBox ID="lstColumnSort" runat="server" SelectionMode="Multiple" CssClass="column-list-selected browser-default"></asp:ListBox>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div id="dvSort" style="float: right;">
                                                    <table>
                                                        <tr>
                                                            <td>Sort by</td>
                                                            <td style="padding-left: 15px">
                                                                <asp:DropDownList ID="drpSortBy" runat="server" CssClass="browser-default"
                                                                    Width="150px">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>Sort in</td>
                                                            <td style="padding-left: 15px">
                                                                <asp:RadioButtonList ID="rdbOrders" runat="server" CssClass="ListControl">
                                                                    <asp:ListItem Selected="True" Value="1">Ascending order</asp:ListItem>
                                                                    <asp:ListItem Value="2">Descending order</asp:ListItem>
                                                                </asp:RadioButtonList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2" style="padding-top: 30px">Put a check mark next to each column that
                                                            <br />
                                                                you want to appear in the report.
                                                            <br />
                                                                <br />
                                                                And set the order of the columns
                                                            <br />
                                                                with up and down arrow.
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </fieldset>
                                        </div>
                                        <div id="tab2" class="tab" style="height: 280px;">
                                            <div style="float: left;">
                                                <fieldset style="border-color: #CDCDCD; width: 600px;">
                                                    <legend><b>CHOOSE FILTER:</b></legend>
                                                    <div id="dvFilterList">
                                                        <asp:ListBox ID="lstFilter" runat="server" CssClass="column-list-filter browser-default"></asp:ListBox>
                                                    </div>
                                                    <div id="Div2" style="float: right; margin: 20px; width: 220px;">
                                                        <table id="tblState" style="display: none">
                                                            <tr>
                                                                <td>State
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:DropDownCheckBoxes ID="ddlState" runat="server" AddJQueryReference="false" UseButtons="false"
                                                                        Width="150px" CssClass="browser-default" UseSelectAllNode="false">
                                                                        <Style2 SelectBoxWidth="200" DropDownBoxBoxWidth="200" DropDownBoxBoxHeight="130"
                                                                            SelectBoxCssClass="ListControl" />
                                                                        <Texts SelectBoxCaption="Select" />
                                                                        <Items>
                                                                            <%--<asp:ListItem Value="All">All</asp:ListItem>--%>
                                                                            <asp:ListItem Value="AL">Alabama</asp:ListItem>
                                                                            <asp:ListItem Value="AK">Alaska</asp:ListItem>
                                                                            <asp:ListItem Value="AZ">Arizona</asp:ListItem>
                                                                            <asp:ListItem Value="AR">Arkansas</asp:ListItem>
                                                                            <asp:ListItem Value="CA">California</asp:ListItem>
                                                                            <asp:ListItem Value="CO">Colorado</asp:ListItem>
                                                                            <asp:ListItem Value="CT">Connecticut</asp:ListItem>
                                                                            <asp:ListItem Value="DC">District of Columbia</asp:ListItem>
                                                                            <asp:ListItem Value="DE">Delaware</asp:ListItem>
                                                                            <asp:ListItem Value="FL">Florida</asp:ListItem>
                                                                            <asp:ListItem Value="GA">Georgia</asp:ListItem>
                                                                            <asp:ListItem Value="HI">Hawaii</asp:ListItem>
                                                                            <asp:ListItem Value="ID">Idaho</asp:ListItem>
                                                                            <asp:ListItem Value="IL">Illinois</asp:ListItem>
                                                                            <asp:ListItem Value="IN">Indiana</asp:ListItem>
                                                                            <asp:ListItem Value="IA">Iowa</asp:ListItem>
                                                                            <asp:ListItem Value="KS">Kansas</asp:ListItem>
                                                                            <asp:ListItem Value="KY">Kentucky</asp:ListItem>
                                                                            <asp:ListItem Value="LA">Louisiana</asp:ListItem>
                                                                            <asp:ListItem Value="ME">Maine</asp:ListItem>
                                                                            <asp:ListItem Value="MD">Maryland</asp:ListItem>
                                                                            <asp:ListItem Value="MA">Massachusetts</asp:ListItem>
                                                                            <asp:ListItem Value="MI">Michigan</asp:ListItem>
                                                                            <asp:ListItem Value="MN">Minnesota</asp:ListItem>
                                                                            <asp:ListItem Value="MS">Mississippi</asp:ListItem>
                                                                            <asp:ListItem Value="MO">Missouri</asp:ListItem>
                                                                            <asp:ListItem Value="MT">Montana</asp:ListItem>
                                                                            <asp:ListItem Value="NE">Nebraska</asp:ListItem>
                                                                            <asp:ListItem Value="NV">Nevada</asp:ListItem>
                                                                            <asp:ListItem Value="NH">New Hampshire</asp:ListItem>
                                                                            <asp:ListItem Value="NJ">New Jersey</asp:ListItem>
                                                                            <asp:ListItem Value="NM">New Mexico</asp:ListItem>
                                                                            <asp:ListItem Value="NY">New York</asp:ListItem>
                                                                            <asp:ListItem Value="NC">North Carolina</asp:ListItem>
                                                                            <asp:ListItem Value="ND">North Dakota</asp:ListItem>
                                                                            <asp:ListItem Value="OH">Ohio</asp:ListItem>
                                                                            <asp:ListItem Value="OK">Oklahoma</asp:ListItem>
                                                                            <asp:ListItem Value="OR">Oregon</asp:ListItem>
                                                                            <asp:ListItem Value="PA">Pennsylvania</asp:ListItem>
                                                                            <asp:ListItem Value="RI">Rhode Island</asp:ListItem>
                                                                            <asp:ListItem Value="SC">South Carolina</asp:ListItem>
                                                                            <asp:ListItem Value="SD">South Dakota</asp:ListItem>
                                                                            <asp:ListItem Value="TN">Tennessee</asp:ListItem>
                                                                            <asp:ListItem Value="TX">Texas</asp:ListItem>
                                                                            <asp:ListItem Value="UT">Utah</asp:ListItem>
                                                                            <asp:ListItem Value="VT">Vermont</asp:ListItem>
                                                                            <asp:ListItem Value="VA">Virginia</asp:ListItem>
                                                                            <asp:ListItem Value="WA">Washington</asp:ListItem>
                                                                            <asp:ListItem Value="WV">West Virginia</asp:ListItem>
                                                                            <asp:ListItem Value="WI">Wisconsin</asp:ListItem>
                                                                            <asp:ListItem Value="WY">Wyoming</asp:ListItem>
                                                                            <asp:ListItem Value="AB">Alberta</asp:ListItem>
                                                                            <asp:ListItem Value="BC">British Columbia</asp:ListItem>
                                                                            <asp:ListItem Value="MB">Manitoba</asp:ListItem>
                                                                            <asp:ListItem Value="NB">New Brunswick</asp:ListItem>
                                                                            <asp:ListItem Value="NL">Newfoundland and Labrador</asp:ListItem>
                                                                            <asp:ListItem Value="NT">Northwest Territories</asp:ListItem>
                                                                            <asp:ListItem Value="NS">Nova Scotia</asp:ListItem>
                                                                            <asp:ListItem Value="NU">Nunavut</asp:ListItem>
                                                                            <asp:ListItem Value="PE">Prince Edward Island</asp:ListItem>
                                                                            <asp:ListItem Value="SK">Saskatchewan</asp:ListItem>
                                                                            <asp:ListItem Value="ON">Ontario</asp:ListItem>
                                                                            <asp:ListItem Value="QC">Quebec</asp:ListItem>
                                                                            <asp:ListItem Value="YT">Yukon</asp:ListItem>
                                                                        </Items>
                                                                    </asp:DropDownCheckBoxes>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <table id="tblStateRef" style="display: none">
                                                            <tr>
                                                                <td>
                                                                    <asp:DropDownList ID="ddlStateReference" runat="server" ToolTip="State" CssClass="browser-default"
                                                                        Width="150px">
                                                                        <asp:ListItem Value="All">All</asp:ListItem>
                                                                        <asp:ListItem Value="AL">Alabama</asp:ListItem>
                                                                        <asp:ListItem Value="AK">Alaska</asp:ListItem>
                                                                        <asp:ListItem Value="AZ">Arizona</asp:ListItem>
                                                                        <asp:ListItem Value="AR">Arkansas</asp:ListItem>
                                                                        <asp:ListItem Value="CA">California</asp:ListItem>
                                                                        <asp:ListItem Value="CO">Colorado</asp:ListItem>
                                                                        <asp:ListItem Value="CT">Connecticut</asp:ListItem>
                                                                        <asp:ListItem Value="DC">District of Columbia</asp:ListItem>
                                                                        <asp:ListItem Value="DE">Delaware</asp:ListItem>
                                                                        <asp:ListItem Value="FL">Florida</asp:ListItem>
                                                                        <asp:ListItem Value="GA">Georgia</asp:ListItem>
                                                                        <asp:ListItem Value="HI">Hawaii</asp:ListItem>
                                                                        <asp:ListItem Value="ID">Idaho</asp:ListItem>
                                                                        <asp:ListItem Value="IL">Illinois</asp:ListItem>
                                                                        <asp:ListItem Value="IN">Indiana</asp:ListItem>
                                                                        <asp:ListItem Value="IA">Iowa</asp:ListItem>
                                                                        <asp:ListItem Value="KS">Kansas</asp:ListItem>
                                                                        <asp:ListItem Value="KY">Kentucky</asp:ListItem>
                                                                        <asp:ListItem Value="LA">Louisiana</asp:ListItem>
                                                                        <asp:ListItem Value="ME">Maine</asp:ListItem>
                                                                        <asp:ListItem Value="MD">Maryland</asp:ListItem>
                                                                        <asp:ListItem Value="MA">Massachusetts</asp:ListItem>
                                                                        <asp:ListItem Value="MI">Michigan</asp:ListItem>
                                                                        <asp:ListItem Value="MN">Minnesota</asp:ListItem>
                                                                        <asp:ListItem Value="MS">Mississippi</asp:ListItem>
                                                                        <asp:ListItem Value="MO">Missouri</asp:ListItem>
                                                                        <asp:ListItem Value="MT">Montana</asp:ListItem>
                                                                        <asp:ListItem Value="NE">Nebraska</asp:ListItem>
                                                                        <asp:ListItem Value="NV">Nevada</asp:ListItem>
                                                                        <asp:ListItem Value="NH">New Hampshire</asp:ListItem>
                                                                        <asp:ListItem Value="NJ">New Jersey</asp:ListItem>
                                                                        <asp:ListItem Value="NM">New Mexico</asp:ListItem>
                                                                        <asp:ListItem Value="NY">New York</asp:ListItem>
                                                                        <asp:ListItem Value="NC">North Carolina</asp:ListItem>
                                                                        <asp:ListItem Value="ND">North Dakota</asp:ListItem>
                                                                        <asp:ListItem Value="OH">Ohio</asp:ListItem>
                                                                        <asp:ListItem Value="OK">Oklahoma</asp:ListItem>
                                                                        <asp:ListItem Value="OR">Oregon</asp:ListItem>
                                                                        <asp:ListItem Value="PA">Pennsylvania</asp:ListItem>
                                                                        <asp:ListItem Value="RI">Rhode Island</asp:ListItem>
                                                                        <asp:ListItem Value="SC">South Carolina</asp:ListItem>
                                                                        <asp:ListItem Value="SD">South Dakota</asp:ListItem>
                                                                        <asp:ListItem Value="TN">Tennessee</asp:ListItem>
                                                                        <asp:ListItem Value="TX">Texas</asp:ListItem>
                                                                        <asp:ListItem Value="UT">Utah</asp:ListItem>
                                                                        <asp:ListItem Value="VT">Vermont</asp:ListItem>
                                                                        <asp:ListItem Value="VA">Virginia</asp:ListItem>
                                                                        <asp:ListItem Value="WA">Washington</asp:ListItem>
                                                                        <asp:ListItem Value="WV">West Virginia</asp:ListItem>
                                                                        <asp:ListItem Value="WI">Wisconsin</asp:ListItem>
                                                                        <asp:ListItem Value="WY">Wyoming</asp:ListItem>
                                                                        <asp:ListItem Value="AB">Alberta</asp:ListItem>
                                                                        <asp:ListItem Value="BC">British Columbia</asp:ListItem>
                                                                        <asp:ListItem Value="MB">Manitoba</asp:ListItem>
                                                                        <asp:ListItem Value="NB">New Brunswick</asp:ListItem>
                                                                        <asp:ListItem Value="NL">Newfoundland and Labrador</asp:ListItem>
                                                                        <asp:ListItem Value="NT">Northwest Territories</asp:ListItem>
                                                                        <asp:ListItem Value="NS">Nova Scotia</asp:ListItem>
                                                                        <asp:ListItem Value="NU">Nunavut</asp:ListItem>
                                                                        <asp:ListItem Value="PE">Prince Edward Island</asp:ListItem>
                                                                        <asp:ListItem Value="SK">Saskatchewan</asp:ListItem>
                                                                        <asp:ListItem Value="ON">Ontario</asp:ListItem>
                                                                        <asp:ListItem Value="QC">Quebec</asp:ListItem>
                                                                        <asp:ListItem Value="YT">Yukon</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <table id="tblCity" style="display: none">
                                                            <tr>
                                                                <td>City
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:DropDownCheckBoxes ID="drpCity" CssClass="browser-default" runat="server" AddJQueryReference="false" UseButtons="false"
                                                                        Width="150px" UseSelectAllNode="false">
                                                                        <Style2 SelectBoxWidth="200" DropDownBoxBoxWidth="200" DropDownBoxBoxHeight="170"
                                                                            SelectBoxCssClass="ListControl" />
                                                                        <Texts SelectBoxCaption="Select" />
                                                                    </asp:DropDownCheckBoxes>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <table id="tblCustomer" style="display: none">
                                                            <tr>
                                                                <td>Customer
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:DropDownCheckBoxes ID="drpCustomer" CssClass="form-control" runat="server" AddJQueryReference="false" UseButtons="false"
                                                                        Width="150px" UseSelectAllNode="false">
                                                                        <Style2 SelectBoxWidth="200" DropDownBoxBoxWidth="200" DropDownBoxBoxHeight="170"
                                                                            SelectBoxCssClass="ListControl" />
                                                                        <Texts SelectBoxCaption="Select" />
                                                                    </asp:DropDownCheckBoxes>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <table id="tblLocation" style="display: none">
                                                            <tr>
                                                                <td>Location
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <%-- <asp:TextBox ID="txtCity" runat="server" CssClass="register_input_bg" Width="150px"></asp:TextBox>--%>
                                                                    <asp:DropDownCheckBoxes ID="drpLocation" CssClass="form-control" runat="server" AddJQueryReference="false" UseButtons="false"
                                                                        Width="150px" UseSelectAllNode="false">
                                                                        <Style2 SelectBoxWidth="200" DropDownBoxBoxWidth="200" DropDownBoxBoxHeight="170"
                                                                            SelectBoxCssClass="ListControl" />
                                                                        <Texts SelectBoxCaption="Select" />
                                                                    </asp:DropDownCheckBoxes>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <table id="tblZip" style="display: none">
                                                            <tr>
                                                                <td>Zip
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:TextBox ID="txtZip" runat="server" CssClass="form-control" Width="150px"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <table id="tblPhone" style="display: none">
                                                            <tr>
                                                                <td>Phone
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:TextBox ID="txtPhone" runat="server" CssClass="form-control" Width="150px"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <table id="tblFax" style="display: none">
                                                            <tr>
                                                                <td>Fax
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:TextBox ID="txtFax" runat="server" CssClass="form-control" Width="150px"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <table id="tblContact" style="display: none">
                                                            <tr>
                                                                <td>Contact
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:TextBox ID="txtContact" runat="server" CssClass="form-control" Width="150px"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <table id="tblAddress" style="display: none">
                                                            <tr>
                                                                <td>Address
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <%-- <asp:TextBox ID="txtAddress" runat="server" CssClass="register_input_bg" Width="150px"></asp:TextBox>--%>
                                                                    <asp:DropDownCheckBoxes ID="drpAddress" runat="server" CssClass="form-control" AddJQueryReference="false"
                                                                        UseButtons="false" Width="150px" UseSelectAllNode="false">
                                                                        <Style2 SelectBoxWidth="200" DropDownBoxBoxWidth="200" DropDownBoxBoxHeight="170"
                                                                            SelectBoxCssClass="ListControl" />
                                                                        <Texts SelectBoxCaption="Select" />
                                                                    </asp:DropDownCheckBoxes>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <table id="tblEmail" style="display: none">
                                                            <tr>
                                                                <td>Email
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" Width="150px"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <table id="tblCountry" style="display: none">
                                                            <tr>
                                                                <td>Country
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:TextBox ID="txtCountry" runat="server" CssClass="form-control" Width="150px"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <table id="tblWebsite" style="display: none">
                                                            <tr>
                                                                <td>Website
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:TextBox ID="txtWebsite" runat="server" CssClass="form-control" Width="150px"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <table id="tblCellular" style="display: none">
                                                            <tr>
                                                                <td>Cellular
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:TextBox ID="txtCellular" runat="server" CssClass="form-control" Width="150px"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <table id="tblCategory" style="display: none">
                                                            <tr>
                                                                <td>Category
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:DropDownList ID="drpCategory" runat="server" CssClass="browser-default" Width="150px">
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <table id="tblType" style="display: none">
                                                            <tr>
                                                                <td>Type
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:DropDownCheckBoxes ID="drpType" runat="server" AddJQueryReference="false" UseButtons="false"
                                                                        Width="150px" UseSelectAllNode="false" CssClass="browser-default">
                                                                        <Style2 SelectBoxWidth="200" DropDownBoxBoxWidth="200" DropDownBoxBoxHeight="130"
                                                                            SelectBoxCssClass="ListControl" />
                                                                        <Texts SelectBoxCaption="Types" />
                                                                    </asp:DropDownCheckBoxes>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <table id="tblBalance" style="display: none">
                                                            <tr>
                                                                <td colspan="4">Balance
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>&nbsp;
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="4">
                                                                    <asp:RadioButton ID="rdbAny" runat="server" GroupName="Balance" Text="Any" CssClass="ListControl" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:RadioButton ID="rdbEqual" runat="server" Width="40px" GroupName="Balance" Text="="
                                                                        CssClass="ListControl" />
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtBalEqual" runat="server" CssClass="form-control" Width="80px"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:RadioButton ID="rdbLessAndEqual" runat="server" Width="40px" GroupName="Balance"
                                                                        Text="&lt;=" CssClass="ListControl" />
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtBalLessAndEqual" runat="server" CssClass="form-control"
                                                                        Width="80px"></asp:TextBox>
                                                                    <span style="padding-left: 10px;">and</span>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:RadioButton ID="rdbGreaterAndEqual" runat="server" Width="40px" GroupName="Balance"
                                                                        Text="&gt;=" CssClass="ListControl" />
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtBalGreaterAndEqual" runat="server" CssClass="form-control"
                                                                        Width="80px"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <table id="tblStatus" style="display: none">
                                                            <tr>
                                                                <td>Status
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:DropDownList ID="drpStatus" runat="server" CssClass="browser-default" Width="150px">
                                                                        <asp:ListItem Value="Status">All</asp:ListItem>
                                                                        <asp:ListItem Value="0">Active</asp:ListItem>
                                                                        <asp:ListItem Value="1">Inactive</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <table id="tblLocationId" style="display: none">
                                                            <tr>
                                                                <td>Location Id
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:DropDownCheckBoxes ID="drpLocationId" runat="server" AddJQueryReference="false"
                                                                        UseButtons="false" Width="150px" UseSelectAllNode="false" CssClass="form-control">
                                                                        <Style2 SelectBoxWidth="200" DropDownBoxBoxWidth="200" DropDownBoxBoxHeight="130"
                                                                            SelectBoxCssClass="ListControl" />
                                                                        <Texts SelectBoxCaption="Select" />
                                                                    </asp:DropDownCheckBoxes>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <table id="tblLocationName" style="display: none">
                                                            <tr>
                                                                <td>Location Name
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:DropDownCheckBoxes ID="drpLocationName" runat="server" AddJQueryReference="false"
                                                                        UseButtons="false" Width="150px" CssClass="form-control" UseSelectAllNode="false">
                                                                        <Style2 SelectBoxWidth="200" DropDownBoxBoxWidth="200" DropDownBoxBoxHeight="130"
                                                                            SelectBoxCssClass="ListControl" />
                                                                        <Texts SelectBoxCaption="Select" />
                                                                    </asp:DropDownCheckBoxes>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <table id="tblLocationAddress" style="display: none">
                                                            <tr>
                                                                <td>Location Address
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:DropDownCheckBoxes ID="drpLocationAddress" runat="server" AddJQueryReference="false"
                                                                        UseButtons="false" CssClass="form-control" Width="150px" UseSelectAllNode="false">
                                                                        <Style2 SelectBoxWidth="200" DropDownBoxBoxWidth="200" DropDownBoxBoxHeight="130"
                                                                            SelectBoxCssClass="ListControl" />
                                                                        <Texts SelectBoxCaption="Select" />
                                                                    </asp:DropDownCheckBoxes>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <table id="tblLocationCity" style="display: none">
                                                            <tr>
                                                                <td>Location City
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:DropDownCheckBoxes ID="drpLocationCity" runat="server" AddJQueryReference="false"
                                                                        UseButtons="false" Width="150px" CssClass="form-control" UseSelectAllNode="false">
                                                                        <Style2 SelectBoxWidth="200" DropDownBoxBoxWidth="200" DropDownBoxBoxHeight="130"
                                                                            SelectBoxCssClass="ListControl" />
                                                                        <Texts SelectBoxCaption="Select" />
                                                                    </asp:DropDownCheckBoxes>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <table id="tblLocationState" style="display: none">
                                                            <tr>
                                                                <td>Location State
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:DropDownCheckBoxes ID="drpLocationState" runat="server" AddJQueryReference="false"
                                                                        UseButtons="false" Width="150px" CssClass="form-control" UseSelectAllNode="false">
                                                                        <Style2 SelectBoxWidth="200" DropDownBoxBoxWidth="200" DropDownBoxBoxHeight="130"
                                                                            SelectBoxCssClass="ListControl" />
                                                                        <Texts SelectBoxCaption="Select" />
                                                                        <Items>
                                                                            <%--<asp:ListItem Value="All">All</asp:ListItem>--%>
                                                                            <asp:ListItem Value="AL">Alabama</asp:ListItem>
                                                                            <asp:ListItem Value="AK">Alaska</asp:ListItem>
                                                                            <asp:ListItem Value="AZ">Arizona</asp:ListItem>
                                                                            <asp:ListItem Value="AR">Arkansas</asp:ListItem>
                                                                            <asp:ListItem Value="CA">California</asp:ListItem>
                                                                            <asp:ListItem Value="CO">Colorado</asp:ListItem>
                                                                            <asp:ListItem Value="CT">Connecticut</asp:ListItem>
                                                                            <asp:ListItem Value="DC">District of Columbia</asp:ListItem>
                                                                            <asp:ListItem Value="DE">Delaware</asp:ListItem>
                                                                            <asp:ListItem Value="FL">Florida</asp:ListItem>
                                                                            <asp:ListItem Value="GA">Georgia</asp:ListItem>
                                                                            <asp:ListItem Value="HI">Hawaii</asp:ListItem>
                                                                            <asp:ListItem Value="ID">Idaho</asp:ListItem>
                                                                            <asp:ListItem Value="IL">Illinois</asp:ListItem>
                                                                            <asp:ListItem Value="IN">Indiana</asp:ListItem>
                                                                            <asp:ListItem Value="IA">Iowa</asp:ListItem>
                                                                            <asp:ListItem Value="KS">Kansas</asp:ListItem>
                                                                            <asp:ListItem Value="KY">Kentucky</asp:ListItem>
                                                                            <asp:ListItem Value="LA">Louisiana</asp:ListItem>
                                                                            <asp:ListItem Value="ME">Maine</asp:ListItem>
                                                                            <asp:ListItem Value="MD">Maryland</asp:ListItem>
                                                                            <asp:ListItem Value="MA">Massachusetts</asp:ListItem>
                                                                            <asp:ListItem Value="MI">Michigan</asp:ListItem>
                                                                            <asp:ListItem Value="MN">Minnesota</asp:ListItem>
                                                                            <asp:ListItem Value="MS">Mississippi</asp:ListItem>
                                                                            <asp:ListItem Value="MO">Missouri</asp:ListItem>
                                                                            <asp:ListItem Value="MT">Montana</asp:ListItem>
                                                                            <asp:ListItem Value="NE">Nebraska</asp:ListItem>
                                                                            <asp:ListItem Value="NV">Nevada</asp:ListItem>
                                                                            <asp:ListItem Value="NH">New Hampshire</asp:ListItem>
                                                                            <asp:ListItem Value="NJ">New Jersey</asp:ListItem>
                                                                            <asp:ListItem Value="NM">New Mexico</asp:ListItem>
                                                                            <asp:ListItem Value="NY">New York</asp:ListItem>
                                                                            <asp:ListItem Value="NC">North Carolina</asp:ListItem>
                                                                            <asp:ListItem Value="ND">North Dakota</asp:ListItem>
                                                                            <asp:ListItem Value="OH">Ohio</asp:ListItem>
                                                                            <asp:ListItem Value="OK">Oklahoma</asp:ListItem>
                                                                            <asp:ListItem Value="OR">Oregon</asp:ListItem>
                                                                            <asp:ListItem Value="PA">Pennsylvania</asp:ListItem>
                                                                            <asp:ListItem Value="RI">Rhode Island</asp:ListItem>
                                                                            <asp:ListItem Value="SC">South Carolina</asp:ListItem>
                                                                            <asp:ListItem Value="SD">South Dakota</asp:ListItem>
                                                                            <asp:ListItem Value="TN">Tennessee</asp:ListItem>
                                                                            <asp:ListItem Value="TX">Texas</asp:ListItem>
                                                                            <asp:ListItem Value="UT">Utah</asp:ListItem>
                                                                            <asp:ListItem Value="VT">Vermont</asp:ListItem>
                                                                            <asp:ListItem Value="VA">Virginia</asp:ListItem>
                                                                            <asp:ListItem Value="WA">Washington</asp:ListItem>
                                                                            <asp:ListItem Value="WV">West Virginia</asp:ListItem>
                                                                            <asp:ListItem Value="WI">Wisconsin</asp:ListItem>
                                                                            <asp:ListItem Value="WY">Wyoming</asp:ListItem>
                                                                            <asp:ListItem Value="AB">Alberta</asp:ListItem>
                                                                            <asp:ListItem Value="BC">British Columbia</asp:ListItem>
                                                                            <asp:ListItem Value="MB">Manitoba</asp:ListItem>
                                                                            <asp:ListItem Value="NB">New Brunswick</asp:ListItem>
                                                                            <asp:ListItem Value="NL">Newfoundland and Labrador</asp:ListItem>
                                                                            <asp:ListItem Value="NT">Northwest Territories</asp:ListItem>
                                                                            <asp:ListItem Value="NS">Nova Scotia</asp:ListItem>
                                                                            <asp:ListItem Value="NU">Nunavut</asp:ListItem>
                                                                            <asp:ListItem Value="PE">Prince Edward Island</asp:ListItem>
                                                                            <asp:ListItem Value="SK">Saskatchewan</asp:ListItem>
                                                                            <asp:ListItem Value="ON">Ontario</asp:ListItem>
                                                                            <asp:ListItem Value="QC">Quebec</asp:ListItem>
                                                                            <asp:ListItem Value="YT">Yukon</asp:ListItem>
                                                                        </Items>
                                                                    </asp:DropDownCheckBoxes>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <table id="tblLocationZip" style="display: none">
                                                            <tr>
                                                                <td>Location Zip
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <%--  <asp:TextBox ID="txtLocationZip" runat="server" CssClass="register_input_bg" Width="150px"></asp:TextBox>--%>
                                                                    <asp:DropDownCheckBoxes ID="drpLocationZip" runat="server" AddJQueryReference="false"
                                                                        UseButtons="false" Width="150px" CssClass="form-control" UseSelectAllNode="false">
                                                                        <Style2 SelectBoxWidth="200" DropDownBoxBoxWidth="200" DropDownBoxBoxHeight="130"
                                                                            SelectBoxCssClass="ListControl" />
                                                                        <Texts SelectBoxCaption="Select" />
                                                                    </asp:DropDownCheckBoxes>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <table id="tblLocationType" style="display: none">
                                                            <tr>
                                                                <td>Location Type
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:DropDownCheckBoxes ID="drpLocationType" runat="server" AddJQueryReference="false"
                                                                        UseButtons="false" Width="150px" CssClass="form-control" UseSelectAllNode="false">
                                                                        <Style2 SelectBoxWidth="200" DropDownBoxBoxWidth="200" DropDownBoxBoxHeight="130"
                                                                            SelectBoxCssClass="ListControl" />
                                                                        <Texts SelectBoxCaption="Select" />
                                                                    </asp:DropDownCheckBoxes>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <table id="tblEquipmentName" style="display: none">
                                                            <tr>
                                                                <td>Equipment Name
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:DropDownCheckBoxes ID="drpEquipmentName" runat="server" AddJQueryReference="false"
                                                                        UseButtons="false" Width="150px" UseSelectAllNode="false" CssClass="form-control">
                                                                        <Style2 SelectBoxWidth="200" DropDownBoxBoxWidth="200" DropDownBoxBoxHeight="130"
                                                                            SelectBoxCssClass="ListControl" />
                                                                        <Texts SelectBoxCaption="Select" />
                                                                    </asp:DropDownCheckBoxes>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <table id="tblManuf" style="display: none">
                                                            <tr>
                                                                <td>Manuf
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:DropDownCheckBoxes ID="drpManuf" runat="server" AddJQueryReference="false" UseButtons="false"
                                                                        Width="150px" UseSelectAllNode="false" CssClass="form-control">
                                                                        <Style2 SelectBoxWidth="200" DropDownBoxBoxWidth="200" DropDownBoxBoxHeight="130"
                                                                            SelectBoxCssClass="ListControl" />
                                                                        <Texts SelectBoxCaption="Select" />
                                                                    </asp:DropDownCheckBoxes>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <table id="tblEquipmentType" style="display: none">
                                                            <tr>
                                                                <td>Equipment Type
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:DropDownCheckBoxes ID="drpEquipmentType" runat="server" AddJQueryReference="false"
                                                                        UseButtons="false" Width="150px" UseSelectAllNode="false" CssClass="form-control">
                                                                        <Style2 SelectBoxWidth="200" DropDownBoxBoxWidth="200" DropDownBoxBoxHeight="130"
                                                                            SelectBoxCssClass="ListControl" />
                                                                        <Texts SelectBoxCaption="Select" />
                                                                    </asp:DropDownCheckBoxes>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <table id="tblServiceType" style="display: none">
                                                            <tr>
                                                                <td>Service Type
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:DropDownCheckBoxes ID="drpServiceType" runat="server" AddJQueryReference="false"
                                                                        UseButtons="false" Width="150px" UseSelectAllNode="false" CssClass="form-control">
                                                                        <Style2 SelectBoxWidth="200" DropDownBoxBoxWidth="200" DropDownBoxBoxHeight="130"
                                                                            SelectBoxCssClass="ListControl" />
                                                                        <Texts SelectBoxCaption="Select" />
                                                                    </asp:DropDownCheckBoxes>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <table id="tblEquipmentPrice" style="display: none">
                                                            <tr>
                                                                <td colspan="4">Equipment Price
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>&nbsp;
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="4">
                                                                    <asp:RadioButton ID="rdbEquipmentPriceAny" runat="server" GroupName="ep" Text="Any"
                                                                        CssClass="ListControl" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:RadioButton ID="rdbEquipmentPriceEqual" runat="server" Width="40px" GroupName="ep"
                                                                        Text="=" CssClass="ListControl" />
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtEquipmentPriceEqual" runat="server" CssClass="form-control"
                                                                        Width="80px"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:RadioButton ID="rdbEquipmentPriceGreaterAndEqual" runat="server" Width="40px"
                                                                        GroupName="ep" Text="&gt;=" CssClass="ListControl" />
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtEquipmentPriceGreatAndEqual" runat="server" CssClass="form-control"
                                                                        Width="80px"></asp:TextBox>
                                                                    <span style="padding-left: 10px;">and</span>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:RadioButton ID="rdbEquipmentPriceLessAndEqual" runat="server" Width="40px" GroupName="ep"
                                                                        Text="&lt;=" CssClass="ListControl" />
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtEquipmentPriceLessAndEqual" runat="server" CssClass="form-control"
                                                                        Width="80px"></asp:TextBox>
                                                                </td>
                                                            </tr>

                                                        </table>
                                                        <table id="tblLoc" style="display: none">
                                                            <tr>
                                                                <td colspan="4">Loc
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>&nbsp;
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="4">
                                                                    <asp:RadioButton ID="rdbLocAny" runat="server" GroupName="loc" Text="Any" CssClass="ListControl" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:RadioButton ID="rdbLocEqual" runat="server" Width="40px" GroupName="loc" Text="="
                                                                        CssClass="ListControl" />
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtLocEqual" runat="server" CssClass="form-control" Width="80px"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:RadioButton ID="rdbLocGreaterAndEqual" runat="server" Width="40px" GroupName="loc"
                                                                        Text="&gt;=" CssClass="ListControl" />
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtLocGreatAndEqual" runat="server" CssClass="form-control"
                                                                        Width="80px"></asp:TextBox>
                                                                    <span style="padding-left: 10px;">and</span>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:RadioButton ID="rdbLocLessAndEqual" runat="server" Width="40px" GroupName="loc"
                                                                        Text="&lt;=" CssClass="ListControl" />
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtLocLessAndEqual" runat="server" CssClass="form-control"
                                                                        Width="80px"></asp:TextBox>
                                                                </td>
                                                            </tr>

                                                        </table>
                                                        <table id="tblEquip" style="display: none">
                                                            <tr>
                                                                <td colspan="4">Equipment
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>&nbsp;
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="4">
                                                                    <asp:RadioButton ID="rdbEquipAny" runat="server" GroupName="equip" Text="Any" CssClass="ListControl" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:RadioButton ID="rdbEquipEqual" runat="server" Width="40px" GroupName="equip"
                                                                        Text="=" CssClass="ListControl" />
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtEquipEqual" runat="server" CssClass="form-control" Width="80px"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:RadioButton ID="rdbEquipGreaterAndEqual" runat="server" Width="40px" GroupName="equip"
                                                                        Text="&gt;=" CssClass="ListControl" />
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtEquipGreatAndEqual" runat="server" CssClass="form-control"
                                                                        Width="80px"></asp:TextBox>
                                                                    <span style="padding-left: 10px;">and</span>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:RadioButton ID="rdbEquipLessAndEqual" runat="server" Width="40px" GroupName="equip"
                                                                        Text="&lt;=" CssClass="ListControl" />
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtEquipLessAndEqual" runat="server" CssClass="form-control"
                                                                        Width="80px"></asp:TextBox>

                                                                </td>
                                                            </tr>

                                                        </table>
                                                        <table id="tblOpenCalls" style="display: none">
                                                            <tr>
                                                                <td colspan="4">Open Calls
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>&nbsp;
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="4">
                                                                    <asp:RadioButton ID="rdbOCAny" runat="server" GroupName="oc" Text="Any" CssClass="ListControl" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:RadioButton ID="rdbOCEqual" runat="server" Width="40px" GroupName="oc" Text="="
                                                                        CssClass="ListControl" />
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtOCEqual" runat="server" CssClass="form-control" Width="80px"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:RadioButton ID="rdbOCGreaterAndEqual" runat="server" Width="40px" GroupName="oc"
                                                                        Text="&gt;=" CssClass="ListControl" />
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtOCGreatAndEqual" runat="server" CssClass="form-control"
                                                                        Width="80px"></asp:TextBox>
                                                                    <span style="padding-left: 10px;">and</span>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:RadioButton ID="rdbOCLessAndEqual" runat="server" Width="40px" GroupName="oc"
                                                                        Text="&lt;=" CssClass="ListControl" />
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtOCLessAndEqual" runat="server" CssClass="form-control" Width="80px"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <table id="tblRoute" style="display: none">
                                                            <tr>
                                                                <td>Route
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:DropDownCheckBoxes ID="drpRoute" runat="server" CssClass="form-control" AddJQueryReference="false" UseButtons="false"
                                                                        Width="150px" UseSelectAllNode="false">
                                                                        <Style2 SelectBoxWidth="200" DropDownBoxBoxWidth="200" DropDownBoxBoxHeight="170"
                                                                            SelectBoxCssClass="ListControl" />
                                                                        <Texts SelectBoxCaption="Select" />
                                                                    </asp:DropDownCheckBoxes>
                                                                    <%--<asp:DropDownCheckBoxes ID="drpRoute" runat="server" AddJQueryReference="false" UseButtons="false"
                                                                    Width="150px" CssClass="form-control" UseSelectAllNode="false">
                                                                    <Style2 SelectBoxWidth="200" DropDownBoxBoxWidth="200" DropDownBoxBoxHeight="170"
                                                                        SelectBoxCssClass="ListControl" />
                                                                    <Texts SelectBoxCaption="Select" />
                                                                </asp:DropDownCheckBoxes>--%>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <table id="tblBuildingType" style="display: none">
                                                            <tr>
                                                                <td>Building Type
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:DropDownCheckBoxes ID="drpBuldingType" runat="server" CssClass="form-control" AddJQueryReference="false" UseButtons="false"
                                                                        Width="150px" UseSelectAllNode="false">
                                                                        <Style2 SelectBoxWidth="200" DropDownBoxBoxWidth="200" DropDownBoxBoxHeight="170"
                                                                            SelectBoxCssClass="ListControl" />
                                                                        <Texts SelectBoxCaption="Select" />
                                                                    </asp:DropDownCheckBoxes>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <table id="tblEquipmentState" style="display: none">
                                                            <tr>
                                                                <td>Equipment State
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:DropDownCheckBoxes ID="drpEquipmentState" runat="server" CssClass="form-control" AddJQueryReference="false" UseButtons="false"
                                                                        Width="150px" UseSelectAllNode="false">
                                                                        <Style2 SelectBoxWidth="200" DropDownBoxBoxWidth="200" DropDownBoxBoxHeight="170"
                                                                            SelectBoxCssClass="ListControl" />
                                                                        <Texts SelectBoxCaption="Select" />
                                                                    </asp:DropDownCheckBoxes>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <table id="tblInstalledOn" style="display: none">
                                                            <tr>
                                                                <td>Installed on
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:TextBox ID="txtInstalledOn" runat="server" CssClass="form-control" MaxLength="50"
                                                                        TabIndex="2"></asp:TextBox>
                                                                    <asp:CalendarExtender ID="txtInstalledOn_CalendarExtender" runat="server" Enabled="True"
                                                                        TargetControlID="txtInstalledOn">
                                                                    </asp:CalendarExtender>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <table id="tblSalesPerson" style="display: none">
                                                            <tr>
                                                                <td>Default Salesperson
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:DropDownCheckBoxes ID="drpDefaultSalesPerson" runat="server" CssClass="form-control" AddJQueryReference="false" UseButtons="false"
                                                                        Width="150px" UseSelectAllNode="false">
                                                                        <Style2 SelectBoxWidth="200" DropDownBoxBoxWidth="200" DropDownBoxBoxHeight="170"
                                                                            SelectBoxCssClass="ListControl" />
                                                                        <Texts SelectBoxCaption="Select" />
                                                                    </asp:DropDownCheckBoxes>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <table id="tblLocationSTax" style="display: none">
                                                            <tr>
                                                                <td>Location STax
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:DropDownCheckBoxes ID="drpLocationSTax" runat="server" CssClass="form-control" AddJQueryReference="false" UseButtons="false"
                                                                        Width="150px" UseSelectAllNode="false">
                                                                        <Style2 SelectBoxWidth="200" DropDownBoxBoxWidth="200" DropDownBoxBoxHeight="170"
                                                                            SelectBoxCssClass="ListControl" />
                                                                        <Texts SelectBoxCaption="Select" />
                                                                    </asp:DropDownCheckBoxes>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <table id="tblEquipmentCounts" style="display: none">
                                                            <tr>
                                                                <td colspan="4">Equipment Counts
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>&nbsp;
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="4">
                                                                    <asp:RadioButton ID="rdbEquipmentCountsAny" runat="server" GroupName="equipmentcounts" Text="Any" CssClass="ListControl" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:RadioButton ID="rdbEquipmentCountsEqual" runat="server" Width="40px" GroupName="equipmentcounts"
                                                                        Text="=" CssClass="ListControl" />
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtEquipmentCountsEqual" runat="server" CssClass="form-control" Width="80px"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:RadioButton ID="rdbEquipmentCountsGreaterAndEqual" runat="server" Width="40px" GroupName="equipmentcounts"
                                                                        Text="&gt;=" CssClass="ListControl" />
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtEquipmentCountsGreatAndEqual" runat="server" CssClass="form-control"
                                                                        Width="80px"></asp:TextBox>
                                                                    <span style="padding-left: 10px;">and</span>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:RadioButton ID="rdbEquipmentCountsLessAndEqual" runat="server" Width="40px" GroupName="equipmentcounts"
                                                                        Text="&lt;=" CssClass="ListControl" />
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtEquipmentCountsLessAndEqual" runat="server" CssClass="form-control"
                                                                        Width="80px"></asp:TextBox>

                                                                </td>
                                                            </tr>

                                                        </table>
                                                        <table id="tblDoorFrameInstalled" style="display: none">
                                                            <tr>
                                                                <td>Door Frame Installed
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:TextBox ID="txtDoorFrameInstalled" runat="server" CssClass="form-control datepicker_mom" MaxLength="50"></asp:TextBox>
                                                                    <%--<asp:CalendarExtender ID="txtDoorFrameInstalledt_CalendarExtender" runat="server" Enabled="True"
                                                                    Format="MM/dd/yyyy" ViewStateMode="Enabled" TargetControlID="txtDoorFrameInstalled">
                                                                </asp:CalendarExtender>--%>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <table id="tblDoorFrameDelivered" style="display: none">
                                                            <tr>
                                                                <td>Door Frame Delivered
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:TextBox ID="txtDoorFrameDelivered" runat="server" CssClass="form-control datepicker_mom" MaxLength="50"></asp:TextBox>
                                                                    <%--<asp:CalendarExtender ID="txtDoorFrameDelivered_CalendarExtender" runat="server" Enabled="True"
                                                                    Format="MM/dd/yyyy" ViewStateMode="Enabled" TargetControlID="txtDoorFrameDelivered">
                                                                </asp:CalendarExtender>--%>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </fieldset>
                                            </div>
                                            <div style="float: right;">
                                                <fieldset style="border-color: #CDCDCD; width: 320px;">
                                                    <legend><b>CURRENT FILTER CHOICES</b></legend>
                                                    <div style="margin-top: 20px; margin-left: 7px;">
                                                        <div style="height: 265px; border: solid 1px black; overflow: auto;">
                                                            <table>
                                                                <thead>
                                                                    <tr style="background: gray; color: White;">
                                                                        <td style="width: 100px; height: 25px;">FILTER
                                                                        </td>
                                                                        <td style="width: 220px; height: 25px">SET TO
                                                                        </td>
                                                                    </tr>
                                                                </thead>
                                                            </table>
                                                            <table id="tblFilterChoices">
                                                                <tbody>
                                                                </tbody>
                                                            </table>
                                                        </div>
                                                    </div>
                                                    <br />
                                                    <div style="text-align: center; padding-bottom: 10px;">
                                                        <input type="button" class="myButton" value="Remove Selected Filter" id="btnRemoveFilter" />
                                                    </div>
                                                </fieldset>
                                            </div>
                                        </div>
                                        <div id="tab3" class="tab header-footer-info" style="height: 300px;">
                                            <div style="float: left;">
                                                <fieldset style="border-color: #CDCDCD; width: 460px;">
                                                    <legend><b>SHOW HEADER INFORMATION:</b></legend>
                                                    <table style="width: 400px;">
                                                        <tr>
                                                            <td colspan="2" style="width: 150px; padding-left: 25px;">
                                                                <asp:CheckBox runat="server" ID="chkMainHeader" Text="Main Header" CssClass="ListControl"
                                                                    Checked="true" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 150px; padding-left: 25px;">
                                                                <asp:CheckBox runat="server" ID="chkCompanyName" Text="Company Name" CssClass="ListControl" />
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtCompanyName" runat="server" Width="215px" CssClass="form-control"
                                                                    Enabled="false"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 150px; padding-left: 25px;">
                                                                <asp:CheckBox runat="server" ID="chkReportTitle" Text="Report Title" CssClass="ListControl" />
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtReportTitle" runat="server" Width="215px" CssClass="form-control"
                                                                    Enabled="false"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 150px; padding-left: 25px;">
                                                                <asp:CheckBox runat="server" ID="chkSubtitle" Text="Subtitle" CssClass="ListControl" />
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtSubtitle" runat="server" Width="215px" CssClass="form-control"
                                                                    Enabled="false"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 150px; padding-left: 25px;">
                                                                <asp:CheckBox runat="server" ID="chkDatePrepared" Text="Date Prepared" CssClass="ListControl"
                                                                    Checked="true" />
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="drpDatePrepared" runat="server" Width="215px" CssClass="browser-default"
                                                                    Enabled="false">
                                                                    <asp:ListItem Value="12/31/01" Selected="True">12/31/01</asp:ListItem>
                                                                    <asp:ListItem Value="Dec 31, 01">Dec 31, 01</asp:ListItem>
                                                                    <asp:ListItem Value="December 31, 01">December 31, 01</asp:ListItem>
                                                                    <asp:ListItem Value="Dec 31, 2001">Dec 31, 2001</asp:ListItem>
                                                                    <asp:ListItem Value="December 31, 2001">December 31, 2001</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2" style="width: 150px; padding-left: 25px;">
                                                                <asp:CheckBox runat="server" ID="chkTimePrepared" Text="Time Prepared" CssClass="ListControl"
                                                                    Checked="true" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </fieldset>
                                                <br />
                                                <fieldset style="border-color: #CDCDCD; width: 460px;">
                                                    <legend><b>SHOW FOOTER INFORMATION:</b></legend>
                                                    <table style="width: 400px;">
                                                        <tr>
                                                            <td style="width: 150px; padding-left: 25px;">
                                                                <asp:CheckBox runat="server" ID="chkPageNumber" Text="Page Number" CssClass="ListControl" />
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="drpPageNumber" runat="server" Width="215px" CssClass="browser-default"
                                                                    Enabled="false">
                                                                    <asp:ListItem Value="Page 1">Page 1</asp:ListItem>
                                                                    <asp:ListItem Value="pg 1">pg 1</asp:ListItem>
                                                                    <asp:ListItem Value="p 1">p 1</asp:ListItem>
                                                                    <asp:ListItem Value="<1>"><1></asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 170px; padding-left: 25px;">
                                                                <asp:CheckBox runat="server" ID="chkExtraFootLine" Text="Extra Footer Line" CssClass="ListControl" />
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtExtraFooterLine" runat="server" Width="215px" CssClass="form-control"
                                                                    Enabled="false"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </fieldset>
                                            </div>
                                            <div style="float: right">
                                                <fieldset style="border-color: #CDCDCD; width: 290px; height: 230px;">
                                                    <legend><b>PAGE LAYOUT:</b></legend>
                                                    <table style="width: 180px;">
                                                        <tr>
                                                            <td>
                                                                <span>Alignment</span>
                                                            </td>
                                                            <td style="padding-left: 15px;">
                                                                <asp:DropDownList ID="drpAlignment" runat="server" Width="170px" CssClass="browser-default">
                                                                    <asp:ListItem Value="Standard" Selected="True">Standard</asp:ListItem>
                                                                    <asp:ListItem Value="Left">Left</asp:ListItem>
                                                                    <asp:ListItem Value="Right">Right</asp:ListItem>
                                                                    <asp:ListItem Value="Centered">Centered</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </fieldset>
                                            </div>
                                        </div>
                                        <div id="tab4" class="tab" style="height: 150px;">
                                            <div style="float: left;">
                                                <fieldset style="border-color: #CDCDCD; width: 460px; height: 100px;">
                                                    <legend><b>PDF Page Size:</b></legend>
                                                    <table style="width: 400px; padding-top: 15px; padding-left: 15px;">
                                                        <tr>
                                                            <td>Select Size:
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="drpPDFPageSize" runat="server" Width="215px" CssClass="browser-default">
                                                                    <asp:ListItem Value="Auto" Selected="True">Auto Width</asp:ListItem>
                                                                    <asp:ListItem Value="11X17">11X17</asp:ListItem>
                                                                    <asp:ListItem Value="A0">A0</asp:ListItem>
                                                                    <asp:ListItem Value="A1">A1</asp:ListItem>
                                                                    <asp:ListItem Value="A10">A10</asp:ListItem>
                                                                    <asp:ListItem Value="A2">A2</asp:ListItem>
                                                                    <asp:ListItem Value="A3">A3</asp:ListItem>
                                                                    <asp:ListItem Value="A4">A4</asp:ListItem>
                                                                    <asp:ListItem Value="A4_LANDSCAPE">A4_LANDSCAPE</asp:ListItem>
                                                                    <asp:ListItem Value="A5">A5</asp:ListItem>
                                                                    <asp:ListItem Value="A6">A6</asp:ListItem>
                                                                    <asp:ListItem Value="A7">A7</asp:ListItem>
                                                                    <asp:ListItem Value="A8">A8</asp:ListItem>
                                                                    <asp:ListItem Value="A9">A9</asp:ListItem>
                                                                    <asp:ListItem Value="ARCH_A">ARCH_A</asp:ListItem>
                                                                    <asp:ListItem Value="ARCH_B">ARCH_B</asp:ListItem>
                                                                    <asp:ListItem Value="ARCH_C">ARCH_C</asp:ListItem>
                                                                    <asp:ListItem Value="ARCH_D">ARCH_D</asp:ListItem>
                                                                    <asp:ListItem Value="ARCH_E">ARCH_E</asp:ListItem>
                                                                    <asp:ListItem Value="B0">B0</asp:ListItem>
                                                                    <asp:ListItem Value="B1">B1</asp:ListItem>
                                                                    <asp:ListItem Value="B10">B10</asp:ListItem>
                                                                    <asp:ListItem Value="B2">B2</asp:ListItem>
                                                                    <asp:ListItem Value="B3">B3</asp:ListItem>
                                                                    <asp:ListItem Value="B4">B4</asp:ListItem>
                                                                    <asp:ListItem Value="B5">B5</asp:ListItem>
                                                                    <asp:ListItem Value="B6">B6</asp:ListItem>
                                                                    <asp:ListItem Value="B7">B7</asp:ListItem>
                                                                    <asp:ListItem Value="B8">B8</asp:ListItem>
                                                                    <asp:ListItem Value="B9">B9</asp:ListItem>
                                                                    <asp:ListItem Value="CROWN_OCTAVO">CROWN_OCTAVO</asp:ListItem>
                                                                    <asp:ListItem Value="CROWN_QUARTO">CROWN_QUARTO</asp:ListItem>
                                                                    <asp:ListItem Value="DEMY_OCTAVO">DEMY_OCTAVO</asp:ListItem>
                                                                    <asp:ListItem Value="DEMY_QUARTO">DEMY_QUARTO</asp:ListItem>
                                                                    <asp:ListItem Value="EXECUTIVE">EXECUTIVE</asp:ListItem>
                                                                    <asp:ListItem Value="FLSA">FLSA</asp:ListItem>
                                                                    <asp:ListItem Value="FLSE">FLSE</asp:ListItem>
                                                                    <asp:ListItem Value="HALFLETTER">HALFLETTER</asp:ListItem>
                                                                    <asp:ListItem Value="ID_1">ID_1</asp:ListItem>
                                                                    <asp:ListItem Value="ID_2">ID_2</asp:ListItem>
                                                                    <asp:ListItem Value="ID_3">ID_3</asp:ListItem>
                                                                    <asp:ListItem Value="LARGE_CROWN_OCTAVO">LARGE_CROWN_OCTAVO</asp:ListItem>
                                                                    <asp:ListItem Value="LARGE_CROWN_QUARTO">LARGE_CROWN_QUARTO</asp:ListItem>
                                                                    <asp:ListItem Value="LEDGER">LEDGER</asp:ListItem>
                                                                    <asp:ListItem Value="LEGAL">LEGAL</asp:ListItem>
                                                                    <asp:ListItem Value="LEGAL_LANDSCAPE">LEGAL_LANDSCAPE</asp:ListItem>
                                                                    <asp:ListItem Value="LETTER">LETTER</asp:ListItem>
                                                                    <asp:ListItem Value="LETTER_LANDSCAPE">LETTER_LANDSCAPE</asp:ListItem>
                                                                    <asp:ListItem Value="NOTE">NOTE</asp:ListItem>
                                                                    <asp:ListItem Value="PENGUIN_LARGE_PAPERBACK">PENGUIN_LARGE_PAPERBACK</asp:ListItem>
                                                                    <asp:ListItem Value="PENGUIN_SMALL_PAPERBACK">PENGUIN_SMALL_PAPERBACK</asp:ListItem>
                                                                    <asp:ListItem Value="POSTCARD">POSTCARD</asp:ListItem>
                                                                    <asp:ListItem Value="ROYAL_OCTAVO">ROYAL_OCTAVO</asp:ListItem>
                                                                    <asp:ListItem Value="ROYAL_QUARTO">ROYAL_QUARTO</asp:ListItem>
                                                                    <asp:ListItem Value="SMALL_PAPERBACK">SMALL_PAPERBACK</asp:ListItem>
                                                                    <asp:ListItem Value="TABLOID">TABLOID</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </fieldset>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <footer>
                                <div class="btnlinks" style="float: right; height: 40px">
                                    <a href="#" id="btnApply" data-modal-id="dvSaveReport">Apply</a>
                                    <a href="#" class="js-modal-close" id="btnCancel">Cancel</a>
                                </div>
                            </footer>
                        </div>
                        <div id="dvSaveReport" class="modal-box" runat="Server">
                            <header>
                                <a href="#" class="js-modal-close close">×</a>
                                <h3>Save Report</h3>
                            </header>
                            <div class="modal-body">
                                <div class="alert alert-danger" runat="server" id="divInfo" style="display: none">
                                    You don't have permission to customize this report. Please choose another title.
                                </div>
                                <table style="padding-left: 250px; height: 50px; width: 500px;">
                                    <tr>
                                        <td style="text-align: right">
                                            <b style="font-size: 14px;">Report Name:</b>
                                        </td>
                                        <td style="padding-left: 10px;">
                                            <asp:TextBox ID="txtReportName" runat="server" Width="360px" Height="20px" Font-Size="14px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right">
                                            <b style="font-size: 14px;">Is Global:</b>
                                        </td>
                                        <td style="padding-left: 10px;">
                                            <div>
                                                <input type="checkbox" id="chkIsGlobal" runat="server" />
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <footer>
                                <div class="btnlinks" style="float: right; height: 40px">
                                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                        <ContentTemplate>
                                            <asp:LinkButton ID="btnSaveReport" runat="server" Text="Save Report" OnClientClick="return EmptyReportName();"
                                                OnClick="btnSaveReport_Click"></asp:LinkButton>
                                            <a href="#" class="js-modal-close" id="btnCancel2">Cancel</a>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </footer>
                        </div>
                        <asp:HiddenField runat="server" ID="hdnColumnList" />
                        <asp:HiddenField runat="server" ID="hdnColumnWidth" />
                        <asp:HiddenField runat="server" ID="hdnCustomizeReportName" />
                        <asp:HiddenField runat="server" ID="hdnCustomizeReportTitle" />
                        <asp:HiddenField runat="server" ID="hdnCustomizeReportSubject" />
                        <asp:HiddenField runat="server" ID="hdnIsStock" />
                        <asp:HiddenField runat="server" ID="hdnReportAction" />
                        <asp:HiddenField runat="server" ID="hdnDrpSortBy" />
                        <asp:HiddenField runat="server" ID="hdnLstColumns" />
                        <asp:HiddenField runat="server" ID="hdnFilterColumns" />
                        <asp:HiddenField runat="server" ID="hdnFilterValues" />
                        <asp:HiddenField runat="server" ID="hdnMainHeader" />
                        <asp:HiddenField runat="server" ID="hdnDivToExport" />
                        <asp:HiddenField runat="server" ID="hdnSendReportType" />
                    </div>
                </div>
                <!-- edit-tab end -->
                <div class="clearfix"></div>
            </div>
            <!-- END DASHBOARD STATS -->
            <div class="clearfix"></div>
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
                                            <i class="mdi-action-swap-vert-circle"></i>&nbsp; Open Job Report
                                        </div>
                                        <div class="buttonContainer">
                                            <div class="btnlinks">
                                                <asp:LinkButton runat="server" ID="LnkSend" Text="Send" OnClick="hideModalPopupViaServerConfirm_Click" OnClientClick="ShowLoading();" ValidationGroup="mail" />
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
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" Width="300"
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
                                            <asp:LinkButton CommandArgument='<%# Container.DataItem %>' ID="btnAttachmentDel" OnClientClick="return false;"
                                                Style="color: #000;" runat="server" Text='<%# System.IO.Path.GetFileName(Container.DataItem.ToString()) %>'></asp:LinkButton>
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
                                            <asp:Label ID="lblEmailRecordCount" runat="server">0 Record(s) found</asp:Label>
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
                                                    <a id="aSelectTo" onclick="SelectEmailsFromGrid('inputTo');" href="javascript:void(0);"><span>To &nbsp;</span></a>
                                                </div>
                                            </div>
                                            <div class="input-field col s11" style="margin-top: -10px;">
                                                <input name="inputTo" type="text" value="" id="inputTo" class="txtUserName form-control validate" />
                                            </div>
                                            <div class="input-field col s1"style="margin-top: 0px;">
                                                <div class="srchtitle  srchtitlecustomwidth btnlinks">
                                                    <a id="aSelectCc" onclick="SelectEmailsFromGrid('inputCc');" href="javascript:void(0);"><span>Cc &nbsp;</span></a>
                                                </div>
                                            </div>
                                            <div class="input-field col s11" style="margin-top: -10px;">
                                                <input name="inputTo" type="text" value="" id="inputCc" class="txtUserName form-control validate" />
                                            </div>
                                            <div class="input-field col s1" style="margin-top: 0px;">
                                                <div class="srchtitle  srchtitlecustomwidth btnlinks">
                                                    <a id="aSelectBcc" onclick="SelectEmailsFromGrid('inputBcc');" href="javascript:void(0);"><span>Bcc</span></a>
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
</asp:Content>
