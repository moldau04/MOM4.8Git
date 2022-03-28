<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Mom.master" Title="Add User Role || MOM" ValidateRequest="false" CodeBehind="AddUserRole.aspx.cs" Inherits="MOMWebApp.AddUserRole" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <!--Grid Control-->
    <%--<link href="Design/css/grid.css" rel="stylesheet" />--%>
    <link href="Design/css/grid.css" rel="stylesheet" />
    <!--File Upload Control-->
    <link href="Design/js/plugins/dropify/css/dropify.css" type="text/css" rel="stylesheet" media="screen,projection">
    <script src="js/jquery.sumoselect.min.js"></script>
    <script type="text/javascript" src="js/jquery.geocomplete.js"></script>
    <link href="Styles/sumoselect.css" rel="stylesheet" type="text/css" />
    <style>
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

        .display-block {
            display: block;
        }

        .display-none {
            display: none !important;
        }
        .valiateField {
            /*top: -60px !important;*/
            width: 190px !important;
        }

            .valiateField td, .valiateField div {
                border: solid 1px Black;
                background-color: LemonChiffon;
            }
    </style>
    <script>
        function HideME() {
            $("#DivEqup").hide();
        }

        function SelectRowsUser() {
            var Name = document.getElementById("<%=txtUnit.ClientID %>");
            var div = document.getElementById('eqtag');
            div.innerHTML = '';
            Name.value = '';

            var grid = $find("<%=RadGrid_Users.ClientID %>");
            var masterTable = grid.get_masterTableView();
            if (masterTable != null) {
                for (var i = 0; i < masterTable.get_dataItems().length; i++) {
                    var gridItemElement = masterTable.get_dataItems()[i].findElement("chkSelect");
                    var lblUnit = masterTable.get_dataItems()[i].findElement("lblUnit");
                    var lblID = masterTable.get_dataItems()[i].findElement("lblID");
                    var lblTypeid = masterTable.get_dataItems()[i].findElement("lblTypeid");
                    var ddlApplyUserRolePermission = masterTable.get_dataItems()[i].findElement("ddlApplyUserRolePermission");
                    if (gridItemElement.checked) {
                        if (Name.value != '') {
                            Name.value = Name.value + ', ' + lblUnit.innerHTML;
                        }
                        else {
                            Name.value = lblUnit.innerHTML;
                        }

                        var tag = "<div class='chip' style='width:auto !important;padding-left:5px !important;padding-right:5px !important ;margin-left:2px !important ;margin-right:2px !important ;margin-top:3px !important ;'><a href='adduser.aspx?uid=" + lblID.innerHTML + "&type=" + lblTypeid.innerHTML + "' target='_blank' style='color:white'>" + lblUnit.innerHTML + "</a></div>"

                        div.innerHTML += tag;
                        ddlApplyUserRolePermission.disabled = false;
                    } else {
                        ddlApplyUserRolePermission.disabled = true;
                    }
                }
            }
        }

        function EqCheckBOX(checked) {
            var grid = $find("<%=RadGrid_Users.ClientID %>");
            var masterTable = grid.get_masterTableView();
            if (masterTable != null) {
                for (var i = 0; i < masterTable.get_dataItems().length; i++) {
                    var gridItemElement = masterTable.get_dataItems()[i].findElement("chkSelect");
                    var lblUnit = masterTable.get_dataItems()[i].findElement("lblUnit");
                    gridItemElement.checked = checked;
                }
            }
        }

        function OnCheckChanged(checkCtr) {
            debugger
            var chkID = checkCtr.id;
            var chkValue = checkCtr.checked;
            if (chkID == "chkCustomerModule") {
                $("#<%=chkCustomeradd.ClientID%>").prop("checked", chkValue);
                $("#<%=chkCustomeredit.ClientID%>").prop("checked", chkValue);
                $("#<%=chkCustomerdelete.ClientID%>").prop("checked", chkValue);
                $("#<%=chkCustomerview.ClientID%>").prop("checked", chkValue);

                $("#<%=chkLocationadd.ClientID%>").prop("checked", chkValue);
                $("#<%=chkLocationedit.ClientID%>").prop("checked", chkValue);
                $("#<%=chkLocationdelete.ClientID%>").prop("checked", chkValue);
                $("#<%=chkLocationview.ClientID%>").prop("checked", chkValue);

                $("#<%=chkReceivePaymentAdd.ClientID%>").prop("checked", chkValue);
                $("#<%=chkReceivePaymentEdit.ClientID%>").prop("checked", chkValue);
                $("#<%=chkReceivePaymentDelete.ClientID%>").prop("checked", chkValue);
                $("#<%=chkReceivePaymentView.ClientID%>").prop("checked", chkValue);

                $("#<%=chkMakeDepositAdd.ClientID%>").prop("checked", chkValue);
                $("#<%=chkMakeDepositEdit.ClientID%>").prop("checked", chkValue);
                $("#<%=chkMakeDepositDelete.ClientID%>").prop("checked", chkValue);
                $("#<%=chkMakeDepositView.ClientID%>").prop("checked", chkValue);

                $("#<%=chkCollectionsAdd.ClientID%>").prop("checked", chkValue);
                $("#<%=chkCollectionsEdit.ClientID%>").prop("checked", chkValue);
                $("#<%=chkCollectionsDelete.ClientID%>").prop("checked", chkValue);
                $("#<%=chkCollectionsReport.ClientID%>").prop("checked", chkValue);
                $("#<%=chkCollectionsView.ClientID%>").prop("checked", chkValue);

                $("#<%=chkEquipmentsadd.ClientID%>").prop("checked", chkValue);
                $("#<%=chkEquipmentsedit.ClientID%>").prop("checked", chkValue);
                $("#<%=chkEquipmentsdelete.ClientID%>").prop("checked", chkValue);
                $("#<%=chkEquipmentsview.ClientID%>").prop("checked", chkValue);

                $("#<%=chkCreditHold.ClientID%>").prop("checked", chkValue);
                $("#<%=chkCreditFlag.ClientID%>").prop("checked", chkValue);
                $("#<%=chkWriteOff.ClientID%>").prop("checked", chkValue);
            } else if (chkID == "chkRecurring") {
                $("#<%=chkRecContractsAdd.ClientID%>").prop("checked", chkValue);
                $("#<%=chkRecContractsEdit.ClientID%>").prop("checked", chkValue);
                $("#<%=chkRecContractsDelete.ClientID%>").prop("checked", chkValue);
                $("#<%=chkRecContractsView.ClientID%>").prop("checked", chkValue);

                $("#<%=chkRecInvoicesAdd.ClientID%>").prop("checked", chkValue);
                $("#<%=chkRecInvoicesDelete.ClientID%>").prop("checked", chkValue);
                $("#<%=chkRecInvoicesView.ClientID%>").prop("checked", chkValue);

                $("#<%=chkRecTicketsAdd.ClientID%>").prop("checked", chkValue);
                $("#<%=chkRecTicketsDelete.ClientID%>").prop("checked", chkValue);
                $("#<%=chkRecTicketsView.ClientID%>").prop("checked", chkValue);

                $("#<%=chkSafetyTestsAdd.ClientID%>").prop("checked", chkValue);
                $("#<%=chkSafetyTestsEdit.ClientID%>").prop("checked", chkValue);
                $("#<%=chkSafetyTestsDelete.ClientID%>").prop("checked", chkValue);
                $("#<%=chkSafetyTestsView.ClientID%>").prop("checked", chkValue);

                $("#<%=chkRenewEscalateAdd.ClientID%>").prop("checked", chkValue);
                $("#<%=chkRenewEscalateView.ClientID%>").prop("checked", chkValue);

                //chkViolationsAdd
                $("#<%=chkViolationsAdd.ClientID%>").prop("checked", chkValue);
                $("#<%=chkViolationsDelete.ClientID%>").prop("checked", chkValue);
            } else if (chkID == "chkSchedule") {
                $("#<%=chkScheduleBoard.ClientID%>").prop("checked", chkValue);

                $("#<%=chkTimesheetadd.ClientID%>").prop("checked", chkValue);
                $("#<%=chkTimesheetedit.ClientID%>").prop("checked", chkValue);
                $("#<%=chkTimesheetdelete.ClientID%>").prop("checked", chkValue);
                $("#<%=chkTimesheetview.ClientID%>").prop("checked", chkValue);
                $("#<%=chkETimesheetreport.ClientID%>").prop("checked", chkValue);

                $("#<%=chkMapAdd.ClientID%>").prop("checked", chkValue);
                $("#<%=chkMapEdit.ClientID%>").prop("checked", chkValue);
                $("#<%=chkMapDelete.ClientID%>").prop("checked", chkValue);
                $("#<%=chkMapView.ClientID%>").prop("checked", chkValue);
                $("#<%=chkMapReport.ClientID%>").prop("checked", chkValue);

                $("#<%=chkRouteBuilderAdd.ClientID%>").prop("checked", chkValue);
                $("#<%=chkRouteBuilderEdit.ClientID%>").prop("checked", chkValue);
                $("#<%=chkRouteBuilderDelete.ClientID%>").prop("checked", chkValue);
                $("#<%=chkRouteBuilderView.ClientID%>").prop("checked", chkValue);
                $("#<%=chkRouteBuilderReport.ClientID%>").prop("checked", chkValue);

                $("#<%=chkMassReview.ClientID%>").prop("checked", chkValue);
                $("#<%=chkMassTimesheetCheck.ClientID%>").prop("checked", chkValue);

                $("#<%=chkETimesheetview.ClientID%>").prop("checked", chkValue);
                $("#<%=chkTimestampFix.ClientID%>").prop("checked", chkValue);

                $("#<%=chkResolveTicketView.ClientID%>").prop("checked", chkValue);
                $("#<%=chkResolveTicketAdd.ClientID%>").prop("checked", chkValue);
                $("#<%=chkResolveTicketEdit.ClientID%>").prop("checked", chkValue);
                $("#<%=chkResolveTicketDelete.ClientID%>").prop("checked", chkValue);
                $("#<%=chkResolveTicketReport.ClientID%>").prop("checked", chkValue);

                $("#<%=chkTicketListAdd.ClientID%>").prop("checked", chkValue);
                $("#<%=chkTicketListEdit.ClientID%>").prop("checked", chkValue);
                $("#<%=chkTicketListDelete.ClientID%>").prop("checked", chkValue);
                $("#<%=chkTicketListView.ClientID%>").prop("checked", chkValue);
                $("#<%=chkTicketListReport.ClientID%>").prop("checked", chkValue);

                $("#<%=chkTicketVoidPermission.ClientID%>").prop("checked", chkValue);

                $("#<%=chkMassPayrollTicket.ClientID%>").prop("checked", chkValue);
            }
            //chkProjectmodule
            else if (chkID == "chkProjectmodule") {
                $("#<%=chkProjectadd.ClientID%>").prop("checked", chkValue);
                $("#<%=chkProjectDelete.ClientID%>").prop("checked", chkValue);
                $("#<%=chkProjectEdit.ClientID%>").prop("checked", chkValue);
                $("#<%=chkProjectView.ClientID%>").prop("checked", chkValue);

                $("#<%=chkProjectTempAdd.ClientID%>").prop("checked", chkValue);
                $("#<%=chkProjectTempDelete.ClientID%>").prop("checked", chkValue);
                $("#<%=chkProjectTempEdit.ClientID%>").prop("checked", chkValue);
                $("#<%=chkProjectTempView.ClientID%>").prop("checked", chkValue);

                $("#<%=chkAddBOM.ClientID%>").prop("checked", chkValue);
                $("#<%=chkDeleteBOM.ClientID%>").prop("checked", chkValue);
                $("#<%=chkEditBOM.ClientID%>").prop("checked", chkValue);
                $("#<%=chkViewBOM.ClientID%>").prop("checked", chkValue);

                $("#<%=chkAddMilesStones.ClientID%>").prop("checked", chkValue);
                $("#<%=chkDeleteMilesStones.ClientID%>").prop("checked", chkValue);
                $("#<%=chkEditMilesStones.ClientID%>").prop("checked", chkValue);
                $("#<%=chkViewMilesStones.ClientID%>").prop("checked", chkValue);

                $("#<%=chkAddWIP.ClientID%>").prop("checked", chkValue);
                $("#<%=chkDeleteWIP.ClientID%>").prop("checked", chkValue);
                $("#<%=chkEditWIP.ClientID%>").prop("checked", chkValue);
                $("#<%=chkViewWIP.ClientID%>").prop("checked", chkValue);
                $("#<%=chkReportWIP.ClientID%>").prop("checked", chkValue);

                $("#<%=chkJobClosePermission.ClientID%>").prop("checked", chkValue);
                $("#<%=chkJobCompletedPermission.ClientID%>").prop("checked", chkValue);
                $("#<%=chkJobReopenPermission.ClientID%>").prop("checked", chkValue);
                $("#<%=chkViewProjectList.ClientID%>").prop("checked", chkValue);
                $("#<%=chkViewFinance.ClientID%>").prop("checked", chkValue);

                $("#<%=chkProjectManager.ClientID%>").prop("checked", chkValue);
                $("#<%=chkAssignedProject.ClientID%>").prop("checked", chkValue);
            }
            //chkInventorymodule
            else if (chkID == "chkInventorymodule") {
                $("#<%=chkInventoryItemadd.ClientID%>").prop("checked", chkValue);
                $("#<%=chkInventoryItemdelete.ClientID%>").prop("checked", chkValue);
                $("#<%=chkInventoryItemedit.ClientID%>").prop("checked", chkValue);
                $("#<%=chkInventoryItemview.ClientID%>").prop("checked", chkValue);

                $("#<%=chkInventoryAdjustmentadd.ClientID%>").prop("checked", chkValue);
                $("#<%=chkInventoryAdjustmentdelete.ClientID%>").prop("checked", chkValue);
                $("#<%=chkInventoryAdjustmentedit.ClientID%>").prop("checked", chkValue);
                $("#<%=chkInventoryAdjustmentview.ClientID%>").prop("checked", chkValue);

                $("#<%=chkInventoryWareHouseadd.ClientID%>").prop("checked", chkValue);
                $("#<%=chkInventoryWareHousedelete.ClientID%>").prop("checked", chkValue);
                $("#<%=chkInventoryWareHouseedit.ClientID%>").prop("checked", chkValue);
                $("#<%=chkInventoryWareHouseview.ClientID%>").prop("checked", chkValue);

                $("#<%=chkInventorysetupadd.ClientID%>").prop("checked", chkValue);
                $("#<%=chkInventorysetupdelete.ClientID%>").prop("checked", chkValue);
                $("#<%=chkInventorysetupedit.ClientID%>").prop("checked", chkValue);
                $("#<%=chkInventorysetupview.ClientID%>").prop("checked", chkValue);

                $("#<%=chkInventoryFinanceAdd.ClientID%>").prop("checked", chkValue);
                $("#<%=chkInventoryFinancedelete.ClientID%>").prop("checked", chkValue);
                $("#<%=chkInventoryFinanceedit.ClientID%>").prop("checked", chkValue);
                $("#<%=chkInventoryFinanceview.ClientID%>").prop("checked", chkValue);
            }
            //chkSalesMgr
            else if (chkID == "chkSalesMgr") {
                $("#<%=chkLeadAdd.ClientID%>").prop("checked", chkValue);
                $("#<%=chkLeadEdit.ClientID%>").prop("checked", chkValue);
                $("#<%=chkLeadDelete.ClientID%>").prop("checked", chkValue);
                $("#<%=chkLeadReport.ClientID%>").prop("checked", chkValue);
                $("#<%=chkLeadView.ClientID%>").prop("checked", chkValue);

                $("#<%=chkTasks.ClientID%>").prop("checked", chkValue);
                $("#<%=chkCompleteTask.ClientID%>").prop("checked", chkValue);
                $("#<%=chkFollowUp.ClientID%>").prop("checked", chkValue);

                $("#<%=chkOppAdd.ClientID%>").prop("checked", chkValue);
                $("#<%=chkOppEdit.ClientID%>").prop("checked", chkValue);
                $("#<%=chkOppDelete.ClientID%>").prop("checked", chkValue);
                $("#<%=chkOppView.ClientID%>").prop("checked", chkValue);
                $("#<%=chkOppReport.ClientID%>").prop("checked", chkValue);

                $("#<%=chkEstimateAdd.ClientID%>").prop("checked", chkValue);
                $("#<%=chkEstimateEdit.ClientID%>").prop("checked", chkValue);
                $("#<%=chkEstimateDelete.ClientID%>").prop("checked", chkValue);
                $("#<%=chkEstimateView.ClientID%>").prop("checked", chkValue);
                $("#<%=chkEstimateReport.ClientID%>").prop("checked", chkValue);

                $("#<%=chkConvertEstimate.ClientID%>").prop("checked", chkValue);
                $("#<%=chkSalesSetup.ClientID%>").prop("checked", chkValue);

                $("#<%=chkSalesperson.ClientID%>").prop("checked", chkValue);
                $("#<%=chkSalesAssigned.ClientID%>").prop("checked", chkValue);
                $("#<%=chkNotification.ClientID%>").prop("checked", chkValue);

                $("#<%=chkEstApprovalStatus.ClientID%>").prop("checked", chkValue);
            }
            //chkAccountPayable
            else if (chkID == "chkAccountPayable") {
                $("#<%=chkVendorsAdd.ClientID%>").prop("checked", chkValue);
                $("#<%=chkVendorsEdit.ClientID%>").prop("checked", chkValue);
                $("#<%=chkVendorsDelete.ClientID%>").prop("checked", chkValue);
                $("#<%=chkVendorsView.ClientID%>").prop("checked", chkValue);

                $("#<%=chkAddBills.ClientID%>").prop("checked", chkValue);
                $("#<%=chkEditBills.ClientID%>").prop("checked", chkValue);
                $("#<%=chkDeleteBills.ClientID%>").prop("checked", chkValue);
                $("#<%=chkViewBills.ClientID%>").prop("checked", chkValue);

                $("#<%=chkAddManageChecks.ClientID%>").prop("checked", chkValue);
                $("#<%=chkEditManageChecks.ClientID%>").prop("checked", chkValue);
                $("#<%=chkDeleteManageChecks.ClientID%>").prop("checked", chkValue);
                $("#<%=chkShowBankBalances.ClientID%>").prop("checked", chkValue);
                $("#<%=chkViewManageChecks.ClientID%>").prop("checked", chkValue);
            }
            //chkFinancialmodule
            else if (chkID == "chkFinancialmodule") {
                $("#<%=chkChartAdd.ClientID%>").prop("checked", chkValue);
                $("#<%=chkChartEdit.ClientID%>").prop("checked", chkValue);
                $("#<%=chkChartDelete.ClientID%>").prop("checked", chkValue);
                $("#<%=chkChartView.ClientID%>").prop("checked", chkValue);

                $("#<%=chkJournalEntryAdd.ClientID%>").prop("checked", chkValue);
                $("#<%=chkJournalEntryEdit.ClientID%>").prop("checked", chkValue);
                $("#<%=chkJournalEntryDelete.ClientID%>").prop("checked", chkValue);
                $("#<%=chkJournalEntryView.ClientID%>").prop("checked", chkValue);

                $("#<%=chkFinanceMgr.ClientID%>").prop("checked", chkValue);
                $("#<%=chkBankAdd.ClientID%>").prop("checked", chkValue);
                $("#<%=chkBankEdit.ClientID%>").prop("checked", chkValue);
                $("#<%=chkBankDelete.ClientID%>").prop("checked", chkValue);
                $("#<%=chkBankView.ClientID%>").prop("checked", chkValue);

                $("#<%=chkFinanceStatement.ClientID%>").prop("checked", chkValue);
            }
            //chkBillingmodule
            else if (chkID == "chkBillingmodule") {
                $("#<%=chkInvoicesAdd.ClientID%>").prop("checked", chkValue);
                $("#<%=chkInvoicesEdit.ClientID%>").prop("checked", chkValue);
                $("#<%=chkInvoicesDelete.ClientID%>").prop("checked", chkValue);
                $("#<%=chkInvoicesView.ClientID%>").prop("checked", chkValue);

                $("#<%=chkPaymentHistoryView.ClientID%>").prop("checked", chkValue);

                $("#<%=chkBillingcodesAdd.ClientID%>").prop("checked", chkValue);
                $("#<%=chkBillingcodesEdit.ClientID%>").prop("checked", chkValue);
                $("#<%=chkBillingcodesDelete.ClientID%>").prop("checked", chkValue);
                $("#<%=chkBillingcodesView.ClientID%>").prop("checked", chkValue);
            }
            //chkPurchasingmodule
            else if (chkID == "chkPurchasingmodule") {
                $("#<%=chkPOAdd.ClientID%>").prop("checked", chkValue);
                $("#<%=chkPOEdit.ClientID%>").prop("checked", chkValue);
                $("#<%=chkPODelete.ClientID%>").prop("checked", chkValue);
                $("#<%=chkPOView.ClientID%>").prop("checked", chkValue);

                $("#<%=chkRPOAdd.ClientID%>").prop("checked", chkValue);
                $("#<%=chkRPOEdit.ClientID%>").prop("checked", chkValue);
                $("#<%=chkRPODelete.ClientID%>").prop("checked", chkValue);
                $("#<%=chkRPOView.ClientID%>").prop("checked", chkValue);

                $("#<%=chkPONotification.ClientID%>").prop("checked", chkValue);
            }
            // chkProgram
            else if (chkID == "chkProgram") {
                $("#<%=chkEmpMainten.ClientID%>").prop("checked", chkValue);
                $("#<%=chkExpenses.ClientID%>").prop("checked", chkValue);
                $("#<%=chkAccessUser.ClientID%>").prop("checked", chkValue);
                $("#<%=chkDispatch.ClientID%>").prop("checked", chkValue);
            }
            //payrollModulchck
            else if (chkID == "payrollModulchck") {
                $("#<%=empAdd.ClientID%>").prop("checked", chkValue);
                $("#<%=empEdit.ClientID%>").prop("checked", chkValue);
                $("#<%=empDelete.ClientID%>").prop("checked", chkValue);
                $("#<%=empView.ClientID%>").prop("checked", chkValue);

                $("#<%=runAdd.ClientID%>").prop("checked", chkValue);
                $("#<%=runEdit.ClientID%>").prop("checked", chkValue);
                $("#<%=runDelete.ClientID%>").prop("checked", chkValue);
                $("#<%=runView.ClientID%>").prop("checked", chkValue);

                $("#<%=payrollchckAdd.ClientID%>").prop("checked", chkValue);
                $("#<%=payrollchckEdit.ClientID%>").prop("checked", chkValue);
                $("#<%=payrollchckDelete.ClientID%>").prop("checked", chkValue);
                $("#<%=payrollchckView.ClientID%>").prop("checked", chkValue);
                
                $("#<%=payrollformAdd.ClientID%>").prop("checked", chkValue);
                $("#<%=payrollformEdit.ClientID%>").prop("checked", chkValue);
                $("#<%=payrollformDelete.ClientID%>").prop("checked", chkValue);
                $("#<%=payrollformView.ClientID%>").prop("checked", chkValue);

                $("#<%=wagesadd.ClientID%>").prop("checked", chkValue);
                $("#<%=wagesEdit.ClientID%>").prop("checked", chkValue);
                $("#<%=wagesDelete.ClientID%>").prop("checked", chkValue);
                $("#<%=wagesView.ClientID%>").prop("checked", chkValue);

                $("#<%=deductionsAdd.ClientID%>").prop("checked", chkValue);
                $("#<%=deductionsEdit.ClientID%>").prop("checked", chkValue);
                $("#<%=deductionsDelete.ClientID%>").prop("checked", chkValue);
                $("#<%=deductionsView.ClientID%>").prop("checked", chkValue);

                $("#<%=chkMassPayrollTicket1.ClientID%>").prop("checked", chkValue);
            }
        }

        function UpdateCheckAPModule(checkCtr) {
            // AP Module 
            var chkID = checkCtr.id;
            var chkValue = checkCtr.checked;
            if (chkValue == false) {
                switch (chkID) {
                    case '<%= chkVendorsView.ClientID %>':
                        document.getElementById('<%= chkVendorsAdd.ClientID%>').checked
                            = document.getElementById('<%= chkVendorsEdit.ClientID%>').checked
                            = document.getElementById('<%= chkVendorsDelete.ClientID%>').checked
                            = document.getElementById('<%=chkVendorsView.ClientID %>').checked
                        break;
                    case '<%=chkViewBills.ClientID %>':
                        document.getElementById('<%= chkAddBills.ClientID%>').checked
                            = document.getElementById('<%= chkEditBills.ClientID%>').checked
                            = document.getElementById('<%= chkDeleteBills.ClientID%>').checked
                            = document.getElementById('<%=chkViewBills.ClientID %>').checked
                        break;
                    case '<%=chkViewManageChecks.ClientID %>':
                        document.getElementById('<%= chkAddManageChecks.ClientID%>').checked
                            = document.getElementById('<%= chkEditManageChecks.ClientID%>').checked
                            = document.getElementById('<%= chkDeleteManageChecks.ClientID%>').checked
                            = document.getElementById('<%=chkShowBankBalances.ClientID %>').checked
                            = document.getElementById('<%=chkViewManageChecks.ClientID %>').checked
                        break;
                }
            }
            else {
                if (document.getElementById('<%= chkVendorsAdd.ClientID%>').checked
                    || document.getElementById('<%= chkVendorsEdit.ClientID%>').checked
                    || document.getElementById('<%= chkVendorsDelete.ClientID%>').checked
                    || document.getElementById('<%=chkVendorsView.ClientID %>').checked
                ) {
                    document.getElementById('<%=chkVendorsView.ClientID %>').checked = true;
                    document.getElementById('<%=chkAccountPayable.ClientID %>').checked = true;
                }
                if (document.getElementById('<%= chkAddBills.ClientID%>').checked
                    || document.getElementById('<%= chkEditBills.ClientID%>').checked
                    || document.getElementById('<%= chkDeleteBills.ClientID%>').checked
                    || document.getElementById('<%=chkViewBills.ClientID %>').checked
                ) {
                    document.getElementById('<%=chkViewBills.ClientID %>').checked = true;
                    document.getElementById('<%=chkAccountPayable.ClientID %>').checked = true;
                }
                if (document.getElementById('<%= chkAddManageChecks.ClientID%>').checked
                    || document.getElementById('<%= chkEditManageChecks.ClientID%>').checked
                    || document.getElementById('<%= chkDeleteManageChecks.ClientID%>').checked
                    || document.getElementById('<%=chkViewManageChecks.ClientID %>').checked
                    || document.getElementById('<%=chkShowBankBalances.ClientID %>').checked
                ) {
                    document.getElementById('<%=chkViewManageChecks.ClientID %>').checked = true;
                    document.getElementById('<%=chkAccountPayable.ClientID %>').checked = true;
                }
            }
        }

        // Financial Module 
        function UpdateCheckFinancialModule(checkCtr) {
            var chkID = checkCtr.id;
            var chkValue = checkCtr.checked;
            if (chkValue == false) {
                switch (chkID) {
                    case '<%= chkChartView.ClientID %>':
                        document.getElementById('<%= chkChartAdd.ClientID%>').checked
                            = document.getElementById('<%= chkChartEdit.ClientID%>').checked
                            = document.getElementById('<%= chkChartDelete.ClientID%>').checked
                            = document.getElementById('<%=chkChartView.ClientID %>').checked
                        break;
                    case '<%=chkJournalEntryView.ClientID %>':
                        document.getElementById('<%= chkJournalEntryAdd.ClientID%>').checked
                            = document.getElementById('<%= chkJournalEntryEdit.ClientID%>').checked
                            = document.getElementById('<%= chkJournalEntryDelete.ClientID%>').checked
                            = document.getElementById('<%=chkJournalEntryView.ClientID %>').checked
                        break;
                    case '<%=chkBankView.ClientID %>':
                        document.getElementById('<%= chkBankAdd.ClientID%>').checked
                            = document.getElementById('<%= chkBankEdit.ClientID%>').checked
                            = document.getElementById('<%= chkBankDelete.ClientID%>').checked
                            = document.getElementById('<%=chkBankView.ClientID %>').checked
                        break;
                }
            }
            else {
                if (document.getElementById('<%= chkChartAdd.ClientID%>').checked
                    || document.getElementById('<%= chkChartEdit.ClientID%>').checked
                    || document.getElementById('<%= chkChartDelete.ClientID%>').checked
                    || document.getElementById('<%=chkChartView.ClientID %>').checked
                ) {
                    document.getElementById('<%=chkChartView.ClientID %>').checked = true;
                    document.getElementById('<%=chkFinancialmodule.ClientID %>').checked = true;
                }
                if (document.getElementById('<%= chkJournalEntryAdd.ClientID%>').checked
                    || document.getElementById('<%= chkJournalEntryEdit.ClientID%>').checked
                    || document.getElementById('<%= chkJournalEntryDelete.ClientID%>').checked
                    || document.getElementById('<%=chkJournalEntryView.ClientID %>').checked
                ) {
                    document.getElementById('<%=chkJournalEntryView.ClientID %>').checked = true;
                    document.getElementById('<%=chkFinancialmodule.ClientID %>').checked = true;
                }
                if (document.getElementById('<%= chkBankAdd.ClientID%>').checked
                    || document.getElementById('<%= chkBankEdit.ClientID%>').checked
                    || document.getElementById('<%= chkBankDelete.ClientID%>').checked
                    || document.getElementById('<%=chkBankView.ClientID %>').checked
                ) {
                    document.getElementById('<%=chkBankView.ClientID %>').checked = true;
                    document.getElementById('<%=chkFinancialmodule.ClientID %>').checked = true;
                }
            }
        }

        //checkCustomerModule
        function UpdateCheckBillingModule(checkCtr) {
            // Billing Module 
            var chkID = checkCtr.id;
            var chkValue = checkCtr.checked;
            if (chkValue == false) {
                switch (chkID) {
                    case '<%= chkInvoicesView.ClientID %>':
                        document.getElementById('<%= chkInvoicesAdd.ClientID%>').checked
                            = document.getElementById('<%= chkInvoicesEdit.ClientID%>').checked
                            = document.getElementById('<%= chkInvoicesDelete.ClientID%>').checked
                            = document.getElementById('<%=chkInvoicesView.ClientID %>').checked
                        break;
                    case '<%=chkBillingcodesView.ClientID %>':
                        document.getElementById('<%= chkBillingcodesAdd.ClientID%>').checked
                            = document.getElementById('<%= chkBillingcodesEdit.ClientID%>').checked
                            = document.getElementById('<%= chkBillingcodesDelete.ClientID%>').checked
                            = document.getElementById('<%=chkBillingcodesView.ClientID %>').checked
                        break;
                }
            }
            else {
                if (document.getElementById('<%= chkInvoicesAdd.ClientID%>').checked
                    || document.getElementById('<%= chkInvoicesEdit.ClientID%>').checked
                    || document.getElementById('<%= chkInvoicesDelete.ClientID%>').checked
                    || document.getElementById('<%=chkInvoicesView.ClientID %>').checked
                ) {
                    document.getElementById('<%=chkInvoicesView.ClientID %>').checked = true;
                    document.getElementById('<%=chkBillingmodule.ClientID %>').checked = true;
                }
                if (document.getElementById('<%= chkBillingcodesAdd.ClientID%>').checked
                    || document.getElementById('<%= chkBillingcodesEdit.ClientID%>').checked
                    || document.getElementById('<%= chkBillingcodesDelete.ClientID%>').checked
                    || document.getElementById('<%=chkBillingcodesView.ClientID %>').checked
                ) {
                    document.getElementById('<%=chkBillingcodesView.ClientID %>').checked = true;
                    document.getElementById('<%=chkBillingmodule.ClientID %>').checked = true;
                }
            }
        }

        function UpdateCheckPurchasingModule(checkCtr) {
            var chkID = checkCtr.id;
            var chkValue = checkCtr.checked;
            if (chkValue == false) {
                switch (chkID) {
                    case '<%= chkPOView.ClientID %>':
                        document.getElementById('<%= chkPOAdd.ClientID%>').checked
                            = document.getElementById('<%= chkPOEdit.ClientID%>').checked
                            = document.getElementById('<%= chkPODelete.ClientID%>').checked
                            = document.getElementById('<%=chkPOView.ClientID %>').checked
                        break;
                    case '<%=chkRPOView.ClientID %>':
                        document.getElementById('<%= chkRPOAdd.ClientID%>').checked
                            = document.getElementById('<%= chkRPOEdit.ClientID%>').checked
                            = document.getElementById('<%= chkRPODelete.ClientID%>').checked
                            = document.getElementById('<%=chkRPOView.ClientID %>').checked
                        break;
                }
            }
            else {
                // purchasing module 
                if (document.getElementById('<%= chkPOAdd.ClientID%>').checked
                    || document.getElementById('<%= chkPOEdit.ClientID%>').checked
                    || document.getElementById('<%= chkPODelete.ClientID%>').checked
                    || document.getElementById('<%=chkPOView.ClientID %>').checked
                ) {
                    document.getElementById('<%=chkPOView.ClientID %>').checked = true;
                    document.getElementById('<%=chkPurchasingmodule.ClientID %>').checked = true;
                }
                if (document.getElementById('<%= chkRPOAdd.ClientID%>').checked
                    || document.getElementById('<%= chkRPOEdit.ClientID%>').checked
                    || document.getElementById('<%= chkRPODelete.ClientID%>').checked
                    || document.getElementById('<%=chkRPOView.ClientID %>').checked
                ) {
                    document.getElementById('<%=chkRPOView.ClientID %>').checked = true;
                    document.getElementById('<%=chkPurchasingmodule.ClientID %>').checked = true;
                }
            }
        }

        function UpdateCheckRecurringModule(checkCtr) {
            // Recurring Module 
            var chkID = checkCtr.id;
            var chkValue = checkCtr.checked;
            if (chkValue == false) {
                switch (chkID) {
                    case '<%= chkRecContractsView.ClientID %>':
                        document.getElementById('<%= chkRecContractsAdd.ClientID%>').checked
                            = document.getElementById('<%= chkRecContractsEdit.ClientID%>').checked
                            = document.getElementById('<%= chkRecContractsDelete.ClientID%>').checked
                            = document.getElementById('<%=chkRecContractsView.ClientID %>').checked
                        break;
                    case '<%=chkRecInvoicesView.ClientID %>':
                        document.getElementById('<%= chkRecInvoicesAdd.ClientID%>').checked
                            = document.getElementById('<%= chkRecInvoicesDelete.ClientID%>').checked
                            = document.getElementById('<%=chkRecInvoicesView.ClientID %>').checked
                        break;
                    case '<%=chkRecTicketsView.ClientID %>':
                        document.getElementById('<%= chkRecTicketsAdd.ClientID%>').checked
                            = document.getElementById('<%= chkRecTicketsDelete.ClientID%>').checked
                            = document.getElementById('<%=chkRecTicketsView.ClientID %>').checked
                        break;
                    case '<%=chkSafetyTestsView.ClientID %>':
                        document.getElementById('<%= chkSafetyTestsAdd.ClientID%>').checked
                            = document.getElementById('<%= chkSafetyTestsEdit.ClientID%>').checked
                            = document.getElementById('<%= chkSafetyTestsDelete.ClientID%>').checked
                            = document.getElementById('<%=chkSafetyTestsView.ClientID %>').checked
                        break;
                    case '<%=chkRenewEscalateView.ClientID %>':
                        document.getElementById('<%= chkRenewEscalateAdd.ClientID%>').checked
                            = document.getElementById('<%=chkRenewEscalateView.ClientID %>').checked
                        break;
                }
            }
            else {
                if (document.getElementById('<%= chkRecContractsAdd.ClientID%>').checked
                    || document.getElementById('<%= chkRecContractsEdit.ClientID%>').checked
                    || document.getElementById('<%= chkRecContractsDelete.ClientID%>').checked
                    || document.getElementById('<%=chkRecContractsView.ClientID %>').checked
                ) {
                    document.getElementById('<%=chkRecContractsView.ClientID %>').checked = true;
                    document.getElementById('<%=chkRecurring.ClientID %>').checked = true;
                }
                if (document.getElementById('<%= chkRecInvoicesAdd.ClientID%>').checked
                    || document.getElementById('<%= chkRecInvoicesDelete.ClientID%>').checked
                    || document.getElementById('<%=chkRecInvoicesView.ClientID %>').checked
                ) {
                    document.getElementById('<%=chkRecInvoicesView.ClientID %>').checked = true;
                    document.getElementById('<%=chkRecurring.ClientID %>').checked = true;
                }
                if (document.getElementById('<%= chkRecTicketsAdd.ClientID%>').checked
                    || document.getElementById('<%= chkRecTicketsDelete.ClientID%>').checked
                    || document.getElementById('<%=chkRecTicketsView.ClientID %>').checked
                ) {
                    document.getElementById('<%=chkRecTicketsView.ClientID %>').checked = true;
                    document.getElementById('<%=chkRecurring.ClientID %>').checked = true;
                }
                if (document.getElementById('<%= chkSafetyTestsAdd.ClientID%>').checked
                    || document.getElementById('<%= chkSafetyTestsEdit.ClientID%>').checked
                    || document.getElementById('<%= chkSafetyTestsDelete.ClientID%>').checked
                    || document.getElementById('<%=chkSafetyTestsView.ClientID %>').checked
                ) {
                    document.getElementById('<%=chkSafetyTestsView.ClientID %>').checked = true;
                    document.getElementById('<%=chkRecurring.ClientID %>').checked = true;
                }
                if (document.getElementById('<%= chkRenewEscalateAdd.ClientID%>').checked
                    || document.getElementById('<%=chkRenewEscalateView.ClientID %>').checked
                ) {
                    document.getElementById('<%=chkRenewEscalateView.ClientID %>').checked = true;
                    document.getElementById('<%=chkRecurring.ClientID %>').checked = true;
                }
            }
        }

        function UpdateCheckScheduleModule(checkCtr) {
            debugger
            // Schedule Module 
            var chkID = checkCtr.id;
            var chkValue = checkCtr.checked;
            if (chkValue == false) {
                switch (chkID) {
                    case '<%= chkTimesheetview.ClientID %>':
                        document.getElementById('<%= chkTimesheetadd.ClientID%>').checked
                            = document.getElementById('<%= chkTimesheetedit.ClientID%>').checked
                            = document.getElementById('<%= chkTimesheetdelete.ClientID%>').checked
                            = document.getElementById('<%=chkTimesheetview.ClientID %>').checked
                        break;
                    case '<%=chkETimesheetview.ClientID %>':
                        document.getElementById('<%= chkETimesheetadd.ClientID%>').checked
                            = document.getElementById('<%= chkTimesheetedit.ClientID%>').checked
                            = document.getElementById('<%= chkETimesheetdelete.ClientID%>').checked
                            = document.getElementById('<%=chkETimesheetview.ClientID %>').checked
                        break;
                    case '<%=chkMapView.ClientID %>':
                        document.getElementById('<%= chkMapAdd.ClientID%>').checked
                            = document.getElementById('<%= chkMapEdit.ClientID%>').checked
                            = document.getElementById('<%= chkMapDelete.ClientID%>').checked
                            = document.getElementById('<%=chkMapView.ClientID %>').checked
                        break;
                    case '<%=chkRouteBuilderView.ClientID %>':
                        document.getElementById('<%= chkRouteBuilderAdd.ClientID%>').checked
                            = document.getElementById('<%= chkRouteBuilderEdit.ClientID%>').checked
                            = document.getElementById('<%= chkRouteBuilderDelete.ClientID%>').checked
                            = document.getElementById('<%=chkRouteBuilderView.ClientID %>').checked
                        break;
                    case '<%= chkResolveTicketView.ClientID %>':
                        $("#<%=chkResolveTicketAdd.ClientID%>").prop("checked", chkValue);
                        $("#<%=chkResolveTicketEdit.ClientID%>").prop("checked", chkValue);
                        $("#<%=chkResolveTicketDelete.ClientID%>").prop("checked", chkValue);
                        $("#<%=chkResolveTicketReport.ClientID%>").prop("checked", chkValue);
                        $("#<%=chkResolveTicketView.ClientID%>").prop("checked", chkValue);
                        break;
                    case '<%=chkTicketListView.ClientID %>':
                        $("#<%=chkTicketListAdd.ClientID%>").prop("checked", chkValue);
                        $("#<%=chkTicketListEdit.ClientID%>").prop("checked", chkValue);
                        $("#<%=chkTicketListDelete.ClientID%>").prop("checked", chkValue);
                        $("#<%=chkTicketListReport.ClientID%>").prop("checked", chkValue);
                        $("#<%=chkTicketListView.ClientID%>").prop("checked", chkValue);
                        break;
                }
            }
            else {
                if (document.getElementById('<%= chkTimesheetadd.ClientID%>').checked
                    || document.getElementById('<%= chkTimesheetedit.ClientID%>').checked
                    || document.getElementById('<%= chkTimesheetdelete.ClientID%>').checked
                    || document.getElementById('<%=chkTimesheetview.ClientID %>').checked
                ) {
                    document.getElementById('<%=chkTimesheetview.ClientID %>').checked = true;
                    document.getElementById('<%=chkSchedule.ClientID %>').checked = true;
                }
                if (document.getElementById('<%= chkETimesheetadd.ClientID%>').checked
                    || document.getElementById('<%= chkTimesheetedit.ClientID%>').checked
                    || document.getElementById('<%= chkETimesheetdelete.ClientID%>').checked
                    || document.getElementById('<%=chkETimesheetview.ClientID %>').checked
                ) {
                    document.getElementById('<%=chkETimesheetview.ClientID %>').checked = true;
                    document.getElementById('<%=chkSchedule.ClientID %>').checked = true;
                }
                if (document.getElementById('<%= chkMapAdd.ClientID%>').checked
                    || document.getElementById('<%= chkMapEdit.ClientID%>').checked
                    || document.getElementById('<%= chkMapDelete.ClientID%>').checked
                    || document.getElementById('<%=chkMapView.ClientID %>').checked
                ) {
                    document.getElementById('<%=chkMapView.ClientID %>').checked = true;
                    document.getElementById('<%=chkSchedule.ClientID %>').checked = true;
                }
                if (document.getElementById('<%= chkRouteBuilderAdd.ClientID%>').checked
                    || document.getElementById('<%= chkRouteBuilderEdit.ClientID%>').checked
                    || document.getElementById('<%= chkRouteBuilderDelete.ClientID%>').checked
                    || document.getElementById('<%=chkRouteBuilderView.ClientID %>').checked
                ) {
                    document.getElementById('<%=chkRouteBuilderView.ClientID %>').checked = true;
                    document.getElementById('<%=chkSchedule.ClientID %>').checked = true;
                }
                if (document.getElementById('<%= chkResolveTicketAdd.ClientID%>').checked
                    || document.getElementById('<%= chkResolveTicketEdit.ClientID%>').checked
                    || document.getElementById('<%= chkResolveTicketDelete.ClientID%>').checked
                    || document.getElementById('<%= chkResolveTicketView.ClientID %>').checked
                ) {
                    document.getElementById('<%=chkResolveTicketView.ClientID %>').checked = true;
                    document.getElementById('<%=chkSchedule.ClientID %>').checked = true;
                }
                if (document.getElementById('<%= chkTicketListAdd.ClientID%>').checked
                    || document.getElementById('<%= chkTicketListEdit.ClientID%>').checked
                    || document.getElementById('<%= chkTicketListDelete.ClientID%>').checked
                    || document.getElementById('<%=chkTicketListReport.ClientID %>').checked
                    || document.getElementById('<%=chkTicketListView.ClientID %>').checked
                ) {
                    document.getElementById('<%=chkTicketListView.ClientID %>').checked = true;
                    document.getElementById('<%=chkSchedule.ClientID %>').checked = true;
                }
            }
        }

        function UpdateCheckSalesModule(checkCtr) {
            // Sales Module 
            var chkID = checkCtr.id;
            var chkValue = checkCtr.checked;
            if (chkValue == false) {
                switch (chkID) {
                    case '<%= chkLeadView.ClientID %>':
                        document.getElementById('<%= chkLeadAdd.ClientID%>').checked
                            = document.getElementById('<%= chkLeadEdit.ClientID%>').checked
                            = document.getElementById('<%= chkLeadDelete.ClientID%>').checked
                            = document.getElementById('<%= chkLeadReport.ClientID%>').checked
                            = document.getElementById('<%=chkLeadView.ClientID %>').checked
                        break;
                    case '<%=chkOppView.ClientID %>':
                        document.getElementById('<%=chkOppAdd.ClientID%>').checked
                            = document.getElementById('<%=chkOppEdit.ClientID%>').checked
                            = document.getElementById('<%=chkOppDelete.ClientID%>').checked
                            = document.getElementById('<%=chkOppReport.ClientID%>').checked
                            = document.getElementById('<%=chkOppView.ClientID %>').checked
                        break;
                    case '<%=chkEstimateView.ClientID %>':
                        document.getElementById('<%=chkEstimateAdd.ClientID%>').checked
                            = document.getElementById('<%=chkEstimateEdit.ClientID%>').checked
                            = document.getElementById('<%=chkEstimateDelete.ClientID%>').checked
                            = document.getElementById('<%=chkEstimateReport.ClientID%>').checked
                            = document.getElementById('<%=chkEstimateView.ClientID %>').checked
                        break;
                }
            }
            else {
                if (document.getElementById('<%= chkLeadAdd.ClientID%>').checked
                    || document.getElementById('<%= chkLeadEdit.ClientID%>').checked
                    || document.getElementById('<%= chkLeadDelete.ClientID%>').checked
                    || document.getElementById('<%= chkLeadReport.ClientID%>').checked
                    || document.getElementById('<%=chkLeadView.ClientID %>').checked
                ) {
                    document.getElementById('<%=chkLeadView.ClientID %>').checked = true;
                    document.getElementById('<%=chkSalesMgr.ClientID %>').checked = true;
                }
                if (document.getElementById('<%=chkOppAdd.ClientID%>').checked
                    || document.getElementById('<%=chkOppEdit.ClientID%>').checked
                    || document.getElementById('<%=chkOppDelete.ClientID%>').checked
                    || document.getElementById('<%=chkOppReport.ClientID%>').checked
                    || document.getElementById('<%=chkOppView.ClientID %>').checked
                ) {
                    document.getElementById('<%=chkOppView.ClientID %>').checked = true;
                    document.getElementById('<%=chkSalesMgr.ClientID %>').checked = true;
                }
                if (document.getElementById('<%=chkEstimateAdd.ClientID%>').checked
                    || document.getElementById('<%=chkEstimateEdit.ClientID%>').checked
                    || document.getElementById('<%=chkEstimateDelete.ClientID%>').checked
                    || document.getElementById('<%=chkEstimateReport.ClientID%>').checked
                    || document.getElementById('<%=chkEstimateView.ClientID %>').checked
                ) {
                    document.getElementById('<%=chkEstimateView.ClientID %>').checked = true;
                    document.getElementById('<%=chkSalesMgr.ClientID %>').checked = true;
                }
            }
        }

        function UpdateCheckPayrollModule(checkCtr) {
            //payroll Module
            var chkID = checkCtr.id;
            var chkValue = checkCtr.checked;
            if (chkValue == false) {
                switch (chkID) {
                    case '<%= empView.ClientID %>':
                        document.getElementById('<%= empAdd.ClientID%>').checked
                            = document.getElementById('<%= empEdit.ClientID%>').checked
                            = document.getElementById('<%= empDelete.ClientID%>').checked
                            = document.getElementById('<%=empView.ClientID %>').checked
                        break;
                    case '<%=runView.ClientID %>':
                        document.getElementById('<%=runAdd.ClientID%>').checked
                            = document.getElementById('<%=runEdit.ClientID%>').checked
                            = document.getElementById('<%=runDelete.ClientID%>').checked
                            = document.getElementById('<%=runView.ClientID %>').checked
                        break;
                    case '<%=payrollchckView.ClientID %>':
                        document.getElementById('<%=payrollchckAdd.ClientID%>').checked
                            = document.getElementById('<%=payrollchckEdit.ClientID%>').checked
                            = document.getElementById('<%=payrollchckDelete.ClientID%>').checked
                            = document.getElementById('<%=payrollchckView.ClientID %>').checked
                        break;
                    case '<%=payrollformView.ClientID %>':
                        document.getElementById('<%=payrollformAdd.ClientID%>').checked
                            = document.getElementById('<%=payrollformEdit.ClientID%>').checked
                            = document.getElementById('<%=payrollformDelete.ClientID%>').checked
                            = document.getElementById('<%=payrollformView.ClientID %>').checked
                        break;
                    case '<%=wagesView.ClientID %>':
                        document.getElementById('<%=wagesadd.ClientID%>').checked
                            = document.getElementById('<%=wagesEdit.ClientID%>').checked
                            = document.getElementById('<%=wagesDelete.ClientID%>').checked
                        = document.getElementById('<%=wagesView.ClientID %>').checked
                        break;
                    case '<%=deductionsView.ClientID %>':
                        document.getElementById('<%=deductionsAdd.ClientID%>').checked
                            = document.getElementById('<%=deductionsEdit.ClientID%>').checked
                            = document.getElementById('<%=deductionsDelete.ClientID%>').checked
                            = document.getElementById('<%=deductionsView.ClientID %>').checked
                        break;
                }
            }
            else {
                if (document.getElementById('<%= empAdd.ClientID%>').checked
                    || document.getElementById('<%= empEdit.ClientID%>').checked
                    || document.getElementById('<%= empDelete.ClientID%>').checked
                    || document.getElementById('<%=empView.ClientID %>').checked
                ) {
                    document.getElementById('<%=empView.ClientID %>').checked = true;
                    document.getElementById('<%=payrollModulchck.ClientID %>').checked = true;
                }
                if (document.getElementById('<%=runAdd.ClientID%>').checked
                    || document.getElementById('<%=runEdit.ClientID%>').checked
                    || document.getElementById('<%=runDelete.ClientID%>').checked
                    || document.getElementById('<%=runView.ClientID %>').checked
                ) {
                    document.getElementById('<%=runView.ClientID %>').checked = true;
                    document.getElementById('<%=payrollModulchck.ClientID %>').checked = true;
                }
                if (document.getElementById('<%=payrollchckAdd.ClientID%>').checked
                    || document.getElementById('<%=payrollchckEdit.ClientID%>').checked
                    || document.getElementById('<%=payrollchckDelete.ClientID%>').checked
                    || document.getElementById('<%=payrollchckView.ClientID %>').checked
                ) {
                    document.getElementById('<%=payrollchckView.ClientID %>').checked = true;
                    document.getElementById('<%=payrollModulchck.ClientID %>').checked = true;
                }
                if (document.getElementById('<%=payrollformAdd.ClientID%>').checked
                    || document.getElementById('<%=payrollformEdit.ClientID%>').checked
                    || document.getElementById('<%=payrollformDelete.ClientID%>').checked
                    || document.getElementById('<%=payrollformView.ClientID %>').checked
                ) {
                    document.getElementById('<%=payrollformView.ClientID %>').checked = true;
                    document.getElementById('<%=payrollModulchck.ClientID %>').checked = true;
                }
                if (document.getElementById('<%=wagesadd.ClientID%>').checked
                    || document.getElementById('<%=wagesEdit.ClientID%>').checked
                    || document.getElementById('<%=wagesDelete.ClientID%>').checked
                    || document.getElementById('<%=wagesView.ClientID %>').checked
                ) {
                    document.getElementById('<%=wagesView.ClientID %>').checked = true;
                    document.getElementById('<%=payrollModulchck.ClientID %>').checked = true;
                }
                if (document.getElementById('<%=deductionsAdd.ClientID%>').checked
                    || document.getElementById('<%=deductionsEdit.ClientID%>').checked
                    || document.getElementById('<%=deductionsDelete.ClientID%>').checked
                    || document.getElementById('<%=deductionsView.ClientID %>').checked
                ) {
                    document.getElementById('<%=deductionsView.ClientID %>').checked = true;
                    document.getElementById('<%=payrollModulchck.ClientID %>').checked = true;
                }
            }
        }

        //checkCustomerModule
        function UpdateCheckCustomerModule(checkCtr) {
            debugger
            var chkID = checkCtr.id;
            var chkValue = checkCtr.checked;
            if (chkValue == false) {
                switch (chkID) {
                    case '<%= chkCustomerview.ClientID %>':
                        document.getElementById('<%= chkCustomeradd.ClientID%>').checked
                            = document.getElementById('<%= chkCustomeredit.ClientID%>').checked
                            = document.getElementById('<%= chkCustomerdelete.ClientID%>').checked
                            = false;
                        break;
                    case '<%=chkLocationview.ClientID %>':
                        document.getElementById('<%= chkLocationadd.ClientID%>').checked
                            = document.getElementById('<%= chkLocationedit.ClientID%>').checked
                            = document.getElementById('<%= chkLocationdelete.ClientID%>').checked
                            = document.getElementById('<%=chkLocationview.ClientID %>').checked
                        break;
                    case '<%=chkEquipmentsview.ClientID %>':
                        document.getElementById('<%= chkEquipmentsadd.ClientID%>').checked
                            = document.getElementById('<%= chkEquipmentsedit.ClientID%>').checked
                            = document.getElementById('<%= chkEquipmentsdelete.ClientID%>').checked
                            = document.getElementById('<%=chkEquipmentsview.ClientID %>').checked
                        break;
                    case '<%=chkReceivePaymentView.ClientID %>':
                        document.getElementById('<%= chkReceivePaymentAdd.ClientID%>').checked
                            = document.getElementById('<%= chkReceivePaymentEdit.ClientID%>').checked
                            = document.getElementById('<%= chkReceivePaymentDelete.ClientID%>').checked
                            = document.getElementById('<%=chkReceivePaymentView.ClientID %>').checked
                        break;
                    case '<%=chkMakeDepositView.ClientID %>':
                        document.getElementById('<%= chkMakeDepositAdd.ClientID%>').checked
                            = document.getElementById('<%= chkMakeDepositEdit.ClientID%>').checked
                            = document.getElementById('<%= chkMakeDepositDelete.ClientID%>').checked
                            = document.getElementById('<%=chkMakeDepositView.ClientID %>').checked
                        break;
                    case '<%=chkCollectionsView.ClientID %>':
                        document.getElementById('<%= chkCollectionsAdd.ClientID%>').checked
                            = document.getElementById('<%= chkCollectionsEdit.ClientID%>').checked
                            = document.getElementById('<%= chkCollectionsDelete.ClientID%>').checked
                            = document.getElementById('<%= chkCollectionsReport.ClientID%>').checked
                            = document.getElementById('<%=chkCollectionsView.ClientID %>').checked
                        break;
                }
            }
            else {
                // Customer Module
                if (document.getElementById('<%= chkCustomeradd.ClientID%>').checked
                    || document.getElementById('<%= chkCustomeredit.ClientID%>').checked
                    || document.getElementById('<%= chkCustomerdelete.ClientID%>').checked
                    || document.getElementById('<%= chkCustomerview.ClientID %>').checked
                ) {
                    document.getElementById('<%=chkCustomerview.ClientID %>').checked = true;
                    document.getElementById('<%=chkCustomerModule.ClientID %>').checked = true;
                }
                if (document.getElementById('<%= chkLocationadd.ClientID%>').checked
                    || document.getElementById('<%= chkLocationedit.ClientID%>').checked
                    || document.getElementById('<%= chkLocationdelete.ClientID%>').checked
                    || document.getElementById('<%=chkLocationview.ClientID %>').checked
                ) {
                    document.getElementById('<%=chkLocationview.ClientID %>').checked = true;
                    document.getElementById('<%=chkCustomerModule.ClientID %>').checked = true;
                }
                if (document.getElementById('<%= chkEquipmentsadd.ClientID%>').checked
                    || document.getElementById('<%= chkEquipmentsedit.ClientID%>').checked
                    || document.getElementById('<%= chkEquipmentsdelete.ClientID%>').checked
                    || document.getElementById('<%=chkEquipmentsview.ClientID %>').checked
                ) {
                    document.getElementById('<%=chkEquipmentsview.ClientID %>').checked = true;
                    document.getElementById('<%=chkCustomerModule.ClientID %>').checked = true;
                }
                if (document.getElementById('<%= chkReceivePaymentAdd.ClientID%>').checked
                    || document.getElementById('<%= chkReceivePaymentEdit.ClientID%>').checked
                    || document.getElementById('<%= chkReceivePaymentDelete.ClientID%>').checked
                    || document.getElementById('<%=chkReceivePaymentView.ClientID %>').checked
                ) {
                    document.getElementById('<%=chkReceivePaymentView.ClientID %>').checked = true;
                    document.getElementById('<%=chkCustomerModule.ClientID %>').checked = true;
                }
                if (document.getElementById('<%= chkMakeDepositAdd.ClientID%>').checked
                    || document.getElementById('<%= chkMakeDepositEdit.ClientID%>').checked
                    || document.getElementById('<%= chkMakeDepositDelete.ClientID%>').checked
                    || document.getElementById('<%=chkMakeDepositView.ClientID %>').checked
                ) {
                    document.getElementById('<%=chkMakeDepositView.ClientID %>').checked = true;
                    document.getElementById('<%=chkCustomerModule.ClientID %>').checked = true;
                }
                if (document.getElementById('<%= chkCollectionsAdd.ClientID%>').checked
                    || document.getElementById('<%= chkCollectionsEdit.ClientID%>').checked
                    || document.getElementById('<%= chkCollectionsDelete.ClientID%>').checked
                    || document.getElementById('<%= chkCollectionsReport.ClientID%>').checked
                    || document.getElementById('<%=chkCollectionsView.ClientID %>').checked
                ) {
                    document.getElementById('<%=chkCollectionsView.ClientID %>').checked = true;
                    document.getElementById('<%=chkCustomerModule.ClientID %>').checked = true;
                }
            }
        }
        
        //chkInventorymodule
        function UpdateCheckInventoryModule(checkCtr) {
            debugger
            var chkID = checkCtr.id;
            var chkValue = checkCtr.checked;
            if (chkValue == false) {
                switch (chkID) {
                    case '<%= chkInventoryItemview.ClientID %>':
                        $("#<%=chkInventoryItemadd.ClientID%>").prop("checked", chkValue);
                        $("#<%=chkInventoryItemdelete.ClientID%>").prop("checked", chkValue);
                        $("#<%=chkInventoryItemedit.ClientID%>").prop("checked", chkValue);
                        $("#<%=chkInventoryItemview.ClientID%>").prop("checked", chkValue);
                        break;
                    case '<%=chkInventoryAdjustmentview.ClientID %>':
                        $("#<%=chkInventoryAdjustmentadd.ClientID%>").prop("checked", chkValue);
                        $("#<%=chkInventoryAdjustmentdelete.ClientID%>").prop("checked", chkValue);
                        $("#<%=chkInventoryAdjustmentedit.ClientID%>").prop("checked", chkValue);
                        $("#<%=chkInventoryAdjustmentview.ClientID%>").prop("checked", chkValue);
                        break;
                    case '<%=chkInventoryWareHouseview.ClientID %>':
                        $("#<%=chkInventoryWareHouseadd.ClientID%>").prop("checked", chkValue);
                        $("#<%=chkInventoryWareHousedelete.ClientID%>").prop("checked", chkValue);
                        $("#<%=chkInventoryWareHouseedit.ClientID%>").prop("checked", chkValue);
                        $("#<%=chkInventoryWareHouseview.ClientID%>").prop("checked", chkValue);
                        break;
                    case '<%=chkInventorysetupview.ClientID %>':
                        $("#<%=chkInventorysetupadd.ClientID%>").prop("checked", chkValue);
                        $("#<%=chkInventorysetupdelete.ClientID%>").prop("checked", chkValue);
                        $("#<%=chkInventorysetupedit.ClientID%>").prop("checked", chkValue);
                        $("#<%=chkInventorysetupview.ClientID%>").prop("checked", chkValue);
                        break;
                    case '<%=chkInventoryFinanceview.ClientID %>':
                        $("#<%=chkInventoryFinanceAdd.ClientID%>").prop("checked", chkValue);
                        $("#<%=chkInventoryFinancedelete.ClientID%>").prop("checked", chkValue);
                        $("#<%=chkInventoryFinanceedit.ClientID%>").prop("checked", chkValue);
                        $("#<%=chkInventoryFinanceview.ClientID%>").prop("checked", chkValue);
                        break;
                }
            }
            else {
                // Customer Module
                if (document.getElementById('<%= chkInventoryItemadd.ClientID%>').checked
                    || document.getElementById('<%= chkInventoryItemdelete.ClientID%>').checked
                    || document.getElementById('<%= chkInventoryItemedit.ClientID%>').checked
                    || document.getElementById('<%= chkInventoryItemview.ClientID %>').checked
                ) {
                    document.getElementById('<%=chkInventoryItemview.ClientID %>').checked = true;
                    document.getElementById('<%=chkInventorymodule.ClientID %>').checked = true;
                }
                if (document.getElementById('<%= chkInventoryAdjustmentadd.ClientID%>').checked
                    || document.getElementById('<%= chkInventoryAdjustmentdelete.ClientID%>').checked
                    || document.getElementById('<%= chkInventoryAdjustmentedit.ClientID%>').checked
                    || document.getElementById('<%=chkInventoryAdjustmentview.ClientID %>').checked
                ) {
                    document.getElementById('<%=chkInventoryAdjustmentview.ClientID %>').checked = true;
                    document.getElementById('<%=chkInventorymodule.ClientID %>').checked = true;
                }
                if (document.getElementById('<%= chkInventoryWareHouseadd.ClientID%>').checked
                    || document.getElementById('<%= chkInventoryWareHousedelete.ClientID%>').checked
                    || document.getElementById('<%= chkInventoryWareHouseedit.ClientID%>').checked
                    || document.getElementById('<%=chkInventoryWareHouseview.ClientID %>').checked
                ) {
                    document.getElementById('<%=chkInventoryWareHouseview.ClientID %>').checked = true;
                    document.getElementById('<%=chkInventorymodule.ClientID %>').checked = true;
                }
                if (document.getElementById('<%= chkInventorysetupadd.ClientID%>').checked
                    || document.getElementById('<%= chkInventorysetupdelete.ClientID%>').checked
                    || document.getElementById('<%= chkInventorysetupedit.ClientID%>').checked
                    || document.getElementById('<%=chkInventorysetupview.ClientID %>').checked
                ) {
                    document.getElementById('<%=chkInventorysetupview.ClientID %>').checked = true;
                    document.getElementById('<%=chkInventorymodule.ClientID %>').checked = true;
                }
                if (document.getElementById('<%= chkInventoryFinanceAdd.ClientID%>').checked
                    || document.getElementById('<%= chkInventoryFinancedelete.ClientID%>').checked
                    || document.getElementById('<%= chkInventoryFinanceedit.ClientID%>').checked
                    || document.getElementById('<%=chkInventoryFinanceview.ClientID %>').checked
                ) {
                    document.getElementById('<%=chkInventoryFinanceview.ClientID %>').checked = true;
                    document.getElementById('<%=chkInventorymodule.ClientID %>').checked = true;
                }
            }
        }


        //Project Module
        function UpdateCheckProjectModule(checkCtr) {
            debugger
            var chkID = checkCtr.id;
            var chkValue = checkCtr.checked;
            if (chkValue == false) {
                switch (chkID) {
                    case '<%= chkProjectView.ClientID %>':
                        $("#<%=chkProjectadd.ClientID%>").prop("checked", chkValue);
                        $("#<%=chkProjectDelete.ClientID%>").prop("checked", chkValue);
                        $("#<%=chkProjectEdit.ClientID%>").prop("checked", chkValue);
                        $("#<%=chkProjectView.ClientID%>").prop("checked", chkValue);
                        $("#<%=chkJobClosePermission.ClientID%>").prop("checked", chkValue);
                        $("#<%=chkJobCompletedPermission.ClientID%>").prop("checked", chkValue);
                        $("#<%=chkJobReopenPermission.ClientID%>").prop("checked", chkValue);
                        break;
                    case '<%=chkProjectTempView.ClientID %>':
                        $("#<%=chkProjectTempAdd.ClientID%>").prop("checked", chkValue);
                        $("#<%=chkProjectTempDelete.ClientID%>").prop("checked", chkValue);
                        $("#<%=chkProjectTempEdit.ClientID%>").prop("checked", chkValue);
                        $("#<%=chkProjectTempView.ClientID%>").prop("checked", chkValue);
                        break;
                    case '<%=chkViewBOM.ClientID %>':
                        $("#<%=chkAddBOM.ClientID%>").prop("checked", chkValue);
                        $("#<%=chkDeleteBOM.ClientID%>").prop("checked", chkValue);
                        $("#<%=chkEditBOM.ClientID%>").prop("checked", chkValue);
                        $("#<%=chkViewBOM.ClientID%>").prop("checked", chkValue);
                        break;
                    case '<%=chkViewMilesStones.ClientID %>':
                        $("#<%=chkAddMilesStones.ClientID%>").prop("checked", chkValue);
                        $("#<%=chkDeleteMilesStones.ClientID%>").prop("checked", chkValue);
                        $("#<%=chkEditMilesStones.ClientID%>").prop("checked", chkValue);
                        $("#<%=chkViewMilesStones.ClientID%>").prop("checked", chkValue);
                        break;
                    case '<%=chkViewWIP.ClientID %>':
                        $("#<%=chkAddWIP.ClientID%>").prop("checked", chkValue);
                        $("#<%=chkDeleteWIP.ClientID%>").prop("checked", chkValue);
                        $("#<%=chkEditWIP.ClientID%>").prop("checked", chkValue);
                        $("#<%=chkViewWIP.ClientID%>").prop("checked", chkValue);
                        $("#<%=chkReportWIP.ClientID%>").prop("checked", chkValue);
                        break;
                    case '<%=chkProjectEdit.ClientID %>':
                        $("#<%=chkJobClosePermission.ClientID%>").prop("checked", chkValue);
                        $("#<%=chkJobCompletedPermission.ClientID%>").prop("checked", chkValue);
                        $("#<%=chkJobReopenPermission.ClientID%>").prop("checked", chkValue);
                        break;
                }
            }
            else {
                // Customer Module
                if (document.getElementById('<%= chkProjectadd.ClientID%>').checked
                    || document.getElementById('<%= chkProjectDelete.ClientID%>').checked
                    || document.getElementById('<%= chkProjectEdit.ClientID%>').checked
                    || document.getElementById('<%= chkProjectView.ClientID %>').checked
                ) {
                    document.getElementById('<%=chkProjectView.ClientID %>').checked = true;
                    document.getElementById('<%=chkProjectmodule.ClientID %>').checked = true;
                }
                if (document.getElementById('<%= chkJobClosePermission.ClientID%>').checked
                    || document.getElementById('<%= chkJobCompletedPermission.ClientID%>').checked
                    || document.getElementById('<%= chkJobReopenPermission.ClientID%>').checked
                ) {
                    document.getElementById('<%=chkProjectView.ClientID %>').checked = true;
                    document.getElementById('<%=chkProjectEdit.ClientID %>').checked = true;
                    document.getElementById('<%=chkProjectmodule.ClientID %>').checked = true;
                }
                if (document.getElementById('<%= chkProjectTempAdd.ClientID%>').checked
                    || document.getElementById('<%= chkProjectTempDelete.ClientID%>').checked
                    || document.getElementById('<%= chkProjectTempEdit.ClientID%>').checked
                    || document.getElementById('<%=chkProjectTempView.ClientID %>').checked
                ) {
                    document.getElementById('<%=chkProjectTempView.ClientID %>').checked = true;
                    document.getElementById('<%=chkProjectmodule.ClientID %>').checked = true;
                }
                if (document.getElementById('<%= chkAddBOM.ClientID%>').checked
                    || document.getElementById('<%= chkDeleteBOM.ClientID%>').checked
                    || document.getElementById('<%= chkEditBOM.ClientID%>').checked
                    || document.getElementById('<%=chkViewBOM.ClientID %>').checked
                ) {
                    document.getElementById('<%=chkViewBOM.ClientID %>').checked = true;
                    document.getElementById('<%=chkProjectmodule.ClientID %>').checked = true;
                }
                if (document.getElementById('<%= chkAddMilesStones.ClientID%>').checked
                    || document.getElementById('<%= chkDeleteMilesStones.ClientID%>').checked
                    || document.getElementById('<%= chkEditMilesStones.ClientID%>').checked
                    || document.getElementById('<%=chkViewMilesStones.ClientID %>').checked
                ) {
                    document.getElementById('<%=chkViewMilesStones.ClientID %>').checked = true;
                    document.getElementById('<%=chkProjectmodule.ClientID %>').checked = true;
                }
                if (document.getElementById('<%= chkAddWIP.ClientID%>').checked
                    || document.getElementById('<%= chkDeleteWIP.ClientID%>').checked
                    || document.getElementById('<%= chkEditWIP.ClientID%>').checked
                    || document.getElementById('<%=chkViewWIP.ClientID %>').checked
                    || document.getElementById('<%=chkReportWIP.ClientID %>').checked
                ) {
                    document.getElementById('<%=chkViewWIP.ClientID %>').checked = true;
                    document.getElementById('<%=chkProjectmodule.ClientID %>').checked = true;
                }
            }
        }
        
        function UpdateCheckDocsNContactModule(checkCtr) {
            var chkID = checkCtr.id;
            var chkValue = checkCtr.checked;
            if (chkValue == false) {
                switch (chkID) {
                    case '<%= chkDocumentView.ClientID %>':
                        $("#<%=chkDocumentAdd.ClientID%>").prop("checked", chkValue);
                        $("#<%=chkDocumentEdit.ClientID%>").prop("checked", chkValue);
                        $("#<%=chkDocumentDelete.ClientID%>").prop("checked", chkValue);
                        $("#<%=chkDocumentView.ClientID%>").prop("checked", chkValue);
                        break;
                    case '<%=chkContactView.ClientID %>':
                        $("#<%=chkContactAdd.ClientID%>").prop("checked", chkValue);
                        $("#<%=chkContactEdit.ClientID%>").prop("checked", chkValue);
                        $("#<%=chkContactDelete.ClientID%>").prop("checked", chkValue);
                        $("#<%=chkContactView.ClientID%>").prop("checked", chkValue);
                        break;
                }
            }
            else {
                // Customer Module
                if (document.getElementById('<%= chkDocumentAdd.ClientID%>').checked
                    || document.getElementById('<%= chkDocumentEdit.ClientID%>').checked
                    || document.getElementById('<%= chkDocumentDelete.ClientID%>').checked
                    || document.getElementById('<%= chkDocumentView.ClientID %>').checked
                ) {
                    document.getElementById('<%=chkDocumentView.ClientID %>').checked = true;
                }
                if (document.getElementById('<%= chkContactAdd.ClientID%>').checked
                    || document.getElementById('<%= chkContactEdit.ClientID%>').checked
                    || document.getElementById('<%= chkContactDelete.ClientID%>').checked
                    || document.getElementById('<%=chkContactView.ClientID %>').checked
                ) {
                    document.getElementById('<%=chkContactView.ClientID %>').checked = true;
                }
                
            }
        }

        function HideShowPOAmount(value) {
            //debugger
            if (value == "0") {
                $('#<%=divApprovePo.ClientID%>').hide();
                $('#<%=divMinAmount.ClientID%>').hide();
                $('#<%=divMaxAmount.ClientID%>').hide();
            }
            else {
                $('#<%=divApprovePo.ClientID%>').show();
                var apprPOAmount = $("#<%=ddlPOApproveAmt.ClientID%>").val();
                HideShowMinMaxAmount(apprPOAmount);
            }
        }
        function HideShowMinMaxAmount(value) {
            //debugger
            if (value == "0") {
                $('#<%=divMinAmount.ClientID%>').show();
                $('#<%=divMaxAmount.ClientID%>').show();
            }
            else if (value == "1") {
                $('#<%=divMinAmount.ClientID%>').show();
                $('#<%=divMaxAmount.ClientID%>').hide();
            } else {
                $('#<%=divMinAmount.ClientID%>').hide();
                $('#<%=divMaxAmount.ClientID%>').hide();
            }
        }

        function LoadLogs() {
            if (document.getElementById('<%= hdnLoadLogs.ClientID%>').value == "0") {
                document.getElementById('<%= lnkLoadLogs.ClientID%>').click();
            }
        }
        
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

            $("#eqtag").click(function () {
                $("#DivEqup").show();
            });

            Materialize.updateTextFields();
        });
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
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
        <img src="images/wheel.GIF" alt="Be patient..." style="position: fixed; margin-top: 25%; margin-left: 50%;" />
    </div>
    <telerik:RadAjaxManager ID="RadAjaxManager_UserRole" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="btnSubmit">
                <UpdatedControls>
                    <%--<telerik:AjaxUpdatedControl ControlID="txtRoleName" />
                    <telerik:AjaxUpdatedControl ControlID="txtRoleDescription" />--%>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_Users" />
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_gvLogs"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="lnkLoadLogs">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="hdnLoadLogs" />
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_gvLogs"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>

    <div style="height: 65px !important;">
        <div id="divButtons">
            <div id="breadcrumbs-wrapper">
                <header>
                    <div class="container row-color-grey">
                        <div class="row">
                            <div class="col s12 m12 l12">
                                <div class="row">
                                    <div class="page-title"><i class="mdi-social-person-outline"></i>&nbsp;<asp:Label ID="lblHeader" runat="server">Add User Role</asp:Label></div>
                                    <div class="btnlinks">
                                        <asp:LinkButton ID="btnSubmit" runat="server" OnClientClick="return showConfirm();" OnClick="btnSubmit_Click" ToolTip="Save" Text="Save"
                                            ></asp:LinkButton>
                                        <asp:HiddenField ID="hdnLocCount" runat="server" />
                                    </div>
                                    <div class="btnlinks">
                                        <a runat="server" id="lnkCancelContact" href="#" onclick="cancel();" class="close_button_Form"
                                            visible="false">Close</a>
                                    </div>
                                    <div class="rght-content">
                                        <div class="btnclosewrap">
                                            <%--<a href="#"><i class="mdi-content-clear"></i></a>--%>
                                            <asp:LinkButton ID="lnkClose" runat="server" ToolTip="Close" CausesValidation="false"
                                                OnClick="lnkClose_Click"><i class="mdi-content-clear"></i></asp:LinkButton>
                                        </div>
                                    </div>
                                    <div class="rght-content" style="margin-top: 5px; margin-right: 6px;">
                                        <asp:Label ID="lblUserName" runat="server"></asp:Label>
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
                                    <li class="accrdUserInfo"><a href="#accrdUserInfo">User Role Info</a></li>
                                    <li class="accrdPermissions"><a href="#accrdPermissions">Permissions</a></li>
                                    <li id="liLogs" runat="server" style="display: none"><a href="#accrdlogs">Logs</a></li>
                                </ul>
                            </div>
                            <div class="tblnksright">
                                <div class="nextprev">
                                    <asp:Panel ID="pnlSave" runat="server" Visible="false">
                                        <asp:Panel ID="pnlNext" runat="server">
                                            <span class="angleicons">
                                                <asp:LinkButton ID="lnkFirst" ToolTip="First" runat="server" OnClick="lnkFirst_Click" CausesValidation="False">
                                                        <i class="fa fa-angle-double-left"></i>
                                                </asp:LinkButton>
                                            </span>
                                            <span class="angleicons">
                                                <asp:LinkButton ID="lnkPrevious" CssClass="lnkPrevious" ToolTip="Previous" runat="server" CausesValidation="False" OnClick="lnkPrevious_Click">
                                                        <i class="fa fa-angle-left"></i>
                                                </asp:LinkButton>
                                            </span>
                                            <span class="angleicons">
                                                <asp:LinkButton ID="lnkNext" CssClass="lnkNext" ToolTip="Next" runat="server" CausesValidation="False" OnClick="lnkNext_Click">
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

    <div class="container accordian-wrap">
        <div class="row">
            <div class="col s12 m12 l12">
                <div class="row">
                    <ul class="collapsible popout collapsible-accordion form-accordion-head" data-collapsible="expandable">
                        <li>
                            <div id="accrdUserInfo" class="collapsible-header accrd active accordian-text-custom "><i class="mdi-social-poll"></i>User Role Info</div>
                            <div class="collapsible-body">
                                <div class="form-content-wrap">
                                    <div class="form-content-pd">
                                        <div class="form-section-row">
                                            <div class="col s12">
                                                <div class="form-section-row">
                                                    <div class="form-section2">
                                                        <div class="form-section-row">
                                                            <div class="form-section2">
                                                                <div class="input-field col s12">
                                                                    <div class="row">
                                                                        <asp:TextBox ID="txtRoleName" CssClass="validate" runat="server"></asp:TextBox>
                                                                        <label>Role Name</label>
                                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                                                                            ControlToValidate="txtRoleName" Display="None" ErrorMessage="Role Name Required"
                                                                            SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                        <asp:ValidatorCalloutExtender
                                                                            ID="RequiredFieldValidator1_ValidatorCalloutExtender" CssClass="valiateField" runat="server" Enabled="True" PopupPosition="BottomLeft"
                                                                            TargetControlID="RequiredFieldValidator1">
                                                                        </asp:ValidatorCalloutExtender>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="form-section2-blank">
                                                                &nbsp;
                                                            </div>
                                                            <div class="form-section2">
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
                                                        </div>
                                                    </div>
                                                    <div class="form-section2-blank">
                                                        &nbsp;
                                                    </div>
                                                    <div class="form-section2">
                                                        <div class="input-field col s12">
                                                            <div class="row">
                                                                <asp:TextBox ID="txtRoleDescription" Text="" CssClass="validate" runat="server"></asp:TextBox>
                                                                <label>Description</label>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="form-section-row">
                                                    <%--<div class="section-ttle">Users</div>--%>
                                                    <div class="input-field col s12" id="divUsers">
                                                        <div class="row">
                                                            <label class="drpdwn-label" style="transform: none;">Users</label>
                                                            <div class="tag-div materialize-textarea textarea-border" id="eqtag">
                                                            </div>
                                                            <div id="DivEqup" class="popup_div " style="width: 1000px; z-index: 5; left: 0;">
                                                                <div class="btnlinks" style="margin-bottom: 5px; float: right;">
                                                                    <a href="#" onclick="HideME();">Close</a>
                                                                </div>
                                                                <div class="grid_container">
                                                                    <div class="RadGrid RadGrid_Material FormGrid">
                                                                        <telerik:RadAjaxPanel ID="RadAjaxPanel_Users" runat="server">
                                                                            <telerik:RadGrid RenderMode="Auto" ID="RadGrid_Users" AllowFilteringByColumn="false" ShowFooter="True" PageSize="50"
                                                                                ShowStatusBar="true" runat="server" AllowPaging="false" AllowSorting="false" Width="100%" PagerStyle-AlwaysVisible="true" FilterType="CheckList"
                                                                                AllowCustomPaging="True"
                                                                                OnNeedDataSource="RadGrid_Users_NeedDataSource"
                                                                                OnPreRender="RadGrid_Users_PreRender"
                                                                                OnDataBound="RadGrid_Users_DataBound">
                                                                                <CommandItemStyle />
                                                                                <GroupingSettings CaseSensitive="false" />
                                                                                <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                                                                                    <Selecting AllowRowSelect="True"></Selecting>
                                                                                    <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                                                                    <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                                                                </ClientSettings>
                                                                                <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="false" ShowFooter="True">
                                                                                    <Columns>
                                                                                        <%--<telerik:GridClientSelectColumn UniqueName="chkSelect" HeaderStyle-Width="40">
                                                                                        </telerik:GridClientSelectColumn>
                                                                                        <telerik:GridTemplateColumn AllowFiltering="false" ShowFilterIcon="false" Visible="false" UniqueName="UserID">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblId" runat="server" Text='<%# Bind("UserID") %>'></asp:Label>
                                                                                                <asp:Label ID="lblTypeid" runat="server" Text='<%# Bind("usertypeid") %>'></asp:Label>
                                                                                            </ItemTemplate>
                                                                                        </telerik:GridTemplateColumn>--%>
                                                                                        <telerik:GridTemplateColumn DataField="UserID" AutoPostBackOnFilter="false" AllowFiltering="false" SortExpression="UserID"
                                                                                            CurrentFilterFunction="Contains" UniqueName="UserID" HeaderText="" ShowFilterIcon="false" HeaderStyle-Width="30">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblID" runat="server" Style="display: none;" Text='<%# Bind("UserID") %>'></asp:Label>
                                                                                                <asp:Label ID="lblTypeid" runat="server" Style="display: none;" Text='<%# Bind("usertypeid") %>'></asp:Label>
                                                                                                <asp:CheckBox ID="chkSelect" runat="server" />
                                                                                            </ItemTemplate>
                                                                                            <HeaderTemplate>
                                                                                                <asp:CheckBox ID="chkAll" runat="server" />
                                                                                            </HeaderTemplate>
                                                                                        </telerik:GridTemplateColumn>
                                                                                        <telerik:GridTemplateColumn UniqueName="fuser" SortExpression="fuser" AutoPostBackOnFilter="true" DataField="fuser"
                                                                                            CurrentFilterFunction="Contains" HeaderText="Username" ShowFilterIcon="false" HeaderStyle-Width="230">
                                                                                            <ItemTemplate>
                                                                                                <%--<asp:HyperLink ID="lnkName" runat="server" Text='<%# Bind("fuser") %>'></asp:HyperLink>--%>
                                                                                                <a href="adduser?uid=<%# Eval("UserID") %>&type=<%#Eval("usertypeid")%>" target="_blank" style="color: white">
                                                                                                    <asp:Label ID="lblUnit" runat="server" Text='<%# Bind("fuser") %>'></asp:Label></a>
                                                                                            </ItemTemplate>
                                                                                        </telerik:GridTemplateColumn>
                                                                                        <telerik:GridBoundColumn UniqueName="ffirst" DataField="ffirst" HeaderText="First Name" SortExpression="ffirst"
                                                                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                                                                        </telerik:GridBoundColumn>
                                                                                        <telerik:GridBoundColumn UniqueName="lLast" DataField="lLast" HeaderText="Last Name" SortExpression="lLast"
                                                                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                                                                        </telerik:GridBoundColumn>
                                                                                        <telerik:GridBoundColumn UniqueName="RoleName" DataField="RoleName" HeaderText="User Role" SortExpression="RoleName"
                                                                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                                                                        </telerik:GridBoundColumn>
                                                                                        <telerik:GridBoundColumn UniqueName="usertype" DataField="usertype" HeaderText="Type" SortExpression="usertype"
                                                                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false" HeaderStyle-Width="80">
                                                                                        </telerik:GridBoundColumn>
                                                                                        <telerik:GridTemplateColumn UniqueName="status" SortExpression="status" AutoPostBackOnFilter="true" DataField="status"
                                                                                            CurrentFilterFunction="Contains" HeaderText="Status" ShowFilterIcon="false" HeaderStyle-Width="80">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblStatus" runat="server"><%# Convert.ToInt32( Eval("status")) == 0 ? "Active" : "Inactive"%></asp:Label>
                                                                                            </ItemTemplate>
                                                                                        </telerik:GridTemplateColumn>
                                                                                        <telerik:GridTemplateColumn UniqueName="ApplyUserRolePermission" SortExpression="ApplyUserRolePermission" AutoPostBackOnFilter="true" DataField="ApplyUserRolePermission"
                                                                                            CurrentFilterFunction="Contains" HeaderText="Applying User Role Permission" ShowFilterIcon="false" HeaderStyle-Width="140">
                                                                                            <ItemTemplate>
                                                                                                <%--<asp:Label ID="lblApplyUserRolePermission" runat="server"><%# Eval("ApplyUserRolePermission")%></asp:Label>--%>
                                                                                                <asp:DropDownList ID="ddlApplyUserRolePermission" runat="server"
                                                                                                    Style="font-size: 0.9rem !important;"
                                                                                                    SelectedValue='<%# Eval("ApplyUserRolePermission") == DBNull.Value ? "0" : Eval("ApplyUserRolePermission") %>'
                                                                                                    CssClass="form-control input-sm input-small browser-default preventdownrow">
                                                                                                    <asp:ListItem Value="0">None</asp:ListItem>
                                                                                                    <asp:ListItem Value="1">Merge</asp:ListItem>
                                                                                                    <asp:ListItem Value="2">Override</asp:ListItem>
                                                                                                </asp:DropDownList>
                                                                                            </ItemTemplate>
                                                                                        </telerik:GridTemplateColumn>
                                                                                        <%--<telerik:GridBoundColumn DataField="super" HeaderText="Supervisor" SortExpression="super" UniqueName="super"
                                                                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                                                                        </telerik:GridBoundColumn>--%>

                                                                                    </Columns>
                                                                                </MasterTableView>
                                                                                <FilterMenu CssClass="RadFilterMenu_CheckList">
                                                                                </FilterMenu>
                                                                            </telerik:RadGrid>
                                                                        </telerik:RadAjaxPanel>
                                                                    </div>
                                                                </div>
                                                            </div>

                                                            <asp:TextBox ID="txtUnit" runat="server" Style="display: none;"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="cf"></div>
                                            </div>
                                            <div class="cf"></div>
                                        </div>
                                        <div class="cf"></div>
                                    </div>
                                </div>
                            </div>
                        </li>
                        <li>
                            <div id="accrdPermissions" class="collapsible-header accrd accordian-text-custom"><i class="mdi-communication-vpn-key"></i>Permissions</div>
                            <div class="collapsible-body">
                                <div class="form-content-wrap form-content-wrapwd">
                                    <div class="form-content-pd">
                                        <div class="form-section-row">
                                            <div class="section-ttle">
                                                User Role Permissions
                                            </div>

                                            <div class="grid_container">
                                                <div class="col-lg-7 col-md-7 col-sm-7">
                                                    <div class="permisson">
                                                        <asp:Panel ID="pnlMomCred" runat="server" Visible="False">
                                                            <fieldset class="roundCorner">
                                                                <h3><b>MOM Credentials</b> </h3>
                                                                <table style="width: 100%;">
                                                                    <tr>
                                                                        <td class="register_lbl">Username<asp:RequiredFieldValidator ID="RequiredFieldValidator17" runat="server"
                                                                            ControlToValidate="txtMOMUserName" Display="None" ErrorMessage="Username Required"
                                                                            SetFocusOnError="True"></asp:RequiredFieldValidator><asp:ValidatorCalloutExtender
                                                                                ID="RequiredFieldValidator17_ValidatorCalloutExtender" runat="server" Enabled="True" TargetControlID="RequiredFieldValidator17">
                                                                            </asp:ValidatorCalloutExtender>
                                                                            &nbsp;
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtMOMUserName" runat="server" CssClass="form-control" MaxLength="50"
                                                                                TabIndex="28"></asp:TextBox>
                                                                            <asp:FilteredTextBoxExtender ID="txtMOMUserName_FilteredTextBoxExtender" runat="server"
                                                                                Enabled="True" FilterMode="InvalidChars" InvalidChars="~*()+|}{&quot;:?&gt;&lt;,/;'[]\ `"
                                                                                TargetControlID="txtMOMUserName">
                                                                            </asp:FilteredTextBoxExtender>
                                                                            &nbsp;
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="register_lbl">Password<asp:RequiredFieldValidator ID="RequiredFieldValidator18" runat="server"
                                                                            ControlToValidate="txtMOMPassword" Display="None" ErrorMessage="Password Required"
                                                                            SetFocusOnError="True"></asp:RequiredFieldValidator><asp:ValidatorCalloutExtender
                                                                                ID="RequiredFieldValidator18_ValidatorCalloutExtender" runat="server" Enabled="True" TargetControlID="RequiredFieldValidator18">
                                                                            </asp:ValidatorCalloutExtender>
                                                                            &nbsp;
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtMOMPassword" runat="server" CssClass="form-control" MaxLength="10"
                                                                                TabIndex="29"></asp:TextBox>
                                                                            <asp:FilteredTextBoxExtender ID="txtMOMPassword_FilteredTextBoxExtender" runat="server"
                                                                                Enabled="True" FilterMode="InvalidChars" InvalidChars="~*()+|}{&quot;:?&gt;&lt;,/;'[]\ `"
                                                                                TargetControlID="txtMOMPassword">
                                                                            </asp:FilteredTextBoxExtender>
                                                                            &nbsp;
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </fieldset>
                                                        </asp:Panel>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="cf"></div>
                                        </div>

                                        <asp:Panel ID="tblPermission" runat="server">
                                            <telerik:RadAjaxPanel ID="rapTablePermission" runat="server">
                                                <div class="form-content-wrap">
                                                    <div class="form-content-pd">

                                                        <div class="form-section3">
                                                            <div class="section-ttle">
                                                                <asp:CheckBox ID="chkCustomerModule" ClientIDMode="Static" OnClick="OnCheckChanged(this);" CssClass="css-checkbox" AutoPostBack="false" runat="server" Font-Bold="true" Text="Customer module" />
                                                            </div>
                                                            <div class="input-field col s12">
                                                                <div class="row">
                                                                    <table>

                                                                        <tr runat="server">
                                                                            <td runat="server">Customer </td>
                                                                            <td runat="server">
                                                                                <asp:CheckBox ID="chkCustomeradd" OnClick='UpdateCheckCustomerModule(this)' CssClass="css-checkbox" runat="server" Text="Add" />&nbsp;
                                                                                <asp:CheckBox ID="chkCustomeredit" OnClick='UpdateCheckCustomerModule(this)' CssClass="css-checkbox" runat="server" Text="Edit" />&nbsp;
                                                                                <asp:CheckBox ID="chkCustomerdelete" OnClick='UpdateCheckCustomerModule(this)' CssClass="css-checkbox" runat="server" Text="Delete" />&nbsp;
                                                                                <asp:CheckBox ID="chkCustomerview" OnClick='UpdateCheckCustomerModule(this)' CssClass="css-checkbox" runat="server" Text="View" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr runat="server">
                                                                            <td runat="server">Location
                                                                            </td>
                                                                            <td runat="server">
                                                                                <asp:CheckBox ID="chkLocationadd" OnClick='UpdateCheckCustomerModule(this)' CssClass="css-checkbox" runat="server" Text="Add" />&nbsp;
                                                                                <asp:CheckBox ID="chkLocationedit" OnClick='UpdateCheckCustomerModule(this)' CssClass="css-checkbox" runat="server" Text="Edit" />&nbsp;
                                                                                <asp:CheckBox ID="chkLocationdelete" OnClick='UpdateCheckCustomerModule(this)' CssClass="css-checkbox" runat="server" Text="Delete" />&nbsp;
                                                                                <asp:CheckBox ID="chkLocationview" OnClick='UpdateCheckCustomerModule(this)' CssClass="css-checkbox" runat="server" Text="View" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr runat="server">
                                                                            <td runat="server">Equipment
                                                                            </td>
                                                                            <td runat="server">
                                                                                <asp:CheckBox ID="chkEquipmentsadd" OnClick='UpdateCheckCustomerModule(this)' CssClass="css-checkbox" runat="server" Text="Add" />&nbsp;
                                                                                <asp:CheckBox ID="chkEquipmentsedit" OnClick='UpdateCheckCustomerModule(this)' CssClass="css-checkbox" runat="server" Text="Edit" />&nbsp;
                                                                                <asp:CheckBox ID="chkEquipmentsdelete" OnClick='UpdateCheckCustomerModule(this)' CssClass="css-checkbox" runat="server" Text="Delete" />&nbsp;
                                                                                <asp:CheckBox ID="chkEquipmentsview" OnClick='UpdateCheckCustomerModule(this)' CssClass="css-checkbox" runat="server" Text="View" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr runat="server">
                                                                            <td runat="server">Receive Payment</td>
                                                                            <td runat="server">
                                                                                <asp:CheckBox ID="chkReceivePaymentAdd" OnClick='UpdateCheckCustomerModule(this)' CssClass="css-checkbox" runat="server" Text="Add" />&nbsp;
                                                                                <asp:CheckBox ID="chkReceivePaymentEdit" OnClick='UpdateCheckCustomerModule(this)' CssClass="css-checkbox" runat="server" Text="Edit" />&nbsp;
                                                                                <asp:CheckBox ID="chkReceivePaymentDelete" OnClick='UpdateCheckCustomerModule(this)' CssClass="css-checkbox" runat="server" Text="Delete" />&nbsp;
                                                                                <asp:CheckBox ID="chkReceivePaymentView" OnClick='UpdateCheckCustomerModule(this)' CssClass="css-checkbox" runat="server" Text="View" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr runat="server">
                                                                            <td runat="server">Make Deposit
                                                                            </td>
                                                                            <td runat="server">
                                                                                <asp:CheckBox ID="chkMakeDepositAdd" OnClick='UpdateCheckCustomerModule(this)' CssClass="css-checkbox" runat="server" Text="Add" />&nbsp;
                                                                                <asp:CheckBox ID="chkMakeDepositEdit" OnClick='UpdateCheckCustomerModule(this)' CssClass="css-checkbox" runat="server" Text="Edit" />&nbsp;
                                                                                <asp:CheckBox ID="chkMakeDepositDelete" OnClick='UpdateCheckCustomerModule(this)' CssClass="css-checkbox" runat="server" Text="Delete" />&nbsp;
                                                                                <asp:CheckBox ID="chkMakeDepositView" OnClick='UpdateCheckCustomerModule(this)' CssClass="css-checkbox" runat="server" Text="View" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr runat="server">
                                                                            <td runat="server">
                                                                                Collections
                                                                            </td>
                                                                            <td runat="server">
                                                                                <asp:CheckBox ID="chkCollectionsAdd"  Style="display: none;" CssClass="css-checkbox" runat="server" Text="Add" />
                                                                                <asp:CheckBox ID="chkCollectionsEdit" OnClick='UpdateCheckCustomerModule(this)' CssClass="css-checkbox" runat="server" Text="Edit" />&nbsp;
                                                                                <asp:CheckBox ID="chkCollectionsDelete" Style="display: none;" CssClass="css-checkbox" runat="server" Text="Delete" />&nbsp;                                      
                                                                                <asp:CheckBox ID="chkCollectionsReport" OnClick='UpdateCheckCustomerModule(this)'  CssClass="css-checkbox" runat="server" Text="Report" />&nbsp;                                      
                                                                                <asp:CheckBox ID="chkCollectionsView" OnClick='UpdateCheckCustomerModule(this)' CssClass="css-checkbox" runat="server" Text="View" />
                                                                            </td>
                                                                            
                                                                        </tr>
                                                                        <tr runat="server">
                                                                            <td runat="server">
                                                                                <asp:CheckBox ID="chkWriteOff" CssClass="css-checkbox" AutoPostBack="false" runat="server" Text="Write off" />
                                                                            </td>
                                                                            <td runat="server">
                                                                                <asp:CheckBox ID="chkCreditHold" CssClass="css-checkbox" AutoPostBack="false" runat="server" Text="Credit Hold" />&nbsp;
                                                                                <asp:CheckBox ID="chkCreditFlag" CssClass="css-checkbox" AutoPostBack="false" runat="server" Text="Credit Flag" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="form-section3-blank">
                                                            &nbsp;
                                                        </div>
                                                        <div class="form-section3">
                                                            <div class="section-ttle">
                                                                <asp:CheckBox ID="chkRecurring" ClientIDMode="Static" OnClick="OnCheckChanged(this);" CssClass="css-checkbox" AutoPostBack="false"  runat="server" Font-Bold="true" Text="Recurring  module" />
                                                            </div>
                                                            <div class="input-field col s12">
                                                                <div class="row">
                                                                    <table>
                                                                        <tr runat="server">
                                                                            <td runat="server" style="width:140px;">Recurring Contracts</td>
                                                                            <td runat="server">
                                                                                <asp:CheckBox ID="chkRecContractsAdd" OnClick='UpdateCheckRecurringModule(this)' CssClass="css-checkbox" runat="server" Text="Add" />&nbsp;
                                                                                <asp:CheckBox ID="chkRecContractsEdit" OnClick='UpdateCheckRecurringModule(this)' CssClass="css-checkbox " runat="server" Text="Edit" />&nbsp;
                                                                                <asp:CheckBox ID="chkRecContractsDelete" OnClick='UpdateCheckRecurringModule(this)' CssClass="css-checkbox " runat="server" Text="Delete" />&nbsp;
                                                                                <asp:CheckBox ID="chkRecContractsView" OnClick='UpdateCheckRecurringModule(this)' CssClass="css-checkbox" runat="server" Text="View" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr runat="server">
                                                                            <td runat="server">Safety Tests</td>
                                                                            <td runat="server">
                                                                                <asp:CheckBox ID="chkSafetyTestsAdd" OnClick='UpdateCheckRecurringModule(this)' CssClass="css-checkbox" runat="server" Text="Add" />&nbsp;
                                                                                <asp:CheckBox ID="chkSafetyTestsEdit" OnClick='UpdateCheckRecurringModule(this)' CssClass="css-checkbox" runat="server" Text="Edit" />&nbsp;
                                                                                <asp:CheckBox ID="chkSafetyTestsDelete" OnClick='UpdateCheckRecurringModule(this)' CssClass="css-checkbox" runat="server" Text="Delete" />&nbsp;
                                                                                <asp:CheckBox ID="chkSafetyTestsView" OnClick='UpdateCheckRecurringModule(this)' CssClass="css-checkbox" runat="server" Text="View" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr runat="server">
                                                                            <td runat="server">Recurring Invoices</td>
                                                                            <td runat="server">
                                                                                <asp:CheckBox ID="chkRecInvoicesAdd" OnClick='UpdateCheckRecurringModule(this)' CssClass="css-checkbox" runat="server" Text="Add" />&nbsp;
                                                                                <asp:CheckBox ID="chkRecInvoicesEdit" OnClick='UpdateCheckRecurringModule(this)' CssClass="css-checkbox" runat="server" Style="display: none;" Text="Edit" />
                                                                                <asp:CheckBox ID="chkRecInvoicesDelete" OnClick='UpdateCheckRecurringModule(this)' CssClass="css-checkbox" runat="server" Text="Delete" />&nbsp;
                                                                                <asp:CheckBox ID="chkRecInvoicesView" OnClick='UpdateCheckRecurringModule(this)' CssClass="css-checkbox" runat="server" Text="View" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr runat="server">
                                                                            <td runat="server">Recurring Tickets</td>
                                                                            <td runat="server">
                                                                                <asp:CheckBox ID="chkRecTicketsAdd" OnClick='UpdateCheckRecurringModule(this)' CssClass="css-checkbox" runat="server" Text="Add" />&nbsp;
                                                                                <asp:CheckBox ID="chkRecTicketsEdit" OnClick='UpdateCheckRecurringModule(this)' CssClass="css-checkbox" runat="server" Style="display: none;" Text="Edit" />
                                                                                <asp:CheckBox ID="chkRecTicketsDelete" OnClick='UpdateCheckRecurringModule(this)' CssClass="css-checkbox" runat="server" Text="Delete" />&nbsp;
                                                                                <asp:CheckBox ID="chkRecTicketsView" OnClick='UpdateCheckRecurringModule(this)' CssClass="css-checkbox" runat="server" Text="View" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr runat="server">
                                                                            <td runat="server">Renew/Escalate</td>
                                                                            <td runat="server">
                                                                                <asp:CheckBox ID="chkRenewEscalateAdd" OnClick='UpdateCheckRecurringModule(this)' CssClass="css-checkbox" runat="server" Text="Add" />&nbsp;
                                                                                <asp:CheckBox ID="chkRenewEscalateEdit" CssClass="css-checkbox" Style="display: none;" runat="server" Text="Edit" />
                                                                                <asp:CheckBox ID="chkRenewEscalateDelete" CssClass="css-checkbox" Style="display: none;" runat="server" Text="Delete" />
                                                                                <asp:CheckBox ID="chkRenewEscalateView" OnClick='UpdateCheckRecurringModule(this)' CssClass="css-checkbox" runat="server" Text="View" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr runat="server">
                                                                            <td runat="server">Violations </td>
                                                                            <td runat="server">
                                                                                <asp:CheckBox ID="chkViolationsAdd" CssClass="css-checkbox" runat="server" Text="Add" />&nbsp;
                                                                               <asp:CheckBox ID="chkViolationsDelete" CssClass="css-checkbox" runat="server" Text="Delete" />&nbsp;
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="form-section3-blank">
                                                            &nbsp;
                                                        </div>
                                                        <div class="form-section3">
                                                            <div class="section-ttle">
                                                                <asp:CheckBox ID="chkSchedule" ClientIDMode="Static" OnClick="OnCheckChanged(this);" CssClass="css-checkbox" AutoPostBack="false"  runat="server" Font-Bold="true" Text="Schedule module" />
                                                            </div>
                                                            <div class="input-field col s12">
                                                                <div class="row">
                                                                    <table>
                                                                        <tr runat="server">
                                                                            <td runat="server">Ticket
                                                                            </td>
                                                                            <td runat="server">
                                                                                <asp:CheckBox ID="chkTicketListAdd" OnClick='UpdateCheckScheduleModule(this)' CssClass="css-checkbox" runat="server" Text="Add" />&nbsp;
                                                                                <asp:CheckBox ID="chkTicketListEdit" OnClick='UpdateCheckScheduleModule(this)' CssClass="css-checkbox" runat="server" Text="Edit" />&nbsp;
                                                                                <asp:CheckBox ID="chkTicketListDelete" OnClick='UpdateCheckScheduleModule(this)' CssClass="css-checkbox" runat="server" Text="Delete" />&nbsp;
                                                                                <asp:CheckBox ID="chkTicketListReport" OnClick='UpdateCheckScheduleModule(this)' CssClass="css-checkbox" runat="server" Text="Report" />&nbsp;
                                                                                <asp:CheckBox ID="chkTicketListView" OnClick='UpdateCheckScheduleModule(this)' CssClass="css-checkbox" runat="server" Text="View" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr runat="server">
                                                                            <td runat="server">Completed Ticket</td>
                                                                            <td runat="server">
                                                                                <asp:CheckBox ID="chkResolveTicketAdd" OnClick='UpdateCheckScheduleModule(this)' CssClass="css-checkbox" runat="server" Text="Add" />&nbsp;
                                                                                <asp:CheckBox ID="chkResolveTicketEdit" OnClick='UpdateCheckScheduleModule(this)' CssClass="css-checkbox" runat="server" Text="Edit" />&nbsp;
                                                                                <asp:CheckBox ID="chkResolveTicketDelete" OnClick='UpdateCheckScheduleModule(this)' CssClass="css-checkbox" runat="server" Text="Delete" />&nbsp;
                                                                                <asp:CheckBox ID="chkResolveTicketReport" Style="display: none" CssClass="css-checkbox" runat="server" Text="Report" />&nbsp;
                                                                                <asp:CheckBox ID="chkResolveTicketView" OnClick='UpdateCheckScheduleModule(this)' CssClass="css-checkbox" runat="server" Text="View" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr runat="server">
                                                                            <td runat="server">
                                                                                <asp:CheckBox ID="chkMassReview" AutoPostBack="false" CssClass="css-checkbox" runat="server" Text="Mass Review Ticket" />
                                                                                <asp:CheckBox ID="chkMassTimesheetCheck" AutoPostBack="false" CssClass="css-checkbox" runat="server" Text="Mass Review Timesheet" />
                                                                            </td>
                                                                            <td runat="server">
                                                                                <asp:CheckBox ID="chkMassPayrollTicket" AutoPostBack="false" CssClass="css-checkbox" runat="server" Text="Mass Review Payroll" />
                                                                            </td>
                                                                            <td runat="server">&nbsp;</td>
                                                                        </tr>
                                                                        <tr runat="server">
                                                                            <td runat="server">
                                                                                <asp:CheckBox ID="chkScheduleBoard" CssClass="css-checkbox" AutoPostBack="false" runat="server" Text="Schedule Board" />
                                                                            </td>

                                                                            <td runat="server">
                                                                                <asp:CheckBox ID="chkRouteBuilderView"  AutoPostBack="false" CssClass="css-checkbox" runat="server" Text="Route Builder" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr runat="server">
                                                                            <td runat="server">
                                                                                <asp:CheckBox ID="chkTimestampFix" AutoPostBack="false" CssClass="css-checkbox" runat="server" Text="Timestamps Fixed" />
                                                                            </td>
                                                                            <td runat="server">
                                                                                <asp:CheckBox ID="chkTimesheetadd" Style="display: none" CssClass="css-checkbox" runat="server" Text="Add" />
                                                                                <asp:CheckBox ID="chkTimesheetedit" Style="display: none" CssClass="css-checkbox" runat="server" Text="Edit" />
                                                                                <asp:CheckBox ID="chkTimesheetdelete" Style="display: none" CssClass="css-checkbox" runat="server" Text="Delete" />
                                                                                <asp:CheckBox ID="chkTimesheetreport" Style="display: none" CssClass="css-checkbox" runat="server" Text="Report" />
                                                                                <asp:CheckBox ID="chkTimesheetview" AutoPostBack="false" CssClass="css-checkbox" runat="server" Text="Timesheet Entry" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr runat="server">
                                                                            <td runat="server">
                                                                                <asp:CheckBox ID="chkETimesheetadd" Style="display: none" CssClass="css-checkbox" runat="server" Text="Add" />
                                                                                <asp:CheckBox ID="chkETimesheetedit" Style="display: none" CssClass="css-checkbox" runat="server" Text="Edit" />
                                                                                <asp:CheckBox ID="chkETimesheetdelete" Style="display: none" CssClass="css-checkbox" runat="server" Text="Delete" />
                                                                                <asp:CheckBox ID="chkETimesheetreport" Style="display: none" CssClass="css-checkbox" runat="server" Text="Report" />
                                                                                <asp:CheckBox ID="chkETimesheetview" AutoPostBack="false" CssClass="css-checkbox" runat="server" Text="e-Timesheet (Payroll data)" />
                                                                            </td>
                                                                            <td runat="server">
                                                                                <asp:CheckBox ID="chkMapView" AutoPostBack="false" CssClass="css-checkbox" runat="server" Text="Map" />
                                                                                <asp:CheckBox ID="chkTicketVoidPermission" AutoPostBack="false" CssClass="css-checkbox" runat="server" Text="Void" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr runat="server">
                                                                            <td runat="server">
                                                                                <asp:CheckBox ID="chkMapAdd" Style="display: none" CssClass="css-checkbox" runat="server" Text="Add" />
                                                                                <asp:CheckBox ID="chkMapEdit" Style="display: none" CssClass="css-checkbox" runat="server" Text="Edit" />
                                                                                <asp:CheckBox ID="chkMapDelete" Style="display: none" CssClass="css-checkbox" runat="server" Text="Delete" />
                                                                                <asp:CheckBox ID="chkMapReport" Style="display: none" CssClass="css-checkbox" runat="server" Text="Report" />
                                                                            </td>
                                                                            <td runat="server">
                                                                                <asp:CheckBox ID="chkRouteBuilderAdd" Style="display: none" CssClass="css-checkbox" runat="server" Text="Add" />
                                                                                <asp:CheckBox ID="chkRouteBuilderEdit" Style="display: none" CssClass="css-checkbox" runat="server" Text="Edit" />
                                                                                <asp:CheckBox ID="chkRouteBuilderDelete" Style="display: none" CssClass="css-checkbox" runat="server" Text="Delete" />
                                                                                <asp:CheckBox ID="chkRouteBuilderReport" Style="display: none" CssClass="css-checkbox" runat="server" Text="Report" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div style="clear: both;"></div>
                                                <div class="form-content-wrap">
                                                    <div class="form-content-pd">

                                                        <div class="form-section3">
                                                            <div class="section-ttle">
                                                                <b>
                                                                    <asp:CheckBox ID="chkProjectmodule" ClientIDMode="Static" OnClick="OnCheckChanged(this);" CssClass="css-checkbox" AutoPostBack="false" runat="server" Text="Project module" />
                                                                </b>
                                                            </div>
                                                            <div class="input-field col s12">
                                                                <div class="row">
                                                                    <table>
                                                                        <tr runat="server">
                                                                            <td runat="server">Project
                                                                            </td>
                                                                            <td runat="server">
                                                                                <asp:CheckBox OnClick='UpdateCheckProjectModule(this)' ID="chkProjectadd" CssClass="css-checkbox" runat="server" Text="Add" />&nbsp;
                                                                                <asp:CheckBox OnClick='UpdateCheckProjectModule(this)' ID="chkProjectEdit" CssClass="css-checkbox" runat="server" Text="Edit" />&nbsp;
                                                                                <asp:CheckBox OnClick='UpdateCheckProjectModule(this)' ID="chkProjectDelete" CssClass="css-checkbox" runat="server" Text="Delete" />&nbsp;
                                                                                <asp:CheckBox OnClick='UpdateCheckProjectModule(this)' ID="chkProjectView" CssClass="css-checkbox" runat="server" Text="View" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr runat="server" style="display: none;">
                                                                            <td runat="server"></td>
                                                                            <td runat="server">
                                                                                <asp:CheckBox ID="CheckBox5" CssClass="css-checkbox" runat="server" Style="display: none;" Text="Add" />&nbsp;
                                                                                <asp:CheckBox ID="CheckBox6" CssClass="css-checkbox" runat="server" Style="display: none;" Text="Edit" />&nbsp;
                                                                                <asp:CheckBox ID="CheckBox7" CssClass="css-checkbox" runat="server" Style="display: none;" Text="Delete" />&nbsp;
                                                                                <asp:CheckBox ID="CheckBox1" CssClass="css-checkbox" runat="server" Style="display: none;" Text="Add" />&nbsp;
                                                                                <asp:CheckBox ID="CheckBox2" CssClass="css-checkbox" runat="server" Style="display: none;" Text="Edit" />&nbsp;
                                                                                <asp:CheckBox ID="CheckBox3" CssClass="css-checkbox" runat="server" Style="display: none;" Text="Delete" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr runat="server">
                                                                            <td runat="server">Project Template
                                                                            </td>
                                                                            <td runat="server">
                                                                                <asp:CheckBox OnClick='UpdateCheckProjectModule(this)' ID="chkProjectTempAdd" CssClass="css-checkbox" runat="server" Text="Add" />&nbsp;
                                                                                <asp:CheckBox OnClick='UpdateCheckProjectModule(this)' ID="chkProjectTempEdit" CssClass="css-checkbox" runat="server" Text="Edit" />&nbsp;
                                                                                <asp:CheckBox OnClick='UpdateCheckProjectModule(this)' ID="chkProjectTempDelete" CssClass="css-checkbox" runat="server" Text="Delete" />&nbsp; 
                                                                                <asp:CheckBox OnClick='UpdateCheckProjectModule(this)' ID="chkProjectTempView" CssClass="css-checkbox" runat="server" Text="View" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr runat="server">
                                                                            <td runat="server">BOM
                                                                            </td>
                                                                            <td runat="server">
                                                                                <asp:CheckBox OnClick='UpdateCheckProjectModule(this)' ID="chkAddBOM" CssClass="css-checkbox" runat="server" Text="Add" />&nbsp;
                                                                                <asp:CheckBox OnClick='UpdateCheckProjectModule(this)' ID="chkEditBOM" CssClass="css-checkbox" runat="server" Text="Edit" />&nbsp;
                                                                                <asp:CheckBox OnClick='UpdateCheckProjectModule(this)' ID="chkDeleteBOM" CssClass="css-checkbox" runat="server" Text="Delete" />&nbsp;
                                                                                <asp:CheckBox OnClick='UpdateCheckProjectModule(this)' ID="chkViewBOM" CssClass="css-checkbox" runat="server" Text="View" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr runat="server">
                                                                            <td runat="server">Billing
                                                                            </td>
                                                                            <td runat="server">
                                                                                <asp:CheckBox OnClick='UpdateCheckProjectModule(this)' ID="chkAddMilesStones" CssClass="css-checkbox" runat="server" Text="Add" />&nbsp;
                                                                                <asp:CheckBox OnClick='UpdateCheckProjectModule(this)' ID="chkEditMilesStones" CssClass="css-checkbox" runat="server" Text="Edit" />&nbsp;
                                                                                <asp:CheckBox OnClick='UpdateCheckProjectModule(this)' ID="chkDeleteMilesStones" CssClass="css-checkbox" runat="server" Text="Delete" />&nbsp;
                                                                                <asp:CheckBox OnClick='UpdateCheckProjectModule(this)' ID="chkViewMilesStones" CssClass="css-checkbox" runat="server" Text="View" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr runat="server">
                                                                            <td runat="server">WIP
                                                                            </td>
                                                                            <td runat="server">
                                                                                <asp:CheckBox OnClick='UpdateCheckProjectModule(this)' ID="chkAddWIP" CssClass="css-checkbox" runat="server" Text="Add" />&nbsp;
                                                                                <asp:CheckBox OnClick='UpdateCheckProjectModule(this)' ID="chkEditWIP" CssClass="css-checkbox" runat="server" Text="Edit" />&nbsp;
                                                                                <asp:CheckBox OnClick='UpdateCheckProjectModule(this)' ID="chkDeleteWIP" CssClass="css-checkbox" runat="server" Text="Delete" />&nbsp;
                                                                                <asp:CheckBox OnClick='UpdateCheckProjectModule(this)' ID="chkViewWIP" CssClass="css-checkbox" runat="server" Text="View" />
                                                                                <asp:CheckBox OnClick='UpdateCheckProjectModule(this)' ID="chkReportWIP" CssClass="css-checkbox" runat="server" Text="Report" />&nbsp;
                                                                            </td>
                                                                        </tr>
                                                                        <tr runat="server">
                                                                            <td runat="server">Project status</td>
                                                                            <td runat="server">
                                                                                <asp:CheckBox OnClick='UpdateCheckProjectModule(this)' ID="chkJobClosePermission" CssClass="css-checkbox" runat="server" Text="Close" />
                                                                                <asp:CheckBox OnClick='UpdateCheckProjectModule(this)' ID="chkJobCompletedPermission" CssClass="css-checkbox" runat="server" Text="Complete" />
                                                                                <asp:CheckBox OnClick='UpdateCheckProjectModule(this)' ID="chkJobReopenPermission" CssClass="css-checkbox" runat="server" Text="Reopen" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr runat="server">
                                                                            <td runat="server">
                                                                                <asp:CheckBox ID="chkViewProjectList" CssClass="css-checkbox" runat="server" Text="ProjectList Finance" />
                                                                            </td>
                                                                            <td runat="server">
                                                                                <asp:CheckBox ID="chkViewFinance" CssClass="css-checkbox" runat="server" Text="Project Finance" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr runat="server">
                                                                            <td runat="server">
                                                                                <asp:CheckBox ID="chkProjectManager" runat="server"  CssClass="css-checkbox" Text="Project Manager" />
                                                                            </td>
                                                                            <td runat="server">
                                                                                <asp:CheckBox ID="chkAssignedProject" runat="server"  CssClass="css-checkbox" Text="Assigned Projects" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="form-section3-blank">
                                                            &nbsp;
                                                        </div>
                                                        <div class="form-section3">
                                                            <div class="section-ttle">
                                                                <b> 
                                                                    <asp:CheckBox ID="chkInventorymodule" ClientIDMode="Static" OnClick="OnCheckChanged(this);" CssClass="css-checkbox" runat="server" AutoPostBack="false" Text="Inventory module" />
                                                                </b>
                                                            </div>
                                                            <div class="input-field col s12">
                                                                <div class="row">
                                                                    <table>
                                                                        <tr runat="server">
                                                                            <td runat="server">Inventory Item List</td>
                                                                            <td runat="server">
                                                                                <asp:CheckBox OnClick='UpdateCheckInventoryModule(this)' ID="chkInventoryItemadd" CssClass="css-checkbox" runat="server" Text="Add" />&nbsp;
                                                                                <asp:CheckBox OnClick='UpdateCheckInventoryModule(this)' ID="chkInventoryItemedit" CssClass="css-checkbox" runat="server" Text="Edit" />&nbsp;
                                                                                <asp:CheckBox OnClick='UpdateCheckInventoryModule(this)' ID="chkInventoryItemdelete" CssClass="css-checkbox" runat="server" Text="Delete" />&nbsp;
                                                                                <asp:CheckBox OnClick='UpdateCheckInventoryModule(this)' ID="chkInventoryItemview" CssClass="css-checkbox" runat="server" Text="View" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr runat="server">
                                                                            <td runat="server">Inventory Adjustment</td>
                                                                            <td runat="server">
                                                                                <asp:CheckBox OnClick='UpdateCheckInventoryModule(this)' ID="chkInventoryAdjustmentadd" CssClass="css-checkbox" runat="server" Text="Add" />&nbsp;
                                                                                <asp:CheckBox OnClick='UpdateCheckInventoryModule(this)' ID="chkInventoryAdjustmentedit" CssClass="css-checkbox" runat="server" Text="Edit" />&nbsp;
                                                                                <asp:CheckBox OnClick='UpdateCheckInventoryModule(this)' ID="chkInventoryAdjustmentdelete" CssClass="css-checkbox" runat="server" Text="Delete" />&nbsp;
                                                                                <asp:CheckBox OnClick='UpdateCheckInventoryModule(this)' ID="chkInventoryAdjustmentview" CssClass="css-checkbox" runat="server" Text="View" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr runat="server">
                                                                            <td runat="server">Inventory WareHouse</td>
                                                                            <td runat="server">
                                                                                <asp:CheckBox OnClick='UpdateCheckInventoryModule(this)' ID="chkInventoryWareHouseadd" CssClass="css-checkbox" runat="server" Text="Add" />&nbsp;
                                                                                <asp:CheckBox OnClick='UpdateCheckInventoryModule(this)' ID="chkInventoryWareHouseedit" CssClass="css-checkbox" runat="server" Text="Edit" />&nbsp;
                                                                                <asp:CheckBox OnClick='UpdateCheckInventoryModule(this)' ID="chkInventoryWareHousedelete" CssClass="css-checkbox" runat="server" Text="Delete" />&nbsp;
                                                                                <asp:CheckBox OnClick='UpdateCheckInventoryModule(this)' ID="chkInventoryWareHouseview" CssClass="css-checkbox" runat="server" Text="View" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr runat="server">
                                                                            <td runat="server">Inventory setup</td>
                                                                            <td runat="server">
                                                                                <asp:CheckBox OnClick='UpdateCheckInventoryModule(this)' ID="chkInventorysetupadd" CssClass="css-checkbox" runat="server" Text="Add" />&nbsp;
                                                                                <asp:CheckBox OnClick='UpdateCheckInventoryModule(this)' ID="chkInventorysetupedit" CssClass="css-checkbox" runat="server" Text="Edit" />&nbsp;
                                                                                <asp:CheckBox OnClick='UpdateCheckInventoryModule(this)' ID="chkInventorysetupdelete" CssClass="css-checkbox" runat="server" Text="Delete" />&nbsp;
                                                                                <asp:CheckBox OnClick='UpdateCheckInventoryModule(this)' ID="chkInventorysetupview" CssClass="css-checkbox" runat="server" Text="View" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr runat="server">
                                                                            <td runat="server">Inventory Finance</td>
                                                                            <td runat="server">
                                                                                <asp:CheckBox OnClick='UpdateCheckInventoryModule(this)' ID="chkInventoryFinanceAdd" CssClass="css-checkbox" runat="server" Text="Add" />&nbsp;
                                                                                <asp:CheckBox OnClick='UpdateCheckInventoryModule(this)' ID="chkInventoryFinanceedit" CssClass="css-checkbox" runat="server" Text="Edit" />&nbsp;
                                                                                <asp:CheckBox OnClick='UpdateCheckInventoryModule(this)' ID="chkInventoryFinancedelete" CssClass="css-checkbox" runat="server" Text="Delete" />&nbsp;
                                                                                <asp:CheckBox OnClick='UpdateCheckInventoryModule(this)' ID="chkInventoryFinanceview" CssClass="css-checkbox" runat="server" Text="View" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                            </div>
                                                        </div>

                                                        <div class="form-section3-blank">
                                                            &nbsp;
                                                        </div>
                                                        <div class="form-section3">
                                                            <div class="section-ttle">
                                                                <asp:CheckBox ID="chkSalesMgr" ClientIDMode="Static" OnClick="OnCheckChanged(this);" CssClass="css-checkbox" AutoPostBack="false" runat="server" Font-Bold="true" Text="Sales module" />
                                                            </div>
                                                            <div class="input-field col s12">
                                                                <div class="row">
                                                                    <table>
                                                                        <tr runat="server">
                                                                            <td runat="server">Leads </td>
                                                                            <td runat="server">
                                                                                <asp:CheckBox ID="chkLeadAdd" OnClick='UpdateCheckSalesModule(this)' CssClass="css-checkbox" runat="server" Text="Add" />&nbsp;
                                                                                <asp:CheckBox ID="chkLeadEdit" OnClick='UpdateCheckSalesModule(this)' CssClass="css-checkbox" runat="server" Text="Edit" />&nbsp;
                                                                                <asp:CheckBox ID="chkLeadDelete" OnClick='UpdateCheckSalesModule(this)' CssClass="css-checkbox" runat="server" Text="Delete" />&nbsp;
                                                                                <asp:CheckBox ID="chkLeadReport" OnClick='UpdateCheckSalesModule(this)' CssClass="css-checkbox" runat="server" Text="Report" />&nbsp;
                                                                                <asp:CheckBox ID="chkLeadView"  OnClick='UpdateCheckSalesModule(this)' CssClass="css-checkbox" runat="server" Text="View" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr runat="server">
                                                                            <td runat="server">Opportunities </td>
                                                                            <td runat="server">
                                                                                <asp:CheckBox ID="chkOppAdd" OnClick='UpdateCheckSalesModule(this)' CssClass="css-checkbox" runat="server" Text="Add" />&nbsp;
                                                                                <asp:CheckBox ID="chkOppEdit" OnClick='UpdateCheckSalesModule(this)' CssClass="css-checkbox" runat="server" Text="Edit" />&nbsp;
                                                                                <asp:CheckBox ID="chkOppDelete" OnClick='UpdateCheckSalesModule(this)' CssClass="css-checkbox" runat="server" Text="Delete" />&nbsp;
                                                                                <asp:CheckBox ID="chkOppReport" OnClick='UpdateCheckSalesModule(this)' CssClass="css-checkbox" runat="server" Text="Report" />&nbsp;
                                                                                <asp:CheckBox ID="chkOppView" OnClick='UpdateCheckSalesModule(this)' CssClass="css-checkbox" runat="server" Text="View" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr runat="server">
                                                                            <td runat="server">Estimate  </td>
                                                                            <td runat="server">
                                                                                <asp:CheckBox ID="chkEstimateAdd" OnClick='UpdateCheckSalesModule(this)' CssClass="css-checkbox" runat="server" Text="Add" />&nbsp;
                                                                                <asp:CheckBox ID="chkEstimateEdit" OnClick='UpdateCheckSalesModule(this)' CssClass="css-checkbox" runat="server" Text="Edit" />&nbsp;
                                                                                <asp:CheckBox ID="chkEstimateDelete" OnClick='UpdateCheckSalesModule(this)' CssClass="css-checkbox" runat="server" Text="Delete" />&nbsp;
                                                                                <asp:CheckBox ID="chkEstimateReport" OnClick='UpdateCheckSalesModule(this)' CssClass="css-checkbox" runat="server" Text="Report" />&nbsp;
                                                                                <asp:CheckBox ID="chkEstimateView"  OnClick='UpdateCheckSalesModule(this)' CssClass="css-checkbox" runat="server" Text="View" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr runat="server">
                                                                            <td runat="server">
                                                                                <asp:CheckBox ID="chkCompleteTask" AutoPostBack="false" CssClass="css-checkbox" runat="server" Text="Complete Task" />
                                                                            </td>
                                                                            <td runat="server">
                                                                                <asp:CheckBox ID="chkFollowUp"  AutoPostBack="false" CssClass="css-checkbox" runat="server" Text="Task FollowUp" />
                                                                                <asp:CheckBox ID="chkTasks"  AutoPostBack="false" CssClass="css-checkbox" runat="server" Text="Tasks" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr runat="server">
                                                                            <td runat="server">
                                                                                <asp:CheckBox ID="chkConvertEstimate" AutoPostBack="false" CssClass="css-checkbox" runat="server" Text="Convert Estimate" />
                                                                            </td>
                                                                            <td runat="server">
                                                                                <asp:CheckBox ID="chkSalesSetup" AutoPostBack="false" CssClass="css-checkbox" runat="server" Text="Sales Setup" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr runat="server">
                                                                            <td runat="server">
                                                                                <asp:CheckBox ID="chkSalesperson" AutoPostBack="false" CssClass="css-checkbox" runat="server" Text="Salesperson" />
                                                                            </td>
                                                                            <td runat="server">
                                                                                <asp:CheckBox ID="chkSalesAssigned" AutoPostBack="false" CssClass="css-checkbox" runat="server" Text="Sales Assigned" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr runat="server">
                                                                            <td runat="server">
                                                                                <asp:CheckBox ID="chkNotification" AutoPostBack="false" CssClass="css-checkbox" runat="server" Text="Opportunity Notification" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr runat="server">
                                                                            <td runat="server">
                                                                                <asp:CheckBox ID="chkEstApprovalStatus" AutoPostBack="false" CssClass="css-checkbox" runat="server" Text="Estimate Approve Proposal" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div style="clear: both;"></div>

                                                <div class="form-content-wrap">
                                                    <div class="form-content-pd">
                                                        <div class="form-section3">
                                                            <div class="section-ttle">
                                                                <asp:CheckBox ID="chkAccountPayable" ClientIDMode="Static" OnClick="OnCheckChanged(this);" CssClass="css-checkbox" AutoPostBack="false" runat="server" Font-Bold="true" Text="AP module" />
                                                            </div>
                                                            <div class="input-field col s12">
                                                                <div class="row">
                                                                    <table>
                                                                        <tr runat="server">
                                                                            <td runat="server">Vendors</td>
                                                                            <td runat="server">
                                                                                <asp:CheckBox ID="chkVendorsAdd" OnClick='UpdateCheckAPModule(this)' CssClass="css-checkbox" runat="server" Text="Add" />&nbsp;
                                                                                <asp:CheckBox ID="chkVendorsEdit" OnClick='UpdateCheckAPModule(this)' CssClass="css-checkbox" runat="server" Text="Edit" />&nbsp;
                                                                                <asp:CheckBox ID="chkVendorsDelete" OnClick='UpdateCheckAPModule(this)' CssClass="css-checkbox" runat="server" Text="Delete" />&nbsp;
                                                                                <asp:CheckBox ID="chkVendorsView" OnClick='UpdateCheckAPModule(this)' CssClass="css-checkbox" runat="server" Text="View" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr runat="server">
                                                                            <td runat="server">Bills</td>
                                                                            <td runat="server">
                                                                                <asp:CheckBox ID="chkAddBills" OnClick='UpdateCheckAPModule(this)' CssClass="css-checkbox" runat="server" Text="Add" />&nbsp;
                                                                                <asp:CheckBox ID="chkEditBills" OnClick='UpdateCheckAPModule(this)' CssClass="css-checkbox" runat="server" Text="Edit" />&nbsp;
                                                                                <asp:CheckBox ID="chkDeleteBills" OnClick='UpdateCheckAPModule(this)' CssClass="css-checkbox" runat="server" Text="Delete" />&nbsp;
                                                                                <asp:CheckBox ID="chkViewBills" OnClick='UpdateCheckAPModule(this)' CssClass="css-checkbox" runat="server" Text="View" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr runat="server">
                                                                            <td runat="server">Manage Checks</td>
                                                                            <td runat="server">
                                                                                <asp:CheckBox ID="chkAddManageChecks" OnClick='UpdateCheckAPModule(this)' CssClass="css-checkbox" runat="server" Text="Add" />&nbsp;
                                                                                <asp:CheckBox ID="chkEditManageChecks" OnClick='UpdateCheckAPModule(this)' CssClass="css-checkbox" runat="server" Text="Edit" />&nbsp;
                                                                                <asp:CheckBox ID="chkDeleteManageChecks" OnClick='UpdateCheckAPModule(this)' CssClass="css-checkbox" runat="server" Text="Delete" />&nbsp;
                                                                                <asp:CheckBox ID="chkViewManageChecks" OnClick='UpdateCheckAPModule(this)' CssClass="css-checkbox" runat="server" Text="View" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr runat="server">
                                                                            <td runat="server" colspan="2">
                                                                                <asp:CheckBox ID="chkShowBankBalances" OnClick='UpdateCheckAPModule(this)' CssClass="css-checkbox" runat="server" Text="Show Bank Balances" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="form-section3-blank">
                                                            &nbsp;
                                                        </div>
                                                        <div class="form-section3">
                                                            <div class="section-ttle">
                                                                <asp:CheckBox ID="chkFinancialmodule" ClientIDMode="Static" OnClick="OnCheckChanged(this);" CssClass="css-checkbox" AutoPostBack="false" runat="server" Font-Bold="true" Text="Financial module" />
                                                            </div>
                                                            <div class="input-field col s12">
                                                                <div class="row">
                                                                    <table>
                                                                        <tr runat="server">
                                                                            <td runat="server">Chart of Accounts</td>
                                                                            <td runat="server">
                                                                                <asp:CheckBox ID="chkChartAdd" OnClick='UpdateCheckFinancialModule(this)' CssClass="css-checkbox" runat="server" Text="Add" />&nbsp;
                                                                                <asp:CheckBox ID="chkChartEdit" OnClick='UpdateCheckFinancialModule(this)' CssClass="css-checkbox" runat="server" Text="Edit" />&nbsp;
                                                                                <asp:CheckBox ID="chkChartDelete" OnClick='UpdateCheckFinancialModule(this)' CssClass="css-checkbox" runat="server" Text="Delete" />&nbsp;
                                                                                <asp:CheckBox ID="chkChartView" OnClick='UpdateCheckFinancialModule(this)' CssClass="css-checkbox" runat="server" Text="View" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr runat="server">
                                                                            <td runat="server">Journal Entry</td>
                                                                            <td runat="server">
                                                                                <asp:CheckBox ID="chkJournalEntryAdd" OnClick='UpdateCheckFinancialModule(this)' CssClass="css-checkbox" runat="server" Text="Add" />&nbsp;
                                                                                <asp:CheckBox ID="chkJournalEntryEdit" OnClick='UpdateCheckFinancialModule(this)' CssClass="css-checkbox" runat="server" Text="Edit" />&nbsp;
                                                                                <asp:CheckBox ID="chkJournalEntryDelete" OnClick='UpdateCheckFinancialModule(this)' CssClass="css-checkbox" runat="server" Text="Delete" />&nbsp;
                                                                                <asp:CheckBox ID="chkJournalEntryView" OnClick='UpdateCheckFinancialModule(this)' CssClass="css-checkbox" runat="server" Text="View" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr runat="server">
                                                                            <td runat="server">Bank Reconciliation</td>
                                                                            <td runat="server">
                                                                                <asp:CheckBox ID="chkBankAdd" OnClick='UpdateCheckFinancialModule(this)' CssClass="css-checkbox" runat="server" Text="Add" />&nbsp;
                                                                                <asp:CheckBox ID="chkBankEdit" OnClick='UpdateCheckFinancialModule(this)' CssClass="css-checkbox" runat="server" Text="Edit" />&nbsp;
                                                                                <asp:CheckBox ID="chkBankDelete" OnClick='UpdateCheckFinancialModule(this)' CssClass="css-checkbox" runat="server" Text="Delete" />&nbsp;
                                                                                <asp:CheckBox ID="chkBankView" OnClick='UpdateCheckFinancialModule(this)' CssClass="css-checkbox" runat="server" Text="View" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr runat="server">
                                                                            <td runat="server" colspan="2">
                                                                                <asp:CheckBox ID="chkFinanceStatement" CssClass="css-checkbox" runat="server" Text="Financial Statement Module" />
                                                                                <asp:CheckBox ID="chkFinanceMgr" CssClass="css-checkbox" runat="server" Style="display: none" Text="Financial Manager" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                            </div>
                                                        </div>

                                                        <div class="form-section3-blank">
                                                            &nbsp;
                                                        </div>
                                                        <div class="form-section3">
                                                            <div class="section-ttle">
                                                                <asp:CheckBox ID="chkBillingmodule" ClientIDMode="Static" OnClick="OnCheckChanged(this);" CssClass="css-checkbox" AutoPostBack="false" runat="server" Font-Bold="true" Text="Billing module" />
                                                            </div>
                                                            <div class="input-field col s12">
                                                                <div class="row">
                                                                    <table>
                                                                        <tr runat="server">
                                                                            <td runat="server">Invoices</td>
                                                                            <td runat="server">
                                                                                <asp:CheckBox ID="chkInvoicesAdd" OnClick='UpdateCheckBillingModule(this)' CssClass="css-checkbox Billingmodule" runat="server" Text="Add" />&nbsp;
                                                                                <asp:CheckBox ID="chkInvoicesEdit" OnClick='UpdateCheckBillingModule(this)' CssClass="css-checkbox Billingmodule" runat="server" Text="Edit" />&nbsp;
                                                                                <asp:CheckBox ID="chkInvoicesDelete" OnClick='UpdateCheckBillingModule(this)' CssClass="css-checkbox Billingmodule" runat="server" Text="Delete" />&nbsp;
                                                                                <asp:CheckBox ID="chkInvoicesView"  OnClick='UpdateCheckBillingModule(this)' CssClass="css-checkbox Billingmodule" runat="server" Text="View" />
                                                                            </td>
                                                                        </tr>

                                                                        <tr runat="server">
                                                                            <td runat="server">Billing Codes</td>
                                                                            <td runat="server">
                                                                                <asp:CheckBox ID="chkBillingcodesAdd" OnClick='UpdateCheckBillingModule(this)' CssClass="css-checkbox Billingmodule" runat="server" Text="Add" />&nbsp;
                                                                                <asp:CheckBox ID="chkBillingcodesEdit" OnClick='UpdateCheckBillingModule(this)' CssClass="css-checkbox Billingmodule" runat="server" Text="Edit" />&nbsp;
                                                                                <asp:CheckBox ID="chkBillingcodesDelete" OnClick='UpdateCheckBillingModule(this)' CssClass="css-checkbox Billingmodule" runat="server" Text="Delete" />&nbsp;
                                                                                <asp:CheckBox ID="chkBillingcodesView" OnClick='UpdateCheckBillingModule(this)' CssClass="css-checkbox Billingmodule" runat="server" Text="View" />
                                                                            </td>
                                                                        </tr>

                                                                        <tr runat="server" id="trOnlinePayment">
                                                                            <td runat="server">Online Payment</td>
                                                                            <td runat="server">
                                                                                <asp:CheckBox ID="chkPaymentHistoryView" AutoPostBack="false" CssClass="css-checkbox Billingmodule" runat="server" Text="View" />
                                                                                <asp:CheckBox ID="chkPaymentHistoryAdd" Style="display: none;" CssClass="css-checkbox Billingmodule" runat="server" Text="Add" />&nbsp;
                                                                                <asp:CheckBox ID="chkPaymentHistoryEdit" CssClass="css-checkbox Billingmodule" Style="display: none;" runat="server" Text="Edit" />&nbsp;
                                                                                <asp:CheckBox ID="chkPaymentHistoryDelete" CssClass="css-checkbox Billingmodule" Style="display: none;" runat="server" Text="Delete" />&nbsp;
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div style="clear: both;"></div>

                                                <div class="form-content-wrap">
                                                    <div class="form-content-pd">
                                                        <div class="form-section3">
                                                            <div class="section-ttle">
                                                                <asp:CheckBox ID="chkPurchasingmodule" ClientIDMode="Static" OnClick="OnCheckChanged(this);" AutoPostBack="false" CssClass="css-checkbox" runat="server" Font-Bold="true" Text="Purchasing module" />
                                                            </div>
                                                            <div class="input-field col s12">
                                                                <div class="row">
                                                                    <table>
                                                                        <tr runat="server">
                                                                            <td runat="server">PO</td>
                                                                            <td runat="server">
                                                                                <asp:CheckBox ID="chkPOAdd" OnClick='UpdateCheckPurchasingModule(this)' CssClass="css-checkbox" runat="server" Text="Add" />&nbsp;
                                                                                <asp:CheckBox ID="chkPOEdit" OnClick='UpdateCheckPurchasingModule(this)' CssClass="css-checkbox" runat="server" Text="Edit" />&nbsp;
                                                                                <asp:CheckBox ID="chkPODelete" OnClick='UpdateCheckPurchasingModule(this)' CssClass="css-checkbox" runat="server" Text="Delete" />&nbsp;
                                                                                <asp:CheckBox ID="chkPOView" OnClick='UpdateCheckPurchasingModule(this)' CssClass="css-checkbox" runat="server" Text="View" />
                                                                            </td>
                                                                        </tr>

                                                                        <tr runat="server">
                                                                            <td runat="server">Receive PO</td>
                                                                            <td runat="server">
                                                                                <asp:CheckBox ID="chkRPOAdd" OnClick='UpdateCheckPurchasingModule(this)' CssClass="css-checkbox" runat="server" Text="Add" />&nbsp;
                                                                                <asp:CheckBox ID="chkRPOEdit" OnClick='UpdateCheckPurchasingModule(this)' CssClass="css-checkbox" runat="server" Text="Edit" />&nbsp;
                                                                                <asp:CheckBox ID="chkRPODelete" OnClick='UpdateCheckPurchasingModule(this)' CssClass="css-checkbox" runat="server" Text="Delete" />&nbsp;
                                                                                <asp:CheckBox ID="chkRPOView" OnClick='UpdateCheckPurchasingModule(this)' CssClass="css-checkbox" runat="server" Text="View" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr runat="server">
                                                                            <td runat="server" colspan="2">
                                                                                <asp:CheckBox ID="chkPONotification" CssClass="css-checkbox" runat="server" Text="PO Notification" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="form-section3-blank">
                                                            &nbsp;
                                                        </div>
                                                        <div class="form-section3">
                                                            <div class="section-ttle">
                                                                <asp:CheckBox ID="chkProgram" ClientIDMode="Static" OnClick="OnCheckChanged(this);" CssClass="css-checkbox" AutoPostBack="false" runat="server" Font-Bold="true" Text="Program Module" />
                                                            </div>
                                                            <div class="input-field col s12">
                                                                <div class="row">
                                                                    <table>
                                                                        <tr runat="server">
                                                                            <%--<td runat="server">
                                                                                <asp:CheckBox ID="chkTimestampFix" CssClass="css-checkbox" runat="server" Text="Timestamps Fixed" />
                                                                            </td>--%>
                                                                            <td runat="server">
                                                                                <asp:CheckBox ID="chkExpenses" runat="server" CssClass="css-checkbox" Text="Enter expenses" /></td>
                                                                            <td runat="server">
                                                                                <asp:CheckBox ID="chkEmpMainten" CssClass="css-checkbox" runat="server" Text="Employee Maintenance" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr runat="server">
                                                                            <td runat="server">
                                                                                <asp:CheckBox ID="chkAccessUser" CssClass="css-checkbox" runat="server" Text="Users" />
                                                                            </td>
                                                                            <td runat="server">
                                                                                <asp:CheckBox ID="chkDispatch" CssClass="css-checkbox" runat="server" Text="Email Dispatch" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                            </div>
                                                        </div>

                                                        <div class="form-section3-blank">
                                                            &nbsp;
                                                        </div>
                                                        <div class="form-section3">

                                                            <div class="section-ttle"><b>Document/Contact </b></div>
                                                            <div class="input-field col s12">
                                                                <div class="row">
                                                                    <table>
                                                                        <tr runat="server">
                                                                            <td runat="server">Document</td>
                                                                            <td runat="server">
                                                                                <asp:CheckBox OnClick="UpdateCheckDocsNContactModule(this);" ID="chkDocumentAdd" CssClass="css-checkbox" runat="server" Text="Add" />&nbsp;
                                                                                <asp:CheckBox OnClick="UpdateCheckDocsNContactModule(this);" ID="chkDocumentEdit" CssClass="css-checkbox" runat="server" Text="Edit" />&nbsp;
                                                                                <asp:CheckBox OnClick="UpdateCheckDocsNContactModule(this);" ID="chkDocumentDelete" CssClass="css-checkbox" runat="server" Text="Delete" />&nbsp;
                                                                                <asp:CheckBox OnClick="UpdateCheckDocsNContactModule(this);" ID="chkDocumentView" CssClass="css-checkbox" runat="server" Text="View" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr runat="server">
                                                                            <td runat="server">Contact</td>
                                                                            <td runat="server">
                                                                                <asp:CheckBox OnClick="UpdateCheckDocsNContactModule(this);" ID="chkContactAdd" CssClass="css-checkbox" runat="server" Text="Add" />&nbsp;
                                                                                <asp:CheckBox OnClick="UpdateCheckDocsNContactModule(this);" ID="chkContactEdit" CssClass="css-checkbox" runat="server" Text="Edit" />&nbsp;
                                                                                <asp:CheckBox OnClick="UpdateCheckDocsNContactModule(this);" ID="chkContactDelete" CssClass="css-checkbox" runat="server" Text="Delete" />&nbsp;
                                                                                <asp:CheckBox OnClick="UpdateCheckDocsNContactModule(this);" ID="chkContactView" CssClass="css-checkbox" runat="server" Text="View" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                            </div>


                                                        </div>
                                                    </div>
                                                </div>
                                                <div style="clear: both;"></div>

                                                <%-- Payroll Module--%>
                                                <div class="form-content-wrap">
                                                    <div class="form-content-pd">
                                                        <div class="form-section3" runat="server" id="payrollsection" visible="false">
                                                            <div class="section-ttle">
                                                                <asp:CheckBox ID="payrollModulchck" ClientIDMode="Static" OnClick="OnCheckChanged(this);"  CssClass="css-checkbox" AutoPostBack="false" runat="server" Font-Bold="true" Text="Payroll module" />
                                                            </div>
                                                            <div class="input-field col s12">
                                                                <div class="row">
                                                                    <table>
                                                                        <tr runat="server">
                                                                            <td runat="server">Employees </td>
                                                                            <td runat="server">
                                                                                <asp:CheckBox ID="empAdd" OnClick='UpdateCheckPayrollModule(this)' CssClass="css-checkbox" runat="server" Text="Add" />&nbsp;
                                                                                <asp:CheckBox ID="empEdit" OnClick='UpdateCheckPayrollModule(this)' CssClass="css-checkbox" runat="server" Text="Edit" />&nbsp;
                                                                                <asp:CheckBox ID="empDelete" OnClick='UpdateCheckPayrollModule(this)' CssClass="css-checkbox" runat="server" Text="Delete" />&nbsp;
                                                                                <asp:CheckBox ID="empView" OnClick='UpdateCheckPayrollModule(this)' CssClass="css-checkbox" runat="server" Text="View" />&nbsp;
                                                                            </td>
                                                                        </tr>
                                                                        <tr runat="server">
                                                                            <td runat="server">Run Payroll </td>
                                                                            <td runat="server">
                                                                                <asp:CheckBox ID="runAdd" OnClick='UpdateCheckPayrollModule(this)' CssClass="css-checkbox" runat="server" Text="Add" />&nbsp;
                                                                                <asp:CheckBox ID="runEdit" OnClick='UpdateCheckPayrollModule(this)' CssClass="css-checkbox" runat="server" Text="Edit" />&nbsp;
                                                                                <asp:CheckBox ID="runDelete" OnClick='UpdateCheckPayrollModule(this)' CssClass="css-checkbox" runat="server" Text="Delete" />&nbsp;
                                                                                <asp:CheckBox ID="runView" OnClick='UpdateCheckPayrollModule(this)' CssClass="css-checkbox" runat="server" Text="View" />&nbsp;
                                                                            </td>
                                                                        </tr>
                                                                        <tr runat="server">
                                                                            <td runat="server">Payroll Checks  </td>
                                                                            <td runat="server">
                                                                                <asp:CheckBox ID="payrollchckAdd" OnClick='UpdateCheckPayrollModule(this)' CssClass="css-checkbox" runat="server" Text="Add" />&nbsp;
                                                                                <asp:CheckBox ID="payrollchckEdit" OnClick='UpdateCheckPayrollModule(this)' CssClass="css-checkbox" runat="server" Text="Edit" />&nbsp;
                                                                                <asp:CheckBox ID="payrollchckDelete" OnClick='UpdateCheckPayrollModule(this)' CssClass="css-checkbox" runat="server" Text="Delete" />&nbsp;
                                                                                <asp:CheckBox ID="payrollchckView" OnClick='UpdateCheckPayrollModule(this)' CssClass="css-checkbox" runat="server" Text="View" />&nbsp;
                                                                            </td>
                                                                        </tr>
                                                                        <tr runat="server">
                                                                            <td runat="server">Payroll Form  </td>
                                                                            <td runat="server">
                                                                                <asp:CheckBox ID="payrollformAdd" OnClick='UpdateCheckPayrollModule(this)' CssClass="css-checkbox" runat="server" Text="Add" />&nbsp;
                                                                                <asp:CheckBox ID="payrollformEdit" OnClick='UpdateCheckPayrollModule(this)' CssClass="css-checkbox" runat="server" Text="Edit" />&nbsp;
                                                                                <asp:CheckBox ID="payrollformDelete" OnClick='UpdateCheckPayrollModule(this)' CssClass="css-checkbox" runat="server" Text="Delete" />&nbsp;
                                                                                <asp:CheckBox ID="payrollformView" OnClick='UpdateCheckPayrollModule(this)' CssClass="css-checkbox" runat="server" Text="View" />&nbsp;
                                                                            </td>
                                                                        </tr>
                                                                        <tr runat="server">
                                                                            <td runat="server">Wages  </td>
                                                                            <td runat="server">
                                                                                <asp:CheckBox ID="wagesadd" OnClick='UpdateCheckPayrollModule(this)' CssClass="css-checkbox" runat="server" Text="Add" />&nbsp;
                                                                                <asp:CheckBox ID="wagesEdit" OnClick='UpdateCheckPayrollModule(this)' CssClass="css-checkbox" runat="server" Text="Edit" />&nbsp;
                                                                                <asp:CheckBox ID="wagesDelete" OnClick='UpdateCheckPayrollModule(this)' CssClass="css-checkbox" runat="server" Text="Delete" />&nbsp;
                                                                                <asp:CheckBox ID="wagesView" OnClick='UpdateCheckPayrollModule(this)' CssClass="css-checkbox" runat="server" Text="View" />&nbsp;
                                                                            </td>
                                                                        </tr>
                                                                        <tr runat="server">
                                                                            <td runat="server">Deductions  </td>
                                                                            <td runat="server">
                                                                                <asp:CheckBox ID="deductionsAdd" OnClick='UpdateCheckPayrollModule(this)' CssClass="css-checkbox" runat="server" Text="Add" />&nbsp;
                                                                                <asp:CheckBox ID="deductionsEdit" OnClick='UpdateCheckPayrollModule(this)' CssClass="css-checkbox" runat="server" Text="Edit" />&nbsp;
                                                                                <asp:CheckBox ID="deductionsDelete" OnClick='UpdateCheckPayrollModule(this)' CssClass="css-checkbox" runat="server" Text="Delete" />&nbsp;
                                                                                <asp:CheckBox ID="deductionsView" OnClick='UpdateCheckPayrollModule(this)' CssClass="css-checkbox" runat="server" Text="View" />&nbsp;
                                                                            </td>
                                                                        </tr>
                                                                        <tr runat="server">
                                                                            <td runat="server">
                                                                                <asp:CheckBox ID="chkMassPayrollTicket1" OnClick='UpdateCheckPayrollModule(this)' CssClass="css-checkbox" runat="server" Text="Mass Review Payroll" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div style="clear: both;"></div>
                                            </telerik:RadAjaxPanel>
                                        </asp:Panel>

                                        <div class="form-section-row">
                                            <div class="section-ttle">Purchase Order Details</div>
                                            <div class="form-section3">
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <%--<input id="email" type="text" class="validate">--%>
                                                        <asp:TextBox ID="txtPOLimit" runat="server" CssClass="validate" Text="0.00"></asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="txtPOLimit_FilteredTextBoxExtender" runat="server"
                                                            Enabled="True" TargetControlID="txtPOLimit" ValidChars="1234567890.99">
                                                        </asp:FilteredTextBoxExtender>
                                                        <label for="email">PO Limit($)</label>
                                                    </div>
                                                </div>

                                            </div>
                                            <div class="form-section3-blank">
                                                &nbsp;
                                            </div>
                                            <div class="form-section3">
                                                <div class="input-field col s5">
                                                    <div class="row">
                                                        <label class="drpdwn-label">Approve PO</label>
                                                        <asp:UpdatePanel ID="UpdatePanel14" runat="server">
                                                            <ContentTemplate>
                                                                <asp:DropDownList ID="ddlPOApprove" onChange="HideShowPOAmount(this.value)"
                                                                    runat="server" CssClass="browser-default">
                                                                    <asp:ListItem Value="0">No</asp:ListItem>
                                                                    <asp:ListItem Value="1">Yes</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </div>
                                                <div class="input-field col s2">
                                                    <div class="row">
                                                        &nbsp;
                                                    </div>
                                                </div>
                                                <div class="input-field col s5" id="divApprovePo" runat="server">
                                                    <div class="row">
                                                        <label class="drpdwn-label">Approve PO Amount</label>
                                                        <asp:UpdatePanel ID="UpdatePanel15" runat="server">
                                                            <ContentTemplate>
                                                                <asp:DropDownList ID="ddlPOApproveAmt" onChange="HideShowMinMaxAmount(this.value)"
                                                                    runat="server" CssClass="browser-default">
                                                                    <asp:ListItem Value="-1">Select</asp:ListItem>
                                                                    <asp:ListItem Value="0">Starting and max</asp:ListItem>
                                                                    <asp:ListItem Value="1">Greater than</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </div>

                                            </div>
                                            <div class="form-section3-blank">
                                                &nbsp;
                                            </div>
                                            <div class="form-section3">
                                                <div class="input-field col s5" id="divMinAmount" runat="server">
                                                    <div class="row">
                                                        <%--<input id="minAmount" type="text" class="validate">--%>
                                                        <asp:UpdatePanel ID="UpdatePanel16" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox ID="txtMinAmount" runat="server" CssClass="validate" Text="0.00"
                                                                    MaxLength="25"></asp:TextBox>
                                                                <label for="minAmount">Min Amount</label>

                                                                <asp:FilteredTextBoxExtender ID="txtMinAmount_FilteredTextBoxExtender" runat="server"
                                                                    Enabled="True" TargetControlID="txtMinAmount" ValidChars="1234567890.99">
                                                                </asp:FilteredTextBoxExtender>

                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </div>
                                                <div class="input-field col s2">
                                                    <div class="row">
                                                        &nbsp;
                                                    </div>
                                                </div>
                                                <div class="input-field col s5" id="divMaxAmount" runat="server">
                                                    <div class="row">
                                                        <%--<input id="maxAmount" type="text" class="validate">--%>
                                                        <asp:UpdatePanel ID="UpdatePanel17" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox ID="txtMaxAmount" runat="server" CssClass="form-control" Text="0.00" TabIndex="11" MaxLength="25"></asp:TextBox>
                                                                <asp:FilteredTextBoxExtender ID="txtMaxAmount_FilteredTextBoxExtender" runat="server"
                                                                    Enabled="True" TargetControlID="txtMaxAmount" ValidChars="1234567890.99">
                                                                </asp:FilteredTextBoxExtender>
                                                                <label for="txtMaxAmount">Max Amount</label>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                    </div>

                                </div>

                                <div style="clear: both;"></div>
                            </div>
                        </li>
                        <li id="tbLogs" runat="server" style="display: none">
                            <div id="accrdlogs" onclick="LoadLogs();" class="collapsible-header accrd accordian-text-custom"><i class="mdi-content-content-paste"></i>Logs</div>
                            <div class="collapsible-body">
                                <div class="form-content-wrap">
                                    <div class="form-content-pd">
                                        <div class="grid_container">
                                            <div class="form-section-row" style="margin-bottom: 0 !important;">
                                                <div class="RadGrid RadGrid_Material">
                                                    <asp:HiddenField ID="hdnLoadLogs" runat="server" Value="0" />
                                                    <asp:LinkButton ID="lnkLoadLogs" Style="display: none;" runat="server" Text="" OnClick="lnkLoadLogs_Click" />
                                                    <%--<telerik:RadAjaxPanel ID="RadAjaxPanel_gvLogs" runat="server" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">--%>
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
                                                    <%--</telerik:RadAjaxPanel>--%>
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
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="javascriptHolder" runat="Server">
    <script>
        function showConfirm() {
            return confirm('Your changes can affect to the permissions of selected users.  Are you sure you want to continue?')
        }

        function showNotyConfirm() {
            return noty({
                dismissQueue: true,
                layout: 'topCenter',
                theme: 'noty_theme_default',
                animateOpen: { height: 'toggle' },
                animateClose: { height: 'toggle' },
                easing: 'swing',
                text: 'Your changes can affect to the permissions of selected users.  Are you sure you want to continue?',
                type: 'alert',
                speed: 500,
                timeout: false,
                closeButton: false,
                closeOnSelfClick: false,
                closeOnSelfOver: false,
                force: true,
                onShow: false,
                onShown: false,
                onClose: false,
                onClosed: false,
                buttons: [
                    {
                        type: 'btn-primary', text: 'Yes', click: function ($noty) {
                            $noty.close();
                            //__doPostBack("ctl00$ContentPlaceHolder1$ddlApplyUserRolePermission", "ddlApplyUserRolePermissionchange");
                            return true;
                        }
                    },
                    {
                        type: 'btn-danger', text: 'No', click: function ($noty) {
                            //event.preventDefault();
                            $noty.close();
                            //__doPostBack("ctl00$ContentPlaceHolder1$ddlUserRole", "ddlUserRolechange");
                            return false;
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
            //eturn false;
        }
        function pageLoad(sender, args) {
            SelectRowsUser();

            $("#<%=RadGrid_Users.ClientID%> input[id*='chkAll']:checkbox").click(function () {
                debugger;
                if ($(this).is(':checked')) {
                    EqCheckBOX(true);
                }
                else {
                    EqCheckBOX(false);
                }
                SelectRowsUser();
            });

            Materialize.updateTextFields();
        }
    </script>
</asp:Content>
