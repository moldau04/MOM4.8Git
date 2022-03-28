<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TelerikTest.aspx.cs" Inherits="MOMWebApp.TelerikTest" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <script type="text/javascript">
    function MyClientShowing(sender, eventArgs) {
        eventArgs.get_loadingElement().style.border = "2px solid red";
        eventArgs.set_cancelNativeDisplay(true);
        $telerik.$(eventArgs.get_loadingElement()).show("slow");
    }
    function MyClientHiding(sender, eventArgs) {
        eventArgs.get_loadingElement().style.border = "2px solid blue";
        eventArgs.set_cancelNativeDisplay(true);
        $telerik.$(eventArgs.get_loadingElement()).hide("slow");
    }
    </script>

<%--     <script>

         function BindClickEventForGridCheckBox_uc() {
             $("#<%=RadGrid_Emails_uc.ClientID%> input[id*='chkSelect']:checkbox").unbind('click').bind('click', function () {
                 CheckUncheckAllCheckBoxAsNeeded_uc();
             });

             $("#<%=RadGrid_Emails_uc.ClientID%> input[id*='chkTask']:checkbox").unbind('click').bind('click', function () {
                 OnCheck_TaskCheckBox('<%=RadGrid_Emails_uc.ClientID%>');
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
                         teamArr[index] = value.trim();
                     });

                     $("#<%=RadGrid_Emails_uc.ClientID%> input[id*='chkSelect']:checkbox").each(function (index) {
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

         function CloseTeamMemberWindow_uc() {
             var line = $("#<%= hdnLineOpenned_uc.ClientID%>").val();
             if (line != null && line != '') {
                 var hdnMembersValue = $("#<%= hdnOrgMemberKey_uc.ClientID%>").val();
                var txtMembersValue = $("#<%= hdnOrgMemberDisp_uc.ClientID%>").val();

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
             var wnd = $find('<%=TeamMembersWindow_uc.ClientID %>');
             wnd.Close();
         }

         function ShowTeamMemberWindow_uc(txtTeamMember) {
             var line = $(txtTeamMember).closest('tr').find('td:eq(0) > span.customline').text();
             var txtTeamMembersId = $(txtTeamMember).attr("id");
             var hdnTeamMembersId = txtTeamMembersId.replace("cusLabelTag", "hdnMembers");
             var teamMembers = $("#" + hdnTeamMembersId).val();
             var txtCustomLabelId = txtTeamMembersId.replace("cusLabelTag", "lblCustom");
             var txtCustomLabelVal = $("#" + txtCustomLabelId).text();
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
         }

    </script>--%>


</head>
<body>
    <form id="form1" runat="server">
        <div>
            <telerik:RadScriptManager ID="RadScriptManager1" runat="server" />
            <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server">
<%--                BackColor="yellow" OnClientShowing="MyClientShowing" OnClientHiding="MyClientHiding">--%>
            </telerik:RadAjaxLoadingPanel>
            <telerik:RadAjaxManager ID="RadAjaxManager2" runat="server" DefaultLoadingPanelID="RadAjaxLoadingPanel1">
                <AjaxSettings>
                    <telerik:AjaxSetting AjaxControlID="Button1">
                        <UpdatedControls>
                            <telerik:AjaxUpdatedControl ControlID="Panel1" />
                        </UpdatedControls>
                    </telerik:AjaxSetting>
                </AjaxSettings>
            </telerik:RadAjaxManager>
            <asp:Button ID="Button1" runat="server" Text="AJAX" OnClick="Button1_Click" /><br />
            <br />
            <table cellpadding="10">
                <tr>
                    <td>
                        <asp:Panel ID="Panel1" runat="server" Width="600px" Height="500px" BorderStyle="Dotted">
                            <asp:Label ID="Label1" runat="server" />
                         </asp:Panel>
                    </td>
                </tr>
            </table>

<%--------------------------------------------------------------------------------------------------------------------------%>

<%--    <telerik:RadWindow ID="TeamMembersWindow_uc" Skin="Material" VisibleTitlebar="true" Behaviors="Default" CenterIfModal="true"
    Animation="FlyIn" AnimationDuration="200" RenderMode="Lightweight" VisibleStatusbar="false"
    runat="server" Modal="true" Width="1050" Height="635">
    <ContentTemplate>
        <telerik:RadAjaxPanel ID="RadAjaxPanel32" runat="server">
            <div class="margin-tp">
                <div class="form-section-row">
                    <div class="form-section">
                        <div class="row mb" >
                            <div class="grid_container" id="divMemberGrid_uc" runat="server">
                                <div class="RadGrid RadGrid_Material RadGrid RadGrid_Popup">

                                    <telerik:RadGrid RenderMode="Lightweight" ID="RadGrid_Emails_uc" AllowFilteringByColumn="true" ShowFooter="false" PageSize="1000"
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
                        <a id="lnkPopupOK_uc" onclick="CloseTeamMemberWindow_uc();" style="cursor: pointer;">OK</a>
                           
                    </div>
                    </div>
                </div>
                  
                </div>
            </telerik:RadAjaxPanel>
        </ContentTemplate>
    </telerik:RadWindow>--%>


        </div>
    </form>
</body>
</html>
