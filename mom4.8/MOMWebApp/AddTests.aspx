    <%@ Page Title="" Language="C#" MasterPageFile="~/Mom.master" AutoEventWireup="true" Inherits="AddTests" Codebehind="AddTests.aspx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <!--Grid Control-->
    <link href="Design/css/grid.css" rel="stylesheet" />

    <link href="Design/css/pikaday.css" rel="stylesheet" />
      <script type="text/javascript" src="js/jquery.formatCurrency.js"></script>
    <style>
        a[disabled=disabled] {
            color: gray;
        }
        /** Columns */
        .rcbHeader ul,
        .rcbFooter ul,
        .rcbItem ul,
        .rcbHovered ul,
        .rcbDisabled ul {
            margin: 0;
            padding: 0;
            width: 100%;
            display: inline-block;
            list-style-type: none;
        }

        .RadComboBoxDropDown .rcbList {
            margin-top: -6px !important;
        }

            .RadComboBoxDropDown .rcbList li {
                padding-top: 5px !important;
            }

        .RadComboBoxDropDown .rcbCheckBox {
            top: 4px !important;
        }
        input:disabled {
            margin: 9px 0px 7px 0px!important;
        }
        .rcbScroll {
            overflow: scroll !important;
            overflow-x: hidden !important;
        }

        .colCombo {
            margin-right: 5px;
            padding: 0 5px 0 0;
            width: 16%;
            line-height: 14px;
            float: left;
            word-break: break-all;
        }

        .cbHeader {
            background-color: #e7e5e5 !important;
            color: #2e6b89 !important;
            border-bottom: 1px solid #ccc !important;
            background-image: url(../Design/images/accrd.gif) !important;
            background-repeat: repeat-x !important;
            padding: 0 !important;
            font-size: 0.9rem;
        }

        .comboboxHeader {
            margin: -20px 0 11px 30px !important;
            float: left;
            color: black;
        }

        .comboboxHeaderTitle {
            line-height: 40px;
            background-color: whitesmoke;
            margin: -4px 0px -5px 1px !important;
        }

        .comboboxFooter {
            line-height: 40px;
            background-color: whitesmoke;
            margin: 0px -12px -4px -12px;
            padding-left: 10px !important;
        }

        .rcbList > li:first-child {
            background-color: #edf4fc;
        }

        .rcbList > li:nth-child(odd) {
            background-color: #edf4fc;
        }

        .RadComboBoxDropDown .rcbHeader {
            padding: 0 !important;
        }

      #dvCustomSetup .itemHeader {
            color: #2e6b89 !important;
            font-weight: bold !important;
        }
        #dvContactSetup .itemHeader ,#tabProposals .itemHeader ,#tabSchedule .itemHeader {
            color: #2e6b89 !important;
            font-weight: bold !important;
        }
        
        /** Multiple rows and columns */
        .multipleRowsColumns .rcbItem,
        .multipleRowsColumns .rcbHovered {
            float: left;
            margin: 0 1px;
            min-height: 13px;
            overflow: hidden;
            padding: 2px 19px 2px 6px;
            width: 193px;
        }


        .results {
            display: block;
            margin-top: 20px;
        }

        .font_09rem {
            font-size: 0.9rem !important;
        }

         .fontHeader {          
                 
            font-size: .91rem !important;
        }
         .breakContent{
             word-break:break-all;
         }
          .RadGrid_Popup > div > div.rgDataDiv {
            height: 450px !important;
        }

        input[class*='txtMembers_'] {
            cursor: pointer;
        }
         input[class*='txtRoles_'] {
            cursor: pointer;
        }
          .chipUsers{
            width:auto !important;
            padding-left:5px !important;
            padding-right:5px !important ;
            margin-left:2px !important ;
            margin-right:2px !important ;
            margin-top:3px !important ;
        }

         .browser-default {
                margin-top: 0px!important;
                margin-bottom: 8px!important;
        }   
        .tag-div{
            margin-bottom: 0px!important;
            margin-top: 0px!important;
             height:2rem!important;
        }
        .chipUserRoles{
            background-color: #2bab54 !important;
            width:auto !important;
            padding-left:5px !important;
            padding-right:5px !important ;
            margin-left:2px !important ;
            margin-right:2px !important ;
            margin-top:3px !important ;
        }
        .chip > label{
            font-size: 13px;
            font-weight: normal;
            color: #fff;
            line-height: 22px;
        }
        /* The container */
        .cusCheckContainer {
            display: block;
            position: relative;
            padding-left: 12px;
            margin-bottom: 1px;
            cursor: pointer;
            font-size: 15px;
            -webkit-user-select: none;
            -moz-user-select: none;
            -ms-user-select: none;
            user-select: none;
            padding-right: 3px;
        }

        /* Hide the browser's default checkbox */
        .cusCheckContainer input {
            position: absolute;
            opacity: 0;
            cursor: pointer;
            height: 0;
            width: 0;
        }
        .checkrow label{
            top: -1px !important;
        }

        /* Create a custom checkbox */
       .checkmark {
            position: absolute;
            top: 7px;
            left: 3px;
            height: 8px;
            width: 8px;
            border-radius: 9px;
            background-color: black;
        }
        .chip {
            display: inline-block;
            height: 22px;
            font-size: 10px;
            font-weight: normal;
            color: #fff;
            line-height: 23px;
            padding: 0px 2px;
            border-radius: 12px;
            margin:1px;
            background-color: #1565c0;
        }

        /* On mouse-over, add a grey background color */
        .cusCheckContainer:hover input ~ .checkmark {
            background-color: black;
        }

        /* When the checkbox is checked, add a blue background */
        .cusCheckContainer input:checked ~ .checkmark {
            background-color: black;
        }

        /* Create the checkmark/indicator (hidden when not checked) */
        .checkmark:after {
            content: "";
            position: absolute;
            display: none;
        }

        /* Show the checkmark when checked */
        .cusCheckContainer input:checked ~ .checkmark:after {
            display: block;
        }

        /* Style the checkmark/indicator */
        .cusCheckContainer .checkmark:after {
            left: 5px;
            top: 1px;
            width: 6px;
            height: 10px;
            border: solid white;
            border-width: 0 2px 2px 0;
            -webkit-transform: rotate(45deg);
            -ms-transform: rotate(45deg);
            transform: rotate(45deg);
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
    </style>
    
     <script>
          function BindClickEventForGridCheckBox() {
            $("#<%=RadGrid_Emails.ClientID%> input[id*='chkSelect']:checkbox").unbind('click').bind('click', function () {
                CheckUncheckAllCheckBoxAsNeeded();
            });

            $("#<%=RadGrid_Emails.ClientID%> input[id*='chkTask']:checkbox").unbind('click').bind('click', function () {
                OnCheck_TaskCheckBox('<%=RadGrid_Emails.ClientID%>');
            });

            $("#<%=RadGrid_Emails.ClientID%> input[id*='chkAll']:checkbox").unbind('click').bind('click', function () {
                if ($(this).is(':checked')) {
                    $("#<%=RadGrid_Emails.ClientID%> input[id*='chkSelect']:checkbox").prop('checked', true);
                }
                else {
                    $("#<%=RadGrid_Emails.ClientID%> input[id*='chkSelect']:checkbox").prop('checked', false);
                }
                CheckEmailsCheckBox('<%=RadGrid_Emails.ClientID%>');
            });

            UpdatedivDisplayTeamMember();
           

            var line = $("#<%= hdnLineOpenned.ClientID%>").val();
            if (line != null && line != '') {
                var hdnMembersID = $(".txtMembers_" + line).attr("id").replace("txtMembers", "hdnMembers");
                var teamMembers = $("#" + hdnMembersID).val();
                
                // Update selected for grid
                if (teamMembers != null && teamMembers != "") {
                    var teamArr = teamMembers.toString().split(';');
                    // trim value of teamArr
                    $.each(teamArr, function (index, value) {
                        teamArr[index] = value.trim();
                    });

                    $("#<%=RadGrid_Emails.ClientID%> input[id*='chkSelect']:checkbox").each(function (index) {
                        var userId = $(this).closest('tr').find('td:eq(0)').find('span').html();
                        var taskCheckID = this.id.replace("chkSelect", "chkTask");
                        var intUserType = userId.split('_')[0];

                        if (teamArr.indexOf(userId) >= 0) {
                            $(this).prop('checked', true);
                            if (intUserType == 0 || intUserType == 1 || intUserType == 6 || intUserType == 7) {
                                $("#" + taskCheckID).prop('disabled', false);
                            }
                            else {
                                $("#" + taskCheckID).prop('disabled', true);
                            }
                        } else {
                            $(this).prop('checked', false);
                            $("#" + taskCheckID).prop('disabled', true);
                        }
                    });
                } else {
                    $("#<%=RadGrid_Emails.ClientID%> input[id*='chkSelect']:checkbox").prop('checked', false);
                }
            }
        }
        function UpdatedivDisplayTeamMember() {
            var txtMembers = $("#<%=gvTestCustom.ClientID %> input[id*='txtMembers']");
            $.each(txtMembers, function (index, item) {
                var txtId = $(item).attr("id");
                var div = document.getElementById(txtId.replace("txtMembers", "cusLabelTag"));
                var hdnMembersID = txtId.replace("txtMembers", "hdnMembers");
                var hdnMembersValue = $("#" + hdnMembersID).val();
                div.innerHTML = '';
                var disTeamMembers = $(item).val();
                // Update selected for grid
                if (disTeamMembers != null && disTeamMembers != "") {
                    var teamArr = disTeamMembers.toString().split(';');
                    var teamKeyArr = hdnMembersValue.toString().split(';');
                    // trim value of teamArr
                    $.each(teamArr, function (index, value) {
                        teamArr[index] = value.trim();
                    });

                    if (teamArr != null && teamArr.length > 0) {
                        for (var i = 0; i < teamArr.length; i++) {
                            var tempTeamKeyArr = teamKeyArr[i].toString().split('_');
                            var tag = "";
                            if (teamKeyArr[i].indexOf('6') == 0) {
                                if (tempTeamKeyArr.length == 3 && tempTeamKeyArr[2] == "1")
                                    //tag = "<div class='chip chipUserRoles'><input type='checkbox' checked='checked' disabled style='margin-right: 5px;'/><label>" + teamArr[i] + "</label></div>";
                                    tag = "<div class='chip chipUserRoles'><label  class='cusCheckContainer'>" + teamArr[i] + "<input type='checkbox' checked='checked' disabled><span class='checkmark'></span></label></div>";
                                else
                                    tag = "<div class='chip chipUserRoles'><label  class='cusCheckContainer'>" + teamArr[i] + "<input type='checkbox' disabled><span class='checkmark'></span></label></div>";
                                    //tag = "<div class='chip chipUserRoles'><input type='checkbox' disabled  style='margin-right: 5px;' /><label>" + teamArr[i] + "</label></div>";
                                div.innerHTML += tag;
                            } else if (teamKeyArr[i].indexOf('7') == 0) {
                                if (tempTeamKeyArr.length >= 3) {
                                    // && tempTeamKeyArr[2] == "1"
                                    var tempTitle = tempTeamKeyArr[tempTeamKeyArr.length - 2];
                                    if (tempTeamKeyArr[tempTeamKeyArr.length - 1] == "1" && tempTitle.charAt(tempTitle.length - 1) == "|") {
                                        tag = "<div class='chip chipUserRoles'><label  class='cusCheckContainer'>" + teamArr[i] + "<input type='checkbox' checked='checked' disabled><span class='checkmark'></span></label></div>";
                                        //tag = "<div class='chip chipUserRoles'><input type='checkbox' checked='checked' disabled style='margin-right: 5px;'/><label>" + teamArr[i] + "</label></div>";
                                    } else {
                                        tag = "<div class='chip chipUserRoles'><label  class='cusCheckContainer'>" + teamArr[i] + "<input type='checkbox' disabled><span class='checkmark'></span></label></div>";
                                        //tag = "<div class='chip chipUserRoles'><input type='checkbox' disabled  style='margin-right: 5px;' /><label>" + teamArr[i] + "</label></div>";
                                    }
                                }
                                else
                                    tag = "<div class='chip chipUserRoles'><label  class='cusCheckContainer'>" + teamArr[i] + "<input type='checkbox' disabled><span class='checkmark'></span></label></div>";
                                    //tag = "<div class='chip chipUserRoles'><input type='checkbox' disabled  style='margin-right: 5px;' /><label>" + teamArr[i] + "</label></div>";
                                div.innerHTML += tag;
                            } else {
                                if (tempTeamKeyArr.length == 3 && (tempTeamKeyArr[0] == "0" || tempTeamKeyArr[0] == "1") && tempTeamKeyArr[2] == "1")
                                    tag = "<div class='chip chipUsers'><label  class='cusCheckContainer'>" + teamArr[i] + "<input type='checkbox' checked='checked' disabled><span class='checkmark'></span></label></div>";
                                    //tag = "<div class='chip chipUsers' ><input type='checkbox' checked='checked' disabled  style='margin-right: 5px;'/><label>" + teamArr[i] + "</label></div>";
                                else
                                    tag = "<div class='chip chipUsers'><label  class='cusCheckContainer'>" + teamArr[i] + "<input type='checkbox' disabled><span class='checkmark'></span></label></div>";
                                    //tag = "<div class='chip chipUsers' ><input type='checkbox' disabled  style='margin-right: 5px;'/><label>" + teamArr[i] + "</label></div>";
                                div.innerHTML += tag;
                            }
                        }
                    }
                }
            });
        }

        function CloseTeamMemberWindow() {
            var line = $("#<%= hdnLineOpenned.ClientID%>").val();
            if (line != null && line != '') {
                var hdnMembersValue = $("#<%= hdnOrgMemberKey.ClientID%>").val();
                var txtMembersValue = $("#<%= hdnOrgMemberDisp.ClientID%>").val();

                var txtMembersID = $(".txtMembers_" + line).attr("id");
                $("#" + txtMembersID).val(txtMembersValue);
                var hdnMembersID = txtMembersID.replace("txtMembers", "hdnMembers");
                $("#" + hdnMembersID).val(hdnMembersValue);

                var div = document.getElementById(txtMembersID.replace("txtMembers", "cusLabelTag"));
                div.innerHTML = '';
                var disTeamMembers = txtMembersValue;
                // Update selected for grid
                if (disTeamMembers != null && disTeamMembers != "") {
                    var teamArr = disTeamMembers.toString().split(';');
                    var teamKeyArr = hdnMembersValue.toString().split(';');
                    // trim value of teamArr
                    $.each(teamArr, function (index, value) {
                        teamArr[index] = value.trim();
                    });

                    if (teamArr != null && teamArr.length > 0)
                        for (var i = 0; i < teamArr.length; i++) {
                            var tempTeamKeyArr = teamKeyArr[i].toString().split('_');
                            var tag = "";
                            if (teamKeyArr[i].indexOf('6') == 0) {
                                if (tempTeamKeyArr.length == 3 && tempTeamKeyArr[2] == "1")
                                    tag = "<div class='chip chipUserRoles'><label  class='cusCheckContainer'>" + teamArr[i] + "<input type='checkbox' checked='checked' disabled><span class='checkmark'></span></label></div>";
                                    //tag = "<div class='chip chipUserRoles'><input type='checkbox' checked='checked' disabled style='margin-right: 5px;'/><label>" + teamArr[i] + "</label></div>";
                                else
                                    tag = "<div class='chip chipUserRoles'><label  class='cusCheckContainer'>" + teamArr[i] + "<input type='checkbox' disabled><span class='checkmark'></span></label></div>";
                                    //tag = "<div class='chip chipUserRoles'><input type='checkbox' disabled  style='margin-right: 5px;' /><label>" + teamArr[i] + "</label></div>";
                                div.innerHTML += tag;
                            } else if (teamKeyArr[i].indexOf('7') == 0) {
                                if (tempTeamKeyArr.length >= 3) {
                                    // && tempTeamKeyArr[2] == "1"
                                    var tempTitle = tempTeamKeyArr[tempTeamKeyArr.length - 2];
                                    if (tempTeamKeyArr[tempTeamKeyArr.length - 1] == "1" && tempTitle.charAt(tempTitle.length - 1) == "|") {
                                        tag = "<div class='chip chipUserRoles'><label  class='cusCheckContainer'>" + teamArr[i] + "<input type='checkbox' checked='checked' disabled><span class='checkmark'></span></label></div>";
                                        //tag = "<div class='chip chipUserRoles'><input type='checkbox' checked='checked' disabled style='margin-right: 5px;'/><label>" + teamArr[i] + "</label></div>";
                                    } else {
                                        tag = "<div class='chip chipUserRoles'><label  class='cusCheckContainer'>" + teamArr[i] + "<input type='checkbox' disabled><span class='checkmark'></span></label></div>";
                                        //tag = "<div class='chip chipUserRoles'><input type='checkbox' disabled  style='margin-right: 5px;' /><label>" + teamArr[i] + "</label></div>";
                                    }
                                }
                                else
                                    tag = "<div class='chip chipUserRoles'><label  class='cusCheckContainer'>" + teamArr[i] + "<input type='checkbox' disabled><span class='checkmark'></span></label></div>";
                                    //tag = "<div class='chip chipUserRoles'><input type='checkbox' disabled  style='margin-right: 5px;' /><label>" + teamArr[i] + "</label></div>";
                                div.innerHTML += tag;
                            } else {
                                if (tempTeamKeyArr.length == 3 && (tempTeamKeyArr[0] == "0" || tempTeamKeyArr[0] == "1") && tempTeamKeyArr[2] == "1")
                                    tag = "<div class='chip chipUsers'><label  class='cusCheckContainer'>" + teamArr[i] + "<input type='checkbox' checked='checked' disabled><span class='checkmark'></span></label></div>";
                                    //tag = "<div class='chip chipUsers' ><input type='checkbox' checked='checked' disabled  style='margin-right: 5px;'/><label>" + teamArr[i] + "</label></div>";
                                else
                                    tag = "<div class='chip chipUsers'><label  class='cusCheckContainer'>" + teamArr[i] + "<input type='checkbox' disabled><span class='checkmark'></span></label></div>";
                                    //tag = "<div class='chip chipUsers' ><input type='checkbox' disabled  style='margin-right: 5px;'/><label>" + teamArr[i] + "</label></div>";
                                div.innerHTML += tag;
                            }
                        }
                }
            }
            var wnd = $find('<%=TeamMembersWindow.ClientID %>');
            wnd.Close();
        }

        function ShowTeamMemberWindow(txtTeamMember) {
            var line = $(txtTeamMember).closest('tr').find('td:eq(0) > span.customline').text();
            var txtTeamMembersId = $(txtTeamMember).attr("id");
            var hdnTeamMembersId = txtTeamMembersId.replace("cusLabelTag", "hdnMembers");
            var teamMembers = $("#" + hdnTeamMembersId).val();
            var txtCustomLabelId = txtTeamMembersId.replace("cusLabelTag", "lblCustom");
            var txtCustomLabelVal = $("#" + txtCustomLabelId).text();
            var txtTeamMemberDispId = txtTeamMembersId.replace("cusLabelTag", "txtMembers");
            var txtTeamMemberDispVal = $("#" + txtTeamMemberDispId).val();

            $('#<%= hdnLineOpenned.ClientID%>').val(line);
            $('#<%= hdnOrgMemberKey.ClientID%>').val(teamMembers);
            $('#<%= hdnOrgMemberDisp.ClientID%>').val(txtTeamMemberDispVal);

            $("#<%=RadGrid_Emails.ClientID%> input[id*='chkSelect']:checkbox").unbind('click').bind('click', function () {
                CheckUncheckAllCheckBoxAsNeeded();
            });

            $("#<%=RadGrid_Emails.ClientID%> input[id*='chkTask']:checkbox").unbind('click').bind('click', function () {
                OnCheck_TaskCheckBox('<%=RadGrid_Emails.ClientID%>');
            });

            $("#<%=RadGrid_Emails.ClientID%> input[id*='chkAll']:checkbox").unbind('click').bind('click', function () {
                if ($(this).is(':checked')) {
                    $("#<%=RadGrid_Emails.ClientID%> input[id*='chkSelect']:checkbox").prop('checked', true);
                }
                else {
                    $("#<%=RadGrid_Emails.ClientID%> input[id*='chkSelect']:checkbox").prop('checked', false);
                }
                CheckEmailsCheckBox('<%=RadGrid_Emails.ClientID%>');
            });

            // Update selected for grid
            if (teamMembers != null && teamMembers != "") {
                var teamArr = teamMembers.toString().split(';');
                var teamArrWithTask = teamMembers.toString().split(';');
                // trim value of teamArr
                $.each(teamArr, function (index, value) {
                    var temp = value.trim().split('_');
                    
                    if (temp.length == 3) {
                        temp.splice(2, 1);
                        teamArr[index] = temp.join("_");
                    } else {
                        teamArr[index] = value.trim();
                    }
                });

                $("#<%=RadGrid_Emails.ClientID%> input[id*='chkSelect']:checkbox").each(function (index) {
                    var userId = $(this).closest('tr').find('td:eq(0)').find('span').html();
                    var taskCheckID = this.id.replace("chkSelect", "chkTask");
                    var intUserType = userId.split('_')[0];

                    var idx = teamArr.indexOf(userId);
                    if (idx >= 0) {
                        $(this).prop('checked', true);
                        var memberkeywithTask = teamArrWithTask[idx].split('_');
                        if (memberkeywithTask.length == 3 && memberkeywithTask[2] == 1) {
                            $("#" + taskCheckID).prop('checked', true);
                        } else {
                            $("#" + taskCheckID).prop('checked', false);
                        }

                        if (intUserType == 0 || intUserType == 1 || intUserType == 6 || intUserType == 7) {
                            $("#" + taskCheckID).prop('disabled', false);
                        }
                        else {
                            $("#" + taskCheckID).prop('disabled', true);
                        }
                    } else {
                        $(this).prop('checked', false);
                        $("#" + taskCheckID).prop('disabled', true);
                        $("#" + taskCheckID).prop('checked', false);
                    }
                });
            } else {
                $("#<%=RadGrid_Emails.ClientID%> input[id*='chkSelect']:checkbox").prop('checked', false);
            }

            var wnd = $find('<%=TeamMembersWindow.ClientID %>');
            wnd.set_title("Team Member: " + txtCustomLabelVal);
            wnd.Show();
        }

        function CheckEmailsCheckBox(gridview) {
            var line = $("#<%= hdnLineOpenned.ClientID%>").val();
            var hdnMembersID = $(".txtMembers_" + line).attr("id").replace("txtMembers", "hdnMembers");
            var hdnMembersValue = $("#<%= hdnOrgMemberKey.ClientID%>").val();
            var txtMembersValue = $("#<%= hdnOrgMemberDisp.ClientID%>").val();

            var tempArrayKey = [];
            tempArrayKey.length = 0;
            var tempArrayDisplay = [];
            tempArrayDisplay.length = 0;

            $("#" + gridview + " input[id*='chkSelect']:checkbox").each(function (index) {
                var tempMemberKey = $(this).closest('tr').find('td:eq(0)').find('span').html().trim();
                var teamMemberDisp = $(this).closest('tr').find('td:eq(1)').find('span').html().trim();
                if (teamMemberDisp == "")
                    teamMemberDisp = $(this).closest('tr').find('td:eq(2)').find('span').html().trim();

                var intUserType = tempMemberKey.split('_')[0];

                var temp = tempMemberKey.trim().split('_');
                if (temp.length == 3) {
                    temp.splice(2, 1);
                } 

                var taskCheckID = this.id.replace("chkSelect", "chkTask");
                if ($(this).is(":checked")) {
                    if (intUserType == 0 || intUserType == 1 || intUserType == 6 || intUserType == 7) {
                        $("#" + taskCheckID).prop('disabled', false);
                    }
                    else {
                        $("#" + taskCheckID).prop('disabled', true);
                    }
                    if (jQuery.inArray(tempMemberKey, tempArrayKey) < 0) {
                        if ($("#" + taskCheckID).is(":checked")) {
                            temp.push(1);
                        } else {
                            temp.push(0);
                        }
                        tempMemberKey = temp.join("_")
                        tempArrayKey.push(tempMemberKey);
                        tempArrayDisplay.push(teamMemberDisp);
                    }
                } else {
                    if (jQuery.inArray(tempMemberKey, tempArrayKey) >= 0) {
                        tempArrayKey = jQuery.grep(tempArrayKey, function (value) {
                            return value != tempMemberKey;
                        });
                        tempArrayDisplay = jQuery.grep(tempArrayDisplay, function (value) {
                            return value != teamMemberDisp;
                        });
                    }
                    $("#" + taskCheckID).prop('checked', false);
                    $("#" + taskCheckID).prop('disabled', true);
                }
            });
            
            $("#<%= hdnOrgMemberKey.ClientID%>").val(tempArrayKey.join(";"));
            $("#<%= hdnOrgMemberDisp.ClientID%>").val(tempArrayDisplay.join(";"));
        }

        function CheckUncheckAllCheckBoxAsNeeded() {
            var totalCheckboxes = $("#<%=RadGrid_Emails.ClientID%> input[id*='chkSelect']:checkbox").size();

            var checkedCheckboxes = $("#<%=RadGrid_Emails.ClientID%> input[id*='chkSelect']:checkbox:checked").size();

            if (totalCheckboxes == checkedCheckboxes) {

                $("#<%=RadGrid_Emails.ClientID %> input[id*='chkAll']:checkbox").prop('checked', true);//.each(function () { this.checked = true; });
            }
            else {
                $("#<%=RadGrid_Emails.ClientID %> input[id*='chkAll']:checkbox").prop('checked', false);//.attr('checked', false);
            }

            if ($('#<%=RadGrid_Emails.ClientID%>').length > 0) {
                CheckEmailsCheckBox('<%=RadGrid_Emails.ClientID%>');
            }
        }

        function OnCheck_TaskCheckBox(gridview) {
            var line = $("#<%= hdnLineOpenned.ClientID%>").val();
            var hdnMembersID = $(".txtMembers_" + line).attr("id").replace("txtMembers", "hdnMembers");
            var hdnMembersValue = $("#<%= hdnOrgMemberKey.ClientID%>").val();

            var tempArrayKey = [];
            tempArrayKey.length = 0;
            $("#" + gridview + " input[id*='chkSelect']:checkbox").each(function (index) {
                var tempMemberKey = $(this).closest('tr').find('td:eq(0)').find('span').html().trim();

                var temp = tempMemberKey.trim().split('_');
                if (temp.length == 3) {
                    temp.splice(2, 1);
                }

                var taskCheckID = this.id.replace("chkSelect", "chkTask");

                if ($(this).is(":checked")) {
                    if (jQuery.inArray(tempMemberKey, tempArrayKey) < 0) {
                        if ($("#" + taskCheckID).is(":checked")) {
                            temp.push(1);
                        } else {
                            temp.push(0);
                        }
                        tempMemberKey = temp.join("_")
                        tempArrayKey.push(tempMemberKey);
                    }
                } else {
                    if (jQuery.inArray(tempMemberKey, tempArrayKey) >= 0) {
                        tempArrayKey = jQuery.grep(tempArrayKey, function (value) {
                            return value != tempMemberKey;
                        });
                    }
                }
            });

            $("#<%= hdnOrgMemberKey.ClientID%>").val(tempArrayKey.join(";"));
        }
    </script>
      <script>
          
    </script>
    <script type="text/javascript">
       
        var changes = 0;
        $(document).on("change", ":input", function () {
            changes = 1;
        });

        $(document).ready(function () {
            var queryloc = "";
            var query = "";
            function dtaa() {
                this.prefixText = null;
                this.con = 0;
                this.custID = null;
            }
            function locc() {
                this.SearchValue = null;
            }
             
            $("#<%= txtWorker.ClientID %>").autocomplete(
                {
                    source: function (request, response) {
                        var dtaaa = new locc();
                        dtaaa.SearchValue = request.term;
                        queryloc = request.term;
                        $.ajax({
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            url: "CustomerAuto.asmx/GetWorkers",
                            data: JSON.stringify(dtaaa),
                            dataType: "json",
                            async: true,
                            success: function (data) {
                                response($.parseJSON(data.d));
                            },
                            error: function (result) {
                                alert("Due to unexpected errors we were unable to load workers");
                            }

                        });

                    },
                    select: function (event, ui) {
                        $("#<%= txtWorker.ClientID %>").val(ui.item.fDesc);
                        $("#<%= hdnWorker.ClientID %>").val(ui.item.id);
                        return false;
                    },
                    focus: function (event, ui) {
                        $("#<%= txtWorker.ClientID %>").val(ui.item.fDesc);
                        $("#<%= hdnWorker.ClientID %>").val(ui.item.id);
                        return false;
                    },
                    minLength: 0,
                    delay: 250
                })
                .data("ui-autocomplete")._renderItem = function (ul, item) {                    
                    var result_desc = item.fDesc;                   
                    return $("<li></li>")
                        .data("ui-autocomplete-item", item)
                        .append("<a><span style='color:Gray;'>" + result_desc + "</span></a>")
                        .appendTo(ul);
                };

            $("#<%= txtWorker.ClientID %>").keyup(function (event) {

                if (document.getElementById('<%= txtWorker.ClientID %>').value == '') {

                    $("#<%= txtWorker.ClientID %>").val('');
                    $("#<%=hdnWorker.ClientID%>").val('');
                }
            });

           
            $("#<%= txtAccount.ClientID %>").autocomplete(
                {
                    source: function (request, response) {
                        var dtaaa = new dtaa();
                        dtaaa.prefixText = request.term;
                        dtaaa.custID = 0;


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
                        $("#<%= txtAccount.ClientID %>").val(ui.item.label);
                        $("#<%= hdnaccount.ClientID %>").val(ui.item.value);
                       
                            if($("#<%= divContact.ClientID %>").css('display') != 'none'){
                                var btn = document.getElementById('<%=btnReloadContact.ClientID%>');
                                btn.click(); 
                        }
                                             
                        return false;
                    },
                    focus: function (event, ui) {
                        $("#<%= txtAccount.ClientID %>").val(ui.item.label);
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
                        .data("ui-autocomplete-item", item)
                        .append("<a>" + result_item + ", <span style='color:Gray;'>" + result_desc + "</span></a>")
                        .appendTo(ul);
                };


            $("#<%= txtAccount.ClientID %>").keyup(function (event) {
                var hdnLocId = document.getElementById('<%= hdnaccount.ClientID %>');
                if (document.getElementById('<%= txtAccount.ClientID %>').value == '') {
                    hdnLocId.value = '';
                    $("#<%= txtJob.ClientID %>").val('');
                    $("#<%=hdnjob.ClientID%>").val('');
                }
            });



            $("#<%= txtUnit.ClientID %>").autocomplete({
                source: function (request, response) {                  
                    var objdataEquip = new dtaa();
                    objdataEquip.prefixText = request.term;
                    objdataEquip.custID = $("#<%=hdnaccount.ClientID%>").val();
                    query = request.term;
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "CustomerAuto.asmx/GetEquipment",
                        data: JSON.stringify(objdataEquip),
                        dataType: "json",
                        async: true,
                        success: function (data) {
                            response($.parseJSON(data.d));
                        },
                        error: function (result) { }
                    });                   
                },
                select: function (event, ui) {                        
                    
                    $("#<%= txtUnit.ClientID %>").val(ui.item.unit);
                    $("#<%=hdnEquipment.ClientID%>").val(ui.item.id);
                    $("#<%= txtstate.ClientID%>").val(ui.item.state);
                    $("#<%= txtEquipmentType.ClientID%>").val(ui.item.type);
                    //Pricing
                     $("#<%= ddlClassification.ClientID%>").val(ui.item.Classification);
                    $("#<%= txtEquipmentDesc.ClientID%>").val(ui.item.fdesc);
                    reloadTestPrice();                   
                    Materialize.updateTextFields();
                    return false;
                },
                focus: function (event, ui) {
                    //$("#<%= txtUnit.ClientID %>").val(ui.item.label);
                    return false;
                },
                minLength: 0,
                delay: 250
            })
                .data("ui-autocomplete")._renderItem = function (ul, item) {
                    // debugger;
                    var result_item = item.unit;
                    if (item.id != "") {
                        result_item = item.id + ", Unit#: " + item.unit + " ,Type: " + item.type;
                    }
                    var result_desc = "Customer: " + item.name;
                    var result_descLoc = "Location: " + item.locid + "-" + item.tag;

                    var x = new RegExp('\\b' + query, 'ig');
                    result_item = result_item.replace(x, function (FullMatch, n) {
                        return '<span class="highlight">' + FullMatch + '</span>'
                    });
                    if (result_desc != null) {
                        result_desc = result_desc.replace(x, function (FullMatch, n) {
                            return '<span class="highlight">' + FullMatch + '</span>'
                        });
                    }
                    if (result_descLoc != null) {
                        result_descLoc = result_descLoc.replace(x, function (FullMatch, n) {
                            return '<span class="highlight">' + FullMatch + '</span>'
                        });
                    }
                    return $("<li></li>")
                        .data("ui-autocomplete-item", item)
                        .append("<a>" + result_item + "<BR/> <span style='color:Gray;'>" + result_desc + "</span><BR/> <span style='color:Gray;'>" + result_descLoc + "</span></a>")
                        .appendTo(ul);
                };

            //Test
            $("#<%= txtUnit.ClientID %>").keyup(function (event) {
                $('.ui-helper-hidden-accessible').hide();
            });

            $("#<%= txtJob.ClientID %>").autocomplete({
                source: function (request, response) {
                    <%-- if ($("#<%=hdnaccount.ClientID%>").val() != '') {--%>
                    var asdas = new locc();
                    asdas.SearchValue = $("#<%=hdnaccount.ClientID%>").val();
                

                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "CustomerAuto.asmx/GetJobsByLocation",
                        data: JSON.stringify(asdas),
                        dataType: "json",
                        async: true,
                        success: function (data) {                          
                            response($.parseJSON(data.d));
                        },
                        error: function (result) { debugger }
                    });
                    //}
                    //else {
                    //    var objdataEmpty = new dataEmpty();
                    //    response(objdataEmpty);
                    //}
                },
                select: function (event, ui) {
                    $("#<%= txtJob.ClientID %>").val(ui.item.ID + ", Project: " + ui.item.fDesc);
                    $("#<%=hdnjob.ClientID%>").val(ui.item.ID);

                    return false;
                },
                focus: function (event, ui) {
                    //$("#<%= txtJob.ClientID %>").val(ui.item.label);
                    return false;
                },
                minLength: 0,
                delay: 250
            })
                .data("ui-autocomplete")._renderItem = function (ul, item) {
                    //debugger;
                    var result_item = item.fDesc;
                    if (item.ID != "") {
                        result_item = item.ID + ", Project: " + item.fDesc;
                    }                   

                    return $("<li></li>")
                        .data("ui-autocomplete-item", item)
                        .append("<a> <span style='color:Gray;'>" + result_item + "</span><BR/></a>")
                        .appendTo(ul);
                };

            $("#<%= txtJob.ClientID %>").keyup(function (event) {
                $('.ui-helper-hidden-accessible').hide();
            });
            //Ammount
            $("#<%=txtAmount.ClientID%>").blur(function () {
                $(this).formatCurrency();
            });
            //Override Ammount
            $("#<%=txtOverrideAmount.ClientID%>").blur(function () {
                $(this).formatCurrency();
            });

            //Additional Amount
            $("#<%=txtOverrideAmount.ClientID%>").blur(function () {               
                 $(this).formatCurrency();
            }); 
            $("#<%=txtOverrideAmount.ClientID%>").formatCurrency();
            $("#<%=txtAmount.ClientID%>").formatCurrency();           
             
            $("#<%=txtOverrideAmount.ClientID%>").formatCurrency();
        });
       
         function SelectRowmailPage(mail, lnkMail) {


            var maillink = document.getElementById(lnkMail);
            var mailid = document.getElementById(mail);


            maillink.href = 'mailto:' + mailid.innerHTML;

        }
        
        function OnClientItemChecked(sender, args ) {           
           
            var  id=sender.get_attributes().getAttribute("hdSelectTeam")
            var obj = document.getElementById(id);
            var lsUser = '';
           
            var string = "";
            var lsItem = "";
           for (var i = 0; i < sender.get_checkedItems().length; i++) {
                string = string + sender.get_checkedItems()[i].get_text() + ", ";
                 lsItem = lsItem + sender.get_checkedItems()[i].get_value() + ","; 
            }
             obj.value = lsItem.slice(0,-1);
            sender.set_text(string.slice(0,-2));
            console.log(string);
   
        }
        
        function OnClientLoadValue(sender, args ) {
            
            var string = "";
           for (var i = 0; i < sender.get_checkedItems().length; i++) {
                string = string + sender.get_checkedItems()[i].get_text() + ", ";
               
            }            
            sender.set_text(string.slice(0,-2));  
        }
       
          function UpdateItemCountField(sender, args) {
                //Set the footer text.
                sender.get_dropDownElement().lastChild.innerHTML = "A total of " + sender.get_items().get_count() + " items";
        }
        

     
    </script>
    
    <style>
        .ui-autocomplete {
            max-height: 300px;
            overflow-y: auto; /* prevent horizontal scrollbar */
            overflow-x: hidden; /* add padding to account for vertical scrollbar */
            z-index: 1000 !important;
        }

        .ModalPopupBG {
            background-color: black;
            filter: alpha(opacity=50);
            opacity: 0.7;
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
        .w-25{
           width: 23.5%;
        }
    
        @media only screen and (min-width: 250px) and (max-width: 700px) {
            .w-25{
             width: 100%;
             padding-right: 3%;
             }
            .s11{
                width:100%
            }
        }
    </style>

         
    <script>
       
      <%-- function aceEmail_itemSelected(sender, e) {           
            var hdnAcctID = document.getElementById('<%= hdnAlterEmail.ClientID %>');
            hdnAcctID.value = e.get_value();
        }--%>
    </script>
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
        <img src="images/wheel.GIF" alt="Be patient..." class="lodder" />
    </div>
    <telerik:RadAjaxManager ID="RadAjaxManager_Test" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="btnSubmit">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_PriceHistory" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="ddlYear" />
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_gvLogs" />
                    <telerik:AjaxUpdatedControl ControlID="lnkAssign" />
                    <telerik:AjaxUpdatedControl ControlID="lnkAddPrice" />
                    <telerik:AjaxUpdatedControl ControlID="lnkEditPrice" />
                    <telerik:AjaxUpdatedControl ControlID="lnkDeletePrice" />
                    <telerik:AjaxUpdatedControl ControlID="lnkGeneralProposal" />
                    <telerik:AjaxUpdatedControl ControlID="lnkAddSchedule" />
                    <telerik:AjaxUpdatedControl ControlID="lnkEditSchedule" />
                    <telerik:AjaxUpdatedControl ControlID="lnkDeleteSchedule" />
                    <telerik:AjaxUpdatedControl ControlID="lnkDeleteSchedule" />
                    <telerik:AjaxUpdatedControl ControlID="divSuccess" />
                </UpdatedControls>
            </telerik:AjaxSetting>
           
            <telerik:AjaxSetting AjaxControlID="btnDelete">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_Contacts" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="lnkContactSave">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_Contacts" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
             <telerik:AjaxSetting AjaxControlID="btnReloadContact">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_Contacts" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
              <telerik:AjaxSetting AjaxControlID="lnkApproveProposal">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="gvDocuments" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="lnkDeleteForms">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="gvDocuments" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnSendEmail">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="gvDocuments" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="lnkScheduleSave">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="gvSchedule" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="txtSchedule"  />
                    <telerik:AjaxUpdatedControl ControlID="txtWorker"  />
                </UpdatedControls>
            </telerik:AjaxSetting>
             <telerik:AjaxSetting AjaxControlID="lnkDeleteSchedule">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="gvSchedule" LoadingPanelID="RadAjaxLoadingPanel1" />
                     <telerik:AjaxUpdatedControl ControlID="txtSchedule"  />
                    <telerik:AjaxUpdatedControl ControlID="txtWorker"  />
                </UpdatedControls>
            </telerik:AjaxSetting> 
            <telerik:AjaxSetting AjaxControlID="lnkPriceSave">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_PriceHistory" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
             <telerik:AjaxSetting AjaxControlID="lnkDeletePrice">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_PriceHistory" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
      <!-- ===================================================== START POPUP CODE ================================================-->
    <telerik:RadWindowManager ID="RadWindowManagerContact" runat="server">
        <Windows>
            <telerik:RadWindow ID="RadWindowContact" Skin="Material" VisibleTitlebar="true" Title="Add Contact" Behaviors="Default" CenterIfModal="true"
                Animation="FlyIn" AnimationDuration="200" RenderMode="Auto" VisibleStatusbar="false" MinWidth="450"
                runat="server" Modal="true" Width="800" Height="400">
                <ContentTemplate>
                    <div>
                        <div class="form-section-row">
                            <div class="form-section3">
                                <div class="input-field col s12">
                                    <div class="row">
                                         <asp:HiddenField ID="hdnIndex" runat="server" ></asp:HiddenField>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ControlToValidate="txtContcName"
                                            Display="None" ErrorMessage="Contact Name Required" SetFocusOnError="True" ValidationGroup="cont">
                                        </asp:RequiredFieldValidator>
                                        <asp:ValidatorCalloutExtender ID="RequiredFieldValidator12_ValidatorCalloutExtender"
                                            runat="server" Enabled="True" PopupPosition="left" TargetControlID="RequiredFieldValidator12">
                                        </asp:ValidatorCalloutExtender>
                                        <asp:TextBox ID="txtContcName" runat="server" CssClass="Contact-search" AutoCompleteType="Disabled" MaxLength="50"></asp:TextBox>
                                        <asp:Label runat="server" AssociatedControlID="txtContcName">Contact Name</asp:Label>
                                    </div>
                                </div>
                            </div>
                            <div class="form-section3-blank">
                                &nbsp;
                            </div>
                            <div class="form-section3">
                                <div class="input-field col s12">
                                    <div class="row">
                                        <asp:TextBox ID="txtTitle" runat="server" MaxLength="50"></asp:TextBox>
                                        <asp:Label runat="server" AssociatedControlID="txtTitle">Title</asp:Label>
                                    </div>
                                </div>

                            </div>
                            <div class="form-section3-blank">
                                &nbsp;
                            </div>
                            <div class="form-section3">
                                <div class="input-field col s12">
                                    <div class="row">
                                        <asp:TextBox ID="txtContPhone" runat="server" AutoCompleteType="Disabled" MaxLength="22"></asp:TextBox>
                                        <asp:Label runat="server" AssociatedControlID="txtContPhone">Phone</asp:Label>
                                    </div>
                                </div>

                            </div>

                        </div>
                        <div class="form-section-row">
                            <div class="form-section3">
                                <div class="input-field col s12">
                                    <div class="row">
                                        <asp:TextBox ID="txtContFax" runat="server" AutoCompleteType="Disabled" MaxLength="22"></asp:TextBox>
                                        <asp:Label runat="server" AssociatedControlID="txtContFax">Fax</asp:Label>
                                    </div>
                                </div>

                            </div>
                            <div class="form-section3-blank">
                                &nbsp;
                            </div>
                            <div class="form-section3">
                                <div class="input-field col s12">
                                    <div class="row">
                                        <asp:TextBox ID="txtContCell" runat="server" AutoCompleteType="Disabled" CssClass="form-control" MaxLength="22"></asp:TextBox>
                                        <asp:Label runat="server" AssociatedControlID="txtContCell">Cell</asp:Label>
                                    </div>
                                </div>

                            </div>
                            <div class="form-section3-blank">
                                &nbsp;
                            </div>
                            <div class="form-section3">
                                <div class="input-field col s12">
                                    <div class="row">
                                        <asp:TextBox ID="txtContEmail" runat="server" CssClass="form-control" AutoCompleteType="Disabled" MaxLength="50"></asp:TextBox>
                                        <asp:Label runat="server" AssociatedControlID="txtContEmail">Email</asp:Label>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtContEmail"
                                            Display="None" ErrorMessage="Invalid Email" ValidationGroup="cont" SetFocusOnError="True"
                                            ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*">
                                        </asp:RegularExpressionValidator>
                                        <asp:ValidatorCalloutExtender ID="RegularExpressionValidator1_ValidatorCalloutExtender"
                                            runat="server" Enabled="True" TargetControlID="RegularExpressionValidator1">
                                        </asp:ValidatorCalloutExtender>
                                    </div>
                                </div>

                            </div>
                            <div class="form-section3-blank">
                                &nbsp;
                            </div>


                            <div class="row">
                                <div class="section-ttle">Email</div>
                            </div>
                            <div class="form-section3-blank">
                                &nbsp;
                            </div>
                        </div>
                        <div class="form-section-row">
                            <div class="form-section4">
                                <div class="input-field col s12">
                                    <div class="row">
                                        <div class="checkrow">
                                            <asp:CheckBox ID="chkEmailTicket" runat="server" class="filled-in" />
                                            <label for="chkEmailTicket">Ticket</label>
                                        </div>
                                    </div>
                                </div>

                            </div>
                            <div class="form-section4">
                                <div class="input-field col s12">
                                    <div class="row">
                                        <div class="checkrow">
                                            <asp:CheckBox ID="chkEmailInvoice" runat="server" class="filled-in" />
                                            <label for="chkEmailInvoice">Invoice/Statements</label>
                                        </div>
                                    </div>
                                </div>

                            </div>
                            <div class="form-section4">
                                <div class="input-field col s12">
                                    <div class="row">
                                        <div class="checkrow">
                                            <asp:CheckBox ID="chkShutdownAlert" runat="server" class="filled-in" />
                                            <label for="chkShutdownAlert" >Shutdown Alerts</label>
                                        </div>
                                    </div>
                                </div>

                            </div>
                            <div class="form-section4">
                                <div class="input-field col s12">
                                    <div class="row">
                                        <div class="checkrow">
                                            <asp:CheckBox ID="chkTestProposals" runat="server" class="filled-in" />
                                            <label for="chkTestProposals" >Test Proposals</label>
                                        </div>
                                    </div>
                                </div>

                            </div>
                        </div>

                        <div style="clear: both;"></div>

                        <div class="btnlinks">
                            <asp:LinkButton ID="lnkContactSave" runat="server" OnClick="lnkContactSave_Click" ValidationGroup="cont">Save</asp:LinkButton>
                        </div>
                    </div>
                </ContentTemplate>
            </telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>
    <!-- ===================================================== END POPUP CODE ================================================-->
        <telerik:RadWindow ID="RadWindowSchedule" Skin="Material" VisibleTitlebar="true" Title="Add Schedule" Behaviors="Default" CenterIfModal="true"
                Animation="FlyIn" AnimationDuration="200" RenderMode="Auto" VisibleStatusbar="false" MinWidth="450"
                runat="server" Modal="true" Width="400" Height="400">
                <ContentTemplate>
                    <div class="mt-10" >
                        <div class="form-section-row">
                            <div class="form-section">
                                <div class="input-field col s12">
                                    <div class="row">
                                        <asp:HiddenField ID="hdnID" runat="server" Value="0"></asp:HiddenField>
                                        <asp:TextBox ID="txtScheduleYear" runat="server" AutoCompleteType="Disabled" MaxLength="50"></asp:TextBox>
                                        <asp:Label runat="server" AssociatedControlID="txtScheduleYear">Schedule Year</asp:Label>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="form-section-row">
                            <div class="form-section">

                                <div class="input-field col s12">
                                    <div class="row">

                                        <asp:TextBox ID="txtScheduleDate" runat="server" AutoCompleteType="Disabled" MaxLength="50"></asp:TextBox>
                                        <asp:Label runat="server" AssociatedControlID="txtScheduleDate">Schedule Date</asp:Label>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="form-section-row">
                            <div class="form-section">
                                <div class="input-field col s12">
                                    <div class="row">

                                        <%--<asp:TextBox ID="txtScheduleWorker" runat="server" AutoCompleteType="Disabled" MaxLength="50"></asp:TextBox>--%>
                                          <asp:HiddenField ID="auto_hdnScheduleWorker" runat="server" />
                                          <asp:TextBox ID="txtScheduleWorker" runat="server" onkeyup="EmptyValue(this);"
                                        autocomplete="off" placeholder="Search by name"></asp:TextBox>
                                    <asp:AutoCompleteExtender runat="server" Enabled="True" ServicePath="CustomerAuto.asmx" TargetControlID="txtScheduleWorker"
                                        EnableCaching="False" ServiceMethod="ServiceGetWorkers" UseContextKey="True" MinimumPrefixLength="0"
                                        CompletionListCssClass="autocomplete_completionListElement"
                                        CompletionListItemCssClass="autocomplete_listItem"
                                        CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                        CompletionListElementID="ListTestType"
                                        OnClientItemSelected="aceTestType_itemSelected"
                                        ID="TestTypeAutoCompleteExtender" DelimiterCharacters="" CompletionInterval="250">
                                    </asp:AutoCompleteExtender>
                                    <div id="ListTestType"></div>
                                        <asp:Label runat="server" AssociatedControlID="txtScheduleWorker">Worker</asp:Label>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="form-section-row">
                            <div class="form-section">
                                <div class="input-field col s12">
                                    <div class="row">
                                          <label class="drpdwn-label">Status</label>
                                        <asp:DropDownList ID="ddlScheduleStatus" runat="server" AutoCompleteType="Disabled" MaxLength="50"  CssClass="browser-default">
                                            <asp:ListItem Value="0" Text="Pending"> </asp:ListItem>
                                                <asp:ListItem Value="1" Text="Notified"> </asp:ListItem>
                                                <asp:ListItem Value="2" Text="Accepted"> </asp:ListItem>
                                                <asp:ListItem Value="3" Text="Cancelled"> </asp:ListItem>                                            
                                        </asp:DropDownList>
                                         
                                      
                                    </div>
                                </div>
                            </div>
                        </div>
                          

                        <div style="clear: both;"></div>

                        <div class="btnlinks">
                            <asp:LinkButton ID="lnkScheduleSave" runat="server" OnClientClick="return ValidateSchedule();" OnClick="lnkScheduleSave_Click" >Save</asp:LinkButton>
                        </div>
                    </div>
                </ContentTemplate>
            </telerik:RadWindow>
     <telerik:RadWindow ID="RadWindowPrice" Skin="Material" VisibleTitlebar="true" Title="Add Price" Behaviors="Default" CenterIfModal="true"
                Animation="FlyIn" AnimationDuration="200" RenderMode="Auto" VisibleStatusbar="false" MinWidth="450"
                runat="server" Modal="true" Width="600" Height="400">
                <ContentTemplate>
                    <div class="mt-10">
                        <div class="form-section-row">
                            <div class="form-section">
                                <div class="input-field col s12">
                                    <div class="row">
                                        <asp:TextBox ID="txtPriceYear" runat="server"></asp:TextBox>
                                        <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="txtPriceYear"
                                            ErrorMessage="Please enter year." Display="None"
                                            ValidationGroup="Price">
                                        </asp:RequiredFieldValidator>
                                        <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender2" runat="server" Enabled="True" PopupPosition="BottomLeft"
                                            TargetControlID="RequiredFieldValidator1" />

                                        <asp:Label runat="server" AssociatedControlID="txtPriceYear">Year</asp:Label>
                                    </div>
                                </div>
                            </div>
                        </div>                      

                        <div class="form-section-row">
                            <div class="form-section2">
                                 <label for="chkChargeablePriceYear" class="drpdwn-label">Chargeable</label><br />
                                        <asp:CheckBox ID="chkChargeablePriceYear" CssClass="css-checkbox" Text="&nbsp;" runat="server" onclick="updatePriceDetail();"/>
                            </div>
                            <div class="form-section2-blank">&nbsp;</div>
                             <div class="form-section2">
                                   <label for="chkThirdPartyYear" class="drpdwn-label">Third Party Witness</label><br />
                                        <asp:CheckBox ID="chkThirdPartyYear" CssClass="css-checkbox" Text="&nbsp;" runat="server"/>
                            </div>
                        </div>

                         <div class="form-section-row">
                            <div class="form-section2">
                                 <asp:Label runat="server" AssociatedControlID="txtDefaultAmountYear">Default Amount</asp:Label>
                              <asp:TextBox ID="txtDefaultAmountYear" runat="server" MaxLength="50"></asp:TextBox>
                                       
                            </div>
                            <div class="form-section2-blank">&nbsp;</div>
                             <div class="form-section2">
                                     <asp:Label runat="server" AssociatedControlID="txtThirdPartyNameYear">Third Party Name</asp:Label>
                                   <asp:TextBox ID="txtThirdPartyNameYear" runat="server" MaxLength="50"></asp:TextBox>
                                    
                            </div>
                        </div>
                         <div class="form-section-row">
                            <div class="form-section2">
                                  <asp:Label runat="server" AssociatedControlID="txtOverrideAmountYear">Override Amount</asp:Label>
                                <asp:TextBox ID="txtOverrideAmountYear" runat="server"  MaxLength="50"></asp:TextBox>
                                      
                            </div>
                            <div class="form-section2-blank">&nbsp;</div>
                             <div class="form-section2">
                                  <asp:Label runat="server" AssociatedControlID="txtThirdPartyPhoneYear">Third Party Phone</asp:Label>
                                    <asp:TextBox ID="txtThirdPartyPhoneYear" runat="server"  MaxLength="50"></asp:TextBox>
                                       
                            </div>
                        </div>

                    
                        <div style="clear: both;"></div>

                        <div class="btnlinks">
                        
                            <asp:LinkButton ID="lnkPriceYearSave" runat="server" OnClientClick="ShowConfirmUpdatePriceMessage(); return false; " >Save</asp:LinkButton>
                                                    <asp:Button ID="lnkPriceSave" runat="server" Text="Assign" Style="display: none;" OnClick="lnkPriceSave_Click" ValidationGroup="Price" />
                        </div>
                    </div>
                </ContentTemplate>
            </telerik:RadWindow>
    <telerik:RadWindow ID="RadWindowTemplate" Skin="Material" VisibleTitlebar="true" Title="Choose Template" Behaviors="Default" CenterIfModal="true"
        Animation="FlyIn" AnimationDuration="200" RenderMode="Auto" VisibleStatusbar="false" MinWidth="450"
        runat="server" Modal="true" Width="800" Height="500">
        <ContentTemplate>
            <div class="form-section-row mt-10" >
                <div class="RadGrid RadGrid_Material  FormGrid">
                    <telerik:RadCodeBlock ID="RadCodeBlock2" runat="server">
                        <script type="text/javascript">
                            function pageLoad() {
                                var grid = $find("<%= gvFormTemplate.ClientID %>");
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
                    <div class="row">
                        <div class="input-field col s5">
                            <div class="row">
                                <label class="drpdwn-label">Proposal Year</label>
                                <asp:DropDownList ID="ddlYear" runat="server" CssClass="browser-default" TabIndex="1"></asp:DropDownList>


                            </div>
                        </div>
                    </div>

                    <div class="RadGrid RadGrid_Material">
                        <telerik:RadGrid RenderMode="Auto" ID="gvFormTemplate" AllowFilteringByColumn="true" ShowFooter="True" PageSize="50"
                            OnNeedDataSource="gvFormTemplate_NeedDataSource" PagerStyle-AlwaysVisible="true"
                            ShowStatusBar="true" runat="server" AllowPaging="True" AllowSorting="true" Width="100%" AllowCustomPaging="True">
                            <CommandItemStyle />
                            <GroupingSettings CaseSensitive="false" />
                            <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                                <Selecting AllowRowSelect="True"></Selecting>
                                <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                            </ClientSettings>
                            <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="True" ShowFooter="True">
                                <Columns>
                                    <telerik:GridClientSelectColumn UniqueName="chkSelect" HeaderStyle-Width="28">
                                    </telerik:GridClientSelectColumn>

                                    <telerik:GridTemplateColumn HeaderText="Name" UniqueName="ID" DataField="FileName" ShowFilterIcon="false" HeaderStyle-Width="100">
                                        <ItemTemplate>
                                            <asp:Label ID="lblName" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                                            <asp:HiddenField ID="hdnFormID" Value='<%# Eval("ID")%>' runat="server" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="File Name" DataField="FileName" ShowFilterIcon="false" HeaderStyle-Width="200">
                                        <ItemTemplate>
                                            <asp:Label ID="lblFileName" runat="server" Text='<%# Eval("FileName") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn HeaderText="Added By" DataField="AddedBy" ShowFilterIcon="false" HeaderStyle-Width="100">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAddedBy" runat="server" Text='<%# Eval("AddedBy") %>'></asp:Label>

                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn HeaderText="AddedOn" DataField="AddedOn" ShowFilterIcon="false" HeaderStyle-Width="100">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAddedOn" Text='<%# Eval("AddedOn", "{0:MM/dd/yyyy hh:mm}")%>' runat="server" CssClass="breakContent" />

                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    </telerik:GridTemplateColumn>

                                </Columns>
                            </MasterTableView>
                            <SelectedItemStyle CssClass="selectedrowcolor" />
                            <FilterMenu CssClass="RadFilterMenu_CheckList">
                            </FilterMenu>
                        </telerik:RadGrid>
                    </div>

                </div>
            </div>
            <div class="mt-10">
                <div class="btnlinks">
                    <asp:LinkButton ID="btnGenerateTemplete" runat="server" OnClick="btnGenerateTemplete_Click" Text="Generate Template" OnClientClick="return checkSelectTemplate();"></asp:LinkButton>
                </div>
            </div>
        </ContentTemplate>
    </telerik:RadWindow>


      <telerik:RadWindow ID="TeamMembersWindow" Skin="Material" VisibleTitlebar="true" Behaviors="Default" CenterIfModal="true"
        Animation="FlyIn" AnimationDuration="200" RenderMode="Auto" VisibleStatusbar="false"
        runat="server" Modal="true" Width="1050" Height="635">
        <ContentTemplate>
            <telerik:RadAjaxPanel ID="RadAjaxPanel32" runat="server">
                <div class="margin-tp">
                    <div class="form-section-row">
                        <div class="form-section">
                            <div class="row mb" >
                                <div class="grid_container" id="divMemberGrid" runat="server">
                                    <div class="RadGrid RadGrid_Material RadGrid RadGrid_Popup">

                                        <telerik:RadGrid RenderMode="Auto" ID="RadGrid_Emails" AllowFilteringByColumn="true" ShowFooter="false" PageSize="1000"
                                            ShowStatusBar="true" runat="server" AllowPaging="false" AllowSorting="true" PagerStyle-AlwaysVisible="true" FilterType="CheckList"
                                            AllowCustomPaging="false" Width="100%" Height="516px" OnPreRender="RadGrid_Emails_PreRender"
                                            OnNeedDataSource="RadGrid_Emails_NeedDataSource">
                                            <CommandItemStyle />
                                            <GroupingSettings CaseSensitive="false" />
                                            <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                                                <Selecting AllowRowSelect="True"></Selecting>

                                                <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                                <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                            </ClientSettings>
                                            <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="True" >
                                                <Columns>
                                                    <telerik:GridTemplateColumn AllowFiltering="false" HeaderStyle-Width="28" ShowFilterIcon="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblID" runat="server" Style="display: none;"><%#Eval("memberkey")%></asp:Label>
                                                            <asp:CheckBox ID="chkSelect" runat="server" />
                                                        </ItemTemplate>
                                                        <HeaderTemplate>
                                                            <asp:CheckBox ID="chkAll" runat="server" />
                                                        </HeaderTemplate>
                                                        <ItemStyle Width="0px"></ItemStyle>
                                                    </telerik:GridTemplateColumn>
                                                  
                                                    <telerik:GridTemplateColumn
                                                        DataField="fUser" SortExpression="fUser" AutoPostBackOnFilter="true" DataType="System.String"
                                                        CurrentFilterFunction="Contains" HeaderText="User Name" ShowFilterIcon="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblUserName" runat="server"><%#Eval("fUser")%></asp:Label>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn
                                                        DataField="RoleName" SortExpression="RoleName" AutoPostBackOnFilter="true"
                                                        CurrentFilterFunction="Contains" HeaderText="User Role" ShowFilterIcon="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblRoleName" runat="server"><%#Eval("RoleName")%></asp:Label>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                     <telerik:GridTemplateColumn SortExpression="IsTask" AllowFiltering="false" HeaderStyle-Width="100" ShowFilterIcon="false" DataField="IsTask" HeaderText="Task" >
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkTask" Checked='<%# (Convert.ToString(Eval("IsTask")) == "1") ? true : false %>' runat="server" />
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn
                                                        DataField="email" SortExpression="email" AutoPostBackOnFilter="true"
                                                        CurrentFilterFunction="Contains" DataType="System.String" HeaderText="Email" ShowFilterIcon="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblEmail" runat="server"><%#Eval("email")%></asp:Label>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>

                                                    <telerik:GridTemplateColumn
                                                        DataField="usertype" SortExpression="usertype" AutoPostBackOnFilter="true"
                                                        CurrentFilterFunction="Contains" HeaderText="Type" ShowFilterIcon="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblType" runat="server"><%#Eval("usertype")%></asp:Label>
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
                             <div class="btnlinks">
                            <a id="lnkPopupOK" onclick="CloseTeamMemberWindow();" style="cursor: pointer;">OK</a>
                           
                        </div>
                        </div>
                    </div>
                  
                  
                </div>
            </telerik:RadAjaxPanel>
        </ContentTemplate>
    </telerik:RadWindow>

 
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server">
    </telerik:RadAjaxLoadingPanel>
    <div class="divbutton-container">
        <div id="divButtons">
            <div id="breadcrumbs-wrapper">
                <header>
                    <div class="container row-color-grey">
                        <div class="row">
                             <div class="alert alert-warning" runat="server" id="divSuccess" style="margin-left:10px;">
                            <button type="button" class="close" data-dismiss="alert" style="float:left; margin-right:10px;">×</button>
                         
                                 <label id="lblExistTestMsg" runat="server"  style="font-size:14px; color:red; font-weight:bolder">  This test has not initial data for this year yet. </label>
                        </div>
                            <div style="clear:both;"></div>
                            <div class="col s12 m12 l12">
                                <div class="row">
                                    <div class="page-title"><i class="mdi-communication-contacts"></i>&nbsp;<asp:Label runat="server" ID="lblHeader">Add New Test</asp:Label></div>
                                    <div class="buttonContainer">
                                        <div class="btnlinks">
                                            <asp:LinkButton ID="lnkSave" runat="server" OnClientClick="ShowConfirmSaveThirdpartyMessage(); return false; " >Save</asp:LinkButton>
                                            <asp:Button Text="Save" ID="btnSubmit" runat="server" TabIndex="38" OnClick="btnSubmit_Click" ValidationGroup="test" Style="display: none;"></asp:Button>
                                        </div>
                                        <div class="btnlinks">
                                             <asp:LinkButton ID="lnkAssign" runat="server" OnClientClick="ShowConfirmAssignMessage(); return false; " >Assign</asp:LinkButton>
                                                    <asp:Button ID="lnkAssignTicket" runat="server" Text="Assign" Style="display: none;" OnClick="lnkAssignTicket_Click" CausesValidation="False" />
                        
                                           
                                        </div>
                                       

                                    </div>
                                    <div class="btnclosewrap">
                                        <asp:LinkButton CssClass="icon-closed" ID="lnkClose" runat="server" CausesValidation="false" ToolTip="Close"
                                            TabIndex="39" OnClick="lnkClose_Click"><i class="mdi-content-clear"></i></asp:LinkButton>
                                    </div>
                                    
                                    <div class="rght-content">
                                        <div class="editlabel">
                                            <asp:Label runat="server" ID="lblUnitTestName"></asp:Label>                                           
                                        </div>                                        
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
                            <div class="tblnks">
                               <ul class="anchor-links">	                                   
                                     <li runat="server" id="liCustom"><a href="#accrdcustom" id="defaultCustom">Custom</a></li>
                                     <li runat="server" id="liContacts"><a href="#accrdcontacts" id="defaultContact">Contacts</a></li>
                                  <%--  <li runat="server" id="liViewHistory"><a href="#accrdViewHistory">View History</a></li>--%>
                                    <li runat="server" id="liSchedule"><a href="#accrdSchedule" >Schedule</a></li>  
                                    <li  runat="server"  id="liPriceHistory"><a href="#accrdPriceHistory">Price History</a></li>
                                    <li runat="server" id="liDocument"><a href="#accrdProposal" >Proposal</a></li> 
                                                                     
                                   <li runat="server" id="liLogs"><a href="#accrdLogs" >Logs</a></li>
					            </ul>
                            </div>
                            <div class="tblnksright">
                                <div class="nextprev">
                                    <asp:Panel ID="pnlSave" runat="server">
                                        <asp:Panel ID="pnlNext" runat="server">
                                            <span class="angleicons">
                                                <asp:LinkButton ID="lnkFirst" ToolTip="First" runat="server" CausesValidation="False" OnClick="lnkFirst_Click">
                                                        <i class="fa fa-angle-double-left"></i>
                                                </asp:LinkButton>
                                            </span>
                                            <span class="angleicons">
                                                <asp:LinkButton ID="lnkPrevious" ToolTip="Previous" runat="server" CausesValidation="False" OnClick="lnkPrevious_Click">
                                                        <i class="fa fa-angle-left"></i>
                                                </asp:LinkButton>
                                            </span>
                                            <span class="angleicons">
                                                <asp:LinkButton ID="lnkNext" ToolTip="Next" runat="server" CausesValidation="False" OnClick="lnkNext_Click">
                                                        <i class="fa fa-angle-right"></i>
                                                </asp:LinkButton>
                                            </span>
                                            <span class="angleicons">
                                                <asp:LinkButton ID="lnkLast" ToolTip="Last" runat="server" CssClass="icon-last" CausesValidation="False" OnClick="lnkLast_Click">
                                                        <i class="fa fa-angle-double-right"></i>
                                                </asp:LinkButton>
                                            </span>
                                        </asp:Panel>
                                    </asp:Panel>
                                </div>
                            </div>
                        </div>
                    </div>

                </div>
            </div>
        </div>
    </div>
     
    <div class="container ">
        <div class="row">
            <div class="col s12 m12 l12">
                <div class="row">
                    <div class="card">
                        <div class="card-content">
                            <div class="form-content-wrap formpdtop2">                               
                                    <div class="form-content-pd">
                                        <div class="form-section-row">
                                            <div class="col s12 m12 l12 p-r-0">
                                                <div class="row">
                                                    <div class="form-section-row">
                                                        <%--Equipment Infor--%>
                                                        <div class="form-section3 w-25" >
                                                            <div class="section-ttle">Equipment Info</div>
                                                            <div class="input-field col s5">
                                                                <div class="row">
                                                                    <label class="drpdwn-label">Test Type</label>
                                                                    <asp:DropDownList ID="ddlTestTypes" runat="server" CssClass="browser-default" TabIndex="1"></asp:DropDownList>
                                                                    <asp:RequiredFieldValidator runat="server" ID="rftesttype" InitialValue="0" ErrorMessage="Test Type Required."
                                                                        Display="None" ControlToValidate="ddlTestTypes" ValidationGroup="test" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                    <asp:ValidatorCalloutExtender ID="vceVendor" runat="server" Enabled="True"
                                                                        TargetControlID="rftesttype" />
                                                                    <asp:HiddenField ID="hdnTestType" runat="server" />
                                                                    <asp:HiddenField ID="hdnTestTypeFrequency" runat="server" />
                                                                      <asp:HiddenField ID="hdnIsTicketCoveredByTestType" runat="server" />
                                                                </div>
                                                            </div>
                                                            <div class="input-field col s2">
                                                                <div class="row">
                                                                    &nbsp;
                                                                </div>
                                                            </div>
                                                            <div class="input-field col s5">
                                                                <div class="row">
                                                                    <label class="drpdwn-label">Test Status</label>
                                                                    <asp:DropDownList ID="ddlStatus" runat="server" CssClass="browser-default"
                                                                        TabIndex="8">
                                                                    </asp:DropDownList>
                                                                </div>
                                                            </div>
                                                            <div class="input-field col s12">
                                                                <div class="row">
                                                                    <label for="loc">Location</label>
                                                                    <asp:TextBox ID="txtAccount" runat="server" CssClass="pd-negate searchinputloc ui-autocomplete-input"
                                                                        TabIndex="2" autocomplete="off" placeholder="Search by location name, phone#, address etc."></asp:TextBox>
                                                                    <asp:RequiredFieldValidator ID="rfvaccount"
                                                                        runat="server" ControlToValidate="txtAccount" Display="None" ErrorMessage="Account to is Required" ValidationGroup="test"
                                                                        SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                    <asp:ValidatorCalloutExtender ID="vceaccount" runat="server" Enabled="True"
                                                                        PopupPosition="Right" TargetControlID="rfvaccount" />
                                                                    <asp:HiddenField ID="hdnaccount" runat="server" />
                                                                    <asp:HiddenField ID="hdnRolId" runat="server" />
                                                                    <asp:HiddenField ID="hdnNumberOfTestNoTicketInLoc" runat="server" />
                                                                </div>
                                                            </div>
                                                            <div class="input-field col s5">
                                                                <div class="row">
                                                                   <label for="unit" onclick="RedirectToEquipment()" style="cursor:pointer;text-decoration:underline">Unit Number</label>
                                                                    <asp:TextBox ID="txtUnit" runat="server" CssClass="pd-negate">
                                                                    </asp:TextBox>
                                                                    <asp:CustomValidator ID="CustomValidator1" runat="server" ControlToValidate="txtUnit"
                                                                        ErrorMessage="Unit is Required" ClientValidationFunction="ChkUnit"
                                                                        Display="None" SetFocusOnError="True" ValidationGroup="test"></asp:CustomValidator>
                                                                    <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender3" runat="server" Enabled="True"
                                                                        PopupPosition="Right" TargetControlID="CustomValidator1">
                                                                    </asp:ValidatorCalloutExtender>

                                                                    <asp:RequiredFieldValidator ID="rfvUnit"
                                                                        runat="server" ControlToValidate="txtUnit" Display="None" ErrorMessage="Unit is Required" ValidationGroup="test"
                                                                        SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                    <asp:ValidatorCalloutExtender ID="vceUnit" runat="server" Enabled="True"
                                                                        PopupPosition="Right" TargetControlID="rfvUnit" />
                                                                    <asp:HiddenField ID="hdnEquipment" runat="server" />
                                                                    
                                                                </div>
                                                            </div>
                                                            <div class="input-field col s2">
                                                                <div class="row">
                                                                    &nbsp;
                                                                </div>
                                                            </div>
                                                            <div class="input-field col s5">
                                                                <div class="row">
                                                                    <label for="unique">Unique No.</label>
                                                                    <asp:TextBox ID="txtstate" runat="server" CssClass="pd-negate" onkeypress="return false;">
                                                                    </asp:TextBox>
                                                                </div>
                                                            </div>
                                                            <div class="input-field col s12">
                                                                <div class="row">
                                                                    <label class="drpdwn-label">Classification</label>
                                                                  <%--  <asp:TextBox ID="txtClassification" runat="server" Text=""> </asp:TextBox>--%>
                                                                    <asp:HiddenField ID="txtEquipmentDesc" runat="server"></asp:HiddenField>

                                                                     <asp:DropDownList ID="ddlClassification" runat="server" CssClass="browser-default">
                                                            </asp:DropDownList>

                                                                </div>
                                                            </div>
                                                              <div class="input-field col s12">
                                                                <div class="row">
                                                                    <label for="loc">Equipment Type</label>
                                                                    <asp:TextBox ID="txtEquipmentType" runat="server" Text=""> </asp:TextBox>
                                                                 
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="form-section3-blank">
                                                            &nbsp;
                                                        </div>
                                                        <%-- Test info--%> <div class="form-section3 w-25" >
                                                            <div class="section-ttle">Test dates</div>
                                                           
                                                            <div class="input-field col s12">
                                                                <div class="row ">
                                                                    <label class="drpdwn-label" for="drpdwnTestDueBy">Test Due By</label>
                                                                    <asp:DropDownList ID="drpdwnTestDueBy" runat="server" CssClass="browser-default">
                                                                        <asp:ListItem Value="0">Last Actual</asp:ListItem>
                                                                        <asp:ListItem Value="1">Last Due</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                    <asp:RequiredFieldValidator runat="server" ID="rfTestDueBy" InitialValue="-1" ErrorMessage="Test Due By Required."
                                                                        Display="None" ControlToValidate="drpdwnTestDueBy" ValidationGroup="test" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                    <asp:ValidatorCalloutExtender ID="vcTestDueBy" runat="server" Enabled="True"
                                                                        TargetControlID="rfTestDueBy" />
                                                                </div>
                                                            </div>
                                                            <div class="input-field col s12">

                                                                <div class="row">
                                                                    <label for="lto">Last Tested On</label>
                                                                    <asp:TextBox ID="txtLasttestdon" runat="server" CssClass="pd-negate datepicker_mom" TabIndex="2"
                                                                        MaxLength="15"></asp:TextBox>
                                                                    <asp:RegularExpressionValidator ID="revDate" ControlToValidate="txtLasttestdon"
                                                                        ValidationExpression="^(0?[1-9]|1[012])[\/\-](0?[1-9]|[12][0-9]|3[01])[\/\-]\d{4}$"
                                                                        runat="server" ErrorMessage="Invalid Date format. Valid Date Format MM/dd/yyyy" Display="None">
                                                                    </asp:RegularExpressionValidator>
                                                                    <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender1" runat="server" Enabled="True" PopupPosition="Right"
                                                                        TargetControlID="revDate" />
                                                                </div>
                                                            </div>
                                                            <div class="input-field col s12">
                                                                <div class="row">
                                                                    <label for="tdd">Next Due Date</label>
                                                                    <asp:TextBox ID="txtTestNextDueDate" runat="server" CssClass="pd-negate datepicker_mom" TabIndex="2"
                                                                        MaxLength="15"></asp:TextBox>
                                                                    <asp:RequiredFieldValidator ID="rfvDate" ValidationGroup="test"
                                                                        runat="server" ControlToValidate="txtTestNextDueDate" Display="None" ErrorMessage="Date is Required"
                                                                        SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                    <asp:ValidatorCalloutExtender ID="vceDate" runat="server" Enabled="True"
                                                                        PopupPosition="Right" TargetControlID="rfvDate" />
                                                                    <asp:RegularExpressionValidator ID="rfvDate1" ControlToValidate="txtTestNextDueDate" ValidationGroup="test"
                                                                        ValidationExpression="^(0?[1-9]|1[012])[\/\-](0?[1-9]|[12][0-9]|3[01])[\/\-]\d{4}$"
                                                                        runat="server" ErrorMessage="Invalid Date format. Valid Date Format 01/12/2001" Display="None">
                                                                    </asp:RegularExpressionValidator>
                                                                    <asp:ValidatorCalloutExtender ID="vceDate1" runat="server" Enabled="True" PopupPosition="Right"
                                                                        TargetControlID="rfvDate1" />
                                                                </div>
                                                            </div>
                                                            <div class="input-field col s12">
                                                                <div class="row">
                                                                    <label for="ldd">Last Due Date</label>
                                                                    <asp:TextBox ID="txtLastDueDate" runat="server" CssClass="pd-negate datepicker_mom" TabIndex="2"></asp:TextBox>
                                                                    <asp:RegularExpressionValidator ID="rfvDueDate1" ControlToValidate="txtLastDueDate" ValidationGroup="test"
                                                                        ValidationExpression="^(0?[1-9]|1[012])[\/\-](0?[1-9]|[12][0-9]|3[01])[\/\-]\d{4}$"
                                                                        runat="server" ErrorMessage="Invalid Date format. Valid Date Format 01/12/2001" Display="None">
                                                                    </asp:RegularExpressionValidator>
                                                                    <asp:ValidatorCalloutExtender ID="vceDueDate1" runat="server" Enabled="True" PopupPosition="Right"
                                                                        TargetControlID="rfvDueDate1" />

                                                                </div>
                                                            </div>

                                                        </div>
                                                      
                                                        <div class="form-section3-blank">
                                                            &nbsp;
                                                        </div>
                                                        <%--Test dates--%>
                                                         <div class="form-section3 w-25" >
                                                            <div class="section-ttle">Test info</div>
                                                             <div class="input-field col s12">
                                                                <div class="row">
                                                                    <label for="project">Project</label>
                                                                    <asp:TextBox ID="txtJob" runat="server">
                                                                    </asp:TextBox>
                                                                    <asp:HiddenField ID="hdnjob" runat="server" />
                                                                </div>
                                                            </div>
                                                            <div class="input-field col s5" style="height: 50px">
                                                                <div class="row">
                                                                    <label for="chkChargeableEdit" class="drpdwn-label">Chargeable</label><br />
                                                                    <asp:CheckBox ID="chkChargeableEdit" CssClass="css-checkbox" Text="&nbsp;" runat="server" onclick="UpdatePrice();" />

                                                                </div>
                                                            </div>
                                                            <div class="input-field col s2">
                                                                <div class="row">
                                                                    &nbsp;
                                                                </div>
                                                            </div>
                                                            <div class="input-field col s5" style="height: 50px">
                                                                <div class="row">
                                                                    <label for="chkThirdParty" class="drpdwn-label">Third Party Witness</label><br />

                                                                    <asp:CheckBox ID="chkThirdParty" CssClass="css-checkbox" Text="&nbsp;" runat="server" />
                                                                </div>
                                                            </div>
                                                            
                                                            <div class="input-field col s5">
                                                                <div class="row">
                                                                      <asp:Label runat="server" ID="lblAmount" AssociatedControlID="txtAmount">Default Amount</asp:Label>
                                                                    <asp:TextBox ID="txtAmount" runat="server"></asp:TextBox>                                                                    

                                                                </div>
                                                            </div>
                                                            <div class="input-field col s2">
                                                                <div class="row">
                                                                    &nbsp;
                                                                </div>
                                                            </div>
                                                            <div class="input-field col s5">
                                                                <div class="row ">
                                                                    <label for="txtThirdPartyName">Third Party Name</label>
                                                                    <asp:TextBox ID="txtThirdPartyName" runat="server" MaxLength="50"></asp:TextBox>
                                                                  
                                                                          <asp:HiddenField ID="hdnThirdPartyName" runat="server" ></asp:HiddenField>
                                                                     
                                                                </div>
                                                            </div>
                                                           <div class="input-field col s5">
                                                                <div class="row">

                                                                    <asp:Label runat="server" ID="lblOverrideAmount" AssociatedControlID="txtOverrideAmount">Override Amount</asp:Label>
                                                                    <asp:TextBox ID="txtOverrideAmount" runat="server"></asp:TextBox>
                                                                </div>
                                                            </div>  
                                                            <div class="input-field col s2">
                                                                <div class="row">
                                                                    &nbsp;
                                                                </div>
                                                            </div>
                                                            <div class="input-field col s5">
                                                                <div class="row">
                                                                    <label for="txtThirdPartyPhone">Third Party Phone</label>
                                                                    <asp:TextBox ID="txtThirdPartyPhone" runat="server" MaxLength="15"></asp:TextBox>
                                                                </div>
                                                            </div>                                                           
                                                            <div class="input-field col s2">
                                                                <div class="row">
                                                                    &nbsp;
                                                                </div>
                                                            </div>
                                                                                                                   

                                                        </div>
                                                        <div class="form-section3-blank">
                                                            &nbsp;
                                                        </div>
                                                        <%--Ticket info--%>
                                                        <div class="form-section3 w-25" >
                                                            <div class="section-ttle">Ticket info</div> 

                                                              <div class="input-field col s5">
                                                                <div class="row" >
                                                                    <label >Ticket#<a href="#" id="lnkTicket" runat="server"></a>
                                                                        
                                                                        </label>
                                                                    
                                                                    <asp:TextBox ID="TextBox1" runat="server" CssClass="pd-negate" onkeypress="return false;" TabIndex="2"
                                                                        MaxLength="50"></asp:TextBox> 

                                                                </div>
                                                            </div>
                                                            <div class="input-field col s2">
                                                                <div class="row">
                                                                    &nbsp;
                                                                </div>
                                                            </div>
                                                            <div class="input-field col s5">
                                                                <div class="row">
                                                                       <label for="lblwho">Entered By</label>
                                                                    <asp:TextBox ID="lblwho" runat="server" CssClass="pd-negate" onkeypress="return false;" TabIndex="2"
                                                                        MaxLength="50"></asp:TextBox>  
                                                                </div>
                                                            </div>





                                                            <div class="input-field col s5">
                                                                <div class="row">
                                                                    <label for="txtTicketStatus">Status</label>
                                                                    <asp:TextBox ID="txtTicketStatus" runat="server" CssClass="pd-negate" TabIndex="2" onkeypress="return false;"
                                                                        MaxLength="50"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                            <div class="input-field col s2">
                                                                <div class="row">
                                                                    &nbsp;
                                                                </div>
                                                            </div>
                                                            <div class="input-field col s5">
                                                                <div class="row">
                                                                       <label for="txtCat">Category</label>
                                                                    <asp:TextBox ID="txtCat" runat="server" CssClass="pd-negate" onkeypress="return false;" TabIndex="2"
                                                                        MaxLength="50"></asp:TextBox>  
                                                                </div>
                                                            </div>
                                                            <div class="input-field col s12">
                                                                <div class="row">
                                                                    <label for="txtWorker" style="margin-top:-10px; top:0px;" >Worker</label>
                                                                    <asp:TextBox ID="txtWorker" runat="server" CssClass="pd-negate" onkeypress="return false;" TabIndex="2"
                                                                        MaxLength="50"></asp:TextBox>
                                                                    <asp:HiddenField ID="hdnWorker" runat="server" />
                                                                    
                                                                </div>
                                                            </div>
                                                            
                                                             <div class="input-field col s12">
                                                                <div class="row">
                                                                     
                                                                    <asp:TextBox ID="txtSchedule" runat="server" CssClass="pd-negate" onkeypress="return false;" TabIndex="2"  
                                                                        MaxLength="50"></asp:TextBox> 
                                                                    <label for="txtSchedule" style="margin-top:-10px; top:0px;" runat="server">Schedule</label>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="form-section-row">
                                                        <div class="form-section12">
                                                            <div class="section-ttle" id ="divCustomTitle" runat="server">Custom</div>
                                                            <div class="form-section3 w-25"  id="groupCusField1" runat="server">
                                                                <div class="input-field col s11">
                                                                    <div class="row">
                                                                        <asp:Label ID="lblCusField1" runat="server" AssociatedControlID="txtCusField1"></asp:Label>
                                                                        <asp:TextBox ID="txtCusField1" runat="server">
                                                                        </asp:TextBox>

                                                                    </div>
                                                                </div>
                                                                <div class="input-field col s2">
                                                                    <div class="row">
                                                                        &nbsp;
                                                                    </div>
                                                                </div>
                                                            </div>

                                                            <div id="groupCusField2" class="form-section3 w-25"  runat="server">
                                                                <div class="input-field col s11">
                                                                    <div class="row">
                                                                        <asp:Label ID="lblCusField2" runat="server" AssociatedControlID="txtCusField2"></asp:Label>
                                                                        <asp:TextBox ID="txtCusField2" runat="server">
                                                                        </asp:TextBox>
                                                                    </div>
                                                                </div>
                                                                <div runat="server" id="groupCusField2Blank" visible="false" class="input-field col s2">
                                                                    <div class="row">
                                                                        &nbsp;
                                                                    </div>
                                                                </div>
                                                            </div>

                                                            <div id="groupCusField3" class="form-section3 w-25"  runat="server">
                                                                <div class="input-field col s11">
                                                                    <div class="row">
                                                                        <asp:Label ID="lblCusField3" runat="server" AssociatedControlID="txtCusField3"></asp:Label>
                                                                        <asp:TextBox ID="txtCusField3" runat="server">
                                                                        </asp:TextBox>
                                                                    </div>
                                                                </div>
                                                                <div runat="server" id="groupCusField3Blank" visible="true" class="input-field col s2">
                                                                    <div class="row">
                                                                        &nbsp;
                                                                    </div>
                                                                </div>
                                                            </div>

                                                            <div id="groupCusField4" class="form-section3 w-25"  runat="server">
                                                                <div class="input-field col s12">
                                                                    <div class="row">
                                                                        <asp:Label ID="lblCusField4" runat="server" AssociatedControlID="txtCusField4"></asp:Label>

                                                                        <asp:TextBox ID="txtCusField4" runat="server">
                                                                        </asp:TextBox>
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
                                                   
                                                    <div style="clear: both;"></div>
                                                </div>
                                            </div>
                                            <div class="cf"></div>
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
                                        <li runat ="server" id="divCustom">
                                            <div id="accrdcustom" class="collapsible-header accrd accordian-text-custom ">
                                                <i class="mdi-action-account-circle"></i>
                                                <asp:Label ID="Label1" runat="server">Test Workflow</asp:Label>
                                            </div>
                                            <div class="collapsible-body" id="dvCustomSetup">

                                                 <asp:HiddenField ID="hdnLineOpenned" runat="server" />
                                                                                <asp:HiddenField ID="hdnOrgMemberKey" runat="server" />
                                                                                <asp:HiddenField ID="hdnOrgMemberDisp" runat="server" />
                                                <asp:Panel ID="Panel1" runat="server">
                                                    <div class="form-content-wrap">
                                                        <div class="form-content-pd">
                                                            <div class="grid_container">
                                                                <div class="RadGrid RadGrid_Material FormGrid">
                                                                    <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel_Setup" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">
                                                                        <telerik:RadGrid RenderMode="Auto" ID="gvTestCustom" ShowFooter="false" PageSize="50"
                                                                            PagerStyle-AlwaysVisible="false" OnItemDataBound="gvTestCustom_ItemDataBound" OnItemCommand="gvCustom_RowCommand"
                                                                            ShowStatusBar="true" runat="server" AllowPaging="True" Width="100%" AllowCustomPaging="false">
                                                                            <CommandItemStyle />
                                                                            <GroupingSettings CaseSensitive="false" />
                                                                            <ClientSettings AllowColumnsReorder="false" ReorderColumnsOnClient="false">
                                                                                <Selecting AllowRowSelect="false"></Selecting>
                                                                                <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                                                                <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                                                            </ClientSettings>
                                                                            <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="false" ShowFooter="True">
                                                                                <Columns>

                                                                                    <telerik:GridTemplateColumn HeaderStyle-Width="10" AllowFiltering="false" ItemStyle-Width="0.5%" FooterStyle-Width="0.5%">

                                                                                        <ItemTemplate>
                                                                                            <asp:HiddenField ID="txtOrderNo" Value='<%# Eval("OrderNo") %>' runat="server"></asp:HiddenField>
                                                                                            <asp:Label ID="lblIndex" Visible="true" runat="server" Text="<%# Container.ItemIndex +1 %>"></asp:Label>
                                                                                            <asp:Label ID="lblID" runat="server" Text='<%# Eval("ID") %>' Visible="false"></asp:Label>
                                                                                            <asp:Label ID="lblLine" runat="server" Text='<%# Eval("Line") %>' CssClass="customline" Style="display: none;"></asp:Label>
                                                                                            <asp:Label ID="lblFormat" runat="server" Text='<%# Eval("Format") %>' Visible="false"></asp:Label>
                                                                                            <asp:Label ID="lblCustom" runat="server" Text='<%# Eval("Label") %>' Visible="false"></asp:Label>
                                                                                        </ItemTemplate>

                                                                                    </telerik:GridTemplateColumn>

                                                                                    <telerik:GridTemplateColumn HeaderStyle-Width="150" AllowFiltering="false" HeaderText="Name" HeaderStyle-CssClass="itemHeader">
                                                                                        <ItemTemplate>
                                                                                            <div style="text-align: left ;">
                                                                                                <asp:HiddenField ID="hdnTestItemValueID" Value='' runat="server"></asp:HiddenField>
                                                                                                <asp:Panel ID="divFormatText" runat="server" Visible="false">
                                                                                                    <label class="fontHeader" for="txtFormatText"><%# Eval("Label") %></label>
                                                                                                    <asp:TextBox ID="txtFormatText" runat="server" Text=''></asp:TextBox>
                                                                                                </asp:Panel>

                                                                                                <asp:Panel ID="divFormatDrop" runat="server" Visible="false">
                                                                                                    <label class="fontHeader" for="drpdwnCustom"><%# Eval("Label") %></label>
                                                                                                    <asp:DropDownList ID="drpdwnCustom" runat="server" CssClass="browser-default fontHeader">
                                                                                                    </asp:DropDownList>
                                                                                                </asp:Panel>
                                                                                                <asp:Panel ID="divFormatCurrent" runat="server" Visible="false">
                                                                                                    <label class="fontHeader"  for="txtFormatCurrent"><%# Eval("Label") %></label>
                                                                                                    <asp:TextBox ID="txtFormatCurrent" runat="server" Text=''
                                                                                                        CssClass="custom currency"></asp:TextBox>
                                                                                                </asp:Panel>
                                                                                                <asp:Panel ID="divFormatDate" runat="server" Visible="false">
                                                                                                    <label class="fontHeader"  for="txtFormatDate"><%# Eval("Label") %></label>
                                                                                                    <asp:TextBox ID="txtFormatDate" runat="server" Text=''
                                                                                                        CssClass="custom datepicker_mom"></asp:TextBox>
                                                                                                </asp:Panel>
                                                                                                <asp:Panel ID="divFormatCheckbox" runat="server" Visible="false" >

                                                                                                    <asp:CheckBox ID="chkCustomFormat" runat="server" Text='&nbsp'
                                                                                                        CssClass="css-checkbox custom"></asp:CheckBox>
                                                                                                    <label  class="fontHeader" for="chkCustomFormat"><%# Eval("Label") %></label>
                                                                                                </asp:Panel>
                                                                                            </div>

                                                                                        </ItemTemplate>
                                                                                        <FooterStyle VerticalAlign="Middle" />

                                                                                    </telerik:GridTemplateColumn>

                                                                                    <telerik:GridTemplateColumn HeaderStyle-Width="60" AllowFiltering="false" HeaderText="Alert " HeaderStyle-CssClass="itemHeader">
                                                                                        <ItemTemplate>
                                                                                            <asp:CheckBox ID="chkSelectAlert" CssClass="css-checkbox" Text=" " runat="server" />
                                                                                        </ItemTemplate>
                                                                                        <FooterStyle VerticalAlign="Middle" />

                                                                                    </telerik:GridTemplateColumn>

                                                                                    <telerik:GridTemplateColumn HeaderStyle-Width="150" AllowFiltering="false" HeaderText="Team Member" HeaderStyle-CssClass="itemHeader">

                                                                                        <ItemTemplate>
                                                                                            <div class="tag-div materialize-textarea textarea-border" id="cusLabelTag" style="text-align: left !important; cursor: pointer;" onclick="ShowTeamMemberWindow(this);" runat="server"></div>
                                                                                            <asp:HiddenField ID="hdnMembers" runat="server" Value='<%# Eval("TeamMember") %>' />
                                                                                            <asp:TextBox ID="txtMembers" class='<%# "txtMembers_" + Eval("Line") %>' runat="server" Style='min-width: 100px !important; display: none;'
                                                                                                Text='<%# Eval("TeamMemberDisplay") %>'></asp:TextBox>

                                                                                        </ItemTemplate>

                                                                                    </telerik:GridTemplateColumn>

                                                                                    <telerik:GridTemplateColumn HeaderStyle-Width="100" AllowFiltering="false" HeaderText="Updated By" HeaderStyle-CssClass="itemHeader">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lbUpdatedBy" runat="server"></asp:Label>
                                                                                        </ItemTemplate>


                                                                                    </telerik:GridTemplateColumn>
                                                                                    <telerik:GridTemplateColumn HeaderStyle-Width="60" AllowFiltering="false" HeaderText="Updated Date" HeaderStyle-CssClass="itemHeader">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lbUpdatedDate" runat="server"></asp:Label>
                                                                                        </ItemTemplate>


                                                                                    </telerik:GridTemplateColumn>
                                                                                </Columns>
                                                                            </MasterTableView>
                                                                        </telerik:RadGrid>
                                                                    </telerik:RadAjaxPanel>

                                                                </div>
                                                            </div>
                                                            <div class="cf"></div>
                                                        </div>
                                                    </div>
                                                </asp:Panel>
                                                <div style="clear: both;"></div>
                                            </div>
                                        </li>
                                        
                                          <%--Begin Contact--%>
                                        <li runat="server" id="divContact">
                                            <div id="accrdcontacts" class="collapsible-header accrd accordian-text-custom ">
                                                <i class="mdi-action-account-circle"></i>
                                                <asp:Label ID="Label2" runat="server">Contacts</asp:Label>
                                            </div>
                                            <div class="collapsible-body" id="dvContactSetup">
                                                <asp:Panel ID="pnlgvConPermission" runat="server">
                                                    <div class="form-content-wrap">
                                                        <div class="form-content-pd">
                                                            <div class="btncontainer">
                                                                <div class="btnlinks">
                                                                    <asp:LinkButton ID="lnkAddnew" runat="server" CausesValidation="False" OnClientClick="OpenContactWindow();return false">Add</asp:LinkButton>
                                                                    <div style="display: none;">
                                                                        <asp:LinkButton ID="btnReloadContact" runat="server" CausesValidation="False" OnClick="btnReloadContact_Click"> Add</asp:LinkButton>
                                                                    </div>
                                                                </div>
                                                                <div class="btnlinks">
                                                                     <asp:LinkButton ToolTip="Edit"
                                                    ID="btnEdit" runat="server"
                                                    OnClientClick="OpenContactWindowEdit();return false;" CausesValidation="False" >Edit</asp:LinkButton>
                                                                </div>
                                                                <div class="btnlinks">
                                                                    <asp:LinkButton OnClientClick="return confirm('Are you sure you want to delete the items?')"
                                                                        ID="btnDelete" runat="server" CausesValidation="False" OnClick="btnDelete_Click">Delete</asp:LinkButton>
                                                                </div>
                                                                <div class="btnlinks">
                                                                    <asp:HyperLink ID="lnkMail" Style="cursor: pointer;" runat="server">Email</asp:HyperLink>
                                                                </div>
                                                            </div>
                                                            <div class="grid_container">
                                                                <div class="RadGrid RadGrid_Material FormGrid">
                                                                    <telerik:RadCodeBlock ID="RadCodeBlock_Contacts" runat="server">
                                                                        <script type="text/javascript">
                                                                            function pageLoad() {
                                                                                var grid = $find("<%= RadGrid_Contacts.ClientID %>");
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

                                                                    <telerik:RadAjaxPanel ID="RadAjaxPanel_gvContacts" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">
                                                                        <telerik:RadGrid RenderMode="Auto" ID="RadGrid_Contacts" AllowFilteringByColumn="true" ShowFooter="True" PageSize="50"
                                                                            OnNeedDataSource="RadGrid_Contacts_NeedDataSource" PagerStyle-AlwaysVisible="true" OnPreRender="RadGrid_Contacts_PreRender"
                                                                            ShowStatusBar="true" runat="server" AllowPaging="true" AllowSorting="true" Width="100%" AllowCustomPaging="True">
                                                                            <CommandItemStyle />
                                                                            <GroupingSettings CaseSensitive="false" />
                                                                            <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                                                                                <Selecting AllowRowSelect="True"></Selecting>
                                                                                <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                                                                <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                                                            </ClientSettings>
                                                                            <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="True" ShowFooter="True">
                                                                                <Columns>

                                                                                    <telerik:GridClientSelectColumn UniqueName="ClientSelectColumn" HeaderStyle-Width="28">
                                                                                    </telerik:GridClientSelectColumn>

                                                                                    <telerik:GridTemplateColumn UniqueName="lblIndexID" Visible="false" AllowFiltering="false" ShowFilterIcon="false">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblIndex" runat="server" Text='<%# Container.ItemIndex %>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>

                                                                                    <telerik:GridTemplateColumn Visible="false" AllowFiltering="false" ShowFilterIcon="false">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblId" runat="server" Text='<%# Bind("ContactID") %>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>

                                                                                    <telerik:GridTemplateColumn UniqueName="lblIndexID" Display="false" ShowFilterIcon="false">
                                                                                        <ItemTemplate>                                                                                           
                                                                                            <%# Container.ItemIndex %>
                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>

                                                                                    <telerik:GridTemplateColumn DataField="Name" HeaderText="Name" SortExpression="Name"
                                                                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" HeaderStyle-Width="140"
                                                                                        ShowFilterIcon="false">
                                                                                        <ItemTemplate>
                                                                                             <asp:HiddenField ID="grid_hdnIndex" runat="server" Value="<%# Container.ItemIndex %>"/>
                                                                                             <asp:HiddenField ID="grid_Id" runat="server" Value=<%# Bind("ContactID") %>/>
                                                                                            <asp:Label ID="lblName" runat="server"><%#Eval("Name")%></asp:Label>
                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>


                                                                                    <telerik:GridTemplateColumn DataField="Title" HeaderText="Title" HeaderStyle-Width="140"
                                                                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="Title"
                                                                                        ShowFilterIcon="false">
                                                                                         <ItemTemplate>
                                                                                            <asp:Label ID="lblTitle" runat="server"><%#Eval("Title")%></asp:Label>
                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>

                                                                                    <telerik:GridTemplateColumn DataField="Phone" HeaderText="Phone" HeaderStyle-Width="140"
                                                                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="Phone"
                                                                                        ShowFilterIcon="false">
                                                                                          <ItemTemplate>
                                                                                            <asp:Label ID="lblPhone" runat="server"><%#Eval("Phone")%></asp:Label>
                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>

                                                                                    <telerik:GridTemplateColumn DataField="Fax" HeaderText="Fax" HeaderStyle-Width="140"
                                                                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="Fax"
                                                                                        ShowFilterIcon="false">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblFax" runat="server"><%#Eval("Fax")%></asp:Label>
                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>

                                                                                    <telerik:GridTemplateColumn DataField="Cell" HeaderText="Cell" HeaderStyle-Width="140"
                                                                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="Cell"
                                                                                        ShowFilterIcon="false">
                                                                                         <ItemTemplate>
                                                                                            <asp:Label ID="lblCell" runat="server"><%#Eval("Cell")%></asp:Label>
                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>

                                                                                    <telerik:GridTemplateColumn HeaderStyle-Width="140" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="Email" ShowFilterIcon="false">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblEmail" runat="server"><%#Eval("Email")%></asp:Label>
                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>

                                                                                    <telerik:GridTemplateColumn HeaderText="Ticket" ShowFilterIcon="false" HeaderStyle-Width="60"  HeaderStyle-CssClass="itemHeader">
                                                                                        <ItemTemplate>
                                                                                            <div style="text-align: center">
                                                                                                <asp:CheckBox ID="chkTicket" runat="server" Enabled="false" Checked='<%# Convert.ToBoolean((Eval("EmailTicket")==DBNull.Value ? false:Eval("EmailTicket")  )) %>' />
                                                                                            </div>
                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>
                                                                                    <telerik:GridTemplateColumn HeaderText="Invoice/Statements" ShowFilterIcon="false" HeaderStyle-Width="140" HeaderStyle-CssClass="itemHeader">
                                                                                        <ItemTemplate>
                                                                                            <div style="text-align: center">
                                                                                                <asp:CheckBox ID="chkInvoice" runat="server" Enabled="false" Checked='<%# Convert.ToBoolean((Eval("EmailRecInvoice")==DBNull.Value ? false:Eval("EmailRecInvoice")  )) %>' />
                                                                                            </div>
                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>
                                                                                    <telerik:GridTemplateColumn HeaderText="Shutdown" ShowFilterIcon="false" HeaderStyle-Width="80"  HeaderStyle-CssClass="itemHeader">
                                                                                        <ItemTemplate>
                                                                                            <div style="text-align: center">
                                                                                                <asp:CheckBox ID="chkShutdown" runat="server" Enabled="false" Checked='<%# Convert.ToBoolean((Eval("ShutdownAlert")==DBNull.Value ? false:Eval("ShutdownAlert")  )) %>' />
                                                                                            </div>
                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>
                                                                                    <telerik:GridTemplateColumn HeaderText="Tests" ShowFilterIcon="false" HeaderStyle-Width="60"  HeaderStyle-CssClass="itemHeader">
                                                                                        <ItemTemplate>
                                                                                            <div style="text-align: center">
                                                                                                <asp:CheckBox ID="chkTests" runat="server" Enabled="false" Checked='<%# Convert.ToBoolean((Eval("EmailRecTestProp")==DBNull.Value ? false:Eval("EmailRecTestProp")  )) %>' />
                                                                                            </div>
                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>
                                                                                </Columns>
                                                                            </MasterTableView>
                                                                        </telerik:RadGrid>
                                                                    </telerik:RadAjaxPanel>


                                                                </div>
                                                            </div>

                                                            <div class="cf"></div>
                                                        </div>
                                                    </div>
                                                </asp:Panel>
                                                <div style="clear: both;"></div>
                                            </div>
                                        </li>
                                          <%--End Contact--%>
                                    
                                       
                                        
                                        <li runat="server" id="divSchedule">
                                            <div id="accrdSchedule" class="collapsible-header accrd accordian-text-custom ">
                                                <i class="mdi-action-account-circle"></i>
                                                <asp:Label ID="Label7" runat="server">Schedule</asp:Label>
                                            </div>
                                            <div class="collapsible-body">
                                                <div id="tabSchedule" class="col s12 tab-container-border lighten-4">
                                                    <div class="form-section-row">
                                                        <div class="form-content-pd">
                                                            <div class="btncontainer">

                                                                <div class="btnlinks">
                                                                    <asp:LinkButton ID="lnkAddSchedule" runat="server" OnClientClick="OpenAddScheduleWindow();return false;">Add Schedule</asp:LinkButton>
                                                                </div>
                                                                <div class="btnlinks">
                                                                    <asp:LinkButton ID="lnkEditSchedule" runat="server" OnClientClick="OpenEditScheduleWindow();return false;">Edit Schedule</asp:LinkButton>
                                                                </div>
                                                                <div class="btnlinks">
                                                                    <asp:LinkButton ID="lnkDeleteSchedule" runat="server" CausesValidation="False" OnClick="lnkDeleteSchedule_Click" OnClientClick="return validateDeleteSchedule(); ">Delete</asp:LinkButton>
                                                                </div>
                                                            </div>

                                                            <div class="grid_container">
                                                                <div class="form-section-row" style="margin-bottom: 0 !important;">
                                                                    <div class="RadGrid RadGrid_Material FormGrid">
                                                                        <telerik:RadCodeBlock ID="RadCodeBlock3" runat="server">
                                                                            <script type="text/javascript">
                                                                                function pageLoad() {
                                                                                    var grid = $find("<%= gvSchedule.ClientID %>");
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
                                                                        <telerik:RadGrid RenderMode="Auto" ID="gvSchedule" AllowFilteringByColumn="true" ShowFooter="True" PageSize="50"
                                                                            OnNeedDataSource="gvSchedule_NeedDataSource" 
                                                                            OnPreRender="gvSchedule_PreRender"
                                                                            PagerStyle-AlwaysVisible="true"
                                                                            ShowStatusBar="true" runat="server" AllowPaging="True" AllowSorting="true" Width="100%" AllowCustomPaging="True">
                                                                            <CommandItemStyle />
                                                                            <GroupingSettings CaseSensitive="false" />
                                                                            <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                                                                                <Selecting AllowRowSelect="True"></Selecting>
                                                                                <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                                                                <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                                                              
                                                                               
                                                                            </ClientSettings>
                                                                            <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="True" ShowFooter="True">
                                                                                <Columns>
                                                                                    <telerik:GridClientSelectColumn UniqueName="chkSelect" HeaderStyle-Width="28">
                                                                                    </telerik:GridClientSelectColumn>
                                                                                    <telerik:GridTemplateColumn HeaderText="Ticket" DataField="TicketID" ShowFilterIcon="false" HeaderStyle-Width="100" HeaderStyle-CssClass="itemHeader">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblTicketID" Text='<%# Eval("TicketID")%>' runat="server" CssClass="breakContent" />
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                                                    </telerik:GridTemplateColumn>
                                                                                    <telerik:GridTemplateColumn HeaderText="Ticket Status" DataField="TicketStatusName" ShowFilterIcon="false" HeaderStyle-Width="100" HeaderStyle-CssClass="itemHeader">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblTicketStatus" Text='<%# Eval("TicketStatusName")%>' runat="server" CssClass="breakContent" />
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                                                    </telerik:GridTemplateColumn>
                                                                                    <telerik:GridTemplateColumn HeaderText="Schedule Date" DataField="Schedule" ShowFilterIcon="false" HeaderStyle-Width="150" HeaderStyle-CssClass="itemHeader">
                                                                                        <ItemTemplate>
                                                                                            <asp:HiddenField ID="hdnScheduleYear" runat="server" Value=<%# Bind("ScheduledYear") %>/>
                                                                                                <asp:HiddenField ID="hdnScheduleID" runat="server" Value=<%# Bind("ID") %>/>
                                                                                              <asp:HiddenField ID="hdnScheduleStatusID" runat="server" Value=<%# Bind("ScheduledStatusID") %>/>
                                                                                            <asp:Label ID="lblScheduleDate" Text='<%# Eval("ScheduledDate", "{0:MM/dd/yyyy hh:mm}")%>' runat="server" CssClass="breakContent" />
                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>
                                                                                    <telerik:GridTemplateColumn HeaderText="ScheduledStatus" DataField="Type" ShowFilterIcon="false" HeaderStyle-Width="150" HeaderStyle-CssClass="itemHeader">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblScheduledStatus" Text='<%# Eval("ScheduledStatus")%>' runat="server" CssClass="breakContent" />
                                                                                        </ItemTemplate>
                                                                                       
                                                                                    </telerik:GridTemplateColumn>
                                                                                    <telerik:GridTemplateColumn HeaderText="ScheduledYear" DataField="ScheduledYear" ShowFilterIcon="false" HeaderStyle-Width="100" HeaderStyle-CssClass="itemHeader">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblScheduledYear" Text='<%# Eval("ScheduledYear")%>' runat="server" CssClass="breakContent" />
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                                                    </telerik:GridTemplateColumn>


                                                                                    <telerik:GridTemplateColumn HeaderText="Worker" DataField="Worker" ShowFilterIcon="false" HeaderStyle-Width="200" HeaderStyle-CssClass="itemHeader">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblScheduleWorker" Text='<%# Eval("Worker")%>' runat="server" CssClass="breakContent" />
                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>

                                                                                </Columns>
                                                                            </MasterTableView>
                                                                            <SelectedItemStyle CssClass="selectedrowcolor" />
                                                                            <FilterMenu CssClass="RadFilterMenu_CheckList">
                                                                            </FilterMenu>
                                                                        </telerik:RadGrid>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="cf"></div>
                                                        </div>
                                                    </div>

                                                </div>
                                                <div style="clear: both;"></div>
                                            </div>

                                        </li>
                                         <li runat="server" id="divPriceHistory">
                                            <div id="accrdPriceHistory" class="collapsible-header accrd accordian-text-custom ">
                                                <i class="mdi-action-account-circle"></i>
                                                <asp:Label ID="Label6" runat="server">Price History</asp:Label>
                                            </div>
                                            <div class="collapsible-body" id="dvPricesHistoryetup">
                                                <asp:Panel ID="Panel2" runat="server">
                                                    <div class="form-content-wrap">
                                                        <div class="form-content-pd">
                                                              <div class="btncontainer">

                                                                  <div class="btnlinks">
                                                                      <asp:LinkButton ID="lnkAddPrice" runat="server" OnClientClick="OpenAddPriceWindow();return false;">Add Price</asp:LinkButton>
                                                                  </div>
                                                                  <div class="btnlinks">
                                                                      <asp:LinkButton ID="lnkEditPrice" runat="server" OnClientClick="OpenEditPriceWindow();return false;">Edit Price</asp:LinkButton>
                                                                  </div>
                                                                  <div class="btnlinks">
                                                                      <asp:LinkButton ID="lnkDeletePrice" runat="server" CausesValidation="False" OnClick="lnkDeletePrice_Click" OnClientClick="return confirm('Do you really want to delete this price ?');">Delete</asp:LinkButton>
                                                                  </div>
                                                              </div>

                                                            <div class="grid_container">
                                                                <div class="RadGrid RadGrid_Material FormGrid">
                                                                    <telerik:RadCodeBlock ID="RadCodeBlock_PriceHistory" runat="server">
                                                                        <script type="text/javascript">
                                                                            function pageLoad() {
                                                                                var grid = $find("<%= RadGrid_PriceHistory.ClientID %>");
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

                                                                    <telerik:RadAjaxPanel ID="RadAjaxPanel_gvPricesHistory" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">
                                                                        <telerik:RadGrid RenderMode="Auto" ID="RadGrid_PriceHistory" AllowFilteringByColumn="false" ShowFooter="True" PageSize="50"
                                                                            OnNeedDataSource="RadGrid_PriceHistory_NeedDataSource" PagerStyle-AlwaysVisible="true" 
                                                                            ShowStatusBar="true" runat="server" AllowPaging="true" AllowSorting="true" Width="100%" AllowCustomPaging="True">
                                                                            <CommandItemStyle />
                                                                            <GroupingSettings CaseSensitive="false" />
                                                                            <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                                                                                <Selecting AllowRowSelect="True"></Selecting>
                                                                                <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                                                                <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                                                            </ClientSettings>
                                                                            <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="True" ShowFooter="True">
                                                                                <Columns>
                                                                                    <telerik:GridClientSelectColumn UniqueName="chkSelect" HeaderStyle-Width="28">
                                                                                    </telerik:GridClientSelectColumn>
                                                                                    <telerik:GridTemplateColumn DataField="PriceYear" HeaderText="Year"  SortExpression="PriceYear"
                                                                                        AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" HeaderStyle-Width="140"
                                                                                        ShowFilterIcon="false">
                                                                                        <ItemTemplate>
                                                                                            
                                                                                            <asp:Label ID="lblPriceYear" runat="server" Text='<%# Eval("PriceYear" )%>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>

                                                                                     <telerik:GridTemplateColumn SortExpression="Chargeable" AutoPostBackOnFilter="true"  
                                                                                        HeaderText="Chargeable" ShowFilterIcon="false" HeaderStyle-Width="100" UniqueName="Chargeable">
                                                                                        <ItemTemplate>

                                                                                            <asp:Label
                                                                                                ID="lblChargeable"
                                                                                                runat="server"
                                                                                                Text='<%# Eval("Chargeable") == DBNull.Value ? "No" : Convert.ToBoolean (Eval("Chargeable"))==false ?"No":"Yes" %>'>
                                                                                            </asp:Label>

                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>
                                                                                    <telerik:GridTemplateColumn DataField="DefaultAmount" HeaderText="Default Amount" HeaderStyle-Width="140"
                                                                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="DefaultAmount"
                                                                                        ShowFilterIcon="false">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblAmount" runat="server"><%#Eval("DefaultAmount", "{0:c}")%></asp:Label>
                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>

                                                                                    <telerik:GridTemplateColumn DataField="OverrideAmount" HeaderText="Override Amount" HeaderStyle-Width="140"
                                                                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="OverrideAmount"
                                                                                        ShowFilterIcon="false">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblOverrideAmount" runat="server"><%#Eval("OverrideAmount", "{0:c}")%></asp:Label>
                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>

                                                                                     <telerik:GridTemplateColumn SortExpression="ThirdPartyRequired" AutoPostBackOnFilter="true"  
                                                                                        HeaderText="Third Party Witness" ShowFilterIcon="false" HeaderStyle-Width="100" UniqueName="ThirdPartyRequired">
                                                                                        <ItemTemplate>

                                                                                            <asp:Label
                                                                                                ID="lblThirdPartyYear"
                                                                                                runat="server"
                                                                                                Text='<%# Eval("ThirdPartyRequired") == DBNull.Value ? "No" : Convert.ToBoolean (Eval("ThirdPartyRequired"))==false ?"No":"Yes" %>'>
                                                                                            </asp:Label>

                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>
                                                                                       <telerik:GridTemplateColumn DataField="ThirdPartyName" HeaderText="Third Party Name" HeaderStyle-Width="140"
                                                                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="ThirdPartyName" UniqueName="ThirdPartyName"
                                                                                        ShowFilterIcon="false">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblThirdPartyNameYear" runat="server"><%#Eval("ThirdPartyName", "{0:c}")%></asp:Label>
                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>
                                                                                     <telerik:GridTemplateColumn DataField="ThirdPartyPhone" HeaderText="Third Party Phone" HeaderStyle-Width="140"
                                                                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="ThirdPartyPhone" UniqueName="ThirdPartyPhone"
                                                                                        ShowFilterIcon="false">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblThirdPartyPhoneYear" runat="server"><%#Eval("ThirdPartyPhone", "{0:c}")%></asp:Label>
                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>
                                                                                   
                                                                                   <%--  <telerik:GridTemplateColumn DataField="DueDate" HeaderText="Due Date" HeaderStyle-Width="140" SortExpression="DueDate"
                                                                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" 
                                                                                        ShowFilterIcon="false">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblDueDate" runat="server"><%#Eval("DueDate","{0:MM/dd/yyyy}")%></asp:Label>
                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>--%>

                                                                                    <telerik:GridTemplateColumn DataField="CreatedDate" HeaderText="Updated Date" HeaderStyle-Width="140"  SortExpression="CreatedDate"
                                                                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" 
                                                                                        ShowFilterIcon="false">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblCreatedDate" runat="server"><%#Eval("CreatedDate","{0:MM/dd/yyyy}")%></asp:Label>
                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>

                                                                                      <telerik:GridTemplateColumn DataField="CreatedBy" HeaderText="Updated By" HeaderStyle-Width="140"  SortExpression="CreatedBy"
                                                                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" 
                                                                                        ShowFilterIcon="false">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblCreatedBy" runat="server"><%#Eval("CreatedBy")%></asp:Label>
                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>
                                                                                </Columns>
                                                                            </MasterTableView>
                                                                        </telerik:RadGrid>
                                                                    </telerik:RadAjaxPanel>


                                                                </div>
                                                            </div>

                                                            <div class="cf"></div>
                                                        </div>
                                                    </div>
                                                </asp:Panel>
                                                <div style="clear: both;"></div>
                                            </div>
                                        </li>
                                        <li runat="server" id="divDocument">
                                            <div id="accrdProposal" class="collapsible-header accrd accordian-text-custom ">
                                                <i class="mdi-action-account-circle"></i>
                                                <asp:Label ID="Label4" runat="server">Proposal</asp:Label>
                                            </div>
                                            <div class="collapsible-body">
                                                <div id="tabProposals" class="col s12 tab-container-border lighten-4">
                                                    <div class="form-section-row">
                                                        <div class="form-content-pd">
                                                        <div class="btncontainer">
                                                           
                                                            <div class="btnlinks">
                                                                <asp:LinkButton ID="lnkGeneralProposal" runat="server" OnClientClick="OpenSelectFormWindow();return false;">Generate Proposal</asp:LinkButton>
                                                            </div>
                                                            <div class="btnlinks">
                                                                <asp:LinkButton ID="btnSendEmail" CausesValidation="false" runat="server" OnClick="btnSendEmail_Click" Text="Send Email" Visible="true"></asp:LinkButton>
                                                            </div>
                                                            <div class="btnlinks">
                                                                <asp:LinkButton ID="lnkDeleteForms" runat="server" CausesValidation="False" OnClick="lnkDeleteForms_Click" OnClientClick="return confirm('Do you really want to delete this document ?');">Delete</asp:LinkButton>
                                                            </div>
                                                            </div>                                                       
                                                  
                                                        <div class="grid_container">
                                                            <div class="form-section-row" style="margin-bottom: 0 !important;">
                                                                <div class="RadGrid RadGrid_Material FormGrid">
                                                                    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
                                                                        <script type="text/javascript">
                                                                            function pageLoad() {
                                                                                var grid = $find("<%= gvDocuments.ClientID %>");
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
                                                                    <telerik:RadGrid RenderMode="Auto" ID="gvDocuments" AllowFilteringByColumn="true" ShowFooter="True" PageSize="50"
                                                                        OnNeedDataSource="gvDocuments_NeedDataSource" PagerStyle-AlwaysVisible="true"
                                                                       OnItemDataBound="gvDocuments_ItemDataBound"
                                                                        ShowStatusBar="true" runat="server" AllowPaging="True" AllowSorting="true" Width="100%" AllowCustomPaging="True">
                                                                        <CommandItemStyle />
                                                                        <GroupingSettings CaseSensitive="false" />
                                                                        <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                                                                            <Selecting AllowRowSelect="True"></Selecting>
                                                                            <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                                                            <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                                                        </ClientSettings>
                                                                        <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="True" ShowFooter="True">
                                                                            <Columns>
                                                                                <telerik:GridClientSelectColumn UniqueName="chkSelect" HeaderStyle-Width="28">
                                                                                </telerik:GridClientSelectColumn>
                                                                                 <telerik:GridTemplateColumn HeaderText="Proposal ID" DataField="ID" ShowFilterIcon="false" HeaderStyle-Width="100" HeaderStyle-CssClass="itemHeader">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblProposalID" Text='<%# Eval("ID")%>' runat="server" CssClass="breakContent" />
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                                                </telerik:GridTemplateColumn>

                                                                                <telerik:GridTemplateColumn HeaderText="Status" DataField="Status"  ShowFilterIcon="false" HeaderStyle-Width="100" HeaderStyle-CssClass="itemHeader">
                                                                                    <ItemTemplate>
                                                                                       <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("Status")%>' Visible="false" />
                                                                                           <asp:HiddenField ID="g_ProposalID" Value='<%# Eval("ID")%>' runat="server" />
                                                                                            <asp:HiddenField ID="hdnPDFPath" Value='<%# Eval("PdfFilePath")%>' runat="server" />
                                                                                          <asp:HiddenField ID="hdnDocPath" Value='<%# Eval("FilePath")%>' runat="server" />
                                                                                        <asp:DropDownList ID="ddlStatusDocument" runat="server" OnSelectedIndexChanged="ddlStatusDocument_SelectedIndexChanged" AutoPostBack="true" CssClass="browser-default">
                                                                                            <asp:ListItem Value="Pending">Pending</asp:ListItem>
                                                                                              <asp:ListItem Value="Declined">Declined</asp:ListItem>
                                                                                              <asp:ListItem Value="Sold">Sold</asp:ListItem>
                                                                                        </asp:DropDownList>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                                                </telerik:GridTemplateColumn>
                                                                                <telerik:GridTemplateColumn HeaderText="File Name" UniqueName="ID" DataField="FileName"  ShowFilterIcon="false" HeaderStyle-Width="200" HeaderStyle-CssClass="itemHeader">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="txtEDIT" runat="server"
                                                                                            Text='<%# Eval("FileName")%>'></asp:Label>
                                                                                     
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                                                </telerik:GridTemplateColumn>
                                                                                <telerik:GridTemplateColumn HeaderText="File Type" DataField="FileName"  ShowFilterIcon="false" HeaderStyle-Width="80" HeaderStyle-CssClass="itemHeader">
                                                                                    <ItemTemplate>
                                                                                        <asp:HyperLink runat="server" ID="Image1" ImageWidth="15px" 
                                                                                            ImageUrl='<%# Eval("ID").ToString() != "" ? "images/2PDFIcon.png" : "data:image/png;base64,R0lGODlhAQABAIAAAAAAAP///yH5BAEAAAAALAAAAAABAAEAAAIBRAA7"%>' 
                                                                                            onclick='<%# string.Format("downloadFile({0},{1})",Eval("ID"),1) %>'></asp:HyperLink>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                                                </telerik:GridTemplateColumn>
                                                                               <%-- <telerik:GridTemplateColumn HeaderText="File Name" DataField="FileName"  ShowFilterIcon="false" HeaderStyle-Width="60" HeaderStyle-CssClass="itemHeader">
                                                                                    <ItemTemplate>                                                                                      
                                                                                        <asp:HyperLink runat="server" ID="Images2" ImageWidth="15px" 
                                                                                            ImageUrl='<%# Eval("ID").ToString() != "" ? "images/Document.png" : "data:image/png;base64,R0lGODlhAQABAIAAAAAAAP///yH5BAEAAAAALAAAAAABAAEAAAIBRAA7"%>' 
                                                                                            onclick='<%# string.Format("downloadFile({0},{1})",Eval("ID"),0) %>'></asp:HyperLink>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                                                </telerik:GridTemplateColumn>--%>
                                                                              

                                                                                 <telerik:GridTemplateColumn HeaderText="Due Date" DataField="ToDate" ShowFilterIcon="false" HeaderStyle-Width="150" HeaderStyle-CssClass="itemHeader">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblDueDate" Text='<%# Eval("ToDate", "{0:MM/dd/yyyy hh:mm}")%>' runat="server" CssClass="breakContent" />

                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                                                </telerik:GridTemplateColumn>
                                                                                  <telerik:GridTemplateColumn HeaderText="Type" DataField="Type" ShowFilterIcon="false" HeaderStyle-Width="150" HeaderStyle-CssClass="itemHeader">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblSummary" Text='<%# Eval("Type").ToString()=="1"?"Summary":"Test Proposal"%>' runat="server" CssClass="breakContent" />

                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                                                </telerik:GridTemplateColumn>
                                                                                 <telerik:GridTemplateColumn HeaderText="Year" DataField="YearProposal" ShowFilterIcon="false" HeaderStyle-Width="100" HeaderStyle-CssClass="itemHeader">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblYearProposal" Text='<%# Eval("YearProposal")%>' runat="server" CssClass="breakContent" />
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                                                </telerik:GridTemplateColumn>

                                                                                
                                                                                <telerik:GridTemplateColumn HeaderText="Send To" DataField="SendTo" ShowFilterIcon="false" HeaderStyle-Width="200" HeaderStyle-CssClass="itemHeader">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblSendTo" Text='<%# Eval("SendTo")%>' runat="server" CssClass="breakContent" />
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                                                </telerik:GridTemplateColumn>
                                                                                <telerik:GridTemplateColumn HeaderText="Send By" DataField="SendFrom" ShowFilterIcon="false" HeaderStyle-Width="200" HeaderStyle-CssClass="itemHeader">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblSendBy" Text='<%# Eval("SendFrom")%>' runat="server" CssClass="breakContent" />
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                                                </telerik:GridTemplateColumn>
                                                                                <telerik:GridTemplateColumn HeaderText="Send On" DataField="SendOn" ShowFilterIcon="false" HeaderStyle-Width="150" HeaderStyle-CssClass="itemHeader">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblSendOn" Text='<%# Eval("SendOn", "{0:MM/dd/yyyy hh:mm}")%>' runat="server" CssClass="breakContent" />
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                                                </telerik:GridTemplateColumn>
                                                                                        
                                                                            </Columns>
                                                                        </MasterTableView>
                                                                        <SelectedItemStyle CssClass="selectedrowcolor" />
                                                                        <FilterMenu CssClass="RadFilterMenu_CheckList">
                                                                        </FilterMenu>
                                                                    </telerik:RadGrid>
                                                                </div>
                                                            </div>
                                                        </div>
                                                            <div class="cf"></div>
                                                        </div>
                                                    </div>
                                                   
                                                </div>
                                                 <div style="clear: both;"></div>
                                            </div>
                                             
                                        </li>
                                        <li runat="server" id="divLogs">
                                            <div id="accrdLogs" class="collapsible-header accrd accordian-text-custom ">
                                                <i class="mdi-action-account-circle"></i>
                                                <asp:Label ID="Label5" runat="server">Logs</asp:Label>
                                            </div>
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
                                                                                    <telerik:GridTemplateColumn DataField="LogYear" SortExpression="CreatedStamp" AutoPostBackOnFilter="true" DataType="System.String"
                                                                                        CurrentFilterFunction="Contains" HeaderText="Year" ShowFilterIcon="false">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblLogYear" runat="server" Text='<%# Eval("LogYear")%>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>
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
              
                </div>
            </div>
        </div>
    </div>
   
     <!-- Hidden Field -->

    <asp:HiddenField runat="server" ID="hdnAddeContact" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnEditeContact" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnDeleteContact" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnViewContact" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnAddEditContact" Value="0" />
    <asp:HiddenField runat="server" ID="hdnAddEditSchedule" Value="0" />
    <asp:HiddenField runat="server" ID="hdnAddEditPrice" Value="0" />
    <asp:HiddenField runat="server" ID="hdnUpdatePriceForAll" Value="0" />

    <div style="display: none">
        <asp:Button runat="server" ID="btnProcessDownload" OnClick="btnProcessDownload_Click" />
        <asp:HiddenField runat="server" ID="hdnDownloadID" Value="0" />
        <asp:HiddenField runat="server" ID="hdnDownloadType" Value="1" />
    </div>
    <asp:HiddenField runat="server" ID="hdnCreateTicketForAll" Value="0" />

    <asp:HiddenField runat="server" ID="hdnUpdateThirdPartyForAll" Value="0" />


    <asp:HiddenField runat="server" ID="hdnIsParentTestType" Value="0" />
    <asp:HiddenField runat="server" ID="hdnTestTypeChildID" Value="0" />
    <asp:HiddenField runat="server" ID="hdnTestTypeChildName" Value="" />
    <asp:HiddenField runat="server" ID="hdnHasChildTest" Value="0" />

    <asp:HiddenField runat="server" ID="hdnTestTypeParentName" Value="" />
    <asp:HiddenField runat="server" ID="hdnTestTypeParentID" Value="0" />
    <asp:HiddenField runat="server" ID="hdnHasParentTest" Value="0" />
    <asp:HiddenField runat="server" ID="hdnParentTestID" Value="0" />
    <asp:HiddenField runat="server" ID="hdnTestTypeParentChargable" Value="0" />

    <asp:HiddenField ID="hdntestid" runat="server" />
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="javascriptHolder" runat="Server">
     <script type="text/javascript" src="js/jquery.formatCurrency.js"></script>
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

            ddlTestTypeInit();
          $('#<%=txtLastDueDate.ClientID%>').change(function () {
              
                var testDueBy = $("#<%= drpdwnTestDueBy.ClientID %>").val();
                if (testDueBy === "0") {
                    updateDatetime($("#<%= txtLasttestdon.ClientID %>"), $("#<%= txtTestNextDueDate.ClientID %>"), 1);

            } else {
                updateDatetime($("#<%= txtLastDueDate.ClientID %>"), $("#<%= txtTestNextDueDate.ClientID %>"), 1);
                }

            });

            $('#<%=txtTestNextDueDate.ClientID%>').change(function () {
                updateDatetime($('#<%=txtTestNextDueDate.ClientID%>'), $('#<%=txtLastDueDate.ClientID%>'), -1);
                   reloadTestPrice();
            });
          
        });

        function ddlTestTypeInit() {
             // reloadTestPrice();
            $('#<%=ddlTestTypes.ClientID%>').change(function () {              
                var frequency = $('#<%=ddlTestTypes.ClientID%> :selected').data("frequency");
                var DueBy = $('#<%=ddlTestTypes.ClientID%> :selected').data("dueby");
                   var IsTicketCoveredByTestType = $('#<%=ddlTestTypes.ClientID%> :selected').data("isticketcoveredbytesttype");

               $("#<%= drpdwnTestDueBy.ClientID %>").val(DueBy);

                $('#<%=hdnTestTypeFrequency.ClientID%>').val(frequency);
                 $('#<%=hdnIsTicketCoveredByTestType.ClientID%>').val(IsTicketCoveredByTestType);
                updateDatetime($('#<%=txtLastDueDate.ClientID%>'), $('#<%=txtTestNextDueDate.ClientID%>'), 1);
                //Pricing
                  reloadTestType();
                reloadTestPrice();
              
                Materialize.updateTextFields();
            });
        }

        function updateDatetime(source, target, weight) {
            debugger;
            if ($('#<%=hdnTestTypeFrequency.ClientID%>').val() == '') {
                return;
            }
            var status = $("#<%= ddlStatus.ClientID %>").val();
            if (status == 0) {
                var sourceDate = moment(source.val(), 'MM/DD/YYYY');

            if (!sourceDate.isValid()) {
                return;
            }

            let month = parseInt($('#<%=hdnTestTypeFrequency.ClientID%>').val()) * weight;
            var newDate = sourceDate.add(month, "M");
            target.val(newDate.format('MM/DD/YYYY'));

            }
            
            Materialize.updateTextFields();
        }        

        $("#<%= txtLasttestdon.ClientID %>").change(function () {         
          <%--  debugger
            var testDueBy = $("#<%= drpdwnTestDueBy.ClientID %>").val();
            if (testDueBy === "0") {
                updateDatetime($("#<%= txtLasttestdon.ClientID %>"), $("#<%= txtTestNextDueDate.ClientID %>"), 1);

            } else {
                updateDatetime($("#<%= txtLastDueDate.ClientID %>"), $("#<%= txtTestNextDueDate.ClientID %>"), 1);
            }--%>
           //reloadTestPrice();
        });
        $("#<%= drpdwnTestDueBy.ClientID %>").change(function () {            
             var testDueBy = $("#<%= drpdwnTestDueBy.ClientID %>").val();             
            if (testDueBy === "0") {
                updateDatetime($("#<%= txtLasttestdon.ClientID %>"), $("#<%= txtTestNextDueDate.ClientID %>"), 1);

            } else {
                updateDatetime($("#<%= txtLastDueDate.ClientID %>"), $("#<%= txtTestNextDueDate.ClientID %>"), 1);
            }
                         reloadTestPrice();
        });
             $("#<%= txtThirdPartyPhone.ClientID %>").mask("(999) 999-9999? Ext 99999");
        $("#<%= txtThirdPartyPhone.ClientID %>").bind('paste', function () { $(this).val(''); });
                  $("#<%= txtThirdPartyPhoneYear.ClientID %>").mask("(999) 999-9999? Ext 99999");
        $("#<%= txtThirdPartyPhoneYear.ClientID %>").bind('paste', function () { $(this).val(''); });

       
         var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_pageLoaded(function () {
            $("[id*=txtContPhone]").mask("(999) 999-9999? Ext 99999");
            $("[id*=txtContPhone]").bind('paste', function () { $(this).val(''); });
            $("[id*=txtContCell]").mask("(999) 999-9999");
            $("[id*=txtContFax]").mask("(999) 999-9999");
        });

    
        
    

    </script>   
    <%--Pricing script--%>
    <script type="text/javascript">
        //Pricing

      
        $(document).ready(function () {
          
            $("#<%=txtAmount.ClientID%>").blur(function () {               
                 $(this).formatCurrency();
            }); 
             $("#<%=txtOverrideAmount.ClientID%>").blur(function () {               
                 $(this).formatCurrency();
            }); 
            //reloadTestPrice();
        });

         ///////////// Custom validator function for Unit auto search  ////////////////////
        function ChkUnit(sender, args) {
            var hdnEquipment = document.getElementById('<%=hdnEquipment.ClientID%>');
            if (hdnEquipment.value == '') {
                args.IsValid = false;
            }
        }

        function dataPrice() {
            this.elevId = 0;
            this.testtypeId = 0;
            this.priceYear = 0;
           
        }
        function dataDefaultPrice() {
            this.classification ='';
            this.testtypeId = 0;
            this.priceYear = 0;
           
        }
        function reloadTestPrice() {
            var objdataEquip = new dataDefaultPrice();          
            objdataEquip.classification = $('#<%=ddlClassification.ClientID%>').val();
            objdataEquip.testtypeId = parseInt($('#<%=ddlTestTypes.ClientID%>').val());
            objdataEquip.elevId = 0;
            if ($('#<%=hdnEquipment.ClientID%>').val() != "") {
                objdataEquip.elevId= parseInt($('#<%=hdnEquipment.ClientID%>').val());
            }

            var txtTestNextDueDate = $('#<%=txtTestNextDueDate.ClientID%>').val();

            if (txtTestNextDueDate == "") {
                objdataEquip.priceYear = 0;
            } else {
                debugger
                var index = txtTestNextDueDate.lastIndexOf("/")
                var strYear = txtTestNextDueDate.substring(index + 1)
                objdataEquip.priceYear = parseInt(strYear);

            }

            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "CustomerAuto.asmx/GetDefaultTestPriceByYear",
                data: JSON.stringify(objdataEquip),
                dataType: "json",
                async: true,
                success: function (data) {
                    var obj = JSON.parse(data.d);
                    if (obj.length > 0) {

                        var isChargable = 0;
                        var Amount = obj[0].Amount;
                        var Override = obj[0].Override;
                        var PriceYear = obj[0].PriceYear;
                        var ThirdPartyRequired = obj[0].ThirdPartyRequired;
                        var ExsitTestCover = obj[0].ExsitTestCover; 
                        if (ExsitTestCover == true) { 
                            $('#<%=txtAmount.ClientID%>').val(0);
                            $('#<%=txtOverrideAmount.ClientID%>').val(0);
                            $('#<%=txtAmount.ClientID%>').prop("disabled", true);
                  $('#<%=txtOverrideAmount.ClientID%>').prop("disabled", true);

                        }
                        $('#<%=chkThirdParty.ClientID%>').prop("checked", false);
                        if (ThirdPartyRequired != 0) {
                            $('#<%=chkThirdParty.ClientID%>').prop("checked", true);
                        }
                           isChargable =  $('#<%=chkChargeableEdit.ClientID%>').is(":checked");
                        if (isChargable == 1) {
                               $('#<%=txtAmount.ClientID%>').prop("disabled", false);
                  $('#<%=txtOverrideAmount.ClientID%>').prop("disabled", false);
                            if ($('#<%=txtAmount.ClientID%>').val() == "$0.00" || $('#<%=txtAmount.ClientID%>').val() == "0") {

                                $('#<%=txtAmount.ClientID%>').val(Amount);
                                $('#<%=txtOverrideAmount.ClientID%>').val(Override);

                                $('#<%=txtAmount.ClientID%>').formatCurrency();
                                $('#<%=txtOverrideAmount.ClientID%>').formatCurrency();

                            }
                        }


                       
                      
                        Materialize.updateTextFields();

                    } else {
                        $('#<%=txtAmount.ClientID%>').val(0);
                        $('#<%=txtOverrideAmount.ClientID%>').val(0);
                        $('#<%=txtAmount.ClientID%>').formatCurrency();
                        $('#<%=txtOverrideAmount.ClientID%>').formatCurrency();
                        $('#<%=chkThirdParty.ClientID%>').prop("checked", false);
                        Materialize.updateTextFields();
                    }

                },
                error: function (result) { }
            });

            return false;
        }
        function dataTestType() {           
            this.testtypeId = 0;
        }
         function reloadTestType() {
          
            var objdataEquip = new dataTestType();           
            objdataEquip.testtypeId = parseInt($('#<%=ddlTestTypes.ClientID%>').val());
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "CustomerAuto.asmx/ReceiveTestType",
                data: JSON.stringify(objdataEquip),
                dataType: "json",
                async: true,
                success: function (data) {                    
                    var obj = JSON.parse(data.d);
                    if (obj.length > 0) {
                        var chargeable = obj[0].Charge;
                        if (chargeable) {
                            $('#<%=chkChargeableEdit.ClientID%>').prop("checked", true);
                        } 
                    }   
                },
                error: function (result) { }
            }); 

              var ischeck = $('#<%=chkChargeableEdit.ClientID%>').is(":checked");
              if (!ischeck) {
                  $('#<%=txtAmount.ClientID%>').val(0);
                  $('#<%=txtOverrideAmount.ClientID%>').val(0);

                  $('#<%=txtAmount.ClientID%>').prop("disabled", true);
                  $('#<%=txtOverrideAmount.ClientID%>').prop("disabled", true);
              } else {
                    $('#<%=txtAmount.ClientID%>').prop("disabled", false);
                  $('#<%=txtOverrideAmount.ClientID%>').prop("disabled", false);
                 
              } 

            return false;
        }
        function UpdatePrice() {
              var ischeck = $('#<%=chkChargeableEdit.ClientID%>').is(":checked");
              if (!ischeck) {
                  $('#<%=txtAmount.ClientID%>').val(0);
                  $('#<%=txtOverrideAmount.ClientID%>').val(0);

                  $('#<%=txtAmount.ClientID%>').prop("disabled", true);
                  $('#<%=txtOverrideAmount.ClientID%>').prop("disabled", true);
              } else {
                    $('#<%=txtAmount.ClientID%>').prop("disabled", false);
                  $('#<%=txtOverrideAmount.ClientID%>').prop("disabled", false);
                  reloadTestPrice();
              }

              
              
          }
      
    </script>
    <%--Contact script--%>
    <script type="text/javascript">
        function OpenContactWindow() {
            $('#<%=txtContcName.ClientID%>').val("");
              $('#<%=txtTitle.ClientID%>').val("");
              $('#<%=txtContPhone.ClientID%>').val("");
              $('#<%=txtContFax.ClientID%>').val("");
              $('#<%=txtContCell.ClientID%>').val("");
              $('#<%=txtContEmail.ClientID%>').val("");

              $('#<%=chkEmailTicket.ClientID%>').prop("checked", false);
              $('#<%=chkEmailInvoice.ClientID%>').prop("checked", false);
              $('#<%=chkShutdownAlert.ClientID%>').prop("checked", false);
              $('#<%=chkTestProposals.ClientID%>').prop("checked", false);
              $('#<%=hdnAddEditContact.ClientID%>').val("0");
              var wnd = $find('<%=RadWindowContact.ClientID %>');
            wnd.set_title("Add Contact");
            wnd.Show();
        }
        function OpenContactWindowEdit() {
            var ID = "";
            var Name = "";
            var Title = "";
            var Phone = "";
            var Fax = "";
            var Cell = "";
            var Email = "";
            var EmailTicket = "";
            var EmailInvoice = "";
            var ShutdownAlert = "";
            var TestProposals = "";
            var gridIndex = "";

            $("#<%=RadGrid_Contacts.ClientID %>").find('tr:not(:first,:last)').each(function () {
                var $tr = $(this);
                $tr.find('input[type="checkbox"]:first:checked').each(function (index, value) {
                    ID = $tr.find('input[id*=grid_Id]').val();
                    Name = $tr.find('span[id*=lblName]').text();
                    Phone = $tr.find('span[id*=lblPhone]').text();
                    Fax = $tr.find('span[id*=lblFax]').text();
                    Cell = $tr.find('span[id*=lblCell]').text();
                    Email = $tr.find('span[id*=lblEmail]').text();
                    Title = $tr.find('span[id*=lblTitle]').text();

                    EmailTicket = $tr.find('input:checkbox[id*=chkTicket]').prop('checked');
                    EmailInvoice = $tr.find('input:checkbox[id*=chkInvoice]').prop('checked');
                    ShutdownAlert = $tr.find('input:checkbox[id*=chkShutdown]').prop('checked');
                    TestProposals = $tr.find('input:checkbox[id*=chkTests]').prop('checked');

                    gridIndex = $tr.find('input:hidden[id*=grid_hdnIndex]').val();

                });
            });

            if (ID != "") {

                $('#<%=hdnIndex.ClientID%>').val(gridIndex);
                $('#<%=hdnAddEditContact.ClientID%>').val("1");
                $('#<%=txtContcName.ClientID%>').val(Name);
                $('#<%=txtTitle.ClientID%>').val(Title);
                $('#<%=txtContPhone.ClientID%>').val(Phone);
                $('#<%=txtContFax.ClientID%>').val(Fax);
                $('#<%=txtContCell.ClientID%>').val(Cell);
                $('#<%=txtContEmail.ClientID%>').val(Email);

                $('#<%=chkEmailTicket.ClientID%>').prop("checked", EmailTicket);
                $('#<%=chkEmailInvoice.ClientID%>').prop("checked", EmailInvoice);
                $('#<%=chkShutdownAlert.ClientID%>').prop("checked", ShutdownAlert);
                $('#<%=chkTestProposals.ClientID%>').prop("checked", TestProposals);

                var wnd = $find('<%=RadWindowContact.ClientID %>');
                wnd.set_title("Edit Contact");

                wnd.set_height(500);
                wnd.Show();

                Materialize.updateTextFields();
            }
            else {
                ChkWarning();
            }
        }
        function ChkWarning() {
            noty({
                text: 'Please select any one to edit.',
                type: 'warning',
                layout: 'topCenter',
                closeOnSelfClick: false,
                timeout: 5000,
                theme: 'noty_theme_default',
                closable: true
            });
            return false;
        }
        function CloseContactWindow() {
            var wnd = $find('<%=RadWindowContact.ClientID %>');
            wnd.Close();
        }
        $(document).ready(function () {

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

           
        });

        function downloadFile(obj, type) {

            $("#<%= hdnDownloadID.ClientID %>").val(obj);
            $("#<%= hdnDownloadType.ClientID %>").val(type);
            var btn = document.getElementById('<%=btnProcessDownload.ClientID%>');
            btn.click();
        }

        

        function OpenSelectFormWindow() {
             var wnd = $find('<%=RadWindowTemplate.ClientID %>');
                wnd.set_title("Form");               
                wnd.Show();
        }
         function CloseSelectFormWindow() {
            var wnd = $find('<%=RadWindowTemplate.ClientID %>');
            wnd.Close();
        }
        function RedirectToEquipment() {
            debugger
            var elv = $("#<%=hdnEquipment.ClientID%>").val();
            
             var url = "addequipment.aspx?uid=";
             var backUrl = "&page=AddTests.aspx";
            if (getQueryStringValue('elv') != "") {
                backUrl= backUrl + "&elv=" + getQueryStringValue('elv');
                elv = getQueryStringValue('elv');
            }
            if (getQueryStringValue("LID") != "") {
                backUrl = backUrl + "&lid=" + getQueryStringValue("LID");
            }
            if (elv != "") {
               
                 window.location.replace(url + elv + backUrl);
            }
           
        }
        function getQueryStringValue(key) {
            return decodeURIComponent(window.location.search.replace(new RegExp("^(?:.*[&\\?]" + encodeURIComponent(key).replace(/[\.\+\*]/g, "\\$&") + "(?:\\=([^&]*))?)?.*$", "i"), "$1"));
        }  
        function checkSelectTemplate(){
            var flag = false;
             $("#<%=gvFormTemplate.ClientID %>").find('tr:not(:first,:last)').each(function () {
                var $tr = $(this);
                $tr.find('input[type="checkbox"]:first:checked').each(function (index, value) {
                    flag = true;

                });
            });
            
            if (flag==false) {
                  noty({
                text: 'Please select template.',
                type: 'warning',
                layout: 'topCenter',
                closeOnSelfClick: false,
                timeout: 5000,
                theme: 'noty_theme_default',
                closable: true
            });
            }
            return flag;

            
        }
        //function UpdateDefaultPrice() {
           
        //            reloadTestPrice();                   
        //            Materialize.updateTextFields();
        //            return false;
        //}
        function ShowConfirmAssignMessage() {
            var IsTicketCoveredByTestType = $('#<%=ddlTestTypes.ClientID%> :selected').data("isticketcoveredbytesttype");
            var numberOfTestInLoc = $("#<%=hdnNumberOfTestNoTicketInLoc.ClientID%>").val();
            if (numberOfTestInLoc >= 1) {
                noty({
                    dismissQueue: true,
                    layout: 'topCenter',
                    theme: 'noty_theme_default',
                    animateOpen: { height: 'toggle' },
                    animateClose: { height: 'toggle' },
                    easing: 'swing',
                    text: 'There are other tests due for this location. Would you like to mass create tickets?',
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
                                $("#<%=hdnCreateTicketForAll.ClientID%>").val("1");
                                $("#<%=lnkAssignTicket.ClientID%>").click();
                                $noty.close();

                            }
                        },
                        {
                            type: 'btn-danger', text: 'No', click: function ($noty) {
                                $("#<%=hdnCreateTicketForAll.ClientID%>").val("0");
                                $("#<%=lnkAssignTicket.ClientID%>").click();
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
                $("#<%=hdnCreateTicketForAll.ClientID%>").val("0");
                $("#<%=lnkAssignTicket.ClientID%>").click();
            }
        }

        function ShowConfirmUpdatePriceMessage() {    
               $("#<%=hdnUpdatePriceForAll.ClientID%>").val("0");
                noty({
                    dismissQueue: true,
                    layout: 'topCenter',
                    theme: 'noty_theme_default',
                    animateOpen: { height: 'toggle' },
                    animateClose: { height: 'toggle' },
                    easing: 'swing',
                    text: 'There are other tests due for this location. Would you like to update amount?',
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
                                $("#<%=hdnUpdatePriceForAll.ClientID%>").val("1");
                                $("#<%=lnkPriceSave.ClientID%>").click();
                                $noty.close();

                            }
                        },
                        {
                            type: 'btn-danger', text: 'No', click: function ($noty) {
                                $("#<%=hdnUpdatePriceForAll.ClientID%>").val("0");
                                $("#<%=lnkPriceSave.ClientID%>").click();
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
        function ShowConfirmSaveThirdpartyMessage() {    
            
            $("#<%=hdnUpdateThirdPartyForAll.ClientID%>").val("0");
            var oldValue = "";
            var newValue = "";
            oldValue = $("#<%=hdnThirdPartyName.ClientID%>").val();
            newValue = $("#<%=txtThirdPartyName.ClientID%>").val();

            if (oldValue != newValue) {
                $("#<%=hdnThirdPartyName.ClientID%>").val(newValue);
                var numberOfTestInLoc = $("#<%=hdnNumberOfTestNoTicketInLoc.ClientID%>").val();
                if (numberOfTestInLoc >= 1) {
                    noty({
                        dismissQueue: true,
                        layout: 'topCenter',
                        theme: 'noty_theme_default',
                        animateOpen: { height: 'toggle' },
                        animateClose: { height: 'toggle' },
                        easing: 'swing',
                        text: 'There are other tests in this location. Would you like to update third party witness?',
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
                                    $("#<%=hdnUpdateThirdPartyForAll.ClientID%>").val("1");
                                    $("#<%=btnSubmit.ClientID%>").click();
                                    $noty.close();

                                }
                            },
                            {
                                type: 'btn-danger', text: 'No', click: function ($noty) {
                                    $("#<%=hdnUpdateThirdPartyForAll.ClientID%>").val("0");
                                    $("#<%=btnSubmit.ClientID%>").click();
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
                    $("#<%=hdnUpdateThirdPartyForAll.ClientID%>").val("0");
                    $("#<%=btnSubmit.ClientID%>").click();
                }
            } else {
                 $("#<%=hdnUpdateThirdPartyForAll.ClientID%>").val("0");
                                $("#<%=btnSubmit.ClientID%>").click();
            }
          
               
           
        }

        
    </script>
    <script>
        function OpenAddScheduleWindow() {
             $('#<%=txtScheduleYear.ClientID%>').val("");
             $('#<%=txtScheduleDate.ClientID%>').val("");
             $('#<%=txtScheduleWorker.ClientID%>').val("");
             $('#<%=hdnAddEditSchedule.ClientID%>').val(0);
            $('#<%=ddlScheduleStatus.ClientID%>').val(0);
            $('#<%=hdnID.ClientID%>').val(0);  
             var wnd = $find('<%=RadWindowSchedule.ClientID %>');
             wnd.set_title("Add Schedule");
             wnd.set_height(450);
             wnd.Show();
         }

        function OpenEditScheduleWindow() {
            var ID = 0;
            var ScheduleDate = "";
            var TicketID = "";
            var ScheduleStatus = 0;
            var Worker = "";
            var ShcheduleYear = 0;           

            $("#<%=gvSchedule.ClientID %>").find('tr:not(:first,:last)').each(function () {
                 var $tr = $(this);
                 $tr.find('input[type="checkbox"]:first:checked').each(function (index, value) {
                      TicketID = $tr.find('span[id*=lblTicketID]').text();
                     ScheduleDate = $tr.find('span[id*=lblScheduleDate]').text();
                     Worker = $tr.find('span[id*=lblScheduleWorker]').text();
                     ShcheduleYear = $tr.find('span[id*=lblScheduledYear]').text();

                     ID = $tr.find('input:hidden[id*=hdnScheduleID]').val();
                     ScheduleStatus = $tr.find('input:hidden[id*=hdnScheduleStatusID]').val();
                 });
            });
            if (TicketID != "") {
                noty({
                    text: 'Ticket is created for this schedule. You can not edit',
                    type: 'warning',
                    layout: 'topCenter',
                    closeOnSelfClick: false,
                    timeout: 5000,
                    theme: 'noty_theme_default',
                    closable: true
                });
                return false;
            } else {
                 if (ID != "") {
                 $('#<%=hdnAddEditSchedule.ClientID%>').val(1);
                $('#<%=txtScheduleDate.ClientID%>').val(ScheduleDate);
                $('#<%=txtScheduleWorker.ClientID%>').val(Worker);
                $('#<%=txtScheduleYear.ClientID%>').val(ShcheduleYear);
                 $('#<%=ddlScheduleStatus.ClientID%>').val(ScheduleStatus);
                 $('#<%=hdnID.ClientID%>').val(ID);
                var wnd = $find('<%=RadWindowSchedule.ClientID %>');
                wnd.set_title("Edit Schedule");

                wnd.set_height(450);
                wnd.Show();

                Materialize.updateTextFields();
            }
            else {
                ChkWarning();
            }
            }

            
        }
        function CloseScheduleWindow() {
            var wnd = $find('<%=RadWindowSchedule.ClientID %>');
             wnd.Close();
        }
        function validateDeleteSchedule() {
            var ID = 0;

            var TicketID = "";
            $("#<%=gvSchedule.ClientID %>").find('tr:not(:first,:last)').each(function () {
                var $tr = $(this);
                $tr.find('input[type="checkbox"]:first:checked').each(function (index, value) {
                    TicketID = $tr.find('span[id*=lblTicketID]').text();

                    ID = $tr.find('input:hidden[id*=hdnScheduleID]').val();

                });
            });

            if (ID != "") {
                if (TicketID != "") {
                    noty({
                        text: 'Ticket is created for this schedule. You can not delete',
                        type: 'warning',
                        layout: 'topCenter',
                        closeOnSelfClick: false,
                        timeout: 5000,
                        theme: 'noty_theme_default',
                        closable: true
                    });
                    return false;
                } else {

                    if (!confirm("Do you really want to delete this schedule ?")) {
                        return false;
                    }
                    else {
                        return true;
                    }
                }

            } else {
                noty({
                    text: 'Please select any one to delete.',
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
         function ValidateSchedule() {
            var ScheduleDate= $('#<%=txtScheduleDate.ClientID%>').val();
             var ShcheduleYear = $('#<%=txtScheduleYear.ClientID%>').val();
              if (!ScheduleDate.includes(ShcheduleYear)) {
                            noty({
                            text: 'The scheduled date should be in the year ' + ShcheduleYear,
                            type: 'warning',
                            layout: 'topCenter',
                            closeOnSelfClick: false,
                            timeout: 5000,
                            theme: 'noty_theme_default',
                            closable: true
                  });
                  return false;
             }
             return true;
           }
    </script>
       <script>
           function OpenAddPriceWindow() {
               $('#<%=txtDefaultAmountYear.ClientID%>').prop("disabled", false);
               $('#<%=txtOverrideAmountYear.ClientID%>').prop("disabled", false);
               $('#<%=txtPriceYear.ClientID%>').prop("disabled", false);
               $('#<%=txtPriceYear.ClientID%>').val("");
               $('#<%=chkChargeablePriceYear.ClientID%>').prop("checked", true)
               $('#<%=txtOverrideAmountYear.ClientID%>').val("");
               $('#<%=txtDefaultAmountYear.ClientID%>').val("");

               $('#<%=chkThirdPartyYear.ClientID%>').prop("checked", false)
               $('#<%=txtThirdPartyNameYear.ClientID%>').val("");
               $('#<%=txtThirdPartyPhoneYear.ClientID%>').val("");


               $('#<%=hdnAddEditPrice.ClientID%>').val(0);
               var wnd = $find('<%=RadWindowPrice.ClientID %>');
               wnd.set_title("Add Test Pricing");
               wnd.set_height(400);

               wnd.Show();
         }

           function OpenEditPriceWindow() {
              $('#<%=txtDefaultAmountYear.ClientID%>').prop("disabled", false);
               $('#<%=txtOverrideAmountYear.ClientID%>').prop("disabled", false);
               $('#<%=txtPriceYear.ClientID%>').prop("disabled", false);
            var ID = 0;
            var charageable = 1;
            var DefaultAmount = "";
            var OverrideAmount = 0;  

             var ThirdParty = 0;
            var Phone = "";
            var Name ="";  
            var PriceYear = 0;          

            $("#<%=RadGrid_PriceHistory.ClientID %>").find('tr:not(:first,:last)').each(function () {
                 var $tr = $(this);
                 $tr.find('input[type="checkbox"]:first:checked').each(function (index, value) {
                     charageable = $tr.find('span[id*=lblChargeable]').text();
                     DefaultAmount = $tr.find('span[id*=lblAmount]').text();
                     OverrideAmount = $tr.find('span[id*=lblOverrideAmount]').text();

                      ThirdParty = $tr.find('span[id*=lblThirdPartyYear]').text();
                     Phone = $tr.find('span[id*=lblThirdPartyPhoneYear]').text();
                     Name = $tr.find('span[id*=lblThirdPartyNameYear]').text();


                     PriceYear = $tr.find('span[id*=lblPriceYear]').text();
                   
                 });
             });

             if (PriceYear != "") {
                 $('#<%=hdnAddEditPrice.ClientID%>').val(1);
                 $('#<%=txtPriceYear.ClientID%>').val(PriceYear);
                 if (charageable == "No") {
                     $('#<%=chkChargeablePriceYear.ClientID%>').prop("checked", false)
                     $('#<%=txtOverrideAmountYear.ClientID%>').val("0");
                     $('#<%=txtDefaultAmountYear.ClientID%>').val("0");
                     $('#<%=txtDefaultAmountYear.ClientID%>').prop("disabled", true);
                     $('#<%=txtOverrideAmountYear.ClientID%>').prop("disabled", true);
                 } else {
                     $('#<%=txtOverrideAmountYear.ClientID%>').val(OverrideAmount);
                     $('#<%=txtDefaultAmountYear.ClientID%>').val(DefaultAmount);
                     $('#<%=chkChargeablePriceYear.ClientID%>').prop("checked", true)
                 }
                 $('#<%=chkThirdPartyYear.ClientID%>').prop("checked", true);
                 if (ThirdParty == "No") {
                     $('#<%=chkThirdPartyYear.ClientID%>').prop("checked", false);
                 }

                 $('#<%=txtThirdPartyNameYear.ClientID%>').val(Name);
                 $('#<%=txtThirdPartyPhoneYear.ClientID%>').val(Phone);

                 $('#<%=txtPriceYear.ClientID%>').prop("disabled", true);
                 var wnd = $find('<%=RadWindowPrice.ClientID %>');
                 wnd.set_title("Edit Test Pricing");

                 wnd.set_height(450);
                 wnd.Show();

                 Materialize.updateTextFields();
            }
            else {
                ChkWarning();
            }
        }
        function ClosePriceWindow() {
            var wnd = $find('<%=RadWindowPrice.ClientID %>');
             wnd.Close();
           }
           function updatePriceDetail() {


               var ischeck = $('#<%=chkChargeablePriceYear.ClientID%>').is(":checked");
              if (!ischeck) {
                  $('#<%=txtDefaultAmountYear.ClientID%>').val(0);
                  $('#<%=txtOverrideAmountYear.ClientID%>').val(0);

                  $('#<%=txtDefaultAmountYear.ClientID%>').prop("disabled", true);
                  $('#<%=txtOverrideAmountYear.ClientID%>').prop("disabled", true);
              } else {
                    $('#<%=txtDefaultAmountYear.ClientID%>').prop("disabled", false);
                  $('#<%=txtOverrideAmountYear.ClientID%>').prop("disabled", false);
                
              }
             
           }

         
            function aceTestType_itemSelected(sender, e) {

            var obj = document.getElementById('<%= auto_hdnScheduleWorker.ClientID %>');
            obj.value = e.get_text();
        }
        function EmptyValue(txt) {
            if ($(txt).val() == '') { $('#<%= auto_hdnScheduleWorker.ClientID %>').val(''); }
        }
       </script>

</asp:Content>
