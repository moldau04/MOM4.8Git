<%@ Page Title="" Language="C#" MasterPageFile="~/Mom.master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="ContractListingByRouteActiveOnly.aspx.cs" Inherits="ContractListingByRouteActiveOnly" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="Stimulsoft.Report.WebDesign" Namespace="Stimulsoft.Report.Web" TagPrefix="cc1" %>

<%@ Register Assembly="Stimulsoft.Report.Web" Namespace="Stimulsoft.Report.Web" TagPrefix="cc1" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        body:nth-of-type(1) img[src*="Blank.gif"] {
            display: none;
        }

        table input[type="text"], input[type="password"], input[type="email"], input[type="url"], input[type="time"], input[type="date"], input[type="datetime-local"], input[type="tel"], input[type="number"], input[type="search"] {
            background-color: white !important;
            margin: 0px !important;
            height: 17px !important;
        }

        td, th {
            padding: 0px 5px !important;
            display: table-cell;
            text-align: left;
            vertical-align: middle;
        }

        .materialize-textarea {
            max-height: 100px !important;
            height: 100px !important;
        }

        body {
            background-color: white !important;
        }

        .RadComboBox_Metro .rcbInner {
            padding-top: 0px !important;
        }

        .stiJsViewerPageShadow {
            height: auto !important;
        }
        .RadComboBoxDropDown.RadComboBoxDropDown_Metro {
            width: 220px !important;
        }
    </style>
    <script type="text/javascript">
        function showMailReport() {
            jQuery("#txtTo").text = "";
            jQuery("#txtCC").text = "";
            var radwindow = $find('<%=mailWindow.ClientID %>');
            radwindow.show();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="mainWindow">
        <div class="divbutton-container">
            <div id="divButtons">
                <div id="breadcrumbs-wrapper">
                    <header>
                        <div class="col s12 m12 l12">
                            <div class="row">
                                <div class="page-title"><i class="mdi-action-swap-vert-circle"></i>&nbsp; Contract Listing by Route Active Only Report</div>
                                <div class="buttonContainer">
                                    <div class="btnlinks">
                                        <a id="mailReport" href="javascript:void(0);" onclick="showMailReport();return false;">Email</a>
                                    </div>

                                    <ul class="nomgn hideMenu menuList">
                                        <li>
                                            <asp:Label CssClass="title_text" ID="lblHeader" runat="server"></asp:Label></li>
                                        <li>
                                            <asp:LinkButton CssClass="icon-closed" runat="server" CausesValidation="false" ToolTip="close"
                                                OnClick="lnkClose_Click"></asp:LinkButton></li>
                                    </ul>
                                </div>
                                <div class="btnclosewrap">
                                    <asp:LinkButton ID="lnkClose" runat="server" OnClick="lnkClose_Click"><i class="mdi-content-clear"></i></asp:LinkButton>
                                </div>
                                <div class="rght-content">
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
                        <div class="srchtitle date-csss">
                            Start Date
                        </div>
                        <div class="srchinputwrap">
                            <asp:TextBox ID="txtFromDate" runat="server" MaxLength="50" CssClass="datepicker_mom"></asp:TextBox>
                        </div>
                        <div class="srchtitle date-csss">
                            End Date
                        </div>
                        <div class="srchinputwrap">
                            <asp:TextBox ID="txtToDate" runat="server" MaxLength="50" CssClass="datepicker_mom"></asp:TextBox>
                        </div>
                        <div class="srchinputwrap tabcontainer">
                            <ul class="tabselect accrd-tabselect" id="testradiobutton">
                                <li>
                                    <asp:LinkButton AutoPostBack="False" ID="decDate" runat="server" Text="<i class='mdi-hardware-keyboard-arrow-left'></i>" OnClientClick="dec_date('dec','ctl00_ContentPlaceHolder1_txtToDate','ctl00_ContentPlaceHolder1_txtFromDate','rdCal');return false;"></asp:LinkButton>
                                </li>
                                <li>
                                    <label id="lblDay" runat="server">
                                        <input type="radio" id="rdDay" name="rdCal" value="rdDay" onclick="SelectDate('Day', 'ctl00_ContentPlaceHolder1_txtFromDate', 'ctl00_ContentPlaceHolder1_txtToDate', '#lblDay', 'hdnInvoiceDate', 'rdCal')" />
                                        Day
                                    </label>
                                </li>
                                <li>
                                    <label id="lblWeek" runat="server">
                                        <input type="radio" id="rdWeek" name="rdCal" value="rdWeek" onclick="SelectDate('Week', 'ctl00_ContentPlaceHolder1_txtFromDate', 'ctl00_ContentPlaceHolder1_txtToDate', '#ctl00_ContentPlaceHolder1_lblWeek', 'hdnInvoiceDate', 'rdCal')" />
                                        Week
                                    </label>
                                </li>
                                <li>
                                    <label id="lblMonth" runat="server">
                                        <input type="radio" id="rdMonth" name="rdCal" value="rdMonth" onclick="SelectDate('Month', 'ctl00_ContentPlaceHolder1_txtFromDate', 'ctl00_ContentPlaceHolder1_txtToDate', '#ctl00_ContentPlaceHolder1_lblMonth', 'hdnInvoiceDate', 'rdCal')" />
                                        Month
                                    </label>
                                </li>
                                <li>
                                    <label id="lblQuarter" runat="server">
                                        <input type="radio" id="rdQuarter" name="rdCal" value="rdQuarter" onclick="SelectDate('Quarter', 'ctl00_ContentPlaceHolder1_txtFromDate', 'ctl00_ContentPlaceHolder1_txtToDate', '#ctl00_ContentPlaceHolder1_lblQuarter', 'hdnInvoiceDate', 'rdCal')" />
                                        Quarter
                                    </label>
                                </li>
                                <li>
                                    <label id="lblYear" runat="server">
                                        <input type="radio" id="rdYear" name="rdCal" value="rdYear" onclick="SelectDate('Year', 'ctl00_ContentPlaceHolder1_txtFromDate', 'ctl00_ContentPlaceHolder1_txtToDate', '#ctl00_ContentPlaceHolder1_lblYear', 'hdnInvoiceDate', 'rdCal')" />
                                        Year
                                    </label>
                                </li>
                                <li>
                                    <asp:LinkButton ID="incDate" runat="server" OnClientClick="dec_date('inc','ctl00_ContentPlaceHolder1_txtToDate','ctl00_ContentPlaceHolder1_txtFromDate','rdCal');return false" Text="<i class='mdi-hardware-keyboard-arrow-right'></i>"></asp:LinkButton>
                                </li>
                            </ul>
                        </div>

                        <div class="srchtitle ser-css2">
                            Select Routes 
                        </div>
                        <div class="srchinputwrap">
                            <telerik:RadComboBox RenderMode="Auto" Skin="Metro" ID="rcRoute" runat="server" Filter="StartsWith" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" Width="200"
                                EmptyMessage="--Select Route--" CssClass="inGrid">
                            </telerik:RadComboBox>
                        </div>

                        <div class="btnlinksicon">
                            <asp:LinkButton ID="lnkSearch" runat="server" OnClick="lnkSearch_Click" CausesValidation="false" ToolTip="Load Report"><i class="fa fa-refresh"></i></asp:LinkButton>
                        </div>
                    </div>
                </div>
            </div>

            <div class="grid_container">
                <div class="form-section-row m-b-0">
                    <cc1:StiWebViewer ID="StiWebViewerContractListingByRoute" Height="800px" RequestTimeout="20000" runat="server" CacheMode="None" ViewMode="Continuous" ScrollbarsMode="true"
                        OnGetReport="StiWebViewerContractListingByRoute_GetReport" OnGetReportData="StiWebViewerContractListingByRoute_GetReportData" Visible="false" />
                </div>
            </div>

        </div>
    </div>

    <telerik:RadWindowManager ID="RadWindowManagerCustomer" runat="server">
        <Windows>
            <telerik:RadWindow ID="mailWindow" Skin="Material" VisibleTitlebar="true" Title="Mail" Behaviors="Default" CenterIfModal="true"
                Animation="FlyIn" AnimationDuration="200" RenderMode="Auto" VisibleStatusbar="false" MinWidth="450"
                runat="server" Modal="true" Width="600" Height="450">
                <ContentTemplate>
                    <div class="m-t-15">
                        <div class="form-section-row">
                            <div class="form-section2">
                                <div class="input-field col s12">
                                    <div class="row">
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
                                        <asp:ValidatorCalloutExtender ID="RequiredFieldValidator2_ValidatorCalloutExtender"
                                            runat="server" Enabled="True" TargetControlID="RequiredFieldValidator2">
                                        </asp:ValidatorCalloutExtender>
                                        <asp:TextBox ID="txtFrom" runat="server"></asp:TextBox>
                                        <asp:Label runat="server" AssociatedControlID="txtFrom">From</asp:Label>
                                    </div>
                                </div>
                            </div>
                            <div class="form-section2-blank">
                                &nbsp;
                            </div>
                            <div class="form-section2">
                                <div class="input-field col s12">
                                    <div class="row">
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
                                        <asp:TextBox ID="txtTo" runat="server"></asp:TextBox>
                                        <asp:Label runat="server" AssociatedControlID="txtTo"> To</asp:Label>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="form-section-row">
                            <div class="form-section2">
                                <div class="input-field col s12">
                                    <div class="row">

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
                                        <asp:TextBox ID="txtCC" runat="server"></asp:TextBox>
                                        <asp:Label runat="server" AssociatedControlID="txtCC">CC</asp:Label>
                                    </div>
                                </div>

                            </div>
                            <div class="form-section2-blank">
                                &nbsp;
                            </div>
                            <div class="form-section2">
                                <div class="input-field col s12">
                                    <div class="row">
                                        &nbsp;
                                    </div>
                                </div>
                            </div>

                        </div>
                        <div class="form-section-row">

                            <div class="form-section">
                                <div class="input-field col s12">
                                    <div class="row">
                                        <asp:Label runat="server" AssociatedControlID="txtBody">Mail Body</asp:Label>
                                        <asp:TextBox ID="txtBody" runat="server" TextMode="MultiLine" CssClass="materialize-textarea" Text="This is report email sent from Mobile Office Manager. Please find the Contract Listing by Route Active Only Report attached."></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div style="clear: both;"></div>

                        <footer style="float: right;">
                            <div class="btnlinks">
                                <asp:LinkButton runat="server" ID="hideModalPopupViaServerConfirm" Text="Save" OnClick="hideModalPopupViaServerConfirm_Click"
                                    ValidationGroup="mail" />
                            </div>
                        </footer>
                    </div>
                </ContentTemplate>
            </telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>

    <asp:HiddenField runat="server" ID="hdnReportSelectDtRange" Value="" />
    <asp:HiddenField runat="server" ID="hdnCssActive" Value="1" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="javascriptHolder" runat="Server">
    <script type="text/javascript">
        function CssClearLabel() {
            $('#<%=lblDay.ClientID%>').removeClass("labelactive");
            $('#<%=lblWeek.ClientID%>').removeClass("labelactive");
            $('#<%=lblMonth.ClientID%>').removeClass("labelactive");
            $("#<%=lblQuarter.ClientID%>").removeClass("labelactive");
            $('#<%=lblYear.ClientID%>').removeClass("labelactive");
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
                var val;
                val = localStorage.getItem("hdnReportDate");
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
            var radio = document.getElementsByName(rdGroup); //Client ID of the RadioButtonList1 
            var selected;
            for (var i = 0; i < radio.length; i++) {
                if (radio[i].checked) { // Checked property to check radio Button check or not
                    //alert("Radio button having value " + radio[i].value + " was checked."); // Show the checked value
                    selected = radio[i].value;

                }
                if (selected == "") {
                    selected = 'rdWeek';
                }
            }
            if (selected == 'rdDay') {

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
            else if (selected == 'rdWeek') {
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


            else if (selected == 'rdQuarter') {
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
            else if (selected == 'rdYear') {

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
            //clickSearchButton.click();
            document.getElementById('<%= hdnCssActive.ClientID%>').value = "1";
        }
    </script>
</asp:Content>
