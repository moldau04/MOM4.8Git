<%@ Page Title="e-Timesheet || MOM" Language="C#" MasterPageFile="~/Mom.master" AutoEventWireup="true" Inherits="Etimesheet" CodeBehind="Etimesheet.aspx.cs" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <!--Grid Control-->
    <link href="Design/css/grid.css" rel="stylesheet" />
    <!--File Upload Control-->
    <link href="Design/js/plugins/dropify/css/dropify.css" type="text/css" rel="stylesheet" media="screen,projection">

    <link href="Design/css/pikaday.css" rel="stylesheet" />
    <style>
        .money-symbol {
            width: 10px;
            /* height: 100%; */
            float: left;
            padding-top: 12px;
            align-content: center;
            vertical-align: middle !important;
        }
        .borderLabel{
           
            padding:4px 15px;
             font-size: 8pt;
             border: none; border-bottom: 1px solid #9e9e9e!important;
        }
        .texttransparent{
                font-size: 9pt;
                    width: 35px!important;
        }
         
         [id$='PageSizeComboBox']{
           width:5.1em !important;
        }
         .test{
             width:150px!important;
         }

        .RadGrid_Bootstrap .rgAltRow > td {
        font-size: 10px!important;
        }
        #ctl00_ContentPlaceHolder1_gvTimesheet_ctl00_ctl03_ctl01_PageSizeComboBox{
            width:5.1em !important;
        }
        .text-area-css {
            border: 1px solid !important;
            border-color: #ccc !important;
            color: #1c5fb1 !important;
            height: 20px !important;
            width: 30px !important
        }
        .RadGrid_Material .rgHeader {
            font-size: 0.75rem!important;
            font-weight:bold!important;
        }
        .RadGrid_Material th a, rgHeader {
         padding: 0px!important;
         font-size: 0.75rem!important;
         }
        .RadGrid .rgRow > td {
        font-size: 0.75rem !important;
        }
        .RadGrid {
        line-height: 0;
        }
        .rgAltRow input[type=text]{
            height: .5rem;
            margin: 0 0 0px 0;
        }
        .rgRow input[type=text]{
            background-color: transparent;
            border: none;
            border-bottom: 1px solid #9e9e9e;
            border-radius: 0;
            outline: none;
            height: .5rem;
            width: 100%;
            font-size: 0.9em;
            margin: 0 0 0px 0;
            box-shadow: none;
            -webkit-box-sizing: content-box;
            -moz-box-sizing: content-box;
             box-sizing: content-box;
                transition: all .3s;
        }
        .RadGrid_Material .rgHeader {
            padding: 5px 11px !important;
        }
       .RadGrid_Bootstrap .rgDetailTable {
            
            padding: 5px;
            background-color: #5282b7;
            border-radius: 7px;
        }
       
        .rgDetailTable tr:nth-child(odd) {
            background:#bad7f7!important;
        }
    </style>

    <script type="text/javascript">
        function ShowMessage(message, messageType) {
            noty({
                text: message,
                type: messageType,
                layout: 'topCenter',
                closeOnSelfClick: false,
                timeout: true,
                theme: 'noty_theme_default',
                closable: true,
                timeout: 3000
            });
        }
    </script>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="topNav">
        <div id="divButtons" class="">
            <div id="breadcrumbs-wrapper">

                <header>
                    <div class="container row-color-grey">
                        <div class="row">
                            <div class="col s12 m12 l12">
                                <div class="row">
                                    <div class="page-title"><i class="mdi-image-timelapse"></i>&nbsp;e-Timesheet</div>
                                    <div class="buttonContainer">
                                        <div class="btnlinks">
                                            <%--<a class="dropdown-button" data-beloworigin="true" href="#!" data-activates="dropdown0">Export
                                        </a>--%>
                                            <asp:LinkButton ID="lnkExport" class="dropdown-button" data-beloworigin="true" data-activates="dropdown0" runat="server">Export To</asp:LinkButton>
                                        </div>
                                        <ul id="dropdown0" class="dropdown-content">
                                            <li>
                                                <asp:LinkButton ID="lnkExportToCSV" runat="server" OnClick="lnkExportToCSV_Click" CssClass="-text" Text="CSV"></asp:LinkButton>
                                            </li>
                                            <li>
                                                <asp:LinkButton ID="lnkExportToText" runat="server" OnClick="lnkExportToText_Click" CssClass="-text" Text="Text"></asp:LinkButton>
                                            </li>
                                            <li>
                                                <asp:LinkButton ID="lnkExportToExcel" runat="server" OnClick="lnkExportToExcel_Click" CssClass="-text" Text="Excel"></asp:LinkButton>
                                            </li>
                                        </ul>
                                        <div class="btnlinks">
                                            <asp:LinkButton CssClass="icon-save" ID="lnkProcess" runat="server" CausesValidation="False" Style="display: none" OnClientClick="removeDummyRows();">Save</asp:LinkButton>
                                        </div>
                                        <div class="btnlinks">
                                            <asp:LinkButton ID="lnkSave" runat="server" CausesValidation="False" Style="display: none">Save</asp:LinkButton>
                                        </div>
                                        <div class="btnlinks">
                                            <a class="dropdown-button" data-beloworigin="true" href="#!" data-activates="dropdown1">Reports
                                            </a>
                                        </div>
                                        <ul id="dropdown1" class="dropdown-content">
                                            <li>
                                                <a href="CustomersReport.aspx?type=Customer" class="-text">Add New Report</a>
                                            </li>
                                            <li>
                                                <a href="TicketByType.aspx" class="-text">Ticket Listing Report</a>
                                            </li>
                                            <li>
                                                <asp:LinkButton ID="lnkTimeRecap" OnClick="lnkTimeRecap_Click" runat="server">Time Recap 
                                                </asp:LinkButton>
                                            </li>
                                            <li>
                                                <asp:LinkButton ID="lnkTimeRecapAll" OnClick="lnkTimeRecapAll_Click" runat="server">Time Recap - all hours
                                                </asp:LinkButton>
                                            </li>
                                            <li>
                                                <asp:LinkButton ID="lnkTimeRecapwithlaborcost" OnClick="lnkTimeRecapwithlaborcost_Click" runat="server">Time Recap with labor cost
                                                </asp:LinkButton>
                                            </li>
                                            <li>
                                                <asp:LinkButton ID="lnkTimeRecapWithCostAll" OnClick="lnkTimeRecapWithCostAll_Click" runat="server">Time Recap with labor cost - all hours
                                                </asp:LinkButton>
                                            </li>
                                        </ul>
                                        <div class="btnlinks">
                                            <asp:LinkButton ID="lnkExcel" runat="server" OnClick="lnkExcel_Click">Export to Excel</asp:LinkButton>
                                        </div>
                                        <div class="btnlinks" id="divExportPayRoll" runat="server">
                                            <asp:LinkButton ID="lnkExportPayRoll" runat="server" OnClientClick="return checkCoCode();" OnClick="lnkExportPayRoll_Click">Export Payroll</asp:LinkButton>
                                        </div>
                                        <div class="rght-content">
                                        </div>
                                    </div>

                                    <div class="btnclosewrap">
                                        <asp:LinkButton ID="lnkClose" runat="server" CausesValidation="false" OnClick="lnkClose_Click">  <i class="mdi-content-clear"></i></asp:LinkButton>
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
            <div class="srchpane-advanced">
                <asp:HiddenField ID="hdnTickets" runat="server" />
                <asp:HiddenField ID="hdnWeeks" runat="server" />
                <div class="srchpaneinner">
                    <asp:UpdatePanel runat="server" ID="timeSelectionPanel" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="srchtitle srchtitlecustomwidth">
                                Payroll Period
                            </div>
                            <div class="srchinputwrap">
                                <asp:TextBox ID="txtFromDate" runat="server" CssClass="srchcstm datepicker_mom"></asp:TextBox>
                            </div>
                            <div class="srchinputwrap">
                                <%--<asp:TextBox ID="TextBox1" runat="server" CssClass="srchcstm datepicker_mom" placeholder="To"></asp:TextBox>--%>
                                <asp:TextBox ID="txtToDate" runat="server" CssClass="srchcstm datepicker_mom"></asp:TextBox>
                            </div>
                            <div class="srchinputwrap srchclr btnlinksicon">
                                <%-- <a id="lnkSearch" runat="server" causesvalidation="false"><i class="mdi-action-search"></i></a>--%>
                                <asp:LinkButton ID="lnkSearch" runat="server" CausesValidation="false" OnClick="lnkSearch_Click">
                             <i class="mdi-action-search"></i>
                                </asp:LinkButton>
                            </div>
                            <div class="srchinputwrap">
                                <ul class="tabselect accrd-tabselect" id="testradiobutton">
                                    <li>
                                        <asp:LinkButton AutoPostBack="False" ID="LinkButton3" runat="server" Text="<i class='mdi-hardware-keyboard-arrow-left'></i>" OnClick="decDate_Click"></asp:LinkButton>
                                    </li>
                                    <li>
                                        <label id="lblDay" runat="server">
                                            <asp:RadioButton runat="server" ID="rdDay" GroupName="rdCal" AutoPostBack="True" OnCheckedChanged="rdDay_CheckedChanged" />
                                            Day
                                        </label>
                                    </li>
                                    <li>
                                        <label id="lblWeek" runat="server">
                                            <asp:RadioButton runat="server" ID="rdWeek" GroupName="rdCal" AutoPostBack="True" OnCheckedChanged="rdWeek_CheckedChanged" />
                                            Week
                                        </label>
                                    </li>
                                    <li>
                                        <label id="lblMonth" runat="server">
                                            <asp:RadioButton runat="server" ID="rdMonth" GroupName="rdCal" AutoPostBack="True" OnCheckedChanged="rdMonth_CheckedChanged" />
                                            Month
                                        </label>
                                    </li>
                                    <li>
                                        <label id="lblQuarter" runat="server">
                                            <%--<input type="radio" id="rdQuarter" name="rdCal" value="rdQuarter" onclick="SelectDate('Quarter', 'ctl00_ContentPlaceHolder1_txtFromDate', 'ctl00_ContentPlaceHolder1_txtToDate', '#ctl00_ContentPlaceHolder1_lblQuarter', 'hdnRecvDate', 'rdCal')" />--%>
                                            <asp:RadioButton runat="server" ID="rdQuarter" GroupName="rdCal" AutoPostBack="True" OnCheckedChanged="rdQuarter_CheckedChanged" />
                                            Quarter
                                        </label>
                                    </li>
                                    <li>
                                        <label id="lblYear" runat="server">
                                            <asp:RadioButton runat="server" ID="rdYear" GroupName="rdCal" AutoPostBack="True" OnCheckedChanged="rdYear_CheckedChanged" />
                                            Year
                                        </label>
                                    </li>
                                    <li>
                                        <asp:LinkButton ID="LinkButton4" runat="server" Text="<i class='mdi-hardware-keyboard-arrow-right'></i>" OnClick="incDate_Click"></asp:LinkButton>
                                    </li>
                                </ul>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="srchpaneinner">

                    <div class="srchtitle srchtitlecustomwidth">
                        Department
                    </div>
                    <div class="srchinputwrap">
                        <asp:DropDownList ID="ddlDepartment" AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddlDepartment_SelectedIndexChanged" CssClass="browser-default selectsml selectst">
                        </asp:DropDownList>

                    </div>
                    <div class="srchtitle srchtitlecustomwidth">
                        Supervisor
                    </div>
                    <div class="srchinputwrap">
                        <asp:DropDownList ID="ddlSuper" AutoPostBack="true" OnSelectedIndexChanged="ddlSuper_SelectedIndexChanged" runat="server" CssClass="browser-default selectsml selectst">
                        </asp:DropDownList>
                    </div>
                    <div class="srchtitle srchtitlecustomwidth">
                        Worker
                    </div>
                    <div class="srchinputwrap">
                        <asp:DropDownList ID="ddlworker" AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddlworker_SelectedIndexChanged" CssClass="browser-default selectsml selectst">
                        </asp:DropDownList>
                    </div>
                    <div class="srchtitle srchtitlecustomwidth">
                        Timesheet 
                    </div>
                    <div class="srchinputwrap">
                        <asp:DropDownList ID="ddlTimesheet" AutoPostBack="true" OnSelectedIndexChanged="ddlTimesheet_SelectedIndexChanged" runat="server" CssClass="browser-default selectsml selectst">
                            <asp:ListItem Text="All" Value="-1"></asp:ListItem>
                            <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                            <asp:ListItem Text="No" Value="0"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="col lblsz2 lblszfloat">
                        <div class="row">
                            <%-- <asp:UpdatePanel runat="server" ID="udpRecordCount" UpdateMode="Conditional">
                                <ContentTemplate>--%>
                            <span class="tro trost">
                                <asp:LinkButton ID="lnkClear" runat="server" OnClick="lnkClear_Click">Clear</asp:LinkButton>
                            </span>
                            <span class="tro trost">
                                <asp:Label ID="lblRecordCount" runat="server"></asp:Label>
                            </span>
                            <%--     
                                </ContentTemplate>
                            </asp:UpdatePanel>--%>

                            <span class="tro trost">
                                <asp:Label ID="lblProcessed" runat="server" CssClass="shadow" Style="color: Red; font-weight: bold; font-size: medium;"
                                    Text="PROCESSED"></asp:Label>
                            </span>
                            <span class="tro trost">
                                <asp:Label ID="lblSaved" runat="server" CssClass="shadow " Style="color: Blue; font-weight: bold; font-size: medium;"
                                    Text="SAVED"></asp:Label>
                            </span>
                        </div>
                    </div>
                </div>

            </div>

            <div class="grid_container">
                <div class="form-section-row mb">
                    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
                        <AjaxSettings>
                            <telerik:AjaxSetting AjaxControlID="lnkSearch">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="gvTimesheet" LoadingPanelID="RadAjaxLoadingPanel_Timesheet" />
                                    <telerik:AjaxUpdatedControl ControlID="lblRecordCount" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>

                            <telerik:AjaxSetting AjaxControlID="lnkShowAll">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="gvTimesheet" LoadingPanelID="RadAjaxLoadingPanel_Timesheet" />
                                    <telerik:AjaxUpdatedControl ControlID="lblRecordCount" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                            <telerik:AjaxSetting AjaxControlID="lnkClear">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="ddlSuper" />
                                    <telerik:AjaxUpdatedControl ControlID="ddlDepartment" />
                                    <telerik:AjaxUpdatedControl ControlID="ddlworker" />
                                    <telerik:AjaxUpdatedControl ControlID="ddlTimesheet" />
                                    <telerik:AjaxUpdatedControl ControlID="gvTimesheet" LoadingPanelID="RadAjaxLoadingPanel_Timesheet" />
                                    <telerik:AjaxUpdatedControl ControlID="lblRecordCount" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                            <telerik:AjaxSetting AjaxControlID="gvTimesheet">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="lblRecordCount" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                            <telerik:AjaxSetting AjaxControlID="ddlDepartment">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="gvTimesheet" />
                                     <telerik:AjaxUpdatedControl ControlID="lblRecordCount" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                            <telerik:AjaxSetting AjaxControlID="ddlSuper">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="ddlworker" />
                                    <telerik:AjaxUpdatedControl ControlID="gvTimesheet" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                            <telerik:AjaxSetting AjaxControlID="ddlTimesheet">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="gvTimesheet" />
                                     <telerik:AjaxUpdatedControl ControlID="lblRecordCount" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                            <telerik:AjaxSetting AjaxControlID="ddlworker">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="gvTimesheet" />
                                     <telerik:AjaxUpdatedControl ControlID="lblRecordCount" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                            <telerik:AjaxSetting AjaxControlID="gvTimesheet">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="gvTimesheet" />

                                </UpdatedControls>
                            </telerik:AjaxSetting>
                            <telerik:AjaxSetting AjaxControlID="lnkCoCodeSave">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="hdnCoCode" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                           
                        </AjaxSettings>
                    </telerik:RadAjaxManager>
                    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel_Timesheet" runat="server">
                    </telerik:RadAjaxLoadingPanel>
                    <div class="RadGrid RadGrid_Material">
                        <telerik:RadCodeBlock ID="RadCodeBlock_Timesheet" runat="server">
                            <script type="text/javascript">
                                function pageLoad() {
                                    var grid = $find("<%= gvTimesheet.ClientID %>");
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
                                if (element && element.tagName == "INPUT" && element.getAttribute('type') != 'checkbox') {
                                    element.focus();
                                    element.selectionStart = selectionStart;
                                }
                            }

                                function scrollItemToTop(itemID) {
                                    console.log($telerik.$($get(itemID)).offset().top);
                                    var grid = $find("<%= gvTimesheet.ClientID %>");
                                grid.scrollTo(0, $telerik.$($get(itemID)).offset().top);
                            }
                            </script>

                        </telerik:RadCodeBlock>

                        <telerik:RadAjaxPanel ID="RadAjaxPanel_Timesheet" runat="server" LoadingPanelID="RadAjaxLoadingPanel_Timesheet" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">


                            <telerik:RadPersistenceManager ID="RadPersistenceManager_Timesheet" runat="server">
                                <PersistenceSettings>
                                    <telerik:PersistenceSetting ControlID="gvTimesheet" />
                                </PersistenceSettings>
                            </telerik:RadPersistenceManager>


                            <telerik:RadGrid ID="gvTimesheet" RenderMode="Auto" ItemStyle-Font-Size="X-Small" HeaderStyle-Font-Size="X-Small" runat="server" CssClass="gvTimesheet table table-bordered table-striped table-condensed flip-content"
                                Width="100%" ShowStatusBar="true" AllowPaging="true" PageSize="25" PagerStyle-AlwaysVisible="true" AllowSorting="true" MasterTableView-DataKeyNames="ID,Reg1,OT1,DT1,TT1,NT1,Zone1,Mileage1,Extra1,Misc1,Toll1,HourRate1"
                                ShowFooter="true"
                                AllowFilteringByColumn="false"
                                OnItemCommand="gvTimesheet_ItemCommand"
                                OnPreRender="gvTimesheet_PreRender"
                                AutoGenerateColumns="false"
                                OnItemCreated="gvTimesheet_ItemCreated"
                                OnNeedDataSource="gvTimesheet_NeedDataSource"  
                              
                                >

                                
                                <PagerStyle Mode="NextPrevAndNumeric" />

                                <CommandItemStyle />
                                <GroupingSettings CaseSensitive="false" />
                                <ClientSettings AllowColumnsReorder="false" ReorderColumnsOnClient="true" AllowGroupExpandCollapse="true">
                                    <Selecting AllowRowSelect="False"></Selecting>
                                    <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                    <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                </ClientSettings>
                                <HeaderStyle Width="20px"></HeaderStyle>
                                <MasterTableView AutoGenerateColumns="false" ShowFooter="True" AllowFilteringByColumn="false"
                                    DataKeyNames="ID,Reg1,OT1,DT1,TT1,NT1,Zone1,Mileage1,Extra1,Misc1,Toll1,HourRate1,countDetail"
                                    GroupLoadMode="Client"
                                    HierarchyLoadMode="ServerOnDemand">
                                    <DetailTables>

                                        <telerik:GridTableView DataKeyNames="TicketID" ItemStyle-Font-Size="X-Small" Name="EditTickets" Width="75%" runat="server"
                                            GroupLoadMode="Client"
                                            HierarchyLoadMode="Client"
                                            ShowFooter="true"
                                            AllowSorting="false"
                                            AllowFilteringByColumn="false"
                                            AutoGenerateColumns="false">
                                            <Columns>

                                                <telerik:GridTemplateColumn HeaderText="Ticket#" HeaderStyle-Width="15px">
                                                    <ItemTemplate>
                                                        <asp:HyperLink Style="font-size: 10px!important" ID="lnkTick" NavigateUrl='<%# "addticket.aspx?comp=1&id=" + Eval("ticketid") %>'
                                                            Target="_blank" runat="server" Text='<%# Eval("ticketid") %>'></asp:HyperLink>
                                                        <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                                            <ContentTemplate>--%>
                                                        <asp:Button ID="lnkUpdateTick" CssClass="updatet" CausesValidation="false" CommandName="UpdateTicket" Style="display: none;" CommandArgument='<%# Eval("ticketid") %>' runat="server" Text="Update" />
                                                        <%--</ContentTemplate>
                                                                        </asp:UpdatePanel>--%>
                                                    </ItemTemplate>
                                                    <ItemStyle />
                                                </telerik:GridTemplateColumn>

                                                    <telerik:GridTemplateColumn HeaderText="Date" HeaderStyle-Width="40px">
                                                    <ItemTemplate>
                                                      <%#  Eval("Date") %>
                                                    </ItemTemplate>
                                                    <ItemStyle />
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Custom" HeaderStyle-Width="20px">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtTCustom" ForeColor="DarkSlateGray" Font-Size="8pt" Style="display: none"
                                                            Width="30px" Text='<%# BindList( Eval("custom") )%>' Onkeyup="calculateExpand($(this).closest('table'));"
                                                            runat="server">
                                                        </asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="ftb115" runat="server" TargetControlID="txtTCustom"
                                                            ValidChars="1234567890.-">
                                                        </asp:FilteredTextBoxExtender>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox Font-Size="8pt" ForeColor="DarkSlateGray" ID="lblTCustom" runat="server"
                                                            onfocus="this.blur();" CssClass="texttransparent" Width="30px"></asp:TextBox>
                                                    </FooterTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn AllowFiltering="false" HeaderText="Rate" HeaderStyle-Width="15px">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtTRate" class="text-area-css" Style=" border-color: #ccc !important; color: #1c5fb1 !important; height: 20px !important; width: 30px !important"
                                                            Font-Size="8pt" ToolTip="Hourly Rate"
                                                            Text='<%# BindList( Eval("hourlyRate") )%>' Onkeyup="calculateExpand($(this).closest('table'));"
                                                            onblur="$(this).closest('tr').find('.updatet').click();" runat="server">
                                                        </asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="ftb15" runat="server" TargetControlID="txtTRate"
                                                            ValidChars="1234567890.-">
                                                        </asp:FilteredTextBoxExtender>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Rate Job" HeaderStyle-Width="20px">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkTJobRate" runat="server"
                                                            Checked='<%# Convert.ToBoolean( Eval("CustomTick3") !=DBNull.Value ? Eval("CustomTick3") : 0 ) %>'
                                                            onchange="calculateExpand($(this).closest('table')); $(this).closest('tr').find('.updatet').click();"></asp:CheckBox>

                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Rate Job Hr." HeaderStyle-Width="20px">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtTJobRate" class="text-area-css"  Style=" border-color: #ccc !important; color: #1c5fb1 !important; height: 20px !important; width: 30px !important"
                                                            ForeColor="DarkSlateGray" Font-Size="8pt"
                                                            Width="30px" Text='<%#  Eval("CustomTick1") %>' Onkeyup="calculateExpand($(this).closest('table'));"
                                                            onblur="$(this).closest('tr').find('.updatet').click();" runat="server">
                                                        </asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="ftbn15" runat="server" TargetControlID="txtTJobRate"
                                                            ValidChars="1234567890.-">
                                                        </asp:FilteredTextBoxExtender>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Reg" DataField="Reg" HeaderStyle-Width="15px"
                                                    ItemStyle-BorderStyle="Solid" FooterStyle-BorderStyle="Solid" ItemStyle-BorderColor="#D6D6D6"
                                                    FooterStyle-BorderColor="#D6D6D6">
                                                    <ItemTemplate>
                                                        <asp:TextBox  class="text-area-css" Style="border-color: #ccc !important; color: #1c5fb1 !important; height: 20px !important; width: 30px !important"
                                                            Font-Size="8pt" ForeColor="DarkSlateGray" ID="txtTReg" Width="30px"
                                                            runat="server" Text='<%# BindList( Eval("Reg") )%>'
                                                            onblur="$(this).closest('tr').find('.updatet').click();"
                                                            Onkeyup="calculateExpand($(this).closest('table'));">
                                                        </asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="ftb16" runat="server" TargetControlID="txtTReg"
                                                            ValidChars="1234567890.-">
                                                        </asp:FilteredTextBoxExtender>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox Font-Size="8pt" ForeColor="DarkSlateGray" ID="lblTReg" runat="server"
                                                            onfocus="this.blur();" CssClass="texttransparent" Width="30px"></asp:TextBox>
                                                    </FooterTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="OT" HeaderStyle-Width="15px"
                                                    ItemStyle-BorderStyle="Solid" FooterStyle-BorderStyle="Solid" ItemStyle-BorderColor="#D6D6D6"
                                                    FooterStyle-BorderColor="#D6D6D6" DataField="OT">
                                                    <ItemTemplate>
                                                        <asp:TextBox Font-Size="8pt" class="text-area-css" Style="border-color: #ccc !important; color: #1c5fb1 !important; height: 20px !important; width: 30px !important"
                                                            ID="txtTOT" Width="30px" runat="server"
                                                            Text='<%# BindList (Eval("OT") )%>'
                                                            onblur="$(this).closest('tr').find('.updatet').click();"
                                                            Onkeyup="calculateExpand($(this).closest('table'));">
                                                        </asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="ftb17" runat="server" TargetControlID="txtTOT"
                                                            ValidChars="1234567890.-">
                                                        </asp:FilteredTextBoxExtender>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox Font-Size="8pt" ForeColor="DarkSlateGray" ID="lblTOT" runat="server"
                                                            onfocus="this.blur();" CssClass="texttransparent" Width="30px"></asp:TextBox>
                                                    </FooterTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="1.7" HeaderStyle-Width="15px"
                                                    ItemStyle-BorderStyle="Solid" FooterStyle-BorderStyle="Solid" ItemStyle-BorderColor="#D6D6D6"
                                                    FooterStyle-BorderColor="#D6D6D6" DataField="NT">
                                                    <ItemTemplate>
                                                        <asp:TextBox  class="text-area-css" Style="border-color: #ccc !important; color: #1c5fb1 !important; height: 20px !important; width: 30px !important"
                                                            Font-Size="8pt" ForeColor="DarkSlateGray" ID="txtTNT" Width="30px" runat="server"
                                                            Text='<%# BindList( Eval("NT") )%>'
                                                            onblur="$(this).closest('tr').find('.updatet').click();"
                                                            Onkeyup="calculateExpand($(this).closest('table'));">
                                                        </asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="ftb18" runat="server" TargetControlID="txtTNT"
                                                            ValidChars="1234567890.-">
                                                        </asp:FilteredTextBoxExtender>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox Font-Size="8pt" ForeColor="DarkSlateGray" ID="lblTNT" runat="server"
                                                            onfocus="this.blur();" CssClass="texttransparent" Width="30px"></asp:TextBox>
                                                    </FooterTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="DT" HeaderStyle-Width="15px"
                                                    ItemStyle-BorderStyle="Solid" FooterStyle-BorderStyle="Solid" ItemStyle-BorderColor="#D6D6D6"
                                                    FooterStyle-BorderColor="#D6D6D6" DataField="DT">
                                                    <ItemTemplate>
                                                        <asp:TextBox class="text-area-css" Style="border-color: #ccc !important; color: #1c5fb1 !important; height: 20px !important; width: 30px !important"
                                                            Font-Size="8pt" ForeColor="DarkSlateGray" ID="txtTDT" Width="30px" runat="server"
                                                            Text='<%# BindList( Eval("DT") )%>'
                                                            onblur="$(this).closest('tr').find('.updatet').click();"
                                                            Onkeyup="calculateExpand($(this).closest('table'));">
                                                        </asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="ftb19" runat="server" TargetControlID="txtTDT"
                                                            ValidChars="1234567890.-">
                                                        </asp:FilteredTextBoxExtender>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox Font-Size="8pt" ForeColor="DarkSlateGray" ID="lblTDT" runat="server"
                                                            onfocus="this.blur();" CssClass="texttransparent" Width="30px"></asp:TextBox>
                                                    </FooterTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="TT" HeaderStyle-Width="15px"
                                                    ItemStyle-BorderStyle="Solid" FooterStyle-BorderStyle="Solid" ItemStyle-BorderColor="#D6D6D6"
                                                    FooterStyle-BorderColor="#D6D6D6">
                                                    <ItemTemplate>
                                                        <asp:TextBox class="text-area-css" Style="border-color: #ccc !important; color: #1c5fb1 !important; height: 20px !important; width: 30px !important"
                                                            Font-Size="8pt" ForeColor="DarkSlateGray" ID="txtTravel" Width="30px"
                                                            runat="server" Text='<%# BindList( Eval("TT") )%>'
                                                            onblur="$(this).closest('tr').find('.updatet').click();"
                                                            Onkeyup="calculateExpand($(this).closest('table'));">
                                                        </asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="ftb20" runat="server" TargetControlID="txtTravel"
                                                            ValidChars="1234567890.-">
                                                        </asp:FilteredTextBoxExtender>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox Font-Size="8pt" ForeColor="DarkSlateGray" ID="lblTravel" runat="server"
                                                            onfocus="this.blur();" CssClass="texttransparent" Width="30px"></asp:TextBox>
                                                    </FooterTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Total Time" HeaderStyle-Width="20px"
                                                    ItemStyle-BorderStyle="Solid" FooterStyle-BorderStyle="Solid" ItemStyle-BorderColor="#D6D6D6"
                                                    FooterStyle-BorderColor="#D6D6D6">
                                                    <ItemTemplate>
                                                        <asp:TextBox Style="border: 0 solid white!important" Font-Size="8pt" ForeColor="DarkSlateGray" ID="txtPTimeTotal" Width="45px"
                                                            runat="server" onfocus="this.blur();" CssClass="texttransparent">
                                                        </asp:TextBox>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox Font-Size="8pt" ForeColor="DarkSlateGray" ID="lblPTimeTotal" runat="server"
                                                            onfocus="this.blur();" CssClass="texttransparent" Width="30px">
                                                        </asp:TextBox>
                                                    </FooterTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Amount" HeaderStyle-Width="25px">
                                                    <ItemTemplate>
                                                        <asp:TextBox Style="border: 0 solid white!important" Font-Size="8pt" onfocus="this.blur();" CssClass="texttransparent" ForeColor="DarkSlateGray"
                                                            ID="txtTAmount" Width="30px" runat="server">
                                                        </asp:TextBox>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox Font-Size="8pt" ForeColor="DarkSlateGray" ID="lblTAmount" runat="server"
                                                            onfocus="this.blur();" CssClass="texttransparent" Width="30px">
                                                        </asp:TextBox>
                                                    </FooterTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Zone" HeaderStyle-Width="10px">
                                                    <ItemTemplate>
                                                        <asp:TextBox class="text-area-css" Style=" border-color: #ccc !important; color: #1c5fb1 !important; height: 20px !important; width: 10px !important"
                                                            Font-Size="8pt" ForeColor="DarkSlateGray" ID="txtTZone" Width="30px"
                                                            runat="server" Text='<%# BindList( Eval("Zone") )%>'
                                                            onblur="$(this).closest('tr').find('.updatet').click();"
                                                            Onkeyup="calculateExpand($(this).closest('table'));">
                                                        </asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="ftb21" runat="server" TargetControlID="txtTZone"
                                                            ValidChars="1234567890.-">
                                                        </asp:FilteredTextBoxExtender>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox Font-Size="8pt" ForeColor="DarkSlateGray" ID="lblTZone" runat="server"
                                                            onfocus="this.blur();" CssClass="texttransparent" Width="30px">
                                                        </asp:TextBox>
                                                    </FooterTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Misc" HeaderStyle-Width="10px">
                                                    <ItemTemplate>
                                                        <asp:TextBox class="text-area-css" Style="border-color: #ccc !important; color: #1c5fb1 !important; height: 20px !important; width: 10px !important"
                                                            Font-Size="8pt" ForeColor="DarkSlateGray" ID="txtTMisc" Width="30px"
                                                            runat="server" Text='<%# BindList( Eval("othere") )%>'
                                                            onblur="$(this).closest('tr').find('.updatet').click();"
                                                            Onkeyup="calculateExpand($(this).closest('table'));">
                                                        </asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="ftb23" runat="server" TargetControlID="txtTMisc"
                                                            ValidChars="1234567890.-">
                                                        </asp:FilteredTextBoxExtender>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox Font-Size="8pt" ForeColor="DarkSlateGray" ID="lblTMisc" runat="server"
                                                            onfocus="this.blur();" CssClass="texttransparent" Width="30px">
                                                        </asp:TextBox>
                                                    </FooterTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Toll" HeaderStyle-Width="10px">
                                                    <ItemTemplate>
                                                        <asp:TextBox class="text-area-css" Style=" border-color: #ccc !important; color: #1c5fb1 !important; height: 20px !important; width: 30px !important"
                                                            Font-Size="8pt" ForeColor="DarkSlateGray" ID="txtPToll" Width="30px"
                                                            runat="server" Text='<%# BindList( Eval("toll") )%>'
                                                            onblur="$(this).closest('tr').find('.updatet').click();"
                                                            nkeyup="calculateExpand($(this).closest('table'));">
                                                        </asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="ftb24" runat="server" TargetControlID="txtPToll"
                                                            ValidChars="1234567890.-">
                                                        </asp:FilteredTextBoxExtender>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox Font-Size="8pt" ForeColor="DarkSlateGray" ID="lblPToll" runat="server"
                                                            onfocus="this.blur();" CssClass="texttransparent" Width="30px">
                                                        </asp:TextBox>
                                                    </FooterTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Extra Exp." HeaderStyle-Width="20px">
                                                    <ItemTemplate>
                                                        <asp:TextBox class="text-area-css" Style="border-color: #ccc !important; color: #1c5fb1 !important; height: 20px !important; width: 30px !important"
                                                            Font-Size="8pt" ForeColor="DarkSlateGray" ID="txtTExtra" Width="30px"
                                                            runat="server" Text='<%# BindList(Eval("Extra")) %>'
                                                            onblur="$(this).closest('tr').find('.updatet').click();"
                                                            Onkeyup="calculateExpand($(this).closest('table'));">
                                                        </asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="ftb25" runat="server" TargetControlID="txtTExtra"
                                                            ValidChars="1234567890.-">
                                                        </asp:FilteredTextBoxExtender>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox Font-Size="8pt" ForeColor="DarkSlateGray" ID="lblTExtra" runat="server"
                                                            onfocus="this.blur();" CssClass="texttransparent" Width="30px">
                                                        </asp:TextBox>
                                                    </FooterTemplate>
                                                </telerik:GridTemplateColumn>
                                                <%--<telerik:GridTemplateColumn>
                                                    <ItemTemplate>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="452px" />
                                                </telerik:GridTemplateColumn>--%>
                                                <telerik:GridTemplateColumn HeaderText="Fringe Benefit" HeaderStyle-Width="25px">
                                                    <ItemTemplate>
                                                        <asp:TextBox class="text-area-css" Style=" border-color: #ccc !important; color: #1c5fb1 !important; height: 20px !important; width: 30px !important"
                                                            Font-Size="8pt" ForeColor="DarkSlateGray" ID="txtTCustomTick2" Width="30px"
                                                            runat="server" Text='<%# Eval("CustomTick2") %>'
                                                            onblur="$(this).closest('tr').find('.updatet').click();"
                                                            Onkeyup="calculateExpand($(this).closest('table'));">
                                                        </asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="ftbct25" runat="server" TargetControlID="txtTCustomTick2"
                                                            ValidChars="1234567890.-">
                                                        </asp:FilteredTextBoxExtender>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox Font-Size="8pt" ForeColor="DarkSlateGray" ID="lblTCustomTick2" runat="server"
                                                            onfocus="this.blur();" CssClass="texttransparent" Width="30px">
                                                        </asp:TextBox>
                                                    </FooterTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Mileage" HeaderStyle-Width="20px">
                                                    <ItemTemplate>
                                                        <asp:TextBox ReadOnly="false" Style="border: 0px solid !important; border-color: white !important; height: 20px !important; width: 30px !important"
                                                            Font-Size="8pt" ForeColor="DarkSlateGray" ID="txtTMileage" Width="30px"
                                                            runat="server" Text='<%# BindList( Eval("Mileage") )%>'
                                                            onblur="$(this).closest('tr').find('.updatet').click();"
                                                            Onkeyup="calculateExpand($(this).closest('table'));">
                                                        </asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="ftb22" runat="server" TargetControlID="txtTMileage"
                                                            ValidChars="1234567890.-">
                                                        </asp:FilteredTextBoxExtender>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox Font-Size="8pt" ForeColor="DarkSlateGray" ID="lblTMileage" runat="server"
                                                            onfocus="this.blur();" CssClass="texttransparent" Width="30px">
                                                        </asp:TextBox>
                                                    </FooterTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Total" HeaderStyle-Width="25px">
                                                    <ItemTemplate>
                                                        <asp:TextBox Style="border: 0 solid white!important" Font-Size="8pt" ForeColor="DarkSlateGray" ID="txtTTotal" onfocus="this.blur();"
                                                            CssClass="texttransparent" Width="30px" runat="server">
                                                        </asp:TextBox>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox Font-Size="8pt" ForeColor="DarkSlateGray" ID="lblTGtotal" runat="server"
                                                            onfocus="this.blur();" CssClass="texttransparent" Width="30px">
                                                        </asp:TextBox>
                                                    </FooterTemplate>
                                                </telerik:GridTemplateColumn>
                                            </Columns>
                                        </telerik:GridTableView>
                                    </DetailTables>

                                    <Columns>
                                        <%--<telerik:GridClientSelectColumn UniqueName="ClientSelectColumn">
                                        </telerik:GridClientSelectColumn>      --%>
                                        <telerik:GridBoundColumn DataField="id" Visible="false"></telerik:GridBoundColumn>
                                        <telerik:GridTemplateColumn AllowFiltering="false" HeaderStyle-Width="30px">
                                            <ItemTemplate>
                                                <asp:Image ID="imgSign" runat="server" Width="16px" ToolTip="Signature" Visible='<%# Eval("signature") != DBNull.Value ? true : false %>'
                                                    ImageUrl="images/Signature.png" />
                                                <asp:Image ID="imgSignature" runat="server" CssClass="hoverGrid shadow transparent roundCorner"
                                                    ImageUrl=' <%# Eval("signature") != DBNull.Value ?( "data:image/png;base64," + Convert.ToBase64String((byte[])Eval("signature"))) : "data:image/png;base64,R0lGODlhAQABAIAAAAAAAP///yH5BAEAAAAALAAAAAABAAEAAAIBRAA7" %>'></asp:Image>
                                                <asp:HoverMenuExtender ID="hmeRes" runat="server" OffsetY="20" OffsetX="20" PopupControlID="imgSignature"
                                                    TargetControlID="imgSign" HoverDelay="250">
                                                </asp:HoverMenuExtender>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderStyle-Width="130px" CurrentFilterFunction="Contains" ShowFilterIcon="false" DataField="Name" SortExpression="Name"
                                            HeaderText="Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblID" runat="server" Visible="false" Text='<%# Eval("id") %>'></asp:Label>
                                                <asp:HyperLink Style="font-size: 12px!important" ID="lblname" runat="server" Text='<%# Eval("Name") %>' Visible="true"
                                                    NavigateUrl='<%#Eval("userid").ToString()!=""? "adduser.aspx?type=1&uid=" + Eval("userid"):"" %>' Target="_blank"></asp:HyperLink>


                                              

                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label runat="server" Text="Total :-"></asp:Label>
                                            </FooterTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderStyle-Width="50px" CurrentFilterFunction="Contains" ShowFilterIcon="false" DataField="Company" SortExpression="Company" UniqueName="Company" HeaderText="Company">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCompany" runat="server" Text='<%# Eval("Company") %>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderStyle-Width="50px" CurrentFilterFunction="Contains" ShowFilterIcon="false" DataField="ref" SortExpression="ref" HeaderText="EmpID">
                                            <ItemTemplate>
                                                <asp:Label Font-Size="9pt" ID="lblEmpID" runat="server" Text='<%# Eval("ref") %>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderStyle-Width="78px" CurrentFilterFunction="Contains" ShowFilterIcon="false" DataField="usertype" SortExpression="usertype" HeaderText="Emp Type">
                                            <ItemTemplate>
                                                <asp:Label Font-Size="9pt" ID="lbltype" runat="server" Text='<%# Eval("usertype") %>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderStyle-Width="50px" HeaderText="Pay" ShowFilterIcon="false" AllowFiltering="false" AllowSorting="false">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkPay" runat="server" Checked='<%# BindList(Eval("total")) == "" ? false : true %>' />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderStyle-Width="70px" CurrentFilterFunction="Contains" ShowFilterIcon="false" DataField="paymethod" SortExpression="paymethod" HeaderText="Method">
                                            <ItemTemplate>
                                                <asp:Label Font-Size="9pt" ID="lblHours" Style="display: none" runat="server" Text='<%# Eval("phour") %>'></asp:Label>
                                                <asp:Label Font-Size="9pt" ID="lblSalary" Style="display: none" runat="server" Text='<%# Eval("salary") %>'></asp:Label>
                                                <asp:Label Font-Size="9pt" ID="lblMlRate" Style="display: none" runat="server" Text='<%# lblProcessed.Visible == true ? Eval("mileage") : Eval("mileagerate") %>'></asp:Label>
                                                <asp:Label Font-Size="9pt" ID="lblHourlyRate" Style="display: none" runat="server" Text='<%# lblProcessed.Visible == true ? Eval("HourlyRate")  : Eval("phour") %>'></asp:Label>
                                                <asp:Label Font-Size="9pt" ID="lblMID" Style="display: none" runat="server" Text='<%# Eval("pmethod") %>'></asp:Label>
                                                <asp:Label Font-Size="9pt" ID="lblMethod" runat="server" Text='<%# Eval("paymethod") %>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderStyle-Width="50px" HeaderText="Custom" AllowFiltering="false" AllowSorting="false">
                                            <ItemTemplate>
                                                <asp:TextBox Font-Size="9pt" ID="txtCustom" Text='<%# BindList( Eval("custom") )%>' onfocus="this.blur();" CssClass="texttransparent textbox" runat="server"
                                                    Width="30px" Style="display: none"></asp:TextBox>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox onfocus="this.blur();" CssClass="texttransparent textbox" Width="30px" ID="lblCustom"
                                                    runat="server" Style="display: none"></asp:TextBox>
                                            </FooterTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderStyle-Width="105px" CurrentFilterFunction="Contains" ShowFilterIcon="false" DataField="HourlyRate" SortExpression="HourlyRate" HeaderText="Hourly Rate($)"
                                            DataType="System.String">
                                            <ItemTemplate>
                                                <asp:TextBox Style="border: 0 solid white!important" Font-Size="8pt" ID="txtHRate" onfocus="this.blur();" CssClass="texttransparent textbox" runat="server"
                                                    Width="50px" Text='<%# BindList(Eval("HourlyRate")) %>'></asp:TextBox>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderStyle-Width="80px" AllowFiltering="false" AllowSorting="false" HeaderText="Rate Job">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkTJobRate" runat="server"
                                                    Checked='<%# Convert.ToBoolean( Eval("CustomTick1") !=DBNull.Value ? Eval("CustomTick1") : 0 ) %>'
                                                    onchange="calculateExpand($(this).closest('table')); $(this).closest('tr').find('.updatet').click();"></asp:CheckBox>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderStyle-Width="90px" AllowFiltering="false" AllowSorting="false" HeaderText="RateJob Hr." FooterStyle-Font-Size="9pt">
                                            <ItemTemplate>
                                                <asp:TextBox class="text-area-css" Style="border: none; border-bottom: 1px solid #9e9e9e!important;  color: #1c5fb1 !important; height: 20px !important; width: 30px !important"
                                                    ID="txtTJobRate" ForeColor="DarkSlateGray" Font-Size="8pt"
                                                    Width="30px" Text='<%#  Eval("CustomTick1") %>' Onkeyup="calculateExpand($(this).closest('table'));"
                                                    onblur="$(this).closest('tr').find('.updatet').click();" runat="server">
                                                </asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="ftbn15" runat="server" TargetControlID="txtTJobRate"
                                                    ValidChars="1234567890.-">
                                                </asp:FilteredTextBoxExtender>
                                                  <%-- <asp:Label ID="txtTJobRate" runat="server"  Text='<%#  Eval("CustomTick1") %>'></asp:Label>--%>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderStyle-Width="50px" CurrentFilterFunction="Contains" ShowFilterIcon="false" DataField="Reg" SortExpression="Reg" HeaderText="Reg" DataType="System.String">
                                            <ItemTemplate>
                                               <%-- <asp:TextBox Style="border: 0 solid white!important" ID="txtReg" Font-Size="8pt" runat="server" Onkeyup="CalculateTimesheet();" Width="30px"
                                                    Text='<%# BindList(Eval("Reg")) %>'></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtReg"
                                                    ValidChars="1234567890.-">
                                                </asp:FilteredTextBoxExtender>--%>
                                                   <asp:Label ID="txtReg" runat="server"  Text='<%# BindList(Eval("Reg")) %>' CssClass="texttransparent"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox onfocus="this.blur();" CssClass="texttransparent textbox" Width="30px" ID="lblReg"
                                                    runat="server"></asp:TextBox>
                                            </FooterTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="OT" HeaderStyle-Width="50px" CurrentFilterFunction="Contains" ShowFilterIcon="false" DataField="OT" SortExpression="OT" DataType="System.String">
                                            <ItemTemplate>
                                               <%-- <asp:TextBox Style="border: 0 solid white!important" ID="txtOT" Font-Size="8pt" runat="server" Onkeyup="CalculateTimesheet();" Width="30px"
                                                    Text='<%# BindList(Eval("OT")) %>'></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtOT"
                                                    ValidChars="1234567890.-">
                                                </asp:FilteredTextBoxExtender>--%>
                                                                   <asp:Label ID="txtOT" runat="server"  Text='<%# BindList(Eval("OT")) %>'  CssClass="texttransparent"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox onfocus="this.blur();" CssClass="texttransparent textbox" Width="30px" ID="lblOT"
                                                    runat="server"></asp:TextBox>
                                            </FooterTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="1.7" HeaderStyle-Width="50px" CurrentFilterFunction="Contains" ShowFilterIcon="false" DataField="NT" SortExpression="NT" DataType="System.String">
                                            <ItemTemplate>
                                              <%--  <asp:TextBox Style="border: 0 solid white!important" ID="txtoneseven" Font-Size="8pt" runat="server" Onkeyup="CalculateTimesheet();" Width="30px"
                                                    Text='<%# BindList(Eval("NT")) %>'></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txtoneseven"
                                                    ValidChars="1234567890.-">
                                                </asp:FilteredTextBoxExtender>--%>
                                                 <asp:Label ID="txtoneseven" runat="server"  Text='<%# BindList(Eval("NT")) %>'  CssClass="texttransparent"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox onfocus="this.blur();" CssClass="texttransparent textbox" Width="30px" ID="lblNT"
                                                    runat="server"></asp:TextBox>
                                            </FooterTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="DT" HeaderStyle-Width="50px" CurrentFilterFunction="Contains" ShowFilterIcon="false" DataField="DT" SortExpression="DT" DataType="System.String">
                                            <ItemTemplate>
                                               <%-- <asp:TextBox Style="border: 0 solid white!important" ID="txtDT" Font-Size="8pt" runat="server" Onkeyup="CalculateTimesheet();" Width="30px"
                                                    Text='<%# BindList(Eval("DT")) %>'></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txtDT"
                                                    ValidChars="1234567890.-">
                                                </asp:FilteredTextBoxExtender>--%>
                                                   <asp:Label ID="txtDT" runat="server"  Text='<%# BindList(Eval("DT")) %>'  CssClass="texttransparent"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox onfocus="this.blur();" CssClass="texttransparent textbox" Width="30px" ID="lblDT"
                                                    runat="server"></asp:TextBox>
                                            </FooterTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="TT" HeaderStyle-Width="50px" CurrentFilterFunction="Contains" ShowFilterIcon="false" DataField="TT" SortExpression="TT" DataType="System.String">
                                            <ItemTemplate>
                                               <%-- <asp:TextBox Style="border: 0 solid white!important" ID="txtPTravel" Font-Size="8pt" runat="server" Onkeyup="CalculateTimesheet();" Width="30px"
                                                    Text='<%# BindList(Eval("TT")) %>'></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" TargetControlID="txtPTravel"
                                                    ValidChars="1234567890.-">
                                                </asp:FilteredTextBoxExtender>--%>
                                                <asp:Label ID="txtPTravel" runat="server"  Text='<%# BindList(Eval("TT")) %>'  CssClass="texttransparent"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox onfocus="this.blur();" CssClass="texttransparent textbox" Width="30px" ID="lblPTravel"
                                                    runat="server"></asp:TextBox>
                                            </FooterTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderStyle-Width="85px" HeaderText="Total Time" AllowFiltering="false" AllowSorting="false">
                                            <ItemTemplate>
                                                <%--<asp:TextBox Style="border: 0 solid white!important" ID="txtTimeTotal" Font-Size="8pt" onfocus="this.blur();" CssClass="texttransparent textbox"
                                                    runat="server" Width="30px"></asp:TextBox>--%>
                                                   <asp:Label ID="txtTimeTotal" runat="server"   CssClass="texttransparent"></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Amount ($)" HeaderStyle-Width="90px" CurrentFilterFunction="Contains" ShowFilterIcon="false" DataField="dollaramount" SortExpression="dollaramount" DataType="System.String">
                                            <ItemTemplate>
                                             <%--   <asp:TextBox Style="border: 0 solid white!important" ID="txtAmount" Font-Size="8pt" runat="server" onfocus="this.blur();" CssClass="texttransparent textbox"
                                                    Width="30px" Text='<%# BindList(Eval("dollaramount")) %>'></asp:TextBox>--%>
                                                 <asp:Label ID="txtAmount" runat="server"  Text='<%# BindList(Eval("dollaramount")) %>'  CssClass="texttransparent"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox onfocus="this.blur();" CssClass="texttransparent textbox" Width="45px" ID="lblAmount"
                                                    runat="server"></asp:TextBox>
                                            </FooterTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Zone($)" HeaderStyle-Width="55px" CurrentFilterFunction="Contains" ShowFilterIcon="false" DataField="zone" SortExpression="zone" DataType="System.String">
                                            <ItemTemplate>
                                               <%-- <asp:TextBox Style="border: 0 solid white!important" ID="txtZone" Font-Size="8pt" Width="30px" Onkeyup="CalculateTimesheet();" runat="server"
                                                    Text='<%# BindList(Eval("zone")) %>'></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" TargetControlID="txtZone"
                                                    ValidChars="1234567890.-">
                                                </asp:FilteredTextBoxExtender>--%>
                                                       <asp:Label ID="txtZone" runat="server"  Text='<%# BindList(Eval("zone")) %>'  CssClass="texttransparent"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox onfocus="this.blur();" CssClass="texttransparent textbox" Width="30px" ID="lblZone"
                                                    runat="server"></asp:TextBox>
                                            </FooterTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Mileage (Miles)" HeaderStyle-Width="110px" CurrentFilterFunction="Contains" ShowFilterIcon="false" DataField="mileage" SortExpression="mileage" DataType="System.String">
                                            <ItemTemplate>
                                               <%-- <asp:TextBox Style="border: 0 solid white!important" ID="txtMileage" Font-Size="8pt" Width="30px" Onkeyup="CalculateTimesheet();" runat="server"
                                                    Text='<%# BindList(Eval("mileage")) %>'></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender11" runat="server" TargetControlID="txtMileage"
                                                    ValidChars="1234567890.-">
                                                </asp:FilteredTextBoxExtender>--%>
                                                  <asp:Label ID="txtMileage" runat="server"  Text='<%# BindList(Eval("mileage")) %>'  CssClass="texttransparent"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox onfocus="this.blur();" CssClass="texttransparent textbox" Width="30px" ID="lblMileage"
                                                    runat="server"></asp:TextBox>
                                            </FooterTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Misc($)" HeaderStyle-Width="50px" CurrentFilterFunction="Contains" ShowFilterIcon="false" DataField="othere" SortExpression="othere" DataType="System.String">
                                            <ItemTemplate>
                                            <%--    <asp:TextBox Style="border: 0 solid white!important" ID="txtMisc" Font-Size="8pt" Width="30px" Onkeyup="CalculateTimesheet();" runat="server"
                                                    Text='<%# BindList(Eval("othere")) %>'></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FTBExtender11" runat="server" TargetControlID="txtMisc"
                                                    ValidChars="1234567890.-">
                                                </asp:FilteredTextBoxExtender>--%>
                                                          <asp:Label ID="txtMisc" runat="server"  Text='<%# BindList(Eval("othere")) %>'  CssClass="texttransparent"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox onfocus="this.blur();" CssClass="texttransparent textbox" Width="30px" ID="lblMisc"
                                                    runat="server"></asp:TextBox>
                                            </FooterTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Toll($)" HeaderStyle-Width="50px" CurrentFilterFunction="Contains" ShowFilterIcon="false" DataField="toll" SortExpression="toll" DataType="System.String">
                                            <ItemTemplate>
                                              <%--  <asp:TextBox Style="border: 0 solid white!important" Font-Size="8pt" ID="txtToll" Width="30px" Onkeyup="CalculateTimesheet();" runat="server"
                                                    Text='<%# BindList(Eval("toll")) %>'></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FTBoxExtender11" runat="server" TargetControlID="txtToll"
                                                    ValidChars="1234567890.-">
                                                </asp:FilteredTextBoxExtender>--%>
                                                 <asp:Label ID="txtToll" runat="server"  Text='<%# BindList(Eval("toll")) %>'  CssClass="texttransparent"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox onfocus="this.blur();" CssClass="texttransparent textbox" Width="30px" ID="lblToll"
                                                    runat="server"></asp:TextBox>
                                            </FooterTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Extra Exp.($)" HeaderStyle-Width="100px" CurrentFilterFunction="Contains" ShowFilterIcon="false" DataField="Extra" SortExpression="Extra" DataType="System.String">
                                            <ItemTemplate>
                                              <%--  <asp:TextBox Style="border: 0 solid white!important" ID="txtExtra" Font-Size="8pt" Width="30px" Onkeyup="CalculateTimesheet();" runat="server"
                                                    Text='<%# BindList(Eval("Extra")) %>'></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender13" runat="server" TargetControlID="txtExtra"
                                                    ValidChars="1234567890.-">
                                                </asp:FilteredTextBoxExtender>--%>

                                                 <asp:Label ID="txtExtra" runat="server"  Text='<%# BindList(Eval("Extra")) %>' CssClass="texttransparent"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox onfocus="this.blur();" CssClass="texttransparent textbox" Width="30px" ID="lblExtra"
                                                    runat="server"></asp:TextBox>
                                            </FooterTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Holiday" HeaderStyle-Width="60px" CurrentFilterFunction="Contains" ShowFilterIcon="false" DataField="holiday" SortExpression="holiday" DataType="System.String">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDlrH" CssClass="money-symbol" Style="display: none;" runat="server" Text="$" Visible="false"></asp:Label>
                                              <%--  <asp:TextBox Style="border: 1px solid !important; border-color: #ccc !important; color: #1c5fb1 !important; height: 20px !important; width: 30px !important"
                                                    Font-Size="8pt" ID="txtHoliday" Onkeyup="CalculateTimesheet();" Width="30px" runat="server"
                                                    Text='<%# BindList(Eval("holiday")) %>'></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" TargetControlID="txtHoliday"
                                                    ValidChars="1234567890.-">
                                                </asp:FilteredTextBoxExtender>--%>

                                                  <asp:Label ID="txtHoliday" runat="server"  Text='<%# BindList(Eval("holiday")) %>' CssClass="borderLabel" ></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox onfocus="this.blur();" CssClass="texttransparent textbox" Width="30px" ID="lblHoliday"
                                                    runat="server" ></asp:TextBox>
                                            </FooterTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Vacation" HeaderStyle-Width="60px" CurrentFilterFunction="Contains" ShowFilterIcon="false" DataField="vacation" SortExpression="vacation" DataType="System.String">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDlrV" CssClass="money-symbol" Style="display: none;" runat="server" Text="$" Visible="false"></asp:Label>
                                               <%-- <asp:TextBox Style="border: 1px solid !important; border-color: #ccc !important; color: #1c5fb1 !important; height: 20px !important; width: 30px !important"
                                                    Font-Size="8pt" ID="txtVacation" Onkeyup="CalculateTimesheet();" Width="30px" runat="server"
                                                    Text='<%# BindList(Eval("vacation")) %>'></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" TargetControlID="txtVacation"
                                                    ValidChars="1234567890.-">
                                                </asp:FilteredTextBoxExtender>--%>

                                                  <asp:Label ID="txtVacation" runat="server"  Text='<%# BindList(Eval("vacation")) %>' CssClass="borderLabel"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox onfocus="this.blur();" CssClass="texttransparent textbox" Width="30px" ID="lblVacation"
                                                    runat="server"></asp:TextBox>
                                            </FooterTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Sick Time" HeaderStyle-Width="80px" CurrentFilterFunction="Contains" ShowFilterIcon="false" DataField="sicktime" SortExpression="sicktime" DataType="System.String">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDlrS" CssClass="money-symbol" Style="display: none;" runat="server" Text="$" Visible="false"></asp:Label>
                                                <%--<asp:TextBox Style="border: 1px solid !important; border-color: #ccc !important; color: #1c5fb1 !important; height: 20px !important; width: 30px !important"
                                                    Font-Size="8pt" ID="txtSick" Onkeyup="CalculateTimesheet();" Width="30px" runat="server"
                                                    Text='<%# BindList(Eval("sicktime")) %>'></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" runat="server" TargetControlID="txtSick"
                                                    ValidChars="1234567890.-">
                                                </asp:FilteredTextBoxExtender>--%>
                                                  <asp:Label ID="txtSick" runat="server"  Text='<%# BindList(Eval("sicktime")) %>' CssClass="borderLabel"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox onfocus="this.blur();" CssClass="texttransparent textbox" Width="30px" ID="lblSickTime"
                                                    runat="server"></asp:TextBox>
                                            </FooterTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Reimb ($)" HeaderStyle-Width="80px" CurrentFilterFunction="Contains" ShowFilterIcon="false" DataField="reimb" SortExpression="reimb" DataType="System.String">
                                            <ItemTemplate>
                                              <%--  <asp:TextBox Style="border: 1px solid !important; border-color: #ccc !important; color: #1c5fb1 !important; height: 20px !important; width: 30px !important"
                                                    Font-Size="8pt" ID="txtReimb" Onkeyup="CalculateTimesheet();" Width="30px" runat="server"
                                                    Text='<%# BindList(Eval("reimb")) %>'></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender10" runat="server" TargetControlID="txtReimb"
                                                    ValidChars="1234567890.-">
                                                </asp:FilteredTextBoxExtender>--%>
                                                  <asp:Label ID="txtReimb" runat="server"  Text='<%# BindList(Eval("reimb")) %>' CssClass="borderLabel"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox onfocus="this.blur();" CssClass="texttransparent textbox" Width="30px" ID="lblReimb"
                                                    runat="server"></asp:TextBox>
                                            </FooterTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Bonus ($)" HeaderStyle-Width="80px" CurrentFilterFunction="Contains" ShowFilterIcon="false" DataField="bonus" SortExpression="bonus" DataType="System.String">
                                            <ItemTemplate>
                                               <%-- <asp:TextBox Style="border: 1px solid !important; border-color: #ccc !important; color: #1c5fb1 !important; height: 20px !important; width: 30px !important"
                                                    Font-Size="8pt" ID="txtBonus" Onkeyup="CalculateTimesheet();" Width="30px" runat="server"
                                                    Text='<%# BindList(Eval("bonus")) %>'></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender12" runat="server" TargetControlID="txtBonus"
                                                    ValidChars="1234567890.-">
                                                </asp:FilteredTextBoxExtender>--%>
                                                <asp:Label ID="txtBonus" runat="server"  Text='<%# BindList(Eval("bonus")) %>' CssClass="borderLabel"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox onfocus="this.blur();" CssClass="texttransparent textbox" Width="30px" ID="lblBonus"
                                                    runat="server"></asp:TextBox>
                                            </FooterTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Salary ($)" HeaderStyle-Width="100px" CurrentFilterFunction="Contains" ShowFilterIcon="false" DataField="salary" SortExpression="salary" DataType="System.String">
                                            <ItemTemplate>
                                                <%--<asp:TextBox Style="border: 1px solid !important; border-color: #ccc !important; color: #1c5fb1 !important; height: 20px !important; width: 30px !important"
                                                    Font-Size="8pt" ID="txtSalary" Onkeyup="CalculateTimesheet();" Text='<%# BindList(Eval("salary")) %>'
                                                    runat="server" Width="30px"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="ftb12" runat="server" TargetControlID="txtSalary"
                                                    ValidChars="1234567890.-">
                                                </asp:FilteredTextBoxExtender>--%>
                                                  <asp:Label ID="txtSalary" runat="server" Text='<%# BindList(Eval("salary")) %>' CssClass="borderLabel test" ></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox onfocus="this.blur();" CssClass="texttransparent textbox"  Width="30px" ID="lblSalary"
                                                    runat="server"></asp:TextBox>
                                            </FooterTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="FringeBenefit" HeaderStyle-Width="100px" AllowFiltering="false" AllowSorting="false">
                                            <ItemTemplate>
                                                <asp:TextBox Style="border: 0 solid white!important" Font-Size="8pt" ID="txtCustomT2" onfocus="this.blur();" CssClass="texttransparent textbox" runat="server" Width="30px"></asp:TextBox>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox onfocus="this.blur();" CssClass="texttransparent textbox" Width="30px" ID="lblCustomT2"
                                                    runat="server"></asp:TextBox>
                                            </FooterTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Total ($)" HeaderStyle-Width="70px" CurrentFilterFunction="Contains" ShowFilterIcon="false" DataField="total" SortExpression="total" DataType="System.String">
                                            <ItemTemplate>
                                             <%--   <asp:TextBox Style="border: 0 solid white!important" Font-Size="8pt" ID="txtTotal" onfocus="this.blur();" CssClass="texttransparent textbox" Width="30px"
                                                    runat="server" Text='<%# BindList(Eval("total")) %>'></asp:TextBox>--%>
                                                  <asp:Label ID="txtTotal" runat="server"  Text='<%# BindList(Eval("total")) %>' CssClass="texttransparent"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox onfocus="this.blur();" CssClass="texttransparent textbox" Width="30px" ID="lblGtotal"
                                                    runat="server"></asp:TextBox>
                                            </FooterTemplate>
                                        </telerik:GridTemplateColumn>
                                    </Columns>
                                </MasterTableView>
                            </telerik:RadGrid>
                        </telerik:RadAjaxPanel>
                    </div>

                </div>
                <table>
                    <tr>
                        <td>

                            <asp:LinkButton ID="lnkSaved" runat="server" Visible="false"
                                Style="float: right">Timesheet Updates for this Period</asp:LinkButton>
                            <asp:LinkButton ID="lnkBack" runat="server" Visible="false" Style="float: right">Back</asp:LinkButton>
                            <asp:LinkButton ID="lnkMerge" runat="server" Visible="false" ToolTip="Update Timesheet with the newly added Tickets and Employees."
                                Style="float: right; margin-right: 20px">Merge Updates</asp:LinkButton>
                        </td>
                    </tr>
                </table>
                <div class="clearfix"></div>
            </div>
        </div>
    </div>

    <div style="display: none">
        <asp:HiddenField ID="hdnCoCode" runat="server" />
        <telerik:RadGrid RenderMode="Auto" ID="gvPayRoll" runat="server" AllowPaging="true"
            PageSize="25" AutoGenerateColumns="false" OnItemDataBound="gvPayRoll_ItemDataBound" OnGridExporting="gvPayRoll_GridExporting">
            <MasterTableView CommandItemDisplay="Top">
                <Columns>
                    <telerik:GridBoundColumn DataField="CoCode" HeaderText="Co Code">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="BatchID" HeaderText="Batch ID" HeaderStyle-Width="130px">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="EmpRef" HeaderText="File #" HeaderStyle-Width="130px">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="[Shift]" HeaderText="Shift" HeaderStyle-Width="130px">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="TempDept" HeaderText="Temp Dept" HeaderStyle-Width="140px">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="RateCode" HeaderText="Rate Code" HeaderStyle-Width="240px">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="RegHours" HeaderText="Reg Hours" HeaderStyle-Width="130px">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="OTHours" HeaderText="O/T Hours" HeaderStyle-Width="130px">
                    </telerik:GridBoundColumn>
               <%--      <telerik:GridBoundColumn DataField="HTHours" HeaderText="H/T Hours" HeaderStyle-Width="130px">
                    </telerik:GridBoundColumn>--%>
                    <telerik:GridBoundColumn DataField="Hours3Code" HeaderText="Hours 3 Code" HeaderStyle-Width="130px">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="Hours3Amount" HeaderText="Hours 3 Amount" HeaderStyle-Width="130px">
                    </telerik:GridBoundColumn>
                </Columns>
            </MasterTableView>
        </telerik:RadGrid>
        <asp:Button ID="btnHiddenExport" runat="server" OnClick="btnHiddenExport_Click" />
    </div>
    <telerik:RadWindowManager ID="RadWindowManager1" runat="server">
        <Windows>
            <telerik:RadWindow ID="CoCodeTypeWindow" Skin="Material" VisibleTitlebar="true" Behaviors="Default" CenterIfModal="true"
                Animation="FlyIn" AnimationDuration="200" RenderMode="Auto" VisibleStatusbar="false" MinWidth="450"
                runat="server" Modal="true" Width="600" Height="200">
                <ContentTemplate>
                    <div class="m-t-15">
                        <div class="form-section-row">
                            <div class="form-section2">
                                <div class="input-field col s12">
                                    <div class="row">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ControlToValidate="txtADP"
                                            Display="None" ErrorMessage="Customer Type Required" SetFocusOnError="True" ValidationGroup="cont"></asp:RequiredFieldValidator>
                                        <asp:ValidatorCalloutExtender ID="RequiredFieldValidator12_ValidatorCalloutExtender"
                                            runat="server" Enabled="True" PopupPosition="BottomLeft" TargetControlID="RequiredFieldValidator12">
                                        </asp:ValidatorCalloutExtender>

                                        <asp:TextBox ID="txtADP" runat="server" CssClass="Contact-search" MaxLength="50"></asp:TextBox>

                                        <label for="txtADP">ADP</label>
                                    </div>
                                </div>
                            </div>

                        </div>
                        <div style="clear: both;"></div>
                        <footer class="footer-css">
                            <div class="btnlinks">
                                <asp:LinkButton ID="lnkCoCodeSave" runat="server" OnClick="lnkCoCodeSave_Click" ValidationGroup="cont">Save</asp:LinkButton>
                            </div>
                        </footer>
                    </div>
                </ContentTemplate>
            </telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>

</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="javascriptHolder" runat="Server">
    <script src="Design/js/moment.js"></script>
    <script src="Design/js/pikaday.js"></script>
    <script type="text/javascript">

        $(document).ready(function () {
            function CloseCoCodeModalWindow() {
            debugger;
            var wnd = $find('<%=CoCodeTypeWindow.ClientID %>');
            wnd.Close();
        }
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
                    $('.divbutton-container').animate({ height: 350 }, 500);


            });

            $(window).scroll(function () {
                if ($(window).scrollTop() >= 0) {
                    $('#divButtons').addClass('fixed-header');
                }
                if ($(window).scrollTop() <= 0) {
                    $('#divButtons').removeClass('fixed-header');
                }
            });
            OnTimesheetGvRendered();
        });
        function confirmUpdate(sender, message) {
            var update = confirm(message);
            if (update) {
                return true;
            } else {
                sender.value = sender.defaultValue;
                return false;
            }
        }

        function removeDummyRows() {
            var all = $(".dummy").map(function () {
                this.remove();
            }).get();
        }


        function calculate(Gridview) {
            CalculateRate("#" + Gridview, 0);
        }

        function calculateExpand(Gridview) {
            CalculateRate(Gridview, 1);
            var gvprev = $(Gridview).closest('tr').prev().find("table");

            $(Gridview).find('tr').each(function (index, value) {
                $(gvprev).find('tr').each(function (secindex, secvalue) {

                    if (index == secindex) {
                        $(secvalue).find('input[id*=txtTCustom]').val($(value).find('input[id*=txtTCustom]').val());
                        $(secvalue).find('input[id*=txtTRate]').val($(value).find('input[id*=txtTRate]').val());
                        $(secvalue).find('input[id*=txtTReg]').val($(value).find('input[id*=txtTReg]').val());
                        $(secvalue).find('input[id*=txtTOT]').val($(value).find('input[id*=txtTOT]').val());
                        $(secvalue).find('input[id*=txtTNT]').val($(value).find('input[id*=txtTNT]').val());
                        $(secvalue).find('input[id*=txtTDT]').val($(value).find('input[id*=txtTDT]').val());
                        $(secvalue).find('input[id*=txtTAmount]').val($(value).find('input[id*=txtTAmount]').val());
                        $(secvalue).find('input[id*=txtTravel]').val($(value).find('input[id*=txtTravel]').val());
                        $(secvalue).find('input[id*=txtTZone]').val($(value).find('input[id*=txtTZone]').val());
                        $(secvalue).find('input[id*=txtTMileage]').val($(value).find('input[id*=txtTMileage]').val());
                        $(secvalue).find('input[id*=txtTExtra]').val($(value).find('input[id*=txtTExtra]').val());
                        $(secvalue).find('input[id*=txtTTotal]').val($(value).find('input[id*=txtTTotal]').val());
                        $(secvalue).find('input[id*=txtTMisc]').val($(value).find('input[id*=txtTMisc]').val());
                        $(secvalue).find('input[id*=txtPToll]').val($(value).find('input[id*=txtPToll]').val());
                        $(secvalue).find('input[id*=txtTJobRate]').val($(value).find('input[id*=txtTJobRate]').val());
                        $(secvalue).find('input[id*=txtTCustomTick2]').val($(value).find('input[id*=txtTCustomTick2]').val());
                        $(secvalue).find('input[id*=chkTJobRate]').prop('checked', $(value).find('input[id*=chkTJobRate]').is(':checked'));

                    }
                });
            });
        }

        function CalculateRate(Gridview, expand) {
            var regtotal = 0;
            var ottotal = 0;
            var nttotal = 0;
            var dttotal = 0;
            var tttotal = 0;
            var tAmtotal = 0;
            var zonetotal = 0;
            var mileagetotal = 0;
            var extratotal = 0;
            var grandtotal = 0;
            var misctotal = 0;
            var tolltotal = 0;
            var Timetotals = 0;
            var CustomTotal = 0;
            var CustomT2Total = 0;

            var trprev = $(Gridview).closest('tr');
            if (expand == 1)
                trprev = $(Gridview).closest('tr').prev();

            var lblMethodID = trprev.find('span[id*=lblMID]');
            var lblHours = trprev.find('span[id*=lblHours]');
            var lblSalary = trprev.find('span[id*=lblSalary]');
            var lblHourlyRate = trprev.find('span[id*=lblHourlyRate]');
            var lblMileageRate = trprev.find('span[id*=lblMlRate]');
            $(Gridview).find('tbody tr').each(function () {
                var $tr = $(this);
                var txtCustom = $tr.find('input[id*=txtTCustom]');
                var txtRate = $tr.find('input[id*=txtTRate]');
                var txtReg = $tr.find('input[id*=txtTReg]');
                var txtOT = $tr.find('input[id*=txtTOT]');
                var txtNT = $tr.find('input[id*=txtTNT]');
                var txtDT = $tr.find('input[id*=txtTDT]');
                var txtTT = $tr.find('input[id*=txtTravel]');
                var txtAmount = $tr.find('input[id*=txtTAmount]');
                var txtZone = $tr.find('input[id*=txtTZone]');
                var txtMileage = $tr.find('input[id*=txtTMileage]');
                var txtExtra = $tr.find('input[id*=txtTExtra]');
                var txtTotal = $tr.find('input[id*=txtTTotal]');
                var txtMisc = $tr.find('input[id*=txtTMisc]');
                var txtToll = $tr.find('input[id*=txtPToll]');
                var txtTimeTotal = $tr.find('input[id*=txtPTimeTotal]');
                var txtJobRate = $tr.find('input[id*=txtTJobRate]');
                var chkTJobRate = $tr.find('input[id*=chkTJobRate]');
                var txtTCustomTick2 = $tr.find('input[id*=txtTCustomTick2]');
               
                var total = 0;
                var TimeAmount = 0;
                var TotalTime = Isnull(txtReg.val()) + Isnull(txtOT.val()) + Isnull(txtNT.val()) + Isnull(txtDT.val()) + Isnull(txtTT.val());
                var OvertimeAmount = 0;
                txtTimeTotal.val(TotalTime.toFixed(2));
                if (!isNaN(parseFloat(txtRate.val()))) {

                    if ($(lblMethodID).text() == '0') {
                        TimeAmount = 0;
                    }
                    else if ($(lblMethodID).text() == '1') {
                        OvertimeAmount = Isnull(txtReg.val()) + (Isnull(txtNT.text()) * 1.7) + (Isnull(txtDT.val()) * 2) + Isnull(txtTT.val()) + (Isnull(txtOT.val()) * 1.5);
                        if (chkTJobRate.is(':checked') == true)
                            TimeAmount = OvertimeAmount * Isnull(txtJobRate.val());
                        else
                            TimeAmount = OvertimeAmount * Isnull(txtRate.val());
                    }
                    else if ($(lblMethodID).text() == '2') {
                        TimeAmount = 0;
                    }
                }
                $(txtAmount).val(TimeAmount.toFixed(2));
                total = TimeAmount + Isnull(txtZone.val()) + (Isnull(txtMileage.val()) * Isnull(lblMileageRate.text())) + Isnull(txtExtra.val()) + Isnull(txtMisc.val()) + Isnull(txtToll.text()) + Isnull(txtCustom.val()) + Isnull(txtTCustomTick2.val());
                $tr.find('input[id*=txtTTotal]').val(total.toFixed(2));

                Timetotals += TotalTime;
                regtotal += Isnull(txtReg.val());
                ottotal += Isnull(txtOT.val());
                nttotal += Isnull(txtNT.val());
                dttotal += Isnull(txtDT.val());
                tttotal += Isnull(txtTT.val());

                if ($(lblMethodID).text() == '1') {
                    tAmtotal += Isnull(TimeAmount);
                }

                zonetotal += Isnull(txtZone.val());
                mileagetotal += Isnull(txtMileage.val());
                extratotal += Isnull(txtExtra.val());
                misctotal += Isnull(txtMisc.val());
                tolltotal += Isnull(txtToll.val());
                grandtotal += total;
                CustomTotal += Isnull(txtCustom.val());
                CustomT2Total += Isnull(txtTCustomTick2.val());

            });

            if ($(lblMethodID).text() == '0') {
                tAmtotal = Isnull(lblSalary.text());
            }
            else if ($(lblMethodID).text() == '2') {
                tAmtotal = Isnull(lblHours.text()) * Isnull(lblHourlyRate.text());
            }


            $(Gridview).find('tfoot tr.rgFooter').each(function () {
                var $tr = $(this);
                var txtCustom = $tr.find('input[id*=lblTCustom]');
                var txtRate = $tr.find('input[id*=lblTRate]');
                var txtReg = $tr.find('input[id*=lblTReg]');
                var txtOT = $tr.find('input[id*=lblTOT]');
                var txtNT = $tr.find('input[id*=lblTNT]');
                var txtDT = $tr.find('input[id*=lblTDT]');
                var txtTT = $tr.find('input[id*=lblTravel]');
                var txtTAmount = $tr.find('input[id*=lblTAmount]');
                var txtZone = $tr.find('input[id*=lblTZone]');
                var txtMileage = $tr.find('input[id*=lblTMileage]');
                var txtExtra = $tr.find('input[id*=lblTExtra]');
                var txtTotal = $tr.find('input[id*=lblTGtotal]');
                var txtMisc = $tr.find('input[id*=lblTMisc]');
                var txtToll = $tr.find('input[id*=lblPToll]');
                var txtCustomTick2 = $tr.find('input[id*=lblTCustomTick2]');
                var txtTimetotal = $tr.find('input[id*=lblPTimeTotal]');

                $(txtCustom).val(CustomTotal.toFixed(2));
                $(txtReg).val(regtotal.toFixed(2));
                $(txtOT).val(ottotal.toFixed(2));
                $(txtNT).val(nttotal.toFixed(2));
                $(txtDT).val(dttotal.toFixed(2));
                $(txtTT).val(tttotal.toFixed(2));
                $(txtTAmount).val(tAmtotal.toFixed(2));
                $(txtZone).val(zonetotal.toFixed(2));
                $(txtMileage).val(mileagetotal.toFixed(2));
                $(txtExtra).val(extratotal.toFixed(2));
                $(txtTotal).val(grandtotal.toFixed(2));
                $(txtMisc).val(misctotal.toFixed(2));
                $(txtToll).val(tolltotal.toFixed(2));
                $(txtCustomTick2).val(CustomT2Total.toFixed(2));
                $(txtTimetotal).val(Timetotals.toFixed(2));

                var txtPCustom = trprev.find('span[id*=txtCustom]');
                var txtPRate = trprev.find('input[id*=txtRate]');
                var txtPReg = trprev.find('span[id*=txtReg]');
                var txtPOT = trprev.find('span[id*=txtOT]');
                var txtPNT = trprev.find('span[id*=txtoneseven]');
                var txtPDT = trprev.find('span[id*=txtDT]');
                var txtPTT = trprev.find('span[id*=txtPTravel]');
                var txtPAmt = trprev.find('span[id*=txtAmount]');
                var txtPZone = trprev.find('span[id*=txtZone]');
                var txtPMileage = trprev.find('span[id*=txtMileage]');
                var txtPExtra = trprev.find('span[id*=txtExtra]');
                var txtPMisc = trprev.find('span[id*=txtMisc]');
                var txtPToll = trprev.find('span[id*=txtToll]');
                var txtPCustomT2 = trprev.find('span[id*=txtCustomT2]');

                $(txtPCustom).text(CustomTotal.toFixed(2));
                if ($(lblMethodID).text() == '2')
                    $(txtPReg).text(lblHours.text());
                else
                    $(txtPReg).text(regtotal.toFixed(2));
                $(txtPOT).text(ottotal.toFixed(2));
                $(txtPNT).text(nttotal.toFixed(2));
                $(txtPDT).text(dttotal.toFixed(2));
                $(txtPTT).text(tttotal.toFixed(2));
                $(txtPAmt).text(tAmtotal.toFixed(2));
                $(txtPZone).text(zonetotal.toFixed(2));
                $(txtPMileage).text(mileagetotal.toFixed(2));
                $(txtPExtra).text(extratotal.toFixed(2));
                $(txtPMisc).text(misctotal.toFixed(2));
                $(txtPToll).text(tolltotal.toFixed(2));
                $(txtPCustomT2).text(CustomT2Total.toFixed(2));

                CalculateTimesheet();
            });

        }

        function CalculateTimesheet() {
            var Gridview = $("#" + '<%= gvTimesheet.ClientID %>');
            var regtotal = 0;
            var ottotal = 0;
            var nttotal = 0;
            var dttotal = 0;
            var tttotal = 0;
            var Amttotal = 0;
            var zonetotal = 0;
            var mileagetotal = 0;
            var extratotal = 0;
            var Holidaytotal = 0;
            var Vacationtotal = 0;
            var Sicktotal = 0;
            var Reimbtotal = 0;
            var Bonustotal = 0;
            var grandtotal = 0;
            var misctotal = 0;
            var tolltotal = 0;
            var customtotal = 0;
            var customT2total = 0;

            $(Gridview).find('table.rgMasterTable tbody tr').each(function () {
                var $tr = $(this);

                var txtCustom = $tr.find('input[id*=txtCustom]');
                var txtReg = $tr.find('span[id*=txtReg]');
                var txtOT = $tr.find('span[id*=txtOT]');
                var txtNT = $tr.find('span[id*=txtoneseven]');
                var txtDT = $tr.find('span[id*=txtDT]');
                var txtTT = $tr.find('span[id*=txtPTravel]');
                var txtAmount = $tr.find('span[id*=txtAmount]');
                var txtTimeTotal = $tr.find('span[id*=txtTimeTotal]');

                var txtZone = $tr.find('span[id*=txtZone]');
                var txtMileage = $tr.find('span[id*=txtMileage]');
                var txtExtra = $tr.find('span[id*=txtExtra]');
                var txtHoliday = $tr.find('span[id*=txtHoliday]');
                var txtVacation = $tr.find('span[id*=txtVacation]');
                var txtSick = $tr.find('span[id*=txtSick]');
                var txtReimb = $tr.find('span[id*=txtReimb]');
                var txtBonus = $tr.find('span[id*=txtBonus]');
                var txtTotal = $tr.find('span[id*=txtTotal]');
                var txtMisc = $tr.find('span[id*=txtMisc]');
                var txtToll = $tr.find('span[id*=txtToll]');
                var txtSalary = $tr.find('span[id*=txtSalary]');
                var txtCustomT2 = $tr.find('span[id*=txtCustomT2]');

                var lblMethodID = $tr.find('span[id*=lblMID]');
                var lblHours = $tr.find('span[id*=lblHours]');
                var lblSalary = $tr.find('span[id*=lblSalary]');
                var lblHourlyRate = $tr.find('span[id*=lblHourlyRate]');
                var lblMlRate = $tr.find('span[id*=lblMlRate]');
             
                $(txtTimeTotal).text((Isnull(txtReg.text()) + Isnull(txtOT.text()) + Isnull(txtNT.text()) + Isnull(txtDT.text()) + Isnull(txtTT.text())).toFixed(2));
               
                if ($(lblMethodID).text() != '1') {
                    $(txtAmount).text(($(txtTimeTotal).text() * Isnull(lblHourlyRate.text())).toFixed(2));
                }

                var TimeAmount = $(txtAmount).text();

                var total = Isnull(TimeAmount) + Isnull(txtZone.text()) + (Isnull(txtMileage.text()) * Isnull(lblMlRate.text())) + Isnull(txtExtra.text()) + (Isnull(txtHoliday.text()) * Isnull(lblHourlyRate.text())) + (Isnull(txtVacation.text()) * Isnull(lblHourlyRate.text())) + (Isnull(txtSick.text()) * Isnull(lblHourlyRate.text())) + Isnull(txtReimb.text()) + Isnull(txtBonus.text()) + Isnull(txtToll.text()) + Isnull(txtMisc.text()) + Isnull(txtSalary.text()) + Isnull(txtCustom.text()) + Isnull(txtCustomT2.text());  // +(Isnull(txtFixedH.val()) * Isnull(lblHourlyRate.text()));
                if ($(lblMethodID).text() == '0')
                    total = Isnull(TimeAmount) + Isnull(txtZone.text()) + (Isnull(txtMileage.text()) * Isnull(lblMlRate.text())) + Isnull(txtExtra.text()) + (Isnull(txtHoliday.text())) + (Isnull(txtVacation.text())) + (Isnull(txtSick.text())) + Isnull(txtReimb.text()) + Isnull(txtBonus.text()) + Isnull(txtToll.text()) + Isnull(txtMisc.text()) + Isnull(txtSalary.text()) + Isnull(txtCustom.text()) + Isnull(txtCustomT2.text()); //+(Isnull(txtFixedH.val()) * Isnull(lblHourlyRate.text()));

                $tr.find('span[id*=txtTotal]').text(total.toFixed(2));

                regtotal += Isnull(txtReg.text());
                ottotal += Isnull(txtOT.text());
                nttotal += Isnull(txtNT.text());
                dttotal += Isnull(txtDT.text());
                tttotal += Isnull(txtTT.text());
                Amttotal += Isnull(TimeAmount);
                zonetotal += Isnull(txtZone.text());
                mileagetotal += Isnull(txtMileage.text());
                extratotal += Isnull(txtExtra.text());
                Holidaytotal += Isnull(txtHoliday.text());
                Vacationtotal += Isnull(txtVacation.text());
                Sicktotal += Isnull(txtSick.text());
                Reimbtotal += Isnull(txtReimb.text());
                Bonustotal += Isnull(txtBonus.text());
                grandtotal += total;
                misctotal += Isnull(txtMisc.text());
                tolltotal += Isnull(txtToll.text());
                customtotal += Isnull(txtCustom.text());
                customT2total += Isnull(txtCustomT2.text());
            });


            $(Gridview).find('table tr.rgFooter').each(function () {
                var $tr = $(this);
                var txtCustom_lb = $tr.find('input[id*=lblCustom]');
                var txtReg_lb = $tr.find('input[id*=lblReg]');
                var txtOT_lb = $tr.find('input[id*=lblOT]');
                var txtNT_lb = $tr.find('input[id*=lblNT]');
                var txtDT_lb = $tr.find('input[id*=lblDT]');
                var txtTT_lb = $tr.find('input[id*=lblPTravel]');
                var txtAmt_lb = $tr.find('input[id*=lblAmount]');
                var txtZone_lb = $tr.find('input[id*=lblZone]');
                var txtMileage_lb = $tr.find('input[id*=lblMileage]');
                var txtExtra_lb = $tr.find('input[id*=lblExtra]');
                var txtHoliday_lb = $tr.find('input[id*=lblHoliday]');
                var txtVacation_lb = $tr.find('input[id*=lblVacation]');
                var txtSick_lb = $tr.find('input[id*=lblSick]');
                var txtReimb_lb = $tr.find('input[id*=lblReimb]');
                var txtBonus_lb = $tr.find('input[id*=lblBonus]');
                var txtTotal_lb = $tr.find('input[id*=lblGtotal]');
                var txtMisc_lb = $tr.find('input[id*=lblMisc]');
                var txtToll_lb = $tr.find('input[id*=lblToll]');
                var txtCustomT2_lb = $tr.find('input[id*=lblCustomT2]');

                $(txtCustom_lb).val(customtotal.toFixed(2));
                $(txtReg_lb).val(regtotal.toFixed(2));
                $(txtOT_lb).val(ottotal.toFixed(2));
                $(txtNT_lb).val(nttotal.toFixed(2));
                $(txtDT_lb).val(dttotal.toFixed(2));
                $(txtTT_lb).val(tttotal.toFixed(2));
                $(txtAmt_lb).val(Amttotal.toFixed(2));
                $(txtZone_lb).val(zonetotal.toFixed(2));
                $(txtMileage_lb).val(mileagetotal.toFixed(2));
                $(txtExtra_lb).val(extratotal.toFixed(2));
                $(txtReimb_lb).val(Reimbtotal.toFixed(2));
                $(txtBonus_lb).val(Bonustotal.toFixed(2));
                $(txtTotal_lb).val(grandtotal.toFixed(2));
                $(txtMisc_lb).val(misctotal.toFixed(2));
                $(txtCustomT2_lb).val(customT2total.toFixed(2));
                $(txtToll_lb).val(tolltotal.toFixed(2));
            });
        }

        function Isnull(value) {
            value = parseFloat(value);
            if (isNaN(value)) {
                value = 0;
            }
            return value;
        }

        function calculateDetailTable(tableId) {
            var detailTable = $("#" + tableId);
            debugger
            calculateExpand(detailTable);
        }

        function OnTimesheetGvRendered() {
            CalculateTimesheet();
        }
       
    </script>
    <script>
        function checkCoCode() {            
            var wnd = $find('<%=CoCodeTypeWindow.ClientID %>');
            var code = $('#<%=hdnCoCode.ClientID%>').val();
            if (code == "") {
                wnd.set_title("Add ADP Code");
                wnd.Show();
                return false;
            } else {
                return true;
            }
        }
        function CloseCoCodeModalWindow() {           
            var wnd = $find('<%=CoCodeTypeWindow.ClientID %>');
            wnd.Close();
            $('#<%=btnHiddenExport.ClientID%>').click();
            }
    </script>
</asp:Content>
