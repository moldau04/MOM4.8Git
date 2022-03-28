<%@ Page Title="" Language="C#" MasterPageFile="~/MOM.master" AutoEventWireup="true" Inherits="AddEquipment" CodeBehind="AddEquipment.aspx.cs" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <!--Grid Control-->
    <link href="Design/css/grid.css" rel="stylesheet" />
    <!--File Upload Control-->
    <link href="Design/js/plugins/dropify/css/dropify.css" type="text/css" rel="stylesheet" media="screen,projection">

    <style>
        div.row.checkbox {
            border-bottom: 1px solid #9e9e9e;
            padding-bottom: 17px;
            margin-bottom: 10px !important;
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
        .collapsible-body input[type=text]{
            height: 1.7rem!important;
            margin: 0 0 7px 0!important;
        }
    </style>
    <%--//error build checkin--%>
    <script type="text/javascript">
        //-----Ticket-------
        function AddTicketClick(hyperlink) {

            var id = document.getElementById('<%= hdnAddeTicket.ClientID%>').value;
            if (id == "Y") { return editticket(); } else {
                noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                return false;
            }
        }

        var changes = 0;
        $(document).on("change", ":input", function () {
            changes = 1;
        });

        function getLocInfo() {
            var LocID = document.getElementById('<%= hdnLocId.ClientID %>').value;
            $.ajax({
                type: "POST",
                url: 'AddEquipment.aspx/FillLocInfo',
                data: '{LocID:"' + LocID + '"}',
                contentType: 'application/json; charset=utf-8',
                success: function (response) {

                    document.getElementById('<%= txtCompany.ClientID %>').value = response.d;
                    Materialize.updateTextFields();
                }
            });
        }

        $(document).ready(function () {


            var queryloc = "";
            function dtaa() {
                this.prefixText = null;
                this.con = document.getElementById('<%= hdnCon.ClientID %>').value;
                this.custID = null;
            }

            if ((window.location.href.indexOf("page=addprospect") === -1)) {
                $("#<%= txtLocation.ClientID %>").autocomplete(
                    {
                        source: function (request, response) {
                            var dtaaa = new dtaa();
                            dtaaa.prefixText = request.term;
                            dtaaa.custID = 0;
                            var hdnpat = document.getElementById('<%= hdnPatientId.ClientID %>').value;
                            if (hdnpat != '') {
                                dtaaa.custID = hdnpat;
                            }

                            queryloc = request.term;
                            $.ajax({
                                type: "POST",
                                contentType: "application/json; charset=utf-8",
                                url: "CustomerAuto.asmx/GetLocation",
                                data: JSON.stringify(dtaaa),
                                dataType: "json",
                                async: true,
                                success: function (data) {
                                    response($.parseJSON(data.d));
                                },
                                error: function (result) {
                                    alert("Due to unexpected errors we were unable to load customers");
                                }
                                //                            error: function(XMLHttpRequest, textStatus, errorThrown) {
                                //                                var err = eval("(" + XMLHttpRequest.responseText + ")");
                                //                                alert(err.Message);
                                //                            }
                            });

                        },
                        select: function (event, ui) {
                            //window.onbeforeunload = null;
                            $("#<%= txtLocation.ClientID %>").val(ui.item.label);
                            $("#<%= hdnLocId.ClientID %>").val(ui.item.value);
                            document.getElementById('<%=btnSelectLoc.ClientID%>').click();
                            return false;
                        },
                        focus: function (event, ui) {
                            $("#<%= txtLocation.ClientID %>").val(ui.item.label);
                            return false;
                        },
                        minLength: 0,
                        delay: 250
                    })
                    .data("ui-autocomplete")._renderItem = function (ul, item) {
                        var result_item = item.label;
                        var result_desc = item.desc;
                        var x = new RegExp('\\b' + queryloc, 'ig'); // notice the escape \ here...            
                        result_item = result_item.replace(x, function (FullMatch, n) {
                            return '<span class="highlight">' + FullMatch + '</span>'
                        });
                        if (result_desc != null) {
                            result_desc = result_desc.replace(x, function (FullMatch, n) {
                                return '<span class="highlight">' + FullMatch + '</span>'
                            });
                        }
                        return $("<li></li>")
                            .data("item.autocomplete", item)
                            .append("<a>" + result_item + ", <span style='color:Gray;'>" + result_desc + "</span></a>")
                            .appendTo(ul);
                    };


                $("#<%= txtLocation.ClientID %>").keyup(function (event) {
                    var hdnLocId = document.getElementById('<%= hdnLocId.ClientID %>');
                    if (document.getElementById('<%= txtLocation.ClientID %>').value == '') {
                        hdnLocId.value = '';
                    }
                });
            }


                ///////////////// Prevent page from leaving before save ////////////////////
                <%--if ($('#<%= btnSubmit.ClientID %>').length) {
                    document.getElementById('<%= btnSubmit.ClientID %>').onclick = function () {
                        if (Page_ClientValidate() == true) {
                            window.btn_clicked = true;
                            return checkLocChange() && ShutdownReasonValidation();
                        }
                    };
                }--%>
        });
        //////////////////////

        function ChangeConfirm(ddl) {
            var totalRows = $("#<%=RadGrid_gvCtemplItems.ClientID %> tr").length;
            if (totalRows > 0) {
                if (!confirm('Are you sure you want to change the template? This will erase the information under the current template.')) {
                    $(ddl).val($('#<%= hdnSelectedVal.ClientID %>').val());
                    return false;
                }
            }
        }

    <%--    window.onbeforeunload = function () {
            if ($("#<%= hdnSaved.ClientID %>").val() == "0") {
                if (!confirm('Changes you made may not be saved.')) {
                    return false;
                }
                else {
                    return true;
                }
            } else {
                return false;
            }
        }; --%>

        function ChkLocation(sender, args) {
            var hdnLocId = document.getElementById('<%= hdnLocId.ClientID %>');
            if (hdnLocId.value == '') {
                args.IsValid = false;
            }
        }


        ////////////////// Called when save form and validation caused on tab 1 //////////////
        <%--  function tabchng() {
            tc = $find("<%=TabContainer1.ClientID%>");
            tc.set_activeTabIndex(0);
        }--%>

      <%--  function switchToTab() {
            var tabContainer = $find("<%= TabContainer1.ClientID %>");
            if (Page_ClientValidate('general') == false) {
                $find("<%= TabContainer1.ClientID %>").set_activeTabIndex(0);
            }
            else if (Page_ClientValidate('rep') == false) {
                $find("<%= TabContainer1.ClientID %>").set_activeTabIndex(2);
            }
        Page_ClientValidate();
    }--%>

        function showModalPopupViaClientCust(lblTicketId, lblComp) {
            //            document.getElementById('<%= iframeCustomer.ClientID %>').src = "addticket.aspx?id=" + lblTicketId + "&comp=" + lblComp;
            //            var modalPopupBehavior = $find('PMPBehaviour');
            //            modalPopupBehavior.show();
            //alert(lblTicketId);

            if ('<%= Session["type"].ToString() %>' == 'c') {
                window.open('Printticket.aspx?id=' + lblTicketId + '&c=' + lblComp + '&pop=1', '_blank');
            }
            else {
                window.open('addticket.aspx?id=' + lblTicketId + '&comp=' + lblComp + '&pop=1', '_blank');
            }
        }

        function linkstyle(internet) {
            alert(internet);
            var style = "cursor:pointer";
            if ('<%= Session["type"].ToString() %>' == 'c') {
                if (internet == "0") { style = "color:black" }
            }
            alert(style);
            return style;
        }

        function hideModalPopup() {
            var modalPopupBehavior = $find('PMPBehaviour');
            modalPopupBehavior.hide();
            document.getElementById('<%= iframeCustomer.ClientID %>').src = "";
        }

        function ClientSidePrint() {
            //debugger
            //var idDiv = '<%=imgQR.ClientID %>';

            var w = 300;
            var h = 300;
            var l = (window.screen.availWidth - w) / 2;
            var t = (window.screen.availHeight - h) / 2;

            var sOption = "toolbar=no,location=no,directories=no,menubar=no,scrollbars=yes,width=" + w + ",height=" + h + ",left=" + l + ",top=" + t;
            // Get the HTML content of the div
            var sDivText = window.document.getElementById('<%=imgQR.ClientID %>').src;
            var unittext = window.document.getElementById('<%= txtEquipID.ClientID %>').value;
            if (window.location.href.indexOf("page=addprospect") === -1) {
                var loctext = window.document.getElementById('<%= txtLocation.ClientID %>').value;
            }
            else {
                var loctext = "";
            }
            // Open a new window
            var objWindow = window.open("", "Print", sOption);
            // Write the div element to the window

            objWindow.document.write("<div style='width:100%;'>");
            objWindow.document.write("<img id='img' src='" + sDivText + "'/>");
            objWindow.document.write("</div><div>" + unittext + " - " + loctext + "</div>");

            objWindow.document.close();

            setTimeout(function () { objWindow.print(); objWindow.close(); }, 1000);

        }

        function checkLocChange() {
            var oldloc = $("#<%= hdnLocPrevious.ClientID %>");
            var newloc = $("#<%= hdnLocId.ClientID %>");
           <%-- var hdnUpdateTicket = $("#<%= hdnUpdateTicket.ClientID %>");--%>
            var ret = true;
            if (oldloc.val() != '') {
                if (oldloc.val() != newloc.val()) {
                    if (getUrlParameter("t") != "c") {
                        ret = confirm('You have changed the equipment location. This will also change the location for completed tickets having this equipment. Do you want to continue?');
                    }

                    //if (ret == false)
                    //    hdnUpdateTicket.val('1');
                    //else
                    //    hdnUpdateTicket.val('');

                }
            }
            return ret;

        }

        ///////////RefreshAddTicket/////////
        function RefreshAddTicket() {

            if (window.opener && !window.opener.closed) {
                if (window.opener.document.getElementById('ctl00_ContentPlaceHolder1_btnSelectLoc'))
                    window.opener.document.getElementById('ctl00_ContentPlaceHolder1_btnSelectLoc').click();
                window.close();

            }
        }


    </script>

    <style type="text/css">
        .RadGrid td .CalendarBlue .ajax__calendar td {
            padding: 0;
        }

        .RadInput .rcSelect {
            height: 31px;
        }
        /*.RadInput .rcSelect a {
          display: inline-block;
       }*/

        /*rad datepicker input font*/
        .RadInput {
            font-family: 'Lato', sans-serif !important;
        }
    </style>

    <%--<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>--%>
    <link rel="stylesheet" href="https://ajax.googleapis.com/ajax/libs/jqueryui/1.12.1/themes/smoothness/jquery-ui.css">
    <script src="https://ajax.googleapis.com/ajax/libs/jqueryui/1.12.1/jquery-ui.min.js"></script>
    <script type="text/javascript">
        //$(function () {
        function pageLoad() {
            $("[id*=gvCtemplItems]").sortable({
                items: 'tr:not(tr:first-child)',
                cursor: 'pointer',
                axis: 'y',
                dropOnEmpty: false,
                start: function (e, ui) {
                    ui.item.addClass("selected");
                },
                stop: function (e, ui) {
                    ui.item.removeClass("selected");
                    updategridindex();
                },
                receive: function (e, ui) {
                    $(this).find("tbody").append(ui.item);
                }
            });

            ///////////// Quick Codes //////////////
            $("#<%=txtRemarks.ClientID%>").keyup(function (event) {
                replaceQuickCodes(event, '<%=txtRemarks.ClientID%>', $("#<%=hdnCon.ClientID%>").val());
            });
        }
        //});
        function updategridindex() {
            var grid1 = 1;
            $('#<%= RadGrid_gvCtemplItems.ClientID %> > tbody  > tr').not(':first').not(':last').each(function () {
                var cat = $(this).find('input[id*="txtRowLine"]');
                cat.val(grid1);
                grid1 = grid1 + 1;
                console.log(cat.val());
            });
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server" Visible="false">
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
        <img src="images/wheel.GIF" alt="Be patient..." class="lodder" />
    </div>
    <telerik:RadAjaxManagerProxy ID="AjaxManagerProxy1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="lnkPopupSave">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="divShutdownReason" />
                    <telerik:AjaxUpdatedControl ControlID="ddlShutdownReason" />
                    <telerik:AjaxUpdatedControl ControlID="hdnShutdownReasonPlanned" />
                    <telerik:AjaxUpdatedControl ControlID="hdnShutdownLongDesc" />
                    <telerik:AjaxUpdatedControl ControlID="hdnEqShutdownStatus" />
                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="ddlShutdownReason">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="lnkEditShutdownReason" />
                    <telerik:AjaxUpdatedControl ControlID="hdnShutdownReasonPlanned" />
                    <telerik:AjaxUpdatedControl ControlID="hdnShutdownLongDesc" />
                    <telerik:AjaxUpdatedControl ControlID="divShutdownReason" />
                    <telerik:AjaxUpdatedControl ControlID="hdnEqShutdownStatus" />

                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="chkShutdown">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="ddlShutdownReason" />
                    <telerik:AjaxUpdatedControl ControlID="divShutdownReason" />
                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="btnSubmit">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="divShutdownReasonDesc" />
                    <telerik:AjaxUpdatedControl ControlID="divShutdownReason" />
                    <telerik:AjaxUpdatedControl ControlID="hdnEqShutdownStatus" />
                    <telerik:AjaxUpdatedControl ControlID="hdnShutdownLongDesc" />
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_gvShutdownLogs" />
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_gvLogs" />

                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="lnkPopupOK">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="divShutdownReasonDesc" />
                    <telerik:AjaxUpdatedControl ControlID="divShutdownReason" />
                    <telerik:AjaxUpdatedControl ControlID="hdnShutdownReasonPlanned" />
                    <telerik:AjaxUpdatedControl ControlID="hdnShutdownLongDesc" />
                    <telerik:AjaxUpdatedControl ControlID="hdnEqShutdownStatus" />
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_gvShutdownLogs" />
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_gvLogs" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnsearch">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_gvRepDetails" />
                    <telerik:AjaxUpdatedControl ControlID="lblRecordCountHist" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManagerProxy>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanelAE" runat="server">
    </telerik:RadAjaxLoadingPanel>
    <telerik:RadWindowManager ID="RadWindowManager1" runat="server">
        <Windows>
            <telerik:RadWindow ID="ShutdownReasonWindow" Skin="Material" VisibleTitlebar="true" Behaviors="Default" CenterIfModal="true"
                Animation="FlyIn" AnimationDuration="200" RenderMode="Auto" VisibleStatusbar="false"
                runat="server" Modal="true" Width="430" Height="225">
                <ContentTemplate>
                    <div style="margin-top: 15px;">
                        <div class="form-section-row">
                            <div class="form-section">
                                <div class="input-field col s12">
                                    <div class="row">
                                        <asp:RequiredFieldValidator ID="rfvPopupShutdownReason" runat="server" ControlToValidate="txtPopupShutdownReason"
                                            Display="None" ErrorMessage="Shut Down Reason Required" Enabled="false" SetFocusOnError="True" ValidationGroup="popupShutdownReason"></asp:RequiredFieldValidator>
                                        <asp:ValidatorCalloutExtender ID="rfvPopupShutdownReason_ValidatorCalloutExtender"
                                            runat="server" Enabled="True" PopupPosition="BottomLeft" TargetControlID="rfvPopupShutdownReason">
                                        </asp:ValidatorCalloutExtender>
                                        <asp:TextBox ID="txtPopupShutdownReason" runat="server" CssClass="Contact-search" MaxLength="50"></asp:TextBox>
                                        <label for="txtPopupShutdownReason">Shut Down Reason</label>
                                    </div>
                                </div>

                                <div class="input-field col s12">
                                    <div class="row">
                                        <asp:CheckBox ID="chkPopupPlanned" runat="server" CssClass="css-checkbox" Text="Planned" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div style="clear: both;"></div>
                        <footer style="float: left; padding-left: 0 !important;">
                            <div class="btnlinks">
                                <asp:LinkButton ID="lnkPopupSave" runat="server" OnClick="lnkPopupSave_Click" ValidationGroup="popupShutdownReason" OnClientClick="EnablePopupValidation(); return true;">Save</asp:LinkButton>
                            </div>
                        </footer>
                    </div>
                </ContentTemplate>
            </telerik:RadWindow>

            <telerik:RadWindow ID="ShutdownReasonDescWindow" Skin="Material" VisibleTitlebar="true" Behaviors="Default" CenterIfModal="true"
                Animation="FlyIn" AnimationDuration="200" RenderMode="Auto" VisibleStatusbar="false"
                runat="server" Modal="true" Width="430" Height="215">
                <ContentTemplate>
                    <div style="margin-top: 15px;">
                        <div class="form-section-row">
                            <div class="form-section">
                                <div class="input-field col s12">
                                    <div class="row" style="margin-bottom: 0;">
                                        <%--<asp:RequiredFieldValidator ID="rfvPopupShutdownReasonDesc" runat="server" ControlToValidate="txtPopupShutdownReasonDesc"
                                            Display="None" ErrorMessage="Shut Down Reason Description Required" Enabled="false" SetFocusOnError="True" ValidationGroup="popupShutdownReasonDesc"></asp:RequiredFieldValidator>
                                        <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender4"
                                            runat="server" Enabled="True" PopupPosition="BottomLeft" TargetControlID="rfvPopupShutdownReasonDesc">
                                        </asp:ValidatorCalloutExtender>--%>
                                        <asp:TextBox ID="txtPopupShutdownReasonDesc" runat="server" CssClass="materialize-textarea" MaxLength="50" TextMode="MultiLine"></asp:TextBox>
                                        <label for="txtPopupShutdownReasonDesc">Shut Down Description</label>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div style="clear: both;"></div>
                        <footer style="float: left; padding-left: 0 !important;">
                            <div class="btnlinks">
                                <%--<asp:LinkButton ID="lnkPopupOK" OnClick="lnkPopupOK_Click" runat="server" ValidationGroup="popupShutdownReasonDesc" OnClientClick="EnablePopupDescValidation(); return true;">OK</asp:LinkButton>--%>
                                <asp:LinkButton ID="lnkPopupOK" OnClick="lnkPopupOK_Click" runat="server" ValidationGroup="popupShutdownReasonDesc" OnClientClick="CloseShutdownReasonDescWindow(); return true;">OK</asp:LinkButton>
                            </div>
                        </footer>
                    </div>
                </ContentTemplate>
            </telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>

    <div class="divbutton-container">
        <div id="divButtons">
            <div id="breadcrumbs-wrapper">
                <header>
                    <div class="container row-color-grey">
                        <telerik:RadAjaxPanel runat="server" ID="RadAP_Equipment_MainButton">
                            <div class="row">
                                <div class="col s12 m12 l12">
                                    <div class="row">
                                        <div class="page-title"><i class="mdi-maps-local-laundry-service"></i>&nbsp;<asp:Label CssClass="title_text" ID="lblHeader" runat="server">Add Equipment</asp:Label></div>
                                        <div class="buttonContainer">
                                            <div class="btnlinks">
                                                <asp:LinkButton ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" OnClientClick="return ShutdownReasonValidation();"
                                                    ValidationGroup="general, rep" ToolTip="Save">Save</asp:LinkButton>
                                            </div>
                                            <div class="btnlinks">
                                                <a class="dropdown-button" id="lnkReport" data-beloworigin="true" href="#!" data-activates="ctl00_ContentPlaceHolder1_dynamicUI" runat="server">Reports </a>
                                                <%--<asp:LinkButton  runat="server" OnClick="lnkReport_Click">Report</asp:LinkButton>--%>
                                            </div>

                                            <ul id="dynamicUI" class="dropdown-content" runat="server">
                                                <li>
                                                    <asp:LinkButton ID="lnkEquipmentCustomDetailReport" runat="server" OnClick="lnkEquipmentCustomDetailReport_Click">Equipment Custom Detail Report</asp:LinkButton>
                                                </li>
                                                <li>
                                                    <asp:LinkButton runat="server" OnClick="lnkReport_Click">Equipment Shut Down Activity Report</asp:LinkButton>
                                                </li>
                                            </ul>
                                        </div>
                                        <div class="btnclosewrap">
                                            <asp:LinkButton ID="lnkClose" ToolTip="Close" runat="server" CausesValidation="false"
                                                OnClick="lnkClose_Click"><i class="mdi-content-clear"></i></asp:LinkButton>
                                        </div>
                                        <div class="rght-content">
                                            <div class="editlabel">
                                                <asp:Label CssClass="title_text_Name" ID="lblEquipName" runat="server"></asp:Label>
                                            </div>

                                        </div>
                                    </div>
                                </div>
                            </div>
                        </telerik:RadAjaxPanel>
                    </div>
                </header>
            </div>

            <div class="container breadcrumbs-bg-custom">
                <div class="row">
                    <div class="col s12 m12 l12">
                        <div class="row">
                            <div class="tblnks">
                                <ul class="anchor-links">
                                    <li id="liGeneral" runat="server"><a href="#accrdgeneralTab">General</a></li>
                                    <li id="liCustom" runat="server"><a href="#accrdcustomTab">Custom</a></li>
                                    <li id="liShutdownLogs" runat="server" style="display: none"><a href="#accrdshutdownlogsTab">Shut Down History</a></li>
                                    <li id="liMCPTemp" runat="server"><a href="#accrdtemplateTab">MCP Template</a></li>
                                    <li id="liMCPHistory" runat="server" style="display: none"><a href="#accrdhistoryTab">MCP History</a></li>
                                    <li id="liTest" runat="server" style="display: none"><a href="#accrdtestTab">Tests</a></li>
                                    <li id="lidocuments" runat="server" style="display: none"><a href="#accrddocuments">Documents</a></li>
                                    <li id="liLogs" runat="server" style="display: none"><a href="#accrdlogs">Logs</a></li>
                                   

                                </ul>
                            </div>
                            <div class="tblnksright" id="pnlNext" runat="server">
                                <div class="nextprev">
                                    <span class="angleicons">
                                        <asp:LinkButton ID="lnkFirst" ToolTip="First" runat="server" CausesValidation="False"
                                            OnClick="lnkFirst_Click"><i class="fa fa-angle-double-left"></i></asp:LinkButton></span>
                                    <span class="angleicons">
                                        <asp:LinkButton ID="lnkPrevious" ToolTip="Previous" runat="server" CausesValidation="False"
                                            OnClick="lnkPrevious_Click"><i class="fa fa-angle-left"></i></asp:LinkButton></span>
                                    <span class="angleicons">
                                        <asp:LinkButton ID="lnkNext" ToolTip="Next" runat="server" CssClass="icon-next" CausesValidation="False"
                                            OnClick="lnkNext_Click">
                                           <i class="fa fa-angle-right"></i> </asp:LinkButton></span>
                                    <span class="angleicons">
                                        <asp:LinkButton ID="lnkLast" ToolTip="Last" runat="server" CssClass="icon-last" CausesValidation="False"
                                            OnClick="lnkLast_Click"><i class="fa fa-angle-double-right"></i></asp:LinkButton></span>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="container accordian-wrap">
        <div class="row">
            <div class="col s12 m12 l12">
                <div class="row">
                    <ul class="collapsible popout collapsible-accordion form-accordion-head" data-collapsible="expandable">
                        <li runat="server" id="pnlEquipments">
                            <div id="accrdgeneralTab" class="collapsible-header accrd active accordian-text-custom "><i class="mdi-social-poll"></i>General</div>
                            <div class="collapsible-body">
                                <div class="form-content-wrap">
                                    <div class="form-content-pd">
                                        <div class="form-section-row">
                                            <telerik:RadAjaxPanel runat="server" ID="RadAP_Equipment_Info">
                                                <div class="section-ttle">Equipment Info.</div>
                                                <%--hidden fields--%>
                                                <input id="hdnPatientId" runat="server" type="hidden" />
                                                <input id="hdnCon" runat="server" type="hidden" />
                                                <input id="hdnLocId" runat="server" type="hidden" />
                                                <input id="hdnSaved" runat="server" value="0" type="hidden" />
                                                <input id="hdnLocPrevious" runat="server" type="hidden" />
                                                <input id="hdnUpdateTicket" runat="server" type="hidden" />
                                                <asp:Button CausesValidation="false" ID="btnSelectLoc" runat="server" Text="Button" Style="display: none;" OnClientClick="getLocInfo();return false" />

                                                <asp:HiddenField ID="hdnShutdownReasonMode" runat="server" />
                                                <asp:HiddenField ID="hdnShutdownLongDesc" runat="server" />
                                                <asp:HiddenField ID="hdnShutdownReasonPlanned" runat="server" />
                                                <asp:HiddenField ID="hdnEqShutdownStatus" runat="server" />
                                                <%--hidden fields--%>

                                                <div class="form-section3">
                                                    <div class="input-field col s12" id="location_container" runat="server">
                                                        <div class="row">
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                                                                ControlToValidate="txtLocation" Display="None" ErrorMessage="Location Name Required"
                                                                SetFocusOnError="True" ValidationGroup="general"></asp:RequiredFieldValidator>
                                                            <asp:ValidatorCalloutExtender ID="RequiredFieldValidator1_ValidatorCalloutExtender"
                                                                runat="server" Enabled="True" TargetControlID="RequiredFieldValidator1">
                                                            </asp:ValidatorCalloutExtender>
                                                            <asp:CustomValidator ID="CustomValidator2" runat="server" ControlToValidate="txtLocation"
                                                                ErrorMessage="Please select the location" ClientValidationFunction="ChkLocation"
                                                                Display="None" SetFocusOnError="True" ValidationGroup="general"></asp:CustomValidator>
                                                            <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender1" runat="server" Enabled="True"
                                                                TargetControlID="CustomValidator2">
                                                            </asp:ValidatorCalloutExtender>
                                                            <asp:TextBox ID="txtLocation" runat="server"
                                                                autocomplete="off" placeholder="Search by location name, phone#, address etc."></asp:TextBox>
                                                            <asp:FilteredTextBoxExtender ID="txtLocation_FilteredTextBoxExtender" runat="server"
                                                                Enabled="False" FilterMode="InvalidChars" InvalidChars="'\" TargetControlID="txtLocation">
                                                            </asp:FilteredTextBoxExtender>
                                                            <label for="txtLocation">Location Name</label>
                                                        </div>
                                                    </div>
                                                    <div class="input-field col s12">
                                                        <div class="row">
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtEquipID"
                                                                Display="None" ErrorMessage="Equipment ID Required" SetFocusOnError="True" ValidationGroup="general"></asp:RequiredFieldValidator>
                                                            <asp:ValidatorCalloutExtender ID="RequiredFieldValidator2_ValidatorCalloutExtender"
                                                                runat="server" Enabled="True" TargetControlID="RequiredFieldValidator2">
                                                            </asp:ValidatorCalloutExtender>
                                                            <asp:TextBox ID="txtEquipID" runat="server" CssClass="form-control" MaxLength="20"></asp:TextBox>
                                                            <label class="active" for="txtEquipID">Equipment ID</label>
                                                        </div>
                                                    </div>
                                                    <div class="input-field col s12" id="dvCompanyPermission" runat="server">
                                                        <div class="row">
                                                            <asp:TextBox ID="txtCompany" runat="server"></asp:TextBox>
                                                            <label for="txtCompany">Company</label>
                                                        </div>
                                                    </div>

                                                    <div class="input-field col s12">
                                                        <div class="row">
                                                            <asp:TextBox ID="txtManuf" runat="server"
                                                                autocomplete="off"></asp:TextBox>
                                                            <label for="txtManuf">Manufacturer</label>
                                                        </div>
                                                    </div>
                                                    <div class="input-field col s12">
                                                        <div class="row">
                                                            <asp:TextBox ID="txtDesc" runat="server"></asp:TextBox>
                                                            <label for="txtDesc">Description</label>
                                                        </div>
                                                    </div>
                                                    <div class="input-field col s12">
                                                        <div class="row">
                                                            <label class="drpdwn-label">
                                                                <asp:Label ID="lblType" runat="server"></asp:Label></label>
                                                            <asp:DropDownList ID="ddlType" runat="server" CssClass="browser-default">
                                                            </asp:DropDownList>

                                                        </div>
                                                    </div>
                                                    <div class="input-field col s12" runat="server" id="divContractType" style="display: none">
                                                        <div class="row">
                                                            <asp:Label ID="lblContractType" CssClass="form-control" runat="server"></asp:Label>
                                                            <label for="lblContractType">Contract Type</label>
                                                        </div>
                                                    </div>
                                                    <div class="input-field col s12">
                                                        <div class="row">
                                                            <label class="drpdwn-label">Service Type</label>
                                                            <asp:DropDownList ID="ddlServiceType" runat="server" class="browser-default">
                                                            </asp:DropDownList>

                                                        </div>
                                                    </div>

                                                    <div class="input-field col s12">
                                                        <div class="row">
                                                            <label class="drpdwn-label">
                                                                <asp:Label ID="lblCategory" runat="server"></asp:Label></label>
                                                            <asp:DropDownList ID="ddlCategory" runat="server" CssClass="browser-default">
                                                            </asp:DropDownList>

                                                        </div>
                                                    </div>
                                                    <div class="input-field col s12">
                                                        <div class="row">

                                                            <label class="drpdwn-label">
                                                                <asp:Label ID="lblBuilding" runat="server"></asp:Label></label>

                                                            <asp:DropDownList ID="ddlBuilding" runat="server" CssClass="browser-default">
                                                            </asp:DropDownList>

                                                        </div>
                                                    </div>
                                                    <div class="input-field col s12">
                                                        <div class="row">

                                                            <label class="drpdwn-label">Status</label>
                                                            <asp:DropDownList ID="rbStatus" runat="server" CssClass="browser-default">
                                                                <asp:ListItem Value="0">Active</asp:ListItem>
                                                                <asp:ListItem Value="1">Inactive</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="form-section3-blank">
                                                    &nbsp;
                                                </div>

                                                <div class="form-section3">
                                                    <div class="input-field col s12">
                                                        <div class="row">
                                                            <label class="drpdwn-label">
                                                                <asp:Label ID="lblClassification" runat="server"></asp:Label></label>

                                                            <asp:DropDownList ID="ddlClassification" runat="server" CssClass="browser-default">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                    <div class="input-field col s12">
                                                        <div class="row checkbox">
                                                            <asp:CheckBox ID="chkShutdown" runat="server" CssClass="css-checkbox" Text="Shut Down" OnCheckedChanged="chkShutdown_Changed" AutoPostBack="true" />
                                                        </div>
                                                    </div>

                                                    <div class="input-field col s12" style="padding: 0;" id="divShutdownReason" runat="server">
                                                        <div class="row col s11" style="padding-left: 0;">
                                                            <label class="drpdwn-label">Shut Down Reason</label>
                                                            <asp:DropDownList ID="ddlShutdownReason" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlShutdownReason_SelectedIndexChanged" CssClass="browser-default"></asp:DropDownList>
                                                        </div>
                                                        <div class="row col s1" style="text-align: center; margin-left: -10px; margin-top: 5px;">
                                                            <div class="btnlinksicon">
                                                                <asp:LinkButton runat="server" ID="lnkAddEditShutdownReason" OnClientClick="OpenShutdownReasonModal();return false" CausesValidation="false" Visible="True"><i class="mdi-social-person-add"></i></asp:LinkButton>
                                                            </div>
                                                        </div>
                                                    </div>

                                                    <div class="input-field col s12 ">
                                                        <div class="row" id="divShutdownReasonDesc" runat="server">
                                                            <asp:TextBox ID="txtShutdownReason" runat="server"
                                                                autocomplete="off" Enabled="false"></asp:TextBox>
                                                            <label for="txtShutdownReason">Shut Down Description</label>
                                                            <%--<asp:RequiredFieldValidator ID="rfvShutdownReason" runat="server" ControlToValidate="txtShutdownReason"
                                                                Display="None" ErrorMessage="Shut Down Description Required" SetFocusOnError="True" ValidationGroup="general"></asp:RequiredFieldValidator>
                                                            <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender3"
                                                                runat="server" Enabled="True" TargetControlID="rfvShutdownReason">
                                                            </asp:ValidatorCalloutExtender>--%>
                                                        </div>
                                                    </div>

                                                    <div class="input-field col s12">
                                                        <div class="row">
                                                            <asp:TextBox ID="txtSerial" runat="server"
                                                                autocomplete="off"></asp:TextBox>
                                                            <label for="txtSerial">Serial #</label>
                                                        </div>
                                                    </div>
                                                    <div class="input-field col s12">
                                                        <div class="row">
                                                            <asp:TextBox ID="txtUnique" runat="server"></asp:TextBox>
                                                            <label for="txtUnique">Unique #</label>
                                                        </div>
                                                    </div>

                                                    <div class="input-field2 col s12">
                                                        <div class="row">
                                                            <label class="date-label">Installed</label>
                                                            <asp:TextBox ID="txtInstalled" CssClass="datepicker_mom" runat="server"></asp:TextBox>

                                                        </div>
                                                    </div>

                                                    <div class="input-field2 col s12">
                                                        <div class="row">
                                                            <label class="date-label">Service Since</label>
                                                            <asp:TextBox ID="txtSince" CssClass="datepicker_mom" runat="server"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="input-field2 col s12">
                                                        <div class="row">
                                                            <label class="date-label">Last Service</label>
                                                            <asp:TextBox ID="txtLast" runat="server" CssClass="datepicker_mom" MaxLength="50"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="input-field col s12">
                                                        <div class="row">
                                                            <asp:TextBox ID="txtPrice" runat="server">00.00</asp:TextBox>
                                                            <asp:MaskedEditExtender ID="txtPrice_MaskedEditExtender" runat="server" Enabled="False"
                                                                Mask="9,999,999.99" TargetControlID="txtPrice" MaskType="Number" DisplayMoney="Left"
                                                                CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat=""
                                                                CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                                                CultureTimePlaceholder="">
                                                            </asp:MaskedEditExtender>
                                                            <asp:FilteredTextBoxExtender ID="txtPrice_FilteredTextBoxExtender" runat="server"
                                                                TargetControlID="txtPrice" ValidChars="0123456789." Enabled="True">
                                                            </asp:FilteredTextBoxExtender>
                                                            <label for="txtPrice">Price</label>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="form-section3-blank">
                                                    &nbsp;
                                                </div>

                                                <div class="form-section3">


                                                    <div class="input-field col s12">
                                                        <div class="row">
                                                            <asp:TextBox ID="txtRemarks" runat="server" TextMode="MultiLine"
                                                                CssClass="materialize-textarea"></asp:TextBox>

                                                            <label for="txtRemarks">Remarks</label>
                                                        </div>
                                                    </div>

                                                    <div class="input-field col s12">
                                                        <div class="row" style="text-align: center;">
                                                            <%-- <img src="Design/images/bar.png" />--%>
                                                            <asp:Panel runat="server" ID="pnlQR" Visible="False">
                                                                <asp:Image ID="imgQR" runat="server" Height="250px" Width="250px" />

                                                            </asp:Panel>

                                                        </div>
                                                    </div>

                                                    <div class="input-field col s12 cntrbtn">
                                                        <div class="row" style="text-align: center;">
                                                            <div class="btnlinks">
                                                                <a onclick="ClientSidePrint();" title="Print QR Code">Print</a>
                                                            </div>
                                                        </div>

                                                    </div>
                                                </div>
                                            </telerik:RadAjaxPanel>
                                        </div>
                                        <div class="cf"></div>
                                    </div>

                                </div>
                            </div>
                        </li>


                        <li id="tpCustom" runat="server">
                            <div id="accrdcustomTab" class="collapsible-header accrd accordian-text-custom"><i class="mdi-action-perm-data-setting"></i>Custom</div>
                            <div class="collapsible-body">
                                <div class="form-content-wrap">
                                    <div class="form-content-pd">
                                        <div class="srchpaneinner">
                                            <div class="srchtitle pl">
                                                Select Template
                                            </div>
                                            <div class="srchinputwrap">

                                                <asp:DropDownList ID="ddlCustTemplate" onchange="ChangeConfirm(this);" CssClass="browser-default selectst"
                                                    runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlCustTemplate_SelectedIndexChanged">
                                                </asp:DropDownList>
                                                <input id="hdnSelectedVal" runat="server" type="hidden" />

                                            </div>
                                        </div>


                                        <div class="grid_container">
                                            <div class="form-section-row mb">
                                                <telerik:RadAjaxManager ID="RadAjaxManager_AEquipment" runat="server">
                                                    <AjaxSettings>
                                                        <telerik:AjaxSetting AjaxControlID="ddlCustTemplate">
                                                            <UpdatedControls>
                                                                <telerik:AjaxUpdatedControl ControlID="RadGrid_gvCtemplItems" />
                                                            </UpdatedControls>
                                                        </telerik:AjaxSetting>


                                                        <telerik:AjaxSetting AjaxControlID="lnkSaveTemplate">
                                                            <UpdatedControls>
                                                                <telerik:AjaxUpdatedControl ControlID="RadGrid_gvTemplateItems" />
                                                            </UpdatedControls>
                                                        </telerik:AjaxSetting>

                                                    </AjaxSettings>
                                                </telerik:RadAjaxManager>
                                                <telerik:RadPersistenceManager ID="RadPersistenceManager1" runat="server">
                                                    <PersistenceSettings>
                                                        <telerik:PersistenceSetting ControlID="RadGrid_gvtests" />
                                                    </PersistenceSettings>

                                                </telerik:RadPersistenceManager>

                                                <div class="RadGrid RadGrid_Material FormGrid">
                                                    <telerik:RadCodeBlock ID="RadCodeBlock6" runat="server">
                                                        <script type="text/javascript">
                                                            function pageLoad() {
                                                                var grid = $find("<%= RadGrid_gvCtemplItems.ClientID %>");
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
                                                    <telerik:RadAjaxPanel ID="RadAjaxPanel_gvCtemplItems" runat="server" LoadingPanelID="RadAjaxLoadingPanelAE" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">
                                                        <telerik:RadGrid RenderMode="Auto" ID="RadGrid_gvCtemplItems" ShowFooter="True" PageSize="50"
                                                            ShowStatusBar="true" runat="server" AllowPaging="True" Width="100%" OnPreRender="RadGrid_gvCtemplItems_PreRender"
                                                            AllowCustomPaging="True" OnNeedDataSource="RadGrid_gvCtemplItems_NeedDataSource">
                                                            <CommandItemStyle />
                                                            <GroupingSettings CaseSensitive="false" />
                                                            <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                                                                <Selecting AllowRowSelect="False"></Selecting>

                                                                <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                                                <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>

                                                            </ClientSettings>
                                                            <MasterTableView AutoGenerateColumns="false" ShowFooter="True">
                                                                <Columns>
                                                                    <telerik:GridTemplateColumn Visible="false" AllowFiltering="false" ShowFilterIcon="false" HeaderText="SNO">
                                                                        <ItemTemplate>

                                                                            <asp:Label ID="lblFormat" runat="server" Text='<%# Eval("formatmom") %>' Visible="false"></asp:Label>
                                                                            <asp:Label ID="lblID" runat="server" Text='<%# Eval("customid") %>' Visible="false"></asp:Label>
                                                                            <asp:Label ID="lblIndex" runat="server" Text='<%# Eval("line") %>'></asp:Label>
                                                                            <asp:Label ID="lblValueh" runat="server" Text='<%# Eval("Value") %>' Visible="false"></asp:Label>

                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn DataField="fdesc" SortExpression="fdesc" AutoPostBackOnFilter="true"
                                                                        HeaderText="Desc" ShowFilterIcon="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblDesc" runat="server" Text='<%# Eval("fdesc") %>'></asp:Label>

                                                                        </ItemTemplate>

                                                                    </telerik:GridTemplateColumn>



                                                                    <telerik:GridTemplateColumn DataField="Value" SortExpression="Value" AutoPostBackOnFilter="true"
                                                                        HeaderText="Value" ShowFilterIcon="false" UniqueName="Value">
                                                                        <ItemTemplate>

                                                                            <asp:DropDownList ID="ddlFormat" runat="server" CssClass="browser-default" Visible="false">
                                                                            </asp:DropDownList>
                                                                            <asp:TextBox ID="lblValue" MaxLength="50" runat="server" Text='<%# Eval("Value") %>'
                                                                                Visible='<%# Eval("formatmom").ToString()=="Dropdown" ? false : true %>'></asp:TextBox>
                                                                            <asp:MaskedEditExtender Enabled='<%# Session["MSM"].ToString() == "TS" ? false : (Eval("formatmom").ToString()=="Short Date" ? true : false) %>'
                                                                                TargetControlID="lblValue" ID="MaskedEditDate" runat="server" Mask="99/99/9999"
                                                                                MaskType="Date" UserDateFormat="MonthDayYear">
                                                                            </asp:MaskedEditExtender>
                                                                            <asp:MaskedEditExtender Enabled='<%# Session["MSM"].ToString() == "TS" ? false : ( Eval("formatmom").ToString()=="Short Time" ? true : false) %>'
                                                                                ID="MaskedEditTime" runat="server" AcceptAMPM="True" Mask="99:99" MaskType="Time"
                                                                                TargetControlID="lblValue">
                                                                            </asp:MaskedEditExtender>
                                                                            <asp:FilteredTextBoxExtender Enabled='<%# Session["MSM"].ToString() == "TS" ? false : ( Eval("formatmom").ToString()=="Currency" ? true : false) %>'
                                                                                ID="FilteredTextBoxExtender1" TargetControlID="lblValue" runat="server" ValidChars="0123456789.-+">
                                                                            </asp:FilteredTextBoxExtender>
                                                                            <asp:FilteredTextBoxExtender Enabled='<%# Session["MSM"].ToString() == "TS" ? false : ( Eval("formatmom").ToString()=="Numeric" ? true : false) %>'
                                                                                ID="FilteredTextBoxExtender2" TargetControlID="lblValue" runat="server" ValidChars="0123456789.-+">
                                                                            </asp:FilteredTextBoxExtender>


                                                                        </ItemTemplate>
                                                                        <FooterTemplate>

                                                                            <asp:Label ID="lblRowCount" runat="server" Text=""></asp:Label>

                                                                        </FooterTemplate>

                                                                    </telerik:GridTemplateColumn>

                                                                    <telerik:GridTemplateColumn DataField="LastUpdated" SortExpression="LastUpdated" AutoPostBackOnFilter="true"
                                                                        HeaderText="Last Update" ShowFilterIcon="false">
                                                                        <ItemTemplate>

                                                                            <asp:Label ID="lblUpdateUser" runat="server" Text='<%# Eval("LastUpdateUser") %>'></asp:Label>
                                                                            <asp:Label ID="lblUpdateDate" runat="server" Text='<%# Eval("LastUpdated", "{0:MM/dd/yy hh:mm tt}") %>'></asp:Label>

                                                                            <asp:HiddenField ID="hdnValue" runat="server" Value='<%# Eval("value") %>' />
                                                                            <asp:HiddenField ID="txtRowLine" runat="server" Value='<%# Eval("orderno") %>'></asp:HiddenField>


                                                                        </ItemTemplate>

                                                                    </telerik:GridTemplateColumn>


                                                                </Columns>
                                                            </MasterTableView>

                                                        </telerik:RadGrid>
                                                    </telerik:RadAjaxPanel>

                                                </div>

                                                <div class="cf"></div>
                                            </div>
                                        </div>
                                        <div style="clear: both;"></div>
                                    </div>
                                </div>
                            </div>
                        </li>

                        <li id="tbShutdownLogs" runat="server" style="display: none">
                            <div id="accrdshutdownlogsTab" class="collapsible-header accrd accordian-text-custom"><i class="mdi-content-content-paste"></i>Shut Down History</div>
                            <div class="collapsible-body">
                                <div class="form-content-wrap">
                                    <div class="form-content-pd">
                                        <div class="grid_container">
                                            <div class="form-section-row mb">
                                                <div class="RadGrid RadGrid_Material">
                                                    <telerik:RadCodeBlock ID="RadCodeBlock8" runat="server">
                                                        <script type="text/javascript">
                                                            function pageLoad() {
                                                                try {
                                                                    var grid = $find("<%= RadGrid_gvShutdownLogs.ClientID %>");
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

                                                    <telerik:RadAjaxPanel ID="RadAjaxPanel_gvShutdownLogs" runat="server" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">
                                                        <telerik:RadGrid RenderMode="Auto" ID="RadGrid_gvShutdownLogs" AllowFilteringByColumn="true" ShowFooter="True" PageSize="50" PagerStyle-AlwaysVisible="true"
                                                            ShowStatusBar="true" runat="server" AllowPaging="True" AllowSorting="true" Width="100%" FilterType="CheckList"
                                                            AllowCustomPaging="True" OnNeedDataSource="RadGrid_gvShutdownLogs_NeedDataSource" OnItemCreated="RadGrid_gvShutdownLogs_ItemCreated">
                                                            <CommandItemStyle />
                                                            <GroupingSettings CaseSensitive="false" />
                                                            <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                                                                <Selecting AllowRowSelect="True"></Selecting>
                                                                <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                                                <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                                            </ClientSettings>
                                                            <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="True" ShowFooter="True" AllowNaturalSort="false" DataKeyNames="id">
                                                                <Columns>
                                                                    <telerik:GridTemplateColumn DataField="created_on" SortExpression="created_on" AutoPostBackOnFilter="true" DataType="System.String"
                                                                        CurrentFilterFunction="Contains" HeaderText="Date" ShowFilterIcon="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lbldate" runat="server" Text='<%# Eval("created_on", "{0:M/d/yyyy}")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn DataField="created_on" SortExpression="created_on" AutoPostBackOnFilter="true" DataType="System.String"
                                                                        CurrentFilterFunction="Contains" HeaderText="Time" ShowFilterIcon="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lbltime" runat="server" Text='<%# Eval("created_on","{0: hh:mm tt}") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn DataField="worker" SortExpression="worker" AutoPostBackOnFilter="true"
                                                                        CurrentFilterFunction="Contains" HeaderText="User" ShowFilterIcon="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblUpdBy" runat="server" Text='<%# Eval("worker") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn DataField="ticket_id" SortExpression="ticket_id" AutoPostBackOnFilter="true"
                                                                        CurrentFilterFunction="Contains" HeaderText="Ticket ID" ShowFilterIcon="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblTicketID" runat="server" Text='<%# Eval("ticket_id") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn DataField="status" SortExpression="status" AutoPostBackOnFilter="true" DataType="System.String"
                                                                        CurrentFilterFunction="Contains" HeaderText="Shut Down" ShowFilterIcon="false" HeaderStyle-Width="305">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblShutdownStatus" runat="server" Text='<%# Eval("status") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn DataField="reason" SortExpression="reason" AutoPostBackOnFilter="true"
                                                                        CurrentFilterFunction="Contains" HeaderText="Reason" ShowFilterIcon="false" HeaderStyle-Width="305">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblReason" runat="server" Text='<%# Eval("reason") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn DataField="longdesc" SortExpression="longdesc" AutoPostBackOnFilter="true"
                                                                        CurrentFilterFunction="Contains" HeaderText="Long Description" ShowFilterIcon="false" HeaderStyle-Width="305">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblLongDesc" runat="server" Text='<%# Eval("longdesc") %>'></asp:Label>
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
                                <div style="clear: both;"></div>
                            </div>
                        </li>

                        <li id="tpREP" runat="server">
                            <div id="accrdtemplateTab" class="collapsible-header accrd accordian-text-custom"><i class="mdi-action-open-in-browser"></i>MCP Template</div>
                            <div class="collapsible-body">
                                <div class="form-content-wrap">
                                    <div class="form-content-pd">

                                        <telerik:RadAjaxPanel ID="RadAjaxPanel_gvTemplateItems" runat="server" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">

                                            <div class="grid_container mb-10">
                                                <div class="form-section-row mb">
                                                    <div class="RadGrid RadGrid_Material FormGrid">
                                                        <telerik:RadCodeBlock ID="RadCodeBlock2" runat="server">
                                                            <script type="text/javascript">
                                                                function pageLoad() {
                                                                    var grid = $find("<%= RadGrid_gvSelectTemplate.ClientID %>");
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
                                                      <telerik:RadAjaxPanel ID="RadAjaxPanel_gvSelectTemplate" runat="server" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd"> 
                                                         <telerik:RadGrid RenderMode="Auto" ID="RadGrid_gvSelectTemplate" ShowFooter="True"
                                                            ShowStatusBar="true" runat="server" AllowPaging="false" Width="100%"
                                                            AllowCustomPaging="false" OnNeedDataSource="RadGrid_gvSelectTemplate_NeedDataSource">
                                                            <CommandItemStyle />
                                                            <GroupingSettings CaseSensitive="false" />
                                                            <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                                                                <Selecting AllowRowSelect="True"></Selecting>

                                                                <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                                                <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>

                                                            </ClientSettings>
                                                            <MasterTableView AutoGenerateColumns="false" ShowFooter="True" DataKeyNames="ID">
                                                                <Columns>

                                                                    <telerik:GridTemplateColumn AutoPostBackOnFilter="true"
                                                                        HeaderText="Last Date" ShowFilterIcon="false" HeaderStyle-Width="200">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txtStartDate" CssClass="datepicker_mom" runat="server"></asp:TextBox>
                                                                         
                                                                        </ItemTemplate>

                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn AutoPostBackOnFilter="true"
                                                                        HeaderText="Template" ShowFilterIcon="false"  >
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="lblRepTempName" OnClick="cbRepTemplate_SelectedIndexChanged"
                                                                                CausesValidation="false" runat="server" Text='<%# Eval("fdesc") %>' OnClientClick="changes = 1;"></asp:LinkButton>
                                                                            <asp:Label ID="lblRepTempId" runat="server" Visible="false" Text='<%# Eval("ID") %>'></asp:Label>

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

                                            <div class="btnlinks">
                                                <asp:LinkButton ID="btnAddNewItem" runat="server" OnClick="btnAddNewItem_Click" ToolTip="Add New"
                                                    CausesValidation="False">Add</asp:LinkButton>
                                                <%--  <a class="modal-trigger" href="#templateModal">Add
                                            </a>--%>
                                            </div>
                                            <div class="btnlinks">

                                                <asp:LinkButton ID="btnDeleteItem" runat="server" CausesValidation="False" ToolTip="Delete"
                                                    OnClick="btnDeleteItem_Click">Delete</asp:LinkButton>
                                                <%-- <a href="#">Delete
                                            </a>--%>
                                            </div>

                                            <div class="grid_container mt-10" id="gvtempgrid" runat="server">
                                                <div class="form-section-row mb">
                                                    <div class="RadGrid RadGrid_Material FormGrid">
                                                        <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
                                                            <script type="text/javascript">
                                                                function pageLoad() {
                                                                    // debugger;
                                                                    var grid = $find("<%= RadGrid_gvTemplateItems.ClientID %>");
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
                                                        <telerik:RadGrid RenderMode="Auto" ID="RadGrid_gvTemplateItems" EnableAjaxSkinRendering="true"
                                                            runat="server" CellSpacing="0">
                                                            <CommandItemStyle />
                                                            <GroupingSettings CaseSensitive="false" />
                                                            <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                                                                <Selecting AllowRowSelect="True"></Selecting>

                                                                <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                                                <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>

                                                            </ClientSettings>
                                                            <MasterTableView AutoGenerateColumns="false" ShowFooter="True" DataKeyNames="fdesc">
                                                                <Columns>

                                                                

                                                                    <telerik:GridTemplateColumn AutoPostBackOnFilter="true"
                                                                        HeaderText=" " ShowFilterIcon="false" HeaderStyle-Width="50">
                                                                        <ItemTemplate>

                                                                            <asp:CheckBox ID="chkSelect" runat="server" />
                                                                        </ItemTemplate>

                                                                    </telerik:GridTemplateColumn>

                                                                    <telerik:GridTemplateColumn DataField="Code" SortExpression="Code" AutoPostBackOnFilter="true"
                                                                        HeaderText="Code" ShowFilterIcon="false" HeaderStyle-Width="130">
                                                                        <ItemTemplate>

                                                                            <asp:Label ID="lblCode" runat="server" Text='<%# Eval("Code") %>'></asp:Label>
                                                                        </ItemTemplate>

                                                                    </telerik:GridTemplateColumn>


                                                                    <telerik:GridTemplateColumn DataField="Section" SortExpression="Section" AutoPostBackOnFilter="true"
                                                                        HeaderText="Section" ShowFilterIcon="false" HeaderStyle-Width="100">
                                                                        <ItemTemplate>

                                                                            <asp:TextBox ID="txtSection" MaxLength="25" runat="server" Text='<%# Eval("section") %>'></asp:TextBox>
                                                                        </ItemTemplate>

                                                                    </telerik:GridTemplateColumn>


                                                                    <telerik:GridTemplateColumn DataField="Name" SortExpression="Name" AutoPostBackOnFilter="true"
                                                                        HeaderText="Name" ShowFilterIcon="false" HeaderStyle-Width="250">
                                                                        <ItemTemplate>


                                                                            <asp:Label ID="lblName" runat="server" Enabled="false" Text='<%# Eval("Name") %>'></asp:Label>
                                                                            <asp:Label ID="lblEquipT" Visible="false" runat="server" Text='<%# Eval("EquipT") %>'></asp:Label>
                                                                            <asp:Label ID="lblID" Visible="false" runat="server" Text='<%# Eval("ID") %>'></asp:Label>
                                                                        </ItemTemplate>

                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn DataField="fdesc" SortExpression="fdesc" AutoPostBackOnFilter="true"
                                                                        HeaderText="Desc" ShowFilterIcon="false" HeaderStyle-Width="200">
                                                                        <ItemTemplate>



                                                                            <asp:TextBox ID="lblDesc" runat="server" Text='<%# Eval("fdesc") %>'></asp:TextBox>
                                                                            <asp:RequiredFieldValidator ID="rfvDesc" runat="server" ValidationGroup="rep" ControlToValidate="lblDesc"
                                                                                Display="Dynamic" ErrorMessage="*" SetFocusOnError="True" Font-Bold="True" Font-Size="Larger"></asp:RequiredFieldValidator>

                                                                        </ItemTemplate>

                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn DataField="lastdate" SortExpression="lastdate" AutoPostBackOnFilter="true"
                                                                        HeaderText="Last Date" ShowFilterIcon="false" HeaderStyle-Width="180">
                                                                        <ItemTemplate>



                                                                            <telerik:RadDatePicker RenderMode="Auto"
                                                                                ID="RadDatePickerLastDate" MinDate="1900/1/1" runat="server" Width="99%"
                                                                                DatePopupButton="false" AutoPostBack="true"
                                                                                DateInput-ToolTip="Last Date"
                                                                                DateInput-DateFormat="MM/dd/yyyy"
                                                                                OnSelectedDateChanged="txtLdate_TextChanged"
                                                                                DbSelectedDate='<%# Bind("lastdate") %>'>
                                                                            </telerik:RadDatePicker>
                                                                        </ItemTemplate>

                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn DataField="Frequency" SortExpression="Frequency" AutoPostBackOnFilter="true"
                                                                        HeaderText="Freq." ShowFilterIcon="false" HeaderStyle-Width="150">
                                                                        <ItemTemplate>
                                                                            <asp:DropDownList ID="ddlFreq" runat="server" SelectedValue='<%# Eval("Frequency") %>'
                                                                                AutoPostBack="true" OnSelectedIndexChanged="ddlFreq_SelectedIndexChanged" CssClass="browser-default ">
                                                                                <asp:ListItem Value="-1">-Select-</asp:ListItem>
                                                                                <asp:ListItem Value="0">Daily</asp:ListItem>
                                                                                <asp:ListItem Value="1">Weekly</asp:ListItem>
                                                                                <asp:ListItem Value="2">Bi-Weekly</asp:ListItem>
                                                                                <asp:ListItem Value="3">Monthly</asp:ListItem>
                                                                                <asp:ListItem Value="4">Bi-Monthly</asp:ListItem>
                                                                                <asp:ListItem Value="5">Quarterly</asp:ListItem>
                                                                                <asp:ListItem Value="6">Semi-Annually </asp:ListItem>
                                                                                <asp:ListItem Value="7">Annually</asp:ListItem>
                                                                                <asp:ListItem Value="8">One Time</asp:ListItem>
                                                                                <asp:ListItem Value="9">3 Times a Year</asp:ListItem>
                                                                                <asp:ListItem Value="10">Every 2 Year</asp:ListItem>
                                                                                <asp:ListItem Value="11">Every 3 Year</asp:ListItem>
                                                                                <asp:ListItem Value="12">Every 5 Year</asp:ListItem>
                                                                                <asp:ListItem Value="13">Every 7 Year</asp:ListItem>
                                                                                <asp:ListItem Value="14">On-Demand</asp:ListItem>
                                                                            </asp:DropDownList>

                                                                        </ItemTemplate>

                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn DataField="NextDateDue" SortExpression="NextDateDue" AutoPostBackOnFilter="true"
                                                                        HeaderText="Next Due Date" ShowFilterIcon="false" HeaderStyle-Width="130">
                                                                        <ItemTemplate>

                                                                            <asp:TextBox ID="txtDuedate" runat="server" CssClass="datepicker_mom" Text='<%#Eval("NextDateDue").ToString().Length>0? Convert.ToDateTime(Eval("NextDateDue")).ToShortDateString():"" %>'></asp:TextBox>

                                                                            <asp:RequiredFieldValidator ID="rfvNextDate" runat="server" ValidationGroup="rep"
                                                                                ControlToValidate="txtDuedate" Display="Dynamic" ErrorMessage="*" SetFocusOnError="True"
                                                                                Font-Bold="True" Font-Size="Larger"></asp:RequiredFieldValidator>
                                                                        </ItemTemplate>

                                                                    </telerik:GridTemplateColumn>

                                                                    <telerik:GridTemplateColumn DataField="Notes" SortExpression="Notes" AutoPostBackOnFilter="true"
                                                                        HeaderText="Notes" ShowFilterIcon="false" HeaderStyle-Width="150">
                                                                        <ItemTemplate>


                                                                            <asp:TextBox ID="txtNotes" TextMode="MultiLine" runat="server" Text='<%# Eval("Notes") %>' Width="200px"></asp:TextBox>

                                                                        </ItemTemplate>

                                                                    </telerik:GridTemplateColumn>


                                                                </Columns>
                                                            </MasterTableView>
                                                        </telerik:RadGrid>

                                                    </div>
                                                </div>
                                            </div>

                                            <div class="cf"></div>

                                        </telerik:RadAjaxPanel>
                                    </div>
                                </div>

                                <div style="clear: both;"></div>
                            </div>
                        </li>

                        <li id="tpnlREPH" runat="server" style="display: none">
                            <div id="accrdhistoryTab" class="collapsible-header accrd accordian-text-custom"><i class="mdi-action-cached"></i>MCP History</div>
                            <div class="collapsible-body">
                                <div class="form-content-wrap">
                                    <div class="form-content-pd">
                                        <div class="srchpaneinner">
                                            <div class="srchtitle pl">
                                                Search
                                            </div>
                                            <div class="srchinputwrap">
                                                <asp:DropDownList ID="ddlSearch" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlSearch_SelectedIndexChanged"
                                                    CssClass="browser-default selectst">
                                                    <asp:ListItem Value=" ">--Select--</asp:ListItem>
                                                    <asp:ListItem Value="rd.ticketID">Ticket #</asp:ListItem>
                                                    <asp:ListItem Value="fwork">Worker</asp:ListItem>
                                                    <asp:ListItem Value="rd.Code">Code</asp:ListItem>
                                                    <asp:ListItem Value="eti.fDesc">Desc</asp:ListItem>
                                                    <asp:ListItem Value="template">Template</asp:ListItem>
                                                    <asp:ListItem Value="eti.frequency">Frequency</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="srchinputwrap">
                                                <asp:TextBox ID="txtSearch" runat="server" CssClass="srchcstm" placeholder="Search..."></asp:TextBox>
                                                <asp:TextBox ID="txtCodeSearch" runat="server" CssClass="srchcstm"
                                                    Visible="False" autocomplete="off"></asp:TextBox>
                                                <asp:AutoCompleteExtender runat="server" Enabled="True" ServicePath="CustomerAuto.asmx" TargetControlID="txtCodeSearch"
                                                    EnableCaching="False" ServiceMethod="GetCodes11" UseContextKey="True" MinimumPrefixLength="0"
                                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListItemCssClass="autocomplete_listItem"
                                                    CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" ID="AutoCompleteExtender"
                                                    DelimiterCharacters="" CompletionInterval="250">
                                                </asp:AutoCompleteExtender>
                                                <asp:DropDownList ID="ddlTemplate" runat="server" Visible="false" CssClass="browser-default selectst">
                                                </asp:DropDownList>
                                                <asp:DropDownList ID="ddlFreq" runat="server" Visible="false" CssClass="browser-default selectst">
                                                    <asp:ListItem Value="0">Daily</asp:ListItem>
                                                    <asp:ListItem Value="1">Weekly</asp:ListItem>
                                                    <asp:ListItem Value="2">Bi-Weekly</asp:ListItem>
                                                    <asp:ListItem Value="3">Monthly</asp:ListItem>
                                                    <asp:ListItem Value="4">Bi-Monthly</asp:ListItem>
                                                    <asp:ListItem Value="5">Quarterly</asp:ListItem>
                                                    <asp:ListItem Value="6">Semi-Annually </asp:ListItem>
                                                    <asp:ListItem Value="7">Annually</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="srchpaneinner">
                                            <div class="srchtitle pl">
                                                Date
                                            </div>
                                            <div class="srchinputwrap">
                                                <asp:DropDownList ID="ddlDates" runat="server" CssClass="browser-default selectst">
                                                    <asp:ListItem Value="0">Last Date</asp:ListItem>
                                                    <asp:ListItem Value="1">Next Due Date</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="srchinputwrap">
                                                <%--<label for="txtfromDate">From</label>--%>
                                                <asp:TextBox ID="txtfromDate" runat="server" CssClass="srchcstm"></asp:TextBox>
                                                <asp:CalendarExtender ID="txtfromDate_CalendarExtender" runat="server" Enabled="True"
                                                    TargetControlID="txtfromDate">
                                                </asp:CalendarExtender>

                                            </div>
                                            <div class="srchinputwrap">
                                                <%--<label for="txtToDate">To</label>--%>
                                                <asp:TextBox ID="txtToDate" runat="server" CssClass="srchcstm"></asp:TextBox>
                                                <asp:CalendarExtender ID="txtToDate_CalendarExtender" runat="server" Enabled="True"
                                                    TargetControlID="txtToDate">
                                                </asp:CalendarExtender>

                                            </div>
                                            <div class="srchinputwrap">
                                                <div class="btnlinksicon">
                                                    <asp:LinkButton ID="btnSearch" runat="server" CausesValidation="False"
                                                        OnClick="btnSearch_Click" ToolTip="Search"><i class="mdi-action-search"></i></asp:LinkButton>




                                                </div>
                                            </div>
                                            <div class="btnlinksicon">
                                                <asp:Label ID="lblRecordCountHist" runat="server"></asp:Label>

                                            </div>
                                            <div class="btnlinks">

                                                <asp:LinkButton ID="lnkclear" runat="server" OnClick="lnkclear_Click">Clear</asp:LinkButton>
                                            </div>
                                            <div class="btnlinksicon">
                                                <asp:LinkButton ID="lnkPrintMCP" runat="server" OnClick="lnkPrintMCP_Click" ToolTip="Print Maintenance Control Plan Report"><i class="fa fa-print"></i></asp:LinkButton>
                                            </div>
                                        </div>
                                        <div class="srchpaneinner">
                                            <div class="btnlinks">
                                                <asp:HyperLink ID="lnkAddTicket" OnClick='return AddTicketClick(this)' runat="server" ToolTip="Add New"
                                                    Target="_blank">Add</asp:HyperLink>
                                            </div>
                                        </div>

                                        <div class="grid_container">
                                            <div class="form-section-row mb">
                                                <div class="RadGrid RadGrid_Material FormGrid">
                                                    <telerik:RadCodeBlock ID="RadCodeBlock3" runat="server">
                                                        <script type="text/javascript">
                                                            function pageLoad() {
                                                                var grid = $find("<%= RadGrid_gvRepDetails.ClientID %>");
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
                                                    <telerik:RadAjaxPanel ID="RadAjaxPanel_gvRepDetails" runat="server" LoadingPanelID="RadAjaxLoadingPanelAE" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">


                                                        <telerik:RadGrid RenderMode="Auto" ID="RadGrid_gvRepDetails" AllowFilteringByColumn="true" ShowFooter="True" PageSize="50" PagerStyle-AlwaysVisible="true"
                                                            ShowStatusBar="true" runat="server" AllowPaging="True" AllowSorting="true" Width="100%" FilterType="CheckList" OnPreRender="RadGrid_gvRepDetails_PreRender"
                                                            AllowCustomPaging="True" OnNeedDataSource="RadGrid_gvRepDetails_NeedDataSource">
                                                            <CommandItemStyle />
                                                            <GroupingSettings CaseSensitive="false" />
                                                            <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                                                                <Selecting AllowRowSelect="True"></Selecting>

                                                                <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                                                <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                                                <%--<Resizing ResizeGridOnColumnResize="True" AllowColumnResize="True"></Resizing>--%>
                                                            </ClientSettings>
                                                            <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="True" ShowFooter="True" DataKeyNames="ticketid">
                                                                <Columns>

                                                                    <telerik:GridTemplateColumn DataField="ticketid" SortExpression="ticketid" AutoPostBackOnFilter="true"
                                                                        CurrentFilterFunction="Contains" HeaderText="Ticket #" ShowFilterIcon="false" HeaderStyle-Width="305">
                                                                        <ItemTemplate>
                                                                            <a style="cursor: pointer"
                                                                                onclick='javascript:showModalPopupViaClientCust(<%# Eval("ticketid") %>,<%# Eval("comp") %>);'>
                                                                                <asp:Label ID="lblTicketId" runat="server" Text='<%# Bind("ticketid") %>'></asp:Label>
                                                                            </a>
                                                                        </ItemTemplate>

                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn DataField="fwork" SortExpression="fwork" AutoPostBackOnFilter="true"
                                                                        CurrentFilterFunction="Contains" HeaderText="Worker" ShowFilterIcon="false" HeaderStyle-Width="305">
                                                                        <ItemTemplate>

                                                                            <asp:Label ID="lblWork" runat="server" Text='<%# Bind("fwork") %>'></asp:Label>

                                                                        </ItemTemplate>

                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn DataField="Code" SortExpression="Code" AutoPostBackOnFilter="true"
                                                                        CurrentFilterFunction="Contains" HeaderText="Code" ShowFilterIcon="false" HeaderStyle-Width="305">
                                                                        <ItemTemplate>

                                                                            <asp:Label ID="lblCode" runat="server" Text='<%# Bind("Code") %>'></asp:Label>

                                                                        </ItemTemplate>

                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn DataField="Section" SortExpression="Section" AutoPostBackOnFilter="true"
                                                                        CurrentFilterFunction="Contains" HeaderText="Section" ShowFilterIcon="false" HeaderStyle-Width="305">
                                                                        <ItemTemplate>

                                                                            <asp:Label ID="lblSection" runat="server" Text='<%# Bind("Section") %>'></asp:Label>
                                                                        </ItemTemplate>

                                                                    </telerik:GridTemplateColumn>

                                                                    <telerik:GridTemplateColumn DataField="Template" SortExpression="Template" AutoPostBackOnFilter="true"
                                                                        CurrentFilterFunction="Contains" HeaderText="Template" ShowFilterIcon="false" HeaderStyle-Width="305">
                                                                        <ItemTemplate>


                                                                            <asp:Label ID="lblTemplate" runat="server" Text='<%# Bind("template") %>'></asp:Label>
                                                                        </ItemTemplate>

                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn DataField="fDesc" SortExpression="fDesc" AutoPostBackOnFilter="true"
                                                                        CurrentFilterFunction="Contains" HeaderText="Desc" ShowFilterIcon="false" HeaderStyle-Width="305">
                                                                        <ItemTemplate>



                                                                            <asp:Label ID="lblDesc" runat="server" Text='<%# Bind("fDesc") %>'></asp:Label>
                                                                        </ItemTemplate>

                                                                    </telerik:GridTemplateColumn>

                                                                    <telerik:GridTemplateColumn DataField="freq" SortExpression="freq" AutoPostBackOnFilter="true"
                                                                        CurrentFilterFunction="Contains" HeaderText="Frequency" ShowFilterIcon="false" HeaderStyle-Width="305">
                                                                        <ItemTemplate>

                                                                            <asp:Label ID="lblFreq" runat="server" Text='<%# Bind("freq") %>'></asp:Label>


                                                                        </ItemTemplate>

                                                                    </telerik:GridTemplateColumn>

                                                                    <telerik:GridTemplateColumn DataField="comments" SortExpression="comments" AutoPostBackOnFilter="true"
                                                                        CurrentFilterFunction="Contains" HeaderText="Comments" ShowFilterIcon="false" HeaderStyle-Width="305">
                                                                        <ItemTemplate>


                                                                            <asp:Label ID="lblComments" runat="server" Text='<%# Bind("comment") %>'></asp:Label>


                                                                        </ItemTemplate>

                                                                    </telerik:GridTemplateColumn>


                                                                    <telerik:GridTemplateColumn DataField="Status" SortExpression="Status" AutoPostBackOnFilter="true"
                                                                        CurrentFilterFunction="Contains" HeaderText="Status" ShowFilterIcon="false" HeaderStyle-Width="305">
                                                                        <ItemTemplate>



                                                                            <asp:Label ID="lblStatus" runat="server" Text='<%# Bind("Status") %>'></asp:Label>


                                                                        </ItemTemplate>

                                                                    </telerik:GridTemplateColumn>

                                                                    <telerik:GridTemplateColumn DataField="LastDate" SortExpression="LastDate" AutoPostBackOnFilter="true"
                                                                        CurrentFilterFunction="Contains" HeaderText="Last Date" ShowFilterIcon="false" HeaderStyle-Width="305">
                                                                        <ItemTemplate>




                                                                            <asp:Label ID="lblLastdate" runat="server" Text='<%# Eval("LastDate", "{0:MM/dd/yyyy}")%>'></asp:Label>


                                                                        </ItemTemplate>

                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn DataField="NextDateDue" SortExpression="NextDateDue" AutoPostBackOnFilter="true"
                                                                        CurrentFilterFunction="Contains" HeaderText="Next Due Date " ShowFilterIcon="false" HeaderStyle-Width="305">
                                                                        <ItemTemplate>





                                                                            <asp:Label ID="lblNextDateDue" runat="server" Text='<%# Eval("NextDateDue", "{0:MM/dd/yyyy}")%>'></asp:Label>


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
                                    <div style="clear: both;"></div>
                                </div>
                            </div>
                        </li>

                        <li id="tbTests" runat="server" style="display: none">
                            <div id="accrdtestTab" class="collapsible-header accrd accordian-text-custom"><i class="mdi-content-content-paste"></i>Tests</div>
                            <div class="collapsible-body">
                                <div class="form-content-wrap">
                                    <div class="form-content-pd">
                                        <div class="btncontainer" id="divTestButton" runat="server">
                                            <div class="btnlinks">
                                                <asp:LinkButton ID="lnkAddTest" runat="server" OnClick="lnkAddTest_Click" ToolTip="Add New"
                                                    CausesValidation="False">Add</asp:LinkButton>
                                            </div>
                                            <div class="btnlinks">
                                                <asp:LinkButton ID="lnkDelTest" runat="server" CausesValidation="False" ToolTip="Delete"
                                                    OnClick="lnkDelTest_Click">Delete</asp:LinkButton>
                                            </div>
                                        </div>


                                        <div class="grid_container">
                                            <div class="form-section-row mb">
                                                <div class="RadGrid RadGrid_Material FormGrid">
                                                    <telerik:RadCodeBlock ID="RadCodeBlock4" runat="server">
                                                        <script type="text/javascript">
                                                            function pageLoad() {
                                                                var grid = $find("<%= RadGrid_gvtests.ClientID %>");
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
                                                    <telerik:RadAjaxPanel ID="RadAjaxPanel_gvtests" runat="server" LoadingPanelID="RadAjaxLoadingPanelAE" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">


                                                        <telerik:RadGrid RenderMode="Auto" ID="RadGrid_gvtests" AllowFilteringByColumn="true" ShowFooter="True" PageSize="50" PagerStyle-AlwaysVisible="true"
                                                            ShowStatusBar="true" runat="server" AllowPaging="True" AllowSorting="true" Width="100%" FilterType="CheckList" OnPreRender="RadGrid_gvtests_PreRender"
                                                            AllowCustomPaging="True" OnNeedDataSource="RadGrid_gvtests_NeedDataSource">
                                                            <CommandItemStyle />
                                                            <GroupingSettings CaseSensitive="false" />
                                                            <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                                                                <Selecting AllowRowSelect="True"></Selecting>

                                                                <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                                                <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                                                <%--<Resizing ResizeGridOnColumnResize="True" AllowColumnResize="True"></Resizing>--%>
                                                            </ClientSettings>
                                                            <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="True" ShowFooter="True" DataKeyNames="idUnit">
                                                                <Columns>
                                                                    <telerik:GridTemplateColumn Visible="false" AllowFiltering="false" ShowFilterIcon="false">
                                                                        <ItemTemplate>

                                                                            <asp:HiddenField ID="hdnidUnit" runat="server" Value='<%# Eval("idUnit") %>' />
                                                                            <asp:HiddenField ID="hdnidTestItem" runat="server" Value='<%# Eval("idTestItem") %>' />
                                                                            <asp:HiddenField ID="hdnTestYear" runat="server" Value='<%# Eval("TestYear") %>' />
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridClientSelectColumn UniqueName="chkSelect" HeaderStyle-Width="28">
                                                                    </telerik:GridClientSelectColumn>



                                                                    <telerik:GridTemplateColumn DataField="Name" SortExpression="Name" AutoPostBackOnFilter="true"
                                                                        CurrentFilterFunction="Contains" HeaderText="TestName" ShowFilterIcon="false" HeaderStyle-Width="100">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblName" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>

                                                                     <telerik:GridTemplateColumn DataField="TestYear" SortExpression="TestYear" AutoPostBackOnFilter="true"
                                                                        CurrentFilterFunction="Contains" HeaderText="TestYear" ShowFilterIcon="false" HeaderStyle-Width="100">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblTestYear" runat="server" Text='<%# Eval("TestYear") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
 
                                                                    <telerik:GridTemplateColumn DataField="Last" SortExpression="Last" AutoPostBackOnFilter="true"
                                                                        CurrentFilterFunction="Contains" HeaderText="Last" ShowFilterIcon="false" HeaderStyle-Width="100">
                                                                        <ItemTemplate>

                                                                            <asp:Label ID="lblLast" runat="server" Text='<%# Eval("Last", "{0:MM/dd/yyyy}")%>'></asp:Label>
                                                                        </ItemTemplate>

                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn DataField="Next" SortExpression="Next" AutoPostBackOnFilter="true"
                                                                        CurrentFilterFunction="Contains" HeaderText="Next" ShowFilterIcon="false" HeaderStyle-Width="100">
                                                                        <ItemTemplate>

                                                                            <asp:Label ID="lblNext" runat="server" Text='<%# Eval("Next", "{0:MM/dd/yyyy}")%>'></asp:Label>
                                                                        </ItemTemplate>

                                                                    </telerik:GridTemplateColumn>

                                                                      <telerik:GridTemplateColumn DataField="Schedule" SortExpression="Schedule" AutoPostBackOnFilter="true"
                                                                        CurrentFilterFunction="Contains" HeaderText="Schedule" ShowFilterIcon="false" HeaderStyle-Width="100">
                                                                        <ItemTemplate>

                                                                            <asp:Label ID="lblSchedule" runat="server" Text='<%# Eval("Schedule") %>'></asp:Label>
                                                                        </ItemTemplate>

                                                                    </telerik:GridTemplateColumn>
                                                                  
                                                                    <telerik:GridTemplateColumn DataField="Ticket" SortExpression="Ticket" AutoPostBackOnFilter="true"
                                                                        CurrentFilterFunction="Contains" HeaderText="Ticket" ShowFilterIcon="false" HeaderStyle-Width="100">
                                                                        <ItemTemplate>

                                                                            <asp:Label ID="lblTicket" runat="server" Text='<%# Eval("Ticket") %>'></asp:Label>
                                                                        </ItemTemplate>

                                                                    </telerik:GridTemplateColumn>

                                                                      <telerik:GridTemplateColumn DataField="TestStatusID" SortExpression="TestStatusID" AutoPostBackOnFilter="true"
                                                                        CurrentFilterFunction="Contains" HeaderText="TicketStatus" ShowFilterIcon="false" HeaderStyle-Width="100">
                                                                        <ItemTemplate>

                                                                            <asp:Label ID="lblTestStatusID" runat="server" Text='<%# Eval("TestStatusID") %>'></asp:Label>
                                                                        </ItemTemplate>

                                                                    </telerik:GridTemplateColumn>

                                                                    <telerik:GridTemplateColumn DataField="Worker" SortExpression="Worker" AutoPostBackOnFilter="true"
                                                                        CurrentFilterFunction="Contains" HeaderText="Worker" ShowFilterIcon="false" HeaderStyle-Width="100">
                                                                        <ItemTemplate>

                                                                            <asp:Label ID="lblWorker" runat="server" Text='<%# Eval("Worker") %>'></asp:Label>
                                                                        </ItemTemplate>

                                                                    </telerik:GridTemplateColumn>

                                                                      <telerik:GridTemplateColumn DataField="Ticketed" SortExpression="Ticketed" AutoPostBackOnFilter="true"
                                                                        CurrentFilterFunction="Contains" HeaderText="Ticketed" ShowFilterIcon="false" HeaderStyle-Width="100">
                                                                        <ItemTemplate>

                                                                            <asp:CheckBox ID="chkticketed" runat="server" Checked='<%# Convert.ToBoolean(Eval("Ticketed")) %>' />
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
                                <div style="clear: both;"></div>
                            </div>
                        </li>

                          <li runat="server" id="dvDocuments" style="display: none">
                            <div id="accrddocuments" class="collapsible-header accrd accordian-text-custom"><i class="mdi-action-credit-card"></i>Documents</div>
                            <div class="collapsible-body">
                                <div class="form-content-wrap">
                                    <div class="form-content-pd">
                                        <div class="form-section-row">
                                            <div class="col s12 m12 l12">
                                                <div class="row">
                                                    <!--<p>Maximum file upload size 2MB.</p>-->
                                                    <asp:FileUpload ID="FileUpload1" runat="server" AllowMultiple="true"
                                                        onchange="AddDocumentClick(this);" class="dropify" />
                                                    <!--data-max-file-size="2M"-->
                                                </div>
                                            </div>
                                        </div>
                                        <div class="btnlinks">
                                            <asp:LinkButton ID="lnkUploadDoc" runat="server"
                                                CausesValidation="False" OnClick="lnkUploadDoc_Click"
                                                Style="display: none">Upload</asp:LinkButton>
                                            <asp:LinkButton
                                                ID="lnkPostback" runat="server"
                                                CausesValidation="False" Style="display: none">Postback</asp:LinkButton>
                                        </div>
                                        <div class="btnlinks">
                                            <asp:LinkButton ID="lnkDeleteDoc" runat="server" CausesValidation="False"
                                                OnClick="lnkDeleteDoc_Click"
                                                OnClientClick="return DeleteDocumentClick(this);">Delete</asp:LinkButton>
                                        </div>


                                        <div class="grid_container mt-10">
                                            <div class="form-section-row mb">
                                                <div class="RadGrid RadGrid_Material FormGrid">
                                                    <telerik:RadCodeBlock ID="RadCodeBlock7" runat="server">
                                                        <script type="text/javascript">
                                                            function pageLoad() {
                                                                var grid = $find("<%= RadGrid_Documents.ClientID %>");
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
                                                    <telerik:RadAjaxPanel ID="RadAjaxPanel_Documents" PostBackControls="lblName" runat="server" LoadingPanelID="RadAjaxLoadingPanel_Customer" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">
                                                        <telerik:RadGrid RenderMode="Auto" ID="RadGrid_Documents" ShowFooter="True" PageSize="50" AllowFilteringByColumn="true"
                                                            PagerStyle-AlwaysVisible="true" ShowStatusBar="true" runat="server" AllowPaging="True" AllowSorting="true" Width="100%" OnPreRender="RadGrid_Documents_PreRender"
                                                            AllowCustomPaging="True" OnNeedDataSource="RadGrid_Documents_NeedDataSource">
                                                            <CommandItemStyle />
                                                            <GroupingSettings CaseSensitive="false" />
                                                            <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                                                                <Selecting AllowRowSelect="True"></Selecting>

                                                                <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                                                <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>

                                                            </ClientSettings>
                                                            <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="True" ShowFooter="True">
                                                                <Columns>
                                                                    <telerik:GridTemplateColumn Visible="false" AllowFiltering="false" ShowFilterIcon="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblID" runat="server" Text='<%# Eval("id") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>

                                                                    <telerik:GridClientSelectColumn UniqueName="chkSelect"
                                                                        HeaderStyle-Width="28">
                                                                    </telerik:GridClientSelectColumn>

                                                                    <telerik:GridTemplateColumn DataField="filename" SortExpression="filename" AutoPostBackOnFilter="true" HeaderStyle-Width="30%"
                                                                        CurrentFilterFunction="Contains" HeaderText="File Name" ShowFilterIcon="false">
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="lblName" runat="server" CausesValidation="false"
                                                                                CommandArgument='<%#Eval("filename") + "," + Eval("Path") %>'
                                                                                OnClientClick="return ViewDocumentClick(this);" OnClick="lblName_Click" Text='<%# Eval("filename") %>'>
                                                                            </asp:LinkButton>

                                                                        </ItemTemplate>

                                                                    </telerik:GridTemplateColumn>

                                                                    <telerik:GridTemplateColumn DataField="doctype" SortExpression="doctype" AutoPostBackOnFilter="true"
                                                                        CurrentFilterFunction="Contains" HeaderText="File Type" ShowFilterIcon="false" HeaderStyle-Width="15%">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblType" runat="server" Text='<%# Eval("doctype") %>'></asp:Label>
                                                                        </ItemTemplate>

                                                                    </telerik:GridTemplateColumn>

                                                                    <telerik:GridTemplateColumn  DataField="portal" SortExpression="portal" AutoPostBackOnFilter="true"
                                                                        CurrentFilterFunction="Contains" HeaderText="Portal" ShowFilterIcon="false" HeaderStyle-Width="15%">
                                                                        <ItemTemplate>
                                                                        
                                                                            <asp:CheckBox ID="chkPortal" runat="server" Checked='<%# (Eval("portal")!=DBNull.Value) ? Convert.ToBoolean(Eval("portal")): false %>' />
                                                                        </ItemTemplate>
                                                                        

                                                                    </telerik:GridTemplateColumn>

                                                                    <%--<telerik:GridTemplateColumn DataField="MSVisible" SortExpression="MSVisible" AutoPostBackOnFilter="true" HeaderStyle-Width="20%"
                                                                        CurrentFilterFunction="Contains" HeaderText="Mobile Service" ShowFilterIcon="false">
                                                                        <ItemTemplate>
                                                                            <asp:CheckBox ID="chkMSVisible" runat="server" Checked='<%# (Eval("MSVisible")!=DBNull.Value) ? Convert.ToBoolean(Eval("MSVisible")): false %>' />
                                                                        </ItemTemplate>

                                                                    </telerik:GridTemplateColumn>--%>

                                                                    <telerik:GridTemplateColumn DataField="MSVisible" SortExpression="MSVisible" AutoPostBackOnFilter="true"
                                                                HeaderText="Mobile Service" ShowFilterIcon="false" HeaderStyle-Width="120"
                                                                DataType="System.Int16" UniqueName='MSVisible' >
                                                                <FilterTemplate>
                                                                    <telerik:RadComboBox RenderMode="Auto" ID="ImportedFilter" runat="server" OnClientSelectedIndexChanged="ImportedFilterSelectedIndexChanged"
                                                                        SelectedValue='<%# ((GridItem)Container).OwnerTableView.GetColumn("MSVisible").CurrentFilterValue %>'
                                                                        Width="100px" >
                                                                        <Items>
                                                                            <telerik:RadComboBoxItem Text="All" Value="" />
                                                                            <telerik:RadComboBoxItem Text="Yes" Value="1" />
                                                                            <telerik:RadComboBoxItem Text="No" Value="0" />
                                                                        </Items>
                                                                    </telerik:RadComboBox>
                                                                    <telerik:RadScriptBlock ID="RadScriptBlock12" runat="server">
                                                                        <script type="text/javascript">
                                                                            function ImportedFilterSelectedIndexChanged(sender, args) {
                                                                                var tableView = $find("<%# ((GridItem)Container).OwnerTableView.ClientID %>");
                                                                                var filterVal = args.get_item().get_value();
                                                                                if (filterVal == "") {
                                                                                    tableView.filter("MSVisible", filterVal, "NoFilter");
                                                                                }
                                                                                else if (filterVal == "1") {
                                                                                    tableView.filter("MSVisible", "1", "EqualTo");
                                                                                }
                                                                                else if (filterVal == "0") {
                                                                                    tableView.filter("MSVisible", "0", "EqualTo");
                                                                                }
                                                                            }
                                                                        </script>
                                                                    </telerik:RadScriptBlock>
                                                                </FilterTemplate>
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkMSVisible" runat="server" Checked='<%# (Eval("MSVisible")!=DBNull.Value) ? Convert.ToBoolean(Eval("MSVisible")): false %>' />
                                                                </ItemTemplate>

                                                            </telerik:GridTemplateColumn>


                                                                    <telerik:GridTemplateColumn DataField="remarks" SortExpression="remarks" AutoPostBackOnFilter="true"
                                                                        CurrentFilterFunction="Contains" HeaderText="Remarks" ShowFilterIcon="false" HeaderStyle-Width="20%">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txtremarks" Width="500px" runat="server" Text='<%# Eval("remarks") %>'></asp:TextBox>
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
                                <div style="clear: both;"></div>
                            </div>
                        </li>

                        <li id="tbLogs" runat="server" style="display: none">
                            <div id="accrdlogs" class="collapsible-header accrd accordian-text-custom"><i class="mdi-content-content-paste"></i>Logs</div>
                            <div class="collapsible-body">
                                <div class="form-content-wrap">
                                    <div class="form-content-pd">
                                        <div class="grid_container">
                                            <div class="form-section-row mb">
                                                <div class="RadGrid RadGrid_Material">
                                                    <telerik:RadCodeBlock ID="RadCodeBlock5" runat="server">
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
                                                    <telerik:RadAjaxPanel ID="RadAjaxPanel_gvLogs" runat="server" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">
                                                        <telerik:RadGrid RenderMode="Auto" ID="RadGrid_gvLogs" AllowFilteringByColumn="true" ShowFooter="True" PageSize="50" PagerStyle-AlwaysVisible="true"
                                                            ShowStatusBar="true" runat="server" AllowPaging="True" AllowSorting="true" Width="100%" FilterType="CheckList"
                                                            AllowCustomPaging="True" OnNeedDataSource="RadGrid_gvLogs_NeedDataSource" OnItemCreated="RadGrid_gvLogs_ItemCreated">
                                                            <CommandItemStyle />
                                                            <GroupingSettings CaseSensitive="false" />
                                                            <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                                                                <Selecting AllowRowSelect="True"></Selecting>
                                                                <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                                                <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                                            </ClientSettings>
                                                            <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="True" ShowFooter="True" AllowNaturalSort="false" DataKeyNames="fUser">
                                                                <Columns>
                                                                    <telerik:GridTemplateColumn DataField="CreatedStamp" SortExpression="CreatedStamp" AutoPostBackOnFilter="true" DataType="System.String"
                                                                        CurrentFilterFunction="Contains" HeaderText="Date" ShowFilterIcon="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lbldate" runat="server" Text='<%# Eval("CreatedStamp", "{0:M/d/yyyy}")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn DataField="CreatedStamp" SortExpression="CreatedStamp" AutoPostBackOnFilter="true" DataType="System.String"
                                                                        CurrentFilterFunction="Contains" HeaderText="Time" ShowFilterIcon="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lbltime" runat="server" Text='<%# Eval("CreatedStamp","{0: hh:mm tt}") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn DataField="fUser" SortExpression="fUser" AutoPostBackOnFilter="true"
                                                                        CurrentFilterFunction="Contains" HeaderText="User" ShowFilterIcon="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblUpdBy" runat="server" Text='<%# Eval("fUser") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn DataField="Field" SortExpression="Field" AutoPostBackOnFilter="true"
                                                                        CurrentFilterFunction="Contains" HeaderText="Field" ShowFilterIcon="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblField" runat="server" Text='<%# Eval("field") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn DataField="OldVal" SortExpression="OldVal" AutoPostBackOnFilter="true"
                                                                        CurrentFilterFunction="Contains" HeaderText="Old Value" ShowFilterIcon="false" HeaderStyle-Width="305">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblOldVal" runat="server" Text='<%# Eval("OldVal") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn DataField="NewVal" SortExpression="NewVal" AutoPostBackOnFilter="true"
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

                                        <div class="cf"></div>
                                    </div>
                                </div>
                                <div style="clear: both;"></div>
                            </div>
                        </li>

                      

                    </ul>
                </div>
            </div>
        </div>
    </div>




    <asp:Button runat="server" ID="hiddenTargetControlForModalPopup" Style="display: none"
        CausesValidation="False" />
    <asp:ModalPopupExtender runat="server" ID="programmaticModalPopup" BehaviorID="programmaticModalPopupBehavior"
        TargetControlID="hiddenTargetControlForModalPopup" PopupControlID="programmaticPopup"
        BackgroundCssClass="ModalPopupBG" PopupDragHandleControlID="programmaticPopupDragHandle"
        RepositionMode="RepositionOnWindowResizeAndScroll">
    </asp:ModalPopupExtender>
    <asp:Panel runat="server" ID="programmaticPopup" Style="display: none; background: #fff; border: 1px solid #316b9d;"
        CssClass="roundCorner shadow">
        <asp:Panel runat="Server" ID="programmaticPopupDragHandle" Style="cursor: move; color: Black; text-align: center;">
        </asp:Panel>
        <div class="iframe-eqipment">
            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                <ContentTemplate>
                    <asp:Panel ID="pnlREPT" runat="server" Visible="false">
                        <div class="model-popup-body">


                            <div class="btnlinks">
                                <asp:Label CssClass="title_text" ID="Label13" runat="server">Select MCP Template</asp:Label>
                                <asp:LinkButton ID="lnkCloseTemplate" runat="server" CausesValidation="False"
                                    OnClick="lnkCloseTemplate_Click">Close</asp:LinkButton>

                                <asp:LinkButton ID="lnkSaveTemplate" runat="server"
                                    ValidationGroup="templ00"
                                    OnClick="lnkSaveTemplate_Click">Add</asp:LinkButton>
                            </div>
                        </div>
                        <asp:HiddenField ID="HdnEquipTempID" runat="server" />
                        <div class="table-css">
                            <table>
                                <tr>
                                    <td>
                                        <b>MCP Template</b>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="ddlRepTemp"
                                            Display="None" ErrorMessage="Required" SetFocusOnError="True" ValidationGroup="templ00"></asp:RequiredFieldValidator>
                                        <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender2" runat="server" Enabled="True" PopupPosition="BottomLeft"
                                            TargetControlID="RequiredFieldValidator3">
                                        </asp:ValidatorCalloutExtender>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlRepTemp" runat="server" CssClass="browser-default"
                                            ValidationGroup="templ">
                                        </asp:DropDownList>
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>Last Date
                                    </td>
                                    <td style="padding-top: 10px">
                                        <asp:TextBox ID="txtLastDate" runat="server" CssClass="browser-default" MaxLength="25"></asp:TextBox>
                                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" TargetControlID="txtLastDate">
                                        </asp:CalendarExtender>
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </asp:Panel>


                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </asp:Panel>

    <asp:Button runat="server" ID="btnhidden" Style="display: none" CausesValidation="False" />
    <asp:ModalPopupExtender runat="server" ID="ModalPopupExtender1" BehaviorID="PMPBehaviour"
        TargetControlID="btnhidden" PopupControlID="Panel1"
        RepositionMode="RepositionOnWindowResizeAndScroll">
    </asp:ModalPopupExtender>
    <asp:Panel runat="server" ID="Panel1" Style="display: none; background: #fff; border: solid;">
        <div>
            <iframe id="iframeCustomer" runat="server" class="iframe-eqipment" frameborder="0"></iframe>
        </div>
    </asp:Panel>
    <input id="ctl00_ContentPlaceHolder1_hideModalPopupViaServer" style="display: none;"
        onclick="hideModalPopup();" type="button" value="button" />
    <asp:HiddenField runat="server" ID="hdnAddeTicket" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnAddeDocument" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnDeleteDocument" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnViewDocument" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnEditeDocument" Value="Y" />

    <asp:HiddenField runat="server" ID="hdnAddSafetyTest" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnDeleteSafetyTest" Value="Y" />


</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="javascriptHolder" runat="Server">

    <script type="text/javascript" src="Design/js/plugins/dropify/js/dropify.min.js"></script>

    <script type="text/javascript">

        $(document).ready(function () {
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

            $(window).scroll(function () {
                if ($(window).scrollTop() >= 0) {
                    $('#divButtons').addClass('fixed-header');
                }
                if ($(window).scrollTop() <= 0) {
                    $('#divButtons').removeClass('fixed-header');
                }
            });

            $(".dropdown-content.select-dropdown li").on("click", function () {
                var that = this;
                setTimeout(function () {
                    if ($(that).parent().hasClass('active')) {
                        $(that).parent().removeClass('active');
                        $(that).parent().hide();
                    }
                }, 100);
            });

            //Dropify Basic
            $('.dropify').dropify();
            // Used events
            var drEvent = $('.dropify-event').dropify();

            drEvent.on('dropify.beforeClear', function (event, element) {
                return confirm("Do you really want to delete \"" + element.filename + "\" ?");
            });

            drEvent.on('dropify.afterClear', function (event, element) {
                alert('File deleted');
            });


            $("#accrdgeneralTab").addClass("active");
            $("#firstTab").show();


        });
        function ViewDocumentClick(hyperlink) {
            var IsView = document.getElementById('<%= hdnViewDocument.ClientID%>').value;
            if (IsView == "Y") {
                return true;
            } else {
                noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                return false;
            }
        }
        function DeleteDocumentClick(hyperlink) {
            var IsDelete = document.getElementById('<%= hdnDeleteDocument.ClientID%>').value;
            if (IsDelete == "Y") {
                return checkdelete();
            } else {
                noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                return false;
            }
        }
        function AddDocumentClick(hyperlink) {
            var IsAdd = document.getElementById('<%= hdnAddeDocument.ClientID%>').value;
            if (IsAdd == "Y") {
                ConfirmUpload(ctl00_ContentPlaceHolder1_FileUpload1.value)
            } else {
                noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
            }
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

            if (confirm('Upload ' + filename + '?')) { document.getElementById('<%= lnkUploadDoc.ClientID %>').click(); }
            else { document.getElementById('<%= lnkPostback.ClientID %>').click(); }
        }

        function EnablePopupValidation() {
            var valName = document.getElementById("<%=rfvPopupShutdownReason.ClientID%>");
            ValidatorEnable(valName, true);
        }
        <%--function EnablePopupDescValidation() {
            var valName = document.getElementById("<%=rfvPopupShutdownReasonDesc.ClientID%>");
            ValidatorEnable(valName, true);
        }--%>
        function CloseShutdownReasonWindow() {
            var valName = document.getElementById("<%=rfvPopupShutdownReason.ClientID%>");
            ValidatorEnable(valName, false);
            var wnd = $find('<%=ShutdownReasonWindow.ClientID %>');
            wnd.Close();
        }
        function OpenShutdownReasonModal() {
            var shutdownReasonID = $('#<%=ddlShutdownReason.ClientID%>').val();
            if (shutdownReasonID == "0") {
                $('#<%=txtPopupShutdownReason.ClientID%>').val("");
                $('#<%=hdnShutdownReasonMode.ClientID%>').val("0"); // Addnew mode
                $('#<%=chkPopupPlanned.ClientID%>').prop('checked', false);

                var wnd = $find('<%=ShutdownReasonWindow.ClientID %>');
                wnd.set_title("Add Shut Down Reason");
                wnd.Show();
            }
            else {
                $('#<%=hdnShutdownReasonMode.ClientID%>').val("1"); // Edit mode
                var shutdownReason = $('#<%=ddlShutdownReason.ClientID%> option:selected').text();
                var shutdownReasonPlanned = $('#<%=hdnShutdownReasonPlanned.ClientID%>').val();
                //debugger
                $('#<%=txtPopupShutdownReason.ClientID%>').val(shutdownReason);
                if (shutdownReasonPlanned == '1') {
                    $('#<%=chkPopupPlanned.ClientID%>').prop('checked', true);
                } else {
                    $('#<%=chkPopupPlanned.ClientID%>').prop('checked', false);
                }

                var wnd = $find('<%=ShutdownReasonWindow.ClientID %>');
                wnd.set_title("Edit Shut Down Reason");
                wnd.Show();
            }
            Materialize.updateTextFields();

        }
        <%--var valName = document.getElementById("<%=rfvPopupShutdownReasonDesc.ClientID%>");
        ValidatorEnable(valName, false);--%>
        function CloseShutdownReasonDescWindow() {

            var wnd = $find('<%=ShutdownReasonDescWindow.ClientID %>');
            wnd.Close();
        }
        function OpenShutdownReasonDescModal() {
            $('#<%=txtPopupShutdownReasonDesc.ClientID%>').val("");
            var wnd = $find('<%=ShutdownReasonDescWindow.ClientID %>');
            wnd.set_title("Enter Shut Down Reason");
            wnd.Show();

            //Materialize.updateTextFields();
        }

        function ShutdownReasonValidation() {
            // debugger
            var retVal = false;
            if (Page_ClientValidate() == true) {
                window.btn_clicked = true;


                retVal = checkLocChange();

                if (retVal) {
                    var shutdownReasonPlanned = $('#<%=hdnShutdownReasonPlanned.ClientID%>').val();
                    var isShutdown = $('#<%= chkShutdown.ClientID%>').prop('checked');
                    var shutdownReasonID = $('#<%=ddlShutdownReason.ClientID%>').val();
                    var orgShutdownStatus = false;
                    //debugger
                    if ($('#<%=hdnEqShutdownStatus.ClientID%>').val() == "1") {
                        orgShutdownStatus = true;
                    }
                    if (orgShutdownStatus != isShutdown) {
                        if (isShutdown == true) {
                            //if (shutdownReasonPlanned == '1') {// Planned
                            //    retVal = true;
                            //} else {// unplanned
                            //    // Show Popup for enter reason
                            //    if (shutdownReasonID == "0") {
                            //        noty({ text: 'Please select a reason for shut down equipment!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                            //    } else {
                            //        OpenShutdownReasonDescModal();
                            //    }

                            //    retVal = false;
                            //}
                            // Show Popup for enter reason
                            if (shutdownReasonID == "0") {
                                noty({ text: 'Please select a reason for shut down equipment!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                            } else {
                                OpenShutdownReasonDescModal();
                            }

                            retVal = false;
                        } else {
                            retVal = true;
                        }
                    }
                }
            }
            return retVal;
        }
        function getUrlParameter(name) {
            name = name.replace(/[\[]/, '\\[').replace(/[\]]/, '\\]');
            var regex = new RegExp('[\\?&]' + name + '=([^&#]*)');
            var results = regex.exec(location.search);
            return results === null ? '' : decodeURIComponent(results[1].replace(/\+/g, ' '));
        }

        function pageLoad() {
            ///////////// Quick Codes //////////////
            $("#<%=txtRemarks.ClientID%>").keyup(function (event) {
                replaceQuickCodes(event, '<%=txtRemarks.ClientID%>', $("#<%=hdnCon.ClientID%>").val());
            });
        }
    </script>
</asp:Content>


