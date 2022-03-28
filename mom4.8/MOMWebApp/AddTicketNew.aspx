<%@ Page Title="" Language="C#" MasterPageFile="~/Mom.master" AutoEventWireup="true" ValidateRequest="false" EnableEventValidation="false" Inherits="AddTicketNew" CodeBehind="AddTicketNew.aspx.cs" %>

<%@ Register Src="uc_CustomerSearch.ascx" TagName="uc_customersearch" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Namespace="CustomControls" TagPrefix="cc1" Assembly="MOMWebApp" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">


    <!--Grid Control-->
    <link href="Design/css/grid.css" rel="stylesheet" />
    <!--File Upload Control-->
    <link href="Design/js/plugins/dropify/css/dropify.css" type="text/css" rel="stylesheet" media="screen,projection">
    <%-- --OLD-----%>
    <link rel="stylesheet" href="https://ajax.googleapis.com/ajax/libs/jqueryui/1.12.1/themes/smoothness/jquery-ui.css">
    <link rel="stylesheet" href="js/signature/jquery.signaturepad.css" />
    <script type="text/javascript" src="js/Signature/jquery.signaturepad.js"></script>
    <%--Map Script--%>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery.maskedinput/1.4.1/jquery.maskedinput.min.js"></script>
    <script type="text/javascript" src="js/jquery.ns-autogrow.js"></script>
    <script type="text/javascript" src="js/jquery.geocomplete.js"></script>
    <script src="Appearance/js/bootstrap-filestyle.js"></script>
    <script type="text/javascript" src="js/quickcodes.js"></script>
    <%--<script type="text/javascript" src="css/jquery-3.1.1.min.js"></script>--%>
    <script type="text/javascript" src="Scripts/Timepicker/jquery.timepicker.js"></script>
    <link rel="stylesheet" href="Scripts/Timepicker/jquery.timepicker.css" />


    <telerik:RadCodeBlock ID="codeBlock1" runat="server">
        <script type="text/javascript">

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


        <style type="text/css">
            .lnkProcessInvoicingCss {
                display: none;
            }

            .highlight {
                background-color: Yellow;
            }

            .highlighted {
                background-color: Yellow;
            }

        </style>

        <script type="text/javascript">
            // Is automatically  Create New Project for the that Ticket
            function SaveTicket() {
                //Ref SECO-450 we should not create projects automatically 

                var hdnIsCreateJob = document.getElementById('<%=hdnIsCreateJob.ClientID%>').value;
                var hdnProjectId = document.getElementById('<%=hdnProjectId.ClientID%>').value;
                var e = document.getElementById('<%=ddlTemplate.ClientID%>');
                var ddlTemplate = e.options[e.selectedIndex].value;
                var hdnQuickBooksIntegration = document.getElementById('<%=hdnQuickBooksIntegration.ClientID%>').value;

                if ($('#<%=ddlStatus.ClientID %>').val() == 4) {
                    if (hdnProjectId == "0" || hdnProjectId == "" || hdnProjectId === undefined) {
                        if (hdnQuickBooksIntegration == "0" || hdnQuickBooksIntegration == "" || hdnQuickBooksIntegration === undefined) {
                            if (hdnIsCreateJob == "0") {
                                if (confirm("Are you sure you want to add a new project?")) {
                                    $("#<%=hdnIsCreateJob.ClientID%>").val('1');
                                    if (ddlTemplate == 0) {

                                        noty({ text: 'Please select the Project Template.', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                                        return false;
                                    }
                                }
                                else {
                                    $('#<%=txtProject.ClientID%>').focus();
                                    $("#DivProject").show();
                                    return false;
                                }
                            }
                            else {
                                if (ddlTemplate == 0) {

                                    noty({ text: 'Please select the Project Template.', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                                    return false;
                                }
                            }
                        }
                    }

                    //2 txtWorkCompl
                    if (document.getElementById('<%= txtWorkCompl.ClientID%>').value == '') {
                        noty({ text: 'Please fill Work complete description.', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                        $('#<%=txtWorkCompl.ClientID%>').focus();
                        return false;
                    }
                    //3 ddlRoute
                    var e3 = document.getElementById('<%=ddlRoute.ClientID%>');
                    var ddlRoute = e3.options[e3.selectedIndex].value;
                    if (ddlRoute == 0) {
                        noty({ text: 'Please select worker.', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                        $('#<%=ddlRoute.ClientID%>').focus();
                        return false;
                    }
                }

                // JAVA SCRIPT VALIDATION 
                //1 customer
                if (document.getElementById('<%= txtCustomer.ClientID%>').value == '') {
                    noty({ text: 'Please select the customer.', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                    $('#<%=txtCustomer.ClientID%>').focus();
                    return false;
                }

                <%-- //2 location
                     if (document.getElementById('<%= txtLocation.ClientID%>').value == '') { 
                         noty({ text: 'Please select the location.', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                          $('#<%=txtLocation.ClientID%>').focus();
                                     return false;
                }--%>

                var e1 = document.getElementById('<%=ddlCategory.ClientID%>');
                var ddlCategory = e1.options[e1.selectedIndex].value;

                //3 category
                if (ddlCategory == 0) {
                    noty({ text: 'Please select category.', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                    $('#<%=ddlCategory.ClientID%>').focus();
                    return false;
                }

                //4 txtReason
                if (document.getElementById('<%= txtReason.ClientID%>').value == '') {
                    noty({ text: 'Please fill Reason for Service.', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                    $('#<%=txtReason.ClientID%>').focus();
                    return false;
                }

                //5 Caller
                if (document.getElementById('<%= txtNameWho.ClientID%>').value == '') {
                    noty({ text: 'Please fill Caller.', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                    $('#<%=txtNameWho.ClientID%>').focus();
                    return false;
                }


                //6 city
                if (document.getElementById('<%= txtCity.ClientID%>').value == '') {
                    noty({ text: 'Please fill City.', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                    $('#<%=txtCity.ClientID%>').focus();
                    return false;
                }

                var e2 = document.getElementById('<%=ddlState.ClientID%>');
                <%--var ddlState = e2.options[e2.selectedIndex].value;

                //7 State
                if (ddlState == 0) {
                    noty({ text: 'Please select state.', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                    $('#<%=ddlState.ClientID%>').focus();
                    return false;
                }--%>
                debugger
                if ($(e2).val() == '') {
                    noty({ text: 'Please select state.', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                    $('#<%=ddlState.ClientID%>').focus();
                    return false;
                }


                // 8 Call In Time 
                if (document.getElementById('<%= txtCallTime.ClientID%>').value == '') {
                    noty({ text: 'Please fill Called In Time.', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                    $('#<%=txtCity.ClientID%>').focus();
                    return false;
                }

                // 9 Scheduled Time
                if (document.getElementById('<%= txtSchTime.ClientID%>').value == '') {
                    noty({ text: 'Please fill Scheduled Time.', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                    $('#<%=txtCity.ClientID%>').focus();
                    return false;
                }
                itemJSON();
                return checkTranslation();
            }

            ///-Document permission
            function AddDocumentClick(hyperlink) {
                var IsAdd = document.getElementById('<%= hdnAddeDocument.ClientID%>').value;
                if (IsAdd == "Y") {
                    ConfirmUpload(hyperlink.value);

                } else {
                    noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                }
            }

            function DeleteDocumentClick(hyperlink) {
                var IsDelete = document.getElementById('<%= hdnDeleteDocument.ClientID%>').value;
                if (IsDelete == "Y") {
                    return SelectedRowDelete('<%= RadgvDocuments.ClientID%>', 'file');
                } else {
                    noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                    return false;
                }
            }

            function ViewDocumentClick(hyperlink) {
                var IsView = document.getElementById('<%= hdnViewDocument.ClientID%>').value;
                if (IsView == "Y") {
                    return true;
                } else {
                    noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                    return false;
                }
            }

            function RedirectToAddPoScreen(ticketid, comp) {
                var str = 'addpo.aspx?TicketId=' + ticketid + '&comp=' + comp + '&pop=1';
                if (str != null) {
                    setTimeout(window.location.href = str, 3000);
                }
                //console.log(str);
            }
            function btnaddpoCClick() {
                var str = window.location.href;
                var s = str.search('id') > -1;
                //console.log(s);
                if (!s) {
                    var r = confirm("Save Ticket Before Add PO!");
                    if (r == true) {
                        $("#<%=hdnIsAddPO.ClientID%>").val('1');
                        document.getElementById("<%=lnkSave.ClientID%>").click();
                        return false;
                    } else {
                        $("#<%=hdnIsAddPO.ClientID%>").val('0');
                        return false;
                    }
                }
                else { return true; }
            }

            function focusParent() {
                window.close();
                //var a = false;
                //if (window.opener)
                //{
                //    a = window.opener.confirm('Close ticket window?');
                //}
                //else {
                //    window.close();
                //}
                //if (a == true)
                //{
                //    window.close();
                //} else
                //{
                //    window.focus();
                //}
            }

            function RefressTicketListContact() {
                if (window.opener && !window.opener.closed) {
                    if (window.opener.document.getElementById('ctl00_ContentPlaceHolder1_btnSearch'))
                        window.opener.document.getElementById('ctl00_ContentPlaceHolder1_btnSearch').click();
                }
                document.getElementById("<%=btnRefressTicketScreen.ClientID%>").click();

            }
            function checkedHidden(checkbox) {
                $(checkbox).closest('tr').find($("input[id*='hdnChecked']")).val("1");
            }

            function RefressTicketPage() {
                var r = confirm("Can't save the Ticket. The ticket has been already completed from Mobile Service. Click OK to refresh the screen and get the latest changes.");
                if (r == true) {
                    var str = window.location.href.replace("comp=0", "comp=1");
                    window.location.href = str;
                }
            }
        </script>

        <script type="text/javascript">
            /// Category IMG
            function CategoryImage() {
                var ddlCat = document.getElementById('<%=ddlCategory.ClientID%>');
                var cat = ddlCat.options[ddlCat.selectedIndex].value;
                $("#<%=imgCategory.ClientID%>").attr("src", 'imagehandler.ashx?catid=' + cat);
            }

            /////////////////////////////////////////////////////////////////////////////////////////
            ///////////////////////////////        Page load      ///////////////////////////////////
            /////////////////////////////////////////////////////////////////////////////////////////

            $(document).ready(function () {
                // multiple ticket             
                $("#<%= txtPhoneCust.ClientID %>").mask("(999) 999-9999? Ext 99999");
                $("#<%= txtCell.ClientID %>").mask("(999) 999-9999");

                <%--  $("#<%= chkcatlist.ClientID %> tr").click(function () {
                    var text = '';
                    (($("#<%= chkcatlist.ClientID %> tr td")).find("input[type='checkbox']:checked")).each(function () {
                        text += jQuery('label[for=' + $(this).attr('id') + ']').html() + ",";
                    });
                    $("#<%= txtWorkers.ClientID %>").val(text);
                });--%>

                $('.sigPad').signaturePad();

                jQuery('.sign-title-l').click(function () {
                    jQuery('#hdnDrawdata').val("");
                });

                ////When your form is submitted
                //$("form").submit(function (e) {
                //    //Display your loading icon
                //    if (Page_ClientValidate()) {
                //        $("#loading").show();
                //    }
                //});

                $("#NearWorkers").empty();
                //$("#NearWorkers").append("<th style='padding:4px 5px; text-align:center'>Nearest Workers <img src='images/refresh2.png' alt='refresh' title='Refresh' onclick='CallNearestWorker();' style='cursor:pointer; float:right' /></th>");

                var isTS = '<%= Session["MSM"].ToString() %>';
                var isTSint = '<%=ViewState["tsint"].ToString() %>';
                if (isTSint != "1") {
                    if (isTS == 'TS') {
                        // document.getElementById("<%=ddlStatus.ClientID%>").options[4].disabled = true;
                        // document.getElementById("<%=ddlStatus.ClientID%>").options[4].onclick = function () { alert('Ticket can only be completed from Total Service.'); };
                    }
                }

                ValidateChargeable();
                CheckIsProspect();
                calculateTotalTime();
                calculateMileage();

                ///////////////////////Ajax call for equipment search//////////////////////////////
                function dataEquip() {
                    this.prefixText = null;
                }
                function dataEmpty() {
                    this.d = "";
                }

                ///////////// Select text onclick handling ////////////////////
                selectAll('<%=txtEnrTime.ClientID%>');
                selectAll('<%=txtOnsitetime.ClientID%>');
                selectAll('<%=txtComplTime.ClientID%>');
                selectAll('<%=txtRT.ClientID%>');
                selectAll('<%=txtOT.ClientID%>');
                selectAll('<%=txtNT.ClientID%>');
                selectAll('<%=txtDT.ClientID%>');
                selectAll('<%=txtTT.ClientID%>');
                selectAll('<%=txtBT.ClientID%>');

                ///////////// Quick Codes //////////////
                $('#<%=txtReason.ClientID%>').change(function () { $("#<%=hdnIsEdited.ClientID%>").val('1'); });

                $("#<%=txtReason.ClientID%>").keyup(function (event) {
                    replaceQuickCodes(event, '<%=txtReason.ClientID%>', $("#<%=hdnCon.ClientID%>").val());
                });

                $("#<%=txtWorkCompl.ClientID%>").keyup(function (event) {
                    replaceQuickCodes(event, '<%=txtWorkCompl.ClientID%>', $("#<%=hdnCon.ClientID%>").val());
                });

                $("#<%=txtRemarks.ClientID%>").keyup(function (event) {
                    replaceQuickCodes(event, '<%=txtRemarks.ClientID%>', $("#<%=hdnCon.ClientID%>").val());
                });

                ////////////////////////////// Textbox resize ///////////////////
                <%--$('#<%=txtRemarks.ClientID%>').focus(function () {
                    $(this).animate({
                        //right: "+=0",
                        //width: '520px',
                        height: '+=150px'
                    }, 500, function () {
                        // Animation complete.
                    });
                });

                $('#<%=txtRemarks.ClientID%>').blur(function () {
                    $(this).animate({
                        width: '100%',
                        height: '63px'
                    }, 500, function () {
                        // Animation complete.
                    });
                });--%>

                <%--$('#<%=txtReason.ClientID%>').focus(function () {
                    $(this).animate({
                        width: '520px',
                        height: '+=150px'
                    }, 500, function () {
                        // Animation complete.
                    });

                    //$(this).zIndex(1);
                });

                $('#<%=txtReason.ClientID%>').blur(function () {
                    $(this).animate({
                        width: '100%'
                        , height: '63px'
                    }, 500, function () {
                        // Animation complete.
                    });
                    // $(this).zIndex(0);
                });--%>

                $('#<%=txtRecommendation.ClientID%>').focus(function () {
                    $(this).animate({
                        width: '520px'
                        , height: '+=150px'
                    }, 500, function () {
                        // Animation complete.
                    });
                });

                $('#<%=txtRecommendation.ClientID%>').blur(function () {
                    $(this).animate({
                        width: '100%',
                        height: '63px'
                    }, 500, function () {
                        // Animation complete.
                    });
                });

                $('#<%=txtPartsUsed.ClientID%>').focus(function () {
                    $(this).animate({
                        width: '520px'
                        , height: '+=150px'
                    }, 500, function () {
                        // Animation complete.
                    });
                });

                $('#<%=txtPartsUsed.ClientID%>').blur(function () {
                    $(this).animate({
                        width: '100%',
                        height: '63px'
                    }, 500, function () {
                        // Animation complete.
                    });
                });

                $('#<%=txtComments.ClientID%>').focus(function () {
                    $(this).animate({
                        width: '520px'
                        , height: '+=150px'
                    }, 500, function () {
                        // Animation complete.
                    });
                });

                $('#<%=txtComments.ClientID%>').blur(function () {
                    $(this).animate({
                        width: '100%',
                        height: '63px'
                    }, 500, function () {
                        // Animation complete.
                    });
                });

                <%--$('#<%=txtCreditReason.ClientID%>').focus(function () {
                    $(this).animate({
                        width: '520px'
                        , height: '+=150px'
                    }, 500, function () {
                        // Animation complete.
                    });
                    // $(this).zIndex(2);
                });

                $('#<%=txtCreditReason.ClientID%>').blur(function () {
                    $(this).animate({
                        width: '100%',
                        height: '75px'
                    }, 500, function () {
                        // Animation complete.
                    });
                    // $(this).zIndex(0);
                });--%>

                $('#<%=txtWorkCompl.ClientID%>').focus(function () {
                    $(this).animate({
                        width: '520px'
                        , height: '+=150px'
                    }, 500, function () {
                        // Animation complete.
                    });
                });

                $('#<%=txtWorkCompl.ClientID%>').blur(function () {
                    $(this).animate({
                        width: '100%'
                        , height: '63px'
                    }, 500, function () {
                        // Animation complete.
                    });
                });

                ///////////// Invoice ////////////////////////////////////
                //            $('#<%=txtInvoiceNo.ClientID%>').blur(function() {
                //                CheckIsInvoiced();
                //            });

                $('#<%=chkInvoice.ClientID%>').change(function () {
                    ValidateChargeable();
                });


                var oImg = document.getElementById("<%=imgSign.ClientID%>");
                var ImgHdn = document.getElementById("<%=hdnImg.ClientID%>");
                oImg.src = ImgHdn.value;

                $('#<%= txtGoogleAutoc.ClientID %>').keyup(function (ev) {
                    //                if (event.which != 27 && event.which != 37 && event.which != 38 && event.which != 39 && event.which != 40 && event.which != 13) {
                    if ((e.which >= 46 && e.which <= 90) || (e.which >= 96 && e.which <= 105) || (e.which >= 186 && e.which <= 222) || e.which == 8) {
                        var txtLat = document.getElementById('<%= lat.ClientID %>');
                        var txtLng = document.getElementById('<%= lng.ClientID %>');
                        txtLat.value = '';
                        txtLng.value = '';
                    }
                });

                //Time picker focus fuction
                $('input.timepicker').timepicker({
                    dropdown: false
                });
                $('input.timepicker1').timepicker({
                    dropdown: false
                });
                $('input.timepicker1').on('click', function () {
                    if ($('input.timepicker1').val() == "") {
                        $('input.timepicker1').timepicker('setTime', new Date());
                        $(this).select();
                    }
                    else { $(this).select(); }
                });
                $('input.timepicker1').on('focus', function () {

                    $(this).select();
                });
                $('input.timepicker').on('focus', function () {

                    $(this).select();
                });
                
                $('#<%=txtRemarks.ClientID%>').animate({
                    //right: "+=0",
                    //width: '520px',
                    height: '63px'
                }, 500, function () {
                    // Animation complete.
                });
                   
            });

            function isCanvasSupported() {
                var elem = document.createElement('canvas');
                return !!(elem.getContext && elem.getContext('2d'));
            }

            ///////////// Custom validator function for customer auto search  ////////////////////
            function ChkCustomer(sender, args) {
                var hdnPatientId = document.getElementById('<%=hdnPatientId.ClientID%>');
                if (hdnPatientId.value == '') {
                    args.IsValid = false;
                }
            }

            ///////////// Custom validator function for location auto search  ////////////////////
            function ChkLocation(sender, args) {
                var hdnLocId = document.getElementById('<%=hdnLocId.ClientID%>');
                if (hdnLocId.value == '') {
                    args.IsValid = false;
                }
            }

            function ace_itemSelected(sender, e) {
                //            var hdnPatientId = document.getElementById('ctl00_ContentPlaceHolder1_hdnPatientId');
                //            hdnPatientId.value = e.get_value();
                //            document.getElementById('ctl00_ContentPlaceHolder1_btnSelectCustomer').click();
            }

            function cancel() {
                ////window.parent.document.getElementById('ctl00_ContentPlaceHolder1_hideModalPopupViaServer').click();
                //var conf = confirm('Do you want to close the ticket screen?')
                //if (conf) {
                window.close();
                //}
            }

            ///////////////////////////////    Convert signature to image      ////////////////////////////////
            function toImage() {
                var hdnDrawdata = document.getElementById("hdnDrawdata");
                var hdnImg = document.getElementById("<%=hdnImg.ClientID%>");
                var oImgElement = document.getElementById("<%=imgSign.ClientID%>");
                var canvas = document.getElementById("canvas");
                if (hdnDrawdata.value != "") {
                    var img = canvas.toDataURL("image/png");
                    oImgElement.src = img;
                    hdnImg.value = img;
                }
            }

            ////////////////////////    Select all text in textbox on click     ////////////////////////////
            function selectAll(id) {
                var eraInput = document.getElementById(id);
                $("#" + id).click(function () {
                    //                setTimeout(function() {
                    eraInput.setSelectionRange(0, 9999);
                    //                }, 10);
                });
            }

            ////////////////////////    Calculate time difference between ER-OS-CT    ////////////////////////////
            function calculate_Time() {

                var txtER = $("#<%=txtEnrTime.ClientID%>").val();
                var txtOS = $("#<%=txtOnsitetime.ClientID%>").val();
                var txtCT = $("#<%=txtComplTime.ClientID%>").val();

                if (txtOS != '' && txtER != '' && txtOS != '__:__ AM' && txtOS != '__:__ PM' && txtER != '__:__ AM' && txtER != '__:__ PM') {
                    var diff = Math.round(((new Date('01/01/2009 ' + txtOS) - new Date('01/01/2009 ' + txtER)) / 1000 / 60 / 60) * 100) / 100;
                    if (diff < 0) { diff = diff + 24; }
                    $("#<%=txtTT.ClientID%>").val(diff.toFixed(2));
                }
                else {
                    $("#<%=txtTT.ClientID%>").val('0.00');
                }

                if (txtOS != '' && txtCT != '' && txtOS != '__:__ AM' && txtOS != '__:__ PM' && txtCT != '__:__ AM' && txtCT != '__:__ PM') {
                    var diff = Math.round(((new Date('01/01/2009 ' + txtCT) - new Date('01/01/2009 ' + txtOS)) / 1000 / 60 / 60) * 100) / 100;
                    if (diff < 0) { diff = diff + 24; }
                    $("#<%=txtRT.ClientID%>").val(diff.toFixed(2));
                }
                else {
                    $("#<%=txtRT.ClientID%>").val('0.00');
                }

                //For Break time Deduction  reset the values
                if ($("#<%=txtBT.ClientID%>").val() != "") {
                    if (parseFloat($("#<%=txtBT.ClientID%>").val()) != 0) {
                        $("#<%=txtOT.ClientID%>").val($("#<%=HiddenFieldOT.ClientID%>").val());
                        $("#<%=txtNT.ClientID%>").val($("#<%=HiddenFieldNT.ClientID%>").val());
                        $("#<%=txtDT.ClientID%>").val($("#<%=HiddenFieldDT.ClientID%>").val());
                    }
                }

                $("#<%=HiddenFieldRT.ClientID%>").val($("#<%=txtRT.ClientID%>").val());
                $("#<%=HiddenFieldTT.ClientID%>").val($("#<%=txtTT.ClientID%>").val());

                calculateTotalTime();
                DeductedValue(false);
            }

            //////DeductedValue////////////
            function DeductedValue(TXTbtTextChange) {

                var DeductedValue = 0;
                var txtHdnBT = 0;
                if (TXTbtTextChange == true) {
                    txtHdnBT = $("#<%=HiddenFieldBT.ClientID%>").val();
                }
                else {
                    $("#<%=HiddenFieldBT.ClientID%>").val(0);
                }

                if (txtHdnBT == '') {
                    txtHdnBT = '0.00';
                }

                if ($("#<%=HiddenFieldBTFlag.ClientID%>").val() == '1' || (parseFloat(txtHdnBT) > parseFloat(0))) {

                    $("#<%=txtRT.ClientID%>").val($("#<%=HiddenFieldRT.ClientID%>").val());
                    $("#<%=txtOT.ClientID%>").val($("#<%=HiddenFieldOT.ClientID%>").val());
                    $("#<%=txtNT.ClientID%>").val($("#<%=HiddenFieldNT.ClientID%>").val());
                    $("#<%=txtDT.ClientID%>").val($("#<%=HiddenFieldDT.ClientID%>").val());
                    $("#<%=txtTT.ClientID%>").val($("#<%=HiddenFieldTT.ClientID%>").val());
                }
                var txtRT = $("#<%=txtRT.ClientID%>").val();
                var txtOT = $("#<%=txtOT.ClientID%>").val();
                var txtNT = $("#<%=txtNT.ClientID%>").val();
                var txtDT = $("#<%=txtDT.ClientID%>").val();
                var txtTT = $("#<%=txtTT.ClientID%>").val();

                calculateTotalTime();

                var txtBT = $("#<%=txtBT.ClientID%>").val();

                var txtTotal = $("#<%=txtTotal.ClientID%>").val();


                if (txtBT == '') {
                    txtBT = '0.00';
                }

                if (txtTotal == '') {
                    txtTotal = '0.00';
                }
                if ((parseFloat(txtHdnBT) > parseFloat(0))) {
                    txtBT = (parseFloat(txtBT) - parseFloat(txtHdnBT));
                }
                //|| (parseFloat(txtBT) < parseFloat(0))
                if ((parseFloat(txtBT) > parseFloat(txtTotal))) {
                    if (TXTbtTextChange == true) {
                        if ((parseFloat(txtBT) > parseFloat(txtTotal))) {

                            noty({ text: "Break Time must be less than Total time", dismissQueue: true, type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: false, theme: 'noty_theme_default', closable: false });
                            $("#<%=txtBT.ClientID%>").val(0);
                        }

                        <%--  if ((parseFloat(txtBT) < parseFloat(0))) {

                        noty({ text: "Break time must be greater than zero", dismissQueue: true, type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: false, theme: 'noty_theme_default', closable: false });
                         
                        $("#<%=txtBT.ClientID%>").val(0);
                    }--%>
                    }
                }

                // && (parseFloat(txtBT) > parseFloat(0)) 
                if ((parseFloat(txtBT) <= parseFloat(txtTotal))) {

                    DeductedValue = parseFloat(txtBT);

                    if (txtRT == '') {
                        txtRT = '0.00';
                    }
                    if (txtOT == '') {
                        txtOT = '0.00';
                    }
                    if (txtNT == '') {
                        txtNT = '0.00';
                    }
                    if (txtDT == '') {
                        txtDT = '0.00';
                    }
                    if (txtTT == '') {
                        txtTT = '0.00';
                    }

                    if (txtBT == '') {
                        txtBT = '0.00';
                    }

                    //  rt > tt > ot > dt>  > nt 

                    if (parseFloat(DeductedValue) < parseFloat(txtRT)) {

                        txtRT = (parseFloat(txtRT) - parseFloat(DeductedValue));
                        DeductedValue = 0
                    }
                    else {
                        DeductedValue = (parseFloat(DeductedValue) - parseFloat(txtRT));
                        txtRT = 0;
                    }


                    if (parseFloat(DeductedValue) < parseFloat(txtTT)) {

                        txtTT = (parseFloat(txtTT) - parseFloat(DeductedValue));
                        DeductedValue = 0
                    }
                    else {
                        DeductedValue = (parseFloat(DeductedValue) - parseFloat(txtTT));
                        txtTT = 0;
                    }

                    if (parseFloat(DeductedValue) < parseFloat(txtOT)) {

                        txtOT = (parseFloat(txtOT) - parseFloat(DeductedValue));
                        DeductedValue = 0
                    }
                    else {
                        DeductedValue = (parseFloat(DeductedValue) - parseFloat(txtOT));
                        txtOT = 0;
                    }

                    if (parseFloat(DeductedValue) < parseFloat(txtDT)) {

                        txtDT = (parseFloat(txtDT) - parseFloat(DeductedValue));
                        DeductedValue = 0
                    }
                    else {
                        DeductedValue = (parseFloat(DeductedValue) - parseFloat(txtDT));
                        txtDT = 0;
                    }

                    if (parseFloat(DeductedValue) < parseFloat(txtNT)) {

                        txtNT = (parseFloat(txtNT) - parseFloat(DeductedValue));
                        DeductedValue = 0
                    }
                    else {
                        DeductedValue = (parseFloat(DeductedValue) - parseFloat(txtNT));
                        txtNT = 0;
                    }

                    $("#<%=txtRT.ClientID%>").val(txtRT);
                    $("#<%=txtOT.ClientID%>").val(txtOT);
                    $("#<%=txtNT.ClientID%>").val(txtNT);
                    $("#<%=txtDT.ClientID%>").val(txtDT);
                    $("#<%=txtTT.ClientID%>").val(txtTT);
                    $("#<%=HiddenFieldBTFlag.ClientID%>").val(1);
                }
                calculateTotalTime();
            }

            ////////////////////////    Calculate total time    ////////////////////////////
            function calculateTotalTime() {
                var txtRT = $("#<%=txtRT.ClientID%>").val();
                var txtOT = $("#<%=txtOT.ClientID%>").val();
                var txtNT = $("#<%=txtNT.ClientID%>").val();
                var txtDT = $("#<%=txtDT.ClientID%>").val();
                var txtTT = $("#<%=txtTT.ClientID%>").val();


                if (txtRT == '') {
                    txtRT = '0.00';
                }
                if (txtOT == '') {
                    txtOT = '0.00';
                }
                if (txtNT == '') {
                    txtNT = '0.00';
                }
                if (txtDT == '') {
                    txtDT = '0.00';
                }
                if (txtTT == '') {
                    txtTT = '0.00';
                }

                var total = Math.round((parseFloat(txtRT) + parseFloat(txtOT) + parseFloat(txtNT) + parseFloat(txtDT) + parseFloat(txtTT)) * 100) / 100;

                $("#<%=txtTotal.ClientID%>").val(total.toFixed(2));
                $("#<%=lblTotal.ClientID%>").text(total.toFixed(2));

            }

            ////////////////////  Check Translation ///////////////////
            function checkTranslation() {

                ToggleValidator();

                if (Page_ClientValidate()) {
                    debugger
                    var charge = ValidateChargeable();
                    if (charge == false) {
                        return false;
                    }

                    var prospect = NewProspectAlert();
                    if (prospect == false) {
                        return false;
                    }

                    var hdnMultiLang = $("#<%=hdnMultiLang.ClientID%>").val();
                    var hdnLang = $("#<%=hdnLang.ClientID%>").val();
                    var hdnIsEdited = $("#<%=hdnIsEdited.ClientID%>").val();

                    if (hdnLang != 'english' && hdnIsEdited == '1' && hdnMultiLang == '1') {

                        //                var r = confirm('The ticket has not been converted to ' + hdnLang + '. Do you want to convert the ticket to ' + hdnLang + ' ?');
                        var r = confirm('The reason for service text has been changed but not translated to Spanish/English. Click OK to open the translation box else cancel to continue save the ticket.');
                        if (r == true) {
                            //$('#<%=pnlTranslate.ClientID%>').show();
                            $('#<%=txtReason.ClientID%>').focus();
                            return false;
                        }
                        else {
                            return true;
                        }
                    }
                }
            }

            ////////////////////  Check Follow-up ticket ///////////////////
            function CheckWorkComplete(ticid, checked, msg, comp, invoice) {

                //            var checked = $("#<%=chkWorkComp.ClientID%>").is(':checked');
                if (checked == 0) {
                    var r = confirm('Would you like to create a follow-up ticket at this time?');
                    if (r == true) {
                        window.location.href = "addticket.aspx?copy=1&follow=1&id=" + ticid + "&comp=" + comp;
                    }
                    else {
                        noty({ text: msg, dismissQueue: true, type: 'success', layout: 'topCenter', closeOnSelfClick: true, timeout: false, theme: 'noty_theme_default', closable: false });
                    }
                }
                else {
                    noty({ text: msg, dismissQueue: true, type: 'success', layout: 'topCenter', closeOnSelfClick: true, timeout: false, theme: 'noty_theme_default', closable: false });
                }
                if (invoice == 1) {
                    {
                        document.getElementById("<%=lnkProcessInvoicing.ClientID%>").click();
                    }
                }
            }

            /////////////////// Calculate Traveled Mileage //////////////////
            function calculateMileage() {
                var txtSM = $("#<%=txtMileStart.ClientID%>").val();
                var txtEM = $("#<%=txtMileEnd.ClientID%>").val();

                if (txtSM == '') {
                    txtSM = '0.00';
                }
                if (txtEM == '') {
                    txtEM = '0.00';
                }

                var total = parseInt(txtEM) - parseInt(txtSM);

                $("#<%=txtMileTraveled.ClientID%>").text(total);

                ////////Expenses
                var txtExpMisc = $("#<%=txtExpMisc.ClientID%>").val();
                var txtExpToll = $("#<%=txtExpToll.ClientID%>").val();
                var txtExpZone = $("#<%=txtExpZone.ClientID%>").val();

                if (txtExpMisc == '') {
                    txtExpMisc = '0.00';
                }
                if (txtExpToll == '') {
                    txtExpToll = '0.00';
                }
                if (txtExpZone == '') {
                    txtExpZone = '0.00';
                }

                var Expensestotal = parseFloat(txtExpMisc) + parseFloat(txtExpToll) + parseFloat(txtExpZone);

                $("#<%=lblTotalExp.ClientID%>").text(Expensestotal);
            }

            ////////////////// Confirm Document Upload ////////////////////
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

                if (confirm('Upload ' + filename + '?')) { document.getElementById("<%=lnkUploadDoc.ClientID%>").click(); }
                else { document.getElementById("<%=lnkPostback.ClientID%>").click(); }
            }

            //////////////// Confirm Mail Send to worker ///////////////////
            function notyConfirm(ticid) {
                noty({
                    dismissQueue: true,
                    layout: 'topCenter',
                    theme: 'noty_theme_default',
                    animateOpen: { height: 'toggle' },
                    animateClose: { height: 'toggle' },
                    easing: 'swing',
                    text: 'Do you want to send a text message to the assigned worker at this time?',
                    type: 'alert',
                    speed: 500,
                    timeout: false,
                    closeButton: false,
                    closeOnSelfClick: true,
                    closeOnSelfOver: false,
                    force: false,
                    onShow: false,
                    onShown: false,
                    onClose: false,
                    onClosed: false,
                    buttons: [
                        {
                            type: 'btn-primary', text: 'Yes', click: function ($noty) {
                                $noty.close();
                                window.open("mailticket.aspx?id=" + ticid + "&c=0", "_blank");
                            }
                        },
                        {
                            type: 'btn-danger', text: 'No', click: function ($noty) {
                                $noty.close();
                                window.open("addticket.aspx?id=" + ticid + "&comp=0&pop=1&fr=tlv", "_self");
                            }
                        }
                    ],
                    modal: true,
                    template: '<div class="noty_message"><span class="noty_text"></span><div class="noty_close"></div></div>',
                    cssPrefix: 'noty_',
                    custom:
                    {
                        container: null
                    }
                });
            }

            ////////////////// Called when save form and validation caused on tab 1 //////////////
            function tabchng() {

                <%--tc = $find("<%=TabContainer2.ClientID%>");
                tc.set_activeTabIndex(0);--%>
            }

            function ValidateTime(sender, args) {
                var start = document.getElementById('<%=txtEnrTime.ClientID%>').value;
                var end = document.getElementById('<%=txtOnsitetime.ClientID%>').value;

                var fromdt = "2010/01/01 " + start;
                var todt = "2010/01/01 " + end;

                var from = new Date(Date.parse(fromdt));
                var to = new Date(Date.parse(todt));

                if (from > to) {
                    args.IsValid = false;
                } else {
                    args.IsValid = true;
                }
            }

            function ValidateTimeComplete(sender, args) {
                var start = document.getElementById('<%=txtOnsitetime.ClientID%>').value;
                var end = document.getElementById('<%=txtComplTime.ClientID%>').value;

                var fromdt = "2010/01/01 " + start;
                var todt = "2010/01/01 " + end;

                var from = new Date(Date.parse(fromdt));
                var to = new Date(Date.parse(todt));

                if (from > to) {
                    args.IsValid = false;
                } else {
                    args.IsValid = true;
                }
            }

            function ValidateChargeable() {
                var chkChargeable = document.getElementById('<%=chkChargeable.ClientID%>');
                var chkInvoice = document.getElementById('<%=chkInvoice.ClientID%>');
                var txtInvoiceID = document.getElementById('<%=txtInvoiceNo.ClientID%>');

                if (chkInvoice.checked == true) {

                    if (chkChargeable.checked == false) {
                        noty({
                            text: 'Ticket must be chargeable to generate invoice.',
                            type: 'warning',
                            layout: 'topCenter',
                            closeOnSelfClick: false,
                            timeout: 5000,
                            theme: 'noty_theme_default',
                            closable: true
                        });
                        chkInvoice.checked = false;
                        return false;
                    }
                    else {
                        return true;
                    }
                }
                else {
                    return true;
                }
            }

            function CheckIsProspect() {
                var hdnPatientId = document.getElementById('<%=hdnPatientId.ClientID%>');
                var txtLoc = document.getElementById('<%=txtLocation.ClientID%>');
                var txtCust = document.getElementById('<%=txtCustomer.ClientID%>');
                var hdnProspect = document.getElementById('<%=hdnProspect.ClientID%>');
                if (hdnPatientId.value == '' && txtCust.value != '') {
                    txtLoc.disabled = true;
                    txtLoc.value = '';
                    hdnProspect.value = "1";
                    txtCust.style.color = "brown";
                    ValidatorEnable(document.getElementById('<%=RequiredFieldValidator1.ClientID%>'), false);
                    //document.getElementById('<%=btnValidateLocation.ClientID%>').click();
                    document.getElementById('<%=txtAcctno.ClientID%>').value = '--';
                }
                else {
                    hdnProspect.value = "";
                }
            }

            function NewProspectAlert() {
                var hdnPatientId = document.getElementById('<%=hdnPatientId.ClientID%>');
                var txtLoc = document.getElementById('<%=txtLocation.ClientID%>');
                var txtCust = document.getElementById('<%=txtCustomer.ClientID%>');
                var hdnLocID = document.getElementById('<%=hdnLocId.ClientID%>');
                var lblalertprospect = document.getElementById('<%=lblAlertProspect.ClientID%>');
                if (hdnPatientId.value == '' && txtCust.value != '' && hdnLocID.value == '') {
                    var r = confirm('You have not selected an existing Customer/Lead. The entered Customer name will be considered as a new Lead. Click OK to continue creating a new Lead or click on Cancel to select an existing Customer/Lead.');
                    return r;
                }
            }

            function CheckIsInvoiced() {
                var txtInvoiceID = document.getElementById('<%=txtInvoiceNo.ClientID%>');
                var chkInvoice = document.getElementById('<%=chkInvoice.ClientID%>');
                var chkCharge = document.getElementById('<%=chkChargeable.ClientID%>');

                if (txtInvoiceID.value != '') {
                    chkInvoice.checked = false;
                }
            }

            function CallNearestWorker() {
                var txtlat = document.getElementById('<%= lat.ClientID %>');
                var txtlng = document.getElementById('<%= lng.ClientID %>');

                if (txtlat.value == '') {

                    var mainaddress = document.getElementById('<%= txtGoogleAutoc.ClientID %>').value;
                    var city = document.getElementById('<%= txtCity.ClientID %>').value;
                    var state = document.getElementById('<%= ddlState.ClientID %>').value;
                    var zip = document.getElementById('<%= txtZip.ClientID %>').value;
                    var address = mainaddress + ', ' + city + ', ' + state + ', ' + zip;

                    if (mainaddress != '') {
                        $("#wait").show();
                        codeAddress(address, function (latlng) {
                            txtlat.value = latlng.lat();
                            txtlng.value = latlng.lng();
                            InitMap(latlng.lat(), latlng.lng());
                            CallNearestWorkerList(latlng.lat(), latlng.lng());
                        });
                    }
                }
                else {
                    $("#wait").show();
                    CallNearestWorkerList(txtlat.value, txtlng.value);
                }
            }

            function CallNearestWorkerList(lat, lng) {
                var worker = '';
                if (lat != '') {
                    if (lng != '') {
                        function NearWorkerData() {
                            this.lat = null;
                            this.lng = null;
                            this.worker = null;
                            // this.ISDefaultWorker = null;
                        }

                        var objNearWorkerData = new NearWorkerData();
                        objNearWorkerData.lat = lat;
                        objNearWorkerData.lng = lng;
                        objNearWorkerData.worker = worker;
                        // objNearWorkerData.ISDefaultWorker = 0;

                        $.ajax({
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            url: "NearWorker.asmx/GetNearWorker",
                            data: JSON.stringify(objNearWorkerData),
                            dataType: "json",
                            async: true,
                            success: function (data) {
                                drawTable($.parseJSON(data.d), worker);
                            },
                            error: function (XMLHttpRequest, textStatus, errorThrown) {
                                //var err = eval("(" + XMLHttpRequest.responseText + ")");
                                //alert(err.Message);
                            }
                        }).done(CallDefaultworker);
                    }
                }
            }

            function CallDefaultworker() {
                var ddlDefRoute = document.getElementById('<%= ddlDefRoute.ClientID %>');
                var worker = ddlDefRoute.options[ddlDefRoute.selectedIndex].text
                var lat = document.getElementById('<%= lat.ClientID %>').value;
                var lng = document.getElementById('<%= lng.ClientID %>').value;
                if (lat != '') {
                    if (lng != '') {
                        function NearWorkerData() {
                            this.lat = null;
                            this.lng = null;
                            this.worker = null;
                            //this.ISDefaultWorker = null;
                        }

                        var objNearWorkerData = new NearWorkerData();
                        objNearWorkerData.lat = lat;
                        objNearWorkerData.lng = lng;
                        objNearWorkerData.worker = worker;
                        //objNearWorkerData.ISDefaultWorker = 1;
                        $.ajax({
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            url: "NearWorker.asmx/GetNearWorker",
                            data: JSON.stringify(objNearWorkerData),
                            dataType: "json",
                            async: true,
                            success: function (data) {
                                drawTable($.parseJSON(data.d), worker);
                            },
                            error: function (XMLHttpRequest, textStatus, errorThrown) {
                                //var err = eval("(" + XMLHttpRequest.responseText + ")");
                                //alert(err.Message);
                            }
                        });
                    }
                }
                $("#wait").hide();
            }

            function drawTable(data, worker) {
                if (worker == "") {
                    $("#NearWorkers").empty();
                    //$("#NearWorkers").append("<th>Nearest Workers <img src='images/refresh2.png' alt='refresh' title='Refresh' onclick='CallNearestWorker();' style='cursor:pointer; float:right' /></th>");
                    for (var i = 0; i < data.length; i++) {
                        drawRow(data[i]);
                    }
                }
                else {
                    for (var i = 0; i < data.length; i++) {
                        drawRowDefault(data[i]);
                    }
                }
            }

            function drawRow(rowData) {
                //if (rowData.latitude != "" && rowData.longitude != "") {
                if (rowData.GPS == 1) {
                    getGeoAddress(rowData.latitude, rowData.longitude, function (addr) {
                        createrow(rowData, addr);
                    });
                }
                else {
                    createrow(rowData, rowData.address);
                }
            }

            function createrow(rowData, addr) {
                var row = $("<tr/>")
                $("#NearWorkers").append(row);

                var strrow = "<td><hr/>";
                strrow += "<div class='nearest-head'>";
                strrow += " <span class='worker-name'><b>" + rowData.worker + " </b></span>";
                strrow += " <span class='nearest-time'>" + rowData.Time + "</span>";
                strrow += " </div>";
                strrow += " <div class='nearest-address'>";
                if (addr != '') {
                    strrow += " <span>" + addr + " </span>";
                }
                strrow += " <span style='float: right' class='distance'>" + rowData.dist + " miles";
                strrow += "  </span>";
                strrow += " </div> ";
                strrow += "</td>";
                row.append($(strrow));

            }

            function drawRowDefault(rowData) {
                //if (rowData.latitude != "" && rowData.longitude != "") {
                if (rowData.GPS == 1) {
                    getGeoAddress(rowData.latitude, rowData.longitude, function (addr) {
                        CreateRowDefault(rowData, addr);
                    });
                }
                else {
                    CreateRowDefault(rowData, rowData.address);
                }
            }

            function CreateRowDefault(rowData, addr) {
                var row = $("<tr/>")
                row.prependTo("#NearWorkers > tbody");

                var strrow = "<td style='background-color:yellow;'><hr/>";
                strrow += "<div class='nearest-head'>";
                strrow += " <span class='worker-name'><b>" + rowData.worker + " </b></span>";
                strrow += " <span class='nearest-time'>" + rowData.Time + " </span>";
                strrow += " </div>";
                strrow += " <div class='nearest-address'>";
                if (addr != '') {
                    strrow += " <span>" + addr + " </span>";
                }
                strrow += " <span style='float: right' class='distance'>" + rowData.dis + " miles";
                strrow += "  </span>";
                strrow += " </div> ";
                strrow += "</td>";
                row.append($(strrow));
            }

            function getGeoAddress(lat, lng, callback) {
                var latlng = new google.maps.LatLng(lat, lng);
                var geocoder = new google.maps.Geocoder();
                geocoder.geocode({ 'latLng': latlng }, function (results, status) {
                    if (status == google.maps.GeocoderStatus.OK) {
                        if (results[1]) {
                            callback(results[1].formatted_address);
                        }
                    }
                    else {
                        callback('');
                    }
                });
            }

            function codeAddress(address, callback) {
                var success = 0;
                var geocoder = new google.maps.Geocoder();
                geocoder.geocode({ 'address': address }, function (results, status) {
                    if (status == google.maps.GeocoderStatus.OK) {
                        if (results[0].geometry.location_type == 'ROOFTOP') {
                            callback(results[0].geometry.location);
                            success = 1;
                        }
                    }
                });
                //            if (success == 0) {
                //                $("#NearWorkers").empty();
                //                $("#NearWorkers").append("<th>Nearest Workers <img src='images/refresh2.png' alt='refresh' title='Refresh' onclick='CallNearestWorker();' style='cursor:pointer; float:right' /></th>");
                //            }
            }
            $(":file").filestyle({
                buttonName: "btn-primary",
            });

            function ToggleValidator() {
                var chk = false;
                // var valName = document.getElementById("=rfvjtempl.ClientID");
                <%--if (document.getElementById("<%=ddlStatus.ClientID%>").value == '4') {
                    chk = (document.getElementById("<%=hdnProjectId.ClientID%>").value == '' || document.getElementById("<%=hdnProjectId.ClientID%>").value == '0') ? true : false
                }--%>
                //  ValidatorEnable(valName, chk);
            }

            function checkMaxLength(textarea, evt, maxLength) {
                if ($("#<%= hdnSageInt.ClientID %>").val() == "1") {
                    var lines = textarea.value.split("\n");
                    for (var i = 0; i < lines.length; i++) {
                        if (lines[i].length <= 30) continue;
                        var j = 0; space = 30;
                        while (j++ <= 30) {
                            if (lines[i].charAt(j) === " ") space = j;
                        }
                        lines[i + 1] = lines[i].substring(space + 1) + (lines[i + 1] || "");
                        lines[i] = lines[i].substring(0, space);
                    }
                    textarea.value = lines.slice(0, 2).join("\n");
                    $("#spnAddress").fadeIn('slow', function () {
                        $(this).delay(500).fadeOut('slow');
                    });
                }
            }

            function OpenWOreport() {
                if ($("#<%= txtWO.ClientID %>").val() != "" && $("#<%= hdnLocId.ClientID %>").val() != "")//&& $("#<%= chkReviewed.ClientID %>").is(':checked') == true && $("#<%= chkInternet.ClientID %>").is(':checked') == true
                {
                    window.open("printwo.aspx?wo=" + $("#<%= txtWO.ClientID %>").val() + "&lid=" + $("#<%= hdnLocId.ClientID %>").val(), '_blank');
                }
            }

            function NewProject() {
                $('#<%= hdnProjectId.ClientID %>').val("");
                $('#<%= txtProject.ClientID %>').val("--New Project--");
                $('#<%= projectNo.ClientID %>').val("--New Project--");
                $('#<%= ddlTemplate.ClientID %>').prop('disabled', false);
                $('#<%= lnkProjectID.ClientID %>').removeAttr("href");
                $('#<%= txtJobCode.ClientID %>').val("");
            }
        </script>
    </telerik:RadCodeBlock>

    <script type="text/javascript">
        function ProcessInvoicing() {
            var chkChargeable = document.getElementById('<%=chkChargeable.ClientID%>');

            if (chkChargeable.checked == true) {
                return true;
            } else {
                noty({
                    text: 'Ticket must be chargeable to generate invoice.',
                    type: 'warning',
                    layout: 'topCenter',
                    closeOnSelfClick: false,
                    timeout: 5000,
                    theme: 'noty_theme_default',
                    closable: true
                });
                return false;
            }
        }

        function ProcessInvoicingnext() {
            debugger;
            var Workorderonly = 0, Project = 0, Reviewonly = 0, Combind = 0;

            if ($('#rdTicket').is(':checked')) {
                Workorderonly = 0, Project = 0, Combind = 0;
            }
            else {

                if ($('#rdWorkorderonly').is(':checked')) {
                    Workorderonly = 1;
                }

                if ($('#rdProject').is(':checked')) {
                    Project = 1;
                }

                if ($('#<%= Combind.ClientID %>').is(':checked')) {
                    Combind = 1;
                }
            }
            if ($('#<%= Reviewonly.ClientID %>').is(':checked')) {
                Reviewonly = 1;
            }
            
            var tickid =<%= Request.QueryString["id"] %>
            //todo
                window.open('addinvoiceNew.aspx?o=1&Reviewonly=' + Reviewonly + '&Project=' + Project + '&Workorderonly=' + Workorderonly + '&Combind=' + Combind + '&tickid=' + tickid, 'Invoice', 'height = 768, width = 1300, scrollbars = yes,toolbar=no,menubar=no,location=no');

               
            return false;
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <asp:HiddenField ID="hdnIsJobChargable" runat="server" Value="0" />
    <asp:HiddenField ID="hdnIsCatChargable" runat="server" Value="0" />

    <%-------$$$$$$$$$$$$$$$ RAD AJAX MANAGER  $$$$$$$$$$$$$$$-----%>

    <telerik:RadAjaxManager ID="RadAjaxManager_AddTicket" runat="server" OnAjaxRequest="RadAjaxManager_AddTicket_AjaxRequest">
        <AjaxSettings>


            <telerik:AjaxSetting AjaxControlID="btnAddNewLines">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="accrdInventory" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnchkCreateMultipleTicket">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadAjaxPanelTicketInfo" />
                    <telerik:AjaxUpdatedControl ControlID="RadAjaxPanelCompleted" />
                    <telerik:AjaxUpdatedControl ControlID="ddlStatus" />
                    <telerik:AjaxUpdatedControl ControlID="ddlStatus1" />
                    <telerik:AjaxUpdatedControl ControlID="ddlRoute" />
                    <telerik:AjaxUpdatedControl ControlID="ddlRoute1" />
                </UpdatedControls>
            </telerik:AjaxSetting>


            <telerik:AjaxSetting AjaxControlID="lnkProcessInvoicing">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadWindow1" />
                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="btnCodes">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadWindowReasonForServervice" />
                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="ddlCodeCat">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadWindowReasonForServervice" />
                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="btnCodesCmpl">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadWindowReasonForServervice" />
                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="lnkbtnlocContractinfo">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadWindowlocContractinfo" />
                    <telerik:AjaxUpdatedControl ControlID="RadAjaxPanelRadGVContractInfo" />
                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="RadgvEquip">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadWindowlocContractinfo" />
                    <telerik:AjaxUpdatedControl ControlID="RadAjaxPanelRadGVContractInfo" />
                </UpdatedControls>

            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="lblRelatedTickets">
                <UpdatedControls>
                    <%-- <telerik:AjaxUpdatedControl ControlID="RadWindow1RelatedTicket" />--%>
                    <telerik:AjaxUpdatedControl ControlID="RadAjaxPanelForRadGvlstRelatedTickets" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <%-- --------------btnEnroute--------------%>
            <telerik:AjaxSetting AjaxControlID="btnEnroute">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadAjaxPanelTicketInfo" />
                    <telerik:AjaxUpdatedControl ControlID="RadAjaxPanelCompleted" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <%-- --------------btnOnsite--------------%>
            <telerik:AjaxSetting AjaxControlID="btnOnsite">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadAjaxPanelTicketInfo" />
                    <telerik:AjaxUpdatedControl ControlID="RadAjaxPanelCompleted" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <%-- --------------btnComplete--------------%>
            <telerik:AjaxSetting AjaxControlID="btnComplete">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadAjaxPanelTicketInfo" />
                    <telerik:AjaxUpdatedControl ControlID="RadAjaxPanelCompleted" />
                </UpdatedControls>
            </telerik:AjaxSetting>


            <%-- --------------ddlstatus--------------%>
            <telerik:AjaxSetting AjaxControlID="lnkddlStatus">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadAjaxPanelTicketInfo" />
                    <telerik:AjaxUpdatedControl ControlID="RadAjaxPanelCompleted" />
                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="ddlStatus1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadAjaxPanelTicketInfo" />
                    <telerik:AjaxUpdatedControl ControlID="RadAjaxPanelCompleted" />
                </UpdatedControls>
            </telerik:AjaxSetting>

            <%-- --------------ddlCategory--------------%>


            <telerik:AjaxSetting AjaxControlID="ddlCategory">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="chkChargeable" />
                    <telerik:AjaxUpdatedControl ControlID="chkJobChargeable" />
                    <telerik:AjaxUpdatedControl ControlID="ddlCategory" />
                    <telerik:AjaxUpdatedControl ControlID="imgCategory" />
                </UpdatedControls>
            </telerik:AjaxSetting>

            <%-- --------------ddlRoute1--------------%>


            <telerik:AjaxSetting AjaxControlID="ddlRoute1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="ddlRoute" />
                    <telerik:AjaxUpdatedControl ControlID="RadAjaxPanelHiddenField" />
                    <telerik:AjaxUpdatedControl ControlID="RadAjaxPanelTicketInfo" />
                    <telerik:AjaxUpdatedControl ControlID="RadAjaxPanelCompleted" />
                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="ddlRoute">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="ddlRoute1" />
                    <telerik:AjaxUpdatedControl ControlID="RadAjaxPanelHiddenField" />
                    <telerik:AjaxUpdatedControl ControlID="RadAjaxPanelTicketInfo" />
                    <telerik:AjaxUpdatedControl ControlID="RadAjaxPanelCompleted" />
                </UpdatedControls>
            </telerik:AjaxSetting>

            <%-- --------------Customer --------------%>
            <telerik:AjaxSetting AjaxControlID="txtCustomer">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadAjaxPanelHiddenField" />
                    <telerik:AjaxUpdatedControl ControlID="RadAjaxPanelTicketInfo" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <%-- --------------btnSelectCustomer --------------%>
            <telerik:AjaxSetting AjaxControlID="btnSelectCustomer">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadAjaxPanelHiddenField" />
                    <telerik:AjaxUpdatedControl ControlID="RadAjaxPanelTicketInfo" />
                    <telerik:AjaxUpdatedControl ControlID="lnkConvert" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="lnkConvert">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadAjaxPanelHiddenField" />
                    <telerik:AjaxUpdatedControl ControlID="RadAjaxPanelTicketInfo" />
                    <telerik:AjaxUpdatedControl ControlID="pnlCustomer" />
                </UpdatedControls>
            </telerik:AjaxSetting>

            <%-- --------------txtProject --------------%>
            <telerik:AjaxSetting AjaxControlID="txtProject">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="DivProject" />
                    <telerik:AjaxUpdatedControl ControlID="RadAjaxPanelTicketInfo" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <%-- --------------btnSelectLoc --------------%>
            <telerik:AjaxSetting AjaxControlID="btnSelectLoc">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadAjaxPanelHiddenField" />
                    <telerik:AjaxUpdatedControl ControlID="lblRelatedTickets" />
                    <telerik:AjaxUpdatedControl ControlID="RadAjaxPanelstats" />
                    <telerik:AjaxUpdatedControl ControlID="RadAjaxPanelTicketInfo" />
                    <telerik:AjaxUpdatedControl ControlID="lnkbtnlocContractinfo" />
                    <telerik:AjaxUpdatedControl ControlID="RadAjaxPanelRadgvEquip" />
                    <telerik:AjaxUpdatedControl ControlID="lstRecentCalls" />
                    <telerik:AjaxUpdatedControl ControlID="RadAjaxPanelHeader" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <%-- --------------btnGetCode --------------%>
            <telerik:AjaxSetting AjaxControlID="btnGetCode">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadAjaxPanelHiddenField" />
                    <telerik:AjaxUpdatedControl ControlID="RadAjaxPanelTicketInfo" />
                    <telerik:AjaxUpdatedControl ControlID="RadAjaxPanelCompleted" />
                    <telerik:AjaxUpdatedControl ControlID="rptCodesList" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <%-- ------------------btnDone---------------------%>
            <telerik:AjaxSetting AjaxControlID="btnDone">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadAjaxPanelHiddenField" />
                    <telerik:AjaxUpdatedControl ControlID="txtReason" />
                    <telerik:AjaxUpdatedControl ControlID="chklstCodes" />
                </UpdatedControls>
            </telerik:AjaxSetting>

        </AjaxSettings>
    </telerik:RadAjaxManager>

    <%-------$$$$$$$$$$$$$$$ HEADER SETION  $$$$$$$$$$$$$$$-----%>



    <div class="divbutton-container">
        <div id="divButtons" class="">
            <div id="breadcrumbs-wrapper">
                <header>
                    <div class="container row-color-grey">
                        <div class="row">
                            <div class="row">

                                <div class="col s12 m12 l12">
                                    <div class="page-title"><i class="mdi-action-trending-up"></i>&nbsp;<asp:Label CssClass="title_text" ID="lblHeader" runat="server">Add Ticket</asp:Label></div>
                                    <div class="buttonContainer">
                                        <div class="btnlinks">
                                            <asp:LinkButton ID="lnkSave" ToolTip="Save Ticket" runat="server" ValidationGroup="addticketVG" CausesValidation="true" OnClick="lnkSave_Click"
                                                OnClientClick="return SaveTicket();">Save</asp:LinkButton>
                                        </div>

                                        <div class="btnlinks">
                                            <asp:HyperLink ID="lnkCopy" Visible="false" ToolTip="Copy Ticket" runat="server" CausesValidation="false">Copy</asp:HyperLink>
                                        </div>

                                        <div class="btnlinks">
                                            <asp:LinkButton ID="lnkProcessInvoicing" ToolTip="Invoice" CssClass="lnkProcessInvoicingCss" runat="server" OnClick="lnkProcessInvoicing_Click" CausesValidation="false"
                                                OnClientClick="return ProcessInvoicing();">Invoice</asp:LinkButton>
                                        </div>

                                        <div class="btnlinks">
                                            <asp:LinkButton class="dropdown-button" data-beloworigin="true" data-activates="dynamicUI" ID="lnkPrint" runat="server" CausesValidation="false"
                                                Visible="true" Text="Report" ToolTip="Report">                         
                                            </asp:LinkButton>
                                        </div>

                                        <ul id="dynamicUI" class="dropdown-content">
                                            <asp:Literal ID="dynamicUIPlaceholder" runat="server" />
                                        </ul>

                                        <div class="btnlinks">
                                            <asp:LinkButton ID="lnkPDF" OnClick="lnkPDF_Click" runat="server" CausesValidation="false"
                                                Visible="true" Text="PDF" ToolTip="Print Ticket">                        
                                            </asp:LinkButton>
                                        </div>
                                    </div>
                                    <%--<div class="btnclosewrap"><a href="TicketListView.aspx" data-tooltip="Close"><i class="mdi-content-clear"></i></a></div>--%>
                                    <div class="btnclosewrap">
                                        <asp:LinkButton ID="lnkClose" ToolTip="Close" runat="server" CausesValidation="false"
                                            OnClick="lnkClose_Click"><i class="mdi-content-clear"></i></asp:LinkButton>
                                    </div>
                                    <div class="btnclosewrap-one">
                                        <a class="collapse-expand opened" data-position="bottom" data-tooltip="Expand/Collapse Accordion">
                                            <i class="mdi-action-open-in-browser"></i>
                                        </a>
                                    </div>
                                    <div class="rght-content">
                                        <div class="editlabel">
                                            <telerik:RadAjaxPanel ID="RadAjaxPanelHeader" Visible="false" runat="server">
                                                <ul class="anchor-links" style="display: inline-block;">


                                                    <li runat="server" visible="false" id="liTicket">
                                                        <asp:Label runat="server" ID="lblTicketHeader"></asp:Label>
                                                    </li>
                                                    <li>
                                                        <asp:Label runat="server" ID="lblAVCHeader"></asp:Label>
                                                    </li>
                                                    <li>
                                                        <asp:Label runat="server" ID="lblLocHeader"></asp:Label>
                                                    </li>
                                                    <li>
                                                        <div class="tooltipped" id="Statustooltipped" runat="server" style="background-color: whitesmoke; border-radius: 100px; height: 20px; width: 20px; margin-top: 7px;" data-position="left" data-tooltip="Un-Assigned"></div>
                                                    </li>
                                                </ul>
                                            </telerik:RadAjaxPanel>
                                        </div>
                                    </div>

                                </div>
                            </div>
                        </div>
                    </div>
                    <a class="icon-closed" id="lnkCancelContact" title="Close" onclick="focusParent()"></a>
                    <a id="lnkRefressTicketList" style="display: none;" onclick="RefressTicketListContact()"></a>
                    <asp:Button ID="btnMail" runat="server" Text="Email" Style="display: none;" OnClick="btnMail_Click"
                        CausesValidation="False" />
                    <asp:LinkButton ID="btnRefressTicketScreen" Style="display: none;" runat="server" CausesValidation="false" OnClick="btnRefressTicketScreen_Click"></asp:LinkButton>


                </header>
            </div>

            <div class="container breadcrumbs-bg-custom">
                <div class="row">
                    <div class="col s12 m12 l12">
                        <div class="row">
                            <div class="tblnks">
                                <ul class="anchor-links">
                                    <li style="width: 90px; text-align: center;"><a class="add-btn-click">Show Stats</a> </li>
                                    <li><a href="#accrdticketInfo" class="link-slide">Ticket Info.</a></li>
                                    <li><a href="#accrdcompleted" class="link-slide">Completed</a></li>
                                    <li><a href="#accrdcustom" class="link-slide">Custom</a></li>
                                    <li><a href="#accrdResolution" class="link-slide" onclick="LoadaccrdResolutiontab();">Resolution</a></li>
                                    <li><a href="#accrdMCP" class="link-slide" onclick="LoadaccrdMCPtab();">MCP</a></li>
                                    <li><a href="#accrdPO" class="link-slide" onclick="LoadaccrdPOtab();">PO</a></li>
                                    <li><a href="#accrddocuments" class="link-slide">Documents</a></li>
                                    <li><a href="#accrdInventory" class="link-slide" onclick="LoadaccrdInventorytab();">Inventory Used</a></li>
                                    <li id="liLogs" runat="server" style="display: none"><a href="#accrdlogs" onclick="LoadGvlog();">Logs</a></li>
                                </ul>
                            </div>
                            <div class="tblnksright">
                                <div class="nextprev" style="display: inline-block;">


                                    <asp:Panel ID="pnlNext" runat="server" Visible="false">

                                        <span class="angleicons">
                                            <asp:LinkButton ID="lnkFirst" OnClick="lnkFirst_Click" ToolTip="First" runat="server" CausesValidation="False">
                                                        <i class="fa fa-angle-double-left"></i>
                                            </asp:LinkButton>
                                        </span>
                                        <span class="angleicons">
                                            <asp:LinkButton ID="lnkPrevious" OnClick="lnkPrevious_Click" ToolTip="Previous" runat="server" CausesValidation="False">
                                                        <i class="fa fa-angle-left"></i>
                                            </asp:LinkButton>
                                        </span>
                                        <span class="angleicons">
                                            <asp:LinkButton ID="lnkNext" OnClick="lnkNext_Click" ToolTip="Next" runat="server" CausesValidation="False">
                                                        <i class="fa fa-angle-right"></i>
                                            </asp:LinkButton>
                                        </span>
                                        <span class="angleicons">
                                            <asp:LinkButton ID="lnkLast" OnClick="lnkLast_Click" ToolTip="Last" runat="server" CssClass="icon-last" CausesValidation="False">
                                                        <i class="fa fa-angle-double-right"></i>
                                            </asp:LinkButton>
                                        </span>
                                    </asp:Panel>
                                </div>

                            </div>
                        </div>
                    </div>


                </div>

                <div id="stats" style="background-color: #fff !important;">
                    <div id="addinfo" class="form-section-row" style="background-color: #fff; padding: 10px !important; box-shadow: 0 2px 2px 2px #ccc !important;">

                        <telerik:RadAjaxPanel ID="RadAjaxPanelstats" runat="server">
                            <div class="form-section3">
                                <div class="input-field col s12">

                                    <div>

                                        <div id="Coord" style="display: block; background-color: #E5E3DF; border: 1px solid; display: none">
                                            <input id="lat" runat="server" type="text" onfocus="this.blur();" />
                                            <input id="lng" runat="server" type="text" onfocus="this.blur();" />
                                            <input id="locality" disabled="disabled" style="display: none;" />
                                            <input id="country" disabled="disabled" style="display: none;" />
                                        </div>
                                        <div id="map" style="border: 0; width: 100%; height: 250px;">
                                        </div>

                                    </div>

                                </div>

                            </div>
                            <div class="form-section3-blank">
                                &nbsp;
                            </div>
                            <div class="form-section1">
                                <div class="section-ttle">
                                    Nearest Workers 
                                <img src='images/refresh2.png' alt='refresh' title='Refresh' onclick='CallNearestWorker();' style='cursor: pointer; float: right' />

                                    <div id="wait" style="display: none">
                                        <div>
                                            <img src="images/wheel.gif" />
                                        </div>
                                        <span>Loading
                                            Nearest Workers...</span>
                                    </div>

                                </div>
                                <div class="nearest-wrap">
                                    <table id="NearWorkers" class="roundCorner" style="margin: 5px 0px 0px 0; border-radius: 5px; border: 1px solid #000; width: 100%">
                                    </table>
                                </div>
                            </div>
                            <div class="form-section3-blank">
                                &nbsp;
                            </div>
                        </telerik:RadAjaxPanel>
                        <div class="form-section1">
                            <div class="section-ttle">
                                Recent Calls
                                            <span style="float: right;">
                                                <label style="height: 20px !important; font-size: 0.8em !important;">(Click on ticket too see more info.)</label>
                                            </span>
                            </div>
                            <div class="nearest-worker-wrapper">

                                <div class="nearest-worker ps-container ps-active-y" style="margin-top: -17px !important;">
                                    <div class="ticket-wrap">
                                        <asp:ListView ID="lstRecentCalls" runat="server">
                                            <LayoutTemplate>

                                                <asp:PlaceHolder runat="server" ID="itemPlaceholder"></asp:PlaceHolder>

                                            </LayoutTemplate>
                                            <EmptyDataTemplate>
                                                <table>
                                                    <tr>
                                                        <td>No records available.
                                                        </td>
                                                    </tr>
                                                </table>
                                            </EmptyDataTemplate>
                                            <ItemTemplate>


                                                <ul class="collapsible collapsible-tckts" data-collapsible="expandable">
                                                    <li>

                                                        <div class="collapsible-header collapsible-ticket-head tckts" style="<%# ( Convert.ToString(Eval("elev")) == hdnUnitID.Value  && hdnUnitID.Value!=""  && hdnUnitID.Value!="0") ?  "background-color:yellow": "background-color:white" %>">
                                                            <span class="worker-name"><b>
                                                                <asp:HyperLink runat="server" ID="lblId"
                                                                    NavigateUrl='<%# String.Format("addticket.aspx?id={0}&comp={1}", Eval("id"), Eval("comp")) %>'
                                                                    Text='<%# "Ticket#" + Eval("ID") %>' Target="_blank">
                                                                </asp:HyperLink></b></span>

                                                            <span class="ticket-datetime" style="float: right; font-size: 0.8em;">
                                                                <span class="ticket-date">
                                                                    <asp:Label runat="server" ID="lblDate"><%# Eval("edate", "{0:MM/dd/yy}") %></asp:Label></span>
                                                                <span class="ticket-time"><%# Eval("edate", "{0: hh:mm tt}") %></span>
                                                            </span>
                                                        </div>
                                                        <div class="collapsible-body">
                                                            <div class="form-content-wrap">
                                                                <div class="form-content-pd tckts-content">
                                                                    <div class="ticket-row">

                                                                        <div class="ticket-row">
                                                                            <div class="ticket-rw-left">
                                                                                <asp:Label runat="server" ID="lblStatus"><%# Eval("assignname").ToString()%>&nbsp;</asp:Label>


                                                                                <%#  (Eval("assignname").ToString().ToLower() == "assigned") ? " to" : " by "  %>
                                                                            </div>
                                                                            <div class="ticket-rw-right"><%#Eval("worker").ToString()%></div>
                                                                        </div>
                                                                        <div class="ticket-row">
                                                                            <div class="ticket-rw-left">Category</div>
                                                                            <div class="ticket-rw-right"><%#Eval("cat").ToString()%></div>
                                                                        </div>
                                                                        <div class="ticket-row">
                                                                            <div class="ticket-rw-left">Equipment</div>
                                                                            <div class="ticket-rw-right"><%#Eval("elevname").ToString()%></div>
                                                                        </div>



                                                                    </div>
                                                                    <div class="cf"></div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </li>

                                                </ul>

                                            </ItemTemplate>
                                        </asp:ListView>


                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="form-section3-blank">
                            &nbsp;
                        </div>

                        <div class="form-section1">
                            <div class="section-ttle">Stats</div>
                            <div class="nearest-wrap">
                                <div class="nearest-head">
                                    <span class="worker-name"><b>Contract Type:</b></span>
                                    <span class="nearest-time">Personal</span>
                                </div>
                            </div>
                            <div class="nearest-wrap">
                                <div class="nearest-head">
                                    <span class="worker-name"><b># of Calls on Avg:</b></span>
                                    <span class="nearest-time">45</span>
                                </div>
                            </div>
                            <div class="nearest-wrap">
                                <div class="nearest-head">
                                    <span class="worker-name"><b>Random Info #1</b></span>
                                    <span class="nearest-time">Random</span>
                                </div>
                            </div>
                            <div class="nearest-wrap">
                                <div class="nearest-head">
                                    <span class="worker-name"><b>Random Info #2</b></span>
                                    <span class="nearest-time">Random</span>
                                </div>
                            </div>
                            <div class="nearest-wrap">
                                <div class="nearest-head">
                                    <span class="worker-name"><b>Random Info #3</b></span>
                                    <span class="nearest-time">Random</span>
                                </div>
                            </div>
                            <div class="nearest-wrap">
                                <div class="nearest-head">
                                    <span class="worker-name"><b>Random Info #4</b></span>
                                    <span class="nearest-time">Random</span>
                                </div>
                            </div>
                        </div>
                        <div class="form-section3-blank">
                            &nbsp;
                        </div>


                    </div>
                </div>

            </div>

        </div>
    </div>

    <%-------$$$$$$$$$$$$$$$ ACCORDIAN  $$$$$$$$$$$$$$$-----%>

    <div class="container accordian-wrap">
        <div class="row">
            <div class="col s12 m12 l12">
                <div class="row">
                    <ul class="collapsible popout collapsible-accordion form-accordion-head" data-collapsible="expandable">
                        <li class="active">

                            <div id="accrdticketInfo" class="collapsible-header accrd active accordian-text-custom "><i class="mdi-image-filter-frames"></i>Ticket Info.</div>

                            <div class="collapsible-body" style="display: block;">
                                <div class="form-content-wrap">
                                    <telerik:RadAjaxPanel ID="RadAjaxPanelTicketInfo" runat="server">
                                        <div class="form-content-pd">
                                            <div class="form-section-row">
                                                <div class="form-section3">
                                                    <div class="input-field col s5">
                                                        <div class="row">
                                                            <label for="lblTicketnumber">Ticket # </label>
                                                            <asp:TextBox ID="lblTicketnumber" runat="server">
                                                            </asp:TextBox>



                                                        </div>
                                                    </div>
                                                    <div class="input-field col s2">
                                                        <div class="row">
                                                            &nbsp; 
                                                        <asp:Image ID="imgHigh" runat="server" Visible="false" Width="25px" ToolTip="Declined"
                                                            ImageUrl="images/exclamation.png" />
                                                        </div>
                                                    </div>
                                                    <div class="input-field col s5">
                                                        <div class="row">
                                                            <asp:TextBox ID="txtWO" runat="server" MaxLength="10"
                                                                TabIndex="1"></asp:TextBox>
                                                            <label for="txtWO"><a href="#" onclick="OpenWOreport();" style="color: #1565C0; margin-left: -15px;">WO #</a></label>


                                                            <asp:HiddenField ID="hdnWO" runat="server" />

                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator33" runat="server" ControlToValidate="txtWO"
                                                                Display="None" ErrorMessage="WO # Required" SetFocusOnError="True" Enabled="False"
                                                                ValidationGroup="wo"></asp:RequiredFieldValidator>

                                                            <asp:ValidatorCalloutExtender ID="RequiredFieldValidator33_ValidatorCalloutExtender"
                                                                runat="server" Enabled="True" PopupPosition="TopLeft" TargetControlID="RequiredFieldValidator33">
                                                            </asp:ValidatorCalloutExtender>
                                                        </div>
                                                    </div>

                                                    <div class="srchclr btnlinksicon rowbtn">
                                                        <asp:LinkButton ID="lblRelatedTickets" runat="server" ToolTip="Related Tickets"
                                                            CausesValidation="False" OnClientClick="lblRelatedTickets_Client();" OnClick="lblRelatedTickets_Click">  <i class="mdi-maps-local-attraction" style="margin-left:0px !important;"></i></asp:LinkButton>
                                                    </div>
                                                </div>
                                                <div class="form-section3-blank">
                                                    &nbsp;
                                                </div>
                                                <div class="form-section3">
                                                    <div class="input-field col s9">

                                                        <asp:TextBox runat="server" TabIndex="2" ID="txtProject" autocomplete="off"></asp:TextBox>
                                                        <label for="txtProject">Project</label>
                                                        <div id="DivProject" class="popup_div " style="width: 500px; z-index: 2">

                                                            <div class="btnlinks" style="margin-bottom: 5px; float: left;">
                                                                <asp:LinkButton OnClientClick="NewProject();" CausesValidation="false" OnClick="addnewproject_Click" runat="server" ID="addnewproject">Add New </asp:LinkButton>

                                                            </div>

                                                            <div class="btnlinks" style="margin-bottom: 5px; float: right;">
                                                                <asp:LinkButton CausesValidation="false" ID="HideMELinkButton3" runat="server" OnClientClick="HideME();">Close</asp:LinkButton>
                                                            </div>
                                                            <div class="grid_container">
                                                                <div class="RadGrid RadGrid_Material FormGrid">
                                                                    <telerik:RadAjaxPanel ID="RadAjaxPanelRadgvProject" runat="server">
                                                                        <telerik:RadGrid ID="RadgvProject"
                                                                            RenderMode="Auto" AllowFilteringByColumn="false" ShowFooter="True" PageSize="10"
                                                                            ShowStatusBar="true" runat="server" AllowPaging="true" AllowSorting="false" Width="100%"
                                                                            OnNeedDataSource="RadgvProject_NeedDataSource"
                                                                            PagerStyle-AlwaysVisible="true"
                                                                            AllowCustomPaging="false">
                                                                            <CommandItemStyle />
                                                                            <GroupingSettings CaseSensitive="false" />
                                                                            <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                                                                                <Selecting AllowRowSelect="True"></Selecting>
                                                                                <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                                                                <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>

                                                                            </ClientSettings>
                                                                            <MasterTableView AutoGenerateColumns="false" AllowPaging="true" AllowFilteringByColumn="True" ShowFooter="True">
                                                                                <Columns>
                                                                                    <telerik:GridTemplateColumn DataField="id" AutoPostBackOnFilter="false" AllowFiltering="false" SortExpression="id"
                                                                                        CurrentFilterFunction="Contains" UniqueName="id" HeaderText="Project#" ShowFilterIcon="false" HeaderStyle-Width="50">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblID" runat="server" Text='<%# Bind("id") %>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle Width="0px" />
                                                                                    </telerik:GridTemplateColumn>
                                                                                    <telerik:GridTemplateColumn DataField="fdesc" AutoPostBackOnFilter="false" AllowFiltering="false" SortExpression="fdesc"
                                                                                        CurrentFilterFunction="Contains" UniqueName="fdesc" HeaderText="Description" ShowFilterIcon="false" HeaderStyle-Width="150">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblDescP" runat="server"><%#Eval("fdesc")%></asp:Label>
                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>


                                                                                    <telerik:GridTemplateColumn DataField="fdate" AutoPostBackOnFilter="false" AllowFiltering="false" SortExpression="fdate"
                                                                                        CurrentFilterFunction="Contains" UniqueName="fdate" HeaderText="Date" ShowFilterIcon="false" HeaderStyle-Width="100">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lbldate" runat="server"><%#Eval("fdate", "{0:MM/dd/yyyy}")%></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <footertemplate><%= RadgvProject.VirtualItemCount  %>  Record(s) Found.   </footertemplate>
                                                                                        </FooterTemplate>
                                                                                    </telerik:GridTemplateColumn>

                                                                                    <telerik:GridTemplateColumn DataField="charge" AutoPostBackOnFilter="false" AllowFiltering="false" SortExpression="charge"
                                                                                        CurrentFilterFunction="Contains" UniqueName="charge" HeaderText="Charge" ShowFilterIcon="false" HeaderStyle-Width="01">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblCharge" runat="server"><%#Eval("charge")%></asp:Label>
                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>

                                                                                </Columns>
                                                                            </MasterTableView>
                                                                        </telerik:RadGrid>
                                                                    </telerik:RadAjaxPanel>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="srchclr btnlinksicon rowbtn">

                                                            <asp:HyperLink ID="lnkProjectID" Visible="true" Target="_blank" runat="server"><i class="mdi-action-work" style="margin-left:0px !important;"></i> </asp:HyperLink>

                                                        </div>

                                                    </div>
                                                    <div class="input-field col s3">
                                                        <div class="checkrow">
                                                            <asp:CheckBox ID="chkJobChargeable" runat="server" onclick="JobChargeableCheck()" CssClass="css-checkbox" Text="Chargeable" />
                                                        </div>

                                                    </div>



                                                </div>
                                                <div class="form-section3-blank">
                                                    &nbsp;     
                                                </div>
                                                <div class="form-section3">

                                                    <div class="input-field col s4">
                                                        <div class="row">
                                                            <asp:TextBox ID="txtInvoiceNo" runat="server" MaxLength="12"
                                                                TabIndex="4" Enabled="False"></asp:TextBox>
                                                            <label for="txtInvoiceNo">Invoice Number</label>

                                                            <asp:FilteredTextBoxExtender ID="txtInvoiceNo_FilteredTextBoxExtender" runat="server"
                                                                Enabled="False" FilterType="Numbers" TargetControlID="txtInvoiceNo">
                                                            </asp:FilteredTextBoxExtender>
                                                        </div>
                                                    </div>
                                                    <div class="input-field col s2">
                                                        <div class="row">
                                                            &nbsp;
                                                             <a id="lnkInvoice" runat="server" target="_blank" title="Invoice" visible="False"
                                                                 style="cursor: pointer;">
                                                                 <asp:Image ID="imgInv" runat="server" Style="height: 20px; margin: 1px 1px 1px 1px"
                                                                     ToolTip="Invoice" />

                                                             </a>
                                                        </div>
                                                    </div>
                                                    <div class="input-field col s4">
                                                        <div class="row">
                                                            <asp:TextBox ID="txtopp" runat="server" MaxLength="12"
                                                                TabIndex="4" Enabled="False"></asp:TextBox>
                                                            <label for="txtInvoiceNo">Opportunity #</label>

                                                        </div>
                                                    </div>
                                                    <div class="input-field col s2">
                                                        <div class="row">


                                                            <asp:HyperLink ID="lnkOpport" runat="server"
                                                                Target="_blank" Visible="False">
                                                                <asp:Image ID="Image1" runat="server" Style="height: 25px; margin: 1px 2px 0px 5px"
                                                                    ToolTip="Opportunity" ImageUrl="images/thumb_up.png" />
                                                            </asp:HyperLink>

                                                        </div>
                                                    </div>

                                                </div>

                                            </div>


                                            <div class="form-section-row">
                                                <div class="form-section3">
                                                    <div class="section-ttle">Search Fields</div>
                                                    <div class="input-field col s12">
                                                        <div class="row">

                                                            <label for="txtCustomer">Customer Name <span class="reqd">*</span> </label>


                                                            <asp:Label ID="lblAlertProspect" runat="server" Style="padding: 3px; left: 211px; top: 38px; display: none; position: absolute; color: Gray; background-color: White;"
                                                                CssClass="roundCorner transparent shadow" Text="You have not selected the existing customer/lead. This will add new lead."></asp:Label>


                                                            <asp:FilteredTextBoxExtender ID="txtCustomer_FilteredTextBoxExtender" runat="server"
                                                                Enabled="False" FilterMode="InvalidChars" InvalidChars="'\" TargetControlID="txtCustomer">
                                                            </asp:FilteredTextBoxExtender>

                                                            <asp:Button ID="btnSelectCustomer" runat="server" CausesValidation="False" OnClick="btnSelectCustomer_Click"
                                                                Style="display: none;" Text="Button" />


                                                            <asp:CustomValidator ID="CustomValidator1" runat="server" ClientValidationFunction="ChkCustomer"
                                                                ControlToValidate="txtCustomer" Display="None" ErrorMessage="Please select the customer"
                                                                SetFocusOnError="True" Enabled="False"></asp:CustomValidator>

                                                            <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender2" runat="server" Enabled="True"
                                                                PopupPosition="BottomLeft" TargetControlID="CustomValidator1">
                                                            </asp:ValidatorCalloutExtender>

                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator19" runat="server" ControlToValidate="txtCustomer" ValidationGroup="addticketVG"
                                                                Display="None" ErrorMessage="Please select the customer" SetFocusOnError="True"></asp:RequiredFieldValidator>

                                                            <asp:ValidatorCalloutExtender ID="RequiredFieldValidator19_ValidatorCalloutExtender"
                                                                runat="server" Enabled="True" PopupPosition="BottomLeft" TargetControlID="RequiredFieldValidator19">
                                                            </asp:ValidatorCalloutExtender>

                                                            <asp:TextBox ID="txtCustomer" runat="server" onkeydown="return (event.keyCode!=13);" autocomplete="off"></asp:TextBox>


                                                        </div>
                                                    </div>
                                                    <div class="srchclr btnlinksicon rowbtn">
                                                        <asp:LinkButton ID="lnkConvert" runat="server" Visible="False" OnClick="lnkConvert_Click" CausesValidation="False" ToolTip="Convert Lead"><i class="mdi-communication-ring-volume" style="margin-left:0px !important;"></i></asp:LinkButton>

                                                    </div>
                                                    <div class="srchclr btnlinksicon rowbtn">
                                                        <asp:HyperLink for="txtCustomer" ID="lnkCustomerID" Visible="true" Target="_blank" runat="server"><i class="mdi-social-people" style="margin-left:0px !important;"></i></asp:HyperLink>
                                                    </div>
                                                    <div class="input-field col s12">
                                                        <div class="row">
                                                            <label for="txtLocation">Location Name <span class="reqd">*</span> </label>

                                                            <asp:TextBox ID="txtLocation" runat="server" onkeydown="return (event.keyCode!=13);" autocomplete="off"></asp:TextBox>

                                                            <asp:FilteredTextBoxExtender ID="txtLocation_FilteredTextBoxExtender" runat="server"
                                                                Enabled="False" FilterMode="InvalidChars" InvalidChars="'\" TargetControlID="txtLocation">
                                                            </asp:FilteredTextBoxExtender>

                                                            <asp:Button ID="btnSelectLoc" runat="server" CausesValidation="False" OnClick="btnSelectLoc_Click"
                                                                Style="display: none;" Text="Button" />
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ValidationGroup="addticketVG"
                                                                ControlToValidate="txtLocation" Display="None" ErrorMessage="Please select the location"
                                                                SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                            <asp:ValidatorCalloutExtender
                                                                ID="RequiredFieldValidator1_ValidatorCalloutExtender" runat="server" Enabled="True" PopupPosition="BottomLeft"
                                                                TargetControlID="RequiredFieldValidator1">
                                                            </asp:ValidatorCalloutExtender>

                                                            <asp:CustomValidator ID="CustomValidator2" runat="server" ClientValidationFunction="ChkLocation"
                                                                ControlToValidate="txtLocation" Display="None" ErrorMessage="Please select the location"
                                                                SetFocusOnError="True"></asp:CustomValidator>
                                                            <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender1" runat="server" Enabled="True"
                                                                TargetControlID="CustomValidator2">
                                                            </asp:ValidatorCalloutExtender>

                                                        </div>
                                                    </div>

                                                    <div class="srchclr multirowbtn multirowbtnone" style="float: right; margin-right: 40px !important;" title="Credit Hold">
                                                        <img id="imgCreditH" visible="false" runat="server" title="Credit Hold" src="images/MSCreditHold.png" style="width: 20px;">
                                                    </div>
                                                    <div class="srchclr btnlinksicon rowbtn">
                                                        <asp:HyperLink for="txtLocation" ID="lnkLocationID" Visible="true" Target="_blank" runat="server"><i class="mdi-communication-location-on" style="margin-left:0px !important;"></i></asp:HyperLink>
                                                    </div>

                                                    <div class="input-field col s12">
                                                        <div class="row">


                                                            <label for="txtAcctno">Location ID </label>
                                                            <asp:TextBox ID="txtAcctno" TabIndex="7" runat="server" Style="background-color: white;" ReadOnly="True" MaxLength="15"></asp:TextBox>

                                                        </div>
                                                    </div>


                                                    <div class="input-field col s12" id="dvCompanyPermission" runat="server">
                                                        <div class="row">

                                                            <label for="txtCompany">Company <span class="reqd">*</span></label>
                                                            <asp:TextBox ID="txtCompany" runat="server" Enabled="false"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="input-field col s12" style="display: none;">
                                                        <div class="row">

                                                            <label for="txtCustSageID">
                                                                <asp:Label ID="lblSageid" runat="server" Text="SageID"></asp:Label>
                                                                <span class="reqd">*</span></label>
                                                            <asp:TextBox ID="txtCustSageID" runat="server" class="validate" Width="137px" ReadOnly="True" MaxLength="10"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <label class="drpdwn-label" style="width: 100%">
                                                        Equipment    
                                                               <%-- <a causesvalidation="False" style="float: right; padding: 0 3px 0 15px;" runat="server" id="lblEqGrid">
                                                                    <i class="mdi-content-add-box" style="font-size: 2.2em; color: #1565c0; line-height: 24px;"></i>
                                                                </a>--%>
                                                    </label>
                                                    <div class="input-field col s12" style="margin-top: -10px;">
                                                        <div class="row">

                                                            <div class="tag-div materialize-textarea textarea-border" id="eqtag" style="">
                                                            </div>


                                                            <div id="DivEqup" class="popup_div " style="width: 1000px; z-index: 5">
                                                                <div class="btnlinks" style="margin-bottom: 5px;">
                                                                    <asp:HyperLink ID="HyperLinkAddEquip" Visible="true" Target="_blank" runat="server">Add New</asp:HyperLink>
                                                                </div>
                                                                <div class="btnlinks" style="margin-bottom: 5px; float: right;">
                                                                    <asp:LinkButton CausesValidation="false" ID="HideMELinkButton1" runat="server" OnClientClick="HideME();">Close</asp:LinkButton>
                                                                </div>


                                                                <div class="grid_container">
                                                                    <div class="RadGrid RadGrid_Material FormGrid">

                                                                        <telerik:RadAjaxPanel ID="RadAjaxPanelRadgvEquip" runat="server">

                                                                            <telerik:RadGrid ID="RadgvEquip"
                                                                                RenderMode="Auto" AllowFilteringByColumn="True" ShowFooter="false" PageSize="10"
                                                                                ShowStatusBar="true" runat="server" AllowPaging="false" AllowSorting="false" Width="100%" PagerStyle-AlwaysVisible="true"
                                                                                OnDataBound="gvEquip_DataBound"
                                                                                OnNeedDataSource="RadgvEquip_NeedDataSource"
                                                                                AllowCustomPaging="True">
                                                                                <CommandItemStyle />
                                                                                <GroupingSettings CaseSensitive="false" />
                                                                                <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                                                                                    <Selecting AllowRowSelect="True"></Selecting>
                                                                                    <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                                                                    <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                                                                </ClientSettings>
                                                                                <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="True" ShowFooter="True">

                                                                                    <Columns>

                                                                                        <telerik:GridTemplateColumn DataField="id" AutoPostBackOnFilter="false" AllowFiltering="false" SortExpression="id"
                                                                                            CurrentFilterFunction="Contains" UniqueName="id" HeaderText="" ShowFilterIcon="false" HeaderStyle-Width="30">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblID" runat="server" Style="display: none;" Text='<%# Bind("id") %>'></asp:Label>
                                                                                                <asp:CheckBox ID="chkSelect" runat="server" />
                                                                                            </ItemTemplate>
                                                                                            <HeaderTemplate>
                                                                                                <asp:CheckBox ID="chkAll" runat="server" />
                                                                                            </HeaderTemplate>
                                                                                        </telerik:GridTemplateColumn>
                                                                                        <telerik:GridTemplateColumn DataField="unit" AutoPostBackOnFilter="false" AllowFiltering="false" SortExpression="unit"
                                                                                            CurrentFilterFunction="Contains" UniqueName="unit" HeaderText="Name" ShowFilterIcon="false" HeaderStyle-Width="150">

                                                                                            <ItemTemplate>
                                                                                                <a href="addequipment.aspx?uid=<%# Eval("id") %>" target="_blank" style="color: white">
                                                                                                    <asp:Label ID="lblUnit" runat="server" Text='<%# Bind("unit") %>'></asp:Label></a>
                                                                                            </ItemTemplate>
                                                                                        </telerik:GridTemplateColumn>

                                                                                        <telerik:GridTemplateColumn DataField="state" AutoPostBackOnFilter="false" AllowFiltering="false" SortExpression="state"
                                                                                            CurrentFilterFunction="Contains" UniqueName="state" HeaderText="Unique#" ShowFilterIcon="True" HeaderStyle-Width="150">

                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblUID" runat="server"><%#Eval("state")%></asp:Label>
                                                                                            </ItemTemplate>
                                                                                        </telerik:GridTemplateColumn>

                                                                                        <telerik:GridTemplateColumn DataField="fdesc" AutoPostBackOnFilter="false" AllowFiltering="false" SortExpression="fdesc"
                                                                                            CurrentFilterFunction="Contains" UniqueName="fdesc" HeaderText="Description" ShowFilterIcon="false" HeaderStyle-Width="150">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblDesc" runat="server"><%#Eval("fdesc")%></asp:Label>
                                                                                            </ItemTemplate>
                                                                                        </telerik:GridTemplateColumn>

                                                                                        <telerik:GridTemplateColumn DataField="Type" AutoPostBackOnFilter="false" AllowFiltering="false" SortExpression="Type"
                                                                                            CurrentFilterFunction="Contains" UniqueName="Type" HeaderText="Type" ShowFilterIcon="false" HeaderStyle-Width="100">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblType" runat="server"><%#Eval("Type")%></asp:Label>
                                                                                            </ItemTemplate>
                                                                                        </telerik:GridTemplateColumn>

                                                                                        <telerik:GridTemplateColumn DataField="category" AutoPostBackOnFilter="false" AllowFiltering="false" SortExpression="id"
                                                                                            CurrentFilterFunction="Contains" UniqueName="category" HeaderText="Category" ShowFilterIcon="false" HeaderStyle-Width="150">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblcat" runat="server"><%#Eval("category")%></asp:Label>
                                                                                            </ItemTemplate>
                                                                                        </telerik:GridTemplateColumn>
                                                                                        <telerik:GridTemplateColumn DataField="cat" AutoPostBackOnFilter="false" AllowFiltering="false" SortExpression="cat"
                                                                                            CurrentFilterFunction="Contains" UniqueName="cat" HeaderText="ServiceType" ShowFilterIcon="false" HeaderStyle-Width="150">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblServiceType" runat="server"><%#Eval("cat")%></asp:Label>
                                                                                            </ItemTemplate>
                                                                                        </telerik:GridTemplateColumn>
                                                                                        <telerik:GridTemplateColumn DataField="Building" AutoPostBackOnFilter="false" AllowFiltering="false" SortExpression="Building"
                                                                                            CurrentFilterFunction="Contains" UniqueName="Building" HeaderText="Building" ShowFilterIcon="false" HeaderStyle-Width="150">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblbuilding" runat="server"><%#Eval("building")%></asp:Label>
                                                                                            </ItemTemplate>
                                                                                        </telerik:GridTemplateColumn>
                                                                                        <telerik:GridTemplateColumn DataField="status" AutoPostBackOnFilter="false" AllowFiltering="false" SortExpression="status"
                                                                                            CurrentFilterFunction="Contains" UniqueName="status" HeaderText="Status" ShowFilterIcon="false" HeaderStyle-Width="100">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblStatus" runat="server"><%# Convert.ToInt32( Eval("status")) == 0 ? "Active" : "Inactive"%></asp:Label>
                                                                                            </ItemTemplate>
                                                                                        </telerik:GridTemplateColumn>
                                                                                        <telerik:GridTemplateColumn DataField="shut_down" AutoPostBackOnFilter="false" AllowFiltering="false" SortExpression="shut_down"
                                                                                            CurrentFilterFunction="Contains" UniqueName="shut_down" HeaderText="Shut Down" ShowFilterIcon="false" HeaderStyle-Width="100">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblShutdown" runat="server"><%#Eval("shut_down")%></asp:Label>
                                                                                            </ItemTemplate>
                                                                                        </telerik:GridTemplateColumn>
                                                                                        <telerik:GridTemplateColumn DataField="ShutdownReason" AutoPostBackOnFilter="false" AllowFiltering="false" SortExpression="ShutdownReason"
                                                                                            CurrentFilterFunction="Contains" UniqueName="ShutdownReason" HeaderText="Shut Down Description" ShowFilterIcon="false" HeaderStyle-Width="100">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblShutdownReason" runat="server"><%#Eval("ShutdownReason")%></asp:Label>
                                                                                            </ItemTemplate>
                                                                                        </telerik:GridTemplateColumn>
                                                                                        <telerik:GridTemplateColumn AutoPostBackOnFilter="false" AllowFiltering="false"
                                                                                            CurrentFilterFunction="Contains" HeaderText="%Hours" ShowFilterIcon="false" HeaderStyle-Width="100">
                                                                                            <ItemTemplate>
                                                                                                <asp:TextBox ID="txtHours" runat="server" Width="50px" MaxLength="20"></asp:TextBox>
                                                                                            </ItemTemplate>
                                                                                        </telerik:GridTemplateColumn>
                                                                                        <telerik:GridTemplateColumn AutoPostBackOnFilter="false" AllowFiltering="false"
                                                                                            CurrentFilterFunction="Contains" HeaderText="Contract_Info" ShowFilterIcon="false" HeaderStyle-Width="150">
                                                                                            <ItemTemplate>
                                                                                                <asp:LinkButton Visible="false" ID="lnkbtnEuipContractinfo" CommandName="1" CommandArgument='<%# Bind("id") %>' runat="server" CausesValidation="False" OnClick="btnEquipContractinfo_Click">
                                                                                    Multiple Contract 
                                                                                                </asp:LinkButton>
                                                                                                <asp:Label Style="font-size: 11px; color: #1565C0;" ID="EquipContractinfo" runat="server"></asp:Label>


                                                                                            </ItemTemplate>
                                                                                            <FooterTemplate><%= RadgvEquip.VirtualItemCount  %>  Record(s) Found.   </FooterTemplate>
                                                                                        </telerik:GridTemplateColumn>
                                                                                    </Columns>
                                                                                    <NoRecordsTemplate>No Record Found.   </NoRecordsTemplate>
                                                                                </MasterTableView>
                                                                            </telerik:RadGrid>

                                                                        </telerik:RadAjaxPanel>

                                                                    </div>
                                                                </div>
                                                            </div>


                                                            <asp:TextBox ID="txtUnit" runat="server" Style="display: none;"></asp:TextBox>
                                                            <asp:Button ID="btnEquip" CausesValidation="False" OnClick="btnEquip_Click" Style="display: none" runat="server" Text="Button" />

                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="form-section3-blank">
                                                    &nbsp;
                                                </div>
                                                <div class="form-section3">
                                                    <div class="section-ttle">Location Info</div>
                                                    <div class="nlne text-field col s12" id="divlocContractinfo" runat="server">
                                                        <div class="row txtrow">
                                                            <label id="locContractinfo" runat="server"></label>
                                                            <asp:LinkButton ID="lnkbtnlocContractinfo" runat="server" Style="color: #1565C0; margin-left: -15px; text-decoration: underline;" Visible="false" CausesValidation="False" OnClick="btnlocContractinfo_Click"> Multiple Contract   
                                                            </asp:LinkButton>
                                                        </div>
                                                    </div>

                                                    <%--Button for Multiple Contract Info--%>


                                                    <div class="input-field col s12">
                                                        <div class="row">
                                                            <div>
                                                                <label id="lblgoogleloc" for="txtGoogleAutoc" class="drpdwn-label">Location Address</label>
                                                            </div>
                                                            <asp:TextBox TextMode="MultiLine" CssClass="materialize-textarea" ID="txtGoogleAutoc" runat="server" placeholder=""
                                                                onkeyup="return checkMaxLength(this, event, 35)"></asp:TextBox>
                                                        </div>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ControlToValidate="txtGoogleAutoc"
                                                            Display="None" ErrorMessage="Address Required" SetFocusOnError="True"></asp:RequiredFieldValidator><asp:ValidatorCalloutExtender
                                                                ID="RequiredFieldValidator11_ValidatorCalloutExtender" runat="server" Enabled="True"
                                                                TargetControlID="RequiredFieldValidator11">
                                                            </asp:ValidatorCalloutExtender>
                                                        <%--  <a id="mapLink" style="display: none">
                                                            <img src="images/map.ico" title="Map" class="shadowHover" width="20px" height="20px" />
                                                        </a>
                                                        <span id="spnAddress" style="color: red; 
                                                            s: none;">Max 2 lines 30 characters each</span>--%>
                                                    </div>

                                                    <div class="input-field col s5">
                                                        <div class="row">
                                                            <asp:TextBox ID="txtCity" runat="server" MaxLength="50"
                                                                TabIndex="15"></asp:TextBox>
                                                            <label for="txtCity">City</label>

                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ValidationGroup="addticketVG" ControlToValidate="txtCity"
                                                                Display="None" ErrorMessage="City Required" SetFocusOnError="True"></asp:RequiredFieldValidator>

                                                            <asp:ValidatorCalloutExtender
                                                                ID="RequiredFieldValidator6_ValidatorCalloutExtender" runat="server" Enabled="True"
                                                                TargetControlID="RequiredFieldValidator6">
                                                            </asp:ValidatorCalloutExtender>
                                                        </div>
                                                    </div>
                                                    <div class="input-field col s2">
                                                        <div class="row">
                                                            &nbsp;
                                                        </div>
                                                    </div>
                                                    <div class="input-field col s5">
                                                        <div class="row">
                                                            <label >State/Province</label>

                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ValidationGroup="addticketVG"
                                                                ControlToValidate="ddlState" Display="None" ErrorMessage="State Required"
                                                                SetFocusOnError="True"></asp:RequiredFieldValidator>

                                                            <asp:ValidatorCalloutExtender
                                                                ID="RequiredFieldValidator7_ValidatorCalloutExtender" runat="server" Enabled="True"
                                                                TargetControlID="RequiredFieldValidator7">
                                                            </asp:ValidatorCalloutExtender>
                                                               <asp:TextBox ID="ddlState" runat="server" MaxLength="2"
                                                                TabIndex="20"></asp:TextBox>
                                                        <%--    <asp:DropDownList ID="ddlState" runat="server" CssClass="browser-default" TabIndex="19"
                                                                ToolTip="State">
                                                                <asp:ListItem Value="State">State</asp:ListItem>
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
                                                            </asp:DropDownList>--%>
                                                        </div>
                                                    </div>
                                                    <div class="input-field col s5">
                                                        <div class="row">
                                                            <asp:TextBox ID="txtZip" runat="server" MaxLength="10"
                                                                TabIndex="21"></asp:TextBox>
                                                            <label for="txtZip">Zip/Postal Code</label>
                                                        </div>
                                                    </div>
                                                    <div class="input-field col s2">
                                                        <div class="row">
                                                            &nbsp;
                                                        </div>
                                                    </div>
                                                    <div class="input-field col s5">
                                                        <div class="row">
                                                            <label for="txtCountry">Country</label>
                                                            <asp:TextBox ID="txtCountry" runat="server"></asp:TextBox>
                                                        </div>
                                                    </div>


                                                </div>
                                                <div class="form-section3-blank">
                                                    &nbsp;
                                                </div>
                                                <div class="form-section3">
                                                    <div class="section-ttle">Main Contact Info</div>
                                                    <div class="input-field col s5">
                                                        <div class="row">
                                                            <asp:TextBox ID="txtMaincontact" runat="server" MaxLength="50"
                                                                TabIndex="25"></asp:TextBox>
                                                            <label for="txtMaincontact">Main Contact</label>
                                                        </div>
                                                    </div>
                                                    <div class="input-field col s2">
                                                        <div class="row">
                                                            &nbsp;
                                                        </div>
                                                    </div>
                                                    <div class="input-field col s5">
                                                        <div class="row">
                                                            <asp:TextBox ID="txtNameWho" runat="server" MaxLength="30"
                                                                TabIndex="30"></asp:TextBox>
                                                            <label for="txtNameWho">Caller<span class="reqd">*</span></label>

                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator31" runat="server" ValidationGroup="addticketVG"
                                                                ControlToValidate="txtNameWho" Display="None" ErrorMessage="Caller Required" SetFocusOnError="True">

                                                            </asp:RequiredFieldValidator>

                                                            <asp:ValidatorCalloutExtender
                                                                ID="RequiredFieldValidator31_ValidatorCalloutExtender" runat="server" Enabled="True"
                                                                PopupPosition="BottomLeft" TargetControlID="RequiredFieldValidator31">
                                                            </asp:ValidatorCalloutExtender>
                                                        </div>
                                                    </div>
                                                    <div class="input-field col s5">
                                                        <div class="row">
                                                            <asp:TextBox ID="txtPhoneCust" runat="server" MaxLength="28"
                                                                Style="padding: 1px 1px 1px 1px;"
                                                                TabIndex="28"></asp:TextBox>
                                                            <label for="txtPhoneCust">Contact Phone</label>
                                                        </div>
                                                    </div>
                                                    <div class="input-field col s2">
                                                        <div class="row">
                                                            &nbsp;
                                                        </div>
                                                    </div>
                                                    <div class="input-field col s5">
                                                        <div class="row">
                                                            <asp:TextBox ID="txtCell" runat="server" MaxLength="28"
                                                                TabIndex="32"></asp:TextBox>
                                                            <label for="txtCell">Caller Phone</label>
                                                        </div>
                                                    </div>
                                                    <div class="input-field col s12">
                                                        <div class="row">
                                                            <label class="label">Entered By</label>
                                                            <asp:TextBox ID="txtFby" CssClass="browser-default" runat="server"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="input-field2 col s5">
                                                        <div class="row">
                                                            <label class="date-label">Date Called In<span class="reqd">*</span></label>
                                                            <asp:TextBox ID="txtCallDt" runat="server" class="datepicker_mom" MaxLength="28"
                                                                TabIndex="17"></asp:TextBox>


                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator23" runat="server" ControlToValidate="txtCallDt" Display="None"
                                                                ErrorMessage="Date Called In Required" ValidationGroup="addticketVG" SetFocusOnError="True"></asp:RequiredFieldValidator>

                                                            <asp:ValidatorCalloutExtender ID="RequiredFieldValidator23_ValidatorCalloutExtender"
                                                                runat="server" Enabled="True" PopupPosition="BottomLeft" TargetControlID="RequiredFieldValidator23">
                                                            </asp:ValidatorCalloutExtender>



                                                        </div>
                                                    </div>


                                                    <div class="input-field col s2">
                                                        <div class="row">
                                                            &nbsp;
                                                        </div>
                                                    </div>

                                                    <div class="input-field col s5">
                                                        <div class="row">
                                                            <asp:TextBox ID="txtCallTime" class="validate" runat="server" TabIndex="18" MaxLength="28"></asp:TextBox>

                                                            <asp:RequiredFieldValidator ValidationGroup="addticketVG" ID="txtCallTime_Requiredfieldvalidator" runat="server" ControlToValidate="txtCallTime" Display="None" ErrorMessage="Time is invalid" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                            <asp:ValidatorCalloutExtender ID="txtCallTime_ValidatorCalloutExtender"
                                                                runat="server" Enabled="True" PopupPosition="Left" TargetControlID="txtCallTime_Requiredfieldvalidator">
                                                            </asp:ValidatorCalloutExtender>
                                                            <label>Called In Time<span class="reqd">*</span></label>


                                                            <asp:MaskedEditExtender ID="MaskedEditExtender5" runat="server" AcceptAMPM="True"
                                                                CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat=""
                                                                CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                                                CultureTimePlaceholder="" Enabled="True" Mask="99:99" MaskType="Time" TargetControlID="txtCallTime">
                                                            </asp:MaskedEditExtender>

                                                            <asp:MaskedEditValidator ID="MaskedEditValidator5" runat="server" ControlExtender="MaskedEditExtender5"
                                                                ControlToValidate="txtCallTime" Display="None" ErrorMessage="MaskedEditExtender5"
                                                                InvalidValueMessage="Time is invalid" SetFocusOnError="True"></asp:MaskedEditValidator>
                                                        </div>
                                                    </div>

                                                    <div class="input-field col s5">
                                                        <div class="row">
                                                            <label class="drpdwn-label" id="lblDefaultWorker" runat="server"></label>


                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator34" runat="server" ControlToValidate="ddlDefRoute"
                                                                Display="None" ErrorMessage="Required" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                            <asp:ValidatorCalloutExtender ID="RequiredFieldValidator34_ValidatorCalloutExtender"
                                                                runat="server" Enabled="True" PopupPosition="Left" TargetControlID="RequiredFieldValidator34">
                                                            </asp:ValidatorCalloutExtender>

                                                            <asp:DropDownList ID="ddlDefRoute" runat="server" Enabled="false" TabIndex="37" Style="margin-left: 3px" CssClass="browser-default"
                                                                onchange="CallNearestWorker();">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                    <div class="input-field col s2">
                                                        <div class="row">
                                                            &nbsp;
                                                        </div>
                                                    </div>
                                                    <div class="input-field col s5">
                                                        <div class="row">
                                                            <label class="drpdwn-label" runat="server" id="lblZone">Zone</label>
                                                            <asp:DropDownList ID="ddlZone" runat="server" Enabled="false" CssClass="browser-default">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>

                                                </div>
                                            </div>



                                            <div class="form-section-row">

                                                <div class="form-section3">
                                                    <div class="section-ttle">Service Info</div>
                                                    <div class="input-field col s5">
                                                        <div class="row">
                                                            <label class="drpdwn-label">
                                                                Category <span class="reqd">*</span>

                                                            </label>

                                                            <asp:DropDownList ID="ddlCategory" runat="server" CssClass="browser-default" AutoPostBack="True" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged"
                                                                TabIndex="16">
                                                            </asp:DropDownList>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator20" runat="server" ValidationGroup="addticketVG"
                                                                ControlToValidate="ddlCategory" Display="None" ErrorMessage="Category Required"
                                                                SetFocusOnError="True"></asp:RequiredFieldValidator>

                                                            <asp:ValidatorCalloutExtender
                                                                PopupPosition="BottomLeft"
                                                                ID="RequiredFieldValidator20_ValidatorCalloutExtender" runat="server" Enabled="True"
                                                                TargetControlID="RequiredFieldValidator20">
                                                            </asp:ValidatorCalloutExtender>
                                                        </div>
                                                        <div class="srchclr btnlinksicon rowbtn">
                                                            <asp:Image ID="imgCategory" runat="server" AlternateText="Icon" ToolTip="Category Icon"
                                                                Width="25px" />
                                                        </div>
                                                    </div>

                                                    <div class="input-field col s2">
                                                        &nbsp;
                                                    </div>
                                                    <div class="input-field col s5">
                                                        <div class="row">
                                                            <label class="drpdwn-label">Level</label>
                                                            <asp:DropDownList ID="ddlLevel" runat="server" CssClass="browser-default"></asp:DropDownList>
                                                        </div>
                                                    </div>
                                                    <div class="input-field col s12" style="margin-top: 20px!important">


                                                        <div class="row">

                                                            <asp:LinkButton OnClick="btnCodes_Click" CausesValidation="False" Style="float: right; padding: 0 0px 0 0px;" runat="server" ID="btnCodes">
                                                                <i class="mdi-content-add-box" style="font-size: 1.5em; color: #1565c0; line-height: 20px;"></i>
                                                            </asp:LinkButton>



                                                            <label for="txtReason" class="drpdwn-label">
                                                                Reason for service<span class="reqd">*</span>
                                                            </label>
                                                            <asp:TextBox ID="txtReason" runat="server" Height="120"
                                                                TextMode="MultiLine" Style="padding: 0.4rem 0 !important"
                                                                CssClass="materialize-textarea textarea-border"></asp:TextBox>





                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator30" runat="server" ControlToValidate="txtReason" ValidationGroup="addticketVG"
                                                                Display="None" ErrorMessage="Reason for Service Required" SetFocusOnError="True"></asp:RequiredFieldValidator>

                                                            <asp:ValidatorCalloutExtender ID="RequiredFieldValidator30_ValidatorCalloutExtender"
                                                                runat="server" Enabled="True" PopupPosition="BottomLeft" TargetControlID="RequiredFieldValidator30">
                                                            </asp:ValidatorCalloutExtender>
                                                        </div>

                                                    </div>
                                                </div>
                                                <div class="form-section3-blank">
                                                    &nbsp;
                                                </div>
                                                <div class="form-section3">
                                                    <div class="section-ttle">
                                                        Worker Info
                                                                <span style="float: right;">

                                                                    <asp:CheckBox ID="chkCreateMultipleTicket" class="css-checkbox" runat="server"
                                                                        AutoPostBack="false"
                                                                        TabIndex="14" Text="Multiple Ticket" />
                                                                    <asp:LinkButton ID="btnchkCreateMultipleTicket" Style="display: none;" runat="server" CausesValidation="false" OnClick="btnchkCreateMultipleTicket_Click"></asp:LinkButton>

                                                                </span>
                                                    </div>

                                                    <div class="input-field2 col s5">
                                                        <div class="row">
                                                            <label class="date-label">Date Scheduled<span class="reqd">*</span></label>

                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator24" runat="server" ValidationGroup="addticketVG"
                                                                ControlToValidate="txtSchDt" Display="None" ErrorMessage="Date Scheduled Required"
                                                                SetFocusOnError="True"></asp:RequiredFieldValidator>

                                                            <asp:ValidatorCalloutExtender
                                                                ID="RequiredFieldValidator24_ValidatorCalloutExtender" runat="server" Enabled="True"
                                                                PopupPosition="BottomLeft" TargetControlID="RequiredFieldValidator24">
                                                            </asp:ValidatorCalloutExtender>

                                                            <asp:TextBox ID="txtSchDt" runat="server" class="datepicker_mom" MaxLength="28"
                                                                TabIndex="23"></asp:TextBox>

                                                        </div>
                                                    </div>

                                                    <div class="input-field col s2">
                                                        <div class="row">
                                                            &nbsp;
                                                        </div>
                                                    </div>
                                                    <div class="input-field col s5">
                                                        <div class="row">

                                                            <label>Time<span class="reqd">*</span></label>
                                                            <asp:TextBox ID="txtSchTime" class="validate" TabIndex="24" runat="server" MaxLength="28"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator_txtSchTime" runat="server" ValidationGroup="addticketVG"
                                                                ControlToValidate="txtSchTime" ErrorMessage="Time is invalid" SetFocusOnError="True">

                                                            </asp:RequiredFieldValidator>
                                                            <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender3"
                                                                runat="server" Enabled="True" PopupPosition="BottomLeft" TargetControlID="RequiredFieldValidator_txtSchTime">
                                                            </asp:ValidatorCalloutExtender>



                                                            <asp:MaskedEditExtender ID="MaskedEditExtender4" runat="server" AcceptAMPM="True"
                                                                CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat=""
                                                                CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                                                CultureTimePlaceholder="" Enabled="True" Mask="99:99" MaskType="Time" TargetControlID="txtSchTime">
                                                            </asp:MaskedEditExtender>

                                                            <asp:MaskedEditValidator ID="MaskedEditValidator3" runat="server" ControlExtender="MaskedEditExtender4"
                                                                ControlToValidate="txtSchTime" Display="None" ErrorMessage="MaskedEditExtender4"
                                                                InvalidValueMessage="Time is invalid" SetFocusOnError="True"></asp:MaskedEditValidator>

                                                        </div>
                                                    </div>

                                                    <div class="input-field2 col s5">
                                                        <div class="row">

                                                            <label for="txtEST">Estimate</label>
                                                            <asp:TextBox ID="txtEST" runat="server" CssClass="validate" MaxLength="28"
                                                                TabIndex="36">00.00</asp:TextBox>
                                                            <asp:MaskedEditExtender ID="txtEST_MaskedEditExtender" runat="server" Mask="99.99"
                                                                MaskType="Number" TargetControlID="txtEST" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                                                CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                                                CultureTimePlaceholder="" Enabled="True">
                                                            </asp:MaskedEditExtender>
                                                        </div>
                                                    </div>
                                                    <div class="input-field col s2">
                                                        <div class="row">
                                                            &nbsp;
                                                        </div>
                                                    </div>

                                                    <div class="input-field col s5" id="divForCreateMultipleTicket" runat="server" visible="false">
                                                        <div class="row">
                                                            <label for="txtDays">Days</label>
                                                            <asp:TextBox ID="txtDays" TextMode="Number" Text="1" runat="server" CssClass="validate"></asp:TextBox>
                                                            <asp:FilteredTextBoxExtender ID="txtDays_FilteredTextBoxExtender" runat="server"
                                                                Enabled="true" FilterType="Numbers" ValidChars="0123456789" TargetControlID="txtDays">
                                                            </asp:FilteredTextBoxExtender>
                                                            <asp:RequiredFieldValidator ID="rf12" runat="server"
                                                                ControlToValidate="txtDays" ErrorMessage="Days Required"
                                                                SetFocusOnError="True"></asp:RequiredFieldValidator><asp:ValidatorCalloutExtender
                                                                    ID="ValidatorCalloutExtender10" runat="server" Enabled="True"
                                                                    PopupPosition="Right" TargetControlID="rf12">
                                                                </asp:ValidatorCalloutExtender>

                                                        </div>
                                                    </div>
                                                    <div class="input-field col s12" runat="server" id="divForCreateMultipleTicket3" visible="true">
                                                        <div class="row">
                                                            <label class="drpdwn-label" style="width: 100%">
                                                                Assigned Worker<span class="reqd">*</span></label>

                                                            <asp:DropDownList ID="ddlRoute1" runat="server" CssClass="browser-default" TabIndex="30"
                                                                AutoPostBack="True" OnSelectedIndexChanged="ddlRoute1_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>

                                                    <div class="input-field col s12 mgntp10" runat="server" id="divForCreateMultipleTicket2" visible="false">
                                                        <div class="row">
                                                            <label class="drpdwn-label">
                                                                Assigned Worker</label>

                                                            <telerik:RadComboBox ID="chkcatlist" BackColor="White" CssClass="browser-default" runat="server" CheckBoxes="true" Width="102%"
                                                                EnableCheckAllItemsCheckBox="true">
                                                            </telerik:RadComboBox>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server"
                                                                ControlToValidate="chkcatlist" ErrorMessage="Worker Required"
                                                                SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                            <asp:ValidatorCalloutExtender
                                                                ID="ValidatorCalloutExtender11" runat="server" Enabled="True"
                                                                PopupPosition="Right" TargetControlID="RequiredFieldValidator3">
                                                            </asp:ValidatorCalloutExtender>

                                                        </div>
                                                    </div>
                                                    <div class="input-field col s12">
                                                        <div class="row">
                                                            <label class="drpdwn-label">Status</label>

                                                            <asp:DropDownList ID="ddlStatus1" runat="server" AutoPostBack="True" CssClass="browser-default"
                                                                OnSelectedIndexChanged="ddlStatus1_SelectedIndexChanged">
                                                                <asp:ListItem Value="0">Un-Assigned</asp:ListItem>
                                                                <asp:ListItem Value="1">Assigned</asp:ListItem>
                                                                <asp:ListItem Value="2">Enroute</asp:ListItem>
                                                                <asp:ListItem Value="3">Onsite</asp:ListItem>
                                                                <asp:ListItem Value="4">Completed</asp:ListItem>
                                                                <asp:ListItem Value="5">Hold</asp:ListItem>
                                                                <asp:ListItem Value="6" disabled="disabled">Voided</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="form-section3-blank">
                                                    &nbsp;
                                                </div>

                                                <div class="form-section3">
                                                    <div class="section-ttle">Additional Info</div>
                                                    <div>
                                                        <div class="input-field col s4">
                                                            <div class="checkrow">
                                                                <asp:CheckBox ID="chkDispAlert" Text="Dispatch Alert" CssClass="css-checkbox" runat="server" />

                                                            </div>
                                                        </div>
                                                        <div class="input-field col s4" style="float: right !important;">
                                                            <div class="checkrow">

                                                                <asp:CheckBox ID="chkIsRecurring" Text="Recurring" CssClass="css-checkbox" runat="server" />
                                                            </div>
                                                        </div>
                                                        <div class="input-field col s4" style="float: right !important;">
                                                            <div class="checkrow">

                                                                <asp:CheckBox ID="chkCreditHold" Text="Credit Hold" CssClass="css-checkbox" runat="server" />
                                                            </div>
                                                        </div>
                                                    </div>

                                                    <div class="input-field col s12" style="margin-top: 20px !important;">
                                                        <div class="row">
                                                            <asp:TextBox ID="txtCreditReason" runat="server" class="materialize-textarea" TextMode="MultiLine" ToolTip="Reason"></asp:TextBox>
                                                            <label for="txtCreditReason">Reason</label>
                                                        </div>
                                                    </div>
                                                    <div class="input-field col s12">
                                                        <div class="row">
                                                            <%--<asp:TextBox ID="txtRemarks" runat="server" CssClass="materialize-textarea"
                                                                MaxLength="8000"
                                                                TextMode="MultiLine"></asp:TextBox>--%>
                                                            <asp:TextBox ID="txtRemarks" runat="server" MaxLength="8000" Height="120"
                                                                TextMode="MultiLine" Style="padding: 0.4rem 0 !important"
                                                                CssClass="materialize-textarea textarea-border"></asp:TextBox>
                                                            <label for="txtRemarks" class="txtbrdlbl">
                                                                <asp:Label ID="lblRemarks" runat="server" Text="Location Remarks" Visible="False"></asp:Label>
                                                            </label>
                                                            
                                                        </div>
                                                    </div>

                                                </div>


                                            </div>
                                            <div class="cf"></div>
                                        </div>
                                    </telerik:RadAjaxPanel>
                                </div>
                            </div>


                        </li>
                        <li>


                            <div id="accrdcompleted" class="collapsible-header accrd accordian-text-custom"><i class="mdi-action-perm-data-setting"></i>Completed 
                                <asp:Label ID="lblaccrdcompleted" runat="server" ForeColor="Red" Visible="false">(You do not have completed ticket permissions!)</asp:Label></div>





                            <div class="collapsible-body">
                                <div class="form-content-wrap">
                                    <telerik:RadAjaxPanel ID="RadAjaxPanelCompleted" runat="server">
                                        <div class="form-content-pd" id="DIVaccrdcompleted" runat="server">
                                            <div class="form-section-row">
                                                <div class="input-field s12" style="margin-top: 0px !important; position: relative; margin-bottom: 50px;">


                                                    <div class="form-section3">
                                                        <div class="input-field col s4">
                                                            <div class="checkrow">
                                                                <asp:CheckBox ID="chkWorkComp" runat="server" Text="Work Complete" class="css-checkbox" Checked="True" TabIndex="11" />
                                                            </div>
                                                        </div>

                                                        <div class="input-field col s4">
                                                            <div class="checkrow">
                                                                <asp:CheckBox ID="chkReviewed" runat="server" class="css-checkbox" TabIndex="10" Text="Reviewed" />
                                                            </div>
                                                        </div>

                                                        <div class="input-field col s4">
                                                            <div class="checkrow">
                                                                <asp:CheckBox ID="chkChargeable" runat="server" class="css-checkbox" TabIndex="9" Text="Chargeable" onclick="ChargeableCheck()" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="form-section3-blank">
                                                        &nbsp;
                                                    </div>
                                                    <div class="form-section3">

                                                        <div class="input-field col s4">
                                                            <div class="checkrow">
                                                                <asp:CheckBox ID="chkInvoice" runat="server" class="css-checkbox" TabIndex="12" Text="Invoice" />
                                                            </div>
                                                        </div>
                                                        <div class="input-field col s4">
                                                            <div class="checkrow">
                                                                <asp:CheckBox ID="chkTimeTrans" runat="server" class="css-checkbox" TabIndex="13" Text="Timesheet" />

                                                            </div>
                                                        </div>
                                                        <div class="input-field col s4">
                                                            <div class="checkrow">

                                                                <asp:CheckBox ID="chkInternet" runat="server" class="css-checkbox" TabIndex="8" Text="Internet" />

                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="form-section3-blank">
                                                        &nbsp;
                                                    </div>

                                                    <div class="form-section3-blank">
                                                        &nbsp;
                                                    </div>
                                                    <div class="form-section3">
                                                    </div>
                                                </div>
                                                <div class="form-section3">
                                                    <div class="section-ttle">Work Description</div>

                                                    <div class="input-field col s12">
                                                        <div class="row">


                                                            <span style="float: right;">
                                                                <asp:LinkButton OnClick="btnCodesCmpl_Click" Visible="False" CausesValidation="False" runat="server" ID="btnCodesCmpl">
                                                                <i class="mdi-content-add-box" style="font-size: 1.5em; color: #1565c0;"></i>
                                                                </asp:LinkButton>
                                                            </span>

                                                            <asp:TextBox ID="txtWorkCompl" runat="server"
                                                                TextMode="MultiLine" Style="padding: 0.4rem 0 !important"
                                                                CssClass="materialize-textarea textarea-border" TabIndex="39"></asp:TextBox>
                                                            <label for="txtWorkCompl" class="txtbrdlbl">
                                                                Work Complete Desc.<span class="reqd">*</span>
                                                            </label>


                                                            <asp:Label ID="lblWCD" runat="server" Visible="False"></asp:Label>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator28" runat="server" ControlToValidate="txtWorkCompl"
                                                                Display="None" Enabled="False" ErrorMessage="Work complete description Required" ValidationGroup="addticketVG"
                                                                SetFocusOnError="True" Style="z-index: 9999 !important;"></asp:RequiredFieldValidator>

                                                            <asp:ValidatorCalloutExtender ID="RequiredFieldValidator28_ValidatorCalloutExtender"
                                                                runat="server" Enabled="True" PopupPosition="BottomLeft" TargetControlID="RequiredFieldValidator28">
                                                            </asp:ValidatorCalloutExtender>

                                                        </div>
                                                    </div>
                                                    <div>
                                                        <label class="drpdwn-label">Signature</label>
                                                    </div>
                                                    <div class="input-field col s12">
                                                        <div class="row">
                                                            <div id="signbg" style="border: 1px solid #9e9e9e; width: 100%; height: 123px; border-radius: 5px;">
                                                                <asp:Image ID="imgSign" runat="server" Style="width: 99%; max-height: 99%;" />
                                                            </div>


                                                        </div>
                                                    </div>

                                                </div>




                                                <div class="form-section3-blank">
                                                    &nbsp;
                                                </div>
                                                <div class="form-section3">

                                                    <div class="section-ttle">
                                                        Timestamps
                                                    </div>

                                                    <div class="input-field col s5">
                                                        <div class="row">
                                                            <label class="drpdwn-label">Assigned Worker<span class="reqd">*</span></label>

                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator25" runat="server" ValidationGroup="addticketVG"
                                                                ControlToValidate="ddlRoute" Display="None" Enabled="False" ErrorMessage="Worker Required"
                                                                SetFocusOnError="True"></asp:RequiredFieldValidator>

                                                            <asp:ValidatorCalloutExtender
                                                                ID="RequiredFieldValidator25_ValidatorCalloutExtender" runat="server" Enabled="True"
                                                                PopupPosition="BottomLeft" TargetControlID="RequiredFieldValidator25">
                                                            </asp:ValidatorCalloutExtender>
                                                            <asp:Label ID="lblWorkStatus" runat="server" Font-Size="Smaller" ForeColor="Red"></asp:Label>

                                                            <asp:DropDownList ID="ddlRoute" runat="server" CssClass="browser-default" TabIndex="30"
                                                                AutoPostBack="True" OnSelectedIndexChanged="ddlRoute_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                    <div class="input-field col s2">
                                                        <div class="row">
                                                            &nbsp;
                                                        </div>
                                                    </div>
                                                    <div class="input-field col s5">
                                                        <div class="row">
                                                            <label class="drpdwn-label">Status</label>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator22" runat="server"
                                                                ControlToValidate="ddlStatus" Display="None" ErrorMessage="Status Required" SetFocusOnError="True"></asp:RequiredFieldValidator><asp:ValidatorCalloutExtender
                                                                    ID="RequiredFieldValidator22_ValidatorCalloutExtender" runat="server" Enabled="True"
                                                                    PopupPosition="Left" TargetControlID="RequiredFieldValidator22">
                                                                </asp:ValidatorCalloutExtender>


                                                            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="browser-default">
                                                                <asp:ListItem Value="0">Un-Assigned</asp:ListItem>
                                                                <asp:ListItem Value="1">Assigned</asp:ListItem>
                                                                <asp:ListItem Value="2">Enroute</asp:ListItem>
                                                                <asp:ListItem Value="3">Onsite</asp:ListItem>
                                                                <asp:ListItem Value="4">Completed</asp:ListItem>
                                                                <asp:ListItem Value="5">Hold</asp:ListItem>
                                                                <asp:ListItem Value="6" disabled="disabled">Voided</asp:ListItem>
                                                            </asp:DropDownList>
                                                            <asp:LinkButton ID="lnkddlStatus" OnClick="lnkddlStatus_Click" runat="server" CausesValidation="false"> </asp:LinkButton>

                                                        </div>
                                                    </div>

                                                    <telerik:RadAjaxPanel ID="RadAjaxPanelTimestamps" runat="server">
                                                        <div class="section-ttle" style="margin-top: 15px;">
                                                            Time Spent
                                                                <span style="float: right;">
                                                                    <label style="height: 20px !important; font-size: 1em !important;">
                                                                        Total:<asp:Label ID="lblTotal" runat="server" Style="font-weight: bold;">0.0</asp:Label>
                                                                        Hrs</label>
                                                                </span>
                                                            <asp:HiddenField runat="server" ID="txtTotal" Value="0" />
                                                        </div>

                                                        <div class="input-field2 col s3">
                                                            <div class="row">
                                                                <asp:TextBox ID="txtRT" TextMode="Number" runat="server" MaxLength="28"
                                                                    step="any" TabIndex="15">0.00</asp:TextBox>
                                                                <asp:FilteredTextBoxExtender ID="txtRT_FilteredTextBoxExtender" runat="server" Enabled="True"
                                                                    TargetControlID="txtRT" ValidChars="1234567890.-">
                                                                </asp:FilteredTextBoxExtender>
                                                                <asp:MaskedEditExtender ID="txtRT_MaskedEditExtender" runat="server" AutoComplete="False"
                                                                    CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat=""
                                                                    CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                                                    CultureTimePlaceholder="" Enabled="False" Mask="99.99" MaskType="Number" TargetControlID="txtRT">
                                                                </asp:MaskedEditExtender>
                                                                <label for="txtRT">RT</label>

                                                            </div>
                                                        </div>
                                                        <div class="input-field col s1">
                                                            <div class="row">
                                                                &nbsp;
                                                            </div>
                                                        </div>
                                                        <div class="input-field col s3">
                                                            <div class="row">
                                                                <asp:TextBox ID="txtOT" TextMode="Number" runat="server" MaxLength="28"
                                                                    step="any" TabIndex="16">0.00</asp:TextBox>
                                                                <asp:FilteredTextBoxExtender ID="txtOT_FilteredTextBoxExtender" runat="server" Enabled="True"
                                                                    TargetControlID="txtOT" ValidChars="1234567890.-">
                                                                </asp:FilteredTextBoxExtender>
                                                                <asp:MaskedEditExtender ID="txtOT_MaskedEditExtender" runat="server" CultureAMPMPlaceholder=""
                                                                    CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder=""
                                                                    CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder=""
                                                                    Enabled="False" Mask="99.99" MaskType="Number" TargetControlID="txtOT">
                                                                </asp:MaskedEditExtender>
                                                                <label for="txtOT">OT</label>
                                                            </div>
                                                        </div>
                                                        <div class="input-field col s1">
                                                            <div class="row">
                                                                &nbsp;
                                                            </div>
                                                        </div>
                                                        <div class="input-field col s4">
                                                            <div class="row">

                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator26" runat="server" ControlToValidate="txtEnrTime" ValidationGroup="addticketVG"
                                                                    Display="None" Enabled="False" ErrorMessage="Enroute time Required" SetFocusOnError="True"></asp:RequiredFieldValidator>


                                                                <asp:ValidatorCalloutExtender ID="RequiredFieldValidator26_ValidatorCalloutExtender"
                                                                    runat="server" Enabled="True" PopupPosition="BottomLeft" TargetControlID="RequiredFieldValidator26">
                                                                </asp:ValidatorCalloutExtender>

                                                                <asp:TextBox ID="txtEnrTime" runat="server" MaxLength="28"></asp:TextBox>
                                                                <asp:MaskedEditExtender ID="MaskedEditExtender1" runat="server" AcceptAMPM="True"
                                                                    CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat=""
                                                                    CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                                                    CultureTimePlaceholder="" Enabled="True" Mask="99:99" MaskType="Time" TargetControlID="txtEnrTime">
                                                                </asp:MaskedEditExtender>
                                                                <asp:MaskedEditValidator ID="MaskedEditValidator1" runat="server" ControlExtender="MaskedEditExtender1"
                                                                    ControlToValidate="txtEnrTime" Display="None" ErrorMessage="MaskedEditValidator1"
                                                                    InvalidValueMessage="Time is invalid" SetFocusOnError="True"></asp:MaskedEditValidator>
                                                                <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender4" runat="server" Enabled="True"
                                                                    PopupPosition="Left" TargetControlID="MaskedEditValidator1">
                                                                </asp:ValidatorCalloutExtender>
                                                                <asp:Label ID="lblenroute" runat="server" AssociatedControlID="txtEnrTime">
                                                                    <asp:LinkButton ID="btnEnroute" Visible="true" runat="server" CausesValidation="False" OnClick="btnEnroute_Click" Style="color: #1565C0; margin-left: -15px;"
                                                                        Text="Enroute"></asp:LinkButton></asp:Label>
                                                            </div>
                                                        </div>

                                                        <div class="input-field2 col s3">
                                                            <div class="row">
                                                                <asp:TextBox ID="txtNT" TextMode="Number" runat="server" MaxLength="28"
                                                                    step="any" TabIndex="17">0.00</asp:TextBox>
                                                                <asp:FilteredTextBoxExtender ID="txtNT_FilteredTextBoxExtender" runat="server" Enabled="True"
                                                                    TargetControlID="txtNT" ValidChars="1234567890.-">
                                                                </asp:FilteredTextBoxExtender>
                                                                <asp:MaskedEditExtender ID="txtNT_MaskedEditExtender" runat="server" CultureAMPMPlaceholder=""
                                                                    CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder=""
                                                                    CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder=""
                                                                    Enabled="False" Mask="99.99" MaskType="Number" TargetControlID="txtNT">
                                                                </asp:MaskedEditExtender>
                                                                <label for="txtNT">1.7</label>

                                                            </div>
                                                        </div>
                                                        <div class="input-field col s1">
                                                            <div class="row">
                                                                &nbsp;
                                                            </div>
                                                        </div>
                                                        <div class="input-field col s3">
                                                            <div class="row">
                                                                <asp:TextBox ID="txtDT" TextMode="Number" runat="server" MaxLength="28"
                                                                    step="any" TabIndex="18">0.00</asp:TextBox>
                                                                <asp:FilteredTextBoxExtender ID="txtDT_FilteredTextBoxExtender" runat="server" Enabled="True"
                                                                    TargetControlID="txtDT" ValidChars="1234567890.-">
                                                                </asp:FilteredTextBoxExtender>
                                                                <asp:MaskedEditExtender ID="txtDT_MaskedEditExtender" runat="server" CultureAMPMPlaceholder=""
                                                                    CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder=""
                                                                    CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder=""
                                                                    Enabled="False" Mask="99.99" MaskType="Number" TargetControlID="txtDT">
                                                                </asp:MaskedEditExtender>
                                                                <label for="txtDT">DT</label>
                                                            </div>
                                                        </div>
                                                        <div class="input-field col s1">
                                                            <div class="row">
                                                                &nbsp;
                                                            </div>
                                                        </div>
                                                        <div class="input-field col s4">
                                                            <div class="row">
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator29" runat="server" ControlToValidate="txtOnsitetime" ValidationGroup="addticketVG"
                                                                    Display="None" Enabled="False" ErrorMessage="Onsite time Required" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                <asp:ValidatorCalloutExtender ID="RequiredFieldValidator29_ValidatorCalloutExtender"
                                                                    runat="server" Enabled="True" PopupPosition="BottomLeft" TargetControlID="RequiredFieldValidator29">
                                                                </asp:ValidatorCalloutExtender>

                                                                <asp:TextBox ID="txtOnsitetime" runat="server" MaxLength="28"></asp:TextBox>
                                                                <asp:MaskedEditExtender ID="MaskedEditExtender2" runat="server" Mask="99:99" MaskType="Time"
                                                                    AcceptAMPM="True" TargetControlID="txtOnsitetime" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                                                    CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                                                    CultureTimePlaceholder="" Enabled="True">
                                                                </asp:MaskedEditExtender>
                                                                <asp:MaskedEditValidator ID="MaskedEditValidator2" runat="server" ControlExtender="MaskedEditExtender2"
                                                                    ControlToValidate="txtOnsitetime" InvalidValueMessage="Time is invalid" Display="None"
                                                                    SetFocusOnError="True" ErrorMessage="MaskedEditValidator2" />
                                                                <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender5" runat="server" Enabled="True"
                                                                    TargetControlID="MaskedEditValidator2" PopupPosition="Left">
                                                                </asp:ValidatorCalloutExtender>
                                                                <asp:CustomValidator ID="CustomValidator3" Visible="false" runat="server" Display="None" ErrorMessage="Time can't be less than enroute time."
                                                                    ControlToValidate="txtOnsitetime" ClientValidationFunction="ValidateTime" SetFocusOnError="True"></asp:CustomValidator>
                                                                <asp:ValidatorCalloutExtender ID="CustomValidator3_ValidatorCalloutExtender" runat="server"
                                                                    Enabled="True" TargetControlID="CustomValidator3" PopupPosition="Left">
                                                                </asp:ValidatorCalloutExtender>

                                                                <label>
                                                                    <asp:LinkButton ID="btnOnsite" Visible="true" runat="server" CausesValidation="False" OnClick="btnOnsite_Click" Style="color: #1565C0; margin-left: -15px;"
                                                                        Text="Onsite"></asp:LinkButton></label>
                                                            </div>
                                                        </div>

                                                        <div class="input-field2 col s3">
                                                            <div class="row">
                                                                <asp:TextBox ID="txtTT" TextMode="Number" runat="server" MaxLength="28"
                                                                    step="any" TabIndex="19">0.00</asp:TextBox>
                                                                <asp:FilteredTextBoxExtender ID="txtTT_FilteredTextBoxExtender" runat="server" Enabled="True"
                                                                    TargetControlID="txtTT" ValidChars="1234567890.-">
                                                                </asp:FilteredTextBoxExtender>
                                                                <asp:MaskedEditExtender ID="txtTT_MaskedEditExtender" runat="server" CultureAMPMPlaceholder=""
                                                                    CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder=""
                                                                    CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder=""
                                                                    Enabled="False" Mask="99.99" MaskType="Number" TargetControlID="txtTT">
                                                                </asp:MaskedEditExtender>
                                                                <label for="txtTT">TT</label>

                                                            </div>
                                                        </div>
                                                        <div class="input-field col s1">
                                                            <div class="row">
                                                                &nbsp;
                                                            </div>
                                                        </div>
                                                        <div class="input-field col s3">
                                                            <div class="row">
                                                                <asp:TextBox ID="txtBT" TextMode="Number" runat="server" MaxLength="28"
                                                                    step="any" TabIndex="19">0.00</asp:TextBox>
                                                                <asp:FilteredTextBoxExtender ID="txtBT_FilteredTextBoxExtender" runat="server" Enabled="True"
                                                                    TargetControlID="txtBT" ValidChars="1234567890.-">
                                                                </asp:FilteredTextBoxExtender>
                                                                <asp:MaskedEditExtender ID="txtBT_MaskedEditExtender" runat="server" CultureAMPMPlaceholder=""
                                                                    CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder=""
                                                                    CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder=""
                                                                    Enabled="False" Mask="99.99" MaskType="Number" TargetControlID="txtBT">
                                                                </asp:MaskedEditExtender>
                                                                <label for="txtBT">BT</label>
                                                            </div>
                                                        </div>
                                                        <div class="input-field col s1">
                                                            <div class="row">
                                                                &nbsp;
                                                            </div>
                                                        </div>
                                                        <div class="input-field col s4">
                                                            <div class="row">
                                                                <asp:TextBox ID="txtComplTime" runat="server" MaxLength="28"
                                                                    TabIndex="30"></asp:TextBox>
                                                                <asp:MaskedEditExtender ID="MaskedEditExtender3" runat="server" AcceptAMPM="True"
                                                                    CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat=""
                                                                    CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                                                    CultureTimePlaceholder="" Enabled="True" Mask="99:99" MaskType="Time" TargetControlID="txtComplTime">
                                                                </asp:MaskedEditExtender>
                                                                <asp:MaskedEditValidator ID="MaskedEditValidator4" runat="server" ControlExtender="MaskedEditExtender3"
                                                                    ControlToValidate="txtComplTime" Display="None" ErrorMessage="MaskedEditValidator4"
                                                                    InvalidValueMessage="Time is invalid" SetFocusOnError="True"></asp:MaskedEditValidator>
                                                                <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender6" runat="server" Enabled="True"
                                                                    PopupPosition="Left" TargetControlID="MaskedEditValidator4">
                                                                </asp:ValidatorCalloutExtender>
                                                                <asp:CustomValidator ID="CustomValidator4" Visible="false" runat="server" Display="None" ErrorMessage="Time can't be less than onsite time."
                                                                    ControlToValidate="txtComplTime" ClientValidationFunction="ValidateTimeComplete"
                                                                    SetFocusOnError="True"></asp:CustomValidator>
                                                                <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender7" runat="server" Enabled="True"
                                                                    TargetControlID="CustomValidator4" PopupPosition="Left">
                                                                </asp:ValidatorCalloutExtender>
                                                                <label>
                                                                    <asp:LinkButton ID="btnComplete" Visible="true" runat="server" CausesValidation="False" OnClick="btnComplete_Click" Style="color: #1565C0; margin-left: -15px;"
                                                                        Text="Complete"></asp:LinkButton></label>
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator27" runat="server" ControlToValidate="txtComplTime" ValidationGroup="addticketVG"
                                                                    Display="None" Enabled="False" ErrorMessage="Completed time Required" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                <asp:ValidatorCalloutExtender ID="RequiredFieldValidator27_ValidatorCalloutExtender"
                                                                    runat="server" Enabled="True" PopupPosition="BottomLeft" TargetControlID="RequiredFieldValidator27">
                                                                </asp:ValidatorCalloutExtender>
                                                            </div>
                                                        </div>
                                                    </telerik:RadAjaxPanel>
                                                </div>
                                                <div class="form-section3-blank">
                                                    &nbsp;
                                                </div>
                                                <div class="form-section3">
                                                    <div class="section-ttle">Project Info</div>


                                                    <div class="input-field col s5">
                                                        <div class="row">
                                                            <label for="txtProject">
                                                                Project#</label>
                                                            <asp:TextBox ID="projectNo" Style="pointer-events: none; color: darkgrey" class="validate" runat="server"></asp:TextBox>

                                                            <asp:Button ID="btnGetCode" runat="server" CausesValidation="False" Text="Button" Style="display: none;" OnClick="btnGetCode_Click" />
                                                        </div>
                                                    </div>
                                                    <div class="input-field col s2">
                                                        <div class="row">
                                                            &nbsp; 
                                                        </div>
                                                    </div>
                                                    <div class="input-field col s5">
                                                        <div class="row">

                                                            <label class="drpdwn-label">New Project Template<span class="reqd">*</span></label>

                                                            <asp:DropDownList ID="ddlTemplate" runat="server" onchange="ToggleValidator();"
                                                                CssClass="browser-default" />
                                                            <asp:RequiredFieldValidator ID="rfvjtempl" Visible="false" runat="server" ControlToValidate="ddlTemplate" ValidationGroup="addticketVG"
                                                                Display="None" ErrorMessage="Please select the Project Template" InitialValue="0" SetFocusOnError="True"></asp:RequiredFieldValidator>

                                                            <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender8"
                                                                runat="server" PopupPosition="BottomLeft" Enabled="True" TargetControlID="rfvjtempl">
                                                            </asp:ValidatorCalloutExtender>



                                                        </div>
                                                    </div>
                                                    <div class="input-field col s12">
                                                        <div class="row">


                                                            <label for="txtJobCode">
                                                                <%--Issue...  document.getElementById('<%=btnGetCode.ClientID%>').click()--%>
                                                                    Project Type         <a id="aref" style="color: #5b9bd1; display: none;" onclick="" href="#">
                                                                        <i class="mdi-navigation-refresh" style="font-size: 1.2em; color: #1565c0; line-height: 5px;"></i>

                                                                    </a>
                                                            </label>

                                                            <asp:TextBox runat="server" TabIndex="3" ID="txtJobCode" autocomplete="off"> 
                                                            </asp:TextBox>
                                                            <div id="DivprojectType" class="popup_div " style="width: 450px; z-index: 2">
                                                                <div class="btnlinks" style="margin-bottom: 5px; float: right;">
                                                                    <asp:LinkButton CausesValidation="false" ID="HideMELinkButton2" runat="server" OnClientClick="HideME();">Close</asp:LinkButton>
                                                                </div>
                                                                <div class="grid_container">
                                                                    <div class="RadGrid RadGrid_Material FormGrid">
                                                                        <telerik:RadAjaxPanel ID="RadAjaxPanelRadgvProjectCode" runat="server">

                                                                            <telerik:RadGrid ID="RadgvProjectCode"
                                                                                RenderMode="Auto" AllowFilteringByColumn="false" ShowFooter="True" PageSize="10"
                                                                                ShowStatusBar="true" runat="server" AllowPaging="true" AllowSorting="false" Width="100%"
                                                                                OnNeedDataSource="RadgvProjectCode_NeedDataSource"
                                                                                PagerStyle-AlwaysVisible="true"
                                                                                AllowCustomPaging="false">
                                                                                <CommandItemStyle />
                                                                                <GroupingSettings CaseSensitive="false" />
                                                                                <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                                                                                    <Selecting AllowRowSelect="True"></Selecting>
                                                                                    <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                                                                    <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                                                                </ClientSettings>
                                                                                <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="True" ShowFooter="True" AllowPaging="true">
                                                                                    <Columns>

                                                                                        <telerik:GridTemplateColumn DataField="GroupName" AutoPostBackOnFilter="false" AllowFiltering="false" SortExpression="GroupName"
                                                                                            CurrentFilterFunction="Contains" UniqueName="GroupName" HeaderText="Group Name" ShowFilterIcon="false" HeaderStyle-Width="50">
                                                                                            <ItemTemplate>
                                                                                                <%# Eval("GroupName") %>
                                                                                            </ItemTemplate>
                                                                                        </telerik:GridTemplateColumn>

                                                                                        <telerik:GridTemplateColumn DataField="code" AutoPostBackOnFilter="false" AllowFiltering="false" SortExpression="code"
                                                                                            CurrentFilterFunction="Contains" UniqueName="code" HeaderText="Code" ShowFilterIcon="false" HeaderStyle-Width="50">
                                                                                            <ItemTemplate>
                                                                                                <%# Eval("code") %>
                                                                                            </ItemTemplate>
                                                                                        </telerik:GridTemplateColumn>

                                                                                        <telerik:GridTemplateColumn DataField="CodeDesc" AutoPostBackOnFilter="false" AllowFiltering="false" SortExpression="CodeDesc"
                                                                                            CurrentFilterFunction="Contains" UniqueName="CodeDesc" HeaderText="Code Desc" ShowFilterIcon="false" HeaderStyle-Width="50">
                                                                                            <ItemTemplate>
                                                                                                <%# Eval("CodeDesc") %>
                                                                                            </ItemTemplate>
                                                                                        </telerik:GridTemplateColumn>

                                                                                        <telerik:GridTemplateColumn DataField="line" AutoPostBackOnFilter="false" AllowFiltering="false" SortExpression="line"
                                                                                            CurrentFilterFunction="Contains" UniqueName="line" HeaderText="Phase" ShowFilterIcon="false" HeaderStyle-Width="50">
                                                                                            <ItemTemplate>
                                                                                                <%# Eval("line") %>
                                                                                            </ItemTemplate>
                                                                                        </telerik:GridTemplateColumn>

                                                                                        <telerik:GridTemplateColumn DataField="bomtype" AutoPostBackOnFilter="false" AllowFiltering="false" SortExpression="bomtype"
                                                                                            CurrentFilterFunction="Contains" UniqueName="bomtype" HeaderText="Type" ShowFilterIcon="false" HeaderStyle-Width="50">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblID" Style="display: none" runat="server" Text='<%# Eval("code") %>'></asp:Label>
                                                                                                <asp:Label ID="lblIDname" Style="display: none" runat="server" Text='<%# Eval("line") +":"+ Eval("code") +":"+ Eval("fdesc")  %>'></asp:Label>
                                                                                                <asp:Label ID="lblIDname1" Style="display: none" runat="server" Text='<%# Eval("Code") +"/"+ Eval("line") +"/"+ Eval("fdesc")  %>'></asp:Label>
                                                                                                <asp:Label ID="lblType" runat="server" Text='<%# Eval("bomtype") %>'></asp:Label>
                                                                                                <asp:Label ID="lblphase" Style="display: none" runat="server" Text='<%# Eval("line") %>'></asp:Label>
                                                                                            </ItemTemplate>
                                                                                        </telerik:GridTemplateColumn>
                                                                                        <telerik:GridTemplateColumn DataField="fdesc" AutoPostBackOnFilter="false" AllowFiltering="false" SortExpression="fdesc"
                                                                                            CurrentFilterFunction="Contains" UniqueName="fdesc" HeaderText="Description" ShowFilterIcon="false" HeaderStyle-Width="100">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblDesc" runat="server"><%#Eval("fdesc")%></asp:Label>
                                                                                            </ItemTemplate>
                                                                                            <FooterTemplate>
                                                                                                <footertemplate><%= RadgvProjectCode.VirtualItemCount  %>  Record(s) Found.   </footertemplate>
                                                                                            </FooterTemplate>
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




                                                    <div class="input-field col s12">
                                                        <div class="row">
                                                            <label class="drpdwn-label">Department</label>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator32" runat="server" Visible="false" ValidationGroup="addticketVG"
                                                                ControlToValidate="ddlDepartment" Display="None" ErrorMessage="Department Required"
                                                                SetFocusOnError="True"></asp:RequiredFieldValidator>

                                                            <asp:ValidatorCalloutExtender
                                                                ID="RequiredFieldValidator32_ValidatorCalloutExtender" runat="server"
                                                                TargetControlID="RequiredFieldValidator32">
                                                            </asp:ValidatorCalloutExtender>

                                                            <asp:DropDownList ID="ddlDepartment" runat="server" CssClass="browser-default"
                                                                TabIndex="22">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                    <div class="input-field col s12">
                                                        <div class="row">
                                                            <label class="drpdwn-label">Wage<span class="reqd">*</span></label>

                                                            <asp:RequiredFieldValidator ID="RequiredFieldVWage" runat="server" ValidationGroup="addticketVG"
                                                                InitialValue="0" ControlToValidate="ddlWage" Enabled="false" Display="None" ErrorMessage="Wage Required"
                                                                SetFocusOnError="True"></asp:RequiredFieldValidator>

                                                            <asp:ValidatorCalloutExtender PopupPosition="BottomLeft"
                                                                ID="ValidatorCVWage" runat="server" Enabled="false"
                                                                TargetControlID="RequiredFieldVWage">
                                                            </asp:ValidatorCalloutExtender>

                                                            <asp:DropDownList ID="ddlWage" runat="server" CssClass="browser-default"
                                                                TabIndex="22">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                    <div id="QbpayrollDiv" runat="server">

                                                        <div class="input-field col s5">
                                                            <div class="row">
                                                                <label class="drpdwn-label">Service Item</label>
                                                                <asp:DropDownList ID="ddlService" runat="server" CssClass="browser-default"
                                                                    TabIndex="26" OnSelectedIndexChanged="ddlService_SelectedIndexChanged"
                                                                    AutoPostBack="True">
                                                                </asp:DropDownList>
                                                            </div>
                                                        </div>
                                                        <div class="input-field col s2">
                                                            <div class="row">
                                                                &nbsp;
                                                            </div>
                                                        </div>
                                                        <div class="input-field col s5">
                                                            <div class="row">
                                                                <label class="drpdwn-label">Payroll Item</label>
                                                                <asp:DropDownList ID="ddlPayroll" runat="server" CssClass="browser-default"
                                                                    TabIndex="31">
                                                                </asp:DropDownList>
                                                            </div>
                                                        </div>
                                                    </div>

                                                </div>


                                            </div>

                                            <div class="form-section-row">
                                                <div class="form-section3">
                                                    <div class="section-ttle">
                                                        More Details
                                                    </div>
                                                    <div class="input-field col s12">
                                                        <div class="row">
                                                            <asp:TextBox ID="txtRecommendation" runat="server" class="materialize-textarea" Height="50px" Style="min-height: 2.1em; max-height: 7.1em; overflow-y: auto;"
                                                                Enabled="False" TextMode="MultiLine"></asp:TextBox>
                                                            <label for="txtRecommendation">Recommendation</label>
                                                        </div>
                                                    </div>
                                                    <div class="input-field col s12">
                                                        <div class="row">
                                                            <asp:TextBox ID="txtPartsUsed" runat="server"
                                                                Style="position: relative; z-index: 1"
                                                                TextMode="MultiLine"
                                                                CssClass="materialize-textarea" TabIndex="40"></asp:TextBox>
                                                            <label for="txtPartsUsed">Parts Used</label>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="form-section3-blank">
                                                    &nbsp;
                                                </div>
                                                <div class="form-section3">
                                                    <div class="section-ttle">Comments</div>

                                                    <div class="input-field col s12">
                                                        <div class="row">
                                                            <asp:TextBox ID="txtComments" runat="server"
                                                                TextMode="MultiLine" Style="padding: 0.4rem 0 !important"
                                                                CssClass="materialize-textarea textarea-border" TabIndex="39"></asp:TextBox>
                                                            <label for="txtComments" class="txtbrdlbl">
                                                                Comments
                                                            </label>

                                                        </div>
                                                    </div>


                                                </div>
                                                <div class="form-section3-blank">
                                                    &nbsp;
                                                </div>
                                                <div class="form-section3">
                                                    <div class="section-ttle">
                                                        Expenses
                                                                <span style="float: right;">


                                                                    <label style="height: 20px !important; font-size: 1em !important;">
                                                                        Total Exp: $
                                                                        <asp:Label ID="lblTotalExp" runat="server" Style="font-weight: bold;">0.00</asp:Label></label>
                                                                    <label style="height: 20px !important; font-size: 1em !important;">
                                                                        || Traveled Miles:                       
                                                                        <asp:Label ID="txtMileTraveled" runat="server" Style="font-weight: bold;">0.00</asp:Label>
                                                                    </label>
                                                                </span>
                                                    </div>

                                                    <div class="input-field col s5">
                                                        <div class="row">
                                                            <asp:TextBox ID="txtExpMisc" TextMode="Number" runat="server" MaxLength="28"
                                                                step="any" TabIndex="19">0.00</asp:TextBox>
                                                            <asp:FilteredTextBoxExtender ID="txtExpMisc_FilteredTextBoxExtender" runat="server"
                                                                Enabled="True" TargetControlID="txtExpMisc" ValidChars="1234567890.-">
                                                            </asp:FilteredTextBoxExtender>
                                                            <asp:MaskedEditExtender ID="txtExpMisc_MaskedEditExtender" runat="server" AutoComplete="False"
                                                                CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat=""
                                                                CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                                                CultureTimePlaceholder="" Enabled="False" Mask="99.99" MaskType="Number" TargetControlID="txtExpMisc">
                                                            </asp:MaskedEditExtender>
                                                            <label for="txtExpMisc">Miscellaneous</label>
                                                        </div>
                                                    </div>
                                                    <div class="input-field col s2">
                                                        <div class="row">
                                                            &nbsp;
                                                        </div>
                                                    </div>
                                                    <div class="input-field col s5">
                                                        <div class="row">
                                                            <asp:TextBox ID="txtMileStart" TextMode="Number" runat="server"
                                                                MaxLength="28" step="any" TabIndex="19">0.00</asp:TextBox>
                                                            <asp:FilteredTextBoxExtender ID="txtMileStart_FilteredTextBoxExtender" runat="server"
                                                                Enabled="True" TargetControlID="txtMileStart" ValidChars="1234567890.-">
                                                            </asp:FilteredTextBoxExtender>
                                                            <asp:MaskedEditExtender ID="txtMileStart_MaskedEditExtender" runat="server" AutoComplete="False"
                                                                CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat=""
                                                                CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                                                CultureTimePlaceholder="" Enabled="False" Mask="99.99" MaskType="Number" TargetControlID="txtMileStart">
                                                            </asp:MaskedEditExtender>
                                                            <label for="txtMileStart">Starting Mileage</label>
                                                        </div>
                                                    </div>

                                                    <div class="input-field col s5">
                                                        <div class="row">
                                                            <asp:TextBox ID="txtExpZone" TextMode="Number" runat="server" MaxLength="28"
                                                                step="any" TabIndex="19">0.00</asp:TextBox>
                                                            <asp:FilteredTextBoxExtender ID="txtExpZone_FilteredTextBoxExtender" runat="server"
                                                                Enabled="True" TargetControlID="txtExpZone" ValidChars="1234567890.-">
                                                            </asp:FilteredTextBoxExtender>
                                                            <asp:MaskedEditExtender ID="txtExpZone_MaskedEditExtender" runat="server" CultureAMPMPlaceholder=""
                                                                CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder=""
                                                                CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder=""
                                                                Enabled="False" Mask="99.99" MaskType="Number" TargetControlID="txtExpZone">
                                                            </asp:MaskedEditExtender>
                                                            <label for="txtExpZone">Zone</label>
                                                        </div>
                                                    </div>
                                                    <div class="input-field col s2">
                                                        <div class="row">
                                                            &nbsp;
                                                        </div>
                                                    </div>
                                                    <div class="input-field col s5">

                                                        <div class="row">
                                                            <asp:TextBox ID="txtMileEnd" TextMode="Number" runat="server"
                                                                MaxLength="28" step="any" TabIndex="19">0.00</asp:TextBox><asp:FilteredTextBoxExtender
                                                                    ID="txtMileEnd_FilteredTextBoxExtender" runat="server" Enabled="True" TargetControlID="txtMileEnd"
                                                                    ValidChars="1234567890.-">
                                                                </asp:FilteredTextBoxExtender>
                                                            <asp:MaskedEditExtender ID="txtMileEnd_MaskedEditExtender" runat="server" AutoComplete="False"
                                                                CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat=""
                                                                CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                                                CultureTimePlaceholder="" Enabled="False" Mask="99.99" MaskType="Number" TargetControlID="txtMileEnd">
                                                            </asp:MaskedEditExtender>
                                                            <label for="txtMileEnd">Ending Mileage</label>
                                                        </div>
                                                    </div>
                                                    <div class="input-field col s5">
                                                        <div class="row">

                                                            <asp:TextBox ID="txtExpToll" TextMode="Number" runat="server" MaxLength="28"
                                                                step="any" TabIndex="19">0.00</asp:TextBox>
                                                            <asp:FilteredTextBoxExtender ID="txtExpToll_FilteredTextBoxExtender" runat="server"
                                                                Enabled="True" TargetControlID="txtExpToll" ValidChars="1234567890.-">
                                                            </asp:FilteredTextBoxExtender>
                                                            <asp:MaskedEditExtender ID="txtExpToll_MaskedEditExtender" runat="server" CultureAMPMPlaceholder=""
                                                                CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder=""
                                                                CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder=""
                                                                Enabled="False" Mask="99.99" MaskType="Number" TargetControlID="txtExpToll">
                                                            </asp:MaskedEditExtender>
                                                            <label for="txtExpToll">Toll</label>
                                                        </div>
                                                    </div>
                                                    <div class="input-field col s2">
                                                        <div class="row">
                                                            &nbsp;
                                                        </div>
                                                    </div>

                                                </div>
                                            </div>
                                        </div>
                                    </telerik:RadAjaxPanel>
                                </div>
                                <div style="clear: both;"></div>
                            </div>


                        </li>
                        <li>
                            <div id="accrdcustom" class="collapsible-header accrd accordian-text-custom"><i class="mdi-action-perm-data-setting"></i>Custom</div>
                            <div class="collapsible-body">
                                <div class="form-content-wrap">
                                    <div class="form-content-pd">
                                        <div class="form-section2">
                                            <div class="section-ttle">Custom Fields</div>
                                            <div class="input-field col s12">
                                                <div class="row">

                                                    <label for="txtTickCustom1">
                                                        <asp:Label for="txtCst1" ID="lblCustom1" runat="server"></asp:Label></label>
                                                    <asp:TextBox ID="txtCst1" runat="server"> </asp:TextBox>

                                                </div>
                                            </div>
                                            <div class="input-field col s12">
                                                <div class="row">

                                                    <label for="txtCst2">
                                                        <asp:Label ID="lblCustom2" runat="server"></asp:Label></label>
                                                    <asp:TextBox ID="txtCst2" runat="server"></asp:TextBox>
                                                </div>
                                            </div>

                                            <div class="input-field col s12">
                                                <div class="row">

                                                    <label for="txtCst3">
                                                        <asp:Label ID="lblCustom3" runat="server"></asp:Label></label>
                                                    <asp:TextBox ID="txtCst3" runat="server"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="input-field col s12">
                                                <div class="row">
                                                    <label for="txtCst4">
                                                        <asp:Label ID="lblCustom4" runat="server"></asp:Label></label>
                                                    <asp:TextBox ID="txtCst4" runat="server"></asp:TextBox>

                                                </div>
                                            </div>
                                            <div class="input-field col s12">
                                                <div class="row">

                                                    <label for="txtCst5">
                                                        <asp:Label ID="lblCustom5" runat="server"></asp:Label></label>
                                                    <asp:TextBox ID="txtCst5" runat="server"></asp:TextBox>
                                                </div>
                                            </div>

                                            <div class="input-field col s6">
                                                <div class="checkrow">
                                                    <asp:CheckBox ID="chkCst1" CssClass="filled-in" runat="server" />
                                                    <label for="chkCst1">
                                                        <asp:Label ID="lblCustom6" runat="server"></asp:Label></label>

                                                </div>
                                            </div>
                                            <div class="input-field col s6">
                                                <div class="checkrow">
                                                    <asp:CheckBox ID="chkCst2" runat="server" />
                                                    <label for="chkCst2">
                                                        <asp:Label ID="lblCustom7" runat="server"></asp:Label></label>

                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-section3-blank">
                                            &nbsp;
                                        </div>
                                        <div class="form-section2">
                                            <div class="section-ttle">Ticket Custom Fields</div>

                                            <div class="input-field col s12">
                                                <div class="row">
                                                    <asp:TextBox ID="txtTickCustom1" TabIndex="39" runat="server"></asp:TextBox>
                                                    <label for="txtTickCustom1">
                                                        <asp:Label ID="lblCustomTick1" runat="server">Ticket Custom 1</asp:Label></label>


                                                </div>
                                            </div>

                                            <div class="input-field col s12">
                                                <div class="row">

                                                    <asp:TextBox ID="txtTickCustom2" TabIndex="40" runat="server"></asp:TextBox>
                                                    <label for="txtTickCustom2">
                                                        <asp:Label ID="lblCustomTick2" runat="server">Ticket Custom 2</asp:Label></label>

                                                </div>
                                            </div>
                                            <div class="input-field col s12">
                                                <div class="row">

                                                    <asp:TextBox ID="txtTickCustom3" TabIndex="41" runat="server" MaxLength="28"></asp:TextBox>
                                                    <label for="txtTickCustom3">

                                                        <asp:Label ID="lblCustomTick5" runat="server">Ticket Custom 3</asp:Label></label>

                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True"
                                                        TargetControlID="txtTickCustom3" ValidChars="0123456789">
                                                    </asp:FilteredTextBoxExtender>
                                                </div>
                                            </div>

                                            <div class="input-field col s6">
                                                <div class="checkrow">

                                                    <asp:CheckBox ID="chkTickCustom1" CssClass="css-checkbox" TabIndex="42" runat="server" Text="Ticket Checkbox 1" />



                                                </div>
                                            </div>
                                            <div class="input-field col s6">
                                                <div class="checkrow">

                                                    <asp:CheckBox ID="chkTickCustom2" CssClass="css-checkbox" runat="server" Text="Ticket Checkbox 2" />


                                                </div>
                                            </div>
                                        </div>

                                        <div class="cf"></div>
                                    </div>
                                </div>
                                <div style="clear: both;"></div>
                            </div>
                        </li>

                        <li>
                            <div id="accrdResolution" style="display: block;" class="collapsible-header accrd accordian-text-custom"><i class="mdi-action-perm-data-setting"></i>Resolution Tasks</div>
                            <div class="collapsible-body">
                                <div class="form-content-wrap">
                                    <div class="form-content-pd">
                                        <div class="form-section-row">
                                            <div class="section-ttle">Resolution Tasks</div>
                                            <div class="grid_container">
                                                <div class="form-section-row">
                                                    <div class="RadGrid RadGrid_Material FormGrid">
                                                        <asp:Repeater ID="rptCodesList" runat="server">

                                                            <HeaderTemplate>
                                                                <table id="tblCodesList">
                                                                    <th>Code</th>
                                                                    <th>Updated</th>
                                                                    <th>Ticket#</th>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <tr>
                                                                    <td>
                                                                        <asp:CheckBox ID="chkCode" onchange="checkedHidden(this);" runat="server" Text='<%# Eval("fdesc") %>' />
                                                                        <asp:HiddenField ID="hdnCodeCat" runat="server" Value='<%# Eval("category") %>' />
                                                                        <asp:HiddenField ID="hdnTicket" runat="server" />
                                                                        <asp:HiddenField ID="hdnDate" runat="server" />
                                                                        <asp:HiddenField ID="hdnUsername" runat="server" />
                                                                        <asp:HiddenField ID="hdnChecked" runat="server" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="lblDescRP" runat="server"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:HyperLink ID="lnkTicket" Target="_blank" runat="server"></asp:HyperLink>
                                                                    </td>
                                                                </tr>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                </table>
                                                                               
                                                            </FooterTemplate>

                                                        </asp:Repeater>
                                                        <asp:Label ID="lblResolutionTasks" runat="server">No Record Found...</asp:Label>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div style="clear: both;"></div>
                            </div>
                        </li>
                        <li>
                            <div id="accrdMCP" style="display: block;" class="collapsible-header accrd accordian-text-custom"><i class="mdi-action-perm-data-setting"></i>MCP</div>
                            <div class="collapsible-body">
                                <div class="form-content-wrap">
                                    <div class="form-content-pd">
                                        <div class="form-section-row">
                                            <div class="section-ttle">MCP</div>
                                            <div class="grid_container">
                                                <div class="form-section-row">
                                                    <div class="RadGrid RadGrid_Material FormGrid">



                                                        <telerik:RadGrid ID="RadgvMCPDetails"
                                                            RenderMode="Auto" AllowFilteringByColumn="false" ShowFooter="True" PageSize="10"
                                                            ShowStatusBar="true" runat="server" AllowPaging="false" AllowSorting="false" Width="100%" PagerStyle-AlwaysVisible="true"
                                                            AllowCustomPaging="True">
                                                            <CommandItemStyle />
                                                            <GroupingSettings CaseSensitive="false" />
                                                            <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                                                                <Selecting AllowRowSelect="True"></Selecting>
                                                                <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                                                <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                                            </ClientSettings>
                                                            <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="True" ShowFooter="True">
                                                                <Columns>
                                                                    <telerik:GridTemplateColumn DataField="Equip" AutoPostBackOnFilter="false" AllowFiltering="false" SortExpression="Equip"
                                                                        CurrentFilterFunction="Contains" UniqueName="Equip" HeaderText="Equip" ShowFilterIcon="false" HeaderStyle-Width="100">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblEquip" runat="server" Text='<%# Bind("Equip") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>

                                                                    <telerik:GridTemplateColumn DataField="Code" AutoPostBackOnFilter="false" AllowFiltering="false" SortExpression="Code"
                                                                        CurrentFilterFunction="Contains" UniqueName="Code" HeaderText="Type" ShowFilterIcon="false" HeaderStyle-Width="100">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblCode" runat="server" Text='<%# Bind("Code") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>

                                                                    <telerik:GridTemplateColumn DataField="Section" AutoPostBackOnFilter="false" AllowFiltering="false" SortExpression="Section"
                                                                        CurrentFilterFunction="Contains" UniqueName="Section" HeaderText="Section" ShowFilterIcon="false" HeaderStyle-Width="100">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblSection" runat="server" Text='<%# Bind("Section") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>

                                                                    <telerik:GridTemplateColumn DataField="template" AutoPostBackOnFilter="false" AllowFiltering="false" SortExpression="template"
                                                                        CurrentFilterFunction="Contains" UniqueName="template" HeaderText="Template" ShowFilterIcon="false" HeaderStyle-Width="100">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblTemplate" runat="server" Text='<%# Bind("template") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>

                                                                    <telerik:GridTemplateColumn DataField="fDesc" AutoPostBackOnFilter="false" AllowFiltering="false" SortExpression="fDesc"
                                                                        CurrentFilterFunction="Contains" UniqueName="fDesc" HeaderText="Desc" ShowFilterIcon="false" HeaderStyle-Width="100">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblDesc" runat="server" Text='<%# Bind("fDesc") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>

                                                                    <telerik:GridTemplateColumn DataField="freq" AutoPostBackOnFilter="false" AllowFiltering="false" SortExpression="freq"
                                                                        CurrentFilterFunction="Contains" UniqueName="freq" HeaderText="freq" ShowFilterIcon="false" HeaderStyle-Width="100">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblFreq" runat="server" Text='<%# Bind("freq") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>

                                                                    <telerik:GridTemplateColumn DataField="comment" AutoPostBackOnFilter="false" AllowFiltering="false" SortExpression="comment"
                                                                        CurrentFilterFunction="Contains" UniqueName="comment" HeaderText="Comments" ShowFilterIcon="false" HeaderStyle-Width="100">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblComments" runat="server" Text='<%# Bind("comment") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>

                                                                    <telerik:GridTemplateColumn DataField="Status" AutoPostBackOnFilter="false" AllowFiltering="false" SortExpression="Status"
                                                                        CurrentFilterFunction="Contains" UniqueName="Status" HeaderText="Status" ShowFilterIcon="false" HeaderStyle-Width="100">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblStatus" runat="server" Text='<%# Bind("Status") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>

                                                                    <telerik:GridTemplateColumn DataField="LastDate" AutoPostBackOnFilter="false" AllowFiltering="false" SortExpression="LastDate"
                                                                        CurrentFilterFunction="Contains" UniqueName="LastDate" HeaderText="LastDate" ShowFilterIcon="false" HeaderStyle-Width="100">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblLastdate" runat="server" Text='<%# Eval("LastDate", "{0:MM/dd/yyyy}")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>

                                                                    <telerik:GridTemplateColumn DataField="NextDateDue" AutoPostBackOnFilter="false" AllowFiltering="false" SortExpression="NextDateDue"
                                                                        CurrentFilterFunction="Contains" UniqueName="NextDateDue" HeaderText="NextDateDue" ShowFilterIcon="false" HeaderStyle-Width="100">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblNextDateDue" runat="server" Text='<%# Eval("NextDateDue", "{0:MM/dd/yyyy}")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <FooterTemplate><%= RadgvMCPDetails.VirtualItemCount  %>  Record(s) Found.   </FooterTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                </Columns>
                                                            </MasterTableView>
                                                        </telerik:RadGrid>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div style="clear: both;"></div>
                            </div>
                        </li>
                        <li>
                            <div id="accrdPO" style="display: block;" class="collapsible-header accrd accordian-text-custom"><i class="mdi-action-perm-data-setting"></i>Purchase Orders</div>
                            <div class="collapsible-body">
                                <div class="form-content-wrap">
                                    <div class="form-content-pd">
                                        <div class="form-section-row">
                                            <div class="section-ttle">PO</div>

                                            <div class="form-section-row">
                                                <div class="btnlinks">


                                                    <asp:LinkButton CausesValidation="false" ID="btnaddpo" runat="server" OnClientClick="return btnaddpoCClick();" OnClick="btnaddpo_Click" Text="Add PO"></asp:LinkButton>
                                                </div>
                                            </div>
                                            <div class="grid_container">
                                                <div class="form-section-row">
                                                    <div class="RadGrid RadGrid_Material FormGrid">
                                                        <telerik:RadGrid ID="RadGVPO"
                                                            RenderMode="Auto" AllowFilteringByColumn="false" ShowFooter="True" PageSize="10"
                                                            ShowStatusBar="true" runat="server" AllowPaging="false" AllowSorting="false" Width="100%" PagerStyle-AlwaysVisible="true"
                                                            AllowCustomPaging="True">
                                                            <CommandItemStyle />
                                                            <GroupingSettings CaseSensitive="false" />
                                                            <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                                                                <Selecting AllowRowSelect="True"></Selecting>
                                                                <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                                                <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                                            </ClientSettings>
                                                            <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="True" ShowFooter="True">
                                                                <Columns>
                                                                    <telerik:GridTemplateColumn DataField="VendorName" AutoPostBackOnFilter="false" AllowFiltering="false" SortExpression="VendorName"
                                                                        CurrentFilterFunction="Contains" UniqueName="VendorName" HeaderText="VendorName" ShowFilterIcon="false" HeaderStyle-Width="100">
                                                                        <ItemTemplate>

                                                                            <asp:Label ID="lblVendorName" runat="server" Text='<%# Bind("VendorName") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn DataField="fDesc" AutoPostBackOnFilter="false" AllowFiltering="false" SortExpression="fDesc"
                                                                        CurrentFilterFunction="Contains" UniqueName="fDesc" HeaderText="Desc" ShowFilterIcon="false" HeaderStyle-Width="100">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblfDesc" runat="server" Text='<%# Bind("fDesc") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn DataField="Amount" AutoPostBackOnFilter="false" AllowFiltering="false" SortExpression="Amount"
                                                                        CurrentFilterFunction="Contains" UniqueName="Amount" HeaderText="Amount" ShowFilterIcon="false" HeaderStyle-Width="100">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblAmount" runat="server" Text='<%# Bind("Amount") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <FooterTemplate><%= RadGVPO.VirtualItemCount  %>  Record(s) Found.   </FooterTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                </Columns>
                                                            </MasterTableView>
                                                        </telerik:RadGrid>
                                                    </div>
                                                </div>

                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div style="clear: both;"></div>
                            </div>
                        </li>
                        <li>

                            <div id="accrddocuments" class="collapsible-header accrd accordian-text-custom"><i class="mdi-action-credit-card"></i>Documents</div>
                            <div class="collapsible-body">
                                <div class="form-content-wrap">
                                    <div class="form-content-pd">
                                        <asp:Panel ID="pnlDocPermission" runat="server">
                                            <asp:Panel ID="pnlDocumentButtons" runat="server">
                                                <div class="form-section-row">
                                                    <div class="col s12 m12 l12">
                                                        <!--<p>Maximum file upload size 2MB.</p>-->

                                                        <asp:FileUpload ID="FileUpload1" class="dropify" runat="server"
                                                            onchange="AddDocumentClick(this);" />
                                                        <!--data-max-file-size="2M"-->
                                                    </div>
                                                </div>
                                                <div class="form-section-row">
                                                    <div class="btnlinks">
                                                        <asp:LinkButton ID="lnkDeleteDoc" runat="server" CausesValidation="False" OnClick="lnkDeleteDoc_Click" ToolTip="Delete"
                                                            OnClientClick="return DeleteDocumentClick(this);">Delete</asp:LinkButton>
                                                        <asp:LinkButton ID="lnkUploadDoc" runat="server"
                                                            CausesValidation="False" OnClick="lnkUploadDoc_Click"
                                                            Style="display: none">Upload</asp:LinkButton>
                                                        <asp:LinkButton ID="lnkPostback" runat="server"
                                                            CausesValidation="False" Style="display: none">Postback</asp:LinkButton>
                                                    </div>
                                                </div>
                                            </asp:Panel>
                                            <div class="form-section-row">
                                                <div class="grid_container">
                                                    <div class="form-section-row">
                                                        <div class="RadGrid RadGrid_Material FormGrid">

                                                            <telerik:RadGrid ID="RadgvDocuments"
                                                                RenderMode="Auto" AllowFilteringByColumn="false" ShowFooter="True" PageSize="10"
                                                                ShowStatusBar="true" runat="server" AllowPaging="false" AllowSorting="false" Width="100%" PagerStyle-AlwaysVisible="true"
                                                                AllowCustomPaging="True">
                                                                <CommandItemStyle />
                                                                <GroupingSettings CaseSensitive="false" />
                                                                <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                                                                    <Selecting AllowRowSelect="True"></Selecting>
                                                                    <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                                                    <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                                                </ClientSettings>
                                                                <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="True" ShowFooter="True">
                                                                    <Columns>
                                                                        <telerik:GridTemplateColumn AutoPostBackOnFilter="false" AllowFiltering="false"
                                                                            CurrentFilterFunction="Contains" HeaderText="" ShowFilterIcon="false" HeaderStyle-Width="50">
                                                                            <ItemTemplate>
                                                                                <asp:CheckBox ID="chkSelect" runat="server" />
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>
                                                                        <telerik:GridTemplateColumn Visible="false">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblID" runat="server" Text='<%# Eval("id") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>
                                                                        <telerik:GridTemplateColumn DataField="filename" AutoPostBackOnFilter="false" AllowFiltering="false" SortExpression="Equip"
                                                                            CurrentFilterFunction="Contains" UniqueName="filename" HeaderText="File Name" ShowFilterIcon="false" HeaderStyle-Width="150">
                                                                            <ItemTemplate>
                                                                                <asp:LinkButton ID="lblName" runat="server" CausesValidation="false" CommandArgument='<%#Eval("filename") + "," + Eval("Path") %>'
                                                                                    OnClientClick="return ViewDocumentClick(this);" OnClick="lblName_Click" Text='<%# Eval("filename") %>'> </asp:LinkButton>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>

                                                                        <telerik:GridTemplateColumn DataField="doctype" AutoPostBackOnFilter="false" AllowFiltering="false" SortExpression="doctype"
                                                                            CurrentFilterFunction="Contains" UniqueName="doctype" HeaderText="File Type" ShowFilterIcon="false" HeaderStyle-Width="100">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblType" runat="server" Text='<%# Eval("doctype") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>
                                                                        <telerik:GridTemplateColumn DataField="Path" AutoPostBackOnFilter="false" AllowFiltering="false" SortExpression="Path"
                                                                            CurrentFilterFunction="Contains" UniqueName="Path" HeaderText="Path" ShowFilterIcon="false" HeaderStyle-Width="200">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblPath" runat="server" Text='<%# Eval("Path") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <FooterTemplate><%= RadgvDocuments.VirtualItemCount  %>  Record(s) Found.   </FooterTemplate>
                                                                        </telerik:GridTemplateColumn>
                                                                    </Columns>
                                                                </MasterTableView>
                                                            </telerik:RadGrid>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </asp:Panel>
                                    </div>
                                </div>
                                <div style="clear: both;"></div>
                            </div>
                        </li>
                        <li>
                            <div id="accrdInventory" style="display: block;" class="collapsible-header accrd accordian-text-custom"><i class="mdi-notification-vibration"></i>Inventory Used</div>
                            <div class="collapsible-body">
                                <div class="form-content-wrap">
                                    <div class="form-content-pd">
                                        <div class="grid_container">
                                            <%--<h1>Grid Here</h1>--%>
                                            <div class="RadGrid RadGrid_Material FormGrid">
                                                <%--<telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
                                                        <script type="text/javascript">
                                                            function pageLoad() {
                                                                try {
                                                                    var grid = $find("<%= RadGrid_Inventory.ClientID %>");
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
                                                    </telerik:RadCodeBlock>--%>
                                                <%--<telerik:RadAjaxPanel ID="RadAjaxLoadingPanel_Setup" runat="server" LoadingPanelID="RadAjaxLoadingPanel_Setup1">--%>
                                                <telerik:RadGrid RenderMode="Auto" ID="RadGrid_Inventory" AllowFilteringByColumn="false" ShowFooter="True" PageSize="50"
                                                    ShowStatusBar="true" runat="server" AllowSorting="true" Width="100%" PagerStyle-AlwaysVisible="true" OnItemCommand="RadGrid_Inventory_ItemCommand"
                                                    AllowCustomPaging="True">
                                                    <CommandItemStyle />
                                                    <GroupingSettings CaseSensitive="false" />
                                                    <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowKeyboardNavigation="true">
                                                        <Selecting AllowRowSelect="True"></Selecting>
                                                        <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                                        <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                                    </ClientSettings>
                                                    <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="True" ShowFooter="True">
                                                        <Columns>

                                                            <telerik:GridTemplateColumn DataField="Phase" AutoPostBackOnFilter="true" AllowFiltering="false" HeaderStyle-Width="70"
                                                                CurrentFilterFunction="Contains" HeaderText="Code" ShowFilterIcon="false">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtGvPhase" runat="server" CssClass="texttransparent phsearchinput"
                                                                        Text='<%# Bind("Phase") %>'></asp:TextBox>
                                                                    <asp:HiddenField ID="hdnPID" Value='<%# Eval("PhaseID") != DBNull.Value ? Eval("PhaseID") : "" %>' runat="server" />
                                                                    <asp:HiddenField ID="hdnTypeId" Value='<%# Eval("TypeID") != DBNull.Value ? Eval("TypeID") : "" %>' runat="server" />
                                                                    <asp:HiddenField ID="hdnAID" Value='<%# Eval("AID") %>' runat="server" />
                                                                    <asp:HiddenField ID="hdntxtGvPhase" Value='<%# Bind("Phase") %>' runat="server" />
                                                                    <asp:HiddenField ID="hdnLine" runat="server" Value='<%# Bind("Line") %>'></asp:HiddenField>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:Label ID="lblTotal" Text="Total" runat="server"></asp:Label>
                                                                </FooterTemplate>
                                                            </telerik:GridTemplateColumn>

                                                            <telerik:GridTemplateColumn DataField="ItemDesc" AutoPostBackOnFilter="true" AllowFiltering="false" HeaderStyle-Width="70"
                                                                CurrentFilterFunction="Contains" HeaderText="Item" ShowFilterIcon="false">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtGvItem" runat="server" CssClass="texttransparent pisearchinput"
                                                                        Text='<%# Bind("ItemDesc") %>'></asp:TextBox>
                                                                    <asp:HiddenField ID="hdnItemID" Value='<%# Eval("Inv") != DBNull.Value ? Eval("Inv") : "" %>' runat="server" />
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn DataField="fDesc" AutoPostBackOnFilter="true" AllowFiltering="false" HeaderStyle-Width="60"
                                                                CurrentFilterFunction="Contains" HeaderText="Item Description" ShowFilterIcon="false">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtGvDesc" runat="server" CssClass="texttransparent"
                                                                        Text='<%# Bind("fDesc") %>' MaxLength="255"></asp:TextBox>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn DataField="Quan" AutoPostBackOnFilter="true" AllowFiltering="false" HeaderStyle-Width="40"
                                                                CurrentFilterFunction="Contains" HeaderText="Quan" ShowFilterIcon="false">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtGvQuan" runat="server" CssClass="texttransparent" autocomplete="off"
                                                                        MaxLength="15" Text='<%# Bind("Quan") %>'
                                                                        onchange="CalTotalVal(this);"></asp:TextBox>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:Label ID="lblTotalQty" runat="server" Style="text-align: left;"></asp:Label>
                                                                </FooterTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn AutoPostBackOnFilter="true" AllowFiltering="false" HeaderStyle-Width="70"
                                                                CurrentFilterFunction="Contains" HeaderText="Warehouse" ShowFilterIcon="false">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtGvWarehouse"
                                                                        Text='<%# Bind("Warehousefdesc") %>'
                                                                        runat="server" CssClass="texttransparent Warehousesearchinput"></asp:TextBox>
                                                                    <asp:HiddenField ID="hdnWarehouse" runat="server" Value='<%# Bind("WarehouseID") %>'></asp:HiddenField>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>

                                                            <telerik:GridTemplateColumn AutoPostBackOnFilter="true" AllowFiltering="false" HeaderStyle-Width="60"
                                                                CurrentFilterFunction="Contains" HeaderText="Warehouse Location" ShowFilterIcon="false">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtGvWarehouseLocation"
                                                                        Text='<%# Bind("Locationfdesc") %>'
                                                                        runat="server" CssClass="texttransparent WarehouseLocationsearchinput "></asp:TextBox>
                                                                    <asp:HiddenField ID="hdnWarehouseLocationID" runat="server" Value='<%# Bind("LocationID") %>'></asp:HiddenField>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>


                                                            <telerik:GridTemplateColumn DataField="Price" AutoPostBackOnFilter="true" AllowFiltering="false" HeaderStyle-Width="40"
                                                                CurrentFilterFunction="Contains" HeaderText="Price" ShowFilterIcon="false" Display="false">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtGvPrice" runat="server" CssClass="texttransparent" autocomplete="off"
                                                                        MaxLength="15" Text='<%# Bind("Price") %>'
                                                                        onchange="CalTotalVal(this);"></asp:TextBox>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>

                                                            <telerik:GridTemplateColumn DataField="Amount" AutoPostBackOnFilter="true" AllowFiltering="false" HeaderStyle-Width="40"
                                                                CurrentFilterFunction="Contains" HeaderText="$Amount" ShowFilterIcon="false" Display="false">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtGvAmount" runat="server" CssClass="texttransparent clsAmount" autocomplete="off"
                                                                        MaxLength="15" onkeypress="return isDecimalKey(this,event)" Text='<%# Bind("Amount") %>'
                                                                        onchange="CalculateTotal(this);"></asp:TextBox>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:Label ID="lblTotalAmt" runat="server" Style="text-align: left;"></asp:Label>
                                                                </FooterTemplate>
                                                            </telerik:GridTemplateColumn>


                                                            <telerik:GridTemplateColumn DataField="Billed" AutoPostBackOnFilter="true" AllowFiltering="false" HeaderStyle-Width="40"
                                                                CurrentFilterFunction="Contains" HeaderText="Chargeable" ShowFilterIcon="false">
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkBill"  runat="server" Checked='<%# (Convert.ToString(Eval("Billed")) == "1") ? true : false %>' />
                                                                    <asp:HiddenField ID="hdnBill" runat="server" Value='<%# Bind("Billed") %>'></asp:HiddenField>
                                                                </ItemTemplate>

                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn AutoPostBackOnFilter="true" AllowFiltering="false"
                                                                CurrentFilterFunction="Contains" HeaderText="Action" ShowFilterIcon="false" HeaderStyle-Width="40">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="ibDelete" runat="server" CommandName="DeleteTransaction"
                                                                        CommandArgument="<%#Container.ItemIndex%>"
                                                                        ImageUrl="~/images/glyphicons-17-bin.png" Width="13px" />
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>

                                                        </Columns>
                                                    </MasterTableView>
                                                </telerik:RadGrid>
                                                <%--</telerik:RadAjaxPanel>--%>
                                                <%--  </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnAddNewLines" />
                        </Triggers>
                    </asp:UpdatePanel>--%>
                                            </div>
                                        </div>
                                        <div class="row" style="margin-top: 10px!important;">
                                            <asp:LinkButton ID="btnAddNewLines" runat="server" CausesValidation="false" OnClientClick="itemJSON();" Style="color: #000; font-size: 1.5em;" OnClick="btnAddNewLines_Click">
                                                    <i class="mdi-content-add-circle" style="color: #2bab54;font-size: 1.2em; font-weight: bold;margin-left:10px;"></i>
                                            </asp:LinkButton>
                                        </div>
                                        <div class="cf"></div>
                                    </div>
                                </div>
                                <div style="clear: both;"></div>
                            </div>
                        </li>
                        <li id="tbLogs" runat="server" style="display: block">
                            <div id="accrdlogs" style="display: block;" class="collapsible-header accrd accordian-text-custom"><i class="mdi-content-content-paste"></i>Logs</div>
                            <div class="collapsible-body">
                                <div class="form-content-wrap">
                                    <div class="form-content-pd">
                                        <div class="grid_container">
                                            <div class="form-section-row" style="margin-bottom: 0 !important;">
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


                                                            function LoadaccrdResolutiontab() {

                                                                var x = document.getElementById("accrdResolution");

                                                                x.style.display = "block";
                                                            }

                                                            function LoadaccrdMCPtab() {

                                                                var x = document.getElementById("accrdMCP");

                                                                x.style.display = "block";
                                                            }

                                                            function LoadaccrdPOtab() {

                                                                var x = document.getElementById("accrdPO");

                                                                x.style.display = "block";
                                                            }

                                                            function LoadaccrdInventorytab() {

                                                                var x = document.getElementById("accrdInventory");

                                                                x.style.display = "block";
                                                            }


                                                            function LoadGvlog() {

                                                                var x = document.getElementById("accrdlogs");

                                                                x.style.display = "block";


                                                                if (document.getElementById('<%= hdnloadlogtab.ClientID%>').value == "0") {



                                                                    document.getElementById('<%= lnkloadlogtab.ClientID%>').click();
                                                                }
                                                            }

                                                        </script>
                                                    </telerik:RadCodeBlock>
                                                    <telerik:RadAjaxPanel ID="RadAjaxPanel_gvLogs" runat="server" LoadingPanelID="RadAjaxLoadingPanel_gvLogs" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">

                                                        <asp:LinkButton ID="lnkloadlogtab" runat="server" Text="" OnClick="lnkloadlogtab_Click" />

                                                        <asp:HiddenField ID="hdnloadlogtab" runat="server" Value="0" />


                                                        <telerik:RadGrid Visible="false" RenderMode="Auto" ID="RadGrid_gvLogs" AllowFilteringByColumn="true" ShowFooter="True" PageSize="10" PagerStyle-AlwaysVisible="true" OnItemCreated="RadGrid_gvLogs_ItemCreated"
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



    <%--  ---------$$$$$$$$$$$$$$$  OLD  Design   $$$$$$$$$$$$$$-----------------%>


    <%--  ------Reason-----%>

    <div runat="server" id="pnlTranslate" style="display: none;">
        <asp:HiddenField ID="hdnIsEdited" runat="server" Value="0" />
        <div>
            Spanish text (sent from/to device)
                                                                            <asp:LinkButton ID="lnkTransReasonToEnglish" runat="server" CausesValidation="False"
                                                                                OnClick="lnkTransReasonToEnglish_Click">
                                                                               Translate to English</asp:LinkButton>
            <a id="lnkCloseTranslate" onclick="$('#<%=pnlTranslate.ClientID%>').hide();" style="float: right; vertical-align: top; margin-right: 10px; cursor: pointer">X</a>
        </div>
        <asp:TextBox ID="txtTranslate" runat="server"
            TextMode="MultiLine"></asp:TextBox>
    </div>

    <div id="divTransIconReason" runat="server" style="height: 55px; background-color: Gray; display: none">
        <div style="margin: 5px 5px 5px 5px">
            <asp:LinkButton ID="lnkTranslate" runat="server" CausesValidation="False" OnClick="lnkTranslate_Click">
                                                                            <img alt="Translate" src="images/translate.png" class="shadowHover" title="Translate to Spanish/English"
                                                                        width="20" height="20" /></asp:LinkButton>
        </div>
        <div style="margin: 5px 5px 5px 5px">
            <%--  <a id="btnTranslate" onclick="$('#<%=pnlTranslate.ClientID%>').show();" tabindex="32">
                <img alt="Translate" src="images/showdiv.png" class="shadowHover" title="Show Spanish Text"
                    width="20" height="20" />
            </a>--%>
        </div>
    </div>

    <asp:HoverMenuExtender ID="HoverMenuExtender1" runat="server" PopupControlID="divTransIconReason"
        TargetControlID="txtReason" Enabled="True" DynamicServicePath="" HoverDelay="1"
        OffsetX="-30">
    </asp:HoverMenuExtender>


    <div class="page-content" style="display: none;">

        <asp:TabContainer ID="TabContainer2" runat="server" ActiveTabIndex="0">
            <asp:TabPanel ID="TabPanel1" runat="server" HeaderText="Ticket Info">

                <ContentTemplate>







                    <div class="fc-input">
                        <div runat="server" id="pnlTransDesc" style="display: none;">
                            <div>
                                Spanish text (sent from/to device)
                                                                                <asp:LinkButton ID="lnkTransDescToEnglish" runat="server" CausesValidation="False"
                                                                                    OnClick="lnkTransDescToEnglish_Click">
                                                                                   Translate to English</asp:LinkButton>
                                <%-- <a id="lnkClosetransDesc" onclick="$('#<%=pnlTransDesc.ClientID%>').hide();" style="float: right; vertical-align: top; margin-right: 10px; cursor: pointer">X</a>--%>
                            </div>
                            <asp:TextBox ID="txtTransDesc" runat="server"
                                TextMode="MultiLine"></asp:TextBox>
                        </div>
                        <div id="divTransicon" runat="server" style="width: 30px; height: 55px; background-color: Gray; display: none">
                            <div style="margin: 5px 5px 5px 5px">
                                <asp:LinkButton ID="lnkTransEnglish" runat="server" CausesValidation="False" OnClick="lnkTransEnglish_Click">
                                             <img alt="Translate" src="images/translate.png" class="shadowHover" title="Translate to Spanish/English"
                                            width="20" height="20" /></asp:LinkButton>
                            </div>
                            <div style="margin: 5px 5px 5px 5px">
                                <a id="lnkTransDesc" onclick="$('#<%=pnlTransDesc.ClientID%>').show();" tabindex="33">
                                    <img alt="Translate" src="images/showdiv.png" class="shadowHover" title="Show Spanish Text"
                                        width="20" height="20" />
                                </a>
                            </div>
                        </div>
                        <asp:HoverMenuExtender ID="HoverMenuExtender2" runat="server" PopupControlID="divTransicon"
                            TargetControlID="txtWorkCompl" Enabled="True" DynamicServicePath="" HoverDelay="1"
                            OffsetX="-30">
                        </asp:HoverMenuExtender>


                        <asp:Image ID="imgEmail" Visible="false" runat="server" ImageUrl="images/email_notf.png" ToolTip="Email Notified" />
                        <asp:Image ID="imgWeekend" Visible="false" runat="server" ImageUrl="images/weekend.png" ToolTip="Worked on Weekend" />
                        <asp:Image ID="imgAfterHours" Visible="false" runat="server" ImageUrl="images/hours.png" ToolTip="Worked Afterhours" />
                </ContentTemplate>
            </asp:TabPanel>



        </asp:TabContainer>

    </div>


    <%-------$$$$$$$$$$$$ HIDDEN FIELD $$$$$$$$$$$$$$$$$$-----%>

    <telerik:RadAjaxPanel runat="server" ID="RadAjaxPanelHiddenField">
        <asp:HiddenField runat="server" ID="HiddenFieldCanada" />
        <asp:HiddenField runat="server" ID="MSTimeDataFieldVisibility" />
        <asp:HiddenField runat="server" ID="HiddenFieldRT" />
        <asp:HiddenField runat="server" ID="HiddenFieldOT" />
        <asp:HiddenField runat="server" ID="HiddenFieldNT" />
        <asp:HiddenField runat="server" ID="HiddenFieldDT" />
        <asp:HiddenField runat="server" ID="HiddenFieldTT" />
        <asp:HiddenField runat="server" ID="HiddenFieldBT" />
        <asp:HiddenField runat="server" ID="HiddenFieldBTFlag" Value="0" />
        <asp:HiddenField runat="server" ID="HiddenFielLocOnCreditHold" Value="0" />
        <!-- Hidden Field -->
        <asp:HiddenField runat="server" ID="hdnAddeDocument" Value="Y" />
        <asp:HiddenField runat="server" ID="hdnEditeDocument" Value="Y" />
        <asp:HiddenField runat="server" ID="hdnDeleteDocument" Value="Y" />
        <asp:HiddenField runat="server" ID="hdnViewDocument" Value="Y" />
        <asp:HiddenField runat="server" ID="hdnIsAddPO" Value="0" />
        <asp:HiddenField runat="server" ID="hdnIsPostBack" Value="0" />
        <asp:HiddenField runat="server" ID="hdnIsCreateJob" Value="0" />
        <asp:HiddenField runat="server" ID="hdnQuickBooksIntegration" Value="0" />

        <asp:Button runat="server" ID="btnRebindRadWindow" CausesValidation="false" />

        <asp:HiddenField runat="server" ID="hdnFocus" />
        <asp:HiddenField ID="hdnReviewed" runat="server" />
        <asp:HiddenField ID="hdnProspect" runat="server" />
        <asp:HiddenField ID="hdnImg" runat="server" />
        <asp:HiddenField ID="hdnCon" runat="server" />
        <asp:HiddenField ID="hdnPatientId" runat="server" />
        <asp:HiddenField ID="hdnLocId" runat="server" />
        <asp:HiddenField ID="hdnUnitID" runat="server" />
        <asp:HiddenField ID="hdnProjectId" runat="server" />
        <asp:HiddenField ID="hdnRolID" runat="server" />
        <asp:HiddenField ID="hdnLang" runat="server" />
        <asp:HiddenField ID="hdnMultiLang" runat="server" />
        <asp:HiddenField ID="hdnFormId" runat="server" />
        <asp:HiddenField ID="hdnComp" runat="server" />
        <asp:HiddenField ID="hdnProjectCode" runat="server" />
        <asp:HiddenField ID="hdnSageInt" runat="server" />
        <asp:HiddenField ID="hdnProjectTaskCategory" runat="server" />
        <%--inventory--%>
        <asp:HiddenField ID="hdnItemJSON" runat="server" />
        <asp:Button ID="btnValidateLocation" runat="server" Style="display: none;" OnClick="txtCustomer_TextChanged"
            CausesValidation="False" />
    </telerik:RadAjaxPanel>

    <%-------$$$$$$$$$$$$$$ RAD WINDOW $$$$$$$$$$$$$$$$-----%>

    <telerik:RadWindowManager ID="RadWindowManagerTicket" runat="server">
        <Windows>

            <telerik:RadWindow ID="RadWindowLead" Skin="Material" VisibleTitlebar="true" Title="Convert Lead" Behaviors="Default" CenterIfModal="true"
                Animation="FlyIn" AnimationDuration="200" RenderMode="Auto" VisibleStatusbar="false"
                runat="server" Modal="true" Width="600" Height="200">
                <ContentTemplate>
                    <asp:Panel ID="pnlCustomer" runat="server" Visible="False" Width="100%">
                        <div class="form-section-row" style="height: 60px">
                            <div class="row" style="height: 60px">
                                <div class="section-ttle" style="margin-top: 5px;">
                                    Please select an existing customer or leave the field blank and this will create
                                            a new Customer.
                                </div>
                                <div class="input-field col s12" style="height: 100px">
                                    <div class="row" style="height: 100px">
                                        <uc1:uc_customersearch ID="uc_CustomerSearch1" runat="server" />
                                    </div>
                                </div>


                            </div>
                        </div>
                    </asp:Panel>
                </ContentTemplate>
            </telerik:RadWindow>

            <telerik:RadWindow ID="RadWindowReasonForServervice" Skin="Material" VisibleTitlebar="true" Title="" Behaviors="Default" CenterIfModal="true"
                Animation="FlyIn" AnimationDuration="200" RenderMode="Auto" VisibleStatusbar="false" MinWidth="600"
                runat="server" Modal="true" Width="600" Height="300">
                <ContentTemplate>


                    <asp:Panel ID="pnlCodes" runat="server" CssClass="pnl-codes" Visible="False">
                        <div class="model-popup-body">
                            <asp:Label ID="lblCodeHeader" runat="server" CssClass="title_text"></asp:Label>
                            <asp:LinkButton ID="btnCancel" runat="server" Visible="false" CausesValidation="False" OnClick="btnCancel_Click" Style="color: black; float: right; margin-left: 10px"
                                Text="Close" CssClass="buttonsBox" />
                            <asp:LinkButton ID="btnDone" runat="server" CausesValidation="False" OnClick="btnDone_Click" Style="color: black; float: right;"
                                Text="Save" CssClass="buttonsBox" />
                        </div>


                        <telerik:RadAjaxPanel runat="server" ID="RadAjaxPanel1">

                            <div class="grid_container">
                                <div style="width: 40%; float: left;" class="RadGrid RadGrid_Material FormGrid">
                                    <b>&nbsp;Categories </b>
                                    <asp:ListBox ID="ddlCodeCat" runat="server" AutoPostBack="True" Style="padding: 5px !important;" OnSelectedIndexChanged="ddlCodeCat_SelectedIndexChanged"
                                        Height="170px" Width="165px"></asp:ListBox>
                                </div>
                                <div style="width: 40%; float: left;" class="RadGrid RadGrid_Material FormGrid">
                                    <b>Description </b>
                                    <asp:CheckBoxList ID="chklstCodes" runat="server">
                                    </asp:CheckBoxList>

                                </div>

                            </div>

                        </telerik:RadAjaxPanel>


                    </asp:Panel>



                </ContentTemplate>
            </telerik:RadWindow>

            <telerik:RadWindow ID="RadWindowlocContractinfo" Skin="Material" VisibleTitlebar="true" Title="Contract Info" Behaviors="Default" CenterIfModal="true"
                Animation="FlyIn" AnimationDuration="200" RenderMode="Auto" VisibleStatusbar="false" MinWidth="600"
                runat="server" Modal="true" Width="600" Height="320">
                <ContentTemplate>
                    <div>
                        <div class="grid_container">
                            <div class="RadGrid RadGrid_Material FormGrid">
                                <telerik:RadAjaxPanel ID="RadAjaxPanelRadGVContractInfo" runat="server">
                                    <telerik:RadGrid ID="RadGVContractInfo"
                                        RenderMode="Auto" AllowFilteringByColumn="false" ShowFooter="True" PageSize="10"
                                        ShowStatusBar="true" runat="server" AllowPaging="false" AllowSorting="false" Width="100%" PagerStyle-AlwaysVisible="true"
                                        AllowCustomPaging="True">
                                        <CommandItemStyle />
                                        <GroupingSettings CaseSensitive="false" />
                                        <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                                            <Selecting AllowRowSelect="True"></Selecting>
                                            <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                            <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                        </ClientSettings>
                                        <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="True" ShowFooter="True">
                                            <Columns>
                                                <telerik:GridTemplateColumn DataField="ContractType" AutoPostBackOnFilter="false" AllowFiltering="false" SortExpression="ContractType"
                                                    CurrentFilterFunction="Contains" UniqueName="ContractType" HeaderText="Contract Type" ShowFilterIcon="false" HeaderStyle-Width="150">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblContractType" runat="server" Text='<%# Eval("ContractType") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn DataField="ScheduleFrequency" AutoPostBackOnFilter="false" AllowFiltering="false" SortExpression="ScheduleFrequency"
                                                    CurrentFilterFunction="Contains" UniqueName="ScheduleFrequency" HeaderText="Schedule Frequency" ShowFilterIcon="false" HeaderStyle-Width="150">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblScheduleFrequency" runat="server" Text='<%# Eval("ScheduleFrequency") %>'></asp:Label>
                                                    </ItemTemplate>

                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn DataField="" AutoPostBackOnFilter="false" AllowFiltering="false" SortExpression=""
                                                    CurrentFilterFunction="Contains" UniqueName="" HeaderText="" ShowFilterIcon="false" HeaderStyle-Width="100">

                                                    <FooterTemplate>
                                                        <footertemplate><%= RadGVContractInfo.VirtualItemCount  %>  Record(s) Found.   </footertemplate>
                                                    </FooterTemplate>
                                                </telerik:GridTemplateColumn>

                                            </Columns>
                                        </MasterTableView>
                                    </telerik:RadGrid>
                                </telerik:RadAjaxPanel>
                            </div>
                        </div>
                </ContentTemplate>
            </telerik:RadWindow>

            <telerik:RadWindow ID="RadWindow1RelatedTicket" Skin="Material" VisibleTitlebar="true" Title="Related Ticket" Behaviors="Default" CenterIfModal="true"
                Animation="FlyIn" AnimationDuration="200" RenderMode="Auto" VisibleStatusbar="false" MinWidth="600"
                runat="server" Modal="true" Width="900" Height="400">
                <ContentTemplate>
                    <div class="grid_container">
                        <div class="RadGrid RadGrid_Material FormGrid">

                            <telerik:RadAjaxPanel ID="RadAjaxPanelForRadGvlstRelatedTickets" runat="server">

                                <telerik:RadGrid ID="RadGvlstRelatedTickets"
                                    RenderMode="Auto" AllowFilteringByColumn="false" ShowFooter="True" PageSize="10"
                                    ShowStatusBar="true" runat="server" AllowPaging="false" AllowSorting="false" Width="100%" PagerStyle-AlwaysVisible="true"
                                    AllowCustomPaging="True">
                                    <CommandItemStyle />
                                    <GroupingSettings CaseSensitive="false" />
                                    <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                                        <Selecting AllowRowSelect="True"></Selecting>
                                        <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                        <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                    </ClientSettings>
                                    <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="True" ShowFooter="True">

                                        <Columns>
                                            <%--CurrentFilterFunction="Contains"--%>
                                            <telerik:GridTemplateColumn DataField="id" AutoPostBackOnFilter="false" AllowFiltering="false" SortExpression="id"
                                                UniqueName="id" HeaderText="Ticket#" ShowFilterIcon="false" HeaderStyle-Width="50">
                                                <ItemTemplate>
                                                    <asp:HyperLink runat="server" Target="_blank" ID="lblId"
                                                        NavigateUrl='<%# String.Format("addticket.aspx?id={0}&comp={1}", Eval("id"), Eval("comp")) %>'
                                                        Text='<%#Eval("ID") %>'></asp:HyperLink>
                                                </ItemTemplate>

                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn DataField="locname" AutoPostBackOnFilter="false" AllowFiltering="false" SortExpression="locname"
                                                CurrentFilterFunction="Contains" UniqueName="locname" HeaderText="Location" ShowFilterIcon="false" HeaderStyle-Width="150">
                                                <ItemTemplate>

                                                    <strong>
                                                        <asp:Label runat="server" ID="lblLoc"><%#Eval("locname") %></asp:Label></strong>
                                                </ItemTemplate>

                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn DataField="fdesc" AutoPostBackOnFilter="false" AllowFiltering="false" SortExpression="fdesc"
                                                CurrentFilterFunction="Contains" UniqueName="fdesc" HeaderText="Reason" ShowFilterIcon="false" HeaderStyle-Width="150">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblReason"><%# objGeneralFunctions.TruncateWithText(Eval("fdesc").ToString(), 29)%></asp:Label>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn DataField="edate" AutoPostBackOnFilter="false" AllowFiltering="false" SortExpression="edate"
                                                CurrentFilterFunction="Contains" UniqueName="edate" HeaderText="Date" ShowFilterIcon="false" HeaderStyle-Width="150">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblDate"><%# Eval("edate", "{0:MM/dd/yy hh:mm tt}") %></asp:Label>

                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn DataField="assignname" AutoPostBackOnFilter="false" AllowFiltering="false" SortExpression="assignname"
                                                CurrentFilterFunction="Contains" UniqueName="assignname" HeaderText="Status" ShowFilterIcon="false" HeaderStyle-Width="100">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblStatus"><%#Eval("assignname") %></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate><%= RadGvlstRelatedTickets.VirtualItemCount  %>  Record(s) Found.   </FooterTemplate>
                                            </telerik:GridTemplateColumn>

                                        </Columns>
                                        <NoRecordsTemplate>No Record Found.   </NoRecordsTemplate>
                                    </MasterTableView>
                                </telerik:RadGrid>

                            </telerik:RadAjaxPanel>
                        </div>
                    </div>
                </ContentTemplate>
            </telerik:RadWindow>

            <telerik:RadWindow ID="RadWindow1Signature" Skin="Material" VisibleTitlebar="true" Title="Signature" Behaviors="Default" CenterIfModal="true"
                Animation="FlyIn" AnimationDuration="200" RenderMode="Auto" VisibleStatusbar="false"
                runat="server" Modal="true" Width="350" Height="280">
                <ContentTemplate>
                    <div id="sign" tabindex="0" class="sign_popup sigPad" style="width: 300px; outline: none;">
                        <div>
                            <div class="btnlinks">
                                <a class="clearButton">Clear Signature</a>
                            </div>
                            <div class="btnlinks">
                                <a id="convertpngbtn" class="sign-title-r">Accept </a>
                            </div>
                        </div>
                        <div class="sig" style="outline: none !important;">
                            <div class="typed">
                            </div>
                            <canvas class="pad" style="border: 1px solid #9e9e9e; border-radius: 6px; position: relative; margin-top: 10px; background-color: #fff;"
                                id="canvas"></canvas>
                            <input id="hdnDrawdata" tabindex="43" type="hidden" name="output" class="output" style="outline: none !important;" />
                        </div>
                    </div>
                </ContentTemplate>
            </telerik:RadWindow>

            <telerik:RadWindow ID="RadWindow1" Skin="Material" VisibleTitlebar="true" Title="" Visible="true" Behaviors="Close" CenterIfModal="true"
                Animation="FlyIn" AnimationDuration="200" RenderMode="Mobile" VisibleStatusbar="false"
                runat="server" Modal="true" Width="500" Height="240">
                <ContentTemplate>
                    <%--//////////////// Confirm for Invoicing ///////////////////--%>

                    <!-- Modal content -->
                    <span class="section-ttle">Would you like to invoice by ticket, project or work order?</span>

                    <div>

                        <input id="rdTicket" class="with-gap" name="radio-group" type="radio" value="Ticket" checked>

                        <label for="rdTicket">Ticket</label>

                        <br />

                        <input id="rdProject" class="with-gap" name="radio-group" type="radio" value="Project">

                        <label for="rdProject">Project</label>

                        <br />

                        <input id="rdWorkorderonly" class="with-gap" name="radio-group" type="radio" value="Workorderonly">

                        <label for="rdWorkorderonly">Work order</label>

                    </div>

                    <br />


                    &nbsp;
                    <asp:CheckBox ID="Reviewonly" runat="server" Checked="true" class="css-checkbox" TabIndex="12" Text="Reviewed" />

                    &nbsp; &nbsp;<asp:CheckBox ID="Combind" runat="server" class="css-checkbox" Style="display: none;" TabIndex="12" Text="Separate" />



                    <br />

                    <div style="float: right;" class="btnlinks">
                        <asp:LinkButton ID="LinkButton1" ToolTip="Cancel" runat="server" OnClick="LinkButton1_Click" CausesValidation="false">Cancel</asp:LinkButton>
                        &nbsp; &nbsp;
                                            <asp:LinkButton ID="lnkProcessInvoicingnext" ToolTip="Process Invoicing" runat="server" CausesValidation="false"
                                                OnClientClick="return ProcessInvoicingnext();">Next</asp:LinkButton>

                    </div>

                </ContentTemplate>
            </telerik:RadWindow>

        </Windows>
    </telerik:RadWindowManager>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="javascriptHolder" runat="Server">
    <script defer src="https://use.fontawesome.com/releases/v5.0.10/js/all.js"></script>
    <!-- dropify -->
    <script type="text/javascript" src="Design/js/plugins/dropify/js/dropify.min.js"></script>
    <telerik:RadCodeBlock ID="RadCodeBlock21" runat="server">
        <script type="text/javascript"> 
            //Inventory
            function itemJSON() {
                debugger
                <%-- var rawData = $('#<%=gvGLItems.ClientID%>').serializeFormJSON();--%>
                var rawData = $('#<%=RadGrid_Inventory.ClientID%>').serializeFormJSON();
                var formData = JSON.stringify(rawData);
                $('#<%=hdnItemJSON.ClientID%>').val(formData);
            }
            //$(document).ready(function ()
            //{
            (function ($) {
                $.extend({
                    toDictionary: function (query) {
                        var parms = {};
                        var items = query.split("&");
                        for (var i = 0; i < items.length; i++) {
                            var values = items[i].split("=");
                            var key1 = decodeURIComponent(values.shift().replace(/\+/g, '%20'));
                            var key = key1.split('$')[key1.split('$').length - 1];
                            var value = values.join("=")
                            parms[key] = decodeURIComponent(value.replace(/\+/g, '%20'));
                        }
                        return (parms);
                    }
                })
            })(jQuery);
            (function ($) {
                $.fn.serializeFormJSON = function () {
                    var o = [];
                    $(this).find('tr:not(:first, :last)').each(function () {
                        var elements = $(this).find('input, textarea, select')
                        if (elements.size() > 0) {
                            var serialized = $(this).find('input, textarea, select').serialize();
                            var item = $.toDictionary(serialized);
                            o.push(item);
                        }
                    });
                    return o;
                };
            })(jQuery);
            function CalTotalVal(obj) {


                var txt = obj.id;

                var txtGvQuan;
                var txtGvPrice;
                var txtGvAmount;

                if (txt.indexOf("Quan") >= 0) {
                    txtGvQuan = document.getElementById(txt);
                    txtGvPrice = document.getElementById(txt.replace('txtGvQuan', 'txtGvPrice'));
                    txtGvAmount = document.getElementById(txt.replace('txtGvQuan', 'txtGvAmount'));
                }
                else if (txt.indexOf("Price") >= 0) {
                    txtGvPrice = document.getElementById(txt);
                    txtGvQuan = document.getElementById(txt.replace('txtGvPrice', 'txtGvQuan'));
                    txtGvAmount = document.getElementById(txt.replace('txtGvPrice', 'txtGvAmount'));
                }
                else if (txt.indexOf("Amount") >= 0) {
                    txtGvPrice = document.getElementById(txt.replace('txtGvAmount', 'txtGvPrice'));
                    txtGvQuan = document.getElementById(txt.replace('txtGvAmount', 'txtGvQuan'));
                    txtGvAmount = document.getElementById(txt);
                }

                if (!jQuery.trim($(txtGvQuan).val()) == '') {
                    if (isNaN(parseFloat($(txtGvQuan).val()))) {
                        $(txtGvQuan).val('0.00');
                    }
                }

                if (!jQuery.trim($(txtGvPrice).val()) == '') {
                    if (isNaN(parseFloat($(txtGvPrice).val()))) {
                        $(txtGvPrice).val('0.00');
                    }
                }

                if (!jQuery.trim($(txtGvQuan).val()) == '' && !jQuery.trim($(txtGvPrice).val()) == '') {
                    if (!isNaN(parseFloat($(txtGvQuan).val())) && !isNaN(parseFloat($(txtGvPrice).val()))) {
                        var valAmount = parseFloat($(txtGvQuan).val()) * parseFloat($(txtGvPrice).val());
                        $(txtGvAmount).val(valAmount.toFixed(2));
                    }
                }


                if (!isNaN(parseFloat(document.getElementById(obj.id).value))) {
                    document.getElementById(obj.id).value = parseFloat(document.getElementById(obj.id).value).toFixed(2);
                }
            }
            function dtaa() {
                this.prefixText = null;
                this.con = null;
                this.Acct = null;
            }
            InitializeGrids('<%=RadGrid_Inventory.ClientID%>');
            function InitializeGrids(Gridview) {

                var rowone = $("#" + Gridview).find('tr').eq(1);
                $("input", rowone).each(function () {
                    $(this).blur();
                });
            }
            var totalQty = 0.00;
            $("[id*=txtGvQuan]").each(function () {
                if (!jQuery.trim($(this).val()) == '') {
                    if (!isNaN(parseFloat($(this).val()))) {
                        totalQty = totalQty + parseFloat($(this).val());
                    } else
                        $(this).val('');
                }
                else {
                    $(this).val('');
                }
            });

            $('[id*=lblTotalQty]').text(totalQty.toFixed(2));


            /////  GET PHASE 


            $("[id*=txtGvPhase]").autocomplete({

                source: function (request, response) {
                    debugger
                    var curr_control = $("[id*=txtGvPhase]").attr('id');
                    var hdnProjectId = document.getElementById('<%=hdnProjectId.ClientID%>');
                    var job = hdnProjectId.value;
                    var prefixText = "Materials";

                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "AccountAutoFill.asmx/GetPhase",
                        data: '{"jobID": "' + job + '", "prefixText": "' + prefixText + '"}',
                        dataType: "json",
                        async: true,
                        success: function (data) {
                            var phase = {};
                            var phase1 = [];
                            phase = $.parseJSON(data.d);
                            $.each(phase, function (index) {
                                if (phase[index].TypeName == "Materials") {
                                    phase1[0] = phase[index];
                                }
                            });
                            var jsonConvertedData = JSON.stringify(phase1);
                            response($.parseJSON(jsonConvertedData));
                        },
                        error: function (result) {
                            alert("Due to unexpected errors we were unable to load type.");
                        },
                        complete: function () {
                            $(this).data('requestRunning', false);
                        }
                    });

                },
                deferRequestBy: 200,
                select: function (event, ui) {
                    debugger;
                    var txtGvPhase = this.id;
                    var hdnTypeId = document.getElementById(txtGvPhase.replace('txtGvPhase', 'hdnTypeId'));
                    var hdntxtGvPhase = document.getElementById(txtGvPhase.replace('txtGvPhase', 'hdntxtGvPhase'));
                    var str = ui.item.TypeName;
                    if (str == "No Record Found!") {
                        $(this).val("");
                    }
                    else {

                        $(hdnTypeId).val(ui.item.Type);
                        console.log(hdnTypeId.value)
                        $(this).val(ui.item.TypeName);
                        $(hdntxtGvPhase).val(ui.item.TypeName);
                        console.log(hdntxtGvPhase.value)

                    }
                    return false;
                },
                focus: function (event, ui) {

                    $(this).val(ui.item.TypeName);

                    return false;
                },
                minLength: 0,
                delay: 250
            }).click(function () {
                $(this).autocomplete('search', $(this).val())
            })
            $.each($(".phsearchinput"), function (index, item) {

                $(item).data("ui-autocomplete")._renderItem = function (ul, item) {
                    var ula = ul;
                    var itema = item;
                    var result_value = item.Type;
                    var result_item = item.TypeName;

                    return $("<li></li>")
                        .data("item.autocomplete", item)
                        .append("<a>" + result_item + "</a>")
                        .appendTo(ul);
                };
            });


            ///////////////  $$$$$$$$$$$$$$  INVETORY USED START $$$$$$$$$$$$$$$$$$$


            $("[id*=txtGvPhase]").change(function () {
                debugger
                var txtGvPhase = $(this);
                var strPhase = $(this).val();

                var txtGvPhase1 = $(txtGvPhase).attr('id');
                var hdnTypeId = document.getElementById(txtGvPhase1.replace('txtGvPhase', 'hdnTypeId'));
                var hdnPID = document.getElementById(txtGvPhase1.replace('txtGvPhase', 'hdnPID'));
                var txtGvItem = document.getElementById(txtGvPhase1.replace('txtGvPhase', 'txtGvItem'));
                var hdnItemID = document.getElementById(txtGvPhase1.replace('txtGvPhase', 'hdnItemID'));
                var txtGvDesc = document.getElementById(txtGvPhase1.replace('txtGvPhase', 'txtGvDesc'));

                if (strPhase != "") {
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "AccountAutoFill.asmx/GetAutoFillPhase",
                        data: '{"prefixText": "' + strPhase + '"}',
                        dataType: "json",
                        async: true,
                        success: function (data) {
                            debugger
                            var ui = $.parseJSON(data.d);

                            if (ui.length == 0) {
                                $(txtGvPhase).val('');
                                $(hdnTypeId).val('');
                                $(hdnPID).val('');
                                $(txtGvItem).val('');
                                $(hdnItemID).val('');
                                noty({
                                    text: 'Type \'' + strPhase + '\' doesn\'t exist!',
                                    type: 'warning',
                                    layout: 'topCenter',
                                    closeOnSelfClick: false,
                                    timeout: 5000,
                                    theme: 'noty_theme_default',
                                    closable: true
                                });
                            }
                            else {
                                var lbl = ui[0].Label;
                                var val = ui[0].Value;
                                $(txtGvPhase).val(lbl);
                                $(hdnTypeId).val(val);
                            }
                        },
                        error: function (result) {
                            alert("Due to unexpected errors we were unable to load Type");
                        }
                    });
                }
                else {
                    $(hdnPID).val('');
                    $(hdnTypeId).val('');
                    $(txtGvItem).val('');
                    $(hdnItemID).val('');
                    $(txtGvDesc).val('');
                }
            });


            //Item
            $("[id*=txtGvItem]").change(function () {

                var txtGvItem = $(this);
                var strItem = $(this).val();

                var txtGvItem1 = $(txtGvItem).attr('id');
                var hdnTypeId = document.getElementById(txtGvItem1.replace('txtGvItem', 'hdnTypeId'));
                var hdnPID = document.getElementById(txtGvItem1.replace('txtGvItem', 'hdnPID'));
                var txtGvItem = document.getElementById(txtGvItem1.replace('txtGvItem', 'txtGvItem'));
                var hdnItemID = document.getElementById(txtGvItem1.replace('txtGvItem', 'hdnItemID'));
                var txtGvDesc = document.getElementById(txtGvItem1.replace('txtGvItem', 'txtGvDesc'));
                var typeId = $(hdnTypeId).val();

                if (strItem != "") {

                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "AccountAutoFill.asmx/GetPhaseExpByJobTypePO",

                        data: '{"prefixText": "' + strItem + '", "typeId": "' + typeId + '", "job": "' + job + '"}',
                        dataType: "json",
                        async: true,
                        success: function (data) {
                            var ui = $.parseJSON(data.d);
                            //debugger;
                            if (ui.length == 0) {

                                //$(txtGvItem).val('');
                                $(hdnItemID).val('');
                                $(hdnPID).val('');


                            }
                            else {
                                $(txtGvItem).val(ui[0].ItemDesc1);
                                $(hdnItemID).val(ui[0].ItemID);
                                $(hdnPID).val(ui[0].Line);
                                $(txtGvDesc).val(ui[0].fDesc);
                            }
                        },
                        error: function (result) {
                            alert("Due to unexpected errors we were unable to load Item");
                        }
                    });
                }
                else {
                    $(hdnPID).val('');
                    $(hdnItemID).val('');
                }
            });

            $("[id*=txtGvItem]").autocomplete({
                source: function (request, response) {
                    var curr_control = this.element.attr('id');
                    var job = "0";
                    var typeId = "8";
                    //var typeId = document.getElementById(curr_control.replace('txtGvItem', 'hdnTypeId')).value; 
                    var prefixText = request.term;
                    query = request.term;

                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "AccountAutoFill.asmx/GetPhaseByInventoryItem",
                        data: '{"typeId": "' + typeId + '", "jobId": "' + job + '", "prefixText": "' + prefixText + '"}',
                        dataType: "json",
                        async: true,
                        success: function (data) {
                            var result = {};
                            var result1 = [];
                            result = $.parseJSON(data.d);
                            var i = 0;
                            $.each(result, function (index) {
                                if (result[index].INVtype == "0") {
                                    result1[i] = result[index];
                                    i = i + 1;
                                }
                            });
                            var jsonConvertedData = JSON.stringify(result1);
                            response($.parseJSON(jsonConvertedData));
                            //response($.parseJSON(data.d));
                        },
                        error: function (result) {
                            alert("Due to unexpected errors we were unable to load item.");
                        },
                        complete: function () {
                            $(this).data('requestRunning', false);
                        }
                    });

                    return false;
                },
                deferRequestBy: 50,
                select: function (event, ui) {
                    var curr_control = this.id;
                    var hdnItemID = document.getElementById(curr_control.replace('txtGvItem', 'hdnItemID'));
                    var txtGvDesc = document.getElementById(curr_control.replace('txtGvItem', 'txtGvDesc'));

                    var txtGvWarehouse = document.getElementById(curr_control.replace('txtGvItem', 'txtGvWarehouse'));
                    var hdnWarehouseID = document.getElementById(curr_control.replace('txtGvItem', 'hdnWarehouse'));

                    var txtGvWarehouseLocation = document.getElementById(curr_control.replace('txtGvItem', 'txtGvWarehouseLocation'));
                    var hdnWarehouseLocationID = document.getElementById(curr_control.replace('txtGvItem', 'hdnWarehouseLocationID'));

                    $(txtGvWarehouse).val("");
                    $(hdnWarehouseID).val("");
                    $(txtGvWarehouseLocation).val("");
                    $(hdnWarehouseLocationID).val("0");

                    var hdnPID = document.getElementById(curr_control.replace('txtGvItem', 'hdnPID'));
                    var job = "";
                    var str = ui.item.ItemDesc;
                    var strId = ui.item.ItemID;


                    if (strId == "0") {
                        $(this).val("");
                        $(hdnItemID).val("");
                        $(hdnPID).val("");

                    }
                    else {
                        if (ui.item.ItemID) {
                            var result = $(this);
                            if (ui.item.OnHand > 0) {
                                $(txtGvDesc).val(ui.item.fDesc);
                                $(hdnItemID).val(ui.item.ItemID);
                                $(hdnPID).val(ui.item.Line);
                                $(this).val(ui.item.ItemDesc1);
                                //$.ajax({
                                //    type: "POST",
                                //    contentType: "application/json; charset=utf-8",
                                //    url: "AccountAutoFill.asmx/IsItemOnHand",
                                //    data: '{"INVitemID": "' + ui.item.ItemID + '","WareHouseID": "0","WHLocationID": "0"}',
                                //    dataType: "json",
                                //    async: true,
                                //    success: function (data) {
                                //        if (data.d == "false") {
                                //            alert("Item selected is not on hand, please choose another one.");
                                //            $(hdnItemID).val("");
                                //            $(result).val("");
                                //            $(txtGvDesc).val("");
                                //        }
                                //        else {
                                //            var txtGvPrice = document.getElementById(result[0].id.replace('txtGvItem', 'txtGvPrice'));
                                //            $(txtGvPrice).val(parseFloat(data.d).toFixed(2));
                                //        }
                                //    },
                                //    error: function (result) {
                                //        alert("Due to unexpected errors we were unable to load item.");
                                //    },
                                //});
                                var txtGvPrice = document.getElementById(result[0].id.replace('txtGvItem', 'txtGvPrice'));
                                $(txtGvPrice).val(parseFloat(ui.item.Price).toFixed(2));
                                // Get warehouse 
                                var dtaaa = new dtaa();
                                dtaaa.prefixText = '';
                                dtaaa.InvID = ui.item.ItemID;
                                dtaaa.isShowAll = "no";
                                $.ajax({
                                    type: "POST",
                                    contentType: "application/json; charset=utf-8",
                                    url: "AccountAutoFill.asmx/GetWarehouseName",
                                    data: JSON.stringify(dtaaa),
                                    dataType: "json",
                                    async: true,
                                    success: function (data) {
                                        var warehouse = $.parseJSON(data.d);
                                        if (warehouse != null && warehouse.length == 1) {
                                            $(txtGvWarehouse).val(warehouse[0]["WarehouseName"]);
                                            $(hdnWarehouseID).val(warehouse[0]["WarehouseID"]);
                                        }
                                    },
                                    error: function (result) {
                                        // alert("Due to unexpected errors we were unable to load account name");
                                    }
                                });
                            } else {
                                alert("Item selected is not on hand, please choose another one.");
                                $(hdnItemID).val("");
                                $(result).val("");
                                $(txtGvDesc).val("");
                            }
                        }
                        else {
                            $(this).val("");
                            $(hdnPID).val(ui.item.Line);
                            $(txtGvDesc).val(ui.item.ItemDesc1);
                        }

                    }
                    return false;
                },
                focus: function (event, ui) {
                    if (ui.item) {
                        $(this).val(ui.item.ItemDesc1);
                    }
                    return false;
                },

                minLength: 0,
                delay: 250
            }).click(function () {
                $(this).autocomplete('search', $(this).val())
            })
            $.each($(".pisearchinput"), function (index, item) {

                $(item).data("ui-autocomplete")._renderItem = function (ul, item) {
                    var ula = ul;
                    var itema = item;
                    var result_value = item.ItemID;
                    var result_item = item.ItemDesc1;
                    var result_line = item.Line;
                    var result_itemfdesc = item.fDesc;
                    var OnHand = item.OnHand;


                    var x = new RegExp('\\b' + query, 'ig');

                    try {



                        if (result_item != null) {

                            result_item = result_item.replace(x, function (FullMatch, n) {
                                return '<span class="highlight">' + FullMatch + '</span>'
                            });

                        }

                        if (result_itemfdesc != null) {

                            result_itemfdesc = result_itemfdesc.replace(x, function (FullMatch, n) {
                                return '<span class="highlight">' + FullMatch + '</span>'
                            });

                        }

                    } catch{ }

                    if (result_line == "0") {
                        return $("<li></li>")
                            .data("item.autocomplete", item)
                            .append("<a>  " + result_item + ', Qty :' + OnHand + ", <span style='color:Gray;'><b>  </b>" + result_itemfdesc + "</span></a>")
                            .appendTo(ul);
                    }
                    else {

                        if (result_item == undefined) { result_item = 'No Record Found!'; }
                        return $("<li></li>")
                            .data("item.autocomplete", item)
                            .append("<span class='auto_item'><i style='display:inline-block; margin-right:8px;width:auto;color:#1565C0 !important;' class='fas fa-check-square' title=''></i>" + result_item + "</span>")
                            .appendTo(ul);

                    }
                };
            });
            //warehouse

            //txtGvWarehouse
            $("[id*=txtGvWarehouse]").autocomplete({
                source: function (request, response) {
                    //debugger;
                    var dtaaa = new dtaa();
                    dtaaa.prefixText = request.term;
                    query = request.term;

                    var str = request.term;

                    var txtGvWarehouse_GetID = $(this.element).attr("id");
                    var hdnInvID = document.getElementById(txtGvWarehouse_GetID.replace('txtGvWarehouse', 'hdnItemID'));

                    var hdntxtGvPhase = document.getElementById(txtGvWarehouse_GetID.replace('txtGvWarehouse', 'hdntxtGvPhase'));
                    //  if (hdntxtGvPhase.value != "Inventory") { return; }
                    console.log(hdntxtGvPhase.value);

                    var ID = $(hdnInvID).val();


                    dtaaa.InvID = ID;
                    dtaaa.isShowAll = "no";
                    //debugger;
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "AccountAutoFill.asmx/GetWarehouseName",
                        data: JSON.stringify(dtaaa),
                        dataType: "json",
                        async: true,
                        success: function (data) {
                            response($.parseJSON(data.d));
                        },
                        error: function (result) {
                            // alert("Due to unexpected errors we were unable to load account name");
                        }
                    });
                },
                select: function (event, ui) {
                    try {
                        var txtGvWarehouse = this.id;
                        var hdnWarehouse = document.getElementById(txtGvWarehouse.replace('txtGvWarehouse', 'hdnWarehouse'));
                        var hdnInvID = document.getElementById(txtGvWarehouse.replace('txtGvWarehouse', 'hdnItemID'));



                        var txtGvWarehouseLocation = document.getElementById(txtGvWarehouse.replace('txtGvWarehouse', 'txtGvWarehouseLocation'));
                        var hdnWarehouseLocationID = document.getElementById(txtGvWarehouse.replace('txtGvWarehouse', 'hdnWarehouseLocationID'));

                        $(txtGvWarehouseLocation).val("");
                        $(hdnWarehouseLocationID).val("0");


                        var Str = ui.item.WarehouseID + ", " + ui.item.WarehouseName;
                        //$(this).val(Str);
                        //$(txtGvWarehouse).val(Str);
                        $(this).val(ui.item.WarehouseName);
                        $(txtGvWarehouse).val(ui.item.WarehouseName);
                        $(hdnWarehouse).val(ui.item.WarehouseID);

                        var locationID = 0;
                        var warehouseID = $(hdnWarehouse).val();
                        var invID = $(hdnInvID).val();

                        //// Check ON Hand
                        $.ajax({
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            url: "AccountAutoFill.asmx/IsItemOnHand",
                            data: '{"INVitemID": "' + invID + '","WareHouseID": "' + ui.item.WarehouseID + '","WHLocationID": "0"}',
                            dataType: "json",
                            async: true,
                            success: function (data) {
                                if (data.d == "false") {
                                    alert("Item selected is not on hand this warehouse, please choose another one.");
                                    $(txtGvWarehouse).val('');
                                    $(hdnWarehouse).val('');
                                }
                            },
                            error: function (result) {
                                // alert("Due to unexpected errors we were unable to load item.");
                            },

                        });

                        ////

                    } catch{ }
                    return false;
                },
                focus: function (event, ui) {
                    try {
                        //$(this).val(ui.item.WarehouseID);
                        $(this).val(ui.item.WarehouseName);
                    } catch{ }
                    return false;
                },
                minLength: 0,
                delay: 250
            }).click(function () {
                $(this).autocomplete('search', $(this).val())
            });
            $.each($(".Warehousesearchinput"), function (index, item) {

                if (item && typeof item == "object")
                    $(item).data("ui-autocomplete")._renderItem = function (ul, item) {
                        debugger;
                        var ula = ul;
                        var itema = item;
                        var result_value = item.ID;
                        var result_item = item.WarehouseName;
                        var result_desc = item.WarehouseID;
                        var x = new RegExp('\\b' + query, 'ig'); // notice the escape \ here...            
                        result_item = result_item.replace(x, function (FullMatch, n) {
                            return '<span class="highlight">' + FullMatch + '</span>'
                        });
                        if (result_desc != null) {
                            result_desc = result_desc.replace(x, function (FullMatch, n) {
                                return '<span class="highlight">' + FullMatch + '</span>'
                            });
                        }

                        if (result_value == 0) {
                            return $("<li></li>")
                                .data("item.autocomplete", item)
                                .append("<a>" + result_item + "</a>")
                                .appendTo(ul);
                        }
                        else {
                            return $("<li></li>")
                                .data("item.autocomplete", item)
                                .append("<a style='color:blue;'>" + result_item + "</a>")
                                .appendTo(ul);
                        }
                    };
            });
            //warehouselocation

            //txtGvWarehouseLocation
            $("[id*=txtGvWarehouseLocation]").autocomplete({


                source: function (request, response) {

                    debugger;
                    var dtaaa = new dtaa();
                    dtaaa.prefixText = request.term;
                    query = request.term;
                    var str = request.term;

                    var txtGvWarehouseLocation_GetID = $(this.element).attr("id");
                    var hdnWarehouse = document.getElementById(txtGvWarehouseLocation_GetID.replace('txtGvWarehouseLocation', 'hdnWarehouse'));
                    var ID = $(hdnWarehouse).val();
                    var hdntxtGvPhase = document.getElementById(txtGvWarehouseLocation_GetID.replace('txtGvWarehouseLocation', 'hdntxtGvPhase'));
                    dtaaa.WarehouseID = ID;

                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "AccountAutoFill.asmx/GetWarehouseLocation",
                        data: JSON.stringify(dtaaa),
                        dataType: "json",
                        async: true,
                        success: function (data) {

                            response($.parseJSON(data.d));

                        },
                        error: function (result) {
                            alert("Due to unexpected errors we were unable to load account name");
                        }
                    });
                },
                select: function (event, ui) {
                    try {
                        var txtGvWarehouseLocation = this.id;
                        var hdnWarehouseLocationID = document.getElementById(txtGvWarehouseLocation.replace('txtGvWarehouseLocation', 'hdnWarehouseLocationID'));
                        var hdnInvID = document.getElementById(txtGvWarehouseLocation.replace('txtGvWarehouseLocation', 'hdnItemID'));
                        var hdnWarehouse = document.getElementById(txtGvWarehouseLocation.replace('txtGvWarehouseLocation', 'hdnWarehouse'));

                        var Str = ui.item.ID + ", " + ui.item.Name;
                        $(this).val(Str);
                        $(txtGvWarehouseLocation).val(Str);
                        $(hdnWarehouseLocationID).val(ui.item.ID);

                        var locationID = $(hdnWarehouseLocationID).val();
                        // alert(ui.item.ID);
                        var warehouseID = $(hdnWarehouse).val();
                        var invID = $(hdnInvID).val();


                        //// Check ON Hand
                        $.ajax({
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            url: "AccountAutoFill.asmx/IsItemOnHand",
                            data: '{"INVitemID": "' + invID + '","WareHouseID": "' + warehouseID + '","WHLocationID": "' + ui.item.ID + '"}',
                            dataType: "json",
                            async: true,
                            success: function (data) {
                                if (data.d == "false") {
                                    alert("Item selected is not on hand this location, please choose another one.");
                                    $(txtGvWarehouseLocation).val('');
                                    $(hdnWarehouseLocationID).val('');
                                }
                            },
                            error: function (result) {
                                alert("Due to unexpected errors we were unable to load item.");
                            },

                        });

                        ////


                    } catch{ }

                    return false;
                },
                focus: function (event, ui) {
                    try {
                        $(this).val(ui.item.ID);
                    } catch{ }
                    return false;
                },
                minLength: 0,
                delay: 250
            }).click(function () {
                $(this).autocomplete('search', $(this).val())
            })
            $.each($(".WarehouseLocationsearchinput"), function (index, item) {
                $(item).data("ui-autocomplete")._renderItem = function (ul, item) {
                    debugger;
                    var ula = ul;
                    var itema = item;
                    var result_value = item.ID;
                    var result_item = item.Name;

                    var x = new RegExp('\\b' + query, 'ig'); // notice the escape \ here...            
                    result_item = result_item.replace(x, function (FullMatch, n) {
                        return '<span class="highlight">' + FullMatch + '</span>'
                    });


                    if (result_value == 0) {
                        return $("<li></li>")
                            .data("item.autocomplete", item)
                            .append("<a>" + result_item + "</a>")
                            .appendTo(ul);
                    }
                    else {
                        return $("<li></li>")
                            .data("item.autocomplete", item)
                            .append("<a style='color:blue;'>" + result_item + "</a>")
                            .appendTo(ul);
                    }
                };
            });




            function isDecimalKey(el, evt) {
                var charCode = (evt.which) ? evt.which : event.keyCode;
                if (charCode == 45) {
                    return true;
                }
                var number = el.value.split('.');
                if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
                    return false;
                }
                if (number.length > 1 && charCode == 46) {
                    return false;
                }
                var caratPos = getSelectionStart(el);
                var dotPos = el.value.indexOf(".");
                if (caratPos > dotPos && dotPos > -1 && (number[1].length > 1)) {
                    return false;
                }
                return true;
            }

            function getSelectionStart(o) {
                if (o.createTextRange) {
                    var r = document.selection.createRange().duplicate()
                    r.moveEnd('character', o.value.length)
                    if (r.text == '') return o.value.length
                    return o.value.lastIndexOf(r.text)
                } else return o.selectionStart
            }





            //})
            //end inventory

            ///////////////  $$$$$$$$$$$$$$  INVETORY USED $$$$$$$$$$$$$$$$$$$


            function HideME() {
                $("#DivEqup").hide();
                $("#DivprojectType").hide();
                $("#DivProject").hide();
            }


            $(document).ready(function () {




                $('#phone-demo').mask("(999) 999-9999? Ext 99999");
                $('#phone-demo').bind('paste', function () { $(this).val(''); });
                $('#phone_cell').mask("(999) 999-9999");
                $('#phone_fax').mask("(999) 999-9999");
                $('#calledinTime').mask("99:99 aa")
                $('#Time').mask("99:99 aa")
                $('#EnrouteTime').mask("99:99 aa")
                $('#OnsiteTime').mask("99:99 aa")
                $('#CompletedTime').mask("99:99 aa")

                $('a[href^="#accrd"]').on('click', function (e) {
                    e.preventDefault();
                    $("a.anchorActive").removeClass("anchorActive");
                    $(this).addClass("anchorActive");
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
                    if (document.getElementsByClassName("add-btn-click")[0].innerHTML.indexOf("Show") != -1)
                        document.getElementsByClassName("add-btn-click")[0].innerHTML = "Hide Stats";
                    else
                        document.getElementsByClassName("add-btn-click")[0].innerHTML = "Show Stats";


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


                function collapseAll() {
                    $(".accrd").removeClass(function () {
                        return "active";
                    });
                    $('html, body').stop().animate({
                        'scrollTop': 0
                    }, 300, 'swing');
                    $(".collapsible-accordion").collapsible({ accordion: true });
                    $(".collapsible-accordion").collapsible({ accordion: false });

                }

                function expandAll() {
                    $('html, body').stop().animate({
                        'scrollTop': 0
                    }, 300, 'swing');
                    $(".accrd").addClass("active");
                    $(".collapsible-accordion").collapsible({ accordion: false });
                }

                $('.collapse-expand').on('click', function (e) {
                    if (this.classList.contains("opened") === true) {
                        this.classList.remove("opened");
                        this.innerHTML.replace(/mdi-action-open-in-browser/g, 'mdi-action-system-update-tv')
                        collapseAll();
                    }
                    else {
                        this.classList.add("is-active");
                        this.classList.add("opened");
                        this.innerHTML.replace(/mdi-action-system-update-tv/g, 'mdi-action-open-in-browser')
                        expandAll();
                    }
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
            });
        </script>

        <script>

            function initialize() {
                var address = new google.maps.LatLng(document.getElementById('<%= lat.ClientID %>').value, document.getElementById('<%= lng.ClientID %>').value);
                var marker;
                var map;
                var mapOptions = {
                    zoom: 13,
                    mapTypeId: google.maps.MapTypeId.ROADMAP,
                    center: address
                };

                map = new google.maps.Map(document.getElementById('map'),
                    mapOptions);

                marker = new google.maps.Marker({
                    map: map,
                    draggable: false,
                    position: address
                });

                if (document.getElementById('<%= hdnComp.ClientID %>').value != "2" && document.getElementById('<%= hdnComp.ClientID %>').value != "1") {
                    CallNearestWorker();
                }
            }

            function InitMap(lat, lng) {
                var address = new google.maps.LatLng(lat, lng);
                var marker;
                var map;
                var mapOptions = {
                    zoom: 13,
                    mapTypeId: google.maps.MapTypeId.ROADMAP,
                    center: address
                };

                map = new google.maps.Map(document.getElementById('map'),
                    mapOptions);

                marker = new google.maps.Marker({
                    map: map,
                    draggable: false,
                    position: address
                });
            }






            function SelectRowsEq() {

                var Name = document.getElementById("<%=txtUnit.ClientID %>");
                var div = document.getElementById('eqtag');
                div.innerHTML = '';
                Name.value = '';
                // debugger;

                var grid = $find("<%=RadgvEquip.ClientID %>");
                var masterTable = grid.get_masterTableView();
                if (masterTable != null) {
                    for (var i = 0; i < masterTable.get_dataItems().length; i++) {
                        var gridItemElement = masterTable.get_dataItems()[i].findElement("chkSelect");
                        var lblUnit = masterTable.get_dataItems()[i].findElement("lblUnit");
                        var lblID = masterTable.get_dataItems()[i].findElement("lblID");
                        if (gridItemElement.checked) {
                            if (Name.value != '') {
                                Name.value = Name.value + ', ' + lblUnit.innerHTML;
                            }
                            else {
                                Name.value = lblUnit.innerHTML;
                            }

                            var tag = "<div class='chip' style='width:auto !important;padding-left:5px !important;padding-right:5px !important ;margin-left:2px !important ;margin-right:2px !important ;margin-top:3px !important ;'><a href='addequipment.aspx?uid=" + lblID.innerHTML + "' target='_blank' style='color:white'>" + lblUnit.innerHTML + "</a></div>"

                            div.innerHTML += tag;
                        }
                    }
                }
            }


            function EqCheckBOX(checked) {

                // debugger;

                var grid = $find("<%=RadgvEquip.ClientID %>");
                var masterTable = grid.get_masterTableView();
                if (masterTable != null) {
                    for (var i = 0; i < masterTable.get_dataItems().length; i++) {
                        var gridItemElement = masterTable.get_dataItems()[i].findElement("chkSelect");
                        var lblUnit = masterTable.get_dataItems()[i].findElement("lblUnit");
                        gridItemElement.checked = checked;
                    }
                }
            }




            ////////////////////Project \\\\\\\\\\\\\\\\\\\\\\\\\


            function SelectProjectRow(gridview, id, name, chargeable, hdnProjectId, txtProject, projectNo, div) {
                debugger;
                var rowid = document.getElementById(id);
                var rowname = document.getElementById(name);
                var rowChargeable = document.getElementById(chargeable);
                var grid = document.getElementById(gridview);
                var hidden = document.getElementById(hdnProjectId);
                var hdnName = document.getElementById(txtProject);
                var projectNo = document.getElementById(projectNo);

                document.getElementById('<%= hdnIsJobChargable.ClientID %>').value = rowChargeable.innerHTML;

                ///Check if the job is charge then check chargeable boxes
                if (rowChargeable.innerHTML == '1') {
                    var chkChargeable = document.getElementById('<%=chkChargeable.ClientID%>');
                    var chkJobChargeable = document.getElementById('<%=chkJobChargeable.ClientID%>');
                    chkChargeable.checked = true; chkJobChargeable.checked = true
                }


                hdnName.value = rowid.innerHTML + ' ' + rowname.innerHTML;
                hidden.value = rowid.innerHTML;
                projectNo.value = rowid.innerHTML + ' ' + rowname.innerHTML;

                $("#DivProject").hide();

                Materialize.updateTextFields();

            }

            function SelectProjectTypeRow(gridview, id, name, hdnProjectCode, txtJobCode, projectNo, div) {

                var rowid = document.getElementById(id);
                var rowname = document.getElementById(name);

                var hidden = document.getElementById(hdnProjectCode);
                var hdnName = document.getElementById(txtJobCode);


                hdnName.value = rowname.innerHTML;
                hidden.value = rowid.innerHTML;


                $("#DivprojectType").hide();

                Materialize.updateTextFields();
            }


            function lblRelatedTickets_Client() {
                var wo = document.getElementById("<%=txtWO.ClientID %>");
                if (wo.value == '') {
                    noty({ text: 'Please enter WO', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                }
            }




        </script>

    </telerik:RadCodeBlock>

    <script type="text/javascript">



        function pageLoad(sender, args) {


            ///////////// Calculate total time fields handling (KEYUP) ////////////////////


            $("#<%=txtOT.ClientID%>").keyup(function (event) {
                calculateTotalTime();
            });

            $("#<%=txtNT.ClientID%>").keyup(function (event) {
                calculateTotalTime();
            });

            $("#<%=txtDT.ClientID%>").keyup(function (event) {
                calculateTotalTime();
            });

            $("#<%=txtTT.ClientID%>").keyup(function (event) {
                calculateTotalTime();
            });

            $("#<%=txtBT.ClientID%>").keyup(function (event) {
                DeductedValue(true);
            });



            /////////// Calculate total time fields handling (BLUR)  ////////////////////
            $("#<%=txtRT.ClientID%>").blur(function (event) {
                debugger;
                if (this.value == '') {
                    this.value = '0.00';
                } calculateTotalTime();
            });
            $("#<%=txtRT.ClientID%>").keyup(function (event) {
                debugger;
                calculateTotalTime();
            });
            $("#<%=txtOT.ClientID%>").blur(function (event) {
                if (this.value == '') {
                    this.value = '0.00';
                } calculateTotalTime();
            });

            $("#<%=txtNT.ClientID%>").blur(function (event) {
                if (this.value == '') {
                    this.value = '0.00';
                } calculateTotalTime();
            });

            $("#<%=txtDT.ClientID%>").blur(function (event) {
                if (this.value == '') {
                    this.value = '0.00';
                } calculateTotalTime();
            });

            $("#<%=txtTT.ClientID%>").blur(function (event) {
                if (this.value == '') {
                    this.value = '0.00';
                } calculateTotalTime();
            });

            $("#<%=txtBT.ClientID%>").blur(function (event) {
                if (this.value == '') {
                    this.value = '0.00';
                } DeductedValue(true);
            });



            ///////////// Calculate time fields handling (KEYUP)  ////////////////////
            $("#<%=txtEnrTime.ClientID%>").keyup(function (event) {
                calculate_Time();
            });

            $("#<%=txtOnsitetime.ClientID%>").keyup(function (event) {
                calculate_Time();
            });

            $("#<%=txtComplTime.ClientID%>").keyup(function (event) {
                calculate_Time();
            });

            ///////////// Calculate time fields handling (BLUR)  ////////////////////
            //            $("#<%=txtEnrTime.ClientID%>").blur(function(event) {
            //                calculate_Time();
            //            });

            //            $("#<%=txtOnsitetime.ClientID%>").blur(function(event) {
            //                calculate_Time();
            //            });

            //            $("#<%=txtComplTime.ClientID%>").blur(function(event) {
            //                calculate_Time();
            //            });


            ///////////// Calculate Mileage (KEYUP) ////////////////////
            $("#<%=txtMileStart.ClientID%>").keyup(function (event) {
                calculateMileage();
            });

            $("#<%=txtMileEnd.ClientID%>").keyup(function (event) {
                calculateMileage();
            });

            ///////////// Calculate Mileage (BLUR) ////////////////////
            $("#<%=txtMileStart.ClientID%>").blur(function (event) {
                calculateMileage();
            });

            $("#<%=txtMileEnd.ClientID%>").blur(function (event) {
                calculateMileage();
            });
            ///////////// Calculate Expenses (KEYUP) ////////////////////
            $("#<%=txtExpMisc%>").keyup(function (event) {
                calculateMileage();
            });

            $("#<%=txtExpToll.ClientID%>").keyup(function (event) {
                calculateMileage();
            });

            $("#<%=txtExpZone.ClientID%>").keyup(function (event) {
                calculateMileage();
            });

            ///////////// Calculate Expenses (BLUR) ////////////////////
            $("#<%=txtExpMisc.ClientID%>").blur(function (event) {
                calculateMileage();
            });

            $("#<%=txtExpToll.ClientID%>").blur(function (event) {
                calculateMileage();
            });

            $("#<%=txtExpZone.ClientID%>").blur(function (event) {
                calculateMileage();
            });

            $('#<%=txtRemarks.ClientID%>').focus(function () {
                var height = $(this)[0].scrollHeight;
                if (height != null && height > 12) {
                    $(this).animate({
                        //right: "+=0",
                        //width: '520px',
                        height: height - 12
                    }, 500, function () {
                        // Animation complete.
                    });
                }
            });

            $('#<%=txtRemarks.ClientID%>').blur(function () {
                $(this).animate({
                    width: '100%',
                    height: '63px'
                }, 500, function () {
                    // Animation complete.
                });
            });

            $('#<%=txtReason.ClientID%>').focus(function () {
                $(this).animate({
                    width: '520px',
                    height: '+=150px'
                }, 500, function () {
                    // Animation complete.
                });

                //$(this).zIndex(1);
            });

            $('#<%=txtReason.ClientID%>').blur(function () {
                $(this).animate({
                    width: '100%'
                    , height: '63px'
                }, 500, function () {
                    // Animation complete.
                });
                // $(this).zIndex(0);
            });

            ////////////////////////



            Materialize.updateTextFields();

            SelectRowsEq();


            $('#<%= lnkConvert.ClientID %>').click(function () {

                var window = $find('<%= RadWindowLead.ClientID %>');
                window.show();

            });

            $('#<%= ddlCategory.ClientID %>').change(function () {
                CategoryImage();
            });
            CategoryImage();




            ///////////// Ajax call for customer auto search ////////////////////                
            var query = "";
            function dtaa() {
                this.prefixText = null;
                this.con = document.getElementById('<%=hdnCon.ClientID%>').value;
                this.custID = null;
            }

            $("#<%=txtCustomer.ClientID%>").autocomplete({

                open: function (e, ui) {
                    /* create the scrollbar each time autocomplete menu opens/updates */
                    $(".ui-autocomplete").mCustomScrollbar({
                        setHeight: 182,
                        theme: "dark-3",
                        autoExpandScrollbar: true
                    });
                },
                response: function (e, ui) {
                    /* destroy the scrollbar after each search completes, before the menu is shown */
                    $(".ui-autocomplete").mCustomScrollbar("destroy");
                },

                source: function (request, response) {


                    var dtaaa = new dtaa();
                    dtaaa.prefixText = request.term;
                    query = request.term;
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "CustomerAuto.asmx/GetCustomerProspect",
                        //data: '{"prefixText":' + JSON.stringify(request.term) + ',"con":' + JSON.stringify(document.getElementById('ctl00_ContentPlaceHolder1_hdnCon').value) + '}',
                        data: JSON.stringify(dtaaa),
                        dataType: "json",
                        async: true,
                        success: function (data) {
                            response($.parseJSON(data.d));
                        },
                        //                        error: function(result) {
                        //                            alert("Due to unexpected errors we were unable to load customers");
                        //                        }
                        error: function (XMLHttpRequest, textStatus, errorThrown) {
                            var err = eval("(" + XMLHttpRequest.responseText + ")");
                            alert(err.Message);
                        }
                    });

                },
                select: function (event, ui) {
                    $("#<%=txtCustomer.ClientID%>").val(ui.item.label);
                        if (ui.item.prospect == 1) {
                            $("#<%=txtLocation.ClientID%>").val('');
                            $("#<%=hdnLocId.ClientID%>").val(ui.item.value);
                            $("#<%=txtLocation.ClientID%>").attr("disabled", "disabled");
                            $("#<%=hdnProspect.ClientID%>").val('1');
                            document.getElementById('<%=btnSelectLoc.ClientID%>').click();
                        }
                        else {
                            $("#<%=hdnPatientId.ClientID%>").val(ui.item.value);
                            $("#<%=txtLocation.ClientID%>").focus();
                            $("#<%=txtLocation.ClientID%>").val('');
                            $("#<%=hdnLocId.ClientID%>").val('');
                            $("#<%=hdnProspect.ClientID%>").val('');
                            document.getElementById('<%=btnSelectCustomer.ClientID%>').click();

                    }
                    return false;
                },
                focus: function (event, ui) {
                        // $("#<%=txtCustomer.ClientID%>").val(ui.item.label);
                    return false;
                },
                minLength: 0,
                delay: 50
            })


                .data("ui-autocomplete")._renderItem = function (ul, item) {
                    try {
                        var result_item = item.label;
                        var result_desc = item.desc;
                        var result_Prospect = item.prospect;
                        var x = new RegExp('\\b' + query, 'ig'); // notice the escape \ here...     


                        if (query != "") {

                            result_item = result_item.replace(x, function (FullMatch, n) {
                                return '<span class="highlight">' + FullMatch + '</span>'
                            });

                            if (result_desc != null) {
                                result_desc = result_desc.replace(x, function (FullMatch, n) {
                                    return '<span class="highlight">' + FullMatch + '</span>'
                                });
                            }
                        }
                        var color = '#222';
                        if (result_Prospect != 0) {
                            display = "inline-block";
                        }
                        else {
                            display = "none";
                        }
                        return $("<li></li>")
                            .data("item.autocomplete", item)
                            .append("<span class='auto_item'><i style='display:" + display + ";margin-right:8px;width:auto;color:#1565C0 !important;' class='fas fa-thumbs-up' title='Prospect'></i>" + result_item + "</span> <span class='auto_desc'>" + result_desc + "</span>")
                            .appendTo(ul);
                    } catch{ }
                };


            ///////////// Ajax call for location auto search ////////////////////
            var queryloc = "";
            $("#<%=txtLocation.ClientID%>").autocomplete(
                {

                    open: function (e, ui) {
                        /* create the scrollbar each time autocomplete menu opens/updates */
                        $(".ui-autocomplete").mCustomScrollbar({
                            setHeight: 182,
                            theme: "dark-3",
                            autoExpandScrollbar: true
                        });
                    },
                    response: function (e, ui) {
                        /* destroy the scrollbar after each search completes, before the menu is shown */
                        $(".ui-autocomplete").mCustomScrollbar("destroy");
                    },
                    source: function (request, response) {
                        //                        if (document.getElementById('ctl00_ContentPlaceHolder1_hdnPatientId').value != '') {
                        var dtaaa = new dtaa();
                        dtaaa.prefixText = request.term;
                        dtaaa.custID = 0;
                        if (document.getElementById('<%=hdnPatientId.ClientID%>').value != '') {
                                dtaaa.custID = document.getElementById('<%=hdnPatientId.ClientID%>').value;
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

                        });


                    },
                    select: function (event, ui) {
                        $("#<%=txtLocation.ClientID%>").val(ui.item.label);
                            $("#<%=hdnLocId.ClientID%>").val(ui.item.value);
                            document.getElementById('<%=btnSelectLoc.ClientID%>').click();
                        return false;
                    },
                    focus: function (event, ui) {
                            //  $("#<%=txtLocation.ClientID%>").val(ui.item.label);
                        return false;
                    },
                    minLength: 0,
                    delay: 10
                })
                .data("ui-autocomplete")._renderItem = function (ul, item) {
                    try {
                        var result_item = item.label;
                        var result_desc = item.desc;
                        var x = new RegExp('\\b' + queryloc, 'ig'); // notice the escape \ here...    

                        if (queryloc != "") {

                            result_item = result_item.replace(x, function (FullMatch, n) {
                                return '<span class="highlight">' + FullMatch + '</span>'
                            });

                            if (result_desc != null) {
                                result_desc = result_desc.replace(x, function (FullMatch, n) {
                                    return '<span class="highlight">' + FullMatch + '</span>'
                                });
                            }
                        }



                        return $("<li></li>")
                            .data("item.autocomplete", item)
                            .append("<span class='auto_item'>" + result_item + "</span> <span class='auto_desc'>" + result_desc + "</span>")
                            .appendTo(ul);
                    } catch{ }
                };


            ////////////MAP///////////
            $("#mapLink").click(function () {
                $("#map").toggle();
                $("#Coord").toggle();
                initialize();
            });


            initialize();



            ////////MAP/////



            ////////////////Project \\\\\\\\\\\\\\\\\

            ///////////// Validations for auto search ////////////////////

            $("#<%=txtCustomer.ClientID%>").keyup(function (e) {
                var hdnPatientId = document.getElementById('<%=hdnPatientId.ClientID%>');
                var txtLoc = document.getElementById('<%=txtLocation.ClientID%>');
                var txtCust = document.getElementById('<%=txtCustomer.ClientID%>');
                var hdnLocID = document.getElementById('<%=hdnLocId.ClientID%>');

                if ((e.which >= 46 && e.which <= 90) || (e.which >= 96 && e.which <= 105) || (e.which >= 186 && e.which <= 222) || e.which == 8) {
                    hdnPatientId.value = '';
                    hdnLocID.value = '';
                    $('#<%= lnkCustomerID.ClientID %>').removeAttr("href");
                }

                if (document.getElementById('<%=txtCustomer.ClientID%>').value == '') {
                    hdnPatientId.value = '';
                    $('#<%= lnkLocationID.ClientID %>').removeAttr("href");
                }
            });

            $("#<%=txtCustomer.ClientID%>").blur(function (e) {
                CheckIsProspect();
            });

            $("#<%=txtLocation.ClientID%>").keyup(function (event) {
                var hdnLocId = document.getElementById('<%=hdnLocId.ClientID%>');
                if (document.getElementById('<%=txtLocation.ClientID%>').value == '') {
                    hdnLocId.value = '';
                    $('#<%= lnkLocationID.ClientID %>').removeAttr("href");
                }
            });


            ///////////// Unit dropdown control handling ////////////////////
            $("#<%=txtUnit.ClientID%>").keyup(function (event) {
                var hdnUnitId = document.getElementById('<%=hdnUnitID.ClientID%>');
                if (document.getElementById('<%=txtUnit.ClientID%>').value == '') {
                    hdnUnitId.value = '';
                }
            });

            $("#<%=txtProject.ClientID%>").keyup(function (event) {
                var hdnProjectId = document.getElementById('<%=hdnProjectId.ClientID%>');
                if (document.getElementById('<%=txtProject.ClientID%>').value == '') {
                    hdnProjectId.value = '';
                    $('#<%= lnkProjectID.ClientID %>').removeAttr("href");
                    $('#<%= ddlTemplate.ClientID %>').prop('disabled', false);

                }

            });



            $("#<%=txtJobCode.ClientID%>").keyup(function (event) {
                var hdnProjectCode = document.getElementById('<%=hdnProjectCode.ClientID%>');
                if (document.getElementById('<%=txtJobCode.ClientID%>').value == '') {
                    hdnProjectCode.value = '';
                }
            });







            ////////Grid Equipment checkbox Check\\\\\


            $("#<%=RadgvEquip.ClientID%> input[id*='chkAll']:checkbox").click(function () {
                //  debugger;
                if ($(this).is(':checked')) {


                    EqCheckBOX(true);

                }
                else {


                    EqCheckBOX(false);
                }

                SelectRowsEq();

            });
            $("#<%=chkCreateMultipleTicket.ClientID%>").click(function () {
                document.getElementById('<%= btnchkCreateMultipleTicket.ClientID%>').click();
            });



            ///////////// Signature box handling  ////////////////////


            $("#signbg").click(function () {
                if (isCanvasSupported()) {
                    //$("#sign").toggle();
                    //$("#sign").focus();

                    var window = $find('<%= RadWindow1Signature.ClientID %>');
                    window.show();
                }
                else {
                    alert('Signature not supported on this web browser.');
                }
            });

            $("#sign").blur(function () {
                //$("#sign").hide();

            });


            $("#convertpngbtn").click(function () {
                // $("#sign").hide();

                toImage();
            });


            $("#eqtag").click(function () {

                $("#DivEqup").show();
            });

            $("#<%=txtJobCode.ClientID%>").click(function () {

                $("#DivprojectType").show();

            });

            $("#<%=txtProject.ClientID%>").click(function toggleProject() {

                $("#DivProject").show();

            });

            var grid = $find("<%=RadgvEquip.ClientID %>");
            var masterTable = grid.get_masterTableView();
            if (masterTable != null) {

                var columns = grid.get_masterTableView().get_columns();
                for (var i = 0; i < columns.length; i++) {
                    columns[i].resizeToFit(false, true);
                }
            }




            $(function () {
                $("#<%= txtGoogleAutoc.ClientID %>").geocomplete({
                    map: false,
                    details: "#divmain",
                    types: ["geocode", "establishment"],
                    address_components: "#<%= txtGoogleAutoc.ClientID %>",
                    city: "#<%= txtCity.ClientID %>",
                    state: "#<%= ddlState.ClientID %>",
                    zip: "#<%= txtZip.ClientID %>",
                    lat: "#<%= lat.ClientID %>",
                    lng: "#<%= lng.ClientID %>"
                }).bind("geocode:result", function (event, result) {

                    //var getCountry = "";
                    //for (var i = 0; i < result.address_components.length; i++) {
                    //    var addr = result.address_components[i];
                    //    var getCountry;
                    //    if (addr.types[0] == 'country')
                    //        getCountry = addr.short_name;
                    //}
                    var countryCode = "", city = "", cityAlt = "",getCountry = "";                   
                    for (var i = 0; i < result.address_components.length; i++) {
                       
                        var addr = result.address_components[i];

                        if (addr.types[0] == 'country')
                            getCountry = addr.short_name;
                        if (addr.types[0] == 'locality')
                            city = addr.long_name;
                         if (addr.types[0] == 'administrative_area_level_1')
                            cityAlt = addr.short_name;
                        if (addr.types[0] == 'postal_code')
                            countryCode = addr.long_name;                      
                    }
                 if (cityAlt.length > 2)
                         for (var i = 0; i < result.address_components.length; i++) {                       
                        var addr = result.address_components[i];                      
                             if (addr.types[0] == 'administrative_area_level_2')
                                 cityAlt = addr.short_name;                                            
                    }
                    $("#<%=txtCountry.ClientID%>").val(getCountry);
                     $("#<%=txtCity.ClientID%>").val(city);
                    $("#<%=txtZip.ClientID%>").val(countryCode);              
                       $("#<%=ddlState.ClientID%>").val(cityAlt);
                    
                     Materialize.updateTextFields();

                    });
            });

            Materialize.updateTextFields();
            $("#<%=ddlStatus.ClientID%>").change(function () {
                document.getElementById("<%=lnkddlStatus.ClientID%>").click();
            });

        }

        function JobChargeableCheck() {
            var chkJobChargeable = document.getElementById('<%=chkJobChargeable.ClientID%>');
            var chkChargeable = document.getElementById('<%=chkChargeable.ClientID%>');
            chkChargeable.checked = chkJobChargeable.checked;

        }

        function ChargeableCheck() {
            var chkChargeable = document.getElementById('<%=chkChargeable.ClientID%>');
            var chkJobChargeable = document.getElementById('<%=chkJobChargeable.ClientID%>');
            chkJobChargeable.checked = chkChargeable.checked;

        }
        $(document).ready(function () {
           // calculate_Time();

        });


    </script>

</asp:Content>



