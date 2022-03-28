<%@ Page Title="PeriodCloseout || MOM" Language="C#" MasterPageFile="~/Mom.master" AutoEventWireup="true" Inherits="PeriodCloseout" Codebehind="PeriodCloseout.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        function showDiv() {

            if ($("#<%=chkYearEndClose.ClientID %>").is(":checked")) {
                $(".closeyear").hide();
                $(".thisyear").show();
            }
            else {
                $(".closeyear").show();
                $(".thisyear").hide();
            }
        }

        function getNextDate(date) {
            var newDate = new Date(new Date(date).getTime() + 86400000);
            var getDay = newDate.getDate();
            var getMonth = newDate.getMonth() + 1;
            var getYear = newDate.getFullYear();

            var nextDate = getMonth + "/" + getDay + "/" + getYear;
            return nextDate;
        }

        function getLastDateOfMonth(date) {
            var getDate = new Date(date);
            var getDay = new Date(getDate.getFullYear(), getDate.getMonth() + 1, 0).getDate();
            var getMonth = new Date(getDate.getFullYear(), getDate.getMonth() + 1, 0).getMonth() + 1;
            var getYear = new Date(getDate.getFullYear(), getDate.getMonth() + 1, 0).getFullYear();

            var lastDateOfMonth = getMonth + "/" + getDay + "/" + getYear;
            return lastDateOfMonth;
        }

        function setUserDatePermission(control) {
            var valCloseOutDate = $(control).val();
            var fStart = getNextDate(valCloseOutDate);
            var fEnd = getLastDateOfMonth(fStart);

            $(".txtStartDate").pikaday('setDate', new Date(fStart));
            $(".txtEndDate").pikaday('setDate', new Date(fEnd));
        }

    </script>
    <style>
.card {
overflow: hidden;
min-height: 183px !important;
border-radius: 6px;
}
</style>

    <!--Grid Control-->
    <%--<link href="Design/css/grid.css" rel="stylesheet" />--%>

    <link href="Design/css/pikaday.css" rel="stylesheet" />
    <!--Grid Control-->
    <link href="Design/css/grid.css" rel="stylesheet" />
    <!--File Upload Control-->
    <link href="Design/js/plugins/dropify/css/dropify.css" type="text/css" rel="stylesheet" media="screen,projection">
    <script src="js/jquery.sumoselect.min.js"></script>
    <script type="text/javascript" src="js/jquery.geocomplete.js"></script>
    <link href="Styles/sumoselect.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="divbutton-container">
        <div id="breadcrumbs-wrapper">
            <header>
                <div class="container row-color-grey">
                    <div class="row">
                        <div class="col s12 m12 l12">
                            <div class="row">
                                <div class="page-title"><i class="mdi-communication-contacts"></i>&nbsp;Period Closeout</div>
                                <div class="buttonContainer">
                                    <div class="btnlinks">
                                        <asp:LinkButton runat="server" ID="btnSubmit" CssClass="icon-save" Text="Save" OnClick="btnSubmit_Click">

                                        </asp:LinkButton>
                                        <%--    <a class="icon-save" id="btnSubmit" runat="server" title="Save" onserverclick="btnSubmit_Click">Save
                                    </a>--%>
                                    </div>
                                </div>
                                <div class="btnclosewrap">
                                    <a runat="server" onserverclick="lnkClose_Click"><i class="mdi-content-clear" id="lnkClose" runat="server" title="Close"></i></a>
                                </div>
                                <div class="rght-content">
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </header>
        </div>

     <%--   <div class="container breadcrumbs-bg-custom">
            <div class="row">
              
                <div class="col s12 m5 l5">
                    <div class="row">
                        <ul class="anchor-links" style="float: right;">
                            <li>
                                <asp:Label runat="server" ID="lblUserName" class="title_text_Name_1"></asp:Label>
                            </li>
                            <li>
                                <asp:Label runat="server" ID="lblLastProcessDate" CssClass="title_text_Name_1"></asp:Label>
                            </li>
                            <li>
                                <asp:Label runat="server" ID="lblProcessPeriod" CssClass="title_text_Name_1 "></asp:Label>
                            </li>
                        </ul>
                    </div>
                </div>
            </div>

        </div>--%>

        <!-- edit-tab start -->
        <div class="container accordian-wrap">
            <div class="row">
                <div class="col s12 m12 l12">
                    <div class="row">
                        <div class="card">
                            <div class="card-content">
                                <div class="form-content-wrap formpdtop2">
                                    <div class="form-content-pd">
                                        <div class="form-section-row">
                                            <div class="col s12 m12 l12 p-r-0" >
                                                <div class="row">
                                                    <div class="form-section-row">

                                                        <div class="form-section3">
                                                            <div class="section-ttle">Close Out Info</div>
                                                            <div class="input-field col s5">
                                                                <div class="row">
                                                                    <label>Close Out Date</label>
                                                                    <asp:RequiredFieldValidator ID="rfvCloseOutDate"
                                                                        runat="server" ControlToValidate="txtCloseOutDate" Display="None" ErrorMessage="Closed out date is Required"
                                                                        SetFocusOnError="True">
                                                                    </asp:RequiredFieldValidator>
                                                                    <asp:ValidatorCalloutExtender ID="vceCloseOutDate" runat="server" Enabled="True"
                                                                        PopupPosition="Right" TargetControlID="rfvCloseOutDate" />
                                                                    <%--<input type="text" id="txtCloseOutDate" runat="server" class="pd-negate datepicker_mom">--%>
                                                                    <asp:TextBox ID="txtCloseOutDate" runat="server" CssClass="pd-negate datepicker_mom txtCloseOutDate"
                                                                        onkeypress="return false;" onblur="setUserDatePermission(this)"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                            <div class="input-field col s2">
                                                                <div class="row">
                                                                    &nbsp;
                                                                </div>
                                                            </div>
                                                            <div class="input-field col s5">
                                                                <div class="checkrow">
                                                                    <%--<input type="checkbox" id="chkYearEndClose" runat="server" value="This is a Year End Close" class="filled-in" onchange="showDiv();">--%>
                                                                    <asp:CheckBox ID="chkYearEndClose" runat="server" onchange="showDiv();" />
                                                                    <label for="referral">This is year end close</label>
                                                                </div>
                                                            </div>

                                                            <div class="input-field col s12">
                                                                <div class="row">
                                                                    <br>
                                                                    Please note: Year end close out will make a journal entry to the selected GL accounts, which can be modified at a later time. 
                                                                </div>
                                                            </div>

                                                        </div>
                                                        <div class="form-section3-blank">
                                                            &nbsp;
                                                        </div>

                                                        <div class="form-section3 thisyear" style="display: none;">
                                                            <div class="section-ttle"><i class="mdi-alert-warning warningcss" ></i>&nbsp; Caution</div>
                                                            <div class="">
                                                                <div class="input-field col s12">
                                                                    <div class="row align-items-center">
                                                                        <label class="drpdwn-label">Retained Earnings GL Account</label>
                                                                        <asp:RequiredFieldValidator ID="rfvRetainEarn"
                                                                            runat="server" ControlToValidate="ddlRetainedGLAcct" Display="None" ErrorMessage="Select Retained earnings GL account"
                                                                            SetFocusOnError="True">

                                                                        </asp:RequiredFieldValidator>
                                                                        <asp:ValidatorCalloutExtender
                                                                            ID="vceRetainEarn" runat="server" Enabled="True"
                                                                            PopupPosition="Right" TargetControlID="rfvRetainEarn" />
                                                                        <asp:DropDownList ID="ddlRetainedGLAcct" runat="server" CssClass="browser-default  selectsm"></asp:DropDownList>
                                                                    </div>
                                                                </div>
                                                                <div class="input-field col s12">
                                                                    <div class="row align-items-center">
                                                                        <label class="drpdwn-label">Current Earnings GL Account</label>
                                                                        <asp:RequiredFieldValidator ID="rfvCurrentEarn"
                                                                            runat="server" ControlToValidate="ddlCurrentGLAcct" Display="None" ErrorMessage="Select Current earnings GL account"
                                                                            SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                        <asp:ValidatorCalloutExtender
                                                                            ID="vceCurrentEarn" runat="server" Enabled="True"
                                                                            PopupPosition="Right" TargetControlID="rfvCurrentEarn" />
                                                                        <asp:DropDownList ID="ddlCurrentGLAcct" runat="server" CssClass="browser-default  selectsm"></asp:DropDownList>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="form-section3-blank">
                                                            &nbsp;
                                                        </div>
                                                        <div class="form-section3">
                                                            <div class="section-ttle">Set User Date Permission</div>
                                                            <div class="input-field col s5">
                                                                <div class="row">
                                                                    <label>Start Date</label>
                                                                    <asp:RequiredFieldValidator ID="rfvStartDate"
                                                                        runat="server" ControlToValidate="txtStartDate" Display="None" ErrorMessage="Start date is required"
                                                                        SetFocusOnError="True">
                                                                    </asp:RequiredFieldValidator>
                                                                    <asp:ValidatorCalloutExtender ID="vceStartDate" runat="server" Enabled="True"
                                                                        PopupPosition="Right" TargetControlID="rfvStartDate" />
                                                                    <asp:TextBox ID="txtStartDate" runat="server" CssClass="pd-negate datepicker_mom txtStartDate"
                                                                        onkeypress="return false;"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                            <div class="input-field col s2">
                                                                <div class="row">
                                                                    &nbsp;
                                                                </div>
                                                            </div>
                                                            <div class="input-field col s5">
                                                                <div class="row">
                                                                    <label>End Date</label>
                                                                    <asp:RequiredFieldValidator ID="rfvEndDate"
                                                                        runat="server" ControlToValidate="txtEndDate" Display="None" ErrorMessage="End date is required"
                                                                        SetFocusOnError="True">
                                                                    </asp:RequiredFieldValidator>
                                                                    <asp:ValidatorCalloutExtender ID="vceEndDate" runat="server" Enabled="True"
                                                                        PopupPosition="Right" TargetControlID="rfvEndDate" />
                                                                    <asp:TextBox ID="txtEndDate" runat="server" CssClass="pd-negate datepicker_mom txtEndDate"
                                                                        onkeypress="return false;"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                            <div class="input-field col s2">
                                                                <div class="row">
                                                                    &nbsp;
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>


                                                    <%--     <div class="form-section-row">
                                                        <div class="clearfix"></div>
                                                        <div class="form-group">
                                                            <div class="form-col">
                                                                <div class="closeyear" style="display: block;">
                                                                    <span style="font-size: small;">Closing out the period will update all user edit start to the day following the period closeout. </span>
                                                                </div>
                                                                <div id="thisyear" class="thisyear" style="display: none;">
                                                                    <span style="font-size: small;">Closing out the year will transfer the net profit amount of the designated period from
                                the current earnings account to the retained earnings account on the balance sheet.
                                The transfer will be in the form of a GL adjustment and can be edited or deleted afterwards
                                like any other adjustment. </span>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>--%>

                                                    <%--                       <div class="form-section-row">
                                                        <div class="form-group">
                                                            <div class="form-col">
                                                                <asp:Label ID="lblWarningMesg" runat="server" Text="CAUTION" ForeColor="Red" Font-Bold="True" Font-Size="Large"></asp:Label>
                                                            </div>
                                                        </div>
                                                        <div class="thisyear" style="display: none;">
                                                            <div class="form-section3">
                                                                <div class="form-col">
                                                                    <div class="fc-label">
                                                                        Retained Earnings GL Account
                                                                        <asp:RequiredFieldValidator ID="rfvRetainEarn"
                                                                            runat="server" ControlToValidate="ddlRetainedGLAcct" Display="None" ErrorMessage="Select Retained earnings GL account"
                                                                            SetFocusOnError="True">

                                                                        </asp:RequiredFieldValidator>
                                                                        <asp:ValidatorCalloutExtender
                                                                            ID="vceRetainEarn" runat="server" Enabled="True"
                                                                            PopupPosition="Right" TargetControlID="rfvRetainEarn" />
                                                                    </div>
                                                                    <div class="fc-input">
                                                                        <asp:DropDownList ID="ddlRetainedGLAcct" runat="server" CssClass="browser-default selectst selectsm"></asp:DropDownList>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="form-section3-blank">
                                                                &nbsp;
                                                            </div>
                                                            <div class="form-section3">
                                                                <div class="form-col">
                                                                    <div class="fc-label">
                                                                        Current Earnings GL Account
                                                                        <asp:RequiredFieldValidator ID="rfvCurrentEarn"
                                                                            runat="server" ControlToValidate="ddlCurrentGLAcct" Display="None" ErrorMessage="Select Current earnings GL account"
                                                                            SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                        <asp:ValidatorCalloutExtender
                                                                            ID="vceCurrentEarn" runat="server" Enabled="True"
                                                                            PopupPosition="Right" TargetControlID="rfvCurrentEarn" />
                                                                    </div>
                                                                    <div class="fc-input">
                                                                        <asp:DropDownList ID="ddlCurrentGLAcct" runat="server" CssClass="browser-default selectst selectsm"></asp:DropDownList>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>--%>
                                                </div>
                                                <div class="cf"></div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
           <div id="tbLogs" runat="server" class="collapsible-header accrd accordian-text-custom" style="padding: 0px;">
                <i class="mdi-content-content-paste"></i>Logs
                                        <div class="grid_container">
                                            <div class="form-section-row m-b-t">
                                                <div class="RadGrid RadGrid_Material">
                                                    <telerik:RadCodeBlock ID="RadCodeBlock6" runat="server">
                                                        <script type="text/javascript">
                                                            function pageLoad() {
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
                                                            <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="True" ShowFooter="True" AllowNaturalSort="false" DataKeyNames="fUser">
                                                                <Columns>
                                                                    <telerik:GridTemplateColumn FilterDelay="5" DataField="CreatedStamp" SortExpression="CreatedStamp" AutoPostBackOnFilter="true" DataType="System.String"
                                                                        CurrentFilterFunction="Contains" HeaderText="Date" ShowFilterIcon="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lbldate" runat="server" Text='<%# Eval("CreatedStamp", "{0:M/d/yyyy}")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn FilterDelay="5" DataField="CreatedStamp" SortExpression="CreatedStamp" AutoPostBackOnFilter="true" DataType="System.String"
                                                                        CurrentFilterFunction="Contains" HeaderText="Time" ShowFilterIcon="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lbltime" runat="server" Text='<%# Eval("CreatedStamp","{0: hh:mm tt}") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn FilterDelay="5" DataField="fUser" SortExpression="fUser" AutoPostBackOnFilter="true"
                                                                        CurrentFilterFunction="Contains" HeaderText="User" ShowFilterIcon="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblUpdBy" runat="server" Text='<%# Eval("fUser") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn FilterDelay="5" DataField="Field" SortExpression="Field" AutoPostBackOnFilter="true"
                                                                        CurrentFilterFunction="Contains" HeaderText="Field" ShowFilterIcon="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblField" runat="server" Text='<%# Eval("field") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn FilterDelay="5" DataField="OldVal" SortExpression="OldVal" AutoPostBackOnFilter="true"
                                                                        CurrentFilterFunction="Contains" HeaderText="Old Value" ShowFilterIcon="false" HeaderStyle-Width="305">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblOldVal" runat="server" Text='<%# Eval("OldVal") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn FilterDelay="5" DataField="NewVal" SortExpression="NewVal" AutoPostBackOnFilter="true"
                                                                        CurrentFilterFunction="Contains" HeaderText="New Value" ShowFilterIcon="false" HeaderStyle-Width="305">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblNewVal" runat="server" Text='<%# Eval("NewVal") %>'></asp:Label>
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
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="javascriptHolder" runat="Server">
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
    </script>
    <script>
    </script>
</asp:Content>
