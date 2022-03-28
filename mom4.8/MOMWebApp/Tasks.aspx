<%@ Page Title="Tasks || MOM" Language="C#" MasterPageFile="~/Mom.master" AutoEventWireup="true" Inherits="Tasks" Codebehind="Tasks.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <!--Grid Control-->
    <link href="Design/css/grid.css" rel="stylesheet" />
    <link href="Design/css/pikaday.css" rel="stylesheet" />


    <script type="text/javascript">

        function CheckDelete() {
            var result = false;
            $("#<%=RadGrid_Task.ClientID%> tr").each(function () {
                var checkBox = $(this).find("input[type='checkbox']");
                if (checkBox.is(":checked")) {
                    result = true;
                }
            });

            if (result == true) {
                return confirm('Do you really want to delete this Task ?');
            }
            else {
                alert('Please select a Task to delete.')
                return false;
            }
        }

        jQuery(document).ready(function () {
            $('#colorNav #dynamicUI li').remove();
            $(reports).each(function (index, report) {
                var imagePath = null;
                if (report.IsGlobal == true) {
                    imagePath = "images/globe.png";
                }
                else {
                    imagePath = "images/blog_private.png";
                }

                $('#dynamicUI').append('<li><a href="TaskListingReport.aspx?reportId=' + report.ReportId + '&reportName=' + report.ReportName + '&type=Task"><span><i class="fa fa-book" Style="color:Blue;font-size:16px;" aria-hidden="true"></i> ' + report.ReportName + '</span><div style="clear:both;"></div></a></li>')

            });

            $(window).scroll(function () {
                if ($(window).scrollTop() >= 0) {
                    $('#divButtons').addClass('fixed-header');
                }
                if ($(window).scrollTop() <= 0) {
                    $('#divButtons').removeClass('fixed-header');
                }
            });
        });
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
                                        <asp:Label CssClass="title_text" ID="lblHeader" runat="server">Tasks</asp:Label>
                                    </div>
                                    <div class="buttonContainer">
                                        <div class="btnlinks">
                                            <asp:LinkButton ID="lnkAdd" runat="server" CausesValidation="False" OnClick="lnkAdd_Click">Add</asp:LinkButton>
                                        </div>
                                        <div class="btnlinks">
                                            <asp:LinkButton ID="lnkEdit" runat="server" CausesValidation="False" OnClick="lnkEdit_Click">Edit</asp:LinkButton>
                                        </div>
                                        <div class="btnlinks menuAction">
                                            <a onclick="DropdownMenu()" class="dropbtn" style="cursor: pointer;">Actions
                                            </a>
                                        </div>

                                        <ul id="drpMenu" class="nomgn hideMenu menuList">
                                            <li>
                                                <div class="btnlinks">
                                                    <asp:LinkButton ID="lnkDelete" runat="server" OnClientClick="return CheckDelete();" CausesValidation="False" OnClick="lnkDelete_Click">Delete</asp:LinkButton>
                                                </div>
                                            </li>
                                            <li>
                                                <div class="btnlinks">
                                                    <asp:LinkButton ID="lnkFollowup" runat="server" CausesValidation="False" OnClick="lnkFollowup_Click">Follow-Up</asp:LinkButton>
                                                </div>
                                            </li>
                                            <li>
                                                <ul id="dropdown1" class="dropdown-content">
                                                    <li>
                                                       <asp:LinkButton ID="lnkTaskSummaryReport" OnClick="lnkTaskSummaryReport_Click" runat="server">Task Summary Report</asp:LinkButton>
                                                    </li>
                                                </ul>
                                                <div class="btnlinks">
                                                    <a class="dropdown-button" data-beloworigin="true" href="customersreport.aspx" data-activates="dropdown1">Reports
                                                    </a>
                                                </div>
                                            </li>
                                        </ul>
                                    </div>
                                    <div class="btnclosewrap">
                                        <asp:LinkButton ToolTip="Close" ID="lnkClose" runat="server" CausesValidation="false"
                                            OnClick="lnkClose_Click"><i class="mdi-content-clear"></i></asp:LinkButton>
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
                <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="chkAllTask" />
                    </Triggers>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="chkAllUsers" />
                    </Triggers>
                    <ContentTemplate>
                        <div class="srchpaneinner">
                            <div class="srchtitle srchtitlecustomwidth ser-css2">
                                Date
                            </div>
                            <div class="srchinputwrap">
                                <asp:TextBox ID="txtFrom" CssClass="datepicker_mom srchcstm" placeholder="From" runat="server"></asp:TextBox>
                            </div>
                            <div class="srchinputwrap ">
                                <asp:TextBox ID="txtTo" CssClass="datepicker_mom srchcstm" runat="server" placeholder="To" MaxLength="28"></asp:TextBox>
                            </div>
                            <div class="srchinputwrap">
                                <ul class="tabselect accrd-tabselect" id="testradiobutton">
                                    <li>
                                        <asp:LinkButton AutoPostBack="True" ID="decDate" Style="margin-right: 3px;" runat="server" OnClientClick="dec_date('dec','ctl00_ContentPlaceHolder1_txtTo','ctl00_ContentPlaceHolder1_txtFrom','rdCal');return false;" Text="<i class='mdi-hardware-keyboard-arrow-left'></i>"></asp:LinkButton>
                                    <li>
                                        <label id="lblDay" runat="server">
                                            <input type="radio" id="rdDay" name="rdCal" value="rdDay" onclick="SelectDate('Day', 'ctl00_ContentPlaceHolder1_txtFrom', 'ctl00_ContentPlaceHolder1_txtTo', '#lblDay', 'hdnTaskDate', 'rdCal')" />
                                            Day
                                        </label>
                                    </li>
                                    <li>
                                        <label id="lblWeek" runat="server">
                                            <input type="radio" id="rdWeek" name="rdCal" value="rdWeek" onclick="SelectDate('Week', 'ctl00_ContentPlaceHolder1_txtFrom', 'ctl00_ContentPlaceHolder1_txtTo', '#ctl00_ContentPlaceHolder1_lblWeek', 'hdnTaskDate', 'rdCal')" />
                                            Week
                                        </label>
                                    </li>
                                    <li>
                                        <label id="lblMonth" runat="server">
                                            <input type="radio" id="rdMonth" name="rdCal" value="rdMonth" onclick="SelectDate('Month', 'ctl00_ContentPlaceHolder1_txtFrom', 'ctl00_ContentPlaceHolder1_txtTo', '#ctl00_ContentPlaceHolder1_lblMonth', 'hdnTaskDate', 'rdCal')" />
                                            Month
                                        </label>
                                    </li>
                                    <li>
                                        <label id="lblQuarter" runat="server">
                                            <input type="radio" id="rdQuarter" name="rdCal" value="rdQuarter" onclick="SelectDate('Quarter', 'ctl00_ContentPlaceHolder1_txtFrom', 'ctl00_ContentPlaceHolder1_txtTo', '#ctl00_ContentPlaceHolder1_lblQuarter', 'hdnTaskDate', 'rdCal')" />
                                            Quarter
                                        </label>
                                    </li>
                                    <li>
                                        <label id="lblYear" runat="server">
                                            <input type="radio" id="rdYear" name="rdCal" value="rdYear" onclick="SelectDate('Year', 'ctl00_ContentPlaceHolder1_txtFrom', 'ctl00_ContentPlaceHolder1_txtTo', '#ctl00_ContentPlaceHolder1_lblYear', 'hdnTaskDate', 'rdCal')" />
                                            Year
                                        </label>
                                    </li>
                                    <li>
                                        <asp:LinkButton AutoPostBack="True" ID="incDate" OnClientClick="dec_date('inc','ctl00_ContentPlaceHolder1_txtTo','ctl00_ContentPlaceHolder1_txtFrom','rdCal');return false" runat="server" Text="<i class='mdi-hardware-keyboard-arrow-right'></i>"></asp:LinkButton>
                                    </li>
                                </ul>
                            </div>

                            <div class="col lblsz2 lblszfloat">
                                <div class="row">
                                    <span class="tro trost">
                                        <asp:CheckBox ID="chkAllTask" runat="server" CausesValidation="false" CssClass="css-checkbox" AutoPostBack="true" Text="Show Completed" OnCheckedChanged="chkAllTask_CheckedChanged" />
                                    </span>
                                    <span class="tro trost">
                                        <asp:CheckBox ID="chkAllUsers" runat="server" CausesValidation="false" CssClass="css-checkbox" AutoPostBack="true" Text="Show All Users" OnCheckedChanged="chkAllUsers_CheckedChanged" />
                                    </span>
                                    <span class="tro trost">
                                        <asp:LinkButton ID="lnkShowAll" runat="server" CausesValidation="False"
                                            OnClick="lnkShowAll_Click">Show All</asp:LinkButton>
                                    </span>
                                    <%--Add Clear Functionality--%>
                                    <%--<span class="tro trost">
                                        <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False">Clear*</asp:LinkButton>
                                    </span>--%>
                                    <span class="tro trost">
                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                            <ContentTemplate>
                                                <asp:Label ID="lblRecordCount" runat="server"></asp:Label>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </span>
                                </div>
                            </div>
                        </div>
                        <div class="srchpaneinner">
                            <div class="srchtitle srchtitlecustomwidth ser-css2">
                                Search
                            </div>
                            <div class="srchinputwrap">
                                <asp:DropDownList ID="ddlSearch" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlSearch_SelectedIndexChanged"
                                    CssClass="browser-default selectst selectsml">
                                    <asp:ListItem Value=" ">--Select--</asp:ListItem>
                                    <asp:ListItem Value="r.name">Name</asp:ListItem>
                                    <asp:ListItem Value="t.subject">Subject</asp:ListItem>
                                    <asp:ListItem Value="t.remarks">Desc</asp:ListItem>
                                    <asp:ListItem Value="t.result">Resolution</asp:ListItem>
                                    <asp:ListItem Value="t.fuser">Assigned to</asp:ListItem>
                                    <asp:ListItem Value="days">Days</asp:ListItem>
                                    <asp:ListItem Value="status">Status</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="srchinputwrap">
                                <asp:DropDownList ID="ddlcompare" runat="server" CssClass="browser-default selectst selectsml" Visible="false">
                                    <asp:ListItem Value="0">=</asp:ListItem>
                                    <asp:ListItem Value="1">&#62;=</asp:ListItem>
                                    <asp:ListItem Value="2">&#60;=</asp:ListItem>
                                    <asp:ListItem Value="3">&#62;</asp:ListItem>
                                    <asp:ListItem Value="4">&#60;</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="srchinputwrap">
                                <asp:TextBox ID="txtSearch" CssClass="srchcstm" placeholder="Search" runat="server"></asp:TextBox>
                            </div>
                            <div class="srchinputwrap">
                                <asp:DropDownList ID="ddlUser" CssClass="browser-default selectst selectsml" runat="server" Visible="False">
                                </asp:DropDownList>
                            </div>
                            <div class="srchinputwrap">
                                <asp:DropDownList ID="ddlStatus" CssClass="browser-default selectst selectsml" Visible="false" runat="server" AutoPostBack="True">
                                    <asp:ListItem Value="0">Open</asp:ListItem>
                                    <asp:ListItem Value="1">Completed</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="srchinputwrap srchclr btnlinksicon m-lm-t">
                                <asp:LinkButton CausesValidation="false" ID="lnkSearch" runat="server"
                                    OnClick="lnkSearch_Click"><i class="mdi-action-search"></i> </asp:LinkButton>
                            </div>

                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>


            </div>

            <div class="grid_container">
                <div class="form-section-row pmd-card">

                    <telerik:RadAjaxManager ID="RadAjaxManager_Task" runat="server">
                        <AjaxSettings>
                            <telerik:AjaxSetting AjaxControlID="lnkChk">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="RadGrid_Task" LoadingPanelID="RadAjaxLoadingPanel_Task" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                            <telerik:AjaxSetting AjaxControlID="lnkSearch">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="RadGrid_Task" LoadingPanelID="RadAjaxLoadingPanel_Task" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                            <telerik:AjaxSetting AjaxControlID="lnkShowAll">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="RadGrid_Task" LoadingPanelID="RadAjaxLoadingPanel_Task" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>

                            <telerik:AjaxSetting AjaxControlID="chkAllTask">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="RadGrid_Task" LoadingPanelID="RadAjaxLoadingPanel_Task" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>

                            <telerik:AjaxSetting AjaxControlID="chkAllUsers">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="RadGrid_Task" LoadingPanelID="RadAjaxLoadingPanel_Task" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                            <telerik:AjaxSetting AjaxControlID="lnkDelete">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="RadGrid_Task" LoadingPanelID="RadAjaxLoadingPanel_Task" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>

                        </AjaxSettings>
                    </telerik:RadAjaxManager>

                    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel_Task" runat="server">
                    </telerik:RadAjaxLoadingPanel>

                    <div class="RadGrid RadGrid_Material">
                        <telerik:RadCodeBlock ID="RadCodeBlock_Task" runat="server">
                            <script type="text/javascript">
                                function pageLoad() {
                                    var grid = $find("<%= RadGrid_Task.ClientID %>");
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
                        <telerik:RadAjaxPanel ID="RadAjaxPanel_Task" runat="server" LoadingPanelID="RadAjaxLoadingPanel_Task" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">


                            <telerik:RadGrid RenderMode="Auto" ID="RadGrid_Task" AllowFilteringByColumn="true" ShowFooter="True" PageSize="50"
                                ShowStatusBar="true" runat="server" AllowPaging="True" AllowSorting="true" Width="100%" PagerStyle-AlwaysVisible="true" OnPreRender="RadGrid_Task_PreRender"
                                AllowCustomPaging="True" OnNeedDataSource="RadGrid_Task_NeedDataSource" OnItemCreated="RadGrid_Task_ItemCreated">
                                <CommandItemStyle />
                                <GroupingSettings CaseSensitive="false" />
                                <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                                    <Selecting AllowRowSelect="True"></Selecting>
                                    <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                    <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                </ClientSettings>
                                <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="True" ShowFooter="True" DataKeyNames="Name">
                                    <Columns>

                                        <telerik:GridClientSelectColumn UniqueName="ClientSelectColumn" HeaderStyle-Width="28">
                                        </telerik:GridClientSelectColumn>

                                        <telerik:GridTemplateColumn DataField="name" AutoPostBackOnFilter="true" SortExpression="name" HeaderText="Location Name" ShowFilterIcon="false" HeaderStyle-Width="140">
                                            <ItemTemplate>
                                                <asp:Label ID="lblID" runat="server" Visible="false" Text='<%# Eval("id") %>'></asp:Label>
                                                <asp:Label ID="lblname" runat="server" Text='<%# Eval("name") %>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn DataField="duedate" AutoPostBackOnFilter="true" SortExpression="duedate" HeaderText="Due Date/Date Done" ShowFilterIcon="false" HeaderStyle-Width="140" CurrentFilterFunction="Contains" DataType="System.String">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDuedate" runat="server" Style='<%#  Eval("statusid").ToString()!="1" ?( string.Format("color:{0}",Convert.ToDateTime( Eval("duedate") )<= System.DateTime.Now ? "RED": "BLACK")) : "Black" %>'
                                                    Text='<%# Eval("duedate", "{0:g}") %>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn DataField="days" AutoPostBackOnFilter="true" SortExpression="days" HeaderText="# Days" ShowFilterIcon="false" HeaderStyle-Width="80">
                                            <ItemTemplate>
                                                <asp:Label ID="lbldays" runat="server" Text='<%#   Eval("days").ToString().Replace("-",String.Empty) %>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridBoundColumn DataField="Subject" HeaderText="Subject"
                                            AutoPostBackOnFilter="true" SortExpression="Subject" CurrentFilterFunction="Contains" HeaderStyle-Width="150"
                                            ShowFilterIcon="false">
                                        </telerik:GridBoundColumn>

                                        <telerik:GridBoundColumn DataField="Remarks" HeaderText="Desc" HeaderStyle-Width="200"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="Remarks"
                                            ShowFilterIcon="false">
                                        </telerik:GridBoundColumn>

                                        <telerik:GridBoundColumn DataField="result" HeaderText="Resolution" HeaderStyle-Width="200"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="result"
                                            ShowFilterIcon="false">
                                        </telerik:GridBoundColumn>

                                        <telerik:GridBoundColumn DataField="fUser" HeaderText="Assigned to" HeaderStyle-Width="115"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="fUser"
                                            ShowFilterIcon="false">
                                        </telerik:GridBoundColumn>

                                        <telerik:GridBoundColumn DataField="status" HeaderText="Status" HeaderStyle-Width="80"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="status"
                                            ShowFilterIcon="false">
                                        </telerik:GridBoundColumn>

                                        <telerik:GridBoundColumn DataField="Company" HeaderText="Company" HeaderStyle-Width="140"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="Company"
                                            ShowFilterIcon="false">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="keyword" HeaderText="Category" HeaderStyle-Width="115"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="keyword"
                                            ShowFilterIcon="false">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="CreatedBy" HeaderText="Created By" HeaderStyle-Width="115"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="CreatedBy"
                                            ShowFilterIcon="false">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridTemplateColumn SortExpression="CreatedDate" HeaderText="Created Date" HeaderStyle-Width="140" DataField="CreatedDate" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCreatedDate" runat="server" 
                                                    Text='<%# (String.IsNullOrEmpty(Eval("CreatedDate").ToString())) ? "" : Eval("CreatedDate", "{0:MM/dd/yyyy h:mm tt}") %>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridBoundColumn DataField="screen" HeaderText="Screen" HeaderStyle-Width="120"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="screen"
                                            ShowFilterIcon="false">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="ref" HeaderText="Ref ID" HeaderStyle-Width="100"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="ref"
                                            ShowFilterIcon="false">
                                        </telerik:GridBoundColumn>

                                    </Columns>
                                </MasterTableView>
                            </telerik:RadGrid>
                        </telerik:RadAjaxPanel>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField runat="server" ID="hdnTaskSelectDtRange" Value="" />
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
            debugger;
            if (typeof (Storage) !== "undefined") {

                // Retrieve
                var SesVar = '<%= Convert.ToString(Session["lblTaskActive"])%>';
                var val;
                val = localStorage.getItem("hdnTaskDate");
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
                    else {
                        $("#<%=lblWeek.ClientID%>").addClass("labelactive");
                        document.getElementById("rdWeek").checked = true;
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
                        if (DATE.getMonth() == 5)
                        { NEWDATE.setDate(NEWDATE.getDate() + 1); }
                        else
                        { NEWDATE.setDate(NEWDATE.getDate()); }

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
                document.getElementById('<%= hdnTaskSelectDtRange.ClientID%>').value = "Day";
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
                document.getElementById('<%= hdnTaskSelectDtRange.ClientID%>').value = "Week";
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
                document.getElementById('<%= hdnTaskSelectDtRange.ClientID%>').value = "Month";
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
                document.getElementById('<%= hdnTaskSelectDtRange.ClientID%>').value = "Quarter";
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
                document.getElementById('<%= hdnTaskSelectDtRange.ClientID%>').value = "Year";
            }
            if (typeof (Storage) !== "undefined") {
                // Store
                localStorage.setItem(UniqueVal, document.getElementById('<%= hdnTaskSelectDtRange.ClientID%>').value);
            }
            document.getElementById('<%= hdnCssActive.ClientID%>').value = "CssActive";
            var clickSearchButton = document.getElementById("<%= lnkSearch.ClientID %>");
            clickSearchButton.click();
            document.getElementById('<%= hdnCssActive.ClientID%>').value = "1";
        }
    </script>
</asp:Content>
