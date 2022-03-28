<%@ Page Title="Scheduler || MOM" Language="C#" EnableEventValidation="false" AutoEventWireup="true"  Inherits="Scheduler" CodeBehind="Scheduler.aspx.cs" %>
 <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html>

<html>
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1.0, user-scalable=no">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="msapplication-tap-highlight" content="no">
    <title>MOM v5.0</title>

    <link rel="icon" href="Design/images/favicon.png" sizes="32x32">
    <link href="Design/css/materialize.css" type="text/css" rel="stylesheet" media="screen,projection">
    <link href="Design/css/style.css" type="text/css" rel="stylesheet" media="screen,projection">
    <link href="Design/css/custom/custom.css" type="text/css" rel="stylesheet" media="screen,projection">
    <link href="Design/js/plugins/prism/prism.css" type="text/css" rel="stylesheet" media="screen,projection">
    <link href="Design/js/plugins/perfect-scrollbar/perfect-scrollbar.css" type="text/css" rel="stylesheet" media="screen,projection">
    <link href="Design/css/pikaday.css" rel="stylesheet" />
<%--    <link href="https://fonts.googleapis.com/css?family=Lato" rel="stylesheet"> --%>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js"></script>
<%--    <script src="https://use.fontawesome.com/827cb0cf39.js"></script>--%>
 <%--   <script type="text/javascript" src="https://maps.googleapis.com/maps/api/js?sensor=false&amp;libraries=places&key=AIzaSyCndNAw_XYuJaz2SLtNd40zaVw8e2S8N2Q"></script>--%>


    <script src="Design/js/UI/jquery-ui.js"></script>
    <script src="Design/js/materialize-plugins/forms.js"></script>
    <script src="Design/js/Notifyjs/jquery.noty.js"></script>
    <script src="Design/js/Notifyjs/themes/default.js"></script>
    <script src="Design/js/Notifyjs/layouts/topCenter.js"></script>
    <link href="Design/css/grid.css" rel="stylesheet" />

    <%--<link href="css/bootstrap.css" rel="stylesheet" />--%>
    <script src="js/date.format.js"></script>
    <script src="js/tether.min.js"></script>
    <script src="js/bootstrap.min.js"></script>
    <script src="js/bootstrap-notify.js"></script>
    <script src="js/datediff.js"></script>
    <script>

        $(document).ready(function () {

            $('[id*=chkboxSelectAll]').click(function () {
                //debugger;
                $("[id*='chkSelect']").attr('checked', this.checked);
            });
        });
    </script>
 

    <script>
        (function ($) {
            $(window).load(function () {
                $(".content").mCustomScrollbar({
                    theme: "minimal"
                });
            });
        })(jQuery);


    </script>

 


      <%-- rsMonthView--%>
    <style>

        .RadScheduler .rsMonthView .rsWrap,
    .RadScheduler .rsMonthView .rsApt,
    .RadScheduler .rsMonthView .rsAptOut,
    .RadScheduler .rsMonthView .rsAptIn,
    .RadScheduler .rsMonthView .rsAptMid,
    .RadScheduler .rsMonthView .rsAptContent
    {
            position: static !important;
            height: auto !important;
    }
    .RadScheduler .rsMonthView .rsWrap
    {
            overflow: hidden;
            font-size: 0;
            line-height: 0;
    }
    .RadScheduler .rsMonthView .rsLastWrap
    {
            height: 16px !important;
    }
    .RadScheduler .rsMonthView .rsAptContent
    {
            position: relative !important;
            border-top: 0;
            border-bottom: 0;
            left: 0;
            top: 0;
    }
    * html .RadScheduler .rsMonthView .rsAptIn
    {
            border-width: 0 1px;
            top: 0;
        margin-left: -1px;
        margin-right: -1px;
    }
    * html .RadScheduler .rsMonthView .rsAptMid
    {
            top: 0;
            left: 1px;
            margin: 0 1px;
            border-width: 1px 0;
    }
    .rsContentScrollArea{
        height: 98%!important;
    }

    </style>

    <style>
        .RadWindow_Material .rwTitleBar {
            border-top-left-radius: 6px !important;
            border-top-right-radius: 6px !important;
            background-color: #1c5fb1 !important;
            height:40px !important;
        }
        .RadWindow RadWindow_Material
        {
            height:auto !important;
        }

        .todo_notify {
            padding: 0.75rem 1.25rem;
            border: 1px solid transparent;
            border-radius: 0.25rem;
            background-color: #ffffff;
            width: 500px;
            opacity: 0.9;
            font-size: 12px;
            padding-right: 25px;
            padding-left: 50px;
            -webkit-box-shadow: 1px 1px 1px 1px rgba(112,104,112,1);
            -moz-box-shadow: 1px 1px 1px 1px rgba(112,104,112,1);
            box-shadow: 1px 1px 1px 1px rgba(112,104,112,1);
        }
        .todo_notify > button.close{
            padding: 0;
            cursor: pointer;
            background: transparent;
            border: 0;
            -webkit-appearance: none;
            float: right;
            font-size: 1.5rem;
            font-weight: bold;
            line-height: 1;
            color: #000;
            text-shadow: 0 1px 0 #fff;
            opacity: .5;
        }
        .todo_notify > span[data-notify="title"]{
            font-size: 14px;
            font-weight: bold;
        }
        .todo_notify > .notifyIcon {
            display: inline-block;
            background-image: url(images/application_view_list.ico);
            width: 32px;
            height: 32px;
            position: absolute;
            top: 15px;
            left: 10px
        }
        
    </style>

    

    <style type="text/css">
        .spannk {
            font-weight: bolder;
            color: #826200 !important;
        }

        .spannk1 {
            font-weight: 600;
            color: #826200 !important;
            font-size: 13px;
        }

        .spannk2 {
            width: 20px;
            height: 20px;
            margin: 1px;
            padding: 1px;
            float: left;
            display: none;
        }

        .spannk3 {
            width: 20px;
            height: 20px;
            margin: 1px;
            padding: 1px;
            float: left;
            display: block;
        }



        .tcktdiv1 {
            width: 100% !important;
            margin-left: -8px !important;
            padding-left: 10px !important;
            height: 0px !important;
            line-height: 10px !important;
            margin-top: -6px !important;
            margin-bottom: 10px !important;
        }

        .TimeNow1 {
            border-bottom: 1px solid red !important;
        }

        .TimeNow2 {
            border-bottom: 1px solid red !important;
        }

        .RadToolTip_Material {
            background-color: #ffeaa8 !important;
            width: 30% !important;
        }

            .RadToolTip_Material .rtCalloutTopRight, .RadToolTip_Material .rtCalloutTopCenter, .RadToolTip_Material .rtCalloutTopLeft {
                border-top-color: #222 !important;
            }

                .RadToolTip_Material .rtCalloutTopRight:before, .RadToolTip_Material .rtCalloutTopCenter:before, .RadToolTip_Material .rtCalloutTopLeft:before {
                    border-top-color: #222 !important;
                }

        .padding-top-9 {
            padding-top: 9px;
        }

        .min-width-200px {
            min-width: 200px;
        }

        .RadScheduler_Material .rsAptContent {
            background: linear-gradient(to bottom right, #ccc, #fff);
            font-family: 'Lato', sans-serif !important;
            font-size: 12.15px !important;
            border: 1px solid #ccc !important;
            width: 100%;
            border-radius: 8px !important;
            color: black !important;
        }

       

        .imgCategory {
            width: 19px;
            height: 19px;
            margin: -2px;
        }

        .imgCategoryTicket {
            width: 15px;
            float: left;
            font-weight: bold;
            padding-right: 1.5px !important;
            margin: 0.5px !important;
        }

        .TicketID {
            padding-left: 0 !important;
        }

        .MiddlePane1 .rspSlideContent {
            max-height: 130px;
        }

        .MiddlePane1 .rspSlidePane {
            max-height: 168px;
        }

        .MiddlePane1 #RAD_SPLITTER_PANE_CONTENT_ctl00_ContentPlaceHolder1_Radpane1 {
            max-height: 200px;
        }

        .LeftPane .rspSlideContent {
            min-width: 350px;
        }

        .LeftPane .rspSlideHeader .rspSlideTitle, .MiddlePane1 .rspSlideHeader .rspSlideTitle {
            height: 36px !important;
            margin-top: -5px !important;
        }

        .LeftPane .rspSlideHeader > td > table, .MiddlePane1 .rspSlideHeader > td > table {
            max-height: 36px;
        }

        .hideDesc_container {
            margin-bottom: 10px !important;
        }
    </style>

    <telerik:RadScriptBlock ID="RadScriptBlock1" runat="server">
        <script type="text/javascript">

            $(function () {
                $(".MiddlePane1 .rspSlideHeaderUndockIcon").click(function () {
                    $(".MiddlePane1 .rspTabsContainer").show();
                });

                $(".MiddlePane1 .rspPaneTabContainer").click(function () {
                    $(".MiddlePane1 .rspTabsContainer").hide();
                });

                var warningTimer;

                warningTimer = setTimeout(function () {
                    //console.log('timeoutstart');
                   RefreshSchedular();
                }, 60000);

                $(document).on('mousemove', '.container', function () {
                    //console.log('timeoutclar');
                    ResetTimers();
                });

                function StartTimers() {
                    warningTimer = setTimeout(function () {
                        //console.log('timeoutstart');
                        RefreshSchedular();
                    }, 60000);
                }

                function ResetTimers() {
                    clearTimeout(warningTimer);
                    StartTimers();
                }

            });

            function AddTicketClick(hyperlink) {
                var id = document.getElementById('<%= hdnAddeTicket.ClientID%>').value;
                if (id == "Y") {
                    return true;
                } else {
                    noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                    return false;
                }
            }

            window.OnClientAppointmentContextMenu = function (sender, args) {
                hideActiveToolTip();
            }

            window.hideActiveToolTip = function () {      
                try {
                    var tooltip = Telerik.Web.UI.RadToolTip.getCurrent();
                    if (tooltip) {
                        tooltip.hide();
                    }
                }
                catch{ }
            }

            window.rowDropping = function (sender, eventArgs) {
                //debugger;
                try {
                    if ($("#<%=hdnAddeTicket.ClientID%>").val() == 'N') {
                        noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 1000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                        args.set_cancel(true);

                    }
                    else {

                        hideActiveToolTip();

                        var scheduler = $find('<%= RadScheduler1.ClientID %>');
                        // Fired when the user drops a grid row
                        var htmlElement = eventArgs.get_destinationHtmlElement();

                        if (isPartOfSchedulerAppointmentArea(htmlElement)) {

                            // The row was dropped over the scheduler appointment area
                            // Find the exact time slot and save its unique index in the hidden field
                            var timeSlot;
                            if ($telerik.$(htmlElement).parents(".rsApt").length != 0) {
                                timeSlot = scheduler.getAppointmentFromDomElement(htmlElement).get_timeSlot();

                            }
                            else
                                timeSlot = scheduler._activeModel.getTimeSlotFromDomElement(htmlElement);
                            var Resource = timeSlot.get_resource();
                            var rsname = Resource._text;
                            var startTime1 = new Date(timeSlot._startTime);
                            // alert(Resource._id);
                            if (rsname != '') {
                                if (confirm('Do you really want to assign Ticket to ' + rsname + ' ?')) {
                                    $('#<%= TargetSlotHiddenField.ClientID %>').val(timeSlot.get_index());
                                    $('#<%= hdnAssignworker.ClientID %>').val(rsname);
                                    $('#<%= hdnAssigndate.ClientID %>').val(startTime1.toString());
                                    console.log(startTime1.toString());

                                    //window.location.href=window.location.href;
                                }
                                else {
                                    // The node was dropped elsewhere on the document
                                    eventArgs.set_cancel(true);
                                }
                            }

                            // The HTML needs to be set in order for the postback to execute normally -----
                            //eventArgs.set_destinationHtmlElement(TargetSlotHiddenField);

                        }
                        else {
                            // The node was dropped elsewhere on the document
                            eventArgs.set_cancel(true);
                        }
                    }
                }
                catch (e) { console.log(e); args.set_cancel(true);}
            }

            function isPartOfSchedulerAppointmentArea(htmlElement) {
                // Determines if an HTML element is part of the scheduler appointment area
                // This can be either the rsContent or the rsAllDay div (in day and week view)
                return $telerik.$(htmlElement).parents().is("div.rsAllDay") ||
                    $telerik.$(htmlElement).parents().is("div.rsContent")
            }



            function RefreshSchedular() {
          
               try {
                  <%-- if ($("#<%=hdnCheckActionScheduler.ClientID%>").val() == '1') {--%>
                       var btn = document.getElementById('<%=btnSearch.ClientID%>');
                     btn.click();
               <%--    } else {

                       $("#<%=hdnCheckActionScheduler.ClientID%>").val('1');
                   }--%>
                    
               } catch (e) { console.log(e) };
            }

    
            //Get date format for New Time slot.
            function getFormattedString(d) {                
                return (d.getMonth() + 1) + "/" + d.getDate() + "/" + d.getFullYear() + ' ' + d.toString().split(' ')[4];
            }
  

            function OnClientAppointmentMoveStart(sender, args) {
                //debugger;
                console.log(args.get_appointment().get_start());
                try {
                    console.log('OnClientAppointmentMoveStart');
                    hideActiveToolTip();
                } catch (e) {
                    args.set_cancel(true);
                    console.log('OnClientAppointmentMovingError');
                }
            }

            function OnClientAppointmentMoving(sender, args) {
                //debugger;
                try {
                  //  console.log('OnClientAppointmentMoveing');
                    
                } catch (e) {
                    args.set_cancel(true);
                    console.log('OnClientAppointmentMovingerror');
                }
            }
            function OnClientAppointmentClick(sender, eventArgs) {
                var apt = eventArgs.get_appointment();
                console.log(("You clicked on an appointment with the subject: " + apt.get_subject()));
            }

            function OnClientAppointmentMoveEnd(sender, args) { 
                //debugger;
                console.log(args.get_appointment().get_end());

                var scheduler = null;               
                // Cache the Scheduler object reference so we don't have to retrieve it every time
                scheduler = sender;
                try {
                    hideActiveToolTip();
                    if ($("#<%=hdnEditeTicket.ClientID%>").val() == 'N') {
                        noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 1000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                        args.set_cancel(true);
                    }
                    else {
                        var appt = args.get_appointment();
                        var list = appt.get_attributes();
                        var attrAssigned = list.getAttribute('assigned');
                        var _NewStartDate = args.get_newStartTime();
                        var todaysDate = getFormattedString(_NewStartDate);
                        var NewTimeSlotIndex = args.get_targetSlot().get_index();
                        var TicketId = appt._id;
                        var resource = args.get_targetSlot().get_resource();
                        var workername = resource.get_text();                       

                        if (appt != null && list != null && attrAssigned != null && NewTimeSlotIndex != null) {
                            if (attrAssigned == "4") {
                                noty({ text: 'It is not allowed to move the completed ticket.', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 500, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                                args.set_cancel(true);
                            }
                            else if (attrAssigned != "4") {
                                if (confirm('Do you really want to move ' + appt._subject + ' ?')) {


                                    //call Ajax request
                                    $.ajax({
                                        type: "POST",
                                        contentType: "application/json; charset=utf-8",
                                        url: "WS_Scheduler.asmx/AppointmentMove",
                                        data: '{ "TicketId" : "' + TicketId + '", "TimeSlotIndex": "' + NewTimeSlotIndex + '", "StartTime": "' + todaysDate + '", "workername" : "' + workername + '" }',
                                        dataType: "json",
                                        async: false,
                                        success: function (data) {
                                            console.log(data.d);
                                            args.set_cancel(false);
                                        },
                                        error: function (result) {
                                            alert(result.responseText);
                                            args.set_cancel(true);
                                        },
                                        failure: function (result) {
                                            alert(result.responseText);
                                            args.set_cancel(true);
                                        }
                                    });



                                                                                              
                                }                              
                                else {
                                    args.set_cancel(true);
                                }
                            }
                        } else { args.set_cancel(true); }
                    }
                } catch (e) { console.log(e); args.set_cancel(true);}

            }

            function overlapsWithAnotherAppointment(appointment, startTime, endTime) {
                // Get all appointments in the given time range
                var appointments = scheduler.get_appointments().getAppointmentsInRange(startTime, endTime);

                // If there are no appointments in the range, there is no overlapping
                if (appointments.get_count() == 0)
                    return false;

                // If we are checking a specific appointment and it's the only one in the range, there is no overlapping
                if (appointment && appointments.get_count() == 1 && appointments.getAppointment(0) == appointment)
                    return false;

                return true;
            }


            function OnClientAppointmentResizeStart(sender, args) {

              
                try {
                    hideActiveToolTip();

                    if ($("#<%=hdnEditeTicket.ClientID%>").val() == 'N') {
                        noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 1000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                        args.set_cancel(true);

                    }
                    else {
                        var appt = args.get_appointment();
                        var list = appt.get_attributes();
                        var attrAssigned = list.getAttribute('assigned');
                        if (attrAssigned == "4") {
                            noty({ text: 'It is not allowed to resize the completed ticket.', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 1000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                            args.set_cancel(true);
                        }
                    }
                }
                catch (e) { console.log(e); args.set_cancel(true); }
            }

            function OnClientAppointmentResizeEnd(sender, args) {

                
                //debugger;
                var scheduler = null;
                // Cache the Scheduler object reference so we don't have to retrieve it every time
                scheduler = sender;
                try {
                    hideActiveToolTip();

                    if ($("#<%=hdnEditeTicket.ClientID%>").val() === 'Y') {
                        var appt = args.get_appointment();
                        var list = appt.get_attributes();
                        var attrAssigned = list.getAttribute('assigned');
                        var _NewStartDate = args.get_newStartTime();
                        var _NewEndDate = args.get_newEndTime();
                        var NewTimeSlotIndex = args.get_targetSlot().get_index();

                        if (appt !== null && list !== null && attrAssigned != null && NewTimeSlotIndex !== null && _NewStartDate !== null) {
                            if (attrAssigned !== "4") {

                                if (confirm('Do you really want to resize ' + appt._subject + ' ?')) {

                                   
                                    console.log('ApptID:-' + appt._id + 'NewTimeSlotIndex' + NewTimeSlotIndex + 'attrAssigned' + attrAssigned);

                                    var TicketId = appt._id;


                                    //call Ajax request
                                    $.ajax({
                                        type: "POST",
                                        contentType: "application/json; charset=utf-8",
                                        url: "WS_Scheduler.asmx/AppointmentResize",
                                        data: '{ "TicketId" : "' + TicketId + '", "ResizeNewStartDate": "' + _NewStartDate + '", "ResizeNewEndDate": "' + _NewEndDate + '"  }',
                                        dataType: "json",
                                        async: false,
                                        success: function (data) {
                                            console.log(data.d);
                                        },
                                        error: function (result) {
                                            alert(result.responseText);
                                        },
                                        failure: function (result) {
                                            alert(result.responseText);
                                        }
                                    });


                                }
                            } else { args.set_cancel(true); }
                        } else { args.set_cancel(true); }
                    }
                } catch (e) { console.log(e); args.set_cancel(true);}
            }

            function ShowHideToolTip(sender, eventArgs) {
                try {                   
                    var isChecked = $("#chkHideTicketDesc").is(":checked");
                    var control = $(".RadScheduler1 .rsApt");
                    if (isChecked) {
                        eventArgs.set_cancel(true);
                        control.removeAttr("title");
                    }
                    else {


                        //copy the content first to allow the browser to cache the image
                        sender.set_content($get("ttipContent").innerHTML);
                        var dataKeys = sender.get_value().split("NK|");
                        var ttipParent = sender.get_popupElement();
                        var imgstatus = dataKeys[0];

                        $get("TicketID", ttipParent).innerHTML = dataKeys[1];
                        $get("Location", ttipParent).innerHTML = dataKeys[2];
                        $get("Customer", ttipParent).innerHTML = dataKeys[3];
                        $get("Category", ttipParent).innerHTML = dataKeys[4];
                        $get("address", ttipParent).innerHTML = dataKeys[5];
                        $get("City", ttipParent).innerHTML = dataKeys[6];
                        //$get("phone", ttipParent).innerHTML = dataKeys[7];
                        $get("edate", ttipParent).innerHTML = dataKeys[8];
                        $get("fdesc", ttipParent).innerHTML = dataKeys[9];
                        $get("descres", ttipParent).innerHTML = dataKeys[10];
                        //$get("WorkOrder", ttipParent).innerHTML = dataKeys[11];
                        $get("assigned", ttipParent).innerHTML = dataKeys[12];
                       



                        var Isdoc = imgstatus.charAt(0);
                        var Isalert = imgstatus.charAt(1);
                        var Iscredithold = imgstatus.charAt(2);
                        var isMS = imgstatus.charAt(3);
                        var isSignature = imgstatus.charAt(4);
                        var isConfirmed = imgstatus.charAt(5);
                        var isdollarRed = imgstatus.charAt(6);
                        var isdollarblue = imgstatus.charAt(7);
                        var isdollar = imgstatus.charAt(8);
                        var Isreview = imgstatus.charAt(9);
                        var isRecommendation = imgstatus.charAt(10);
                        

                        if (Isdoc == '1') { $get("imgIsdoc", ttipParent).setAttribute("class", "spannk3"); }
                        if (Isalert == '1') { $get("imgIsalert", ttipParent).setAttribute("class", "spannk3"); }
                        if (Iscredithold == '1') { $get("imgIscredithold", ttipParent).setAttribute("class", "spannk3"); }
                        if (isMS == '1') { $get("imgisMS", ttipParent).setAttribute("class", "spannk3"); }
                        if (isSignature == '1') { $get("imgisSignature", ttipParent).setAttribute("class", "spannk3"); }
                        if (isConfirmed == '1') { $get("imgisConfirmed", ttipParent).setAttribute("class", "spannk3"); }
                        if (isdollarRed == '1') { $get("ImgisdollarRed", ttipParent).setAttribute("class", "spannk3"); }
                        if (isdollarblue == '1') { $get("Imgisdollarblue", ttipParent).setAttribute("class", "spannk3"); }
                        if (isdollar == '1') { $get("Imgisdollar", ttipParent).setAttribute("class", "spannk3"); }
                        if (Isreview == '1') { $get("imgIsreview", ttipParent).setAttribute("class", "spannk3"); }
                        if (isRecommendation != '0') { $get("ImgRecommendation", ttipParent).setAttribute("class", "spannk3"); }

                        

                        eventArgs.set_cancel(false);
                    }
                } catch (e) { eventArgs.set_cancel(true); };
            }

            function OnClientAppointmentContextMenuItemClicked(sender, eventArgs) {
                try {
                    var itemValue = eventArgs.get_item().get_value();
                    var appt = eventArgs.get_appointment();
                    var list = appt.get_attributes();
                    var assigned = list.getAttribute('assigned');
                    var TicketID = appt._id;
                    //alert(TicketID);
                    //alert(assigned);
                    if (assigned == "4") {
                        if (itemValue == "Copy") { window.open('addticket.aspx?copy=1&id=' + TicketID + '&comp=0'); }
                        else if (itemValue == "Edit") { window.open('addticket.aspx?id=' + TicketID + '&comp=1#accrdcompleted'); }
                        else {
                            noty({ text: 'It is not allowed to change of completed ticket.', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                        }
                    }
                    else if (itemValue == "Delete") {
                        if ($("#<%=hdnDeleteTicket.ClientID%>").val() == "N") {
                            noty({ text: 'You do not have delete permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                        }
                        else
                            if (confirm('Do you really want to delete Ticket#' + TicketID + ' ?')) {
                                var btn = document.getElementById('<%=TicketDelete.ClientID%>');
                                $("#<%=hdnDeleteTicketId.ClientID%>").val(TicketID);
                                btn.click();
                            }
                    }
                    else if (itemValue == "Void") {
                        if ($("#<%=hdnTicketVoidPermission.ClientID%>").val() == "N") {
                            noty({ text: 'You do not have Void permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                        }
                        else
                            if (confirm('Are you sure you want to Void Ticket#' + TicketID + ' ?')) {
                                var btnVoid = document.getElementById('<%=btnTicketVoid.ClientID%>');
                                $("#<%=hdnTicketVoidID.ClientID%>").val(TicketID);
                                btnVoid.click();
                            }
                    }
                    else if ($("#<%=hdnEditeTicket.ClientID%>").val() == "N") {
                        noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                    }
                    else if (itemValue == "Unassigned") { window.open('addticket.aspx?id=' + TicketID + '&st=0'); }
                    else if (itemValue == "Assigned") { window.open('addticket.aspx?id=' + TicketID + '&st=1'); }
                    else if (itemValue == "Enroute") { window.open('addticket.aspx?id=' + TicketID + '&st=2'); }
                    else if (itemValue == "Onsite") { window.open('addticket.aspx?id=' + TicketID + '&st=3'); }
                    else if (itemValue == "Completed") { window.open('addticket.aspx?id=' + TicketID + '&st=4'); }
                    else if (itemValue == "Hold") { window.open('addticket.aspx?id=' + TicketID + '&st=5'); }                        
                    else if (itemValue == "Edit") { window.open('addticket.aspx?id=' + TicketID + '&comp=0'); }
                    else if (itemValue == "Copy") { window.open('addticket.aspx?copy=1&id=' + TicketID + '&comp=0'); }

                    else if (itemValue == 'SendMessage') {
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
                                        window.open('mailticket.aspx?id=' + TicketID + '&c=' + assigned);
                                    }
                                },
                                {
                                    type: 'btn-danger', text: 'No', click: function ($noty) {
                                        $noty.close();
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
                    else {
                        eventArgs.set_cancel(true);
                    }
                } catch (e) { eventArgs.set_cancel(true); };
            }
        </script>
    </telerik:RadScriptBlock>

</head>
<body>
    <form id="form1" runat="server" autocomplete="off">
         <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" ScriptMode="Release"
            AsyncPostBackTimeout="360000">
        </asp:ToolkitScriptManager>  
        
            <div class="wrapper">   
        
 
    <%--  <asp:LinkButton ID="lnkTicketDragDrop" runat="server" Text="" OnClick="lnkTicketDragDrop_Click" />--%>

    <script>

        function myFunction() {
            var x = document.getElementById("stats");
            if (x.style.display === "none") {
                x.style.display = "block";
            } else {
                x.style.display = "none";
            }
        }
    </script>


    <div>
        <div>
            <div id="divButtons">
                <div id="breadcrumbs-wrapper">
                    <header>
                        <div class="container row-color-grey">
                            <div class="row">
                                <div class="col s12 m12 l12">
                                    <div class="row">
                                        <div class="page-title"><i class="mdi-calender"></i>&nbsp;Scheduler</div>
                                        <div class="buttonContainer">
                                            <div class="btnlinks">
                                                <asp:HyperLink ID="lnkAddticket" TabIndex="23" runat="server" OnClick='return AddTicketClick(this)' NavigateUrl="addticket.aspx" ToolTip="Add New Ticket" CssClass="icon-addnew" Target="_blank">Add New Ticket</asp:HyperLink>
                                            </div>

                                            <div class="btnlinks">
                                                 <a onclick="myFunction()"><i class="mdi-action-search"></i></a>

                                            </div>
                                            <div class="btnlinks"> 
                                                <asp:CheckBox ID="chkHideTicketDesc" Text="  Hide Ticket Description" AutoPostBack="true" OnCheckedChanged="chkHideTicketDesc_CheckedChanged" runat="server" />
                                                <%--<input id="chkHideTicketDesc" checked="checked"  type="checkbox" runat="server" />
                                                <label for="chkHideTicketDesc">Hide Ticket Description</label>--%>
                                               
                                            </div>
                                        </div>
                                        
                                     <asp:Label  id="loaderlbl" runat="server"> </asp:Label>
                                    </div>
                                </div>

                            </div>
                        </div>
                    </header>

                </div>
                <div class="container breadcrumbs-bg-custom">
                  
                    <div id="stats" style="background-color: #fff !important; display: none;"> 

                            <div class="srchpane-advanced">

                                <div class="srchpaneinner">
                                    <div class="srchinputwrap">

                                        <label class="drpdwn-label">Supervisor</label>
                                        <asp:DropDownList ID="ddlSuper" runat="server" Width="180px" AutoPostBack="true" CssClass="browser-default" OnSelectedIndexChanged="ddlSuper_SelectedIndexChanged"></asp:DropDownList>

                                    </div>

                                    <div class="srchinputwrap">

                                        <label class="drpdwn-label">Status</label>
                                        <asp:DropDownList CssClass="browser-default" Width="180px" ID="ddlStatus" runat="server">
                                            <asp:ListItem Text=":: All ::" Value="-1" />
                                            <asp:ListItem Text="Assigned" Value="1" />
                                            <asp:ListItem Text="Enroute" Value="2" />
                                            <asp:ListItem Text="Onsite" Value="3" />
                                            <asp:ListItem Text="Completed" Value="4" />
                                            <asp:ListItem Text="Hold" Value="5" />
                                             <asp:ListItem Text="Voided" Value="6" />
                                        </asp:DropDownList>


                                    </div>

                                    <div class="srchinputwrap">
                                        <label class="drpdwn-label">Worker</label>
                                        <asp:DropDownList CssClass="browser-default"  Width="180px" ID="ddlworker" AutoPostBack="true" OnSelectedIndexChanged="ddlworker_SelectedIndexChanged" runat="server">
                                        </asp:DropDownList>
                                    </div>

                                    <div class="srchinputwrap">
                                        <label class="drpdwn-label">Department</label>
                                        <asp:DropDownList CssClass="browser-default" Width="180px" ID="ddlDepartment" runat="server">
                                        </asp:DropDownList>
                                    </div>

                                    <div class="srchinputwrap">
                                        <label class="drpdwn-label">Category</label>
                                        <asp:DropDownList CssClass="browser-default" Width="180px" ID="ddlCategory" runat="server">
                                        </asp:DropDownList>
                                    </div>

                                    <div class="srchinputwrap">
                                        <label class="drpdwn-label">Location</label>
                                        <asp:TextBox ID="txtSearch" runat="server" placeholder="Search..."></asp:TextBox>

                                    </div> 

                                    <div class="srchinputwrap">


                                        <div class="btnlinks" style="float: right;">
                                            <asp:LinkButton ID="btnSearch" runat="server" CausesValidation="false"
                                                OnClick="btnSearch_Click"><i class="mdi-notification-sync"></i></asp:LinkButton>
                                        </div>
                                    </div>
                                     
                            </div> 

                            </div> 

                        </div> 
                </div>
            </div>
        </div>
    </div>

 

    <div class="container">
        <div class="row">

            <telerik:RadStyleSheetManager ID="radStyleSheerManager" runat="server" CdnSettings-TelerikCdn="Enabled">
            </telerik:RadStyleSheetManager>
            <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
                <AjaxSettings>
                    <telerik:AjaxSetting AjaxControlID="RadScheduler1">
                        <UpdatedControls>
                            <telerik:AjaxUpdatedControl ControlID="RadScheduler1" LoadingPanelID="RadAjaxLoadingPanelRadScheduler1"></telerik:AjaxUpdatedControl>
                        </UpdatedControls>
                    </telerik:AjaxSetting>

                    <telerik:AjaxSetting AjaxControlID="RadGrid1">
                        <UpdatedControls>
                            <telerik:AjaxUpdatedControl ControlID="RadScheduler1" LoadingPanelID="RadAjaxLoadingPanelRadScheduler1" ></telerik:AjaxUpdatedControl>
                        </UpdatedControls>
                    </telerik:AjaxSetting>

                    <telerik:AjaxSetting AjaxControlID="RadScheduler1">
                        <UpdatedControls>
                            <telerik:AjaxUpdatedControl ControlID="HiddenWrapper"></telerik:AjaxUpdatedControl>
                            <telerik:AjaxUpdatedControl ControlID="RadToolTipManager1"></telerik:AjaxUpdatedControl>
                            <telerik:AjaxUpdatedControl ControlID="hdnCheckActionScheduler"></telerik:AjaxUpdatedControl>
                           <%-- <telerik:AjaxUpdatedControl ControlID="RadScheduler1" ></telerik:AjaxUpdatedControl>--%>
                            <telerik:AjaxUpdatedControl ControlID="hdnMoveTicketId"></telerik:AjaxUpdatedControl>
                            <telerik:AjaxUpdatedControl ControlID="hdnMoveNewTimeSlotIndex"></telerik:AjaxUpdatedControl>
                            <telerik:AjaxUpdatedControl ControlID="loaderlbl"  LoadingPanelID="RadAjaxLoadingPanelRadScheduler1"></telerik:AjaxUpdatedControl>
                            
                        </UpdatedControls>
                    </telerik:AjaxSetting>

                    <telerik:AjaxSetting AjaxControlID="btnSearchUnassignedTicket">
                        <UpdatedControls>
                            <telerik:AjaxUpdatedControl ControlID="RadSplitter1" ></telerik:AjaxUpdatedControl>
                                   
                        </UpdatedControls>
                    </telerik:AjaxSetting>

                    <telerik:AjaxSetting AjaxControlID="btnSearch">   
                        <UpdatedControls> 
                            <telerik:AjaxUpdatedControl ControlID="RadScheduler1" ></telerik:AjaxUpdatedControl>  
                            <telerik:AjaxUpdatedControl ControlID="RadGrid1" ></telerik:AjaxUpdatedControl> 
                        </UpdatedControls>
                    </telerik:AjaxSetting>

                    <telerik:AjaxSetting AjaxControlID="ddlSuper">   
                        <UpdatedControls>
                             <telerik:AjaxUpdatedControl ControlID="ddlworker"></telerik:AjaxUpdatedControl>
                            <telerik:AjaxUpdatedControl ControlID="RadScheduler1"   ></telerik:AjaxUpdatedControl>
                            <telerik:AjaxUpdatedControl ControlID="RadGrid1"></telerik:AjaxUpdatedControl>
                            <telerik:AjaxUpdatedControl ControlID="ddlworker"  />
                            <telerik:AjaxUpdatedControl ControlID="txtSearch"   />
                            <telerik:AjaxUpdatedControl ControlID="RadToolTipManager1"></telerik:AjaxUpdatedControl>
                        </UpdatedControls>
                    </telerik:AjaxSetting>

                    <telerik:AjaxSetting AjaxControlID="ddlworker">   
                        <UpdatedControls>
                            <telerik:AjaxUpdatedControl ControlID="RadScheduler1"   ></telerik:AjaxUpdatedControl>
                            <telerik:AjaxUpdatedControl ControlID="RadGrid1"></telerik:AjaxUpdatedControl> 
                            <telerik:AjaxUpdatedControl ControlID="txtSearch"  />
                            <telerik:AjaxUpdatedControl ControlID="RadToolTipManager1"></telerik:AjaxUpdatedControl>
                        </UpdatedControls>
                    </telerik:AjaxSetting>

                    <telerik:AjaxSetting AjaxControlID="TicketDelete">
                        <UpdatedControls>
                            <telerik:AjaxUpdatedControl ControlID="RadScheduler1"></telerik:AjaxUpdatedControl>
                            <telerik:AjaxUpdatedControl ControlID="RadToolTipManager1"></telerik:AjaxUpdatedControl>
                        </UpdatedControls>
                    </telerik:AjaxSetting>

                      <telerik:AjaxSetting AjaxControlID="btnTicketVoid">
                        <UpdatedControls>
                            <telerik:AjaxUpdatedControl ControlID="RadScheduler1"></telerik:AjaxUpdatedControl>
                            <telerik:AjaxUpdatedControl ControlID="RadToolTipManager1"></telerik:AjaxUpdatedControl>
                        </UpdatedControls>
                    </telerik:AjaxSetting>
                <%--    
                    <telerik:AjaxSetting AjaxControlID="btnAppointmentMove">
                        <UpdatedControls>
                            <telerik:AjaxUpdatedControl ControlID="hdnMoveTicketId"></telerik:AjaxUpdatedControl>
                            <telerik:AjaxUpdatedControl ControlID="hdnMoveNewTimeSlotIndex"></telerik:AjaxUpdatedControl>
                            <telerik:AjaxUpdatedControl ControlID="RadToolTipManager1"></telerik:AjaxUpdatedControl>
                          
                        </UpdatedControls>
                    </telerik:AjaxSetting>--%>

               <%--     <telerik:AjaxSetting AjaxControlID="btnAppointmentResize">
                        <UpdatedControls>

                            <telerik:AjaxUpdatedControl ControlID="hdnResizeTicketId"></telerik:AjaxUpdatedControl>
                            <telerik:AjaxUpdatedControl ControlID="hdnResizeNewStartDate"></telerik:AjaxUpdatedControl>
                            <telerik:AjaxUpdatedControl ControlID="hdnResizeNewEndDate"></telerik:AjaxUpdatedControl>
                            <telerik:AjaxUpdatedControl ControlID="hdnResizeNewTimeSlotIndex"></telerik:AjaxUpdatedControl>
                            <telerik:AjaxUpdatedControl ControlID="RadToolTipManager1"></telerik:AjaxUpdatedControl>
                        </UpdatedControls>
                    </telerik:AjaxSetting>--%>
                </AjaxSettings>
            </telerik:RadAjaxManager>
           
            <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanelRadScheduler1" runat="server" Skin="Material" Height="100px" IsSticky="false" Width="100px">
            </telerik:RadAjaxLoadingPanel>




            <telerik:RadSplitter RenderMode="Auto" ID="RadSplitter1"  CssClass="RadSplitter1" Skin="Material" runat="server" Width="100%" Height="640px">
                <telerik:RadPane ID="LeftPane" CssClass="LeftPane" runat="server" Width="22px" Scrolling="none">
                    <telerik:RadSlidingZone ID="SlidingZone1" runat="server" Width="22px" ClickToOpen="true">

                        <telerik:RadSlidingPane ID="RadSlidingPane1" Title="Unassigned Calls" runat="server" Width="350px" DockOnOpen="true"
                            MinWidth="350">
                            <div class="form-section3 padding-top-9" style="width: 100%; max-width: 350px;">
                                <div class="input-field col s11">
                                    <div class="row">
                                        <telerik:RadTextBox ID="txtSearchUnassignedTicket" Width="90%" ViewStateMode="Enabled" runat="server" CssClass="srchcstm" placeholder="Search ...">
                                        </telerik:RadTextBox>
                                    </div>
                                </div>
                                <div class="input-field col s1" style="margin-left: -25px;">
                                    <div class="row">
                                        <div class="btnlinksicon">
                                            <asp:LinkButton ID="btnSearchUnassignedTicket" runat="server" CausesValidation="false"
                                                OnClick="btnSearchUnassignedTicket_Click"><i class="mdi-notification-sync"></i></asp:LinkButton>
                                        </div>
                                    </div>

                                </div>
                            </div>

                            <div class="demo-container no-bg" style="height: 99%; width: 350px;">
                                <telerik:RadAjaxPanel ID="rapUnassignedCalls" runat="server">
                                    <telerik:RadGrid  Width="250px" Font-Size="8px" RenderMode="Auto" runat="server" ID="RadGrid1" 
                                        GridLines="None" CssClass="radOpenCalls" AutoGenerateColumns="False" 
                                        OnRowDrop="RadGrid1_RowDrop" 
                                        OnNeedDataSource="RadGrid1_NeedDataSource"
                                        EnableLinqExpressions="false"
                                        Style="border: none; outline: 0" Skin="Material" 
                                        ShowFooter="true" ShowHeader="false" PagerStyle-AlwaysVisible="true" AllowCustomPaging="true"
                                        AllowSorting="false" AllowFilteringByColumn="false"
                                        AllowPaging="true" PageSize="10">
                                        <ClientSettings AllowRowsDragDrop="True">
                                            <Selecting AllowRowSelect="True"></Selecting>
                                            <ClientEvents OnRowDropping="rowDropping"></ClientEvents>
                                        </ClientSettings>
                                        <GroupingSettings CaseSensitive="false" />
                                        <MasterTableView DataKeyNames="id, ldesc1" FilterExpression="" AllowPaging="true">
                                            <Columns >
                                                <telerik:GridTemplateColumn >
                                                    <ItemTemplate >
                                                        <div class="table" style="cursor: move;">
                                                            <b>TicketID# :</b>  <a target="_blank" style="cursor: pointer!important; text-decoration: underline;" href='addticket.aspx?id=<%# Eval("id") %>&comp=0'><%# Eval("id").ToString() %></a>
                                                            <br />
                                                            <b>Location :</b> <%# Eval("ldesc1") %>
                                                            <br />
                                                            <b>Address :</b> <span style="text-justify: auto"><%# Eval("address") %> </span>
                                                            <br />
                                                            <b>Category :</b><span class="spCategory"><%# Eval("cat") %> </span>
                                                            <asp:HiddenField ID="hdCategory" runat="server" Value='<%# Eval("cat") %>' />
                                                            <img class="imgCategory" src='imagehandler.ashx?catid=<%# Eval("cat") %>' />
                                                            <br />
                                                            <b>Call Date :</b> <%# Eval("cdate","{0:MM/dd/yyyy}") %>
                                                            <br />
                                                            <b>City :</b> <%# Eval("city") %>
                                                            <br />
                                                        </div>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                            </Columns>
                                        </MasterTableView>
                                    </telerik:RadGrid>
                                </telerik:RadAjaxPanel>

                            </div>


                        </telerik:RadSlidingPane>

                    </telerik:RadSlidingZone>
                </telerik:RadPane>
                <telerik:RadPane ID="MiddlePane1" CssClass="MiddlePane1" runat="server" Scrolling="None" Skin="Material"  >
                    <telerik:RadSplitter RenderMode="Auto" ID="Radsplitter2" runat="server" Orientation="Horizontal" VisibleDuringInit="true" Skin="Material">
                     
                        <telerik:RadPane ID="Radpane2" runat="server" Scrolling="None" Height="100%" Width="100%">



                            <telerik:RadAjaxPanel runat="server" ID="RadAjaxPanel1" >



                                <telerik:RadToolTipManager RenderMode="Auto" ID="RadToolTipManager1" OnClientBeforeShow="ShowHideToolTip" HideEvent="Default"
                                    runat="server" Animation="None" RelativeTo="Element" Position="TopRight"
                                    OffsetX="8" ShowDelay="10">
                                </telerik:RadToolTipManager>

                                <script type="text/javascript">

</script>
                                <div style="display: none; height: auto;">
                                    <div id="ttipContent" class="ttContent">
                                        <div style="padding: 2px!important">
                                            <img id="imgIsreview" class="spannk2" alt="Product Image" src="images/review.png" />
                                            <img id="imgIsdoc" class="spannk2" alt="Product Image" src="images/Document.png" />
                                            <img id="imgIsalert" class="spannk2" alt="Product Image" src="images/alert.png" />
                                            <img id="imgIscredithold" class="spannk2" alt="Product Image" src="images/MSCreditHold.png" />
                                            <img id="imgisMS" class="spannk2" alt="Product Image" src="images/1331034893_pda.png" />
                                            <img id="imgisSignature" class="spannk2" alt="Product Image" src="images/Signature.png" />
                                            <img id="imgisConfirmed" class="spannk2" alt="Product Image" src="images/1331036429_Check.png" />
                                            <img id="ImgisdollarRed" class="spannk2" alt="Product Image" src="images/dollarRed.png" />
                                            <img id="Imgisdollarblue" class="spannk2" alt="Product Image" src="images/dollarblue.png" />
                                            <img id="Imgisdollar" class="spannk2" alt="Product Image" src="images/dollar.png" />
                                             <img id="ImgRecommendation" class="spannk2" alt="Product Image" src="images/thumb_up.png" />
                                        </div>
                                        <br />
                                        <span class="spannk">Ticket# </span><span class="spannk1" id="TicketID"></span>

                                        <span class="spannk">Status: </span><span class="spannk1" id="assigned"></span>

                                        <span class="spannk">City: </span><span class="spannk1" id="City"></span>
                                        <br />
                                        <span class="spannk">Category: </span><span class="spannk1" id="Category"></span>

                                        <span class="spannk">Date: </span><span class="spannk1" id="edate"></span>
                                        <br />
                                        <span class="spannk">Customer: </span><span class="spannk1" id="Customer"></span>
                                        <br />
                                        <span class="spannk">Location: </span><span class="spannk1" id="Location"></span>
                                        <br />
                                        <span class="spannk">Address: </span><span class="spannk1" id="address"></span>

                                        <br />
                                        <span class="spannk">Reason for service: </span><span class="spannk1" id="fdesc"></span>
                                        <br />
                                        <span class="spannk">Work Description: </span><span class="spannk1" id="descres"></span>


                                    </div>
                                </div>


                                <div>


                                    <asp:Panel ID="HiddenWrapper" runat="server" Visible="false">

                                        <telerik:RadScriptBlock ID="RadScriptBlockNK" runat="server">
                                            <script type="text/javascript">

                                                Telerik.Web.UI.RadScheduler.prototype._compensateScrollOffset = function (contentTable) {
                                                    //In Chrome and Safari document.documentElement.scrollTop is always 0, due to a browser bug
                                                    //In Internet Explorer there is no "document.scrollingElement"
                                                    var scrollingElement;
                                                    if ($telerik.isSafari || $telerik.isChrome) {
                                                        scrollingElement = document.body.scrollTop <= document.scrollingElement.scrollTop ? document.scrollingElement : document.body;
                                                    }

                                                    var bodyOffsetElement = scrollingElement ? scrollingElement : document.documentElement,
                                                        bodyScrollOffset = $telerik.getScrollOffset(bodyOffsetElement, false),
                                                        scrollOffset = $telerik.getScrollOffset(contentTable, true);

                                                    scrollOffset.x -= bodyScrollOffset.x;
                                                    scrollOffset.y -= bodyScrollOffset.y;

                                                    contentTable.targetRect.x += scrollOffset.x;
                                                    contentTable.targetRect.y += scrollOffset.y;


                                                }

                                            </script>
                                        </telerik:RadScriptBlock>
                                    </asp:Panel>

                                    <telerik:RadScheduler ID="RadScheduler1" 
                                        OverflowBehavior="Scroll"   
                                        runat="server"  
                                        Skin="Material"  
                                        CssClass="RadScheduler1" 
                                        ShowHeader="true"
                                        ShowFooter="false"  
                                        Height="650"
                                        ShowNavigationPane="true"
                                        CustomAttributeNames="id"
                                        DisplayDeleteConfirmation="false"
                                        AllowDelete="false"
                                        StartEditingInAdvancedForm="false"
                                        StartInsertingInAdvancedForm="false"
                                        ShowAllDayRow="true" 
                                        EnableExactTimeRendering="true"
                                        RenderMode="Auto"
                                        OnClientAppointmentContextMenu="OnClientAppointmentContextMenu"
                                        OnClientAppointmentMoveEnd="OnClientAppointmentMoveEnd"
                                        OnClientAppointmentMoving="OnClientAppointmentMoving"
                                        OnClientAppointmentMoveStart="OnClientAppointmentMoveStart"
                                        OnClientAppointmentResizeEnd="OnClientAppointmentResizeEnd"
                                        OnClientAppointmentResizeStart="OnClientAppointmentResizeStart" 
                                        OnClientAppointmentContextMenuItemClicked="OnClientAppointmentContextMenuItemClicked"
                                        OnClientAppointmentClick="OnClientAppointmentClick"
                                        
                                        OnNavigationComplete="RadScheduler1_NavigationComplete"
                                        OnAppointmentDataBound="RadScheduler1_AppointmentDataBound"
                                        OnFormCreating="RadScheduler1_FormCreating"
                                        OnTimeSlotCreated="RadScheduler1_TimeSlotCreated"
                                        OnAppointmentCreated="RadScheduler1_AppointmentCreated"
                                        OnResourceHeaderCreated="RadScheduler1_ResourceHeaderCreated"
                                        OnTimeSlotContextMenuItemClicked="RadScheduler1_TimeSlotContextMenuItemClicked">

                                        <TimelineView UserSelectable="false" SlotDuration="30" /> 
                                     
                                        <ResourceHeaderTemplate>
                                            <asp:Panel ID="ResourceImageWrapper" runat="server" EnableTheming="true" BackColor="Yellow">
                                                <asp:Panel Style="width: 50px; height: 50px; max-height: 50px; float: left" ID="SpeakerImageDiv" runat="server">
                                                    <asp:Image ID="SpeakerImage" Style="border: none; padding: 1px; margin: 1px; margin-top: 2px; background-color: none; border-radius: 50%;" runat="server" Height="30px" Width="30px"></asp:Image>
                                                </asp:Panel>

                                                <div style="width: 60%; float: right; margin-top: 2px; text-align: left;">
                                                    <asp:Label ID="ResourceLabel" ForeColor="White" runat="server" Style="line-height: 14px; font-size: 11px !important; word-wrap: break-word; white-space: normal"></asp:Label>
                                                </div>

                                            </asp:Panel>
                                        </ResourceHeaderTemplate>
                                        <ResourceTypes>
                                            <telerik:ResourceType Name="workerProfileImage" ForeignKeyField="workerid"></telerik:ResourceType>
                                            <telerik:ResourceType Name="worker" ForeignKeyField="workerid"></telerik:ResourceType>
                                        </ResourceTypes>

                                        <AppointmentContextMenus>

                                            <telerik:RadSchedulerContextMenu runat="server" ID="SchedulerAppointmentContextMenu">
                                                <Items>
                                                    <telerik:RadMenuItem Text="Open" ToolTip="Open" Value="Edit">
                                                    </telerik:RadMenuItem>
                                                    <telerik:RadMenuItem IsSeparator="True">
                                                    </telerik:RadMenuItem>
                                                    <telerik:RadMenuItem Text="Copy" ToolTip="Copy" Value="Copy">
                                                    </telerik:RadMenuItem>
                                                    <telerik:RadMenuItem IsSeparator="True">
                                                    </telerik:RadMenuItem>
                                                    <telerik:RadMenuItem IsSeparator="True">
                                                    </telerik:RadMenuItem>
                                                    <telerik:RadMenuItem Text="Set as">
                                                        <Items>
                                                            <telerik:RadMenuItem Text="Unassigned" ToolTip="Unassigned" Value="Unassigned">
                                                            </telerik:RadMenuItem>
                                                            <telerik:RadMenuItem Text="Assigned" ToolTip="Assigned" Value="Assigned">
                                                            </telerik:RadMenuItem>
                                                            <telerik:RadMenuItem Text="Enroute" ToolTip="Enroute" Value="Enroute">
                                                            </telerik:RadMenuItem>
                                                            <telerik:RadMenuItem Text="Onsite" ToolTip="Onsite" Value="Onsite">
                                                            </telerik:RadMenuItem>
                                                            <telerik:RadMenuItem Text="Completed" ToolTip="Completed" Value="Completed">
                                                            </telerik:RadMenuItem>
                                                            <telerik:RadMenuItem Text="Hold" ToolTip="Hold" Value="Hold">
                                                            </telerik:RadMenuItem>
                                                            
                                                        </Items>
                                                    </telerik:RadMenuItem>
                                                    <telerik:RadMenuItem IsSeparator="True">
                                                    </telerik:RadMenuItem>
                                                    <telerik:RadMenuItem Text="Delete" Value="Delete" ImageUrl="images/menu_delete.png">
                                                    </telerik:RadMenuItem>
                                                    <telerik:RadMenuItem Text="Void" ToolTip="Void" Value="Void">
                                                            </telerik:RadMenuItem>
                                                    <telerik:RadMenuItem IsSeparator="True">
                                                    </telerik:RadMenuItem>
                                                    <telerik:RadMenuItem Text="Send Message" Value="SendMessage">
                                                    </telerik:RadMenuItem>
                                                </Items>
                                            </telerik:RadSchedulerContextMenu>
                                        </AppointmentContextMenus>

                                        <AppointmentTemplate>

                                           <%-- <asp:Panel ID="AppointmentPanel" CssClass="rsAptRecurrence" runat="server">--%>

                                              <span style="text-align:center; font-size: 10px; font-weight: bold;">Ticket: 
                                                            <asp:HyperLink Target="_blank" ForeColor="Blue" ID="HLTicketID" runat="Server" Style="text-decoration: underline; margin-right: 2px !important;margin-top: 0px !important;"></asp:HyperLink>
                                                        </span>
                                                <div class="appointmentHeader" style="text-wrap: normal;overflow:hidden;margin-top: 0px !important;">
                                                          <div id="Location" style="font-size: 10px;">
                                                        <asp:Label ID="lblLocation" runat="Server"></asp:Label>
                                                    </div>
                                                    <div id="Customer" style="font-size: 10px;">
                                                        <asp:Label ID="lblCustomer" runat="Server"></asp:Label>
                                                    </div>

                                                    <div id="category" style="font-size: 10px;">
                                                        <asp:Label ID="lblCategory" runat="Server"></asp:Label>
                                                    </div>
                                                    <div id="city" style="font-size: 10px;">
                                                        <asp:Label ID="lblCity" runat="Server"></asp:Label>
                                                    </div>
                                                    <div id="sub" style="font-size: 10px;">
                                                        <asp:Label ID="lblTicketSub" Visible="false" runat="Server" Text='<%#Eval("Subject")%>'></asp:Label>
                                                    </div>


                                                    <div id="TicketID" class="tcktdiv1 TicketID">

                                                      

                                                        <asp:Image ID="imgreview" Visible="false" CssClass="imgCategoryTicket" runat="server" ImageUrl="#" />

                                                        <asp:Image ID="imgConfirmed" Visible="false" CssClass="imgCategoryTicket" runat="server" ImageUrl="#" />

                                                        <asp:Image ID="imgMS" Visible="false" CssClass="imgCategoryTicket" runat="server" ImageUrl="#" />

                                                        <asp:Image ID="ImgDocument" Visible="false" CssClass="imgCategoryTicket" runat="server" ImageUrl="#" />

                                                        <asp:Image ID="imgalert" Visible="false" CssClass="imgCategoryTicket" runat="server" ImageUrl="#" />

                                                        <asp:Image ID="imgcredithold" Visible="false" CssClass="imgCategoryTicket" runat="server" ImageUrl="#" />

                                                        <asp:Image ID="ImgSignature" Visible="false" CssClass="imgCategoryTicket" runat="server" ImageUrl="#" />

                                                        <asp:Image ID="ImgChargeable" Visible="false" CssClass="imgCategoryTicket" runat="server" ImageUrl="#" />

                                                        <asp:Image ID="imgCategory" CssClass="imgCategoryTicket" runat="server" AlternateText="Icon" ToolTip="Category Icon" />

                                                         <asp:Image ID="ImgRecommendation" Visible="false" CssClass="imgCategoryTicket" runat="server" ImageUrl="#" />

                                                    </div>
                                              
                                                </div>

                                                <asp:Label ID="lblTicketID" Visible="false" runat="Server" Text='<%#Eval("id")%>'></asp:Label>


                                       <%--     </asp:Panel>--%>
                                        </AppointmentTemplate>

                                        <TimeSlotContextMenus>
                                            <telerik:RadSchedulerContextMenu ID="SchedulerTimeSlotContextMenu" runat="server">
                                                <Items>
                                                    <telerik:RadMenuItem Text="Refresh" Value="Refresh" />
                                                    <telerik:RadMenuItem IsSeparator="True" />
                                                    <telerik:RadMenuItem Text="Create new ticket" Value="Open" />
                                                    <telerik:RadMenuItem IsSeparator="True" />
                                                    <telerik:RadMenuItem Text="Go to today" Value="CommandGoToToday" />
                                                    <telerik:RadMenuItem IsSeparator="True" />
                                                    <telerik:RadMenuItem Text="Show 24 hours..." Value="CommandShow24Hours" />
                                                </Items>
                                            </telerik:RadSchedulerContextMenu>
                                        </TimeSlotContextMenus>

                                        <TimeSlotContextMenuSettings EnableDefault="true"></TimeSlotContextMenuSettings>
                                        <AppointmentContextMenuSettings EnableDefault="true"></AppointmentContextMenuSettings>
                                        <AdvancedForm Modal="false"></AdvancedForm>
                                    </telerik:RadScheduler>
                                
                                </div> 
                            </telerik:RadAjaxPanel> 
                        </telerik:RadPane>
                    </telerik:RadSplitter>
                </telerik:RadPane>
            </telerik:RadSplitter> 


            <telerik:RadLabel Visible="false" runat="server" ID="RadLabel1" Text="Skin" AssociatedControlID="RadSkinManager1"></telerik:RadLabel>
            <telerik:RadSkinManager ID="RadSkinManager1" Visible="true" Skin="Material" runat="server" ShowChooser="false" />

            <telerik:RadLabel runat="server" Visible="false" Text="View" ID="RadLabel4" AssociatedControlID="ddlviewBy"></telerik:RadLabel>
            <telerik:RadDropDownList runat="server" Visible="false" ID="ddlviewBy" AutoPostBack="true" OnSelectedIndexChanged="btnToggle_Click">
                <Items>
                    <telerik:DropDownListItem Text="By Worker" Value="0" />
                    <telerik:DropDownListItem Text="By Time" Value="1" />
                </Items>
            </telerik:RadDropDownList>

            <telerik:RadLabel runat="server" Visible="false" Text="View Direction" ID="RadLabel2" AssociatedControlID="ddlViewDirection"></telerik:RadLabel>
            <telerik:RadDropDownList runat="server" Visible="false" ID="ddlViewDirection" AutoPostBack="true" OnSelectedIndexChanged="btnToggle_Click">
                <Items>
                    <telerik:DropDownListItem Text="Horizontal" Value="0" />
                    <telerik:DropDownListItem Text="Vertical" Value="1" />
                </Items>
            </telerik:RadDropDownList>

            <telerik:RadAjaxPanel ID="rapButton" runat="server">
            </telerik:RadAjaxPanel>
            <%--  OnClick="btnAppointmentMove_Click"--%>
          <%--  <asp:Button ID="btnAppointmentMove" runat="server" Text="" Style="display: none;" CausesValidation="false"></asp:Button>
            <asp:Button ID="btnAppointmentResize" runat="server" Text="" Style="display: none;" CausesValidation="false" OnClick="btnAppointmentResize_Click"></asp:Button>--%>
            <asp:Button ID="TicketDelete" runat="server" Text="" Style="display: none;" CausesValidation="false" OnClick="TicketDelete_Click"></asp:Button>
              <asp:Button ID="btnTicketVoid" runat="server" Text="" Style="display: none;" CausesValidation="false" OnClick="btnTicketVoid_Click"></asp:Button>
            <%-- Ticket Delete--%>

               <asp:HiddenField ID="hdnDeleteTicketId" runat="server" Value="" />

               <asp:HiddenField ID="hdnTicketVoidID" runat="server" Value="0" />
               <asp:HiddenField ID="hdnTicketVoidPermission" runat="server" Value="Y" />

            <%-- Ticket Resize--%>

            <asp:HiddenField ID="hdnResizeTicketId" runat="server" Value="" />
            <asp:HiddenField ID="hdnResizeNewTimeSlotIndex" runat="server" Value="" />
            <asp:HiddenField ID="hdnResizeNewStartDate" runat="server" Value="" />
            <asp:HiddenField ID="hdnResizeNewEndDate" runat="server" Value="" />

            <%-- Ticket MOVE--%>

            <asp:HiddenField ID="hdnMoveTicketId" runat="server" Value="" />
            <asp:HiddenField ID="hdnMoveNewTimeSlotIndex" runat="server" Value="" />
            <asp:HiddenField ID="hdnStartTime" runat="server" Value="" />

            <%-- Ticket Assign--%>
            <asp:HiddenField runat="server" ID="hdnAssignworker" />
            <asp:HiddenField runat="server" ID="hdnAssigndate" />
            <asp:HiddenField runat="server" ID="TargetSlotHiddenField" />


            <asp:HiddenField runat="server" ID="hdnAddeTicket" Value="Y" />
            <asp:HiddenField runat="server" ID="hdnEditeTicket" Value="Y" />
            <asp:HiddenField runat="server" ID="hdnDeleteTicket" Value="Y" />
            <asp:HiddenField runat="server" ID="hdnViewTicket" Value="Y" />

            <%--Check action of scheduler--%>
            <asp:HiddenField runat="server" ID="hdnCheckActionScheduler" Value="0" />

            <%-- Open mail page OnClick="btnMail_Click" --%>
            <telerik:RadAjaxPanel ID="rapbtnMail" runat="server">
                <asp:Button ID="btnMail" runat="server" Text="Email" Style="display: none;"
                    CausesValidation="False" />
            </telerik:RadAjaxPanel>


        </div>
    </div>
 
 
     </div> 
      
    </form>

    <script src="Design/js/jquery.mCustomScrollbar.js"></script>
    <%--<script type="text/javascript" src="Design/js/plugins/jquery-1.11.2.min.js"></script>--%>
    <script type="text/javascript" src="Design/js/materialize.js"></script>
    <script type="text/javascript" src="Design/js/plugins/prism/prism.js"></script>
    <script type="text/javascript" src="Design/js/plugins/perfect-scrollbar/perfect-scrollbar.min.js"></script>
    <script type="text/javascript" src="Design/js/plugins.js"></script>
    <script type="text/javascript" src="Design/js/custom-script.js"></script>
    <%--Calendar Scripts--%>
    <script src="Design/js/moment.js"></script>
    <script src="Design/js/pikaday.js"></script>
    <script src="Design/js/pikaday.jquery.js"></script>
    <%--    <script src='https://cdnjs.cloudflare.com/ajax/libs/moment.js/2.15.1/moment.min.js'></script>
    <script src='https://rawgit.com/tieppt/Pikaday/master/pikaday.js'></script>--%>

    <%--Calendar Scripts--%>

    <%--Masked Input Scripts--%>
   <%-- <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/jquery.maskedinput/1.4.1/jquery.maskedinput.min.js"></script>--%>
    <%--Masked Input Scripts--%>
  

    <script>

</script>
  
</body>
</html>