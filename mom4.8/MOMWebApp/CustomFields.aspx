<%@ Page Title="Custom Labels || MOM" Language="C#" MasterPageFile="~/Mom.master" AutoEventWireup="true" Inherits="CustomFields" CodeBehind="CustomFields.aspx.cs" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <!--Grid Control-->
    <link href="Design/css/grid.css" rel="stylesheet" />
    <link href="Design/css/pikaday.css" rel="stylesheet" />
    <!--File Upload Control-->
    <link href="Design/js/plugins/dropify/css/dropify.css" type="text/css" rel="stylesheet" media="screen,projection">

    <style>
        .RadGrid_Popup > div > div.rgDataDiv {
            height: 450px !important;
        }

        input[class*='txtMembers_'] {
            cursor: pointer;
        }

        .chipUsers {
            width: auto !important;
            padding-left: 5px !important;
            padding-right: 5px !important;
            margin-left: 2px !important;
            margin-right: 2px !important;
            margin-top: 3px !important;
        }

        .chipUserRoles {
            background-color: #2bab54 !important;
            width: auto !important;
            padding-left: 5px !important;
            padding-right: 5px !important;
            margin-left: 2px !important;
            margin-right: 2px !important;
            margin-top: 3px !important;
        }

        .chip > label {
            font-size: 13px;
            font-weight: normal;
            color: #fff;
            line-height: 24px;
        }

        /* The container */
        .cusCheckContainer {
            display: block;
            position: relative;
            padding-left: 19px;
            /* margin-bottom: 12px; */
            cursor: pointer;
            font-size: 15px;
            -webkit-user-select: none;
            -moz-user-select: none;
            -ms-user-select: none;
            user-select: none;
        }

            /* Hide the browser's default checkbox */
            .cusCheckContainer input {
                position: absolute;
                opacity: 0;
                cursor: pointer;
                height: 0;
                width: 0;
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
        .tag-div{
            margin-bottom: 0px!important;
            margin-top: 0px!important;
            height:2rem!important;
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
        
    </style>

    <script type="text/javascript">
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
                            if (intUserType == 0 || intUserType == 1 || intUserType == 6) {
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
            var txtMembers = $("#<%=gvCustom.ClientID %> input[id*='txtMembers']");
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
                                if (tempTeamKeyArr.length == 3 && tempTeamKeyArr[2] == "1") {
                                    tag = "<div class='chip chipUserRoles'><label  class='cusCheckContainer'>" + teamArr[i] + "<input type='checkbox' checked='checked' disabled><span class='checkmark'></span></label></div>";
                                }
                                else
                                    tag = "<div class='chip chipUserRoles'><label  class='cusCheckContainer'>" + teamArr[i] + "<input type='checkbox' disabled><span class='checkmark'></span></label></div>";
                                div.innerHTML += tag;
                            } else {
                                if (tempTeamKeyArr.length == 3 && (tempTeamKeyArr[0] == "0" || tempTeamKeyArr[0] == "1") && tempTeamKeyArr[2] == "1")
                                    tag = "<div class='chip chipUsers'><label  class='cusCheckContainer'>" + teamArr[i] + "<input type='checkbox' checked='checked' disabled><span class='checkmark'></span></label></div>";
                                else
                                    tag = "<div class='chip chipUsers'><label  class='cusCheckContainer'>" + teamArr[i] + "<input type='checkbox' disabled><span class='checkmark'></span></label></div>";

                                div.innerHTML += tag;
                            }
                        }
                    }
                }
            });
        }

        function CloseTeamMemberWindow() {
            debugger
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

                    if (teamArr != null && teamArr.length > 0) {
                        for (var i = 0; i < teamArr.length; i++) {
                            var tempTeamKeyArr = teamKeyArr[i].toString().split('_');
                            var tag = "";
                            if (teamKeyArr[i].indexOf('6') == 0) {
                                if (tempTeamKeyArr.length == 3 && tempTeamKeyArr[2] == "1")
                                    tag = "<div class='chip chipUserRoles'><label  class='cusCheckContainer'>" + teamArr[i] + "<input type='checkbox' checked='checked' disabled><span class='checkmark'></span></label></div>";
                                else
                                    tag = "<div class='chip chipUserRoles'><label  class='cusCheckContainer'>" + teamArr[i] + "<input type='checkbox' disabled><span class='checkmark'></span></label></div>";
                                div.innerHTML += tag;
                            } else {
                                if (tempTeamKeyArr.length == 3 && (tempTeamKeyArr[0] == "0" || tempTeamKeyArr[0] == "1") && tempTeamKeyArr[2] == "1")
                                    tag = "<div class='chip chipUsers'><label  class='cusCheckContainer'>" + teamArr[i] + "<input type='checkbox' checked='checked' disabled><span class='checkmark'></span></label></div>";
                                else
                                    tag = "<div class='chip chipUsers'><label  class='cusCheckContainer'>" + teamArr[i] + "<input type='checkbox' disabled><span class='checkmark'></span></label></div>";
                                div.innerHTML += tag;
                            }
                        }
                    }
                }
            }
            var wnd = $find('<%=TeamMembersWindow.ClientID %>');
            wnd.Close();
        }

        function ShowTeamMemberWindow(txtTeamMember) {
            debugger
            var line = $(txtTeamMember).closest('tr').find('td:eq(0) > span.customline').text();
            var txtTeamMembersId = $(txtTeamMember).attr("id");
            var hdnTeamMembersId = txtTeamMembersId.replace("cusLabelTag", "hdnMembers");
            var teamMembers = $("#" + hdnTeamMembersId).val();
            var txtCustomLabelId = txtTeamMembersId.replace("cusLabelTag", "lblDesc");
            var txtCustomLabelVal = $("#" + txtCustomLabelId).val();
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

                        if (intUserType == 0 || intUserType == 1 || intUserType == 6) {
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
                $("#<%=RadGrid_Emails.ClientID%> input[id*='chkTask']:checkbox").prop('checked', false);
                $("#<%=RadGrid_Emails.ClientID%> input[id*='chkTask']:checkbox").prop('disabled', true);
            }

            var wnd = $find('<%=TeamMembersWindow.ClientID %>');
            wnd.set_title("Team Member: " + txtCustomLabelVal);
            wnd.Show();
        }

        function CheckEmailsCheckBox(gridview) {
            debugger
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
                    if (intUserType == 0 || intUserType == 1 || intUserType == 6) {
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
            debugger
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

    <%--User Custom--%>
    <script>

        function ShowTeamMemberWindow_uc(txtTeamMember) {

            var line = $(txtTeamMember).closest('tr').find('td:eq(0) > span.customline').text();
            var txtTeamMembersId = $(txtTeamMember).attr("id");
            var hdnTeamMembersId = txtTeamMembersId.replace("cusLabelTag", "hdnMembers");
            var teamMembers = $("#" + hdnTeamMembersId).val();
            var txtCustomLabelId = txtTeamMembersId.replace("cusLabelTag", "lblDesc");
            var txtCustomLabelVal = $("#" + txtCustomLabelId).val();
            var txtTeamMemberDispId = txtTeamMembersId.replace("cusLabelTag", "txtMembers");
            var txtTeamMemberDispVal = $("#" + txtTeamMemberDispId).val();

            $('#<%= hdnLineOpenned_uc.ClientID%>').val(line);
            $('#<%= hdnOrgMemberKey_uc.ClientID%>').val(teamMembers);
            $('#<%= hdnOrgMemberDisp_uc.ClientID%>').val(txtTeamMemberDispVal);

            $("#<%=RadGrid_Emails_uc.ClientID%> input[id*='chkSelect']:checkbox").unbind('click').bind('click', function () {
                CheckUncheckAllCheckBoxAsNeeded_uc();
            });

            $("#<%=RadGrid_Emails_uc.ClientID%> input[id*='chkTask']:checkbox").unbind('click').bind('click', function () {
                OnCheck_TaskCheckBox_uc('<%=RadGrid_Emails_uc.ClientID%>');
            });

            $("#<%=RadGrid_Emails_uc.ClientID%> input[id*='chkAll']:checkbox").unbind('click').bind('click', function () {
                if ($(this).is(':checked')) {
                    $("#<%=RadGrid_Emails_uc.ClientID%> input[id*='chkSelect']:checkbox").prop('checked', true);
                }
                else {
                    $("#<%=RadGrid_Emails_uc.ClientID%> input[id*='chkSelect']:checkbox").prop('checked', false);
                }
                CheckEmailsCheckBox_uc('<%=RadGrid_Emails_uc.ClientID%>');
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

                $("#<%=RadGrid_Emails_uc.ClientID%> input[id*='chkSelect']:checkbox").each(function (index) {
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

                        if (intUserType == 0 || intUserType == 1 || intUserType == 6) {
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
                $("#<%=RadGrid_Emails_uc.ClientID%> input[id*='chkSelect']:checkbox").prop('checked', false);
            }

            var wnd = $find('<%=TeamMembersWindow_uc.ClientID %>');
            wnd.set_title("Team Member: " + txtCustomLabelVal);
            wnd.Show();
        }
        function CheckEmailsCheckBox_uc(gridview) {
            var line = $("#<%= hdnLineOpenned_uc.ClientID%>").val();
            var hdnMembersID = $(".txtMembers_uc_" + line).attr("id").replace("txtMembers", "hdnMembers");
            var hdnMembersValue = $("#<%= hdnOrgMemberKey_uc.ClientID%>").val();
            var txtMembersValue = $("#<%= hdnOrgMemberDisp_uc.ClientID%>").val();
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
                    if (intUserType == 0 || intUserType == 1 || intUserType == 6) {
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

            $("#<%= hdnOrgMemberKey_uc.ClientID%>").val(tempArrayKey.join(";"));
            $("#<%= hdnOrgMemberDisp_uc.ClientID%>").val(tempArrayDisplay.join(";"));
        }

        function CheckUncheckAllCheckBoxAsNeeded_uc() {
            var totalCheckboxes = $("#<%=RadGrid_Emails_uc.ClientID%> input[id*='chkSelect']:checkbox").size();

            var checkedCheckboxes = $("#<%=RadGrid_Emails_uc.ClientID%> input[id*='chkSelect']:checkbox:checked").size();

            if (totalCheckboxes == checkedCheckboxes) {

                $("#<%=RadGrid_Emails_uc.ClientID %> input[id*='chkAll']:checkbox").prop('checked', true);//.each(function () { this.checked = true; });
            }
            else {
                $("#<%=RadGrid_Emails_uc.ClientID %> input[id*='chkAll']:checkbox").prop('checked', false);//.attr('checked', false);
            }

            if ($('#<%=RadGrid_Emails_uc.ClientID%>').length > 0) {
                CheckEmailsCheckBox_uc('<%=RadGrid_Emails_uc.ClientID%>');
            }
        }

        function OnCheck_TaskCheckBox_uc(gridview) {
            var line = $("#<%= hdnLineOpenned_uc.ClientID%>").val();
            var hdnMembersID = $(".txtMembers_uc_" + line).attr("id").replace("txtMembers", "hdnMembers");
            var hdnMembersValue = $("#<%= hdnOrgMemberKey_uc.ClientID%>").val();

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

            $("#<%= hdnOrgMemberKey_uc.ClientID%>").val(tempArrayKey.join(";"));
            //alert(tempArrayKey);
        }

        function BindClickEventForGridCheckBox_uc() {

            $("#<%=RadGrid_Emails_uc.ClientID%> input[id*='chkSelect']:checkbox").unbind('click').bind('click', function () {
                CheckUncheckAllCheckBoxAsNeeded_uc();
            });

            $("#<%=RadGrid_Emails_uc.ClientID%> input[id*='chkAll']:checkbox").unbind('click').bind('click', function () {
                if ($(this).is(':checked')) {
                    $("#<%=RadGrid_Emails_uc.ClientID%> input[id*='chkSelect']:checkbox").prop('checked', true);
                }
                else {
                    $("#<%=RadGrid_Emails_uc.ClientID%> input[id*='chkSelect']:checkbox").prop('checked', false);
                }
                CheckEmailsCheckBox_uc('<%=RadGrid_Emails_uc.ClientID%>');
            });

            UpdatedivDisplayTeamMember_uc();
            var line = $("#<%= hdnLineOpenned_uc.ClientID%>").val();
            if (line != null && line != '') {
                var hdnMembersID = $(".txtMembers_uc_" + line).attr("id").replace("txtMembers", "hdnMembers");
                var teamMembers = $("#" + hdnMembersID).val();

                // Update selected for grid
                if (teamMembers != null && teamMembers != "") {
                    var teamArr = teamMembers.toString().split(';');
                    // trim value of teamArr
                    $.each(teamArr, function (index, value) {
                        teamArr[index] = value.trim();//.replace("0_", "").replace("1_", "").replace("3_", "").replace("4_", "");
                    });

                    $("#<%=RadGrid_Emails_uc.ClientID%> input[id*='chkSelect']:checkbox").each(function (index) {

                        var userId = $(this).closest('tr').find('td:eq(0)').find('span').html();
                        //userId = userId.replace("0_", "").replace("1_", "").replace("3_", "").replace("4_", "");
                        if (teamArr.indexOf(userId) >= 0) {
                            $(this).prop('checked', true);

                        } else {
                            $(this).prop('checked', false);
                        }
                    });
                } else {
                    $("#<%=RadGrid_Emails_uc.ClientID%> input[id*='chkSelect']:checkbox").prop('checked', false);
                }
            }
        }

        function UpdatedivDisplayTeamMember_uc() {
            var txtMembers = $("#<%=gvUserCustom.ClientID %> input[id*='txtMembers']");
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
                                if (tempTeamKeyArr.length == 3 && tempTeamKeyArr[2] == "1") {

                                    tag = "<div class='chip chipUserRoles'><label  class='cusCheckContainer'>" + teamArr[i] + "<input type='checkbox' checked='checked' disabled><span class='checkmark'></span></label></div>";
                                }
                                else

                                    tag = "<div class='chip chipUserRoles'><label  class='cusCheckContainer'>" + teamArr[i] + "<input type='checkbox' disabled><span class='checkmark'></span></label></div>";
                                div.innerHTML += tag;
                            } else {
                                if (tempTeamKeyArr.length == 3 && (tempTeamKeyArr[0] == "0" || tempTeamKeyArr[0] == "1") && tempTeamKeyArr[2] == "1")

                                    tag = "<div class='chip chipUsers'><label  class='cusCheckContainer'>" + teamArr[i] + "<input type='checkbox' checked='checked' disabled><span class='checkmark'></span></label></div>";
                                else

                                    tag = "<div class='chip chipUsers'><label  class='cusCheckContainer'>" + teamArr[i] + "<input type='checkbox' disabled><span class='checkmark'></span></label></div>";

                                div.innerHTML += tag;
                            }
                        }
                    }
                }
            });
        }

        function CloseTeamMemberWindow_uc() {
            var line = $("#<%= hdnLineOpenned_uc.ClientID%>").val();
            if (line != null && line != '') {
                var hdnMembersValue = $("#<%= hdnOrgMemberKey_uc.ClientID%>").val();
                var txtMembersValue = $("#<%= hdnOrgMemberDisp_uc.ClientID%>").val();
                //alert(hdnMembersValue);
                //alert(txtMembersValue);
                var txtMembersID = $(".txtMembers_uc_" + line).attr("id");
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

                    if (teamArr != null && teamArr.length > 0) {
                        for (var i = 0; i < teamArr.length; i++) {
                            var tempTeamKeyArr = teamKeyArr[i].toString().split('_');
                            var tag = "";
                            if (teamKeyArr[i].indexOf('6') == 0) {
                                if (tempTeamKeyArr.length == 3 && tempTeamKeyArr[2] == "1")
                                    tag = "<div class='chip chipUserRoles'><label  class='cusCheckContainer'>" + teamArr[i] + "<input type='checkbox' checked='checked' disabled><span class='checkmark'></span></label></div>";
                                else
                                    tag = "<div class='chip chipUserRoles'><label  class='cusCheckContainer'>" + teamArr[i] + "<input type='checkbox' disabled><span class='checkmark'></span></label></div>";
                                div.innerHTML += tag;
                            } else {
                                if (tempTeamKeyArr.length == 3 && (tempTeamKeyArr[0] == "0" || tempTeamKeyArr[0] == "1") && tempTeamKeyArr[2] == "1")

                                    tag = "<div class='chip chipUsers'><label  class='cusCheckContainer'>" + teamArr[i] + "<input type='checkbox' checked='checked' disabled><span class='checkmark'></span></label></div>";
                                else
                                    tag = "<div class='chip chipUsers'><label  class='cusCheckContainer'>" + teamArr[i] + "<input type='checkbox' disabled><span class='checkmark'></span></label></div>";

                                div.innerHTML += tag;
                            }
                        }
                    }
                }
            }
            var wnd = $find('<%=TeamMembersWindow_uc.ClientID %>');
            wnd.Close();
        }

        function validateGridWorkfolow() {
           <%-- var grid = $find("<%=gvUserCustom.ClientID %>");

            var isEmptyLabel = true;
             $("#<%=gvUserCustom.ClientID %>").find('tr:not(:first,:last)').each(function () {
                try {
                    var $tr = $(this);
                    var temp = $tr.find('input[id*=txtLabel]').val();                   
                    if (temp!=undefined &&  temp == "") {
                          isEmptyLabel= false;
                      
                    }                   
                } catch (ex) { }           

            });  

            if (isEmptyLabel == false) {
                noty({
                    text: 'Please input custom field label.',
                    type: 'warning',
                    layout: 'topCenter',
                    closeOnSelfClick: false,
                    timeout: 5000,
                    theme: 'noty_theme_default',
                    closable: true
                });
                return false;
            } else {
                return true;
            }--%>


        }


        function HideME() {
            $("#DivEqup").hide();
        }

<%--        function SelectRowsUser() {
            //debugger
            var Name = document.getElementById("<%=txtUnit.ClientID %>");
            var div = document.getElementById('eqtag');
            div.innerHTML = '';
            Name.value = '';

            var grid = $find("<%=RadGrid_StageDepartment.ClientID %>");
            var masterTable = grid.get_masterTableView();
            if (masterTable != null) {
                for (var i = 0; i < masterTable.get_dataItems().length; i++) {
                    var gridItemElement = masterTable.get_dataItems()[i].findElement("chkSelect");
                    var lblUnit = masterTable.get_dataItems()[i].findElement("lblUnit");
                    var lblID = masterTable.get_dataItems()[i].findElement("lblID");
                    //var lblTypeid = masterTable.get_dataItems()[i].findElement("lblTypeid");
                    //var ddlApplyUserRolePermission = masterTable.get_dataItems()[i].findElement("ddlApplyUserRolePermission");
                    if (gridItemElement.checked) {
                        if (Name.value != '') {
                            Name.value = Name.value + ', ' + lblID.innerHTML;
                        }
                        else {
                            Name.value = lblID.innerHTML;
                        }

                        var tag = "<div class='chip' style='width:auto !important;padding-left:5px !important;padding-right:5px !important ;margin-left:2px !important ;margin-right:2px !important ;margin-top:3px !important ;'>" + lblUnit.innerHTML + "</div>"

                        div.innerHTML += tag;
                    }
                }
            }
        }--%>

<%--        function EqCheckBOX(checked) {
            var grid = $find("<%=RadGrid_StageDepartment.ClientID %>");
            var masterTable = grid.get_masterTableView();
            if (masterTable != null) {
                for (var i = 0; i < masterTable.get_dataItems().length; i++) {
                    var gridItemElement = masterTable.get_dataItems()[i].findElement("chkSelect");
                    var lblUnit = masterTable.get_dataItems()[i].findElement("lblUnit");
                    gridItemElement.checked = checked;
                }
            }
        }--%>

</script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <telerik:RadWindow ID="TeamMembersWindow_uc" Skin="Material" VisibleTitlebar="true" Behaviors="Default" CenterIfModal="true"
        Animation="FlyIn" AnimationDuration="200" RenderMode="Auto" VisibleStatusbar="false"
        runat="server" Modal="true" Width="1050" Height="635">
        <ContentTemplate>
            <telerik:RadAjaxPanel ID="RadAjaxPanel34" runat="server">
                <div style="margin-top: 15px;">
                    <div class="form-section-row">
                        <div class="form-section">
                            <div class="row" style="margin-bottom: 0;">
                                <div class="grid_container" id="divMemberGrid_uc" runat="server">
                                    <div class="RadGrid RadGrid_Material RadGrid RadGrid_Popup">

                                        <telerik:RadGrid RenderMode="Auto" ID="RadGrid_Emails_uc" AllowFilteringByColumn="true" ShowFooter="false" PageSize="1000"
                                            ShowStatusBar="true" runat="server" AllowPaging="false" AllowSorting="true" PagerStyle-AlwaysVisible="true" FilterType="CheckList"
                                            AllowCustomPaging="false" Width="100%" Height="516px" OnPreRender="RadGrid_Emails_uc_PreRender"
                                            OnNeedDataSource="RadGrid_Emails_uc_NeedDataSource">
                                            <CommandItemStyle />
                                            <GroupingSettings CaseSensitive="false" />
                                            <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                                                <Selecting AllowRowSelect="True"></Selecting>

                                                <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                                <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                            </ClientSettings>
                                            <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="True">
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
                                                        DataField="RoleName" SortExpression="RoleName" AutoPostBackOnFilter="true" DataType="System.String"
                                                        CurrentFilterFunction="Contains" HeaderText="User Role" ShowFilterIcon="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblUserRole" runat="server"><%#Eval("RoleName")%></asp:Label>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn SortExpression="IsTask" AllowFiltering="false" HeaderStyle-Width="100" ShowFilterIcon="false" DataField="IsTask" HeaderText="Task">
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
                                <a id="lnkPopupOK_uc" onclick="CloseTeamMemberWindow_uc();" style="cursor: pointer;">OK</a>

                            </div>
                        </div>
                    </div>


                </div>
            </telerik:RadAjaxPanel>
        </ContentTemplate>
    </telerik:RadWindow>

    <div class="divbutton-container">
        <div id="divButtons" class="">
            <div id="breadcrumbs-wrapper">

                <header>
                    <div class="container row-color-grey">
                        <div class="row">
                            <div class="col s12 m12 l12">
                                <div class="row">
                                    <div class="page-title"><i class="mdi-action-trending-up"></i>&nbsp;Custom Labels</div>
                                    <div class="buttonContainer">
                                        <div class="btnlinks">
                                            <asp:LinkButton CssClass="icon-save" ID="btnSubmit" runat="server" TabIndex="27" ToolTip="Save" OnClick="btnSubmit_Click">Save</asp:LinkButton>
                                        </div>
                                    </div>
                                    <div class="btnclosewrap">
                                        <asp:LinkButton ID="lnkClose" runat="server" CausesValidation="false" OnClick="lnkClose_Click"><i class="mdi-content-clear"></i></asp:LinkButton>
                                    </div>
                                    <div class="rght-content">
                                    </div>
                                </div>
                            </div>

                        </div>
                    </div>
                </header>
            </div>



            <div class="container breadcrumbs-bg-custom">
                <div class="row">
                    <div class="col s7 m7 l7">
                        <div class="row">
                            <div class="tblnks">
                                <ul class="anchor-links">
                                    <li><a href="#accrdCustomers" class="link-slide">Customers</a></li>
                                    <li><a href="#accrdLoc" class="link-slide">Locations</a></li>
                                    <li><a href="#accrdTicket" class="link-slide">Tickets</a></li>
                                    <li><a href="#accrdTest" class="link-slide">Test</a></li>
                                    <li><a href="#accrdProject" class="link-slide">Project</a></li>
                                    <li><a href="#accrdPO" class="link-slide">PO</a></li>
                                    <li><a href="#accrdEstimate" class="link-slide">Estimate</a></li>
                                    <li><a href="#accrdUsers" class="link-slide">Users</a></li>
                                </ul>
                            </div>

                        </div>
                    </div>
                    <%--                    <div class="col s5 m5 l5">
                        <div class="row">
                            <div class="rght-content">
                                <div class="editlabel">
                                    <span>Total Fields: <asp:Label ID="lblTotalLabel" runat="server"></asp:Label></span>
                                </div>
                            </div>

                        </div>
                    </div>--%>
                </div>
            </div>
        </div>
    </div>

    <div class="container accordian-wrap">
        <div class="row">
            <div class="col s12 m12 l12">
                <div class="row">
                    <ul class="collapsible popout collapsible-accordion form-accordion-head" data-collapsible="expandable">
                        <li class="active">
                            <div id="accrdCustomers" class="collapsible-header accrd active accordian-text-custom"><i class="mdi-social-people"></i>Customers</div>
                            <div class="collapsible-body" style="display: block;">
                                <div class="form-content-wrap">
                                    <div class="form-content-pd">
                                        <div class="form-section-row">
                                            <div class="section-ttle">Customer Custom Fields</div>
                                            <div class="form-section3">
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <asp:TextBox ID="txtCstOwner1" runat="server" CssClass="validate" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                                        <label for="txtCstOwner1">Custom 1</label>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="form-section3-blank">
                                                &nbsp;
                                            </div>
                                            <div class="form-section3">
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <asp:TextBox ID="txtCstOwner2" runat="server" CssClass="validate" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                                        <label for="txtCstOwner2">Custom 2</label>
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
                        <li>
                            <div id="accrdLoc" class="collapsible-header accrd accordian-text-custom"><i class="mdi-action-perm-data-setting"></i>Locations</div>
                            <div class="collapsible-body">
                                <div class="form-content-wrap">
                                    <div class="form-content-pd">
                                        <div class="form-section-row">
                                            <div class="section-ttle">Location Custom Fields</div>
                                            <div class="form-section3">
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <asp:TextBox ID="txtCstLoc1" runat="server" CssClass="validate" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                                        <label for="Lcstm1">Custom 1</label>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="form-section3-blank">
                                                &nbsp;
                                            </div>
                                            <div class="form-section3">
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <asp:TextBox ID="txtCstLoc2" runat="server" CssClass="validate" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                                        <label for="tcstm2">Custom 2</label>
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
                        <li>
                            <div id="accrdTicket" class="collapsible-header accrd  accordian-text-custom "><i class="mdi-social-poll"></i>Tickets</div>
                            <div class="collapsible-body">
                                <div class="form-content-wrap">
                                    <div class="form-content-pd">

                                        <div class="form-section-row">
                                            <div class="form-section3">
                                                <div class="section-ttle">Text Fields</div>
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <asp:TextBox ID="txtCst1" runat="server" CssClass="validate" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                                        <label for="txtCst1">Custom 1</label>
                                                    </div>
                                                </div>
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <asp:TextBox ID="txtCst2" runat="server" CssClass="validate" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                                        <label for="TextBox2">Custom 2</label>
                                                    </div>
                                                </div>
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <asp:TextBox ID="txtCst3" runat="server" CssClass="validate" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                                        <label for="cstm3">Custom 3</label>
                                                    </div>
                                                </div>
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <asp:TextBox ID="txtCst4" runat="server" CssClass="validate" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                                        <label for="cstm4">Custom 4</label>
                                                    </div>
                                                </div>
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <asp:TextBox ID="txtCst5" runat="server" CssClass="validate" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                                        <label for="cstm5">Custom 5</label>
                                                    </div>
                                                </div>

                                            </div>
                                            <div class="form-section3-blank">
                                                &nbsp;
                                            </div>
                                            <div class="form-section3">
                                                <div class="section-ttle">Checkbox Fields</div>
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <asp:TextBox ID="txtCst6" runat="server" CssClass="validate" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                                        <label for="cstm6">Custom 6</label>
                                                    </div>
                                                </div>
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <asp:TextBox ID="txtCst7" runat="server" CssClass="validate" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                                        <label for="cstm7">Custom 7</label>
                                                    </div>
                                                </div>
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <asp:TextBox ID="txtCst8" runat="server" CssClass="validate" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                                        <label for="cstm8">Custom 8</label>
                                                    </div>
                                                </div>
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <asp:TextBox ID="txtCst9" runat="server" CssClass="validate" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                                        <label for="cstm9">Custom 9</label>
                                                    </div>
                                                </div>
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <asp:TextBox ID="txtCst10" runat="server" CssClass="validate" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                                        <label for="cstm10">Custom 10</label>
                                                    </div>
                                                </div>

                                            </div>

                                            <div class="form-section3-blank">
                                                &nbsp;
                                            </div>
                                            <div class="form-section3">
                                                <div class="section-ttle">Ticket Custom Fields</div>
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <asp:TextBox ID="txtTickCst1" runat="server" CssClass="validate" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                                        <label for="tcstm">Ticket Custom 1</label>
                                                    </div>
                                                </div>
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <asp:TextBox ID="txtTickCst2" runat="server" CssClass="validate" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                                        <label for="tcstm1">Ticket Custom 2</label>
                                                    </div>
                                                </div>
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <asp:TextBox ID="txtTickCst3" runat="server" CssClass="validate" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                                        <label for="tcstm2">Ticket Custom 3</label>
                                                    </div>
                                                </div>
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <asp:TextBox ID="txtTickCst4" runat="server" CssClass="validate" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                                        <label for="tcstm3">Ticket Custom 4</label>
                                                    </div>
                                                </div>
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <asp:TextBox ID="txtTickCst5" runat="server" CssClass="validate" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                                        <label for="tcstm4">Ticket Custom 5</label>
                                                    </div>
                                                </div>

                                            </div>
                                        </div>

                                        <div class="cf"></div>
                                    </div>
                                </div>
                            </div>
                        </li>
                        <li>
                            <div id="accrdTest" class="collapsible-header accrd  accordian-text-custom "><i class="mdi-social-poll"></i>Test</div>
                            <div class="collapsible-body">
                                <div class="form-content-wrap">
                                    <div class="form-content-pd">

                                        <div class="form-section-row">
                                            <div class="section-ttle">Test Custom Fields</div>
                                            <div class="form-section3">
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <asp:TextBox ID="txtTestCst1" runat="server" CssClass="validate" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                                        <label for="txtTestCst1">Custom 1</label>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="form-section3-blank">
                                                &nbsp;
                                            </div>
                                            <div class="form-section3">
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <asp:TextBox ID="txtTestCst2" runat="server" CssClass="validate" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                                        <label for="txtTestCst2">Custom 2</label>
                                                    </div>
                                                </div>
                                            </div>

                                        </div>
                                        <div class="form-section-row">
                                            <div class="form-section3">
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <asp:TextBox ID="txtTestCst3" runat="server" CssClass="validate" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                                        <label for="txtTestCst3">Custom 3</label>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="form-section3-blank">
                                                &nbsp;
                                            </div>
                                            <div class="form-section3">
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <asp:TextBox ID="txtTestCst4" runat="server" CssClass="validate" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                                        <label for="txtTestCst4">Custom 4</label>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="cf"></div>
                                    </div>
                                </div>
                            </div>
                        </li>
                        <li>
                            <div id="accrdProject" class="collapsible-header accrd  accordian-text-custom "><i class="mdi-social-poll"></i>Project</div>
                            <div class="collapsible-body">
                                <div class="form-content-wrap">
                                    <div class="form-content-pd">
                                        <div class="form-section-row">
                                            <div class="section-ttle">Project Custom Fields</div>
                                            <div class="form-section4">
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <asp:TextBox ID="txtJobCust1" runat="server" class="filled-in" />
                                                        <label id="lblJobCust1" runat="server" for="txtJobCust1"  >Custom 1</label>
                                                    </div>
                                                </div>

                                            </div>
                                            <div class="form-section3-blank">
                                                &nbsp;
                                            </div>
                                            <div class="form-section4">
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <asp:TextBox ID="txtJobCust2" runat="server" class="filled-in" />
                                                        <label id="lblJobCust2" runat="server" for="txtJobCust2"  >Custom 2</label>
                                                    </div>
                                                </div>

                                            </div>
                                            <div class="form-section3-blank">
                                                &nbsp;
                                            </div>
                                            <div class="form-section4">
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <asp:TextBox ID="txtJobCust3" runat="server" class="filled-in" />
                                                        <label id="lblJobCust3" runat="server" for="txtJobCust3"  >Custom 3</label>
                                                    </div>
                                                </div>

                                            </div>
                                            <div class="form-section3-blank">
                                                &nbsp;
                                            </div>
                                            <div class="form-section4">
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <asp:TextBox ID="txtJobCust4" runat="server" class="filled-in" />
                                                        <label id="lblJobCust4" runat="server" for="txtJobCust4"  >Custom 4</label>
                                                    </div>
                                                </div>

                                            </div>
                                        </div>
                                        <div class="form-section-row">
                                            <div class="form-section4">
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <asp:TextBox ID="txtJobCust5" runat="server" class="filled-in" />
                                                        <label id="lblJobCust5" runat="server" for="txtJobCust5"  >Custom 5</label>
                                                    </div>
                                                </div>

                                            </div>
                                            <div class="form-section3-blank">
                                                &nbsp;
                                            </div>
                                            <div class="form-section4">
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <asp:TextBox ID="txtJobCust6" runat="server" class="filled-in" />
                                                        <label id="lblJobCust6" runat="server" for="txtJobCust6"  >Custom 6</label>
                                                    </div>
                                                </div>

                                            </div>
                                            <div class="form-section3-blank">
                                                &nbsp;
                                            </div>
                                            <div class="form-section4">
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <asp:TextBox ID="txtJobCust7" runat="server" class="filled-in" />
                                                        <label id="lblJobCust7" runat="server" for="txtJobCust7"  >Custom 7</label>
                                                    </div>
                                                </div>

                                            </div>
                                            <div class="form-section3-blank">
                                                &nbsp;
                                            </div>
                                            <div class="form-section4">
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <asp:TextBox ID="txtJobCust8" runat="server" class="filled-in" />
                                                        <label id="lblJobCust8" runat="server" for="txtJobCust8"  >Custom 8</label>
                                                    </div>
                                                </div>

                                            </div>
                                        </div>
                                        <div class="form-section-row">
                                            <div class="form-section4">
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <asp:TextBox ID="txtJobCust9" runat="server" class="filled-in" />
                                                        <label id="lblJobCust9" runat="server" for="txtJobCust9"  >Custom 9</label>
                                                    </div>
                                                </div>

                                            </div>
                                            <div class="form-section3-blank">
                                                &nbsp;
                                            </div>
                                            <div class="form-section4">
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <asp:TextBox ID="txtJobCust10" runat="server" class="filled-in" />
                                                        <label id="lblJobCust10" runat="server" for="txtJobCust10"  >Custom 10</label>
                                                    </div>
                                                </div>

                                            </div>
                                            <div class="form-section3-blank">
                                                &nbsp;
                                            </div>
                                            <div class="form-section4">
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <asp:TextBox ID="txtJobCust11" runat="server" class="filled-in" />
                                                        <label id="lblJobCust11" runat="server" for="txtJobCust11"  >Custom 11</label>
                                                    </div>
                                                </div>

                                            </div>
                                            <div class="form-section3-blank">
                                                &nbsp;
                                            </div>
                                            <div class="form-section4">
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <asp:TextBox ID="txtJobCust12" runat="server" class="filled-in" />
                                                        <label id="lblJobCust12" runat="server" for="txtJobCust12"  >Custom 12</label>
                                                    </div>
                                                </div>

                                            </div>
                                        </div>
                                        <div class="form-section-row">
                                            <div class="form-section4">
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <asp:TextBox ID="txtJobCust13" runat="server" class="filled-in" />
                                                        <label id="lblJobCust13" runat="server" for="txtJobCust13"  >Custom 13</label>
                                                    </div>
                                                </div>

                                            </div>
                                            <div class="form-section3-blank">
                                                &nbsp;
                                            </div>
                                            <div class="form-section4">
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <asp:TextBox ID="txtJobCust14" runat="server" class="filled-in" />
                                                        <label id="lblJobCust14" runat="server" for="txtJobCust14"  >Custom 14</label>
                                                    </div>
                                                </div>

                                            </div>
                                            <div class="form-section3-blank">
                                                &nbsp;
                                            </div>
                                            <div class="form-section4">
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <asp:TextBox ID="txtJobCust15" runat="server" class="filled-in" />
                                                        <label id="lblJobCust15" runat="server" for="txtJobCust15"  >Custom 15</label>
                                                    </div>
                                                </div>

                                            </div>
                                            <div class="form-section3-blank">
                                                &nbsp;
                                            </div>
                                            <div class="form-section4">
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <asp:TextBox ID="txtJobCust16" runat="server" class="filled-in" />
                                                        <label id="lblJobCust16" runat="server" for="txtJobCust16"  >Custom 16</label>
                                                    </div>
                                                </div>

                                            </div>
                                        </div>
                                        <div class="form-section-row">
                                            <div class="form-section4">
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <asp:TextBox ID="txtJobCust17" runat="server" class="filled-in" />
                                                        <label id="lblJobCust17" runat="server" for="txtJobCust17"  >Custom 17</label>
                                                    </div>
                                                </div>

                                            </div>
                                            <div class="form-section3-blank">
                                                &nbsp;
                                            </div>
                                            <div class="form-section4">
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <asp:TextBox ID="txtJobCust18" runat="server" class="filled-in" />
                                                        <label id="lblJobCust18" runat="server" for="txtJobCust18"  >Custom 18</label>
                                                    </div>
                                                </div>

                                            </div>
                                            <div class="form-section3-blank">
                                                &nbsp;
                                            </div>
                                            <div class="form-section4">
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <asp:TextBox ID="txtJobCust19" runat="server" class="filled-in" />
                                                        <label id="lblJobCust19" runat="server" for="txtJobCust19"  >Custom 19</label>
                                                    </div>
                                                </div>

                                            </div>
                                            <div class="form-section3-blank">
                                                &nbsp;
                                            </div>
                                            <div class="form-section4">
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <asp:TextBox ID="txtJobCust20" runat="server" class="filled-in" />
                                                        <label id="lblJobCust20" runat="server" for="txtJobCust20"  >Custom 20</label>
                                                    </div>
                                                </div>

                                            </div>
                                        </div>
                                        <div class="cf"></div>
                                    </div>



                                    <div class="form-content-pd">

                                        <div class="form-section-row">
                                            <div class="section-ttle">Attributes General Custom </div>
                                            <div class="form-section3">
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <asp:TextBox ID="txtAttributesGeneralCustom1" runat="server" CssClass="filled-in" />
                                                        <label for="txtAttributesGeneralCustom1">Custom 1</label>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="form-section3-blank">
                                                &nbsp;
                                            </div>
                                            <div class="form-section3">
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <asp:TextBox ID="txtAttributesGeneralCustom2" runat="server" CssClass="filled-in" />
                                                        <label for="txtAttributesGeneralCustom2">Custom 2</label>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="form-section3-blank">
                                                &nbsp;
                                            </div>
                                            <div class="form-section3">
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <asp:TextBox ID="txtAttributesGeneralCustom3" runat="server" CssClass="filled-in" />
                                                        <label for="txtAttributesGeneralCustom3">Custom 3</label>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-section-row">
                                            <div class="form-section3">
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <asp:TextBox ID="txtAttributesGeneralCustom4" runat="server" CssClass="filled-in" />
                                                        <label for="txtAttributesGeneralCustom4">Custom 4</label>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="form-section3-blank">
                                                &nbsp;
                                            </div>
                                            <div class="form-section3">
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <asp:TextBox ID="txtAttributesGeneralCustom5" runat="server" CssClass="filled-in" />
                                                        <label for="txtAttributesGeneralCustom5" style="top: -1px !important">Custom 5</label>
                                                        <%--<asp:CalendarExtender ID="txtAttributesGeneralCustom5_CalendarExtender" runat="server" Enabled="True"
                                                            TargetControlID="txtAttributesGeneralCustom5">
                                                        </asp:CalendarExtender>--%>
                                                    </div>
                                                </div>
                                            </div>

                                        </div>
                                        <div class="cf"></div>
                                    </div>


                                </div>
                            </div>
                        </li>
                        <li>
                            <div id="accrdPO" class="collapsible-header accrd  accordian-text-custom "><i class="mdi-social-people"></i>PO</div>
                            <div class="collapsible-body">
                                <div class="form-content-wrap">
                                    <div class="form-content-pd">
                                        <div class="form-section-row">
                                            <div class="section-ttle">PO Custom Fields</div>
                                            <div class="form-section3">
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <asp:TextBox ID="txtPO1" runat="server" CssClass="validate" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                                        <label for="txtPO1">Custom 1</label>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="form-section3-blank">
                                                &nbsp;
                                            </div>
                                            <div class="form-section3">
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <asp:TextBox ID="txtPO2" runat="server" CssClass="validate" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                                        <label for="txtPO2">Custom 2</label>
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
                        <li>
                            <div id="accrdEstimate" class="collapsible-header accrd  accordian-text-custom "><i class="mdi-maps-map"></i>Estimate</div>
                            <div class="collapsible-body">
                                <div class="form-content-wrap form-content-wrapwd">
                                    <div class="form-content-pd">
                                        <div class="grid_container">
                                            <div class="form-section-row m-b-0">
                                                <div class="RadGrid RadGrid_Material">
                                                    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                                        <ContentTemplate>
                                                            <asp:HiddenField ID="hdnLineOpenned" runat="server" />
                                                            <asp:HiddenField ID="hdnOrgMemberKey" runat="server" />
                                                            <asp:HiddenField ID="hdnOrgMemberDisp" runat="server" />
                                                            <asp:HiddenField ID="hdnOrgUserRoleID" runat="server" />
                                                            <asp:HiddenField ID="hdnOrgUserRoleDisp" runat="server" />
                                                            <asp:GridView ID="gvCustom" runat="server" AutoGenerateColumns="False" AlternatingRowStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HorizontalAlign="Center"
                                                                CssClass="BomGrid" Width="100%"
                                                                ShowFooter="true" HeaderStyle-Font-Size="0.9em" OnRowCommand="gvCustom_RowCommand"
                                                                OnRowDataBound="gvCustom_RowDataBound">
                                                                <Columns>
                                                                    <asp:TemplateField ItemStyle-Width="0.5%" HeaderStyle-Width="0.5%" FooterStyle-Width="0.5%">
                                                                        <HeaderTemplate>
                                                                            <asp:LinkButton ID="ibtnDeleteCItem" OnClientClick="return confirm('Are you sure you want to delete the items? This will delete the items from Workflow field.')"
                                                                                CausesValidation="false" ToolTip="Delete" runat="server" Style="color: #000; font-size: 1.5em;" Width="20px"
                                                                                OnClick="ibtnDeleteCItem_Click"><i class="mdi-navigation-cancel cancels" style="margin-left: -6px; color: #f00;font-size: 1.2em; font-weight: bold;"></i></asp:LinkButton>
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>

                                                                            <asp:HiddenField ID="txtRowLine" Value='<%# Eval("OrderNo") %>' runat="server"></asp:HiddenField>
                                                                            <%--<asp:HiddenField ID="hdnLine" runat="server" Value='<%# Eval("Line") %>'></asp:HiddenField>--%>
                                                                            <asp:Label ID="lblIndex" Visible="false" runat="server" Text="<%# Container.DataItemIndex +1 %>"></asp:Label>
                                                                            <asp:Label ID="lblID" runat="server" Text='<%# Eval("ID") %>' Visible="false"></asp:Label>
                                                                            <asp:Label ID="lblLine" runat="server" Text='<%# Eval("Line") %>' CssClass="customline" Style="display: none;"></asp:Label>
                                                                            <asp:CheckBox ID="chkSelect" CssClass="css-checkbox" Text=" " runat="server" />
                                                                        </ItemTemplate>
                                                                        <FooterTemplate>
                                                                            <asp:LinkButton ID="lnkAddnewRow" runat="server" CausesValidation="False" Style="color: #000; font-size: 1.5em;" Width="20px"
                                                                                ToolTip="Add New Row" OnClick="lnkAddnewRow_Click"><i class="mdi-content-add-circle" style="margin-left: -11px; color: #2bab54;font-size: 1.2em; font-weight: bold;"></i></asp:LinkButton>
                                                                        </FooterTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField ItemStyle-Width="20px">
                                                                        <ItemTemplate>
                                                                            <div class="handle" style="cursor: move" title="Move Up/Down">
                                                                                <img src="images/Dragdrop.png" width="20px" />
                                                                            </div>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Label" FooterStyle-VerticalAlign="Middle" HeaderStyle-Width="300">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="lblDesc" runat="server" Text='<%# Eval("Label") %>' MaxLength="255" Style='min-width: 100px!important;'
                                                                                CssClass="form-control input-sm input-small"></asp:TextBox>
                                                                            <%--<asp:RequiredFieldValidator ID="rfvDescT" runat="server" ControlToValidate="lblDesc"
                                                                                Display="Dynamic" ErrorMessage="***Required***" SetFocusOnError="True" ValidationGroup="ctempl"></asp:RequiredFieldValidator>--%>
                                                                        </ItemTemplate>
                                                                        <FooterStyle VerticalAlign="Middle" />
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="Alert " HeaderStyle-CssClass="itemHeader" HeaderStyle-Width="50">
                                                                        <ItemTemplate>
                                                                            <asp:CheckBox ID="chkSelectAlert" CssClass="css-checkbox" Text=" " runat="server"
                                                                                Checked='<%# Convert.ToBoolean((Eval("IsAlert")==DBNull.Value ? false:Eval("IsAlert"))) %>' />
                                                                        </ItemTemplate>
                                                                        <FooterStyle VerticalAlign="Middle" />
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField
                                                                        HeaderText="Team Member" HeaderStyle-CssClass="itemHeader" HeaderStyle-Width="300">
                                                                        <ItemTemplate>
                                                                            <div class="tag-div materialize-textarea textarea-border" id="cusLabelTag" style="text-align: left !important; cursor: pointer;" onclick="ShowTeamMemberWindow(this);" runat="server"></div>
                                                                            <asp:HiddenField ID="hdnMembers" runat="server" Value='<%# Eval("TeamMember") %>' />
                                                                            <asp:TextBox ID="txtMembers" class='<%# "txtMembers_" + Eval("Line") %>' runat="server" Style='min-width: 100px !important; display: none;'
                                                                                Text='<%# Eval("TeamMemberDisplay") %>'></asp:TextBox>
                                                                        </ItemTemplate>
                                                                        <FooterStyle VerticalAlign="Middle" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Format" Visible="true">
                                                                        <ItemTemplate>
                                                                            <div style="border-spacing: 0; padding: 0;">
                                                                                <div style="display:flex;">
                                                                                    <div>
                                                                                        <asp:DropDownList ID="ddlFormat" runat="server" Width="100px" AutoPostBack="true" CssClass="browser-default"
                                                                                            OnSelectedIndexChanged="ddlFormat_SelectedIndexChanged" DataValueField="value" DataSource='<%#dtFormat%>' DataTextField="format"
                                                                                            SelectedValue='<%# Eval("format") == DBNull.Value ? 0 : Eval("format") %>'>
                                                                                        </asp:DropDownList>
                                                                                    </div>
                                                                                    <div>
                                                                                        <asp:Panel ID="pnlCustomValue" runat="server" Visible="false">
                                                                                            <div style="border-spacing: 0px; padding: 0px;">
                                                                                                <div style="display:flex;">
                                                                                                    <div style="padding-left: 10px;">
                                                                                                        <asp:DropDownList ID="ddlCustomValue" Width="100px" runat="server" AutoPostBack="true"
                                                                                                            OnSelectedIndexChanged="ddlCustomValue_SelectedIndexChanged" CssClass="browser-default">
                                                                                                            <asp:ListItem Value="">--Add New--</asp:ListItem>
                                                                                                        </asp:DropDownList>
                                                                                                    </div>
                                                                                                    <div style="padding-left: 10px;">
                                                                                                        <asp:TextBox ID="txtCustomValue" Width="100px" runat="server" CssClass="form-control input-sm input-small"></asp:TextBox>
                                                                                                    </div>
                                                                                                    <div style="padding-left: 10px;">
                                                                                                        <div style="border-spacing: 0; padding: 0;">
                                                                                                            <div>
                                                                                                                <div>
                                                                                                                    <div class="btnlinks">
                                                                                                                        <asp:LinkButton ID="lnkAddCustomValue" CommandName="AddCustomValue" CommandArgument='<%# Container.DataItemIndex %>'
                                                                                                                            runat="server" CausesValidation="False">Add</asp:LinkButton>
                                                                                                                    </div>
                                                                                                                    <div class="btnlinks">
                                                                                                                        <asp:LinkButton ID="lnkUpdateCustomValue" CommandName="UpdateCustomValue" Visible="false" runat="server"
                                                                                                                            CommandArgument='<%# Container.DataItemIndex %>' CausesValidation="False">Update</asp:LinkButton>
                                                                                                                    </div>
                                                                                                                </div>
                                                                                                                <div>
                                                                                                                    <asp:LinkButton ID="lnkDelCustomValue" CommandName="DeleteCustomValue" CommandArgument='<%# Container.DataItemIndex %>'
                                                                                                                        CausesValidation="False" Visible="false" runat="server">
                                                                                                                    <img height="12px" alt="Delete" title="Delete" src="images/delete-grid.png" /></asp:LinkButton>
                                                                                                                </div>
                                                                                                            </div>
                                                                                                        </div>
                                                                                                    </div>
                                                                                                </div>
                                                                                            </div>
                                                                                        </asp:Panel>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                        </ItemTemplate>
                                                                        <FooterTemplate>
                                                                            <div style="float: left; margin-top: 5px; margin-left: 10px;">
                                                                                <asp:Label ID="lblRowCount" runat="server" Text=""></asp:Label>

                                                                            </div>
                                                                        </FooterTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="hdnLineOpenned" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="cf"></div>
                                    </div>
                                </div>
                                <div style="clear: both;"></div>
                            </div>
                        </li>
                        <li> <%--accrdUsers--%>
                            <div id="accrdUsers" class="collapsible-header accrd  accordian-text-custom "><i class="mdi-social-people"></i>Users</div>
                            <div class="collapsible-body">
                                <div class="form-content-wrap form-content-wrapwd">

                                    <%---------------------------%>

                                    <div class="form-section-row">
                                        <div class="btnlinks">
                                            <asp:HiddenField ID="hdnLineOpenned_uc" runat="server" />
                                            <asp:HiddenField ID="hdnOrgMemberKey_uc" runat="server" />
                                            <asp:HiddenField ID="hdnOrgMemberDisp_uc" runat="server" />
                                            <asp:HiddenField ID="hdnOrgUserRoleID_uc" runat="server" />
                                            <asp:HiddenField ID="hdnOrgUserRoleDisp_uc" runat="server" />
                                            <%--<asp:LinkButton ID="LnkSaveUserCustom" CausesValidation="false" OnClientClick="return validateGridCustom();" runat="server" OnClick="LnkSaveUserCustom_Click" ValidationGroup="groupCustomer">Save</asp:LinkButton>--%>
                                        </div>
                                    </div>

                                    <div class="form-section-row" style="margin-bottom: 0 !important;">
                                        <div class="grid_container">
                                            <div class="form-section-row" style="margin-bottom: 0 !important;">
                                                <div class="RadGrid RadGrid_Material FormGrid">
                                                    <telerik:RadAjaxPanel ID="RadAjaxPanel_UserCustom" runat="server">
                                                        <telerik:RadGrid RenderMode="Auto" ID="gvUserCustom" ShowFooter="True" PageSize="50"
                                                            PagerStyle-AlwaysVisible="true" OnItemDataBound="gvUserCustom_ItemDataBound" OnItemCommand="gvUserCustom_ItemCommand"
                                                            ShowStatusBar="true" runat="server" AllowPaging="True" AllowSorting="true" Width="100%" AllowCustomPaging="True" >
                                                            <%--OnItemDataBound="gvUserCustom_ItemDataBound" OnItemCommand="gvCustom_RowCommand"--%>
                                                            <CommandItemStyle />
                                                            <GroupingSettings CaseSensitive="false" />
                                                            <ClientSettings AllowColumnsReorder="false" ReorderColumnsOnClient="false">
                                                                <Selecting AllowRowSelect="false"></Selecting>
                                                                <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                                                <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                                            </ClientSettings>
                                                            <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="false" ShowFooter="True">
                                                                <Columns>

                                                                    <telerik:GridTemplateColumn HeaderStyle-Width="40" AllowFiltering="false" ItemStyle-Width="30px" FooterStyle-Width="30px">
                                                                        <HeaderTemplate>
                                                                            <asp:LinkButton ID="ibtnDeleteUserCustomItem" OnClientClick="return confirm('Are you sure you want to delete the items? This will delete the items from User Custom field.')"
                                                                                CausesValidation="false" ToolTip="Delete" runat="server" Style="color: #000; font-size: 1.5em; top: 0px; margin-left: 0px;" Width="20px"
                                                                                OnClick="ibtnDeleteUserCustomItem_Click"><i class="mdi-navigation-cancel" style=" color: #f00;font-size: 1.2em; font-weight: bold;margin-left: -8px; "></i></asp:LinkButton>
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <asp:HiddenField ID="txtOrderNo" Value='<%# Eval("OrderNo") %>' runat="server"></asp:HiddenField>
                                                                            <asp:Label ID="lblIndex" Visible="false" runat="server" Text="<%# Container.ItemIndex +1 %>"></asp:Label>
                                                                            <asp:Label ID="lblID" runat="server" Text='<%# Eval("ID") %>' Visible="false"></asp:Label>
                                                                            <asp:Label ID="lblLine" runat="server" Text='<%# Eval("Line") %>' CssClass="customline" Style="display: none;"></asp:Label>
                                                                            <asp:HiddenField ID="hdnMaxLineNo" Value='<%# Eval("MaxLineNo") %>' runat="server"></asp:HiddenField>
                                                                            <asp:CheckBox ID="chkSelect" runat="server" />
                                                                        </ItemTemplate>
                                                                        <FooterTemplate>
                                                                            <asp:LinkButton ID="lnkAddnewRowUserCustom" runat="server" CausesValidation="False" Style="color: #000; font-size: 1.5em;" Width="20px"
                                                                                ToolTip="Add New Row" OnClick="lnkAddnewRowUserCustom_Click"><i class="mdi-content-add-circle" style=" margin-left: -16px;color: #2bab54;font-size: 1.2em; font-weight: bold;"></i></asp:LinkButton>
                                                                        </FooterTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    
                                                                    <telerik:GridTemplateColumn HeaderStyle-Width="40" AllowFiltering="false">
                                                                        <ItemTemplate>
                                                                            <div class="handle" style="cursor: move" title="Move Up/Down">
                                                                                <img src="images/Dragdrop.png" width="20px" />
                                                                            </div>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>

                                                                    <telerik:GridTemplateColumn HeaderStyle-Width="200" AllowFiltering="false" HeaderText="Label" HeaderStyle-CssClass="itemHeader">
                                                                        <ItemTemplate>
                                                                            <div style="text-align: left">
                                                                                <asp:TextBox ID="txtLabel" runat="server" Text='<%# Eval("Label") %>' MaxLength="100"
                                                                                    CssClass="form-control font_09rem"></asp:TextBox>
                                                                                <asp:RequiredFieldValidator runat="server" ID="reqLabel" ControlToValidate="txtLabel" ErrorMessage="Please enter label name!"
                                                                                    ValidationGroup="groupCustomer" />
                                                                            </div>
                                                                        </ItemTemplate>
                                                                        <FooterStyle VerticalAlign="Middle" />
                                                                    </telerik:GridTemplateColumn>

                                                                    <telerik:GridTemplateColumn HeaderStyle-Width="50" AllowFiltering="false" HeaderText="Alert " HeaderStyle-CssClass="itemHeader">
                                                                        <ItemTemplate>
                                                                            <asp:CheckBox ID="chkSelectAlert" CssClass="css-checkbox" Text=" " runat="server" Checked='<%# Convert.ToBoolean((Eval("IsAlert")==DBNull.Value ? false:Eval("IsAlert"))) %>' />
                                                                        </ItemTemplate>
                                                                        <FooterStyle VerticalAlign="Middle" />
                                                                    </telerik:GridTemplateColumn>

                                                                    <telerik:GridTemplateColumn HeaderStyle-Width="250" AllowFiltering="false" HeaderText="Team Member" HeaderStyle-CssClass="itemHeader">
                                                                        <ItemTemplate>
                                                                            <div class="tag-div materialize-textarea textarea-border" id="cusLabelTag" style="text-align: left !important; cursor: pointer;" onclick="ShowTeamMemberWindow_uc(this);" runat="server"></div>
                                                                            <asp:HiddenField ID="hdnMembers" runat="server" Value='<%# Eval("TeamMember") %>' />
                                                                            <asp:TextBox ID="txtMembers" class='<%# "txtMembers_uc_" + Eval("Line") %>' runat="server" Style='min-width: 100px !important; display: none;'
                                                                                Text='<%# Eval("TeamMemberDisplay") %>'></asp:TextBox>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>

                                                                    <telerik:GridTemplateColumn FilterDelay="5" HeaderText="Format" HeaderStyle-Width="400" CurrentFilterFunction="Contains" ShowFilterIcon="false" AllowFiltering="false" HeaderStyle-CssClass="itemHeader">
                                                                        <ItemTemplate>
                                                                            <div style="display:flex;">
                                                                                
                                                                                    <div>
                                                                                        <asp:DropDownList ID="ddlUserCustomFormat" runat="server" Width="100px" AutoPostBack="true" CssClass="browser-default font_09rem"
                                                                                            OnSelectedIndexChanged="ddlUserCustomFormat_SelectedIndexChanged" DataValueField="value" DataSource='<%#dtUserCstFormat%>' DataTextField="Format">
                                                                                        </asp:DropDownList>
                                                                                    </div>

                                                                                    <div>
                                                                                        <asp:Panel ID="pnlUserCustomValue" runat="server" Visible="true">
                                                                                            <div style="border-spacing: 0px; padding: 0px;">
                                                                                                <div style="display:flex;">
                                                                                                    <div style="padding-left: 10px;">
                                                                                                        <asp:DropDownList ID="ddlUserCustomValue" Width="100px" runat="server" AutoPostBack="true"
                                                                                                            OnSelectedIndexChanged="ddlUserCustomValue_SelectedIndexChanged" CssClass="browser-default font_09rem">
                                                                                                            <asp:ListItem Value="">--Add New--</asp:ListItem>
                                                                                                        </asp:DropDownList>
                                                                                                    </div>
                                                                                                    <div style="padding-left: 10px;">
                                                                                                        <asp:HiddenField ID="hddCustomValueId" runat="server"></asp:HiddenField>
                                                                                                        <asp:TextBox ID="txtUserCustomValue" Width="100px" runat="server" CssClass="form-control input-sm input-small font_09rem"></asp:TextBox>
                                                                                                    </div>
                                                                                                    <div style="padding-left: 10px;">
                                                                                                        <div style="border-spacing: 0; padding: 0;">
                                                                                                            <div>
                                                                                                                <div>
                                                                                                                    <div class="btnlinks">

                                                                                                                        <asp:LinkButton ID="lnkAddUserCustomValue" CommandName="AddUserCustomValue"
                                                                                                                            runat="server" CausesValidation="False">Add</asp:LinkButton>
                                                                                                                    </div>
                                                                                                                    <div class="btnlinks">

                                                                                                                        <asp:LinkButton ID="lnkUpdateUserCustomValue" CommandName="UpdateUserCustomValue" Visible="false" runat="server"
                                                                                                                            CausesValidation="False">Update</asp:LinkButton>
                                                                                                                    </div>
                                                                                                                </div>
                                                                                                                <div>
                                                                                                                    <asp:LinkButton ID="lnkDeleteUserCustomValue" CommandName="DeleteUserCustomValue"
                                                                                                                        CausesValidation="False" Visible="false" runat="server">
                                                                                                                <img height="12px" alt="Delete" title="Delete" src="images/delete-grid.png" /></asp:LinkButton>
                                                                                                                </div>
                                                                                                            </div>
                                                                                                        </div>
                                                                                                    </div>
                                                                                                </div>
                                                                                            </div>
                                                                                        </asp:Panel>


                                                                                    </div>
                                                                                
                                                                            </div>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn HeaderStyle-Width="70" AllowFiltering="false" HeaderText="Is Formula" HeaderStyle-CssClass="itemHeader" Visible="false">

                                                                        <ItemTemplate>

                                                                            <asp:CheckBox ID="chkFormula" runat="server" class="css-checkbox" Text=" " Checked='<%# Convert.ToBoolean((Eval("UseFormula")==DBNull.Value ? false:Eval("UseFormula"))) %>' />

                                                                        </ItemTemplate>

                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn HeaderStyle-Width="250" AllowFiltering="false" HeaderText="Formula Detail" HeaderStyle-CssClass="itemHeader" Visible="false">

                                                                        <ItemTemplate>

                                                                            <asp:TextBox ID="txtFormula" runat="server" Text='<%# Eval("Formula") %>'></asp:TextBox>

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


                                    <%----------------------------%>

                                    <%--<div class="form-content-pd">
                                        <div class="grid_container">
                                            <div class="form-section-row m-b-0">
                                                <div class="RadGrid RadGrid_Material">
                                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                        <ContentTemplate>
                                                            <asp:HiddenField ID="hdnLineOpenned1" runat="server" />
                                                            <asp:HiddenField ID="hdnOrgMemberKey1" runat="server" />
                                                            <asp:HiddenField ID="hdnOrgMemberDisp1" runat="server" />
                                                            <asp:HiddenField ID="hdnOrgUserRoleID1" runat="server" />
                                                            <asp:HiddenField ID="hdnOrgUserRoleDisp1" runat="server" />

                                                            
                                                            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" AlternatingRowStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HorizontalAlign="Center"
                                                                CssClass="BomGrid" Width="100%"
                                                                ShowFooter="true" HeaderStyle-Font-Size="0.9em" OnRowCommand="gvCustom_RowCommand"
                                                                OnRowDataBound="gvCustom_RowDataBound">
                                                                <Columns>
                                                                    <asp:TemplateField ItemStyle-Width="0.5%" HeaderStyle-Width="0.5%" FooterStyle-Width="0.5%">
                                                                        <HeaderTemplate>
                                                                            <asp:LinkButton ID="ibtnDeleteCItem" OnClientClick="return confirm('Are you sure you want to delete the items? This will delete the items from Workflow field.')"
                                                                                CausesValidation="false" ToolTip="Delete" runat="server" Style="color: #000; font-size: 1.5em;" Width="20px"
                                                                                OnClick="ibtnDeleteCItem_Click"><i class="mdi-navigation-cancel cancels" style="margin-left: -6px; color: #f00;font-size: 1.2em; font-weight: bold;"></i></asp:LinkButton>
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>

                                                                            <asp:HiddenField ID="txtRowLine" Value='<%# Eval("OrderNo") %>' runat="server"></asp:HiddenField>
                                                                            <asp:Label ID="lblIndex" Visible="false" runat="server" Text="<%# Container.DataItemIndex +1 %>"></asp:Label>
                                                                            <asp:Label ID="lblID" runat="server" Text='<%# Eval("ID") %>' Visible="false"></asp:Label>
                                                                            <asp:Label ID="lblLine" runat="server" Text='<%# Eval("Line") %>' CssClass="customline" Style="display: none;"></asp:Label>
                                                                            <asp:CheckBox ID="chkSelect" CssClass="css-checkbox" Text=" " runat="server" />
                                                                        </ItemTemplate>
                                                                        <FooterTemplate>
                                                                            <asp:LinkButton ID="lnkAddnewRow" runat="server" CausesValidation="False" Style="color: #000; font-size: 1.5em;" Width="20px"
                                                                                ToolTip="Add New Row" OnClick="lnkAddnewRow_Click"><i class="mdi-content-add-circle" style="margin-left: -11px; color: #2bab54;font-size: 1.2em; font-weight: bold;"></i></asp:LinkButton>
                                                                        </FooterTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField ItemStyle-Width="20px">
                                                                        <ItemTemplate>
                                                                            <div class="handle" style="cursor: move" title="Move Up/Down">
                                                                                <img src="images/Dragdrop.png" width="20px" />
                                                                            </div>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Label"
                                                                        FooterStyle-VerticalAlign="Middle">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="lblDesc" runat="server" Text='<%# Eval("Label") %>' MaxLength="255" Style='min-width: 100px!important;'
                                                                                CssClass="form-control input-sm input-small"></asp:TextBox>
                                                                        </ItemTemplate>
                                                                        <FooterStyle VerticalAlign="Middle" />
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="Alert " HeaderStyle-CssClass="itemHeader" HeaderStyle-Width="50">
                                                                        <ItemTemplate>
                                                                            <asp:CheckBox ID="chkSelectAlert" CssClass="css-checkbox" Text=" " runat="server"
                                                                                Checked='<%# Convert.ToBoolean((Eval("IsAlert")==DBNull.Value ? false:Eval("IsAlert"))) %>' />
                                                                        </ItemTemplate>
                                                                        <FooterStyle VerticalAlign="Middle" />
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField
                                                                        HeaderText="Team Member" HeaderStyle-CssClass="itemHeader">
                                                                        <ItemTemplate>
                                                                            <div class="tag-div materialize-textarea textarea-border" id="cusLabelTag" style="text-align: left !important; cursor: pointer;" onclick="ShowTeamMemberWindow(this);" runat="server"></div>
                                                                            <asp:HiddenField ID="hdnMembers" runat="server" Value='<%# Eval("TeamMember") %>' />
                                                                            <asp:TextBox ID="txtMembers" class='<%# "txtMembers_" + Eval("Line") %>' runat="server" Style='min-width: 100px !important; display: none;'
                                                                                Text='<%# Eval("TeamMemberDisplay") %>'></asp:TextBox>
                                                                        </ItemTemplate>
                                                                        <FooterStyle VerticalAlign="Middle" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Format" Visible="true">
                                                                        <ItemTemplate>
                                                                            <table style="border-spacing: 0; padding: 0;">
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:DropDownList ID="ddlFormat" runat="server" Width="100px" AutoPostBack="true" CssClass="browser-default"
                                                                                            OnSelectedIndexChanged="ddlFormat_SelectedIndexChanged" DataValueField="value" DataSource='<%#dtFormat%>' DataTextField="format"
                                                                                            SelectedValue='<%# Eval("format") == DBNull.Value ? 0 : Eval("format") %>'>
                                                                                        </asp:DropDownList>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:Panel ID="pnlCustomValue" runat="server" Visible="false">
                                                                                            <table style="border-spacing: 0px; padding: 0px;">
                                                                                                <tr>
                                                                                                    <td style="padding-left: 10px;">
                                                                                                        <asp:DropDownList ID="ddlCustomValue" Width="100px" runat="server" AutoPostBack="true"
                                                                                                            OnSelectedIndexChanged="ddlCustomValue_SelectedIndexChanged" CssClass="browser-default">
                                                                                                            <asp:ListItem Value="">--Add New--</asp:ListItem>
                                                                                                        </asp:DropDownList>
                                                                                                    </td>
                                                                                                    <td style="padding-left: 10px;">
                                                                                                        <asp:TextBox ID="txtCustomValue" Width="100px" runat="server" CssClass="form-control input-sm input-small"></asp:TextBox>
                                                                                                    </td>
                                                                                                    <td style="padding-left: 10px;">
                                                                                                        <table style="border-spacing: 0; padding: 0;">
                                                                                                            <tr>
                                                                                                                <td>
                                                                                                                    <div class="btnlinks">
                                                                                                                        <asp:LinkButton ID="lnkAddCustomValue" CommandName="AddCustomValue" CommandArgument='<%# Container.DataItemIndex %>'
                                                                                                                            runat="server" CausesValidation="False">Add</asp:LinkButton>
                                                                                                                    </div>
                                                                                                                    <div class="btnlinks">
                                                                                                                        <asp:LinkButton ID="lnkUpdateCustomValue" CommandName="UpdateCustomValue" Visible="false" runat="server"
                                                                                                                            CommandArgument='<%# Container.DataItemIndex %>' CausesValidation="False">Update</asp:LinkButton>
                                                                                                                    </div>
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <asp:LinkButton ID="lnkDelCustomValue" CommandName="DeleteCustomValue" CommandArgument='<%# Container.DataItemIndex %>'
                                                                                                                        CausesValidation="False" Visible="false" runat="server">
                                                                                                                    <img height="12px" alt="Delete" title="Delete" src="images/delete-grid.png" /></asp:LinkButton>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </asp:Panel>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </ItemTemplate>
                                                                        <FooterTemplate>
                                                                            <div style="float: left; margin-top: 5px; margin-left: 10px;">
                                                                                <asp:Label ID="lblRowCount" runat="server" Text=""></asp:Label>

                                                                            </div>
                                                                        </FooterTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>

                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="hdnLineOpenned1" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="cf"></div>
                                    </div>--%>
                                </div>
                                <div style="clear: both;"></div>
                            </div>
                        </li> <%--accrdUsers--%>
                    </ul>
                </div>
            </div>
        </div>
    </div>

    <telerik:RadWindow ID="TeamMembersWindow" Skin="Material" VisibleTitlebar="true" Behaviors="Default" CenterIfModal="true"
        Animation="FlyIn" AnimationDuration="200" RenderMode="Auto" VisibleStatusbar="false"
        runat="server" Modal="true" Width="1050" Height="635">
        <ContentTemplate>
            <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">
                <div style="margin-top: 15px;">
                    <div class="form-section-row">
                        <div class="form-section">
                            <div class="row" style="margin-bottom: 0;">
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
                                            <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="True">
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
                                                        DataField="RoleName" SortExpression="RoleName" AutoPostBackOnFilter="true" DataType="System.String"
                                                        CurrentFilterFunction="Contains" HeaderText="User Role" ShowFilterIcon="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblUserRole" runat="server"><%#Eval("RoleName")%></asp:Label>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn SortExpression="IsTask" AllowFiltering="false" HeaderStyle-Width="100" ShowFilterIcon="false" DataField="IsTask" HeaderText="Task">
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
                        </div>
                    </div>
                    <div style="clear: both;"></div>
                    <footer style="float: left; padding-left: 0 !important;">
                        <div class="btnlinks">
                            <a id="lnkPopupOK" onclick="CloseTeamMemberWindow();" style="cursor: pointer;">OK</a>
                        </div>
                    </footer>
                </div>
            </telerik:RadAjaxPanel>
        </ContentTemplate>
    </telerik:RadWindow>

</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="javascriptHolder" runat="Server">

    <script type="text/javascript" src="Design/js/plugins/dropify/js/dropify.min.js"></script>
    <script type="text/javascript">

        $(document).ready(function () {

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

        });
    </script>
    <script>

</script>
</asp:Content>
