<%@ Page Title="Safety Tests || MOM" Language="C#" MasterPageFile="~/Mom.master" AutoEventWireup="true" Inherits="SafetyTestList" Codebehind="SafetyTestList.aspx.cs" %>
<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <!--Grid Control-->
    <link href="Design/css/grid.css" rel="stylesheet" />
    <%--Calendar CSS--%>
    <link href="Design/css/pikaday.css" rel="stylesheet" />

    <script type="text/javascript">

        function UpdateddlScheduledStatusMulti(TestID, TestYear, ScheduledStatus) {
            var mgs = "Do you want to update schedule status for all scheduled test?"
            var bulkUpdateConfirm = confirm(mgs);
            $("#<%= HiddenField1B.ClientID%>").val('0');
            if (bulkUpdateConfirm == true) {
                $("#<%= HiddenField1B.ClientID%>").val('1');
            }

            $("#<%= HiddenFieldYear.ClientID%>").val(TestYear);
            $("#<%= HiddenFieldStatus.ClientID%>").val(ScheduledStatus);
            $("#<%= HiddenFieldLID.ClientID%>").val(TestID);
            document.getElementById('ctl00_ContentPlaceHolder1_Button1Schedule').click();
        }



        /* UpdateFireDate*/

        function UpdateFireDateMulti(item, param) {
            debugger;
            var radGridSafetyTest = $find('<%=RadGrid_SafetyTest.ClientID %>');
            var masterTable = radGridSafetyTest.get_masterTableView();
            var row = masterTable.get_dataItems().length;
            console.log('UpdateFireDateMulti');
            var mgs = "Do you want to change/update the Fire Test Date in order " + row + " records as well?"
            var bulkUpdateConfirm = confirm(mgs);
            if (bulkUpdateConfirm == true) {
                console.log('Bulk updates for Fire Test Date...')
                var showalertOncetime = false;
                for (var j = 0; j < row; j++) {

                    if (j == 0) {
                        showalertOncetime = true;
                    } else { showalertOncetime = false; }
                    var Elem = masterTable.get_dataItems()[j].findElement("TB_CustomField_FireTestDate_1");
                    //**Firing onChange()**
                    for (var i = 0; i < param.length; i++) {
                        console.log('Updating for ' + Elem.id);
                        //SET default value for FireTest
                        Elem.value = item.value;
                        /*if (param[i].controlID != null)*/
                        {
                            UpdateFireDate(Elem, "TB_" + param[i].controlID, "TB_" + param[i].ControlUpdate, param[i].xFireDay, param[i].xScheduleDay, true, showalertOncetime)
                        }
                    }
                }
            }
            else {
                for (var i = 0; i < param.length; i++) {
                    /* if (param[i].controlID != null)*/
                    {
                        UpdateFireDate(item, "TB_" + param[i].controlID, "TB_" + param[i].ControlUpdate, param[i].xFireDay, param[i].xScheduleDay, false, false)
                    }
                }
            }

        }

        function UpdateFireDate(item, objName, objUpdateControl, xFireDay, xScheduleDay, isBulkUpdate, showalertOncetime) {
            var id = item.id;

            debugger;
            var lblTestYear = document.getElementById(id.replace(objName, 'lblTestYear'));
            var TestID = document.getElementById(id.replace(objName, 'hdnTestID'));
            var EquipmentID = document.getElementById(id.replace(objName, 'hduid'));

            var obj = new dtaa();
            obj.TestID = $(TestID).val()
            obj.EquipmentID = $(EquipmentID).val();
            obj.TestCustomFieldID = objName.split("_")[3];
            obj.Value = $(item).val();
            obj.OldValue = "";
            obj.strUrl = getURL();
            obj.TestYear = lblTestYear.innerText;
            UpdateCustomField(obj, isBulkUpdate, showalertOncetime);
            processCOAByFireDate(item, objName, objUpdateControl, xFireDay, xScheduleDay);

        }

        /* UpdateScheduleDate*/

        function UpdateScheduleDateMulti(item, param) {
            debugger;
            var radGridSafetyTest = $find('<%=RadGrid_SafetyTest.ClientID %>');
            var masterTable = radGridSafetyTest.get_masterTableView();
            var row = masterTable.get_dataItems().length;
            var mgs = "Do you want to change/update the scheduled date for all " + row + " records?. Please Note The schedule date format should be MM/DD/YYYY."
            var bulkUpdateConfirm = confirm(mgs);
            if (bulkUpdateConfirm == true) {
                console.log('Bulk updates for scheduled date...')

                var showalertOncetime = false;
                for (var j = 0; j < row; j++) {

                    if (j == 0) {
                        showalertOncetime = true;
                    } else { showalertOncetime = false; }
                    var scheduleDateElem = masterTable.get_dataItems()[j].findElement("txtScheduleDate");
                    //**Firing onChange()**
                    for (var i = 0; i < param.length; i++) {
                        console.log('Updating for ' + scheduleDateElem.id);
                        //SET default value for Schedule
                        scheduleDateElem.value = item.value;
                        UpdateScheduleDate(scheduleDateElem, "TB_" + param[i].controlID, "TB_" + param[i].ControlUpdate, param[i].xFireDay, param[i].xScheduleDay, true, showalertOncetime)

                    }
                }
                noty({
                    text: 'Test updated successfully!',
                    type: 'success',
                    layout: 'topCenter',
                    closeOnSelfClick: false,
                    timeout: 5000,
                    theme: 'noty_theme_default',
                    closable: true
                });
            }
            else {
                for (var i = 0; i < param.length; i++) {
                    //alert(param[i].controlName);
                    UpdateScheduleDate(item, "TB_" + param[i].controlID, "TB_" + param[i].ControlUpdate, param[i].xFireDay, param[i].xScheduleDay, false, false)
                }
            }

        }

        function UpdateScheduleDate(item, objName, objUpdateControl, xFireDay, xScheduleDay, isBulkUpdate, showalertOncetime) {

            debugger;
            var txtScheduleDate = item.id;
            var hdnTestID = document.getElementById(txtScheduleDate.replace('txtScheduleDate', 'hdnTestID'));
            var lblTestYear = document.getElementById(txtScheduleDate.replace('txtScheduleDate', 'lblTestYear'));
            var hdnMembers = document.getElementById(txtScheduleDate.replace('txtScheduleDate', 'hdnMembers'));
            var hdnScheduledStatus = document.getElementById(txtScheduleDate.replace('txtScheduleDate', 'hdnScheduledStatus'));


            var status = 0;
            status = $(hdnScheduledStatus).val();
            if ($(hdnScheduledStatus).val() == "") {
                status = 0;
            }

            if (item.value != "") {
                var isDateTime = true;
                var arrTime = item.value.split(",");
                for (var i = 0; i < arrTime.length; i++) {
                    if (!arrTime[i].includes(lblTestYear.innerText)) {
                        isDateTime = false;
                    }
                }
                if (isDateTime == false) {

                    if (showalertOncetime) {
                        noty({
                            text: 'Date format is invalid. The schedule date format should be MM/DD/YYYY',
                            type: 'warning',
                            layout: 'topCenter',
                            closeOnSelfClick: false,
                            timeout: 5000,
                            theme: 'noty_theme_default',
                            closable: true
                        });
                    }
                } else {
                    if (isBulkUpdate) {
                        BulkUpdateTestSchedule($(hdnTestID).val(), lblTestYear.innerText, item.value, status, $(hdnMembers).val(), txtScheduleDate, showalertOncetime);
                        var txtFireDateID = document.getElementById(txtScheduleDate.replace('txtScheduleDate', objName));
                        processCOAByFireDateForBulkUpdate(txtFireDateID, objName, objUpdateControl, xFireDay, xScheduleDay)
                        
                    }
                    else {
                        UpdateTestSchedule($(hdnTestID).val(), lblTestYear.innerText, item.value, status, $(hdnMembers).val(), txtScheduleDate);
                        var txtFireDateID = document.getElementById(txtScheduleDate.replace('txtScheduleDate', objName));
                        processCOAByFireDate(txtFireDateID, objName, objUpdateControl, xFireDay, xScheduleDay)
                        
                    }

                }

            }
        }

        function UpdateTextCustomField(item, objName) {
            debugger;
            $('#MOMloading').show();

            var radGridSafetyTest = $find('<%=RadGrid_SafetyTest.ClientID %>');
            var masterTable = radGridSafetyTest.get_masterTableView();
            var row = masterTable.get_dataItems().length;

            var mgs = "";
            if (objName.split("_")[2] == 'FireServiceAccepted') {
                mgs = "Do you want to change/update the  Fire Service Accepted in order " + row + " records as well?"
            }
            else {

                mgs = "Do you want to change/update the  " + objName.split("_")[2] + " in order " + row + " records as well?"
            }
            var bulkUpdateConfirm = confirm(mgs);
            if (bulkUpdateConfirm == true) {
                console.log('Bulk updates for ' + objName.split("_")[2])

                let rs = true;
                for (var j = 0; j < row; j++) {


                    var Elem = masterTable.get_dataItems()[j].findElement(objName);
                    //**Fire Service Accepted onChange()**
                    console.log('Updating for ' + Elem.id);
                    //SET default value for Fire Service Accepted
                    Elem.value = item.value;
                    if (objName.split("_")[2] == 'FireServiceAccepted') {
                        $(Elem).val(item.value);
                    } else {
                        Elem.value = item.value;
                    }
                    var id = Elem.id;
                    var lblTestYear = document.getElementById(id.replace(objName, 'lblTestYear'));
                    var TestID = document.getElementById(id.replace(objName, 'hdnTestID'));
                    var EquipmentID = document.getElementById(id.replace(objName, 'hduid'));

                    var obj = new dtaa();
                    obj.TestID = $(TestID).val()
                    obj.EquipmentID = $(EquipmentID).val();
                    obj.TestCustomFieldID = objName.split("_")[3];
                    obj.Value = $(Elem).val();
                    obj.OldValue = "";
                    obj.strUrl = getURL();
                    obj.TestYear = lblTestYear.innerText;

                    if (BulkUpdateCustomField(obj) == false) {
                        rs = false;
                    }

                }

                if (rs) {

                    setTimeout(refreshGrid, 10000);

                }
                else {
                    var msg = "Test updated unsuccessfully!";
                    if (response.responseText.includes("Sending Email - Authentication Error")) {
                        msg = "Sending Email - Authentication Error";
                    }
                    noty({
                        text: msg,
                        type: 'error',
                        layout: 'topCenter',
                        closeOnSelfClick: false,
                        timeout: 5000,
                        theme: 'noty_theme_default',
                        closable: true
                    });
                }

            }
            else {
                var id = item.id;
                var lblTestYear = document.getElementById(id.replace(objName, 'lblTestYear'));
                var TestID = document.getElementById(id.replace(objName, 'hdnTestID'));
                var EquipmentID = document.getElementById(id.replace(objName, 'hduid'));

                var obj = new dtaa();
                obj.TestID = $(TestID).val()
                obj.EquipmentID = $(EquipmentID).val();
                obj.TestCustomFieldID = objName.split("_")[3];
                obj.Value = $(item).val();
                obj.OldValue = "";
                obj.strUrl = getURL();
                obj.TestYear = lblTestYear.innerText;

                UpdateCustomField(obj, false, false);

                setTimeout(refreshGrid, 10000);
            }



        }
        function refreshGrid() {
            document.getElementById('ctl00_ContentPlaceHolder1_lnkSearch').click();
            $('#MOMloading').hide();
            noty({
                text: 'Test updated successfully!',
                type: 'success',
                layout: 'topCenter',
                closeOnSelfClick: false,
                timeout: 5000,
                theme: 'noty_theme_default',
                closable: true
            });
        }
        function processCOAByFireDateForBulkUpdate(item, objName, objUpdateControl, xFireDay, xScheduleDay) {
            var txtFireDateID = item.id;
            var fireDate = document.getElementById(txtFireDateID).value;
            var txtScheduleDate = document.getElementById(txtFireDateID.replace(objName, 'txtScheduleDate'));
            var lblTestYear = document.getElementById(txtFireDateID.replace(objName, 'lblTestYear'));
            var TestID = document.getElementById(txtFireDateID.replace(objName, "hdnTestID"));
            var EquipmentID = document.getElementById(txtFireDateID.replace(objName, "hduid"));

            var updateControlID = document.getElementById(txtFireDateID.replace(objName, objUpdateControl));



            if (fireDate != "") {

                var oldValue = $(updateControlID).val();
                $(updateControlID).val(DueDateCalculations(fireDate, xFireDay));
                if (oldValue != $(updateControlID).val()) {
                    var obj = new dtaa();
                    obj.TestID = $(TestID).val()
                    obj.EquipmentID = $(EquipmentID).val();
                    obj.TestCustomFieldID = objUpdateControl.split("_")[3];
                    obj.Value = $(updateControlID).val();
                    obj.OldValue = oldValue;
                    obj.strUrl = getURL();
                    obj.TestYear = lblTestYear.innerText;
                    BulkUpdateCustomField(obj);
                }

            } else {
                var oldValue = $(updateControlID).val();;
                $(updateControlID).val(DueDateCalculations($(txtScheduleDate).val(), xScheduleDay));
                if (oldValue != $(updateControlID).val()) {
                    var obj = new dtaa();
                    obj.TestID = $(TestID).val()
                    obj.EquipmentID = $(EquipmentID).val();
                    obj.TestCustomFieldID = objUpdateControl.split("_")[3];
                    obj.Value = $(updateControlID).val();
                    obj.OldValue = oldValue;
                    obj.strUrl = getURL();
                    obj.TestYear = lblTestYear.innerText;
                    BulkUpdateCustomField(obj);
                }
            }
        }

        function processCOAByFireDate(item, objName, objUpdateControl, xFireDay, xScheduleDay) {

            var txtFireDateID = item.id;
            var fireDate = document.getElementById(txtFireDateID).value;
            var txtScheduleDate = document.getElementById(txtFireDateID.replace(objName, 'txtScheduleDate'));
            var lblTestYear = document.getElementById(txtFireDateID.replace(objName, 'lblTestYear'));
            var TestID = document.getElementById(txtFireDateID.replace(objName, "hdnTestID"));
            var EquipmentID = document.getElementById(txtFireDateID.replace(objName, "hduid"));

            var updateControlID = document.getElementById(txtFireDateID.replace(objName, objUpdateControl));



            if (fireDate != "") {

                var oldValue = $(updateControlID).val();
                $(updateControlID).val(DueDateCalculations(fireDate, xFireDay));
                if (oldValue != $(updateControlID).val()) {
                    var obj = new dtaa();
                    obj.TestID = $(TestID).val()
                    obj.EquipmentID = $(EquipmentID).val();
                    obj.TestCustomFieldID = objUpdateControl.split("_")[3];
                    obj.Value = $(updateControlID).val();
                    obj.OldValue = oldValue;
                    obj.strUrl = getURL();
                    obj.TestYear = lblTestYear.innerText;
                    UpdateCustomField(obj, false, false);
                }

            } else {
                var oldValue = $(updateControlID).val();;
                $(updateControlID).val(DueDateCalculations($(txtScheduleDate).val(), xScheduleDay));
                if (oldValue != $(updateControlID).val()) {
                    var obj = new dtaa();
                    obj.TestID = $(TestID).val()
                    obj.EquipmentID = $(EquipmentID).val();
                    obj.TestCustomFieldID = objUpdateControl.split("_")[3];
                    obj.Value = $(updateControlID).val();
                    obj.OldValue = oldValue;
                    obj.strUrl = getURL();
                    obj.TestYear = lblTestYear.innerText;
                    UpdateCustomField(obj, false, false);
                }
            }
        }

        function CheckDelete() {
            var result = false;
            var gridCount = $("#<%=RadGrid_SafetyTest.ClientID%> tbody tr input[type='checkbox']:checked").length;
            console.log(gridCount);
            result = gridCount > 0;

            if (result == true) {
                return confirm('Do you really want to delete this ?');
            }
            else {
                ShowMessage('Please select a record to delete.', 'warning');
                return false;
            }
        }

        function CheckAssign() {

            var result = false;
            var gridCount = $("#<%=RadGrid_SafetyTest.ClientID%> tbody tr input[type='checkbox']:checked").length;
            console.log(gridCount);
            result = gridCount > 0;

            if (result == true) {

                var hdnTicketID = $("[id$='chkSelect']:checked").closest("tr").find("[id$='hdnTicketID']").val();
                var hdnTestStatus = $("[id$='chkSelect']:checked").closest("tr").find("[id$='hdnTestStatus']").val();
                var hdnNumberOfTestNoTicketInLoc = $("[id$='chkSelect']:checked").closest("tr").find("[id$='hdnNumberOfTestNoTicketInLoc']").val();
                if (hdnTestStatus == "3") {
                    noty({
                        text: "Test is Inactive.",
                        type: 'warning',
                        layout: 'topCenter',
                        closeOnSelfClick: false,
                        timeout: true,
                        theme: 'noty_theme_default',
                        closable: true,
                        timeout: 3000
                    });
                }
                else {
                    if (hdnTicketID != "0" && hdnTicketID != "") {
                        noty({
                            text: "Ticket already assigned.",
                            type: 'warning',
                            layout: 'topCenter',
                            closeOnSelfClick: false,
                            timeout: true,
                            theme: 'noty_theme_default',
                            closable: true,
                            timeout: 3000
                        });
                    } else if (hdnNumberOfTestNoTicketInLoc >= 1) {
                        //return true;
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
            }
            else {
                ShowMessage('Please select a record to assign.', 'warning');

            }
            return false;
        }

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

        function SetDefaultDateRangeCss() {

            CssClearLabel();
            if (document.getElementById('<%= hdnRcvPymtSelectDtRange.ClientID%>').value == "Year") {
                $("#<%= lblYear.ClientID%>").addClass("labelactive");
            }
            else {
                $("#<%= lblWeek.ClientID%>").addClass("labelactive");
                document.getElementById('<%= hdnRcvPymtSelectDtRange.ClientID%>').value = "Week";
            }
        }

        ///////////// Select all checkbox ////////////////////
        function checkAllChecBox() {
            var checked = $(".chkSelectAll input").is(":checked");
            if (checked) {
                $(".chkSelect input").prop("checked", true)
            }
            else {
                $(".chkSelect input").prop("checked", false)
            }
        }

        ///////////// Unselect all checkbox ////////////////////
        function unCheckSelectAll() {
            var checked = $(".chkSelect input").is(":checked");
            var checkedAll = $(".chkSelectAll input").is(":checked");
            var checkCountCheckbox = $(".chkSelect input:checked").length;
            var checkCountCheckboxSelected = $(".chkSelect input").length
            if (checked && checkedAll) {
                $(".chkSelectAll input").prop("checked", false);
            }

            if (checkCountCheckbox === checkCountCheckboxSelected) {
                $(".chkSelectAll input").prop("checked", true);
            }

        }

        ///////////// Hide select all checkbox ////////////////////
        function hideSelectAllChkb() {
            $(".chkSelectAll").hide();
        }
        ///////////// Show select all checkbox ////////////////////
        function showSelectAllChkb() {
            $(".chkSelectAll").show();
        }


    </script>
      <script>

          function ShowRestoreGridSettingsButton() {

              document.getElementById("<%=lbSaveGridSettings.ClientID %>").style.display = "none";
              document.getElementById("<%=lbRestoreGridSettings.ClientID %>").style.display = "block";
          }

          function OnGridCreated(sender, args) {

              var frozenScroll = $get(sender.get_id() + "_Frozen");
              var allColumns = sender.get_masterTableView().get_columns();
              var scrollLeftOffset = 0;
              var allColumnsWidth = new Array;
              var grid = sender.get_element();
              for (var i = 0; i < allColumns.length; i++) {
                  allColumnsWidth[i] = allColumns[i].get_element().offsetWidth;
              }

              $get(sender.get_id() + "_GridData").onscroll = function (e) {
                  for (var i = 0; i < allColumns.length; i++) {
                      if (!allColumns[i].get_visible()) {
                          scrollLeftOffset += allColumnsWidth[i];
                      }
                      if ($telerik.isIE7) {
                          var thisColumn = grid.getElementsByTagName("colgroup")[0].getElementsByTagName("col")[i];
                          if (thisColumn.style.display == "none") {
                              scrollLeftOffset += parseInt(thisColumn.style.width);
                          }
                      }
                  }
                  var thisScrollLeft = this.scrollLeft;
                  if (frozenScroll != null) {
                      if (thisScrollLeft > 0)
                          frozenScroll.scrollLeft = thisScrollLeft + scrollLeftOffset + 300;
                      this.scrollLeft = 0;
                  }

                  scrollLeftOffset = 0;
              };
          }

          function headerMenuShowing(sender, args) {
              var session = '<%= Session["COPer"] %>';
              var menu = args.get_menu();

              for (var i = 0; i < menu.get_items().get_count(); i++) {
                  var item = menu.get_items().getItem(i);
                  if (item.get_value() != 'ColumnsContainer') {
                      item.get_element().style.display = 'none';
                  }
              }

              var columnsItem = menu.findItemByText("Columns");
              columnsItem.get_items().getItem(0).get_element().style.display = "none";


              //if (session != 1) {
              //    columnsItem.get_items().getItem(7).get_element().style.display = "none";
              //}
          }

          function ColumnSettingsChange(menu, args) {
              document.getElementById("<%=lbSaveGridSettings.ClientID %>").style.display = "block";
              document.getElementById("<%=lbRestoreGridSettings.ClientID %>").style.display = "none";
          }

          function GridCommand(sender, args) {
              if (args.get_commandName() == "Sort") {
                  ColumnSettingsChange();
              }
          }

          function SaveGridSettings() {
              document.getElementById('<%=lnkSaveGridSettings.ClientID%>').click();
              document.getElementById("<%=lbSaveGridSettings.ClientID %>").style.display = "none";
              document.getElementById("<%=lbRestoreGridSettings.ClientID %>").style.display = "block";
          }

          function RestoreGridSettings() {
              document.getElementById('<%=lnkRestoreGridSettings.ClientID%>').click();
              document.getElementById("<%=lbRestoreGridSettings.ClientID %>").style.display = "none";
              document.getElementById("<%=lbSaveGridSettings.ClientID %>").style.display = "none";
          }
      </script>
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

                           if (teamArr.indexOf(userId) >= 0) {
                               $(this).prop('checked', true);

                           } else {
                               $(this).prop('checked', false);

                           }
                       });
                   } else {
                       $("#<%=RadGrid_Emails.ClientID%> input[id*='chkSelect']:checkbox").prop('checked', false);
                   }
                   var membersValue = $("#<%= hdnOrgMemberDisp.ClientID%>");
                   if (membersValue.val() != null && membersValue.val() != "") {
                       teamMembers = membersValue.val();
                       var teamArr = teamMembers.toString().split(';');
                       // trim value of teamArr
                       $.each(teamArr, function (index, value) {
                           teamArr[index] = value.trim();
                       });

                       $("#<%=RadGrid_Emails.ClientID%> input[id*='chkSelect']:checkbox").each(function (index) {
                           var userId = $(this).closest('tr').find('td:eq(0)').find('span').html();

                           if (teamArr.indexOf(userId) >= 0) {
                               $(this).prop('checked', true);

                           } else {
                               $(this).prop('checked', false);

                           }
                       });
                   }
               }
           }

           function UpdatedivDisplayTeamMember() {
               var txtMembers = $("#<%=RadGrid_SafetyTest.ClientID %> input[id*='txtMembers']");
               $.each(txtMembers, function (index, item) {
                   var txtId = $(item).attr("id");
                   var div = document.getElementById(txtId.replace("txtMembers", "cusLabelTag"));

                   div.innerHTML = '';
                   var disTeamMembers = $(item).val().replaceAll(",", ";");
                   // Update selected for grid
                   if (disTeamMembers != null && disTeamMembers != "") {
                       var teamArr = disTeamMembers.toString().split(';');

                       // trim value of teamArr
                       $.each(teamArr, function (index, value) {
                           teamArr[index] = value.trim();
                       });

                       if (teamArr != null && teamArr.length > 0) {
                           for (var i = 0; i < teamArr.length; i++) {
                               tag = "<div class='chip chipUsers'><label  class='cusCheckContainer'>" + teamArr[i] + "<input type='checkbox' disabled><span class='checkmark'></span></label></div>";

                               div.innerHTML += tag;
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

                   while (hdnMembersValue.charAt(0) === ';') {
                       hdnMembersValue = hdnMembersValue.substring(1);
                   }
                   while (txtMembersValue.charAt(0) === ';') {
                       txtMembersValue = txtMembersValue.substring(1);
                   }

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
                       // trim value of teamArr
                       $.each(teamArr, function (index, value) {
                           teamArr[index] = value.trim();
                       });

                       if (teamArr != null && teamArr.length > 0)
                           for (var i = 0; i < teamArr.length; i++) {
                               tag = "<div class='chip chipUsers'><label  class='cusCheckContainer'>" + teamArr[i] + "<input type='checkbox' disabled><span class='checkmark'></span></label></div>";
                               div.innerHTML += tag;
                           }
                   }




                   var radGridSafetyTest = $find('<%=RadGrid_SafetyTest.ClientID %>');
                   var masterTable = radGridSafetyTest.get_masterTableView();
                   var row = masterTable.get_dataItems().length;

                   var mgs = "Do you want to change/update the scheduled worker for all " + row + " records?"
                   var bulkUpdateConfirm = confirm(mgs);
                   if (bulkUpdateConfirm == true) {
                       console.log('Bulk updates for scheduled worker...')

                       var showalertOncetime = false;
                       for (var j = 0; j < row; j++) {

                           if (j == 0) {
                               showalertOncetime = true;
                           } else { showalertOncetime = false; }
                           var cusLabelTagElem = masterTable.get_dataItems()[j].findElement("cusLabelTag");


                           console.log('Updating for ' + cusLabelTagElem.id);

                           var BulkhdnElementID = cusLabelTagElem.id;
                           var BulktxtScheduleDate = document.getElementById(BulkhdnElementID.replace('cusLabelTag', 'txtScheduleDate'));
                           var BulkhdnTestID = document.getElementById(BulkhdnElementID.replace('cusLabelTag', 'hdnTestID'));
                           var BulklblTestYear = document.getElementById(BulkhdnElementID.replace('cusLabelTag', 'lblTestYear'));
                           var BulkhdnScheduledStatus = document.getElementById(BulkhdnElementID.replace('cusLabelTag', 'hdnScheduledStatus'));
                           var BulktxtScheduleDateID = BulkhdnElementID.replace('cusLabelTag', 'txtScheduleDate');
                           var BulktxtMembers = BulkhdnElementID.replace('cusLabelTag', 'txtMembers');
                           var Bulkdiv = document.getElementById(BulkhdnElementID);
                           var BulkhdnMembersValue = BulkhdnElementID.replace('cusLabelTag', 'hdnMembers');

                           $(BulkhdnMembersValue).val(hdnMembersValue);

                           $(BulktxtMembers).val(txtMembersValue);

                           Bulkdiv.innerHTML = div.innerHTML;


                           var Bulkstatus = 0;
                           Bulkstatus = $(BulkhdnScheduledStatus).val();
                           if ($(BulkhdnScheduledStatus).val() == "") {
                               Bulkstatus = 0;
                           }
                           if (BulkhdnMembersValue != "") {
                               UpdateTestSchedule($(BulkhdnTestID).val(), BulklblTestYear.innerText, BulktxtScheduleDate.value, Bulkstatus, hdnMembersValue.replaceAll(";", ","), BulktxtScheduleDateID);
                           }


                       }

                   }
                   else {





                       var hdnElementID = $("#<%= hdnElementID.ClientID%>").val();
                       var txtScheduleDate = document.getElementById(hdnElementID.replace('cusLabelTag', 'txtScheduleDate'));
                       var hdnTestID = document.getElementById(hdnElementID.replace('cusLabelTag', 'hdnTestID'));
                       var lblTestYear = document.getElementById(hdnElementID.replace('cusLabelTag', 'lblTestYear'));
                       var hdnScheduledStatus = document.getElementById(hdnElementID.replace('cusLabelTag', 'hdnScheduledStatus'));
                       var txtScheduleDateID = hdnElementID.replace('cusLabelTag', 'txtScheduleDate');
                       var status = 0;
                       status = $(hdnScheduledStatus).val();
                       if ($(hdnScheduledStatus).val() == "") {
                           status = 0;
                       }
                       if (hdnMembersValue != "") {
                           UpdateTestSchedule($(hdnTestID).val(), lblTestYear.innerText, txtScheduleDate.value, status, hdnMembersValue.replaceAll(";", ","), txtScheduleDateID);
                       }


                   }
                   //
               }











               var wnd = $find('<%=TeamMembersWindow.ClientID %>');
               wnd.Close();


               noty({
                   text: ' scheduled worker updated successfully. ',
                   type: 'success',
                   layout: 'topCenter',
                   closeOnSelfClick: false,
                   timeout: 5000,
                   theme: 'noty_theme_default',
                   closable: true
               });
           }



           function ShowTeamMemberWindow(txtTeamMember) {

               var txtTeamMembersId = $(txtTeamMember).attr("id");
               var line = txtTeamMembersId.replace("cusLabelTag", "hdnLine");
               var hdnTeamMembersId = txtTeamMembersId.replace("cusLabelTag", "hdnMembers");
               var teamMembers = $("#" + hdnTeamMembersId).val().replaceAll(",", ";");
               var txtTicket = txtTeamMembersId.replace("cusLabelTag", "txtTicket");
               if ($("#" + txtTicket).val() == "") {



                   var txtTeamMemberDispId = txtTeamMembersId.replace("cusLabelTag", "txtMembers");
                   var txtTeamMemberDispVal = $("#" + txtTeamMemberDispId).val();

                   $('#<%= hdnElementID.ClientID%>').val(txtTeamMembersId);
                   $('#<%= hdnLineOpenned.ClientID%>').val($("#" + line).val());
                   $('#<%= hdnOrgMemberKey.ClientID%>').val(teamMembers.replaceAll(",", ";"));
                   $('#<%= hdnOrgMemberDisp.ClientID%>').val(txtTeamMemberDispVal.replaceAll(",", ";"));

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

                       // trim value of teamArr
                       $.each(teamArr, function (index, value) {
                           teamArr[index] = value.trim();
                       });

                       $("#<%=RadGrid_Emails.ClientID%> input[id*='chkSelect']:checkbox").each(function (index) {
                        var userId = $(this).closest('tr').find('td:eq(0)').find('span').html();

                        var idx = teamArr.indexOf(userId);
                        if (idx >= 0) {
                            $(this).prop('checked', true);

                        } else {
                            $(this).prop('checked', false);

                        }
                    });
                } else {
                    $("#<%=RadGrid_Emails.ClientID%> input[id*='chkSelect']:checkbox").prop('checked', false);
                   }

                   var wnd = $find('<%=TeamMembersWindow.ClientID %>');
                   wnd.set_title("Team Member ");
                   wnd.Show();
               }
           }

           function CheckEmailsCheckBox(gridview) {
               var line = $("#<%= hdnLineOpenned.ClientID%>").val();
               var hdnMembersValue = $("#<%= hdnOrgMemberKey.ClientID%>").val();
               var txtMembersValue = $("#<%= hdnOrgMemberDisp.ClientID%>").val();
               var selectedMembers = hdnMembersValue.split(';');
               var selectedDisplayMembers = txtMembersValue.split(';');
               var tempArrayKey = [];
               tempArrayKey.length = 0;
               var tempArrayDisplay = [];
               tempArrayDisplay.length = 0;

               $("#" + gridview + " input[id*='chkSelect']:checkbox").each(function (index) {
                   var tempMemberKey = $(this).closest('tr').find('td:eq(0)').find('span').html().trim();
                   var teamMemberDisp = $(this).closest('tr').find('td:eq(1)').find('span').html().trim();
                   if (teamMemberDisp == "")
                       teamMemberDisp = $(this).closest('tr').find('td:eq(2)').find('span').html().trim();

                   var index = selectedMembers.indexOf(tempMemberKey);
                   var indexDisplay = selectedDisplayMembers.indexOf(tempMemberKey);
                   if (index > -1) {
                       if ($(this).is(":checked")) {
                           //do nothing
                       }
                       else {
                           //remove from selectedMemebers
                           selectedMembers.splice(index, 1);
                       }
                   }
                   else {
                       if ($(this).is(":checked")) {
                           selectedMembers.push(tempMemberKey);
                       }
                   }

                   if (indexDisplay > -1) {
                       if ($(this).is(":checked")) {
                           //do nothing
                       }
                       else {
                           //remove from selectedMemebers
                           selectedDisplayMembers.splice(index, 1);
                       }
                   }
                   else {
                       if ($(this).is(":checked")) {
                           selectedDisplayMembers.push(teamMemberDisp);
                       }
                   }



                   if ($(this).is(":checked")) {
                       tempArrayKey.push(tempMemberKey);
                       tempArrayDisplay.push(teamMemberDisp);
                   } else {
                       if (jQuery.inArray(tempMemberKey, tempArrayKey) >= 0) {
                           tempArrayKey = jQuery.grep(tempArrayKey, function (value) {
                               return value != tempMemberKey;
                           });
                           tempArrayDisplay = jQuery.grep(tempArrayDisplay, function (value) {
                               return value != teamMemberDisp;
                           });
                       }

                   }
               });

               $("#<%= hdnOrgMemberKey.ClientID%>").val(selectedMembers.join(";"));
               $("#<%= hdnOrgMemberDisp.ClientID%>").val(selectedDisplayMembers.join(";"));
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

       </script>
         <style type="text/css">
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

        td>div .RadComboBox {
            width:7em;
        }
        .rgPagerCell .NextPrevAndNumeric{
            padding: 0px!important;

        }
        .trost{
            margin-right: 6px!important;
            margin-left: 3px!important;
        }
             .RadGrid .rgFilterRow > td {
                    padding-left: 7px !important;
             }
        .RadGrid .rgFilterRow > td >div {
    padding-left: 0px !important;
    padding-right: 0px !important;
    padding-bottom: 0px !important;
    padding-top: 0px !important;
}
        .RadComboBox_Bootstrap .rcbInner {
            margin-top: 9px !important;
        }
          td>div{
            padding: 1px !important;
        }
        td > span {
           display:block;
             padding: 5px 8px !important;
        }

        .RadGrid_Popup > div > div.rgDataDiv {
            height: 450px !important;
        }
 /* The container */
        .cusCheckContainer {
            display: block;
            position: relative;
            padding-left: 15px;
            /* margin-bottom: 12px; */
            cursor: pointer;
            font-size: 9px;
            -webkit-user-select: none;
            -moz-user-select: none;
            -ms-user-select: none;
            user-select: none;
            color: white !important;
            padding-right : 4px;

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
        .divbutton-container {
    height: 82px !important;
}
        .RadGrid_SafetyTest [id$='chkSelectSelectCheckBox'] {
            margin-left: 10px !important;
        }

        .RadGrid_SafetyTest [id$='_PageSizeComboBox'] {
            width: 63px !important;
            margin-top:-15px;
        }

        .RadGrid_SafetyTest [id$='_AddNewRecordButton'] {
            display: none;
        }
       
        .RadGrid_SafetyTest .rgFilterRow  >td > div > input{
           height:1.6rem !important;
            margin-top:-3px !important;
        }
        .RadGrid_Bootstrap .rgFilterRow .riTextBox {
            border: none !important;
            border-bottom: solid 1px !important;           
            border-radius: 0 !important;
        }
        .RadPicker .rcSelect {
            border: none !important;
            height: 30px;
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

        .labelButton {
            padding: 5px 10px 5px 10px;
            font-size: 0.9em;
            float: left;
            line-height: 19px !important;
            border-radius: 3px;
            background-color: #1C5FB1 !important;
            color: #fff !important;
            margin: 3px -9px;
            cursor: pointer;    
            margin-right: 15px;
        }
        .ui-autocomplete{
            width:200px;
        }
        .LI1pnlGridButtons {
            margin-left:-10px;
        }
        .report_extend {
            min-width: 225px;
        }
        .ddlScheduledCustom {
            margin-left:5px;
        }
        .RadGrid_Bootstrap .rgFilterBox {
            padding-left:0 !important;
        }
        .RadGrid_Material .rgHeader {
    padding: 5px 3px !important;
        }
        .sels-wid{
            width:120px!important;
        }
        .datepicker_mom{
            width: 100px !important;
        }
        .browser-default{
            min-width: 120px !important;
        }
        .srchcstm{
                width: 169px !important;
        }
        @media only screen and (min-width: 250px) and (max-width: 700px){
            .divbutton-container {
                 
                 margin-bottom: 42px!important;
             }
            .sels-wid, .datepicker_mom, .browser-default, .srchcstm{
            width:100%!important;
            }
             .RadGrid .rgCommandRow,srchcstm  {
                line-height: 21px!important;
             }
             .pd-negatenw{
                 text-align:center;
             }
            }
    </style>
  

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
   
      <telerik:RadAjaxManager ID="RadAjaxManager_SafetyTest" runat="server">
        <AjaxSettings>
                       
             
        
            <telerik:AjaxSetting AjaxControlID="lnkDelete">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_SafetyTest" LoadingPanelID="RadAjaxLoadingPanel_SafetyTest" />
                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="lnkSearch">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_SafetyTest" LoadingPanelID="RadAjaxLoadingPanel_SafetyTest" />
                     <telerik:AjaxUpdatedControl ControlID="txtStartDate"  />
                       <telerik:AjaxUpdatedControl ControlID="txtEndDate"  />
                    <telerik:AjaxUpdatedControl ControlID="lblRecordCount"  />
                   
                </UpdatedControls>
            </telerik:AjaxSetting>
            
            <telerik:AjaxSetting AjaxControlID="lnkAssignTicket">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_SafetyTest" LoadingPanelID="RadAjaxLoadingPanel_SafetyTest" />                   
                    <telerik:AjaxUpdatedControl ControlID="lblRecordCount"  />                   
                </UpdatedControls>
            </telerik:AjaxSetting>
          
            <telerik:AjaxSetting AjaxControlID="RadGrid_SafetyTest">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_SafetyTest" LoadingPanelID="RadAjaxLoadingPanel_SafetyTest" />                   
                    <telerik:AjaxUpdatedControl ControlID="lblRecordCount"  />                   
                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="lnkClear">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_SafetyTest" LoadingPanelID="RadAjaxLoadingPanel_SafetyTest" />
                    <telerik:AjaxUpdatedControl ControlID="txtStartDate"  />
                    <telerik:AjaxUpdatedControl ControlID="txtEndDate"  />
                    <telerik:AjaxUpdatedControl ControlID="ddlSearch"  />
                    <telerik:AjaxUpdatedControl ControlID="txtSearch"  />
                    <telerik:AjaxUpdatedControl ControlID="ddlTestTypes"  />
                    <telerik:AjaxUpdatedControl ControlID="ddlProposal"  />
                    <telerik:AjaxUpdatedControl ControlID="ddlBillingAmount"  />
                    <telerik:AjaxUpdatedControl ControlID="lblRecordCount"  />
                    <telerik:AjaxUpdatedControl ControlID="lnkChk"  />
                    <telerik:AjaxUpdatedControl ControlID="lnkIncludeInActiveTest"  />
                    <telerik:AjaxUpdatedControl ControlID="isShowAll" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            
            <telerik:AjaxSetting AjaxControlID="lnkShowAll">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_SafetyTest" LoadingPanelID="RadAjaxLoadingPanel_SafetyTest" />
                    <telerik:AjaxUpdatedControl ControlID="txtStartDate"  />
                    <telerik:AjaxUpdatedControl ControlID="txtEndDate"  />
                    <telerik:AjaxUpdatedControl ControlID="lblRecordCount"  />
                    <telerik:AjaxUpdatedControl ControlID="lnkChk"  />
                    <telerik:AjaxUpdatedControl ControlID="lnkIncludeInActiveTest"  />
                    <telerik:AjaxUpdatedControl ControlID="isShowAll" />
                    <telerik:AjaxUpdatedControl ControlID="ddlSearch"  />
                    <telerik:AjaxUpdatedControl ControlID="txtSearch"  />
                    <telerik:AjaxUpdatedControl ControlID="ddlTestTypes"  />
                    <telerik:AjaxUpdatedControl ControlID="ddlProposal"  />
                    <telerik:AjaxUpdatedControl ControlID="ddlBillingAmount"  />                  
                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="lnkChk">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_SafetyTest" LoadingPanelID="RadAjaxLoadingPanel_SafetyTest" />
                     <telerik:AjaxUpdatedControl ControlID="txtStartDate"  />
                    <telerik:AjaxUpdatedControl ControlID="txtEndDate"  />
                    <telerik:AjaxUpdatedControl ControlID="lblRecordCount"  />
                    <telerik:AjaxUpdatedControl ControlID="lnkChk"  />
                    <telerik:AjaxUpdatedControl ControlID="lnkIncludeInActiveTest"  />
                    <telerik:AjaxUpdatedControl ControlID="isShowAll" />
                    <telerik:AjaxUpdatedControl ControlID="ddlSearch"  />
                    <telerik:AjaxUpdatedControl ControlID="txtSearch"  />
                    <telerik:AjaxUpdatedControl ControlID="ddlTestTypes"  />
                    <telerik:AjaxUpdatedControl ControlID="ddlProposal"  />
                    <telerik:AjaxUpdatedControl ControlID="ddlBillingAmount"  />  
                       <telerik:AjaxUpdatedControl ControlID="lblRecordCount"  />
                </UpdatedControls>
             </telerik:AjaxSetting>
          
            <telerik:AjaxSetting AjaxControlID="lnkIncludeInActiveTest">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_SafetyTest" LoadingPanelID="RadAjaxLoadingPanel_SafetyTest" />
                     <telerik:AjaxUpdatedControl ControlID="txtStartDate"  />
                    <telerik:AjaxUpdatedControl ControlID="txtEndDate"  />
                    <telerik:AjaxUpdatedControl ControlID="lblRecordCount"  />
                    <telerik:AjaxUpdatedControl ControlID="lnkChk"  />
                    <telerik:AjaxUpdatedControl ControlID="isShowAll" />
                    <telerik:AjaxUpdatedControl ControlID="ddlSearch"  />
                    <telerik:AjaxUpdatedControl ControlID="txtSearch"  />
                    <telerik:AjaxUpdatedControl ControlID="ddlTestTypes"  />
                    <telerik:AjaxUpdatedControl ControlID="ddlProposal"  />
                    <telerik:AjaxUpdatedControl ControlID="ddlBillingAmount"  />  
                       <telerik:AjaxUpdatedControl ControlID="lblRecordCount"  />
                </UpdatedControls>
            </telerik:AjaxSetting>
          
            <telerik:AjaxSetting AjaxControlID="lnkSaveGridSettings">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_SafetyTest" />
                </UpdatedControls>
            </telerik:AjaxSetting>
         
            <telerik:AjaxSetting AjaxControlID="lnkRestoreGridSettings">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_SafetyTest" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        
            <telerik:AjaxSetting AjaxControlID="RadGrid_SafetyTest">
                <UpdatedControls>
                         <telerik:AjaxUpdatedControl ControlID="RadGrid_SafetyTest" LoadingPanelID="RadAjaxLoadingPanel_SafetyTest" />
                     <telerik:AjaxUpdatedControl ControlID="txtStartDate"  />
                       <telerik:AjaxUpdatedControl ControlID="txtEndDate"  />
                    <telerik:AjaxUpdatedControl ControlID="lblRecordCount"  />
                </UpdatedControls>
            </telerik:AjaxSetting>
         
            <telerik:AjaxSetting AjaxControlID="lnkMailAll">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_gvLogs" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>

      <div class="divbutton-container" style="height: 50px !important;">
        <div id="divButtons">
            <div id="breadcrumbs-wrapper">
                <header>
                    <div class="container row-color-grey">
                        <div class="row">
                            <div class="col s12 m12 l12">
                                <div class="row">
                                    <div class="page-title"><i class="mdi-action-info"></i>&nbsp;Safety Tests</div>
                                    <asp:Panel runat="server" ID="pnlGridButtons">
                                        <div class="buttonContainer">
                                            <div class="btnlinks">
                                                <asp:LinkButton ID="lnkAddnew" runat="server" OnClick="lnkAddTests_Click">Add</asp:LinkButton>
                                            </div>
                                            <div class="btnlinks">
                                                <asp:LinkButton ID="btnEdit" runat="server" OnClick="btnEdit_Click">Edit</asp:LinkButton>
                                            </div>
                                            <div class="btnlinks menuAction">
                                                <a onclick="DropdownMenu()" class="dropbtn" style="cursor: pointer;">Actions
                                                </a>

                                            </div>
                                            <ul id="drpMenu" class="nomgn hideMenu menuList">
                                                <li>
                                                    <div class="btnlinks">
                                                        <asp:LinkButton ID="lnkDelete" runat="server" OnClientClick="return CheckDelete();" OnClick="lnkDelete_Click">Delete</asp:LinkButton>
                                                    </div>
                                                </li>
                                                <li>
                                                    <div class="btnlinks">
                                                        <asp:LinkButton ID="lnkExcel" runat="server" OnClick="lnkExcel_Click">Export to Excel</asp:LinkButton>

                                                    </div>
                                                </li>
                                                <li>
                                                    <div class="btnlinks">
                                                          <asp:LinkButton ID="lnkAssign" runat="server" OnClientClick="CheckAssign(); return false; " >Assign</asp:LinkButton>
                                                  <%--<div style="display:none">
                                                        <asp:LinkButton ID="lnkAssignTicket" runat="server"  OnClick="lnkAssignTicket_Click">Assign</asp:LinkButton>
                                                      </div>--%>
                                                         <asp:Button ID="lnkAssignTicket" runat="server" Text="Assign" Style="display: none;" OnClick="lnkAssignTicket_Click" CausesValidation="False" />
                        
                                                    </div>
                                                </li>
                                                <li>
                                                    <div class="btnlinks">
                                                        <asp:LinkButton ID="LnkGenerateProposal" runat="server" OnClick="LnkGenerateProposal_Click">Generate Proposal</asp:LinkButton>
                                                    </div>
                                                </li>
                                                <li>
                                                    <div class="btnlinks">
                                                        <asp:LinkButton ID="lnkEmail" runat="server" OnClick="lnkEmail_Click" OnClientClick="return confirm('Are you sure you want to send email?')">Email All</asp:LinkButton>

                                                        
                                                    </div>
                                                </li>
                                                
                                                 <li>
                                                    <div class="btnlinks">
                                                        <asp:LinkButton ID="lnkPDF" runat="server" OnClick="lnkPDF_Click">PDF</asp:LinkButton>
                                                    </div>
                                                </li>
                                                   <li>
                                                    <div class="btnlinks">
                                                      <asp:LinkButton ID="lnkAddcalendar" runat="server" OnClick="lnkAddcalendar_Click" OnClientClick="return confirm('Are you sure you want to Add Test to Calendar? .\n Note :- The schedule date format should be MM/DD/YYYY, please update the date format before proceeding.')">Add to Calendar</asp:LinkButton>
                                                    </div>
                                                </li>
                                                <li>
                                                    <div class="btnlinks">
                                                        <asp:LinkButton ID="lnkRestoreGridSettings" runat="server" CausesValidation="False" OnClick="lnkRestoreGridSettings_Click"
                                                            Style="display: none">Restore Grid</asp:LinkButton>
                                                    </div>
                                                    <div class="btnlinks">
                                                        <asp:LinkButton ID="lnkSaveGridSettings" runat="server" CausesValidation="False" OnClick="lnkSaveGridSettings_Click"
                                                            Style="display: none">Save Grid</asp:LinkButton>
                                                    </div>

                                                    <label id="lbSaveGridSettings" runat="server" class="labelButton" tooltip="Save Grid Settings" style="display: none">
                                                        <input type="radio" id="rdSaveGridSettings" onclick="SaveGridSettings();" />
                                                        Save Grid
                                                    </label>
                                                    <label id="lbRestoreGridSettings" runat="server" class="labelButton" tooltip="Restore Default Settings for Grid" style="display: none">
                                                        <input type="radio" id="rdRestoreGridSettings" onclick="RestoreGridSettings();" />
                                                        Restore Grid
                                                    </label>
                                                </li>
                                                <li>
                                                    <ul id="dynamicUI" class="dropdown-content">
                                                        <li class="report_extend">
                                                            <asp:LinkButton ID="lnkTESTReportLocation" runat="server" CausesValidation="true" OnClick="lnkTESTReportLocation_Click" Enabled="true">Test Scheduled Details Report</asp:LinkButton>
                                                        </li>

                                                    </ul>
                                                    <div class="btnlinks LI1pnlGridButtons" id="LI1pnlGridButtons" runat="server">
                                                        <a class="dropdown-button" data-beloworigin="true" href="#" data-activates="dynamicUI">Reports
                                                        </a>
                                                    </div>
                                                </li>
                                            </ul>
                                        </div>
                                    </asp:Panel>

                               
                                    <div class="btnclosewrap">
                                        <asp:LinkButton CssClass="icon-closed" ID="lnkClose" runat="server" CausesValidation="false" ToolTip="Close"
                                            TabIndex="39" OnClick="lnkClose_Click"><i class="mdi-content-clear"></i></asp:LinkButton>
                                    </div>

                                    <div class="rght-content">
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
                <div class="srchpaneinner">
                    <div class="srchtitle" >
                        <asp:DropDownList ID="ddldateRage123" runat="server"
                            CssClass="browser-default selectst  sels-wid"  >
                             <asp:ListItem Text="Next Due On" Value="Next Due On" Selected="True" > </asp:ListItem>
                             <asp:ListItem Text="Last Tested On" Value="Last Tested On" Selected="False" > </asp:ListItem>
                             <asp:ListItem Text="Last Due On" Value="Last Due On" Selected="False" > </asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="srchinputwrap">
                        <asp:TextBox ID="txtStartDate" runat="server"  CssClass="datepicker_mom"></asp:TextBox>
                    </div>
                    <div class="srchinputwrap">
                        <asp:TextBox ID="txtEndDate" runat="server"   CssClass="datepicker_mom"></asp:TextBox>
                    </div>
                    <div class="srchinputwrap pd-negatenw">
                       <ul class="tabselect accrd-tabselect" id="testradiobutton">
                            <li>
                                <asp:LinkButton AutoPostBack="False" ID="decDate" runat="server" Text="<i class='mdi-hardware-keyboard-arrow-left'></i>" OnClientClick="dec_date('dec','ctl00_ContentPlaceHolder1_txtStartDate','ctl00_ContentPlaceHolder1_txtEndDate','rdCal');return false;"></asp:LinkButton>
                            </li>
                            <li>
                                <label id="lblDay" runat="server">
                                    <input type="radio" id="rdDay" name="rdCal" value="rdDay" onclick="SelectDate('Day')" />
                                    Day
                                </label>
                            </li>
                            <li>
                                <label id="lblWeek" runat="server" >
                                    <input type="radio" id="rdWeek" name="rdCal" value="rdWeek" onclick="SelectDate('Week')" />
                                    Week
                                </label>
                            </li>
                            <li>
                                <label id="lblMonth" runat="server">
                                    <input type="radio" id="rdMonth" name="rdCal" value="rdMonth" onclick="SelectDate('Month')" />
                                    Month
                                </label>
                            </li>
                            <li>
                                <label id="lblQuarter" runat="server">
                                    <input type="radio" id="rdQuarter" name="rdCal" value="rdQuarter" onclick="SelectDate('Quarter')" />
                                    Quarter
                                </label>
                            </li>
                            <li>
                                <label id="lblYear" runat="server">
                                    <input type="radio" id="rdYear" name="rdCal" value="rdYear" onclick="SelectDate('Year')" />
                                    Year
                                </label>
                            </li>
                            <li>
                                <asp:LinkButton ID="incDate" runat="server" OnClientClick="dec_date('inc','ctl00_ContentPlaceHolder1_txtStartDate','ctl00_ContentPlaceHolder1_txtEndDate','rdCal');return false" Text="<i class='mdi-hardware-keyboard-arrow-right'></i>"></asp:LinkButton>
                            </li>
                        </ul>
                    </div>
                    <div class="col lblsz2 lblszfloat">
                        <div class="row">
                            <span class="tro trost">
                                <asp:CheckBox ID="lnkChk" runat="server" CssClass="css-checkbox" Text="Incl. Assigned" AutoPostBack="true" OnCheckedChanged="lnkChk_CheckedChanged"></asp:CheckBox>
                            </span>
                            <span class="tro trost">
                                <asp:CheckBox ID="lnkIncludeInActiveTest" runat="server" CssClass="css-checkbox" Text="Show Inactive" AutoPostBack="true" OnCheckedChanged="lnkIncludeInActiveTest_CheckedChanged"></asp:CheckBox>
                            </span>
                            <span class="tro trost">
                                <asp:LinkButton ID="lnkClear" runat="server" OnClick="lnkClear_Click">Clear </asp:LinkButton>
                            </span>
                            <span class="tro trost">
                                <asp:LinkButton ID="lnkShowAll" runat="server" OnClientClick="showAll_Test()" OnClick="lnkShowAll_Click">Show all Active </asp:LinkButton>
                            </span>
                            <span class="tro trost">
                                <asp:Label ID="lblRecordCount" runat="server">0 Record(s) found.</asp:Label>
                            </span>
                        </div>
                    </div>
                </div>

                <div class="srchpaneinner">


                    <div class="srchtitle srchtitlecustomwidth" >
                        Search
                    </div>

                    <div class="srchinputwrap">
                        <asp:DropDownList ID="ddlSearch" runat="server"
                            CssClass="browser-default selectst"  ClientIDMode="Static" onChange="showFilterSearchHistory()">
                           
                        </asp:DropDownList>
                    </div>

                    <div class="srchinputwrap">
                        <asp:TextBox ID="txtSearch" runat="server" CssClass="pd-negate srchcstm" placeholder="Search for Item, description" ToolTip="Search for Item, description" ></asp:TextBox>
                    </div>

                    <div class="srchinputwrap" >
                        <asp:DropDownList ID="ddlBillingAmount" runat="server"
                            CssClass="browser-default selectsml selectst" Style="display: none"
                            ClientIDMode="Static">
                             <asp:ListItem Value="">All</asp:ListItem>
                             <asp:ListItem Value="Default">Default Amount</asp:ListItem>
                            <asp:ListItem Value="Override">Override Amount</asp:ListItem>
                        </asp:DropDownList>
                    </div>

                    <div class="srchtitle srchtitlecustomwidth"">
                        Test Type
                    </div>

                    <div class="srchinputwrap">
                        <asp:DropDownList ID="ddlTestTypes" runat="server"
                            CssClass="browser-default selectsml selectst" Style="min-width: 200px !important;"
                            ClientIDMode="Static">
                        </asp:DropDownList>
                    </div>
                    
                    <div class="srchtitle srchtitlecustomwidth">
                        Proposal
                    </div>

                    <div class="srchinputwrap">
                        <asp:DropDownList ID="ddlProposal" runat="server"
                            CssClass="browser-default selectsml selectst" Style="min-width: 30px !important;"
                            ClientIDMode="Static">
                            <asp:ListItem Value="ALL">All</asp:ListItem>
                            <asp:ListItem Value="Yes">Yes</asp:ListItem>
                            <asp:ListItem Value="No">No</asp:ListItem>
                        </asp:DropDownList>
                    </div>

                    <div class="srchinputwrap srchclr btnlinksicon">
                        <asp:LinkButton ID="lnkSearch" runat="server" CausesValidation="false"
                            OnClick="lnkSearch_Click" >
                                           <i class="mdi-action-search"></i>
                        </asp:LinkButton>
                       
                        
                    </div>

                </div>

            </div>

            <div class="grid_container">
                <div class="form-section-row" >
                    

                    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel_SafetyTest" runat="server">
                    </telerik:RadAjaxLoadingPanel>

                    <div class="RadGrid RadGrid_Material">
                        <telerik:RadCodeBlock ID="codeBlock1" runat="server">
                            <script type="text/javascript">
                                function pageLoad() {
                                    var grid = $find("<%= RadGrid_SafetyTest.ClientID %>");
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

                                function SelectRow(index) {
                                    var grid = $find("<%= RadGrid_SafetyTest.ClientID%>");
                                    if (grid) {
                                        var MasterTable = grid.get_masterTableView();
                                        MasterTable.clearSelectedItems();
                                        var Rows = MasterTable.get_dataItems();
                                        var row = Rows[index];
                                        row.set_selected(true);
                                    }
                                }
                                var column = null;

                                function filterMenuShowing(sender, eventArgs) {
                                    // Set value for column to be used in MenuShowing().
                                    column = eventArgs.get_column();
                                }

                                function MenuShowing(menu, args) {

                                    if (column == null) return;

                                    // Iterate through filter menu items.
                                    var items = menu.get_items();
                                    for (i = 0; i < items.get_count(); i++) {
                                        var item = items.getItem(i);
                                        if (item === null)
                                            continue;

                                        // Make adjustments based on data type.
                                        switch (column.get_dataType()) {

                                            case "System.String":

                                                if (!(item.get_value() in { 'NoFilter': '', 'Contains': '', 'NotIsEmpty': '', 'IsEmpty': '', 'NotEqualTo': '', 'EqualTo': '' }))
                                                    item.set_visible(false);
                                                else
                                                    item.set_visible(true);
                                                break;

                                        }
                                    }

                                    column = null;
                                    menu.repaint();
                                }
                            </script>
                        </telerik:RadCodeBlock>
                               <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel_SafetyTest" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">
                        <telerik:RadGrid RenderMode="Auto" ID="RadGrid_SafetyTest" runat="server" LoadingPanelID="RadAjaxLoadingPanel_SafetyTest"
                            CssClass="RadGrid_SafetyTest"
                            AllowFilteringByColumn="true"
                            ShowFooter="True"
                            PageSize="25"
                            ShowStatusBar="true"
                            AllowPaging="True"
                            AllowSorting="true"
                            Width="100%"
                            PagerStyle-AlwaysVisible="true"
                            OnNeedDataSource="RadGrid_SafetyTest_NeedDataSource"
                            OnPreRender="RadGrid_SafetyTest_PreRender"
                            OnItemEvent="RadGrid_SafetyTest_ItemEvent"
                            OnItemCreated="RadGrid_SafetyTest_ItemCreated"
                            OnItemDataBound="RadGrid_SafetyTest_ItemDataBound"
                            OnExcelMLExportRowCreated="RadGrid_SafetyTest_ExcelMLExportRowCreated"
                            EnableLinqExpressions="false"
                            AllowAutomaticUpdates="True"
                            AllowAutomaticInserts="false"
                            OnBatchEditCommand="RadGrid_SafetyTest_BatchEditCommand"
                            ClientSettings-AllowColumnsReorder="true"
                            ClientSettings-ReorderColumnsOnClient="true"
                            ClientSettings-Scrolling-AllowScroll="true"
                            ClientSettings-Scrolling-FrozenColumnsCount="6"
                            ClientSettings-Scrolling-UseStaticHeaders="true"                                              
                            ClientSettings-ClientEvents-OnBatchEditCellValueChanged="TestSave"
                            OnPageIndexChanged="RadGrid_SafetyTest_PageIndexChanged"
                            OnPageSizeChanged="RadGrid_SafetyTest_PageSizeChanged"
                            AllowMultiRowEdit="true"
                                                        >
                            <CommandItemStyle />
                            <GroupingSettings CaseSensitive="false" />
                           
                            <ClientSettings>
                                <Selecting AllowRowSelect="True"></Selecting>
                                <Scrolling AllowScroll="true" SaveScrollPosition="true" FrozenColumnsCount="6" UseStaticHeaders="true" />
                                <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true" ></Resizing>
                                <ClientEvents
                                    OnGridCreated="OnGridCreated"           OnHeaderMenuShowing="headerMenuShowing"
                                    OnColumnHidden="ColumnSettingsChange"   OnColumnShown="ColumnSettingsChange"
                                    OnColumnResized="ColumnSettingsChange"  OnColumnSwapped="ColumnSettingsChange" />
                            </ClientSettings>
                            <FilterMenu OnClientShowing="MenuShowing" />
                            <MasterTableView AutoGenerateColumns="false" CommandItemSettings-ShowSaveChangesButton="false" CommandItemSettings-ShowRefreshButton="false" 
                                CommandItemSettings-ShowCancelChangesButton="false" AllowFilteringByColumn="true" ShowFooter="True" CommandItemDisplay="Top" 
                                EnableHierarchyExpandAll="false" EditMode="Batch" EnableHeaderContextMenu="true"  >
                                <PagerStyle AlwaysVisible="true" Mode="NextPrevAndNumeric" />
                            <BatchEditingSettings EditType="Cell" HighlightDeletedRows="true"  />
                                <Columns>
                                    <telerik:GridTemplateColumn HeaderStyle-Width="40" AllowFiltering="false" ShowFilterIcon="false">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkSelect" CssClass="chkSelect" runat="server" onchange="unCheckSelectAll();" />
                                                </ItemTemplate>
                                                <HeaderTemplate>
                                                    <asp:CheckBox ID="chkSelectAll" CssClass="chkSelectAll" onchange="checkAllChecBox();" runat="server" />
                                                </HeaderTemplate>
                                            </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn AllowFiltering="false" ShowFilterIcon="false" Visible="false">
                                        <ItemTemplate>
                                            <asp:HiddenField ID="hdnSelected" runat="server" />     
                                            <asp:HiddenField ID="hdnProposalId" runat="server" Value='<%# Eval("ProposalId") %>' ClientIDMode="Static" />
                                             <asp:HiddenField ID="hdnIsCoverTestType" runat="server" Value='<%# Eval("IsCoverTestType") %>' ClientIDMode="Static" />
                                            <asp:Label ID="lblLine" runat="server" Text='<%# Eval("LID") %>' CssClass="customline" Style="display: none;"></asp:Label>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    
                                    <telerik:GridTemplateColumn UniqueName="Image" ShowFilterIcon="false" AllowFiltering="false" HeaderStyle-Width="30">
                                        <ItemTemplate>
                                            <asp:HyperLink runat="server" ID="Images2" ImageWidth="15px" ImageUrl='<%# Eval("ProposalID").ToString() != "" ? "images/Document.png" : "data:image/png;base64,R0lGODlhAQABAIAAAAAAAP///yH5BAEAAAAALAAAAAABAAEAAAIBRAA7"%>' onclick='<%# string.Format("downloadFile({0},{1})",Eval("ProposalID"),1) %>'></asp:HyperLink>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>


                                    
                                        

                                    <telerik:GridTemplateColumn UniqueName="Tag" HeaderStyle-Width="200" DataField="Tag" HeaderText="Location Name" SortExpression="Tag" DataType="System.String"
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                        ShowFilterIcon="false">
                                        <ItemTemplate>
                                           
                                              <asp:Image ID="imgCreditH" runat="server" Width="16px" ToolTip="Credit Hold" ImageUrl='<%# Eval("credithold").ToString() != "0" ? "images/MSCreditHold.png" : "data:image/png;base64,R0lGODlhAQABAIAAAAAAAP///yH5BAEAAAAALAAAAAABAAEAAAIBRAA7"%>' />
                                             <asp:Label ID="lblAccount" runat="server" Text='<%# Eval("Tag") %>'></asp:Label>
                                               <asp:Label ID="lblcredithold" Visible="false" runat="server" Text='<%# Eval("credithold") %>'></asp:Label>
                                        </ItemTemplate> 
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn HeaderStyle-Width="200" DataField="Address" HeaderText="Location Address" SortExpression="Address" DataType="System.String"
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                        ShowFilterIcon="false"   UniqueName="Address">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAddress" runat="server" Text='<%# Eval("Address") %>'></asp:Label>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                      <telerik:GridTemplateColumn DataField="Unit" HeaderText="Equipment ID" SortExpression="Unit" DataType="System.String" HeaderStyle-Width="100"
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                        ShowFilterIcon="false" UniqueName="Unit">
                                        <ItemTemplate>
                                            <asp:Label ID="lblUnit" runat="server" Text='<%# Eval("Unit") %>'></asp:Label>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn DataField="State" HeaderText="Unique #" SortExpression="State" DataType="System.String"
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" HeaderStyle-Width="80"
                                        ShowFilterIcon="false"  UniqueName="State">
                                        <ItemTemplate>
                                            <asp:Label ID="lblState" runat="server" Text='<%# Eval("State") %>'></asp:Label>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn UniqueName="ProposalID" HeaderStyle-Width="100" DataField="ProposalID" HeaderText="Proposal ID" SortExpression="ProposalID" DataType="System.Int32"
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo"
                                        ShowFilterIcon="false">
                                        <ItemTemplate> 
                                               <asp:HiddenField ID="hdnid"       runat="server"       Value='<%# Eval("LID") %>'  />
                                               <asp:HiddenField ID="hduid"       runat="server"       Value='<%# Eval("NID") %>' />
                                               <asp:HiddenField ID="hdUnit"      runat="server"       Value='<%# Eval("Unit") %>' />
                                               <asp:HiddenField ID="hdnTestYear" runat="server"       Value='<%# Eval("TestYear") %>'  />
                                               <asp:Label ID="lblProposalID"     runat="server"       Text='<%# Eval("ProposalID") %>'></asp:Label>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn DataField="ProposalStatus" HeaderText="Proposal Status" SortExpression="ProposalStatus" DataType="System.String"
                                        AutoPostBackOnFilter="true"
                                        CurrentFilterFunction="Contains"
                                        HeaderStyle-Width="120"
                                        ShowFilterIcon="false" UniqueName="ProposalStatus" AllowFiltering="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblProposalStatus" runat="server" Text='<%# Eval("ProposalStatus") %>' Visible="false"></asp:Label>
                                             <asp:DropDownList ID="ddlStatusDocument" runat="server" OnSelectedIndexChanged="ddlStatusDocument_SelectedIndexChanged" AutoPostBack="true" CssClass="browser-default"  >
                                                                                              <asp:ListItem Value="Pending">Pending</asp:ListItem>
                                                                                              <asp:ListItem Value="Declined">Declined</asp:ListItem>
                                                                                              <asp:ListItem Value="Sold">Sold</asp:ListItem>
                                                                                              <asp:ListItem Value="Covered">Covered</asp:ListItem>
                                                                                        </asp:DropDownList>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn DataField="TestYear" HeaderText="Year" SortExpression="TestYear" DataType="System.String"
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" HeaderStyle-Width="100"
                                        ShowFilterIcon="false"  UniqueName="TestYear" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate >
                                            <asp:Label ID="lblTestYear" runat="server" Text='<%# Eval("TestYear") %>'></asp:Label>
                                        </ItemTemplate>
                                         <ItemStyle CssClass="center-align" />
                                    </telerik:GridTemplateColumn>                                 



                                            <telerik:GridTemplateColumn UniqueName="Name" HeaderStyle-Width="100" DataField="Name" HeaderText="Customer" SortExpression="Name" DataType="System.String"
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                        ShowFilterIcon="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCustomer" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn DataField="Status" HeaderText="Status" SortExpression="Status" DataType="System.String"
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" HeaderStyle-Width="62"
                                        ShowFilterIcon="false"  UniqueName="Status" >
                                        <ItemTemplate >
                                            <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("Status") %>'></asp:Label>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>  
                                    
                                    <telerik:GridTemplateColumn UniqueName="SendMailStatus" HeaderStyle-Width="80" DataField="SendMailStatus" HeaderText="Send Email" SortExpression="SendMailStatus" DataType="System.String"
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                        ShowFilterIcon="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSendMailStatus" runat="server" Text='<%# Eval("SendMailStatus") %>'></asp:Label>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn DataField="City" HeaderText="City" SortExpression="City" DataType="System.String"
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" HeaderStyle-Width="80"
                                        ShowFilterIcon="false"  UniqueName="City">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCity" runat="server" Text='<%# Eval("City") %>'></asp:Label>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn DataField="LocState" HeaderText="State" SortExpression="State" DataType="System.String"
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" HeaderStyle-Width="60"
                                        ShowFilterIcon="false"  UniqueName="LocState">
                                        <ItemTemplate>
                                            <asp:Label ID="lblLocState" runat="server" Text='<%# Eval("LocState") %>'></asp:Label>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    
                                    
                                  

                                    <telerik:GridTemplateColumn HeaderStyle-Width="80" SortExpression="Capacity" UniqueName="Capacity" AutoPostBackOnFilter="true" DataField="Capacity" DataType="System.String"
                                        CurrentFilterFunction="Contains" HeaderText="Capacity" ShowFilterIcon="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCapacity" runat="server" Text='<%# Eval("Capacity") %>'></asp:Label>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                

                                    <telerik:GridTemplateColumn HeaderStyle-Width="80" DataField="NTest" HeaderText="Test" SortExpression="NTest" DataType="System.String"
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                        ShowFilterIcon="false"  UniqueName="NTest">
                                        <ItemTemplate>
                                            <a href='<%# string.Format("AddTests.aspx?elv={0}&LID={1}",Eval("NID"),Eval("LID"))%>' >
                                                <asp:Label ID="lblTest" runat="server" Text='<%# Eval("NTest") %>' ></asp:Label>
                                            </a>

                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn DataField="ID" HeaderText="Acct#" SortExpression="ID" DataType="System.String"
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" HeaderStyle-Width="150"
                                        ShowFilterIcon="false"   UniqueName="ID">
                                        <ItemTemplate>
                                            <asp:Label ID="lblLid" runat="server" Text='<%# Eval("ID") %>'></asp:Label>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>  
                             
                                    <telerik:GridTemplateColumn HeaderStyle-Width="80" DataField="BillingAmount" HeaderText="Billing Amount" SortExpression="BillingAmount" DataType="System.Decimal" 
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo"
                                        ShowFilterIcon="false"  UniqueName="BillingAmount" FooterAggregateFormatString="{0:c}" Aggregate="Sum" >
                                        <ItemTemplate>                                           
                                                <asp:Label ID="lblBillingAmount" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "BillingAmount", "{0:c}")%>' ForeColor='<%# Convert.ToString(Eval("IsDefaultAmount"))=="Default Amount"?System.Drawing.Color.Black:System.Drawing.Color.Red %>'></asp:Label>                                          

                                        </ItemTemplate>                                        
                                    </telerik:GridTemplateColumn>
                                    
                                    <telerik:GridTemplateColumn HeaderStyle-Width="90" DataField="Classification" HeaderText="Classification" SortExpression="Classification" DataType="System.String"
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                        ShowFilterIcon="false" UniqueName="Classification">
                                        <ItemTemplate>
                                            <asp:Label ID="lblClassification" runat="server" Text='<%# Eval("Classification") %>'></asp:Label>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn
                                        AllowFiltering="true"
                                        SortExpression="Last"
                                        AutoPostBackOnFilter="true"
                                        DataField="Last"
                                        DataType="System.String"
                                        UniqueName="Last"
                                        AllowSorting="true"
                                        ShowSortIcon="true"
                                        CurrentFilterFunction="Contains"
                                        HeaderText="Last Tested On"
                                        ShowFilterIcon="false"
                                        HeaderStyle-Width="100">
                                        <ItemTemplate>

                                            <asp:Label
                                                ID="lblLast"
                                                runat="server"
                                                Visible='<%# Eval("Last").ToString() == "1/1/1900 12:00:00 AM" ? false : true %>'
                                                Text='<%# Eval("Last") == DBNull.Value ? "" : String.Format("{0:MM/dd/yyyy}", Eval("Last")) %>'>
                                            </asp:Label>

                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn AllowFiltering="true"
                                        SortExpression="Next"
                                        AutoPostBackOnFilter="true"
                                        DataField="Next"
                                        DataType="System.String"
                                        CurrentFilterFunction="Contains"
                                        HeaderText="Next Due On"
                                        ShowFilterIcon="false"
                                        UniqueName="Next"
                                        AllowSorting="true"
                                        ShowSortIcon="true"
                                        HeaderStyle-Width="100">
                                        <ItemTemplate>

                                            <asp:Label
                                                ID="lblNext"
                                                runat="server"
                                                Text='<%# Eval("Next") == DBNull.Value ? "01/01/1900" : String.Format("{0:MM/dd/yyyy}", Eval("Next")) %>'>
                                            </asp:Label>

                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn
                                        AllowFiltering="true"
                                        SortExpression="Lastdue"
                                        AutoPostBackOnFilter="true"
                                        DataField="Lastdue"
                                        DataType="System.String"
                                        UniqueName="Lastdue"
                                        AllowSorting="true"
                                        ShowSortIcon="true"
                                        CurrentFilterFunction="Contains"
                                        HeaderText="Last Due On"
                                        ShowFilterIcon="false"
                                        HeaderStyle-Width="100">
                                        <ItemTemplate>

                                            <asp:Label
                                                ID="lblLastdue"
                                                runat="server"
                                                Visible='<%# Eval("Lastdue").ToString() == "1/1/1900 12:00:00 AM" ? false : true %>'
                                                Text='<%# Eval("Lastdue") == DBNull.Value ? "" : String.Format("{0:MM/dd/yyyy}", Eval("Lastdue")) %>'>
                                            </asp:Label>

                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn HeaderStyle-Width="80"   
                                         DataField="ChargeableName"
                                         SortExpression="Chargeable"
                                          AutoPostBackOnFilter="true"
                                         CurrentFilterFunction="Contains"    
                                        HeaderText="Chargeable" ShowFilterIcon="false"   
                                        FilterControlWidth="80px" DataType="System.String"   UniqueName="ChargeableName" 
                                       >
                                        <ItemTemplate>
                                            
                                             <asp:Label
                                                ID="lblChargeable"
                                                runat="server"
                                                Text='<%# Eval("ChargeableName")%>'>
                                            </asp:Label>
                                         
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn HeaderStyle-Width="80"
                                        DataField="ThirdPartyName"
                                        SortExpression="ThirdPartyName"
                                        AutoPostBackOnFilter="true"
                                        CurrentFilterFunction="Contains"
                                        HeaderText="Third Party Name"
                                        ShowFilterIcon="false"
                                        FilterControlWidth="80px" DataType="System.String" UniqueName="ThirdPartyName">
                                        <ItemTemplate>
                                            <asp:Label ID="lblThirdPartyName" runat="server" Text='<%# Eval("ThirdPartyName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn DataField="RouteName" SortExpression="RouteName" UniqueName="RouteName" HeaderText="RouteName" HeaderStyle-Width="80"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                            ShowFilterIcon="false">
                                            <ItemTemplate>
                                            <asp:Label ID="lblRouteName" runat="server" Text='<%# Eval("RouteName") %>'></asp:Label>
                                        </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                               
                                    <telerik:GridTemplateColumn DataField="RecurringContract" SortExpression="RecurringContract" UniqueName="RecurringContract" HeaderText="Recurring Contract" HeaderStyle-Width="80"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                            ShowFilterIcon="false">
                                         <ItemTemplate>
                                            <asp:Label ID="lblRecurringContract" runat="server" Text='<%# Eval("RecurringContract") %>'></asp:Label>
                                        </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                   
                                    <telerik:GridTemplateColumn SortExpression="idTicket" AutoPostBackOnFilter="true" DataField="idTicket" DataType="System.String"
                                        CurrentFilterFunction="Contains" HeaderText="Tkt#" ShowFilterIcon="false" HeaderStyle-Width="80"  UniqueName="idTicket">
                                        <ItemTemplate>
                                            <div>
                                      <%--      <a href='<%# Eval("idTicket","addticket.aspx?id={0}&comp=0&pop=1&screen=STList")%>'><%# Eval("idTicket")%></a>--%>
                                           <asp:TextBox ID="txtTicket" Text='<%# Eval("idTicket") %>' runat="server" CssClass="txtTicket"></asp:TextBox>
                                              <asp:HiddenField ID="hdnLoc" runat="server" Value='<%# Eval("Loc") %>' />
                                               <asp:HiddenField ID="hdnTicketID" runat="server" Value='<%# Eval("idTicket") %>'  />
                                              <asp:HiddenField ID="hdnTestID" runat="server" Value='<%# Eval("LID") %>'  />
                                                <asp:HiddenField ID="hdnNumberOfTestNoTicketInLoc" runat="server" Value='<%# Eval("NumberOfTestNoTicketInLoc") %>'  />
                                                    <asp:HiddenField ID="IsTemp" runat="server" Value='<%# Eval("IsTemp") %>'  />
                                                         <asp:HiddenField ID="hdnTestStatus" runat="server" Value='<%# Eval("StatusValue") %>'  />
                                                </div>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn UniqueName="TicketStatusText" SortExpression="TicketStatusText" AutoPostBackOnFilter="true" DataField="TicketStatusText" DataType="System.String"
                                        CurrentFilterFunction="Contains" HeaderText="Tkt Status" ShowFilterIcon="false" HeaderStyle-Width="80">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTktStatus" runat="server" Text='<%# Eval("TicketStatusText") %>'></asp:Label>

                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                                                  
                                    <telerik:GridTemplateColumn HeaderStyle-Width="80" SortExpression="EDate" UniqueName="EDate" AutoPostBackOnFilter="true" DataField="EDate" DataType="System.String"
                                        CurrentFilterFunction="Contains" HeaderText="Scheduled Date" ShowFilterIcon="false">
                                        <ItemTemplate>
                                            <div>
                                            <asp:TextBox ID="txtScheduleDate" placeholder="MM/DD/YYYY" Text='<%# Eval("EDate") %>' runat="server" Enabled='<%# Eval("idTicket")!= DBNull.Value?false:true  %>'></asp:TextBox>
                                                </div>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                                                     
                                    <telerik:GridTemplateColumn HeaderStyle-Width="150" SortExpression="CallSign" UniqueName="CallSign" AutoPostBackOnFilter="true" DataField="CallSign" DataType="System.String"
                                        CurrentFilterFunction="Contains" HeaderText="Scheduled Worker" ShowFilterIcon="false">

                                        <ItemTemplate>
                                         
                                            <div class="tag-div materialize-textarea textarea-border" id="cusLabelTag" style="text-align: left !important; cursor: pointer; margin: 2px 11px;" onclick="ShowTeamMemberWindow(this);" runat="server"></div>
                                            <asp:HiddenField ID="hdnMembers" runat="server" Value='<%# Eval("CallSign").ToString().ToUpper().Replace(",",";") %>' />
                                            <asp:HiddenField ID="hdnLine" Value='<%# Eval("SafetyTestID") %>' runat="server" />     
                                            <asp:TextBox ID="txtMembers" class='<%# "txtMembers_" + Eval("SafetyTestID") %>' runat="server" Style='min-width: 100px !important; display: none;'
                                                Text='<%# Eval("CallSign").ToString().ToUpper()  %>'></asp:TextBox>
                                        </ItemTemplate>

                                    </telerik:GridTemplateColumn>
                                                                      
                                    <telerik:GridTemplateColumn HeaderStyle-Width="80" SortExpression="ScheduledStatus" UniqueName="ScheduledStatus" AutoPostBackOnFilter="true" DataField="ScheduledStatus" DataType="System.String"
                                        CurrentFilterFunction="Contains" HeaderText="Scheduled Status" ShowFilterIcon="false">
                                        <ItemTemplate>

                                            <asp:HiddenField ID="hdnScheduledStatus" Value='<%# Eval("ScheduledStatus")  %>' runat="server"></asp:HiddenField>

                                            <asp:DropDownList ID="ddlScheduledStatus" runat="server" AutoPostBack="true"  OnSelectedIndexChanged="ddlScheduledStatus_SelectedIndexChanged" CssClass="browser-default ddlScheduledCustom">
                                                <asp:ListItem Value="0">Pending</asp:ListItem>
                                                <asp:ListItem Value="1">Notified</asp:ListItem>
                                                <asp:ListItem Value="2">Accepted</asp:ListItem>
                                                <asp:ListItem Value="3">Cancelled</asp:ListItem>
                                            </asp:DropDownList>

                                           

                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                                                         
                                    <telerik:GridTemplateColumn Visible="false" HeaderStyle-Width="80" SortExpression="Company" UniqueName="Company" AutoPostBackOnFilter="true" DataField="Company" DataType="System.String"
                                        CurrentFilterFunction="Contains" HeaderText="Company" ShowFilterIcon="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCompany" runat="server" Text='<%# Eval("Company") %>'></asp:Label>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                </Columns>
                            </MasterTableView>
                        </telerik:RadGrid>
                      </telerik:RadAjaxPanel>
                    </div>
                </div>
            </div>

                   <%--Email Sending Logs--%>
            <div class="accordian-wrap">
                <div class="col s12 m12 l12">
                    <div class="row">
                        <ul class="collapsible popout collapsible-accordion form-accordion-head" data-collapsible="expandable">
                            <li id="tbLogs" runat="server" style="display: block">
                                <div id="accrdlogs" class="collapsible-header accrd accordian-text-custom"><i class="mdi-content-content-paste"></i>Email History Log</div>
                                <div class="collapsible-body">
                                    <div class="form-content-wrap">
                                        <div class="form-content-pd">
                                            <div class="grid_container">
                                                <div class="form-section-row" >
                                                    <div class="RadGrid RadGrid_Material">
                                                        <telerik:RadCodeBlock ID="RadCodeBlock6" runat="server">
                                                            <script type="text/javascript">
                                                                function pageLoadLog() {
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
                                                                <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="True" ShowFooter="True" AllowNaturalSort="false">
                                                                    <PagerStyle AlwaysVisible="true" Mode="NextPrevAndNumeric" />
                                                                    <Columns>
                                                                        
                                                                        <telerik:GridTemplateColumn DataField="EmailDate" SortExpression="EmailDate" AutoPostBackOnFilter="true" DataType="System.String"
                                                                            CurrentFilterFunction="Contains" HeaderText="Date" ShowFilterIcon="false">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lbldate" runat="server" Text='<%# Eval("EmailDate", "{0:M/d/yyyy}")%>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>
                                                                        <telerik:GridTemplateColumn DataField="EmailDate" SortExpression="EmailDate" AutoPostBackOnFilter="true" DataType="System.String"
                                                                            CurrentFilterFunction="Contains" HeaderText="Time" ShowFilterIcon="false">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lbltime" runat="server" Text='<%# Eval("EmailDate","{0: hh:mm tt}") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>
                                                                        <telerik:GridTemplateColumn DataField="Username" SortExpression="Username" AutoPostBackOnFilter="true"
                                                                            CurrentFilterFunction="Contains" HeaderText="User" ShowFilterIcon="false">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblUsername" runat="server" Text='<%# Eval("Username") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>
                                                                        <telerik:GridTemplateColumn DataField="Ref" SortExpression="Ref" AutoPostBackOnFilter="true" DataType="System.String"
                                                                            CurrentFilterFunction="Contains" HeaderText="Proposal" ShowFilterIcon="false">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblRef" runat="server" Text='<%# Eval("Ref") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>
                                                                        <telerik:GridTemplateColumn DataField="EmailFunction" SortExpression="EmailFunction" AutoPostBackOnFilter="true"
                                                                            CurrentFilterFunction="Contains" HeaderText="Function" ShowFilterIcon="false">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblEmailFunction" runat="server" Text='<%# Eval("EmailFunction") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>
                                                                        <telerik:GridTemplateColumn DataField="MailTo" SortExpression="MailTo" AutoPostBackOnFilter="true"
                                                                            CurrentFilterFunction="Contains" HeaderText="Mail To" ShowFilterIcon="false">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblEmailTo" runat="server" Text='<%# Eval("MailTo") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>
                                                                        <telerik:GridTemplateColumn DataField="Status" SortExpression="Status" AutoPostBackOnFilter="true"
                                                                            CurrentFilterFunction="Contains" HeaderText="Status" ShowFilterIcon="false">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("Status") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>
                                                                        <telerik:GridTemplateColumn DataField="UsrErrMessage" SortExpression="UsrErrMessage" AutoPostBackOnFilter="true"
                                                                            CurrentFilterFunction="Contains" HeaderText="Error Message" ShowFilterIcon="false" HeaderStyle-Width="305">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblUsrErrMessage" runat="server" Text='<%# Eval("UsrErrMessage") %>'></asp:Label>
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
                                    <%--<div style="clear: both;"></div>--%>
                                </div>
                            </li>
                        </ul>
                    </div>
                </div>
            </div>
        </div>
    </div>

      <telerik:RadWindow ID="TeamMembersWindow" Skin="Material" VisibleTitlebar="true" Behaviors="Default" CenterIfModal="true"
        Animation="FlyIn" AnimationDuration="200" RenderMode="Auto" VisibleStatusbar="false"
        runat="server" Modal="true" Width="1050" Height="635">
        <ContentTemplate>
            <telerik:RadAjaxPanel ID="RadAjaxPanel32" runat="server">
                <div class="mb-15">
                    <div class="form-section-row">
                        <div class="form-section">
                            <div class="row mb">
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
                                                 <PagerStyle AlwaysVisible="true" Mode="NextPrevAndNumeric" />
                                                <Columns>
                                                    <telerik:GridTemplateColumn AllowFiltering="false" HeaderStyle-Width="28" ShowFilterIcon="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblID" runat="server" Style="display: none;"><%#Eval("fDesc")%></asp:Label>
                                                            <asp:CheckBox ID="chkSelect" runat="server" />
                                                        </ItemTemplate>
                                                        <HeaderTemplate>
                                                            <asp:CheckBox ID="chkAll" runat="server" />
                                                        </HeaderTemplate>
                                                        <ItemStyle Width="0px"></ItemStyle>
                                                    </telerik:GridTemplateColumn>
                                                  
                                                    <telerik:GridTemplateColumn
                                                        DataField="fDesc" SortExpression="fDesc" AutoPostBackOnFilter="true" DataType="System.String"
                                                        CurrentFilterFunction="Contains" HeaderText="Worker" ShowFilterIcon="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblUserName" runat="server"><%#Eval("fDesc")%></asp:Label>
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
     
     <asp:HiddenField runat="server" ID="HiddenFieldYear" Value="0" />
    <asp:HiddenField runat="server" ID="HiddenFieldStatus"  Value="0" />
    <asp:HiddenField runat="server" ID="HiddenFieldLID"  Value="0" />
     <asp:HiddenField runat="server" ID="HiddenField1B"  Value="0" />
    <asp:Button runat="server" ID="Button1Schedule" OnClick="Button1Schedule_Click" />
    <asp:HiddenField runat="server" ID="hdnCssActive" Value="1" />
    <asp:HiddenField runat="server" ID="hdnRcvPymtSelectDtRange" Value="Week" />
    <asp:HiddenField runat="server" ID="isShowAll" Value="0" />

    <div style="display: none">
        <asp:Button runat="server" ID="btnProcessDownload" OnClick="btnProcessDownload_Click" />
        <asp:HiddenField runat="server" ID="hdnDownloadID" Value="0" />
        <asp:HiddenField runat="server" ID="hdnDownloadType" Value="0" />
        <asp:HiddenField runat="server" ID="hdnCreateTicketForAll" Value="0" />


        <asp:HiddenField ID="hdnLineOpenned" runat="server" />
        <asp:HiddenField ID="hdnOrgMemberKey" runat="server" />
        <asp:HiddenField ID="hdnOrgMemberDisp" runat="server" />

         <asp:HiddenField ID="hdnElementID" runat="server" />
          <asp:HiddenField ID="hdnFormularFieldID" runat="server" />
          <asp:HiddenField ID="hdnFormularValue" runat="server" />
          <asp:HiddenField ID="hdnFireTestDate" runat="server" />
    </div>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="javascriptHolder" runat="Server">
    <script type="text/javascript">
        function CssClearLabel() {
            $('#<%=lblDay.ClientID%>').removeClass("labelactive");
            $('#<%=lblWeek.ClientID%>').removeClass("labelactive");
            $('#<%=lblMonth.ClientID%>').removeClass("labelactive");
            $("#<%=lblQuarter.ClientID%>").removeClass("labelactive");
            $('#<%=lblYear.ClientID%>').removeClass("labelactive");
        }
        function SetActiveDateRangeCss(rangeName) {
           
            CssClearLabel();
            if (rangeName == "Day")
                $("#<%= lblDay.ClientID%>").addClass("labelactive");
            else if (rangeName == "Week")
                $("#<%= lblWeek.ClientID%>").addClass("labelactive");
            else if (rangeName == "Month")
                $("#<%= lblMonth.ClientID%>").addClass("labelactive");
            else if (rangeName == "Quarter")
                $("#<%= lblQuarter.ClientID%>").addClass("labelactive");
            else if (rangeName == "Year")
                $("#<%= lblYear.ClientID%>").addClass("labelactive");
            document.getElementById('<%= hdnRcvPymtSelectDtRange.ClientID%>').value = rangeName;
        }

        $(document).ready(function () {
            $('label input[type=radio]').click(function () {
                $('input[name="' + this.name + '"]').each(function () {
                    $(this.parentNode).toggleClass('labelactive', this.checked);
                });
            });
            if (typeof (Storage) !== "undefined") {
                
                // Retrieve
                var SesVar = '<%= Convert.ToString(Session["lblSafetyTestActive"])%>';
                var val;
                //val = localStorage.getItem("hdnReceivePODate");
                val = document.getElementById('<%= hdnRcvPymtSelectDtRange.ClientID%>').value;
                CssClearLabel();

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
                
                    if (SesVar != "") SetActiveDateRangeCss(SesVar);
                    else SetDefaultDateRangeCss();
                }
                //}
            }

            $(window).scroll(function () {
                if ($(window).scrollTop() >= 0) {
                    $('#divButtons').addClass('fixed-header');
                }
                if ($(window).scrollTop() <= 0) {
                    $('#divButtons').removeClass('fixed-header');
                }
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

        });

        function downloadFile(obj, type) {

            $("#<%= hdnDownloadID.ClientID %>").val(obj);
            $("#<%= hdnDownloadType.ClientID %>").val(type);
            var btn = document.getElementById('<%=btnProcessDownload.ClientID%>');
            btn.click();
        }
    </script>
    <script type="text/javascript">
        function showAll_Test() {
            $("#<%=lblDay.ClientID%>").removeClass("labelactive");
            $("#<%=lblWeek.ClientID%>").removeClass("labelactive");
            $("#<%=lblMonth.ClientID%>").removeClass("labelactive");
            $("#<%=lblQuarter.ClientID%>").removeClass("labelactive");
            $("#<%=lblYear.ClientID%>").addClass("labelactive");
            document.getElementById('<%= hdnRcvPymtSelectDtRange.ClientID%>').value = 'Year';
        } 
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
                var tt = document.getElementById(txtDateTo).value;

                var date = new Date(tt).toDateString();
                var newdate = new Date(date);

                newdate.addMonths(xMonth);

                var dd = newdate.getDate();
                var mm = newdate.getMonth() + 1;
                var y = newdate.getFullYear();

                var someFormattedDate = mm + '/1/' + y;
                document.getElementById(txtDateTo).value = someFormattedDate;


                //dec the to date 
                if (select == 'dec') {
                    var ti = document.getElementById(txtDateFrom).value;
                    var date = new Date(ti).toDateString();
                    var newdateti = new Date(date);
                    newdateti.addMonthsLastDec(xMonth);

                    var dd = newdateti.getDate();
                    var mm = newdateti.getMonth() + 1;
                    var y = newdateti.getFullYear();

                    var someFormattedDate = mm + '/' + dd + '/' + y;
                    document.getElementById(txtDateFrom).value = someFormattedDate;
                }

                else {
                    var ti = document.getElementById(txtDateFrom).value;

                    var date = new Date(ti).toDateString();
                    var newdateti = new Date(date);
                    newdateti.addMonthsLast(xMonth);

                    var dd = newdateti.getDate();
                    var mm = newdateti.getMonth() + 1;
                    var y = newdateti.getFullYear();

                    var someFormattedDate = mm + '/' + dd + '/' + y;
                    document.getElementById(txtDateFrom).value = someFormattedDate;
                }
            }


            else if (selected == 'rdQuarter') {
              
                //dec the from date 
                var tt = document.getElementById(txtDateTo).value;

                var date = new Date(tt);
                var newdate = new Date(date);

                newdate.setMonth(newdate.getMonth() + xQuarter);

                var dd = newdate.getDate();
                var mm = newdate.getMonth() + 1;
                var y = newdate.getFullYear();

                var someFormattedDate = mm + '/' + dd + '/' + y;
                document.getElementById(txtDateTo).value = someFormattedDate;
                //dec the to date 

                var TT = document.getElementById(txtDateFrom).value;

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
                document.getElementById(txtDateFrom).value = someFormattedDATE;
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
        function SelectDate(type) {
            var type = type;
            var txtDateFrom = "<%= txtStartDate.ClientID%>";
            var txtdateTo = "<%= txtEndDate.ClientID%>";
            CssClearLabel();
            if (type == 'Day') {
                var todaydate = new Date();
                var day = todaydate.getDate();
                var month = todaydate.getMonth() + 1;
                var year = todaydate.getFullYear();
                var datestring = month + "/" + day + "/" + year;
                document.getElementById(txtdateTo).value = datestring;
                document.getElementById(txtDateFrom).value = datestring;
                $("#<%= lblDay.ClientID%>").addClass("labelactive");
                document.getElementById('<%= hdnRcvPymtSelectDtRange.ClientID%>').value = "Day";
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
                $("#<%= lblWeek.ClientID%>").addClass("labelactive");
                document.getElementById('<%= hdnRcvPymtSelectDtRange.ClientID%>').value = "Week";
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
                $("#<%= lblMonth.ClientID%>").addClass("labelactive");
                document.getElementById('<%= hdnRcvPymtSelectDtRange.ClientID%>').value = "Month";
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
                $("#<%= lblQuarter.ClientID%>").addClass("labelactive");
                document.getElementById('<%= hdnRcvPymtSelectDtRange.ClientID%>').value = "Quarter";
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
                $("#<%= lblYear.ClientID%>").addClass("labelactive");
                document.getElementById('<%= hdnRcvPymtSelectDtRange.ClientID%>').value = "Year";
            }

            document.getElementById('<%= hdnCssActive.ClientID%>').value = "CssActive";
            var clickSearchButton = document.getElementById("<%= lnkSearch.ClientID %>");
            clickSearchButton.click();
            document.getElementById('<%= hdnCssActive.ClientID%>').value = "1";
        }
    </script>
    <script>      

        function showFilterSearchHistory() {
          
            var ddlSearch = $("#<%=ddlSearch.ClientID%>");
            var ddlBillingAmount = $("#<%=ddlBillingAmount.ClientID%>");
            var txtSearch = $("#<%=txtSearch.ClientID%>");

            ddlBillingAmount.css("display", "none");
            txtSearch.css("display", "none");
            txtSearch.val('');





            if (ddlSearch.val() === "IsDefaultAmount") {
                ddlBillingAmount.css("display", "block");
            } else {
                txtSearch.css("display", "block");
            }
            try {
                ddlBillingAmount.get(0).selectedIndex = 0;
            } catch (ex) {

            }
        }

        function ResetValueAll() {
            $("#<%=lblDay.ClientID%>").removeClass("labelactive");
            $("#<%=lblWeek.ClientID%>").removeClass("labelactive");
            $("#<%=lblMonth.ClientID%>").removeClass("labelactive");
            $("#<%=lblQuarter.ClientID%>").removeClass("labelactive");
            $("#<%=lblYear.ClientID%>").removeClass("labelactive");
            var ddlSearch = $("#<%=ddlSearch.ClientID%>");
            var ddlBillingAmount = $("#<%=ddlBillingAmount.ClientID%>");
            var txtSearch = $("#<%=txtSearch.ClientID%>");

            ddlBillingAmount.css("display", "none");
            txtSearch.css("display", "block");
            txtSearch.val('');

            try {
                ddlBillingAmount.get(0).selectedIndex = 0;
            } catch (ex) { }

        }

        function ResetValue() {
            $("#<%=lblDay.ClientID%>").removeClass("labelactive");
            $("#<%=lblWeek.ClientID%>").removeClass("labelactive");
            $("#<%=lblMonth.ClientID%>").removeClass("labelactive");
            $("#<%=lblQuarter.ClientID%>").removeClass("labelactive");
            $("#<%=lblYear.ClientID%>").removeClass("labelactive");
            var ddlSearch = $("#<%=ddlSearch.ClientID%>");
            var ddlBillingAmount = $("#<%=ddlBillingAmount.ClientID%>");
            var txtSearch = $("#<%=txtSearch.ClientID%>");

            ddlBillingAmount.css("display", "none");
            txtSearch.css("display", "block");
            txtSearch.val('');

            try {
                ddlBillingAmount.get(0).selectedIndex = 0;
            } catch (ex) { }

        }

        function dtaa() {
            this.TestID = null;
            this.EquipmentID = 0;
            this.TestCustomFieldID = 0;
            this.Value = null;
            this.OldValue = null;
            this.strUrl = "";
            this.TestYear = 0;
        }

        function TestSave(sender, args) {
            var row = args.get_row();
            var cell = args.get_cell();
            var columnUniqueName = args.get_columnUniqueName();
            var editorValue = args.get_editorValue();
            var cellValue = args.get_cellValue();

            var grid = $find("<%=RadGrid_SafetyTest.ClientID %>");
            var MasterTable = grid.get_masterTableView();
            var dtaaa = new dtaa();
            var testyear = 0;

            for (var i = 0; i < MasterTable.get_dataItems().length; i++) {
                if (i == row.rowIndex - 1) {
                    
                    var row = MasterTable.get_dataItems()[i];
                    var cell = MasterTable.getCellByColumnUniqueName(row, "ProposalID");
                    var objName = "ProposalID";
                    dtaaa.TestID = cell.childNodes[1].value;
                    dtaaa.EquipmentID = cell.childNodes[3].value;
                    dtaaa.TestCustomFieldID = columnUniqueName.split('_')[1];
                    dtaaa.Value = editorValue;
                    dtaaa.OldValue = cellValue;
                    dtaaa.strUrl = getURL();
                    dtaaa.TestYear = cell.childNodes[7].value;
                    break; 

                }
            }

            if (dtaaa.TestID != 0) {

                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: "CustomerAuto.asmx/SaveTestCustomerByYear",
                    data: JSON.stringify(dtaaa),
                    dataType: "json",
                    success: function (data) {
                        noty({
                            text: 'Test updated successfully!',
                            type: 'success',
                            layout: 'topCenter',
                            closeOnSelfClick: false,
                            timeout: 5000,
                            theme: 'noty_theme_default',
                            closable: true
                        });
                    },
                    error: function (response) {
                        noty({
                            text: 'Test updated unsuccessfully!',
                            type: 'error',
                            layout: 'topCenter',
                            closeOnSelfClick: false,
                            timeout: 5000,
                            theme: 'noty_theme_default',
                            closable: true
                        });
                    }
                });
            }

        }

        function getURL() {
            var pathname = window.location.pathname;
            var n = pathname.lastIndexOf("/");
            var arg = pathname.substring(0, n)
            var url = window.location.protocol + "//" + window.location.hostname + arg;
            return url;

        }

    </script>
    <script>
        function pageLoad(sender, args) {

            $('label input[type=radio]').click(function () {
                $('input[name="rdCal"]').each(function () {
                    $(this.parentNode).toggleClass('labelactive', this.checked);
                });
            });
            $("#<%=txtStartDate.ClientID %>").pikaday({
              firstDay: 0,
              format: 'MM/DD/YYYY',
              minDate: new Date(1900, 1, 1),
              maxDate: new Date(2100, 12, 31),
              yearRange: [1900, 2100]
          });
          $("#<%=txtEndDate.ClientID %>").pikaday({
              firstDay: 0,
              format: 'MM/DD/YYYY',
              minDate: new Date(1900, 1, 1),
              maxDate: new Date(2100, 12, 31),
              yearRange: [1900, 2100]
          });

          var grid = $find("<%= RadGrid_SafetyTest.ClientID %>");
            var columns = grid.get_masterTableView().get_columns();
            for (var i = 0; i < columns.length; i++) {
                columns[i].resizeToFit(false, true);
            }


            var query = "";
            function dtaaTicket() {
                this.prefixText = null;
                this.loc = 0;
                this.ticketYear = 0;
            }
            //txtTicket
            $("[id*=txtTicket]").autocomplete({

                source: function (request, response) {

                    var dtaaa = new dtaaTicket();
                    dtaaa.prefixText = request.term;
                    query = request.term;
                    var txtTicket = this.element[0].id;
                    var hdnLoc = document.getElementById(txtTicket.replace('txtTicket', 'hdnLoc'));
                     var lblTestYear = document.getElementById(txtTicket.replace('txtTicket', 'lblTestYear'));
                    dtaaa.loc = hdnLoc.value;
                    dtaaa.ticketYear = lblTestYear.innerText;
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "SafetyTestAuto.asmx/ServiceSearchTicketInLocation",
                        data: JSON.stringify(dtaaa),
                        dataType: "json",
                        async: true,
                        success: function (data) {
                            response($.parseJSON(data.d));
                        },
                        error: function (result) {

                            noty({ text: 'We were unable to load ticket', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                        }
                    });
                },
                select: function (event, ui) {
                  
                    var oldTicket = "0";
                    var txtTicket = this.id;
                    var lblStatus = document.getElementById(txtTicket.replace('txtTicket', 'lblStatus'));
                    var lblTestYear = document.getElementById(txtTicket.replace('txtTicket', 'lblTestYear'));
                    var hdnTestID = document.getElementById(txtTicket.replace('txtTicket', 'hdnTestID'));
                    var hdnTicketID = document.getElementById(txtTicket.replace('txtTicket', 'hdnTicketID'));
                   // var txtScheduledWorker = document.getElementById(txtTicket.replace('txtTicket', 'txtScheduledWorker'));
                     var hdnMembers = document.getElementById(txtTicket.replace('txtTicket', 'hdnMembers'));
                    var txtScheduleDate = document.getElementById(txtTicket.replace('txtTicket', 'txtScheduleDate'));
                    var lblTktStatus = document.getElementById(txtTicket.replace('txtTicket', 'lblTktStatus'));
                   
                    if ($(hdnTicketID).val() != "") {
                        oldTicket = $(hdnTicketID).val();
                    }


                    $(hdnTicketID).val(ui.item.ID);
                    $(this).val(ui.item.ID);
                    $(hdnMembers).val(ui.item.dwork.replaceAll(",",";"));
                    $(txtScheduleDate).val(ui.item.EDate);
                    lblTktStatus.innerText = ui.item.TicketStatus;
                    lblStatus.innerText = 'Assigned';
                    //Assign Ticket to Test          
                    if (ui.item.ID != oldTicket) {
                        AssignTicket($(hdnTestID).val(), ui.item.ID, lblTestYear.innerText, this.id, oldTicket);
                    }


                    return false;
                },
                focus: function (event, ui) {

                    $(this).val(ui.item.ID);
                    return false;
                },
                minLength: 0,
                delay: 250
            }).click(function () {
                $(this).autocomplete('search', $(this).val())
            });
            $.each($(".txtTicket"), function (index, item) {
                $(item).data("ui-autocomplete")._renderItem = function (ul, item) {

                    var result_item = item.ID;
                    var result_desc = item.TicketStatus;
                    var result_value = item.ID;
                    var x = new RegExp('\\b' + query, 'ig'); // notice the escape \ here...            
                    result_item = result_item.toString().replace(x, function (FullMatch, n) {
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
                            .append("<a>" + result_item + ": <span style='color:Gray;'>" + result_desc + "</span></a>")
                            .appendTo(ul);
                    }
                };
            });
            $("[id*=txtTicket]").blur(function () {
                var txtTicket = this.id;
                var hdnTicketID = document.getElementById(txtTicket.replace('txtTicket', 'hdnTicketID'));
                //var lblWorker = document.getElementById(txtTicket.replace('txtTicket', 'lblWorker'));
                //var lblTktStatus = document.getElementById(txtTicket.replace('txtTicket', 'lblTktStatus'));
                var hdnTestID = document.getElementById(txtTicket.replace('txtTicket', 'hdnTestID'));
                var lblTestYear = document.getElementById(txtTicket.replace('txtTicket', 'lblTestYear'));
                var oldTicket = "0";
                if ($(hdnTicketID).val() != "") {
                    oldTicket = $(hdnTicketID).val();
                }


                //Assign Ticket to Test          
                if ($(this).val() != "" && oldTicket != $(this).val()) {
                    AssignTicket($(hdnTestID).val(), $(this).val(), lblTestYear.innerText, this.id, oldTicket);
                }


            });

          
         
            
        
            function validateDate(dateStr) {
                const regExp = /^(\d\d?)\/(\d\d?)\/(\d{4})$/;
                let matches = dateStr.match(regExp);
                let isValid = matches;
                let maxDate = [0, 31, 29, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];

                if (matches) {
                    const month = parseInt(matches[1]);
                    const date = parseInt(matches[2]);
                    const year = parseInt(matches[3]);

                    isValid = month <= 12 && month > 0;
                    isValid &= date <= maxDate[month] && date > 0;

                    const leapYear = (year % 400 == 0)
                        || (year % 4 == 0 && year % 100 != 0);
                    isValid &= month != 2 || leapYear || date <= 28;
                }

                return isValid
            }



        }

    </script>
    <script>      

        function dtaaTicketDetail() {
            this.prefixText = null;
            this.loc = 0;
            this.ticketYear = 0;
        }

        function updateTicketInfo(ticketID, ticketYear, txtTicket) {
            var dtaaa = new dtaaTicketDetail();
            dtaaa.prefixText = ticketID;
            var hdnLoc = document.getElementById(txtTicket.replace('txtTicket', 'hdnLoc'));

            dtaaa.loc = hdnLoc.value;
            dtaaa.ticketYear = parseInt(ticketYear);
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "SafetyTestAuto.asmx/ServiceSearchTicketByID",
                data: JSON.stringify(dtaaa),
                dataType: "json",
                async: true,
                success: function (data) {
                    var obj = $.parseJSON(data.d);
                    //get currently Ticket                    
                    var hdnTicketID = document.getElementById(txtTicket.replace('txtTicket', 'hdnTicketID'));
                    var txtScheduleDate = document.getElementById(txtTicket.replace('txtTicket', 'txtScheduleDate'));
                    var lblTktStatus = document.getElementById(txtTicket.replace('txtTicket', 'lblTktStatus'));

                    var lblStatus = document.getElementById(txtTicket.replace('txtTicket', 'lblStatus'));
                    var hdnMembers = document.getElementById(txtTicket.replace('txtTicket', 'hdnMembers'));
                    $(hdnTicketID).val(ticketID);
                    
                    $(txtScheduleDate).val(obj[0].EDate).change();
                    lblTktStatus.innerText = obj[0].TicketStatus;
                    lblStatus.innerText = 'Assigned';
                      $(hdnMembers).val(obj[0].dwork);

                    var div = document.getElementById(txtTicket.replace("txtTicket", "cusLabelTag"));
                    div.innerHTML = '';
                    var disTeamMembers = obj[0].dwork;
                      $("#<%= hdnOrgMemberKey.ClientID%>").val(obj[0].dwork);
                    $("#<%= hdnOrgMemberDisp.ClientID%>").val(obj[0].dwork);

                    //Updte COA
                    //processCOA(txtTicket.replace('txtTicket', 'txtScheduleDate'));

                   // $(txtScheduleDate).onchange();
                  
                // Update selected for grid
                if (disTeamMembers != null && disTeamMembers != "") {
                    var teamArr = disTeamMembers.toString().split(';');                   
                    // trim value of teamArr
                    $.each(teamArr, function (index, value) {
                        teamArr[index] = value.trim();
                    });

                    if (teamArr != null && teamArr.length > 0)
                        for (var i = 0; i < teamArr.length; i++) {                           
                            tag = "<div class='chip chipUsers'><label  class='cusCheckContainer'>" + teamArr[i] + "<input type='checkbox' disabled><span class='checkmark'></span></label></div>";
                            div.innerHTML += tag;
                        }
                }
                    CheckUncheckAllCheckBoxAsNeeded();

                },
                error: function (result) {

                    noty({ text: 'We were unable to load ticket', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                }
            });

        }

        function dtaaAssignTicket() {
            this.LID = "0";
            this.TicketID = "0";
            this.TestYear = "0";
            this.OldTicket = "0"
        }

        function AssignTicket(lid, ticketID, ticketYear, txtTicket, OldTicket) {
            var obj = new dtaaAssignTicket();
            obj.LID = lid;
            obj.TicketID = ticketID.toString();
            obj.TestYear = ticketYear
            obj.OldTicket = OldTicket;
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "SafetyTestAuto.asmx/AssignTicketToTest",
                data: JSON.stringify(obj),
                dataType: "json",
                success: function (obj) {
                  

                    if (obj.d.indexOf("successful") == -1) {
                        var msg = 'We were unable to assign ticket ' + ticketID + ' to test.';
                        if (obj.d == 'Ticket does not exist') {
                            msg = 'Ticket ' + ticketID + ' does not exist in location.';
                        }
                        noty({
                            //text: 'We were unable to assign ticket ' + ticketID + ' to test.',
                            text: msg,
                            type: 'warning',
                            layout: 'topCenter',
                            closeOnSelfClick: false,
                            timeout: 5000,
                            theme: 'noty_theme_default',
                            closable: true
                        });

                    } else {
                        //get currently Ticket
                        updateTicketInfo(ticketID, ticketYear, txtTicket);

                    }
                },
                error: function (response) {
                    noty({
                        text: 'We were unable to assign ticket ' + ticketID + ' to test.',
                        type: 'warning',
                        layout: 'topCenter',
                        closeOnSelfClick: false,
                        timeout: 5000,
                        theme: 'noty_theme_default',
                        closable: true
                    });
                }
            });
        }

        function dtaaScheduleDate() {
            this.LID = "0";
            this.ScheduleDate = "0";
            this.ScheduleYear = "0";
            this.ScheduleStatus = "0";
            this.ScheduleWorker = "";
        }

        function UpdateTestSchedule(lid, scheduleYear, scheduleDate, ScheduleStatus, ScheduleWorker,txtScheduleDateID) {
            var obj = new dtaaScheduleDate();
            obj.LID = lid;
            obj.ScheduleDate = scheduleDate;
            obj.ScheduleYear = scheduleYear;
            obj.ScheduleStatus = ScheduleStatus.toString();
            obj.ScheduleWorker = ScheduleWorker.replaceAll(";",",");

          
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "SafetyTestAuto.asmx/UpdateTestSchedule",
                data: JSON.stringify(obj),
                dataType: "json",
                success: function (obj) {

                    if (obj.d.lastIndexOf("successful") == -1) {
                      
                        noty({
                            text: obj.d + '. We were unable to update the schedule for test ' + lid,
                            type: 'warning',
                            layout: 'topCenter',
                            closeOnSelfClick: false,
                            timeout: 5000,
                            theme: 'noty_theme_default',
                            closable: true
                        });

                    }
                   // processCOA(txtScheduleDateID);

                   
                },
                error: function (response) {
                    noty({
                        text: '. We were unable to update the schedule for test ' + lid,
                        type: 'warning',
                        layout: 'topCenter',
                        closeOnSelfClick: false,
                        timeout: 5000,
                        theme: 'noty_theme_default',
                        closable: true
                    });
                }
            });
        }

        function BulkUpdateTestSchedule(lid, scheduleYear, scheduleDate, ScheduleStatus, ScheduleWorker, txtScheduleDateID, showalertOncetime) {
            var obj = new dtaaScheduleDate();
            obj.LID = lid;
            obj.ScheduleDate = scheduleDate;
            obj.ScheduleYear = scheduleYear;
            obj.ScheduleStatus = ScheduleStatus.toString();
            obj.ScheduleWorker = ScheduleWorker.replaceAll(";",",");

          
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "SafetyTestAuto.asmx/UpdateTestSchedule",
                data: JSON.stringify(obj),
                dataType: "json",
                success: function (obj) {

                    if (obj.d.lastIndexOf("successful") == -1) {
                        
                        if (showalertOncetime) {
                            noty({
                                text: obj.d + '. We were unable to update the schedule for test ' + lid,
                                type: 'warning',
                                layout: 'topCenter',
                                closeOnSelfClick: false,
                                timeout: 5000,
                                theme: 'noty_theme_default',
                                closable: true
                            });
                        }

                    }
                   

                   
                },
                error: function (response) {
                    if (showalertOncetime) {
                        noty({
                            text: '. We were unable to update the schedule for test ' + lid,
                            type: 'warning',
                            layout: 'topCenter',
                            closeOnSelfClick: false,
                            timeout: 5000,
                            theme: 'noty_theme_default',
                            closable: true
                        });
                    }
                }
            });
        }
    </script>
    <script>
        function ShowConfirmAssignMessage() {
           
             var hdnNumberOfTestNoTicketInLoc = $("[id$='chkSelectSelectCheckBox']:checked").closest("tr").find("[id$='hdnNumberOfTestNoTicketInLoc']").val();
            if (hdnNumberOfTestNoTicketInLoc >= 1) {
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
       
    </script>
    <script>
      
        function UpdateCustomField(dtaaa, isBulkUpdate, showalertOncetime) {

            if ( (isNaN(dtaaa.TestID)==false) && (dtaaa.TestID != 0 ))
            {
                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: "CustomerAuto.asmx/SaveTestCustomerByYear",
                    data: JSON.stringify(dtaaa),
                    dataType: "json",
                    success: function (data) {

                        if (showalertOncetime == true) {
                            noty({
                                text: 'Test updated successfully!',
                                type: 'success',
                                layout: 'topCenter',
                                closeOnSelfClick: false,
                                timeout: 5000,
                                theme: 'noty_theme_default',
                                closable: true
                            });
                        }
                    },
                    error: function (response) {
                        var msg = "Test updated unsuccessfully!";
                        if (response.responseText.includes("Sending Email - Authentication Error")) {
                            msg = "Sending Email - Authentication Error";
                        }
                            
                        noty({
                            text: msg,
                            type: 'error',
                            layout: 'topCenter',
                            closeOnSelfClick: false,
                            timeout: 5000,
                            theme: 'noty_theme_default',
                            closable: true
                        });
                    }
                });
            }

        }

        function BulkUpdateCustomField(dtaaa) {
           
            if (dtaaa.TestID != 0) {

                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: "CustomerAuto.asmx/SaveTestCustomerByYear",
                    data: JSON.stringify(dtaaa),
                    dataType: "json",
                    success: function (data) {
                        return true;
                    },
                    error: function (response) {
                        return false;
                    }
                });
            }

        }

        function DueDateCalculations(schedule, xday) {
            var date = new Date(schedule);
            if (date != "Invalid Date") {
                date.setDate(date.getDate() + parseInt(xday));
                var day = date.getDate();
                var month = date.getMonth() + 1;
                var year = date.getFullYear();
                var dateString = month + "/" + day + "/" + year;
                return dateString;
            } else {
                return "";
            }
           
        }
    
        function processCOA(txtScheduleDateID) {
            if (txtScheduleDateID) {
                var hdnFireTestDateName = $("#<%= hdnFireTestDate.ClientID%>").val();
                var FireTestDateID = txtScheduleDateID.replace('txtScheduleDate', "TB_" + hdnFireTestDateName);
                if (document.getElementById(FireTestDateID)) {

                    var FireTestDate = $(document.getElementById(FireTestDateID)).val();
                    if (FireTestDate != "") {
                        processCOAByFireDate(FireTestDateID);
                    } else {
                        if (document.getElementById(txtScheduleDateID)) {
                            var scheduleDate = document.getElementById(txtScheduleDateID).value;
                            var hdnFormularFieldID = $("#<%= hdnFormularFieldID.ClientID%>").val();
                        var hdnFormularValue = $("#<%= hdnFormularValue.ClientID%>").val();
                            var lblTestYear = document.getElementById(txtScheduleDateID.replace('txtScheduleDate', 'lblTestYear'));
                            var lsFieldID = hdnFormularFieldID.split(",");
                            var lsValue = hdnFormularValue.split(",");
                            if (scheduleDate != "") {
                                var lsScheduleDate = scheduleDate.split(",");

                                for (var i = 0; i < lsFieldID.length; i++) {
                                    var item = document.getElementById(txtScheduleDateID.replace('txtScheduleDate', "TB_" + lsFieldID[i]));

                                    var TestID = document.getElementById(txtScheduleDateID.replace('txtScheduleDate', "hdnTestID"));
                                    var EquipmentID = document.getElementById(txtScheduleDateID.replace('txtScheduleDate', "hduid"));

                                    var oldValue = $(item).val();
                                    $(item).val(DueDateCalculations(lsScheduleDate[0], lsValue[i]));
                                    if (oldValue != $(item).val()) {
                                        var obj = new dtaa();
                                        obj.TestID = $(TestID).val()
                                        obj.EquipmentID = $(EquipmentID).val();
                                        obj.TestCustomFieldID = lsFieldID[i].split("_")[2];
                                        obj.Value = $(item).val();
                                        obj.OldValue = oldValue;
                                        obj.strUrl = getURL();
                                        obj.TestYear = lblTestYear.innerText;
                                        UpdateCustomField(obj , false , false);
                                    }

                                }

                            }
                        }
                    }
                }
            }

        }

      


    </script>
  <script type="text/javascript">
      $(document).ready(function () {
          document.getElementById('ctl00_ContentPlaceHolder1_lnkSearch').click();
      });
  </script>
</asp:Content>
 