<%@ Page Title="Ticket List || MOM" Language="C#" MasterPageFile="~/Mom.master" AutoEventWireup="true" Inherits="TicketListView" CodeBehind="TicketListView.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <!--Grid Control-->
    <link href="Design/css/grid.css" rel="stylesheet" />
    <!--File Upload Control-->
    <link href="Design/js/plugins/dropify/css/dropify.css" type="text/css" rel="stylesheet" media="screen,projection">

    <link href="Design/css/pikaday.css" rel="stylesheet" />

    <style type="text/css">
        .massupdatecss {
            padding-left: 0px !important;
        }

        html {
            position: fixed;
            height: 100%;
            overflow: hidden;
        }

        body {
            width: 100vw;
            height: 100vh;
            overflow-y: scroll;
            overflow-x: hidden;
            -webkit-overflow-scrolling: touch;
        }

        .validatorPopup table {
            width: 1px;
        }

        #ctl00_ContentPlaceHolder1_RadAjaxPanel_GroupTicket {
            display: none !important;
        }

        .TicketlistTooltip {
            background: #000 none repeat scroll 0 0;
            filter: alpha(opacity=80);
            -moz-opacity: 0.80;
            opacity: 0.80;
            border-radius: 0px !important;
            color: #fff;
            display: none;
            padding: 10px;
            position: absolute;
            width: 300px;
            z-index: 1000;
            margin-bottom: 20px;
        }

            .TicketlistTooltip:after {
                top: 0%;
                left: 0%;
                border: solid transparent;
                content: " ";
                height: 0;
                width: 0;
                position: absolute;
                pointer-events: none;
                border-color: rgba(0, 0, 0, 0);
                border-top-color: #000;
                border-width: 10px;
                margin-left: -10px;
                margin-bottom: 20px;
            }

        .RadGrid .rgNoRecords > td {
            padding-left: 15px !important;
            font-size: 15px !important;
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

        .labelButton {
            padding: 5px 10px 5px 10px;
            font-size: 0.9em;
            float: left;
            line-height: 19px !important;
            border-radius: 3px;
            background-color: #1C5FB1 !important;
            color: #fff !important;
            margin: 3px -9px;
            cursor: pointer;
        }

        div.rmSlide {
            top: 0 !important;
        }

        #ctl00_ContentPlaceHolder1_RadAjaxPanel_GroupTicket {
            display: none;
        }

        div.rmSlide > ul.rmVertical, div.rmSlide > div.rmScrollWrap > ul.rmVertical {
            padding-left: 30px !important;
            padding-right: 10px !important;
        }

        div.rmSlide input[type=checkbox] {
            vertical-align: middle !important;
            display: inline-block !important;
        }

        .dropdown-content li {
            white-space: normal;
        }

        .GroupStatus input.rcbCheckBox {
            display: none !important;
        }

        .GroupStatus label {
            font-weight: bolder !important;
            opacity: 100% !important;
        }

        .RadGvTicketList [id$='AddNewRecordButton'] {
            display: none;
        }

        .RadGrid_Bootstrap .rgFilterRow .riTextBox {
            border: none !important;
            border-bottom: solid 1px !important;
            border-radius: 0 !important;
        }

        div [id$='FilterTextBox_manualinvoice'] {
            margin-top: -14px !important;
        }

        .rgDataDiv {
            padding-bottom: 150px;
        }

        @media screen and (max-width: 2048px) {

            .rgDataDiv {
                padding-bottom: 150px;
            }
        }

        @media screen and (max-width: 2304px) {

            .rgDataDiv {
                padding-bottom: 150px;
            }
        }

        @media screen and (max-width: 1920px) {

            .rgDataDiv {
                padding-bottom: 150px;
            }
        }

        @media screen and (max-width: 1706px) {

            .rgDataDiv {
                padding-bottom: 150px;
            }
        }

        @media screen and (max-width: 1688px) {

            .rgDataDiv {
                padding-bottom: 150px;
            }
        }

        @media screen and (max-width: 1366px) {

            .rgDataDiv {
                padding-bottom: 150px;
            }
        }
    </style>


    <%-- ----OLD Design------%>


    <script type="text/javascript">
        function CssClearLabel() {
            $('#<%=lblDay.ClientID%>').removeClass("labelactive");
            $('#<%=lblWeek.ClientID%>').removeClass("labelactive");
            $('#<%=lblMonth.ClientID%>').removeClass("labelactive");
            $("#<%=lblQuarter.ClientID%>").removeClass("labelactive");
            $('#<%=lblYear.ClientID%>').removeClass("labelactive");
        }
        function SetActiveDateRangeCss(rangeName) {
            debugger
            CssClearLabel();
            if (rangeName == "Day")
                $("#<%= lblDay.ClientID%>").addClass("labelactive");
            else if (rangeName == "Week")
                $("#<%= lblWeek.ClientID%>").addClass("labelactive");
            else if (rangeName == "Month")
                $("#<%= lblMonth.ClientID%>").addClass("labelactive");
            else if (rangeName == "Quarter")
                $("#<%= lblQuarter.ClientID%>").addClass("labelactive");
            else if (rangeName == "Year")
                $("#<%= lblYear.ClientID%>").addClass("labelactive");
            document.getElementById('<%= hdnTicketListSelectDtRange.ClientID%>').value = rangeName;
        }


        function OpenNewWindow(MyPath) {
            //alert(MyPath);
            var Path = 'addinvoice?uid=' + MyPath;
            window.open(Path, "PopupWindow", 'width=400px,height=400px,top=150,left=250');
            //window.open(MyPath, "", "toolbar=no,status=no,menubar=no,location=center,scrollbars=no,resizable=no,height=500,width=657");
        }
        function MassReview() {

            //Ref SECO-450 we should not create projects automatically for mass review


            var IsQBInt = document.getElementById('<%=hdnIsQBInt.ClientID%>').value;

            if (Page_ClientValidate()) {
                var alertmass = "";
                if (IsQBInt != "1") {
                    alertmass = 'Please note tickets without a project number will be excluded.';
                }
                if (confirm('Do you want to make changes? \n\n' + alertmass)) {
                    return true;
                }
                else { return false; }

            } else {

                return false;
            }
        }

        function selectReview(chkTimesheet) {

            //if ($(chkTimesheet).is(":checked")) {
            //    $(chkTimesheet).parent().find('input[id*=chkReview]').attr("checked", "checked");
            //}
        }

        function RFC_Click(hyperlink) {
            $('#divRFC').slideToggle('fast');
            return true;
        }
        function AddTicketClick(hyperlink) {
            var id = document.getElementById('<%= hdnAddeTicket.ClientID%>').value;
            if (id == "Y") {
                return true;
            } else {
                noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                return false;
            }
        }
        function EditTicketClick(hyperlink) {

            var id = document.getElementById('<%= hdnEditeTicket.ClientID%>').value;
            if (id == "Y") { return editticket(); } else {
                noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                return false;
            }
        }
        function CopyTicketClick(hyperlink) {

            var id = document.getElementById('<%= hdnAddeTicket.ClientID%>').value;
            if (id == "Y") { return Copyticket(); } else {
                noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                return false;
            }
        }
        function isDeleteResolvedTicket() {
            var isDeleteResolvedTicket = true;
            var resolvedId = document.getElementById('<%= hdnDeleteResolvedTicket.ClientID%>').value;
            if (resolvedId == "N") {
                var gvOpenCalls = document.getElementById('<%= RadGvTicketList.ClientID %>');
                $('input:checkbox[id$=chkSelect]:checked', gvOpenCalls).each(function (index, item) {
                    var statusValue = $(this).parent().parent().next().next().next().next().next().next().next().next().next().next().next().next().next().next().next()[0].innerText;

                    if (statusValue == "Completed") {
                        isDeleteResolvedTicket = false
                        //noty({ text: 'You do not have delete completed ticket permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                        //return false;
                    }
                });
                return isDeleteResolvedTicket;
            }
        }
        function DeleteTicketClick(hyperlink) {


            var id = document.getElementById('<%= hdnDeleteTicket.ClientID%>').value;
            var ss = isDeleteResolvedTicket();
            console.log(ss);
            if (ss == false) {
                noty({ text: 'You do not have delete completed ticket permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                return false;
            }
            else if (id == "Y") {
                return DeleteAlert();

            } else {

                noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                return false;
            }
        }

        function emailAllConfirm() {
            var mess = "Are you sure you want to email a completed ticket report to all locations in the list?";
            if (confirm(mess)) { return true; }
            else { return false; }
        }

        function VoidedTicket(hyperlink) {

            var id = document.getElementById('<%= hdnTicketVoidPermission.ClientID%>').value;

            if (id == "Y") {

                var mess = "All open tickets listed will be voided. Are you sure you want to proceed?";

                if (confirm(mess)) { return true; }

                else {

                    return false;
                }

            } else {
                noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                return false;
            }
        }

    </script>

    <script type="text/javascript">

        function pageLoad() {





            $addHandler($get("A1"), 'click', hideModalPopupViaClientCust);

        }

        function SelectAll(obj) {
            debugger;

            var grid = document.getElementById("<%= RadGvTicketList.ClientID %>");
            var inputs = grid.getElementsByTagName("input");

            for (var i = 0; i < inputs.length; i++) {
                if (inputs[i].type == "checkbox") {
                    inputs[i].checked = obj.checked;
                }
            }
        }

        function hideModalPopupViaClientCust(ev) {
            ev.preventDefault();
            var modalPopupBehavior = $find('PMPBehaviour');
            modalPopupBehavior.hide();
        }

        function showModalPopupViaClientCust(lblTicketId, lblComp) {
            //            document.getElementById('<%= iframeCustomer.ClientID %>').width = "1024px";
            document.getElementById('<%= iframeCustomer.ClientID %>').src = "addticket.aspx?id=" + lblTicketId + "&comp=" + lblComp;
            document.getElementById('<%= Panel2.ClientID %>').style.display = "none";
            var modalPopupBehavior = $find('PMPBehaviour');
            modalPopupBehavior.show();
        }

        function showModalPopupViaClientCustLogin(lblTicketId, lblComp) {
            //            document.getElementById('<%= iframeCustomer.ClientID %>').width = "1024px";
            document.getElementById('<%= iframeCustomer.ClientID %>').src = "Printticket.aspx?popup=1&id=" + lblTicketId + "&c=" + lblComp;
            document.getElementById('<%= Panel2.ClientID %>').style.display = "none";
            var modalPopupBehavior = $find('PMPBehaviour');
            modalPopupBehavior.show();
        }

        function DeleteAlert() {
            var gvOpenCalls = document.getElementById('<%= RadGvTicketList.ClientID %>');
            var result = false;
            $('input:checkbox[id$=chkSelect]:checked', gvOpenCalls).each(function (index, item) {
                var id = $(this).parent().parent().find('span[id$=lblTicketId]');
                result = confirm('Are you sure you want to delete ticket # ' + id.text() + ' ?');
            });
            return result;
        }

        function editticket() {
            $("#<%=RadGvTicketList.ClientID %>").find('tr:not(:first,:last)').each(function () {
                var $tr = $(this);
                $tr.find('input[type="checkbox"]:checked').each(function (index, value) {
                    var ticket = $tr.find('span[id*=lblTicketId]').text();
                    var comp = $tr.find('span[id*=lblComp]').text();
                    if ('<%= Session["type"].ToString()%>' == "c") {
                        var format = document.getElementById('<%= hdnTicketReportFormat.ClientID%>').value;
                        var url = "printticket.aspx?id=" + ticket + "&c=" + comp;
                        if (format == "mrt") {
                            var url = "TicketReport.aspx?id=" + ticket + "&fr=tlv";
                        }

                        window.open(url, '_blank');
                    }
                    else {
                        var url = "addticket.aspx?id=" + ticket + "&comp=" + comp;
                        window.open(url, '_blank');
                    }
                });
            });
        }

        function Copyticket() {
            $("#<%=RadGvTicketList.ClientID %>").find('tr:not(:first,:last)').each(function () {
                var $tr = $(this);
                $tr.find('input[type="checkbox"]:checked').each(function (index, value) {
                    var ticket = $tr.find('span[id*=lblTicketId]').text();
                    var comp = $tr.find('span[id*=lblComp]').text();
                    if ('<%= Session["type"].ToString()%>' == "c") {
                        var url = "printticket.aspx?id=" + ticket + "&c=" + comp;
                        window.open(url, '_blank');
                    }
                    else {
                        var url = "addticket.aspx?copy=1&id=" + ticket + "&comp=" + comp;
                        window.open(url, '_blank');
                    }
                });
            });
        }

        function HoverMenutext(row, tooltip, event) {
            var left = event.pageX - (150) + 'px';
            //var top = event.pageY + (10) + 'px';
            //$('#' + tooltip).css({ top: top, left: left }).show();
            $('#' + tooltip).css({ left: left }).show();
        }

        function ShowRestoreGridSettingsButton() {
            debugger
            document.getElementById("<%=lbSaveGridSettings.ClientID %>").style.display = "none";
            document.getElementById("<%=lbRestoreGridSettings.ClientID %>").style.display = "block";
        }

        function SetDefaultDateRangeCss() {
            debugger
            CssClearLabel();
            $("#<%= lblWeek.ClientID%>").addClass("labelactive");
            document.getElementById('<%= hdnTicketListSelectDtRange.ClientID%>').value = "Week";
        }

    </script>

    <style>
        .ModalWindowAbs {
            position: absolute !important;
        }

        .cursorstyle {
            cursor: pointer;
        }

        #chkcatlist label {
            padding-right: 5px !important;
            padding-bottom: 2px;
        }

        #chkcatlist input {
            height: 0px !important;
        }

        .togglehide {
            display: none;
        }

        .toggleshow {
            display: block;
        }

        .chklist label {
            margin-left: 10px !important;
        }

        .chklist input {
            height: 12px !important;
        }

        .spanStyle {
            color: grey;
            font-weight: normal;
            font-size: 14px;
            font-family: sans-serif;
            text-align: center;
            align-self: center;
        }

        .form-control1, .form-control_2.input-sm {
            border: 1px solid #e0e0e0;
            padding: 5px;
            color: #808080;
            background: #fff;
            width: 100%;
            font-size: small;
            font-weight: 300;
            font-family: sans-serif;
            border-radius: 2px !important;
            -webkit-appearance: none;
            outline: none;
            height: 28px;
        }

        .btn {
            border: 1px solid #0086b3;
            font-weight: 500;
            letter-spacing: 0px;
            outline: ridge;
            height: 28px;
            margin-top: -4px !important;
        }

            .btn:focus, .btn:active:focus, .btn.active:focus {
                outline: ridge;
            }

        .btn-primary {
            background: #0099cc !important;
            border-color: #208eb3 !important;
            padding: 3px 10px !important;
        }

            .btn-primary:hover, .btn-primary:focus, .btn-primary:active, .btn-primary.active, .open > .dropdown-toggle.btn-primary {
                background: #33a6cc !important;
                height: 28px;
            }

            .btn-primary:active, .btn-primary.active {
                background: #007299 !important;
                box-shadow: none;
                height: 28px;
            }

        .istyle {
            padding-right: 0px;
            margin-right: -1px;
            margin-top: -4px !important;
            padding: 2px !important;
            margin-left: -1px !important;
        }

        .dropdown-content {
            width: 220px !important;
            max-height: unset !important;
        }

            .dropdown-content .submenu {
                min-width: 40px !important;
                background: url("https://cdn0.iconfinder.com/data/icons/navigation-set-arrows-part-one/32/ChevronRight-16.png") no-repeat scroll;
                background-position: center right;
                z-index: 999;
            }

                .dropdown-content .submenu a {
                    background-color: transparent !important;
                }

                .dropdown-content .submenu:hover {
                    background-color: #1C5FB1 !important;
                }
    </style>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
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
        <img src="images/wheel.GIF" alt="Be patient..." style="position: ` fixed; margin-top: 25%; margin-left: 50%;" />
    </div>
    <asp:HiddenField runat="server" ID="hdnHideDates" Value="0" />
    <asp:HiddenField runat="server" ID="hdnIsShowAll" Value="0" />
    <%------------Ajax Manager-------------%>

    <telerik:RadAjaxManager ID="RadAjaxManager_TicketList" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="RadGvTicketList">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGvTicketList" />
                    <telerik:AjaxUpdatedControl ControlID="RadAjaxPanelSearch" />
                    <telerik:AjaxUpdatedControl ControlID="lblRecordCount" />
                    <telerik:AjaxUpdatedControl ControlID="txtfromDate" />
                    <telerik:AjaxUpdatedControl ControlID="txtToDate" />
                    <telerik:AjaxUpdatedControl ControlID="txtSearch" />
                    <telerik:AjaxUpdatedControl ControlID="lblRecordCount" />

                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="lnkDelete">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGvTicketList" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnEdit">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGvTicketList" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="lnkClear">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="lnkClear" />
                    <telerik:AjaxUpdatedControl ControlID="txtfromDate" />
                    <telerik:AjaxUpdatedControl ControlID="txtToDate" />
                    <telerik:AjaxUpdatedControl ControlID="RadAjaxPanelAdvancedSearch" />
                    <telerik:AjaxUpdatedControl ControlID="RadAjaxPanelSearch" />
                    <telerik:AjaxUpdatedControl ControlID="RadGvTicketList" />
                    <telerik:AjaxUpdatedControl ControlID="chkcatlist" />

                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="lnkShowAll">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadAjaxPanelAdvancedSearch" />
                    <telerik:AjaxUpdatedControl ControlID="RadAjaxPanelSearch" />
                    <telerik:AjaxUpdatedControl ControlID="RadGvTicketList" />
                    <telerik:AjaxUpdatedControl ControlID="txtfromDate" />
                    <telerik:AjaxUpdatedControl ControlID="txtToDate" />
                    <telerik:AjaxUpdatedControl ControlID="hdnHideDates" />
                    <telerik:AjaxUpdatedControl ControlID="chkcatlist" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="hideModalPopupViaServer">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGvTicketList" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnSearch">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGvTicketList" />
                    <telerik:AjaxUpdatedControl ControlID="RadAjaxPanel_GroupTicket" />
                    <telerik:AjaxUpdatedControl ControlID="RadAjaxPanelSearch" />
                    <telerik:AjaxUpdatedControl ControlID="txtfromDate" />
                    <telerik:AjaxUpdatedControl ControlID="txtToDate" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnRefresh">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGvTicketList" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="chkHideTicketDesc">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGvTicketList" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RadGvRequestForService">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGvRequestForService" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="ddlSearch">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadAjaxPanelSearch" />
                    <telerik:AjaxUpdatedControl ControlID="txtSearch" />
                    <telerik:AjaxUpdatedControl ControlID="txtfromDate" />
                    <telerik:AjaxUpdatedControl ControlID="txtToDate" />
                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="cbReview">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="reviewPnl" />
                    <telerik:AjaxUpdatedControl ControlID="ddlStatus" />
                    <telerik:AjaxUpdatedControl ControlID="ddlReviewed" />
                    <telerik:AjaxUpdatedControl ControlID="ddlPayroll" />
                    <telerik:AjaxUpdatedControl ControlID="RadGvTicketList" />
                    <telerik:AjaxUpdatedControl ControlID="lblRecordCount" />
                </UpdatedControls>

            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnMassUpdate">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGvTicketList" />
                    <telerik:AjaxUpdatedControl ControlID="lblRecordCount" />
                    <telerik:AjaxUpdatedControl ControlID="txtfromDate" />
                    <telerik:AjaxUpdatedControl ControlID="txtToDate" />
                    <telerik:AjaxUpdatedControl ControlID="txtSearch" />
                </UpdatedControls>
            </telerik:AjaxSetting>


            <telerik:AjaxSetting AjaxControlID="ddllocation">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="btnSearch" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="lnkMailAll">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_gvLogs" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="lnkRestoreGridSettings">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGvTicketList" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="lnkSaveGridSettings">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGvTicketList" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>



    <%-- -------Loading------------%>

    <%--<telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel_Ticket" runat="server">
    </telerik:RadAjaxLoadingPanel>

    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1_RSTicket" runat="server">
    </telerik:RadAjaxLoadingPanel>

    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel_Review" runat="server">
    </telerik:RadAjaxLoadingPanel>--%>

    <%-- -------Loading------------%>


    <telerik:RadCodeBlock ID="codeBlock1" runat="server">
        <script type="text/javascript">
            function pageLoad() {
                var grid = $find("<%= RadGvTicketList.ClientID %>");
                var columns = grid.get_masterTableView().get_columns();
                for (var i = 0; i < columns.length; i++) {
                    columns[i].resizeToFit(false, true);
                }

                var grid = $find("<%= RadGvRequestForService.ClientID %>");
                var columns = grid.get_masterTableView().get_columns();
                for (var i = 0; i < columns.length; i++) {
                    columns[i].resizeToFit(false, true);
                }
            }
            var requestInitiator = null;
            var selectionStart = null;

            function requestStart(sender, args) {
                requestInitiator = document.activeElement.id;
                if (document.activeElement.tagName == "INPUT" && document.activeElement.type == 'text') {
                    selectionStart = document.activeElement.selectionStart;
                }
            }

            function responseEnd(sender, args) {
                var element = document.getElementById(requestInitiator);
                if (element && element.tagName == "INPUT" && element.type == 'text') {
                    element.focus();
                    element.selectionStart = selectionStart;
                }
            }

        </script>
        <style type="text/css">
            .w-80{
                    width: 180px;
                }
            @media only screen and (min-width: 250px) and (max-width: 700px) {
                .w-80{
                    width: 100%;
                }
                .RadGrid .rgCommandRow{
                    line-height: 24px;
                }
                .align-c{
                    text-align:center;
                }
            }
        </style>
    </telerik:RadCodeBlock>

    <%------------Ajax Manager-------------%>

    <div class="divbutton-container">
        <div id="divButtons">
            <div id="breadcrumbs-wrapper">

                <header>
                    <div class="container row-color-grey">
                        <div class="row">
                            <div class="col s12 m12 l12">
                                <div class="row">
                                    <div class="page-title"><i class="mdi-image-filter-frames"></i>&nbsp;Tickets</div>
                                    <div class="buttonContainer">
                                        <div class="btnlinks">
                                            <asp:HyperLink ID="lnkAddticket" TabIndex="23" runat="server" NavigateUrl="addticket.aspx" OnClick='return AddTicketClick(this)' ToolTip="Add New Ticket" CssClass="icon-addnew" Target="_blank">Add</asp:HyperLink>
                                        </div>
                                        <div class="btnlinks">
                                            <asp:LinkButton ID="btnEdit" runat="server" OnClientClick='return EditTicketClick(this)' CssClass="icon-edit" ToolTip="Edit">Edit</asp:LinkButton>
                                        </div>
                                        <div class="btnlinks menuAction">
                                            <a onclick="DropdownMenu()" class="dropbtn" style="cursor: pointer;">Actions
                                            </a>
                                        </div>
                                        <ul id="drpMenu" class="nomgn hideMenu menuList">
                                            <li>
                                                <div class="btnlinks">
                                                    <asp:LinkButton ID="lnkCopy" runat="server" OnClientClick='return CopyTicketClick(this)' CssClass="icon-edit" ToolTip="Copy">Copy</asp:LinkButton>
                                                </div>
                                            </li>
                                            <li>
                                                <div class="btnlinks">
                                                    <asp:LinkButton ID="lnkDelete" runat="server" CausesValidation="False" OnClick="lnkDelete_Click" Text="Delete" ToolTip="Delete"
                                                        OnClientClick='return DeleteTicketClick(this)' TabIndex="23"></asp:LinkButton>
                                                </div>
                                            </li>
                                            <li>
                                                <div class="btnlinks">
                                                    <asp:LinkButton ID="lnkVoided" OnClick="btnVoided_Click" OnClientClick="return VoidedTicket(this)" runat="server"> Void </asp:LinkButton>
                                                </div>
                                            </li>
                                            <li>
                                                <div class="btnlinks">
                                                    <asp:LinkButton ID="lnkExcel" runat="server" OnClick="lnkExcel_Click">Export to Excel</asp:LinkButton>
                                                </div>
                                            </li>
                                            <li>
                                                <div class="btnlinks">
                                                    <asp:LinkButton ID="lnkPDF" runat="server" Text="PDF" CausesValidation="true"
                                                        OnClick="lnkPDF_Click" Visible="true"> 
                                                    </asp:LinkButton>
                                                </div>
                                            </li>
                                            <li>
                                                <div class="btnlinks">

                                                    <%--OnClick="lnkPrint_Click"--%>
                                                    <%-- OnClientClick="tabchng(); return checkTranslation();"--%>
                                                    <asp:LinkButton class="dropdown-button" data-beloworigin="true" data-activates="dynamicUI" ID="lnkPrint" runat="server" CausesValidation="false" OnClientClick="return false;"
                                                        Visible="true" Text="Reports" ToolTip="Print Ticket">
                                                    </asp:LinkButton>
                                                </div>
                                                <%--      <li> <asp:LinkButton ID="lnkTicketListwithWageCategory" OnClick="lnkTicketListwithWageCategory_Click" runat="server" >TicketList with WageCategory</asp:LinkButton></li>
                                                --%>
                                                <ul id="dynamicUI" class="dropdown-content">
                                                    <li>
                                                        <asp:LinkButton ID="lnkBuildingReportTemplate" OnClick="lnkBuildingReportTemplate_Click" runat="server">Building Report Template</asp:LinkButton>

                                                    </li>
                                                    <li>
                                                        <asp:LinkButton ID="lnkCategoryDueReport" Visible="false" OnClick="lnkCategoryDueReport_Click" runat="server">Category Due Report</asp:LinkButton>
                                                    </li>

                                                    <li>
                                                        <a class='dropdown-button2' data-activates='dropdown7' data-hover="hover" data-alignment="left">New Call Report<i style="float: right;" class="mdi-av-play-arrow"></i></a>

                                                    </li>
                                                    <li>
                                                        <a class='dropdown-button2' data-activates='dropdown3' data-hover="hover" data-alignment="left">Call Warning Report<i style="float: right;" class="mdi-av-play-arrow"></i></a>

                                                    </li>
                                                    <li>
                                                        <a class='dropdown-button2' data-activates='dropdown4' data-hover="hover" data-alignment="left">Completed Ticket Report<i style="float: right;" class="mdi-av-play-arrow"></i></a>

                                                    </li>
                                                    <li>
                                                        <asp:LinkButton ID="lnkDetailsReport" OnClick="lnkDetailsReport_Click" runat="server"> Details Report</asp:LinkButton>

                                                    </li>
                                                    <li>
                                                        <asp:LinkButton ID="lnkEquipmentHistoryPastXDays" Visible="false" OnClick="lnkEquipmentHistoryPastXDays_Click" runat="server">Equipment History Past 30 Days</asp:LinkButton>
                                                    </li>
                                                    <li>
                                                        <asp:LinkButton ID="lnkExpenseReport" OnClick="lnkExpenseReport_Click" runat="server">Expense Report</asp:LinkButton>

                                                    </li>
                                                    <li>
                                                        <asp:LinkButton ID="lnkInstallationSchedule" OnClick="lnkInstallationSchedule_Click" runat="server">Installation Schedule</asp:LinkButton>

                                                    </li>
                                                    <li>
                                                        <asp:LinkButton ID="lnkLaborbyDepartment" OnClick="lnkLaborbyDepartment_Click" runat="server">Labor by Department</asp:LinkButton>

                                                    </li>
                                                    <li>
                                                        <asp:LinkButton ID="lnkOpenTicketReportbyRoutes" OnClick="lnkOpenTicketReportbyRoutes_Click" runat="server">Open Ticket by Routes Report </asp:LinkButton>
                                                    </li>

                                                    <li>
                                                        <asp:LinkButton ID="lnkServiceSchedule" OnClick="lnkServiceSchedule_Click" runat="server">Schedule</asp:LinkButton>
                                                    </li>
                                                    <li>
                                                        <a class='dropdown-button2' data-activates='dropdown8' data-hover="hover" data-alignment="left" id="lnkServiceCallPastXDays">Monthly reports <i style="float: right;" class="mdi-av-play-arrow"></i></a>
                                                    </li>
                                                    <li>
                                                        <a class='dropdown-button2' data-activates='dropdown2' data-hover="hover" data-alignment="left" id="lnkServiceCallPastXDays">Service Call <i style="float: right;" class="mdi-av-play-arrow"></i></a>
                                                    </li>
                                                    <li>
                                                        <a class='dropdown-button2' data-activates='dropdown6' data-hover="hover" data-alignment="left" id="lnkServiceCallPastXDays">Ticket Reports <i style="float: right;" class="mdi-av-play-arrow"></i></a>

                                                    </li>


                                                    <li>
                                                        <a class='dropdown-button2' data-activates='dropdown5' data-hover="hover" data-alignment="left">Timesheet Report<i style="float: right;" class="mdi-av-play-arrow"></i></a>

                                                    </li>
                                                    <li>
                                                        <asp:LinkButton ID="lnkWorkerReport" OnClick="lnkWorkerReport_Click" runat="server">Worker Report</asp:LinkButton>

                                                    </li>
                                                </ul>
                                                <ul id='dropdown2' class='dropdown-content'>
                                                    <li><a href="ServiceCallHistoryPastXdaysReport.aspx?PastXDays=7">Past 7 Days</a></li>
                                                    <li><a href="ServiceCallHistoryPastXdaysReport.aspx?PastXDays=30">Past 30 Days</a></li>
                                                    <li><a href="ServiceCallHistoryPastXdaysReport.aspx?PastXDays=90">Past 90 Days</a></li>
                                                    <li><a href="ServiceCallHistoryPastXdaysReport.aspx?PastXDays=180">Past 180 Days</a></li>
                                                    <li><a href="ServiceCallHistoryPastXdaysReport.aspx?PastXDays=365">Past 365 Days</a></li>
                                                </ul>
                                                <ul id='dropdown3' class='dropdown-content'>
                                                    <li>
                                                        <asp:LinkButton ID="lnkCallWarningReport" OnClick="lnkCallWarningReport_Click" runat="server">Call Warning by Route</asp:LinkButton></li>
                                                    <li>
                                                        <asp:LinkButton ID="lnkCallWarningSalespersonReport" OnClick="lnkCallWarningSalespersonReport_Click" runat="server">Call Warning by Salesperson</asp:LinkButton></li>
                                                    <li>
                                                        <asp:LinkButton ID="lnkNewCallWarningReport" OnClick="lnkNewCallWarningReport_Click" runat="server">New Car Call Warning</asp:LinkButton></li>
                                                </ul>
                                                <ul id='dropdown4' class='dropdown-content'>
                                                    <li>
                                                        <asp:LinkButton ID="lnkCompletedTicketReport" OnClick="lnkCompletedTicketReport_Click" runat="server">Completed Ticket Report</asp:LinkButton></li>
                                                    <li>
                                                        <asp:LinkButton ID="lnkCompletedTicketSignatureReport" OnClick="lnkCompletedTicketSignatureReport_Click" runat="server">Completed Ticket Signature</asp:LinkButton></li>
                                                    <li>
                                                        <asp:LinkButton ID="lnkCompletedTicketEntrapment" OnClick="lnkCompletedTicketEntrapment_Click" runat="server">Completed Ticket Level</asp:LinkButton></li>
                                                </ul>
                                                <ul id='dropdown5' class='dropdown-content'>
                                                    <li>
                                                        <asp:LinkButton ID="lnkTimeSheetReport" OnClick="lnkTimeSheetReport_Click" runat="server">Timesheet Report</asp:LinkButton></li>
                                                    <li>
                                                        <asp:LinkButton ID="lnkTimeSheetCertifiedProject" OnClick="lnkTimeSheetCertifiedProject_Click" runat="server">Timesheet Certified Project Report</asp:LinkButton></li>
                                                    <li>
                                                        <asp:LinkButton ID="knkTimesheetbyDepartment" OnClick="knkTimesheetbyDepartment_Click" runat="server">Timesheet Report by Department</asp:LinkButton></li>
                                                    <li>
                                                        <asp:LinkButton ID="lnkTimeSheetReportNoTT" OnClick="lnkTimeSheetReportNoTT_Click" runat="server">Timesheet Report (no TT)</asp:LinkButton></li>
                                                    <li>
                                                        <asp:LinkButton ID="lnkTimesheetbyWageCategory" OnClick="lnkTimesheetbyWageCategory_Click" runat="server">Timesheet Report by Wage Category</asp:LinkButton></li>
                                                    <li>
                                                        <asp:LinkButton Visible="true" ID="lnkTimeRecap" OnClick="lnkTimeRecap_Click" runat="server">Time Recap 
                                                        </asp:LinkButton>
                                                    </li>
                                                    <li>
                                                        <asp:LinkButton ID="lnkModTimeSheetReport" OnClick="lnkModTimeSheetReport_Click" runat="server">Mod Timesheet Report</asp:LinkButton></li>
                                                </ul>
                                                <ul id='dropdown6' class='dropdown-content'>
                                                    <li>
                                                        <asp:LinkButton ID="lnkTicketCategoryHistory" OnClick="lnkTicketCategoryHistory_Click" runat="server">Ticket Category History</asp:LinkButton>

                                                    </li>
                                                    <li>
                                                        <asp:LinkButton ID="lnkTicketReport" OnClick="lnkTicketReport_Click" runat="server">Ticket Report</asp:LinkButton>

                                                    </li>
                                                    <li>
                                                        <asp:LinkButton ID="lnkTicketReportbyWO" OnClick="lnkTicketReportbyWO_Click" runat="server">Ticket Report by WO</asp:LinkButton>

                                                    </li>
                                                    <li>
                                                        <asp:LinkButton ID="lnkTicketReportSignature" OnClick="lnkTicketReportSignature_Click" runat="server">Ticket Report (Signature)</asp:LinkButton>

                                                    </li>
                                                    <li>
                                                        <asp:LinkButton ID="lnkTicketListReport" OnClick="lnkTicketListReport_Click" runat="server">Ticket List Report</asp:LinkButton>

                                                    </li>
                                                    <li>
                                                        <asp:LinkButton ID="lnkOpenTicketDetailsByMechanicReport" OnClick="lnkOpenTicketDetailsByMechanicReport_Click" runat="server">Open Ticket Details By Mechanic</asp:LinkButton>
                                                    </li>
                                                    <li>
                                                        <asp:LinkButton ID="lnkOpenTicketsByMechanicReport" OnClick="lnkOpenTicketsByMechanicReport_Click" runat="server">Open Tickets By Mechanic</asp:LinkButton>
                                                    </li>
                                                    <li>
                                                        <asp:LinkButton ID="lnkTicketListPayroll" Visible="false" OnClick="lnkTicketListPayroll_Click" runat="server">Ticket List Payroll Hours Report</asp:LinkButton>
                                                    </li>
                                                </ul>
                                                <ul id='dropdown7' class='dropdown-content'>
                                                    <li>
                                                        <asp:LinkButton ID="lnkCallbackReport" OnClick="lnkCallbackReport_Click" runat="server">Callback Report</asp:LinkButton>

                                                    </li>
                                                    <li>
                                                        <asp:LinkButton ID="lnkCallsPerRouteReport" OnClick="lnkCallsPerRouteReport_Click" runat="server">Calls per Route Report</asp:LinkButton>

                                                    </li>
                                                </ul>
                                                <ul id='dropdown8' class='dropdown-content'>
                                                    <li>
                                                        <asp:LinkButton ID="lnkMonthlyMaintenanceTicketReport" OnClick="lnkMonthlyMaintenanceTicketReport_Click" runat="server">Monthly Maintenance</asp:LinkButton>
                                                    </li>
                                                    <li>
                                                        <asp:LinkButton ID="lnkMonthlyRecurringHoursReport" OnClick="lnkMonthlyRecurringHoursReport_Click" runat="server">Monthly Recurring Hours By Location Report</asp:LinkButton>
                                                    </li>
                                                    <li>
                                                        <asp:LinkButton ID="lnkMonthlyServiceCallBackReport" OnClick="lnkMonthlyServiceCallBackReport_Click" runat="server">Monthly Service CallBack</asp:LinkButton>
                                                    </li>
                                                </ul>

                                            </li>
                                            <li>
                                                <div class="btnlinks">
                                                    <asp:LinkButton ID="lnkMailAll" OnClick="lnkMailAll_Click" OnClientClick="return emailAllConfirm();" runat="server">Email All</asp:LinkButton>
                                                </div>
                                            </li>

                                            <li>
                                                <div class="btnlinks">
                                                    <asp:LinkButton ID="lnkRestoreGridSettings" runat="server" CausesValidation="False" OnClick="lnkRestoreGridSettings_Click"
                                                        Style="display: none">Restore Grid</asp:LinkButton>
                                                </div>
                                                <div class="btnlinks">
                                                    <asp:LinkButton ID="lnkSaveGridSettings" runat="server" CausesValidation="False" OnClick="lnkSaveGridSettings_Click"
                                                        Style="display: none">Save Grid</asp:LinkButton>
                                                </div>

                                                <label id="lbSaveGridSettings" runat="server" class="labelButton" tooltip="Save Grid Settings" style="display: none">
                                                    <input type="radio" id="rdSaveGridSettings" onclick="SaveGridSettings();" />
                                                    Save Grid
                                                </label>
                                                <label id="lbRestoreGridSettings" runat="server" class="labelButton" tooltip="Restore Default Settings for Grid" style="display: none">
                                                    <input type="radio" id="rdRestoreGridSettings" onclick="RestoreGridSettings();" />
                                                    Restore Grid
                                                </label>
                                            </li>
                                        </ul>
                                    </div>
                                    <div class="btnclosewrap">
                                        <asp:LinkButton ID="lnkClose" ToolTip="Close" runat="server" CausesValidation="false"
                                            OnClick="lnkClose_Click"> <i class="mdi-content-clear"></i> </asp:LinkButton>
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
                            <ul class="anchor-links">
                                <li style="text-align: center;"><a class="add-btn-click">Advanced Search</a> </li>
                                <li id="liLogs" runat="server"><a href="#accrdlogs">Email History Log</a></li>
                            </ul>
                        </div>
                    </div>

                </div>
                <div id="stats" style="background-color: #fff !important;">
                    <div id="addinfo" class="form-section-row adv-sarch">

                        <telerik:RadAjaxPanel ID="RadAjaxPanelAdvancedSearch" runat="server" LoadingPanelID="RadAjaxLoadingPanelSearch">

                            <div class="form-section-row">
                                <div class="section-ttle">Search Criteria</div>
                                <div class="form-sectioncustom1">

                                    <div class="collapse_wrap">

                                        <div class="form-collapsewrap1" runat="server" id="divSuper">

                                            <div class="form_collapserow">
                                                <div class="input-field col s12">
                                                    <label class="drpdwn-label">
                                                        <asp:Label ID="lblSuper" runat="server" Text="Supervisor"></asp:Label></label>
                                                    <asp:DropDownList ID="ddlSuper" runat="server" AutoPostBack="True" CssClass="browser-default"
                                                        OnSelectedIndexChanged="ddlSuper_SelectedIndexChanged" TabIndex="6">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>


                                            <div class="form_collapserow">
                                                <div class="input-field col s12">
                                                    <label class="drpdwn-label">Chargeable</label>
                                                    <asp:DropDownList ID="ddlCharge" TabIndex="9" runat="server" CssClass="browser-default">
                                                        <asp:ListItem Value="">All</asp:ListItem>
                                                        <asp:ListItem Value="1">Yes</asp:ListItem>
                                                        <asp:ListItem Value="0">No</asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>

                                            <div class="form_collapserow" runat="server" id="divReviewed">
                                                <div class="input-field col s12">
                                                    <label class="drpdwn-label">
                                                        <asp:Label ID="lblReviewed" runat="server" Text="Reviewed"></asp:Label></label>
                                                    <asp:DropDownList ID="ddlReviewed" runat="server" TabIndex="12" CssClass="browser-default">
                                                        <asp:ListItem Value="">All</asp:ListItem>
                                                        <asp:ListItem Value="1">Yes</asp:ListItem>
                                                        <asp:ListItem Value="0">No</asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>

                                            <div class="form_collapserow" runat="server" id="divWorker">
                                                <div class="input-field col s12">
                                                    <label class="drpdwn-label">
                                                        <asp:Label ID="lblWorker" runat="server" Text="Worker"></asp:Label></label>

                                                    <asp:DropDownList ID="ddlworker" runat="server" CssClass="browser-default"
                                                        TabIndex="7">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>

                                            <div class="form_collapserow" runat="server" id="divRoute">
                                                <div class="input-field col s12">
                                                    <label class="drpdwn-label">
                                                        <asp:Label ID="lblRoute" runat="server" Text="Default Worker"></asp:Label></label>
                                                    <asp:DropDownList ID="ddlRoute" runat="server" CssClass="browser-default">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>

                                        </div>

                                        <div class="form-collapsewrap2">
                                            <div class="form_collapserow" runat="server" id="divStatus">
                                                <div class="input-field col s12">
                                                    <label class="drpdwn-label">
                                                        <asp:Label ID="lblStatus" runat="server" Text="Status"></asp:Label></label>

                                                    <asp:DropDownList ID="ddlStatus" runat="server" CssClass="browser-default"
                                                        TabIndex="10">
                                                        <asp:ListItem Value="-1">All</asp:ListItem>
                                                        <asp:ListItem Value="-2">All Open</asp:ListItem>
                                                        <asp:ListItem Value="0">Un-Assigned</asp:ListItem>
                                                        <asp:ListItem Value="1">Assigned</asp:ListItem>
                                                        <asp:ListItem Value="2">Enroute</asp:ListItem>
                                                        <asp:ListItem Value="3">Onsite</asp:ListItem>
                                                        <asp:ListItem Value="4">Completed</asp:ListItem>
                                                        <asp:ListItem Value="5">Hold</asp:ListItem>
                                                        <asp:ListItem Value="6">Voided</asp:ListItem>
                                                    </asp:DropDownList>

                                                </div>
                                            </div>


                                            <div class="form_collapserow">
                                                <div class="input-field col s12">
                                                    <label class="drpdwn-label">Department</label>
                                                    <asp:DropDownList ID="ddlDepartment" TabIndex="13" runat="server" CssClass="browser-default">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>

                                            <div class="form_collapserow">
                                                <div class="input-field col s12">
                                                    <label class="drpdwn-label">Portal</label>
                                                    <asp:DropDownList ID="ddlportal" TabIndex="13" runat="server" CssClass="browser-default">
                                                        <asp:ListItem Value="">All</asp:ListItem>
                                                        <asp:ListItem Value="1">Yes</asp:ListItem>
                                                        <asp:ListItem Value="0">No</asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>

                                            <div class="form_collapserow">
                                                <div class="input-field col s12">
                                                    <label class="drpdwn-label">Recommendation</label>

                                                    <asp:DropDownList ID="ddlRecommendation" TabIndex="5" runat="server" CssClass="browser-default">
                                                        <asp:ListItem Value="">All</asp:ListItem>
                                                        <asp:ListItem Value="1">Yes</asp:ListItem>
                                                        <asp:ListItem Value="0">No</asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="form_collapserow" runat="server" id="div1">
                                                <div class="input-field col s12">
                                                    <label class="drpdwn-label">Category</label>
                                                    <telerik:RadComboBox ID="chkcatlist" BackColor="White" CssClass="browser-default" runat="server" CheckBoxes="true" Width="102%" EnableCheckAllItemsCheckBox="true" Style="margin-top: 10px;">
                                                    </telerik:RadComboBox>
                                                </div>
                                            </div>
                                        </div>

                                    </div>

                                </div>

                                <div class="form-sectioncustom2">

                                    <div class="form_collapserow" runat="server" id="divTimeSheet">
                                        <div class="input-field col s12">
                                            <label class="drpdwn-label">
                                                <asp:Label ID="lblTimeSh" runat="server" Text="Time Sheet"></asp:Label></label>
                                            <asp:DropDownList ID="ddlTimeS" TabIndex="8" runat="server" CssClass="browser-default">
                                                <asp:ListItem Value="">All</asp:ListItem>
                                                <asp:ListItem Value="1">Yes</asp:ListItem>
                                                <asp:ListItem Value="0">No</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>

                                    <div class="form_collapserow" runat="server" id="divPayroll">
                                        <div class="input-field col s12">
                                            <label class="drpdwn-label">Payroll</label>
                                            <asp:DropDownList ID="ddlPayroll" TabIndex="5" runat="server" CssClass="browser-default">
                                                <asp:ListItem Value="-1">All</asp:ListItem>
                                                <asp:ListItem Value="1">Yes</asp:ListItem>
                                                <asp:ListItem Value="0">No</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>

                                    <div class="form_collapserow" runat="server" id="divMobile">
                                        <div class="input-field col s12">
                                            <label class="drpdwn-label">
                                                <asp:Label ID="lblMobile" runat="server" Text="Mobile"></asp:Label></label>
                                            <asp:DropDownList ID="ddlMobile" runat="server" CssClass="browser-default"
                                                TabIndex="11">
                                                <asp:ListItem Value="0">All</asp:ListItem>
                                                <asp:ListItem Value="2">Yes</asp:ListItem>
                                                <asp:ListItem Value="1">No</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>

                                    <div class="form_collapserow">
                                        <div class="input-field col s12">
                                            <label class="drpdwn-label">Invoiced</label>
                                            <asp:DropDownList ID="ddlInvoiced" runat="server" CssClass="browser-default"
                                                TabIndex="11">
                                                <asp:ListItem Value="0">All</asp:ListItem>
                                                <asp:ListItem Value="1">Yes</asp:ListItem>
                                                <asp:ListItem Value="2">No</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>

                                    <div class="form_collapserow" id="DivEmail" runat="server">
                                        <div class="input-field col s12">
                                            <label class="drpdwn-label">Email</label>
                                            <asp:DropDownList ID="ddlEmail" runat="server" CssClass="browser-default"
                                                TabIndex="11">
                                                <asp:ListItem Value="0">All</asp:ListItem>
                                                <asp:ListItem Value="1">Yes</asp:ListItem>
                                                <asp:ListItem Value="2">No</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>


                                    <div class="form_collapserow" runat="server" id="divWO" visible="false">
                                        <div class="input-field col s12">

                                            <label class="drpdwn-label">
                                                <asp:Label ID="lblWO" runat="server" Text="Work Order #" Visible="False"></asp:Label>
                                            </label>


                                            <asp:TextBox ID="txtWo" runat="server" CssClass="browser-default" MaxLength="50"
                                                Visible="False"></asp:TextBox>

                                        </div>
                                    </div>

                                    <div class="form_collapserow" runat="server" id="divShowAllSuper" visible="false">
                                        <div class="input-field col s12">

                                            <label class="drpdwn-label">
                                                <asp:Label ID="lblShowAll" runat="server" Text="Show All" Visible="False"></asp:Label></label>
                                            <asp:CheckBox ID="chkShowAll" runat="server" ToolTip="Show unassigned and tickets from other supervisors"
                                                Visible="False" />

                                        </div>
                                    </div>

                                </div>
                            </div>

                        </telerik:RadAjaxPanel>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div class="container">
        <div class="row">
            <div class="srchpane-advanced">
                <div class="srchpaneinner">
                    <%--                    <div class="srchtitle srchtitlecustomwidth" style="padding-left: 0px;">
                        Date
                    </div>--%>
                    <div class="srchinputwrap ">
                        <asp:DropDownList ID="ddlDateRange" runat="server" CssClass="browser-default select selectst">
                            <asp:ListItem Value="1">Ticket Date</asp:ListItem>
                            <asp:ListItem Value="2">Invoice Date</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="srchinputwrap">
                        <asp:TextBox ID="txtfromDate" runat="server"  TabIndex="3" class="srchcstm datepicker_mom w-80" MaxLength="50" AutoPostBack="false"></asp:TextBox>
                    </div>
                    <div class="srchinputwrap">
                        <asp:TextBox ID="txtToDate" runat="server"  class="srchcstm datepicker_mom w-80" TabIndex="4" MaxLength="50" AutoPostBack="false"></asp:TextBox>
                    </div>

                    <div class="srchinputwrap align-c">
                        <ul class="tabselect accrd-tabselect" id="testradiobutton">
                            <li>
                                <asp:LinkButton AutoPostBack="False" ID="decDate" runat="server" Text="<i class='mdi-hardware-keyboard-arrow-left'></i>" OnClientClick="dec_date('dec');return false;"></asp:LinkButton>
                            </li>
                            <li>
                                <label id="lblDay" runat="server">
                                    <%--<input type="radio" id="rdDay" name="rdCal" value="rdDay" OnCheckedChanged="rdDay_CheckedChanged"onclick="SelectDate('Day', 'ctl00_ContentPlaceHolder1_txtfromDate', 'ctl00_ContentPlaceHolder1_txtToDate', '#lblDay', 'hdnTicketListDate', 'rdCal')" />--%>
                                    <asp:RadioButton ID="rdDay" GroupName="rdCal" runat="server" AutoPostBack="false" onclick="SelectDate('Day')" />
                                    Day
                                </label>
                            </li>
                            <li>
                                <label id="lblWeek" runat="server">
                                    <%--<input type="radio" id="rdWeek" name="rdCal" value="rdWeek" OnCheckedChanged="rdWeek_CheckedChanged"onclick="SelectDate('Week', 'ctl00_ContentPlaceHolder1_txtfromDate', 'ctl00_ContentPlaceHolder1_txtToDate', '#ctl00_ContentPlaceHolder1_lblWeek', 'hdnTicketListDate', 'rdCal')" />--%>
                                    <asp:RadioButton ID="rdWeek" GroupName="rdCal" runat="server" AutoPostBack="false" onclick="SelectDate('Week')" />
                                    Week
                                </label>
                            </li>
                            <li>
                                <label id="lblMonth" runat="server">
                                    <%--<input type="radio" id="rdMonth" name="rdCal" value="rdMonth" OnCheckedChanged="rdMonth_CheckedChanged"onclick="SelectDate('Month', 'ctl00_ContentPlaceHolder1_txtfromDate', 'ctl00_ContentPlaceHolder1_txtToDate', '#ctl00_ContentPlaceHolder1_lblMonth', 'hdnTicketListDate', 'rdCal')" />--%>
                                    <asp:RadioButton ID="rdMonth" GroupName="rdCal" runat="server" AutoPostBack="false" onclick="SelectDate('Month')" />
                                    Month
                                </label>
                            </li>
                            <li>
                                <label id="lblQuarter" runat="server">
                                    <%--<input type="radio" id="rdQuarter" name="rdCal" value="rdQuarter" OnCheckedChanged="rdQuarter_CheckedChanged"onclick="SelectDate('Quarter', 'ctl00_ContentPlaceHolder1_txtfromDate', 'ctl00_ContentPlaceHolder1_txtToDate', '#ctl00_ContentPlaceHolder1_lblQuarter', 'hdnTicketListDate', 'rdCal')" />--%>
                                    <asp:RadioButton ID="rdQuarter" GroupName="rdCal" runat="server" AutoPostBack="false" onclick="SelectDate('Quarter')" />
                                    Quarter
                                </label>
                            </li>
                            <li>
                                <label id="lblYear" runat="server">
                                    <%--<input type="radio" id="rdYear" name="rdCal" value="rdYear" OnCheckedChanged="rdYear_CheckedChanged"onclick="SelectDate('Year', 'ctl00_ContentPlaceHolder1_txtfromDate', 'ctl00_ContentPlaceHolder1_txtToDate', '#ctl00_ContentPlaceHolder1_lblYear', 'hdnTicketListDate', 'rdCal')" />--%>
                                    <asp:RadioButton ID="rdYear" GroupName="rdCal" runat="server" AutoPostBack="false" onclick="SelectDate('Year')" />
                                    Year
                                </label>
                            </li>
                            <li>
                                <asp:LinkButton ID="incDate" runat="server" OnClientClick="dec_date('inc');return false" Text="<i class='mdi-hardware-keyboard-arrow-right'></i>"></asp:LinkButton>
                            </li>
                        </ul>
                    </div>



                    <telerik:RadAjaxPanel ID="RadAjaxPanelSearch" runat="server" LoadingPanelID="RadAjaxLoadingPanelSearch">

                        <div class="srchtitle srchtitlecustomwidth pad-le-15">
                            Search
                        </div>
                        <div class="srchinputwrap">

                            <asp:DropDownList ID="ddlSearch" TabIndex="1" runat="server" CssClass="browser-default selectst selectsml"
                                AutoPostBack="True" OnSelectedIndexChanged="ddlSearch_SelectedIndexChanged">
                                <asp:ListItem Value="">Select</asp:ListItem>
                                <asp:ListItem Value="t.ldesc4">Address</asp:ListItem>
                                <asp:ListItem Value="r.name">Customer</asp:ListItem>
                                <asp:ListItem Value="l.City">City</asp:ListItem>
                                <asp:ListItem Value="t.cat">Category</asp:ListItem>
                                <asp:ListItem Value="e.unit">Equipment ID</asp:ListItem>
                                <asp:ListItem Value="l.tag">Location</asp:ListItem>
                                <asp:ListItem Value="t.Level">Level</asp:ListItem>
                                <asp:ListItem Value="t.fdesc">Reason for service</asp:ListItem>
                                <asp:ListItem Value="l.state">State</asp:ListItem>
                                <asp:ListItem Value="t.ID">Ticket #</asp:ListItem>
                                <asp:ListItem Value="t.WorkOrder">WO #</asp:ListItem>
                                <asp:ListItem Value="t.descres">Work Comp Desc</asp:ListItem>
                                <asp:ListItem Value="l.Zip">Zip/Postal Code</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="srchinputwrap">
                            <asp:TextBox ID="txtSearch" runat="server" CssClass="srchcstm" placeholder="Search..."></asp:TextBox>
                        </div>
                        <div class="srchinputwrap">
                            <asp:DropDownList ID="ddlCategory" runat="server" CssClass="browser-default selectst selectsml"
                                Visible="false">
                            </asp:DropDownList>
                        </div>
                        <div class="srchinputwrap">
                            <telerik:RadComboBox ID="chkrcbLevel" BackColor="White" CssClass="browser-default" runat="server" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" Visible="false">
                            </telerik:RadComboBox>
                        </div>
                        <div class="srchinputwrap srchclr btnlinksicon">
                            <asp:LinkButton ID="btnSearch" runat="server" CausesValidation="false"
                                OnClick="btnSearch_Click"><i class="mdi-notification-sync"></i></asp:LinkButton>
                            <asp:LinkButton ID="btnRefresh" Style="display: none;" runat="server" CausesValidation="false"
                                OnClick="btnRefresh_Click"><i class="mdi-notification-sync"></i></asp:LinkButton>
                        </div>
                        <span class="tro trost">
                            <asp:Label ID="lblRecordCount" Style="font-style: italic;" runat="server"></asp:Label>
                        </span>



                    </telerik:RadAjaxPanel>

                </div>

                <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanelSearch" runat="server">
                </telerik:RadAjaxLoadingPanel>

            </div>

            <div runat="server" id="divLoc" visible="false">
                <div class="srchpane-advanced">
                    <div class="srchpaneinner">
                        <div class="srchinputwrap">
                            <asp:Label ID="lblLoc" runat="server" Text="Location" Visible="False"></asp:Label>
                        </div>
                        <div class="srchinputwrap">
                            <%--OnSelectedIndexChanged="ddllocation_SelectedIndexChanged"--%>
                            <asp:DropDownList ID="ddllocation" Style="width: 250px !important;" runat="server" CssClass="browser-default selectst" AutoPostBack="false"
                                Visible="False">
                            </asp:DropDownList>
                        </div>

                    </div>
                </div>
            </div>

            <div style="display: none;">
                <div id="divRequestForService" runat="server" class="container breadcrumbs-bg-custom">
                    <div class="row">
                        <div class="col s8 m8 l8">
                            <div class="row">
                                <ul class="anchor-links">
                                    <li style="text-align: center;">
                                        <a onclick='return RFC_Click(this)' id="imgrfc"><span style="color: black; font-weight: bold">Request for Service from Portal </span></a>
                                    </li>

                                </ul>
                            </div>
                        </div>

                    </div>
                </div>

                <div class="grid_container">
                    <div class="form-section-row mb">
                        <div class="table-scrollable">

                            <%---Rad Grid-----%>

                            <telerik:RadAjaxPanel ID="RSRadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1_RSTicket" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">

                                <div id="divRFC" style="display: none;" class="RadGrid RadGrid_Material">
                                    <telerik:RadGrid ID="RadGvRequestForService"
                                        RenderMode="Auto" AllowFilteringByColumn="false" ShowFooter="false" PageSize="10"
                                        ShowStatusBar="true" runat="server" AllowPaging="true" AllowSorting="true" Width="100%" PagerStyle-AlwaysVisible="true"
                                        OnNeedDataSource="RadGvRequestForService_NeedDataSource"
                                        AllowCustomPaging="True">
                                        <CommandItemStyle />
                                        <GroupingSettings CaseSensitive="false" />
                                        <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                                            <Selecting AllowRowSelect="True"></Selecting>
                                            <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                            <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                        </ClientSettings>
                                        <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="false" ShowFooter="True">
                                            <Columns>
                                                <telerik:GridTemplateColumn FilterDelay="5" DataField="id" AutoPostBackOnFilter="false" AllowFiltering="false" SortExpression="id"
                                                    CurrentFilterFunction="Contains" UniqueName="id" HeaderText="Ticket #" ShowFilterIcon="false" HeaderStyle-Width="50">
                                                    <ItemTemplate>
                                                        <asp:HyperLink ID="lblTicketId"
                                                            Text='<%# Bind("id") %>' Target="_blank"
                                                            ForeColor="#0066cc"
                                                            runat="server"
                                                            NavigateUrl='<%# String.Format("~/addticket.aspx?id={0}&comp=0&pop=1", DataBinder.Eval(Container.DataItem, "id")) %>' />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>

                                                <telerik:GridTemplateColumn FilterDelay="5" DataField="unit" AutoPostBackOnFilter="false" AllowFiltering="false" SortExpression="unit"
                                                    CurrentFilterFunction="Contains" UniqueName="unit" HeaderText="Equipment" ShowFilterIcon="false" HeaderStyle-Width="140">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" Text='<%#Eval("unit")%>' ID="lblEquip"></asp:Label>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>

                                                <telerik:GridTemplateColumn FilterDelay="5" DataField="edate" AutoPostBackOnFilter="false" AllowFiltering="false" SortExpression="edate"
                                                    CurrentFilterFunction="Contains" UniqueName="edate" HeaderText="Date" ShowFilterIcon="false" HeaderStyle-Width="100">
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label8" runat="server" Text='<%# Eval("edate", "{0:MM/dd/yy hh:mm tt}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>

                                                <telerik:GridTemplateColumn FilterDelay="5" DataField="customername" AutoPostBackOnFilter="false" AllowFiltering="false" CurrentFilterFunction="Contains" UniqueName="customername" ShowFilterIcon="false" HeaderStyle-Width="140" HeaderText="Customer Name" SortExpression="customername">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblc"
                                                            runat="server"
                                                            Text='<%# Eval("customername") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                </telerik:GridTemplateColumn>

                                                <telerik:GridTemplateColumn FilterDelay="5" DataField="locname" AutoPostBackOnFilter="false" AllowFiltering="false" CurrentFilterFunction="Contains" UniqueName="locname" ShowFilterIcon="false" HeaderStyle-Width="140" HeaderText="Location" SortExpression="locname">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblLoc"
                                                            runat="server"
                                                            Text='<%# Eval("locname") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                </telerik:GridTemplateColumn>

                                                <telerik:GridTemplateColumn FilterDelay="5" DataField="fullAddress" AutoPostBackOnFilter="false"
                                                    AllowFiltering="false" CurrentFilterFunction="Contains" UniqueName="fullAddress" ShowFilterIcon="false" HeaderStyle-Width="140" HeaderText="Address" SortExpression="Address">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAdd" runat="server" Text='<%# Bind("fullAddress") %>'></asp:Label>
                                                    </ItemTemplate>

                                                </telerik:GridTemplateColumn>

                                                <telerik:GridTemplateColumn FilterDelay="5" DataField="cat" AutoPostBackOnFilter="false"
                                                    AllowFiltering="false" CurrentFilterFunction="Contains" UniqueName="cat" ShowFilterIcon="false"
                                                    HeaderStyle-Width="100" HeaderText="Category" SortExpression="cat">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCategory" runat="server" Text='<%# Bind("cat") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>

                                                <telerik:GridTemplateColumn FilterDelay="5" DataField="assignname" AutoPostBackOnFilter="false" AllowFiltering="false" CurrentFilterFunction="Contains" UniqueName="assignname" ShowFilterIcon="false" HeaderStyle-Width="100" HeaderText="Status" SortExpression="assignname">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblStatus" runat="server" Text='<%# Bind("assignname") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>

                                                <telerik:GridTemplateColumn FilterDelay="5" DataField="Company" AutoPostBackOnFilter="false" AllowFiltering="false" CurrentFilterFunction="Contains" UniqueName="Company" ShowFilterIcon="false" HeaderStyle-Width="100" HeaderText="Company" SortExpression="Company">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCompany" runat="server" Text='<%# Bind("Company") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>

                                                <telerik:GridTemplateColumn FilterDelay="5" DataField="department" AutoPostBackOnFilter="false" AllowFiltering="false" CurrentFilterFunction="Contains" UniqueName="department" ShowFilterIcon="false" HeaderStyle-Width="100" Visible="false" HeaderText="Department" SortExpression="department">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDepart" runat="server" Text='<%# Eval("department") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>


                                            </Columns>

                                        </MasterTableView>
                                    </telerik:RadGrid>
                                </div>

                            </telerik:RadAjaxPanel>

                        </div>
                    </div>
                </div>
            </div>
            <%-- --------------Main Rad Grid------------------%>


            <div class="container breadcrumbs-bg-custom">
                <div class="row">
                    <div class="col s12 m12 l12">
                        <div class="row">
                            <span class="tro trost">
                                <asp:CheckBox ID="cbReview" Text="Review Tickets" class="css-checkbox sup-css" runat="server" OnCheckedChanged="cbReview_CheckedChanged" ForeColor="Black" AutoPostBack="true" />

                            </span>


                            <span class="tro trost" id="lnkRequestForService" visible="false" runat="server">|<a class="add-btn-click" href="AddTicketFromCustPortal.aspx" onclick='return AddTicketClick(this)' title="Request for service" target="_blank">Request for service</a>

                            </span>

                            <span class="tro trost">
                                <asp:CheckBox ID="chkHideTicketDesc" Text="Hide Ticket Description" class="css-checkbox sup-css" ForeColor="Black" AutoPostBack="true" OnCheckedChanged="chkHideTicketDesc_CheckedChanged" runat="server" />

                            </span>

                            <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanelSearch">

                                <div class="col lblsz2 lblszfloat">
                                    <div class="row">
                                        <span class="tro trost">
                                            <asp:LinkButton ID="lnkClear" runat="server" OnClick="lnkClear_Click">Clear</asp:LinkButton>
                                        </span>
                                        <span class="tro trost">
                                            <asp:LinkButton ID="lnkShowAll" runat="server"
                                                OnClick="lnkShowAll_Click">Show All Tickets</asp:LinkButton>
                                        </span>

                                    </div>
                                </div>
                            </telerik:RadAjaxPanel>
                        </div>

                        <asp:Panel ID="reviewPnl" runat="server" Visible="false">
                            <div class="row validatorPopup">
                                <div class="srchpane-advanced">
                                    <div class="srchpaneinner">

                                        <div class="col s3 m3 l3" runat="server" id="DivMassReview" visible="false">
                                            <div class="input-field col s6">
                                                <div class="btnlinks">
                                                    <asp:LinkButton ID="btnMassUpdate" runat="server" OnClick="btnMassUpdate_Click"
                                                        ValidationGroup="rev"
                                                        OnClientClick="return MassReview();">Mass update</asp:LinkButton>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col s3 m3 l3" runat="server" id="ddlReviewPayrollDiv" visible="false">
                                            <div class="input-field col s6">
                                                <label class="drpdwn-label">Payroll Item</label>
                                                <asp:DropDownList ID="ddlReviewPayroll" runat="server" CssClass="browser-default" TabIndex="12"></asp:DropDownList>
                                            </div>
                                        </div>

                                        <div class="col s3 m3 l3" runat="server" id="ddlReviewServiceDiv" visible="false">
                                            <div class="input-field col s6">
                                                <label class="drpdwn-label">Service Item</label>
                                                <asp:DropDownList ID="ddlReviewService" runat="server" CssClass="browser-default" TabIndex="12"></asp:DropDownList>
                                            </div>
                                        </div>

                                        <div class="col s3 m3 l3" runat="server" id="ddlReviewDepartmentDiv" visible="false">
                                            <div class="input-field col s6">
                                                <label class="drpdwn-label">Department  </label>
                                                <asp:DropDownList ID="ddlReviewDepartment" runat="server" Visible="true" CssClass="browser-default" TabIndex="12"></asp:DropDownList>
                                            </div>
                                        </div>



                                    </div>

                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                                        ControlToValidate="ddlReviewPayroll" Display="None" ErrorMessage="Payroll Item Required"
                                        SetFocusOnError="True" ValidationGroup="rev"></asp:RequiredFieldValidator>

                                    <asp:ValidatorCalloutExtender
                                        ID="ValidatorCalloutExtender1" runat="server" Enabled="True"
                                        PopupPosition="Left"
                                        TargetControlID="RequiredFieldValidator1">
                                    </asp:ValidatorCalloutExtender>

                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                                        ControlToValidate="ddlReviewService" Display="None" ErrorMessage="Service Item Required"
                                        SetFocusOnError="True" ValidationGroup="rev"></asp:RequiredFieldValidator>

                                    <asp:ValidatorCalloutExtender
                                        ID="ValidatorCalloutExtender2" runat="server" Enabled="True"
                                        PopupPosition="Left"
                                        TargetControlID="RequiredFieldValidator2">
                                    </asp:ValidatorCalloutExtender>

                                </div>
                            </div>
                        </asp:Panel>
                    </div>

                </div>
            </div>


            <div class="grid_container m-t-5">
                <div class="form-section-row mb">
                    <div class="table-scrollable">
                        <div class="RadGrid RadGrid_Material">

                            <%-- ---------------1-----------------%>

                            <%--<telerik:RadAjaxPanel ID="RadAjaxPanel_Ticket" runat="server" LoadingPanelID="RadAjaxLoadingPanel_Ticket" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">--%>
                            <telerik:RadGrid ID="RadGvTicketList" CssClass="RadGvTicketList"
                                RenderMode="Auto" AllowFilteringByColumn="true" ShowFooter="True" PageSize="50"
                                ShowStatusBar="true" runat="server" AllowPaging="True" AllowSorting="true"
                                EnableLinqExpressions="false"
                                PagerStyle-AlwaysVisible="true"
                                OnRowDataBound="RadGvTicketList_RowDataBound"
                                OnNeedDataSource="RadGvTicketList_NeedDataSource"
                                OnItemCreated="RadGvTicketList_ItemCreated"
                                OnExcelMLExportRowCreated="RadGvTicketList_ExcelMLExportRowCreated"
                                OnSortCommand="RadGvTicketList_SortCommand"
                                OnPreRender="RadGvTicketList_PreRender"
                                EmptyDataText="No Tickets Found..."
                                OnPageIndexChanged="RadGvTicketList_PageIndexChanged"
                                OnPageSizeChanged="RadGvTicketList_PageSizeChanged"
                                OnItemCommand="RadGvTicketList_ItemCommand"
                                AllowCustomPaging="True"
                                ClientSettings-AllowColumnsReorder="true"
                                ClientSettings-ReorderColumnsOnClient="true"
                                AllowAutomaticUpdates="True"
                                OnBatchEditCommand="RadGvTicketList_BatchEditCommand">
                                <CommandItemStyle />
                                <GroupingSettings CaseSensitive="false" />
                                <ClientSettings>
                                    <Selecting AllowRowSelect="True"></Selecting>
                                    <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                    <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                    <ClientEvents
                                        OnGridCreated="OnGridCreated" OnHeaderMenuShowing="headerMenuShowing"
                                        OnColumnHidden="ColumnSettingsChange" OnColumnShown="ColumnSettingsChange"
                                        OnColumnResized="ColumnSettingsChange" OnColumnSwapped="ColumnSettingsChange" />
                                </ClientSettings>

                                <MasterTableView DataKeyNames="id" UseAllDataFields="true" AutoGenerateColumns="false" AllowFilteringByColumn="true" ShowFooter="True" EnableHeaderContextMenu="true" EditMode="Batch" CommandItemDisplay="Top">
                                    <BatchEditingSettings EditType="Cell" HighlightDeletedRows="true" OpenEditingEvent="Click" />
                                    <Columns>

                                        <telerik:GridTemplateColumn UniqueName="Comp" Reorderable="false" Resizable="false" AutoPostBackOnFilter="false" AllowFiltering="false"
                                            HeaderText="" ShowFilterIcon="false" HeaderStyle-Width="55px">
                                            <HeaderTemplate>
                                                <div class="checkrow">

                                                    <asp:CheckBox ID="chkSelectAll" onclick="SelectAll(this)" Text="All" title="All" runat="server" Checked="false" class="css-checkbox massupdatecss" Style="color: black; font-weight: bold" ForeColor="Black" AutoPostBack="false" />

                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkSelect" Text="." Visible='<%# Eval("ClearPR").ToString().Trim() == "1" ? (!cbReview.Checked) : true %>' class="css-checkbox" runat="server" />
                                                <asp:Label ID="lblComp" Visible="true" Style="display: none" runat="server" Text='<%# Bind("Comp") %>'></asp:Label>
                                                <asp:Label ID="lblRes" runat="server" CssClass="TicketlistTooltip"
                                                    Text='<%# ShowHoverText(Eval("description"),Eval("fdescreason"), Eval("unit"),Eval("id"),Eval("EmailNotified"),Eval("EmailTime"),Eval("Tottime"),Eval("OT"), Eval("Reg"), Eval("NT"),Eval("DT"),Eval("TT")) %>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn UniqueName="ReviewCheck" AutoPostBackOnFilter="false" Reorderable="false" Resizable="false" AllowFiltering="false" Visible="false"
                                            HeaderText="" ShowFilterIcon="false" HeaderStyle-Width="50px">
                                            <HeaderTemplate>
                                                <div class="checkrow">
                                                    <asp:CheckBox ID="chkHeadReview" Text="R" title="Review" runat="server" Checked="false" class="css-checkbox massupdatecss" Style="color: black; font-weight: bold" ForeColor="Black" OnCheckedChanged="chkHeadReview_CheckedChanged" AutoPostBack="true" />


                                                </div>
                                            </HeaderTemplate>

                                            <ItemTemplate>
                                                <div class="checkrow">
                                                    <asp:CheckBox ID="chkReview" Text="." class="css-checkbox" Checked="false" runat="server" title="Review" />

                                                </div>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>


                                        <telerik:GridTemplateColumn UniqueName="ReviewCheckTimesheet" AutoPostBackOnFilter="false" Reorderable="false" Resizable="false" AllowFiltering="false" Visible="false"
                                            HeaderText="" ShowFilterIcon="false" HeaderStyle-Width="50px">
                                            <HeaderTemplate>
                                                <div class="checkrow">


                                                    <asp:CheckBox ID="chkHeadTimesheet" Text="T" title="Timesheet" runat="server" Checked="false" class="css-checkbox massupdatecss" Style="color: black; font-weight: bold" ForeColor="Black" OnCheckedChanged="chkHeadTimesheet_CheckedChanged" AutoPostBack="true" />
                                                </div>
                                            </HeaderTemplate>

                                            <ItemTemplate>
                                                <div class="checkrow">

                                                    <asp:CheckBox ID="chkTimesheet" Text="." class="css-checkbox" runat="server" Checked="false" title="Timesheet" />
                                                </div>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn UniqueName="PayrollHeader" AutoPostBackOnFilter="false" Reorderable="false" Resizable="false" AllowFiltering="false" Visible="false"
                                            HeaderText="" ShowFilterIcon="false" HeaderStyle-Width="55px">
                                            <HeaderTemplate>
                                                <div class="checkrow">

                                                    <asp:CheckBox ID="ChkHeadpayroll" Text="PR" title="Payroll" runat="server" Checked="false" class="css-checkbox massupdatecss" Style="color: black; font-weight: bold" ForeColor="Black" OnCheckedChanged="chkHeadPayroll_CheckedChanged" AutoPostBack="true" />

                                                </div>
                                            </HeaderTemplate>

                                            <ItemTemplate>
                                                <div class="checkrow">
                                                    <asp:CheckBox ID="chkPayroll" Text="." class="css-checkbox" runat="server" Checked="false" title="Payroll" />
                                                </div>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>



                                        <telerik:GridTemplateColumn Reorderable="false" Resizable="false" UniqueName="Documentcount" Visible="true" AutoPostBackOnFilter="false" AllowFiltering="false"
                                            HeaderText="" ShowFilterIcon="false" HeaderStyle-Width="100">
                                            <ItemTemplate>
                                                <asp:Image ID="imgCompleted" runat="server" Width="15px" ToolTip='<%# Bind("assignname") %>' ImageUrl='<%# LockColor(Convert.ToInt16(Eval("assigned")) , Eval("assignname").ToString())%>' />
                                                <asp:Image ID="imgPortal" runat="server" Width="15px" ToolTip="Portal" ImageUrl='<%# Eval("fBy").ToString().Trim() == "Portal" ? "images/portal.png" : "data:image/png;base64,R0lGODlhAQABAIAAAAAAAP///yH5BAEAAAAALAAAAAABAAEAAAIBRAA7"%>' />
                                                    <asp:Image ID="imgMS" runat="server" Width="15px" ToolTip="MS" ImageUrl='<%# Eval("fBy").ToString().Trim() == "MOBILEUSER" ? "images/1331034893_pda.png" : "data:image/png;base64,R0lGODlhAQABAIAAAAAAAP///yH5BAEAAAAALAAAAAABAAEAAAIBRAA7"%>' />
                                            
                                                <asp:Image ID="imgStaus" runat="server" Width="15px" ToolTip="" ImageUrl='<%# StatusIcon(Eval("confirmed").ToString(), Eval("comp").ToString(), Eval("ClearCheck").ToString()) %>' />
                                                <asp:Image ID="imgDoc" runat="server" Width="15px" ToolTip="Documents" ImageUrl='<%# Eval("Documentcount").ToString() != "0" ? "images/Document.png" : "data:image/png;base64,R0lGODlhAQABAIAAAAAAAP///yH5BAEAAAAALAAAAAABAAEAAAIBRAA7"%>' />
                                                <asp:Image ID="imgSign" runat="server" Width="15px" ToolTip="Signature" ImageUrl='<%# Eval("signatureCount").ToString() != "0" ? "images/Signature.png" : "data:image/png;base64,R0lGODlhAQABAIAAAAAAAP///yH5BAEAAAAALAAAAAABAAEAAAIBRAA7"%>' />
                                                <asp:Image ID="imgCharge" runat="server" Width="15px" ToolTip="Chargeable" ImageUrl='<%# ChargeableImage(Eval("charge").ToString(), Eval("invoice").ToString(), Eval("manualinvoice").ToString(), Eval("statusName").ToString(), Eval("qbinvoiceid").ToString()) %>' />
                                                <asp:Image ID="imgTimeS" runat="server" Width="15px" ToolTip="Timesheet" ImageUrl='<%# Eval("TransferTime").ToString() != "0" ? "images/timesheet.png" : "data:image/png;base64,R0lGODlhAQABAIAAAAAAAP///yH5BAEAAAAALAAAAAABAAEAAAIBRAA7"%>' />
                                                <asp:Image ID="imgHigh" runat="server" Width="16px" ToolTip="Declined" ImageUrl='<%# Eval("high").ToString() != "0" ? "images/exclamation.png" : "data:image/png;base64,R0lGODlhAQABAIAAAAAAAP///yH5BAEAAAAALAAAAAABAAEAAAIBRAA7"%>' />
                                                <asp:Image ID="imgRecommend" runat="server" Width="16px" ToolTip="Recommendation" ImageUrl='<%# Eval("bremarks").ToString().Trim() != "" ? "images/thumb_up.png" : "data:image/png;base64,R0lGODlhAQABAIAAAAAAAP///yH5BAEAAAAALAAAAAABAAEAAAIBRAA7"%>' />
                                                <asp:Image ID="imgOverTime" runat="server" Width="16px" ToolTip="Over Time" ImageUrl='<%# (Eval("OT").ToString().Trim() != "0.00" || Eval("NT").ToString().Trim() != "0.00" || Eval("DT").ToString().Trim() != "0.00") ? "images/overtime.png" : "data:image/png;base64,R0lGODlhAQABAIAAAAAAAP///yH5BAEAAAAALAAAAAABAAEAAAIBRAA7"%>' />
                                                <i class="mdi-notification-sync" style="font-weight: 800;" width="16px" title="Recurring Ticket" id="imgisRecurring" runat="server" visible='<%# Eval("isRecurring").ToString().Trim() == "1" ? true:false %>'></i>
                                                <asp:Image ID="imgpayroll" runat="server" Width="23px" Height="15px" Style="font-weight: 900" ToolTip="Payroll" ImageUrl='<%# Eval("ClearPR").ToString().Trim() == "1" ? "images/payroll.png" : "data:image/png;base64,R0lGODlhAQABAIAAAAAAAP///yH5BAEAAAAALAAAAAABAAEAAAIBRAA7"%>' />



                                                  <asp:Image ID="imgPO" runat="server" Width="23px" Height="15px" Style="font-weight: 900" ToolTip="PO" ImageUrl='<%# Eval("IsPOicon").ToString().Trim() != "0" ? "images/PO.png" : "data:image/png;base64,R0lGODlhAQABAIAAAAAAAP///yH5BAEAAAAALAAAAAABAAEAAAIBRAA7"%>' />




                                                  <asp:Image ID="imgPU" runat="server" Width="23px" Height="15px" Style="font-weight: 900" ToolTip="Parts Used" ImageUrl='<%# Eval("IsPartsUsedIcon").ToString().Trim() != "0" ? "images/PU.png" : "data:image/png;base64,R0lGODlhAQABAIAAAAAAAP///yH5BAEAAAAALAAAAAABAAEAAAIBRAA7"%>' />



                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label runat="server" Text="Total :-"></asp:Label>
                                            </FooterTemplate>
                                        </telerik:GridTemplateColumn>
                                        <%--"--%>
                                     <%--   <telerik:GridBoundColumn DataField="locid" SortExpression="locid" HeaderText="Acct #" UniqueName="locid"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" HeaderStyle-Width="100"
                                            ShowFilterIcon="false">
                                        </telerik:GridBoundColumn>--%>
                                        <telerik:GridTemplateColumn DataField="locid" AutoPostBackOnFilter="true" 
                                            CurrentFilterFunction="Contains"  UniqueName="locid" HeaderText="Acct #"
                                            ShowFilterIcon="false" HeaderStyle-Width="100">
                                            <ItemTemplate>
                                                <asp:Label ID="locid" runat="server" Text='<%# Bind("locid") %>'></asp:Label>
                                            </ItemTemplate>

                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn DataField="ID" DataType="System.String"
                                            AutoPostBackOnFilter="true" SortExpression="ID" Reorderable="true"
                                            UniqueName="ID" AllowFiltering="true" CurrentFilterFunction="Contains"
                                            HeaderText="Ticket#" ShowFilterIcon="false" HeaderStyle-Width="80">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTicketId" Style="display: none" runat="server" Text='<%# Bind("id") %>'></asp:Label>
                                                <asp:Label ID="lblsignatureCount" Style="display: none" runat="server" Text='<%# Bind("signatureCount") %>'></asp:Label>
                                                <asp:HyperLink ID="lbllnk" runat="server" Text='<%#Eval("id")%>'></asp:HyperLink>
                                            </ItemTemplate>

                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn DataField="WorkOrder" AutoPostBackOnFilter="true" Reorderable="true"
                                            CurrentFilterFunction="Contains" SortExpression="WorkOrder" UniqueName="WorkOrder" HeaderText="WO#"
                                            ShowFilterIcon="false" HeaderStyle-Width="80">
                                            <ItemTemplate>
                                                <asp:Label ID="lblWOId" runat="server" Text='<%# Bind("WorkOrder") %>'></asp:Label>
                                            </ItemTemplate>

                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn DataField="edate" AutoPostBackOnFilter="true" AllowFiltering="false" DataType="System.String"
                                            CurrentFilterFunction="Contains" SortExpression="edate" UniqueName="edate" HeaderText="Date" ShowFilterIcon="false" HeaderStyle-Width="135">

                                            <ItemTemplate>
                                                <asp:Label ID="Label8" runat="server" Text='<%# Eval("edate", "{0:MM/dd/yy hh:mm tt}") %>'></asp:Label>
                                            </ItemTemplate>

                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn DataField="" AutoPostBackOnFilter="false" AllowFiltering="false"
                                            CurrentFilterFunction="Contains" UniqueName="Credit_Hold" HeaderText="" ShowFilterIcon="false" HeaderStyle-Width="50">
                                            <ItemTemplate>
                                                <asp:Image ID="imgCreditH" runat="server" Width="16px" ToolTip="Credit Hold" ImageUrl='<%# Eval("credithold").ToString() != "0" ? "images/MSCreditHold.png" : "data:image/png;base64,R0lGODlhAQABAIAAAAAAAP///yH5BAEAAAAALAAAAAABAAEAAAIBRAA7"%>' />
                                                <asp:Image ID="imgDispalert" runat="server" Width="16px" ToolTip="Dispatch Alert"
                                                    ImageUrl='<%# Eval("dispalert").ToString() != "0" ? "images/alert.png" : "data:image/png;base64,R0lGODlhAQABAIAAAAAAAP///yH5BAEAAAAALAAAAAABAAEAAAIBRAA7"%>' />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn DataField="customername" AutoPostBackOnFilter="true" AllowFiltering="true"
                                            CurrentFilterFunction="Contains" SortExpression="customername" UniqueName="customername" HeaderText="Customer Name" ShowFilterIcon="false" HeaderStyle-Width="150" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCustomername" runat="server" Text='<%# Eval("customername") %>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn DataField="locname" AutoPostBackOnFilter="true" AllowFiltering="true"
                                            CurrentFilterFunction="Contains" SortExpression="locname" UniqueName="locname" HeaderText="Location" ShowFilterIcon="false" HeaderStyle-Width="150">
                                            <ItemTemplate>
                                                <asp:Label ID="lblLoc" Style='<%# string.Format("color:{0}",Eval("ownerid")!=DBNull.Value? "Black": "Brown") %>'
                                                    ToolTip='<%# Eval("ownerid")!= DBNull.Value ? "": "Prospect" %>' runat="server"
                                                    Text='<%# Eval("locname") %>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn DataField="unit" AutoPostBackOnFilter="true"
                                            CurrentFilterFunction="Contains" SortExpression="unit" UniqueName="unit" HeaderText="Equipment" ShowFilterIcon="false" HeaderStyle-Width="150">
                                            <ItemTemplate>
                                                <div style="max-height: 90px; max-width: 100px; overflow: hidden">
                                                    <asp:Label ID="lblEquip" runat="server" Text='<%# Eval("unit") %>'></asp:Label>
                                                </div>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn DataField="Job" DataType="System.String" AutoPostBackOnFilter="true" AllowFiltering="true"
                                            CurrentFilterFunction="Contains" SortExpression="Job" UniqueName="Job" HeaderText="Project#" ShowFilterIcon="false" HeaderStyle-Width="100">
                                            <ItemTemplate>
                                                <asp:Label ID="lblProjectId" runat="server" Text='<%# Eval("Job").ToString()=="0" ? "": Eval("Job").ToString() %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn DataField="invoiceno" AutoPostBackOnFilter="true"
                                            CurrentFilterFunction="Contains" SortExpression="invoiceno" UniqueName="invoiceno" HeaderText="Invoice#" ShowFilterIcon="false" HeaderStyle-Width="100">
                                            <ItemTemplate>
                                                <a onclick="OpenNewWindow(<%# Eval("invoiceno").ToString()=="0" ? "": Eval("invoiceno").ToString() %>)"><%# Eval("invoiceno").ToString()=="0" ? "": Eval("invoiceno").ToString() %></a>
                                                <%--<asp:LinkButton ID="lblInvoiceId" OnClick="lnkInvoice_Click" runat="server" Text='<%# Eval("invoiceno").ToString()=="0" ? "": Eval("invoiceno").ToString() %>'></asp:LinkButton>--%>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn DataField="InvoiceDate" AutoPostBackOnFilter="true"
                                            CurrentFilterFunction="Contains" SortExpression="InvoiceDate" UniqueName="InvoiceDate" HeaderText="Invoice Date" ShowFilterIcon="false" HeaderStyle-Width="100">
                                            <ItemTemplate>
                                                <asp:Label ID="lblInvoiceDate" runat="server" Text='<%# Eval("InvoiceDate").ToString()=="0" ? "": Eval("InvoiceDate").ToString() %>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn DataField="manualinvoice" AutoPostBackOnFilter="true"
                                            CurrentFilterFunction="Contains" SortExpression="manualinvoice" UniqueName="manualinvoice" HeaderText="Manual Invoice #" ShowFilterIcon="false" HeaderStyle-Width="100">
                                            <ItemTemplate>
                                                <asp:Label ID="lblmanualinvoice" runat="server" Text='<%# Eval("manualinvoice").ToString()=="0" ? "": Eval("manualinvoice").ToString() %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <span>
                                                    <telerik:RadTextBox RenderMode="Auto" Width="100px" runat="server" ID="tbmanualInvoice">
                                                    </telerik:RadTextBox>
                                                </span>
                                            </EditItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn DataField="ProjectDescription" Visible="false" DataType="System.String" AutoPostBackOnFilter="true" AllowFiltering="true"
                                            CurrentFilterFunction="Contains" SortExpression="ProjectDescription" UniqueName="ProjectDescription" HeaderText="ProjectDesc" ShowFilterIcon="false" HeaderStyle-Width="100">
                                            <ItemTemplate>
                                                <asp:Label ID="lblProjectDesc" runat="server" Text='<%# Eval("ProjectDescription").ToString()=="" ? "": Eval("ProjectDescription").ToString() %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn DataField="City" AutoPostBackOnFilter="true" AllowFiltering="true"
                                            CurrentFilterFunction="Contains" SortExpression="City" UniqueName="City" HeaderText="City" ShowFilterIcon="false" HeaderStyle-Width="100">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCity" runat="server" Text='<%# Bind("City") %>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn DataField="State" AutoPostBackOnFilter="true" AllowFiltering="true"
                                            CurrentFilterFunction="Contains" SortExpression="State" UniqueName="State" HeaderText="State" ShowFilterIcon="false" HeaderStyle-Width="100" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblState" runat="server" Text='<%# Bind("State") %>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn DataField="Zip" AutoPostBackOnFilter="true" AllowFiltering="true"
                                            CurrentFilterFunction="Contains" SortExpression="Zip" UniqueName="Zip" HeaderText="Zip" ShowFilterIcon="false" HeaderStyle-Width="100" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblZip" runat="server" Text='<%# Bind("Zip") %>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn DataField="fullAddress" AutoPostBackOnFilter="true" AllowFiltering="true"
                                            CurrentFilterFunction="Contains" SortExpression="fullAddress" UniqueName="fullAddress" HeaderText="Address" ShowFilterIcon="false" HeaderStyle-Width="200">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAdd" runat="server" Text='<%# Bind("fullAddress") %>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn DataField="Phone" AutoPostBackOnFilter="true" AllowFiltering="true"
                                            CurrentFilterFunction="Contains" SortExpression="Phone" UniqueName="Phone" HeaderText="Phone" ShowFilterIcon="false" HeaderStyle-Width="150" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPhone" runat="server" Text='<%# Bind("Phone") %>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn DataField="Email" AutoPostBackOnFilter="true" AllowFiltering="true"
                                            CurrentFilterFunction="Contains" SortExpression="Email" UniqueName="Email" HeaderText="Email" ShowFilterIcon="false" HeaderStyle-Width="180" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEmail" runat="server" Text='<%# Bind("Email") %>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn Visible="true" DataField="dwork" AutoPostBackOnFilter="true" AllowFiltering="true"
                                            CurrentFilterFunction="Contains" SortExpression="dwork" UniqueName="dwork" HeaderText="Assignedto" ShowFilterIcon="false" HeaderStyle-Width="150">
                                            <ItemTemplate>
                                                <asp:Label ID="lblWorkerID" Visible="false" runat="server" Text='<%# Bind("workerid") %>'></asp:Label>
                                                <asp:Label ID="lblAssdTo" runat="server" Text='<%# Bind("dwork") %>'></asp:Label>
                                                <asp:Image ID="imgAfterHours" ToolTip="After Hours" runat="server" Width="16px" ImageUrl='<%# AfterHours(Eval("timeroute"),Eval("timecomp")) %>' />
                                                <asp:Image ID="imgWeekend" ToolTip="Weekend" runat="server" Width="16px" ImageUrl='<%# Weekend(Eval("edate")) %>' />

                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn Visible="true" DataField="Name" AutoPostBackOnFilter="true" AllowFiltering="true"
                                            CurrentFilterFunction="Contains" SortExpression="Name" UniqueName="Name" ShowFilterIcon="false" HeaderStyle-Width="150">
                                            <ItemTemplate>
                                                <asp:Label ID="lblName" Visible="true" runat="server" Text='<%# Bind("Name") %>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn DataField="cat" AutoPostBackOnFilter="true" ShowFilterIcon="false" CurrentFilterFunction="Contains"
                                            HeaderText="Category" SortExpression="cat" HeaderStyle-Width="140"
                                            UniqueName="cat">
                                            <ItemTemplate>
                                                <asp:Label ID="lblcat" runat="server" Text='<%# Bind("cat") %>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn DataField="assignname" AutoPostBackOnFilter="true" AllowFiltering="true"
                                            CurrentFilterFunction="Contains" UniqueName="assignname" HeaderText="Status" ShowFilterIcon="false" HeaderStyle-Width="100" SortExpression="assignname">
                                            <ItemTemplate>
                                                <asp:Label ID="lblStatus" runat="server" Text='<%# Bind("assignname") %>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn DataField="Company" AutoPostBackOnFilter="true" AllowFiltering="true"
                                            CurrentFilterFunction="Contains" UniqueName="Company" HeaderText="Company" ShowFilterIcon="false" HeaderStyle-Width="150" SortExpression="Company">

                                            <ItemTemplate>
                                                <asp:Label ID="lbCompany" runat="server" Text='<%# Bind("Company") %>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn DataType="System.Decimal" DataField="Tottime" UniqueName="Tottime" AutoPostBackOnFilter="true" AllowFiltering="true" SortExpression="Tottime"
                                            CurrentFilterFunction="EqualTo" HeaderText="TotalTime" ShowFilterIcon="false" HeaderStyle-Width="100">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTottime" runat="server" Text='<%# Eval("Tottime") %>'></asp:Label>
                                                <asp:Label ID="lblSumOfTotalTime" Visible="false" runat="server" Text='<%# Eval("SumOfTotalTime") %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblfTottime" runat="server"></asp:Label>
                                            </FooterTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn DataField="timediff" AutoPostBackOnFilter="true" AllowFiltering="true" SortExpression="timediff" DataType="System.Decimal"
                                            CurrentFilterFunction="EqualTo" UniqueName="timediff" HeaderText="TimeDiff" ShowFilterIcon="false" HeaderStyle-Width="100">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTotdiff" runat="server" Text='<%# Eval("timediff") %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblBalanceTotaldiff" runat="server"></asp:Label>
                                            </FooterTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn DataField="department" AutoPostBackOnFilter="true" ShowFilterIcon="false" AllowFiltering="true" CurrentFilterFunction="Contains"
                                            HeaderText="Department" SortExpression="Department" HeaderStyle-Width="140"
                                            UniqueName="department">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDepartment" runat="server" Text='<%# Bind("Department") %>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn DataField="fdesc" AutoPostBackOnFilter="false" UniqueName="ReasonforService" AllowFiltering="false" CurrentFilterFunction="Contains" ShowFilterIcon="false" HeaderStyle-Width="100" Visible="false" HeaderText="Reason for Service">
                                            <ItemTemplate>
                                                <asp:Label ID="lblfdesc" runat="server" Text='<%# Eval("fdesc") %>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn DataField="descres" AutoPostBackOnFilter="false" UniqueName="WorkCompleteDesc" AllowFiltering="false" CurrentFilterFunction="Contains" ShowFilterIcon="false" HeaderStyle-Width="100" Visible="false" HeaderText="Work Complete Desc">
                                            <ItemTemplate>
                                                <asp:Label ID="lbldescres" runat="server" Text='<%# Eval("descres") %>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn DataField="Who" AutoPostBackOnFilter="false" UniqueName="Who" AllowFiltering="false" CurrentFilterFunction="Contains" ShowFilterIcon="false" HeaderStyle-Width="100" Visible="false" HeaderText="Caller">
                                            <ItemTemplate>
                                                <asp:Label ID="lblWho" runat="server" Text='<%# Eval("Who") %>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn DataField="CPhone" AutoPostBackOnFilter="false" UniqueName="CPhone" AllowFiltering="false" CurrentFilterFunction="Contains" ShowFilterIcon="false" HeaderStyle-Width="100" Visible="false" HeaderText="Caller Phone">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCPhone" runat="server" Text='<%# Eval("CPhone") %>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn DataField="CDate" AutoPostBackOnFilter="false" UniqueName="CDate" AllowFiltering="false" CurrentFilterFunction="Contains" ShowFilterIcon="false" HeaderStyle-Width="100" Visible="false" HeaderText="Date Called In">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCDate" runat="server" Text='<%# Eval("CDate") %>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn DataField="Custom3" AutoPostBackOnFilter="false" UniqueName="Custom3" AllowFiltering="false" CurrentFilterFunction="Contains" ShowFilterIcon="false" HeaderStyle-Width="100" Visible="false" HeaderText="Custom3">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCustom3" runat="server" Text='<%# Eval("Custom3") %>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn DataField="Custom4" AutoPostBackOnFilter="false" UniqueName="Custom4" AllowFiltering="false" CurrentFilterFunction="Contains" ShowFilterIcon="false" HeaderStyle-Width="100" Visible="false" HeaderText="Custom4">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCustom4" runat="server" Text='<%# Eval("Custom4") %>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn DataField="PassedInspection" AutoPostBackOnFilter="true" AllowFiltering="true"
                                            CurrentFilterFunction="Contains" SortExpression="PassedInspection" UniqueName="PassedInspection" HeaderText="Last Inspection Date" ShowFilterIcon="false" HeaderStyle-Width="100" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPassedInspection" runat="server" Text='<%# Bind("PassedInspection") %>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                    </Columns>
                                </MasterTableView>
                            </telerik:RadGrid>
                            <%-- </telerik:RadAjaxPanel>--%>

                            <telerik:RadAjaxPanel ID="RadAjaxPanel_GroupTicket" runat="server" LoadingPanelID="RadAjaxLoadingPanel_Ticket" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">
                                <br />
                                <br />
                                <br />
                                <br />
                                <%--  ---------------2---------------------%>
                                <asp:GridView ID="gvGroupedTickets" runat="server" AutoGenerateColumns="False" Width="100%"
                                    CssClass="table-scrollable"
                                    EmptyDataText="No Tickets Found..." OnDataBound="OnDataBound">
                                    <RowStyle CssClass="evenrowcolor" />
                                    <AlternatingRowStyle CssClass="oddrowcolor" />
                                    <FooterStyle CssClass="footer" />
                                    <SelectedRowStyle CssClass="selectedrowcolor" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="Location" SortExpression="locname">
                                            <ItemTemplate>
                                                <asp:Label ID="lblLoc"
                                                    runat="server"
                                                    Text='<%# Eval("locname") %>'></asp:Label>
                                                <asp:Label ID="lbllocid" Visible="false" runat="server" Text='<%# Bind("locid") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="WO #" SortExpression="WorkOrder">
                                            <ItemTemplate>
                                                <asp:Label ID="lblWo" Visible="false" runat="server" Text='<%# Bind("WorkOrder") %>'></asp:Label>
                                                <asp:HyperLink ID="lnkWO" runat="server" NavigateUrl='<%# (Session["type"].ToString() == "c") ? "~/PrintWO.aspx?wo=" +  Eval("WorkOrder")+"&lid="+Eval("lid") : ""%>' Target="_blank" Text='<%# Eval("WorkOrder") %>'></asp:HyperLink>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Ticket #" SortExpression="id">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTicketId" runat="server" Text='<%# Bind("id") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Prateek #" SortExpression="Invoice">
                                            <ItemTemplate>
                                                <asp:Label ID="lblInvoiceId" runat="server" Text='<%# Bind("invoiceno") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Manual Inv. #" SortExpression="manualinvoice">
                                            <ItemTemplate>
                                                <asp:Label ID="lblManualInvoiceId" runat="server" Text='<%# Bind("manualinvoice") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Date" SortExpression="edate">
                                            <ItemTemplate>
                                                <asp:Label ID="Label8" runat="server" Text='<%# Eval("edate", "{0:MM/dd/yy hh:mm tt}") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Assigned to" SortExpression="dwork">
                                            <ItemTemplate>
                                                <asp:Label ID="lblWorkerID" Visible="false" runat="server" Text='<%# Bind("workerid") %>'></asp:Label>
                                                <asp:Label ID="lblAssdTo" runat="server" Text='<%# Bind("dwork") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="">
                                            <ItemTemplate>
                                                <asp:Image ID="imgIcon" runat="server" Style='max-width: 16px' ToolTip="Category"
                                                    ImageUrl='<%# "ImageHandler.ashx?catid=" + Eval("cat")%>'></asp:Image>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Category" SortExpression="cat">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCategory" runat="server" Text='<%# Bind("cat") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Company" SortExpression="Company">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCompanys" runat="server" Text='<%# Bind("Company") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Total Time" SortExpression="Tottime">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTot" runat="server" Text='<%# Eval("Tottime") %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblBalanceTotal" runat="server"></asp:Label>
                                            </FooterTemplate>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <br />
                                <br />
                                <br />
                                <%--  ---------------3------------------------------%>
                                <asp:Repeater ID="repGroupTicket" runat="server" OnItemDataBound="OnItemDataBound1">
                                    <HeaderTemplate>
                                        <table width="100%" class="table-scrollable ">
                                            <tr>
                                                <th width="100px">Ticket #</th>
                                                <th width="100px">Date</th>
                                                <th width="100px">Worker</th>
                                                <th width="100px">Category</th>
                                                <th width="100px">Status</th>
                                                <th width="100px">Total Time</th>
                                            </tr>
                                    </HeaderTemplate>

                                    <ItemTemplate>
                                        <tr>
                                            <td colspan="6">
                                                <table width="100%" class="table-scrollable">
                                                    <tr style="font-weight: bold; font-size: 14px; color: #316B9D">
                                                        <td><%# Eval("locname") %>
                                                            <asp:Label ID="lblLid" runat="server" Visible="false" Text='<%# Eval("lid") %>'></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <table width="100%" class="table-scrollable">
                                                                <asp:Repeater ID="repGroupTicketSub1" runat="server" OnItemDataBound="OnItemDataBound2">
                                                                    <ItemTemplate>
                                                                        <tr style="font-weight: bold">
                                                                            <td colspan="6">
                                                                                <asp:HyperLink ID="lnkWO" runat="server" NavigateUrl='<%# "~/PrintWO.aspx?wo=" +  Eval("WorkOrder")+"&lid="+Eval("lid") %>' Target="_blank" Text='<%# "Work Order :" +  Eval("WorkOrder") %>'></asp:HyperLink>
                                                                                <asp:Label ID="lblWO" runat="server" Visible="false" Text='<%# Eval("Workorder") %>'></asp:Label>
                                                                            </td>
                                                                        </tr>
                                                                        <tr style="background-color: white;">
                                                                            <td colspan="6">
                                                                                <table class="table-scrollable">
                                                                                    <asp:Repeater ID="repGroupTicketSub2" runat="server">
                                                                                        <ItemTemplate>
                                                                                            <tr class="evenrowcolor">
                                                                                                <td width="100px">
                                                                                                    <asp:HyperLink ID="lnkTicket" runat="server" NavigateUrl='<%# "~/Printticket.aspx?id=" +  Eval("id")+"&c="+Eval("comp") %>' Target="_blank" Text='<%# "Ticket # " +  Eval("ID") %>'></asp:HyperLink></td>
                                                                                                <td width="100px"><%# Eval("edate", "{0:MM/dd/yy hh:mm tt}") %></td>
                                                                                                <td width="100px"><%# Eval("dwork") %></td>
                                                                                                <td width="100px"><%# Eval("cat") %></td>
                                                                                                <td width="100px"><%# Eval("AssignName") %></td>
                                                                                                <td width="100px"><%# Eval("tottime") %></td>
                                                                                            </tr>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <tr>
                                                                                                <td colspan="5" style="text-align: right">Total : </td>
                                                                                                <td width="100px">
                                                                                                    <asp:Label ID="lbltotalsub" runat="server"></asp:Label></td>
                                                                                            </tr>
                                                                                        </FooterTemplate>
                                                                                    </asp:Repeater>
                                                                                </table>
                                                                            </td>
                                                                        </tr>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <tr style="background-color: silver; text-align: left; font-weight: bold">
                                                                            <td width="100px"></td>
                                                                            <td width="100px"></td>
                                                                            <td width="100px"></td>
                                                                            <td width="100px"></td>
                                                                            <td width="100px" style="text-align: right;">Sub Total :</td>
                                                                            <td width="100px">
                                                                                <asp:Label ID="lbltotal" runat="server"></asp:Label>
                                                                            </td>
                                                                        </tr>
                                                                    </FooterTemplate>
                                                                </asp:Repeater>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </ItemTemplate>

                                    <FooterTemplate>
                                        <tr style="font-weight: bold; text-align: left">
                                            <td colspan="5" style="text-align: right">Grand Total :</td>
                                            <td>
                                                <asp:Label ID="lblGrtotal" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        </table>
                                    </FooterTemplate>
                                </asp:Repeater>
                            </telerik:RadAjaxPanel>
                        </div>
                    </div>
                </div>
            </div>


            <%--  ----------------Main Rad Grid---------------%>
        </div>
    </div>

    <%---------OLD Design-------------%>

    <%--Email Sending Logs--%>
    <div class="container accordian-wrap">
        <div class="col s12 m12 l12">
            <div class="row">
                <ul class="collapsible popout collapsible-accordion form-accordion-head" data-collapsible="expandable">
                    <li id="tbLogs" runat="server" style="display: none">
                        <div id="accrdlogs" class="collapsible-header accrd accordian-text-custom"><i class="mdi-content-content-paste"></i>Email History Log</div>
                        <div class="collapsible-body">
                            <div class="form-content-wrap">
                                <div class="form-content-pd">
                                    <div class="grid_container">
                                        <div class="form-section-row mb">
                                            <div class="RadGrid RadGrid_Material">
                                                <telerik:RadCodeBlock ID="RadCodeBlock6" runat="server">
                                                    <script type="text/javascript">
                                                        function pageLoadLog() {
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
                                                        <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="True" ShowFooter="True" AllowNaturalSort="false">
                                                            <%--<GroupByExpressions>
                                                                <telerik:GridGroupByExpression>
                                                                    <SelectFields>
                                                                        <telerik:GridGroupByField FieldAlias="SessionNo" FieldName="SessionNo" HeaderText="Session No"></telerik:GridGroupByField>
                                                                    </SelectFields>
                                                                    <GroupByFields>
                                                                        <telerik:GridGroupByField FieldName="SessionNo"></telerik:GridGroupByField>
                                                                    </GroupByFields>
                                                                </telerik:GridGroupByExpression>
                                                            </GroupByExpressions>--%>
                                                            <Columns>
                                                                <%--<telerik:GridTemplateColumn DataField="SessionNo" SortExpression="SessionNo" AutoPostBackOnFilter="true"
                                                                    CurrentFilterFunction="Contains" HeaderText="Session No" ShowFilterIcon="false" HeaderStyle-Width="305">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblSessionNo" runat="server" Text='<%# Eval("SessionNo") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>--%>
                                                                <telerik:GridTemplateColumn DataField="EmailDate" SortExpression="EmailDate" AutoPostBackOnFilter="true" DataType="System.String"
                                                                    CurrentFilterFunction="Contains" HeaderText="Date" ShowFilterIcon="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lbldate" runat="server" Text='<%# Eval("EmailDate", "{0:M/d/yyyy}")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>
                                                                <telerik:GridTemplateColumn DataField="EmailDate" SortExpression="EmailDate" AutoPostBackOnFilter="true" DataType="System.String"
                                                                    CurrentFilterFunction="Contains" HeaderText="Time" ShowFilterIcon="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lbltime" runat="server" Text='<%# Eval("EmailDate","{0: hh:mm tt}") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>
                                                                <telerik:GridTemplateColumn DataField="Username" SortExpression="Username" AutoPostBackOnFilter="true"
                                                                    CurrentFilterFunction="Contains" HeaderText="User" ShowFilterIcon="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblUsername" runat="server" Text='<%# Eval("Username") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>
                                                                <telerik:GridTemplateColumn DataField="Ref" SortExpression="Ref" AutoPostBackOnFilter="true" DataType="System.String"
                                                                    CurrentFilterFunction="Contains" HeaderText="Ticket #" ShowFilterIcon="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblRef" runat="server" Text='<%# Eval("Ref") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>
                                                                <telerik:GridTemplateColumn DataField="EmailFunction" SortExpression="EmailFunction" AutoPostBackOnFilter="true"
                                                                    CurrentFilterFunction="Contains" HeaderText="Function" ShowFilterIcon="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblEmailFunction" runat="server" Text='<%# Eval("EmailFunction") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>
                                                                <telerik:GridTemplateColumn DataField="MailTo" SortExpression="MailTo" AutoPostBackOnFilter="true"
                                                                    CurrentFilterFunction="Contains" HeaderText="Mail To" ShowFilterIcon="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblEmailTo" runat="server" Text='<%# Eval("MailTo") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>
                                                                <telerik:GridTemplateColumn DataField="Status" SortExpression="Status" AutoPostBackOnFilter="true"
                                                                    CurrentFilterFunction="Contains" HeaderText="Status" ShowFilterIcon="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("Status") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>
                                                                <telerik:GridTemplateColumn DataField="UsrErrMessage" SortExpression="UsrErrMessage" AutoPostBackOnFilter="true"
                                                                    CurrentFilterFunction="Contains" HeaderText="Error Message" ShowFilterIcon="false" HeaderStyle-Width="305">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblUsrErrMessage" runat="server" Text='<%# Eval("UsrErrMessage") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>

                                                            </Columns>
                                                        </MasterTableView>
                                                    </telerik:RadGrid>
                                                </telerik:RadAjaxPanel>
                                            </div>

                                        </div>
                                    </div>

                                    <div class="cf"></div>
                                </div>
                            </div>
                            <%--<div style="clear: both;"></div>--%>
                        </div>
                    </li>
                </ul>
            </div>
        </div>
    </div>


    <asp:Button runat="server" ID="hiddenTargetControlForModalPopup" Style="display: none"
        CausesValidation="False" />

    <asp:ModalPopupExtender runat="server" ID="ModalPopupExtender1" BehaviorID="PMPBehaviour"
        TargetControlID="hiddenTargetControlForModalPopup" PopupControlID="Panel1" BackgroundCssClass="pnlUpdateoverlay"
        RepositionMode="RepositionOnWindowResizeAndScroll" X="180" Y="400">
    </asp:ModalPopupExtender>

    <asp:Panel runat="server" ID="Panel1" Style="display: none; background: #fff; border: solid;"
        CssClass="roundCorner shadow ModalWindowAbs">
        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
            <ContentTemplate>
                <asp:Panel runat="Server" ID="Panel2" Style="background-color: #DDDDDD; border: solid 1px Gray; color: Black; text-align: center;">
                    <div class="title_bar_popup">
                        <a id="A1" href="#" style="float: right; margin-right: 20px; color: #fff; margin-left: 10px; height: 16px;">Close</a>
                    </div>
                </asp:Panel>
                <div>
                    <iframe id="iframeCustomer" runat="server" frameborder="0" width="1040px" height="730px"></iframe>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>

    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
        <ContentTemplate>
            <asp:Button runat="server" ID="hideModalPopupViaServer" Style="float: right; margin-right: 20px; color: #fff; margin-left: 10px; height: 16px; display: none;"
                Text="Close" OnClick="hideModalPopupViaServer_Click"
                CausesValidation="false" />
        </ContentTemplate>
    </asp:UpdatePanel>


    <!-- Hidden Field -->
    <asp:HiddenField runat="server" ID="hdnAddeTicket" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnEditeTicket" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnDeleteTicket" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnViewTicket" Value="Y" />
    <asp:HiddenField runat="server" ID="HiddenField1" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnIsQBInt" Value="0" />
    <asp:HiddenField runat="server" ID="hdnTicketListSelectDtRange" Value="" />
    <asp:HiddenField runat="server" ID="hdnCssActive" Value="1" />
    <asp:HiddenField runat="server" ID="hdnRoute" Value="" />
    <asp:HiddenField runat="server" ID="hdnDeleteResolvedTicket" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnTicketVoidPermission" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnTicketReportFormat" Value="rdlc" />

</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="javascriptHolder" runat="Server">

    <script src="Design/js/moment.js"></script>

    <script src="Design/js/pikaday.js"></script>

    <script type="text/javascript">

        $(document).ready(function () {

            $('#ctl00_ContentPlaceHolder1_ddllocation').change(function () {

                var nkref = document.getElementById('ctl00_ContentPlaceHolder1_btnSearch');
                nkref.click();
            });





            //alert(($('.dropdown-content').width() * 2.4) / 2.5 + 5);
            $('.dropdown-button2').dropdown({
                inDuration: 300,
                outDuration: 225,
                constrain_width: false, // Does not change width of dropdown to that of the activator
                hover: true, // Activate on hover
                gutter: ($('.dropdown-content').width() * 2.4) / 2.5 + 5, // Spacing from edge
                belowOrigin: false, // Displays dropdown below the button
                alignment: 'left' // Displays dropdown with edge aligned to the left of button
            });



            $('a[href^="#accrd"]').on('click', function (e) {
                e.preventDefault();

                var target = this.hash;
                var $target = $(target);
                if ($(target).hasClass('active') || target == "") {

                }
                else {
                    $(target).click();
                }

                $('html, body').stop().animate({
                    'scrollTop': $target.offset().top - 125
                }, 900, 'swing');
            });

            $('#addinfo').hide();
            $('.add-btn-click').click(function () {

                $('#addinfo').slideToggle('2000', "swing", function () {
                    // Animation complete.

                });

                if ($('.divbutton-container').height() != 65)
                    $('.divbutton-container').animate({ height: 65 }, 500);
                else
                    $('.divbutton-container').animate({ height: 430 }, 500);


            });

            $('#<%=ddlRoute.ClientID %>').change(function () {
                $('#<%=hdnRoute.ClientID %>').val($(this).val());

            });

        });
    </script>

    <script>
        var picker = new Pikaday(
            {
                field: document.getElementById('ctl00_ContentPlaceHolder1_txtInvDt'),
                firstDay: 0,
                format: 'MM/DD/YYYY',
                minDate: new Date(2000, 1, 1),
                maxDate: new Date(2020, 12, 31),
                yearRange: [2000, 2020]
            });
    </script>

    <script type="text/javascript" src="js/jquery.inputmask.bundle.min.js">

    </script>

    <script type="text/javascript">

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

                var val;
                //val = localStorage.getItem("hdnTicketListDate");
                val = document.getElementById('<%= hdnTicketListSelectDtRange.ClientID%>').value;
                <%--if (val == '') {
                    $("#<%=lblDay.ClientID%>").addClass("");
                    $("#<%=lblWeek.ClientID%>").addClass("");
                    $("#<%=lblMonth.ClientID%>").addClass("");
                    $("#<%=lblQuarter.ClientID%>").addClass("");
                    $("#<%=lblYear.ClientID%>").addClass("");
                }
                else {--%>
                CssClearLabel();
                if (val == 'Day') {
                    $("#<%=lblDay.ClientID%>").addClass("labelactive");
                    document.getElementById("<%= rdDay.ClientID%>").checked = true;
                }
                else if (val == 'Week') {

                    $("#<%=lblWeek.ClientID%>").addClass("labelactive");
                    document.getElementById("<%= rdWeek.ClientID%>").checked = true;
                }
                else if (val == 'Month') {

                    $("#<%=lblMonth.ClientID%>").addClass("labelactive");
                    document.getElementById("<%= rdMonth.ClientID%>").checked = true;
                }
                else if (val == 'Quarter') {

                    $("#<%=lblQuarter.ClientID%>").addClass("labelactive");
                    document.getElementById("<%= rdQuarter.ClientID%>").checked = true;
                }
                else if (val == 'Year') {

                    $("#<%=lblYear.ClientID%>").addClass("labelactive");
                    document.getElementById("<%= rdYear.ClientID%>").checked = true;
                }
                else {
                    $("#<%=lblWeek.ClientID%>").addClass("labelactive");
                    document.getElementById("<%= rdWeek.ClientID%>").checked = true;
                }
                //}
            }
        });
    </script>

    <script type="text/javascript">
        function dec_date(select) {
            debugger
            var select = select;
            var txtDateTo = "<%= txtToDate.ClientID%>";
            var txtDateFrom = "<%= txtfromDate.ClientID%>";
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
            debugger

            var rdDayName = document.getElementById("<%= rdDay.ClientID%>").name;
            var radio = document.getElementsByName(rdDayName);
            var selected;
            for (var i = 0; i < radio.length; i++) {
                if (radio[i].checked) { // Checked property to check radio Button check or not
                    //alert("Radio button having value " + radio[i].value + " was checked."); // Show the checked value
                    selected = radio[i].value;

                }
                if (selected == "") {
                    selected = 'rdDay';
                }
            }
            if (selected == 'rdDay') {

                //dec the from date 
                var tt = document.getElementById(txtDateFrom).value;
                debugger
                var date = new Date(tt);
                var newdate = new Date(date);

                if (date.getDate() && newdate.getDate()) {
                    newdate.setDate(newdate.getDate() + xday);

                    var dd = newdate.getDate();
                    var mm = newdate.getMonth() + 1;
                    var y = newdate.getFullYear();

                    var someFormattedDate = mm + '/' + dd + '/' + y;
                    document.getElementById(txtDateFrom).value = someFormattedDate;
                }



                //dec the to date 

                var TT = document.getElementById(txtDateTo).value;

                var DATE = new Date(TT);
                var NEWDATE = new Date(DATE);
                if (DATE.getDate() && NEWDATE.getDate()) {
                    NEWDATE.setDate(NEWDATE.getDate() + xday);

                    var DD = NEWDATE.getDate();
                    var MM = NEWDATE.getMonth() + 1;
                    var Y = NEWDATE.getFullYear();

                    var someFormattedDATE = MM + '/' + DD + '/' + Y;
                    document.getElementById(txtDateTo).value = someFormattedDATE;
                }
            }
            else if (selected == 'rdWeek') {
                //dec the from date 
                var tt = document.getElementById(txtDateFrom).value;

                var date = new Date(tt);
                var newdate = new Date(date);
                if (date.getDate() && newdate.getDate()) {
                    newdate.setDate(newdate.getDate() + xWeek);

                    var dd = newdate.getDate();
                    var mm = newdate.getMonth() + 1;
                    var y = newdate.getFullYear();

                    var someFormattedDate = mm + '/' + dd + '/' + y;
                    document.getElementById(txtDateFrom).value = someFormattedDate;
                    //dec the to date 
                }
                var TT = document.getElementById(txtDateTo).value;

                var DATE = new Date(TT);
                var NEWDATE = new Date(DATE);
                if (DATE.getDate() && NEWDATE.getDate()) {
                    NEWDATE.setDate(NEWDATE.getDate() + xWeek);

                    var DD = NEWDATE.getDate();
                    var MM = NEWDATE.getMonth() + 1;
                    var Y = NEWDATE.getFullYear();

                    var someFormattedDATE = MM + '/' + DD + '/' + Y;
                    document.getElementById(txtDateTo).value = someFormattedDATE;
                }
            }
            else if (selected == 'rdMonth') {
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
                //var date = new Date(tt).toDateString();
                var date = new Date(tt);
                var newdate = new Date(date);
                if (date.getDate() && newdate.getDate()) {
                    newdate.addMonths(xMonth);

                    var dd = newdate.getDate();
                    var mm = newdate.getMonth() + 1;
                    var y = newdate.getFullYear();

                    var someFormattedDate = mm + '/' + dd + '/' + y;
                    document.getElementById(txtDateFrom).value = someFormattedDate;
                    //dec the to date 
                }
                if (select == 'dec') {
                    var ti = document.getElementById(txtDateTo).value;
                    //var date = new Date(ti).toDateString();
                    var date = new Date(ti);
                    var newdateti = new Date(date);
                    if (date.getDate() && newdateti.getDate()) {
                        newdateti.addMonthsLastDec(xMonth);

                        var dd = newdateti.getDate();
                        var mm = newdateti.getMonth() + 1;
                        var y = newdateti.getFullYear();

                        var someFormattedDate = mm + '/' + dd + '/' + y;
                        document.getElementById(txtDateTo).value = someFormattedDate;
                    }
                }
                else {
                    var ti = document.getElementById(txtDateTo).value;

                    //var date = new Date(ti).toDateString();
                    var date = new Date(ti);
                    var newdateti = new Date(date);
                    if (date.getDate() && newdateti.getDate()) {
                        newdateti.addMonthsLast(xMonth);

                        var dd = newdateti.getDate();
                        var mm = newdateti.getMonth() + 1;
                        var y = newdateti.getFullYear();

                        var someFormattedDate = mm + '/' + dd + '/' + y;
                        document.getElementById(txtDateTo).value = someFormattedDate;
                    }
                }
            }


            else if (selected == 'rdQuarter') {
                //dec the from date 
                var tt = document.getElementById(txtDateFrom).value;

                var date = new Date(tt);
                var newdate = new Date(date);
                if (date.getDate() && newdate.getDate()) {
                    newdate.setMonth(newdate.getMonth() + xQuarter);

                    var dd = newdate.getDate();
                    var mm = newdate.getMonth() + 1;
                    var y = newdate.getFullYear();

                    var someFormattedDate = mm + '/' + dd + '/' + y;
                    document.getElementById(txtDateFrom).value = someFormattedDate;
                    //dec the to date 
                }
                var TT = document.getElementById(txtDateTo).value;

                var DATE = new Date(TT);
                var NEWDATE = new Date(DATE);
                if (DATE.getDate() && NEWDATE.getDate()) {
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
            }
            else if (selected == 'rdYear') {

                var tt = document.getElementById(txtDateFrom).value;

                var date = new Date(tt);
                var newdate = new Date(date);
                if (date.getDate() && newdate.getDate()) {
                    newdate.setFullYear(newdate.getFullYear() + xYear);

                    var dd = newdate.getDate();
                    var mm = newdate.getMonth() + 1;
                    var y = newdate.getFullYear();

                    var someFormattedDate = mm + '/' + dd + '/' + y;
                    document.getElementById(txtDateFrom).value = someFormattedDate;
                }
                //dec the to date 

                var TT = document.getElementById(txtDateTo).value;

                var DATE = new Date(TT);
                var NEWDATE = new Date(DATE);
                if (DATE.getDate() && NEWDATE.getDate()) {
                    NEWDATE.setFullYear(NEWDATE.getFullYear() + xYear);
                    var DD = NEWDATE.getDate();
                    var MM = NEWDATE.getMonth() + 1;
                    var Y = NEWDATE.getFullYear();

                    var someFormattedDATE = MM + '/' + DD + '/' + Y;
                    document.getElementById(txtDateTo).value = someFormattedDATE;
                }
            }

            return false;

        }
        function SelectDate(type) {//, txtDateFrom, txtdateTo, label, UniqueVal, rdGroup
            var type = type;
            var txtDateFrom = "<%= txtfromDate.ClientID%>";
            var txtdateTo = "<%= txtToDate.ClientID%>";;
            //var UniqueVal = UniqueVal;
            //var label = label;
            CssClearLabel();
            if (type == 'Day') {
                var todaydate = new Date();
                var day = todaydate.getDate();
                var month = todaydate.getMonth() + 1;
                var year = todaydate.getFullYear();
                var datestring = month + "/" + day + "/" + year;
                document.getElementById(txtdateTo).value = datestring;
                document.getElementById(txtDateFrom).value = datestring;
                $("#<%= lblDay.ClientID%>").addClass("labelactive");
                document.getElementById('<%= hdnTicketListSelectDtRange.ClientID%>').value = "Day";
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
                $("#<%= lblWeek.ClientID%>").addClass("labelactive");
                document.getElementById('<%= hdnTicketListSelectDtRange.ClientID%>').value = "Week";
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
                $("#<%= lblMonth.ClientID%>").addClass("labelactive");
                document.getElementById('<%= hdnTicketListSelectDtRange.ClientID%>').value = "Month";
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
                $("#<%= lblQuarter.ClientID%>").addClass("labelactive");
                document.getElementById('<%= hdnTicketListSelectDtRange.ClientID%>').value = "Quarter";
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
                $("#<%= lblYear.ClientID%>").addClass("labelactive");
                document.getElementById('<%= hdnTicketListSelectDtRange.ClientID%>').value = "Year";
            }
            <%-- if (typeof (Storage) !== "undefined") {
                // Store
                localStorage.setItem(UniqueVal, document.getElementById('<%= hdnTicketListSelectDtRange.ClientID%>').value);
            }--%>
            document.getElementById('<%= hdnCssActive.ClientID%>').value = "CssActive";
            var clickSearchButton = document.getElementById("<%= btnSearch.ClientID %>");
            clickSearchButton.click();
            document.getElementById('<%= hdnCssActive.ClientID%>').value = "1";
        }

        function OnGridCreated(sender, args) {
            //alert('OnGridCreated');
            var frozenScroll = $get(sender.get_id() + "_Frozen");
            var allColumns = sender.get_masterTableView().get_columns();
            var scrollLeftOffset = 0;
            var allColumnsWidth = new Array;
            var grid = sender.get_element();
            for (var i = 0; i < allColumns.length; i++) {
                allColumnsWidth[i] = allColumns[i].get_element().offsetWidth;
            }

            $get(sender.get_id() + "_GridData").onscroll = function (e) {
                for (var i = 0; i < allColumns.length; i++) {
                    if (!allColumns[i].get_visible()) {
                        scrollLeftOffset += allColumnsWidth[i];
                    }
                    if ($telerik.isIE7) {
                        var thisColumn = grid.getElementsByTagName("colgroup")[0].getElementsByTagName("col")[i];
                        if (thisColumn.style.display == "none") {
                            scrollLeftOffset += parseInt(thisColumn.style.width);
                        }
                    }
                }
                var thisScrollLeft = this.scrollLeft;
                if (frozenScroll != null) {
                    if (thisScrollLeft > 0)
                        frozenScroll.scrollLeft = thisScrollLeft + scrollLeftOffset + 300;
                    this.scrollLeft = 0;
                }

                scrollLeftOffset = 0;
            };
        }

        function headerMenuShowing(sender, args) {
            var menu = args.get_menu();
            debugger
            for (var i = 0; i < menu.get_items().get_count(); i++) {
                var item = menu.get_items().getItem(i);
                if (item.get_value() != 'ColumnsContainer') {
                    item.get_element().style.display = 'none';
                }
            }

            var columnsItem = menu.findItemByText("Columns");
            columnsItem.get_items().getItem(0).get_element().style.display = "none";
            columnsItem.get_items().getItem(1).get_element().style.display = "none";
            //columnsItem.get_items().getItem(2).get_element().style.display = "none";
        }

        function ColumnSettingsChange(menu, args) {
            debugger
            document.getElementById("<%=lbSaveGridSettings.ClientID %>").style.display = "block";
            document.getElementById("<%=lbRestoreGridSettings.ClientID %>").style.display = "none";
        }

        function GridCommand(sender, args) {
            if (args.get_commandName() == "Sort") {
                ColumnSettingsChange();
            }
        }

        function SaveGridSettings() {
            document.getElementById('<%=lnkSaveGridSettings.ClientID%>').click();
            document.getElementById("<%=lbSaveGridSettings.ClientID %>").style.display = "none";
            document.getElementById("<%=lbRestoreGridSettings.ClientID %>").style.display = "block";
        }

        function RestoreGridSettings() {
            document.getElementById('<%=lnkRestoreGridSettings.ClientID%>').click();
            document.getElementById("<%=lbRestoreGridSettings.ClientID %>").style.display = "none";
            document.getElementById("<%=lbSaveGridSettings.ClientID %>").style.display = "none";
        }

    </script>


</asp:Content>
